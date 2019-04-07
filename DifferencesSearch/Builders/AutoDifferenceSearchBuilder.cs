using DifferencesSearch.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DifferencesSearch.Builders
{
    public interface IAutoDifferenceSearchBuilder<TSource> : IDifferenceSearchBuilder<TSource>
    {
        IAutoDifferenceSearchBuilder<TSource> Stop<TProp>(Expression<Func<TSource, TProp>> expression) where TProp : class;
        IAutoDifferenceSearchBuilder<TSource> Ignore<TProp>(Expression<Func<TSource, TProp>> expression);
    }

    public class AutoDifferenceSearchBuilder<TSource> : IAutoDifferenceSearchBuilder<TSource>
    {
        private BuildTreeNode _buildRootNode;

        public PropertiesTreeNode PropertyTree { get; private set; }

        public AutoDifferenceSearchBuilder()
        {
            PropertyTree = null;
            _buildRootNode = new BuildTreeNode(typeof(TSource));
        }

        public void Build()
        {
            PropertiesTreeNode root = new PropertiesTreeNode(typeof(TSource));

            DepthBuild(_buildRootNode, root);

            PropertyTree = root;
        }

        private void DepthBuild(BuildTreeNode buildNode, PropertiesTreeNode newNode)
        {
            PropertyInfo[] allTypeProperties = buildNode.PropertyType.GetProperties();

            // Исключаем все игнорируемые поля.
            PropertyInfo[] neededProperties = allTypeProperties
                .Where(info => !buildNode.IgnoredProperties.Contains(info.Name)).ToArray();

            // Создаём другие node дерева.
            List<PropertiesTreeNode> newNodes = new List<PropertiesTreeNode>();
            foreach (var info in neededProperties)
            {
                // Если в build версии дерева есть node, тогда проходим по его правилам.
                if (buildNode.ChildNodes.ContainsKey(info.Name))
                {
                    if (buildNode.ChildNodes[info.Name].IsDeadEnd)
                        continue;

                    var newInnerNode = new PropertiesTreeNode(info);
                    DepthBuild(buildNode.ChildNodes[info.Name], newInnerNode);
                    newNodes.Add(newInnerNode);
                }
                // Если в build версии дерева нету node, тогда запускаем полностью рефлексивный обход.
                else if (!info.PropertyType.IsSimple())
                {
                    var newInnerNode = new PropertiesTreeNode(info);
                    DepthBuild(info.PropertyType, newInnerNode);
                    newNodes.Add(newInnerNode);
                }
            }

            newNode.InnerProperties = neededProperties;
            newNode.Nodes = newNodes.ToArray();
        }

        // TODO Надо добавить Hashset<Type> чтобы отслеживать рекурсии и на них реагировать эксепшеном.
        private void DepthBuild(Type type, PropertiesTreeNode newNode)
        {
            PropertyInfo[] allTypeProperties = type.GetProperties();

            if (allTypeProperties.Any(x => x.PropertyType == type))
                throw new ArgumentException($"Property type matched class type. Stack overflow is inevitable. Type: '{type}'");

            List<PropertiesTreeNode> newNodes = new List<PropertiesTreeNode>();
            foreach (var info in allTypeProperties)
            {
                if (!info.PropertyType.IsSimple())
                {
                    var newInnerNode = new PropertiesTreeNode(info);
                    DepthBuild(info.PropertyType, newInnerNode);
                    newNodes.Add(newInnerNode);
                }
            }

            newNode.InnerProperties = allTypeProperties;
            newNode.Nodes = newNodes.ToArray();
        }

        public IAutoDifferenceSearchBuilder<TSource> Ignore<TProp>(Expression<Func<TSource, TProp>> expression)
        {
            _buildRootNode.AddExpressionToTree(expression, (node, property) => node.IgnoredProperties.Add(property.Name));
            return this;
        }

        public IAutoDifferenceSearchBuilder<TSource> Stop<TProp>(Expression<Func<TSource, TProp>> expression) where TProp : class
        {
            _buildRootNode.AddExpressionToTree(expression, (node, property) => node.IsDeadEnd = true);
            return this;
        }
    }
}

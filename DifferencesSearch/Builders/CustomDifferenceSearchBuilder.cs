using DifferencesSearch.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DifferencesSearch.Builders
{
    public interface ICustomDifferenceSearchBuilder<TSource> : IDifferenceSearchBuilder<TSource>
    {
        ICustomDifferenceSearchBuilder<TSource> One<TProp>(Expression<Func<TSource, TProp>> expression);
        ICustomDifferenceSearchBuilder<TSource> All<TProp>(Expression<Func<TSource, TProp>> expression) where TProp : class;
    }

    public class CustomDifferenceSearchBuilder<TSource> : ICustomDifferenceSearchBuilder<TSource>
    {
        private BuildTreeNode _buildRootNode;

        public PropertiesTreeNode PropertyTree { get; private set; }

        public CustomDifferenceSearchBuilder()
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
            // Создаём другие node дерева.
            List<PropertiesTreeNode> newNodes = new List<PropertiesTreeNode>();
            foreach (var nodePair in buildNode.ChildNodes)
            {
                string propName = nodePair.Key;
                if (buildNode.IgnoredProperties.Contains(propName))
                    continue;

                var newInnerNode = new PropertiesTreeNode(nodePair.Value.Property);
                DepthBuild(nodePair.Value, newInnerNode);
                newNodes.Add(newInnerNode);
            }

            newNode.InnerProperties = buildNode.NeedAllProperties
                ? buildNode.PropertyType.GetProperties()
                : buildNode.SelectedProperties.ToArray();
            newNode.Nodes = newNodes.ToArray();
        }

        public ICustomDifferenceSearchBuilder<TSource> One<TProp>(Expression<Func<TSource, TProp>> expression)
        {
            _buildRootNode.AddExpressionToTree(expression, (node, property) => node.SelectedProperties.Add(property));
            return this;
        }

        public ICustomDifferenceSearchBuilder<TSource> All<TProp>(Expression<Func<TSource, TProp>> expression) where TProp : class
        {
            _buildRootNode.AddExpressionToTree(expression, (node, property) => node.NeedAllProperties = true);
            return this;
        }
    }
}

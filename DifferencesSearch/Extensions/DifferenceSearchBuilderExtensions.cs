using DifferencesSearch.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DifferencesSearch.Extensions
{
    internal static class DifferenceSearchBuilderExtensions
    {
        public static void AddExpressionToTree<TSource, TProp>(this BuildTreeNode rootNode, Expression<Func<TSource, TProp>> expression, Action<BuildTreeNode, PropertyInfo> action)
        {
            Stack<PropertyInfo> propertiesPath = new Stack<PropertyInfo>();

            // Записываем весь путь в стек.
            MemberExpression currentStep = expression.Body as MemberExpression;
            if (currentStep != null)
            {
                while (currentStep is MemberExpression)
                {
                    if (!(currentStep.Member is PropertyInfo))
                        continue;
                    propertiesPath.Push(currentStep.Member as PropertyInfo);
                    currentStep = currentStep.Expression as MemberExpression;
                }
            }

            // Проходим по стеку и достраиваем дерево.
            BuildTreeNode node = rootNode;
            PropertyInfo pathPart = null;
            while (propertiesPath.Count != 0)
            {
                pathPart = propertiesPath.Pop();

                if (!pathPart.PropertyType.IsSimple())
                {
                    if (!node.ChildNodes.ContainsKey(pathPart.Name))
                        node.ChildNodes[pathPart.Name] = new BuildTreeNode(pathPart);
                    node = node.ChildNodes[pathPart.Name];
                }
            }

            action(node, pathPart);
        }

        public static bool IsSimple(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return IsSimple(type.GetGenericArguments()[0]);

            return type.IsEnum
              || type.IsPrimitive
              || type.Equals(typeof(string))
              || type.Equals(typeof(DateTime))
              || type.Equals(typeof(decimal));
        }
    }
}

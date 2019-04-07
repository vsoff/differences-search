using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DifferencesSearch
{
    public class PropertiesTreeNode
    {
        public Type PropertyType { get; }
        public string PropertyName { get; }
        public PropertyInfo Property { get; }
        public PropertyInfo[] InnerProperties { get; set; }
        public PropertiesTreeNode[] Nodes { get; set; }

        public PropertiesTreeNode(Type propertyType)
        {
            PropertyType = propertyType;
        }

        public PropertiesTreeNode(PropertyInfo property)
        {
            Property = property;
            PropertyName = property.Name;
            PropertyType = property.PropertyType;
        }
    }

    internal class BuildTreeNode
    {
        public Type PropertyType { get; }
        public string PropertyName { get; }
        public PropertyInfo Property { get; }
        public Dictionary<string, BuildTreeNode> ChildNodes { get; }

        #region Custom search
        public HashSet<PropertyInfo> SelectedProperties { get; }
        public HashSet<string> GoDepthProperties { get; }
        public bool NeedAllProperties { get; set; }
        #endregion

        #region Auto search
        public HashSet<string> IgnoredProperties { get; }
        public bool IsDeadEnd { get; set; }
        #endregion

        public BuildTreeNode(PropertyInfo property) : this(property.PropertyType)
        {
            Property = property;
            PropertyName = property.Name;
        }

        public BuildTreeNode(Type propertyType)
        {
            PropertyType = propertyType;
            PropertyName = null;
            ChildNodes = new Dictionary<string, BuildTreeNode>();
            SelectedProperties = new HashSet<PropertyInfo>();
            GoDepthProperties = new HashSet<string>();
            IgnoredProperties = new HashSet<string>();
            NeedAllProperties = false;
            IsDeadEnd = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DifferencesSearch
{
    public interface IDifferenceController
    {
        IAutoDifferenceSearchBuilder<T> SetAutoBuilder<T>();
        ICustomDifferenceSearchBuilder<T> SetCustomBuilder<T>();
        PropertyDifference[] GetAutoDifferences<T>(T firstValue, T secondValue);
        PropertyDifference[] GetCustomDifferences<T>(T firstValue, T secondValue);
    }

    public interface IDifferenceSearchBuilder<TSource>
    {
        void Clear();
        void Build();
        IAutoDifferenceSearchBuilder<TSource> Stop<TProp>(Expression<Func<TSource, TProp>> expression);
    }

    public interface IAutoDifferenceSearchBuilder<TSource> : IDifferenceSearchBuilder<TSource>
    {
        IAutoDifferenceSearchBuilder<TSource> Ignore<TProp>(Expression<Func<TSource, TProp>> expression);
    }

    public interface ICustomDifferenceSearchBuilder<TSource> : IDifferenceSearchBuilder<TSource>
    {
        IAutoDifferenceSearchBuilder<TSource> Trace<TProp>(Expression<Func<TSource, TProp>> expression);
    }

    internal class PropertyTreeNode
    {

    }
}

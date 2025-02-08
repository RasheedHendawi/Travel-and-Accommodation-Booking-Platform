using System;
using System.Linq.Expressions;

namespace Application.Helper
{
    public static class ExpressionCombiner
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            if (first == null) return second;
            if (second == null) return first;

            var parameter = Expression.Parameter(typeof(T));

            var combined = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    Expression.Invoke(first, parameter),
                    Expression.Invoke(second, parameter)
                ),
                parameter
            );

            return combined;
        }
    }

}

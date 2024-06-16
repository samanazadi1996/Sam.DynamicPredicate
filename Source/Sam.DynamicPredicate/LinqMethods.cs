using System.Collections.Generic;
using System.Linq;

namespace Sam.DynamicPredicate
{
    public static class LinqMethods
    {
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, string predicate)
        {
            var expression = PredicateBuilder.Build<TSource>(predicate);

            return source.Where(expression);
        }
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, string predicate)
        {
            var expression = PredicateBuilder.Build<TSource>(predicate);

            return source.Where(expression.Compile());
        }
    }
}

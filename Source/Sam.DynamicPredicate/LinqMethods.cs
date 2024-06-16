using System.Linq;

namespace Sam.DynamicPredicate
{
    public static class LinqMethods
    {
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, string predicate)
        {
            var expression = PredicateBuilder.Compile<TSource>(predicate);

            return source.Where(expression);
        }
    }
}

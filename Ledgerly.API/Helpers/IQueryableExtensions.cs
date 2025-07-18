using System.Linq.Expressions;

namespace Ledgerly.API.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string property, bool descending)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.PropertyOrField(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            var methodName = descending ? "OrderByDescending" : "OrderBy";

            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), propertyAccess.Type);

            return (IQueryable<T>)method.Invoke(null, new object[] { source, orderByExp })!;
        }
    }
}

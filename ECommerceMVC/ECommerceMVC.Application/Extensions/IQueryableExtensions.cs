using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string propertyName, string sortDirection)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return source;
            }

            // Tạo biểu thức cho thuộc tính
            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            // Chọn phương thức sắp xếp dựa trên hướng sắp xếp
            var methodName = sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase) ? "OrderBy" : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
                .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
                .Single()
                .MakeGenericMethod(typeof(T), property.Type);

            // Thực hiện sắp xếp
            return (IQueryable<T>)method.Invoke(null, new object[] { source, lambda });
        }
    }
}

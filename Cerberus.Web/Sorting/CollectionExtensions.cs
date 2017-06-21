using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cerberus.Web.Sorting
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> collection, string columnName, SortDirection direction)
        {
            Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> expression =  SortExpressionConverter<T>.Convert(direction);

            Func<T, object> lambda =  SortLambdaBuilder<T>.Build(columnName, direction);

            IEnumerable<T> sorted = expression(collection, lambda);

            return sorted;
        }
    }
}
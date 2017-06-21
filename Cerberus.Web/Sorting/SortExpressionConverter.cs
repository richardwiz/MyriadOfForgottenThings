using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cerberus.Web.Sorting
{
    public static class SortExpressionConverter<T>
    {
        private static IDictionary<SortDirection, ISortExpression> expressions =
            new Dictionary<SortDirection, ISortExpression>
        {
        { SortDirection.Ascending, new OrderByAscendingSortExpression() },
        { SortDirection.Descending, new OrderByDescendingSortExpression() }
        };

        interface ISortExpression
        {
            Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> GetExpression();
        }

        class OrderByAscendingSortExpression : ISortExpression
        {
            public Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> GetExpression()
            {
                return (c, f) => c.OrderBy(f);
            }
        }

        class OrderByDescendingSortExpression : ISortExpression
        {
            public Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> GetExpression()
            {
                return (c, f) => c.OrderByDescending(f);
            }
        }

        public static Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>>
            Convert(SortDirection direction)
        {
            ISortExpression sortExpression = expressions[direction];
            return sortExpression.GetExpression();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cerberus.Web.Sorting
{
    public static class SortLambdaBuilder<T>
    {
        public static Func<T, object> Build(string columnName, SortDirection direction)
        {
            // x
            ParameterExpression param = Expression.Parameter(typeof(T), "x");

            // x.ColumnName1.ColumnName2
            Expression property = columnName.Split('.') .Aggregate<string, Expression>  (param, (c, m) => Expression.Property(c, m));

            // x => x.ColumnName1.ColumnName2
            Expression<Func<T, object>> lambda = Expression.Lambda<Func<T, object>>( Expression.Convert(property, typeof(object)), param);

            Func<T, object> func = lambda.Compile();
            return func;
        }
    }
}
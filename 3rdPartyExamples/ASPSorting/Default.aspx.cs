using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GridViewOnSorting
{
    public partial class _Default : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            IEnumerable<Person> persons = GetPersons();
            
            gridPersons.DataSource = persons.ToArray();
            gridPersons.DataBind();
        }

        private static IEnumerable<Person> GetPersons()
        {
            Person p = new Person { FirstName = "Darth", LastName = "Wader", Father = new Person() };
            yield return p;

            yield return new Person { FirstName = "Luke", LastName = "Skywalker", Father = p };
            yield return new Person { FirstName = "Leia", LastName = "Organa Solo", Father = p };
        }

        protected void gridPersons_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<Person> persons = GetPersons();
            persons = persons.OrderBy(e.SortExpression, e.SortDirection);

            gridPersons.DataSource = persons.ToArray();
            gridPersons.DataBind();
        }
    }

    class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Person Father { get; set; }
    }

    public static class SortExpressionConverter<T>
    {
        private static IDictionary<SortDirection, ISortExpression> expressions = new Dictionary<SortDirection, ISortExpression>
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

        public static Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> Convert(SortDirection direction)
        {
            ISortExpression sortExpression = expressions[direction];
            return sortExpression.GetExpression();
        }
    }

    public static class SortLambdaBuilder<T>
    {
        public static Func<T, object> Build(string columnName, SortDirection direction)
        {
            // x
            ParameterExpression param = Expression.Parameter(typeof(T), "x");

            // x.ColumnName1.ColumnName2
            Expression property = columnName.Split('.')
                                            .Aggregate<string, Expression>(param, (c, m) => Expression.Property(c, m));

            // x => x.ColumnName1.ColumnName2
            Expression<Func<T, object>> lambda = Expression.Lambda<Func<T, object>>(
                Expression.Convert(property, typeof(object)),
                param);

            Func<T, object> func = lambda.Compile();
            return func;
        }
    }

    public static class CollectionExtensions
    {
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> collection,
               string columnName, SortDirection direction)
        {
            Func<IEnumerable<T>, Func<T, object>, IEnumerable<T>> expression =
                SortExpressionConverter<T>.Convert(direction);

            Func<T, object> lambda = SortLambdaBuilder<T>.Build(columnName, direction);

            IEnumerable<T> sorted = expression(collection, lambda);
            return sorted;
        }
    }
}
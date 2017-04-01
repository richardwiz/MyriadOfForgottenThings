using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Tatts.NextGen.StatsData
{
    public static class FluentNHibernateHelper
    {
        public static ISession OpenSession()
        {
            string connectionString = @"Data Source=ta030633\SQL2K14DEV1;Initial Catalog=NextGenStatistics;User ID=NextGenDBUser;Password=NextGenDBUser!@;MultipleActiveResultSets=True;Max Pool Size=10000;Min Pool Size=300;Connection Timeout=5";

            ISessionFactory sessionFactory = Fluently.Configure().Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString).ShowSql())
                .Mappings(m =>
                    {
                        m.FluentMappings.AddFromAssemblyOf<FixturesMap>();
                        m.FluentMappings.AddFromAssemblyOf<MergeDataMap>();
                    })
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(false, false))
                .BuildSessionFactory();

            return sessionFactory.OpenSession();
        }
    }
}

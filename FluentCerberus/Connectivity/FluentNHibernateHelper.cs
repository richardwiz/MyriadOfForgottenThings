using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using FluentCerberus;
using System.Configuration;

namespace FluentCerberus.Connectivity
{
    public static class FluentNHibernateHelper
    {

        public static ISession OpenCerberusSession()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Cerberus"].ToString();
            return OpenSession(connectionString);
        }

        public static ISession OpenEisaSession()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Eisa"].ToString();
            return OpenSession(connectionString);
        }

        private static ISession OpenSession(string connection)
        {

            ISessionFactory sessionFactory = Fluently.Configure().Database(MsSqlConfiguration.MsSql2012.ConnectionString(connection).ShowSql())
                .Mappings(m => m.FluentMappings
                .AddFromAssemblyOf<EFTTerminalAudit>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(false, false))
                .BuildSessionFactory();

            return sessionFactory.OpenSession();
        }

    }
}

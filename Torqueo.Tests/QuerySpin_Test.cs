using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NHibernate;
using NHibernate.Linq;
using Torqueo.Connectivity;

namespace Torqueo.Tests
{
    [TestFixture]
    public class QuerySpin_Test
    {
        [Test]
        public void SpinJson_Query()
        {
            using (ISession session = FluentNHibernateHelper.OpenSession())
            {
                var json = session.Query<SpinJson>().ToList();
                Assert.IsNotNull(json);
            }
        }

        [Test]
        public void SpinForsetiMapping_Query()
        {
            using (ISession session = FluentNHibernateHelper.OpenSession())
            {
                var s2f = session.Query<SpinForsetiMapping>().Take(100).ToList();
                Assert.IsNotNull(s2f);
            }
        }
    }
}

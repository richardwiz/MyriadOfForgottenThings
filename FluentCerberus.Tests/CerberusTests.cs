using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cerberus;
using NUnit.Framework;
using NHibernate;
using FluentCerberus.Connectivity;
using NHibernate.Linq;

namespace FluentCerberus.Tests
{
    [TestFixture]
    public class CerberusTests
    {
        [Test]
        public void AddEFTTerminal_Test ()
        {
            EFTTerminalAudit eftta = new EFTTerminalAudit();
            eftta.FirstVerified = DateTime.Now;
            eftta.LastVerified = DateTime.Now;
            eftta.Make = "Thismake";
            eftta.MerchantId = 12345;
            eftta.Model = "44";
            eftta.OfficeNo = 1701;
            eftta.StationNo = 1;
            eftta.SWVersion = "asd1564";
            eftta.TerminalId = "sdfgsdfg";

            using (ISession session = FluentNHibernateHelper.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Save(eftta);

                    var eft = session.Query<EFTTerminalAudit>().ToList();
                    Assert.IsNotNull(eft);
                }
            }
        }
    }
}

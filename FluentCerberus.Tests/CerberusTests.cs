using Cerberus;
using Cerberus.Library;
using FluentCerberus.Connectivity;
using NHibernate;
using NHibernate.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace FluentCerberus.Tests
{
    [TestFixture]
    public class CerberusTests
    {
        static String _cerberusConnection;
        static String _eisaConnection;

        [SetUp]
        public void Init()
        {
            _eisaConnection = ConfigurationManager.ConnectionStrings["Eisa"].ToString();
            _cerberusConnection = ConfigurationManager.ConnectionStrings["Cerberus"].ToString();
        }

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
            eftta.PinPadId = 123457;
            using (ISession session = FluentNHibernateHelper.OpenSession(_cerberusConnection))
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Save(eftta);
                    txn.Commit();
                    var eft = session.Query<EFTTerminalAudit>().ToList();
                    Assert.IsNotNull(eft);
                }
            }
        }

        [Test]
        public void UpdateEFTTerminal_Test()
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
            eftta.PinPadId = 123456;
            using (ISession session = FluentNHibernateHelper.OpenSession(_cerberusConnection))
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Save(eftta);
                    txn.Commit();
                    var eft = session.Query<EFTTerminalAudit>().ToList();
                    Assert.IsNotNull(eft);
                }
            }
        }

        [Test]
        public void GetEFTTransInfo_Query()
        {
            using (ISession session = FluentNHibernateHelper.OpenSession(_eisaConnection))
            {
                using (var txn = session.BeginTransaction())
                {
                    var eft = session.Query<EFTTransactionInfo>().Select(x => x.SerialNo).ToList();
                    Assert.IsNotNull(eft);
                }
            }
        }

        [Test]
        public void GetGetKnownEftTerminals_Query()
        {
            using (ISession session = FluentNHibernateHelper.OpenSession(_cerberusConnection))
            {
                using (var txn = session.BeginTransaction())
                {
                    var eft = session.Query<EFTTerminalAudit>().ToList();
                    Assert.IsNotNull(eft);
                }
            }
        }

        [Test]
        public void IsKnownTerminal_Test()
        {
            // Known Id = 123456
            bool exists = CerberusTools.IsKnownTerminal(123456, _eisaConnection);
            Assert.IsTrue(exists);

            // Unknown Id = 987654
            exists = CerberusTools.IsKnownTerminal(987654, _eisaConnection);
            Assert.IsFalse(exists);

        }

        [Test]
        public void SendEmail_Test()
        {
            List<EFTTerminalAudit> newTerminals = new List<EFTTerminalAudit>();
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
            eftta.PinPadId = 123457;
            newTerminals.Add(eftta);

            bool sent = CerberusTools.EmailNewTerminalsInfo(newTerminals
                , ConfigurationManager.AppSettings["RecipientList"].ToString().Split(new char[] { ',' }).ToList()
                , ConfigurationManager.AppSettings["MailHost"].ToString()
                , ConfigurationManager.AppSettings["FromAddress"].ToString());
            Assert.IsTrue(sent);
        }
    }
}

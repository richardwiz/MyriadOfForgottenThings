using Cerberus;
using FluentCerberus.Connectivity;
using NHibernate;
using NHibernate.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
            eftta.PinPadId = 123457;
            using (ISession session = FluentNHibernateHelper.OpenCerberusSession())
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
            using (ISession session = FluentNHibernateHelper.OpenCerberusSession())
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
            using (ISession session = FluentNHibernateHelper.OpenEisaSession())
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
            using (ISession session = FluentNHibernateHelper.OpenCerberusSession())
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
            bool exists = CerberusTools.IsKnownTerminal(123456);
            Assert.IsTrue(exists);

            // Unknown Id = 987654
            exists = CerberusTools.IsKnownTerminal(987654);
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

            bool sent = CerberusTools.EmailNewTerminalsInfo(newTerminals);
            Assert.IsTrue(sent);
        }
    }
}

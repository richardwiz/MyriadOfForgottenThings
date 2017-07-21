﻿using FluentCerberus;
using FluentCerberus.Connectivity;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Library
{
    public static class CerberusTools
    {
        private const String _PT11Start = "11";
        private const String _QT720Start = "72";

        #region Fluent Nhibernate Queries
        public static List<EFTTerminalAudit> GetTerminalByPinPadId(Int64 pinPadId, String connection)
        {
            using (ISession session = FluentNHibernateHelper.OpenSession(connection))
            {
                using (var txn = session.BeginTransaction())
                {
                    return session.Query<EFTTerminalAudit>()
                        .Where(x => x.PinPadId == pinPadId).ToList();
                }
            }
        }

        public static bool IsKnownAndMovedTerminal(Int64 pinPadId, Int32 officeNo, String connection)
        {
            using (ISession session = FluentNHibernateHelper.OpenSession(connection))
            {
                using (var txn = session.BeginTransaction())
                {
                    var id = session.Query<EFTTerminalAudit>()
                        .Where(x => x.PinPadId == pinPadId && x.OfficeNo != officeNo)
                        .Select(x => x.PinPadId).FirstOrDefault();
                    return id > 0;
                }
            }
        }

        public static List<EFTTerminalAudit> GetKnownEftTerminals(String connection)
        {
            using (ISession session = FluentNHibernateHelper.OpenSession(connection))
            {
                using (var txn = session.BeginTransaction())
                {
                    return session.Query<EFTTerminalAudit>().ToList();
                }
            }
        }

        public static List<Int64> FindSerialNos(String connection)
        {
            // Load _ids
            using (ISession session = FluentNHibernateHelper.OpenSession(connection))
            {
                using (var txn = session.BeginTransaction())
                {
                    // Modify this to be for a date (range?)
                    return session.Query<EFTTransactionInfo>().Select(x => x.SerialNo).ToList();
                }
            }
        }

        public static void AddEftTerminal(EFTTerminalAudit eftta, String connection)
        {
            // 5: ADD to the database
            using (ISession session = FluentNHibernateHelper.OpenSession(connection))
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Save(eftta);
                    txn.Commit();
                }
            }

        }

        #endregion

        #region Email Helper
        public static bool EmailNewTerminalsInfo(List<EFTTerminalAudit> newTerminals
            , List<EFTTerminalAudit> movedTerminals
            , List<String> recipients
            , String host
            , String from)
        {
            
            StringBuilder sb = new StringBuilder();
            if (newTerminals.Count > 0)
            {
                sb.AppendFormat("The following new Terminals have been added.\n");
                foreach (EFTTerminalAudit eftInfo in newTerminals)
                {
                    sb.AppendFormat("\n", eftInfo.ToString());
                    sb.AppendFormat("{0}", eftInfo.ToString());
                }
            }

            if (movedTerminals.Count > 0)
            {
                sb.AppendFormat("\n\nThe following new Terminals have Moved Offices.\n");
                foreach (EFTTerminalAudit eftInfo in movedTerminals)
                {
                    sb.AppendFormat("\n", eftInfo.ToString());
                    sb.AppendFormat("{0}", eftInfo.ToString());
                }
            }
            recipients.AddRange(recipients);

            MailMan mailer = new MailMan(host
                , "New EFT Terminals Have Been Added or Moved"
                , from
                , recipients
                , true);
            mailer.Send(sb.ToString());

            // To Fix
            return true;
        }

        #endregion

        #region Checking and Validation
        public static bool HasTerminalMoved(List<EFTTerminalAudit> existingTerminals, EFTTerminalAudit eftta)
        {
            return (existingTerminals.Select(x => x.PinPadId == eftta.PinPadId && x.OfficeNo == eftta.OfficeNo).ToList().Count) == 0;
        }

        public static void GetEFTMakeAndModel(long pinPadId, out string make, out string model)
        {
            if (pinPadId.ToString().StartsWith(_PT11Start))
            {
                make = "QUEST";
                model = "Swift PT11";
            }
            else if (pinPadId.ToString().StartsWith(_QT720Start))
            {
                make = "QUEST";
                model = "QT720";
            }
            else
            {
                make = "Make";
                model = "Model";
            }
        }
        #endregion


    }
}

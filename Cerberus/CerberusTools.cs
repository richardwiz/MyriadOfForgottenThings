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

namespace Cerberus
{
    public static class CerberusTools
    {
        public static bool IsKnownTerminal(Int64 pinPadId)
        {
            using (ISession session = FluentNHibernateHelper.OpenCerberusSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    var id = session.Query<EFTTerminalAudit>()
                        .Where(x => x.PinPadId == pinPadId)
                        .Select(x => x.PinPadId).FirstOrDefault();
                    return id > 0;
                }
            }
        }
        internal static List<EFTTerminalAudit> GetKnownEftTerminals()
        {
            using (ISession session = FluentNHibernateHelper.OpenCerberusSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    return session.Query<EFTTerminalAudit>().ToList();
                }
            }
        }

        public static bool EmailNewTerminalsInfo(List<EFTTerminalAudit> newTerminals)
        {
            
            StringBuilder sb = new StringBuilder("The following new Terminals have been added.\n");
            foreach (EFTTerminalAudit eftInfo in newTerminals)
            {
                sb.AppendFormat("\n", eftInfo.ToString());
                sb.AppendFormat("{0}", eftInfo.ToString());
            }
            List<String> recipients = new List<string>();
            recipients.Add(ConfigurationManager.AppSettings["RecipientList"].ToString());

            MailMan mailer = new MailMan(ConfigurationManager.AppSettings["MailHost"].ToString()
                , "New EFT Terminals Have Been Added"
                , ConfigurationManager.AppSettings["MailHost"].ToString()
                , recipients
                , true);
            mailer.Send(sb.ToString());

            // To Fix
            return true;
        }
    }
}

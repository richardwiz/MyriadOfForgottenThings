using FluentCerberus;
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
        public static bool IsKnownTerminal(Int64 pinPadId, String connection)
        {
            using (ISession session = FluentNHibernateHelper.OpenSession(connection))
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
        internal static List<EFTTerminalAudit> GetKnownEftTerminals(String connection)
        {
            using (ISession session = FluentNHibernateHelper.OpenSession(connection))
            {
                using (var txn = session.BeginTransaction())
                {
                    return session.Query<EFTTerminalAudit>().ToList();
                }
            }
        }

        public static bool EmailNewTerminalsInfo(List<EFTTerminalAudit> newTerminals
            , List<String> recipients
            , String host
            , String from)
        {
            
            StringBuilder sb = new StringBuilder("The following new Terminals have been added.\n");
            foreach (EFTTerminalAudit eftInfo in newTerminals)
            {
                sb.AppendFormat("\n", eftInfo.ToString());
                sb.AppendFormat("{0}", eftInfo.ToString());
            }
            recipients.AddRange(recipients);

            MailMan mailer = new MailMan(host
                , "New EFT Terminals Have Been Added"
                , from
                , recipients
                , true);
            mailer.Send(sb.ToString());

            // To Fix
            return true;
        }

        public static List<Int64> FindSerialNos(String connection)
        {
            // Load _ids
            using (ISession session = FluentNHibernateHelper.OpenSession(connection))
            {
                using (var txn = session.BeginTransaction())
                {
                    return session.Query<EFTTransactionInfo>().Select(x => x.SerialNo).ToList();
                }
            }
        }

    }
}

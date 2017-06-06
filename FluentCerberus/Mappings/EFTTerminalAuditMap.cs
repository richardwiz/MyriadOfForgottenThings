using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace FluentCerberus.Mappings
{
    public class EFTTerminalAuditMap : ClassMap<EFTTerminalAudit>
    {
        public EFTTerminalAuditMap()
        {
            Id(x => x.PinPadId).GeneratedBy.Assigned();
            Map(x => x.FirstVerified);
            Map(x => x.LastVerified);
            Map(x => x.Make);
            Map(x => x.MerchantId);
            Map(x => x.Model);
            Map(x => x.OfficeNo);
            Map(x => x.StationNo);
            Map(x => x.SWVersion);
            Map(x => x.TerminalId);
            Table("EFTTerminalAudit");
        }
    }
}

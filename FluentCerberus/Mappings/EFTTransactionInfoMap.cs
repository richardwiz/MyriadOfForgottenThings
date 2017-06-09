using FluentNHibernate.Mapping;

namespace FluentCerberus.Mappings
{
    public class EFTTransactionInfoMap : ClassMap<EFTTransactionInfo>
    {
        public EFTTransactionInfoMap()
        {
            Id(x => x.SerialNo).GeneratedBy.Assigned();
            Map(x => x.TxnId);
            Map(x => x.TxnName);
            Map(x => x.TxnTime);
            Table("vw_EFTTransactionInfo");
        }

    }
}

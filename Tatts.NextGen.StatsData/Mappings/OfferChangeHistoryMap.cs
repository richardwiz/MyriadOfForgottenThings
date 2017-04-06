using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Tatts.NextGen.StatsData
{
    public class OfferChangeHistoryMap  : ClassMap<OfferChangeHistory>    
    {
        public OfferChangeHistoryMap()
        {
             Id(x => x.SerialNo);
             Map(x => x.ChangeId);
             Map(x => x.ChangerUserId);
             Map(x => x.ActionDesc);
             Map(x => x.SubEventId);
             Map(x => x.OfferIdOld);
             Map(x => x.OfferIdNew);
             Map(x => x.OfferNameOld);
             Map(x => x.OfferNameNew);
             Map(x => x.StatusOld);
             Map(x => x.StatusNew);
             Map(x => x.WWRetailReturnOld);
             Map(x => x.WWRetailReturnNew);
             Map(x => x.WWInternetReturnOld);
             Map(x => x.WWInternetReturnNew);
             Map(x => x.WWTelebetReturnOld);
             Map(x => x.WWTelebetReturnNew);
             Map(x => x.PPRetailReturnOld);
             Map(x => x.PPRetailReturnNew);
             Map(x => x.PPInternetReturnOld);
             Map(x => x.PPInternetReturnNew);
             Map(x => x.PPTelebetReturnOld);
             Map(x => x.PPTelebetReturnNew);
             Map(x => x.Value1Old);
             Map(x => x.Value1New);
             Map(x => x.Value2Old);
             Map(x => x.Value2New);
             Map(x => x.RiskLimitOld);
             Map(x => x.RiskLimitNew);
            Table("OfferChangeHistory");
        }
    }
}

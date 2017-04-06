using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatts.NextGen.StatsData
{
    public class OfferChangeHistory
    {
        public virtual Int64 SerialNo { get; protected set; }     
        public virtual Int32 ChangeId { get; protected set; }
        public virtual Int32 ChangerUserId { get; protected set; }
        public virtual String ActionDesc { get; protected set; }
        public virtual Int32 SubEventId { get; protected set; }
        public virtual Int32 OfferIdOld { get; protected set; }
        public virtual Int32 OfferIdNew { get; protected set; }
        public virtual String OfferNameOld { get; protected set; }
        public virtual String OfferNameNew { get; protected set; }
        public virtual String StatusOld { get; protected set; }
        public virtual String StatusNew { get; protected set; }
        public virtual Decimal WWRetailReturnOld { get; protected set; }
        public virtual Decimal WWRetailReturnNew { get; protected set; }
        public virtual Decimal WWInternetReturnOld { get; protected set; }
        public virtual Decimal WWInternetReturnNew { get; protected set; }
        public virtual Decimal WWTelebetReturnOld { get; protected set; }
        public virtual Decimal WWTelebetReturnNew { get; protected set; }
        public virtual Decimal PPRetailReturnOld { get; protected set; }
        public virtual Decimal PPRetailReturnNew { get; protected set; }
        public virtual Decimal PPInternetReturnOld { get; protected set; }
        public virtual Decimal PPInternetReturnNew { get; protected set; }
        public virtual Decimal PPTelebetReturnOld { get; protected set; }
        public virtual Decimal PPTelebetReturnNew { get; protected set; }
        public virtual Decimal Value1Old { get; protected set; }
        public virtual Decimal Value1New { get; protected set; }
        public virtual Decimal Value2Old { get; protected set; }
        public virtual Decimal Value2New { get; protected set; }
        public virtual Decimal RiskLimitOld { get; protected set; }
        public virtual Decimal RiskLimitNew { get; protected set; }

        public OfferChangeHistory()
        {
            this.SerialNo = 0;
            this.ChangeId = 0;
            this.ChangerUserId = 0;
            this.ActionDesc = String.Empty;
            this.SubEventId = 0;
            this.OfferIdOld = 0;
            this.OfferIdNew = 0;
            this.OfferNameOld = String.Empty;
            this.OfferNameNew = String.Empty;
            this.StatusOld = String.Empty;
            this.StatusNew = String.Empty;
            this.WWRetailReturnOld = 0.0M;
            this.WWRetailReturnNew = 0.0M;
            this.WWInternetReturnOld = 0.0M;
            this.WWInternetReturnNew = 0.0M;
            this.WWTelebetReturnOld = 0.0M;
            this.WWTelebetReturnNew = 0.0M;
            this.PPRetailReturnOld = 0.0M;
            this.PPRetailReturnNew = 0.0M;
            this.PPInternetReturnOld = 0.0M;
            this.PPInternetReturnNew = 0.0M;
            this.PPTelebetReturnOld = 0.0M;
            this.PPTelebetReturnNew = 0.0M;
            this.Value1Old = 0.0M;
            this.Value1New = 0.0M;
            this.Value2Old = 0.0M;
            this.Value2New = 0.0M;
            this.RiskLimitOld = 0.0M;
            this.RiskLimitNew = 0.0M;

        }

    }
}

using System;

namespace FluentCerberus
{
    /// <summary>
    /// CREATE TABLE [dbo].[EFTTerminalAudit](
    /// [PinPadId] [bigint] NOT NULL,
    /// [Make] [nvarchar](10) NULL,
    /// [Model] [nvarchar](10) NULL,
    /// [MerchantId] [bigint] NULL,
    /// [TerminalId][nvarchar](10) NOT NULL,
    /// [SWVersion] [nvarchar](16) NULL,
    /// [OfficeNo] [int] NULL,
    /// [StationNo] [int] NULL,
    /// [FirstVerified] [datetime] NULL,
    /// [LastVerified] [datetime] NULL,
    /// </summary>
    public class EFTTerminalAudit
    {
        public virtual Int64 PinPadId { get; set; }
        public virtual String Make { get; set; }
        public virtual String Model { get; set; }
        public virtual Int64 MerchantId { get; set; }
        public virtual String TerminalId { get; set; }
        public virtual String SWVersion { get; set; }
        public virtual int OfficeNo { get; set; }
        public virtual int StationNo { get; set; }
        public virtual DateTime FirstVerified { get; set; }
        public virtual DateTime LastVerified { get; set; }

        public EFTTerminalAudit()
        {
            this.PinPadId = 0;
            this.Make = String.Empty;
            this.Model = String.Empty;
            this.MerchantId = 0;
            this.TerminalId = String.Empty;
            this.SWVersion = String.Empty;
            this.OfficeNo = 0;
            this.StationNo = 0;
            this.FirstVerified = DateTime.MinValue;
            this.LastVerified = DateTime.MinValue;
        }

        public override string ToString()
        {
            return String.Format(@"PinPadId:{0}; MerchantId:{1}; TerminalId:{2}; OfficeNo:{3}; StationNo:{4}.",
                this.PinPadId
                , this.MerchantId
                , this.TerminalId
                , this.OfficeNo
                , this.StationNo);
        }
    }
}

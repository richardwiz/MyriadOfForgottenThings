using FluentCerberus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Admin
{
    public class EftTerminalAuditView : INotifyPropertyChanged
    {
        private EFTTerminalAudit _eftTerminal;

        public EFTTerminalAudit InnerEftTerminal { get {   return _eftTerminal;   } }

        public EftTerminalAuditView() : this(new EFTTerminalAudit()) { }

        public EftTerminalAuditView(EFTTerminalAudit eft)
        {
            _eftTerminal = eft;
        }

        #region Data Fields
        public virtual Int64 PinPadId
        {
            get { return _eftTerminal.PinPadId; }
            set
            {
                _eftTerminal.PinPadId  = value;
                NotifyPropertyChanged("PinPadId");
            }
        }
        public virtual String Make
        {
            get { return _eftTerminal.Make; }
            set
            {
                _eftTerminal.Make  = value;
                NotifyPropertyChanged("Make");
            }
        }
        public virtual String Model
        {
            get { return _eftTerminal.Model; }
            set
            {
                _eftTerminal.Model  = value;
                NotifyPropertyChanged("Model");
            }
        }
        public virtual Int64 MerchantId
        {
            get { return _eftTerminal.MerchantId; }
            set
            {
                _eftTerminal.MerchantId  = value;
                NotifyPropertyChanged("MerchantId");
            }
        }
        public virtual String TerminalId
        {
            get { return _eftTerminal.TerminalId; }
            set
            {
                _eftTerminal.TerminalId  = value;
                NotifyPropertyChanged("TerminalId");
            }
        }
        public virtual String SWVersion
        {
            get { return _eftTerminal.SWVersion; }
            set
            {
                _eftTerminal.SWVersion  = value;
                NotifyPropertyChanged("SWVersion");
            }
        }
        public virtual int OfficeNo
        {
            get { return _eftTerminal.OfficeNo; }
            set
            {
                _eftTerminal.OfficeNo  = value;
                NotifyPropertyChanged("OfficeNo");
            }
        }
        public virtual int StationNo
        {
            get { return _eftTerminal.StationNo; }
            set
            {
                _eftTerminal.StationNo  = value;
                NotifyPropertyChanged("StationNo");
            }
        }
        public virtual DateTime FirstVerified
        {
            get { return _eftTerminal.FirstVerified; }
            set
            {
                _eftTerminal.FirstVerified  = value;
                NotifyPropertyChanged("FirstVerified");
            }
        }
        public virtual DateTime LastVerified
        {
            get { return _eftTerminal.LastVerified; }
            set
            {
                _eftTerminal.LastVerified  = value;
                NotifyPropertyChanged("LastVerified");
            }
        }
        #endregion
        #region INotify Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyChanged)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyChanged));
        }

        #endregion
    }
}

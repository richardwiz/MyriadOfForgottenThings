using Cerberus.Admin;
using Cerberus.Library;
using FluentCerberus;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cerberus.Admin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EcObservableCollection<EftTerminalAuditView> _collection = new EcObservableCollection<EftTerminalAuditView>();
        static String _cerberusConnection;
        static String _eisaConnection;

        public MainWindow()
        {
            InitializeComponent();
            _eisaConnection = ConfigurationManager.ConnectionStrings["Eisa"].ToString();
            _cerberusConnection = ConfigurationManager.ConnectionStrings["Cerberus"].ToString();

            FillData();
            
            _collection.CollectionChanged +=  new NotifyCollectionChangedEventHandler(eft_CollectionChanged);
            _collection.ItemChanged +=
                new EcObservableCollection<EftTerminalAuditView>.EcObservableCollectionItemChangedEventHandler(eft_ItemChanged);
            dgrdEFT.ItemsSource = _collection;
        }

        #region Event Handling

        void eft_ItemChanged(object sender, EcObservableCollectionItemChangedEventArgs<EftTerminalAuditView> args)
        {
            // QUERY: At the moment this is a read only grid should it be ??
            //using (var session = sessionFactory.OpenSession())
            //{
            //    session.SaveOrUpdate(args.Item.InnerEftTerminal);
            //    session.Flush();
            //}
        }

        void eft_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // QUERY: At the moment this is a read only grid should it be ??
            //using (var session = sessionFactory.OpenSession())
            //{
            //    switch (e.Action)
            //    {
            //        case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
            //            foreach (CustomerView c in e.OldItems)
            //            {
            //                session.Delete(c.InnerCustomer);
            //                session.Flush();
            //            }
            //            break;
            //        case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
            //            foreach (CustomerView c in e.NewItems)
            //                session.Save(c.InnerCustomer);
            //            break;
            //        default:
            //            foreach (CustomerView c in e.OldItems)
            //                session.SaveOrUpdate(c.InnerCustomer);
            //            break;
            //    }
        }

        #endregion
        private void FillData()
        {
            //string ConString = ConfigurationManager.ConnectionStrings["Cerberus"].ConnectionString;
            //string CmdString = string.Empty;
            //using (SqlConnection con = new SqlConnection(ConString))
            //{
            //    CmdString = "SELECT * FROM EFTTerminalAudit";
            //    SqlCommand cmd = new SqlCommand(CmdString, con);
            //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //    DataTable dt = new DataTable("EFTTerminalAudit");
            //    sda.Fill(dt);
            //    dgrdEFT.ItemsSource = dt.DefaultView;
            //}

            List<EFTTerminalAudit> efts = CerberusTools.GetKnownEftTerminals(_cerberusConnection);

            foreach (var eft in efts)
            {
                _collection.Add(new EftTerminalAuditView
                {
                   PinPadId = eft.PinPadId
                   , FirstVerified = eft.FirstVerified
                   , LastVerified = eft.LastVerified
                   , Make = eft.Make
                   , MerchantId = eft.MerchantId
                   , Model = eft.Model
                   , OfficeNo = eft.OfficeNo
                   , StationNo = eft.StationNo
                   , SWVersion = eft.SWVersion
                   , TerminalId = eft.TerminalId 
                });
            }
        }

    }
}

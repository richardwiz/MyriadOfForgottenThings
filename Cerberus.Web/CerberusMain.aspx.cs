using Cerberus.Web.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;

namespace Cerberus.Web
{
    public partial class CerberusMain : System.Web.UI.Page
    {
        static String _cerberusConnection;
        static String _eisaConnection;
        static Int32 _newTerminalCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            _eisaConnection = ConfigurationManager.ConnectionStrings["Eisa"].ToString();
            _cerberusConnection = ConfigurationManager.ConnectionStrings["Cerberus"].ToString();
            _newTerminalCount = 0;

            FillData();

        }

        private void FillData()
        {
            // Get Data
            List<EFTTerminalAudit> efts = GetAllEftTerminals();
            // Bind Data
            gvEFT.DataSource = efts;
            _newTerminalCount = 0; // This needs to be zeroed before the data bind method as it is set then.
            gvEFT.DataBind();
            // Update Time Last Updated
            lblLastUpdated.Text = String.Format("Last Updated: {0}-{1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            gauNewTerminals.Value = efts.Count.ToString();
            gauNewTerminals.Value = _newTerminalCount.ToString();
        }

        #region Filters
        private void GetNewTerminalsFilter()
        {
            // Get Data
            List<EFTTerminalAudit> efts = GetNewTerminals();
            // Bind Data
            gvEFT.DataSource = efts;
            gvEFT.DataBind();
        }

        private void GetTerminalsByOfficeFilter(Int32 officeNo)
        {
            // Get Data
            List<EFTTerminalAudit> efts = GetTerminalsByOffice(officeNo);
            // Bind Data
            gvEFT.DataSource = efts;
            gvEFT.DataBind();
        }

        private void GetTerminalsByDateFilter(DateTime date)
        {
            // Get Data
            List<EFTTerminalAudit> efts = GetTerminalsByDate(date);
            // Bind Data
            gvEFT.DataSource = efts;
            gvEFT.DataBind();
        }

        private void GetTerminalsByDateRangeFilter(DateTime startDate, DateTime endDate)
        {
            // Get Data
            List<EFTTerminalAudit> efts = GetTerminalsByDateRange(startDate, endDate);
            // Bind Data
            gvEFT.DataSource = efts;
            gvEFT.DataBind();
        }

        #endregion

        #region Data Methods        

        private List<EFTTerminalAudit> GetAllEftTerminals()
        {
            CerberusDataContext db = new CerberusDataContext();
            List<EFTTerminalAudit> eftTerminals = db.EFTTerminalAudits.Select(x => x).ToList();
            return eftTerminals;
        }

        private List<EFTTerminalAudit> GetNewTerminals()
        {
            CerberusDataContext db = new CerberusDataContext();
            List<EFTTerminalAudit> eftTerminals = db.EFTTerminalAudits
                .Where(x => x.FirstVerified.Value.Date == DateTime.Now.Date)
                .ToList();
            return eftTerminals;
        }

        private List<EFTTerminalAudit> GetTerminalsByDate(DateTime date)
        {
            CerberusDataContext db = new CerberusDataContext();
            List<EFTTerminalAudit> eftTerminals = db.EFTTerminalAudits
                .Where(x => x.FirstVerified.Value.Date == date.Date)
                .ToList();
            return eftTerminals;
        }

        private List<EFTTerminalAudit> GetTerminalsByDateRange(DateTime startDate, DateTime endDate)
        {
            CerberusDataContext db = new CerberusDataContext();
            List<EFTTerminalAudit> eftTerminals = db.EFTTerminalAudits
                .Where(x => x.FirstVerified.Value.Date >= startDate.Date)
                .Where(x => x.FirstVerified.Value.Date <= endDate.Date)
                .ToList();
            return eftTerminals;
        }

        private List<EFTTerminalAudit> GetTerminalsByOffice(Int32 officeNo)
        {
            CerberusDataContext db = new CerberusDataContext();
            List<EFTTerminalAudit> eftTerminals = db.EFTTerminalAudits
                .Where(x => x.OfficeNo == officeNo)
                .ToList();
            return eftTerminals;
        }

        #endregion

        #region Data Binding Methods
        private Boolean IsToday(DateTime date)
        {
            if (date != null)
            {
                if (date.Date >= DateTime.Now.Date)
                {
                    return true;
                }
            }
            return false;
        }

        protected void gvEFT_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                String cellDate = e.Row.Cells[8].Text;
                DateTime dt = DateTime.Parse(cellDate);
          
                if (dt.Date >= DateTime.Now.Date)
                {
                    e.Row.BackColor = Color.Red;
                    _newTerminalCount++;
                }
            }  
        }

        #endregion

        protected void gvEFT_Sorting(object sender, GridViewSortEventArgs e)
        {
           IEnumerable<EFTTerminalAudit> efts = GetAllEftTerminals();
            //efts = efts.OrderBy(e.SortExpression, e.SortDirection);

            gvEFT.DataSource = efts;
            gvEFT.DataBind();
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            FillData();
        }

        protected void cmdByOffice_Click(object sender, EventArgs e)
        {
            GetTerminalsByOfficeFilter(1858);
        }

        protected void cmdByDate_Click(object sender, EventArgs e)
        {
            GetTerminalsByDateFilter(DateTime.Now);
        }

        protected void cmdByDateRange_Click(object sender, EventArgs e)
        {
            GetTerminalsByDateRangeFilter(DateTime.Now, DateTime.Now);
        }

        protected void cmdAllTerminals_Click(object sender, EventArgs e)
        {
            FillData();
        }
    }
}
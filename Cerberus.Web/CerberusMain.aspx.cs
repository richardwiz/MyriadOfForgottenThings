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

        protected void Page_Load(object sender, EventArgs e)
        {
            _eisaConnection = ConfigurationManager.ConnectionStrings["Eisa"].ToString();
            _cerberusConnection = ConfigurationManager.ConnectionStrings["Cerberus"].ToString();

            FillData();

        }

        private void FillData()
        {
            List<EFTTerminalAudit> efts = GetKnownEftTerminals();

            gvEFT.DataSource = efts;
            gvEFT.DataBind();
        }

        private List<EFTTerminalAudit> GetKnownEftTerminals()
        {
            // LINQ to SQL
            CerberusDataContext db = new CerberusDataContext();

            List<EFTTerminalAudit> eftTerminals = db.EFTTerminalAudits.Select(x => x).ToList();

            return eftTerminals;
        }

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
                    e.Row.BackColor = Color.Red;
            }  
        }

        protected void gvEFT_Sorting(object sender, GridViewSortEventArgs e)
        {
           IEnumerable<EFTTerminalAudit> efts = GetKnownEftTerminals();
            //efts = efts.OrderBy(e.SortExpression, e.SortDirection);

            gvEFT.DataSource = efts;
            gvEFT.DataBind();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;

namespace Galatee.Silverlight.Web
{
    public partial class PreviewPrintingFacturation : System.Web.UI.Page
    {
        string namesp = "Previews";
        string module = string.Empty;
        string rdlc = string.Empty;
        Previews.UcGeneric control = null;

        public PreviewPrintingFacturation()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    string key = Convert.ToString(Request.QueryString["key"]);
                    string frame = Convert.ToString(Request.QueryString["frame"]);
                    hiddenframe.Value = frame;
                    try
                    {
                        GalateePreview.LocalReport.ReportPath = PrintingPrieview.reportPath[key];

                        if (PrintingPrieview.reportParameter[key].Count > 0)
                            GalateePreview.LocalReport.SetParameters(PrintingPrieview.reportParameter[key]);
                        GalateePreview.LocalReport.DataSources.Add(PrintingPrieview.reportDataSource[key]);
                        GalateePreview.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                        GalateePreview.LocalReport.ReportEmbeddedResource = PrintingPrieview.reportPath[key];
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnApercu_Click(object sender, EventArgs e)
        {
            GalateePreview.Visible = true;
            // call generic method from usercontrol
            DataTable table = control.GetPrintObjects(module);

            if (!string.IsNullOrEmpty(control.DefinedRDLC))
                rdlc = control.DefinedRDLC;

            GalateePreview.ProcessingMode = ProcessingMode.Local;
            GalateePreview.LocalReport.ReportPath = Server.MapPath("~/Reports/" + module + @"/" + rdlc + ".rdlc");
            if (GalateePreview.LocalReport.DataSources != null)
                GalateePreview.LocalReport.DataSources.Clear();
            GalateePreview.LocalReport.DataSources.Add(new ReportDataSource(rdlc, table));

            List<ReportParameter> listeParam = new List<ReportParameter>();
            if (control.Parameters != null && control.Parameters.Count != 0)
            {
                foreach (KeyValuePair<string, string> pair in control.Parameters)
                {
                    ReportParameter param = new ReportParameter(pair.Key, pair.Value);
                    listeParam.Add(param);
                }

                if (listeParam != null & listeParam.Count > 0)
                    GalateePreview.LocalReport.SetParameters(listeParam);
            }
            GalateePreview.LocalReport.Refresh();
            form1.Focus();
        }

        protected void Button1_ServerClick(object sender, EventArgs e)
        {
            try
            {
                GalateePreview.LocalReport.DataSources.Clear();
                GalateePreview.LocalReport.Refresh();
                //GalateePreview.
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
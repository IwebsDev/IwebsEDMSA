using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Galatee.Silverlight.Web.ServicePrintings;
using System.Web.Services;
namespace Galatee.Silverlight.Web
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    //PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());

                    Utilitys objetUtil = new Utilitys(this);
                    string rdlc = Convert.ToString(Request.QueryString["rdlc"]);
                    string module = Convert.ToString(Request.QueryString["module"]);
                    string key = Convert.ToString(Request.QueryString["key"]);
                    string printer = Convert.ToString(Request.QueryString["printer"]);//Convert.ToString(Request.QueryString["printer"]).Replace("-_-",@"\");
                    string port = Convert.ToString(Request.QueryString["port"]);//Convert.ToString(Request.QueryString["printer"]).Replace("-_-",@"\");
                    string machine = Convert.ToString(Request.QueryString["machine"]);//Convert.ToString(Request.QueryString["printer"]).Replace("-_-",@"\");
                    string IscreationOnly = Convert.ToString(Request.QueryString["IscreationOnly"]);//Convert.ToString(Request.QueryString["printer"]).Replace("-_-",@"\");
                    bool _IscreationOnly = string.IsNullOrEmpty(IscreationOnly) ? false : Convert.ToBoolean(IscreationOnly);
                    string IsLandscape = Convert.ToString(Request.QueryString["IsLandscape"]);//Convert.ToString(Request.QueryString["printer"]).Replace("-_-",@"\");
                    string IsExport = Convert.ToString(Request.QueryString["IsExport"]);//Convert.ToString(Request.QueryString["printer"]).Replace("-_-",@"\");
                    string FormatExport = Convert.ToString(Request.QueryString["FormatExport"]);//Convert.ToString(Request.QueryString["printer"]).Replace("-_-",@"\");
                    string NomDuFichier = Convert.ToString(Request.QueryString["NomDuFichier"]);//Convert.ToString(Request.QueryString["printer"]).Replace("-_-",@"\");
                    string TypeEdition = Convert.ToString(Request.QueryString["TypeEdition"]);//Convert.ToString(Request.QueryString["printer"]).Replace("-_-",@"\");

                    bool _IsLandscape = string.IsNullOrEmpty(IsLandscape) ? false : Convert.ToBoolean(IsLandscape);
                    bool _IsExport = string.IsNullOrEmpty(IsExport) ? false : Convert.ToBoolean(IsExport);

                    SessionObject.machine = machine;
                    SessionObject.port = port;
                    SessionObject.portService = port;

                    //PrintingsServiceClient printErrosr = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                    //printErrosr.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), SessionObject.machine + SessionObject.port);

                    string leChemin = printer.Replace('[', '\\');

                    if (string.IsNullOrEmpty(NomDuFichier)) NomDuFichier = rdlc;
                    if (!_IsExport)
                        objetUtil.CreatePDFChemin(NomDuFichier, leChemin, _IsLandscape, rdlc, module, key);
                    else
                        objetUtil.ExportChemin(NomDuFichier, leChemin, _IsLandscape, rdlc, module, key, FormatExport, TypeEdition);

 
                    //if ((module == "Caisse")  && !string.IsNullOrEmpty(port))
                    //    objetUtil.LaunchCashPrint(printer, module, rdlc, key, port, machine);
                    //else
                    //{
                    //    if (!_IscreationOnly)
                    //    {
                    //        string leChemin = printer.Replace('5', '\\');
                    //        objetUtil.CreatePDFChemin(rdlc, leChemin, _IsLandscape, rdlc, module, key);
                    //    }
                    //    else
                    //        objetUtil.LaunchPrinting(_IsLandscape, module, printer, rdlc, key, _IscreationOnly);
                    //}

                }
                catch (Exception ex)
                {
                    PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                    printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), ex.InnerException.Message );
                }
            }
        }

        [WebMethod]
        public bool invokeMethod(string dd)
        {

            return true;
        }
    }
}
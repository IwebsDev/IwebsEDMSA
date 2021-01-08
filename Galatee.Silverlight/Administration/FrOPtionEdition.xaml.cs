using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.ServiceAdministration;

namespace Galatee.Silverlight.Administration
{
    public partial class FrOPtionEdition : ChildWindow
    {
        List<CsUtilisateur> lstUtilisateur = new List<CsUtilisateur>();
        CsUtilisateur leUtilisateur = new CsUtilisateur();
        public FrOPtionEdition(CsUtilisateur _leUtilisateur)
        {
            InitializeComponent();
            leUtilisateur = _leUtilisateur;
        }
        public FrOPtionEdition(List<CsUtilisateur> _leUtilisateur)
        {
            InitializeComponent();
            lstUtilisateur = _leUtilisateur;
        }
        public FrOPtionEdition()
        {
            InitializeComponent();
        }
        List<int?> lstIUSeru ;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.FrmOptionEditon ctrl = new Shared.FrmOptionEditon();
            ctrl.Closed += ctrl_Closed;
            this.IsEnabled = false;
            ctrl.Show();
        }

        string Option = string.Empty;
        void ctrl_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            Galatee.Silverlight.Shared.FrmOptionEditon ctrs = sender as Galatee.Silverlight.Shared.FrmOptionEditon;
            if (ctrs.IsOptionChoisi)
            {
                Option = ctrs.OptionSelect ;
                lstIUSeru = new List<int?>();
                lstUtilisateur.Select(u => u.PK_ID).ToList().ForEach(u => lstIUSeru.Add(u));
                if (this.chk_habilitation.IsChecked == true)
                    RetourneHabilitationUser(lstUtilisateur.Select(u => u.PK_ID).ToList());
                if (this.chk_historiqueConnect.IsChecked == true)
                    RetourneHistoriqueConnectionClient(lstIUSeru);
                if (this.chk_historiquePass.IsChecked == true)
                    RetourneHistoriquePassWordClient(lstIUSeru);
            }
        }
        private void RetourneHabilitationUser(List<int> ListidClient)
        {
            string key = Utility.getKey();
            AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            client.RetourneProfilUtilisateurCompleted += (ss, res) =>
            {
                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                    return;
                }
                if (res.Result == null)
                {
                    Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
                    return;
                }
                //Utility.ActionDirectOrientation<ServicePrintings.CsHabilitationMenu, ServiceAdministration.CsHabilitationMenu>(res.Result, null, SessionObject.CheminImpression, "ReportHabillitationMenuUser", "Administration", true);

                if (Option  == SessionObject.EnvoiPrinter)
                    Utility.ActionDirectOrientation<ServicePrintings.CsHabilitationMenu, ServiceAdministration.CsHabilitationMenu>(res.Result, null, SessionObject.CheminImpression, "ReportHabillitationMenuUser", "Administration", true);
                else if (Option == SessionObject.EnvoiExecl)
                    Utility.ActionExportation<ServicePrintings.CsHabilitationMenu, ServiceAdministration.CsHabilitationMenu>(res.Result, null, string.Empty, SessionObject.CheminImpression, "ReportHabillitationMenuUser", "Administration", true, "xlsx");

                else if (Option == SessionObject.EnvoiWord)
                    Utility.ActionExportation<ServicePrintings.CsHabilitationMenu, ServiceAdministration.CsHabilitationMenu>(res.Result, null, string.Empty, SessionObject.CheminImpression, "ReportHabillitationMenuUser", "Administration", true, "doc");

                else if (Option == SessionObject.EnvoiPdf)
                    Utility.ActionExportation<ServicePrintings.CsHabilitationMenu, ServiceAdministration.CsHabilitationMenu>(res.Result, null, string.Empty, SessionObject.CheminImpression, "ReportHabillitationMenuUser", "Administration", true, "pdf");


            };
            client.RetourneProfilUtilisateurAsync(lstUtilisateur.Select(u => u.PK_ID).ToList(), key);

        }

        private void RetourneHistoriqueConnectionClient(List<int?> idClient)
        {
            string key = Utility.getKey();
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("pNomConnection", leUtilisateur.LIBELLE);

            AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            client.RetourneHistoriqueConnectionfromListUserCompleted += (ss, res) =>
            {
                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                    return;
                }
                if (res.Result == null  )
                {
                    Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
                    return;
                }
                //Utility.ActionDirectOrientation<ServicePrintings.CsHistoriquePassword, ServiceAdministration.CsHistoriquePassword>(res.Result, null, SessionObject.CheminImpression, "ReportConnectionUser", "Administration", true);

                if (Option == SessionObject.EnvoiPrinter)
                    Utility.ActionDirectOrientation<ServicePrintings.CsHistoriquePassword, ServiceAdministration.CsHistoriquePassword>(res.Result, null, SessionObject.CheminImpression, "ReportConnectionUser", "Administration", true);
                else if (Option == SessionObject.EnvoiExecl)
                    Utility.ActionExportation<ServicePrintings.CsHistoriquePassword, ServiceAdministration.CsHistoriquePassword>(res.Result, null, string.Empty, SessionObject.CheminImpression, "ReportConnectionUser", "Administration", true, "xlsx");

                else if (Option == SessionObject.EnvoiWord)
                    Utility.ActionExportation<ServicePrintings.CsHistoriquePassword, ServiceAdministration.CsHistoriquePassword>(res.Result, null, string.Empty, SessionObject.CheminImpression, "ReportConnectionUser", "Administration", true, "doc");

                else if (Option == SessionObject.EnvoiPdf)
                    Utility.ActionExportation<ServicePrintings.CsHistoriquePassword, ServiceAdministration.CsHistoriquePassword>(res.Result, null, string.Empty, SessionObject.CheminImpression, "ReportConnectionUser", "Administration", true, "pdf");
            };
            client.RetourneHistoriqueConnectionfromListUserAsync(idClient);

        }

        private void RetourneHistoriquePassWordClient(List<int?> idClient)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("pNomConnection", string.Empty );

            string key = Utility.getKey();
            AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            client.RetourneHistoriquePasswordFromListUserCompleted += (ss, res) =>
            {
                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                    return;
                }
                if (res.Result == null   )
                {
                    Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
                    return;
                }
                //Utility.ActionDirectOrientation<ServicePrintings.CsHistoriquePassword, ServiceAdministration.CsHistoriquePassword>(res.Result, null, SessionObject.CheminImpression, "ReportPasswordUser", "Administration", true);

                if (Option == SessionObject.EnvoiPrinter)
                    Utility.ActionDirectOrientation<ServicePrintings.CsHistoriquePassword, ServiceAdministration.CsHistoriquePassword>(res.Result, null, SessionObject.CheminImpression, "ReportPasswordUser", "Administration", true);
                else if (Option == SessionObject.EnvoiExecl)
                    Utility.ActionExportation<ServicePrintings.CsHistoriquePassword, ServiceAdministration.CsHistoriquePassword>(res.Result, null, string.Empty, SessionObject.CheminImpression, "ReportPasswordUser", "Administration", true, "xlsx");

                else if (Option == SessionObject.EnvoiWord)
                    Utility.ActionExportation<ServicePrintings.CsHistoriquePassword, ServiceAdministration.CsHistoriquePassword>(res.Result, null, string.Empty, SessionObject.CheminImpression, "ReportPasswordUser", "Administration", true, "doc");

                else if (Option == SessionObject.EnvoiPdf)
                    Utility.ActionExportation<ServicePrintings.CsHistoriquePassword, ServiceAdministration.CsHistoriquePassword>(res.Result, null, string.Empty, SessionObject.CheminImpression, "ReportPasswordUser", "Administration", true, "pdf");

            };
            client.RetourneHistoriquePasswordFromListUserAsync(idClient );
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);

        }
    }
}


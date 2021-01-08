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
using Galatee.Silverlight.ServiceRecouvrement;
//using Galatee.Silverlight.ServicePrintings;
using System.ComponentModel;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmClientAPoser : ChildWindow
    {
        List<CsCentre> lstCentreCampagne = new List<CsCentre>();
        List<CsCAMPAGNE> lesCampagne = new List<CsCAMPAGNE>();
        List<CsSite> lstSiteCampagne = new List<CsSite>();
        List<CsSite> lstAgentCampagne = new List<CsSite>();
        public FrmClientAPoser()
        {
            try
            {
                InitializeComponent();
                InitialiserControle();
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
               
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        List<int> lesCentreCaisse = new List<int>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                List<int> lstCentreSelect = new List<int>();

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                        ChargerPiaAgence(lstSite.First().CODE);
                        this.btn_Site.IsEnabled = false;
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                        ChargerPiaAgence(lstSite.First().CODE);
                        this.btn_Site.IsEnabled = false;
                    }
                    return;
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite.Count > 0)
                {
                    this.btn_Site.IsEnabled = false;
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "SITE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                    ctrl.Closed += new EventHandler(galatee_OkClickedSite);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Recouvrement");
            }
        }
        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeSite.Text) && Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    Galatee.Silverlight.ServiceAccueil.CsSite _LeSiteClient = Shared.ClasseMEthodeGenerique.RetourneObjectFromList(lstSite, this.Txt_CodeSite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeSiteClient.LIBELLE))
                    {
                        this.Txt_LibelleSite.Text = _LeSiteClient.LIBELLE;
                        this.Txt_CodeSite.Text = _LeSiteClient.CODE;
                        this.Txt_CodeSite.Tag = _LeSiteClient.PK_ID;
                        ChargerPiaAgence(_LeSiteClient.CODE);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

            }
        }
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODE;
                this.Txt_CodeSite.Tag = leSite.PK_ID;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
            }
            this.btn_Site.IsEnabled = true;
        }


        private void ChargerPiaAgence(string CodeSite)
        {
            try
            {

                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.RetournePIAAgenceCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    List<CsUtilisateur> lstPia = new List<CsUtilisateur>();
                    lstPia.Add(new CsUtilisateur() { LIBELLE = "Aucun" });
                    lstPia.AddRange(args.Result);

                    this.cmbAgent.ItemsSource = null;
                    this.cmbAgent.ItemsSource = lstPia;
                    this.cmbAgent.DisplayMemberPath = "LIBELLE";
                    return;
                };
                service.RetournePIAAgenceAsync(CodeSite);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void InitialiserControle()
        {
            try
            {
                ChargerDonneeDuSite();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public event EventHandler Closed;
        public bool IsGettingIdCoupure = false;
        public void ClosedEnventHandler()
        {
            if (this.Closed != null)
            {
                this.Closed(this, null);
            }
        }

        public CsCAMPAGNE CampagneSelect;
        public List<CsCAMPAGNE> LesCampagneSelect = new List<CsCAMPAGNE>();

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lvwResultat.ItemsSource != null)
                {
                    List<CsDetailCampagne> lstSource = (List<CsDetailCampagne>)this.lvwResultat.ItemsSource;
                    Utility.ActionExportation<ServicePrintings.CsDetailCampagne, ServiceRecouvrement.CsDetailCampagne>(lstSource, null, string.Empty, SessionObject.CheminImpression, "CampagnePoser", "Recouvrement", true, "xlsx");
                }
            }
            catch (Exception ex)
            {

                this.OKButton.IsEnabled = true;
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void btnReinitialiser_Click(object sender, EventArgs e)
        {
            try
            {
                this.lvwResultat.ItemsSource = null;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClosedEnventHandler();
            this.DialogResult = false;
        }

        List<CsDetailCampagne> detailcampagnes ; 
        void Recherche(DateTime? datedebut, DateTime? datefin)
        {
            try
            {
                detailcampagnes = new List<CsDetailCampagne>();
                lvwResultat.ItemsSource = null;
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                client.EditerCLientaPoserAsync(datedebut, datefin);
                client.EditerCLientaPoserCompleted += (ss, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError("Erreur à l'exécution du service", "SearchCampagne");
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            Message.ShowInformation("Aucune donnée trouvée", "SearchCampagne");
                            return;
                        }
                        detailcampagnes = args.Result;
                        lvwResultat.ItemsSource = null;
                        lvwResultat.ItemsSource = detailcampagnes;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void btnsearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string CodeSite = string.Empty; string IdCampagne = string.Empty; int IdPia = 0; DateTime? DateDebut = null; DateTime? DateFin = null; string Centre = null; string Client = null; string Ordre = null;
                if (!string.IsNullOrEmpty(this.Txt_CodeSite.Text)) CodeSite = this.Txt_CodeSite.Text;
                if (!string.IsNullOrEmpty(this.Txt_NumCampagne.Text)) IdCampagne = this.Txt_NumCampagne.Text;
                if (cmbAgent.SelectedItem != null) IdPia = ((CsUtilisateur)cmbAgent.SelectedItem).PK_ID;
                if (this.dtpDate.SelectedDate != null) DateDebut = this.dtpDate.SelectedDate.Value;
                if (this.dtpDateFin.SelectedDate != null) DateFin = this.dtpDateFin.SelectedDate.Value;
                 Client = string.Empty ;
                 Ordre = string.Empty;
                Recherche(CodeSite, IdCampagne, IdPia, DateDebut, DateFin, Centre, Client, Ordre);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        void Recherche(string CodeSite, string IdCampagne, int IdPia, DateTime? DateDebut, DateTime? DateFin, string Centre, string Client, string Ordre)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible ;
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                client.RechercheClientCampagneCompleted += (ss, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed ;
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError("Erreur à l'exécution du service", "SearchCampagne");
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            Message.ShowInformation("Aucune donnée trouvée", "SearchCampagne");
                            return;
                        }

                        List<CsDetailCampagne> detailcampagnes = new List<CsDetailCampagne>();
                        detailcampagnes = args.Result;

                        this.lvwResultat.ItemsSource = null;
                        this.lvwResultat.ItemsSource = detailcampagnes;

                        OKButton.Visibility = System.Windows.Visibility.Visible;

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                client.RechercheClientCampagneAsync(CodeSite, IdCampagne, IdPia, DateDebut, DateFin, Centre, Client, Ordre, 5);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void btnreset_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbSite_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                 
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void btn_Exporter_Click_1(object sender, RoutedEventArgs e)
        {
            

                if (lvwResultat.ItemsSource != null)
                {
                    List<CsDetailCampagne> lstSource = (List<CsDetailCampagne>)this.lvwResultat.ItemsSource;
                    Utility.ActionExportation<ServicePrintings.CsDetailCampagne, ServiceRecouvrement.CsDetailCampagne>(lstSource, null, string.Empty, SessionObject.CheminImpression, "CampagnePoser", "Recouvrement", true, "xlsx");

                }
        }
    }
}


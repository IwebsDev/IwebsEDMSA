using Galatee.Silverlight.Shared;
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

namespace Galatee.Silverlight.Precontentieux
{
    public partial class FrmSaisiFicheDecharge : ChildWindow
    {
        public FrmSaisiFicheDecharge()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            MiseAJourDecharge();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        List<ServiceAccueil.CsCentre> LstCentre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre.Count != 0)
                {
                    LstCentre = SessionObject.LstCentre;
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    if (lstSite != null)
                    {
                        List<ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = _LstSite[0].CODE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.IsReadOnly = true;
                        }
                        LstCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == _LstSite[0].PK_ID).ToList();
                        if (LstCentre != null && LstCentre.Count == 1)
                        {
                            this.Txt_CodeCentre.Text = LstCentre.First().CODE;
                            this.Txt_LibelleCentre.Text = LstCentre.First().LIBELLE;
                            this.Txt_CodeCentre.Tag = LstCentre.First().PK_ID ;
                        }
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = SessionObject.LstCentre;
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    if (lstSite != null)
                    {
                        List<ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = _LstSite[0].CODE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.IsReadOnly = true;
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerDonneeDuSite");

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
                Message.ShowError(ex.Message,"Precontentieux");
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
                List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList();
                if (lsiteCentre.Count == 1)
                {
                    this.Txt_CodeCentre.Text = lsiteCentre.First().CODE;
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_CodeCentre.Tag = lsiteCentre.First().PK_ID;
                }
            }
            this.btn_Site.IsEnabled = true;
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentrePerimetre.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList());
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "CENTRE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Liste");
                    ctrl.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message,"Precontentieux");
            }
        }
        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Centre.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsCentre leCentre = (Galatee.Silverlight.ServiceAccueil.CsCentre)ctrs.MyObject;
                this.Txt_CodeCentre.Text = leCentre.CODE;
                this.Txt_LibelleCentre.Text = leCentre.LIBELLE;
                this.Txt_CodeCentre.Tag = leCentre.PK_ID;
            }
            else
                this.btn_Centre.IsEnabled = true;

        }
        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeSite.Text) && Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    Galatee.Silverlight.ServiceAccueil.CsSite _LeSiteClient = ClasseMEthodeGenerique.RetourneObjectFromList(lstSite, this.Txt_CodeSite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeSiteClient.LIBELLE))
                    {
                        this.Txt_LibelleSite.Text = _LeSiteClient.LIBELLE;
                        this.Txt_CodeSite.Text = _LeSiteClient.CODE;
                        this.Txt_CodeSite.Tag = _LeSiteClient.PK_ID;
                        List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList();
                        if (lsiteCentre.Count == 1)
                        {
                            this.Txt_CodeCentre.Text = lsiteCentre.First().CODE;
                            this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                            this.Txt_CodeCentre.Tag = lsiteCentre.First().PK_ID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

            }
        }

        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeCentre.Text) && Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    ServiceAccueil.CsCentre _LeCentreClient = ClasseMEthodeGenerique.RetourneObjectFromList(LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList(), this.Txt_CodeCentre.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
                    {
                        this.Txt_CodeCentre.Text = _LeCentreClient.CODE;
                        this.Txt_LibelleCentre.Text = _LeCentreClient.LIBELLE;
                        this.Txt_CodeCentre.Tag = _LeCentreClient.PK_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

            }
        }

        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient)
                    ChargerClientFromReference((int)this.Txt_CodeCentre.Tag,this.Txt_ReferenceClient.Text,this.Txt_Ordre .Text );
                else
                {
                    Message.Show("La reference saisie n'est pas correcte", "Infomation");
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }
        private void ChargerClientFromReference(int idCentre, string ReferenceClient,string Ordre)
        {
            try
            {
                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.RetourneClientByReferenceOrdreCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                   
                        if (args.Result != null  )
                        {
                            this.Txt_NomClient.Text = args.Result.NOMABON;
                            this.Txt_AdresseClient.Text =!string.IsNullOrEmpty( args.Result.ADRESSE )?args.Result.ADRESSE :string.Empty ;
                            this.Txt_ReferenceClient.Tag = args.Result ;
                        }
                };
                service.RetourneClientByReferenceOrdreAsync(idCentre, ReferenceClient, Ordre);
                service.CloseAsync();

            }
            catch (Exception)
            {
                Message.ShowError("Erreur au chargement des données", "Demande");
            }
        }

        private void galatee_OkClickedChoixClient(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsClient _UnClient = (Galatee.Silverlight.ServiceAccueil.CsClient)ctrs.MyObject;

                this.Txt_NomClient.Text = _UnClient.NOMABON;
                this.Txt_AdresseClient.Text = !string.IsNullOrEmpty(_UnClient.ADRMAND1) ? _UnClient.ADRMAND1 : string.Empty;
                this.Txt_ReferenceClient.Tag = _UnClient;
            }
        }

        private void Btn_ListeAppareils_Click(object sender, RoutedEventArgs e)
        {
            RecherCheClientByCompteur ctrl = new RecherCheClientByCompteur();
            ctrl.Closed += ctrl_Closed;
            ctrl.Show();
        }
        List<ServiceRecouvrement.CsClientRechercher> lstClientCpteur = new List<CsClientRechercher>();
        void ctrl_Closed(object sender, EventArgs e)
        {
             RecherCheClientByCompteur ctrs = sender as RecherCheClientByCompteur;
             if (ctrs.leClient != null)
            {
                lstClientCpteur.Add(ctrs.leClient);
                this.dataGrid1.ItemsSource = null;
                this.dataGrid1.ItemsSource = lstClientCpteur;
            }

        }
        private CsPrecontentieuxDechargement GetInfoScream()
        {
           
            CsPrecontentieuxDechargement decharge = new CsPrecontentieuxDechargement();
            decharge.CENTRE = ((CsDetailCampagnePrecontentieux )this.Txt_ReferenceClient.Tag).CENTRE;
            decharge.CLIENT = ((CsDetailCampagnePrecontentieux)this.Txt_ReferenceClient.Tag).CLIENT;
            decharge.ORDRE = ((CsDetailCampagnePrecontentieux)this.Txt_ReferenceClient.Tag).ORDRE;
            decharge.FK_IDCENTRE = ((CsDetailCampagnePrecontentieux)this.Txt_ReferenceClient.Tag).FK_IDCENTRE;
            decharge.FK_IDCLIENT = ((CsDetailCampagnePrecontentieux)this.Txt_ReferenceClient.Tag).PK_ID;
            decharge.DECEDEAVECAYANTDROIT = (this.IsDecedeAvecAyantD.IsChecked == true) ? true : false;
            decharge.DECEDESANSAYANTDROIT  = (this.IsDecedeSansAyantD.IsChecked == true) ? true : false;
            decharge.LOCATAIRE  = (this.IsLocataire.IsChecked == true) ? true : false;
            decharge.POINTINTROUVABLE = (this.IsIntrouvable.IsChecked == true) ? true : false;
            decharge.NUMCOMPTEUR  = this.Txt_NumCompteur .Text ;
            if (lstClientCpteur != null && lstClientCpteur.Count != 0)
            {
                List<CsPrecontentieuxAutreClient> AutreClient = new List<CsPrecontentieuxAutreClient>();
                foreach (var item in lstClientCpteur)
                {
                    CsPrecontentieuxAutreClient AuClient = new CsPrecontentieuxAutreClient();
                    AuClient.FK_IDAUTRECLIENT = item.PK_ID;
                    AuClient.FK_IDCLIENTPRECONTENTIEUX = decharge.FK_IDCLIENT;
                    AutreClient.Add(AuClient);
                }
                decharge.ListAutreClient = new List<CsPrecontentieuxAutreClient>();
                decharge.ListAutreClient= AutreClient;
            }
            return decharge;
        
        }

        private void MiseAJourDecharge()
        {
            try
            {
                CsPrecontentieuxDechargement dech = new CsPrecontentieuxDechargement();
                if (this.Txt_ReferenceClient.Tag != null)
                    dech = GetInfoScream();
                else
                    return;
                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient proxy = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                proxy.InsererDechargePrecontentieuxAsync(dech);
                proxy.InsererDechargePrecontentieuxCompleted += (ssn, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    if (args.Result != null && args.Result==true )
                        Message.ShowInformation("Mise a jour validée", "Info");
                    else
                        Message.ShowInformation("Erreur a la mise a jour", "Info");

                };
                proxy.CloseAsync();

            }
            catch (Exception)
            {
                Message.ShowError("Erreur ", "Demande");
            }
        }

        private void Btn_Rechercher_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IsLocataire_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLocataire.IsChecked == true)
                IsProprietaire.IsEnabled = false; 
        }

        private void IsLocataire_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IsLocataire.IsChecked == false )
                IsProprietaire.IsEnabled = true; 
        }

        private void IsDecedeAvecAyantD_Checked(object sender, RoutedEventArgs e)
        {
            if (IsDecedeAvecAyantD.IsChecked == true)
                IsDecedeSansAyantD.IsEnabled = false;
        }

        private void IsDecedeSansAyantD_Checked(object sender, RoutedEventArgs e)
        {
            if (IsDecedeSansAyantD.IsChecked == true)
                IsDecedeAvecAyantD.IsEnabled = false;
        }

        private void IsDecedeSansAyantD_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IsDecedeSansAyantD.IsChecked == false)
                IsDecedeAvecAyantD.IsEnabled = true; 
        }

        private void IsDecedeAvecAyantD_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IsDecedeAvecAyantD.IsChecked == false)
                IsDecedeSansAyantD.IsEnabled = true; 
        }

    }
}


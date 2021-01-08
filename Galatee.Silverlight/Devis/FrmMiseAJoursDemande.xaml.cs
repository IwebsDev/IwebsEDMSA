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
using Galatee.Silverlight.ServiceAccueil ;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;
using System.Text.RegularExpressions;
using Galatee.Silverlight.MainView;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmMiseAJoursDemande : ChildWindow
    {
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        CsDemande laDetailDemande = null;
        string CodeProduit = string.Empty;
        List<Galatee.Silverlight.ServiceAccueil.CsTypeBranchement> LstTypeBrt;
        List<CsTcompteur> LstTypeCompteur = new List<CsTcompteur>();
        List<CsCalibreCompteur> LstCalibreCompteur = new List<CsCalibreCompteur>();
        List<CsMarqueCompteur> LstMarque = new List<CsMarqueCompteur>();

        private List<CsStatutJuridique> ListStatuJuridique = new List<CsStatutJuridique>();
        public List<CsCATEGORIECLIENT_TYPECLIENT> LstCategorieClient_TypeClient = new List<CsCATEGORIECLIENT_TYPECLIENT>();
        public List<CsNATURECLIENT_TYPECLIENT> LstNatureClient_TypeClient = new List<CsNATURECLIENT_TYPECLIENT>();
        public List<CsUSAGE_NATURECLIENT> LstUsage_NatureClient = new List<CsUSAGE_NATURECLIENT>();
        public List<CsCATEGORIECLIENT_USAGE> LstCategorieClient_Usage = new List<CsCATEGORIECLIENT_USAGE>();

        public FrmMiseAJoursDemande()
        {
            InitializeComponent();
            ChargerListDesSite();
            Tdem = SessionObject.Enumere.BranchementAbonement ;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            ChargerCategorieClient_TypeClient();
            ChargerNatureClient_TypeClient();
            ChargerUsage_NatureClient();
            ChargerCategorieClient_Usage();
            RemplirStatutJuridique();
            ChargerPuissance();
            RemplirListeDesTypeComptage();
            ChargerDiametreBranchement();
            ChargerDiametreCompteur();
            ChargerMarque();
            ChargerTypeCompteur();
            RemplirProprietaire();
        }
        public FrmMiseAJoursDemande(string TypeDemande)
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;

        }
        string Tdem = string.Empty;
        public FrmMiseAJoursDemande(string TypeDemande,string IsInit)
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }
  
        private void ChargerCategorieClient_TypeClient()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCategorieClient_TypeClientCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstCategorieClient_TypeClient.Add(item);
                    }
                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerCategorieClient_TypeClientAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChargerNatureClient_TypeClient()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerNatureClient_TypeClientCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstNatureClient_TypeClient.Add(item);
                    }
                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerNatureClient_TypeClientAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChargerUsage_NatureClient()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerUsage_NatureClientCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstUsage_NatureClient.Add(item);
                    }
                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerUsage_NatureClientAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChargerCategorieClient_Usage()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));

                service.ChargerCategorieClient_UsageCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstCategorieClient_Usage.Add(item);
                    }
                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerCategorieClient_UsageAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChargerPuissance()
        {
            try
            {
                if (SessionObject.LstPuissanceInstalle != null && SessionObject.LstPuissanceInstalle.Count != 0)
                {

                    List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceInstalle;
                    if (_lstPuissance != null && _lstPuissance.Count != 0)
                    {
                        List<ServiceAccueil.CsPuissance> lesPuissance = _lstPuissance.Where(t => t.PRODUIT == SessionObject.Enumere.ElectriciteMT).ToList();
                        Cbo_Puissance.SelectedValuePath = "FK_IDPUISSANCE";
                        Cbo_Puissance.DisplayMemberPath = "VALEUR";
                        Cbo_Puissance.ItemsSource = lesPuissance;
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                        return;
                    }
                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceInstalleCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissanceInstalle = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceInstalle;
                    if (_lstPuissance != null && _lstPuissance.Count != 0)
                    {
                        List<ServiceAccueil.CsPuissance> lesPuissance = _lstPuissance.Where(t => t.PRODUIT == SessionObject.Enumere.ElectriciteMT).ToList();
                        Cbo_Puissance.SelectedValuePath = "FK_IDPUISSANCE";
                        Cbo_Puissance.DisplayMemberPath = "VALEUR";
                        Cbo_Puissance.ItemsSource = lesPuissance;
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                        return;
                    }
                };
                service.ChargerPuissanceInstalleAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerDiametreBranchement()
        {
            try
            {
                if (SessionObject.LstTypeBranchement != null && SessionObject.LstTypeBranchement.Count != 0)
                {
                    LstTypeBrt = SessionObject.LstTypeBranchement;
                    if (LstTypeBrt != null && LstTypeBrt.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text))
                        {
                            
                            Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _LeDiametre = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeBrt, this.Txt_TypeBrancehment.Text, "CODE");
                            if (_LeDiametre != null && !string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                                this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                        }
                    }

                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeBranchementCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    SessionObject.LstTypeBranchement = args.Result;
                    LstTypeBrt = SessionObject.LstTypeBranchement;
                    if (LstTypeBrt != null && LstTypeBrt.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text))
                        {
                            Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _LeDiametre = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeBrt, this.Txt_TypeBrancehment.Text, "CODE");
                            if (_LeDiametre != null && !string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                                this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                        }
                    }
                };
                service.ChargerTypeBranchementAsync();
                service.CloseAsync();
            }
            catch (Exception es)
            {

                MessageBox.Show(es.Message);
            }

        }
        private void RemplirListeDesTypeComptage()
        {

            try
            {
                if (SessionObject.LstTypeComptage != null && SessionObject.LstTypeComptage.Count != 0)
                {
                    List<Galatee.Silverlight.ServiceAccueil.CsTypeComptage> listeTypeDeComptageExistant = SessionObject.LstTypeComptage;
                    Cbo_TypeComptage.SelectedValuePath = "PK_ID";
                    Cbo_TypeComptage.DisplayMemberPath = "LIBELLE";
                    Cbo_TypeComptage.ItemsSource = listeTypeDeComptageExistant;

                    if (listeTypeDeComptageExistant != null && listeTypeDeComptageExistant.Count == 1)
                    {
                        Cbo_TypeComptage.SelectedItem = listeTypeDeComptageExistant.First();
                        Cbo_TypeComptage.Tag = listeTypeDeComptageExistant.First();
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeComptageCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    SessionObject.LstTypeComptage = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsTypeComptage> listeTypeDeComptageExistant = SessionObject.LstTypeComptage;
                    Cbo_TypeComptage.SelectedValuePath = "PK_ID";
                    Cbo_TypeComptage.DisplayMemberPath = "LIBELLE";
                    Cbo_TypeComptage.ItemsSource = listeTypeDeComptageExistant;
                    if (listeTypeDeComptageExistant != null && listeTypeDeComptageExistant.Count == 1)
                    {
                        Cbo_TypeComptage.SelectedItem = listeTypeDeComptageExistant.First();
                        Cbo_TypeComptage.Tag = listeTypeDeComptageExistant.First();
                    }
                    return;
                };
                service.ChargerTypeComptageAsync();
                service.CloseAsync();
            }
            catch (Exception es)
            {

                MessageBox.Show(es.Message);
            }
        }
        private void RemplirProprietaire()
        {
            try
            {
                if (SessionObject.Lsttypeprop.Count != 0)
                {
                    Cbo_Type_Proprietaire.DisplayMemberPath = "LIBELLE";
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RemplirProprietaireCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.Lsttypeprop = args.Result;
                        return;
                    };
                    service.RemplirProprietaireAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirStatutJuridique()
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.GetAllStatutJuridiqueAsync();
                service.GetAllStatutJuridiqueCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    Cbo_StatutJuridique.Items.Clear();
                    if (args.Result != null && args.Result.Count > 0)
                        foreach (var item in args.Result)
                        {
                            Cbo_StatutJuridique.Items.Add(item);
                        }
                    ListStatuJuridique = args.Result;
                    Cbo_StatutJuridique.SelectedValuePath = "PK_ID";
                    Cbo_StatutJuridique.DisplayMemberPath = "LIBELLE";

                    LayoutRoot.Cursor = Cursors.Arrow;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        List<int> lstIdCentre = new List<int>();
        List<CsCentre> _listeDesCentreExistant = null;
        void ChargerListDesSite()
        {
            try
            {

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    _listeDesCentreExistant = lesCentre;

                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        lstIdCentre.Add(item.PK_ID);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteAsync(false);
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        if (SessionObject.LstCentre.Count != 0)
                        {
                            lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                            lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                            _listeDesCentreExistant = lesCentre;

                            foreach (ServiceAccueil.CsCentre item in lesCentre)
                                lstIdCentre.Add(item.PK_ID);

                        }
                        else
                        {
                            Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
     
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void ChargeDetailDEvis(CsClient leclient)
        {

            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GeDetailByFromClientCompleted += (ssender, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed ;
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    else
                    {
                     
                    }
                };
                client.GeDetailByFromClientAsync(leclient);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void RemplireOngletClient( CsClient _LeClient)
        {
            try
            {
                if (_LeClient != null)
                {

                     this.txt_NomClient.Text = (string.IsNullOrEmpty(_LeClient.NOMABON) ? string.Empty : _LeClient.NOMABON);
                     this.txt_Telephone.Text = string.IsNullOrEmpty(_LeClient.TELEPHONE) ? string.Empty : _LeClient.TELEPHONE;
                     this.txt_Adresse.Text = string.IsNullOrEmpty(_LeClient.ADRMAND1) ? string.Empty : _LeClient.ADRMAND1;
                     this.txt_Nina.Text = string.IsNullOrEmpty(_LeClient.NUMEROIDCLIENT) ? string.Empty : _LeClient.NUMEROIDCLIENT;

                    //this.tab12_txt_addresse.Text = string.IsNullOrEmpty(_LeClient.ADRMAND1) ? string.Empty : _LeClient.ADRMAND1;
                    //this.tab12_txt_addresse2.Text = string.IsNullOrEmpty(_LeClient.ADRMAND2) ? string.Empty : _LeClient.ADRMAND2;
                    this.tab12_Txt_LibelleCodeConso.Text = string.IsNullOrEmpty(_LeClient.LIBELLECODECONSO) ? string.Empty : _LeClient.LIBELLECODECONSO;
                    this.tab12_Txt_LibelleCategorie.Text = string.IsNullOrEmpty(_LeClient.LIBELLECATEGORIE) ? string.Empty : _LeClient.LIBELLECATEGORIE;
                    this.tab12_Txt_LibelleEtatClient.Text = string.IsNullOrEmpty(_LeClient.LIBELLERELANCE) ? string.Empty : _LeClient.LIBELLERELANCE;
                    this.tab12_Txt_LibelleTypeClient.Text = string.IsNullOrEmpty(_LeClient.LIBELLENATURECLIENT) ? string.Empty : _LeClient.LIBELLENATURECLIENT;
                    this.tab12_Txt_Nationnalite.Text = string.IsNullOrEmpty(_LeClient.LIBELLENATIONALITE) ? string.Empty : _LeClient.LIBELLENATIONALITE;
                    this.tab12_Txt_Datecreate.Text = string.IsNullOrEmpty(_LeClient.DATECREATION.ToString()) ? string.Empty : Convert.ToDateTime(_LeClient.DATECREATION).ToShortDateString();
                    //this.tab12_Txt_DateModif.Text = string.IsNullOrEmpty(_LeClient.DATEMODIFICATION.ToString()) ? string.Empty : Convert.ToDateTime(_LeClient.DATEMODIFICATION).ToShortDateString();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void RemplirOngletAbonnement(CsAbon _LeAbon)
        {
            if (_LeAbon != null)
            {
                this.Txt_CodeTarif.Text = !string.IsNullOrEmpty(_LeAbon.TYPETARIF) ? _LeAbon.TYPETARIF : string.Empty;
                this.Txt_CodePussanceSoucrite.Text = !string.IsNullOrEmpty(_LeAbon.PUISSANCE.Value.ToString()) ? _LeAbon.PUISSANCE.Value.ToString() : string.Empty;

                if (_LeAbon.PUISSANCE != null)
                    this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(_LeAbon.PUISSANCE.ToString()).ToString("N2");
                if (_LeAbon.PUISSANCEUTILISEE != null)
                    this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(_LeAbon.PUISSANCEUTILISEE.Value).ToString("N2");

                this.Txt_CodeForfait.Text = string.IsNullOrEmpty(_LeAbon.FORFAIT) ? string.Empty : _LeAbon.FORFAIT;
                this.Txt_LibelleForfait.Text = string.IsNullOrEmpty(_LeAbon.LIBELLEFORFAIT) ? string.Empty : _LeAbon.LIBELLEFORFAIT;

                this.Txt_CodeTarif.Text = string.IsNullOrEmpty(_LeAbon.TYPETARIF) ? string.Empty : _LeAbon.TYPETARIF;
                this.Txt_LibelleTarif.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLETARIF) ? _LeAbon.LIBELLETARIF : string.Empty;

                this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(_LeAbon.PERFAC) ? string.Empty : _LeAbon.PERFAC;
                this.Txt_LibelleFrequence.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEFREQUENCE) ? _LeAbon.LIBELLEFREQUENCE : string.Empty;

                this.Txt_CodeMoisIndex.Text = string.IsNullOrEmpty(_LeAbon.MOISREL) ? string.Empty : _LeAbon.MOISREL;
                this.Txt_LibelleMoisIndex.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEMOISIND) ? _LeAbon.LIBELLEMOISIND : string.Empty;

                this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(_LeAbon.MOISFAC) ? string.Empty : _LeAbon.MOISFAC;
                this.Txt_LibMoisFact.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEMOISFACT) ? _LeAbon.LIBELLEMOISFACT : string.Empty;

                this.Txt_DateAbonnement.Text = (_LeAbon.DABONNEMENT == null) ? string.Empty : Convert.ToDateTime(_LeAbon.DABONNEMENT.Value).ToShortDateString();
            }
        }   
        private void RemplireOngletAdresse(CsAg _LeAdresse)
        {
            try
            {
                if (_LeAdresse != null)
                {

                    this.tab3_txt_NomClientBrt.Text = string.IsNullOrEmpty(_LeAdresse.NOMP) ? string.Empty : _LeAdresse.NOMP;
                    this.tab3_txt_LibelleCommune.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLECOMMUNE) ? string.Empty : _LeAdresse.LIBELLECOMMUNE;
                    this.tab3_txt_LibelleQuartier.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLEQUARTIER) ? string.Empty : _LeAdresse.LIBELLEQUARTIER;
                    this.tab3_txt_Secteur.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLESECTEUR) ? string.Empty : _LeAdresse.LIBELLESECTEUR;
                    this.tab3_txt_NomRue.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLERUE) ? string.Empty : _LeAdresse.LIBELLERUE;
                    this.tab3_txt_NumRue.Text = string.IsNullOrEmpty(_LeAdresse.RUE) ? string.Empty : _LeAdresse.RUE;

                    this.tab3_txt_etage.Text = string.IsNullOrEmpty(_LeAdresse.ETAGE) ? string.Empty : _LeAdresse.ETAGE;
                    this.tab3_txt_NumLot.Text = string.IsNullOrEmpty(_LeAdresse.CADR) ? string.Empty : _LeAdresse.CADR;
                    //this.tab3_txt_Email.Text = string.IsNullOrEmpty(_LeAdresse.EMAIL) ? string.Empty : _LeAdresse.EMAIL;

                    this.tab3_txt_Telephone.Text = string.IsNullOrEmpty(_LeAdresse.TELEPHONE) ? string.Empty : _LeAdresse.TELEPHONE;
                    //this.tab3_txt_Fax.Text = string.IsNullOrEmpty(_LeAdresse.FAX) ? string.Empty : _LeAdresse.FAX;
                    this.tab3_txt_OrdreTour.Text = string.IsNullOrEmpty(_LeAdresse.ORDTOUR) ? string.Empty : _LeAdresse.ORDTOUR;
                    this.tab3_txt_tournee.Text = string.IsNullOrEmpty(_LeAdresse.TOURNEE) ? string.Empty : _LeAdresse.TOURNEE;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void btn_transmetre_Click(object sender, RoutedEventArgs e)
        {
            this.btn_transmetre.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            ValidationDemande(laDetailDemande );
        }
        private void PropietaireWindows(Visibility stat)
        {
            this.lbl_NomProprio.Visibility = stat;
            this.Txt_NomProprio_PersonePhysiq.Visibility = stat;
            this.lbl_PrenomProprio.Visibility = stat;
            this.Txt_PrenomProprio_PersonePhysiq.Visibility = stat;
            this.lbl_DateNaissanceProprio.Visibility = stat;
            this.dtp_DateNaissanceProprio.Visibility = stat;
            this.lbl_NaturePieceIdentiteProprio.Visibility = stat;
            this.Cbo_TypePiecePersonnePhysiqueProprio.Visibility = stat;
            this.lbl_NumPieceProprio.Visibility = stat;
            this.txtNumeroPieceProprio.Visibility = stat;
            this.lbl_DateFinValiditeProprio.Visibility = stat;
            this.dtp_finvalidationProprio.Visibility = stat;
            this.txt_Telephone_Proprio.Visibility = stat;
            this.Txt_Email_Proprio.Visibility = stat;
            this.label7_Copy4.Visibility = stat;
            this.label7_Copy5.Visibility = stat;
            this.Txt_Faxe_Proprio.Visibility = stat;
            this.Txt_BoitePosta_Proprio.Visibility = stat;
            this.lbl_Nationalite_Copy1.Visibility = stat;
            this.Cbo_Nationalite_Proprio.Visibility = stat;
            this.label7_Copy6.Visibility = stat;
            this.label7_Copy7.Visibility = stat;
        }
        private void ValidationDemande(CsDemande _Lademande)
        {
            try
            {

                _Lademande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                _Lademande.LaDemande.DATEMODIFICATION = System.DateTime.Now;

                if (laDetailDemande.LstCanalistion == null)
                    laDetailDemande.LstCanalistion = new List<CsCanalisation>();
                if (this.dtpPose.SelectedDate == null && string.IsNullOrEmpty(this.TxtperiodePose.Text) && string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text ))
                {
                    Message.Show("Verifier que les champs : date de pose,periode et typebranchement sont renseignés", "Creation ");
                    return;
                }
                if (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT && Cbo_Puissance.SelectedItem ==null )
                    Message.Show("Veillez sélectionnez la puissance installée", "Creation ");


                laDetailDemande.Branchement = new CsBrt() 
                {
                    CENTRE = laDetailDemande.LaDemande.CENTRE,
                    CLIENT = laDetailDemande.LaDemande.CLIENT,
                    NUMDEM  = laDetailDemande.LaDemande.NUMDEM ,
                    FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE,
                    FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID ,
                    FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT.Value,
                    PRODUIT = laDetailDemande.LaDemande.PRODUIT,
                    LONGBRT =string.IsNullOrEmpty( this.Txt_Distance.Text)? 1 : int.Parse(this.Txt_Distance.Text),
                    FK_IDTYPEBRANCHEMENT = this.Txt_TypeBrancehment.Tag == null ? laDetailDemande.Branchement.FK_IDTYPEBRANCHEMENT : int.Parse(this.Txt_TypeBrancehment.Tag.ToString()),
                    CODEBRT = string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text) ? null : this.Txt_TypeBrancehment.Text,
                    NBPOINT = (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT) ? 6 : 1,
                    USERCREATION = UserConnecte.matricule,
                    DATECREATION = System.DateTime.Now 
                };
                laDetailDemande.Branchement.PUISSANCEINSTALLEE = Cbo_Puissance.SelectedItem != null ? ((CsPuissance)Cbo_Puissance.SelectedItem).VALEUR  : 0;
                laDetailDemande.LstCanalistion.AddRange(((List<CsCanalisation>)this.dg_compteur.ItemsSource).ToList());
                laDetailDemande.LstCanalistion.ForEach(y => y.PERIODE = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.TxtperiodePose.Text));
                laDetailDemande.LstCanalistion.ForEach(y => y.POSE = this.dtpPose.SelectedDate);
                laDetailDemande.LstCanalistion.ForEach(y => y.DEPOSE  = null );
                laDetailDemande.LstCanalistion.ForEach(y => y.PROPRIO = "1");
                laDetailDemande.LstCanalistion.ForEach(y => y.FK_IDPROPRIETAIRE = 2);
                foreach (var leCompteur in laDetailDemande.LstCanalistion)
                {
                    CsEvenement leEvtPose = new CsEvenement();
                    leEvtPose.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
                    leEvtPose.CENTRE = laDetailDemande.LaDemande.CENTRE;
                    leEvtPose.CLIENT = laDetailDemande.LaDemande.CLIENT;
                    leEvtPose.ORDRE = laDetailDemande.LaDemande.ORDRE;
                    leEvtPose.PRODUIT = laDetailDemande.LaDemande.PRODUIT;
                    if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    {
                        leCompteur.POINT = 1;
                        leEvtPose.POINT = 1;
                    }
                    else
                        leEvtPose.POINT = leCompteur.POINT;

                    leEvtPose.FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT.Value;
                    leEvtPose.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                    leEvtPose.MATRICULE = laDetailDemande.LaDemande.MATRICULE;

                    leEvtPose.REGLAGECOMPTEUR = leCompteur.REGLAGECOMPTEUR;
                    leEvtPose.TYPECOMPTEUR = leCompteur.TYPECOMPTEUR;
                    leEvtPose.COMPTEUR = leCompteur.NUMERO;
                    leEvtPose.CATEGORIE = laDetailDemande.LeClient.CATEGORIE;
                    leEvtPose.USERCREATION = UserConnecte.matricule;
                    leEvtPose.USERMODIFICATION = UserConnecte.matricule;
                    leEvtPose.DATECREATION = System.DateTime.Now;
                    leEvtPose.DATEMODIFICATION = System.DateTime.Now;
                    leEvtPose.CAS = leCompteur.CAS;
                    leEvtPose.FK_IDCANALISATION = null;
                    leEvtPose.FK_IDABON = null;

                    leEvtPose.CODEEVT = SessionObject.Enumere.EvenementCodeNormale;
                    leEvtPose.STATUS = SessionObject.Enumere.EvenementPurger ;
                    leEvtPose.DATEEVT = leCompteur.POSE;
                    leEvtPose.INDEXEVT = leCompteur.INDEXEVT;
                    leEvtPose.INDEXPRECEDENTEFACTURE = leCompteur.INDEXEVT;
                    leEvtPose.PERIODE = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(leCompteur.PERIODE);
                    leEvtPose.PERIODEPRECEDENTEFACTURE = leEvtPose.PERIODE;
                    leEvtPose.TYPECOMPTAGE = laDetailDemande.Branchement.TYPECOMPTAGE;

                    leEvtPose.COEFK1 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE1;
                    leEvtPose.COEFK2 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE2;
                    if (laDetailDemande.LstEvenement != null && laDetailDemande.LstEvenement.Count != 0)
                    {
                        CsEvenement _LaCan = laDetailDemande.LstEvenement.FirstOrDefault(p => p.CAS == leEvtPose.CAS && p.POINT == leEvtPose.POINT);
                        if (_LaCan != null)
                        {
                            _LaCan.DATEEVT = leEvtPose.DATEEVT;
                            _LaCan.INDEXEVT = leCompteur.INDEXEVT;
                            _LaCan.PERIODE = leEvtPose.PERIODE;
                        }
                        else
                            laDetailDemande.LstEvenement.Add(leEvtPose);
                    }
                    else
                    {
                        laDetailDemande.LstEvenement = new List<CsEvenement>();
                        laDetailDemande.LstEvenement.Add(leEvtPose);
                    }
                }
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ValiderCreationAsync(_Lademande);
                service1.ValiderCreationCompleted += (srs, ress) =>
                {
                    this.btn_transmetre.IsEnabled = true;
                    this.CancelButton.IsEnabled = true;
                    if (ress.Result)
                    {
                        Message.ShowInformation("Client créé avec succès", "Demande");
                        this.DialogResult = false;
                    }
                };
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        private CsDemande GetDemandeDevisFromScreen(CsDemande pDemandeDevis, bool isTransmettre)
        {
            try
            {
                if (pDemandeDevis == null)
                {
                    pDemandeDevis = new CsDemande();
                    pDemandeDevis.LaDemande = new CsDemandeBase();
                    pDemandeDevis.Abonne = new CsAbon();
                    pDemandeDevis.Ag = new CsAg();
                    pDemandeDevis.Branchement = new CsBrt();
                    pDemandeDevis.LeClient = new CsClient();
                    pDemandeDevis.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                    pDemandeDevis.AppareilDevis = new List<ObjAPPAREILSDEVIS>();
                    pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                    pDemandeDevis.LaDemande.USERCREATION = UserConnecte.matricule;
                    pDemandeDevis.LaDemande.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                }
                #region Demande

                if (pDemandeDevis.LaDemande == null) pDemandeDevis.LaDemande = new CsDemandeBase();
                if (pDemandeDevis.LaDemande.TYPEDEMANDE != SessionObject.Enumere.BranchementAbonement)
                    pDemandeDevis.LaDemande.CLIENT = string.IsNullOrEmpty(this.Txt_ReferenceClient.Text) ? string.Empty : this.Txt_ReferenceClient .Text;

                pDemandeDevis.LaDemande.DATEMODIFICATION = DateTime.Now;
                //pDemandeDevis.LaDemande.MOTIF = txt_Motif.Text;
 
                pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t=>t.CODE == SessionObject.Enumere.TransfertAbonnement).PK_ID ;
                pDemandeDevis.LaDemande.TYPEDEMANDE = SessionObject.Enumere.TransfertAbonnement;

                #endregion
                pDemandeDevis.LaDemande.ISNEW = true;
                pDemandeDevis.LaDemande.ORDRE = laDetailDemande.LeClient.ORDRE;
                pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                pDemandeDevis.LaDemande.PRODUIT = laDetailDemande.Abonne.PRODUIT;
                pDemandeDevis.LaDemande.FK_IDPRODUIT = laDetailDemande.Abonne.FK_IDPRODUIT;
                pDemandeDevis.LaDemande.CENTRE = laDetailDemande.Abonne.CENTRE;
                pDemandeDevis.LaDemande.FK_IDCENTRE = laDetailDemande.Abonne.FK_IDCENTRE;
                

                return pDemandeDevis;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public const string MatchEmailPattern =
@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
+ @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
+ @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
+ @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";
        public static bool IsEmail(string email)
        {
            if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }

        private void cbo_typedoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSite_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtCentre_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            this.tabNewClient.Visibility = System.Windows.Visibility.Visible;
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            this.tabNewClient.Visibility = System.Windows.Visibility.Collapsed ;

        }

        private void Txt_Capital_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal capital = 0;
            if (!decimal.TryParse(Txt_Capital.Text, out capital))
            {
                Message.Show("veuillez saisir une valeur numerique", "Demande");
            }
            
        }


        private void Txt_DateFinvalidite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Shared.ClasseMEthodeGenerique.IsDateValide(Txt_DateFinvalidite.Text) && Txt_DateFinvalidite.Text.Length == SessionObject.Enumere.TailleDate)
            {
                Message.ShowError("La date n'est pas valide", "Accueil");
                this.Txt_DateFinvalidite.Focus();
            }

        }

        private void Txt_DateNaissance_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Shared.ClasseMEthodeGenerique.IsDateValide(Txt_DateNaissance.Text) && Txt_DateNaissance.Text.Length == SessionObject.Enumere.TailleDate)
            {
                Message.ShowError("La date n'est pas valide", "Accueil");
                this.Txt_DateNaissance.Focus();
            }
        }

        private void chk_Email_Checked(object sender, RoutedEventArgs e)
        {
            if (!chk_Email.IsChecked.Value)
            {
                Txt_Email.Text = string.Empty;
            }
            Txt_Email.IsEnabled = chk_Email.IsChecked.Value;
        }

        private void Txt_Email_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Cbo_Type_Proprietaire_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cbo_Type_Client.SelectedItem == null)
            {
                Message.ShowInformation("Selectionnez le type de client", "Demande");
                return;
            }
            if (Cbo_Type_Proprietaire.SelectedItem != null)
            {
                var typeproprio = (CsProprietaire)Cbo_Type_Proprietaire.SelectedItem;
                if (typeproprio.CODE == "1" || typeproprio.CODE == "2")
                {
                    tab_proprio.Visibility = Visibility.Visible;
                    PropietaireWindows(System.Windows.Visibility.Visible);
                    this.tbControleClient.SelectedItem = tab_proprio;
                }
                else
                {
                    tab_proprio.Visibility = Visibility.Collapsed;
                    PropietaireWindows(System.Windows.Visibility.Visible);
                    if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "001".Trim())
                        tbControleClient.SelectedItem = this.tabItemPersonnePhysique;

                    if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "002".Trim())
                        tbControleClient.SelectedItem = this.tabItemPersoneMoral;

                    if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "003".Trim())
                        tbControleClient.SelectedItem = this.tabItemPersoneAdministration;

                }
            }
        }

        private void Cbo_TypeClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CsTypeClient typeclient = ((CsTypeClient)Cbo_Type_Client.SelectedItem);
                switch (typeclient.CODE.Trim())
                {
                    case "001":
                        {
                            this.tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                            this.tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                            this.tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Visible;
                            tbControleClient.SelectedItem = this.tabItemPersonnePhysique;
                            break;
                        }
                    case "002":
                        {
                            tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Collapsed;
                            tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Collapsed;
                            tabItemPersoneMoral.Visibility = System.Windows.Visibility.Visible;
                            tbControleClient.SelectedItem = this.tabItemPersoneMoral;
                            break;
                        }
                    case "003":
                        {
                            tabItemPersonnePhysique.Visibility = System.Windows.Visibility.Collapsed;
                            tabItemPersoneMoral.Visibility = System.Windows.Visibility.Collapsed;
                            tabItemPersoneAdministration.Visibility = System.Windows.Visibility.Visible;
                            tbControleClient.SelectedItem = this.tabItemPersoneAdministration;
                            break;
                        }
                    default:
                        break;
                }
              
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                if (Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient)
                    ChargerClientFromReference(this.Txt_ReferenceClient.Text);
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
        private void VerifieExisteDemande(CsClient leClient)
        {

            try
            {
                if (!string.IsNullOrEmpty(Txt_ReferenceClient.Text) && Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient)
                {
                    string OrdreMax = string.Empty;
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneDemandeClientTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result != null)
                        {
                            if (args.Result.DATEFIN == null && args.Result.ISSUPPRIME != true)
                            {
                                Message.ShowInformation("Il existe une demande numero " + args.Result.NUMDEM + " sur ce client", "Accueil");
                                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                return;
                            }
                        }
                        CsClientRechercher leclientRech = new CsClientRechercher()
                        {
                            CENTRE = leClient.CENTRE,
                            CLIENT = leClient.REFCLIENT,
                            ORDRE = leClient.ORDRE,
                            FK_IDCENTRE = leClient.FK_IDCENTRE.Value,
                        };
                        ChargeDetailDEvis(leClient);
                    };
                    service.RetourneDemandeClientTypeAsync(leClient);
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerClientFromReference(string ReferenceClient)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.GetDemandeByRefClientCompleted += (s, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    if (args.Result != null )
                    {
                        laDetailDemande = new CsDemande();
                        laDetailDemande = args.Result;
                        Tdem = laDetailDemande.LaDemande.TYPEDEMANDE;
                        this.txtSite.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLESITE) ? string.Empty : laDetailDemande.LeClient.LIBELLESITE;
                        this.txtCentre.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLECENTRE) ? string.Empty : laDetailDemande.LeClient.LIBELLECENTRE;
                        this.txt_Produit.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEPRODUIT) ? string.Empty : laDetailDemande.Abonne.LIBELLEPRODUIT;
                        this.txt_Produit.Tag = laDetailDemande.LaDemande.FK_IDPRODUIT;
                        CodeProduit = laDetailDemande.LaDemande.PRODUIT ;
                        this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).LIBELLE;
                        txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem);
                        if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        {
                            this.Cbo_TypeComptage.Visibility = System.Windows.Visibility.Collapsed;
                            this.label_Puissance.Visibility = System.Windows.Visibility.Collapsed;
                            this.Cbo_Puissance.Visibility = System.Windows.Visibility.Collapsed;
                            this.label3.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else
                        {
                            if (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                            {
                                btn_typeCompteur.Visibility = Visibility.Collapsed;
                                Txt_LibelleTypeClient.Visibility = Visibility.Collapsed;
                                Txt_CodeTypeCompteur.Visibility = Visibility.Collapsed;
                                lbl_type.Visibility = Visibility.Collapsed;
                            }
                        }
                        if (laDetailDemande.LeClient != null )
                        RemplireOngletClient(laDetailDemande.LeClient);

                        if (laDetailDemande.Abonne != null)
                        RemplirOngletAbonnement(laDetailDemande.Abonne);

                        if (laDetailDemande.Ag != null)
                        RemplireOngletAdresse(laDetailDemande.Ag);
                    }
                    else 
                        Message.ShowInformation("Demande non trouvée","Demande");
                };
                service.GetDemandeByRefClientAsync(ReferenceClient, lstIdCentre);
                service.CloseAsync();

            }
            catch (Exception)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError("Erreur au chargement des données", "Demande");
            }
        }

        private void galatee_OkClickedChoixClient(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsClient _UnClient = (CsClient)ctrs.MyObject;
                _UnClient.TYPEDEMANDE = Tdem;
                VerifieExisteDemande(_UnClient);
            }
        }

        private List<CsClient> DistinctSiteClient(List<CsClient> lstClient)
        {
            try
            {
                List<CsClient> lstCentreDistClientOrdreProduit = new List<CsClient>();
                var lstCentreDistnct = lstClient.Select(t => new { t.LIBELLESITE, t.FK_IDCENTRE, t.CENTRE, t.REFCLIENT, t.PRODUIT }).Distinct().ToList();
                foreach (var item in lstCentreDistnct)
                {
                    CsClient leClient = new CsClient()
                    {
                        FK_IDCENTRE = item.FK_IDCENTRE,
                        CENTRE = item.CENTRE,
                        REFCLIENT = item.REFCLIENT,
                        PRODUIT = item.PRODUIT
                    };
                    lstCentreDistClientOrdreProduit.Add(leClient);
                }
                return lstCentreDistClientOrdreProduit;
            }
            catch (Exception)
            {

                throw;
            }
        }

        void ChargerDiametreCompteur()
        {
            try
            {
                if (SessionObject.LstCalibreCompteur.Count != 0)
                    LstCalibreCompteur = SessionObject.LstCalibreCompteur;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerCalibreCompteurCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstCalibreCompteur = args.Result;
                        SessionObject.LstCalibreCompteur = LstCalibreCompteur;
                    };
                    service.ChargerCalibreCompteurAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void ChargerMarque()
        {
            try
            {
                if (SessionObject.LstMarque.Count != 0)
                    LstMarque = SessionObject.LstMarque;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneToutMarqueCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstMarque = args.Result;
                        SessionObject.LstMarque = LstMarque;
                    };
                    service.RetourneToutMarqueAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void ChargerTypeComptage()
        {
            try
            {
                if (SessionObject.LstTypeComptage != null && SessionObject.LstTypeComptage.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeComptageCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTypeComptage = args.Result;
                };
                service.ChargerTypeComptageAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void ChargerTypeCompteur()
        {
            try
            {
                if (SessionObject.LstTypeCompteur.Count != 0)
                    LstTypeCompteur = SessionObject.LstTypeCompteur;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstTypeCompteur = args.Result;
                        SessionObject.LstTypeCompteur = LstTypeCompteur;
                    };
                    service.ChargerTypeAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_DiametreCompteur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btn_DiametreCompteur.IsEnabled = false;
                if (LstCalibreCompteur.Count != 0)
                {
                    if (CodeProduit == SessionObject.Enumere.ElectriciteMT)
                    {
                        List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstTypeComptage);
                        UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeDiametre);
                        ctr.Closed += new EventHandler(galatee_OkClickedBtntypeComptage);
                        ctr.Show();
                    }
                    else
                    {
                        List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCalibreCompteur.Where(t => t.PRODUIT == CodeProduit).ToList());
                        UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeDiametre);
                        ctr.Closed += new EventHandler(galatee_OkClickedBtnDiametre);
                        ctr.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedBtntypeComptage(object sender, EventArgs e)
        {
            try
            {
                this.btn_DiametreCompteur.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsTypeComptage _LeDiametre = (CsTypeComptage)ctrs.MyObject;
                    this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                    this.Txt_LibelleDiametre.Tag = _LeDiametre.PK_ID;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedBtnDiametre(object sender, EventArgs e)
        {
            try
            {
                this.btn_DiametreCompteur.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsCalibreCompteur _LeDiametre = (CsCalibreCompteur)ctrs.MyObject;
                    this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                    this.Txt_LibelleDiametre.Tag = _LeDiametre.PK_ID;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }

        private void Txt_CodeMarque_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeMarque.Text.Length == SessionObject.Enumere.TailleCodeMarqueCompteur && (LstMarque != null && LstMarque.Count != 0))
                {
                    CsMarqueCompteur _LaMarque = ClasseMEthodeGenerique.RetourneObjectFromList(LstMarque, this.Txt_CodeMarque.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LaMarque.LIBELLE))
                    {
                        this.Txt_LibelleMarque.Text = _LaMarque.LIBELLE;
                        this.Txt_CodeMarque.Tag = _LaMarque.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeMarque.Focus();
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }

        private void btn_Marque_Click(object sender, RoutedEventArgs e)
        {
            if (LstMarque.Count != 0)
            {
                this.btn_Marque.IsEnabled = false;
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstMarque);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_Marque);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_Marque(object sender, EventArgs e)
        {
            try
            {
                this.btn_Marque.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsMarqueCompteur _LaMarque = (CsMarqueCompteur)ctrs.MyObject;
                    this.Txt_CodeMarque.Text = _LaMarque.CODE;
                    this.Txt_CodeMarque.Tag = _LaMarque.PK_ID;

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }

        private void btn_typeCompteur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstTypeCompteur.Count != 0)
                {
                    this.btn_typeCompteur.IsEnabled = false;
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstTypeCompteur.Where(t => t.PRODUIT == CodeProduit).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "TYPE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedbtntypeCompteur);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedbtntypeCompteur(object sender, EventArgs e)
        {
            try
            {
                this.btn_typeCompteur.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsTcompteur _LeTypeCompteur = (CsTcompteur)ctrs.MyObject;
                    this.Txt_CodeTypeCompteur.Text = _LeTypeCompteur.CODE;
                    this.Txt_CodeTypeCompteur.Tag = _LeTypeCompteur.PK_ID;

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void Txt_CodeTypeCompteur_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeTypeCompteur.Text.Length == SessionObject.Enumere.TailleCodeTypeCompteur && (LstTypeCompteur != null && LstTypeCompteur.Count != 0))
                {
                    CsTcompteur _LeTypeCompte = ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeCompteur.Where(n => n.PRODUIT == CodeProduit).ToList(), this.Txt_CodeTypeCompteur.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeTypeCompte.LIBELLE))
                    {
                        this.Txt_LibelleTypeClient.Text = _LeTypeCompte.LIBELLE;
                        this.Txt_CodeTypeCompteur.Tag = _LeTypeCompte.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeTypeCompteur.Focus();
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void Cbo_TypeComptage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Cbo_TypeComptage.SelectedItem != null)
            {
                this.Txt_LibelleDiametre.Text = ((Galatee.Silverlight.ServiceAccueil.CsTypeComptage)this.Cbo_TypeComptage.SelectedItem).LIBELLE;
                this.Txt_LibelleDiametre.Tag = ((Galatee.Silverlight.ServiceAccueil.CsTypeComptage)this.Cbo_TypeComptage.SelectedItem).PK_ID ;
                this.btn_DiametreCompteur.IsEnabled = false;
            }
        }

        private void Cbo_Puissance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Txt_Distance_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Txt_CodeDiametre_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_Distance_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dtpPose_CalendarClosed(object sender, RoutedEventArgs e)
        {

        }
        List<CsCanalisation> lstCannalisation = new List<CsCanalisation>();
        private void btn_Ajouter_Click(object sender, RoutedEventArgs e)
        {
            if ( !string.IsNullOrEmpty(this.Txt_CodeCadran.Text) &&
                !string.IsNullOrEmpty(this.Txt_CodeMarque.Text) &&
                !string.IsNullOrEmpty(this.Txt_AnneeFab.Text) &&
                !string.IsNullOrEmpty(this.Txt_CodeCadran.Text) &&
                this.Txt_LibelleDiametre.Tag != null &&
                this.Txt_CodeMarque.Tag != null &&
                !string.IsNullOrEmpty(this.Txt_NumCompteur.Text))
            if (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
            {
                List < CsCanalisation > lstCanal = new List<CsCanalisation>();
                for (int i = 1; i <= 6; i++)
                {
                    Galatee.Silverlight.ServiceAccueil.CsCanalisation canal = new Galatee.Silverlight.ServiceAccueil.CsCanalisation()
                    {
                        CENTRE = laDetailDemande.LaDemande.CENTRE,
                        CLIENT = laDetailDemande.LaDemande.CLIENT,
                        ORDRE = laDetailDemande.LaDemande.ORDRE,
                        FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID,
                        FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE,
                        FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT.Value,
                        ANNEEFAB = this.Txt_AnneeFab.Text,
                        CADRAN = byte.Parse(this.Txt_CodeCadran.Text),
                        CAS = SessionObject.Enumere.CasPoseCompteur,
                        NUMDEM = laDetailDemande.LaDemande.NUMDEM,
                        NUMERO = this.Txt_NumCompteur.Text,
                        PRODUIT = laDetailDemande.LaDemande.PRODUIT , 
                        POINT = i,
                        INDEXEVT = 0,

                        FK_IDCALIBRE = (int)this.Txt_LibelleDiametre.Tag,
                        FK_IDMARQUECOMPTEUR = (int)this.Txt_CodeMarque.Tag,
                        //FK_IDTYPECOMPTEUR = (int)this.Txt_CodeTypeCompteur.Tag,
                        //LIBELLETYPECOMPTEUR = Txt_LibelleTypeClient.Text,
                        LIBELLEMARQUE = Txt_LibelleMarque.Text,
                        MARQUE = Txt_CodeMarque.Text ,
                        LIBELLEREGLAGECOMPTEUR = Txt_LibelleDiametre.Text,
                        FK_IDPROPRIETAIRE = SessionObject.Lsttypeprop.FirstOrDefault(t => t.CODE == SessionObject.Enumere.LOCATAIRE).PK_ID,

                        USERCREATION = UserConnecte.matricule,
                        USERMODIFICATION = UserConnecte.matricule,
                        DATECREATION = System.DateTime.Now,
                        DATEMODIFICATION = System.DateTime.Now,
                    };
                    lstCanal.Add(canal);
                }
                lstCannalisation = DataReferenceManager.CodificationCompteurMt(lstCanal);
            }
            else
            {
                Galatee.Silverlight.ServiceAccueil.CsCanalisation canal = new Galatee.Silverlight.ServiceAccueil.CsCanalisation()
                {
                    CENTRE = laDetailDemande.LaDemande.CENTRE,
                    CLIENT = laDetailDemande.LaDemande.CLIENT,
                    ORDRE = laDetailDemande.LaDemande.ORDRE,
                    FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE,
                    FK_IDDEMANDE  = laDetailDemande.LaDemande.PK_ID ,
                    FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT.Value,
                    ANNEEFAB = this.Txt_AnneeFab.Text,
                    CADRAN = byte.Parse(this.Txt_CodeCadran.Text),
                    CAS = SessionObject.Enumere.CasPoseCompteur,
                    NUMDEM = laDetailDemande.LaDemande.NUMDEM,
                    PRODUIT = laDetailDemande.LaDemande.PRODUIT,
                    NUMERO = this.Txt_NumCompteur.Text,
                    TYPECOMPTEUR = this.Txt_CodeTypeCompteur.Text ,
                    POINT = 1,
                    INDEXEVT  = 0,
                    MARQUE = Txt_CodeMarque.Text,
                    FK_IDCALIBRE = (int)this.Txt_LibelleDiametre.Tag,
                    FK_IDMARQUECOMPTEUR = (int)this.Txt_CodeMarque.Tag,
                    FK_IDTYPECOMPTEUR = (int)this.Txt_CodeTypeCompteur.Tag,
                    LIBELLETYPECOMPTEUR = Txt_LibelleTypeClient.Text,
                    LIBELLEMARQUE = Txt_LibelleMarque.Text,
                    LIBELLEREGLAGECOMPTEUR = Txt_LibelleDiametre.Text,
                    FK_IDPROPRIETAIRE = SessionObject.Lsttypeprop.FirstOrDefault(t => t.CODE == SessionObject.Enumere.LOCATAIRE).PK_ID,
                    USERCREATION = UserConnecte.matricule,
                    USERMODIFICATION = UserConnecte.matricule,
                    DATECREATION = System.DateTime.Now,
                    DATEMODIFICATION = System.DateTime.Now,
                };
                lstCannalisation.Add(canal);
            
            }
            this.dg_compteur.ItemsSource = null;
            this.dg_compteur.ItemsSource = lstCannalisation;
            }
     
        private void btn_typeDeBranchement_Click(object sender, RoutedEventArgs e)
        {
            this.btn_typeDeBranchement.IsEnabled = false;
            if (LstTypeBrt.Count != 0)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstTypeBrt.Where(u => u.PRODUIT == laDetailDemande.LaDemande.PRODUIT).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeDiametre);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnTypeBranchement);
                ctr.Show();
            }
        }

        void galatee_OkClickedBtnTypeBranchement(object sender, EventArgs e)
        {

            try
            {
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _LeDiametre = (Galatee.Silverlight.ServiceAccueil.CsTypeBranchement)ctrs.MyObject;
                    this.Txt_TypeBrancehment.Text = _LeDiametre.CODE;
                    this.Txt_TypeBrancehment.Tag = _LeDiametre.PK_ID;
                    this.Txt_LibelleTypeBranchement.Text = string.IsNullOrEmpty(_LeDiametre.LIBELLE) ? string.Empty : _LeDiametre.LIBELLE;
                }
                this.btn_typeDeBranchement.IsEnabled = true;

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
    
    }
}


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
//using Galatee.Silverlight.ServiceScelles;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmChangementCompteur : ChildWindow
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

        public FrmChangementCompteur()
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
            ChargerDiametreCompteur();
            ChargerMarque();
            ChargerTypeCompteur();
            RemplirProprietaire();
        }
        public FrmChangementCompteur(string TypeDemande)
        {
            InitializeComponent();
        }
        string Tdem = string.Empty;
        public FrmChangementCompteur(string TypeDemande, string IsInit)
        {
            InitializeComponent();
            Tdem = SessionObject.Enumere.TransfertAbonnement ;

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
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btn_transmetre.IsEnabled = false;
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        void Translate()
        {

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
                        laDetailDemande = args.Result;
                        this.txt_Produit.Text = laDetailDemande.Abonne.LIBELLEPRODUIT;
                        this.txtCentre.Text = laDetailDemande.LeClient.LIBELLECENTRE;
                        this.txtSite .Text = laDetailDemande.LeClient.LIBELLESITE ;
                        txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == "49").LIBELLE ;
                        RemplireOngletClient(laDetailDemande.LeClient);
                        CsDemandeBase leclien = new CsDemandeBase()
                        {
                            CENTRE = laDetailDemande.LeClient.CENTRE,
                            CLIENT = laDetailDemande.LeClient.REFCLIENT,
                            ORDRE = laDetailDemande.LeClient.ORDRE,
                            FK_IDCENTRE = laDetailDemande.LeClient.FK_IDCENTRE.Value,
                            PRODUIT = laDetailDemande.Abonne.PRODUIT
                        };
                        ChargerCompteurADeposer(leclien);
                        RetourneDernierEvtFacture(leclien);
                    }
                };
                client.GeDetailByFromClientAsync(leclient);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        List<CsEvenement> lesDernierEvt = new List<CsEvenement>();
        private void RetourneDernierEvtFacture(CsDemandeBase laDemande)
        {
            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.RetourneDernierEvtFactureCompleted += (ssender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    string error = args.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                if (args.Result == null && (args.Result!=null && args.Result.Count != 0))
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                else
                    lesDernierEvt = args.Result;
            };
            client.RetourneDernierEvtFactureAsync(laDemande);
        }
        private void ChargerCompteurADeposer(CsDemandeBase laDemande)
        {
            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.RetourneAncienCompteurCompleted += (ssender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    string error = args.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                if (args.Result == null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                else
                {
                    List<CsCanalisation> lesAncienCompteur = new List<CsCanalisation>();
                    lesAncienCompteur = args.Result;
                    if (lesAncienCompteur != null && lesAncienCompteur.Count() > 0)
                    {
                        CodeProduit = args.Result.First().PRODUIT;

                        if (CodeProduit == SessionObject.Enumere.ElectriciteMT)
                        {
                            btn_typeCompteur.Visibility = Visibility.Collapsed;
                            Txt_LibelleTypeClient.Visibility = Visibility.Collapsed;
                            Txt_CodeTypeCompteur.Visibility = Visibility.Collapsed;
                            lbl_type.Visibility = Visibility.Collapsed;

                        }
                    }
                    lesAncienCompteur.ForEach(t => t.ANCCOMPTEUR  = "000");
                    if (lesAncienCompteur.Count == 0)
                    {
                        Message.ShowInformation("Ce client n'a pas de compteur", "Resiliation");
                        return;
                    }
                    if (laDetailDemande.Abonne != null && laDetailDemande.Abonne.PRODUIT == SessionObject.Enumere.ElectriciteMT )
                    {
                        lesAncienCompteur.ForEach(u => u.TYPECOMPTAGE = laDetailDemande.Abonne.TYPECOMPTAGE);
                        lesAncienCompteur.ForEach(u => u.LIBELLEREGLAGECOMPTEUR  = laDetailDemande.Abonne.LIBELLETYPECOMPTAGE  );
                    }
                    dg_AncienCompteur.ItemsSource = lesAncienCompteur;

                    List<CsCanalisation> NouveauCompt = ClasseMEthodeGenerique.RetourneListCopy<CsCanalisation>(lesAncienCompteur);
                    NouveauCompt.ForEach(t => t.INDEXEVT = 0);
                    dg_compteur.ItemsSource = NouveauCompt;
                    List<CsCanalisation> lstAncCompt = lesAncienCompteur;
                    if (lstAncCompt != null && lstAncCompt.Count != 0)
                        lstAncCompt.ForEach(t => t.CAS = SessionObject.Enumere.CasDeposeCompteur);
                    TxtperiodeDepose.Text = string.IsNullOrEmpty(lstAncCompt.First().PERIODE) ? string.Empty : Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(lstAncCompt.First().PERIODE);
                }
            };
            client.RetourneAncienCompteurAsync(laDemande);
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

        private void btn_transmetre_Click(object sender, RoutedEventArgs e)
        {
            this.btn_transmetre.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            ValidationDemande(laDetailDemande );
        }
        private bool VerifieChampObligation()
        {
            try
            {
                bool ReturnValue = true;
                
                return ReturnValue;

            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Accueil");
                return false;
            }

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

                pDemandeDevis.LaDemande.ISNEW = true;
                pDemandeDevis.LaDemande.ORDRE = laDetailDemande.LeClient.ORDRE;
                pDemandeDevis.LaDemande.PRODUIT = laDetailDemande.Abonne.PRODUIT;
                pDemandeDevis.LaDemande.FK_IDPRODUIT = laDetailDemande.Abonne.FK_IDPRODUIT;
                pDemandeDevis.LaDemande.CLIENT = laDetailDemande.Abonne.CLIENT;
                pDemandeDevis.LaDemande.CENTRE = laDetailDemande.Abonne.CENTRE;
                pDemandeDevis.LaDemande.FK_IDCENTRE = laDetailDemande.Abonne.FK_IDCENTRE;
                pDemandeDevis.LaDemande.DATEMODIFICATION = DateTime.Now;
                if (txt_tdem.Tag != null)
                {
                    var typeDevis = (CsTdem)txt_tdem.Tag;
                    if (typeDevis != null)
                    {
                        pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = typeDevis.PK_ID;
                        pDemandeDevis.LaDemande.TYPEDEMANDE = typeDevis.CODE;
                        Tdem = typeDevis.CODE;
                    }
                }
                #endregion


                return pDemandeDevis;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ValidationDemande(CsDemande _Lademande)
        {
            try
            {

                bool IsValideToSave = true;
                var lstAncienCompteur = (List<CsCanalisation>)this.dg_AncienCompteur.ItemsSource;
                var lstNouveauCompteur = (List<CsCanalisation>)this.dg_compteur.ItemsSource;
                if ((lstAncienCompteur != null && lstAncienCompteur.Count() > 0) && (lstNouveauCompteur != null && lstNouveauCompteur.Count() > 0))
                {
                    foreach (var item in lstAncienCompteur)
                    {
                        if (item.DEPOSE == null || item.DEPOSE == new DateTime() || string.IsNullOrWhiteSpace(item.PERIODE)) IsValideToSave = false;
                    }
                    foreach (var item in lstNouveauCompteur)
                    {
                        if (item.POSE == null || item.POSE == new DateTime() || string.IsNullOrWhiteSpace(item.NUMERO) || string.IsNullOrWhiteSpace(item.PERIODE)) IsValideToSave = false;
                    }
                }
                else
                {
                    Message.ShowInformation("Veuillez vous assurer que le numéro et les dates de pose et de dépose soit bien renseignés", "Information");
                    return;
                }
                if (IsValideToSave == true)
                {
                    _Lademande = GetDemandeDevisFromScreen(null);
                    _Lademande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                    _Lademande.LaDemande.DATEMODIFICATION = System.DateTime.Now;
                    foreach (var leCompteur in (List<CsCanalisation>)this.dg_AncienCompteur.ItemsSource)
                    {
                        CsEvenement leEvtPose = new CsEvenement();
                        leEvtPose.NUMDEM = _Lademande.LaDemande.NUMDEM;
                        leEvtPose.CENTRE = _Lademande.LaDemande.CENTRE;
                        leEvtPose.CLIENT = _Lademande.LaDemande.CLIENT;
                        leEvtPose.ORDRE = _Lademande.LaDemande.ORDRE;
                        leEvtPose.PRODUIT = _Lademande.LaDemande.PRODUIT;
                        if (_Lademande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        {
                            leCompteur.POINT = 1;
                            leEvtPose.POINT = 1;
                        }
                        else
                            leEvtPose.POINT = leCompteur.POINT;

                        CsEvenement leEvtDuPoint = lesDernierEvt.FirstOrDefault(t => t.POINT == leCompteur.POINT);
                        int? ConsoPrecedent = leEvtDuPoint != null ? leEvtDuPoint.INDEXPRECEDENTEFACTURE : 0;
                        leEvtPose.CONSO = leCompteur.INDEXEVT - ConsoPrecedent;
                        leEvtPose.FK_IDCANALISATION = leEvtDuPoint.FK_IDCANALISATION;
                        leEvtPose.FK_IDABON = leEvtDuPoint.FK_IDABON;
                        leEvtPose.DATERELEVEPRECEDENTEFACTURE = leEvtDuPoint.DATERELEVEPRECEDENTEFACTURE;
                        leEvtPose.PERIODEPRECEDENTEFACTURE = leEvtDuPoint.PERIODEPRECEDENTEFACTURE;
                        leEvtPose.ORDREAFFICHAGE = leEvtDuPoint.ORDREAFFICHAGE;
                        leEvtPose.INDEXPRECEDENTEFACTURE = leEvtDuPoint.INDEXPRECEDENTEFACTURE;
                        leEvtPose.QTEAREG = leEvtDuPoint.QTEAREG;
                        leEvtPose.CASPRECEDENTEFACTURE = leEvtDuPoint.CASPRECEDENTEFACTURE;
                        leEvtPose.FK_IDCOMPTEUR = leEvtDuPoint.FK_IDCOMPTEUR;
                        leEvtPose.ORDTOUR = leEvtDuPoint.ORDTOUR;
                        leEvtPose.FK_IDTOURNEE = leEvtDuPoint.FK_IDTOURNEE;
                        leEvtPose.CODECONSO = leEvtDuPoint.CODECONSO;
                        leEvtPose.CATEGORIE = leEvtDuPoint.CATEGORIE;

                        leEvtPose.FK_IDPRODUIT = _Lademande.LaDemande.FK_IDPRODUIT.Value;
                        leEvtPose.FK_IDCENTRE = _Lademande.LaDemande.FK_IDCENTRE;
                        leEvtPose.MATRICULE = _Lademande.LaDemande.MATRICULE;

                        leEvtPose.REGLAGECOMPTEUR = leCompteur.REGLAGECOMPTEUR;
                        leEvtPose.TYPECOMPTEUR = leCompteur.TYPECOMPTEUR;
                        leEvtPose.COMPTEUR = leCompteur.NUMERO;
                        leEvtPose.CATEGORIE = laDetailDemande.LeClient.CATEGORIE;
                        leEvtPose.USERCREATION = UserConnecte.matricule;
                        leEvtPose.USERMODIFICATION = UserConnecte.matricule;
                        leEvtPose.DATECREATION = System.DateTime.Now;
                        leEvtPose.DATEMODIFICATION = System.DateTime.Now;
                        leEvtPose.CAS = SessionObject.Enumere.CasDeposeCompteur;
                        leEvtPose.FK_IDCANALISATION = leCompteur.PK_ID;
                        leEvtPose.FK_IDABON = null;

                        //leEvtPose.CODEEVT = SessionObject.Enumere.EvenementCodeNormale;
                        leEvtPose.STATUS = SessionObject.Enumere.EvenementReleve;
                        leEvtPose.DATEEVT = leCompteur.DEPOSE;
                        leEvtPose.INDEXEVT = leCompteur.INDEXEVT;
                        leEvtPose.PERIODE = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.TxtperiodeDepose.Text);
                        leEvtPose.TYPECOMPTAGE = leCompteur.TYPECOMPTAGE;

                        leEvtPose.COEFK1 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE1;
                        leEvtPose.COEFK2 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE2;
                        leEvtPose.CODEEVT = SessionObject.Enumere.EvenementCodeFactureIsole;
                        leEvtPose.LOTRI = leEvtPose.CENTRE + SessionObject.Enumere.LotriChangementCompteur;
                        if (_Lademande.LstEvenement != null && _Lademande.LstEvenement.Count != 0)
                        {
                            CsEvenement _LaCan = _Lademande.LstEvenement.FirstOrDefault(p => p.CAS == leEvtPose.CAS && p.POINT == leEvtPose.POINT);
                            if (_LaCan != null)
                            {
                                _LaCan.DATEEVT = leEvtPose.DATEEVT;
                                _LaCan.INDEXEVT = leCompteur.INDEXEVT;
                                _LaCan.PERIODE = leEvtPose.PERIODE;
                            }
                            else
                                _Lademande.LstEvenement.Add(leEvtPose);
                        }
                        else
                        {
                            _Lademande.LstEvenement = new List<CsEvenement>();
                            _Lademande.LstEvenement.Add(leEvtPose);
                        }
                       
                        //leEvtPose.STATUS = SessionObject.Enumere.EvenementReleve;
                    }
                    foreach (var leCompteur in (List<CsCanalisation>)this.dg_compteur.ItemsSource)
                    {
                        CsEvenement leEvtPose = new CsEvenement();
                        leEvtPose.NUMDEM = _Lademande.LaDemande.NUMDEM;
                        leEvtPose.CENTRE = _Lademande.LaDemande.CENTRE;
                        leEvtPose.CLIENT = _Lademande.LaDemande.CLIENT;
                        leEvtPose.ORDRE = _Lademande.LaDemande.ORDRE;
                        leEvtPose.PRODUIT = _Lademande.LaDemande.PRODUIT;
                        if (_Lademande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        {
                            leCompteur.POINT = 1;
                            leEvtPose.POINT = 1;
                        }
                        else
                            leEvtPose.POINT = leCompteur.POINT;


                        CsEvenement leEvtDuPoint = lesDernierEvt.FirstOrDefault(t => t.POINT == leCompteur.POINT);
                        int? ConsoPrecedent = leEvtDuPoint != null ? leEvtDuPoint.INDEXPRECEDENTEFACTURE : 0;
                        leEvtPose.CONSO = 0;
                        leEvtPose.FK_IDCANALISATION = leEvtDuPoint.FK_IDCANALISATION;
                        leEvtPose.FK_IDABON = leEvtDuPoint.FK_IDABON;
                        leEvtPose.DATERELEVEPRECEDENTEFACTURE = leEvtDuPoint.DATERELEVEPRECEDENTEFACTURE;
                        leEvtPose.PERIODEPRECEDENTEFACTURE = leEvtDuPoint.PERIODEPRECEDENTEFACTURE;
                        leEvtPose.ORDREAFFICHAGE = leEvtDuPoint.ORDREAFFICHAGE;
                        leEvtPose.INDEXPRECEDENTEFACTURE = leEvtDuPoint.INDEXPRECEDENTEFACTURE;
                        leEvtPose.QTEAREG = leEvtDuPoint.QTEAREG;
                        leEvtPose.CASPRECEDENTEFACTURE = leEvtDuPoint.CASPRECEDENTEFACTURE;
                        leEvtPose.FK_IDCOMPTEUR = leEvtDuPoint.FK_IDCOMPTEUR;
                        leEvtPose.ORDTOUR = leEvtDuPoint.ORDTOUR;
                        leEvtPose.FK_IDTOURNEE = leEvtDuPoint.FK_IDTOURNEE;
                        leEvtPose.CODECONSO = leEvtDuPoint.CODECONSO;
                        leEvtPose.CATEGORIE = leEvtDuPoint.CATEGORIE;

                        leEvtPose.FK_IDPRODUIT = _Lademande.LaDemande.FK_IDPRODUIT.Value;
                        leEvtPose.FK_IDCENTRE = _Lademande.LaDemande.FK_IDCENTRE;
                        leEvtPose.MATRICULE = _Lademande.LaDemande.MATRICULE;

                        leEvtPose.REGLAGECOMPTEUR = leCompteur.REGLAGECOMPTEUR;
                        leEvtPose.TYPECOMPTEUR = leCompteur.TYPECOMPTEUR;
                        leEvtPose.COMPTEUR = leCompteur.NUMERO;
                        leEvtPose.CATEGORIE = laDetailDemande.LeClient.CATEGORIE;
                        leEvtPose.USERCREATION = UserConnecte.matricule;
                        leEvtPose.USERMODIFICATION = UserConnecte.matricule;
                        leEvtPose.DATECREATION = System.DateTime.Now;
                        leEvtPose.DATEMODIFICATION = System.DateTime.Now;
                        leEvtPose.CAS = SessionObject.Enumere.CasPoseCompteur;
                        leEvtPose.FK_IDCANALISATION = leCompteur.PK_ID;
                        leEvtPose.FK_IDABON = null;

                        leEvtPose.CODEEVT = SessionObject.Enumere.EvenementCodeNormale;
                        leEvtPose.STATUS = SessionObject.Enumere.EvenementReleve;
                        leEvtPose.DATEEVT = leCompteur.POSE;
                        leEvtPose.INDEXEVT = leCompteur.INDEXEVT;
                        leEvtPose.PERIODE = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.TxtperiodePose.Text);
                        leEvtPose.TYPECOMPTAGE = leCompteur.TYPECOMPTAGE;

                        leEvtPose.COEFK1 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE1;
                        leEvtPose.COEFK2 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE2;
                        if (_Lademande.LstEvenement != null && _Lademande.LstEvenement.Count != 0)
                        {
                            CsEvenement _LaCan = _Lademande.LstEvenement.FirstOrDefault(p => p.CAS == leEvtPose.CAS && p.POINT == leEvtPose.POINT);
                            if (_LaCan != null)
                            {
                                _LaCan.DATEEVT = leEvtPose.DATEEVT;
                                _LaCan.INDEXEVT = leCompteur.INDEXEVT;
                                _LaCan.PERIODE = leEvtPose.PERIODE;
                            }
                            else
                                _Lademande.LstEvenement.Add(leEvtPose);
                        }
                        else
                        {
                            _Lademande.LstEvenement = new List<CsEvenement>();
                            _Lademande.LstEvenement.Add(leEvtPose);
                        }
                    }
                    _Lademande.LstCanalistion = new List<CsCanalisation>();
                    _Lademande.LstCanalistion.AddRange((List<CsCanalisation>)this.dg_compteur.ItemsSource);
                    _Lademande.LstCanalistion.ForEach(u => u.DATECREATION = System.DateTime.Now);
                    _Lademande.LstCanalistion.ForEach(u => u.USERCREATION = UserConnecte.matricule);

                    //Lancer la transaction de Mise à jour en base
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service1.ValiderDemandeInitailisationAsync(_Lademande);
                    service1.ValiderDemandeInitailisationCompleted += (sr, res) =>
                    {
                        this.btn_transmetre.IsEnabled = true;
                        this.CancelButton.IsEnabled = true;
                        string Retour = res.Result;
                        if (!string.IsNullOrEmpty(Retour))
                        {
                            string[] coupe = Retour.Split('.');
                            string numedemande = coupe[1];

                            _Lademande.LaDemande.NUMDEM = numedemande;
                            AcceuilServiceClient service11 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                            service11.InsererCompteurEvtTransitionAsync(_Lademande);
                            service11.InsererCompteurEvtTransitionCompleted += (ssr, ress) =>
                            {
                                if (ress.Result == true)
                                {
                                    Message.ShowInformation("Changement effectué avec succès", "Demande");
                                    this.DialogResult = false;
                                }
                            };
                            service11.CloseAsync();
                        }
                    };
                    service1.CloseAsync();
                }
                else
                {
                    Message.ShowInformation("Veuillez vous assurer que le numéro et les dates de pose et de dépose soit bien renseigné", "Information");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        private CsDemande GetDemandeDevisFromScreen(CsDemande pDemandeDevis )
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
                pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t=>t.CODE == "49").PK_ID ;
                pDemandeDevis.LaDemande.TYPEDEMANDE = "49";

                #endregion
                pDemandeDevis.LaDemande.ORDRE = laDetailDemande.LeClient.ORDRE;
                pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                pDemandeDevis.LaDemande.PRODUIT = laDetailDemande.Abonne.PRODUIT;
                pDemandeDevis.LaDemande.FK_IDPRODUIT = laDetailDemande.Abonne.FK_IDPRODUIT;
                pDemandeDevis.LaDemande.CENTRE = laDetailDemande.Abonne.CENTRE;
                pDemandeDevis.LaDemande.FK_IDCENTRE = laDetailDemande.Abonne.FK_IDCENTRE;
                pDemandeDevis.LaDemande.MATRICULE = UserConnecte.matricule ;

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
                service.RetourneClientByReferenceCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    if (args.Result != null && args.Result.Count > 1)
                    {
                        List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(args.Result);
                        Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CENTRE", "LIBELLESITE", "Liste des site");
                        ctr.Show();
                        ctr.Closed += new EventHandler(galatee_OkClickedChoixClient);
                    }
                    else
                    {
                        CsClient leClient = args.Result.First();
                        leClient.TYPEDEMANDE = Tdem;
                        VerifieExisteDemande(leClient);
                    }
                };
                service.RetourneClientByReferenceAsync(ReferenceClient, lstIdCentre);
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
                        UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE",Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeDiametre);
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
                    this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE ;
                    this.Txt_LibelleDiametre.Tag = _LeDiametre.PK_ID ;
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
        List<CsCanalisation> lstCannalisation = new List<CsCanalisation>();
        private void btn_Ajouter_Click(object sender, RoutedEventArgs e)
        {
            if (
                !string.IsNullOrEmpty(this.Txt_CodeCadran.Text) &&
                !string.IsNullOrEmpty(this.Txt_CodeMarque.Text) &&
                !string.IsNullOrEmpty(this.Txt_AnneeFab.Text) &&
                !string.IsNullOrEmpty(this.Txt_CodeCadran.Text) &&
                !string.IsNullOrEmpty(this.dtpPose.Text) &&
                this.Txt_LibelleDiametre.Tag != null &&
                this.Txt_CodeMarque.Tag != null &&
                !string.IsNullOrEmpty(this.TxtperiodePose.Text) &&
                !string.IsNullOrEmpty(this.Txt_NumCompteur.Text))
            {
                List<CsCanalisation> lstCanalisationNouveau = ((List<CsCanalisation>)this.dg_compteur.ItemsSource).ToList();
                CsCompteur nouveaucompteur = new CsCompteur();
                if (lstCanalisationNouveau.First().PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    nouveaucompteur.NUMERO = lstCanalisationNouveau.First().NUMERO.Substring(0, 5) + this.Txt_NumCompteur.Text;
                else
                    nouveaucompteur.NUMERO = this.Txt_NumCompteur.Text;
                nouveaucompteur.FK_IDMARQUECOMPTEUR = (int)this.Txt_CodeMarque.Tag;
                VerifieCompteurExiste(nouveaucompteur, ((List<CsCanalisation>)this.dg_compteur.ItemsSource).ToList());

                //foreach (CsCanalisation item in lstCanalisationNouveau)
                //{
                   

                //    item.NUMERO = nouveaucompteur.NUMERO;
                //    item.FK_IDCALIBRE = (int)this.Txt_LibelleDiametre.Tag;
                //    item.FK_IDMARQUECOMPTEUR = (int)this.Txt_CodeMarque.Tag;
                //    item.LIBELLEMARQUE = Txt_LibelleMarque.Text;
                //    item.LIBELLEREGLAGECOMPTEUR = Txt_LibelleDiametre.Text;
                //    item.FK_IDPROPRIETAIRE = SessionObject.Lsttypeprop.FirstOrDefault(t => t.CODE == SessionObject.Enumere.LOCATAIRE).PK_ID;
                //}
                //this.dg_compteur.ItemsSource = null;
                //this.dg_compteur.ItemsSource = lstCanalisationNouveau;
            }
            else
                Message.ShowInformation("Vérifier que les informations suivantes sont renseigné : cadran , marque,annee de facturation, date de pose,numero de compteur","Changement compteur");

        }

        private void VerifieCompteurExiste(CsCompteur nouveaucompteur,  List<CsCanalisation> lstCanalisationNouveau)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.VerifieCompteurExisteNewCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result==false)
	            {
                    foreach (CsCanalisation item in lstCanalisationNouveau)
                    {
                        if (lstCanalisationNouveau.First().PRODUIT == SessionObject.Enumere.ElectriciteMT)
                            item.NUMERO = item.NUMERO.Substring(0, 5) + this.Txt_NumCompteur.Text;
                        else
                            item.NUMERO = this.Txt_NumCompteur.Text;

                        item.FK_IDCALIBRE = (int)this.Txt_LibelleDiametre.Tag;
                        item.FK_IDMARQUECOMPTEUR = (int)this.Txt_CodeMarque.Tag;
                        item.LIBELLEMARQUE = Txt_LibelleMarque.Text;
                        item.LIBELLEREGLAGECOMPTEUR = Txt_LibelleDiametre.Text;
                        item.FK_IDPROPRIETAIRE = SessionObject.Lsttypeprop.FirstOrDefault(t => t.CODE == SessionObject.Enumere.LOCATAIRE).PK_ID;
                    }
                    this.dg_compteur.ItemsSource = null;
                    this.dg_compteur.ItemsSource = lstCanalisationNouveau;
                }
                else
                    Message.ShowInformation("Le numéro de compteur : " + nouveaucompteur.NUMERO+" existe déja.", "Changement compteur");
                
            };
            service.VerifieCompteurExisteNewAsync(nouveaucompteur);
            service.CloseAsync();
        }
        DateTime DtpPose = new DateTime();
        DateTime DtpDePose = new DateTime();
        private void dtpPose_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtpPose.SelectedDate != null && dtpPose.SelectedDate.Value != null && dg_compteur.ItemsSource != null)
            {
                List<CsCanalisation> lesCompteur = (List<CsCanalisation>)dg_compteur.ItemsSource;
                lesCompteur.ForEach(t => t.POSE = dtpPose.SelectedDate);
                //LoadCompteur(lesCompteur);
                DtpPose = dtpPose.SelectedDate.Value;
                this.TxtperiodePose.Text = dtpPose.SelectedDate.Value.Month.ToString("00") + "/" + dtpPose.SelectedDate.Value.Year;
                lesCompteur.ForEach(t => t.PERIODE = this.TxtperiodePose.Text);

            }
        }

        private void dtpDepose_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtpDepose.SelectedDate != null && dtpDepose.SelectedDate.Value != null && dg_AncienCompteur.ItemsSource != null)
            {
                List<CsCanalisation> lesCompteur = (List<CsCanalisation>)dg_AncienCompteur.ItemsSource;
                lesCompteur.ForEach(t => t.DEPOSE = dtpDepose.SelectedDate);
                //LoadCompteur(lesCompteur);
                DtpDePose = dtpDepose.SelectedDate.Value;
                this.TxtperiodeDepose.Text = dtpDepose.SelectedDate.Value.Month.ToString("00") + "/" + dtpDepose.SelectedDate.Value.Year;
                lesCompteur.ForEach(t => t.PERIODE = this.TxtperiodeDepose.Text);


            }
        }

      
    }
}


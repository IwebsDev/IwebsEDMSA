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
using Galatee.Silverlight.Devis.UCInitialisation;
using System.Text.RegularExpressions;

namespace Galatee.Silverlight.Accueil
{
    public partial class UcInitialisation : ChildWindow
    {
        private UcImageScanne formScanne = null;
        private Object ModeExecution = null;
        private List<CsTournee> _listeDesTourneeExistant = null;
        private List<CsCommune> _listeDesCommuneExistant = null;
        private List<CsCommune> _listeDesCommuneExistantCentre = null;
        private List< CsDiacomp> _listeDesDiametreExistant = null;
        private List<CsCentre> _listeDesCentreExistant = null;
        private  CsTdem _leTypeDemandeExistant = null;
        private string Tdem = null;
        private List<CsRues> _listeDesRuesExistant = null;
        private List<CsQuartier> _listeDesQuartierExistant = null;
        private ServiceDevis.cClient aClient = null;
        private List<ObjAPPAREILS> listAppareilsSelectionnes = null;
        bool isPreuveSelectionnee = false;
        private DataGrid _dataGrid = null;
        private List<CsUsage> lstusage = new List<CsUsage>();

        public string nom;
        public string prenom;
        public DateTime? datefinvalidité = new DateTime();
        public DateTime? datenaissance = new DateTime();
        public string numeropiece;
        public int? typepiece;

        public string NUMEROREGISTRECOMMERCE;
        public int? FK_IDSTATUTJURIQUE;
        public decimal? CAPITAL;
        public string IDENTIFICATIONFISCALE;
        public DateTime? DATECREATION = new DateTime();
        public string SIEGE;
        public string NOMMANDATAIRE;
        public string PRENOMMANDATAIRE;
        public string RANGMANDATAIRE;
        public string NOMSIGNATAIRE;
        public string PRENOMSIGNATAIRE;
        public string RANGSIGNATAIRE;




        public string DENOMINATION;
        public string NOMMANDATAIRE1;
        public string NOMSIGNATAIRE1;
        public string PRENOMMANDATAIRE1;
        public string PRENOMSIGNATAIRE1;
        public string RANGMANDATAIRE1;
        public string RANGSIGNATAIRE1;




        public UcInitialisation()
        {
            InitializeComponent();
        }

        public UcInitialisation(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid, string Tdem = "05")
        {
            try
            {
                InitializeComponent();


                tab_demande.Visibility = Visibility.Collapsed;

                this.Tdem = Tdem;
                label1.Visibility = Visibility.Collapsed;
                Txt_NumDevis.Visibility = Visibility.Collapsed;
                if (pExecMode != null) ModeExecution = pExecMode[0];


                RemplirProprietaire();
                ChargerTypeDocument();
                ChargerCategorieClient_TypeClient();
                ChargerNatureClient_TypeClient();
                ChargerUsage_NatureClient();
                ChargerCategorieClient_Usage();
                RemplirStatutJuridique();
                RemplirListeDesTypeDemandeExistant();
                RemplirTourneeExistante();
                RemplirCategorieClient();
                RemplirPieceIdentite();
                RemplirUsage();
                RemplirCodeRegroupement();
                RemplirCodeConsomateur();
                RemplirNatureClient();
                RemplirSecteur();
                RemplirNationnalite();
                RemplirCommune();
                RemplirListeDesQuartierExistant();
                RemplirListeDesRuesExistant();
                RemplirListeDesDiametresExistant();
                RemplirTypeClient();
                GestionDeFraixDevis();
                ChargerListDesSite();

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        int IdDemandeDevis = 0;
        CsDemandeBase laDemandeSelect = null;
        CsDemande laDetailDemande = null;
    
        private List<ObjDOCUMENTSCANNE> ObjetScanne = new List<ObjDOCUMENTSCANNE>();
        private List<ObjPIECEIDENTITE> ListeTYpePiece = new List<ObjPIECEIDENTITE>();
        private List<CsStatutJuridique> ListStatuJuridique = new List<CsStatutJuridique>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        public List<CsCATEGORIECLIENT_TYPECLIENT> LstCategorieClient_TypeClient = new List<CsCATEGORIECLIENT_TYPECLIENT>();
        public List<CsNATURECLIENT_TYPECLIENT> LstNatureClient_TypeClient = new List<CsNATURECLIENT_TYPECLIENT>();
        public List<CsUSAGE_NATURECLIENT> LstUsage_NatureClient = new List<CsUSAGE_NATURECLIENT>();
        public List<CsCATEGORIECLIENT_USAGE> LstCategorieClient_Usage = new List<CsCATEGORIECLIENT_USAGE>();
        public List<CsProprietaire> Lsttypeprop = new List<CsProprietaire>();

        public UcInitialisation(int _IdDemandeDevis, int PK_Id_Tdem = 0)
        {
            try
            {
                InitializeComponent();
                this.PK_Id_Tdem = PK_Id_Tdem;
                dtp_DateCreation.DisplayDateEnd = DateTime.Now;
                tab_demande.Visibility = Visibility.Collapsed;

                label21.Visibility = Visibility.Collapsed;
                Cbo_Diametre.Visibility = Visibility.Collapsed;

                label1.Visibility = Visibility.Visible;
                Txt_NumDevis.Visibility = Visibility.Visible;
                ModeExecution = SessionObject.ExecMode.Modification;
                IdDemandeDevis = _IdDemandeDevis;

                RemplirProprietaire();
                ChargerTypeDocument();
                ChargerCategorieClient_TypeClient();
                ChargerNatureClient_TypeClient();
                ChargerUsage_NatureClient();
                ChargerCategorieClient_Usage();
                RemplirStatutJuridique();
                RemplirTourneeExistante();
                RemplirCategorieClient();
                RemplirUsage();
                RemplirPieceIdentite();
                RemplirCodeRegroupement();
                RemplirCodeConsomateur();
                RemplirNatureClient();
                RemplirSecteur();
                RemplirNationnalite();
                RemplirCommune();
                RemplirTypeClient();
                RemplirListeDesQuartierExistant();
                RemplirListeDesRuesExistant();
                RemplirListeDesDiametresExistant();
                GestionDeFraixDevis();
                ChargerListDesSite();
                ChargeDetailDEvis(_IdDemandeDevis);
                RemplirListeDesTypeDemandeExistant();

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        public UcInitialisation(string Tdem)
        {
            try
            {
                InitializeComponent();
                this.Tdem = Tdem;
                ModeExecution = SessionObject.ExecMode.Creation;

                Txt_Porte.MaxLength = 5;
                Txt_Etage.MaxLength = 2;

                tab_demande.Visibility = Visibility.Collapsed;
                label1.Visibility = Visibility.Collapsed;
                Txt_NumDevis.Visibility = Visibility.Collapsed;
                ChargerTypeDocument();
                ChargerCategorieClient_TypeClient();
                ChargerNatureClient_TypeClient();
                ChargerUsage_NatureClient();
                ChargerCategorieClient_Usage();
                RemplirStatutJuridique();
                RemplirListeDesTypeDemandeExistant();
                RemplirTourneeExistante();
                RemplirCategorieClient();
                RemplirPieceIdentite();
                RemplirUsage();
                RemplirCodeRegroupement();
                RemplirCodeConsomateur();
                RemplirNatureClient();
                RemplirSecteur();
                RemplirNationnalite();
                RemplirCommune();
                RemplirListeDesQuartierExistant();
                RemplirListeDesRuesExistant();
                RemplirListeDesDiametresExistant();
                RemplirTypeClient();
                GestionDeFraixDevis();
                RemplirProprietaire();
                ChargerListDesSite();

                //Activation de la zone de recherche en fonction du type de demande
                ActivationEnFonctionDeTdem();

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void ActiverZoneRecherche(string p)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChargerTypeDocument()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerTypeDocumentCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstTypeDocument.Add(item);
                    }
                    cbo_typedoc.ItemsSource = LstTypeDocument;
                    cbo_typedoc.DisplayMemberPath = "LIBELLE";
                    cbo_typedoc.SelectedValuePath = "PK_ID";
                };
                service.ChargerTypeDocumentAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChargerCategorieClient_TypeClient()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));

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

        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            client.GetDemandeByNumIdDemandeAsync(IdDemandeDevis);
            client.GetDemandeByNumIdDemandeCompleted += (ssender, args) =>
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
                    laDetailDemande = args.Result;
                    Tdem = laDetailDemande.LaDemande.TYPEDEMANDE;
                    laDemandeSelect = laDetailDemande.LaDemande;
                    RenseignerChampsSurLeControl(laDetailDemande);
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
        }

        private void GestionDeFraixDevis()
        {
            //btnFind.IsEnabled = SessionObject.Enumere.IsPaiementFraixDevis;
            //ckbNoDeposit.IsEnabled = SessionObject.Enumere.IsPaiementFraixDevis;
            //GboVerificationAccompte.IsEnabled = SessionObject.Enumere.IsPaiementFraixDevis;
            //Txt_ReceiptNumber.IsEnabled = SessionObject.Enumere.IsPaiementFraixDevis;
            //btnCheck.IsEnabled = SessionObject.Enumere.IsPaiementFraixDevis;
            //Txt_Nom.IsEnabled = SessionObject.Enumere.IsPaiementFraixDevis;
            //txtAmountOfDeposit.IsEnabled = SessionObject.Enumere.IsPaiementFraixDevis;
            //Txt_DateOfDeposit.IsEnabled = SessionObject.Enumere.IsPaiementFraixDevis;

            EnabledDevisInformations(true);
        }

        private void Translate()
        {
            try
            {
                //this.ckbNoDeposit.Content = Languages.ckbNoDeposit;
                this.Btn_Annuler.Content = Languages.btnAnnuler;
                this.Title = Languages.ttlCreationDevis;
                //lab_ReceiptNumber.Content = Languages.NumeroRecu;
                //btnCheck.Content = Languages.btnRechercher;
                //lab_AmountOfDeposit.Content = Languages.MontantAccompte;
                //lab_Applicant.Content = Languages.Applicant;
                //lab_DateOfDeposit.Content = Languages.DateAccompte;
                //lnkLetter.Content = Languages.lnkLetter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValiderInitialisation(laDetailDemande, false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void ValiderInitialisation(CsDemande demandedevis, bool IsTransmetre)
        {

            try
            {
                // Get Devis informations from screen
                if (demandedevis != null  )
                    demandedevis = GetDemandeDevisFromScreen(demandedevis, false);
                else
                {
                   
                    demandedevis = GetDemandeDevisFromScreen(demandedevis, false);
                }
                // Get DemandeDevis informations from screen
                if (demandedevis != null)
                {
                    demandedevis.LaDemande.ETAPEDEMANDE = (int)DataReferenceManager.EtapeDevis.Accueil;
                    if (IsTransmetre)
                        demandedevis.LaDemande.ETAPEDEMANDE = null;
                    demandedevis.LaDemande.MATRICULE = UserConnecte.matricule;
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    client.ValiderDemandeInitailisationCompleted += (ss, b) =>
                    {
                        if (b.Cancelled || b.Error != null)
                        {
                            string error = b.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (IsTransmetre)
                        {
                            string Retour = b.Result;
                            string[] coupe = Retour.Split('.');
                            Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], demandedevis.LaDemande.FK_IDCENTRE, coupe[1], demandedevis.LaDemande.FK_IDTYPEDEMANDE);
                        }
                        FermerFenetre();
                    };
                    client.ValiderDemandeInitailisationAsync(demandedevis);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Une erreur c'est produite a la validation ", "ValiderDemandeInitailisation");
            }
        }
        private void FermerFenetre()
        {
            try
            {
                DialogResult = true;
                //Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Cbo_Site_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Site.SelectedItem != null)
                {
                    var csSite = Cbo_Site.SelectedItem as CsSite;
                    if (csSite != null)
                    {
                        this.txtSite.Text = csSite.CODE ?? string.Empty;
                        if (laDemandeSelect != null)
                        {
                            if (laDemandeSelect.FK_IDCENTRE != 0)
                                RemplirCentreDuSite(csSite.PK_ID, laDemandeSelect.FK_IDCENTRE);
                        }
                        else
                            RemplirCentreDuSite(csSite.PK_ID, 0);

                    }
                }
                VerifierDonneesSaisiesInformationsDevis();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        public static List<CsSite> RetourneSiteByCentre(List<CsCentre> _lstCentre)
        {
            try
            {
                // La grid doit afficher le detail d un recu par mode de reglement
                var leCentres = (from p in _lstCentre
                                 group new { p } by new { p.CODESITE, p.FK_IDCODESITE, p.LIBELLESITE } into pResult
                                 select new
                                 {
                                     pResult.Key.CODESITE,
                                     pResult.Key.FK_IDCODESITE,
                                     pResult.Key.LIBELLESITE
                                 });

                List<CsSite> _LstSite = new List<CsSite>();

                foreach (var r in leCentres.OrderByDescending(p => p.CODESITE))
                {
                    CsSite _leSite = new CsSite();
                    _leSite.CODE = r.CODESITE;
                    _leSite.PK_ID = r.FK_IDCODESITE;
                    _leSite.LIBELLE = r.LIBELLESITE;
                    _LstSite.Add(_leSite);
                }
                return _LstSite;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

   
        private void RemplirCentreDuSite(int pIdSite, int pIdcentre)
        {
            try
            {
                Cbo_Centre.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    List<CsCentre> lesCentreDuPerimetreAction = _listeDesCentreExistant.Where(p => p.FK_IDCODESITE == pIdSite).ToList();
                    if (lesCentreDuPerimetreAction != null)
                        foreach (var item in lesCentreDuPerimetreAction)
                        {
                            Cbo_Centre.Items.Add(item);
                        }
                    //Cbo_Centre.ItemsSource = lesCentreDuPerimetreAction;
                    Cbo_Centre.SelectedValuePath = "PK_ID";
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";

                    if (pIdcentre != 0  )
                        this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First(t => t.PK_ID == pIdcentre); 
                    if ( _listeDesCentreExistant.Count == 1)
                        this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirCentrePerimetre(List<CsCentre> lstCentre,List<CsSite> lstSite )
        {
            try
            {
                Cbo_Centre.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    if (lstCentre != null)
                        foreach (var item in lstCentre)
                        {
                            Cbo_Centre.Items.Add(item);
                        }
                    Cbo_Centre.SelectedValuePath = "PK_ID";
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";

                    if (lstSite != null)
                        foreach (var item in lstSite)
                        {
                            Cbo_Site.Items.Add(item);
                        }
                    Cbo_Site.SelectedValuePath = "PK_ID";
                    Cbo_Site.DisplayMemberPath = "LIBELLE";

                    if (lstSite != null && lstSite.Count == 1)
                        Cbo_Site.SelectedItem = lstSite.First();
                
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        void ChargerListDesSite()
        {
            try
            {

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    _listeDesCentreExistant = lesCentre; 
                    RemplirCentrePerimetre(lesCentre, lesSite);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                            RemplirCentrePerimetre(lesCentre, lesSite);
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
        private void RemplirListeDesDiametresExistant()
        {
            try
            {
                if (SessionObject.LstDiametreCompteur != null && SessionObject.LstDiametreCompteur.Count != 0)
                {
                    _listeDesDiametreExistant = SessionObject.LstDiametreCompteur;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerDiametreCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstDiametreCompteur = args.Result;
                    _listeDesDiametreExistant = SessionObject.LstDiametreCompteur;

                };
                service.ChargerDiametreCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RemplirListeDesTypeDemandeExistant()
        {
            try
            {
                if (SessionObject.LstTypeDemande != null && SessionObject.LstTypeDemande.Count != 0)
                {
                    if (!string.IsNullOrWhiteSpace(this.Tdem))
                    {
                        _leTypeDemandeExistant = SessionObject.LstTypeDemande.FirstOrDefault (t => t.CODE == this.Tdem);
                        txt_tdem.Text = _leTypeDemandeExistant.LIBELLE;
                        txt_tdem.Tag = _leTypeDemandeExistant;
                    }
                    LayoutRoot.Cursor = Cursors.Arrow;
                    return;
                }

                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service1.RetourneOptionDemandeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.LstTypeDemande = res.Result;
                    if (!string.IsNullOrWhiteSpace(this.Tdem))
                    {
                        _leTypeDemandeExistant = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == this.Tdem);
                        txt_tdem.Text = _leTypeDemandeExistant.LIBELLE;
                        txt_tdem.Tag = _leTypeDemandeExistant;
                    }
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                service1.RetourneOptionDemandeAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }

        //private void RemplirTournee(int pCentreId)
        //{
        //    List<ServiceDevis.CsTournee> lTourneeDuCentre = null;
        //    try
        //    {
        //        Cbo_Tournee.Items.Clear();
        //        if (_listeDesTourneeExistant != null &&
        //            _listeDesTourneeExistant.FirstOrDefault(p => p.FK_IDCENTRE == pCentreId) != null)
        //        {
        //            lTourneeDuCentre = _listeDesTourneeExistant.Where(q => q.FK_IDCENTRE == pCentreId).ToList();
        //        }
        //        if (lTourneeDuCentre != null && lTourneeDuCentre.Count > 0)
        //            foreach (var item in lTourneeDuCentre)
        //            {
        //                Cbo_Tournee.Items.Add(item);
        //            }

        //        Cbo_Tournee.SelectedValuePath = "PK_ID";
        //        Cbo_Tournee.DisplayMemberPath = "CODE";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void RemplirProduitCentre(CsCentre pCentre)
        {
            try
            {
                Cbo_Produit.ItemsSource = null;
                Cbo_Produit.ItemsSource = pCentre.LESPRODUITSDUSITE;
                Cbo_Produit.SelectedValuePath = "PK_ID";
                Cbo_Produit.DisplayMemberPath = "LIBELLE";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirCategorieClient()
        {
            try
            {
                if (SessionObject.LstCategorie != null && SessionObject.LstCategorie.Count != 0)
                {
                    Cbo_Categorie.Items.Clear();
                    ReloadCategClient(SessionObject.LstCategorie);
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneCategorieCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCategorie = args.Result;
                    Cbo_Categorie.Items.Clear();
                    ReloadCategClient(SessionObject.LstCategorie);
                };
                service.RetourneCategorieAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ReloadCategClient(List<CsCategorieClient> _lstCategorieClient)
        {
            Cbo_Categorie.ItemsSource = null;
            Cbo_Categorie.ItemsSource = _lstCategorieClient;
            Cbo_Categorie.SelectedValuePath = "PK_ID";
            Cbo_Categorie.DisplayMemberPath = "LIBELLE";
        }

        private void RemplirSecteur()
        {
            try
            {
                if (SessionObject.LstSecteur != null && SessionObject.LstSecteur.Count != 0)
                {
                    Cbo_Secteur.Items.Clear();
                    Cbo_Secteur.ItemsSource = SessionObject.LstSecteur;
                    Cbo_Secteur.SelectedValuePath = "PK_ID";
                    Cbo_Secteur.DisplayMemberPath = "LIBELLE";
                    return;
                }
                else
                {

                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerLesSecteursCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstSecteur = args.Result;
                        Cbo_Secteur.Items.Clear();
                        Cbo_Secteur.ItemsSource = SessionObject.LstSecteur;
                        Cbo_Secteur.SelectedValuePath = "PK_ID";
                        Cbo_Secteur.DisplayMemberPath = "LIBELLE";
                        return;
                    };
                    service.ChargerLesSecteursAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirNationnalite()
        {
            try
            {
                if (SessionObject.LstDesNationalites.Count != 0)
                {
                    Cbo_Nationalite.Items.Clear();
                    Cbo_Nationalite.ItemsSource = SessionObject.LstDesNationalites;
                    Cbo_Nationalite.SelectedValuePath = "PK_ID";
                    Cbo_Nationalite.DisplayMemberPath = "LIBELLE";

                    Cbo_Nationalite_Proprio.Items.Clear();
                    Cbo_Nationalite_Proprio.ItemsSource = SessionObject.LstDesNationalites;
                    Cbo_Nationalite_Proprio.SelectedValuePath = "PK_ID";
                    Cbo_Nationalite_Proprio.DisplayMemberPath = "LIBELLE";
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneNationnaliteCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstDesNationalites = args.Result;
                        Cbo_Nationalite.Items.Clear();
                        Cbo_Nationalite.ItemsSource = SessionObject.LstDesNationalites;
                        Cbo_Nationalite.SelectedValuePath = "PK_ID";
                        Cbo_Nationalite.DisplayMemberPath = "LIBELLE";


                        Cbo_Nationalite_Proprio.Items.Clear();
                        Cbo_Nationalite_Proprio.ItemsSource = SessionObject.LstDesNationalites;
                        Cbo_Nationalite_Proprio.SelectedValuePath = "PK_ID";
                        Cbo_Nationalite_Proprio.DisplayMemberPath = "LIBELLE";
                        return;
                    };
                    service.RetourneNationnaliteAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirProprietaire()
        {
            try
            {
                if (SessionObject.Lsttypeprop.Count != 0)
                {
                    Cbo_Type_Proprietaire.Items.Clear();
                    Cbo_Type_Proprietaire.ItemsSource = SessionObject.Lsttypeprop;
                    Cbo_Type_Proprietaire.SelectedValuePath = "PK_ID";
                    Cbo_Type_Proprietaire.DisplayMemberPath = "LIBELLE";
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RemplirProprietaireCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.Lsttypeprop = args.Result;
                        Cbo_Type_Proprietaire.Items.Clear();
                        Cbo_Type_Proprietaire.ItemsSource = SessionObject.Lsttypeprop;
                        Cbo_Type_Proprietaire.SelectedValuePath = "PK_ID";
                        Cbo_Type_Proprietaire.DisplayMemberPath = "LIBELLE";
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


        private void RemplirCodeConsomateur()
        {
            try
            {
                if (SessionObject.LstCodeConsomateur.Count != 0)
                {
                    Cbo_CodeConso.Items.Clear();
                    Cbo_CodeConso.ItemsSource = SessionObject.LstCodeConsomateur;
                    Cbo_CodeConso.SelectedValuePath = "PK_ID";
                    Cbo_CodeConso.DisplayMemberPath = "LIBELLE";
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneCodeConsomateurCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeConsomateur = args.Result;
                        Cbo_CodeConso.Items.Clear();
                        Cbo_CodeConso.ItemsSource = SessionObject.LstCodeConsomateur;
                        Cbo_CodeConso.SelectedValuePath = "PK_ID";
                        Cbo_CodeConso.DisplayMemberPath = "LIBELLE";
                        return;
                    };
                    service.RetourneCodeConsomateurAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirCodeRegroupement()
        {
            try
            {
                if (SessionObject.LstCodeRegroupement.Count != 0)
                {
                    Cbo_Regroupement.Items.Clear();
                    Cbo_Regroupement.ItemsSource = SessionObject.LstCodeRegroupement;
                    Cbo_Regroupement.SelectedValuePath = "PK_ID";
                    Cbo_Regroupement.DisplayMemberPath = "NOM";
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneCodeRegroupementCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeRegroupement = args.Result;
                        Cbo_Regroupement.Items.Clear();
                        Cbo_Regroupement.ItemsSource = SessionObject.LstCodeRegroupement;
                        Cbo_Regroupement.SelectedValuePath = "PK_ID";
                        Cbo_Regroupement.DisplayMemberPath = "NOM";
                    };
                    service.RetourneCodeRegroupementAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void RemplirNatureClient()
        {
            try
            {
                if (SessionObject.LstNatureClient.Count != 0)
                {
                    Cbo_NatureClient.Items.Clear();
                    ReloadNatureClient(SessionObject.LstNatureClient);
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneNatureCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        Cbo_NatureClient.Items.Clear();
                        SessionObject.LstNatureClient = args.Result;
                        ReloadNatureClient(SessionObject.LstNatureClient);

                        return;
                    };
                    service.RetourneNatureAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ReloadNatureClient(List<Galatee.Silverlight.ServiceAccueil.CsNatureClient> LstNatureClient)
        {
            //Cbo_NatureClient.Items.Clear();
            Cbo_NatureClient.ItemsSource = null;
            Cbo_NatureClient.ItemsSource = LstNatureClient;
            Cbo_NatureClient.SelectedValuePath = "PK_ID";
            Cbo_NatureClient.DisplayMemberPath = "LIBELLE";
        }

        private void RemplirPieceIdentite()
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.GetAllPieceIdentiteAsync();
                service.GetAllPieceIdentiteCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    Cbo_TypePiecePersonnePhysique.Items.Clear();
                    Cbo_TypePiecePersonnePhysiqueProprio.Items.Clear();
                    if (args.Result != null && args.Result.Count > 0)
                        foreach (var item in args.Result)
                        {
                            Cbo_TypePiecePersonnePhysique.Items.Add(item);
                            Cbo_TypePiecePersonnePhysiqueProprio.Items.Add(item);
                        }
                    ListeTYpePiece = args.Result;
                    Cbo_TypePiecePersonnePhysiqueProprio.SelectedValuePath = "PK_ID";
                    Cbo_TypePiecePersonnePhysiqueProprio.DisplayMemberPath = "LIBELLE";
                    Cbo_TypePiecePersonnePhysique.SelectedValuePath = "PK_ID";
                    Cbo_TypePiecePersonnePhysique.DisplayMemberPath = "LIBELLE";

                    VerifierTypePiece();
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region sylla le 22/09/2015


        private void RemplirUsage()
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.GetAllUsageAsync();
                service.GetAllUsageCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    Cbo_TypePiece.Items.Clear();
                    if (args.Result != null && args.Result.Count > 0)
                        lstusage = args.Result;
                    ReloadlstUsage(lstusage);

                    VerifierTypePiece();
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ReloadlstUsage(List<CsUsage> args)
        {
            Cbo_TypePiece.Items.Clear();
            Cbo_TypePiece.ItemsSource = null;
            foreach (var item in args)
            {
                Cbo_TypePiece.Items.Add(item);
            }

            Cbo_TypePiece.SelectedValuePath = "PK_ID";
            Cbo_TypePiece.DisplayMemberPath = "LIBELLE";
        }



        private void RemplirTypeClient()
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.GetAllTypeClientAsync();
                service.GetAllTypeClientCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    Cbo_Type_Client.Items.Clear();
                    SessionObject.LstTypeClient = args.Result;
                    if (args.Result != null && args.Result.Count > 0)
                        ReloadTypeClient(args.Result);

                    VerifierTypePiece();
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ReloadTypeClient(List<CsTypeClient> args)
        {
            Cbo_Type_Client.ItemsSource = null;
            foreach (var item in args)
            {
                Cbo_Type_Client.Items.Add(item);
            }
            Cbo_Type_Client.SelectedValuePath = "PK_ID";
            Cbo_Type_Client.DisplayMemberPath = "LIBELLE";
        }


        #endregion

        private void RemplirTourneeExistante()
        {
            try
            {

                if (SessionObject.LstZone != null && SessionObject.LstZone.Count != 0)
                {
                    _listeDesTourneeExistant = SessionObject.LstZone;
                    return;
                }
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerLesTourneesCompleted  += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstZone = args.Result;
                    _listeDesTourneeExistant = SessionObject.LstZone;

                };
                service.ChargerLesTourneesAsync ();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirCommune()
        {
            try
            {
                if (SessionObject.LstCommune != null && SessionObject.LstCommune.Count != 0)
                {
                    _listeDesCommuneExistant = SessionObject.LstCommune;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerCommuneAsync();
                service.ChargerCommuneCompleted  += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    SessionObject.LstCommune = args.Result;
                    _listeDesCommuneExistant = SessionObject.LstCommune;

                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirCommuneParCentre(CsCentre centre)
        {
            try
            {

                if (_listeDesCommuneExistant != null && _listeDesCommuneExistant.Count > 0)
                    _listeDesCommuneExistantCentre = _listeDesCommuneExistant.Where(c => c.FK_IDCENTRE == centre.PK_ID) != null ? _listeDesCommuneExistant.Where(c => c.FK_IDCENTRE == centre.PK_ID).ToList() : new List<CsCommune>();
                txt_Commune.Text = string.Empty;
                Cbo_Commune.ItemsSource = _listeDesCommuneExistantCentre;
                Cbo_Commune.IsEnabled = true;
                Cbo_Commune.SelectedValuePath = "PK_ID";
                Cbo_Commune.DisplayMemberPath = "LIBELLE";

                Cbo_Commune.ItemsSource = _listeDesCommuneExistantCentre;
                if (_listeDesCommuneExistantCentre.Count > 0)
                {
                    if (_listeDesCommuneExistantCentre.Count == 1)
                        Cbo_Commune.SelectedItem = _listeDesCommuneExistantCentre[0];
                }
                else
                {
                    Message.ShowError("Aucune commune associé à ce centre", "Info");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListeDesQuartierExistant()
        {
            try
            {

                if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
                {
                    _listeDesQuartierExistant = SessionObject.LstQuartier;
                    return;
                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerLesQartiersAsync();
                service.ChargerLesQartiersCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstQuartier = args.Result;
                    _listeDesQuartierExistant = SessionObject.LstQuartier;
                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirQuartier(int pCommuneId)
        {
            List<CsQuartier> ListeQuartierFiltres = new List<CsQuartier>();
            List<CsQuartier> QuartierParDefaut = null;

            this.txt_Quartier.Text = string.Empty;
            try
            {
                QuartierParDefaut = _listeDesQuartierExistant.Where(q => q.FK_IDCOMMUNE == pCommuneId).ToList();
                if (QuartierParDefaut != null && QuartierParDefaut.Count > 0)
                    ListeQuartierFiltres.AddRange(QuartierParDefaut);
                ListeQuartierFiltres.AddRange(_listeDesQuartierExistant.Where(q => q.FK_IDCOMMUNE == pCommuneId && q.CODE != DataReferenceManager.QuartierInconnu).ToList());

                if (ListeQuartierFiltres.Count > 0)
                    //foreach (var item in ListeQuartierFiltres)
                    //{
                    //    Cbo_Quartier.Items.Add(item);
                    //}
                    Cbo_Quartier.ItemsSource = null;
                Cbo_Quartier.ItemsSource = ListeQuartierFiltres;
                Cbo_Quartier.SelectedValuePath = "PK_ID";
                Cbo_Quartier.DisplayMemberPath = "LIBELLE";
                Cbo_Quartier.ItemsSource = ListeQuartierFiltres;

                //Cbo_Quartier.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirRues(int pIdCommune)
        {
            List<CsRues> ListeRuesFiltrees = new List<CsRues>();
            List<CsRues> RueParDefaut = null;
            this.txt_NumRue.Text = string.Empty;
            try
            {
                RueParDefaut = _listeDesRuesExistant.Where(q => q.CODE == DataReferenceManager.RueInconnue).ToList();
                if (RueParDefaut != null && RueParDefaut.Count > 0)
                    ListeRuesFiltrees.AddRange(RueParDefaut);
                ListeRuesFiltrees.AddRange(_listeDesRuesExistant.Where(q => q.PK_ID == pIdCommune && q.CODE != DataReferenceManager.RueInconnue).ToList());

                Cbo_Rue.ItemsSource = null;
                Cbo_Rue.ItemsSource = ListeRuesFiltrees;
                Cbo_Rue.SelectedValuePath = "PK_ID";
                Cbo_Rue.DisplayMemberPath = "LIBELLE";
                //Cbo_Rue.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListeDesRuesExistant()
        {
            try
            {

                if (SessionObject.LstRues != null && SessionObject.LstRues.Count != 0)
                {
                    _listeDesRuesExistant = SessionObject.LstRues;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerLesRueDesSecteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstRues = args.Result;
                    _listeDesRuesExistant = SessionObject.LstRues;
                };
                service.ChargerLesRueDesSecteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirDiametreCompteur(int pIdProduit)
        {
            List<CsDiacomp> ListeDesDiametreFiltrees = null;
            try
            {
                if (_listeDesDiametreExistant != null &&
                    _listeDesDiametreExistant.FirstOrDefault(p => p.FK_IDPRODUIT == pIdProduit) != null)
                {
                    ListeDesDiametreFiltrees = _listeDesDiametreExistant.Where(q => q.FK_IDPRODUIT == pIdProduit).ToList();
                }
                if (ListeDesDiametreFiltrees != null && ListeDesDiametreFiltrees.Count > 0)

                    Cbo_Diametre.ItemsSource = null;
                Cbo_Diametre.ItemsSource = ListeDesDiametreFiltrees;
                Cbo_Diametre.SelectedValuePath = "PK_ID";
                Cbo_Diametre.DisplayMemberPath = "LIBELLE";


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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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




        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void UcInitialisation_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    Txt_NumDevis.Text = string.Empty;
                }
                else if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    this.Btn_Annuler.IsEnabled = true;
                    this.Btn_Annuler.Content = Languages.btnFermer;
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                //this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_Produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Produit.SelectedItem != null)
                {
                    var produit = Cbo_Produit.SelectedItem as CsProduit;
                    if (produit != null)
                    {
                        this.txtProduit.Text = produit.CODE ?? string.Empty;
                        //RemplirTypeDevis(produit.PK_ID);
                        RemplirDiametreCompteur(produit.PK_ID);
                        if (produit.CODE == SessionObject.Enumere.Eau)
                        {
                            lab_ListeAppareils.Visibility = Visibility.Collapsed;
                            Cbo_ListeAppareils.Visibility = Visibility.Collapsed;
                            Btn_ListeAppareils.Visibility = Visibility.Collapsed;

                            #region Modif BSY 02-01-2016

                            label21.Margin = new Thickness(20, 228, 208, 0);
                            Cbo_Diametre.Margin = new Thickness(0, 228, 208, 0);

                            #endregion

                            label21.Visibility = Visibility.Collapsed;
                            Cbo_Diametre.Visibility = Visibility.Collapsed;
                        }

                        #region Modif BSY 02-01-2016

                        else if (produit.CODE == SessionObject.Enumere.ElectriciteMT)
                        {
                            lab_ListeAppareils.Visibility = Visibility.Collapsed;
                            Cbo_ListeAppareils.Visibility = Visibility.Collapsed;
                            Btn_ListeAppareils.Visibility = Visibility.Collapsed;

                            label21.Margin = new Thickness(20, 200, 208, 0);
                            Cbo_Diametre.Margin = new Thickness(0, 200, 208, 0);

                            label21.Visibility = Visibility.Collapsed ;
                            Cbo_Diametre.Visibility = Visibility.Collapsed ;

                        }

                        #endregion

                        else
                        {
                            lab_ListeAppareils.Visibility = Visibility.Visible;
                            Cbo_ListeAppareils.Visibility = Visibility.Visible;
                            Btn_ListeAppareils.Visibility = Visibility.Visible;

                            #region Modif BSY 02-01-2016

                            label21.Margin = new Thickness(20, 228, 208, 0);
                            Cbo_Diametre.Margin = new Thickness(0, 228, 208, 0);

                            #endregion

                            label21.Visibility = Visibility.Visible;
                            Cbo_Diametre.Visibility = Visibility.Visible;
                        }
                    }
                    if (txt_tdem.Tag!=null)
                    {
                        if (((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.AbonnementSeul || ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.BranchementAbonement || ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.BranchementAbonementExtention || ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.BranchementSimple)
                        {
                            Cbo_Type_Client.IsEnabled = true;
                        }
                        else
                        {
                            Cbo_Type_Client.IsEnabled = false;
                        }

                       
                    }
                    VerifierDonneesSaisiesInformationsDevis();
                    if (Tdem == SessionObject.Enumere.AugmentationPuissance || Tdem == SessionObject.Enumere.DimunitionPuissance)
                    {
                        tab_demande.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        tab_demande.Visibility = Visibility.Collapsed;

                    } 
                    
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void RenseignerChampsSurLeControl(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                    #region Demande
                    if (laDemande.LaDemande != null)
                    {
                        Txt_NumDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                        if (laDemande.LaDemande.FK_IDCENTRE != 0)
                        {
                            if (_listeDesCentreExistant != null && _listeDesCentreExistant.Count != 0)
                            {
                                if (_listeDesCentreExistant != null)
                                    foreach (var item in _listeDesCentreExistant)
                                    {
                                        Cbo_Centre.Items.Add(item);
                                    }


                                CsCentre leCentreSelect = _listeDesCentreExistant.First(t => t.PK_ID == laDemande.LaDemande.FK_IDCENTRE);
                                this.Cbo_Centre.SelectedItem = leCentreSelect;

                                List<CsSite> lstSite = RetourneSiteByCentre(_listeDesCentreExistant);
                                if (lstSite != null)
                                    foreach (var item in lstSite)
                                    {
                                        Cbo_Site.Items.Add(item);
                                    }
                                CsSite leSite = lstSite.FirstOrDefault(t => t.PK_ID == leCentreSelect.FK_IDCODESITE);
                                Cbo_Site.SelectedItem = leSite;
                            }

                        }
                        if (laDemande.LaDemande.FK_IDPRODUIT != 0)
                        {
                            foreach (CsProduit produit in Cbo_Produit.Items)
                            {
                                if (produit.PK_ID == laDemande.LaDemande.FK_IDPRODUIT)
                                {
                                    Cbo_Produit.SelectedItem = produit;
                                    txtProduit.Text = produit.CODE;
                                    break;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(laDemande.LaDemande.TYPEDEMANDE))
                        {
                            _leTypeDemandeExistant = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == laDemande.LaDemande.TYPEDEMANDE);
                            txt_tdem.Text = _leTypeDemandeExistant.LIBELLE;
                            txt_tdem.Tag = _leTypeDemandeExistant;
                            txt_motif.Text =!string.IsNullOrWhiteSpace( laDemande.LaDemande.MOTIF)?laDemande.LaDemande.MOTIF:string.Empty;
                        }
                    }

                    #endregion
                    #region Client
                    if (laDemande.LeClient != null)
                    {

                        if (laDemande.LeClient.FK_TYPECLIENT != 0)
                        {
                            CsTypeClient TypeClient = SessionObject.LstTypeClient.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_TYPECLIENT);
                            this.Cbo_Type_Client.SelectedItem = TypeClient;
                        }

                        foreach (CsCategorieClient categorieClient in Cbo_Categorie.Items)
                        {
                            if (categorieClient.PK_ID == laDemande.LeClient.FK_IDCATEGORIE)
                            {
                                Cbo_Categorie.SelectedItem = categorieClient;
                                break;
                            }
                        }

                        if (laDemande.LeClient.FK_IDNATURECLIENT != 0)
                        {
                            ServiceAccueil.CsNatureClient laNature = SessionObject.LstNatureClient.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_IDNATURECLIENT);
                            this.Cbo_NatureClient.SelectedItem = laNature;
                        }

                        if (laDemande.LeClient.FK_IDUSAGE != 0)
                        {
                            foreach (CsUsage typePiece in Cbo_TypePiece.Items)
                            {
                                if (typePiece.PK_ID == laDemande.LeClient.FK_IDUSAGE)
                                {
                                    Cbo_TypePiece.SelectedItem = typePiece;
                                    break;
                                }
                            }
                        }

                        if (laDemande.LeClient.FK_IDNATIONALITE != 0)
                        {
                            ServiceAccueil.CsNationalite laNation = SessionObject.LstDesNationalites.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_IDNATIONALITE);
                            this.Cbo_Nationalite.SelectedItem = laNation;
                        }
                        foreach (ObjPIECEIDENTITE piece in Cbo_TypePiecePersonnePhysique.Items)
                        {
                            if (piece.PK_ID == laDemande.LeClient.FK_IDPIECEIDENTITE)
                            {
                                Cbo_TypePiecePersonnePhysique.SelectedItem = piece;
                                break;
                            }
                        }

                        if (laDemande.LeClient.FK_IDCODECONSO != 0)
                        {
                            ServiceAccueil.CsCodeConsomateur codeConso = SessionObject.LstCodeConsomateur.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_IDCODECONSO);
                            this.Cbo_CodeConso.SelectedItem = codeConso;
                        }
                        if (laDemande.LeClient.FK_IDREGROUPEMENT != 0)
                        {
                            ServiceAccueil.CsRegCli regroup = SessionObject.LstCodeRegroupement.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_IDREGROUPEMENT);
                            this.Cbo_Regroupement.SelectedItem = regroup;
                        }
                        if (!string.IsNullOrWhiteSpace(laDemande.LeClient.PROPRIO))
                        {
                             CsProprietaire typeprop = SessionObject.Lsttypeprop.FirstOrDefault(t => t.CODE == laDemande.LeClient.PROPRIO);
                            this.Cbo_Type_Proprietaire.SelectedItem = typeprop;
                        }
                        #region Sylla le 24/9/2015

                        RemplirInfopersonnephysique(laDemande);
                        RemplirInfoSocietePrive(laDemande);
                        RemplirInfoAdmnistrationInstitut(laDemande);
                        RemplirInfoPropritaire(laDemande);
                        #endregion

                        Txt_NomClient.Text = !string.IsNullOrEmpty(laDemande.LeClient.FAX) ? laDemande.LeClient.FAX : string.Empty;
                        Txt_PrenomClient.Text = !string.IsNullOrEmpty(laDemande.LeClient.BOITEPOSTAL) ? laDemande.LeClient.BOITEPOSTAL : string.Empty;
                        TxtCategorieClient.Text = !string.IsNullOrEmpty(laDemande.LeClient.CATEGORIE) ? laDemande.LeClient.CATEGORIE : string.Empty;
                        //txtNumeroPiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMEROPIECEIDENTITE) ? laDemande.LeClient.NUMEROPIECEIDENTITE : string.Empty;
                        txtAdresse.Text = !string.IsNullOrEmpty(laDemande.LeClient.ADRMAND1) ? laDemande.LeClient.ADRMAND1 : string.Empty;
                        txt_Telephone.Text = !string.IsNullOrEmpty(laDemande.LeClient.TELEPHONE) ? laDemande.LeClient.TELEPHONE : string.Empty;
                        Txt_Numeronina.Text = !string.IsNullOrEmpty(laDemande.LeClient.CODEIDENTIFICATIONNATIONALE) ? laDemande.LeClient.CODEIDENTIFICATIONNATIONALE : string.Empty;
                        txt_ordre.Text = !string.IsNullOrEmpty(laDemande.LeClient.ORDRE) ? laDemande.LeClient.ORDRE : string.Empty;
                        Txt_Email.Text = !string.IsNullOrEmpty(laDemande.LeClient.EMAIL) ? laDemande.LeClient.EMAIL : string.Empty;
                        txt_Telephone_Fixe.Text = !string.IsNullOrEmpty(laDemande.LeClient.TELEPHONEFIXE) ? laDemande.LeClient.TELEPHONEFIXE : string.Empty;
                        chk_SMS.IsChecked = laDemande.LeClient.ISFACTURESMS != null ? laDemande.LeClient.ISFACTURESMS : false;
                        chk_Email.IsChecked = laDemande.LeClient.ISFACTUREEMAIL != null ? laDemande.LeClient.ISFACTUREEMAIL : false;

                    }
                    #endregion
                    #region Ag
                    if (laDemande.Ag != null)
                    {
                        txt_Commune.Text = !string.IsNullOrEmpty(laDemande.Ag.COMMUNE) ? laDemande.Ag.COMMUNE : string.Empty;
                        txt_Quartier.Text = !string.IsNullOrEmpty(laDemande.Ag.QUARTIER) ? laDemande.Ag.QUARTIER : string.Empty;
                        txt_NumRue.Text = !string.IsNullOrEmpty(laDemande.Ag.RUE) ? laDemande.Ag.RUE : string.Empty;
                        txtPropriete.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;

                        //txt_Telephone.Text = !string.IsNullOrEmpty(laDemande.Ag.TELEPHONE) ? laDemande.Ag.TELEPHONE : string.Empty;
                        //txt_NumLot.Text = !string.IsNullOrEmpty(laDemande.Ag.l) ? pInformationsDevis.DemandeDevis.NUMLOT : string.Empty;

                        if (laDemande.Ag.FK_IDCOMMUNE != null && laDemande.Ag.FK_IDCOMMUNE != 0)
                        {
                            foreach (CsCommune commune in Cbo_Commune.Items)
                            {
                                if (commune.PK_ID == laDemande.Ag.FK_IDCOMMUNE)
                                {
                                    Cbo_Commune.SelectedItem = commune;
                                    break;
                                }
                            }
                        }
                        if (laDemande.Ag.FK_IDQUARTIER != null && laDemande.Ag.FK_IDQUARTIER != 0)
                        {
                            foreach ( CsQuartier quartier in Cbo_Quartier.Items)
                            {
                                if (quartier.PK_ID == laDemande.Ag.FK_IDQUARTIER)
                                {
                                    Cbo_Quartier.SelectedItem = quartier;
                                    break;
                                }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(laDemande.Ag.RUE))
                        {
                            foreach ( CsRues rues in Cbo_Rue.Items)
                            {
                                if (rues.CODE == laDemande.Ag.RUE)
                                {
                                    Cbo_Rue.SelectedItem = rues;
                                    break;
                                }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(laDemande.Ag.SECTEUR))
                        {
                            foreach (Galatee.Silverlight.ServiceAccueil.CsSecteur secteur in Cbo_Secteur.Items)
                            {
                                if (secteur.CODE == laDemande.Ag.SECTEUR)
                                {
                                    Cbo_Secteur.SelectedItem = secteur;
                                    break;
                                }
                            }
                        }
                    }
                    Txt_Porte.Text = laDemande.Ag.PORTE != null ? laDemande.Ag.PORTE : string.Empty;
                    Txt_Etage.Text = laDemande.Ag.ETAGE != null ? laDemande.Ag.ETAGE : string.Empty;

                    #endregion
                    #region DocumentScanne
                    if (laDemande.ObjetScanne != null && laDemande.ObjetScanne.Count != 0)
                    {
                        isPreuveSelectionnee = true;
                        foreach (var item in laDemande.ObjetScanne)
                        {
                            LstPiece.Add(item);
                        }
                        dgListePiece.ItemsSource = this.LstPiece;
                    }
                    else
                    {
                        isPreuveSelectionnee = false;
                    }
                    #endregion
                    #region Apperiel
                    if (laDemande.AppareilDevis != null && laDemande.AppareilDevis.Count != 0)
                    {
                        Cbo_ListeAppareils.Items.Clear();
                        List<ObjAPPAREILS> lstAppareil = new List<ObjAPPAREILS>();
                        foreach (ObjAPPAREILSDEVIS item in laDemande.AppareilDevis)
                        {
                            ObjAPPAREILS Appareil = new ObjAPPAREILS()
                            {
                                CODEAPPAREIL = item.CODEAPPAREIL,
                                DESIGNATION = item.DESIGNATION,
                                NOMBRE = item.NBRE.Value,
                                PUISSANCE = item.PUISSANCE.Value,
                                TEMPSUTILISATION = item.TEMPSUTILISATION,
                                DISPLAYLABEL = item.DESIGNATION,
                                PK_IDAPPAREILDEVIS = item.FK_IDCODEAPPAREIL,
                                PK_ID = item.FK_IDCODEAPPAREIL
                            };

                            Cbo_ListeAppareils.Items.Add(Appareil);
                        }
                        Cbo_ListeAppareils.SelectedValuePath = "CODEAPPAREIL";
                        Cbo_ListeAppareils.DisplayMemberPath = "DESIGNATION";
                        Cbo_ListeAppareils.SelectedIndex = 0;
                    }
                    #endregion
                    #region brt
                    if (SessionObject.LstDiametreCompteur != null && SessionObject.LstDiametreCompteur.Count != 0 && laDemande.Branchement != null && !string.IsNullOrEmpty(laDemande.Branchement.DIAMBRT))
                        this.Cbo_Diametre.SelectedItem = SessionObject.LstDiametreCompteur.FirstOrDefault(t => t.CODE == laDemande.Branchement.DIAMBRT);
                    #endregion
                    #region Abon
                    if (laDemande.Abonne != null)
                    {
                        bool? mynull = null;
                        rbtn_AugtPiss.IsChecked = laDemande.Abonne.ISAUGMENTATIONPUISSANCE != null ? laDemande.Abonne.ISAUGMENTATIONPUISSANCE : mynull;
                        rbtn_DiminPuiss.IsChecked = laDemande.Abonne.ISDIMINUTIONPUISSANCE != null ? laDemande.Abonne.ISDIMINUTIONPUISSANCE : mynull;
                        txt_ancienPuiss.Text = laDemande.Abonne.PUISSANCE != null ? laDemande.Abonne.PUISSANCE.ToString() : string.Empty;
                        txt_nouvelPuiss.Text = laDemande.Abonne.NOUVELLEPUISSANCE != null ? laDemande.Abonne.NOUVELLEPUISSANCE.ToString() : string.Empty;
                    }

                    #endregion
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AbonnePicker_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (Galatee.Silverlight.Devis.UcAbonnePicker)sender;
                if (form != null)
                {
                    if (form.DialogResult == true && form.abonneSelectionne != null)
                    {
                        var abonne = form.abonneSelectionne;
                        aClient = abonne;
                        if (abonne != null)
                        {
                            this.txt_Commune.Text = abonne.COMMUNE;
                            if (!string.IsNullOrEmpty(abonne.COMMUNE.Trim()))
                            {
                                foreach (CsCommune commune in Cbo_Commune.Items)
                                {
                                    if (commune.CODE == abonne.COMMUNE)
                                    {
                                        Cbo_Commune.SelectedItem = commune;
                                        break;
                                    }
                                }
                            }
                            this.txt_Quartier.Text = abonne.QUARTIER;
                            if (!string.IsNullOrEmpty(abonne.QUARTIER.Trim()))
                            {
                                foreach (CsQuartier quartier in Cbo_Quartier.Items)
                                {
                                    if (quartier.CODE == abonne.QUARTIER)
                                    {
                                        Cbo_Quartier.SelectedItem = quartier;
                                        break;
                                    }
                                }
                            }
                            this.txt_NumRue.Text = abonne.RUE;
                            if (!string.IsNullOrEmpty(abonne.RUE.Trim()))
                            {
                                foreach (CsRues rue in Cbo_Rue.Items)
                                {
                                    if (rue.CODE == abonne.RUE)
                                    {
                                        Cbo_Rue.SelectedItem = rue;
                                        break;
                                    }
                                }
                            }
                            //txtNumeroPiece.Text = abonne.CNI;
                            txt_Telephone.Text = abonne.TELEPHONE;
                            foreach (CsCategorieClient categorieClient in Cbo_Categorie.Items)
                            {
                                if (abonne.CATEGORIECLIENT == categorieClient.CODE)
                                {
                                    Cbo_Categorie.SelectedItem = categorieClient;
                                    break;
                                }
                            }
                        }
                    }
                }
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MakeReadOnlyOrEnabledClientInformation(bool pValue)
        {
            try
            {
                Txt_NomClient.IsReadOnly = pValue;
                Txt_PrenomClient.IsReadOnly = pValue;
                //Cbo_Tournee.IsEnabled = pValue;
                txt_Commune.IsReadOnly = pValue;
                Cbo_Quartier.IsEnabled = pValue;
                Cbo_Rue.IsEnabled = pValue;
                //txtNumeroPiece.IsReadOnly = pValue;
                //txt_Telephone.IsReadOnly = pValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void VerifierDonneesSaisiesInformationsDevis()
        {
            try
            {
                //if (Cbo_Site.SelectedItem != null && Cbo_Centre.SelectedItem != null && Cbo_Produit.SelectedItem != null && Cbo_TypeDevis.SelectedItem != null && Cbo_Type_Client.SelectedItem != null)
                if (Cbo_Site.SelectedItem != null && Cbo_Centre.SelectedItem != null && Cbo_Produit.SelectedItem != null && Cbo_Type_Client.SelectedItem != null)

                    EnabledDemandeDevisInformations(true);
                else
                    EnabledDemandeDevisInformations(false);

                ActiverOuDesactiverEnFonctionTdem();

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void ActiverOuDesactiverEnFonctionTdem()
        {
            if (txt_tdem.Tag!=null)
            {
                if (((CsTdem)txt_tdem.Tag).CODE != SessionObject.Enumere.BranchementAbonement && ((CsTdem)txt_tdem.Tag).CODE != SessionObject.Enumere.AbonnementSeul && ((CsTdem)txt_tdem.Tag).CODE != SessionObject.Enumere.Reabonnement)
                {
                    EnabledDemandeDevisInformationsByTdem(false);
                }
                else
                {
                    EnabledDemandeDevisInformationsByTdem(true);
                } 
            }
        }

        

        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Centre.SelectedItem != null)
                {
                    CsCentre centre = Cbo_Centre.SelectedItem as  CsCentre;
                    if (centre != null)
                    {
                        this.txtCentre.Text = centre.CODE ?? string.Empty;
                        this.txtCentre.Tag = centre.PK_ID;
                        RemplirCommuneParCentre(centre);
                        RemplirProduitCentre(centre);
                    }
                    VerifierDonneesSaisiesInformationsDevis();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void EnabledDevisInformations(bool pValue)
        {
            try
            {
                Cbo_Site.IsEnabled = pValue;
                Cbo_Centre.IsEnabled = pValue;
                Cbo_Produit.IsEnabled = pValue;
                //Cbo_TypeDevis.IsEnabled = pValue;
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void EnabledDemandeDevisInformations(bool pValue)
        {
            try
            {
                //ckbIdentifiable.IsEnabled = pValue;
                //Btn_RechercherClient.IsEnabled = pValue;
                //hyperlinkButtonPropScannee.IsEnabled = pValue;
                dtp_finvalidationProprio.IsEnabled = pValue;
                txtNumeroPieceProprio.IsEnabled = pValue;
                //Cbo_TypePiecePersonnePhysiqueProprio.IsEnabled = pValue;
                dtp_DateNaissanceProprio.IsEnabled = pValue;
                Txt_PrenomProprio_PersonePhysiq.IsEnabled = pValue;
                Txt_NomProprio_PersonePhysiq.IsEnabled = pValue;
                txt_Telephone_Proprio.IsEnabled = pValue;
                Txt_Email_Proprio.IsEnabled = pValue;
                Txt_BoitePosta_Proprio.IsEnabled = pValue;
                Txt_Faxe_Proprio.IsEnabled = pValue;
                Cbo_Nationalite_Proprio.IsEnabled = pValue;
                Cbo_Type_Proprietaire.IsEnabled = pValue;
                //Cbo_Tournee.IsEnabled = pValue;
                Cbo_Categorie.IsEnabled = pValue;
                //txt_Telephone.IsEnabled = pValue;
                Cbo_Secteur.IsEnabled = pValue;
                Cbo_CodeConso.IsEnabled = pValue;
                Cbo_NatureClient.IsEnabled = pValue;
                Cbo_Regroupement.IsEnabled = pValue;
                //txt_NumLot.IsEnabled = pValue;
                Cbo_TypePiece.IsEnabled = pValue;
                Cbo_Commune.IsEnabled = pValue;
                Cbo_Quartier.IsEnabled = pValue;
                Cbo_Rue.IsEnabled = pValue;
                Cbo_Secteur.IsEnabled = pValue;
                Cbo_Nationalite.IsEnabled = pValue;

                #region Sylla 24/09/2015
                tabC_Onglet.IsEnabled = pValue;
                //Cbo_Type_Client.IsEnabled = pValue;
                Txt_NomClient.IsEnabled = pValue;
                Txt_PrenomClient.IsEnabled = pValue;

                cbo_typedoc.IsEnabled = pValue;
                btn_ajoutpiece.IsEnabled = pValue;
                btn_supprimerpiece.IsEnabled = pValue;
                dgListePiece.IsEnabled = pValue;
                Txt_Etage.IsEnabled = pValue;
                Txt_Porte.IsEnabled = pValue;
                #endregion

                //txtNumeroPiece.IsEnabled = pValue;
                txtPropriete.IsEnabled = pValue;
                txtAdresse.IsEnabled = pValue;
                Cbo_Diametre.IsEnabled = pValue;
                //hyperlinkButtonPropScannee.IsEnabled = pValue;
                lab_ListeAppareils.IsEnabled = pValue;
                Cbo_ListeAppareils.IsEnabled = pValue;
                Btn_ListeAppareils.IsEnabled = pValue;
                Btn_ListeAppareils.IsEnabled = pValue;
                ActiverEnregistrerOuTransmettre();
                ActiverOuDesactiverEnFonctionTdem();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        private void EnabledDemandeDevisInformationsByTdem(bool pValue)
        {
            try
            {
               
                dtp_finvalidationProprio.IsEnabled = pValue;
                txtNumeroPieceProprio.IsReadOnly = !pValue;
                Cbo_TypePiecePersonnePhysiqueProprio.IsEnabled = pValue;
                dtp_DateNaissanceProprio.IsEnabled = pValue;
                Txt_PrenomProprio_PersonePhysiq.IsReadOnly = !pValue;
                Txt_NomProprio_PersonePhysiq.IsReadOnly = !pValue;
                txt_Telephone_Proprio.IsReadOnly = !pValue;
                Txt_Email_Proprio.IsReadOnly = !pValue;
                Txt_BoitePosta_Proprio.IsReadOnly = !pValue;
                Txt_Faxe_Proprio.IsReadOnly = !pValue;
                Cbo_Nationalite_Proprio.IsEnabled = pValue;
                Cbo_Type_Proprietaire.IsEnabled = pValue;
                Cbo_Categorie.IsEnabled = pValue;
                Cbo_Secteur.IsEnabled = pValue;
                Cbo_CodeConso.IsEnabled = pValue;
                Cbo_NatureClient.IsEnabled = pValue;
                Cbo_Regroupement.IsEnabled = pValue;
                Cbo_TypePiece.IsEnabled = pValue;
                Cbo_Commune.IsEnabled = pValue;
                Cbo_Quartier.IsEnabled = pValue;
                Cbo_Rue.IsEnabled = pValue;
                Cbo_Secteur.IsEnabled = pValue;
                Cbo_Nationalite.IsEnabled = pValue;

                #region Sylla 24/09/2015

                Txt_NomClient.IsReadOnly = !pValue;
                Txt_PrenomClient.IsReadOnly = !pValue;

                cbo_typedoc.IsEnabled = pValue;
                btn_ajoutpiece.IsEnabled = pValue;
                btn_supprimerpiece.IsEnabled = pValue;
                dgListePiece.IsReadOnly = !pValue;
                Txt_Etage.IsReadOnly = !pValue;
                Txt_Porte.IsReadOnly = !pValue;
                #endregion

                txtPropriete.IsReadOnly = !pValue;
                txtAdresse.IsReadOnly = !pValue;
                Cbo_Diametre.IsEnabled = pValue;
                lab_ListeAppareils.IsEnabled = pValue;
                Cbo_ListeAppareils.IsEnabled = pValue;
                Btn_ListeAppareils.IsEnabled = pValue;
                Btn_ListeAppareils.IsEnabled = pValue;

                Txt_NomClient_PersonePhysiq.IsReadOnly = !pValue;
                dtp_DateNaissance.IsEnabled = pValue;
                Cbo_TypePiecePersonnePhysique.IsEnabled = pValue;
                dtp_DateNaissance.IsEnabled = pValue;
                Txt_Numeronina.IsReadOnly = !pValue;
                Txt_Email.IsReadOnly = !pValue;
                txt_Telephone.IsReadOnly = !pValue;
                txt_Telephone_Fixe.IsReadOnly = !pValue;
                chk_Email.IsEnabled = pValue;
                chk_SMS.IsEnabled = pValue;
                Cbo_Type_Client.IsEnabled = pValue;
                Cbo_Quartier.IsEnabled = pValue;
                txt_Quartier.IsReadOnly = !pValue;
                Cbo_Rue.IsEnabled = pValue;
                txt_NumRue.IsReadOnly = !pValue;
                dtp_finvalidation.IsEnabled = pValue;

                cbo_typedoc.IsEnabled = true;
                btn_ajoutpiece.IsEnabled = true;
                btn_supprimerpiece.IsEnabled = true;
                dgListePiece.IsEnabled = true;

                ActiverEnregistrerOuTransmettre();

                //if (txt_tdem.Tag!=null)
                //{
                   
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }







        private void Cbo_TypeDevis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActivationEnFonctionDeTdem();
        }

        private void ActivationEnFonctionDeTdem()
        {
            try
            {
                //if (txt_tdem.Tag != null)
                //{
                    if (this.Tdem != SessionObject.Enumere.BranchementAbonement)
                    {
                        btn_rech_branch.IsEnabled = true;
                        txt_Ref_Branchement.IsEnabled = true;
                        lab_ListeAppareils.Visibility = Visibility.Collapsed;
                        Cbo_ListeAppareils.Visibility = Visibility.Collapsed;
                        Btn_ListeAppareils.Visibility = Visibility.Collapsed;

                        label21.Visibility = Visibility.Collapsed;
                        Cbo_Diametre.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        btn_rech_branch.IsEnabled = false;
                        txt_Ref_Branchement.IsEnabled = false;
                        //lbl_RefBranch.Visibility = Visibility.Collapsed;
                        lab_ListeAppareils.Visibility = Visibility.Visible;
                        Cbo_ListeAppareils.Visibility = Visibility.Visible;
                        Btn_ListeAppareils.Visibility = Visibility.Visible;
                        label21.Visibility = Visibility.Visible;
                        Cbo_Diametre.Visibility = Visibility.Visible;
                    }
                    if (this.Tdem == SessionObject.Enumere.AugmentationPuissance || this.Tdem == SessionObject.Enumere.DimunitionPuissance)
                    {
                        tab_demande.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        tab_demande.Visibility = Visibility.Collapsed;

                    }
                    VerifierDonneesSaisiesInformationsDevis();
                    ActiverEnregistrerOuTransmettre();
                //}
            }

            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_Commune_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Commune.SelectedItem != null)
                {
                     CsCommune commune = Cbo_Commune.SelectedItem as  CsCommune;
                    if (commune != null)
                    {
                        Cbo_Commune.SelectedItem = commune;
                        txt_Commune.Text = commune.CODE ?? string.Empty;
                        RemplirQuartier(commune.PK_ID);
                        RemplirRues(commune.PK_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Btn_Transmettre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValiderInitialisation(laDetailDemande, true);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_Quartier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Quartier.SelectedItem != null)
                {
                    var quartier = Cbo_Quartier.SelectedItem as  CsQuartier;
                    if (quartier != null)
                    {
                        txt_Quartier.Text = quartier.CODE ?? string.Empty;
                        List<ServiceAccueil.CsSecteur> lstSecteur = SessionObject.LstSecteur.Where(t => t.FK_IDQUARTIER == quartier.PK_ID).ToList();
                        this.Cbo_Secteur.ItemsSource = lstSecteur;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_Rue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Rue.SelectedItem != null)
                {
                    var Secteur = Cbo_Rue.SelectedItem as  CsRues;
                    if (Secteur != null)
                        txt_NumRue.Text = Secteur.CODE ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void ActiverEnregistrerOuTransmettre()
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation ||
                    (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    bool ChampObligatoirTypeClientok = false;
                    if (Tdem == SessionObject.Enumere.BranchementAbonement ||
                        Tdem == SessionObject.Enumere.BranchementSimple)
                    {
                        #region Nouvelle demande
                        if (Cbo_Type_Client.SelectedItem != null)
                        {
                            if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "001".Trim())
                            {
                                ChampObligatoirTypeClientok = (!string.IsNullOrEmpty(this.Txt_NomClient_PersonePhysiq.Text) &&
                                            dtp_DateNaissance.SelectedDate != null &&
                                            Cbo_TypePiecePersonnePhysique.SelectedItem != null &&
                                            !string.IsNullOrEmpty(this.txtNumeroPiece.Text) &&
                                            dtp_finvalidation.SelectedDate != null
                                            );
                            }
                            if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "002".Trim())
                            {
                                ChampObligatoirTypeClientok = (!string.IsNullOrEmpty(this.Txt_RegistreCommerce.Text) &&
                                            !string.IsNullOrEmpty(this.Txt_Capital.Text) &&
                                            !string.IsNullOrEmpty(this.Txt_IdentiteFiscale.Text) &&
                                            !string.IsNullOrEmpty(this.Txt_NomMandataire.Text) &&
                                            !string.IsNullOrEmpty(this.Txt_PrenomMandataire.Text) &&
                                            !string.IsNullOrEmpty(this.Txt_PrenomSignataire.Text) &&
                                            !string.IsNullOrEmpty(this.Txt_NomSignataire.Text) &&
                                            Cbo_StatutJuridique.SelectedItem != null);
                            }
                            if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE.Trim() == "003".Trim())
                            {
                                ChampObligatoirTypeClientok = (!string.IsNullOrEmpty(this.Txt_Denomination.Text) &&
                                            !string.IsNullOrEmpty(this.Txt_PrenomMandataireAd.Text) &&
                                            !string.IsNullOrEmpty(this.Txt_NomMandataireAd.Text) &&
                                            !string.IsNullOrEmpty(this.Txt_PrenomSignataireAd.Text) &&
                                            !string.IsNullOrEmpty(this.Txt_NomSignataireAd.Text) &&
                                            !string.IsNullOrEmpty(this.Txt_PrenomSignataire.Text) &&
                                            !string.IsNullOrEmpty(this.Txt_NomSignataire.Text) &&
                                            Cbo_StatutJuridique.SelectedItem != null);
                            }
                        }
                        this.Btn_Transmettre.IsEnabled =
                                  (
                                  ChampObligatoirTypeClientok &&
                                  !string.IsNullOrEmpty(txt_Commune.Text) &&
                                  !string.IsNullOrEmpty(Txt_NomClient.Text) &&
                                  isPreuveSelectionnee &&
                                  Cbo_Centre.SelectedItem != null &&
                                  Cbo_Site.SelectedItem != null &&
                                  Cbo_Produit.SelectedItem != null &&
                                  Cbo_TypePiece.SelectedItem != null &&
                                  Cbo_Commune.SelectedItem != null &&
                                  Cbo_Quartier.SelectedItem != null &&
                                  Cbo_Rue.SelectedItem != null &&
                                  Cbo_CodeConso.SelectedItem != null &&
                                  Cbo_Type_Proprietaire.SelectedItem != null &&
                                  Cbo_Nationalite.SelectedItem != null &&
                                  Cbo_NatureClient.SelectedItem != null &&
                                  Cbo_Categorie.SelectedItem != null);

                        this.Btn_Enregistrer.IsEnabled =
                                           (
                                            ChampObligatoirTypeClientok &&
                                            !string.IsNullOrEmpty(txt_Commune.Text) &&
                                            !string.IsNullOrEmpty(Txt_NomClient.Text) &&
                                            isPreuveSelectionnee &&
                                            Cbo_Centre.SelectedItem != null &&
                                            Cbo_Site.SelectedItem != null &&
                                            Cbo_Produit.SelectedItem != null &&
                                            Cbo_TypePiece.SelectedItem != null &&
                                            Cbo_Commune.SelectedItem != null &&
                                            Cbo_Quartier.SelectedItem != null &&
                                            Cbo_Rue.SelectedItem != null &&
                                            Cbo_CodeConso.SelectedItem != null &&
                                            Cbo_NatureClient.SelectedItem != null &&
                                            Cbo_Categorie.SelectedItem != null);
                    }
                   #endregion
                    #region Autre
                    else
                    {
                        ChampObligatoirTypeClientok = true;
                        Btn_Enregistrer.IsEnabled = true;
                        Btn_Transmettre.IsEnabled = true;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void ActiverEnregistrerOuTransmettreCasEau()
        //{
        //    try
        //    {
        //        this.Btn_Enregistrer.IsEnabled =
        //                this.Btn_Transmettre.IsEnabled =
        //                (((this.ckbIdentifiable.IsChecked == true && aClient != null) ||
        //                  (!this.ckbIdentifiable.IsChecked == true && this.Txt_NomClient.Text != string.Empty)) &&
        //                 txtNumeroPiece.Text != string.Empty && txt_Commune.Text != string.Empty &&
        //                 Cbo_Tournee.SelectedItem != null && Cbo_Centre.SelectedItem != null && Cbo_Site.SelectedItem != null && Cbo_Produit.SelectedItem != null && Cbo_TypeDevis.SelectedItem != null);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void Txt_NomClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void txt_Commune_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void txtNumeroPiece_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_Tournee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //if (Cbo_Tournee.SelectedItem != null)
                //{
                //    var tournee = (CsTournee)Cbo_Tournee.SelectedItem;
                //    //if(tournee != null)
                //        //TxtCodeTournee.Text = tournee.CODE ?? string.Empty;
                //}
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void VerifierTypePiece()
        {
            try
            {
                //this.txtNumeroPiece.IsEnabled = ((this.Cbo_TypePiece.SelectedValue != null) && ((Galatee.Silverlight.ServiceDevis.ObjPIECEIDENTITE)this.Cbo_TypePiece.SelectedItem).LIBELLE != string.Empty);
                //if (this.txtNumeroPiece.IsEnabled)
                //    txtNumeroPiece.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Cbo_TypePiece_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_TypePiece.SelectedItem != null)
                {
                    var usage = ((CsUsage)Cbo_TypePiece.SelectedItem);

                    //ReloadNatureClientForusage(usage);
                    //ReloadCategorieClientForUsage(usage);

                    //if (usage != null)
                    //    TxtCategorieClient.Text = usage.CODE ?? string.Empty;
                }
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void ReloadCategorieClientForUsage(CsUsage usage)
        {
            var myls = LstCategorieClient_Usage.Where(ct => ct.FK_IDUSAGE == usage.PK_ID);
            if (myls != null)
            {
                var templst = myls.Select(t => t.FK_IDCATEGORIECLIENT);
                var lst = SessionObject.LstCategorie.Where(t => templst.Contains(t.PK_ID)).ToList();
                ReloadCategClient(lst);

            }
        }

        private void ReloadNatureClientForusage(CsUsage usage)
        {
            var myls = LstUsage_NatureClient.Where(ct => ct.FK_IDUSAGE == usage.PK_ID);
            if (myls != null)
            {
                var templst = myls.Select(t => t.FK_IDNATURECLIENT);
                var lst = SessionObject.LstNatureClient.Where(t => templst.Contains(t.PK_ID)).ToList();
                ReloadNatureClient(lst);
            }
        }

        void formUcPersonePhysique_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //laDetailDemande.LeClient.PersonePhysique = new CsPersonePhysique();

            nom = ((UcPersone_Physique)sender).nom;
            prenom = ((UcPersone_Physique)sender).prenom;
            datefinvalidité = ((UcPersone_Physique)sender).datefinvalidité;
            datenaissance = ((UcPersone_Physique)sender).datenaissance;
            numeropiece = ((UcPersone_Physique)sender).numeropiece;
            typepiece = ((UcPersone_Physique)sender).Cbo_TypePiece.SelectedItem != null ? ((ObjPIECEIDENTITE)((UcPersone_Physique)sender).Cbo_TypePiece.SelectedItem).PK_ID : 0;
            //laDetailDemande.LeClient.PersonePhysique. = string.IsNullOrEmpty(((UcPersone_Physique)sender).txtNumeroPiece.Text) ? null : ((UcPersone_Physique)sender).txtNumeroPiece.Text;
            //laDetailDemande.LeClient.PersonePhysique.NUMEROPIECEIDENTITE = string.IsNullOrEmpty(((UcPersone_Physique)sender).txtNumeroPiece.Text) ? null : ((UcPersone_Physique)sender).txtNumeroPiece.Text; 


        }

        private string GetClient(int pLongueurClient)
        {
            try
            {
                long client = 1;
                return client.ToString().PadLeft(pLongueurClient, '0');
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
                    pDemandeDevis.LaDemande.NUMDEM = string.IsNullOrEmpty(this.Txt_NumDevis.Text) ? null : this.Txt_NumDevis.Text;
                }
                #region Demande

                //if (pDemandeDevis.LaDemande == null) pDemandeDevis.LaDemande = new CsDemandeBase();
                //pDemandeDevis.LaDemande.NUMDEM = Txt_NumDevis.Text;
                //if (pDemandeDevis.LaDemande.TYPEDEMANDE != SessionObject.Enumere.BranchementAbonement)
                //   pDemandeDevis.LaDemande.CLIENT = string.IsNullOrEmpty(this.txt_Ref_Branchement.Text) ? string.Empty : this.txt_Ref_Branchement.Text;

                 pDemandeDevis.LaDemande.DATEMODIFICATION = DateTime.Now;
                pDemandeDevis.LaDemande.MOTIF = txt_motif.Text;
                if (Cbo_Centre.SelectedItem != null)
                {
                    var csCentre = Cbo_Centre.SelectedItem as CsCentre;
                    if (csCentre != null)
                    {
                        pDemandeDevis.LaDemande.FK_IDCENTRE = csCentre.PK_ID;
                        pDemandeDevis.LaDemande.CENTRE = csCentre.CODE;
                    }
                }

                if (Cbo_Produit.SelectedItem != null)
                {
                    var produit = Cbo_Produit.SelectedItem as CsProduit;
                    if (produit != null)
                    {
                        pDemandeDevis.LaDemande.FK_IDPRODUIT = produit.PK_ID;
                        pDemandeDevis.LaDemande.PRODUIT = produit.CODE;
                    }
                }

                if (txt_tdem.Tag != null)
                {
                    var typeDevis = (CsTdem)txt_tdem.Tag;
                    if (typeDevis != null)
                    {
                        pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = typeDevis.PK_ID;
                        pDemandeDevis.LaDemande.TYPEDEMANDE = typeDevis.CODE;
                    }
                }


                #endregion
                if (Tdem != SessionObject.Enumere.Resiliation &&
                    Tdem != SessionObject.Enumere.AugmentationPuissance &&
                    Tdem != SessionObject.Enumere.DimunitionPuissance)
                {
                    #region Client
                    if (pDemandeDevis.LeClient == null)
                        pDemandeDevis.LeClient = new CsClient();

                    if (Cbo_NatureClient.SelectedItem != null)
                    {
                        var NatureClient = Cbo_NatureClient.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsNatureClient;
                        if (NatureClient != null)
                        {
                            pDemandeDevis.LeClient.NATURE = NatureClient.CODE;
                            pDemandeDevis.LeClient.FK_IDNATURECLIENT = NatureClient.PK_ID;
                        }
                    }

                    if (Cbo_Nationalite.SelectedItem != null)
                    {
                        var NationnaliteClient = Cbo_Nationalite.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsNationalite;
                        if (NationnaliteClient != null)
                        {
                            pDemandeDevis.LeClient.NATIONNALITE = NationnaliteClient.CODE;
                            pDemandeDevis.LeClient.FK_IDNATIONALITE = NationnaliteClient.PK_ID;
                        }
                    }

                    if (Cbo_Type_Proprietaire.SelectedItem != null)
                    {
                        var TypeProprietaire = Cbo_Type_Proprietaire.SelectedItem as CsProprietaire;
                        if (TypeProprietaire != null)
                        {
                            pDemandeDevis.LeClient.PROPRIO = TypeProprietaire.CODE;
                            pDemandeDevis.LeClient.FK_IDPROPRIETAIRE = TypeProprietaire.PK_ID;
                        }
                    }
                    #region Sylla 24/09/2015

                    if (Cbo_Type_Client.SelectedItem != null)
                    {
                        var TypeClient = Cbo_Type_Client.SelectedItem as CsTypeClient;
                        if (TypeClient != null)
                        {
                            //pDemandeDevis.LeClient.NATIONNALITE = TypeClient.CODE;
                            pDemandeDevis.LeClient.FK_TYPECLIENT = TypeClient.PK_ID;
                        }

                    }


                    if (Cbo_TypePiece.SelectedItem != null)
                    {
                        var TypeClient = Cbo_TypePiece.SelectedItem as CsUsage;
                        if (TypeClient != null)
                        {
                            pDemandeDevis.LeClient.FK_IDUSAGE = TypeClient.PK_ID;
                        }
                    }

                    #endregion

                    pDemandeDevis.LeClient.TELEPHONEFIXE = string.IsNullOrEmpty(this.txt_Telephone_Fixe.Text) ? null : this.txt_Telephone_Fixe.Text;
                    pDemandeDevis.LeClient.FAX = string.IsNullOrEmpty(this.Txt_NomClient.Text) ? null : this.Txt_NomClient.Text;
                    pDemandeDevis.LeClient.BOITEPOSTAL = string.IsNullOrEmpty(this.Txt_PrenomClient.Text) ? null : this.Txt_PrenomClient.Text;
                    pDemandeDevis.LeClient.TELEPHONE = string.IsNullOrEmpty(this.txt_Telephone.Text) ? null : this.txt_Telephone.Text;
                    pDemandeDevis.LeClient.NUMPROPRIETE = string.IsNullOrEmpty(this.txtPropriete.Text) ? null : this.txtPropriete.Text;
                    pDemandeDevis.LeClient.CENTRE = string.IsNullOrEmpty(pDemandeDevis.LaDemande.CENTRE) ? null : pDemandeDevis.LaDemande.CENTRE;
                    pDemandeDevis.LeClient.REFCLIENT = string.IsNullOrEmpty(pDemandeDevis.LaDemande.CLIENT) ? null : pDemandeDevis.LaDemande.CLIENT;

                    if (((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.BranchementAbonement ||
                            ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.BranchementSimple)
                        this.txt_ordre.Text = "01";

                    pDemandeDevis.LeClient.ORDRE = this.txt_ordre.Text;
                    pDemandeDevis.LeClient.FK_IDCENTRE = pDemandeDevis.LaDemande.FK_IDCENTRE;
                    pDemandeDevis.LeClient.DATECREATION = DateTime.Now;
                    pDemandeDevis.LeClient.USERCREATION = UserConnecte.matricule;
                    pDemandeDevis.LeClient.NUMDEM = string.IsNullOrEmpty(this.Txt_NumDevis.Text) ? null : this.Txt_NumDevis.Text;
                    pDemandeDevis.LeClient.ISFACTUREEMAIL = chk_Email.IsChecked.Value;
                    pDemandeDevis.LeClient.ISFACTURESMS = chk_SMS.IsChecked.Value;


                    if (!string.IsNullOrWhiteSpace(this.Txt_Email.Text))
                    {
                        if (IsEmail(this.Txt_Email.Text))
                        {
                            pDemandeDevis.LeClient.EMAIL = string.IsNullOrEmpty(this.Txt_Email.Text) ? null : this.Txt_Email.Text;
                        }
                        else
                        {
                            Message.Show("Veuillez saisi un email client correct", "Erreur");
                            return null;
                        }
                    }

                    pDemandeDevis.LeClient.ADRMAND1 = txtAdresse.Text;
                    if (Cbo_CodeConso.SelectedItem != null)
                    {
                        var CodeConsoClient = Cbo_NatureClient.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsNatureClient;
                        if (CodeConsoClient != null)
                        {
                            pDemandeDevis.LeClient.CODECONSO = CodeConsoClient.CODE;
                            pDemandeDevis.LeClient.FK_IDCODECONSO = CodeConsoClient.PK_ID;
                        }
                    }
                    if (Cbo_Regroupement.SelectedItem != null)
                    {
                        var RegroupementClient = Cbo_Regroupement.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsRegCli;
                        if (RegroupementClient != null)
                        {
                            pDemandeDevis.LeClient.REGROUPEMENT = RegroupementClient.CODE;
                            pDemandeDevis.LeClient.FK_IDREGROUPEMENT = RegroupementClient.PK_ID;
                        }
                    }

                    if (Cbo_Categorie.SelectedItem != null)
                    {
                        var cat = Cbo_Categorie.SelectedItem as CsCategorieClient;
                        if (cat != null)
                        {
                            pDemandeDevis.LeClient.FK_IDCATEGORIE = cat.PK_ID;
                            pDemandeDevis.LeClient.CATEGORIE = cat.CODE;
                            if (pDemandeDevis.LeClient.CATEGORIE == SessionObject.Enumere.CategorieAgentEdm && string.IsNullOrEmpty(this.txt_MaticuleAgent.Text))
                                throw new Exception("Le matricule est obligatoire pour les agents EDM");
                            pDemandeDevis.LeClient.AGENTFACTURE = this.txt_MaticuleAgent.Text;
                        }
                    }
                    pDemandeDevis.LeClient.CODEIDENTIFICATIONNATIONALE = string.IsNullOrEmpty(this.Txt_Numeronina.Text) ? null : this.Txt_Numeronina.Text;
                    pDemandeDevis.LeClient.FK_IDRELANCE = 1;
                    pDemandeDevis.LeClient.CODERELANCE = "0";
                    pDemandeDevis.LeClient.MODEPAIEMENT = "0";
                    pDemandeDevis.LeClient.FK_IDMODEPAIEMENT = 1;

                    #endregion
                    #region AG
                    if (pDemandeDevis.Ag == null) pDemandeDevis.Ag = new CsAg();
                    if (Cbo_Commune.SelectedItem != null)
                    {
                        var commune = Cbo_Commune.SelectedItem as CsCommune;
                        if (commune != null)
                        {
                            pDemandeDevis.Ag.FK_IDCOMMUNE = commune.PK_ID;
                            pDemandeDevis.Ag.COMMUNE = commune.CODE;
                        }
                    }
                    pDemandeDevis.Ag.FK_IDTOURNEE = null;
                    if (Cbo_Quartier.SelectedItem != null)
                    {
                        var quartier = Cbo_Quartier.SelectedItem as CsQuartier;
                        if (quartier != null)
                        {
                            pDemandeDevis.Ag.FK_IDQUARTIER = quartier.PK_ID;
                            pDemandeDevis.Ag.QUARTIER = quartier.CODE;
                        }
                    }

                    if (Cbo_Rue.SelectedItem != null)
                    {
                        var rue = Cbo_Rue.SelectedItem as CsRues;
                        if (rue != null)
                        {
                            pDemandeDevis.Ag.FK_IDRUE = rue.PK_ID;
                            pDemandeDevis.Ag.RUE = rue.CODE;
                        }
                    }
                    if (Cbo_Secteur.SelectedItem != null)
                    {
                        var Secteur = Cbo_Secteur.SelectedItem as ServiceAccueil.CsSecteur;
                        if (Secteur != null)
                        {
                            pDemandeDevis.Ag.FK_IDSECTEUR = Secteur.PK_ID;
                            pDemandeDevis.Ag.SECTEUR = Secteur.CODE;
                        }
                    }
                    //pDemandeDevis.Ag.NOMP  = string.IsNullOrEmpty(this.Txt_NomClient.Text) ? null : this.Txt_NomClient.Text; 
                    pDemandeDevis.Ag.TELEPHONE = string.IsNullOrEmpty(this.txt_Telephone.Text) ? null : this.txt_Telephone.Text;
                    pDemandeDevis.Ag.ETAGE = string.IsNullOrEmpty(this.Txt_Etage.Text) ? null : this.Txt_Etage.Text;
                    pDemandeDevis.Ag.PORTE = string.IsNullOrEmpty(this.Txt_Porte.Text) ? null : this.Txt_Porte.Text;
                    pDemandeDevis.Ag.CENTRE = string.IsNullOrEmpty(pDemandeDevis.LaDemande.CENTRE) ? null : pDemandeDevis.LaDemande.CENTRE;
                    pDemandeDevis.Ag.CLIENT = string.IsNullOrEmpty(pDemandeDevis.LaDemande.CLIENT) ? null : pDemandeDevis.LaDemande.CLIENT;
                    pDemandeDevis.Ag.NUMDEM = string.IsNullOrEmpty(this.Txt_NumDevis.Text) ? null : this.Txt_NumDevis.Text;
                    pDemandeDevis.Ag.FK_IDCENTRE = pDemandeDevis.LaDemande.FK_IDCENTRE;
                    pDemandeDevis.Ag.DATECREATION = DateTime.Now;
                    pDemandeDevis.Ag.USERCREATION = UserConnecte.matricule;
                    #endregion
                    #region BRT
                    if (pDemandeDevis.Branchement == null)
                        pDemandeDevis.Branchement = new CsBrt();

                    pDemandeDevis.Branchement.CENTRE = string.IsNullOrEmpty(pDemandeDevis.LaDemande.CENTRE) ? null : pDemandeDevis.LaDemande.CENTRE;
                    pDemandeDevis.Branchement.CLIENT = string.IsNullOrEmpty(pDemandeDevis.LaDemande.CLIENT) ? null : pDemandeDevis.LaDemande.CLIENT;
                    pDemandeDevis.Branchement.PRODUIT = string.IsNullOrEmpty(pDemandeDevis.LaDemande.PRODUIT) ? null : pDemandeDevis.LaDemande.PRODUIT;
                    pDemandeDevis.Branchement.NUMDEM = string.IsNullOrEmpty(this.Txt_NumDevis.Text) ? null : this.Txt_NumDevis.Text;
                    pDemandeDevis.Branchement.FK_IDCENTRE = pDemandeDevis.LaDemande.FK_IDCENTRE;
                    pDemandeDevis.Branchement.FK_IDPRODUIT = pDemandeDevis.LaDemande.FK_IDPRODUIT.Value;
                    pDemandeDevis.Branchement.DATECREATION = DateTime.Now;
                    pDemandeDevis.Branchement.USERCREATION = UserConnecte.matricule;
                    if (pDemandeDevis.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        pDemandeDevis.Branchement.NBPOINT = 1;


                    if (Cbo_Diametre.SelectedItem != null)
                    {
                        var Diametre = Cbo_Diametre.SelectedItem as CsDiacomp;
                        if (Diametre != null)
                            pDemandeDevis.Branchement.DIAMBRT = Diametre.CODE;
                    }
                    #endregion
                    #region Sylla 24/09/2015

                    if (Cbo_Type_Client.SelectedItem != null)
                    {
                        if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE != null)
                        {
                            string codetypeclient = ((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE;
                            switch (codetypeclient.Trim())
                            {
                                case "001":
                                    #region Personne Physique
                                    GetPersonnPhyqueData(pDemandeDevis);
                                    #endregion
                                    break;
                                case "002":
                                    if (GetSocietePriveData(pDemandeDevis) == null)
                                        return null;
                                    break;
                                case "003":
                                    GetAdministraionInstitutData(pDemandeDevis);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            Message.Show("Veuillez renseigné les informations spécifique au typr de client", "Demande");
                            return null;
                        }

                    }
                    else
                    {
                        Message.Show("Veuillez renseigné les informations spécifique au typr de client", "Demande");
                        return null;
                    }

                    GetSocieteProprietaire(pDemandeDevis);

                    #endregion
                    #region Appareil
                    List<ObjAPPAREILSDEVIS> lAppareilsDevis = new List<ObjAPPAREILSDEVIS>();
                    ObjAPPAREILSDEVIS AppareilDevis = null;
                    if (listAppareilsSelectionnes != null && listAppareilsSelectionnes.Count > 0)
                    {
                        foreach (ObjAPPAREILS appareil in listAppareilsSelectionnes)
                        {
                            AppareilDevis = new ObjAPPAREILSDEVIS();
                            AppareilDevis.PK_ID = appareil.PK_IDAPPAREILDEVIS;
                            AppareilDevis.FK_IDCODEAPPAREIL = appareil.PK_ID;
                            AppareilDevis.CODEAPPAREIL = appareil.CODEAPPAREIL;
                            if (pDemandeDevis != null && pDemandeDevis.LaDemande != null)
                            {
                                AppareilDevis.FK_IDDEVIS = pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE;
                                AppareilDevis.NUMDEVIS = pDemandeDevis.LaDemande.NUMDEM;
                            }
                            AppareilDevis.NBRE = appareil.NOMBRE;
                            AppareilDevis.PUISSANCE = appareil.PUISSANCE;
                            AppareilDevis.DATECREATION = DateTime.Now;
                            AppareilDevis.USERCREATION = UserConnecte.matricule;
                            lAppareilsDevis.Add(AppareilDevis);
                        }
                        pDemandeDevis.AppareilDevis = lAppareilsDevis;
                    }
                    #endregion

                }
                #region Abon

                if (((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.AugmentationPuissance || ((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.DimunitionPuissance)
                {
                    if (pDemandeDevis.Abonne == null) pDemandeDevis.Abonne = new CsAbon();
                    if (laDetailDemande != null)
                        pDemandeDevis.Abonne = laDetailDemande.Abonne != null ? laDetailDemande.Abonne : new CsAbon();

                    //pDemandeDevis.Abonne.PK_ID = 0;

                    if ((rbtn_AugtPiss.IsChecked.Value || rbtn_DiminPuiss.IsChecked.Value) && !string.IsNullOrWhiteSpace(txt_nouvelPuiss.Text))
                    {
                        pDemandeDevis.Abonne.ISAUGMENTATIONPUISSANCE = rbtn_AugtPiss.IsChecked.Value;
                        pDemandeDevis.Abonne.ISDIMINUTIONPUISSANCE = rbtn_DiminPuiss.IsChecked.Value;
                        //pDemandeDevis.Abonne.PUISSANCE=decimal.Parse( txt_ancienPuiss.Text );

                        decimal NOUVELLEPUISSANCE = 0;
                        decimal ANCIENNEPUISSANCE = 0;

                        if (decimal.TryParse(txt_nouvelPuiss.Text, out NOUVELLEPUISSANCE))
                        {
                            decimal.TryParse(txt_ancienPuiss.Text, out ANCIENNEPUISSANCE);

                            if (pDemandeDevis.Abonne.ISAUGMENTATIONPUISSANCE)
                            {
                                if (NOUVELLEPUISSANCE < ANCIENNEPUISSANCE)
                                {
                                    Message.ShowError(" La nouvelle puissance doit etre supérieur à l'ancien ", "Information demande");
                                    return null;
                                }

                            }
                            if (pDemandeDevis.Abonne.ISDIMINUTIONPUISSANCE)
                            {
                                if (ANCIENNEPUISSANCE < NOUVELLEPUISSANCE)
                                {
                                    Message.ShowError(" L'ancien puissance doit etre supérieur à nouvel ", "Information demande");
                                    return null;
                                }
                            }
                            pDemandeDevis.Abonne.NOUVELLEPUISSANCE = NOUVELLEPUISSANCE;
                        }
                        else
                        {
                            Message.ShowError("Veuillez saisir une valeur numerique", "Information demande");
                            return null;
                        }
                    }
                    else
                    {
                        Message.ShowError("Veuillez correctement saisir les information de nouvelle puissance", "Information demande");
                        return null;
                    }


                }
                #endregion
                #region Doc Scanne
                if (pDemandeDevis.ObjetScanne == null) pDemandeDevis.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                pDemandeDevis.ObjetScanne.AddRange(LstPiece);
                #endregion

                pDemandeDevis.LaDemande.ISNEW = true;
                Tdem = pDemandeDevis.LaDemande.TYPEDEMANDE;
                pDemandeDevis.LaDemande.ORDRE = string.IsNullOrEmpty(txt_ordre.Text) ? "01" : txt_ordre.Text;
                pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;

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


        private void GetAdministraionInstitutData(CsDemande pDemandeDevis)
        {
            pDemandeDevis.LeClient.AdministrationInstitut = new CsAdministration_Institut();

            pDemandeDevis.LeClient.AdministrationInstitut.PK_ID = Pk_IdAdministration != 0 ? Pk_IdAdministration : 0;
            pDemandeDevis.LeClient.AdministrationInstitut.NOMMANDATAIRE = Txt_NomMandataireAd.Text;
            pDemandeDevis.LeClient.AdministrationInstitut.PRENOMMANDATAIRE = Txt_PrenomMandataireAd.Text;
            pDemandeDevis.LeClient.AdministrationInstitut.RANGMANDATAIRE = Txt_RangMandataireAd.Text;
            pDemandeDevis.LeClient.AdministrationInstitut.NOMSIGNATAIRE = Txt_NomSignataireAd.Text;
            pDemandeDevis.LeClient.AdministrationInstitut.PRENOMSIGNATAIRE = Txt_PrenomSignataireAd.Text;
            pDemandeDevis.LeClient.AdministrationInstitut.RANGSIGNATAIRE = Txt_RangSignataireAd.Text;

        }

        private void GetPersonnPhyqueData(CsDemande pDemandeDevis)
        {
            int? mynull = null;
            pDemandeDevis.LeClient.PersonePhysique = new CsPersonePhysique();
            pDemandeDevis.LeClient.PersonePhysique.PK_ID = Pk_IdPersPhys != 0 ? Pk_IdPersPhys : 0;
            pDemandeDevis.LeClient.PersonePhysique.NOMABON  = Txt_NomClient_PersonePhysiq.Text;
            pDemandeDevis.LeClient.PersonePhysique.DATEFINVALIDITE = dtp_finvalidation.SelectedDate;
            pDemandeDevis.LeClient.PersonePhysique.DATENAISSANCE = dtp_DateNaissance.SelectedDate;
            pDemandeDevis.LeClient.PersonePhysique.NUMEROPIECEIDENTITE = txtNumeroPiece.Text;
            pDemandeDevis.LeClient.PersonePhysique.FK_IDPIECEIDENTITE = Cbo_TypePiecePersonnePhysique.SelectedItem != null ? ((ObjPIECEIDENTITE)Cbo_TypePiecePersonnePhysique.SelectedItem).PK_ID : mynull;
            pDemandeDevis.LeClient.NOMABON = pDemandeDevis.LeClient.PersonePhysique.NOMABON  ;
            pDemandeDevis.Ag.NOMP = pDemandeDevis.LeClient.PersonePhysique.NOMABON  ;
        }
        private CsDemande GetSocietePriveData(CsDemande pDemandeDevis)
        {
            pDemandeDevis.LeClient.SocietePrives = new CsSocietePrive();
            int? Mynull = null;
            decimal capital = 0;
            if (!decimal.TryParse(Txt_Capital.Text, out capital))
            {
                Message.Show("veuillez saisir une valeur numerique", "Demande");
                return null;
            }
            pDemandeDevis.LeClient.SocietePrives.PK_ID = Pk_IdSocoiete != 0 ? Pk_IdSocoiete : 0;
            pDemandeDevis.LeClient.SocietePrives.NUMEROREGISTRECOMMERCE = Txt_RegistreCommerce.Text;
            pDemandeDevis.LeClient.SocietePrives.FK_IDSTATUTJURIQUE = Cbo_StatutJuridique.SelectedItem != null ? ((CsStatutJuridique)Cbo_StatutJuridique.SelectedItem).PK_ID : Mynull;
            pDemandeDevis.LeClient.SocietePrives.CAPITAL = capital;
            pDemandeDevis.LeClient.SocietePrives.IDENTIFICATIONFISCALE = Txt_IdentiteFiscale.Text;
            pDemandeDevis.LeClient.SocietePrives.DATECREATION = dtp_DateCreation.SelectedDate;
            pDemandeDevis.LeClient.SocietePrives.SIEGE = Txt_Siege.Text;
            pDemandeDevis.LeClient.SocietePrives.NOMMANDATAIRE = Txt_NomMandataire.Text;
            pDemandeDevis.LeClient.SocietePrives.PRENOMMANDATAIRE = Txt_PrenomMandataire.Text;
            pDemandeDevis.LeClient.SocietePrives.RANGMANDATAIRE = Txt_RangMandataire.Text;
            pDemandeDevis.LeClient.SocietePrives.NOMSIGNATAIRE = Txt_NomSignataire.Text;
            pDemandeDevis.LeClient.SocietePrives.PRENOMSIGNATAIRE = Txt_PrenomSignataire.Text;
            pDemandeDevis.LeClient.SocietePrives.RANGSIGNATAIRE = Txt_RangSignataire.Text;

            pDemandeDevis.LeClient.NOMABON = pDemandeDevis.LeClient.SocietePrives.NOMMANDATAIRE + " " + pDemandeDevis.LeClient.SocietePrives.PRENOMMANDATAIRE;
            pDemandeDevis.Ag.NOMP = pDemandeDevis.LeClient.SocietePrives.NOMMANDATAIRE + " " + pDemandeDevis.LeClient.SocietePrives.PRENOMMANDATAIRE;

            return pDemandeDevis;
        }

        private CsDemande GetSocieteProprietaire(CsDemande pDemandeDevis)
        {
            pDemandeDevis.LeClient.InfoProprietaire_ = new CsInfoProprietaire();
            int? Mynull = null;

            pDemandeDevis.LeClient.InfoProprietaire_.PK_ID = Pk_IdPropietaire != 0 ? Pk_IdPropietaire : 0;
            pDemandeDevis.LeClient.InfoProprietaire_.FK_IDNATIONNALITE = Cbo_Nationalite_Proprio.SelectedItem != null ? ((Galatee.Silverlight.ServiceAccueil.CsNationalite)Cbo_Nationalite_Proprio.SelectedItem).PK_ID : Mynull;
            pDemandeDevis.LeClient.InfoProprietaire_.BOITEPOSTALE = Txt_BoitePosta_Proprio.Text;
            pDemandeDevis.LeClient.InfoProprietaire_.DATEFINVALIDITE = dtp_finvalidationProprio.SelectedDate;
            pDemandeDevis.LeClient.InfoProprietaire_.DATENAISSANCE = dtp_DateNaissanceProprio.SelectedDate;
            pDemandeDevis.LeClient.InfoProprietaire_.EMAIL = Txt_Email_Proprio.Text;

            if (!string.IsNullOrWhiteSpace(this.Txt_Email_Proprio.Text))
            {
                if (IsEmail(this.Txt_Email_Proprio.Text))
                {
                    pDemandeDevis.LeClient.InfoProprietaire_.EMAIL = string.IsNullOrEmpty(this.Txt_Email_Proprio.Text) ? null : this.Txt_Email_Proprio.Text;
                }
                else
                {
                    Message.Show("Veuillez saisi un email propriétaire correct", "Erreur");
                    return null;
                }
            }
            pDemandeDevis.LeClient.InfoProprietaire_.FAX = Txt_Faxe_Proprio.Text;
            pDemandeDevis.LeClient.InfoProprietaire_.FK_DCLIENT = pDemandeDevis.LeClient.PK_ID;
            pDemandeDevis.LeClient.InfoProprietaire_.FK_IDPIECEIDENTITE = Cbo_TypePiecePersonnePhysiqueProprio.SelectedItem != null ? ((ObjPIECEIDENTITE)Cbo_TypePiecePersonnePhysiqueProprio.SelectedItem).PK_ID : Mynull;
            pDemandeDevis.LeClient.InfoProprietaire_.NOM = Txt_NomProprio_PersonePhysiq.Text;
            pDemandeDevis.LeClient.InfoProprietaire_.PRENOM = Txt_PrenomProprio_PersonePhysiq.Text;
            pDemandeDevis.LeClient.InfoProprietaire_.TELEPHONEMOBILE = txt_Telephone_Proprio.Text;
            pDemandeDevis.LeClient.InfoProprietaire_.NUMEROPIECEIDENTITE = txtNumeroPieceProprio.Text;

      

            return pDemandeDevis;
        }



        private void Btn_ListeAppareils_Click(object sender, RoutedEventArgs e)
        {
            List<ObjAPPAREILS> listeAppareil = null;
            try
            {
                var UcListAppareils = new Galatee.Silverlight.Devis.UcListAppareils();
                if (Cbo_ListeAppareils.Items.Count > 0)
                {
                    listeAppareil = new List<ObjAPPAREILS>();
                    foreach (ObjAPPAREILS appareil in Cbo_ListeAppareils.Items)
                        listeAppareil.Add(appareil);
                }
                UcListAppareils.AppareilsSelectionnes = listeAppareil;
                UcListAppareils.Closed += new EventHandler(UcListAppareils_Closed);
                UcListAppareils.Show();

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void UcListAppareils_Closed(object sender, EventArgs e)
        {
            try
            {
                var lappareils = ((Galatee.Silverlight.Devis.UcListAppareils)sender).AppareilsSelectionnes;
                if (lappareils != null && lappareils.Count > 0)
                {
                    RemplirListeAppareils(lappareils);
                    ActiverEnregistrerOuTransmettre();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void RemplirListeDiametresParPuissance(List<ObjAPPAREILS> lappareils)
        //{
        //    try
        //    {
        //        int sommePuissance = lappareils.Where(u=> u.PUISSANCE != 0 && u.NOMBRE != 0 ).Sum(t => (t.NOMBRE * t.PUISSANCE));

        //        List<CsDiacomp> _listeDesDiametrePuissance = _listeDesDiametreExistant.Where(p => p.PUISSANCEDUDIAMETRE >= (Decimal)sommePuissance).ToList();

        //        if (_listeDesDiametrePuissance == null || _listeDesDiametrePuissance.Count ==0)
        //        {
        //            Cbo_Diametre.ItemsSource = null;
        //            Cbo_Diametre.ItemsSource = _listeDesDiametrePuissance;
        //            Cbo_Diametre.DisplayMemberPath = "LIBELLE";
        //            return;
        //        }

        //        Cbo_Diametre.ItemsSource = null;
        //        Cbo_Diametre.ItemsSource = _listeDesDiametreExistant;
        //        Cbo_Diametre.DisplayMemberPath = "LIBELLE";

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void RemplirListeAppareils(List<ObjAPPAREILS> lappareils)
        {
            try
            {
                int sommePuissance = 0;
                decimal intensite = 0;

                Cbo_ListeAppareils.Items.Clear();
                foreach (var item in lappareils)
                {
                    sommePuissance = sommePuissance + (item.NOMBRE * item.PUISSANCE);
                    Cbo_ListeAppareils.Items.Add(item);
                }
                Cbo_ListeAppareils.SelectedValuePath = "CODEAPPAREIL";
                Cbo_ListeAppareils.DisplayMemberPath = "DISPLAYLABEL";
                listAppareilsSelectionnes = lappareils;
                Cbo_ListeAppareils.SelectedIndex = 0;

                CsProduit leProduitSelect = Cbo_Produit.SelectedItem as CsProduit;

                if (sommePuissance != 0)
                    intensite = sommePuissance / 220;
                if (leProduitSelect != null)
                {
                    List<CsDiacomp> _listeDesDiametrePuissance = _listeDesDiametreExistant.Where(p => p.CALIBRE  >= intensite && p.FK_IDPRODUIT == leProduitSelect.PK_ID).ToList();
                    if (_listeDesDiametrePuissance == null || _listeDesDiametrePuissance.Count == 0)
                    {
                        Cbo_Diametre.ItemsSource = null;
                        Cbo_Diametre.ItemsSource = _listeDesDiametreExistant.Where(p => p.FK_IDPRODUIT == leProduitSelect.PK_ID);
                        Cbo_Diametre.DisplayMemberPath = "LIBELLE";
                        return;
                    }

                    Cbo_Diametre.ItemsSource = null;
                    Cbo_Diametre.ItemsSource = _listeDesDiametrePuissance;
                    Cbo_Diametre.DisplayMemberPath = "LIBELLE";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListeAppareilsDevis(List<ObjAPPAREILSDEVIS> pListAppareilsDevis)
        {
            try
            {

                List<ObjAPPAREILSDEVIS> lAppareilsDevis = new List<ObjAPPAREILSDEVIS>();
                ObjAPPAREILS Appareil = null;
                listAppareilsSelectionnes = new List<ObjAPPAREILS>();
                if (pListAppareilsDevis != null && pListAppareilsDevis.Count > 0)
                {
                    foreach (ObjAPPAREILSDEVIS appareildevis in pListAppareilsDevis)
                    {
                        Appareil = new ObjAPPAREILS();
                        Appareil.PK_ID = appareildevis.FK_IDCODEAPPAREIL;
                        Appareil.PK_IDAPPAREILDEVIS = appareildevis.PK_ID;
                        Appareil.DESIGNATION = appareildevis.DESIGNATION;
                        Appareil.CODEAPPAREIL = appareildevis.CODEAPPAREIL;
                        Appareil.NOMBRE = (int)appareildevis.NBRE;
                        Appareil.PUISSANCE = (int)appareildevis.PUISSANCE;
                        Appareil.DISPLAYLABEL = appareildevis.CODEAPPAREIL + "-" + appareildevis.DESIGNATION + " N " +
                                                 appareildevis.NBRE.ToString() + " P " + appareildevis.PUISSANCE.ToString();
                        listAppareilsSelectionnes.Add(Appareil);
                    }
                }
                Cbo_ListeAppareils.Items.Clear();
                foreach (var item in listAppareilsSelectionnes)
                {
                    Cbo_ListeAppareils.Items.Add(item);
                }
                Cbo_ListeAppareils.SelectedValuePath = "CODEAPPAREIL";
                Cbo_ListeAppareils.DisplayMemberPath = "DISPLAYLABEL";
                Cbo_ListeAppareils.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void txtCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void txtProduit_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_ListeAppareils_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_Categorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Categorie.SelectedItem != null)
                {
                    var cat = ((CsCategorieClient)Cbo_Categorie.SelectedItem);

                    //ReloadTypeclientForCateg(cat);
                    ReloadUsageForCateg(cat);

                    if (cat != null)
                    {
                        TxtCategorieClient.Text = cat.CODE ?? string.Empty;
                        if (cat.CODE == SessionObject.Enumere.CategorieAgentEdm)
                            this.txt_MaticuleAgent.IsReadOnly = false;
                    }
                }
                //ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void ReloadTypeclientForCateg(CsCategorieClient cat)
        {


            var myls = LstCategorieClient_TypeClient.Where(ct => ct.FK_IDCATEGORIECLIENT == cat.PK_ID);
            if (myls != null)
            {
                var templst = myls.Select(t => t.FK_IDTYPECLIENT);
                var lsttypecient = SessionObject.LstTypeClient.Where(t => templst.Contains(t.PK_ID)).ToList();
                ReloadTypeClient(lsttypecient);
            }

        }
        private void ReloadUsageForCateg(CsCategorieClient cat)
        {


            var myls = LstCategorieClient_Usage.Where(ct => ct.FK_IDCATEGORIECLIENT == cat.PK_ID);
            if (myls != null)
            {
                var templst = myls.Select(t => t.FK_IDUSAGE);
                var lst = lstusage.Where(t => templst.Contains(t.PK_ID)).ToList();
                ReloadlstUsage(lst);
            }

        }
        private void Cbo_Secteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Secteur.SelectedItem != null)
                {
                    var Secteur = Cbo_Secteur.SelectedItem as ServiceAccueil.CsSecteur;
                    if (Secteur != null)
                        txt_NumSecteur.Text = Secteur.CODE ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_CodeConso_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_CodeConso.SelectedItem != null)
                {
                    var conso = ((ServiceAccueil.CsCodeConsomateur)Cbo_CodeConso.SelectedItem);
                    if (conso != null)
                        Txt_CodeConso.Text = conso.CODE ?? string.Empty;
                }
                //ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_NatureClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_NatureClient.SelectedItem != null)
                {
                    var Nature = ((ServiceAccueil.CsNatureClient)Cbo_NatureClient.SelectedItem);

                    //ReloadTypeclientForNature(Nature);
                    //ReloadUsageForNature(Nature);

                    if (Nature != null)
                        Txt_CodeNatureClient.Text = Nature.CODE ?? string.Empty;
                }
                //ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void ReloadUsageForNature(ServiceAccueil.CsNatureClient Nature)
        {

            var myls = LstUsage_NatureClient.Where(ct => ct.FK_IDNATURECLIENT == Nature.PK_ID);
            if (myls != null)
            {
                var templst = myls.Select(t => t.FK_IDUSAGE);
                var lst = lstusage.Where(t => templst.Contains(t.PK_ID)).ToList();
                ReloadlstUsage(lst);
            }
        }

        private void ReloadTypeclientForNature(ServiceAccueil.CsNatureClient Nature)
        {
            var myls = LstNatureClient_TypeClient.Where(ct => ct.FK_IDNATURECLIENT == Nature.PK_ID);
            if (myls != null)
            {
                var templst = myls.Select(t => t.FK_IDTYPECLIENT);
                var lsttypecient = SessionObject.LstTypeClient.Where(t => templst.Contains(t.PK_ID)).ToList();
                ReloadTypeClient(lsttypecient);
            }
        }

        private void Cbo_Regroupement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Regroupement.SelectedItem != null)
                {
                    var Regroupement = ((ServiceAccueil.CsRegCli)Cbo_Regroupement.SelectedItem);
                    if (Regroupement != null)
                        Txt_CodeRegroupement.Text = Regroupement.CODE ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        #region Sylla 24/09/2015

        private void Txt_PrenomClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Cbo_TypeClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CsTypeClient typeclient = ((CsTypeClient)Cbo_Type_Client.SelectedItem);

                ReloadCategorieClientFortypeclient(typeclient);
                ReloadNatureClientFortypeclient(typeclient);


                switch (typeclient.CODE.Trim())
                {
                    case "001":
                        PersonPhysiqueWindows(Visibility.Visible);
                        SocietePriveWindows(Visibility.Collapsed);
                        AdministraionInstitutWindows(Visibility.Collapsed);
                        break;
                    case "002":
                        SocietePriveWindows(Visibility.Visible);
                        AdministraionInstitutWindows(Visibility.Collapsed);
                        PersonPhysiqueWindows(Visibility.Collapsed);
                        break;
                    case "003":
                        AdministraionInstitutWindows(Visibility.Visible);
                        PersonPhysiqueWindows(Visibility.Collapsed);
                        SocietePriveWindows(Visibility.Collapsed);
                        break;
                    default:
                        break;
                }
                VerifierDonneesSaisiesInformationsDevis();
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void ReloadNatureClientFortypeclient(CsTypeClient typeclient)
        {
            var myls = LstNatureClient_TypeClient.Where(ct => ct.FK_IDTYPECLIENT == typeclient.PK_ID);
            if (myls != null)
            {
                var templst = myls.Select(t => t.FK_IDNATURECLIENT);
                var lst = SessionObject.LstNatureClient.Where(t => templst.Contains(t.PK_ID)).ToList();
                ReloadNatureClient(lst);
            }
        }

        private void ReloadCategorieClientFortypeclient(CsTypeClient typeclient)
        {
            var myls = LstCategorieClient_TypeClient.Where(ct => ct.FK_IDTYPECLIENT == typeclient.PK_ID);
            if (myls != null)
            {
                var templst = myls.Select(t => t.FK_IDCATEGORIECLIENT);
                var lst = SessionObject.LstCategorie.Where(t => templst.Contains(t.PK_ID)).ToList();
                ReloadCategClient(lst);

            }
        }


        private void formUcAdministration_Institut_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DENOMINATION = ((UcAdministration_Institut)sender).Txt_Denomination.Text;
            NOMMANDATAIRE1 = ((UcAdministration_Institut)sender).Txt_NomMandataire.Text;
            NOMSIGNATAIRE1 = ((UcAdministration_Institut)sender).Txt_NomSignataire.Text;
            PRENOMMANDATAIRE1 = ((UcAdministration_Institut)sender).Txt_PrenomMandataire.Text;
            PRENOMSIGNATAIRE1 = ((UcAdministration_Institut)sender).Txt_PrenomSignataire.Text;
            RANGMANDATAIRE1 = ((UcAdministration_Institut)sender).Txt_RangMandataire.Text;
            RANGSIGNATAIRE1 = ((UcAdministration_Institut)sender).Txt_RangSignataire.Text;
        }


        private void AdministraionInstitutWindows(Visibility stat)
        {

            //UcAdministration_Institut formUcAdministration_Institut = new UcAdministration_Institut(laDetailDemande);
            //formUcAdministration_Institut.Closing += formUcAdministration_Institut_Closing;
            //formUcAdministration_Institut.Show();
            lbl_Denomination.Visibility = stat;
            Txt_Denomination.Visibility = stat;
            lbl_NomSignataireAd.Visibility = stat;
            Txt_NomSignataireAd.Visibility = stat;
            lbl_PrenomSignataireAd.Visibility = stat;
            Txt_PrenomSignataireAd.Visibility = stat;
            lbl_RangSignataireAd.Visibility = stat;
            Txt_RangSignataireAd.Visibility = stat;
            lbl_NomMandataireAd.Visibility = stat;
            Txt_NomMandataireAd.Visibility = stat;
            lbl_PrenomMandataireAd.Visibility = stat;
            Txt_PrenomMandataireAd.Visibility = stat;
            lbl_RangMAndataireAd.Visibility = stat;
            Txt_RangMandataireAd.Visibility = stat;
        }
        private void SocietePriveWindows(Visibility stat)
        {
            //UcSocietePrive formUcSocietePrive = new UcSocietePrive(laDetailDemande);
            //formUcSocietePrive.Closing += formUcSocietePrive_Closing;
            //formUcSocietePrive.Show();

            lbl_RegistreCommerce.Visibility = stat;
            Txt_RegistreCommerce.Visibility = stat;
            lbl_Capital.Visibility = stat;
            Txt_Capital.Visibility = stat;
            lbl_IdentificationFiscale.Visibility = stat;
            Txt_IdentiteFiscale.Visibility = stat;
            lbl_Siege.Visibility = stat;
            Txt_Siege.Visibility = stat;
            lbl_NomMandataire.Visibility = stat;
            Txt_NomMandataire.Visibility = stat;
            lbl_PrenomMandatataire.Visibility = stat;
            Txt_PrenomMandataire.Visibility = stat;
            lbl_StatutJuridique.Visibility = stat;
            Cbo_StatutJuridique.Visibility = stat;
            lbl_DateCreation.Visibility = stat;
            dtp_DateCreation.Visibility = stat;
            lbl_RangSignataire.Visibility = stat;
            Txt_RangSignataire.Visibility = stat;
            lbl_PrenomSignataire.Visibility = stat;
            Txt_PrenomSignataire.Visibility = stat;
            lbl_NomSignateur.Visibility = stat;
            Txt_NomSignataire.Visibility = stat;
            lbl_RangMandataire.Visibility = stat;
            Txt_RangMandataire.Visibility = stat;

        }
        private void PersonPhysiqueWindows(Visibility stat)
        {
            this.lbl_NomClient.Visibility = stat;
            this.Txt_NomClient_PersonePhysiq.Visibility = stat;
            this.lbl_DateNaissance.Visibility = stat;
            this.dtp_DateNaissance.Visibility = stat;
            this.lbl_NaturePieceIdentite.Visibility = stat;
            this.Cbo_TypePiecePersonnePhysique.Visibility = stat;
            this.lbl_NumPiece.Visibility = stat;
            this.txtNumeroPiece.Visibility = stat;
            this.lbl_DateFinValidite.Visibility = stat;
            this.dtp_finvalidation.Visibility = stat;
        }

        private void RemplirInfoSocietePrive(CsDemande laDemande)
        {
            if (laDemande.LeClient.SocietePrives != null)
            {
                Pk_IdSocoiete = laDemande.LeClient.SocietePrives.PK_ID != null ? laDemande.LeClient.SocietePrives.PK_ID : 0;
                this.Txt_RegistreCommerce.Text = laDemande.LeClient.SocietePrives.NUMEROREGISTRECOMMERCE;
                this.Txt_Capital.Text = laDemande.LeClient.SocietePrives.CAPITAL.ToString();
                this.Txt_IdentiteFiscale.Text = laDemande.LeClient.SocietePrives.IDENTIFICATIONFISCALE;
                this.Txt_Siege.Text = laDemande.LeClient.SocietePrives.SIEGE;
                this.Txt_NomMandataire.Text = laDemande.LeClient.SocietePrives.NOMMANDATAIRE;
                this.Txt_PrenomMandataire.Text = laDemande.LeClient.SocietePrives.PRENOMMANDATAIRE;
                this.Txt_RangMandataire.Text = laDemande.LeClient.SocietePrives.RANGMANDATAIRE;
                this.Txt_NomSignataire.Text = laDemande.LeClient.SocietePrives.NOMSIGNATAIRE;
                this.Txt_PrenomSignataire.Text = laDemande.LeClient.SocietePrives.PRENOMSIGNATAIRE;
                this.Txt_RangSignataire.Text = laDemande.LeClient.SocietePrives.RANGSIGNATAIRE;
                this.dtp_DateCreation.SelectedDate = laDemande.LeClient.SocietePrives.DATECREATION;
                this.Cbo_StatutJuridique.SelectedItem = ListStatuJuridique.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.SocietePrives.FK_IDSTATUTJURIQUE);

            }

        }
        private void RemplirInfopersonnephysique(CsDemande laDemande)
        {
            if (laDemande.LeClient.PersonePhysique != null)
            {
                Pk_IdPersPhys = laDemande.LeClient.PersonePhysique.PK_ID != null ? laDemande.LeClient.PersonePhysique.PK_ID : 0;
                this.Txt_NomClient_PersonePhysiq.Text = laDemande.LeClient.PersonePhysique.NOMABON != null ? laDemande.LeClient.PersonePhysique.NOMABON : string.Empty;
                this.txtNumeroPiece.Text = laDemande.LeClient.PersonePhysique.NUMEROPIECEIDENTITE != null ? laDemande.LeClient.PersonePhysique.NUMEROPIECEIDENTITE : string.Empty;
                this.dtp_DateNaissance.SelectedDate = laDemande.LeClient.PersonePhysique.DATENAISSANCE != null ? laDemande.LeClient.PersonePhysique.DATENAISSANCE : DateTime.Now;
                this.dtp_finvalidation.SelectedDate = laDemande.LeClient.PersonePhysique.DATEFINVALIDITE != null ? laDemande.LeClient.PersonePhysique.DATEFINVALIDITE : DateTime.Now;
                this.Cbo_TypePiecePersonnePhysique.SelectedItem = ListeTYpePiece.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.PersonePhysique.FK_IDPIECEIDENTITE);
            }
            else
            {
                Cbo_TypePiecePersonnePhysiqueProprio.IsEnabled = true ;
                Cbo_TypePiecePersonnePhysiqueProprio.SelectedItem = SessionObject.LstTypeClient.First();
            
            }

        }
        private void RemplirInfoAdmnistrationInstitut(CsDemande laDemande)
        {
            if (laDemande.LeClient.AdministrationInstitut != null)
            {
                Pk_IdAdministration = laDemande.LeClient.AdministrationInstitut.PK_ID != null ? laDemande.LeClient.AdministrationInstitut.PK_ID : 0;
                this.Txt_NomMandataireAd.Text = laDemande.LeClient.AdministrationInstitut.NOMMANDATAIRE;
                this.Txt_PrenomMandataireAd.Text = laDemande.LeClient.AdministrationInstitut.PRENOMMANDATAIRE;
                this.Txt_RangMandataireAd.Text = laDemande.LeClient.AdministrationInstitut.RANGMANDATAIRE;
                this.Txt_NomSignataireAd.Text = laDemande.LeClient.AdministrationInstitut.NOMSIGNATAIRE;
                this.Txt_PrenomSignataireAd.Text = laDemande.LeClient.AdministrationInstitut.PRENOMSIGNATAIRE;
                this.Txt_RangSignataireAd.Text = laDemande.LeClient.AdministrationInstitut.RANGSIGNATAIRE;

            }

        }
        private void RemplirInfoPropritaire(CsDemande laDemande)
        {
            if (laDemande.LeClient.InfoProprietaire_ != null)
            {
                Pk_IdPropietaire = laDemande.LeClient.InfoProprietaire_.PK_ID != null ? laDemande.LeClient.InfoProprietaire_.PK_ID : 0;
                this.Txt_NomProprio_PersonePhysiq.Text = laDemande.LeClient.InfoProprietaire_.NOM != null ? laDemande.LeClient.InfoProprietaire_.NOM : string.Empty;
                this.Txt_PrenomProprio_PersonePhysiq.Text = laDemande.LeClient.InfoProprietaire_.PRENOM != null ? laDemande.LeClient.InfoProprietaire_.PRENOM : string.Empty;
                this.txtNumeroPieceProprio.Text = laDemande.LeClient.InfoProprietaire_.NUMEROPIECEIDENTITE != null ? laDemande.LeClient.InfoProprietaire_.NUMEROPIECEIDENTITE : string.Empty;
                this.dtp_finvalidationProprio.SelectedDate = laDemande.LeClient.InfoProprietaire_.DATEFINVALIDITE != null ? laDemande.LeClient.InfoProprietaire_.DATEFINVALIDITE : DateTime.Now;
                this.txt_Telephone_Proprio.Text = laDemande.LeClient.InfoProprietaire_.TELEPHONEMOBILE != null ? laDemande.LeClient.InfoProprietaire_.TELEPHONEMOBILE : string.Empty;
                this.Txt_Email_Proprio.Text = laDemande.LeClient.InfoProprietaire_.EMAIL != null ? laDemande.LeClient.InfoProprietaire_.EMAIL : string.Empty;
                this.Cbo_TypePiecePersonnePhysiqueProprio.SelectedItem = ListeTYpePiece.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.InfoProprietaire_.FK_IDPIECEIDENTITE) != null ? ListeTYpePiece.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.InfoProprietaire_.FK_IDPIECEIDENTITE) : null;
                this.dtp_DateNaissanceProprio.SelectedDate = laDemande.LeClient.InfoProprietaire_.DATENAISSANCE != null ? laDemande.LeClient.InfoProprietaire_.DATENAISSANCE : DateTime.Now;
                this.Txt_Faxe_Proprio.Text = laDemande.LeClient.InfoProprietaire_.FAX != null ? laDemande.LeClient.InfoProprietaire_.FAX : string.Empty;
                this.Txt_BoitePosta_Proprio.Text = laDemande.LeClient.InfoProprietaire_.BOITEPOSTALE != null ? laDemande.LeClient.InfoProprietaire_.BOITEPOSTALE : string.Empty;
                this.Cbo_Nationalite_Proprio.SelectedItem = SessionObject.LstDesNationalites.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.InfoProprietaire_.FK_IDNATIONNALITE) != null ? SessionObject.LstDesNationalites.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.InfoProprietaire_.FK_IDNATIONNALITE) : null;

            }

        }


        void formUcSocietePrive_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //laDetailDemande.LeClient.PersonePhysique = new CsPersonePhysique();

            NUMEROREGISTRECOMMERCE = ((UcSocietePrive)sender).Txt_RegistreCommerce.Text;
            decimal capital = 0;
            decimal.TryParse(((UcSocietePrive)sender).Txt_Capital.Text, out capital);
            this.FK_IDSTATUTJURIQUE = (CsStatutJuridique)((UcSocietePrive)sender).Cbo_StatutJuridique.SelectedItem != null ? ((CsStatutJuridique)((UcSocietePrive)sender).Cbo_StatutJuridique.SelectedItem).PK_ID : 0;
            CAPITAL = capital;
            IDENTIFICATIONFISCALE = ((UcSocietePrive)sender).Txt_IdentiteFiscale.Text;
            DATECREATION = ((UcSocietePrive)sender).dtp_DateCreation.SelectedDate != null ? ((UcSocietePrive)sender).dtp_DateCreation.SelectedDate : DateTime.Now;
            SIEGE = ((UcSocietePrive)sender).Txt_Siege.Text;
            NOMMANDATAIRE = ((UcSocietePrive)sender).Txt_NomMandataire.Text;
            PRENOMMANDATAIRE = ((UcSocietePrive)sender).Txt_PrenomMandataire.Text;
            RANGMANDATAIRE = ((UcSocietePrive)sender).Txt_RangMandataire.Text;
            NOMSIGNATAIRE = ((UcSocietePrive)sender).Txt_NomSignataire.Text;
            PRENOMSIGNATAIRE = ((UcSocietePrive)sender).Txt_PrenomSignataire.Text;
            RANGSIGNATAIRE = ((UcSocietePrive)sender).Txt_RangMandataire.Text;
            //RANGMANDATAIRE = ((UcSocietePrive)sender).RANGMANDATAIRE;
            //datenaissance = ((UcSocietePrive)sender).datenaissance;
            //numeropiece = ((UcSocietePrive)sender).numeropiece;
            //typepiece = ((ObjPIECEIDENTITE)((UcSocietePrive)sender).Cbo_TypePiece.SelectedItem).PK_ID;
            //laDetailDemande.LeClient.PersonePhysique. = string.IsNullOrEmpty(((UcPersone_Physique)sender).txtNumeroPiece.Text) ? null : ((UcPersone_Physique)sender).txtNumeroPiece.Text;
            //laDetailDemande.LeClient.PersonePhysique.NUMEROPIECEIDENTITE = string.IsNullOrEmpty(((UcPersone_Physique)sender).txtNumeroPiece.Text) ? null : ((UcPersone_Physique)sender).txtNumeroPiece.Text; 


        }

        #endregion

        private void Txt_NumDevis_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_RefClient_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_RefBranch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void RetourneOrdre(CsClient leClient)
        {
            try
            {
                string OrdreMax = string.Empty;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneOrdreMaxCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    OrdreMax = args.Result;
                    if (OrdreMax != null)
                    {
                        leClient.ORDRE = OrdreMax; 
                        ChargeDetailDEvis(leClient);
                    }
                };
                service.RetourneOrdreMaxAsync(leClient.FK_IDCENTRE.Value , leClient.CENTRE, leClient.REFCLIENT, leClient.PRODUIT );
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargeDetailDEvis(CsClient leclient)
        {

            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GeDetailByFromClientCompleted += (ssender, args) =>
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

                        laDetailDemande = new CsDemande();
                        laDetailDemande = args.Result;
                        if (laDetailDemande.LeClient != null && laDetailDemande.LeClient.PersonePhysique == null &&
                            laDetailDemande.LeClient.SocietePrives == null &&
                            laDetailDemande.LeClient.AdministrationInstitut == null)
                        {
                            PersonPhysiqueWindows(Visibility.Visible);
                            this.Txt_NomClient_PersonePhysiq.Text = laDetailDemande.LeClient.NOMABON != null ? laDetailDemande.LeClient.NOMABON : string.Empty;
                            this.txtNumeroPiece.Text = laDetailDemande.LeClient.NUMEROPIECEIDENTITE != null ? laDetailDemande.LeClient.NUMEROPIECEIDENTITE : string.Empty;
                        }

                        RenseignerChampsSurLeControl(laDetailDemande);
                        if (((CsTdem)txt_tdem.Tag).CODE != SessionObject.Enumere.BranchementAbonement &&
                            ((CsTdem)txt_tdem.Tag).CODE != SessionObject.Enumere.AbonnementSeul &&
                            ((CsTdem)txt_tdem.Tag).CODE != SessionObject.Enumere.Reabonnement)
                        {
                            Btn_ListeAppareils.Visibility = Visibility.Collapsed;
                            Cbo_ListeAppareils.Visibility = Visibility.Collapsed;
                            lab_ListeAppareils.Visibility = Visibility.Collapsed;
                            EnabledDemandeDevisInformationsByTdem(false);
                        }
                        else
                            EnabledDemandeDevisInformationsByTdem(true);


                        if (((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.AbonnementSeul)
                            this.txt_ordre.Text = (int.Parse(laDetailDemande.Abonne.ORDRE) + 1).ToString("00");
                        else
                            this.txt_ordre.Text = laDetailDemande.Abonne.ORDRE;

                        if (((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.Resiliation && laDetailDemande.Abonne.DRES != null)
                        {
                            throw new Exception("Abonné deja résilié ");
                        }

                        desableProgressBar();
                    }
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.GeDetailByFromClientAsync(leclient);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void chk_Email_Checked(object sender, RoutedEventArgs e)
        {
            if (!chk_Email.IsChecked.Value)
            {
                Txt_Email.Text = string.Empty;
            }
            Txt_Email.IsEnabled = chk_Email.IsChecked.Value;
            ActiverEnregistrerOuTransmettre();
        }

        private void chk_SMS_Checked(object sender, RoutedEventArgs e)
        {
            if (!chk_SMS.IsChecked.Value)
            {
                txt_Telephone.Text = string.Empty;
            }
            txt_Telephone.IsEnabled = chk_SMS.IsChecked.Value;
            ActiverEnregistrerOuTransmettre();
        }

        private void tabC_Onglet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Txt_Capital_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal capital = 0;
            if (!decimal.TryParse(Txt_Capital.Text, out capital))
            {
                Message.Show("veuillez saisir une valeur numerique", "Demande");
            }
            ActiverEnregistrerOuTransmettre();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Cbo_Site.SelectedItem != null && Cbo_Centre.SelectedItem != null && Cbo_Produit.SelectedItem != null)
                {
                    if (txt_Ref_Branchement.Text.Length == 11)
                    {
                        allowProgressBar();
                        CsClient leClient = new CsClient();
                        leClient.FK_IDCENTRE = (int)this.txtCentre.Tag;
                        leClient.CENTRE = this.txtCentre.Text;
                        leClient.REFCLIENT = this.txt_Ref_Branchement.Text;
                        leClient.PRODUIT = this.txtProduit.Text;
                        leClient.TYPEDEMANDE = ((CsTdem)txt_tdem.Tag).CODE;
                        RetourneOrdre(leClient);
                    }
                }
                else
                {
                    Message.Show("Veuillez vous assurer que le site le centre et le produit soit selectionné", "Infomation");
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }
        void allowProgressBar()
        {
            progressBar1.IsEnabled = true;
            progressBar1.Visibility = Visibility.Visible;
            progressBar1.IsIndeterminate = true;
        }

        void desableProgressBar()
        {
            progressBar1.IsIndeterminate = false;
            progressBar1.Visibility = Visibility.Collapsed;
        }
        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        private byte[] image;
        private int Pk_IdPropietaire = 0;

        private int Pk_IdPersPhys = 0;
        private int Pk_IdSocoiete = 0;
        private int Pk_IdAdministration = 0;
        private int PK_Id_Tdem = 0;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (cbo_typedoc.SelectedItem != null)
            {
                // Create an instance of the open file dialog box.
                var openDialog = new OpenFileDialog();
                // Set filter options and filter index.
                openDialog.Filter =
                    "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                openDialog.FilterIndex = 1;
                openDialog.Multiselect = false;
                // Call the ShowDialog method to show the dialog box.
                bool? userClickedOk = openDialog.ShowDialog();
                // Process input if the user clicked OK.
                if (userClickedOk == true)
                {
                    if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                    {
                        FileStream stream = openDialog.File.OpenRead();
                        var memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        image = memoryStream.GetBuffer();
                        formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                        formScanne.Closed += new EventHandler(GetInformationFromChildWindowImagePreuve);
                        formScanne.Show();
                    }
                }
            }
        }

        private void GetInformationFromChildWindowImagePreuve(object sender, EventArgs e)
        {
            this.LstPiece.Add(new ObjDOCUMENTSCANNE { PK_ID = Guid.NewGuid(), NOMDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).LIBELLE, FK_IDTYPEDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).PK_ID, CONTENU = image, DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule });
            this.dgListePiece.ItemsSource = this.LstPiece;
            if (LstPiece.Count() > 0)
            {
                this.isPreuveSelectionnee = true;
                EnabledDevisInformations(true);
            }
            else
            {
                this.isPreuveSelectionnee = false;
                EnabledDevisInformations(false);
            }
            ActiverEnregistrerOuTransmettre();
        }

        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Ête-vous sure de vouloire supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    ObjDOCUMENTSCANNE Fraix = (ObjDOCUMENTSCANNE)dgListePiece.SelectedItem;
                    this.LstPiece.Remove(Fraix);
                    this.dgListePiece.ItemsSource = this.LstPiece;
                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }

        private void txt_Telephone_TextChanged(object sender, TextChangedEventArgs e)
        {

            //if (!string.IsNullOrWhiteSpace(txt_Telephone.Text))
            //{
            //    double telephone;
            //    if (!double.TryParse(txt_Telephone.Text, out telephone))
            //    {
            //        Message.Show("Veuillez saisir un numéro de phone mobile valide", "Erreur");
            //        txt_Telephone.Focus();
            //    }
            //}
            ActiverEnregistrerOuTransmettre();
        }

        private void txt_Telephone_Fixe_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_Telephone_Fixe.Text))
            {
                //double telephone;
                //if (!double.TryParse(txt_Telephone_Fixe.Text, out telephone))
                //{
                //    Message.Show("Veuillez saisir un numéro de phone fixe valide", "Erreur");
                //    txt_Telephone_Fixe.Text = string.Empty;
                //    txt_Telephone_Fixe.Focus();

                //}
                ActiverEnregistrerOuTransmettre();
            }
        }

        private void Cbo_Type_Proprietaire_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cbo_Type_Proprietaire.SelectedItem != null)
            {
                var typeproprio = (CsProprietaire)Cbo_Type_Proprietaire.SelectedItem;
                if (typeproprio.CODE == "1" || typeproprio.CODE == "2")
                {
                    tab_proprio.Visibility = Visibility.Visible;
                }
                else
                {
                    tab_proprio.Visibility = Visibility.Collapsed;
                }
                ActiverEnregistrerOuTransmettre();
            }
        }

      

        private void txt_Telephone_Proprio_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_Telephone_Proprio.Text))
            {
                double telephone;
                if (!double.TryParse(txt_Telephone_Proprio.Text, out telephone))
                {
                    Message.Show("Veuillez saisir un numéro de phone mobile proprietaire valide", "Erreur");
                    txt_Telephone_Proprio.Focus();
                }
            }

        }

        private void dtp_DateNaissance_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {

            ActiverEnregistrerOuTransmettre();
        }

        private void Cbo_TypePiecePersonnePhysique_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void dtp_finvalidation_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_RegistreCommerce_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_IdentiteFiscale_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_Siege_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_NomMandataire_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_PrenomMandataire_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Cbo_StatutJuridique_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void dtp_DateCreation_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_RangSignataire_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_PrenomSignataire_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_NomSignataire_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_RangMandataire_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_Denomination_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_PrenomMandataireAd_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_NomMandataireAd_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_RangSignataireAd_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_PrenomSignataireAd_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_NomSignataireAd_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_RangMandataireAd_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_Numeronina_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_Email_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

        private void Txt_NomClient_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrerOuTransmettre();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void cbo_typedoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}


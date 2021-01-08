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

namespace Galatee.Silverlight.Accueil
{
    public partial class UcInitialisationEP : ChildWindow
    {
        private UcImageScanne formScanne = null;
        private Object ModeExecution = null;
        private List<CsTournee> _listeDesTourneeExistant = null;
        private List<CsCommune> _listeDesCommuneExistant = null;
        private List<CsCommune> _listeDesCommuneExistantCentre = null;
        private List<CsReglageCompteur> _listeDesReglageCompteurExistant = null;
        private List<CsCentre> _listeDesCentreExistant = null;
        private  CsTdem _leTypeDemandeExistant = null;
        private string Tdem = null;
        private List<CsRues> _listeDesRuesExistant = null;
        private List<CsQuartier> _listeDesQuartierExistant = null;
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


        public UcInitialisationEP()
        {
            InitializeComponent();
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

        public UcInitialisationEP(int _IdDemandeDevis, int PK_Id_Tdem = 0)
        {
            try
            {
                InitializeComponent();
                this.PK_Id_Tdem = PK_Id_Tdem;

                label21.Visibility = Visibility.Collapsed;
                Cbo_ReglageCompteur.Visibility = Visibility.Collapsed;

                ModeExecution = SessionObject.ExecMode.Modification;
                IdDemandeDevis = _IdDemandeDevis;

                //RemplirProprietaire();
                ChargerTypeDocument();
                ChargerCategorieClient_TypeClient();
                ChargerNatureClient_TypeClient();
                ChargerUsage_NatureClient();
                ChargerCategorieClient_Usage();
                RemplirTourneeExistante();
                RemplirCategorieClient();
                RemplirUsage();
                RemplirCodeRegroupement();
                RemplirCodeConsomateur();
                RemplirSecteur();
                RemplirCommune();
                RemplirProduit();
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

        public UcInitialisationEP(string Tdem, string IsInit)
        {
            try
            {
                InitializeComponent();
                this.Tdem = Tdem;
                ModeExecution = SessionObject.ExecMode.Creation;

                Txt_Porte.MaxLength = 5;

                ChargerPuissance();
                ChargerTypeDocument();
                ChargerCategorieClient_TypeClient();
                ChargerNatureClient_TypeClient();
                ChargerUsage_NatureClient();
                ChargerCategorieClient_Usage();
                RemplirListeDesTypeDemandeExistant();
                RemplirTourneeExistante();
                RemplirCategorieClient();
                RemplirPieceIdentite();
                RemplirUsage();
                RemplirCodeRegroupement();
                RemplirCodeConsomateur();
                RemplirSecteur();
                RemplirNationnalite();
                RemplirCommune();
                RemplirProduit();
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
        private void ChargerListeDeProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit != null && SessionObject.ListeDesProduit.Count != 0)
                    return;
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = res.Result;
                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerPuissance()
        {
            try
            {
                if (SessionObject.LstPuissance != null && SessionObject.LstPuissance.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceSouscriteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissance = args.Result;
                };
                service.ChargerPuissanceSouscriteAsync();
                service.CloseAsync();
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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

        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
        private bool  VerifieChampObligation()
        {
           try
           {
               bool ReturnValue = true;
               if (((CsTdem)this.txt_tdem.Tag).CODE == SessionObject.Enumere.BranchementAbonnementEp ) 
               {
                   

                   #region information abonnement

                   if (string.IsNullOrEmpty(this.TxtCategorieClient.Text))
                       throw new Exception("Selectionnez la catégorie du client ");

                   if (string.IsNullOrEmpty(this.Txt_CodeConso.Text))
                       throw new Exception("Selectionnez le code consommateur ");

                   if (string.IsNullOrEmpty(this.Txt_CodeConso.Text))
                       throw new Exception("Selectionnez le code consommateur ");

                   if (((CsProduit)Cbo_Produit.SelectedItem).CODE != SessionObject.Enumere.ElectriciteMT)
                   {
                       if (Cbo_ReglageCompteur.SelectedItem == null)
                           throw new Exception("Selectionnez le calibre ");
                   }
                   #endregion
                   #region Adresse géographique
                   if (string.IsNullOrEmpty(this.txt_Commune.Text))
                       throw new Exception("Séléctionnez la commune ");

                   if (string.IsNullOrEmpty(this.txt_Quartier.Text))
                       throw new Exception("Séléctionnez le quartier ");
                   #endregion
               }
               return ReturnValue;

           }
	        catch (Exception ex)
	        {
                this.Btn_Transmettre.IsEnabled = true ;
                this.Btn_Enregistrer.IsEnabled = true;
		        Message.ShowInformation(ex.Message ,"Accueil");
                return false;
	        }
        
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Btn_Enregistrer.IsEnabled = false ;
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
                
                if (!VerifieChampObligation()) return;
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
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    client.ValiderDemandeInitailisationCompleted += (ss, b) =>
                    {
                        if (b.Cancelled || b.Error != null)
                        {
                            string error = b.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        string numedemande = string.Empty;
                        string Client = string.Empty;
                        if (IsTransmetre)
                        {
                            string Retour = b.Result;
                            string[] coupe = Retour.Split('.');
                            Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], demandedevis.LaDemande.FK_IDCENTRE, coupe[1], demandedevis.LaDemande.FK_IDTYPEDEMANDE);
                            numedemande = coupe[1];
                            Client = coupe[2];
                        }
                        List<CsDemandeBase> leDemandeAEditer = new List<CsDemandeBase>();
                        demandedevis.LaDemande.NOMCLIENT = demandedevis.LeClient.NOMABON;
                        demandedevis.LaDemande.LIBELLETYPEDEMANDE = txt_tdem.Text;
                        demandedevis.LaDemande.NUMDEM = numedemande;
                        demandedevis.LaDemande.CLIENT = Client;
                        demandedevis.LaDemande.LIBELLECOMMUNE = ((CsCommune)this.Cbo_Commune.SelectedItem).LIBELLE;
                        demandedevis.LaDemande.LIBELLEQUARTIER = this.Cbo_Quartier.SelectedItem != null ? ((CsQuartier)this.Cbo_Quartier.SelectedItem).LIBELLE : string.Empty;
                        demandedevis.LaDemande.ANNOTATION = string.IsNullOrEmpty(demandedevis.LeClient.TELEPHONE) ? string.Empty : demandedevis.LeClient.TELEPHONE;
                        demandedevis.LaDemande.NOMPERE = string.IsNullOrEmpty(demandedevis.LeClient.TELEPHONE) ? string.Empty : demandedevis.Ag.RUE;
                        demandedevis.LaDemande.NOMMERE = string.IsNullOrEmpty(demandedevis.LeClient.PORTE) ? string.Empty : demandedevis.Ag.PORTE;
                        demandedevis.LaDemande.LIBELLEPRODUIT = ((CsProduit)this.Cbo_Produit.SelectedItem).LIBELLE;
                        demandedevis.LaDemande.LIBELLE = "Imprimé le " + DateTime.Now + " sur le poste " + SessionObject.LePosteCourant.NOMPOSTE + " par " + UserConnecte.nomUtilisateur + "(" + UserConnecte.matricule + ") du centre " + UserConnecte.LibelleCentre;

                        leDemandeAEditer.Add(demandedevis.LaDemande);
                        Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(leDemandeAEditer, null, SessionObject.CheminImpression, "AccuseRecption", "Accueil", true);
                        FermerFenetre();
                    };
                    client.ValiderDemandeInitailisationAsync(demandedevis);
                }
            }
            catch (Exception ex)
            {
                this.Btn_Transmettre.IsEnabled = true;
                Message.ShowError("Une erreur s'est produite a la validation ", "ValiderDemandeInitailisation");
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

        private void RemplirCommune()
        {
            try
            {
                if (SessionObject.LstCommune != null && SessionObject.LstCommune.Count != 0)
                {
                    _listeDesCommuneExistant = SessionObject.LstCommune;
                    _listeDesCommuneExistant = RemplirDistinctCommune(SessionObject.LstCommune).OrderBy(t => t.LIBELLE).ToList();
                    Cbo_Commune.ItemsSource = _listeDesCommuneExistant;
                    Cbo_Commune.IsEnabled = true;
                    Cbo_Commune.SelectedValuePath = "PK_ID";
                    Cbo_Commune.DisplayMemberPath = "LIBELLE";
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCommuneAsync();
                service.ChargerCommuneCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    SessionObject.LstCommune = args.Result;
                    _listeDesCommuneExistant = RemplirDistinctCommune(SessionObject.LstCommune).OrderBy(t => t.LIBELLE).ToList();
                    Cbo_Commune.ItemsSource = _listeDesCommuneExistant;
                    Cbo_Commune.IsEnabled = true;
                    Cbo_Commune.SelectedValuePath = "PK_ID";
                    Cbo_Commune.DisplayMemberPath = "LIBELLE";

                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<CsCommune> RemplirDistinctCommune(List<CsCommune> lstCommune)
        {
            try
            {
                List<CsCommune> lstCentreDist = new List<CsCommune>();
                var lstCentreDistnct = lstCommune.Select(t => new { LIBELLE = t.LIBELLE.ToUpper(), t.CODE }).Distinct().ToList();
                foreach (var item in lstCentreDistnct)
                    lstCentreDist.Add(lstCommune.FirstOrDefault(t => t.CODE == item.CODE && t.LIBELLE == item.LIBELLE));

                return lstCentreDist;
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
                if (SessionObject.LstReglageCompteur != null && SessionObject.LstReglageCompteur.Count != 0)
                {
                    _listeDesReglageCompteurExistant = SessionObject.LstReglageCompteur;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerReglageCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstReglageCompteur  = args.Result;
                    _listeDesReglageCompteurExistant = SessionObject.LstReglageCompteur;

                };
                service.ChargerReglageCompteurAsync();
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

                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
        //    List<CsTournee> lTourneeDuCentre = null;
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
       private void RemplirProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit != null && SessionObject.ListeDesProduit.Count != 0)
                    return;
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = res.Result;
                    Cbo_Produit.ItemsSource = null;
                    Cbo_Produit.ItemsSource = SessionObject.ListeDesProduit;
                    Cbo_Produit.SelectedValuePath = "PK_ID";
                    Cbo_Produit.DisplayMemberPath = "LIBELLE";
                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
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
                    CsCategorieClient laCateg = SessionObject.LstCategorie.FirstOrDefault(t=>t.CODE == SessionObject.Enumere.CategorieEp );
                    if (laCateg != null)
                    {
                        this.TxtCategorieClient.Text = laCateg.LIBELLE;
                        this.TxtCategorieClient.Tag  = laCateg.PK_ID ;

                    }
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCategorieCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCategorie = args.Result;
                    CsCategorieClient laCateg = SessionObject.LstCategorie.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CategorieEp);
                    if (laCateg != null)
                    {
                        this.TxtCategorieClient.Text = laCateg.LIBELLE;
                        this.TxtCategorieClient.Tag = laCateg.PK_ID;

                    }
                };
                service.RetourneCategorieAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneNationnaliteCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstDesNationalites = args.Result;

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


        private void RemplirCodeConsomateur()
        {
            try
            {
                if (SessionObject.LstCodeConsomateur.Count != 0)
                {
                    CsCodeConsomateur leCodeConsoClient = SessionObject.LstCodeConsomateur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeConsomateurEp);
                    if (leCodeConsoClient != null)
                    {
                        Txt_CodeConso.Text = leCodeConsoClient.LIBELLE;
                        this.Txt_CodeConso.Tag = leCodeConsoClient.PK_ID;
                    }
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneCodeConsomateurCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeConsomateur = args.Result;

                        CsCodeConsomateur leCodeConsoClient = SessionObject.LstCodeConsomateur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeConsomateurEp);
                        if (leCodeConsoClient != null)
                        {
                            Txt_CodeConso.Text = leCodeConsoClient.LIBELLE;
                            this.Txt_CodeConso.Tag = leCodeConsoClient.PK_ID;
                        }
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
                    Cbo_Regroupement.ItemsSource = SessionObject.LstCodeRegroupement.OrderBy(t=>t.NOM).ToList();
                    Cbo_Regroupement.SelectedValuePath = "PK_ID";
                    Cbo_Regroupement.DisplayMemberPath = "NOM";
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneCodeRegroupementCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeRegroupement = args.Result;
                        Cbo_Regroupement.Items.Clear();
                        Cbo_Regroupement.ItemsSource = SessionObject.LstCodeRegroupement.OrderBy(t => t.NOM).ToList();
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

        #region sylla le 22/09/2015

        private void RemplirPieceIdentite()
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                    ListeTYpePiece = args.Result;

                    VerifierTypePiece();
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirUsage()
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                    if (args.Result != null && args.Result.Count > 0)
                        lstusage = args.Result;


                    CsUsage  leUsageClient = lstusage.FirstOrDefault(t => t.CODE  == SessionObject.Enumere.UsageEp );
                    if (leUsageClient != null)
                    {
                        TxtUsageClient.Text = leUsageClient.LIBELLE;
                        this.TxtUsageClient.Tag = leUsageClient.PK_ID;
                    }
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirTypeClient()
        {
            try
            {
                if (SessionObject.LstTypeClient.Count != 0)
                {
                    return;
                }
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                    SessionObject.LstTypeClient = args.Result;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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

        private void RemplirListeDesQuartierExistant()
        {
            try
            {

                if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
                {
                    _listeDesQuartierExistant = SessionObject.LstQuartier;
                    return;
                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
            List<CsReglageCompteur > ListeDesDiametreFiltrees = null;
            try
            {
                if (_listeDesReglageCompteurExistant != null &&
                    _listeDesReglageCompteurExistant.FirstOrDefault(p => p.FK_IDPRODUIT == pIdProduit) != null)
                {
                    ListeDesDiametreFiltrees = _listeDesReglageCompteurExistant.Where(q => q.FK_IDPRODUIT == pIdProduit).ToList();
                }
                if (ListeDesDiametreFiltrees != null && ListeDesDiametreFiltrees.Count > 0)

                    Cbo_ReglageCompteur.ItemsSource = null;
                Cbo_ReglageCompteur.ItemsSource = ListeDesDiametreFiltrees;
                Cbo_ReglageCompteur.SelectedValuePath = "PK_ID";
                Cbo_ReglageCompteur.DisplayMemberPath = "LIBELLE";


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
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
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
                        RemplirDiametreCompteur(produit.PK_ID);
                        if (produit.CODE == SessionObject.Enumere.Eau)
                        {
                            label21.Visibility = Visibility.Collapsed;
                            Cbo_ReglageCompteur.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            label21.Visibility = Visibility.Visible;
                            Cbo_ReglageCompteur.Visibility = Visibility.Visible;
                        }
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
                        if (laDemande.LaDemande.FK_IDCENTRE != 0)
                        {
                            //if (_listeDesCentreExistant != null && _listeDesCentreExistant.Count != 0)
                            //{
                            //    if (_listeDesCentreExistant != null)
                            //        foreach (var item in _listeDesCentreExistant)
                            //        {
                            //            Cbo_Centre.Items.Add(item);
                            //        }


                            //    CsCentre leCentreSelect = _listeDesCentreExistant.First(t => t.PK_ID == laDemande.LaDemande.FK_IDCENTRE);
                            //    this.Cbo_Centre.SelectedItem = leCentreSelect;

                            //    List<CsSite> lstSite = RetourneSiteByCentre(_listeDesCentreExistant);
                            //    if (lstSite != null)
                            //        foreach (var item in lstSite)
                            //        {
                            //            Cbo_Site.Items.Add(item);
                            //        }
                            //    CsSite leSite = lstSite.FirstOrDefault(t => t.PK_ID == leCentreSelect.FK_IDCODESITE);
                            //    Cbo_Site.SelectedItem = leSite;
                            //}

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
                        }
                    }

                    #endregion
                    if (((CsTdem)txt_tdem.Tag).CODE == SessionObject.Enumere.AbonnementSeul)
                        return;

                    #region Client
                    if (laDemande.LeClient != null)
                    {

                        if (laDemande.LeClient.FK_TYPECLIENT != null && laDemande.LeClient.FK_TYPECLIENT != 0)
                        {
                            CsTypeClient TypeClient = SessionObject.LstTypeClient.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_TYPECLIENT);
                            //this.Cbo_Type_Client.SelectedItem = TypeClient;
                        }
                        if (laDemande.LeClient.FK_TYPECLIENT == null || laDemande.LeClient.FK_TYPECLIENT == 0)
                        {
                            CsTypeClient TypeClient = SessionObject.LstTypeClient.FirstOrDefault(t => t.CODE.Trim() == "001");
                            //this.Cbo_Type_Client.SelectedItem = TypeClient;
                        }

                        if (laDemande.LeClient.FK_IDNATURECLIENT != 0)
                        {
                            ServiceAccueil.CsNatureClient laNature = SessionObject.LstNatureClient.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_IDNATURECLIENT);
                        }

                        if (laDemande.LeClient.FK_IDUSAGE != 0)
                        {
                        }

                        if (laDemande.LeClient.FK_IDNATIONALITE != 0)
                        {
                            ServiceAccueil.CsNationalite laNation = SessionObject.LstDesNationalites.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_IDNATIONALITE);
                            //this.Cbo_TypeDePiece.SelectedItem = laNation;
                        }
                        //foreach (ObjPIECEIDENTITE piece in Cbo_TypePiecePersonnePhysique.Items)
                        //{
                        //    if (piece.PK_ID == laDemande.LeClient.FK_IDPIECEIDENTITE)
                        //    {
                        //        Cbo_TypePiecePersonnePhysique.SelectedItem = piece;
                        //        break;
                        //    }
                        //}

                        if (laDemande.LeClient.FK_IDCODECONSO != 0)
                        {
                            ServiceAccueil.CsCodeConsomateur codeConso = SessionObject.LstCodeConsomateur.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_IDCODECONSO);
                        }
                        if (laDemande.LeClient.FK_IDREGROUPEMENT != 0)
                        {
                            ServiceAccueil.CsRegCli regroup = SessionObject.LstCodeRegroupement.FirstOrDefault(t => t.PK_ID == laDemande.LeClient.FK_IDREGROUPEMENT);
                            this.Cbo_Regroupement.SelectedItem = regroup;
                        }
                        if (!string.IsNullOrWhiteSpace(laDemande.LeClient.PROPRIO))
                        {
                             CsProprietaire typeprop = SessionObject.Lsttypeprop.FirstOrDefault(t => t.CODE == laDemande.LeClient.PROPRIO);
                        }
                        TxtCategorieClient.Text = !string.IsNullOrEmpty(laDemande.LeClient.CATEGORIE) ? laDemande.LeClient.CATEGORIE : string.Empty;
                        txtAdresse.Text = !string.IsNullOrEmpty(laDemande.LeClient.ADRMAND1) ? laDemande.LeClient.ADRMAND1 : string.Empty;

                    }
                    #endregion
                    #region Ag
                    if (laDemande.Ag != null)
                    {
                        txt_Commune.Text = !string.IsNullOrEmpty(laDemande.Ag.COMMUNE) ? laDemande.Ag.COMMUNE : string.Empty;
                        txt_Quartier.Text = !string.IsNullOrEmpty(laDemande.Ag.QUARTIER) ? laDemande.Ag.QUARTIER : string.Empty;
                        txt_NumRue.Text = !string.IsNullOrEmpty(laDemande.Ag.RUE) ? laDemande.Ag.RUE : string.Empty;

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
                    #region brt
                    if (SessionObject.LstReglageCompteur != null && SessionObject.LstReglageCompteur.Count != 0 && laDemande.Branchement != null && !string.IsNullOrEmpty(laDemande.Branchement.DIAMBRT))
                        this.Cbo_ReglageCompteur.SelectedItem = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.CODE == laDemande.Branchement.DIAMBRT);
                    #endregion
                    
                }
               
            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur à l'affichage des données", "Init");
            }
        }

        private void MakeReadOnlyOrEnabledClientInformation(bool pValue)
        {
            try
            {
                 
                txt_Commune.IsReadOnly = pValue;
                Cbo_Quartier.IsEnabled = pValue;
                Cbo_Rue.IsEnabled = pValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void EnabledDevisInformations(bool pValue)
        {
            try
            {
                //Cbo_Site.IsEnabled = pValue;
                //Cbo_Centre.IsEnabled = pValue;
                Cbo_Produit.IsEnabled = pValue;
                ActiverEnregistrerOuTransmettre();
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
                if (this.Tdem != SessionObject.Enumere.BranchementAbonement)
                {
                    label21.Visibility = Visibility.Collapsed;
                    Cbo_ReglageCompteur.Visibility = Visibility.Collapsed;
                }
                else
                {
                    
                    label21.Visibility = Visibility.Visible;
                    Cbo_ReglageCompteur.Visibility = Visibility.Visible;
                }
            }

            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        CsCentre leCentreSelect = new CsCentre();
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
                        CsCommune CentreCommune = SessionObject.LstCommune.LastOrDefault(t => t.CODE == ((CsCommune)Cbo_Commune.SelectedItem).CODE);
                        leCentreSelect = SessionObject.LstCentre.FirstOrDefault (t => t.CODE  == CentreCommune.CENTRE  && t.CODESITE == SessionObject.Enumere.CodeSiteScaBT ) ;
                        RemplirProduitCentre(leCentreSelect);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void _Click(object sender, RoutedEventArgs e)
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

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
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                }
                pDemandeDevis.LaDemande.ISBONNEINITIATIVE = this.rdb_IsBonneInitiative.IsChecked == true ? true : false;
                pDemandeDevis.LaDemande.ISEDM  = this.rdb_IsEdm.IsChecked == true ? true : false;
                pDemandeDevis.LaDemande.ISCOMMUNE = this.rdb_IsCommune.IsChecked == true ? true : false;
                pDemandeDevis.LaDemande.NOMBREDEFOYER = string.IsNullOrEmpty(this.Txt_NombreFoyer.Text) ? 0 :int.Parse(this.Txt_NombreFoyer.Text);

                if (Cbo_ReglageCompteur.SelectedItem != null)
                {
                    var reglage = Cbo_ReglageCompteur.SelectedItem as CsReglageCompteur;
                    if (reglage != null)
                    {
                        pDemandeDevis.LaDemande.REGLAGECOMPTEUR = reglage.CODE;
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
                if (leCentreSelect != null )
                {
                    pDemandeDevis.LaDemande.FK_IDCENTRE = leCentreSelect.PK_ID;
                    pDemandeDevis.LaDemande.CENTRE  = leCentreSelect.CODE  ;
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
                #region Client
                if (pDemandeDevis.LeClient == null)
                    pDemandeDevis.LeClient = new CsClient();


                pDemandeDevis.LeClient.NOMABON = Txt_NomClient_PersonePhysiq.Text ;

                if (TxtUsageClient.Tag != null)
                    pDemandeDevis.LeClient.FK_IDUSAGE = (int)TxtUsageClient.Tag;

                if (TxtCategorieClient.Tag != null)
                {
                    CsCategorieClient laCateg = SessionObject.LstCategorie.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CategorieEp);
                    pDemandeDevis.LeClient.FK_IDCATEGORIE = laCateg.PK_ID ;
                    pDemandeDevis.LeClient.CATEGORIE = laCateg.CODE ;
                }
             
                if (this.Txt_CodeConso.Tag != null)
                {
                    CsCodeConsomateur leCodeConsoClient = SessionObject.LstCodeConsomateur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeConsomateurEp);
                    if (leCodeConsoClient != null)
                    {
                        pDemandeDevis.LeClient.CODECONSO = leCodeConsoClient.CODE;
                        pDemandeDevis.LeClient.FK_IDCODECONSO   = leCodeConsoClient.PK_ID ;
                    }
                }
                pDemandeDevis.LeClient.FK_IDPROPRIETAIRE = SessionObject.Lsttypeprop.First().PK_ID;
                pDemandeDevis.LeClient.PROPRIO = SessionObject.Lsttypeprop.First().CODE ; ;

                pDemandeDevis.LeClient.NATIONNALITE = SessionObject.LstDesNationalites.FirstOrDefault().CODE ;
                pDemandeDevis.LeClient.FK_IDNATIONALITE = SessionObject.LstDesNationalites.FirstOrDefault().PK_ID ;
                pDemandeDevis.LeClient.FK_TYPECLIENT = SessionObject.LstTypeClient.FirstOrDefault(t => t.CODE.Trim() == "004").PK_ID;       
                 
           

                //pDemandeDevis.LeClient.TELEPHONEFIXE = string.IsNullOrEmpty(this.txt_Telephone_Fixe.Text) ? null : this.txt_Telephone_Fixe.Text;
                //pDemandeDevis.LeClient.FAX = string.IsNullOrEmpty(this.Txt_NumFax.Text) ? null : this.Txt_NumFax.Text;
                //pDemandeDevis.LeClient.BOITEPOSTAL = string.IsNullOrEmpty(this.Txt_BoitePostale .Text) ? null : this.Txt_BoitePostale .Text;
                //pDemandeDevis.LeClient.TELEPHONE = string.IsNullOrEmpty(this.txt_Telephone.Text) ? null : this.txt_Telephone.Text;
                pDemandeDevis.LeClient.CENTRE = string.IsNullOrEmpty(pDemandeDevis.LaDemande.CENTRE) ? null : pDemandeDevis.LaDemande.CENTRE;
                pDemandeDevis.LeClient.REFCLIENT = string.IsNullOrEmpty(pDemandeDevis.LaDemande.CLIENT) ? null : pDemandeDevis.LaDemande.CLIENT;

                pDemandeDevis.LeClient.ORDRE = "01";
                pDemandeDevis.LeClient.FK_IDCENTRE = pDemandeDevis.LaDemande.FK_IDCENTRE;
                pDemandeDevis.LeClient.DATECREATION = DateTime.Now;
                pDemandeDevis.LeClient.USERCREATION = UserConnecte.matricule;


                pDemandeDevis.LeClient.ADRMAND1 = txtAdresse.Text;
                if (Cbo_Regroupement.SelectedItem != null)
                {
                    var RegroupementClient = Cbo_Regroupement.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsRegCli;
                    if (RegroupementClient != null)
                    {
                        pDemandeDevis.LeClient.REGROUPEMENT = RegroupementClient.CODE;
                        pDemandeDevis.LeClient.FK_IDREGROUPEMENT = RegroupementClient.PK_ID;
                    }
                }
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
                pDemandeDevis.Ag.PORTE = string.IsNullOrEmpty(this.Txt_Porte.Text) ? null : this.Txt_Porte.Text;
                pDemandeDevis.Ag.CENTRE = string.IsNullOrEmpty(pDemandeDevis.LaDemande.CENTRE) ? null : pDemandeDevis.LaDemande.CENTRE;
                pDemandeDevis.Ag.CLIENT = string.IsNullOrEmpty(pDemandeDevis.LaDemande.CLIENT) ? null : pDemandeDevis.LaDemande.CLIENT;
                pDemandeDevis.Ag.FK_IDCENTRE = pDemandeDevis.LaDemande.FK_IDCENTRE;
                pDemandeDevis.Ag.DATECREATION = DateTime.Now;
                pDemandeDevis.Ag.USERCREATION = UserConnecte.matricule;
                #endregion
                //#region Sylla 24/09/2015

                //if (Cbo_Type_Client.SelectedItem != null)
                //{
                //    if (((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE != null)
                //    {
                //        string codetypeclient = ((CsTypeClient)Cbo_Type_Client.SelectedItem).CODE;
                //        switch (codetypeclient.Trim())
                //        {
                //            case "001":
                //                #region Personne Physique
                //                GetPersonnPhyqueData(pDemandeDevis);
                //                #endregion
                //                break;
                //            default:
                //                break;
                //        }
                //    }
                //    else
                //    {
                //        Message.Show("Veuillez renseigné les informations spécifique au typr de client", "Demande");
                //        return null;
                //    }

                //}
                //else
                //{
                //    Message.Show("Veuillez renseigné les informations spécifique au typr de client", "Demande");
                //    return null;
                //}
                //#endregion
                #region Abon
                #endregion
                #region Doc Scanne
                if (pDemandeDevis.ObjetScanne == null) pDemandeDevis.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                pDemandeDevis.ObjetScanne.AddRange(LstPiece);
                #endregion

                pDemandeDevis.LaDemande.ISNEW = true;
                Tdem = pDemandeDevis.LaDemande.TYPEDEMANDE;
                pDemandeDevis.LaDemande.ORDRE = "01";
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

        private void ReloadTypeclientForCateg(CsCategorieClient cat)
        {


            var myls = LstCategorieClient_TypeClient.Where(ct => ct.FK_IDCATEGORIECLIENT == cat.PK_ID);
            if (myls != null)
            {
                var templst = myls.Select(t => t.FK_IDTYPECLIENT);
                var lsttypecient = SessionObject.LstTypeClient.Where(t => templst.Contains(t.PK_ID)).ToList();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (Cbo_Site.SelectedItem != null && Cbo_Centre.SelectedItem != null && Cbo_Produit.SelectedItem != null)
                //{
                //}
                //else
                //{
                //    Message.Show("Veuillez vous assurer que le site le centre et le produit soit selectionné", "Infomation");
                //}
            }
            catch (Exception ex)
            {
                desableProgressBar();
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
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
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

        private void Cbo_TypeDePiece_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Btn_Transmettre_Click(object sender, RoutedEventArgs e)
        {
            this.Btn_Transmettre.IsEnabled = false;
            ValiderInitialisation(laDetailDemande, true );

        }
        private void Txt_CodeRegroupement_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeRegroupement.Text.Length == SessionObject.Enumere.TailleCodeRegroupement)
            {
                CsRegCli leRegroupement = SessionObject.LstCodeRegroupement.FirstOrDefault(t => t.CODE == this.Txt_CodeRegroupement.Text);
                if (leRegroupement != null)
                {
                    if ((this.Cbo_Regroupement.SelectedItem != null && (CsRegCli)this.Cbo_Regroupement.SelectedItem != leRegroupement) || this.Cbo_Regroupement.SelectedItem == null)
                        this.Cbo_Regroupement.SelectedItem = leRegroupement;
                }
                else
                {
                    Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                    return;
                }
            }

        }

    }
}


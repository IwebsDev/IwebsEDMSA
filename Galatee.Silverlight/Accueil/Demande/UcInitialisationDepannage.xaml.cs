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
using Galatee.Silverlight.Tarification.Helper;

namespace Galatee.Silverlight.Accueil
{
    public partial class UcInitialisationDepannage : ChildWindow
    {
        private UcImageScanne formScanne = null;
        private Object ModeExecution = null;
        private List<CsTournee> _listeDesTourneeExistant = null;
        private List<CsCommune> _listeDesCommuneExistant = null;
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
        //Galatee.Silverlight.ServiceFraude.CsClient Client = new Galatee.Silverlight.ServiceFraude.CsClient();
        Galatee.Silverlight.ServiceAccueil.CsClient Client = new Galatee.Silverlight.ServiceAccueil.CsClient();

        public string nom;
        public string prenom;
        public DateTime? datefinvalidité = new DateTime();
        public DateTime? datenaissance = new DateTime();
        public string numeropiece;
        public int? typepiece;

        public UcInitialisationDepannage()
        {
            InitializeComponent();
        }
    
        private List<ObjDOCUMENTSCANNE> ObjetScanne = new List<ObjDOCUMENTSCANNE>();
        private List<ObjPIECEIDENTITE> ListeTYpePiece = new List<ObjPIECEIDENTITE>();
        private List<CsStatutJuridique> ListStatuJuridique = new List<CsStatutJuridique>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        public List<CsCATEGORIECLIENT_TYPECLIENT> LstCategorieClient_TypeClient = new List<CsCATEGORIECLIENT_TYPECLIENT>();
        public List<CsNATURECLIENT_TYPECLIENT> LstNatureClient_TypeClient = new List<CsNATURECLIENT_TYPECLIENT>();
        public List<CsUSAGE_NATURECLIENT> LstUsage_NatureClient = new List<CsUSAGE_NATURECLIENT>();
        public List<CsCATEGORIECLIENT_USAGE> LstCategorieClient_Usage = new List<CsCATEGORIECLIENT_USAGE>();
        public List<CsProprietaire> Lsttypeprop = new List<CsProprietaire>();

        List<CsTypePanne> lstTypeDetailPanne = new List<CsTypePanne>();
        List<CsGroupeDepannageCommune> lstGroupeDepannageCommune = new List<CsGroupeDepannageCommune>();

      
        public UcInitialisationDepannage(string Tdem)
        {
            try
            {
                InitializeComponent();
                this.Tdem = Tdem;
                ModeExecution = SessionObject.ExecMode.Creation;

                Txt_Porte.MaxLength = 5;
                RemplirListeDesTypeDemandeExistant();
                RemplirSecteur();
                RemplirCommune();
                RemplirListeDesQuartierExistant();
                RemplirListeDesRuesExistant();
                RemplirListeDesDiametresExistant();
                RemplirGroupeValidationDepannage();
                ChargerListDesSite();
                RemplirTypePanne();
                RemplirModeCommunication();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        public UcInitialisationDepannage(string Tdem,string Init )
        {
            try
            {
                InitializeComponent();
                this.Tdem = Tdem;
                ModeExecution = SessionObject.ExecMode.Creation;

                Txt_Porte.MaxLength = 5;
                RemplirListeDesTypeDemandeExistant();
                RemplirSecteur();
                RemplirCommune();
                RemplirListeDesQuartierExistant();
                RemplirListeDesRuesExistant();
                RemplirListeDesDiametresExistant();
                RemplirGroupeValidationDepannage();
                ChargerListDesSite();
                RemplirTypePanne();
                RemplirTypeDetailPanne();
                RemplirModeCommunication();
                RemplirGroupeCommune();

                SessionObject.IsChargerDashbord = false;
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
  
        private CsDemande GetDemandeDevisFromScreen(CsDemande pDemandeDevis, bool isTransmettre)
        {
            try
            {
                if (pDemandeDevis == null)
                {
                    pDemandeDevis = new CsDemande();
                    pDemandeDevis.LaDemande = new CsDemandeBase();
                    pDemandeDevis.Depannage  = new CsDepannage ();
                    pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                    pDemandeDevis.LaDemande.USERCREATION  = UserConnecte.matricule ;
                }

                pDemandeDevis.Depannage.FK_IDCENTRE = ((CsCommune)Cbo_Commune.SelectedItem).FK_IDCENTRE;
                pDemandeDevis.Depannage.FK_IDCOMMUNE = ((CsCommune)Cbo_Commune.SelectedItem).PK_ID;
                pDemandeDevis.Depannage.FK_IDQUARTIER = ((CsQuartier)Cbo_Quartier.SelectedItem).PK_ID;


                #region Sylla 17/04/2017
                pDemandeDevis.Depannage.PORTE = Txt_Porte.Text;
                pDemandeDevis.Depannage.TELEPHONE = txt_Telephonne.Text;
                pDemandeDevis.Depannage.NOM_DECLARANT = txt_nom_declarant.Text;
                pDemandeDevis.Depannage.ISEDM = Chk_EstClient.IsChecked;
                pDemandeDevis.Depannage.ISPERSONNEEXTERIEUR = !Chk_EstClient.IsChecked;

                #endregion

                pDemandeDevis.Depannage.FK_IDMODERECEUIL = ((CsModeCommunication)Cbo_ModeReceuil.SelectedItem).ID;
                //pDemandeDevis.Depannage.FK_IDMODERECEUIL = int.Parse(((CsModeReception)Cbo_ModeReceuil.SelectedItem).pk_id.ToString());
                pDemandeDevis.Depannage.DESCRIPTIONPANNE = this.Txt_Commentaire.Text ;


                if (this.Cbo_TypeDetaildePanne.SelectedItem != null)
                {
                    pDemandeDevis.Depannage.FK_IDDETAILPANNE = ((CsTypePanne)Cbo_TypeDetaildePanne.SelectedItem).ID;
                    pDemandeDevis.Depannage.FK_IDTYPEDEPANNE_TRAITE = ((CsTypePanne)Cbo_TypeDetaildePanne.SelectedItem).ID;
                }
                CsTdem TypeDemande = new CsTdem();
                CsTypePanne leTypePanne = new CsTypePanne();
                if (this.Cbo_TypedePanne.SelectedItem != null)
                {
                    pDemandeDevis.Depannage.FK_IDTYPEDEPANNE = ((CsTypePanne)Cbo_TypedePanne.SelectedItem).ID;

                    leTypePanne = (CsTypePanne)Cbo_TypedePanne.SelectedItem;
                    if (leTypePanne.CODE == "EP") TypeDemande = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DepannageEp);
                    if (leTypePanne.CODE == "CL") TypeDemande = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DepannageClient);
                    if (leTypePanne.CODE == "RE") TypeDemande = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DepannageMaintenance);
                    if (leTypePanne.CODE == "PR") TypeDemande = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DepannagePrepayer);
                    if (leTypePanne.CODE == "MT") TypeDemande = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DepannageMT);
                    pDemandeDevis.LaDemande.TYPEDEMANDE = TypeDemande.CODE;
                    pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE  = TypeDemande.PK_ID ;

                }

                if (this.Cbo_Rue.SelectedItem != null)
                    pDemandeDevis.Depannage.FK_IDRUE = ((CsRues)Cbo_Rue.SelectedItem).PK_ID;

                if (this.Cbo_Secteur.SelectedItem != null)
                    pDemandeDevis.Depannage.FK_IDSECTEUR = ((CsSecteur)Cbo_Secteur.SelectedItem).PK_ID;

                

                pDemandeDevis.LaDemande.ISNEW = true;
                Tdem = pDemandeDevis.LaDemande.TYPEDEMANDE;
                //pDemandeDevis.LaDemande.TYPEDEMANDE =((CsTdem) txt_tdem.Tag).CODE ;
                pDemandeDevis.LaDemande.CENTRE = ((CsCommune)Cbo_Commune.SelectedItem).CENTRE;
                pDemandeDevis.LaDemande.FK_IDCENTRE = ((CsCommune)Cbo_Commune.SelectedItem).FK_IDCENTRE;
                //pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = ((CsTdem)txt_tdem.Tag).PK_ID ;

                pDemandeDevis.LaDemande.ORDRE = "01";

                if (Client != null)
                {
                    pDemandeDevis.LaDemande.CENTRE = Client.CENTRE;
                    pDemandeDevis.LaDemande.FK_IDCENTRE = Client.FK_IDCENTRE.Value;
                    pDemandeDevis.LaDemande.ORDRE = Client.ORDRE;
                    pDemandeDevis.LaDemande.CLIENT = Client.REFCLIENT;

                    /*pDemandeDevis.LeClient = new CsClient();
                    pDemandeDevis.LeClient.CENTRE = Client.CENTRE;
                    pDemandeDevis.LeClient.FK_IDCENTRE = Client.FK_IDCENTRE.Value;
                    pDemandeDevis.LeClient.ORDRE = Client.ORDRE;
                    pDemandeDevis.LeClient.REFCLIENT = Client.REFCLIENT;*/

                }

                return pDemandeDevis;
            }
            catch (Exception ex)
            {
                this.Btn_Transmettre.IsEnabled = true;
                throw ex;
            }
        }

        private void ValiderInitialisation(CsDemande demandedevis,Guid idgroupValidation, bool IsTransmetre)
        {

            try
            {

                // Get Devis informations from screen$
                if (this.Cbo_Commune.SelectedItem == null)
                {
                    Message.ShowInformation("Sélectionnez la commune", "Demande");
                    this.Btn_Transmettre.IsEnabled = true;
                    return;
                }

                if (this.Cbo_Quartier.SelectedItem == null)
                {
                    Message.ShowInformation("Sélectionnez le quartier", "Demande");
                    this.Btn_Transmettre.IsEnabled = true;
                    return;
                }

                if (demandedevis != null)
                    demandedevis = GetDemandeDevisFromScreen(demandedevis, false);
                else
                    demandedevis = GetDemandeDevisFromScreen(demandedevis, false);

                // Get DemandeDevis informations from screen
                if (demandedevis != null)
                {
                    demandedevis.LaDemande.ETAPEDEMANDE = (int)DataReferenceManager.EtapeDevis.Accueil;
                    //demandedevis.LaDemande.TYPEDEMANDE = SessionObject.LstTypeDemande.First(a => a.CODE == SessionObject.Enumere.DepannageClient).CODE;
                    //demandedevis.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.First(a => a.CODE == SessionObject.Enumere.DepannageClient).PK_ID;
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
                            this.Btn_Transmettre.IsEnabled = true;

                            return;
                        }
                        string numedemande = string.Empty;
                        string Client = string.Empty;
                        if (IsTransmetre)
                        {
                            string Retour = b.Result;
                            string[] coupe = Retour.Split('.');
                            Shared.ClasseMEthodeGenerique.InitWOrkflowToGroupValidation(coupe[0], demandedevis.LaDemande.FK_IDCENTRE, coupe[1],idgroupValidation, demandedevis.LaDemande.FK_IDTYPEDEMANDE);
                            numedemande = coupe[1];
                            Client = coupe[2];
                        }
                        List<CsDemandeBase> leDemandeAEditer = new List<CsDemandeBase>();
                        demandedevis.LaDemande.LIBELLETYPEDEMANDE = txt_tdem.Text;
                        demandedevis.LaDemande.NUMDEM = numedemande;
                        demandedevis.LaDemande.CLIENT = Client;
                        demandedevis.LaDemande.LIBELLECOMMUNE = ((CsCommune)this.Cbo_Commune.SelectedItem).LIBELLE;
                        demandedevis.LaDemande.LIBELLEQUARTIER = this.Cbo_Quartier.SelectedItem != null ? ((CsQuartier)this.Cbo_Quartier.SelectedItem).LIBELLE : string.Empty;
                        demandedevis.LaDemande.LIBELLE = "Imprimé le " + DateTime.Now + " sur le poste " + SessionObject.LePosteCourant.NOMPOSTE + " par " + UserConnecte.nomUtilisateur + "(" + UserConnecte.matricule + ") du centre " + UserConnecte.LibelleCentre;

                        leDemandeAEditer.Add(demandedevis.LaDemande);
                        Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(leDemandeAEditer, null, SessionObject.CheminImpression, "AccuseRecption", "Accueil", true);
                        this.DialogResult = true;

                    };




                    client.ValiderDemandeInitailisationAsync(demandedevis);
                }
            }
            catch (Exception ex)
            {
                this.Btn_Transmettre.IsEnabled = true;
                Message.ShowError("Une erreur s'est produite à la validation ", "ValiderDemandeInitailisation");
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
        List<Galatee.Silverlight.ServiceParametrage.CsGroupeValidation> lesGroupeValidation = new List<ServiceParametrage.CsGroupeValidation>();
        private void RemplirGroupeValidationDepannage()
        {
            try
            {

                Galatee.Silverlight.ServiceParametrage.ParametrageClient service = new Galatee.Silverlight.ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                service.SelectAllGroupeValidationSpecifiqueCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    lesGroupeValidation = args.Result;
                };
                service.SelectAllGroupeValidationSpecifiqueAsync(2);
                service.CloseAsync();
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
        private void RemplirModeCommunication()
        {
            try
            {
                List<CsModeCommunication> lstModeCommunication = new List<CsModeCommunication>();
                //List<CsModeReception> lstModeCommunication = new List<CsModeReception>();
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneModeCommunicationAsync();
                service.RetourneModeCommunicationCompleted += (s, args) =>
                //service.SelectAllModeReceptionAsync();
                //service.SelectAllModeReceptionCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    lstModeCommunication = args.Result;

                    Cbo_ModeReceuil.ItemsSource = lstModeCommunication;
                    Cbo_ModeReceuil.IsEnabled = true;
                    Cbo_ModeReceuil.DisplayMemberPath = "LIBELLE";

                   /* Cbo_ModeReceuil.IsEnabled = true;
                    Cbo_ModeReceuil.ItemsSource = null;
                    Cbo_ModeReceuil.DisplayMemberPath = "Libelle";
                    Cbo_ModeReceuil.SelectedValuePath = "PK_ID";
                    Cbo_ModeReceuil.ItemsSource = args.Result;
                    */

                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirGroupeCommune()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneGroupeDepannageCommuneAsync();
                service.RetourneGroupeDepannageCommuneCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    lstGroupeDepannageCommune = args.Result;
                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirTypeDetailPanne()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneDetailTypePanneAsync();
                service.RetourneDetailTypePanneCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    lstTypeDetailPanne = args.Result;

                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirTypePanne()
        {
            try
            {
                List<CsTypePanne> lstTypePanne = new List<CsTypePanne>();
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneTypePanneAsync();
                service.RetourneTypePanneCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    lstTypePanne = args.Result;

                    Cbo_TypedePanne.ItemsSource = lstTypePanne.OrderBy(t=>t.LIBELLE).ToList();
                    Cbo_TypedePanne.IsEnabled = true;
                    Cbo_TypedePanne.DisplayMemberPath = "LIBELLE";

                };
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
                        Cbo_Secteur.ItemsSource = SessionObject.LstSecteur.OrderBy(t => t.LIBELLE).ToList();
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
                service.ChargerCommuneCompleted  += (s, args) =>
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
                    var lstCentreDistnct = lstCommune.Select(t => new {LIBELLE= t.LIBELLE.ToUpper(),t.CODE   }).Distinct().ToList();
                    foreach (var item in lstCentreDistnct)
                        lstCentreDist.Add(lstCommune.FirstOrDefault(t=>t.CODE == item.CODE && t.LIBELLE == item.LIBELLE ));

                return lstCentreDist;
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
                    _listeDesQuartierExistant = SessionObject.LstQuartier.OrderBy(t => t.LIBELLE).ToList();
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

            try
            {
                QuartierParDefaut = _listeDesQuartierExistant.Where(q => q.FK_IDCOMMUNE == pCommuneId).OrderBy(t => t.LIBELLE).ToList();
                if (QuartierParDefaut != null && QuartierParDefaut.Count > 0)
                    ListeQuartierFiltres.AddRange(QuartierParDefaut);
                ListeQuartierFiltres.AddRange(_listeDesQuartierExistant.Where(q => q.FK_IDCOMMUNE == pCommuneId && q.CODE != DataReferenceManager.QuartierInconnu).OrderBy(t => t.LIBELLE).ToList());

                if (ListeQuartierFiltres.Count > 0)
                    //foreach (var item in ListeQuartierFiltres)
                    //{
                    //    Cbo_Quartier.Items.Add(item);
                    //}
                    Cbo_Quartier.ItemsSource = null;
                Cbo_Quartier.ItemsSource = ListeQuartierFiltres.OrderBy(t => t.LIBELLE).ToList();
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
            try
            {
                RueParDefaut = _listeDesRuesExistant.Where(q => q.CODE == DataReferenceManager.RueInconnue).ToList();
                if (RueParDefaut != null && RueParDefaut.Count > 0)
                    ListeRuesFiltrees.AddRange(RueParDefaut);
                ListeRuesFiltrees.AddRange(_listeDesRuesExistant.Where(q => q.PK_ID == pIdCommune && q.CODE != DataReferenceManager.RueInconnue).ToList());

                Cbo_Rue.ItemsSource = null;
                Cbo_Rue.ItemsSource = ListeRuesFiltrees.OrderBy(t => t.LIBELLE).ToList();
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


        private void Cbo_TypeDevis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
                        CsCommune communeSelect = SessionObject.LstCommune.FirstOrDefault(t => t.CODE == commune.CODE && t.LIBELLE == commune.LIBELLE);
                        RemplirQuartier(communeSelect.PK_ID);
                        RemplirRues(communeSelect.PK_ID);
                    }
                }
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
        private void Cbo_Secteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Secteur.SelectedItem != null)
                {
                    var Secteur = Cbo_Secteur.SelectedItem as ServiceAccueil.CsSecteur;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void Txt_PrenomClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActiverEnregistrerOuTransmettre();
        }

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
        private int PK_Id_Tdem = 0;

        private void txt_Telephone_TextChanged(object sender, TextChangedEventArgs e)
        {
 
            ActiverEnregistrerOuTransmettre();
        }
        Guid GroupeDeValidation = new Guid();
        private void Btn_Transmettre_Click(object sender, RoutedEventArgs e)
        {
            this.Btn_Transmettre.IsEnabled = false;
            GroupeDeValidation = (Guid)this.txt_GroupeValidation.Tag;
            ValiderInitialisation(null, GroupeDeValidation, true);
        }

        private void txt_ModeRecueil_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_GroupeValidation_Click_1(object sender, RoutedEventArgs e)
        {
            if (Cbo_Commune.SelectedItem == null)
            {
                Message.ShowInformation("Veuillez sélectionner la commune.", "Information");
                return;
            }

            if (lesGroupeValidation != null && lesGroupeValidation.Count != 0 && Cbo_Commune.SelectedItem != null )
            {
             List<CsGroupeDepannageCommune> lstGr=lstGroupeDepannageCommune.Where(t => t.CODECOMMUNE == ((CsCommune)Cbo_Commune.SelectedItem).CODE ).ToList();

                List<object> _LstObj = new List<object>();
                _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(lesGroupeValidation.Where(y=>lstGr.Select(o=>o.FK_IDGROUPEVALIDATION).Contains(y.PK_ID)).ToList());
                Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                _LstColonneAffich.Add("GROUPENAME", "GROUPE DESTINATAIRE");

                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Groupe");
                ctrl.Closed += new EventHandler(galatee_OkClickedBatch);
                ctrl.Show();
            }
        }

        void galatee_OkClickedBatch(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceParametrage.CsGroupeValidation _LesGroupeValiadation = ctrs.MyObject as Galatee.Silverlight.ServiceParametrage.CsGroupeValidation;
                this.txt_GroupeValidation.Text = string.IsNullOrEmpty(_LesGroupeValiadation.GROUPENAME) ? string.Empty : _LesGroupeValiadation.GROUPENAME;
                this.txt_GroupeValidation.Tag = _LesGroupeValiadation.PK_ID;
            }
        }

        private void Cbo_TypedePanne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Cbo_TypedePanne.SelectedItem != null)
            {
                CsTypePanne leTypePanneSelect = (CsTypePanne)this.Cbo_TypedePanne.SelectedItem;
                Cbo_TypeDetaildePanne.ItemsSource = lstTypeDetailPanne.Where(u => u.ID_TYPE_RECLAMATION == leTypePanneSelect.ID).OrderBy(t => t.LIBELLE).ToList();
                Cbo_TypeDetaildePanne.IsEnabled = true;
                Cbo_TypeDetaildePanne.DisplayMemberPath = "LIBELLE";
                Cbo_TypeDetaildePanne.SelectedItem = null;
            }
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            lbl_ref_client.Visibility = Visibility.Visible;
            lbl_nom_declarant.Visibility = Visibility.Collapsed;
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            lbl_ref_client.Visibility = Visibility.Collapsed;
            lbl_nom_declarant.Visibility = Visibility.Visible;
        }

        private void Chk_EstClient_Checked(object sender, RoutedEventArgs e)
        {
           

            if (Chk_EstClient.IsChecked == true)
            {
                lbl_ref_client.Visibility = Visibility.Visible;
                lbl_nom_declarant.Visibility = Visibility.Collapsed;
                Btn_RechercherClient.IsEnabled = true;

            }
            else
            {
                lbl_ref_client.Visibility = Visibility.Collapsed;
                lbl_nom_declarant.Visibility = Visibility.Visible;
                Btn_RechercherClient.IsEnabled = false;
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Galatee.Silverlight.Fraude.UcRechercheClient Newfrm = new Galatee.Silverlight.Fraude.UcRechercheClient(); ;
                Newfrm.CallBack += Newfrm_CallBack;
                Newfrm.Show();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void Newfrm_CallBack(object sender, CustumEventArgs e)
        {
            //Client = e.Data as Galatee.Silverlight.ServiceFraude.CsClient;
            Client = e.Data as Galatee.Silverlight.ServiceAccueil.CsClient;
            txt_nom_declarant.Text = Client.NOMABON;
            Txt_Porte.Text = Client.PORTE == null ? "" : Client.PORTE;

            txt_Telephonne.Text = Client.TELEPHONE == null ? "" : Client.TELEPHONE;
            if (Client.FK_IDCOMMUNE > 0)
            {
                CsCommune com = SessionObject.LstCommune.FirstOrDefault(t => t.PK_ID == Client.FK_IDCOMMUNE);
                Cbo_Commune.ItemsSource = _listeDesCommuneExistant;
                Cbo_Commune.SelectedItem = com;
                RemplirQuartier(com.PK_ID);
                RemplirRues(com.PK_ID);

            } 
            if (Client.FK_IDQUARTIER > 0)
            {
                CsQuartier com = SessionObject.LstQuartier.FirstOrDefault(t => t.PK_ID == Client.FK_IDQUARTIER);
                Cbo_Quartier.ItemsSource = _listeDesQuartierExistant;
                Cbo_Quartier.SelectedItem = com;
            } 
            if (Client.FK_IDSECTEUR > 0)
            {
                CsSecteur com = SessionObject.LstSecteur.FirstOrDefault(t => t.PK_ID == Client.FK_IDSECTEUR);
                Cbo_Secteur.SelectedItem = com;
            } 

            if (Client.FK_IDRUE > 0)
            {
                CsRues com = SessionObject.LstRues.FirstOrDefault(t => t.PK_ID == Client.FK_IDRUE);
                Cbo_Rue.SelectedItem = com;
            } 



        }
    
    }
}


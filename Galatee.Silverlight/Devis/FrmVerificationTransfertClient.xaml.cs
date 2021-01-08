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

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmVerificationTransfertClient : ChildWindow
    {
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        CsDemandeBase laDemandeSelect = null;
        bool isPreuveSelectionnee = false;
        private UcImageScanne formScanne = null;
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        CsDemande laDetailDemande = null;
        Galatee.Silverlight.Shared.UcFichierJoint ctrl = null;

        public FrmVerificationTransfertClient()
        {
            InitializeComponent();
            ChargerListDesSite();
            List<string> ListOperation = new List<string>();
            ListOperation = SessionObject.TypeOperationClasseur().ToList();
            tab4_cbo_Operation.ItemsSource = null;
            tab4_cbo_Operation.ItemsSource = ListOperation;
            if (ListOperation != null && ListOperation.Count != 0)
                tab4_cbo_Operation.SelectedItem = ListOperation[0];
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            RemplirCodeRegroupement();
        }
        public FrmVerificationTransfertClient(int IdDemande)
        {
            InitializeComponent();
            ChargerListDesSite();
            List<string> ListOperation = new List<string>();
            ListOperation = SessionObject.TypeOperationClasseur().ToList();
            tab4_cbo_Operation.ItemsSource = null;
            tab4_cbo_Operation.ItemsSource = ListOperation;
            if (ListOperation != null && ListOperation.Count != 0)
                tab4_cbo_Operation.SelectedItem = ListOperation[0];
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            tabPieceJointe.Visibility = System.Windows.Visibility.Collapsed;
            RemplirCodeRegroupement();
            ChargeDetailDEvis(IdDemande);
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            prgBar.Visibility = System.Windows.Visibility.Visible ;
            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeCompleted += (ssender, args) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
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
                    this.txtCentre_Origine.Text = laDetailDemande.Transfert.LIBELLECENTREORIGINE;
                    this.txtSite_Origine.Text = laDetailDemande.Transfert.LIBELLESITEORIGINE;
                    this.txtCentreTransfert.Text = laDetailDemande.Transfert.LIBELLECENTRETRANSFERT;
                    this.txtSiteTransfert.Text = laDetailDemande.Transfert.LIBELLESITETRANSFERT;
                    this.Txt_CodeRegroupement.Text = string.IsNullOrEmpty(laDetailDemande.Transfert.LIBELLEREGROUPEMENT) ? string.Empty : laDetailDemande.Transfert.LIBELLEREGROUPEMENT;
                    this.txtNumeroDemande.Text = laDetailDemande.LaDemande.NUMDEM;
                    this.txtProduit.Text = laDetailDemande.LaDemande.LIBELLEPRODUIT;
                    this.txt_Ref_Branchement.Text = laDetailDemande.LaDemande.CLIENT;
                    this.txt_ordre.Text = laDetailDemande.LaDemande.ORDRE;
                    laDetailDemande.LaDemande.FK_IDETAPEENCOURE = laDetailDemande.InfoDemande.FK_IDETAPEACTUELLE;
                    CsClientRechercher leclientRech = new CsClientRechercher()
                    {
                        CENTRE = laDetailDemande.LeClient.CENTRE,
                        CLIENT = laDetailDemande.LeClient.REFCLIENT,
                        ORDRE = laDetailDemande.LeClient.ORDRE,
                        FK_IDCENTRE = laDetailDemande.LeClient.FK_IDCENTRE.Value,
                    };
                    RetourneLeCompteClient(leclientRech);

                    RemplireOngletClient(laDetailDemande.LeClient);
                    RemplirOngletAbonnement(laDetailDemande.Abonne);
                    RemplireOngletCanalisation(laDetailDemande.LstCanalistion);
                    RemplireBranchementParProduit(laDetailDemande.Branchement);
                    RemplireOngletAdresse(laDetailDemande.Ag);

                    if (laDetailDemande.ObjetScanne != null && laDetailDemande.ObjetScanne.Count != 0) this.tabPieceJointe.Visibility = System.Windows.Visibility.Visible;
                    RenseignerInformationsDocumentScanne();
                }
            };
            client.ChargerDetailDemandeAsync (IdDemandeDevis,string.Empty );
        }
        private void RenseignerInformationsDocumentScanne()
        {
            try
            {
                #region DocumentScanne
                ctrl = new Galatee.Silverlight.Shared.UcFichierJoint(laDetailDemande.ObjetScanne, true);
                Vwb.Stretch = Stretch.None;
                Vwb.Child = ctrl;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        private List<CsCentre> _listeDesCentreExistant = null;

        void ChargerListDesSite()
        {
            try
            {

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    SessionObject.ModuleEnCours = "Accueil";
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
                        SessionObject.ModuleEnCours = "Accueil";
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

        CsCompteClient leClasseurClient = null;
        List<CsLclient> LstReglementClient = null ;
        List<CsLclient> LstFactureClient = null;
        
        private void RetourneLeCompteClient(CsClientRechercher leClient)
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneLeCompteClientCompleted += (s, args) =>
                {
                    leClasseurClient = new CsCompteClient();
                    if (args != null && args.Cancelled)
                        return;

                    leClasseurClient = args.Result;
                    if (leClasseurClient != null)
                    {
                        decimal _totalDebit = 0;
                        decimal _totalCredit = 0;

                        _totalDebit = decimal.Parse(leClasseurClient.ToutLClient.Where(t => t.DC == SessionObject.Enumere.Debit).Sum(p => p.MONTANT).ToString());
                        this.tab4_txt_TotalDebit.Text = _totalDebit.ToString(SessionObject.FormatMontant);

                        _totalCredit = leClasseurClient.ToutLClient.Where(t => t.DC == SessionObject.Enumere.Credit).Sum(p => decimal.Parse(p.MONTANT.ToString()));
                        this.tab4_txt_TotalCredit.Text = _totalCredit.ToString(SessionObject.FormatMontant);
                        tab4_txt_balance.Text = (_totalDebit - _totalCredit).ToString(SessionObject.FormatMontant); ;

                        LstReglementClient = new List<CsLclient>();
                        LstFactureClient = new List<CsLclient>();
                        if (leClasseurClient.LstFacture != null)
                            LstFactureClient = leClasseurClient.LstFacture;
                        if (leClasseurClient.LstReglement != null)
                            LstReglementClient = leClasseurClient.LstReglement;
                        RemplirTypeAction(0);
                    }
                };
                service.RetourneLeCompteClientAsync(leClient.FK_IDCENTRE, leClient.CENTRE, leClient.CLIENT, leClient.ORDRE);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }
        private void RemplirTypeAction(int Index)
        {
            try
            {
                int caseSwitch = Index;
                switch (caseSwitch)
                {
                    case 0:
                        {
                            this.tab4_dataGrid1.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid2.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid3.Visibility = System.Windows.Visibility.Visible;
                            if (leClasseurClient != null)
                            {
                                //_LeClasseur.LeCompteClient.ToutLClient = new List<CsLclient>();

                                tab4_dataGrid3.ItemsSource = null;
                                List<CsLclient> _ToutLeCompteClient = ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(leClasseurClient.ToutLClient.OrderBy(t => t.REFEM).ToList());
                                List<CsLclient> _ToutLeCompteClientReg = leClasseurClient.ToutLClient.Where(p => p.DC == "C").ToList();
                                RemplireOngletToutLeCompte(_ToutLeCompteClient.OrderBy(t => t.REFEM).ToList());
                            }
                        }
                        break;
                    case 1:
                        {
                            this.tab4_dataGrid1.Visibility = System.Windows.Visibility.Visible;
                            this.tab4_dataGrid2.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid3.Visibility = System.Windows.Visibility.Collapsed;
                            tab4_dataGrid1.ItemsSource = null;
                            RemplireOngletReglement(LstReglementClient.OrderBy(t => t.REFEM).ToList());
                        }
                        break;
                    case 2:
                        {
                            this.tab4_dataGrid1.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid2.Visibility = System.Windows.Visibility.Visible;
                            this.tab4_dataGrid3.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGrid2.ItemsSource = null;
                            RemplireOngletFacture(LstFactureClient.OrderBy(t => t.REFEM).ToList());
                        }
                        break;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RemplireOngletToutLeCompte(List<CsLclient> _LeCompteClient)
        {
            try
            {
                tab4_dataGrid3.ItemsSource = null;
                tab4_dataGrid3.ItemsSource = FormateListe(_LeCompteClient);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private List<CsLclient> FormateListe(List<CsLclient> _LeCompteClient)
        {
            List<CsLclient> _LstFactureFinal = new List<CsLclient>();
            if (_LeCompteClient != null && ((_LeCompteClient != null && _LeCompteClient.Count != 0)))
            {
                List<CsLclient> _LstFacture = _LeCompteClient.Where(p => p.DC == SessionObject.Enumere.Debit).ToList();
                List<CsLclient> _LstEncaissement = _LeCompteClient.Where(p => p.DC == SessionObject.Enumere.Credit).ToList();
                if (_LstFacture != null && _LstFacture.Count != 0)
                    foreach (var item in _LstFacture)
                    {
                        _LstFactureFinal.Add(item);
                        List<CsLclient> lstFacture = _LstEncaissement.Where(p => p.REFEM == item.REFEM && p.NDOC == item.NDOC).ToList();
                        if (lstFacture != null && lstFacture.Count != 0)
                            _LstFactureFinal.AddRange(TransLClient(lstFacture));
                    }
                else
                    _LstFactureFinal.AddRange(_LstEncaissement);

                //if (_LstEncaissement != null && _LstEncaissement.Count != 0)
                // foreach (CsLclient item in _LstEncaissement)
                // {
                //     if (_LstFactureFinal.FirstOrDefault(t => t.REFEM == item.REFEM && t.NDOC == item.NDOC) == null)
                //         _LstFactureFinal.Add(item);
                // }
            }
            return _LstFactureFinal;
        }
        private List<CsLclient> TransLClient(List<CsLclient> _LeTranscaisse)
        {
            List<CsLclient> _LeReglt = new List<CsLclient>();
            foreach (var item in _LeTranscaisse)
            {
                item.REFEM = string.Empty;
                item.NDOC = string.Empty;
                item.ACQUIT = string.Empty;
                _LeReglt.Add(item);
            }
            return _LeReglt;
        }

        private void RemplireOngletFacture(List<CsLclient> _LesFacture)
        {
            try
            {
                tab4_dataGrid2.ItemsSource = null;
                tab4_dataGrid2.ItemsSource = _LesFacture.OrderByDescending(p => p.DENR);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void RemplireOngletReglement(List<CsLclient> _LesReglement)
        {
            try
            {

                var reglemntParModereg = (from p in _LesReglement
                                          group new { p } by new { p.ACQUIT, p.DTRANS, p.NOMCAISSIERE } into pResult
                                          select new
                                          {
                                              pResult.Key.ACQUIT,
                                              pResult.Key.NOMCAISSIERE,
                                              pResult.Key.DTRANS,
                                              MONTANT = (decimal?)pResult.Where(t => t.p.ACQUIT == pResult.Key.ACQUIT).Sum(o => o.p.MONTANT)
                                          });
                tab4_dataGrid1.ItemsSource = null;
                tab4_dataGrid1.ItemsSource = reglemntParModereg.OrderByDescending(p => p.DTRANS);
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

        private void RemplireBranchementParProduit(CsBrt _LeBrtSelectionne)
        {
            try
            {
                this.Txt_Distance.Text = laDetailDemande.Branchement.LONGBRT == null ? string.Empty : laDetailDemande.Branchement.LONGBRT.ToString();
                this.Txt_PuissanceInstalle .Text = laDetailDemande.Branchement.PUISSANCEINSTALLEE == null ? string.Empty : laDetailDemande.Branchement.PUISSANCEINSTALLEE.Value.ToString(SessionObject.FormatMontant);
                this.TxtLongitude.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LONGITUDE) ? string.Empty : laDetailDemande.Branchement.LONGITUDE;
                this.TxtLatitude.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LATITUDE) ? string.Empty : laDetailDemande.Branchement.LATITUDE;

                Txt_LibelleTypeBrt.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLETYPEBRANCHEMENT) ? string.Empty : laDetailDemande.Branchement.LIBELLETYPEBRANCHEMENT;
                Txt_LibellePosteSource.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEPOSTESOURCE) ? string.Empty : laDetailDemande.Branchement.LIBELLEPOSTESOURCE;
                Txt_LibelleDepartHTA.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEDEPARTHTA) ? string.Empty : laDetailDemande.Branchement.LIBELLEDEPARTHTA;
                Txt_LibelleQuartierPoste.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEQUARTIER) ? string.Empty : laDetailDemande.Branchement.LIBELLEQUARTIER;
                Txt_LibellePosteTransformateur.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLETRANSFORMATEUR) ? string.Empty : laDetailDemande.Branchement.LIBELLETRANSFORMATEUR;
                Txt_LibelleDepartBt.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.DEPARTBT) ? string.Empty : laDetailDemande.Branchement.DEPARTBT;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void RemplireCannalisationProduit(CsCanalisation _LaCannalisationSelect)
        {
            try
            {
                this.tab5_txt_NumCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.NUMERO) ? string.Empty : _LaCannalisationSelect.NUMERO;
                this.tab5_txt_AnnefabricCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.ANNEEFAB) ? string.Empty : _LaCannalisationSelect.ANNEEFAB;
                this.tab5_txt_LibelleTypeCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLETYPECOMPTEUR) ? string.Empty : _LaCannalisationSelect.LIBELLETYPECOMPTEUR;
                this.tab5_txt_NumCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.NUMERO) ? string.Empty : _LaCannalisationSelect.NUMERO;
                this.tab5_txt_MarqueCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLEMARQUE) ? string.Empty : _LaCannalisationSelect.LIBELLEMARQUE;


                if (_LaCannalisationSelect.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    this.tab5_txt_LibelleDiametreCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLEREGLAGECOMPTEUR) ? string.Empty : _LaCannalisationSelect.LIBELLEREGLAGECOMPTEUR;
                else
                    this.tab5_txt_LibelleDiametreCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLETYPECOMPTAGE) ? string.Empty : _LaCannalisationSelect.LIBELLETYPECOMPTAGE;

                this.tab5_txt_CoefDeMultiplication.Text = _LaCannalisationSelect.COEFLECT.ToString();
                if (_LaCannalisationSelect.COEFLECT == 0)
                    this.tab5_Chk_CoefMultiplication.IsChecked = false;
                else
                    tab5_Chk_CoefMultiplication.IsChecked = true;

                if (_LaCannalisationSelect.POSE != null)
                    this.tab5_txt_DateMiseEnService.Text = Convert.ToDateTime(_LaCannalisationSelect.POSE).ToShortDateString();

                if (_LaCannalisationSelect.DEPOSE != null)
                    this.tab5_txt_DateFinServce.Text = Convert.ToDateTime(_LaCannalisationSelect.DEPOSE).ToShortDateString();


                this.tab5_txt_LibelleDigit.Text = _LaCannalisationSelect.CADRAN.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void RemplireOngletCanalisation(List<CsCanalisation> _LstCannalisation)
        {
            try
            {

                if (_LstCannalisation != null && _LstCannalisation.Count != 0)
                {
                    foreach (CsCanalisation item in _LstCannalisation)
                    {
                        //RetourneEvenement(item);
                        if (item.DEPOSE != null)
                            item.LIBELLEETATCOMPTEUR = Galatee.Silverlight.Resources.Accueil.Langue.lbl_EtatCompteDepose;
                        else
                            item.LIBELLEETATCOMPTEUR = Galatee.Silverlight.Resources.Accueil.Langue.lbl_EtatCompteActif;
                       
                    }
                    dtgCompteur .ItemsSource = _LstCannalisation;
                    dtgCompteur.SelectedItem = _LstCannalisation[0];

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
                    return;
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneCodeRegroupementCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeRegroupement = args.Result;
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
            ValiderInitialisation(null, true);
        }
        private void ValiderInitialisation(CsDemande demandedevis, bool IsTransmetre)
        {

            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible ;
                ValdiderDemandeTrf(laDetailDemande);

            }
            catch (Exception ex)
            {
                Message.ShowError("Une erreur s'est produite a la validation ", "ValiderDemandeInitailisation");
            }
        }
        private void ValdiderDemandeTrf(CsDemande laDemande)
        { 
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.VerificationDemandeTransfertAsync(laDemande.Transfert ,laDemande.LaDemande );
                service.VerificationDemandeTransfertCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (string.IsNullOrEmpty( args.Result))
                            Message.ShowInformation( "Demande transmise avec succès","Demande");
                        else
                        {
                            Message.ShowInformation(args.Result, "Erreur");
                            return;
                        }
                        this.DialogResult = true;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
             
           
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
                    pDemandeDevis.LaDemande.CLIENT = string.IsNullOrEmpty(this.txt_Ref_Branchement.Text) ? string.Empty : this.txt_Ref_Branchement .Text;

                pDemandeDevis.LaDemande.DATEMODIFICATION = DateTime.Now;
                //pDemandeDevis.LaDemande.MOTIF = txt_Motif.Text;
 
                pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t=>t.CODE == SessionObject.Enumere.TransfertAbonnement).PK_ID ;
                pDemandeDevis.LaDemande.TYPEDEMANDE = SessionObject.Enumere.TransfertAbonnement;
                 
                #region Doc Scanne
                if (pDemandeDevis.ObjetScanne == null) pDemandeDevis.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                #region Doc Scanne
                if (pDemandeDevis.ObjetScanne == null) laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                if (ctrl != null && ctrl.LstPiece != null)
                {
                    pDemandeDevis.ObjetScanne.Clear();
                    pDemandeDevis.ObjetScanne.AddRange(ctrl.LstPiece.Where(i => i.ISNEW == true || i.ISTOREMOVE == true));
                    //foreach (Galatee.Silverlight.Accueil.CsDocumentScan item in ctrl.LstPieceSave)
                    //    Classes.SaveDocument.SaveDoc(item.ObjScanne, SessionObject.CheminImpression, laDetailDemande.LaDemande.NUMDEM + item.TypeDocument, "jpeg"); 
                }
                #endregion
                #endregion

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

        private void cbo_typedoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSite_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtCentre_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dtgCompteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.dtgCompteur .SelectedIndex >= 0)
                {
                    CsCanalisation _LeCompteurSelect = (CsCanalisation)this.dtgCompteur.SelectedItem;
                    RemplireCannalisationProduit(_LeCompteurSelect);
                    if (laDetailDemande.LstEvenement != null && laDetailDemande.LstEvenement.Count != 0)
                        RemplireOngletEvenement(laDetailDemande.LstEvenement.Where(p => p.PRODUIT == _LeCompteurSelect.PRODUIT && p.POINT == _LeCompteurSelect.POINT).ToList());

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);

            }
        }
        private void RemplireOngletEvenement(List<CsEvenement> _LstEvenement)
        {
            try
            {
                tab5_Stab2dataGrid2.ItemsSource = null;
                if (_LstEvenement != null && _LstEvenement.Count != 0)
                {
                    if (_LstEvenement != null && _LstEvenement.Count != 0)
                    {
                        if (_LstEvenement.First().PRODUIT == SessionObject.Enumere.ElectriciteMT)
                            _LstEvenement.ForEach(t => t.REGLAGECOMPTEUR = t.TYPECOMPTAGE);
                        tab5_Stab2dataGrid2.ItemsSource = _LstEvenement.OrderBy(t => t.NUMEVENEMENT);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void tab4_cbo_Operation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.tab4_cbo_Operation.SelectedIndex >= 0)
                RemplirTypeAction(this.tab4_cbo_Operation.SelectedIndex);
        }


    
    }
}


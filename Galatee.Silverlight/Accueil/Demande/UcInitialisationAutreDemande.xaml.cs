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
    public partial class UcInitialisationAutreDemande : ChildWindow
    {
        private UcImageScanne formScanne = null;
        private Object ModeExecution = null;
        private ObjDEVIS ObjetDevisSelectionne = null;
        private List<CsCentre> _listeDesCentreExistant = null;
        private  CsTdem _leTypeDemandeExistant = null;
        private string Tdem = null;
        private DataGrid _dataGrid = null;
        private List<CsUsage> lstusage = new List<CsUsage>();

        List<CsTarif> LstTarif = new List<CsTarif>();
        List<CsForfait> LstForfait = new List<CsForfait>();
        List<CsPuissance> LstPuissance = new List<CsPuissance>();
        List<CsFrequence> LstFrequence = new List<CsFrequence>();
        List<CsMois> LstMois = new List<CsMois>();
        List<CsCodeTaxeApplication> LstCodeApplicationTaxe = new List<CsCodeTaxeApplication>();


        public UcInitialisationAutreDemande()
        {
            InitializeComponent();
        }

        public UcInitialisationAutreDemande(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid, string Tdem = "05")
        {
            try
            {
                InitializeComponent();
                this.Tdem = Tdem;
                label1.Visibility = Visibility.Collapsed;
                Txt_NumDevis.Visibility = Visibility.Collapsed;
                if (pExecMode != null) ModeExecution = pExecMode[0];
                var devis = new ObjDEVIS();
                if (pObjects[0] != null && pExecMode[0] != SessionObject.ExecMode.Creation)
                    ObjetDevisSelectionne = Utility.ParseObject(devis, pObjects[0] as ObjDEVIS);

                if (pGrid != null) _dataGrid = pGrid[0];
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

        public UcInitialisationAutreDemande(int _IdDemandeDevis, int PK_Id_Tdem = 0)
        {
            try
            {
                InitializeComponent();
                label1.Visibility = Visibility.Visible;
                Txt_NumDevis.Visibility = Visibility.Visible;
                ModeExecution = SessionObject.ExecMode.Modification;
                IdDemandeDevis = _IdDemandeDevis;
                ChargerListDesSite();
                ChargeDetailDEvis(_IdDemandeDevis);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        public UcInitialisationAutreDemande(string Tdem)
        {
            try
            {
                InitializeComponent();
                this.Tdem = Tdem;
                ModeExecution = SessionObject.ExecMode.Creation;

                label1.Visibility = Visibility.Collapsed;
                Txt_NumDevis.Visibility = Visibility.Collapsed;
                ChargerListDesSite();
                RemplirListeDesTypeDemandeExistant();
                //Activation de la zone de recherche en fonction du type de demande
                ActivationEnFonctionDeTdem();
                EnabledDevisInformations(true);
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
                if (SessionObject.LstPuissance.Count != 0)
                {
                    LstPuissance = SessionObject.LstPuissance;
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerPuissanceCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstPuissance = args.Result;
                        LstPuissance = SessionObject.LstPuissance;

                    };
                    service.ChargerPuissanceAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerForfait()
        {
            try
            {
                if (SessionObject.LstForfait.Count != 0)
                    LstForfait = SessionObject.LstForfait;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerForfaitCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;

                        SessionObject.LstForfait = args.Result;
                        LstForfait = SessionObject.LstForfait;
                    };
                    service.ChargerForfaitAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerTarif()
        {
            try
            {
                if (SessionObject.LstTarif.Count != 0)
                    LstTarif = SessionObject.LstTarif;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerTarifCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstTarif = args.Result;
                        LstTarif = SessionObject.LstTarif;

                    };
                    service.ChargerTarifAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerFrequence()
        {
            if (SessionObject.LstFrequence.Count != 0)
                LstFrequence = SessionObject.LstFrequence;
            else
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerTousFrequenceCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstFrequence = args.Result;
                    LstFrequence = SessionObject.LstFrequence;
                };
                service.ChargerTousFrequenceAsync();
                service.CloseAsync();
            }
        }
        private void ChargerMois()
        {
            try
            {
                if (SessionObject.LstMois.Count != 0)
                    LstMois = SessionObject.LstMois;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerTousMoisCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstMois = args.Result;
                        LstMois = SessionObject.LstMois;
                    };
                    service.ChargerTousMoisAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerApplicationTaxe()
        {
            try
            {
                if (SessionObject.LstCodeApplicationTaxe.Count != 0)
                    LstCodeApplicationTaxe = SessionObject.LstCodeApplicationTaxe;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneTousApplicationTaxeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeApplicationTaxe = args.Result;
                        LstCodeApplicationTaxe = SessionObject.LstCodeApplicationTaxe;
                    };
                    service.RetourneTousApplicationTaxeAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
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
                    laDemandeSelect = laDetailDemande.LaDemande;
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void FermerFenetre()
        {
            try
            {
                DialogResult = true;
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
                        this.txtSite.Text = csSite.CODESITE ?? string.Empty;
                        if (laDemandeSelect != null)
                        {
                            if (laDemandeSelect.FK_IDCENTRE != 0)
                                RemplirCentreDuSite(csSite.PK_ID, laDemandeSelect.FK_IDCENTRE);
                        }
                        else
                            RemplirCentreDuSite(csSite.PK_ID, 0);

                    }
                }
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
                    _leSite.CODESITE = r.CODESITE;
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void UcInitialisation_Loaded(object sender, RoutedEventArgs e)
        {
            //int resultLoding = 0;
            try
            {
                //Btn_Transmettre.Visibility = System.Windows.Visibility.Collapsed;
                LayoutRoot.Cursor = Cursors.Wait;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    Txt_NumDevis.Text = string.Empty;
                }
                else if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    this.Btn_Annuler.IsEnabled = true;
                    this.Btn_Annuler.Content = Languages.btnFermer;

                    //this.btnCheck.Visibility = this.btnFind.Visibility /*= this.Btn_Enregistrer.Visibility*/ = System.Windows.Visibility.Collapsed;

                    if (ObjetDevisSelectionne != null) // Chercher les informations de DEMANDEDEVIS
                    {
                    }
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                Message.ShowError(ex.Message, Languages.txtDevis);
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
                        _leTypeDemandeExistant = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == this.Tdem);
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
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
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
                        RemplirProduitCentre(centre);
                    }
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
                        btn_rech_branch.IsEnabled = true;
                        txt_Ref_Branchement.IsEnabled = true;
                    }
                    else
                    {
                        btn_rech_branch.IsEnabled = false;
                        txt_Ref_Branchement.IsEnabled = false;
                    }
                    if (this.Tdem == SessionObject.Enumere.AugmentationPuissance || this.Tdem == SessionObject.Enumere.DimunitionPuissance)
                    {
                    }
                    else
                    {
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


        public const string MatchEmailPattern =  @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
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
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
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
                    //laDetailDemande_temp = args.Result;
                    desableProgressBar();
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
            client.GeDetailByFromClientAsync(leclient);
        }
        private void tabC_Onglet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txt_Ref_Branchement.Text.Length == 11)
            {
                allowProgressBar();
                CsClient leClient = new CsClient();
                leClient.FK_IDCENTRE  =(int) this.txtCentre.Tag ;
                leClient.CENTRE = this.txtCentre.Text ;
                leClient.REFCLIENT = this.txt_Ref_Branchement.Text;
                leClient.PRODUIT = this.txtProduit.Text;
                RetourneOrdre(leClient);
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
        private void Txt_CodeForfait_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LstForfait.Count != 0 && this.Txt_CodeForfait.Text.Length == SessionObject.Enumere.TailleForfait)
                {
                    CsForfait _LeForfait = LstForfait.FirstOrDefault(p => p.PRODUIT  == this.txtProduit.Text  && p.CODE == this.Txt_CodeForfait.Text);
                    if (_LeForfait != null)
                    {
                        this.Txt_LibelleForfait.Text = _LeForfait.LIBELLE;
                        this.Txt_CodeForfait.Tag  = _LeForfait.PK_ID ;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeForfait.Focus();
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
        private void Txt_CodeForfait_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Txt_CodeForfait.Text.Length != 0)
                this.Txt_CodeForfait.Text = this.Txt_CodeForfait.Text.PadLeft(SessionObject.Enumere.TailleForfait, '0');
        }

        private void btn_forfait_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstForfait.Count != 0)
                {
                    this.btn_forfait.IsEnabled = false;
                    List<object> _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstForfait.Where(t => t.PRODUIT == this.txtProduit .Text).ToList());
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObjet, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnForfait);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message,Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void galatee_OkClickedBtnForfait(object sender, EventArgs e)
        {
            try
            {
                this.btn_forfait.IsEnabled = true;
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsForfait _Leforfait = (CsForfait)ctrs.MyObject;
                    this.Txt_CodeForfait.Text = _Leforfait.CODE;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }

        private void btn_frequence_Click(object sender, RoutedEventArgs e)
        {
            this.btn_frequence.IsEnabled = false;
            if (LstFrequence != null && LstFrequence.Count != 0)
            {
                List<object> _ListObj = ClasseMEthodeGenerique.RetourneListeObjet(LstFrequence);
                this.btn_frequence.IsEnabled = false;
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_ListObj, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnFrequence);
                ctr.Show();
            }
        }
        private void galatee_OkClickedBtnFrequence(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsFrequence _LaFrequence = (CsFrequence)ctrs.MyObject;
                this.Txt_CodeFrequence.Text = _LaFrequence.CODE;
                this.Txt_LibelleFrequence.Text = _LaFrequence.LIBELLE;
                this.Txt_CodeFrequence.Tag = _LaFrequence.PK_ID;
            }
            this.btn_frequence.IsEnabled = true;
        }

        private void Txt_CodeMoisFacturation_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LstMois.Count != 0 && this.Txt_CodeMoisFacturation.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
                {
                    CsMois _LeMois = ClasseMEthodeGenerique.RetourneObjectFromList(LstMois, this.Txt_CodeMoisFacturation.Text, "CODE");
                    if (_LeMois != null)
                    {
                        this.Txt_LibMoisFact.Text = _LeMois.LIBELLE;
                        this.Txt_CodeMoisFacturation.Tag = _LeMois.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeMoisFacturation.Focus();
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
        private void Txt_CodeMoisFacturation_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Txt_CodeMoisFacturation.Text.Length != 0)
                this.Txt_CodeMoisFacturation.Text = this.Txt_CodeMoisFacturation.Text.PadLeft(SessionObject.Enumere.TailleMoisDeFacturation, '0');
        }
        private void btn_moisdefacturation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstMois.Count != 0)
                {
                    this.btn_moisdefacturation.IsEnabled = false;
                    List<object> _LstOject = ClasseMEthodeGenerique.RetourneListeObjet(LstMois);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstOject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnMoisFact);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void galatee_OkClickedBtnMoisFact(object sender, EventArgs e)
        {
            try
            {
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsMois _LeMois = (CsMois)ctrs.MyObject;
                    this.Txt_CodeMoisFacturation.Text = _LeMois.CODE;
                }
                this.btn_moisdefacturation.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }

        private void btn_PussSouscrite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstPuissance.Count != 0)
                {
                    List<object> _LstObjet = ClasseMEthodeGenerique.RetourneListeObjet(LstPuissance.Where(t => t.CODE == this.Txt_CodeTarif.Text).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnpuissanceSouscrite);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message,Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }

        private void galatee_OkClickedBtnpuissanceSouscrite(object sender, EventArgs e)
        {
            try
            {
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsPuissance _LaPuissanceSelect = (CsPuissance)ctrs.MyObject;
                    this.Txt_CodePussanceSoucrite.Text = _LaPuissanceSelect.CODE;
                    this.Txt_CodePuissanceUtilise.Text = _LaPuissanceSelect.CODE;
                }
                else
                    this.btn_PussSouscrite.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }

        private void Txt_CodeMoisIndex_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LstMois.Count != 0 && this.Txt_CodeMoisIndex.Text.Length == SessionObject.Enumere.TailleMoisDeFacturation)
                {
                    CsMois _LeMois = ClasseMEthodeGenerique.RetourneObjectFromList(LstMois, this.Txt_CodeMoisIndex.Text, "CODE");
                    if (_LeMois != null)
                    {
                        this.Txt_LibelleMoisIndex.Text = _LeMois.LIBELLE;
                        this.Txt_CodeMoisIndex.Tag = _LeMois.PK_ID;
                        //EnregisterDemande(LaDemande);
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue .lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeMoisFacturation.Focus();
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
        private void btn_MoisIndex_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstMois.Count != 0)
                {
                    this.btn_MoisIndex.IsEnabled = false;
                    List<object> _LstOject = ClasseMEthodeGenerique.RetourneListeObjet(LstMois);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstOject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnMoisIndex);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void galatee_OkClickedBtnMoisIndex(object sender, EventArgs e)
        {
            try
            {
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsMois _LeMois = (CsMois)ctrs.MyObject;
                    this.Txt_CodeMoisIndex.Text = _LeMois.CODE;
                }
                this.btn_MoisIndex.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
    }
}


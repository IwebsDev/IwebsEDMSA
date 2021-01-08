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
using Galatee.Silverlight.ServiceAccueil;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmValidationRemboursementAvance : ChildWindow
    {
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode { get; set; }
        CsDemandeBase laDemandeSelect = null;
        bool isPreuveSelectionnee = false;
        private UcImageScanne formScanne = null;
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        CsDemande laDetailDemande = null;
        string Facture = string.Empty;
        string Periode = string.Empty;
        private List<CsCentre> _listeDesCentreExistant = null;

        public FrmValidationRemboursementAvance()
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            ChargerTypeDocument();
            ChargerListDesSite();
            AfficherOuMasquer(tabItemCompte, false);
        }
        public FrmValidationRemboursementAvance(int idDemande)
        {
            InitializeComponent();
            ChargerTypeDocument();
            ChargerListDesSite();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            ChargeDetailDEvis(idDemande);
            AfficherOuMasquer(tabItemCompte, false);
        }
        string Tdem = string.Empty;
        public FrmValidationRemboursementAvance(string TypeDemande, string Init)
        {
            InitializeComponent();
            ChargerTypeDocument();
            ChargerListDesSite();
            this.Txt_ReferenceClient.MaxLength = SessionObject.Enumere.TailleClient;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            Tdem = TypeDemande;
            AfficherOuMasquer(tabItemCompte, false);
        }
        private void AfficherOuMasquer(TabItem pTabItem, bool pValue)
        {
            try
            {
                pTabItem.Visibility = pValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur ", "AfficherOuMasquer");
            }
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            try
            {
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GetDevisByNumIdDevisCompleted += (ssender, args) =>
                {
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
                        laDetailDemande = new CsDemande();
                        laDetailDemande = args.Result;
                        this.txtSite.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLESITE) ? string.Empty : laDetailDemande.LaDemande.LIBELLESITE;
                        this.txtCentre.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLECENTRE) ? string.Empty : laDetailDemande.LaDemande.LIBELLECENTRE;
                        this.txt_Produit.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLEPRODUIT) ? string.Empty : laDetailDemande.LaDemande.LIBELLEPRODUIT;
                        this.Txt_NumeroDemande.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                        this.Txt_ReferenceClient.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? string.Empty : laDetailDemande.LaDemande.CLIENT;
                        this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == laDetailDemande.LaDemande.TYPEDEMANDE).LIBELLE;

                        txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == laDetailDemande.LaDemande.TYPEDEMANDE);
                        RemplireOngletClient(laDetailDemande.LeClient);
                        RemplirOngletAbonnement(laDetailDemande.Abonne);
                        RemplirOngketPieceJoint(laDetailDemande.ObjetScanne);
                        ChargerCompteDeResiliation(laDetailDemande.LeClient);
                    }
                };
                client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
            }
            catch (Exception ex)
            {
                 Message.ShowInformation("Erreur au chargement des données", "Erreur");
            }
        }
        //List<ObjDOCUMENTSCANNE> LstPiece = new List<ObjDOCUMENTSCANNE>();
        private void RemplirOngketPieceJoint(List<ObjDOCUMENTSCANNE> list)
        {
            #region DocumentScanne
            if (list != null && list.Count() != 0)
            {
                //isPreuveSelectionnee = true;
                ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                LstPiece = new List<ObjDOCUMENTSCANNE>();
                foreach (var item in list)
                {
                    LstPiece.Add(item);
                    ObjetScanne.Add(item);
                }
                dgListePiece.ItemsSource = null;
                dgListePiece.ItemsSource = ObjetScanne;

            }
            else
            {
                //isPreuveSelectionnee = false;
            }
            #endregion
        }
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        List<int> lstIdCentre = new List<int>();
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

        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
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
                //EnabledDevisInformations(true);
            }
            else
            {
                this.isPreuveSelectionnee = false;
                //EnabledDevisInformations(false);
            }
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
        public List<ObjDOCUMENTSCANNE> LstPiece = new List<ObjDOCUMENTSCANNE>();
        private byte[] image;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.OKButton.IsEnabled = false;
                this.DialogResult = true;
                ValiderInitialisation(laDetailDemande , true);
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
        private CsDemande GetDemandeDevisFromScreen(CsDemande pDemandeDevis, bool isTransmettre)
        {
            if (pDemandeDevis == null)
            {
                pDemandeDevis = new CsDemande();
                pDemandeDevis.LaDemande = new CsDemandeBase();
                pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                pDemandeDevis.LaDemande.USERCREATION = UserConnecte.matricule;
                pDemandeDevis.LaDemande.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                pDemandeDevis.LaDemande.TYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.RemboursementTrvxNonRealise).CODE;
                pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.RemboursementTrvxNonRealise).PK_ID;
            }
            if (pDemandeDevis.LaDemande == null) pDemandeDevis.LaDemande = new CsDemandeBase();
            pDemandeDevis.LaDemande.DATEMODIFICATION = DateTime.Now;
            List<CsDemandeDetailCout> lesCoutduDevis = new List<CsDemandeDetailCout>();
            if (lstFactureDuClient != null && lstFactureDuClient.Count != 0)
            {

                CsDemandeDetailCout leCoutduDevis;
                foreach (CsLclient st in lstFactureDuClient)
                {
                    leCoutduDevis = new CsDemandeDetailCout();
                    leCoutduDevis.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                    leCoutduDevis.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                    leCoutduDevis.ORDRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.ORDRE) ? null : laDetailDemande.LaDemande.ORDRE;
                    leCoutduDevis.NUMDEM = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? null : laDetailDemande.LaDemande.NUMDEM;
                    leCoutduDevis.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                    leCoutduDevis.COPER = SessionObject.Enumere.CoperRAC;
                    leCoutduDevis.MONTANTTAXE = 0;
                    leCoutduDevis.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperRAC).PK_ID;
                    leCoutduDevis.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == "00").PK_ID;
                    //leCoutduDevis.REFEM = System.DateTime.Today.Year + System.DateTime.Today.Month.ToString("00");
                    //leCoutduDevis.NDOC = System.DateTime.Today.Year + System.DateTime.Today.Month.ToString("00");


                    leCoutduDevis.DATECREATION = DateTime.Now;
                    leCoutduDevis.USERCREATION = UserConnecte.matricule;

                    if (st.REFEM.Length == 7)
                        leCoutduDevis.REFEM = st.REFEM.Substring(3, 4) + st.REFEM.Substring(0, 2);
                    else
                        leCoutduDevis.REFEM = st.REFEM;
                    leCoutduDevis.NDOC = st.NDOC;
                    leCoutduDevis.MONTANTHT = st.SOLDEFACTURE * (-1);

                    lesCoutduDevis.Add(leCoutduDevis);
                }
            }
            pDemandeDevis.LstCoutDemande = lesCoutduDevis;
            return pDemandeDevis;
        }

        private void ValiderInitialisation(CsDemande demandedevis, bool IsTransmetre)
        {

            try
            {

                demandedevis = GetDemandeDevisFromScreen(demandedevis, false);
                if (demandedevis != null)
                {
                    if (IsTransmetre)
                        demandedevis.LaDemande.ETAPEDEMANDE = null;
                    demandedevis.LaDemande.MATRICULE = UserConnecte.matricule;
                    //demandedevis.LaDemande.CENTRE = SessionObject.LePosteCourant.CODECENTRE;
                    //demandedevis.LaDemande.FK_IDCENTRE = SessionObject.LePosteCourant.FK_IDCENTRE.Value;

                    demandedevis.LeClient = null ;
                    demandedevis.Abonne = null;
                    demandedevis.Branchement = null;
                    demandedevis.Ag  = null;
                    AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    client.ValidationDemandeCompleted += (ss, b) =>
                    {
                        this.OKButton.IsEnabled = true;
                        if (b.Cancelled || b.Error != null)
                        {
                            string error = b.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        //List<CsLclient> lesFacture = lstFactureDuClient.Where(t => t.COPER != SessionObject.Enumere.CoperCAU).ToList();
                        //decimal MontantAvance =
                        //LettrageAutomatique();
 /*                       if (IsTransmetre)
                        {
                            List<string> codes = new List<string>();
                            codes.Add(laDetailDemande.InfoDemande.CODE);
                            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);

                            //List<CsUtilisateur> leUser = new List<CsUtilisateur>();
                            //if (laDetailDemande.InfoDemande != null && laDetailDemande.InfoDemande.CODE != null)
                            //{
                            //    foreach (CsUtilisateur item in laDetailDemande.InfoDemande.UtilisateurEtapeSuivante)
                            //        leUser.Add(item);
                            //    Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", laDetailDemande.LaDemande.NUMDEM, laDetailDemande.LaDemande.LIBELLETYPEDEMANDE);
                            //}
                        }
  */ 
                    };
                    /*client.ValiderDemandeAsync(demandedevis);*/
                    client.ValidationDemandeAsync(demandedevis, true);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Une erreur est survenu suite à la validation", "Validation demande");
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
        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void btn_transmetre_Click(object sender, RoutedEventArgs e)
        {
        }

        private void RemplireOngletClient(CsClient _LeClient)
        {
            try
            {
                if (_LeClient != null)
                {

                    this.Txt_NomClient.Text = (string.IsNullOrEmpty(_LeClient.NOMABON) ? string.Empty : _LeClient.NOMABON);
                    this.Txt_Telephone.Text = string.IsNullOrEmpty(_LeClient.TELEPHONE) ? string.Empty : _LeClient.TELEPHONE;
                    //this.tab12_txt_addresse.Text = string.IsNullOrEmpty(_LeClient.ADRMAND1) ? string.Empty : _LeClient.ADRMAND1;
                    //this.tab12_txt_addresse2.Text = string.IsNullOrEmpty(_LeClient.ADRMAND2) ? string.Empty : _LeClient.ADRMAND2;
                    //this.txt_NINA.Text = string.IsNullOrEmpty(_LeClient.NUMEROIDCLIENT) ? string.Empty : _LeClient.NUMEROIDCLIENT;
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

        private void Cbo_Produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
        List<CsLclient> lstFactureDuClient;
        private void ChargerCompteDeResiliation(CsClient _UnClient)
        {

            try
            {
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.ChargerCompteDeResiliationCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == null || args.Result.Count == 0)
                    {
                        Message.ShowInformation("Ce client n'existe pas", "Demande");
                        return;
                    }
                    lstFactureDuClient = new List<CsLclient>();
                    lstFactureDuClient = args.Result;

                    lstFactureDuClient.ForEach(t => t.REFEM = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(t.REFEM));
                    if (lstFactureDuClient != null && lstFactureDuClient.Count != 0)
                    {
                        AfficherOuMasquer(tabItemCompte, true);
                        dtg_CompteClient.ItemsSource = lstFactureDuClient;
                        //Txt_TotalSoldeResil.Text = lstFactureDuClient.Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
                        Txt_TotalSoldeResil.Text = lstFactureDuClient.First().SOLDEFACTURE.Value.ToString(SessionObject.FormatMontant);

                        //if (lstFactureDuClient.Sum(t => t.SOLDEFACTURE) >= 0)
                        if (lstFactureDuClient.First().SOLDEFACTURE >= 0)
                        {
                            Message.ShowInformation("Aucun remboursement prevu pour ce client", "Demande");
                            this.OKButton.IsEnabled = false;
                            return;
                        }
                        this.OKButton.IsEnabled = true;
                    }
                };
                client.ChargerCompteDeResiliationAsync(_UnClient);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur au chargement des cout", "Demande");

            }
        }

        private void  LettrageAutomatique(List<CsLclient> _ListeFactureAreglee, decimal montantAvance)
        {
            try
            {
                List<CsLclient> lstFactureAValider = new List<CsLclient>();
                decimal? MontantRestant = montantAvance;
                foreach (CsLclient laFactureClient in _ListeFactureAreglee)
                {
                    if (MontantRestant > 0)
                    {
                        decimal? MontantPaye = MontantRestant;
                        if (laFactureClient.SOLDEFACTURE <= MontantPaye)
                            MontantPaye = laFactureClient.SOLDEFACTURE;
                        else if (laFactureClient.SOLDEFACTURE >= MontantPaye)
                            MontantPaye = MontantRestant;

                        CsLclient leReglement = GetElementDeReglement(laFactureClient, MontantPaye);
                            lstFactureAValider.Add(leReglement);
                            MontantRestant = MontantRestant - laFactureClient.SOLDEFACTURE;
                            lstFactureAValider.Add(leReglement);
                    }
                }
                //AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                //client.LettrageAutomatiqueCompleted += (ss, b) =>
                //{
                //    if (b.Cancelled || b.Error != null)
                //    {
                //        string error = b.Error.Message;
                //        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                //        return;
                //    }
                //    List<string> codes = new List<string>();
                //    codes.Add(laDetailDemande.InfoDemande.CODE);
                //    Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);

                //    List<CsUtilisateur> leUser = new List<CsUtilisateur>();
                //    if (laDetailDemande.InfoDemande != null && laDetailDemande.InfoDemande.CODE != null)
                //    {
                //        foreach (CsUtilisateur item in laDetailDemande.InfoDemande.UtilisateurEtapeSuivante)
                //            leUser.Add(item);
                //        Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", laDetailDemande.LaDemande.NUMDEM, laDetailDemande.LaDemande.LIBELLETYPEDEMANDE);
                //    }
                //};
                //client.LettrageAutomatiqueAsync(lstFactureAValider);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private CsLclient GetElementDeReglement(CsLclient Facture, decimal? Montant)
        {
            CsLclient Reglement = Facture;
            try
            {
                if (!string.IsNullOrEmpty(Reglement.REFEM) && Reglement.REFEM.Length == 7)
                    Reglement.REFEM = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(Facture.REFEM);

                //Reglement.ACQUIT =  ;
                Reglement.CAISSE = SessionObject.LePosteCourant.NUMCAISSE;
                Reglement.DC = Facture.DC;
                Reglement.MODEREG = SessionObject.Enumere.ModePayementEspece  ;
                Reglement.MOISCOMPT = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00");
                Reglement.TOP1 = SessionObject.Enumere.TopCaisse;
                Reglement.ECART = 0;
                Reglement.PERCU = Facture.SOLDEFACTURE ;
                Reglement.USERCREATION = Facture.USERCREATION == null ? UserConnecte.matricule : Facture.USERCREATION;
                Reglement.USERMODIFICATION = UserConnecte.matricule;
                Reglement.DATECREATION = Facture.DATECREATION == null ? DateTime.Now : Facture.DATECREATION;
                Reglement.DATEMODIFICATION = Facture.DATEMODIFICATION == null ? DateTime.Now : Facture.DATEMODIFICATION;
                Reglement.ORIGINE = UserConnecte.Centre;
                Reglement.POSTE = SessionObject.LePosteCourant.NOMPOSTE;
                Reglement.DENR = System.DateTime.Now;
                Reglement.DTRANS = System.DateTime.Today.Date ;
                Reglement.NOMCAISSIERE = UserConnecte.nomUtilisateur;

                Reglement.NUMCHEQ = null;
                Reglement.PLACE = "-";
                Reglement.BANQUE = "------";
                Reglement.GUICHET = "------";

                Reglement.NUMDEM = Facture.NUMDEM;
                Reglement.NUMDEVIS = Facture.NUMDEVIS;
                Reglement.FK_IDCENTRE = Facture.FK_IDCENTRE;
                Reglement.FK_IDLCLIENT = Facture.PK_ID;
                Reglement.FK_IDCLIENT = Facture.FK_IDCLIENT;
                Reglement.FK_IDCOPER = SessionObject.LstDesCopers.First(t => t.CODE == Facture.COPER).PK_ID;
                Reglement.NATURE = "00";
                Reglement.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                Reglement.FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.First(t => t.CODE == SessionObject.Enumere.TopCaisse).PK_ID;
                Reglement.FK_IDHABILITATIONCAISSE = SessionObject.LaCaisseCourante.PK_ID;
                Reglement.FK_IDCAISSIERE = SessionObject.LaCaisseCourante.FK_IDCAISSIERE;
                Reglement.FK_IDAGENTSAISIE = null;
                Reglement.FK_IDPOSTECLIENT = null;
                Reglement.FK_IDMODEREG = SessionObject.ListeModesReglement.FirstOrDefault(t=>t.CODE == SessionObject.Enumere.ModePayementEspece).PK_ID ;
                Reglement.MATRICULE = UserConnecte.matricule;
                Reglement.LIBELLESITE = SessionObject.LstCentre.FirstOrDefault(t => SessionObject.LaCaisseCourante.FK_IDCENTRE == t.PK_ID).LIBELLESITE;
                Reglement.LIBELLEAGENCE = SessionObject.LstCentre.FirstOrDefault(t => SessionObject.LaCaisseCourante.FK_IDCENTRE == t.PK_ID).LIBELLE;
                Reglement.LIBELLEAGENCE = SessionObject.LstCentre.FirstOrDefault(t => SessionObject.LaCaisseCourante.FK_IDCENTRE == t.PK_ID).LIBELLE;
                Reglement.IsPAIEMENTANTICIPE = Facture.IsPAIEMENTANTICIPE;


                Reglement.MONTANTEXIGIBLE = Facture.MONTANTEXIGIBLE;
                Reglement.MONTANTNONEXIGIBLE = Facture.MONTANTNONEXIGIBLE;
                Reglement.SOLDECLIENT = Facture.SOLDECLIENT;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Reglement;
        }


        public List<ObjDOCUMENTSCANNE> ObjetScanne { get; set; }

        private void RejeterButton_Click(object sender, RoutedEventArgs e)
        {
            this.OKButton.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            this.RejeterButton.IsEnabled = false;
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
        }
    }
}


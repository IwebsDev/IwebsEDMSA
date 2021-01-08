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
    public partial class FrmInitRemboursementAvance : ChildWindow
    {
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode { get; set; }
        CsDemandeBase laDemandeSelect = null;
        bool isPreuveSelectionnee = false;
        private UcImageScanne formScanne = null;
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        CsDemande laDetailDemande = null;
        private List<CsCentre> _listeDesCentreExistant = null;

        public FrmInitRemboursementAvance()
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            ChargerTypeDocument();
            this.Txt_ReferenceClient.MaxLength = SessionObject.Enumere.TailleClient;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            ChargerListDesSite();
            AfficherOuMasquer(tabItemCompte, false);
        }
        string Tdem = string.Empty;
        public FrmInitRemboursementAvance(string TypeDemande,string IsInit)
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
                throw ex;
            }
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
            this.LstPiece.Add(new ObjDOCUMENTSCANNE { PK_ID = Guid.NewGuid(), NOMDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).LIBELLE, FK_IDTYPEDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).PK_ID, CONTENU = image, DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule, ISNEW = true });
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
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        private byte[] image;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.OKButton.IsEnabled = false;
                this.CancelButton .IsEnabled = false;
                //this.DialogResult = true;

                #region Doc Scanne
                if (laDetailDemande.ObjetScanne == null) laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                laDetailDemande.ObjetScanne.AddRange(this.LstPiece);
                #endregion



                VerifierRemboursementEnCours(laDetailDemande);
                //ValiderInitialisation(null, true);
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }


        private void VerifierRemboursementEnCours(CsDemande demandedevis)
        {

            try
            {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    client.VerifierRemboursementEnCoursCompleted += (ss, b) =>
                    {
                        if (b.Cancelled || b.Error != null)
                        {
                            string error = b.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (b.Result)
                        {
                            Message.ShowInformation("Ce client a déjà un remboursement en cours", "Remboursement ASC");
                            this.DialogResult = false;
                        }
                        else
                        {
                            ValiderInitialisation(null, true);
                            this.DialogResult = true;
                        }
                    };
                    client.VerifierRemboursementEnCoursAsync(demandedevis);


            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur lors de la vérification du client ", "Remboursement ASC");
            }
        }

        

        private void ValiderInitialisation(CsDemande demandedevis, bool IsTransmetre)
        {

            try
            {
                demandedevis = GetDemandeDevisFromScreen(demandedevis, false);
                // Get DemandeDevis informations from screen
                if (demandedevis != null)
                {
                    demandedevis.LaDemande.ETAPEDEMANDE = (int)DataReferenceManager.EtapeDevis.Accueil;
                    if (IsTransmetre)
                        demandedevis.LaDemande.ETAPEDEMANDE = null;
                    demandedevis.LaDemande.MATRICULE = UserConnecte.matricule;

                    
                    demandedevis.LeClient = laDetailDemande.LeClient;
                    demandedevis.Abonne = laDetailDemande.Abonne;
                    demandedevis.Branchement = laDetailDemande.Branchement;
                    demandedevis.Ag = laDetailDemande.Ag;
                    string numDemande = string.Empty;



                    /*
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                            numDemande = coupe[1];
                        }
                        List<CsDemandeBase> leDemandeAEditer = new List<CsDemandeBase>();
                        demandedevis.LaDemande.NOMCLIENT = demandedevis.LeClient.NOMABON;
                        demandedevis.LaDemande.LIBELLETYPEDEMANDE = txt_tdem.Text;
                        demandedevis.LaDemande.NUMDEM = numDemande;
                        demandedevis.LaDemande.LIBELLEPRODUIT = this.txt_Produit.Text;
                        demandedevis.LaDemande.MOTIF = this.txt_Motif.Text;
                        leDemandeAEditer.Add(demandedevis.LaDemande);
                        Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(leDemandeAEditer, null, SessionObject.CheminImpression, "AccuseRecption", "Accueil", true);
                    };
                    client.ValiderDemandeInitailisationAsync(demandedevis);
                     * 
                     * */


                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    client.CreeDemandeCompleted += (ss, b) =>
                    {
                        if (b.Cancelled || b.Error != null)
                        {
                            string error = b.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (b.Result != null)
                        {
                            List<CsDemandeBase> leDemandeAEditer = new List<CsDemandeBase>();
                            demandedevis.LaDemande.NOMCLIENT = demandedevis.LeClient.NOMABON;
                            demandedevis.LaDemande.LIBELLETYPEDEMANDE = txt_tdem.Text;
                            demandedevis.LaDemande.NUMDEM = b.Result.NUMDEM; ;
                            demandedevis.LaDemande.LIBELLEPRODUIT = this.txt_Produit.Text;
                            demandedevis.LaDemande.MOTIF = this.txt_Motif.Text;
                            leDemandeAEditer.Add(demandedevis.LaDemande);
                            Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(leDemandeAEditer, null, SessionObject.CheminImpression, "AccuseRecption", "Accueil", true);

                            Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + b.Result.NUMDEM,
                            Silverlight.Resources.Devis.Languages.txtDevis);
                        }
                    };
                    client.CreeDemandeAsync(demandedevis, IsTransmetre);



                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Une erreur s'est produite a la validation ", "ValiderDemandeInitailisation");
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
                    pDemandeDevis.LaDemande.CLIENT = string.IsNullOrEmpty(this.Txt_ReferenceClient.Text) ? string.Empty : this.Txt_ReferenceClient.Text;

                pDemandeDevis.LaDemande.DATEMODIFICATION = DateTime.Now;
                pDemandeDevis.LaDemande.MOTIF = txt_Motif.Text;

                if (txt_tdem.Tag != null)
                {
                    var typeDevis = (CsTdem)txt_tdem.Tag;
                    if (typeDevis != null)
                    {
                        pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = typeDevis.PK_ID;
                        pDemandeDevis.LaDemande.TYPEDEMANDE = typeDevis.CODE;
                    }
                }
                #region Doc Scanne
                if (pDemandeDevis.ObjetScanne == null) pDemandeDevis.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                pDemandeDevis.ObjetScanne.AddRange(LstPiece);
                #endregion

                #endregion
                pDemandeDevis.LaDemande.ISNEW = true;
                Tdem = pDemandeDevis.LaDemande.TYPEDEMANDE;
                pDemandeDevis.LaDemande.ORDRE = laDetailDemande.LeClient.ORDRE;
                pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                pDemandeDevis.LaDemande.PRODUIT = laDetailDemande.Abonne.PRODUIT;
                pDemandeDevis.LaDemande.FK_IDPRODUIT = laDetailDemande.Abonne.FK_IDPRODUIT;
                pDemandeDevis.LaDemande.CENTRE = laDetailDemande.Abonne.CENTRE;
                pDemandeDevis.LaDemande.FK_IDCENTRE = laDetailDemande.Abonne.FK_IDCENTRE;
                pDemandeDevis.LeClient = laDetailDemande.LeClient;

                return pDemandeDevis;
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

        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient && 
                    this.Txt_Ordre.Text.Length  == SessionObject.Enumere.TailleOrdre  )
                    ChargerClientFromReferenceOrdre(this.Txt_ReferenceClient.Text,this.Txt_Ordre.Text );
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
                        ChargeDetailDEvis(args.Result.First());
                };
                service.RetourneClientByReferenceAsync(ReferenceClient, lstIdCentre);
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ChargerClientFromReferenceOrdre(string ReferenceClient,string Ordre)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneClientByReferenceOrdreCompleted += (s, args) =>
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
                        ChargeDetailDEvis(args.Result.First());
                };
                service.RetourneClientByReferenceOrdreAsync (ReferenceClient,Ordre , lstIdCentre);
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void galatee_OkClickedChoixClient(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsClient _UnClient = (CsClient)ctrs.MyObject;
                ChargeDetailDEvis(_UnClient);
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
                        if (laDetailDemande.Abonne != null && !string.IsNullOrEmpty(laDetailDemande.Abonne.CLIENT))
                        {
                            //if (laDetailDemande.Abonne.AVANCE == 0 || laDetailDemande.Abonne.AVANCE == null)
                            //{
                            //    Message.ShowInformation("Ce client n'a pas d'avance positionné,Veuillez modifier l'abonnement .", "Acceuil");
                            //    return;
                            //}
                        }
                        this.txtSite.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLESITE) ? string.Empty : laDetailDemande.LeClient.LIBELLESITE;
                        this.txtCentre.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLECENTRE) ? string.Empty : laDetailDemande.LeClient.LIBELLECENTRE;
                        this.txt_Produit.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEPRODUIT) ? string.Empty : laDetailDemande.Abonne.LIBELLEPRODUIT;
                        this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).LIBELLE;
                        txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem);
                        RemplireOngletClient(laDetailDemande.LeClient);
                        RemplirOngletAbonnement(laDetailDemande.Abonne);
                   
                        ChargerCompteDeResiliation(laDetailDemande.LeClient);
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
        private void ChargerCompteDeResiliation(CsClient _UnClient)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerCompteDeResiliationCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null || args.Result.Count == 0)
                {
                    //Message.ShowInformation("Ce client n'existe pas", "Demande");
                    //return;
                    Message.ShowInformation("Ce client n'a pas d'avance positionnée.\nVeuillez vérifier l'abonnement .", "Acceuil");
                    this.OKButton.IsEnabled = false;

                    //    return;
                }
                List<CsLclient> lstFactureDuClient = args.Result;
                if (lstFactureDuClient != null && lstFactureDuClient.Count != 0)
                {
                    lstFactureDuClient.ForEach(t => t.REFEM = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(t.REFEM));

                    AfficherOuMasquer(tabItemCompte, true);
                    dtg_CompteClient.ItemsSource = lstFactureDuClient;
                    //Txt_TotalSoldeResil.Text = lstFactureDuClient.Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
                    Txt_TotalSoldeResil.Text = lstFactureDuClient.First().SOLDEFACTURE.Value.ToString(SessionObject.FormatMontant);

                    //if (lstFactureDuClient.Sum(t => t.SOLDEFACTURE) >= 0)
                    //{
                    //    Message.ShowInformation("Aucun remboursement pour ce client car le solde est supperieur a 0", "Demande");
                    //    this.OKButton.IsEnabled = false;
                    //    return;
                    //}
                    this.OKButton.IsEnabled = true ;
                }
            };
            client.ChargerCompteDeResiliationAsync(_UnClient);
            client.CloseAsync();

        }
    }
}


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

namespace Galatee.Silverlight.Devis
{
    public partial class FrmCompteRenduVerif : ChildWindow
    {
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode { get; set; }
        CsDemandeBase laDemandeSelect = null;
        bool isPreuveSelectionnee = false;
        private UcImageScanne formScanne = null;
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        CsDemande laDetailDemande = null;
        private List<CsCentre> _listeDesCentreExistant = null;

        public FrmCompteRenduVerif()
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            ChargerTypeDocument();
            ChargerListDesSite();
        }
        public FrmCompteRenduVerif(int idDemande)
        {
            InitializeComponent();
            ChargerTypeDocument();
            ChargerListDesSite();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            ChargeDetailDEvis(idDemande);
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
                    }
                };
                client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
            }
            catch (Exception ex)
            {
                 Message.ShowInformation("Erreur au chargement des données", "Erreur");
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
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
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
            return pDemandeDevis;
        }

        private void ValiderInitialisation(CsDemande demandedevis, bool IsTransmetre)
        {

            try
            {
                if (this.Rdb_Etalonnage.IsChecked == false && this.rdb_Changement.IsChecked == false)
                {
                    Message.ShowInformation("Veuillez saisir le resulat de la verification", "Demande");
                    return;
                }
                demandedevis = GetDemandeDevisFromScreen(demandedevis, false);
                if (demandedevis != null)
                {
                    if (IsTransmetre)
                        demandedevis.LaDemande.ETAPEDEMANDE = null;
                    demandedevis.LaDemande.MATRICULE = UserConnecte.matricule;
                    demandedevis.LaDemande.CENTRE = laDetailDemande.LaDemande.CENTRE ;
                    demandedevis.LaDemande.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                    demandedevis.LaDemande.ISCHANGECOMPTEUR = this.rdb_Changement.IsChecked.Value;
                    demandedevis.LaDemande.ISETALONNAGE  = this.Rdb_Etalonnage .IsChecked.Value;

                  
                    //if (this.rdb_Changement.IsChecked == true)
                    //{
                        //demandedevis.Abonne = null;
                        //demandedevis.Branchement = null;
                        //demandedevis.Ag = null;
                        demandedevis.LaDemande.NUMDEM = string.Empty;
                        demandedevis.LaDemande.PK_ID  = 0;
                        AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                        client.ValiderDemandeInitailisationCompleted += (ss, b) =>
                        {
                            this.OKButton.IsEnabled = true;
                            if (b.Cancelled || b.Error != null)
                            {
                                string error = b.Error.Message;
                                Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                                return;
                            }
                            CsTdem letype = new CsTdem();
                            if (this.rdb_Changement.IsChecked == true)
                            letype = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ChangementCompteur);
                            else if (this.Rdb_Etalonnage.IsChecked == true)
                                letype = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.Etalonage );

                            List<string> codes = new List<string>();
                            codes.Add(laDetailDemande.InfoDemande.CODE);
                            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);


                            string Retour = b.Result;
                            string[] coupe = Retour.Split('.');
                            Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], demandedevis.LaDemande.FK_IDCENTRE, coupe[1], letype.PK_ID);
                        };
                        client.ValiderDemandeInitailisationAsync(demandedevis);
                    //}
                    //else
                    //{
                    //    demandedevis.LeClient = null;
                    //    demandedevis.Abonne = null;
                    //    demandedevis.Branchement = null;
                    //    demandedevis.Ag = null;
                    //    AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    //    client.ValiderDemandeInitailisationCompleted += (ss, b) =>
                    //    {
                    //        if (b.Cancelled || b.Error != null)
                    //        {
                    //            string error = b.Error.Message;
                    //            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    //            return;
                    //        }
                    //        List<string> codes = new List<string>();
                    //        codes.Add(laDetailDemande.InfoDemande.CODE);
                    //        Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
                    //        List<CsUtilisateur> leUser = new List<CsUtilisateur>();
                    //        if (laDetailDemande.InfoDemande != null && laDetailDemande.InfoDemande.CODE != null)
                    //        {
                    //            foreach (CsUtilisateur item in laDetailDemande.InfoDemande.UtilisateurEtapeSuivante)
                    //                leUser.Add(item);
                    //            Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", laDetailDemande.LaDemande.NUMDEM, laDetailDemande.LaDemande.LIBELLETYPEDEMANDE);
                    //        }
                    //    };
                    //    client.ValiderDemandeInitailisationAsync(demandedevis);
                    //}
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

        private void cbo_typedoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSite_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void txtCentre_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}


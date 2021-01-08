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
using Galatee.Silverlight.ServiceAccueil   ;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmSuppressionCout : ChildWindow
    {
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        int IdDemandeDevis = 0;
        CsDemandeBase laDemandeSelect = null;
        bool isPreuveSelectionnee = false;
        private UcImageScanne formScanne = null;
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        CsDemande laDetailDemande = null;

        public bool IsForAnalyse { get; set; }

        public FrmSuppressionCout()
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            ChargerTypeDocument();
            this.Txt_NumeroDemande.MaxLength = SessionObject.Enumere.TailleNumeroDemande;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            AfficherOuMasquer(tabItemClient, false);
            AfficherOuMasquer(tabItemAbon, false);
            AfficherOuMasquer(tabItemClient, false);
            AfficherOuMasquer(tabItemCompte, false);
            AfficherOuMasquer(tabPieceJointe, false);

        }

        string leEtatExecuter = string.Empty;
        public FrmSuppressionCout(string typeEtat)
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            ChargerTypeDocument();
            this.Txt_NumeroDemande.MaxLength = SessionObject.Enumere.TailleNumeroDemande;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            AfficherOuMasquer(tabItemClient, false);
            AfficherOuMasquer(tabItemAbon, false);
            AfficherOuMasquer(tabItemClient, false);
            AfficherOuMasquer(tabItemCompte, false);
            AfficherOuMasquer(tabPieceJointe, false);
            leEtatExecuter = typeEtat;

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

                var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Confirmez-vous la mise à jour de la demande ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {


                        if (laDetailDemande != null)
                        {
                            ValiderDemande(laDetailDemande);
                        }
                    }
                    else
                    {
                        return;
                    }
                };
                messageBox.Show();
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
        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_NumeroDemande.Text))
                    RetourneDemandeByNumero(Txt_NumeroDemande.Text);
                else
                {
                    Message.ShowInformation("Saisir le numéro de la demande", "Demande");
                    return;
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }
        CsReglageCompteur ReglageCompt = null;
        private void RetourneDemandeByNumero(string Numerodemande)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible ;
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GetDevisByNumDemandeCompleted += (ssender, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed ;

                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, "Suppression coûts");
                        return;
                    }
                    if (args.Result == null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, "Suppression coûts");
                        return;
                    }
                    else
                    {
                        laDetailDemande = args.Result;
                        laDemandeSelect = laDetailDemande.LaDemande;
                        this.txtCentre.Text = string.IsNullOrEmpty(laDemandeSelect.LIBELLECENTRE) ? string.Empty : laDemandeSelect.LIBELLECENTRE;  
                        this.txtSite.Text = string.IsNullOrEmpty(laDemandeSelect.LIBELLESITE) ? string.Empty : laDemandeSelect.LIBELLESITE; 
                     
                        if (laDemandeSelect.ISSUPPRIME==true )
                        {
                            this.OKButton.IsEnabled = false;
                            Message.ShowInformation("Demande déja supprimée", "Demande");
                            return;
                        }
                        else
                        {
                            if (laDetailDemande.InfoDemande != null && laDetailDemande.InfoDemande.CODEETAPE != "ENCAI")
                            {
                                this.OKButton.IsEnabled = false;
                                Message.ShowInformation("Cette demande n'est pas à la caisse", "Demande");
                                return;
                            }
                            else
                                //this.OKButton.IsEnabled = true;
                                this.OKButton.IsEnabled = !string.IsNullOrEmpty(this.txt_motif.Text);

                            if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                            {
                                ReglageCompt = new CsReglageCompteur();
                                int idreglageCpt = 0;
                                ReglageCompt = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.CODE == laDetailDemande.LaDemande.REGLAGECOMPTEUR);
                                if (ReglageCompt != null)
                                    idreglageCpt = ReglageCompt.PK_ID;
                                ChargerTarifClient(laDetailDemande.LaDemande.FK_IDCENTRE, laDetailDemande.LeClient.FK_IDCATEGORIE.Value, idreglageCpt, null, "0", laDetailDemande.LaDemande.FK_IDPRODUIT.Value);
                            }


                            RemplireOngletClient(laDetailDemande.LeClient);
                            RemplirOngletAbonnement(laDetailDemande.Abonne );
                            RemplireOngletFacture(laDetailDemande.LstCoutDemande);
                            RenseignerInformationsDevis(laDetailDemande);
                            this.tabItemCompte.IsSelected = true;

                        }
                    }
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.GetDevisByNumDemandeAsync(Numerodemande);
            }
            catch (Exception ex)
            {
                Message.Show("Erreur au chargement de la demande", "Demande");
            }
        }
        List<CsTarifClient> lstTarif = new List<CsTarifClient>();
        private void ChargerTarifClient(int idcentre, int idcategorie, int idreglageCompteur, int? idtypecomptage, string propriotaire, int idproduit)
        {
            try
            {
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.RetourneTarifClientCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, "Suppression coûts");
                        return;
                    }
                    if (args.Result == null)
                    {

                    }
                    else
                    {
                        lstTarif = args.Result;
                        lstTarif.ForEach(t => t.REDEVANCE = t.REDEVANCE + " " + t.TRANCHE.ToString());
                    }
                };
                client.RetourneTarifClientAsync(idcentre, idcategorie, idreglageCompteur, idtypecomptage, propriotaire, idproduit);
            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur au chargement des tarif", "Demande");
            }
        }

        private void RenseignerInformationsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                     CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == laDemande.LaDemande.FK_IDCENTRE);

                    Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE.ToString()) ? laDemande.LaDemande.ORDRE.ToString() : string.Empty;
                    Txt_CodeSite.Text = !string.IsNullOrEmpty(leCentre.CODESITE) ? leCentre.CODESITE : string.Empty;
                    Txt_CodeCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CENTRE) ? laDemande.LaDemande.CENTRE : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLECENTRE) ? laDemande.LaDemande.LIBELLECENTRE : string.Empty;
                    Txt_CodeProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.PRODUIT) ? laDemande.LaDemande.PRODUIT : string.Empty;
                    Txt_LibelleSite.Text = !string.IsNullOrEmpty(leCentre.LIBELLESITE) ? leCentre.LIBELLESITE : string.Empty;
                    Txt_LibelleProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLEPRODUIT) ? laDemande.LaDemande.LIBELLEPRODUIT : string.Empty;
                    Txt_LibelleTypeDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLETYPEDEMANDE) ? laDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;

                    this.Txt_EtapeCourante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Title = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Txt_EtapeSuivante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_SUIVANTE) ? laDemande.InfoDemande.ETAPE_SUIVANTE : string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void RemplireOngletClient(CsClient _LeClient)
        {
            try
            {
                if (_LeClient != null)
                {
                    AfficherOuMasquer(tabItemClient, true );

                    this.Txt_NomClient.Text = (string.IsNullOrEmpty(_LeClient.NOMABON) ? string.Empty : _LeClient.NOMABON);
                    //this.Txt_Telephone1.Text = string.IsNullOrEmpty(_LeClient.TELEPHONE) ? string.Empty : _LeClient.TELEPHONE;
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

        private void RemplirOngletAbonnement(CsAbon  _LeAbon)
        {
            if (_LeAbon != null)
            {
                AfficherOuMasquer(tabItemAbon, true);

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

                this.Txt_DateAbonnement.Text = (_LeAbon.DABONNEMENT == null) ?string.Empty  : Convert.ToDateTime(_LeAbon.DABONNEMENT.Value).ToShortDateString();
            }
        }

        private void RemplireOngletFacture(List<CsDemandeDetailCout>  _LesFactClient)
        {
            try
            {
                if (_LesFactClient != null && _LesFactClient.Count != 0)
                {
                    AfficherOuMasquer(tabItemCompte, true);

                    _LesFactClient.ForEach(t => t.MONTANTTTC = t.MONTANTHT + t.MONTANTTAXE);
                    this.LsvFacture.ItemsSource = null;
                    this.LsvFacture.ItemsSource = _LesFactClient;
                    this.Txt_TotalHt.Text = _LesFactClient.Sum(t => t.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
                    this.Txt_totalTaxe.Text = _LesFactClient.Sum(t => t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                    this.Txt_TotalTTC.Text = _LesFactClient.Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void Btn_Supprimer_Click(object sender, RoutedEventArgs e)
        {
            Supprimer();
        }
        private void Supprimer()
        {
            try
            {

                if (this.LsvFacture.SelectedItem != null)
                {
                    var mBoxControl = new MessageBoxControl.MessageBoxChildWindow(this.Title.ToString(), Languages.msgConfirmSuppression, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    mBoxControl.OnMessageBoxClosed += (_, result) =>
                    {
                        if (mBoxControl.Result == MessageBoxResult.OK)
                        {
                            List<CsDemandeDetailCout> EltDeLagrid = LsvFacture.ItemsSource as List<CsDemandeDetailCout>;
                            CsDemandeDetailCout select = LsvFacture.SelectedItem as CsDemandeDetailCout;
                            CsDemandeDetailCout ObjSelect = EltDeLagrid.FirstOrDefault(t => t.COPER == select.COPER);
                            if (select != null)
                            {
                                select.MONTANTHT = 0;
                                select.MONTANTTAXE = 0;
                                select.MONTANTTTC = 0;
                            }
                            this.Txt_TotalHt.Text = EltDeLagrid.Sum(t => t.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
                            this.Txt_totalTaxe.Text = EltDeLagrid.Sum(t => t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                            this.Txt_TotalTTC.Text = EltDeLagrid.Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);

                            laDetailDemande.LaDemande.ISSUPPRIMERCOUT = true;
                        }
                        else
                        {
                            return;
                        }
                    };
                    mBoxControl.Show();
                }
                else
                    throw new Exception("Veuillez sélectionner un élément!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ValiderDemande(CsDemande laDemande)
        {
            if (LsvFacture.ItemsSource != null )
            laDetailDemande.LstCoutDemande = ((List<CsDemandeDetailCout>)this.LsvFacture.ItemsSource).ToList();

            laDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
            laDemande.LaDemande.ANNOTATION = SessionObject.LePosteCourant.NOMPOSTE;
            laDemande.LaDemande.ISPASSERCAISSE = false;
            laDemande.LaDemande.MOTIF = this.txt_motif.Text;

            prgBar.Visibility = System.Windows.Visibility.Visible;
            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ValiderActionSurDemandeCompleted += (ssender, args) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                if (args.Cancelled || args.Error != null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    string error = args.Error.Message;
                    Message.ShowError(error, "Suppression coûts");
                    return;
                }
                if (args.Result == null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, "Suppression coûts");
                    return;
                }
                else
                {
                    if (args.Result == true)
                    {
                        Message.ShowInformation ("Mise à jour effectuée avec succès", "Suppression coûts");
                        this.DialogResult = true;
                        return;
                    }
                    else
                    {
                        //string error = args.Error.Message;
                        //Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        Message.ShowError("Echec lors de la mise à jour de la demande", "Suppression coûts");
                        return;
                    }

                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
            client.ValiderActionSurDemandeAsync(laDemande);
        }

        private void txt_motif_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.OKButton.IsEnabled = !string.IsNullOrEmpty(this.txt_motif.Text);
        }
    }
}


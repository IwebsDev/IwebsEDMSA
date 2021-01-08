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

namespace Galatee.Silverlight.Devis
{
    public partial class UcClotureDemande : ChildWindow
    {
        private ObjDEVIS ObjetDevisSelectionne = null;
        private ObjDEVIS MyDevis = new ObjDEVIS();
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = new CsDemande();

        public bool IsForAnalyse { get; set; }

        public UcClotureDemande()
        {
            InitializeComponent();
        }
        public UcClotureDemande( int idDevis)
        {
            InitializeComponent();
            AfficherOuMasquer(tabItemCompteClient, false);
            AfficherOuMasquer(tabItemDemandeur, false);
            AfficherOuMasquer(tabItemAppareils, false);
            AfficherOuMasquer(tabItemFournitures, false);
            AfficherOuMasquer(tabItemTravaux, false);
            AfficherOuMasquer(tabItemCompteClient, false);
            ChargeDetailDEvis(idDevis);

        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeCompleted  += (ssender, args) =>
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
                    #region DocumentScanne
                    if (laDetailDemande.ObjetScanne != null && laDetailDemande.ObjetScanne.Count != 0)
                    {
                        foreach (var item in laDetailDemande.ObjetScanne)
                        {
                            LstPiece.Add(item);
                            ObjetScanne.Add(item);
                        }
                        dgListePiece.ItemsSource = ObjetScanne;
                    }
                    #endregion
                    laDemandeSelect = laDetailDemande.LaDemande;
                    RenseignerInformationsDevis(laDetailDemande);
                    RenseignerInformationsDemandeDevis(laDetailDemande);
                    RenseignerInformationsAppareilsDevis(laDetailDemande);
                    RenseignerInformationsFournitureDevis(laDetailDemande);
                    if (laDetailDemande.TravauxDevis != null )
                    RenseignerInformationsTravauxDevis(laDetailDemande);

                    ChargerCompteDeResiliation(laDetailDemande.LeClient);
                    LayoutRoot.Cursor = Cursors.Arrow;
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
            client.ChargerDetailDemandeAsync(IdDemandeDevis, string.Empty );
        }
   
        private void ChargerCompteDeResiliation(CsClient _UnClient)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            client.ChargerCompteDeResiliationCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null || args.Result.Count == 0)
                {
                    return;
                }
                List<CsLclient> lstFactureDuClient = args.Result;
                lstFactureDuClient.ForEach(t => t.REFEM =(!string.IsNullOrEmpty(t.REFEM)? ClasseMEthodeGenerique.FormatPeriodeMMAAAA(t.REFEM):string.Empty ));
                if (lstFactureDuClient != null && lstFactureDuClient.Count != 0)
                {
                    AfficherOuMasquer(tabItemCompteClient, true);
                    dtg_CompteClient.ItemsSource = null ;
                    dtg_CompteClient.ItemsSource = lstFactureDuClient;
                    lstFactureDuClient.ForEach(i => i.MONTANTPAYPARTIEL = ((i.MONTANT == null ? 0 : i.MONTANT) - (i.SOLDEFACTURE == null ? 0 : i.SOLDEFACTURE)));
                    Txt_TotalSoldeResil.Text = lstFactureDuClient.Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
                }
            };
            client.ChargerCompteDeResiliationAsync(_UnClient);
            client.CloseAsync();
        
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
        private void RenseignerInformationsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                   ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID  == laDemande.LaDemande.FK_IDCENTRE );

                    Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE.ToString()) ? laDemande.LaDemande.ORDRE.ToString() : string.Empty;
                    Txt_client.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CLIENT) ? laDemande.LaDemande.CLIENT : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLECENTRE) ? laDemande.LaDemande.LIBELLECENTRE : string.Empty;
                    Txt_LibelleSite.Text = !string.IsNullOrEmpty(leCentre.LIBELLESITE) ? leCentre.LIBELLESITE : string.Empty;
                    Txt_LibelleProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLEPRODUIT) ? laDemande.LaDemande.LIBELLEPRODUIT : string.Empty;
                    Txt_LibelleTypeDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLETYPEDEMANDE) ? laDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;
                    txtPropriete.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;

                    this.Txt_EtapeCourante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Txt_EtapeSuivante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_SUIVANTE) ? laDemande.InfoDemande.ETAPE_SUIVANTE : string.Empty; 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsDemandeDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.LeClient != null && laDemande.Ag != null)
                {
                    Txt_NomClient.Text = !string.IsNullOrEmpty(laDemande.LeClient.NOMABON) ? laDemande.LeClient.NOMABON : string.Empty; 
                    txt_Commune.Text = !string.IsNullOrEmpty(laDemande.Ag.COMMUNE) ? laDemande.Ag.COMMUNE : string.Empty;
                    txt_Quartier.Text = !string.IsNullOrEmpty(laDemande.Ag.QUARTIER) ? laDemande.Ag.QUARTIER : string.Empty;
                    txt_NumRue.Text = !string.IsNullOrEmpty(laDemande.Ag.RUE) ? laDemande.Ag.RUE : string.Empty;
                    txtAdresse.Text =(laDemande.Branchement != null && !string.IsNullOrEmpty(laDemande.Branchement.ADRESSERESEAU)) ? laDemande.Branchement.ADRESSERESEAU : string.Empty;
                    this.TxtPoteau.Text = (laDemande.Branchement != null && laDemande.Branchement.NBPOINT != null) ? laDemande.Branchement.NBPOINT.ToString() : string.Empty;
                    txtNumeroPiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMEROPIECEIDENTITE) ? laDemande.LeClient.NUMEROPIECEIDENTITE : string.Empty;
                    txt_Telephone.Text = !string.IsNullOrEmpty(laDemande.LeClient.TELEPHONE) ? laDemande.LeClient.TELEPHONE : string.Empty;
                    txt_NumLot.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;
                    Txt_LibelleCommune.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLECOMMUNE) ? laDemande.Ag.LIBELLECOMMUNE : string.Empty;
                    Txt_LibelleQuartier.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLEQUARTIER) ? laDemande.Ag.LIBELLEQUARTIER : string.Empty;
                    Txt_LibelleRue.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLERUE) ? laDemande.Ag.LIBELLERUE : string.Empty;
                    Txt_LibelleDiametre.Text = (laDemande.Branchement != null && !string.IsNullOrEmpty(laDemande.Branchement.LIBELLETYPEBRANCHEMENT)) ? laDemande.Branchement.LIBELLETYPEBRANCHEMENT : string.Empty;
                    Txt_LibelleCategorie.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLECATEGORIE) ? laDemande.LeClient.LIBELLECATEGORIE : string.Empty;
                    Txt_TypePiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLETYPEPIECE) ? laDemande.LeClient.LIBELLETYPEPIECE : string.Empty;
                    Txt_LibelleTournee.Text = !string.IsNullOrEmpty(laDemande.Ag.TOURNEE) ? laDemande.Ag.TOURNEE : string.Empty;

                    TxtLongitude.Text = (laDemande.Branchement != null && !string.IsNullOrEmpty(laDemande.Branchement.LONGITUDE)) ? laDemande.Branchement.LONGITUDE : string.Empty;
                    TxtLatitude.Text =(laDemande.Branchement != null &&  !string.IsNullOrEmpty(laDemande.Branchement.LATITUDE)) ? laDemande.Branchement.LATITUDE : string.Empty;

            
                    AfficherOuMasquer(tabItemDemandeur, true);
                }
                else
                    AfficherOuMasquer(tabItemDemandeur, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsAppareilsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.AppareilDevis != null && laDemande.AppareilDevis.Count > 0)
                {
                    dtgAppareils.ItemsSource = laDemande.AppareilDevis;
                    AfficherOuMasquer(tabItemAppareils, true);
                }
                else
                    AfficherOuMasquer(tabItemAppareils, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void RenseignerInformationsFournitureDevis(CsDemande lademande)
        {
            try
            {
                if (lademande != null && (lademande.EltDevis != null && lademande.EltDevis.Count > 0))
                {
                    dataGridForniture.ItemsSource = lademande.EltDevis;
                    this.Txt_TotalHt.Text = lademande.EltDevis.Sum(t => t.MONTANTHT).Value .ToString(SessionObject.FormatMontant);
                    this.Txt_TotalTva.Text = lademande.EltDevis.Sum(t => t.MONTANTTAXE ).Value .ToString(SessionObject.FormatMontant);
                    this.Txt_TotalTtc.Text = lademande.EltDevis.Sum(t => t.MONTANTTTC).Value .ToString(SessionObject.FormatMontant);
                    AfficherOuMasquer(tabItemFournitures, true);
                }
                else
                    AfficherOuMasquer(tabItemFournitures, false);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsTravauxDevis(CsDemande lademande)
        {
            try
            {
                if (lademande != null && lademande.LstCanalistion != null && lademande.LstCanalistion.Count != 0)
                {
                        if (lademande.TravauxDevis != null)
                        {
                            this.DtpDebutTravaux.SelectedDate = lademande.TravauxDevis.DATEDEBUTTRVX;
                            this.DtpFinTravaux.SelectedDate = lademande.TravauxDevis.DATEFINTRVX;
                            this.TxtProcesVerbal.Text = !string.IsNullOrEmpty(lademande.TravauxDevis.PROCESVERBAL) ? lademande.TravauxDevis.PROCESVERBAL : string.Empty;
                            TxtChefEquipe.Text = !string.IsNullOrEmpty(lademande.TravauxDevis.NOMCHEFEQUIPE) ? lademande.TravauxDevis.NOMCHEFEQUIPE : string.Empty;
                        }
                        TxtNumeroDevis.Text =!string.IsNullOrEmpty(lademande.LaDemande.NUMDEM) ? lademande.LaDemande.NUMDEM: string.Empty;  
                        TxtReferenceClient.Text = !string.IsNullOrEmpty(lademande.LaDemande.CLIENT ) ? lademande.LaDemande.CLIENT : string.Empty;
                        TxtCategorieClient.Text = !string.IsNullOrEmpty(lademande.LeClient.LIBELLECATEGORIE) ? lademande.LeClient.LIBELLECATEGORIE : string.Empty;
                        lademande.LstEvenement.ForEach(t => t.LIBELLECASPRECEDENT = laDetailDemande.LstCanalistion.First().LIBELLEMARQUE);
                        dg_compteur.ItemsSource = lademande.LstEvenement ;

                        AfficherOuMasquer(tabItemTravaux, true);
                    }
                    else
                    AfficherOuMasquer(tabItemTravaux, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        private UcImageScanne formScanne = null;
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        private byte[] image, imageFraix;
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
        private List<ObjDOCUMENTSCANNE> ObjetScanne = new List<ObjDOCUMENTSCANNE>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();

        private void GetInformationFromChildWindowImagePreuve(object sender, EventArgs e)
        {
            this.LstPiece.Add(new ObjDOCUMENTSCANNE { PK_ID = Guid.NewGuid(), NOMDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).LIBELLE, FK_IDTYPEDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).PK_ID, CONTENU = image, DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule });
            this.dgListePiece.ItemsSource = this.LstPiece;
            ObjetScanne = this.LstPiece.ToList();
        }
        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
        }
        private void ChargerTypeDocument()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                    ObjetScanne = this.LstPiece.ToList();

                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }

        private void btn_Transmetre_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                clientDevis.ClotureValiderDemandeCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (b.Result == true)
                    {
                       Message.ShowInformation("Demande cloturée avec succès", "Demande");
                       this.DialogResult = false;
                    }
                    else
                    {
                        Message.ShowError("Erreur à la cloture de la demande", Silverlight.Resources.Devis.Languages.txtDevis);
                         
                    }
                };
                clientDevis.ClotureValiderDemandeAsync(laDetailDemande);
                 
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void btn_Rejeter_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);

        }

        private void btn_Imprimer_Click(object sender, RoutedEventArgs e)
        {
            if (this.dtg_CompteClient.ItemsSource != null )
            {
                List<CsLclient> lesFacture = (List<CsLclient>)this.dtg_CompteClient.ItemsSource;
                Utility.ActionDirectOrientation<ServicePrintings.CsLclient, CsLclient>(lesFacture, null, SessionObject.CheminImpression, "CompteResiliationClient", "Accueil", true);
 
            }
        }
    }
}


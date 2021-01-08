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
    public partial class UcSuiveTravaux : ChildWindow
    {
        private ObjDEVIS ObjetDevisSelectionne = null;
        private ObjDEVIS MyDevis = new ObjDEVIS();
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        private CsDemande laDetailDemande = new CsDemande();
        public bool IsForAnalyse { get; set; }
        Galatee.Silverlight.Shared.UcFichierJoint ctrl = null;

        public UcSuiveTravaux()
        {
            InitializeComponent();
        }
        public UcSuiveTravaux( int idDevis)
        {
            try
            {
                InitializeComponent();
                ChargeDetailDEvis(idDevis);
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }
        public UcSuiveTravaux(ObjDEVIS pDevis)
        {
            InitializeComponent();
            ObjetDevisSelectionne = pDevis;
            AfficherOuMasquer(tabItemDemandeur, false);
            AfficherOuMasquer(tabItemAppareils, false);
            AfficherOuMasquer(tabItemFournitures, false);
            AfficherOuMasquer(tabItemOT, false);

        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            try
            {
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.ChargerDetailDemandeCompleted += (ssender, args) =>
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
                        RenseignerInformationsDocumentScanne();
                        RenseignerInformationsDevis(laDetailDemande);
                        RenseignerInformationsDemandeDevis(laDetailDemande);
                        RenseignerInformationsAppareilsDevis(laDetailDemande);
                        RenseignerInformationsFournitureDevis(laDetailDemande);
                        RenseignerInformationsOrdreTravail(laDetailDemande);
                        //RenseignerInformationsControleTrx(laDetailDemande);
                        this.tabControl_Consultation.SelectedIndex = 5;
                        LayoutRoot.Cursor = Cursors.Arrow;
                    }
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.ChargerDetailDemandeAsync(IdDemandeDevis, string.Empty);
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
                this.btn_transmetre.IsEnabled = false ;
                this.RejeterButton.IsEnabled = false;
                this.CancelButton.IsEnabled = false;
                this.OKButton.IsEnabled = false;
                EnregisterOuTransmettre(false);    
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
        void EnregisterOuTransmettre(bool IsTransmetre)
        {
            try
            {
                if (!VerifieChampObligatoire())
                    return;

                laDetailDemande.OrdreTravail = null;
                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.ValiderSuivieTravauxCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (string.IsNullOrEmpty(b.Result))
                    {
                        if (IsTransmetre)
                            Message.ShowInformation("Demande transmise avec succes", "Demande");
                        else
                            Message.ShowInformation("Mise a jour éffectué avec succes", "Demande");
                        this.DialogResult = false;
                    }
                    else
                        Message.ShowError(b.Result, "Demande");
                };
                clientDevis.ValiderSuivieTravauxAsync(laDetailDemande, true);
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message ,"Demande");
            }
        }
        bool VerifieChampObligatoire()
        {
            if (string.IsNullOrEmpty(Txt_CommentaireTrx.Text))
            {
                Message.ShowInformation("Saisir faire une observation","VerifieChampObligatoir");
                this.btn_transmetre.IsEnabled = true;
                this.RejeterButton.IsEnabled = true;
                this.CancelButton.IsEnabled = true;
                this.OKButton.IsEnabled = true;
                return false;
            }
          
            return true;
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
        private void RenseignerInformationsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                   ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID  == laDemande.LaDemande.FK_IDCENTRE );

                    Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE.ToString()) ? laDemande.LaDemande.ORDRE.ToString() : string.Empty;
                    Txt_CodeSite.Text = !string.IsNullOrEmpty(leCentre.CODESITE) ? leCentre.CODESITE  : string.Empty;
                    Txt_CodeCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CENTRE) ? laDemande.LaDemande.CENTRE : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLECENTRE) ? laDemande.LaDemande.LIBELLECENTRE : string.Empty;
                    Txt_CodeProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.PRODUIT) ? laDemande.LaDemande.PRODUIT : string.Empty;
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
                    txtAdresse.Text = !string.IsNullOrEmpty(laDemande.Branchement.ADRESSERESEAU) ? laDemande.Branchement.ADRESSERESEAU : string.Empty;
                    txtNumeroPiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMEROPIECEIDENTITE) ? laDemande.LeClient.NUMEROPIECEIDENTITE : string.Empty;
                    txt_Telephone.Text = !string.IsNullOrEmpty(laDemande.LeClient.TELEPHONE) ? laDemande.LeClient.TELEPHONE : string.Empty;
                    txt_NumLot.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;
                    Txt_LibelleCommune.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLECOMMUNE) ? laDemande.Ag.LIBELLECOMMUNE : string.Empty;
                    Txt_LibelleQuartier.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLEQUARTIER) ? laDemande.Ag.LIBELLEQUARTIER : string.Empty;
                    Txt_LibelleRue.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLERUE) ? laDemande.Ag.LIBELLERUE : string.Empty;
                    Txt_LibelleDiametre.Text = !string.IsNullOrEmpty(laDemande.Branchement.LIBELLETYPEBRANCHEMENT) ? laDemande.Branchement.LIBELLETYPEBRANCHEMENT : string.Empty;
                    Txt_LibelleCategorie.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLECATEGORIE) ? laDemande.LeClient.LIBELLECATEGORIE : string.Empty;
                    Txt_TypePiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLETYPEPIECE) ? laDemande.LeClient.LIBELLETYPEPIECE : string.Empty;
                    Txt_LibelleTournee.Text = !string.IsNullOrEmpty(laDemande.Ag.TOURNEE) ? laDemande.Ag.TOURNEE : string.Empty;

                    TxtLongitude.Text = !string.IsNullOrEmpty(laDemande.Branchement.LONGITUDE) ? laDemande.Branchement.LONGITUDE : string.Empty;
                    TxtLatitude.Text = !string.IsNullOrEmpty(laDemande.Branchement.LATITUDE) ? laDemande.Branchement.LATITUDE : string.Empty;

            
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

        private void RemplirListeMateriel(List<ObjELEMENTDEVIS> lstEltDevis)
        {
            List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();
            if (lstEltDevis.Count != 0)
            {
                List<ObjELEMENTDEVIS> lstFourExtension = new List<ObjELEMENTDEVIS>();
                List<ObjELEMENTDEVIS> lstFourBranchement = new List<ObjELEMENTDEVIS>();

                lstFourExtension = lstEltDevis.Where(t => t.ISEXTENSION == true).ToList();
                lstFourBranchement = lstEltDevis.Where(t => t.ISEXTENSION == false).ToList();
                lstFourExtension.ForEach(t => t.IsCOLORIE = false);
                lstFourBranchement.ForEach(t => t.IsCOLORIE = false);

                ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
                leSeparateur.DESIGNATION = "----------------------------------";


                if (lstFourBranchement.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                    leResultatBranchanchement.DESIGNATION = "TOTAL BRANCHEMENT ";
                    leResultatBranchanchement.IsCOLORIE = true;
                    leResultatBranchanchement.MONTANTHT = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTHT);
                    leResultatBranchanchement.MONTANTTAXE = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTTAXE);
                    leResultatBranchanchement.MONTANTTTC = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTTTC);

                    lstFourgenerale.AddRange(lstFourBranchement);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatBranchanchement);
                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        DESIGNATION = "    "
                    });

                }
                if (lstFourExtension.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatExtension = new ObjELEMENTDEVIS();
                    leResultatExtension.DESIGNATION = "TOTAL EXTENSION ";
                    leResultatExtension.IsCOLORIE = true;
                    leResultatExtension.MONTANTHT = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTHT);
                    leResultatExtension.MONTANTTAXE = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTTAXE);
                    leResultatExtension.MONTANTTTC = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTTTC);

                    lstFourgenerale.AddRange(lstFourExtension);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatExtension);

                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        DESIGNATION = "    "
                    });
                }
                if (lstFourBranchement.Count != 0 || lstFourExtension.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
                    leResultatGeneral.DESIGNATION = "TOTAL GENERAL ";
                    leResultatGeneral.IsCOLORIE = true;
                    leResultatGeneral.MONTANTHT = lstEltDevis.Sum(t => t.MONTANTHT);
                    leResultatGeneral.MONTANTTAXE = lstEltDevis.Sum(t => t.MONTANTTAXE);
                    leResultatGeneral.MONTANTTTC = lstEltDevis.Sum(t => t.MONTANTTTC);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatGeneral);
                }
            }
            this.dataGridForniture.ItemsSource = null;
            this.dataGridForniture.ItemsSource = lstFourgenerale;
            if (dataGridForniture.ItemsSource != null)
            {
                Txt_TotalTtc.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                Txt_TotalTva.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                Txt_TotalHt.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
            }
        }
        private void RemplirListeMaterielMT(List<ObjELEMENTDEVIS> lstEltDevis)
        {
            List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();
            if (lstEltDevis.Count != 0)
            {
                List<ObjELEMENTDEVIS> lstFourHTA = new List<ObjELEMENTDEVIS>();
                List<ObjELEMENTDEVIS> lstFourBTA = new List<ObjELEMENTDEVIS>();
                List<ObjELEMENTDEVIS> lstComptage = new List<ObjELEMENTDEVIS>();

                lstFourHTA = lstEltDevis.Where(t => t.RUBRIQUE == "HTA").ToList();
                lstFourBTA = lstEltDevis.Where(t => t.RUBRIQUE == "BTA").ToList();
                lstComptage = lstEltDevis.Where(t => t.RUBRIQUE == "COM").ToList();

                ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
                leSeparateur.DESIGNATION = "----------------------------------";
                leSeparateur.ISDEFAULT = true;


                if (lstFourHTA.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                    leResultatBranchanchement.DESIGNATION = "Sous Total Ligne HTA";
                    leResultatBranchanchement.IsCOLORIE = true;
                    leResultatBranchanchement.ISDEFAULT = true;
                    leResultatBranchanchement.MONTANTHT = lstEltDevis.Where(t => t.RUBRIQUE == "HTA").Sum(t => t.MONTANTHT);
                    leResultatBranchanchement.MONTANTTAXE = lstEltDevis.Where(t => t.RUBRIQUE == "HTA").Sum(t => t.MONTANTTAXE);
                    leResultatBranchanchement.MONTANTTTC = lstEltDevis.Where(t => t.RUBRIQUE == "HTA").Sum(t => t.MONTANTTTC);

                    lstFourgenerale.AddRange(lstFourHTA);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatBranchanchement);
                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        DESIGNATION = "    "
                    });

                }
                if (lstFourBTA.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatExtension = new ObjELEMENTDEVIS();
                    leResultatExtension.DESIGNATION = "Sous Total Ligne HTA/BT";
                    leResultatExtension.IsCOLORIE = true;
                    leResultatExtension.ISDEFAULT = true;
                    leResultatExtension.MONTANTHT = lstEltDevis.Where(t => t.RUBRIQUE == "BTA").Sum(t => t.MONTANTHT);
                    leResultatExtension.MONTANTTAXE = lstEltDevis.Where(t => t.RUBRIQUE == "BTA").Sum(t => t.MONTANTTAXE);
                    leResultatExtension.MONTANTTTC = lstEltDevis.Where(t => t.RUBRIQUE == "BTA").Sum(t => t.MONTANTTTC);

                    lstFourgenerale.AddRange(lstFourBTA);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatExtension);

                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        DESIGNATION = "    "
                    });
                }

                if (lstFourBTA.Count != 0 || lstFourHTA.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
                    leResultatGeneral.DESIGNATION = "TOTAL GENERAL ";
                    leResultatGeneral.IsCOLORIE = true;
                    leResultatGeneral.ISDEFAULT = true;
                    leResultatGeneral.MONTANTHT = lstEltDevis.Sum(t => t.MONTANTHT);
                    leResultatGeneral.MONTANTTAXE = lstEltDevis.Sum(t => t.MONTANTTAXE);
                    leResultatGeneral.MONTANTTTC = lstEltDevis.Sum(t => t.MONTANTTTC);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatGeneral);
                }
            }
            this.dataGridForniture.ItemsSource = null;
            this.dataGridForniture.ItemsSource = lstFourgenerale.ToList();
            if (dataGridForniture.ItemsSource != null)
            {
                Txt_TotalTtc.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                Txt_TotalTva.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                Txt_TotalHt.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
            }
        }
        private void dgMyDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dmdRow = e.Row.DataContext as ObjELEMENTDEVIS;
            if (dmdRow != null)
            {
                if (dmdRow.IsCOLORIE)
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Green);
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Bold;
                }
            }
        }
        private void RenseignerInformationsFournitureDevis(CsDemande lademande)
        {
            try
            {
                if (lademande != null && (lademande.EltDevis != null && lademande.EltDevis.Count > 0))
                {
                    if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        RemplirListeMateriel(lademande.EltDevis);
                    else
                        RemplirListeMaterielMT(lademande.EltDevis);
                    AfficherOuMasquer(tabItemFournitures, true);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur au chargement des fournitures", "Validation devis");
            }
        }
        private void RenseignerInformationsOrdreTravail(CsDemande lademande)
        {
            try
            {
                this.TxtNumeroDevis.Text = lademande.LaDemande.NUMDEM ;
                this.TxtNumeroDevisTrx.Text = lademande.LaDemande.NUMDEM;
                if (lademande != null && lademande.OrdreTravail  != null )
                {
                    this.TxtNomAgent.Text = !string.IsNullOrEmpty(lademande.OrdreTravail.LIBELLEAGENT) ? lademande.OrdreTravail.LIBELLEAGENT : string.Empty;
                    this.TxtMatricule.Text = !string.IsNullOrEmpty(lademande.OrdreTravail.MATRICULE) ? lademande.OrdreTravail.MATRICULE : string.Empty;
                    this.Txt_Prestataire.Text = !string.IsNullOrEmpty(lademande.OrdreTravail.PRESTATAIRE) ? lademande.OrdreTravail.PRESTATAIRE : string.Empty;
                    this.TxtMatricule.Tag = lademande.OrdreTravail.FK_IDADMUTILISATEUR ;
                    if (lademande.OrdreTravail .DATEDEBUTTRAVAUX != null )
                    this.DtpDebutTravaux.Text =   lademande.OrdreTravail.DATEDEBUTTRAVAUX  .Value.ToShortDateString();
                    this.Txt_Commentaire.Text = !string.IsNullOrEmpty(lademande.OrdreTravail.COMMENTAIRE) ? lademande.OrdreTravail.COMMENTAIRE : string.Empty;

                    if (lademande.OrdreTravail.DATEFINTRAVAUX != null)
                    this.DtpFinTravaux.Text =   lademande.OrdreTravail.DATEFINTRAVAUX .Value.ToShortDateString();
                    AfficherOuMasquer(tabItemOT, true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Search()
        {
            try
            {
                UserAgentPicker FormUserAgentPicker = new UserAgentPicker();
                FormUserAgentPicker.Closed += new EventHandler(FormUserAgentPicker_Closed);
                FormUserAgentPicker.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void FormUserAgentPicker_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (UserAgentPicker)sender;
                if (form != null)
                {
                    if (form.DialogResult == true && form.AgentSelectionne != null)
                    {
                        var agent = form.AgentSelectionne;
                        if (agent != null)
                        {
                            this.TxtMatricule.Text = agent.MATRICULE;
                            this.TxtNomAgent.Text = agent.LIBELLE;
                            this.TxtMatricule.Tag = agent.PK_ID;
                        }
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void btn_transmetre_Click(object sender, RoutedEventArgs e)
        {
            this.btn_transmetre.IsEnabled = false;
            this.RejeterButton.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            this.OKButton.IsEnabled = false;
            EnregisterOuTransmettre(true);
        }

        private void RejeterButton_Click(object sender, RoutedEventArgs e)
        {
            this.btn_transmetre.IsEnabled = false;
            this.RejeterButton.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            this.OKButton.IsEnabled = false;
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
            Thread.Sleep(5000);
            this.btn_transmetre.IsEnabled = true;
            this.RejeterButton.IsEnabled = true;
            this.CancelButton.IsEnabled = true;
            this.OKButton.IsEnabled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

        }

        private void Editer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EditionOR(laDetailDemande);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void EditionOR(CsDemande laDemande)
        {
            try
            {
                if (laDemande.EltDevis != null && laDetailDemande.EltDevis.Where(u=>u.ISEXTENSION).ToList().Count != 0)
                {
                    laDetailDemande.EltDevis.Where(u => u.ISEXTENSION).ToList().ForEach(t => t.CLIENT = laDetailDemande.LeClient.NOMABON + " (" + laDetailDemande.LeClient.CENTRE + " " + laDetailDemande.LeClient.REFCLIENT + " " + laDetailDemande.LeClient.ORDRE + ")");
                    laDetailDemande.EltDevis.Where(u => u.ISEXTENSION).ToList().ForEach(t => t.LIBELLE = laDetailDemande.Ag.LIBELLEQUARTIER);
                    laDetailDemande.EltDevis.Where(u => u.ISEXTENSION).ToList().ForEach(t => t.LIBELLETYPEDEMANDE = laDetailDemande.Ag.LIBELLEQUARTIER);
                    laDetailDemande.EltDevis.Where(u => u.ISEXTENSION).ToList().ForEach(t => t.COMMUNE = laDetailDemande.Ag.LIBELLECOMMUNE);
                    laDetailDemande.EltDevis.Where(u => u.ISEXTENSION).ToList().ForEach(t => t.NUMDEVIS = laDetailDemande.LeClient.ADRMAND1);
                    laDetailDemande.EltDevis.Where(u => u.ISEXTENSION).ToList().ForEach(t => t.RUE = laDetailDemande.Ag.RUE);
                    laDetailDemande.EltDevis.Where(u => u.ISEXTENSION).ToList().ForEach(t => t.PORTE = laDetailDemande.Ag.PORTE);
                    laDetailDemande.EltDevis.Where(u => u.ISEXTENSION).ToList().ForEach(t => t.TELEPHONE = TxtNomAgent.Text);
                    laDetailDemande.EltDevis.Where(u => u.ISEXTENSION).ToList().ForEach(t => t.UTILISE = Txt_Prestataire.Text);
                    Utility.ActionDirectOrientation<ServicePrintings.ObjELEMENTDEVIS, ObjELEMENTDEVIS>(laDetailDemande.EltDevis.Where(u => u.ISEXTENSION).ToList(), null, SessionObject.CheminImpression, "OrdreDeTravail", "Devis", true);
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}


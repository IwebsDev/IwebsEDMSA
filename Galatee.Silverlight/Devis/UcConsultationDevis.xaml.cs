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
    public partial class UcConsultationDevis : ChildWindow
    {
        private ObjDEVIS ObjetDevisSelectionne = null;
        private ObjDEVIS MyDevis = new ObjDEVIS();
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = new CsDemande();

        public bool IsForAnalyse { get; set; }

        public UcConsultationDevis()
        {
            InitializeComponent();
        }
        public UcConsultationDevis( int idDevis)
        {
            InitializeComponent();
            ChargeDetailDEvis(idDevis);
        }
        public UcConsultationDevis(ObjDEVIS pDevis)
        {
            InitializeComponent();
            ObjetDevisSelectionne = pDevis;
            AfficherOuMasquer(tabItemDemandeur, false);
            AfficherOuMasquer(tabItemAppareils, false);
            AfficherOuMasquer(tabItemFournitures, false);
            AfficherOuMasquer(tabItemProgramme, false);
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeConsultationCompleted += (ssender, args) =>
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
                    RenseignerInformationsProgrammeDevis(laDetailDemande);
                    LayoutRoot.Cursor = Cursors.Arrow;
                }
                LayoutRoot.Cursor = Cursors.Arrow;



            };
            client.ChargerDetailDemandeConsultationAsync(IdDemandeDevis);
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                        //int idTrv = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;

                        //CsDemandeDetailCout leCoutduDevis = new CsDemandeDetailCout();
                        //leCoutduDevis.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                        //leCoutduDevis.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                        //leCoutduDevis.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                        //leCoutduDevis.NUMDEM = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                        //leCoutduDevis.COPER = SessionObject.Enumere.CoperTRV;
                        //leCoutduDevis.NATURE = SessionObject.Enumere.NatureSCF;
                        //leCoutduDevis.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
                        //leCoutduDevis.FK_IDTAXE = laDetailDemande.EltDevis.First().FK_IDTAXE.Value  ;
                        //leCoutduDevis.FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID;
                        //leCoutduDevis.MONTANTHT =(decimal?)Math.Ceiling((double)laDetailDemande.EltDevis.Where(t => t.FK_IDCOPER == idTrv).Sum(h => h.MONTANT));
                        //leCoutduDevis.MONTANTTAXE = (decimal?)Math.Ceiling((double)laDetailDemande.EltDevis.Where(t => t.FK_IDCOPER == idTrv).Sum(h => h.TAXE ));
                        //leCoutduDevis.REFEM = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00");
                        //leCoutduDevis.DATECREATION = DateTime.Now;
                        //leCoutduDevis.USERCREATION = UserConnecte.matricule;
                        //if (laDetailDemande.LstCoutDemande == null)
                        //{
                        //    laDetailDemande.LstCoutDemande = new List<CsDemandeDetailCout>();
                        //    laDetailDemande.LstCoutDemande.Add(leCoutduDevis);
                        //}
                        //else
                        //    laDetailDemande.LstCoutDemande.Add(leCoutduDevis);

                        //foreach (Galatee.Silverlight.ServiceDevis.ObjELEMENTDEVIS item in laDetailDemande.EltDevis.Where(t => t.FK_IDCOPER != idTrv))
                        //{
                        //        leCoutduDevis = new CsDemandeDetailCout();
                        //        leCoutduDevis.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                        //        leCoutduDevis.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                        //        leCoutduDevis.ORDRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.ORDRE) ? null : laDetailDemande.LaDemande.ORDRE;
                        //        leCoutduDevis.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                        //        leCoutduDevis.NUMDEM = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                        //        leCoutduDevis.COPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.PK_ID == item.FK_IDCOPER).CODE;
                        //        leCoutduDevis.NATURE = SessionObject.Enumere.NatureBIL ;
                        //        leCoutduDevis.FK_IDCOPER = item.FK_IDCOPER.Value;
                        //        leCoutduDevis.FK_IDTAXE = item.FK_IDTAXE .Value ;
                        //        leCoutduDevis.FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID;
                        //        leCoutduDevis.MONTANTHT = (decimal?)Math.Ceiling((double)item.MONTANT);
                        //        leCoutduDevis.MONTANTTAXE = (decimal?)Math.Ceiling((double)item.TAXE);
                        //        leCoutduDevis.REFEM = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00");
                        //        leCoutduDevis.DATECREATION = DateTime.Now;
                        //        leCoutduDevis.USERCREATION = UserConnecte.matricule;
                        //        if (laDetailDemande.LstCoutDemande == null)
                        //        {
                        //            laDetailDemande.LstCoutDemande = new List<CsDemandeDetailCout>();
                        //            laDetailDemande.LstCoutDemande.Add(leCoutduDevis);
                        //        }
                        //        else
                        //            laDetailDemande.LstCoutDemande.Add(leCoutduDevis);
                        //}
                      

                        //laDetailDemande.EltDevis = null;
                        //DevisServiceClient clientDevis = new AccesServiceWCF().GetDevisClient();
                        //clientDevis.ValiderDemandeCompleted += (ss, b) =>
                        //{
                        //    if (b.Cancelled || b.Error != null)
                        //    {
                        //        string error = b.Error.Message;
                        //        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        //        return;
                        //    }
                        //    this.DialogResult = false;
                        //};
                        //clientDevis.ValiderDemandeAsync(laDetailDemande);
                   
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
        private void Btn_Etape_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ObjetDevisSelectionne != null)
                {
                    var form = new UcSuiviDevis(ObjetDevisSelectionne);
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }


        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int resultLoding = 0;
            
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

        //private void RenseignerInformationsAccount(CsInformationsDevis pInformationsDevis)
        //{
        //    try
        //    {
        //        if (pInformationsDevis != null && pInformationsDevis.Deposit != null)
        //        {
        //            Txt_ReceiptNumber.Text = !string.IsNullOrEmpty(pInformationsDevis.Deposit.RECEIPT) ? pInformationsDevis.Deposit.RECEIPT : string.Empty;
        //            txtAmountOfDeposit.Text = !string.IsNullOrEmpty(pInformationsDevis.Deposit.TOTAL.ToString()) ? pInformationsDevis.Deposit.TOTAL.ToString() : string.Empty;
        //            Txt_DateOfDeposit.Text = !string.IsNullOrEmpty(pInformationsDevis.Deposit.DATEENC.ToString()) ? pInformationsDevis.Deposit.DATEENC.ToShortDateString() : string.Empty;
        //            Txt_Nom.Text = !string.IsNullOrEmpty(pInformationsDevis.Deposit.NOM) ? pInformationsDevis.Deposit.NOM : string.Empty;
        //            if (pInformationsDevis.Deposit.IDLETTER != Guid.Empty)
        //            {
        //                if (pInformationsDevis.DocumentAutorisation != null)
        //                {
        //                    this.LienAccountScanne.Tag = pInformationsDevis.DocumentAutorisation.CONTENU;
        //                    LienAccountScanne.IsEnabled = true;
        //                    LienAccountScanne.Content = "Autorisation scannée";
        //                }
        //            }
        //            else
        //            {
        //                LienAccountScanne.IsEnabled = false;
        //                LienAccountScanne.Content = "Aucunne autorisation joint";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void RenseignerInformationsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {

                    Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE.ToString()) ? laDemande.LaDemande.ORDRE.ToString() : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLECENTRE) ? laDemande.LaDemande.LIBELLECENTRE : string.Empty;
                    Txt_LibelleSite.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLESITE) ? laDemande.LaDemande.LIBELLESITE : string.Empty;
                    Txt_LibelleProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLEPRODUIT) ? laDemande.LaDemande.LIBELLEPRODUIT : string.Empty;
                    Txt_LibelleTypeDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLETYPEDEMANDE) ? laDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;
                    txtPropriete.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;
                    Txt_Client.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CLIENT) ? laDemande.LaDemande.CLIENT : string.Empty;
                    if (laDemande.InfoDemande != null)
                    {
                        this.Txt_EtapeCourante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                        this.Txt_EtapeSuivante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_SUIVANTE) ? laDemande.InfoDemande.ETAPE_SUIVANTE : string.Empty;
                        this.Txt_DatePaiement.Text = laDemande.LaDemande.DCAISSE != null ? laDemande.LaDemande.DCAISSE.Value.ToShortDateString() : string.Empty;
                        this.Txt_Statdemande.Text  = string.Empty;

                        if (laDemande.InfoDemande.FK_IDSTATUS > 0)
                        {
                            string statut = string.Empty;
                            if (laDemande.InfoDemande.FK_IDSTATUS == (int)SessionObject.StatutDemande.Initiée)
                                statut = "Demande initiée";
                            if (laDemande.InfoDemande.FK_IDSTATUS == (int)SessionObject.StatutDemande.En_cours)
                                statut = "Demande en cours de traitement";
                            if (laDemande.InfoDemande.FK_IDSTATUS == (int)SessionObject.StatutDemande.Suspendue)
                                statut = "Demande suspendue";
                            if (laDemande.InfoDemande.FK_IDSTATUS == (int)SessionObject.StatutDemande.Annulée)
                                statut = "Demande annulée le " + laDemande.LaDemande.DATESUPPRESSION.Value.ToShortDateString() + " par le matricule " + laDemande.LaDemande.USERSUPPRESSION;
                            if (laDemande.InfoDemande.FK_IDSTATUS == (int)SessionObject.StatutDemande.Rejetée)
                                statut = "Demande rejetée";
                            if (laDemande.InfoDemande.FK_IDSTATUS == (int)SessionObject.StatutDemande.Terminée)
                                statut = "Demande finalisée";

                            this.Txt_Statdemande.Text = statut;
                        }
                    }
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
                    txt_Matricule.Text = !string.IsNullOrEmpty(laDemande.LeClient.MATRICULE) ? laDemande.LeClient.MATRICULE : string.Empty; 
                    txt_Commune.Text = !string.IsNullOrEmpty(laDemande.Ag.COMMUNE) ? laDemande.Ag.COMMUNE : string.Empty;
                    //txt_Quartier.Text = !string.IsNullOrEmpty(laDemande.Ag.QUARTIER) ? laDemande.Ag.QUARTIER : string.Empty;
                    //txt_NumRue.Text = !string.IsNullOrEmpty(laDemande.Ag.RUE) ? laDemande.Ag.RUE : string.Empty;
                    txtAdresse.Text =laDemande.Branchement != null && !string.IsNullOrEmpty(laDemande.Branchement.ADRESSERESEAU) ? laDemande.Branchement.ADRESSERESEAU : string.Empty;
                    //this.TxtPoteau.Text = laDemande.Branchement.NBPOINT != null ? laDemande.Branchement.NBPOINT.ToString() : string.Empty;
                    txtNumeroPiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMEROPIECEIDENTITE) ? laDemande.LeClient.NUMEROPIECEIDENTITE : string.Empty;
                    txt_Telephone.Text = !string.IsNullOrEmpty(laDemande.LeClient.TELEPHONE) ? laDemande.LeClient.TELEPHONE : string.Empty;
                    txt_NumLot.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;
                    Txt_LibelleCommune.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLECOMMUNE) ? laDemande.Ag.LIBELLECOMMUNE : string.Empty;
                    Txt_LibelleQuartier.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLEQUARTIER) ? laDemande.Ag.LIBELLEQUARTIER : string.Empty;
                    Txt_LibelleRue.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLERUE) ? laDemande.Ag.LIBELLERUE : string.Empty;
                    //Txt_LibelleDiametre.Text = !string.IsNullOrEmpty(laDemande.Branchement.LIBELLETYPEBRANCHEMENT) ? laDemande.Branchement.LIBELLETYPEBRANCHEMENT : string.Empty;

                    if (SessionObject.LstReglageCompteur.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(laDemande.LaDemande.REGLAGECOMPTEUR))
                        {
                            CsReglageCompteur ReglageCompt = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.CODE == laDetailDemande.LaDemande.REGLAGECOMPTEUR);
                            Txt_LibelleDiametre.Text = ReglageCompt != null ? ReglageCompt.LIBELLE  : string.Empty;
                        }
                    }
                    Txt_LibelleCategorie.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLECATEGORIE) ? laDemande.LeClient.LIBELLECATEGORIE : string.Empty;
                    Txt_TypePiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLETYPEPIECE) ? laDemande.LeClient.LIBELLETYPEPIECE : string.Empty;
                    Txt_LibelleTournee.Text = !string.IsNullOrEmpty(laDemande.Ag.TOURNEE) ? laDemande.Ag.TOURNEE : string.Empty;

                    TxtLongitude.Text =laDemande.Branchement!=null && !string.IsNullOrEmpty(laDemande.Branchement.LONGITUDE) ? laDemande.Branchement.LONGITUDE : string.Empty;
                    TxtLatitude.Text = laDemande.Branchement != null && !string.IsNullOrEmpty(laDemande.Branchement.LATITUDE) ? laDemande.Branchement.LATITUDE : string.Empty;
                    TxtCompteur.Text = laDemande.LstCanalistion != null && laDemande.LstCanalistion.Count != 0 ? laDemande.LstCanalistion.FirstOrDefault().NUMERO : string.Empty;

            
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
        private void RenseignerInformationsProgrammeDevis(CsDemande lademande)
        {
            try
            {
                if (lademande != null)
                {
                    if (lademande.Programmation  != null)
                    {
                        this.Txt_DateExecution.Text =  lademande.Programmation.DATEPROGRAMME.Value.ToShortDateString() ;
                        this.Txt_DateSortieCompteur.Text = lademande.Programmation.DATELIVRAISONCOMPTEUR != null ? lademande.Programmation.DATELIVRAISONCOMPTEUR.Value.ToShortDateString() : string.Empty;
                        this.Txt_AgentLivreurCompteur.Text = lademande.Programmation.LIVREURCOMPTEUR != null ? lademande.Programmation.LIVREURCOMPTEUR : string.Empty ;
                        this.Txt_AgentRecepteurCompteur.Text = lademande.Programmation.RECEPTEURCOMPTEUR != null ? lademande.Programmation.RECEPTEURCOMPTEUR : string .Empty ;
                        this.Txt_DateSortieMateriel.Text = lademande.Programmation.DATELIVRAISONMATERIEL != null ? lademande.Programmation.DATELIVRAISONMATERIEL.Value.ToShortDateString() : string.Empty;
                        this.Txt_AgentLivreurMateriel.Text = lademande.Programmation.LIVREURMATERIEL != null ?  lademande.Programmation.LIVREURMATERIEL : string.Empty ;
                        this.Txt_AgentRecepeturMateriel.Text = lademande.Programmation.RECEPTEURMATERIEL != null ? lademande.Programmation.RECEPTEURMATERIEL : string.Empty;
                        this.Txt_Equipe.Text = lademande.Programmation.LIBELLEEQUIPE;
                        AfficherOuMasquer(tabItemProgramme, true);
                    }
                    else
                        AfficherOuMasquer(tabItemProgramme, false);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RenseignerInformationsFournitureDevis( CsDemande lademande)
        {
            try
            {
                if (lademande != null)
                {
                    if(lademande.EltDevis != null && lademande.EltDevis.Count > 0)
                    {
                        dataGridForniture.ItemsSource = lademande.EltDevis;
                        this.Txt_TotalHt.Text = lademande.EltDevis.Sum(t => t.MONTANTHT ).Value .ToString(SessionObject.FormatMontant);
                        this.Txt_TotalTtc.Text = lademande.EltDevis.Sum(t => t.MONTANTTTC  ).Value .ToString(SessionObject.FormatMontant);
                        this.Txt_TotalTva.Text = lademande.EltDevis.Sum(t => t.MONTANTTAXE ).Value .ToString(SessionObject.FormatMontant);
                        AfficherOuMasquer(tabItemFournitures, true);
                    }
                    else
                        AfficherOuMasquer(tabItemFournitures, false );
                }

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
        private List<ObjDOCUMENTSCANNE> ObjetScanne = new List<ObjDOCUMENTSCANNE>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        FileStream fs;
        private void OuvrirPieceJointe_Click(object sender, RoutedEventArgs e)
        {
            if (dgListePiece.SelectedItem != null)
            {
                ObjDOCUMENTSCANNE selectObj = (ObjDOCUMENTSCANNE)this.dgListePiece.SelectedItem;
                if (selectObj.CONTENU != null)
                {
                    MemoryStream memoryStream = new MemoryStream(selectObj.CONTENU);
                    var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                    ucImageScanne.Show();
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.DocumentScanneContenuCompleted += (s, args) =>
                    {
                        if ((args != null && args.Cancelled) || (args.Error != null))
                            return;

                        MemoryStream memoryStream = new MemoryStream(args.Result.CONTENU);
                        var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                        ucImageScanne.Show();
                    };
                    service.DocumentScanneContenuAsync(selectObj);
                    service.CloseAsync();
                }
            }
        }

        //private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        //{
        //    if (dgListePiece.SelectedItem != null)
        //    {
        //        ObjDOCUMENTSCANNE selectObj = (ObjDOCUMENTSCANNE)this.dgListePiece.SelectedItem;
        //        if (selectObj.CONTENU != null)
        //        {
        //            MemoryStream memoryStream = new MemoryStream(selectObj.CONTENU);
        //            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
        //            ucImageScanne.Show();
        //        }
        //        else
        //        {
        //            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //            service.DocumentScanneContenuCompleted += (s, args) =>
        //            {
        //                if ((args != null && args.Cancelled) || (args.Error != null))
        //                    return;

        //                MemoryStream memoryStream = new MemoryStream(args.Result.CONTENU);
        //                var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
        //                ucImageScanne.Show();
        //            };
        //            service.DocumentScanneContenuAsync(selectObj);
        //            service.CloseAsync();

        //        }


        //    }
        //}
     
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
    }
}


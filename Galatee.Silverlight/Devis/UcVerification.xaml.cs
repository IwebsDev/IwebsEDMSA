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
    public partial class UcVerification : ChildWindow
    {
        private ObjDEVIS ObjetDevisSelectionne = null;
        private ObjDEVIS MyDevis = new ObjDEVIS();
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = new CsDemande();

        public bool IsForAnalyse { get; set; }
        Galatee.Silverlight.Shared.UcFichierJoint ctrl = null;

        public UcVerification()
        {
            InitializeComponent();
        }
        public UcVerification(int idDevis)
        {
            InitializeComponent();
            ChargerTypeDocument();
            AfficherOuMasquer(tabItemDemandeur, false);
            AfficherOuMasquer(tabItemAppareils, false);
            AfficherOuMasquer(tabItemFournitures, false);
            AfficherOuMasquer(tabItemRejet, false);
            AfficherOuMasquer(tabItemAbonnement, false);
            AfficherOuMasquer(tabItemMetre, false);
            this.Chk_PasDeFacture.Visibility = System.Windows.Visibility.Collapsed;
            ChargeDetailDEvis(idDevis);

        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeAsync(IdDemandeDevis, string.Empty);
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
                    //#region DocumentScanne
                    //if (laDetailDemande.ObjetScanne != null && laDetailDemande.ObjetScanne.Count != 0)
                    //{
                    //    foreach (var item in laDetailDemande.ObjetScanne)
                    //    {
                    //        LstPiece.Add(item);
                    //        ObjetScanne.Add(item);
                    //    }
                    //    dgListePiece.ItemsSource = null ;
                    //    dgListePiece.ItemsSource = ObjetScanne;
                    //}
                    //#endregion

                    //if(  laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation)
                    //   this.Chk_PasDeFacture.Visibility = System.Windows.Visibility.Visible ;
 
                    laDemandeSelect = laDetailDemande.LaDemande;
                    RenseignerInformationsDevis(laDetailDemande);
                    RenseignerInformationsDemandeDevis(laDetailDemande);
                    RenseignerInformationsAppareilsDevis(laDetailDemande);
                    RenseignerInformationsFournitureDevis(laDetailDemande);
                    RenseignerInformationsAbonnement(laDetailDemande);
                    RenseignerInformationsBrt(laDetailDemande);
                    //RenseignerInformationsDocumentScanne();
                    RenseignerInformationsAnnotationDevis(laDetailDemande);

                    #region DocumentScanne
                    ctrl = new Galatee.Silverlight.Shared.UcFichierJoint(laDetailDemande.ObjetScanne, false);
                    Vwb.Stretch = Stretch.None;
                    Vwb.Child = ctrl;
                    #endregion


                    LayoutRoot.Cursor = Cursors.Arrow;
                }
                LayoutRoot.Cursor = Cursors.Arrow;



            };
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btn_transmetre.IsEnabled = false;
                EnregisterOuTransmetre(false);
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        CsAnnotation leAnnotation;
        private void EnregisterOuTransmetre(bool isTransmetre)
        {
            try
            {
                #region Doc Scanne
                if (laDetailDemande.ObjetScanne == null) laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                if (ctrl != null && ctrl.LstPiece != null)
                {
                    laDetailDemande.ObjetScanne.Clear();
                    laDetailDemande.ObjetScanne.AddRange(ctrl.LstPiece.Where(i => i.ISNEW == true || i.ISTOREMOVE == true));
                }
                #endregion

                if (!string.IsNullOrEmpty(this.Txt_Annotation.Text))
                {
                    leAnnotation = new CsAnnotation();
                    leAnnotation.COMMENTAIRE = this.Txt_Annotation.Text;
                    leAnnotation.DATECREATION = System.DateTime.Now;
                    leAnnotation.USERCREATION = UserConnecte.matricule;
                    leAnnotation.MATRICULE = UserConnecte.matricule;
                    leAnnotation.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                    leAnnotation.FK_IDETAPE = laDetailDemande.InfoDemande.FK_IDETAPEACTUELLE;
                    laDetailDemande.AnnotationDemande = new List<CsAnnotation>();
                    laDetailDemande.AnnotationDemande.Add(leAnnotation);
                }
                else
                    laDetailDemande.AnnotationDemande = null;

                laDetailDemande.LaDemande.ISPASDEFACTURE = this.Chk_PasDeFacture.IsChecked.Value;
                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                clientDevis.VerificationDemandeCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (string.IsNullOrEmpty(b.Result))
                    {
                        if (isTransmetre)
                            Message.ShowInformation("Demande transmise avec succès", "Demande");
                        else
                            Message.ShowInformation("Mise à jour effectuée avec succès", "Demande");
                        this.DialogResult = false;

                    }
                    else
                        Message.ShowError(b.Result, "Demande");

                };
                clientDevis.VerificationDemandeAsync(laDetailDemande, isTransmetre);
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
        private void RenseignerInformationsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                   ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID  == laDemande.LaDemande.FK_IDCENTRE );

                    Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE.ToString()) ? laDemande.LaDemande.ORDRE.ToString() : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLECENTRE) ? laDemande.LaDemande.LIBELLECENTRE : string.Empty;
                    Txt_Client.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CLIENT) ? laDemande.LaDemande.CLIENT : string.Empty;
                    Txt_LibelleSite.Text = !string.IsNullOrEmpty(leCentre.LIBELLESITE) ? leCentre.LIBELLESITE : string.Empty;
                    Txt_LibelleProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLEPRODUIT) ? laDemande.LaDemande.LIBELLEPRODUIT : string.Empty;
                    Txt_LibelleTypeDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLETYPEDEMANDE) ? laDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;
                    txtPropriete.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;

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

        private void RenseignerInformationsDemandeDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.LeClient != null && laDemande.Ag != null)
                {
                    Txt_NomClient.Text = !string.IsNullOrEmpty(laDemande.LeClient.NOMABON) ? laDemande.LeClient.NOMABON : string.Empty; 
                    txt_Quartier.Text = !string.IsNullOrEmpty(laDemande.Ag.QUARTIER) ? laDemande.Ag.QUARTIER : string.Empty;
                    txt_NumRue.Text = !string.IsNullOrEmpty(laDemande.Ag.RUE) ? laDemande.Ag.RUE : string.Empty;
                    txtAdresse.Text = !string.IsNullOrEmpty(laDemande.Branchement.ADRESSERESEAU) ? laDemande.Branchement.ADRESSERESEAU : string.Empty;
                    txtNumeroPiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMEROPIECEIDENTITE) ? laDemande.LeClient.NUMEROPIECEIDENTITE : string.Empty;
                    txt_Telephone.Text = !string.IsNullOrEmpty(laDemande.LeClient.TELEPHONE) ? laDemande.LeClient.TELEPHONE : string.Empty;
                    txt_NumLot.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;
                    Txt_LibelleCommune.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLECOMMUNE) ? laDemande.Ag.LIBELLECOMMUNE : string.Empty;
                    Txt_LibelleQuartier.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLEQUARTIER) ? laDemande.Ag.LIBELLEQUARTIER : string.Empty;
                    Txt_Porte.Text = !string.IsNullOrEmpty(laDemande.Ag.PORTE) ? laDemande.Ag.PORTE : string.Empty;
                    Txt_LibelleCategorie.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLECATEGORIE) ? laDemande.LeClient.LIBELLECATEGORIE : string.Empty;
                    Txt_TypePiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLETYPEPIECE) ? laDemande.LeClient.LIBELLETYPEPIECE : string.Empty;
                    Txt_LibelleTournee.Text = !string.IsNullOrEmpty(laDemande.Ag.TOURNEE) ? laDemande.Ag.TOURNEE : string.Empty;
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

            this.Txt_TotalHt.Text = lstFourgenerale.Where(t=>t.QUANTITE != 0 && t.QUANTITE != null).Sum(t => t.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
            this.Txt_TotalTtc.Text = lstFourgenerale.Where(t => t.QUANTITE != 0 && t.QUANTITE != null).Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
            this.Txt_TotalTva.Text = lstFourgenerale.Where(t => t.QUANTITE != 0 && t.QUANTITE != null).Sum(t => t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
        }
        //private void RemplirListeMaterielMT(List<ObjELEMENTDEVIS> lstEltDevis)
        //{
        //    List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();
        //    if (lstEltDevis.Count != 0)
        //    {
        //        List<ObjELEMENTDEVIS> lstFourHTA = new List<ObjELEMENTDEVIS>();
        //        List<ObjELEMENTDEVIS> lstFourBTA = new List<ObjELEMENTDEVIS>();
        //        List<ObjELEMENTDEVIS> lstComptage = new List<ObjELEMENTDEVIS>();

        //        lstFourHTA = lstEltDevis.Where(t => t.RUBRIQUE == "HTA").ToList();
        //        lstFourBTA = lstEltDevis.Where(t => t.RUBRIQUE == "BTA").ToList();
        //        lstComptage = lstEltDevis.Where(t => t.RUBRIQUE == "COM").ToList();

        //        ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
        //        leSeparateur.DESIGNATION = "----------------------------------";
        //        leSeparateur.ISDEFAULT = true;


        //        if (lstFourHTA.Count != 0)
        //        {
        //            ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
        //            leResultatBranchanchement.DESIGNATION = "Sous Total Ligne HTA";
        //            leResultatBranchanchement.IsCOLORIE = true;
        //            leResultatBranchanchement.ISDEFAULT = true;
        //            leResultatBranchanchement.MONTANTHT = lstEltDevis.Where(t => t.RUBRIQUE == "HTA").Sum(t => t.MONTANTHT);
        //            leResultatBranchanchement.MONTANTTAXE = lstEltDevis.Where(t => t.RUBRIQUE == "HTA").Sum(t => t.MONTANTTAXE);
        //            leResultatBranchanchement.MONTANTTTC = lstEltDevis.Where(t => t.RUBRIQUE == "HTA").Sum(t => t.MONTANTTTC);

        //            lstFourgenerale.AddRange(lstFourHTA);
        //            lstFourgenerale.Add(leSeparateur);
        //            lstFourgenerale.Add(leResultatBranchanchement);
        //            lstFourgenerale.Add(new ObjELEMENTDEVIS()
        //            {
        //                DESIGNATION = "    "
        //            });

        //        }
        //        if (lstFourBTA.Count != 0)
        //        {
        //            ObjELEMENTDEVIS leResultatExtension = new ObjELEMENTDEVIS();
        //            leResultatExtension.DESIGNATION = "Sous Total Ligne HTA/BT";
        //            leResultatExtension.IsCOLORIE = true;
        //            leResultatExtension.ISDEFAULT = true;
        //            leResultatExtension.MONTANTHT = lstEltDevis.Where(t => t.RUBRIQUE == "BTA").Sum(t => t.MONTANTHT);
        //            leResultatExtension.MONTANTTAXE = lstEltDevis.Where(t => t.RUBRIQUE == "BTA").Sum(t => t.MONTANTTAXE);
        //            leResultatExtension.MONTANTTTC = lstEltDevis.Where(t => t.RUBRIQUE == "BTA").Sum(t => t.MONTANTTTC);

        //            lstFourgenerale.AddRange(lstFourBTA);
        //            lstFourgenerale.Add(leSeparateur);
        //            lstFourgenerale.Add(leResultatExtension);

        //            lstFourgenerale.Add(new ObjELEMENTDEVIS()
        //            {
        //                DESIGNATION = "    "
        //            });
        //        }

        //        if (lstFourBTA.Count != 0 || lstFourHTA.Count != 0)
        //        {
        //            ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
        //            leResultatGeneral.DESIGNATION = "TOTAL GENERAL ";
        //            leResultatGeneral.IsCOLORIE = true;
        //            leResultatGeneral.ISDEFAULT = true;
        //            leResultatGeneral.MONTANTHT = lstEltDevis.Sum(t => t.MONTANTHT);
        //            leResultatGeneral.MONTANTTAXE = lstEltDevis.Sum(t => t.MONTANTTAXE);
        //            leResultatGeneral.MONTANTTTC = lstEltDevis.Sum(t => t.MONTANTTTC);
        //            lstFourgenerale.Add(leSeparateur);
        //            lstFourgenerale.Add(leResultatGeneral);
        //        }
        //    }
        //    this.dataGridForniture.ItemsSource = null;
        //    this.dataGridForniture.ItemsSource = lstFourgenerale.ToList();
        //    if (dataGridForniture.ItemsSource != null)
        //    {
        //        Txt_TotalTtc.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
        //        Txt_TotalTva.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
        //        Txt_TotalHt.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
        //    }
        //}
        private void RemplirListeMaterielMT(List<ObjELEMENTDEVIS> lstEltDevis, List<CsRubriqueDevis> leRubriques)
        {
            try
            {
                ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
                leSeparateur.LIBELLE = "----------------------------------";
                leSeparateur.ISDEFAULT = true;
                List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();

                foreach (CsRubriqueDevis item in leRubriques)
                {
                    bool MiseAZereLigne = false;
                    List<ObjELEMENTDEVIS> lstFourRubrique = lstEltDevis.Where(t => t.FK_IDRUBRIQUEDEVIS == item.PK_ID).ToList();
                    if (lstFourRubrique != null && lstFourRubrique.Count != 0)
                    {
                        int CoperTrv = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
                        lstFourRubrique.ForEach(t => t.FK_IDCOPER = CoperTrv);
                        if (item.PK_ID == 1 && laDetailDemande.Branchement.CODEBRT == "0001")
                        {
                            decimal? MontantLigne = 0;

                            ObjELEMENTDEVIS leIncidence = lstEltDevis.FirstOrDefault(t => t.ISGENERE == true);
                            leIncidence.FK_IDRUBRIQUEDEVIS = item.PK_ID;
                            leIncidence.QUANTITE = 1;
                            leIncidence.FK_IDCOPER = CoperTrv;
                            leIncidence.MONTANTTAXE = 0;
                            leIncidence.MONTANTHT = 0;
                            leIncidence.MONTANTTTC = leIncidence.QUANTITE * (leIncidence.COUTUNITAIRE_FOURNITURE + leIncidence.COUTUNITAIRE_POSE) * (-1);
                            if (lstFourRubrique.FirstOrDefault(t => t.ISGENERE) == null)
                                lstFourRubrique.Add(leIncidence);
                            MontantLigne = lstFourRubrique.Sum(t => t.MONTANTTTC);
                            if (MontantLigne < 0)
                                MiseAZereLigne = true;

                        }
                        decimal? MontantTotRubrique = lstFourRubrique.Sum(t => t.MONTANTTTC);
                        decimal? MontantTotRubriqueHt = lstFourRubrique.Sum(t => t.MONTANTHT);
                        decimal? MontantTotRubriqueTaxe = lstFourRubrique.Sum(t => t.MONTANTTAXE);
                        if (MiseAZereLigne == true)
                        { MontantTotRubrique = 0; MontantTotRubriqueHt = 0; MontantTotRubriqueTaxe = 0; }
                        ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                        leResultatBranchanchement.DESIGNATION = "Sous Total " + item.LIBELLE;
                        leResultatBranchanchement.IsCOLORIE = true;
                        leResultatBranchanchement.FK_IDRUBRIQUEDEVIS = item.PK_ID;
                        leResultatBranchanchement.ISDEFAULT = true;
                        leResultatBranchanchement.MONTANTHT = MontantTotRubriqueHt;
                        leResultatBranchanchement.MONTANTTAXE = MontantTotRubriqueTaxe;
                        leResultatBranchanchement.MONTANTTTC = MontantTotRubrique;

                        lstFourgenerale.AddRange(lstFourRubrique);
                        lstFourgenerale.Add(leSeparateur);
                        lstFourgenerale.Add(leResultatBranchanchement);
                        lstFourgenerale.Add(new ObjELEMENTDEVIS()
                        {
                            LIBELLE = "    "
                        });
                    }

                }
                if (lstFourgenerale.Count != 0)
                {
                    decimal? MontantTotRubrique = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTTTC);
                    decimal? MontantTotRubriqueHt = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTHT);
                    decimal? MontantTotRubriqueTaxe = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTTAXE);
                    if (MontantTotRubrique < 0)
                    { MontantTotRubrique = 0; MontantTotRubriqueHt = 0; MontantTotRubriqueTaxe = 0; }


                    ObjELEMENTDEVIS leSurveillance = new ObjELEMENTDEVIS();
                    leSurveillance.DESIGNATION = "Etude et surveillance ";
                    leSurveillance.ISFORTRENCH = true;
                    leSurveillance.QUANTITE = 1;
                    leSurveillance.MONTANTHT = MontantTotRubriqueHt * (decimal)(0.10); ;
                    leSurveillance.MONTANTTAXE = MontantTotRubriqueTaxe * (decimal)(0.10); ;
                    leSurveillance.MONTANTTTC = MontantTotRubrique * (decimal)(0.10);
                    leSurveillance.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
                    leSurveillance.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;

                    lstFourgenerale.Add(leSurveillance);


                    ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
                    leResultatGeneral.DESIGNATION = "TOTAL FACTURE TRAVAUX ";
                    //leResultatGeneral.IsCOLORIE = true;
                    leResultatGeneral.ISDEFAULT = true;
                    leResultatGeneral.MONTANTHT = MontantTotRubrique;
                    leResultatGeneral.MONTANTTAXE = MontantTotRubriqueHt;
                    leResultatGeneral.MONTANTTTC = MontantTotRubriqueTaxe;
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatGeneral);
                }
                ObjELEMENTDEVIS leResultatGeneralaVANCE = new ObjELEMENTDEVIS();
                leResultatGeneralaVANCE.DESIGNATION = "FACTURE AVANCE SUR CONSOMMATION ";
                //leResultatGeneralaVANCE.IsCOLORIE = true;
                leResultatGeneralaVANCE.ISDEFAULT = true;
                leResultatGeneralaVANCE.QUANTITE = lstEltDevis.FirstOrDefault(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).QUANTITE;
                leResultatGeneralaVANCE.MONTANTHT = lstEltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).Sum(y => y.MONTANTHT);
                leResultatGeneralaVANCE.MONTANTTAXE = lstEltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).Sum(y => y.MONTANTTAXE);
                leResultatGeneralaVANCE.MONTANTTTC = lstEltDevis.Where(t => t.CODECOPER == SessionObject.Enumere.CoperCAU).Sum(y => y.MONTANTTTC);
                leResultatGeneralaVANCE.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperCAU).PK_ID;
                leResultatGeneralaVANCE.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;

                lstFourgenerale.Add(leSeparateur);
                lstFourgenerale.Add(leResultatGeneralaVANCE);
                this.dataGridForniture.ItemsSource = null;
                this.dataGridForniture.ItemsSource = lstFourgenerale;

                this.Txt_TotalTtc.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalTva.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalHt.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
            }
            catch (Exception ex)
            {
                Message.Show("Erreur au chargement des couts", "");
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
        private void RenseignerInformationsFournitureDevis( CsDemande lademande)
        {
            try
            {
                if (lademande != null && (lademande.EltDevis != null && lademande.EltDevis.Count > 0))
                {

                    if (lademande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        RemplirListeMateriel(lademande.EltDevis);
                    else
                        RemplirListeMaterielMT(lademande.EltDevis, SessionObject.LstRubriqueDevis);
                    AfficherOuMasquer(tabItemFournitures, true);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur au chargement des fournitures", "Validation devis");
            }
        }
        private void RenseignerInformationsAbonnement(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.Abonne != null && laDemande.Abonne != null)
                {
                        this.Txt_CodePuissanceUtilise.Text = laDetailDemande.Branchement.PUISSANCEINSTALLEE.ToString();
                        this.Txt_CodeTarif.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.TYPETARIF) ? laDetailDemande.Abonne.TYPETARIF : string.Empty;
                        this.Txt_CodePussanceSoucrite.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.PUISSANCE.Value.ToString()) ? laDetailDemande.Abonne.PUISSANCE.Value.ToString() : string.Empty;

                        if (laDetailDemande.Abonne.PUISSANCE != null)
                            this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(laDetailDemande.Abonne.PUISSANCE.ToString()).ToString("N2");
                        if (laDetailDemande.Abonne.PUISSANCEUTILISEE != null)
                            this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(laDetailDemande.Abonne.PUISSANCEUTILISEE.Value).ToString("N2");
                        this.Txt_CodeRistoune.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.RISTOURNE.ToString()) ? string.Empty : Convert.ToDecimal(laDetailDemande.Abonne.RISTOURNE.Value).ToString("N2");

                        this.Txt_CodeForfait.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.FORFAIT) ? string.Empty : laDetailDemande.Abonne.FORFAIT;
                        this.Txt_LibelleForfait.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEFORFAIT) ? string.Empty : laDetailDemande.Abonne.LIBELLEFORFAIT;

                        this.Txt_CodeTarif.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.TYPETARIF) ? string.Empty : laDetailDemande.Abonne.TYPETARIF;
                        this.Txt_LibelleTarif.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLETARIF) ? laDetailDemande.Abonne.LIBELLETARIF : string.Empty;

                        this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.PERFAC) ? string.Empty : laDetailDemande.Abonne.PERFAC;
                        this.Txt_LibelleFrequence.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEFREQUENCE) ? laDetailDemande.Abonne.LIBELLEFREQUENCE : string.Empty;

                        this.Txt_CodeMoisIndex.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.MOISREL) ? string.Empty : laDetailDemande.Abonne.MOISREL;
                        this.Txt_LibelleMoisIndex.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEMOISIND) ? laDetailDemande.Abonne.LIBELLEMOISIND : string.Empty;

                        this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.MOISFAC) ? string.Empty : laDetailDemande.Abonne.MOISFAC;
                        this.Txt_LibMoisFact.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEMOISFACT) ? laDetailDemande.Abonne.LIBELLEMOISFACT : string.Empty;

                        this.Txt_DateAbonnement.Text = (laDetailDemande.Abonne.DABONNEMENT == null) ? DateTime.Now.ToShortDateString() : Convert.ToDateTime(laDetailDemande.Abonne.DABONNEMENT.Value).ToShortDateString();

                        this.Chk_IsExonneration.IsChecked = (laDetailDemande.Abonne.ESTEXONERETVA == true ) ? true : false ;
                        this.txt_DebutExoneration.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.DEBUTEXONERATIONTVA ) ? string.Empty :
                                                  ClasseMEthodeGenerique.FormatPeriodeMMAAAA(laDetailDemande.Abonne.DEBUTEXONERATIONTVA);

                        this.txt_FinExoneration.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.FINEXONERATIONTVA ) ? string.Empty :
                                   ClasseMEthodeGenerique.FormatPeriodeMMAAAA(laDetailDemande.Abonne.FINEXONERATIONTVA);
                       
                    AfficherOuMasquer(tabItemAbonnement, true);
                }
                else
                    AfficherOuMasquer(tabItemAbonnement, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsBrt(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.Branchement  != null  )
                {
                    if (laDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    {
                        if (SessionObject.LstReglageCompteur != null && SessionObject.LstReglageCompteur.Count != 0 && laDemande.LaDemande.REGLAGECOMPTEUR != null)
                            this.Txt_Typedecompteur.Text = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.CODE == laDemande.LaDemande.REGLAGECOMPTEUR).LIBELLE;
                    }
                    this.Txt_Distance.Text = laDetailDemande.Branchement.LONGBRT == null  ? string.Empty : laDetailDemande.Branchement.LONGBRT.ToString();
                    this.Txt_Tournee.Text = string.IsNullOrEmpty(laDetailDemande.Ag.TOURNEE) ? string.Empty : laDetailDemande.Ag.TOURNEE;
                    this.TxtOrdreTournee.Text = string.IsNullOrEmpty(laDetailDemande.Ag.ORDTOUR) ? string.Empty : laDetailDemande.Ag.ORDTOUR;
                    this.TxtPuissance.Text = laDetailDemande.Branchement.PUISSANCEINSTALLEE == null ? string.Empty : laDetailDemande.Branchement.PUISSANCEINSTALLEE.Value.ToString(SessionObject.FormatMontant);
                    this.TxtLongitude.Text =string.IsNullOrEmpty(laDetailDemande.Branchement.LONGITUDE)? string.Empty : laDetailDemande.Branchement.LONGITUDE;
                    this.TxtLatitude.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LATITUDE) ? string.Empty : laDetailDemande.Branchement.LATITUDE;

                    dgListeFraixParicipation.ItemsSource = null;
                    dgListeFraixParicipation.ItemsSource = laDemande.LstFraixParticipation;

                    Txt_LibelleTypeBrt.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLETYPEBRANCHEMENT) ? string.Empty : laDetailDemande.Branchement.LIBELLETYPEBRANCHEMENT;
                    Txt_LibellePosteSource.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEPOSTESOURCE) ? string.Empty : laDetailDemande.Branchement.LIBELLEPOSTESOURCE;
                    Txt_LibelleDepartHTA.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEDEPARTHTA) ? string.Empty : laDetailDemande.Branchement.LIBELLEDEPARTHTA;
                    Txt_LibelleQuartierPoste.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEQUARTIER) ? string.Empty : laDetailDemande.Branchement.LIBELLEQUARTIER;
                    Txt_LibellePosteTransformateur.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLETRANSFORMATEUR) ? string.Empty : laDetailDemande.Branchement.LIBELLETRANSFORMATEUR;
                    Txt_LibelleDepartBt.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.DEPARTBT) ? string.Empty : laDetailDemande.Branchement.DEPARTBT;

                    AfficherOuMasquer(tabItemMetre, true);
                }
                else
                    AfficherOuMasquer(tabItemMetre, false);

            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur au chargement du metré", "Demande");
            }
        }
          
     private void RenseignerInformationsDocumentScanne()
        {
            try
            {
                #region DocumentScanne
                ctrl = new Galatee.Silverlight.Shared.UcFichierJoint(laDetailDemande.ObjetScanne,true );
                Vwb.Stretch = Stretch.None;
                Vwb.Child = ctrl;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RenseignerInformationsAnnotationDevis(CsDemande lademande)
        {
            try
            {
                //if (lademande != null && (lademande.LstCommentaire != null && lademande.LstCommentaire.Count != 0))
                if (lademande != null && (lademande.AnnotationDemande != null && lademande.AnnotationDemande.Count != 0))
                {
                    //List<CsCommentaireRejet> lstRejets = new List<CsCommentaireRejet>();
                    List<CsAnnotation> lstRejets = new List<CsAnnotation>();
                    lstRejets.AddRange (lademande.AnnotationDemande);
                    dtg_RejetDemande.ItemsSource = lstRejets;
                    AfficherOuMasquer(tabItemRejet, true);

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
            if (((HyperlinkButton)sender).Tag != null)
            {
                MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
                var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                ucImageScanne.Show();
            }
        }
        private void hyperlinkButtonPropScannee___Click(object sender, RoutedEventArgs e)
        {
            if (((HyperlinkButton)sender).Tag != null)
            {
                MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
                var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                ucImageScanne.Show();
            }
            else
            {
                Message.ShowInformation("Aucune image associer à cette ligne", "Information");
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

        private void btn_transmetre_Click(object sender, RoutedEventArgs e)
        {
            this.btn_transmetre.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            this.RejeterButton .IsEnabled = false;
            EnregisterOuTransmetre(true);
        }

        private void RejeterButton_Click(object sender, RoutedEventArgs e)
        {
            this.btn_transmetre.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            this.RejeterButton.IsEnabled = false;
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
        }



     
    }
}


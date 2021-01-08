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
    public partial class UcOrdreDeTravail : ChildWindow
    {
        private ObjDEVIS ObjetDevisSelectionne = null;
        private ObjDEVIS MyDevis = new ObjDEVIS();
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        private CsDemande laDetailDemande = new CsDemande();
        public bool IsForAnalyse { get; set; }
        Galatee.Silverlight.Shared.UcFichierJoint ctrl = null;

        public UcOrdreDeTravail()
        {
            InitializeComponent();
        }
        public UcOrdreDeTravail( int idDevis)
        {
            InitializeComponent();
            ChargeDetailDEvis(idDevis);
            AfficherOuMasquer(tabItemDemandeur, false);
            AfficherOuMasquer(tabItemAppareils, false);
            AfficherOuMasquer(tabItemFournitures, false);
            //AfficherOuMasquer(tabItemOT, false);
            AfficherOuMasquer(tabItemSuivie, false);
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerDetailDemandeCompleted += (ssender, args) =>
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
                    RenseignerInformationsCrTravaux(laDetailDemande);
                    this.tabControl_Consultation.SelectedIndex = 5;
                    LayoutRoot.Cursor = Cursors.Arrow;
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
            service.ChargerDetailDemandeAsync(IdDemandeDevis,string.Empty );
        }
        private void btn_Enregister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.TxtMatricule.Text))
                    Message.ShowInformation("Sélectionnez un agent pour le suivide travaux ", "Demande");
                else
                    EnregisterOuTransmettre(laDetailDemande, leAffectation, false);  
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
      
        private void EnregisterOuTransmettre(CsDemande laDemande, CsAffectationDemandeUser leAffectation, bool IsTransmetre)
        {
            try
            {
                laDemande.OrdreTravail = new CsOrdreTravail();
                laDemande.OrdreTravail.FK_IDADMUTILISATEUR = leAffectation.FK_IDUSERAFFECTER;
                laDemande.OrdreTravail.FK_IDDEMANDE = laDemande.LaDemande.PK_ID  ;
                laDemande.OrdreTravail.LIBELLEAGENT = this.TxtNomAgent.Text;
                laDemande.OrdreTravail.MATRICULE  = UserConnecte.matricule ;
                laDemande.OrdreTravail.DATEDEBUTTRAVAUX = System.DateTime.Today.Date;
                laDemande.OrdreTravail.DATEFINTRAVAUX = System.DateTime.Today.Date;
                laDemande.OrdreTravail.PRESTATAIRE = this.Txt_Prestataire.Text;

                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.ValiderOrdreDeTravailCompleted += (ss, b) =>
                {
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
                clientDevis.ValiderOrdreDeTravailAsync(laDemande,leAffectation, IsTransmetre);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
        bool VerifieChampObligatoir()
        {
            if (string.IsNullOrEmpty(DtpDebutTravaux.Text))
            {
                Message.ShowInformation("Saisir la date début prévisionnelle des travaux", "VerifieChampObligatoir");
                return false;
            }
            if (string.IsNullOrEmpty(DtpFinTravaux.Text))
            {
                Message.ShowInformation("Saisir la date fin prévisionnelle des travaux", "VerifieChampObligatoir");
                return false;
            }
            if (string.IsNullOrEmpty(TxtMatricule.Text))
            {
                Message.ShowInformation("Sélectionner l'agent", "VerifieChampObligatoir");
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

       
        
        private void RenseignerInformationsFournitureDevis(CsDemande lademande)
        {
            try
            {
                if (lademande != null && (lademande.EltDevis != null && lademande.EltDevis.Count > 0))
                {
                    if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
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
        Dictionary<CsRubriqueDevis, ObjELEMENTDEVIS> TotalRubrique = new Dictionary<CsRubriqueDevis, ObjELEMENTDEVIS>();
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
        private void RenseignerInformationsOrdreTravail(CsDemande lademande)
        {
            try
            {
                this.TxtNumeroDevis.Text = lademande.LaDemande.NUMDEM;
                if (lademande != null && lademande.OrdreTravail  != null )
                {
                    this.TxtNomAgent.Text = !string.IsNullOrEmpty(lademande.OrdreTravail.LIBELLEAGENT) ? lademande.OrdreTravail.LIBELLEAGENT : string.Empty;
                    this.TxtMatricule.Text = !string.IsNullOrEmpty(lademande.OrdreTravail.MATRICULE) ? lademande.OrdreTravail.MATRICULE : string.Empty;
                    this.Txt_Prestataire.Text = !string.IsNullOrEmpty(lademande.OrdreTravail.PRESTATAIRE) ? lademande.OrdreTravail.PRESTATAIRE : string.Empty;
                    this.TxtMatricule.Tag = lademande.OrdreTravail.FK_IDADMUTILISATEUR ;
                    if (lademande.OrdreTravail .DATEDEBUTTRAVAUX != null )
                        this.DtpDebutTravaux.Text =   lademande.OrdreTravail.DATEDEBUTTRAVAUX  .Value.ToShortDateString();

                    if (lademande.OrdreTravail.DATEFINTRAVAUX != null)
                        this.DtpFinTravaux.Text =   lademande.OrdreTravail.DATEFINTRAVAUX .Value.ToShortDateString();

                    if (lademande.TravauxDevis != null)
                        this.TxtSectionCable.Text = lademande.TravauxDevis.NBRCABLESECTION != null ? lademande.TravauxDevis.NBRCABLESECTION.ToString() : string.Empty;
                    else
                        this.TxtSectionCable.Visibility = System.Windows.Visibility.Collapsed;
                    AfficherOuMasquer(tabItemOT, false);
                }
                else
                    this.Txt_Prestataire.Text = !string.IsNullOrEmpty(lademande.LaDemande.PRESTATAIRE) ? lademande.LaDemande.PRESTATAIRE : string.Empty;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsCrTravaux(CsDemande lademande)
        {
            try
            {
                if (lademande != null && lademande.TravauxDevis  != null)
                {
                    AfficherOuMasquer(tabItemSuivie, true);
                    this.Txt_NumeroDevis.Text = lademande.LaDemande.NUMDEM != null ? lademande.LaDemande.NUMDEM : string.Empty;
                    this.Txt_DateTravaux.Text = lademande.TravauxDevis.DATEFINTRVX != null ? lademande.TravauxDevis.DATEFINTRVX.Value.ToShortDateString() : string.Empty;
                    this.Txt_CommentaireTrx.Text = !string.IsNullOrEmpty(lademande.TravauxDevis.PROCESVERBAL) ? lademande.TravauxDevis.PROCESVERBAL : string.Empty;
                    this.Txt_NbrCableSection.Text = !string.IsNullOrEmpty(lademande.TravauxDevis.NBRCABLESECTION) ? lademande.TravauxDevis.NBRCABLESECTION : string.Empty;
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
                Galatee.Silverlight.Workflow.UserAgentPickerAffectation FormUserAgentPicker = new Galatee.Silverlight.Workflow.UserAgentPickerAffectation(laDetailDemande.LaDemande.NUMDEM, laDetailDemande.LaDemande.PK_ID, laDetailDemande.InfoDemande.FK_IDETAPEACTUELLE);
                FormUserAgentPicker.Closed += new EventHandler(FormUserAgentPicker_Closed);
                FormUserAgentPicker.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        CsAffectationDemandeUser leAffectation = new CsAffectationDemandeUser();
        private void FormUserAgentPicker_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (Galatee.Silverlight.Workflow.UserAgentPickerAffectation)sender;
                if (form != null)
                {
                    if (form.AgentSelectionne != null && form.lAffectationDem != null)
                    {
                        var agent = form.AgentSelectionne;
                        if (agent != null)
                        {
                            this.TxtMatricule.Text = agent.MATRICULE;
                            this.TxtNomAgent.Text = agent.LIBELLE;
                            this.TxtMatricule.Tag = agent.PK_ID;

                            leAffectation = form.lAffectationDem;
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
            try
            {
                if (string.IsNullOrEmpty(this.TxtMatricule.Text))
                    Message.ShowInformation("Sélectionnez un agent pour le suivide travaux ", "Demande");
                else
                    EnregisterOuTransmettre(laDetailDemande, leAffectation, true);
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void RejeterButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

        }

        
    }
}


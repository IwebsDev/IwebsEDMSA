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
    public partial class FrmInitRemboursementTravaux : ChildWindow
    {
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        int IdDemandeDevis = 0;
         CsDemandeBase laDemandeSelect = null;
        bool isPreuveSelectionnee = false;
        private UcImageScanne formScanne = null;
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        CsDemande laDetailDemande = null;
        Galatee.Silverlight.Shared.UcFichierJoint ctrl = null;

        public bool IsForAnalyse { get; set; }

        public FrmInitRemboursementTravaux()
        {
            InitializeComponent();

            ctrl = new Galatee.Silverlight.Shared.UcFichierJoint(null, false);
            Vwb.Stretch = Stretch.None;
            Vwb.Child = ctrl;

            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_NumeroDemande.MaxLength = SessionObject.Enumere.TailleNumeroDemande;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;

        }
        public FrmInitRemboursementTravaux(string TypeDemande)
        {
            InitializeComponent();
            ctrl = new Galatee.Silverlight.Shared.UcFichierJoint(null, false);
            Vwb.Stretch = Stretch.None;
            Vwb.Child = ctrl;
            this.Txt_NumeroDemande.MaxLength = SessionObject.Enumere.TailleNumeroDemande;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;

        }

        public FrmInitRemboursementTravaux(string TypeDemande,string isinit)
        {
            InitializeComponent();

            ctrl = new Galatee.Silverlight.Shared.UcFichierJoint(null, false);
            Vwb.Stretch = Stretch.None;
            Vwb.Child = ctrl;

            this.Txt_NumeroDemande.MaxLength = SessionObject.Enumere.TailleNumeroDemande;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.OKButton.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            ServiceAccueil.CsDemande ladm = new CsDemande();
            ValiderInitialisation(ladm, true);
        }
        private void ValiderInitialisation(CsDemande demandedevis, bool IsTransmetre)
        {
            try
            {
                 
                    demandedevis = GetDemandeDevisFromScreen(null, false);
                if (demandedevis != null)
                {
                    if (IsTransmetre)
                        demandedevis.LaDemande.ETAPEDEMANDE = null;
                    demandedevis.LaDemande.MATRICULE = UserConnecte.matricule;
                    demandedevis.LaDemande.CENTRE = SessionObject.LePosteCourant.CODECENTRE;
                    demandedevis.LaDemande.FK_IDCENTRE = SessionObject.LePosteCourant.FK_IDCENTRE.Value;
                    demandedevis.LaDemande.MOTIF = string.IsNullOrEmpty(this.Txt_Motif.Text) ? string.Empty : this.Txt_Motif.Text;

                    demandedevis.LeClient = laDetailDemande.LeClient;
                    demandedevis.Abonne  = laDetailDemande.Abonne ;
                    AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                        }
                    };
                    client.ValiderDemandeInitailisationAsync(demandedevis);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Une erreur est survenu suite à la validation", "Validation demande");
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
            #region Doc Scanne
            if (pDemandeDevis.ObjetScanne == null) laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
            if (ctrl != null && ctrl.LstPiece != null)
            {
                pDemandeDevis.ObjetScanne.Clear();
                pDemandeDevis.ObjetScanne.AddRange(ctrl.LstPiece.Where(i => i.ISNEW == true || i.ISTOREMOVE == true));
            }
            #endregion


            List< CsDemandeDetailCout> lesCoutduDevis = new List<CsDemandeDetailCout> ();
            if (laDetailDemande.LstCoutDemande != null && laDetailDemande.LstCoutDemande.Count != 0)
            {
                    List< CsDemandeDetailCout> lesCoutduDevisRemb =laDetailDemande.LstCoutDemande.Where(t=>t.COPER != SessionObject.Enumere.CoperFDO && t.COPER != SessionObject.Enumere.CoperFPO ).ToList();
               
                    CsDemandeDetailCout leCoutduDevis = new CsDemandeDetailCout();
                    leCoutduDevis.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                    leCoutduDevis.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                    leCoutduDevis.ORDRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.ORDRE) ? null : laDetailDemande.LaDemande.ORDRE;
                    leCoutduDevis.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                    leCoutduDevis.COPER = SessionObject.Enumere.CoperRembTrvx;
                    leCoutduDevis.MONTANTHT = lesCoutduDevisRemb.Sum(t => t.MONTANTHT) + lesCoutduDevisRemb.Sum(t => t.MONTANTTAXE);
                    leCoutduDevis.MONTANTTAXE = 0;
                    leCoutduDevis.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperRembTrvx).PK_ID;
                    leCoutduDevis.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;
                    leCoutduDevis.REFEM = System.DateTime.Today.Year.ToString()+ System.DateTime.Today.Month.ToString("00");
                    leCoutduDevis.DATECREATION = DateTime.Now;
                    leCoutduDevis.USERCREATION = UserConnecte.matricule;
                    lesCoutduDevis.Add(leCoutduDevis);
            }
            pDemandeDevis.LstCoutDemande = lesCoutduDevis;
            return pDemandeDevis;
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
                if (!string.IsNullOrEmpty(Txt_NumeroDemande.Text))
                    RetourneDemandeByNumero(Txt_NumeroDemande.Text);
                else
                {
                    Message.ShowInformation("Saisir le numero de la demande", "Demande");
                    return;
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }
        private void RetourneDemandeByNumero(string Numerodemande)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible ;

                //Galatee.Silverlight.ServiceDevis.DevisServiceClient client = new Galatee.Silverlight.ServiceDevis.DevisServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Devis"));
                AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GetDevisByNumDemandeCompleted += (ssender, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed ;
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
                        this.txt_tdem.Text = string.IsNullOrEmpty( laDemandeSelect.LIBELLETYPEDEMANDE)? string .Empty : laDemandeSelect.LIBELLETYPEDEMANDE;
                        this.txtCentre.Text = string.IsNullOrEmpty(laDemandeSelect.LIBELLECENTRE) ? string.Empty : laDemandeSelect.LIBELLECENTRE;
                        this.txtSite.Text = string.IsNullOrEmpty(laDemandeSelect.LIBELLESITE) ? string.Empty : laDemandeSelect.LIBELLESITE;
                        this.txt_Produit.Text = string.IsNullOrEmpty(laDemandeSelect.LIBELLEPRODUIT) ? string.Empty : laDemandeSelect.LIBELLEPRODUIT; 
                        this.txt_tdem.Text = laDemandeSelect.LIBELLETYPEDEMANDE;
                        if (laDemandeSelect.DCAISSE == null)
                        {
                            this.OKButton.IsEnabled = false;
                            Message.ShowInformation("Cette demande n a pas été payéé", "Demande");
                            return;
                        }
                        else
                        {

                            if (laDemandeSelect.DATEFIN  != null)
                            {
                                this.OKButton.IsEnabled = false;
                                Message.ShowInformation("Cette demande été réalisée", "Demande");
                                return;
                            }
                            if (laDetailDemande.LstCoutDemande != null && laDetailDemande.LstCoutDemande.Count != 0)
                            {
                                RemplireOngletClient(laDetailDemande.LeClient);
                                RemplirOngletAbonnement(laDetailDemande.Abonne );
                                RemplireOngletFacture(laDetailDemande.LstCoutDemande);
                                AfficherDocumentScanne(laDetailDemande.ObjetScanne);
                                Txt_Motif.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.MOTIF) ? string.Empty : laDetailDemande.LaDemande.MOTIF;
                            }
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
     

        private void RemplireOngletClient( CsClient _LeClient)
        {
            try
            {
                if (_LeClient != null)
                {

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

        private void RemplirOngletAbonnement( CsAbon  _LeAbon)
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

                this.Txt_DateAbonnement.Text = (_LeAbon.DABONNEMENT == null) ?string.Empty  : Convert.ToDateTime(_LeAbon.DABONNEMENT.Value).ToShortDateString();
            }
        }

        private void RemplireOngletFacture(List<CsDemandeDetailCout>  _LesFactClient)
        {
            try
            {
                if (_LesFactClient != null)
                {
                    _LesFactClient.ForEach(t => t.MONTANTTTC = t.MONTANTHT + t.MONTANTTAXE);
                    this.LsvFacture.ItemsSource = null;
                    this.LsvFacture.ItemsSource = _LesFactClient;
                    this.Txt_TotalHt.Text = _LesFactClient.Sum(t => t.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
                    this.Txt_totalTaxe .Text = _LesFactClient.Sum(t => t.MONTANTTAXE ).Value .ToString(SessionObject.FormatMontant );
                    this.Txt_TotalTTC.Text = _LesFactClient.Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                }
            }
            catch (Exception ex)
            {

                throw ex;
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
        private void AfficherDocumentScanne(List<ObjDOCUMENTSCANNE> _LesDocScanne)
        {
            try
            {
                if (_LesDocScanne != null && _LesDocScanne.Count != 0)
                {
                    #region DocumentScanne
                    ctrl = new Galatee.Silverlight.Shared.UcFichierJoint(_LesDocScanne, false);
                    Vwb.Stretch = Stretch.None;
                    Vwb.Child = ctrl;
                    #endregion
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
 
 
       

      
    }
}


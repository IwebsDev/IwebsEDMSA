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
using System.ComponentModel;
using System.Globalization;

namespace Galatee.Silverlight.Devis
{
    public partial class UcEtablissementDevis : ChildWindow
    {
        private ObjDEVIS DevisSelectionne = null;
        private SessionObject.ExecMode ModeExecution;
        private DataGrid _dataGrid = null;
        private decimal montantTotal = (decimal)0;
        List<ObjELEMENTDEVIS> lElements = new List<ObjELEMENTDEVIS>();
        ObjELEMENTDEVIS selectedElement = new ObjELEMENTDEVIS();
        ObjELEMENTDEVIS eltAdditional = new ObjELEMENTDEVIS();

        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = null;
        decimal seuilDistance = 0;
        decimal Supplement = 0;
        decimal taux = (decimal)0;
        bool isFicheSelectionnee = false;
        bool isFicheSupprimee = false;
        ObjDOCUMENTSCANNE doc = new ObjDOCUMENTSCANNE();
        ObjTYPEDEVIS typeDevis = new ObjTYPEDEVIS();
        public List<ObjELEMENTDEVIS> MyElements { get; set; }
        List<ObjELEMENTDEVIS> MesElements = new List<ObjELEMENTDEVIS>();
        public decimal Frais { get; set; }
        public List<ObjELEMENTDEVIS> MyFournitures { get; set; }
        //decimal? ParametreDistanceMaximale = null;
        List<ObjELEMENTDEVIS> donnesDatagrid = new List<ObjELEMENTDEVIS>();
        public CsCtax Taxe { get; set; }

        List<Galatee.Silverlight.ServiceAccueil.CsCoutDemande> LstDesCoutsDemande = new List<Galatee.Silverlight.ServiceAccueil.CsCoutDemande>();


        private List<ObjELEMENTDEVIS > ListeFournitureExistante = null;
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }

        //public ObservableCollection<ObjELEMENTDEVIS> DonnesDatagrid
        //{
        //    get { return donnesDatagrid; }
        //    set
        //    {
        //        if (value == donnesDatagrid)
        //            return;
        //        donnesDatagrid = value;
        //        NotifyPropertyChanged("DonnesDatagrid");
        //    }
        //}

        public UcEtablissementDevis(CsDemande iddemande)
        {
            try
            {
                if (LayoutRoot != null)
                    LayoutRoot.Cursor = Cursors.Wait;
                InitializeComponent();
                this.OKButton.IsEnabled = false ;
                this.CancelButton.IsEnabled = false ;
                this.Txt_DistanceExtension.Visibility = System.Windows.Visibility.Collapsed;
                this.labelDistExt.Visibility = System.Windows.Visibility.Collapsed;
                ChargeDetailDEvis(iddemande);
            }
            catch (Exception ex)
            {
                if (LayoutRoot != null)
                    LayoutRoot.Cursor = Cursors.Arrow;
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        public UcEtablissementDevis(int iddemande)
        {
            try
            {
                if (LayoutRoot != null)
                    LayoutRoot.Cursor = Cursors.Wait;
                InitializeComponent();
                this.OKButton.IsEnabled = false;
                this.CancelButton.IsEnabled = false;
                this.Txt_DistanceExtension.Visibility = System.Windows.Visibility.Collapsed;
                this.labelDistExt.Visibility = System.Windows.Visibility.Collapsed;
                ChargeDetailDEvis(iddemande);
            }
            catch (Exception ex)
            {
                if (LayoutRoot != null)
                    LayoutRoot.Cursor = Cursors.Arrow;
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeAsync(IdDemandeDevis,string.Empty );
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
                    ChargeDetailDEvis(laDetailDemande);
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
        }

        private void ChargeDetailDEvis(CsDemande  LaDem)
        {

            try
            {
                laDetailDemande = LaDem;
                laDemandeSelect = laDetailDemande.LaDemande;
                RemplirListeMateriel(laDetailDemande);
            }
            catch (Exception ex)
            {

                Message.Show(ex.Message, "Demande");
            }

        }

        void ChargerCoutDemande(CsDemande _Lademande)
        {
            try
            {
                if (SessionObject.LstDesCoutDemande.Count != 0)
                {
                    string typedemande = _Lademande.LaDemande.TYPEDEMANDE;
                    if (_Lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                        _Lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementProduit  ||
                        _Lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementMt ||
                        _Lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonementExtention ||
                        _Lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance )
                        typedemande = SessionObject.Enumere.BranchementAbonement ;

                        LstDesCoutsDemande = SessionObject.LstDesCoutDemande.Where(p => p.TYPEDEMANDE == typedemande).ToList();
                    if (!string.IsNullOrEmpty(_Lademande.LaDemande.CENTRE))
                        LstDesCoutsDemande = LstDesCoutsDemande.Where(p => p.CENTRE == _Lademande.LaDemande.CENTRE || p.CENTRE == "000").ToList();

                    if (!string.IsNullOrEmpty(_Lademande.LaDemande.PRODUIT))
                        LstDesCoutsDemande = LstDesCoutsDemande.Where(p => p.PRODUIT == _Lademande.LaDemande.PRODUIT || p.PRODUIT == "00").ToList();

                    if (LstDesCoutsDemande.Count != 0)
                    {
                        string pDiametre = string.Empty;
                        if (_Lademande.LaDemande  != null)
                            pDiametre = string.IsNullOrEmpty(_Lademande.LaDemande.REGLAGECOMPTEUR) ? string.Empty : _Lademande.LaDemande.REGLAGECOMPTEUR;
                        LstDesCoutsDemande = LstDesCoutsDemande.Where(p => p.REGLAGECOMPTEUR == pDiametre || string.IsNullOrEmpty(p.REGLAGECOMPTEUR)).ToList();

                        string pCategorie = string.Empty;
                        if (_Lademande.LeClient  != null)
                            pCategorie = string.IsNullOrEmpty(_Lademande.LeClient.CATEGORIE) ? string.Empty : _Lademande.LeClient.CATEGORIE;
                        LstDesCoutsDemande = LstDesCoutsDemande.Where(p => p.CATEGORIE == pCategorie || string.IsNullOrEmpty(p.CATEGORIE)).ToList();

                        if (_Lademande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                        {
                            decimal? pPuissanceSouscrite = 0;
                            if (_Lademande.LaDemande != null)
                                pPuissanceSouscrite = _Lademande.LaDemande.PUISSANCESOUSCRITE;

                            CsCoutDemande leCoutAvance = LstDesCoutsDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperCAU);
                            if (leCoutAvance != null)
                                leCoutAvance.MONTANT = leCoutAvance.MONTANT * pPuissanceSouscrite;

                        }

                        /**Frais de participation**/
                        ServiceAccueil.CsCoutDemande leFraisParticipation = LstDesCoutsDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperFAB);
                        if (leFraisParticipation != null && _Lademande.LstFraixParticipation != null && _Lademande.LstFraixParticipation.Count != 0)
                        {

                            int idtaxe = leFraisParticipation.FK_IDTAXE;
                            Galatee.Silverlight.ServiceAccueil.CsCtax tax = SessionObject.LstDesTaxe.FirstOrDefault(t => t.PK_ID == leFraisParticipation.FK_IDTAXE);
                            if (tax != null)
                                taux = tax.TAUX;

                            _element = new ObjELEMENTDEVIS();
                            _element.NUMDEVIS = _Lademande.LaDemande.NUMDEM;
                            _element.DESIGNATION =_element.LIBELLE  = leFraisParticipation.LIBELLECOPER;
                            _element.PRIX = Convert.ToDecimal(_Lademande.LstFraixParticipation.Sum(t => t.MONTANT));
                            _element.QUANTITE = 1;
                            _element.MONTANTHT = Convert.ToDecimal(_Lademande.LstFraixParticipation.Sum(t => t.MONTANT));
                            _element.MONTANTTAXE = _Lademande.LstFraixParticipation.Sum(t => t.MONTANT) * taux;
                            _element.MONTANTTTC = _element.MONTANTHT + _element.MONTANTTAXE;


                            _element.TAUXTAXE = taux;
                            _element.FK_IDTAXE = idtaxe;
                            _element.ISSUMMARY = true;
                            _element.ISADDITIONAL = true;
                            _element.ISEXTENSION  = false;
                            _element.ISFORTRENCH = false;
                            _element.ISDEFAULT = false;
                            _element.NUMFOURNITURE = leFraisParticipation.COPER;
                            _element.CODECOPER = leFraisParticipation.COPER;
                            _element.FK_IDCOUTCOPER = leFraisParticipation.PK_ID;
                            _element.FK_IDCOPER = leFraisParticipation.FK_IDCOPER;
                            _element.FK_IDFOURNITURE = null;
                            _element.FK_IDDEMANDE = _Lademande.LaDemande.PK_ID;
                            _element.ISFOURNITURE = true;
                            _element.ISPOSE = true;

                            _element.FK_IDRUBRIQUEDEVIS = SessionObject.LstRubriqueDevis.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DEVISBRANCHEMENT).PK_ID;
                            _element.RUBRIQUE = SessionObject.Enumere.DEVISBRANCHEMENT;

                            if (MyElements == null)
                                MyElements = new List<ObjELEMENTDEVIS>();
                            this.MyElements.Add(_element);
                            donnesDatagrid.Add(_element);
                        }
 
                        /**Autre cout**/
                        foreach (Galatee.Silverlight.ServiceAccueil.CsCoutDemande item in LstDesCoutsDemande.Where(t=>t.COPER != SessionObject.Enumere.CoperTRV && t.COPER != SessionObject.Enumere.CoperFAB ).ToList())
                        {
                            int idtaxe = item.FK_IDTAXE;
                            Galatee.Silverlight.ServiceAccueil.CsCtax tax = SessionObject.LstDesTaxe.FirstOrDefault(t => t.PK_ID == item.FK_IDTAXE);
                            if (tax != null)
                                taux = tax.TAUX;

                            if (item.MONTANT != null && item.MONTANT != 0)
                            {
                                _element = new ObjELEMENTDEVIS();
                                _element.NUMDEVIS = _Lademande.LaDemande.NUMDEM;
                                _element.DESIGNATION = _element.LIBELLE = item.LIBELLECOPER;
                                _element.PRIX = item.MONTANT != null ? (decimal)item.MONTANT : 0;
                                _element.COUTFOURNITURE = item.MONTANT != null ? (decimal)item.MONTANT : 0;

                                if (item.COPER == SessionObject.Enumere.CoperCAU &&
                                    laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT )
                                    _element.QUANTITE = int.Parse(laDetailDemande.LaDemande.PUISSANCESOUSCRITE.ToString());
                                else
                                    _element.QUANTITE = 1;
                                _element.MONTANTHT  = (item.MONTANT != null && _element.QUANTITE != null) ? (int)_element.QUANTITE * (decimal)item.MONTANT : 0;
                                _element.MONTANTTAXE  =(decimal)Math.Ceiling((double)( _element.COUT * taux));
                                _element.MONTANTTTC = _element.MONTANTHT + _element.MONTANTTAXE ;

                                _element.FK_IDTAXE = idtaxe;
                                _element.TAUXTAXE = taux;
                                _element.ISEXTENSION = false;

                                _element.TVARECAP = _element.MONTANTTAXE.Value .ToString(SessionObject.FormatMontant);
                                _element.ISDEFAULT = true ;
                                _element.NUMFOURNITURE = item.COPER;
                                _element.CODECOPER = item.COPER;
                                _element.FK_IDCOPER = item.FK_IDCOPER;
                                _element.FK_IDCOUTCOPER  = item.PK_ID ;
                                _element.FK_IDMATERIELDEVIS   = null ;
                                _element.FK_IDDEMANDE = _Lademande.LaDemande.PK_ID;
                                _element.ISFOURNITURE = true;
                                _element.ISPOSE = true;
                                if (MyElements == null)
                                    MyElements = new List<ObjELEMENTDEVIS>();
                                this.MyElements.Add(_element);
                                donnesDatagrid.Add(_element);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public UcEtablissementDevis()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.OKButton.IsEnabled = false;
                this.CancelButton .IsEnabled = false;
                Enregistrer(laDetailDemande,true );
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Enregistrer(CsDemande laDemande, bool IsTransmetre)
        {
            try
            {
                this.MyElements = this.LireElements();
                if (this.MyElements.Count == 0)
                    throw new Exception(Languages.msgAddFournitures);

                laDemande.EltDevis = this.MyElements;
                laDemande.LstCanalistion = null;
                laDemande.Abonne = null;

                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                clientDevis.ValiderMetreCompleted += (ss, b) =>
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
                            Message.ShowInformation("Demande transmise avec succès", "Demande");
                        else
                            Message.ShowInformation("Mise en attente effectuée avec succès", "Demande");
                        this.DialogResult = false;

                    }
                    else
                        Message.ShowError(b.Result, "Demande");

                    this.DialogResult = false;
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                clientDevis.ValiderMetreAsync(laDemande, IsTransmetre);
            }
            catch (Exception ex)
            {
                this.OKButton.IsEnabled = true;
                this.CancelButton.IsEnabled = true;
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }


        void frmBilanEtablissementDevis_Closed(object sender, EventArgs e)
        {
            try
            {
                DialogResult = true;
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

        private void Btn_Ajouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Ajouter();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Ajouter()
        {
            try
            {
                if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                {
                    var MyLstFourniture = this.ListeFournitureExistante;
                    if (MyLstFourniture != null)
                    {
                        if (MyElements == null)
                            MyElements = new List<ObjELEMENTDEVIS>();
                        UcListeDesignation frm = new UcListeDesignation(this.ListeFournitureExistante, MyElements,laDetailDemande );
                        if (frm != null)
                        {
                            frm.Closed += new EventHandler(frm_Closed);
                            frm.Show();
                        }
                    }
                    else
                    {
                        Message.ShowInformation("Aucun élément de fourniture coreespondant", "Information");
                    }
                }
                else
                {
                    var MyLstFourniture = this.ListeFournitureExistante;
                    if (MyLstFourniture != null)
                    {
                        UcListeDesignationMT frm = new UcListeDesignationMT(this.ListeFournitureExistante, MyElements, laDetailDemande);
                        if (frm != null)
                        {
                            frm.Closed += new EventHandler(frmMt_Closed);
                            frm.Show();
                        }
                    }
                    else
                    {
                        Message.ShowInformation("Aucun élément de fourniture coreespondant", "Information");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void frmMt_Closed(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    var form = ((UcListeDesignationMT)sender);
                    if (form != null && form.DialogResult.Value)
                    {
                        MesElements.AddRange(form.MyElements);
                        RemplirListeMaterielMT(MyElements, SessionObject.LstRubriqueDevis);
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
        private void RemplirListeMaterielMT(List<ObjELEMENTDEVIS> lstEltDevis, List<CsRubriqueDevis> leRubriques)
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
                    lstFourRubrique.ForEach(t=>t.FK_IDCOPER =CoperTrv ) ;
                    if (item.CODE  == SessionObject.Enumere.LIGNEHTA   && laDetailDemande.Branchement.CODEBRT == "0001")
                    {
                        decimal? MontantLigne = 0;

                        ObjELEMENTDEVIS leIncidence = ListeFournitureExistante.FirstOrDefault(t => t.ISGENERE == true);
                        leIncidence.FK_IDRUBRIQUEDEVIS = item.PK_ID;
                        leIncidence.QUANTITE = 1;
                        leIncidence.FK_IDCOPER = CoperTrv;
                        leIncidence.MONTANTTAXE  = 0;
                        leIncidence.MONTANTHT = 0;
                        leIncidence.FK_IDMATERIELDEVIS = leIncidence.FK_IDMATERIELDEVIS ;
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
                    leResultatBranchanchement.LIBELLE = "Sous Total " + item.LIBELLE;
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
                leSurveillance.LIBELLE = "Etude et surveillance ";
                leSurveillance.ISFORTRENCH = true;
                leSurveillance.QUANTITE  = 1;
                leSurveillance.MONTANTHT = MontantTotRubriqueHt * (decimal)(0.10); ;
                leSurveillance.MONTANTTAXE = MontantTotRubriqueTaxe * (decimal)(0.10); ;
                leSurveillance.MONTANTTTC = MontantTotRubrique * (decimal)(0.10);
                leSurveillance.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
                leSurveillance.FK_IDTAXE  = SessionObject.LstDesTaxe .FirstOrDefault(t => t.CODE  == SessionObject.Enumere.CodeSansTaxe ).PK_ID;
                lstFourgenerale.Add(leSurveillance);


                ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
                leResultatGeneral.LIBELLE = "TOTAL FACTURE TRAVAUX ";
                //leResultatGeneral.IsCOLORIE = true;
                leResultatGeneral.ISDEFAULT = true;
                leResultatGeneral.MONTANTHT = MontantTotRubrique;
                leResultatGeneral.MONTANTTAXE = MontantTotRubriqueHt;
                leResultatGeneral.MONTANTTTC = MontantTotRubriqueTaxe;
                lstFourgenerale.Add(leSeparateur);
                lstFourgenerale.Add(leResultatGeneral);
            }
            ObjELEMENTDEVIS leResultatGeneralaVANCE = new ObjELEMENTDEVIS();
            leResultatGeneralaVANCE.LIBELLE = "FACTURE AVANCE SUR CONSOMMATION ";
            //leResultatGeneralaVANCE.IsCOLORIE = true;
            leResultatGeneralaVANCE.ISDEFAULT = true;
            leResultatGeneralaVANCE.QUANTITE =int.Parse(laDetailDemande.LaDemande.PUISSANCESOUSCRITE.ToString()) ;
            leResultatGeneralaVANCE.MONTANTHT = donnesDatagrid.Sum(y=>y.MONTANTHT );
            leResultatGeneralaVANCE.MONTANTTAXE = donnesDatagrid.Sum(y => y.MONTANTTAXE );
            leResultatGeneralaVANCE.MONTANTTTC = donnesDatagrid.Sum(y => y.MONTANTTTC);
            CsCoutDemande leCoutAvance = LstDesCoutsDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperCAU);
            if (leCoutAvance != null)
                leResultatGeneralaVANCE.COUTFOURNITURE = leCoutAvance.MONTANT.Value  ;

            leResultatGeneralaVANCE.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperCAU ).PK_ID;
            leResultatGeneralaVANCE.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault (t => t.CODE   == SessionObject.Enumere.CodeSansTaxe ).PK_ID;
            
            lstFourgenerale.Add(leSeparateur);
            lstFourgenerale.Add(leResultatGeneralaVANCE);

            MyElements.Clear();
            this.MyElements.AddRange(lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).ToList());
            this.dataGridElementDevis.ItemsSource = null;
            this.dataGridElementDevis.ItemsSource = lstFourgenerale.ToList();

            this.Txt_MontantTotal.Text = MyElements.Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
        }

        void frm_Closed(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    var form = ((UcListeDesignation)sender);
                    if (form != null && form.DialogResult.Value)
                    {
                        this.MyElements = form.MyElements;
                        this.MyFournitures = form.MyFournitures;
                        if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        RemplirListeMateriel(this.MyElements);
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
        private void RemplirListeMateriel(List<ObjELEMENTDEVIS > lstEltDevis)
        {
            List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();
            if (lstEltDevis.Count != 0)
            {
                List<ObjELEMENTDEVIS> lstFourExtension = new List<ObjELEMENTDEVIS>();
                List<ObjELEMENTDEVIS> lstFourBranchement = new List<ObjELEMENTDEVIS>();

                lstFourExtension = lstEltDevis.Where(t => t.ISEXTENSION == true).ToList();
                lstFourBranchement = lstEltDevis.Where(t => t.ISEXTENSION == false).ToList();

                ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
                leSeparateur.LIBELLE  = "----------------------------------";
                leSeparateur.ISDEFAULT  =true ;

       
                if (lstFourBranchement.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                    leResultatBranchanchement.LIBELLE = "TOTAL BRANCHEMENT ";
                    leResultatBranchanchement.IsCOLORIE = true;
                    leResultatBranchanchement.ISDEFAULT  = true;
                    leResultatBranchanchement.MONTANTHT = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTHT);
                    leResultatBranchanchement.MONTANTTAXE = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTTAXE);
                    leResultatBranchanchement.MONTANTTTC = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTTTC);
                    
                    lstFourgenerale.AddRange(lstFourBranchement);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatBranchanchement);
                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        LIBELLE = "    "
                    });

                }
                if (lstFourExtension.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatExtension = new ObjELEMENTDEVIS();
                    leResultatExtension.LIBELLE = "TOTAL EXTENSION ";
                    leResultatExtension.IsCOLORIE = true;
                    leResultatExtension.ISDEFAULT = true;
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
                    leResultatGeneral.LIBELLE = "TOTAL GENERAL ";
                    leResultatGeneral.IsCOLORIE = true;
                    leResultatGeneral.ISDEFAULT = true;
                    leResultatGeneral.MONTANTHT = lstEltDevis.Sum(t => t.MONTANTHT);
                    leResultatGeneral.MONTANTTAXE = lstEltDevis.Sum(t => t.MONTANTTAXE);
                    leResultatGeneral.MONTANTTTC = lstEltDevis.Sum(t => t.MONTANTTTC);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatGeneral);
                }
            }
            this.dataGridElementDevis.ItemsSource = null;
            this.dataGridElementDevis.ItemsSource = lstFourgenerale.ToList();
        }
        private List<ObjELEMENTDEVIS> LireElements()
        {
            try
            {
                List<ObjELEMENTDEVIS> ListElementDevis = new List<ObjELEMENTDEVIS>();
                if (dataGridElementDevis.ItemsSource != null)
                {
                    foreach (ObjELEMENTDEVIS elementDevis in ((List<ObjELEMENTDEVIS>)dataGridElementDevis.ItemsSource).Where(t=>t.QUANTITE != null && t.QUANTITE != 0).ToList())
                    {
                        elementDevis.USERCREATION = UserConnecte.matricule;
                        elementDevis.DATECREATION = System.DateTime.Today.Date ;
                        elementDevis.ORDRE = int.Parse(laDemandeSelect.ORDRE);
                        elementDevis.FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID;
                        elementDevis.NUMDEM = laDetailDemande.LaDemande.NUMDEM;

                        if (elementDevis.ISEXTENSION)
                        {
                            elementDevis.FK_IDRUBRIQUEDEVIS = SessionObject.LstRubriqueDevis.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DEVISDEXTENSION).PK_ID;
                            elementDevis.RUBRIQUE = SessionObject.Enumere.DEVISDEXTENSION;
                        }
                        else
                        {
                            elementDevis.FK_IDRUBRIQUEDEVIS = SessionObject.LstRubriqueDevis.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DEVISBRANCHEMENT).PK_ID;
                            elementDevis.RUBRIQUE = SessionObject.Enumere.DEVISBRANCHEMENT ;
                        }
                        CsCoutDemande leCoutAvance = LstDesCoutsDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperCAU);
                        if (leCoutAvance != null)
                            elementDevis.COUTFOURNITURE  = leCoutAvance.MONTANT.Value  ;
                        ListElementDevis.Add(elementDevis);
                    }
                }
                return ListElementDevis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btn_Supprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Supprimer();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Supprimer()
        {
            try
            {
                
                if (this.dataGridElementDevis.SelectedItem != null )
                {
                    var mBoxControl = new MessageBoxControl.MessageBoxChildWindow(this.Title.ToString(), Languages.msgConfirmSuppression, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    mBoxControl.OnMessageBoxClosed += (_, result) =>
                    {
                        if (mBoxControl.Result == MessageBoxResult.OK)
                        {
                            List< ObjELEMENTDEVIS> EltDeLagrid = dataGridElementDevis.ItemsSource  as List< ObjELEMENTDEVIS>;
                            ObjELEMENTDEVIS select = dataGridElementDevis.SelectedItem as ObjELEMENTDEVIS;
                            ObjELEMENTDEVIS ObjSelect = EltDeLagrid.FirstOrDefault(t => t.FK_IDMATERIELDEVIS  == select.FK_IDMATERIELDEVIS && t.FK_IDRUBRIQUEDEVIS == select.FK_IDRUBRIQUEDEVIS );
                            if (ObjSelect != null)
                                EltDeLagrid.Remove(ObjSelect);
                            this.dataGridElementDevis.ItemsSource = null;
                            this.dataGridElementDevis.ItemsSource = EltDeLagrid;
                            Txt_MontantTotal.Text = CalculerCoutTotal().ToString(SessionObject.FormatMontant);

                            this.Txt_PrixUnitaire.Text = string.Empty;
                            this.Txt_Quantite.Text = string.Empty;
                            //this.OKButton.IsEnabled = BtnTransmettre.IsEnabled = (this.Txt_Distance.Text != string.Empty) && (donnesDatagrid.Count > 0);
                        }
                        else
                        {
                            return;
                        }
                    };
                    mBoxControl.Show();
                }
                else
                    throw new Exception("Veuillez sélectionner un élément sil vous plaît !");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region "Gestion MenuContextuel"

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    Supprimer();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    Ajouter();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void MenuContextuel_Opened(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuContextuel.IsEnabled = dataGridElementDevis.SelectedItem != null;
                MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        #endregion

        private void ChildWindow_Closed(object sender, EventArgs e)
        {

        }

        private void dataGridElementDevis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.ModeExecution != SessionObject.ExecMode.Consultation)
                {
                    if (this.dataGridElementDevis.SelectedItems.Count == 1)
                    {
                        ObjELEMENTDEVIS elt = (ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem;
                        this.selectedElement = elt;
                        this.Btn_Supprimer.IsEnabled = (selectedElement.ISDEFAULT != true) ? true : false;
                        this.Txt_Quantite.IsReadOnly = false;
                        this.Txt_PrixUnitaire.Text = selectedElement.PRIX.ToString(DataReferenceManager.FormatMontant);
                        this.Txt_Quantite.Text = selectedElement.QUANTITE.ToString();
                        this.Txt_Quantite.SelectAll();
                        this.Txt_Quantite.Focus();
                    }
                    else
                    {
                        this.Txt_PrixUnitaire.IsReadOnly = true;
                        this.Txt_Quantite.IsReadOnly = true;
                        Txt_PrixUnitaire.Text = string.Empty;
                        Txt_Quantite.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private int DecToInt(decimal montant)
        {
            char[] separateur = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray();
            string[] partie = montant.ToString().Split(separateur);
            return int.Parse(partie[0]);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        ObjELEMENTDEVIS _element = new ObjELEMENTDEVIS();
        private void RemplirListeMateriel(CsDemande laDemandedevis)
        {
            try
            {
                if (laDemandedevis.Branchement.LONGBRT != null && laDemandedevis.Branchement.LONGBRT > 0)
                    this.Txt_Distance.Text = DecToInt((decimal)laDemandedevis.Branchement.LONGBRT).ToString();

                if (laDemandedevis.Branchement.LONGEXTENSION != null && laDemandedevis.Branchement.LONGEXTENSION > 0)
                {
                    this.Txt_DistanceExtension.Visibility = System.Windows.Visibility.Visible ;
                    this.labelDistExt.Visibility = System.Windows.Visibility.Visible;
                    this.Txt_DistanceExtension.Text = DecToInt((decimal)laDemandedevis.Branchement.LONGEXTENSION).ToString();
                }


                Txt_NumDevis.Text = laDetailDemande.LaDemande.NUMDEM;
                Txt_TypeDevis.Text = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                AcceuilServiceClient Serviceclient = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                Serviceclient.SelectAllMaterielCompleted += (ss, bc) =>
                {
                    try
                    {
                        if (bc.Cancelled || bc.Error != null)
                        {
                            string error = bc.Error.Message;
                            if (LayoutRoot != null)
                                LayoutRoot.Cursor = Cursors.Arrow;
                            Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (bc.Result != null)
                        {
                            this.OKButton.IsEnabled = true;
                            this.CancelButton.IsEnabled = true;
                            ListeFournitureExistante = bc.Result;
                            if (laDemandedevis.EltDevis != null && laDetailDemande.EltDevis.Count != 0)
                            {
                                this.dataGridElementDevis.ItemsSource = null;
                                this.dataGridElementDevis.ItemsSource = laDetailDemande.EltDevis;
                                this.Txt_MontantTotal.Text = laDetailDemande.EltDevis.Sum(t => t.COUT).ToString(SessionObject.FormatMontant);
                                if (MyElements == null)
                                    MyElements = new List<ObjELEMENTDEVIS>();
                                this.MyElements = laDetailDemande.EltDevis;
                                return;
                            }
                            
                            #region Cout detail
                                ChargerCoutDemande(laDemandedevis);
                                dataGridElementDevis.ItemsSource = null;
                                dataGridElementDevis.ItemsSource = donnesDatagrid;
                                OKButton.IsEnabled = true;
                                this.Txt_MontantTotal.Text = donnesDatagrid.Sum(t => t.MONTANTTTC.Value).ToString(SessionObject.FormatMontant);
                            #endregion
                              
                        }
                    }
                    catch (Exception ex)
                    {
                        if (LayoutRoot != null)
                            LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(ex.Message, Languages.txtDevis);
                    }
                };
                Serviceclient.SelectAllMaterielAsync();

            }
            catch (Exception ex)
            {
                if (LayoutRoot != null)
                    LayoutRoot.Cursor = Cursors.Arrow;
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }


        private decimal CalculerCoutTotal()
        {
            decimal MontantTotal = 0;
          
            try
            {
                if (dataGridElementDevis.ItemsSource != null)
                    MontantTotal = ((List<ObjELEMENTDEVIS>)dataGridElementDevis.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTTC).Value;
                return MontantTotal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Txt_Quantite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if ((!string.IsNullOrEmpty(this.Txt_Quantite.Text)) && (int.Parse(this.Txt_Quantite.Text) > 0))
                {
                    var selectedElement = (ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem;
                    this.selectedElement = selectedElement;
                    if (selectedElement != null)
                    {
                        this.selectedElement.QUANTITE = int.Parse(this.Txt_Quantite.Text);
                        this.selectedElement.COUT = (decimal)this.selectedElement.QUANTITE * this.selectedElement.PRIX;
                        Txt_MontantTotal.Text = CalculerCoutTotal().ToString(SessionObject.FormatMontant );
                    }
                    //this.RemplirElements();
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Txt_Quantite_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((!string.IsNullOrEmpty(this.Txt_Quantite.Text)) && (int.Parse(this.Txt_Quantite.Text) > 0))
                {
                    //if (selectedElement != null)
                    //{
                    //    this.selectedElement.Quantite = int.Parse(this.Txt_Quantite.Text);
                    //    this.selectedElement.Cout = (float)this.selectedElement.Quantite * this.selectedElement.Prix;
                    //}
                    //this.RemplirElements();
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        DateTime lastClick = DateTime.Now;
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<ObjELEMENTDEVIS >;

            if (dg.SelectedItem != null)
            {
                ObjELEMENTDEVIS SelectedObject = (ObjELEMENTDEVIS)dg.SelectedItem;
                if ((DateTime.Now - lastClick).Ticks < 2500000)
                {
                    UcSaisiQuantite ctrl = new UcSaisiQuantite(SelectedObject);
                    ctrl.Closed +=ctrl_Closed;
                    ctrl.Show();
                }
                lastClick = DateTime.Now;
            }
        }
        void ctrl_Closed(object sender, EventArgs e)
        {
            UcSaisiQuantite ctrs = sender as UcSaisiQuantite;
            if (ctrs.isOkClick)
            {
                List<ObjELEMENTDEVIS> allObjects = ((ObservableCollection< ObjELEMENTDEVIS >) dataGridElementDevis.ItemsSource).ToList() ;
                if (allObjects != null)
                    this.Txt_MontantTotal.Text = allObjects.Sum(t => t.COUT).ToString(SessionObject.FormatMontant);
            }
        }

        private void dgMyDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dmdRow = e.Row.DataContext as ObjELEMENTDEVIS;
            if (dmdRow != null)
            {
                if (dmdRow.IsCOLORIE )
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Green);
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Bold;
                }
            }
        }

        private void btn_MiseEnAttente_Click(object sender, RoutedEventArgs e)
        {
            this.OKButton.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            Enregistrer(laDetailDemande, false);
        }
     
    }
}


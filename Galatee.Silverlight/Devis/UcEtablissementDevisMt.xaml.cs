﻿using System;
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
    public partial class UcEtablissementDevisMt : ChildWindow
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
        ObjDOCUMENTSCANNE doc = new ObjDOCUMENTSCANNE();
        ObjTYPEDEVIS typeDevis = new ObjTYPEDEVIS();
        public List<ObjELEMENTDEVIS> MyElements { get; set; }
        List<ObjELEMENTDEVIS> MesElements = new List<ObjELEMENTDEVIS>();
        public decimal Frais { get; set; }
        public List<ObjELEMENTDEVIS> MyFournitures { get; set; }
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


        List<ObjELEMENTDEVIS> LesElementInit = new List<ObjELEMENTDEVIS>();

        public UcEtablissementDevisMt(CsDemande iddemande)
        {
            try
            {
                if (LayoutRoot != null)
                    LayoutRoot.Cursor = Cursors.Wait;
                InitializeComponent();
                this.CancelButton.IsEnabled = false ;
                this.Txt_DistanceExtension.Visibility = System.Windows.Visibility.Collapsed;
                this.labelDistExt.Visibility = System.Windows.Visibility.Collapsed;
                this.Btn_Taux.Visibility = System.Windows.Visibility.Collapsed;
                ChargeDetailDEvis(iddemande);
            }
            catch (Exception ex)
            {
                if (LayoutRoot != null)
                    LayoutRoot.Cursor = Cursors.Arrow;
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        public UcEtablissementDevisMt(int iddemande)
        {
            try
            {
                if (LayoutRoot != null)
                    LayoutRoot.Cursor = Cursors.Wait;
                InitializeComponent();
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

            try
            {
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GetDevisByNumIdDevisCompleted += (ssender, args) =>
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
                        RenseignerInformationsDevis(laDetailDemande);
                        RenseignerInformationsDemandeDevis(laDetailDemande);
                        this.tabControl_Consultation.SelectedItem = tabItemFournitures;
                    }
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
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
                    ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == laDemande.LaDemande.FK_IDCENTRE);
                    Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE.ToString()) ? laDemande.LaDemande.ORDRE.ToString() : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLECENTRE) ? laDemande.LaDemande.LIBELLECENTRE : string.Empty;
                    Txt_LibelleSite.Text = !string.IsNullOrEmpty(leCentre.LIBELLESITE) ? leCentre.LIBELLESITE : string.Empty;
                    Txt_LibelleProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLEPRODUIT) ? laDemande.LaDemande.LIBELLEPRODUIT : string.Empty;
                    Txt_Client.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CLIENT) ? laDemande.LaDemande.CLIENT : string.Empty;
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
                    Txt_LibelleRue.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLERUE) ? laDemande.Ag.LIBELLERUE : string.Empty;
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
        private void ChargeDetailDEvis(CsDemande  LaDem)
        {

            try
            {
                laDetailDemande = LaDem;
                laDemandeSelect = laDetailDemande.LaDemande;
                //RemplirListeDevis(laDetailDemande);
                RemplirListeMateriel(laDetailDemande);
                laDemandeSelect = laDetailDemande.LaDemande;
                LesElementInit = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(laDetailDemande.EltDevis);
                RenseignerInformationsDevis(laDetailDemande);
                RenseignerInformationsDemandeDevis(laDetailDemande);
                this.tabControl_Consultation.SelectedItem = tabItemFournitures;
            }
            catch (Exception ex)
            {

                Message.Show(ex.Message, "Demande");
            }

        }
        decimal CoutAvance = 0;

        void ChargerCoutDemande(CsDemande _Lademande,bool IsCoutDejaSaisi)
        {
            try
            {
                if (SessionObject.LstDesCoutDemande.Count != 0)
                {
                    string typedemande = _Lademande.LaDemande.TYPEDEMANDE;
                    if (_Lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                        _Lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementMt ||
                        _Lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonementExtention ||
                        _Lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance )
                        typedemande = SessionObject.Enumere.BranchementAbonement ;
                        List<CsCoutDemande> lesCoutDemande = Shared.ClasseMEthodeGenerique.RetourneListCopy<CsCoutDemande>( SessionObject.LstDesCoutDemande);
                        LstDesCoutsDemande = lesCoutDemande.Where(p => p.TYPEDEMANDE == typedemande).ToList();

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
                                CoutAvance = leCoutAvance.MONTANT.Value;

                        }
                        if (IsCoutDejaSaisi) return;
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
                            _element.FK_IDCOPER = leFraisParticipation.FK_IDCOPER;
                            _element.FK_IDFOURNITURE = null;
                            _element.FK_IDDEMANDE = _Lademande.LaDemande.PK_ID;
                            _element.ISFOURNITURE = true;
                            _element.ISPOSE = true;
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
                                _element.PRIX_UNITAIRE = CoutAvance != null ? (decimal)CoutAvance : 0;
                                _element.PRIX = item.MONTANT != null ? (decimal)item.MONTANT : 0;
                                _element.COUTFOURNITURE = item.MONTANT != null ? (decimal)item.MONTANT : 0;

                                if (item.COPER == SessionObject.Enumere.CoperCAU &&
                                    laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                                {
                                    _element.QUANTITE = int.Parse(laDetailDemande.LaDemande.PUISSANCESOUSCRITE.ToString());
                                    _element.MONTANTHT = (CoutAvance != null && _element.QUANTITE != null) ? (decimal)(CoutAvance * _element.QUANTITE) : 0;
                                    _element.PRIX_UNITAIRE = CoutAvance != null ? (decimal)CoutAvance : 0;

                                }
                                else
                                {
                                    _element.QUANTITE = 1;
                                    _element.MONTANTHT = (item.MONTANT != null && _element.QUANTITE != null) ? (int)_element.QUANTITE * (decimal)item.MONTANT : 0;
                                }
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
        public UcEtablissementDevisMt()
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
                this.CancelButton .IsEnabled = false;
                Enregistrer(laDetailDemande,true );
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
        private void btn_MiseEnAttente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.CancelButton.IsEnabled = false;
                Enregistrer(laDetailDemande, false );
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

                 AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                 clientDevis.ValiderDemandeCompleted += (ss, b) =>
                 {
                     this.CancelButton.IsEnabled = true;
                     if (b.Cancelled || b.Error != null)
                     {
                         string error = b.Error.Message;
                         Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                         return;
                     }
                     if (IsTransmetre)
                     {
                         List<string> codes = new List<string>();
                         codes.Add(laDemande.InfoDemande.CODE);
                         Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
                         //List<CsUtilisateur> leUser = new List<CsUtilisateur>();
                         //if (laDemande.InfoDemande != null && laDemande.InfoDemande.CODE != null)
                         //{
                         //    foreach (CsUtilisateur item in laDemande.InfoDemande.UtilisateurEtapeSuivante)
                         //        leUser.Add(item);
                         //    Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", laDemande.LaDemande.NUMDEM, laDemande.LaDemande.LIBELLETYPEDEMANDE);
                         //}

                     }
                     else
                     {
                         Message.ShowInformation("Mise en attente effectuée avec succès", "Devis");
                         this.DialogResult = false;
                     }
                     LayoutRoot.Cursor = Cursors.Arrow;
                 };
                 clientDevis.ValiderDemandeAsync(laDemande);


                //montantTotal = !string.IsNullOrEmpty(Txt_MontantTotal.Text) ? decimal.Parse(Txt_MontantTotal.Text) : 0;
                //UcBilanEtablissementDevis frmBilanEtablissementDevis = new UcBilanEtablissementDevis(this.laDetailDemande, this.MyElements, montantTotal,false );
                //frmBilanEtablissementDevis.ExecMode = ModeExecution;
                //frmBilanEtablissementDevis.Taxe = this.Taxe;
                //frmBilanEtablissementDevis.Distance = (decimal)this.laDetailDemande.Branchement.LONGBRT;
                //frmBilanEtablissementDevis.Schema = doc;
                //frmBilanEtablissementDevis.Closed += new EventHandler(frmBilanEtablissementDevis_Closed);
                //frmBilanEtablissementDevis.Show();

            }
            catch (Exception ex)
            {
                this.CancelButton.IsEnabled = true ;
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
                        if (MyElements == null)
                            MyElements = new List<ObjELEMENTDEVIS>();
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
        Dictionary<CsRubriqueDevis, ObjELEMENTDEVIS> TotalRubrique = new Dictionary<CsRubriqueDevis, ObjELEMENTDEVIS>();

        private void RemplirListeMaterielMT(List<ObjELEMENTDEVIS> lstEltDevis, List<CsRubriqueDevis> leRubriques)
        {
            ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
            leSeparateur.LIBELLE = "----------------------------------";
            leSeparateur.ISDEFAULT = true;
            List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();
            List<ObjELEMENTDEVIS> lstFourTVA = new List<ObjELEMENTDEVIS>();
            int CoperTrv = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;

            foreach (CsRubriqueDevis item in leRubriques.Where(t=>t.CODE != "004").ToList())
            {
                List<ObjELEMENTDEVIS> lstFourRubrique = lstEltDevis.Where(t => t.FK_IDRUBRIQUEDEVIS == item.PK_ID).ToList();
                if (lstFourRubrique != null && lstFourRubrique.Count != 0)
                {
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
                        leIncidence.ISGENERE  = true;
                        leIncidence.FK_IDMATERIELDEVIS = leIncidence.FK_IDMATERIELDEVIS ;
                        leIncidence.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;
                        leIncidence.MONTANTHT = leIncidence.QUANTITE * (leIncidence.COUTUNITAIRE_FOURNITURE + leIncidence.COUTUNITAIRE_POSE);
                        leIncidence.MONTANTTTC = leIncidence.MONTANTHT;
                        if (lstFourRubrique.FirstOrDefault(t => t.ISGENERE) == null)
                           if (lstEltDevis.FirstOrDefault(t => t.MONTANTHT < 0) == null)
                            lstFourRubrique.Add(leIncidence);
                        MontantLigne = lstFourRubrique.Sum(t => t.MONTANTHT );
                    }
                    decimal? MontantTotRubriqueHt = lstFourRubrique.Sum(t => t.MONTANTHT);
                    decimal? MontantTotRubriqueTaxe = lstFourRubrique.Sum(t => t.MONTANTTAXE);
                    decimal? MontantTotRubrique = lstFourRubrique.Sum(t => t.MONTANTTTC);

                    if (MontantTotRubriqueHt <0)
                    { MontantTotRubriqueHt = 0; MontantTotRubriqueTaxe = 0; MontantTotRubrique = 0; }
                        ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                        leResultatBranchanchement.LIBELLE = "SOUS TOTAL  " + item.LIBELLE;
                        leResultatBranchanchement.ISGENERE = true;
                        leResultatBranchanchement.ISDEFAULT = true;
                        leResultatBranchanchement.MONTANTHT = MontantTotRubriqueHt;
                        leResultatBranchanchement.MONTANTTAXE = MontantTotRubriqueTaxe;
                        leResultatBranchanchement.MONTANTTTC = MontantTotRubrique;
                        lstFourTVA.Add(leResultatBranchanchement);
                        TotalRubrique.Add(item, leResultatBranchanchement);

                        lstFourgenerale.AddRange(lstFourRubrique);
                        lstFourgenerale.Add(leSeparateur);
                        lstFourgenerale.Add(leResultatBranchanchement);
                        lstFourgenerale.Add(new ObjELEMENTDEVIS()
                        {
                            LIBELLE = "    ",
                            ISGENERE = true
                        });
                }
            }
            ObjELEMENTDEVIS leTHT = new ObjELEMENTDEVIS();
            ObjELEMENTDEVIS leTVA = new ObjELEMENTDEVIS();
            if (lstFourgenerale.Count != 0)
            {
                decimal? MontantTotRubrique = 0;
                decimal? MontantTotRubriqueHt =0;
                decimal? MontantTotRubriqueTaxe =0;

                foreach (var item in TotalRubrique.Where(o=>o.Key .CODE == SessionObject.Enumere.LIGNEHTA || 
                                                            o.Key .CODE == SessionObject.Enumere.POSTEHTABT ||
                                                            o.Key .CODE == SessionObject.Enumere.LIGNEBT).ToList())
                {
                    MontantTotRubrique =MontantTotRubrique + item.Value.MONTANTTTC;
                    MontantTotRubriqueHt = MontantTotRubriqueHt + item.Value.MONTANTHT;
                    MontantTotRubriqueTaxe = MontantTotRubriqueTaxe + item.Value.MONTANTTAXE; 
                }

                if (MontantTotRubriqueHt < 0) 
                { 
                    MontantTotRubriqueHt = 0; 
                    MontantTotRubriqueTaxe = 0; 
                }
                ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
                leResultatGeneral.LIBELLE = "TOTAL FACTURE TRAVAUX ";
                leResultatGeneral.ISDEFAULT = true;
                leResultatGeneral.ISGENERE = true;
                leResultatGeneral.MONTANTHT = MontantTotRubriqueHt;
                lstFourgenerale.Add(leSeparateur);
                lstFourgenerale.Add(leResultatGeneral);

                ObjELEMENTDEVIS leSurveillance = new ObjELEMENTDEVIS();
                if (lstFourTVA != null && lstFourTVA.Count != 0)
                {
                    leSurveillance.LIBELLE = "ETUDE ET SURVEILLANCE 10 %";
                    leSurveillance.ISFORTRENCH = true;
                    leSurveillance.QUANTITE = 1;
                    leSurveillance.ISGENERE = true;

                    leSurveillance.MONTANTHT = MontantTotRubriqueHt * (decimal)(0.10); ;
                    leSurveillance.MONTANTTAXE = MontantTotRubriqueTaxe * (decimal)(0.10); ;
                    leSurveillance.MONTANTTTC = MontantTotRubrique * (decimal)(0.10);

                    leSurveillance.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == "093").PK_ID;
                    leSurveillance.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;
                    lstFourgenerale.Add(leSurveillance);
                    lstFourTVA.Add(leSurveillance);
                }
             
                ObjELEMENTDEVIS lstFourEnsembleCmpt = lstEltDevis.FirstOrDefault (t => t.FK_IDRUBRIQUEDEVIS == 5);
                if (lstFourEnsembleCmpt != null && lstFourEnsembleCmpt.MONTANTHT  != 0)
                {
                    ObjELEMENTDEVIS leResultatComptage = new ObjELEMENTDEVIS();
                    leResultatComptage.LIBELLE = SessionObject.LstRubriqueDevis.FirstOrDefault(k => k.PK_ID == 5).LIBELLE;
                    leResultatComptage.ISDEFAULT = true;
                    //leResultatComptage.IsCOLORIE = true;
                    leResultatComptage.QUANTITE = lstEltDevis.Where(t => t.FK_IDRUBRIQUEDEVIS == 5).Count();
                    leResultatComptage.MONTANTTAXE  =lstFourEnsembleCmpt.MONTANTTAXE != null ? lstFourEnsembleCmpt.MONTANTTAXE :0 ;
                    leResultatComptage.MONTANTHT = lstFourEnsembleCmpt.MONTANTHT != null ? lstFourEnsembleCmpt.MONTANTHT : 0;
                    leResultatComptage.MONTANTTTC = leResultatComptage.MONTANTTAXE + leResultatComptage.MONTANTHT;
                    leResultatComptage.FK_IDRUBRIQUEDEVIS = 5;
                    leResultatComptage.FK_IDCOPER = CoperTrv;
                    leResultatComptage.FK_IDMATERIELDEVIS = lstFourEnsembleCmpt.FK_IDMATERIELDEVIS != null ? lstFourEnsembleCmpt.FK_IDMATERIELDEVIS : null;
                    leResultatComptage.FK_IDTAXE = lstFourEnsembleCmpt.FK_IDTAXE;
                    
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatComptage);
                    lstFourTVA.Add(leResultatComptage);

                }
                if (lstFourTVA != null && lstFourTVA.Count != 0)
                {
                    leTHT.LIBELLE = "TOTAL HT ";
                    leTHT.ISFORTRENCH = true;
                    leTHT.ISGENERE = true;
                    leTHT.MONTANTHT = lstFourTVA.Sum(y => y.MONTANTHT)  ;
                    lstFourgenerale.Add(leTHT);

                }
                if (lstFourTVA != null && lstFourTVA.Count != 0)
                {
                    leTVA.LIBELLE = "TVA 18 % ";
                    leTVA.ISFORTRENCH = true;
                    leTVA.ISGENERE = true;
                    leTVA.MONTANTHT = lstFourTVA.Sum(y => y.MONTANTHT) * (decimal)(0.18); ;
                    lstFourgenerale.Add(leTVA);

                }
            }
            ObjELEMENTDEVIS leResultatGeneralaVANCE = new ObjELEMENTDEVIS();
            leResultatGeneralaVANCE.LIBELLE = "Avance sur consommation ";
            leResultatGeneralaVANCE.ISDEFAULT = true;
            leResultatGeneralaVANCE.ISGENERE = true;
            leResultatGeneralaVANCE.QUANTITE =int.Parse(laDetailDemande.LaDemande.PUISSANCESOUSCRITE.ToString()) ;
            leResultatGeneralaVANCE.MONTANTHT = laDetailDemande.LaDemande.PUISSANCESOUSCRITE * CoutAvance;
            leResultatGeneralaVANCE.MONTANTTTC = leResultatGeneralaVANCE.MONTANTHT;
            leResultatGeneralaVANCE.COUTUNITAIRE_FOURNITURE = CoutAvance;
            leResultatGeneralaVANCE.PRIX_UNITAIRE = CoutAvance;
            leResultatGeneralaVANCE.MONTANTTAXE  = 0;
            CsCoutDemande leCoutAvance = LstDesCoutsDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperCAU);
            if (leCoutAvance != null)
                leResultatGeneralaVANCE.COUTFOURNITURE = leCoutAvance.MONTANT.Value  ;

            leResultatGeneralaVANCE.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperCAU ).PK_ID;
            leResultatGeneralaVANCE.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault (t => t.CODE   == SessionObject.Enumere.CodeSansTaxe ).PK_ID;
            
            lstFourgenerale.Add(leSeparateur);
            lstFourgenerale.Add(leResultatGeneralaVANCE);

            ObjELEMENTDEVIS leResultatGeneralttc = new ObjELEMENTDEVIS();
            leResultatGeneralttc.LIBELLE = "TOTAL GENERAL TTC ";
            leResultatGeneralttc.MONTANTHT = (leTHT.MONTANTHT == null ? 0 : leTHT.MONTANTHT) + (leTVA.MONTANTHT==null ?0:leTVA.MONTANTHT ) + ( leResultatGeneralaVANCE.MONTANTHT==null ?0:leResultatGeneralaVANCE.MONTANTHT);
            //leResultatGeneralttc.IsCOLORIE = true;
            leResultatGeneralttc.ISDEFAULT = true;
            leResultatGeneralttc.ISGENERE = true;
            lstFourgenerale.Add(leResultatGeneralttc);

            MyElements.Clear();
            this.MyElements.AddRange(lstFourgenerale.Where(t=>t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 198).ToList());
            this.dataGridElementDevis.ItemsSource = null;
            this.dataGridElementDevis.ItemsSource = lstFourgenerale.ToList();

            this.Txt_MontantTotal.Text = leResultatGeneralttc.MONTANTHT.Value.ToString(SessionObject.FormatMontant);
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
                        Txt_MontantTotal.Text = CalculerCoutTotal().ToString(SessionObject.FormatMontant);
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

                        //CsCoutDemande leCoutAvance = LstDesCoutsDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperCAU);
                        //if (leCoutAvance != null)
                        //    elementDevis.COUTFOURNITURE  = leCoutAvance.MONTANT.Value  ;
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
                            {
                                EltDeLagrid.Remove(ObjSelect);
                                MyElements.Clear();
                                this.dataGridElementDevis.ItemsSource = null;
                                RemplirListeMaterielMT(EltDeLagrid.Where(t => t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 198).ToList(), SessionObject.LstRubriqueDevis);
                            }
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
                //MenuContextuel.IsEnabled = dataGridElementDevis.SelectedItem != null;
                //MenuContextuel.UpdateLayout();
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
                        if (elt.CODECOPER  == SessionObject.Enumere.CoperEtudeEtSurveillance)
                            this.Btn_Taux.Visibility = System.Windows.Visibility.Visible ;
                        else
                            this.Btn_Taux.Visibility = System.Windows.Visibility.Collapsed;

                        //this.selectedElement = elt;
                        //this.Btn_Supprimer.IsEnabled = (selectedElement.ISDEFAULT != true) ? true : false;
                        //this.Txt_Quantite.IsReadOnly = false;
                        //this.Txt_Quantite.Text = selectedElement.QUANTITE.ToString();
                        //this.Txt_Quantite.SelectAll();
                        //this.Txt_Quantite.Focus();
                    }
                    //else
                    //{
                    //    this.Txt_Quantite.IsReadOnly = true;
                    //    Txt_Quantite.Text = string.Empty;
                    //}
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
                AcceuilServiceClient Serviceclient = new AcceuilServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Accueil"));
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
                                ChargerCoutDemande(laDemandedevis, true);
                                RemplirListeMaterielMT(MyElements, SessionObject.LstRubriqueDevis);
                                return;
                            }
                            
                            #region Cout detail
                            if (laDemandedevis.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                                laDemandedevis.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance  )
                            {
                                if (laDemandedevis.LaDemande.PUISSANCESOUSCRITE == laDetailDemande.Abonne.PUISSANCE)
                                    return ;
                            }
                                ChargerCoutDemande(laDemandedevis,false );
                                dataGridElementDevis.ItemsSource = null;
                                dataGridElementDevis.ItemsSource = donnesDatagrid;
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

        private void dataGridElementDevis_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                var dmdRow = e.Row.DataContext as ObjELEMENTDEVIS;
                if (dmdRow != null)
                {
                    if (dmdRow.QUANTITE == 0 || dmdRow.QUANTITE == null)
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Red);
                        e.Row.Foreground = SolidColorBrush;
                    }
                    else
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Black);
                        e.Row.Foreground = SolidColorBrush;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btn_Imprimer_Click(object sender, RoutedEventArgs e)
        {
            List<ObjELEMENTDEVIS> lstElementDevis = LireElements();
            EdiderDevisMt(lstElementDevis,SessionObject.LstRubriqueDevis);
        }
        //private void EdiderDevisMt( List<CsRubriqueDevis> leRubriques)
        //{
        //    List<ObjELEMENTDEVIS> lstEltDevis = LireElements();
        //    decimal montantTotal = lstEltDevis.Where(u => u.MONTANTTTC != null).Sum(t => (decimal)(t.MONTANTTTC));
        //    List<CsEditionDevis> LstDesRubriqueDevis = new List<CsEditionDevis>();
        //    foreach (ObjELEMENTDEVIS item in lstEltDevis.ToList())
        //    {
        //        CsEditionDevis LaRubriqueDevis = new CsEditionDevis();
        //        LaRubriqueDevis.CENTRE = laDetailDemande.LaDemande.CENTRE;
        //        LaRubriqueDevis.PRODUIT = laDetailDemande.LaDemande.LIBELLEPRODUIT;
        //        LaRubriqueDevis.TYPEDEMANDE = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
        //        LaRubriqueDevis.COMMUNUE = laDetailDemande.Ag.LIBELLECOMMUNE;
        //        LaRubriqueDevis.QUARTIER = laDetailDemande.Ag.LIBELLEQUARTIER;
        //        LaRubriqueDevis.NOM = laDetailDemande.LeClient.NOMABON;
        //        LaRubriqueDevis.NUMDEMANDE = laDetailDemande.LaDemande.NUMDEM;
        //        LaRubriqueDevis.LATITUDE = laDetailDemande.Branchement.LATITUDE;
        //        LaRubriqueDevis.LONGITUDE = laDetailDemande.Branchement.LONGITUDE;
        //        LaRubriqueDevis.DESIGNATION = item.DESIGNATION;

        //        LaRubriqueDevis.QUANTITE = Convert.ToDecimal(item.QUANTITE);
        //        if (item.PRIX_UNITAIRE == null ) item.PRIX_UNITAIRE = item.MONTANTHT;
        //        LaRubriqueDevis.PRIXUNITAIRE = item.PRIX_UNITAIRE.Value;
        //        LaRubriqueDevis.MONTANTHT = (decimal)(item.MONTANTTTC);
        //        LaRubriqueDevis.PRIXTVA = (montantTotal * 18) / 100;
        //        LaRubriqueDevis.TOTALDEVIS = montantTotal;
        //        if (item.FK_IDRUBRIQUEDEVIS != null)
        //            LaRubriqueDevis.SECTION = leRubriques.FirstOrDefault(t => t.PK_ID == item.FK_IDRUBRIQUEDEVIS).LIBELLE;
        //        else
        //            LaRubriqueDevis.SECTION = "";

        //        LstDesRubriqueDevis.Add(LaRubriqueDevis);
        //    }
        //    Dictionary<string, string> param = new Dictionary<string, string>();
        //    param.Add("pDate", "");
        //    param.Add("pNumeroFacture", "");
        //    param.Add("pClient", "");
        //    param.Add("pObjet1", "");
        //    param.Add("pObjet2", "");
        //    Utility.ActionDirectOrientation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevis, param, SessionObject.CheminImpression, "DevisMt", "Accueil", true);
        //}
        private void EdiderDevisMt(List<ObjELEMENTDEVIS> lstEltDevis, List<CsRubriqueDevis> leRubriques)
        {
            ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
            List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();
            List<ObjELEMENTDEVIS> lstFourTVA = new List<ObjELEMENTDEVIS>();
            int CoperTrv = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
            Dictionary<CsRubriqueDevis, ObjELEMENTDEVIS> TotalRubrique = new Dictionary<CsRubriqueDevis, ObjELEMENTDEVIS>();
            foreach (CsRubriqueDevis item in leRubriques.Where(t => t.CODE != "004").ToList())
            {
                List<ObjELEMENTDEVIS> lstFourRubrique = lstEltDevis.Where(t => t.FK_IDRUBRIQUEDEVIS == item.PK_ID).ToList();
                if (lstFourRubrique != null && lstFourRubrique.Count != 0)
                {
                    lstFourRubrique.ForEach(t => t.FK_IDCOPER = CoperTrv);
                    if (item.CODE == SessionObject.Enumere.LIGNEHTA && laDetailDemande.Branchement.CODEBRT == "0001")
                    {

                        decimal? MontantLigne = 0;

                        ObjELEMENTDEVIS leIncidence = ListeFournitureExistante.FirstOrDefault(t => t.ISGENERE == true);
                        leIncidence.FK_IDRUBRIQUEDEVIS = item.PK_ID;
                        leIncidence.QUANTITE = 1;
                        leIncidence.FK_IDCOPER = CoperTrv;
                        leIncidence.MONTANTTAXE = 0;
                        leIncidence.MONTANTHT = 0;
                        leIncidence.FK_IDMATERIELDEVIS = leIncidence.FK_IDMATERIELDEVIS;
                        leIncidence.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;
                        leIncidence.MONTANTHT = leIncidence.QUANTITE * (leIncidence.COUTUNITAIRE_FOURNITURE + leIncidence.COUTUNITAIRE_POSE);
                        leIncidence.MONTANTTTC = leIncidence.MONTANTHT;

                        if (lstFourRubrique.FirstOrDefault(t => t.ISGENERE) == null)
                            if (lstEltDevis.FirstOrDefault(t => t.MONTANTHT < 0) == null)
                                lstFourRubrique.Add(leIncidence);
                        MontantLigne = lstFourRubrique.Sum(t => t.MONTANTHT);
                    }
                    decimal? MontantTotRubriqueHt = lstFourRubrique.Sum(t => t.MONTANTHT);
                    decimal? MontantTotRubriqueTaxe = lstFourRubrique.Sum(t => t.MONTANTTAXE);
                    decimal? MontantTotRubrique = lstFourRubrique.Sum(t => t.MONTANTTTC);

                    if (MontantTotRubriqueHt < 0)
                    { MontantTotRubriqueHt = 0; MontantTotRubriqueTaxe = 0;
                     lstFourRubrique.ForEach(u => u.MONTANT  = 0);
                    }
                    else
                        lstFourRubrique.ForEach(u => u.MONTANT = u.MONTANTHT.Value  );

                    lstFourRubrique.ForEach(u => u.DESIGNATION = item.LIBELLE);
                    lstFourRubrique.ForEach(u => u.CLIENT = laDetailDemande.LaDemande.CENTRE + " " + laDetailDemande.LaDemande.CLIENT + " " + laDetailDemande.LaDemande.ORDRE);
                    lstFourRubrique.ForEach(u => u.CODE = laDetailDemande.LeClient.NOMABON);
                    lstFourRubrique.ForEach(u => u.CODECOPER = laDetailDemande.LeClient.ADRMAND1);
                    lstFourRubrique.ForEach(u => u.CODEMATERIELDEVIS  = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE);

                    lstFourgenerale.AddRange(lstFourRubrique);
                    ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                    leResultatBranchanchement.MONTANTHT = MontantTotRubriqueHt;
                    leResultatBranchanchement.MONTANT = MontantTotRubriqueHt.Value ;
                    leResultatBranchanchement.MONTANTTAXE = MontantTotRubriqueTaxe;
                    leResultatBranchanchement.MONTANTTTC = MontantTotRubrique;
                    lstFourTVA.Add(leResultatBranchanchement);
                    TotalRubrique.Add(item, leResultatBranchanchement);
                }
            }
            ObjELEMENTDEVIS leTHT = new ObjELEMENTDEVIS();
            ObjELEMENTDEVIS leTVA = new ObjELEMENTDEVIS();
            if (lstFourgenerale.Count != 0)
            {
                decimal? MontantTotRubrique = 0;
                decimal? MontantTotRubriqueHt = 0;
                decimal? MontantTotRubriqueTaxe = 0;

                foreach (var item in TotalRubrique.Where(o => o.Key.CODE == SessionObject.Enumere.LIGNEHTA ||
                                                              o.Key.CODE == SessionObject.Enumere.POSTEHTABT ||
                                                              o.Key.CODE == SessionObject.Enumere.LIGNEBT).ToList())
                {
                    MontantTotRubrique = MontantTotRubrique + item.Value.MONTANTTTC;
                    MontantTotRubriqueHt = MontantTotRubriqueHt + item.Value.MONTANTHT;
                    MontantTotRubriqueTaxe = MontantTotRubriqueTaxe + item.Value.MONTANTTAXE;
                }

                ObjELEMENTDEVIS leSurveillance = new ObjELEMENTDEVIS();
                if (lstFourTVA != null && lstFourTVA.Count != 0)
                {
                    if (lstFourgenerale.FirstOrDefault(o => o.ISADDITIONAL == true) != null)
                        leSurveillance.ISADDITIONAL  = true;

                    leSurveillance.LIBELLE = "ETUDE ET SURVEILLANCE 10 %";
                    leSurveillance.ISFORTRENCH = true;
                    leSurveillance.QUANTITE = 1;
                    leSurveillance.ISGENERE = true;
                    leSurveillance.FK_IDRUBRIQUEDEVIS = 5;
                    leSurveillance.DESIGNATION  = "Etude surveillance + Comptage";

                    leSurveillance.MONTANTHT = MontantTotRubriqueHt * (decimal)(0.10); ;
                    leSurveillance.MONTANT = leSurveillance.MONTANTHT.Value ;
                    leSurveillance.MONTANTTAXE = MontantTotRubriqueTaxe * (decimal)(0.10); ;
                    leSurveillance.MONTANTTTC = MontantTotRubrique * (decimal)(0.10);

                    leSurveillance.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperEtudeEtSurveillance).PK_ID;
                    leSurveillance.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;
                    lstFourgenerale.Add(leSurveillance);
                    lstFourTVA.Add(leSurveillance);
                }

                ObjELEMENTDEVIS lstFourEnsembleCmpt = lstEltDevis.FirstOrDefault(t => t.FK_IDRUBRIQUEDEVIS == 5);
                if (lstFourEnsembleCmpt != null && lstFourEnsembleCmpt.MONTANTHT != 0)
                {
                    ObjELEMENTDEVIS leResultatComptage = new ObjELEMENTDEVIS();
                    leResultatComptage.LIBELLE = SessionObject.LstRubriqueDevis.FirstOrDefault(k => k.PK_ID == 5).LIBELLE;
                    leResultatComptage.ISDEFAULT = true;
                    leResultatComptage.QUANTITE = lstEltDevis.Where(t => t.FK_IDRUBRIQUEDEVIS == 5).Count();
                    leResultatComptage.MONTANTTAXE = lstFourEnsembleCmpt.MONTANTTAXE != null ? lstFourEnsembleCmpt.MONTANTTAXE : 0;
                    leResultatComptage.MONTANTHT = lstFourEnsembleCmpt.MONTANTHT != null ? lstFourEnsembleCmpt.MONTANTHT : 0;
                    leResultatComptage.MONTANTTTC = leResultatComptage.MONTANTTAXE + leResultatComptage.MONTANTHT;
                    leResultatComptage.MONTANT = leResultatComptage.MONTANTHT.Value;

                    leResultatComptage.FK_IDRUBRIQUEDEVIS = 5;
                    leResultatComptage.FK_IDCOPER = CoperTrv;
                    leResultatComptage.FK_IDMATERIELDEVIS = lstFourEnsembleCmpt.FK_IDMATERIELDEVIS != null ? lstFourEnsembleCmpt.FK_IDMATERIELDEVIS : null;
                    leResultatComptage.FK_IDTAXE = lstFourEnsembleCmpt.FK_IDTAXE;
                    leResultatComptage.FK_IDRUBRIQUEDEVIS = 5;
                    leResultatComptage.DESIGNATION = "Etude surveillance + Comptage";
                    lstFourgenerale.Add(leResultatComptage);
                    lstFourTVA.Add(leResultatComptage);

                }
           
                if (lstFourTVA != null && lstFourTVA.Count != 0)
                {
                    leTVA.DESIGNATION = "TVA 18 % ";
                    leTVA.LIBELLE = "TVA 18 % ";
                    leTVA.MONTANTHT = lstFourTVA.Sum(y => y.MONTANTHT) * (decimal)(0.18);
                    leTVA.MONTANT  = leTVA.MONTANTHT.Value ;

                    leTVA.FK_IDRUBRIQUEDEVIS = 6;
                    lstFourgenerale.Add(leTVA);

                }
            }
            ObjELEMENTDEVIS leResultatGeneralaVANCE = new ObjELEMENTDEVIS();
            leResultatGeneralaVANCE.LIBELLE = "Avance sur consommation ";
            leResultatGeneralaVANCE.QUANTITE = int.Parse(laDetailDemande.LaDemande.PUISSANCESOUSCRITE.ToString());
            leResultatGeneralaVANCE.MONTANTHT = laDetailDemande.LaDemande.PUISSANCESOUSCRITE * CoutAvance;
            leResultatGeneralaVANCE.MONTANTTTC = leResultatGeneralaVANCE.MONTANTHT;
            leResultatGeneralaVANCE.MONTANT = leResultatGeneralaVANCE.MONTANTHT.Value ;
            leResultatGeneralaVANCE.COUTUNITAIRE_FOURNITURE = CoutAvance;
            leResultatGeneralaVANCE.PRIX_UNITAIRE = CoutAvance;
            leResultatGeneralaVANCE.MONTANTTAXE = 0;
            leResultatGeneralaVANCE.FK_IDRUBRIQUEDEVIS = 5;
            leResultatGeneralaVANCE.DESIGNATION = "Etude surveillance + Comptage";
            CsCoutDemande leCoutAvance = LstDesCoutsDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperCAU);
            if (leCoutAvance != null)
                leResultatGeneralaVANCE.COUTFOURNITURE = leCoutAvance.MONTANT.Value;

            leResultatGeneralaVANCE.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperCAU).PK_ID;
            leResultatGeneralaVANCE.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;
            if (lstFourgenerale.FirstOrDefault(o => o.ISADDITIONAL == true) != null)
                leResultatGeneralaVANCE.ISADDITIONAL = true;
            lstFourgenerale.Add(leResultatGeneralaVANCE);
            Utility.ActionDirectOrientation<ServicePrintings.ObjELEMENTDEVIS, ObjELEMENTDEVIS>(lstFourgenerale, null, SessionObject.CheminImpression, "LeDEvisMt", "Accueil", true);
             
        }

        private void dgMyDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                var dmdRow = e.Row.DataContext as ObjELEMENTDEVIS;
                if (dmdRow != null)
                {
                    if (dmdRow.QUANTITE == 0 || dmdRow.QUANTITE == null)
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Red);
                        e.Row.Foreground = SolidColorBrush;
                    }
                    else
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Black);
                        e.Row.Foreground = SolidColorBrush;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btn_taux(object sender, RoutedEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<ObjELEMENTDEVIS>;
            if (dg.SelectedItem != null)
            {
                ObjELEMENTDEVIS SelectedObject = (ObjELEMENTDEVIS)dg.SelectedItem;
                UcSaisiQuantite ctrl = new UcSaisiQuantite(SelectedObject, true);
                ctrl.Closed += ctrl_ClosedTaux;
                ctrl.Show();
               
            }
        }
    
        void ctrl_ClosedTaux(object sender, EventArgs e)
        {
            UcSaisiQuantite ctrs = sender as UcSaisiQuantite;
            if (ctrs.isOkClick)
            {
            }
        }


        private void ModifierTaux()
        {
            try
            {

                if (this.dataGridElementDevis.SelectedItem != null)
                {
                    var mBoxControl = new MessageBoxControl.MessageBoxChildWindow(this.Title.ToString(), Languages.msgConfirmSuppression, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    mBoxControl.OnMessageBoxClosed += (_, result) =>
                    {
                        if (mBoxControl.Result == MessageBoxResult.OK)
                        {
                            List<ObjELEMENTDEVIS> EltDeLagrid = dataGridElementDevis.ItemsSource as List<ObjELEMENTDEVIS>;
                            ObjELEMENTDEVIS select = dataGridElementDevis.SelectedItem as ObjELEMENTDEVIS;
                            ObjELEMENTDEVIS ObjSelect = EltDeLagrid.FirstOrDefault(t => t.FK_IDMATERIELDEVIS == select.FK_IDMATERIELDEVIS && t.FK_IDRUBRIQUEDEVIS == select.FK_IDRUBRIQUEDEVIS);
                            if (ObjSelect != null)
                            {
                                EltDeLagrid.Remove(ObjSelect);
                                MyElements.Clear();
                                this.dataGridElementDevis.ItemsSource = null;
                                RemplirListeMaterielMT(EltDeLagrid.Where(t => t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 198).ToList(), SessionObject.LstRubriqueDevis);
                            }
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
    }
}


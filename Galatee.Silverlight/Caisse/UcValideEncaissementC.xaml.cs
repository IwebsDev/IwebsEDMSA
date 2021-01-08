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
using Galatee.Silverlight.ServiceCaisse;
using Galatee.Silverlight.Shared;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Linq;
using Galatee.Silverlight.Resources.Caisse;
using System.Collections.ObjectModel;
using System.Windows.Printing;
//using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Caisse
{
    public partial class UcValideEncaissementC : ChildWindow
    {
        List<CsLclient> ListeFactureAreglee = new List<CsLclient>();
        IList<CsModereglement> reglemetDataSource = null;
        List<CsLclient> LstDesReglementAValider = new List<CsLclient>();

        //CaisseServiceClient srv;
        CsFacture _Facture;
        CsModereglement _facs;
        CsBanque  _bankselectioner = new CsBanque ();
        CsLclient  _LeReglement;

        public bool ValidationEditionfacture = false;
        bool reject = true;
        bool IsSaisiCheque = false;
        decimal? MontantFacture = 0;
        decimal? MantantRendu = 0;
        decimal InitValue = 0;
  
        decimal montantRecuInit = 0;
        int NbreClient = 0;

        string OperationCaisse = string.Empty;
        string _ModePayement = string.Empty;
        string NumFactNaf = string.Empty;
        string montantInitialCollected = string.Empty;

        public event EventHandler Closed;

        private string ClientRefence;
        decimal montantEspecesInitial = 0; //ZEG
        private void translateControls()
        {
            //this.groubox.Header = Langue.Mode_paiement;
            this.Chk_Cash.Content = Langue.Chk_cash;
            this.label4.Content = Langue.Montant_due ;
            this.label2.Content = Langue.MontantPayer;
            this.label3.Content = Langue.Lbl_stamp_duty;
            this.Chk_Cheque.Content = Langue.Chk_checque;
            this.label1.Content = Langue.Lbl_bank;
            this.label5.Content = Langue.Montant;
            this.label6.Content = Langue.Lbl_checque_number;
            this.Chk_Autre.Content = Langue.Chk_other;
            this.label7.Content = Langue.Caisse;
            this.label8.Content = Langue.Num_Recu ;
            this.label9.Content = Langue.Caissier;
            //this.Btn_Printer.Content = Langue.Btn_printer;
            //this.headeredContentControl6.Header = Langue.Paiement;
            this.lbl_MontantEspeceTotal.Content = Langue.Montant_recu;
            this.label10.Content = Langue.To_return;
            this.lbl_MontantEspeceTimbre.Content = Langue.Montant_due;
            this.lbl_MontantEspecePaye.Content = Langue.lbl_MontantHT;
            this.OKButton.Content = Langue.Btn_ok;
            this.CancelButton.Content = Langue.CancelButton;
        }

        public bool Yes
        {
            get { return reject; }
            set { reject = value; }
        }
        bool EstFactureNAFsaisi = false;
        decimal? montantNAFSaisi = null;
        List<CsLclient> _factureNAFSaisi = new List<CsLclient>();
        public UcValideEncaissementC()
        {
            InitializeComponent();
            translateControls();
            SessionObject.ListeControlesCaisse.Add(this);
           //this.TxtImprimante.Text = SessionObject.DefaultPrinter;
           RemplirModesDePaiement();
           SessionObject.ListeControlesCaisse.Add(this);
        }


        public UcValideEncaissementC(List<CsLclient> _ListeFactureAreglee, string _OperationCaisse)
        {
            InitializeComponent();
            translateControls();
            SessionObject.ListeControlesCaisse.Add(this);
            ListeFactureAreglee = _ListeFactureAreglee ;
            OperationCaisse = _OperationCaisse;
            //this.TxtImprimante.Text = SessionObject.DefaultPrinter;
            RemplirModesDePaiement();
            SessionObject.ListeControlesCaisse.Add(this);
            Initialisationctrl(_ListeFactureAreglee);
            Btn_AjouterAutre.IsEnabled = false;


        }
        public UcValideEncaissementC(List<CsLclient> _ListeFactureAreglee, List<CsLclient> factureNAFSaisi, string _OperationCaisse, bool pSaisiFactureNAF)
        {
            InitializeComponent();
            translateControls();
            _factureNAFSaisi = factureNAFSaisi;
            SessionObject.ListeControlesCaisse.Add(this);
            ListeFactureAreglee = _ListeFactureAreglee;
            OperationCaisse = _OperationCaisse;
            //this.TxtImprimante.Text = SessionObject.DefaultPrinter;
            RemplirModesDePaiement();
            EstFactureNAFsaisi = pSaisiFactureNAF;
            montantNAFSaisi = factureNAFSaisi.Sum(f => f.MONTANT);
            SessionObject.ListeControlesCaisse.Add(this);

        }

        private void RemplirModesDePaiement()
        {
            this.cbo_OtherPaiement.ItemsSource = SessionObject.ListeModesReglement.Where(t=>t.CODE != "1" && t.CODE !="2").ToList() ;
        }

        private List<CsModereglement> SelectModeDePaiement(bool IsOverpayement)
        {
            try
            {
                List<CsModereglement> _ListeDesModePaiement = new List<CsModereglement>();
                if ((this.Chk_Cash.IsChecked == true))
                {
                    if (!string.IsNullOrEmpty(this.Txt_MontantEspece.Text) && decimal.Parse(this.Txt_MontantEspece.Text) != 0)
                    {
                        CsModereglement _LReglement = new CsModereglement();
                        _LReglement = SessionObject.ListeModesReglement.FirstOrDefault(m => m.CODE == SessionObject.Enumere.ModePayementEspece);
                        if (_LReglement != null)
                        {

                            if (!IsOverpayement)
                            {
                                if (Convert.ToDecimal(this.Txt_MontantFacture.Text) != 0)
                                {
                                    _LReglement.MONTANT = System.Convert.ToDecimal(this.Txt_MontantEspece.Text);
                                    MantantRendu = System.Convert.ToDecimal(this.txt_MontantRendu.Text);
                                }
                                else // cas de paiement anticipé
                                {

                                    _LReglement.MONTANT = System.Convert.ToDecimal(this.Txt_MontantEspece.Text);
                                    MantantRendu = 0;
                                }
                            }
                            else
                            {
                                _LReglement.MONTANT = System.Convert.ToDecimal(this.Txt_MontantEspece.Text);
                                //+ System.Convert.ToDecimal(this.Txt_FraisTimbre.Text);
                                MantantRendu = 0;
                            }
                            _ListeDesModePaiement.Add(_LReglement);
                        }
                    }
                }
                if ((this.Chk_Cheque.IsChecked == true))
                {
                    if (!string.IsNullOrEmpty(this.Txt_MontantCheque.Text) && decimal.Parse(this.Txt_MontantCheque.Text) != 0)
                    {
                        CsModereglement _LReglement = new CsModereglement();
                        _LReglement = SessionObject.ListeModesReglement.FirstOrDefault(m => m.CODE == SessionObject.Enumere.ModePayementCheque);
                        if (_LeReglement == null)
                        {
                            _LReglement.MONTANT = System.Convert.ToDecimal(this.Txt_MontantCheque.Text);
                            _ListeDesModePaiement.Add(_LReglement);
                        }
                    }
                }
                if (this.Chk_Autre.IsChecked == true)
                {
                    if (this.cbo_OtherPaiement.Tag != null)
                    {

                        CsModereglement _LeReglement = (CsModereglement)this.cbo_OtherPaiement.Tag;
                        if (_LeReglement != null)
                        {
                            CsModereglement _LReglement = new CsModereglement();
                            _LReglement = (CsModereglement)this.cbo_OtherPaiement.SelectedItem;
                            _LReglement.MONTANT = System.Convert.ToDecimal(this.txtOtherPaie.Text);
                            _ListeDesModePaiement.Add(_LReglement);
                        }
                    }
                }
                return _ListeDesModePaiement;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        void AnnulerSaisiMontantOnExceptionEspece()
        {
            Txt_MontantEspece.Text = montantInitialCollected;
            txt_MontantRendu.Text = montantInitialCollected;
        }

        private void PayementValidation(CsClient  LeClientSelectione, ref decimal MontantEncaisse, CsModereglement  ModePayement,List<CsLclient>LstfactureClient)
        {
            try
            {
                // intialisation des variables globales 
                decimal? _MontantRestant = (MontantEncaisse - Convert.ToDecimal(this.Txt_FraisTimbre.Text));
                if (LstfactureClient.Count != 0)
                {
                    foreach (CsLclient Facture in LstfactureClient)
                    {
                        if (Facture.COPER == SessionObject.Enumere.CoperNAF)
                            continue;
                        _LeReglement = GetElementDeReglement(Facture, ModePayement , MontantEncaisse);
                        if (_MontantRestant != 0)
                        {
                            if (Facture.Selectionner && !Facture.traiter)
                            {
                          
                                if (_MontantRestant >= Facture.SOLDEFACTURE)
                                {
                                    if (Facture.COPER != SessionObject.Enumere.CoperNAF)
                                    {
                                        _LeReglement.MONTANT = Facture.SOLDEFACTURE;
                                        _MontantRestant = _MontantRestant - _LeReglement.MONTANT;
                                    }
                                    if((Facture.SOLDECLIENT <= 0 && Facture.COPER == SessionObject.Enumere.CoperNAF))
                                    {
                                        _LeReglement.MONTANT = _MontantRestant.Value;
                                        _LeReglement.ACQUIT = Facture.ACQUIT;
                                        _MontantRestant = 0; 
                                    }
                                    Facture.traiter = true;
                                }
                                else if (_MontantRestant < Facture.SOLDEFACTURE)
                                {
                                    _LeReglement.MONTANT = _MontantRestant.Value;
                                    Facture.SOLDEFACTURE = Facture.SOLDEFACTURE - _LeReglement.MONTANT.Value;
                                    _MontantRestant = _MontantRestant - _LeReglement.MONTANT;
                                }
                                if (LstDesReglementAValider.FirstOrDefault(p => p.ACQUIT == _LeReglement.ACQUIT && p.MODEREG == _LeReglement.MODEREG) != null)
                                {
                                    _LeReglement.RENDU = 0;
                                    _LeReglement.PERCU = 0;
                                }
                                LstDesReglementAValider.Add(ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(_LeReglement)); //Peupler la liste de reglement
                                if (Facture.IsExonerationTaxe )
                                    LstDesReglementAValider.Add(ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(GetElementDeReglementExoTVA(Facture, ModePayement)));
                            }
                            else
                            {
                                CsLclient facs = new CsLclient();
                                facs = ListeFactureAreglee.FirstOrDefault(m => m.Selectionner == true && m.traiter == false);
                                if (facs != null)
                                    continue;
                                else
                                {
                                    if (_MontantRestant >= Facture.SOLDEFACTURE)
                                        _LeReglement.MONTANT = Facture.SOLDEFACTURE;
                                    else if (_MontantRestant < Facture.SOLDEFACTURE)
                                    {
                                        _LeReglement.MONTANT = _MontantRestant.Value;
                                        Facture.SOLDEFACTURE = Facture.SOLDEFACTURE - _LeReglement.MONTANT.Value;
                                    }
                                    //Verifier s'il pas existe avant de value montant rendu
                                    if (LstDesReglementAValider.FirstOrDefault(p => p.ACQUIT == _LeReglement.ACQUIT && p.MODEREG == _LeReglement.MODEREG) != null)
                                    {
                                        _LeReglement.RENDU = 0;
                                        _LeReglement.PERCU = 0;
                                    }
                                    Facture.Selectionner = true;
                                    LstDesReglementAValider.Add(ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(_LeReglement)); //Peupler la liste de reglement
                                    _MontantRestant = _MontantRestant - _LeReglement.MONTANT;

                                }

                            }
                        }
                        else break;
                    }
                   
                }
                if (_MontantRestant != 0 && NbreClient == 1)
                {
                    CsLclient leNaf = LstfactureClient.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperNAF);
                     if (leNaf != null)
                     {
                         CsLclient _LeReglementNaf = ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(leNaf);
                         _LeReglementNaf = GetElementDeReglement(_LeReglementNaf, ModePayement, MontantEncaisse);
                         _LeReglementNaf.IsREGLEMENTNAF = true;
                         LstDesReglementAValider.Add(_LeReglementNaf); //Peupler la liste de reglement
                     }
                     else
                     {
                         CsLclient _LeReglementNaf = ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(LstfactureClient.First());

                         _LeReglementNaf.REFEM = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00");
                         _LeReglementNaf.LIBELLENATURE = "NAF";
                         _LeReglementNaf.COPER = SessionObject.Enumere.CoperNAF;
                         _LeReglementNaf.DC = SessionObject.Enumere.Debit;
                         _LeReglementNaf.ACQUIT = this.Txt_NumRecu.Text;
                         _LeReglementNaf.DENR = DateTime.Today.Date;
                         _LeReglementNaf = GetElementDeReglement(_LeReglementNaf, ModePayement, MontantEncaisse);
                         LstDesReglementAValider.Add(_LeReglementNaf); //Peupler la liste de reglement

                     }
                }
                MontantEncaisse = _MontantRestant.Value ;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public CsLclient GetFraisTimbre(decimal? MontantEncaisse, CsModereglement ModePayement)
        {
            #region Frais de timbre
           
                CsLclient factTimbre = new CsLclient()
                {
                    CENTRE = ListeFactureAreglee[0].CENTRE,
                    CLIENT = ListeFactureAreglee[0].CLIENT,
                    ORDRE = ListeFactureAreglee[0].ORDRE,
                    REFEM = ListeFactureAreglee[0].REFEM,
                    COPER = "100",
                    CAISSE = UserConnecte.numcaisse,
                    DC = ListeFactureAreglee[0].DC,
                    NDOC = "TIMBRE",
                    NATURE = "99",
                    NUMDEM = ListeFactureAreglee[0].NUMDEM,
                    MONTANT = Convert.ToDecimal(this.Txt_FraisTimbre.Text),
                    NOM = ListeFactureAreglee[0].NOM,
                    MATRICULE = UserConnecte.matricule,
                    DATECREATION = DateTime.Today.Date,
                    DATEMODIFICATION = DateTime.Today.Date,
                    ACQUIT = SessionObject.DernierNumeroDeRecu.Value.ToString(),
                    FK_IDMODEREG = ModePayement.PK_ID,
                    FK_IDCLIENT = ListeFactureAreglee[0].FK_IDCLIENT,
                    FK_IDCENTRE = ListeFactureAreglee[0].FK_IDCENTRE,
                    FK_IDLCLIENT = ListeFactureAreglee[0].FK_IDLCLIENT,

                };
                if (ListeFactureAreglee[0].NUMDEVIS != null)
                    factTimbre.NUMDEVIS = ListeFactureAreglee[0].NUMDEVIS != null ? ListeFactureAreglee[0].NUMDEVIS : string.Empty;
                factTimbre = GetElementDeReglement(factTimbre, ModePayement, Convert.ToDecimal(this.Txt_MontantEncaisse.Text));

                return factTimbre;
              
          
            
            #endregion
        }

        private void PayementNaf(decimal? MontantEncaisse, CsLclient  LigneNAf,CsModereglement  leReglement,List<CsLclient> ListeFactureClient)
        {
            try
            {
                // intialisation des variables globales 
                decimal?  _MontantRestant = MontantEncaisse;
                foreach (CsLclient Facture in ListeFactureClient)
                {
                    if (Facture.COPER == SessionObject.Enumere.CoperNAF)
                        continue;
                    if (_MontantRestant != 0)
                    {
                        _LeReglement = GetElementDeReglement(Facture, leReglement, MontantEncaisse);
                        if (Facture.Selectionner && !Facture.traiter)
                        {
                            if (_MontantRestant >= Facture.SOLDEFACTURE)
                            {
                                _LeReglement.MONTANT = Facture.SOLDEFACTURE;
                                _LeReglement.IsREGLEMENTPARNAF = true;
                                _LeReglement.FK_IDNAF = LigneNAf.FK_IDLCLIENT;
                                Facture.traiter = true;
                            }
                            else if (_MontantRestant < Facture.SOLDEFACTURE)
                            {
                                _LeReglement.MONTANT = _MontantRestant.Value;
                                Facture.SOLDEFACTURE = Facture.SOLDEFACTURE - _LeReglement.MONTANT.Value;
                                _LeReglement.IsREGLEMENTPARNAF = true;
                                _LeReglement.FK_IDNAF = LigneNAf.FK_IDLCLIENT;
                            }
                            //Verifier s'il pas existe avant de value montant rendu
                            if (LstDesReglementAValider.FirstOrDefault(p => p.ACQUIT == _LeReglement.ACQUIT && p.MODEREG == _LeReglement.MODEREG ) != null)
                            {
                                _LeReglement.RENDU = 0;
                                _LeReglement.PERCU = 0;
                            }
                            CsLclient _LeReglementCree = ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(_LeReglement); // Clonner l'objet reglement pour rompre les pointeurs
                            LstDesReglementAValider.Add(_LeReglementCree); //Peupler la liste de reglement
                            _MontantRestant = _MontantRestant - _LeReglement.MONTANT;
                        }
                    }
                    else break;
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        private CsLclient  GetElementDeReglement(CsLclient  Facture, CsModereglement  ModePaiement,decimal? montantPercu)
        {
            CsLclient Reglement = Facture;
            try
            {
                if (!string.IsNullOrEmpty(Reglement.REFEM) && Reglement.REFEM.Length == 7)
                      Reglement.REFEM = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(Facture.REFEM);

                Reglement.ACQUIT = this.Txt_NumRecu.Text;
                Reglement.DC = Facture.DC;
                Reglement.MODEREG = ModePaiement.CODE ;
                Reglement.MOISCOMPT = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00");
                Reglement.TOP1 = SessionObject.Enumere.TopCaisse ;
                Reglement.ECART = 0;
                Reglement.PERCU = montantPercu.Value;
                Reglement.USERCREATION = Facture.USERCREATION == null ? UserConnecte.matricule : Facture.USERCREATION;
                Reglement.USERMODIFICATION = UserConnecte.matricule;
                Reglement.DATECREATION = Facture.DATECREATION == null ? DateTime.Now  : Facture.DATECREATION;
                Reglement.DATEMODIFICATION = Facture.DATEMODIFICATION == null ? DateTime.Now  : Facture.DATEMODIFICATION;
                Reglement.ORIGINE = UserConnecte.Centre;
                Reglement.POSTE = SessionObject.LePosteCourant.NOMPOSTE;
                Reglement.DENR = System.DateTime.Now;
                Reglement.DTRANS = System.DateTime.Today ;
                Reglement.RENDU = MantantRendu.Value;
                Reglement.NOMCAISSIERE = UserConnecte.nomUtilisateur;
                if (ModePaiement.CODE  == SessionObject.Enumere.ModePayementCheque)
                {
                    Reglement.NUMCHEQ = this.Txt_NumCheque.Text;
                    Reglement.PLACE = SessionObject.Enumere.EtatChequeChequeSurPlace;
                    Reglement.BANQUE = (string.IsNullOrEmpty(_bankselectioner.CODE)) ? null : _bankselectioner.CODE;
                    Reglement.GUICHET = (string.IsNullOrEmpty(_bankselectioner.CODE )) ? null : _bankselectioner.CODE ;
                }
                else
                {
                    Reglement.NUMCHEQ = null;
                    Reglement.PLACE = "-";
                    Reglement.BANQUE = "------";
                    Reglement.GUICHET = "------";
                }

                if (OperationCaisse == SessionObject.Enumere.OperationDeCaisseEncaissementFactureManuel)
                {
                    Reglement.ORIGINE = UserConnecte.CentreSelected ;
                }
                Reglement.NUMDEM = Facture.NUMDEM;
                Reglement.NUMDEVIS = Facture.NUMDEVIS;
                Reglement.FK_IDCENTRE = Facture.FK_IDCENTRE;
                Reglement.FK_IDLCLIENT   = Facture.FK_IDLCLIENT ;
                Reglement.FK_IDCLIENT  = Facture.FK_IDCLIENT ;
                Reglement.FK_IDCOPER = SessionObject.LstDesCopers.First(t => t.CODE == Facture.COPER ).PK_ID;
                Reglement.FK_IDADMUTILISATEUR  = UserConnecte.PK_ID ;
                Reglement.FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.First(t => t.CODE == SessionObject.Enumere.TopCaisse).PK_ID;
                Reglement.FK_IDHABILITATIONCAISSE = SessionObject.LaCaisseCourante.PK_ID;
                Reglement.FK_IDCAISSIERE  = SessionObject.LaCaisseCourante.FK_IDCAISSIERE;
                Reglement.FK_IDAGENTSAISIE = null ;
                Reglement.FK_IDPOSTECLIENT = null;
                Reglement.FK_IDMODEREG = ModePaiement.PK_ID ;
                Reglement.MATRICULE = UserConnecte.matricule ;
                Reglement.LIBELLESITE = SessionObject.LstCentre.FirstOrDefault(t => SessionObject.LaCaisseCourante.FK_IDCENTRE == t.PK_ID).LIBELLESITE;
                Reglement.LIBELLEAGENCE = SessionObject.LstCentre.FirstOrDefault(t => SessionObject.LaCaisseCourante.FK_IDCENTRE == t.PK_ID).LIBELLE;
                Reglement.LIBELLEAGENCE = SessionObject.LstCentre.FirstOrDefault(t => SessionObject.LaCaisseCourante.FK_IDCENTRE == t.PK_ID).LIBELLE;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Reglement;
        }

        private CsLclient GetElementDeReglementExoTVA(CsLclient Facture, CsModereglement  ModePaiement)
        {
            CsLclient Reglement = Facture;
            try
            {
 
                Reglement.REFEM = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(Facture.REFEM);
                Reglement.COPER = SessionObject.Enumere.CoperEXO ;
                Reglement.DENR  = DateTime.Today.Date;
                Reglement.ACQUIT = this.Txt_NumRecu.Text;
                Reglement.MATRICULE = Facture.MATRICULE;
                Reglement.MODEREG = ModePaiement.CODE ;
                Reglement.MOISCOMPT = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00");
                Reglement.TOP1 = SessionObject.Enumere.TopCaisse;
                Reglement.ECART = 0;
                Reglement.NUMDEM = Facture.NUMDEM;
                Reglement.NUMDEVIS = Facture.NUMDEVIS;
                Reglement.SAISIPAR = Facture.SAISIPAR;
                Reglement.CRET = Facture.CRET;
                Reglement.USERCREATION = Facture.USERCREATION == null ? UserConnecte.matricule : Facture.USERCREATION;
                Reglement.USERMODIFICATION = UserConnecte.matricule;
                Reglement.DATECREATION = Facture.DATECREATION == null ? DateTime.Now : Facture.DATECREATION;
                Reglement.DATEMODIFICATION = Facture.DATEMODIFICATION == null ? DateTime.Now : Facture.DATEMODIFICATION;
                Reglement.ORIGINE = UserConnecte.Centre;
                Reglement.POSTE = SessionObject.LePosteCourant.NOMPOSTE;
                if (ModePaiement .CODE == SessionObject.Enumere.ModePayementCheque)
                {
                    Reglement.NUMCHEQ = this.Txt_NumCheque.Text;
                    Reglement.PLACE = SessionObject.Enumere.EtatChequeChequeSurPlace;
                    Reglement.BANQUE = (string.IsNullOrEmpty(_bankselectioner.CODE)) ? null : _bankselectioner.CODE;
                }
                else
                {
                    Reglement.NUMCHEQ = null;
                    Reglement.PLACE = "-";
                    Reglement.BANQUE = "------";
                    Reglement.GUICHET = "------";
                }
                Reglement.RENDU = MantantRendu.Value;
                Reglement.DENR = Facture.DENR;
                Reglement.DATEVALEUR = System.DateTime.Today.Date;
                Reglement.NOMCAISSIERE = UserConnecte.nomUtilisateur;
                // encaissement manuel
                if (OperationCaisse == SessionObject.Enumere.OperationDeCaisseEncaissementFactureManuel)
                {
                    Reglement.ORIGINE = UserConnecte.CentreSelected;
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Reglement;
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              EnregistrerEncaissement(LstDesReglementAValider );
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }

        //private bool EnregistrerEncaissement1(List<CsLclient> _LstFactureAregler)
        //{
        //    try
        //    {
        //        //Controle des données de l'IHM
        //        //if (NbreClient > 1 &&
        //        //    System.Convert.ToDecimal(this.Txt_MontantEspece.Text) + System.Convert.ToDecimal(this.Txt_MontantCheque.Text)
        //        //    != Convert.ToDecimal(Txt_MontantFacture.Text) + Convert.ToDecimal(this.Txt_FraisTimbre.Text))

        //        if (NbreClient > 1 &&
        //            System.Convert.ToDecimal(this.Txt_MontantEspece.Text) + System.Convert.ToDecimal(this.Txt_MontantCheque.Text)
        //            != Convert.ToDecimal(Txt_MontantFacture.Text) + Convert.ToDecimal(this.Txt_FraisTimbre.Text))
        //            Message.ShowInformation(Langue.MsgMultiPayement, Langue.LibelleModule);
        //        else
        //        {
        //            if (Chk_Cheque.IsChecked == true && (string.IsNullOrEmpty(Txt_NumCheque.Text) || Txt_NumCheque.Text.Length < SessionObject.Enumere.TailleNumeroCheque))
        //            {
        //                Message.ShowInformation(Langue.msg_numero_cheque, Langue.errorTitle);
        //                return false;
        //            }
        //            if (Convert.ToDecimal(this.Txt_MontantEncaisse.Text) < (Convert.ToDecimal(this.Txt_MontantEspece.Text)))
        //            {
        //                Message.ShowInformation(Langue.msg_PayerInferieurDu, Langue.errorTitle);
        //                return false;
        //            }
        //            LstDesReglementAValider = new List<CsLclient>();
        //            if (!string.IsNullOrEmpty(Txt_NumRecu.Text) && Txt_NumRecu.Text.Length == 9)
        //            {
        //                if ((Convert.ToDecimal(this.txt_MontantRendu.Text) > 0 &&
        //                    Convert.ToDecimal(this.Txt_MontantFacture.Text) != 0 &&
        //                    OperationCaisse != SessionObject.Enumere.OperationDeCaisseEncaissementFactureManuel)
        //                   || (System.Convert.ToDecimal(this.Txt_MontantEspece.Text) >
        //                       Convert.ToDecimal(Txt_MontantFacture.Text) + Convert.ToDecimal(this.Txt_FraisTimbre.Text)))
        //                {
        //                    decimal MontantCheque = 0;
        //                    Decimal.TryParse(Txt_MontantCheque.Text, out MontantCheque);
        //                    // annuler message overpayment lorsque le mode de paiement contient checque
        //                    if (string.IsNullOrEmpty(Txt_MontantCheque.Text) || MontantCheque <= 0)  // aucun paiement checque
        //                    {
        //                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.LibelleModule, Langue.MsgMontantEleve, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
        //                        w.OnMessageBoxClosed += (_, result) =>
        //                        {
        //                            if (w.Result == MessageBoxResult.OK)
        //                            {
        //                                var ws = new MessageBoxControl.MessageBoxChildWindow(Langue.LibelleModule, Langue.MsgOverpayement, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
        //                                ws.OnMessageBoxClosed += (l, results) =>
        //                                {
        //                                    if (ws.Result == MessageBoxResult.No)
        //                                    {

        //                                        List<CsModereglement> LstsModePaiement = SelectModeDePaiement(true);
        //                                        PaymentNormal(ListeFactureAreglee, LstsModePaiement);
        //                                    }
        //                                    else if (ws.Result == MessageBoxResult.OK)
        //                                    {
        //                                        List<CsModereglement> LstsModePaiement = SelectModeDePaiement(false);
        //                                        PaymentNormal(ListeFactureAreglee, LstsModePaiement);
        //                                    }

        //                                    Valider(LstDesReglementAValider, OperationCaisse);
        //                                };
        //                                ws.Show();
        //                            }
        //                        };
        //                        w.Show();
        //                    }
        //                    else
        //                    {
        //                        List<CsModereglement> LstsModePaiement = SelectModeDePaiement(true);
        //                        PaymentNormal(ListeFactureAreglee, LstsModePaiement);
        //                        Valider(LstDesReglementAValider, OperationCaisse);

        //                    }
        //                }
        //                else
        //                {
        //                    if (Convert.ToDecimal(this.txt_MontantRendu.Text) > 0 &&
        //                             Convert.ToDecimal(this.Txt_MontantFacture.Text) != 0 &&
        //                      OperationCaisse == SessionObject.Enumere.OperationDeCaisseEncaissementFactureManuel)
        //                    {
        //                        List<CsModereglement> LstsModePaiement = SelectModeDePaiement(true);
        //                        PaymentNormal(ListeFactureAreglee, LstsModePaiement);
        //                    }
        //                    else
        //                    {
        //                        List<CsModereglement> LstsModePaiement = SelectModeDePaiement(false);
        //                        PaymentNormal(ListeFactureAreglee, LstsModePaiement);
        //                    }

        //                    Valider(LstDesReglementAValider, OperationCaisse);
        //                }
        //            }
        //            else
        //            {
        //                Message.Show(Langue.msg_entrez_numero_recu, "Payment module");
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Message.Show(ex, Langue.errorTitle);
        //    }

        //    reject = false;
        //    return true;
        //}

        private bool EnregistrerEncaissement(List<CsLclient> _LstFactureAregler)
        {
            try
            {
                decimal MontantPayesSansLesFrais = ((string.IsNullOrEmpty(this.txt_MontantPayeEspece.Text) ? 0 : Convert.ToDecimal(this.txt_MontantPayeEspece.Text)) + (string.IsNullOrEmpty(this.txt_MontantPayeCheque.Text) ? 0 : Convert.ToDecimal(this.txt_MontantPayeCheque.Text)) + (string.IsNullOrEmpty(this.txt_MontantAutre.Text) ? 0 : Convert.ToDecimal(this.txt_MontantAutre.Text)));
                if (NbreClient > 1 && 
                   ((!string.IsNullOrEmpty(this.txt_MontantRendu.Text) && Convert.ToDecimal(this.txt_MontantRendu.Text) > 0) ||
                    MontantPayesSansLesFrais>MontantFacture ))
                {
                    Message.Show(Langue.MsgMultiPayement, Langue.LibelleModule);
                    return false;
                }


                if (string.IsNullOrEmpty(Txt_NumRecu.Text) || Txt_NumRecu.Text.Length != 9)
                {
                    Message.Show(Langue.msg_entrez_numero_recu, Langue.LibelleModule );
                    return false;
                }
                if (Chk_Cheque.IsChecked == true && (string.IsNullOrEmpty(Txt_NumCheque.Text) || Txt_NumCheque.Text.Length < SessionObject.Enumere.TailleNumeroCheque))
                {
                    Message.ShowInformation(Langue.msg_numero_cheque, Langue.errorTitle);
                    return false;
                }
                if (Chk_Cash.IsChecked == true && Convert.ToDecimal(this.Txt_MontantEncaisse.Text) < (Convert.ToDecimal(this.Txt_MontantEspece.Text)))
                {
                    Message.ShowInformation(Langue.msg_PayerInferieurDu, Langue.errorTitle);
                    return false;
                }
                // Reglement par cheque 
                if (Chk_Cheque.IsChecked == true)
                {
                    CsModereglement leModeReglementCheque = SessionObject.ListeModesReglement.FirstOrDefault(m => m.CODE == SessionObject.Enumere.ModePayementCheque);
                    if (leModeReglementCheque == null)
                    {
                        Message.Show(Langue.ErreurModeReglement, Langue.LibelleModule);
                        return false;
                    }
                    leModeReglementCheque.MONTANT = System.Convert.ToDecimal(this.txt_MontantRecuCheque.Text);
                    PaymentNormal(ListeFactureAreglee, leModeReglementCheque);
                }
                //
                // Reglement par cheque 
                if (Chk_Autre.IsChecked == true)
                {
                    if (lesModRegDuGrid != null && lesModRegDuGrid.Count != 0)
                        foreach (CsModereglement item in lesModRegDuGrid)
                            PaymentNormal(ListeFactureAreglee, item);
                }
                //
                // Reglement par espece 
                if (Chk_Cash.IsChecked == true)
                {

                    if ((Convert.ToDecimal(this.txt_MontantRendu.Text) > 0 &&
                         Convert.ToDecimal(this.Txt_MontantFacture.Text) != 0 &&
                        OperationCaisse != SessionObject.Enumere.OperationDeCaisseEncaissementFactureManuel)
                    || (System.Convert.ToDecimal(this.txt_MontantPayeEspece.Text) >
                       Convert.ToDecimal(Txt_MontantFacture.Text) + Convert.ToDecimal(this.Txt_FraisTimbre.Text)))
                    {
                      
                            var w = new MessageBoxControl.MessageBoxChildWindow(Langue.LibelleModule, Langue.MsgMontantEleve, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                            w.OnMessageBoxClosed += (_, result) =>
                            {
                                if (w.Result == MessageBoxResult.OK)
                                {
                                    var ws = new MessageBoxControl.MessageBoxChildWindow(Langue.LibelleModule, Langue.MsgOverpayement, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                                    ws.OnMessageBoxClosed += (l, results) =>
                                    {
                                        if (ws.Result == MessageBoxResult.Yes)
                                        {
                                            CsModereglement leModeReglementEspece = SessionObject.ListeModesReglement.FirstOrDefault(m => m.CODE == SessionObject.Enumere.ModePayementEspece);
                                            if (leModeReglementEspece == null)
                                            {
                                                Message.Show(Langue.ErreurModeReglement, Langue.LibelleModule);
                                                return;
                                            }
                                            if (Chk_InclureFrais.IsChecked == false)
                                                leModeReglementEspece.MONTANT = System.Convert.ToDecimal(this.Txt_MontantEspece.Text);
                                            else
                                                leModeReglementEspece.MONTANT = System.Convert.ToDecimal(this.Txt_MontantEspece.Text) - (string.IsNullOrEmpty(this.Txt_FraisTimbre.Text) ? 0 : Convert.ToDecimal(this.Txt_FraisTimbre.Text));
                                            MantantRendu = System.Convert.ToDecimal(this.txt_MontantRendu.Text);

                                            leModeReglementEspece.MONTANT = leModeReglementEspece.MONTANT - Convert.ToDecimal(this.Txt_FraisTimbre.Text);
                                            PaymentNormal(ListeFactureAreglee, leModeReglementEspece);
                                            if (!string.IsNullOrEmpty(this.Txt_FraisTimbre.Text))
                                            {
                                                if (LstDesReglementAValider.FirstOrDefault(t => t.NDOC == "TIMBRE") == null)
                                                    LstDesReglementAValider.Add(ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(GetFraisTimbre(Convert.ToDecimal(this.Txt_FraisTimbre.Text), leModeReglementEspece)));
                                            }
                                            Valider(LstDesReglementAValider, OperationCaisse);
                                        }
                                        else
                                        {
                                            CsModereglement leModeReglementEspece = SessionObject.ListeModesReglement.FirstOrDefault(m => m.CODE == SessionObject.Enumere.ModePayementEspece);
                                            if (leModeReglementEspece == null)
                                            {
                                                Message.Show(Langue.ErreurModeReglement, Langue.LibelleModule);
                                                return ;
                                            }
                                            if (Chk_InclureFrais.IsChecked == false)
                                                leModeReglementEspece.MONTANT = System.Convert.ToDecimal(this.Txt_MontantEncaisse.Text);
                                            else
                                                leModeReglementEspece.MONTANT = System.Convert.ToDecimal(this.Txt_MontantEncaisse.Text) - (string.IsNullOrEmpty(this.Txt_FraisTimbre.Text) ? 0 : Convert.ToDecimal(this.Txt_FraisTimbre.Text));  
                                                MantantRendu =0;

                                            leModeReglementEspece.MONTANT = leModeReglementEspece.MONTANT - Convert.ToDecimal(this.Txt_FraisTimbre.Text);
                                            PaymentNormal(ListeFactureAreglee, leModeReglementEspece);
                                            if (!string.IsNullOrEmpty(this.Txt_FraisTimbre.Text))
                                            {
                                                if (LstDesReglementAValider.FirstOrDefault(t => t.NDOC == "TIMBRE") == null)
                                                    LstDesReglementAValider.Add(ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(GetFraisTimbre(Convert.ToDecimal(this.Txt_FraisTimbre.Text), leModeReglementEspece)));
                                            }
                                            Valider(LstDesReglementAValider, OperationCaisse);
                                        }
                                    };
                                    ws.Show();
                                }
                            };
                            w.Show();
                    }
                    else
                    {
                        CsModereglement leModeReglementEspece = SessionObject.ListeModesReglement.FirstOrDefault(m => m.CODE == SessionObject.Enumere.ModePayementEspece);
                        if (leModeReglementEspece == null)
                        {
                            Message.Show(Langue.ErreurModeReglement, Langue.LibelleModule);
                            return false;
                        }
                        if (Chk_InclureFrais.IsChecked == false)
                            leModeReglementEspece.MONTANT = System.Convert.ToDecimal(this.txt_MontantRecu.Text);
                        else
                            leModeReglementEspece.MONTANT = System.Convert.ToDecimal(this.Txt_MontantEspece.Text);

                        leModeReglementEspece.MONTANT = leModeReglementEspece.MONTANT - Convert.ToDecimal(this.Txt_FraisTimbre.Text);
                        PaymentNormal(ListeFactureAreglee, leModeReglementEspece);
                        if (!string.IsNullOrEmpty(this.Txt_FraisTimbre.Text))
                        {
                            if (LstDesReglementAValider.FirstOrDefault(t => t.NDOC == "TIMBRE") == null)
                                LstDesReglementAValider.Add(ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(GetFraisTimbre(Convert.ToDecimal(this.Txt_FraisTimbre.Text), leModeReglementEspece)));
                        }
                        Valider(LstDesReglementAValider, OperationCaisse);
                    }
                }
                else
                    Valider(LstDesReglementAValider, OperationCaisse);
                //
                reject = false;
                return true;
            }
            catch (Exception ex)
            {
                Message.Show(ex, Langue.errorTitle);
            }

            reject = false;
            return true;
        }



        private void PaymentNormal(List<CsLclient> ListeFactureAreglee, CsModereglement _ModePaiement)
        {
           
             decimal MontantPaye = _ModePaiement.MONTANT.Value ;
            List<CsClient > _ListeClientSelect = MethodeGenerics.RetourneClientFromFacture (ListeFactureAreglee);
            foreach (CsClient item in _ListeClientSelect)
            {
                List<CsLclient> ListeDesFactureDuClient = new List<CsLclient>();
                if (NbreClient == 1)
                    ListeDesFactureDuClient = ListeFactureAreglee.Where(m => m.FK_IDCLIENT == item.PK_ID && m.traiter == false ).ToList();
                else
                    ListeDesFactureDuClient = ListeFactureAreglee.Where(m => m.FK_IDCLIENT == item.PK_ID  &&  m.traiter == false && (m.Selectionner == true || m.COPER == SessionObject.Enumere.CoperNAF)).ToList();

                if (ListeDesFactureDuClient != null && ListeDesFactureDuClient.Count != 0)
                {
                    

                    CsLclient fac = ListeDesFactureDuClient.FirstOrDefault(m => m.COPER == SessionObject.Enumere.CoperNAF &&
                                                                       m.FK_IDCLIENT == item.PK_ID &&
                                                                       m.SOLDECLIENT > 0);
                    if (fac != null)
                    {
                        CsModereglement _LeReglement = SessionObject.ListeModesReglement.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ModePayementEspece);
                        if (_LeReglement != null)
                            PayementNaf(fac.MONTANT.Value, fac, _LeReglement, ListeDesFactureDuClient);
                    }
                    PayementValidation(item, ref MontantPaye, _ModePaiement, ListeDesFactureDuClient.OrderByDescending(t => t.Selectionner).OrderByDescending(i => i.NDOC).ToList());
                    }
                }
        }
        private void Valider(List<CsLclient> LstDesReglementAInserer, string Action)
        {
            #region impression normale
            if (LstDesReglementAInserer == null || LstDesReglementAInserer.Count == 0)
            {
                Message.ShowInformation("Aucun règlement de facture en cours ...", "Erreur");
                return;
            }
            CaisseServiceClient service = new CaisseServiceClient(Utility.Protocole(), Utility.EndPoint("Caisse"));
            service.InsererEncaissementCompleted += (sender, es) =>
            {
                if (es.Error != null || es.Cancelled)
                {
                    Message.ShowError("L'insertion des encaissements a retournée une erreur. Réessayer svp! :" + es.Error.Message, "Erreur");
                    return;
                }

                if (es.Result == null)
                {
                    Message.ShowError("L'insertion des encaissements a retournée une erreur. Réessayer svp!", "Erreur");
                    return;
                }
                this.DialogResult = true;
                // incrementer le dernier numero d acquit
                SessionObject.DernierNumeroDeRecu += 1;

                List<CsReglementRecu> _ListeDesRecu = new List<CsReglementRecu>();
                LstDesReglementAInserer = LstDesReglementAInserer.Where(p => p.COPER != SessionObject.Enumere.CoperNAF || p.COPER != SessionObject.Enumere.CoperRNA).ToList();

                List<CsClient> _lstClientRecu = Galatee.Silverlight.Caisse.MethodeGenerics.RetourneClientFromFacture(LstDesReglementAInserer);
                if (_lstClientRecu.Count > 1)
                {
                    _ListeDesRecu = EditionRecu.ReorganiserBordero(LstDesReglementAInserer);
                    string key = Utility.getKey();
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    LanceImpression(key, param, _ListeDesRecu, true);

                }
                foreach (var item in _lstClientRecu)
                {
                    List<CsLclient> lstFactureDuClient = LstDesReglementAInserer.Where(t => t.FK_IDCLIENT == item.PK_ID).ToList();
                    if (_lstClientRecu.Count > 1)
                    {
                        lstFactureDuClient.ForEach(t => t.PERCU = lstFactureDuClient.Sum(p => p.MONTANT));
                        lstFactureDuClient.ForEach(t => t.RENDU = 0);
                    }

                    _ListeDesRecu = EditionRecu.ReorganiserReglement(lstFactureDuClient, OperationCaisse);
                    string key = Utility.getKey();
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("pTypeRecu", OperationCaisse);
                    LanceImpression(key, param, _ListeDesRecu, false);
                }

            };
            service.InsererEncaissementAsync(LstDesReglementAInserer, OperationCaisse);

            #endregion
        }
        private void LanceImpression(string key, Dictionary<string, string> parametresRDLC, List<Galatee.Silverlight.ServiceCaisse.CsReglementRecu> res,bool IsBordero)
        {
            bool busyIndicatorEstEnCours = false;
            try
            {

                int loaderHandler = LoadingManager.BeginLoading("Traitement en cours ... ");
                busyIndicatorEstEnCours = true;

                CaisseServiceClient service = new CaisseServiceClient(Utility.Protocole(),Utility.EndPoint("Caisse"));
                service.CashierPaymentsAsync(key, parametresRDLC, res);
                service.CashierPaymentsCompleted += (er_, res_) =>
                {
                    try
                    {
                        if (res_.Error != null || res_.Cancelled)
                        {
                            if (busyIndicatorEstEnCours)
                            {
                                LoadingManager.EndLoading(loaderHandler);
                                busyIndicatorEstEnCours = false;
                            }
                            Message.ShowInformation("Erreur survenue lors de l'appel service", "ERROR");
                            return;
                        }
                        if (res_.Result == null && res_.Result.Count == 0)
                        {
                            if (busyIndicatorEstEnCours)
                            {
                                LoadingManager.EndLoading(loaderHandler);
                                busyIndicatorEstEnCours = false;
                            }

                            Message.ShowInformation("Aucune donnees a afficher", "ERROR");
                            return;
                        }
                        else
                        {
                            if (!IsBordero)
                            Utility.ActionCaisseBanking(key, "recu", "Caisse", false);
                            else
                                Utility.ActionCaisseBanking(key, "BorderoRegroupe", "Caisse", false);
                            //Utility.ActionImpressionDirect(SessionObject.DefaultPrinter , key, "recu", "Caisse");
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        if (busyIndicatorEstEnCours)
                        {
                            LoadingManager.EndLoading(loaderHandler);
                            busyIndicatorEstEnCours = false;
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            reject = true;
            this.DialogResult = true;
        }
        private List<CsLclient> FactureSelectionner(List<CsClient> ListClient, List<CsLclient> _LstFactureAregler)
        {


            List<CsLclient> FactSelect = (from d in _LstFactureAregler
                                         where (d.Selectionner == true && 
                                                 d.COPER != SessionObject.Enumere.CoperNAF )
                                         select d).ToList();
            if (FactSelect == null || FactSelect.Count == 0)
            {
                _LstFactureAregler.ForEach(t => t.Selectionner = true);
                return _LstFactureAregler;
            }

            List<CsLclient> facsClient = new List<CsLclient>();
            foreach (CsClient  LeClient in ListClient)
            {
                facsClient.AddRange((from d in _LstFactureAregler
                        where (d.FK_IDCLIENT  == LeClient.PK_ID &&  
                                d.Selectionner == true  
                                )
                        select d).ToList());
            }
            return facsClient;
        }

        void galatee_OkClicked(object sender, EventArgs e)
        {
            //UcListe ctrs = sender as UcListe;
            MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                _bankselectioner = (CsBanque)ctrs.MyObject;
                this.Txt_LibBank.Text = string.IsNullOrEmpty(_bankselectioner.LIBELLE) ? string.Empty : _bankselectioner.LIBELLE;
                Double MontanY = Single.Parse(this.Txt_MontantFacture.Text) - Single.Parse(this.txt_MontantRecu.Text);
                this.Txt_MontantCheque.Text = MontanY.ToString(SessionObject.FormatMontant );
                this.Txt_MontantCheque.Focus();
                OKButton.IsEnabled = true;
            }
        }

 
        private void Initialisationctrl(List<CsLclient> _LstFactureAregler)
        {
            try
            {
                InitialisationChamp();
                if (!SessionObject.IsServerDown)
                {
                    if (IsSaisiCheque == false  )
                    this.Chk_Cash.IsChecked = true;
                    MontantFacture = InitValue;
                    string _MatriculeConnect = string.Empty;
                    string _NumCaissConect = string.Empty;
                    List<CsClient> LstClient = MethodeGenerics.RetourneClientFromFacture(_LstFactureAregler);
                    NbreClient = LstClient.Count;
                    List<CsLclient> _ListFact = FactureSelectionner(LstClient, _LstFactureAregler);
                    if (_ListFact.Count == 0)
                        _ListFact = ListeFactureAreglee;

                    _NumCaissConect = _ListFact[0].CAISSE;
                    if (OperationCaisse == SessionObject.Enumere.OperationDeCaisseEncaissementFactureManuel)
                    {
                        Txt_NumRecu.IsEnabled = true;
                        OKButton.IsEnabled = false;
                        _MatriculeConnect = _ListFact[0].SAISIPAR;
                    }
                    else
                        _MatriculeConnect = _ListFact[0].MATRICULE;

                    MontantFacture = _ListFact.Where(o => o.COPER != SessionObject.Enumere.CoperNAF).Sum(t => t.SOLDEFACTURE);
                    montantNAFSaisi = _ListFact.Where(o=>o.COPER == SessionObject.Enumere.CoperNAF).Sum(f => f.SOLDEFACTURE );

                    if(montantNAFSaisi == null) 
                        montantNAFSaisi = 0;

                    if (MontantFacture != 0)
                        MontantFacture = MontantFacture - montantNAFSaisi;
                    else
                        MontantFacture = montantNAFSaisi;
                   
                    if (MontantFacture < InitValue && montantNAFSaisi==null) MontantFacture = InitValue;
                    this.txt_MontantPayeEspece.Text = this.Txt_MontantFacture.Text= this.Txt_MontantEncaisse.Text =  this.Txt_MontantEspece.Text = MontantFacture.Value.ToString(SessionObject.FormatMontant);
                    CalculMontantTimbre(MontantFacture.Value);
                    txt_MontantRecu.Text = (Convert.ToDecimal(this.Txt_MontantEspece.Text) + Convert.ToDecimal(this.Txt_FraisTimbre.Text)).ToString(SessionObject.FormatMontant);
                    txt_MontantTotalRegle.Text = txt_MontantRecu.Text; 
                    this.lb_Matricule.Content = _MatriculeConnect;
                    this.lb_Numcaisse.Content = _NumCaissConect;

                    // eliminer les naf saisi
                    foreach (var f in _factureNAFSaisi)
                    {
                        if (ListeFactureAreglee.Count != 1) // Determine si on fait un paiement anticipé
                        ListeFactureAreglee.Remove(f);
                    }

                    var naf = ListeFactureAreglee.Where(f=> f.COPER == SessionObject.Enumere.CoperNAF);
                    foreach (var f in naf)
                    {
                        if (_factureNAFSaisi == null || _factureNAFSaisi.Count == 0)
                            continue;
                        f.MONTANT = _factureNAFSaisi.FirstOrDefault(o=>f.ORDRE == o.ORDRE && f.CENTRE == o.CENTRE &&
                           f.CLIENT == o.CLIENT).MONTANT;
                    }

                    ///      deplacer dans frmEncaissement
                    ///      CHK l'a deplacé dans l'evenement loaded de frmEncaissement le 14/03/2013 
                    if (OperationCaisse != SessionObject.Enumere.OperationDeCaisseEncaissementFactureManuel)
                    {
                            string format = "000000000";
                            this.Txt_NumRecu.Text = (SessionObject.DernierNumeroDeRecu.Value).ToString(format); 
                            this.Txt_MontantFacture.Focus();
                    }
                    else
                    {
                        this.Txt_NumRecu.Text = string.Empty;
                        this.Txt_NumRecu.IsEnabled = true;
                        this.Txt_NumRecu.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void CalculMontantTimbre(decimal montantSaisie)
        {

            CsFraisTimbre leFraistimbre = SessionObject.LstFraisTimbre.FirstOrDefault(p => montantSaisie >= p.BORNEINF && montantSaisie <= (p.BORNESUP + p.FRAIS));
            if (leFraistimbre != null)
            {
                this.txt_MontantTimbreEspece.Text = this.Txt_FraisTimbre.Text = Convert.ToDecimal(leFraistimbre.FRAIS).ToString(SessionObject.FormatMontant );
                montantRecuInit = Math.Abs(MontantFacture.Value + Convert.ToDecimal(leFraistimbre.FRAIS));
                decimal montantARendre = (System.Convert.ToDecimal(this.Txt_MontantEspece.Text) - System.Convert.ToDecimal(this.Txt_MontantEncaisse.Text));
                if (montantARendre > 0)
                    this.txt_MontantRendu.Text = montantARendre.ToString();
            }
            else
                this.txt_MontantTimbreEspece.Text = this.Txt_FraisTimbre.Text = InitValue.ToString(SessionObject.FormatMontant);
        }

        private void InitialisationChamp()
        {
            try
            {
                this.Txt_MontantFacture.Text = InitValue.ToString("N2");
                this.Txt_MontantEspece.Text = InitValue.ToString("N2");
                this.Txt_FraisTimbre.Text = InitValue.ToString("N2");

                this.Txt_LibBank.Text = string.Empty;
                this.Txt_MontantCheque.Text = InitValue.ToString("N2");
                this.Txt_NumCheque.Text = string.Empty;

                //this.txt_MontantImpaye.Text = InitValue.ToString("N2");
                //this.txt_MontantTTC.Text = InitValue.ToString("N2");
                this.txt_MontantRecu.Text = InitValue.ToString("N2");
                this.txt_MontantRendu.Text = InitValue.ToString("N2");

                // CHK - 14/03/2013
                this.lb_Matricule.Content = UserConnecte.matricule;
                this.lb_Numcaisse.Content = UserConnecte.numcaisse;
                string format = "000000000";
                this.Txt_NumRecu.Text = (SessionObject.DernierNumeroDeRecu.Value).ToString(format); 
                this.Txt_MontantFacture.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Initialisationctrl();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Closed(this, new EventArgs());
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
 
        void AnnulerSaisiMontantOnExceptionChecque()
        {

        }

        private void Txt_NumRecu_LostFocus(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (System.Text.RegularExpressions.Regex.IsMatch(Txt_NumRecu.Text, @"^(\w+)$"))
            //    {
            //        this.Txt_NumRecu.Text = this.Txt_NumRecu.Text.PadLeft(9, '0');
            //        OKButton.IsEnabled = false;
            //    }
            //    else
            //        Txt_NumRecu.Text = string.Empty;
            //}
            //catch (Exception ex)
            //{
            //    Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            //}
        }

        private void Txt_NumCheque_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_NumCheque.Text))
                {
                    int dummy;
                    if (!int.TryParse(Txt_NumCheque.Text, out dummy))
                        Txt_NumCheque.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }



        private void Txt_MontantEspece_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                    decimal MontantEspece = string.IsNullOrEmpty(this.Txt_MontantEspece.Text) ? 0 : decimal.Parse(this.Txt_MontantEspece.Text);
                    if (MontantEspece == 0)
                    return;

                    this.txt_MontantPayeEspece.Text = this.Txt_MontantEspece.Text;
                    decimal montantARendre = 0;
                    if (!string.IsNullOrEmpty(this.Txt_MontantEncaisse.Text) && !string.IsNullOrEmpty(this.Txt_MontantEspece.Text))
                        montantARendre = System.Convert.ToDecimal(this.Txt_MontantEncaisse.Text) - (System.Convert.ToDecimal(this.Txt_MontantEspece.Text));
                    if (montantARendre > 0)
                        this.txt_MontantRendu.Text = montantARendre.ToString();
                    else
                        this.txt_MontantRendu.Text = InitValue.ToString(SessionObject.FormatMontant);
                    if (this.Chk_Cash.IsChecked == true)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_MontantEspece.Text) && Convert.ToDecimal(this.Txt_MontantEspece.Text) != 0)
                            CalculMontantTimbre(Convert.ToDecimal(this.Txt_MontantEspece.Text));
                    }
                    txt_MontantRecu.Text = (Convert.ToDecimal(this.Txt_MontantEspece.Text) + Convert.ToDecimal(this.Txt_FraisTimbre.Text)).ToString(SessionObject.FormatMontant);
                    txt_MontantTotalRegle.Text = ((string.IsNullOrEmpty(this.txt_MontantRecu.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecu.Text)) + (string.IsNullOrEmpty(this.txt_MontantRecuCheque.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecuCheque.Text)) + (string.IsNullOrEmpty(this.txt_MontantAutre.Text) ? 0 : Convert.ToDecimal(this.txt_MontantAutre.Text))).ToString(SessionObject.FormatMontant);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void txtOtherPaie_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Btn_AjouterAutre.IsEnabled = true;

            }
            catch (Exception ex)
            {
              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
                
        }

        private void Txt_MontantCheque_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal MontantCheque = string.IsNullOrEmpty(this.Txt_MontantCheque.Text) ? 0 : decimal.Parse(this.Txt_MontantCheque.Text);
                if (MontantCheque == 0)
                    return;
                this.txt_MontantPayeCheque.Text = this.Txt_MontantCheque.Text;
                this.txt_MontantFrais.Text = InitValue.ToString();
                this.txt_MontantRecuCheque.Text = (Convert.ToDecimal(this.Txt_MontantCheque.Text) + Convert.ToDecimal(this.txt_MontantFrais.Text)).ToString(SessionObject.FormatMontant);
                txt_MontantTotalRegle.Text = ((string.IsNullOrEmpty(this.txt_MontantRecu.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecu.Text)) + (string.IsNullOrEmpty(this.txt_MontantRecuCheque.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecuCheque.Text)) + (string.IsNullOrEmpty(this.txt_MontantAutre.Text) ? 0 : Convert.ToDecimal(this.txt_MontantAutre.Text))).ToString(SessionObject.FormatMontant);
            
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        

        private void txt_MontantRecu_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Chk_Cash.IsChecked == true)
                {
                    if (Decimal.Parse(txt_MontantRecu.Text) > 0)
                        OKButton.IsEnabled = true;
                    else
                        OKButton.IsEnabled = false;
                    txt_MontantTotalRegle.Text = ((string.IsNullOrEmpty(this.txt_MontantRecu.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecu.Text)) + (string.IsNullOrEmpty(this.txt_MontantRecuCheque.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecuCheque.Text)) + (string.IsNullOrEmpty(this.txt_MontantAutre.Text) ? 0 : Convert.ToDecimal(this.txt_MontantAutre.Text))).ToString(SessionObject.FormatMontant);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "=> txt_MontantRecu_TextChanged_1");
            }
        }

        //private void Txt_MontantCheque_TextChanged_1(object sender, TextChangedEventArgs e)
        //{
        //    if (Chk_Cheque.IsChecked == true)
        //    {
        //        decimal montantDu = 0;
        //        //float montantRecu = 0;
        //        decimal monnaie = 0;

        //        montantEspeces = Chk_Cash.IsChecked == true ? montantEspeces : 0;
        //        montantCheques = Chk_Cheque.IsChecked == true ? montantCheques : 0;
        //        montantAutre = Chk_Autre.IsChecked == true ? montantAutre : 0;

        //        if (!decimal.TryParse(txt_MontantDu.Text, out montantDu) || !decimal.TryParse(Txt_MontantCheque.Text, out montantCheques))
        //        {
        //            montantDu = 0;
        //            montantCheques = 0;
        //        }
                                    
        //        montantTotalPaye = montantEspeces + montantCheques + montantAutre ;
        //        txt_MontantRecu.Text = montantTotalPaye.ToString();
        //        monnaie = montantTotalPaye - montantDu;                

        //        if (monnaie > 0)
        //            txt_MontantRendu.Text = monnaie.ToString();
        //    }
        //}

        private void Chk_Autre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtOtherPaie.IsEnabled = (bool)Chk_Autre.IsChecked;
                cbo_OtherPaiement.IsEnabled = (bool)cbo_OtherPaiement.IsEnabled;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "=>Chk_Autre_Click");
            }
        }


        #region Clic sur les checkbox

 

        private void Chk_Autre_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Chk_Autre.IsChecked == true)
                {
           
                    txtOtherPaie.IsEnabled = true;
                    cbo_OtherPaiement.IsEnabled = true;
                    //Chk_Cash.IsChecked = false;

                    //Chk_Cheque.IsChecked = false;
                    //Txt_MontantEspece.Text = "0";
                    //Txt_MontantEspece.IsEnabled = false;
                    //Txt_MontantCheque.Text = "0";
                    //Txt_MontantCheque.IsEnabled = false;
                    //Txt_NumCheque.Text = "0";
                    //Txt_NumCheque.IsEnabled = false;
                }
                else
                {
                    txtOtherPaie.Text = "0";
                    txtOtherPaie.IsEnabled = false;
                    cbo_OtherPaiement.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "=>Chk_Autre_Checked");
            }
        }

        private void Chk_Cash_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (Chk_Cash.IsChecked == true)
                //{
                //    Txt_MontantEspece.IsEnabled = true;
                //    Chk_Autre.IsChecked = false;
                //    txtOtherPaie.Text = "0";
                //    txtOtherPaie.IsEnabled = false;
                //}
                //else
                //{
                //    Txt_MontantEspece.Text = "0";
                //    Txt_MontantEspece.IsEnabled = false;
                //}
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "=>Chk_Cash_Checked");
            }
        }

        #endregion

        private void Txt_NumRecu_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(Txt_NumRecu.Text, @"^(\w+)$"))
                {
                    this.Txt_NumRecu.Text = this.Txt_NumRecu.Text;
                    //this.Txt_NumRecu.Text = this.Txt_NumRecu.Text.PadLeft(9, '0');
                    OKButton.IsEnabled = true;
                }
                else
                {
                    Txt_NumRecu.Text = string.Empty;
                    OKButton.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void Chk_Cheque_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (Chk_Cheque.IsChecked == true)
                {
                    this.Txt_MontantCheque.IsEnabled = true;
                    this.Txt_NumCheque.IsEnabled = true;
                    OKButton.IsEnabled = true;
                    List<object> objects = new List<object>();
                    foreach (var o in SessionObject.ListeBanques)
                        objects.Add(o);

                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "BANQUE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(objects, _LstColonneAffich, false, "banque");
                    ctrl.Closed += new EventHandler(galatee_OkClicked);
                    ctrl.Show();
                   
                }
    
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void cbo_OtherPaiement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbo_OtherPaiement.SelectedItem != null)
            {
                this.txtOtherPaie.IsReadOnly = false;
                Double montantY = Single.Parse(this.Txt_MontantFacture.Text) - (Single.Parse(this.txt_MontantPayeEspece .Text) + Single.Parse(this.Txt_MontantCheque.Text));
                this.txtOtherPaie.Text = montantY.ToString(SessionObject.FormatMontant );
                this.cbo_OtherPaiement.Tag = (CsModereglement)cbo_OtherPaiement.SelectedItem;
            }
        }

        private void Chk_Cheque_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Txt_LibBank.Text = string.Empty;
                this.Txt_MontantCheque.Text = "0";
                this.Txt_MontantCheque.IsEnabled = false;
                this.Txt_NumCheque.Text = "0";
                this.Txt_NumCheque.IsEnabled = false;
                //OKButton.IsEnabled = true;
                this.txt_MontantPayeCheque.Text = InitValue.ToString();
                this.txt_MontantFrais.Text = InitValue.ToString();
                this.txt_MontantRecuCheque.Text = InitValue.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Chk_Autre_Checked_1(object sender, RoutedEventArgs e)
        {

        }
        private void Chk_Cash_Unchecked(object sender, RoutedEventArgs e)
        {
            Txt_MontantEspece.Text ="0"; ;
            Txt_MontantEncaisse .Text = "0" ;
            this.Txt_FraisTimbre.Text = "0" ;
            this.txt_MontantRendu .Text = "0";

            this.txt_MontantPayeEspece.Text = InitValue.ToString();
            this.txt_MontantTimbreEspece.Text = InitValue.ToString();
            this.txt_MontantRecu.Text = InitValue.ToString();
        }

        private void Txt_MontantEspece_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_MontantEspece.Text))
            { 
                decimal  Formate = Convert.ToDecimal(this.Txt_MontantEspece.Text);
                this.Txt_MontantEspece.Text = Formate.ToString(SessionObject.FormatMontant);
            }
        }

        private void Txt_MontantEncaisse_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_MontantEncaisse.Text))
            {
                decimal Formate = Convert.ToDecimal(this.Txt_MontantEncaisse.Text);
                this.Txt_MontantEncaisse.Text = Formate.ToString(SessionObject.FormatMontant);
            }
        }

        private void Txt_FraisTimbre_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_MontantCheque_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_MontantCheque.Text))
            {
                decimal Formate = Convert.ToDecimal(this.Txt_MontantCheque.Text);
                this.Txt_MontantCheque.Text = Formate.ToString(SessionObject.FormatMontant);
            }
        }
        List<CsModereglement> lesModRegDuGrid = new List<CsModereglement>();
        private void Btn_AjouterAutre_Click(object sender, RoutedEventArgs e)
        {
            CsModereglement leModreglement = this.cbo_OtherPaiement.SelectedItem as CsModereglement;
            leModreglement.MONTANT = Convert.ToDecimal(this.txtOtherPaie.Text);
            lesModRegDuGrid.Add(leModreglement);
            dtgAutre.ItemsSource = null;
            dtgAutre.ItemsSource = lesModRegDuGrid;
            this.txt_MontantAutre.Text = lesModRegDuGrid.Sum(t => t.MONTANT).Value.ToString(SessionObject.FormatMontant);
            txt_MontantTotalRegle.Text = ((string.IsNullOrEmpty(this.txt_MontantRecu.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecu.Text)) + (string.IsNullOrEmpty(this.txt_MontantRecuCheque.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecuCheque.Text)) + (string.IsNullOrEmpty(this.txt_MontantAutre.Text) ? 0 : Convert.ToDecimal(this.txt_MontantAutre.Text))).ToString(SessionObject.FormatMontant);
        }

        private void Btn_SupprimerAutre_Click(object sender, RoutedEventArgs e)
        {
            if (this.dtgAutre.SelectedItem != null)
            {
                CsModereglement leModreg = (CsModereglement)this.dtgAutre.SelectedItem;
                lesModRegDuGrid.Remove(leModreg);
                dtgAutre.ItemsSource = null;
                dtgAutre.ItemsSource = lesModRegDuGrid;
                this.txt_MontantAutre.Text = lesModRegDuGrid.Sum(t => t.MONTANT).Value.ToString(SessionObject.FormatMontant);
                txt_MontantTotalRegle.Text = ((string.IsNullOrEmpty(this.txt_MontantRecu.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecu.Text)) + (string.IsNullOrEmpty(this.txt_MontantRecuCheque.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecuCheque.Text)) + (string.IsNullOrEmpty(this.txt_MontantAutre.Text) ? 0 : Convert.ToDecimal(this.txt_MontantAutre.Text))).ToString(SessionObject.FormatMontant);
            }
        }

        private void Chk_Autre_Unchecked(object sender, RoutedEventArgs e)
        {
            this.cbo_OtherPaiement.SelectedItem = null;
            this.txtOtherPaie.Text = InitValue.ToString();
            this.dtgAutre.ItemsSource = null;
        }

        private void BtnCharger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChargerListeselectionner();
            }
            catch (Exception ex)
            {
                Message.Show(ex, Langue.errorTitle);
            }
        }
       private void ChargerListeselectionner()
         {
             try
             {
                 FrmdetailleFacture form = new FrmdetailleFacture(ListeFactureAreglee);
                 form.Closed += new EventHandler(formClosed);
                 form.Show();
             }
             catch (Exception)
             {

                 throw;
             }
         }
       void formClosed(object sender, EventArgs e)
       {
           try
           {
               this.Txt_MontantFacture.Text =(ListeFactureAreglee.Where(i=>i.Selectionner).Count() !=0) ? 
                   ListeFactureAreglee.Where(y=>y.Selectionner).Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant) :
                   ListeFactureAreglee.Where(y => y.Selectionner).Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
               this.Txt_MontantEspece.Text = this.Txt_MontantFacture.Text;
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, Langue.errorTitle);
           }

       }

       private void Chk_InclureFrais_Checked(object sender, RoutedEventArgs e)
       {
           txt_MontantPayeEspece.Text = (Convert.ToDecimal(this.Txt_MontantEspece.Text) - Convert.ToDecimal(this.Txt_FraisTimbre.Text)).ToString(SessionObject.FormatMontant);
           txt_MontantRecu.Text = Txt_MontantEspece.Text;
       }

       private void Chk_InclureFrais_Unchecked(object sender, RoutedEventArgs e)
       {
           txt_MontantRecu.Text = (Convert.ToDecimal(this.Txt_MontantEspece.Text) + Convert.ToDecimal(this.Txt_FraisTimbre.Text)).ToString(SessionObject.FormatMontant);
           txt_MontantPayeEspece.Text = (Convert.ToDecimal(this.Txt_MontantEspece.Text)).ToString(SessionObject.FormatMontant);

       }

       private void txt_MontantRecuCheque_TextChanged(object sender, TextChangedEventArgs e)
       {
           txt_MontantTotalRegle.Text = ((string.IsNullOrEmpty(this.txt_MontantRecu.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecu.Text)) + (string.IsNullOrEmpty(this.txt_MontantRecuCheque.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecuCheque.Text)) + (string.IsNullOrEmpty(this.txt_MontantAutre.Text) ? 0 : Convert.ToDecimal(this.txt_MontantAutre.Text))).ToString(SessionObject.FormatMontant);
       }
    }
}


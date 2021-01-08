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
    public partial class UcValideEncaissement : ChildWindow
    {
        List<CsLclient> ListeFactureAreglee = new List<CsLclient>();
        List<CsLclient> LstDesReglementAValider = new List<CsLclient>();

        CsBanque _bankselectioner = new CsBanque();

        public bool ValidationEditionfacture = false;
        bool reject = false;
        bool IsSaisiCheque = false;
        decimal? MontantFacture = 0;
        decimal? MantantRendu = 0;

        decimal? MantantTimbre = 0;
        decimal InitValue = 0;
        int NbreClient = 0;

        string OperationCaisse = string.Empty;
        string _ModePayement = string.Empty;

        public event EventHandler Closed;

        private void translateControls()
        {
            //this.groubox.Header = Langue.Mode_paiement;
            this.Chk_Cash.Content = Langue.Chk_cash;
            //this.label4.Content = Langue.Montant_due ;
            //this.label2.Content = Langue.MontantPayer;
            //this.label3.Content = Langue.Lbl_stamp_duty;
            this.Chk_Cheque.Content = Langue.Chk_checque;
            this.label1.Content = Langue.Lbl_bank;
            this.label5.Content = Langue.Montant;
            this.label6.Content = Langue.Lbl_checque_number;
            this.Chk_Autre.Content = Langue.Chk_other;
            this.label7.Content = Langue.Caisse;
            this.label8.Content = Langue.Num_Recu;
            this.label9.Content = Langue.Caissier;
            //this.Btn_Printer.Content = Langue.Btn_printer;
            //this.headeredContentControl6.Header = Langue.Paiement;
            //this.lbl_MontantEspeceTotal.Content = Langue.Montant_recu;
            //this.label10.Content = Langue.To_return;
            //this.lbl_MontantEspeceTimbre.Content = Langue.Montant_due;
            //this.lbl_MontantEspecePaye.Content = Langue.lbl_MontantHT;
            this.OKButton.Content = Langue.Btn_ok;
            this.CancelButton.Content = Langue.CancelButton;
        }

        public bool Yes
        {
            get { return reject; }
            set { reject = value; }
        }
        bool _IsDecaissement = false;
        public UcValideEncaissement(List<CsLclient> _ListeFactureAreglee, string _OperationCaisse, string IsDecaissement)
        {
            InitializeComponent();
            translateControls();
            SessionObject.ListeControlesCaisse.Add(this);
            ListeFactureAreglee = _ListeFactureAreglee;
            OperationCaisse = _OperationCaisse;
            RemplirModesDePaiement();
            SessionObject.ListeControlesCaisse.Add(this);
            Initialisationctrl(_ListeFactureAreglee);
            Btn_AjouterAutre.IsEnabled = false;
            this.Txt_NumCheque.MaxLength = SessionObject.Enumere.TailleNumeroCheque;
            label2.Content = lbl_MontantPayeCheque_Copy.Content = "Montant à décaisser";
            label.Content = MontantPayeChequeTotal_Copy.Content = "Montant  décaisser";
            this.Txt_MontantEncaisse.IsReadOnly = true;
            Chk_InclureFrais.Visibility = System.Windows.Visibility.Collapsed;
            btn_Facture.Visibility = System.Windows.Visibility.Collapsed;
            gbo_PaiementEspece.Header = "Décaissement espèce";
            Chk_Cheque.IsEnabled = false;
            _IsDecaissement = true;
            lbl_MontantPayeChequeFrais_Copy.Visibility = System.Windows.Visibility.Collapsed;
            txt_MontantTimbreEspece.Visibility = System.Windows.Visibility.Collapsed;

            label10.Visibility = System.Windows.Visibility.Collapsed;
            txt_MontantRendu.Visibility = System.Windows.Visibility.Collapsed;

            this.Txt_NumCheque.IsReadOnly = true;

            if (SessionObject.LaCaisseCourante == null || SessionObject.LaCaisseCourante.FK_IDCAISSE == 0)
                ChargerHabilitationCaisse();

            if (SessionObject.LstFraisTimbre == null || SessionObject.LstFraisTimbre.Count == 0)
                RetourneLstFraixTimbre();


        }
        public UcValideEncaissement(List<CsLclient> _ListeFactureAreglee, string _OperationCaisse)
        {
            InitializeComponent();
            translateControls();
            SessionObject.ListeControlesCaisse.Add(this);
            ListeFactureAreglee = _ListeFactureAreglee;
            OperationCaisse = _OperationCaisse;
            RemplirModesDePaiement();
            SessionObject.ListeControlesCaisse.Add(this);
            Initialisationctrl(_ListeFactureAreglee);
            Btn_AjouterAutre.IsEnabled = false;
            this.Txt_NumCheque.MaxLength = SessionObject.Enumere.TailleNumeroCheque;

            if (_OperationCaisse == SessionObject.Enumere.OperationDeCaisseEncaissementDevis)
                Chk_InclureFrais.Visibility = Visibility.Collapsed;

            if (SessionObject.LaCaisseCourante == null || SessionObject.LaCaisseCourante.FK_IDCAISSE == 0)
                ChargerHabilitationCaisse();

            if (SessionObject.LstFraisTimbre == null || SessionObject.LstFraisTimbre.Count == 0)
                RetourneLstFraixTimbre();
        }


        bool IsRegrouper = false;
        public UcValideEncaissement(List<CsLclient> _ListeFactureAreglee, string _OperationCaisse, bool IsPaiementRegrouper)
        {
            InitializeComponent();
            translateControls();
            SessionObject.ListeControlesCaisse.Add(this);
            ListeFactureAreglee = _ListeFactureAreglee;
            OperationCaisse = _OperationCaisse;
            RemplirModesDePaiement();
            SessionObject.ListeControlesCaisse.Add(this);
            Initialisationctrl(_ListeFactureAreglee);
            Btn_AjouterAutre.IsEnabled = false;
            this.Txt_NumCheque.MaxLength = SessionObject.Enumere.TailleNumeroCheque;
            IsRegrouper = IsPaiementRegrouper;

            if (SessionObject.LaCaisseCourante == null || SessionObject.LaCaisseCourante.FK_IDCAISSE == 0)
                ChargerHabilitationCaisse();


            if (SessionObject.LstFraisTimbre == null || SessionObject.LstFraisTimbre.Count == 0)
                RetourneLstFraixTimbre();

        }



        private void ChargerHabilitationCaisse()
        {
            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.RetouneLaCaisseCouranteAsync(UserConnecte.matricule);
            service.RetouneLaCaisseCouranteCompleted += (sender, es) =>
            {
                try
                {
                    if (es.Cancelled || es.Error != null)
                    {
                        Message.ShowError("Erreur d'invocation du service . Veuillez réssayer svp!", Galatee.Silverlight.Resources.Langue.errorTitle);
                        this.DialogResult = true;
                    }

                    if (es.Result == null)
                    {
                        Message.ShowError("Problème d'habilitation caisse", Langue.errorTitle);
                        return;
                    }

                    SessionObject.LaCaisseCourante = es.Result;
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
            };

        }





        private void RemplirModesDePaiement()
        {
            this.cbo_OtherPaiement.ItemsSource = SessionObject.ListeModesReglement.Where(t => t.CODE != "1" && t.CODE != "2").ToList();
        }
        private void Initialisationctrl(List<CsLclient> _LstFactureAregler)
        {
            try
            {
                InitialisationChamp();

                if (IsSaisiCheque == false)
                    this.Chk_Cash.IsChecked = true;
                MontantFacture = InitValue;
                MantantTimbre = InitValue;
                string _MatriculeConnect = string.Empty;
                string _NumCaissConect = string.Empty;
                this.lb_Matricule.Content = _MatriculeConnect;
                this.lb_Numcaisse.Content = _NumCaissConect;

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

                List<CsClient> LstClient = MethodeGenerics.RetourneClientFromFacture(_LstFactureAregler);
                NbreClient = LstClient.Count;
                decimal? MontantFactureClient = 0;

                foreach (CsClient item in LstClient)
                {
                    List<CsLclient> lstFactureClient = _LstFactureAregler.Where(t => t.FK_IDCLIENT == item.PK_ID).ToList();
                    List<CsLclient> _ListFact = FactureSelectionnerClient(lstFactureClient);
                    if (_ListFact.Count == 0 && LstClient.Count == 1)
                    {
                        _ListFact = ListeFactureAreglee;
                        ListeFactureAreglee.ForEach(t => t.Selectionner = true);
                    }
                    foreach (CsLclient factur in _ListFact)
                    {
                        decimal? ValeurPaye = (factur.SOLDEFACTURE != factur.MONTANTPAYE) ? factur.MONTANTPAYE : factur.SOLDEFACTURE;
                        MontantFactureClient = MontantFactureClient + ValeurPaye;
                    }
                    if (_ListFact.Count == 0 && LstClient.Count == 1) continue;
                    MontantFactureClient = _ListFact.Sum(t => t.MONTANTPAYE);

                    MontantFacture = MontantFacture + MontantFactureClient;
                    if (_LstFactureAregler.First().TYPEDEMANDE != SessionObject.Enumere.RemboursementAvance &&
                        _LstFactureAregler.First().TYPEDEMANDE != SessionObject.Enumere.AchatTimbre &&
                        _LstFactureAregler.First().TYPEDEMANDE != SessionObject.Enumere.RemboursementTrvxNonRealise)
                        MantantTimbre = MantantTimbre + CalculMontantTimbre(MontantFactureClient.Value);
                    _LstFactureAregler.Where(h => h.FK_IDCLIENT == item.PK_ID).ToList().ForEach(t => t.FRAISTIMBRE = CalculMontantTimbre(MontantFactureClient.Value));
                }

                this.txt_MontantPayeEspece.Text = this.Txt_MontantFacture.Text = this.Txt_MontantEspece.Text = MontantFacture.Value.ToString(SessionObject.FormatMontant);
                this.txt_MontantTimbreEspece.Text = Txt_FraisTimbre.Text = MantantTimbre.Value.ToString(SessionObject.FormatMontant);
                this.Txt_MontantEncaisse.Text = txt_MontantRecu.Text = (Convert.ToDecimal(this.Txt_MontantEspece.Text) + Convert.ToDecimal(this.Txt_FraisTimbre.Text)).ToString(SessionObject.FormatMontant);
                txt_MontantTotalRegle.Text = txt_MontantRecu.Text;

                _NumCaissConect = _LstFactureAregler[0].CAISSE;
                if (OperationCaisse == SessionObject.Enumere.OperationDeCaisseEncaissementFactureManuel)
                {
                    Txt_NumRecu.IsEnabled = true;
                    OKButton.IsEnabled = false;
                    _MatriculeConnect = _LstFactureAregler[0].SAISIPAR;
                }
                else
                    _MatriculeConnect = _LstFactureAregler[0].MATRICULE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        private CsLclient GetFraisTimbre(CsLclient laFacture, CsModereglement ModePayement)
        {
            #region Frais de timbre

            CsLclient factTimbre = new CsLclient()
            {
                CENTRE = laFacture.CENTRE,
                CLIENT = laFacture.CLIENT,
                ORDRE = laFacture.ORDRE,
                REFEM = laFacture.REFEM,
                COPER = "100",
                CAISSE = SessionObject.LePosteCourant.NUMCAISSE,
                DC = laFacture.DC,
                NDOC = "TIMBRE",
                NATURE = "99",
                NUMDEM = laFacture.NUMDEM,
                FK_IDETAPEDEVIS = laFacture.FK_IDETAPEDEVIS,
                CODE_WKF = laFacture.CODE_WKF,
                MONTANT = laFacture.FRAISTIMBRE,
                NOM = laFacture.NOM,
                MATRICULE = UserConnecte.matricule,
                DATECREATION = DateTime.Today.Date,
                DATEMODIFICATION = DateTime.Today.Date,
                ACQUIT = SessionObject.DernierNumeroDeRecu.Value.ToString(),
                FK_IDMODEREG = ModePayement.PK_ID,
                FK_IDCLIENT = laFacture.FK_IDCLIENT,
                FK_IDCENTRE = laFacture.FK_IDCENTRE,
                MONTANTEXIGIBLE = laFacture.MONTANTEXIGIBLE,
                MONTANTNONEXIGIBLE = laFacture.MONTANTNONEXIGIBLE,
                SOLDECLIENT = laFacture.SOLDECLIENT,
                PK_ID = laFacture.PK_ID,
                FK_IDMORATOIRE = laFacture.FK_IDMORATOIRE,
                IsPAIEMENTANTICIPE = laFacture.IsPAIEMENTANTICIPE,
                TYPEDEMANDE = laFacture.TYPEDEMANDE,
                ISPRESTATIONSEULEMENT = laFacture.ISPRESTATIONSEULEMENT,
                DTRANS = Convert.ToDateTime(System.DateTime.Today.Date.ToShortDateString())
            };
            if (laFacture.FK_IDLCLIENT != null)
                factTimbre.FK_IDLCLIENT = laFacture.FK_IDLCLIENT;

            if (laFacture.NUMDEVIS != null)
                factTimbre.NUMDEVIS = laFacture.NUMDEVIS != null ? laFacture.NUMDEVIS : string.Empty;
            factTimbre = GetElementDeReglement(factTimbre, ModePayement);

            return factTimbre;



            #endregion
        }
        private CsLclient GetElementDeReglement(CsLclient Facture, CsModereglement ModePaiement)
        {
            CsLclient Reglement = Facture;
            try
            {
                if (!string.IsNullOrEmpty(Reglement.REFEM) && Reglement.REFEM.Length == 7)
                    Reglement.REFEM = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(Facture.REFEM);

                Reglement.ACQUIT = this.Txt_NumRecu.Text;
                Reglement.CAISSE = SessionObject.LePosteCourant.NUMCAISSE;
                Reglement.DC = Facture.DC;
                Reglement.MODEREG = ModePaiement.CODE;
                Reglement.MOISCOMPT = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00");
                Reglement.TOP1 = SessionObject.Enumere.TopCaisse;
                Reglement.ECART = 0;
                Reglement.PERCU = ModePaiement.PERCU;
                Reglement.USERCREATION = UserConnecte.matricule;
                Reglement.USERMODIFICATION = UserConnecte.matricule;
                Reglement.DATECREATION = DateTime.Now; ;
                Reglement.DATEMODIFICATION = DateTime.Now;
                Reglement.ORIGINE = UserConnecte.Centre;
                Reglement.POSTE = SessionObject.LePosteCourant.NOMPOSTE;
                Reglement.DENR = Convert.ToDateTime(System.DateTime.Today.Date.ToShortDateString());
                Reglement.DTRANS = Convert.ToDateTime(System.DateTime.Today.Date.ToShortDateString());
                Reglement.RENDU = MantantRendu.Value;
                Reglement.NOMCAISSIERE = UserConnecte.nomUtilisateur;
                if (ModePaiement.CODE == SessionObject.Enumere.ModePayementCheque)
                {
                    Reglement.NUMCHEQ = this.Txt_NumCheque.Text;
                    Reglement.PLACE = SessionObject.Enumere.EtatChequeChequeSurPlace;
                    Reglement.BANQUE = (string.IsNullOrEmpty(_bankselectioner.CODE)) ? null : _bankselectioner.CODE;
                    Reglement.GUICHET = (string.IsNullOrEmpty(_bankselectioner.CODE)) ? null : _bankselectioner.CODE;
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
                    Reglement.ORIGINE = UserConnecte.CentreSelected;
                }
                Reglement.NUMDEM = Facture.NUMDEM;
                Reglement.FK_IDETAPEDEVIS = Facture.FK_IDETAPEDEVIS;

                Reglement.NUMDEVIS = Facture.NUMDEVIS;
                Reglement.FK_IDCENTRE = Facture.FK_IDCENTRE;
                Reglement.FK_IDLCLIENT = Facture.PK_ID;
                Reglement.FK_IDCLIENT = Facture.FK_IDCLIENT;
                Reglement.FK_IDCOPER = SessionObject.LstDesCopers.First(t => t.CODE == Facture.COPER).PK_ID;
                //Reglement.NATURE = "00";
                //Reglement.FK_IDNATURE = SessionObject.LstDesNature.First(t => t.CODE == "00").PK_ID;
                Reglement.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                Reglement.FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.First(t => t.CODE == SessionObject.Enumere.TopCaisse).PK_ID;
                Reglement.FK_IDHABILITATIONCAISSE = SessionObject.LaCaisseCourante.PK_ID;
                Reglement.FK_IDCAISSIERE = SessionObject.LaCaisseCourante.FK_IDCAISSIERE;
                Reglement.FK_IDAGENTSAISIE = null;
                Reglement.FK_IDPOSTECLIENT = null;
                Reglement.FK_IDMODEREG = ModePaiement.PK_ID;
                Reglement.FK_IDMORATOIRE = Facture.FK_IDMORATOIRE;
                Reglement.MATRICULE = UserConnecte.matricule;
                Reglement.LIBELLESITE = SessionObject.LstCentre.FirstOrDefault(t => SessionObject.LaCaisseCourante.FK_IDCENTRE == t.PK_ID).LIBELLESITE;
                Reglement.LIBELLEAGENCE = SessionObject.LstCentre.FirstOrDefault(t => SessionObject.LaCaisseCourante.FK_IDCENTRE == t.PK_ID).LIBELLE;
                Reglement.LIBELLEAGENCE = SessionObject.LstCentre.FirstOrDefault(t => SessionObject.LaCaisseCourante.FK_IDCENTRE == t.PK_ID).LIBELLE;
                Reglement.IsPAIEMENTANTICIPE = Facture.IsPAIEMENTANTICIPE;


                Reglement.MONTANTEXIGIBLE = Facture.MONTANTEXIGIBLE;
                Reglement.MONTANTNONEXIGIBLE = Facture.MONTANTNONEXIGIBLE;
                Reglement.SOLDECLIENT = Facture.SOLDECLIENT;
                Reglement.TYPEDEMANDE = Facture.TYPEDEMANDE;
                Reglement.ISPRESTATIONSEULEMENT = Facture.ISPRESTATIONSEULEMENT;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Reglement;
        }
        private CsLclient GetElementDeReglementExoTVA(CsLclient Facture, CsModereglement ModePaiement)
        {
            CsLclient Reglement = Facture;
            try
            {
                if (!string.IsNullOrEmpty(Reglement.REFEM) && Reglement.REFEM.Length == 7)
                    Reglement.REFEM = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(Facture.REFEM);
                Reglement.COPER = SessionObject.Enumere.CoperEXO;
                Reglement.DENR = DateTime.Today.Date;
                Reglement.ACQUIT = this.Txt_NumRecu.Text;
                Reglement.MATRICULE = Facture.MATRICULE;
                Reglement.MODEREG = ModePaiement.CODE;
                Reglement.MOISCOMPT = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00");
                Reglement.TOP1 = SessionObject.Enumere.TopCaisse;
                Reglement.ECART = 0;
                Reglement.NUMDEM = Facture.NUMDEM;
                Reglement.FK_IDETAPEDEVIS = Facture.FK_IDETAPEDEVIS;
                Reglement.SAISIPAR = Facture.SAISIPAR;
                Reglement.CRET = Facture.CRET;
                Reglement.USERCREATION = Facture.USERCREATION == null ? UserConnecte.matricule : Facture.USERCREATION;
                Reglement.USERMODIFICATION = UserConnecte.matricule;
                Reglement.DATECREATION = Facture.DATECREATION == null ? DateTime.Now : Facture.DATECREATION;
                Reglement.DATEMODIFICATION = Facture.DATEMODIFICATION == null ? DateTime.Now : Facture.DATEMODIFICATION;
                Reglement.ORIGINE = UserConnecte.Centre;
                Reglement.POSTE = SessionObject.LePosteCourant.NOMPOSTE;
                if (ModePaiement.CODE == SessionObject.Enumere.ModePayementCheque)
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
                Reglement.DTRANS = Convert.ToDateTime(System.DateTime.Today.Date.ToShortDateString());
                Reglement.DATEVALEUR = System.DateTime.Today.Date;
                Reglement.NOMCAISSIERE = UserConnecte.nomUtilisateur;
                // encaissement manuel
                if (OperationCaisse == SessionObject.Enumere.OperationDeCaisseEncaissementFactureManuel)
                {
                    Reglement.ORIGINE = UserConnecte.CentreSelected;
                }
                Reglement.MONTANTEXIGIBLE = Facture.MONTANTEXIGIBLE;
                Reglement.MONTANTNONEXIGIBLE = Facture.MONTANTNONEXIGIBLE;
                Reglement.SOLDECLIENT = Facture.SOLDECLIENT;
                Reglement.MONTANT = Facture.MONTANTTVA;
                Reglement.TYPEDEMANDE = Facture.TYPEDEMANDE;
                Reglement.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperEXO).PK_ID;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Reglement;
        }

        bool IsCliker = false;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsCliker == false)
                {
                    IsCliker = true;
                    this.OKButton.IsEnabled = false;
                    if (VerifieInfoSaisi())
                        ValiderPaiement(ListeFactureAreglee);
                }
            }
            catch (Exception ex)
            {
                this.OKButton.IsEnabled = true;
                Message.ShowError(ex, Langue.errorTitle);
            }
        }

        private void ValiderPaiement(List<CsLclient> lstFacture)
        {
            try
            {
                // Reglement par cheque 
                if (Chk_Cheque.IsChecked == true)
                {
                    CsModereglement leModeReglementCheque = SessionObject.ListeModesReglement.FirstOrDefault(m => m.CODE == SessionObject.Enumere.ModePayementCheque);
                    if (leModeReglementCheque == null)
                    {
                        Message.Show(Langue.ErreurModeReglement, Langue.LibelleModule);
                        return;
                    }
                    leModeReglementCheque.MONTANT = System.Convert.ToDecimal(this.txt_MontantRecuCheque.Text);
                    leModeReglementCheque.PERCU = System.Convert.ToDecimal(this.txt_MontantRecuCheque.Text);
                    LstDesReglementAValider.AddRange(ConstruireListeReglement(lstFacture, leModeReglementCheque));
                    MantantRendu = 0;
                }
                // Reglement par autre 
                if (Chk_Autre.IsChecked == true)
                {
                    if (lesModRegDuGrid != null && lesModRegDuGrid.Count != 0)
                        foreach (CsModereglement item in lesModRegDuGrid)
                        {
                            item.PERCU = item.MONTANT;
                            MantantRendu = 0;
                            LstDesReglementAValider.AddRange(ConstruireListeReglement(lstFacture, item));
                        }
                }
                //
                // Reglement par espece 
                if (Chk_Cash.IsChecked == true)
                {



                    /** ZEG 29/08/2017 **/
                    if (lstFacture.First().TYPEDEMANDE != SessionObject.Enumere.RemboursementAvance &&
                        lstFacture.First().TYPEDEMANDE != SessionObject.Enumere.AchatTimbre &&
                        lstFacture.First().TYPEDEMANDE != SessionObject.Enumere.RemboursementTrvxNonRealise)
                    {
                        if (System.Convert.ToDecimal(this.Txt_FraisTimbre.Text) == 0)
                        {
                            Message.ShowInformation("Frais de timbre non positionné", "Caisse");
                            OKButton.IsEnabled = true;
                            return;
                        }
                    }

                    if (System.Convert.ToDecimal(this.Txt_MontantEncaisse.Text) < (System.Convert.ToDecimal(this.Txt_MontantEspece.Text) + System.Convert.ToDecimal(this.Txt_FraisTimbre.Text)))
                    {
                        Message.ShowInformation("Le montant perçu ne doit pas être inférieur au montant à payer", "Caisse");
                        OKButton.IsEnabled = true;
                        return;
                    }

                    /** ZEG **/



                    if ((Convert.ToDecimal(this.txt_MontantRendu.Text) > 0 && Convert.ToDecimal(this.Txt_MontantFacture.Text) != 0)
                    || (System.Convert.ToDecimal(this.txt_MontantPayeEspece.Text) > Convert.ToDecimal(Txt_MontantFacture.Text) + Convert.ToDecimal(this.Txt_FraisTimbre.Text)))
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.LibelleModule, Langue.MsgMontantEleve, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            if (w.Result == MessageBoxResult.OK)
                            {
                                var ws = new MessageBoxControl.MessageBoxChildWindow(Langue.LibelleModule, Langue.MsgOverpayement, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                                ws.OnMessageBoxClosed += (l, results) =>
                                {
                                    if (ws.Result == MessageBoxResult.OK)
                                    {
                                        CsModereglement leModeReglementEspece = SessionObject.ListeModesReglement.FirstOrDefault(m => m.CODE == SessionObject.Enumere.ModePayementEspece);
                                        if (leModeReglementEspece == null)
                                        {
                                            Message.Show(Langue.ErreurModeReglement, Langue.LibelleModule);
                                            return;
                                        }
                                        leModeReglementEspece.MONTANT = System.Convert.ToDecimal(this.txt_MontantRecu.Text);
                                        leModeReglementEspece.PERCU = System.Convert.ToDecimal(this.Txt_MontantEncaisse.Text);
                                        MantantRendu = System.Convert.ToDecimal(this.txt_MontantRendu.Text);

                                        LstDesReglementAValider.AddRange(ConstruireListeReglement(lstFacture, leModeReglementEspece));



                                        /** ZEG 29/08/2017 **/
                                        if (lstFacture.First().TYPEDEMANDE != SessionObject.Enumere.RemboursementAvance &&
                                            lstFacture.First().TYPEDEMANDE != SessionObject.Enumere.AchatTimbre &&
                                            lstFacture.First().TYPEDEMANDE != SessionObject.Enumere.RemboursementTrvxNonRealise)
                                        {
                                            List<CsLclient> lesTimbres = new List<CsLclient>();
                                            lesTimbres.AddRange(LstDesReglementAValider.Where(t => t.NDOC == "TIMBRE").ToList());

                                            if (lesTimbres == null || lesTimbres.Count == 0)
                                            {
                                                Message.ShowInformation("Les frais de timbre n'ont pas été pris en compte", "Caisse");
                                                OKButton.IsEnabled = true;
                                                return;
                                            }
                                        }
                                        /** ZEG **/



                                        InsererEncaissement(LstDesReglementAValider, OperationCaisse);

                                    }
                                    else
                                    {
                                        CsModereglement leModeReglementEspece = SessionObject.ListeModesReglement.FirstOrDefault(m => m.CODE == SessionObject.Enumere.ModePayementEspece);
                                        if (leModeReglementEspece == null)
                                        {
                                            Message.Show(Langue.ErreurModeReglement, Langue.LibelleModule);
                                            return;
                                        }
                                        leModeReglementEspece.MONTANT = System.Convert.ToDecimal(this.Txt_MontantEncaisse.Text);
                                        leModeReglementEspece.PERCU = System.Convert.ToDecimal(this.Txt_MontantEncaisse.Text);
                                        MantantRendu = 0;
                                        LstDesReglementAValider.AddRange(ConstruireListeReglement(lstFacture, leModeReglementEspece));


                                        /** ZEG 29/08/2017 **/
                                        if (lstFacture.First().TYPEDEMANDE != SessionObject.Enumere.RemboursementAvance &&
                                            lstFacture.First().TYPEDEMANDE != SessionObject.Enumere.AchatTimbre &&
                                            lstFacture.First().TYPEDEMANDE != SessionObject.Enumere.RemboursementTrvxNonRealise)
                                        {
                                            List<CsLclient> lesTimbres = new List<CsLclient>();
                                            lesTimbres.AddRange(LstDesReglementAValider.Where(t => t.NDOC == "TIMBRE").ToList());

                                            if (lesTimbres == null || lesTimbres.Count == 0)
                                            {
                                                Message.ShowInformation("Les frais de timbre n'ont pas été pris en compte", "Caisse");
                                                OKButton.IsEnabled = true;
                                                return;
                                            }
                                        }
                                        /** ZEG **/

                                        InsererEncaissement(LstDesReglementAValider, OperationCaisse);
                                        return;
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
                            OKButton.IsEnabled = true;
                            return;
                        }
                        leModeReglementEspece.MONTANT = System.Convert.ToDecimal(this.txt_MontantRecu.Text);
                        leModeReglementEspece.PERCU = System.Convert.ToDecimal(this.Txt_MontantEncaisse.Text);
                        MantantRendu = 0;
                        LstDesReglementAValider.AddRange(ConstruireListeReglement(lstFacture, leModeReglementEspece));


                        /** ZEG 29/08/2017 **/
                        if (lstFacture.First().TYPEDEMANDE != SessionObject.Enumere.RemboursementAvance &&
                            lstFacture.First().TYPEDEMANDE != SessionObject.Enumere.AchatTimbre &&
                            lstFacture.First().TYPEDEMANDE != SessionObject.Enumere.RemboursementTrvxNonRealise)
                        {
                            List<CsLclient> lesTimbres = new List<CsLclient>();
                            lesTimbres.AddRange(LstDesReglementAValider.Where(t => t.NDOC == "TIMBRE").ToList());

                            if (lesTimbres == null || lesTimbres.Count == 0)
                            {
                                Message.ShowInformation("Les frais de timbre n'ont pas été pris en compte", "Caisse");
                                OKButton.IsEnabled = true;
                                return;
                            }
                        }
                        /** ZEG **/

                        InsererEncaissement(LstDesReglementAValider, OperationCaisse);
                        return;
                    }
                }
                else
                    InsererEncaissement(LstDesReglementAValider, OperationCaisse);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private bool VerifieInfoSaisi()
        {
            try
            {

                if (string.IsNullOrEmpty(Txt_NumRecu.Text) || Txt_NumRecu.Text.Length != 9)
                {
                    Message.Show(Langue.msg_entrez_numero_recu, Langue.LibelleModule);
                    this.OKButton.IsEnabled = true;
                    return false;
                }
                if (Chk_Cheque.IsChecked == true && (string.IsNullOrEmpty(Txt_NumCheque.Text) || Txt_NumCheque.Text.Length < SessionObject.Enumere.TailleNumeroCheque))
                {
                    Message.ShowInformation(Langue.msg_numero_cheque, Langue.errorTitle);
                    this.OKButton.IsEnabled = true;
                    return false;
                }
                if (Chk_Cash.IsChecked == true && Chk_Cheque.IsChecked == false && Convert.ToDecimal(this.Txt_MontantEncaisse.Text) < (Convert.ToDecimal(this.Txt_MontantEspece.Text)))
                {
                    Message.ShowInformation(Langue.msg_PayerInferieurDu, Langue.errorTitle);
                    this.OKButton.IsEnabled = true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Langue.errorTitle);
            }
            return true;
        }
        private List<CsLclient> ConstruireListeReglement(List<CsLclient> _ListeFactureAreglee, CsModereglement _ModePaiement)
        {
            try
            {
                List<CsLclient> lstFactureAValider = new List<CsLclient>();

                decimal? MontantPaye = _ModePaiement.MONTANT;
                List<CsClient> _ListeClientSelect = MethodeGenerics.RetourneClientFromFacture(_ListeFactureAreglee);
                foreach (CsClient leClient in _ListeClientSelect)
                {
                    List<CsLclient> ListeDesFactureDuClient = new List<CsLclient>();
                    if (NbreClient == 1)
                        ListeDesFactureDuClient = _ListeFactureAreglee.Where(m => m.FK_IDCLIENT == leClient.PK_ID && m.traiter == false).ToList();
                    else
                        ListeDesFactureDuClient = _ListeFactureAreglee.Where(m => m.FK_IDCLIENT == leClient.PK_ID && m.traiter == false && m.Selectionner == true).ToList();

                    if (ListeDesFactureDuClient.Count == 0) continue;

                    CsLclient leFraisTimbre = new CsLclient();
                    bool IsTimbreCalculer = false;
                    // Gestion des factures selectionnées //
                    List<CsLclient> lstFactureSelectionner = ListeDesFactureDuClient.Where(t => t.Selectionner).ToList();
                    foreach (CsLclient laFactureSelect in lstFactureSelectionner)
                    {

                        if (string.IsNullOrEmpty(laFactureSelect.CAISSE))
                            laFactureSelect.CAISSE = SessionObject.LaCaisseCourante.NUMCAISSE;

                        // Gestion des frais de timbre
                        if (!IsTimbreCalculer && _ModePaiement.CODE == SessionObject.Enumere.ModePayementEspece
                             && (laFactureSelect.TYPEDEMANDE != SessionObject.Enumere.RemboursementAvance &&
                        laFactureSelect.TYPEDEMANDE != SessionObject.Enumere.AchatTimbre &&
                        laFactureSelect.TYPEDEMANDE != SessionObject.Enumere.RemboursementTrvxNonRealise))
                        {
                            leFraisTimbre = GetFraisTimbre(laFactureSelect, _ModePaiement);
                            MontantPaye = MontantPaye - laFactureSelect.FRAISTIMBRE;
                            IsTimbreCalculer = true;
                        }
                        CsLclient _LeReglement = GetElementDeReglement(laFactureSelect, _ModePaiement);
                        if (MontantPaye != 0)
                        {
                            if (laFactureSelect.Selectionner && !laFactureSelect.traiter)
                            {
                                if (MontantPaye >= laFactureSelect.SOLDEFACTURE)
                                {
                                    _LeReglement.MONTANT = laFactureSelect.SOLDEFACTURE;
                                    MontantPaye = MontantPaye - _LeReglement.MONTANT;
                                    laFactureSelect.traiter = true;
                                    lstFactureAValider.Add(ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(_LeReglement)); //Peupler la liste de reglement
                                    if (laFactureSelect.IsExonerationTaxe)
                                        lstFactureAValider.Add(ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(GetElementDeReglementExoTVA(laFactureSelect, _ModePaiement)));
                                    continue;
                                }
                                if (MontantPaye < laFactureSelect.SOLDEFACTURE)
                                {
                                    _LeReglement.MONTANT = MontantPaye.Value;
                                    laFactureSelect.SOLDEFACTURE = laFactureSelect.SOLDEFACTURE - _LeReglement.MONTANT.Value;
                                    MontantPaye = 0;
                                    lstFactureAValider.Add(ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(_LeReglement)); //Peupler la liste de reglement
                                    if (laFactureSelect.IsExonerationTaxe)
                                        lstFactureAValider.Add(ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(GetElementDeReglementExoTVA(laFactureSelect, _ModePaiement)));
                                    continue;
                                }
                            }
                        }
                        else break;
                    }
                    if (_ModePaiement.CODE == SessionObject.Enumere.ModePayementEspece &&
                        (ListeDesFactureDuClient.First().TYPEDEMANDE != SessionObject.Enumere.RemboursementAvance &&
                        ListeDesFactureDuClient.First().TYPEDEMANDE != SessionObject.Enumere.AchatTimbre &&
                        ListeDesFactureDuClient.First().TYPEDEMANDE != SessionObject.Enumere.RemboursementTrvxNonRealise))
                        lstFactureAValider.Add(ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(leFraisTimbre));

                    if (NbreClient != 1) continue;
                    // Gestion des facture non selectionnee
                    List<CsLclient> lstFactureNonSelectionner = ListeDesFactureDuClient.Where(t => !t.Selectionner).OrderBy(y => y.REFEM).ToList();
                    foreach (var laFactureNonSelect in lstFactureNonSelectionner)
                    {
                        CsLclient _LeReglement = GetElementDeReglement(laFactureNonSelect, _ModePaiement);
                        if (MontantPaye != 0)
                        {
                            if (!laFactureNonSelect.traiter)
                            {
                                if (MontantPaye >= laFactureNonSelect.SOLDEFACTURE)
                                {
                                    _LeReglement.MONTANT = laFactureNonSelect.SOLDEFACTURE;
                                    MontantPaye = MontantPaye - _LeReglement.MONTANT;
                                    laFactureNonSelect.traiter = true;
                                    lstFactureAValider.Add(ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(_LeReglement)); //Peupler la liste de reglement
                                    continue;
                                }
                                if (MontantPaye < laFactureNonSelect.SOLDEFACTURE)
                                {
                                    _LeReglement.MONTANT = MontantPaye.Value;
                                    laFactureNonSelect.SOLDEFACTURE = laFactureNonSelect.SOLDEFACTURE - _LeReglement.MONTANT.Value;
                                    MontantPaye = MontantPaye - _LeReglement.MONTANT;
                                    lstFactureAValider.Add(ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(_LeReglement)); //Peupler la liste de reglement
                                    continue;
                                }
                            }
                        }
                        else break;
                    }
                }
                if (MontantPaye > 0)
                {
                    CsLclient _LeReglementNaf = ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(_ListeFactureAreglee.First());
                    _LeReglementNaf.REFEM = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00");
                    _LeReglementNaf.COPER = SessionObject.Enumere.CoperRGT;
                    _LeReglementNaf.DC = SessionObject.Enumere.Debit;
                    _LeReglementNaf.ACQUIT = this.Txt_NumRecu.Text;
                    _LeReglementNaf.DENR = DateTime.Today.Date;
                    _LeReglementNaf.MONTANT = MontantPaye;
                    _LeReglementNaf.NDOC = string.Empty;
                    _LeReglementNaf.IsPAIEMENTANTICIPE = true;

                    _LeReglementNaf.FK_IDCOPER = SessionObject.LstDesCopers.First(t => t.CODE == _LeReglementNaf.COPER).PK_ID;
                    _LeReglementNaf.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                    _LeReglementNaf.FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.First(t => t.CODE == SessionObject.Enumere.TopCaisse).PK_ID;
                    _LeReglementNaf.FK_IDHABILITATIONCAISSE = SessionObject.LaCaisseCourante.PK_ID;
                    _LeReglementNaf.FK_IDCAISSIERE = SessionObject.LaCaisseCourante.FK_IDCAISSIERE;
                    _LeReglementNaf.FK_IDAGENTSAISIE = null;
                    _LeReglementNaf.FK_IDPOSTECLIENT = null;
                    _LeReglementNaf.LIBELLECOPER = SessionObject.LstDesCopers.First(t => t.CODE == SessionObject.Enumere.CoperDSD).LIBELLE;

                    if (string.IsNullOrEmpty(_LeReglementNaf.CAISSE))
                        _LeReglementNaf.CAISSE = SessionObject.LaCaisseCourante.NUMCAISSE;

                    _LeReglementNaf = GetElementDeReglement(_LeReglementNaf, _ModePaiement);
                    LstDesReglementAValider.Add(_LeReglementNaf);
                }


                return lstFactureAValider;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void InsererEncaissement(List<CsLclient> LstDesReglementAInserer, string Action)
        {
            #region impression normale
            if (LstDesReglementAInserer == null || LstDesReglementAInserer.Count == 0)
            {
                Message.ShowInformation("Aucun règlement de facture en cours ...", "Erreur");
                return;
            }
            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.InsererEncaissementAsync(LstDesReglementAInserer, OperationCaisse);
            service.InsererEncaissementCompleted += (sender, es) =>
            {
                if (es.Error != null || es.Cancelled)
                {
                    Message.ShowError("L'insertion des encaissements a retourné une erreur. Réessayer svp! :" + es.Error.Message, "Erreur");
                    return;
                }
                if (es.Result == null)
                {
                    Message.ShowError("L'insertion des encaissements a retourné une erreur. Réessayer svp!", "Erreur");
                    return;
                }

                //if (!string.IsNullOrEmpty(LstDesReglementAInserer.First().NUMDEM))
                //{
                //    List<string> codes = new List<string>();
                //    codes.Add(LstDesReglementAInserer.First().CODE_WKF);
                //    ClasseMEthodeGenerique.TransmettreDemande(codes,false );
                //}

                reject = true;
                this.DialogResult = true;
                SessionObject.DernierNumeroDeRecu += 1;
                LstDesReglementAInserer = LstDesReglementAInserer.Where(p => p.COPER != SessionObject.Enumere.CoperNAF || p.COPER != SessionObject.Enumere.CoperRNA).ToList();

                List<CsClient> _lstClientRecu = Galatee.Silverlight.Caisse.MethodeGenerics.RetourneClientFromFacture(LstDesReglementAInserer);
                List<CsClient> lesClientDuRecu = new List<CsClient>();
                foreach (var item in _lstClientRecu)
                {

                    CsClient leClientDuRecu = new CsClient();
                    leClientDuRecu.CENTRE = item.CENTRE;
                    leClientDuRecu.REFCLIENT = item.REFCLIENT;
                    leClientDuRecu.ORDRE = item.ORDRE;
                    leClientDuRecu.FK_IDCENTRE = item.FK_IDCENTRE;
                    lesClientDuRecu.Add(leClientDuRecu);
                }
                CaisseServiceClient services = new CaisseServiceClient(Utility.Protocole(), Utility.EndPoint("Caisse"));
                service.RetourneListeFactureNonSoldeClientAsync(lesClientDuRecu);
                service.RetourneListeFactureNonSoldeClientCompleted += (senders, ess) =>
                {

                    if (ess != null && ess.Cancelled)
                        return;
                    List<CsReglementRecu> _ListeDesRecu = new List<CsReglementRecu>();
                    List<CsLclient> leFacturePayeeClient = ess.Result;
                    List<CsClient> _lstClientDistinctRecu = Galatee.Silverlight.Caisse.MethodeGenerics.RetourneClientFromFacture(LstDesReglementAInserer);
                    foreach (CsClient item in _lstClientDistinctRecu)
                    {

                        List<CsLclient> lstFactureDuClient = LstDesReglementAInserer.Where(t => t.FK_IDCLIENT == item.PK_ID).ToList();

                        if (leFacturePayeeClient != null && leFacturePayeeClient.Count != 0)
                        {
                            decimal? MontantPercu = lstFactureDuClient.Sum(o => o.MONTANT);
                            if (_lstClientRecu.Count > 1)
                                lstFactureDuClient.ForEach(t => t.PERCU = MontantPercu);

                            List<CsLclient> leFactureRestantClientDist = leFacturePayeeClient.Where(t => t.FK_IDCLIENT == item.PK_ID).ToList();
                            if (leFactureRestantClientDist != null && leFactureRestantClientDist.Count != 0 && leFactureRestantClientDist.First().SOLDECLIENT != 0)
                            {


                                decimal? MontantExig = leFactureRestantClientDist.Where(t => t.FK_IDCLIENT == item.PK_ID && t.EXIGIBILITE < System.DateTime.Today.Date).Sum(y => y.SOLDEFACTURE);
                                decimal? MontantNonExig = leFactureRestantClientDist.Where(t => t.FK_IDCLIENT == item.PK_ID && t.EXIGIBILITE > System.DateTime.Today.Date).Sum(y => y.SOLDEFACTURE);


                                lstFactureDuClient.ForEach(u => u.MONTANTEXIGIBLE = MontantExig == null ? 0 : MontantExig);
                                lstFactureDuClient.ForEach(u => u.MONTANTNONEXIGIBLE = MontantNonExig == null ? 0 : MontantNonExig);
                                lstFactureDuClient.ForEach(u => u.SOLDECLIENT = leFactureRestantClientDist.First().SOLDECLIENT == null ? 0 : leFactureRestantClientDist.First().SOLDECLIENT);
                            }
                            else
                            {
                                lstFactureDuClient.ForEach(u => u.MONTANTEXIGIBLE = 0);
                                lstFactureDuClient.ForEach(u => u.MONTANTNONEXIGIBLE = 0);
                                lstFactureDuClient.ForEach(u => u.SOLDECLIENT = 0);
                            }
                        }
                        else
                        {
                            lstFactureDuClient.ForEach(u => u.MONTANTEXIGIBLE = 0);
                            lstFactureDuClient.ForEach(u => u.MONTANTNONEXIGIBLE = 0);
                            lstFactureDuClient.ForEach(u => u.SOLDECLIENT = 0);
                        }
                        _ListeDesRecu.AddRange(EditionRecu.ReorganiserReglement(ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(lstFactureDuClient), OperationCaisse));
                    }
                    string key = Utility.getKey();
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    if (!_IsDecaissement)
                        param.Add("pTypeRecu", "RECU D'ENCAISSEMENT");
                    else
                        param.Add("pTypeRecu", "RECU DE DECAISSEMENT");

                    for (int i = 0; i < 2; i++)
                        LanceImpression(key, param, _ListeDesRecu, false);

                    if (_lstClientRecu.Count > 1 && IsRegrouper)
                    {
                        string keys = Utility.getKey();
                        LanceImpression(keys, null, _ListeDesRecu, true);
                    }
                };
            };
            #endregion
        }
        private void LanceImpression(string key, Dictionary<string, string> parametresRDLC, List<Galatee.Silverlight.ServiceCaisse.CsReglementRecu> res, bool IsBordero)
        {
            try
            {
                if (_IsDecaissement)
                    Utility.ActionDirectOrientation<ServicePrintings.CsReglementRecu, ServiceCaisse.CsReglementRecu>(res, parametresRDLC, SessionObject.CheminImpression, "RecuDecaissement", "Caisse", true);
                else
                {
                    if (!IsBordero)
                        Utility.ActionDirectOrientation<ServicePrintings.CsReglementRecu, ServiceCaisse.CsReglementRecu>(res, parametresRDLC, SessionObject.CheminImpression, "recu", "Caisse", true);
                    else
                        Utility.ActionDirectOrientation<ServicePrintings.CsReglementRecu, ServiceCaisse.CsReglementRecu>(res, parametresRDLC, SessionObject.CheminImpression, "BorderoRegroupe", "Caisse", true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            reject = false;
            this.DialogResult = true;
        }

        private List<CsLclient> FactureSelectionnerClient(List<CsLclient> _LstFactureAregler)
        {
            try
            {
                List<CsLclient> FactSelect = (from d in _LstFactureAregler
                                              where (d.Selectionner == true)
                                              select d).ToList();
                if (FactSelect != null && FactSelect.Count != 0)
                    return FactSelect;
                return new List<CsLclient>();
            }
            catch (Exception)
            {

                throw;
            }

        }

        private List<CsLclient> FactureSelectionner(List<CsClient> ListClient, List<CsLclient> _LstFactureAregler)
        {


            List<CsLclient> FactSelect = (from d in _LstFactureAregler
                                          where (d.Selectionner == true &&
                                                  d.COPER != SessionObject.Enumere.CoperNAF)
                                          select d).ToList();
            if (FactSelect == null || FactSelect.Count == 0)
            {
                _LstFactureAregler.ForEach(t => t.Selectionner = true);
                return _LstFactureAregler;
            }

            List<CsLclient> facsClient = new List<CsLclient>();
            foreach (CsClient LeClient in ListClient)
            {
                facsClient.AddRange((from d in _LstFactureAregler
                                     where (d.FK_IDCLIENT == LeClient.PK_ID &&
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
                Double MontanY = Single.Parse(this.Txt_MontantFacture.Text) - Single.Parse(this.Txt_MontantEncaisse.Text);
                this.Txt_MontantCheque.Text = MontanY.ToString(SessionObject.FormatMontant);
                this.Txt_MontantCheque.Focus();
                OKButton.IsEnabled = true;
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
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

                decimal montantARendre = 0;

                if (!string.IsNullOrEmpty(this.Txt_MontantEspece.Text) && !string.IsNullOrEmpty(this.txt_MontantRecu.Text))
                    montantARendre = System.Convert.ToDecimal(this.Txt_MontantEspece.Text) - (System.Convert.ToDecimal(this.txt_MontantRecu.Text));
                if (montantARendre > 0)
                    this.txt_MontantRendu.Text = montantARendre.ToString();
                else
                    this.txt_MontantRendu.Text = InitValue.ToString(SessionObject.FormatMontant);

                if (this.Chk_Cash.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(this.Txt_MontantEspece.Text) && Convert.ToDecimal(this.Txt_MontantEncaisse.Text) != 0)
                        CalculMontantTimbre(Convert.ToDecimal(this.Txt_MontantEspece.Text));
                }
                //txt_MontantRecu.Text = (Convert.ToDecimal(this.Txt_MontantEspece.Text) + Convert.ToDecimal(this.Txt_FraisTimbre.Text)).ToString(SessionObject.FormatMontant);
                txt_MontantTotalRegle.Text = ((string.IsNullOrEmpty(this.txt_MontantRecu.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecu.Text)) + (string.IsNullOrEmpty(this.txt_MontantRecuCheque.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecuCheque.Text)) + (string.IsNullOrEmpty(this.txt_MontantAutre.Text) ? 0 : Convert.ToDecimal(this.txt_MontantAutre.Text))).ToString(SessionObject.FormatMontant);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        private void Txt_MontantEncaisse_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal MontantEspece = string.IsNullOrEmpty(this.Txt_MontantEncaisse.Text) ? 0 : decimal.Parse(this.Txt_MontantEncaisse.Text);
                if (MontantEspece == 0)
                    return;

                decimal montantARendre = 0;

                if (!string.IsNullOrEmpty(this.Txt_MontantEncaisse.Text) && !string.IsNullOrEmpty(this.txt_MontantRecu.Text))
                    montantARendre = System.Convert.ToDecimal(this.Txt_MontantEncaisse.Text) - (System.Convert.ToDecimal(this.txt_MontantRecu.Text));
                if (montantARendre > 0)
                    this.txt_MontantRendu.Text = montantARendre.ToString();
                else
                    this.txt_MontantRendu.Text = InitValue.ToString(SessionObject.FormatMontant);

                if (this.Chk_Cash.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(this.Txt_MontantEspece.Text) && Convert.ToDecimal(this.Txt_MontantEncaisse.Text) != 0)
                        Txt_FraisTimbre.Text = CalculMontantTimbre(Convert.ToDecimal(this.Txt_MontantEncaisse.Text)).Value.ToString(SessionObject.FormatMontant);
                }
                //txt_MontantRecu.Text = (Convert.ToDecimal(this.Txt_MontantEspece.Text) + Convert.ToDecimal(this.Txt_FraisTimbre.Text)).ToString(SessionObject.FormatMontant);
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

                if (this.Chk_Cash.IsChecked == true && !string.IsNullOrEmpty(this.Txt_MontantEncaisse.Text))
                {
                    txt_MontantRecu.Text = (Convert.ToDecimal(this.Txt_MontantEncaisse.Text)).ToString(SessionObject.FormatMontant);
                    txt_MontantPayeEspece.Text = (Convert.ToDecimal(this.Txt_MontantEncaisse.Text)).ToString(SessionObject.FormatMontant);
                }

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
                Double montantY = Single.Parse(this.Txt_MontantFacture.Text) - (Single.Parse(this.txt_MontantPayeEspece.Text) + Single.Parse(this.Txt_MontantCheque.Text));
                this.txtOtherPaie.Text = montantY.ToString(SessionObject.FormatMontant);
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
            Txt_MontantEspece.Text = "0"; ;
            Txt_MontantEncaisse.Text = "0";
            this.Txt_FraisTimbre.Text = "0";
            this.txt_MontantRendu.Text = "0";

            this.txt_MontantPayeEspece.Text = InitValue.ToString();
            this.txt_MontantTimbreEspece.Text = InitValue.ToString();
            this.txt_MontantRecu.Text = InitValue.ToString();
        }

        private void Txt_MontantEspece_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_MontantEspece.Text))
            {
                decimal Formate = Convert.ToDecimal(this.Txt_MontantEspece.Text);
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
                Initialisationctrl(ListeFactureAreglee);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }

        }

        private void Chk_InclureFrais_Checked(object sender, RoutedEventArgs e)
        {
            txt_MontantPayeEspece.Text = (Convert.ToDecimal(this.Txt_MontantFacture.Text) - Convert.ToDecimal(this.Txt_FraisTimbre.Text)).ToString(SessionObject.FormatMontant);
            txt_MontantRecu.Text = (Convert.ToDecimal(this.txt_MontantPayeEspece.Text) + Convert.ToDecimal(this.txt_MontantTimbreEspece.Text)).ToString(SessionObject.FormatMontant);
            Txt_MontantEspece.Text = (Convert.ToDecimal(this.Txt_MontantFacture.Text) - Convert.ToDecimal(this.Txt_FraisTimbre.Text)).ToString(SessionObject.FormatMontant);
            Txt_MontantEncaisse.Text = (Convert.ToDecimal(this.Txt_MontantEspece.Text) + Convert.ToDecimal(this.Txt_FraisTimbre.Text)).ToString(SessionObject.FormatMontant);
            this.txt_MontantRendu.Text = InitValue.ToString(SessionObject.FormatMontant);
        }

        private void Chk_InclureFrais_Unchecked(object sender, RoutedEventArgs e)
        {
            txt_MontantRecu.Text = Txt_MontantEspece.Text = (Convert.ToDecimal(this.Txt_MontantFacture.Text) + Convert.ToDecimal(this.Txt_FraisTimbre.Text)).ToString(SessionObject.FormatMontant);
            Txt_MontantEncaisse.Text = txt_MontantRecu.Text;
            txt_MontantPayeEspece.Text = System.Convert.ToDecimal(this.Txt_MontantFacture.Text).ToString(SessionObject.FormatMontant);
            decimal montantARendre = 0;
            if (!string.IsNullOrEmpty(this.Txt_MontantEncaisse.Text) && !string.IsNullOrEmpty(this.Txt_MontantEspece.Text))
                montantARendre = System.Convert.ToDecimal(this.Txt_MontantEncaisse.Text) - (System.Convert.ToDecimal(this.Txt_MontantEspece.Text) + (System.Convert.ToDecimal(this.Txt_FraisTimbre.Text)));
            if (montantARendre > 0)
                this.txt_MontantRendu.Text = montantARendre.ToString();
            else
                this.txt_MontantRendu.Text = InitValue.ToString(SessionObject.FormatMontant);

        }

        private void txt_MontantRecuCheque_TextChanged(object sender, TextChangedEventArgs e)
        {
            txt_MontantTotalRegle.Text = ((string.IsNullOrEmpty(this.txt_MontantRecu.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecu.Text)) + (string.IsNullOrEmpty(this.txt_MontantRecuCheque.Text) ? 0 : Convert.ToDecimal(this.txt_MontantRecuCheque.Text)) + (string.IsNullOrEmpty(this.txt_MontantAutre.Text) ? 0 : Convert.ToDecimal(this.txt_MontantAutre.Text))).ToString(SessionObject.FormatMontant);
        }



        public decimal? CalculMontantTimbre(decimal montantSaisie)
        {
            decimal? leFrais = 0;

            if (SessionObject.LstFraisTimbre != null && SessionObject.LstFraisTimbre.Count > 0)
            {
                CsFraisTimbre leFraistimbre = SessionObject.LstFraisTimbre.FirstOrDefault(p => montantSaisie >= p.BORNEINF && montantSaisie <= (p.BORNESUP + p.FRAIS));
                if (leFraistimbre != null)
                    leFrais = Convert.ToDecimal(leFraistimbre.FRAIS);

            }
            return leFrais;

        }




        private void RetourneLstFraixTimbre()
        {
            string ServerMode = "Online";

            CaisseServiceClient cais = new CaisseServiceClient(Utility.Protocole(), Utility.EndPoint("Caisse"));
            cais.RetourneListeTimbreCompleted += (send, aa) =>
            {
                try
                {
                    if (aa.Cancelled || aa.Error != null)
                    {
                        Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }
                    if (aa.Result == null || aa.Result.Count == 0)
                    {
                        Message.Show(Galatee.Silverlight.Resources.Langue.msgNodata, "RetourneListeTimbre =>" + Galatee.Silverlight.Resources.Langue.informationTitle);
                        return;
                    }
                    SessionObject.LstFraisTimbre = aa.Result;

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            };
            cais.RetourneListeTimbreAsync(ServerMode);

        }


    }
}


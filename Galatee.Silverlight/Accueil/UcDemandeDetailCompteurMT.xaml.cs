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
using Galatee.Silverlight.ServiceAccueil;
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Accueil
{
    public partial class UcDemandeDetailCompteurMT : UserControl
    {

  

        List<CsDiacomp> LstDiametre;
        List<CsMarqueCompteur> LstMarque;
        List<CsCadran> LstCadran;

        CsCanalisation     CanalisationClientRecherche = new  CsCanalisation ();
        CsEvenement LsDernierEvenement = new CsEvenement();
        List<CsEvenement> LstEvenement = new List<CsEvenement>();
        List<CsCanalisation> LstCanalisation = new List<CsCanalisation>();
        List<CsTcompteur> LstType;
        CsTcompteur  LeCompteurSelect  ;

        CsDemande LaDemande = new CsDemande();
        public CsDemande MaDemande
        {
            get { return LaDemande; }
            set { LaDemande = value; }
        }    
        public UcDemandeDetailCompteurMT()
        {

            InitializeComponent();
        }

        public UcDemandeDetailCompteurMT(CsDemande _LaDemande)
        {
            InitializeComponent();
            LaDemande = _LaDemande;
            if (LaDemande.LstCanalistion == null)
                LaDemande.LstCanalistion = new List<CsCanalisation>();
            if (LaDemande.LstEvenement == null)
                LaDemande.LstEvenement = new List<CsEvenement>();
            ChargerCadran();
            ChargerMarque();
            ChargerDiametre();
            this.Txt_DateEvt.MaxLength = SessionObject.Enumere.TailleDate;
            this.Txt_PeriodeEvt .MaxLength = 6;
            InitialiseCtrl();
        }


        void InitialiseCtrl()
        {
            this.Txt_Client.Text = string.IsNullOrEmpty(LaDemande.Branchement.CLIENT) ? string.Empty : LaDemande.Branchement.CLIENT;
            this.Txt_Addresse.Text = string.IsNullOrEmpty(LaDemande.Branchement.CLIENT) ? string.Empty : LaDemande.Branchement.CLIENT;
            //this.Txt_Ordre.Text = string.IsNullOrEmpty(LaDemande.Ag.ORDRE) ? string.Empty : LaDemande.Ag.ORDRE;
            ChargerTypeCompteur(LaDemande.Branchement.PRODUIT , LaDemande.Branchement.TYPECOMPTAGE   , null);

            if (LaDemande.LeTypeDemande.CODE   == SessionObject.Enumere.FermetureBrt)
                IsControlActif(false);

        }

        private void RetourneInfoDernierEvenementFacture(int fk_idcentre,string centre, string client, string ordre, string produit,int point)
        {
            LsDernierEvenement = new CsEvenement();
            List<CsEvenement> LstEvenement = new List<CsEvenement>();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneEvenementCompleted  += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                LstEvenement = args.Result;
                List<CsEvenement> LstEvenementFacture = new List<CsEvenement>();
                int EvtMAx = 0;
                if (LstEvenement != null && LstEvenement.Count != 0)
                {
                    LstEvenementFacture = LstEvenement.Where(p => !string.IsNullOrEmpty(p.DERPERF) && !string.IsNullOrEmpty(p.DERPERFN)).ToList();
                    if (LstEvenementFacture != null && LstEvenementFacture.Count != 0)
                    {
                        EvtMAx = LstEvenementFacture.Max(p => p.NUMEVENEMENT);
                        CsEvenement _LeDernier = LstEvenementFacture.FirstOrDefault(p => p.NUMEVENEMENT == EvtMAx);
                        if (EvtMAx < LstEvenement.Count)
                        {
                            _LeDernier.NUMEVENEMENT  = LstEvenement[LstEvenement.Count - 1].NUMEVENEMENT ;
                        }
                        LsDernierEvenement = _LeDernier;
                        List<CsEvenement> _LstEvt = new List<CsEvenement>();
                        _LstEvt.Add(LsDernierEvenement);
                        LaDemande.LstEvenement = _LstEvt;
                    }
                }
            };
            service.RetourneEvenementAsync(fk_idcentre,centre, client, ordre, produit, point);
            service.CloseAsync();

        }
        void ChargerCadran()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneToutCadranCompleted += (s, args) =>
                {
                    LstCadran = new List<CsCadran>();
                    if (args != null && args.Cancelled)
                        return;
                    LstCadran = args.Result;
                    if (LstCadran != null && LstCadran.Count != 0)
                    {
                        //if (!string.IsNullOrEmpty(this.Txt_CodeCadran.Text) &&
                        //    (!string.IsNullOrEmpty(this.Txt_LibelleDigit.Text)))
                        //{
                        //    CsCadran _LeCadran = ClasseMEthodeGenerique.RetourneObjectFromList(LstCadran, this.Txt_CodeCadran.Text, "PK_CADRAN");
                        //    if (string.IsNullOrEmpty(_LeCadran.LIBELLE))
                        //        this.Txt_CodeCadran.Text = _LeCadran.LIBELLE;
                        //}
                    }
                };
                service.RetourneToutCadranAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void ChargerMarque()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneToutMarqueCompleted += (s, args) =>
                {
                    LstMarque = new List<CsMarqueCompteur>();
                    if (args != null && args.Cancelled)
                        return;
                    LstMarque = args.Result;
                    //if (LstMarque != null && LstMarque.Count != 0)
                    //{
                    //    if (!string.IsNullOrEmpty(this.Txt_CodeMarque.Text) &&
                    //        (!string.IsNullOrEmpty(this.Txt_LibelleMarque.Text)))
                    //    {
                    //        CsMarqueCompteur _LaMarque = ClasseMEthodeGenerique.RetourneObjectFromList(LstMarque, this.Txt_CodeMarque.Text, "PK_MARQUECOMPTEUR");
                    //        if (string.IsNullOrEmpty(_LaMarque.LIBELLE))
                    //            this.Txt_CodeCadran.Text = _LaMarque.LIBELLE;
                    //    }
                    //}
                };
                service.RetourneToutMarqueAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void ChargerDiametre()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerDiametreCompteurCompleted  += (s, args) =>
                {
                    LstDiametre = new List<CsDiacomp>();
                    if (args != null && args.Cancelled)
                        return;
                    LstDiametre = args.Result;
                    if (LstDiametre != null && LstDiametre.Count != 0)
                    {
                        //if (!string.IsNullOrEmpty(this.Txt_CodeDiametre.Text))
                        //{
                        //    CsDiacomp _LeCompteur = ClasseMEthodeGenerique.RetourneObjectFromList(LstDiametre, this.Txt_CodeDiametre.Text, "PK_DIAMETRE");
                        //    if (!string.IsNullOrEmpty(_LeCompteur.LIBELLE))
                        //        this.Txt_LibelleDiametre.Text = _LeCompteur.LIBELLE;
                        //}
                    }
                };
                service.ChargerDiametreCompteurAsync ();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void ChargerTypeCompteur(string produit, string type, string centre)
        {
            if (SessionObject.LstTypeCompteur != null && SessionObject.LstTypeCompteur.Count != 0)
            {
                LstType = new List<CsTcompteur>();
                LstType = SessionObject.LstTypeCompteur;
                dataGrid1.ItemsSource = null;
                dataGrid1.ItemsSource = LstType;
                this.dataGrid1.SelectedItem = LstType[0];
                return;
            }

            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.ChargerTypeMtCompleted   += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                LstType.AddRange(args.Result);
                if (LstType.Count != 0)
                {
                    dataGrid1.ItemsSource = null;
                    dataGrid1.ItemsSource = LstType;
                    this.dataGrid1.SelectedItem = LstType[0];
                }
            };
            service.ChargerTypeMtAsync(produit , type  , centre );
            service.CloseAsync();
        }
     
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitialiseCtrl();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LeCompteurSelect = new CsTcompteur();
            LeCompteurSelect = this.dataGrid1.SelectedItem as CsTcompteur;
        }
        private int determinePoint(string tcompt)
        {
            LaDemande.LstCanalistion[0].POINT   = 1;
            if (LstCanalisation.Count != 0)
            {

                if (LstCanalisation.FirstOrDefault(p => p.TYPECOMPTEUR    == tcompt) == null)
                    LaDemande.LstCanalistion[0].POINT = LstCanalisation.Count + 1;
                
            }
            return LaDemande.LstCanalistion[0].POINT;
        }
        private void IsControlActif(bool etat)
        {
            this.dataGrid1.IsReadOnly = !etat;
            this.Txt_DateEvt.IsReadOnly = !etat;
            this.Txt_PeriodeEvt.IsReadOnly = !etat;
        }
        void _ctrl_Closed(object sender, EventArgs e)
        {
            //UcListeCompteurAuto ctr = sender as UcListeCompteurAuto;
            CsCompteur _LeCompteurSelectDatagrid = new CsCompteur();
            CsTcompteur _LeCompteurSelect = new CsTcompteur();


            //_LeCompteurSelectDatagrid = (CsCompteur)ctr.dataGrid1.SelectedItem;
            LeCompteurSelect.COMPTEUR = _LeCompteurSelectDatagrid.NUMERO ;
            LeCompteurSelect.MCOMPT = _LeCompteurSelectDatagrid.MARQUE ;
            //LeCompteurSelect.TCOMP = _LeCompteurSelectDatagrid.TCOMPT;
            LeCompteurSelect.ANNEEFAB = _LeCompteurSelectDatagrid.ANNEEFAB;
            LeCompteurSelect.POINT = determinePoint(LeCompteurSelect.TCOMP  );
            LeCompteurSelect.SAISIE = "Oui";
             
            //Inserer canalisation
            CsCanalisation _LeCanalisation = new CsCanalisation();
            //_LeCanalisation.CENTRE = LaDemande.CENTRE;
            //_LeCanalisation.CLIENT = LaDemande.CLIENT;
            //_LeCanalisation.ORDRE = LaDemande.ORDRE;
            //_LeCanalisation.POINT = LaDemande.POINT;
            //_LeCanalisation.ANNEEFAB = string.IsNullOrEmpty(_LeCompteurSelectDatagrid.ANNEEFAB) ? string.Empty : _LeCompteurSelectDatagrid.ANNEEFAB;
            //_LeCanalisation.MCOMPT = string.IsNullOrEmpty(_LeCompteurSelectDatagrid.MCOMPT) ? string.Empty : _LeCompteurSelectDatagrid.MCOMPT;
            //_LeCanalisation.COMPTEUR = string.IsNullOrEmpty(_LeCompteurSelectDatagrid.COMPTEUR) ? string.Empty : _LeCompteurSelectDatagrid.COMPTEUR ;
            //_LeCanalisation.CADCOMPT = string.IsNullOrEmpty(_LeCompteurSelectDatagrid.CADCOMP) ? string.Empty : _LeCompteurSelectDatagrid.CADCOMP;
            //_LeCanalisation.TCOMPT = LeCompteurSelect.PK_TYPE ;
            //if (LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul ||
            //    LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement)
            //    _LeCanalisation.ETATCOMPT = SessionObject.Enumere.CompteurActifValeur;

            //if (!string.IsNullOrEmpty(_LeCanalisation.COMPTEUR))
            //{
            //    if (LstCanalisation.FirstOrDefault(p=>p.TCOMPT  == _LeCanalisation.TCOMPT  )==null )
            //    LstCanalisation.Add(_LeCanalisation);
            //}
            UcDemandeSaisieCompteurMt ctrl = new UcDemandeSaisieCompteurMt(LaDemande, LeCompteurSelect);
            ctrl.Closed += new EventHandler(ctrl_Closed);
            ctrl.Show();

        }
        void ctrl_Closed(object sender, EventArgs e)
        {
            UcDemandeSaisieCompteurMt ctr=  sender as UcDemandeSaisieCompteurMt  ;
            CsTcompteur leCompteur =   ctr.EnregisterCompteur();
            
            CsTcompteur leCompeteurSelect = LstType.FirstOrDefault(t => t.POINT == leCompteur.POINT);
            if (leCompeteurSelect != null)
            {
                LeCompteurSelect.ANNEEFAB = leCompteur.ANNEEFAB;
                LeCompteurSelect.CADCOMP = leCompteur.CADCOMP;
                LeCompteurSelect.COMPTEUR  = leCompteur.COMPTEUR ;
                LeCompteurSelect.MCOMPT  = leCompteur.MCOMPT ;
                LeCompteurSelect.SAISIE  = "OUI";
            }
            dataGrid1.ItemsSource = null;
            dataGrid1.ItemsSource = LstType;
        }

        private void btn_saisir_Click(object sender, RoutedEventArgs e)
        {
            if (LaDemande.LeTypeDemande.DEMOPTION7 == SessionObject.Enumere.CodeObligatoire)
            {
                    //UcListeCompteurAuto _ctrl = new UcListeCompteurAuto(LaDemande);
                    //_ctrl.Closed += new EventHandler(_ctrl_Closed);
                    //_ctrl.Show();
            }
            else
            {
                UcDemandeSaisieCompteurMt ctrl = new UcDemandeSaisieCompteurMt(LaDemande, LeCompteurSelect);
                ctrl.Closed += new EventHandler(ctrl_Closed);
                ctrl.Show();
            }
        }
        private  void  EnregistrerEvenement(CsDemande _Lademande)
        {
            foreach (CsEvenement _item in _Lademande.LstEvenement )
            {
                _item.PERIODE = this.Txt_PeriodeEvt.Text;
            }  
        }
        private void EnregistrerCanalisation(CsDemande _Lademande)
        {
            foreach (CsCanalisation _item in _Lademande.LstCanalistion )
            {
                if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement ||
                    LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul ||
                     LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementSimple)
                {
                    if (!string.IsNullOrEmpty(this.Txt_DateEvt.Text))
                        _item.DATEPOSE = _item.MISEENSERVICE = DateTime.Parse(this.Txt_DateEvt.Text);
                }

            }
        }
        public void EnregistrerInfoSaisie(CsDemande _Lademande)
        {
            EnregistrerCanalisation(_Lademande);
            EnregistrerEvenement(_Lademande);
        }

        private void Txt_DateEvt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_DateEvt.Text.Length == SessionObject.Enumere.TailleDate )
                if (ClasseMEthodeGenerique.IsDateValide(this.Txt_DateEvt.Text)==null )
                {
                    DialogResult dialogue = new DialogResult("Date invalide", "Saisie date", false, true, false);
                    dialogue.Closed += new EventHandler(dialogue_Closed);
                    dialogue.Show();
                }

        }

        void dialogue_Closed(object sender, EventArgs e)
        {
            DialogResult ctrs = sender as DialogResult;
            if (ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
            {
                ctrs.DialogResult = false;
                return;
            }
        }

    }
}

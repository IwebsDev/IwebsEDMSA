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
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Resources.Accueil ;
namespace Galatee.Silverlight.Accueil
{
    public partial class UcDemandeSaisieCompteurMt : ChildWindow
    {
        List<CsTcompteur> LstTypeCompteur;
        List<CsDiacomp> LstDiametre;
         List<CsMarqueCompteur> LstMarque;
         List<CsCadran> LstCadran;
        CsCanalisation     CanalisationClientRecherche = new  CsCanalisation ();
        CsEvenement LsDernierEvenement = new CsEvenement();
        public  CsDemande LaDemande = new CsDemande();
        CsEvenement LeEvt = new CsEvenement();
        public bool  GetOK { get; set; }
        CsTcompteur LeCompteurSelect = new CsTcompteur();
        public UcDemandeSaisieCompteurMt(CsDemande _LaDemande, CsTcompteur _LeCompteurSelect)
        {
            InitializeComponent();
            ChargerCadran();
            ChargerMarque();
            LeCompteurSelect = _LeCompteurSelect;
            LaDemande = _LaDemande;
        }


        void InitialiseCtrl()
        {
            this.Txt_Client.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
            this.Txt_Addresse.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
            this.Txt_Ordre.Text = string.IsNullOrEmpty(LaDemande.LaDemande.ORDRE) ? string.Empty : LaDemande.LaDemande.ORDRE ;
            this.Txt_Point.Text = LeCompteurSelect.POINT .ToString();
            this.Txt_Libellecompteur.Text = LeCompteurSelect.LIBELLE;
            this.Txt_NumCompteur.Text = string.IsNullOrEmpty(LeCompteurSelect.COMPTEUR) ? string.Empty : LeCompteurSelect.COMPTEUR;
            this.Txt_CadranCompteur.Text = string.IsNullOrEmpty(LeCompteurSelect.CADCOMP) ? string.Empty : LeCompteurSelect.CADCOMP;
            this.Txt_MarqueCompteur.Text = string.IsNullOrEmpty(LeCompteurSelect.MCOMPT) ? string.Empty : LeCompteurSelect.MCOMPT;
        }

        public CsTcompteur  EnregisterCompteur()
        {
            try
            {
                CsCanalisation _LeCompteur = new CsCanalisation();
                _LeCompteur.CENTRE = LaDemande.LaDemande.CENTRE;
                _LeCompteur.CLIENT = LaDemande.LaDemande.CLIENT;
                _LeCompteur.PRODUIT = LaDemande.LaDemande.PRODUIT;
                _LeCompteur.NUMDEM = LaDemande.LaDemande.NUMDEM;
                _LeCompteur.POINT = int.Parse(LeCompteurSelect.POINT.ToString());
                _LeCompteur.TYPECOMPTEUR  = LeCompteurSelect.CODE  ;
                _LeCompteur.TYPECOMPTAGE = LaDemande.Branchement.TYPECOMPTAGE ;
                _LeCompteur.NUMERO    = string.IsNullOrEmpty(this.Txt_NumCompteur.Text) ? string.Empty : this.Txt_NumCompteur.Text;
                _LeCompteur.ANNEEFAB = string.IsNullOrEmpty(this.Txt_AnneeFab.Text) ? string.Empty : this.Txt_AnneeFab.Text;
                _LeCompteur.MARQUE = string.IsNullOrEmpty(this.Txt_MarqueCompteur.Text) ? string.Empty : this.Txt_MarqueCompteur.Text;
                _LeCompteur.CADRAN  =System.Convert.ToByte(Txt_CadranCompteur.Text);
                _LeCompteur.USERCREATION = UserConnecte.matricule;
                _LeCompteur.USERMODIFICATION = UserConnecte.matricule;
                _LeCompteur.DATECREATION =  System.DateTime.Now;
                _LeCompteur.DATEMODIFICATION =  System.DateTime.Now;

                CsEvenement _LeEvt = new CsEvenement();
                int? _AncIndex = _LeEvt.INDEXEVT;
                _LeEvt.NUMDEM = LaDemande.LaDemande.NUMDEM;
                _LeEvt.CENTRE = LaDemande.LaDemande.CENTRE;
                _LeEvt.CLIENT = LaDemande.LaDemande.CLIENT;
                _LeEvt.ORDRE = LaDemande.LaDemande.ORDRE;
                _LeEvt.PRODUIT = LaDemande.LaDemande.PRODUIT;
                _LeEvt.DIAMETRE = _LeCompteur.DIAMETRE;
                _LeEvt.TYPECOMPTEUR = _LeCompteur.TYPECOMPTEUR ;
                _LeCompteur.TYPECOMPTAGE = LaDemande.Branchement.TYPECOMPTAGE;
                _LeEvt.COMPTEUR = _LeCompteur.NUMERO;
                _LeEvt.POINT = _LeCompteur.POINT;
                _LeEvt.IDCANALISATION = _LeCompteur.PK_ID;
                _LeEvt.MATRICULE = LaDemande.LaDemande.MATRICULE;
                _LeEvt.INDEXEVT = null;
                if (!string.IsNullOrEmpty(this.Txt_Index.Text))
                    _LeEvt.INDEXEVT = int.Parse(this.Txt_Index.Text);

                if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur ||
                    LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FermetureBrt ||
                    LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DeposeCompteur)
                    _LeEvt.CAS = SessionObject.Enumere.CasDeposeCompteur;
                else
                    _LeEvt.CAS = SessionObject.Enumere.CasPoseCompteur;

                _LeEvt.CODEEVT = SessionObject.Enumere.EvenementCodeNormale;
                _LeEvt.STATUS = SessionObject.Enumere.EvenementCree;

                _LeEvt.USERCREATION = UserConnecte.matricule;
                _LeEvt.USERMODIFICATION = UserConnecte.matricule;
                _LeEvt.DATECREATION = System.DateTime.Now;
                _LeEvt.DATEMODIFICATION = System.DateTime.Now;


                if (LaDemande.LstCanalistion != null && LaDemande.LstCanalistion.Count != 0)
                {
                    CsCanalisation _LaCan = LaDemande.LstCanalistion.FirstOrDefault(p => p.POINT == _LeCompteur.POINT);
                    if (_LaCan != null)
                        LaDemande.LstCanalistion.Remove(_LaCan);
                    LaDemande.LstCanalistion.Add(_LeCompteur);
                }
                else
                    LaDemande.LstCanalistion.Add(_LeCompteur);

                if (LaDemande.LstEvenement != null && LaDemande.LstEvenement.Count != 0)
                {
                    CsEvenement _LeEvtR = LaDemande.LstEvenement.FirstOrDefault(p => p.POINT == _LeCompteur.POINT);
                    if (_LeEvtR != null)
                        LaDemande.LstEvenement.Remove(_LeEvtR);
                    LaDemande.LstEvenement.Add(_LeEvt);
                }
                else
                    LaDemande.LstEvenement.Add(_LeEvt);


                CsTcompteur leCompteur = new CsTcompteur() 
                { 
                      POINT =int.Parse( LeCompteurSelect.POINT.ToString()) ,
                      COMPTEUR = string.IsNullOrEmpty(this.Txt_NumCompteur.Text) ? string.Empty : this.Txt_NumCompteur.Text,
                      ANNEEFAB = string.IsNullOrEmpty(this.Txt_AnneeFab.Text) ? string.Empty : this.Txt_AnneeFab.Text,
                      MCOMPT = string.IsNullOrEmpty(this.Txt_MarqueCompteur.Text) ? string.Empty : this.Txt_MarqueCompteur.Text,
                      CADCOMP = string.IsNullOrEmpty(this.Txt_CadranCompteur.Text) ? string.Empty : this.Txt_CadranCompteur.Text
                };
                return leCompteur;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        void translate()
        {
            this.lbl_Adresse.Content = Langue.lbl_adresse;
            this.lbl_client.Content = Langue.lbl_client;
            this.lbl_Ordre.Content = Langue.lbl_Ordre;
            this.lbl_Point.Content = Langue.lbl_Point;
            this.lbl_Compteur.Content = Langue.lbl_Compteur;
            this.lbl_cadran.Content = Langue.lbl_cadran;
            this.lbl_Marque.Content = Langue.lbl_Marque;
            this.lbl_TypeCompteur.Content = Langue.lbl_TypeCompteur;
            this.lbl_AnneFabrication.Content = Langue.lbl_AnneFabrication;
            this.lbl_Index.Content = Langue.lbl_Index;

        }

        void ChargerCadran()
        {
            try
            {
                if (SessionObject.LstCadran.Count != 0)
                {
                    LstCadran = SessionObject.LstCadran;
                    if (LstCadran != null && LstCadran.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_CadranCompteur.Text) &&
                            (string.IsNullOrEmpty(this.Txt_LibelleCadran.Text)))
                        {
                            CsCadran _LeCadran = ClasseMEthodeGenerique.RetourneObjectFromList(LstCadran, this.Txt_CadranCompteur.Text, "CODE");
                            if (_LeCadran != null && !string.IsNullOrEmpty(_LeCadran.LIBELLE))
                                this.Txt_LibelleCadran.Text = _LeCadran.LIBELLE;
                        }
                    }
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneToutCadranCompleted += (s, args) =>
                {
                    LstCadran = new List<CsCadran>();
                    if (args != null && args.Cancelled)
                        return;
                    LstCadran = args.Result;
                    if (LstCadran != null && LstCadran.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_CadranCompteur .Text) &&
                            (string.IsNullOrEmpty(this.Txt_LibelleCadran .Text)))
                        {
                            CsCadran _LeCadran = ClasseMEthodeGenerique.RetourneObjectFromList(LstCadran, this.Txt_CadranCompteur.Text, "PK_CADRAN");
                            if (string.IsNullOrEmpty(_LeCadran.LIBELLE))
                                this.Txt_CadranCompteur.Text = _LeCadran.LIBELLE;
                        }
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
                    if (LstMarque != null && LstMarque.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_MarqueCompteur .Text) &&
                            (!string.IsNullOrEmpty(this.Txt_LibelleMarque.Text)))
                        {
                            CsMarqueCompteur _LaMarque = ClasseMEthodeGenerique.RetourneObjectFromList(LstMarque, this.Txt_MarqueCompteur.Text, "PK_MARQUECOMPTEUR");
                            if (string.IsNullOrEmpty(_LaMarque.LIBELLE))
                                this.Txt_MarqueCompteur.Text = _LaMarque.LIBELLE;
                        }
                    }
                };
                service.RetourneToutMarqueAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UcDemandeSaisieCompteurMt()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitialiseCtrl();
        }

        private void btn_Marque_Click(object sender, RoutedEventArgs e)
        {
            this.btn_Marque.IsEnabled = false;

            if (LstMarque.Count != 0)
            {
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstMarque);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_Marque);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_Marque(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsMarqueCompteur _LaMarque = (CsMarqueCompteur)ctrs.MyObject;
                    this.Txt_MarqueCompteur .Text = _LaMarque.CODE ;
                    this.btn_Marque.IsEnabled = true;
                }
                else
                    this.btn_Marque.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }
        private void Txt_MarqueCompteur_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_MarqueCompteur.Text.Length == SessionObject.Enumere.TailleCodeMarqueCompteur && (LstMarque != null && LstMarque.Count != 0))
                {
                    CsMarqueCompteur _LaMarque = ClasseMEthodeGenerique.RetourneObjectFromList(LstMarque, this.Txt_MarqueCompteur.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LaMarque.LIBELLE))
                    {
                        this.Txt_LibelleMarque.Text = _LaMarque.LIBELLE;
                        //EnregistrerInfoSaisie(LaDemande);
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_MarqueCompteur.Focus();
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_Digit_Click(object sender, RoutedEventArgs e)
        {

            this.btn_Digit.IsEnabled = false;
            if (LstCadran != null && LstCadran.Count != 0)
            {
                this.btn_Digit.IsEnabled = false;
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCadran);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedbtnCadran);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtnCadran(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsCadran _LeCadran = (CsCadran)ctrs.MyObject;
                    this.Txt_CadranCompteur .Text = _LeCadran.CODE ;
                }
                this.btn_Digit .IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void Txt_CadranCompteur_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CadranCompteur.Text.Length == SessionObject.Enumere.TailleDigitCompteur && LstCadran != null && LstCadran.Count != 0)
                {
                    CsCadran _LeCadran = ClasseMEthodeGenerique.RetourneObjectFromList(LstCadran, this.Txt_CadranCompteur.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeCadran.LIBELLE))
                    {
                        this.Txt_LibelleCadran.Text = _LeCadran.LIBELLE;
                        //EnregistrerInfoSaisie(LaDemande);
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CadranCompteur.Focus();
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


      
    }
}


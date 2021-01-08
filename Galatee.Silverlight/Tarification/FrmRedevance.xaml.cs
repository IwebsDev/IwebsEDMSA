using Galatee.Silverlight.MainView;
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.ServiceTarification;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Tarification.Helper;
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

namespace Galatee.Silverlight.Tarification
{
    public partial class FrmRedevance : ChildWindow
    {

        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();

        protected virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion

        #region Variables
            List<CsTypeRedevence> LstDeTypeRedevence = new List<CsTypeRedevence>();
            List<CsTypeLienRedevence> LstDeTypeLienRedevence = new List<CsTypeLienRedevence>();
            List<CsTypeLienProduit> LstDeTypeLienProduit = new List<CsTypeLienProduit>();
            List<Galatee.Silverlight.ServiceAccueil.CsProduit> LstDeProduit = new List<Galatee.Silverlight.ServiceAccueil.CsProduit>();
            List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<Galatee.Silverlight.ServiceAccueil.CsCentre>();
            private bool p1;
            private bool p2;
            private bool p;
            private ServiceTarification.CsRedevance csRedevance;
            private object p3;
            private bool p4;
        #endregion

        #region Constructeurs

            public FrmRedevance()
        {
            InitializeComponent();

            this.csRedevance = new ServiceTarification.CsRedevance();
            this.csRedevance.TRANCHEREDEVANCE = new List<ServiceTarification.CsTrancheRedevence>();
            this.txt_code.MaxLength = 2;
            LayoutRoot.DataContext = this.csRedevance;
            ChargerListeDeProduit();
            ChargerDonneeDuSite();
            LoadAllTypeRedevance();
            LoadAllTypeLienRedevance();
            LoadAllTypeLienProduit();
        }

            public FrmRedevance(ServiceTarification.CsRedevance csRedevance)
            {
                InitializeComponent();

                ChargerListeDeProduit();
                ChargerDonneeDuSite();
                LoadAllTypeRedevance();
                LoadAllTypeLienRedevance();
                LoadAllTypeLienProduit();
                // TODO: Complete member initialization
                this.csRedevance = csRedevance;

                //DataBinding de la redevence au context de la fenetre
                LayoutRoot.DataContext = this.csRedevance;

                //Mise de la fenetre en lecture 
                InitializeScreenConsultation();

            }

            public FrmRedevance(ServiceTarification.CsRedevance csRedevance, bool p)
            {
                InitializeComponent();


                ChargerListeDeProduit();
                ChargerDonneeDuSite();
                LoadAllTypeRedevance();
                LoadAllTypeLienRedevance();
                // TODO: Complete member initialization
                this.csRedevance = csRedevance;
                this.p = p;

                //DataBinding de la redevence au context de la fenetre
                LayoutRoot.DataContext = this.csRedevance;

                //Mise de la fenetre en lecture 
                InitializeScreenModification();

            }

        #endregion

        #region Methodes d'interface
            private void InitializeScreenConsultation()
        {
            txt_code.IsReadOnly = true;
            //Txt_CodeCentre.IsReadOnly = true;
            Txt_CodeProduit.IsReadOnly = true;
            txt_libelle.IsReadOnly = true;
            txt_tranche.IsReadOnly = true;

            chk_gratuit.IsEnabled = false;
            btn_ajouter.IsEnabled = false;
            //btn_Centre.IsEnabled = false;
            btn_Produit.IsEnabled = false;
            btn_editer.IsEnabled = false;
            btn_supprimer.IsEnabled = false;
            OKButton.IsEnabled = false;

            InitCentreProduit(); 

        }

            private void InitCentreProduit()
            {
                Galatee.Silverlight.ServiceAccueil.CsProduit _Leproduit = LstDeProduit.FirstOrDefault(p => p.PK_ID == this.csRedevance.FK_IDPRODUIT);
                if (_Leproduit != null)
                {
                    this.Txt_CodeProduit.Text = _Leproduit.CODE;
                    this.Txt_LibelleProduit.Text = _Leproduit.LIBELLE;
                    this.Txt_LibelleProduit.Text = _Leproduit.LIBELLE;
                    Txt_LibelleProduit.Tag = _Leproduit;
                    //this.btn_Centre.Tag = _Leproduit;
                }

                Galatee.Silverlight.ServiceAccueil.CsCentre _Lecentre = LstCentre.FirstOrDefault(p => p.PK_ID == this.csRedevance.FK_IDCENTRE);
               
            }

            private void InitializeScreenModification()
            {
                txt_code.IsReadOnly = true;

                InitCentreProduit();
            }

        #endregion

        #region Events Handlers

            private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.csRedevance.CODE) && !string.IsNullOrWhiteSpace(this.csRedevance.FK_IDPRODUIT.ToString()))
            {
                this.csRedevance.PRODUIT   = this.Txt_CodeProduit.Text ;
                this.csRedevance.FK_IDPRODUIT = Txt_LibelleProduit.Tag != null ? ((Galatee.Silverlight.ServiceAccueil.CsProduit)Txt_LibelleProduit.Tag).PK_ID : 0;
                this.csRedevance.FK_IDTYPELIENREDEVANCE = Txt_LibelleTypeLienRed.Tag != null ? ((CsTypeLienRedevence)Txt_LibelleTypeLienRed.Tag).PK_ID :LstDeTypeLienRedevence.FirstOrDefault(t=>t.CODE == SessionObject.Enumere.NOLIEN).PK_ID  ;
                MyEventArg.Bag = this.csRedevance;
                OnEvent(MyEventArg);


            }
            else
            {
                Message.ShowInformation("Veuillez vous assurer que tous les champs obligatoire sont renseignés", "Information");
            }
            this.DialogResult = true;
        }
            private void CancelButton_Click(object sender, RoutedEventArgs e)
            {
                this.DialogResult = false;
            }

            private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
            {
                this.csRedevance.CODE = txt_code.Text;
                if (this.p==false)
                {
                    if (txt_code.Text.Length <= 2)
                    {
                        CheickCodeRedevanceExist();
                    }
                    else
                    {
                        Message.ShowInformation("Veuillez vous assurer que le code est sur deux position", "Information");
                    }
                }
            }

            private void Txt_LibelleTypeRed_TextChanged_1(object sender, TextChangedEventArgs e)
            {

            }
            private void btn_TypeRed_Click(object sender, RoutedEventArgs e)
            {

                try
                {
                    if (LstDeTypeRedevence != null && LstDeTypeRedevence.Count != 0)
                    {
                        List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstDeTypeRedevence);
                        UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                        ctr.Closed += new EventHandler(galatee_OkClickedTypeRed);
                        ctr.Show();
                    }

                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            void galatee_OkClickedTypeRed(object sender, EventArgs e)
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsTypeRedevence _TypeRedevence = (CsTypeRedevence)ctrs.MyObject;
                    this.Txt_CodeTypeRed.Text = _TypeRedevence.CODE;
                    this.Txt_LibelleTypeRed.Text = _TypeRedevence.LIBELLE;
                    this.Txt_LibelleTypeRed.Tag = _TypeRedevence;
                }
            }
            private void Txt_CodeTypeRed_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Txt_CodeTypeRed.Text) && this.Txt_CodeTypeRed.Text.Length == 1)
                    {
                        CsTypeRedevence _TypeRedevence = ClasseMEthodeGenerique.RetourneObjectFromList(LstDeTypeRedevence, this.Txt_CodeTypeRed.Text, "CODE");
                        if (!string.IsNullOrEmpty(_TypeRedevence.CODE))
                        {
                            this.csRedevance.FK_IDTYPEREDEVANCE= _TypeRedevence.PK_ID;
                            this.Txt_LibelleTypeRed.Text = _TypeRedevence.LIBELLE;
                            this.Txt_LibelleTypeRed.Tag = _TypeRedevence;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                }
            }

            private void btn_TypeLienRed_Click(object sender, RoutedEventArgs e)
            {

                try
                {
                    if (LstDeTypeLienRedevence != null && LstDeTypeLienRedevence.Count != 0)
                    {
                        var typelienValideProduit=LstDeTypeLienProduit.Where(t=>t.FK_IDPRODUIT==((CsProduit)Txt_LibelleProduit.Tag).PK_ID);

                        var ListeTypeLienRedevenceAAfficher=LstDeTypeLienRedevence.Where(t=>typelienValideProduit.Select(l=>l.FK_IDTYPELIEN).Contains(t.PK_ID)).Distinct();
                        if (ListeTypeLienRedevenceAAfficher!=null)
	                    {
                            if (ListeTypeLienRedevenceAAfficher.ToList().Count>0)
                            {
                                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(ListeTypeLienRedevenceAAfficher.ToList());
                                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                                ctr.Closed += new EventHandler(galatee_OkClickedTypeLienRed);
                                ctr.Show();
                            }
                            else
                            {
                                Message.ShowError("Le produit selectionné ne corresponà aucun Type de lien de redevance", "Information");
                            }

	                    }
                    }

                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            void galatee_OkClickedTypeLienRed(object sender, EventArgs e)
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsTypeLienRedevence _TypeLienRedevence = (CsTypeLienRedevence)ctrs.MyObject;
                    this.Txt_CodeTypeLienRed.Text = _TypeLienRedevence.CODE;
                    this.Txt_LibelleTypeLienRed.Text = _TypeLienRedevence.LIBELLE;
                    this.Txt_LibelleTypeLienRed.Tag = _TypeLienRedevence;
                }
            }
            private void Txt_CodeTypeLienRed_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Txt_CodeTypeLienRed.Text) && this.Txt_CodeTypeLienRed.Text.Length == 1)
                    {
                        CsTypeLienRedevence _TypeLienRedevence = ClasseMEthodeGenerique.RetourneObjectFromList(LstDeTypeLienRedevence, this.Txt_CodeTypeLienRed.Text, "CODE");
                        if (!string.IsNullOrEmpty(_TypeLienRedevence.CODE))
                        {
                            this.csRedevance.FK_IDTYPELIENREDEVANCE = _TypeLienRedevence.PK_ID;
                            this.Txt_LibelleTypeLienRed.Text = _TypeLienRedevence.LIBELLE;
                            this.Txt_LibelleTypeLienRed.Tag = _TypeLienRedevence;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                }
            }


            private void Txt_LibelleProduit_TextChanged_1(object sender, TextChangedEventArgs e)
            {

            }

            private void btn_Produit_Click(object sender, RoutedEventArgs e)
            {

                try
                {
                    if (LstDeProduit != null && LstDeProduit.Count != 0)
                    {
                        List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstDeProduit);
                        UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                        ctr.Closed += new EventHandler(galatee_OkClickedProduit);
                        ctr.Show();
                    }

                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            void galatee_OkClickedProduit(object sender, EventArgs e)
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsProduit _Leproduit = (Galatee.Silverlight.ServiceAccueil.CsProduit)ctrs.MyObject;
                    this.Txt_CodeProduit.Text = _Leproduit.CODE;
                    this.Txt_LibelleProduit.Text = _Leproduit.LIBELLE;
                    this.Txt_LibelleProduit.Tag = _Leproduit;
                }
            }
            private void Txt_CodeProduit_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Txt_CodeProduit.Text) && this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
                    {
                        Galatee.Silverlight.ServiceAccueil.CsProduit _LeProduit = ClasseMEthodeGenerique.RetourneObjectFromList(LstDeProduit, this.Txt_CodeProduit.Text, "CODE");
                        if (!string.IsNullOrEmpty(_LeProduit.CODE))
                        {
                            this.csRedevance.FK_IDPRODUIT = _LeProduit.PK_ID;
                            this.Txt_LibelleProduit.Text = _LeProduit.LIBELLE;
                            this.Txt_LibelleProduit.Tag = _LeProduit;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                }
            }

            private void btn_ajouter_Click_1(object sender, RoutedEventArgs e)
            {
                if (!string.IsNullOrWhiteSpace(txt_tranche.Text))
                {
                    byte ordre =byte.Parse((this.csRedevance.TRANCHEREDEVANCE.Count() + 1).ToString());
                    this.csRedevance.TRANCHEREDEVANCE.Add(new ServiceTarification.CsTrancheRedevence { FK_IDREDEVANCE = this.csRedevance.PK_ID, LIBELLE = txt_tranche.Text, GRATUIT = chk_gratuit.IsChecked.Value, ORDRE = ordre });


                    LayoutRoot.DataContext = null;
                    LayoutRoot.DataContext = this.csRedevance;
                    InitCentreProduit();
                }
            }

            private void txt_tranche_TextChanged(object sender, TextChangedEventArgs e)
            {

            }

            private void btn_editer_Click_1(object sender, RoutedEventArgs e)
            {

                if (dgListeRedevence.SelectedItem != null)
                {
                    if (btn_editer.Content.ToString().Trim() == "Editer".Trim())
                    {
                        txt_tranche.Text = ((ServiceTarification.CsTrancheRedevence)dgListeRedevence.SelectedItem).LIBELLE;
                        txt_tranche.Tag = ((ServiceTarification.CsTrancheRedevence)dgListeRedevence.SelectedItem);
                        chk_gratuit.IsChecked = ((ServiceTarification.CsTrancheRedevence)dgListeRedevence.SelectedItem).GRATUIT;
                        btn_editer.Content = "Mise a Jour";
                    }
                    else
                    {
                        ServiceTarification.CsTrancheRedevence tranche = ((ServiceTarification.CsTrancheRedevence)txt_tranche.Tag);
                        if (tranche != null)
                        {
                            int index = this.csRedevance.TRANCHEREDEVANCE.IndexOf(tranche);

                            tranche.LIBELLE = txt_tranche.Text;
                            tranche.GRATUIT = chk_gratuit.IsChecked.Value;
                            this.csRedevance.TRANCHEREDEVANCE[index] = tranche;

                            btn_editer.Content = "Editer".Trim();
                            txt_tranche.Tag = null;
                            LayoutRoot.DataContext = null;
                            LayoutRoot.DataContext = this.csRedevance;
                            InitCentreProduit();
                        }

                    }


                }
            }

            private void btn_supprimer_Click_1(object sender, RoutedEventArgs e)
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        ServiceTarification.CsTrancheRedevence tranche = ((ServiceTarification.CsTrancheRedevence)dgListeRedevence.SelectedItem);
                        this.csRedevance.TRANCHEREDEVANCE.Remove(tranche);

                        LayoutRoot.DataContext = null;
                        LayoutRoot.DataContext = this.csRedevance;
                        InitCentreProduit();
                    }
                    else
                    {
                        return;
                    }
                };
                messageBox.Show();


            }

            private void txt_libelle_TextChanged(object sender, TextChangedEventArgs e)
            {
                this.csRedevance.LIBELLE = txt_libelle.Text;
            }

        #endregion

        #region Servies

            private void LoadAllTypeRedevance()
        {
            try
            {
                //if (SessionObject.ListeDesProduit.Count != 0)
                //{
                //    LstDeProduit = SessionObject.ListeDesProduit;
                //    return;
                //}
                Galatee.Silverlight.ServiceTarification.TarificationServiceClient service1 = new Galatee.Silverlight.ServiceTarification.TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                service1.LoadAllTypeRedevanceCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    //SessionObject.ListeDesProduit = res.Result;
                    LstDeTypeRedevence = res.Result;
                    btn_TypeRed.IsEnabled = true;

                };
                service1.LoadAllTypeRedevanceAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerListeDeProduit");
            }
        }
      
            private void LoadAllTypeLienRedevance()
            {
                try
                {
                    //if (SessionObject.ListeDesProduit.Count != 0)
                    //{
                    //    LstDeProduit = SessionObject.ListeDesProduit;
                    //    return;
                    //}
                    Galatee.Silverlight.ServiceTarification.TarificationServiceClient service1 = new Galatee.Silverlight.ServiceTarification.TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                    service1.LoadAllTypeLienRedevanceCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        //SessionObject.ListeDesProduit = res.Result;
                        LstDeTypeLienRedevence = res.Result;
                        btn_TypeLienRed.IsEnabled = true;

                    };
                    service1.LoadAllTypeLienRedevanceAsync();
                    service1.CloseAsync();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "ChargerListeDeTypeDeLienDeRedevance");
                }
            }
            private void LoadAllTypeLienProduit()
            {
                try
                {
                    //if (SessionObject.ListeDesProduit.Count != 0)
                    //{
                    //    LstDeProduit = SessionObject.ListeDesProduit;
                    //    return;
                    //}
                    Galatee.Silverlight.ServiceTarification.TarificationServiceClient service1 = new Galatee.Silverlight.ServiceTarification.TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                    service1.LoadAllTypeLienProduitCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        //SessionObject.ListeDesProduit = res.Result;
                        LstDeTypeLienProduit= res.Result;
                        btn_TypeLienRed.IsEnabled = true;

                    };
                    service1.LoadAllTypeLienProduitAsync();
                    service1.CloseAsync();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "ChargerListeDeTypeDeLienDeRedevance");
                }
            }

            private void ChargerListeDeProduit()
            {
                try
                {
                    if (SessionObject.ListeDesProduit.Count != 0)
                    {
                        LstDeProduit = SessionObject.ListeDesProduit;
                        return;
                    }
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service1.ListeDesProduitCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        SessionObject.ListeDesProduit = res.Result;
                        LstDeProduit = SessionObject.ListeDesProduit;

                    };
                    service1.ListeDesProduitAsync();
                    service1.CloseAsync();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "ChargerListeDeProduit");
                }
            }

            private void ChargerDonneeDuSite()
            {
                try
                {
                    if (SessionObject.LstCentre.Count != 0)
                    {
                        LstCentre = SessionObject.LstCentre;
                        //lstSite = new ClasseMEthodeGenerique().RetourneSiteByCentre(LstCentre);
                        //if (lstSite != null)
                        //{
                        //    List<CsSite> _LstSite = lstSite.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        //    if (_LstSite.Count == 1)
                        //    {
                        //        this.Txt_CodeSite.Text = _LstSite[0].CODESITE;
                        //        this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                        //        this.btn_Site.IsEnabled = false;
                        //        this.Txt_CodeSite.IsReadOnly = true;
                        //    }
                        //}
                        return;
                    }
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        LstCentre = SessionObject.LstCentre;
                        //lstSite = new ClasseMEthodeGenerique().RetourneSiteByCentre(LstCentre);
                        //if (lstSite != null)
                        //{
                        //    List<CsSite> _LstSite = lstSite.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        //    if (_LstSite.Count == 1)
                        //    {
                        //        this.Txt_CodeSite.Text = _LstSite[0].CODESITE;
                        //        this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                        //        this.btn_Site.IsEnabled = false;
                        //        this.Txt_CodeSite.IsReadOnly = true;
                        //    }
                        //}
                    };
                    service.ListeDesDonneesDesSiteAsync(false);
                    service.CloseAsync();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "ChargerDonneeDuSite");

                }
            }

            private void CheickCodeRedevanceExist()
            {
                try
                {
                    //if (SessionObject.ListeDesProduit.Count != 0)
                    //{
                    //    LstDeProduit = SessionObject.ListeDesProduit;
                    //    return;
                    //}
                    Galatee.Silverlight.ServiceTarification.TarificationServiceClient service1 = new Galatee.Silverlight.ServiceTarification.TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                    service1.CheickCodeRedevanceExistCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        //SessionObject.ListeDesProduit = res.Result;
                        //LstDeTypeRedevence = res.Result;
                        if (res.Result==false)
                        {
                            Message.ShowError("Le code de redevence exite deja,Veuillez le modifier", "Avertissement");
                            txt_code.Text = string.Empty;
                            txt_code.Focus();
                        }

                    };
                    service1.CheickCodeRedevanceExistAsync(txt_code.Text.Trim());
                    service1.CloseAsync();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "ChargerListeDeProduit");
                }
            }

        #endregion


    }
}


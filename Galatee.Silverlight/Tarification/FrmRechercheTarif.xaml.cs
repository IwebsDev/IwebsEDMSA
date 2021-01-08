using Galatee.Silverlight.MainView;
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
    public partial class FrmRechercheTarif : ChildWindow
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


        List<Galatee.Silverlight.ServiceTarification.CsContenantCritereTarif> LstDeContenantCritereTarif = new List<Galatee.Silverlight.ServiceTarification.CsContenantCritereTarif>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<Galatee.Silverlight.ServiceAccueil.CsCentre>();
        private bool p1;
        private bool p2;
        private bool p;
        private ServiceTarification.CsRechercheTarif csRechercheTarif;
        private object p3;
        private bool p4;

        public FrmRechercheTarif()
        {
            InitializeComponent();

            this.csRechercheTarif = new ServiceTarification.CsRechercheTarif();
            this.csRechercheTarif.CTARCOMP = new List<ServiceTarification.CsCtarcomp>();

            LayoutRoot.DataContext = this.csRechercheTarif;
            LoadAllContenantCritereTarif();
            //ChargerDonneeDuSite();
        }

        

        //public FrmRechercheTarif(bool p1, bool p2)
        //{
        //    InitializeComponent();

        //    ChargerListeDeProduit();
        //    ChargerDonneeDuSite();

        //    // TODO: Complete member initialization
        //    this.p1 = p1;
        //    this.p2 = p2;
        //}

        //public FrmRechercheTarif(bool p)
        //{
        //    InitializeComponent();

        //    ChargerListeDeProduit();
        //    ChargerDonneeDuSite();

        //    // TODO: Complete member initialization
        //    this.p = p;
        //}

        public FrmRechercheTarif(ServiceTarification.CsRechercheTarif csRechercheTarif)
        {
            InitializeComponent();

            LoadAllContenantCritereTarif();
            //ChargerDonneeDuSite();

            // TODO: Complete member initialization
            this.csRechercheTarif = csRechercheTarif;

            //DataBinding de la redevence au context de la fenetre
            LayoutRoot.DataContext = this.csRechercheTarif;

            //Mise de la fenetre en lecture 
            InitializeScreenConsultation();

        }

        private void InitializeScreenConsultation()
        {
            txt_code.IsReadOnly = true;
            //Txt_CodeCentre.IsReadOnly = true;
            //Txt_CodeProduit.IsReadOnly = true;
            txt_libelle.IsReadOnly = true;
            CboTable.IsEnabled = false;

            btn_ajouter.IsEnabled = false;
            //btn_Centre.IsEnabled = false;
            //btn_Produit.IsEnabled = false;
            //btn_editer.IsEnabled = false;
            btn_supprimer.IsEnabled = false;
            OKButton.IsEnabled = false;

            InitCentreProduit(); 

        }

        private void InitCentreProduit()
        {
            //Galatee.Silverlight.ServiceAccueil.CsProduit _Leproduit = LstDeProduit.FirstOrDefault(p => p.PK_ID == this.csRechercheTarif.CODE);
            //if (_Leproduit != null)
            //{
            //    //this.Txt_CodeProduit.Text = _Leproduit.CODE;
            //    //this.Txt_LibelleProduit.Text = _Leproduit.LIBELLE;
            //    //this.Txt_LibelleProduit.Text = _Leproduit.LIBELLE;
            //    //this.btn_Centre.Tag = _Leproduit;
            //}

            //Galatee.Silverlight.ServiceAccueil.CsCentre _Lecentre = LstCentre.FirstOrDefault(p => p.PK_ID == this.csRechercheTarif.FK_IDCENTRE);

            //if (_Lecentre != null)
            //{
            //    //this.Txt_CodeCentre.Text = _Lecentre.CODECENTRE;
            //    //this.Txt_LibelleCentre.Text = _Lecentre.LIBELLE;
            //    //this.Txt_LibelleCentre.Text = _Lecentre.LIBELLE;
            //    //this.Txt_LibelleCentre.Tag = _Lecentre;
            //}
        }
    
        public FrmRechercheTarif(ServiceTarification.CsRechercheTarif csRechercheTarif, bool p)
        {
            InitializeComponent();


            LoadAllContenantCritereTarif();
            //ChargerDonneeDuSite();

            // TODO: Complete member initialization
            this.csRechercheTarif = csRechercheTarif;
            this.p = p;

            //DataBinding de la redevence au context de la fenetre
            LayoutRoot.DataContext = this.csRechercheTarif;

            //Mise de la fenetre en lecture 
            InitializeScreenModification();
            
        }

        private void InitializeScreenModification()
        {
            txt_code.IsReadOnly = true;

            InitCentreProduit();
        }

        //public FrmRechercheTarif(object p3)
        //{
        //    // TODO: Complete member initialization
        //    this.p3 = p3;
        //}

        //public FrmRechercheTarif(object p3, bool p4)
        //{
        //    // TODO: Complete member initialization
        //    this.p3 = p3;
        //    this.p4 = p4;
        //}

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.csRechercheTarif.CODE) && !string.IsNullOrWhiteSpace(this.csRechercheTarif.LIBELLE))
            {
                //this.csRechercheTarif.FK_IDCENTRE = Txt_LibelleCentre.Tag!=null?((Galatee.Silverlight.ServiceAccueil.CsCentre)Txt_LibelleCentre.Tag).PK_ID:0;
                //this.csRechercheTarif.FK_IDPRODUIT = Txt_LibelleProduit.Tag!=null?((Galatee.Silverlight.ServiceAccueil.CsProduit)Txt_LibelleProduit.Tag).PK_ID:0;
                MyEventArg.Bag = this.csRechercheTarif;
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
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_LibelleProduit_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_Produit_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (LstDeContenantCritereTarif != null && LstDeContenantCritereTarif.Count != 0)
                {
                    //List<object> _LstObject = new ClasseMEthodeGenerique().RetourneListeObjet(LstDeProduit);
                    //UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    //ctr.Closed += new EventHandler(galatee_OkClickedProduit);
                    //ctr.Show();
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
                //this.Txt_CodeProduit.Text = _Leproduit.CODE;
                //this.Txt_LibelleProduit.Text = _Leproduit.LIBELLE;
                //this.Txt_LibelleProduit.Tag = _Leproduit;
            }
        }
        private void Txt_CodeProduit_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //if (!string.IsNullOrEmpty(Txt_CodeProduit.Text) && this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
                //{
                //    Galatee.Silverlight.ServiceAccueil.CsProduit _LeProduit = new ClasseMEthodeGenerique().RetourneObjectFromList(LstDeProduit, this.Txt_CodeProduit.Text, LstDeProduit[0].CODE);
                //    if (!string.IsNullOrEmpty(_LeProduit.CODE))
                //    {
                //        this.Txt_LibelleProduit.Text = _LeProduit.LIBELLE;
                //        this.Txt_LibelleProduit.Tag = _LeProduit;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }


        private void LoadAllContenantCritereTarif()
        {
            try
            {
                //if (SessionObject.ListeDesProduit.Count != 0)
                //{
                //    LstDeProduit = SessionObject.ListeDesProduit;
                //    return;
                //}
                Galatee.Silverlight.ServiceTarification.TarificationServiceClient service1 = new Galatee.Silverlight.ServiceTarification.TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                service1.LoadAllContenantCritereTarifCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    //SessionObject.ListeDesProduit = res.Result;
                    LstDeContenantCritereTarif = res.Result;
                    CboTable.ItemsSource = LstDeContenantCritereTarif;
                    CboTable.DisplayMemberPath = "LIBELLE";
                    CboTable.SelectedValuePath = "PK_ID";

                };
                service1.LoadAllContenantCritereTarifAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerListeDeProduit");
            }
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                //List<object> _LstObject = new ClasseMEthodeGenerique().RetourneListeObjet(LstCentre.OrderBy(p => p.CODECENTRE).ToList());
                //UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODECENTRE", "LIBELLE", "Liste");
                //ctr.Closed += new EventHandler(galatee_OkClicked);
                //ctr.Show();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        void galatee_OkClicked(object sender, EventArgs e)
        {
            //UcListeDesCentres ctrs = sender as UcListeDesCentres;
            //this.Txt_CodeCentre.Text = ctrs.MySite.CODECENTRE;
            ////this.Txt_LibelleCentre.Text = ctrs.MySite.LIBELLE;
            //LstDeProduit = new ClasseMEthodeGenerique().RetourneListeDeProduitDuSite(ctrs.MySite);
            //if (LstDeProduit.Count == 1)
            //{
            //    this.Txt_CodeProduit.Text = LstDeProduit[0].CODE ;
            //    //this.Txt_LibelleProduit.Text = LstDeProduit[0].LIBELLE;
            //    this.btn_Produit.IsEnabled = false;

            //}

            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsCentre _LeCentreClient = (Galatee.Silverlight.ServiceAccueil.CsCentre)ctrs.MyObject;
                //this.Txt_CodeCentre.Text = _LeCentreClient.CODECENTRE;
                //this.Txt_LibelleCentre.Text = _LeCentreClient.LIBELLE;
                //this.Txt_LibelleCentre.Tag = _LeCentreClient;
            }
        }
        int IdCentreClient = 0;
        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ////LstCommuneSelect = new List<CsCommune>();
                //if (!string.IsNullOrEmpty(Txt_CodeCentre.Text) && Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                //{
                //    Galatee.Silverlight.ServiceAccueil.CsCentre _LeCentreClient = new ClasseMEthodeGenerique().RetourneObjectFromList(LstCentre, this.Txt_CodeCentre.Text, "CODECENTRE");
                //    if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
                //    {
                //        this.Txt_LibelleCentre.Text = _LeCentreClient.LIBELLE;
                //        this.Txt_LibelleCentre.Tag = _LeCentreClient;
                //        IdCentreClient = _LeCentreClient.PK_ID;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);

            }
        }

        //private void ChargerDonneeDuSite()
        //{
        //    try
        //    {
        //        if (SessionObject.LstCentre.Count != 0)
        //        {
        //            LstCentre = SessionObject.LstCentre;
        //            //lstSite = new ClasseMEthodeGenerique().RetourneSiteByCentre(LstCentre);
        //            //if (lstSite != null)
        //            //{
        //            //    List<CsSite> _LstSite = lstSite.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
        //            //    if (_LstSite.Count == 1)
        //            //    {
        //            //        this.Txt_CodeSite.Text = _LstSite[0].CODESITE;
        //            //        this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
        //            //        this.btn_Site.IsEnabled = false;
        //            //        this.Txt_CodeSite.IsReadOnly = true;
        //            //    }
        //            //}
        //            return;
        //        }
        //        Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //        service.ListeDesDonneesDesSiteCompleted += (s, args) =>
        //        {
        //            if (args != null && args.Cancelled)
        //                return;
        //            SessionObject.LstCentre = args.Result;
        //            LstCentre = SessionObject.LstCentre;
        //            //lstSite = new ClasseMEthodeGenerique().RetourneSiteByCentre(LstCentre);
        //            //if (lstSite != null)
        //            //{
        //            //    List<CsSite> _LstSite = lstSite.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
        //            //    if (_LstSite.Count == 1)
        //            //    {
        //            //        this.Txt_CodeSite.Text = _LstSite[0].CODESITE;
        //            //        this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
        //            //        this.btn_Site.IsEnabled = false;
        //            //        this.Txt_CodeSite.IsReadOnly = true;
        //            //    }
        //            //}
        //        };
        //        service.ListeDesDonneesDesSiteAsync();
        //        service.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex.Message, "ChargerDonneeDuSite");

        //    }
        //}

        private void btn_ajouter_Click_1(object sender, RoutedEventArgs e)
        {
            if (CboTable.SelectedItem!=null)
            {
                ServiceTarification.CsCtarcomp CsCtarcomp =new ServiceTarification.CsCtarcomp();
                CsCtarcomp.FK_IDRECHERCHETARIF=this.csRechercheTarif.PK_ID;
                CsCtarcomp.FK_IDCONTENANTCRITERETARIF = ((CsContenantCritereTarif)CboTable.SelectionBoxItem).PK_ID;

                this.csRechercheTarif.CTARCOMP.Add(new ServiceTarification.CsCtarcomp { FK_IDRECHERCHETARIF = this.csRechercheTarif.PK_ID, FK_IDCONTENANTCRITERETARIF = ((CsContenantCritereTarif)CboTable.SelectionBoxItem).PK_ID, ORDRE = byte.Parse((((List<CsCtarcomp>)dgListeRedevence.ItemsSource).Count() + 1).ToString()), DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule, LIBELLECONTENANTCRITERETARIF = ((CsContenantCritereTarif)CboTable.SelectionBoxItem) .LIBELLE});

                LayoutRoot.DataContext = null;
                LayoutRoot.DataContext = this.csRechercheTarif;
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
                //if (btn_editer.Content.ToString().Trim() == "Editer".Trim())
                //{
                //    txt_tranche.Text = ((ServiceFacturation.CsTrancheRedevence)dgListeRedevence.SelectedItem).LIBELLE;
                //    txt_tranche.Tag = ((ServiceFacturation.CsTrancheRedevence)dgListeRedevence.SelectedItem);
                //    btn_editer.Content = "Mis a Jour";
                //}
                //else
                //{
                //    ServiceFacturation.CsTrancheRedevence tranche = ((ServiceFacturation.CsTrancheRedevence)txt_tranche.Tag);
                //    if (tranche!=null)
                //    {
                //        //int index = this.csRechercheTarif.TrancheRedevences.IndexOf(tranche);

                //        //tranche.LIBELLE = txt_tranche.Text;
                //        //this.csRechercheTarif.TrancheRedevences[index] = tranche;

                //        //btn_editer.Content = "Editer".Trim();
                //        //txt_tranche.Tag = null;
                //        //LayoutRoot.DataContext = null;
                //        //LayoutRoot.DataContext = this.csRechercheTarif;
                //        //InitCentreProduit();
                //    }
                    
                //}


            }
        }

        private void btn_supprimer_Click_1(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    ServiceTarification.CsCtarcomp Ctarcomp = ((ServiceTarification.CsCtarcomp)dgListeRedevence.SelectedItem);
                    this.csRechercheTarif.CTARCOMP.Remove(Ctarcomp);

                    LayoutRoot.DataContext = null;
                    LayoutRoot.DataContext = this.csRechercheTarif;
                    InitCentreProduit();
                }
                else
                {
                    return;
                }
            };
            messageBox.Show();


        }


    }
}


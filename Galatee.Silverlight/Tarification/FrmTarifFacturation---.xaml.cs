using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.ServiceTarification;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Tarification.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class FrmTarifFacturation : ChildWindow
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



        #region

        private ServiceTarification.CsTarifFacturation csTarifFacturation;
        private bool p;


        #region MethodeUI

        private void InitCentre_Redev_RechTarif_ModeCalc_ModeApp()
        {
            if (csTarifFacturation!=null)
            {

                CsProduit _LeProduit = LstDeProduit.FirstOrDefault(p => p.PK_ID == this.csTarifFacturation.FK_IDPRODUIT);
                if (_LeProduit != null)
                {
                    this.Txt_CodeProduit.Text = _LeProduit.CODE;
                    this.Txt_LibelleProduit.Text = _LeProduit.LIBELLE;
                    this.Txt_LibelleProduit.Tag = _LeProduit;
                }

                CsCommune _LaCommune = LstCommune.FirstOrDefault(p => p.CODE == this.csTarifFacturation.COMMUNE);
                if (_LaCommune != null)
                {
                    this.Txt_CodeCommune.Text = _LaCommune.CODE;
                    this.Txt_LibelleCommune.Text = _LaCommune.LIBELLE;
                    this.Txt_LibelleCommune.Tag = _LaCommune;
                }

                CsCodeTaxeApplication _LeCodeTaxeApplication = LstCodeApplicationTaxe.FirstOrDefault(p => p.PK_ID == this.csTarifFacturation.FK_IDTAXE.ToString());
                if (_LeCodeTaxeApplication != null)
                {
                    this.Txt_CodeTaxe.Text = _LeCodeTaxeApplication.PK_CODEAPPLICATION;
                    this.Txt_LibelleTaxe.Text = _LeCodeTaxeApplication.LIBELLE;
                    this.Txt_LibelleTaxe.Tag = _LeCodeTaxeApplication;
                }

                CsUniteComptage _LeUniteComptage = LstUniteComptage.FirstOrDefault(p => p.CODE == this.csTarifFacturation.UNITE);
                if (_LeUniteComptage != null)
                {
                    this.Txt_CodeUniteComptage.Text = _LeUniteComptage.CODE;
                    this.Txt_LibelleUniteComptage.Text = _LeUniteComptage.LIBELLE;
                    this.Txt_LibelleUniteComptage.Tag = _LeUniteComptage;
                }

                Galatee.Silverlight.ServiceAccueil.CsCentre _Lecentre = LstCentre.FirstOrDefault(p => p.PK_ID == this.csTarifFacturation.FK_IDCENTRE);
                if (_Lecentre != null)
                {
                    this.Txt_CodeCentre.Text = _Lecentre.CODE;
                    this.Txt_LibelleCentre.Text = _Lecentre.LIBELLE;
                    this.Txt_LibelleCentre.Tag = _Lecentre;
                } 
            }
        }

        #endregion



        public FrmTarifFacturation()
        {
            InitializeComponent();
            this.csVariableDeTarification = new ServiceTarification.CsTarifFacturation();
            this.csVariableDeTarification.DETAILTARIFFACTURATION = new List<ServiceTarification.CsDetailTarifFacturation>();
            dgListeRedevence.ItemsSource = ListeDETAILTARIFFACTURATION;

                    LayoutRoot.DataContext = this.csVariableDeTarification;

                    //LoadAllTarifFacturation();
                    ChargerDonneeDuSite();
                    ChargerLesBrancheDesCommune();
                    ChargerApplicationTaxe();
                    ChargerListeDeProduit();
        }

        public FrmTarifFacturation(ServiceTarification.CsTarifFacturation csTarifFacturation)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.csTarifFacturation = csTarifFacturation;
            dgListeRedevence.ItemsSource = ListeDETAILTARIFFACTURATION;

        }

        public FrmTarifFacturation(ServiceTarification.CsTarifFacturation csTarifFacturation, bool p)
        {
            InitializeComponent();

            //LoadAllTarifFacturation();
            LoadAllRedevance();
            LoadAllUniteComptage();
            ChargerDonneeDuSite();
            ChargerLesBrancheDesCommune();
            ChargerApplicationTaxe();
            ChargerListeDeProduit();
            dgListeRedevence.ItemsSource = csTarifFacturation.DETAILTARIFFACTURATION ;

            // TODO: Complete member initialization
            this.csTarifFacturation = csTarifFacturation;
            this.p = p;

            //DataBinding de la TarifFacturation au context de la fenetre
            LayoutRoot.DataContext = this.csTarifFacturation;

            //Mise de la fenetre en lecture 
            //InitializeScreenModification();
            InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
        }

        private void LoadAllUniteComptage()
        {
            try
            {
                if (SessionObject.ListeUniteComptage.Count != 0)
                    LstUniteComptage = SessionObject.ListeUniteComptage;
                else
                {
                    TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                    service.LoadAllUniteComptageCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.ListeUniteComptage = args.Result;
                        LstUniteComptage = SessionObject.ListeUniteComptage;
                        InitCentre_Redev_RechTarif_ModeCalc_ModeApp();

                    };
                    service.LoadAllUniteComptageAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            if (!(this.csTarifFacturation.FK_IDCENTRE <= 0 && this.csTarifFacturation.FK_IDPRODUIT <= 0 && this.csTarifFacturation.FK_IDTAXE <= 0 && this.csTarifFacturation.FK_IDUNITECOMPTAGE <= 0))
            {
                this.csTarifFacturation.FK_IDCENTRE = Txt_LibelleCentre.Tag != null ? ((CsCentre)Txt_LibelleCentre.Tag).PK_ID : 0;
                this.csTarifFacturation.FK_IDPRODUIT = Txt_LibelleProduit.Tag != null ? ((CsProduit)Txt_LibelleProduit.Tag).PK_ID : 0;
                this.csTarifFacturation.FK_IDTAXE = Txt_LibelleTaxe.Tag != null ? int.Parse(((CsCodeTaxeApplication)Txt_LibelleTaxe.Tag).PK_CODEAPPLICATION) : 0;
                this.csTarifFacturation.FK_IDUNITECOMPTAGE = Txt_LibelleUniteComptage.Tag != null ? ((CsUniteComptage)Txt_LibelleUniteComptage.Tag).PK_ID : 0;
                this.csTarifFacturation.UNITE  = Txt_CodeUniteComptage .Text ;
                //this.csVariableDeTarification.FK_IDVARIABLETARIF = Txt_LibelleRedevence.Tag != null ? ((CsRedevance)Txt_LibelleRedevence.Tag).PK_ID : 0;
                MyEventArg.Bag = this.csTarifFacturation;
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

        private void dgListeRedevence_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn_ajouter_Click_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_tranche.Text))
            {
                int PK_IDREDEVENCE = 0;
                if (SessionObject.ListeRedevence.FirstOrDefault(r => r.CODE == this.csTarifFacturation.REDEVANCE)!=null)
                {
                    PK_IDREDEVENCE = SessionObject.ListeRedevence.FirstOrDefault(r => r.CODE == this.csTarifFacturation.REDEVANCE).PK_ID;
                }
                else
                {
                    Message.ShowError("Le code de redeence associer a se tarif nexite pas dans liste des redevences connue", "Avertissement");
                }
                
                decimal Prix = 0;
                int qte = 0;
                if (!decimal.TryParse(txt_Prix.Text, out Prix))
	            {
                    Message.ShowError("Veuillez Saisir une valeur numeric dans le champs prix", "Info");
                    return;
	            }
                if (!int.TryParse(txt_tranche.Text, out qte))
                {
                    Message.ShowError("Veuillez Saisir une valeur numeric dans le champs Quantite", "Info");
                    return;
                }
                int tranche = this.csTarifFacturation.DETAILTARIFFACTURATION.Count();
               if(tranche>=ListeDETAILTARIFFACTURATION.Count())
               {
                    var MonDETAILTARIFFACTURATION=this.csTarifFacturation.DETAILTARIFFACTURATION.FirstOrDefault(d=>(d.PRIXUNITAIRE==0 || d.PRIXUNITAIRE==null) && (d.QTEANNUELMAXI==0 || d.QTEANNUELMAXI==null));

                  if (MonDETAILTARIFFACTURATION!=null)
	                {
		                 var index=this.csTarifFacturation.DETAILTARIFFACTURATION.IndexOf(MonDETAILTARIFFACTURATION);

                                   this.csTarifFacturation.DETAILTARIFFACTURATION[index].PRIXUNITAIRE=Prix;
                                   this.csTarifFacturation.DETAILTARIFFACTURATION[index].QTEANNUELMAXI=qte;

                                    MonDETAILTARIFFACTURATION.PRIXUNITAIRE=Prix;
                                    MonDETAILTARIFFACTURATION.QTEANNUELMAXI = qte;
                                    MonDETAILTARIFFACTURATION.NUMEROTRANCHE = (byte)(index + 1 );
                                    MonDETAILTARIFFACTURATION.USERCREATION = UserConnecte.matricule;
                                    MonDETAILTARIFFACTURATION.DATECREATION = System.DateTime.Now;
                                    ListeDETAILTARIFFACTURATION.Add(MonDETAILTARIFFACTURATION);
                                    dgListeRedevence.ItemsSource = ListeDETAILTARIFFACTURATION;
	                }
                  else
                  {
                      Message.ShowWarning("Vous avez attein la limite de tranche à ajouter,vous pouvez tout de même faire des modification", "Information");
                  }
               }
               else
	           {
                   Message.ShowWarning("Vous avez attein la limite de tranche à ajouter,vous pouvez tout de même faire des modification","Information");
	           }
                //this.csTarifFacturation.DETAILTARIFFACTURATION.Add(new ServiceTarification.CsDetailTarifFacturation { FK_IDREDEVANCE = PK_IDREDEVENCE, PRIXUNITAIRE = Prix, QTEANNUELMAXI = qte, NUMEROTRANCHE = (byte)tranche });


                LayoutRoot.DataContext = null;
                LayoutRoot.DataContext = this.csTarifFacturation;
                InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
            }
        }

        private void btn_editer_Click_1(object sender, RoutedEventArgs e)
        {
            if (dgListeRedevence.SelectedItem != null)
            {
                if (btn_editer.Content.ToString().Trim() == "Editer".Trim())
                {
                    txt_tranche.Text = ((ServiceTarification.CsDetailTarifFacturation)dgListeRedevence.SelectedItem).QTEANNUELMAXI.ToString();
                    txt_Prix.Text = ((ServiceTarification.CsDetailTarifFacturation)dgListeRedevence.SelectedItem).PRIXUNITAIRE.ToString();
                    txt_tranche.Tag = ((ServiceTarification.CsDetailTarifFacturation)dgListeRedevence.SelectedItem);
                    btn_editer.Content = "Mis a Jour";
                }
                else
                {
                    ServiceTarification.CsDetailTarifFacturation tranche = ((ServiceTarification.CsDetailTarifFacturation)txt_tranche.Tag);
                    if (tranche != null)
                    {
                        int index = this.csTarifFacturation.DETAILTARIFFACTURATION.IndexOf(tranche);
                        int index_ = ListeDETAILTARIFFACTURATION.IndexOf(tranche);
                        decimal Prix = 0;
                        int qte = 0;
                        if (!decimal.TryParse(txt_Prix.Text, out Prix))
                        {
                            Message.ShowError("Veuillez Saisir une valeur numeric dans le champs prix", "Info");
                            return;
                        }
                        if (!int.TryParse(txt_tranche.Text, out qte))
                        {
                            Message.ShowError("Veuillez Saisir une valeur numeric dans le champs Quantite", "Info");
                            return;
                        }
                        tranche.QTEANNUELMAXI = qte;
                        tranche.PRIXUNITAIRE = Prix;
                        this.csTarifFacturation.DETAILTARIFFACTURATION[index] = tranche;
                        ListeDETAILTARIFFACTURATION[index_] = tranche;
                        dgListeRedevence.ItemsSource = ListeDETAILTARIFFACTURATION;

                        btn_editer.Content = "Editer".Trim();
                        txt_tranche.Tag = null;
                        LayoutRoot.DataContext = null;
                        LayoutRoot.DataContext = this.csTarifFacturation;
                        InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
                    }

                }


            }
        }

        private void btn_supprimer_Click_1(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Ête-vous sure de vouloire supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    ServiceTarification.CsDetailTarifFacturation tranche = ((ServiceTarification.CsDetailTarifFacturation)dgListeRedevence.SelectedItem);

                    ListeDETAILTARIFFACTURATION.Remove(tranche);
                    var index = this.csTarifFacturation.DETAILTARIFFACTURATION.IndexOf(tranche);
                    this.csTarifFacturation.DETAILTARIFFACTURATION[index].QTEANNUELMAXI = 0;
                    this.csTarifFacturation.DETAILTARIFFACTURATION[index].PRIXUNITAIRE = 0;
                    dgListeRedevence.ItemsSource = ListeDETAILTARIFFACTURATION;

                    LayoutRoot.DataContext = null;
                    LayoutRoot.DataContext = this.csTarifFacturation;
                    InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }

        private void Txt_CodeTypeRed_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_TypeRed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Txt_LibelleTypeRed_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_CodeTypeLienRed_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_TypeLienRed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Txt_LibelleProduit_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        //private void Txt_CodeUniteComptage_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        //private void btn_UniteComptage_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void Txt_LibelleUniteComptage_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        //private void Txt_CodeCommune_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        //private void btn_Commune_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void Txt_LibelleCommune_TextChanged_1(object sender, TextChangedEventArgs e)
        //{

        //}

        //private void Txt_CodeProduit_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        //private void btn_Produit_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void txt_tranche_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        #endregion


        #region

            #region Variables

            List<CsTarifFacturation> ListeTarifFacturation = new List<CsTarifFacturation>();
            List<CsModeCalcul> ListeModeCalcule = new List<CsModeCalcul>();
            List<CsModeApplicationTarif> ListeModeApplicationTarif = new List<CsModeApplicationTarif>();
            List<CsRedevance> ListeRedevence = new List<CsRedevance>();
            public static List<CsCentre> LstCentre = new List<CsCentre>();
            ObservableCollection<CsDetailTarifFacturation> ListeDETAILTARIFFACTURATION = new ObservableCollection<CsDetailTarifFacturation>();

        List<CsProduit> LstDeProduit = new List<CsProduit>();
        public static List<CsCommune> LstCommune = new List<CsCommune>();
        List<CsCodeTaxeApplication> LstCodeApplicationTaxe=new List<CsCodeTaxeApplication>();
        List<CsUniteComptage> LstUniteComptage = new List<CsUniteComptage>();




                private bool p1;
                private bool p2;
                //private bool p;
                private CsTarifFacturation csVariableDeTarification;
                private object p3;
                private bool p4;
            #endregion

            //#region Constructeurs

            //    public FrmVariableTarif()
            //    {
            //        InitializeComponent();

            //        this.csVariableDeTarification = new ServiceTarification.CsTarifFacturation();
            //        //this.csVariableDeTarification.TRANCHEREDEVANCE = new List<ServiceTarification.CsTrancheRedevence>();

            //        LayoutRoot.DataContext = this.csVariableDeTarification;

            //        //LoadAllTarifFacturation();
            //        ChargerDonneeDuSite();
            //        ChargerLesBrancheDesCommune();
            //        ChargerApplicationTaxe();
            //        ChargerListeDeProduit();

            //    }

            //    public FrmVariableTarif(CsVariableDeTarification csVariableDeTarification)
            //    {
            //        InitializeComponent();

            //         //LoadAllTarifFacturation();
            //        ChargerDonneeDuSite();
            //        ChargerLesBrancheDesCommune();
            //        ChargerApplicationTaxe();
            //        ChargerListeDeProduit();

            //        // TODO: Complete member initialization
            //        this.csVariableDeTarification = csVariableDeTarification;

            //        //DataBinding de la redevence au context de la fenetre
            //        LayoutRoot.DataContext = this.csVariableDeTarification;

            //        //Mise de la fenetre en lecture 
            //        InitializeScreenConsultation();

            //    }

            //    public FrmVariableTarif(CsVariableDeTarification csVariableDeTarification, bool p)
            //    {
            //        InitializeComponent();

            //         //LoadAllTarifFacturation();
            //        ChargerDonneeDuSite();
            //        ChargerLesBrancheDesCommune();
            //        ChargerApplicationTaxe();
            //        ChargerListeDeProduit();
            //        // TODO: Complete member initialization
            //        this.csVariableDeTarification = csVariableDeTarification;
            //        this.p = p;

            //        //DataBinding de la redevence au context de la fenetre
            //        LayoutRoot.DataContext = this.csVariableDeTarification;

            //        //Mise de la fenetre en lecture 
            //        InitializeScreenModification();

            //    }

            //#endregion

            //#region Methodes d'interface

            //    private void InitializeScreenConsultation()
            //{
            //    Txt_CodeCentre.IsReadOnly = true;
            //    Txt_CodeModeApp.IsReadOnly = true;
            //    Txt_CodeModeCalcule.IsReadOnly = true;
            //    Txt_CodeTarifFacturation.IsReadOnly = true;
            //    Txt_CodeRedevence.IsReadOnly = true;
            //    txt_commune.IsReadOnly = true;
            //    txt_DateAppli.IsEnabled = false;
            //    txt_Formule.IsReadOnly = true;
            //    Txt_LibelleCentre1.IsReadOnly = true;
            //    txt_LibelleComptable.IsReadOnly = true;
            //    Txt_LibelleModeApp.IsReadOnly = true;
            //    Txt_LibelleModeCalcule.IsReadOnly = true;
            //    txt_LibelleRed.IsReadOnly = true;
            //    Txt_LibelleRedevence.IsReadOnly = true;
            //    txt_OrdreEdition.IsReadOnly = true;
            //    txt_Region.IsReadOnly = true;
            //    txt_SRegion.IsReadOnly = true;

            //    btn_Centre1.IsEnabled = false;
            //    btn_ModeApp.IsEnabled = false;
            //    btn_ModeCalcule.IsEnabled = false;
            //    btn_TarifFacturation.IsEnabled = false;
            //    btn_Redevence.IsEnabled = false;

            //    OKButton.IsEnabled = false;

            //    InitCentre_Redev_RechTarif_ModeCalc_ModeApp(); 

            //}

            //    private void InitCentre_Redev_RechTarif_ModeCalc_ModeApp()
            //    {
            //        CsRedevance _LeRedevence = ListeRedevence.FirstOrDefault(p => p.PK_ID == this.csVariableDeTarification.FK_IDREDEVANCE);
            //        if (_LeRedevence != null)
            //        {
            //            this.Txt_CodeRedevence.Text = _LeRedevence.CODE;
            //            this.Txt_LibelleRedevence.Text = _LeRedevence.LIBELLE;
            //            this.Txt_LibelleRedevence.Tag = _LeRedevence;
            //        }

            //        CsTarifFacturation _LeTarifFacturation = ListeTarifFacturation.FirstOrDefault(p => p.PK_ID == this.csVariableDeTarification.FK_IDTarifFacturation);
            //        if (_LeTarifFacturation != null)
            //        {
            //            this.Txt_CodeTarifFacturation.Text = _LeTarifFacturation.CODE;
            //            this.Txt_LibelleTarifFacturation.Text = _LeTarifFacturation.LIBELLE;
            //            this.Txt_LibelleTarifFacturation.Tag = _LeTarifFacturation;
            //        }

            //        CsModeCalcul _LeModeCalcul = ListeModeCalcule.FirstOrDefault(p => p.PK_ID == this.csVariableDeTarification.FK_IDMODECALCUL);
            //        if (_LeModeCalcul != null)
            //        {
            //            this.Txt_CodeModeCalcule.Text = _LeModeCalcul.CODE;
            //            this.Txt_LibelleModeCalcule.Text = _LeModeCalcul.LIBELLE;
            //            this.Txt_LibelleModeCalcule.Tag = _LeModeCalcul;
            //        }

            //        CsModeApplicationTarif _LeModeApp = ListeModeApplicationTarif.FirstOrDefault(p => p.PK_ID == this.csVariableDeTarification.FK_IDMODEAPPLICATION);
            //        if (_LeModeApp != null)
            //        {
            //            this.Txt_CodeModeApp.Text = _LeModeApp.CODE;
            //            this.Txt_LibelleModeApp.Text = _LeModeApp.LIBELLE;
            //            this.Txt_LibelleModeApp.Tag = _LeModeApp;
            //        }

            //        Galatee.Silverlight.ServiceAccueil.CsCentre _Lecentre = LstCentre.FirstOrDefault(p => p.PK_ID == this.csVariableDeTarification.FK_IDCENTRE);
            //        if (_Lecentre != null)
            //        {
            //            this.Txt_CodeCentre.Text = _Lecentre.CODE;
            //            this.Txt_LibelleCentre1.Text = _Lecentre.LIBELLE;
            //            this.Txt_LibelleCentre1.Tag = _Lecentre;
            //        }
            //    }

            //    private void InitializeScreenModification()
            //    {
            //        //txt_code.IsReadOnly = true;
            //        InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
            //    }

                //#endregion

                #region Events Handlers

                private void btn_Centre_Click(object sender, RoutedEventArgs e)
                {

                    try
                    {

                        List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCentre.OrderBy(p => p.CODE).ToList());
                        UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                        ctr.Closed += new EventHandler(galatee_OkClicked);
                        ctr.Show();
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                    }
                }
                void galatee_OkClicked(object sender, EventArgs e)
                {
                    UcListeGenerique ctrs = sender as UcListeGenerique;
                    if (ctrs.isOkClick)
                    {
                        CsCentre _LeCentreClient = (CsCentre)ctrs.MyObject;
                        if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
                        {
                            this.Txt_CodeCentre.Text = _LeCentreClient.CODE;
                            this.Txt_LibelleCentre.Text = _LeCentreClient.LIBELLE;
                            IdCentreClient = _LeCentreClient.PK_ID;
                            this.csTarifFacturation.FK_IDCENTRE = IdCentreClient;
                            this.csTarifFacturation.LIBELLECENTRE = _LeCentreClient.LIBELLE;
                            this.csTarifFacturation.CENTRE = _LeCentreClient.CODE;
                        }
                    }
                }
                int IdCentreClient = 0;
                private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(Txt_CodeCentre.Text) && Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                        {
                            CsCentre _LeCentreClient = ClasseMEthodeGenerique.RetourneObjectFromList(LstCentre, this.csTarifFacturation.FK_IDCENTRE.ToString(), "PK_ID");
                            if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
                            {
                                this.Txt_LibelleCentre.Text = _LeCentreClient.LIBELLE;
                                IdCentreClient = _LeCentreClient.PK_ID;
                                this. csTarifFacturation.FK_IDCENTRE = IdCentreClient;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Langue.lbl_Menu);
                    }
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
                        CsProduit _Leproduit = (CsProduit)ctrs.MyObject;
                        this.Txt_CodeProduit.Text = _Leproduit.CODE;
                        this.Txt_LibelleProduit.Text = _Leproduit.LIBELLE;
                    }
                }
                private void Txt_CodeProduit_TextChanged(object sender, TextChangedEventArgs e)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(Txt_CodeProduit.Text) && this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
                        {
                            CsProduit _LeProduit = ClasseMEthodeGenerique.RetourneObjectFromList(LstDeProduit, this.Txt_CodeProduit.Text, "CODE");
                            if (!string.IsNullOrEmpty(_LeProduit.CODE))

                                this.Txt_LibelleProduit.Text = _LeProduit.LIBELLE;
                            this.csTarifFacturation.FK_IDPRODUIT = _LeProduit.PK_ID;
                            this.csTarifFacturation.LIBELLEPRODUIT = _LeProduit.LIBELLE;
                            this.csTarifFacturation.PRODUIT = _LeProduit.CODE;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Langue.lbl_Menu);
                    }
                }

        
        
                private void btn_UniteComptage_Click(object sender, RoutedEventArgs e)
                        {

                            try
                            {
                                if (LstUniteComptage != null && LstUniteComptage.Count != 0)
                                {
                                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstUniteComptage);
                                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                                    ctr.Closed += new EventHandler(galatee_OkClickedUniteComptage);
                                    ctr.Show();
                                }

                            }
                            catch (Exception ex)
                            {
                                string error = ex.Message;
                            }
                        }
                void galatee_OkClickedUniteComptage(object sender, EventArgs e)
                {
                    UcListeGenerique ctrs = sender as UcListeGenerique;
                    if (ctrs.GetisOkClick)
                    {
                        CsUniteComptage _LeUniteComptage = (CsUniteComptage)ctrs.MyObject;
                        this.Txt_CodeUniteComptage.Text = _LeUniteComptage.CODE;
                        this.Txt_LibelleUniteComptage.Text = _LeUniteComptage.LIBELLE;
                        Txt_LibelleUniteComptage.Tag = _LeUniteComptage;
                        this.csTarifFacturation.FK_IDUNITECOMPTAGE = _LeUniteComptage.PK_ID;
                        this.csTarifFacturation.UNITE = _LeUniteComptage.CODE;
                    }
                }
                private void Txt_CodeUniteComptage_TextChanged(object sender, TextChangedEventArgs e)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(Txt_CodeUniteComptage.Text) )
                        {
                            CsUniteComptage _LeUniteComptage = ClasseMEthodeGenerique.RetourneObjectFromList(LstUniteComptage , this.Txt_CodeUniteComptage.Text, "CODE");
                            if (!string.IsNullOrEmpty(_LeUniteComptage.CODE))
                            {
                                this.Txt_LibelleUniteComptage.Text = _LeUniteComptage.LIBELLE;
                                this.csTarifFacturation.FK_IDUNITECOMPTAGE = _LeUniteComptage.PK_ID;
                                //this.csTarifFacturation.LIBELLEUniteComptage = _LeUniteComptage.LIBELLE;
                                this.csTarifFacturation.UNITE = _LeUniteComptage.CODE;
                                this.csTarifFacturation.FK_IDUNITECOMPTAGE  = _LeUniteComptage.PK_ID ;
                                Txt_LibelleUniteComptage.Tag = _LeUniteComptage;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Langue.lbl_Menu);
                    }
                }

                private void btn_Commune_Click(object sender, RoutedEventArgs e)
                {

                    try
                    {
                        if (LstCommune != null && LstCommune.Count != 0)
                        {
                            List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCommune);
                            UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                            ctr.Closed += new EventHandler(galatee_OkClickedCommune);
                            ctr.Show();
                        }

                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                    }
                }
                void galatee_OkClickedCommune(object sender, EventArgs e)
                {
                    UcListeGenerique ctrs = sender as UcListeGenerique;
                    if (ctrs.GetisOkClick)
                    {
                        CsCommune _Lecommune = (CsCommune)ctrs.MyObject;
                        this.Txt_CodeCommune.Text = _Lecommune.CODE;
                        this.Txt_LibelleCommune.Text = _Lecommune.LIBELLE;
                    }
                }
                private void Txt_CodeCommune_TextChanged(object sender, TextChangedEventArgs e)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(Txt_CodeCommune.Text) )
                        {
                            CsCommune _LaCommune = ClasseMEthodeGenerique.RetourneObjectFromList(LstCommune, this.Txt_CodeCommune.Text, "CODE");
                            if (!string.IsNullOrEmpty(_LaCommune.CODE))

                                this.Txt_LibelleCommune.Text = _LaCommune.LIBELLE;
                            this.csTarifFacturation.COMMUNE = _LaCommune.CODE;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Langue.lbl_Menu);
                    }
                }


                private void btn_Taxe_Click(object sender, RoutedEventArgs e)
                {

                    try
                    {
                        if (LstCodeApplicationTaxe != null && LstCodeApplicationTaxe.Count != 0)
                        {
                            List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCodeApplicationTaxe);
                            UcListeGenerique ctr = new UcListeGenerique(_LstObject, "PK_CODEAPPLICATION", "LIBELLE", "Liste");
                            ctr.Closed += new EventHandler(galatee_OkClickedTaxe);
                            ctr.Show();
                        }

                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                    }
                }
                void galatee_OkClickedTaxe(object sender, EventArgs e)
                {
                    UcListeGenerique ctrs = sender as UcListeGenerique;
                    if (ctrs.GetisOkClick)
                    {
                        CsCodeTaxeApplication _LeTaxe = (CsCodeTaxeApplication)ctrs.MyObject;
                        this.Txt_CodeTaxe.Text = _LeTaxe.PK_CODEAPPLICATION;
                        this.Txt_LibelleTaxe.Text = _LeTaxe.LIBELLE;
                        Txt_LibelleTaxe.Tag = _LeTaxe;


                        this.csTarifFacturation.FK_IDTAXE = int.Parse(_LeTaxe.PK_ID);
                        this.csTarifFacturation.TAXE = _LeTaxe.PK_CODEAPPLICATION; ;
                    }
                }
                private void Txt_CodeTaxe_TextChanged(object sender, TextChangedEventArgs e)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(Txt_CodeTaxe.Text))
                        {
                            CsCodeTaxeApplication _LaTaxe = ClasseMEthodeGenerique.RetourneObjectFromList(LstCodeApplicationTaxe, this.Txt_CodeTaxe.Text, "PK_CODEAPPLICATION");
                            if (!string.IsNullOrEmpty(_LaTaxe.PK_CODEAPPLICATION))
                            {
                                this.Txt_LibelleTaxe.Text = _LaTaxe.LIBELLE;
                                this.csTarifFacturation.FK_IDTAXE = int.Parse(_LaTaxe.PK_ID);
                                this.csTarifFacturation.TAXE = _LaTaxe.PK_CODEAPPLICATION; ;
                                Txt_LibelleTaxe.Tag = _LaTaxe;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Langue.lbl_Menu);
                    }
                }
                //private void btn_UniteComptage_Click(object sender, RoutedEventArgs e)
                //{

                //    try
                //    {
                //        if (LstUniteComptage != null && LstUniteComptage.Count != 0)
                //        {
                //            List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstUniteComptage);
                //            UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                //            ctr.Closed += new EventHandler(galatee_OkClickedUniteComptage);
                //            ctr.Show();
                //        }

                //    }
                //    catch (Exception ex)
                //    {
                //        string error = ex.Message;
                //    }
                //}
                //void galatee_OkClickedUniteComptage(object sender, EventArgs e)
                //{
                //    UcListeGenerique ctrs = sender as UcListeGenerique;
                //    if (ctrs.GetisOkClick)
                //    {
                //        CsUniteComptage _LeUniteComptage = (CsUniteComptage)ctrs.MyObject;
                //        this.Txt_CodeUniteComptage.Text = _LeUniteComptage.CODE;
                //        this.Txt_LibelleUniteComptage.Text = _LeUniteComptage.LIBELLE;
                //    }
                //}
                //private void Txt_CodeProduit_TextChanged(object sender, TextChangedEventArgs e)
                //{
                //    try
                //    {
                //        if (!string.IsNullOrEmpty(Txt_CodeUniteComptage.Text) )
                //        {
                //            CsUniteComptage _LeUniteComptage = ClasseMEthodeGenerique.RetourneObjectFromList(LstUniteComptage, this.Txt_CodeUniteComptage.Text, LstUniteComptage[0].CODE);
                //            if (!string.IsNullOrEmpty(_LeUniteComptage.CODE))

                //                this.Txt_LibelleUniteComptage.Text = _LeUniteComptage.LIBELLE;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Message.ShowError(ex.Message, Langue.lbl_Menu);
                //    }
                //}


                //    private void OKButton_Click(object sender, RoutedEventArgs e)
            //    {
            //        if (this.csVariableDeTarification.FK_IDCENTRE < 0 && this.csVariableDeTarification.FK_IDMODEAPPLICATION < 0 && this.csVariableDeTarification.FK_IDMODECALCUL < 0 && this.csVariableDeTarification.FK_IDTarifFacturation < 0 && this.csVariableDeTarification.FK_IDREDEVANCE<0)
            //        {
            //            this.csVariableDeTarification.FK_IDCENTRE = Txt_LibelleCentre1.Tag != null ? ((CsCentre)Txt_LibelleCentre1.Tag).PK_ID : 0;
            //            this.csVariableDeTarification.FK_IDMODEAPPLICATION = Txt_LibelleModeApp.Tag != null ? ((CsModeApplicationTarif)Txt_LibelleModeApp.Tag).PK_ID : 0;
            //            this.csVariableDeTarification.FK_IDMODECALCUL = Txt_LibelleModeCalcule.Tag != null ? ((CsModeCalcul)Txt_LibelleModeCalcule.Tag).PK_ID : 0;
            //            this.csVariableDeTarification.FK_IDTarifFacturation = Txt_LibelleTarifFacturation.Tag != null ? ((CsTarifFacturation)Txt_LibelleTarifFacturation.Tag).PK_ID : 0;
            //            this.csVariableDeTarification.FK_IDREDEVANCE = Txt_LibelleRedevence.Tag != null ? ((CsRedevance)Txt_LibelleRedevence.Tag).PK_ID : 0;
            //            MyEventArg.Bag = this.csVariableDeTarification;
            //            OnEvent(MyEventArg);
            //        }
            //        else
            //        {
            //            Message.ShowInformation("Veuillez vous assurer que tous les champs obligatoire sont renseignés", "Information");
            //        }
            //        this.DialogResult = true;
            //    }
            //    private void CancelButton_Click(object sender, RoutedEventArgs e)
            //    {
            //        this.DialogResult = false;
            //    }

            //    private void btn_Centre_Click(object sender, RoutedEventArgs e)
            //    {

            //        try
            //        {

            //            List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCentre.OrderBy(p => p.CODE).ToList());
            //            UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
            //            ctr.Closed += new EventHandler(galatee_OkClicked);
            //            ctr.Show();
            //        }
            //        catch (Exception ex)
            //        {
            //            string error = ex.Message;
            //        }
            //    }
            //    void galatee_OkClicked(object sender, EventArgs e)
            //    {
            //        UcListeGenerique ctrs = sender as UcListeGenerique;
            //        if (ctrs.isOkClick)
            //        {
            //            CsCentre _LaCateg = (CsCentre)ctrs.MyObject;
            //            this.Txt_CodeCentre.Text = _LaCateg.CODE;
            //        }
            //    }
            //    int IdCentreClient = 0;
            //    private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
            //    {
            //        try
            //        {
            //            if (!string.IsNullOrEmpty(Txt_CodeCentre.Text) && Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
            //            {
            //                CsCentre _LeCentreClient = ClasseMEthodeGenerique.RetourneObjectFromList(LstCentre, this.Txt_CodeCentre.Text, "CODE");
            //                if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
            //                {
            //                    this.Txt_LibelleCentre1.Text = _LeCentreClient.LIBELLE;
            //                    IdCentreClient = _LeCentreClient.PK_ID;
            //                    this.csVariableDeTarification.FK_IDCENTRE = IdCentreClient;
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Message.ShowError(ex.Message, Langue.lbl_Menu);
            //        }
            //    }

            //    private void btn_Redevence_Click(object sender, RoutedEventArgs e)
            //    {

            //        try
            //        {

            //            List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(ListeRedevence.OrderBy(p => p.CODE).ToList());
            //            UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
            //            ctr.Closed += new EventHandler(galatee_Redevence_OkClicked);
            //            ctr.Show();
            //        }
            //        catch (Exception ex)
            //        {
            //            string error = ex.Message;
            //        }
            //    }
            //    private void galatee_Redevence_OkClicked(object sender, EventArgs e)
            //    {
            //        UcListeGenerique ctrs = sender as UcListeGenerique;
            //        if (ctrs.isOkClick)
            //        {
            //            CsRedevance _LaRedevance = (CsRedevance)ctrs.MyObject;
            //            this.Txt_CodeRedevence.Text = _LaRedevance.CODE;
            //        }
            //    }
            //    int IdRedevance = 0;
            //    private void Txt_CodeRedevence_TextChanged(object sender, TextChangedEventArgs e)
            //    {
            //        try
            //        {
            //            if (!string.IsNullOrEmpty(Txt_CodeRedevence.Text) && Txt_CodeRedevence.Text.Length == 2)
            //            {
            //                CsRedevance _LaRedevance = ClasseMEthodeGenerique.RetourneObjectFromList(ListeRedevence, this.Txt_CodeRedevence.Text, "CODE");
            //                if (!string.IsNullOrEmpty(_LaRedevance.LIBELLE))
            //                {
            //                    this.Txt_LibelleRedevence.Text = _LaRedevance.LIBELLE;
            //                    IdRedevance = _LaRedevance.PK_ID;
            //                    this.csVariableDeTarification.FK_IDREDEVANCE = IdRedevance;

            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Message.ShowError(ex.Message, Langue.lbl_Menu);
            //        }
            //    }

            //    private void btn_TarifFacturation_Click(object sender, RoutedEventArgs e)
            //    {
            //        try
            //        {
            //            List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(ListeTarifFacturation.OrderBy(p => p.CODE).ToList());
            //            UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
            //            ctr.Closed += new EventHandler(galatee_TarifFacturation_OkClicked);
            //            ctr.Show();
            //        }
            //        catch (Exception ex)
            //        {
            //            string error = ex.Message;
            //        }
            //    }
            //    private void galatee_TarifFacturation_OkClicked(object sender, EventArgs e)
            //    {
            //        UcListeGenerique ctrs = sender as UcListeGenerique;
            //        if (ctrs.isOkClick)
            //        {
            //            CsTarifFacturation _LaTarifFacturation = (CsTarifFacturation)ctrs.MyObject;
            //            this.Txt_CodeTarifFacturation.Text = _LaTarifFacturation.CODE;
            //        }
            //    }
            //    int IdTarifFacturation = 0;
            //    private void Txt_CodeTarifFacturation_TextChanged(object sender, TextChangedEventArgs e)
            //    {
            //        try
            //        {
            //            if (!string.IsNullOrEmpty(Txt_CodeTarifFacturation.Text) && Txt_CodeTarifFacturation.Text.Length == 3)
            //            {
            //                CsTarifFacturation _LaTarifFacturation = ClasseMEthodeGenerique.RetourneObjectFromList(ListeTarifFacturation, this.Txt_CodeTarifFacturation.Text, "CODE");
            //                if (!string.IsNullOrEmpty(_LaTarifFacturation.LIBELLE))
            //                {
            //                    this.Txt_LibelleRedevence.Text = _LaTarifFacturation.LIBELLE;
            //                    IdTarifFacturation = _LaTarifFacturation.PK_ID;
            //                    this.csVariableDeTarification.FK_IDTarifFacturation = IdTarifFacturation;

            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Message.ShowError(ex.Message, Langue.lbl_Menu);
            //        }
            //    }

            //    private void btn_ModeCalcule_Click(object sender, RoutedEventArgs e)
            //    {
            //        try
            //        {
            //            List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(ListeModeCalcule.OrderBy(p => p.CODE).ToList());
            //            UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
            //            ctr.Closed += new EventHandler(galatee_ModeCalcule_OkClicked);
            //            ctr.Show();
            //        }
            //        catch (Exception ex)
            //        {
            //            string error = ex.Message;
            //        }
            //    }
            //    private void galatee_ModeCalcule_OkClicked(object sender, EventArgs e)
            //    {
            //        UcListeGenerique ctrs = sender as UcListeGenerique;
            //        if (ctrs.isOkClick)
            //        {
            //            CsModeCalcul _LeModeCalcul = (CsModeCalcul)ctrs.MyObject;
            //            this.Txt_CodeModeCalcule.Text = _LeModeCalcul.CODE;
            //        }
            //    }
            //    int IdModeCalcul = 0;
            //    private void Txt_CodeModeCalcule_TextChanged(object sender, TextChangedEventArgs e)
            //    {
            //        try
            //        {
            //            if (!string.IsNullOrEmpty(Txt_CodeModeCalcule.Text) && Txt_CodeModeCalcule.Text.Length == 2)
            //            {
            //                CsModeCalcul _LeModeCalcul = ClasseMEthodeGenerique.RetourneObjectFromList(ListeModeCalcule, this.Txt_CodeModeCalcule.Text, "CODE");
            //                if (!string.IsNullOrEmpty(_LeModeCalcul.LIBELLE))
            //                {
            //                    this.Txt_LibelleModeCalcule.Text = _LeModeCalcul.LIBELLE;
            //                    IdModeCalcul = _LeModeCalcul.PK_ID;
            //                    this.csVariableDeTarification.FK_IDMODECALCUL = IdModeCalcul;

            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Message.ShowError(ex.Message, Langue.lbl_Menu);
            //        }
            //    }

            //    private void btn_ModeApp_Click(object sender, RoutedEventArgs e)
            //    {
            //        try
            //        {
            //            List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(ListeModeApplicationTarif.OrderBy(p => p.CODE).ToList());
            //            UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
            //            ctr.Closed += new EventHandler(galatee_ModeApp_OkClicked);
            //            ctr.Show();
            //        }
            //        catch (Exception ex)
            //        {
            //            string error = ex.Message;
            //        }
            //    }
            //    private void galatee_ModeApp_OkClicked(object sender, EventArgs e)
            //    {
            //        UcListeGenerique ctrs = sender as UcListeGenerique;
            //        if (ctrs.isOkClick)
            //        {
            //            CsModeApplicationTarif _LeModeApp = (CsModeApplicationTarif)ctrs.MyObject;
            //            this.Txt_CodeModeApp.Text = _LeModeApp.CODE;
            //        }
            //    }
            //    int IdModeApp = 0;
            //    private void Txt_CodeModeApp_TextChanged(object sender, TextChangedEventArgs e)
            //    {
            //        try
            //        {
            //            if (!string.IsNullOrEmpty(Txt_CodeModeApp.Text) && Txt_CodeModeApp.Text.Length == 1)
            //            {
            //                CsModeApplicationTarif _LeModeApp = ClasseMEthodeGenerique.RetourneObjectFromList(ListeModeApplicationTarif, this.Txt_CodeModeApp.Text, "CODE");
            //                if (!string.IsNullOrEmpty(_LeModeApp.LIBELLE))
            //                {
            //                    this.Txt_LibelleModeApp.Text = _LeModeApp.LIBELLE;
            //                    IdModeApp = _LeModeApp.PK_ID;
            //                    this.csVariableDeTarification.FK_IDMODEAPPLICATION = IdModeApp;

            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Message.ShowError(ex.Message, Langue.lbl_Menu);
            //        }
                //    }



                #endregion

                #region Servies
                public void LoadAllRedevance()
                {

                    try
                    {
                        if (SessionObject.ListeRedevence.Count > 0)
                        {
                            return;
                        }
                        //else
                        //{
                        TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                        int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                        service.LoadAllRedevanceAsync();
                        service.LoadAllRedevanceCompleted += (er, res) =>
                        {
                            try
                            {
                                if (res.Error != null || res.Cancelled)
                                    Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                                else
                                    if (res.Result != null)
                                    {
                                        SessionObject.ListeRedevence = res.Result;
                                        foreach (var item in SessionObject.ListeRedevence)
                                        {
                                            ListeRedevence.Add(item);
                                        }
                                    }
                                    else
                                        Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                            "Erreur");
                                LoadingManager.EndLoading(handler);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        };

                        //    }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                private void ChargerDonneeDuSite()
                {
                    try
                    {
                        if (SessionObject.LstCentre.Count != 0)
                        {
                            LstCentre = SessionObject.LstCentre;

                            return;
                        }
                        Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                        service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                        {
                            if (args != null && args.Cancelled)
                                return;
                            SessionObject.LstCentre = args.Result;
                            LstCentre = SessionObject.LstCentre;
                        };
                        service.ListeDesDonneesDesSiteAsync(false );
                        service.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, "ChargerDonneeDuSite");

                    }
                }
                private void ChargerLesBrancheDesCommune()
                {
                    try
                    {
                        if (SessionObject.LstCommune .Count != 0)
                        {
                            LstCommune = SessionObject.LstCommune;
                            return;
                        }
                        AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                        service1.ChargerCommuneCompleted  += (sr, res) =>
                        {
                            if (res != null && res.Cancelled)
                                return;
                            SessionObject.LstCommune = res.Result;
                            LstCommune = SessionObject.LstCommune;

                        };
                        service1.ChargerCommuneAsync ();
                        service1.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, "ChargerListeDeProduit");
                    }
                }
                private void ChargerApplicationTaxe()
                {
                    try
                    {
                        if (SessionObject.LstCodeApplicationTaxe.Count != 0)
                            LstCodeApplicationTaxe = SessionObject.LstCodeApplicationTaxe;
                        else
                        {
                            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                            service.RetourneTousApplicationTaxeCompleted += (s, args) =>
                            {
                                if (args != null && args.Cancelled)
                                    return;
                                SessionObject.LstCodeApplicationTaxe = args.Result;
                                LstCodeApplicationTaxe = SessionObject.LstCodeApplicationTaxe;

                            };
                            service.RetourneTousApplicationTaxeAsync();
                            service.CloseAsync();
                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
                private void ChargerListeDeProduit()
                {
                    try
                    {
                        if (SessionObject.ListeDesProduit .Count != 0)
                        {
                            LstDeProduit = SessionObject.ListeDesProduit;
                            return;
                        }
                        AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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

                private void Txt_LibelleTaxe_TextChanged(object sender, TextChangedEventArgs e)
                {

                }

                //public void LoadAllTarifFacturation()
                //{
                //    try
                //    {
                //        if (SessionObject.ListeTarifFacturation.Count != 0)
                //        {
                //            ListeTarifFacturation = SessionObject.ListeTarifFacturation;

                //            return;
                //        }
                //        TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                //        int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                //        service.LoadAllTarifFacturationAsync();
                //        service.LoadAllTarifFacturationCompleted += (er, res) =>
                //        {
                //            try
                //            {
                //                if (res.Error != null || res.Cancelled)
                //                    Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                //                else
                //                    if (res.Result != null)
                //                    {
                //                        foreach (var item in res.Result)
                //                        {
                //                            ListeTarifFacturation.Add(item);
                //                        }
                //                        SessionObject.ListeTarifFacturation = ListeTarifFacturation;
                //                        InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
                //                        //System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeTarifFacturation);
                //                        //dgListeTarifFacturation.ItemsSource = view;
                //                        //datapager.Source = view;
                //                    }
                //                    else
                //                        Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                //                            "Erreur");
                //                LoadingManager.EndLoading(handler);
                //            }
                //            catch (Exception ex)
                //            {
                //                throw ex;
                //            }
                //        };

                //        //    }
                //    }
                //    catch (Exception ex)
                //    {
                //        throw ex;
                //    }
                //}
                //private void LoadAllModeCalcule()
                //{
                //    try
                //    {
                //        if (SessionObject.ListeModeCalcule.Count != 0)
                //        {
                //            ListeModeCalcule = SessionObject.ListeModeCalcule;

                //            return;
                //        }
                //        TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                //        int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                //        service.LoadAllModeCalculeAsync();
                //        service.LoadAllModeCalculeCompleted += (er, res) =>
                //        {
                //            try
                //            {
                //                if (res.Error != null || res.Cancelled)
                //                    Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                //                else
                //                    if (res.Result != null)
                //                    {
                //                        foreach (var item in res.Result)
                //                        {
                //                            ListeModeCalcule.Add(item);
                //                        }
                //                        SessionObject.ListeModeCalcule = ListeModeCalcule;
                //                        InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
                //                        //System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeModeCalcule);
                //                        //dgListeTarifFacturation.ItemsSource = view;
                //                        //datapager.Source = view;
                //                    }
                //                    else
                //                        Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                //                            "Erreur");
                //                LoadingManager.EndLoading(handler);
                //            }
                //            catch (Exception ex)
                //            {
                //                throw ex;
                //            }
                //        };

                //        //    }
                //    }
                //    catch (Exception ex)
                //    {
                //        throw ex;
                //    }
                //}
                //private void LoadAllModeApplicationTarif()
                //{
                //    try
                //    {
                //        if (SessionObject.ListeModeApplicationTarif.Count != 0)
                //        {
                //            ListeModeApplicationTarif = SessionObject.ListeModeApplicationTarif;

                //            return;
                //        }
                //        TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                //        int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                //        service.LoadAllModeApplicationTarifAsync();
                //        service.LoadAllModeApplicationTarifCompleted += (er, res) =>
                //        {
                //            try
                //            {
                //                if (res.Error != null || res.Cancelled)
                //                    Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                //                else
                //                    if (res.Result != null)
                //                    {
                //                        foreach (var item in res.Result)
                //                        {
                //                            ListeModeApplicationTarif.Add(item);
                //                        }
                //                        SessionObject.ListeModeApplicationTarif = ListeModeApplicationTarif;
                //                        InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
                //                        //System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeModeApplicationTarif);
                //                        //dgListeTarifFacturation.ItemsSource = view;
                //                        //datapager.Source = view;
                //                    }
                //                    else
                //                        Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                //                            "Erreur");
                //                LoadingManager.EndLoading(handler);
                //            }
                //            catch (Exception ex)
                //            {
                //                throw ex;
                //            }
                //        };

                //        //    }
                //    }
                //    catch (Exception ex)
                //    {
                //        throw ex;
                //    }
                //}

            #endregion

        #endregion

    }
}


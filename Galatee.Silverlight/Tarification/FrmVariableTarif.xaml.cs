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
    public partial class FrmVariableTarif : ChildWindow
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

        List<CsModeCalcul> ListeModeCalcule = new List<CsModeCalcul>();
        List<CsModeApplicationTarif> ListeModeApplicationTarif = new List<CsModeApplicationTarif>();
        List<CsRedevance> ListeRedevence = new List<CsRedevance>();
        public static List<CsCentre> LstCentre = new List<CsCentre>();
        List<CsRechercheTarif> ListeRechercheTarif = new List<CsRechercheTarif>();

        List<CsSite> lstSite = new List<CsSite>();



            private bool p1;
            private bool p2;
            private bool p;
            private CsVariableDeTarification csVariableDeTarification;
            private object p3;
            private bool p4;
        #endregion

        #region Constructeurs

            public FrmVariableTarif()
            {
                InitializeComponent();

                this.csVariableDeTarification = new ServiceTarification.CsVariableDeTarification();
                //this.csVariableDeTarification.TRANCHEREDEVANCE = new List<ServiceTarification.CsTrancheRedevence>();

                LayoutRoot.DataContext = this.csVariableDeTarification;

                txt_DateAppli.SelectedDate = DateTime.Now;
                EstAnalytique.IsChecked = false;
                GenerationAnomalie.IsChecked = false;

                LoadAllRechercheTarif();
                LoadAllModeCalcule();
                LoadAllModeApplicationTarif();
                LoadAllRedevance();
                ChargerDonneeDuSite();

            }

            public FrmVariableTarif(CsVariableDeTarification csVariableDeTarification)
            {
                InitializeComponent();

                LoadAllRechercheTarif();
                LoadAllModeCalcule();
                LoadAllModeApplicationTarif();
                LoadAllRedevance();
                ChargerDonneeDuSite();

                // TODO: Complete member initialization
                this.csVariableDeTarification = csVariableDeTarification;

                //DataBinding de la redevence au context de la fenetre
                LayoutRoot.DataContext = this.csVariableDeTarification;

                //Mise de la fenetre en lecture 
                InitializeScreenConsultation();

            }

            public FrmVariableTarif(CsVariableDeTarification csVariableDeTarification, bool p)
            {
                InitializeComponent();

                LoadAllRechercheTarif();
                LoadAllModeCalcule();
                LoadAllModeApplicationTarif();
                LoadAllRedevance();
                ChargerDonneeDuSite();

                // TODO: Complete member initialization
                this.csVariableDeTarification = csVariableDeTarification;
                this.p = p;

                //DataBinding de la redevence au context de la fenetre
                LayoutRoot.DataContext = this.csVariableDeTarification;

                //Mise de la fenetre en lecture 
                InitializeScreenModification();

            }

        #endregion

        #region Methodes d'interface

            private void InitializeScreenConsultation()
            {
                Txt_CodeCentre.IsReadOnly = true;
                Txt_CodeModeApp.IsReadOnly = true;
                Txt_CodeModeCalcule.IsReadOnly = true;
                Txt_CodeRechercheTarif.IsReadOnly = true;
                Txt_CodeRedevence.IsReadOnly = true;
                txt_commune.IsReadOnly = true;
                txt_DateAppli.IsEnabled = false;
                txt_Formule.IsReadOnly = true;
                Txt_LibelleCentre1.IsReadOnly = true;
                txt_LibelleComptable.IsReadOnly = true;
                Txt_LibelleModeApp.IsReadOnly = true;
                Txt_LibelleModeCalcule.IsReadOnly = true;
                Txt_LibelleRedevence.IsReadOnly = true;
                txt_OrdreEdition.IsReadOnly = true;
                txt_Region.IsReadOnly = true;
                txt_SRegion.IsReadOnly = true;
                txt_CompteComptable.IsReadOnly = true;

                GenerationAnomalie.IsEnabled = false;
                EstAnalytique.IsEnabled = false;

                btn_Centre1.IsEnabled = false;
                btn_ModeApp.IsEnabled = false;
                btn_ModeCalcule.IsEnabled = false;
                btn_RechercheTarif.IsEnabled = false;
                btn_Redevence.IsEnabled = false;

            
                OKButton.IsEnabled = false;

                InitCentre_Redev_RechTarif_ModeCalc_ModeApp(); 

            }

            private void InitCentre_Redev_RechTarif_ModeCalc_ModeApp()
            {
                CsRedevance _LeRedevence = ListeRedevence.FirstOrDefault(p => p.PK_ID == this.csVariableDeTarification.FK_IDREDEVANCE);
                if (_LeRedevence != null)
                {
                    this.Txt_CodeRedevence.Text = _LeRedevence.CODE;
                    this.Txt_LibelleRedevence.Text = _LeRedevence.LIBELLE;
                    this.Txt_LibelleRedevence.Tag = _LeRedevence;
                }

                CsRechercheTarif _LeRechercheTarif = ListeRechercheTarif.FirstOrDefault(p => p.PK_ID == this.csVariableDeTarification.FK_IDRECHERCHETARIF);
                if (_LeRechercheTarif != null)
                {
                    this.Txt_CodeRechercheTarif.Text = _LeRechercheTarif.CODE;
                    this.Txt_LibelleRechercheTarif.Text = _LeRechercheTarif.LIBELLE;
                    this.Txt_LibelleRechercheTarif.Tag = _LeRechercheTarif;
                }

                CsModeCalcul _LeModeCalcul = ListeModeCalcule.FirstOrDefault(p => p.PK_ID == this.csVariableDeTarification.FK_IDMODECALCUL);
                if (_LeModeCalcul != null)
                {
                    this.Txt_CodeModeCalcule.Text = _LeModeCalcul.CODE;
                    this.Txt_LibelleModeCalcule.Text = _LeModeCalcul.LIBELLE;
                    this.Txt_LibelleModeCalcule.Tag = _LeModeCalcul;
                }

                CsModeApplicationTarif _LeModeApp = ListeModeApplicationTarif.FirstOrDefault(p => p.PK_ID == this.csVariableDeTarification.FK_IDMODEAPPLICATION);
                if (_LeModeApp != null)
                {
                    this.Txt_CodeModeApp.Text = _LeModeApp.CODE;
                    this.Txt_LibelleModeApp.Text = _LeModeApp.LIBELLE;
                    this.Txt_LibelleModeApp.Tag = _LeModeApp;
                }

                Galatee.Silverlight.ServiceAccueil.CsCentre _Lecentre = LstCentre.FirstOrDefault(p => p.PK_ID == this.csVariableDeTarification.FK_IDCENTRE);
                if (_Lecentre != null)
                {
                    this.Txt_CodeCentre.Text = _Lecentre.CODE;
                    this.Txt_LibelleCentre1.Text = _Lecentre.LIBELLE;
                    this.Txt_LibelleCentre1.Tag = _Lecentre;
                }
            }

            private void InitializeScreenModification()
            {
                //txt_code.IsReadOnly = true;
                InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
            }

        #endregion

        #region Events Handlers

            private void txt_OrdreEdition_TextChanged(object sender, TextChangedEventArgs e)
            {
                int ordre = 0;
                if (!int.TryParse(txt_OrdreEdition.Text, out ordre))
                {
                    Message.ShowError("Veuillez saisir une valeur numérique dans le champ ordre d'ediion", "Erreur");
                }
            }

            private void OKButton_Click(object sender, RoutedEventArgs e)
            {
                if (this.csVariableDeTarification.FK_IDCENTRE > 0 && this.csVariableDeTarification.FK_IDMODEAPPLICATION > 0
                    && this.csVariableDeTarification.FK_IDMODECALCUL > 0 && this.csVariableDeTarification.FK_IDRECHERCHETARIF > 0 
                    && this.csVariableDeTarification.FK_IDREDEVANCE>0)
                {
                    this.csVariableDeTarification.CENTRE  =!string.IsNullOrEmpty( Txt_CodeCentre.Text  )   ? Txt_CodeCentre.Text  : string.Empty ;
                    this.csVariableDeTarification.MODEAPPLICATION = !string.IsNullOrEmpty(this.Txt_CodeModeApp.Text) ? Txt_CodeModeApp.Text : string.Empty;
                    this.csVariableDeTarification.MODECALCUL = !string.IsNullOrEmpty(this.Txt_CodeModeCalcule.Text) ? Txt_CodeModeCalcule.Text : string.Empty;
                    this.csVariableDeTarification.RECHERCHETARIF = !string.IsNullOrEmpty(this.Txt_CodeRechercheTarif.Text) ? Txt_CodeRechercheTarif.Text : string.Empty;
                    this.csVariableDeTarification.REDEVANCE = !string.IsNullOrEmpty(this.Txt_CodeRedevence.Text) ? Txt_CodeRedevence.Text : string.Empty;
                    this.csVariableDeTarification.REGION = !string.IsNullOrEmpty(this.txt_Region.Text) ? txt_Region.Text : string.Empty;
                    this.csVariableDeTarification.SREGION = !string.IsNullOrEmpty(this.txt_SRegion.Text) ? txt_SRegion.Text : string.Empty;
                    this.csVariableDeTarification.COMMUNE = !string.IsNullOrEmpty(this.txt_commune.Text) ? txt_commune.Text : string.Empty;
                    this.csVariableDeTarification.DATEAPPLICATION   =this.txt_DateAppli.SelectedDate!=null? this.txt_DateAppli.SelectedDate.Value:DateTime.Now; 
                    this.csVariableDeTarification.USERCREATION = UserConnecte.matricule;
                    this.csVariableDeTarification.DATECREATION = System.DateTime.Today.Date;
                    this.csVariableDeTarification.ESTANALYTIQUE = EstAnalytique.IsChecked;
                    this.csVariableDeTarification.GENERATIONANOMALIE = GenerationAnomalie.IsChecked;


                    MyEventArg.Bag = this.csVariableDeTarification;
                    OnEvent(MyEventArg);
                    this.DialogResult = false;
                }
                else
                    Message.ShowInformation("Veuillez vous assurer que tous les champs obligatoire sont renseignés", "Information");
            }
            private void CancelButton_Click(object sender, RoutedEventArgs e)
            {
                Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);

            }

            private void btn_Site_Click(object sender, RoutedEventArgs e)
            {
                try
                {

                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(lstSite.OrderBy(p => p.CODE).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkSiteClicked);
                    ctr.Show();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            private void galatee_OkSiteClicked(object sender, EventArgs e)
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                CsSite _LeSite = (CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = _LeSite.CODE;
                LstCentre = SessionObject.LstCentre.Where(t => t.FK_IDCODESITE == _LeSite.PK_ID).ToList();
            }
            private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Txt_CodeSite.Text) && Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                    {
                        CsSite _LeSiteClient = ClasseMEthodeGenerique.RetourneObjectFromList(lstSite, this.Txt_CodeSite.Text, "CODE");
                        if (!string.IsNullOrEmpty(_LeSiteClient.LIBELLE))
                        {
                            this.Txt_LibelleSite.Text = _LeSiteClient.LIBELLE;
                            LstCentre = SessionObject.LstCentre.Where(t => t.FK_IDCODESITE == _LeSiteClient.PK_ID).ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Langue.lbl_Menu);

                }
            }

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
                    CsCentre _LaCateg = (CsCentre)ctrs.MyObject;
                    this.Txt_CodeCentre.Text = _LaCateg.CODE;
                }
            }
            int IdCentreClient = 0;
            private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Txt_CodeCentre.Text) && Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                    {
                        CsCentre _LeCentreClient = ClasseMEthodeGenerique.RetourneObjectFromList(LstCentre, this.Txt_CodeCentre.Text, "CODE");
                        if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
                        {
                            this.Txt_LibelleCentre1.Text = _LeCentreClient.LIBELLE;
                            IdCentreClient = _LeCentreClient.PK_ID;
                            this.csVariableDeTarification.FK_IDCENTRE = IdCentreClient;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Langue.lbl_Menu);
                }
            }

            private void btn_Redevence_Click(object sender, RoutedEventArgs e)
            {

                try
                {

                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(ListeRedevence.OrderBy(p => p.CODE).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_Redevence_OkClicked);
                    ctr.Show();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            private void galatee_Redevence_OkClicked(object sender, EventArgs e)
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsRedevance _LaRedevance = (CsRedevance)ctrs.MyObject;
                    this.Txt_CodeRedevence.Text = _LaRedevance.CODE;
                }
            }
            int IdRedevance = 0;
            private void Txt_CodeRedevence_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Txt_CodeRedevence.Text) && Txt_CodeRedevence.Text.Length == 2)
                    {
                        CsRedevance _LaRedevance = ClasseMEthodeGenerique.RetourneObjectFromList(ListeRedevence, this.Txt_CodeRedevence.Text, "CODE");
                        if (!string.IsNullOrEmpty(_LaRedevance.LIBELLE))
                        {
                            this.Txt_LibelleRedevence.Text = _LaRedevance.LIBELLE;
                            IdRedevance = _LaRedevance.PK_ID;
                            this.csVariableDeTarification.FK_IDREDEVANCE = IdRedevance;

                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Langue.lbl_Menu);
                }
            }

            private void btn_RechercheTarif_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(ListeRechercheTarif.OrderBy(p => p.CODE).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_RechercheTarif_OkClicked);
                    ctr.Show();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            private void galatee_RechercheTarif_OkClicked(object sender, EventArgs e)
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsRechercheTarif _LaRechercheTarif = (CsRechercheTarif)ctrs.MyObject;
                    this.Txt_CodeRechercheTarif.Text = _LaRechercheTarif.CODE;
                }
            }
            int IdRechercheTarif = 0;
            private void Txt_CodeRechercheTarif_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Txt_CodeRechercheTarif.Text) && Txt_CodeRechercheTarif.Text.Length == 3)
                    {
                        CsRechercheTarif _LaRechercheTarif = ClasseMEthodeGenerique.RetourneObjectFromList(ListeRechercheTarif, this.Txt_CodeRechercheTarif.Text, "CODE");
                        if (!string.IsNullOrEmpty(_LaRechercheTarif.LIBELLE))
                        {
                            this.Txt_LibelleRechercheTarif.Text = _LaRechercheTarif.LIBELLE;
                            IdRechercheTarif = _LaRechercheTarif.PK_ID;
                            this.csVariableDeTarification.FK_IDRECHERCHETARIF = IdRechercheTarif;

                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Langue.lbl_Menu);
                }
            }

            private void btn_ModeCalcule_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(ListeModeCalcule.OrderBy(p => p.CODE).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_ModeCalcule_OkClicked);
                    ctr.Show();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            private void galatee_ModeCalcule_OkClicked(object sender, EventArgs e)
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsModeCalcul _LeModeCalcul = (CsModeCalcul)ctrs.MyObject;
                    this.Txt_CodeModeCalcule.Text = _LeModeCalcul.CODE;
                }
            }
            int IdModeCalcul = 0;
            private void Txt_CodeModeCalcule_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Txt_CodeModeCalcule.Text) && Txt_CodeModeCalcule.Text.Length == 2)
                    {
                        CsModeCalcul _LeModeCalcul = ClasseMEthodeGenerique.RetourneObjectFromList(ListeModeCalcule, this.Txt_CodeModeCalcule.Text, "CODE");
                        if (!string.IsNullOrEmpty(_LeModeCalcul.LIBELLE))
                        {
                            this.Txt_LibelleModeCalcule.Text = _LeModeCalcul.LIBELLE;
                            IdModeCalcul = _LeModeCalcul.PK_ID;
                            this.csVariableDeTarification.FK_IDMODECALCUL = IdModeCalcul;

                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Langue.lbl_Menu);
                }
            }

            private void btn_ModeApp_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(ListeModeApplicationTarif.OrderBy(p => p.CODE).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_ModeApp_OkClicked);
                    ctr.Show();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            private void galatee_ModeApp_OkClicked(object sender, EventArgs e)
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsModeApplicationTarif _LeModeApp = (CsModeApplicationTarif)ctrs.MyObject;
                    this.Txt_CodeModeApp.Text = _LeModeApp.CODE;
                }
            }
            int IdModeApp = 0;
            private void Txt_CodeModeApp_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Txt_CodeModeApp.Text) && Txt_CodeModeApp.Text.Length == 1)
                    {
                        CsModeApplicationTarif _LeModeApp = ClasseMEthodeGenerique.RetourneObjectFromList(ListeModeApplicationTarif, this.Txt_CodeModeApp.Text, "CODE");
                        if (!string.IsNullOrEmpty(_LeModeApp.LIBELLE))
                        {
                            this.Txt_LibelleModeApp.Text = _LeModeApp.LIBELLE;
                            IdModeApp = _LeModeApp.PK_ID;
                            this.csVariableDeTarification.FK_IDMODEAPPLICATION = IdModeApp;

                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Langue.lbl_Menu);
                }
            }

          

        #endregion

        #region Servies

            private void ChargerDonneeDuSite()
            {
                try
                {
                    if (SessionObject.LstCentre.Count != 0)
                    {
                        LstCentre = SessionObject.LstCentre;
                        lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                        if (lstSite != null)
                        {
                            List<CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                            if (_LstSite.Count == 1)
                            {
                                this.Txt_CodeSite.Text = _LstSite[0].CODE;
                                this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                                this.btn_Site.IsEnabled = false;
                                this.Txt_CodeSite.IsReadOnly = true;
                            }
                        }
                        return;
                    }
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        LstCentre = SessionObject.LstCentre;
                        lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                        if (lstSite != null)
                        {
                            List<CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                            if (_LstSite.Count == 1)
                            {
                                this.Txt_CodeSite.Text = _LstSite[0].CODE;
                                this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                                this.btn_Site.IsEnabled = false;
                                this.Txt_CodeSite.IsReadOnly = true;
                            }
                        }
                    };
                    service.ListeDesDonneesDesSiteAsync(false);
                    service.CloseAsync();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "ChargerDonneeDuSite");

                }
            }
           
            public void LoadAllRedevance()
            {
                try
                {
                    if (SessionObject.ListeRedevence.Count != 0)
                    {
                        ListeRedevence = SessionObject.ListeRedevence;

                        return;
                    }
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
                                    SessionObject.ListeRedevence = ListeRedevence;
                                    InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
                                    //System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeRedevence);
                                    //dgListeRedevence.ItemsSource = view;
                                    //datapager.Source = view;
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
            public void LoadAllRechercheTarif()
            {
                try
                {
                    if (SessionObject.ListeRechercheTarif.Count != 0)
                    {
                        ListeRechercheTarif = SessionObject.ListeRechercheTarif;

                        return;
                    }
                    TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                    int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                    service.LoadAllRechercheTarifAsync();
                    service.LoadAllRechercheTarifCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {
                                    foreach (var item in res.Result)
                                    {
                                        ListeRechercheTarif.Add(item);
                                    }
                                    SessionObject.ListeRechercheTarif = ListeRechercheTarif;
                                    InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
                                    //System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeRechercheTarif);
                                    //dgListeRechercheTarif.ItemsSource = view;
                                    //datapager.Source = view;
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
            private void LoadAllModeCalcule()
            {
                try
                {
                    if (SessionObject.ListeModeCalcule.Count != 0)
                    {
                        ListeModeCalcule = SessionObject.ListeModeCalcule;

                        return;
                    }
                    TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                    int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                    service.LoadAllModeCalculeAsync();
                    service.LoadAllModeCalculeCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {
                                    foreach (var item in res.Result)
                                    {
                                        ListeModeCalcule.Add(item);
                                    }
                                    SessionObject.ListeModeCalcule = ListeModeCalcule;
                                    InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
                                    //System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeModeCalcule);
                                    //dgListeRechercheTarif.ItemsSource = view;
                                    //datapager.Source = view;
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
            private void LoadAllModeApplicationTarif()
            {
                try
                {
                    if (SessionObject.ListeModeApplicationTarif.Count != 0)
                    {
                        ListeModeApplicationTarif = SessionObject.ListeModeApplicationTarif;

                        return;
                    }
                    TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                    int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                    service.LoadAllModeApplicationTarifAsync();
                    service.LoadAllModeApplicationTarifCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {
                                    foreach (var item in res.Result)
                                    {
                                        ListeModeApplicationTarif.Add(item);
                                    }
                                    SessionObject.ListeModeApplicationTarif = ListeModeApplicationTarif;
                                    InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
                                    //System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeModeApplicationTarif);
                                    //dgListeRechercheTarif.ItemsSource = view;
                                    //datapager.Source = view;
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

        #endregion

           

    }
}


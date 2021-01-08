using Galatee.Silverlight.MainView;
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
    public partial class FrmGenerationCtarcomp : ChildWindow
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

        #region Constructeur

        public FrmGenerationCtarcomp()
        {
            InitializeComponent();
            #region Sylla
            ChargerDonneeDuSite();
            #endregion
            LoadAllVariableTarif();
            LoadAllRedevance();
            LoadAllRechercheTarif();
            ChargerListeDeProduit();
        }

       

        #endregion

        #region Events
        bool isAnnule = false;
            private void OKButton_Click(object sender, RoutedEventArgs e)
                {
                    this.DialogResult = true;
                }
            #region Sylla
          
            #endregion
            private void CancelButton_Click(object sender, RoutedEventArgs e)
            {
                isAnnule = true;
                this.DialogResult = false;
            }
            private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {
                if (TarifFacturationoInserte.Count > 0 )
                {
                    if (!isAnnule)
                        Save(TarifFacturationoInserte);
                }
            }

            private void dgListeTarifFacturation_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
            {

            }
            private void dgListeTarifFacturation_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
            {

            }
            private void dgListeTarifFacturation_LoadingRow(object sender, DataGridRowEventArgs e)
            {

            }

            private void Button_Click(object sender, RoutedEventArgs e)
            {
                string CTARCOMP = ((Button)e.OriginalSource).Tag.ToString();
                //int tarifselectionne_CTARCOMP = int.Parse(CTARCOMP);
                CsTarifFacturation tarifselectionne = ListeTarifFacturation.FirstOrDefault(t => t.CTARCOMP == CTARCOMP);
                FrmTarifFacturation Updatefrm = new FrmTarifFacturation(tarifselectionne, false );
                   
                Updatefrm.CallBack += Newfrm_CallBack;
                ListeTarifFacturation.Remove(tarifselectionne);
                TarifFacturationoInserte.Remove(tarifselectionne);
               
                Updatefrm.Show();
            }
            private void Button_Click_1(object sender, RoutedEventArgs e)
            {
                if (cbo_variable_tarification.SelectedItem != null)
                {
                    new FrmVariableTarif((CsVariableDeTarification)cbo_variable_tarification.SelectedItem).Show();
                }
                else
                {
                    Message.Show("Veuillez patienter svp,données en cour de chargement", "Info");
                }
            }
            private void Button_Click_2(object sender, RoutedEventArgs e)
            {
                if (lstvariable != null && lstvariable.Count != 0)
                {
                    CsRedevance laRedevSelect = new CsRedevance();
                    Galatee.Silverlight.ServiceAccueil.CsCentre  leCentreSelect = new  Galatee.Silverlight.ServiceAccueil.CsCentre();
                    CsProduit leProduitSelect = new CsProduit();
                    CsRechercheTarif LaRechercheTarifSelect = new CsRechercheTarif();


                    if (this.Txt_CodeRedevence.Tag != null )
                        laRedevSelect = (CsRedevance)this.Txt_CodeRedevence.Tag;

                     if (this.Txt_CodeCentre .Tag != null )
                        leCentreSelect = (Galatee.Silverlight.ServiceAccueil.CsCentre)this.Txt_CodeCentre.Tag;

                     if (this.Txt_CodeProduit.Tag != null)
                         leProduitSelect = (CsProduit)this.Txt_CodeProduit.Tag;

                     if (this.Txt_CodeRechercheTarif.Tag != null)
                         LaRechercheTarifSelect = (CsRechercheTarif)this.Txt_CodeRechercheTarif.Tag;

                     CsVariableDeTarification laVariable = lstvariable.FirstOrDefault(t => t.FK_IDREDEVANCE == laRedevSelect.PK_ID && t.FK_IDCENTRE == leCentreSelect.PK_ID && t.FK_IDRECHERCHETARIF == LaRechercheTarifSelect.PK_ID);
                     if (laVariable != null)
                         LoadTarifGenerer(laVariable);
                }
            }

            void Newfrm_CallBack(object sender, CustumEventArgs e)
            {
                TarifFacturationoInserte.Add((CsTarifFacturation)e.Bag);
                ListeTarifFacturation.Add((CsTarifFacturation)e.Bag);
                SessionObject.ListeTarifFacturation = ListeTarifFacturation.ToList();
                LoadDatagrid(ListeTarifFacturation.OrderBy(t=>t.CTARCOMP).ToList());
            }
            private void btn_RechercheTarif_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(ListeRechercheTarif.OrderBy(p => p.CODE).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_RechercheTarif_OkClicked);
                    this.IsEnabled = false ;
                    ctr.Show();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            private void galatee_RechercheTarif_OkClicked(object sender, EventArgs e)
            {
                this.IsEnabled = true ;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsRechercheTarif _LaRechercheTarif = (CsRechercheTarif)ctrs.MyObject;
                    this.Txt_CodeRechercheTarif.Text = _LaRechercheTarif.CODE;
                    this.Txt_CodeRechercheTarif.Tag = _LaRechercheTarif;
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
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "Menu");
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
                        this.IsEnabled = false ;

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
                this.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsProduit _Leproduit = (CsProduit)ctrs.MyObject;
                    this.Txt_CodeProduit.Text = _Leproduit.CODE;
                    this.Txt_LibelleProduitRech.Text = _Leproduit.LIBELLE;
                    this.Txt_CodeProduit.Tag = _Leproduit;
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

                            this.Txt_LibelleProduitRech.Text = _LeProduit.LIBELLE;
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "Menu");
                }
            }


            private void btn_Redevence_Click(object sender, RoutedEventArgs e)
            {

                try
                {

                    List<object> _LstObject = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.ListeRedevence.OrderBy(p => p.CODE).ToList());
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "CODE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");
                    _LstColonneAffich.Add("LIBELLEPRODUIT", "PRODUIT");
                   
                    #region Sylla

                    //MainView.UcListeGenerique ctr = new MainView.UcListeGenerique(_LstObject, _LstColonneAffich, false, "Liste");  
                    FrmListeRedevanceFiltre ctr = new FrmListeRedevanceFiltre();
                    //ctr.Closed += new EventHandler(galatee_Redevence_OkClicked);
                    ctr.CallBack += ctr_CallBack;
                    #endregion

                    ctr.Show();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            #region Sylla

            void ctr_CallBack(object sender, CustumEventArgs e)
            {

                FrmListeRedevanceFiltre ctrs = sender as FrmListeRedevanceFiltre;

                if (e.Data != null)
                {
                    CsRedevance _LaRedevance = (CsRedevance)e.Data;
                    this.Txt_CodeRedevence.Text = _LaRedevance.CODE;
                    this.Txt_CodeRedevence.Tag = _LaRedevance;
                    this.Txt_LibelleRedevence.Text = _LaRedevance.LIBELLE;
                    this.Txt_CodeProduit.IsReadOnly = true;
                    this.Txt_LibelleProduitRech.IsReadOnly = true;
                    this.btn_Produit.IsEnabled = true;
                    this.Txt_LibelleProduitRech.Text = _LaRedevance.LIBELLEPRODUIT;
                    this.Txt_CodeProduit.Text = _LaRedevance.PRODUIT;


                }
            }
            #endregion

            private void galatee_Redevence_OkClicked(object sender, EventArgs e)
            {

                FrmListeRedevanceFiltre ctrs = sender as FrmListeRedevanceFiltre;

                if (ctrs.MyObject!=null)
                {
                    CsRedevance _LaRedevance = (CsRedevance)ctrs.MyObject;
                    this.Txt_CodeRedevence.Text = _LaRedevance.CODE;
                    this.Txt_CodeRedevence.Tag = _LaRedevance;
                    this.Txt_LibelleRedevence.Text = _LaRedevance.LIBELLE;
                    this.Txt_CodeProduit.IsReadOnly = true;
                    this.Txt_LibelleProduitRech.IsReadOnly = true;
                    this.btn_Produit.IsEnabled  = true;
                    this.Txt_LibelleProduitRech.Text = _LaRedevance.LIBELLEPRODUIT;
                    this.Txt_CodeProduit.Text = _LaRedevance.PRODUIT ;

                    
                }
            }

            private void btn_Centre_Click(object sender, RoutedEventArgs e)
            {

                try
                {

                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCentre.OrderBy(p => p.CODE).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClicked);
                    this.IsEnabled = false;
                    ctr.Show();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            void galatee_OkClicked(object sender, EventArgs e)
            {
                this.IsEnabled = true ;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsCentre _LaCateg = (Galatee.Silverlight.ServiceAccueil.CsCentre)ctrs.MyObject;
                    this.Txt_CodeCentre.Text = _LaCateg.CODE;
                    this.Txt_LibelleCentre1.Text = _LaCateg.LIBELLE ;
                    this.Txt_CodeCentre.Tag = _LaCateg;
                }
            }
            private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Txt_CodeCentre.Text) && Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                    {
                        Galatee.Silverlight.ServiceAccueil.CsCentre _LeCentreClient = ClasseMEthodeGenerique.RetourneObjectFromList(LstCentre, this.Txt_CodeCentre.Text, "CODE");
                        if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
                        {
                            this.Txt_LibelleCentre1.Text = _LeCentreClient.LIBELLE;
                            Txt_CodeCentre.Tag = _LeCentreClient.PK_ID;
                        }
                        else
                        {
                            Message.ShowInformation("Centre inexistant dans pour ce site", "Centre");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "Tarif");
                }
            }

            private void btn_Site_Click(object sender, RoutedEventArgs e)
            {
                try
                {

                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(lstSite.OrderBy(p => p.CODE).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkSiteClicked);
                    this.IsEnabled = false;
                    ctr.Show();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            private void galatee_OkSiteClicked(object sender, EventArgs e)
            {
                this.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsSite _LeSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                    this.Txt_CodeSite.Text = _LeSite.CODE;
                    this.Txt_CodeSite.Tag = _LeSite.PK_ID;
                    LstCentre = SessionObject.LstCentre.Where(t => t.FK_IDCODESITE == _LeSite.PK_ID).ToList();
                }
            }
            private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Txt_CodeSite.Text) && Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                    {
                        Galatee.Silverlight.ServiceAccueil.CsSite _LeSiteClient = lstSite.FirstOrDefault(t => t.CODE == this.Txt_CodeSite.Text);
                        if (!string.IsNullOrEmpty(_LeSiteClient.LIBELLE))
                        {
                            this.Txt_LibelleSite.Text = _LeSiteClient.LIBELLE;
                            LstCentre = SessionObject.LstCentre.Where(t => t.FK_IDCODESITE == _LeSiteClient.PK_ID).ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "Tarification");

                }
            }


        #endregion

        #region Services

        #region Load
            List<CsVariableDeTarification> lstvariable = new List<CsVariableDeTarification>();
        public void LoadAllVariableTarif()
        {

            try
            {

                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                service.LoadAllVariableTarifAsync();
                service.LoadAllVariableTarifCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                //SessionObject.ListeVariableTarif = res.Result;
                                lstvariable = res.Result;
                                //foreach (var item in res.Result)
                                //{
                                //    ListeVariableTarif.Add(item);
                                //}
                                //cbo_variable_tarification.DisplayMemberPath = "REDEVANCE_RECHERCHE";
                                //cbo_variable_tarification.SelectedValuePath = "PK_ID";
                                //cbo_variable_tarification.ItemsSource = ListeVariableTarif;
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
        #region Sylla
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
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = _LstSite.First().CODE;
                            this.Txt_LibelleSite.Text = _LstSite.First().LIBELLE;
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
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = _LstSite.First().CODE;
                            this.Txt_LibelleSite.Text = _LstSite.First().LIBELLE;
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
        #endregion
        public void LoadAllRedevance()
        {
            try
            {
                if (SessionObject.ListeRedevence.Count != 0)
                    return;
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
                                SessionObject.ListeRedevence = res.Result;
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
                            {SessionObject.ListeRechercheTarif = res.Result;
                                ListeRechercheTarif=SessionObject.ListeRechercheTarif ;}
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
                if (SessionObject.ListeDesProduit.Count != 0)
                {
                    LstDeProduit = SessionObject.ListeDesProduit;
                    return;
                }
                else
                {
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
               
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerListeDeProduit");
            }
        }
        private void LoadTarifGenerer(CsVariableDeTarification lavariable)
        {
            try
            {
                //string FK_IDRECHERCHETARIF=string.Empty;
                //string PK_ID=string.Empty;
                //if (cbo_variable_tarification.SelectedItem!=null)
                //{
                //    FK_IDRECHERCHETARIF= ((CsVariableDeTarification)cbo_variable_tarification.SelectedItem).FK_IDRECHERCHETARIF.ToString();
                //    PK_ID=((CsVariableDeTarification)cbo_variable_tarification.SelectedItem).PK_ID.ToString();
                //}
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                service.LoadTarifGenererAsync(lavariable.FK_IDRECHERCHETARIF.ToString() ,lavariable. PK_ID.ToString(),lavariable.PRODUIT );
                service.LoadTarifGenererCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                //Code de chargement de la grid pr les ligne de redevence
                                foreach (var item in res.Result)
                                {
                                    ListeTarifFacturation.Add(item);
                                }
                                //TarifFacturationoInserte = res.Result;
                                LoadDatagrid(ListeTarifFacturation.OrderBy(t=>t.CTARCOMP).ToList());
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

     
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void LoadDatagrid(List<CsTarifFacturation> ListeTarifFacturation)
        {
            var datasource = (from t in ListeTarifFacturation
                              group new
                              {
                                  PK_ID = t.PK_ID,
                                  LIBELLECENTRE = t.LIBELLECENTRE,
                                  CENTRE = t.CENTRE,
                                  DATECREATION = t.DATECREATION,
                                  DATEMODIFICATION = t.DATEMODIFICATION,
                                  LIBELLECOMMUNE = t.LIBELLECOMMUNE,
                                  COMMUNE = t.COMMUNE,
                                  USERCREATION = t.USERCREATION,
                                  USERMODIFICATION = t.USERMODIFICATION,
                                  CTARCOMP = t.CTARCOMP,
                                  DEBUTAPPLICATION = t.DEBUTAPPLICATION,
                                  FINAPPLICATION = t.FINAPPLICATION,
                                  FK_IDCENTRE = t.FK_IDCENTRE,
                                  FK_IDPRODUIT = t.FK_IDPRODUIT,
                                  FK_IDTAXE = t.FK_IDTAXE,
                                  FK_IDUNITECOMPTAGE = t.FK_IDUNITECOMPTAGE,
                                  FK_IDVARIABLETARIF = t.FK_IDVARIABLETARIF,
                                  FORFVAL = t.FORFVAL,
                                  MINIVAL = t.MINIVAL,
                                  MINIVOL = t.MINIVOL,
                                  MONTANTANNUEL = t.MONTANTANNUEL,
                                  PERDEB = t.PERDEB,
                                  PERFIN = t.PERFIN,
                                  LIBELLEPRODUIT = t.LIBELLEPRODUIT,
                                  PRODUIT = t.PRODUIT,
                                  LIBELLERECHERCHETARIF = t.LIBELLERECHERCHETARIF,
                                  RECHERCHETARIF = t.RECHERCHETARIF,
                                  LIBELLEREDEVANCE = t.LIBELLEREDEVANCE,
                                  REDEVANCE = t.REDEVANCE,
                                  REGION = t.REGION,
                                  SREGION = t.SREGION,
                                  TAXE = t.TAXE,
                                  UNITE = t.UNITE,
                                  MODEAPPLICATION = t.MODEAPPLICATION
                              } by new
                              {
                                  //PK_ID = t.PK_ID,
                                  //CENTRE = t.CENTRE,
                                  //LIBELLECENTRE=t.LIBELLECENTRE,
                                  //DATECREATION = t.DATECREATION,
                                  //DATEMODIFICATION = t.DATEMODIFICATION,
                                  //COMMUNE = t.COMMUNE,
                                  //LIBELLECOMMUNE=t.LIBELLECOMMUNE,
                                  //USERCREATION = t.USERCREATION,
                                  //USERMODIFICATION = t.USERMODIFICATION,
                                  //CTARCOMP         tey.CTARCOMP          ,
                                  //DEBUTAPPLICATION = t.DEBUTAPPLICATION,
                                  //FINAPPLICATION = t.FINAPPLICATION,
                                  //FK_IDCENTRE = t.FK_IDCENTRE,
                                  //FK_IDPRODUIT = t.FK_IDPRODUIT,
                                  //FK_IDTAXE = t.FK_IDTAXE,
                                  //FK_IDUNITECOMPTAGE = t.FK_IDUNITECOMPTAGE,
                                  //FK_IDVARIABLETARIF = t.FK_IDVARIABLETARIF,
                                  //FORFVAL = t.FORFVAL,
                                  //MINIVAL = t.MINIVAL,
                                  //MINIVOL = t.MINIVOL,
                                  //MONTANTANNUEL = t.MONTANTANNUEL,
                                  //PERDEB = t.PERDEB,
                                  //PERFIN = t.PERFIN,
                                  //PRODUIT = t.PRODUIT,
                                  //LIBELLEPRODUIT=t.LIBELLEPRODUIT,
                                  MODEAPPLICATION = t.MODEAPPLICATION,
                                  RECHERCHETARIF = t.RECHERCHETARIF,
                                  LIBELLERECHERCHETARIF = t.LIBELLERECHERCHETARIF,
                                  REDEVANCE = t.REDEVANCE,
                                  LIBELLEREDEVANCE = t.LIBELLEREDEVANCE

                                  //REGION = t.REGION,
                                  //SREGION = t.SREGION,
                                  //TAXE = t.TAXE,
                                  //UNITE = t.UNITE
                              } into groupetarif
                              select new
                              {
                                  //PK_ID = groupetarif.Key.PK_ID,
                                  //CENTRE = groupetarif.Key.CENTRE,
                                  //LIBELLECENTRE = groupetarif.Key.LIBELLECENTRE,
                                  //DATECREATION = groupetarif.Key.DATECREATION,
                                  //DATEMODIFICATION = groupetarif.Key.DATEMODIFICATION,
                                  //COMMUNE = groupetarif.Key.COMMUNE,
                                  //LIBELLECOMMUNE = groupetarif.Key.LIBELLECOMMUNE,
                                  //USERCREATION = groupetarif.Key.USERCREATION,
                                  //USERMODIFICATION = groupetarif.Key.USERMODIFICATION,
                                  //DEBUTAPPLICATION = groupetarif.Key.DEBUTAPPLICATION,
                                  //FINAPPLICATION = groupetarif.Key.FINAPPLICATION,
                                  //FK_IDCENTRE = groupetarif.Key.FK_IDCENTRE,
                                  //FK_IDPRODUIT = groupetarif.Key.FK_IDPRODUIT,
                                  //FK_IDTAXE = groupetarif.Key.FK_IDTAXE,
                                  //FK_IDUNITECOMPTAGE = groupetarif.Key.FK_IDUNITECOMPTAGE,
                                  //FK_IDVARIABLETARIF = groupetarif.Key.FK_IDVARIABLETARIF,
                                  //FORFVAL = groupetarif.Key.FORFVAL,
                                  //MINIVAL = groupetarif.Key.MINIVAL,
                                  //MINIVOL = groupetarif.Key.MINIVOL,
                                  //MONTANTANNUEL = groupetarif.Key.MONTANTANNUEL,
                                  //PERDEB = groupetarif.Key.PERDEB,
                                  //PERFIN = groupetarif.Key.PERFIN,
                                  //PRODUIT = groupetarif.Key.PRODUIT,
                                  //LIBELLEPRODUIT = groupetarif.Key.LIBELLEPRODUIT,
                                  MODEAPPLICATION = groupetarif.Key.MODEAPPLICATION,
                                  RECHERCHETARIF = groupetarif.Key.RECHERCHETARIF,
                                  LIBELLERECHERCHETARIF = groupetarif.Key.LIBELLERECHERCHETARIF,
                                  REDEVANCE = groupetarif.Key.REDEVANCE,
                                  LIBELLEREDEVANCE = groupetarif.Key.LIBELLEREDEVANCE,
                                  DETAIL = groupetarif
                                  //REGION = groupetarif.Key.REGION,
                                  //SREGION = groupetarif.Key.SREGION,
                                  //TAXE = groupetarif.Key.TAXE,
                                  //UNITE = groupetarif.Key.UNITE
                              }
                            ).ToList();
            System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(datasource);
            dgListeTarifFacturation.ItemsSource = view;
            datapager.Source = view;
        }

        #endregion

        #region Save

        public void Save(List<CsTarifFacturation> TarifFacturation)
        {
            try
            {
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                int handler = LoadingManager.BeginLoading("Mise à jour des données ...");
                service.CreateTarifAsync(TarifFacturation, 1);
                service.CreateTarifCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                if (res.Result)
                                {
                                    Message.Show("Sauvegarde effectuée avec succès",
                                    "Info");
                                }
                                else
                                {
                                    Message.Show("Sauvegarde non effectuée avec succès, il se peut que vos modifications n'aient pas été prises en compte",
                                    "Info");
                                }
                            }
                            else
                                Message.Show("Une erreur s'est produite, veuillez consulter le journal des erreurs",
                                    "Erreur");
                        LoadingManager.EndLoading(handler);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Variables

            List<CsProduit> LstDeProduit = new List<CsProduit>();
            List<CsRechercheTarif> ListeRechercheTarif = new List<CsRechercheTarif>();

            #region Sylla 
            List<CsCentre> LstCentre = new List<CsCentre>();

            List<CsSite> lstSite = new List<CsSite>();
            #endregion

            ObservableCollection<CsVariableDeTarification> ListeVariableTarif = new ObservableCollection<CsVariableDeTarification>();
        ObservableCollection<CsTarifFacturation> ListeTarifFacturation = new ObservableCollection<CsTarifFacturation>();
        public List<CsTarifFacturation> TarifFacturationoInserte = new List<CsTarifFacturation>();

        #endregion


       




    }
}


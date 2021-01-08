using Galatee.Silverlight.Rpnt.Helper;
//using Galatee.Silverlight.ServiceRpnt;
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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class AffecteClientCampagne : ChildWindow
    {
        #region Variable
        private ObservableCollection<Galatee.Silverlight.ServiceRecouvrement.CsClient> ListeClientEligible = new ObservableCollection<Galatee.Silverlight.ServiceRecouvrement.CsClient>();
        int FkCentre=-1;


        public delegate void BranchementClientEventHandler(object sender, BranchementClientEventArgs e);
        public event BranchementClientEventHandler MethodeAbonnee;


        protected virtual void OnEvent(BranchementClientEventArgs e)
        {
            if (MethodeAbonnee != null)
                MethodeAbonnee(this, e);
        }
        #endregion

        #region Constructeur
        
        public AffecteClientCampagne()
        {
            InitializeComponent();
            Decharger.Content = "<<";
            LoadUI();
        }
        public AffecteClientCampagne(int FkCentre_=-1)
        {
            InitializeComponent();
            Decharger.Content = "<<";
            if (FkCentre_ > -1)
            {
                this.FkCentre = FkCentre_;
                Cbo_Centre.IsEnabled = false;
                //Cbo_Site.IsEnabled = false;
            }
            else
            {
                //Cbo_Centre.IsEnabled = true;
                //Cbo_Site.IsEnabled = true;

            }
            LoadUI();
        }
        public AffecteClientCampagne(ObservableCollection<Galatee.Silverlight.ServiceAccueil.CsCentre> ListExploitation, string CODECENTRE,int FkCentre=-1)
        {
            InitializeComponent();
            Decharger.Content = "<<";
            if (FkCentre >-1)
            {
                this.FkCentre = FkCentre;
                //Cbo_Centre.IsEnabled = false;
                //Cbo_Site.IsEnabled = false;
            }
            else
            {
                //Cbo_Centre.IsEnabled = true;
                //Cbo_Site.IsEnabled = true;
            }
            LoadUI();
        }

        #endregion

        #region Services


            private void LoadMethodeDetection()
            {
                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                int handler = LoadingManager.BeginLoading("Recuperation des factures ...");
                service.GetMethodeDetectionBTAAsync();
                service.GetMethodeDetectionBTACompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                cbxmethrech.ItemsSource = res.Result;
                                cbxmethrech.DisplayMemberPath = "Libele_Methode";
                                cbxmethrech.SelectedValuePath = "Methode_ID";
                            }
                            else
                                Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                    "Erreur");

                        LoadingManager.EndLoading(handler);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                };
            }
            //private void LoadListCentre()
            //{
            //    cbxexploitaition.ItemsSource = SessionObject.LstCentre;
            //    cbxexploitaition.DisplayMemberPath = "LIBELLE";
            //    cbxexploitaition.SelectedValuePath = "CODE";

            //    cbxexploitaition.SelectedItem = SessionObject.LstCentre.FirstOrDefault(c => c.PK_ID == this.FkCentre);

            //}

            void LoadListCentre()
            {
                try
                {

                    if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                    {
                        lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                        lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                        _listeDesCentreExistant = lesCentre;
                        RemplirCentrePerimetre(lesCentre, lesSite);
                        return;
                    }
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ListeDesDonneesDesSiteAsync(false);
                    service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                    {
                        try
                        {
                            if (args != null && args.Cancelled)
                                return;
                            SessionObject.LstCentre = args.Result;
                            if (SessionObject.LstCentre.Count != 0)
                            {
                                lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                                lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                                _listeDesCentreExistant = lesCentre;
                                RemplirCentrePerimetre(lesCentre, lesSite);
                            }
                            else
                            {
                                Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex, "Erreur");
                        }
                    };
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }
            }

            private void RemplirCentrePerimetre(List<Galatee.Silverlight.ServiceAccueil.CsCentre> lstCentre, List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite)
            {
                try
                {
                    Cbo_Centre.Items.Clear();
                    if (_listeDesCentreExistant != null &&
                        _listeDesCentreExistant.Count != 0)
                    {

                        var LeCentre = lstCentre.FirstOrDefault(c => c.PK_ID == this.FkCentre);
                        //if (lstSite != null)
                        //    foreach (var item in lstSite)
                        //    {
                        //        Cbo_Site.Items.Add(item);
                        //    }
                        //Cbo_Site.SelectedValuePath = "PK_ID";
                        //Cbo_Site.DisplayMemberPath = "LIBELLE";

                        //if (lstSite != null && lstSite.Count == 1)
                        //    Cbo_Site.SelectedItem = lstSite.FirstOrDefault(c=>c.PK_ID==LeCentre.FK_IDCODESITE);


                        if (lstCentre != null)
                            foreach (var item in lstCentre)
                            {
                                Cbo_Centre.Items.Add(item);
                            }
                        Cbo_Centre.SelectedValuePath = "PK_ID";
                        Cbo_Centre.DisplayMemberPath = "LIBELLE";
                        Cbo_Centre.SelectedItem = LeCentre;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            private void RemplirCentreDuSite(int pIdSite, int pIdcentre)
            {
                try
                {
                    Cbo_Centre.Items.Clear();
                    if (_listeDesCentreExistant != null &&
                        _listeDesCentreExistant.Count != 0)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsCentre> lesCentreDuPerimetreAction = _listeDesCentreExistant.Where(p => p.FK_IDCODESITE == pIdSite).ToList();
                        if (lesCentreDuPerimetreAction != null)
                            foreach (var item in lesCentreDuPerimetreAction)
                            {
                                Cbo_Centre.Items.Add(item);
                            }
                        //Cbo_Centre.ItemsSource = lesCentreDuPerimetreAction;
                        Cbo_Centre.SelectedValuePath = "PK_ID";
                        Cbo_Centre.DisplayMemberPath = "LIBELLE";

                        if (pIdcentre != 0)
                            this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First(t => t.PK_ID == pIdcentre);
                        if (_listeDesCentreExistant.Count == 1)
                            this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            public void LoadTypeClient()
            {
                List<Galatee.Silverlight.ServiceAccueil.CsCategorieClient> ListeCategClient = new List<Galatee.Silverlight.ServiceAccueil.CsCategorieClient>();
                if (SessionObject.LstCategorie.Count > 0)
                {
                    ListeCategClient = SessionObject.LstCategorie;
                    cbxtypeclient.DisplayMemberPath = "LIBELLE";
                    cbxtypeclient.ItemsSource = ListeCategClient;
                }
                else
                {
                    //RpntServiceClient Service = new RpntServiceClient();
                    Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    int handler = LoadingManager.BeginLoading("Recuperation des type de client ...");
                    service.GetTypeClientAsync();
                    service.GetTypeClientCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {

                                    foreach (var item in res.Result)
                                    {
                                        SessionObject.LstCategorie.Add(new Galatee.Silverlight.ServiceAccueil.CsCategorieClient
                                        {
                                            DATECREATION = item.DATECREATION,
                                            DATEMODIFICATION = item.DATEMODIFICATION,
                                            LIBELLE = item.LIBELLE,
                                            CODE = item.CODE,
                                            PK_ID = item.PK_ID,
                                            USERCREATION = item.USERCREATION,
                                            USERMODIFICATION = item.USERMODIFICATION
                                        });

                                    }
                                    ListeCategClient = SessionObject.LstCategorie;
                                    cbxtypeclient.DisplayMemberPath = "LIBELLE";
                                    cbxtypeclient.ItemsSource = ListeCategClient;
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
                cbxtypeclient.DisplayMemberPath = "LIBELLE";
                cbxtypeclient.ItemsSource = ListeCategClient;
            }
            public void LoadTypeTarif()
            {
                List<Galatee.Silverlight.ServiceRecouvrement.CsTypeTarif> ListeTarif = new List<Galatee.Silverlight.ServiceRecouvrement.CsTypeTarif>();
               
                    //RpntServiceClient Service = new RpntServiceClient();
                    Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    int handler = LoadingManager.BeginLoading("Recuperation des Types Tarif ...");
                    service.GetTypeTarifAsync();
                    service.GetTypeTarifCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                            {
                                if (res.Result != null)
                                {
                                    ListeTarif = res.Result;
                                    cbxtarif.DisplayMemberPath = "LIBELLE";
                                    cbxtarif.ItemsSource = ListeTarif;
                                }
                                else
                                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                        "Erreur");

                                LoadingManager.EndLoading(handler);
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    };
                
                cbxtarif.DisplayMemberPath = "LIBELLE";
                cbxtarif.ItemsSource = ListeTarif;
            }
            public void LoadAgentZont()
            {
                List<Galatee.Silverlight.ServiceRecouvrement.CsReleveur> ListeReleveur = new List<Galatee.Silverlight.ServiceRecouvrement.CsReleveur>();
               
                    //RpntServiceClient Service = new RpntServiceClient();
                    Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    int handler = LoadingManager.BeginLoading("Recuperation des Agents Zone ...");
                    service.GetAgentZontAsync();
                    service.GetAgentZontCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {
                                    ListeReleveur = res.Result;
                                    cbxagetzone.DisplayMemberPath = "NOMRELEVEUR";
                                    cbxagetzone.ItemsSource = ListeReleveur;
                                }
                                else
                                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                        "Erreur");

                            LoadingManager.EndLoading(handler);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    };
                cbxagetzone.DisplayMemberPath = "MATRICULE";
                cbxagetzone.ItemsSource = ListeReleveur;

            }
            public void LoadGroupeFacture()
            {
                List<Galatee.Silverlight.ServiceRecouvrement.CsGroupeDeFacturation> ListeGroupefacturation = new List<Galatee.Silverlight.ServiceRecouvrement.CsGroupeDeFacturation>();
               
                    //RpntServiceClient Service = new RpntServiceClient();
                    Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    int handler = LoadingManager.BeginLoading("Recuperation des Groupes Facture ...");
                    service.GetGroupeFactureAsync();
                    service.GetGroupeFactureCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {
                                    ListeGroupefacturation = res.Result;
                                    //cbxgropfacture.DisplayMemberPath = "Libelle";
                                    //cbxgropfacture.ItemsSource = ListeGroupefacturation;
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
                //cbxgropfacture.DisplayMemberPath = "Libelle";
                //cbxgropfacture.ItemsSource = ListeGroupefacturation;
            }
            //private void FillMonth()
            //{
            //    try
            //    {
            //        List<string> ListMois = new List<string>();
            //        if (SessionObject.periode.Count() > 0)
            //        {
            //            ListMois = SessionObject.periode;
            //            var listeMois = ListMois;
            //            cbxperiodedepart.ItemsSource = listeMois;
            //            //cbxperiodedepart.DisplayMemberPath = "PERIODE";
            //        }
            //        else
            //        {
            //            int loaderHandler = LoadingManager.BeginLoading("Please Wait for pass payment ... ");

            //            cbxperiodedepart.Items.Clear();
            //            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            //            service.GetMoisAsync();
            //            service.GetMoisCompleted += (er, res) =>
            //            {
            //                try
            //                {
            //                    if (res.Error != null || res.Cancelled)
            //                    {
            //                        LoadingManager.EndLoading(loaderHandler);
            //                        throw new Exception("Cannot display report");
            //                    }

            //                    if (res.Result != null)
            //                    {
            //                        SessionObject.periode = res.Result;
            //                        ListMois = SessionObject.periode;
            //                        var listeMois = ListMois;
            //                        cbxperiodedepart.ItemsSource = listeMois;
            //                        //cbxperiodedepart.DisplayMemberPath = "PERIODE";
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    MessageBox.Show(ex.Message);
            //                }
            //                finally
            //                {
            //                    LoadingManager.EndLoading(loaderHandler);
            //                }
            //            };
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}
            public void LoadTypeCompteur()
            {
                List<Galatee.Silverlight.ServiceRecouvrement.CsTcompteur> ListeTcompt = new List<Galatee.Silverlight.ServiceRecouvrement.CsTcompteur>();

               
                    //RpntServiceClient Service = new RpntServiceClient();
                    Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    int handler = LoadingManager.BeginLoading("Recuperation des Types Compteur ...");
                    service.GetTypeCompteurAsync();
                    service.GetTypeCompteurCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {
                                    ListeTcompt = res.Result;
                                    cbxcompteur.DisplayMemberPath = "LIBELLE";
                                    cbxcompteur.ItemsSource = ListeTcompt;
                                }
                                else
                                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                        "Erreur");

                            LoadingManager.EndLoading(handler);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    };
                cbxcompteur.DisplayMemberPath = "LIBELLE";
                cbxcompteur.ItemsSource = ListeTcompt;
            }

        #endregion

        #region Méthode

            private void LoadUI()
            {
                LoadListCentre();
                LoadMethodeDetection();
                LoadTypeClient();
                LoadTypeTarif();
                LoadAgentZont();
                LoadGroupeFacture();
                //FillMonth();
                LoadTypeCompteur();
            }

            public void LoadClientEligible()
            {
                string CodeTypeClient = "";
                string CodeTypeTarif = "";
                string CodeTypeCompteur = "";
                string CodeAgentZone = "";
                string CodeGroupe = "";
                string MoisDepart = "";
                string CodeCentre = "";
                string CodeMethode = "";
                int FkiCentre = 0;

                try
                {
                    if (cbxmethrech.SelectedItem != null)
                    {
                        CodeMethode = ((Galatee.Silverlight.ServiceRecouvrement.CsRefMethodesDeDetectionClientsBTA)cbxmethrech.SelectedItem).Methode_ID.ToString();
                    }
                    else
                    {
                        Message.Show("Veuillez choisir une méthode de détection", "Info");
                        return;
                    }
                    if (Cbo_Centre.SelectedItem != null)
                    {
                        CodeCentre = ((Galatee.Silverlight.ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).CODE;
                        FkiCentre = ((Galatee.Silverlight.ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).PK_ID;

                    }
                    else
                    {
                        Message.Show("Veuillez choisir un centre", "Info");
                        return;
                    }
                    if (cbxtypeclient.SelectedItem != null)
                    {
                        CodeTypeClient = ((Galatee.Silverlight.ServiceAccueil.CsCategorieClient)cbxtypeclient.SelectedItem).CODE.ToString();
                    }
                    if (cbxtarif.SelectedItem != null)
                    {
                        CodeTypeTarif = ((Galatee.Silverlight.ServiceRecouvrement.CsTypeTarif )cbxtarif.SelectedItem).PK_ID.ToString();
                    }
                 
                    if (!string.IsNullOrEmpty(txt_Periode.Text ))
                    {
                        MoisDepart = this.txt_Periode.Text;
                    }
                    else
                    {
                        Message.Show("Veuillez choisir une periode ", "Info");
                        return;
                    }

                    if (cbxagetzone.SelectedItem != null)
                    {
                        CodeAgentZone = ((Galatee.Silverlight.ServiceRecouvrement.CsReleveur)cbxagetzone.SelectedItem).MATRICULE;
                    }
                    if (cbxcompteur.SelectedItem != null)
                    {
                        CodeTypeCompteur = ((Galatee.Silverlight.ServiceRecouvrement.CsTcompteur)cbxcompteur.SelectedItem).PK_ID.ToString();
                    }

                    //RpntServiceClient Service = new RpntServiceClient();
                    Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    int handler = LoadingManager.BeginLoading("Recuperation des données ...");
                    service.GetClientEligibleAsync(CodeMethode, FkiCentre, CodeTypeClient, CodeTypeTarif, CodeTypeCompteur, CodeAgentZone, CodeGroupe, MoisDepart);
                    service.GetClientEligibleCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {


                                    foreach (var item in res.Result)
                                    {
                                        ListeClientEligible.Add(item);
                                    }
                                    dgClientEligible.ItemsSource = null;
                                    dgClientEligible.ItemsSource = ListeClientEligible;
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
                    Message.Show(ex.Message, "Erreur");
                }


            }

        #endregion

        #region Event Handler


            private void Cbo_Site_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                try
                {
                    //if (this.Cbo_Site.SelectedItem != null)
                    //{
                    //    var csSite = Cbo_Site.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsSite;
                    //    if (csSite != null)
                    //    {
                    //        this.txtSite.Text = csSite.CODE ?? string.Empty;
                            
                    //            RemplirCentreDuSite(csSite.PK_ID, 0);
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                }
            }
            private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                try
                {
                    if (this.Cbo_Centre.SelectedItem != null)
                    {
                        Galatee.Silverlight.ServiceAccueil.CsCentre centre = Cbo_Centre.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsCentre;
                        if (centre != null)
                        {
                            this.txtCentre.Text = centre.CODE ?? string.Empty;
                            this.txtCentre.Tag = centre.PK_ID;
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                }
            }

            private void OKButton_Click(object sender, RoutedEventArgs e)
            {
                if (dgclientselectionne.ItemsSource != null)
                {
                    if (((List<Galatee.Silverlight.ServiceRecouvrement.CsClient>)dgclientselectionne.ItemsSource).Count > 0)
                    {
                        this.DialogResult = true;
                        List<Galatee.Silverlight.ServiceRecouvrement.CsClient> ListeClientEligibleSellection = new List<Galatee.Silverlight.ServiceRecouvrement.CsClient>();
                        //Charger info Client dans objet BranchementClientEventArgs
                        BranchementClientEventArgs enventarg = new BranchementClientEventArgs();
                        foreach (var item in dgclientselectionne.ItemsSource)
                        {
                            ListeClientEligibleSellection.Add((Galatee.Silverlight.ServiceRecouvrement.CsClient)item);
                        }
                        enventarg.ListeClientEligibleSellection = ListeClientEligibleSellection;
                        enventarg.MethodeDetection = cbxmethrech.SelectedItem;
                        enventarg.PeriodeDepart = string.IsNullOrEmpty(txt_Periode.Text) != null ? txt_Periode.Text : "";
                        //Executer OnEvent avec objet BranchementClientEventArgs précédamant créer
                        OnEvent(enventarg);
                    }
                }
            }
            private void CancelButton_Click(object sender, RoutedEventArgs e)
            {
                this.DialogResult = false;
            }
            private void bntLoadClient_Click(object sender, RoutedEventArgs e)
            {
                LoadClientEligible();
            }
            private void Button_Click_1(object sender, RoutedEventArgs e)
            {
                new CommonMethode().Transfertclient<Galatee.Silverlight.ServiceRecouvrement.CsClient>(dgClientEligible, dgclientselectionne);
            }
            private void Button_Click_2(object sender, RoutedEventArgs e)
            {
                new CommonMethode().Transfertclient<Galatee.Silverlight.ServiceRecouvrement.CsClient>(dgclientselectionne, dgClientEligible);
            }

            private void cbxmethrech_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (cbxmethrech.SelectedItem!=null)
                {
                    var methrech = (Galatee.Silverlight.ServiceRecouvrement.CsRefMethodesDeDetectionClientsBTA)cbxmethrech.SelectedItem;

                    dgparam.ItemsSource = methrech.ListParametres;
                }
            }

            private void DataGridDragDropTarget_Drop(object sender, Microsoft.Windows.DragEventArgs e)
            {
                //var data =(List<CsClient>)e.GetData<CsClient>();

                //foreach (var item in data)
                //{
                //    ListeClientEligibleSellection.Add(item);
                //}
                //e.Handled = true;
            }
            private void DataGridDragDropTarget_Drop_2(object sender, Microsoft.Windows.DragEventArgs e)
            {
                //var data = (List<CsClient>)e.GetData<CsClient>();

                //foreach (var item in data)
                //{
                //    ListeClientEligibleSellection.Add(item);
                //}
                //e.Handled = true;
            }

        #endregion

            private void cbxexploitaition_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {

            }



            public List<ServiceAccueil.CsCentre> lesCentre { get; set; }

            public List<ServiceAccueil.CsSite> lesSite { get; set; }

            public List<ServiceAccueil.CsCentre> _listeDesCentreExistant { get; set; }

            private void txtCentre_TextChanged(object sender, TextChangedEventArgs e)
            {

            }
    }
}


using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceParametrage;
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
using Galatee.Workflow;
using Galatee.Silverlight.ServiceAccueil ;
using System.Windows.Media.Imaging;

namespace Galatee.Silverlight.Workflow
{
    public partial class UcWKFDashBoard : UserControl
    {

        #region Membres

        List<CsVwConfigurationWorkflowCentre> _lsConfig;
        List<CsFormulaire> _lsFormulaires;
        List<CsVwDashboardDemande> lsElementsDuDashBoard;
        ObservableCollection<Galatee.Silverlight.ServiceParametrage.CsVwJournalDemande> lsDemandes;
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        bool isAdmin = false;
        int IndexOperation = 0;
        string IDLigne;
        List<Galatee.Silverlight.ServiceParametrage.CsGroupeValidation> GroupeValidationDuUser;
        #endregion

        List<CsOperation> OperationNonLieCentre = new List<CsOperation>();
        public UcWKFDashBoard()
        {
            try
            {
                InitializeComponent();
                Translate();
                //initialisation du timer
                //timer.Interval = new TimeSpan(0, 10, 0);
                timer.Interval = new TimeSpan(1, 0, 0);
                timer.Tick += timer_Tick;
                ChargerListDesCentreHabiliter();
                InitialiseDonneesGuichet();
                GetOperation();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord ");
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            if (SessionObject.IsChargerDashbord)
                GetData();
            else
                timer.Start();

        }

        public void RechargerDashboard()
        {
            BuildDashBoard();
        }

        List<int> CentreHabiliter = new List<int>();
        void ChargerListDesCentreHabiliter()
        {
            List<ServiceAccueil.CsCentre> LstCentre = new List<ServiceAccueil.CsCentre>();
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstCentre = args.Result;
                        SessionObject.LstCentre = LstCentre;
                        if (LstCentre.Count != 0)
                        {
                            SessionObject.ModuleEnCours = "Accueil";
                            List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(LstCentre, UserConnecte.listeProfilUser);
                            foreach (var item in lesCentre)
                                CentreHabiliter.Add(item.PK_ID);
                            GetData();
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
                service.ListeDesDonneesDesSiteAsync(false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        #region Fonctions

        void Translate()
        {

        }

        void GetData()
        {
            wrapPnl.Children.Clear();
            if (SessionObject.LstCentre.Count != 0)
            {
                if (CentreHabiliter != null && CentreHabiliter.Count != 0) CentreHabiliter.Clear();
                SessionObject.ModuleEnCours = "Accueil";
                List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre (SessionObject.LstCentre, UserConnecte.listeProfilUser);
                foreach (var item in lesCentre)
                    CentreHabiliter.Add(item.PK_ID);
            }
            //Récupération des configurations en fonction du centre de l'utilisateur connecté
            isAdmin = (UserConnecte.nomUtilisateur.ToLower() == "administrateur" || UserConnecte.nomUtilisateur.ToLower() == "administrator"
                || UserConnecte.nomUtilisateur.ToLower() == "admin");

            int back = LoadingManager.BeginLoading("Chargement du tableau de bord");
            ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Parametrage"));
            if (isAdmin)
            {
                client.SelectVwDashBoardDemandeCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.ListeCodePoste);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    //Si tout est bon je crée des boutons, juste pour tester, dans le WrapPanel
                    lsElementsDuDashBoard = new List<CsVwDashboardDemande>();
                    lsElementsDuDashBoard = args.Result;

                    BuildDashBoard();

                    LoadingManager.EndLoading(back);
                };
                client.SelectVwDashBoardDemandeAsync(CentreHabiliter,UserConnecte.PK_ID );
            }
            else
            {
                //Affichage selon le groupe

                    //1- On recupère les groupes de validation de l'utilisateur
                    client.SelectGroupeValidationByUserIdCompleted += (gsender, gargs) =>
                    {
                        if (gargs.Cancelled || gargs.Error != null)
                        {
                            string error = gargs.Error.Message;
                            Message.Show(error, Languages.ListeCodePoste);
                            return;
                        }
                        if (gargs.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }
                        List<Galatee.Silverlight.ServiceParametrage.CsGroupeValidation> groupes = gargs.Result;
                        GroupeValidationDuUser = new List<Galatee.Silverlight.ServiceParametrage.CsGroupeValidation>();
                        GroupeValidationDuUser = groupes;
                        client.SelectVwDashBoardDemandeCompleted += (dsender, dargs) =>
                        {
                            LoadingManager.EndLoading(back);
                            if (dargs.Cancelled || dargs.Error != null)
                            {
                                string error = dargs.Error.Message;
                                Message.Show(error, Languages.ListeCodePoste);
                                return;
                            }
                            if (dargs.Result == null)
                            {
                                Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                                return;
                            }
                            lsElementsDuDashBoard = new List<CsVwDashboardDemande>();
                            lsElementsDuDashBoard = dargs.Result;
                            BuildDashBoard();
                        };
                        client.SelectVwDashBoardDemandeAsync(CentreHabiliter ,UserConnecte.PK_ID );
                    };
                    client.SelectGroupeValidationByUserIdAsync(UserConnecte.PK_ID);
            }
        }

        void GetOperation()
        {
            ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            client.SelectAllOperationCompleted += (dsender, dargs) =>
            {
                if (dargs.Cancelled || dargs.Error != null)
                {
                    string error = dargs.Error.Message;
                    Message.Show(error, Languages.ListeCodePoste);
                    return;
                }
                if (dargs.Result == null)
                {
                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                    return;
                }
                SessionObject.ListeDesOperation = dargs.Result;
                OperationNonLieCentre = SessionObject.ListeDesOperation.Where(t => SessionObject.TypeOperationNonLieAuCentre().Contains(t.CODE)).ToList();
            };
            client.SelectAllOperationAsync();
        }
 
        
        void BuildDashBoard()
        {
            try
            {
                if (null != lsElementsDuDashBoard && lsElementsDuDashBoard.Count > 0)
                {
                    if (!SessionObject.IsChargerDashbord)
                        return;
                    wrapPnl.Children.Clear();
                    var lsOpérationsNom = lsElementsDuDashBoard.OrderBy(d => d.NOMOPERATION).Select(d => d.NOMOPERATION).Distinct();

                    MyTab.Items.Clear();
                    if (MyTab.Visibility == Visibility.Collapsed)
                        MyTab.Visibility = Visibility.Visible;
                    
                    foreach (string strOperation in lsOpérationsNom)
                        LoadDashBoardInTabControl(strOperation);
                    
                    //MyTab.SelectedIndex = IndexOperation;
                    #region Commenté
                    //var lsOpérations = lsElementsDuDashBoard.Select(d => d.IDCIRCUIT).Distinct();                

                    //foreach (Guid rWorkflowCentre in lsOpérations)
                    //{
                    //    var lsEtapes = lsElementsDuDashBoard.Where(d => d.IDCIRCUIT == rWorkflowCentre)
                    //        .ToList()
                    //        .OrderBy(d => d.ORDRE);

                    //    List<CsVwDashboardInfo> Dashboard = new List<CsVwDashboardInfo>();

                    //    if (null != lsEtapes && lsEtapes.Count() > 0)
                    //    {
                    //        StackPanel stackPnl = new StackPanel();
                    //        Label lbl = new Label();
                    //        lbl.Content = lsEtapes.First().NOMOPERATION;
                    //        lbl.Margin = new Thickness(10);
                    //        lbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    //        lbl.Height = 28;
                    //        int sommeDemande = lsEtapes.Sum(d => (d.NOMBREDEMANDE.HasValue) ? d.NOMBREDEMANDE.Value : 0);
                    //        lbl.Content += " (" + sommeDemande + ")";

                    //        DataGrid dtGrid = new DataGrid();
                    //        dtGrid.Height = 142;
                    //        dtGrid.Margin = new Thickness(10, 5, 10, 5);
                    //        dtGrid.AutoGenerateColumns = false;
                    //        dtGrid.SelectionMode = DataGridSelectionMode.Single;

                    //        DataGridTextColumn coltext = new DataGridTextColumn();
                    //        coltext.Binding = new System.Windows.Data.Binding("DashBoard.NOMBREDEMANDE");
                    //        coltext.Header = "Demandes";
                    //        coltext.Width = new DataGridLength(60, DataGridLengthUnitType.Auto);

                    //        DataGridTemplateColumn colTemplate = new DataGridTemplateColumn();
                    //        colTemplate.CellTemplate = (DataTemplate)Resources["HPLink"];
                    //        colTemplate.Header = "Etapes";
                    //        colTemplate.Width = new DataGridLength(60, DataGridLengthUnitType.Auto);

                    //        dtGrid.Columns.Add(colTemplate);
                    //        dtGrid.Columns.Add(coltext);

                    //        //Avant de générer les infos
                    //        lsEtapes.ToList().ForEach((CsVwDashboardDemande dashbdmd) =>
                    //        {
                    //            Dashboard.Add(new CsVwDashboardInfo()
                    //            {
                    //                DashBoard = dashbdmd,
                    //                COMBINAISON_FKETAPE_FKOPERATION = dashbdmd.FK_IDETAPE.ToString() + ":" +
                    //                    dashbdmd.FK_IDOPERATION.ToString()
                    //            });
                    //        });

                    //        dtGrid.ItemsSource = (from info in Dashboard
                    //                              select info);

                    //        stackPnl.Height = 200;
                    //        stackPnl.Width = 364;
                    //        stackPnl.Margin = new Thickness(20);
                    //        stackPnl.Background = new SolidColorBrush(Color.FromArgb(100, 200, 255, 208));
                    //        stackPnl.Orientation = Orientation.Vertical;



                    //        stackPnl.Children.Add(lbl);
                    //        stackPnl.Children.Add(dtGrid);

                    //        wrapPnl.Children.Add(stackPnl);
                    //    }
                    //}
                    #endregion
                }

                //ON relance le timer
                timer.Start();
            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur au chargement de tableau de bord", "DashBord");
            }
        }

        private void LoadDashBoard(string strOperation)
        {
            try
            {
                var lsEtapes = lsElementsDuDashBoard.Where(d => d.NOMOPERATION == strOperation)
                      .Distinct(new EtapeEqualityComparer())
                      .OrderBy(d => d.ORDRE)
                      .ToList();

                List<CsVwDashboardInfo> DashBoard = new List<CsVwDashboardInfo>();
                if (null != lsEtapes && lsEtapes.Count() > 0)
                {
                    StackPanel stackPnl = new StackPanel();
                    Label lbl = new Label();
                    lbl.Content = lsEtapes.First().NOMOPERATION;
                    lbl.Margin = new Thickness(10);
                    lbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    lbl.Height = 28;
                    int sommeDemande = lsElementsDuDashBoard.Where(c => c.NOMOPERATION == strOperation)
                        .Sum(d => (d.NOMBREDEMANDE.HasValue) ? d.NOMBREDEMANDE.Value : 0);
                    lbl.Content += " (" + sommeDemande + ")";

                    DataGrid dtGrid = new DataGrid();
                    dtGrid.Height = 142;
                    dtGrid.Margin = new Thickness(10, 5, 10, 5);
                    dtGrid.AutoGenerateColumns = false;
                    dtGrid.SelectionMode = DataGridSelectionMode.Single;

                    DataGridTextColumn coltext = new DataGridTextColumn();
                    coltext.Binding = new System.Windows.Data.Binding("DashBoard.NOMBREDEMANDE");
                    coltext.Header = "Nombres";
                    coltext.Width = new DataGridLength(60, DataGridLengthUnitType.Auto);

                    DataGridTemplateColumn colTemplate = new DataGridTemplateColumn();
                    colTemplate.CellTemplate = (DataTemplate)Resources["HPLink"];
                    colTemplate.Header = "Etapes";
                    colTemplate.Width = new DataGridLength(60, DataGridLengthUnitType.Auto);

                    dtGrid.Columns.Add(colTemplate);
                    dtGrid.Columns.Add(coltext);

                    //Avant de générer les infos
                    lsEtapes.ToList().ForEach((CsVwDashboardDemande dashbdmd) =>
                    {
                        dashbdmd.NOMBREDEMANDE = lsElementsDuDashBoard.Where(e => e.NOMOPERATION == dashbdmd.NOMOPERATION
                            && e.FK_IDETAPE == dashbdmd.FK_IDETAPE)
                            .ToList()
                            .Sum(e => (e.NOMBREDEMANDE.HasValue) ? e.NOMBREDEMANDE.Value : 0);
                        DashBoard.Add(new CsVwDashboardInfo()
                        {
                            DashBoard = dashbdmd,
                            COMBINAISON_FKETAPE_FKOPERATION = dashbdmd.FK_IDETAPE.ToString() + ":" +
                                dashbdmd.FK_IDOPERATION.ToString()
                        });
                    });

                    dtGrid.ItemsSource = (from info in DashBoard
                                          select info);

                    stackPnl.Height = 200;
                    stackPnl.MaxWidth = 300;
                    stackPnl.Margin = new Thickness(20);
                    stackPnl.Background = new SolidColorBrush(Color.FromArgb(100, 200, 255, 208));
                    stackPnl.Orientation = Orientation.Vertical;



                    stackPnl.Children.Add(lbl);
                    stackPnl.Children.Add(dtGrid);

                    wrapPnl.Children.Add(stackPnl);
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur de chargement", "Tableau de bord");
            }
        }


        private void LoadDashBoardInTabControl(string strOperation)
        {

            try
            {
  

                var lsEtapes = lsElementsDuDashBoard.Where(d => d.NOMOPERATION == strOperation)
                      .Distinct(new EtapeEqualityComparer())
                      .OrderBy(d => d.ORDRE)
                      .ToList();

                List<CsVwDashboardInfo> DashBoard = new List<CsVwDashboardInfo>();
                if (null != lsEtapes && lsEtapes.Count() > 0)
                {
                    StackPanel stackPnl = new StackPanel();
                    Label lbl = new Label();
                    Color Background_ = ColorFromString("#FFFF6600");

                    lbl.Content = lsEtapes.First().NOMOPERATION;
                    lbl.Margin = new Thickness(10);
                    lbl.Width = 1200;
                    lbl.Background = new SolidColorBrush(Background_);
                    lbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    lbl.Height = 40;
                    lbl.Padding = new Thickness(10, 0, 0, 0);
                    Background_ = ColorFromString("#FFFFFFFF");
                    lbl.Foreground = new SolidColorBrush(Background_);
                    lbl.FontSize = 20;// taille libelle operation (orange)
                    int sommeDemande = lsElementsDuDashBoard.Where(c => c.NOMOPERATION == strOperation)
                        .Sum(d => (d.NOMBREDEMANDE.HasValue) ? d.NOMBREDEMANDE.Value : 0);
                    lbl.Content += " (" + sommeDemande + ")";

                    DataGrid dtGrid = new DataGrid();
                    dtGrid.Height = 450; // Taille datagrid

                    dtGrid.Margin = new Thickness(10, 5, 10, 5);
                    dtGrid.AutoGenerateColumns = false;
                    dtGrid.SelectionMode = DataGridSelectionMode.Single;

                    DataGridTextColumn coltext = new DataGridTextColumn();
                    coltext.Binding = new System.Windows.Data.Binding("DashBoard.NOMBREDEMANDE");
                    coltext.Header = "Nombres";
                    coltext.Width = new DataGridLength(60, DataGridLengthUnitType.Auto);

                    DataGridTemplateColumn colTemplate = new DataGridTemplateColumn();
                    colTemplate.CellTemplate = (DataTemplate)Resources["HPLink"];
                    colTemplate.Header = "Etapes";
                    colTemplate.Width = new DataGridLength(60, DataGridLengthUnitType.Auto);

                    dtGrid.Columns.Add(colTemplate);
                    dtGrid.Columns.Add(coltext);

                    dtGrid.Columns[0].MinWidth = 250;

                    dtGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

                    //Avant de générer les infos
                    lsEtapes.ToList().ForEach((CsVwDashboardDemande dashbdmd) =>
                    {
                        dashbdmd.NOMBREDEMANDE = lsElementsDuDashBoard.Where(e => e.NOMOPERATION == dashbdmd.NOMOPERATION
                            && e.FK_IDETAPE == dashbdmd.FK_IDETAPE)
                            .ToList()
                            .Sum(e => (e.NOMBREDEMANDE.HasValue) ? e.NOMBREDEMANDE.Value : 0);
                        DashBoard.Add(new CsVwDashboardInfo()
                        {
                            DashBoard = dashbdmd,
                            COMBINAISON_FKETAPE_FKOPERATION = dashbdmd.FK_IDETAPE.ToString() + ":" +
                                dashbdmd.FK_IDOPERATION.ToString()
                        });
                    });

                    dtGrid.ItemsSource = (from info in DashBoard
                                          select info);


                    stackPnl.Height = 525;
                    stackPnl.Width = 700;
                    stackPnl.Margin = new Thickness(5);
                    stackPnl.Background = new SolidColorBrush(Color.FromArgb(100, 200, 255, 208));
                    //stackPnl.Background = new SolidColorBrush(Background);
                    stackPnl.Orientation = Orientation.Vertical;

                    stackPnl.Children.Add(lbl);
                    stackPnl.Children.Add(dtGrid);

                    StackPanel st = new StackPanel();
                    st.Orientation = Orientation.Horizontal;

                    Image img = new Image();
                    img.HorizontalAlignment = HorizontalAlignment.Left;
                    img.Height = 15;

                    img.Width = 10;
                    string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                    int PositionOfClientBin = strBaseWebAddress.ToLower().IndexOf("clientbin");
                    string root = strBaseWebAddress.Substring(0, PositionOfClientBin);

                    img.Source = new BitmapImage(new Uri(root + "/Galatee.ModuleLoader/Image/phone2-36.png", UriKind.Absolute));
                    st.Children.Add(img);

                    TextBlock tb = new TextBlock();
                    tb.Margin = new Thickness(21, 0, 0, 0);

                    tb.VerticalAlignment = VerticalAlignment.Center;
                    tb.HorizontalAlignment = HorizontalAlignment.Right;
                    tb.FontWeight = FontWeights.Normal;

                    tb.Foreground = new SolidColorBrush(Colors.Black);
                    tb.Text = lbl.Content.ToString();
                    st.Children.Add(tb);

                    //Button btnAdd = new Button();
                    //btnAdd.Content = st;

                    CsLibelleTabItemDashBord MyDataContext = new CsLibelleTabItemDashBord();
                    MyDataContext.Operation = lsEtapes.First().NOMOPERATION;
                    MyDataContext.NombreDemande = sommeDemande.ToString();


                    var tabItem = new TabItem();

                    Color Foreground = ColorFromString("#FF130202");
                    tabItem.Foreground = new System.Windows.Media.SolidColorBrush(Foreground);
                    tabItem.Height = 28;
                    tabItem.VerticalAlignment = VerticalAlignment.Center;
                    Color Background = ColorFromString("#FFD4EEF6");
                    tabItem.Background = new System.Windows.Media.SolidColorBrush(Background);
                    tabItem.BorderThickness = new Thickness(0);
                    tabItem.AllowDrop = true;
                    //tabItem.Margin = new Thickness(0, 7, 1, -7);

                    tabItem.Style = App.Current.Resources["tabperso"] as Style;
                    tabItem.FontSize = 10;

                    tabItem.DataContext = MyDataContext;
                    var stackPanel = new StackPanel();

                    Grid TabItemContainte = new Grid { Background = tabItem.Background, Margin = new Thickness(-5, -5, -5, -9) };
                    Grid.SetRow(stackPnl, 1);
                    Grid.SetColumn(stackPnl, 1);
                    TabItemContainte.Children.Add(stackPnl);
                    tabItem.Content = TabItemContainte;

                    MyTab.Items.Add(tabItem);
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur de dashbord", "Dashbord Erreur");
            }
        }



        public static Color ColorFromString(string htmlColor)
        {
            htmlColor = htmlColor.Replace("#", "");
            byte a = 0xff, r = 0, g = 0, b = 0;
            switch (htmlColor.Length)
            {
                case 3:
                    r = byte.Parse(htmlColor.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(htmlColor.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(htmlColor.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                    break;
                case 4:
                    a = byte.Parse(htmlColor.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                    r = byte.Parse(htmlColor.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(htmlColor.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(htmlColor.Substring(3, 1), System.Globalization.NumberStyles.HexNumber);
                    break;
                case 6:
                    r = byte.Parse(htmlColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(htmlColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(htmlColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    break;
                case 8:
                    a = byte.Parse(htmlColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    r = byte.Parse(htmlColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(htmlColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(htmlColor.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                    break;
            }
            return Color.FromArgb(a, r, g, b);
        }


        #endregion

        private void btnCharger_Click(object sender, RoutedEventArgs e)
        {
            //Chargement du tableau de bord
            //if (null != _lsConfig && _lsConfig.Count > 0 && null != cmbConfiguration.SelectedValue)
            //{
            //    Guid laSelection = Guid.Parse(cmbConfiguration.SelectedValue.ToString());

            //    var Config = _lsConfig.Where(cfg => cfg.OPERATIONID == laSelection).FirstOrDefault();
            //    if (null != Config)
            //    {
            //        int back = LoadingManager.BeginLoading("Chargement du Tableau de bord");

            //        ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            //        client.SelectVwDashBoardDemandeByHabilitationCompleted += (ssender, args) =>
            //            {
            //                if (args.Cancelled || args.Error != null)
            //                {
            //                    string error = args.Error.Message;
            //                    Message.Show(error, Languages.ListeCodePoste);
            //                    return;
            //                }
            //                if (args.Result == null)
            //                {
            //                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
            //                    return;
            //                }
            //                //Si tout est bon je crée des boutons, juste pour tester, dans le WrapPanel
            //                lsElementsDuDashBoard = new List<CsVwDashboardDemande>();
            //                lsElementsDuDashBoard = args.Result;

            //                wrapPnl.Children.Clear();
            //                foreach (var dashB in lsElementsDuDashBoard.OrderBy(d => d.ORDRE))
            //                {
            //                    Button btn = new Button();
            //                    btn.Content = dashB.NOM + " (" + dashB.NOMBREDEMANDE + ") ";
            //                    btn.FontSize = 15;
            //                    btn.Width = 150;
            //                    btn.Height = 82;
            //                    btn.Margin = new Thickness(10);
            //                    btn.Tag = dashB;
            //                    btn.Click += btn_Click;

            //                    wrapPnl.Children.Add(btn);
            //                }

            //                LoadingManager.EndLoading(back);
            //            };
            //        client.SelectVwDashBoardDemandeByHabilitationAsync(UserConnecte.FK_IDCENTRE, Config.PK_ID,
            //            GroupeValidationDuUser.Select(g => g.PK_ID).ToList());
            //    }
            //}
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            //Ici on va charger les élements de l'étape
            CsVwDashboardDemande dashB = ((Button)sender).Tag as CsVwDashboardDemande;
            if (dashB != null)
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                int back = LoadingManager.BeginLoading("Chargement des demandes ...");

                client.SelectVwJournalDemandeCompleted += (ssender, args) =>
                {
                    LoadingManager.EndLoading(back);

                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.ListeCodePoste);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }


                    //Si tout est bon on filtre
                    lsDemandes = new ObservableCollection<CsVwJournalDemande>(args.Result.Where(d => d.FK_IDOPERATION == dashB.FK_IDOPERATION && d.FK_IDETAPEACTUELLE == dashB.FK_IDETAPEACTUELLE
                        && d.FK_IDCENTRE == dashB.FK_IDCENTRE && d.IDCIRCUIT == dashB.IDCIRCUIT));

                    //dtgListeAccueil.ItemsSource = lsDemandes;                        
                };

                client.SelectVwJournalDemandeAsync();
            }
        }


        #region Gestion DataGrid

        private void dtgListeAccueil_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //try
            //{
            //    var devisRow = e.Row.DataContext as ObjDEVIS;
            //    if (devisRow != null)
            //    {
            //        if (devisRow.DELAI <= (double)0)
            //        {
            //            if (devisRow.DELAI < (double)0)
            //            {
            //                SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Red);
            //                e.Row.Foreground = SolidColorBrush;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            //}
        }

        private void dtgListeAccueil_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //aTa0 o = lvwResultat.SelectedItem as aTa0;
                string Ucname = string.Empty;
                //    if(int.Parse(o.CODE) > 0 && int.Parse(o.CODE)  < 1000)
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void dtgListeAccueil_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    SessionObject.gridUtilisateur = dtgListeAccueil;
            //    if (dtgListeAccueil.SelectedItem != null)
            //    {
            //        var devis = (ObjDEVIS)this.dtgListeAccueil.SelectedItems[0];
            //        if (devis != null)
            //        {
            //            SessionObject.objectSelected = devis;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            //}
        }

        private void dtgListeAccueil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //try
            //{
            //    ActiverMenuContextuel();
            //}
            //catch (Exception ex)
            //{
            //    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            //}
        }

        private void ActiverMenuContextuel()
        {
            try
            {
                //MenuContextuelModifier.IsEnabled = Btn_Modifier.IsEnabled = (dtgListeAccueil.SelectedItems.Count == 1);
                //MenuContextuelConsulter.IsEnabled = Btn_Consultation.IsEnabled = (dtgListeAccueil.SelectedItems.Count == 1);
                //MenuContextuelCommenter.IsEnabled = Btn_Commenter.IsEnabled = (dtgListeAccueil.SelectedItems.Count == 1);
                //bool enable = true;
                //if (this.dtgListeAccueil.SelectedItems.Count > 0)
                //{
                //    foreach (ObjDEVIS devis in this.dtgListeAccueil.SelectedItems)
                //    {
                //        if (devis.ORIGINE.ToUpper() == Languages.aCorriger.ToUpper())
                //        {
                //            enable = false;
                //            break;
                //        }
                //    }
                //}
                //MenuContextuelTransmettre.IsEnabled = Btn_Transmettre.IsEnabled = enable && (dtgListeAccueil.SelectedItems.Count > 0);
                //MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        private void ViewLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HyperlinkButton Hyp = (HyperlinkButton)sender;
                string FKETAPEOPERATION = Hyp.Tag.ToString();

                //On envoi le nom de l'opération 
                if (null != FKETAPEOPERATION && string.Empty != FKETAPEOPERATION)
                {
                    bool IsTravaux = false;
                    int FKIDETAP = int.Parse(FKETAPEOPERATION.Split(':')[0]);
                    Guid OPERATIONID = Guid.Parse(FKETAPEOPERATION.Split(':')[1]);
                    string opération = lsElementsDuDashBoard.Where(d => d.FK_IDETAPE == FKIDETAP && d.FK_IDOPERATION == OPERATIONID)
                        .Select(d => d.NOMOPERATION)
                        .FirstOrDefault();
                    CsEtape leEtape = SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(i => i.PK_ID == FKIDETAP);
                    if (leEtape != null && !string.IsNullOrEmpty(leEtape.CODE))
                    {
                        if (leEtape.CODE == "SCOMP" || leEtape.CODE == "SMAT")
                            IsTravaux = true;
                    }
                    CsVwDashboardDemande leOperationSelect = lsElementsDuDashBoard.FirstOrDefault(d => d.FK_IDETAPE == FKIDETAP && d.FK_IDOPERATION == OPERATIONID);

                    Galatee.Silverlight.ServiceParametrage.CsGroupeValidation leGroupeValidation = GroupeValidationDuUser.FirstOrDefault(t => t.PK_ID == leOperationSelect.FK_IDGROUPEVALIDATIOIN);

                    List<Guid> lstIdDemande = new List<Guid>();
                    if (null == opération && opération == string.Empty)
                    {
                        if (leGroupeValidation == null || string.IsNullOrEmpty(leGroupeValidation.GROUPENAME))
                            throw new Exception("Le groupe de validation est vide");

                        if (OperationNonLieCentre.Where(t => t.PK_ID == OPERATIONID) != null)
                        {
                            UcWKFListeAutreDemandeEtape lstForm = new UcWKFListeAutreDemandeEtape(leGroupeValidation.ESTCONSULTATION, OPERATIONID, lstIdDemande, leOperationSelect.IS_TRAITEMENT_LOT, leOperationSelect.NOMOPERATION, FKIDETAP);
                            lstForm.Closed += lstForm_Closed;
                            lstForm.Show();
                        }
                        else
                        {
                           
                            UcWKFListeDemandeEtape lstForm = new UcWKFListeDemandeEtape(leGroupeValidation.ESTCONSULTATION, OPERATIONID, leOperationSelect.IS_TRAITEMENT_LOT, leOperationSelect.NOMOPERATION, FKIDETAP);
                            lstForm.Closed += lstForm_Closed;
                            lstForm.Show();
                        }
                    }
                    else
                    {
                        if (IsTravaux)
                        {
                            UcWKFListeDemandeTravaux lstForm = new UcWKFListeDemandeTravaux(leGroupeValidation.ESTCONSULTATION, OPERATIONID, lstIdDemande, leOperationSelect.IS_TRAITEMENT_LOT, leOperationSelect.NOMOPERATION, FKIDETAP, leEtape.CODE);
                            lstForm.Closed += lstForm_Closed;
                            lstForm.Show();
                        }
                        else
                        {
                            CsOperation leOp = OperationNonLieCentre.FirstOrDefault(t => t.PK_ID == OPERATIONID);
                            if (leOp != null)
                            {
                                UcWKFListeAutreDemandeEtape lstForm = new UcWKFListeAutreDemandeEtape(leGroupeValidation.ESTCONSULTATION, OPERATIONID, lstIdDemande, leOperationSelect.IS_TRAITEMENT_LOT, leOperationSelect.NOMOPERATION, FKIDETAP);
                                lstForm.Closed += lstForm_Closed;
                                lstForm.Show();
                            }
                            else
                            {
                                UcWKFListeDemandeEtape lstForm = new UcWKFListeDemandeEtape(leGroupeValidation.ESTCONSULTATION, OPERATIONID, leOperationSelect.IS_TRAITEMENT_LOT, leOperationSelect.NOMOPERATION, FKIDETAP);
                                lstForm.Closed += lstForm_Closed;
                                lstForm.Show();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur", ex.Message);
            }
        }

        void lstForm_Closed(object sender, EventArgs e)
        {
            //On rafraichi tout simplement le tableau de bord
            GetData();
        }
        private void ChargerPuissanceInstalle()
        {
            try
            {
                if (SessionObject.LstPuissanceInstalle != null && SessionObject.LstPuissanceInstalle.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceInstalleCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissanceInstalle = args.Result;
                };
                service.ChargerPuissanceInstalleAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ChargerPuissance()
        {
            try
            {
                if (SessionObject.LstPuissance != null && SessionObject.LstPuissance.Count != 0)
                    return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceSouscriteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissance = args.Result;
                };
                service.ChargerPuissanceSouscriteAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerForfait()
        {
            try
            {
                if (SessionObject.LstForfait != null && SessionObject.LstForfait.Count != 0)
                    return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerForfaitCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                    SessionObject.LstForfait = args.Result;
                };
                service.ChargerForfaitAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerTarif()
        {
            try
            {
                if (SessionObject.LstTarif != null && SessionObject.LstTarif.Count != 0)
                    return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTarifCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTarif = args.Result;
                };
                service.ChargerTarifAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerTarifPuissance()
        {
            try
            {
                if (SessionObject.LstTarifPuissance != null && SessionObject.LstTarifPuissance.Count != 0)
                    return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTarifPuissanceCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTarifPuissance = args.Result;
                };
                service.ChargerTarifPuissanceAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerFrequence()
        {
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerTousFrequenceCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstFrequence = args.Result;
            };
            service.ChargerTousFrequenceAsync();
            service.CloseAsync();
        }

        //WCO le 12/01/2016
        private void ChargerEtapesWorkflow()
        {
            Galatee.Silverlight.ServiceParametrage.ParametrageClient service = new Galatee.Silverlight.ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            service.SelectAllEtapesCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject._ToutesLesEtapesWorkflows = args.Result;
            };
            service.SelectAllEtapesAsync();
            service.CloseAsync();
        }


        private void ChargerMois()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTousMoisCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstMois = args.Result;
                };
                service.ChargerTousMoisAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerApplicationTaxe()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneTousApplicationTaxeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCodeApplicationTaxe = args.Result;
                };
                service.RetourneTousApplicationTaxeAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerLaListeDesCommunes()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCommuneCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    SessionObject.LstCommune = args.Result;
                };
                service.ChargerCommuneAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ChargeQuartier()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesQartiersCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstQuartier = args.Result;
                };
                service.ChargerLesQartiersAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ChargerRubriqueDevis()
        {
            try
            {
                if (SessionObject.LstRubriqueDevis != null && SessionObject.LstRubriqueDevis.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerRubriqueCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstRubriqueDevis = args.Result;
                };
                service.ChargerRubriqueAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargeSecteur()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesSecteursCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstSecteur = args.Result;
                };
                service.ChargerLesSecteursAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargeRue()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesRueDesSecteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstRues = args.Result;
                };
                service.ChargerLesRueDesSecteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerTournee()
        {
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerLesTourneesCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstZone = args.Result;
            };
            service.ChargerLesTourneesAsync();
            service.CloseAsync();
        }

        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                    return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerEtapeDemande()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ChargerEtapeDemandeCompleted += (sr, res) =>
                {
                    try
                    {
                        if (res != null && res.Cancelled)
                            return;
                        SessionObject.LstEtapeDemande = res.Result;
                    }
                    catch (Exception)
                    {
                    }
                };
                service1.ChargerEtapeDemandeAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        private void ChargerTypeDemande()
        {
            try
            {
                if (SessionObject.LstTypeDemande != null && SessionObject.LstTypeDemande.Count != 0)
                    return;

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneOptionDemandeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.LstTypeDemande = res.Result;
                };
                service1.RetourneOptionDemandeAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }
        private void ChargerPuissanceReglageCompteur()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceReglageCompteurCompleted  += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissanceParReglageCompteur = args.Result;
                };
                service.ChargerPuissanceReglageCompteurAsync ();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RetournePosteConnecter()
        {
            if (SessionObject.ListePoste != null && SessionObject.ListePoste.Count != 0)
            {
                CsInfoServiceLocal objectLocal = new CsInfoServiceLocal();
                objectLocal.AdresseMachineLocal = Silverlight.Classes.IsolatedStorage.getMachine();
                if (!string.IsNullOrEmpty(objectLocal.AdresseMachineLocal))
                    SessionObject.LePosteCourant = SessionObject.ListePoste.FirstOrDefault(p => p.NOMPOSTE == objectLocal.AdresseMachineLocal);

                objectLocal.AdresseMachineLocal = SessionObject.LePosteCourant.NOMPOSTE;
                SessionObject.CheminImpression = "[[" + SessionObject.LePosteCourant.NOMPOSTE + "[" + "Impression";
                SessionObject.CheminDocumentScanne = "\\\\" + SessionObject.LePosteCourant.NOMPOSTE + "\\" + "DocumentScane";

                SessionObject.MachineName = SessionObject.LePosteCourant.NOMPOSTE;
                return;
            }

            //Obtenir les donnees de l'arborescence des modules , programmes et des menus relatifs
            Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient prgram = new Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            prgram.RetourneListePosteCompleted += (sprog, resprog) =>
            {
                try
                {
                    if (resprog.Cancelled || resprog.Error != null)
                    {
                        string error = resprog.Error.Message;
                    }
                    if (resprog.Result == null || resprog.Result.Count == 0)
                    {

                    }
                    else
                    {
                        SessionObject.ListePoste = resprog.Result;

                        CsInfoServiceLocal objectLocal = new CsInfoServiceLocal();
                        objectLocal.AdresseMachineLocal = Silverlight.Classes.IsolatedStorage.getMachine();
                        if (!string.IsNullOrEmpty(objectLocal.AdresseMachineLocal))
                            SessionObject.LePosteCourant = SessionObject.ListePoste.FirstOrDefault(p => p.NOMPOSTE == objectLocal.AdresseMachineLocal);

                        objectLocal.AdresseMachineLocal = SessionObject.LePosteCourant.NOMPOSTE;
                        SessionObject.MachineName = SessionObject.LePosteCourant.NOMPOSTE;
                        SessionObject.CheminImpression = "[[" + SessionObject.LePosteCourant.NOMPOSTE + "[" + "Impression";
                        SessionObject.CheminDocumentScanne = "\\\\" + SessionObject.LePosteCourant.NOMPOSTE + "\\" + "DocumentScane";
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "RetournePoste");
                }

            };
            prgram.RetourneListePosteAsync();
        }
        private void ChargerTypeBranchement()
        {
            try
            {
                if (SessionObject.LstTypeBranchement != null && SessionObject.LstTypeBranchement.Count != 0)
                    return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ChargerTypeBranchementAsync();
                service1.ChargerTypeBranchementCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    SessionObject.LstTypeBranchement = args.Result;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerTypeComptage()
        {
            try
            {
                if (SessionObject.LstTypeComptage != null && SessionObject.LstTypeComptage.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeComptageCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTypeComptage = args.Result;
                };
                service.ChargerTypeComptageAsync();
                service.CloseAsync();
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = res.Result;
                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerCategorie()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCategorieCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCategorie = args.Result;
                };
                service.RetourneCategorieAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerFermable()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneFermableCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstFermable = args.Result;
                };
                service.RetourneFermableAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerNatureClient()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneNatureCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstNatureClient = args.Result;
                };
                service.RetourneNatureAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerCivilite()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneListeDenominationAllCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCivilite = args.Result;
                };
                service.RetourneListeDenominationAllAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerNationnalite()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneNationnaliteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstDesNationalites = args.Result;
                };
                service.RetourneNationnaliteAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerCodeRegroupement()
        {
            try
            {
                if (SessionObject.LstCodeRegroupement != null && SessionObject.LstCodeRegroupement.Count != 0)
                    return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCodeRegroupementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCodeRegroupement = args.Result;
                };
                service.RetourneCodeRegroupementAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ChargerCodeConsomateur()
        {
            try
            {
                if (SessionObject.LstCodeConsomateur != null && SessionObject.LstCodeConsomateur.Count != 0) return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCodeConsomateurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCodeConsomateur = args.Result;
                };
                service.RetourneCodeConsomateurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void ChargerReglageCompteur()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerReglageCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstReglageCompteur = args.Result;
                };
                service.ChargerReglageCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerCalibreCompteur()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCalibreCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCalibreCompteur = args.Result;
                };
                service.ChargerCalibreCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerCadran()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneToutCadranCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCadran = args.Result;
                };
                service.RetourneToutCadranAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerMarque()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneToutMarqueCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstMarque = args.Result;
                };
                service.RetourneToutMarqueAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerTypeCompteur()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTypeCompteur = args.Result;
                };
                service.ChargerTypeAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
   
        private void RetouneListeDesTaxes()
        {
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneListeTaxeCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstDesTaxe = args.Result;
            };
            service.RetourneListeTaxeAsync();
            service.CloseAsync();
        }

        private void RetourneParametreDistanceElectricite()
        {
            try
            {
                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.GetParametresDistanceCompleted += (ssender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;

                            Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (args.Result != null)
                        {
                            SessionObject.distanceMaxiElectricite = (decimal)args.Result.Maxi;
                            SessionObject.seuilDistanceElectricite = (decimal)args.Result.Seuil;

                            if (args.Result.MaxiSubvention != null)
                                SessionObject.distanceMaxiSubventionElectricite = (decimal)args.Result.MaxiSubvention;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                    }
                };
                clientDevis.GetParametresDistanceAsync(DataReferenceManager.ParametreDistanceMaxiElectricite, DataReferenceManager.ParametreSeuilDistanceAdditionnelleElectricite, string.Empty);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void RetourneParametreDistanceEau()
        {
            try
            {
                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.GetParametresDistanceCompleted += (ssender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (args.Result != null)
                        {
                            SessionObject.distanceMaxiEau = (decimal)args.Result.Maxi;
                            SessionObject.seuilDistanceEau = (decimal)args.Result.Seuil;
                            if (args.Result.MaxiSubvention != null)
                                SessionObject.distanceMaxiSubventionElectricite = (decimal)args.Result.MaxiSubvention;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                    }
                };
                clientDevis.GetParametresDistanceAsync(DataReferenceManager.ParametreDistanceMaxiEau, DataReferenceManager.ParametreSeuilDistanceAdditionnelleEau, DataReferenceManager.ParametreDistanceMaxiEauSubvention);
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        private void ChargerCoutDemande()
        {
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerCoutDemandeCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstDesCoutDemande = args.Result;
            };
            service.ChargerCoutDemandeAsync();
            service.CloseAsync();
        }
       
        private void RetourneListeDesCas()
        {
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneListeDesCasCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstDesCas = args.Result;
            };
            service.RetourneListeDesCasAsync();
            service.CloseAsync();
        }
        private void ChargerListeDesPostesSource()
        {
            if (SessionObject.LsDesPosteElectriquesSource != null && SessionObject.LsDesPosteElectriquesSource.Count != 0)
            {
                return;
            }

            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerPosteSourceCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    SessionObject.LsDesPosteElectriquesSource = args.Result;
                }
            };
            service.ChargerPosteSourceAsync();
            service.CloseAsync();
        }
        private void RetourneListeDesDepartHta()
        {
            if (SessionObject.LsDesDepartHTA != null && SessionObject.LsDesDepartHTA.Count != 0)
            {
                return;
            }

            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerDepartHTACompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    SessionObject.LsDesDepartHTA = args.Result;
                }
            };
            service.ChargerDepartHTAAsync();
            service.CloseAsync();
        }
        private void ChargerListeDesPostesTransformation()
        {
            if (SessionObject.LsDesPosteElectriquesTransformateur != null && SessionObject.LsDesPosteElectriquesTransformateur.Count != 0)
            {
                return;
            }
            Galatee.Silverlight.ServiceParametrage.ParametrageClient service = new ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            service.SelectAllPosteTransformationCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;

                if (args.Result != null)
                {
                    ServiceAccueil.CsPosteTransformation poste;
                    SessionObject.LsDesPosteElectriquesTransformateur.Clear();
                    foreach (var item in args.Result)
                    {
                        poste = new ServiceAccueil.CsPosteTransformation();
                        poste.CODE = item.CODE;
                        poste.CODEDEPARTHTA = item.CODEDEPARTHTA;
                        poste.DATECREATION = item.DATECREATION;
                        poste.DATEMODIFICATION = item.DATEMODIFICATION;
                        poste.FK_IDDEPARTHTA = item.FK_IDDEPARTHTA;
                        poste.LIBELLE = item.LIBELLE;
                        poste.LIBELLEDEPARTHTA = item.LIBELLEDEPARTHTA;
                        poste.OriginalCODE = item.OriginalCODE;
                        poste.PK_ID = item.PK_ID;
                        poste.USERCREATION = item.USERCREATION;
                        poste.USERMODIFICATION = item.USERMODIFICATION;
                        SessionObject.LsDesPosteElectriquesTransformateur.Add(poste);
                    }
                }
            };
            service.SelectAllPosteTransformationAsync();
            service.CloseAsync();
        }
        //private void RetourneListeDesDepartBT()
        //{
        //    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //    service.ChargerDepartBTCompleted += (s, args) =>
        //    {
        //        if (args != null && args.Cancelled)
        //            return;
        //        SessionObject.LsDesDepartBT = args.Result;
        //    };
        //    service.ChargerDepartBTAsync();
        //    service.CloseAsync();
        //}
        private void ChargerCoper()
        {
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneTousCoperCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstDesCopers = args.Result;
            };
            service.RetourneTousCoperAsync();
            service.CloseAsync();
        }
        private void ChargerTarifReglageCompteur()
        {
            try
            {
                if (SessionObject.LstTarifParReglageCompteur != null && SessionObject.LstTarifParReglageCompteur.Count != 0) return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTarifParReglageCompteurCompleted  += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTarifParReglageCompteur = args.Result;

                };
                service.ChargerTarifParReglageCompteurAsync ();
                service.CloseAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerTarifParCategorie()
        {
            try
            {
                if (SessionObject.LstTarifCategorie != null && SessionObject.LstTarifCategorie.Count != 0) return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTarifParCategorieCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTarifCategorie = args.Result;

                };
                service.ChargerTarifParCategorieAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void InitialiseDonneesGuichet()
        {
            RetournePosteConnecter();
            ChargerEtapesWorkflow();
            ChargerTypeBranchement();
            ChargerTypeComptage();
            ChargerRubriqueDevis();
            ChargerPuissance();
            ChargerPuissanceInstalle();
            ChargerForfait();
            ChargerTarif();
            ChargerTarifPuissance();
            ChargerPuissanceReglageCompteur();
            ChargerFrequence();
            ChargerMois();
            ChargerApplicationTaxe();
            ChargerLaListeDesCommunes();
            ChargerListeDesPostesSource();
            RetourneListeDesDepartHta();
            ChargerListeDesPostesTransformation();
            //RetourneListeDesDepartBT();
            ChargeQuartier();
            ChargeSecteur();
            ChargeRue();
            ChargerTournee();
            ChargerEtapeDemande();
            ChargerTypeDemande();
            ChargerListeDeProduit();
            ChargerCategorie();
            ChargerFermable();
            ChargerNatureClient();
            ChargerCivilite();
            ChargerNationnalite();
            ChargerCodeRegroupement();
            ChargerCodeConsomateur();
            ChargerCalibreCompteur();
            ChargerReglageCompteur(); 
            ChargerCadran();
            ChargerMarque();
            ChargerTypeCompteur();
            RetouneListeDesTaxes();
            ChargerCoutDemande();
            ChargerCoper();
            RetourneListeDesCas();
            ChargerTarifParCategorie();
            ChargerTarifReglageCompteur();

        }

        private void hyperlinkButton1_Click_1(object sender, RoutedEventArgs e)
        {

            SessionObject.IsChargerDashbord = true;
            timer.Stop();
            GetData();
        }

        private void MyTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IndexOperation = MyTab.SelectedIndex;
        }
    }

    //Pour le distinct
    public class EtapeEqualityComparer : IEqualityComparer<CsVwDashboardDemande>
    {
        #region IEqualityComparer<CsVwDashboardDemande> Members

        public bool Equals(CsVwDashboardDemande x, CsVwDashboardDemande y)
        {
            return x.FK_IDETAPE.Equals(y.FK_IDETAPE);
        }

        public int GetHashCode(CsVwDashboardDemande obj)
        {
            return obj.FK_IDETAPE.GetHashCode();
        }

        #endregion
    }

}

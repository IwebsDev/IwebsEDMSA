using System;
using System.Net;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Navigation;
using System.Text;
using Galatee.Silverlight.Library;
using SilverlightCommands;
using System.Windows.Controls;
using System.IO;
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.ServiceCaisse;
using Galatee.Silverlight.ServiceAuthenInitialize;
using System.Collections.Generic;
using Galatee.Silverlight.Caisse ;
using System.Windows.Media.Imaging;
using System.Windows.Browser;


namespace Galatee.Silverlight.MainView
{
    public class ModuleViewModel 
    {
        Dictionary<ServiceAuthenInitialize.CSMenuGalatee, List<ServiceAuthenInitialize.TreeNodes>> dico = new Dictionary<ServiceAuthenInitialize.CSMenuGalatee, List<ServiceAuthenInitialize.TreeNodes>>();
        static MenuViewModel viewModelG = new MenuViewModel();
        string Module = string.Empty;
        string EtatCaisse = string.Empty;
        static bool IsRefreh = false;
        Galatee.Silverlight.ServiceCaisse.CParametre CaisseSelection = new Galatee.Silverlight.ServiceCaisse.CParametre();
        //ServiceCaisse.CsOpenningDay CaisseOverte = null;
        Page _mainPage;
        static Library.Menu _menu;
        Library.Menu _menu2;

        public ModuleViewModel(Page mainPage, Library.Menu m, List<ServiceAuthenInitialize.CsDesktopGroup> modules, Accordion accordian)
        {
            try
            {
                _mainPage = mainPage;
                _menu = m;
                _menu2 = m;
                CreateModuleMenu(modules, accordian);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        void CreateModuleMenu(List<ServiceAuthenInitialize.CsDesktopGroup> modules, Accordion accordian)
        {

            try
            {
                //AccordionItem
                foreach (ServiceAuthenInitialize.CsDesktopGroup module in modules)
                {
                    AccordionItem AcorItems = new AccordionItem();
                    AcorItems.Header = module.NOM;
                    //AcorItems.Background = new SolidColorBrush(bgColor);
                    StackPanel bodyPanel = new StackPanel();
                    //panel.Height = Double.NaN;
                    //panel.Width = Double.NaN;
                    bodyPanel.Height = Double.NaN;
                    bodyPanel.Width = Double.NaN;
                    //bodyPanel.Height = 150;
                    //bodyPanel.Width = 150;
                    bodyPanel.Background = new SolidColorBrush(Colors.White);
                    //bodyPanel.Background = new SolidColorBrush(Colors.LightGray);

                    foreach (ServiceAuthenInitialize.CsDesktopItem item in module.SubItems)
                        CreateModuleItem(AcorItems, item, bodyPanel);

                    // peupler l'accordian 
                    AcorItems.Content = bodyPanel;
                    accordian.Items.Add(AcorItems);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void CreateModuleItem(AccordionItem AcorItems, ServiceAuthenInitialize.CsDesktopItem moduleItem, StackPanel bodyPanel)
        {
            try
            {
                StackPanel panel = new StackPanel();
                panel.Height = Double.NaN;
                panel.Width = Double.NaN;
                panel.Background = new SolidColorBrush(Colors.White);
                panel.HorizontalAlignment = HorizontalAlignment.Left;
                panel.Orientation = Orientation.Vertical;
                //panel.Background = new SolidColorBrush(Colors.LightGray);


                //Image
                Image image = new Image();
                image.Height = 45;
                image.Name = moduleItem.Process ;
                image.Margin = new Thickness(50, 8, 8, 5);
                image.Width = 45;
                image.Stretch = Stretch.Fill;

                string urlImage = "/Galatee.Silverlight;component/Image/" + moduleItem.NOM + ".png";
                string defaultImage = "/Galatee.Silverlight;component/Image/mnuToolbox.png";
                Uri uriImage;
                try
                {
                    uriImage = new Uri(urlImage, UriKind.Relative);
                }
                catch (Exception ex)
                {
                    uriImage = new Uri(defaultImage, UriKind.Relative);
                    Message.Show(ex, "CreateModuleItem");
                    //string error = ex.Message;
                }

                image.Source = new BitmapImage(uriImage);
                image.SetValue(Canvas.LeftProperty, 42.0);
                image.SetValue(Canvas.TopProperty, 15.0);

                image.MouseLeftButtonDown += new MouseButtonEventHandler(button_Click);
                panel.Children.Add(image);

                //TextBlock


                TextBlock textb = new TextBlock();
                textb.Text = moduleItem.LIBELLE_FONCTION;
                textb.SetValue(Canvas.LeftProperty, 30.0);
                textb.SetValue(Canvas.TopProperty, 86.0);
                textb.Foreground = new SolidColorBrush(Colors.Red);

                panel.Children.Add(textb);
                //Button button = new Button();
                //button.Content = moduleItem.LIBELLE_FONCTION;
                //button.SetValue(Canvas.LeftProperty, 30.0);
                //button.SetValue(Canvas.TopProperty, 86.0);
                //button.Name = moduleItem.NOM;
                //// add event delegate
                //button.Click += new RoutedEventHandler(button_Click);
                //panel.Children.Add(button);

                bodyPanel.Children.Add(panel);
                //AcorItems.Content = bodyPanel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ConnexionSuccess(object sender, EventArgs e)
        {
            try
            {
                FrmCreeCaisse frm = sender as FrmCreeCaisse;
                //CaisseSelection.LIBELLE = frm.CaisseSelectionee;
                UserConnecte.CaisseSelect = frm.CaisseSelectionee;
                UserConnecte.MatriculeSelect = frm.MatriculeSelectionee;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Construction du menu relatif à la fonction du UserConnecte
                Image b = sender as Image;
                //Button b = sender as Button;
                Module = b.Name;
                AuthentInitializeServiceClient client = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
                // MessageBox.Show(Module);
                client.GetMenuDuRoleCompleted += (senders, arg) =>
                {
                    if (arg.Cancelled || arg.Error != null)
                    {
                        Message.Show("Error occurs while processing request ! ", Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                        // 
                    }

                    dico = arg.Result;

                    if (dico.Count == 0 || dico == null)
                    {
                        Message.Show("Aucun module trouvé " + " " + "Payement module", Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }
                    else
                    {
                        if (Module == "Caisse")
                        {
                            /// appel menu galatee
                            /// 
                            CaisseServiceClient clt = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                            clt.VerifieEtatCaisseCompleted += (xx, args) =>
                            {
                                try
                                {
                                    if (args.Cancelled || args.Error != null)
                                    {
                                        Message.Show("Error occurs while processing request !", Galatee.Silverlight.Resources.Langue.errorTitle);
                                        return;
                                        // 
                                    }
                                    EtatCaisse = args.Result;
                                    if (EtatCaisse == SessionObject.Enumere.EtatDeCaissePasCassier)
                                    {
                                        // Charger le controle de choix de la caisse
                                        FrmCreeCaisse frm = new FrmCreeCaisse();
                                        frm.success += new EventHandler(ConnexionSuccess);
                                        frm.Show();
                                    }
                                    else if (EtatCaisse == SessionObject.Enumere.EtatDeCaisseDejaCloture)
                                    {                                   
                                        Message.Show(Galatee.Silverlight.Resources.Caisse.Langue.MsgCaisseDejaFerme , Galatee.Silverlight.Resources.Langue.errorTitle);
                                        return;
                                    }
                                    else if (EtatCaisse == SessionObject.Enumere.EtatDeCaisseAutreSessionOuvert)
                                    {
                                        Message.ShowWarning("Vous avez ouvert une caisse sur un autre poste " + "\n\r" + "Veuillez la clôturer", "Caisse");
                                        return;
                                    }
                                    else if (EtatCaisse == SessionObject.Enumere.EtatDeCaisseOuverteALaDemande)
                                        Message.Show(Galatee.Silverlight.Resources.Caisse.Langue.MsgCaisseOuverteAlaDemande, Galatee.Silverlight.Resources.Langue.errorTitle);

                                    //SessionObject.CaisseOverte = CaisseOverte;
                                    //if (!string.IsNullOrEmpty(CaisseOverte.Caissiere))
                                    //{
                                    //    DialogResult diag = new DialogResult("The day of  as been openned for you . \n Please correct the <<" + CaisseOverte.Raison + ">> and close that day", false, true, false);
                                    //    diag.Closed += new EventHandler(diag_Closed);
                                    //    diag.Show();

                                    //    // new DialogResult("Wrong parameters", false).Show();
                                    //    MessageBox.Show();//, "Payement modul");//, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //    return;
                                    //}


                                    RefreshMenuBar();
                                    MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                                    viewModelG = viewModel;

                                    if (!IsRefreh)
                                    {
                                        _mainPage.DataContext = viewModelG;

                                        IsRefreh = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Message.Show(ex, "Module Caisse");
                                }
                            };
                            clt.VerifieEtatCaisseAsync(UserConnecte.matricule,SessionObject.LePosteCourant.FK_IDCAISSE.Value  );
                        }

                        else // si le matricule ne correspond pas à celui d'une caissiere
                        {
                            /// Peupler les menus du module select 
                            try
                            {
                                RefreshMenuBar();
                                MenuViewModel viewModel = new MenuViewModel(_mainPage, dico, "Galatee.Silverlight." + Module, Module);
                                viewModelG = viewModel;

                                if (!IsRefreh)
                                {
                                    _mainPage.DataContext = viewModelG;

                                    IsRefreh = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                Message.Show(ex, "Module Caisse");
                            }

                        }

                    }
                };
                client.GetMenuDuRoleAsync(UserConnecte.FK_IDFONCTION, Module);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void diag_Closed(object sender, EventArgs e)
        {
            HtmlPage.Document.Submit();
        }

        void RefreshMenuBar()
        {

            if (IsRefreh)
            {

                try
                {
                    IsRefreh = false;
                    _menu.RefleshMenu();
                }
                catch (Exception ex)
                {
                    Message.Show(ex, "Module");
                }
            }
        }
    }

   
}


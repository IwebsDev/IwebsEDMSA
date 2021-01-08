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
using Galatee.ModuleLoader.ServiceCaisse;
using Galatee.ModuleLoader.ServiceAuthenInitialize;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Windows.Browser;
using System.Windows.Messaging;


namespace Galatee.ModuleLoader.MainView
{
    public class ModuleViewModel 
    {
        Dictionary<ServiceAuthenInitialize.CSMenuGalatee, List<ServiceAuthenInitialize.TreeNodes>> dico = new Dictionary<ServiceAuthenInitialize.CSMenuGalatee, List<ServiceAuthenInitialize.TreeNodes>>();
        string Module = string.Empty;
        string EtatCaisse = string.Empty;
        serviceWeb.CParametre CaisseSelection = new serviceWeb.CParametre();
        private LocalMessageSender messageSender;


        public ModuleViewModel(List<ServiceAuthenInitialize.CsDesktopGroup> modules, Accordion accordian)
        {
            messageSender = new LocalMessageSender(
               "receiver", LocalMessageSender.Global);
            messageSender.SendCompleted += sender_SendCompleted;

            CreateModuleMenu(modules, accordian);
        }

        private void sender_SendCompleted(object sender, SendCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    LogError(e);

                    return;
                }
            }
            catch (Exception ex)
            {
                string errror = ex.Message;
            }


            //string error  =
            //    "Message: " + e.Message + Environment.NewLine +
            //    "Attempt " + (int)e.UserState +
            //    "Completed." + Environment.NewLine +
            //    "Response: " + e.Response + Environment.NewLine +
            //    "ReceiverName: " + e.ReceiverName + Environment.NewLine +
            //    "ReceiverDomain: " + e.ReceiverDomain;


        }

        private void LogError(SendCompletedEventArgs e)
        {
            MessageBox.Show(e.Error.Message);
            //System.Diagnostics.Debug.WriteLine(
            //    "Attempt number {0}: {1}: {2}", (int)e.UserState,
            //    e.Error.GetType().ToString(), e.Error.Message);
         
        }

        void CreateModuleMenu(List<ServiceAuthenInitialize.CsDesktopGroup> modules, Accordion accordian)
        {
            accordian.BorderThickness = new Thickness(0);
            accordian.FontSize = 10;
            //AccordionItem
            foreach (ServiceAuthenInitialize.CsDesktopGroup module in modules)
            {
                AccordionItem AcorItems = new AccordionItem();
                AcorItems.Header = module.NOM;
                StackPanel bodyPanel = new StackPanel();
                bodyPanel.Height = Double.NaN;
                bodyPanel.Width = Double.NaN;
                bodyPanel.Background = new SolidColorBrush(Colors.White);

                foreach (ServiceAuthenInitialize.CsDesktopItem item in module.SubItems)
                {
                    CreateModuleItem(AcorItems, item, bodyPanel);
                    break;
                }
                // peupler l'accordian 
                AcorItems.Content = bodyPanel;
                accordian.Items.Add(AcorItems);
            }
        }

        void CreateModuleItem(AccordionItem AcorItems, ServiceAuthenInitialize.CsDesktopItem moduleItem, StackPanel bodyPanel)
        {
            StackPanel panel = new StackPanel();
            panel.Height = Double.NaN;
            panel.Width = Double.NaN;
            panel.Background = new SolidColorBrush(Colors.Red);
            //panel.b= new Thickness(0, 0, 0, 1);

            panel.Background = new SolidColorBrush(Color.FromArgb(250, 74, 74, 74));
            //ligne ou se trouve le carrÃ© ou se trouve icone de menu gauche
            AcorItems.Background = new SolidColorBrush(Color.FromArgb(250, 74, 74, 74));
            AcorItems.FontSize = 13;
            AcorItems.FontWeight = FontWeights.Bold;
            AcorItems.Foreground = new SolidColorBrush(Colors.White);
            AcorItems.BorderBrush = new SolidColorBrush(Color.FromArgb(200, 102, 102, 102));
            AcorItems.BorderThickness = new Thickness(0, 0, 0, 1);

            //Image
            Image image = new Image();
            image.Height = 20;
            image.Name = moduleItem.Process;
            image.Margin = new Thickness(50, 8, 50, 5);
            image.Width = 20;
            image.Stretch = Stretch.Fill;

            string urlImage = "/Galatee.ModuleLoader;component/Image/" + moduleItem.Process  + ".png";
            string defaultImage = "/Galatee.ModuleLoader;component/Image/mnuToolbox.png";
            Uri uriImage;
            try
            {
                uriImage = new Uri(urlImage, UriKind.Relative);
            }
            catch (Exception ex)
            {
                uriImage = new Uri(defaultImage, UriKind.Relative);
                string error = ex.Message;
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

            bodyPanel.Children.Add(panel);
        }

        private void SendMessage(string message)
        {
            messageSender.SendAsync(message);
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            // Construction du menu relatif à la fonction du UserConnecte
            Image b = sender as Image;
            Module = b.Name;

           SendMessage(Module);
        }
    }

   
}


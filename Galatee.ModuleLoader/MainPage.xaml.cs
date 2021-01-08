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
using Galatee.ModuleLoader.ServiceAuthenInitialize;
using Galatee.ModuleLoader.MainView;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Messaging;

namespace Galatee.ModuleLoader
{
    public partial class MainPage : UserControl
    {
        private LocalMessageSender messageSender;

        public MainPage()
        {
            InitializeComponent();

            //messageSender = new LocalMessageSender(
            //   "receiver", LocalMessageSender.Global);
            //messageSender.SendCompleted += sender_SendCompleted;
        }

        XmlSerializer serializer = new XmlSerializer(typeof(CsDesktopGroup));

        void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
            {
                List<CsDesktopGroup> desks = new List<CsDesktopGroup>();

                foreach (string file in store.GetFileNames("*.desktopgroup"))
                {
                    using (FileStream stream = store.OpenFile(file, FileMode.Open))
                    {
                        CsDesktopGroup desk = (CsDesktopGroup)serializer.Deserialize(stream);
                        desks.Add(desk);
                    }
                }

                InitializeAccordion(desks);
            }
        }

        void InitializeAccordion(List<CsDesktopGroup> moduleTree)
        {
            ModuleViewModel viewModel = new ModuleViewModel(moduleTree, moduleAccordion);
        }

        void sender_SendCompleted(object sender, SendCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LogError(e);
               
                return;
            }

            //string error  =
            //    "Message: " + e.Message + Environment.NewLine +
            //    "Attempt " + (int)e.UserState +
            //    "Completed." + Environment.NewLine +
            //    "Response: " + e.Response + Environment.NewLine +
            //    "ReceiverName: " + e.ReceiverName + Environment.NewLine +
            //    "ReceiverDomain: " + e.ReceiverDomain;

        }

        void LogError(SendCompletedEventArgs e)
        {
            MessageBox.Show(e.Error.Message);
            //System.Diagnostics.Debug.WriteLine(
            //    "Attempt number {0}: {1}: {2}", (int)e.UserState,
            //    e.Error.GetType().ToString(), e.Error.Message);
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
            {
                List<CsDesktopGroup> desks = new List<CsDesktopGroup>();

                foreach (string file in store.GetFileNames("*.desktopgroup"))
                {
                    using (FileStream stream = store.OpenFile(file, FileMode.Open))
                    {
                        CsDesktopGroup desk = (CsDesktopGroup)serializer.Deserialize(stream);
                        desks.Add(desk);
                    }
                }

                InitializeAccordion(desks);
            }
        }

    }
}

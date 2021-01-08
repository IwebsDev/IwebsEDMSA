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

namespace Licence
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FrmGenereLicence ctrl = new FrmGenereLicence();
            ctrl.Show();
        }

        private void Button_Connexion(object sender, RoutedEventArgs e)
        {
            FrmGenereChaineConnexion ctrl = new FrmGenereChaineConnexion();
            ctrl.Show();
        }
    }
}

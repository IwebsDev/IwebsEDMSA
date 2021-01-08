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
using Galatee.Silverlight.ServiceSig;

namespace Galatee.Silverlight.SIG
{
    public partial class PushpinDetails : UserControl
    {
        public PushpinDetails(CsAbonneCarte abonne)
        {
            InitializeComponent();

            try
            {
                this.DataContext = abonne;
            }
            catch (Exception ex)
            {
                Message.Show(ex, "erreur");
            }
            //this.LayoutRoot.Children.Add(panel);
        }
    }
}

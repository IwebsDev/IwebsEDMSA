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

namespace Galatee.Silverlight.Shared
{
    public partial class UcPreviewReport : UserControl
    {
        private UserControl pUserControl = null;
        public UcPreviewReport(UserControl usercontrol)
        {
            InitializeComponent();
            //UcContainer.Height = usercontrol.Height;
            pUserControl = usercontrol;
            //UcContainer.Width = usercontrol.Width;
            Loaded+=new RoutedEventHandler(UcPreviewReport_Loaded);
        }
        public UcPreviewReport()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(UcPreviewReport_Loaded);
        }

        private void UcPreviewReport_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if(pUserControl != null)
                    UcContainer.Content = pUserControl;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, "Report");
            }
        }
    }
}

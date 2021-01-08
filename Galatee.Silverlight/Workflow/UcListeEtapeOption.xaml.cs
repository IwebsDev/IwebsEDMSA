using Galatee.Silverlight.ServiceParametrage;
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

namespace Galatee.Silverlight.Workflow
{
    public partial class UcListeEtapeOption : UserControl
    {
        public UcListeEtapeOption()
        {
            InitializeComponent();
        }
        List<CsVwDashboardDemande> elt = new List<CsVwDashboardDemande>();
        public UcListeEtapeOption(List<CsVwDashboardDemande> lstElt)
        {
            InitializeComponent();
            elt = lstElt;
            this.dtgEtape.ItemsSource = elt;
            lbl_Circuitname.Content = lstElt.First().NOMOPERATION;
        }
    }
}

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
using Galatee.Silverlight.ServiceFacturation;

namespace Galatee.Silverlight.Report
{
    public partial class FrmDetailCasFacturation : ChildWindow
    {
        public FrmDetailCasFacturation()
        {
            InitializeComponent();
        }
        public FrmDetailCasFacturation(List<CsEvenement> lesEvenement)
        {
            InitializeComponent();
            dataGrid1.ItemsSource = null;
            dataGrid1.ItemsSource = lesEvenement;
        }
     
        Dictionary<string, string> param = null;
        List<CsEvenement> lstDonnee = new List<CsEvenement>();
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            param = new Dictionary<string, string>();
            try
            {
                if (dataGrid1.ItemsSource == null)
                    throw new Exception(Galatee.Silverlight.Resources.Parametrage.Languages.AucuneDonneeAImprimer);
                lstDonnee = ((List<CsEvenement>)dataGrid1.ItemsSource).ToList();
                Galatee.Silverlight.Shared.FrmOptionEditon ctrl = new Shared.FrmOptionEditon();
                ctrl.Closed += ctrl_Closed;
                this.IsEnabled = false;
                ctrl.Show();

            }
            catch (Exception ex)
            {

            }
        }

        void ctrl_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            Galatee.Silverlight.Shared.FrmOptionEditon ctrs = sender as Galatee.Silverlight.Shared.FrmOptionEditon;
            if (ctrs.IsOptionChoisi)
            {
                if (ctrs.OptionSelect == SessionObject.EnvoiPrinter)
                    Utility.ActionDirectOrientation<ServicePrintings.CsEvenement, ServiceFacturation.CsEvenement>(lstDonnee, param, SessionObject.CheminImpression, "CasFactureDetail", "Report", true);
                else if (ctrs.OptionSelect == SessionObject.EnvoiExecl)
                    Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceFacturation.CsEvenement>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "CasFactureDetail", "Report", true, "xlsx");

                else if (ctrs.OptionSelect == SessionObject.EnvoiWord)
                    Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceFacturation.CsEvenement>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "CasFactureDetail", "Report", true, "doc");

                else if (ctrs.OptionSelect == SessionObject.EnvoiPdf)
                    Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceFacturation.CsEvenement>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "CasFactureDetail", "Report", true, "pdf");

            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


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
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.ServiceAccueil ;

namespace Galatee.Silverlight.Devis
{
    public partial class UcSuiviDevis : ChildWindow
    {
        private ObjDEVIS Devis = null;
        public UcSuiviDevis()
        {
            InitializeComponent();
        }

        public UcSuiviDevis(ObjDEVIS pDevis)
        {
            InitializeComponent();
            Devis = pDevis;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.dataGrid.SelectedItem != null && this.dataGrid.SelectedItems.Count == 1)
                {
                    var suiviDevis = dataGrid.SelectedItem as ObjSUIVIDEVIS;
                    if (suiviDevis != null)
                    {
                       var form = new UcDetailsSuivi(suiviDevis,Devis,SessionObject.ExecMode.Consultation);
                        if (form != null)
                            form.Show();
                    }
                }
                //this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void dataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CsCriteresDevis criteres = new CsCriteresDevis();
                criteres.IdDevis = Devis.PK_ID;
                criteres.NumeroDevis = Devis.NUMDEVIS;
                criteres.CodeProduit = Devis.CODEPRODUIT;
                criteres.DateEtape = (DateTime)Devis.DATEETAPE;
                if (Devis.FK_IDETAPEDEVIS != null)
                criteres.IdEtapeDevis = (int)Devis.FK_IDETAPEDEVIS;
                LayoutRoot.Cursor = Cursors.Wait;

                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GetSuiviDevisByDevisIdCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.Show(error, Languages.txtDevis);
                        return;
                    }
                    if (args.Result == null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        Message.Show(Languages.msgErreurChargementDonnees, Languages.txtDevis);
                        return;
                    }
                    dataGrid.ItemsSource = args.Result;
                    this.OKButton.IsEnabled = this.dataGrid.SelectedItems.Count == 1;
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.GetSuiviDevisByDevisIdAsync(criteres);
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            SolidColorBrush SolidColorBrush = null;
            try
            {
                var devisRow = e.Row.DataContext as ObjSUIVIDEVIS;
                if (devisRow != null)
                {
                    if (Devis.FK_IDETAPEDEVIS == devisRow.FK_IDDEMANDE)
                    {
                        SolidColorBrush = new SolidColorBrush(Colors.Red);
                        e.Row.Background = SolidColorBrush;
                        e.Row.FontWeight = FontWeights.Bold;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.OKButton.IsEnabled = this.dataGrid.SelectedItems.Count == 1;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
    }
}


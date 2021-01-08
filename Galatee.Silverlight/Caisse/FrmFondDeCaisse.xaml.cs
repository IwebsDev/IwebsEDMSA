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
using Galatee.Silverlight.Resources.Caisse;
using Galatee.Silverlight.ServiceCaisse;
using Galatee.Silverlight.MainView;

namespace Galatee.Silverlight.Caisse
{
    public partial class FrmFondDeCaisse : ChildWindow
    {
        public FrmFondDeCaisse()
        {
            InitializeComponent();
            InitializeInfoOnWindows();
        }
        public FrmFondDeCaisse(string matricule,string Caisse)
        {
            InitializeComponent();
        }

        private void InitializeInfoOnWindows()
        {
            
            this.txtFondActuel.IsEnabled = false;
            this.cbo_MoisComptable.IsEnabled = false;

            this.cbo_MoisComptable.Items.Add(DateTime.Now.Year.ToString() +FormatMonthLenght(DateTime.Now.Month.ToString()));
            this.cbo_MoisComptable.SelectedIndex = 0;
            this.txtFondActuel.Text = "0";
            
        }
        private void LoadInfoInControl()
        {
            CsCaisse LaCaisse = SessionObject.ListeCaisse.FirstOrDefault(c => c.NUMCAISSE == this.txtNumCaisse.Text);
            if (LaCaisse != null)
            {
                this.txtFondActuel.Text = LaCaisse.FONDCAISSE.ToString();
            }
        }
        public void Translate()
        {
            this.lbl_fondCaisseActuel.Content = Langue.lbl_FondCaisseInit;
            this.lbl_NouveauFondCaisse .Content = Langue.lbl_NouveauFond ;
            this.lbl_NumCaisse.Content = Langue.Caisse;

        }

        private string  FormatMonthLenght(string MonthNumber)
        {
            return MonthNumber.Length == 1 ? '0' + MonthNumber : MonthNumber;
        }

        private void AjustementDeFondCaisse()
        {

            CaisseServiceClient srv = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            srv.AjustementDeFondCaisseCompleted += (ss, ee) =>
            {
                try
                {
                    if (ee.Cancelled || ee.Error != null || ee.Result == null)
                    {
                        string error = ee.Error.InnerException.ToString();
                        return;
                    }
                    Message.ShowError("Ajustement effectué avec succès","");
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                }
            };
            if (!string.IsNullOrEmpty(this.txtNumCaisse.Text) && !string.IsNullOrEmpty(this.txtFondNouveau.Text) && !string.IsNullOrEmpty(this.txtFondActuel.Text) && !string.IsNullOrEmpty(this.cbo_MoisComptable.SelectedValue.ToString()))
            srv.AjustementDeFondCaisseAsync( this.txtNumCaisse.Text,( decimal.Parse( this.txtFondNouveau.Text) -decimal.Parse(this.txtFondActuel.Text)).ToString(),this.cbo_MoisComptable.SelectedValue.ToString(),UserConnecte.matricule);
        }
        private void ChargerListeCaisseDispo()
        {
            //UserConnecte.Centre = Silverlight.Classes.IsolatedStorage.getCentre();
            CaisseServiceClient srv = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            srv.ListeCaisseDisponibleCompleted += (ss, ee) =>
            {
                try
                {
                    if (ee.Cancelled || ee.Error != null || ee.Result == null)
                    {
                        string error = ee.Error.InnerException.ToString();
                        return;
                    }
                    SessionObject.ListeCaisse = ee.Result;
                    if (SessionObject.ListeCaisse.Count > 0)
                    {
                        LoadInfoInControl();
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                }
            };
            srv.ListeCaisseDisponibleAsync(UserConnecte.Centre, UserConnecte.matricule);
        }

        private void txtNumCaisse_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.txtNumCaisse.Text.Length == SessionObject.Enumere.TailleNumCaisse)
                    if (SessionObject.ListeCaisse.Count > 0)
                    {
                        LoadInfoInControl();
                    }
                    else
                        ChargerListeCaisseDispo();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
            }


          
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            AjustementDeFondCaisse();
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}


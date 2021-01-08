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
using Galatee.Silverlight.ServiceAccueil;
//using Galatee.Silverlight.serviceWeb;

namespace Galatee.Silverlight.MainView
{
    public partial class UcListeTa : ChildWindow
    {

        public event EventHandler Closed;
        bool isOkClick;

        public bool GetisOkClick
        {
            set { isOkClick = value; }
            get { return isOkClick; }
        }
        CParametre EltSelectioner = new CParametre();
        public CParametre MyElt
        {
            get { return EltSelectioner; }
            set { EltSelectioner = value; }
        }

        public List<CParametre> ListeDeSite = new List<CParametre>();

        public UcListeTa()
        {
            InitializeComponent();
        }

        public UcListeTa(List<CParametre> _LstSite)
        {
            ListeDeSite = _LstSite;
            InitializeComponent();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
            isOkClick = true;
            getCloseAfterSelection();
            this.DialogResult = false;

        }
        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            getCloseAfterSelection();

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Closed(this, new EventArgs());
            this.DialogResult = false;
        }
        void ChargerDataGrid(List<CParametre> ListDesElts)
        {
            try
            {
                this.dtg_Elts.ItemsSource = ListDesElts;
                if (ListDesElts != null && ListDesElts.Count != 0)
                    this.dtg_Elts.SelectedItem = ListDesElts[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dtg_Elts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isOkClick = true;
            getCloseAfterSelection();
        }

        void getCloseAfterSelection()
        {
            try
            {
                if (Closed != null)
                {
                    EltSelectioner = (CParametre)dtg_Elts.SelectedItem;
                    Closed(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ChargerDataGrid(ListeDeSite);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
    }
}


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
using Galatee.Silverlight.ServiceCaisse;
//using Galatee.Silverlight.serviceWeb;

namespace Galatee.Silverlight.Shared
{
    public partial class UcListeParametre : ChildWindow
    {
        public event EventHandler Closed;
        public bool isOkClick;
        public bool IsMultiselect;

        public bool GetisOkClick
        {
            set { isOkClick = value; }
            get { return isOkClick; }
        }
        ServiceAccueil.CParametre ObjectSelect = new ServiceAccueil.CParametre();
        public ServiceAccueil.CParametre MyObject { get; set; }

        public List<ServiceAccueil.CParametre> MyObjectList { get; set; }
        public UcListeParametre()
        {
            InitializeComponent();
        }


        List<ServiceAccueil.CParametre> ListDesElts = new List<ServiceAccueil.CParametre>();
        public UcListeParametre(List<ServiceAccueil.CParametre> ParamListeDesELts, bool Multiselect, string Title)
        {
            InitializeComponent();
            ListDesElts = ParamListeDesELts;
            IsMultiselect = Multiselect;
            this.Title = Title;
            ChargerDataGrid(ListDesElts);
        }

        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<ServiceAccueil.CParametre>;
            if (!IsMultiselect)
                allObjects.ForEach(t => t.IsSelect = false);
            if (dg.SelectedItem != null)
            {
                ServiceAccueil.CParametre SelectedObject = (ServiceAccueil.CParametre)dg.SelectedItem;

                if (SelectedObject.IsSelect  == false)
                    SelectedObject.IsSelect = true;
                else
                    SelectedObject.IsSelect = false;
            }
        }

        void getCloseAfterSelectionAll()
        {
            try
            {
                if (Closed != null)
                {
                    List < ServiceAccueil.CParametre> lstSelect = ((List<ServiceAccueil.CParametre>)Lst_Liste.ItemsSource).Where(t=>t.IsSelect == true ).ToList();
                    MyObjectList = lstSelect;
                   Closed(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      void getCloseAfterSelectionOne()
        {
            try
            {
                if (Closed != null)
                {
                    ObjectSelect = Lst_Liste.SelectedItems.OfType<ServiceAccueil.CParametre>().FirstOrDefault(t => t.IsSelect);
                   MyObject = ObjectSelect;
                   Closed(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
            isOkClick = true;
            if (IsMultiselect)
                getCloseAfterSelectionAll();
            else
                getCloseAfterSelectionOne();

            this.DialogResult = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Closed != null)
            {
                Closed(this, new EventArgs());
            }
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
        void ChargerDataGrid(List<ServiceAccueil.CParametre> ListDesElts)
        {
            Lst_Liste.ItemsSource = ListDesElts;
        }

        private void btn_ToutCategorie_Click(object sender, RoutedEventArgs e)
        {
            ToutOuRien(true);
        }

        private void ToutOuRien(bool etat)
        {
            if (Lst_Liste.ItemsSource != null)
            {
                foreach (ServiceAccueil.CParametre item in Lst_Liste.ItemsSource)
                {
                    if (item.IsSelect == !etat)
                        item.IsSelect = etat;
                }
            }
        }

        private void btn_rienCategorie_Click_1(object sender, RoutedEventArgs e)
        {
            ToutOuRien(false);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<ServiceAccueil.CParametre> TempListDesElts = ListDesElts;
            if (!string.IsNullOrWhiteSpace( txt_Code_Rech.Text))
            {
                var temp = TempListDesElts.Where(c => c.CODE.Trim() == txt_Code_Rech.Text.Trim());
                TempListDesElts = temp!=null?temp.ToList():null;
            }
            if (!string.IsNullOrWhiteSpace(txt_Libelle_Rech.Text))
            {
                var temp = TempListDesElts.Where(c => c.LIBELLE.Trim() == txt_Libelle_Rech.Text.Trim());
                TempListDesElts = temp != null ? temp.ToList() : null;
            }
            Lst_Liste.ItemsSource = TempListDesElts;
        }
    }
}


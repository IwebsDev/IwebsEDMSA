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


namespace Galatee.Silverlight.Facturation
{
    public partial class UcGenerique : ChildWindow
    {
        public event EventHandler Closed;
        public bool isOkClick;
        public bool IsMultiselect;

        public bool GetisOkClick
        {
            set { isOkClick = value; }
            get { return isOkClick; }
        }
        CsLotri ObjectSelect = new CsLotri();
        public CsLotri MyObject { get; set; }

        public List<CsLotri> MyObjectList { get; set; }
        public UcGenerique()
        {
            InitializeComponent();
        }


        List<CsLotri> ListDesElts = new List<CsLotri>();
        public UcGenerique(List<CsLotri> ParamListeDesELts, bool Multiselect, string Title)
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
            var allObjects = dg.ItemsSource as List<CsLotri>;
            if (!IsMultiselect)
                allObjects.ForEach(t => t.IsSelect = false);
            if (dg.SelectedItem != null)
            {
                 CsLotri SelectedObject = (CsLotri)dg.SelectedItem;

                if (SelectedObject.IsSelect == false)
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
                    List<CsLotri> lstSelect = ((List<CsLotri>)Lst_Liste.ItemsSource).Where(t => t.IsSelect == true).ToList();
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
                    ObjectSelect = Lst_Liste.SelectedItems.OfType<CsLotri>().FirstOrDefault(t => t.IsSelect);
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
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
        void ChargerDataGrid(List<CsLotri> ListDesElts)
        {
            Lst_Liste.ItemsSource = ListDesElts;
        }
    }
}


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
//using Galatee.Silverlight.serviceWeb;

namespace Galatee.Silverlight.MainView
{
    public partial class UcListeGenerique : ChildWindow
    {

        public event EventHandler Closed;
        public bool isOkClick;
        public bool IsMultiselect;

        public bool GetisOkClick
        {
            set { isOkClick = value; }
            get { return isOkClick; }
        }
        object ObjectSelect = new object();
        public object  MyObject{ get; set; }
      
        public List<object> MyObjectList { get; set; }


        public UcListeGenerique()
        {
            InitializeComponent();
        }
        public  List<object> MaListe = new List<object>();
        string pNomColonne1 = string.Empty;
        string pNomColonne2 = string.Empty;

        public UcListeGenerique(List<object> _LstCas, string _pNomColone1, string _pNomColone2,string title)
        {
            InitializeComponent();
            try
            {
                MaListe = _LstCas;
                pNomColonne1 = _pNomColone1;
                pNomColonne2 = _pNomColone2;
                this.Title = title;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        List<string> pListeColonne = new List<string>(); 
        public UcListeGenerique(List<object> _LstCas, List<string> _pListeColonne, string title)
        {
            InitializeComponent();
            try
            {
                MaListe = _LstCas;
                pListeColonne = _pListeColonne;
                this.Title = title;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        public UcListeGenerique(List<object> _LstParametres, List<string> _pListeColonne,bool _IsMultiselect, string title)
        {
            InitializeComponent();
            try
            {
                MaListe = _LstParametres;
                IsMultiselect = _IsMultiselect;
                pListeColonne = _pListeColonne;
                this.Title = title;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        Dictionary<string, string> pListeColonneDico = new Dictionary<string, string>(); 

        public UcListeGenerique(List<object> _LstParametres, Dictionary<string, string> _pListeColonne, bool _IsMultiselect, string title)
        {
            InitializeComponent();
            try
            {
                MaListe = _LstParametres;
                IsMultiselect = _IsMultiselect;
                pListeColonneDico = _pListeColonne;
                this.Title = title;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.DialogResult = true;
                isOkClick = true;
                if (IsMultiselect)
                    getCloseAfterSelection();
                else
                    getElementCloseAfterSelection();

                this.DialogResult = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                getCloseAfterSelection();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.DialogResult = true;
                isOkClick = false;
                Closed(this, new EventArgs());
                //if (IsMultiselect)
                //    getCloseAfterSelection();
                //else
                //    getElementCloseAfterSelection();

                this.DialogResult = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void ChargerDataGrid(List<object > LaListe,string pColonne0,string pColonne1)
        {

            try
            {
                this.dtg_Elts.ItemsSource = LaListe;
                DataGridTextColumn Colonne0 = new DataGridTextColumn();
                Colonne0.Binding = new System.Windows.Data.Binding(pColonne0);
                DataGridTextColumn Colonne1 = new DataGridTextColumn();
                Colonne1.CanUserResize = false;
                //Colonne1.Width =
                Colonne1.Binding = new System.Windows.Data.Binding(pColonne1);
                dtg_Elts.Columns.Add(Colonne0);
                dtg_Elts.Columns.Add(Colonne1);
                if (LaListe.Count != 0)
                    this.dtg_Elts.SelectedItem = LaListe[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void ChargerDataGrid(List<object> LaListe, List<string> pListeColonne)
        {

            try
            {
                this.dtg_Elts.ItemsSource = LaListe;
                foreach (string item in pListeColonne)
                {
                    DataGridTextColumn colonne = new DataGridTextColumn();
                    colonne.Binding = new System.Windows.Data.Binding(item);
                    colonne.Header = item;
                    colonne.CanUserResize = true ;
                    colonne.IsReadOnly = true;
                    dtg_Elts.Columns.Add(colonne);
                }
                if (LaListe.Count != 0)
                    this.dtg_Elts.SelectedItem = LaListe[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ChargerDataGrid(List<object> LaListe, Dictionary<string, string> pListeColonneDico)
        {

            try
            {
                this.dtg_Elts.ItemsSource = LaListe;

                foreach (var item in pListeColonneDico)
                {
                    DataGridTextColumn colonne = new DataGridTextColumn();
                    colonne.Binding = new System.Windows.Data.Binding(item.Key );
                    colonne.Header = item.Value ;
                    colonne.CanUserResize = true;
                    colonne.IsReadOnly = true;
                    dtg_Elts.Columns.Add(colonne);
                }
                if (LaListe.Count != 0)
                    this.dtg_Elts.SelectedItem = LaListe[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void dtg_Elts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                isOkClick = true;
                getCloseAfterSelection();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void 
        getCloseAfterSelection()
        {
            try
            {
                if (Closed != null)
                {
                   ObjectSelect = (object)dtg_Elts.SelectedItem;
                   MyObjectList = dtg_Elts.SelectedItems.OfType<object>().ToList();// as List<object>;
                   Closed(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    public void   getElementCloseAfterSelection()
        {
            try
            {
                if (Closed != null)
                {
                   ObjectSelect = (object)dtg_Elts.SelectedItem;
                   MyObject = ObjectSelect;
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
                if (pListeColonne != null && pListeColonne.Count != 0)
                    ChargerDataGrid(MaListe, pListeColonne);
                else if (pListeColonneDico != null && pListeColonneDico.Count != 0)
                    ChargerDataGrid(MaListe, pListeColonneDico);
                else 
                   ChargerDataGrid(MaListe, pNomColonne1, pNomColonne2);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        DateTime lastClick = DateTime.Now;
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsMultiselect)
            {
                DataGrid dg = (sender as DataGrid);
                if (dg.SelectedItem != null)
                {
                
                }
            }
        }


    }
}


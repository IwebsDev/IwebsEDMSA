using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.ServiceAccueil ;

namespace Galatee.Silverlight.Devis
{
    public partial class UcListAppareils : ChildWindow, INotifyPropertyChanged
    {
        ObservableCollection<ObjAPPAREILS> donnesDatagrid = new ObservableCollection<ObjAPPAREILS>();
        public List<ObjAPPAREILS> AppareilsSelectionnes { get; set; }
        public int puissanceTotaleAppareil = 0;
        private string _title = string.Empty;
        public UcListAppareils()
        {
            InitializeComponent();
            
        }

        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetSelectedAppareilsFromScreen();
              //GetPuissanceTotaleAppareil();
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, _title);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = false;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, _title);
            }
        }

        public ObservableCollection<ObjAPPAREILS> DonnesDatagrid
        {
            get { return donnesDatagrid; }
            set
            {
                if (value == donnesDatagrid)
                    return;
                donnesDatagrid = value;
                NotifyPropertyChanged("DonnesDatagrid");
            }
        }

        private void GetSelectedAppareilsFromScreen()
        {
            try
            {
                AppareilsSelectionnes = new List<ObjAPPAREILS>();
                foreach (ObjAPPAREILS appareils in donnesDatagrid)
                {
                    if(appareils.NOMBRE > 0 && appareils.PUISSANCE >0)
                    {
                        appareils.DISPLAYLABEL = appareils.CODEAPPAREIL + "-" + appareils.DESIGNATION + " N "+
                                                 appareils.NOMBRE.ToString()  + " P " + appareils.PUISSANCE.ToString();
                        AppareilsSelectionnes.Add(appareils);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetData()
        {
            try
            {
                if (SessionObject.LstAppareil != null && SessionObject.LstAppareil.Count != 0)
                {
                    foreach (var item in SessionObject.LstAppareil)
                        DonnesDatagrid.Add(item);
                    dtgAppareils.ItemsSource = DonnesDatagrid;
                    return;
                
                }
                LayoutRoot.Cursor = Cursors.Wait;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.GetAllAppareilCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.Show(error, _title);
                        return;
                    }
                    DonnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var item in args.Result)
                        {
                            DonnesDatagrid.Add(item);
                        }
                    dtgAppareils.ItemsSource = DonnesDatagrid;
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                service.GetAllAppareilAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetPuissanceTotaleAppareil()
        {
            if (AppareilsSelectionnes != null && AppareilsSelectionnes.Count >0)
            {
                foreach(ObjAPPAREILS apsel in AppareilsSelectionnes)
                {
                    this.puissanceTotaleAppareil += apsel.PUISSANCE * apsel.NOMBRE;
                }
            }
        }

        private void Translate()
        {
            try
            {
                _title = "Appareils";
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
                Translate();
                GetData();
               
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, _title);
            }
        }

        private void dtgAppareils_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(dtgAppareils.SelectedItem != null)
                {
                    var appareil = dtgAppareils.SelectedItem as ObjAPPAREILS;
                    if (appareil != null)
                    {
                        Txt_Nombre.Text = appareil.NOMBRE.ToString();
                        //Txt_Puissance.Text = appareil.PUISSANCE.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, _title);
            }
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, _title);
            }
        }

        private void Txt_Nombre_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (!string.IsNullOrEmpty(Txt_Nombre.Text) && int.Parse(Txt_Nombre.Text) > 0)
                //{
                //    if (dtgAppareils.SelectedItem != null)
                //    {
                //        var appareil = dtgAppareils.SelectedItem as ObjAPPAREILS;

                //        if (appareil != null) appareil.Nombre = int.Parse(Txt_Nombre.Text);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, _title);
            }
        }

        private void Txt_Puissance_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (!string.IsNullOrEmpty(Txt_Puissance.Text) && int.Parse(Txt_Puissance.Text) > 0)
                //{
                //    if (dtgAppareils.SelectedItem != null)
                //    {
                //        var appareil = dtgAppareils.SelectedItem as ObjAPPAREILS;
                //        if (appareil != null) appareil.Puissance = int.Parse(Txt_Puissance.Text);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, _title);
            }
        }

        private void dtgAppareils_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            SolidColorBrush SolidColorBrush = null;
            try
            {
                var appareilRow = e.Row.DataContext as ObjAPPAREILS;
                if (appareilRow != null)
                {
                    if (AppareilsSelectionnes != null && AppareilsSelectionnes.Count > 0)
                    {
                        foreach (var appareil in AppareilsSelectionnes)
                        {
                            if (appareil.NOMBRE >0 && appareil.PUISSANCE > 0 && appareil.CODEAPPAREIL == appareilRow.CODEAPPAREIL &&  appareil.PK_ID == appareilRow.PK_ID )
                            {
                                appareilRow.PUISSANCE = appareil.PUISSANCE;
                                appareilRow.NOMBRE = appareil.NOMBRE;
                                SolidColorBrush = new SolidColorBrush(Colors.LightGray);
                                e.Row.Background = SolidColorBrush;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Txt_Puissance_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //if (!string.IsNullOrEmpty(Txt_Puissance.Text) && int.Parse(Txt_Puissance.Text) > 0)
                //{
                //    if (dtgAppareils.SelectedItem != null)
                //    {
                //        var appareil = dtgAppareils.SelectedItem as ObjAPPAREILS;
                //        if (appareil != null) appareil.PUISSANCE = int.Parse(Txt_Puissance.Text);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, _title);
            }
        }
        private int PuisanceTotal()
        {
            int Puissance = 0;
            foreach (ObjAPPAREILS item in dtgAppareils.ItemsSource)
                Puissance = Puissance + (item.PUISSANCE * item.NOMBRE);

            return Puissance;
        }
        private void Txt_Nombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                   
                if (!string.IsNullOrEmpty(Txt_Nombre.Text) && int.Parse(Txt_Nombre.Text) > 0)
                {
                    if (dtgAppareils.SelectedItem != null)
                    {
                        var appareil = dtgAppareils.SelectedItem as ObjAPPAREILS;
                        if (appareil != null) appareil.NOMBRE = int.Parse(Txt_Nombre.Text);
                    }
                }
                this.Txt_PuissanceGlobale.Text = PuisanceTotal().ToString();

            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, _title);
            }
        }
    }
}


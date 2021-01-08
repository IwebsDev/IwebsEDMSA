using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Classes;
using Galatee.Silverlight.ServiceParametrage;
using Galatee.Silverlight.Resources.Parametrage;

namespace Galatee.Silverlight.Parametrage
{
    public partial class UcListActiviteCouleur : ChildWindow
    {
        public CsCouleurActivite ObjetSelectionne { get; set; }

        ObservableCollection<CsCouleurActivite> donnesDatagrid = new ObservableCollection<CsCouleurActivite>();

        public UcListActiviteCouleur()
        {
            try
            {
                InitializeComponent();
                Translate();
                GetData();
                this.DataContext = ObjetSelectionne;
            }
            catch (Exception ex)
            {

                Message.Show(ex.Message, Languages.Coutcoper);
            }
        }
        private void Translate()
        {
            try
            {
 
                Title = Languages.CoperDemande;
                btnDelete.Content = Languages.Supprimer;
                btnPrint.Content = Languages.Imprimer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region INotifyPropertyChanged Membres
       

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion
        public ObservableCollection<CsCouleurActivite> DonnesDatagrid
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
        List<CsCouleurActivite> lstInit = new List<CsCouleurActivite>();
        private void GetData()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllActiviteCouleurCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.CoperDemande);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    DonnesDatagrid.Clear();
                    if (args.Result != null)
                    {
                        lstInit = args.Result;
                        foreach (var item in args.Result)
                            DonnesDatagrid.Add(item);
                    }
                    dtgrdParametre.ItemsSource = DonnesDatagrid;
                };
                client.SelectAllActiviteCouleurAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dtgrdParametre_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //aTa0 o = lvwResultat.SelectedItem as aTa0;
                string Ucname = string.Empty;
                //    if(int.Parse(o.CODE) > 0 && int.Parse(o.CODE)  < 1000)
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Coutcoper);
            }
        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsCouleurActivite;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsCouleurActivite;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.CoperDemande);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            UcActiviteCouleur ctr = new UcActiviteCouleur(lstInit);
            ctr.Closed += new EventHandler(ctrlr_Closed);
            ctr.Show();
        }

        private void ctrlr_Closed(object sender, EventArgs e)
        {
            GetData();
        }

        private void btn_Modifier_Click(object sender, RoutedEventArgs e)
        {
            if (this.dtgrdParametre.SelectedItem != null)
            {
                UcActiviteCouleur ctr = new UcActiviteCouleur((CsCouleurActivite)this.dtgrdParametre.SelectedItem, lstInit);
                ctr.Closed += new EventHandler(ctrlr_Closed);
                ctr.Show();
            }
        }
    }
}


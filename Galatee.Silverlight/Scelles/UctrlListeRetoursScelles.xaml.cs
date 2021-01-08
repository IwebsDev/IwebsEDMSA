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
using Galatee.Silverlight.ServiceScelles;
using Galatee.Silverlight.Resources.Scelles;
using Galatee.Silverlight.Tarification.Helper;

namespace Galatee.Silverlight.Scelles
{
    public partial class UctrlListeRetoursScelles : ChildWindow
    {
        public CsRetourScelles ObjetSelectionne { get; set; }
        ObservableCollection<CsRetourScelles> donnesDatagrid = new ObservableCollection<CsRetourScelles>();
        //List<CsRetourScelles> listForInsertOrUpdate = new List<CsRetourScelles>();
        //List<CsRetourScelles> Listgrid = new List<CsRetourScelles>();

        //UcRetourScelle Newfrm = new UcRetourScelle();

        public UctrlListeRetoursScelles()
        {
            try
            {
                InitializeComponent();
                GetData();
            }
            catch (Exception ex)
            {
                
          Message.ShowError(ex.Message, Languages.LibelleCodeDepart);

            }
        }
        private void RafraichirList(object sender, EventArgs e)
        {
            GetData();
        }
        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion
        public ObservableCollection<CsRetourScelles> DonnesDatagrid
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

       

     
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeASupprimer);
                if (dtgrdRetourScelle.SelectedItems.Count > 0)
                {
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleReceptionScelle, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            if (dtgrdRetourScelle.SelectedItem != null)
                            {
                                var selected = dtgrdRetourScelle.SelectedItem as CsRetourScelles;

                                if (selected != null)
                                {
                                    IScelleServiceClient delete = new IScelleServiceClient(Utility.Protocole(),
                                                                                     Utility.EndPoint("Scelles"));
                                    delete.DeleteRetoursCompleted += (del, argDel) =>
                                    {
                                        if (argDel.Cancelled || argDel.Error != null)
                                        {
                                            Message.ShowError(argDel.Error.Message, Languages.LibelleReceptionScelle);
                                            return;
                                        }

                                        if (argDel.Result == false)
                                        {
                                            Message.ShowError(Languages.ErreurSuppressionDonnees, Languages.LibelleReceptionScelle);
                                            return;
                                        }
                                        donnesDatagrid.Remove(selected);
                                    };
                                    delete.DeleteRetoursAsync(selected);
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                    };
                    messageBox.Show();
                }
                else
                {
                    throw new Exception(Languages.SelectionnerUnElement);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleCodeDepart);
            }
        }

        private void GetData()
        {
            try
            {
                //UcRetourScelle ctrl = new UcRetourScelle();
                //ctrl.Closed += new EventHandler(RafraichirList);
                //ctrl.Show();
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.SelectAllRetourScelleCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.LibelleReceptionScelle);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var item in args.Result)
                        {
                            if (item.Date_Retour.Date == DateTime.Now.Date)
                            donnesDatagrid.Add(item);
                        }
                    dtgrdRetourScelle.ItemsSource = donnesDatagrid;
                };
                client.SelectAllRetourScelleAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Newfrm_CallBack(object sender, CustumEventArgs e)
        {

            GetData();
            //LoadDatagrid();
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            //UcRetourScelle Newfrm = new UcRetourScelle((CsLotMagasinGeneral)dtgrdRetourScelle.SelectedItem, SessionObject.ExecMode.Consultation, dtgrdRetourScelle); ;
            //Newfrm.CallBack += Newfrm_CallBack;
            //Newfrm.Show();
        }
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UcRetourScelle Newfrm = new UcRetourScelle(); ;
                Newfrm.CallBack += Newfrm_CallBack;
                Newfrm.Show();
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                //UcRetourScelle Updatefrm = new UcRetourScelle((CsLotMagasinGeneral)dtgrdRetourScelle.SelectedItem, SessionObject.ExecMode.Modification, dtgrdRetourScelle);
                ////Updatefrm.CallBack += Newfrm_CallBack;
                ////Updatefrm.Show();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}


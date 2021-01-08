using Galatee.Silverlight.ServiceFacturation;
using Galatee.Silverlight.ServiceTarification;
using Galatee.Silverlight.Tarification.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Tarification
{
    public partial class FrmListeVariableDeTarification : ChildWindow
    {
        #region Services

        #region Load

        public void LoadAllVariableTarif()
        {

            try
            {
                if (SessionObject.ListeRechercheTarif.Count > 0)
                {
                    foreach (var item in SessionObject.ListeVariableTarif)
                        ListeVariableTarif.Add(item);
                    LoadDatagrid();
                }
                else
                {
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                    int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                    service.LoadAllVariableTarifAsync();
                    service.LoadAllVariableTarifCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {
                                    SessionObject.ListeVariableTarif = res.Result;
                                    foreach (var item in res.Result)
                                    {
                                        ListeVariableTarif.Add(item);
                                    }
                                    LoadDatagrid();
                                }
                                else
                                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                        "Erreur");
                            LoadingManager.EndLoading(handler);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    };

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDatagrid()
        {
            System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeVariableTarif);
            dgListeVariableTarif.ItemsSource = view;
            datapager.Source = view;
        }

        #endregion

        #region Update

        public void Save(List<CsVariableDeTarification> VariableTarifoUpdate, List<CsVariableDeTarification> VariableTarifoInserte, List<CsVariableDeTarification> VariableTarifoDelete)
        {
            try
            {
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                int handler = LoadingManager.BeginLoading("Mise à jour des données ...");
                service.SaveVariableTarifAsync(VariableTarifoUpdate, VariableTarifoInserte, VariableTarifoDelete);
                service.SaveVariableTarifCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                if (res.Result > 0)
                                {
                                    //Message.Show("Sauvegarde effectué avec succès",
                                    //"Info");
                                }
                                else
                                {
                                    Message.Show("Sauvegarde non effectuée avec succès, il se peut que vos modifications n'aient pas été prises en compte",
                                    "Info");
                                }
                            }
                            else
                                Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                    "Erreur");
                        LoadingManager.EndLoading(handler);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Variables

        ObservableCollection<CsVariableDeTarification> ListeVariableTarif = new ObservableCollection<CsVariableDeTarification>();

        public List<CsVariableDeTarification> VariableTarifoUpdate = new List<CsVariableDeTarification>();
        public List<CsVariableDeTarification> VariableTarifoInserte = new List<CsVariableDeTarification>();
        public List<CsVariableDeTarification> VariableTarifoDelete = new List<CsVariableDeTarification>();

        FrmVariableTarif Newfrm = new FrmVariableTarif();

        #endregion

        #region EventHandler

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);

        }

        void Updatefrm_CallBack(object sender, CustumEventArgs e)
        {
            List<CsVariableDeTarification> VariableTarifoUpdate = new List<CsVariableDeTarification>();
            VariableTarifoUpdate.Add((CsVariableDeTarification)e.Bag);
            Save(VariableTarifoUpdate, new List<CsVariableDeTarification>(), new List<CsVariableDeTarification>());

                if (VariableTarifoUpdate.Select(l => l.PK_ID).Contains(((CsVariableDeTarification)e.Bag).PK_ID))
                {
                    //CsVariableDeTarification LotsScelleRecuToUpdate_ = VariableTarifoUpdate.FirstOrDefault(l => l.PK_ID == ((CsVariableDeTarification)e.Bag).PK_ID);
                    //int indexOfLotsScelleRecuToUpdate_ = VariableTarifoUpdate.IndexOf(LotsScelleRecuToUpdate_);
                    //VariableTarifoUpdate[indexOfLotsScelleRecuToUpdate_] = (CsVariableDeTarification)e.Bag;

                    CsVariableDeTarification LotsScelleRecuToUpdate = ListeVariableTarif.FirstOrDefault(l => l.PK_ID == ((CsVariableDeTarification)e.Bag).PK_ID);
                    int indexOfLotsScelleRecuToUpdate = ListeVariableTarif.IndexOf(LotsScelleRecuToUpdate);
                    ListeVariableTarif[indexOfLotsScelleRecuToUpdate] = (CsVariableDeTarification)e.Bag;

                    SessionObject.ListeVariableTarif = ListeVariableTarif.ToList();
                }
                else
                {
                    VariableTarifoUpdate.Add((CsVariableDeTarification)e.Bag);
                    Save(VariableTarifoUpdate, new List<CsVariableDeTarification>(), new List<CsVariableDeTarification>());


                    CsVariableDeTarification LotsScelleRecuToUpdate = ListeVariableTarif.FirstOrDefault(l => l.PK_ID == ((CsVariableDeTarification)e.Bag).PK_ID);
                    int indexOfLotsScelleRecuToUpdate = ListeVariableTarif.IndexOf(LotsScelleRecuToUpdate);
                    ListeVariableTarif[indexOfLotsScelleRecuToUpdate] = (CsVariableDeTarification)e.Bag;

                    SessionObject.ListeVariableTarif = ListeVariableTarif.ToList();
                }
                LoadDatagrid();
        }
        void Newfrm_CallBack(object sender, CustumEventArgs e)
        {
            List<CsVariableDeTarification> VariableTarifoInserte = new List<CsVariableDeTarification>();
            VariableTarifoInserte.Add((CsVariableDeTarification)e.Bag);
            Save(new List<CsVariableDeTarification>(), VariableTarifoInserte, new List<CsVariableDeTarification>());


            ListeVariableTarif.Add((CsVariableDeTarification)e.Bag);

            SessionObject.ListeVariableTarif = ListeVariableTarif.ToList();
            LoadDatagrid();
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            new FrmVariableTarif((CsVariableDeTarification)dgListeVariableTarif.SelectedItem).Show();
        }
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            FrmVariableTarif Newfrm = new FrmVariableTarif();
            Newfrm.CallBack += Newfrm_CallBack;
            Newfrm.Show();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            FrmVariableTarif Updatefrm = new FrmVariableTarif((CsVariableDeTarification)dgListeVariableTarif.SelectedItem, true);
            CsVariableDeTarification VariableTarif = ((CsVariableDeTarification)dgListeVariableTarif.SelectedItem);
            //On verifie que l'element actuelement selectionner a jamais été insérer en base en s'assurant que PK_ID est different de 0
            if (VariableTarif.PK_ID != 0)
            {
                Updatefrm.CallBack += Updatefrm_CallBack;
            }
            else
            {
                //Sinon on la considere comme une nouvelle insertion
                Updatefrm.CallBack += Newfrm_CallBack;
                ListeVariableTarif.Remove(VariableTarif);
                VariableTarifoInserte.Remove(VariableTarif);
            }
            Updatefrm.Show();
           
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    CsVariableDeTarification LotsScelleRecuToDelete = (CsVariableDeTarification)dgListeVariableTarif.SelectedItem;


                    List<CsVariableDeTarification> VariableTarifoDelete = new List<CsVariableDeTarification>();
                    VariableTarifoDelete.Add(LotsScelleRecuToDelete);
                    Save(new List<CsVariableDeTarification>(), new List<CsVariableDeTarification>(), VariableTarifoDelete);


                    ListeVariableTarif.Remove(LotsScelleRecuToDelete);

                    SessionObject.ListeVariableTarif = ListeVariableTarif.ToList();
                    LoadDatagrid();
                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }

        private void ChildWindow_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (VariableTarifoUpdate.Count > 0 || VariableTarifoInserte.Count > 0 || VariableTarifoDelete.Count > 0)
            //{
            //    Save(VariableTarifoUpdate, VariableTarifoInserte, VariableTarifoDelete);
            //}
        }

        #endregion

        #region Constructeurs

        public FrmListeVariableDeTarification()
        {
            InitializeComponent();
            InitializeComponentData();

            LoadAllVariableTarif();
        }

        #endregion

        #region Menue Contextuel

        ContextMenu ctxmn = null;
        MenuItem mnitem;

        void mnitem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnitem = (MenuItem)sender;

            switch (mnitem.Header.ToString())
            {
                case "Créer":
                    FrmVariableTarif Newfrm = new FrmVariableTarif();
                    Newfrm.CallBack += Newfrm_CallBack;
                    Newfrm.Show();
                    break;
                case "Consultation":
                    new FrmVariableTarif((CsVariableDeTarification)dgListeVariableTarif.SelectedItem).Show();
                    break;
                case "Modification":
                    FrmVariableTarif Updatefrm = new FrmVariableTarif((CsVariableDeTarification)dgListeVariableTarif.SelectedItem, true);
                    Updatefrm.CallBack += Updatefrm_CallBack;
                    Updatefrm.Show();
                    break;
                case "Supprimer":
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            CsVariableDeTarification LotsScelleRecuToDelete = (CsVariableDeTarification)dgListeVariableTarif.SelectedItem;
                            VariableTarifoDelete.Add(LotsScelleRecuToDelete);
                            ListeVariableTarif.Remove(LotsScelleRecuToDelete);
                        }
                        else
                        {
                            return;
                        }
                    };
                    messageBox.Show();
                    break;
                default:
                    break;
            }
        }

        void Row_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            dgListeVariableTarif.SelectedItem = ((sender) as DataGridRow).DataContext;
        }

        private void dgListeVariableTarif_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void dgListeVariableTarif_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ctxmn = new ContextMenu();

            mnitem = new MenuItem();
            mnitem.Header = "Créer";
            ctxmn.Items.Add(mnitem);
            mnitem.Click += new RoutedEventHandler(mnitem_Click);

            mnitem = new MenuItem();
            mnitem.Header = "Consultation";
            ctxmn.Items.Add(mnitem);
            mnitem.Click += new RoutedEventHandler(mnitem_Click);

            mnitem = new MenuItem();
            mnitem.Header = "Modification";
            ctxmn.Items.Add(mnitem);
            mnitem.Click += new RoutedEventHandler(mnitem_Click);

            mnitem = new MenuItem();
            mnitem.Header = "Supprimer";
            ctxmn.Items.Add(mnitem);
            mnitem.Click += new RoutedEventHandler(mnitem_Click);

            ctxmn.IsOpen = true;
            ctxmn.HorizontalOffset = e.GetPosition(null).X;
            ctxmn.VerticalOffset = e.GetPosition(null).Y;
        }

        private void dgListeVariableTarif_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseRightButtonDown += new MouseButtonEventHandler(Row_MouseRightButtonDown);
        }

        #endregion

        #region Methodes  d'interface

        private void InitializeComponentData()
        {
            this.dgListeVariableTarif.ItemsSource = ListeVariableTarif;
        }

        #endregion

    }
}


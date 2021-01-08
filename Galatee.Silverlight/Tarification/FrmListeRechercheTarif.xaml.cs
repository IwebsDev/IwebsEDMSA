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
    public partial class FrmListeRechercheTarif : ChildWindow
    {
        #region Services

        #region Load

        public void LoadAllRechercheTarif()
        {

            try
            {
                if (SessionObject.ListeRechercheTarif.Count > 0)
                {
                    foreach (var item in SessionObject.ListeRechercheTarif)
                    {
                        ListeRechercheTarif.Add(item);
                    }
                    LoadDatagraid();
                }
                else
                {
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                    int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                    service.LoadAllRechercheTarifAsync();
                    service.LoadAllRechercheTarifCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {
                                    SessionObject.ListeRechercheTarif = res.Result;
                                    foreach (var item in res.Result)
                                    {
                                        ListeRechercheTarif.Add(item);
                                    }
                                    LoadDatagraid();
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

        private void LoadDatagraid()
        {
            System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeRechercheTarif.OrderBy(r=>r.LIBELLE));
            dgListeRechercheTarif.ItemsSource = view;
            datapager.Source = view;
        }

        #endregion

        #region Update

        public void Save(List<CsRechercheTarif> RechercheTarifoUpdate, List<CsRechercheTarif> RechercheTarifoInserte, List<CsRechercheTarif> RechercheTarifoDelete)
        {
            try
            {
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                int handler = LoadingManager.BeginLoading("Mise à jour des données ...");
                service.SaveRechercheTarifAsync(RechercheTarifoUpdate, RechercheTarifoInserte, RechercheTarifoDelete);
                service.SaveRechercheTarifCompleted += (er, res) =>
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
                                    CsRechercheTarif RechercheTarif = ListeRechercheTarif.FirstOrDefault(l => l.PK_ID == 0);
                                    if (RechercheTarif != null)
                                    {
                                        int index = ListeRechercheTarif.IndexOf(RechercheTarif);
                                        RechercheTarif.PK_ID = res.Result;
                                        ListeRechercheTarif[index] = RechercheTarif;
                                    }
                                    LoadDatagraid();
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

        ObservableCollection<CsRechercheTarif> ListeRechercheTarif = new ObservableCollection<CsRechercheTarif>();

        public List<CsRechercheTarif> RechercheTarifoUpdate = new List<CsRechercheTarif>();
        public List<CsRechercheTarif> RechercheTarifoInserte = new List<CsRechercheTarif>();
        public List<CsRechercheTarif> RechercheTarifoDelete = new List<CsRechercheTarif>();

        FrmRechercheTarif Newfrm = new FrmRechercheTarif();

        #endregion

        #region EventHandler

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);

        }

        void Updatefrm_CallBack(object sender, CustumEventArgs e)
        {
            List<CsRechercheTarif> RechercheTarifoUpdate = new List<CsRechercheTarif>();
            RechercheTarifoUpdate.Add((CsRechercheTarif)e.Bag);
            Save(RechercheTarifoUpdate, new List<CsRechercheTarif>(), new List<CsRechercheTarif>());

                if (RechercheTarifoUpdate.Select(l => l.PK_ID).Contains(((CsRechercheTarif)e.Bag).PK_ID))
                {
                    //CsRechercheTarif LotsScelleRecuToUpdate_ = RechercheTarifoUpdate.FirstOrDefault(l => l.PK_ID == ((CsRechercheTarif)e.Bag).PK_ID);
                    //int indexOfLotsScelleRecuToUpdate_ = RechercheTarifoUpdate.IndexOf(LotsScelleRecuToUpdate_);
                    //RechercheTarifoUpdate[indexOfLotsScelleRecuToUpdate_] = (CsRechercheTarif)e.Bag;

                    CsRechercheTarif LotsScelleRecuToUpdate = ListeRechercheTarif.FirstOrDefault(l => l.PK_ID == ((CsRechercheTarif)e.Bag).PK_ID);
                    int indexOfLotsScelleRecuToUpdate = ListeRechercheTarif.IndexOf(LotsScelleRecuToUpdate);
                    ListeRechercheTarif[indexOfLotsScelleRecuToUpdate] = (CsRechercheTarif)e.Bag;

                    SessionObject.ListeRechercheTarif = ListeRechercheTarif.ToList();
                    LoadDatagraid();
                }
                else
                {
                    RechercheTarifoUpdate.Add((CsRechercheTarif)e.Bag);
                    Save(RechercheTarifoUpdate, new List<CsRechercheTarif>(), new List<CsRechercheTarif>());


                    CsRechercheTarif LotsScelleRecuToUpdate = ListeRechercheTarif.FirstOrDefault(l => l.PK_ID == ((CsRechercheTarif)e.Bag).PK_ID);
                    int indexOfLotsScelleRecuToUpdate = ListeRechercheTarif.IndexOf(LotsScelleRecuToUpdate);
                    ListeRechercheTarif[indexOfLotsScelleRecuToUpdate] = (CsRechercheTarif)e.Bag;

                    SessionObject.ListeRechercheTarif = ListeRechercheTarif.ToList();
                    LoadDatagraid();
                }
            

        }
        void Newfrm_CallBack(object sender, CustumEventArgs e)
        {
            List<CsRechercheTarif> RechercheTarifoInserte = new List<CsRechercheTarif>();
            RechercheTarifoInserte.Add((CsRechercheTarif)e.Bag);
            Save(new List<CsRechercheTarif>(), RechercheTarifoInserte, new List<CsRechercheTarif>());
            
            
            ListeRechercheTarif.Add((CsRechercheTarif)e.Bag);

            SessionObject.ListeRechercheTarif = ListeRechercheTarif.ToList();
            LoadDatagraid();

        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            new FrmRechercheTarif((CsRechercheTarif)dgListeRechercheTarif.SelectedItem).Show();
            //new FrmRechercheTarif((CsRechercheTarif)dgListeRechercheTarif.SelectedItem).Show();
        }
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            FrmRechercheTarif Newfrm = new FrmRechercheTarif();
            Newfrm.CallBack += Newfrm_CallBack;
            Newfrm.Show();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            FrmRechercheTarif Updatefrm = new FrmRechercheTarif((CsRechercheTarif)dgListeRechercheTarif.SelectedItem, true);
            CsRechercheTarif RechercheTarif = ((CsRechercheTarif)dgListeRechercheTarif.SelectedItem);
            //On verifie que l'element actuelement selectionner a jamais été insérer en base en s'assurant que PK_ID est different de 0
            if (RechercheTarif.PK_ID!=0)
            {
                Updatefrm.CallBack += Updatefrm_CallBack;
            }
            else
            {
                //Sinon on la considere comme une nouvelle insertion
                Updatefrm.CallBack += Newfrm_CallBack;
                ListeRechercheTarif.Remove(RechercheTarif);
                RechercheTarifoInserte.Remove(RechercheTarif);
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
                    CsRechercheTarif LotsScelleRecuToDelete = (CsRechercheTarif)dgListeRechercheTarif.SelectedItem;

                    
                    List<CsRechercheTarif> RechercheTarifoDelete = new List<CsRechercheTarif>();
                    RechercheTarifoDelete.Add(LotsScelleRecuToDelete);
                    Save(new List<CsRechercheTarif>(), new List<CsRechercheTarif>(), RechercheTarifoDelete);


                    ListeRechercheTarif.Remove(LotsScelleRecuToDelete);

                    SessionObject.ListeRechercheTarif = ListeRechercheTarif.ToList();
                    LoadDatagraid();

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
            //if (RechercheTarifoUpdate.Count > 0 || RechercheTarifoInserte.Count > 0 || RechercheTarifoDelete.Count > 0)
            //{
            //    Save(RechercheTarifoUpdate, RechercheTarifoInserte, RechercheTarifoDelete);
            //}
        }

        #endregion

        #region Constructeurs

        public FrmListeRechercheTarif()
        {
            InitializeComponent();
            InitializeComponentData();

            LoadAllRechercheTarif();
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
                    FrmRechercheTarif Newfrm = new FrmRechercheTarif();
                    Newfrm.CallBack += Newfrm_CallBack;
                    Newfrm.Show();
                    break;
                case "Consultation":
                    new FrmRechercheTarif((CsRechercheTarif)dgListeRechercheTarif.SelectedItem).Show();
                    break;
                case "Modification":
                    FrmRechercheTarif Updatefrm = new FrmRechercheTarif((CsRechercheTarif)dgListeRechercheTarif.SelectedItem, true);
                    Updatefrm.CallBack += Updatefrm_CallBack;
                    Updatefrm.Show();
                    break;
                case "Supprimer":
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            CsRechercheTarif LotsScelleRecuToDelete = (CsRechercheTarif)dgListeRechercheTarif.SelectedItem;
                            RechercheTarifoDelete.Add(LotsScelleRecuToDelete);
                            ListeRechercheTarif.Remove(LotsScelleRecuToDelete);
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
            dgListeRechercheTarif.SelectedItem = ((sender) as DataGridRow).DataContext;
        }

        private void dgListeRechercheTarif_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void dgListeRechercheTarif_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
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

        private void dgListeRechercheTarif_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseRightButtonDown += new MouseButtonEventHandler(Row_MouseRightButtonDown);
        }

        #endregion

        #region Methodes  d'interface

        private void InitializeComponentData()
        {
            this.dgListeRechercheTarif.ItemsSource = ListeRechercheTarif;
        }

        #endregion

    }
}


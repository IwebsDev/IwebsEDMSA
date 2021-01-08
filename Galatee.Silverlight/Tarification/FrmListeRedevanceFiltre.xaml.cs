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
    public partial class FrmListeRedevanceFiltre : ChildWindow
    {
        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();

        protected virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion

        #region Services

        #region Load

        public void LoadAllRedevance()
        {

            try
            {
                //if (SessionObject.ListeRedevence.Count > 0)
                //{
                //    foreach (var item in SessionObject.ListeRedevence)
                //    {
                //        ListeRedevence.Add(item);
                //    }

                //    cbo_produit.DisplayMemberPath = "LIBELLE";
                //    cbo_produit.SelectedValuePath = "PK_ID";
                //    cbo_produit.ItemsSource = ListeRedevence.Select(r => new { PK_ID = r.FK_IDPRODUIT, LIBELLE = r.LIBELLEPRODUIT }).Distinct().ToList();


                //    cbo_redevence.DisplayMemberPath = "LIBELLE";
                //    cbo_redevence.SelectedValuePath = "PK_ID";
                //    cbo_redevence.ItemsSource = ListeRedevence.Select(r => new { PK_ID = r.PK_ID, LIBELLE = r.LIBELLE }).Distinct().ToList();
                   
                //    ListeRedevenceTemp = ListeRedevence.ToList();
                //    LoadDatagrid();
                //}
                //else
                //{
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                    int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                    service.LoadAllRedevanceAsync();
                    service.LoadAllRedevanceCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {
                                    SessionObject.ListeRedevence = res.Result;
                                    foreach (var item in SessionObject.ListeRedevence)
                                    {
                                        ListeRedevence.Add(item);
                                    }

                                    cbo_produit.DisplayMemberPath = "LIBELLE";
                                    cbo_produit.SelectedValuePath = "PK_ID";
                                    cbo_produit.ItemsSource = ListeRedevence.Select(r => new { PK_ID = r.FK_IDPRODUIT, LIBELLE = r.LIBELLEPRODUIT }).Distinct().ToList();


                                    cbo_redevence.DisplayMemberPath = "LIBELLE";
                                    cbo_redevence.SelectedValuePath = "PK_ID";
                                    cbo_redevence.ItemsSource = ListeRedevence.Select(r => new { LIBELLE = r.LIBELLE }).Distinct().ToList();

                                    ListeRedevenceTemp = ListeRedevence.ToList();

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

                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDatagrid()
        {
            SessionObject.ListeRedevence = ListeRedevenceTemp.ToList();
            System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeRedevenceTemp.OrderBy(r => r.LIBELLE));
            dgListeRedevence.ItemsSource = view;
            datapager.Source = view;
        }

        #endregion

        #region Update

        public void Save(List<CsRedevance> RedevanceRecuToUpdate, List<CsRedevance> RedevanceRecuToInserte, List<CsRedevance> RedevanceRecuToDelete)
        {
            try
            {
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                int handler = LoadingManager.BeginLoading("Mise à jour des données ...");
                service.SaveRedevanceAsync(RedevanceRecuToUpdate, RedevanceRecuToInserte, RedevanceRecuToDelete);
                service.SaveRedevanceCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                if (res.Result>0)
                                {
                                    CsRedevance Redevance = ListeRedevence.FirstOrDefault(l => l.PK_ID == 0);
                                    if (Redevance!=null)
                                    {
                                        int index = ListeRedevence.IndexOf(Redevance);
                                        Redevance.PK_ID = res.Result;
                                        ListeRedevence[index] = Redevance;
                                    }

                                    LoadDatagrid();
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

        ObservableCollection<CsRedevance> ListeRedevence = new ObservableCollection<CsRedevance>();
        List<CsRedevance> ListeRedevenceTemp = new List<CsRedevance>();

        public List<CsRedevance> RedevanceRecuToUpdate = new List<CsRedevance>();
        public List<CsRedevance> RedevanceRecuToInserte = new List<CsRedevance>();
        public List<CsRedevance> RedevanceRecuToDelete = new List<CsRedevance>();
        public CsRedevance MyObject = new CsRedevance();
        FrmRedevance Newfrm = new FrmRedevance();

        #endregion

        #region EventHandler

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ListeRedevenceTemp = ListeRedevence.ToList();
            if (cbo_produit.SelectedItem != null)
            {
                dynamic Produit = cbo_produit.SelectedItem;
                ListeRedevenceTemp = ListeRedevenceTemp.Where(r => r.LIBELLEPRODUIT == Produit.LIBELLE).ToList();
            }
            if (cbo_redevence.SelectedItem!=null)
            {
                dynamic Redevence=cbo_redevence.SelectedItem;
                ListeRedevenceTemp = ListeRedevenceTemp.Where(r => r.LIBELLE == Redevence.LIBELLE).ToList();
            }
            LoadDatagrid();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.MyEventArg.Data = ((CsRedevance)dgListeRedevence.SelectedItem);
            OnEvent(MyEventArg);

            this.DialogResult = true;
        }

        void Updatefrm_CallBack(object sender, CustumEventArgs e)
        {
            List<CsRedevance> RedevanceRecuToUpdate = new List<CsRedevance>();
            RedevanceRecuToUpdate.Add((CsRedevance)e.Bag);
            Save(RedevanceRecuToUpdate, new List<CsRedevance>(), new List<CsRedevance>());

            if (RedevanceRecuToUpdate.Select(l => l.PK_ID).Contains(((CsRedevance)e.Bag).PK_ID))
            {
                //CsRedevance LotsScelleRecuToUpdate_ = RedevanceRecuToUpdate.FirstOrDefault(l => l.PK_ID == ((CsRedevance)e.Bag).PK_ID);
                //int indexOfLotsScelleRecuToUpdate_ = RedevanceRecuToUpdate.IndexOf(LotsScelleRecuToUpdate_);
                //RedevanceRecuToUpdate[indexOfLotsScelleRecuToUpdate_] = (CsRedevance)e.Bag;

                CsRedevance LotsScelleRecuToUpdate = ListeRedevence.FirstOrDefault(l => l.PK_ID == ((CsRedevance)e.Bag).PK_ID);
                int indexOfLotsScelleRecuToUpdate = ListeRedevence.IndexOf(LotsScelleRecuToUpdate);
                ListeRedevence[indexOfLotsScelleRecuToUpdate] = (CsRedevance)e.Bag;

                SessionObject.ListeRedevence = ListeRedevence.ToList();
                LoadDatagrid();
            }
            else
            {
                RedevanceRecuToUpdate.Add((CsRedevance)e.Bag);
                Save(RedevanceRecuToUpdate, new List<CsRedevance>(), new List<CsRedevance>());


                CsRedevance LotsScelleRecuToUpdate = ListeRedevence.FirstOrDefault(l => l.PK_ID == ((CsRedevance)e.Bag).PK_ID);
                int indexOfLotsScelleRecuToUpdate = ListeRedevence.IndexOf(LotsScelleRecuToUpdate);
                ListeRedevence[indexOfLotsScelleRecuToUpdate] = (CsRedevance)e.Bag;

                SessionObject.ListeRedevence = ListeRedevence.ToList();
            }

        }
        void Newfrm_CallBack(object sender, CustumEventArgs e)
        {
            this.IsEnabled = true;
            List<CsRedevance> RedevanceRecuToInserte = new List<CsRedevance>();
            RedevanceRecuToInserte.Add((CsRedevance)e.Bag);
            Save(new List<CsRedevance>(), RedevanceRecuToInserte, new List<CsRedevance>());


            ListeRedevence.Add((CsRedevance)e.Bag);

            SessionObject.ListeRedevence = ListeRedevence.ToList();
            LoadDatagrid();
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            new FrmRedevance((CsRedevance)dgListeRedevence.SelectedItem).Show();
            //new FrmRedevance((CsRedevance)dgListeRedevence.SelectedItem).Show();
        }
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            FrmRedevance Newfrm = new FrmRedevance();
            Newfrm.CallBack += Newfrm_CallBack;
            Newfrm.Show();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            FrmRedevance Updatefrm = new FrmRedevance((CsRedevance)dgListeRedevence.SelectedItem, true);
            CsRedevance Redevance = ((CsRedevance)dgListeRedevence.SelectedItem);
            //On verifie que l'element actuelement selectionner a jamais été insérer en base en s'assurant que PK_ID est different de 0
            if (Redevance.PK_ID != 0)
            {
                Updatefrm.CallBack += Updatefrm_CallBack;
            }
            else
            {
                //Sinon on la considere comme une nouvelle insertion
                Updatefrm.CallBack += Newfrm_CallBack;
                ListeRedevence.Remove(Redevance);
                RedevanceRecuToInserte.Remove(Redevance);
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
                    CsRedevance redevenceSelectionne = (CsRedevance)dgListeRedevence.SelectedItem;


                    List<CsRedevance> RedevanceRecuToDelete = new List<CsRedevance>();
                    RedevanceRecuToDelete.Add(redevenceSelectionne);
                    Save(new List<CsRedevance>(), new List<CsRedevance>(), RedevanceRecuToDelete);


                    ListeRedevence.Remove(redevenceSelectionne);

                    SessionObject.ListeRedevence = ListeRedevence.ToList();
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
            if (RedevanceRecuToUpdate.Count > 0 || RedevanceRecuToInserte.Count > 0 || RedevanceRecuToDelete.Count > 0)
            {
                Save(RedevanceRecuToUpdate, RedevanceRecuToInserte, RedevanceRecuToDelete);
            }
        }

        #endregion

        #region Constructeurs

        public FrmListeRedevanceFiltre()
        {
            InitializeComponent();
            InitializeComponentData();

            LoadAllRedevance();
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
                    FrmRedevance Newfrm = new FrmRedevance();
                    Newfrm.CallBack += Newfrm_CallBack;
                    this.IsEnabled = false;
                    Newfrm.Show();
                    break;
                case "Consultation":
                    new FrmRedevance((CsRedevance)dgListeRedevence.SelectedItem).Show();
                    break;
                case "Modification":
                    FrmRedevance Updatefrm = new FrmRedevance((CsRedevance)dgListeRedevence.SelectedItem, true);
                    Updatefrm.CallBack += Updatefrm_CallBack;
                    Updatefrm.Show();
                    break;
                case "Supprimer":
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            CsRedevance LotsScelleRecuToDelete = (CsRedevance)dgListeRedevence.SelectedItem;
                            RedevanceRecuToDelete.Add(LotsScelleRecuToDelete);
                            ListeRedevence.Remove(LotsScelleRecuToDelete);
                            LoadDatagrid();
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
            dgListeRedevence.SelectedItem = ((sender) as DataGridRow).DataContext;
        }

        private void dgListeRedevence_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void dgListeRedevence_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
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

        private void dgListeRedevence_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseRightButtonDown += new MouseButtonEventHandler(Row_MouseRightButtonDown);
        }

        #endregion

        #region Methodes  d'interface

        private void InitializeComponentData()
        {
            this.dgListeRedevence.ItemsSource = ListeRedevence.OrderBy(r=>r.LIBELLE);
        }

        #endregion

      
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ListeRedevenceTemp = ListeRedevence.ToList();
            LoadDatagrid();
        }

        private void ChildWindow_Closed_1(object sender, EventArgs e)
        {
           
        }

    }
}


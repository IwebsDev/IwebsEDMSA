using Galatee.Silverlight.ServiceRecouvrement;
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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmListSaisieAjustement : ChildWindow
    {
        #region Services

        #region Load

        public void LoadAllAjustement()
        {

            try
            {
                ListeAjustement = new ObservableCollection<CsLotComptClient>();
                //if (SessionObject.ListeAjustement.Count > 0)
                //{
                //    foreach (var item in SessionObject.ListeAjustement)
                //    {
                //        ListeAjustement.Add(item);
                //    }
                //    LoadDatagrid();
                //}
                //else
                //{
                    RecouvrementServiceClient service = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                    service.LoadAllAjustementAsync();
                    service.LoadAllAjustementCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null)
                                {
                                    SessionObject.ListeAjustement = res.Result;
                                    foreach (var item in SessionObject.ListeAjustement)
                                    {
                                        ListeAjustement.Add(item);
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

                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDatagrid()
        {
            SessionObject.ListeAjustement = ListeAjustement.ToList();
            System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeAjustement.OrderBy(r => r.NUMEROLOT));
            dgListeAjustement.ItemsSource = view;
            datapager.Source = view;
        }

        #endregion

        #region Update

        public void Save(List<CsLotComptClient> AjustementRecuToUpdate, List<CsLotComptClient> AjustementRecuToInserte, List<CsLotComptClient> AjustementRecuToDelete)
        {
            try
            {
                RecouvrementServiceClient service = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                int handler = LoadingManager.BeginLoading("Mise à jour des données ...");
                service.SaveAjustementAsync(AjustementRecuToUpdate, AjustementRecuToInserte, AjustementRecuToDelete);
                service.SaveAjustementCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement des méthodes de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                if (res.Result>0)
                                {
                                    CsLotComptClient Ajustement = ListeAjustement.FirstOrDefault(l => l.PK_ID == 0);
                                    if (Ajustement!=null)
                                    {
                                        int index = ListeAjustement.IndexOf(Ajustement);
                                        Ajustement.PK_ID = res.Result;
                                        Ajustement.DetaiLot.ForEach(c=>c.FK_IDLOTCOMPECLIENT=res.Result);
                                        
                                        ListeAjustement[index] = Ajustement;
                                    }

                                    LoadDatagrid();
                                    LoadAllAjustement();
                                }
                                else
                                {
                                    Message.Show("Sauvegarde non effectuée avec succès,il se peut que vos modifications n'aient pas été prises en compte",
                                    "Info");
                                }
                            }
                            else
                                Message.Show("Une erreur s'est produite, veuillez consulter le journal des erreurs",
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

        ObservableCollection<CsLotComptClient> ListeAjustement = new ObservableCollection<CsLotComptClient>();

        public List<CsLotComptClient> AjustementRecuToUpdate = new List<CsLotComptClient>();
        public List<CsLotComptClient> AjustementRecuToInserte = new List<CsLotComptClient>();
        public List<CsLotComptClient> AjustementRecuToDelete = new List<CsLotComptClient>();

        FrmSaisieAjustement Newfrm = new FrmSaisieAjustement();

        #endregion

        #region EventHandler

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);

        }

        void Updatefrm_CallBack(object sender, CustumEventArgs e)
        {
            List<CsLotComptClient> AjustementRecuToUpdate = new List<CsLotComptClient>();
            AjustementRecuToUpdate.Add((CsLotComptClient)e.Bag);
            Save(AjustementRecuToUpdate, new List<CsLotComptClient>(), new List<CsLotComptClient>());

            if (AjustementRecuToUpdate.Select(l => l.PK_ID).Contains(((CsLotComptClient)e.Bag).PK_ID))
            {
                //CsLotComptClient LotsScelleRecuToUpdate_ = AjustementRecuToUpdate.FirstOrDefault(l => l.PK_ID == ((CsLotComptClient)e.Bag).PK_ID);
                //int indexOfLotsScelleRecuToUpdate_ = AjustementRecuToUpdate.IndexOf(LotsScelleRecuToUpdate_);
                //AjustementRecuToUpdate[indexOfLotsScelleRecuToUpdate_] = (CsLotComptClient)e.Bag;

                CsLotComptClient LotsScelleRecuToUpdate = ListeAjustement.FirstOrDefault(l => l.PK_ID == ((CsLotComptClient)e.Bag).PK_ID);
                int indexOfLotsScelleRecuToUpdate = ListeAjustement.IndexOf(LotsScelleRecuToUpdate);
                ListeAjustement[indexOfLotsScelleRecuToUpdate] = (CsLotComptClient)e.Bag;

                SessionObject.ListeAjustement = ListeAjustement.ToList();
                LoadDatagrid();
            }
            else
            {
                AjustementRecuToUpdate.Add((CsLotComptClient)e.Bag);
                Save(AjustementRecuToUpdate, new List<CsLotComptClient>(), new List<CsLotComptClient>());


                CsLotComptClient LotsScelleRecuToUpdate = ListeAjustement.FirstOrDefault(l => l.PK_ID == ((CsLotComptClient)e.Bag).PK_ID);
                int indexOfLotsScelleRecuToUpdate = ListeAjustement.IndexOf(LotsScelleRecuToUpdate);
                ListeAjustement[indexOfLotsScelleRecuToUpdate] = (CsLotComptClient)e.Bag;

                SessionObject.ListeAjustement = ListeAjustement.ToList();
            }

        }
        void Newfrm_CallBack(object sender, CustumEventArgs e)
        {
            this.IsEnabled = true;
            if (e.Bag!=null)
            {
                List<CsLotComptClient> AjustementRecuToInserte = new List<CsLotComptClient>();
                AjustementRecuToInserte.Add((CsLotComptClient)e.Bag);
                Save(new List<CsLotComptClient>(), AjustementRecuToInserte, new List<CsLotComptClient>());


                ListeAjustement.Add((CsLotComptClient)e.Bag);

                SessionObject.ListeAjustement = ListeAjustement.ToList();
                LoadDatagrid();
            }
            else
            {
                LoadAllAjustement();
            }
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            new FrmSaisieAjustement((CsLotComptClient)dgListeAjustement.SelectedItem).Show();
            //new FrmAjustement((CsLotComptClient)dgListeAjustement.SelectedItem).Show();
        }
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            FrmSaisieAjustement Newfrm = new FrmSaisieAjustement();
            Newfrm.CallBack += Newfrm_CallBack;
            Newfrm.Closed += Newfrm_Closed;
            this.IsEnabled = false;
            Newfrm.Show();
        }

        void Newfrm_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            FrmSaisieAjustement Updatefrm = new FrmSaisieAjustement((CsLotComptClient)dgListeAjustement.SelectedItem, false);
            CsLotComptClient Ajustement = ((CsLotComptClient)dgListeAjustement.SelectedItem);
            //On verifie que l'element actuelement selectionner a jamais été insérer en base en s'assurant que PK_ID est different de 0
            if (Ajustement.PK_ID != 0)
            {
                Updatefrm.CallBack += Updatefrm_CallBack;
            }
            else
            {
                //Sinon on la considere comme une nouvelle insertion
                Updatefrm.CallBack += Newfrm_CallBack;
                ListeAjustement.Remove(Ajustement);
                AjustementRecuToInserte.Remove(Ajustement);
            }
            Updatefrm.Closed += Updatefrm_Closed;
            this.IsEnabled = false;
            Updatefrm.Show();
        }

        void Updatefrm_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    CsLotComptClient redevenceSelectionne = (CsLotComptClient)dgListeAjustement.SelectedItem;


                    List<CsLotComptClient> AjustementRecuToDelete = new List<CsLotComptClient>();
                    AjustementRecuToDelete.Add(redevenceSelectionne);
                    Save(new List<CsLotComptClient>(), new List<CsLotComptClient>(), AjustementRecuToDelete);


                    ListeAjustement.Remove(redevenceSelectionne);

                    SessionObject.ListeAjustement = ListeAjustement.ToList();
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
            if (AjustementRecuToUpdate.Count > 0 || AjustementRecuToInserte.Count > 0 || AjustementRecuToDelete.Count > 0)
            {
                Save(AjustementRecuToUpdate, AjustementRecuToInserte, AjustementRecuToDelete);
            }
        }

        #endregion

        #region Constructeurs

        public FrmListSaisieAjustement()
        {
            InitializeComponent();
            InitializeComponentData();

            LoadAllAjustement();
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
                    FrmSaisieAjustement Newfrm = new FrmSaisieAjustement();
                    Newfrm.CallBack += Newfrm_CallBack;
                    this.IsEnabled = false;
                    Newfrm.Show();
                    break;
                case "Consultation":
                    new FrmSaisieAjustement((CsLotComptClient)dgListeAjustement.SelectedItem).Show();
                    break;
                case "Modification":
                    FrmSaisieAjustement Updatefrm = new FrmSaisieAjustement((CsLotComptClient)dgListeAjustement.SelectedItem, false);
                    Updatefrm.CallBack += Updatefrm_CallBack;
                    Updatefrm.Show();
                    break;
                case "Supprimer":
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            CsLotComptClient LotsScelleRecuToDelete = (CsLotComptClient)dgListeAjustement.SelectedItem;
                            LotsScelleRecuToDelete.STATUS = "0";
                            AjustementRecuToDelete.Add(LotsScelleRecuToDelete);
                            ListeAjustement.Remove(LotsScelleRecuToDelete);
                            Save(AjustementRecuToDelete,new List<CsLotComptClient>(), new List<CsLotComptClient>());
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
            dgListeAjustement.SelectedItem = ((sender) as DataGridRow).DataContext;
        }

        private void dgListeAjustement_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void dgListeAjustement_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
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

        private void dgListeAjustement_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseRightButtonDown += new MouseButtonEventHandler(Row_MouseRightButtonDown);
        }

        #endregion

        #region Methodes  d'interface

        private void InitializeComponentData()
        {
            this.dgListeAjustement.ItemsSource = ListeAjustement.OrderBy(r=>r.NUMEROLOT);
        }

        #endregion
    }
}


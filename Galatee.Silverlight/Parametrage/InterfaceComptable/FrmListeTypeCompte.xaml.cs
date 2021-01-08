using Galatee.Silverlight.ServiceInterfaceComptable;
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

namespace Galatee.Silverlight.Parametrage
{
    public partial class FrmListeTypeCompteCompte : ChildWindow
    {
        #region Services

        #region Load

        public void LoadAllTypeCompte()
        {

            try
            {
                InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                service.RetourneTypeCompteCompleted += (s, args) =>
                    {
                        try
                        {
                            if (args.Cancelled || args.Error != null || args.Result == null)
                            {
                                string error = args.Error.InnerException.ToString();
                                return;
                            }
                            else
                                if (args.Result != null && args.Result.Count != 0)
                                {
                                    args.Result.ForEach(t=>ListeTypeCompte.Add(t));
                                    LoadDatagrid();
                                }
                                else
                                {
                                    Message.ShowInformation("Une erreur s'est produite, veuillez consultez le journal des erreurs", "Avertissement");
                                }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    };
                service.RetourneTypeCompteAsync();    


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDatagrid()
        {
            //SessionObject.ListeTypeCompte = ListeTypeCompte.ToList();
            System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeTypeCompte.OrderBy(r => r.LIBELLE));
            dgListeTypeCompte.ItemsSource = view;
            datapager.Source = view;
        }

        #endregion

        #region Update

        public void Save(List<CsTypeCompte> TypeCompteRecuToUpdate, List<CsTypeCompte> TypeCompteRecuToInserte, List<CsTypeCompte> TypeCompteRecuToDelete)
        {
            try
            {
                InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                int handler = LoadingManager.BeginLoading("Mise à jour des données ...");
                service.SaveTypeCompteAsync(TypeCompteRecuToUpdate, TypeCompteRecuToInserte, TypeCompteRecuToDelete);
                service.SaveTypeCompteCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                if (res.Result > 0)
                                {
                                    CsTypeCompte TypeCompte = ListeTypeCompte.FirstOrDefault(l => l.PK_ID == 0);
                                    if (TypeCompte != null)
                                    {
                                        int index = ListeTypeCompte.IndexOf(TypeCompte);
                                        TypeCompte.PK_ID = res.Result;
                                        ListeTypeCompte[index] = TypeCompte;
                                    }

                                    LoadDatagrid();
                                }
                                else
                                {
                                    Message.Show("Sauvegarde non effectué avec succes,il se peut vos modification n'est pas été pris en conte",
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

        public ObservableCollection<CsTypeCompte> ListeTypeCompte = new ObservableCollection<CsTypeCompte>();

        public List<CsTypeCompte> TypeCompteRecuToUpdate = new List<CsTypeCompte>();
        public List<CsTypeCompte> TypeCompteRecuToInserte = new List<CsTypeCompte>();
        public List<CsTypeCompte> TypeCompteRecuToDelete = new List<CsTypeCompte>();

        FrmTypeCompteCompte Newfrm = new FrmTypeCompteCompte();

        #endregion

        #region EventHandler

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //if (TypeCompteRecuToUpdate.Count > 0 || TypeCompteRecuToInserte.Count > 0 || TypeCompteRecuToDelete.Count > 0)
            //{
            //    Save(TypeCompteRecuToUpdate, TypeCompteRecuToInserte, TypeCompteRecuToDelete);
            //}

            this.DialogResult = true;
        }

        void Updatefrm_CallBack(object sender, CustumEventArgs e)
        {
            List<CsTypeCompte> TypeCompteRecuToUpdate = new List<CsTypeCompte>();
            TypeCompteRecuToUpdate.Add((CsTypeCompte)e.Bag);
            Save(TypeCompteRecuToUpdate, new List<CsTypeCompte>(), new List<CsTypeCompte>());

            if (TypeCompteRecuToUpdate.Select(l => l.PK_ID).Contains(((CsTypeCompte)e.Bag).PK_ID))
            {
                //CsTypeCompte LotsScelleRecuToUpdate_ = TypeCompteRecuToUpdate.FirstOrDefault(l => l.PK_ID == ((CsTypeCompte)e.Bag).PK_ID);
                //int indexOfLotsScelleRecuToUpdate_ = TypeCompteRecuToUpdate.IndexOf(LotsScelleRecuToUpdate_);
                //TypeCompteRecuToUpdate[indexOfLotsScelleRecuToUpdate_] = (CsTypeCompte)e.Bag;

                CsTypeCompte LotsScelleRecuToUpdate = ListeTypeCompte.FirstOrDefault(l => l.PK_ID == ((CsTypeCompte)e.Bag).PK_ID);
                int indexOfLotsScelleRecuToUpdate = ListeTypeCompte.IndexOf(LotsScelleRecuToUpdate);
                ListeTypeCompte[indexOfLotsScelleRecuToUpdate] = (CsTypeCompte)e.Bag;

                //SessionObject.ListeTypeCompte = ListeTypeCompte.ToList();
                LoadDatagrid();
            }
            else
            {
                TypeCompteRecuToUpdate.Add((CsTypeCompte)e.Bag);
                Save(TypeCompteRecuToUpdate, new List<CsTypeCompte>(), new List<CsTypeCompte>());


                CsTypeCompte LotsScelleRecuToUpdate = ListeTypeCompte.FirstOrDefault(l => l.PK_ID == ((CsTypeCompte)e.Bag).PK_ID);
                int indexOfLotsScelleRecuToUpdate = ListeTypeCompte.IndexOf(LotsScelleRecuToUpdate);
                ListeTypeCompte[indexOfLotsScelleRecuToUpdate] = (CsTypeCompte)e.Bag;

                //SessionObject.ListeTypeCompte = ListeTypeCompte.ToList();
            }

        }
        void Newfrm_CallBack(object sender, CustumEventArgs e)
        {
            List<CsTypeCompte> TypeCompteRecuToInserte = new List<CsTypeCompte>();
            TypeCompteRecuToInserte.Add((CsTypeCompte)e.Bag);
            Save(new List<CsTypeCompte>(), TypeCompteRecuToInserte, new List<CsTypeCompte>());


            ListeTypeCompte.Add((CsTypeCompte)e.Bag);

            //SessionObject.ListeTypeCompte = ListeTypeCompte.ToList();
            LoadDatagrid();
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            new FrmTypeCompteCompte((CsTypeCompte)dgListeTypeCompte.SelectedItem).Show();
            //new FrmTypeCompteCompte((CsTypeCompte)dgListeTypeCompte.SelectedItem).Show();
        }
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            FrmTypeCompteCompte Newfrm = new FrmTypeCompteCompte();
            Newfrm.CallBack += Newfrm_CallBack;
            Newfrm.Show();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            FrmTypeCompteCompte Updatefrm = new FrmTypeCompteCompte((CsTypeCompte)dgListeTypeCompte.SelectedItem, true);
            CsTypeCompte TypeCompte = ((CsTypeCompte)dgListeTypeCompte.SelectedItem);
            //On verifie que l'element actuelement selectionner a jamais été insérer en base en s'assurant que PK_ID est different de 0
            if (TypeCompte.PK_ID != 0)
            {
                Updatefrm.CallBack += Updatefrm_CallBack;
            }
            else
            {
                //Sinon on la considere comme une nouvelle insertion
                Updatefrm.CallBack += Newfrm_CallBack;
                ListeTypeCompte.Remove(TypeCompte);
                TypeCompteRecuToInserte.Remove(TypeCompte);
            }
            Updatefrm.Show();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Ête-vous sure de vouloire supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    CsTypeCompte redevenceSelectionne = (CsTypeCompte)dgListeTypeCompte.SelectedItem;


                    List<CsTypeCompte> TypeCompteRecuToDelete = new List<CsTypeCompte>();
                    TypeCompteRecuToDelete.Add(redevenceSelectionne);
                    Save(new List<CsTypeCompte>(), new List<CsTypeCompte>(), TypeCompteRecuToDelete);


                    ListeTypeCompte.Remove(redevenceSelectionne);

                    //SessionObject.ListeTypeCompte = ListeTypeCompte.ToList();
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
            if (TypeCompteRecuToUpdate.Count > 0 || TypeCompteRecuToInserte.Count > 0 || TypeCompteRecuToDelete.Count > 0)
            {
                Save(TypeCompteRecuToUpdate, TypeCompteRecuToInserte, TypeCompteRecuToDelete);
            }
        }

        #endregion

        #region Constructeurs

        public FrmListeTypeCompteCompte()
        {
            InitializeComponent();
            InitializeComponentData();

            LoadAllTypeCompte();
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
                    FrmTypeCompteCompte Newfrm = new FrmTypeCompteCompte();
                    Newfrm.CallBack += Newfrm_CallBack;
                    Newfrm.Show();
                    break;
                case "Consultation":
                    new FrmTypeCompteCompte((CsTypeCompte)dgListeTypeCompte.SelectedItem).Show();
                    break;
                case "Modification":
                    FrmTypeCompteCompte Updatefrm = new FrmTypeCompteCompte((CsTypeCompte)dgListeTypeCompte.SelectedItem, true);
                    Updatefrm.CallBack += Updatefrm_CallBack;
                    Updatefrm.Show();
                    break;
                case "Supprimer":
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Ête-vous sure de vouloire supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            CsTypeCompte LotsScelleRecuToDelete = (CsTypeCompte)dgListeTypeCompte.SelectedItem;
                            TypeCompteRecuToDelete.Add(LotsScelleRecuToDelete);
                            ListeTypeCompte.Remove(LotsScelleRecuToDelete);
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
            dgListeTypeCompte.SelectedItem = ((sender) as DataGridRow).DataContext;
        }

        private void dgListeTypeCompte_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void dgListeTypeCompte_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
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

        private void dgListeTypeCompte_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseRightButtonDown += new MouseButtonEventHandler(Row_MouseRightButtonDown);
        }

        #endregion

        #region Methodes  d'interface

        private void InitializeComponentData()
        {
            this.dgListeTypeCompte.ItemsSource = ListeTypeCompte.OrderBy(r=>r.LIBELLE);
        }

        #endregion

    }
}


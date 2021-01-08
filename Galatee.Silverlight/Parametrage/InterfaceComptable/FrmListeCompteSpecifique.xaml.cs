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
    public partial class FrmListeCompteSpecifique : ChildWindow
    {
        #region Services

        #region Load

        public void LoadAllCompteSpecifique()
        {

            try
            {
                InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                service.RetourneCompteSpecifiqueCompleted += (s, args) =>
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
                                    args.Result.ForEach(t=>ListeCompteSpecifique.Add(t));
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
                service.RetourneCompteSpecifiqueAsync();    


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDatagrid()
        {
            //SessionObject.ListeCompteSpecifique = ListeCompteSpecifique.ToList();
            System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeCompteSpecifique.OrderBy(r => r.LIBELLE));
            dgListeCompteSpecifique.ItemsSource = view;
            datapager.Source = view;
        }

        #endregion

        #region Update

        public void Save(List<CsCompteSpecifique> CompteSpecifiqueRecuToUpdate, List<CsCompteSpecifique> CompteSpecifiqueRecuToInserte, List<CsCompteSpecifique> CompteSpecifiqueRecuToDelete)
        {
            try
            {
                InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                int handler = LoadingManager.BeginLoading("Mise à jour des données ...");
                service.SaveCompteSpecifiqueAsync(CompteSpecifiqueRecuToUpdate, CompteSpecifiqueRecuToInserte, CompteSpecifiqueRecuToDelete);
                service.SaveCompteSpecifiqueCompleted += (er, res) =>
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
                                    CsCompteSpecifique CompteSpecifique = ListeCompteSpecifique.FirstOrDefault(l => l.PK_ID == 0);
                                    if (CompteSpecifique != null)
                                    {
                                        int index = ListeCompteSpecifique.IndexOf(CompteSpecifique);
                                        CompteSpecifique.PK_ID = res.Result;
                                        ListeCompteSpecifique[index] = CompteSpecifique;
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

        public ObservableCollection<CsCompteSpecifique> ListeCompteSpecifique = new ObservableCollection<CsCompteSpecifique>();

        public List<CsCompteSpecifique> CompteSpecifiqueRecuToUpdate = new List<CsCompteSpecifique>();
        public List<CsCompteSpecifique> CompteSpecifiqueRecuToInserte = new List<CsCompteSpecifique>();
        public List<CsCompteSpecifique> CompteSpecifiqueRecuToDelete = new List<CsCompteSpecifique>();

        FrmCompteSpecifique Newfrm = new FrmCompteSpecifique();

        #endregion

        #region EventHandler

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //if (CompteSpecifiqueRecuToUpdate.Count > 0 || CompteSpecifiqueRecuToInserte.Count > 0 || CompteSpecifiqueRecuToDelete.Count > 0)
            //{
            //    Save(CompteSpecifiqueRecuToUpdate, CompteSpecifiqueRecuToInserte, CompteSpecifiqueRecuToDelete);
            //}

            this.DialogResult = true;
        }

        void Updatefrm_CallBack(object sender, CustumEventArgs e)
        {
            List<CsCompteSpecifique> CompteSpecifiqueRecuToUpdate = new List<CsCompteSpecifique>();
            CompteSpecifiqueRecuToUpdate.Add((CsCompteSpecifique)e.Bag);
            Save(CompteSpecifiqueRecuToUpdate, new List<CsCompteSpecifique>(), new List<CsCompteSpecifique>());

            if (CompteSpecifiqueRecuToUpdate.Select(l => l.PK_ID).Contains(((CsCompteSpecifique)e.Bag).PK_ID))
            {
                //CsCompteSpecifique LotsScelleRecuToUpdate_ = CompteSpecifiqueRecuToUpdate.FirstOrDefault(l => l.PK_ID == ((CsCompteSpecifique)e.Bag).PK_ID);
                //int indexOfLotsScelleRecuToUpdate_ = CompteSpecifiqueRecuToUpdate.IndexOf(LotsScelleRecuToUpdate_);
                //CompteSpecifiqueRecuToUpdate[indexOfLotsScelleRecuToUpdate_] = (CsCompteSpecifique)e.Bag;

                CsCompteSpecifique LotsScelleRecuToUpdate = ListeCompteSpecifique.FirstOrDefault(l => l.PK_ID == ((CsCompteSpecifique)e.Bag).PK_ID);
                int indexOfLotsScelleRecuToUpdate = ListeCompteSpecifique.IndexOf(LotsScelleRecuToUpdate);
                ListeCompteSpecifique[indexOfLotsScelleRecuToUpdate] = (CsCompteSpecifique)e.Bag;

                //SessionObject.ListeCompteSpecifique = ListeCompteSpecifique.ToList();
                LoadDatagrid();
            }
            else
            {
                CompteSpecifiqueRecuToUpdate.Add((CsCompteSpecifique)e.Bag);
                Save(CompteSpecifiqueRecuToUpdate, new List<CsCompteSpecifique>(), new List<CsCompteSpecifique>());


                CsCompteSpecifique LotsScelleRecuToUpdate = ListeCompteSpecifique.FirstOrDefault(l => l.PK_ID == ((CsCompteSpecifique)e.Bag).PK_ID);
                int indexOfLotsScelleRecuToUpdate = ListeCompteSpecifique.IndexOf(LotsScelleRecuToUpdate);
                ListeCompteSpecifique[indexOfLotsScelleRecuToUpdate] = (CsCompteSpecifique)e.Bag;

                //SessionObject.ListeCompteSpecifique = ListeCompteSpecifique.ToList();
            }

        }
        void Newfrm_CallBack(object sender, CustumEventArgs e)
        {
            List<CsCompteSpecifique> CompteSpecifiqueRecuToInserte = new List<CsCompteSpecifique>();
            CompteSpecifiqueRecuToInserte.Add((CsCompteSpecifique)e.Bag);
            Save(new List<CsCompteSpecifique>(), CompteSpecifiqueRecuToInserte, new List<CsCompteSpecifique>());


            ListeCompteSpecifique.Add((CsCompteSpecifique)e.Bag);

            //SessionObject.ListeCompteSpecifique = ListeCompteSpecifique.ToList();
            LoadDatagrid();
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            new FrmCompteSpecifique((CsCompteSpecifique)dgListeCompteSpecifique.SelectedItem).Show();
            //new FrmCompteSpecifique((CsCompteSpecifique)dgListeCompteSpecifique.SelectedItem).Show();
        }
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            FrmCompteSpecifique Newfrm = new FrmCompteSpecifique();
            Newfrm.CallBack += Newfrm_CallBack;
            Newfrm.Show();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            FrmCompteSpecifique Updatefrm = new FrmCompteSpecifique((CsCompteSpecifique)dgListeCompteSpecifique.SelectedItem, true);
            CsCompteSpecifique CompteSpecifique = ((CsCompteSpecifique)dgListeCompteSpecifique.SelectedItem);
            //On verifie que l'element actuelement selectionner a jamais été insérer en base en s'assurant que PK_ID est different de 0
            if (CompteSpecifique.PK_ID != 0)
            {
                Updatefrm.CallBack += Updatefrm_CallBack;
            }
            else
            {
                //Sinon on la considere comme une nouvelle insertion
                Updatefrm.CallBack += Newfrm_CallBack;
                ListeCompteSpecifique.Remove(CompteSpecifique);
                CompteSpecifiqueRecuToInserte.Remove(CompteSpecifique);
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
                    CsCompteSpecifique redevenceSelectionne = (CsCompteSpecifique)dgListeCompteSpecifique.SelectedItem;


                    List<CsCompteSpecifique> CompteSpecifiqueRecuToDelete = new List<CsCompteSpecifique>();
                    CompteSpecifiqueRecuToDelete.Add(redevenceSelectionne);
                    Save(new List<CsCompteSpecifique>(), new List<CsCompteSpecifique>(), CompteSpecifiqueRecuToDelete);


                    ListeCompteSpecifique.Remove(redevenceSelectionne);

                    //SessionObject.ListeCompteSpecifique = ListeCompteSpecifique.ToList();
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
            if (CompteSpecifiqueRecuToUpdate.Count > 0 || CompteSpecifiqueRecuToInserte.Count > 0 || CompteSpecifiqueRecuToDelete.Count > 0)
            {
                Save(CompteSpecifiqueRecuToUpdate, CompteSpecifiqueRecuToInserte, CompteSpecifiqueRecuToDelete);
            }
        }

        #endregion

        #region Constructeurs

        public FrmListeCompteSpecifique()
        {
            InitializeComponent();
            InitializeComponentData();

            LoadAllCompteSpecifique();
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
                    FrmCompteSpecifique Newfrm = new FrmCompteSpecifique();
                    Newfrm.CallBack += Newfrm_CallBack;
                    Newfrm.Show();
                    break;
                case "Consultation":
                    new FrmCompteSpecifique((CsCompteSpecifique)dgListeCompteSpecifique.SelectedItem).Show();
                    break;
                case "Modification":
                    FrmCompteSpecifique Updatefrm = new FrmCompteSpecifique((CsCompteSpecifique)dgListeCompteSpecifique.SelectedItem, true);
                    Updatefrm.CallBack += Updatefrm_CallBack;
                    Updatefrm.Show();
                    break;
                case "Supprimer":
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Ête-vous sure de vouloire supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            CsCompteSpecifique LotsScelleRecuToDelete = (CsCompteSpecifique)dgListeCompteSpecifique.SelectedItem;
                            CompteSpecifiqueRecuToDelete.Add(LotsScelleRecuToDelete);
                            ListeCompteSpecifique.Remove(LotsScelleRecuToDelete);
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
            dgListeCompteSpecifique.SelectedItem = ((sender) as DataGridRow).DataContext;
        }

        private void dgListeCompteSpecifique_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void dgListeCompteSpecifique_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
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

        private void dgListeCompteSpecifique_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseRightButtonDown += new MouseButtonEventHandler(Row_MouseRightButtonDown);
        }
        private void dgListeCompteSpecifique_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion

        #region Methodes  d'interface

        private void InitializeComponentData()
        {
            this.dgListeCompteSpecifique.ItemsSource = ListeCompteSpecifique.OrderBy(r=>r.LIBELLE);
        }

        #endregion

       

    }
}


using Galatee.Silverlight.MainView;
using Galatee.Silverlight.ServiceInterfaceComptable;
using Galatee.Silverlight.ServiceTarification;
using Galatee.Silverlight.Shared;
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
    public partial class FrmCoper_TypeCompte : ChildWindow
    {
        #region Services

        #region Load

        public void LoadAllCoperTypeCoper()
        {

            try
            {
                InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                service.RetourneCoper_TypeCompteCompleted += (s, args) =>
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
                                    args.Result.ForEach(t=>List_Coper_Type_Compte.Add(t));
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
                service.RetourneCoper_TypeCompteAsync();    


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RetourneCodeOperation()
        {
            try
            {
                InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                service.RetourneCodeOperationCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null
                            )
                        {
                            string error = args.Error.InnerException.ToString();
                            return;
                        }
                        else
                        {
                            if (args.Result != null && args.Result.Count != 0)
                            {
                                ListeOperation = args.Result;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                    }
                };
                service.RetourneCodeOperationAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RetourneTypeCompte()
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
                        {
                            if (args.Result != null && args.Result.Count != 0)
                            {
                                ListeTypeCompte = new List<CsTypeCompte>();
                                ListeTypeCompte = args.Result;
                            }
                            else
                            {
                                Message.ShowInformation("Le système n'a trouvé aucun Compte spécifique dans la base de données ", "Avertissement");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
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
            //SessionObject.ListeCompteSpecifique = ListeCompteSpecifique.ToList();
            System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(List_Coper_Type_Compte.OrderBy(r => r.LIBELLE_COPER));
            dgListeCompteSpecifique.ItemsSource = view;
            datapager.Source = view;
        }

        #endregion

        #region Update

        public void Save(List<CsCoper_Type_Compte> CompteSpecifiqueRecuToUpdate, List<CsCoper_Type_Compte> CompteSpecifiqueRecuToInserte, List<CsCoper_Type_Compte> CompteSpecifiqueRecuToDelete)
        {
            try
            {
                InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                int handler = LoadingManager.BeginLoading("Mise à jour des données ...");
                service.SaveCoper_Type_CompteAsync(CompteSpecifiqueRecuToUpdate, CompteSpecifiqueRecuToInserte, CompteSpecifiqueRecuToDelete);
                service.SaveCoper_Type_CompteCompleted += (er, res) =>
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
                                    CsCoper_Type_Compte CompteSpecifique = ListeCompteSpecifique.FirstOrDefault(l => l.PK_ID == 0);
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

        public List<ServiceInterfaceComptable.CsTypeCompte> ListeTypeCompte = new List<CsTypeCompte>();


        public ObservableCollection<CsCoper_Type_Compte> ListeCompteSpecifique = new ObservableCollection<CsCoper_Type_Compte>();
        public List<ServiceInterfaceComptable.CsCoper> ListeOperation = new List<CsCoper>();
        CsCoper_Type_Compte CsCoper_Type_Compte = new CsCoper_Type_Compte();

        ObservableCollection<CsCoper_Type_Compte> List_Coper_Type_Compte = new ObservableCollection<CsCoper_Type_Compte>();

        public List<CsCoper_Type_Compte> CompteSpecifiqueRecuToUpdate = new List<CsCoper_Type_Compte>();
        public List<CsCoper_Type_Compte> CompteSpecifiqueRecuToInserte = new List<CsCoper_Type_Compte>();
        public List<CsCoper_Type_Compte> CompteSpecifiqueRecuToDelete = new List<CsCoper_Type_Compte>();

        FrmCompteSpecifique Newfrm = new FrmCompteSpecifique();

        #endregion

        #region EventHandler

        private void Txt_LibelleProduit_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_Produit_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (ListeOperation != null && ListeOperation.Count != 0)
                {
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(ListeOperation);
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedProduit);
                    ctr.Show();
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        void galatee_OkClickedProduit(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.GetisOkClick)
            {
                ServiceInterfaceComptable.CsCoper _Leproduit = (ServiceInterfaceComptable.CsCoper)ctrs.MyObject;
                this.Txt_CodeProduit.Text = _Leproduit.CODE;
                this.Txt_LibelleProduit.Text = _Leproduit.LIBELLE;
                this.Txt_LibelleProduit.Tag = _Leproduit;
            }
        }
        private void Txt_CodeProduit_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeProduit.Text) )
                {
                    ServiceInterfaceComptable.CsCoper _LeProduit = ClasseMEthodeGenerique.RetourneObjectFromList(ListeOperation, this.Txt_CodeProduit.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeProduit.CODE))
                    {
                        this.CsCoper_Type_Compte.FK_IDCOPER = _LeProduit.PK_ID;
                        this.Txt_LibelleProduit.Text = _LeProduit.LIBELLE;
                        this.Txt_LibelleProduit.Tag = _LeProduit;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }




        private void Txt_LibelleTypeCompte_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_TypeCompte_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (ListeTypeCompte != null && ListeTypeCompte.Count != 0)
                {
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(ListeTypeCompte);
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedTypeCompte);
                    ctr.Show();
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        void galatee_OkClickedTypeCompte(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.GetisOkClick)
            {
                ServiceInterfaceComptable.CsTypeCompte _Leproduit = (ServiceInterfaceComptable.CsTypeCompte)ctrs.MyObject;
                this.Txt_TypeCompte.Text = _Leproduit.CODE;
                this.Txt_LibelleTypeCompte.Text = _Leproduit.LIBELLE;
                this.Txt_LibelleTypeCompte.Tag = _Leproduit;
            }
        }
        private void Txt_CodeTypeCompte_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeProduit.Text) )
                {
                    ServiceInterfaceComptable.CsTypeCompte _LeProduit = ClasseMEthodeGenerique.RetourneObjectFromList(ListeTypeCompte, this.Txt_TypeCompte.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeProduit.CODE))
                    {
                        this.CsCoper_Type_Compte.FK_IDTYPE_COMPTE = _LeProduit.PK_ID;
                        this.Txt_LibelleTypeCompte.Text = _LeProduit.LIBELLE;
                        this.Txt_LibelleTypeCompte.Tag = _LeProduit;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }






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
            List<CsCoper_Type_Compte> CompteSpecifiqueRecuToUpdate = new List<CsCoper_Type_Compte>();
            CompteSpecifiqueRecuToUpdate.Add((CsCoper_Type_Compte)e.Bag);
            Save(CompteSpecifiqueRecuToUpdate, new List<CsCoper_Type_Compte>(), new List<CsCoper_Type_Compte>());

            if (CompteSpecifiqueRecuToUpdate.Select(l => l.PK_ID).Contains(((CsCoper_Type_Compte)e.Bag).PK_ID))
            {
                //CsCoper_Type_Compte LotsScelleRecuToUpdate_ = CompteSpecifiqueRecuToUpdate.FirstOrDefault(l => l.PK_ID == ((CsCoper_Type_Compte)e.Bag).PK_ID);
                //int indexOfLotsScelleRecuToUpdate_ = CompteSpecifiqueRecuToUpdate.IndexOf(LotsScelleRecuToUpdate_);
                //CompteSpecifiqueRecuToUpdate[indexOfLotsScelleRecuToUpdate_] = (CsCoper_Type_Compte)e.Bag;

                CsCoper_Type_Compte LotsScelleRecuToUpdate = ListeCompteSpecifique.FirstOrDefault(l => l.PK_ID == ((CsCoper_Type_Compte)e.Bag).PK_ID);
                int indexOfLotsScelleRecuToUpdate = ListeCompteSpecifique.IndexOf(LotsScelleRecuToUpdate);
                ListeCompteSpecifique[indexOfLotsScelleRecuToUpdate] = (CsCoper_Type_Compte)e.Bag;

                //SessionObject.ListeCompteSpecifique = ListeCompteSpecifique.ToList();
                LoadDatagrid();
            }
            else
            {
                CompteSpecifiqueRecuToUpdate.Add((CsCoper_Type_Compte)e.Bag);
                Save(CompteSpecifiqueRecuToUpdate, new List<CsCoper_Type_Compte>(), new List<CsCoper_Type_Compte>());


                CsCoper_Type_Compte LotsScelleRecuToUpdate = ListeCompteSpecifique.FirstOrDefault(l => l.PK_ID == ((CsCoper_Type_Compte)e.Bag).PK_ID);
                int indexOfLotsScelleRecuToUpdate = ListeCompteSpecifique.IndexOf(LotsScelleRecuToUpdate);
                ListeCompteSpecifique[indexOfLotsScelleRecuToUpdate] = (CsCoper_Type_Compte)e.Bag;

                //SessionObject.ListeCompteSpecifique = ListeCompteSpecifique.ToList();
            }

        }
        void Newfrm_CallBack(object sender, CustumEventArgs e)
        {
            List<CsCoper_Type_Compte> CompteSpecifiqueRecuToInserte = new List<CsCoper_Type_Compte>();

            this.CsCoper_Type_Compte.DATECREATION = DateTime.Now;
            this.CsCoper_Type_Compte.DATEMODIFICATION = DateTime.Now;
            this.CsCoper_Type_Compte.USERCREATION = UserConnecte.matricule;
            this.CsCoper_Type_Compte.USERMODIFICATION = UserConnecte.matricule;
            this.CsCoper_Type_Compte.ISFACTURE = chbx_EstFacture.IsChecked;

            CompteSpecifiqueRecuToInserte.Add(this.CsCoper_Type_Compte);
            Save(new List<CsCoper_Type_Compte>(), CompteSpecifiqueRecuToInserte, new List<CsCoper_Type_Compte>());

            List_Coper_Type_Compte.Add(this.CsCoper_Type_Compte);

            //SessionObject.ListeCompteSpecifique = ListeCompteSpecifique.ToList();
            LoadDatagrid();
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            //new FrmCompteSpecifique((CsCoper_Type_Compte)dgListeCompteSpecifique.SelectedItem).Show();
            //new FrmCompteSpecifique((CsCoper_Type_Compte)dgListeCompteSpecifique.SelectedItem).Show();
        }
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            Newfrm_CallBack(null, null);
            //FrmCompteSpecifique Newfrm = new FrmCompteSpecifique();
            //Newfrm.CallBack += Newfrm_CallBack;
            //Newfrm.Show();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            //FrmCompteSpecifique Updatefrm = new FrmCompteSpecifique((CsCoper_Type_Compte)dgListeCompteSpecifique.SelectedItem, true);
            //CsCoper_Type_Compte CompteSpecifique = ((CsCoper_Type_Compte)dgListeCompteSpecifique.SelectedItem);
            ////On verifie que l'element actuelement selectionner a jamais été insérer en base en s'assurant que PK_ID est different de 0
            //if (CompteSpecifique.PK_ID != 0)
            //{
            //    Updatefrm.CallBack += Updatefrm_CallBack;
            //}
            //else
            //{
            //    //Sinon on la considere comme une nouvelle insertion
            //    Updatefrm.CallBack += Newfrm_CallBack;
            //    ListeCompteSpecifique.Remove(CompteSpecifique);
            //    CompteSpecifiqueRecuToInserte.Remove(CompteSpecifique);
            //}
            //Updatefrm.Show();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Ête-vous sure de vouloire supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    CsCoper_Type_Compte redevenceSelectionne = (CsCoper_Type_Compte)dgListeCompteSpecifique.SelectedItem;


                    if (redevenceSelectionne!=null)
                    {
                        List<CsCoper_Type_Compte> CompteSpecifiqueRecuToDelete = new List<CsCoper_Type_Compte>();
                        CompteSpecifiqueRecuToDelete.Add(redevenceSelectionne);
                        Save(new List<CsCoper_Type_Compte>(), new List<CsCoper_Type_Compte>(), CompteSpecifiqueRecuToDelete);


                        ListeCompteSpecifique.Remove(redevenceSelectionne);

                        //SessionObject.ListeCompteSpecifique = ListeCompteSpecifique.ToList();
                        LoadDatagrid();
                    }
                    else
                    {
                        Message.ShowInformation("Veuillez selection une ligne", "Avertissement");
                    }
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

        public FrmCoper_TypeCompte()
        {
            InitializeComponent();
            InitializeComponentData();

            LoadAllCoperTypeCoper();
            RetourneCodeOperation();
            RetourneTypeCompte();
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
                    //new FrmCompteSpecifique((CsCoper_Type_Compte)dgListeCompteSpecifique.SelectedItem).Show();
                    break;
                case "Modification":
                    //FrmCompteSpecifique Updatefrm = new FrmCompteSpecifique((CsCoper_Type_Compte)dgListeCompteSpecifique.SelectedItem, true);
                    //Updatefrm.CallBack += Updatefrm_CallBack;
                    //Updatefrm.Show();
                    break;
                case "Supprimer":
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Ête-vous sure de vouloire supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            CsCoper_Type_Compte LotsScelleRecuToDelete = (CsCoper_Type_Compte)dgListeCompteSpecifique.SelectedItem;
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
            this.dgListeCompteSpecifique.ItemsSource = ListeCompteSpecifique.OrderBy(r=>r.LIBELLE_COPER);
        }

        #endregion

       

       

    }
}


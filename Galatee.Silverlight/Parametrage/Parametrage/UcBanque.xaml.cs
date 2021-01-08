using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.ServiceParametrage;
using Galatee.Silverlight.Resources.Parametrage;
namespace Galatee.Silverlight.Parametrage
{
    public partial class UcBanque : ChildWindow
    {
        List<aBanque> listForInsertOrUpdate = null;
        ObservableCollection<aBanque> donnesDatagrid = new ObservableCollection<aBanque>();
        private aBanque ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcBanque()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Banque);
            }
        }
        public UcBanque(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new aBanque();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as aBanque);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<aBanque>;
                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                              Txt_Code.Text = ObjetSelectionnee.CODE ;
                              Txt_Libelle.Text = ObjetSelectionnee.LIBELLE ;
                               btnOk.IsEnabled = false;

                    }
                }
                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Banque);
            }
        }

        private void GetDataNew()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllBanqueCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.Banque);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var pBanque in args.Result)
                        {
                            donnesDatagrid.Add(pBanque);
                        }
                   
                };
                client.SelectAllBanqueAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(aBanque pBanque)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    GetDataNew();
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    GetDataNew();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Translate()
        {
            try
            {
                Title = Languages.LibelleCategorieBranchement;
                btnOk.Content = Languages.OK;
                Btn_Annuler.Content = Languages.Annuler;
                //GboBanque.Header = Languages.InformationsBanque;
                lab_Code.Content = Languages.Code;
                lab_Libelle.Content = Languages.Libelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<aBanque> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<aBanque>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var Banque = new aBanque
                    {    CODE=Txt_Code.Text,
                         LIBELLE = Txt_Libelle.Text,
                         DATECREATION = DateTime.Now,
                         USERCREATION = UserConnecte.matricule
                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == Banque.CODE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(Banque);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    
                    ObjetSelectionnee.CODE = Txt_Code.Text;
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text;
                    ObjetSelectionnee.DATECREATION = DateTime.Now;
                    ObjetSelectionnee.USERCREATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Banque, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        listForInsertOrUpdate = GetInformationsFromScreen();
                        var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));

                        if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        {
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                            {
                                service.InsertBanqueCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.Show(insertR.Error.Message, Languages.Banque);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.Show(Languages.ErreurInsertionDonnees, Languages.Banque);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertBanqueAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateBanqueCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.Show(UpdateR.Error.Message, Languages.Banque);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.Show(Languages.ErreurMiseAJourDonnees, Languages.Banque);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateBanqueAsync(listForInsertOrUpdate);
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
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Banque);
            }
        }

         //private void DialogResultEnregistrer(object sender, EventArgs e)
         //{
         //    try
         //    {
         //        var ctrs = sender as DialogResult;
         //        if (ctrs != null && ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
         //        {

         //            listForInsertOrUpdate = GetInformationsFromScreen();
         //            var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));

         //            if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
         //            {
         //                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Creation)
         //                {
         //                    service.InsertBanqueCompleted += (snder, insertR) =>
         //                       {
         //                           if (insertR.Cancelled ||
         //                               insertR.Error != null)
         //                           {
         //                               Message.Show(insertR.Error.Message, Languages.Banque);
         //                               return;
         //                           }
         //                           if (!insertR.Result)
         //                           {
         //                               Message.Show(Languages.ErreurInsertionDonnees, Languages.Banque);
         //                               return;
         //                           }
         //                           UpdateParentList(listForInsertOrUpdate[0]);
         //                       };
         //                    service.InsertBanqueAsync(listForInsertOrUpdate);
         //                }
         //                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Modification)
         //                {
         //                    service.UpdateBanqueCompleted += (snder, UpdateR) =>
         //                       {
         //                           if (UpdateR.Cancelled ||
         //                               UpdateR.Error != null)
         //                           {
         //                               Message.Show(UpdateR.Error.Message, Languages.Banque);
         //                               return;
         //                           }
         //                           if (!UpdateR.Result)
         //                           {
         //                               Message.Show(Languages.ErreurMiseAJourDonnees, Languages.Banque);
         //                               return;
         //                           }
         //                           UpdateParentList(listForInsertOrUpdate[0]);
         //                       };
         //                    service.UpdateBanqueAsync(listForInsertOrUpdate);
         //                }
         //            }
         //            this.Close();
         //        }
         //    }
         //    catch (Exception ex)
         //    {
         //        Message.Show(ex.Message, Languages.Banque);
         //    }
         //}

        private void VerifierSaisie()
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_Code.Text) && !string.IsNullOrEmpty(Txt_Libelle.Text) && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation)
                    btnOk.IsEnabled = true;
                else
                {
                    btnOk.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btn_Annuler_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Reinitialiser();
                DialogResult = false;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Banque);
            }
        }

        private void Reinitialiser()
        {
            try
            {
                Txt_Code.Text = string.Empty;
                Txt_Libelle.Text = string.Empty;
                btnOk.IsEnabled = false;
                Txt_Code.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void On_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Banque);
            }
        }
    }
}



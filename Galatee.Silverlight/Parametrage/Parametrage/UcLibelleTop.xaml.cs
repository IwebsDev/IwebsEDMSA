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
using Galatee.Silverlight.Library;
using Galatee.Silverlight.ServiceParametrage;
using Galatee.Silverlight.Resources.Parametrage;
namespace Galatee.Silverlight.Parametrage
{
    public partial class UcLibelleTop: ChildWindow
    {
        List<CsLibelleTop> listForInsertOrUpdate = null;
        ObservableCollection<CsLibelleTop> donnesDatagrid = new ObservableCollection<CsLibelleTop>();
        private CsLibelleTop ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcLibelleTop()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleLibelleTop);
            }
        }
        public UcLibelleTop(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new CsLibelleTop();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsLibelleTop);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsLibelleTop>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_Code.Text = ObjetSelectionnee.CODE;
                        Txt_Libelle.Text = ObjetSelectionnee.LIBELLE;
                        btnOk.IsEnabled = false;

                        //Txt_Code.IsReadOnly = true;
                    }
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot,false);
                }
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleLibelleTop);
            }
        }
        private void GetDatanEW()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllLibelleTopCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.LibelleLibelleTop);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var pLibelleTop in args.Result)
                        {
                            donnesDatagrid.Add(pLibelleTop);
                        }
                    
                };
                client.SelectAllLibelleTopAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(CsLibelleTop pLibelleTop)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    GetDatanEW();
                    //donnesDatagrid.Add(pLibelleTop);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    GetDatanEW();
                    //var nationalite = donnesDatagrid.First(p => p.PK_ID == pLibelleTop.PK_ID);
                    //donnesDatagrid.Remove(nationalite);
                    //donnesDatagrid.Add(pLibelleTop);
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
                Title = Languages.LibelleLibelleTop;
                btnOk.Content = Languages.OK;
                Btn_Reinitialiser.Content = Languages.Annuler;
                GroupBox.Header = Languages.InformationsLibelleTop;
                lab_Code.Content = Languages.Code;
                lab_Libelle.Content = Languages.Libelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsLibelleTop> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsLibelleTop>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var LibelleTop= new CsLibelleTop
                    {
                        CODE = Txt_Code.Text,
                        LIBELLE = Txt_Libelle.Text,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule
                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == LibelleTop.CODE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(LibelleTop);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.CODE = Txt_Code.Text;
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text;
                    ObjetSelectionnee.DATEMODIFICATION = DateTime.Now;
                    ObjetSelectionnee.USERMODIFICATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleLibelleTop);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
                            {
                    try
                    {
                        var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleLibelleTop, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                    listForInsertOrUpdate = GetInformationsFromScreen();
                    var service = new ParametrageClient(Utility.Protocole(),
                                                        Utility.EndPoint(
                                                            "Parametrage"));

                    if (listForInsertOrUpdate != null &&
                        listForInsertOrUpdate.Count > 0)
                    {
                        if ((SessionObject.ExecMode) ModeExecution ==
                            SessionObject.ExecMode.Creation)
                        {
                            service.InsertLibelleTopCompleted +=
                                (declancherInsert, insertR) =>
                                    {
                                        if (insertR.Cancelled ||
                                            insertR.Error != null)
                                        {
                                            Message.ShowError(
                                                insertR.Error.Message,
                                                Languages.LibelleLibelleTop);
                                            return;
                                        }
                                        if (!insertR.Result)
                                        {
                                            Message.ShowError(
                                                Languages.
                                                    ErreurInsertionDonnees,
                                                Languages.LibelleLibelleTop);
                                            return;
                                        }
                                        UpdateParentList(listForInsertOrUpdate[0]);
                                        DialogResult = true;

                                    };
                            service.InsertLibelleTopAsync(listForInsertOrUpdate);
                        }
                        if ((SessionObject.ExecMode) ModeExecution ==
                            SessionObject.ExecMode.Modification)
                        {
                            service.UpdateLibelleTopCompleted +=
                                (declancherUpdate, UpdateR) =>
                                    {
                                        if (UpdateR.Cancelled ||
                                            UpdateR.Error != null)
                                        {
                                            Message.ShowError(
                                                UpdateR.Error.Message,
                                                Languages.LibelleLibelleTop);
                                            return;
                                        }
                                        if (!UpdateR.Result)
                                        {
                                            Message.ShowError(
                                                Languages.
                                                    ErreurMiseAJourDonnees,
                                                Languages.LibelleLibelleTop);
                                            return;
                                        }
                                        UpdateParentList(listForInsertOrUpdate[0]);
                                        DialogResult = true;
                                    };
                            service.UpdateLibelleTopAsync(listForInsertOrUpdate);
                        }
                    }
                    else
                    {
                        return;
                    }
                    }
                    };
                        messageBox.Show();
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Languages.LibelleLibelleTop);
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
         //                    service.InsertLibelleTopCompleted += (snder, insertR) =>
         //                       {
         //                           if (insertR.Cancelled ||
         //                               insertR.Error != null)
         //                           {
         //                               Message.Show(insertR.Error.Message, Languages.LibelleLibelleTop);
         //                               return;
         //                           }
         //                           if (!insertR.Result)
         //                           {
         //                               Message.Show(Languages.ErreurInsertionDonnees, Languages.LibelleLibelleTop);
         //                               return;
         //                           }
         //                           UpdateParentList(listForInsertOrUpdate[0]);
         //                       };
         //                    service.InsertLibelleTopAsync(listForInsertOrUpdate);
         //                }
         //                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Modification)
         //                {
         //                    service.UpdateLibelleTopCompleted += (snder, UpdateR) =>
         //                       {
         //                           if (UpdateR.Cancelled ||
         //                               UpdateR.Error != null)
         //                           {
         //                               Message.Show(UpdateR.Error.Message, Languages.LibelleLibelleTop);
         //                               return;
         //                           }
         //                           if (!UpdateR.Result)
         //                           {
         //                               Message.Show(Languages.ErreurMiseAJourDonnees, Languages.LibelleLibelleTop);
         //                               return;
         //                           }
         //                           UpdateParentList(listForInsertOrUpdate[0]);
         //                       };
         //                    service.UpdateLibelleTopAsync(listForInsertOrUpdate);
         //                }
         //            }
         //            this.Close();
         //        }
         //    }
         //    catch (Exception ex)
         //    {
         //        Message.Show(ex.Message, Languages.LibelleLibelleTop);
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

        private void Btn_Reinitialiser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Reinitialiser();
                DialogResult = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleLibelleTop);
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
                Message.ShowError(ex.Message, Languages.LibelleLibelleTop);
            }
        }
    }
}



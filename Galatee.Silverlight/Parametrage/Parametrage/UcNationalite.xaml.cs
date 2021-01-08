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
using Galatee.Silverlight.ServiceParametrage;
using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.Library;
namespace Galatee.Silverlight.Parametrage
{
    public partial class UcNationalite : ChildWindow
    {
        List<CsNationalite> listForInsertOrUpdate = null;
        ObservableCollection<CsNationalite> donnesDatagrid = new ObservableCollection<CsNationalite>();
        private CsNationalite nationaliteSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcNationalite(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new CsNationalite();
                if (pObjects[0] != null)
                    nationaliteSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsNationalite);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsNationalite>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (nationaliteSelectionnee != null)
                    {
                        Txt_Code.Text = nationaliteSelectionnee.CODE;
                        Txt_Libelle.Text = nationaliteSelectionnee.LIBELLE;
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
                Message.ShowError(ex.Message, Languages.LibelleNationalite);
            }
        }

        private void GetDataNew()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllNationaliteCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.LibelleNationalite);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var pNationalite in args.Result)
                        {
                            donnesDatagrid.Add(pNationalite);
                        }
                    

                };
                client.SelectAllNationaliteAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(CsNationalite pNationalite)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    GetDataNew();
                    //donnesDatagrid.Add(pNationalite);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                     GetDataNew();
                    //var nationalite = donnesDatagrid.First(p => p.PK_ID == pNationalite.PK_ID);
                    //donnesDatagrid.Remove(nationalite);
                    //donnesDatagrid.Add(pNationalite);
                }
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        public UcNationalite()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleNationalite);
            }
        }

        private void Translate()
        {
            try
            {
                Title = Languages.LibelleNationalite;
                btnOk.Content = Languages.OK;
                Btn_Annuler.Content = Languages.Annuler;
                GroupBox.Header = Languages.InformationsNationalite;
                lab_Code.Content = Languages.Code;
                lab_Libelle.Content = Languages.Libelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsNationalite> GetInformationsFromScreen()
        {
            var listNationalite = new List<CsNationalite>();
            try
            {
                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var nationalite = new CsNationalite
                                                    {
                                                        CODE = Txt_Code.Text,
                                                        LIBELLE = Txt_Libelle.Text,
                                                        DATECREATION = DateTime.Now,
                                                        USERCREATION = UserConnecte.matricule
                                                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == nationalite.CODE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listNationalite.Add(nationalite);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    nationaliteSelectionnee.CODE = Txt_Code.Text;
                    nationaliteSelectionnee.LIBELLE = Txt_Libelle.Text;
                    nationaliteSelectionnee.DATEMODIFICATION = DateTime.Now;
                    nationaliteSelectionnee.USERMODIFICATION = UserConnecte.matricule;
                    listNationalite.Add(nationaliteSelectionnee);
                }
                return listNationalite;
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
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleNationalite, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertNationaliteCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled || insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.LibelleNationalite);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.LibelleNationalite);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertNationaliteAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateNationaliteCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled || UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.LibelleNationalite);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.LibelleNationalite);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateNationaliteAsync(listForInsertOrUpdate);
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
                Message.ShowError(ex.Message, Languages.LibelleNationalite);
            }
            finally
            {
                Txt_Code.Focus();
            }
        }
       
        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (!string.IsNullOrEmpty(Txt_Code.Text) && (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                //{

                //    if (donnesDatagrid.FirstOrDefault(p => p.CODE == Txt_Code.Text.Trim()) != null)
                //    {
                //        throw new Exception(Languages.CetElementExisteDeja);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleNationalite);
            }
        }

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
                Message.ShowError(ex.Message, Languages.LibelleNationalite);
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
                Message.ShowError(ex.Message, Languages.LibelleNationalite);
            }
        }
    }
}



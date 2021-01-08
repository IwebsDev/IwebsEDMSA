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
    public partial class UcParametresGeneraux : ChildWindow
    {
        List<CsParametresGeneraux> listForInsertOrUpdate = null;
        ObservableCollection<CsParametresGeneraux> donnesDatagrid = new ObservableCollection<CsParametresGeneraux>();
        private CsParametresGeneraux ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcParametresGeneraux()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleParametresGeneraux);
            }
        }
        public UcParametresGeneraux(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new CsParametresGeneraux();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsParametresGeneraux);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsParametresGeneraux>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_Code.Text = ObjetSelectionnee.CODE;
                        Txt_Libelle.Text = ObjetSelectionnee.LIBELLE;
                        Txt_Descrition.Text = ObjetSelectionnee.DESCRIPTION;
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
                Message.ShowError(ex.Message, Languages.LibelleParametresGeneraux);
            }
        }

        private void Translate()
        {
            try
            {
                Title = Languages.LibelleParametresGeneraux;
                btnOk.Content = Languages.OK;
                Btn_Annuler.Content = Languages.Annuler;
                GroupBox.Header = Languages.InformationsParametresGeneraux;
                lab_Code.Content = Languages.Code;
                lab_Libelle.Content = Languages.Libelle;
                lab_Description.Content = Languages.Description;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDataNew()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllParametresGenerauxCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.LibelleParametresGeneraux);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var pParametresGeneraux in args.Result)
                        {
                            donnesDatagrid.Add(pParametresGeneraux);
                        }
                   

                };
                client.SelectAllParametresGenerauxAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(CsParametresGeneraux pParametresGeneraux)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    GetDataNew();
                    //donnesDatagrid.Add(pParametresGeneraux);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    GetDataNew();
                    //var nationalite = donnesDatagrid.First(p => p.PK_ID == pParametresGeneraux.PK_ID);
                    //donnesDatagrid.Remove(nationalite);
                    //donnesDatagrid.Add(pParametresGeneraux);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsParametresGeneraux> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsParametresGeneraux>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var parametresGeneraux = new CsParametresGeneraux
                    {
                        CODE = Txt_Code.Text,
                        LIBELLE = Txt_Libelle.Text,
                        DESCRIPTION = Txt_Descrition.Text,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule
                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == parametresGeneraux.CODE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(parametresGeneraux);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.CODE = Txt_Code.Text;
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text;
                    ObjetSelectionnee.DESCRIPTION = Txt_Descrition.Text;
                    ObjetSelectionnee.DATEMODIFICATION = DateTime.Now;
                    ObjetSelectionnee.USERMODIFICATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleNationalite);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleParametresGeneraux, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertParametresGenerauxCompleted += (snder, insertR) =>
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
                                service.InsertParametresGenerauxAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateParametresGenerauxCompleted += (snder, UpdateR) =>
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
                                service.UpdateParametresGenerauxAsync(listForInsertOrUpdate);
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
                Message.ShowError(ex.Message, Languages.LibelleParametresGeneraux);
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
                Message.ShowError(ex.Message, Languages.LibelleParametresGeneraux);
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

        private void Btn_Annuler_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Reinitialiser();
                DialogResult = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleParametresGeneraux);
            }
        }

        private void Reinitialiser()
        {
            try
            {
                Txt_Code.Text = string.Empty;
                Txt_Libelle.Text = string.Empty;
                Txt_Descrition.Text = string.Empty;
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
                Message.ShowError(ex.Message, Languages.LibelleParametresGeneraux);
            }
        }
       
    }
}



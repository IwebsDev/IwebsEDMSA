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
    public partial class UcDenomination : ChildWindow
    {
        List<CsDenomination> listForInsertOrUpdate = null;
        ObservableCollection<CsDenomination> donnesDatagrid = new ObservableCollection<CsDenomination>();
        private CsDenomination DenominationSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcDenomination(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new CsDenomination();
                if (pObjects[0] != null)
                    DenominationSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsDenomination);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsDenomination>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (DenominationSelectionnee != null)
                    {
                        Txt_Code.Text = DenominationSelectionnee.CODE;
                        Txt_Libelle.Text = DenominationSelectionnee.LIBELLE;
                        btnOk.IsEnabled = false;
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
                Message.ShowError(ex.Message, Languages.LibelleDenomination);
            }
        }

        private void UpdateParentList(CsDenomination pDenomination)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    donnesDatagrid.Add(pDenomination);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    var Denomination = donnesDatagrid.First(p => p.PK_ID == pDenomination.PK_ID);
                    donnesDatagrid.Remove(Denomination);
                    donnesDatagrid.Add(pDenomination);
                }
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        public UcDenomination()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleDenomination);
            }
        }

        private void Translate()
        {
            try
            {
                Title = Languages.LibelleDenomination;
                btnOk.Content = Languages.OK;
                Btn_Reinitialiser.Content = Languages.Annuler;
                GboDenomination.Header = Languages.InformationsDenomination;
                lab_Code.Content = Languages.Code;
                lab_Libelle.Content = Languages.Libelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsDenomination> GetInformationsFromScreen()
        {
            var listDenomination = new List<CsDenomination>();
            try
            {
                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var Denomination = new CsDenomination
                                                    {
                                                        CODE = Txt_Code.Text,
                                                        LIBELLE = Txt_Libelle.Text,
                                                        DATECREATION = DateTime.Now,
                                                        USERCREATION = UserConnecte.matricule
                                                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == Denomination.CODE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listDenomination.Add(Denomination);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    DenominationSelectionnee.CODE = Txt_Code.Text;
                    DenominationSelectionnee.LIBELLE = Txt_Libelle.Text;
                    DenominationSelectionnee.DATEMODIFICATION = DateTime.Now;
                    DenominationSelectionnee.USERMODIFICATION = UserConnecte.matricule;
                    listDenomination.Add(DenominationSelectionnee);
                }
                return listDenomination;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleDenomination);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleDenomination, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertDenominationCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled || insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.LibelleDenomination);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.LibelleDenomination);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertDenominationAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateDenominationCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled || UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.LibelleDenomination);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.LibelleDenomination);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateDenominationAsync(listForInsertOrUpdate);
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
                Message.ShowError(ex.Message, Languages.LibelleDenomination);
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
                Message.ShowError(ex.Message, Languages.LibelleDenomination);
            }
        }

        private void OnDialogOK(object sender, EventArgs e)
        {
            var ctrs = sender as DialogResult;
            if (ctrs != null && ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
            {
                ctrs.DialogResult = false;
                //Txt_Code.Focus();
            }
        }

        private void DialogResultOk(object sender, EventArgs e)
        {
            var ctrs = sender as DialogResult;
            if (ctrs != null && ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
            {
                ctrs.DialogResult = false;
                return;
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
                Message.ShowError(ex.Message, Languages.LibelleDenomination);
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
                Message.ShowError(ex.Message, Languages.LibelleDenomination);
            }
        }
    }
}



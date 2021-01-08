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
    public partial class UcPeriodiciteFacturation : ChildWindow
    {
       List<CsPeriodiciteFacturation> listForInsertOrUpdate = null;
       ObservableCollection<CsPeriodiciteFacturation> donnesDatagrid = new ObservableCollection<CsPeriodiciteFacturation>();
        private CsPeriodiciteFacturation ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcPeriodiciteFacturation()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
            }
        }
        public UcPeriodiciteFacturation(object[] pObjects, SessionObject.ExecMode []pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new CsPeriodiciteFacturation();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsPeriodiciteFacturation);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsPeriodiciteFacturation>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification 
                    || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
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
                Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
            }
        }

        private void GetDataNew()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllPeriodiciteFacturationCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.PeriodiciteFacturation);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.Show(Languages.msgErreurChargementDonnees, Languages.PeriodiciteFacturation);
                        return;
                    }
                    else
                    {
                        donnesDatagrid.Clear();
                        if (args.Result != null)
                            foreach (var pParametreEvenement in args.Result)
                            {
                                donnesDatagrid.Add(pParametreEvenement);
                            }
                       
                    }
                };
                client.SelectAllPeriodiciteFacturationAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(CsPeriodiciteFacturation pParametreEvenement)
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
                    //var ParametreEvenement = donnesDatagrid.First(p => p.PK_ID == pParametreEvenement.PK_ID);
                    //donnesDatagrid.Remove(ParametreEvenement);
                    //donnesDatagrid.Add(pParametreEvenement);
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
                Title = Languages.PeriodiciteFacturation;
                btnOk.Content = Languages.OK;
                Btn_Annuler.Content = Languages.Annuler;
                lab_Code.Content = Languages.Code;
                lab_Libelle.Content = Languages.Libelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsPeriodiciteFacturation> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsPeriodiciteFacturation>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var PeriodiciteFacturation = new CsPeriodiciteFacturation
                    {
                        CODE = Txt_Code.Text,
                        LIBELLE = Txt_Libelle.Text,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule
                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == PeriodiciteFacturation.CODE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(PeriodiciteFacturation);
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
                Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.PeriodiciteFacturation, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertPeriodiciteFacturationCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.Show(insertR.Error.Message, Languages.lblModule);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.Show(Languages.ErreurInsertionDonnees, Languages.lblModule);
                                        return;
                                    }
                                   
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = false;
                                
                                };
                                service.InsertPeriodiciteFacturationAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdatePeriodiciteFacturationCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.Show(UpdateR.Error.Message, Languages.lblModule);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.Show(Languages.ErreurMiseAJourDonnees, Languages.lblModule);
                                        return;
                                    }
                                    
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = false;
                              
                                };
                                service.UpdatePeriodiciteFacturationAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.PeriodiciteFacturation);

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
                Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);

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
                Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);

            }
        }
    }
}



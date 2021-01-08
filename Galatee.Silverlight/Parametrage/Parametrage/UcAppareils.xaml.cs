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
    public partial class UcAppareils : ChildWindow
    {
        List<CsAppareils> listForInsertOrUpdate = null;
        ObservableCollection<CsAppareils> donnesDatagrid = new ObservableCollection<CsAppareils>();
        private CsAppareils ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
   
        public UcAppareils()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {

                Message.Show(ex.Message, Languages.Appareils);
     
            }
           
        }
        public UcAppareils(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new CsAppareils();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsAppareils);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsAppareils>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_CodeAppareil.Text = Convert.ToString(ObjetSelectionnee.CodeAppareil);
                        Txt_Designation.Text = ObjetSelectionnee.Designation;
                        Txt_DETAILS.Text = ObjetSelectionnee.Details;
                        txt_TpsUtil.Text = ObjetSelectionnee.TEMPSUTILISATION.ToString();
                        

                    }
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    Txt_CodeAppareil.Text = "";
                    Txt_Designation.Text = "";
                    Txt_DETAILS.Text ="";
                    txt_TpsUtil.Text = "";
                     
                }
                //VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Appareils);
            }
        }

        private void Translate()
        {
            try
            {
                Title = Languages.Appareils;
                BtnOK.Content = Languages.OK;
                Btn_Annuler.Content = Languages.Annuler;
             
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void VerifierSaisie()
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(Txt_CodeAppareil.Text) && !string.IsNullOrEmpty(Txt_Designation.Text) &&    !string.IsNullOrEmpty(Txt_DETAILS.Text) && !string.IsNullOrEmpty(txt_TpsUtil.Text) && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation )
        //           BtnOK.IsEnabled = true;
        //        else
        //        {
        //            BtnOK.IsEnabled = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private void GetDataNew()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllAPPAREILSCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.LibelleProduit);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var item in args.Result)
                        {
                            donnesDatagrid.Add(item);
                        }
                    //dtgrdParametre.ItemsSource = donnesDatagrid;
                };
                client.SelectAllAPPAREILSAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateParentList(CsAppareils pAppareils)
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
        private List<CsAppareils> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsAppareils>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    int TEMPSUTILISATION = 0;
                    var appareils = new CsAppareils
                    {
                        CodeAppareil =  Txt_CodeAppareil.Text,
                        Designation = Txt_Designation.Text,
                        Details  =Txt_DETAILS.Text,
                       TEMPSUTILISATION =int.TryParse(txt_TpsUtil.Text,out TEMPSUTILISATION)==true?TEMPSUTILISATION:0,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule
                    };
                    if (!string.IsNullOrEmpty(Txt_CodeAppareil.Text) && donnesDatagrid.FirstOrDefault(p => p.CodeAppareil == appareils.CodeAppareil ) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(appareils);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.CodeAppareil = Txt_CodeAppareil.Text;
                    ObjetSelectionnee.Designation = Txt_Designation.Text;
                    ObjetSelectionnee.Details = Txt_DETAILS.Text;
                    ObjetSelectionnee.TEMPSUTILISATION = Convert.ToInt32(txt_TpsUtil.Text);
                    ObjetSelectionnee.DATECREATION = DateTime.Now;
                    ObjetSelectionnee.USERCREATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Appareils);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Caisse, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertAppareilsCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.Appareils);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Appareils);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertAppareilsAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateAppareilsCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.Appareils);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Appareils);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateAppareilsAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.Appareils);
            }
        }
   

        private void Btn_Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


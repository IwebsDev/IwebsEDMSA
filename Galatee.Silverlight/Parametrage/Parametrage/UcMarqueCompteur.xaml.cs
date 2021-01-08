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
    public partial class UcMarqueCompteur : ChildWindow
    {
        List<CsMarqueCompteur> listForInsertOrUpdate = null;
        ObservableCollection<CsMarqueCompteur> donnesDatagrid = new ObservableCollection<CsMarqueCompteur>();
        private CsMarqueCompteur ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcMarqueCompteur()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, "Marque compteur");
            }
        }
        public UcMarqueCompteur(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var marque = new CsMarqueCompteur();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(marque, pObjects[0] as CsMarqueCompteur);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsMarqueCompteur>;
                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                              Txt_Code.Text = ObjetSelectionnee.CODE ;
                              Txt_Libelle.Text = ObjetSelectionnee.LIBELLE;
                              Txt_Coefficient.Text = ObjetSelectionnee.COEFFICIENTDEMULTIPLICATION.ToString();
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
                Message.Show(ex.Message, "Marque compteur");
            }
        }

        private void GetDataNew()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllMarqueCompteurCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, "Marque compteur");
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var pMarque in args.Result)
                        {
                            donnesDatagrid.Add(pMarque);
                        }
                   
                };
                client.SelectAllMarqueCompteurAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(CsMarqueCompteur pMarque)
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

        private List<CsMarqueCompteur> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsMarqueCompteur>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var Marque = new CsMarqueCompteur
                    {    CODE=Txt_Code.Text,
                         LIBELLE = Txt_Libelle.Text,
                         COEFFICIENTDEMULTIPLICATION = int.Parse(Txt_Coefficient.Text),
                         DATECREATION = DateTime.Now,
                         USERCREATION = UserConnecte.matricule
                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == Marque.CODE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(Marque);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    
                    ObjetSelectionnee.CODE = Txt_Code.Text;
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text;
                    ObjetSelectionnee.COEFFICIENTDEMULTIPLICATION = int.Parse(Txt_Coefficient.Text);
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
                var messageBox = new MessageBoxControl.MessageBoxChildWindow("Marque compteur", Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertMarqueCompteurCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.Show(insertR.Error.Message, "Marque compteur");
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.Show(Languages.ErreurInsertionDonnees, "Marque compteur");
                                        return;
                                    }
                                    //UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertMarqueCompteurAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateMarqueCompteurCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.Show(UpdateR.Error.Message, "Marque compteur");
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.Show(Languages.ErreurMiseAJourDonnees, "Marque compteur");
                                        return;
                                    }
                                    //UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateMarqueCompteurAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, "Marque compteur");
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
         //                               Message.Show(insertR.Error.Message, "Marque compteur");
         //                               return;
         //                           }
         //                           if (!insertR.Result)
         //                           {
         //                               Message.Show(Languages.ErreurInsertionDonnees, "Marque compteur");
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
         //                               Message.Show(UpdateR.Error.Message, "Marque compteur");
         //                               return;
         //                           }
         //                           if (!UpdateR.Result)
         //                           {
         //                               Message.Show(Languages.ErreurMiseAJourDonnees, "Marque compteur");
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
         //        Message.Show(ex.Message, "Marque compteur");
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
                Message.Show(ex.Message, "Marque compteur");
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
                Message.Show(ex.Message, "Marque compteur");
            }
        }
    }
}



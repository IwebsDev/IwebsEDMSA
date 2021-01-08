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
    public partial class UcCategoriesBranchement : ChildWindow
    {
        List<CsTypeBranchement> listForInsertOrUpdate = null;
        ObservableCollection<CsTypeBranchement> donnesDatagrid = new ObservableCollection<CsTypeBranchement>();
        private CsTypeBranchement ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcCategoriesBranchement()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.LibelleCategorieBranchement);
            }
        }
        public UcCategoriesBranchement(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new CsTypeBranchement();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsTypeBranchement);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsTypeBranchement>;
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
                Message.Show(ex.Message, Languages.LibelleCategorieBranchement);
            }
        }
        private void GetDataNew()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllCategorieBranchementCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.LibelleCategorieBranchement);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var pCategorieBranchement in args.Result)
                        {
                            donnesDatagrid.Add(pCategorieBranchement);
                        }
                  
                };
                client.SelectAllCategorieBranchementAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(CsTypeBranchement pCategorieBranchement)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    GetDataNew();
                    //donnesDatagrid.Add(pCategorieBranchement);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    GetDataNew();
                    //var nationalite = donnesDatagrid.First(p => p.PK_ID == pCategorieBranchement.PK_ID);
                    //donnesDatagrid.Remove(nationalite);
                    //donnesDatagrid.Add(pCategorieBranchement);
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
                GboCategorirBranchement.Header = Languages.InformationsCategorieBranchement;
                lab_Code.Content = Languages.Code;
                lab_Libelle.Content = Languages.Libelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsTypeBranchement> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsTypeBranchement>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var CategorieBranchement = new CsTypeBranchement
                    {
                        CODE = Txt_Code.Text,
                        LIBELLE = Txt_Libelle.Text,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule
                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == CategorieBranchement.CODE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(CategorieBranchement);
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
                throw ex;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleCategorieBranchement, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertCategorieBranchementCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.Show(insertR.Error.Message, Languages.LibelleCategorieBranchement);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.Show(Languages.ErreurInsertionDonnees, Languages.LibelleCategorieBranchement);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertCategorieBranchementAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateCategorieBranchementCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.Show(UpdateR.Error.Message, Languages.LibelleCategorieBranchement);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.Show(Languages.ErreurMiseAJourDonnees, Languages.LibelleCategorieBranchement);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateCategorieBranchementAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.LibelleCategorieBranchement);
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
                Message.Show(ex.Message, Languages.LibelleCategorieBranchement);
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
                Message.Show(ex.Message, Languages.LibelleCategorieBranchement);
            }
        }
    }
}



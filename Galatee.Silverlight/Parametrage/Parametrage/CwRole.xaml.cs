using Galatee.Silverlight.Library;
using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceParametrage;
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
    public partial class CwRole : ChildWindow
    {
        List<CsFonction> listForInsertOrUpdate = null;
        ObservableCollection<CsFonction> donnesDatagrid = new ObservableCollection<CsFonction>();
        private CsFonction ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public CwRole()
        {
            InitializeComponent();
        }

        public CwRole(CsFonction pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                if (pObject != null)
                    ObjetSelectionnee = pObject;
                ModeExecution = pExecMode;
                dataGrid = pGrid;
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsFonction>;
                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        TxtCode.Text = ObjetSelectionnee.CODE.ToString() ?? string.Empty;
                        TxtLibelle.Text = ObjetSelectionnee.ROLENAME.ToString() ?? string.Empty;
                        TxtDescription.Text = ObjetSelectionnee.ROLEDISPLAYNAME.ToString() ?? string.Empty;
                        ChkEstAdmin.IsChecked = ObjetSelectionnee.ESTADMIN;
                        OKButton.IsEnabled = false;
                        //TxtCode.IsReadOnly = true;
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
                Message.Show(ex.Message, Languages.Parametrage);
            }
        }

        private void VerifierSaisie()
        {
            try
            {
                if (!string.IsNullOrEmpty(TxtCode.Text) && !string.IsNullOrEmpty(TxtLibelle.Text)
                    && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation)
                    OKButton.IsEnabled = true;
                else
                    OKButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsFonction> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsFonction>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var fonction = new CsFonction();
                    if (!string.IsNullOrEmpty(TxtCode.Text))
                        fonction.CODE = TxtCode.Text;
                    if (!string.IsNullOrEmpty(TxtLibelle.Text))
                        fonction.ROLENAME = TxtLibelle.Text;
                    if (!string.IsNullOrEmpty(TxtDescription.Text))
                        fonction.ROLEDISPLAYNAME = TxtDescription.Text;
                    fonction.ESTADMIN = Convert.ToBoolean(ChkEstAdmin.IsChecked);
                    fonction.DATECREATION = DateTime.Now;
                    fonction.USERCREATION = UserConnecte.matricule;
                    if (!string.IsNullOrEmpty(TxtCode.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == fonction.CODE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(fonction);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    if (!string.IsNullOrEmpty(TxtCode.Text))
                        ObjetSelectionnee.CODE = TxtCode.Text;
                    if (!string.IsNullOrEmpty(TxtLibelle.Text))
                        ObjetSelectionnee.ROLENAME = TxtLibelle.Text;
                    if (!string.IsNullOrEmpty(TxtDescription.Text))
                        ObjetSelectionnee.ROLEDISPLAYNAME = TxtDescription.Text;
                    ObjetSelectionnee.ESTADMIN = Convert.ToBoolean(ChkEstAdmin.IsChecked);

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

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Parametrage, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertFonctionCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.Show(insertR.Error.Message, Languages.Parametrage);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.Show(Languages.ErreurInsertionDonnees, Languages.Parametrage);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate);
                                    DialogResult = true;
                                };
                                service.InsertFonctionAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateFonctionCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.Show(UpdateR.Error.Message, Languages.Parametrage);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.Show(Languages.ErreurMiseAJourDonnees, Languages.Parametrage);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate);
                                    DialogResult = true;
                                };
                                service.UpdateFonctionAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.Parametrage);
            }
        }

        private void UpdateParentList(List<CsFonction> pListeObjet)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    if (pListeObjet != null && pListeObjet.Count > 0)
                        foreach (var item in pListeObjet)
                        {
                            donnesDatagrid.Add(item);
                        }
                }
                else if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    foreach (var item in pListeObjet)
                    {
                        donnesDatagrid.Remove(item);
                        donnesDatagrid.Add(item);
                    }
                }
                donnesDatagrid.OrderBy(p => p.PK_ID);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void TxtCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Parametrage);
            }
        }

        private void TxtLibelle_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Parametrage);
            }
        }
    }
}

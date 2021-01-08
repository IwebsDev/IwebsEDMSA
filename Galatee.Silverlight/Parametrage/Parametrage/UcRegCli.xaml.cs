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
    public partial class UcRegCli : ChildWindow
    {
        List<CsRegCli> listForInsertOrUpdate = null;
        ObservableCollection<CsRegCli> donnesDatagrid = new ObservableCollection<CsRegCli>();
        private CsRegCli ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcRegCli()
        {
            try
            {
                InitializeComponent();
                this.Txt_Regroupement.MaxLength = SessionObject.Enumere.TailleCodeRegroupement;
                Translate();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.RegroupementClient);
            }
        }
        public UcRegCli(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new CsRegCli();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsRegCli);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsRegCli>;
                InitialisationControle();
                this.Txt_Regroupement.MaxLength = SessionObject.Enumere.TailleCodeRegroupement;
                
                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.RegroupementClient);
            }
        }
        private void InitialisationControle()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllDenominationCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.RegroupementClient);
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    else
                    {

                        if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                        (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                        {
                            if (ObjetSelectionnee != null)
                            {
                            }
                        }
                    }

                    if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            Txt_Regroupement.Text = ObjetSelectionnee.CODE ?? string.Empty;
                            Txt_Libelle.Text = ObjetSelectionnee.NOM ?? string.Empty;
                            Txt_Adresse1.Text = ObjetSelectionnee.ADR1 ?? string.Empty;
                            Txt_Adresse2.Text = ObjetSelectionnee.ADR2 ?? string.Empty;
                            Txt_CodePostal.Text = ObjetSelectionnee.CODPOS ?? string.Empty;
                            Txt_Bureau.Text = ObjetSelectionnee.BUREAU ?? string.Empty;
                            Txt_Traitement.Text = ObjetSelectionnee.TRAITFAC ?? string.Empty;
                            btnOk.IsEnabled = false;
                        }
                    }
                };
                client.SelectAllDenominationAsync();
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
                client.SelectAllRegCliCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.RegroupementClient);
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
                  
                };
                client.SelectAllRegCliAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(CsRegCli pRegCli)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    GetDataNew();
                    //donnesDatagrid.Add(pRegCli);
                    //donnesDatagrid.OrderBy(p => p.PK_ID);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    GetDataNew();
                    //var regCli = donnesDatagrid.First(p => p.PK_ID == pRegCli.PK_ID);
                    //donnesDatagrid.Remove(regCli);
                    //donnesDatagrid.Add(pRegCli);
                    //donnesDatagrid.OrderBy(p => p.PK_ID);
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
                Title = Languages.RegroupementClient;
                btnOk.Content = Languages.OK;
                Btn_Annuler.Content = Languages.Annuler;
                GroupBox.Header = Languages.InformationsRegroupementClient;
                lab_Regroupement.Content = Languages.Regroupement;
                //lab_Denomination.Content = Languages.LibelleDenomination;
                lab_Libelle.Content = Languages.Libelle;
                lab_Adresse1.Content = Languages.Adresse + " 1";
                lab_Adresse2.Content = Languages.Adresse + " 2";
                lab_Bureau.Content = Languages.Bureau;
                lab_CodePostal.Content = Languages.CodePostal;
                lab_Traitement.Content = Languages.Traitement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsRegCli> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsRegCli>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var regCli = new CsRegCli();
                    regCli.CODE = Txt_Regroupement.Text;
                    regCli.BUREAU = Txt_Bureau.Text;
                    regCli.CODPOS = Txt_CodePostal.Text;
                    regCli.NOM = Txt_Libelle.Text;
                    regCli.ADR1 = Txt_Adresse1.Text;
                    regCli.ADR2 = Txt_Adresse2.Text;
                    regCli.TRAITFAC = Txt_Traitement.Text;
                    regCli.DATECREATION = DateTime.Now;
                    regCli.USERCREATION = UserConnecte.matricule;
                    if (!string.IsNullOrEmpty(Txt_Regroupement.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == regCli.CODE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(regCli);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.CODE = Txt_Regroupement.Text;
                    ObjetSelectionnee.BUREAU = Txt_Bureau.Text;
                    ObjetSelectionnee.CODPOS = Txt_CodePostal.Text;
                    ObjetSelectionnee.NOM = Txt_Libelle.Text;
                    ObjetSelectionnee.ADR1 = Txt_Adresse1.Text;
                    ObjetSelectionnee.ADR2 = Txt_Adresse2.Text;
                    ObjetSelectionnee.TRAITFAC = Txt_Traitement.Text;
                    ObjetSelectionnee.DATECREATION = DateTime.Now;
                    ObjetSelectionnee.USERCREATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.RegroupementClient);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.RegroupementClient, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertRegCliCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.RegroupementClient);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.RegroupementClient);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertRegCliAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateRegCliCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.RegroupementClient);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.RegroupementClient);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateRegCliAsync(listForInsertOrUpdate);
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
                Message.ShowError(ex.Message, Languages.RegroupementClient);
            }
        }
        
        private void VerifierSaisie()
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_Regroupement.Text) && !string.IsNullOrEmpty(Txt_Libelle.Text) && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation)
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
                Message.Show(ex.Message, Languages.RegroupementClient);
            }
        }

        private void Reinitialiser()
        {
            try
            {
                Txt_Regroupement.Text = string.Empty;
                //Cbo_Denomination.SelectedItem = null;
                Txt_Bureau.Text = string.Empty;
                Txt_CodePostal.Text = string.Empty;
                Txt_Libelle.Text = string.Empty;
                Txt_Adresse1.Text = string.Empty;
                Txt_Adresse2.Text = string.Empty;
                Txt_Traitement.Text = string.Empty;
                btnOk.IsEnabled = false;
                Txt_Regroupement.Focus();
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
                Message.ShowError(ex.Message, Languages.RegroupementClient);
            }
        }
    }
}



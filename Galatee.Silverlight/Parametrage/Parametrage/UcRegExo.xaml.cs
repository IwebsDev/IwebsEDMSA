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
    public partial class UcRegExo : ChildWindow
    {
        List<CsRegExo> listForInsertOrUpdate = null;
        ObservableCollection<CsRegExo> donnesDatagrid = new ObservableCollection<CsRegExo>();
        private CsRegExo ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcRegExo()
        {
            try
            {
                InitializeComponent();
                Translate();
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Exoneration);
            }
        }
        public UcRegExo(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new CsRegExo();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsRegExo);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsRegExo>;
                RemplirListeDeroulante();
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Exoneration);
            }
        }

        private void RemplirListeDeroulante()
        {
            try
            {
                // Charger Centre
                ParametrageClient clientcentre = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                clientcentre.SelectAllCentreCompleted += (scentre, argscentre) =>
                {
                    if (argscentre.Cancelled || argscentre.Error != null)
                    {
                        string error = argscentre.Error.Message;
                        Message.ShowError(error, Languages.Exoneration);
                    }
                    if (argscentre.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    
                    Cbo_Centre.Items.Clear();
                    if (argscentre.Result != null)
                        foreach (var item in argscentre.Result)
                        {
                            Cbo_Centre.Items.Add(item);
                        }

                    Cbo_Centre.SelectedValuePath = "PK_ID";
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";

                    if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsCentre centre in Cbo_Centre.Items)
                            {
                                if (centre.PK_ID == ObjetSelectionnee.PK_ID)
                                {
                                    Cbo_Centre.SelectedItem = centre;
                                    break;
                                }
                            }
                        }
                    }

                    // Charger produit
                    ParametrageClient clientProduit = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                    clientProduit.SelectAllProduitCompleted += (sProduit, argsProduit) =>
                    {
                        if (argsProduit.Cancelled || argsProduit.Error != null)
                        {
                            string error = argsProduit.Error.Message;
                            Message.ShowError(error, Languages.Exoneration);
                        }
                        if (argsProduit.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }
                        Cbo_Produit.Items.Clear();
                        if (argsProduit.Result != null)
                            foreach (var item in argsProduit.Result)
                            {
                                Cbo_Produit.Items.Add(item);
                            }

                        Cbo_Produit.SelectedValuePath = "PK_ID";
                        Cbo_Produit.DisplayMemberPath = "LIBELLE";

                        if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                        (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                        {
                            if (ObjetSelectionnee != null)
                            {
                                foreach (CsProduit produit in Cbo_Produit.Items)
                                {
                                    if (produit.PK_ID == ObjetSelectionnee.FK_IDPRODUIT)
                                    {
                                        Cbo_Produit.SelectedItem = produit;
                                        break;
                                    }
                                }
                            }
                        }
                    };
                    clientProduit.SelectAllProduitAsync();
                    // Charger regroupement client
                    ParametrageClient clientRegcli = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                    clientRegcli.SelectAllRegCliCompleted += (sRegcli, argsRegcli) =>
                    {
                        if (argsRegcli.Cancelled || argsRegcli.Error != null)
                        {
                            string error = argsRegcli.Error.Message;
                            Message.ShowError(error, Languages.Exoneration);
                        }
                        if (argsRegcli.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }
                        Cbo_Regroupement.Items.Clear();
                        if (argsRegcli.Result != null)
                            foreach (var item in argsRegcli.Result)
                            {
                                Cbo_Regroupement.Items.Add(item);
                            }

                        Cbo_Regroupement.SelectedValuePath = "PK_ID";
                        Cbo_Regroupement.DisplayMemberPath = "NOM";

                        if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                        (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                        {
                            if (ObjetSelectionnee != null)
                            {
                                foreach (CsRegCli regCli in Cbo_Regroupement.Items)
                                {
                                    if (regCli.PK_ID == ObjetSelectionnee.PK_ID)
                                    {
                                        Cbo_Regroupement.SelectedItem = regCli;
                                        break;
                                    }
                                }
                                Txt_Traitement.Text = ObjetSelectionnee.TRAITFAC ?? string.Empty;
                                Chk_Avance.IsChecked = ObjetSelectionnee.EXFAV == "1";
                                Chk_Dossier.IsChecked = ObjetSelectionnee.EXFDOS == "1";
                                Chk_Police.IsChecked = ObjetSelectionnee.EXFPOL == "1";
                                btnOk.IsEnabled = false;

                                //Cbo_Centre.IsEnabled = false;
                                //Cbo_Produit.IsEnabled = false;
                                //Cbo_Regroupement.IsEnabled = false;

                                VerifierSaisie();
                            }
                        }

                    };
                    clientRegcli.SelectAllRegCliAsync();

                };
                clientcentre.SelectAllCentreAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        private void UpdateParentList(CsRegExo pRegCli)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    donnesDatagrid.Add(pRegCli);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    var regCli = donnesDatagrid.First(p => p.PK_ID == pRegCli.PK_ID);
                    donnesDatagrid.Remove(regCli);
                    donnesDatagrid.Add(pRegCli);
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
                Title = Languages.RegroupementExoneration;
                btnOk.Content = Languages.OK;
                Btn_Annuler.Content = Languages.Annuler;
                lab_Regroupement.Content = Languages.Regroupement;
                lab_Produit.Content = Languages.Produit;
                lab_Centre.Content = Languages.Centre;
                lab_Traitement.Content = Languages.Traitement;
                Chk_Avance.Content = Languages.Avance;
                Chk_Dossier.Content = Languages.Dossier;
                Chk_Police.Content = Languages.Police;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsRegExo> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsRegExo>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var regCli = new CsRegExo();
                    if (Cbo_Regroupement.SelectedItem != null)
                    {
                        regCli.FK_IDREGCLI = ((CsRegCli)Cbo_Regroupement.SelectedItem).PK_ID;
                        regCli.REGCLI = ((CsRegCli)Cbo_Regroupement.SelectedItem).CODE;
                        regCli.LIBELLEREGCLI = ((CsRegCli)Cbo_Regroupement.SelectedItem).NOM;
                    }
                    if (Cbo_Produit.SelectedItem != null)
                    {
                        regCli.FK_IDPRODUIT = ((CsProduit)Cbo_Produit.SelectedItem).PK_ID;
                        regCli.LIBELLEPRODUIT = ((CsProduit)Cbo_Produit.SelectedItem).LIBELLE;
                    }
                    if (Cbo_Centre.SelectedItem != null)
                    {
                        regCli.FK_IDCENTRE = ((CsCentre)Cbo_Centre.SelectedItem).PK_ID;
                        regCli.LIBELLECENTRE = ((CsCentre)Cbo_Centre.SelectedItem).LIBELLE;
                    }
                    regCli.EXFAV = Chk_Avance.IsChecked != null && (bool)Chk_Avance.IsChecked ? "1" : "0";
                    regCli.EXFDOS = Chk_Dossier.IsChecked != null && (bool)Chk_Dossier.IsChecked ? "1" : "0";
                    regCli.EXFPOL = Chk_Police.IsChecked != null && (bool)Chk_Police.IsChecked ? "1" : "0";
                    regCli.TRAITFAC = Txt_Traitement.Text;
                    regCli.DATECREATION = DateTime.Now;
                    regCli.USERCREATION = UserConnecte.matricule;
                    if (Cbo_Regroupement.SelectedItem != null && Cbo_Produit.SelectedItem != null &&
                        Cbo_Centre.SelectedItem != null && donnesDatagrid.FirstOrDefault(p => p.REGCLI == regCli.REGCLI && p.PRODUIT == regCli.PRODUIT  && p.CENTRE == regCli.CENTRE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(regCli);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    if (Cbo_Regroupement.SelectedItem != null)
                        ObjetSelectionnee.REGCLI = ((CsRegCli)Cbo_Regroupement.SelectedItem).CODE;
                    if (Cbo_Produit.SelectedItem != null)
                        ObjetSelectionnee.FK_IDPRODUIT = ((CsProduit)Cbo_Produit.SelectedItem).PK_ID;
                    if (Cbo_Centre.SelectedItem != null)
                        ObjetSelectionnee.FK_IDCENTRE = ((CsCentre)Cbo_Centre.SelectedItem).PK_ID;
                    ObjetSelectionnee.EXFAV = Chk_Avance.IsChecked != null && (bool) Chk_Avance.IsChecked ? "1" : "0";
                    ObjetSelectionnee.EXFDOS = Chk_Dossier.IsChecked != null && (bool)Chk_Dossier.IsChecked ? "1" : "0";
                    ObjetSelectionnee.EXFPOL = Chk_Police.IsChecked != null && (bool)Chk_Police.IsChecked ? "1" : "0";
                    ObjetSelectionnee.TRAITFAC = Txt_Traitement.Text;
                    ObjetSelectionnee.DATEMODIFICATION = DateTime.Now;
                    ObjetSelectionnee.USERMODIFICATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Exoneration);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Exoneration, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertRegExoCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.Exoneration);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Exoneration);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertRegExoAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateRegExoCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.Exoneration);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Exoneration);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateRegExoAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.Exoneration);
            }
        }
        
        private void VerifierSaisie()
        {
            try
            {
                if (Cbo_Centre.SelectedItem != null && Cbo_Produit.SelectedItem != null && Cbo_Regroupement.SelectedItem != null && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation)
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
                Message.ShowError(ex.Message, Languages.Exoneration);
            }
        }

        private void Reinitialiser()
        {
            try
            {
                Cbo_Centre.SelectedItem = null;
                Cbo_Produit.SelectedItem = null;
                Cbo_Regroupement.SelectedItem = null;
                Txt_Traitement.Text = string.Empty;
                btnOk.IsEnabled = false;
                Cbo_Centre.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Exoneration);
            }
        }
    }
}



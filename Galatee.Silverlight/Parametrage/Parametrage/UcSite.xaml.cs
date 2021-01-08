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
    public partial class UcSite : ChildWindow
    {
        List<CsSite> listForInsertOrUpdate = null;
        ObservableCollection<CsSite> donnesDatagrid = new ObservableCollection<CsSite>();
        private CsSite ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcSite()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Site);
            }
        }
        public UcSite(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var site = new CsSite();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(site, pObjects[0] as CsSite);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsSite>;
                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_Site.Text = ObjetSelectionnee.CODE  ?? string.Empty;
                        Txt_Serveur.Text = ObjetSelectionnee.SERVEUR ?? string.Empty;
                        Txt_Libelle.Text = ObjetSelectionnee.LIBELLE ?? string.Empty;
                        Txt_Catalogue.Text = ObjetSelectionnee.CATALOGUE ?? string.Empty;
                        Txt_Utilisateur.Text = ObjetSelectionnee.USERID ?? string.Empty;
                        Txt_Pwd.Password = Security.Cryptage.Decrypt(ObjetSelectionnee.PWD) ?? string.Empty;
                        Txt_ConfirmationPwd.Password = Security.Cryptage.Decrypt(ObjetSelectionnee.PWD) ?? string.Empty;
                        Txt_Libelle.Focus();
                        btnOk.IsEnabled = false;

                        //Txt_Site.IsReadOnly = true;
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
                Message.ShowError(ex.Message, Languages.Site);
            }
        }

        private void GetDataNew()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllSitesCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Site);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var pSite in args.Result)
                        {
                           donnesDatagrid.Add(pSite);
                        }
                    
                };
                client.SelectAllSitesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(CsSite pSite)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    GetDataNew();
                    //donnesDatagrid.Add(pSite);
                    //donnesDatagrid.OrderBy(p=>p.PK_ID);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    GetDataNew();
                    //var Site = donnesDatagrid.First(p => p.PK_ID == pSite.PK_ID);
                    //donnesDatagrid.Remove(Site);
                    //donnesDatagrid.Add(pSite);
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
                Title = Languages.Site;
                btnOk.Content = Languages.OK;
                Btn_Annuler.Content = Languages.Annuler;
                GroupBox.Header = Languages.Site;
                lab_Site.Content = Languages.Site;
                lab_Serveur.Content = Languages.Serveur;
                lab_Catalogue.Content = Languages.Catalogue;
                lab_pwd.Content = Languages.MotDePasse;
                lab_Utilisateur.Content = Languages.Utilisateur;
                lab_ConfirmationPwd.Content = Languages.Confirmation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsSite> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsSite>();
            try
            {
                if(Txt_Pwd.Password != Txt_ConfirmationPwd.Password)
                    throw new Exception(Languages.MotDePasseIncorrect);
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    Txt_Site.Focus();
                    var Site = new CsSite
                    {
                        CODE  = Txt_Site.Text,
                        SERVEUR = Txt_Serveur.Text,
                        LIBELLE = Txt_Libelle.Text,
                        CATALOGUE = Txt_Catalogue.Text,
                        PWD = Galatee.Silverlight.Security.Cryptage.Encrypt(Txt_Pwd.Password),
                        NUMERODEMANDE="1",
                        NUMEROFACTURE="1",
                        DATECREATION = DateTime.Now,
                        USERID = Txt_Utilisateur.Text,
                        USERCREATION = UserConnecte.matricule
                    };
                    if (!string.IsNullOrEmpty(Txt_Site.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE  != null && p.CODE  == Site.CODE ) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(Site);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.CODE  = Txt_Site.Text;
                    ObjetSelectionnee.SERVEUR = Txt_Serveur.Text;
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text;
                    ObjetSelectionnee.CATALOGUE = Txt_Catalogue.Text;
                    ObjetSelectionnee.USERID = Txt_Utilisateur.Text;
                    ObjetSelectionnee.NUMERODEMANDE="1";
                    ObjetSelectionnee.NUMEROFACTURE = "1";
                    ObjetSelectionnee.PWD = Security.Cryptage.Encrypt(Txt_Pwd.Password);
                    ObjetSelectionnee.DATECREATION = DateTime.Now;
                    ObjetSelectionnee.USERCREATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Site);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Site, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertSiteCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.Site);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Site);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertSiteAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateSiteCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.Site);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Site);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateSiteAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.Site);
            }
        }
        
        private void VerifierSaisie()
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_Site.Text) && !string.IsNullOrEmpty(Txt_Libelle.Text) && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation)
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
                Reinitialiser();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Site);
            }
        }

        private void Reinitialiser()
        {
            try
            {
                Txt_Site.Text = string.Empty;
                Txt_Serveur.Text = string.Empty;
                Txt_Libelle.Text = string.Empty;
                Txt_Catalogue.Text = string.Empty;
                Txt_Pwd.Password = string.Empty;
                Txt_ConfirmationPwd.Password = string.Empty;
                btnOk.IsEnabled = false;
                Txt_Site.Focus();
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
                Message.ShowError(ex.Message, Languages.Site);
            }
        }
    }
}



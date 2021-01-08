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
    public partial class UcDomBanc : ChildWindow
    {
        List<CsDomBanc> listForInsertOrUpdate = null;
        ObservableCollection<CsDomBanc> donnesDatagrid = new ObservableCollection<CsDomBanc>();
        private CsDomBanc ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcDomBanc()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.DomiciliationBancaire);
            }
        }
        public UcDomBanc(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new CsDomBanc();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsDomBanc);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                RemplirListeDesBanques();
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsDomBanc>;
                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_Banque.Text = ObjetSelectionnee.BANQUE ?? string.Empty;
                        Txt_Guichet.Text = ObjetSelectionnee.GUICHET ?? string.Empty;
                        Txt_Libelle.Text = ObjetSelectionnee.LIBELLE ?? string.Empty;
                        Txt_Compte.Text = ObjetSelectionnee.COMPTE ?? string.Empty;
                        Txt_Comptabilite.Text = ObjetSelectionnee.COMPTA ?? string.Empty;
                        btnOk.IsEnabled = false;

                        //Txt_Banque.IsReadOnly = true;
                        //Txt_Guichet.IsReadOnly = true;
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
                Message.ShowError(ex.Message, Languages.DomiciliationBancaire);
            }
        }

        private void RemplirListeDesBanques()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllBanqueCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    else
                    {
                        this.CboBanque.ItemsSource = args.Result;
                        this.CboBanque.DisplayMemberPath = "LIBELLE";
                        this.CboBanque.SelectedValuePath = "PK_ID";

                        if (ObjetSelectionnee != null)
                        {
                            foreach (aBanque banque in CboBanque.ItemsSource)
                            {
                                if (banque.PK_ID == ObjetSelectionnee.FK_IDBANQUE)
                                {
                                    CboBanque.SelectedItem = banque;
                                    break;
                                }
                                //CboBanque.IsEnabled = false;
                            }
                        }
                    }
                };
                client.SelectAllBanqueAsync();
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
                client.SelectAllDomBancCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        throw new Exception(error);
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    donnesDatagrid.Clear();
                    foreach (var pDomBanc in args.Result)
                    {
                        donnesDatagrid.Add(pDomBanc);
                    }
                   
                };
                client.SelectAllDomBancAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(CsDomBanc pDomBanc)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    GetDataNew();
                    //donnesDatagrid.Add(pDomBanc);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    GetDataNew();
                    //var DomBanc = donnesDatagrid.First(p => p.PK_ID == pDomBanc.PK_ID);
                    //donnesDatagrid.Remove(DomBanc);
                    //donnesDatagrid.Add(pDomBanc);
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
                Title = Languages.DomiciliationBancaire;
                btnOk.Content = Languages.OK;
                Btn_Reinitialiser.Content = Languages.Annuler;
                GroupBox.Header = Languages.InformationsDomBanc;
                lab_Banque.Content = Languages.DomiciliationBancaire;
                lab_Guichet.Content = Languages.Guichet;
                lab_Compte.Content = Languages.Compte;
                lab_Comptabilite.Content = Languages.Comptabilite;
                lab_Libelle.Content = Languages.Libelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsDomBanc> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsDomBanc>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var DomBanc = new CsDomBanc
                    {
                        BANQUE = Txt_Banque.Text,
                        GUICHET = Txt_Guichet.Text,
                        LIBELLE = Txt_Libelle.Text,
                        COMPTE = Txt_Compte.Text,
                        COMPTA = Txt_Comptabilite.Text,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule
                    };
                    if (!string.IsNullOrEmpty(Txt_Banque.Text) && !string.IsNullOrEmpty(Txt_Guichet.Text) && donnesDatagrid.FirstOrDefault(p => p.GUICHET != null && p.BANQUE != null && p.BANQUE == DomBanc.BANQUE && p.GUICHET == DomBanc.GUICHET) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(DomBanc);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.BANQUE = Txt_Banque.Text;
                    ObjetSelectionnee.GUICHET = Txt_Guichet.Text;
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text;
                    ObjetSelectionnee.COMPTE = Txt_Compte.Text;
                    ObjetSelectionnee.COMPTA = Txt_Comptabilite.Text;
                    ObjetSelectionnee.DATEMODIFICATION = DateTime.Now;
                    ObjetSelectionnee.USERMODIFICATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.DomiciliationBancaire);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.DomiciliationBancaire, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertDomBancCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.DomiciliationBancaire);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.DomiciliationBancaire);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertDomBancAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateDomBancCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.DomiciliationBancaire);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.DomiciliationBancaire);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateDomBancAsync(listForInsertOrUpdate);
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
                Message.ShowError(ex.Message, Languages.DomiciliationBancaire);
            }
        }
        
        private void VerifierSaisie()
        {
            try
            {
                if (CboBanque.SelectedItem != null && !string.IsNullOrEmpty(Txt_Banque.Text) && !string.IsNullOrEmpty(Txt_Guichet.Text) && !string.IsNullOrEmpty(Txt_Libelle.Text) && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation)
                    btnOk.IsEnabled = false;
                else
                {
                    btnOk.IsEnabled = true;
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
                Message.ShowError(ex.Message, Languages.DomiciliationBancaire);
            }
        }

        private void Reinitialiser()
        {
            try
            {
                Txt_Banque.Text = string.Empty;
                Txt_Guichet.Text = string.Empty;
                Txt_Libelle.Text = string.Empty;
                Txt_Compte.Text = string.Empty;
                Txt_Comptabilite.Text = string.Empty;
                btnOk.IsEnabled = false;
                Txt_Banque.Focus();
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
                Message.ShowError(ex.Message, Languages.DomiciliationBancaire);
            }
        }

        private void CboBanque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CboBanque.SelectedItem != null)
                {
                    var banque = (aBanque)CboBanque.SelectedItem;
                    if (banque != null)
                    {
                        Txt_Banque.Text = banque.CODE ?? string.Empty;
                     //   Txt_Guichet.Text = banque.CODEGUICHET ?? string.Empty;
                    }
                }
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.DomiciliationBancaire);
            }
        }
    }
}



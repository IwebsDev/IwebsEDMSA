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
    public partial class UcDiametrecompteur : ChildWindow
    {
        List <CsDiacomp> listForInsertOrUpdate = null;
        ObservableCollection<CsDiacomp> donnesDatagrid = new ObservableCollection<CsDiacomp>();
        private CsDiacomp ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        public UcDiametrecompteur()
        {
            InitializeComponent();
            //Translate();
        }
        public UcDiametrecompteur(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                //Translate();
                var categorieClient = new CsDiacomp();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsDiacomp);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                RemplirProduit();
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsDiacomp>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_Code.Text = ObjetSelectionnee.CODE;
                        Txt_Libelle.Text = ObjetSelectionnee.LIBELLE;
                        Txt_BNI.Text = Convert.ToString(ObjetSelectionnee.BNI);
                        Txt_BNS.Text = Convert.ToString(ObjetSelectionnee.BNS);
                        Txt_CFI.Text = Convert.ToString(ObjetSelectionnee.CFI);
                        Txt_CFS.Text = Convert.ToString(ObjetSelectionnee.CFS);
                        btnOk.IsEnabled = false;

                    }
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Diametrecompteur);
            }
        }

        private void RemplirProduit()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                client.SelectAllProduitCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.Produit);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    else
                    {
                        CboProduit.ItemsSource = args.Result;
                        CboProduit.SelectedValuePath = "PK_ID";
                        CboProduit.DisplayMemberPath = "LIBELLE";

                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsProduit produit in CboProduit.ItemsSource)
                            {
                                if (produit.PK_ID == ObjetSelectionnee.FK_IDPRODUIT)
                                {
                                    CboProduit.SelectedItem = produit;
                                    break;
                                }
                            }
                            CboProduit.IsEnabled = false;
                        }
                    }
                };
                client.SelectAllProduitAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void VerifierSaisie()
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_Code.Text) && !string.IsNullOrEmpty(Txt_Libelle.Text) && !string.IsNullOrEmpty(Txt_BNI.Text) && !string.IsNullOrEmpty(Txt_BNS.Text) && !string.IsNullOrEmpty(Txt_CFI.Text) && !string.IsNullOrEmpty(Txt_CFS.Text) && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation
                    && CboProduit.SelectedItem != null)
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
                Txt_BNI.Text = string.Empty;
                Txt_BNS.Text = string.Empty;
                Txt_CFI.Text = string.Empty;
                Txt_CFS.Text = string.Empty;
                CboProduit.SelectedItem = string.Empty;
                btnOk.IsEnabled = false;
                Txt_Code.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

        private void OnComboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Forfait);
            }
        }

        private void GetDataNew()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                client.SelectAllDiacompCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Preriodicitefacturation);
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
                client.SelectAllDiacompAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateParentList(CsDiacomp pDiacomp)
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

        private List<CsDiacomp> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsDiacomp>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var Diacomp = new CsDiacomp
                    {
                        CODE = Txt_Code.Text,
                        LIBELLE = Txt_Libelle.Text,
                        BNI = Convert.ToInt32(Txt_BNI.Text),
                        BNS = Convert.ToInt32(Txt_BNS.Text),
                        CFI = Convert.ToInt32(Txt_CFI.Text),
                        CFS = Convert.ToInt32(Txt_CFS.Text),
                        FK_IDPRODUIT = ((CsProduit)CboProduit.SelectedItem).PK_ID,
                        PRODUIT = ((CsProduit)CboProduit.SelectedItem).CODE,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule
                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == Diacomp.CODE && p.PRODUIT == Diacomp.PRODUIT) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(Diacomp);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.CODE = Txt_Code.Text;
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text;
                     ObjetSelectionnee.BNI = Convert.ToInt32(Txt_BNI.Text);
                     ObjetSelectionnee.BNS = Convert.ToInt32(Txt_BNS.Text);
                     ObjetSelectionnee.CFI = Convert.ToInt32(Txt_CFI.Text);
                     ObjetSelectionnee.CFS = Convert.ToInt32(Txt_CFS.Text);
                     ObjetSelectionnee.FK_IDPRODUIT = ((CsProduit)CboProduit.SelectedItem).PK_ID;
                    ObjetSelectionnee.PRODUIT = ((CsProduit)CboProduit.SelectedItem).CODE;
                    ObjetSelectionnee.DATECREATION = DateTime.Now;
                    ObjetSelectionnee.USERCREATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Diametrecompteur);
                return null;
            }
        }


        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Diametrecompteur, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        listForInsertOrUpdate = GetInformationsFromScreen();
                        var service = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));

                        if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        {
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                            {
                                service.InsertDiacompCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.Diametrecompteur);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Diametrecompteur);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertDiacompAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateDiacompCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.Diametrecompteur);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Diametrecompteur);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateDiacompAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.Diametrecompteur);
            }
        }
    

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
   
    }
}


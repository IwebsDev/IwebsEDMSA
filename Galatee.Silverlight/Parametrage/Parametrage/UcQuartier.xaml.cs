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
    public partial class UcQuartier : ChildWindow
    {
        List<CsQuartier> listForInsertOrUpdate = null;
        ObservableCollection<CsQuartier> donnesDatagrid = new ObservableCollection<CsQuartier>();
        private CsQuartier ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcQuartier()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Quartier);
            }
        }
        public UcQuartier(CsQuartier pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var Quartier = new CsQuartier();
                if (pObject != null)
                    ObjetSelectionnee = Utility.ParseObject(Quartier, pObject as CsQuartier);
                ModeExecution = pExecMode;
                dataGrid = pGrid;
                RemplirListeDesCentreExistant();
                RemplirCommune();
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsQuartier>;
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
                Message.ShowError(ex.Message, Languages.Quartier);
            }
        }
        private void GetData()
        {
            //int back = 0;
            try
            {
                //back = LoadingManager.BeginLoading("Veuillez patienter s'il vous plaît, chargement des données en cours...");
                LayoutRoot.Cursor = Cursors.Wait;
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllQuartierCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.Show(error, Languages.Parametrage);
                        //LoadingManager.EndLoading(back);
                        return;
                    }
                    if (args.Result == null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Quartier);
                        //LoadingManager.EndLoading(back);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var pQuartier in args.Result)
                        {
                            donnesDatagrid.Add(pQuartier);
                        }
                    //dtgrdParametre.ItemsSource = DonnesDatagrid;
                    //LoadingManager.EndLoading(back);
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.SelectAllQuartierAsync();
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                //LoadingManager.EndLoading(back);
                throw ex;
            }
        }
        private void UpdateParentList(CsQuartier pQuartier)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    GetData();
                    //donnesDatagrid.Add(pQuartier);
                    //donnesDatagrid.OrderBy(p => p.PK_ID);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    GetData();
                    //var Rues = donnesDatagrid.First(p => p.PK_ID == pQuartier.PK_ID );
                    //donnesDatagrid.Remove(Rues);
                    //donnesDatagrid.Add(pQuartier);
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
                //Title = Languages.Rue;
                //btnOk.Content = Languages.OK;
                //Btn_Reinitialiser.Content = Languages.Annuler;
                //GboCodeDepart.Header = Languages.InformationsCodePoste;
                //lab_Code.Content = Languages.Code;
                //lab_Libelle.Content = Languages.Libelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsQuartier> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsQuartier>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var Quartier = new CsQuartier
                    {
                        CODE = Txt_Code.Text,
                        COMMUNE = ((CsCommune)CboCommune.SelectedItem).CODE,
                        FK_IDCOMMUNE = ((CsCommune)CboCommune.SelectedItem).PK_ID,
                        LIBELLE = Txt_Libelle.Text,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule
                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == Quartier.CODE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(Quartier);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.CODE = Txt_Code.Text;
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text;
                    ObjetSelectionnee.COMMUNE = ((CsCommune)CboCommune.SelectedItem).CODE;
                    ObjetSelectionnee.FK_IDCOMMUNE = ((CsCommune)CboCommune.SelectedItem).PK_ID;
                    ObjetSelectionnee.DATEMODIFICATION = DateTime.Now;
                    ObjetSelectionnee.USERMODIFICATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Quartier);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Quartier, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertQuartierCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.Quartier);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Quartier);
                                        return;
                                    }
                                    //UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertQuartierAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.InsertQuartierCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.Quartier);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Quartier);
                                        return;
                                    }
                                    //UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertQuartierAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.Quartier);
            }
        }

        private void RemplirListeDesCentreExistant()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllCentreCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Quartier);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    else
                    {
                        this.CboCentre.ItemsSource = args.Result;
                        this.CboCentre.DisplayMemberPath = "LIBELLE";
                        this.CboCentre.SelectedValuePath = "PK_ID";

                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsCentre centre in CboCentre.ItemsSource)
                            {
                                if (centre.PK_ID == ObjetSelectionnee.PK_ID)
                                {
                                    CboCentre.SelectedItem = centre;
                                    break;
                                }
                            }
                        }
                    }
                };
                client.SelectAllCentreAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirCommune()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllCommuneCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.Quartier);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    else
                    {
                        CboCommune.ItemsSource = args.Result;
                        CboCommune.SelectedValuePath = "PK_ID";
                        CboCommune.DisplayMemberPath = "LIBELLE";

                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsCommune commune in CboCommune.ItemsSource)
                            {
                                if (commune.PK_ID == ObjetSelectionnee.FK_IDCOMMUNE)
                                {
                                    CboCommune.SelectedItem = commune;
                                    break;
                                }
                            }
                        }
                    }
                };
                client.SelectAllCommuneAsync();
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
                if (!string.IsNullOrEmpty(Txt_Code.Text) && !string.IsNullOrEmpty(Txt_Libelle.Text) && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation
                    && CboCentre.SelectedItem != null && CboCommune.SelectedItem != null)
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
                Message.ShowError(ex.Message, Languages.Quartier);
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
                Message.ShowError(ex.Message, Languages.Quartier);
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
                Message.ShowError(ex.Message, Languages.Quartier);
            }
        }
    }
}



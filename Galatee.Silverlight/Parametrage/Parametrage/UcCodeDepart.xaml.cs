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
    public partial class UcCodeDepart : ChildWindow
    {
        List<CsCodeDepart> listForInsertOrUpdate = null;
        ObservableCollection<CsCodeDepart> donnesDatagrid = new ObservableCollection<CsCodeDepart>();
        private CsCodeDepart ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        List<CsPosteSource> _listeDesPosteSourceInitiale = new List<CsPosteSource>();

        public UcCodeDepart()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleCodeDepart);
            }
        }
        public UcCodeDepart(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new CsCodeDepart();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsCodeDepart);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                RemplirPosteSource();
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsCodeDepart>;
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
                Message.ShowError(ex.Message, Languages.LibelleCodeDepart);
            }
        }





        private void RemplirPosteSource()
        {

            try
            {
                if (_listeDesPosteSourceInitiale != null && _listeDesPosteSourceInitiale.Count != 0)
                {
                    Cbo_PosteSource.ItemsSource = _listeDesPosteSourceInitiale.OrderBy(t => t.LIBELLE).ToList();
                    Cbo_PosteSource.IsEnabled = true;
                    Cbo_PosteSource.SelectedValuePath = "PK_ID";
                    Cbo_PosteSource.DisplayMemberPath = "LIBELLE";
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPosteSourceAsync();
                service.ChargerPosteSourceCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;

                    _listeDesPosteSourceInitiale.Clear();

                    List<CsPosteSource> _listeDesPostesExistant = new List<CsPosteSource>();
                    CsPosteSource st = null;
                    foreach (ServiceAccueil.CsPosteSource item in args.Result)
                    {
                        st = new CsPosteSource();
                        st.PK_ID = item.PK_ID;
                        st.CODE = item.CODE;
                        st.LIBELLE = item.LIBELLE;

                        _listeDesPostesExistant = _listeDesPosteSourceInitiale.Where(t => t.LIBELLE == st.LIBELLE).ToList();

                        if (_listeDesPostesExistant == null || _listeDesPostesExistant.Count == 0)
                            _listeDesPosteSourceInitiale.Add(st);
                    }
                    Cbo_PosteSource.ItemsSource = _listeDesPosteSourceInitiale.OrderBy(t => t.LIBELLE).ToList();
                    Cbo_PosteSource.IsEnabled = true;
                    Cbo_PosteSource.SelectedValuePath = "PK_ID";
                    Cbo_PosteSource.DisplayMemberPath = "LIBELLE";

                    if (ObjetSelectionnee != null && ObjetSelectionnee.FK_IDPOSTESOURCE > 0)
                        Cbo_PosteSource.SelectedItem = _listeDesPosteSourceInitiale.FirstOrDefault(t => t.PK_ID == ObjetSelectionnee.FK_IDPOSTESOURCE);


                };
                service.CloseAsync();
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
                client.SelectAllCodeDepartCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.LibelleCodeDepart);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var pCodeDepart in args.Result)
                        {
                            donnesDatagrid.Add(pCodeDepart);
                        }
                   
                };
                client.SelectAllCodeDepartAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(CsCodeDepart pCodeDepart)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    GetDataNew();
                    //donnesDatagrid.Add(pCodeDepart);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    GetDataNew();
                    //var nationalite = donnesDatagrid.First(p => p.OriginalCODE == pCodeDepart.OriginalCODE);
                    //donnesDatagrid.Remove(nationalite);
                    //donnesDatagrid.Add(pCodeDepart);
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
                Title = Languages.LibelleCodeDepart;
                btnOk.Content = Languages.OK;
                Btn_Reinitialiser.Content = Languages.Annuler;
                GboCodeDepart.Header = Languages.InformationsCodeDepart;
                lab_Code.Content = Languages.Code;
                lab_Libelle.Content = Languages.Libelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsCodeDepart> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsCodeDepart>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var CodeDepart = new CsCodeDepart
                    {
                        CODE = Txt_Code.Text,
                        LIBELLE = Txt_Libelle.Text,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule,
                        FK_IDPOSTESOURCE = (Cbo_PosteSource.SelectedItem as CsPosteSource).PK_ID
                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == CodeDepart.CODE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(CodeDepart);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.CODE = Txt_Code.Text;
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text;
                    ObjetSelectionnee.DATEMODIFICATION = DateTime.Now;
                    ObjetSelectionnee.USERMODIFICATION = UserConnecte.matricule;
                    ObjetSelectionnee.FK_IDPOSTESOURCE = (Cbo_PosteSource.SelectedItem as CsPosteSource).PK_ID;

                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleCodeDepart);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow("Depart HTA ", Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertCodeDepartCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.LibelleCodeDepart);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.LibelleCodeDepart);
                                        return;
                                    }
                                    //UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertCodeDepartAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateCodeDepartCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.LibelleCodeDepart);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.LibelleCodeDepart);
                                        return;
                                    }
                                    //UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateCodeDepartAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.LibelleCodeDepart);
            }
        }
      
        private void VerifierSaisie()
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_Code.Text) && !string.IsNullOrEmpty(Txt_Libelle.Text) && Cbo_PosteSource.SelectedItem != null && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation)
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
                DialogResult = false;
                //Reinitialiser();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleCodeDepart);
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
                Message.ShowError(ex.Message, Languages.LibelleCodeDepart);
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
                Message.ShowError(ex.Message, Languages.Commune);
            }
        }
    }
}



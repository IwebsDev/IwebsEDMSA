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
    public partial class UcCodePoste : ChildWindow
    {
        List<CsPosteTransformation> listForInsertOrUpdate = null;
        ObservableCollection<CsPosteTransformation> donnesDatagrid = new ObservableCollection<CsPosteTransformation>();
        private CsPosteTransformation ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        List<CsCodeDepart> _listeDesDepartHTAInitiale = new List<CsCodeDepart>();

        public UcCodePoste()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleCodePoste);
            }
        }
        public UcCodePoste(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new CsPosteTransformation();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsPosteTransformation);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                RemplirDepartHTA();
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsPosteTransformation>;
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
                Message.ShowError(ex.Message, Languages.LibelleCodePoste);
            }
        }





        private void RemplirDepartHTA()
        {

            try
            {
                if (_listeDesDepartHTAInitiale != null && _listeDesDepartHTAInitiale.Count != 0)
                {
                    Cbo_Depart.ItemsSource = _listeDesDepartHTAInitiale.OrderBy(t => t.LIBELLE).ToList();
                    Cbo_Depart.IsEnabled = true;
                    Cbo_Depart.SelectedValuePath = "PK_ID";
                    Cbo_Depart.DisplayMemberPath = "LIBELLE";
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerDepartHTAAsync();
                service.ChargerDepartHTACompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;

                    _listeDesDepartHTAInitiale.Clear();

                    List<CsCodeDepart> _listeDesPostesExistant = new List<CsCodeDepart>();
                    CsCodeDepart st = null;
                    foreach (var item in args.Result)
                    {
                        st = new CsCodeDepart();
                        st.PK_ID = item.PK_ID;
                        st.CODE = item.CODE;
                        st.LIBELLE = item.LIBELLE;

                        _listeDesPostesExistant = _listeDesDepartHTAInitiale.Where(t => t.LIBELLE == st.LIBELLE).ToList();

                        if (_listeDesPostesExistant == null || _listeDesPostesExistant.Count == 0)
                            _listeDesDepartHTAInitiale.Add(st);
                    }
                    Cbo_Depart.ItemsSource = _listeDesDepartHTAInitiale.OrderBy(t => t.LIBELLE).ToList();
                    Cbo_Depart.IsEnabled = true;
                    Cbo_Depart.SelectedValuePath = "PK_ID";
                    Cbo_Depart.DisplayMemberPath = "LIBELLE";

                    if (ObjetSelectionnee != null && ObjetSelectionnee.FK_IDDEPARTHTA > 0)
                        Cbo_Depart.SelectedItem = _listeDesDepartHTAInitiale.FirstOrDefault(t => t.PK_ID == ObjetSelectionnee.FK_IDDEPARTHTA);

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
                client.SelectAllPosteTransformationCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.ListeCodePoste);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var pCodePoste in args.Result)
                        {
                            donnesDatagrid.Add(pCodePoste);
                        }
                    //dtgrdParametre.ItemsSource = DonnesDatagrid;
                };
                client.SelectAllPosteTransformationAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(CsPosteTransformation pCodePoste)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    GetDataNew();
                    //donnesDatagrid.Add(pCodePoste);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    GetDataNew();
                    //var nationalite = donnesDatagrid.First(p => p.PK_ID == pCodePoste.PK_ID);
                    //donnesDatagrid.Remove(nationalite);
                    //donnesDatagrid.Add(pCodePoste);
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
                Title = Languages.LibelleCodePoste;
                btnOk.Content = Languages.OK;
                Btn_Reinitialiser.Content = Languages.Annuler;
                GboCodeDepart.Header = Languages.InformationsCodePoste;
                lab_Code.Content = Languages.Code;
                lab_Libelle.Content = Languages.Libelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsPosteTransformation> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsPosteTransformation>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var CodePoste = new CsPosteTransformation
                    {
                        CODE = Txt_Code.Text,
                        LIBELLE = Txt_Libelle.Text,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule,
                        FK_IDDEPARTHTA = (Cbo_Depart.SelectedItem as CsCodeDepart).PK_ID

                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == CodePoste.CODE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(CodePoste);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.CODE = Txt_Code.Text;
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text;
                    ObjetSelectionnee.DATEMODIFICATION = DateTime.Now;
                    ObjetSelectionnee.USERMODIFICATION = UserConnecte.matricule;
                    ObjetSelectionnee.FK_IDDEPARTHTA = (Cbo_Depart.SelectedItem as CsCodeDepart).PK_ID;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleCodePoste);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow("Poste de transformation", Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertPosteTransformationCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.LibelleCodePoste);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.LibelleCodePoste);
                                        return;
                                    }
                                    //UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;

                                };
                                service.InsertPosteTransformationAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdatePosteTransformationCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.LibelleCodePoste);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.LibelleCodePoste);
                                        return;
                                    }
                                    //UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdatePosteTransformationAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.LibelleCodePoste);
            }
        }
         
        private void VerifierSaisie()
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_Code.Text) && !string.IsNullOrEmpty(Txt_Libelle.Text) && Cbo_Depart.SelectedItem != null && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation)
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
                Message.ShowError(ex.Message, Languages.LibelleCodePoste);
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
                Message.ShowError(ex.Message, Languages.LibelleCodePoste);
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



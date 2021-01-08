using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceParametrage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class CwListeFonction : ChildWindow
    {
        public CsFonction ObjetSelectionne { get; set; }

        ObservableCollection<CsFonction> donnesDatagrid = new ObservableCollection<CsFonction>();

        public CwListeFonction()
        {
            try
            {
                InitializeComponent();
                GetData();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Fonction);
            }
        }

        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion

        public ObservableCollection<CsFonction> DonnesDatagrid
        {
            get { return donnesDatagrid; }
            set
            {
                if (value == donnesDatagrid)
                    return;
                donnesDatagrid = value;
                NotifyPropertyChanged("DonnesDatagrid");
            }
        }

        private void GetData()
        {
            int back = 0;
            try
            {
                back = LoadingManager.BeginLoading("Chargement des données en cours...");
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllFonctionCompleted += (ssender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.Show(error, Languages.Parametrage);
                            LoadingManager.EndLoading(back);
                            return;
                        }
                        if (args.Result == null)
                        {
                            Message.Show(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            LoadingManager.EndLoading(back);
                            return;
                        }
                        DonnesDatagrid.Clear();
                        if (args.Result != null)
                            foreach (var item in args.Result)
                            {
                                DonnesDatagrid.Add(item);
                            }
                        dtgrdParametre.ItemsSource = DonnesDatagrid;
                        LoadingManager.EndLoading(back);
                    }
                    catch (Exception ex)
                    {
                        Message.Show(ex.Message, Languages.Parametrage);
                    }
                };
                client.SelectAllFonctionAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(back);
                throw ex;
            }
        }

        private void Supprimer()
        {
            try
            {
                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeASupprimer);
                if (dtgrdParametre.SelectedItem != null && dtgrdParametre.SelectedItems.Count > 0)
                {

                    var w = new MessageBoxControl.MessageBoxChildWindow(Languages.Parametrage, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                        {
                            var selected = dtgrdParametre.SelectedItem as CsFonction;
                            if (selected != null)
                            {
                                ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                 Utility.EndPoint("Parametrage"));
                                delete.DeleteFonctionCompleted += (del, argDel) =>
                                {
                                    if (argDel.Cancelled || argDel.Error != null)
                                    {
                                        Message.Show(argDel.Error.Message, Languages.Parametrage);
                                        return;
                                    }
                                    if (argDel.Result == false)
                                    {
                                        Message.Show(Languages.ErreurSuppressionDonnees, Languages.Parametrage);
                                        return;
                                    }
                                    DonnesDatagrid.Remove(selected);
                                };
                                delete.DeleteFonctionAsync(selected);
                            }
                        }
                        else
                        {
                            return;
                        }
                    };
                    w.Show();
                }
                else
                {
                    throw new Exception(Languages.SelectionnerUnElement);
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Parametrage);
            }
        }

        private void Imprimer()
        {
            var dictionaryParam = new Dictionary<string, string>();
            try
            {
                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeAImprimer);
                dictionaryParam.Add("RptParam_Code", Languages.ColonneCode.ToUpper());
                dictionaryParam.Add("RptParam_DateCreation", Languages.DateCreation);
                dictionaryParam.Add("RptParam_DateModification", Languages.DateModification);
                dictionaryParam.Add("RptParam_UserCreation", Languages.UserCreation);
                dictionaryParam.Add("RptParam_UserModification", Languages.UserModification);
                dictionaryParam.Add("RptParam_RoleName", Languages.ColonneRoleName.ToUpper());
                dictionaryParam.Add("RptParam_RoleDisplayName", Languages.ColonneRoleDisplayName.ToUpper());
                dictionaryParam.Add("RptParam_EstAdmin", Languages.ColonneEstAdmin.ToUpper());
                dictionaryParam.Add("RptParam_Title", Languages.Fonction.ToUpper());
                var w = new MessageBoxControl.MessageBoxChildWindow(Languages.Parametrage, Languages.QuestionImpressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                w.OnMessageBoxClosed += (_, result) =>
                {
                    if (w.Result == MessageBoxResult.OK)
                    {
                        string key = Utility.getKey();
                        var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                        service.EditerListeFonctionCompleted += (snder, print) =>
                        {
                            if (print.Cancelled || print.Error != null)
                            {
                                Message.Show(print.Error.Message, Languages.Parametrage);
                                return;
                            }
                            if (!print.Result)
                            {
                                Message.Show(Languages.ErreurImpressionDonnees, Languages.Parametrage);
                                return;
                            }
                            Utility.ActionImpressionDirect(null, key, "Fonction", "Parametrage");
                        };
                        service.EditerListeFonctionAsync(key, dictionaryParam);
                    }
                    else
                    {
                        return;
                    }
                };
                w.Show();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Parametrage);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        #region "Gestion MenuContextuel"

        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CwFonction form = new CwFonction(null, SessionObject.ExecMode.Creation, dtgrdParametre);
                form.Show();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Fonction);
            }
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CsFonction)dtgrdParametre.SelectedItem;
                    CwFonction form = new CwFonction(objetselectionne, SessionObject.ExecMode.Modification, dtgrdParametre);
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Fonction);
            }
        }

        private void Editer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Imprimer();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Fonction);
            }
        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    Supprimer();
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Fonction);
            }
        }

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CsFonction)dtgrdParametre.SelectedItem;
                    CwFonction form = new CwFonction(objetselectionne, SessionObject.ExecMode.Consultation, dtgrdParametre);
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Fonction);
            }
        }

        private void MenuContextuel_Opened(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuContextuelModifier.IsEnabled = MenuContextuelConsulter.IsEnabled =  MenuContextuelSupprimer.IsEnabled = dtgrdParametre.SelectedItems.Count > 0;
                MenuContextuelModifier.UpdateLayout();
                MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Fonction);
            }
        }

        #endregion

    }
}


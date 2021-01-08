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
using Galatee.Silverlight.Classes;
using Galatee.Silverlight.ServiceParametrage;
using Galatee.Silverlight.Resources.Parametrage;
namespace Galatee.Silverlight.Parametrage
{
    public partial class UcListRues : ChildWindow, INotifyPropertyChanged
    {
        public CsRues ObjetSelectionne { get; set; }

        ObservableCollection<CsRues> donnesDatagrid = new ObservableCollection<CsRues>();

        public UcListRues()
        {
            try
            {
                InitializeComponent();
                //Translate();
                GetData();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Rue);
            }
        }

        //private void Translate()
        //{
        //    try
        //    {
        //        dtgrdParametre.Columns[0].Header = Languages.Banque;
        //        dtgrdParametre.Columns[1].Header = Languages.Guichet;
        //        dtgrdParametre.Columns[2].Header = Languages.Libelle;
        //        dtgrdParametre.Columns[3].Header = Languages.Compte;
        //        Title = Languages.Banque;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
      
        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;
        
        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion

        public ObservableCollection<CsRues> DonnesDatagrid
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
            //int back = 0;
            try
            {
                //back = LoadingManager.BeginLoading("Veuillez patienter s'il vous plaît, chargement des données en cours...");
                LayoutRoot.Cursor = Cursors.Wait;
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllRuesCompleted += (ssender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            LayoutRoot.Cursor = Cursors.Arrow;
                            string error = args.Error.Message;
                            Message.Show(error,Languages.Parametrage);
                            //LoadingManager.EndLoading(back);
                            return;
                        }
                        if (args.Result == null)
                        {
                            LayoutRoot.Cursor = Cursors.Arrow;
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Rue);
                            //LoadingManager.EndLoading(back);
                            return;
                        }
                        DonnesDatagrid.Clear();
                        if (args.Result != null)
                            foreach (var item in args.Result)
                            {
                                DonnesDatagrid.Add(item);
                            }
                        DonnesDatagrid.OrderBy(p => p.PK_ID);
                        dtgrdParametre.ItemsSource = DonnesDatagrid;
                        //LoadingManager.EndLoading(back);
                        LayoutRoot.Cursor = Cursors.Arrow;
                    };
                client.SelectAllRuesAsync();
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                //LoadingManager.EndLoading(back);
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

                    var w = new MessageBoxControl.MessageBoxChildWindow(Languages.Rue, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                        {
                            var selected = dtgrdParametre.SelectedItem as CsRues;
                            if (selected != null)
                            {
                                ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                 Utility.EndPoint("Parametrage"));
                                delete.DeleteRuesCompleted += (del, argDel) =>
                                {
                                    if (argDel.Cancelled || argDel.Error != null)
                                    {
                                        Message.ShowError(argDel.Error.Message, Languages.Rue);
                                        return;
                                    }
                                    if (argDel.Result == false)
                                    {
                                        Message.ShowError(Languages.ErreurSuppressionDonnees, Languages.Rue);
                                        return;
                                    }
                                    DonnesDatagrid.Remove(selected);
                                };
                                delete.DeleteRuesAsync(selected);
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
                Message.ShowError(ex.Message, Languages.Rue);
            }
        }

        private void Imprimer()
        {
            var dictionaryParam = new Dictionary<string, string>();
            try
            {
                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeAImprimer);
                dictionaryParam.Add("Rpt_Code", Languages.ColonneCode.ToUpper());
                dictionaryParam.Add("RptParam_Libelle", Languages.ColonneLibelle.ToUpper());
                dictionaryParam.Add("RptParam_DateCreation", Languages.DateCreation);
                dictionaryParam.Add("RptParam_DateModification", Languages.DateModification);
                dictionaryParam.Add("RptParam_UserCreation", Languages.UserCreation);
                dictionaryParam.Add("RptParam_UserModification", Languages.UserModification);
                dictionaryParam.Add("Rpt_ParamCentre", Languages.Centre.ToUpper());
                dictionaryParam.Add("Rpt_Param_Commune", Languages.Commune.ToUpper());
                dictionaryParam.Add("RptParam_Title", Languages.ListeRues.ToUpper());
                var w = new MessageBoxControl.MessageBoxChildWindow(Languages.Rue, Languages.QuestionImpressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                w.OnMessageBoxClosed += (_, result) =>
                {
                    if (w.Result == MessageBoxResult.OK)
                    {
                        var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                        string key = Utility.getKey();
                        service.EditerListeRuesCompleted += (snder, print) =>
                        {
                            if (print.Cancelled || print.Error != null)
                            {
                                Message.ShowError(print.Error.Message, Languages.Rue);
                                return;
                            }
                            if (!print.Result)
                            {
                                Message.ShowError(Languages.ErreurImpressionDonnees, Languages.Rue);
                                return;
                            }
                            Utility.ActionImpressionDirect(SessionObject.CheminImpression, key, "Rues", "Parametrage");
                        };
                        service.EditerListeRuesAsync(key, dictionaryParam);
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
                Message.ShowError(ex.Message, Languages.Rue);
            }
        }
      
        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItems != null)
                {
                    MenuContextuel.IsEnabled = (this.dtgrdParametre.SelectedItems.Count == 1);
                }
                MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Rue);
            }
        }

        #region "Gestion MenuContextuel"

        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UcRues form = new UcRues(null, SessionObject.ExecMode.Creation, dtgrdParametre);
                form.Closed += form_Closed;
                form.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Rue);
            }
        }

        void form_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (UcRues)sender;
                if (form != null && form.DialogResult == true)
                {
                    GetData();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CsRues)dtgrdParametre.SelectedItem;
                    UcRues form = new UcRues(objetselectionne, SessionObject.ExecMode.Modification, dtgrdParametre);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Rue);
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
                Message.ShowError(ex.Message, Languages.Rue);
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
                Message.ShowError(ex.Message, Languages.Rue);
            }
        }

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CsRues)dtgrdParametre.SelectedItem;
                    UcRues form = new UcRues(objetselectionne, SessionObject.ExecMode.Consultation, dtgrdParametre);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Rue);
            }
        }

        private void MenuContextuel_Opened(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuContextuelModifier.IsEnabled = MenuContextuelConsulter.IsEnabled  = MenuContextuelSupprimer.IsEnabled = dtgrdParametre.SelectedItems.Count > 0;
                MenuContextuelModifier.UpdateLayout();
                MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Rue);
            }
        }

        #endregion
    }
}



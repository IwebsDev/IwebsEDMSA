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
    public partial class UcListRedevance : ChildWindow, INotifyPropertyChanged
    {
        public CsRedevance ObjetSelectionne { get; set; }

        ObservableCollection<CsRedevance> donnesDatagrid = new ObservableCollection<CsRedevance>();
        List<CsRedevance> ListeRedevance = null;
        public UcListRedevance()
        {
            try
            {
                InitializeComponent();
                //Translate();
                GetData();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Redevance);
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

        public ObservableCollection<CsRedevance> DonnesDatagrid
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
                ParametrageClient client = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                client.SelectAllRedevanceCompleted += (ssender, args) =>
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
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Redevance);
                            //LoadingManager.EndLoading(back);
                            return;
                        }
                        DonnesDatagrid.Clear();
                        if (args.Result != null)
                        {
                            foreach (var item in args.Result)
                            {
                                DonnesDatagrid.Add(item);
                            }
                            ListeRedevance = args.Result;
                        }
                        dtgrdParametre.ItemsSource = DonnesDatagrid;
                        //LoadingManager.EndLoading(back);
                        LayoutRoot.Cursor = Cursors.Arrow;
                    };
                client.SelectAllRedevanceAsync();
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

                    var w = new MessageBoxControl.MessageBoxChildWindow(Languages.Redevance, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                        {
                            var selected = dtgrdParametre.SelectedItem as CsRedevance;
                            if (selected != null)
                            {
                                ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                 Utility.EndPoint("Parametrage"));
                                delete.DeleteRedevanceCompleted += (del, argDel) =>
                                {
                                    if (argDel.Cancelled || argDel.Error != null)
                                    {
                                        Message.ShowError(argDel.Error.Message, Languages.Redevance);
                                        return;
                                    }
                                    if (argDel.Result == false)
                                    {
                                        Message.ShowError(Languages.ErreurSuppressionDonnees, Languages.Redevance);
                                        return;
                                    }
                                    DonnesDatagrid.Remove(selected);
                                };
                                delete.DeleteRedevanceAsync(selected);
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
                Message.ShowError(ex.Message, Languages.Redevance);
            }
        }

        private void Imprimer()
        {
            var dictionaryParam = new Dictionary<string, string>();
            try
            {
                if (ListeRedevance != null && ListeRedevance.Count == 0)
                    throw new Exception(Languages.AucuneDonneeAImprimer);
                dictionaryParam.Add("RptParam_Centre", Languages.Centre.ToUpper());
                dictionaryParam.Add("RptParam_Libelle", Languages.ColonneLibelle.ToUpper());
                dictionaryParam.Add("RptParam_DateCreation", Languages.DateCreation);
                dictionaryParam.Add("RptParam_DateModification", Languages.DateModification);
                dictionaryParam.Add("RptParam_UserCreation", Languages.UserCreation);
                dictionaryParam.Add("RptParam_UserModification", Languages.UserModification);
                dictionaryParam.Add("Rpt_Redevance", Languages.Redevance.ToUpper());
                dictionaryParam.Add("RptParam_Title", Languages.ListeRedevance.ToUpper());
                dictionaryParam.Add("Rpt_Tranche", Languages.ColonneTranche.ToUpper());
                dictionaryParam.Add("Rpt_Produit", Languages.Produit.ToUpper());
                dictionaryParam.Add("Rpt_NatureClient", Languages.NatureClient.ToUpper());
                var w = new MessageBoxControl.MessageBoxChildWindow(Languages.Redevance, Languages.QuestionImpressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                w.OnMessageBoxClosed += (_, result) =>
                {
                    if (w.Result == MessageBoxResult.OK)
                    {
                        var service = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                        string key = Utility.getKey();
                        service.EditerListeRedevanceCompleted += (snder, print) =>
                        {
                            if (print.Cancelled || print.Error != null)
                            {
                                Message.ShowError(print.Error.Message, Languages.Redevance);
                                return;
                            }
                            if (!print.Result)
                            {
                                Message.ShowError(Languages.ErreurImpressionDonnees, Languages.Redevance);
                                return;
                            }
                            Utility.ActionImpressionDirect(null, key, "Redevance", "Parametrage");
                        };
                        service.EditerListeRedevanceAsync(key, dictionaryParam, ListeRedevance);
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
                Message.ShowError(ex.Message, Languages.Redevance);
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
                Message.ShowError(ex.Message, Languages.Redevance);
            }
        }

        #region "Gestion MenuContextuel"

        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UcRedevance form = new UcRedevance(null, SessionObject.ExecMode.Creation, dtgrdParametre);
                form.Closed += form_Closed;
                form.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Redevance);
            }
        }

        void form_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (UcRedevance)sender;
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
                    var objetselectionne = (CsRedevance)dtgrdParametre.SelectedItem;
                    UcRedevance form = new UcRedevance(objetselectionne, SessionObject.ExecMode.Modification, dtgrdParametre);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Redevance);
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
                Message.ShowError(ex.Message, Languages.Redevance);
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
                Message.ShowError(ex.Message, Languages.Redevance);
            }
        }

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CsRedevance)dtgrdParametre.SelectedItem;
                    UcRedevance form = new UcRedevance(objetselectionne, SessionObject.ExecMode.Consultation, dtgrdParametre);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Redevance);
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
                Message.ShowError(ex.Message, Languages.Redevance);
            }
        }

        #endregion
    }
}



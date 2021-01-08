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
    public partial class UcListPeriodiciteFacturation : ChildWindow, INotifyPropertyChanged
    {
        public CsPeriodiciteFacturation ObjetSelectionne { get; set; }

        ObservableCollection<CsPeriodiciteFacturation> donnesDatagrid = new ObservableCollection<CsPeriodiciteFacturation>();
        string Namespace = "Galatee.Silverlight.Parametrage.";
        public UcListPeriodiciteFacturation()
        {
            try
            {
                InitializeComponent();
                Translate();
                GetData();
                this.DataContext = ObjetSelectionne;


                var ContextMenuItem = new List<ContextMenuItem>()
             {
                new ContextMenuItem(){ Code=Namespace+"UcPeriodiciteFacturation",Label=Galatee.Silverlight.Resources.Langue.ContextMenuCreate,ModeExcecution=SessionObject.ExecMode.Creation,Title =Galatee.Silverlight.Resources.Langue.Ajout+ " " + Galatee.Silverlight.Resources.Parametrage.Languages.ListePeriodiciteFacturation},
                new ContextMenuItem(){ Code=Namespace+"UcPeriodiciteFacturation",Label=Galatee.Silverlight.Resources.Langue.ContextMenuConsult,ModeExcecution=SessionObject.ExecMode.Consultation,Title =Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Parametrage.Languages.ListePeriodiciteFacturation },
                new ContextMenuItem(){ Code=Namespace+"UcPeriodiciteFacturation",Label=Galatee.Silverlight.Resources.Langue.ContextMenuModify,ModeExcecution=SessionObject.ExecMode.Modification,Title =Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Parametrage.Languages.ListePeriodiciteFacturation },
             };

                SessionObject.MenuContextuelItem = ContextMenuItem;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
            }
        }

        private void Translate()
        {
            try
            {
                dtgrdParametre.Columns[0].Header = Languages.Code;
                dtgrdParametre.Columns[1].Header = Languages.Libelle;
                Title = Languages.PeriodiciteFacturation;
                GroupBox.Header = Languages.ElementDansTable;
                btnDelete.Content = Languages.Supprimer;
                btnPrint.Content = Languages.Imprimer;
            }
            catch (Exception ex)
            {
                throw ex;
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
        public ObservableCollection<CsPeriodiciteFacturation> DonnesDatagrid
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
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllPeriodiciteFacturationCompleted += (ssender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError(error, Languages.PeriodiciteFacturation);
                            return;
                        }
                        if (args.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.PeriodiciteFacturation);
                            return;
                        }
                        else
                        {
                            DonnesDatagrid.Clear();
                            if (args.Result != null)
                                foreach (var item in args.Result)
                                {
                                    DonnesDatagrid.Add(item);
                                }
                            dtgrdParametre.ItemsSource = DonnesDatagrid;
                        }
                    };
                client.SelectAllPeriodiciteFacturationAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void Supprimer()
        //{
        //    try
        //    {
        //        if (DonnesDatagrid.Count == 0)
        //            throw new Exception(Languages.AucuneDonneeASupprimer);
        //        if (dtgrdParametre.SelectedItems.Count > 0)
        //        {
        //            var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.PeriodiciteFacturation, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
        //            messageBox.OnMessageBoxClosed += (_, result) =>
        //            {
        //                if (messageBox.Result == MessageBoxResult.OK)
        //                {
        //                    if (dtgrdParametre.SelectedItem != null)
        //                    {
        //                        var selected = dtgrdParametre.SelectedItem as CsPeriodiciteFacturation;

        //                        if (selected != null)
        //                        {
        //                            ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
        //                                                                             Utility.EndPoint("Parametrage"));
        //                            delete.DeletePeriodiciteFacturationCompleted += (del, argDel) =>
        //                            {
        //                                if (argDel.Cancelled || argDel.Error != null)
        //                                {
        //                                    Message.ShowError(argDel.Error.Message, Languages.PeriodiciteFacturation);
        //                                    return;
        //                                }
        //                                if (argDel.Result == false)
        //                                {
        //                                    Message.ShowError(argDel.Error.Message, Languages.PeriodiciteFacturation);
        //                                    return;
        //                                }
        //                                DonnesDatagrid.Remove(selected);
        //                            };
        //                            delete.DeletePeriodiciteFacturationAsync(selected);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    return;
        //                }
        //            };
        //            messageBox.Show();
        //        }
        //        else
        //        {
        //            throw new Exception(Languages.SelectionnerUnElement);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
        //    }
        //}

        //private void Imprimer()
        //{
        //    var dictionaryParam = new Dictionary<string, string>();
        //    try
        //    {
        //        dictionaryParam.Add("RptParam_Code", Languages.Code.ToUpper());
        //        dictionaryParam.Add("RptParam_Libelle", Languages.Libelle.ToUpper());
        //        dictionaryParam.Add("RptParam_DateCreation", Languages.DateCreation);
        //        dictionaryParam.Add("RptParam_DateModification", Languages.DateModification);
        //        dictionaryParam.Add("RptParam_UserCreation", Languages.UserCreation);
        //        dictionaryParam.Add("RptParam_UserModification", Languages.UserModification);
        //        dictionaryParam.Add("RptParam_Title", Languages.ListePeriodiciteFacturation.ToUpper());

        //        if (DonnesDatagrid.Count == 0)
        //            throw new Exception(Languages.AucuneDonneeAImprimer);
        //        var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.PeriodiciteFacturation, Languages.QuestionImpressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
        //        messageBox.OnMessageBoxClosed += (_, result) =>
        //        {
        //            if (messageBox.Result == MessageBoxResult.OK)
        //            {
        //                string key = Utility.getKey();
        //                var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
        //                service.EditerListePeriodiciteFacturationCompleted += (snder, print) =>
        //                {
        //                    if (print.Cancelled || print.Error != null)
        //                    {
        //                        Message.ShowError(print.Error.Message, Languages.PeriodiciteFacturation);
        //                        return;
        //                    }
        //                    if (!print.Result)
        //                    {
        //                        Message.ShowError(Languages.ErreurImpressionDonnees, Languages.PeriodiciteFacturation);
        //                        return;
        //                    }
        //                    Utility.ActionImpressionDirect(null, key, "PeriodiciteFacturation", "Parametrage");
        //                };
        //                service.EditerListePeriodiciteFacturationAsync(key, dictionaryParam);
        //            }
        //            else
        //            {
        //                return;
        //            }
        //        };
        //        messageBox.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
        //    }
        //}

        private void dtgrdParametre_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //aTa0 o = lvwResultat.SelectedItem as aTa0;
                string Ucname = string.Empty;
                //    if(int.Parse(o.CODE) > 0 && int.Parse(o.CODE)  < 1000)
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Banque);
            }
        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsPeriodiciteFacturation;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsPeriodiciteFacturation;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
            }
        }

        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsPeriodiciteFacturation;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsPeriodiciteFacturation;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
            }
        }

        #region "Gestion MenuContextuel"

        //private void Creer_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        UcPeriodiciteFacturation form = new UcPeriodiciteFacturation(null, SessionObject.ExecMode.Creation, dtgrdParametre);
        //        form.Closed += form_Closed;
        //        form.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
        //    }
        //}

        //void form_Closed(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var form = (UcPeriodiciteFacturation)sender;
        //        if (form != null && form.DialogResult == true)
        //        {
        //            GetData();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private void Modifier_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (dtgrdParametre.SelectedItem != null)
        //        {
        //            var objetselectionne = (CsPeriodiciteFacturation)dtgrdParametre.SelectedItem;
        //            UcPeriodiciteFacturation form = new UcPeriodiciteFacturation(objetselectionne, SessionObject.ExecMode.Modification, dtgrdParametre);
        //            form.Closed += form_Closed;
        //            form.Show();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
        //    }
        //}

        //private void Editer_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Imprimer();
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
        //    }
        //}

        //private void Supprimer_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (dtgrdParametre.SelectedItem != null)
        //        {
        //            Supprimer();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
        //    }
        //}

        //private void Consulter_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (dtgrdParametre.SelectedItem != null)
        //        {
        //            var objetselectionne = (CsPeriodiciteFacturation)dtgrdParametre.SelectedItem;
        //            UcPeriodiciteFacturation form = new UcPeriodiciteFacturation(objetselectionne, SessionObject.ExecMode.Consultation, dtgrdParametre);
        //            form.Closed += form_Closed;
        //            form.Show();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
        //    }
        //}

        //private void MenuContextuel_Opened(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        MenuContextuelModifier.IsEnabled = MenuContextuelConsulter.IsEnabled = MenuContextuelSupprimer.IsEnabled = dtgrdParametre.SelectedItems.Count > 0;
        //        MenuContextuelModifier.UpdateLayout();
        //        MenuContextuel.UpdateLayout();
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
        //    }
        //}

        #endregion

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeASupprimer);
                if (dtgrdParametre.SelectedItems.Count > 0)
                {
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.PeriodiciteFacturation, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            if (dtgrdParametre.SelectedItem != null)
                            {
                                var selected = dtgrdParametre.SelectedItem as CsPeriodiciteFacturation;

                                if (selected != null)
                                {
                                    ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                     Utility.EndPoint("Parametrage"));
                                    delete.DeletePeriodiciteFacturationCompleted += (del, argDel) =>
                                    {
                                        if (argDel.Cancelled || argDel.Error != null)
                                        {
                                            Message.ShowError(argDel.Error.Message, Languages.PeriodiciteFacturation);
                                            return;
                                        }
                                        if (argDel.Result == false)
                                        {
                                            Message.ShowError(argDel.Error.Message, Languages.PeriodiciteFacturation);
                                            return;
                                        }
                                        DonnesDatagrid.Remove(selected);
                                     
                                    };
                                    delete.DeletePeriodiciteFacturationAsync(selected);
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
                else
                {
                    throw new Exception(Languages.SelectionnerUnElement);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
            }
        }
        Dictionary<string, string> param = null;
        List<CsPeriodiciteFacturation> lstDonnee = new List<CsPeriodiciteFacturation>();
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            var dictionaryParam = new Dictionary<string, string>();
            try
            {
                dictionaryParam.Add("RptParam_Code", Languages.Code.ToUpper());
                dictionaryParam.Add("RptParam_Libelle", Languages.Libelle.ToUpper());
                dictionaryParam.Add("RptParam_DateCreation", Languages.DateCreation);
                dictionaryParam.Add("RptParam_DateModification", Languages.DateModification);
                dictionaryParam.Add("RptParam_UserCreation", Languages.UserCreation);
                dictionaryParam.Add("RptParam_UserModification", Languages.UserModification);
                dictionaryParam.Add("RptParam_Title", Languages.ListePeriodiciteFacturation.ToUpper());

                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeAImprimer);


                lstDonnee = DonnesDatagrid.ToList();
                Galatee.Silverlight.Shared.FrmOptionEditon ctrl = new Shared.FrmOptionEditon();
                ctrl.Closed += ctrl_Closed;
                this.IsEnabled = false;
                ctrl.Show();

            }
            catch (Exception ex)
            {

            }
        }

        void ctrl_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            Galatee.Silverlight.Shared.FrmOptionEditon ctrs = sender as Galatee.Silverlight.Shared.FrmOptionEditon;
            if (ctrs.IsOptionChoisi)
            {
                if (ctrs.OptionSelect == SessionObject.EnvoiPrinter)
                    Utility.ActionDirectOrientation<ServicePrintings.CsPeriodiciteFacturation, ServiceParametrage.CsPeriodiciteFacturation>(lstDonnee, param, SessionObject.CheminImpression, "PeriodiciteFacturation", "Parametrage", true);
                else if (ctrs.OptionSelect == SessionObject.EnvoiExecl)
                    Utility.ActionExportation<ServicePrintings.CsPeriodiciteFacturation, ServiceParametrage.CsPeriodiciteFacturation>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "PeriodiciteFacturation", "Parametrage", true, "xlsx");

                else if (ctrs.OptionSelect == SessionObject.EnvoiWord)
                    Utility.ActionExportation<ServicePrintings.CsPeriodiciteFacturation, ServiceParametrage.CsPeriodiciteFacturation>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "PeriodiciteFacturation", "Parametrage", true, "doc");

                else if (ctrs.OptionSelect == SessionObject.EnvoiPdf)
                    Utility.ActionExportation<ServicePrintings.CsPeriodiciteFacturation, ServiceParametrage.CsPeriodiciteFacturation>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "PeriodiciteFacturation", "Parametrage", true, "pdf");

            }
        }

        #region Sylla 11/06/2016
        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ParamLabel = Galatee.Silverlight.Resources.Langue.ContextMenuCreate;
                var ParamModeExcecution = SessionObject.ExecMode.Creation;
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Ajout + " " + Galatee.Silverlight.Resources.Parametrage.Languages.PeriodiciteFacturation;
                ShowWindows(ParamLabel, ParamModeExcecution, ParamTitle);
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
                var ParamLabel = Galatee.Silverlight.Resources.Langue.ContextMenuModify;
                var ParamModeExcecution = SessionObject.ExecMode.Modification;
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Parametrage.Languages.PeriodiciteFacturation;
                ShowWindows(ParamLabel, ParamModeExcecution, ParamTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowWindows(string ParamLabel, SessionObject.ExecMode ParamModeExcecution, string ParamTitle)
        {
            var contextMenuItem = new ContextMenuItem { Code = Namespace + "UcPeriodiciteFacturation", Label = ParamLabel, ModeExcecution = ParamModeExcecution, Title = ParamTitle };
            //SessionObject.MenuItemClicked = (MenuItem)sender;
            if (contextMenuItem != null && !string.IsNullOrEmpty(contextMenuItem.Code))
                new DataGridContexMenuBehavior().CreateUserView(contextMenuItem.Code, contextMenuItem.Title, contextMenuItem.ModeExcecution);
        }

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ParamLabel = Galatee.Silverlight.Resources.Langue.ContextMenuConsult;
                var ParamModeExcecution = SessionObject.ExecMode.Consultation;
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Parametrage.Languages.PeriodiciteFacturation;
                ShowWindows(ParamLabel, ParamModeExcecution, ParamTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}



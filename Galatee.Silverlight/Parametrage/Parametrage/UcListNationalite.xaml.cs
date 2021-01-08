using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Classes;
using Galatee.Silverlight.ServiceParametrage;
using Galatee.Silverlight.Resources.Parametrage;
namespace Galatee.Silverlight.Parametrage
{
    public partial class UcListNationalite : ChildWindow, INotifyPropertyChanged
    {
        public CsNationalite SelectedUser{get;set;}

        ObservableCollection<CsNationalite> donnesDatagrid = new ObservableCollection<CsNationalite>();
                string  Namespace = "Galatee.Silverlight.Parametrage.";
        public UcListNationalite()
        {
            try
            {
                InitializeComponent();
                Translate();
                GetData();

                this.DataContext = SelectedUser;


                var ContextMenuItem = new List<ContextMenuItem>()
             {
                new ContextMenuItem(){ Code=Namespace+"UcNationalite",Label=Galatee.Silverlight.Resources.Langue.ContextMenuCreate,ModeExcecution=SessionObject.ExecMode.Creation,Title =Galatee.Silverlight.Resources.Langue.Ajout+ " " + Galatee.Silverlight.Resources.Parametrage.Languages.LibelleNationalite },
                new ContextMenuItem(){ Code=Namespace+"UcNationalite",Label=Galatee.Silverlight.Resources.Langue.ContextMenuConsult,ModeExcecution=SessionObject.ExecMode.Consultation,Title =Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Parametrage.Languages.LibelleNationalite },
                new ContextMenuItem(){ Code=Namespace+"UcNationalite",Label=Galatee.Silverlight.Resources.Langue.ContextMenuModify,ModeExcecution=SessionObject.ExecMode.Modification,Title =Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Parametrage.Languages.LibelleNationalite },
             };

                SessionObject.MenuContextuelItem = ContextMenuItem;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.LibelleNationalite);
            }
        }
        private void Translate()
        {
            try
            {
                dtgrdParametre.Columns[0].Header = Languages.Code;
                dtgrdParametre.Columns[1].Header = Languages.Libelle;
                Title = Languages.LibelleNationalite;
                GroupBox.Header = Languages.ElementDansTable;
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

        public ObservableCollection<CsNationalite> DonnesDatagrid
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
                client.SelectAllNationaliteCompleted += (ssender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.Show(error, Languages.LibelleNationalite);
                            return;
                        }
                        if (args.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }
                        DonnesDatagrid.Clear();
                        if (args.Result != null)
                            foreach (var item in args.Result)
                            {
                                DonnesDatagrid.Add(item);
                            }
                        dtgrdParametre.ItemsSource = DonnesDatagrid;

                    };
                client.SelectAllNationaliteAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeASupprimer);
                if (dtgrdParametre.SelectedItems.Count > 0)
                {
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleNationalite, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            if (dtgrdParametre.SelectedItem != null)
                            {
                                var selected = dtgrdParametre.SelectedItem as CsNationalite;

                                if (selected != null)
                                {
                                    var delete = new ParametrageClient(Utility.Protocole(),
                                                                                     Utility.EndPoint("Parametrage"));
                                    delete.DeleteNationaliteCompleted += (del, argDel) =>
                                    {
                                        if (argDel.Cancelled || argDel.Error != null)
                                        {
                                            Message.Show(argDel.Error.Message, Languages.LibelleNationalite);
                                            return;
                                        }
                                        if (argDel.Result == false)
                                        {
                                            Message.Show(Languages.ErreurSuppressionDonnees, Languages.LibelleNationalite);
                                            return;
                                        }
                                        DonnesDatagrid.Remove(selected);
                                    };
                                    delete.DeleteNationaliteAsync(selected);
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
                Message.Show(ex.Message, Languages.LibelleNationalite);
            }
        }
        Dictionary<string, string> param = null;
        List<CsNationalite> lstDonnee = new List<CsNationalite>();
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            param = new Dictionary<string, string>();
            try
            {
                if(DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeAImprimer);
                lstDonnee.Clear();
                param.Add("RptParam_Code", Languages.Code.ToUpper());
                param.Add("RptParam_Libelle", Languages.Libelle.ToUpper());
                param.Add("RptParam_DateCreation", Languages.DateCreation);
                param.Add("RptParam_DateModification", Languages.DateModification);
                param.Add("RptParam_UserCreation", Languages.UserCreation);
                param.Add("RptParam_UserModification", Languages.UserModification);
                param.Add("RptParam_Title", Languages.ListeNationalite.ToUpper());
                foreach (CsNationalite  item in DonnesDatagrid )
                    lstDonnee.Add(item); 
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
                    Utility.ActionDirectOrientation<ServicePrintings.CsNationalite, ServiceParametrage.CsNationalite>(lstDonnee, param, SessionObject.CheminImpression, "Nationalite", "Parametrage", true);
                else if (ctrs.OptionSelect == SessionObject.EnvoiExecl)
                    Utility.ActionExportation<ServicePrintings.CsNationalite, ServiceParametrage.CsNationalite>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "Nationalite", "Parametrage", true, "xlsx");

                else if (ctrs.OptionSelect == SessionObject.EnvoiWord)
                    Utility.ActionExportation<ServicePrintings.CsNationalite, ServiceParametrage.CsNationalite>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "Nationalite", "Parametrage", true, "doc");

                else if (ctrs.OptionSelect == SessionObject.EnvoiPdf)
                    Utility.ActionExportation<ServicePrintings.CsNationalite, ServiceParametrage.CsNationalite>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "Nationalite", "Parametrage", true, "pdf");

            }
        }

        
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
                Message.Show(ex.Message, Languages.LibelleNationalite);
            }

        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelectedUser = dtgrdParametre.SelectedItem as CsNationalite;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsNationalite;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.LibelleNationalite);
            }
        }
        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SelectedUser = dtgrdParametre.SelectedItem as CsNationalite;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsNationalite;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.LibelleNationalite);
            }
        }

        #region Sylla 11/06/2016
        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ParamLabel = Galatee.Silverlight.Resources.Langue.ContextMenuCreate;
                var ParamModeExcecution = SessionObject.ExecMode.Creation;
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Ajout + " " + Galatee.Silverlight.Resources.Parametrage.Languages.LibelleNationalite;
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
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Parametrage.Languages.LibelleNationalite;
                ShowWindows(ParamLabel, ParamModeExcecution, ParamTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowWindows(string ParamLabel, SessionObject.ExecMode ParamModeExcecution, string ParamTitle)
        {
            var contextMenuItem = new ContextMenuItem { Code = Namespace + "UcNationalite", Label = ParamLabel, ModeExcecution = ParamModeExcecution, Title = ParamTitle };
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
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Parametrage.Languages.LibelleNationalite;
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



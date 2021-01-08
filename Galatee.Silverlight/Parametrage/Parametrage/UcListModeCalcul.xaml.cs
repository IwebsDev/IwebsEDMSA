﻿using System;
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
    public partial class UcListModeCalcul : ChildWindow, INotifyPropertyChanged
    {
         public CsModeCalcul ObjetSelectionne { get; set; }

        ObservableCollection<CsModeCalcul> donnesDatagrid = new ObservableCollection<CsModeCalcul>();

        public UcListModeCalcul()
        {
            try
            {
                InitializeComponent();
                Translate();
                GetData();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.ModeCalcul);
            }
        }

        private void Translate()
        {
            try
            {
                dtgrdParametre.Columns[0].Header = Languages.Code;
                dtgrdParametre.Columns[1].Header = Languages.Libelle;
                Title = Languages.ModeCalcul;
                GroupBox.Header = Languages.ElementDansTable;
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

        public ObservableCollection<CsModeCalcul> DonnesDatagrid
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
                client.SelectAllModeCalculCompleted += (ssender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError(error, Languages.ModeCalcul);
                            return;
                        }
                        if (args.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.ModeCalcul);
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
                client.SelectAllModeCalculAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Supprimer()
        {
            try
            {
                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeASupprimer);
                if (dtgrdParametre.SelectedItems.Count > 0)
                {
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.ModeCalcul, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            if (dtgrdParametre.SelectedItem != null)
                            {
                                var selected = dtgrdParametre.SelectedItem as CsModeCalcul;

                                if (selected != null)
                                {
                                    ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                     Utility.EndPoint("Parametrage"));
                                    delete.DeleteModeCalculCompleted += (del, argDel) =>
                                    {
                                        if (argDel.Cancelled || argDel.Error != null)
                                        {
                                            Message.ShowError(argDel.Error.Message, Languages.ModeCalcul);
                                            return;
                                        }
                                        if (argDel.Result == false)
                                        {
                                            Message.ShowError(argDel.Error.Message, Languages.ModeCalcul);
                                            return;
                                        }
                                        DonnesDatagrid.Remove(selected);
                                    };
                                    delete.DeleteModeCalculAsync(selected);
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
                Message.ShowError(ex.Message, Languages.ModeCalcul);
            }
        }

        private void Imprimer()
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
                dictionaryParam.Add("RptParam_Title", Languages.ListeModeCalcul.ToUpper());

                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeAImprimer);
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.ModeCalcul, Languages.QuestionImpressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        string key = Utility.getKey();
                        var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                        service.EditerListeModeCalculCompleted += (snder, print) =>
                        {
                            if (print.Cancelled || print.Error != null)
                            {
                                Message.ShowError(print.Error.Message, Languages.ModeCalcul);
                                return;
                            }
                            if (!print.Result)
                            {
                                Message.ShowError(Languages.ErreurImpressionDonnees, Languages.ModeCalcul);
                                return;
                            }
                            Utility.ActionImpressionDirect(SessionObject.CheminImpression, key, "ModeCalcul", "Parametrage");
                        };
                        service.EditerListeModeCalculAsync(key, dictionaryParam);
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
                Message.ShowError(ex.Message, Languages.ModeCalcul);
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
                Message.ShowError(ex.Message, Languages.ModeCalcul);
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
                Message.ShowError(ex.Message, Languages.EtapeDevis);
            }
        }

        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsModeCalcul;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsModeCalcul;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.ModeCalcul);
            }
        }

        #region "Gestion MenuContextuel"

        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UcModeCalcul form = new UcModeCalcul(null, SessionObject.ExecMode.Creation, dtgrdParametre);
                form.Closed += form_Closed;
                form.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.ModeCalcul);
            }
        }

        void form_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (UcModeCalcul)sender;
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
                    var objetselectionne = (CsModeCalcul)dtgrdParametre.SelectedItem;
                    UcModeCalcul form = new UcModeCalcul(objetselectionne, SessionObject.ExecMode.Modification, dtgrdParametre);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.ModeCalcul);
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
                Message.ShowError(ex.Message, Languages.ModeCalcul);
            }
        }

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CsModeCalcul)dtgrdParametre.SelectedItem;
                    UcModeCalcul form = new UcModeCalcul(objetselectionne, SessionObject.ExecMode.Consultation, dtgrdParametre);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.ModeCalcul);
            }
        }

        private void MenuContextuel_Opened(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuContextuelModifier.IsEnabled = MenuContextuelConsulter.IsEnabled = MenuContextuelSupprimer.IsEnabled = dtgrdParametre.SelectedItems.Count > 0;
                MenuContextuelModifier.UpdateLayout();
                MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.ModeCalcul);
            }
        }

        #endregion
    }
}



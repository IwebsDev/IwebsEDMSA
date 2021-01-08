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
    public partial class UcListUniteComptage : ChildWindow, INotifyPropertyChanged
    {
         public CsUniteComptage ObjetSelectionne { get; set; }

        ObservableCollection<CsUniteComptage> donnesDatagrid = new ObservableCollection<CsUniteComptage>();

        public UcListUniteComptage()
        {
            try
            {
                InitializeComponent();
                Translate();
                GetData();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.UniteComptage);
            }
        }

        private void Translate()
        {
            try
            {
                dtgrdParametre.Columns[0].Header = Languages.Code;
                dtgrdParametre.Columns[1].Header = Languages.Libelle;
                Title = Languages.UniteComptage;
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

        public ObservableCollection<CsUniteComptage> DonnesDatagrid
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
                client.SelectAllUniteComptageCompleted += (ssender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError(error, Languages.UniteComptage);
                            return;
                        }
                        if (args.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.UniteComptage);
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
                client.SelectAllUniteComptageAsync();
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
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.UniteComptage, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            if (dtgrdParametre.SelectedItem != null)
                            {
                                var selected = dtgrdParametre.SelectedItem as CsUniteComptage;

                                if (selected != null)
                                {
                                    ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                     Utility.EndPoint("Parametrage"));
                                    delete.DeleteUniteComptageCompleted += (del, argDel) =>
                                    {
                                        if (argDel.Cancelled || argDel.Error != null)
                                        {
                                            Message.ShowError(argDel.Error.Message, Languages.UniteComptage);
                                            return;
                                        }
                                        if (argDel.Result == false)
                                        {
                                            Message.ShowError(argDel.Error.Message, Languages.UniteComptage);
                                            return;
                                        }
                                        DonnesDatagrid.Remove(selected);
                                    };
                                    delete.DeleteUniteComptageAsync(selected);
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
                Message.ShowError(ex.Message, Languages.UniteComptage);
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
                dictionaryParam.Add("RptParam_Title", Languages.ListeUniteComptage.ToUpper());

                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeAImprimer);
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.UniteComptage, Languages.QuestionImpressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        string key = Utility.getKey();
                        var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                        service.EditerListeUniteComptageCompleted += (snder, print) =>
                        {
                            if (print.Cancelled || print.Error != null)
                            {
                                Message.ShowError(print.Error.Message, Languages.UniteComptage);
                                return;
                            }
                            if (!print.Result)
                            {
                                Message.ShowError(Languages.ErreurImpressionDonnees, Languages.UniteComptage);
                                return;
                            }
                            Utility.ActionImpressionDirect(SessionObject.CheminImpression, key, "UniteComptage", "Parametrage");
                        };
                        service.EditerListeUniteComptageAsync(key, dictionaryParam);
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
                Message.ShowError(ex.Message, Languages.UniteComptage);
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
                Message.ShowError(ex.Message, Languages.UniteComptage);
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
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsUniteComptage;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsUniteComptage;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.UniteComptage);
            }
        }

        #region "Gestion MenuContextuel"

        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UcUniteComptage form = new UcUniteComptage(null, SessionObject.ExecMode.Creation, dtgrdParametre);
                form.Closed += form_Closed;
                form.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.UniteComptage);
            }
        }

        void form_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (UcUniteComptage)sender;
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
                    var objetselectionne = (CsUniteComptage)dtgrdParametre.SelectedItem;
                    UcUniteComptage form = new UcUniteComptage(objetselectionne, SessionObject.ExecMode.Modification, dtgrdParametre);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.UniteComptage);
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
                Message.ShowError(ex.Message, Languages.UniteComptage);
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
                Message.ShowError(ex.Message, Languages.UniteComptage);
            }
        }

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CsUniteComptage)dtgrdParametre.SelectedItem;
                    UcUniteComptage form = new UcUniteComptage(objetselectionne, SessionObject.ExecMode.Consultation, dtgrdParametre);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.UniteComptage);
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
                Message.ShowError(ex.Message, Languages.UniteComptage);
            }
        }

        #endregion
    }
}



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
    public partial class CwListeCasReleveIndex : ChildWindow
    {
        public CsCasind ObjetSelectionne { get; set; }

        ObservableCollection<CsCasind> donnesDatagrid = new ObservableCollection<CsCasind>();

        public CwListeCasReleveIndex()
        {
            try
            {
                InitializeComponent();
                GetData();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Fonction);
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

        public ObservableCollection<CsCasind> DonnesDatagrid
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

        private string RetournerLibelleElementSaisie(string pCode)
        {
            try
            {
                switch (pCode)
                {
                    case "F":
                        return Languages.LibFacultatif;
                    case "I":
                        return Languages.LibInterdit;
                    case "O":
                        return Languages.LibObligatoire;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetData()
        {
            int back = 0;
            try
            {
                back = LoadingManager.BeginLoading("Chargement des données en cours...");
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.GetAllCasindCompleted += (ssender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            LoadingManager.EndLoading(back);
                            Message.ShowError(error, Languages.CasDeReleve);
                            return;
                        }
                        if (args.Result == null)
                        {
                            LoadingManager.EndLoading(back);
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.CasDeReleve);
                            return;
                        }
                        DonnesDatagrid.Clear();
                        if (args.Result != null)
                            foreach (var item in args.Result)
                            {
                                
                                item.SAISIEINDEX = RetournerLibelleElementSaisie(item.SAISIEINDEX);
                                item.SAISIECOMPTEUR = RetournerLibelleElementSaisie(item.SAISIECOMPTEUR);
                                item.SAISIECONSO = RetournerLibelleElementSaisie(item.SAISIECONSO);
                                DonnesDatagrid.Add(item);
                            }
                        dtgrdParametre.ItemsSource = DonnesDatagrid;
                        LoadingManager.EndLoading(back);
                    }
                    catch (Exception ex)
                    {
                        LoadingManager.EndLoading(back);
                        Message.ShowError(ex.Message, Languages.CasDeReleve);
                    }
                };
                client.GetAllCasindAsync();
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
                            var selected = dtgrdParametre.SelectedItem as CsCasind;
                            if (selected != null)
                            {
                                List<CsCasind> ListForDelete = new List<CsCasind>();
                                ListForDelete.Add(selected);
                                ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                 Utility.EndPoint("Parametrage"));
                                delete.DeleteCasindCompleted += (del, argDel) =>
                                {
                                    if (argDel.Cancelled || argDel.Error != null)
                                    {
                                        Message.ShowError(argDel.Error.Message, Languages.Parametrage);
                                        return;
                                    }
                                    if (argDel.Result == false)
                                    {
                                        Message.ShowError(Languages.ErreurSuppressionDonnees, Languages.Parametrage);
                                        return;
                                    }
                                    DonnesDatagrid.Remove(selected);
                                };
                                delete.DeleteCasindAsync(ListForDelete);
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
                Message.ShowError(ex.Message, Languages.Parametrage);
            }
        }
        Dictionary<string, string> param = null;
        List<CsCasind> lstDonnee = new List<CsCasind>();
        private void Imprimer(List<CsCasind> lstCas)
        {
            try
            {
                param = new Dictionary<string, string>();

                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeAImprimer);
                param.Add("Rpt_Cas", Languages.ColonneCode.ToUpper());
                param.Add("RptParam_DateCreation", Languages.DateCreation);
                param.Add("RptParam_DateModification", Languages.DateModification);
                param.Add("RptParam_UserCreation", Languages.UserCreation);
                param.Add("RptParam_UserModification", Languages.UserModification);
                param.Add("RptParam_Libelle", Languages.ColonneLibelle.ToUpper());
                param.Add("Rpt_Param_Enquete", Languages.ColonneEnquete.ToUpper());
                param.Add("RptParam_Centre", Languages.ColonneCentre.ToUpper());
                param.Add("RptParam_Title", Languages.ListeCasReleve.ToUpper());
                param.Add("Rpt_Param_Facture", Languages.ColonneFacture.ToUpper());
                param.Add("Rpt_Param_LibFac", Languages.ColonneLibFac.ToUpper());
                param.Add("Rpt_Param_LibCourt", Languages.ColonneLibcourt.ToUpper());

                lstDonnee = lstCas;
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
                    Utility.ActionDirectOrientation<ServicePrintings.CsCasind, ServiceParametrage.CsCasind>(lstDonnee, param, SessionObject.CheminImpression, "Casind", "Parametrage", true);
                else if (ctrs.OptionSelect == SessionObject.EnvoiExecl)
                    Utility.ActionExportation<ServicePrintings.CsCasind, ServiceParametrage.CsCasind>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "Casind", "Parametrage", true, "xlsx");

                else if (ctrs.OptionSelect == SessionObject.EnvoiWord)
                    Utility.ActionExportation<ServicePrintings.CsCasind, ServiceParametrage.CsCasind>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "Casind", "Parametrage", true, "doc");

                else if (ctrs.OptionSelect == SessionObject.EnvoiPdf)
                    Utility.ActionExportation<ServicePrintings.CsCasind, ServiceParametrage.CsCasind>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "Casind", "Parametrage", true, "pdf");

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
                CwCasReleveIndex form = new CwCasReleveIndex(null, SessionObject.ExecMode.Creation, dtgrdParametre);
                form.Closed += form_Closed;
                form.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Fonction);
            }
        }

        void form_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (CwCasReleveIndex)sender;
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
                    var objetselectionne = (CsCasind)dtgrdParametre.SelectedItem;
                    CwCasReleveIndex form = new CwCasReleveIndex(objetselectionne, SessionObject.ExecMode.Modification, dtgrdParametre);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Fonction);
            }
        }

        private void Editer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.dtgrdParametre.ItemsSource != null)
                {
                    List<CsCasind> lstCas = ((ObservableCollection <CsCasind>)this.dtgrdParametre.ItemsSource).ToList();
                    Imprimer(lstCas);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Fonction);
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
                Message.ShowError(ex.Message, Languages.Fonction);
            }
        }

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CsCasind)dtgrdParametre.SelectedItem;
                    CwCasReleveIndex form = new CwCasReleveIndex(objetselectionne, SessionObject.ExecMode.Consultation, dtgrdParametre);
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Fonction);
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
                Message.ShowError(ex.Message, Languages.Fonction);
            }
        }

        #endregion
    }
}


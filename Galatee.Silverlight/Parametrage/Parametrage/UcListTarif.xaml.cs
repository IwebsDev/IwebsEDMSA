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
    public partial class UcListTarif : ChildWindow, INotifyPropertyChanged
    {
        public CsTarif ObjetSelectionne { get; set; }

        ObservableCollection<CsTarif> donnesDatagrid = new ObservableCollection<CsTarif>();
        List<CsTarif> ListeDesTarifs = null;
        public UcListTarif()
        {
            try
            {
                InitializeComponent();
                //Translate();
                GetData();
                GetProduit();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Tarif);
            }
        }
        List<CsProduit> lstProduit = new List<CsProduit>();
        private void GetProduit()
        {
            try
            {
                lstProduit.Clear();
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllProduitCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.LibelleProduit);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    CsProduit leProduit = new CsProduit()
                    {
                     PK_ID = 0,
                     LIBELLE = "AUCUN" 
                    };
                    lstProduit.Add(leProduit);
                    lstProduit.AddRange(args.Result);

                    cbo_produit.ItemsSource = null;
                    cbo_produit.ItemsSource = lstProduit;
                    cbo_produit.DisplayMemberPath = "LIBELLE";
                };
                client.SelectAllProduitAsync();
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

        public ObservableCollection<CsTarif> DonnesDatagrid
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
                client.SelectAllTarifCompleted += (ssender, args) =>
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
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Tarif);
                            //LoadingManager.EndLoading(back);
                            return;
                        }
                        DonnesDatagrid.Clear();
                        if (args.Result != null)
                        {
                            ListeDesTarifs = args.Result;
                            foreach (var item in args.Result)
                            {
                                DonnesDatagrid.Add(item);
                            }
                        }
                        dtgrdParametre.ItemsSource = DonnesDatagrid;
                        //LoadingManager.EndLoading(back);
                        LayoutRoot.Cursor = Cursors.Arrow;
                    };
                client.SelectAllTarifAsync();
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

                    var w = new MessageBoxControl.MessageBoxChildWindow(Languages.Tarif, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                        {
                            var selected = dtgrdParametre.SelectedItem as CsTarif;
                            if (selected != null)
                            {
                                ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                 Utility.EndPoint("Parametrage"));
                                delete.DeleteTarifCompleted += (del, argDel) =>
                                {
                                    if (argDel.Cancelled || argDel.Error != null)
                                    {
                                        Message.ShowError(argDel.Error.Message, Languages.Tarif);
                                        return;
                                    }
                                    if (argDel.Result == false)
                                    {
                                        Message.ShowError(Languages.ErreurSuppressionDonnees, Languages.Tarif);
                                        return;
                                    }
                                    DonnesDatagrid.Remove(selected);
                                };
                                delete.DeleteTarifAsync(selected);
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
                Message.ShowError(ex.Message, Languages.Tarif);
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
                Message.ShowError(ex.Message, Languages.Tarif);
            }
        }

        #region "Gestion MenuContextuel"

        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UcTarif form = new UcTarif(null, SessionObject.ExecMode.Creation, dtgrdParametre);
                form.Closed += form_Closed;
                form.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Tarif);
            }
        }

        void form_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (UcTarif)sender;
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
                    var objetselectionne = (CsTarif)dtgrdParametre.SelectedItem;
                    UcTarif form = new UcTarif(objetselectionne, SessionObject.ExecMode.Modification, dtgrdParametre);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Tarif);
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
                Message.ShowError(ex.Message, Languages.Tarif);
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
                Message.ShowError(ex.Message, Languages.Tarif);
            }
        }

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CsTarif)dtgrdParametre.SelectedItem;
                    UcTarif form = new UcTarif(objetselectionne, SessionObject.ExecMode.Consultation, dtgrdParametre);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Tarif);
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
                Message.ShowError(ex.Message, Languages.Tarif);
            }
        }

        #endregion

        private void cbo_produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.cbo_produit.SelectedItem != null)
                {
                    CsProduit leProduitSelect = (CsProduit)cbo_produit.SelectedItem;
                    ObservableCollection<CsTarif> lstTarif = new ObservableCollection<CsTarif>();
                    var lstTarifProduit = new List<CsTarif>();

                        if (leProduitSelect.PK_ID != 0)
                            lstTarifProduit = DonnesDatagrid.Where(t => t.FK_IDPRODUIT == ((CsProduit)this.cbo_produit.SelectedItem).PK_ID).ToList();
                        else
                            lstTarifProduit = DonnesDatagrid.ToList();

                        foreach (var item in lstTarifProduit)
                            lstTarif.Add(item);
                        dtgrdParametre.ItemsSource = null;
                        dtgrdParametre.ItemsSource = lstTarif;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Select Produit");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }



        Dictionary<string, string> param = null;
        List<CsTarif> lstDonnee = new List<CsTarif>();
        private void Imprimer()
        {
            if (dtgrdParametre.ItemsSource != null)
            {
                param = new Dictionary<string, string>();
                param.Add("Rpt_Code", Languages.ColonneTarif.ToUpper());
                param.Add("RptParam_Libelle", Languages.ColonneLibelle.ToUpper());
                param.Add("RptParam_DateCreation", Languages.DateCreation);
                param.Add("RptParam_DateModification", Languages.DateModification);
                param.Add("RptParam_UserCreation", Languages.UserCreation);
                param.Add("RptParam_UserModification", Languages.UserModification);
                param.Add("Rpt_ParamCentre", Languages.Centre.ToUpper());
                param.Add("Rpt_Param_Produit", Languages.Produit.ToUpper());
                param.Add("RptParam_Title", Languages.ListeTarif.ToUpper());

                lstDonnee = ((ObservableCollection<CsTarif>)dtgrdParametre.ItemsSource).ToList();
                Galatee.Silverlight.Shared.FrmOptionEditon ctrl = new Shared.FrmOptionEditon();
                ctrl.Closed += ctrl_Closed;
                this.IsEnabled = false;
                ctrl.Show();

            }
        }

        void ctrl_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            Galatee.Silverlight.Shared.FrmOptionEditon ctrs = sender as Galatee.Silverlight.Shared.FrmOptionEditon;
            if (ctrs.IsOptionChoisi)
            {
                if (ctrs.OptionSelect == SessionObject.EnvoiPrinter)
                    Utility.ActionDirectOrientation<ServicePrintings.CsTarif, ServiceParametrage.CsTarif>(lstDonnee, param, SessionObject.CheminImpression, "Tarif", "Parametrage", true);
                else if (ctrs.OptionSelect == SessionObject.EnvoiExecl)
                    Utility.ActionExportation<ServicePrintings.CsTarif, ServiceParametrage.CsTarif>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "Tarif", "Parametrage", true, "xlsx");

                else if (ctrs.OptionSelect == SessionObject.EnvoiWord)
                    Utility.ActionExportation<ServicePrintings.CsTarif, ServiceParametrage.CsTarif>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "Tarif", "Parametrage", true, "doc");

                else if (ctrs.OptionSelect == SessionObject.EnvoiPdf)
                    Utility.ActionExportation<ServicePrintings.CsTarif, ServiceParametrage.CsTarif>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "Tarif", "Parametrage", true, "pdf");

            }
        }

       

    }
}



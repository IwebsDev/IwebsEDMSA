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
    public partial class UcListCentre : ChildWindow, INotifyPropertyChanged
    {
        public CsCentre ObjetSelectionne { get; set; }

        ObservableCollection<CsCentre> donnesDatagrid = new ObservableCollection<CsCentre>();
        string Namespace = "Galatee.Silverlight.Parametrage.";

        public UcListCentre()
        {
            try
            {
                InitializeComponent();
                Translate();
                GetData();
                this.DataContext = ObjetSelectionne;

                var ContextMenuItem = new List<ContextMenuItem>()
             {
                new ContextMenuItem(){ Code=Namespace+"UcCentre",Label=Galatee.Silverlight.Resources.Langue.ContextMenuCreate,ModeExcecution=SessionObject.ExecMode.Creation,Title =Galatee.Silverlight.Resources.Langue.Ajout+ " " + Galatee.Silverlight.Resources.Parametrage.Languages.Centre },
                new ContextMenuItem(){ Code=Namespace+"UcCentre",Label=Galatee.Silverlight.Resources.Langue.ContextMenuConsult,ModeExcecution=SessionObject.ExecMode.Consultation,Title =Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Parametrage.Languages.Centre },
                new ContextMenuItem(){ Code=Namespace+"UcCentre",Label=Galatee.Silverlight.Resources.Langue.ContextMenuModify,ModeExcecution=SessionObject.ExecMode.Modification,Title =Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Parametrage.Languages.Centre },
             };

                SessionObject.MenuContextuelItem = ContextMenuItem;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Centre);
            }
        }

        private void Translate()
        {
            try
            {
                dtgrdParametre.Columns[0].Header = Languages.Centre;
                dtgrdParametre.Columns[1].Header = Languages.Libelle;
                dtgrdParametre.Columns[2].Header = Languages.Site;
                dtgrdParametre.Columns[3].Header = Languages.LibelleTypeCentre;
                Title = Languages.Centre;
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

        public ObservableCollection<CsCentre> DonnesDatagrid
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
        List<CsCentre> lstCentre = new List<CsCentre>();
        private void GetData()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllCentreCompleted+= (ssender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError(error, Languages.Centre);
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
                        lstCentre = args.Result;
                        dtgrdParametre.ItemsSource = DonnesDatagrid;
                        Cbo_Site.DisplayMemberPath = "LIBELLESITE";
                        Cbo_Site.ItemsSource = RetourneSiteFromCentre(args.Result);
                    };
                client.SelectAllCentreAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<CsCentre> RetourneSiteFromCentre(List<CsCentre> ListeDesCentre)
        {
            try
            {
                List<CsCentre> lstCentre = new List<CsCentre>();
                if (ListeDesCentre.Count > 0)
                {
                    var lstClientFactureDistnct = ListeDesCentre.Select(t => new { t.CODESITE , t.FK_IDCODESITE  ,t.LIBELLESITE    }).Distinct().ToList();
                    foreach (var item in lstClientFactureDistnct)
                        lstCentre.Add(new CsCentre { CODESITE = item.CODESITE, FK_IDCODESITE = item.FK_IDCODESITE, LIBELLESITE = item.LIBELLESITE });
                }
                return lstCentre;
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
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Centre, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            if (dtgrdParametre.SelectedItem != null)
                            {
                                var selected = dtgrdParametre.SelectedItem as CsCentre;

                                if (selected != null)
                                {
                                    ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                     Utility.EndPoint("Parametrage"));
                                    delete.DeleteCentreCompleted += (del, argDel) =>
                                                                        {
                                                                            if (argDel.Cancelled || argDel.Error != null)
                                                                            {
                                                                               Message.ShowError(argDel.Error.Message, Languages.Centre);
                                                                               return;
                                                                            }

                                                                            if (argDel.Result == false)
                                                                            {
                                                                                Message.ShowError(Languages.ErreurSuppressionDonnees, Languages.Centre);
                                                                                return;
                                                                            }

                                                                            DonnesDatagrid.Remove(selected);
                                                                        };
                                    delete.DeleteCentreAsync(selected);
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
                Message.ShowError(ex.Message, Languages.Centre);
            }
        }

        private void DialogResultPrint(object sender, EventArgs e)
        {
            var dictionaryParam = new Dictionary<string, string>();
            try
            {
                dictionaryParam.Add("RptParam_Libelle",Languages.Libelle.ToUpper());
                dictionaryParam.Add("RptParam_DateCreation",Languages.DateCreation);
                dictionaryParam.Add("RptParam_DateModification",Languages.DateModification);
                dictionaryParam.Add("RptParam_UserCreation",Languages.UserCreation);
                dictionaryParam.Add("RptParam_UserModification", Languages.UserModification);
                dictionaryParam.Add("Rpt_Centre", Languages.Centre.ToUpper());
                dictionaryParam.Add("Rpt_Site", Languages.Site.ToUpper());
                dictionaryParam.Add("Rpt_TypeCentre", Languages.LibelleTypeCentre.ToUpper());
                dictionaryParam.Add("RptParam_Title", Languages.ListeCentre.ToUpper());
                var ctrs = sender as DialogResult;
                if (ctrs != null && ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
                {
                    var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                    string key = Utility.getKey();
                    service.EditerListeCentreCompleted += (snder, print) =>
                    {
                        if (print.Cancelled || print.Error != null)
                        {
                            Message.ShowError(print.Error.Message, Languages.Centre);
                            return;
                        }
                        if (!print.Result)
                        {
                            Message.ShowError(Languages.ErreurImpressionDonnees, Languages.Centre);
                            return;
                        }
                        Utility.ActionImpressionDirect(SessionObject.CheminImpression , key, "Centre", "Parametrage");
                    };
                    service.EditerListeCentreAsync(key,dictionaryParam);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Centre);
            }
        }
        Dictionary<string, string> param = null;
        List<CsCentre> lstDonnee = new List<CsCentre>();
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            param = new Dictionary<string, string>();
            try
            {
                param.Add("RptParam_Libelle", Languages.Libelle.ToUpper());
                param.Add("RptParam_DateCreation", Languages.DateCreation);
                param.Add("RptParam_DateModification", Languages.DateModification);
                param.Add("RptParam_UserCreation", Languages.UserCreation);
                param.Add("RptParam_UserModification", Languages.UserModification);
                param.Add("Rpt_Centre", Languages.Centre.ToUpper());
                param.Add("Rpt_Site", Languages.Site.ToUpper());
                param.Add("Rpt_TypeCentre", Languages.LibelleTypeCentre.ToUpper());
                param.Add("RptParam_Title", Languages.ListeCentre.ToUpper());
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
                    Utility.ActionDirectOrientation<ServicePrintings.CsCentre, ServiceParametrage.CsCentre>(lstDonnee, param, SessionObject.CheminImpression, "Centre", "Parametrage", true);
                else if (ctrs.OptionSelect == SessionObject.EnvoiExecl)
                    Utility.ActionExportation<ServicePrintings.CsCentre, ServiceParametrage.CsCentre>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "Centre", "Parametrage", true, "xlsx");

                else if (ctrs.OptionSelect == SessionObject.EnvoiWord)
                    Utility.ActionExportation<ServicePrintings.CsCentre, ServiceParametrage.CsCentre>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "Centre", "Parametrage", true, "doc");

                else if (ctrs.OptionSelect == SessionObject.EnvoiPdf)
                    Utility.ActionExportation<ServicePrintings.CsCentre, ServiceParametrage.CsCentre>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "Centre", "Parametrage", true, "pdf");

            }
        }

        private void DialogResultOk(object sender, EventArgs e)
        {
            DialogResult ctrs = sender as DialogResult;
            if (ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
            {
                ctrs.DialogResult = false;
                return;
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
                Message.ShowError(ex.Message, Languages.Centre);
            }
        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsCentre;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsCentre;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Centre);
            }
        }

        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsCentre;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsCentre;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Centre);
            }
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {

        }

        private void Cbo_Site_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Cbo_Site.SelectedItem != null)
            {
                CsCentre leSiteSelect = (CsCentre) Cbo_Site.SelectedItem ;
                if (lstCentre != null && lstCentre.Count != 0)
                {
                    DonnesDatagrid.Clear();
                    if (leSiteSelect.CODESITE != "000")
                    foreach (var item in lstCentre.Where(t => t.FK_IDCODESITE ==leSiteSelect.FK_IDCODESITE ))
                        DonnesDatagrid.Add(item);
                    else 
                     foreach (var item in lstCentre )
                        DonnesDatagrid.Add(item);
                }
            }
        }

        #region Sylla 11/06/2016
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var ParamLabel = Galatee.Silverlight.Resources.Langue.ContextMenuCreate;
                var ParamModeExcecution = SessionObject.ExecMode.Creation;
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Ajout + " " + Galatee.Silverlight.Resources.Parametrage.Languages.libelleCentre;
                ShowWindows(ParamLabel, ParamModeExcecution, ParamTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                var ParamLabel = Galatee.Silverlight.Resources.Langue.ContextMenuModify;
                var ParamModeExcecution = SessionObject.ExecMode.Modification;
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Parametrage.Languages.libelleCentre;
                ShowWindows(ParamLabel, ParamModeExcecution, ParamTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowWindows(string ParamLabel, SessionObject.ExecMode ParamModeExcecution, string ParamTitle)
        {
            var contextMenuItem = new ContextMenuItem { Code = Namespace + "UcCentre", Label = ParamLabel, ModeExcecution = ParamModeExcecution, Title = ParamTitle };
            //SessionObject.MenuItemClicked = (MenuItem)sender;
            if (contextMenuItem != null && !string.IsNullOrEmpty(contextMenuItem.Code))
                new DataGridContexMenuBehavior().CreateUserView(contextMenuItem.Code, contextMenuItem.Title, contextMenuItem.ModeExcecution);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                var ParamLabel = Galatee.Silverlight.Resources.Langue.ContextMenuConsult;
                var ParamModeExcecution = SessionObject.ExecMode.Consultation;
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Parametrage.Languages.libelleCentre;
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



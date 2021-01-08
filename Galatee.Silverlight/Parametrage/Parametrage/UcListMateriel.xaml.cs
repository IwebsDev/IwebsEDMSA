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
    public partial class UcListMateriel : ChildWindow, INotifyPropertyChanged
    {
        public CsTypeComptage ObjetSelectionne { get; set; }

        ObservableCollection<CsTypeComptage> donnesDatagrid = new ObservableCollection<CsTypeComptage>();
        string Namespace = "Galatee.Silverlight.Parametrage.";

        public UcListMateriel()
        {
            try
            {
                InitializeComponent();
                Translate();
                GetData();
                this.DataContext = ObjetSelectionne;

                var ContextMenuItem = new List<ContextMenuItem>()
             {
                new ContextMenuItem(){ Code=Namespace+"UcComptage",Label=Galatee.Silverlight.Resources.Langue.ContextMenuCreate,ModeExcecution=SessionObject.ExecMode.Creation,Title =Galatee.Silverlight.Resources.Langue.Ajout+ " " + Galatee.Silverlight.Resources.Parametrage.Languages.LibelleCodePoste },
                new ContextMenuItem(){ Code=Namespace+"UcComptage",Label=Galatee.Silverlight.Resources.Langue.ContextMenuConsult,ModeExcecution=SessionObject.ExecMode.Consultation,Title =Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Parametrage.Languages.LibelleCodePoste },
                new ContextMenuItem(){ Code=Namespace+"UcComptage",Label=Galatee.Silverlight.Resources.Langue.ContextMenuModify,ModeExcecution=SessionObject.ExecMode.Modification,Title =Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Parametrage.Languages.LibelleCodePoste },
             };

                SessionObject.MenuContextuelItem = ContextMenuItem;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleCodePoste);
            }
        }

        private void Translate()
        {
            try
            {
                dtgrdParametre.Columns[0].Header = Languages.Code;
                dtgrdParametre.Columns[1].Header = Languages.Libelle;
                Title = Languages.LibelleCodePoste;
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

        public ObservableCollection<CsTypeComptage > DonnesDatagrid
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
                client.GetAllTypeComptageCompleted += (ssender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.Show(error, Languages.ListeCodePoste);
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
                client.GetAllTypeComptageAsync();
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
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleCodePoste,Languages.QuestionSuppressionDonnees,MessageBoxControl.MessageBoxButtons.YesNo,MessageBoxControl.MessageBoxIcon.Question);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                                                         {
                                                             if (messageBox.Result == MessageBoxResult.OK)
                                                             {
                                                                 if (dtgrdParametre.SelectedItem != null)
                                                                 {
                                                                     var selected =
                                                                         dtgrdParametre.SelectedItem as CsTypeComptage;

                                                                     if (selected != null)
                                                                     {
                                                                         List<CsTypeComptage> lstComptage = new List<CsTypeComptage>();
                                                                         lstComptage.Add(selected);
                                                                         ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                                   Utility.EndPoint("Parametrage"));
                                                                         delete.DeleteTypeComptageCompleted += (del, argDel) =>
                                                                                 {
                                                                                     if (argDel.Cancelled || argDel.Error != null)
                                                                                     {
                                                                                         Message.Show(argDel.Error.Message, Languages.LibelleCodePoste);
                                                                                         return;
                                                                                     }

                                                                                     if (argDel.Result == false)
                                                                                     {
                                                                                         Message.Show(Languages.ErreurSuppressionDonnees, Languages.LibelleCodePoste);
                                                                                         return;
                                                                                     }
                                                                                     DonnesDatagrid.Remove(selected);
                                                                                 };
                                                                         delete.DeleteTypeComptageAsync(lstComptage);
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
                Message.Show(ex.Message, Languages.LibelleCodePoste);
            }
        }

        private void DialogResultPrint(object sender, EventArgs e)
        {
            var dictionaryParam = new Dictionary<string, string>();
            try
            {
                dictionaryParam.Add("RptParam_Code",Languages.Code.ToUpper());
                dictionaryParam.Add("RptParam_Libelle",Languages.Libelle.ToUpper());
                dictionaryParam.Add("RptParam_DateCreation",Languages.DateCreation);
                dictionaryParam.Add("RptParam_DateModification",Languages.DateModification);
                dictionaryParam.Add("RptParam_UserCreation",Languages.UserCreation);
                dictionaryParam.Add("RptParam_UserModification", Languages.UserModification);
                dictionaryParam.Add("RptParam_Title", Languages.ListeCodePoste.ToUpper());
                var ctrs = sender as DialogResult;
                if (ctrs != null && ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
                {
                    
                }
            }
            catch (Exception ex)
            {
            }
        }

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
                dictionaryParam.Add("RptParam_Title", Languages.ListeCodePoste.ToUpper());
                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeAImprimer);
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleCodePoste, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        string key = Utility.getKey();
                        var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                        service.EditerListeMaterielCompleted += (snder, print) =>
                        {
                            if (print.Cancelled || print.Error != null)
                            {
                                Message.Show(print.Error.Message, Languages.LibelleCodePoste);
                                return;
                            }
                            if (!print.Result)
                            {
                                Message.Show(Languages.ErreurImpressionDonnees, Languages.LibelleCodePoste);
                                return;
                            }
                            //Utility.ActionImpressionDirectOrientation(SessionObject.CheminImpression, key, "CodePoste", "Parametrage", false);
                            Utility.ActionPreview(key, string.Empty, "CodePoste", "Parametrage");

                        };
                        service.EditerListeMaterielAsync(key, dictionaryParam);
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
                Message.Show(ex.Message, Languages.LibelleCodePoste);
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
                Message.Show(ex.Message, Languages.LibelleCodePoste);
            }
        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsTypeComptage;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsTypeComptage;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.LibelleCodePoste);
            }
        }
        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsTypeComptage;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsTypeComptage;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.LibelleCodePoste);
            }
        }

        #region Sylla 11/06/2016
        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ParamLabel = Galatee.Silverlight.Resources.Langue.ContextMenuCreate;
                var ParamModeExcecution = SessionObject.ExecMode.Creation;
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Ajout + " Materiel";
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
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Modification + " Materiel" ;
                ShowWindows(ParamLabel, ParamModeExcecution, ParamTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void ShowWindows(string ParamLabel, SessionObject.ExecMode ParamModeExcecution, string ParamTitle)
        {
            var contextMenuItem = new ContextMenuItem { Code = Namespace + "UcComptage", Label = ParamLabel, ModeExcecution = ParamModeExcecution, Title = ParamTitle };
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
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Consultation + " Materiel";
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



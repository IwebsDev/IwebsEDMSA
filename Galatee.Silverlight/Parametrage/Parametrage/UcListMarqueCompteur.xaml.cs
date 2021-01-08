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
    public partial class UcListMarqueCompteur : ChildWindow, INotifyPropertyChanged
    {
        public CsMarqueCompteur ObjetSelectionne { get; set; }

        ObservableCollection<CsMarqueCompteur> donnesDatagrid = new ObservableCollection<CsMarqueCompteur>();
        string Namespace = "Galatee.Silverlight.Parametrage.";

        public UcListMarqueCompteur()
        {
            try
            {
                InitializeComponent();
                Translate();
                GetData();
                this.DataContext = ObjetSelectionne;

             //   var ContextMenuItem = new List<ContextMenuItem>()
             //{
             //   new ContextMenuItem(){ Code=Namespace+"UcMarqueCompteur",Label=Galatee.Silverlight.Resources.Langue.ContextMenuCreate,ModeExcecution=SessionObject.ExecMode.Creation,Title =Galatee.Silverlight.Resources.Langue.Ajout+ " Marque compteur" },
             //   new ContextMenuItem(){ Code=Namespace+"UcMarqueCompteur",Label=Galatee.Silverlight.Resources.Langue.ContextMenuConsult,ModeExcecution=SessionObject.ExecMode.Consultation,Title =Galatee.Silverlight.Resources.Langue.Consultation + " Marque compteur" },
             //   new ContextMenuItem(){ Code=Namespace+"UcMarqueCompteur",Label=Galatee.Silverlight.Resources.Langue.ContextMenuModify,ModeExcecution=SessionObject.ExecMode.Modification,Title =Galatee.Silverlight.Resources.Langue.Modification + " Marque compteur" },
             //};

             //   SessionObject.MenuContextuelItem = ContextMenuItem;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, "Marque compteur");
            }
        }

        private void Translate()
        {
            try
            {
                dtgrdParametre.Columns[0].Header = Languages.Code;
                dtgrdParametre.Columns[1].Header = Languages.Libelle;
                Title = Languages.LibelleParametresGeneraux;
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

        public ObservableCollection<CsMarqueCompteur> DonnesDatagrid
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
                client.SelectAllMarqueCompteurCompleted+= (ssender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.Show(error,"Marque compteur");
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
                client.SelectAllMarqueCompteurAsync();
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
                if (dtgrdParametre.SelectedItem != null && dtgrdParametre.SelectedItems.Count > 0)
                {

                    var w = new MessageBoxControl.MessageBoxChildWindow("Marque compteur", Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                        {
                            var selected = dtgrdParametre.SelectedItem as CsMarqueCompteur;
                            if (selected != null)
                            {
                                ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                 Utility.EndPoint("Parametrage"));
                                delete.DeleteMarqueCompteurCompleted += (del, argDel) =>
                                {
                                    if (argDel.Cancelled || argDel.Error != null)
                                    {
                                        Message.ShowError(argDel.Error.Message, "Marque compteur");
                                        return;
                                    }
                                    if (argDel.Result == false)
                                    {
                                        Message.ShowError(Languages.ErreurSuppressionDonnees, "Marque compteur");
                                        return;
                                    }
                                    DonnesDatagrid.Remove(selected);
                                    GetData();
                                };
                                delete.DeleteMarqueCompteurAsync(selected);
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
                Message.ShowError(ex.Message, "Marque compteur");
            }
        }

        //private void DialogResultDelete(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DialogResult ctrs = sender as DialogResult;
        //        if (ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
        //        {
        //            if (dtgrdParametre.SelectedItem != null)
        //            {
        //                var selected = dtgrdParametre.SelectedItem as CsMarqueCompteur;

        //                if (selected != null)
        //                {
        //                    ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
        //                                                                     Utility.EndPoint("Parametrage"));
        //                    delete.DeleteBanqueCompleted += (del, argDel) =>
        //                                                         {
        //                                                             if (argDel.Cancelled || argDel.Error != null)
        //                                                             {
        //                                                                 var dialogResult = new DialogResult(argDel.Error.Message, "Marque compteur", false, true, false);
        //                                                                 dialogResult.Closed += new EventHandler(DialogResultOk);
        //                                                                 dialogResult.Show();
        //                                                             }

        //                                                             if (argDel.Result == false)
        //                                                             {
        //                                                                 var dialogResult = new DialogResult(Languages.ErreurSuppressionDonnees, "Marque compteur", false, true, false);
        //                                                                 dialogResult.Closed += new EventHandler(DialogResultOk);
        //                                                                 dialogResult.Show();
        //                                                             }

        //                                                             DonnesDatagrid.Remove(selected);
        //                                                         };
        //                    delete.DeleteBanqueAsync(selected);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var dialogResult = new DialogResult(ex.Message, "Marque compteur", false, true, false);
        //        dialogResult.Closed += new EventHandler(DialogResultOk);
        //        dialogResult.Show();

        //    }
        //}

        //private void DialogResultPrint(object sender, EventArgs e)
        //{
        //    var dictionaryParam = new Dictionary<string, string>();
        //    try
        //    {
        //        dictionaryParam.Add("RptParam_Libelle",Languages.Libelle.ToUpper());
        //        dictionaryParam.Add("RptParam_DateCreation",Languages.DateCreation);
        //        dictionaryParam.Add("RptParam_DateModification",Languages.DateModification);
        //        dictionaryParam.Add("RptParam_UserCreation",Languages.UserCreation);
        //        dictionaryParam.Add("RptParam_UserModification", Languages.UserModification);
        //        dictionaryParam.Add("Rpt_Banque", "Marque compteur".ToUpper());
        //        dictionaryParam.Add("Rpt_Guichet", Languages.Guichet.ToUpper());
        //        dictionaryParam.Add("Rpt_Compte", Languages.Compte.ToUpper());
        //        dictionaryParam.Add("RptParam_Title", Languages.ListeBanque.ToUpper());
        //        var ctrs = sender as DialogResult;
        //        if (ctrs != null && ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
        //        {
        //            var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
        //            service.EditerListeBanqueCompleted += (snder, print) =>
        //            {
        //                if (print.Cancelled || print.Error != null)
        //                {
        //                    var dialogResult = new DialogResult(print.Error.Message, "Marque compteur", false, true, false);
        //                    dialogResult.Closed += new EventHandler(DialogResultOk);
        //                    dialogResult.Show();
        //                }
        //                if (!print.Result)
        //                {
        //                    var dialogResult = new DialogResult(Languages.ErreurImpressionDonnees, "Marque compteur", false, true, false);
        //                    dialogResult.Closed += new EventHandler(DialogResultOk);
        //                    dialogResult.Show();
        //                }
        //                Utility.Action(null, "", "Banque", "Parametrage");
        //            };
        //            service.EditerListeBanqueAsync(Utility.getKey(),dictionaryParam);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var dialogResult = new DialogResult(ex.Message, "Marque compteur", false, true, false);
        //        dialogResult.Closed += new EventHandler(DialogResultOk);
        //        dialogResult.Show();
        //    }
        //}
        Dictionary<string, string> param = null;
        List<CsMarqueCompteur> lstDonnee = new List<CsMarqueCompteur>();
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            param = new Dictionary<string, string>();
            try
            {
                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeAImprimer);
                param.Add("RptParam_Code", Languages.Code.ToUpper());
                param.Add("RptParam_Libelle", Languages.Libelle.ToUpper());
                param.Add("RptParam_DateCreation", Languages.DateCreation);
                param.Add("RptParam_DateModification", Languages.DateModification);
                param.Add("RptParam_UserCreation", Languages.UserCreation);
                param.Add("RptParam_UserModification", Languages.UserModification);
                param.Add("RptParam_Title", Languages.ListeParametreGeneraux.ToUpper());
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
                    Utility.ActionDirectOrientation<ServicePrintings.aBanque, ServiceParametrage.CsMarqueCompteur>(lstDonnee, param, SessionObject.CheminImpression, "MarqueCompteur", "Parametrage", true);
                else if (ctrs.OptionSelect == SessionObject.EnvoiExecl)
                    Utility.ActionExportation<ServicePrintings.aBanque, ServiceParametrage.CsMarqueCompteur>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "MarqueCompteur", "Parametrage", true, "xlsx");

                else if (ctrs.OptionSelect == SessionObject.EnvoiWord)
                    Utility.ActionExportation<ServicePrintings.aBanque, ServiceParametrage.CsMarqueCompteur>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "MarqueCompteur", "Parametrage", true, "doc");

                else if (ctrs.OptionSelect == SessionObject.EnvoiPdf)
                    Utility.ActionExportation<ServicePrintings.aBanque, ServiceParametrage.CsMarqueCompteur>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "MarqueCompteur", "Parametrage", true, "pdf");

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
                Message.ShowError(ex.Message, "Marque compteur");
            }

        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsMarqueCompteur;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsMarqueCompteur;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Marque compteur");
            }
        }

        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsMarqueCompteur;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsMarqueCompteur;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Marque compteur");
            }
        }



        #region Sylla 11/06/2016
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var ParamLabel = Galatee.Silverlight.Resources.Langue.ContextMenuCreate;
                var ParamModeExcecution = SessionObject.ExecMode.Creation;
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Ajout + " Marque compteur";
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
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Modification + " Marque compteur";
                ShowWindows(ParamLabel, ParamModeExcecution, ParamTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowWindows(string ParamLabel, SessionObject.ExecMode ParamModeExcecution, string ParamTitle)
        {
          /*  var contextMenuItem = new ContextMenuItem { Code = Namespace + "UcMarqueCompteur", Label = ParamLabel, ModeExcecution = ParamModeExcecution, Title = ParamTitle };
            //SessionObject.MenuItemClicked = (MenuItem)sender;
            if (contextMenuItem != null && !string.IsNullOrEmpty(contextMenuItem.Code))
                new DataGridContexMenuBehavior().CreateUserView(contextMenuItem.Code, contextMenuItem.Title, contextMenuItem.ModeExcecution);*/

            object UserObject = SessionObject.objectSelected;
            DataGrid gridUser = SessionObject.gridUtilisateur;


            UcMarqueCompteur uc = new UcMarqueCompteur(new object[] { UserObject }, new SessionObject.ExecMode[] { ParamModeExcecution }, new DataGrid[] { gridUser });
            uc.Closed += new EventHandler(Form_Closed);
            uc.Show();

        }


        private void Form_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (UcMarqueCompteur)sender;
                if (form != null)
                    GetData();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, "Paramétrage");
            }
        }
        

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                var ParamLabel = Galatee.Silverlight.Resources.Langue.ContextMenuConsult;
                var ParamModeExcecution = SessionObject.ExecMode.Consultation;
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Consultation + " Marque compteur";
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



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
    public partial class UcListChefEquipe : ChildWindow, INotifyPropertyChanged
    {
        public CsGroupe ObjetSelectionne { get; set; }

        ObservableCollection<CsGroupe> donnesDatagrid = new ObservableCollection<CsGroupe>();
        string Namespace = "Galatee.Silverlight.Parametrage.";

        public UcListChefEquipe()
        {
            try
            {
                InitializeComponent();
                Translate();
                GetData();
                this.DataContext = ObjetSelectionne;

                var ContextMenuItem = new List<ContextMenuItem>()
             {
                new ContextMenuItem(){ Code=Namespace+"UcChefEquipe",Label=Galatee.Silverlight.Resources.Langue.ContextMenuCreate,ModeExcecution=SessionObject.ExecMode.Creation,Title =Galatee.Silverlight.Resources.Langue.Ajout+ " Chef équipe" },
                new ContextMenuItem(){ Code=Namespace+"UcChefEquipe",Label=Galatee.Silverlight.Resources.Langue.ContextMenuConsult,ModeExcecution=SessionObject.ExecMode.Consultation,Title =Galatee.Silverlight.Resources.Langue.Consultation + " Chef équipe" },
                new ContextMenuItem(){ Code=Namespace+"UcChefEquipe",Label=Galatee.Silverlight.Resources.Langue.ContextMenuModify,ModeExcecution=SessionObject.ExecMode.Modification,Title =Galatee.Silverlight.Resources.Langue.Modification + " Chef équipe" },
             };

                SessionObject.MenuContextuelItem = ContextMenuItem;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, "Paramétrage");
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

        public ObservableCollection<CsGroupe> DonnesDatagrid
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
                client.SelectAllGRCGroupeCompleted+= (ssender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.Show(error, "Paramétrage");
                            return;
                        }
                        if (args.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }
                        DonnesDatagrid.Clear();
                        if (args.Result != null)
                        {
                            List<CsGroupe> groupes = new List<CsGroupe>();
                            groupes = args.Result.OrderBy(p => p.LIBELLE.Substring(5)).ToList();

                            foreach (var item in groupes)
                            {
                                DonnesDatagrid.Add(item);
                            }
                        dtgrdParametre.ItemsSource = DonnesDatagrid;
                        }
                    };
                client.SelectAllGRCGroupeAsync();
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

                    var w = new MessageBoxControl.MessageBoxChildWindow("Paramétrage", Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                        {
                            var selected = dtgrdParametre.SelectedItem as CsGroupe;
                            if (selected != null)
                            {
                                ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                 Utility.EndPoint("Parametrage"));
                                delete.DeleteGRCGroupeCompleted += (del, argDel) =>
                                {
                                    if (argDel.Cancelled || argDel.Error != null)
                                    {
                                        Message.ShowError(argDel.Error.Message, "Paramétrage");
                                        return;
                                    }
                                    if (argDel.Result == false)
                                    {
                                        Message.ShowError(Languages.ErreurSuppressionDonnees, "Paramétrage");
                                        return;
                                    }
                                    DonnesDatagrid.Remove(selected);
                                };
                                delete.DeleteGRCGroupeAsync(selected);
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
                Message.ShowError(ex.Message, "Paramétrage");
            }
        }


        Dictionary<string, string> param = null;
        List<CsGroupe> lstDonnee = new List<CsGroupe>();
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
                    Utility.ActionDirectOrientation<ServicePrintings.aBanque, ServiceParametrage.CsGroupe>(lstDonnee, param, SessionObject.CheminImpression, "GRCGroupe", "Parametrage", true);
                else if (ctrs.OptionSelect == SessionObject.EnvoiExecl)
                    Utility.ActionExportation<ServicePrintings.aBanque, ServiceParametrage.CsGroupe>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "GRCGroupe", "Parametrage", true, "xlsx");

                else if (ctrs.OptionSelect == SessionObject.EnvoiWord)
                    Utility.ActionExportation<ServicePrintings.aBanque, ServiceParametrage.CsGroupe>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "GRCGroupe", "Parametrage", true, "doc");

                else if (ctrs.OptionSelect == SessionObject.EnvoiPdf)
                    Utility.ActionExportation<ServicePrintings.aBanque, ServiceParametrage.CsGroupe>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "GRCGroupe", "Parametrage", true, "pdf");

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
                Message.ShowError(ex.Message, "Paramétrage");
            }

        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsGroupe;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsGroupe;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Paramétrage");
            }
        }

        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsGroupe;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsGroupe;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Paramétrage");
            }
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var ParamLabel = Galatee.Silverlight.Resources.Langue.ContextMenuCreate;
                var ParamModeExcecution = SessionObject.ExecMode.Creation;
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Ajout + " Chef équipe";
                //ShowWindows(ParamLabel, ParamModeExcecution, ParamTitle);

                Search();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void Search()
        {
            try
            {
                UserAgentsPicker FormUserAgentPicker = new UserAgentsPicker();
 
                FormUserAgentPicker.Closed += new EventHandler(FormUserAgentPicker_Closed);
                FormUserAgentPicker.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FormUserAgentPicker_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (UserAgentsPicker)sender;
                if (form != null)
                {
                    if (form.DialogResult == true && form.AgentsSelectionnes != null &&  form.AgentsSelectionnes.Count > 0)
                    {
                        var agent = form.AgentsSelectionnes;
                        if (agent != null)
                        {
                            List<CsGroupe> lstGroup = new List<CsGroupe>();
                            CsGroupe group;

                            foreach (ServiceAccueil.CsUtilisateur st in agent)
                            {
                                group = new CsGroupe();

                                group.ID = Guid.NewGuid();
                                group.LIBELLE = st.MATRICULE + " " + st.LIBELLE;
                                group.ID_TYPE_RECLAMATION = 1;
                                group.ID_NATURE_BASE = null;
                                group.EST_SUPPRIME = false;
                                group.CREER_PAR = UserConnecte.matricule;
                                group.DATE_CREATION = DateTime.Now;
                                group.DATE_MODIFICATION = null;

                                CsGroupe gr = DonnesDatagrid.FirstOrDefault(t => t.LIBELLE == group.LIBELLE);
                                if (gr != null)
                                    throw new Exception(string.Format("L'agent {0} existe déjà comme chef d'équipe",group.LIBELLE));
                                lstGroup.Add(group);
                            }

                            InsertorUpdate(lstGroup);
                        }
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, "Paramétrage");
            }
        }




        private void InsertorUpdate(List<CsGroupe> lGroupes)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow("Paramétrage", Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));

                        if (lGroupes != null && lGroupes.Count > 0)
                        {
                                service.InsertGRCGroupeCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.Show(insertR.Error.Message, "Paramétrage");
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.Show(Languages.ErreurInsertionDonnees, "Paramétrage");
                                        return;
                                    }
                                    GetData();
                                };
                                service.InsertGRCGroupeAsync(lGroupes);
                        }
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
                Message.Show(ex.Message, "Paramétrage");
            }

        }






        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                var ParamLabel = Galatee.Silverlight.Resources.Langue.ContextMenuModify;
                var ParamModeExcecution = SessionObject.ExecMode.Modification;
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Modification + " Chef équipe";
                ShowWindows(ParamLabel, ParamModeExcecution, ParamTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowWindows(string ParamLabel, SessionObject.ExecMode ParamModeExcecution, string ParamTitle)
        {
            var contextMenuItem = new ContextMenuItem { Code = Namespace + "UcChefEquipe", Label = ParamLabel, ModeExcecution = ParamModeExcecution, Title = ParamTitle };
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
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Consultation + " Chef équipe";
                ShowWindows(ParamLabel, ParamModeExcecution, ParamTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}



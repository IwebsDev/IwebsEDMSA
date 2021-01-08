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
    public partial class UcListCoperDemande : ChildWindow
    {
        public CsCoutDemande ObjetSelectionne { get; set; }

        ObservableCollection<CsCoutDemande> donnesDatagrid = new ObservableCollection<CsCoutDemande>();
        string Namespace = "Galatee.Silverlight.Parametrage.";

        public UcListCoperDemande()
        {
            try
            {
                InitializeComponent();
                Translate();
                GetData();
                this.DataContext = ObjetSelectionne;

                var ContextMenuItem = new List<ContextMenuItem>()
             {
                new ContextMenuItem(){ Code=Namespace+"UcCoperDemande",Label=Galatee.Silverlight.Resources.Langue.ContextMenuCreate,ModeExcecution=SessionObject.ExecMode.Creation,Title =Galatee.Silverlight.Resources.Langue.Ajout+ " " + Galatee.Silverlight.Resources.Parametrage.Languages.Coutcoper },
                new ContextMenuItem(){ Code=Namespace+"UcCoperDemande",Label=Galatee.Silverlight.Resources.Langue.ContextMenuConsult,ModeExcecution=SessionObject.ExecMode.Consultation,Title =Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Parametrage.Languages.Coutcoper },
                new ContextMenuItem(){ Code=Namespace+"UcCoperDemande",Label=Galatee.Silverlight.Resources.Langue.ContextMenuModify,ModeExcecution=SessionObject.ExecMode.Modification,Title =Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Parametrage.Languages.Coutcoper },
             };

                SessionObject.MenuContextuelItem = ContextMenuItem;

            }
            catch (Exception ex)
            {

                Message.Show(ex.Message, Languages.Coutcoper);
            }
        }
        private void Translate()
        {
            try
            {
                //dtgrdParametre.Columns[0].Header = Languages.libelleCentre;
                //dtgrdParametre.Columns[1].Header = Languages.LibelleProduit;
                //dtgrdParametre.Columns[2].Header = Languages.TDEM;
                //dtgrdParametre.Columns[3].Header = Languages.LibelleCOPER;
                //dtgrdParametre.Columns[4].Header = Languages.libelleTaxe;
                //dtgrdParametre.Columns[5].Header = Languages.LibelleMONTANT;
                Title = Languages.CoperDemande;
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
        public ObservableCollection<CsCoutDemande> DonnesDatagrid
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
                client.SelectAllCoperDemandeCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.CoperDemande);
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
                        // Combo typebranchement
                        var lstTypeDemandeDistnct = args.Result.Select(t => new { t.LIBELLETYPEDEMANDE, t.TYPEDEMANDE }).Distinct().ToList();
                        List<CsCoutDemande> lstTypeDemande = new List<CsCoutDemande>();
                        foreach (var item in lstTypeDemandeDistnct)
                        {
                            CsCoutDemande leCout = new CsCoutDemande()
                            {
                                TYPEDEMANDE = item.TYPEDEMANDE,
                                LIBELLETYPEDEMANDE = item.LIBELLETYPEDEMANDE
                            };
                            lstTypeDemande.Add(leCout);

                        }
                        cbo_typedemande.DisplayMemberPath = "LIBELLETYPEDEMANDE";
                        cbo_typedemande.ItemsSource = lstTypeDemande;
                        //

                        // Combo Produit
                        var lstProduitDistnct = args.Result.Select(t => new { t.PRODUIT , t.LIBELLEPRODUIT  }).Distinct().ToList();
                        List<CsCoutDemande> lstProduit = new List<CsCoutDemande>();
                        foreach (var item in lstProduitDistnct)
                        {
                            CsCoutDemande leCout = new CsCoutDemande
                            {
                                PRODUIT = item.PRODUIT,
                                LIBELLEPRODUIT = item.LIBELLEPRODUIT
                            };
                            lstProduit.Add(leCout);

                        }
                        cbo_produit.DisplayMemberPath = "LIBELLEPRODUIT";
                        cbo_produit.ItemsSource = lstProduit;
                        //
                        // Combo calibre
                        var lstcalibreDistnct = args.Result.Select(t => new { t.REGLAGECOMPTEUR  , t.LIBELLEREGLAGECOMPTEUR }).Distinct().ToList();
                        List<CsCoutDemande> lstCalibre= new List<CsCoutDemande>();
                        foreach (var item in lstcalibreDistnct)
                        {
                            CsCoutDemande leCout = new CsCoutDemande
                            {
                                REGLAGECOMPTEUR = item.REGLAGECOMPTEUR,
                                LIBELLEREGLAGECOMPTEUR = item.LIBELLEREGLAGECOMPTEUR
                            };
                            lstCalibre.Add(leCout);

                        }
                        cbo_Calibre.DisplayMemberPath = "LIBELLEREGLAGECOMPTEUR";
                        cbo_Calibre.ItemsSource = lstCalibre;
                        //

                        foreach (var item in args.Result.OrderBy(t => t.CENTRE).ThenBy(u=>u.PRODUIT).ThenBy(y => y.TYPEDEMANDE).ThenBy(k=>k.REGLAGECOMPTEUR).ToList())
                            DonnesDatagrid.Add(item);
                    }
                    dtgrdParametre.ItemsSource = DonnesDatagrid;
                };
                client.SelectAllCoperDemandeAsync();
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
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.CoperDemande, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            if (dtgrdParametre.SelectedItem != null)
                            {
                                var selected = dtgrdParametre.SelectedItem as CsCoutDemande;

                                if (selected != null)
                                {
                                    ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                     Utility.EndPoint("Parametrage"));
                                    delete.DeleteCoperDemandeCompleted += (del, argDel) =>
                                    {
                                        if (argDel.Cancelled || argDel.Error != null)
                                        {
                                            Message.ShowError(argDel.Error.Message, Languages.CoperDemande);
                                            return;
                                        }

                                        if (argDel.Result == false)
                                        {
                                            Message.ShowError(Languages.ErreurSuppressionDonnees, Languages.CoperDemande);
                                            return;
                                        }
                                        DonnesDatagrid.Remove(selected);
                                    };
                                    delete.DeleteCoperDemandeAsync(selected);
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
                Message.Show(ex.Message, Languages.CoperDemande);
            }
        }
        Dictionary<string, string> param = null;
        List<CsCoutDemande> lstDonnee = new List<CsCoutDemande>();
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
            
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
                    Utility.ActionDirectOrientation<ServicePrintings.CsCoutDemande, ServiceParametrage.CsCoutDemande>(lstDonnee, param, SessionObject.CheminImpression, "CoutDemande", "Parametrage", true);
                else if (ctrs.OptionSelect == SessionObject.EnvoiExecl)
                    Utility.ActionExportation<ServicePrintings.CsCoutDemande, ServiceParametrage.CsCoutDemande>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "CoutDemande", "Parametrage", true, "xlsx");

                else if (ctrs.OptionSelect == SessionObject.EnvoiWord)
                    Utility.ActionExportation<ServicePrintings.CsCoutDemande, ServiceParametrage.CsCoutDemande>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "CoutDemande", "Parametrage", true, "doc");

                else if (ctrs.OptionSelect == SessionObject.EnvoiPdf)
                    Utility.ActionExportation<ServicePrintings.CsCoutDemande, ServiceParametrage.CsCoutDemande>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "CoutDemande", "Parametrage", true, "pdf");

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
                Message.Show(ex.Message, Languages.Coutcoper);
            }
        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsCoutDemande;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsCoutDemande;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.CoperDemande);
            }
        }
        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsCoutDemande;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsCoutDemande;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.CoperDemande);
            }
        }

        private void cbo_typedemande_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RecherchebyCritere();

        }

        private void cbo_produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.PRODUIT  == ((CsCoutDemande)cbo_produit.SelectedItem).PRODUIT ).ToList();
            //ObservableCollection<CsCoutDemande> lstCout = new ObservableCollection<CsCoutDemande>();
            //foreach (var item in lstCoutTypeDemande)
            //    lstCout.Add(item);
            //dtgrdParametre.ItemsSource = null;
            //dtgrdParametre.ItemsSource = lstCout;
            RecherchebyCritere();
        }

        private void cbo_Calibre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.REGLAGECOMPTEUR == ((CsCoutDemande)cbo_Calibre.SelectedItem).REGLAGECOMPTEUR).ToList();
            //ObservableCollection<CsCoutDemande> lstCout = new ObservableCollection<CsCoutDemande>();
            //foreach (var item in lstCoutTypeDemande)
            //    lstCout.Add(item);
            //dtgrdParametre.ItemsSource = null;
            //dtgrdParametre.ItemsSource = lstCout;
            RecherchebyCritere();
        }
        private void RecherchebyCritere()
        {
            ObservableCollection<CsCoutDemande> lstCout = new ObservableCollection<CsCoutDemande>();
            if (this.cbo_typedemande.SelectedItem != null && this.cbo_produit.SelectedItem == null && cbo_Calibre.SelectedItem == null)
            {
                var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.TYPEDEMANDE == ((CsCoutDemande)cbo_typedemande.SelectedItem).TYPEDEMANDE).ToList();
                foreach (var item in lstCoutTypeDemande)
                    lstCout.Add(item);
            }
            if (this.cbo_typedemande.SelectedItem == null && this.cbo_produit.SelectedItem != null && cbo_Calibre.SelectedItem == null)
            {
                var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.PRODUIT == ((CsProduit)cbo_produit.SelectedItem).CODE ).ToList();
                foreach (var item in lstCoutTypeDemande)
                    lstCout.Add(item);
            }
            if (this.cbo_typedemande.SelectedItem == null && this.cbo_produit.SelectedItem == null && cbo_Calibre.SelectedItem != null)
            {
                var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.REGLAGECOMPTEUR == ((CsReglageCompteur)cbo_Calibre.SelectedItem).CODE ).ToList();
                foreach (var item in lstCoutTypeDemande)
                    lstCout.Add(item);
            }
            if (this.cbo_typedemande.SelectedItem != null && this.cbo_produit.SelectedItem != null && cbo_Calibre.SelectedItem == null)
            {
                var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.TYPEDEMANDE == ((CsCoutDemande)cbo_typedemande.SelectedItem).TYPEDEMANDE &&  t.PRODUIT  == ((CsCoutDemande)cbo_produit.SelectedItem).PRODUIT ).ToList();
                foreach (var item in lstCoutTypeDemande)
                    lstCout.Add(item);
            }
            if (this.cbo_typedemande.SelectedItem != null && this.cbo_produit.SelectedItem == null && cbo_Calibre.SelectedItem != null)
            {
                var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.TYPEDEMANDE == ((CsCoutDemande)cbo_typedemande.SelectedItem).TYPEDEMANDE && t.REGLAGECOMPTEUR  == ((CsCoutDemande)cbo_produit.SelectedItem).REGLAGECOMPTEUR ).ToList();
                foreach (var item in lstCoutTypeDemande)
                    lstCout.Add(item);
            }
            if (this.cbo_typedemande.SelectedItem != null && this.cbo_produit.SelectedItem != null && cbo_Calibre.SelectedItem != null)
            {
                var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.TYPEDEMANDE == ((CsCoutDemande)cbo_typedemande.SelectedItem).TYPEDEMANDE && t.PRODUIT == ((CsCoutDemande)cbo_produit.SelectedItem).PRODUIT && t.REGLAGECOMPTEUR == ((CsCoutDemande)cbo_Calibre.SelectedItem).REGLAGECOMPTEUR).ToList();
                foreach (var item in lstCoutTypeDemande)
                    lstCout.Add(item);
            }
            dtgrdParametre.ItemsSource = null;
            dtgrdParametre.ItemsSource = lstCout;
        }

        #region Sylla 11/06/2016
        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ParamLabel = Galatee.Silverlight.Resources.Langue.ContextMenuCreate;
                var ParamModeExcecution = SessionObject.ExecMode.Creation;
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Ajout + " " + Galatee.Silverlight.Resources.Parametrage.Languages.LibelleCOPER;
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
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Parametrage.Languages.LibelleCOPER;
                ShowWindows(ParamLabel, ParamModeExcecution, ParamTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowWindows(string ParamLabel, SessionObject.ExecMode ParamModeExcecution, string ParamTitle)
        {
            var contextMenuItem = new ContextMenuItem { Code = Namespace + "UcCoperDemande", Label = ParamLabel, ModeExcecution = ParamModeExcecution, Title = ParamTitle };
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
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Parametrage.Languages.LibelleCOPER;
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


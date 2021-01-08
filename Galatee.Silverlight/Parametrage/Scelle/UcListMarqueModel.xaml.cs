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
    public partial class UcListMarqueModel : ChildWindow
    {
        public CsMarque_Modele  ObjetSelectionne { get; set; }

        ObservableCollection<CsMarque_Modele> donnesDatagrid = new ObservableCollection<CsMarque_Modele>();

        public UcListMarqueModel()
        {
            try
            {
                InitializeComponent();
                Translate();
                GetData();
                this.DataContext = ObjetSelectionne;

                var Namespace = "Galatee.Silverlight.Parametrage.";
                var ContextMenuItem = new List<ContextMenuItem>()
             {
                new ContextMenuItem(){ Code=Namespace+"UcMarqueModel",Label=Galatee.Silverlight.Resources.Langue.ContextMenuCreate,ModeExcecution=SessionObject.ExecMode.Creation,Title =Galatee.Silverlight.Resources.Langue.Ajout+ " " + Galatee.Silverlight.Resources.Parametrage.Languages.Coutcoper },
                new ContextMenuItem(){ Code=Namespace+"UcMarqueModel",Label=Galatee.Silverlight.Resources.Langue.ContextMenuConsult,ModeExcecution=SessionObject.ExecMode.Consultation,Title =Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Parametrage.Languages.Coutcoper },
                new ContextMenuItem(){ Code=Namespace+"UcMarqueModel",Label=Galatee.Silverlight.Resources.Langue.ContextMenuModify,ModeExcecution=SessionObject.ExecMode.Modification,Title =Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Parametrage.Languages.Coutcoper },
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
 
                Title = Languages.CoperDemande;
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
        public ObservableCollection<CsMarque_Modele > DonnesDatagrid
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
        List<CsMarque_Modele> lstInit = new List<CsMarque_Modele>();
        private void GetData()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllMarqueModeleCompleted += (ssender, args) =>
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
                        lstInit = args.Result;
                        foreach (var item in args.Result)
                            DonnesDatagrid.Add(item);
                    }
                    dtgrdParametre.ItemsSource = DonnesDatagrid;
                };
                client.SelectAllMarqueModeleAsync ();
            }
            catch (Exception ex)
            {
                throw ex;
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

        }
        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsMarque_Modele ;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsMarque_Modele;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.CoperDemande);
            }
        }
 


        private void btnNouveau_Click(object sender, RoutedEventArgs e)
        {
            UcMarqueModel ctrl = new UcMarqueModel(lstInit);
            ctrl.Closed += new EventHandler(ctrlr_Closed);
            ctrl.Show();
        }

        private void ctrlr_Closed(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnModifier_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.dtgrdParametre.SelectedItem != null)
            {
                UcMarqueModel ctr = new UcMarqueModel( lstInit,(CsMarque_Modele )this.dtgrdParametre.SelectedItem);
                ctr.Closed += new EventHandler(ctrlr_Closed);
                ctr.Show();
            }
        }
    }
}


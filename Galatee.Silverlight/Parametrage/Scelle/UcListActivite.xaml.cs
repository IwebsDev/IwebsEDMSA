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
    public partial class UcListActivite : ChildWindow
    {
        public CsActivite ObjetSelectionne { get; set; }

        ObservableCollection<CsActivite> donnesDatagrid = new ObservableCollection<CsActivite>();

        public UcListActivite()
        {
            try
            {
                InitializeComponent();
                Translate();
                GetData();
                this.DataContext = ObjetSelectionne;
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
        public ObservableCollection<CsActivite> DonnesDatagrid
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
                client.SelectAllActiviteCompleted += (ssender, args) =>
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
                        foreach (var item in args.Result )
                            DonnesDatagrid.Add(item);
                    }
                    dtgrdParametre.ItemsSource = DonnesDatagrid;
                };
                client.SelectAllActiviteAsync();
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
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsActivite  ;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsActivite;
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
            //var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.DIAMETRE == ((CsCoutDemande)cbo_Calibre.SelectedItem).DIAMETRE).ToList();
            //ObservableCollection<CsCoutDemande> lstCout = new ObservableCollection<CsCoutDemande>();
            //foreach (var item in lstCoutTypeDemande)
            //    lstCout.Add(item);
            //dtgrdParametre.ItemsSource = null;
            //dtgrdParametre.ItemsSource = lstCout;
            RecherchebyCritere();
        }
        private void RecherchebyCritere()
        {
            //ObservableCollection<CsCoutDemande> lstCout = new ObservableCollection<CsCoutDemande>();
            //if (this.cbo_typedemande.SelectedItem != null && this.cbo_produit.SelectedItem == null && cbo_Calibre.SelectedItem == null)
            //{
            //    var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.TYPEDEMANDE == ((CsCoutDemande)cbo_typedemande.SelectedItem).TYPEDEMANDE).ToList();
            //    foreach (var item in lstCoutTypeDemande)
            //        lstCout.Add(item);
            //}
            //if (this.cbo_typedemande.SelectedItem == null && this.cbo_produit.SelectedItem != null && cbo_Calibre.SelectedItem == null)
            //{
            //    var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.PRODUIT == ((CsCoutDemande)cbo_typedemande.SelectedItem).PRODUIT).ToList();
            //    foreach (var item in lstCoutTypeDemande)
            //        lstCout.Add(item);
            //}
            //if (this.cbo_typedemande.SelectedItem == null && this.cbo_produit.SelectedItem == null && cbo_Calibre.SelectedItem != null)
            //{
            //    var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.DIAMETRE == ((CsCoutDemande)cbo_typedemande.SelectedItem).DIAMETRE).ToList();
            //    foreach (var item in lstCoutTypeDemande)
            //        lstCout.Add(item);
            //}
            //if (this.cbo_typedemande.SelectedItem != null && this.cbo_produit.SelectedItem != null && cbo_Calibre.SelectedItem == null)
            //{
            //    var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.TYPEDEMANDE == ((CsCoutDemande)cbo_typedemande.SelectedItem).TYPEDEMANDE &&  t.PRODUIT  == ((CsCoutDemande)cbo_produit.SelectedItem).PRODUIT ).ToList();
            //    foreach (var item in lstCoutTypeDemande)
            //        lstCout.Add(item);
            //}
            //if (this.cbo_typedemande.SelectedItem != null && this.cbo_produit.SelectedItem == null && cbo_Calibre.SelectedItem != null)
            //{
            //    var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.TYPEDEMANDE == ((CsCoutDemande)cbo_typedemande.SelectedItem).TYPEDEMANDE && t.DIAMETRE  == ((CsCoutDemande)cbo_produit.SelectedItem).DIAMETRE ).ToList();
            //    foreach (var item in lstCoutTypeDemande)
            //        lstCout.Add(item);
            //}
            //if (this.cbo_typedemande.SelectedItem != null && this.cbo_produit.SelectedItem != null && cbo_Calibre.SelectedItem != null)
            //{
            //    var lstCoutTypeDemande = DonnesDatagrid.Where(t => t.TYPEDEMANDE == ((CsCoutDemande)cbo_typedemande.SelectedItem).TYPEDEMANDE && t.PRODUIT == ((CsCoutDemande)cbo_produit.SelectedItem).PRODUIT && t.DIAMETRE == ((CsCoutDemande)cbo_Calibre.SelectedItem).DIAMETRE).ToList();
            //    foreach (var item in lstCoutTypeDemande)
            //        lstCout.Add(item);
            //}
            //dtgrdParametre.ItemsSource = null;
            //dtgrdParametre.ItemsSource = lstCout;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
               var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Caisse, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
               messageBox.OnMessageBoxClosed += (_, result) =>
               {
                   if (messageBox.Result == MessageBoxResult.OK)
                   {
                       if (!string.IsNullOrEmpty(this.txt_LibelleModele.Text))
                       {
                           List<CsActivite> lstModel = new List<CsActivite>();
                           lstModel.Add(new CsActivite()
                           {
                                Activite_Libelle = this.txt_LibelleModele.Text ,
                                Activite_ID =(DonnesDatagrid != null && DonnesDatagrid.Count != 0)? DonnesDatagrid.Max(t=>t.Activite_ID ) + 1 : 1
                           });
                           InsererModel(lstModel);
                           GetData();
                       }
                   }
               };
               messageBox.Show();
        }
        private void InsererModel(List<CsActivite> leModel)
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.InsertActiviteCompleted += (ssender, args) =>
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
                    if (args.Result == true)
                    {
                        Message.ShowInformation("Mise à jour effectuée avec succès", "Parametrage");
                        GetData();
                    }
                    else
                        Message.ShowInformation("Erreur lors de la Mise à jour", "Parametrage");
                };
                client.InsertActiviteAsync(leModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateModel(List<CsActivite> leModel)
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.UpdateActiviteCompleted += (ssender, args) =>
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
                    if (args.Result == true)
                    {
                        Message.ShowInformation("Mise à jour effectuée avec succès", "Parametrage");
                        GetData();
                    }
                    else
                        Message.ShowInformation("Erreur lors de la mise à jour", "Parametrage");
                    GetData();
                };
                client.UpdateActiviteAsync(leModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_Modifier_Click(object sender, RoutedEventArgs e)
        {
               var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Caisse, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
               messageBox.OnMessageBoxClosed += (_, result) =>
               {
                   if (messageBox.Result == MessageBoxResult.OK)
                   {
                        if (this.dtgrdParametre.SelectedItem != null)
                        {
                            CsActivite leSelect = (CsActivite)this.dtgrdParametre.SelectedItem;
                            leSelect.Activite_Libelle   = this.txt_LibelleModele.Text;
                            List<CsActivite> lstMar = new List<CsActivite>();
                            lstMar.Add(leSelect);
                            UpdateModel(lstMar);
                        }
                   }
               };
               messageBox.Show();
        }
    }
}


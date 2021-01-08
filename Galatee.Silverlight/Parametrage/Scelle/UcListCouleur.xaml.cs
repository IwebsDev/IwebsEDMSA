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
    public partial class UcListCouleur : ChildWindow
    {
        public CsCouleurScelle ObjetSelectionne { get; set; }

        ObservableCollection<CsCouleurScelle> donnesDatagrid = new ObservableCollection<CsCouleurScelle>();

        public UcListCouleur()
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
        public ObservableCollection<CsCouleurScelle> DonnesDatagrid
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
                client.SelectAllCouleurCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.CoperDemande);
                        return;
                    }
                    //if (args.Result.First().Couleur_libelle  == null)
                    //{
                    //    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                    //    return;
                    //}
                    DonnesDatagrid.Clear();
                    if (args.Result != null)
                    {
                        foreach (var item in args.Result)
                            DonnesDatagrid.Add(item);
                    }
                    dtgrdParametre.ItemsSource = DonnesDatagrid;
                };
                client.SelectAllCouleurAsync();
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
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsCouleurScelle  ;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsCouleurScelle;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.CoperDemande);
            }
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
                           List<CsCouleurScelle> lstModel = new List<CsCouleurScelle>();
                           lstModel.Add(new CsCouleurScelle()
                           {
                                Couleur_ID =(DonnesDatagrid != null && DonnesDatagrid.Count != 0)? DonnesDatagrid.Max(t=>t.Couleur_ID  ) + 1 : 1,
                                 Couleur_libelle = this.txt_LibelleModele.Text 
                                
                           });
                           InsererModel(lstModel);
                       }
                   }
               };
               messageBox.Show();
        }
        private void InsererModel(List<CsCouleurScelle> leModel)
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.InsertCouleurCompleted += (ssender, args) =>
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
                client.InsertCouleurAsync(leModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateModel(List<CsCouleurScelle> leModel)
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.UpdateCouleurCompleted += (ssender, args) =>
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
                client.UpdateCouleurAsync(leModel);
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
                            CsCouleurScelle leSelect = (CsCouleurScelle)this.dtgrdParametre.SelectedItem;
                            leSelect.Couleur_libelle  = this.txt_LibelleModele.Text;
                            List<CsCouleurScelle> lstMar = new List<CsCouleurScelle>();
                            lstMar.Add(leSelect);
                            UpdateModel(lstMar);
                        }
                   }
               };
               messageBox.Show();
        }
    }
}


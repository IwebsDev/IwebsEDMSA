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
    public partial class UcActiviteCouleur : ChildWindow
    {
        public CsCouleurActivite ObjetSelectionne { get; set; }

        ObservableCollection<CsCouleurActivite> donnesDatagrid = new ObservableCollection<CsCouleurActivite>();
        List<CsCouleurActivite> listForInsertOrUpdate = new List<CsCouleurActivite>();
        List<CsCouleurActivite> listInitial = new List<CsCouleurActivite>();
        private CsCouleurActivite ObjetSelectionnee = null;

        public UcActiviteCouleur(List<CsCouleurActivite>  listInit )
        {
            try
            {
                InitializeComponent();
                Translate();
                RetourneActivite();
                RetourneCouleur();
                listInitial = listInit;
            }
            catch (Exception ex)
            {

                Message.Show(ex.Message, Languages.Coutcoper);
            }
        }


        public UcActiviteCouleur(CsCouleurActivite leActvtCouleur, List<CsCouleurActivite> listInit)
        {
            try
            {
                InitializeComponent();
                Translate();
                RetourneActivite();
                RetourneCouleur();
                listInitial = listInit;
                ObjetSelectionnee = leActvtCouleur;
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
        public ObservableCollection<CsCouleurActivite> DonnesDatagrid
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
        private void RetourneCouleur()
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
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    this.Cbo_Couleur.ItemsSource = null;
                    this.Cbo_Couleur.DisplayMemberPath = "Couleur_libelle";
                    this.Cbo_Couleur.ItemsSource = args.Result;

                };
                client.SelectAllCouleurAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RetourneActivite()
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
                    this.Cbo_Activite.ItemsSource = null;
                    this.Cbo_Activite.DisplayMemberPath = "Activite_Libelle";
                    this.Cbo_Activite.ItemsSource = args.Result;
                };
                client.SelectAllActiviteAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    

        private List<CsCouleurActivite> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsCouleurActivite>();
            try
            {

                if (ObjetSelectionnee == null)
                {

                    var tCompteur = new CsCouleurActivite
                    {
                        Activite_ID = ((CsActivite)Cbo_Activite.SelectedItem).Activite_ID,
                        Couleur_ID  = ((CsCouleurScelle )Cbo_Couleur.SelectedItem).Couleur_ID ,
                        PK_ID = Guid.NewGuid()
                    };
                    listObjetForInsertOrUpdate.Add(tCompteur);
                }

                if (ObjetSelectionnee != null)
                {
                    ObjetSelectionnee.Activite_ID = ((CsActivite)Cbo_Activite.SelectedItem).Activite_ID;
                    ObjetSelectionnee.Couleur_ID  = ((CsCouleurScelle)Cbo_Couleur.SelectedItem).Couleur_ID;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.EtatDuCompteur);
                return null;
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Commune, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {

                        listForInsertOrUpdate = GetInformationsFromScreen();
                        var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                        if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        {
                            if (ObjetSelectionnee == null)
                            {
                                if (listInitial != null && listInitial.FirstOrDefault(t => t.Couleur_ID  == listForInsertOrUpdate.First().Activite_ID  && t.Couleur_ID  == listForInsertOrUpdate.First().Couleur_ID ) != null)
                                {
                                    Message.ShowWarning("Ce model a deja été paramétré", "Parametrage");
                                    listForInsertOrUpdate.Clear();
                                    return;
                                }
                                service.InsertActiviteCouleurCompleted  += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                       insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.CoperDemande);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.CoperDemande);
                                        return;
                                    }
                                    this.DialogResult = false;
                                };
                                service.InsertActiviteCouleurAsync(listForInsertOrUpdate);
                            }

                            if (ObjetSelectionnee != null)
                            {
                                service.UpdateActiviteCouleurCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.CoperDemande);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.CoperDemande);
                                        return;
                                    }
                                    this.DialogResult = false;
                                };
                                service.UpdateActiviteCouleurAsync(listForInsertOrUpdate);
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
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Commune);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }
    }
}


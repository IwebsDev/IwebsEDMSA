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
    public partial class UcListModel : ChildWindow
    {
        public CsMarque_Modele  ObjetSelectionne { get; set; }

        ObservableCollection<CsMarque_Modele> donnesDatagrid = new ObservableCollection<CsMarque_Modele>();

        public UcListModel()
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

        private void GetData()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllModelCompleted += (ssender, args) =>
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
                        foreach (var item in args.Result)
                            DonnesDatagrid.Add(item);
                    }
                    dtgrdParametre.ItemsSource = DonnesDatagrid;
                };
                client.SelectAllModelAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void btnDelete_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (DonnesDatagrid.Count == 0)
        //            throw new Exception(Languages.AucuneDonneeASupprimer);
        //        if (dtgrdParametre.SelectedItems.Count > 0)
        //        {
        //            var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.CoperDemande, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
        //            messageBox.OnMessageBoxClosed += (_, result) =>
        //            {
        //                if (messageBox.Result == MessageBoxResult.OK)
        //                {
        //                    if (dtgrdParametre.SelectedItem != null)
        //                    {
        //                        var selected = dtgrdParametre.SelectedItem as CsCoutDemande;

        //                        if (selected != null)
        //                        {
        //                            ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
        //                                                                             Utility.EndPoint("Parametrage"));
        //                            delete.DeleteCoperDemandeCompleted += (del, argDel) =>
        //                            {
        //                                if (argDel.Cancelled || argDel.Error != null)
        //                                {
        //                                    Message.ShowError(argDel.Error.Message, Languages.CoperDemande);
        //                                    return;
        //                                }

        //                                if (argDel.Result == false)
        //                                {
        //                                    Message.ShowError(Languages.ErreurSuppressionDonnees, Languages.CoperDemande);
        //                                    return;
        //                                }
        //                                //DonnesDatagrid.Remove(selected);
        //                            };
        //                            delete.DeleteCoperDemandeAsync(selected);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    return;
        //                }
        //            };
        //            messageBox.Show();
        //        }
        //        else
        //        {
        //            throw new Exception(Languages.SelectionnerUnElement);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.Show(ex.Message, Languages.CoperDemande);
        //    }
        //}

        //private void btnPrint_Click(object sender, RoutedEventArgs e)
        //{
        //    var dictionaryParam = new Dictionary<string, string>();
        //    try
        //    {
        //        dictionaryParam.Add("RptParam_CENTRE", Languages.Centre.ToUpper());
        //        dictionaryParam.Add("RptParam_PRODUIT", Languages.LibelleProduit.ToUpper());
        //        dictionaryParam.Add("RptParam_PUISSANCE", Languages.LibellePUISSANCE.ToUpper());
        //        dictionaryParam.Add("RptParam_CODETARIF", Languages.LibelleCODETARIF.ToUpper());
        //        dictionaryParam.Add("RptParam_COPER", Languages.LibelleCOPER.ToUpper());
        //        dictionaryParam.Add("RptParam_MONTANT", Languages.LibelleMONTANT.ToUpper());
        //        dictionaryParam.Add("RptParam_DateCreation", Languages.DateCreation);
        //        dictionaryParam.Add("RptParam_DateModification", Languages.DateModification);
        //        dictionaryParam.Add("RptParam_UserCreation", Languages.UserCreation);
        //        dictionaryParam.Add("RptParam_UserModification", Languages.UserModification);
        //        dictionaryParam.Add("RptParam_Title", Languages.ListeCoutCoper.ToUpper());
        //        if (DonnesDatagrid.Count == 0)
        //            throw new Exception(Languages.AucuneDonneeAImprimer);
        //        var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleProduit, Languages.QuestionImpressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
        //        messageBox.OnMessageBoxClosed += (_, result) =>
        //        {
        //            if (messageBox.Result == MessageBoxResult.OK)
        //            {
        //                string key = Utility.getKey();
        //                var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
        //                service.EditerListeCoperDemandeCompleted += (snder, print) =>
        //                {
        //                    if (print.Cancelled || print.Error != null)
        //                    {
        //                        string error = print.Error.Message;
        //                        Message.ShowError(error, Languages.Coutcoper);
        //                    }
        //                    if (!print.Result)
        //                    {
        //                        Message.ShowError(Languages.ErreurImpressionDonnees, Languages.Coutcoper);
        //                    }
        //                    Utility.ActionImpressionDirect(SessionObject.CheminImpression, key, "CoutCoper", "Parametrage");
        //                };
        //               //service.EditerListeCoperDemandeAsync(key, dictionaryParam);
        //            }
        //            else
        //            {
        //                return;
        //            }
        //        };
        //        messageBox.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.Show(ex.Message, Languages.LibelleCaisse);
        //    }
        //}

       

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
                           List<CsMarque_Modele> lstModel = new List<CsMarque_Modele>();
                           lstModel.Add(new CsMarque_Modele()
                           {
                               MARQUE_ID = (DonnesDatagrid != null && DonnesDatagrid.Count != 0) ? DonnesDatagrid.Max(t => t.MARQUE_ID) + 1 : 1,
                               Libelle_Modele = this.txt_LibelleModele.Text
                           });
                           InsererModel(lstModel);
                       }
                   }
               };
               messageBox.Show();
        }
        private void InsererModel(List<CsMarque_Modele> leModel)
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.InsertModeleCompleted += (ssender, args) =>
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
                };
                client.InsertModeleAsync(leModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateModel(List<CsMarque_Modele> leModel)
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.UpdateModeleCompleted += (ssender, args) =>
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
                client.UpdateModeleAsync(leModel);
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
                            CsMarque_Modele leSelect = (CsMarque_Modele)this.dtgrdParametre.SelectedItem;
                            leSelect.Libelle_Modele = this.txt_LibelleModele.Text;
                            List<CsMarque_Modele> lstMar = new List<CsMarque_Modele>();
                            lstMar.Add(leSelect);
                            UpdateModel(lstMar);
                        }
                   }
               };
               messageBox.Show();
        }
    }
}


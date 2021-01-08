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
    public partial class UcListFourniture : ChildWindow
    {
        public ObjFOURNITURE ObjetSelectionne { get; set; }

        ObservableCollection<ObjFOURNITURE> donnesDatagrid = new ObservableCollection<ObjFOURNITURE>();
        string Namespace = "Galatee.Silverlight.Parametrage.";

        public UcListFourniture()
        {
            try
            {
                InitializeComponent();
                Translate();
                GetData();
                RemplirProduit();
                ChargerTypeDemande();
                this.DataContext = ObjetSelectionne;

                var ContextMenuItem = new List<ContextMenuItem>()
                 {
                    new ContextMenuItem(){ Code=Namespace+"UcFourniture",Label=Galatee.Silverlight.Resources.Langue.ContextMenuCreate,ModeExcecution=SessionObject.ExecMode.Creation,Title =Galatee.Silverlight.Resources.Langue.Ajout+ " " + Galatee.Silverlight.Resources.Parametrage.Languages.Fourniture },
                    new ContextMenuItem(){ Code=Namespace+"UcFourniture",Label=Galatee.Silverlight.Resources.Langue.ContextMenuConsult,ModeExcecution=SessionObject.ExecMode.Consultation,Title =Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Parametrage.Languages.Fourniture },
                    new ContextMenuItem(){ Code=Namespace+"UcFourniture",Label=Galatee.Silverlight.Resources.Langue.ContextMenuModify,ModeExcecution=SessionObject.ExecMode.Modification,Title =Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Parametrage.Languages.Fourniture },
                 };

                SessionObject.MenuContextuelItem = ContextMenuItem;

            }
            catch (Exception ex)
            {

                Message.Show(ex.Message, Languages.Fourniture);
            }

        }
        private void RemplirProduit()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllProduitCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.Produit);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    else
                    {
                        CboProduit.ItemsSource = args.Result;
                        CboProduit.SelectedValuePath = "PK_ID";
                        CboProduit.DisplayMemberPath = "LIBELLE";
                    }
                };
                client.SelectAllProduitAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerTypeDemande()
        {
            try
            {
                if (SessionObject.LstTypeDemande != null && SessionObject.LstTypeDemande.Count != 0)
                {
                    CboTypeDevis.ItemsSource = SessionObject.LstTypeDemande;
                    CboTypeDevis.SelectedValuePath = "PK_ID";
                    CboTypeDevis.DisplayMemberPath = "LIBELLE";
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneOptionDemandeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.LstTypeDemande = res.Result;
                    CboTypeDevis.ItemsSource = SessionObject.LstTypeDemande;
                    CboTypeDevis.SelectedValuePath = "PK_ID";
                    CboTypeDevis.DisplayMemberPath = "LIBELLE";
                };
                service1.RetourneOptionDemandeAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }
        private void Translate()
        {
            try
            {
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
        public ObservableCollection<ObjFOURNITURE> DonnesDatagrid
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
                client.SelectAllFournitureCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Fourniture);
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
                client.SelectAllFournitureAsync();
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
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleCaisse, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            if (dtgrdParametre.SelectedItem != null)
                            {
                                var selected = dtgrdParametre.SelectedItem as ObjFOURNITURE;

                                if (selected != null)
                                {
                                    ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                     Utility.EndPoint("Parametrage"));
                                    delete.DeleteFournitureCompleted += (del, argDel) =>
                                    {
                                        if (argDel.Cancelled || argDel.Error != null)
                                        {
                                            Message.ShowError(argDel.Error.Message, Languages.Fourniture);
                                            return;
                                        }

                                        if (argDel.Result == false)
                                        {
                                            Message.ShowError(Languages.ErreurSuppressionDonnees, Languages.Fourniture);
                                            return;
                                        }
                                        DonnesDatagrid.Remove(selected);
                                    };
                                    delete.DeleteFournitureAsync(selected);
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
                Message.Show(ex.Message, Languages.LibelleProduit);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            var dictionaryParam = new Dictionary<string, string>();
            try
            {
                dictionaryParam.Add("RptParam_CODE", Languages.Code.ToUpper());
                dictionaryParam.Add("RptParam_PRODUIT", Languages.LibelleProduit.ToUpper());
                dictionaryParam.Add("RptParam_LibelleDIAMETRE", Languages.LibelleDIAMETRE.ToUpper());
                dictionaryParam.Add("RptParam_DESIGNATION", Languages.lblDESIGNATION.ToUpper());
                dictionaryParam.Add("RptParam_DIAMETRE", Languages.LibelleDIAMETRE.ToUpper());
                dictionaryParam.Add("RptParam_QUANTITY", Languages.LibelleQUANTITY.ToUpper());
              
                dictionaryParam.Add("RptParam_DateCreation", Languages.DateCreation);
                dictionaryParam.Add("RptParam_DateModification", Languages.DateModification);
                dictionaryParam.Add("RptParam_UserCreation", Languages.UserCreation);
                dictionaryParam.Add("RptParam_UserModification", Languages.UserModification);
                dictionaryParam.Add("RptParam_Title", Languages.ListeCaisse.ToUpper());
                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeAImprimer);
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleProduit, Languages.QuestionImpressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        string key = Utility.getKey();
                        var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                        service.EditerListeFournitureCompleted += (snder, print) =>
                        {
                            if (print.Cancelled || print.Error != null)
                            {
                                string error = print.Error.Message;
                                Message.ShowError(error, Languages.LibelleProduit);
                            }
                            if (!print.Result)
                            {
                                Message.ShowError(Languages.ErreurImpressionDonnees, Languages.LibelleProduit);
                            }
                            Utility.ActionImpressionDirect(null, key, "Fourniture", "Parametrage");
                        };
                        //service.EditerListeCaisseAsync(key, dictionaryParam);
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
                Message.Show(ex.Message, Languages.Fourniture);
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
                Message.Show(ex.Message, Languages.Fourniture);
            }
        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as ObjFOURNITURE;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as ObjFOURNITURE;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Fourniture);
            }
        }

        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as ObjFOURNITURE;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as ObjFOURNITURE;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Fourniture);
            }
        }

        private void CboProduit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgrdParametre.ItemsSource != null)
            {
                List<ObjFOURNITURE> lst = new List<ObjFOURNITURE>();
                int idProduit =((CsProduit)CboProduit.SelectedItem).PK_ID;
                lst = ((ObservableCollection<ObjFOURNITURE>)DonnesDatagrid).Where(t => t.FK_IDPRODUIT == idProduit).ToList();
                dtgrdParametre.ItemsSource = null;
                dtgrdParametre.ItemsSource = lst;
            }
        }

        private void CboTypeDevis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgrdParametre.ItemsSource != null)
            {
                List<ObjFOURNITURE> lst = new List<ObjFOURNITURE>();
                int idTdem = ((ServiceAccueil.CsTdem )CboTypeDevis.SelectedItem).PK_ID;
                lst = ((ObservableCollection<ObjFOURNITURE>)DonnesDatagrid).Where(t => t.FK_IDTYPEDEMANDE ==idTdem ).ToList();
                dtgrdParametre.ItemsSource = null;
                dtgrdParametre.ItemsSource = lst;
            }
        }

        #region Sylla 11/06/2016
        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ParamLabel = Galatee.Silverlight.Resources.Langue.ContextMenuCreate;
                var ParamModeExcecution = SessionObject.ExecMode.Creation;
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Ajout + " " + Galatee.Silverlight.Resources.Parametrage.Languages.Fourniture;
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
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Parametrage.Languages.Fourniture;
                ShowWindows(ParamLabel, ParamModeExcecution, ParamTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowWindows(string ParamLabel, SessionObject.ExecMode ParamModeExcecution, string ParamTitle)
        {
            var contextMenuItem = new ContextMenuItem { Code = Namespace + "UcFourniture", Label = ParamLabel, ModeExcecution = ParamModeExcecution, Title = ParamTitle };
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
                var ParamTitle = Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Parametrage.Languages.Fourniture;
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


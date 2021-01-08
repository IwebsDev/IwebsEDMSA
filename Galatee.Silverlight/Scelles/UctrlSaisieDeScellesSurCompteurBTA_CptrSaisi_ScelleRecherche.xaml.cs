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
using Galatee.Silverlight.ServiceScelles;
using Galatee.Silverlight.Resources.Scelles;
using Galatee.Silverlight.Tarification.Helper;

namespace Galatee.Silverlight.Scelles
{
     

   public partial class UctrlSaisieDeScellesSurCompteurBTA_CptrSaisi_ScelleRecherche : ChildWindow, INotifyPropertyChanged
    {
       public CsCompteurBta ObjetSelectionne { get; set; }
       ObservableCollection<CsCompteurBta> donnesDatagrid = new ObservableCollection<CsCompteurBta>();
       List<CsCompteurBta> listForInsertOrUpdate = new List<CsCompteurBta>();
       List<CsCompteurBta> listForView = new List<CsCompteurBta>();

       public UctrlSaisieDeScellesSurCompteurBTA_CptrSaisi_ScelleRecherche()
        {
            try
            {
                InitializeComponent();
                Translate();
                GetData();
                this.DataContext = ObjetSelectionne;
              
                var Namespace = "Galatee.Silverlight.Scelles.";
                var ContextMenuItem = new List<ContextMenuItem>()
             { 
                new ContextMenuItem(){ Code=Namespace+"UcScelleCompteurBta",Label=Galatee.Silverlight.Resources.Langue.ContextMenuCreate,ModeExcecution=SessionObject.ExecMode.Creation,Title =Galatee.Silverlight.Resources.Langue.Ajout+ " " + Galatee.Silverlight.Resources.Scelles.Languages.LibelleReceptionScelle },
                new ContextMenuItem(){ Code=Namespace+"UcScelleCompteurBta",Label=Galatee.Silverlight.Resources.Langue.ContextMenuConsult,ModeExcecution=SessionObject.ExecMode.Consultation,Title =Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Scelles.Languages.LibelleReceptionScelle },
                new ContextMenuItem(){ Code=Namespace+"UcScelleCompteurBta",Label=Galatee.Silverlight.Resources.Langue.ContextMenuModify,ModeExcecution=SessionObject.ExecMode.Modification,Title =Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Scelles.Languages.LibelleReceptionScelle },
             };

                SessionObject.MenuContextuelItem = ContextMenuItem;
               
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleCodeDepart);
            }
        }
       private void RafraichirList(object sender, EventArgs e)
       {
           GetData();
       }

        private void Translate()
        {
            try
            {
            //    dtgrdParametre.Columns[0].Header = Languages.Code;
            //    dtgrdParametre.Columns[1].Header = Languages.Libelle;
            //    Title = Languages.LibelleCodeDepart;
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

        public ObservableCollection<CsCompteurBta> DonnesDatagrid
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
                //UcScelleCompteurBta ctrl = new UcScelleCompteurBta();
                //ctrl.Closed += new EventHandler(RafraichirList);
                //ctrl.Show();
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.SelectAllCompteurCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.LibelleReceptionScelle);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    DonnesDatagrid.Clear();
                    if (args.Result != null)
                        listForView = args.Result;
                        foreach (var item in args.Result)
                        {
                            //if (item.DATECREATION.Date == DateTime.Now.Date)
                            DonnesDatagrid.Add(item);
                        }
                    dtgrdParametre.ItemsSource = DonnesDatagrid;
                };
                client.SelectAllCompteurAsync();
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
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleReceptionScelle, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            if (dtgrdParametre.SelectedItem != null)
                            {
                                var selected = dtgrdParametre.SelectedItem as CsCompteurBta;

                                if (selected != null)
                                {
                                    IScelleServiceClient delete = new IScelleServiceClient(Utility.Protocole(),
                                                                                     Utility.EndPoint("Scelles"));
                                    delete.DeleteCompteurCompleted += (del, argDel) =>
                                    {
                                        if (argDel.Cancelled || argDel.Error != null)
                                        {
                                            Message.ShowError(argDel.Error.Message, Languages.LibelleReceptionScelle);
                                            return;
                                        }

                                        if (argDel.Result == false)
                                        {
                                            Message.ShowError(Languages.ErreurSuppressionDonnees, Languages.LibelleReceptionScelle);
                                            return;
                                        }
                                        DonnesDatagrid.Remove(selected);
                                    };
                                    delete.DeleteCompteurAsync(selected);
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
                Message.ShowError(ex.Message, Languages.LibelleCodeDepart);
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
                dictionaryParam.Add("RptParam_Title", Languages.ListeCodeDepart.ToUpper());

                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeAImprimer);
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleCodeDepart, Languages.QuestionImpressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        string key = Utility.getKey();
                        var service = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                        //service.EditerListeCodeDepartCompleted += (snder, print) =>
                        //{
                        //    if (print.Cancelled || print.Error != null)
                        //    {
                        //        Message.ShowError(print.Error.Message, Languages.LibelleCodeDepart);
                        //        return;
                        //    }
                        //    if (!print.Result)
                        //    {
                        //        Message.ShowError(Languages.ErreurImpressionDonnees, Languages.LibelleCodeDepart);
                        //        return;
                        //    }
                        //    Utility.ActionImpressionDirect(null, key, "CodeDepart", "Parametrage");
                        //};
                        //service.EditerListeCodeDepartAsync(key, dictionaryParam);
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
                Message.ShowError(ex.Message, Languages.LibelleCodeDepart);
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
                Message.ShowError(ex.Message, Languages.LibelleReceptionScelle);
            }
        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsCompteurBta;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsLotMagasinGeneral;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleReceptionScelle);
            }
        }

        void Newfrm_CallBack(object sender, CustumEventArgs e)
        {
            LoadDatagrid();
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            UcScelleCompteurBta Newfrm = new UcScelleCompteurBta((CsCompteurBta)dtgrdParametre.SelectedItem, SessionObject.ExecMode.Consultation, dtgrdParametre); ;
            Newfrm.CallBack += Newfrm_CallBack;
            Newfrm.Show();
        }
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UcScelleCompteurBta Newfrm = new UcScelleCompteurBta(); ;
                Newfrm.CallBack += Newfrm_CallBack;
                Newfrm.Show();
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                UcScelleCompteurBta Updatefrm = new UcScelleCompteurBta((CsCompteurBta)dtgrdParametre.SelectedItem, SessionObject.ExecMode.Modification, dtgrdParametre);
                Updatefrm.CallBack += Newfrm_CallBack;
                Updatefrm.Show();
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void LoadDatagrid()
        {
            GetData();
            //System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(listForView);
            //dtgrdParametre.ItemsSource = view;
            //datapager.Source = view;
        }


    }
}


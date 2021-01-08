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
     

   public partial class UcListAffectationCompteur : ChildWindow, INotifyPropertyChanged
    {
       public CsCompteurBta ObjetSelectionne { get; set; }
       ObservableCollection<CsCompteurBta> donnesDatagrid = new ObservableCollection<CsCompteurBta>();
       //List<CsRemiseScelles> listForInsertOrUpdate = new List<CsRemiseScelles>();
       //List<CsRemiseScelles> Listgrid = new List<CsRemiseScelles>();

      // UcAffectationCompteur Newfrm = new UcAffectationCompteur();
       #region EventHandling

       public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
       public event MethodeEventHandler CallBack;
       CustumEventArgs MyEventArg = new CustumEventArgs();

       protected virtual void OnEvent(CustumEventArgs e)
       {
           if (CallBack != null)
               CallBack(this, e);
       }

       #endregion
       public UcListAffectationCompteur()
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
                new ContextMenuItem(){ Code=Namespace+"UcListAffectationCompteur",Label=Galatee.Silverlight.Resources.Langue.ContextMenuCreate,ModeExcecution=SessionObject.ExecMode.Creation,Title =Galatee.Silverlight.Resources.Langue.Ajout+ " " + Galatee.Silverlight.Resources.Scelles.Languages.LibelleReceptionScelle },
                new ContextMenuItem(){ Code=Namespace+"UcListAffectationCompteur",Label=Galatee.Silverlight.Resources.Langue.ContextMenuConsult,ModeExcecution=SessionObject.ExecMode.Consultation,Title =Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Scelles.Languages.LibelleReceptionScelle },
                new ContextMenuItem(){ Code=Namespace+"UcListAffectationCompteur",Label=Galatee.Silverlight.Resources.Langue.ContextMenuModify,ModeExcecution=SessionObject.ExecMode.Modification,Title =Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Scelles.Languages.LibelleReceptionScelle },
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
                //dtgrdScelle.Columns[0].Header = Languages.Code;
                //dtgrdScelle.Columns[1].Header = Languages.Libelle;
                //Title = Languages.LibelleCodeDepart;
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
                //UcRemisesScelles ctrl = new UcRemisesScelles();
                //ctrl.Closed += new EventHandler(RafraichirList);
                //ctrl.Show();
                IScelleServiceClient client = new IScelleServiceClient(Utility.Protocole(), Utility.EndPoint("Scelles"));
                client.SelectAllMargasinVirtuelleCompleted += (ssender, args) =>
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
                        foreach (var item in args.Result)
                        {
                            //if (item.Date_Remise == DateTime.Now.Date)
                            DonnesDatagrid.Add(item);
                        }
                    dtgrdScelle.ItemsSource = DonnesDatagrid;
                };
                client.SelectAllMargasinVirtuelleAsync();
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
                if (dtgrdScelle.SelectedItems.Count > 0)
                {
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.LibelleReceptionScelle, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            if (dtgrdScelle.SelectedItem != null)
                            {
                                var selected = dtgrdScelle.SelectedItem as CsCompteurBta;

                                if (selected != null)
                                {
                                    IScelleServiceClient delete = new IScelleServiceClient(Utility.Protocole(),
                                                                                     Utility.EndPoint("Scelles"));
                                    delete.DeleteMargasinVirtuelleCompleted += (del, argDel) =>
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
                                    delete.DeleteMargasinVirtuelleAsync(selected);
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
                        var service = new IScelleServiceClient(Utility.Protocole(), Utility.EndPoint("Scelles"));
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

        private void dtgrdScelle_DoubleClick(object sender, MouseButtonEventArgs e)
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

        private void dtgrdScelle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dtgrdScelle_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdScelle.SelectedItem as CsCompteurBta;
                SessionObject.objectSelected = dtgrdScelle.SelectedItem as CsCompteurBta;
                SessionObject.gridUtilisateur = dtgrdScelle;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleReceptionScelle);
            }
        }
       

        void Newfrm_CallBack(object sender, CustumEventArgs e)
        {
            
            GetData();
            //LoadDatagrid();
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            //UcRemisesScelles Newfrm = new UcRemisesScelles((CsLotMagasinGeneral)dtgrdScelle.SelectedItem, SessionObject.ExecMode.Consultation, dtgrdScelle); ;
            //Newfrm.CallBack += Newfrm_CallBack;
            //Newfrm.Show();
        }
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UcAffectationCompteur Newfrm = new UcAffectationCompteur(); ;
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

            //    UcRemisesScelles Updatefrm = new UcRemisesScelles((CsLotMagasinGeneral)dtgrdScelle.SelectedItem, SessionObject.ExecMode.Modification, dtgrdScelle);
            //Updatefrm.CallBack += Newfrm_CallBack;
            //Updatefrm.Show();
            }
            catch (Exception)
            {
                
                throw;
            }
        }




    }
}


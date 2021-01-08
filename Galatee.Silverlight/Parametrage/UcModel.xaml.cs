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
using Galatee.Silverlight.Library;
using Galatee.Silverlight.ServiceParametrage;
using Galatee.Silverlight.Resources.Parametrage;


namespace Galatee.Silverlight.Parametrage
{
    public partial class UcModel : ChildWindow
    {
        List<CsMarque_Modele> ListdesModelesfonctMarq = new List<CsMarque_Modele>();
        private CsMarque_Modele ObjetSelectionnee = null;
        static int ID_MArque, nbr_capot, nb_cache;

        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        List<CsMarque_Modele> listForInsertOrUpdate = new List<CsMarque_Modele>();
        public UcModel()
        {
            InitializeComponent();
            RemplirListeCmbDesModelesMarqueExistant();
            ModeExecution = SessionObject.ExecMode.Creation;
        }
        public UcModel(CsMarque_Modele pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                var MarqueModele = new CsMarque_Modele();
                if (pObject != null)
                    ObjetSelectionnee = Utility.ParseObject(MarqueModele, pObject as CsMarque_Modele);
                ModeExecution = pExecMode;
                dataGrid = pGrid;
                // dtgrdlReceptionScelle.ItemsSource = donnesDatagrid;
                RemplirListeCmbDesModelesMarqueExistant();
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                      
                        List<CsMarque_Modele> lstMaqmMdt = ListdesModelesfonctMarq;
                        if (lstMaqmMdt != null)
                        {
                            //Cbo_Modele.SelectedItem = lstMaqmMdt.FirstOrDefault(t => t.MODELE_ID  == ObjetSelectionnee.MODELE_ID);
                            //Cbo_Marque.SelectedItem = lstMaqmMdt.FirstOrDefault(t => t.MARQUE_ID == ObjetSelectionnee.MARQUE_ID);
                        }
                        txt_NombreScelleCache.Text = ObjetSelectionnee.Nbre_scel_cache.ToString();
                        txt_NombreScelleCache.Text = ObjetSelectionnee.Nbre_scel_capot.ToString();
                    }
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
            }
        }

        private void RemplirListeCmbDesModelesMarqueExistant()
        {
            try
            {
                Galatee.Silverlight.ServiceParametrage.ParametrageClient client = new Galatee.Silverlight.ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllMarqueModeleCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Quartier);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees,Galatee.Silverlight.Resources.Scelles.Languages.Scelles);
                        return;
                    }
                    else
                    {
                        ListdesModelesfonctMarq = args.Result;

                    }
                };
                client.SelectAllMarqueModeleAsync();
            }
            catch (Exception ex)
            {
                throw ex;
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
                        var service = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                        if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        {
                            
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                            {
                                service.InsertMarqueModeleCompleted += (snder, insertR) =>
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
                                    //UpdateParentList(listForInsertOrUpdate[0]);
                                };
                                service.InsertMarqueModeleAsync(listForInsertOrUpdate);
                               }
                           
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateMarqueModeleCompleted += (snder, UpdateR) =>
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
                                };
                                service.UpdateMarqueModeleAsync(listForInsertOrUpdate);
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


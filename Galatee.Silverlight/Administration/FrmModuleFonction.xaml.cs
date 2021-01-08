using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.ServiceAdministration;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Classes;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Resources;
using UpdateControls.XAML;
using Galatee.Silverlight.Library.Models;
using Galatee.Silverlight.Library.ViewModels;

namespace Galatee.Silverlight.Administration
{
    public partial class FrmModuleFonction : ChildWindow
    {
        public FrmModuleFonction()
        {
            InitializeComponent();
            DataContext = ForView.Wrap(new DesignTimeData().Root);
            GetData();
        }

        ObservableCollection<CsModule> _ListeModuleObs ;
        List<CsModule> ListeModule = new List<CsModule>();
        List<CsModule> ListeModuleSauvegarde = new List<CsModule>();


        void ValidaterAttributionProfil()
        {

            List<CsModuleDeFonction> lesModuleHabile = new List<CsModuleDeFonction>();
            List<CsModule> lesModuleSelect = _ListeModuleObs.Where(t => t.IsSelect == true).ToList();
            CsFonction laFonctionSelect = (CsFonction)this.cbo_profile.SelectedItem;
            foreach (CsModule item in lesModuleSelect)
            {
                CsModuleDeFonction lesModuleDeLaFonction = new CsModuleDeFonction() 
                {
                    FK_IDFONCTION = laFonctionSelect.PK_ID ,
                     FK_IDMODULE = item.PK_ID 
                };
                lesModuleHabile.Add(lesModuleDeLaFonction);
            }

            //AdministrationServiceClient prgram = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            AdministrationServiceClient insertHabil = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            insertHabil.InsertionModuleDeFonctionCompleted += (insers, resultins) =>
            {
                try
                {
                    if (resultins.Cancelled || resultins.Error != null)
                    {
                        string error = resultins.Error.Message;
                        Message.ShowInformation(error, Langue.errorTitle);
                        OKButton.IsEnabled = true;
                        return;
                    }

                    if (resultins.Result == false)
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Administration.Langue.MsgSettingProfilFailed, Langue.informationTitle);
                        return;
                    }
                    else
                        if (resultins.Result == true)
                        {
                            Message.ShowInformation(Galatee.Silverlight.Resources.Administration.Langue.MsgSettingProfilSuccess, Langue.informationTitle);
                            return;
                        }
                }
                catch (Exception ex)
                {
                    OKButton.IsEnabled = true;
                    Message.ShowError(ex.Message, Langue.informationTitle);
                }
            };
            insertHabil.InsertionModuleDeFonctionAsync(lesModuleHabile);
            
            //prgram.InsertionModuleDeFonctionCompleted += (sprog, resprog) =>
            //{
            //    //InsertionModuleDeFonction(lesModuleHabile);
            //};
            //prgram.InsertionModuleDeFonctionAsync();


        }

        void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OKButton.IsEnabled = false;
                var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Langue.ConfirmationTitle, Galatee.Silverlight.Resources.Administration.Langue.confirmHabilitationMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                w.OnMessageBoxClosed += (_, result) =>
                {
                    if (w.Result == MessageBoxResult.OK)
                        ValidaterAttributionProfil();
                    else
                        OKButton.IsEnabled = true;
                };
                w.Show();
            }
            catch (Exception ex)
            {
                OKButton.IsEnabled = true;
                Message.ShowError(ex, Langue.informationTitle);
            }

        }

     
        void RetourneProgramme()
        {
            //Obtenir les donnees de l'arborescence des modules , programmes et des menus relatifs
            AdministrationServiceClient prgram = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            prgram.GetListeDesModuleCompleted += (sprog, resprog) =>
            {
                try
                {
                    if (resprog.Result == null || resprog.Result.Count == 0)
                    {
                        Message.ShowInformation(Langue.msgNodata, Langue.informationTitle);
                        return;
                    }
                    ListeModule.AddRange(resprog.Result);
                    ListeModuleSauvegarde = Shared.ClasseMEthodeGenerique.RetourneListCopy<CsModule>(ListeModule);
                    _ListeModuleObs = new ObservableCollection<CsModule>();
                    foreach (CsModule item in ListeModule)
                        _ListeModuleObs.Add(item);

                    dtg_Module.ItemsSource = null;
                    dtg_Module.ItemsSource = _ListeModuleObs.OrderBy(t => t.LIBELLE);
                    OKButton.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Langue.errorTitle);
                }

            };
            prgram.GetListeDesModuleAsync();
        
        }

        void GetData()
        {
            // desactivation du treeview des modules
            OKButton.IsEnabled = false;

            try
            {
                    AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                    client.SELECT_All_FonctionCompleted += (ss, res) =>
                    {
                        try
                        {
                            if (res.Cancelled || res.Error != null)
                            {
                                string error = res.Error.Message;
                                Message.ShowError(error, Langue.errorTitle);
                                return;
                            }

                            if (res.Result == null || res.Result.Count == 0)
                            {
                                Message.ShowInformation(Langue.msgNodata, Langue.informationTitle);
                                return;
                            }
                            SessionObject.ListeFonction = res.Result;
                            //DonnnesProfils.AddRange(res.Result);
                            cbo_profile.ItemsSource = res.Result.OrderBy(t => t.ROLENAME);
                            cbo_profile.SelectedValuePath = "CODE";
                            cbo_profile.DisplayMemberPath = "ROLENAME";
                            RetourneProgramme();

                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex.Message, Langue.informationTitle);
                        }

                    };
                    client.SELECT_All_FonctionAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }

        void cbo_profile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbo_profile.SelectedItem != null)
                {
                    _ListeModuleObs = new ObservableCollection<CsModule>();
                    CsFonction laFonction = (CsFonction)cbo_profile.SelectedItem;
                    foreach (CsModule item in ListeModuleSauvegarde)
                    {
                        item.IsSelect = false;
                        _ListeModuleObs.Add(item);
                        List<CsModuleDeFonction> lstModuleFonc = item.lstFonction.Where(t => t.FK_IDFONCTION == laFonction.PK_ID).ToList();
                        if (lstModuleFonc != null && lstModuleFonc.Count != 0)
                            item.IsSelect = true;
                    }
                    dtg_Module.ItemsSource = null;
                    dtg_Module.ItemsSource = _ListeModuleObs;
                    OKButton.IsEnabled = true;
                }

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.errorTitle);
            }
        }


         void cbo_profile_DropDownClosed(object sender, EventArgs e)
        {
            //CheckTreeviewParProfil();
        }

        private void dtg_Module_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtg_Module.SelectedItem != null && this.cbo_profile.SelectedItem != null )
            {
                List<CsModule> lesModule = ((ObservableCollection <CsModule>)dtg_Module.ItemsSource).ToList() ;
                lesModule.ForEach(t => t.IsSelect = false);

                CsModule leModuleSelect = (CsModule)dtg_Module.SelectedItem;
                if (leModuleSelect.IsSelect == true)
                    leModuleSelect.IsSelect = false;
                else
                    leModuleSelect.IsSelect = true;
            }
        }

       
      
   
       
    }
}


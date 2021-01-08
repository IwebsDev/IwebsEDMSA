using Galatee.Silverlight.Library.Models;
using Galatee.Silverlight.Library.ViewModels;
using Galatee.Silverlight.ServiceAdministration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using UpdateControls.XAML;

namespace Galatee.Silverlight.Administration
{
    public partial class FrmEditerHabilitationProfil : ChildWindow
    {


        public FrmEditerHabilitationProfil()
        {
            InitializeComponent();
            InitializeComponent();
            DataContext = ForView.Wrap(new DesignTimeData().Root);
            GetData();
        }
        bool isProfilSelected = false;
        List<CsFonction> DonnnesProfils = new List<CsFonction>();
        List<string> idMenuRecurv = new List<string>();
        List<string> idMenu = new List<string>();
        List<string> idProgram = new List<string>();
        List<string> idModule = new List<string>();
        List<CsProgramMenu> ListeProgramMenu = new List<CsProgramMenu>();
        List<CsHabilitationProgram> CurrentProfilProgramMenu = new List<CsHabilitationProgram>();
        ObservableCollection<Module> modules = null;
        CsFonction ta = null;// profil selectionne
        OptionViewModel viewModel = null;

        void DesactiverControle(bool status)
        {
            cbo_profile.IsEnabled = status;
        }

       
        void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    if (cbo_profile.SelectedItem != null)
                        ImpressionHabilitation(((CsFonction)cbo_profile.SelectedItem).CODE);
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "Erreur de donnée");
                }
            }
            catch (Exception ex)
            {
                OKButton.IsEnabled = true;
                Message.ShowError(ex, "Information");
            }

        }

        List<CsHabilitationMetier> lstAImprimer = new List<CsHabilitationMetier>();
        private void ImpressionHabilitation(string code)
        {
            AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            client.RetourneFonctionProfilCentreCompleted += (ss, res) =>
            {
                try
                {
                    if (res.Cancelled || res.Error != null)
                    {
                        string error = res.Error.Message;
                        Message.ShowError(error, "Erreur de donnée");
                        return;
                    }

                    if (res.Result == null || res.Result.Count == 0)
                    {
                        Message.ShowInformation("Aucune données trouvé", "Information");
                        return;
                    }
                    lstAImprimer = res.Result;
                    Galatee.Silverlight.Shared.FrmOptionEditon ctrl = new Shared.FrmOptionEditon();
                    ctrl.Closed += ctrl_Closed;
                    //this.IsEnabled = false;
                    ctrl.Show();
                    //Effectue l'aperçcu avant imprèssion
                    //Utility.ActionDirectOrientation<ServicePrintings.CsHabilitationMetier, ServiceAdministration.CsHabilitationMetier>(res.Result, param, SessionObject.DefaultPrinter, "ReportHabillitationMetier", "Administration", false);
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "Information");
                }

            };
            client.RetourneFonctionProfilCentreAsync(code);
        }

        void ctrl_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true ;
            Galatee.Silverlight.Shared.FrmOptionEditon ctrs = sender as Galatee.Silverlight.Shared.FrmOptionEditon;
            if (ctrs.IsOptionChoisi)
            {
                //if (ctrs.OptionSelect == SessionObject.EnvoiPrinter )
                //    Utility.ActionImpressionPdf<ServicePrintings.CsHabilitationMetier, ServiceAdministration.CsHabilitationMetier>(lstAImprimer, null, "ReportHabillitationMetier", "Administration");
                //else if (ctrs.OptionSelect == SessionObject.EnvoiExecl)
                //    Utility.ActionExportation<ServicePrintings.CsHabilitationMetier, ServiceAdministration.CsHabilitationMetier>(lstAImprimer, null, "SuivieMoratoire", "Recouvrement", "xlsx");
                //else if (ctrs.OptionSelect == SessionObject.EnvoiWord )
                //    Utility.ActionExportation<ServicePrintings.CsHabilitationMetier, ServiceAdministration.CsHabilitationMetier>(lstAImprimer, null, "SuivieMoratoire", "Recouvrement", "doc");
                //else if (ctrs.OptionSelect == SessionObject.EnvoiPdf )
                //    Utility.ActionExportation<ServicePrintings.CsHabilitationMetier, ServiceAdministration.CsHabilitationMetier>(lstAImprimer, null, "SuivieMoratoire", "Recouvrement", "pdf");
            }
        }

        
       
        void GetData()
        {
            // desactivation du treeview des modules
            

            try
            {
                if (SessionObject.ListeFonction != null && SessionObject.ListeFonction.Count != 0)
                {
                    DonnnesProfils.Add(new CsFonction() { CODE = null, ROLENAME = "Tous les métiers" });
                    DonnnesProfils.AddRange( SessionObject.ListeFonction);


                    cbo_profile.ItemsSource = DonnnesProfils;
                    cbo_profile.SelectedValuePath = "CODE";
                    cbo_profile.DisplayMemberPath = "ROLENAME";

                    
                }
                else
                {
                    AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                    client.SELECT_All_FonctionCompleted += (ss, res) =>
                    {
                        try
                        {
                            if (res.Cancelled || res.Error != null)
                            {
                                string error = res.Error.Message;
                                Message.ShowError(error, "Erreur de donnée");
                                return;
                            }

                            if (res.Result == null || res.Result.Count == 0)
                            {
                                Message.ShowInformation("Aucune données trouvé", "Information");
                                return;
                            }
                            SessionObject.ListeFonction = res.Result;
                            DonnnesProfils.Add(new CsFonction() { CODE = null, ROLENAME = "Tous les métiers" });
                            DonnnesProfils.AddRange(res.Result);
                            cbo_profile.ItemsSource = DonnnesProfils;
                            cbo_profile.SelectedValuePath = "CODE";
                            cbo_profile.DisplayMemberPath = "ROLENAME";

                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex.Message, "Information");
                        }

                    };
                    client.SELECT_All_FonctionAsync();
                }
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

        private void cbo_profile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

       

    }
}


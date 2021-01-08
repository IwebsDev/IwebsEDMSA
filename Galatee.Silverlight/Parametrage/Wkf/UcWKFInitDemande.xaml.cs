using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceParametrage;
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
using Galatee.Silverlight;
using Galatee.Workflow.Interfaces;

namespace Galatee.Silverlight.Parametrage
{
    public partial class UcWKFInitDemande : ChildWindow
    {

        #region Membres

        List<CsVwConfigurationWorkflowCentre> _lsConfig;
        string IDLigne;

        #endregion

        public UcWKFInitDemande()
        {
            try
            {
                InitializeComponent();
                Translate();

                OKButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Initier une demande");
            }
        }

        #region Fonctions

        void Translate()
        {
            CancelButton.Content = Languages.Annuler;
        }

        void GetData()
        {
            //Récupération des configurations en fonction du centre de l'utilisateur connecté
            bool isAdmin = (UserConnecte.matricule == "admin");

            ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            int back = LoadingManager.BeginLoading("Chargement des données ...");
            client.SelectAllConfigurationWorkflowCentreCompleted += (sender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.ListeCodePoste);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    cmbConfiguration.Items.Clear();
                    _lsConfig = new List<CsVwConfigurationWorkflowCentre>();
                    _lsConfig.Add(new CsVwConfigurationWorkflowCentre() { NOM = "Aucun", OPERATIONID = Guid.Empty });
                    if (args.Result != null)
                        foreach (var item in args.Result.Distinct(new Classes.GenericComparer<CsVwConfigurationWorkflowCentre>(
                            x => x.OPERATIONID)))
                        {
                            _lsConfig.Add(item);
                        }
                    cmbConfiguration.DisplayMemberPath = "NOM";
                    cmbConfiguration.SelectedValuePath = "OPERATIONID";
                    cmbConfiguration.ItemsSource = _lsConfig;

                    LoadingManager.EndLoading(back);
                };
            client.SelectAllConfigurationWorkflowCentreAsync();
        }

        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnCharger_Click(object sender, RoutedEventArgs e)
        {
            //Chargement de la fenêtre
            if (null != _lsConfig && _lsConfig.Count > 0 && null != cmbConfiguration.SelectedValue)
            {
                Guid laSelection = Guid.Parse(cmbConfiguration.SelectedValue.ToString());

                var Config = _lsConfig.Where(cfg => cfg.OPERATIONID == laSelection).FirstOrDefault();
                if (null != Config)
                {
                    IWKFInitForm uctl = null;
                    if (Config.FORMULAIRE != null && Config.FORMULAIRE != string.Empty && 0 != Config.FK_IDFORMULAIRE)
                    {
                        Type t = Type.GetType(Config.FORMULAIRE);
                        uctl = Activator.CreateInstance(t) as IWKFInitForm;
                    }
                    if (null != uctl)
                    {
                        TabItem tabItem1 = new TabItem();
                        tabItem1.Header = uctl.Title;                        
                        tabItem1.Content = uctl;
                        //tabControl1.Items.Add(tabItem1);
                        //tabControl1.SelectedItem = tabItem1;                        

                        //Création de l'event
                        uctl.ExecutionOkButtonCompleted += uctl_ExecutionOkButtonCompleted;
                    }
                }
            }
        }

        void uctl_ExecutionOkButtonCompleted(object sender, WorkflowManager.ExecutionOkButtonEventArgs e)
        {
            //On récupère les informations
            this.OKButton.IsEnabled = e.ExecutionOk;    //On active le bouton si tout c'est bien passé
        }
    }
}


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
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.ServiceWorkflow;

namespace Galatee.Silverlight.Workflow
{
    public partial class UcWKFMotifRejet : ChildWindow
    {

        DemandeWorkflowInformation _laDemande;
        Galatee.Silverlight.ServiceAccueil.CsInfoDemandeWorkflow _infoDemande;

        public UcWKFMotifRejet(DemandeWorkflowInformation dmdInfo)
        {
            InitializeComponent();
            _laDemande = dmdInfo;

            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            LblChargement.Visibility = System.Windows.Visibility.Collapsed;
        }
        ChildWindow ParentG = new ChildWindow();
        public UcWKFMotifRejet(Galatee.Silverlight.ServiceAccueil.CsInfoDemandeWorkflow dmdInfo, ChildWindow Parent)
        {
            InitializeComponent();
            _infoDemande = new ServiceAccueil.CsInfoDemandeWorkflow();
            _infoDemande = dmdInfo;
            ParentG = Parent;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            LblChargement.Visibility = System.Windows.Visibility.Collapsed;
            txtCodeDemande.Text = dmdInfo.CODE_DEMANDE_TABLE_TRAVAIL ;
        }
        public UcWKFMotifRejet(Galatee.Silverlight.ServiceAccueil.CsInfoDemandeWorkflow dmdInfo)
        {
            InitializeComponent();
            _infoDemande = new ServiceAccueil.CsInfoDemandeWorkflow();
            _infoDemande = dmdInfo;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            LblChargement.Visibility = System.Windows.Visibility.Collapsed;
            txtCodeDemande.Text = dmdInfo.CODE_DEMANDE_TABLE_TRAVAIL;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //Quand on fait Ok, on rejette en même temps la demande
            if (null != _infoDemande && string.Empty != txtMotif.Text)
            {
                RejeterDemande(_infoDemande);
            }
            else if (string.Empty == txtMotif.Text)
            {
                Message.ShowError("Veuillez entrer le motif du rejet de la demande", "Rejet demande");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void txtCodeDemande_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        void RejeterDemande(Galatee.Silverlight.ServiceAccueil.CsInfoDemandeWorkflow dmdInfo)
        {
            prgBar.Visibility = System.Windows.Visibility.Visible;
            LblChargement.Visibility = System.Windows.Visibility.Visible;
            OKButton.IsEnabled = false;
            CancelButton.IsEnabled = false;

            WorkflowClient client = new WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
            client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
            client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
            client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);
            client.ExecuterActionSurDemandeCompleted += (sender, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                    LblChargement.Visibility = System.Windows.Visibility.Collapsed;
                    OKButton.IsEnabled = true;
                    CancelButton.IsEnabled = true;

                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, "Rejet demande");
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, "Rejet demande");
                        return;
                    }
                    if (args.Result.StartsWith("ERR"))
                    {
                        Message.ShowError(args.Result, "Rejet demande");
                    }
                    else
                    {
                        Message.ShowInformation(args.Result, "Rejet demande");
                        ParentG.Close();
                        this.DialogResult = true;
                    }
                };
            client.ExecuterActionSurDemandeAsync(null != dmdInfo ? dmdInfo.CODE : _infoDemande.CODE, SessionObject.Enumere.REJETER, UserConnecte.matricule, txtMotif.Text);            
        }

    }
}


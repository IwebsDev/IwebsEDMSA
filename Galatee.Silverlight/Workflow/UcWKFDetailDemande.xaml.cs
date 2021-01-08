using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceAdministration;
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
using Galatee.Silverlight.ServiceWorkflow;

namespace Galatee.Silverlight.Workflow
{
    public partial class UcWKFDetailDemande : ChildWindow
    {
        DemandeWorkflowInformation _LaDemande;
        string NomOperation = string.Empty;
        string _CodeDemande = string.Empty;
        Galatee.Silverlight.ServiceAdministration.CsUtilisateur leGarQuiAInitie;

        public UcWKFDetailDemande(string codeDemande)
        {
            try
            {
                InitializeComponent();
                Translate();
                AfficherOuCacherChargement(false);
                _CodeDemande = codeDemande;

                GetData();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Détail");
            }
        }

        public UcWKFDetailDemande(string codeDemande, string _operation)
        {
            try
            {
                InitializeComponent();
                Translate();
                AfficherOuCacherChargement(false);
                _CodeDemande = codeDemande;
                NomOperation = _operation;

                GetData();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Détail");
            }
        }


        public UcWKFDetailDemande(DemandeWorkflowInformation InfoDemande)
        {
            try
            {
                InitializeComponent();
                Translate();
                AfficherOuCacherChargement(false);
                _LaDemande = InfoDemande;

                ChercherInfoUtilisateur();
                ShowDetails();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Détail");
            }
        }

        public UcWKFDetailDemande(DemandeWorkflowInformation InfoDemande, string _operation)
        {
            try
            {
                InitializeComponent();
                Translate();
                AfficherOuCacherChargement(false);
                _LaDemande = InfoDemande;
                NomOperation = _operation;

                ChercherInfoUtilisateur();
                ShowDetails();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Détail");
            }
        }


        #region Fonctions

        void Translate()
        {
            CancelButton.Content = Languages.btnFermer;
        }

        void AfficherOuCacherChargement(bool opt)
        {
            if (opt)
            {
                LblChargement.Visibility = System.Windows.Visibility.Visible;
                prgBar.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                LblChargement.Visibility = System.Windows.Visibility.Collapsed;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        void ShowDetails()
        {
            if (null != _LaDemande)
            {
                txtCodeDemande.Text = _LaDemande.CODE;
                txtDateCreation.Text = (_LaDemande.DATECREATION.HasValue) ? _LaDemande.DATECREATION.Value.ToShortDateString()
                    : string.Empty;
                txtOperation.Text = NomOperation;
                if (null != leGarQuiAInitie) txtNomUtilisateur.Text = leGarQuiAInitie.LIBELLE;
            }
        }

        void GetData()
        {
            //On recherche les information sur la demande
            ServiceParametrage.ParametrageClient client = new ServiceParametrage.ParametrageClient(Utility.Protocole(),
                Utility.EndPoint("Parametrage"));
            AfficherOuCacherChargement(true);
            client.SelectInformationDemandeCompleted += (sender, args) =>
            {
                AfficherOuCacherChargement(false);
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

                var theAsk = args.Result;
                _LaDemande = new DemandeWorkflowInformation()
                {
                    ALERTE = theAsk.ALERTE,
                    ALLCENTRE = theAsk.ALLCENTRE,
                    CODE = theAsk.CODE,
                    CODECENTRE = theAsk.CODECENTRE,
                    CODESITE = theAsk.CODESITE,
                    CONTROLEETAPE = theAsk.CONTROLEETAPE,
                    DATECREATION = theAsk.DATECREATION,
                    COMBINAISON_FKETAPE_FKOPERATION = string.Empty,
                    DATEDERNIEREMODIFICATION = theAsk.DATEDERNIEREMODIFICATION,
                    DUREE = theAsk.DUREE,
                    ETAPEPRECEDENTE = theAsk.ETAPEPRECEDENTE,
                    FK_IDCENTRE = theAsk.FK_IDCENTRE,
                    FK_IDETAPE = theAsk.FK_IDETAPE,
                    FK_IDETAPEACTUELLE = theAsk.FK_IDETAPEACTUELLE,
                    FK_IDGROUPEVALIDATIOIN = theAsk.FK_IDGROUPEVALIDATIOIN,
                    FK_IDMENU = theAsk.FK_IDMENU,
                    FK_IDOPERATION = theAsk.FK_IDOPERATION,
                    FK_IDSTATUS = theAsk.FK_IDSTATUS,
                    FK_IDWORKFLOW = theAsk.FK_IDWORKFLOW,
                    IDCENTRE = theAsk.IDCENTRE,
                    IDCIRCUIT = theAsk.IDCIRCUIT,
                    IDETAPE = theAsk.IDETAPE,
                    IDSITE = theAsk.IDSITE,
                    ORDRE = theAsk.ORDRE,
                    LIBELLECENTRE = theAsk.LIBELLECENTRE,
                    LIBELLESITE = theAsk.LIBELLESITE,
                    MATRICULEUSERCREATION = theAsk.MATRICULEUSERCREATION,
                    NOM = theAsk.NOM,
                    NOMOPERATION = string.Empty,
                    STATUT = EnumStatutDemande.RecupererLibelleStatutDemande((WorkflowManager.STATUSDEMANDE)theAsk.FK_IDSTATUS)
                };
            };
            client.SelectInformationDemandeAsync(_CodeDemande);
        }

        void ChercherInfoUtilisateur()
        {
            //On recherche maintenant les infos du gars qui a initiée la demande
            AdministrationServiceClient adminClient = new AdministrationServiceClient(Utility.Protocole(),
                Utility.EndPoint("Administration"));
            AfficherOuCacherChargement(true);
            adminClient.RetourneListeAllUserCompleted += (usender, uargs) =>
            {
                AfficherOuCacherChargement(false);
                if (uargs.Cancelled || uargs.Error != null)
                {
                    string error = uargs.Error.Message;
                    Message.Show(error, Languages.ListeCodePoste);
                    return;
                }
                if (uargs.Result == null)
                {
                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                    return;
                }

                leGarQuiAInitie = uargs.Result.Where(u => u.MATRICULE == _LaDemande.MATRICULEUSERCREATION)
                    .FirstOrDefault();
            };
            adminClient.RetourneListeAllUserAsync();
        }

        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //Transmettre la demande            
            if (null != _LaDemande)
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                LblChargement.Visibility = System.Windows.Visibility.Visible;
                OKButton.IsEnabled = false;
                RejeterButton.IsEnabled = false;

                WorkflowClient client = new WorkflowClient();
                client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
                client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
                client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);

                client.ExecuterActionSurDemandeCompleted += (ssender, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                    LblChargement.Visibility = System.Windows.Visibility.Collapsed;
                    OKButton.IsEnabled = true;
                    RejeterButton.IsEnabled = true;

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
                    if (args.Result.StartsWith("ERR"))
                    {
                        Message.ShowError(args.Result, Languages.Parametrage);
                    }
                    else
                    {
                        Message.ShowInformation(args.Result, Languages.Parametrage);
                        this.DialogResult = true;
                    }
                };
                client.ExecuterActionSurDemandeAsync(_CodeDemande, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule, string.Empty);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void RejeterButton_Click(object sender, RoutedEventArgs e)
        {
            //Transmettre la demande            
            if (null != _LaDemande)
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                LblChargement.Visibility = System.Windows.Visibility.Visible;
                OKButton.IsEnabled = false;
                RejeterButton.IsEnabled = false;

                WorkflowClient client = new WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
                client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
                client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
                client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);

                client.ExecuterActionSurDemandeCompleted += (ssender, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                    LblChargement.Visibility = System.Windows.Visibility.Collapsed;
                    OKButton.IsEnabled = true;
                    RejeterButton.IsEnabled = true;

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
                    if (args.Result.StartsWith("ERR"))
                    {
                        Message.ShowError(args.Result, Languages.Parametrage);
                    }
                    else
                    {
                        Message.ShowInformation(args.Result, Languages.Parametrage);
                        this.DialogResult = true;
                    }
                };
                client.ExecuterActionSurDemandeAsync(_CodeDemande, SessionObject.Enumere.REJETER, UserConnecte.matricule, string.Empty);
            }
        }
    }
}


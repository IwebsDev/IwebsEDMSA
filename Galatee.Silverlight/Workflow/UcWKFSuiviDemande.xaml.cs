using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceAdministration;
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

namespace Galatee.Silverlight.Workflow
{
    public partial class UcWKFSuiviDemande : ChildWindow
    {

        DemandeWorkflowInformation _LaDemande;
        string NomOperation = string.Empty;
        string _CodeDemande = string.Empty;
        CsUtilisateur leGarQuiAInitie;
        List<CsJournalDemandeWorkflow> leJournal;

        public UcWKFSuiviDemande()
        {
            try
            {
                InitializeComponent();
                AfficherOuCacherChargement(false);

                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Suivi de demande");
            }
        }

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
            }
        }

        void GetData()
        {
            //On recherche les information sur la demande
            ServiceParametrage.ParametrageClient client = new ServiceParametrage.ParametrageClient(Utility.Protocole(),
                Utility.EndPoint("Parametrage"));
            _CodeDemande = txtCodeDemande.Text;

            AfficherOuCacherChargement(true);
            client.SelectInformationDemandeCompleted += (sender, args) =>
            {
                
                if (args.Cancelled || args.Error != null)
                {
                    AfficherOuCacherChargement(false);
                    string error = args.Error.Message;
                    Message.Show(error, Languages.ListeCodePoste);
                    return;
                }
                if (args.Result == null)
                {
                    AfficherOuCacherChargement(false);
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

                //On affiche les informations
                ShowDetails();

                //Ensuite on charge son journal, pour afficher son historique
                client.SelectJournalDeLaDemandeCompleted += (jrnlsender, jrnlargs) =>
                    {

                        AfficherOuCacherChargement(false);

                        if (jrnlargs.Cancelled || jrnlargs.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.Show(error, Languages.ListeCodePoste);
                            return;
                        }
                        if (jrnlargs.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }

                        leJournal = jrnlargs.Result;
                        //On affiche les infos depuis le début
                        dtgrdParametre.ItemsSource = leJournal.OrderBy(j => j.DATEACTION);
                    };
                client.SelectJournalDeLaDemandeAsync(_CodeDemande);
            };
            client.SelectInformationDemandeAsync(_CodeDemande);
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnRechercher_Click(object sender, RoutedEventArgs e)
        {
            if (string.Empty != txtCodeDemande.Text)
            {
                btnRechercher.IsEnabled = true;
                //Recherche des données
                GetData();
            }
            else
            {
                Message.ShowError(new Exception("Veuillez entrer le code de la demande à suivre"), "Suivi de demande"); 
            }
        }
    }
}


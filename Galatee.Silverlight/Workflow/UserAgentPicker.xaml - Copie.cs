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
using Galatee.Silverlight.Resources;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.ServiceAdministration;
using Galatee.Silverlight.ServiceAccueil ;
using Galatee.Silverlight.ServiceParametrage;
using Galatee.Silverlight.ServiceWorkflow;

namespace Galatee.Silverlight.Workflow
{
    public partial class UserAgentPickerAffectation : ChildWindow
    {
        public Galatee.Silverlight.ServiceAccueil.CsUtilisateur AgentSelectionne { get; set; }
        string CodeUser = string.Empty;        
        int _idEtape;
        Guid _OperationID;
        int _centreID;
        List<int> _DemandeID = new List<int>();
        List<string> _NumDemande = new List<string>();
        Guid _workflowId;
        string CodeDemande = string.Empty;
        List<string> LesCodeDemande = new List<string>();
        List<string> LesCodeDemandeWkf = new List<string>();
        KeyValuePair<CsCopieDmdCircuit, KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>> _infoNextStep;
        public UserAgentPickerAffectation()
        {
            try
            {
                InitializeComponent();
                Initialisation();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        public UserAgentPickerAffectation(List<int> numDmd, int etape)
        {
            try
            {
                InitializeComponent();
                //LesCodeDemande = numDmd;
                foreach (int item in numDmd)
                    LesCodeDemande.Add(item.ToString());

                Initialisation();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        public UserAgentPickerAffectation(string numDmd)
        {
            try
            {
                InitializeComponent();
                CodeDemande = numDmd;
                Initialisation();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        public UserAgentPickerAffectation(string _codeDemande, int IDEtape, int CentreId, Guid OperationId,Guid workflowId)
        {
            try
            {
                InitializeComponent();
                CodeDemande = _codeDemande;
                _idEtape = IDEtape;
                _OperationID = OperationId;
                _workflowId = workflowId;

                Initialisation();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        Guid idGroupeValidation = new Guid();
        private void RemplirFonction()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Parametrage"));
                int back = LoadingManager.BeginLoading("Chargement des informations de l'étape suivante");
                //Récupération des informations de la demande
                client.GetInfoDemandeWorkflowByListCodeDemandeCompleted += (dsender, dargs) =>
                {
                    if (dargs.Cancelled || dargs.Error != null)
                    {
                        string error = dargs.Error.Message;
                        Message.ShowError(error, Languages.txtDevis);
                    }

                    if (null != dargs.Result)
                    {
                        _OperationID = dargs.Result.First().FK_IDOPERATION;
                        _workflowId = dargs.Result.First().FK_IDWORKFLOW;
                        _centreID = dargs.Result.First().FK_IDCENTRE;
                        _idEtape = dargs.Result.First().FK_IDETAPEACTUELLE;
                        foreach (var item in dargs.Result)
                        {
                            LesCodeDemandeWkf.Add(item.CODE);
                            _NumDemande.Add(item.CODE_DEMANDE_TABLETRAVAIL);
                            _DemandeID.Add(int.Parse(item.FK_IDLIGNETABLETRAVAIL));
                        }

                    client.RecupererInfoEtapeSuivanteByCodeWorkflowCompleted += (ssender, args) =>
                        {
                            LoadingManager.EndLoading(back);
                            if (args.Cancelled || args.Error != null)
                            {
                                string error = args.Error.Message;
                                Message.ShowError(error, Languages.txtDevis);
                            }

                            if (args.Result.key != null && args.Result.value.key != null &&
                                args.Result.value.value != null)
                            {
                                _infoNextStep = new KeyValuePair<CsCopieDmdCircuit, KeyValuePair<CsGroupeValidation,
                                    List<CsRHabilitationGrouveValidation>>>(args.Result.key, 
                                    new KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>( args.Result.value.key, args.Result.value.value));

                                this.Txt_codeFonction.Text = _infoNextStep.Value.Key.GROUPENAME;
                                this.Txt_codeFonction.Tag  = _infoNextStep.Value.Key.PK_ID ;
                            }
                        };
                    client.RecupererInfoEtapeSuivanteByCodeWorkflowAsync(dargs.Result.First().CODE );
                    }
                };
                client.GetInfoDemandeWorkflowByListCodeDemandeAsync(LesCodeDemande);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Initialisation()
        {
            try
            {
                RemplirFonction();
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
                if (Dtg_agent.SelectedItem != null)
                {
                    this.BtnOK.IsEnabled = false;
                    //var agent = Dtg_agent.SelectedItem as CsRHabilitationGrouveValidation;
                    var agent = Dtg_agent.SelectedItem as Galatee.Silverlight.ServiceAccueil .CsUtilisateur ;
                    //Affectation de la demande
                    CsAffectationDemandeUser lAffectation = new CsAffectationDemandeUser();
                    lAffectation.CODEDEMANDE = CodeDemande;
                    lAffectation.FK_IDETAPE = _infoNextStep.Key.FK_IDETAPE;
                    lAffectation.MATRICULEUSER = agent.LOGINNAME;
                    lAffectation.OPERATIONID = _OperationID;
                    lAffectation.CENTREID = _centreID;
                    lAffectation.WORKFLOWID = _workflowId;
                    lAffectation.FK_IDETAPEFROM = _idEtape;
                    lAffectation.MATRICULEUSERCREATION = UserConnecte.matricule;
                    lAffectation.PK_ID = Guid.NewGuid();


                    int back = LoadingManager.BeginLoading("Affectation en cours ...");
                    
                    //On transmet d'abod la demnde avant de l'affecté
                    //Tout est bon on transmet la demande            
                    WorkflowClient Wkfclient = new WorkflowClient(Utility.Protocole(), Utility.EndPoint("Workflow"));        
                    Wkfclient.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
                    Wkfclient.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
                    Wkfclient.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);

                    Wkfclient.ExecuterActionSurPlusieursDemandesCompleted  += (ssender, wargs) =>
                    {
                        LoadingManager.EndLoading(back);
                        if (wargs.Cancelled || wargs.Error != null)
                        {
                            string error = wargs.Error.Message;
                            Message.Show(error, "Affectation Demande");
                            return;
                        }
                        if (wargs.Result == null)
                        {
                            Message.ShowError(Languages.msgErreurChargementDonnees, "Affectation demande");
                            return;
                        }
                        if (wargs.Result.StartsWith("ERR"))
                        {
                            Message.ShowError(wargs.Result, "Affectation demande");
                        }
                        else
                        {
                            ParametrageClient client = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                            client.AffecterDemandeCompleted += (af_sender, args) =>
                            {
                                if (args.Cancelled || args.Error != null)
                                {
                                    string error = args.Error.Message;
                                    Message.ShowError(error, Languages.txtDevis);
                                    return;
                                }
                                if (args.Result == null)
                                {
                                    Message.ShowError("Une erreur est survenue", Languages.txtDevis);
                                    return;
                                }
                                if (args.Result != null)
                                {
                                    if (!args.Result)
                                    {
                                        LoadingManager.EndLoading(back);
                                        Message.ShowInformation("Une erreur est survenue", "Affectation de demande");
                                    }
                                    else
                                    {
                                        Message.ShowInformation(wargs.Result, "Affectation demande");
                                        List<ServiceAccueil.CsUtilisateur> leUser = new List<ServiceAccueil.CsUtilisateur>();
                                        leUser.Add(agent);
                                        //Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", _NumDemande, "BRANCHEMENT NEUF ET ABONNEMENT");
                                        this.DialogResult = true;
                                    }
                                }                                                                                               
                            };
                            client.AffecterDemandeAsync(new List<CsAffectationDemandeUser>() { lAffectation });                                
                        }
                    };
                    Wkfclient.ExecuterActionSurPlusieursDemandesAsync(LesCodeDemandeWkf, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule, string.Empty);                                    
                }
                else
                {
                    throw new Exception(Languages.msgEmptyUser);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

   

        private void Btn_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Recherche(this.Txt_matricule .Text ,this.Txt_name.Text );
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Recherche(string Matricule,string NomAgent)
        {
            ObjMATRICULE critere = new ObjMATRICULE();
            try
            {
                busyIndicator.IsBusy = true;
                var admClient = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                LayoutRoot.Cursor = Cursors.Wait;
                admClient.GetUserByIdGroupeValidationMatriculeNomAsync((Guid)Txt_codeFonction.Tag, _centreID, SessionObject.Enumere.CodeFonctionMetreur, Matricule, NomAgent);
                admClient.GetUserByIdGroupeValidationMatriculeNomCompleted += (sen, result) =>
                {
                    if (result.Cancelled || result.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = result.Error.Message;
                        Message.ShowError(error, Languages.txtDevis);
                        return;
                    }
                    if (result.Result == null || result.Result.Count == 0)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(
                                "Aucun agent ne correspond aux critères saisis !",
                                Languages.txtDevis);
                        return;
                    }
                    if (result.Result != null && result.Result.Count > 0)
                    {
                        List<ServiceAccueil.CsUtilisateur> lstUser = new List<ServiceAccueil.CsUtilisateur>();
                        var LstDistinct = result.Result.Select(t => new { t.PK_ID, t.LIBELLE, t.MATRICULE, t.LOGINNAME,t.E_MAIL  }).Distinct().ToList();
                        foreach (var item in LstDistinct)
                        {

                            lstUser.Add(new ServiceAccueil.CsUtilisateur
                            {
                                LOGINNAME = item.LOGINNAME,
                                MATRICULE = item.MATRICULE,
                                PK_ID = item.PK_ID,
                                LIBELLE = item.LIBELLE,
                                E_MAIL = item.E_MAIL 
                            });
                        }
                        this.Dtg_agent.ItemsSource = lstUser;
                    }
                    busyIndicator.IsBusy = false;
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
            }
            catch (Exception ex)
            {
                busyIndicator.IsBusy = false;
                throw ex;
            }
        }

        private void Btn_reset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Dtg_agent.ItemsSource = null;
                //this.Cbo_Fonction.SelectedItem = null;
                this.Txt_matricule.Text = string.Empty;
                this.Txt_name.Text = string.Empty;
                this.Txt_codeFonction.Text = string.Empty;
                AgentSelectionne = null;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
    }
}


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
using Galatee.Silverlight.ServiceWorkflow;

namespace Galatee.Silverlight.Workflow
{
    public partial class UserAgentPickerAffectation : ChildWindow
    {
        public Galatee.Silverlight.ServiceAccueil.CsUtilisateur AgentSelectionne { get; set; }
        string CodeUser = string.Empty;
        int _idEtape;
        int _idEtapeSuivante ;
        Guid _OperationID;
        int _centreID;
        List<int> _DemandeID = new List<int>();
        List<string> _NumDemande = new List<string>();
        Guid _workflowId;
        string CodeDemande = string.Empty;
        List<int> LesCodeDemande = new List<int>();
        List<string> LesCodeDemandeWkf = new List<string>();
        Dictionary<ServiceParametrage.CsGroupeValidation, List<ServiceParametrage.CsRHabilitationGrouveValidation>> lsDatas;



        public CsAffectationDemandeUser lAffectationDem = new CsAffectationDemandeUser();
        public UserAgentPickerAffectation()
        {
            try
            {
                InitializeComponent();
                RetourneTousUtlisateur();
                GetData();
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
                RetourneTousUtlisateur();
                GetData();

                LesCodeDemande = numDmd;
                _idEtape = etape;
                RemplirFonction(etape, numDmd.First());
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
                RetourneTousUtlisateur();
                GetData();

                CodeDemande = numDmd;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        bool IsOT = false;
        int IdDemande = 0;
        public UserAgentPickerAffectation(string numDmd,int IdDmd, int etape)
        {
            try
            {
                InitializeComponent();
                RetourneTousUtlisateur();
                GetData();

                CodeDemande = numDmd;
                _idEtape = etape;
                IsOT = true;
                IdDemande = IdDmd;
                RemplirFonction(etape,IdDmd);
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
                RetourneTousUtlisateur();
                GetData();

                CodeDemande = _codeDemande;
                _idEtape = IDEtape;
                _OperationID = OperationId;
                _workflowId = workflowId;

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }





        Guid idGroupeValidation = new Guid();



        private void RetourneTousUtlisateur()
        {
            try
            {



                Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient client = new Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                client.RetourneListeAllUserCompleted += (ss, res) =>
                {
                    if (res.Cancelled || res.Error != null)
                    {
                        string error = res.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }

                    if (res.Result == null || res.Result.Count == 0)
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle + "  RetourneTousUtlisateur");
                        return;
                    }
                    SessionObject.ListeDesUtilisateurs = res.Result;
                };
                client.RetourneListeAllUserAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private void RemplirFonction(int Etape,int Iddemande)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.ActeurEtapeAsync(Etape, Iddemande);
                client.ActeurEtapeCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    string numedemande = string.Empty;
                    string Client = string.Empty;
                    if (b.Result != null)
                    {
                        this.Txt_codeFonction.Text = b.Result.Keys.First().GROUPENAME ;
                        this.Txt_codeFonction.Tag = b.Result.Keys.First().PK_ID ;
                        foreach (var item in b.Result)
                        {
                            this.Dtg_agent.ItemsSource = item.Value;
                            _idEtapeSuivante = b.Result.Keys.First().IDETAPE ;
                        }
                    }
                    else
                        Message.ShowError(b.Error.Message, Silverlight.Resources.Devis.Languages.txtDevis);

                };
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
                    List<CsAffectationDemandeUser> lstAffectation = new List<CsAffectationDemandeUser>();
                    var agent = Dtg_agent.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsUtilisateur;
                    //Affectation de la demande

               
                    if (IsOT && lAffectationDem  != null )
                    {

                        CsAffectationDemandeUser lAffectation = new CsAffectationDemandeUser();
                        lAffectation.CODEDEMANDE = CodeDemande;
                        lAffectation.FK_IDETAPE = _idEtapeSuivante;
                        lAffectation.MATRICULEUSER = agent.MATRICULE;
                        lAffectation.FK_IDUSERAFFECTER = agent.PK_ID;
                        lAffectation.OPERATIONID = _OperationID;
                        lAffectation.CENTREID = _centreID;
                        lAffectation.WORKFLOWID = _workflowId;
                        lAffectation.FK_IDETAPEFROM = _idEtape;
                        lAffectation.MATRICULEUSERCREATION = UserConnecte.matricule;
                        lAffectation.PK_ID = Guid.NewGuid();
                        lAffectation.FK_IDDEMANDE = IdDemande;

                        AgentSelectionne = new ServiceAccueil.CsUtilisateur();
                        AgentSelectionne.MATRICULE = agent.MATRICULE ; 
                        AgentSelectionne.PK_ID = agent.PK_ID ; 
                        AgentSelectionne.LIBELLE = agent.LIBELLE ;
                        lAffectationDem = lAffectation;
                        this.DialogResult = false;
                    }
                    else
                    {

                        foreach (int item in LesCodeDemande)
                        {
                            CsAffectationDemandeUser lAffectation = new CsAffectationDemandeUser();
                            lAffectation.CODEDEMANDE = CodeDemande;
                            lAffectation.FK_IDETAPE = _idEtapeSuivante;
                            lAffectation.MATRICULEUSER = agent.MATRICULE;
                            lAffectation.FK_IDUSERAFFECTER = agent.PK_ID;
                            lAffectation.OPERATIONID = _OperationID;
                            lAffectation.CENTREID = _centreID;
                            lAffectation.WORKFLOWID = _workflowId;
                            lAffectation.FK_IDETAPEFROM = _idEtape;
                            lAffectation.MATRICULEUSERCREATION = UserConnecte.matricule;
                            lAffectation.PK_ID = Guid.NewGuid();
                            lAffectation.FK_IDDEMANDE = item;
                            lstAffectation.Add(lAffectation);
                        }

                        int back = LoadingManager.BeginLoading("Affectation en cours ...");
                        Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                        client.AffecterDemandeCompleted += (af_sender, args) =>
                        {
                            LoadingManager.EndLoading(back);
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
                                if (string.IsNullOrEmpty(args.Result))
                                {
                                    LoadingManager.EndLoading(back);
                                    Message.ShowInformation("Affectation", "Affectation de demande");
                                    this.DialogResult = false;
                                }
                                else
                                {
                                    LoadingManager.EndLoading(back);
                                    Message.ShowError("Affectation", "Erreur survenue à l'affectation");
                                }
                                this.DialogResult = true;
                            }
                        };
                        client.AffecterDemandeAsync(lstAffectation);
                    }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lsDatas != null && lsDatas.Count != 0)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lsDatas.Keys.ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "GROUPENAME", "DESCRIPTION", "Liste des groupes");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_Group);
                ctr.Show();
            }

        }



        void galatee_OkClickedbtn_Group(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceParametrage.CsGroupeValidation _LeGroup = (ServiceParametrage.CsGroupeValidation)ctrs.MyObject;

                if (_LeGroup != null)
                {
                    this.Txt_codeFonction.Text = _LeGroup.GROUPENAME;
                    this.Txt_codeFonction.Tag = _LeGroup.PK_ID;
                    KeyValuePair<ServiceParametrage.CsGroupeValidation, List<ServiceParametrage.CsRHabilitationGrouveValidation>> d = lsDatas.Where(item => item.Key.PK_ID == _LeGroup.PK_ID).FirstOrDefault();

                    List<ServiceAccueil.CsUtilisateur> users = new List<ServiceAccueil.CsUtilisateur>();
                    ServiceAccueil.CsUtilisateur us = null;

                    foreach (Galatee.Silverlight.ServiceAdministration.CsUtilisateur st in SessionObject.ListeDesUtilisateurs)
                    {
                        us = new ServiceAccueil.CsUtilisateur();
                        foreach (ServiceParametrage.CsRHabilitationGrouveValidation item in d.Value)
                        {
                            if (item.FK_IDADMUTILISATEUR == st.PK_ID)
                            {
                                us.PK_ID = st.PK_ID;
                                us.MATRICULE = st.MATRICULE;
                                us.LOGINNAME = st.LOGINNAME;
                                us.FK_IDCENTRE = st.FK_IDCENTRE;
                                us.CENTRE = st.CENTRE;
                                us.LIBELLE = st.LIBELLE;
                                us.USERCREATION = st.USERCREATION;
                                us.DATECREATION = st.DATECREATION;
                                us.FK_IDSTATUS = st.FK_IDSTATUS;
                                users.Add(us);
                            }
                        }
                    }

                    this.Dtg_agent.ItemsSource = users;
                }
            }
        }



        private void GetData()
        {
            try
            {
                ServiceParametrage.ParametrageClient client = new ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllGroupeValidationCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, "Groupes de validation");
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, "Groupes de validation");
                        return;
                    }
                    lsDatas = new Dictionary<ServiceParametrage.CsGroupeValidation, List<ServiceParametrage.CsRHabilitationGrouveValidation>>();
                    if (args.Result != null)
                        foreach (var item in args.Result)
                        {
                            lsDatas.Add(item.Key, item.Value);
                        }
                };
                client.SelectAllGroupeValidationAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}


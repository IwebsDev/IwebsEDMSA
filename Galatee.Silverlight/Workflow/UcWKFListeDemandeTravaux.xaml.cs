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
//using Galatee.Silverlight.ServiceExecuterActionWorkflow;
using System.Globalization;
using Galatee.Silverlight.Workflow;
using Galatee.Silverlight.ServiceWorkflow;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Workflow
{
    public partial class UcWKFListeDemandeTravaux : ChildWindow
    {

        //List<DemandeWorkflowInformation> LstLesDemandes;
        List<Galatee.Silverlight.ServiceAccueil.CsProgarmmation> LstLesDemandes;
        List<CsAffectationDemandeUser> lsDemandesAffectes;
        DemandeWorkflowInformation DmdSelectionnee;
        List<CsCopieDmdConditionBranchement> toutesConditionsDeLEtape;
        List<DemandeWorkflowInformation> __demandesCochees;
        Guid FKRWorkflowCentre;
        Guid _RAffEtapeWorkflow;
        int nombreEtapeCircuit = 0;
        int FKEtape;
        string CodeEtape;
        Guid _OperationID;
        CsEtape _LEtape;
        string NomOperation = string.Empty;
        string CodeDemande = string.Empty;
        string LeControle = string.Empty;
        List<Galatee.Silverlight.ServiceAccueil.CsRenvoiRejet> _LesRenvoisEtapes;
        bool IsTraitementParLot = false;
        bool IsSortieCompteur = false;

        public UcWKFListeDemandeTravaux(Guid Operation, int IDEtape, int NbreEtape)
        {
            try
            {
                InitializeComponent();
                FKEtape = IDEtape;
                _OperationID = Operation;
                nombreEtapeCircuit = NbreEtape;
                SessionObject.IsChargerDashbord = false;
                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape):null ;
                ChargerListDesSite(null );
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }
        public UcWKFListeDemandeTravaux(Guid Operation, List<Guid> lstIdDemande, bool _IstraitementLot, string _NomOperation, int IDEtape)
        {
            try
            {
                InitializeComponent();
                Translate();
                NomOperation = _NomOperation;
                _OperationID = Operation;
                FKEtape = IDEtape;
                IsTraitementParLot = _IstraitementLot;
                SessionObject.IsChargerDashbord = false;
                ChargeEquipe(UserConnecte.Centre);
                ChargerListDesSite(lstIdDemande);
                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null;
               
                //On cache dabord le bouton
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }
        bool IsConsultationSeul = false;
        public UcWKFListeDemandeTravaux(bool IsConsultation, Guid Operation, List<Guid> lstIdDemande, bool _IstraitementLot, string _NomOperation, int IDEtape,string _CodeEtape)
        {
            try
            {
                InitializeComponent();
                Translate();
                NomOperation = _NomOperation;
                _OperationID = Operation;
                FKEtape = IDEtape;
                CodeEtape = _CodeEtape;
                IsTraitementParLot = _IstraitementLot;
                ChargeEquipe(UserConnecte.Centre);
                SessionObject.IsChargerDashbord = false;
                IsConsultationSeul = IsConsultation;
                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null;
                if (IsConsultation) EditerButton.Visibility = System.Windows.Visibility.Collapsed;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                if (CodeEtape == "SCOMP") IsSortieCompteur = true;
                else if (CodeEtape == "SMAT") IsSortieCompteur = false;
                ChargerListDesSite(lstIdDemande);

                //On cache dabord le bouton
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }
        public UcWKFListeDemandeTravaux(Guid Operation, int IDEtape,bool _IstraitementLot,  int NbreEtape)
        {
            try
            {
                InitializeComponent();
                FKEtape = IDEtape;
                _OperationID = Operation;
                nombreEtapeCircuit = NbreEtape;
                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null;

                Translate();
                IsTraitementParLot = _IstraitementLot;
                SessionObject.IsChargerDashbord = false;
                ChargerListDesSite(null );
                //On cache dabord le bouton
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }
        public UcWKFListeDemandeTravaux(Guid Operation, int IDEtape, int NbreEtape, string _NomOperation)
        {
            try
            {
                InitializeComponent();
                FKEtape = IDEtape;
                _OperationID = Operation;
                NomOperation = _NomOperation;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                nombreEtapeCircuit = NbreEtape;
                SessionObject.IsChargerDashbord = false;
               
                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null ;
                ChargerListDesSite(null );
                Translate();
                EditerButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord");
            }
        }
        public UcWKFListeDemandeTravaux(Guid Operation, int IDEtape, bool _IstraitementLot, int NbreEtape, string _NomOperation,string CodeEtape)
        {
            try
            {
                InitializeComponent();
                FKEtape = IDEtape;
                _OperationID = Operation;
                NomOperation = _NomOperation;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                nombreEtapeCircuit = NbreEtape;
                SessionObject.IsChargerDashbord = false;
                ChargerListDesSite(null );

                _LEtape = null != SessionObject._ToutesLesEtapesWorkflows ? SessionObject._ToutesLesEtapesWorkflows.FirstOrDefault(e => e.PK_ID == IDEtape) : null;
                IsTraitementParLot = _IstraitementLot;
                this.Title = _NomOperation;
                Translate();

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord");
            }
        }
        #region Fonctions

        void Translate()
        {
            CancelButton.Content = Languages.Annuler;
        }

        void GetData(List<int> LesCentreHabilite, string NumeroProgramme, DateTime? DateDebut, DateTime? DateFin, string IdEquipe, int IdEtape, bool IsSortieCompteur)
        {
            if (Guid.Empty != _OperationID && 0 != FKEtape)
            {
                LstLesDemandes = new List<Galatee.Silverlight.ServiceAccueil.CsProgarmmation>();
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                {
                    client.ChargerListeProgramAsync(LesCentreHabilite,NumeroProgramme, DateDebut, DateFin, IdEquipe, IdEtape, IsSortieCompteur);
                    client.ChargerListeProgramCompleted += (sender, args) =>
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

                        LstLesDemandes = new List<Galatee.Silverlight.ServiceAccueil.CsProgarmmation>();
                        LstLesDemandes = args.Result;
                        dtgrdParametre.ItemsSource = LstLesDemandes;
                        dtgrdParametre.ItemsSource = LstLesDemandes.OrderBy(t => t.DATETRANSMISSION).ToList();

                    };
                }
            }
        }
        #endregion


        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        private List<ServiceAccueil.CsCentre> _listeDesCentreExistant = null;
        void ChargerListDesSite(List<Guid> lstIdDemande)
        {
            try
            {

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    SessionObject.ModuleEnCours = "Accueil";
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    _listeDesCentreExistant = lesCentre;
                    GetData(_listeDesCentreExistant.Select(p => p.PK_ID).ToList(),string.Empty , null, null, null, FKEtape, IsSortieCompteur);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteAsync(false);
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        if (SessionObject.LstCentre.Count != 0)
                        {
                            SessionObject.ModuleEnCours = "Accueil";
                            lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                            lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                            _listeDesCentreExistant = lesCentre;
                            GetData(_listeDesCentreExistant.Select(p => p.PK_ID).ToList(), string.Empty,null, null, null, FKEtape, IsSortieCompteur);

                        }
                        else
                        {
                            Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (__demandesCochees == null) __demandesCochees = new List<DemandeWorkflowInformation>();
            else if (__demandesCochees != null && __demandesCochees.Count != 0)
                __demandesCochees.Clear();
            foreach (DemandeWorkflowInformation item in dtgrdParametre.ItemsSource as List<DemandeWorkflowInformation>)
            {
                if (checkSelectedItem((CheckBox)this.dtgrdParametre.Columns[0].GetCellContent(item) as CheckBox))
                    __demandesCochees.Add(item);
            }
            //seulement le demande qui ont été coché
            if (null != __demandesCochees && __demandesCochees.Count >= 1)
            {

                List<string> codesDemandes = new List<string>();
                foreach (var dmd in __demandesCochees)
                {
                    codesDemandes.Add(dmd.CODE);
                }

                //Transmission par groupe ou non
                TransmettreDemande(codesDemandes);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SessionObject.IsChargerDashbord = true ;
            this.DialogResult = false;
        }

        void TransmettreDemande(DemandeWorkflowInformation dmdInfo)
        {
            prgBar.Visibility = System.Windows.Visibility.Visible;
            WorkflowClient client = new WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
            client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
            client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
            client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);
            client.ExecuterActionSurDemandeCompleted += (sender, args) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
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
                }
            };
            client.ExecuterActionSurDemandeAsync(dmdInfo.CODE, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule, string.Empty);
        }
        void TransmettreDemande(List<string> Codes)
        {
            prgBar.Visibility = System.Windows.Visibility.Visible;

            WorkflowClient client = new WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
            client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
            client.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
            client.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);
            client.ExecuterActionSurPlusieursDemandesCompleted += (sender, args) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
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
                    Message.ShowInformation(args.Result, Languages.Parametrage);
            };
            client.ExecuterActionSurPlusieursDemandesAsync(Codes, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule, string.Empty);
        }

        #region Gestion DataGrid

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ActiverOuDesactiverBouton();
        }
     
        #endregion
        
        private void ConsulterButton_Click(object sender, RoutedEventArgs e)
        {
            //ON récupère la demande sélectionnée, 
            try
            {
                if (dtgrdParametre.SelectedItem != null )
                {
                    DemandeWorkflowInformation SelectedObject = (DemandeWorkflowInformation)dtgrdParametre.SelectedItem;
                    Galatee.Silverlight.Devis.UcConsultationDevis detailForm = new Galatee.Silverlight.Devis.UcConsultationDevis(int.Parse(SelectedObject.FK_IDLIGNETABLETRAVAIL));
                    detailForm.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Impossible de traiter la demande. Détails de l'erreur : " + ex.Message, "Traiter une demande");
            }
          
        }
        
        private void EditerButton_Click(object sender, RoutedEventArgs e)
        {
            //ON récupère la demande sélectionnée, 
            try
            {
                this.EditerButton.IsEnabled = false;
                this.ConsulterButton.IsEnabled = false;
                this.CancelButton.IsEnabled = false;

                if (this.dtgrdParametre.ItemsSource != null)
                {
                    ServiceAccueil.CsProgarmmation lePgmSelect = ((List<ServiceAccueil.CsProgarmmation>)this.dtgrdParametre.ItemsSource).FirstOrDefault(o => o.IsSelect);
                    if (lePgmSelect != null && lePgmSelect.FK_IDEQUIPE != null)
                    {
                        if (CodeEtape == "SCOMP")
                        {
                            Galatee.Silverlight.Devis.UcSortieCompteurDetail ctrl = new Galatee.Silverlight.Devis.UcSortieCompteurDetail(lePgmSelect, FKEtape);
                            ctrl.Closed += ctrl_Closed;
                            ctrl.Show();
                        }
                        else if (CodeEtape == "SMAT")
                        {
                            Galatee.Silverlight.Devis.UcSortieMateriel ctrl = new Galatee.Silverlight.Devis.UcSortieMateriel(lePgmSelect, FKEtape);
                            ctrl.Closed += ctrl_Closed;
                            ctrl.Show();
                        
                        }
                     }
                 
                }
         
            }
            catch (Exception ex)
            {
                //Message.ShowError("Impossible de traiter la demande. Détails de l'erreur : " + ex.Message, "Traiter une demande");
            }
        }

        void ctrl_Closed(object sender, EventArgs e)
        {
            this.EditerButton.IsEnabled = true;
            this.ConsulterButton.IsEnabled = true;
            this.CancelButton.IsEnabled = true;
        }
        private void dtgrdParametre_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dmdRow = e.Row.DataContext as DemandeWorkflowInformation;
            if (dmdRow != null)
            {
                if (dmdRow.DATEFINTRAITEMENT.HasValue)
                {
                    int delai = ((DateTime)dmdRow.DATEFINTRAITEMENT.Value - DateTime.Today)
                        .Days;
                    if (delai <= 0)
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Red);
                        e.Row.Foreground = SolidColorBrush;
                    }
                    return;
                }
               else  if (dmdRow.ESTAFFECTE)
                {
                    if (dmdRow.UTILISATEURAFFECTE == UserConnecte.matricule)
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Green);
                        e.Row.Foreground = SolidColorBrush;
                        e.Row.FontWeight = FontWeights.Bold;
                        return;

                    }
                    else if (dmdRow.UTILISATEURAFFECTE != UserConnecte.matricule)
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.LightGray);
                        e.Row.Foreground = SolidColorBrush;
                        e.Row.FontWeight = FontWeights.Bold;
                        e.Row.IsEnabled = false;
                        return;

                    }
                   
                }
               else  if (dmdRow.FK_IDSTATUS.HasValue && dmdRow.FK_IDSTATUS.Value == (int)WorkflowManager.STATUSDEMANDE.Rejetee)
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Blue);
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Bold;
                }
                else
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Black );
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Normal;

                }
              
            }
        }
        void ucform_Closed(object sender, EventArgs e)
        {
            //On recharge les demandes
            //if (lesCentre != null && lesCentre.Count != 0)
            //    GetData(lesCentre.Select(t => t.PK_ID).ToList());
        }

        void cw_Closed(object sender, EventArgs e)
        {
            //On recharge les demandes
            this.IsEnabled = true;
            this.EditerButton.IsEnabled = true ;
            this.ConsulterButton.IsEnabled = true;
            this.CancelButton.IsEnabled = true;
            List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetreTotal(SessionObject.LstCentre , UserConnecte.listeProfilUser);
        }
        DateTime lastClick = DateTime.Now;
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<ServiceAccueil.CsProgarmmation >;
            if (dg.SelectedItem != null)
            {
                ServiceAccueil.CsProgarmmation SelectedObject = (ServiceAccueil.CsProgarmmation)dg.SelectedItem;
                if (SelectedObject.IsSelect == false)
                {
                    SelectedObject.IsSelect = true;
                    List<ServiceAccueil.CsProgarmmation> l = allObjects.Where(t => t.NUMPROGRAMME != SelectedObject.NUMPROGRAMME).ToList();
                    if (l != null && l.Count != 0)
                        l.ForEach(o => o.IsSelect = false);
                
                }
                else
                    SelectedObject.IsSelect = false;
                if ((DateTime.Now - lastClick).Ticks < 25000000)
                {
                    if (IsConsultationSeul)
                        ConsulterButton_Click(null, null);
                    else
                        EditerButton_Click(null, null);
                }
                lastClick = DateTime.Now;
            }
        }
        bool checkSelectedItem(CheckBox check)
        {
            if (check != null)
            {
                CheckBox chk = check;
                return chk.IsChecked.Value;
            }
            else return false;
        }

        void checkerSelectedItem(CheckBox check)
        {
            try
            {
                CheckBox chk = check;
                chk.IsChecked = true;
                EditerButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void DechekerSelectedItem(CheckBox check)
        {
            try
            {
                CheckBox chk = check;
                chk.IsChecked = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChildWindow_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SessionObject.IsChargerDashbord = true ;
        }
        private void ChargeEquipe(string p)
        {
            try
            {
                List<Galatee.Silverlight.ServiceAccueil.CsGroupe> lstequipe = new List<Galatee.Silverlight.ServiceAccueil.CsGroupe>();
                //Lancer la transaction de mise a jour en base
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeGroupeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    if (res.Result != null && res.Result.Count != 0)
                    {
                        lstequipe.Add(new ServiceAccueil.CsGroupe { LIBELLE = "AUCUN" });
                        lstequipe.AddRange(res.Result);
                        cboEquipe.ItemsSource = lstequipe;
                        cboEquipe.SelectedValuePath = "ID";
                        cboEquipe.DisplayMemberPath = "LIBELLE";
                    }
                };
                service1.RetourneListeGroupeAsync(UserConnecte.Centre);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        List<int> lstCentreSiteDist = new List<int>();
        List<int> lstCentreSelect= new List<int>();
        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            
            GetData(_listeDesCentreExistant.Select(p=>p.PK_ID ).ToList(),this.Txt_NumeroProgramme.Text ,
                                                      (this.dtProgramDeb.SelectedDate ==null ? null :this.dtProgramDeb.SelectedDate), 
                                                      (this.dtProgramFin.SelectedDate ==null ? null :this.dtProgramFin.SelectedDate),
                                                      ((this.cboEquipe.SelectedItem == null || (this.cboEquipe.SelectedItem !=null  && ((ServiceAccueil.CsGroupe)this.cboEquipe.SelectedItem).LIBELLE == "AUCUN")) ? null : ((ServiceAccueil.CsGroupe)this.cboEquipe.SelectedItem).ID.ToString()),
                                                      FKEtape, 
                                                      IsSortieCompteur);


        }
    }
}


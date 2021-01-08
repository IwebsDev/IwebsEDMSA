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
using Galatee.Silverlight.Resources.Accueil;
//using Galatee.Silverlight.Rpnt.Helper;
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.Workflow;

namespace Galatee.Silverlight.Devis
{
    public partial class UcProgrammation : ChildWindow
    {
        List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
        int EtapeActuelle;

        public UcProgrammation()
        {
        }

        public UcProgrammation(List<int> demandes)
        {
            InitializeComponent();
            ChargeEquipe(UserConnecte.Centre);
            //RechercheDemande(demandes);
        }
        List<int> lstIdDemande = new List<int>();
        public UcProgrammation(List<int> demandes, int fkIdEtape)
        {
            InitializeComponent();
            lstIdDemande = demandes;
            ChargeEquipe(UserConnecte.Centre);
            RechercheTypedemande(fkIdEtape);

            EtapeActuelle = fkIdEtape;
        }
        List<CsGroupe> lstequipe = new List<CsGroupe>();
        CsTdem leType = null;
        private void ChargeEquipe(string p)
        {
            try
            {
                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeGroupeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    lstequipe = res.Result;
                    cboEquipe.ItemsSource = lstequipe;
                    cboEquipe.SelectedValuePath = "ID";
                    cboEquipe.DisplayMemberPath = "LIBELLE";

                };
                service1.RetourneListeGroupeAsync(UserConnecte.Centre);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        private void RechercheDemande(List<int> demandes,int idTdem)
        {
            try
            {

                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeDemandeEtapeByIdCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    LstDemande = res.Result;
                    if (LstDemande != null && LstDemande.Count != 0)
                    {
                        dgDemande.ItemsSource = LstDemande;
                    }
                    else
                        Message.ShowError("Aucune données trouvées", Langue.lbl_Menu);
                };
                service1.RetourneListeDemandeEtapeByIdAsync(demandes, idTdem);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        private void RechercheTypedemande(int idetape)
        {
            try
            {

                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneTypeDemandeFromIdEtapeWkfCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    leType = new CsTdem();
                    leType = res.Result;
                    RechercheDemande(lstIdDemande, leType.PK_ID);
                };
                service1.RetourneTypeDemandeFromIdEtapeWkfAsync(idetape);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        private void EditerProgrammation( string NumeroProgramme)
        {
            try
            {

               List<int> demandes = ((List<CsDemandeBase>)dgDemandeAffecte.ItemsSource).Select(d => d.PK_ID).ToList<int>();
               string TypeDemande = ((List<CsDemandeBase>)dgDemandeAffecte.ItemsSource).ToList().FirstOrDefault().LIBELLETYPEDEMANDE;
                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                //service1.RetourneElementDEvisFromIdDemandeCompleted += (sr, res) =>
                service1.ChargerListeDonneeProgramReeditionCompleted += (sr, res) =>
                {
                    
                    if (res != null && res.Cancelled )
                        return;

                    List<ObjELEMENTDEVIS> list = new List<ObjELEMENTDEVIS>();
                    list.AddRange(res.Result.Where(t => !t.ISEXTENSION).ToList());

                    list.ForEach(t => t.NUMFOURNITURE = ((CsGroupe)this.cboEquipe.SelectedItem).LIBELLE);
                    list.ForEach(t => t.NUMDEVIS = this.dtProgram.SelectedDate.Value.ToShortDateString());
                    list.ForEach(t => t.LIBELLETYPEDEMANDE = TypeDemande);
                    list.ForEach(t => t.USERCREATION = NumeroProgramme);
                    Utility.ActionDirectOrientation<ServicePrintings.ObjELEMENTDEVIS, ObjELEMENTDEVIS>(list, null, SessionObject.CheminImpression, "FicheProgrammation", "Devis", true);
                    Message.ShowInformation("Programmation effectuée", Langue.lbl_Menu);
                    this.DialogResult = true;

                };
                //service1.RetourneElementDEvisFromIdDemandeAsync(demandes);
                service1.ChargerListeDonneeProgramReeditionAsync(NumeroProgramme);
                service1.CloseAsync();
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
                this.OKButton.IsEnabled = false;
                if (!string.IsNullOrEmpty(dtProgram.Text) && ((List<CsDemandeBase>)dgDemandeAffecte.ItemsSource).Count() > 0 && cboEquipe.SelectedValue != null)
                {
                    List<CsDemandeBase> lesDemandeSelect = (List<CsDemandeBase>)dgDemandeAffecte.ItemsSource;
                    lesDemandeSelect.ForEach(p => p.FK_IDETAPEENCOURE = EtapeActuelle);
                    lesDemandeSelect.ForEach(p => p.MATRICULE  = UserConnecte.matricule );
                    ServiceWorkflow.WorkflowClient client = new ServiceWorkflow.WorkflowClient(Utility.Protocole(), Utility.EndPoint("Workflow"));
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service1.InsertProgrammationCompleted += (sr, res) =>
                    {

                        if (res != null && res.Cancelled)
                            return;
                        if (!string.IsNullOrEmpty(res.Result))
                            EditerProgrammation(res.Result);
                        else
                            Message.ShowError("Echec de programmation", Langue.lbl_Menu);
                    };
                    service1.InsertProgrammationAsync(Guid.Parse(cboEquipe.SelectedValue.ToString()), lesDemandeSelect, DateTime.Parse(dtProgram.Text));
                    service1.CloseAsync();
                }
                else
                {
                    Message.ShowError("Veuillez renseigner la date et sélectionner le groupe et au moins une demande", Langue.lbl_Menu);
                    this.OKButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Echec", Langue.lbl_Menu);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Charger_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsDemandeBase>(dgDemande, dgDemandeAffecte);
        }

        private void Decharger_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsDemandeBase>(dgDemandeAffecte, dgDemande);
        }

        private void chargerTout_Click(object sender, RoutedEventArgs e)
        {
            //dgDemandeAffecte.ItemsSource = null;
            //dgDemandeAffecte.ItemsSource = LstDemande;


            try
            {
                List<CsDemandeBase> ListeSelect = ((List<CsDemandeBase>)this.dgDemande.ItemsSource).ToList();
                foreach (CsDemandeBase item in ListeSelect)
                    dgDemande.SelectedItems.Add(item);

                Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsDemandeBase>(dgDemande, dgDemandeAffecte);
            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur au chargement des demandes", "Demande");
            }
            
        }

        private void DechargerTout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<CsDemandeBase> ListeSelect = ((List<CsDemandeBase>)this.dgDemandeAffecte.ItemsSource).ToList();
                foreach (CsDemandeBase item in ListeSelect)
                    dgDemandeAffecte.SelectedItems.Add(item);

                Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsDemandeBase>(dgDemandeAffecte, dgDemande);
            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur au chargement des demandes", "Demande");
            }
        }

        private void btn_Attribuer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}


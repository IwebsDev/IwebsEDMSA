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
    public partial class UcProgrammationEP : ChildWindow
    {
        List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
        int EtapeActuelle;

        public UcProgrammationEP()
        {
        }

        public UcProgrammationEP(List<int> demandes)
        {
            InitializeComponent();
            ChargeEquipe(UserConnecte.Centre);
            //RechercheDemandeSansClient(demandes);
        }
        List<int> lstIdDemande = new List<int>();

        public UcProgrammationEP(List<int> demandes, int fkIdEtape)
        {
            InitializeComponent();
            lstIdDemande = demandes;
            ChargeEquipe(UserConnecte.Centre);
            RechercheTypedemande(fkIdEtape);
            EtapeActuelle = fkIdEtape;
        }
        CsTdem leType = null;
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
        private void RechercheDemande(List<int> demandes, int idTdem)
        {
            try
            {

                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeDemandeByIdSansClientCompleted += (sr, res) =>
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
                service1.RetourneListeDemandeByIdSansClientAsync(demandes, idTdem);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        List<CsGroupe> lstequipe = new List<CsGroupe>();
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
        private void EditerProgrammation(string NumeroProgramme)
        {
            try
            {

                List<int> demandes = ((List<CsDemandeBase>)dgDemandeAffecte.ItemsSource).Select(d => d.PK_ID).ToList<int>();
                string TypeDemande = ((List<CsDemandeBase>)dgDemandeAffecte.ItemsSource).ToList().FirstOrDefault().LIBELLETYPEDEMANDE;
                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneElementDEvisFromIdDemandeCompleted += (sr, res) =>
                {

                    if (res != null && res.Cancelled)
                        return;

                    res.Result.ForEach(t => t.NUMFOURNITURE = ((CsGroupe)this.cboEquipe.SelectedItem).LIBELLE);
                    res.Result.ForEach(t => t.NUMDEVIS = this.dtProgram.SelectedDate.Value.ToShortDateString());
                    res.Result.ForEach(t => t.LIBELLETYPEDEMANDE = TypeDemande);
                    res.Result.ForEach(t => t.USERCREATION = NumeroProgramme);
                    Utility.ActionDirectOrientation<ServicePrintings.ObjELEMENTDEVIS, ObjELEMENTDEVIS>(res.Result, null, SessionObject.CheminImpression, "FicheProgrammation", "Devis", true);
                    Message.ShowInformation("Programmation effectuée", Langue.lbl_Menu);
                    this.DialogResult = true;

                };
                service1.RetourneElementDEvisFromIdDemandeAsync(demandes);
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
                if (!string.IsNullOrEmpty(dtProgram.Text) && ((List<CsDemandeBase>)dgDemandeAffecte.ItemsSource).Count() > 0 && cboEquipe.SelectedValue!= null)
                {
                    List<CsDemandeBase> lesDemandes = new List<CsDemandeBase>();
                    lesDemandes = (List<CsDemandeBase>)dgDemandeAffecte.ItemsSource;
                    lesDemandes.ForEach(d => d.FK_IDETAPEENCOURE = EtapeActuelle);

                    ServiceWorkflow.WorkflowClient client = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service1.InsertProgrammationCompleted += (sr, res) =>
                    {
                        this.OKButton.IsEnabled = true;
                        if (res != null && res.Cancelled)
                            return;
                        if (!string.IsNullOrEmpty( res.Result))
                            EditerProgrammation(res.Result);
                        else
                            Message.ShowError("Echec de programation", Langue.lbl_Menu);
                    };
                    service1.InsertProgrammationAsync(Guid.Parse(cboEquipe.SelectedValue.ToString()), (List<CsDemandeBase>)dgDemandeAffecte.ItemsSource, DateTime.Parse(dtProgram.Text));
                    service1.CloseAsync();
                }
                else
                    Message.ShowError("Veuillez renseigner la date et sélectionner le groupe et au moins une demande", Langue.lbl_Menu);
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
            //dgDemande.ItemsSource = null;
            //dgDemande.ItemsSource = LstDemande;
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
    }
}


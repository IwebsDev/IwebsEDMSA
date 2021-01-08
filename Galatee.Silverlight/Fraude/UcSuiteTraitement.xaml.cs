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
using Galatee.Silverlight.Resources.Fraude;
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceFraude;
using System.Windows.Data;
using System.Globalization;

namespace Galatee.Silverlight.Fraude
{
    public partial class UcSuiteTraitement : ChildWindow
    {
        int myMontant = 0;
        bool _isForfait;
        int tv, EtapeActuelle;
        CsDemandeFraude LaDemande = new CsDemandeFraude();
        public UcSuiteTraitement()
        {
            InitializeComponent();
        }
        public UcSuiteTraitement(CsDemandeFraude Demande, int EtapeWk,int tva)
        {
            InitializeComponent();
            LaDemande = Demande;
            tv = tva;
            EtapeActuelle = EtapeWk;
            this.myMontant = LaDemande.ConsommationFrd.MontantFactureTTC;
            this.txtProduit.Text = Demande.CompteurFraude.libelle_Produit;
            _isForfait = LaDemande.ConsommationFrd.IsFactureAuForfait;
           //if (Demande.FactureFraude.Exigibilite != null)
           //    //this.dtpDateLimite.GetValue = Demande.FactureFraude.Exigibilite;
            if (!_isForfait)
                this.txtTotal.Text = string.Format("{0:#,##0}", myMontant);
            //this.IsFactureAuForfait = _isForfait;

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Recuperer(LaDemande);
            Validationdemande(LaDemande);
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ckbControleIndex_Checked(object sender, RoutedEventArgs e)
        {
            if (ckbControleIndex.IsChecked==true)
                this.ckbCloture.IsChecked = this.ckbForfait.IsChecked = false;
        }

        private void ckbCloture_Checked(object sender, RoutedEventArgs e)
        {
            if (this.ckbCloture.IsChecked==true)
                this.ckbForfait.IsChecked = this.ckbControleIndex.IsChecked = false;
        }

        private void ckbForfait_Checked(object sender, RoutedEventArgs e)
        {
            if (this.ckbForfait.IsChecked==true)
            {
                this.ckbCloture.IsChecked = this.ckbControleIndex.IsChecked = false;
              //  this.txtForfait.ExecMode = INOVA.ISF.WINDOWS.FORMS.ExecMode.Creation;
            }
            else
            {
                this.txtForfait.Text = string.Empty;
               // this.txtForfait.ExecMode = INOVA.ISF.WINDOWS.FORMS.ExecMode.Consultation;
            }

           // this.btnOK.Enabled = ((this.dtpDateLimite.Value != null) && (!this.ckbForfait.Checked || this.txtForfait.Text != string.Empty));
    
        }


        private void Recuperer(CsDemandeFraude LaDemande)
        {
         

            ////Facture
            //Demande.FactureFraude.Clear();
            LaDemande.FactureFraude = new CsFactureFraude();
            LaDemande.FactureFraude.FK_IDCLIENTFRAUDE = LaDemande.ClientFraude.PK_ID;
            LaDemande.FactureFraude.FK_IDPRODUIT = (int)LaDemande.CompteurFraude.FK_IDPRODUIT;
            LaDemande.FactureFraude.FK_IDFRAUDE = LaDemande.Fraude.Pk_ID;
            LaDemande.FactureFraude.DateEnregistrement = DateTime.Now.Date;

            if (LaDemande.FactureFraude.Exigibilite !=null)
            {
                LaDemande.FactureFraude.Exigibilite = DateTime.Now; ;
                LaDemande.FactureFraude.Exigibilite = DateTime.Now.AddDays(21); ;
                
            }
            LaDemande.FactureFraude.Exigibilite = Convert.ToDateTime(this.dtpDateLimite.SelectedDate);
            LaDemande.FactureFraude.Montant = LaDemande.ConsommationFrd.MontantFactureTTC;
            LaDemande.FactureFraude.MontantTVA = decimal.Parse(((LaDemande.ConsommationFrd.MontantFactureTTC * LaDemande.ConsommationFrd.TauxTVA) / (1 + LaDemande.ConsommationFrd.TauxTVA)).ToString());
            LaDemande.FactureFraude.NumeroFacture = LaDemande.Fraude.FicheTraitement.Substring(6, 6);
            LaDemande.FactureFraude.Origine = LaDemande.ClientFraude.Centre;
            LaDemande.FactureFraude.Refem = DateTime.Today.Year.ToString() + DateTime.Today.Month .ToString("00");
            LaDemande.FactureFraude.COPER = LaDemande.Coper.CODE;
            LaDemande.FactureFraude.FK_IDCOPER = LaDemande.Coper.PK_ID;


            LaDemande.FactureFraude.Exigibilite = dtpDateLimite.SelectedDate;
            LaDemande.FactureFraude.Montant = myMontant;
            LaDemande.FactureFraude.MontantTVA = tv;
            LaDemande.ConsommationFrd.IsFactureAuForfait = (bool)ckbForfait.IsChecked;
            LaDemande.FactureFraude.MoisComptable = LaDemande.ConsommationFrd.NombreMoisAFacturer.ToString();
            if (ckbForfait.IsChecked==true)
            LaDemande.FactureFraude.MontantTVA = decimal.Parse(((LaDemande.ConsommationFrd.MontantFactureTTC * LaDemande.ConsommationFrd.TauxTVA) / (1 + LaDemande.ConsommationFrd.TauxTVA)).ToString());

        }

        private void Validationdemande(CsDemandeFraude LaDemande)
        {
            try
            {
                if (LaDemande.factureFraudeEdition != null && LaDemande.factureFraudeEdition.Count != 0)
                {
                    foreach (CsEditionFactureFd leEntete in LaDemande.factureFraudeEdition)
                    {
                        leEntete.Mois =Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA( LaDemande.FactureFraude.Refem ) ;
                        leEntete.Facture = LaDemande.FactureFraude.NumeroFacture;
                        leEntete.DateExigibilite = LaDemande.FactureFraude.Exigibilite.Value.ToShortDateString();
                        leEntete.Datefacture = LaDemande.FactureFraude.DateEnregistrement.Value.ToShortDateString(); 
                    }
                    Utility.ActionDirectOrientation<ServicePrintings.CsEditionFactureFd, ServiceFraude.CsEditionFactureFd>(LaDemande.factureFraudeEdition, null, SessionObject.CheminImpression, "Facture", "Fraude", true);
                }
                //FraudeServiceClient Client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude")); ;
                //Client.ValiderDemandeEmissionFactureCompleted += (ss, b) =>
                //{
                //    if (b.Cancelled || b.Error != null)
                //    {
                //        string error = b.Error.Message;
                //        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                //        return;
                //    }
                //    if (b.Result != null)
                //    {
                //        Message.Show("Affectation effectué avce succes", "Information");
                //        List<int> Listid = new List<int>();
                //        Listid.Add(LaDemande.LaDemande.PK_ID);
                //        EnvoyerDemandeEtapeSuivante(Listid);
                //        Editerfacture();
                //        this.DialogResult = true;



                //    }
                //    else
                //        Message.ShowError("Erreur a la cloture de la demande", "Cloturedemande");
                //};
                //Client.ValiderDemandeEmissionFactureAsync(LaDemande);

            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, "Transmit");
            }
        }

        private void EnvoyerDemandeEtapeSuivante(List<int> Listid)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));

            clientWkf.ExecuterActionSurDemandeParPkIDLigneCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec de sortie materiel", Langue.lbl_Menu);
                    return;
                }
                if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
                {
                    Message.ShowError("Echec de sortie materiel", Langue.lbl_Menu);
                    return;
                }
                else
                {
                    //Message.ShowInformation("Sortie materiel éffectuée", Langue.lbl_Menu);

                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }

    }
}


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
    public partial class UcAnalyse : ChildWindow
    {
        int ConsoDejaFacturee, nombreMois, estime;
        ObservableCollection<CsAppareilUtiliserFrd> DonnesDatagrid = new ObservableCollection<CsAppareilUtiliserFrd>();
        CsDemandeFraude LaDemande = new CsDemandeFraude();
        int EtapeActuelle;
        public UcAnalyse()
        {
            InitializeComponent();
        }
        public UcAnalyse(CsDemandeFraude Demande ,int EtapeWk)
        {
            LaDemande = Demande;
            EtapeActuelle = EtapeWk;
            ConsoDejaFacturee = 0;
            nombreMois = 0;
            estime = 0;
            InitializeComponent();
            ChargerDecision();
           
            foreach (CsMoisDejaFactures item in LaDemande.MoisDejaFactures)
            {
                ConsoDejaFacturee = Convert.ToInt32(item.ConsoDejaFacturee) + ConsoDejaFacturee;
                nombreMois ++;
            }
            GetData(LaDemande.AppareilUtiliserFrd);
            txtConsommationDejaFacturee.Text = ConsoDejaFacturee.ToString();
            txtConsommationAFacturer.Text = (Convert.ToInt32(txtTotalEstime.Text)-ConsoDejaFacturee).ToString();
       
        }

        
        private void ChargerDecision()
        {
            try
            {
                FraudeServiceClient client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                client.SelectAllDecisionCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_Decision.ItemsSource = null;
                        Cbo_Decision.DisplayMemberPath = "Libelle";
                        Cbo_Decision.SelectedValuePath = "PK_ID";
                        Cbo_Decision.ItemsSource = args.Result;


                    }
                };
                client.SelectAllDecisionAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GetData(List<CsAppareilUtiliserFrd> _AppareilUtiliserFrd)
        {
            try
            {
                //UcRemisesScelles ctrl = new UcRemisesScelles();
                //ctrl.Closed += new EventHandler(RafraichirList);
                //ctrl.Show();
          
                    DonnesDatagrid.Clear();
                    if (_AppareilUtiliserFrd != null && _AppareilUtiliserFrd.Count > 0)
                    {
                        foreach (var item in _AppareilUtiliserFrd)
                        {
                            item.estimee = item.Mensuelle* nombreMois;
                            estime = item.estimee + estime;
                            DonnesDatagrid.Add(item);
                        }
                    }
                    dtgrdAnnalyse.ItemsSource = DonnesDatagrid;
                    txtConsommationEstimeeEquipement.Text = estime.ToString();
                    this.txtTotalEstime.Text = ((float)estime).ToString();

               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!VerifieChampObligation()) return;
            Recuperer(LaDemande);
            Validationdemande(LaDemande);
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void txtConsommationDejaFacturee_TextChanged(object sender, TextChangedEventArgs e)
        {
            int dejaFacturée = 0, estiméeEquipement = 0, retrogradation = 0;
            if (this.txtConsommationDejaFacturee.Text != string.Empty)
                dejaFacturée = int.Parse(this.txtConsommationDejaFacturee.Text.Trim());
            if (this.txtConsommationEstimeeEquipement.Text != string.Empty)
                estiméeEquipement = int.Parse(this.txtConsommationEstimeeEquipement.Text.Trim());
            if (this.txtRetrogradation.Text != string.Empty)
                retrogradation = int.Parse(this.txtRetrogradation.Text.Trim());

            this.txtTotalEstime.Text = (estiméeEquipement + retrogradation).ToString();
            this.txtConsommationAFacturer.Text = (estiméeEquipement + retrogradation - dejaFacturée).ToString();

        }
        private void Validationdemande(CsDemandeFraude LaDemande)
        {
            try
            {
                FraudeServiceClient Client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude")); ;
                Client.ValiderDemandeConsommationCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (b.Result != null)
                    {
                        Message.Show("Affectation effectuée avec succès", "Information");
                        List<int> Listid = new List<int>();
                        Listid.Add(LaDemande.LaDemande.PK_ID);
                        EnvoyerDemandeEtapeSuivante(Listid);
                        this.DialogResult = true;



                    }
                    else
                        Message.ShowError("Erreur a la cloture de la demande", "Cloturedemande");
                };
                Client.ValiderDemandeConsommationAsync(LaDemande);

            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, "Transmit");
            }
        }
        private void EditerFacture(CsDemandeFraude LaDemande)
        {
            //List<CsEditionFactureFraude> lstFacture = new List<CsEditionFactureFraude>();
            //CsEditionFactureFraude Factures = new CsEditionFactureFraude();
            //Factures.Libelle = "TrancheFraude";
            //lstFacture.Add(Factures);
            //foreach (var item in LaDemande.TrancheFraude)
            //{
            //    CsEditionFactureFraude Facture = new CsEditionFactureFraude();
            //    Facture.Centre = LaDemande.ClientFraude.Centre;
            //    Facture.Client = LaDemande.ClientFraude.Client;
            //    Facture.Ordre = LaDemande.ClientFraude.Ordre;

            //    Facture.Commune = LaDemande.ClientFraude.Commune;
            //    Facture.Quartier = LaDemande.ClientFraude.Quartier;
            //    Facture.Rue = LaDemande.ClientFraude.Rue;
            //    Facture.Porte = LaDemande.ClientFraude.Porte;

            //    Facture.Libelle = item.LIBELLE;
            //    Facture.Quantite  = item.Quantite ;
            //    Facture.MontantHT = item.MontantHT;
            //    Facture.MontantTTC = item.MontantTTC;
            //    Facture.MontantTva = item.MontantTva;
            //    lstFacture.Add(Facture);
  
            //}
            //CsEditionFactureFraude FacturesPsEDm = new CsEditionFactureFraude();
            //Factures.Libelle = "PrestastionEdm ";
            //lstFacture.Add(FacturesPsEDm);
            //foreach (var item in LaDemande.PrestastionEdm )
            //{
            //    CsEditionFactureFraude Facture = new CsEditionFactureFraude();
            //    Facture.Centre = LaDemande.ClientFraude.Centre;
            //    Facture.Client = LaDemande.ClientFraude.Client;
            //    Facture.Ordre = LaDemande.ClientFraude.Ordre;

            //    Facture.Commune = LaDemande.ClientFraude.Commune;
            //    Facture.Quartier = LaDemande.ClientFraude.Quartier;
            //    Facture.Rue = LaDemande.ClientFraude.Rue;
            //    Facture.Porte = LaDemande.ClientFraude.Porte;

            //    Facture.Libelle = item.Libelle ;
            //    Facture.Quantite = item.Quantite;
            //    Facture.MontantHT = item.MontantHT;
            //    Facture.MontantTTC = item.MontantTTC;
            //    Facture.MontantTva = item.MontantTva;
            //    lstFacture.Add(Facture);
            //}
            //CsEditionFactureFraude FacturesPdR = new CsEditionFactureFraude();
            //Factures.Libelle = "PrestationRemboursable ";
            //lstFacture.Add(FacturesPdR);
            //foreach (var item in LaDemande.PrestationRemboursable)
            //{
            //    CsEditionFactureFraude Facture = new CsEditionFactureFraude();
            //    Facture.Centre = LaDemande.ClientFraude.Centre;
            //    Facture.Client = LaDemande.ClientFraude.Client;
            //    Facture.Ordre = LaDemande.ClientFraude.Ordre;

            //    Facture.Commune = LaDemande.ClientFraude.Commune;
            //    Facture.Quartier = LaDemande.ClientFraude.Quartier;
            //    Facture.Rue = LaDemande.ClientFraude.Rue;
            //    Facture.Porte = LaDemande.ClientFraude.Porte;

            //    Facture.Libelle = item.Libelle;
            //    Facture.Quantite = item.Quantite;
            //    Facture.MontantHT = item.MontantHT;
            //    Facture.MontantTTC = item.MontantTTC;
            //    Facture.MontantTva = item.MontantTva;
            //    lstFacture.Add(Facture);
            //}
            //CsEditionFactureFraude FacturesRegl = new CsEditionFactureFraude();
            //Factures.Libelle = "Regularisation";
            //lstFacture.Add(FacturesRegl);
            //foreach (var item in LaDemande.Regularisation)
            //{
            //    CsEditionFactureFraude Facture = new CsEditionFactureFraude();
            //    Facture.Centre = LaDemande.ClientFraude.Centre;
            //    Facture.Client = LaDemande.ClientFraude.Client;
            //    Facture.Ordre = LaDemande.ClientFraude.Ordre;

            //    Facture.Commune = LaDemande.ClientFraude.Commune;
            //    Facture.Quartier = LaDemande.ClientFraude.Quartier;
            //    Facture.Rue = LaDemande.ClientFraude.Rue;
            //    Facture.Porte = LaDemande.ClientFraude.Porte;

            //    Facture.Libelle = item.Libelle;
            //    Facture.Quantite = item.Quantite;
            //    Facture.MontantHT = item.MontantHT;
            //    Facture.MontantTTC = item.MontantTTC;
            //    Facture.MontantTva = item.MontantTva;
            //    lstFacture.Add(Facture);
            //}
           
        }

        private bool VerifieChampObligation()
        {
            try
            {
                bool ReturnValue = true;



                #region information abonnement

                //if (string.IsNullOrEmpty(this.txt_Abonne.Text))
                //    throw new Exception("Remplir le Champs Abonne");


                //if (Chek_Identifia.IsChecked == true)
                //{

                //    if (string.IsNullOrEmpty(this.txt_Contact.Text))
                //        throw new Exception("Remplir le Champs  le Contact ");

                //    if (Cbo_MoyenDenociation.SelectedItem == null)
                //        throw new Exception("Selectionnez Moyen Denociation ");
                //}
                //if (string.IsNullOrEmpty(this.txt_Abonne.Text))
                //    throw new Exception("Remplir le Champs  Abonne");
                //if (Chek_Identifia.IsChecked == true)
                //{

                //    //if (string.IsNullOrEmpty(this.txt_LienAbonne.Text))
                //    //    throw new Exception("Remplir le Champs  Abonnée ");
                //    if (string.IsNullOrEmpty(this.txt_Identite.Text))
                //        throw new Exception("Remplir le Champs  Abonnée ");
                //    //if (string.IsNullOrEmpty(this.txt_Identite.Text))
                //    //    throw new Exception("Remplir le Champs  Abonnée ");
                //    //if (Cbo_MoyenDenociation.SelectedItem == null)
                //    //    throw new Exception("Selectionnez Moyen Denociation ");
                //}
                if (Cbo_Decision.SelectedItem == null)
                    throw new Exception("Selectionnez Decision");
                //if (Cbo_AnormlieCompteur.SelectedItem == null)
                //    throw new Exception("Selectionnez Anormalie Compteur ");
                //if (Cbo_CalibreCompteur.SelectedItem == null)
                //    throw new Exception("Selectionnez Calibre Compteur ");

                //if (Cbo_CalibreDijoncteur.SelectedItem == null)
                //    throw new Exception("Selectionnez Calibre Dijoncteur ");
                //if (Cbo_Fils.SelectedItem == null)
                //    throw new Exception("Selectionnez Calibre Fils ");
                //if (Cbo_MarqueCmpt.SelectedItem == null)
                //    throw new Exception("Selectionnez marque Compteur ");
                //if (Cbo_MArqueDijoncteur.SelectedItem == null)
                //    throw new Exception("Selectionnez marque Dijoncteur ");

                //if (Cbo_NbresfilsDijoncteur.SelectedItem == null)
                //    throw new Exception("Selectionnez nombres de fils rque Dijoncteur ");
                //if (Cbo_ReglageCmpt.SelectedItem == null)
                //    throw new Exception("Selectionnez reglagle compteur");
                //if (Cbo_usage.SelectedItem == null)
                //    throw new Exception("Selectionnez usage");
                //if (Cbo_Produit.SelectedItem == null)
                //    throw new Exception("Selectionnez Produit");




                //if (string.IsNullOrEmpty(this.txt_Identite.Text))
                //    throw new Exception("remplir le coffre Fusile ");

                //if (string.IsNullOrEmpty(this.txt_certifiplombage.Text))
                //    throw new Exception("remplir le certifie plombage ");

                //if (string.IsNullOrEmpty(this.txt_refeplombs.Text))
                //    throw new Exception("remplir referend plomgs ");

                //if (string.IsNullOrEmpty(this.txt_reference_plombs.Text))
                //    throw new Exception("remplir referend plomgs ");
                //if (string.IsNullOrEmpty(this.DateAbonnemnt.SelectedDate.ToString()))
                //    throw new Exception("remplir la date ");
                //if (string.IsNullOrEmpty(this.DateBranchemnt.SelectedDate.ToString()))
                //    throw new Exception("remplir la date ");

                //if (string.IsNullOrEmpty(this.txt.Text))
                //        throw new Exception("remplir referend plomgs ");

                //if (((CsProduit)Cbo_Produit.SelectedItem).CODE != SessionObject.Enumere.ElectriciteMT)
                //{
                //    if (string.IsNullOrEmpty(this.txt_Reglage.Text))
                //        throw new Exception("Selectionnez le calibre ");
                //}
                #endregion
                //#region Adresse géographique
                //if (Cbo_Centre.SelectedItem == null)
                //    throw new Exception("Selectionnez Centre ");

                //if (string.IsNullOrEmpty(this.txtCentre.Text))
                //    throw new Exception("Séléctionnez le Centre ");

                ////if (string.IsNullOrEmpty(this.txt_Quartier.Text))
                ////    throw new Exception("Séléctionnez le quartier ");
                //#endregion

                return ReturnValue;

            }
            catch (Exception ex)
            {
                //this.BtnTRansfert.IsEnabled = true;
                this.OKButton.IsEnabled = true;
                Message.ShowInformation(ex.Message, "Accueil");
                return false;
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
        private void Recuperer(CsDemandeFraude LaDemande)
        {
            try
            {

                LaDemande.Fraude.FK_IDDECISIONFRAUDE = ((CsDecisionfrd)Cbo_Decision.SelectedItem).PK_ID;
                LaDemande.Fraude.IsFraudeConfirmee = this.ckbFraudeConfirmée.IsChecked;
                LaDemande.Fraude.DateEtape = DateTime.Now.Date;
                if (this.txtConsommationAFacturer.Text != string.Empty)
                    LaDemande.ConsommationFrd.ConsommationAFacturer = int.Parse(this.txtConsommationAFacturer.Text.Trim());
                if (this.txtConsommationEstimeeEquipement.Text != string.Empty)
                    LaDemande.ConsommationFrd.ConsommationEstimee = int.Parse(this.txtConsommationEstimeeEquipement.Text.Trim());
                if (this.txtRetrogradation.Text != string.Empty)
                    LaDemande.ConsommationFrd.ConsommationRetrogradation = int.Parse(this.txtRetrogradation.Text.Trim());
                if (this.txtConsommationDejaFacturee.Text != string.Empty)
                    LaDemande.ConsommationFrd.ConsommationDejaFacturee = int.Parse(this.txtConsommationDejaFacturee.Text.Trim());
                LaDemande.ConsommationFrd.NombreMoisAFacturer = this.nombreMois;
                if (LaDemande.ConsommationFrd.NombreMoisAFacturer > 0)
                {
                    float dividende = float.Parse(LaDemande.ConsommationFrd.ConsommationAFacturer.ToString()) / LaDemande.ConsommationFrd.NombreMoisAFacturer;
                    LaDemande.ConsommationFrd.ConsommationMensuelleAFacturer = Convert.ToInt32(dividende);
                }

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        //private CsCompteurFraude GetInformationsFromScreen()
        //{
        //    var listObjetForInsertOrUpdate = new CsCompteurFraude();


        //   // listObjetForInsertOrUpdate = sCompteurFraude;


        //    return listObjetForInsertOrUpdate;
        
        //}

    }
}


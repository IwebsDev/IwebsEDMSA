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
    public partial class UcFacturation : ChildWindow
    {
        ObservableCollection<CsTrancheFraude> DonnesDatagridTfrd = new ObservableCollection<CsTrancheFraude>();
        ObservableCollection<CsPrestastionEdm> DonnesDatagridPrstEdm = new ObservableCollection<CsPrestastionEdm>();
        ObservableCollection<CsPrestationRemboursable> DonnesDatagridPrstRembs = new ObservableCollection<CsPrestationRemboursable>();
        ObservableCollection<CsRegularisation> DonnesDatagridRegul = new ObservableCollection<CsRegularisation>();
        List<CsTrancheFraude> listTrancheFraudeGrid = new List<CsTrancheFraude>();
        List<CsPrestastionEdm> listPrestastionEdmGrid = new List<CsPrestastionEdm>();
        List<CsPrestationRemboursable> listPrestationRemboursableGrid = new List<CsPrestationRemboursable>();
        List<CsRegularisation> listRegularisationGrid = new List<CsRegularisation>();
        CsTrancheFraude TrancheFrd = new CsTrancheFraude();
        CsDemandeFraude LaDemande = new CsDemandeFraude();
        int EtapeActuelle, MontantHT;
        int iResteConso = 0;
        int tvaTranche, tvaPrestEdm = 0;
        public UcFacturation()
        { 
            InitializeComponent();
        }
        public UcFacturation(CsDemandeFraude Demande, int EtapeWk)
        {
             InitializeComponent();
            LaDemande = Demande;
            EtapeActuelle = EtapeWk;
            MontantHT = 0;
            Initialisation(Demande);
        
            //// DonnesDatagridTfrd.Add(listTrancheFraudeGrid);
            // dtgrdTrancheFrd.ItemsSource = listTrancheFraudeGrid;
            //dtgrdPrestEDM.ItemsSource = DonnesDatagridPrstEdm;
            //dtgrdPrestRemboursmnt.ItemsSource = DonnesDatagridPrstRembs;
            //dtgrdPrestRegulation.ItemsSource = DonnesDatagridRegul;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Recuperer();
            //UcSuiteTraitement Newfrm = new UcSuiteTraitement(LaDemande, EtapeActuelle);
            //Newfrm.Show();
           // this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        private void Initialisation(CsDemandeFraude LaDemande)
        {
                try
                {

                    if (LaDemande != null)
                    {
                        this.txtConsommationAFacturer.Text =Arrondir( this.LaDemande.ConsommationFrd.ConsommationAFacturer).ToString();
                        this.txtConsommationEstimee.Text =Arrondir( this.LaDemande.ConsommationFrd.ConsommationEstimee).ToString();
                        this.txtConsommationDejaFacturee.Text =Arrondir( this.LaDemande.ConsommationFrd.ConsommationDejaFacturee).ToString();
                        this.txtNbreMois.Text =Arrondir( this.LaDemande.ConsommationFrd.NombreMoisAFacturer).ToString();
                        this.txtConsommationMensuelle.Text = Arrondir(this.LaDemande.ConsommationFrd.ConsommationMensuelleAFacturer).ToString();
                        this.txtTaxe.Text = ((Decimal)(LaDemande.ConsommationFrd.TauxTVA * 10000) / 100).ToString() + "%";
                      
                        GetDataTrfrd(LaDemande.TrancheFraude);
                        GetDataPrestastionEdm(LaDemande.PrestastionEdm);
                        GetDataPrestastionRemboursable(LaDemande.PrestationRemboursable);
                        GetDataREGULARISATION(LaDemande.Regularisation);
                        if (this.LaDemande.ConsommationFrd.IsFactureAuForfait)
                        {
                            this.txtMontantForfait.Text = this.LaDemande.ConsommationFrd.MontantFactureTTC.ToString();
                            this.txtMontantForfait.IsEnabled = true;
                            this.lblForfait.IsEnabled = true;
                        }
                        else
                        {
                            this.txtMontantForfait.IsEnabled = false;
                            this.lblForfait.IsEnabled = false;
                        }


                        if (LaDemande.ConsommationFrd.IsFactureAuForfait)
                        {
                            this.txtMontantForfait.Text = LaDemande.ConsommationFrd.MontantFactureTTC.ToString();
                            this.txtMontantForfait.IsEnabled = true;
                            this.lblForfait.IsEnabled = true;
                        }
                        else
                        {
                            this.txtMontantForfait.IsEnabled = false;
                            this.lblForfait.IsEnabled = false;
                        }
                    
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }

          

        }

        private void GetDataTrfrd(List<CsTrancheFraude> _TrancheFraude)
        {
            try
            {
                //UcRemisesScelles ctrl = new UcRemisesScelles();
                //ctrl.Closed += new EventHandler(RafraichirList);
                //ctrl.Show();
                this.MontantHT = 0;
                if (_TrancheFraude != null && _TrancheFraude.Count > 0)
                {
                    DonnesDatagridTfrd.Clear();

                    foreach (var item in _TrancheFraude)
                    { 
                        if(item.Quantite==0)
                        item.Quantite = (int)item.CONSOMAXI;
                        item.MontantHT = Arrondir(Convert.ToInt32(Convert.ToDecimal(item.CONSOMAXI) * Convert.ToDecimal(item.PRIXUNITAIRE)));
                        item.MontantTva =Arrondir( Convert.ToInt32(LaDemande.ConsommationFrd.TauxTVA * 10000 * item.MontantHT / 10000));
                        item.MontantTTC =Arrondir( Convert.ToInt32(this.LaDemande.ConsommationFrd.TauxTVA * 10000 * item.MontantTva / 10000));
                        this.MontantHT += Arrondir(item.MontantHT);

                        DonnesDatagridTfrd.Add(item);
                    }
                }
                dtgrdTrancheFrd.ItemsSource = DonnesDatagridTfrd;
                this.txtMontantHTConsommationMensuelle.Text = this.MontantHT.ToString();
                this.txtMontantHTConsommationAnnuelle.Text = Arrondir((this.MontantHT * this.LaDemande.ConsommationFrd.NombreMoisAFacturer)).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDataPrestastionEdm(List<CsPrestastionEdm> _PrestastionEdm)
        {
            try
            {
                //UcRemisesScelles ctrl = new UcRemisesScelles();
                //ctrl.Closed += new EventHandler(RafraichirList);
                //ctrl.Show();
                this.MontantHT = 0;
                DonnesDatagridPrstEdm.Clear();
                if (_PrestastionEdm != null && _PrestastionEdm.Count > 0)
                {
                    foreach (var item in _PrestastionEdm)
                    {
                        if (item.Quantite == null)
                            item.Quantite = 0;
                        item.MontantHT = Convert.ToInt32(item.PrixUnitaire * item.Quantite);
                        item.MontantTva =Convert.ToInt32((this.LaDemande.ConsommationFrd.TauxTVA* 10000 * item.MontantHT) / 10000);
                        item.MontantTTC = Arrondir(item.MontantHT + item.MontantTva);
                        this.MontantHT += Arrondir(item.MontantHT);
                        DonnesDatagridPrstEdm.Add(item);
                    }
                }
                dtgrdPrestEDM.ItemsSource = DonnesDatagridPrstEdm;
                this.txtMontantHTPrestation.Text = Arrondir(this.MontantHT).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDataPrestastionRemboursable(List<CsPrestationRemboursable> _PrestationRemboursable)
        {
            try
            {
                //UcRemisesScelles ctrl = new UcRemisesScelles();
                //ctrl.Closed += new EventHandler(RafraichirList);
                //ctrl.Show();
                this.MontantHT = 0;
                DonnesDatagridPrstRembs.Clear();
                if (_PrestationRemboursable != null && _PrestationRemboursable.Count > 0)
                {
                    foreach (var item in _PrestationRemboursable)
                    {
                        if (item.Quantite == null)
                            item.Quantite = 0;
                        item.MontantHT =Arrondir( Convert.ToInt32(Convert.ToDecimal(item.PrixUnitaire) * Convert.ToDecimal(item.Quantite)));
                        item.MontantTva = Convert.ToInt32((this.LaDemande.ConsommationFrd.TauxTVA * 10000 * item.MontantHT) / 10000);
                        item.MontantTTC =Arrondir( item.MontantHT + item.MontantTva);
                        this.MontantHT += Arrondir(item.MontantHT);
                        DonnesDatagridPrstRembs.Add(item);
                    }
                }
                dtgrdPrestRemboursmnt.ItemsSource = DonnesDatagridPrstRembs;
                this.txtMontantHTPrestationRemboursable.Text = Arrondir(this.MontantHT).ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDataREGULARISATION(List<CsRegularisation> _Regularisation)
        {
            try
            {
                //UcRemisesScelles ctrl = new UcRemisesScelles();
                //ctrl.Closed += new EventHandler(RafraichirList);
                //ctrl.Show();
                this.MontantHT = 0;
                DonnesDatagridRegul.Clear();
                if (_Regularisation != null && _Regularisation.Count > 0)
                {
                    foreach (var item in _Regularisation)
                    {
                        if (item.Quantite == null)
                            item.Quantite = 0;
                        item.MontantHT =Arrondir( Convert.ToInt32(item.PrixUnitaire * item.Quantite));
                        item.MontantTva =Arrondir( Convert.ToInt32(this.LaDemande.ConsommationFrd.TauxTVA * 10000 * item.MontantHT) / 10000);
                        item.MontantTTC =Arrondir( item.MontantHT + item.MontantTva) ;
                        this.MontantHT += Arrondir(item.MontantHT);
                        DonnesDatagridRegul.Add(item);
                    }
                }
                dtgrdPrestRegulation.ItemsSource = DonnesDatagridRegul;
                this.txtMontantHTRegularisation.Text = Arrondir(this.MontantHT).ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dtgrdTrancheFrd_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            listTrancheFraudeGrid = ((ObservableCollection<CsTrancheFraude>)dtgrdTrancheFrd.ItemsSource).ToList();
            GetDataTrfrd(listTrancheFraudeGrid);
        }

        private void dtgrdPrestRemboursmnt_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            listPrestationRemboursableGrid = ((ObservableCollection<CsPrestationRemboursable>)dtgrdPrestRemboursmnt.ItemsSource).ToList();
            GetDataPrestastionRemboursable(listPrestationRemboursableGrid);
        }

        private void dtgrdPrestEDM_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            listPrestastionEdmGrid = ((ObservableCollection<CsPrestastionEdm>)dtgrdPrestEDM.ItemsSource).ToList();
            GetDataPrestastionEdm(listPrestastionEdmGrid);
        }

        private void dtgrdPrestRegulation_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            listRegularisationGrid = ((ObservableCollection<CsRegularisation>)dtgrdPrestRegulation.ItemsSource).ToList();
            GetDataREGULARISATION(listRegularisationGrid);
        }

        private void Recuperer()
        {
           //la a  je recupere  tout les inofr de uc
             LaDemande.Fraude.DateEtape = DateTime.Now.Date;
             LaDemande.ConsommationFrd.ConsommationAFacturer = int.Parse(this.txtConsommationAFacturer.Text.Trim());
             LaDemande.ConsommationFrd.ConsommationEstimee = int.Parse(this.txtConsommationEstimee.Text.Trim());
             LaDemande.ConsommationFrd.ConsommationDejaFacturee = int.Parse(this.txtConsommationDejaFacturee.Text.Trim());
             LaDemande.ConsommationFrd.MontantHTConsommation = int.Parse(this.txtMontantHTConsommationAnnuelle.Text.Trim());
             LaDemande.ConsommationFrd.MontantHTPrestationEDM = int.Parse(this.txtMontantHTPrestation.Text.Trim());
             LaDemande.ConsommationFrd.MontantHTPrestationRemboursable = int.Parse(this.txtMontantHTPrestationRemboursable.Text.Trim());
             LaDemande.ConsommationFrd.MontantHTRegularisationDevis = int.Parse(this.txtMontantHTRegularisation.Text.Trim());
             if (LaDemande.ConsommationFrd.IsFactureAuForfait==true)
                 LaDemande.ConsommationFrd.MontantFactureTTC = Arrondir(float.Parse(this.txtMontantForfait.Text.Trim()));
            else
            {
                LaDemande.ConsommationFrd.MontantFactureTTC = Arrondir(float.Parse(txtTotalFactureTTC.Text.Trim()));
                LaDemande.ConsommationFrd.IsFactureAnnulee = false;
                LaDemande.ConsommationFrd.IsFactureAuForfait = false;
            }

     


            ////Tranches
             LaDemande.DETAILparTRANCHE= new List<CsDETAILparTRANCHE>();
             LaDemande.DETAILparTRANCHE.Clear();
            listTrancheFraudeGrid = ((ObservableCollection<CsTrancheFraude>)dtgrdTrancheFrd.ItemsSource).ToList();
            tvaTranche = 0;
            if (listTrancheFraudeGrid.Count > 0 && listTrancheFraudeGrid != null)
            {
                foreach (CsTrancheFraude item in listTrancheFraudeGrid)
                {
                    CsDETAILparTRANCHE tranche = new CsDETAILparTRANCHE();
                    CsTrancheFraude tr;
                    if (item.Quantite == 0) continue;
                       
                            tranche.FK_IDTRANCHEFRAUDE = (int)item.PK_ID;
                            tranche.FK_IDCONSOMMATION = this.LaDemande.ConsommationFrd.PK_ID;
                            tranche.FK_IDPRODUIT = (int)LaDemande.CompteurFraude.FK_IDPRODUIT;

                            tvaTranche += item.MontantTva;

                         //   tr = new DBTRANCHE().GetById(null, tranche.IdTranche, 0, 1);
                          tr = LaDemande.TrancheFraude.FirstOrDefault(c=>c.PK_ID==item.PK_ID);
                            if ((tr.NUMTRANCHE == 1) && (item.MontantTva == 0))
                                tranche.IsExonereeTVA = true;
                            tranche.ConsommationTranche = item.Quantite;
                            tranche.PrixUnitaire = Convert.ToDecimal(item.PRIXUNITAIRE);
                          LaDemande.DETAILparTRANCHE.Add(tranche);
                        }
                }
            
                 LaDemande.ConsommationFrd.MontantTVAConsommation = this.tvaTranche * LaDemande.ConsommationFrd.NombreMoisAFacturer;

            //Prestations EDM
             this.LaDemande.DetailParPresentationEdm = new List<CsDetailParPresentationEdm>();
            this. LaDemande.DetailParPresentationEdm.Clear();
            listPrestastionEdmGrid = ((ObservableCollection<CsPrestastionEdm>)dtgrdPrestEDM.ItemsSource).ToList();

            tvaPrestEdm = 0;
            if (listPrestastionEdmGrid.Count > 0 && listPrestastionEdmGrid != null)
            {

                foreach (CsPrestastionEdm item in listPrestastionEdmGrid)
                {
                    CsDetailParPresentationEdm prestEdm = new CsDetailParPresentationEdm(); ;

                    if (item.Quantite == 0) continue;
                    
                     
                        prestEdm.FK_IDPRESTATIONEDM = item.PK_ID;
                        prestEdm.FK_IDCONSOMMATION = this.LaDemande.ConsommationFrd.PK_ID;
                        prestEdm.FK_IDPRODUIT = (int)LaDemande.CompteurFraude.FK_IDPRODUIT;

                        tvaPrestEdm += item.MontantTva;


                        prestEdm.NombrePrestation =(int) item.Quantite;
                        prestEdm.PrixUnitaire = Convert.ToDecimal(item.PrixUnitaire);
                        LaDemande.DetailParPresentationEdm.Add(prestEdm);
                     
                }
            }

            //Prestations remboursables
            this.LaDemande.DETAILparPRESTATIONREMBOURSABLE = new List<CsDETAILparPRESTATIONREMBOURSABLE>();
            this.LaDemande.DETAILparPRESTATIONREMBOURSABLE.Clear();
            listPrestationRemboursableGrid = ((ObservableCollection<CsPrestationRemboursable>)dtgrdPrestRemboursmnt.ItemsSource).ToList();

            int tvaPrestRem = 0;
            if (listPrestationRemboursableGrid.Count > 0 && listPrestationRemboursableGrid != null)
            {
                foreach (CsPrestationRemboursable item in listPrestationRemboursableGrid)
                {
                    CsDETAILparPRESTATIONREMBOURSABLE prestRemb = new CsDETAILparPRESTATIONREMBOURSABLE();
                    if (item.Quantite == 0) continue;

                    
                       
                        prestRemb.FK_IDPRESTATIONREMBOURSABLE = item.PK_ID;
                        prestRemb.FK_IDPRODUIT =this.LaDemande.ConsommationFrd.PK_ID;
                        prestRemb.FK_IDCONSOMMATION =  this.LaDemande.ConsommationFrd.PK_ID;

                        tvaPrestRem += item.MontantTva;

                        prestRemb.NombrePrestation =(int) item.Quantite;
                        prestRemb.PrixUnitaire = item.PrixUnitaire;
                        this.LaDemande.DETAILparPRESTATIONREMBOURSABLE.Add(prestRemb);
                   
                }
            }

            //Régularisations
            this.LaDemande.DETAILparREGULARISATION = new List<CsDETAILparREGULARISATION>();
            this.LaDemande.DETAILparREGULARISATION.Clear();
            listRegularisationGrid = ((ObservableCollection<CsRegularisation>)dtgrdPrestRegulation.ItemsSource).ToList();

            int tvaReg = 0;
            if (listRegularisationGrid.Count > 0 && listRegularisationGrid != null)
            {
                foreach (CsRegularisation item in listRegularisationGrid)
                {
                    CsDETAILparREGULARISATION reg = new CsDETAILparREGULARISATION();
                    if (item.Quantite == 0) continue;

                   
                        reg.FK_IDREGULARISATION = item.PK_ID;
                        reg.FK_IDPRODUIT = this.LaDemande.ConsommationFrd.PK_ID;
                        reg.FK_IDCONSOMMATION = this.LaDemande.ConsommationFrd.PK_ID;

                        tvaReg += item.MontantTva;


                        reg.NombrePrestation = item.Quantite;
                        reg.PrixUnitaire = item.PrixUnitaire;
                        LaDemande.DETAILparREGULARISATION.Add(reg);
                     
                }
            }

            int tva = this.LaDemande.ConsommationFrd.MontantTVAConsommation + tvaPrestEdm + tvaPrestRem + tvaReg;
            Editerfacture();
            //UcSuiteTraitement frm = new UcSuiteTraitement(this.myProduit, this.myConsommation.MontantFactureTTC, this.myFacture.Exigibilite.Date, this.myConsommation.IsFactureAuForfait);
            UcSuiteTraitement Newfrm = new UcSuiteTraitement(LaDemande, EtapeActuelle, tva);
            Newfrm.Show();
            DialogResult = true;
            
           

        }
        private void Editerfacture()
        {
            List<CsEditionFactureFd> lstFacture = new List<CsEditionFactureFd>();
            CsEditionFactureFd leEntete = new CsEditionFactureFd();
            leEntete.Centre = LaDemande.ClientFraude.Centre;
            leEntete.Client = LaDemande.ClientFraude.Client + "   " + LaDemande.ClientFraude.Ordre ;
            leEntete.Ordre = LaDemande.ClientFraude.Ordre;
            leEntete.Nomabon = LaDemande.ClientFraude.Nomabon;
            leEntete.Commune = LaDemande.ClientFraude.Commune;
            leEntete.Puissance  =LaDemande.ClientFraude.PuissanceSouscrite != null ? LaDemande.ClientFraude.PuissanceSouscrite .ToString():string.Empty ;
            leEntete.Quartier = LaDemande.ClientFraude.Quartier;
            leEntete.Rue = LaDemande.ClientFraude.Rue;
            leEntete.Porte = LaDemande.ClientFraude.Porte;
            leEntete.Produit = LaDemande.CompteurFraude.libelle_Produit;
            leEntete.Usage = LaDemande.CompteurFraude.libelle_usage;
            leEntete.FicheTraitement = LaDemande.Fraude.FicheTraitement;
            leEntete.Duree = LaDemande.ConsommationFrd.NombreMoisAFacturer.ToString();
            leEntete.Calibre = LaDemande.CompteurFraude.libelle_Calibre;
            leEntete.Source = LaDemande.Fraude.LIBELLESOURCECONTROLE ;
       
            leEntete.DateControle = LaDemande.Controle.DateControle.ToShortDateString();
            leEntete.Pv_Controle = LaDemande.Fraude.FicheTraitement ;
            leEntete.NumeroCompteur = LaDemande.CompteurFraude.NumeroCompteur;
            leEntete.Index = LaDemande.CompteurFraude.IndexCompteur.ToString();
            leEntete.AnnomalieBranchement = LaDemande.CompteurFraude.libelle_AnnomalieBranchement;
            leEntete.AnnomalieCompteur = LaDemande.CompteurFraude.libelle_AnnomalieCompteur;
            leEntete.AutreAnnomalie = LaDemande.CompteurFraude.libelle_AutreAnnomalie;
            leEntete.ConsommationAFacturer = LaDemande.ConsommationFrd.ConsommationAFacturer ;
            leEntete.ConsommationDejaFacturee = LaDemande.ConsommationFrd.ConsommationDejaFacturee;
            leEntete.ConsommationEstimee = LaDemande.ConsommationFrd.ConsommationEstimee;
            leEntete.ConsommationMensuelleAFacturer = LaDemande.ConsommationFrd.ConsommationMensuelleAFacturer;
            leEntete.ConsommationRetrogradation = LaDemande.ConsommationFrd.ConsommationRetrogradation;
            leEntete.MontantTotal = LaDemande.ConsommationFrd.MontantFactureTTC;
            leEntete.MontantTotalConso = LaDemande.ConsommationFrd.MontantHTConsommation;
            leEntete.MontantLettre = Shared.ChiffresEnLettres.AmountInWords((float)LaDemande.ConsommationFrd.MontantFactureTTC, SessionObject.Devise, SessionObject.Centime, "fr-FR");

            int etapeEdition = 0;
            if (listTrancheFraudeGrid.Count > 0 && listTrancheFraudeGrid != null)
            {
                foreach (CsTrancheFraude item in listTrancheFraudeGrid.Where(t=>t.Quantite != 0).ToList())
                {
                     CsEditionFactureFd leElementFact = new  CsEditionFactureFd();
                    leElementFact = leEntete;
                    leElementFact.TypeEtat = "TrancheFraude";

                    leElementFact.Libelle = item.LIBELLE;
                    leElementFact.PrixUnitaire = item.PRIXUNITAIRE;
                    leElementFact.Quantite = item.Quantite;
                    leElementFact.MontantHT = item.MontantHT;
                    leElementFact.MontantTTC = item.MontantTTC;
                    leElementFact.MontantTva = item.MontantTva;

                    leElementFact.IdentificationUnique = etapeEdition.ToString() + 1;
                    lstFacture.Add(Shared.ClasseMEthodeGenerique.RetourneCopyObjet<CsEditionFactureFd>(leElementFact));
                }
            }
            if (listPrestastionEdmGrid.Count > 0 && listPrestastionEdmGrid != null)
            {
                etapeEdition = lstFacture.Count();
                foreach (CsPrestastionEdm item in listPrestastionEdmGrid.Where(t=>t.Quantite != 0).ToList())
                {
                     CsEditionFactureFd leElementFact = new  CsEditionFactureFd();
                    leElementFact = leEntete;
                    leElementFact.TypeEtat = "PrestastionEdm";

                    leElementFact.Libelle = item.Libelle;
                    leElementFact.PrixUnitaire = item.PrixUnitaire;

                    leElementFact.Quantite = item.Quantite;
                    leElementFact.MontantHT = item.MontantHT;
                    leElementFact.MontantTTC = item.MontantTTC;
                    leElementFact.MontantTva = item.MontantTva;

                    leElementFact.IdentificationUnique = etapeEdition.ToString() + 1;
                    lstFacture.Add(Shared.ClasseMEthodeGenerique.RetourneCopyObjet<CsEditionFactureFd>(leElementFact));

                }
            }
            if (listPrestationRemboursableGrid.Count > 0 && listPrestationRemboursableGrid != null)
            {
                etapeEdition = lstFacture.Count();

                foreach (CsPrestationRemboursable item in listPrestationRemboursableGrid.Where(t=>t.Quantite != 0).ToList())
                {
                     CsEditionFactureFd leElementFact = new  CsEditionFactureFd();
                    leElementFact = leEntete;
                    leElementFact.TypeEtat = "PrestationRemboursable";

                    leElementFact.Libelle = item.Libelle;
                    leElementFact.PrixUnitaire = item.PrixUnitaire;

                    leElementFact.Quantite = item.Quantite;
                    leElementFact.MontantHT = item.MontantHT;
                    leElementFact.MontantTTC = item.MontantTTC;
                    leElementFact.MontantTva = item.MontantTva;
                    leElementFact.IdentificationUnique = etapeEdition.ToString() + 1;

                    lstFacture.Add(Shared.ClasseMEthodeGenerique.RetourneCopyObjet<CsEditionFactureFd>(leElementFact));

                }
            }
            if (listRegularisationGrid.Count > 0 && listRegularisationGrid != null)
            {
                etapeEdition = lstFacture.Count();

                foreach (CsRegularisation item in listRegularisationGrid.Where(t=>t.Quantite != 0).ToList())
                {
                     CsEditionFactureFd leElementFact = new  CsEditionFactureFd();
                    leElementFact = leEntete;
                    leElementFact.TypeEtat = "Regularisation";

                    leElementFact.Libelle = item.Libelle;
                    leElementFact.PrixUnitaire = item.PrixUnitaire;

                    leElementFact.Quantite = item.Quantite;
                    leElementFact.MontantHT = item.MontantHT;
                    leElementFact.MontantTTC = item.MontantTTC;
                    leElementFact.MontantTva = item.MontantTva;
                    leElementFact.IdentificationUnique = etapeEdition.ToString() + 1;

                    lstFacture.Add(Shared.ClasseMEthodeGenerique.RetourneCopyObjet<CsEditionFactureFd>(leElementFact));
                }
            }
            LaDemande.factureFraudeEdition = new List<CsEditionFactureFd>();
            LaDemande.factureFraudeEdition.AddRange(lstFacture);

        }

        private void txtMontantHT_TextChanged(object sender, TextChangedEventArgs e)
        {
            int montantHT = 0;
            if (this.txtMontantHTConsommationAnnuelle.Text != string.Empty)
                montantHT += int.Parse(this.txtMontantHTConsommationAnnuelle.Text.Trim());
            if (this.txtMontantHTPrestation.Text != string.Empty)
                montantHT += int.Parse(this.txtMontantHTPrestation.Text.Trim());
            if (this.txtMontantHTPrestationRemboursable.Text != string.Empty)
                montantHT += int.Parse(this.txtMontantHTPrestationRemboursable.Text.Trim());
            if (this.txtMontantHTRegularisation.Text != string.Empty)
                montantHT += int.Parse(this.txtMontantHTRegularisation.Text.Trim());

            this.txtTotalFactureTTC.Text = float.Parse((montantHT + (montantHT * this.LaDemande.ConsommationFrd.TauxTVA)).ToString()).ToString();
    
        }

        private int Arrondir(float montant)
        {
            //string[] partie = montant.ToString().Split(new char[] { ',' });
            //if (partie.Length == 1)
            //    return int.Parse(partie[0]);
            //if (int.Parse(partie[1].Substring(0, 1)) >= 5)
            //    return int.Parse(partie[0]) + (int)1;
            //else
            //    return int.Parse(partie[0]);

            string[] partie = montant.ToString().Split(new char[] { ',' });
            if (partie.Length == 1)
            {
                string[] part = partie[0].ToString().Split(new char[] { '.' });
                return int.Parse(part[0]);
            }
            if (int.Parse(partie[1].Substring(0, 1)) >= 5)
            {
                string[] part = partie[0].ToString().Split(new char[] { '.' });
                return int.Parse(part[0]) + 1;
            }
            else
            {
                string[] part = partie[0].ToString().Split(new char[] { '.' });
                return int.Parse(part[0]);
            }

        }

    }
}


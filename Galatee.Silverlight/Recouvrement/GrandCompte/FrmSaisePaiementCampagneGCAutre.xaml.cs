using Galatee.Silverlight.Recouvrement.GrandCompte;
using Galatee.Silverlight.ServiceRecouvrement;
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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmSaisePaiementCampagneGCAutre : ChildWindow
    {

        #region Variables

        List<CsCampagneGc> ListCapagneGc = new List<CsCampagneGc>();
        CsCampagneGc LaCampagneGc = new CsCampagneGc();
        decimal Montant_Facture_RestAPayer;
        public bool CanShowWindows = true;
        CsDetailMandatementGc Facture_Payer_Partiellement = new CsDetailMandatementGc();
        List<CsDetailCampagneGc> DetailCampagneValider = new List<CsDetailCampagneGc>();
        List<CsDetailCampagneGc> DetailCampagneNonValider = new List<CsDetailCampagneGc>();
        List<CsPaiementGc> ListMandatementGc = new List<CsPaiementGc>();
        int IdCampagne = 0;
        List<int> ListIdCampagne = new List<int>();
        int EtapeActuel = 0;
        #endregion

        #region Constructeur

        public FrmSaisePaiementCampagneGCAutre()
        {
            InitializeComponent();
            RemplirCampagne(UserConnecte.matricule);
        }

        public FrmSaisePaiementCampagneGCAutre(int IdCampagne)
        {
            InitializeComponent();
            this.IdCampagne = IdCampagne;

            RemplirCampagne(UserConnecte.matricule);

        }

        public FrmSaisePaiementCampagneGCAutre(List<int> ListIdCampagne, int EtapeActuel)
        {
            InitializeComponent();
            this.IdCampagne = ListIdCampagne[0];
            this.EtapeActuel = EtapeActuel;
            RemplirCampagne(UserConnecte.matricule);

        }
        #endregion

        #region Event Handler

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            List<CsPaiementGc> LisPaieemntToSaveGc = new List<CsPaiementGc>();
            foreach (var item in ListMandatementGc)
            {
                if (item.DETAILCAMPAGNEGC_.Count > 0)
                {
                    LisPaieemntToSaveGc.Add(item);
                }
            }
            if (LisPaieemntToSaveGc.Count > 0)
            {
                SavePaiement(LisPaieemntToSaveGc);
            }
            else
            {
                Message.ShowWarning("Veuillez selectionner au moins une facture pour valider le paiement", "Information");
            }

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrmFactureHorRegroupement frm = new FrmFactureHorRegroupement();
            frm.CallBack += frm_CallBack1;
            frm.Show();
        }
        void frm_CallBack1(object sender, Tarification.Helper.CustumEventArgs e)
        {
            //Implementer le callback
            if (e.Bag != null)
            {
                var ListFacture = (List<CsLclient>)e.Bag;
                List<CsDetailCampagneGc> datasource = (List<CsDetailCampagneGc>)dg_facture.ItemsSource;
                if (datasource == null)
                {
                    datasource = new List<CsDetailCampagneGc>();
                }
                foreach (var item in ListFacture)
                {
                    CsDetailCampagneGc facture = new CsDetailCampagneGc();
                    facture.CENTRE = item.CENTRE;
                    facture.CLIENT = item.CLIENT;
                    facture.ORDRE = item.ORDRE;
                    facture.NOM = item.NOM;
                    facture.PERIODE = item.REFEM;
                    facture.MONTANT = item.MONTANT;
                    facture.NDOC = item.NDOC;
                    datasource.Add(facture);
                }
                dg_facture.ItemsSource = datasource.OrderBy(d => d.NOM).ToList();
            }
        }

        private void dg_facture_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }
        private void dg_facture_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
          
        }

        private void txt_MontantMandatement_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal MontaMandatement = 0;
            if (decimal.TryParse(txt_MontantMandatement.Text, out MontaMandatement))
            {
                if (MontaMandatement == ListCapagneGc[0].MONTANT)
                {
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow("Information", "Voulez vous lancer la validation automatique de toute les factures", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            chbx_ToutValider.IsChecked = true;
                        }
                        else
                        {
                            return;
                        }
                    };
                    messageBox.Show();
                }
            }
        }

        private void chbx_ToutValider_Checked(object sender, RoutedEventArgs e)
        {
            List<CsDetailCampagneGc> DetailCampagneGc = (List<CsDetailCampagneGc>)dg_facture.ItemsSource;

            for (int index = 0; index < DetailCampagneGc.Count; index++)
            {
                DetailCampagneGc[index].IsMontantValide = true;
                DetailCampagneGc[index].MONTANT_VERSER = DetailCampagneGc[index].MONTANT_RESTANT;
            }
            dg_facture.ItemsSource = DetailCampagneGc.OrderBy(d => d.NOM).ToList();
        }
        private void chbx_ToutValider_Unchecked(object sender, RoutedEventArgs e)
        {
            List<CsDetailCampagneGc> DetailCampagneGc = (List<CsDetailCampagneGc>)dg_facture.ItemsSource;
            foreach (var item in DetailCampagneGc)
            {
                item.IsMontantValide = false;
            }
            dg_facture.ItemsSource = DetailCampagneGc.OrderBy(d => d.NOM).ToList();
        }

        private void CheckBox_Unchecked_(object sender, RoutedEventArgs e)
        {
            var DetailCampagneSelectionner = (CsDetailCampagneGc)dg_facture.SelectedItem;
            if (DetailCampagneSelectionner != null)
            {
                DetailCampagneSelectionner.IsMontantNonValide = false;
                DetailCampagneNonValider.Remove(DetailCampagneSelectionner);
            }
        }
        private void CheckBox_Checked_(object sender, RoutedEventArgs e)
        {
            var DetailCampagneSelectionner = (CsDetailCampagneGc)dg_facture.SelectedItem;

            if (DetailCampagneSelectionner != null)
            {
                if (CanShowWindows)
                {
                    FrmMontantMandatementVerser Frm = new FrmMontantMandatementVerser(DetailCampagneSelectionner.PK_ID);
                    Frm.Closing += Frm_Closing;
                    Frm.Show();
                }
                CanShowWindows = true;

            }
        }

        void Frm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var DataSource = (List<CsDetailCampagneGc>)dg_facture.ItemsSource;
            var Frm = ((FrmMontantMandatementVerser)sender);
            if (Frm.montant > 0)
            {
                var DetailCampagneSelectionner = DataSource.FirstOrDefault(d => d.PK_ID == Frm.Pk_Id);
                int index = DataSource.IndexOf(DetailCampagneSelectionner);

                DetailCampagneSelectionner.MONTANT_VERSER = Frm.montant;

                DetailCampagneSelectionner.IsMontantValide = false;
                DetailCampagneSelectionner.IsMontantNonValide = true;

                DataSource[index] = DetailCampagneSelectionner;
                dg_facture.ItemsSource = DataSource.OrderBy(d => d.NOM).ToList();
                dg_facture.SelectedItem = DetailCampagneSelectionner;
            }
            else
            {
                var DetailCampagneSelectionner = (CsDetailCampagneGc)dg_facture.SelectedItem;

                DetailCampagneSelectionner.IsMontantValide = false;
                DetailCampagneSelectionner.IsMontantNonValide = true;

                DetailCampagneNonValider.Add(DetailCampagneSelectionner);
            }
            CanShowWindows = false;
            dg_facture_CellEditEnded(null, null);
        }

      
  
        private void dg_Paiement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn_Actualiser_Click(object sender, RoutedEventArgs e)
        {
            dg_Paiement.SelectedItem = null;

            if (dg_Campagne.SelectedItem != null)
            {
                var mand = ((CsMandatementGc)dg_Campagne.SelectedItem);
                List<string> LstfacturMandDejaPaye = new List<string>();
                List<string> LstfacturMandDejaPaye_Totalement = new List<string>();
                List<CsDetailPaiementGc> LstfacturMandDejaPaye_Paiement = new List<CsDetailPaiementGc>();
                List<CsDetailPaiementGc> LstfacturMandDejaPaye_Paiement_Valide = new List<CsDetailPaiementGc>();
                #region Recupértion des factures deja payé pour le mandatement selectionné

                //#region Premiere passage de boucle pour recupérer les facture qui on déja fait l'objet de paiement
                ////Pour chaque factur du mandatement
                //foreach (var FACTUREMANDATEMENT in mand.DETAILMANDATEMENTGC_)
                //{
                //    //On verifie Pour chaque facture de chaque paiement du mandatement
                //    if (mand.PAIEMENTGC_ != null && mand.PAIEMENTGC_.Count != 0)
                //    {
                //        foreach (var PAIEMENTGC in mand.PAIEMENTGC_)
                //        {
                //            foreach (var DETAILPAIEMENT in PAIEMENTGC.DETAILCAMPAGNEGC_)
                //            {
                //                //Recupértion des factures deja payé
                //                if (FACTUREMANDATEMENT.NDOC == DETAILPAIEMENT.NDOC)
                //                {
                //                    LstfacturMandDejaPaye.Add(DETAILPAIEMENT.NDOC);
                //                    if (LstfacturMandDejaPaye_Paiement.Select(c => c.NDOC).Contains(DETAILPAIEMENT.NDOC))
                //                    {
                //                        var facturMandDejaPaye_Paiement = LstfacturMandDejaPaye_Paiement.FirstOrDefault(c => c.NDOC == DETAILPAIEMENT.NDOC);
                //                        int index = LstfacturMandDejaPaye_Paiement.IndexOf(facturMandDejaPaye_Paiement);
                //                        facturMandDejaPaye_Paiement.MONTANT += DETAILPAIEMENT.MONTANT;
                //                        LstfacturMandDejaPaye_Paiement[index] = facturMandDejaPaye_Paiement;
                //                    }
                //                    else
                //                    {
                //                        LstfacturMandDejaPaye_Paiement.Add(DETAILPAIEMENT);
                //                    }

                //                }
                //            }
                //        }
                //    }
                //}

                //#endregion

                //#region Deuxième  passage de boucle pour recupérer les factures qui on été Totalement payé

                ////Pour chaque factur du mandatement
                //foreach (var FACTUREMANDATEMENT in mand.DETAILMANDATEMENTGC_)
                //{
                //    //On verifie Pour chaque facture de chaque paiement du mandatement
                //    if (mand.PAIEMENTGC_ != null && mand.PAIEMENTGC_.Count != 0)
                //    {
                //        foreach (var PAIEMENTGC in mand.PAIEMENTGC_)
                //        {
                //            foreach (var DETAILPAIEMENT in LstfacturMandDejaPaye_Paiement)
                //            {
                //                //Recupértion des factures deja payé
                //                if (FACTUREMANDATEMENT.NDOC == DETAILPAIEMENT.NDOC && FACTUREMANDATEMENT.MONTANT == DETAILPAIEMENT.MONTANT)
                //                {
                //                    LstfacturMandDejaPaye_Paiement_Valide.Add(DETAILPAIEMENT);
                //                    LstfacturMandDejaPaye_Totalement.Add(DETAILPAIEMENT.NDOC);
                //                }
                //            }
                //        }
                //    }
                //}
                //#endregion
                #endregion

                //#region Lettrage et validation des factures

                ////Verifié qu'il exite des factures du mandatement qui non pa encore été payé 
                //if (mand.DETAILMANDATEMENTGC_.Where(m => !LstfacturMandDejaPaye_Totalement.Contains(m.NDOC)) != null && mand.DETAILMANDATEMENTGC_.Where(m => !LstfacturMandDejaPaye_Totalement.Contains(m.NDOC)).Count() > 0)
                //{
                //    decimal Montant_Paiement = 0;
                //    //Vérification qu'un montant a été saisi vu que le montant de paiment est indispensable pour fair le lettrage automatique des facture non encore payé
                //    if (!string.IsNullOrWhiteSpace(txt_Montant_Paiement.Text) && decimal.TryParse(txt_Montant_Paiement.Text, out Montant_Paiement))
                //    {
                //        List<CsDetailMandatementGc> DataSource = new List<CsDetailMandatementGc>();
                //        //Recupération des factures dont le paiement est non saisi
                //        var detailmandatement = mand.DETAILMANDATEMENTGC_.Where(m => !LstfacturMandDejaPaye_Totalement.Contains(m.NDOC)).ToList();
                //        decimal Montant = 0;
                //        //Lettrage automatiqunction en fonction du motant de paiement saisi pour chaque factures dont le paiement est non encore saisi en commenssant par les plus petite factures
                //        //foreach (var item in detailmandatement.OrderBy(d => d.MONTANT))
                //        foreach (var item in detailmandatement)
                //        {
                //            //On increment le montant de la facture
                //            Montant += item.MONTANT.Value;
                //            //Si le montant devient superieur au montant saisi
                //            if (Montant >= Montant_Paiement)
                //            {
                //                //Si on rentre de cette boucle cela veut dire que le montant à payé est ateint ou dépassé
                //                //on verifie si le montatnt des factures lettré est égale au montant effectivement payé
                //                if (Montant == Montant_Paiement)
                //                {
                //                    //Recupération des factures pris en compte pour le lettrage
                //                    DataSource.Add(item);

                //                    //On vide la variable "Facture_Payer_Partiellement" pour éviter la mise à jour de la facture en question à la validation
                //                    Facture_Payer_Partiellement = null;
                //                    break;
                //                }
                //                //sinon cela voudrai dir  que le montatnt des factures lettré est devenu supérieur au montant effectivement payé:il faut donc gérer un paiement partiel sur la derniere facture 
                //                else
                //                {
                //                    #region gestion udun paiement partiel sur la derniere facture à lettrer

                //                    //Calcule du montant qu'il devrai resté à payé pour la fcature qui fait l'objet de paiement partiel
                //                    Montant_Facture_RestAPayer = Montant - Montant_Paiement;

                //                    //Recuperation du motant de la facture concerné
                //                    var MontantFacture = item.MONTANT;

                //                    //Récupération de la facture partiellment réglé
                //                    var obj = item;
                //                    Facture_Payer_Partiellement = obj;
                //                    //Mise à jour du montant restant à payer sur la facture
                //                    //Facture_Payer_Partiellement.MONTANT = Montant_Facture_RestAPayer;
                //                    //Mise à jour du montant partiel à prendre en compte
                //                    var Montant_Partiel_Facture = MontantFacture - Montant_Facture_RestAPayer;

                //                    //Mise du montatnt partiel paiyé
                //                    item.MONTANT = Montant_Partiel_Facture;

                //                    //Recupération des factures pris en compte pour le lettrage
                //                    DataSource.Add(item);
                //                    break;

                //                    #endregion
                //                }
                //            }
                //            DataSource.Add(item);
                //        }
                //        // Validé les facture pris en compte pour le lettrage 
                //        DataSource.ForEach(d => d.IsMontantValide = true);
                //        //Mise à jour de la liste des paiement pris en compte
                //        MiseAJourMandatement(true, DataSource);

                //        //Récupération du mandatement
                //        var leMandat = (CsMandatementGc)dg_Campagne.SelectedItem;


                //        //Mise à jour des montant versé lié aux facture du mandatement correspondant
                //        foreach (var item in leMandat.DETAILMANDATEMENTGC_)
                //        {
                //            if (DataSource.FirstOrDefault(d => d.NDOC == item.NDOC) != null)
                //                item.MONTANT_VERSER = DataSource.FirstOrDefault(d => d.NDOC == item.NDOC).MONTANT;
                //        }

                //        //Mise à jour de la grille qui affiche les facture du mandatement en tenant compte les montant versé(c-d-a les montant lettré)
                //        dg_facture.ItemsSource = leMandat.DETAILMANDATEMENTGC_;
                //    }
                //    else
                //    {
                //        Message.ShowWarning("Veuillez saisir le montant du paiement avec un bon format", "Information");
                //    }
                //}
                //else
                //{
                //    dg_facture.ItemsSource = new List<CsDetailMandatementGc>();
                //}
                //#endregion

                #region Lettrage et validation des factures

                //Verifié qu'il exite des factures du mandatement qui non pa encore été payé 
                //if (mand.DETAILMANDATEMENTGC_.Where(m => !LstfacturMandDejaPaye_Totalement.Contains(m.NDOC)) != null && mand.DETAILMANDATEMENTGC_.Where(m => !LstfacturMandDejaPaye_Totalement.Contains(m.NDOC)).Count() > 0)
                
                    decimal Montant_Paiement = 0;
                    //Vérification qu'un montant a été saisi vu que le montant de paiment est indispensable pour fair le lettrage automatique des facture non encore payé
                    if (!string.IsNullOrWhiteSpace(txt_Montant_Paiement.Text) && decimal.TryParse(txt_Montant_Paiement.Text, out Montant_Paiement))
                    {
                        List<CsDetailMandatementGc> DataSource = new List<CsDetailMandatementGc>();
                        //Recupération des factures dont le paiement est non saisi
                        var detailmandatement = mand.DETAILMANDATEMENTGC_.Where(m =>m.MONTANT_RESTANT != 0).ToList();
                        //Lettrage automatiqunction en fonction du motant de paiement saisi pour chaque factures dont le paiement est non encore saisi en commenssant par les plus petite factures
                        //foreach (var item in detailmandatement.OrderBy(d => d.MONTANT))
                        foreach (var item in detailmandatement)
                        {

                            if (Montant_Paiement <= 0) break;

                            if (item.MONTANT_RESTANT  >= Montant_Paiement)
                            {
                                item.MONTANT_REGLER = Montant_Paiement;
                                //Facture_Payer_Partiellement = item;
                                //Facture_Payer_Partiellement.MONTANT  = item.MONTANT - item.MONTANT_REGLER;
                            }
                            else
                                item.MONTANT_REGLER = item.MONTANT_RESTANT ;
                            item.MONTANT_RESTANT = item.MONTANT_RESTANT - item.MONTANT_REGLER;

                            Montant_Paiement = Montant_Paiement - item.MONTANT_REGLER.Value ;
                            DataSource.Add(item);
                        }
                        // Validé les facture pris en compte pour le lettrage 
                        DataSource.ForEach(d => d.IsMontantValide = true);
                        //Mise à jour de la liste des paiement pris en compte
                        MiseAJourMandatement(true, DataSource);

                        //Récupération du mandatement
                        var leMandat = (CsMandatementGc)dg_Campagne.SelectedItem;


                        //Mise à jour des montant versé lié aux facture du mandatement correspondant
                        foreach (var item in leMandat.DETAILMANDATEMENTGC_)
                        {
                            if (DataSource.FirstOrDefault(d => d.NDOC == item.NDOC) != null)
                                item.MONTANT_VERSER = DataSource.FirstOrDefault(d => d.NDOC == item.NDOC).MONTANT;
                        }

                        //Mise à jour de la grille qui affiche les facture du mandatement en tenant compte les montant versé(c-d-a les montant lettré)
                        dg_facture.ItemsSource = leMandat.DETAILMANDATEMENTGC_;
                    }
                    else
                    {
                        Message.ShowWarning("Veuillez saisir le montant du paiement avec un bon format", "Information");
                    }
                }
                else
                {
                    dg_facture.ItemsSource = new List<CsDetailMandatementGc>();
                }
                #endregion
        }

        private void btn_trasmettre_Click(object sender, RoutedEventArgs e)
        {
            TransmettreCampagne();
        }

        private void dg_Campagne_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var leMandat = (CsMandatementGc)dg_Campagne.SelectedItem;
            //dg_facture.ItemsSource = DataSource;
            dg_facture.ItemsSource = leMandat.DETAILMANDATEMENTGC_;
        }
        private void TransmettreCampagne()
        {
            var datasourcepaiement = (List<CsPaiementGc>)dg_Paiement.ItemsSource;
            decimal? montantpaiement = 0;
            datasourcepaiement.ForEach(p => montantpaiement += p.DETAILCAMPAGNEGC_.Sum(d => d.MONTANT));

            var datasourcemandatement = (List<CsMandatementGc>)dg_Campagne.ItemsSource;
            decimal? montantmandatement = 0;
            datasourcemandatement.ForEach(p => montantmandatement += p.MONTANT);
            if (montantpaiement == montantmandatement)
            {
                //Transmission a l'étape suivante
                List<int> lstId = new List<int>();
                lstId.Add(this.IdCampagne);
                EnvoyerDemandeEtapeSuivante(lstId);
            }
            else
            {
                Message.ShowWarning("La campagne ne peut pas etre transmise , paiement imcomplé", "Information");
            }
        }


        private void EnvoyerDemandeEtapeSuivante(List<int> Listid)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.Protocole(), Utility.EndPoint("Workflow"));
            clientWkf.ExecuterActionSurDemandeParPkIDLigneCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec de sortie materiel", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    return;
                }
                if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
                {
                    Message.ShowError("Echec de sortie materiel", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    return;
                }
                else
                {
                    //Message.ShowInformation("Sortie materiel effectuée", Langue.lbl_Menu);

                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, this.EtapeActuel, ServiceWorkflow.CODEACTION.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }

        #endregion

        #region  Service

        private void RemplirCampagne(string Matricule, bool ATransmettre = false)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RemplirCampagneByIdCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                    return;
                LaCampagneGc = args.Result;
                List<CsMandatementGc> Datasource = new List<CsMandatementGc>();
                decimal? MontantPaiement = new decimal();
                List<CsDetailMandatementGc> ListeDetailCampagneAsuprimmer = new List<CsDetailMandatementGc>();

                foreach (var LeMand in LaCampagneGc.MANDATEMENTGC_)
                    {
                        if (LeMand.PAIEMENTGC_ != null && LeMand.PAIEMENTGC_.Count != 0)
                        {
                            LeMand.PAIEMENTGC_.ForEach(p => MontantPaiement += p.DETAILCAMPAGNEGC_.Sum(d => d.MONTANT));
                            dg_Paiement.ItemsSource = null;
                            dg_Paiement.ItemsSource = LeMand.PAIEMENTGC_; 
                        }
                        foreach (var item_ in LeMand.DETAILMANDATEMENTGC_)
                        {
                            item_.MONTANT_RESTANT = 0;
                            item_.MONTANT_REGLER = 0;
                            if (LeMand.PAIEMENTGC_ != null && LeMand.PAIEMENTGC_.Count != 0)
                            {
                                foreach (var paiement in LeMand.PAIEMENTGC_)
                                {
                                    var detailmand = paiement.DETAILCAMPAGNEGC_.Where(dm => dm.CENTRE == item_.CENTRE && dm.CLIENT == item_.CLIENT && dm.ORDRE == item_.ORDRE && dm.NDOC == item_.NDOC);
                                    if (detailmand != null)
                                    {
                                        item_.MONTANT_REGLER = item_.MONTANT_REGLER + detailmand.Sum(c => c.MONTANT);
                                    }
                                }
                            }
                            item_.MONTANT_RESTANT = item_.MONTANT - item_.MONTANT_REGLER;

                            if (item_.MONTANT_RESTANT <= 0)
                            {
                                ListeDetailCampagneAsuprimmer.Add(item_);
                            }
                            item_.MONTANT_VERSER = 0;
                        }
                    }


                Datasource.AddRange(LaCampagneGc.MANDATEMENTGC_);


                txt_Campagne.Text = LaCampagneGc.NUMEROCAMPAGNE != null ? LaCampagneGc.NUMEROCAMPAGNE : string.Empty;
                txt_regroupement.Text = LaCampagneGc.LIBELLEREGROUPEMENT != null ? LaCampagneGc.LIBELLEREGROUPEMENT : string.Empty;
                txt_periode.Text = LaCampagneGc.PERIODE != null ? LaCampagneGc.PERIODE : string.Empty;

                dg_Campagne.ItemsSource = Datasource;
               
                if (ATransmettre == true)
                {
                    TransmettreCampagne();
                }
                return;
            };
            service.RemplirCampagneByIdAsync(this.IdCampagne,"0" );
        }
        private void SavePaiement(List<CsPaiementGc> ListMandatementGc, bool ATransmettre = false)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.SavePaiementCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                    return;
                if (args.Result == true)
                {
                    Message.Show("Paiement enregistré avec succes", "Information");
                    if (ATransmettre == true)
                    {
                        RemplirCampagne(UserConnecte.matricule, true);
                    }
                    RemplirCampagne(UserConnecte.matricule);

                    btn_trasmettre.Visibility = Visibility.Visible;
                    OKButton.Visibility = Visibility.Collapsed;
                    btn_Actualiser.Visibility = Visibility.Collapsed;
                    txt_Montant_Paiement.Visibility = Visibility.Collapsed;
                    lbl_TotalePaiement.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Message.Show("Le Paiement n'a pas été enregistré avec succes,veuillez refaire l'opration ", "Information");
                }

                return;
            };
            Facture_Payer_Partiellement.MONTANT = Montant_Facture_RestAPayer;
            service.SavePaiementAsync(ListMandatementGc, Facture_Payer_Partiellement);
        }

        #endregion

        #region Methodes

        private void MiseAJourMandatement(bool Isvalide, List<CsDetailMandatementGc> ItemsSource = null)
        {
            //Recuperation du mandatement selectionné
            CsMandatementGc CampAnSelectioner = new CsMandatementGc();
            CampAnSelectioner = (CsMandatementGc)dg_Campagne.SelectedItem;

            //Recupération des factures pris en compte dans le lettrage
            var datasource = ItemsSource == null ? (List<CsDetailMandatementGc>)dg_facture.ItemsSource : ItemsSource;

            //Mise à jour 
            UpdateDataSource(Isvalide, datasource);


            //Récupération du mandatement
            var MandatementGc = ListMandatementGc.FirstOrDefault(m => m.FK_IDMANDATEMANT == CampAnSelectioner.PK_ID);
            if (MandatementGc == null)
            {
                //Création d'une instance de d'objet de paiement
                var Mandatement = new CsPaiementGc { FK_IDMANDATEMANT = CampAnSelectioner.PK_ID,
                                                     NUMEROMANDATEMENT =CampAnSelectioner.NUMEROMANDATEMENT ,
                                                     MONTANT = datasource.Where(f => f.MONTANT_REGLER != 0).Sum(dm => dm.MONTANT_REGLER),
                                                     DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, 
                                                     USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule
                                                     };

                //Création des détail de paiement
                List<CsDetailPaiementGc> DETAILMANDATEMENTGC_ = new List<CsDetailPaiementGc>();
                foreach (var item in datasource)
                {
                    if (item.IsMontantValide == true)
                    {
                        CsDetailPaiementGc DetailMandatement = new CsDetailPaiementGc();

                        DetailMandatement.CENTRE = item.CENTRE;
                        DetailMandatement.CLIENT = item.CLIENT;
                        DetailMandatement.ORDRE = item.ORDRE;
                        DetailMandatement.DATECREATION = DateTime.Now;
                        DetailMandatement.DATEMODIFICATION = DateTime.Now;
                        DetailMandatement.FK_IDPAIEMENTCAMPAGNEGC = Mandatement.PK_ID;
                        DetailMandatement.MONTANT = item.MONTANT_REGLER ;
                        DetailMandatement.NDOC = item.NDOC;
                        DetailMandatement.PERIODE = item.PERIODE;
                        DetailMandatement.STATUS = item.STATUS;
                        DetailMandatement.USERCREATION = UserConnecte.matricule;
                        DetailMandatement.USERMODIFICATION = UserConnecte.matricule;

                        DETAILMANDATEMENTGC_.Add(DetailMandatement);

                    }

                }
                //Mise à jour des detail de paiement dans l'objet de paiement
                Mandatement.DETAILCAMPAGNEGC_ = DETAILMANDATEMENTGC_;
                //Mise à jour de la liste des paiement pris en compte
                ListMandatementGc.Add(Mandatement);
            }
            else
            {

                MandatementGc.MONTANT = datasource.Where(f => f.IsMontantValide == true).Sum(dm => dm.MONTANT_REGLER );
                MandatementGc.NUMEROMANDATEMENT = txt_Numdeataire.Text;
                List<CsDetailPaiementGc> DETAILMANDATEMENTGC_ = new List<CsDetailPaiementGc>();
                foreach (var item in datasource)
                {
                    if (item.IsMontantValide == true)
                    {
                        CsDetailPaiementGc DetailMandatement = new CsDetailPaiementGc();

                        DetailMandatement.CENTRE = item.CENTRE;
                        DetailMandatement.CLIENT = item.CLIENT;
                        DetailMandatement.ORDRE = item.ORDRE;
                        DetailMandatement.DATECREATION = DateTime.Now;
                        DetailMandatement.DATEMODIFICATION = DateTime.Now;
                        DetailMandatement.FK_IDPAIEMENTCAMPAGNEGC = MandatementGc.PK_ID;
                        DetailMandatement.MONTANT = item.MONTANT_REGLER ;
                        //DetailMandatement.MONTANT_REGLER = DetailMandatement.MONTANT;
                        //DetailMandatement.MONTANT_VERSER = DetailMandatement.MONTANT;
                        //DetailMandatement.MONTANT_RESTANT = 0;
                        DetailMandatement.NDOC = item.NDOC;
                        DetailMandatement.PERIODE = item.PERIODE;
                        DetailMandatement.STATUS = item.STATUS;
                        DetailMandatement.USERCREATION = UserConnecte.matricule;
                        DetailMandatement.USERMODIFICATION = UserConnecte.matricule;

                        DETAILMANDATEMENTGC_.Add(DetailMandatement);

                    }

                }
                MandatementGc.DETAILCAMPAGNEGC_ = DETAILMANDATEMENTGC_;

                var mand = ListMandatementGc.FirstOrDefault(m => m.PK_ID == MandatementGc.PK_ID);
                var index = ListMandatementGc.IndexOf(mand);
                ListMandatementGc[index] = MandatementGc;

            }

            txt_Montant_Mandatement.Text = datasource.Where(f => f.MONTANT_REGLER != 0).Sum(dm => dm.MONTANT_REGLER).ToString();

        }

        private void UpdateDataSource(bool Isvalide, List<CsDetailMandatementGc> datasource)
        {

            if (dg_facture.SelectedItem != null)
            {
                var FactureSelectionne = (CsDetailMandatementGc)dg_facture.SelectedItem;

                var Index = datasource.IndexOf(FactureSelectionne);

                if (Index > 0)
                {
                    var FactureCorrespondante = datasource.ElementAt(Index);
                    FactureCorrespondante.IsMontantValide = Isvalide;
                    datasource[Index] = FactureCorrespondante;
                }
            }
        }

        #endregion



    }
}


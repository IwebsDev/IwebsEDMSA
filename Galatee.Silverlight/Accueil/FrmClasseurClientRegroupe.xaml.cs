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
using Galatee.Silverlight.ServiceAccueil;
using System.Collections.ObjectModel;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Classes;
using System.Windows.Threading;
using Galatee.Silverlight.ServiceSig;
using Microsoft.Maps.MapControl.Design;
using Microsoft.Maps.MapControl;
using Galatee.Silverlight.SIG;
using System.IO;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmClasseurClientRegroupe : ChildWindow
    {
        List<CsCentre> LstCentre = new List<CsCentre>();
        List<CsProduit> LstDeProduit = new List<CsProduit>();
        List<CsEvenement> lstEvenement;
        Galatee.Silverlight.ServiceAccueil.CsClientRechercher _LeClientSelect;
        CsClasseurClient _LeClasseur;
        CsCompteClient leClasseurClient;
        List<CsTarif> LstTarif;
        List<CsForfait> LstForfait;
        List<CsPuissance> LstPuissance;
        List<CsReglageCompteur> LstReglageCompteur;
        List<CsTypeBranchement> LstDiametreBrt;
        List<CsMois> LstMois;
        List<CsCodeTaxeApplication> LstCodeApplicationTaxe;
        List<CsNationalite> LstDesNationalites;

        List<CsCommune> LstCommuneAll = new List<CsCommune>();
        List<CsCommune> LstCommuneSelect = new List<CsCommune>();
        List<CsQuartier> LstQuartierAll = new List<CsQuartier>();
        List<CsSecteur> LstSecteurAll = new List<CsSecteur>();
        List<CsRues> LstRueAll = new List<CsRues>();
        List<CsTcompteur> LstTypeCompteur = new List<CsTcompteur>();
        List<CsMarqueCompteur> LstMarque = new List<CsMarqueCompteur>();
        List<CsCadran> LstCadran = new List<CsCadran>();
        List<CsMaterielBranchement> LstDeMaterielBrt = new List<CsMaterielBranchement>();
        List<CsRegCli> LstCodeRegroupement = new List<CsRegCli>();



        List<CsFrequence> LstFrequence = new List<CsFrequence>();
        List<CsFermable> LstFermable = new List<CsFermable>();
        List<CsDenomination> LstCivilite = new List<CsDenomination>();
        List<CsCodeConsomateur> LstCodeConsomateur = new List<CsCodeConsomateur>();
        List<CsCategorieClient> LstCategorie = new List<CsCategorieClient>();
        List<CsNatureClient> LstNatureClient = new List<CsNatureClient>();
        List<CsRegCli> LstRegCli = new List<CsRegCli>();


        List<CsLclient> LstFactureClient;
        List<CsLclient> LstReglementClient;
        List<CsLclient> LstImpayesClient;
        List<CsDemandeBase> ListDemande;
        List<CsAbon> lesAbonnements = new List<CsAbon>();

        void translate()
        {
            // Gestion abonnement
            this.lbl_ApplicationTax.Content = Langue.lbl_ApplicationTax;
            this.lbl_Client.Content = Langue.lbl_client;
            this.lbl_DateAbonnement.Content = Langue.lbl_DateAbonnement;
            this.lbl_DateResiliation.Content = Langue.lbl_DateResiliation;
            this.lbl_Forfait.Content = Langue.lbl_Forfait;
            this.lbl_ForfaitPersonaliseAnnuel.Content = Langue.lbl_ForfaitPersonaliseAnnuel;
            //this.lbl_Comsomation.Content = Langue.lbl_consommation;
            this.lbl_MoisFact.Content = Langue.lbl_MoisFact;
            this.lbl_MoisReleve.Content = Langue.lbl_MoisReleve;
            this.lbl_Ordre.Content = Langue.lbl_Ordre;
            this.lbl_Periodicite.Content = Langue.lbl_Periodicite;
            this.lbl_PuissanceSouscrite.Content = Langue.lbl_PuissanceSouscrite;
            this.lbl_PuissanceUtilise.Content = Langue.lbl_PuissanceUtilise;
            this.lbl_Ristourne.Content = Langue.lbl_Ristourne;
            this.lbl_CodeTarif.Content = Langue.lbl_Tarif;
            this.rdb_GprInvoiceNo.Content = Langue.lbl_Non;
            this.rdb_gprInvoiceYes.Content = Langue.lbl_Oui;
            this.lbl_dateCreation.Content = Langue.dg_dateCreation;
            // Gestion adresse
            this.lbl_adresse.Content = Langue.lbl_adresse;
            this.lbl_Commune.Content = Langue.lbl_Commune;
            this.lbl_Etage.Content = Langue.lbl_Etage;
            this.lbl_Lot.Content = Langue.lbl_Lot;
            this.lbl_NomProprietaire.Content = Langue.lbl_NomProprietaire;
            this.lbl_NumRue.Content = Langue.lbl_NumRue;
            this.lbl_Ordre.Content = Langue.lbl_Ordre;
            this.lbl_Quartier.Content = Langue.lbl_Quartier;
            //this.lbl_Rue.Content = Langue.lbl_Rue;
            this.lbl_Secteur.Content = Langue.lbl_Secteur;
            this.lbl_Sequence.Content = Langue.lbl_Ordre;
            this.lbl_Telephone.Content = Langue.lbl_Telephone;
            this.lbl_Tournee.Content = Langue.lbl_Tournee;
            this.lbl_autresInfo.Content = Langue.lbl_autresInfo;
            // Compteur 
            this.lbl_diametre.Content = Langue.lbl_diametre;
            this.lbl_Localisation.Content = Langue.lbl_Localisation;
            this.lbl_Marque.Content = Langue.lbl_Marque;
            this.lbl_NumeroCompteur.Content = Langue.lbl_NumeroCompteur;
            this.lbl_typeCompteur.Content = Langue.lbl_type;
            this.tab5_Chk_CoefMultiplication.Content = Langue.lbl_CoefMultiplication;
            this.lbl_AnneFabrication.Content = Langue.lbl_AnneFabrication;
            // Evenement
            this.tab5_Stab2dataGrid2.Columns[0].Header = Langue.lbl_numEvent;
            this.tab5_Stab2dataGrid2.Columns[1].Header = Langue.lbl_NumeroCompteur;
            this.tab5_Stab2dataGrid2.Columns[2].Header = Langue.lbl_diametre;
            this.tab5_Stab2dataGrid2.Columns[3].Header = Langue.dg_date;
            this.tab5_Stab2dataGrid2.Columns[4].Header = Langue.lbl_Code;
            this.tab5_Stab2dataGrid2.Columns[5].Header = Langue.lbl_Index;
            this.tab5_Stab2dataGrid2.Columns[6].Header = Galatee.Silverlight.Resources.Index.Langue.lbl_IndexCas;
            this.tab5_Stab2dataGrid2.Columns[7].Header = Galatee.Silverlight.Resources.Index.Langue.lbl_Conso;
            this.tab5_Stab2dataGrid2.Columns[8].Header = Galatee.Silverlight.Resources.Index.Langue.lbl_Enquete;
            this.tab5_Stab2dataGrid2.Columns[9].Header = Galatee.Silverlight.Resources.Index.Langue.lbl_Statut;

            //Brt
            this.lbl_adresse.Content = Langue.lbl_adresse;
            this.lbl_Ordre.Content = Langue.lbl_Ordre;
            this.tab6_stab1_rdb_InService.Content = Langue.rdb_EnService;
            this.tab6_stab1_rdb_deconnecter.Content = Langue.rdb_HorsService;
            this.lbl_DateConnectionbrt.Content = Langue.lbl_DateRacordement;
            this.lbl_DateFermeture.Content = Langue.lbl_DateFermeture;

            // CompteClient
            this.lbl_CentreCptClient.Content = Langue.lbl_center;
            this.lbl_ClientCptClient.Content = Langue.lbl_client;
            this.lbl_OrdreCptClient.Content = Langue.lbl_Ordre;
            this.tab4_dataGridAllCompteClient.Columns[0].Header = Langue.lbl_NumFact;
            this.tab4_dataGridAllCompteClient.Columns[1].Header = Langue.lbl_periode;
            this.tab4_dataGridAllCompteClient.Columns[2].Header = Langue.lbl_coper;
            this.tab4_dataGridAllCompteClient.Columns[4].Header = Langue.lbl_Montant;
            this.tab4_dataGridAllCompteClient.Columns[5].Header = Langue.lbl_Direction;
            this.tab4_dataGridAllCompteClient.Columns[6].Header = Langue.lbl_ModePayement;
            this.tab4_dataGridAllCompteClient.Columns[7].Header = Langue.lbl_dateExigible;
            this.tab4_dataGridAllCompteClient.Columns[8].Header = Langue.lbl_Operateur;

            this.tab4_dataGridReglement.Columns[0].Header = Langue.lbl_DateEncaissement;
            this.tab4_dataGridReglement.Columns[1].Header = Langue.lbl_cashier;
            this.tab4_dataGridReglement.Columns[2].Header = Langue.lbl_Montant;
            this.tab4_dataGridReglement.Columns[3].Header = Langue.lbl_RecuOuBatch;


            //this.tab4_dataGridFacture.Columns[0].Header = Langue.lbl_NumFact;
            //this.tab4_dataGridFacture.Columns[1].Header = Langue.lbl_periode;
            //this.tab4_dataGridFacture.Columns[2].Header = Langue.lbl_coper;
            //this.tab4_dataGridFacture.Columns[3].Header = Langue.lbl_Date;
            //this.tab4_dataGridFacture.Columns[4].Header = Langue.lbl_Montant;
            //this.tab4_dataGridFacture.Columns[5].Header = Langue.lbl_Direction;
            //this.tab4_dataGridFacture.Columns[6].Header = Langue.lbl_dateExigible;

            // Client 
            this.lbl_adresse.Content = Langue.lbl_adresse;
            this.lbl_categoie.Content = Langue.lbl_categoie;
            this.lbl_CodeConsomateur.Content = Langue.lbl_CodeConsomateur;
            this.lbl_CodeRegroupement.Content = Langue.lbl_CodeRegroupement;
            this.lbl_CodeRelance.Content = Langue.lbl_CodeRelance;
            this.lbl_Nationnalite.Content = Langue.lbl_Nationnalite;
            this.rdb_Owner.Content = Langue.rdb_landlord;
            this.rdb_Tenant.Content = Langue.rdb_tenant;

        }
        public FrmClasseurClientRegroupe()
        {
            try
            {
                InitializeComponent();
                translate();
                ChargerDonneeDuSite();
                ChargerListeDeProduit();
                ChargerCodeRegroupement();
                SessionObject.IsChargerDashbord = false;
                this.tab4_dataGridMandatement.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }
        private void txt_Regroupement_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txt_Regroupement.Text.Length == SessionObject.Enumere.TailleCodeRegroupement)
            {
                CsRegCli leRegcli = SessionObject.LstCodeRegroupement.FirstOrDefault(y => y.CODE == this.txt_Regroupement.Text);
                if (leRegcli != null && !string.IsNullOrEmpty(leRegcli.CODE))
                {
                    this.Txt_LibelleRegroupement.Text = leRegcli.NOM;
                    this.txt_Regroupement.Tag = leRegcli;
                }
                else
                    Message.ShowInformation("Le regroupement saisi est inexistant", "Recouvrement");

            }
        }
        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ServiceAccueil.CsCentre> lstCentreDistinct = new List<ServiceAccueil.CsCentre>();
                if (SessionObject.LstCentre.Count != 0)
                {
                    var lesDistinct = SessionObject.LstCentre.Where(ip => ip.CODE != "001" && ip.CODE != "002"
                                                                     && ip.CODE != "003" && ip.CODE != "004").
                                                                     Select(u => new { u.CODE }).Distinct();
                    foreach (var item in lesDistinct)
                    {
                        ServiceAccueil.CsCentre leCentr = new ServiceAccueil.CsCentre();
                        leCentr.CODE = item.CODE;
                        lstCentreDistinct.Add(leCentr);
                    }
                }
                if (lstCentreDistinct != null && lstCentreDistinct.Count != 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<Galatee.Silverlight.ServiceFacturation.CsLotri> leLotSelect = new List<Galatee.Silverlight.ServiceFacturation.CsLotri>();
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "CODE");
                    foreach (ServiceAccueil.CsCentre item in lstCentreDistinct)
                    {
                        leLotSelect.Add(new ServiceFacturation.CsLotri()
                        {
                            CODE = item.CODE,
                        });
                    }
                    Galatee.Silverlight.Facturation.UcGenerique ctrl = new Galatee.Silverlight.Facturation.UcGenerique(leLotSelect, true, "Liste des centres");
                    ctrl.Closed += new EventHandler(ucgCentre);
                    ctrl.Show();
                    this.btn_Centre.IsEnabled = true;
                }


            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void ucgCentre(object sender, EventArgs e)
        {

            Galatee.Silverlight.Facturation.UcGenerique ctrs = sender as Galatee.Silverlight.Facturation.UcGenerique;
            if (ctrs.isOkClick)
            {
                List<Galatee.Silverlight.ServiceFacturation.CsLotri> LesCentreeDuLot = (List<Galatee.Silverlight.ServiceFacturation.CsLotri>)ctrs.MyObjectList;
                if (LesCentreeDuLot != null && LesCentreeDuLot.Count > 0)
                {
                    int passage = 1;
                    foreach (Galatee.Silverlight.ServiceFacturation.CsLotri item in LesCentreeDuLot)
                    {
                        if (passage == 1)
                            this.Txt_LibelleCentre.Text = item.CODE;
                        else
                            this.Txt_LibelleCentre.Text = this.Txt_LibelleCentre.Text + "  " + item.CODE;
                        passage++;

                    }
                    this.Txt_LibelleCentre.Tag = LesCentreeDuLot.Select(o => o.CODE).ToList();
                }
            }
        }
        private void initControl()
        {
            try
            {
                List<string> ListOperation = new List<string>();
                ListOperation = SessionObject.TypeOperationClasseurReg().ToList();
                tab4_cbo_Operation.ItemsSource = null;
                tab4_cbo_Operation.ItemsSource = ListOperation.ToList() ;
                if (ListOperation != null && ListOperation.Count != 0)
                    tab4_cbo_Operation.SelectedItem = ListOperation[0];

            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, Langue.lbl_Menu + "=>initControl");
            }
        }
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre.Count != 0)
                {
                    LstCentre = SessionObject.LstCentre;
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    if (lstSite != null)
                    {
                        List<CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                        }
                        LstCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == _LstSite[0].PK_ID).ToList();
                        if (LstCentre != null && LstCentre.Count == 1)
                        {
                        }
                    }
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = SessionObject.LstCentre;
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    if (lstSite != null)
                    {
                        List<CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerDonneeDuSite");

            }
        }

        private void ChargerListeDeProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit.Count != 0)
                {
                    LstDeProduit = SessionObject.ListeDesProduit;
                    return;
                }
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = res.Result;
                    LstDeProduit = SessionObject.ListeDesProduit;

                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerListeDeProduit");
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void RemplireOngletToutLeCompte(List<CsLclient> _LeCompteClient)
        {
            try
            {
                tab4_dataGridAllCompteClient.ItemsSource = null;
                this.tab4_txt_LibelleCentre.Text = string.IsNullOrEmpty(this.Txt_LibelleCentre.Text) ? string.Empty : this.Txt_LibelleCentre.Text;
                tab4_dataGridAllCompteClient.ItemsSource = FormateListe(_LeCompteClient);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private List<CsLclient> FormateListe(List<CsLclient> _LeCompteClient)
        {
            List<CsLclient> _LstFactureFinal = new List<CsLclient>();
            if (_LeCompteClient != null && ((_LeCompteClient != null && _LeCompteClient.Count != 0)))
            {
                List<CsLclient> _LstFacture = _LeCompteClient.Where(p => p.DC == SessionObject.Enumere.Debit).ToList();
                List<CsLclient> _LstEncaissement = _LeCompteClient.Where(p => p.DC == SessionObject.Enumere.Credit).ToList();
                if (_LstFacture != null && _LstFacture.Count != 0)
                    foreach (var item in _LstFacture)
                    {
                        _LstFactureFinal.Add(item);
                        List<CsLclient> lstFacture = _LstEncaissement.Where(p => p.REFEM == item.REFEM && p.NDOC == item.NDOC).ToList();
                        if (lstFacture != null && lstFacture.Count != 0)
                            _LstFactureFinal.AddRange(TransLClient(lstFacture));
                    }
                else
                    _LstFactureFinal.AddRange(_LstEncaissement);

                this.tab4_txt_TotalCredit.Text = _LstEncaissement.Sum(t => t.MONTANT).Value.ToString(SessionObject.FormatMontant);
                this.tab4_txt_TotalDebit.Text = _LstFacture.Sum(t => t.MONTANT).Value.ToString(SessionObject.FormatMontant);
                this.tab4_txt_balance.Text = (_LstFacture.Sum(t => t.MONTANT) - _LstEncaissement.Sum(t => t.MONTANT)).Value.ToString(SessionObject.FormatMontant);
                //if (_LstEncaissement != null && _LstEncaissement.Count != 0)
                // foreach (CsLclient item in _LstEncaissement)
                // {
                //     if (_LstFactureFinal.FirstOrDefault(t => t.REFEM == item.REFEM && t.NDOC == item.NDOC) == null)
                //         _LstFactureFinal.Add(item);
                // }
            }
            return _LstFactureFinal;
        }
        private List<CsLclient> TransLClient(List<CsLclient> _LeTranscaisse)
        {
            List<CsLclient> _LeReglt = new List<CsLclient>();
            foreach (var item in _LeTranscaisse)
            {
                item.REFEM = string.Empty;
                item.NDOC = string.Empty;
                item.ACQUIT = string.Empty;
                _LeReglt.Add(item);
            }
            return _LeReglt;
        }


        private void RemplireOngletToutLeCompte(List<CsLclient> _LeCompteFacture, List<CsLclient> toutLeCompte)
        {
            try
            {
                tab4_dataGridAllCompteClient.ItemsSource = null;
                this.tab4_txt_LibelleCentre.Text = string.IsNullOrEmpty(this.Txt_LibelleCentre.Text) ? string.Empty : this.Txt_LibelleCentre.Text;
                tab4_dataGridAllCompteClient.ItemsSource = FormateListe(_LeCompteFacture, toutLeCompte);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private List<CsLclient> FormateListe(List<CsLclient> _LeCompteFacture, List<CsLclient> toutLeCompte)
        {
            List<CsLclient> _LstFactureFinal = new List<CsLclient>();
            decimal MontantRglt = 0;
            if (_LeCompteFacture != null && ((_LeCompteFacture != null && _LeCompteFacture.Count != 0)))
            {
                List<CsLclient> _LstEncaissement = toutLeCompte.Where(p => p.DC == SessionObject.Enumere.Credit).ToList();
                if (_LeCompteFacture != null && _LeCompteFacture.Count != 0)
                    foreach (var item in _LeCompteFacture)
                    {
                        _LstFactureFinal.Add(item);
                        List<CsLclient> lstFacture = _LstEncaissement.Where(p => p.REFEM == item.REFEM && p.NDOC == item.NDOC).ToList();
                        if (lstFacture != null && lstFacture.Count != 0)
                        {
                            _LstFactureFinal.AddRange(TransLClient(lstFacture));
                            MontantRglt = MontantRglt + lstFacture.Sum(t => t.MONTANT).Value;
                        }
                    }
                else
                    _LstFactureFinal.AddRange(_LstEncaissement);

                this.tab4_txt_TotalCredit.Text = MontantRglt.ToString(SessionObject.FormatMontant);
                this.tab4_txt_TotalDebit.Text = _LeCompteFacture.Sum(t => t.MONTANT).Value.ToString(SessionObject.FormatMontant);
                this.tab4_txt_balance.Text = (_LeCompteFacture.Sum(t => t.MONTANT) - MontantRglt).Value.ToString(SessionObject.FormatMontant);

                //if (_LstEncaissement != null && _LstEncaissement.Count != 0)
                // foreach (CsLclient item in _LstEncaissement)
                // {
                //     if (_LstFactureFinal.FirstOrDefault(t => t.REFEM == item.REFEM && t.NDOC == item.NDOC) == null)
                //         _LstFactureFinal.Add(item);
                // }
            }
            return _LstFactureFinal;
        }


        private void RemplireOngletFacture(List<CsLclient> _LesFacture)
        {
            try
            {
                tab4_dataGridFacture.ItemsSource = null;
                this.tab4_txt_LibelleCentre.Text = string.IsNullOrEmpty(this.Txt_LibelleCentre.Text) ? string.Empty : this.Txt_LibelleCentre.Text;
                tab4_dataGridFacture.ItemsSource = _LesFacture.OrderByDescending(p => p.DENR);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void RemplireOngletImpaye(List<CsLclient> _LesFacture)
        {
            try
            {
                tab4_dataGridImpaye.ItemsSource = null;
                this.tab4_txt_LibelleCentre.Text = string.IsNullOrEmpty(this.Txt_LibelleCentre.Text) ? string.Empty : this.Txt_LibelleCentre.Text;
                tab4_dataGridImpaye.ItemsSource = _LesFacture.OrderByDescending(p => p.DENR).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void RemplireOngletReglement(List<CsLclient> _LesReglement)
        {
            try
            {

                var reglemntParModereg = (from p in _LesReglement
                                          group new { p } by new { p.ACQUIT, p.DTRANS, p.NOMCAISSIERE } into pResult
                                          select new
                                          {
                                              pResult.Key.ACQUIT,
                                              pResult.Key.NOMCAISSIERE,
                                              pResult.Key.DTRANS,
                                              MONTANT = (decimal?)pResult.Where(t => t.p.ACQUIT == pResult.Key.ACQUIT).Sum(o => o.p.MONTANT)
                                          });
                tab4_dataGridReglement.ItemsSource = null;
                this.tab4_txt_LibelleCentre.Text = string.IsNullOrEmpty(this.Txt_LibelleCentre.Text) ? string.Empty : this.Txt_LibelleCentre.Text;
                tab4_dataGridReglement.ItemsSource = reglemntParModereg.OrderByDescending(p => p.DTRANS);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void RetourneAdresse(Galatee.Silverlight.ServiceAccueil.CsClientRechercher leClient)
        {
            try
            {
                int res = LoadingManager.BeginLoading(Langue.En_Cours);
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneAdresseCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                    CsAg leAg = args.Result;
                    if (leAg != null)
                        RemplireOngletAdresse(leAg);
                    else
                        RemplireOngletAdresse(new CsAg());

                    LoadingManager.EndLoading(res);
                };
                service.RetourneAdresseAsync(leClient.FK_IDCENTRE, leClient.CENTRE, leClient.CLIENT, leClient.ORDRE);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }
        private void RemplireOngletAdresse(CsAg _LeAdresse)
        {
            try
            {
                this.tab3_txt_LibelleCentre.Text = string.IsNullOrEmpty(this.Txt_LibelleCentre.Text) ? string.Empty : this.Txt_LibelleCentre.Text;
                if (_LeAdresse != null)
                {
                    this.tab12_txt_NomProprietaire.Text = (string.IsNullOrEmpty(_LeAdresse.NOMP) ? string.Empty : _LeAdresse.NOMP);
                    this.tab3_txt_NomClientBrt.Text = string.IsNullOrEmpty(_LeAdresse.NOMP) ? string.Empty : _LeAdresse.NOMP;
                    this.tab3_txt_LibelleCommune.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLECOMMUNE) ? string.Empty : _LeAdresse.LIBELLECOMMUNE;
                    this.tab3_txt_LibelleQuartier.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLEQUARTIER) ? string.Empty : _LeAdresse.LIBELLEQUARTIER;
                    this.tab3_txt_Secteur.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLESECTEUR) ? string.Empty : _LeAdresse.LIBELLESECTEUR;
                    this.tab3_txt_NomRue.Text = string.IsNullOrEmpty(_LeAdresse.LIBELLERUE) ? string.Empty : _LeAdresse.LIBELLERUE;
                    this.tab3_txt_NumRue.Text = string.IsNullOrEmpty(_LeAdresse.RUE) ? string.Empty : _LeAdresse.RUE;
                    this.tab3_txt_etage.Text = string.IsNullOrEmpty(_LeAdresse.ETAGE) ? string.Empty : _LeAdresse.ETAGE;
                    this.tab3_txt_NumLot.Text = string.IsNullOrEmpty(_LeAdresse.CADR) ? string.Empty : _LeAdresse.CADR;
                    //this.tab3_txt_Email.Text = string.IsNullOrEmpty(_LeAdresse.EMAIL) ? string.Empty : _LeAdresse.EMAIL;

                    this.tab3_txt_Telephone.Text = string.IsNullOrEmpty(_LeAdresse.TELEPHONE) ? string.Empty : _LeAdresse.TELEPHONE;
                    //this.tab3_txt_Fax.Text = string.IsNullOrEmpty(_LeAdresse.FAX) ? string.Empty : _LeAdresse.FAX;
                    this.tab3_txt_OrdreTour.Text = string.IsNullOrEmpty(_LeAdresse.ORDTOUR) ? string.Empty : _LeAdresse.ORDTOUR;
                    this.tab3_txt_tournee.Text = string.IsNullOrEmpty(_LeAdresse.TOURNEE) ? string.Empty : _LeAdresse.TOURNEE;
                    this.tab3_txt_porte.Text = string.IsNullOrEmpty(_LeAdresse.PORTE) ? string.Empty : _LeAdresse.PORTE;

                    this.tab3_txt_DateCreate.Text = _LeAdresse.DATECREATION.ToShortDateString();
                    this.tab3_txt_DateModif.Text = _LeAdresse.DATEMODIFICATION == null ? string.Empty : _LeAdresse.DATEMODIFICATION.Value.ToShortDateString();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void RetourneAbonnement(Galatee.Silverlight.ServiceAccueil.CsClientRechercher leClient)
        {
            try
            {
                int res = LoadingManager.BeginLoading(Langue.En_Cours);
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneAbonCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                    lesAbonnements = args.Result;
                    List<CsAbon> lesAbonnement = args.Result;
                    if (lesAbonnement != null)
                        RemplireOngletAbon(lesAbonnement);
                    else
                        RemplireOngletAbon(new List<CsAbon>());

                    LoadingManager.EndLoading(res);
                };
                service.RetourneAbonAsync(leClient.FK_IDCENTRE, leClient.CENTRE, leClient.CLIENT, leClient.ORDRE);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }
        private void RemplireOngletAbon(List<CsAbon> _LstAbonnement)
        {
            try
            {
                this.tab2_txt_LibelleCentre.Text = string.IsNullOrEmpty(this.Txt_LibelleCentre.Text) ? string.Empty : this.Txt_LibelleCentre.Text;

                if (_LstAbonnement != null && _LstAbonnement.Count != 0)
                {
                    tab2_cbo_produit.ItemsSource = null;
                    tab2_cbo_produit.ItemsSource = _LstAbonnement;
                    tab2_cbo_produit.DisplayMemberPath = "PRODUIT";
                    this.tab2_cbo_produit.SelectedItem = _LstAbonnement[0];

                    _LeClientSelect = (Galatee.Silverlight.ServiceAccueil.CsClientRechercher)dataGrid1.SelectedItem;
                    _LeClientSelect.FK_IDABON = _LstAbonnement[0].PK_ID;
                    RetourneCanalisation(_LeClientSelect, _LstAbonnement);

                    RemplireAbonParProduit(_LstAbonnement[0]);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RemplireAbonParProduit(CsAbon _LeAbonnementSelectionne)
        {

            try
            {
                if (_LeAbonnementSelectionne.NOMBREDEFOYER == 0 || _LeAbonnementSelectionne.NOMBREDEFOYER == null)
                {
                    lbl_NbreFoyer.Visibility = System.Windows.Visibility.Collapsed;
                    this.tab2_txt_NombreFoyer.Visibility = System.Windows.Visibility.Collapsed;
                }
                this.tab2_txt_LibelleProduit.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.LIBELLEPRODUIT) ? string.Empty : _LeAbonnementSelectionne.LIBELLEPRODUIT;
                this.tab2_txt_CodeTarif.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.LIBELLETARIF) ? string.Empty : _LeAbonnementSelectionne.LIBELLETARIF;
                this.tab_txt_LibellePrimeFixe.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.LIBELLEFORFAIT) ? string.Empty : _LeAbonnementSelectionne.LIBELLEFORFAIT;
                this.tab2_txt_LibelleMoisIndex.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.LIBELLEMOISIND) ? string.Empty : _LeAbonnementSelectionne.LIBELLEMOISIND;
                this.tab2_txt_LibMoisFact.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.LIBELLEMOISFACT) ? string.Empty : _LeAbonnementSelectionne.LIBELLEMOISFACT;
                this.tab_txt_CodePussanceSoucrite.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.PUISSANCE.ToString()) ? string.Empty : Convert.ToDecimal(_LeAbonnementSelectionne.PUISSANCE).ToString("N2");
                //this.tab2_txt_puissanceutilise.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.PUISSANCEUTILISEE.ToString()) ? string.Empty : Convert.ToDecimal(_LeAbonnementSelectionne.PUISSANCEUTILISEE).ToString("N2");
                this.tab2_txt_NombreFoyer.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.NOMBREDEFOYER.ToString()) ? string.Empty : Convert.ToInt32(_LeAbonnementSelectionne.NOMBREDEFOYER).ToString(SessionObject.FormatMontant);
                this.tab2_txt_Ristourne.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.RISTOURNE.ToString()) ? string.Empty : Convert.ToDecimal(_LeAbonnementSelectionne.RISTOURNE).ToString("N2");
                this.tab2_txt_LibelleFrequence.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.LIBELLEFREQUENCE) ? string.Empty : _LeAbonnementSelectionne.LIBELLEFREQUENCE;
                this.tab2_txt_Avance.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.AVANCE.ToString()) ? string.Empty : Convert.ToDecimal(_LeAbonnementSelectionne.AVANCE).ToString("N2");
                this.tab2_txt_dateAbon.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.DABONNEMENT.ToString()) ? string.Empty : _LeAbonnementSelectionne.DABONNEMENT.ToString();
                this.tab2_txt_DateMisaJour.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.DATECREATION.ToString()) ? string.Empty : _LeAbonnementSelectionne.DATECREATION.ToString();
                this.tab2_txt_dateresile.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.DRES.ToString()) ? string.Empty : _LeAbonnementSelectionne.DRES.ToString();
                this.tab2_txt_DateModif.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.DATEMODIFICATION.ToString()) ? string.Empty : _LeAbonnementSelectionne.DATEMODIFICATION.ToString();

                if (_LeAbonnementSelectionne.REGROU == "1")
                    this.rdb_gprInvoiceYes.IsChecked = true;
                else
                    this.rdb_GprInvoiceNo.IsChecked = true;
                this.tab2_txt_NbreFacture.Text = string.IsNullOrEmpty(_LeAbonnementSelectionne.NBFAC.ToString()) ? "0" : _LeAbonnementSelectionne.NBFAC.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void tab2_cbo_produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.tab2_cbo_produit.SelectedIndex >= 0)
                    RemplireAbonParProduit((CsAbon)this.tab2_cbo_produit.SelectedItem);
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, Langue.lbl_Menu);

            }
        }

        private void RetourneEvenement(CsCanalisation leCompteur)
        {
            try
            {
                int res = LoadingManager.BeginLoading(Langue.En_Cours);
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneEvenementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                    lstEvenement.AddRange(args.Result);

                    if (lstEvenement != null)
                        RemplireOngletEvenement(lstEvenement);
                    else
                        RemplireOngletEvenement(new List<CsEvenement>());

                    LoadingManager.EndLoading(res);
                };
                service.RetourneEvenementAsync(leCompteur.FK_IDCENTRE, leCompteur.CENTRE, leCompteur.CLIENT, leCompteur.ORDRE, leCompteur.PRODUIT, leCompteur.POINT);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private void RetourneClient(Galatee.Silverlight.ServiceAccueil.CsClientRechercher leClient)
        {
            try
            {
                int res = LoadingManager.BeginLoading(Langue.En_Cours);
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneClientCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                    CsClient leCsClient = args.Result;
                    if (leCsClient != null)
                        RemplireOngletClient(leCsClient);
                    else
                        RemplireOngletClient(new CsClient());

                    LoadingManager.EndLoading(res);
                };
                service.RetourneClientAsync(leClient.FK_IDCENTRE, leClient.CENTRE, leClient.CLIENT, leClient.ORDRE);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }
        private void RemplireOngletClient(CsClient _LeClient)
        {
            try
            {
                this.tab1_txt_LibelleCentre.Text = string.IsNullOrEmpty(this.Txt_LibelleCentre.Text) ? string.Empty : this.Txt_LibelleCentre.Text;
                if (_LeClient != null)
                {

                    if (string.IsNullOrEmpty(_LeClient.PROPRIO))
                        this.rdb_Tenant.IsChecked = true;
                    else
                        this.rdb_Owner.IsChecked = true;
                    string LibCivilite = string.Empty;
                    if (!string.IsNullOrEmpty(_LeClient.DENABON))
                    {
                        CsDenomination leDenom = SessionObject.LstCivilite.FirstOrDefault(t => t.CODE == _LeClient.DENABON);
                        if (leDenom != null && !string.IsNullOrEmpty(leDenom.LIBELLE))
                            LibCivilite = leDenom.LIBELLE;
                    }
                    this.tab12_txt_NomClient.Text = (string.IsNullOrEmpty(_LeClient.NOMABON) ? string.Empty : LibCivilite + "  " + _LeClient.NOMABON);

                    this.tab12_txt_Telephone1.Text = string.IsNullOrEmpty(_LeClient.TELEPHONE) ? string.Empty : _LeClient.TELEPHONE;
                    this.tab12_txt_addresse.Text = string.IsNullOrEmpty(_LeClient.ADRMAND1) ? string.Empty : _LeClient.ADRMAND1;
                    this.tab12_txt_addresse2.Text = string.IsNullOrEmpty(_LeClient.ADRMAND2) ? string.Empty : _LeClient.ADRMAND2;
                    this.txt_NINA.Text = string.IsNullOrEmpty(_LeClient.NUMEROIDCLIENT) ? string.Empty : _LeClient.NUMEROIDCLIENT;
                    this.tab12_Txt_LibelleGroupeCode.Text = string.IsNullOrEmpty(_LeClient.LIBELLEREGCLI) ? string.Empty : "(" + _LeClient.REGROUPEMENT + ")" + _LeClient.LIBELLEREGCLI;
                    this.tab12_Txt_LibelleCodeConso.Text = string.IsNullOrEmpty(_LeClient.LIBELLECODECONSO) ? string.Empty : _LeClient.LIBELLECODECONSO;
                    this.tab12_Txt_LibelleCategorie.Text = string.IsNullOrEmpty(_LeClient.LIBELLECATEGORIE) ? string.Empty : _LeClient.LIBELLECATEGORIE;
                    this.tab12_Txt_LibelleEtatClient.Text = string.IsNullOrEmpty(_LeClient.LIBELLERELANCE) ? string.Empty : _LeClient.LIBELLERELANCE;
                    this.tab12_Txt_Nationnalite.Text = string.IsNullOrEmpty(_LeClient.LIBELLENATIONALITE) ? string.Empty : _LeClient.LIBELLENATIONALITE;
                    this.tab12_Txt_Datecreate.Text = string.IsNullOrEmpty(_LeClient.DATECREATION.ToString()) ? string.Empty : Convert.ToDateTime(_LeClient.DATECREATION).ToShortDateString();
                    this.tab12_Txt_DateModif.Text = string.IsNullOrEmpty(_LeClient.DATEMODIFICATION.ToString()) ? string.Empty : Convert.ToDateTime(_LeClient.DATEMODIFICATION).ToShortDateString();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
        }
        private void RetourneBranchement(Galatee.Silverlight.ServiceAccueil.CsClientRechercher leClient)
        {
            try
            {
                int res = LoadingManager.BeginLoading(Langue.En_Cours);
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneBranchementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                    List<CsBrt> lebranchement = args.Result;
                    if (lebranchement != null && lebranchement.Count != 0)
                    {
                        RemplireOngletbranchement(lebranchement);
                        RetourneScellageBranchement(lebranchement.First());
                    }
                    else
                        RemplireOngletbranchement(new List<CsBrt>());

                    LoadingManager.EndLoading(res);
                };
                service.RetourneBranchementAsync(leClient.FK_IDCENTRE, leClient.CENTRE, leClient.CLIENT, string.Empty);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private void RetourneScellageBranchement(CsBrt leClient)
        {
            try
            {
                int res = LoadingManager.BeginLoading(Langue.En_Cours);
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneScellageCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                    List<CsOrganeScelleDemande> leScellle = args.Result;
                    if (leScellle != null)
                        RemplireOngletScellage(leScellle);
                    else
                        RemplireOngletScellage(new List<CsOrganeScelleDemande>());

                    LoadingManager.EndLoading(res);
                };
                service.RetourneScellageAsync(leClient.PK_ID);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }
        private void RemplireOngletScellage(List<CsOrganeScelleDemande> _LstScelle)
        {
            try
            {
                this.tab4_dataScelle.ItemsSource = null;
                if (_LstScelle != null && _LstScelle.Count != 0)
                    this.tab4_dataScelle.ItemsSource = _LstScelle;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerCodeRegroupement()
        {
            try
            {
                if (SessionObject.LstCodeRegroupement != null && SessionObject.LstCodeRegroupement.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCodeRegroupementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCodeRegroupement = args.Result;
                };
                service.RetourneCodeRegroupementAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private void RemplireOngletbranchement(List<CsBrt> _LstBrt)
        {
            try
            {
                this.tab6_Txt_Centre.Text = string.IsNullOrEmpty(_LeClientSelect.CENTRE) ? string.Empty : _LeClientSelect.CENTRE;
                this.tab6_txt_Client.Text = string.IsNullOrEmpty(_LeClientSelect.CLIENT) ? string.Empty : _LeClientSelect.CLIENT;
                this.tab6_txt_LibelleCentre.Text = string.IsNullOrEmpty(this.Txt_LibelleCentre.Text) ? string.Empty : this.Txt_LibelleCentre.Text;
                if (_LstBrt != null && _LstBrt.Count != 0)
                {
                    tab6_cbo_produit.ItemsSource = null;
                    tab6_cbo_produit.ItemsSource = _LstBrt.ToList();
                    tab6_cbo_produit.DisplayMemberPath = "PRODUIT";
                    tab6_cbo_produit.SelectedItem = _LstBrt[0];

                    if (!string.IsNullOrEmpty(_LstBrt[0].LONGITUDE) || !string.IsNullOrEmpty(_LstBrt[0].LATITUDE))
                    {
                        FlyOutMessage(null);

                        CsAbonneCarte abonneSelectionnne = new CsAbonneCarte();
                        abonneSelectionnne.Centre = _LeClientSelect.CENTRE;
                        abonneSelectionnne.NumeroClient = _LeClientSelect.CLIENT;
                        abonneSelectionnne.Latitude = _LstBrt[0].LATITUDE;
                        abonneSelectionnne.Longitude = _LstBrt[0].LONGITUDE;
                        abonneSelectionnne.NomAbonne = _LeClientSelect.NOMABON;
                        LocationConverter locationConverter = new LocationConverter();

                        Location location = new Location(double.Parse(abonneSelectionnne.Latitude), double.Parse(abonneSelectionnne.Longitude));
                        this.Map.ZoomLevel = 15;
                        this.Map.Center = location;

                        Pushpin pushpin = new Pushpin();

                        listeClientsSurCarte.Add(abonneSelectionnne);

                        // Gestion du clic sur un pushpin
                        pushpin.DataContext = abonneSelectionnne;
                        pushpin.MouseLeftButtonUp += PushPinClick;

                        this.PushpinsLayer.AddChild(pushpin, location);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RemplireBranchementParProduit(CsBrt _LeBrtSelectionne)
        {
            try
            {
                this.tab6_txt_LibelleProduit.Text = string.IsNullOrEmpty(_LeBrtSelectionne.LIBELLEPRODUIT) ? string.Empty : _LeBrtSelectionne.LIBELLEPRODUIT;
                if (_LeBrtSelectionne.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    this.tab6_stab1_Txt_LibelleTypeBrt.Text = string.IsNullOrEmpty(_LeBrtSelectionne.LIBELLETYPECOMPTAGE) ? string.Empty : _LeBrtSelectionne.LIBELLETYPECOMPTAGE;
                else
                    this.tab6_stab1_Txt_LibelleTypeBrt.Text = string.IsNullOrEmpty(_LeBrtSelectionne.LIBELLETYPEBRANCHEMENT) ? string.Empty : _LeBrtSelectionne.LIBELLETYPEBRANCHEMENT;

                this.tab6_stab1_txt_LongueurBrt.Text = _LeBrtSelectionne.LONGBRT.ToString();
                this.tab6_stab1_txt_NbPoint.Text = _LeBrtSelectionne.NBPOINT.ToString();
                this.tab2_txt_puissanceutilise.Text = _LeBrtSelectionne.PUISSANCEINSTALLEE == null ? string.Empty : _LeBrtSelectionne.PUISSANCEINSTALLEE.Value.ToString(); ;
                this.tab6_stab1_txt_Longitude.Text = string.IsNullOrEmpty(_LeBrtSelectionne.LONGITUDE) ? string.Empty : _LeBrtSelectionne.LONGITUDE;
                this.tab6_stab1_txt_Latitude.Text = string.IsNullOrEmpty(_LeBrtSelectionne.LATITUDE) ? string.Empty : _LeBrtSelectionne.LATITUDE;
                this.tab6_stab1_txt_AdresseReseau.Text = string.IsNullOrEmpty(_LeBrtSelectionne.ADRESSERESEAU) ? string.Empty : _LeBrtSelectionne.ADRESSERESEAU;
                this.tab6_stab1_txt_Datecreat.Text = string.IsNullOrEmpty(_LeBrtSelectionne.DATECREATION.ToString()) ? string.Empty : Convert.ToDateTime(_LeBrtSelectionne.DATECREATION).ToShortDateString();
                this.tab6_stab1_txt_DateModif.Text = string.IsNullOrEmpty(_LeBrtSelectionne.DATEMODIFICATION.ToString()) ? string.Empty : Convert.ToDateTime(_LeBrtSelectionne.DATEMODIFICATION).ToShortDateString();

                if (_LeBrtSelectionne.SERVICE == "1")
                    this.tab6_stab1_rdb_InService.IsChecked = true;
                else this.tab6_stab1_rdb_deconnecter.IsChecked = true;

                this.tab6_stab1_txt_dateracordement.Text = Convert.ToDateTime(_LeBrtSelectionne.DRAC).ToShortDateString();
                this.tab6_stab1_txt_dateresil.Text = _LeBrtSelectionne.DRES.ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void tab6_cbo_produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.tab6_cbo_produit.SelectedIndex >= 0)
                    RemplireBranchementParProduit((CsBrt)this.tab6_cbo_produit.SelectedItem);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);

            }
        }

        private void RetourneCanalisation(Galatee.Silverlight.ServiceAccueil.CsClientRechercher leClient, List<CsAbon> lesAbon)
        {
            try
            {
                int res = LoadingManager.BeginLoading(Langue.En_Cours);
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCanalisationClasseurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                    List<CsCanalisation> lstCompteur = args.Result;
                    if (lstCompteur != null)
                        RemplireOngletCanalisation(lstCompteur);
                    else
                        RemplireOngletCanalisation(new List<CsCanalisation>());

                    LoadingManager.EndLoading(res);
                };
                //service.RetourneCanalisationClasseurAsync(leClient.FK_IDCENTRE, leClient.CENTRE, leClient.CLIENT, leClient.FK_IDABON, null, null);
                service.RetourneCanalisationClasseurAsync(leClient.FK_IDCENTRE, leClient.CENTRE, leClient.CLIENT, lesAbon, null, null);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }
        private void RemplireOngletCanalisation(List<CsCanalisation> _LstCannalisation)
        {
            try
            {

                this.tab5_txt_LibelleCentre.Text = string.IsNullOrEmpty(this.Txt_LibelleCentre.Text) ? string.Empty : this.Txt_LibelleCentre.Text;
                if (_LstCannalisation != null && _LstCannalisation.Count != 0)
                {
                    foreach (CsCanalisation item in _LstCannalisation)
                    {
                        item.ORDRE = _LeClientSelect.ORDRE;
                        RetourneEvenement(item);
                        if (item.DEPOSE != null)
                            item.LIBELLEETATCOMPTEUR = Langue.lbl_EtatCompteDepose;
                        else
                            item.LIBELLEETATCOMPTEUR = Langue.lbl_EtatCompteActif;

                        if (LstDeProduit.FirstOrDefault(p => p.CODE == item.PRODUIT) != null)
                            item.LIBELLEPRODUIT = LstDeProduit.FirstOrDefault(p => p.CODE == item.PRODUIT).LIBELLE;
                    }
                    tab5_dataGrid1.ItemsSource = _LstCannalisation;
                    tab5_dataGrid1.SelectedItem = _LstCannalisation[0];

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ReinitAbon();
                ReinitBranchement();
                ReinitCanalisation();
                ReinitClient();
                lstEvenement = new List<CsEvenement>();
                _LeClientSelect = (Galatee.Silverlight.ServiceAccueil.CsClientRechercher)dataGrid1.SelectedItem;
                if (_LeClientSelect != null)
                {
                    tab4_dataGridAllCompteClient.ItemsSource = null;
                    tab5_Stab2dataGrid2.ItemsSource = null;
                    RetourneClasseurClient(_LeClientSelect);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }

        private void RetourneScellageCompteur(CsCanalisation leCompteur)
        {
            try
            {
                int res = LoadingManager.BeginLoading(Langue.En_Cours);
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneScellageCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                    List<CsOrganeScelleDemande> leScellle = args.Result;
                    if (leScellle != null)
                        RemplireOngletScellageCompteur(leScellle);
                    else
                        RemplireOngletScellageCompteur(new List<CsOrganeScelleDemande>());

                    LoadingManager.EndLoading(res);
                };
                service.RetourneScellageCompteurAsync(leCompteur.NUMERO, leCompteur.MARQUE);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }
        private void RemplireOngletScellageCompteur(List<CsOrganeScelleDemande> _LstScelle)
        {
            try
            {
                this.tab4_dataScelle_Copy.ItemsSource = null;
                if (_LstScelle != null && _LstScelle.Count != 0)
                    this.tab4_dataScelle_Copy.ItemsSource = _LstScelle;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




        private void RetourneLeCompteClient(Galatee.Silverlight.ServiceAccueil.CsClientRechercher leClient)
        {
            try
            {
                int res = LoadingManager.BeginLoading(Langue.En_Cours);
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneLeCompteClientCompleted += (s, args) =>
                {
                    leClasseurClient = new CsCompteClient();
                    if (args != null && args.Cancelled)
                        return;

                    leClasseurClient = args.Result;
                    if (leClasseurClient != null)
                    {
                        decimal _totalDebit = 0;
                        decimal _totalCredit = 0;

                        _totalDebit = decimal.Parse(leClasseurClient.ToutLClient.Where(t => t.DC == SessionObject.Enumere.Debit).Sum(p => p.MONTANT).ToString());
                        this.tab4_txt_TotalDebit.Text = _totalDebit.ToString(SessionObject.FormatMontant);

                        _totalCredit = leClasseurClient.ToutLClient.Where(t => t.DC == SessionObject.Enumere.Credit).Sum(p => decimal.Parse(p.MONTANT.ToString()));
                        this.tab4_txt_TotalCredit.Text = _totalCredit.ToString(SessionObject.FormatMontant);
                        tab4_txt_balance.Text = (_totalDebit - _totalCredit).ToString(SessionObject.FormatMontant);

                        LstReglementClient = new List<CsLclient>();
                        LstFactureClient = new List<CsLclient>();
                        LstImpayesClient = new List<CsLclient>();

                        if (leClasseurClient.LstFacture != null)
                            LstFactureClient = leClasseurClient.LstFacture;
                        if (leClasseurClient.LstReglement != null)
                            LstReglementClient = leClasseurClient.LstReglement;

                        if (leClasseurClient.Impayes != null)
                            LstImpayesClient = leClasseurClient.Impayes;
                        RemplirTypeAction(0);
                    }

                    LoadingManager.EndLoading(res);
                };
                service.RetourneLeCompteClientAsync(leClient.FK_IDCENTRE, leClient.CENTRE, leClient.CLIENT, leClient.ORDRE);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }
        private void RemplirTypeAction(int Index)
        {
            try
            {
                tab4_dataGridAllCompteClient.ItemsSource = null;
                tab4_dataGridReglement.ItemsSource = null;
                tab4_dataGridFacture.ItemsSource = null;
                tab4_dataGridImpaye.ItemsSource = null;

                int caseSwitch = Index;
                switch (caseSwitch)
                {
                    case 0:
                        {
                            this.tab4_dataGridMandatement.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGridFacture.Visibility = System.Windows.Visibility.Visible;
                            this.tab4_dataGridImpaye.Visibility = System.Windows.Visibility.Collapsed;
                            if (_LstLClient != null && _LstLClient.Count != 0)
                            {
                                tab4_dataGridFacture.ItemsSource = null;
                                tab4_dataGridFacture.ItemsSource = _LstLClient;

                                decimal _totalDebit = 0;
                                decimal _totalCredit = 0;
                                decimal _SoldeRegroupement = 0;

                                this.tab4_txt_TotalCredit.Visibility = System.Windows.Visibility.Visible ;

                                _totalDebit = decimal.Parse(_LstLClient.Sum(o => o.MONTANT).ToString());
                                this.tab4_txt_TotalDebit.Text = _totalDebit.ToString(SessionObject.FormatMontant);

                                _totalCredit = decimal.Parse(_LstLClient.Sum(o => o.MONTANTCREDIT).ToString());
                                this.tab4_txt_TotalCredit.Text = _totalCredit.ToString(SessionObject.FormatMontant);

                                _SoldeRegroupement = decimal.Parse(_LstLClient.Sum(o => o.SOLDEFACTURE).ToString());
                                tab4_txt_balance.Text = _SoldeRegroupement.ToString(SessionObject.FormatMontant);
                            }
                        }
                        break;
                    case 1:
                        {
                            
                        }
                        break;
                    case 2:
                        {
                            this.tab4_dataGridMandatement.Visibility = System.Windows.Visibility.Collapsed;
                            this.tab4_dataGridFacture.Visibility = System.Windows.Visibility.Visible;
                            this.tab4_dataGridImpaye.Visibility = System.Windows.Visibility.Collapsed;
                            tab4_dataGridFacture.ItemsSource = null;
                            tab4_dataGridFacture.ItemsSource = _LstLClient.Where(t => t.SOLDEFACTURE != 0).ToList();

                            decimal _totalDebit = 0;

                            _totalDebit = decimal.Parse(_LstLClient.Where(t => t.SOLDEFACTURE != 0).Sum(o => o.SOLDEFACTURE).ToString());
                            this.tab4_txt_TotalDebit.Text = _totalDebit.ToString(SessionObject.FormatMontant);
                            
                            this.tab4_txt_TotalCredit.Visibility = System.Windows.Visibility.Collapsed;
                            tab4_txt_balance.Text = _totalDebit.ToString(SessionObject.FormatMontant);
                        }
                        break;
                    case 3:
                        {
                            this.tab4_dataGridMandatement.Visibility = System.Windows.Visibility.Visible ;
                            this.tab4_dataGridFacture.Visibility = System.Windows.Visibility.Collapsed ;
                            this.tab4_dataGridImpaye.Visibility = System.Windows.Visibility.Collapsed;
                            tab4_dataGridMandatement.ItemsSource = null;
                            tab4_dataGridMandatement.ItemsSource = _LstMandatement;
                        }
                        break;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            initControl();
        }



        private void button4_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                ReinitAbon();
                ReinitBranchement();
                ReinitCanalisation();
                ReinitClient();
                if (this.txt_Regroupement.Tag != null)
                {
                    RechercheClientRegroupement((CsRegCli)this.txt_Regroupement.Tag);
                    RechercheMandatementRegroupement((CsRegCli)this.txt_Regroupement.Tag);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void ReinitClient()
        {
            try
            {
                this.tab1_txt_LibelleCentre.Text = string.Empty;
                this.tab12_txt_NomClient.Text = string.Empty;
                this.tab12_txt_Telephone1.Text = string.Empty;
                this.tab12_txt_addresse.Text = string.Empty;
                this.tab12_txt_addresse2.Text = string.Empty;
                this.txt_NINA.Text = string.Empty;
                this.tab12_txt_NomProprietaire.Text = string.Empty;
                this.tab12_Txt_LibelleGroupeCode.Text = string.Empty;
                this.tab12_Txt_LibelleCodeConso.Text = string.Empty;
                this.tab12_Txt_LibelleCategorie.Text = string.Empty;
                this.tab12_Txt_LibelleEtatClient.Text = string.Empty;
                this.tab12_Txt_Nationnalite.Text = string.Empty;
                this.tab12_Txt_Datecreate.Text = string.Empty;
                this.tab12_Txt_DateModif.Text = string.Empty;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ReinitAbon()
        {

            try
            {
                this.tab2_txt_LibelleProduit.Text = string.Empty;
                this.tab2_txt_CodeTarif.Text = string.Empty;
                this.tab_txt_LibellePrimeFixe.Text = string.Empty;
                this.tab2_txt_LibelleMoisIndex.Text = string.Empty;
                this.tab2_txt_LibMoisFact.Text = string.Empty;
                this.tab_txt_CodePussanceSoucrite.Text = string.Empty;
                this.tab2_txt_puissanceutilise.Text = string.Empty;
                this.tab2_txt_NombreFoyer.Text = string.Empty;
                this.tab2_txt_Ristourne.Text = string.Empty;
                this.tab2_txt_LibelleFrequence.Text = string.Empty;
                this.tab2_txt_Avance.Text = string.Empty;
                this.tab2_txt_dateAbon.Text = string.Empty;
                this.tab2_txt_DateMisaJour.Text = string.Empty;
                this.tab2_txt_dateresile.Text = string.Empty;
                this.tab2_txt_DateModif.Text = string.Empty;
                this.tab2_txt_NbreFacture.Text = string.Empty;
                this.tab2_cbo_produit.SelectedItem = null;
                this.tab6_cbo_produit.SelectedItem = null;
                this.tab4_cbo_Operation.SelectedItem = null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ReinitBranchement()
        {
            try
            {
                this.tab6_txt_LibelleProduit.Text = string.Empty;
                this.tab6_stab1_Txt_LibelleTypeBrt.Text = string.Empty;

                this.tab6_stab1_txt_LongueurBrt.Text = string.Empty;
                this.tab6_stab1_txt_NbPoint.Text = string.Empty;
                this.tab6_stab1_txt_Longitude.Text = string.Empty;
                this.tab6_stab1_txt_Latitude.Text = string.Empty;
                this.tab6_stab1_txt_AdresseReseau.Text = string.Empty;
                this.tab6_stab1_txt_Datecreat.Text = string.Empty;
                this.tab6_stab1_txt_DateModif.Text = string.Empty;
                this.tab6_stab1_rdb_InService.IsChecked = false;
                this.tab6_stab1_txt_dateracordement.Text = string.Empty;
                this.tab6_stab1_txt_dateresil.Text = string.Empty;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ReinitCanalisation()
        {
            try
            {
                this.tab5_txt_NumCompteur.Text = string.Empty;
                this.tab5_txt_AnnefabricCompteur.Text = string.Empty;
                this.tab5_txt_LibelleTypeCompteur.Text = string.Empty;
                this.tab5_txt_NumCompteur.Text = string.Empty;
                this.tab5_txt_MarqueCompteur.Text = string.Empty;
                this.tab5_txt_LibelleDiametreCompteur.Text = string.Empty;
                this.tab5_txt_CoefDeMultiplication.Text = string.Empty;
                this.tab5_Chk_CoefMultiplication.IsChecked = false;
                this.tab5_txt_localisationCompteur.Text = string.Empty;
                this.tab5_txt_DateMiseEnService.Text = string.Empty;
                this.tab5_txt_DateFinServce.Text = string.Empty;
                this.tab5_txt_DateCreate.Text = string.Empty;
                this.tab5_txt_LibelleDigit.Text = string.Empty;
                tab5_dataGrid1.ItemsSource = null;
                tab5_Stab2dataGrid2.ItemsSource = null;
                dtg_Demande.ItemsSource = null;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //private void RetourneClasseurClient(CsClientRechercher _LeClientRechercher)
        //{
        //    int res = LoadingManager.BeginLoading(Langue.En_Cours);
        //    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Accueil"));
        //    service.RetourneClasseurClientCompleted += (s, args) =>
        //    {
        //        if (args != null && args.Cancelled)
        //            return;
        //        this.tab3_txt_Centre.Text = _LeClientRechercher.CENTRE ;
        //        this.tab3_txt_Client.Text = _LeClientRechercher.CLIENT ;
        //        this.tab3_txt_Ordre.Text = _LeClientRechercher.ORDRE ;

        //        this.tab1_Txt_Centre.Text = _LeClientRechercher.CENTRE;
        //        this.tab1_txt_Client.Text = _LeClientRechercher.CLIENT;
        //        this.tab_txt_Ordre.Text = _LeClientRechercher.ORDRE;

        //        this.tab2_txt_centre.Text = _LeClientRechercher.CENTRE;
        //        this.tab2_txt_client.Text = _LeClientRechercher.CLIENT;
        //        this.tab2_txt_Ordre.Text = _LeClientRechercher.ORDRE;

        //        this.tab6_Txt_Centre.Text = _LeClientRechercher.CENTRE;
        //        this.tab6_txt_Client.Text = _LeClientRechercher.CLIENT;

        //        this.tab5_Txt_Centre.Text = _LeClientRechercher.CENTRE;
        //        this.tab5_txt_Client.Text = _LeClientRechercher.CLIENT;



        //        this.tab4_txt_Centre.Text = _LeClientRechercher.CENTRE;
        //        this.tab4_txt_Client .Text = _LeClientRechercher.CLIENT;
        //        this.tab4_txt_Ordre.Text = _LeClientRechercher.ORDRE;

        //        _LeClasseur = new CsClasseurClient();
        //        _LeClasseur = args.Result;
        //        if (_LeClasseur != null)
        //        {
        //            if (_LeClasseur.Ag != null )
        //                RemplireOngletAdresse(_LeClasseur.Ag);
        //            else
        //                RemplireOngletAdresse(new CsAg ());

        //            if (_LeClasseur.LeClient != null)
        //                RemplireOngletClient(_LeClasseur.LeClient);
        //            else
        //                RemplireOngletClient(new CsClient());

        //            if (_LeClasseur.LstAbonnement != null && _LeClasseur.LstAbonnement.Count != 0)
        //                RemplireOngletAbon(_LeClasseur.LstAbonnement);
        //            else
        //                RemplireOngletAbon(new List<CsAbon>());

        //            if (_LeClasseur.LstBranchement != null && _LeClasseur.LstBranchement.Count != 0)
        //                RemplireOngletbranchement(_LeClasseur.LstBranchement);
        //            else
        //                RemplireOngletbranchement(new List<CsBrt>());

        //            if (_LeClasseur.LstCanalistion != null && (_LeClasseur.LstCanalistion.Count != 0))                                           
        //                RemplireOngletCanalisation(_LeClasseur.LstCanalistion);
        //            else
        //                RemplireOngletCanalisation(new List<CsCanalisation>());



        //            if (_LeClasseur.LeCompteClient  != null)
        //            {
        //                decimal _totalDebit = 0;
        //                decimal _totalCredit = 0;

        //                _totalDebit = decimal.Parse(_LeClasseur.LeCompteClient.ToutLClient.Where(t=>t.DC == SessionObject.Enumere.Debit).Sum(p => p.MONTANT).ToString());
        //                //this.tab4_txt_TotalDebit.Text = Math.Abs(_totalDebit).ToString("N2");
        //                this.tab4_txt_TotalDebit.Text = ThousandsSeparator.formate(_totalDebit);

        //                _totalCredit = _LeClasseur.LeCompteClient.ToutLClient.Where(t => t.DC == SessionObject.Enumere.Credit).Sum(p => decimal.Parse(p.MONTANT.ToString()));
        //                //this.tab4_txt_TotalCredit.Text = Math.Abs(_totalCredit).ToString("N2");
        //                this.tab4_txt_TotalCredit.Text = ThousandsSeparator.formate(_totalCredit);
        //                 tab4_txt_balance.Text =ThousandsSeparator.formate( (_totalDebit - _totalCredit));

        //                 LstReglementClient = new List<CsTranscaisse >();
        //                 LstFactureClient = new List<CsLclient >();
        //                 if (_LeClasseur.LeCompteClient.LstFacture != null)
        //                     LstFactureClient = _LeClasseur.LeCompteClient.LstFacture ;
        //                 if (_LeClasseur.LeCompteClient.LstReglement  != null)
        //                     LstReglementClient  = _LeClasseur.LeCompteClient.LstReglement;
        //                 RemplirTypeAction(0);
        //            }
        //        }
        //        LoadingManager.EndLoading(res);
        //    };
        //    service.RetourneClasseurClientAsync(_LeClientRechercher);
        //    service.CloseAsync();
        //}

        private void RetourneClasseurClient(Galatee.Silverlight.ServiceAccueil.CsClientRechercher _LeClientRechercher)
        {

            this.tab3_txt_Centre.Text = _LeClientRechercher.CENTRE;
            this.tab3_txt_Client.Text = _LeClientRechercher.CLIENT;
            this.tab3_txt_Ordre.Text = _LeClientRechercher.ORDRE;

            this.tab1_Txt_Centre.Text = _LeClientRechercher.CENTRE;
            this.tab1_txt_Client.Text = _LeClientRechercher.CLIENT;
            this.tab_txt_Ordre.Text = _LeClientRechercher.ORDRE;

            this.tab2_txt_centre.Text = _LeClientRechercher.CENTRE;
            this.tab2_txt_client.Text = _LeClientRechercher.CLIENT;
            this.tab2_txt_Ordre.Text = _LeClientRechercher.ORDRE;

            this.tab6_Txt_Centre.Text = _LeClientRechercher.CENTRE;
            this.tab6_txt_Client.Text = _LeClientRechercher.CLIENT;

            this.tab5_Txt_Centre.Text = _LeClientRechercher.CENTRE;
            this.tab5_txt_Client.Text = _LeClientRechercher.CLIENT;

            this.tab4_txt_Centre.Text = _LeClientRechercher.CENTRE;
            this.tab4_txt_Client.Text = _LeClientRechercher.CLIENT;
            this.tab4_txt_Ordre.Text = _LeClientRechercher.ORDRE;

            RetourneClient(_LeClientRechercher);
            RetourneAdresse(_LeClientRechercher);
            RetourneAbonnement(_LeClientRechercher);
            RetourneBranchement(_LeClientRechercher);
            RetourneCanalisation(_LeClientRechercher, lesAbonnements);
            //RetourneLeCompteClient(_LeClientRechercher);
            RetourneDemandeClient(_LeClientRechercher);
        }

        private void RetourneDemandeClient(ServiceAccueil.CsClientRechercher _LeClientRechercher)
        {
            try
            {
                int res = LoadingManager.BeginLoading(Langue.En_Cours);
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneListeDemandeClientCompleted += (s, args) =>
                {

                    if ((args != null && args.Cancelled) || args.Result == null)
                        return;

                    ListDemande = args.Result;
                    if (ListDemande != null && ListDemande.Count != 0)
                    {
                        foreach (CsDemandeBase item in ListDemande)
                            item.LIBELLESTATUT = RetourneLibelleStatutDemande(item);
                        dtg_Demande.ItemsSource = null;
                        dtg_Demande.ItemsSource = ListDemande;
                    }
                    LoadingManager.EndLoading(res);
                };
                service.RetourneListeDemandeClientAsync(_LeClientRechercher);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }
        List<CsTdem> LstTypeDemande = new List<CsTdem>();
        private void ChargeTypeDemande(string typeDemande)
        {
            try
            {

                if (SessionObject.LstTypeDemande != null && SessionObject.LstTypeDemande.Count != 0)
                    LstTypeDemande = SessionObject.LstTypeDemande;
                else
                {
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service1.RetourneOptionDemandeCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        LstTypeDemande = res.Result;
                        SessionObject.LstTypeDemande = LstTypeDemande;
                    };
                    service1.RetourneOptionDemandeAsync(typeDemande);
                    service1.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }
        private string RetourneLibelleStatutDemande(CsDemandeBase _laDemande)
        {
            string Libelle = string.Empty;
            if (_laDemande.STATUT == "1")
                Libelle = Langue.lib_Statut_EnAttente;

            else if (_laDemande.STATUT == "11")
                Libelle = Langue.lib_Statut_EnCaisse;

            else if (_laDemande.STATUT == "2")
                Libelle = Langue.lib_Statut_DejaEncCaisse;

            else if (_laDemande.STATUT == "3")
                Libelle = Langue.lib_Statut_terminer;

            return Libelle;

        }
        private void FlyOutMessage(string message, int? delay = null)
        {
            if (string.IsNullOrEmpty(message))
                this.MessageContent.Visibility = System.Windows.Visibility.Collapsed;
            else
            {
                this.MessageContent.Visibility = System.Windows.Visibility.Visible;
                this.StatusMessage.Text = message;
                if (delay != null || delay > 0)
                {
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = new TimeSpan(0, 0, (int)delay);
                    timer.Start();
                    timer.Tick += (send, args) =>
                    {
                        this.MessageContent.Visibility = System.Windows.Visibility.Collapsed;
                        timer.Stop();
                    };
                }
            }
        }
        ObservableCollection<CsAbonneCarte> listeClientsSurCarte = new ObservableCollection<CsAbonneCarte>();
        private void PushPinClick(object sender, MouseButtonEventArgs args)
        {
            var pushpin = sender as Pushpin;
            var abonne = pushpin.DataContext as CsAbonneCarte;

            PushpinDetails pushpinDetails = new PushpinDetails(abonne);
            Location location = new Location(double.Parse(abonne.Latitude), double.Parse(abonne.Longitude));
            PushpinsLayer.AddChild(pushpinDetails, location);
        }

        private void RemplireOngletEvenement(List<CsEvenement> _LstEvenement)
        {
            try
            {
                tab5_Stab2dataGrid2.ItemsSource = null;
                if (_LstEvenement != null && _LstEvenement.Count != 0)
                {
                    this.tab1_Txt_Centre.Text = _LstEvenement[0].CENTRE;
                    this.tab1_txt_Client.Text = _LstEvenement[0].CLIENT;
                    this.tab1_txt_LibelleCentre.Text = string.IsNullOrEmpty(this.Txt_LibelleCentre.Text) ? string.Empty : this.Txt_LibelleCentre.Text;
                    if (_LstEvenement != null && _LstEvenement.Count != 0)
                    {
                        if (_LstEvenement.First().PRODUIT == SessionObject.Enumere.ElectriciteMT)
                            _LstEvenement.ForEach(t => t.REGLAGECOMPTEUR = t.TYPECOMPTAGE);
                        tab5_Stab2dataGrid2.ItemsSource = _LstEvenement.OrderBy(t => t.DATEEVT).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




        private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        private void tab4_cbo_Operation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.tab4_cbo_Operation.SelectedIndex >= 0)
                    RemplirTypeAction(this.tab4_cbo_Operation.SelectedIndex);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }

        private void tab5_dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.tab5_dataGrid1.SelectedIndex >= 0)
                {
                    CsCanalisation _LeCompteurSelect = (CsCanalisation)this.tab5_dataGrid1.SelectedItem;
                    RemplireCannalisationProduit(_LeCompteurSelect);
                    if (_LeCompteurSelect.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        RetourneScellageCompteur(_LeCompteurSelect);

                    if (lstEvenement != null && lstEvenement.Count != 0)
                        RemplireOngletEvenement(lstEvenement.Where(p => p.PRODUIT == _LeCompteurSelect.PRODUIT && p.POINT == _LeCompteurSelect.POINT).ToList());

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);

            }
        }
        private void RemplireCannalisationProduit(CsCanalisation _LaCannalisationSelect)
        {
            try
            {
                this.tab5_txt_NumCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.NUMERO) ? string.Empty : _LaCannalisationSelect.NUMERO;
                this.tab5_txt_AnnefabricCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.ANNEEFAB) ? string.Empty : _LaCannalisationSelect.ANNEEFAB;
                this.tab5_txt_LibelleTypeCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLETYPECOMPTEUR) ? string.Empty : _LaCannalisationSelect.LIBELLETYPECOMPTEUR;
                this.tab5_txt_NumCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.NUMERO) ? string.Empty : _LaCannalisationSelect.NUMERO;
                this.tab5_txt_MarqueCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLEMARQUE) ? string.Empty : _LaCannalisationSelect.LIBELLEMARQUE;


                if (_LaCannalisationSelect.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    this.tab5_txt_LibelleDiametreCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLEREGLAGECOMPTEUR) ? string.Empty : _LaCannalisationSelect.LIBELLEREGLAGECOMPTEUR;
                else
                    this.tab5_txt_LibelleDiametreCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.LIBELLETYPECOMPTAGE) ? string.Empty : _LaCannalisationSelect.LIBELLETYPECOMPTAGE;

                this.tab5_txt_CoefDeMultiplication.Text = _LaCannalisationSelect.COEFLECT.ToString();
                if (_LaCannalisationSelect.COEFLECT == 0)
                    this.tab5_Chk_CoefMultiplication.IsChecked = false;
                else
                    tab5_Chk_CoefMultiplication.IsChecked = true;

                this.tab5_txt_localisationCompteur.Text = string.IsNullOrEmpty(_LaCannalisationSelect.REPCOMPT) ? string.Empty : _LaCannalisationSelect.REPCOMPT;
                if (_LaCannalisationSelect.DEPOSE != null)
                    this.tab5_txt_DateMiseEnService.Text = Convert.ToDateTime(_LaCannalisationSelect.DEPOSE).ToShortDateString();

                if (_LaCannalisationSelect.DEPOSE != null)
                    this.tab5_txt_DateFinServce.Text = Convert.ToDateTime(_LaCannalisationSelect.DEPOSE).ToShortDateString();

                if (_LaCannalisationSelect.DATECREATION != null)
                    this.tab5_txt_DateCreate.Text = Convert.ToDateTime(_LaCannalisationSelect.DATECREATION).ToShortDateString();

                this.tab5_txt_LibelleDigit.Text = _LaCannalisationSelect.CADRAN.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
   
        List<Galatee.Silverlight.ServiceAccueil.CsLclient> _LstLClient = new List<Galatee.Silverlight.ServiceAccueil.CsLclient>();
        List<Galatee.Silverlight.ServiceAccueil.CsClientRechercher> _LstClient = new List<Galatee.Silverlight.ServiceAccueil.CsClientRechercher>();
        List<Galatee.Silverlight.ServiceAccueil.CsMandatementGc> _LstMandatement = new List<Galatee.Silverlight.ServiceAccueil.CsMandatementGc>();

        private void RechercheClientRegroupement(CsRegCli _LeRegroupement)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RechercherClientRegrouperCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                    {
                        LoadingManager.EndLoading(res);
                        return;
                    }
                    if (args.Result != null && args.Result.Count != 0)
                    {
                        foreach (var item in args.Result)
                        {
                            _LstClient = item.Key;
                            _LstLClient = item.Value;
                        }
                        if (_LstClient != null && _LstClient.Count != 0)
                        {
                            LoadingManager.EndLoading(res);
                            dataGrid1.ItemsSource = null;
                            System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(_LstClient);
                            dataGrid1.ItemsSource = view;
                            datapager.Source = view;
                            dataGrid1.SelectedItem = _LstClient[0];

                            tab4_dataGridFacture.ItemsSource = null;
                            tab4_dataGridFacture.ItemsSource = _LstLClient;

                            decimal _totalDebit = 0;
                            decimal _totalCredit = 0;
                            decimal _SoldeRegroupement = 0;

                            _totalDebit = decimal.Parse(_LstLClient.Sum(o=>o.MONTANT).ToString());
                            this.tab4_txt_TotalDebit.Text = _totalDebit.ToString(SessionObject.FormatMontant);

                            _totalCredit = decimal.Parse(_LstLClient.Sum(o => o.MONTANTCREDIT ).ToString());
                            this.tab4_txt_TotalCredit.Text = _totalCredit.ToString(SessionObject.FormatMontant);

                            _SoldeRegroupement = decimal.Parse(_LstLClient.Sum(o => o.SOLDEFACTURE).ToString());
                            tab4_txt_balance.Text = _SoldeRegroupement.ToString(SessionObject.FormatMontant);

                        }
                    }
                    else
                    {
                        Message.Show(Langue.MsgEltInexistent, Langue.lbl_Menu);
                        LoadingManager.EndLoading(res);
                    }

                };
                service.RechercherClientRegrouperAsync(_LeRegroupement);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                string error = ex.Message;
            }

        }

        private void RechercheMandatementRegroupement(CsRegCli _LeRegroupement)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.MandatementClientCompleted += (s, args) =>
                {
                    LoadingManager.EndLoading(res);
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result != null && args.Result.Count != 0)
                        _LstMandatement = args.Result;
                    

                };
                service.MandatementClientAsync(_LeRegroupement);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                string error = ex.Message;
            }

        }
        

        bool? IsEditionEvenement = null;
        private void btn_Editer_Click(object sender, RoutedEventArgs e)
        {
                FrmPeriodeEdition ctrperiode = new FrmPeriodeEdition();
                ctrperiode.Closed += ctrperiode_Closed;
                ctrperiode.Show();
        }
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }
        private void Btn_ImprimerEvt_Click_1(object sender, RoutedEventArgs e)
        {
            IsEditionEvenement = true;
            FrmPeriodeEdition ctrperiode = new FrmPeriodeEdition();
            ctrperiode.Closed += ctrperiode_Closed;
            ctrperiode.Show();

        }

        void ctrperiode_Closed(object sender, EventArgs e)
        {
            FrmPeriodeEdition ctrs = sender as FrmPeriodeEdition;
            if (ctrs.IsOKclick == true)
                    ImprimerCompteClient(ctrs.PeriodeDebut, ctrs.PeriodeFin);
              
        }

        void ImprimerCompteClient(string PeriodeDebut, string PeriodeFin)
        {

            if (tab4_dataGridFacture.ItemsSource != null)
                {
                    if (!string.IsNullOrEmpty(PeriodeFin)) PeriodeFin = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(PeriodeFin);
                    if (!string.IsNullOrEmpty(PeriodeDebut)) PeriodeDebut = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(PeriodeDebut);

                    List<CsLclient> ListeDobjects = ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(((List<CsLclient>)this.tab4_dataGridFacture.ItemsSource ).ToList());
                    List<CsLclient> ListeDeClient = ListeDobjects.Where(t => (string.IsNullOrEmpty(PeriodeDebut) || int.Parse(t.REFEM) >= int.Parse(PeriodeDebut)) &&
                        (string.IsNullOrEmpty(PeriodeFin) || int.Parse(t.REFEM) <= int.Parse(PeriodeFin))).ToList();
                    ListeDeClient.ForEach(y => y.REFERENCE = this.txt_Regroupement.Text);
                    Utility.ActionExportation<ServicePrintings.CsLclient, CsLclient>(ListeDeClient, null, string.Empty, SessionObject.CheminImpression, "ImpayeRegroupe", "Accueil", true, "xlsx");
                }
        }

        DateTime lastClick = DateTime.Now;
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now - lastClick).Ticks < 2500000)
                ViewButton_Click(null, null);
            lastClick = DateTime.Now;
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.dtg_Demande.SelectedItem != null)
            {
                Galatee.Silverlight.Devis.UcConsultationDevis detailForm = new Galatee.Silverlight.Devis.UcConsultationDevis(((CsDemandeBase)this.dtg_Demande.SelectedItem).PK_ID);
                detailForm.Show();
            }
            else
            {
                Message.ShowInformation("Sélectionner une demande", "Info");
                return;
            }
        }


        private void RechercheTypeAction(int Index, string PeriodeDebut, string PeriodeFin)
        {
            try
            {

                int caseSwitch = Index;
                switch (caseSwitch)
                {
                    case 0:
                        {
                            if (this.tab4_dataGridAllCompteClient.ItemsSource != null)
                            {
                                List<CsLclient> lstCompteClient = (List<CsLclient>)this.tab4_dataGridAllCompteClient.ItemsSource;
                                List<CsLclient> _ToutLeCompteClient = ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(lstCompteClient.Where(t => t.REFEM == PeriodeDebut && t.DC == "C").OrderBy(t => t.REFEM).ToList());
                                this.tab4_dataGridAllCompteClient.ItemsSource = null;
                                RemplireOngletToutLeCompte(_ToutLeCompteClient.OrderBy(t => t.REFEM).ToList());
                            }
                        }
                        break;
                    case 1:
                        {
                            if (this.tab4_dataGridReglement.ItemsSource != null)
                            {
                                List<CsLclient> lstCompteClient = (List<CsLclient>)this.tab4_dataGridReglement.ItemsSource;
                                this.tab4_dataGridReglement.ItemsSource = null;
                                RemplireOngletReglement(lstCompteClient.Where(t => t.REFEM == PeriodeDebut).OrderBy(t => t.REFEM).ToList());
                            }
                        }
                        break;
                    case 2:
                        {

                            if (this.tab4_dataGridFacture.ItemsSource != null)
                            {
                                List<CsLclient> lstFactureClient = (List<CsLclient>)this.tab4_dataGridFacture.ItemsSource;
                                this.tab4_dataGridFacture.ItemsSource = null;
                                RemplireOngletFacture(lstFactureClient.Where(t => t.REFEM == PeriodeDebut).OrderBy(t => t.REFEM).ToList());
                            }
                        }
                        break;
                    case 3:
                        {
                            this.tab4_dataGridImpaye.Visibility = System.Windows.Visibility.Visible;
                        }
                        break;


                    //case 0:
                    //    {
                    //        this.tab4_dataGridAllCompteClient.Visibility = System.Windows.Visibility.Visible;
                    //    }
                    //    break;
                    //case 1:
                    //    {
                    //        this.tab4_dataGridReglement.Visibility = System.Windows.Visibility.Visible;
                    //    }
                    //    break;
                    //case 2:
                    //    {
                    //        this.tab4_dataGridFacture.Visibility = System.Windows.Visibility.Visible;
                    //    }
                    //    break;
                    //case 3:
                    //    {
                    //        this.tab4_dataGridImpaye.Visibility = System.Windows.Visibility.Visible;
                    //    }
                    //    break;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        private void btn_periode_Click(object sender, RoutedEventArgs e)
        {
            FrmPeriodeEdition ctrperiode = new FrmPeriodeEdition();
            ctrperiode.Closed += ctrperiode_Closed;
            ctrperiode.Show();
        }
        void AfficheCompteClient(string PeriodeDebut, string PeriodeFin)
        {
            switch (this.tab4_cbo_Operation.SelectedIndex)
            {
                case 0:
                    {
                        if (tab4_dataGridAllCompteClient.ItemsSource != null)
                        {
                            List<CsLclient> ListeDobjects = ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(leClasseurClient.ToutLClient.OrderBy(t => t.REFEM).ToList());
                            List<CsLclient> lstEvtImprimer = new List<CsLclient>();
                            if (!string.IsNullOrEmpty(PeriodeDebut) &&
                              string.IsNullOrEmpty(PeriodeFin))
                                lstEvtImprimer = ListeDobjects.Where(t => t.DC == SessionObject.Enumere.Debit && int.Parse(t.REFEM) >= int.Parse(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(PeriodeDebut))).ToList();

                            if (string.IsNullOrEmpty(PeriodeDebut) &&
                                !string.IsNullOrEmpty(PeriodeFin))
                                lstEvtImprimer = ListeDobjects.Where(t => t.DC == SessionObject.Enumere.Debit && int.Parse(t.REFEM) <= int.Parse(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(PeriodeFin))).ToList();

                            if (!string.IsNullOrEmpty(PeriodeDebut) &&
                             !string.IsNullOrEmpty(PeriodeFin))
                                lstEvtImprimer = ListeDobjects.Where(t => t.DC == SessionObject.Enumere.Debit && int.Parse(t.REFEM) >= int.Parse(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(PeriodeDebut))
                                                  && int.Parse(t.REFEM) <= int.Parse(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(PeriodeFin))).ToList();

                            if (string.IsNullOrEmpty(PeriodeDebut) &&
                                string.IsNullOrEmpty(PeriodeFin))
                                lstEvtImprimer = ListeDobjects.ToList();
                            RemplireOngletToutLeCompte(lstEvtImprimer.OrderBy(t => t.REFEM).ToList(), ListeDobjects);
                        }
                    }
                    break;
                case 1:
                    {

                        List<CsLclient> ListeDobjects = ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(leClasseurClient.LstReglement.OrderBy(t => t.REFEM).ToList());
                        List<CsLclient> lstEvtImprimer = new List<CsLclient>();
                        if (!string.IsNullOrEmpty(PeriodeDebut) &&
                          string.IsNullOrEmpty(PeriodeFin))
                            lstEvtImprimer = ListeDobjects.Where(t => int.Parse(t.REFEM) >= int.Parse(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(PeriodeDebut))).ToList();

                        if (string.IsNullOrEmpty(PeriodeDebut) &&
                            !string.IsNullOrEmpty(PeriodeFin))
                            lstEvtImprimer = ListeDobjects.Where(t => int.Parse(t.REFEM) <= int.Parse(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(PeriodeFin))).ToList();

                        if (!string.IsNullOrEmpty(PeriodeDebut) &&
                         !string.IsNullOrEmpty(PeriodeFin))
                            lstEvtImprimer = ListeDobjects.Where(t => int.Parse(t.REFEM) >= int.Parse(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(PeriodeDebut))
                                              && int.Parse(t.REFEM) <= int.Parse(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(PeriodeFin))).ToList();

                        if (string.IsNullOrEmpty(PeriodeDebut) &&
                            string.IsNullOrEmpty(PeriodeFin))
                            lstEvtImprimer = ListeDobjects.ToList();

                        this.tab4_dataGridReglement.ItemsSource = null;
                        this.tab4_dataGridReglement.ItemsSource = lstEvtImprimer;

                    }
                    break;
                case 2:
                    {
                        List<CsLclient> ListeDobjects = ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(leClasseurClient.LstFacture.OrderBy(t => t.REFEM).ToList());
                        List<CsLclient> lstEvtImprimer = new List<CsLclient>();
                        if (!string.IsNullOrEmpty(PeriodeDebut) &&
                          string.IsNullOrEmpty(PeriodeFin))
                            lstEvtImprimer = ListeDobjects.Where(t => int.Parse(t.REFEM) >= int.Parse(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(PeriodeDebut))).ToList();

                        if (string.IsNullOrEmpty(PeriodeDebut) &&
                            !string.IsNullOrEmpty(PeriodeFin))
                            lstEvtImprimer = ListeDobjects.Where(t => int.Parse(t.REFEM) <= int.Parse(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(PeriodeFin))).ToList();

                        if (!string.IsNullOrEmpty(PeriodeDebut) &&
                         !string.IsNullOrEmpty(PeriodeFin))
                            lstEvtImprimer = ListeDobjects.Where(t => int.Parse(t.REFEM) >= int.Parse(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(PeriodeDebut))
                                              && int.Parse(t.REFEM) <= int.Parse(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(PeriodeFin))).ToList();

                        if (string.IsNullOrEmpty(PeriodeDebut) &&
                            string.IsNullOrEmpty(PeriodeFin))
                            lstEvtImprimer = ListeDobjects.ToList();

                        this.tab4_dataGridFacture.ItemsSource = null;
                        this.tab4_dataGridFacture.ItemsSource = lstEvtImprimer;

                    }
                    break;
                default:
                    break;
            }
        }

        private void Txt_Client_TextChanged(object sender, TextChangedEventArgs e)
        {
        }


        private void btn_Regroupement_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (SessionObject.LstCodeRegroupement != null && SessionObject.LstCodeRegroupement.Count != 0)
                {
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstCodeRegroupement);
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "NOM", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedRegroupement);
                    this.IsEnabled = false;
                    ctr.Show();
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        void galatee_OkClickedRegroupement(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.GetisOkClick)
            {
                CsRegCli _LeRegcli = (CsRegCli)ctrs.MyObject;
                this.Txt_LibelleRegroupement.Text = _LeRegcli.NOM;
                this.txt_Regroupement.Text = _LeRegcli.CODE;
                this.txt_Regroupement.Tag = _LeRegcli;

            }
        }

        private void btn_ClearField_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid1.ItemsSource != null && _LstClient .Count != 0)
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("pRegroupement", this.txt_Regroupement.Text);
                Utility.ActionExportation<ServicePrintings.CsClientRechercher, Galatee.Silverlight.ServiceAccueil.CsClientRechercher>(_LstClient, param, string.Empty, SessionObject.CheminImpression, "ClientDuRegroupement", "Accueil", true, "xlsx");
            
            }
        }

    }
}


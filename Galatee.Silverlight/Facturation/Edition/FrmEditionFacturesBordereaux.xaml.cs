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
using Galatee.Silverlight.Resources.Facturation;
using Galatee.Silverlight.ServiceFacturation;
using Galatee.Silverlight.Shared;
using System.Windows.Data;
using System.IO;

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmEditionFacturesBordereaux : ChildWindow
    {
        private List<CsLotri> listeDeLotsInit = new List<CsLotri>();
        private List<CsLotri> listeDeLots = new List<CsLotri>();
        private List<CsLotri> ListeDesTourneeLot = new List<CsLotri>();
        private List<CsLotri> ListeDesPeriodeLot = new List<CsLotri>();
        private List<CsLotri> ListeDesCentreLot = new List<CsLotri>();

        CsLotri lotSelect = new CsLotri();
      
        Galatee.Silverlight.ServiceAccueil.CsRegCli lotRegroupement = new Galatee.Silverlight.ServiceAccueil.CsRegCli();
        Galatee.Silverlight.ServiceAccueil.CsRegCli lotRegroupementFin = new Galatee.Silverlight.ServiceAccueil.CsRegCli();

        List<String> listeDesPeriodes = new List<string> ();
        List<String> listeDesJets = new List<string> ();
        List<String> listeDesFormats = new List<string> ();
        CsLotri LotSelectionne = new CsLotri ();
        string JetSelectionne;
        string periodeSelectionne;
        List<CsEnteteFacture> ClientsTourneeReprise = new List<CsEnteteFacture> ();
        List<CsEnteteFacture> ClientsTourneeStop = new List<CsEnteteFacture> ();

        public FrmEditionFacturesBordereaux()
        {
            InitializeComponent();
            Btn_lot.IsEnabled = false;
            Btn_jet.IsEnabled = false;
            this.Txt_Periode.IsReadOnly = true;
            InitializeTranslate();
            Rdb_facture.IsChecked = true;
            Chk_edition_lot.IsChecked = true;
            ChargerDonneeDuSite(true , string.Empty );
            this.prgBar.Visibility = System.Windows.Visibility.Collapsed;
            Btn_lot.IsEnabled = false;
            Txt_PeriodeLotIsole.Visibility = System.Windows.Visibility.Collapsed;
            Lbl_PeriodeIsole.Visibility = System.Windows.Visibility.Collapsed;
            chk_LotIsoleMisAJour.Visibility = System.Windows.Visibility.Collapsed;
        }
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<ServiceAccueil.CsCentre>();
        private void ChargerDonneeDuSite(bool IsFacturationCourante, string Periode)
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
                    foreach (ServiceAccueil.CsSite item in lstSite)
                        lesDeCentre.Add(item.CODE, LstCentre.Where(i => i.CODESITE == item.CODE).Select(p => p.PK_ID).ToList());
                    ChargerListeLotriEdition(lesDeCentre, IsFacturationCourante, Periode);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
                    foreach (ServiceAccueil.CsSite item in lstSite)
                        lesDeCentre.Add(item.CODE, LstCentre.Where(i => i.CODESITE == item.CODE).Select(p => p.PK_ID).ToList());
                    ChargerListeLotriEdition(lesDeCentre, IsFacturationCourante, Periode);
                    return;
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerListeLotriEdition(Dictionary<string, List<int>> lstSiteCentre, bool IsFacturaioncourante,string Periode)
        {
            listeDeLots = new List<CsLotri>();
            try
            {
        
                ListeDesTourneeLot = new List<CsLotri>();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ChargerLotriPourEditionAsync(lstSiteCentre, UserConnecte.matricule, IsFacturaioncourante, Periode);
                service.ChargerLotriPourEditionCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Error != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur d'invocation du service.", "Erreur");
                            return;
                        }
                        if (args.Result == null)
                        {
                            Message.ShowError("Aucune donnée retournée du système.", "Erreur");
                            return;
                        }
                        listeDeLots = args.Result;
                        listeDeLotsInit = Shared.ClasseMEthodeGenerique.RetourneListCopy<CsLotri>(listeDeLots);
                        this.Txt_batch.Text = string.Empty;
                        Btn_lot.IsEnabled = true;
                        this.Txt_batch.IsReadOnly = false;

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 
        private void InitializeTranslate()
        {
            //this.Lbl_type_edition.Text = Langue.Print_type;
            //this.Lbl_caract_edition.Text = Langue.Print_spec;
            this.Chk_client_lot.Content = Langue.Print_client_group;
            this.Chk_domicilie_bank.Content = Langue.Print_bank;
            this.Chk_edition_lot.Content = Langue.Print_lot;
            this.Btn_lot.Content = Langue.Batch;
            this.Btn_jet.Content = Langue.Run;

            //this.Lbl_print_format.Text = Langue.Print_format;
            this.Rdb_facture.Content = Langue.Invoice;
            this.Rdb_bordereau_detail.Content = Langue.Details_list;
            this.Rdb_bordereau_simple.Content = Langue.List_simplified;
            this.Rdb_edtion_anomalie.Content = Langue.Printings_error;

            this.Chk_edition_tournee.Content = Langue.Chk_print_zone;
            this.Lbl_zone.Content = Langue.Lbl_zone;
            this.Lbl_center1.Content = Langue.Lbl_center1;

            //this.Lbl_reprise_edition.Text = Langue.Lbl_reprise_edition;
            this.Lbl_centre.Content = Langue.Lbl_centre;
            this.Chk_reprise_client.Content = Langue.Chk_reprise_client;
            
            //this.Lbl_impression.Text = Langue.Lbl_impression;
            //this.Btn_enregistrer.Content = Langue.Btn_enregistrer;
            //this.Btn_supprimer.Content = Langue.Btn_supprimer;
            this.Btn_lancer.Content = Langue.Btn_lancer;
            this.Btn_cancel.Content = Langue.Btn_cancel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Chk_edition_lot_Click(object sender, RoutedEventArgs e)
        {
            bool? ischecked =  Chk_edition_lot.IsChecked ;
        }        
        
        private void Btn_lot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<CsLotri> LstLot = new List<CsLotri>();
                List<string> _LstColonneAffich = new List<string>();
    
                if (chk_LotIsole.IsChecked == true)
                {
                    _LstColonneAffich.Add("NUMLOTRI");
                    _LstColonneAffich.Add("PRODUIT");
                    LstLot = Facturation.ClasseMethodeGenerique.DistinctLotriPeriodeProduit(listeDeLots.Where(y => y.ETATFAC10 == "O").ToList());
                }
                else
                {
                    _LstColonneAffich.Add("NUMLOTRI");
                    _LstColonneAffich.Add("PERIODE");
                    _LstColonneAffich.Add("PRODUIT");
                    LstLot = Facturation.ClasseMethodeGenerique.DistinctLotriPeriodeProduit(listeDeLots.Where(y => y.ETATFAC10 != "O").ToList());
                }
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstLot);
                MainView.UcListeGenerique ucg = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                ucg.Closed += new EventHandler(ucgReturn);
                ucg.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ucgReturn(object sender, EventArgs e)
        {
            try
            {
                Btn_jet.IsEnabled = true ;
                MainView.UcListeGenerique uc = sender as MainView.UcListeGenerique;
                if (uc.GetisOkClick)
                {
                    CsLotri lot = (CsLotri)uc.MyObject;
                    this.Txt_batch.Text  = lot.NUMLOTRI;
                    this.Txt_batch.Tag = lot;
                    lotSelect = lot;

                    List<CsLotri> LstLot = Facturation.ClasseMethodeGenerique.DistinctLotriJetProduit(listeDeLots.Where(p => p.NUMLOTRI == lotSelect.NUMLOTRI && p.PRODUIT == lotSelect.PRODUIT).ToList());
                    if (LstLot != null && LstLot.Count == 1)
                    {
                        lotSelect.JET = LstLot.First().JET;
                        this.Txt_run.Text = lotSelect.JET;
                        Btn_jet.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void ucgLot(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique uc = sender as MainView.UcListeGenerique;
                if (uc.GetisOkClick)
                {
                   CsLotri leJet = (CsLotri)uc.MyObject;
                    this.Txt_run.Text = leJet.JET;
                    lotSelect.JET = leJet.JET;
                    if (ClasseMethodeGenerique.IsLotIsole(lotSelect.NUMLOTRI))
                        lotSelect.USERCREATION = listeDeLots.FirstOrDefault(t => t.NUMLOTRI == lotSelect.NUMLOTRI && t.JET == lotSelect.JET).USERCREATION;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void Btn_jet_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("JET");
                List<CsLotri> LstLot = Facturation.ClasseMethodeGenerique.DistinctJetProduit(listeDeLots.Where(p => p.NUMLOTRI == ((CsLotri)this.Txt_batch.Tag).NUMLOTRI && p.PRODUIT == ((CsLotri)this.Txt_batch.Tag).PRODUIT ).ToList()); 
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstLot);
                MainView.UcListeGenerique ucg = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Jet");
                ucg.Closed += new EventHandler(ucgLot);
                ucg.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
 

        /// <summary>
        /// Lance l'impression des factures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /*
        private void Btn_lancer_Click(object sender, RoutedEventArgs e)
        {   
            String rdlcName = String.Empty;
            //String periodeSelectionne = String.Empty;
            
            JetSelectionne = Txt_run.Text;

            //String formatSelectionne;
            String clientTournee = string.Empty;
            String clientTourneeReprise = string.Empty;

            String centreTournee = string.Empty;
            String debutTournee = string.Empty;
            String finTournee = string.Empty;

            String RegroupementDebut = string.Empty;
            String RegroupementStop = string.Empty;
            bool IsoleDejaMiseAjour = false;

             
            CsClient leClient = new CsClient();
            leClient.CENTRE = Txt_centre_reprise.Text;
            leClient.REFCLIENT = Txt_reprise_client.Text;
            leClient.ORDRE = Txt_Ordre_client.Text;
            lotSelect.PERIODE  = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_PeriodeClient.Text);
            if (this.Txt_centre_reprise.Tag != null )
            {
              
              List<ServiceAccueil.CsProduit> lstProduit =((ServiceAccueil.CsCentre)this.Txt_centre_reprise.Tag).LESPRODUITSDUSITE;
              if (lstProduit != null && lstProduit.Count != 0)
              {
                  if (lstProduit.FirstOrDefault(o=>o.CODE == SessionObject.Enumere.ElectriciteMT )!= null )
                  lotSelect.PRODUIT = SessionObject.Enumere.ElectriciteMT;
              }
            }
            CsTournee laTournee = new CsTournee();
            laTournee.CENTRE  = Txt_centre_tournee.Text;
             laTournee.TOURNEDEBUT  = Txt_tournee_debut.Text;
            laTournee.TOURNEFIN   = Txt_tournee_fin.Text;

            if (Chk_edition_tournee.IsChecked == true)
            {
                if (!string.IsNullOrEmpty(laTournee.CENTRE) && !string.IsNullOrEmpty(laTournee.TOURNEDEBUT))
                {
                    if (!string.IsNullOrEmpty(laTournee.TOURNEFIN))
                        if (laTournee.TOURNEDEBUT.CompareTo(laTournee.TOURNEFIN) > 0)
                        {
                            Message.Show("Le debut de tournée doit etre inferieur à la fin", "Erreur de saisie");
                            return;
                        }
                }
                else
                {
                    Message.Show("Vous devez renseigner le centre et le debut de la tournée", "Erreur de saisie");
                    return;
                }
            }
           

            // Edition des lots selectionné
            if (Chk_edition_lot.IsChecked == true && Chk_reprise_client.IsChecked != true )
            {
                
                if ((string.IsNullOrEmpty(this.Txt_batch.Text)) || string.IsNullOrEmpty(JetSelectionne))
                {
                    Message.Show("Choisissez un lot et un jet", "Erreur de saisie");
                    return;
                }
            }

            // Fichier rdlc
            Dictionary<string, string> param = new Dictionary<string, string>();

            if (Rdb_facture.IsChecked == true)
            {
                param.Add("TypeEdition", "Original");
                if (lotSelect.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    rdlcName = "FactureSimple";
                else
                    rdlcName = "FactureSimpleMT";
            }
            if (Rdb_bordereau_detail.IsChecked == true)
            {
                if (lotSelect.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    rdlcName = "FactureDetail";
                else
                    rdlcName = "FactureDetailMT";
            }
            if (Rdb_bordereau_simple.IsChecked == true)
            {
                rdlcName = "BordereauSimple";
                //CsLotri  lelotriSelect = ListeDesLotriAfficher.FirstOrDefault(t => t.NUMLOTRI == lotSelect.NUMLOTRI);
                //ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID  == lelotriSelect.FK_IDCENTRE);
                //if (leCentre.CODESITE == SessionObject.CodeSiteScaBT || 
                //    leCentre.CODESITE == SessionObject.CodeSiteScaMT)
                //    rdlcName = "BordereauSimpleSGC";
            }
            if (Rdb_edtion_anomalie.IsChecked == true)
                rdlcName = "AnomalieFacturation";

                    FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(),Utility.EndPoint("Facturation"));
                    this.prgBar.Visibility = System.Windows.Visibility.Visible;
                    if (rdlcName == "AnomalieFacturation")
                    {

                        service.RetourneAnnomalieFacturesAsync(lotSelect, centreTournee,
                        debutTournee, finTournee, string.Empty, string.Empty, string.Empty, string.Empty,
                        periodeSelectionne, rdlcName);
                        service.RetourneAnnomalieFacturesCompleted += (er, res) =>
                        {
                            try
                            {
                                if (res.Error != null || res.Cancelled)
                                    Message.Show("Erreur dans le traitement des factures : " + res.Error.InnerException.ToString(), "Erreur");

                                else
                                    if (res.Result != null)
                                    {
                                        if (res.Result.Count != 0)
                                        {
                                            //Utility.ActionDirectOrientation<ServicePrintings.CsAnnomalie, ServiceFacturation.CsAnnomalie>(res.Result, param, SessionObject.CheminImpression, rdlcName, "Facturation", true);
                                            Utility.ActionExportation<ServicePrintings.CsAnnomalie, ServiceFacturation.CsAnnomalie>(res.Result, param, string.Empty, SessionObject.CheminImpression, rdlcName, "Facturation", true, "doc");
                                            this.prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                        }
                                        else
                                            Message.Show("Aucune élément trouvé ", "Erreur");
                                    }
                                    else
                                        Message.Show("Une erreur s'est produite lors de la génération des factures, veuillez consultez le journal des erreurs",
                                            "Erreur");


                                return;

                            }
                            catch (Exception ex)
                            {
                                this.prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                Message.Show("Erreur inconnue :" + ex.Message, "Erreur inconnue");
                            }
                        };
                        return;
                    }
                    if (lotSelect != null && !string.IsNullOrEmpty(lotSelect.NUMLOTRI))
                    {
                        if (this.chk_LotIsole.IsChecked == true)
                        {
                            lotSelect.PERIODE = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_PeriodeLotIsole.Text);
                            IsoleDejaMiseAjour = (this.chk_LotIsoleMisAJour.IsChecked.Value);
                        }
                    }

                    service.RetourneFacturesAsync(lotSelect,laTournee,leClient, IsoleDejaMiseAjour,rdlcName);
                    service.RetourneFacturesCompleted += (er, res) =>
                    {
                        try
                        {
                            this.prgBar.Visibility = System.Windows.Visibility.Collapsed;

                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des factures : " + res.Error.InnerException.ToString(), "Erreur");

                            else
                                if (res.Result != null)
                                {
                                    if (res.Result.Count != 0)
                                    {
                                        List<CsFactureClient> lstClient = new List<CsFactureClient>();
                                        var lesClient = res.Result.Select(y => new { y.Centre, y.Client, y.Ordre }).Distinct();
                                        foreach (var item in lesClient)
                                            lstClient.Add(new CsFactureClient() { Centre = item.Centre, Client = item.Client, Ordre = item.Ordre });

                                        int Passage = 0;
                                        string[] tableau = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V" };
                                        while (lstClient.Where(o => o.ISFACTURE != true).Count() != 0)
                                        {
                                            string NomFichier = rdlcName + tableau[Passage];
                                            List<string> clientSelectionne = lstClient.Where(m => m.ISFACTURE != true).Take(100).Select(o => o.Client).ToList();
                                            List<ServiceFacturation.CsFactureClient> factureAEditer = res.Result.Where(p => clientSelectionne.Contains(p.Client)).ToList();
                                            factureAEditer.ForEach(y => y.fk_idClient = clientSelectionne.Count.ToString());
                                            Utility.ActionExportation<ServicePrintings.CsFactureClient, ServiceFacturation.CsFactureClient>(factureAEditer, param, NomFichier, SessionObject.CheminImpression, rdlcName, "Facturation", true, "pdf");
                                            lstClient.Where(p => clientSelectionne.Contains(p.Client)).ToList().ForEach(p => p.ISFACTURE = true);
                                            Passage++;
                                        }
                                        //Utility.ActionPreview<ServicePrintings.CsFactureClient, ServiceFacturation.CsFactureClient>(res.Result, param, rdlcName, "Facturation");

                                        if (rdlcName =="FactureSimple" || rdlcName =="FactureSimpleMT")
                                        Message.ShowInformation(Galatee.Silverlight.Resources.Index.Langue.MsgEditionTerminer, Galatee.Silverlight.Resources.Index.Langue.libelleModule);
                                    }
                                    else
                                        Message.Show("Aucune donnée trouvée", "Edition");
                                }
                                else
                                    Message.Show("Une erreur s'est produite lors de la génération des factures, veuillez consultez le journal des erreurs", "Erreur");
                        }
                        catch (Exception ex)
                        {
                            this.prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            Message.Show("Erreur inconnue :" + ex.Message, "Erreur inconnue");
                     
                        }
                    };
                


        }
        */




        private void Btn_lancer_Click(object sender, RoutedEventArgs e)
        {
            String rdlcName = String.Empty;
            //String periodeSelectionne = String.Empty;

            JetSelectionne = Txt_run.Text;

            //String formatSelectionne;
            String clientTournee = string.Empty;
            String clientTourneeReprise = string.Empty;

            String centreTournee = string.Empty;
            String debutTournee = string.Empty;
            String finTournee = string.Empty;

            String RegroupementDebut = string.Empty;
            String RegroupementStop = string.Empty;
            bool IsoleDejaMiseAjour = false;


            CsClient leClient = new CsClient();
            leClient.CENTRE = Txt_centre_reprise.Text;
            leClient.REFCLIENT = Txt_reprise_client.Text;
            leClient.ORDRE = Txt_Ordre_client.Text;
            lotSelect.PERIODE = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_PeriodeClient.Text);
            if (this.Txt_centre_reprise.Tag != null)
            {

                List<ServiceAccueil.CsProduit> lstProduit = ((ServiceAccueil.CsCentre)this.Txt_centre_reprise.Tag).LESPRODUITSDUSITE;
                if (lstProduit != null && lstProduit.Count != 0)
                {
                    if (lstProduit.FirstOrDefault(o => o.CODE == SessionObject.Enumere.ElectriciteMT) != null)
                        lotSelect.PRODUIT = SessionObject.Enumere.ElectriciteMT;
                }
            }
            CsTournee laTournee = new CsTournee();
            laTournee.CENTRE = Txt_centre_tournee.Text;
            laTournee.TOURNEDEBUT = Txt_tournee_debut.Text;
            laTournee.TOURNEFIN = Txt_tournee_fin.Text;

            if (Chk_edition_tournee.IsChecked == true)
            {
                if (!string.IsNullOrEmpty(centreTournee) && !string.IsNullOrEmpty(debutTournee))
                {
                    if (!string.IsNullOrEmpty(finTournee))
                        if (debutTournee.CompareTo(finTournee) > 0)
                        {
                            Message.Show("La tournée de début doit être inférieure à la tournée de fin", "Erreur de saisie");
                            return;
                        }
                }
                else
                {
                    Message.Show("Vous devez renseigner le centre et la tournée de début", "Erreur de saisie");
                    return;
                }
            }


            // Edition des lots selectionné
            if (Chk_edition_lot.IsChecked == true && Chk_reprise_client.IsChecked != true)
            {

                if ((string.IsNullOrEmpty(this.Txt_batch.Text)) || string.IsNullOrEmpty(JetSelectionne))
                {
                    Message.Show("Choisissez un lot et un jet", "Erreur de saisie");
                    return;
                }
            }

            // Fichier rdlc
            Dictionary<string, string> param = new Dictionary<string, string>();

            if (Rdb_facture.IsChecked == true)
            {
                param.Add("TypeEdition", "Original");
                if (lotSelect.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    rdlcName = "FactureSimpleMT";
                else if (lotSelect.PRODUIT == SessionObject.Enumere.Eau)
                    rdlcName = "FactureSimpleO";
                else
                    rdlcName = "FactureSimple";
            }
            if (Rdb_bordereau_detail.IsChecked == true)
            {
                if (lotSelect.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    rdlcName = "FactureDetail";
                else
                    rdlcName = "FactureDetailMT";
            }
            if (Rdb_bordereau_simple.IsChecked == true)
            {
                rdlcName = "BordereauSimple";
                //CsLotri  lelotriSelect = ListeDesLotriAfficher.FirstOrDefault(t => t.NUMLOTRI == lotSelect.NUMLOTRI);
                //ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID  == lelotriSelect.FK_IDCENTRE);
                //if (leCentre.CODESITE == SessionObject.CodeSiteScaBT || 
                //    leCentre.CODESITE == SessionObject.CodeSiteScaMT)
                //    rdlcName = "BordereauSimpleSGC";
            }
            if (Rdb_edtion_anomalie.IsChecked == true)
                rdlcName = "AnomalieFacturation";

            FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
            this.prgBar.Visibility = System.Windows.Visibility.Visible;
            if (rdlcName == "AnomalieFacturation")
            {

                service.RetourneAnnomalieFacturesAsync(lotSelect, centreTournee,
                debutTournee, finTournee, string.Empty, string.Empty, string.Empty, string.Empty,
                periodeSelectionne, rdlcName);
                service.RetourneAnnomalieFacturesCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement des factures : " + res.Error.InnerException.ToString(), "Erreur");

                        else
                            if (res.Result != null)
                            {
                                if (res.Result.Count != 0)
                                {
                                    //Utility.ActionDirectOrientation<ServicePrintings.CsAnnomalie, ServiceFacturation.CsAnnomalie>(res.Result, param, SessionObject.CheminImpression, rdlcName, "Facturation", true);
                                    Utility.ActionExportation<ServicePrintings.CsAnnomalie, ServiceFacturation.CsAnnomalie>(res.Result, param, string.Empty, SessionObject.CheminImpression, rdlcName, "Facturation", true, "doc");
                                    this.prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                }
                                else
                                    Message.Show("Aucune élément trouvé ", "Erreur");
                            }
                            else
                                Message.Show("Une erreur s'est produite lors de la génération des factures, veuillez consultez le journal des erreurs",
                                    "Erreur");


                        return;

                    }
                    catch (Exception ex)
                    {
                        this.prgBar.Visibility = System.Windows.Visibility.Collapsed;
                        Message.Show("Erreur inconnue :" + ex.Message, "Erreur inconnue");
                    }
                };
                return;
            }
            if (lotSelect != null && !string.IsNullOrEmpty(lotSelect.NUMLOTRI))
            {
                if (this.chk_LotIsole.IsChecked == true)
                {
                    lotSelect.PERIODE = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_PeriodeLotIsole.Text);
                    IsoleDejaMiseAjour = (this.chk_LotIsoleMisAJour.IsChecked.Value);
                }
            }
            else /** ZEG 29/08/2017 **/
            {
                Message.Show("Veuillez refaire le choix du lot", "Erreur de saisie");
                this.prgBar.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }



            Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Printing"));
            var uri = new Uri(App.Current.Host.Source.AbsoluteUri);


            string print = "Imprimé le " + DateTime.Now + " sur le poste " + SessionObject.LePosteCourant.NOMPOSTE + " par " + UserConnecte.nomUtilisateur + "(" + UserConnecte.matricule + ") du centre " + UserConnecte.LibelleCentre;
            param.Add("Print", print);

            service.EditerFacturesAsync(lotSelect, laTournee, leClient, IsoleDejaMiseAjour, rdlcName, param, SessionObject.CheminImpression, UserConnecte.matricule, SessionObject.ServerEndPointName, SessionObject.ServerEndPointPort, uri.Port.ToString());
            service.EditerFacturesCompleted += (er, res) =>
            {
                try
                {
                    this.prgBar.Visibility = System.Windows.Visibility.Collapsed;

                    if (res.Error != null || res.Cancelled)
                        Message.Show("Erreur dans le traitement des factures : " + res.Error.InnerException.ToString(), "Erreur");

                    else
                        if (res.Result == true)
                        {
                            Message.ShowInformation("Les fichiers seront deposés dans le repertoire\n C:\\IMPRESSION", "");
                        }
                        else
                            Message.Show("Une erreur s'est produite lors de la génération des factures, veuillez consultez le journal des erreurs", "Erreur");
                }
                catch (Exception ex)
                {
                    this.prgBar.Visibility = System.Windows.Visibility.Collapsed;
                    Message.Show("Erreur inconnue :" + ex.Message, "Erreur inconnue");
                }
            };
        }













        private bool TestExistanceBorneReprise(string clientReprise, string centreReprise, string LotSelectionne, string periodeSelectionne,bool Isreprise)
        {
            //return true;

            FacturationServiceClient service = new FacturationServiceClient();
            service.RetourneClientDuneBorneAsync(centreReprise, clientReprise, LotSelectionne, periodeSelectionne);
            service.RetourneClientDuneBorneCompleted += (er, res) =>
            {
                try
                {
                    if (res.Error != null || res.Cancelled)
                        return;

                    if (res.Result != null)
                        if (Isreprise)
                            ClientsTourneeReprise = new List<CsEnteteFacture>(res.Result);
                        else
                            ClientsTourneeStop = new List<CsEnteteFacture>(res.Result);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }   
            };
            return true;

        }

        private void Chk_edition_tournee_Click(object sender, RoutedEventArgs e)
        {
            IList<Control> controlesADesactiver = new List<Control>();
            controlesADesactiver.Add(Txt_centre_tournee);
            controlesADesactiver.Add(Txt_tournee_debut);
            controlesADesactiver.Add(Txt_tournee_fin);

            if (Chk_edition_tournee.IsChecked == true)
            {              
                SetControlEnabling(controlesADesactiver, true);
                foreach (Control control in controlesADesactiver)
                    control.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));
            }
            else
            {
                SetControlEnabling(controlesADesactiver, false);
                foreach (Control control in controlesADesactiver)
                    control.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));
            }
        }

        /// <summary>
        /// Active ou desactive une liste de controles
        /// </summary>
        /// <param name="controls">Une Collection de controles</param>
        /// <param name="isEnabled">la valeur a attribuer à la proprieté isEnabled</param>
        private void SetControlEnabling(ICollection<Control> controls, bool isEnabled)
        {
            foreach (Control control in controls)
            {
                control.IsEnabled = isEnabled;
                if (control.GetType() == typeof(TextBox))
                    (control as TextBox).Text = string.Empty;
            }
        }

        private void Rdb_edtion_anomalie_Click(object sender, RoutedEventArgs e)
        {
            if (Rdb_edtion_anomalie.IsChecked == true)
            {
                IList<Control> controlesADesactiver = new List<Control>();
                controlesADesactiver.Add(Chk_reprise_client);
                controlesADesactiver.Add(Chk_edition_tournee);
  

                SetControlEnabling(controlesADesactiver, false);
            }
        }

        private void Chk_reprise_client_Click(object sender, RoutedEventArgs e)
        {
            IList<Control> controlesADesactiver = new List<Control>();
            controlesADesactiver.Add(Txt_centre_reprise);
            controlesADesactiver.Add(Txt_reprise_client);

            if (Chk_reprise_client.IsChecked == true)
            {
                SetControlEnabling(controlesADesactiver, true);
                Txt_centre_reprise.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));
                Txt_reprise_client.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));
            }
            else
            {
                SetControlEnabling(controlesADesactiver, false);
                Txt_centre_reprise.Background = new SolidColorBrush(Colors.White);
                Txt_reprise_client.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void Chk_stop_client_Click(object sender, RoutedEventArgs e)
        {
            IList<Control> controlesADesactiver = new List<Control>();
            controlesADesactiver.Add(Txt_Ordre_client);
     

           
        }
        private void Chk_edition_lot_Checked(object sender, RoutedEventArgs e)
        {
            IList<Control> controlesADesactiver = new List<Control>();
            SetControlEnabling(controlesADesactiver, false);

            IList<Control> controlesAActiver = new List<Control>();
            controlesAActiver.Add(Btn_lot);
      
            controlesAActiver.Add(Rdb_edtion_anomalie);
            SetControlEnabling(controlesAActiver, true);

         

            Txt_batch.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));
            Txt_run.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));
        }

        private void Chk_client_lot_Checked(object sender, RoutedEventArgs e)
        {
        

            //IList<Control> controls = new List<Control>() { Txt_from, Txt_until, Btn_periode };
            //SetControlEnabling(controls, true  );

            // Remise a default des autres textbox
            Txt_batch.Background = new SolidColorBrush(Colors.White);
            Txt_run.Background = new SolidColorBrush(Colors.White);

        }

        private void Chk_domicilie_bank_Checked(object sender, RoutedEventArgs e)
        {
            // Remise a default des autres textbox
            Txt_batch.Background = new SolidColorBrush(Colors.White);
            Txt_run.Background = new SolidColorBrush(Colors.White);

            //Txt_period.Background = new SolidColorBrush(Colors.tr);
        }

        private void Chk_reprise_client_Checked_1(object sender, RoutedEventArgs e)
        {
            Txt_centre_reprise.IsReadOnly  = false;
            Txt_reprise_client.IsReadOnly = false;
            Txt_Ordre_client.IsReadOnly = false;
            btn_CentreReprise.IsEnabled = true; 
          
        }

        private void btn_CentreReprise_Click(object sender, RoutedEventArgs e)
        {
          try
            {
                List<CsLotri> LstLot = new List<CsLotri>();
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CENTRE");
                //List<CsLotri> LstLot = Facturation.ClasseMethodeGenerique.RetourneDistinctCentre(listeDeLots.Where(p => p.NUMLOTRI == this.Txt_batch.Text && p.CENTRE == ((CsLotri)this.Txt_batch.Tag).CENTRE).ToList());
                if (!string.IsNullOrEmpty(this.Txt_batch.Text))
                    LstLot = Facturation.ClasseMethodeGenerique.RetourneDistinctCentre(listeDeLots.Where(p => p.NUMLOTRI == this.Txt_batch.Text).ToList());
                else
                    LstLot = Facturation.ClasseMethodeGenerique.RetourneDistinctCentreFromCentre(LstCentre);

                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstLot);
                MainView.UcListeGenerique ucg = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                ucg.Closed += new EventHandler(ucgReturnCentre);
                ucg.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ucgReturnCentre(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique uc = sender as MainView.UcListeGenerique;
                if (uc.GetisOkClick)
                {
                    CsLotri Centre = (CsLotri)uc.MyObject;
                    this.Txt_centre_reprise.Text = Centre.CENTRE;
                    ServiceAccueil.CsCentre leCentreSelect = LstCentre.FirstOrDefault(t => t.CODE == Centre.CENTRE);
                    if (leCentreSelect != null )
                        this.Txt_centre_reprise.Tag = leCentreSelect;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void Btn_CentreTourne_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CENTRE");
                List<CsLotri> LstLot = Facturation.ClasseMethodeGenerique.RetourneDistinctCentre(listeDeLots.Where(p => p.NUMLOTRI == this.Txt_batch.Text).ToList());
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstLot);
                MainView.UcListeGenerique ucg = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                ucg.Closed += new EventHandler(ucgReturnCentreTournee);
                ucg.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ucgReturnCentreTournee(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique uc = sender as MainView.UcListeGenerique;
                if (uc.GetisOkClick)
                {
                    CsLotri Centre = (CsLotri)uc.MyObject;
                    this.Txt_centre_tournee.Text = Centre.CENTRE;
                    this.Txt_centre_tournee.Tag = Centre;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

  
        private void Btn_Tournee1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("TOURNEE");
                List<CsLotri> LstLot = Facturation.ClasseMethodeGenerique.RetourneDistinctTournee (listeDeLots.Where(p => p.NUMLOTRI == this.Txt_batch.Text).ToList());
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstLot);
                MainView.UcListeGenerique ucg = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                ucg.Closed += new EventHandler(ucgReturnTourneeCentre);
                ucg.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ucgReturnTourneeCentre(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique uc = sender as MainView.UcListeGenerique;
                if (uc.GetisOkClick)
                {
                    CsLotri Tournee = (CsLotri)uc.MyObject;
                    this.Txt_tournee_debut.Text = Tournee.TOURNEE;
                    this.Txt_tournee_debut.Tag = Tournee;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void Btn_Tournee2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("TOURNEE");

                List<CsLotri> LstLot = Facturation.ClasseMethodeGenerique.RetourneDistinctTournee(listeDeLots.Where(p => p.NUMLOTRI == this.Txt_batch.Text).ToList());
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstLot);
                MainView.UcListeGenerique ucg = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                ucg.Closed += new EventHandler(ucgReturnTourneeCentre2);
                ucg.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ucgReturnTourneeCentre2(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique uc = sender as MainView.UcListeGenerique;
                if (uc.GetisOkClick)
                {
                    CsLotri Tournee = (CsLotri)uc.MyObject;
                    this.Txt_tournee_fin.Text = Tournee.TOURNEE;
                    this.Txt_tournee_fin.Tag = Tournee;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void Chk_edition_tournee_Checked(object sender, RoutedEventArgs e)
        {
            Txt_centre_tournee.IsReadOnly = false;
            Txt_tournee_debut.IsReadOnly = false;
            Txt_tournee_fin.IsReadOnly = false;
            Btn_CentreTourne.IsEnabled = true;
            Btn_Tournee1.IsEnabled = true;
            Btn_Tournee2.IsEnabled = true;
        }

        private void btnCentre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<object> _LstObj = new List<object>();
                _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(lstSite );

                Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                _LstColonneAffich.Add("CENTRE", "CENTRE");
                _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, true, "Centre");
                //ctrl.Closed += new EventHandler(btnTournee_OkClicked);
                ctrl.Show();

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }

        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(lstSite.OrderBy(p => p.CODE).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += ctr_Closed;
                this.IsEnabled = false;
                ctr.Show();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        void ctr_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.GetisOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsSite _LeSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_Site.Text = _LeSite.LIBELLE;
                this.Txt_Site.Tag  = _LeSite ;
            }
        }

        private void btn_Centre_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.Txt_Site.Tag != null)
            {
                List<ServiceAccueil.CsCentre> lstCentreSite = lesCentre.Where(t => t.FK_IDCODESITE == ((ServiceAccueil.CsSite)this.Txt_Site.Tag).PK_ID).ToList();
                List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsCentre>(lstCentreSite);
                Shared.UcListeParametre ctr = new UcListeParametre(lstParametre, false, "Centre");
                ctr.Show();
            }
        }

        private void Txt_batch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_batch.Text.Length == SessionObject.Enumere.TailleNumeroBatch)
                {
                    List<CsLotri> LstLot = Facturation.ClasseMethodeGenerique.DistinctLotriPeriodeProduit(listeDeLots);
                    CsLotri lot = LstLot.FirstOrDefault(p => p.NUMLOTRI == this.Txt_batch.Text);
                    if (lot != null)
                    {
                        this.Txt_batch.Tag = lot;
                        lotSelect = lot;
                        List<CsLotri> LstLotJet = Facturation.ClasseMethodeGenerique.DistinctLotriJetProduit(listeDeLots.Where(p => p.NUMLOTRI == lotSelect.NUMLOTRI && p.PRODUIT == lotSelect.PRODUIT).ToList());
                        if (LstLotJet != null && LstLotJet.Count == 1)
                        {
                            lotSelect.JET = LstLotJet.First().JET;
                            this.Txt_run.Text = lotSelect.JET;
                            Btn_jet.IsEnabled = false;
                        }
                    }
                    else
                        Message.ShowInformation("le lot ", "Facturation");
                }
            }
            catch (Exception)
            {
                Message.ShowInformation("Une erreur est survenu ", "Facturation");
            }
        }

        private void chk_Autre_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void chk_Autre_Checked(object sender, RoutedEventArgs e)
        {
            this.Txt_Periode.IsReadOnly = false;
            this.Btn_Recherche.IsEnabled = true;
            chk_LotIsole.IsChecked = false;
        }

        private void chk_Autre_Unchecked_1(object sender, RoutedEventArgs e)
        {
            this.Txt_Periode.IsReadOnly = true ;
            this.Txt_Periode.Text = string.Empty;
            this.Btn_Recherche.IsEnabled = false ;
        }

        private void chk_Autre_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Recherche_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Periode.Text))
            {
                if (Shared.ClasseMEthodeGenerique.IsFormatPeriodeValide(this.Txt_Periode.Text))
                {
                    string Periode = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_Periode.Text);
                    ChargerDonneeDuSite(false, Periode);
                }
                else
                    Message.ShowInformation("Le format de la periode n'est pas valide", "Facturation");
            }
            else
                Message.ShowInformation("Saisir la période", "Facturation");
        }

        private void chk_LotIsole_Checked(object sender, RoutedEventArgs e)
        {
            Txt_PeriodeLotIsole.Visibility = System.Windows.Visibility.Visible ;
            Lbl_PeriodeIsole.Visibility = System.Windows.Visibility.Visible;
            chk_LotIsoleMisAJour.Visibility = System.Windows.Visibility.Visible;

            this.chk_Autre.IsChecked = false;
        }
        private void chk_LotIsole_Unchecked(object sender, RoutedEventArgs e)
        {
            Txt_PeriodeLotIsole.Visibility = System.Windows.Visibility.Collapsed ;
            Lbl_PeriodeIsole.Visibility = System.Windows.Visibility.Collapsed;
            chk_LotIsoleMisAJour.Visibility = System.Windows.Visibility.Collapsed;
            Txt_PeriodeLotIsole.Text  = string.Empty ;
        }

    }
}
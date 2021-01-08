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
using Galatee.Silverlight.ServiceAccueil;

namespace Galatee.Silverlight.Accueil
{
    public partial class UcAffectationScelle : ChildWindow
    {
        List<Galatee.Silverlight.ServiceAccueil.CsUtilisateur> lstAllUser = new List<Galatee.Silverlight.ServiceAccueil.CsUtilisateur>();
        List<CsActivite> lstAllActivite = new List<CsActivite>();
        List<CsLotScelle> ListLotAffecter_Selectionner = new List<CsLotScelle>();
        //Dictionary<string, CsLotScelle> ListLotStat_Selectionner = new Dictionary<string, CsLotScelle>();
        Dictionary<CsLotScelle, string> ListLotStat_Selectionner = new Dictionary<CsLotScelle, string>();
        List<CsDscelle> lademande = new List<CsDscelle>();
        int Nbr_ScelleDemandeRestant = 0;
        int Nbr_ScelleDemandeRestant_OverFlow = 0;
        int EtapeActuelle;
        int total_selected = 0;
        List<CsLotScelle> lstLotBrut = new List<CsLotScelle>();
        List<CsLotScelle> lesLotsChoisis = new List<CsLotScelle>();

        public UcAffectationScelle(int pk_id)
        {
            InitializeComponent();

            ChargerService();
            ChargerCentre();
            ChargeListeUser();
            ChargeDonneDemande(pk_id);
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            //ChargeLot();
        }
        public UcAffectationScelle(List<int> demande, int etape)
        {
            //fkiddemande = demande;
            this.EtapeActuelle = etape;
            InitializeComponent();

            ChargerService();
            ChargerCentre();
            ChargeListeUser();
            ChargeDonneDemande(demande.First());
            prgBar.Visibility = System.Windows.Visibility.Collapsed;

            //ChargeLot();
        }
        private void ChargerCentre()
        {
            List<ServiceAccueil.CsCentre> LstCentre = new List<ServiceAccueil.CsCentre>();
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstCentre = args.Result;
                        SessionObject.LstCentre = LstCentre;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }

                };
                service.ListeDesDonneesDesSiteAsync(false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void ChargeListeUser()
        {
            try
            {

                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeAllUserCompleted += (sr, res) =>
                {

                    if (res != null && res.Cancelled)
                        return;
                    lstAllUser = res.Result;

                    
                };
                service1.RetourneListeAllUserAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        private void ChargerService()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneListeActiviteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        lstAllActivite = args.Result;

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }

                };
                service.RetourneListeActiviteAsync(false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }
        private void ChargeLot()
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneListeScelleCompleted += (s, args) =>
            {
                try
                {
                    if (args != null && args.Cancelled)
                        return;
                    DgLotMag.ItemsSource = args.Result;
                    this.lstLotBrut = args.Result;
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }

            };
            service.RetourneListeScelleAsync();
        }

        private void ChargeLot(int IdCentreRecuperationDeLot,CsDscelle laDScelle)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneListeScelleByCentreCompleted += (s, args) =>
            {
                try
                {
                    if (args != null && args.Cancelled)
                        return;
                    DgLotMag.ItemsSource = args.Result.Where(t=>t.Couleur_ID == laDScelle.FK_IDCOULEURSCELLE).ToList();
                    this.lstLotBrut = args.Result.Where(t => t.Couleur_ID == laDScelle.FK_IDCOULEURSCELLE).ToList();

                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }

            };
            service.RetourneListeScelleByCentreAsync(IdCentreRecuperationDeLot);
        }

        private void ChargeDonneDemande(int pk_id)
        {
            
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Accueil"));
            service.RetourneListeDemandeScelleCompleted += (s, args) =>
            {
                try
                {
                    if (args != null && args.Cancelled)
                        return;
                    lademande = args.Result;

                    if (lademande != null)
                    {
                        CsActivite activite = lstAllActivite.Where(c => c.Activite_ID == lademande.FirstOrDefault().FK_IDACTIVITE).FirstOrDefault();
                        Galatee.Silverlight.ServiceAccueil.CsUtilisateur user = lstAllUser.Where(c => c.PK_ID == lademande.FirstOrDefault().FK_IDAGENT).FirstOrDefault();
                        Galatee.Silverlight.ServiceAccueil.CsCentre centre = SessionObject.LstCentre.Where(c => c.PK_ID == lademande.FirstOrDefault().FK_IDCENTRE).FirstOrDefault();

                         
                        txtcentre.Text = lademande.First().LIBELLECENTREDESTINATAIRE;
                        txtcentre.Tag = lademande.First().FK_IDCENTRE;

                        txtAgent.Text = lademande.First().LIBELLESITEAGENT;
                        txtAgent.Tag = lademande.First().FK_IDAGENT;

                        txtService.Text = lademande.First().LIBELLEACTIVITE;
                        txtService.Tag = lademande.First().FK_IDACTIVITE;

                        string NombreScelle = lademande.FirstOrDefault().NOMBRE_DEM!=null?lademande.FirstOrDefault().NOMBRE_DEM.ToString():string.Empty;
                        string Couleur=lademande.FirstOrDefault().LIBELLECOULEUR!=null?lademande.FirstOrDefault().LIBELLECOULEUR:string.Empty;
                        txtnombreDem.Text = NombreScelle;
                        txtCouleur.Text = Couleur;
                        Nbr_ScelleDemandeRestant =int.Parse( NombreScelle);

                        int IdCentreRecuperationDeLot = lademande.FirstOrDefault().FK_IDCENTREFOURNISSEUR;
                        ChargeLot(IdCentreRecuperationDeLot, lademande.First());
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }

            };
            service.RetourneListeDemandeScelleAsync(pk_id);

        }

        private void SaveAffectationScelle()
        {
            try
            {

                prgBar.Visibility = System.Windows.Visibility.Visible;
                this.OKButton.IsEnabled = false;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.InsertAffectionScelleCompleted += (s, args) =>
                {
                    try
                    {
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;

                        if (args != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors du traitement, il se peut que votre opération ait échoué", "Erreur");
                            return;
                        }
                           if (string.IsNullOrEmpty( args.Result) )
                            Message.Show("Validation effectuée avec succès", "Information");
                        else
                            Message.ShowError(args.Result, "Information");
                        this.DialogResult = true;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                //service.InsertAffectionScelleAsync(lademande.FirstOrDefault().FK_IDDEMANDE, UserConnecte.PK_ID, EtapeActuelle, UserConnecte.matricule, ListLotAffecter_Selectionner);
                service.InsertAffectionScelleAsync(lademande.FirstOrDefault().FK_IDDEMANDE, lademande.FirstOrDefault().NUMDEM , UserConnecte.PK_ID, EtapeActuelle, UserConnecte.matricule, lesLotsChoisis);
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
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
                    //Message.ShowInformation("Sortie materiel effectuée", Langue.lbl_Menu);

                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            SaveAffectationScelle();
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (DgLotMag.SelectedItem != null)
                TxtNbScelle.Text = (((CsLotScelle)DgLotMag.SelectedItem).Nbre_Scelles != null) ? ((CsLotScelle)DgLotMag.SelectedItem).Nbre_Scelles.ToString() : string.Empty;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DgLotMag.SelectedItem != null)
                {
                    CsLotScelle SelectedObject = (CsLotScelle)DgLotMag.SelectedItem;
                    SelectedObject.IsSelect = true;
                    CheckObjet(SelectedObject);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "erreur");
            }
        }

        //private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        //{
        //    //Récupération du lot selectioné
        //    var leLot = (CsLotScelle)DgLotMag.SelectedItem;

        //    CsLotScelle lot = this.lstLotBrut.FirstOrDefault(t => t.Id_LotMagasinGeneral == leLot.Id_LotMagasinGeneral);

        //    CsLotScelle choix = new CsLotScelle();
        //    choix.Activite_ID = lot.Activite_ID;
        //    choix.CodeCentre = lot.CodeCentre;
        //    choix.Couleur_ID = lot.Couleur_ID;
        //    choix.Date_DerniereModif = lot.Date_DerniereModif;
        //    choix.DateReception = lot.DateReception;
        //    choix.Fournisseur_ID = lot.Fournisseur_ID;
        //    choix.Id_Affectation = lot.Id_Affectation;
        //    choix.Id_LotMagasinGeneral = lot.Id_LotMagasinGeneral;
        //    choix.IsSelect = true;
        //    choix.Libelle_Couleur = lot.Libelle_Couleur;
        //    choix.Libelle_Fournisseur = lot.Libelle_Fournisseur;
        //    choix.Libelle_Origine = lot.Libelle_Origine;
        //    choix.Matricule_AgentDerniereModif = lot.Matricule_AgentDerniereModif;
        //    choix.Matricule_AgentReception = lot.Matricule_AgentReception;
        //    choix.Nbre_Scelles = lot.Nbre_Scelles;
        //    choix.Numero_depart = lot.Numero_depart;
        //    choix.Numero_fin = lot.Numero_fin;
        //    choix.Origine_ID = lot.Origine_ID;
        //    choix.StatutLot_ID = lot.StatutLot_ID;


        //    this.total_selected = this.total_selected + lot.Nbre_Scelles;

        //    //Récupération Nombre de Scellé demandé
        //    int Nbr_ScelleDemande = Nbr_ScelleDemandeRestant;
        //    if (ListLotAffecter_Selectionner.Count <= 0)
        //        int.TryParse(txtnombreDem.Text, out Nbr_ScelleDemande);
        //    leLot.IsSelect = true;
        //    //Mise du lot 
        //    leLot.Nbre_Scelles = leLot.Nbre_Scelles - Nbr_ScelleDemande;
        //    choix.Nbre_Scelles = Nbr_ScelleDemande;
        //    if (leLot.Nbre_Scelles <= 0)
        //    {
        //        OKButton.IsEnabled = true;

        //        //if (leLot.Nbre_Scelles == 0)
        //        if (int.Parse(txtnombreDem.Text) == this.total_selected)
        //        {
        //            ListLotStat_Selectionner.Add(leLot, Nbr_ScelleDemande.ToString());
        //            leLot.Numero_depart = leLot.Numero_fin;
        //            leLot.Numero_fin = leLot.Numero_fin;
        //        }
        //        //else 
        //        else if (int.Parse(txtnombreDem.Text) > this.total_selected)
        //        {
        //            ListLotStat_Selectionner.Add(leLot, (Nbr_ScelleDemande - (-(leLot.Nbre_Scelles))).ToString());
        //            Message.ShowInformation("Veuillez sélectionner un autre lot afin de compléter les scellés", "Information");
        //            Nbr_ScelleDemandeRestant = -(leLot.Nbre_Scelles);
        //            Nbr_ScelleDemandeRestant_OverFlow = Nbr_ScelleDemandeRestant;
        //            leLot.Nbre_Scelles = 0;
        //            leLot.Numero_depart = leLot.Numero_fin;
        //            leLot.Numero_fin = leLot.Numero_fin;
        //            OKButton.IsEnabled = false;
        //        }
        //    }
        //    else
        //    {
        //        ListLotStat_Selectionner.Add(leLot, Nbr_ScelleDemande.ToString());
        //        //Mise à jour des info du lot en tenant compte du nombre de position des numero de depart et ou de fin
        //        var NouveauNumeroDeDepart=(int.Parse( leLot.Numero_depart )+ Nbr_ScelleDemande).ToString();
                
        //        var NombreDepositionAncienNumDepart=leLot.Numero_depart.Length;
        //        leLot.Numero_depart = NouveauNumeroDeDepart.PadLeft(NombreDepositionAncienNumDepart, '0');

        //        Nbr_ScelleDemandeRestant = 0;
        //        OKButton.IsEnabled = true;
        //    }

        //    TxtNbScelle.Text = leLot.Nbre_Scelles.ToString();
        //    MiseAjourDataGrille(leLot);
        //    DgLotMag.SelectedItem = leLot;
        //    ListLotAffecter_Selectionner.Add(leLot);
        //    this.lesLotsChoisis.Add(choix);

        //}

        private void MiseAjourDataGrille(CsLotScelle leLot)
        {
            var DataSource = (List<CsLotScelle>)DgLotMag.ItemsSource;
            int index = DataSource.IndexOf((CsLotScelle)DgLotMag.SelectedItem);
            DataSource[index] = leLot;
            DgLotMag.ItemsSource = DataSource;
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DgLotMag.SelectedItem != null)
                {
                    CsLotScelle SelectedObject = (CsLotScelle)DgLotMag.SelectedItem;
                    SelectedObject.IsSelect = false;
                    CheckUncheckObjet(SelectedObject);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Erreur");
            }

        }

        //private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        //{
        //    //Récupération du lot selectioné
        //    var leLot = (CsLotScelle)DgLotMag.SelectedItem;

        //    //Récupération Nombre de Scellé demandé
        //    int Nbr_ScelleDemande = 0;
        //    int.TryParse(txtnombreDem.Text, out Nbr_ScelleDemande);
        //    leLot.IsSelect = false;
        //    var LotStat_Selectionner = ListLotStat_Selectionner.FirstOrDefault(c => c.Key.Id_LotMagasinGeneral == leLot.Id_LotMagasinGeneral);

        //    int Nbr_ScelleDemandeARestituer = 0;

        //    if (LotStat_Selectionner.Value != null)
        //        Nbr_ScelleDemandeARestituer = int.Parse(LotStat_Selectionner.Value);
        //    //Mise à jour des info du lot en tenant compte du nombre de position des numero de depart et ou de fin
        //    var NouveauNumeroDeDepart = (int.Parse(leLot.Numero_depart) - Nbr_ScelleDemandeARestituer).ToString();
        //    //NouveauNumeroDeDepart = (int.Parse(leLot.Numero_depart) - Nbr_ScelleDemandeRestant_OverFlow).ToString();
        //    ListLotStat_Selectionner.Remove(LotStat_Selectionner.Key);

        //    var NombreDepositionAncienNumDepart = leLot.Numero_depart.Length;

        //    leLot.Numero_depart = NouveauNumeroDeDepart.PadLeft(NombreDepositionAncienNumDepart, '0');
        //    //leLot.Nbre_Scelles = leLot.Nbre_Scelles + Nbr_ScelleDemande;
        //    leLot.Nbre_Scelles =(int.Parse( leLot.Numero_fin) - int.Parse( leLot.Numero_depart)) + 1;
        //    //Nbr_ScelleDemandeRestant = Nbr_ScelleDemandeARestituer;
        //    TxtNbScelle.Text = leLot.Nbre_Scelles.ToString();

        //    CsLotScelle lot = this.lstLotBrut.FirstOrDefault(t => t.Id_LotMagasinGeneral == leLot.Id_LotMagasinGeneral);
        //    CsLotScelle a = this.lesLotsChoisis.FirstOrDefault(t => t.Id_LotMagasinGeneral == leLot.Id_LotMagasinGeneral);
        //    this.lesLotsChoisis.Remove(a);

        //    this.total_selected = this.total_selected - lot.Nbre_Scelles;
        //    Nbr_ScelleDemandeRestant = Nbr_ScelleDemandeRestant + lot.Nbre_Scelles;

        //    MiseAjourDataGrille(leLot);
        //    DgLotMag.SelectedItem = leLot;
        //    ListLotAffecter_Selectionner.Remove(leLot);

        //}

        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsLotScelle>;
            if (dg.SelectedItem != null)
            {
                CsLotScelle SelectedObject = (CsLotScelle)dg.SelectedItem;
                if (SelectedObject.IsSelect == false)
                {
                    SelectedObject.IsSelect = true;
                    SelectedObject.Etat = "*";
                    CheckObjet(SelectedObject);
                }
                else
                {
                    SelectedObject.IsSelect = false;
                    SelectedObject.Etat = "";
                    CheckUncheckObjet(SelectedObject);
                }
            }
        }

        private void CheckObjet(CsLotScelle leLot)
        {
            //Récupération du lot selectioné
            CsLotScelle lot = this.lstLotBrut.FirstOrDefault(t => t.Id_LotMagasinGeneral == leLot.Id_LotMagasinGeneral);

            int NbreScelleDuLot = lot.Nbre_Scelles;
            CsLotScelle choix = new CsLotScelle();
            choix.Activite_ID = lot.Activite_ID;
            choix.CodeCentre = lot.CodeCentre;
            choix.Couleur_ID = lot.Couleur_ID;
            choix.Date_DerniereModif = lot.Date_DerniereModif;
            choix.DateReception = lot.DateReception;
            choix.Fournisseur_ID = lot.Fournisseur_ID;
            choix.Id_Affectation = lot.Id_Affectation;
            choix.Id_LotMagasinGeneral = lot.Id_LotMagasinGeneral;
            choix.IsSelect = true;
            choix.Libelle_Couleur = lot.Libelle_Couleur;
            choix.Libelle_Fournisseur = lot.Libelle_Fournisseur;
            choix.Libelle_Origine = lot.Libelle_Origine;
            choix.Matricule_AgentDerniereModif = lot.Matricule_AgentDerniereModif;
            choix.Matricule_AgentReception = lot.Matricule_AgentReception;
            choix.Nbre_Scelles = lot.Nbre_Scelles;
            choix.Numero_depart = lot.Numero_depart;
            choix.Numero_fin = lot.Numero_fin;
            choix.Origine_ID = lot.Origine_ID;
            choix.StatutLot_ID = lot.StatutLot_ID;


            this.total_selected = this.total_selected + lot.Nbre_Scelles;

            //Récupération Nombre de Scellé demandé
            int Nbr_ScelleDemande = Nbr_ScelleDemandeRestant;
            if (ListLotAffecter_Selectionner.Count <= 0)
                int.TryParse(txtnombreDem.Text, out Nbr_ScelleDemande);
            leLot.IsSelect = true;
            //Mise du lot 
            leLot.Nbre_Scelles = leLot.Nbre_Scelles - Nbr_ScelleDemande;
            if (leLot.Nbre_Scelles <= 0)
            {
                OKButton.IsEnabled = true;

                //if (leLot.Nbre_Scelles == 0)
                if (int.Parse(txtnombreDem.Text) == this.total_selected)
                {
                    ListLotStat_Selectionner.Add(leLot, Nbr_ScelleDemande.ToString());
                    leLot.Numero_depart = leLot.Numero_fin;
                    leLot.Numero_fin = leLot.Numero_fin;
                    choix.Nbre_Scelles = Nbr_ScelleDemande;

                }
                //else 
                else if (int.Parse(txtnombreDem.Text) > this.total_selected)
                {
                    ListLotStat_Selectionner.Add(leLot, (Nbr_ScelleDemande - (-(leLot.Nbre_Scelles))).ToString());
                    Message.ShowInformation("Veuillez sélectionner un autre lot afin de compléter les scellés", "Information");
                    Nbr_ScelleDemandeRestant = -(leLot.Nbre_Scelles);
                    Nbr_ScelleDemandeRestant_OverFlow = Nbr_ScelleDemandeRestant;
                    choix.Nbre_Scelles = NbreScelleDuLot - Nbr_ScelleDemandeRestant;
                    leLot.Nbre_Scelles = 0;
                    leLot.Numero_depart = leLot.Numero_fin;
                    leLot.Numero_fin = leLot.Numero_fin;
                    OKButton.IsEnabled = false;
                }
            }
            else
            {
                choix.Nbre_Scelles = Nbr_ScelleDemandeRestant;
                ListLotStat_Selectionner.Add(leLot, Nbr_ScelleDemande.ToString());
                //Mise à jour des info du lot en tenant compte du nombre de position des numero de depart et ou de fin
                var NouveauNumeroDeDepart = (int.Parse(leLot.Numero_depart) + Nbr_ScelleDemande).ToString();

                var NombreDepositionAncienNumDepart = leLot.Numero_depart.Length;
                leLot.Numero_depart = NouveauNumeroDeDepart.PadLeft(NombreDepositionAncienNumDepart, '0');

                Nbr_ScelleDemandeRestant = 0;
                OKButton.IsEnabled = true;
            }

            TxtNbScelle.Text = leLot.Nbre_Scelles.ToString();
            MiseAjourDataGrille(leLot);
            DgLotMag.SelectedItem = leLot;
            ListLotAffecter_Selectionner.Add(leLot);
            this.lesLotsChoisis.Add(choix);

        }

        private void CheckUncheckObjet(CsLotScelle leLot)
        {
            try
            {

                //Récupération Nombre de Scellé demandé
                int Nbr_ScelleDemande = 0;
                int.TryParse(txtnombreDem.Text, out Nbr_ScelleDemande);
                leLot.IsSelect = false;
                var LotStat_Selectionner = ListLotStat_Selectionner.FirstOrDefault(c => c.Key.Id_LotMagasinGeneral == leLot.Id_LotMagasinGeneral);

                int Nbr_ScelleDemandeARestituer = 0;

                if (LotStat_Selectionner.Value != null)
                    Nbr_ScelleDemandeARestituer = int.Parse(LotStat_Selectionner.Value);
                //Mise à jour des info du lot en tenant compte du nombre de position des numero de depart et ou de fin
                var NouveauNumeroDeDepart = (int.Parse(leLot.Numero_depart) - Nbr_ScelleDemandeARestituer).ToString();
                //NouveauNumeroDeDepart = (int.Parse(leLot.Numero_depart) - Nbr_ScelleDemandeRestant_OverFlow).ToString();
                ListLotStat_Selectionner.Remove(LotStat_Selectionner.Key);

                var NombreDepositionAncienNumDepart = leLot.Numero_depart.Length;

                leLot.Numero_depart = NouveauNumeroDeDepart.PadLeft(NombreDepositionAncienNumDepart, '0');
                //leLot.Nbre_Scelles = leLot.Nbre_Scelles + Nbr_ScelleDemande;
                leLot.Nbre_Scelles = (int.Parse(leLot.Numero_fin) - int.Parse(leLot.Numero_depart)) + 1;
                //Nbr_ScelleDemandeRestant = Nbr_ScelleDemandeARestituer;
                TxtNbScelle.Text = leLot.Nbre_Scelles.ToString();

                CsLotScelle lot = this.lstLotBrut.FirstOrDefault(t => t.Id_LotMagasinGeneral == leLot.Id_LotMagasinGeneral);
                CsLotScelle a = this.lesLotsChoisis.FirstOrDefault(t => t.Id_LotMagasinGeneral == leLot.Id_LotMagasinGeneral);
                this.lesLotsChoisis.Remove(a);

                this.total_selected = this.total_selected - lot.Nbre_Scelles;
                Nbr_ScelleDemandeRestant = Nbr_ScelleDemandeRestant + lot.Nbre_Scelles;

                MiseAjourDataGrille(leLot);
                DgLotMag.SelectedItem = leLot;
                ListLotAffecter_Selectionner.Remove(leLot);
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, "Erreur");
            }

        }
    }
}


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
using Galatee.Silverlight.ServiceAccueil  ;
using Galatee.Silverlight.Resources.Accueil;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmModification : ChildWindow
    {
        CsDemande laDemande = new CsDemande();
        public FrmModification()
        {
            InitializeComponent();
        }
        public FrmModification(CsDemande _laDemande)
        {
            InitializeComponent();
            laDemande = _laDemande;
            RemplireDonnee(laDemande);
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            ValidationDemande(laDemande);
           
        }
        private void ValidationDemande(CsDemande _LaDemande)
        {
            try
            {
                _LaDemande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusPriseEnCompte;
                //Lancer la transaction de mise a jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ValiderDemandeCompleted += (sr, res) =>
                {

                    string Messages = string.Empty;
                    //Si la date de d'encaissement n'est pas renseigné c-a-d que la demende est en attente
                    if (_LaDemande.LaDemande.STATUT == SessionObject.Enumere.DemandeStatusEnAttente)
                    {
                        //Msg de confirmation de l'enregistremet
                        Messages = Langue.MsgRequestSaved;

                    }
                    //Si la date d'encaissement est renseigné 
                    else
                    {
                        Messages = Langue.MsgOperationTerminee;
                        Message.ShowInformation(Messages, Langue.lbl_Menu);
                        this.DialogResult = false;
                    }
                };
                service1.ValiderDemandeAsync(_LaDemande);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        UcDemandeDetailAbonnement _LectrlAbonement;
        UcDemandeAddresseClient _LectrlAdresseClient;
        UcDemandeDetailBranchement _LectrlbrtClient;
        UcDemandeDetailClient _LectrlClient;
        UcDemandeDetailModifCompteur _LectrlCompteur;
        private void RemplireDonnee(CsDemande _lademande)
        {
            this.Txt_LibelleCentre.Text = SessionObject.LstCentre.FirstOrDefault(t=>t.CODE == _lademande.LaDemande.CENTRE).LIBELLE ;
            this.Txt_LibelleProduit.Text = SessionObject.ListeDesProduit .FirstOrDefault(t => t.CODE  == _lademande.LaDemande.PRODUIT ).LIBELLE;
            this.Txt_NumDemande .Text = _lademande.LaDemande.NUMDEM ;
            if (_lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ModificationAbonnement)
                RetourneInfoAbon(_lademande.LaDemande.FK_IDCENTRE ,_lademande.LaDemande.CENTRE, _lademande.LaDemande.CLIENT, _lademande.LaDemande.ORDRE, _lademande.LaDemande.PRODUIT);
            if (_lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ModificationAdresse)
                RetourneInfoAddresse(_lademande.LaDemande.FK_IDCENTRE,_lademande.LaDemande.CENTRE, _lademande.LaDemande.CLIENT, _lademande.LaDemande.ORDRE);
            if (_lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ModificationBranchement)
                RetourneInfoBranchement(_lademande.LaDemande.FK_IDCENTRE,_lademande.LaDemande.CENTRE, _lademande.LaDemande.CLIENT, _lademande.LaDemande.PRODUIT);
            if (_lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ModificationClient)
                RetourneInfoClient(_lademande.LaDemande.FK_IDCENTRE,_lademande.LaDemande.CENTRE, _lademande.LaDemande.CLIENT, _lademande.LaDemande.ORDRE);
            if (_lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ModificationCompteur)
                RetourneInfoCompteur(_lademande.LaDemande.FK_IDCENTRE,_lademande.LaDemande.CENTRE, _lademande.LaDemande.CLIENT, _lademande.LaDemande.PRODUIT);

            //else if (_lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ModificationAdresse )
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void RetourneInfoAbon(int fk_idCentre, string centre, string client, string ordre, string produit)
        {

            int res = LoadingManager.BeginLoading(Langue.En_Cours);

            try
            {
                List<CsAbon> AbonementRecherche = new List<CsAbon>();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneAbonCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    AbonementRecherche = args.Result;
                    if (AbonementRecherche != null)
                    {
                        CsAbon _LeAbonnementProduit = AbonementRecherche.FirstOrDefault(p => p.PRODUIT == produit);
                        TabItem tabItem1 = new TabItem();
                        tabItem1.Header = "ANCIENNES DONNEES";
                        _LectrlAbonement = new UcDemandeDetailAbonnement(_LeAbonnementProduit);

                        tabItem1.Content = _LectrlAbonement;
                        tabControl1.Items.Add(tabItem1);
                        tabControl1.SelectedItem = tabItem1;

                        laDemande.Abonne.PK_ID = _LeAbonnementProduit.PK_ID;

                        TabItem tabItem2 = new TabItem();
                        tabItem2.Header = "NOUVELLES DONNEES";
                        _LectrlAbonement = new UcDemandeDetailAbonnement(laDemande, true);
                        tabItem2.Content = _LectrlAbonement;
                        tabControl1.Items.Add(tabItem2);
                        tabControl1.SelectedItem = tabItem2;
                        LoadingManager.EndLoading(res);

                    }
                };
                service.RetourneAbonAsync(fk_idCentre , centre, client, ordre);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }
        }
        private void RetourneInfoAddresse(int fk_idcentre, string centre, string client, string ordre)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);
            try
            {
                CsAg AdresseRechercher = new CsAg();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneAdresseCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    AdresseRechercher = args.Result;
                    if (AdresseRechercher != null && !string.IsNullOrEmpty(AdresseRechercher.CENTRE))
                    {
                       
                            TabItem tabItem1 = new TabItem();
                            tabItem1.Header = "ANCIENNES DONNEES";
                            //_LectrlAdresseClient = new UcDemandeAddresseClient(AdresseRechercher,laDemande.LaDemande.TYPEDEMANDE );

                            tabItem1.Content = _LectrlAdresseClient;
                            tabControl1.Items.Add(tabItem1);
                            tabControl1.SelectedItem = tabItem1;

                            laDemande.Ag.PK_ID = AdresseRechercher.PK_ID;

                            TabItem tabItem2 = new TabItem();
                            tabItem2.Header = "NOUVELLES DONNEES";
                            _LectrlAdresseClient = new UcDemandeAddresseClient(laDemande, true);
                            tabItem2.Content = _LectrlAdresseClient;
                            tabControl1.Items.Add(tabItem2);
                            tabControl1.SelectedItem = tabItem2;
                            LoadingManager.EndLoading(res);

                    }
                    LoadingManager.EndLoading(res);

                };
                service.RetourneAdresseAsync(fk_idcentre,centre, client, ordre);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }

        }
        private void RetourneInfoBranchement(int fk_idcentre,string centre, string client, string produit)
        {
          CsBrt   BranchementClientRecherche = new CsBrt();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneBranchementCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    BranchementClientRecherche = args.Result.FirstOrDefault(p => p.PRODUIT == produit);
                    TabItem tabItem1 = new TabItem();
                    tabItem1.Header = "ANCIENNES DONNEES";
                    _LectrlbrtClient = new UcDemandeDetailBranchement(BranchementClientRecherche,laDemande.LaDemande.TYPEDEMANDE );

                    tabItem1.Content = _LectrlbrtClient;
                    tabControl1.Items.Add(tabItem1);
                    tabControl1.SelectedItem = tabItem1;

                    laDemande.Branchement.PK_ID = BranchementClientRecherche.PK_ID;
                    laDemande.Branchement.FK_IDAG   = BranchementClientRecherche.FK_IDAG  ;

                    TabItem tabItem2 = new TabItem();
                    tabItem2.Header = "NOUVELLES DONNEES";
                    _LectrlbrtClient = new UcDemandeDetailBranchement(laDemande, true);
                    tabItem2.Content = _LectrlbrtClient;
                    tabControl1.Items.Add(tabItem2);
                    tabControl1.SelectedItem = tabItem2;
                }

            };
            service.RetourneBranchementAsync(fk_idcentre,centre, client, produit);
            service.CloseAsync();

        }
        private void RetourneInfoClient(int fk_idcentre,string centre, string client, string ordre)
        {
            CsClient ClientRecherche = new CsClient();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneClientCompleted  += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    ClientRecherche = args.Result;
                    TabItem tabItem1 = new TabItem();
                    tabItem1.Header = "ANCIENNES DONNEES";
                    _LectrlClient = new UcDemandeDetailClient(ClientRecherche, laDemande.LaDemande.TYPEDEMANDE);

                    tabItem1.Content = _LectrlClient;
                    tabControl1.Items.Add(tabItem1);
                    tabControl1.SelectedItem = tabItem1;

                    laDemande.LeClient.PK_ID = ClientRecherche.PK_ID;

                    TabItem tabItem2 = new TabItem();
                    tabItem2.Header = "NOUVELLES DONNEES";
                    _LectrlClient = new UcDemandeDetailClient(laDemande, true);
                    tabItem2.Content = _LectrlClient;
                    tabControl1.Items.Add(tabItem2);
                    tabControl1.SelectedItem = tabItem2;
                }

            };
            service.RetourneClientAsync(fk_idcentre,centre, client, ordre);
            service.CloseAsync();

        }
        private void RetourneInfoCompteur(int fk_idcentre,string centre, string client, string produit)
        {
            CsCanalisation CompteurClientRecherche = new CsCanalisation();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneCanalisationCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    CompteurClientRecherche = args.Result.FirstOrDefault(p => p.PRODUIT == produit);
                    TabItem tabItem1 = new TabItem();
                    tabItem1.Header = "ANCIENNES DONNEES";
                    laDemande.LaDemande.PRODUIT = CompteurClientRecherche.PRODUIT;
                    _LectrlCompteur = new UcDemandeDetailModifCompteur(CompteurClientRecherche, laDemande.LaDemande.TYPEDEMANDE);

                    tabItem1.Content = _LectrlCompteur;
                    tabControl1.Items.Add(tabItem1);
                    tabControl1.SelectedItem = tabItem1;

                    //laDemande.Branchement.PK_ID = BranchementClientRecherche.PK_ID;
                    //laDemande.Branchement.FK_IDCLIENT = BranchementClientRecherche.FK_IDCLIENT;

                    TabItem tabItem2 = new TabItem();
                    tabItem2.Header = "NOUVELLES DONNEES";
                    _LectrlCompteur = new UcDemandeDetailModifCompteur(laDemande, 1, true);
                    tabItem2.Content = _LectrlCompteur;
                    tabControl1.Items.Add(tabItem2);
                    tabControl1.SelectedItem = tabItem2;
                }

            };
            service.RetourneCanalisationAsync(fk_idcentre,centre, client, produit, null);
            service.CloseAsync();

        }

        private void btn_Rejeter_Click_1(object sender, RoutedEventArgs e)
        {
            //FrAnotation ctr = new FrAnotation();
            //laDemande.LaDemande.FREP = ctr.txtAnnotation.Text;
        }
    }
}


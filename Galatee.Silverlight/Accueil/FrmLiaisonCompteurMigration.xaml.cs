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
using Galatee.Silverlight.ServiceAccueil   ;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmLiaisonCompteurMigration : ChildWindow
    {
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        CsDemandeBase laDemandeSelect = null;
        CsDemande laDetailDemande = null;

        List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
        List<CsCompteur> LstCompteur = new List<CsCompteur>();
        List<CsDemande> listeDemandeSelectionees = new List<CsDemande>();
        CsReglageCompteur leReglageCompteur = null;
        CsCalibreCompteur leCalibreCompteur = null;

        public FrmLiaisonCompteurMigration()
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_ReferenceClient.MaxLength = SessionObject.Enumere.TailleClient;
            this.Txt_Ordre.MaxLength = SessionObject.Enumere.TailleOrdre;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            AfficherOuMasquer(tabItemClient, false);
            AfficherOuMasquer(tabItemAbon, false);
            AfficherOuMasquer(tabItemClient, false);
            ChargerDonneeDuSite();
            ChargerCalibreCompteur();
            ChargerReglageCompteur();
        }
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<CsSite>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<CsCentre>(); 
        private void ChargerDonneeDuSite()
        {
            try
            {
                SessionObject.ModuleEnCours = "Accueil";
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                     LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                     lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                     loadCompteur(lstSite.Select(y => y.CODE).ToList());
                     return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
              
                    loadCompteur(lstSite.Select(y => y.CODE).ToList());

                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AfficherOuMasquer(TabItem pTabItem, bool pValue)
        {
            try
            {
                pTabItem.Visibility = pValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
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
                if (laDetailDemande != null)
                   ValiderReliaison(laDetailDemande);
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
               
                    if (Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient && this.Txt_Ordre.Text .Length == SessionObject.Enumere.TailleOrdre )
                    {
                        ChargerClientFromReferenceOrdre(this.Txt_ReferenceClient.Text, this.Txt_Ordre.Text);
                    }
                    else
                        Message.Show("La reference saisie n'est pas correcte", "Infomation");
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }
        private void ChargerClientFromReferenceOrdre(string ReferenceClient, string Ordre)
        {
            try
            {
                
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneClientByReferenceOrdreCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    if (args.Result != null && args.Result.Count > 1)
                    {
                        List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(args.Result);
                        Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CENTRE", "LIBELLESITE", "Liste des site");
                        ctr.Show();
                        ctr.Closed += new EventHandler(galatee_OkClickedChoixClient);
                    }
                    else
                        ChargeDetailDEvis(args.Result.First());
                };
                service.RetourneClientByReferenceOrdreAsync(ReferenceClient, Ordre, LstCentre.Select(o=>o.PK_ID).ToList());
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }

        }
        private void galatee_OkClickedChoixClient(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsClient _UnClient = (CsClient)ctrs.MyObject;
                ChargeDetailDEvis(_UnClient);
            }
        }
        private void VerifieExisteDemande(CsClient leClient)
        {

            try
            {
                if (!string.IsNullOrEmpty(Txt_ReferenceClient.Text) && Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient)
                {
                    string OrdreMax = string.Empty;
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneDemandeClientTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result != null)
                        {
                            if (args.Result.DATEFIN == null && args.Result.ISSUPPRIME != true)
                            {
                                Message.ShowInformation("Il existe une demande numero " + args.Result.NUMDEM + " sur ce client", "Accueil");
                                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                return;
                            }
                        }
                        ChargeDetailDEvis(leClient);
                    };
                    service.RetourneDemandeClientTypeAsync(leClient);
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void ChargeDetailDEvis(CsClient leclient)
        {

            try
            {
                leclient.TYPEDEMANDE =SessionObject.Enumere.ChangementCompteur;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GeDetailByFromClientCompleted += (ssender, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    laDetailDemande =args.Result;



                    laDetailDemande.LaDemande = new CsDemandeBase();
                    laDetailDemande.LaDemande.CENTRE = laDetailDemande.Abonne.CENTRE;
                    laDetailDemande.LaDemande.CLIENT = laDetailDemande.Abonne.CLIENT;
                    laDetailDemande.LaDemande.ORDRE = laDetailDemande.Abonne.ORDRE;
                    laDetailDemande.LaDemande.PRODUIT  = laDetailDemande.Abonne.PRODUIT ;
                    laDetailDemande.LaDemande.FK_IDPRODUIT = laDetailDemande.Abonne.FK_IDPRODUIT;
                    laDetailDemande.LaDemande.CENTRE = laDetailDemande.Abonne.CENTRE;
                    laDetailDemande.LaDemande.NOMCLIENT = laDetailDemande.Abonne.NOMABON;
                    if (laDetailDemande.LstCanalistion != null && laDetailDemande.LstCanalistion.Count != 0)
                    laDetailDemande.LaDemande.REGLAGECOMPTEUR  = laDetailDemande.LstCanalistion.First().REGLAGECOMPTEUR ;


                    laDemandeSelect = laDetailDemande.LaDemande;
                    RemplireOngletClient(laDetailDemande.LeClient);
                    RemplirOngletAbonnement(laDetailDemande.Abonne );
                    RenseignerInformationsDevis(laDetailDemande.Abonne);

                    if (laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.Prepaye)
                    {
                        Message.ShowInformation("Cet abonnement n'est pas prépayé", "Info");
                        this.OKButton.IsEnabled = false;
                    }
                    else
                        this.OKButton.IsEnabled = true ;

                };
                client.GeDetailByFromClientAsync(leclient);
            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError("Erreur au chargement des donnéés", "Demande");
            }
        }
        CsReglageCompteur ReglageCompt = null;
        private void loadCompteur(List<string> lstCodeSite)
        {
            AcceuilServiceClient service2 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service2.RetourneListeCompteurMagasinCompleted += (sr2, res2) =>
            {

                if (res2 != null && res2.Cancelled)
                    return;
                LstCompteur = res2.Result;

            };
            service2.RetourneListeCompteurMagasinAsync(lstCodeSite);
            service2.CloseAsync();
        }
        private void ChargerCalibreCompteur()
        {
            try
            {
                if (SessionObject.LstCalibreCompteur.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCalibreCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCalibreCompteur = args.Result;
                };
                service.ChargerCalibreCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerReglageCompteur()
        {
            try
            {
                if (SessionObject.LstReglageCompteur.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerReglageCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstReglageCompteur = args.Result;
                };
                service.ChargerReglageCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void RenseignerInformationsDevis(CsAbon leAbon)
        {
            try
            {
                if (leAbon != null)
                {
                    CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == leAbon.FK_IDCENTRE);

                    Txt_CodeSite.Text = !string.IsNullOrEmpty(leCentre.CODESITE) ? leCentre.CODESITE : string.Empty;
                    Txt_CodeCentre.Text = !string.IsNullOrEmpty(leAbon.CENTRE) ? leAbon.CENTRE : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(leAbon.LIBELLECENTRE) ? leAbon.LIBELLECENTRE : string.Empty;
                    Txt_CodeProduit.Text = !string.IsNullOrEmpty(leAbon.PRODUIT) ? leAbon.PRODUIT : string.Empty;
                    Txt_LibelleSite.Text = !string.IsNullOrEmpty(leCentre.LIBELLESITE) ? leCentre.LIBELLESITE : string.Empty;
                    Txt_LibelleProduit.Text = !string.IsNullOrEmpty(leAbon.LIBELLEPRODUIT) ? leAbon.LIBELLEPRODUIT : string.Empty;

                    if (laDetailDemande.LstCanalistion != null && laDetailDemande.LstCanalistion.Count != 0)
                        Txt_Compteur.Text = laDetailDemande.LstCanalistion.First().NUMERO;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void RemplireOngletClient(CsClient _LeClient)
        {
            try
            {
                if (_LeClient != null)
                {
                    AfficherOuMasquer(tabItemClient, true );
                    this.Txt_NomClient.Text = (string.IsNullOrEmpty(_LeClient.NOMABON) ? string.Empty : _LeClient.NOMABON);
                    this.tab12_Txt_LibelleCodeConso.Text = string.IsNullOrEmpty(_LeClient.LIBELLECODECONSO) ? string.Empty : _LeClient.LIBELLECODECONSO;
                    this.tab12_Txt_LibelleCategorie.Text = string.IsNullOrEmpty(_LeClient.LIBELLECATEGORIE) ? string.Empty : _LeClient.LIBELLECATEGORIE;
                    this.tab12_Txt_LibelleEtatClient.Text = string.IsNullOrEmpty(_LeClient.LIBELLERELANCE) ? string.Empty : _LeClient.LIBELLERELANCE;
                    this.tab12_Txt_LibelleTypeClient.Text = string.IsNullOrEmpty(_LeClient.LIBELLENATURECLIENT) ? string.Empty : _LeClient.LIBELLENATURECLIENT;
                    this.tab12_Txt_Nationnalite.Text = string.IsNullOrEmpty(_LeClient.LIBELLENATIONALITE) ? string.Empty : _LeClient.LIBELLENATIONALITE;
                    this.tab12_Txt_Datecreate.Text = string.IsNullOrEmpty(_LeClient.DATECREATION.ToString()) ? string.Empty : Convert.ToDateTime(_LeClient.DATECREATION).ToShortDateString();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void RemplirOngletAbonnement(CsAbon  _LeAbon)
        {
            if (_LeAbon != null)
            {
                AfficherOuMasquer(tabItemAbon, true);

                this.Txt_CodeTarif.Text = !string.IsNullOrEmpty(_LeAbon.TYPETARIF) ? _LeAbon.TYPETARIF : string.Empty;
                this.Txt_CodePussanceSoucrite.Text = !string.IsNullOrEmpty(_LeAbon.PUISSANCE.Value.ToString()) ? _LeAbon.PUISSANCE.Value.ToString() : string.Empty;

                if (_LeAbon.PUISSANCE != null)
                    this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(_LeAbon.PUISSANCE.ToString()).ToString("N2");
                if (_LeAbon.PUISSANCEUTILISEE != null)
                    this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(_LeAbon.PUISSANCEUTILISEE.Value).ToString("N2");

                this.Txt_CodeForfait.Text = string.IsNullOrEmpty(_LeAbon.FORFAIT) ? string.Empty : _LeAbon.FORFAIT;
                this.Txt_LibelleForfait.Text = string.IsNullOrEmpty(_LeAbon.LIBELLEFORFAIT) ? string.Empty : _LeAbon.LIBELLEFORFAIT;

                this.Txt_CodeTarif.Text = string.IsNullOrEmpty(_LeAbon.TYPETARIF) ? string.Empty : _LeAbon.TYPETARIF;
                this.Txt_LibelleTarif.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLETARIF) ? _LeAbon.LIBELLETARIF : string.Empty;

                this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(_LeAbon.PERFAC) ? string.Empty : _LeAbon.PERFAC;
                this.Txt_LibelleFrequence.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEFREQUENCE) ? _LeAbon.LIBELLEFREQUENCE : string.Empty;

                this.Txt_CodeMoisIndex.Text = string.IsNullOrEmpty(_LeAbon.MOISREL) ? string.Empty : _LeAbon.MOISREL;
                this.Txt_LibelleMoisIndex.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEMOISIND) ? _LeAbon.LIBELLEMOISIND : string.Empty;

                this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(_LeAbon.MOISFAC) ? string.Empty : _LeAbon.MOISFAC;
                this.Txt_LibMoisFact.Text = !string.IsNullOrEmpty(_LeAbon.LIBELLEMOISFACT) ? _LeAbon.LIBELLEMOISFACT : string.Empty;

                this.Txt_DateAbonnement.Text = (_LeAbon.DABONNEMENT == null) ?string.Empty  : Convert.ToDateTime(_LeAbon.DABONNEMENT.Value).ToShortDateString();
            }
        }
        private void btn_Attribuer_Click_1(object sender, RoutedEventArgs e)
        {
            if (laDetailDemande.LstCanalistion != null && laDetailDemande.LstCanalistion.Count != 0)
            {

                leReglageCompteur = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.CODE == laDetailDemande.LstCanalistion.First().REGLAGECOMPTEUR);
                if (leReglageCompteur != null && leReglageCompteur.PK_ID != 0)
                {
                    int FK_IDTYPECOMPTEUR = 10;
                    if (laDetailDemande.LaDemande.REGLAGECOMPTEUR != null && laDetailDemande.LstCanalistion.First().REGLAGECOMPTEUR.Substring(0, 1) == "4")
                        FK_IDTYPECOMPTEUR = 9;

                    List<CsCalibreCompteur> LeCalibreEquivalant = SessionObject.LstCalibreCompteur.Where(t =>
                                                                                                              t.REGLAGEMAXI >= leReglageCompteur.REGLAGEMAXI &&
                                                                                                              t.FK_IDPRODUIT == laDetailDemande.Abonne.FK_IDPRODUIT).ToList();

                    string site = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == laDetailDemande.Abonne.FK_IDCENTRE).CODESITE;
                    List<int> lesIdCalibre = LeCalibreEquivalant.Select(u => u.PK_ID).ToList();
                    Galatee.Silverlight.Devis.UcDetailCompteur ctr = new Galatee.Silverlight.Devis.UcDetailCompteur(LstCompteur.Where(t => t.FK_IDTYPECOMPTEUR == FK_IDTYPECOMPTEUR && t.FK_IDCALIBRECOMPTEUR != null && t.CODESITE == site && t.CODEPRODUIT == laDetailDemande.Abonne.PRODUIT && lesIdCalibre.Contains(t.FK_IDCALIBRECOMPTEUR.Value)).ToList());
                    ctr.Closed += new EventHandler(galatee_Check);
                    ctr.Show();
                }
            }
        }

        void galatee_Check(object sender, EventArgs e)
        {
            Galatee.Silverlight.Devis.UcDetailCompteur ctrs = sender as Galatee.Silverlight.Devis.UcDetailCompteur;
            if (ctrs.isOkClick)
            {
                   List<CsCompteur> _LesCompteurs = (List<CsCompteur>)ctrs.MyObject;
                   laDetailDemande.LaDemande.COMPTEUR = _LesCompteurs.FirstOrDefault().NUMERO;
                   laDetailDemande.LaDemande.FK_IDMAGAZINVIRTUEL  = _LesCompteurs.FirstOrDefault().PK_ID ;
                   Txt_Compteur.Text = _LesCompteurs.FirstOrDefault().NUMERO;
            }
        }
        private void ValiderReliaison(CsDemande lesDemande)
        {
            try
            {
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.EclipseSimpleCompteurTransitionCompleted += (sr, res) =>
                {

                    if (res != null && res.Cancelled)
                        return;
                    if (res.Result  == true)
                    {

                        List<CsDemandeBase> l = new List<CsDemandeBase>();
                        lesDemande.LaDemande.NOMCLIENT = lesDemande.Abonne.NOMABON;
                        l.Add(lesDemande.LaDemande);
                        Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(l, null, SessionObject.CheminImpression, "LiaisonCompteur", "Accueil", true);
                        Message.ShowInformation("Liaisons de compteurs effectuées", "Devis");
                        this.DialogResult = true;
                    }
                    else
                        Message.ShowError("Erreur a la liaison compteur", "Devis");
                };
                service1.EclipseSimpleCompteurTransitionAsync(lesDemande);
                service1.CloseAsync();
            }
            catch
            {

            }

        }
   
    }
}


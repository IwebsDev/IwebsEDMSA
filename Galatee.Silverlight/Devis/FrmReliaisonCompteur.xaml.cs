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
    public partial class FrmReliaisonCompteur : ChildWindow
    {
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        CsDemandeBase laDemandeSelect = null;
        CsDemande laDetailDemande = null;

        List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
        List<CsCompteur> LstCompteur = new List<CsCompteur>();
        List<CsDemande> listeDemandeSelectionees = new List<CsDemande>();
        CsReglageCompteur leReglageCompteur = null;
        CsCalibreCompteur leCalibreCompteur = null;

        public FrmReliaisonCompteur()
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_NumeroDemande.MaxLength = SessionObject.Enumere.TailleNumeroDemande;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            AfficherOuMasquer(tabItemClient, false);
            AfficherOuMasquer(tabItemAbon, false);
            AfficherOuMasquer(tabItemClient, false);
            ChargerDonneeDuSite();
            ChargerCalibreCompteur();
            ChargerReglageCompteur();
        }

        string leEtatExecuter = string.Empty;
        public FrmReliaisonCompteur(string typeEtat)
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_NumeroDemande.MaxLength = SessionObject.Enumere.TailleNumeroDemande;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            AfficherOuMasquer(tabItemClient, false);
            AfficherOuMasquer(tabItemAbon, false);
            AfficherOuMasquer(tabItemClient, false);
            leEtatExecuter = typeEtat;
            ChargerDonneeDuSite();
            ChargerCalibreCompteur();
            ChargerReglageCompteur();
            this.Txt_CompteurNouv.Visibility = System.Windows.Visibility.Collapsed;
            this.lbl_EtapeEnCours_Copy.Visibility = System.Windows.Visibility.Collapsed;
            
            if (leEtatExecuter == SessionObject.DeliaisonCompteur ||
                leEtatExecuter == SessionObject.CorrectionNumCompteur )
                btn_Attribuer.Visibility = System.Windows.Visibility.Collapsed;

            if (leEtatExecuter != SessionObject.DeliaisonCompteur)
            {
                ckbDefectueux.Visibility = System.Windows.Visibility.Collapsed;
                ckbDoubleLiaison.Visibility = System.Windows.Visibility.Collapsed;
            }


            if (leEtatExecuter == SessionObject.CorrectionNumCompteur)
            {
                this.Txt_CompteurNouv.Visibility = System.Windows.Visibility.Visible;
                this.lbl_EtapeEnCours_Copy.Visibility = System.Windows.Visibility.Visible;

            }


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
                if (laDemandeSelect != null)
                {
                    if (leEtatExecuter == SessionObject.DeliaisonCompteur &&
                         laDetailDemande != null && laDetailDemande.InfoDemande != null &&
                         laDetailDemande.InfoDemande.FK_IDETAPEACTUELLE == 0)
                    {
                        Message.ShowInformation("La demande est déja cloturée vous ne pouvez pas délier ce compteur" + "\n\r" +
                                                "Veuillez corriger le numéro de compteur en cas d'erreur", "Demande");
                        return;
                    }
                    if (leEtatExecuter == SessionObject.DeliaisonCompteur)
                        ValiderDeliaison(laDetailDemande);

                    if (leEtatExecuter == SessionObject.CorrectionNumCompteur)
                    {
                        laDetailDemande.LaDemande.COMPTEUR = this.Txt_CompteurNouv.Text;
                        ValiderReliaisonSimple(laDetailDemande);
                    }
                    if (leEtatExecuter == SessionObject.ReliaisonCompteur)
                    {
                        List<CsDemandeBase> lstDemande = new List<CsDemandeBase>();
                        lstDemande.Add(laDetailDemande.LaDemande);
                        ValiderReliaison(lstDemande);
                    }
                }
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
                if (!string.IsNullOrEmpty(Txt_NumeroDemande.Text))
                    RetourneDemandeByNumero(Txt_NumeroDemande.Text);
                else
                {
                    Message.ShowInformation("Saisir le numéro de la demande", "Demande");
                    return;
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }
        CsReglageCompteur ReglageCompt = null;
        private void RetourneDemandeByNumero(string Numerodemande)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible ;
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GetDevisByNumDemandeCompleted += (ssender, args) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed ;

                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (args.Result == null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    else
                    {
                        laDetailDemande = args.Result;
                        laDemandeSelect = laDetailDemande.LaDemande;
                        this.txtCentre.Text = string.IsNullOrEmpty(laDemandeSelect.LIBELLECENTRE) ? string.Empty : laDemandeSelect.LIBELLECENTRE;  
                        this.txtSite.Text = string.IsNullOrEmpty(laDemandeSelect.LIBELLESITE) ? string.Empty : laDemandeSelect.LIBELLESITE; 
                     
                        if (laDemandeSelect.ISSUPPRIME==true )
                        {
                            this.OKButton.IsEnabled = false;
                            Message.ShowInformation("Demande déja supprimée", "Demande");
                            return;
                        }
                        else
                        {
                            //if (laDemandeSelect.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement
                            //    || laDemandeSelect.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul 
                            //    || laDemandeSelect.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur 
                            //    )
                            //{
                                RemplireOngletClient(laDetailDemande.LeClient);
                                RemplirOngletAbonnement(laDetailDemande.Abonne);
                                RenseignerInformationsDevis(laDetailDemande);
                                this.OKButton.IsEnabled = true ;
                            //}
                            //else 
                            //{
                            //    this.OKButton.IsEnabled = false;
                            //    Message.ShowInformation("Pas de liaison pour ce type de demande", "Demande");
                            //    return;
                            //}
                        }
                    }
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.GetDevisByNumDemandeAsync(Numerodemande);
            }
            catch (Exception ex)
            {
                Message.Show("Erreur au chargement de la demande", "Demande");
            }
        }

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
        private void RenseignerInformationsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                     CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == laDemande.LaDemande.FK_IDCENTRE);

                    Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE.ToString()) ? laDemande.LaDemande.ORDRE.ToString() : string.Empty;
                    Txt_CodeSite.Text = !string.IsNullOrEmpty(leCentre.CODESITE) ? leCentre.CODESITE : string.Empty;
                    Txt_CodeCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CENTRE) ? laDemande.LaDemande.CENTRE : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLECENTRE) ? laDemande.LaDemande.LIBELLECENTRE : string.Empty;
                    Txt_CodeProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.PRODUIT) ? laDemande.LaDemande.PRODUIT : string.Empty;
                    Txt_LibelleSite.Text = !string.IsNullOrEmpty(leCentre.LIBELLESITE) ? leCentre.LIBELLESITE : string.Empty;
                    Txt_LibelleProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLEPRODUIT) ? laDemande.LaDemande.LIBELLEPRODUIT : string.Empty;
                    Txt_LibelleTypeDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLETYPEDEMANDE) ? laDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;
                    this.Title = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;


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
            leReglageCompteur = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.CODE == laDetailDemande.LaDemande.REGLAGECOMPTEUR);
               if (leReglageCompteur != null && leReglageCompteur.PK_ID != 0)
               {
                   int FK_IDTYPECOMPTEUR = 10;
                   if (laDetailDemande.LaDemande.REGLAGECOMPTEUR != null && laDetailDemande.LaDemande.REGLAGECOMPTEUR.Substring(0, 1) == "4")
                       FK_IDTYPECOMPTEUR = 9;

                   List<CsCalibreCompteur> LeCalibreEquivalant = SessionObject.LstCalibreCompteur.Where(t =>
                                                                                                             t.REGLAGEMAXI >= leReglageCompteur.REGLAGEMAXI &&
                                                                                                             t.FK_IDPRODUIT == laDetailDemande.LaDemande.FK_IDPRODUIT).ToList();

                   List<int> lesIdCalibre = LeCalibreEquivalant.Select(u => u.PK_ID).ToList();
                   Galatee.Silverlight.Devis.UcDetailCompteur ctr = new Galatee.Silverlight.Devis.UcDetailCompteur(laDetailDemande.LaDemande, LstCompteur.Where(t => t.FK_IDTYPECOMPTEUR == FK_IDTYPECOMPTEUR && t.FK_IDCALIBRECOMPTEUR != null && t.CODESITE == laDetailDemande.LaDemande.SITE && t.CODEPRODUIT == laDetailDemande.LaDemande.PRODUIT && lesIdCalibre.Contains(t.FK_IDCALIBRECOMPTEUR.Value)).ToList());
                   ctr.Closed += new EventHandler(galatee_Check);
                   ctr.Show();
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
        private void ValiderReliaison(List<CsDemandeBase> lesDemandeSelect)
        {
            try
            {
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.InsertLiaisonCompteurCompleted += (sr, res) =>
                {

                    if (res != null && res.Cancelled)
                        return;
                    List<CsDemandeBase> resultat = res.Result;
                    if (resultat != null && resultat.Count != 0)
                    {
                        Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(resultat, null, SessionObject.CheminImpression, "LiaisonCompteur", "Accueil", true);
                        Message.ShowInformation("Liaisons de compteurs effectuées", "Devis");
                        this.DialogResult = true;
                    }
                    else
                        Message.ShowError("Aucun compteur trouvé", "Devis");
                };
                service1.InsertLiaisonCompteurAsync(lesDemandeSelect);
                service1.CloseAsync();
            }
            catch
            {

            }

        }
        private void ValiderDeliaison(CsDemande lesDemandeSelect)
        {
            try
            {
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.DeLiaisonCompteuriWEBSCompleted += (sr, res) =>
                {

                    if (res != null && res.Cancelled)
                        return;
                    if (res.Result == null )
                    {
                            Message.ShowError("Déliaison non validée", "Devis");
                            return;  
                    }
                    else
                    {
                        if (res.Result.Count == 0)
                        {
                            Message.ShowInformation("Déliaison validée", "Devis");
                            this.Close();
                            return;
                        }
                        else
                        {
                            string list = string.Empty;
                            if (res.Result.Count == 1 && res.Result.First().NUMDEM == "0")
                                Message.ShowWarning(string.Format("Le compteur est lié à d'autres demandes déjà mises à jour.\nImpossible de retirer le compteur du magasin virtuel."), "Devis");
                            else
                            {
                                foreach (CsCanalisation item in res.Result)
                                {
                                    if (string.IsNullOrEmpty(list))
                                        list = item.NUMDEM;
                                    else
                                        list = string.Format(list + "\n{0}", item.NUMDEM);
                                }
                                Message.ShowWarning(string.Format("Impossible de délier cette demande. Le compteur est lié à d'autres demandes \n{0} \nqu'il faudra délier.\nSi vous voulez également délier les autres demandes il faut cocher les deux options.", list), "Devis");
                            }
                            return;
                        }

                    }
                };
                //service1.DeLiaisonCompteurAsync(lesDemandeSelect);
                service1.DeLiaisonCompteuriWEBSAsync(lesDemandeSelect, this.ckbDefectueux.IsChecked.Value, this.ckbDoubleLiaison.IsChecked.Value);
                service1.CloseAsync();
            }
            catch
            {

            }

        }
        private void ValiderReliaisonSimple(CsDemande lesDemandeSelect)
        {
            try
            {
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ReLiaisonCompteurSimpleCompleted += (sr, res) =>
                {

                    if (res != null && res.Cancelled)
                        return;
                    if (res.Result != null)
                    {
                        if (res.Result == true)
                        {
                            Message.ShowInformation("Réliaison validée", "Devis");
                            return;
                        }
                        else
                        {
                            Message.ShowInformation("Réliaison non validée", "Devis");
                            return;
                        }

                    }
                    else
                        Message.ShowError("Aucun compteur trouvé", "Devis");
                };
                service1.ReLiaisonCompteurSimpleAsync(lesDemandeSelect);
                service1.CloseAsync();
            }
            catch
            {

            }

        }
    }
}


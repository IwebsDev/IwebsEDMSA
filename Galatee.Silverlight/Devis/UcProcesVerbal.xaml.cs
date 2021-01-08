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
using Galatee.Silverlight.ServiceAccueil ;
using Galatee.Silverlight.Resources.Devis;
using System.IO;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Devis
{
    public partial class UcProcesVerbal : ChildWindow
    {
        public ObjDEVIS DevisSelectionne { get; set; }
        public List<ObjELEMENTDEVIS> Elements { get; set; }
        public ObjTRAVAUXDEVIS Travaux { get; set; }
        public string ProcesVerbal { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = new CsDemande();
        public DateTime? dtp_DateDepose = new DateTime();

        List<CsOrganeScellable> ListeOrganeScellable = new List<CsOrganeScellable>();
        List<CsOrganeScelleDemande> ListeOrganeScelleDemande = new List<CsOrganeScelleDemande>();
        List<CsTypeDOCUMENTSCANNE> LstTypeDocument = new List<CsTypeDOCUMENTSCANNE>();
        List<CsScelle> ListeScelle = new List<CsScelle>();
        List<CsScelle> ListeScelleToRemove = new List<CsScelle>();
        private List<CsTcompteur> _listeCanalisation = new List<CsTcompteur>();
        private List<CsMarqueCompteur> _listeMarqueCpt = new List<CsMarqueCompteur>();
        private List<CsReglageCompteur> _listeDesDiametreExistant = new List<CsReglageCompteur>();
        Galatee.Silverlight.Shared.UcFichierJoint ctrl = null;
        public UcProcesVerbal()
        {
            InitializeComponent();
        }
        public UcProcesVerbal(int pIdDevis)
        {
            try
            {
                InitializeComponent();
                AfficherOuMasquer(tabItemDemandeur, false);
                AfficherOuMasquer(tabItemAppareils, false);
                AfficherOuMasquer(tabItemFournitures, false);
                //AfficherOuMasquer(tabPoseCompteur, false);
                
                btn_Rejeter.Visibility = System.Windows.Visibility.Collapsed;
                this.tabSaisieDepose.Visibility = System.Windows.Visibility.Collapsed;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                ChargerTypeDocument();
                ChargerPuissanceInstalle();
                ChargeDetailDEvis(pIdDevis);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
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
        private void RenseignerInformationsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                    ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == laDemande.LaDemande.FK_IDCENTRE);

                    Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE.ToString()) ? laDemande.LaDemande.ORDRE.ToString() : string.Empty;
                    Txt_CodeSite.Text = !string.IsNullOrEmpty(leCentre.CODESITE) ? leCentre.CODESITE : string.Empty;
                    Txt_CodeCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CENTRE) ? laDemande.LaDemande.CENTRE : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLECENTRE) ? laDemande.LaDemande.LIBELLECENTRE : string.Empty;
                    Txt_CodeProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.PRODUIT) ? laDemande.LaDemande.PRODUIT : string.Empty;
                    Txt_LibelleSite.Text = !string.IsNullOrEmpty(leCentre.LIBELLESITE) ? leCentre.LIBELLESITE : string.Empty;
                    Txt_LibelleProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLEPRODUIT) ? laDemande.LaDemande.LIBELLEPRODUIT : string.Empty;
                    Txt_LibelleTypeDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLETYPEDEMANDE) ? laDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;
                    txtPropriete.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;
                    Txt_Client.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CLIENT) ? laDemande.LaDemande.CLIENT : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE) ? laDemande.LaDemande.ORDRE : string.Empty;
                    this.Txt_EtapeCourante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Txt_EtapeSuivante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_SUIVANTE) ? laDemande.InfoDemande.ETAPE_SUIVANTE : string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsDemandeDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.LeClient != null && laDemande.Ag != null)
                {
                    Txt_NomClient.Text = !string.IsNullOrEmpty(laDemande.LeClient.NOMABON) ? laDemande.LeClient.NOMABON : string.Empty;
                    txt_Commune.Text = !string.IsNullOrEmpty(laDemande.Ag.COMMUNE) ? laDemande.Ag.COMMUNE : string.Empty;
                    txt_Quartier.Text = !string.IsNullOrEmpty(laDemande.Ag.QUARTIER) ? laDemande.Ag.QUARTIER : string.Empty;
                    txt_NumRue.Text = !string.IsNullOrEmpty(laDemande.Ag.RUE) ? laDemande.Ag.RUE : string.Empty;
                    txtAdresse.Text = !string.IsNullOrEmpty(laDemande.Branchement.ADRESSERESEAU) ? laDemande.Branchement.ADRESSERESEAU : string.Empty;
                    txtNumeroPiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMEROPIECEIDENTITE) ? laDemande.LeClient.NUMEROPIECEIDENTITE : string.Empty;
                    txt_Telephone.Text = !string.IsNullOrEmpty(laDemande.LeClient.TELEPHONE) ? laDemande.LeClient.TELEPHONE : string.Empty;
                    txt_NumLot.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;
                    Txt_LibelleCommune.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLECOMMUNE) ? laDemande.Ag.LIBELLECOMMUNE : string.Empty;
                    Txt_LibelleQuartier.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLEQUARTIER) ? laDemande.Ag.LIBELLEQUARTIER : string.Empty;
                    Txt_LibelleRue.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLERUE) ? laDemande.Ag.LIBELLERUE : string.Empty;
                    Txt_LibelleDiametre.Text = !string.IsNullOrEmpty(laDemande.Branchement.LIBELLETYPEBRANCHEMENT) ? laDemande.Branchement.LIBELLETYPEBRANCHEMENT : string.Empty;
                    Txt_LibelleCategorie.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLECATEGORIE) ? laDemande.LeClient.LIBELLECATEGORIE : string.Empty;
                    Txt_TypePiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLETYPEPIECE) ? laDemande.LeClient.LIBELLETYPEPIECE : string.Empty;
                    Txt_LibelleTournee.Text = !string.IsNullOrEmpty(laDemande.Ag.TOURNEE) ? laDemande.Ag.TOURNEE : string.Empty;

                    TxtLongitude.Text = !string.IsNullOrEmpty(laDemande.Branchement.LONGITUDE) ? laDemande.Branchement.LONGITUDE : string.Empty;
                    TxtLatitude.Text = !string.IsNullOrEmpty(laDemande.Branchement.LATITUDE) ? laDemande.Branchement.LATITUDE : string.Empty;


                    AfficherOuMasquer(tabItemDemandeur, true);
                }
                else
                    AfficherOuMasquer(tabItemDemandeur, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsAppareilsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.AppareilDevis != null && laDemande.AppareilDevis.Count > 0)
                {
                    dtgAppareils.ItemsSource = laDemande.AppareilDevis;
                    AfficherOuMasquer(tabItemAppareils, true);
                }
                else
                    AfficherOuMasquer(tabItemAppareils, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void RenseignerInformationsFournitureDevis(CsDemande lademande)
        {
            try
            {
                if (lademande != null && (lademande.EltDevis != null && lademande.EltDevis.Count > 0))
                {
                    dataGridForniture.ItemsSource = lademande.EltDevis;
                    this.Txt_TotalHt.Text = lademande.EltDevis.Sum(t => t.MONTANTHT).Value .ToString(SessionObject.FormatMontant);
                    this.Txt_TotalTva.Text = lademande.EltDevis.Sum(t => t.MONTANTTAXE).Value .ToString(SessionObject.FormatMontant);
                    this.Txt_TotalTtc.Text = lademande.EltDevis.Sum(t => t.MONTANTTTC).Value .ToString(SessionObject.FormatMontant);
                    AfficherOuMasquer(tabItemFournitures, true);
                }
                else
                    AfficherOuMasquer(tabItemFournitures, false);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerTypeCompteur()
        {
            try
            {
                if (SessionObject.LstTypeCompteur.Count != 0)
                    return;

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerTypeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTypeCompteur = args.Result;
                };
                service.ChargerTypeAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChargerPuissanceInstalle()
        {
            try
            {
                if (SessionObject.LstPuissanceInstalle != null && SessionObject.LstPuissanceInstalle.Count != 0)
                    return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceInstalleCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissanceInstalle = args.Result;
                };
                service.ChargerPuissanceInstalleAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {
            prgBar.Visibility = System.Windows.Visibility.Visible ;
            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            //client.ChargerDetailDemandeCompleted += (ssender, args) =>
            client.GetDevisByNumIdDevisCompleted += (ssender, args) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

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
                    RenseignerInformationsDevis(laDetailDemande);
                    RenseignerInformationsDemandeDevis(laDetailDemande);
                    RenseignerInformationsAppareilsDevis(laDetailDemande);
                    RenseignerInformationsFournitureDevis(laDetailDemande);
                    RenseignerInformationsDocumentScanne();
                    this.Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemandeSelect.NUMDEM) ? laDemandeSelect.NUMDEM : string.Empty;
                   
                    if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur)
                    {
                        this.tabSaisieDepose.Visibility = System.Windows.Visibility.Visible ;
                        this.tabC_Onglet.SelectedItem = this.tabSaisieDepose;
                        laDetailDemande.LeClient.PRODUIT = laDetailDemande.LaDemande.PRODUIT;
                        laDetailDemande.LeClient.FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT;
                        ChargerCompteurDeposer(laDetailDemande.LeClient );

                    }
                    else if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementProduit &&
                        laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.Prepaye)
                    {
                        this.tabSaisieDepose.Visibility = System.Windows.Visibility.Visible;
                        this.tabC_Onglet.SelectedItem = this.tabSaisieDepose;

                        CsClient leClient = Shared.ClasseMEthodeGenerique.RetourneCopyObjet<CsClient>(laDetailDemande.LeClient);
                        leClient.PRODUIT = SessionObject.Enumere.Electricite;
                        leClient.FK_IDPRODUIT = SessionObject.ListeDesProduit.FirstOrDefault(t => t.CODE == SessionObject.Enumere.Electricite).PK_ID;
                        leClient.PK_ID = laDetailDemande.LaDemande.FK_IDCLIENT.Value;
                        ChargerCompteurDeposer(leClient);
                    }
                    else
                    {
                        AfficherOuMasquer(tabPoseCompteur, true);
                        this.tabC_Onglet.SelectedItem = this.tabPoseCompteur;
                    }
                    if (laDetailDemande.LstEvenement != null && laDetailDemande.LstEvenement.Count != 0)
                    {
                        foreach (CsEvenement  item in laDetailDemande.LstEvenement)
                        {
                            CsCanalisation laCann = laDetailDemande.LstCanalistion.FirstOrDefault(t => t.POINT == item.POINT && string.IsNullOrEmpty(item.CASPRECEDENTEFACTURE) );
                            if (laCann != null)
                            {
                                laCann.INDEXEVT = item.INDEXEVT;
                                laCann.POSE  = item.DATEEVT;
                                laCann.PERIODE = item.PERIODE;
                            }
                        }
                    }
                    LoadListeOrganeScellable(laDetailDemande.LaDemande.FK_IDTYPEDEMANDE, laDetailDemande.LaDemande.FK_IDPRODUIT.Value  );
                    LoadCompteur(laDetailDemande.LstCanalistion);
                    if (laDetailDemande.LaDemande.PRODUIT  == SessionObject.Enumere.ElectriciteMT )
                        LoadListeScelleMt();
                    else 
                       LoadListeScelle();
                }
            };
            //client.ChargerDetailDemandeAsync(IdDemandeDevis,string.Empty );
            client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
        }

        private void RenseignerInformationsDocumentScanne()
        {
            try
            {
                #region DocumentScanne
                ctrl = new Galatee.Silverlight.Shared.UcFichierJoint(laDetailDemande.ObjetScanne, true);
                Vwb.Stretch = Stretch.None;
                Vwb.Child = ctrl;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void LoadListeOrganeScellable(int FK_IDTDEM,int FK_IDPRODUIT)
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.LoadListeOrganeScellableCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    ListeOrganeScellable = args.Result;
                    dg_composantScellable.ItemsSource = ListeOrganeScellable;
                    return;
                };
                service.LoadListeOrganeScellableAsync(FK_IDTDEM, FK_IDPRODUIT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void LoadListeScelle()
        {
            try
            {
             AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.LoadListeScellesCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == null)
                    {
                        //tabitemScelle.Visibility = System.Windows.Visibility.Collapsed;
                        return;
                    }
                    if (args.Result.Count == 0)
                    {
                        //tabitemScelle.Visibility = System.Windows.Visibility.Collapsed;
                        return;
                    }
                        
                    ListeScelle = args.Result;
                    if (rbt_NouveauScelle.IsChecked == true)
                        btn_ListScelle.IsEnabled = true;
                    return;
                };
                service.LoadListeScellesAsync(UserConnecte.PK_ID ,laDetailDemande.LaDemande.FK_IDTYPEDEMANDE);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadListeScelleMt()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.LoadListeScellesDemandeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == null)
                    {
                        //tabitemScelle.Visibility = System.Windows.Visibility.Collapsed;
                        return;
                    }
                    if (args.Result.Count == 0)
                    {
                        //tabitemScelle.Visibility = System.Windows.Visibility.Collapsed;
                        return;
                    }

                    ListeScelle = args.Result;
                    if (rbt_NouveauScelle.IsChecked == true)
                        btn_ListScelle.IsEnabled = true;
                    return;
                };
                service.LoadListeScellesDemandeAsync(UserConnecte.PK_ID, laDetailDemande.LaDemande.FK_IDTYPEDEMANDE, 5);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadCompteur(List<CsCanalisation> list)
        {
            if (((laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance || 
                laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance) && 
                (laDetailDemande.LaDemande.ISCHANGECOMPTEUR == true || laDetailDemande.LaDemande.ISMETREAFAIRE ==true ))
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonementExtention 
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementMt 
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementProduit 
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Reabonnement )
            {
                //if (list.First().PRODUIT == SessionObject.Enumere.ElectriciteMT) //ZEG 25/09/2017
                if (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                {
                    DataReferenceManager.CodificationCompteurMt(list);
                    string typeComptage =SessionObject.LstTypeComptage.FirstOrDefault(t=>t.CODE == laDetailDemande.LaDemande.TYPECOMPTAGE )!=null ? 
                        SessionObject.LstTypeComptage.FirstOrDefault(t=>t.CODE == laDetailDemande.LaDemande.TYPECOMPTAGE ).LIBELLE : string.Empty ;
                    this.dg_compteur.Columns[2].Header = "TYPE COMPTAGE";
                    list.ForEach(t => t.LIBELLEREGLAGECOMPTEUR = typeComptage);
                    list.ForEach(t => t.INDEXEVT = 0);
                }
                dg_compteur.ItemsSource = list ;
                List<CsCanalisation> lstNouvCompt = list ;
                if (lstNouvCompt != null && lstNouvCompt.Count != 0)
                    lstNouvCompt.ForEach(t => t.CAS = SessionObject.Enumere.CasPoseCompteur);

                //dg_AncienCompteur.IsEnabled = false;
                //this.dg_AncienCompteur.IsReadOnly = true;
                //TxtperiodePose.Text = string.IsNullOrEmpty(lstNouvCompt.First().PERIODE) ? string.Empty : Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(lstNouvCompt.First().PERIODE);
            }
        }

        private void ChargerCompteurADeposer(CsDemandeBase laDemande)
        {
            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.RetourneAncienCompteurCompleted += (ssender, args) =>
            {
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
                    List<CsCanalisation> lesAncienCompteur = new List<CsCanalisation>();
                    lesAncienCompteur = args.Result;
                    lesAncienCompteur.ForEach(t => t.ANCCOMPTEUR  = "000");
                    if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Etalonage ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementProduit  ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance  ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur  )
                    {
                        if (lesAncienCompteur.Count == 0)
                        {
                            Message.ShowInformation("Ce client n'a pas de compteur", "Resiliation");
                            return;
                        }
                        dg_AncienCompteur.ItemsSource = lesAncienCompteur;
                        List<CsCanalisation> lstAncCompt = lesAncienCompteur;
                        if (lstAncCompt != null && lstAncCompt.Count != 0)
                            lstAncCompt.ForEach(t => t.CAS = SessionObject.Enumere.CasDeposeCompteur);

                        TxtperiodeDepose.Text = string.IsNullOrEmpty(lstAncCompt.First().PERIODE) ? string.Empty : Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(lstAncCompt.First().PERIODE);
                    }
                }
            };
            client.RetourneAncienCompteurAsync(laDemande);
        }

        private void ChargerCompteurDeposer(CsClient  leClient)
        {
            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.RetourneEvenementClientCompleted += (ssender, args) =>
            {
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
                    List<CsEvenement> lesAncienCompteur = new List<CsEvenement>();
                    lesAncienCompteur = args.Result;
                    if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Etalonage ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementProduit ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur)
                    {
                        if (lesAncienCompteur.Count == 0)
                        {
                            Message.ShowInformation("Ce client n'a pas de compteur", "Resiliation");
                            return;
                        }
                        lesAncienCompteur.ForEach(i => i.INDEXEVT = i.INDEXPRECEDENTEFACTURE);
                        dg_AncienCompteur.ItemsSource = lesAncienCompteur;
                        List<CsEvenement> lstAncCompt = lesAncienCompteur;
                        if (lstAncCompt != null && lstAncCompt.Count != 0)
                            lstAncCompt.ForEach(t => t.CAS = SessionObject.Enumere.CasDeposeCompteur);

                        TxtperiodeDepose.Text = string.IsNullOrEmpty(lstAncCompt.First().PERIODE) ? string.Empty : Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(lstAncCompt.First().PERIODE);
                    }
                }
            };
            client.RetourneEvenementClientAsync(leClient);
        }


        //private void EnregisterOuTransmetre(bool IsEnregister)
        //{
        //    try
        //    {
        //        laDetailDemande.LstCanalistion = new List<CsCanalisation>();
        //        laDetailDemande.LstEvenement = new List<CsEvenement>();

        //        #region Gestion AD:/DP
        //        if ((laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance || 
        //            laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance) && 
        //            laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementProduit  && 
        //            laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur)
        //        {
        //            //int IndexDepose = 0;
        //            var Canalisation = ((CsCanalisation)dg_AncienCompteur.SelectedItem);
        //            if (Canalisation.INDEXEVT == null || Canalisation.INDEXEVT < 0)
        //                throw new Exception("Index de dépose incorrecte");

        //            if (!string.IsNullOrEmpty(this.TxtperiodeDepose.Text))
        //                throw new Exception("Saisir la période de dépose");
        //        }

        //        #endregion
        //        if (dg_compteur.ItemsSource != null )
        //        foreach (var leCompteur in ((List<CsCanalisation>)dg_compteur.ItemsSource).ToList())
        //        {
                   
        //            CsEvenement leEvtPose = new CsEvenement();
        //            leEvtPose.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
        //            leEvtPose.CENTRE = laDetailDemande.LaDemande.CENTRE;
        //            leEvtPose.CLIENT = laDetailDemande.LaDemande.CLIENT;
        //            leEvtPose.ORDRE = laDetailDemande.LaDemande.ORDRE;
        //            leEvtPose.PRODUIT = laDetailDemande.LaDemande.PRODUIT;
        //            if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
        //            {
        //                leCompteur.POINT = 1;
        //                leEvtPose.POINT = 1;
        //            }
        //            else
        //                leEvtPose.POINT = leCompteur.POINT;

        //            leEvtPose.FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT.Value;
        //            leEvtPose.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
        //            leEvtPose.MATRICULE = laDetailDemande.LaDemande.MATRICULE;

        //            leEvtPose.REGLAGECOMPTEUR = leCompteur.REGLAGECOMPTEUR;
        //            leEvtPose.TYPECOMPTEUR = leCompteur.TYPECOMPTEUR;
        //            leEvtPose.COMPTEUR = leCompteur.NUMERO;
        //            if (laDetailDemande.LaDemande.TYPEDEMANDE != SessionObject.Enumere.BranchementAbonnementEp)
        //            {
        //                leEvtPose.TYPETARIF = laDetailDemande.Abonne.TYPETARIF;
        //                leEvtPose.PUISSANCE = laDetailDemande.Abonne.PUISSANCE;
        //            }
        //            leEvtPose.CATEGORIE  = laDetailDemande.LeClient.CATEGORIE ;
        //            leEvtPose.USERCREATION = UserConnecte.matricule;
        //            leEvtPose.USERMODIFICATION = UserConnecte.matricule;
        //            leEvtPose.DATECREATION = System.DateTime.Now;
        //            leEvtPose.DATEMODIFICATION = System.DateTime.Now;
        //            leEvtPose.CAS = leCompteur.CAS;
        //            leEvtPose.FK_IDCANALISATION = leCompteur.PK_ID;
        //            leEvtPose.ISCONSOSEULE = this.Chk_ConsoSeul.IsChecked.Value ;
        //            leEvtPose.FK_IDABON  = null  ;
                    
        //            leEvtPose.CODEEVT = SessionObject.Enumere.EvenementCodeNormale;
        //            leEvtPose.STATUS = SessionObject.Enumere.EvenementReleve ;
        //            leEvtPose.DATEEVT = leEvtPose.CAS == SessionObject.Enumere.CasPoseCompteur ? DtpPose : DtpDePose;
        //            leEvtPose.INDEXEVT = leCompteur.INDEXEVT;
        //            leEvtPose.PERIODE = leEvtPose.CAS == SessionObject.Enumere.CasPoseCompteur ? Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(TxtperiodePose.Text) : Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(TxtperiodeDepose.Text);
        //            leEvtPose.TYPECOMPTAGE = laDetailDemande.Branchement.TYPECOMPTAGE;

        //            leEvtPose.COEFK1 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE1;
        //            leEvtPose.COEFK2 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE2;


        //            if (laDetailDemande.LstEvenement != null && laDetailDemande.LstEvenement.Count != 0)
        //            {
        //                CsEvenement _LaCan = laDetailDemande.LstEvenement.FirstOrDefault(p => p.CAS == leEvtPose.CAS && p.POINT == leEvtPose.POINT);
        //                if (_LaCan != null)
        //                {
        //                    _LaCan.DATEEVT = leEvtPose.DATEEVT;
        //                    _LaCan.INDEXEVT = leCompteur.INDEXEVT;
        //                    _LaCan.PERIODE = leEvtPose.PERIODE;                            
        //                }
        //                else
        //                    laDetailDemande.LstEvenement.Add(leEvtPose);
        //            }
        //            else
        //            {
        //                laDetailDemande.LstEvenement = new List<CsEvenement>();
        //                laDetailDemande.LstEvenement.Add(leEvtPose);
        //            }

        //            leCompteur.USERCREATION = UserConnecte.matricule;
        //            leCompteur.USERMODIFICATION = UserConnecte.matricule;
        //            leCompteur.DATECREATION = System.DateTime.Now;
        //            leCompteur.DATEMODIFICATION = System.DateTime.Now;
        //            laDetailDemande.LstCanalistion.Add(leCompteur);
        //        }
        //        if (dg_AncienCompteur.ItemsSource != null)
        //        foreach (var leEvtPose in ((List<CsEvenement>)dg_AncienCompteur.ItemsSource).ToList())
        //        {
        //            leEvtPose.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
        //            leEvtPose.CONSO = leEvtPose.INDEXEVT - leEvtPose.INDEXPRECEDENTEFACTURE;
        //            leEvtPose.PERIODE = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(TxtperiodeDepose.Text);
        //            leEvtPose.USERCREATION = UserConnecte.matricule;
        //            leEvtPose.USERMODIFICATION = UserConnecte.matricule;
        //            leEvtPose.DATECREATION = System.DateTime.Now;
        //            leEvtPose.DATEMODIFICATION = System.DateTime.Now;
        //            if (leEvtPose.DATERELEVEPRECEDENTEFACTURE > leEvtPose.DATEEVT)
        //            {
        //                Message.ShowInformation("La date de dernière facturation supperieur a la date saisie", "Information");
        //                if (laDetailDemande.LstCanalistion.Count != 0) laDetailDemande.LstCanalistion.Clear();
        //                this.CancelButton.IsEnabled = true;
        //                this.btn_Transmetre.IsEnabled = true;
        //                this.btn_Rejeter.IsEnabled = true;
        //                return;
        //            }
        //            laDetailDemande.LstEvenement.Add(leEvtPose);
        //        }
        //        if (dg_Scellage.ItemsSource != null)
        //        {
        //            ObjDOCUMENTSCANNE leDoc = new ObjDOCUMENTSCANNE();
        //            if (lnkLetter.Tag != null)
        //                leDoc = SaveFile((byte[])lnkLetter.Tag, 1, null);

        //            laDetailDemande.LstOrganeScelleDemande = new List<CsOrganeScelleDemande>();
        //            laDetailDemande.LstOrganeScelleDemande = (List<CsOrganeScelleDemande>)dg_Scellage.ItemsSource;
        //            laDetailDemande.LstOrganeScelleDemande.First().CERTIFICAT = leDoc.CONTENU;
        //        }
        //        if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation  )
        //            laDetailDemande.LstCanalistion = null;
        //        laDetailDemande.EltDevis = null;
        //        laDetailDemande.LstControleTvx = null;
        //        laDetailDemande.OrdreTravail  = null;

        //        AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Accueil"));
        //        clientDevis.ValiderDemandeCompleted += (ss, b) =>
        //        {
        //            this.btn_Transmetre.IsEnabled = false;
        //            if (b.Cancelled || b.Error != null)
        //            {
        //                string error = b.Error.Message;
        //                Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
        //                return;
        //            }
        //            if (IsEnregister &&
        //                laDetailDemande.LaDemande.TYPEDEMANDE != SessionObject.Enumere.Resiliation &&
        //                laDetailDemande.LaDemande.TYPEDEMANDE != SessionObject.Enumere.BranchementAbonnementEp 
        //            )
        //            {
        //                AcceuilServiceClient clientDeviss = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
        //                clientDeviss.ClotureValiderDemandeCompleted += (sss, bd) =>
        //                {
        //                    if (bd.Cancelled || bd.Error != null)
        //                    {
        //                        string error = bd.Error.Message;
        //                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
        //                        return;
        //                    }
        //                    //if (bd.Result == true)
        //                    //{
        //                    //    List<string> codes = new List<string>();
        //                    //    codes.Add(laDetailDemande.InfoDemande.CODE);
        //                    //    Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
        //                    //}
        //                    if (bd.Result == true)
        //                    {
        //                        //Message.ShowInformation(Silverlight.Resources.Accueil.Langue.MsgDemandeTransmise, "Demande");
        //                        Message.ShowInformation("Demande Transmise", "Demande");

        //                        this.DialogResult = true;
        //                    }
        //                    else
        //                    {
        //                        Message.ShowError("Erreur a la cloture de la demande", Silverlight.Resources.Devis.Languages.txtDevis);
        //                    }
        //                };
        //                clientDeviss.ClotureValiderDemandeAsync(laDetailDemande);
        //            }
        //            else
        //            {
        //                //List<string> codes = new List<string>();
        //                //codes.Add(laDetailDemande.InfoDemande.CODE);
        //                //Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
        //                if (b.Result == true)
        //                {
        //                    //Message.ShowInformation(Silverlight.Resources.Accueil.Langue.MsgDemandeTransmise, "Demande");
        //                    Message.ShowInformation("Demande Transmise", "Demande");
        //                    this.DialogResult = true;
        //                }
        //            }
        //        };
        //        clientDevis.ValiderDemandeAsync(laDetailDemande,true);

        //        this.DialogResult = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        this.btn_Transmetre.IsEnabled = true ;
        //        Message.Show(ex.Message, Languages.txtDevis);
        //    }
        //}






        private void EnregisterOuTransmetre(bool IsEnregister)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                laDetailDemande.LstCanalistion = new List<CsCanalisation>();
                laDetailDemande.LstEvenement = new List<CsEvenement>();
                laDetailDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                laDetailDemande.LaDemande.DATEMODIFICATION = System.DateTime.Today;
                

                #region Gestion AD:/DP
                if ((laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance) &&
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementProduit &&
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur)
                {
                    //int IndexDepose = 0;
                    var Canalisation = ((CsCanalisation)dg_AncienCompteur.SelectedItem);
                    if (Canalisation.INDEXEVT == null || Canalisation.INDEXEVT < 0)
                        throw new Exception("Index de dépose incorrecte");

                    if (!string.IsNullOrEmpty(this.TxtperiodeDepose.Text))
                        throw new Exception("Saisir la période de dépose");
                }

                #endregion
                if (dg_compteur.ItemsSource != null)
                    foreach (var leCompteur in ((List<CsCanalisation>)dg_compteur.ItemsSource).ToList())
                    {

                        CsEvenement leEvtPose = new CsEvenement();
                        leEvtPose.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
                        leEvtPose.FK_IDDEMANDE  = laDetailDemande.LaDemande.PK_ID ;
                        leEvtPose.CENTRE = laDetailDemande.LaDemande.CENTRE;
                        leEvtPose.CLIENT = laDetailDemande.LaDemande.CLIENT;
                        leEvtPose.ORDRE = laDetailDemande.LaDemande.ORDRE;
                        leEvtPose.PRODUIT = laDetailDemande.LaDemande.PRODUIT;
                        if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        {
                            leCompteur.POINT = 1;
                            leEvtPose.POINT = 1;
                        }
                        else
                            leEvtPose.POINT = leCompteur.POINT;

                        leEvtPose.FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT.Value;
                        leEvtPose.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                        leEvtPose.MATRICULE = laDetailDemande.LaDemande.MATRICULE;

                        leEvtPose.REGLAGECOMPTEUR = leCompteur.REGLAGECOMPTEUR;
                        leEvtPose.TYPECOMPTEUR = leCompteur.TYPECOMPTEUR;
                        leEvtPose.COMPTEUR = leCompteur.NUMERO;
                        if (laDetailDemande.LaDemande.TYPEDEMANDE != SessionObject.Enumere.BranchementAbonnementEp)
                        {
                            leEvtPose.TYPETARIF = laDetailDemande.Abonne.TYPETARIF;
                            leEvtPose.PUISSANCE = laDetailDemande.Abonne.PUISSANCE;
                        }
                        leEvtPose.CATEGORIE = laDetailDemande.LeClient.CATEGORIE;
                        leEvtPose.USERCREATION = UserConnecte.matricule;
                        leEvtPose.USERMODIFICATION = UserConnecte.matricule;
                        leEvtPose.CAS = SessionObject.Enumere.CasPoseCompteur ;

                        leEvtPose.CODEEVT = SessionObject.Enumere.EvenementCodeNormale;
                        leEvtPose.STATUS = SessionObject.Enumere.EvenementReleve;
                        leEvtPose.DATEEVT = leEvtPose.CAS == SessionObject.Enumere.CasPoseCompteur ? DtpPose : DtpDePose;
                        leEvtPose.INDEXEVT = leCompteur.INDEXEVT;
                        leEvtPose.PERIODE = leEvtPose.CAS == SessionObject.Enumere.CasPoseCompteur ? Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(TxtperiodePose.Text) : Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(TxtperiodeDepose.Text);
                        leEvtPose.TYPECOMPTAGE = laDetailDemande.Branchement.TYPECOMPTAGE;

                        leEvtPose.COEFK1 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE1;
                        leEvtPose.COEFK2 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE2;


                        if (laDetailDemande.LstEvenement != null && laDetailDemande.LstEvenement.Count != 0)
                        {
                            CsEvenement _LaCan = laDetailDemande.LstEvenement.FirstOrDefault(p => p.CAS == leEvtPose.CAS && p.POINT == leEvtPose.POINT);
                            if (_LaCan != null)
                            {
                                _LaCan.DATEEVT = leEvtPose.DATEEVT;
                                _LaCan.INDEXEVT = leCompteur.INDEXEVT;
                                _LaCan.PERIODE = leEvtPose.PERIODE;
                            }
                            else
                                laDetailDemande.LstEvenement.Add(leEvtPose);
                        }
                        else
                        {
                            laDetailDemande.LstEvenement = new List<CsEvenement>();
                            laDetailDemande.LstEvenement.Add(leEvtPose);
                        }

                        leCompteur.USERCREATION = UserConnecte.matricule;
                        leCompteur.USERMODIFICATION = UserConnecte.matricule;
                        laDetailDemande.LstCanalistion.Add(leCompteur);
                    }
                if (dg_AncienCompteur.ItemsSource != null)
                    foreach (var leEvtPose in ((List<CsEvenement>)dg_AncienCompteur.ItemsSource).ToList())
                    {
                        leEvtPose.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
                        leEvtPose.CONSO = leEvtPose.INDEXEVT - leEvtPose.INDEXPRECEDENTEFACTURE;
                        leEvtPose.PERIODE = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(TxtperiodeDepose.Text);
                        leEvtPose.USERCREATION = UserConnecte.matricule;
                        leEvtPose.USERMODIFICATION = UserConnecte.matricule;
                        leEvtPose.ISCONSOSEULE = this.Chk_ConsoSeul.IsChecked.Value;
                        if (leEvtPose.DATERELEVEPRECEDENTEFACTURE > leEvtPose.DATEEVT)
                        {
                            Message.ShowInformation("La date de dernière facturation supérieure à la date saisie", "Information");
                            if (laDetailDemande.LstCanalistion.Count != 0) laDetailDemande.LstCanalistion.Clear();
                            this.CancelButton.IsEnabled = true;
                            this.btn_Transmetre.IsEnabled = true;
                            this.btn_Rejeter.IsEnabled = true;
                            return;
                        }
                        laDetailDemande.LstEvenement.Add(leEvtPose);
                    }
                if (dg_Scellage.ItemsSource != null)
                {
                    ObjDOCUMENTSCANNE leDoc = new ObjDOCUMENTSCANNE();
                    if (lnkLetter.Tag != null)
                    {
                        leDoc = SaveFile((byte[])lnkLetter.Tag, 1, null);
                        //leDoc.CODETYPEDOC = LstTypeDocument.First(d => d.CODE == "000").CODE;
                        //leDoc.FK_IDTYPEDOCUMENT = LstTypeDocument.First(d => d.CODE == "000").PK_ID;
                        //leDoc.NOMDOCUMENT = LstTypeDocument.First(d => d.CODE == "000").LIBELLE;
                        //laDetailDemande.ObjetScanne.Add(leDoc);
                    }



                    laDetailDemande.LstOrganeScelleDemande = new List<CsOrganeScelleDemande>();
                    laDetailDemande.LstOrganeScelleDemande = (List<CsOrganeScelleDemande>)dg_Scellage.ItemsSource;
                    laDetailDemande.LstOrganeScelleDemande.First().CERTIFICAT = leDoc.CONTENU;
                }

                laDetailDemande.LaDemande.MATRICULE = UserConnecte.matricule;

                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.ProcesVerbalCompleted += (ss, b) =>
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (string.IsNullOrEmpty(b.Result))
                    {
                        Message.ShowInformation("Demande cloturée avec succès", "Demande");
                        this.DialogResult = true;
                    }
                    else
                    {
                        Message.ShowError(b.Result, "Demande");
                        this.CancelButton.IsEnabled = true;
                        this.btn_Transmetre.IsEnabled = true;
                        this.btn_Rejeter.IsEnabled = true;
                    }
                };
                clientDevis.ProcesVerbalAsync(laDetailDemande,true );

            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                this.btn_Transmetre.IsEnabled = true;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }





        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void VerifierSaisie()
        {
            try
            {
                if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur ||
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementProduit)
                {
                    if (string.IsNullOrEmpty(this.dtpDepose.Text))
                        throw new Exception("Veuillez indiquer la date depose compteur");
                    if (string.IsNullOrEmpty(this.dtpPose.Text))
                        throw new Exception("Veuillez indiquer la date pose du nouveau compteur");

                    if (dg_AncienCompteur.ItemsSource == null)
                        throw new Exception("Veuillez saisir l'index de depose de tous les points de lecture.");

                    List<CsEvenement> lesCompteur = (List<CsEvenement>)dg_AncienCompteur.ItemsSource;
                    foreach (CsEvenement st in lesCompteur)
                    {
                        if (st.INDEXEVT == null)
                        {
                            throw new Exception("Veuillez saisir l'index de depose de tous les points de lecture.");
                        }
                    }


                    if (dg_compteur.ItemsSource == null)
                        throw new Exception("Veuillez saisir l'index de pose de tous les points de lecture.");

                    List<CsCanalisation> lesComp = (List<CsCanalisation>)dg_compteur.ItemsSource;
                    foreach (CsCanalisation st in lesComp)
                    {
                        if (st.INDEXEVT == null)
                            throw new Exception("Veuillez saisir l'index de pose de tous les points de lecture.");
                    }
                }


                if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement)
                { 
                  if (this.dtp_DateDepose.Value ==null )
                      throw new Exception("Sélectionnez la date de pose compteur");
                  if (string.IsNullOrEmpty( this.TxtperiodePose.Text))
                      throw new Exception("Saisir la periode facturation");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DtpDebutTravaux_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //// La date de début de travaux doit être > à la date de fin
                //if (DtpFinTravaux.SelectedDate != null && DtpFinTravaux.SelectedDate.Value != null)
                //{
                //    if (DtpDebutTravaux.SelectedDate != null && DtpDebutTravaux.SelectedDate.Value != null)
                //    {
                //        if (DtpDebutTravaux.SelectedDate.Value.Date > DtpFinTravaux.SelectedDate.Value.Date)
                //            throw new Exception("La date de début de travaux ne peut pas être supérieur à la date de fin travaux !");
                //    }
                //}

                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void DtpFinTravaux_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // La date de début de travaux doit être < à la date de fin
                //if (DtpDebutTravaux.SelectedDate != null && DtpDebutTravaux.SelectedDate.Value != null)
                //{
                //    if (DtpFinTravaux.SelectedDate != null && DtpFinTravaux.SelectedDate.Value != null)
                //    {
                //        if (DtpFinTravaux.SelectedDate.Value.Date < DtpDebutTravaux.SelectedDate.Value.Date)
                //            throw new Exception("La date de début de travaux ne peut pas être inférieur à la date de fin travaux !");
                //    }
                //}
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void TxtCommentaire_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void TxtNumeCompteur_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void TxtIndexDePose_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void DtpAnneeFabrication_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void DtpDatePose_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //if (DtpDebutTravaux.SelectedDate != null && DtpDebutTravaux.SelectedDate.Value != null)
                //{
                //    if (DtpDatePose.SelectedDate != null && DtpDatePose.SelectedDate.Value != null)
                //    {
                //        if (DtpDatePose.SelectedDate.Value.Date < DtpDebutTravaux.SelectedDate.Value.Date)
                //            throw new Exception("La date de pose de compteur ne peut pas être inférieur à la date de début de travaux !");
                //    }
                //}

                //if (DtpFinTravaux.SelectedDate != null && DtpFinTravaux.SelectedDate.Value != null)
                //{
                //    if (DtpDatePose.SelectedDate != null && DtpDatePose.SelectedDate.Value != null)
                //    {
                //        if (DtpDatePose.SelectedDate.Value.Date > DtpFinTravaux.SelectedDate.Value.Date)
                //            throw new Exception("La date de pose de compteur ne peut pas être supérieur à la date de fin de travaux !");
                //    }
                //}

                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void DtpDebutTravaux_CalendarClosed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DtpFinTravaux.SelectedDate != null && DtpFinTravaux.SelectedDate.Value != null)
                {
                    if (DtpDebutTravaux.SelectedDate != null && DtpDebutTravaux.SelectedDate.Value != null)
                    {
                        if (DtpDebutTravaux.SelectedDate.Value.Date > DtpFinTravaux.SelectedDate.Value.Date)
                            throw new Exception("La date de début de travaux ne peut pas être supérieur à la date de fin travaux !");
                    }
                }
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                DtpDebutTravaux.ClearValue(DatePicker.SelectedDateProperty);
                DtpDebutTravaux.Focus();
            }
        }

        private void DtpFinTravaux_CalendarClosed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DtpDebutTravaux.SelectedDate != null && DtpDebutTravaux.SelectedDate.Value != null)
                {
                    if (DtpFinTravaux.SelectedDate != null && DtpFinTravaux.SelectedDate.Value != null)
                    {
                        if (DtpFinTravaux.SelectedDate.Value.Date < DtpDebutTravaux.SelectedDate.Value.Date)
                            throw new Exception("La date de début de travaux ne peut pas être inférieur à la date de fin travaux !");
                    }
                }
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                DtpFinTravaux.ClearValue(DatePicker.SelectedDateProperty);
                DtpFinTravaux.Focus();
            }
        }

        private void Cbo_DiametreCompteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void rbt_RuptureSimple_Checked(object sender, RoutedEventArgs e)
        {
            txt_NumScelleRompu.IsReadOnly = false;
            txt_NumNouveauScelle.IsReadOnly = true;
        }

        private void rbt_RuptureSimple_Unchecked_1(object sender, RoutedEventArgs e)
        {
            txt_NumScelleRompu.IsReadOnly = true;
        }

        private void rbt_NouveauScelle_Checked_1(object sender, RoutedEventArgs e)
        {
            txt_NumNouveauScelle.IsReadOnly = false;
            txt_NumScelleRompu.IsReadOnly = true;
            btn_ListScelle.IsEnabled = true;
        }

        private void rbt_NouveauScelle_Unchecked_1(object sender, RoutedEventArgs e)
        {
            txt_NumNouveauScelle.IsReadOnly = true;
            btn_ListScelle.IsEnabled = false;
        }

        private void rbt_AuneAction_Checked(object sender, RoutedEventArgs e)
        {
            txt_NumNouveauScelle.IsReadOnly = true;
            txt_NumScelleRompu.IsReadOnly = true;
        }

        private void btn_Ajout_Click(object sender, RoutedEventArgs e)
        {
            if (dg_composantScellable.SelectedItem != null)
            {
                int nombre = 0;
                int.TryParse(txt_NombreScelle.Text, out nombre);
                CsOrganeScelleDemande OrganeScelleDemande = new CsOrganeScelleDemande();
                OrganeScelleDemande.FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID;
                OrganeScelleDemande.FK_IDORGANE_SCELLABLE = ((CsOrganeScellable)dg_composantScellable.SelectedItem).PK_ID;
                OrganeScelleDemande.LIBELLEORGANE_SCELLABLE = ListeOrganeScellable.FirstOrDefault(o => o.PK_ID == OrganeScelleDemande.FK_IDORGANE_SCELLABLE).LIBELLE;
                OrganeScelleDemande.NOMBRE = (nombre != 0 ? nombre : 0);

                if (rbt_RuptureSimple.IsChecked == true)
                {
                    if (!string.IsNullOrWhiteSpace(txt_NumScelleRompu.Text))
                    {
                        OrganeScelleDemande.NUM_SCELLE = txt_NumScelleRompu.Text;
                        OrganeScelleDemande.IDCOULEUR =txt_NumNouveauScelle.Tag != null ? ((CsScelle) txt_NumNouveauScelle.Tag).Couleur_Scelle :0 ;
                    }
                    else
                    {
                        Message.ShowWarning("Veuillez saisir le numero du scelle rompu", "Information");
                        return;
                    }
                        //OrganeScelleDemande.NUM_SCELLE = !string.IsNullOrWhiteSpace(txt_NumScelleRompu.Text) ? txt_NumScelleRompu.Text : string.Empty;
                }
                if (rbt_NouveauScelle.IsChecked == true)
                {
                    if (!string.IsNullOrWhiteSpace(txt_NumNouveauScelle.Text))
                    {
                        OrganeScelleDemande.NUM_SCELLE = txt_NumNouveauScelle.Text;
                        OrganeScelleDemande.IDCOULEUR =txt_NumNouveauScelle.Tag != null ? ((CsScelle) txt_NumNouveauScelle.Tag).Couleur_Scelle : 0 ;
                    }
                    else
                    {
                        Message.ShowWarning("Veuillez saisir le numero du nouveau scelle ", "Information");
                        return;
                    }
                }
                if (rbt_AuneAction.IsChecked == true)
                {
                    OrganeScelleDemande.NUM_SCELLE = string.Empty;
                }
                if (dg_Scellage.ItemsSource != null)
                {
                    var ScelleDejaLie = ((List<CsOrganeScelleDemande>)dg_Scellage.ItemsSource).Select(o => o.NUM_SCELLE);
                    if (!ScelleDejaLie.Contains(txt_NumScelleRompu.Text))
                    {
                        ListeOrganeScelleDemande.Add(OrganeScelleDemande);
                        dg_Scellage.ItemsSource = ListeOrganeScelleDemande.OrderBy(c => c.NOMBRE).ToList();
                    }
                }
                else
                {
                    ListeOrganeScelleDemande.Add(OrganeScelleDemande);
                    dg_Scellage.ItemsSource = ListeOrganeScelleDemande.OrderBy(c => c.NOMBRE).ToList();
                }
                this.txt_NumNouveauScelle.Text = string.Empty;
                this.txt_NumNouveauScelle.Tag = null  ;
            }
            else
            {
                Message.ShowWarning("Veuillez sélectionner un composant", "Information");
            }
        }

        private void btn_Supprimer_Click_1(object sender, RoutedEventArgs e)
        {
            if (dg_Scellage.SelectedItem != null)
            {
                CsOrganeScelleDemande OrganeScelleDemande = (CsOrganeScelleDemande)dg_Scellage.SelectedItem;
                ListeOrganeScelleDemande.Remove(OrganeScelleDemande);
                dg_Scellage.ItemsSource = ListeOrganeScelleDemande.OrderBy(c => c.NOMBRE).ToList();
            }
        }

        private void btn_ListScelle_Click(object sender, RoutedEventArgs e)
        {
            this.btn_ListScelle.IsEnabled = false;
            if (ListeScelle.Count != 0)
            {
                var ListeScelleValide = ListeScelle.Where(s => !ListeOrganeScelleDemande.Select(o => o.NUM_SCELLE).Contains(s.Numero_Scelle )).OrderBy(u=>u.Numero_Scelle ).ToList();
                if (ListeScelleValide!=null && ListeScelleValide.Count()>0)
                {
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("Numero_Scelle", "Numero_Scelle");

                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(ListeScelleValide.ToList());
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Scelle");
                    ctrl.Closed += new EventHandler(galatee_OkClickedBtnScelle);
                    ctrl.Show();
                }
                else
                {
                    Message.ShowInformation("Plus de scellé disponible en stock veuillez vous approvisionner", "Information");
                }
            }
            else
            {
                Message.ShowInformation("Vous n'avez plus de scellé disponible", "Information");
            }

        }

        void galatee_OkClickedBtnScelle(object sender, EventArgs e)
        {
            this.btn_ListScelle.IsEnabled = true;
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsScelle Scelle = (CsScelle)ctrs.MyObject;
                this.txt_NumNouveauScelle.Text = Scelle.Numero_Scelle;
                this.txt_NumNouveauScelle.Tag = Scelle;

            }

        }

        private void dg_compteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dg_compteur.SelectedItem != null)
            {
                //var Canalisation = ((CsCanalisation)dg_compteur.SelectedItem);
                //if(Canalisation.ANNEEFAB!=null)
                //    DtpAnneeFabrication.SelectedDate = new DateTime(int.Parse(Canalisation.ANNEEFAB), 1, 1);
                //Cbo_CategorieCompteur.SelectedItem = _listeCanalisation.FirstOrDefault(c => c.PK_ID == Canalisation.FK_IDTYPECOMPTEUR);
                //Cbo_MarqueCompteur.SelectedItem = _listeMarqueCpt.FirstOrDefault(m => m.PK_ID == Canalisation.FK_IDMARQUECOMPTEUR);
                //Cbo_DiametreCompteur.SelectedItem = _listeDesDiametreExistant.FirstOrDefault(d => d.PK_ID == Canalisation.FK_IDDIAMETRECOMPTEUR);
            }
        }

        private void btn_Transmetre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.CancelButton.IsEnabled = false;
                this.btn_Transmetre.IsEnabled = false;
                this.btn_Rejeter.IsEnabled = false;
                VerifierSaisie();
                EnregisterOuTransmetre(true);
            }
            catch (Exception ex)
            {
              Message.ShowInformation(ex.Message ,"Accueil");
              this.CancelButton.IsEnabled = true;
              this.btn_Transmetre.IsEnabled = true;
              this.btn_Rejeter.IsEnabled = true;
            }
        }

        private void btn_Rejeter_Click(object sender, RoutedEventArgs e)
        {
            laDetailDemande.LaDemande.ISDEVISCOMPLEMENTAIRE = true;
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
        }

        private void lnkLetter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lnkLetter.Tag == null)
                {
                    var openDialog = new OpenFileDialog();
                    //openDialog.Filter = "Text Files (*.txt)|*.txt";
                    openDialog.Filter = "Image files (*.jpg; *.jpeg; *.png; *.bmp) | *.jpg; *.jpeg; *.png; *.bmp";
                    openDialog.Multiselect = true;
                    bool? userClickedOK = openDialog.ShowDialog();
                    if (userClickedOK == true)
                    {
                        if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                        {
                            FileStream stream = openDialog.File.OpenRead();
                            var memoryStream = new MemoryStream();
                            stream.CopyTo(memoryStream);
                            lnkLetter.Tag = memoryStream.GetBuffer();
                            var formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                            formScanne.Closed += new EventHandler(GetInformationFromChildWindowImageAutorisation);
                            formScanne.Show();
                        }
                    }
                }

                else
                {
                    MemoryStream memoryStream = new MemoryStream(lnkLetter.Tag as byte[]);
                    var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                    ucImageScanne.Show();
                }

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
        private ObjDOCUMENTSCANNE SaveFile(byte[] pStream, int pTypeDocument, ObjDOCUMENTSCANNE pDocumentScane)
        {
            try
            {
                //Récupération du contenu.
                if (pDocumentScane == null)
                {
                    pDocumentScane = new ObjDOCUMENTSCANNE { CONTENU = pStream, PK_ID = Guid.NewGuid(), DATECREATION = DateTime.Now, USERCREATION = UserConnecte.matricule };
                    pDocumentScane.OriginalPK_ID = pDocumentScane.PK_ID;
                    pDocumentScane.ISNEW = true;
                }
                else
                    pDocumentScane.CONTENU = pStream;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
            return pDocumentScane;
        }



        private void ChargerTypeDocument()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerTypeDocumentCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    LstTypeDocument = args.Result;
                };
                service.ChargerTypeDocumentAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void GetInformationFromChildWindowImageAutorisation(object sender, EventArgs e)
        {
            try
            {
                var form = (UcImageScanne)sender;
                if (form != null)
                {
                    if (form.DialogResult == true /*&& form.ImageScannee != null*/)
                    {
                        this.lnkLetter.Content = "Voir la pièce jointe";
                        //this.lnkLetter.Tag = form.ImageScannee;
                        //SaveFile(form.ImageScannee, (int)SessionObject.TypeDocumentScanneDevis.Lettre);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        DateTime DtpPose = new DateTime();
        DateTime DtpDePose = new DateTime();
        private void dtpPose_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtpPose.SelectedDate != null && dtpPose.SelectedDate.Value != null && dg_compteur.ItemsSource != null)
            {
                List<CsCanalisation> lesCompteur = (List<CsCanalisation>)dg_compteur.ItemsSource;
                lesCompteur.ForEach(t => t.POSE = dtpPose.SelectedDate);
                DtpPose = dtpPose.SelectedDate.Value ; 
                this.TxtperiodePose.Text = dtpPose.SelectedDate.Value.Month.ToString("00") + "/" + dtpPose.SelectedDate.Value.Year;
            }
        }

        private void dtpDepose_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtpDepose.SelectedDate != null && dtpDepose.SelectedDate.Value != null && dg_AncienCompteur.ItemsSource != null)
            {
                List<CsEvenement> lesCompteur = (List<CsEvenement>)dg_AncienCompteur.ItemsSource;
                lesCompteur.ForEach(t => t.DATEEVT  = dtpDepose.SelectedDate);
                DtpDePose = dtpDepose.SelectedDate.Value; 
                this.TxtperiodeDepose.Text = dtpDepose.SelectedDate.Value.Month.ToString("00") + "/" + dtpDepose.SelectedDate.Value.Year;

            }
        }


        #region ZEG 25/09/2017

        private void dtpPose_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.dtpPose.Text) && this.dtpPose.Text.Length == 10 && dg_compteur.ItemsSource != null)
            {
                List<CsCanalisation> lesCompteur = (List<CsCanalisation>)dg_compteur.ItemsSource;
                lesCompteur.ForEach(t => t.POSE = DateTime.Parse(this.dtpPose.Text));
                DtpPose = DateTime.Parse(this.dtpPose.Text);
                this.TxtperiodePose.Text = DtpPose.Month.ToString("00") + "/" + DtpPose.Year;
            }
        }



        private void dtpDepose_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.dtpDepose.Text) && this.dtpDepose.Text.Length == 10 && dg_AncienCompteur.ItemsSource != null)
            {
                List<CsEvenement> lesCompteur = (List<CsEvenement>)dg_AncienCompteur.ItemsSource;
                lesCompteur.ForEach(t => t.DATEEVT = DateTime.Parse(this.dtpDepose.Text));
                DtpDePose = DateTime.Parse(this.dtpDepose.Text);
                this.TxtperiodeDepose.Text = DtpDePose.Month.ToString("00") + "/" + DtpDePose.Year;

            }

        }

        #endregion



    }
}


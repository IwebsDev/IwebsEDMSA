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
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Devis
{
    public partial class UcProcesVerbalEtalonnage : ChildWindow
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
        List<CsScelle> ListeScelle = new List<CsScelle>();
        List<CsScelle> ListeScelleToRemove = new List<CsScelle>();
        private List<CsTcompteur> _listeCanalisation = new List<CsTcompteur>();
        private List<CsMarqueCompteur> _listeMarqueCpt = new List<CsMarqueCompteur>();
        private List<CsReglageCompteur> _listeDesDiametreExistant = new List<CsReglageCompteur>();

        public UcProcesVerbalEtalonnage()
        {
            InitializeComponent();
        }
        public UcProcesVerbalEtalonnage(int pIdDevis)
        {
            try
            {
                InitializeComponent();
                ChargerPuissanceInstalle();
                ChargeDetailDEvis(pIdDevis);
                ChargerDonneeDuSite();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
        List<int> lesCentrePerimetre = new List<int>();

        private void ChargerDonneeDuSite()
        {
            try
            {
                SessionObject.ModuleEnCours = "Accueil";
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentrePerimetre.Add(item.PK_ID);
                    loadCompteur(lstSite.Select(o => o.CODE).ToList());
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentrePerimetre.Add(item.PK_ID);
                    loadCompteur(lstSite.Select(o => o.CODE).ToList());

                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
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

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.GetDevisByNumIdDevisCompleted += (ssender, args) =>
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
                    laDetailDemande = args.Result;
                    laDemandeSelect = laDetailDemande.LaDemande;
                    this.Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemandeSelect.NUMDEM) ? laDemandeSelect.NUMDEM : string.Empty;

                    ShowControleDeposeCompteur(true);
                    ChargerCompteurADeposer(laDetailDemande.LaDemande);

                    LoadListeOrganeScellable(laDetailDemande.LaDemande.FK_IDTYPEDEMANDE, laDetailDemande.LaDemande.FK_IDPRODUIT.Value  );
                    LoadCompteur(laDetailDemande.LstCanalistion);
                    LoadListeScelle();
                }
            };
            client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
        }

        private void ShowControleDeposeCompteur(bool p)
        {
            Visibility State=p?Visibility.Visible:Visibility.Collapsed;
            
            //txt_CommentaireDepose.Visibility = State;
            //txt_IndexDepose.Visibility = State;
            //dtp_DateDepose.Visibility = State;
            //lbl_CommentaireDepose.Visibility = State;
            //lbl_DatePose.Visibility = State;
            //lbl_IndexDepose.Visibility = State;
            //groupBox2_Copy4.Visibility = State;
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
                    if (dg_composantScellable.ItemsSource == null)
                        tabitemScelle.Visibility = System.Windows.Visibility.Collapsed;

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

        private void LoadCompteur(List<CsCanalisation> list)
        {
          
            if (((laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance || 
                laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance) && 
                laDetailDemande.LaDemande.ISCHANGECOMPTEUR == true) 
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Reabonnement )
            {
                if (list.First().PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    DataReferenceManager.CodificationCompteurMt(list);
                dg_compteur.ItemsSource = list.Where(c => c.CAS == SessionObject.Enumere.CasPoseCompteur).ToList();
                List<CsCanalisation> lstNouvCompt = list.Where(c => c.CAS == SessionObject.Enumere.CasPoseCompteur).ToList();
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
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur  )
                    {
                        if (lesAncienCompteur.Count == 0)
                        {
                            Message.ShowInformation("Ce client n'a pas de compteur", "Resiliation");
                            return;
                        }
                        if (laDetailDemande.LstCanalistion == null) 
                          laDetailDemande.LstCanalistion = new List<CsCanalisation>();

                        laDetailDemande.LstCanalistion.AddRange(lesAncienCompteur);
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

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EnregisterOuTransmetre(false);
        }
        private void EnregisterOuTransmetre(bool IsEnregister)
        {
            try
            {
                if (DtpDebutTravaux.SelectedDate == null || DtpFinTravaux.SelectedDate == null)
                    throw new Exception("Veuillez renseigner les dates de debute et de fin de travaux");
                if ((DateTime.Parse(DtpFinTravaux.SelectedDate.Value.ToString())) < DateTime.Parse(DtpDebutTravaux.SelectedDate.Value.ToString()))
                    throw new Exception("Date de fin inférieure à la date début travaux");

                //if (!Convert.ToBoolean(ChkRemiseEnStock.IsChecked))
                //    this.Elements = null;
                if (this.Chk_AvecChangementCompteur.IsChecked == true )
                {
                    if (laDetailDemande.LstCanalistion == null)
                        laDetailDemande.LstCanalistion = new List<CsCanalisation>();

                    laDetailDemande.LstCanalistion = (List<CsCanalisation>)dg_compteur.ItemsSource;
                    List<CsCanalisation> lst = new List<CsCanalisation>();
                    lst = ((List<CsCanalisation>)dg_AncienCompteur.ItemsSource).ToList();

                    if (laDetailDemande.LstCanalistion == null && lst != null && lst.Count() != 0)
                    {
                        laDetailDemande.LstCanalistion = new List<CsCanalisation>();
                        laDetailDemande.LstCanalistion.AddRange(lst);
                    }
                    else
                        laDetailDemande.LstCanalistion.AddRange(lst);

                    #region Gestion AD:/DP
                    if ((laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance) &&
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur)
                    {
                        //int IndexDepose = 0;
                        var Canalisation = ((CsCanalisation)dg_AncienCompteur.SelectedItem);
                        if (Canalisation.INDEXEVT == null || Canalisation.INDEXEVT < 0)
                            throw new Exception("Index de dépose incorrecte");
                    }

                    #endregion

                    foreach (var leCompteur in laDetailDemande.LstCanalistion)
                    {
                        CsEvenement leEvtPose = new CsEvenement();
                        leEvtPose.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
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
                        {
                            leEvtPose.POINT = leCompteur.POINT;

                        }

                        leEvtPose.FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT.Value;
                        leEvtPose.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                        leEvtPose.MATRICULE = laDetailDemande.LaDemande.MATRICULE;



                        leEvtPose.REGLAGECOMPTEUR = leCompteur.REGLAGECOMPTEUR;
                        leEvtPose.TYPECOMPTEUR = leCompteur.TYPECOMPTEUR;
                        leEvtPose.COMPTEUR = leCompteur.NUMERO;
                        leEvtPose.TYPETARIF = laDetailDemande.Abonne.TYPETARIF;
                        leEvtPose.CATEGORIE = laDetailDemande.LeClient.CATEGORIE;
                        leEvtPose.USERCREATION = UserConnecte.matricule;
                        leEvtPose.USERMODIFICATION = UserConnecte.matricule;
                        leEvtPose.DATECREATION = System.DateTime.Now;
                        leEvtPose.DATEMODIFICATION = System.DateTime.Now;
                        leEvtPose.CAS = leCompteur.CAS;
                        leEvtPose.FK_IDCANALISATION = leCompteur.PK_ID;
                        leEvtPose.FK_IDABON = null;

                        leEvtPose.CODEEVT = SessionObject.Enumere.EvenementCodeNormale;
                        leEvtPose.STATUS = SessionObject.Enumere.EvenementReleve;
                        leEvtPose.DATEEVT = leEvtPose.CAS == SessionObject.Enumere.CasPoseCompteur ? DtpPose : DtpDePose;
                        leEvtPose.INDEXEVT = leCompteur.INDEXEVT;
                        leEvtPose.PERIODE = leEvtPose.CAS == SessionObject.Enumere.CasPoseCompteur ? Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(TxtperiodePose.Text) : Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(TxtperiodeDepose.Text);
                        leEvtPose.PUISSANCE = laDetailDemande.Abonne.PUISSANCE;
                        leEvtPose.TYPECOMPTAGE = laDetailDemande.Branchement.TYPECOMPTAGE;

                        leEvtPose.COEFK1 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE1;
                        leEvtPose.COEFK2 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE2;

                        if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation)
                        {
                            CsEvenement leEvtDuPoint = laDetailDemande.LstEvenement.First(t => t.POINT == leEvtPose.POINT && !string.IsNullOrEmpty(t.CASEVENEMENT));
                            int? ConsoPrecedent = leEvtDuPoint != null ? leEvtDuPoint.INDEXEVT : 0;
                            leEvtPose.CONSO = leCompteur.INDEXEVT - ConsoPrecedent;
                            leEvtPose.FK_IDCANALISATION = leEvtDuPoint.FK_IDCANALISATION;
                            leEvtPose.REGLAGECOMPTEUR = laDetailDemande.LstCanalistion.First().REGLAGECOMPTEUR;
                            leEvtPose.FK_IDABON = leEvtDuPoint.FK_IDABON;
                            leEvtPose.DATERELEVEPRECEDENTEFACTURE = leEvtDuPoint.DATEEVT;
                            leEvtPose.PERIODEPRECEDENTEFACTURE = leEvtDuPoint.PERIODE;
                            leEvtPose.ORDREAFFICHAGE = leEvtDuPoint.ORDREAFFICHAGE;
                            leEvtPose.INDEXPRECEDENTEFACTURE = leEvtDuPoint.INDEXEVT;
                            leEvtPose.QTEAREG = leEvtDuPoint.QTEAREG;
                            leEvtPose.CASPRECEDENTEFACTURE = leEvtDuPoint.CAS;
                            leEvtPose.FK_IDCOMPTEUR = leEvtDuPoint.FK_IDCOMPTEUR;
                            leEvtPose.ORDTOUR = leEvtDuPoint.ORDTOUR;
                            leEvtPose.FK_IDTOURNEE = laDetailDemande.Ag.FK_IDTOURNEE;
                            leEvtPose.CODECONSO = laDetailDemande.LeClient.CODECONSO;
                            leEvtPose.CATEGORIE = laDetailDemande.LeClient.CATEGORIE;
                        }
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
                    }
                    laDetailDemande.LstEvenement.RemoveAll(t => t.CASEVENEMENT == "000");
                    if (laDetailDemande.TravauxDevis == null)
                        laDetailDemande.TravauxDevis = new ObjTRAVAUXDEVIS();

                    laDetailDemande.TravauxDevis.FK_IDDEVIS = laDetailDemande.LaDemande.PK_ID;
                    laDetailDemande.TravauxDevis.NUMDEVIS = laDetailDemande.LaDemande.NUMDEM;
                    laDetailDemande.TravauxDevis.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
                    laDetailDemande.TravauxDevis.ORDRE = int.Parse(laDetailDemande.LaDemande.ORDRE);
                    //laDetailDemande.TravauxDevis.PROCESVERBAL  = this.TxtCommentaire.Text;
                    laDetailDemande.TravauxDevis.DATEDEBUTTRVX = Convert.ToDateTime(this.DtpDebutTravaux.SelectedDate.Value);
                    laDetailDemande.TravauxDevis.DATEFINTRVX = Convert.ToDateTime(this.DtpFinTravaux.SelectedDate.Value);
                    laDetailDemande.TravauxDevis.DATECREATION = System.DateTime.Today;
                    laDetailDemande.TravauxDevis.USERCREATION = UserConnecte.matricule;
                    laDetailDemande.TravauxDevis.MATRICULECHEFEQUIPE = UserConnecte.matricule;
                    laDetailDemande.TravauxDevis.MATRICULEREGLEMENT = UserConnecte.matricule;
                    laDetailDemande.TravauxDevis.NOMCHEFEQUIPE = UserConnecte.nomUtilisateur;
                    laDetailDemande.TravauxDevis.DATEREGLEMENT = System.DateTime.Today;
                    laDetailDemande.TravauxDevis.DATEPREVISIONNELLE = System.DateTime.Today;

                    if (dg_Scellage.ItemsSource != null)
                    {
                        ObjDOCUMENTSCANNE leDoc = new ObjDOCUMENTSCANNE();
                        if (lnkLetter.Tag != null)
                            leDoc = SaveFile((byte[])lnkLetter.Tag, 1, null);

                        laDetailDemande.LstOrganeScelleDemande = new List<CsOrganeScelleDemande>();
                        laDetailDemande.LstOrganeScelleDemande = (List<CsOrganeScelleDemande>)dg_Scellage.ItemsSource;
                        laDetailDemande.LstOrganeScelleDemande.First().CERTIFICAT = leDoc.CONTENU;

                    }
                    this.DateFin = Convert.ToDateTime(this.DtpFinTravaux.SelectedDate.Value);
                    laDetailDemande.LstCanalistion.RemoveAll(t => t.CAS == SessionObject.Enumere.CasDeposeCompteur);
                    laDetailDemande.EltDevis = null;
                    //laDetailDemande.LeClient = null;
                    laDetailDemande.Ag = null;
                    laDetailDemande.Abonne = null;
                    laDetailDemande.Branchement = null;
                    laDetailDemande.LstControleTvx = null;
                    laDetailDemande.OrdreTravail = null;
                    AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    clientDevis.ValiderDemandeCompleted += (ss, b) =>
                    {
                        this.btn_Transmetre.IsEnabled = true;
                        if (b.Cancelled || b.Error != null)
                        {
                            string error = b.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }

                        AcceuilServiceClient clientDeviss = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                        clientDeviss.ClotureValiderDemandeCompleted += (sss, bd) =>
                        {
                            if (bd.Cancelled || bd.Error != null)
                            {
                                string error = bd.Error.Message;
                                Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                                return;
                            }
                            if (bd.Result == true)
                            {

                                List<string> codes = new List<string>();
                                codes.Add(laDetailDemande.InfoDemande.CODE);
                                Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, false, this);
                            }
                            else
                            {
                                Message.ShowError("Erreur a la cloture de la demande", Silverlight.Resources.Devis.Languages.txtDevis);
                            }
                        };
                        clientDeviss.ClotureValiderDemandeAsync(laDetailDemande);
                    };
                    clientDevis.ValiderDemandeAsync(laDetailDemande);
                }
                else
                {
                    List<string> codes = new List<string>();
                    codes.Add(laDetailDemande.InfoDemande.CODE);
                    Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, false, this);
                
                }
            }
            catch (Exception ex)
            {
                this.btn_Transmetre.IsEnabled = true ;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        //private void InitControle(CsDemande laDemande)
        //{
        //    try
        //    {
        //        // Cahrger categorie client
        //        LayoutRoot.Cursor = Cursors.Wait;
        //        DevisServiceClient clientCategorieClient = new DevisServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Devis"));
        //        clientCategorieClient.SelectAllTcomptCompleted += (scat, argsCat) =>
        //        {
        //            try
        //            {
        //                if (argsCat.Cancelled || argsCat.Error != null)
        //                {
        //                    LayoutRoot.Cursor = Cursors.Arrow;
        //                    string error = argsCat.Error.Message;
        //                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
        //                    return;
        //                }
        //                if (argsCat.Result == null)
        //                {
        //                    LayoutRoot.Cursor = Cursors.Arrow;
        //                    Message.ShowError(Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, Silverlight.Resources.Devis.Languages.txtDevis);
        //                    return;
        //                }
        //                else
        //                {
        //                    _listeCanalisation = argsCat.Result;
                            
        //                    // Charger diamètre
        //                    DevisServiceClient clientdiametre = new DevisServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Devis"));
        //                    clientdiametre.SelectAllDiacompCompleted += (sdiam, argsdiam) =>
        //                    {
        //                        try
        //                        {
        //                            if (argsdiam.Cancelled || argsdiam.Error != null)
        //                            {
        //                                LayoutRoot.Cursor = Cursors.Arrow;
        //                                string error = argsdiam.Error.Message;
        //                                Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
        //                                return;
        //                            }
        //                            if (argsdiam.Result == null)
        //                            {
        //                                LayoutRoot.Cursor = Cursors.Arrow;
        //                                Message.ShowError(Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, Silverlight.Resources.Devis.Languages.txtDevis);
        //                                return;
        //                            }
        //                            else
        //                            {
        //                                _listeDesDiametreExistant = argsdiam.Result;
        //                                if (_listeDesDiametreExistant != null && _listeDesDiametreExistant.Count > 0)
        //                                {

        //                                    List<CsReglageCompteur > leDiametre = _listeDesDiametreExistant.Where(t => t.FK_IDPRODUIT == laDemande.LaDemande.FK_IDPRODUIT).ToList();
        //                                    //foreach (CsDiacomp item in leDiametre)
        //                                    //    Cbo_DiametreCompteur.Items.Add(item);
        //                                }
        //                                //        break;
        //                                //    }
        //                                //}
        //                                // Charger marque compteur
        //                                DevisServiceClient clientmarqueCpt = new DevisServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Devis"));
        //                                clientmarqueCpt.SelectAllMarqueCompteurCompleted += (sMarq, argsMarq) =>
        //                                {
        //                                    try
        //                                    {
        //                                        if (argsMarq.Cancelled || argsMarq.Error != null)
        //                                        {
        //                                            LayoutRoot.Cursor = Cursors.Arrow;
        //                                            string error = argsMarq.Error.Message;
        //                                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
        //                                            return;
        //                                        }
        //                                        if (argsMarq.Result == null)
        //                                        {
        //                                            LayoutRoot.Cursor = Cursors.Arrow;
        //                                            Message.Show(Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, Silverlight.Resources.Devis.Languages.txtDevis);
        //                                            return;
        //                                        }
        //                                        else
        //                                        {
        //                                            _listeMarqueCpt = argsMarq.Result;
        //                                            //Cbo_MarqueCompteur.Items.Clear();
        //                                            //if (_listeMarqueCpt != null && _listeMarqueCpt.Count > 0)
        //                                            //    foreach (var item in _listeMarqueCpt)
        //                                            //    {
        //                                            //        Cbo_MarqueCompteur.Items.Add(item);
        //                                            //    }
        //                                            //Cbo_MarqueCompteur.SelectedValuePath = "PK_ID";
        //                                            //Cbo_MarqueCompteur.DisplayMemberPath = "LIBELLE";

        //                                            //foreach (CsMarqueCompteur marqueCpt in Cbo_MarqueCompteur.Items)
        //                                            //{
        //                                            //    if (marqueCpt.CODE == InformationsDevis.Devis.IDMARQUECTR)
        //                                            //    {
        //                                            //        Cbo_MarqueCompteur.SelectedItem = marqueCpt;
        //                                            //        break;
        //                                            //    }
        //                                            //}
        //                                            // Renseigner les champs du formulaire
        //                                            //this.TxtNumeroGps.Text = !string.IsNullOrEmpty(InformationsDevis.Devis.NUMEROGPS) ? InformationsDevis.Devis.NUMEROGPS : string.Empty;
        //                                            if (laDemande.Branchement != null)
        //                                            {
        //                                                this.Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
        //                                                this.TxtBranchementProche.Text = (laDemande.Branchement.NBPOINT != null) ? laDemande.Branchement.NBPOINT.ToString() : string.Empty;
        //                                                //this.TxtEmplacementCrt.Text = !string.IsNullOrEmpty(InformationsDevis.Devis.EMPLACEMENTCOMPTEUR) ? InformationsDevis.Devis.EMPLACEMENTCOMPTEUR : string.Empty;
        //                                                //this.TxtNumeCompteur.Text = !string.IsNullOrEmpty(InformationsDevis.Devis.NUMEROCTR) ? InformationsDevis.Devis.NUMEROCTR : string.Empty;
        //                                                //if (!string.IsNullOrEmpty(TxtNumeCompteur.Text))
        //                                                //    TxtNumeCompteur.IsReadOnly = true;
        //                                                //laDetailDemande.Branchement.NBPOINT.ToString();
        //                                                //rDemande = InformationsDevis.DemandeDevis;
        //                                                //this.TxtPoteau.Text = !string.IsNullOrEmpty(laDemande.Branchement.NUMPOTEAUPROCHE) ? InformationsDevis.DemandeDevis.NUMPOTEAUPROCHE : string.Empty;
        //                                                this.TxtAdresse.Text = laDemande.Branchement.ADRESSERESEAU != null ? laDemande.Branchement.ADRESSERESEAU : string.Empty;
        //                                                this.TxtCategorie.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLECATEGORIE) ? laDemande.LeClient.LIBELLECATEGORIE : string.Empty;
        //                                                //this.TxtNumeroGps=laDetailDemande.Branchement.LATITUDE+";"+laDetailDemande.Branchement.LATITUDE
                                                        
        //                                            }
        //                                            if (laDetailDemande.TravauxDevis!=null)
        //                                            {
        //                                                DtpDebutTravaux.SelectedDate = laDetailDemande.TravauxDevis.DATEDEBUTTRVX;
        //                                                DtpFinTravaux.SelectedDate = laDetailDemande.TravauxDevis.DATEFINTRVX;
        //                                            }
                                                  

        //                                        }
        //                                    }
        //                                    catch (Exception ex)
        //                                    {
        //                                        LayoutRoot.Cursor = Cursors.Arrow;
        //                                        Message.ShowError(ex.Message, Silverlight.Resources.Devis.Languages.txtDevis);
        //                                    }
        //                                };
        //                                clientmarqueCpt.SelectAllMarqueCompteurAsync();
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            LayoutRoot.Cursor = Cursors.Arrow;
        //                            Message.ShowError(ex.Message, Silverlight.Resources.Devis.Languages.txtDevis);
        //                        }
        //                        LayoutRoot.Cursor = Cursors.Arrow;
        //                    };
        //                    clientdiametre.SelectAllDiacompAsync();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                LayoutRoot.Cursor = Cursors.Arrow;
        //                Message.ShowError(ex.Message, Silverlight.Resources.Devis.Languages.txtDevis);
        //            }
        //            LayoutRoot.Cursor = Cursors.Arrow;
        //        };
        //        clientCategorieClient.SelectAllTcomptAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        private void ChkRemiseEnStock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (Convert.ToBoolean(ChkRemiseEnStock.IsChecked))
                //{
                //    UcRemiseEnStock form = new UcRemiseEnStock(laDetailDemande);
                //    form.Closed += new EventHandler(formRemiseEnStock_Closed);
                //    form.Show();
                //}
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void formRemiseEnStock_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (UcRemiseEnStock)sender;
                if (form != null)
                {
                    Elements = form.Elements;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void VerifierSaisie()
        {
            try
            {
                //this.OKButton.IsEnabled = ((this.TxtNumeCompteur.Text != string.Empty) && (this.TxtCommentaire.Text != string.Empty) &&
                //                  (DtpFinTravaux.SelectedDate != null && this.DtpFinTravaux.SelectedDate.Value != null) && (DtpDebutTravaux.SelectedDate != null && this.DtpDebutTravaux.SelectedDate.Value != null) &&
                //                  (DtpAnneeFabrication.SelectedDate != null && this.DtpAnneeFabrication.SelectedDate.Value != null) && (DtpDatePose.SelectedDate != null && this.DtpDatePose.SelectedDate.Value != null) && 
                //                  (this.TxtIndexDePose.Text != string.Empty));
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
                        OrganeScelleDemande.NUM_SCELLE = txt_NumScelleRompu.Text;
                    else
                    {
                        Message.ShowWarning("Veuillez saisir le numero du scelle rompu", "Information");
                        return;
                    }
                        //OrganeScelleDemande.NUM_SCELLE = !string.IsNullOrWhiteSpace(txt_NumScelleRompu.Text) ? txt_NumScelleRompu.Text : string.Empty;
                }
                if (rbt_NouveauScelle.IsChecked == true)
                {
                    if(!string.IsNullOrWhiteSpace(txt_NumNouveauScelle.Text))
                        OrganeScelleDemande.NUM_SCELLE =  txt_NumNouveauScelle.Text;
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
                    Message.ShowInformation("Plus de scellés disponible en stock veuillez vous approvisionner", "Information");
                }
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //if (dg_compteur.SelectedItem != null)
            //{
            //    var compteur = ((CsCanalisation)dg_compteur.SelectedItem);
            //    if (DtpAnneeFabrication.SelectedDate != null)
            //        compteur.ANNEEFAB = DtpAnneeFabrication.SelectedDate.Value.Year.ToString();
            //    if (Cbo_CategorieCompteur.SelectedItem != null)
            //        compteur.FK_IDTYPECOMPTEUR = ((CsTcompteur)Cbo_CategorieCompteur.SelectedItem).PK_ID;
            //    if (Cbo_DiametreCompteur.SelectedItem != null)
            //        compteur.FK_IDDIAMETRECOMPTEUR = ((CsDiacomp)Cbo_DiametreCompteur.SelectedItem).PK_ID;
            //    if (Cbo_MarqueCompteur.SelectedItem != null)
            //        compteur.FK_IDMARQUECOMPTEUR = ((CsMarqueCompteur)Cbo_MarqueCompteur.SelectedItem).PK_ID;

            //    Message.ShowInformation("Les information du compteur sélectionné on été modifié avec succès", "information");
            //}
            //else
            //{
            //    Message.ShowWarning("Veuillez sélectionner le compteur à modifier", "information");
            //}
        }

        private void DtpPose_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            DtpPose = ((DatePicker)sender).SelectedDate.Value;
            this.TxtperiodePose.Text = DtpPose.Month.ToString("00") + "/" + DtpPose.Year.ToString();
        }


        DateTime DtpPose = new DateTime();
        DateTime DtpDePose = new DateTime();

        private void DtpDePose_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            DtpDePose = ((DatePicker)sender).SelectedDate.Value;
            this.TxtperiodeDepose.Text = DtpDePose.Month.ToString("00") + "/" + DtpDePose.Year.ToString();
        }

        private void btn_Transmetre_Click(object sender, RoutedEventArgs e)
        {
            this.btn_Transmetre.IsEnabled = false;
            EnregisterOuTransmetre(true);
        }

        private void btn_Rejeter_Click(object sender, RoutedEventArgs e)
        {
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

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {

        }
        CsReglageCompteur leReglageCompteur = null;
        List<CsCompteur> LstCompteur = new List<CsCompteur>();

        private void Chk_AvecChangementCompteur_Checked(object sender, RoutedEventArgs e)
        {
            leReglageCompteur = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.CODE == laDetailDemande.LaDemande.REGLAGECOMPTEUR);
            List<CsCalibreCompteur> LeCalibreEquivalant = SessionObject.LstCalibreCompteur.Where(t => t.REGLAGEMINI >= leReglageCompteur.REGLAGEMINI &&
                                                                                                      t.REGLAGEMAXI <= leReglageCompteur.REGLAGEMAXI &&
                                                                                                      t.FK_IDPRODUIT == laDetailDemande.LaDemande.FK_IDPRODUIT).ToList();

            List<int> lesIdCalibre = new List<int>();
            foreach (CsCalibreCompteur item in LeCalibreEquivalant)
                lesIdCalibre.Add(item.PK_ID);

            UcDetailCompteur ctr = new UcDetailCompteur(laDetailDemande.LaDemande, LstCompteur.Where(t => lesIdCalibre.Contains(t.FK_IDCALIBRECOMPTEUR.Value)).ToList());
            ctr.Closed += new EventHandler(galatee_Check);
            ctr.Show();
        }
        List<CsCanalisation> lesCanalisationACree = new List<CsCanalisation>();

        void galatee_Check(object sender, EventArgs e)
        {

            UcDetailCompteur ctrs = sender as UcDetailCompteur;
            if (ctrs.isOkClick)
            {
                int i = 1;
                List<CsCompteur> _LesCompteurs = (List<CsCompteur>)ctrs.MyObject;

                foreach (CsCompteur _LeCompteur in _LesCompteurs)
                {
                    CsCanalisation canal = new CsCanalisation()
                    {
                        CENTRE = laDetailDemande.LaDemande.CENTRE,
                        CLIENT = laDetailDemande.LaDemande.CLIENT,
                        NUMDEM = laDetailDemande.LaDemande.NUMDEM,
                        PRODUIT = laDetailDemande.LaDemande.PRODUIT,
                        PROPRIO = "1",
                        MARQUE = _LeCompteur.MARQUE,
                        TYPECOMPTEUR = _LeCompteur.TYPECOMPTEUR,
                        NUMERO = _LeCompteur.NUMERO,
                        INFOCOMPTEUR = _LeCompteur.NUMERO,
                        FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE,
                        FK_IDPRODUIT = int.Parse(laDetailDemande.LaDemande.FK_IDPRODUIT.ToString()),
                        FK_IDMAGAZINVIRTUEL = _LeCompteur.PK_ID,
                        //FK_IDCOMPTEUR  = _LeCompteur.PK_ID,
                        FK_IDTYPECOMPTEUR = _LeCompteur.FK_IDTYPECOMPTEUR,
                        FK_IDMARQUECOMPTEUR = _LeCompteur.FK_IDMARQUECOMPTEUR,
                        FK_IDCALIBRE = _LeCompteur.FK_IDCALIBRECOMPTEUR,
                        FK_IDREGLAGECOMPTEUR = leReglageCompteur.PK_ID,
                        FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID,
                        LIBELLETYPECOMPTEUR = _LeCompteur.NUMERO,
                        LIBELLEMARQUE = _LeCompteur.LIBELLEMARQUE,
                        LIBELLEREGLAGECOMPTEUR  = _LeCompteur.LIBELLECALIBRE  ,
                        POSE = System.DateTime.Now,
                        USERCREATION = UserConnecte.matricule,
                        USERMODIFICATION = UserConnecte.matricule,
                        DATECREATION = System.DateTime.Now,
                        DATEMODIFICATION = System.DateTime.Now,
                        FK_IDPROPRIETAIRE = 1,
                        CAS = SessionObject.Enumere.CasPoseCompteur 
                    };
                    lesCanalisationACree.Add(canal);
                    laDetailDemande.LstCanalistion.Add(canal);
                }
                this.dg_compteur.ItemsSource = null;
                this.dg_compteur.ItemsSource = lesCanalisationACree;
            }
        }
        private void loadCompteur(List<string >CodeSite)
        {
            AcceuilServiceClient service2 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service2.RetourneListeCompteurMagasinCompleted += (sr2, res2) =>
            {

                if (res2 != null && res2.Cancelled)
                    return;
                LstCompteur = res2.Result;

            };
            service2.RetourneListeCompteurMagasinAsync(CodeSite );
            service2.CloseAsync();
        }
    }
}


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
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Resources.Accueil;
using System.IO;


namespace Galatee.Silverlight.Accueil
{
    public partial class FrmModificationBranchement : ChildWindow
    {
        public FrmModificationBranchement()
        {
            InitializeComponent();
        }
        CsCentre LeCentreSelect = new CsCentre();
        List<CsCentre> LstCentre = new List<CsCentre>();

        CsProduit LeProduitSelect = new CsProduit();
        List<CsProduit> ListeDesProduitDuSite = new List<CsProduit>();


        CsDemande LaDemande = new CsDemande();
        public CsDemande MyDemande
        {
            get { return LaDemande; }
            set { LaDemande = value; }
        }
        List<CsTypeBranchement> LstDiametreBrt;
        List<CsMaterielBranchement> LstDeMaterielBrt;
        CsBrt _LeBranchement = new CsBrt();
        List<CsCanalisation> CanalisationClientRecherche = new List<CsCanalisation>();
        CsBrt  BranchementClientRecherche = new  CsBrt();
        int InitValue = 0;
        bool isUpdate = false;
        public event EventHandler Closed;
        public FrmModificationBranchement(CsDemande _LaDemande)
        {
            InitializeComponent();
            LaDemande = _LaDemande;
            TypeDemande = LaDemande.LaDemande.TYPEDEMANDE;
            ChargerDiametre(LaDemande.LaDemande.PRODUIT);
            ChargerMAterielBranchement(LaDemande.LaDemande.PRODUIT);
            ChargerDonneeDuSite();
            ChargerListeDeProduit();
            isUpdate = true;
            this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
            this.Txt_CodeProduit.MaxLength = SessionObject.Enumere.TailleCodeProduit;
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
            BranchementClientRecherche = _LaDemande.Branchement;
            _LaDemande.LaDemande.STATUTDEMANDE = null;

            this.btn_Supprime.Visibility = System.Windows.Visibility.Collapsed;
            this.btn_Modifier.Visibility = System.Windows.Visibility.Collapsed;
            this.btn_Rechercher.Visibility = System.Windows.Visibility.Collapsed;

            this.Txt_Ordre.Text = string.IsNullOrEmpty(_LaDemande.LaDemande.ORDRE) ? string.Empty : _LaDemande.LaDemande.ORDRE;
            this.Txt_CodeCentre.Text = string.IsNullOrEmpty(BranchementClientRecherche.CENTRE) ? string.Empty : BranchementClientRecherche.CENTRE;
            this.Txt_CodeProduit.Text = string.IsNullOrEmpty(BranchementClientRecherche.PRODUIT) ? string.Empty : BranchementClientRecherche.PRODUIT;
            this.Txt_NumDemande.Text = string.IsNullOrEmpty(LaDemande.LaDemande.NUMDEM) ? string.Empty : LaDemande.LaDemande.NUMDEM;
            this.Txt_Client.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
            AfficherBranchemetDemande(BranchementClientRecherche);
            RemplirLibelle();

            this.Txt_CodeCentre.IsReadOnly = true;
            this.Txt_CodeProduit.IsReadOnly = true;
            this.Txt_NumDemande.IsReadOnly = true;
            this.Txt_Client.IsReadOnly = true;
            this.btn_Rechercher.IsEnabled = false;
            this.btn_Centre.IsEnabled = false;
            this.btn_Produit.IsEnabled = false;
            this.lnkMotif.Visibility = System.Windows.Visibility.Collapsed;


            if (LaDemande.LaDemande.FICHIERJOINT != new Guid("00000000-0000-0000-0000-000000000000"))
                RetourneObjectScan(LaDemande.LaDemande);

        }
        ObjDOCUMENTSCANNE leObjectScan = new ObjDOCUMENTSCANNE();
        private void RetourneObjectScan(CsDemandeBase laDemande)
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ReturneObjetScanCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    leObjectScan = args.Result;
                    if (leObjectScan != null)
                    {
                        this.lnkLetter.Content = Langue.msgVoirPiecejointe;
                        this.lnkLetter.Tag = leObjectScan.CONTENU;
                        this.btn_Supprime.Visibility = System.Windows.Visibility.Visible;
                        this.btn_Modifier.Visibility = System.Windows.Visibility.Visible;
                    }
                };
                service.ReturneObjetScanAsync(laDemande);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        string TypeDemande = string.Empty;
        public FrmModificationBranchement(string  _typeDemande)
        {
            InitializeComponent();
            if (LaDemande.LaDemande == null) LaDemande.LaDemande = new CsDemandeBase();
            if (LaDemande.Branchement == null) LaDemande.Branchement = new CsBrt ();
            TypeDemande = _typeDemande;
            ChargerDiametre(LaDemande.LaDemande.PRODUIT);
            ChargerMAterielBranchement(LaDemande.LaDemande.PRODUIT);
            ChargerDonneeDuSite();
            ChargerListeDeProduit();
            this.lnkMotif.Visibility = System.Windows.Visibility.Collapsed;

            this.btn_Supprime.Visibility = System.Windows.Visibility.Collapsed;
            this.btn_Modifier.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_Ordre.Visibility = System.Windows.Visibility.Collapsed;
            this.lbl_Ordre.Visibility = System.Windows.Visibility.Collapsed;


            this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
            this.Txt_CodeProduit.MaxLength = SessionObject.Enumere.TailleCodeProduit;
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
        }
        List<CsSite> lstSite = new List<CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (isUpdate && SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    LstCentre = SessionObject.LstCentre;
                    return;
                }

                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;

                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<CsSite> _LstSite = lstSite.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1 && !isUpdate )
                        {
                            this.Txt_CodeSite.Text = _LstSite[0].CODESITE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.IsReadOnly = true;
                        }
                    }
                    if (LstCentre != null)
                    {
                        List<CsCentre> _LstCentre = LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstCentre.Count == 1)
                        {
                            this.Txt_CodeCentre.Text = _LstCentre[0].CODE;
                            this.Txt_LibelleCentre.Text = _LstCentre[0].LIBELLE;
                        }
                        else
                        {
                            CsCentre _LeCentre = new CsCentre();
                            if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text))

                                _LeCentre = LstCentre.FirstOrDefault(p => p.CODE == this.Txt_CodeCentre.Text);
                            //else
                            //{
                            //    this.Txt_CodeCentre.Text = LaDemande.LaDemande.CENTRE;
                            //    _LeCentre = LstCentre.FirstOrDefault(p => p.CODE == LaDemande.LaDemande.CENTRE);
                            //}
                            if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                            {
                                this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                                this.btn_Centre.IsEnabled = false;
                                this.Txt_CodeCentre.IsReadOnly = true;
                            }
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false );
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerListeDeProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit!= null && SessionObject.ListeDesProduit.Count != 0)
                {
                    ListeDesProduitDuSite = SessionObject.ListeDesProduit;
                    if (ListeDesProduitDuSite != null)
                    {
                        if (ListeDesProduitDuSite.Count == 1)
                        {
                            this.Txt_CodeProduit.Text = ListeDesProduitDuSite[0].CODE;
                            this.Txt_LibelleProduit.Text = ListeDesProduitDuSite[0].LIBELLE;
                        }
                    }
                }
                else
                {
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service1.ListeDesProduitCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        SessionObject.ListeDesProduit = res.Result;
                        ListeDesProduitDuSite = SessionObject.ListeDesProduit;
                        if (ListeDesProduitDuSite != null)
                        {
                            if (ListeDesProduitDuSite.Count == 1)
                            {
                                this.Txt_CodeProduit.Text = ListeDesProduitDuSite[0].CODE;
                                this.Txt_LibelleProduit.Text = ListeDesProduitDuSite[0].LIBELLE;
                                this.btn_Produit.IsEnabled = false;
                            }
                            else
                            {
                                CsProduit _LeProduit = ListeDesProduitDuSite.FirstOrDefault(p => p.CODE == LaDemande.LaDemande.PRODUIT);
                                if (_LeProduit != null && !string.IsNullOrEmpty(_LeProduit.CODE ))
                                {
                                    this.Txt_CodeProduit.Text = LaDemande.LaDemande.PRODUIT;
                                    this.Txt_LibelleProduit.Text = _LeProduit.LIBELLE;
                                    this.Txt_CodeProduit.IsReadOnly = true;
                                }
                            }
                        }
                    };
                    service1.ListeDesProduitAsync();
                    service1.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void InitCtrl()
        {
            if (isUpdate)
            {
                if (!string.IsNullOrEmpty(LaDemande.LaDemande.ANNOTATION))
                    this.lnkMotif.Visibility = System.Windows.Visibility.Visible;
            }
        }       
        void AfficherBranchemetDemande(CsBrt _LeBrtDemande)
        {
            this.Txt_CodeDiametre.Text = string.IsNullOrEmpty(_LeBrtDemande.DIAMBRT) ? string.Empty : _LeBrtDemande.DIAMBRT;
            this.Txt_CodeMateriel.Text = string.IsNullOrEmpty(_LeBrtDemande.NATBRT) ? string.Empty : _LeBrtDemande.NATBRT;
            this.Txt_LongueurBrt.Text = string.IsNullOrEmpty(_LeBrtDemande.LONGBRT.ToString()) ? InitValue.ToString() : _LeBrtDemande.LONGBRT.ToString();
            this.Txt_NombrePoint.Text = _LeBrtDemande.NBPOINT.ToString();
            this.Txt_DateRacordement.Text = string.IsNullOrEmpty(_LeBrtDemande.DRAC.ToString()) ? string.Empty : Convert.ToDateTime( _LeBrtDemande.DRAC).ToShortDateString();
            this.Txt_DateDepose.Text = string.IsNullOrEmpty(_LeBrtDemande.DRES.ToString()) ? string.Empty : Convert.ToDateTime(_LeBrtDemande.DRES).ToShortDateString();
            this.Txt_Longitude.Text = string.IsNullOrEmpty(_LeBrtDemande.LONGITUDE) ? string.Empty : _LeBrtDemande.LONGITUDE;
            this.Txt_Latitude.Text = string.IsNullOrEmpty(_LeBrtDemande.LATITUDE) ? string.Empty : _LeBrtDemande.LATITUDE;
            //this.Txt_CodeGeographique.Text = string.IsNullOrEmpty(_LeBrtDemande.ADRESSERESEAU) ? string.Empty : _LeBrtDemande.ADRESSERESEAU;
        }

        void ChargerDiametre(string Produit)
        {
            try
            {
                //AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                //service.ChargerDiamentreBranchementCompleted += (s, args) =>
                //{
                //    LstDiametreBrt = new List<CsDiametreBranchement>();
                //    if ((args != null && args.Cancelled) || (args.Error != null))
                //        return;
                //    LstDiametreBrt = args.Result;
                //    if (LstDiametreBrt != null && LstDiametreBrt.Count != 0)
                //    {
                //        if (!string.IsNullOrEmpty(this.Txt_CodeDiametre.Text))
                //        {
                //            CsDiametreBranchement _LeDiametre = ClasseMEthodeGenerique.RetourneObjectFromList(LstDiametreBrt, this.Txt_CodeDiametre.Text, "DIAMETRE");
                //            if (_LeDiametre != null && !string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                //                this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                //        }
                //    }
                //};
                //service.ChargerDiamentreBranchementAsync(Produit);
                //service.CloseAsync();
            }
            catch (Exception es)
            {

                MessageBox.Show(es.Message);
            }

        }
        private void btn_diametre_Click(object sender, RoutedEventArgs e)
        {
            //this.btn_diametre.IsEnabled = false;
            if (LstDiametreBrt.Count != 0)
            {
                List<object> _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(LstDiametreBrt);
                UcListeGenerique ctr = new UcListeGenerique(_LstObj, "DIAMETRE", "LIBELLE", Langue.lbl_ListeDiametre);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnDiametre);
                ctr.Show();
            }
            this.btn_diametre.IsEnabled = true;

        }
        void galatee_OkClickedBtnDiametre(object sender, EventArgs e)
        {

            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    //CsDiametreBranchement _LeDiametre = (CsDiametreBranchement)ctrs.MyObject;
                    //this.Txt_CodeDiametre.Text = _LeDiametre.CODE ;
                    //this.btn_diametre.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        void Translate()
        {
            //this.lbl_Adresse.Content = Langue.lbl_adresse;
            //this.lbl_client.Content = Langue.lbl_client;
            this.lbl_Calibre.Content = Langue.lbl_Calibre;
            this.lbl_MaterielUtilise.Content = Langue.lbl_MaterielUtilise;
            this.lbl_DureeConnexion.Content = Langue.lbl_DureeConnexion;
            this.lbl_NombrePoint.Content = Langue.lbl_NombrePoint;
            this.lbl_Reseau.Content = Langue.lbl_Reseau;
            this.lbl_Segment.Content = Langue.lbl_Segment;
            this.lbl_longitude.Content = Langue.lbl_longitude;
            this.lbl_latitude.Content = Langue.lbl_latitude;
            this.lbl_Localisation.Content = Langue.lbl_Localisation;
            this.btn_Transformer.Content = Langue.btn_Transformer;
            this.lbl_dateConnexion.Content = Langue.lbl_dateConnexion;
            this.lbl_DateFermeture.Content = Langue.lbl_DateFermeture;
            this.rdb_InService.Content = Langue.rdb_EnService;
            this.rdb_deconnecter.Content = Langue.rdb_HorsService;
            this.chk_AvantCompteur.Content = Langue.chk_AvantCompteur;
            
        }
        void ChargerMAterielBranchement(string Produit)
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneMaterielBranchementCompleted += (s, args) =>
                {
                    LstDeMaterielBrt = new List<CsMaterielBranchement>();
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    LstDeMaterielBrt.AddRange(args.Result);
                    if (LstDeMaterielBrt != null && LstDeMaterielBrt.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_CodeMateriel.Text))
                        {
                            CsMaterielBranchement _LeMateriel = ClasseMEthodeGenerique.RetourneObjectFromList(LstDeMaterielBrt, this.Txt_CodeMateriel.Text, "MATERIEL");
                            if (_LeMateriel != null && !string.IsNullOrEmpty(_LeMateriel.LIBELLE))
                                this.Txt_LibelleMateriel.Text = _LeMateriel.LIBELLE;
                        }
                    }
                };
                service.RetourneMaterielBranchementAsync(Produit);
                service.CloseAsync();
            }
            catch (Exception es)
            {

                MessageBox.Show(es.Message);
            }

        }
        private void btn_materiel_Click(object sender, RoutedEventArgs e)
        {
            //this.btn_materiel.IsEnabled = false;
            if (LstDeMaterielBrt.Count != 0)
            {
                List<object> _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(LstDeMaterielBrt);
                UcListeGenerique ctr = new UcListeGenerique(_LstObj, "MATERIEL", "LIBELLE", Langue.lbl_ListeMateriel);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnerMateriel);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnerMateriel(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            CsMaterielBranchement _LeMateriel = (CsMaterielBranchement)ctrs.MyObject;
            this.Txt_CodeMateriel.Text = _LeMateriel.CODE;
            this.btn_materiel.IsEnabled = true;

        }
        ObjDOCUMENTSCANNE leDoc = new ObjDOCUMENTSCANNE();
        private void  EnregisterInfo(CsDemande _LaDemande)
        {
            _LaDemande.Branchement.NUMDEM = this.Txt_NumDemande.Text;
            _LaDemande.Branchement.ADRESSERESEAU  = this.Txt_NumDemande.Text;
            _LaDemande.Branchement.DIAMBRT = string.IsNullOrEmpty(this.Txt_CodeDiametre.Text) ? null : this.Txt_CodeDiametre.Text;
            _LaDemande.Branchement. LONGBRT = string.IsNullOrEmpty(this.Txt_LongueurBrt.Text) ? 0 : int.Parse(this.Txt_LongueurBrt.Text);
            _LaDemande.Branchement.NBPOINT  = string.IsNullOrEmpty(this.Txt_NombrePoint.Text) ? 0 : int.Parse(this.Txt_NombrePoint.Text);
            _LaDemande.Branchement.RESEAU = string.IsNullOrEmpty(this.Txt_Reseau.Text) ? null : this.Txt_Reseau.Text;
            _LaDemande.Branchement.LONGITUDE = string.IsNullOrEmpty(this.Txt_Longitude.Text) ? null : this.Txt_Longitude.Text; 
            _LaDemande.Branchement.LATITUDE  = string.IsNullOrEmpty(this.Txt_Latitude.Text) ? null : this.Txt_Latitude.Text; 
            if (string.IsNullOrEmpty(this.Txt_DateRacordement.Text) )
            _LaDemande.Branchement.DRAC =DateTime.Parse(this.Txt_DateRacordement.Text);
            _LaDemande.Branchement .NATBRT = string.IsNullOrEmpty(this.Txt_CodeMateriel.Text) ? null : this.Txt_CodeMateriel.Text;

            _LaDemande.LaDemande.NUMDEM = this.Txt_NumDemande.Text;
            _LaDemande.LaDemande.ORDRE  = this.Txt_Ordre .Text;
            _LaDemande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusEnAttente;
            _LaDemande.LaDemande.TYPEDEMANDE = TypeDemande;
            _LaDemande.LaDemande.CLIENT = BranchementClientRecherche.CLIENT;
            _LaDemande.LaDemande.USERCREATION = UserConnecte.matricule;
            _LaDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
            _LaDemande.LaDemande.DATECREATION =  System.DateTime.Now;
            _LaDemande.LaDemande.DATEMODIFICATION =  System.DateTime.Now;
            if (lnkLetter.Tag != null)
            {
                leDoc = SaveFile((byte[])lnkLetter.Tag, 1, null);
                //_Lademande.LeDocumentScanne = leDoc;
            }
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

                            this.btn_Supprime.Visibility = System.Windows.Visibility.Visible;
                            this.btn_Modifier.Visibility = System.Windows.Visibility.Visible;
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
                    if (LaDemande.LaDemande != null)
                        LaDemande.LaDemande.FICHIERJOINT = pDocumentScane.PK_ID;
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
                        this.lnkLetter.Content = Langue.msgVoirPiecejointe;
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
        private void lnkMotif_Click(object sender, RoutedEventArgs e)
        {
            Message.ShowInformation(LaDemande.LaDemande.ANNOTATION, "Motif réjet");

        }
        private void Txt_CodeDiametre_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (this.Txt_CodeDiametre.Text.Length == SessionObject.Enumere.TailleDiametreBranchement  && SessionObject.LstDiametreBrt .Count != 0)
            //{
                //CsDiametreBranchement leDbrt = SessionObject.LstDiametreBrt.FirstOrDefault(t => t.CODE  == Txt_CodeDiametre.Text);
                //if (leDbrt != null)
                //    this.Txt_LibelleDiametre.Text = leDbrt.LIBELLE;
            //}
        }
        private void Txt_LongueurBrt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void Txt_DateRacordement_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
        private void Txt_DateRacordement_TextChanged_1(object sender, TextChangedEventArgs e)
        {
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EnregisterInfo(LaDemande);
            ValidationDemande(LaDemande);
       

            this.DialogResult = true;

        }
        private void ValidationDemande(CsDemande _LaDemande)
        {
            try
            {
                //Lancer la transaction de mise a jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ValiderDemandeWithObjectCompleted += (sr, res) =>
                {

                    string Messages = string.Empty;
                    //Si la date de d'encaissement n'est pas renseigné c-a-d que la demende est en attente
                    if (_LaDemande.LaDemande.STATUT == SessionObject.Enumere.DemandeStatusEnAttente)
                        //Msg de confirmation de l'enregistremet
                        Messages = Langue.MsgRequestSaved;
                    //Si la date d'encaissement est renseigné 
                    else
                        Messages = Langue.MsgOperationTerminee;

                    Message.ShowInformation(Messages, Langue.lbl_Menu);
                    if (Closed != null)
                        Closed(this, new EventArgs());
                    this.DialogResult = false;
                };
                service1.ValiderDemandeWithObjectAsync(_LaDemande, leDoc);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitCtrl();
        }

        private void Txt_CodeMateriel_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (this.Txt_CodeMateriel.Text.Length == 1  && SessionObject.LstDiametreBrt.Count != 0)
            //{
            //    CsMaterielBranchement leMaterielBrt = SessionObject.LstDeMaterielBrt.FirstOrDefault(t => t.CODE == this.Txt_CodeMateriel.Text);
            //    if (leMaterielBrt != null)
            //        this.Txt_LibelleMateriel .Text = leMaterielBrt.LIBELLE;
            //}
        }

        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
            {
                CsCentre _LeCentre = ClasseMEthodeGenerique.RetourneObjectFromList<CsCentre>(LstCentre, this.Txt_CodeCentre.Text, "CODE");
                if (_LeCentre != null && !string.IsNullOrEmpty( _LeCentre.CODE) )
                {
                    this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                    LaDemande.LaDemande.CENTRE = _LeCentre.CODE; ;
                    LaDemande.LaDemande.FK_IDCENTRE = _LeCentre.PK_ID;
                    string numIncrementiel = _LeCentre.NUMDEM.ToString();
                    if (_LeCentre.NUMDEM.ToString().Length >= 10)
                        numIncrementiel = _LeCentre.NUMDEM.ToString().Substring(numIncrementiel.Length - 9, 9);
                    this.Txt_NumDemande.Text = _LeCentre.CODE + numIncrementiel.PadLeft(10, '0');
                    LaDemande.LaDemande.NUMDEM = this.Txt_NumDemande.Text;
                    LaDemande.LeCentre = LeCentreSelect;
                    LaDemande.LaDemande.MATRICULE = UserConnecte.matricule;
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_CodeCentre.Focus();
                    };
                    w.Show();
                }
            }
        }



        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite.Count > 0)
                {
                    this.btn_Site.IsEnabled = false;
                    List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODESITE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedSite);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                CsSite leSite = (CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODESITE;
                this.LstCentre = LstCentre.Where(t => t.FK_IDCODESITE == leSite.PK_ID).ToList();

            }
            else
                this.btn_Centre.IsEnabled = true;


        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentre.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(LstCentre);
                    UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", Langue.lbl_ListeCentre);
                    ctr.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }
        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            this.btn_Centre.IsEnabled = true;
            LeCentreSelect = new CsCentre();
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsCentre leCentre = (CsCentre)ctrs.MyObject;
                LeCentreSelect = leCentre;
                this.Txt_CodeCentre.Text = leCentre.CODE;

                string numIncrementiel = LeCentreSelect.NUMDEM.ToString();
                if (LeCentreSelect.NUMDEM.ToString().Length >= 10)
                    numIncrementiel = LeCentreSelect.NUMDEM.ToString().Substring(numIncrementiel.Length - 9, 9);
                this.Txt_NumDemande.Text = LeCentreSelect.CODE + numIncrementiel.PadLeft(10, '0');
                LaDemande.LaDemande.FK_IDCENTRE = LeCentreSelect.PK_ID;

            }
        }


        private void btn_Produit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<object> _LstProduit = ClasseMEthodeGenerique.RetourneListeObjet(ListeDesProduitDuSite);
                UcListeGenerique ctr = new UcListeGenerique(_LstProduit, "CODE", "LIBELLE", Langue.lbl_ListeProduit);
                ctr.Closed += new EventHandler(galatee_OkClickedProduit);
                ctr.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedProduit(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    LeProduitSelect = (CsProduit)ctrs.MyObject;
                    this.Txt_CodeProduit.Text = LeProduitSelect.CODE;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void Txt_CodeProduit_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
                {
                    LaDemande.LaDemande.PRODUIT = this.Txt_CodeProduit.Text;
                    CsProduit _LeProduitSelect = ClasseMEthodeGenerique.RetourneObjectFromList(ListeDesProduitDuSite, this.Txt_CodeProduit.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeProduitSelect.LIBELLE))
                    {
                        this.Txt_LibelleProduit.Text = _LeProduitSelect.LIBELLE;
                        LaDemande.LeProduit = LeProduitSelect;
                        LaDemande.LaDemande.FK_IDPRODUIT = _LeProduitSelect.PK_ID;

                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeProduit.Focus();
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }

 
        private void btn_Rechercher_Click_1(object sender, RoutedEventArgs e)
        {
            if (!SessionObject.Enumere.IsModificationAutoriserEnFacturation)
            RetourneOrdre(this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_CodeProduit.Text);

            else
                VerifieDernierEvt(this.Txt_CodeCentre.Text, this.Txt_Client.Text, string.Empty );
        }
        private void RetourneOrdre(string centre, string client, string produit)
        {
            try
            {
                string OrdreMax = string.Empty;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneOrdreMaxCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    OrdreMax = args.Result;
                    this.lbl_Ordre.Visibility = System.Windows.Visibility.Visible;
                    this.Txt_Ordre.Visibility = System.Windows.Visibility.Visible;
                    this.Txt_Ordre.IsReadOnly = true;
                    if (OrdreMax != null)
                    {
                        this.Txt_Ordre.Text = OrdreMax;
                        VerifieExisteDemande(this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_Ordre.Text, LaDemande.LaDemande.FK_IDCENTRE, TypeDemande);
                    }
                    else
                        Message.ShowInformation("Aucune information trouvée", "Recherche de branchement");
                };
                service.RetourneOrdreMaxAsync(centre, client, produit);
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void VerifieDernierEvt(string centre, string client, string Ordre)
        {

            if (!string.IsNullOrEmpty(Txt_Client.Text) && Txt_Client.Text.Length == SessionObject.Enumere.TailleClient)
            {

                LaDemande.LaDemande.CLIENT = Txt_Client.Text;
                string OrdreMax = string.Empty;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.IsDernierEvtEnFacturationCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result != null)
                    {
                        if (args.Result == true)
                        {
                            Message.ShowError(Langue.msgFacturationEnCours, "Accueil");
                            return;
                        }
                        VerifieDernierEvt(this.Txt_CodeCentre.Text, this.Txt_Client.Text, string.Empty);
                    }
                };
                service.IsDernierEvtEnFacturationAsync(centre, client, Ordre);
                service.CloseAsync();
            }

        }
        private void VerifieExisteDemande(string centre, string client,string Ordre, int idCentre, string tdem)
        {

            try
            {
                if (!string.IsNullOrEmpty(Txt_Client.Text) && Txt_Client.Text.Length == SessionObject.Enumere.TailleClient)
                {

                    LaDemande.LaDemande.CLIENT = Txt_Client.Text;
                    string OrdreMax = string.Empty;
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneDemandeClientTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result != null)
                        {
                            if (args.Result.STATUT != SessionObject.Enumere.DemandeStatusPriseEnCompte && args.Result.ISSUPPRIME  != true)
                            {
                                Message.ShowError("Il existe déja une demande de ce type sur ce client", "Accueil");
                                return;
                            }
                        }
                        RetourneInfoBranchement(idCentre, this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_CodeProduit.Text);
                    };
                    service.RetourneDemandeClientTypeAsync(centre, client, Ordre, idCentre, tdem);
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void RetourneInfoBranchement(int fk_idcentre,string centre, string client, string produit)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);

            BranchementClientRecherche = new CsBrt();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneBranchementCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    BranchementClientRecherche = args.Result.FirstOrDefault(p => p.PRODUIT == LaDemande.LaDemande.PRODUIT);
                    if (BranchementClientRecherche != null && !string.IsNullOrEmpty(BranchementClientRecherche.CENTRE))
                    {
                        LaDemande.Branchement = BranchementClientRecherche;
                        AfficherBranchemetDemande(BranchementClientRecherche);
                    }
                    else
                        Message.ShowInformation("Aucune information trouvée", "Recherche de branchement");
                    LoadingManager.EndLoading(res);

                }
                else
                    Message.ShowInformation("Aucune information trouvée", "Recherche de branchement");

                LoadingManager.EndLoading(res);


            };
            service.RetourneBranchementAsync(fk_idcentre,centre, client, produit);
            service.CloseAsync();

        }
        void RemplirLibelle()
        {
            if (this.Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
            {
                CsCentre _LeCentre = ClasseMEthodeGenerique.RetourneObjectFromList<CsCentre>(SessionObject.LstCentre , this.Txt_CodeCentre.Text, "CODE");
                if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                {
                    this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                    this.Txt_CodeSite.Text = _LeCentre.CODESITE;
                    this.Txt_LibelleSite.Text = _LeCentre.LIBELLESITE ;
                }
            }
            //if (this.Txt_CodeDiametre.Text.Length == SessionObject.Enumere.TailleDiametreBranchement && SessionObject.LstDiametreBrt.Count != 0)
            //{
            //    CsTypeBranchement leDbrt = SessionObject.LstDiametreBrt.FirstOrDefault(t => t.CODE == Txt_CodeDiametre.Text);
            //    if (leDbrt != null)
            //        this.Txt_LibelleDiametre.Text = leDbrt.LIBELLE;
            //}
            //if (this.Txt_CodeMateriel.Text.Length == 1 && SessionObject.LstDiametreBrt.Count != 0)
            //{
            //    CsMaterielBranchement leMaterielBrt = SessionObject.LstDeMaterielBrt.FirstOrDefault(t => t.CODE == this.Txt_CodeMateriel.Text);
            //    if (leMaterielBrt != null)
            //        this.Txt_LibelleMateriel.Text = leMaterielBrt.LIBELLE;
            //}
            if (this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
            {
                LaDemande.LaDemande.PRODUIT = this.Txt_CodeProduit.Text;
                CsProduit _LeProduitSelect = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.ListeDesProduit , this.Txt_CodeProduit.Text, "CODE");
                if (!string.IsNullOrEmpty(_LeProduitSelect.LIBELLE))
                {
                    this.Txt_LibelleProduit.Text = _LeProduitSelect.LIBELLE;
                }
            }

        }
        private void Txt_CodeCentre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text))
                this.Txt_CodeCentre.Text = this.Txt_CodeCentre.Text.PadLeft(SessionObject.Enumere.TailleCentre , '0');
        }
        private void Txt_Client_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Client.Text))
                this.Txt_Client.Text = this.Txt_Client.Text.PadLeft(SessionObject.Enumere.TailleClient, '0');
        }

        private void Txt_CodeProduit_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeProduit.Text))
                this.Txt_CodeProduit.Text = this.Txt_CodeProduit.Text.PadLeft(SessionObject.Enumere.TailleCodeProduit , '0');
        }

        private void btn_Supprimer_click(object sender, RoutedEventArgs e)
        {
            if (lnkLetter.Tag != null)
            {
                var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.msgSuppressionFichier, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                w.OnMessageBoxClosed += (_, result) =>
                {
                    if (w.Result == MessageBoxResult.OK)
                    {
                        if (LaDemande != null && LaDemande.LaDemande != null && LaDemande.LaDemande.FICHIERJOINT != new Guid("00000000-0000-0000-0000-000000000000"))
                            LaDemande.LaDemande.FICHIERJOINT = new Guid("00000000-0000-0000-0000-000000000000");

                        this.btn_Supprime.Visibility = System.Windows.Visibility.Collapsed;
                        this.btn_Modifier.Visibility = System.Windows.Visibility.Collapsed;
                        lnkLetter.Tag = null;
                        this.lnkLetter.Content = "Motif de la modification";
                    }
                };
                w.Show();
            }
            else
                Message.ShowInformation(Langue.msgAucunfichier, Langue.lbl_Menu);

        }

        private void btn_Modifier_click(object sender, RoutedEventArgs e)
        {
            try
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
                        //var formScanne = new UcImageScanne(stream, SessionObject.ExecMode.Creation);
                        formScanne.Closed += new EventHandler(GetInformationFromChildWindowImageAutorisation);
                        formScanne.Show();
                    }
                }

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

       

        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
            {
                CsSite _LeSite = ClasseMEthodeGenerique.RetourneObjectFromList<CsSite>(lstSite, this.Txt_CodeSite.Text, "CODESITE");
                if (_LeSite != null && !string.IsNullOrEmpty(_LeSite.CODESITE))
                    this.Txt_LibelleSite.Text = _LeSite.LIBELLE;
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_CodeCentre.Focus();
                    };
                    w.Show();
                }
            }


        }

    }
}


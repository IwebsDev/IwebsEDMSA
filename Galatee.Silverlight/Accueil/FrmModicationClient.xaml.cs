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
using Galatee.Silverlight.Resources.Accueil ;
using Galatee.Silverlight.Resources.Devis;
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using System.IO;
using System.Collections.ObjectModel;
namespace Galatee.Silverlight.Accueil
{
    public partial class FrmModicationClient : ChildWindow
    {
        #region Variables

            #region Listes
                List<CsProduit> ListeDesProduitDuSite = new List<CsProduit>();
                CsProduit LeProduitSelect = new CsProduit();
                private List<CsCentre> _listeDesCentreExistant = null;
                CsDemandeBase laDemandeSelect = null;

            #endregion

            #region Objets Globaux
        
            CsClient LeClient = new CsClient();
            CsClient  LeClientRecherche = new CsClient();
            CsDenomination lacivilite = new CsDenomination();      
            CsCentre LeCentreSelect = new CsCentre();
            bool IsUpate = false;
            public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
            public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
            private byte[] image;
            private UcImageScanne formScanne = null;

            #endregion

        #endregion

        #region Constructeurs

        
            public FrmModicationClient()
            {
                InitializeComponent();
                prgBar.Visibility = Visibility.Collapsed ;
                Txt_CodeTarif.IsEnabled  = false;
                btn_tarifs.IsEnabled = false;
                lbl_Tarif.IsEnabled = false;
            }

            string Tdem = string.Empty;
            CsDemande laDetailDemande = null;
            string CodeProduit = string.Empty;
            bool IsRejeterDemande = false;

            public FrmModicationClient(string _typeDemande, string IsInit)
            {
                //Intialisation du des contrôles de l'UI
                Tdem = _typeDemande;            
                InitializeComponent();
                this.Txt_CodeCivilite.MaxLength = SessionObject.Enumere.TailleCodeCivilite;
                ChargerCategorie();
                ChargerCodeConsomateur();
                ChargerNationnalite();
                ChargerNatureClient();
                ChargerFermable();
                ChargerCivilite();
                ChargerDonneeDuSite();
                ChargerTypeDocument();
                Translate();
                this.Txt_ReferenceClient.MaxLength = SessionObject.Enumere.TailleClient;
                Txt_Addresse1.MaxLength = 30;


                this.label1Matricule.Visibility = System.Windows.Visibility.Collapsed;
                this.Txt_Matricule .Visibility = System.Windows.Visibility.Collapsed;

                Txt_CodeTarif.IsEnabled = false;
                btn_tarifs.IsEnabled = false;
                lbl_Tarif.IsEnabled = false;
            }
            public FrmModicationClient(int iddemande)
            {
                //Intialisation du des contrôles de l'UI
          
                InitializeComponent();

                this.Txt_CodeCivilite.MaxLength = SessionObject.Enumere.TailleCodeCivilite;
                ChargerCategorie();
                ChargerCodeConsomateur();
                ChargerNationnalite();
                ChargerNatureClient();
                ChargerFermable();
                ChargerCivilite();
                ChargerDonneeDuSite();
                ChargerTypeDocument();
                ChargeDetailDEvis(iddemande);
                Translate();
                this.Txt_ReferenceClient.MaxLength = SessionObject.Enumere.TailleClient;
            }
        #endregion
        void Translate()
        {

            // Gestion de la langue
            this.lbl_categoie.Content = Langue.lbl_categoie;
            this.lbl_CodeConsomateur.Content = Langue.lbl_consommation;
            this.lbl_CodeRegroupement.Content = Langue.lbl_CodeRegroupement;
            this.lbl_CodeRelance.Content = Langue.lbl_CodeRelance;
            this.lbl_Nationnalite.Content = Langue.lbl_Nationnalite;
            this.lbl_Nom .Content = Langue.lbl_name;

            //
        }
        List<CsCentre> LstCentre = new List<CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        List<int> lstIdCentre = new List<int>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lstIdCentre.Add(item.PK_ID);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lstIdCentre.Add(item.PK_ID);
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 
        private void ChargerCategorie()
        {
            try
            {
                if (SessionObject.LstCategorie.Count != 0)
                    return;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneCategorieCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCategorie = args.Result;
                    };
                    service.RetourneCategorieAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerFermable()
        {
            try
            {
                if (SessionObject.LstFermable.Count != 0)
                    return ;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneFermableCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                            SessionObject.LstFermable = args.Result;
                    };
                    service.RetourneFermableAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerNatureClient()
        {
            try
            {
                if (SessionObject.LstNatureClient.Count != 0)
                    return ;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneNatureCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstNatureClient = args.Result;
                    };
                    service.RetourneNatureAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerCivilite()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneListeDenominationAllCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCivilite = args.Result;
                };
                service.RetourneListeDenominationAllAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerNationnalite()
        {
            try
            {
                if (SessionObject.LstDesNationalites.Count != 0)
                    return ;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneNationnaliteCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstDesNationalites = args.Result;
                    };
                    service.RetourneNationnaliteAsync();
                    service.CloseAsync();
                }
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
                if (SessionObject.LstCodeRegroupement.Count != 0)
                    return ;
                else
                {
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
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ChargerCodeConsomateur()
        {
            try
            {
                if (SessionObject.LstCodeConsomateur.Count != 0)
                    return;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneCodeConsomateurCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeConsomateur = args.Result;
                    };
                    service.RetourneCodeConsomateurAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void InitialiseCtrl()
        {
            try
            {
            }
            catch (Exception EX)
            {
                Message.ShowError(EX.Message, Langue.lbl_Menu);
            }

        }

        private void AfficherInformationClient(CsClient _LeClient,CsAbon leAbon )
        {
            this.Txt_NomClientAbon.Text = string.IsNullOrEmpty(_LeClient.NOMABON) ? string.Empty : _LeClient.NOMABON;
            if (string.IsNullOrEmpty(this.Txt_NomClientAbon.Text))
                this.Txt_NomClientAbon.Text = (string.IsNullOrEmpty(_LeClient.DENABON) ? string.Empty : _LeClient.DENABON) + (string.IsNullOrEmpty(_LeClient.NOMABON) ? string.Empty : _LeClient.NOMABON);
            this.Txt_telephone.Text = string.IsNullOrEmpty(_LeClient.TELEPHONE) ? string.Empty : _LeClient.TELEPHONE;
            this.Txt_Addresse1.Text = string.IsNullOrEmpty(_LeClient.ADRMAND1) ? string.Empty : _LeClient.ADRMAND1;
            this.Txt_adresse2.Text = string.IsNullOrEmpty(_LeClient.ADRMAND2) ? string.Empty : _LeClient.ADRMAND2;
            this.Txt_CodeConsomateur.Text = string.IsNullOrEmpty(_LeClient.CODECONSO) ? string.Empty : _LeClient.CODECONSO;
            this.Txt_CodeCategorie.Text = string.IsNullOrEmpty(_LeClient.CATEGORIE) ? string.Empty : _LeClient.CATEGORIE;
            this.Txt_CodeFermableClient.Text = string.IsNullOrEmpty(_LeClient.CODERELANCE) ? string.Empty : _LeClient.CODERELANCE;
            this.Txt_CodeNationalite.Text = string.IsNullOrEmpty(_LeClient.NATIONNALITE) ? string.Empty : _LeClient.NATIONNALITE.PadLeft(SessionObject.Enumere.TailleCodeNationalite, '0');
            //this.Txt_Nationnalite.Text = string.IsNullOrEmpty(_LeClient.NATIONNALITE) ? string.Empty : _LeClient.NATIONNALITE;
            this.Txt_CodeCivilite.Text = string.IsNullOrEmpty(_LeClient.DENABON) ? string.Empty : _LeClient.DENABON.PadLeft(SessionObject.Enumere.TailleCodeCivilite, '0');
            this.Txt_CodeRegroupement.Text = string.IsNullOrEmpty(_LeClient.REGROUPEMENT) ? string.Empty : _LeClient.REGROUPEMENT;
            if (leAbon != null)
            {
                this.Txt_CodeTarif.Text = string.IsNullOrEmpty(leAbon.TYPETARIF) ? string.Empty : leAbon.TYPETARIF;
                this.Txt_LibelleTarif.Text = string.IsNullOrEmpty(leAbon.LIBELLETARIF) ? string.Empty : leAbon.LIBELLETARIF;
            }
        }
        private void ActionControle(bool Etat)
        {
            this.btn_Categorie.IsEnabled = !Etat;
            this.btn_CodeConsomateur.IsEnabled = !Etat;
            this.btn_CodeRegroupement.IsEnabled = !Etat;
            this.btn_Nationalite.IsEnabled = !Etat;
            this.btn_FermableClient.IsEnabled = !Etat;
            this.btn_Categorie.IsEnabled = !Etat;
            this.btn_Civilite.IsEnabled = !Etat;

            this.Txt_ReferenceClient.IsReadOnly = Etat;
            this.Txt_NomClientAbon.IsReadOnly = Etat;
            this.Txt_Addresse1.IsReadOnly = Etat;
            this.Txt_adresse2.IsReadOnly = Etat;
            this.Txt_CodeConsomateur.IsReadOnly = Etat;
            this.Txt_LibelleCodeConso.IsReadOnly = Etat;
            this.Txt_CodeFermableClient.IsReadOnly = Etat;
            this.Txt_LibelleFermable.IsReadOnly = Etat;
            this.Txt_CodeCategorie.IsReadOnly = Etat;
            this.Txt_LibelleCategorie.IsReadOnly = Etat;

            this.Txt_CodeRegroupement.IsReadOnly = Etat;
            this.Txt_LibelleGroupeCode.IsReadOnly = Etat;
            this.Txt_Nationnalite.IsReadOnly = Etat;
            this.Txt_CodeNationalite.IsReadOnly = Etat;
            this.Txt_Civilite.IsReadOnly = Etat;
            this.Txt_CodeCivilite.IsReadOnly = Etat;

        }

        private void btn_CodeConsomateur_Click(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LstCodeConsomateur .Count != 0)
            {
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstCodeConsomateur);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste des codes consommateur");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnConsomateur);
                this.IsEnabled = false;
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnConsomateur(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsCodeConsomateur _LeCodeSelect = (CsCodeConsomateur)ctrs.MyObject;
                this.Txt_CodeConsomateur.Text = _LeCodeSelect.CODE;
            }
        }

        private void btn_FermableClient_Click(object sender, RoutedEventArgs e)
        {

            if (SessionObject.LstFermable.Count != 0)
            {
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstFermable);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnFermableClient);
                this.IsEnabled = false;
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnFermableClient(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsFermable _LaFermable = (CsFermable)ctrs.MyObject;
                this.Txt_CodeFermableClient.Text = _LaFermable.CODE;
            }
        }

        private void btn_Categorie_Click(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LstCategorie.Count != 0)
            {
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstCategorie);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnCategorie);
                this.IsEnabled = false;
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnCategorie(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsCategorieClient _LaCateg = (CsCategorieClient)ctrs.MyObject;
                this.Txt_CodeCategorie.Text = _LaCateg.CODE;
                if (laDetailDemande.Abonne.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                {
                    int idpuissance = SessionObject.LstPuissance.FirstOrDefault(u => u.VALEUR == laDetailDemande.Abonne.PUISSANCE && u.FK_IDPRODUIT == laDetailDemande.Abonne.FK_IDPRODUIT).PK_ID;
                    int? idReglage = laDetailDemande.LstCanalistion.FirstOrDefault().FK_IDREGLAGECOMPTEUR;
                    ChargerPuissanceEtTarif(laDetailDemande.Abonne.FK_IDPRODUIT, idpuissance, _LaCateg.PK_ID, idReglage,null );
                    if (this.Txt_CodeCategorie.Text != _LaCateg.CODE)
                    {
                        Txt_CodeTarif.IsEnabled = true;
                        btn_tarifs.IsEnabled = true;
                        lbl_Tarif.IsEnabled = true;
                    }
                    else
                    {
                        Txt_CodeTarif.IsEnabled = false;
                        btn_tarifs.IsEnabled = false;
                        lbl_Tarif.IsEnabled = false;
                    }
                }
            }
        }

        private void btn_Nationalite_Click(object sender, RoutedEventArgs e)
        {

            if (SessionObject.LstDesNationalites .Count != 0)
            {
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstDesNationalites);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODENATIONNALITE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnNationnalite);
                this.IsEnabled = false;
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnNationnalite(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsNationalite _LaNationnalite = (CsNationalite)ctrs.MyObject;
                this.Txt_CodeNationalite.Text = _LaNationnalite.CODE;
                this.Txt_Nationnalite.Text = _LaNationnalite.LIBELLE;
            }
        }

        private void btn_CodeRegroupement_Click(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LstCodeRegroupement != null && SessionObject.LstCodeRegroupement.Count != 0)
            {
                List<object> _Lstobj = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstCodeRegroupement);
                UcListeGenerique ctr = new UcListeGenerique(_Lstobj, "CODE", "NOM", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClicked);
                this.IsEnabled = false;
                ctr.Show();
            }
        }
        void galatee_OkClicked(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsRegCli _LeRegcli = (CsRegCli)ctrs.MyObject;
                this.Txt_CodeRegroupement.Text = _LeRegcli.CODE;

                
            }
        }

        private void ChargerTypeDocument()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeDocumentCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstTypeDocument.Add(item);
                    }
                    cbo_typedoc.ItemsSource = LstTypeDocument;
                    cbo_typedoc.DisplayMemberPath = "LIBELLE";
                    cbo_typedoc.SelectedValuePath = "PK_ID";

                    //this.Resources.Add("FuelList", LstTypeDocument);

                };
                service.ChargerTypeDocumentAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (cbo_typedoc.SelectedItem != null)
            {
                // Create an instance of the open file dialog box.
                var openDialog = new OpenFileDialog();
                // Set filter options and filter index.
                openDialog.Filter =
                    "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                openDialog.FilterIndex = 1;
                openDialog.Multiselect = false;
                // Call the ShowDialog method to show the dialog box.
                bool? userClickedOk = openDialog.ShowDialog();
                // Process input if the user clicked OK.
                if (userClickedOk == true)
                {
                    if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                    {
                        FileStream stream = openDialog.File.OpenRead();
                        var memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        image = memoryStream.GetBuffer();
                        formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                        formScanne.Closed += new EventHandler(GetInformationFromChildWindowImagePreuve);
                        formScanne.Show();
                    }
                }
            }
        }

        private void GetInformationFromChildWindowImagePreuve(object sender, EventArgs e)
        {
            this.LstPiece.Add(new ObjDOCUMENTSCANNE { PK_ID = Guid.NewGuid(), NOMDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).LIBELLE, FK_IDTYPEDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).PK_ID, CONTENU = image, DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule });
            this.dgListePiece.ItemsSource = this.LstPiece;
        }

        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
        }
        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    ObjDOCUMENTSCANNE Fraix = (ObjDOCUMENTSCANNE)dgListePiece.SelectedItem;
                    this.LstPiece.Remove(Fraix);
                    this.dgListePiece.ItemsSource = this.LstPiece;
                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }

        private void cbo_typedoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void EnregistrerDemande(CsDemande LaDemande)
        {
            try
            {

                if (((CsCategorieClient )this.Txt_CodeCategorie.Tag).CODE == SessionObject.Enumere.CategorieAgentEdm )
                {
                    if (string.IsNullOrEmpty(this.Txt_Matricule.Text))
                    {
                        Message.ShowInformation("Le matricule est obligatoire pour les agents EDM", "Acceuil");
                        return;
                    }
                }

                //LaDemande.LeClient.DENABON = string.IsNullOrEmpty(this.Txt_CodeCivilite.Text) ? string.Empty : this.Txt_CodeCivilite.Text;
                LaDemande.LeClient.NOMABON = string.IsNullOrEmpty(this.Txt_NomClientAbon.Text) ? string.Empty : this.Txt_NomClientAbon.Text;
                LaDemande.LeClient.ADRMAND1 = string.IsNullOrEmpty(this.Txt_Addresse1.Text) ? string.Empty : this.Txt_Addresse1.Text;
                LaDemande.LeClient.ADRMAND2 = string.IsNullOrEmpty(this.Txt_adresse2.Text) ? string.Empty : this.Txt_adresse2.Text;
                LaDemande.LeClient.TELEPHONE = string.IsNullOrEmpty(this.Txt_telephone.Text) ? string.Empty : this.Txt_telephone.Text;
                LaDemande.LeClient.CODECONSO = string.IsNullOrEmpty(this.Txt_CodeConsomateur.Text) ? string.Empty : this.Txt_CodeConsomateur.Text;
                LaDemande.LeClient.CATEGORIE = string.IsNullOrEmpty(this.Txt_CodeCategorie.Text) ? string.Empty : this.Txt_CodeCategorie.Text;
                LaDemande.LeClient.CODERELANCE = string.IsNullOrEmpty(this.Txt_CodeFermableClient.Text) ? "0" : this.Txt_CodeFermableClient.Text;
                LaDemande.LeClient.NATIONNALITE = string.IsNullOrEmpty(this.Txt_CodeNationalite.Text) ? string.Empty : this.Txt_CodeNationalite.Text;
                LaDemande.LeClient.DENABON = string.IsNullOrEmpty(this.Txt_CodeCivilite.Text) ? string.Empty : this.Txt_CodeCivilite.Text;
                LaDemande.LeClient.REGROUPEMENT = string.IsNullOrEmpty(this.Txt_CodeRegroupement.Text) ? string.Empty : this.Txt_CodeRegroupement.Text;
                LaDemande.LeClient.MATRICULE = string.IsNullOrEmpty(this.Txt_Matricule.Text) ? string.Empty : this.Txt_Matricule.Text;

                LaDemande.LeClient.FK_TYPECLIENT = (int)this.Txt_CodeFermableClient.Tag; 
                LaDemande.LeClient.FK_IDCODECONSO = (int)this.Txt_CodeConsomateur.Tag;
                LaDemande.LeClient.FK_IDCATEGORIE = ((CsCategorieClient )this.Txt_CodeCategorie.Tag).PK_ID ;
                LaDemande.LeClient.FK_IDNATIONALITE = (int)this.Txt_CodeNationalite.Tag;
                LaDemande.LeClient.FK_IDREGROUPEMENT = null;
                if (this.Txt_CodeRegroupement.Tag != null)
                    LaDemande.LeClient.FK_IDREGROUPEMENT = (int)this.Txt_CodeRegroupement.Tag;
                LaDemande.LeClient.FK_IDUSAGE = null;
                LaDemande.LeClient.FK_IDPIECEIDENTITE  = null;

                LaDemande.LeClient.USERCREATION = UserConnecte.matricule;
                LaDemande.LeClient.USERMODIFICATION = UserConnecte.matricule;
                LaDemande.LeClient.DATECREATION =  System.DateTime.Now;
                LaDemande.LeClient.DATEMODIFICATION =  System.DateTime.Now;


                if (LaDemande.LaDemande == null) LaDemande.LaDemande = new CsDemandeBase();
                LaDemande.LaDemande.CLIENT = this.Txt_ReferenceClient.Text;
                LaDemande.LaDemande.MATRICULE = UserConnecte.matricule;
                LaDemande.LaDemande.CENTRE = LaDemande.LeClient.CENTRE;
                LaDemande.LaDemande.CLIENT = LaDemande.LeClient.REFCLIENT ;
                LaDemande.LaDemande.ORDRE = LaDemande.LeClient.ORDRE ;
                LaDemande.LaDemande.PRODUIT = LaDemande.LeClient.PRODUIT;
                LaDemande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusEnAttente;
                LaDemande.LaDemande.TYPEDEMANDE = Tdem ;
                LaDemande.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem) != null ? SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).PK_ID : 0;
                LaDemande.LaDemande.FK_IDCENTRE = LaDemande.LeClient.FK_IDCENTRE.Value  ;
                LaDemande.LaDemande.FK_IDPRODUIT = (LaDemande.Abonne != null && LaDemande.Abonne.FK_IDPRODUIT != null) ? LaDemande.Abonne.FK_IDPRODUIT : LaDemande.LaDemande.FK_IDPRODUIT;
                LaDemande.LaDemande.PRODUIT = (LaDemande.Abonne != null && LaDemande.Abonne.PRODUIT != null) ? LaDemande.Abonne.PRODUIT : LaDemande.LaDemande.PRODUIT;


                LaDemande.LaDemande.USERCREATION = UserConnecte.matricule;
                LaDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                LaDemande.LaDemande.DATECREATION = System.DateTime.Now;
                LaDemande.LaDemande.DATEMODIFICATION = System.DateTime.Now;
                LaDemande.LaDemande.MOTIF  = this.Txt_Motif.Text ;

                if (this.Txt_CodeTarif.Text != LaDemande.Abonne.TYPETARIF)
                {
                    LaDemande.Abonne.TYPETARIF = this.Txt_CodeTarif.Text;
                    LaDemande.Abonne.FK_IDTYPETARIF  =(int)this.Txt_CodeTarif.Tag ;
                }
                else
                    LaDemande.Abonne = null;

                #region Doc Scanne
                if (LaDemande.ObjetScanne == null) LaDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                LaDemande.ObjetScanne.AddRange(LstPiece);
                #endregion
                LaDemande.Branchement = null;
                LaDemande.LstCanalistion = null;
                LaDemande.LstEvenement = null;
                LaDemande.Ag  = null;
                ValidationDemande(LaDemande);
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur à la récupération des données", "Information");
            }
        }

        private void Txt_CodeCategorie_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeCategorie.Text.Length == SessionObject.Enumere.TailleCodeCategorie
                    && SessionObject.LstCategorie .Count != 0)
                {

                    CsCategorieClient LaCategorie = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstCategorie, Txt_CodeCategorie.Text, "CODE");
                    if (!string.IsNullOrEmpty(LaCategorie.LIBELLE))
                    {
                        this.Txt_LibelleCategorie.Text = LaCategorie.LIBELLE;
                        this.Txt_CodeCategorie.Tag = LaCategorie;

                        if (LaCategorie.CODE == SessionObject.Enumere.CategorieAgentEdm)
                        {
                            this.label1Matricule.Visibility = System.Windows.Visibility.Visible;
                            this.Txt_Matricule.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            this.label1Matricule.Visibility = System.Windows.Visibility.Collapsed ;
                            this.Txt_Matricule.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeCategorie.Focus();
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

        private void Txt_CodeConsomateur_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeConsomateur.Text.Length == SessionObject.Enumere.TailleCodeConso
                    && SessionObject.LstCodeConsomateur .Count != 0)
                {
                    CsCodeConsomateur _leCodeConso = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstCodeConsomateur, this.Txt_CodeConsomateur.Text, "CODE");
                    if (!string.IsNullOrEmpty(_leCodeConso.LIBELLE))
                    {
                        this.Txt_LibelleCodeConso.Text = _leCodeConso.LIBELLE;
                        this.Txt_CodeConsomateur.Tag = _leCodeConso.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeConsomateur.Focus();
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

        private void Txt_CodeNationalite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeNationalite.Text.Length == SessionObject.Enumere.TailleCodeNationalite &&
                    SessionObject.LstDesNationalites .Count != 0)
                {
                    CsNationalite _leCodeNation = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstDesNationalites, this.Txt_CodeNationalite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_leCodeNation.LIBELLE))
                    {
                        this.Txt_Nationnalite.Text = _leCodeNation.LIBELLE;
                        this.Txt_CodeNationalite.Tag = _leCodeNation.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeNationalite.Focus();
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


        private void Txt_CodeFermableClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeFermableClient.Text.Length == SessionObject.Enumere.TailleCodeRelance &&
                    SessionObject.LstFermable .Count != 0)
                {
                    CsFermable _Fermable = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstFermable, this.Txt_CodeFermableClient.Text, "CODE");
                    if (!string.IsNullOrEmpty(_Fermable.LIBELLE))
                    {
                        this.Txt_LibelleFermable.Text = _Fermable.LIBELLE;
                        this.Txt_CodeFermableClient.Tag = _Fermable.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeFermableClient.Focus();
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

        private void Txt_CodeRegroupement_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeRegroupement.Text.Length == SessionObject.Enumere.TailleCodeRegroupement
                               && SessionObject.LstCodeRegroupement.Count != 0)
            {

                CsRegCli LeRegroupement = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstCodeRegroupement, Txt_CodeRegroupement.Text, "CODE");
                if (LeRegroupement != null && !string.IsNullOrEmpty(LeRegroupement.LIBELLE))
                {
                    this.Txt_LibelleGroupeCode.Text = LeRegroupement.NOM;
                    this.Txt_CodeRegroupement.Tag = LeRegroupement.PK_ID;
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_CodeRegroupement.Focus();
                    };
                    w.Show();
                }
            }

        }
        private void HandleLostFocus<T>(TextBox Code, TextBox Libelle, List<T> listItems, int Taille)
        {
            if (!string.IsNullOrEmpty(Code.Text) &&
                Code.Text.Length == Taille &&
                listItems.Count != 0)
            {
                Code.Text = Code.Text.PadLeft(Taille, '0');
            }
            else
            {
                Code.Text = string.Empty;
                Libelle.Text = string.Empty;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
        public event EventHandler Closed;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.OKButton.IsEnabled = false;
            EnregistrerDemande(laDetailDemande);
            if (Closed != null)
                Closed(this, new EventArgs());
        }
        private void Txt_CodeCiviliteAgent_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void ValidationDemande(CsDemande _LaDemande)
        {
            try
            {
                #region creation demande
                _LaDemande.LaDemande.MATRICULE = UserConnecte.matricule;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.CreeDemandeCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (b.Result != null)
                    {
                        Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + b.Result.NUMDEM,
                        Silverlight.Resources.Devis.Languages.txtDevis);
                        this.DialogResult = false;
                    }
                    else
                        Message.ShowError("Une erreur s'est produite à la création de la demande ", "CreeDemande");
                };
                client.CreeDemandeAsync(_LaDemande, true);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChildWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            InitialiseCtrl();
        }
     
        private void Txt_Client_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_ReferenceClient.Text))
                this.Txt_ReferenceClient.Text = this.Txt_ReferenceClient.Text.PadLeft(SessionObject.Enumere.TailleClient, '0');
        }

        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient)
                    ChargerClientFromReference(this.Txt_ReferenceClient.Text);
                else
                {
                    Message.Show("La référence saisie n'est pas correcte", "Infomation");
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }

        private void ChargerClientFromReference(string ReferenceClient)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneClientByReferenceCompleted += (s, args) =>
                {
                    prgBar.Visibility = Visibility.Collapsed;
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
                    {
                        CsClient leClient = args.Result.First();
                        leClient.TYPEDEMANDE = Tdem;
                        VerifieExisteDemande(leClient);
                    }
                };
                service.RetourneClientByReferenceAsync(ReferenceClient, lstIdCentre);
                service.CloseAsync();

            }
            catch (Exception)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError("Erreur au chargement des données", "Demande");
            }
        }

        private void galatee_OkClickedChoixClient(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsClient _UnClient = (CsClient)ctrs.MyObject;
                _UnClient.TYPEDEMANDE = Tdem;
                VerifieExisteDemande(_UnClient);
            }
        }

        private void VerifieExisteDemande(CsClient leClient)
        {

            try
            {
                if (!string.IsNullOrEmpty(Txt_ReferenceClient.Text) && Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient)
                {
                    prgBar.Visibility = System.Windows.Visibility.Visible;
                    string OrdreMax = string.Empty;
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneDemandeClientTypeCompleted += (s, args) =>
                    {
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result != null)
                        {
                            if (args.Result.DATEFIN == null && args.Result.ISSUPPRIME != true)
                            {
                                Message.ShowInformation("Il existe une demande numéro " + args.Result.NUMDEM + " sur ce client", "Accueil");
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
                prgBar.Visibility = System.Windows.Visibility.Visible;
                leclient.TYPEDEMANDE = Tdem;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.ChargerDetailClientCompleted  += (ssender, args) =>
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
                    else
                    {
                        laDetailDemande = new CsDemande();
                        laDetailDemande = args.Result;
                        if (laDetailDemande.Abonne != null && laDetailDemande.Abonne.DRES == null)
                        {
                            this.txtSite.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLESITE) ? string.Empty : laDetailDemande.LeClient.LIBELLESITE;
                            this.txtCentre.Text = string.IsNullOrEmpty(laDetailDemande.LeClient.LIBELLECENTRE) ? string.Empty : laDetailDemande.LeClient.LIBELLECENTRE;
                            this.txt_Produit.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEPRODUIT) ? string.Empty : laDetailDemande.Abonne.LIBELLEPRODUIT;
                            this.txt_Produit.Tag = laDetailDemande.Abonne.FK_IDPRODUIT;
                            this.txtCentre.Tag = laDetailDemande.Abonne.FK_IDCENTRE;
                            CodeProduit = laDetailDemande.Abonne.PRODUIT;
                            this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).LIBELLE;
                            txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem);
                            AfficherInformationClient(laDetailDemande.LeClient,laDetailDemande.Abonne);
                            AfficherDocumentScanne(laDetailDemande.ObjetScanne);
                        }
                        else
                        {
                            Message.ShowInformation("Ce abonné est résilié", "Info");
                            return;
                        }
                    }
                };
                client.ChargerDetailClientAsync (leclient);
            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError("Erreur au chargement des donnéés", "Demande");
            }
        }
        List<CsTarif> lstDesTarif = new List<CsTarif>();
        List<CsTarif> LstPuissanceTarif = new List<CsTarif>();

        private void ChargerPuissanceEtTarif(int idProduit, int? idPuissance, int? idCategorie, int? idReglageCompteur,int ? idtarif)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerTypeTarifCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                LstPuissanceTarif = args.Result;
                if (lstDesTarif == null) lstDesTarif = new List<CsTarif>();
                if (lstDesTarif.Count != 0) lstDesTarif.Clear();

                foreach (CsTarif item in LstPuissanceTarif)
                    lstDesTarif.Add(new CsTarif { PK_ID = item.PK_ID, CODE = item.CODE, LIBELLE = item.LIBELLE });
                if (lstDesTarif != null && lstDesTarif.Count == 1)
                {
                    this.Txt_CodeTarif.Text = lstDesTarif.First().CODE;
                    this.Txt_LibelleTarif.Text = lstDesTarif.First().LIBELLE;
                    this.Txt_CodeTarif.Tag = lstDesTarif.First().PK_ID;
                }
                else
                {
                    this.Txt_CodeTarif.Text = string.Empty;
                    this.Txt_LibelleTarif.Text = string.Empty;
                    this.Txt_CodeTarif.Tag = null;
                }
                if (string.IsNullOrEmpty(this.Txt_CodeTarif.Text))
                    Message.ShowInformation("Sélectionnez le tarif", "Demande");
            };
            service.ChargerTypeTarifAsync(idProduit, idPuissance, idCategorie, idReglageCompteur, idtarif);
            service.CloseAsync();

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
                    laDetailDemande = new CsDemande();
                    laDetailDemande = args.Result;
                    this.txtSite.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLESITE) ? string.Empty : laDetailDemande.LaDemande.LIBELLESITE;
                    this.txtCentre.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLECENTRE) ? string.Empty : laDetailDemande.LaDemande.LIBELLECENTRE;
                    this.Txt_ReferenceClient.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? string.Empty : laDetailDemande.LaDemande.CLIENT;
                    this.txt_Produit.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.LIBELLEPRODUIT) ? string.Empty : laDetailDemande.LaDemande.LIBELLEPRODUIT;
                    this.txt_Produit.Tag  = laDetailDemande.LaDemande.FK_IDPRODUIT ;
                    this.txtCentre.Tag = laDetailDemande.LaDemande.FK_IDCENTRE;
                    CodeProduit = laDetailDemande.LaDemande.PRODUIT;
                    Tdem = laDetailDemande.LaDemande.TYPEDEMANDE;
                    this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).LIBELLE;
                    txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem);

                    this.Txt_Motif.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.MOTIF) ? string.Empty : laDetailDemande.LaDemande.MOTIF;
                    //if (laDetailDemande.LstCommentaire != null && laDetailDemande.LstCommentaire.Count != 0)
                    if (laDetailDemande.AnnotationDemande != null && laDetailDemande.AnnotationDemande.Count != 0)
                        this.Txt_MotifRejet.Text = laDetailDemande.AnnotationDemande.First().COMMENTAIRE;


                    AfficherInformationClient(laDetailDemande.LeClient,laDetailDemande.Abonne  );
                    AfficherDocumentScanne(laDetailDemande.ObjetScanne);
                    IsRejeterDemande = true;
                }

            };
            client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
        }

        private void AfficherDocumentScanne(List<ObjDOCUMENTSCANNE> _LesDocScanne)
        {
            try
            {
                if (_LesDocScanne != null && _LesDocScanne.Count != 0)
                {
                    this.dgListePiece.ItemsSource = null;
                    this.dgListePiece.ItemsSource = _LesDocScanne;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void Txt_CodeTarif_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Txt_CodeTarif.Text.Length != 0)
                this.Txt_CodeTarif.Text = this.Txt_CodeTarif.Text.PadLeft(SessionObject.Enumere.TailleTarif, '0');
        }
        private void btn_tarifs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstDesTarif.Count != 0)
                {
                    List<object> _LstObjet = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstDesTarif);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObjet, "CODE", "LIBELLE", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnTarif);
                    this.IsEnabled = false;
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void galatee_OkClickedBtnTarif(object sender, EventArgs e)
        {
            try
            {
                this.IsEnabled = true;
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsTarif _LeTarif = (CsTarif)ctrs.MyObject;
                    this.Txt_CodeTarif.Text = _LeTarif.CODE;
                }

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }

        private void Txt_CodeCivilite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeCivilite.Text.Length == SessionObject.Enumere.TailleCodeCivilite &&
                    SessionObject.LstCivilite.Count != 0)
                {
                    CsDenomination _leCode = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstCivilite, this.Txt_CodeCivilite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_leCode.LIBELLE))
                    {
                        this.Txt_Civilite.Text = _leCode.LIBELLE;
                        this.Txt_CodeCivilite.Tag = _leCode.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeCivilite.Focus();
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

        private void btn_Civilite_Click(object sender, RoutedEventArgs e)
        {

            if (SessionObject.LstDesNationalites.Count != 0)
            {
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstCivilite);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnCivilite);
                this.IsEnabled = false;
                ctr.Show();
            }
        }

        void galatee_OkClickedBtnCivilite(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsDenomination _La = (CsDenomination)ctrs.MyObject;
                this.Txt_CodeCivilite.Text = _La.CODE;
                this.Txt_Civilite.Text = _La.LIBELLE;
            }
        }
    }
}


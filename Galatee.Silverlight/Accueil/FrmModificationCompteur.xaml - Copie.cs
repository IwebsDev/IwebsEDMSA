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
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using System.IO;
using System.Collections.ObjectModel;


namespace Galatee.Silverlight.Accueil
{
    public partial class FrmModificationCompteur : ChildWindow
    {
        public FrmModificationCompteur()
        {
            InitializeComponent();
        }
        List<CsTcompteur> LstTypeCompteur = new List<CsTcompteur>();
        List<CsCalibreCompteur> LstCalibreCompteur = new List<CsCalibreCompteur>();
        List<CsMarqueCompteur> LstMarque = new List<CsMarqueCompteur>();
        List<CsCadran> LstCadran = new List<CsCadran>();

        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        private byte[] image;
        private UcImageScanne formScanne = null;


        CsCanalisation CanalisationAfficher = new CsCanalisation();
        decimal initValue = 0;
        string Tdem = string.Empty;
        CsDemande laDetailDemande = null;
        string CodeProduit = string.Empty;
           bool IsRejeterDemande = false;
        public FrmModificationCompteur(string _TypeDemande,string Init)
        {
            InitializeComponent();
            Translate();
            Tdem = _TypeDemande;
            ChargerMarque();
            ChargerTypeCompteur();
            ChargerDiametreCompteur();
            ChargerTypeDocument();
            ChargerDonneeDuSite();
            this.Txt_ReferenceClient.MaxLength = SessionObject.Enumere.TailleClient;
        }
       public FrmModificationCompteur(int iddemande)
        {
            InitializeComponent();
            Translate();
            ChargerMarque();
            ChargerTypeCompteur();
            ChargerDiametreCompteur();
            ChargerTypeDocument();
            ChargerDonneeDuSite();
            ChargeDetailDEvis(iddemande);

            this.Txt_ReferenceClient.MaxLength = SessionObject.Enumere.TailleClient;
        }
        void Translate()
        {
            this.lbl_Diametre.Content = Langue.lbl_diametre;
            this.lbl_Localisation.Content = Langue.lbl_Localisation;
            this.lbl_Marque.Content = Langue.lbl_Marque;
            this.lbl_NumeroCompteur.Content = Langue.lbl_NumeroCompteur;
            this.lbl_cadran.Content = Langue.lbl_cadran;
            this.lbl_type.Content = Langue.lbl_type;
            this.lbl_AnneFabrication.Content = Langue.lbl_AnneFabrication;
        }

        void InitialiseCtrl()
        {
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
        private void ChargerTypeDocument()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Ête-vous sure de vouloire supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
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

        void ChargerDiametreCompteur()
        {
            try
            {
                if (SessionObject.LstCalibreCompteur .Count != 0)
                    LstCalibreCompteur = SessionObject.LstCalibreCompteur;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerCalibreCompteurCompleted  += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstCalibreCompteur = args.Result;
                        SessionObject.LstCalibreCompteur = LstCalibreCompteur;
                    };
                    service.ChargerCalibreCompteurAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void ChargerMarque()
        {
            try
            {
                if (SessionObject.LstMarque.Count != 0)
                    LstMarque = SessionObject.LstMarque;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneToutMarqueCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstMarque = args.Result;
                        SessionObject.LstMarque = LstMarque;
                    };
                    service.RetourneToutMarqueAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void ChargerTypeComptage()
        {
            try
            {
                if (SessionObject.LstTypeComptage != null && SessionObject.LstTypeComptage.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerTypeComptageCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTypeComptage = args.Result;
                };
                service.ChargerTypeComptageAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void ChargerTypeCompteur()
        {
            try
            {
                if (SessionObject.LstTypeCompteur.Count  != 0)
                    LstTypeCompteur = SessionObject.LstTypeCompteur;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstTypeCompteur = args.Result;
                        SessionObject.LstTypeCompteur = LstTypeCompteur;
                    };
                    service.ChargerTypeAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitialiseCtrl();
        }
        private void RetourneInfoCompteur(CsClient leClient)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GeDetailByFromClientCompleted += (ss, args) =>
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
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.GeDetailByFromClientAsync(leClient);
            }
            catch (Exception)
            {

                throw;
            }
        }
        void AfficherCannalisationDemande(CsCanalisation LaCanalisation)
        {
                this.Txt_ReferenceClient.Text = (string.IsNullOrEmpty(LaCanalisation.CLIENT)) ? string.Empty : LaCanalisation.CLIENT;

                this.Txt_NumCompteur.Text = (string.IsNullOrEmpty(LaCanalisation.NUMERO)) ? string.Empty : LaCanalisation.NUMERO;
                this.Txt_AnneeFab.Text = (string.IsNullOrEmpty(LaCanalisation.ANNEEFAB)) ? string.Empty : LaCanalisation.ANNEEFAB;

                if (LaCanalisation.PRODUIT != SessionObject.Enumere.ElectriciteMT )
                    this.Txt_LibelleDiametre.Text = (string.IsNullOrEmpty(LaCanalisation.CODECALIBRECOMPTEUR)) ? string.Empty : LaCanalisation.CODECALIBRECOMPTEUR;
                else
                this.Txt_LibelleDiametre.Text = (string.IsNullOrEmpty(LaCanalisation.LIBELLETYPECOMPTAGE)) ? string.Empty : LaCanalisation.LIBELLETYPECOMPTAGE;

                this.Txt_LibelleMarque.Text = (string.IsNullOrEmpty(LaCanalisation.LIBELLEMARQUE)) ? string.Empty : LaCanalisation.LIBELLEMARQUE;

                this.Txt_CodeTypeCompteur.Text = (LstTypeCompteur.FirstOrDefault(c => c.PK_ID == LaCanalisation.FK_IDTYPECOMPTEUR) == null) ? string.Empty : LstTypeCompteur.FirstOrDefault(c => c.PK_ID == LaCanalisation.FK_IDTYPECOMPTEUR).CODE;
                this.Txt_CodeMarque.Text = (LstMarque.FirstOrDefault(c => c.PK_ID == LaCanalisation.FK_IDMARQUECOMPTEUR) == null) ? string.Empty : LstMarque.FirstOrDefault(c => c.PK_ID == LaCanalisation.FK_IDMARQUECOMPTEUR).CODE;
                if(LaCanalisation.CADRAN != null)
                this.Txt_CodeCadran.Text =  LaCanalisation.CADRAN.Value.ToString();
                this.Txt_LibelleDiametre.Tag = LaCanalisation.FK_IDCALIBRE;
        }

        private void btn_DiametreCompteur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btn_DiametreCompteur.IsEnabled = false;
                if (LstCalibreCompteur.Count != 0)
                {
                    if (CodeProduit  == SessionObject.Enumere.ElectriciteMT)
                    {
                        
                        List<object> _LstObj = new List<object>();
                        _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstTypeComptage);
                        Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                        _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                        List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                        MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                        ctrl.Closed += new EventHandler(galatee_OkClickedBtnDiametre);
                        ctrl.Show();
                    }
                    else
                    {
                        List<object> _LstObj = new List<object>();
                        _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(LstCalibreCompteur.Where(t => t.PRODUIT == CodeProduit).ToList());
                        Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                        _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                        List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                        MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                        ctrl.Closed += new EventHandler(galatee_OkClickedBtnDiametre);
                        ctrl.Show();

                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedBtntypeComptage(object sender, EventArgs e)
        {
            try
            {
                this.btn_DiametreCompteur.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsTypeComptage _LeDiametre = (CsTypeComptage)ctrs.MyObject;
                    this.Txt_LibelleDiametre.Text = _LeDiametre.CODE;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedBtnDiametre(object sender, EventArgs e)
        {
            try
            {
                this.btn_DiametreCompteur.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick )
                {
                    CsCalibreCompteur _LeDiametre = (CsCalibreCompteur)ctrs.MyObject;
                    this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE ;
                    this.Txt_LibelleDiametre.Tag = _LeDiametre.PK_ID;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
      
        private void btn_Marque_Click(object sender, RoutedEventArgs e)
        {
            if (LstMarque.Count != 0)
            {
                this.btn_Marque.IsEnabled = false;
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstMarque);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_Marque);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_Marque(object sender, EventArgs e)
        {
            try
            {
                this.btn_Marque.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick )
                {
                    CsMarqueCompteur _LaMarque = (CsMarqueCompteur)ctrs.MyObject;
                    this.Txt_CodeMarque.Text = _LaMarque.CODE;
                    this.Txt_CodeMarque.Tag = _LaMarque.PK_ID;

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void btn_typeCompteur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstTypeCompteur.Count != 0)
                {
                    this.btn_typeCompteur.IsEnabled = false;
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstTypeCompteur.Where(t=>t.PRODUIT == CodeProduit  ).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "TYPE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedbtntypeCompteur);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedbtntypeCompteur(object sender, EventArgs e)
        {
            try
            {
                this.btn_typeCompteur.IsEnabled = true;
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    CsTcompteur _LeTypeCompteur = (CsTcompteur)ctrs.MyObject;
                    this.Txt_CodeTypeCompteur.Text = _LeTypeCompteur.CODE ;
                    this.Txt_CodeTypeCompteur.Tag = _LeTypeCompteur.PK_ID;

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void Txt_CodeTypeCompteur_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeTypeCompteur.Text.Length == SessionObject.Enumere.TailleCodeTypeCompteur && (LstTypeCompteur != null && LstTypeCompteur.Count != 0))
                {
                    CsTcompteur _LeTypeCompte = ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeCompteur.Where(n => n.PRODUIT == CodeProduit).ToList(), this.Txt_CodeTypeCompteur.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeTypeCompte.LIBELLE))
                    {
                        this.Txt_LibelleTypeClient.Text = _LeTypeCompte.LIBELLE;
                        this.Txt_CodeTypeCompteur.Tag = _LeTypeCompte.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeTypeCompteur.Focus();
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

        private void Txt_Client_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_ReferenceClient.Text))
                this.Txt_ReferenceClient.Text = this.Txt_ReferenceClient.Text.PadLeft(SessionObject.Enumere.TailleClient, '0');
        }

        public event EventHandler Closed;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            CsCompteur leCompteur = new CsCompteur()
            {
                ANNEEFAB = string.IsNullOrEmpty(this.Txt_AnneeFab.Text) ? string.Empty : this.Txt_AnneeFab.Text,
                CADRAN = string.IsNullOrEmpty(this.Txt_CodeCadran.Text) ? Convert.ToByte(6) : Convert.ToByte(this.Txt_CodeCadran.Text),
                NUMERO = string.IsNullOrEmpty(this.Txt_NumCompteur.Text) ? string.Empty : this.Txt_NumCompteur.Text,
                MARQUE = string.IsNullOrEmpty(this.Txt_CodeMarque.Text) ? string.Empty : this.Txt_CodeMarque.Text,
                FK_IDMARQUECOMPTEUR = (int)this.Txt_CodeMarque.Tag,
                TYPECOMPTEUR = string.IsNullOrEmpty(this.Txt_CodeTypeCompteur.Text) ? string.Empty : this.Txt_CodeTypeCompteur.Text
            };
            VerifieCompteurExiste(leCompteur);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitialiseCtrl();
        }

        private void Txt_CodeMarque_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeMarque.Text.Length == SessionObject.Enumere.TailleCodeMarqueCompteur && (LstMarque != null && LstMarque.Count != 0))
                {
                    CsMarqueCompteur _LaMarque = ClasseMEthodeGenerique.RetourneObjectFromList(LstMarque, this.Txt_CodeMarque.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LaMarque.LIBELLE))
                    {
                        this.Txt_LibelleMarque.Text = _LaMarque.LIBELLE;
                        this.Txt_CodeMarque.Tag  = _LaMarque.PK_ID ;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeMarque.Focus();
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
        private void VerifieCompteurExiste(CsCompteur nouveaucompteur)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.VerifieSiCompteurExisteCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                {
                    ValidationDemande(laDetailDemande);
                    this.DialogResult = false;
                }
                else
                    Message.ShowInformation("Ce compteur " + args.Result.NUMERO + " de marque " + args.Result.LIBELLEMARQUE + "\n\r" +
                        " existe déja sur la reférence " + args.Result.CENTRE + " " + args.Result.CLIENT, "Modification compteur");
                this.IsEnabled = true ;
            };
            service.VerifieSiCompteurExisteAsync(nouveaucompteur);
            service.CloseAsync();
        }

        private void ValidationDemande(CsDemande _Lademande)
        {
            try
            {
                foreach (var item in _Lademande.LstCanalistion )
                {
                    item.ANNEEFAB = string.IsNullOrEmpty(this.Txt_AnneeFab.Text) ? string.Empty : this.Txt_AnneeFab.Text;
                    item.CADRAN = string.IsNullOrEmpty(this.Txt_CodeCadran.Text) ? Convert.ToByte(6) : Convert.ToByte(this.Txt_CodeCadran.Text);
                    item.NUMERO  = string.IsNullOrEmpty(this.Txt_NumCompteur .Text) ? string.Empty : this.Txt_NumCompteur.Text;
                    item.MARQUE  = string.IsNullOrEmpty(this.Txt_CodeMarque .Text) ? string.Empty : this.Txt_CodeMarque.Text;
                    item.TYPECOMPTEUR  = string.IsNullOrEmpty(this.Txt_CodeTypeCompteur.Text) ? string.Empty : this.Txt_CodeTypeCompteur.Text;

                    item.FK_IDCALIBRE = null;
                    if (this.Txt_LibelleDiametre.Tag != null)
                       item.FK_IDCALIBRE = (int)this.Txt_LibelleDiametre.Tag;
                    
                    item.FK_IDMARQUECOMPTEUR = (int)this.Txt_CodeMarque.Tag;
                    item.FK_IDTYPECOMPTEUR     =(int)this.Txt_CodeTypeCompteur.Tag;
                    item.USERCREATION = UserConnecte.matricule;
                    item.USERMODIFICATION  = UserConnecte.matricule;
                    item.DATECREATION = System.DateTime.Now;
                    item.DATEMODIFICATION  = System.DateTime.Now;

                }

                if (_Lademande.LaDemande == null) _Lademande.LaDemande = new CsDemandeBase();
                _Lademande.LaDemande.MATRICULE = UserConnecte.matricule;
                _Lademande.LaDemande.CENTRE =_Lademande.Abonne!= null ? _Lademande.Abonne.CENTRE:_Lademande.LaDemande.CENTRE ;
                _Lademande.LaDemande.CLIENT = _Lademande.Abonne != null ? _Lademande.Abonne.CLIENT : _Lademande.LaDemande.CLIENT;
                _Lademande.LaDemande.ORDRE = _Lademande.Abonne != null ? _Lademande.Abonne.ORDRE : _Lademande.LaDemande.ORDRE;
                _Lademande.LaDemande.PRODUIT = _Lademande.Abonne != null ? _Lademande.Abonne.PRODUIT : _Lademande.LaDemande.PRODUIT;
                _Lademande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusEnAttente;
                _Lademande.LaDemande.TYPEDEMANDE = Tdem;
                _Lademande.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem) != null ? SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).PK_ID : 0;
                _Lademande.LaDemande.FK_IDCENTRE = (int)txtCentre.Tag;
                _Lademande.LaDemande.FK_IDPRODUIT = (int)txt_Produit.Tag;

                _Lademande.LaDemande.USERCREATION = UserConnecte.matricule;
                _Lademande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                _Lademande.LaDemande.DATECREATION = System.DateTime.Now;
                _Lademande.LaDemande.DATEMODIFICATION = System.DateTime.Now;
                _Lademande.LaDemande.ISDEMANDEREJETERINIT = false;
                _Lademande.LaDemande.MOTIF = string.IsNullOrEmpty(this.Txt_Motif.Text) ? string.Empty : this.Txt_Motif.Text;

                #region Doc Scanne
                if (_Lademande.ObjetScanne == null) _Lademande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                if (LstPiece.Count != 0)
                    _Lademande.ObjetScanne.AddRange(LstPiece);
                #endregion

                _Lademande.Abonne = null;
                _Lademande.Branchement = null;
                _Lademande.LeClient = null;
                _Lademande.LstEvenement = null;
                _Lademande.EltDevis = null;
                _Lademande.OrdreTravail = null;
                _Lademande.Ag = null;

                //Lancer la transaction de mise a jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ValiderDemandeInitailisationAsync(_Lademande);
                service1.ValiderDemandeInitailisationCompleted += (sr, res) =>
                {
                    if (!string.IsNullOrEmpty(res.Result))
                    {
                        if (!IsRejeterDemande)
                        {
                            string Retour = res.Result;
                            string[] coupe = Retour.Split('.');
                            Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], _Lademande.LaDemande.FK_IDCENTRE, coupe[1], _Lademande.LaDemande.FK_IDTYPEDEMANDE);
                        }
                        else
                        {
                            List<string> codes = new List<string>();
                            codes.Add(laDetailDemande.InfoDemande.CODE);

                            ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
                            //List<CsUtilisateur> leUser = new List<CsUtilisateur>();
                            //if (laDetailDemande.InfoDemande != null && laDetailDemande.InfoDemande.CODE != null)
                            //{
                            //    foreach (CsUtilisateur item in laDetailDemande.InfoDemande.UtilisateurEtapeSuivante)
                            //        leUser.Add(item);
                            //    Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", laDetailDemande.LaDemande.NUMDEM, laDetailDemande.LaDemande.LIBELLETYPEDEMANDE);
                            //}
                        }
                    }
                    if (Closed != null)
                        Closed(this, new EventArgs());
                    this.DialogResult = false;
                };
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        private void Txt_CodeCentre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtCentre.Text))
                this.txtCentre.Text = this.txtCentre.Text.PadLeft(SessionObject.Enumere.TailleOrdre, '0');
        }

        private void btn_Valider_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                if (Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient)
                    ChargerClientFromReference(this.Txt_ReferenceClient.Text);
                else
                {
                    Message.Show("La reference saisie n'est pas correcte", "Infomation");
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneClientByReferenceCompleted += (s, args) =>
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
                    string OrdreMax = string.Empty;
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneDemandeClientTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result != null)
                        {
                            if (args.Result.DATEFIN == null)
                            {
                                if (args.Result.ISSUPPRIME != true)
                                {
                                    Message.ShowInformation("Il existe une demande numero " + args.Result.NUMDEM + " sur ce client", "Accueil");
                                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                    return;
                                }
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
                leclient.TYPEDEMANDE = Tdem;
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

                            if (laDetailDemande.LstCanalistion != null && laDetailDemande.LstCanalistion.Count != 0)
                            {
                                if (laDetailDemande.Abonne.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                                {
                                    this.btn_DiametreCompteur.IsEnabled = false;
                                    this.Txt_LibelleDiametre.IsEnabled = false;
                                    laDetailDemande.LstCanalistion.First().NUMERO = laDetailDemande.LstCanalistion.First().NUMERO.Substring(5, (laDetailDemande.LstCanalistion.First().NUMERO.Length - 5));
                                }
                                AfficherCannalisationDemande(laDetailDemande.LstCanalistion.First());
                                AfficherDocumentScanne(laDetailDemande.ObjetScanne);
                            }
                        }
                        else
                        {
                            Message.ShowInformation("Ce abonné est résilié", "Info");
                            return;
                        }
                    }
                };
                client.GeDetailByFromClientAsync(leclient);
            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError("Erreur au chargement des donnéés", "Demande");
            }
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                    this.txt_Produit.Tag = laDetailDemande.LaDemande.FK_IDPRODUIT;
                    this.txtCentre.Tag = laDetailDemande.LaDemande.FK_IDCENTRE;
                    CodeProduit = laDetailDemande.LaDemande.PRODUIT;
                    Tdem = laDetailDemande.LaDemande.TYPEDEMANDE;
                    this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).LIBELLE;
                    txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem);

                    this.Txt_Motif.Text = string.IsNullOrEmpty(laDetailDemande.LaDemande.MOTIF) ? string.Empty : laDetailDemande.LaDemande.MOTIF;
                    if (laDetailDemande.LstCommentaire != null && laDetailDemande.LstCommentaire.Count != 0)
                        this.Txt_MotifRejet.Text = laDetailDemande.LstCommentaire.First().COMMENTAIRE;

                    AfficherCannalisationDemande(laDetailDemande.LstCanalistion.First());
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




    }
}


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
using Galatee.Silverlight.Tarification.Helper;
using Galatee.Silverlight.Fraude;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Resources.Fraude;
//using Galatee.Silverlight.ServiceReclamation;
using Galatee.Silverlight.ServiceAccueil;
using System.Collections.ObjectModel;
using System.IO;


namespace Galatee.Silverlight.Accueil
{
    public partial class UcInformationsReclamation : ChildWindow
    {
        private List<Galatee.Silverlight.ServiceAccueil.CsCommune> _listeDesCommuneExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsCommune> _listeDesCommuneExistantCentre = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsCentre> _listeDesCentreExistant = null;
        private string Tdem = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsRues> _listeDesRuesExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsQuartier> _listeDesQuartierExistant = null;
        Galatee.Silverlight.ServiceAccueil.CsClient Client = new Galatee.Silverlight.ServiceAccueil.CsClient();
        CsReclamationRcl ClsReclamation = new CsReclamationRcl();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        private UcImageScanne formScanne = null;
        int demande;
        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();

        protected virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion

        public UcInformationsReclamation()
        {
            InitializeComponent();
            ChargerModeReception();
            ChargerTypeReclamation();
            Txt_Client.Text = "G" + UserConnecte.matricule + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Day.ToString("00");
            Txt_Ordre.Text = "01";
            System.DateTime today = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(15, 0, 0, 0);
            Dtp_DateRendezVous.SelectedDate = today.Add(duration);
            System.TimeSpan duration1 = new System.TimeSpan(8, 0, 0, 0);
            Dtp_DateretourSouhaite.SelectedDate = today.Add(duration1);

            Dtp_DateOuverture.SelectedDate = today;
            ChargerTypeDocument();
            RemplirGroupeValidationDepannage();

            this.labMotif.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_MotifRejet.Visibility = System.Windows.Visibility.Collapsed;
        }
        //public UcInformationsReclamation()
        //{
        //    InitializeComponent();
        //    ChargerModeReception();
        //    ChargerTypeReclamation();
        //    Txt_Client.Text = "G" + UserConnecte.matricule + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Day.ToString("00");
        //    Txt_Ordre.Text = "01";
        //    System.DateTime today = System.DateTime.Now;
        //    System.TimeSpan duration = new System.TimeSpan(15, 0, 0, 0);
        //    Dtp_DateRendezVous.SelectedDate = today.Add(duration);
        //    System.TimeSpan duration1 = new System.TimeSpan(8, 0, 0, 0);
        //    Dtp_DateretourSouhaite.SelectedDate = today.Add(duration1);

        //    Dtp_DateOuverture.SelectedDate = today;
        //    ChargerTypeDocument();
        //    RemplirGroupeValidationDepannage();
        //}
        int EtapeActuelle;
        CsDemandeReclamation LaDemande = new CsDemandeReclamation();
        public UcInformationsReclamation(List<int> demandes, int fkIdEtape)
        {
            InitializeComponent();
            this.labMotif.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_MotifRejet.Visibility = System.Windows.Visibility.Collapsed; this.demande = demandes.First();
            RemplirCommune();
            RemplirListeDesQuartierExistant();
            RemplirSecteur();
            RemplirListeDesRuesExistant();
            ChargerDonneeDuSite();
            ChargerModeReception_();
            ChargerTypeReclamation_();
            ChargerTypeDocument_();
            RemplirGroupeValidationDepannage();
            ChargeDonneDemande(demandes.First());
            EtapeActuelle = fkIdEtape;

        }
        public UcInformationsReclamation(int demande)
        {
            InitializeComponent();
            this.labMotif.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_MotifRejet.Visibility = System.Windows.Visibility.Collapsed; this.demande = demande;
            RemplirCommune();
            RemplirListeDesQuartierExistant();
            RemplirSecteur();
            RemplirListeDesRuesExistant();
            ChargerDonneeDuSite();
            ChargerModeReception_();
            ChargerTypeReclamation_();
            ChargerTypeDocument_();
            RemplirGroupeValidationDepannage();
            ChargeDonneDemande(demande);
        }
        public UcInformationsReclamation(int demande, bool IsConsultation=false)
        {
            InitializeComponent();
            this.labMotif.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_MotifRejet.Visibility = System.Windows.Visibility.Collapsed; this.IsConsultation = IsConsultation;
            this.demande = demande;
            RemplirCommune();
            RemplirListeDesQuartierExistant();
            RemplirSecteur();
            RemplirListeDesRuesExistant();
            ChargerDonneeDuSite();
            ChargerModeReception_();
            ChargerTypeReclamation_();
            ChargerTypeDocument_();
            RemplirGroupeValidationDepannage();
            ChargeDonneDemande(demande);
        }
        private void RemplirCommune()
        {
            try
            {
                if (SessionObject.LstCommune != null && SessionObject.LstCommune.Count != 0)
                {
                    _listeDesCommuneExistant = SessionObject.LstCommune;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCommuneAsync();
                service.ChargerCommuneCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    SessionObject.LstCommune = args.Result;
                    _listeDesCommuneExistant = SessionObject.LstCommune;

                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirListeDesQuartierExistant()
        {
            try
            {

                if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
                {
                    _listeDesQuartierExistant = SessionObject.LstQuartier;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesQartiersAsync();
                service.ChargerLesQartiersCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstQuartier = args.Result;
                    _listeDesQuartierExistant = SessionObject.LstQuartier;
                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirListeDesRuesExistant()
        {
            try
            {

                if (SessionObject.LstRues != null && SessionObject.LstRues.Count != 0)
                {
                    _listeDesRuesExistant = SessionObject.LstRues;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesRueDesSecteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstRues = args.Result;
                    _listeDesRuesExistant = SessionObject.LstRues;
                };
                service.ChargerLesRueDesSecteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirSecteur()
        {
            try
            {
                if (SessionObject.LstSecteur != null && SessionObject.LstSecteur.Count != 0)
                {
                    return;
                }
                else
                {

                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerLesSecteursCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstSecteur = args.Result;
                        return;
                    };
                    service.ChargerLesSecteursAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre.Count != 0)
                {
                    _listeDesCentreExistant = SessionObject.LstCentre;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    _listeDesCentreExistant = SessionObject.LstCentre;
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerDonneeDuSite");

            }
        }
        private void ChargeDonneDemande(int pk_id)
        {
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourDemandeReclamationCompleted += (s, args) =>
            {
                try
                {
                    if (args != null && args.Cancelled)
                        return;
                    LaDemande = args.Result;

                    if (LaDemande != null)
                    {
                        Txt_Nom.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.NomClient) ? string.Empty : LaDemande.ReclamationRcl.NomClient;
                        Txt_Centre.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.CENTRE) ? string.Empty : SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.Fk_IdCentre).LIBELLE;
                        Txt_Portable.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.NumeroTelephonePortable) ? string.Empty : LaDemande.ReclamationRcl.NumeroTelephonePortable;
                        Dtp_DateOuverture.SelectedDate = LaDemande.ReclamationRcl.DateOuverture ;
                        Txt_NumeroFixe.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.NumeroTelephoneFixe) ? string.Empty : LaDemande.ReclamationRcl.NumeroTelephoneFixe;
                        Cbo_TypeReclamation.SelectedValue = LaDemande.ReclamationRcl.Fk_IdTypeReclamation;
                        var lst_mode_recept=((List<Galatee.Silverlight.ServiceAccueil.CsModeReception>)Cbo_ModeReception.ItemsSource);
                        if (lst_mode_recept != null && lst_mode_recept.Count()>0)
                            Cbo_ModeReception.SelectedItem =lst_mode_recept.FirstOrDefault(c=>c.pk_id== LaDemande.ReclamationRcl.Fk_IdModeReception);
                        Txt_Adress.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.Adresse) ? string.Empty : LaDemande.ReclamationRcl.Adresse;
                        Txt_Email.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.Email) ? string.Empty : LaDemande.ReclamationRcl.Email;
                        Txt_Object.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.ObjetReclamation) ? string.Empty : LaDemande.ReclamationRcl.ObjetReclamation;
                        Txt_Observation.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.Observation) ? string.Empty : LaDemande.ReclamationRcl.Observation;
                        Txt_Client.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.Client) ? string.Empty : LaDemande.ReclamationRcl.Client;
                        Dtp_DateRendezVous.SelectedDate = LaDemande.ReclamationRcl.DateRdv ;
                        Dtp_DateretourSouhaite.SelectedDate = LaDemande.ReclamationRcl.DateRetourSouhaite ;
                        //Dtp_DateTraitement.SelectedDate = System.DateTime.Today;
                        Txt_NumDemande.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.NumeroReclamation) ? string.Empty : LaDemande.ReclamationRcl.NumeroReclamation;
                        if (string.IsNullOrEmpty(LaDemande.ReclamationRcl.NumeroReclamation))
                        {
                            Txt_NumDemande.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            Txt_NumDemande.Visibility = Visibility.Visible;
                        }
                        Txt_EtablirPar.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.NOMAGENTCREATION) ? string.Empty : LaDemande.ReclamationRcl.NOMAGENTCREATION;
                        Txt_EtablirPar.Tag = UserConnecte.matricule;


                        Txt_MotifRejet.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.MotifRejet) ? string.Empty : LaDemande.ReclamationRcl.MotifRejet;

                        if (!string.IsNullOrEmpty(LaDemande.ReclamationRcl.MotifRejet))
                        {
                            this.labMotif.Visibility = System.Windows.Visibility.Visible;
                            this.Txt_MotifRejet.Visibility = System.Windows.Visibility.Visible;
                        }

                        var lst_type_rcl = ((List<Galatee.Silverlight.ServiceAccueil.CsTypeReclamationRcl>)Cbo_TypeReclamation.ItemsSource);
                        if (lst_type_rcl != null && lst_type_rcl.Count() > 0)
                            Cbo_TypeReclamation.SelectedItem = lst_type_rcl.FirstOrDefault(c => c.PK_ID == LaDemande.ReclamationRcl.Fk_IdTypeReclamation);



                        //Txt_AgentTraiteur.Text = UserConnecte.nomUtilisateur;
                        //Txt_Matricule.Text = UserConnecte.matricule;

                        //this.tbControleClient.SelectedItem = this.tabItemtraitement;

                        #region DocumentScanne
                        if (LaDemande.DonneDeDemande != null && LaDemande.DonneDeDemande.Count != 0)
                        {
                            //isPreuveSelectionnee = true;
                            LstPiece.Clear();
                            foreach (var item in LaDemande.DonneDeDemande)
                            {
                                LstPiece.Add(item);
                            }
                            dgListePiece.ItemsSource = null;
                            dgListePiece.ItemsSource = this.LstPiece;
                        }
                        //else
                        //{
                        //    isPreuveSelectionnee = false;
                        //}
                        #endregion

                        if (this.IsConsultation==true)
                        {
                            OKButton.Visibility = Visibility.Collapsed;
                            CancelButton.Visibility = Visibility.Collapsed;
                        }
                        //txtCentre.Text   = _listeDesCentreExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.Fk_IdCentre)!= null ?
                        //    _listeDesCentreExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.Fk_IdCentre).LIBELLE :string.Empty ;

                        //txt_Commune.Text = _listeDesCommuneExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDCOMMUNE) != null ?
                        //    _listeDesCommuneExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDCOMMUNE).LIBELLE : string.Empty ;

                        //txt_Quartier.Text = _listeDesQuartierExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDQUARTIER) != null ?
                        //    _listeDesQuartierExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDQUARTIER).LIBELLE : string.Empty;

                        //txt_NumSecteur.Text = SessionObject.LstSecteur.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDSECTEUR) != null ?
                        //    SessionObject.LstSecteur.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDSECTEUR).LIBELLE : string.Empty;

                        //txt_NumRue.Text =SessionObject.LstRues.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDRUE)!= null ?
                        //    SessionObject.LstRues.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDRUE).LIBELLE : string.Empty ;
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }

            };
            service.RetourDemandeReclamationAsync(pk_id);

        }



        public UcInformationsReclamation(string Tdem, string IsInit)
        {
            InitializeComponent();
            this.labMotif.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_MotifRejet.Visibility = System.Windows.Visibility.Collapsed;
            ChargerModeReception();
            ChargerTypeReclamation();
            Txt_Client.Text = "G" + UserConnecte.matricule  + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Day.ToString("00");
            System.DateTime today = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(15, 0, 0, 0);
            Dtp_DateRendezVous.SelectedDate = today.Add(duration);
            System.TimeSpan duration1 = new System.TimeSpan(8, 0, 0, 0);
            Dtp_DateretourSouhaite.SelectedDate = today.Add(duration1);
            Dtp_DateOuverture.SelectedDate = today;
            ChargerTypeDocument();
            RemplirGroupeValidationDepannage();

            //RemplirCommune();
            //RemplirListeDesQuartierExistant();
            //RemplirSecteur();
            //RemplirListeDesRuesExistant();
            //_listeDesCentreExistant = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
            //RemplirCentrePerimetre(_listeDesCentreExistant);
            //Txt_EtablirPar.Text = UserConnecte.matricule;
            Txt_EtablirPar.Text = UserConnecte.nomUtilisateur + " (" + UserConnecte.matricule + ")";
            Txt_EtablirPar.IsReadOnly = true;
        }
        string centre = string.Empty;
        string ordre = "01";
        #region Piece jointe
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
                    cbo_typedoc.ItemsSource = LstTypeDocument.Where(c=>c.CODE=="001");
                    cbo_typedoc.DisplayMemberPath = "LIBELLE";
                    cbo_typedoc.SelectedValuePath = "PK_ID";
                    //ChargeDonneDemande(demande);
                };
                service.ChargerTypeDocumentAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChargerTypeDocument_()
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
                    cbo_typedoc.ItemsSource = LstTypeDocument.Where(c=>c.CODE=="001");
                    cbo_typedoc.DisplayMemberPath = "LIBELLE";
                    cbo_typedoc.SelectedValuePath = "PK_ID";
                    ChargeDonneDemande(demande);
                };
                service.ChargerTypeDocumentAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
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
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                     ObjDOCUMENTSCANNE Fraix = ( ObjDOCUMENTSCANNE)dgListePiece.SelectedItem;
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
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        private byte[] image;
        #endregion
        //private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        //{
        //    dgListePiece.BeginEdit();
        //}
        private CsDemandeReclamation GetInformationsFromScreen()
        {
            CsDemandeReclamation listObjetForInsertOrUpdate = new CsDemandeReclamation(); 
            //listObjetForInsertOrUpdate.LeClient = new ServiceReclamation.CsClient();
            listObjetForInsertOrUpdate.ReclamationRcl  = new CsReclamationRcl ();
            listObjetForInsertOrUpdate.LaDemande = new CsDemandeBase();
            listObjetForInsertOrUpdate.DonneDeDemande = new List<ObjDOCUMENTSCANNE>();
            try
            {
                Galatee.Silverlight.ServiceAccueil.CsTdem leTydemande = new Galatee.Silverlight.ServiceAccueil.CsTdem();
                leTydemande = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DemandeReclamation);

                listObjetForInsertOrUpdate.LaDemande.TYPEDEMANDE = (string)leTydemande.CODE;
                listObjetForInsertOrUpdate.LaDemande.FK_IDTYPEDEMANDE = (int)leTydemande.PK_ID;
                listObjetForInsertOrUpdate.LaDemande.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                if(this.demande!=null)
                    listObjetForInsertOrUpdate.LaDemande.PK_ID = this.demande;

                listObjetForInsertOrUpdate.LaDemande.USERCREATION = UserConnecte.matricule;
                listObjetForInsertOrUpdate.LaDemande.DATECREATION = DateTime.Now;
                Guid Fk_Id_GroupValidation=new Guid();
                if(this.txt_GroupeValidation.Tag!=null)
                    Guid.TryParse(this.txt_GroupeValidation.Tag.ToString(), out Fk_Id_GroupValidation);
                var sReclamationRcl = new CsReclamationRcl
                    {
                        DateRetourSouhaite = Convert.ToDateTime(Dtp_DateretourSouhaite.SelectedDate),
                        DateOuverture = Convert.ToDateTime(Dtp_DateOuverture.SelectedDate),
                        DateRdv = Convert.ToDateTime(Dtp_DateRendezVous.SelectedDate),
                        NomClient = Txt_Nom.Text,
                        //Ordre =(string) Txt_Nom.Tag,
                        NumeroTelephonePortable  = Txt_Portable.Text,
                        Adresse = Txt_Adress.Text,
                        Email  = Txt_Email.Text,
                        NumeroTelephoneFixe = Txt_NumeroFixe.Text,
                        ObjetReclamation = Txt_Object.Text,
                        Observation = Txt_Observation.Text,
                        //AgentEmetteur = Txt_EtablirPar.Text,
                        //AgentEmetteur = Txt_EtablirPar.Tag!=null? Txt_EtablirPar.Tag.ToString():UserConnecte.matricule,
                        AgentEmetteur = Txt_EtablirPar.Tag != null ? Txt_EtablirPar.Tag.ToString() : UserConnecte.matricule,

                        MotifRejet = Txt_MotifRejet.Text,
                        Fk_IdCentre =UserConnecte.FK_IDCENTRE ,
                        Client = Txt_Client.Text,
                        NumeroReclamation  = string.Empty ,
                        //FK_IDCOMMUNE = ((Galatee.Silverlight.ServiceAccueil.CsCommune)Cbo_Commune.SelectedItem).PK_ID,
                        //FK_IDQUARTIER = ((Galatee.Silverlight.ServiceAccueil.CsQuartier)Cbo_Quartier.SelectedItem).PK_ID,
                        //FK_IDRUE = ((Galatee.Silverlight.ServiceAccueil.CsRues)Cbo_Rue.SelectedItem).PK_ID,
                        //FK_IDSECTEUR = ((Galatee.Silverlight.ServiceAccueil.CsSecteur)Cbo_Secteur.SelectedItem).PK_ID,
                        Fk_IdTypeReclamation =((Galatee.Silverlight.ServiceAccueil.CsTypeReclamationRcl) Cbo_TypeReclamation.SelectedItem).PK_ID ,
                        LIBELLETYPERECLAMATION = ((Galatee.Silverlight.ServiceAccueil.CsTypeReclamationRcl)Cbo_TypeReclamation.SelectedItem).Libelle ,
                        Fk_IdModeReception =int.Parse(((Galatee.Silverlight.ServiceAccueil.CsModeReception) Cbo_ModeReception.SelectedItem).pk_id.ToString()) ,
                        Fk_Id_GroupValidation = Fk_Id_GroupValidation
                       
                    };
                centre = UserConnecte.Centre;
                if (Client != null && !string.IsNullOrEmpty( Client.REFCLIENT) )
                {
                    centre = Client.CENTRE;
 
                    sReclamationRcl.Client = Client.REFCLIENT;
                    sReclamationRcl.Fk_IdClient = Client.PK_ID;
                    sReclamationRcl.Fk_IdCentre = Client.FK_IDCENTRE;
                    sReclamationRcl.CENTRE  = Client.CENTRE;
                    ordre = Client.ORDRE;

                    listObjetForInsertOrUpdate.LaDemande.CENTRE = Client.CENTRE;
                    listObjetForInsertOrUpdate.LaDemande.FK_IDCENTRE = Client.FK_IDCENTRE.Value;
                    listObjetForInsertOrUpdate.LaDemande.PRODUIT = Client.PRODUIT;
                    listObjetForInsertOrUpdate.LaDemande.FK_IDPRODUIT = Client.FK_IDPRODUIT;
                }
                else
                {
                    listObjetForInsertOrUpdate.LaDemande.CENTRE = UserConnecte.Centre;
                    listObjetForInsertOrUpdate.LaDemande.FK_IDCENTRE = UserConnecte.FK_IDCENTRE;
                }
                if (LaDemande.ReclamationRcl!=null && LaDemande.ReclamationRcl.PK_ID != null && LaDemande.ReclamationRcl.PK_ID != 0)
                    sReclamationRcl.PK_ID = LaDemande.ReclamationRcl.PK_ID;
                listObjetForInsertOrUpdate.ReclamationRcl = sReclamationRcl;
                #region Doc Scanne
                if (listObjetForInsertOrUpdate.DonneDeDemande == null) listObjetForInsertOrUpdate.DonneDeDemande = new List<ObjDOCUMENTSCANNE>();
                listObjetForInsertOrUpdate.DonneDeDemande.AddRange(LstPiece);
                #endregion

                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                 Message.ShowError(ex.Message,null);
                return null;
            }
        }

        private bool VerifieChampObligation()
        {
            try
            {
                bool ReturnValue = true;



                #region information abonnement

                if (string.IsNullOrEmpty(this.Dtp_DateRendezVous.SelectedDate.ToString()))
                    throw new Exception("Selectionnez Date Rendez Vous");
                if (string.IsNullOrEmpty(this.Dtp_DateOuverture.SelectedDate.ToString()))
                    throw new Exception("Selectionnez Date Ouverture");
                if (string.IsNullOrEmpty(this.Txt_Nom.Text))
                    throw new Exception("remplir le Nom");
                //if (string.IsNullOrEmpty(this.Txt_Prenom.Text))
                //    throw new Exception("remplir le Prénom");
                if (string.IsNullOrEmpty(this.Txt_Portable.Text))
                    throw new Exception("Saisir le numero de portable");
                //if (string.IsNullOrEmpty(this.Txt_Object.Text))
                //    throw new Exception("remplir l Object");
                if (Cbo_ModeReception.SelectedItem == null)
                    throw new Exception("Selectionnez Mode Reception ");
                if (Cbo_TypeReclamation.SelectedItem == null)
                    throw new Exception("Selectionnez Type Reclamation ");
                //if (this.btn_GroupeValidation.Tag != null)
                //    throw new Exception("Selectionnez Type Reclamation ");

                //if (string.IsNullOrEmpty(this.txt.Text))
                //        throw new Exception("remplir referend plomgs ");

                //if (((CsProduit)Cbo_Produit.SelectedItem).CODE != SessionObject.Enumere.ElectriciteMT)
                //{
                //    if (string.IsNullOrEmpty(this.txt_Reglage.Text))
                //        throw new Exception("Selectionnez le calibre ");
                //}
                #endregion

                #region Adresse géographique

                //if (Cbo_Centre.SelectedItem == null)
                //    throw new Exception("Selectionnez Centre");

                //if (Cbo_Quartier.SelectedItem == null)
                //    throw new Exception("Selectionnez Centre");

                //if (Cbo_Commune.SelectedItem == null)
                //    throw new Exception("Selectionnez Centre");


                //if (string.IsNullOrEmpty(this.txtCentre.Text))
                //    throw new Exception("Séléctionnez le Centre ");

                //if (string.IsNullOrEmpty(this.txt_Commune.Text))
                //    throw new Exception("Séléctionnez la commune ");

                //if (string.IsNullOrEmpty(this.txt_Quartier.Text))
                //    throw new Exception("Séléctionnez le quartier ");
                #endregion

                if (this.txt_GroupeValidation.Tag == null )
                    throw new Exception("Selectionnez groupe destinataire");

                return ReturnValue;

            }
            catch (Exception ex)
            {
               // this.BtnTRansfert.IsEnabled = true;
                this.OKButton.IsEnabled = true;
                Message.ShowInformation(ex.Message, "Fraude");
                return false;
            }

        }
        private void EnvoyerDemandeEtapeSuivante(List<int> Listid, string idDem, string numDem, Guid idGroup)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));

            clientWkf.ExecuterActionSurDemandeParPkIDLigneCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec ", Langue.lbl_Menu);
                    return;
                }
                if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
                {
                    Message.ShowError("Echec ", Langue.lbl_Menu);
                    return;
                }
                else
                {
                    Message.ShowInformation("Demande transmise avec succès", "Reclamation");
                    
                    MiseAJourGroupSurCopieCircuit(idDem, numDem, idGroup);

                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }


        private void ValidationDemande( Guid idgroupValidation )
        {
            try
            {
                CsDemandeReclamation _LaDemande = new CsDemandeReclamation();
                _LaDemande = GetInformationsFromScreen();
              
                //Lancer la transaction de mise a jour en base
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ValiderInitReclamationCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    if (!string.IsNullOrEmpty(res.Result))
                    {
                        string Retour = res.Result;
                        string[] coupe = Retour.Split('.');

                        if (EtapeActuelle == 0)
                            Shared.ClasseMEthodeGenerique.InitWOrkflowToGroupValidation(coupe[0], _LaDemande.LaDemande.FK_IDCENTRE, coupe[1], idgroupValidation, _LaDemande.LaDemande.FK_IDTYPEDEMANDE);
                        else
                        {
                            List<int> Listid = new List<int>();
                            Listid.Add(LaDemande.LaDemande.PK_ID);
                            //EnvoyerDemandeEtapeSuivante(Listid);
                            EnvoyerDemandeEtapeSuivante(Listid, coupe[0], coupe[1], idgroupValidation);
                        }

                        //if (this.demande != 0)
                        //{
                        //    List<int> Listid = new List<int>();
                        //    Listid.Add(this.demande);
                        //    EnvoyerDemandeEtapeSuivante(Listid);
                        //    //Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], _LaDemande.LaDemande.FK_IDCENTRE, coupe[1], _LaDemande.LaDemande.FK_IDTYPEDEMANDE);
                        //}
                        //else
                        //{
                        //    this.demande = int.Parse(coupe[0]);
                        //    InitWOrkflow((coupe[0]), UserConnecte.FK_IDCENTRE, "Galatee.Silverlight.Accueil.UcInformationsReclamation", coupe[1], SaveMandatement, null);
                        //}
                        //numedemande = coupe[1];
                        //Client = coupe[2];


                        List<CsReclamationRcl> leDemandeAEditer = new List<CsReclamationRcl>();
                        _LaDemande.ReclamationRcl.NumeroReclamation = coupe[1];
                        _LaDemande.ReclamationRcl.QUARTIER = centre;
                        _LaDemande.ReclamationRcl.NOMAGENTRECEPTEUR  = ordre;
                        leDemandeAEditer.Add(_LaDemande.ReclamationRcl);
                        Utility.ActionDirectOrientation<ServicePrintings.CsReclamationRcl, CsReclamationRcl>(leDemandeAEditer, null, SessionObject.CheminImpression, "FicheDeTravail", "Reclamation", true);

                    }
                    //if (Closed != null)
                    //    Closed(this, new EventArgs());
                    this.DialogResult = false;
                };
                service1.ValiderInitReclamationAsync(_LaDemande);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }


        private void MiseAJourGroupSurCopieCircuit(string idDemande, string numdem, Guid idGroup)
        {

            Galatee.Silverlight.ServiceParametrage.ParametrageClient service = new Galatee.Silverlight.ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));

            service.UpdateGroupValidationCopieCircuitCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec ", Langue.lbl_Menu);
                    return;
                }
                if (!wsen.Result)
                {
                    Message.ShowError("Echec ", Langue.lbl_Menu);
                    return;
                }
                else
                {

                }
            };
            service.UpdateGroupValidationCopieCircuitAsync(idDemande, numdem, idGroup);
        }




        public delegate void MethodeDeCallBack<T>(T param);
        public void InitWOrkflow(string IdDemandeCree, int centreDemandeCree, string FrmInitialisation, string NumeroDemandeTableTravail, MethodeDeCallBack<dynamic> handler, dynamic Pamatre)
        {
            //Ajouté par WCO le 04/08/2015
            //Pour l'insertion d'une demande directement dans ma table
            Workflow.WorkflowDmdManager managerWkf = new Workflow.WorkflowDmdManager();
            managerWkf.InsertionDemandeWorkflowComplete += (str_rsl) =>
            {
                if (str_rsl.StartsWith("ERR"))
                {
                    Message.ShowError(new Exception(str_rsl), "Création d'une demande");
                }
                else
                {
                    handler(Pamatre);
                    Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + NumeroDemandeTableTravail,
                        Silverlight.Resources.Devis.Languages.txtDevis);

                }
            };
            managerWkf.InsererMaDemande(IdDemandeCree, FrmInitialisation, centreDemandeCree, NumeroDemandeTableTravail);
        }


/*
        public void SaveMandatement(dynamic param)
        {
            //List<CsDetailCampagneGc> ListMandatementGc = (List<CsDetailCampagneGc>)param;
            //Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            //service.SaveMandatementCompleted += (s, args) =>
            //{
            //    if (args != null && args.Cancelled)
            //        return;
            //    if (args.Result == null)
            //        return;
            //    if (args.Result == true)
            //    {
            List<int> Listid = new List<int>();
            Listid.Add(this.demande);
            EtapeActuelle = 6426;
                    EnvoyerDemandeEtapeSuivante(Listid);
            //    }
            //    else
            //    {
            //        Message.Show("Le Mandatment n'a pas été enregistré ", "Information");
            //    }

            //    return;
            //};
            //service.SaveMandatementAsync(ListMandatementGc, true);
        }
 */ 
   
        
        
        //private void EnvoyerDemandeEtapeSuivante(List<int> Listid)
        //{
        //    ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));

        //    clientWkf.ExecuterActionSurDemandeParPkIDLigneCompleted += (wkf, wsen) =>
        //    {
        //        if (null != wsen && wsen.Cancelled)
        //        {
        //            Message.ShowError("Echec de sortie materiel", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
        //            return;
        //        }
        //        if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
        //        {
        //            Message.ShowError("Echec de sortie materiel", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
        //            return;
        //        }
        //        else
        //        {
        //            //Message.ShowInformation("Sortie materiel effectuée", Langue.lbl_Menu);
        //            this.DialogResult = true;
        //        }
        //    };
        //    clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
        //        string.Empty);
        //}

        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!VerifieChampObligation()) return;

    
           Guid GroupeDeValidation  = (Guid)this.txt_GroupeValidation.Tag;
           ValidationDemande( GroupeDeValidation);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChargerModeReception()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.SelectAllModeReceptionCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_ModeReception.ItemsSource = null;
                        Cbo_ModeReception.DisplayMemberPath = "Libelle";
                        Cbo_ModeReception.SelectedValuePath = "PK_ID";
                        Cbo_ModeReception.ItemsSource = args.Result;
                        //ChargeDonneDemande(demande);

                    }
                };
                client.SelectAllModeReceptionAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerModeReception_()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.SelectAllModeReceptionCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_ModeReception.ItemsSource = null;
                        Cbo_ModeReception.DisplayMemberPath = "Libelle";
                        Cbo_ModeReception.SelectedValuePath = "PK_ID";
                        Cbo_ModeReception.ItemsSource = args.Result;
                        ChargeDonneDemande(demande);

                    }
                };
                client.SelectAllModeReceptionAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerTypeReclamation()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.SelectAllTypeReclamationRclCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_TypeReclamation.ItemsSource = null;
                        Cbo_TypeReclamation.DisplayMemberPath = "Libelle";
                        Cbo_TypeReclamation.SelectedValuePath = "PK_ID";
                        Cbo_TypeReclamation.ItemsSource = args.Result;
                        //ChargeDonneDemande(demande);

                    }
                };
                client.SelectAllTypeReclamationRclAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerTypeReclamation_()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.SelectAllTypeReclamationRclCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_TypeReclamation.ItemsSource = null;
                        Cbo_TypeReclamation.DisplayMemberPath = "Libelle";
                        Cbo_TypeReclamation.SelectedValuePath = "PK_ID";
                        Cbo_TypeReclamation.ItemsSource = args.Result;
                        ChargeDonneDemande(demande);

                    }
                };
                client.SelectAllTypeReclamationRclAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
   
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Galatee.Silverlight.Fraude.UcRechercheClient Newfrm = new Galatee.Silverlight.Fraude.UcRechercheClient(); ;
                Newfrm.CallBack += Newfrm_CallBack;
                Newfrm.Show();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
   
        private void NewButton1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Galatee.Silverlight.Reclamation.UcRechercheDemande Newfrm1 = new Galatee.Silverlight.Reclamation.UcRechercheDemande(); ;
                Newfrm1.CallBack += Newfrm1_CallBack;
                Newfrm1.Show();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void Newfrm1_CallBack(object sender, CustumEventArgs e)
        {
            ClsReclamation = e.Data as CsReclamationRcl;
            Txt_Nom.Text = ClsReclamation.NomClient;
            //Txt_Nom.Tag = ClsReclamation.Ordre;
            Txt_Portable.Text = ClsReclamation.NumeroTelephonePortable == null ? "" : ClsReclamation.NumeroTelephonePortable;
            Txt_NumeroFixe.Text = ClsReclamation.NumeroTelephoneFixe == null ? "" : ClsReclamation.NumeroTelephoneFixe;
            Txt_Adress.Text = ClsReclamation.Adresse == null ? "" : ClsReclamation.Adresse;
            Txt_Email.Text = ClsReclamation.Email == null ? "" : ClsReclamation.Email;
            Txt_Client.Text = ClsReclamation.Client == null ? "" : ClsReclamation.Client;
            Txt_Object.Text = ClsReclamation.ObjetReclamation == null ? "" : ClsReclamation.ObjetReclamation;
            Txt_Observation.Text = ClsReclamation.Observation == null ? "" : ClsReclamation.Observation;

            Dtp_DateRendezVous.SelectedDate = ClsReclamation.DateRdv;
            Dtp_DateretourSouhaite.SelectedDate = ClsReclamation.DateRetourSouhaite;
            Dtp_DateOuverture.SelectedDate = ClsReclamation.DateOuverture;
           
        }


        private void Newfrm_CallBack(object sender, CustumEventArgs e)
        {
            Client = e.Data as Galatee.Silverlight.ServiceAccueil.CsClient;
            Txt_Nom.Text = Client.NOMABON;
            Txt_Ordre.Text = Client.ORDRE;

            Txt_Portable.Text = Client.TELEPHONE == null ? "" : Client.TELEPHONE;
            Txt_NumeroFixe.Text = Client.TELEPHONEFIXE == null ? "" : Client.TELEPHONEFIXE;
            Txt_Adress.Text = Client.ADRMAND1 == null ? "" : Client.ADRMAND1;
            Txt_Email.Text = Client.EMAIL == null ? "" : Client.EMAIL;
            Txt_Client.Text = Client.REFCLIENT == null ? "" : Client.REFCLIENT;
            Txt_Centre.Text = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == Client.FK_IDCENTRE).LIBELLE;

        }
        private void NewfrmPj_CallBack(object sender, CustumEventArgs e)
        {
        string test=    e.Data as string;
        }

        private void Chk_EstClient_Checked(object sender, RoutedEventArgs e)
        {
            if (Chk_EstClient.IsChecked == true)
            {
                Btn_RechercherClient.IsEnabled = true;
               
            }
            else
            {
                Btn_RechercherClient.IsEnabled = false;
            }
        }

        private void Chk_EstReclamation_Checked(object sender, RoutedEventArgs e)
        {
            if (Chk_EstReclamation.IsChecked == true)
            {
                Btn_RechercherReclamation.IsEnabled = true;

            }
            else
            {
                Btn_RechercherReclamation.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        List<Galatee.Silverlight.ServiceParametrage.CsGroupeValidation> lesGroupeValidation = new List<ServiceParametrage.CsGroupeValidation>();
        private void RemplirGroupeValidationDepannage()
        {
            try
            {

                Galatee.Silverlight.ServiceParametrage.ParametrageClient service = new Galatee.Silverlight.ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                service.SelectAllGroupeValidationSpecifiqueCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    lesGroupeValidation = args.Result;
                };
                service.SelectAllGroupeValidationSpecifiqueAsync(1);
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void btn_GroupeValidation_Click_1(object sender, RoutedEventArgs e)
        {
            if (lesGroupeValidation != null)
            {

                List<object> _LstObj = new List<object>();
                _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(lesGroupeValidation);
                Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                _LstColonneAffich.Add("GROUPENAME", "GROUPE DESTINATAIRE");

                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Groupe");
                ctrl.Closed += new EventHandler(galatee_OkClickedBatch);
                ctrl.Show();
            }
        }

        void galatee_OkClickedBatch(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceParametrage.CsGroupeValidation _LesGroupeValiadation = ctrs.MyObject as Galatee.Silverlight.ServiceParametrage.CsGroupeValidation;
                this.txt_GroupeValidation.Text = string.IsNullOrEmpty(_LesGroupeValiadation.GROUPENAME) ? string.Empty : _LesGroupeValiadation.GROUPENAME;
                this.txt_GroupeValidation.Tag = _LesGroupeValiadation.PK_ID;
            }
        }



        //private bool VerifieChampObligation()
        //{
        //    try
        //    {
        //        bool ReturnValue = true;



        //        #region information abonnement
                
        //        if (string.IsNullOrEmpty(this.Dtp_DateOuverture.SelectedDate.ToString()))
        //            throw new Exception("Selectionnez Date Ouverture");
        //        if (string.IsNullOrEmpty(this.Txt_Nom.Text))
        //            throw new Exception("Selectionnez le Nom  client");
        //        if (string.IsNullOrEmpty(this.Txt_Nom.Text))
        //            throw new Exception("Selectionnez le Nom  client");
        //        if (string.IsNullOrEmpty(this.Txt_NumeroFixe.Text))
        //            throw new Exception("Selectionnez le Numero Fixe");
        //        if (string.IsNullOrEmpty(this.Txt_Object.Text))
        //            throw new Exception("Selectionnez l'Object");
        //        if (string.IsNullOrEmpty(this.Txt_Adress.Text))
        //            throw new Exception("Selectionnez l Adress");
        //        if (string.IsNullOrEmpty(this.Txt_Email.Text))
        //            throw new Exception("Selectionnez l'Email");
        //        if (string.IsNullOrEmpty(this.Txt_Observation.Text))
        //            throw new Exception("Selectionnez l'Observation");


        //        if (Cbo_TypeReclamation.SelectedItem == null)
        //            throw new Exception("Selectionnez Type Reclamation ");
        //        if (Cbo_ModeReception.SelectedItem == null)
        //            throw new Exception("Selectionnez Mode Reception ");
        //        //if (Cbo_AnorBranchmnt.SelectedItem == null)
        //        //    throw new Exception("Selectionnez Anormalie Branchment ");
        //        //if (Cbo_AnormalieCacheb.SelectedItem == null)
        //        //    throw new Exception("Selectionnez Anormalie Cache borne ");
        //        //if (Cbo_AnormlieCompteur.SelectedItem == null)
        //        //    throw new Exception("Selectionnez Anormalie Compteur ");
        //        //if (Cbo_CalibreCompteur.SelectedItem == null)
        //        //    throw new Exception("Selectionnez Calibre Compteur ");
        //        //if (Cbo_CalibreDijoncteur.SelectedItem == null)
        //        //    throw new Exception("Selectionnez Calibre Dijoncteur ");
        //        //if (Cbo_Fils.SelectedItem == null)
        //        //    throw new Exception("Selectionnez Calibre Fils ");
        //        //if (Cbo_MarqueCmpt.SelectedItem == null)
        //        //    throw new Exception("Selectionnez marque Compteur ");
        //        //if (Cbo_MArqueDijoncteur.SelectedItem == null)
        //        //    throw new Exception("Selectionnez marque Dijoncteur ");

        //        //if (Cbo_NbresfilsDijoncteur.SelectedItem == null)
        //        //    throw new Exception("Selectionnez nombres de fils rque Dijoncteur ");
        //        //if (Cbo_ReglageCmpt.SelectedItem == null)
        //        //    throw new Exception("Selectionnez reglagle compteur");
        //        //if (Cbo_usage.SelectedItem == null)
        //        //    throw new Exception("Selectionnez usage");
        //        //if (Cbo_ListeAppareils.SelectedItem == null)
        //        //    throw new Exception("Selectionnez la liste des appareils");

        //        //if (Cbo_Produit.SelectedItem == null)
        //        //    throw new Exception("Selectionnez Produit");

        //        //if (string.IsNullOrEmpty(this.txt_CoffreFusile.Text))
        //        //    throw new Exception("remplir le coffre Fusile ");

        //        //if (string.IsNullOrEmpty(this.txt_certifiplombage.Text))
        //        //    throw new Exception("remplir le certifie plombage ");

        //        //if (string.IsNullOrEmpty(this.txt_refeplombs.Text))
        //        //    throw new Exception("remplir referend plomgs ");

        //        //if (string.IsNullOrEmpty(this.txt_reference_plombs.Text))
        //        //    throw new Exception("remplir referend plomgs ");
        //        //if (string.IsNullOrEmpty(this.DateAbonnemnt.SelectedDate.ToString()))
        //        //    throw new Exception("remplir la date ");
        //        //if (string.IsNullOrEmpty(this.DateBranchemnt.SelectedDate.ToString()))
        //        //    throw new Exception("remplir la date ");

        //        //if (string.IsNullOrEmpty(this.txt.Text))
        //        //        throw new Exception("remplir referend plomgs ");

        //        //if (((CsProduit)Cbo_Produit.SelectedItem).CODE != SessionObject.Enumere.ElectriciteMT)
        //        //{
        //        //    if (string.IsNullOrEmpty(this.txt_Reglage.Text))
        //        //        throw new Exception("Selectionnez le calibre ");
        //        //}
        //        #endregion

        //        #region Adresse géographique

        //        //if (Cbo_Centre.SelectedItem == null)
        //        //    throw new Exception("Selectionnez Centre");

        //        //if (Cbo_Quartier.SelectedItem == null)
        //        //    throw new Exception("Selectionnez Quartier");

        //        //if (Cbo_Commune.SelectedItem == null)
        //        //    throw new Exception("Selectionnez Commune");


        //        //if (Cbo_Rue.SelectedItem == null)
        //        //    throw new Exception("Selectionnez Rue");

        //        //if (Cbo_Secteur.SelectedItem == null)
        //        //    throw new Exception("Selectionnez Secteur");

        //        //if (string.IsNullOrEmpty(this.txtCentre.Text))
        //        //    throw new Exception("Séléctionnez le Centre ");

        //        //if (string.IsNullOrEmpty(this.txt_Commune.Text))
        //        //    throw new Exception("Séléctionnez la commune ");

        //        //if (string.IsNullOrEmpty(this.txt_Quartier.Text))
        //        //    throw new Exception("Séléctionnez le quartier ");


        //        #endregion

        //        return ReturnValue;

        //    }
        //    catch (Exception ex)
        //    {
        //        this.OKButton.IsEnabled = true;
        //        //this.OKButton.IsEnabled = true;
        //        Message.ShowInformation(ex.Message, "Reclamation");
        //        return false;
        //    }

        //}

        //private void Search()
        //{
        //    try
        //    {
        //        UserAgentPicker FormUserAgentPicker = new UserAgentPicker();
        //        FormUserAgentPicker.Closed += new EventHandler(FormUserAgentPicker_Closed);
        //        FormUserAgentPicker.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private void FormUserAgentPicker_Closed(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var form = (UserAgentPicker)sender;
        //        if (form != null)
        //        {
        //            if (form.DialogResult == true && form.AgentSelectionne != null)
        //            {
        //                var agent = form.AgentSelectionne;
        //                if (agent != null)
        //                {
        //                    this.TxtMatricule.Text = agent.MATRICULE;
        //                    this.TxtNomAgent.Text = agent.LIBELLE;
        //                    this.laDetailDemande.LaDemande.ISCONTROLE = true;
        //                }
        //            }
        //            else
        //                return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool IsConsultation = false;
    }
}


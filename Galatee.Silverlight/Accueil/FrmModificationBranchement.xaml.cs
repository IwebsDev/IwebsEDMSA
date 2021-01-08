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
using Galatee.Silverlight.Resources.Devis;
using System.IO;
using System.Collections.ObjectModel;


namespace Galatee.Silverlight.Accueil
{
    public partial class FrmModificationBranchement : ChildWindow
    {
        public FrmModificationBranchement()
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;

        }
        private UcImageScanne formScanne = null;

        CsBrt _LeBranchement = new CsBrt();
        public event EventHandler Closed;

        List<CsProduit> ListeDesProduitDuSite = new List<CsProduit>();
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        private byte[] image;
        string Tdem = string.Empty;
        CsDemande laDetailDemande = null;
        string CodeProduit = string.Empty;
        bool IsRejeterDemande = false;
        int? CommuneDuPoste = null;

        public FrmModificationBranchement(string  _typeDemande,string IsInit)
        {
            InitializeComponent();        
            Tdem = _typeDemande;
            ChargerDonneeDuSite();
            ChargerTypeDocument();
            ChargerPuissance();
            if (SessionObject.LsDesPosteElectriquesSource == null)
                ChargerPosteSource();
            if (SessionObject.LsDesDepartHTA == null)
                ChargerDepartHTA();
            if (SessionObject.LsDesPosteElectriquesTransformateur == null)
                ChargerPosteTransformation();

            this.Txt_ReferenceClient.MaxLength = SessionObject.Enumere.TailleClient;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }
        public FrmModificationBranchement(int IdDemandeDevis)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            ChargerTypeDocument();
            ChargerPuissance();
            if (SessionObject.LsDesPosteElectriquesSource == null)
                ChargerPosteSource();
            if (SessionObject.LsDesDepartHTA == null)
                ChargerDepartHTA();
            if (SessionObject.LsDesPosteElectriquesTransformateur == null)
                ChargerPosteTransformation();

            ChargeDetailDEvis(IdDemandeDevis);
            this.Txt_ReferenceClient.MaxLength = SessionObject.Enumere.TailleClient;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;

        }




        private void ChargerPosteSource()
        {
            if (SessionObject.LsDesPosteElectriquesSource != null && SessionObject.LsDesPosteElectriquesSource.Count != 0)
            {
                return;
            }
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerPosteSourceCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    SessionObject.LsDesPosteElectriquesSource = args.Result;
                } 
            };
            service.ChargerPosteSourceAsync();
            service.CloseAsync();
        }

        private void ChargerDepartHTA()
        {
            if (SessionObject.LsDesDepartHTA != null  && SessionObject.LsDesDepartHTA.Count != 0)
            {
                return;
            }
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerDepartHTACompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    SessionObject.LsDesDepartHTA = args.Result;
                }
            };
            service.ChargerDepartHTAAsync();
            service.CloseAsync();
        }


        private void ChargerPosteTransformation()
        {
            if (SessionObject.LsDesPosteElectriquesTransformateur != null && SessionObject.LsDesPosteElectriquesTransformateur.Count != 0)
            {
                return;
            }
            //Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            Galatee.Silverlight.ServiceParametrage.ParametrageClient service = new ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            service.SelectAllPosteTransformationCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {

                    CsPosteTransformation poste;
                    SessionObject.LsDesPosteElectriquesTransformateur.Clear();
                    foreach (var item in args.Result)
                    {
                        poste = new CsPosteTransformation();
                        poste.CODE = item.CODE;
                        poste.CODEDEPARTHTA = item.CODEDEPARTHTA;
                        poste.DATECREATION = item.DATECREATION;
                        poste.DATEMODIFICATION = item.DATEMODIFICATION;
                        poste.FK_IDDEPARTHTA = item.FK_IDDEPARTHTA;
                        poste.LIBELLE = item.LIBELLE;
                        poste.LIBELLEDEPARTHTA = item.LIBELLEDEPARTHTA;
                        poste.OriginalCODE = item.OriginalCODE;
                        poste.PK_ID = item.PK_ID;
                        poste.USERCREATION = item.USERCREATION;
                        poste.USERMODIFICATION = item.USERMODIFICATION;
                        SessionObject.LsDesPosteElectriquesTransformateur.Add(poste);
                    }
                }
            };
            service.SelectAllPosteTransformationAsync();
            service.CloseAsync();
        }







        void Translate()
        {
            this.lbl_longitude.Content = Langue.lbl_longitude;
            this.lbl_latitude.Content = Langue.lbl_latitude;
            
        }
        ObjDOCUMENTSCANNE leDoc = new ObjDOCUMENTSCANNE();
        private void  EnregisterInfo(CsDemande _LaDemande)
        {
            try
            {
                _LaDemande.Branchement.LONGBRT = string.IsNullOrEmpty(this.Txt_LongueurBrt.Text) ? 0 : Convert.ToDecimal(this.Txt_LongueurBrt.Text);
                _LaDemande.Branchement.ADRESSERESEAU = string.IsNullOrEmpty(this.Txt_AdresseElectrique.Text) ? null : this.Txt_AdresseElectrique.Text;
                _LaDemande.Branchement.LONGITUDE = string.IsNullOrEmpty(this.Txt_Longitude.Text) ? null : this.Txt_Longitude.Text;
                _LaDemande.Branchement.LATITUDE = string.IsNullOrEmpty(this.Txt_Latitude.Text) ? null : this.Txt_Latitude.Text;
                _LaDemande.Branchement.DRAC = null;
                if (!string.IsNullOrEmpty(this.Txt_DateRacordement .Text)) 
                   _LaDemande.Branchement.DRAC = Convert.ToDateTime(this.Txt_DateRacordement .Text);



                _LaDemande.Branchement.FK_IDTYPEBRANCHEMENT = this.Txt_TypeBrancehment.Tag == null ? _LaDemande.Branchement.FK_IDTYPEBRANCHEMENT : (int)this.Txt_TypeBrancehment.Tag;
                _LaDemande.Branchement.FK_IDPOSTESOURCE = this.Txt_PosteSource.Tag == null ? _LaDemande.Branchement.FK_IDPOSTESOURCE : (int)this.Txt_PosteSource.Tag;
                _LaDemande.Branchement.FK_IDPOSTETRANSFORMATION = this.Txt_PosteTransformateur.Tag == null ? _LaDemande.Branchement.FK_IDPOSTETRANSFORMATION : (int)this.Txt_PosteTransformateur.Tag;
                _LaDemande.Branchement.DEPARTBT = this.Txt_DepartBt.Tag == null ? _LaDemande.Branchement.DEPARTBT : this.Txt_DepartBt.Tag.ToString();
                _LaDemande.Branchement.FK_IDQUARTIER = this.Txt_QuarteirPoste.Tag == null ? _LaDemande.Branchement.FK_IDQUARTIER : (int)this.Txt_QuarteirPoste.Tag;
                _LaDemande.Branchement.FK_IDDEPARTHTA = this.Txt_DepartHTA.Tag == null ? _LaDemande.Branchement.FK_IDDEPARTHTA : (int)this.Txt_DepartHTA.Tag;
                _LaDemande.Branchement.NEOUDFINAL = string.IsNullOrEmpty(this.Txt_NeoudFinal.Text) ? null : this.Txt_NeoudFinal.Text;
                _LaDemande.Branchement.NOMBRETRANSFORMATEUR = string.IsNullOrEmpty(this.Txt_NbreTransformation.Text) ? 0 : int.Parse(this.Txt_NbreTransformation.Text);

                if (Cbo_Puissance.SelectedItem != null )
                    _LaDemande.Branchement.PUISSANCEINSTALLEE = Convert.ToDecimal((Cbo_Puissance.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsPuissance).VALEUR);

                _LaDemande.Branchement.USERCREATION = UserConnecte.matricule;
                _LaDemande.Branchement.USERMODIFICATION = UserConnecte.matricule;
                _LaDemande.Branchement.DATECREATION = System.DateTime.Now;
                _LaDemande.Branchement.DATEMODIFICATION = System.DateTime.Now;


                if (_LaDemande.LaDemande == null) _LaDemande.LaDemande = new CsDemandeBase();
                _LaDemande.LaDemande.CLIENT = this.Txt_ReferenceClient.Text;
                _LaDemande.LaDemande.MATRICULE = UserConnecte.matricule;
                _LaDemande.LaDemande.CENTRE = laDetailDemande.Branchement.CENTRE;
                _LaDemande.LaDemande.CLIENT = laDetailDemande.Branchement.CLIENT;
                _LaDemande.LaDemande.PRODUIT = laDetailDemande.Branchement.PRODUIT;
                _LaDemande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusEnAttente;
                _LaDemande.LaDemande.TYPEDEMANDE = Tdem;
                _LaDemande.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem) != null ? SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).PK_ID : 0;
                _LaDemande.LaDemande.FK_IDCENTRE = laDetailDemande.Branchement.FK_IDCENTRE;
                _LaDemande.LaDemande.FK_IDPRODUIT = (laDetailDemande.Branchement != null && _LaDemande.Branchement.FK_IDPRODUIT != null) ? _LaDemande.Branchement.FK_IDPRODUIT : _LaDemande.LaDemande.FK_IDPRODUIT;
                _LaDemande.LaDemande.PRODUIT = (laDetailDemande.Branchement != null && _LaDemande.Branchement.PRODUIT != null) ? _LaDemande.Branchement.PRODUIT : _LaDemande.LaDemande.PRODUIT;


                _LaDemande.LaDemande.USERCREATION = UserConnecte.matricule;
                _LaDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                _LaDemande.LaDemande.DATECREATION = System.DateTime.Now;
                _LaDemande.LaDemande.DATEMODIFICATION = System.DateTime.Now;
                _LaDemande.LaDemande.MOTIF = this.Txt_Motif.Text;


                #region Doc Scanne
                if (_LaDemande.ObjetScanne == null) laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                _LaDemande.ObjetScanne.AddRange(LstPiece);
                #endregion

                _LaDemande.Abonne = null;
                _LaDemande.Ag = null;
                _LaDemande.LstCanalistion = null;
                _LaDemande.LstEvenement = null;

            }
            catch (Exception ex)
            {
               Message.ShowError("Erreur à la récuperation des données","Info");
            }

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
            this.OKButton.IsEnabled = false;
            EnregisterInfo(laDetailDemande);
            if (!IsRejeterDemande)
                ValidationDemande(laDetailDemande);
            else
                ValidationDemandeSiteReprise(laDetailDemande);
        }
        private void ValidationDemande(CsDemande _LaDemande)
        {
            try
            {
                //Lancer la transaction de mise a jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.CreeDemandeAsync(_LaDemande,true);
                service1.CreeDemandeCompleted += (sr, res) =>
                {
                    if (res.Result != null)
                    {
                        Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + res.Result.NUMDEM,
                        Silverlight.Resources.Devis.Languages.txtDevis);
                        this.DialogResult = false;
                    }
                    else
                        Message.ShowError("Une erreur s'est produite a la création de la demande ", "CreeDemande");
                };
                service1.CloseAsync();

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        private void ValidationDemandeSiteReprise(CsDemande _LaDemande)
        {
            try
            {
                //Lancer la transaction de mise a jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.CreationDemandeSuiteRejetAsync(_LaDemande, true);
                service1.CreationDemandeSuiteRejetCompleted += (sr, res) =>
                {
                    if (string.IsNullOrEmpty(res.Result))
                    {
                        Message.ShowInformation("La demande transmise avec succes",
                        Silverlight.Resources.Devis.Languages.txtDevis);
                        this.DialogResult = false;
                    }
                    else
                        Message.ShowError("Une erreur s'est produite a la transmission de la demande ", "CreeDemande");

                };
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
 
        int InitValue = 0;
        void AfficherBranchemetDemande(CsBrt _LeBrtDemande)
        {

            if (_LeBrtDemande != null && _LeBrtDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
            {
                this.Cbo_Puissance.Visibility = System.Windows.Visibility.Collapsed;
                label_Puissance.Visibility = System.Windows.Visibility.Collapsed;
            }

            this.Txt_TypeBrancehment.Text = string.IsNullOrEmpty(_LeBrtDemande.CODETYPEBRANCHEMENT) ? string.Empty : _LeBrtDemande.CODETYPEBRANCHEMENT;
            this.Txt_LibelleTypeBRT.Text = string.IsNullOrEmpty(_LeBrtDemande.LIBELLETYPEBRANCHEMENT) ? string.Empty : _LeBrtDemande.LIBELLETYPEBRANCHEMENT;
            
            this.Txt_LongueurBrt.Text = string.IsNullOrEmpty(_LeBrtDemande.LONGBRT.ToString()) ? InitValue.ToString() : _LeBrtDemande.LONGBRT.ToString();
            this.Txt_DateRacordement.Text = string.IsNullOrEmpty(_LeBrtDemande.DRAC.ToString()) ? string.Empty : Convert.ToDateTime(_LeBrtDemande.DRAC).ToShortDateString();
            //this.Txt_NbreTransformation.Text = string.IsNullOrEmpty(_LeBrtDemande.NOMBRETRANSFORMATEUR.ToString()) ? InitValue.ToString() : _LeBrtDemande.NOMBRETRANSFORMATEUR.ToString();
            this.Txt_NbreTransformation.Text = (_LeBrtDemande.NOMBRETRANSFORMATEUR == null) ? string.Empty : _LeBrtDemande.NOMBRETRANSFORMATEUR.Value.ToString();

            this.Txt_Longitude.Text = string.IsNullOrEmpty(_LeBrtDemande.LONGITUDE) ? string.Empty : _LeBrtDemande.LONGITUDE;
            this.Txt_Latitude.Text = string.IsNullOrEmpty(_LeBrtDemande.LATITUDE) ? string.Empty : _LeBrtDemande.LATITUDE;

            this.Txt_AdresseElectrique.Text = string.IsNullOrEmpty(_LeBrtDemande.ADRESSERESEAU) ? string.Empty : _LeBrtDemande.ADRESSERESEAU;
            this.Txt_PosteSource.Text = string.IsNullOrEmpty(_LeBrtDemande.CODEPOSTESOURCE) ? string.Empty : _LeBrtDemande.CODEPOSTESOURCE;
            this.Txt_PosteTransformateur.Text = string.IsNullOrEmpty(_LeBrtDemande.CODETRANSFORMATEUR) ? string.Empty : _LeBrtDemande.CODETRANSFORMATEUR;
            this.Txt_DepartBt.Text = string.IsNullOrEmpty(_LeBrtDemande.DEPARTBT) ? string.Empty : _LeBrtDemande.DEPARTBT;
            this.Txt_DepartHTA.Text = string.IsNullOrEmpty(_LeBrtDemande.CODEDEPARTHTA) ? string.Empty : _LeBrtDemande.CODEDEPARTHTA;
            this.Txt_QuarteirPoste.Text = string.IsNullOrEmpty(_LeBrtDemande.CODEQUARTIER) ? string.Empty : _LeBrtDemande.CODEQUARTIER;
            this.Txt_NeoudFinal.Text = string.IsNullOrEmpty(_LeBrtDemande.NEOUDFINAL) ? string.Empty : _LeBrtDemande.NEOUDFINAL;

            if (_LeBrtDemande != null &&_LeBrtDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT   && laDetailDemande.Branchement.PUISSANCEINSTALLEE != null && laDetailDemande.Branchement.PUISSANCEINSTALLEE != 0)
            {
                List<ServiceAccueil.CsPuissance> lesPuissance = SessionObject.LstPuissanceInstalle.Where(t => t.PRODUIT == SessionObject.Enumere.ElectriciteMT).ToList();
                Cbo_Puissance.SelectedItem = lesPuissance.FirstOrDefault(p => p.VALEUR == _LeBrtDemande.PUISSANCEINSTALLEE);
                Cbo_Puissance.Tag = lesPuissance.FirstOrDefault(p => p.PK_ID == _LeBrtDemande.PUISSANCEINSTALLEE);
            }
        }

     
        private void Txt_Client_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_ReferenceClient.Text))
                this.Txt_ReferenceClient.Text = this.Txt_ReferenceClient.Text.PadLeft(SessionObject.Enumere.TailleClient, '0');
        }

        private void ChargerPuissance()
        {
            try
            {
                if (SessionObject.LstPuissanceInstalle != null && SessionObject.LstPuissanceInstalle.Count != 0)
                {

                    List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceInstalle;
                    if (_lstPuissance != null && _lstPuissance.Count != 0)
                    {
                        List<ServiceAccueil.CsPuissance> lesPuissance = _lstPuissance.Where(t => t.PRODUIT == SessionObject.Enumere.ElectriciteMT).ToList();
                        Cbo_Puissance.SelectedValuePath = "FK_IDPUISSANCE";
                        Cbo_Puissance.DisplayMemberPath = "VALEUR";
                        Cbo_Puissance.ItemsSource = lesPuissance;
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                    
                        return;
                    }
                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceInstalleCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissanceInstalle = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceInstalle;
                    if (_lstPuissance != null && _lstPuissance.Count != 0)
                    {
                        List<ServiceAccueil.CsPuissance> lesPuissance = _lstPuissance.Where(t => t.PRODUIT == SessionObject.Enumere.ElectriciteMT).ToList();
                        Cbo_Puissance.SelectedValuePath = "FK_IDPUISSANCE";
                        Cbo_Puissance.DisplayMemberPath = "VALEUR";
                        Cbo_Puissance.ItemsSource = lesPuissance;
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                        return;
                    }
                };
                service.ChargerPuissanceInstalleAsync();
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
        private void ChargerTypeBranchement()
        {
            try
            {
                if (SessionObject.LstTypeBranchement != null && SessionObject.LstTypeBranchement.Count != 0)
                {
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeBranchementCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    SessionObject.LstTypeBranchement = args.Result;
                };
                service.ChargerTypeBranchementAsync();
                service.CloseAsync();
            }
            catch (Exception es)
            {

                MessageBox.Show(es.Message);
            }
        }
        private void ChargerDiametreBranchement()
        {
            try
            {
                if (SessionObject.LstReglageCompteur != null && SessionObject.LstReglageCompteur.Count != 0)
                {
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerReglageCompteurCompleted  += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    SessionObject.LstReglageCompteur  = args.Result;
                };
                service.ChargerReglageCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception es)
            {

                MessageBox.Show(es.Message);
            }
        }
        private void ChargeQuartier()
        {
            try
            {
                if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
                    return ;
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerLesQartiersCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstQuartier = args.Result;
                    };
                    service.ChargerLesQartiersAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void btn_PosteSource_Click(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LsDesPosteElectriquesSource.Count != 0)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesPosteElectriquesSource);
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des poste sources");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_PosteSource);
                ctr.Show();
            }
        }

        void galatee_OkClickedbtn_PosteSource(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsPosteSource _LePoste = (Galatee.Silverlight.ServiceAccueil.CsPosteSource)ctrs.MyObject;
                if (_LePoste != null)
                {
                    this.Txt_PosteSource.Text = _LePoste.CODE;
                    this.Txt_LibellePosteSource.Text = _LePoste.LIBELLE;
                    this.Txt_PosteSource.Tag = _LePoste.PK_ID;
                    this.CommuneDuPoste = _LePoste.FK_IDCOMMUNE;
                }
            }
        }

        private void btn_depart_Click(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LsDesDepartHTA.Count != 0 && this.Txt_PosteSource.Tag != null)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesDepartHTA.Where(t => t.FK_IDPOSTESOURCE == (int)this.Txt_PosteSource.Tag).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des departs HTA");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_depart);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_depart(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsDepart _LeDepart = (Galatee.Silverlight.ServiceAccueil.CsDepart)ctrs.MyObject;
                if (_LeDepart != null)
                {
                    this.Txt_DepartHTA.Text = _LeDepart.CODE;
                    this.Txt_LibelleDepartHTA.Text = _LeDepart.LIBELLE;
                    this.Txt_DepartHTA.Tag = _LeDepart.PK_ID;
                }
            }
        }

        private void btn_PosteTransformateur_Click_1(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LsDesPosteElectriquesTransformateur.Count != 0 && this.Txt_DepartHTA.Tag != null)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesPosteElectriquesTransformateur.Where(t => t.FK_IDDEPARTHTA == (int)this.Txt_DepartHTA.Tag).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des postes transformateurs");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_PosteTransformateur);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_PosteTransformateur(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsPosteTransformation _LsPoste = (Galatee.Silverlight.ServiceAccueil.CsPosteTransformation)ctrs.MyObject;
                if (_LsPoste != null)
                {
                    this.Txt_PosteTransformateur.Text = _LsPoste.CODE;
                    this.Txt_LibellePosteTransformateur.Text = _LsPoste.LIBELLE;
                    this.Txt_PosteTransformateur.Tag = _LsPoste.PK_ID;

                }
            }
        }

        private void btn_TypeBrt_Click(object sender, RoutedEventArgs e)
        {
            if (laDetailDemande.Branchement.FK_IDPRODUIT != 0 && SessionObject.LstTypeBranchement.Count != 0)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstTypeBranchement.Where(t => t.FK_IDPRODUIT == laDetailDemande.Branchement.FK_IDPRODUIT).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeDiametre);
                ctr.Closed += new EventHandler(galatee_OkClickedBtnTypeBrt);
                ctr.Show();
            }

        }
        void galatee_OkClickedBtnTypeBrt(object sender, EventArgs e)
        {

            try
            {
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _LeTypeBrt = (Galatee.Silverlight.ServiceAccueil.CsTypeBranchement)ctrs.MyObject;
                    this.Txt_TypeBrancehment.Text = _LeTypeBrt.CODE;
                    this.Txt_LibelleTypeBRT.Text = _LeTypeBrt.LIBELLE;
                    this.Txt_TypeBrancehment.Tag = _LeTypeBrt.PK_ID;
                }
                this.btn_TypeBrt.IsEnabled = true;

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void btn_QuartierPoste_Click_1(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
            {
                List<CsQuartier> LstQuartierSite;
                if (this.CommuneDuPoste != null)
                    LstQuartierSite = SessionObject.LstQuartier.Where(t => t.FK_IDCOMMUNE == this.CommuneDuPoste.Value).ToList();
                else
                    LstQuartierSite = SessionObject.LstQuartier;

                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstQuartierSite);
                Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                _LstColonneAffich.Add("CODE", "CODE");
                _LstColonneAffich.Add("LIBELLE", "QUARTIER");
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeDiametre);
                ctrl.Closed += new EventHandler(galatee_OkClickedBtnQuartier);
                ctrl.Show();
            }
        }
        void galatee_OkClickedBtnQuartier(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsQuartier _LeQuartier = (ServiceAccueil.CsQuartier)ctrs.MyObject;
                if (_LeQuartier != null)
                {
                    this.Txt_LibelleQuartier.Text = _LeQuartier.LIBELLE;
                    this.Txt_QuarteirPoste.Text = _LeQuartier.CODE ;
                    Txt_QuarteirPoste.Tag  = _LeQuartier.PK_ID ;
                }
            }
        }
        private void Txt_QuartierPoste_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenereCodification();
        }

        private void Txt_SequenceNumPoste_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenereCodification();
        }

        private void Txt_Depart_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Txt_PosteTransformateur.Text = this.Txt_LibellePosteTransformateur.Text = string.Empty;
            GenereCodification();
        }

        private void Txt_NeoudFinal_TextChanged(object sender, TextChangedEventArgs e)
        {

            GenereCodification();

        }
        void GenereCodification()
        {
            if (!string.IsNullOrEmpty(this.Txt_PosteSource.Text) &&
                !string.IsNullOrEmpty(this.Txt_DepartHTA.Text) &&
                !string.IsNullOrEmpty(this.Txt_QuarteirPoste.Text) &&
                !string.IsNullOrEmpty(this.Txt_PosteTransformateur.Text) &&
                !string.IsNullOrEmpty(this.Txt_DepartBt.Text) &&
                !string.IsNullOrEmpty(this.Txt_NeoudFinal.Text))
                this.Txt_AdresseElectrique.Text =
                    (this.Txt_PosteSource.Text + this.Txt_DepartHTA.Text + this.Txt_QuarteirPoste.Text +
                     this.Txt_PosteTransformateur.Text + this.Txt_DepartBt.Text +
                     this.Txt_NeoudFinal.Text);
            else
                this.Txt_AdresseElectrique.Text = string.Empty;
        }

        private void Txt_TypeBrancehment_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (laDetailDemande.Branchement.FK_IDPRODUIT != 0 && SessionObject.LstTypeBranchement.Count != 0)
                {
                    if (this.Txt_TypeBrancehment.Text.Length == SessionObject.Enumere.TailleTypeBranchement
                        && SessionObject.LstTypeBranchement.Count != 0)
                    {
                        CsTypeBranchement _leTypeBrt = ClasseMEthodeGenerique.RetourneObjectFromList(SessionObject.LstTypeBranchement, this.Txt_TypeBrancehment.Text, "CODE");
                        if (_leTypeBrt != null && !string.IsNullOrEmpty(_leTypeBrt.LIBELLE))
                        {
                            this.Txt_LibelleTypeBRT.Text = _leTypeBrt.LIBELLE;
                            this.Txt_TypeBrancehment.Tag = _leTypeBrt.PK_ID;
                        }
                        else
                        {
                            var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            w.OnMessageBoxClosed += (_, result) =>
                            {
                                this.Txt_TypeBrancehment.Focus();
                            };
                            w.Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
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
                    Message.Show("La référence saisie n'est pas correcte", "Information");
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneClientByReferenceCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    if (args.Result != null && args.Result.Count > 1)
                    {
                        List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(args.Result);
                        Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CENTRE", "LIBELLESITE", "Liste des sites");
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
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneDemandeClientTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result != null)
                        {
                            if (args.Result.DATEFIN == null && args.Result.ISSUPPRIME != true)
                            {
                                Message.ShowInformation("Il existe une demande numéro " + args.Result.NUMDEM + " sur ce client", "Accueil");
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
                prgBar.Visibility = System.Windows.Visibility.Visible;

                leclient.TYPEDEMANDE = Tdem;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.ChargerDetailClientAsync(leclient);
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
                            this.txt_Produit.Tag = laDetailDemande.Branchement .FK_IDPRODUIT;
                            this.txtCentre.Tag = laDetailDemande.Branchement.FK_IDCENTRE;
                            CodeProduit = laDetailDemande.Branchement.PRODUIT;
                            this.txt_tdem.Text = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem).LIBELLE;
                            txt_tdem.Tag = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == Tdem);
                            AfficherBranchemetDemande(laDetailDemande.Branchement );
                            AfficherDocumentScanne(laDetailDemande.ObjetScanne);
                        }
                        else
                        {
                            Message.ShowInformation("Cet abonné est résilié", "Information");
                            return;
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError("Erreur au chargement des données", "Demande");
            }
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {
            prgBar.Visibility = System.Windows.Visibility.Visible;

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeCompleted  += (ssender, args) =>
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
                    //if (laDetailDemande.LstCommentaire != null && laDetailDemande.LstCommentaire.Count != 0)
                    if (laDetailDemande.AnnotationDemande != null && laDetailDemande.AnnotationDemande.Count != 0)
                        this.Txt_MotifRejet.Text = laDetailDemande.AnnotationDemande.First().COMMENTAIRE;


                    AfficherBranchemetDemande(laDetailDemande.Branchement );
                    AfficherDocumentScanne(laDetailDemande.ObjetScanne);
                    IsRejeterDemande = true;
                }

            };
            client.ChargerDetailDemandeAsync(IdDemandeDevis,string.Empty );
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

        private void Txt_PosteSource_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Txt_QuarteirPoste.Text = this.Txt_LibelleQuartier.Text = string.Empty;
            this.Txt_DepartHTA.Text = this.Txt_LibelleDepartHTA.Text = string.Empty;
            this.Txt_PosteTransformateur.Text = this.Txt_LibellePosteTransformateur.Text = string.Empty;
            GenereCodification();
        }

        private void Txt_PosteTransformateur_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenereCodification();
        }

        private void Txt_DepartBT_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenereCodification();
        }


    }
}


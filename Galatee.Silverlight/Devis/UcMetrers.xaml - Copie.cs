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
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using System.Globalization;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Devis
{
    public partial class UcMetrers : UserControl
    {
        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = new CsDemande();
  
        decimal distanceMaxiSubvention;
        decimal distanceMaxi;
        decimal seuilDistance;
      
        private List<ObjDOCUMENTSCANNE> ObjetScanne = new List<ObjDOCUMENTSCANNE>();
        private List<ObjDOCUMENTSCANNE> ObjetScanneFraix = new List<ObjDOCUMENTSCANNE>();
        List<Galatee.Silverlight.ServiceAccueil.CsTypeBranchement > LstTypeBrt;

        List<CsFraixParticipation> LstFraixParticipation = new List<CsFraixParticipation>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();

        ChildWindow Parent = new ChildWindow();
        public UcMetrers()
        {

            InitializeComponent();
            ChargerTypeDocument();
            //RemplirPuissanceSouscrite();
            btn_MotifRejet.Visibility = Visibility.Collapsed;

        }

 
        public UcMetrers(CsDemande laDetailDemande, ChildWindow Parent)
        {
            try
            {
                
                InitializeComponent();
                this.Parent = Parent;

                this.Txt_PosteSource.MaxLength = 3;
                this.Txt_DepartHTA.MaxLength = 3;
                this.Txt_QuarteirPoste.MaxLength = 4;
                this.Txt_PosteTransformateur.MaxLength = 4;
                this.Txt_DepartBt.MaxLength =2;
                this.Txt_NeoudFinal.MaxLength =4;
                this.Txt_Distance_Extension.Visibility = System.Windows.Visibility.Collapsed;
                this.labelDistanceExtension.Visibility = System.Windows.Visibility.Collapsed;
                this.laDetailDemande = laDetailDemande;
                if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement &&
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonementExtention &&
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementEp &&
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementMt)
                    Chk_BranchementAvecExtension.Visibility = System.Windows.Visibility.Visible;
                else
                    this.Chk_ChangementDeCompteur.Visibility = System.Windows.Visibility.Collapsed;

                ChargerTypeDocument();
                RenseignerInformationsDevis(laDetailDemande);

                btn_MotifRejet.Visibility = Visibility.Collapsed;
                if (laDetailDemande != null && laDetailDemande.LstCommentaire != null && laDetailDemande.LstCommentaire.Count != 0)
                    btn_MotifRejet.Visibility = Visibility.Visible ;

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void ChargerTypeDocument()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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

        private void ChargeDetailDemande()
        {
            laDemandeSelect = laDetailDemande.LaDemande;
            InitDonnee();
        }

        private void RemplirListeDesDiametresExistant(CsDemande laDemande)
        {

            try
            {
                if (SessionObject.LstReglageCompteur != null && SessionObject.LstReglageCompteur.Count != 0)
                {

                    if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceParReglageCompteur;
                        if (_lstPuissance != null && _lstPuissance.Count != 0)
                        {
                            List<ServiceAccueil.CsPuissance> lesPuissanceRegalage = _lstPuissance.Where(t => t.FK_IDPRODUIT == laDetailDemande.LaDemande.FK_IDPRODUIT && t.VALEUR == laDetailDemande.LaDemande.PUISSANCESOUSCRITE).ToList();
                            List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> lstReglageCompteur = SessionObject.LstReglageCompteur.Where(p => p.FK_IDPRODUIT == laDemande.LaDemande.FK_IDPRODUIT && lesPuissanceRegalage.Select(y => y.FK_IDREGLAGECOMPTEUR).Contains(p.PK_ID)).ToList();
                            Cbo_diametre.SelectedValuePath = "PK_ID";
                            Cbo_diametre.DisplayMemberPath = "LIBELLE";
                            Cbo_diametre.ItemsSource = null;
                            Cbo_diametre.ItemsSource = lstReglageCompteur;
                            if (lstReglageCompteur != null && lstReglageCompteur.Count == 1)
                            {
                                Cbo_diametre.SelectedItem = lstReglageCompteur.First();
                                Cbo_diametre.Tag = lstReglageCompteur.First();
                            }
                            if (laDemande != null && laDemande.LaDemande != null && !string.IsNullOrEmpty(laDemande.LaDemande.REGLAGECOMPTEUR))
                            {
                                Cbo_diametre.SelectedItem = lstReglageCompteur.FirstOrDefault(p => p.CODE == laDemande.LaDemande.REGLAGECOMPTEUR);
                                Cbo_diametre.Tag = lstReglageCompteur.FirstOrDefault(p => p.CODE == laDemande.LaDemande.REGLAGECOMPTEUR);
                            }
                        }
                        return;
                    }
                    List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> listeReglageCompteurExistant = SessionObject.LstReglageCompteur;
                    List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> lstDiam = listeReglageCompteurExistant.Where(p => p.FK_IDPRODUIT == laDemande.LaDemande.FK_IDPRODUIT).ToList();
                    Cbo_diametre.SelectedValuePath = "PK_ID";
                    Cbo_diametre.DisplayMemberPath = "LIBELLE";
                    Cbo_diametre.ItemsSource =lstDiam ;
                    if (lstDiam != null && lstDiam.Count == 1)
                    {
                        Cbo_diametre.SelectedItem = lstDiam.First();
                        Cbo_diametre.Tag  = lstDiam.First();
                    }
                    if (laDemande != null && laDemande.LaDemande != null && !string.IsNullOrEmpty(laDemande.LaDemande.REGLAGECOMPTEUR ))
                    {
                        Cbo_diametre.SelectedItem = lstDiam.FirstOrDefault(p => p.CODE == laDemande.LaDemande.REGLAGECOMPTEUR);
                        Cbo_diametre.Tag = lstDiam.FirstOrDefault(p => p.CODE == laDemande.LaDemande.REGLAGECOMPTEUR);
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerReglageCompteurCompleted  += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    SessionObject.LstReglageCompteur  = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> listeReglageCompteurExistant = SessionObject.LstReglageCompteur;
                    List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> lstDiam = listeReglageCompteurExistant.Where(p => p.FK_IDPRODUIT == laDemande.LaDemande.FK_IDPRODUIT).ToList();
                    Cbo_diametre.SelectedValuePath = "PK_ID";
                    Cbo_diametre.DisplayMemberPath = "LIBELLE";
                    Cbo_diametre.ItemsSource = lstDiam;
                    if (lstDiam != null && lstDiam.Count == 1)
                    {
                        Cbo_diametre.SelectedItem = lstDiam.First();
                        Cbo_diametre.Tag = lstDiam.First();
                    }
                    if (laDemande != null && laDemande.LaDemande != null && !string.IsNullOrEmpty(laDemande.LaDemande.REGLAGECOMPTEUR))
                    {
                        Cbo_diametre.SelectedItem = listeReglageCompteurExistant.FirstOrDefault(p => p.CODE == laDemande.LaDemande.REGLAGECOMPTEUR);
                        Cbo_diametre.Tag = listeReglageCompteurExistant.FirstOrDefault(p => p.CODE == laDemande.LaDemande.REGLAGECOMPTEUR);
                    }
                    return;
                };
                service.ChargerReglageCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception es)
            {

                MessageBox.Show(es.Message);
            }
        }
        private void RemplirTourneeExistante(int pCentreId)
        {
            try
            {
                if (SessionObject.LstZone != null && SessionObject.LstZone.Count != 0)
                {

                    List<Galatee.Silverlight.ServiceAccueil.CsTournee> lstTournee = SessionObject.LstZone;
                    
                    Cbo_Zone.SelectedValuePath = "PK_ID";
                    Cbo_Zone.DisplayMemberPath = "CODE";
                    Cbo_Zone.ItemsSource = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId);
                    if (lstTournee.Where(t => t.FK_IDCENTRE == pCentreId) != null && lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).ToList().Count  == 1)
                    {
                        Cbo_Zone.SelectedItem = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).First();
                        Cbo_Zone.Tag = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).First();
                    }
                    if (laDetailDemande.Ag != null && laDetailDemande.Ag.FK_IDTOURNEE != null && laDetailDemande.Ag.FK_IDTOURNEE != 0)
                    {
                        Cbo_Zone.SelectedItem = lstTournee.FirstOrDefault(p => p.PK_ID == laDetailDemande.Ag.FK_IDTOURNEE);
                        Cbo_Zone.Tag = lstTournee.FirstOrDefault(p => p.PK_ID  == laDetailDemande.Ag.FK_IDTOURNEE);
                    
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesTourneesCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstZone = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsTournee> lstTournee = SessionObject.LstZone;
                    Cbo_Zone.SelectedValuePath = "PK_ID";
                    Cbo_Zone.DisplayMemberPath = "CODE";
                    Cbo_Zone.ItemsSource = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId);
                    if (lstTournee.Where(t => t.FK_IDCENTRE == pCentreId) != null && lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).ToList().Count == 1)
                    {
                        Cbo_Zone.SelectedItem = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).First();
                        Cbo_Zone.Tag = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).First();
                    }
                    if (laDetailDemande.Ag != null && laDetailDemande.Ag.FK_IDTOURNEE != null && laDetailDemande.Ag.FK_IDTOURNEE != 0)
                    {
                        Cbo_Zone.SelectedItem = lstTournee.FirstOrDefault(p => p.PK_ID == laDetailDemande.Ag.FK_IDTOURNEE);
                        Cbo_Zone.Tag = lstTournee.FirstOrDefault(p => p.PK_ID == laDetailDemande.Ag.FK_IDTOURNEE);

                    }
                };
                service.ChargerLesTourneesAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        private void RemplirPuissanceSouscrite(Galatee.Silverlight.ServiceAccueil.CsReglageCompteur   leReglageCompteur)
        {
            try
            {
                if (SessionObject.LstPuissance != null && SessionObject.LstPuissanceParReglageCompteur.Count != 0)
                {

                    List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceParReglageCompteur;
                    if (_lstPuissance != null && _lstPuissance.Count != 0)
                    {
                        List<ServiceAccueil.CsPuissance> lesPuissance = _lstPuissance.Where(t => t.FK_IDPRODUIT == leReglageCompteur.FK_IDPRODUIT && t.FK_IDREGLAGECOMPTEUR == leReglageCompteur.PK_ID).ToList();
                        Cbo_Puissance.ItemsSource = null ;
                        Cbo_Puissance.ItemsSource = lesPuissance;
                        Cbo_Puissance.DisplayMemberPath = "VALEUR";
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                    }
                    return;
                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceReglageCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissanceParReglageCompteur = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceParReglageCompteur;
                    if (_lstPuissance != null && _lstPuissance.Count != 0)
                    {

                        List<ServiceAccueil.CsPuissance> lesPuissance = _lstPuissance.Where(t => t.FK_IDPRODUIT == leReglageCompteur.FK_IDPRODUIT && t.FK_IDREGLAGECOMPTEUR == leReglageCompteur.PK_ID).ToList();
                        Cbo_Puissance.ItemsSource = null ;
                        Cbo_Puissance.ItemsSource = lesPuissance;
                        Cbo_Puissance.DisplayMemberPath = "VALEUR";
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                        return;
                    }
                };
                service.ChargerPuissanceReglageCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        private void ChargerDiametreBranchement(CsDemande laDemande)
        {
            try
            {
                if (SessionObject.LstTypeBranchement != null && SessionObject.LstTypeBranchement.Count != 0)
                {
                    LstTypeBrt = SessionObject.LstTypeBranchement.Where(p => p.PRODUIT == laDemande.LaDemande.PRODUIT).ToList();
                    if (LstTypeBrt != null && LstTypeBrt.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text))
                        {
                            Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _LeDiametre = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeBrt, this.Txt_TypeBrancehment.Text, "CODE");
                            if (_LeDiametre != null && !string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                                this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                        }
                    }
                
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeBranchementCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    SessionObject.LstTypeBranchement = args.Result;
                    LstTypeBrt = SessionObject.LstTypeBranchement.Where(p => p.PRODUIT == laDemande.LaDemande.PRODUIT).ToList();
                    if (LstTypeBrt != null && LstTypeBrt.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text))
                        {
                            Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _LeDiametre = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeBrt, this.Txt_TypeBrancehment.Text, "CODE");
                            if (_LeDiametre != null && !string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                                this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                        }
                    }
                };
                service.ChargerTypeBranchementAsync();
                service.CloseAsync();
            }
            catch (Exception es)
            {

                MessageBox.Show(es.Message);
            }

        }
        private void ChargerListeDesPostesSource()
        {
            if (SessionObject.LsDesPosteElectriquesSource.Count != 0)
            {
                return;
            }
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerpPosteSourceCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LsDesPosteElectriquesSource = args.Result;
            };
            service.ChargerpPosteSourceAsync();
            service.CloseAsync();
        }
        private void RetourneListeDesDepartHta()
        {
            if (SessionObject.LsDesDepartHTA.Count != 0)
            {
                return;
            }
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerDepartHTACompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LsDesDepartHTA = args.Result;
            };
            service.ChargerDepartHTAAsync();
            service.CloseAsync();
        }
        private void ChargerListeDesPostesTransformation()
        {
            if (SessionObject.LsDesPosteElectriquesTransformateur.Count != 0)
            {
                return;
            }
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerPosteTransformateurCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LsDesPosteElectriquesTransformateur = args.Result;
            };
            service.ChargerPosteTransformateurAsync();
            service.CloseAsync();
        }
        private void RetourneListeDesDepartBT()
        {
            if (SessionObject.LsDesDepartBT.Count != 0)
            {
                return;
            }
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerDepartBTCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LsDesDepartBT = args.Result;
            };
            service.ChargerDepartBTAsync();
            service.CloseAsync();
        }
        private void ChargeQuartier(CsDemande laDemande)
        {
            try
            {
                if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
                {
                    LstQuartierSite = SessionObject.LstQuartier.Where(t => t.COMMUNE == laDemande.Ag.COMMUNE).ToList();
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerLesQartiersCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstQuartier = args.Result;
                        LstQuartierSite = SessionObject.LstQuartier;
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

        private void RetourneOrdre(CsClient leClient,bool IsExonneration)
        {
            try
            {
                string OrdreMax = string.Empty;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneOrdreMaxCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    OrdreMax = args.Result;
                    if (OrdreMax != null)
                    {
                        this.Txt_Ordre.Text = OrdreMax;
                        if (!IsExonneration)
                            GetInformationFromChildWindowImagePreuveFraixP(null, null);
                        else
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
                                    imageFraix = memoryStream.GetBuffer();
                                    formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                                    formScanne.Closed += new EventHandler(GetInformationFromChildWindowImagePreuveFraixP);
                                    formScanne.Show();
                                }
                            }
                        }
                    }
                    else
                    {
                        Message.ShowInformation("Ce client n'existe pas ", "Information");
                        this.txt_montant.Text = string.Empty;
                        this.txt_refClient.Text = string.Empty;
                    }
                };
                service.RetourneOrdreMaxAsync(leClient.FK_IDCENTRE.Value, leClient.CENTRE, leClient.REFCLIENT, leClient.PRODUIT);
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        List<Galatee.Silverlight.ServiceAccueil.CsQuartier> LstQuartierSite = new List<Galatee.Silverlight.ServiceAccueil.CsQuartier>();
        List<Galatee.Silverlight.ServiceAccueil.CsDepart> LstDepart = new List<Galatee.Silverlight.ServiceAccueil.CsDepart>();
        private void Txt_CodeDiametre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_TypeBrancehment.Text.Length == SessionObject.Enumere.TailleDiametreBranchement && (LstTypeBrt != null && LstTypeBrt.Count != 0))
                {
                    Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _leDiametre = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneObjectFromList<Galatee.Silverlight.ServiceAccueil.CsTypeBranchement>(LstTypeBrt, this.Txt_TypeBrancehment.Text, "CODE");
                    if (_leDiametre != null && !string.IsNullOrEmpty(_leDiametre.LIBELLE))
                    {
                        this.Txt_LibelleDiametre.Text = _leDiametre.LIBELLE;
                        this.Txt_TypeBrancehment.Tag = _leDiametre.PK_ID;

                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_TypeBrancehment.Focus();
                            this.Txt_TypeBrancehment.Text = string.Empty;
                            this.Txt_LibelleDiametre.Text = string.Empty;
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }

        }

        //private void ChargeDeparts(CsDemande laDemande)
        //{
        //    if (SessionObject.LsDesDepart != null && SessionObject.LsDesDepart.Count != 0)
        //    {
        //        LstDepart = SessionObject.LsDesDepart.Where(t => t.CENTRE == laDemande.LaDemande.CENTRE && t.FK_IDCENTRE == laDemande.LaDemande.FK_IDCENTRE).ToList();
        //        return;
        //    }
        //    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //    service.ChargerDepartCompleted += (s, args) =>
        //    {
        //        if (args != null && args.Cancelled)
        //            return;
        //        SessionObject.LsDesDepart = args.Result;
        //        LstDepart = SessionObject.LsDesDepart.Where(t => t.CENTRE == laDemande.LaDemande.CENTRE && t.FK_IDCENTRE == laDemande.LaDemande.FK_IDCENTRE).ToList();
        //    };
        //    service.ChargerDepartAsync();
        //    service.CloseAsync();
        //}

        private void btn_diametre_Click(object sender, RoutedEventArgs e)
        {
            this.btn_diametre.IsEnabled = false;
            if (LstTypeBrt.Count != 0)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstTypeBrt);
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des type de branchement");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnDiametre);
                ctr.Show();
            }

        }
        void galatee_OkClickedBtnDiametre(object sender, EventArgs e)
        {

            try
            {
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _LeDiametre = (Galatee.Silverlight.ServiceAccueil.CsTypeBranchement)ctrs.MyObject;
                    this.Txt_TypeBrancehment.Text = _LeDiametre.CODE;
                    this.Txt_TypeBrancehment.Tag = _LeDiametre.PK_ID;
                }
                this.btn_diametre.IsEnabled = true;

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void btn_QuartierPoste_Click_1(object sender, RoutedEventArgs e)
        {
            this.btn_QuartierPoste.IsEnabled = false;
            if (LstQuartierSite != null && LstQuartierSite.Count != 0)
            {
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
            this.btn_QuartierPoste.IsEnabled = true;
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsQuartier _LeQuartier = (ServiceAccueil.CsQuartier)ctrs.MyObject;
                if (_LeQuartier != null)
                {
                    this.Txt_LibelleQuartier.Text = _LeQuartier.LIBELLE;
                    Txt_QuarteirPoste.Text = _LeQuartier.CODE;
                    Txt_QuarteirPoste.Tag = _LeQuartier.PK_ID;
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
            GenereCodification();
        }

        private void Txt_NeoudFinal_TextChanged(object sender, TextChangedEventArgs e)
        {

            GenereCodification();

        }
        void GenereCodification()
        {
            if (!string.IsNullOrEmpty(this.Txt_PosteSource .Text) &&
                !string.IsNullOrEmpty(this.Txt_DepartHTA .Text) &&
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


        private void RenseignerInformationsDevis(CsDemande laDetailDemande)
        {
            if (laDetailDemande.LaDemande != null)
            {
                Txt_NumeroDevis.Text = laDetailDemande.LaDemande.NUMDEM != null ? laDetailDemande.LaDemande.NUMDEM : string.Empty;
                Txt_TypeDevis.Text = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE != null ? laDetailDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;
                Txt_Produit.Text = laDetailDemande.LaDemande.LIBELLEPRODUIT != null ? laDetailDemande.LaDemande.LIBELLEPRODUIT : string.Empty;
                Txt_DateRendezVous.Text = laDetailDemande.LaDemande.DPRDEV != null ? laDetailDemande.LaDemande.DPRDEV.Value.ToShortDateString() : string.Empty;
                #region Sylla

                if (!string.IsNullOrWhiteSpace(laDetailDemande.LaDemande.LIBELLETDEM) && laDetailDemande.LaDemande.TYPEDEMANDE  == SessionObject.Enumere.BranchementAbonementExtention)
                    Chk_BranchementAvecExtension.IsChecked = true;
                else
                    Chk_BranchementAvecExtension.IsChecked = false;
                #endregion
            }
            else
            {
                Message.ShowError("Les informations de la demande ne sont pas renseigné ", Languages.txtDevis);
            }


            // Charger données du devis
            if (laDetailDemande.Branchement != null)
            {
                Txt_Distance.Text = laDetailDemande.Branchement.LONGBRT.ToString();
                Txt_BranchementProche.Text = laDetailDemande.Branchement.NBPOINT.ToString();
                if (laDetailDemande.Branchement != null && !string.IsNullOrEmpty(laDetailDemande.Branchement.DIAMBRT))
                {
                    foreach (Galatee.Silverlight.ServiceAccueil.CsReglageCompteur  diametre in Cbo_diametre.Items)
                    {
                        if (diametre.CODE == laDetailDemande.Branchement.DIAMBRT && diametre.CODEPRODUIT == laDetailDemande.Branchement.PRODUIT)
                        {
                            Cbo_diametre.SelectedItem = diametre;
                            ActiverEnregistrer();
                            break;
                        }
                    }
                }

                if (laDetailDemande.Branchement.PUISSANCEINSTALLEE != null && laDetailDemande.Branchement.PUISSANCEINSTALLEE != 0)
                {
                    foreach (Galatee.Silverlight.ServiceAccueil.CsPuissance puissance in Cbo_Puissance.Items)
                    {
                        if (puissance.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE)
                        {
                            Cbo_Puissance.SelectedItem = puissance;
                            ActiverEnregistrer();
                            break;
                        }
                    }
                }

                this.Txt_TypeBrancehment.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.CODEBRT) ? string.Empty : laDetailDemande.Branchement.CODEBRT;
                if (LstTypeBrt != null)
                {
                    ServiceAccueil.CsTypeBranchement _leDiametre = Shared.ClasseMEthodeGenerique.RetourneObjectFromList<ServiceAccueil.CsTypeBranchement>(LstTypeBrt, this.Txt_TypeBrancehment.Text, "CODE");
                    if (_leDiametre != null && !string.IsNullOrEmpty(_leDiametre.LIBELLE))
                        this.Txt_LibelleDiametre.Text = _leDiametre.LIBELLE;
                }
                if (laDetailDemande.LstFraixParticipation  != null)
                {
                    this.LstFraixParticipation = laDetailDemande.LstFraixParticipation;
                    this.dgListeFraixParicipation.ItemsSource = this.LstFraixParticipation;
                    CalculerMonantFraix();
                }

                TxtOrdreTournee.Text = (this.laDetailDemande.Ag != null && !string.IsNullOrEmpty(this.laDetailDemande.Ag.ORDTOUR)) ? this.laDetailDemande.Ag.ORDTOUR : string.Empty;
                TxtLatitude.Text = (this.laDetailDemande.Branchement != null && !string.IsNullOrEmpty(this.laDetailDemande.Branchement.LATITUDE)) ? this.laDetailDemande.Branchement.LATITUDE : string.Empty;
                TxtLongitude.Text = (this.laDetailDemande.Branchement != null && !string.IsNullOrEmpty(this.laDetailDemande.Branchement.LONGITUDE)) ? this.laDetailDemande.Branchement.LONGITUDE : string.Empty;
                Txt_AdresseElectrique.Text = (this.laDetailDemande.Branchement != null && !string.IsNullOrEmpty(this.laDetailDemande.Branchement.ADRESSERESEAU)) ? this.laDetailDemande.Branchement.ADRESSERESEAU : string.Empty;
            }




            #region DocumentScanne
            if (laDetailDemande.ObjetScanne != null && laDetailDemande.ObjetScanne.Count != 0)
            {
                //isPreuveSelectionnee = true;
                ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                LstPiece = new List<ObjDOCUMENTSCANNE>();
                foreach (var item in laDetailDemande.ObjetScanne)
                {
                    LstPiece.Add(item);
                    ObjetScanne.Add(item);
                }
                dgListePiece.ItemsSource = null;
                dgListePiece.ItemsSource = ObjetScanne;

            }
            else
            {
                //isPreuveSelectionnee = false;
            }
            #endregion
            if (this.laDetailDemande.Ag != null && this.laDetailDemande.Ag.FK_IDTOURNEE != 0 && laDetailDemande.Ag.FK_IDTOURNEE != null)
            {
                foreach (ServiceAccueil.CsTournee tournee in Cbo_Zone.Items)
                {
                    if (tournee.PK_ID == this.laDetailDemande.Ag.FK_IDTOURNEE)
                    {
                        Cbo_Zone.SelectedItem = tournee;
                        ActiverEnregistrer();
                        break;
                    }
                }
                TxtOrdreTournee.Text = string.IsNullOrEmpty(this.laDetailDemande.Ag.ORDTOUR) ? string.Empty : this.laDetailDemande.Ag.ORDTOUR;
            }
            if (this.laDetailDemande.LaDemande != null)
            {
                this.Txt_NumeroDevis.Text = !string.IsNullOrEmpty(this.laDetailDemande.LaDemande.NUMDEM) ? this.laDetailDemande.LaDemande.NUMDEM : string.Empty;
                this.Txt_TypeDevis.Text = !string.IsNullOrEmpty(this.laDetailDemande.LaDemande.LIBELLETYPEDEMANDE) ? this.laDetailDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;
                if (this.laDetailDemande.Branchement != null && this.laDetailDemande.Branchement.LONGBRT != null && this.laDetailDemande.Branchement.LONGBRT > 0)
                    this.Txt_Distance.Text = !string.IsNullOrEmpty(this.laDetailDemande.Branchement.LONGBRT.Value.ToString(SessionObject.FormatMontant)) ? this.laDetailDemande.Branchement.LONGBRT.Value.ToString(SessionObject.FormatMontant) : string.Empty;
                if (string.IsNullOrEmpty(this.Txt_Distance.Text))
                {
                    this.Txt_Distance.SelectAll();
                    this.Txt_Distance.Focus();
                }
                //ChkFraisDeParticipation.IsChecked = Convert.ToBoolean(InformationsDevis.Devis.ISPARTICIPATION);
                //TxtFraisDeParticipation.Text = InformationsDevis.Devis.MONTANTPARTICIPATION != null ? decimal.Parse(InformationsDevis.Devis.MONTANTPARTICIPATION.ToString()).ToString(DataReferenceManager.FormatMontant) : string.Empty;
                //ChkFraisDeParticipation.IsChecked = InformationsDevis.Devis.MONTANTPARTICIPATION != null ? true :false ;

                if (!string.IsNullOrEmpty(TxtFraisDeParticipation.Text))
                    TxtFraisDeParticipation.Visibility = System.Windows.Visibility.Visible;
                else
                    TxtFraisDeParticipation.Visibility = System.Windows.Visibility.Collapsed;
            }

            ActiverEnregistrer();
        }


        private void OuvrireEtablissementDevis()
        {
            try
            {
                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.ValiderMetreCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                };
                clientDevis.ValiderMetreAsync(laDetailDemande,false );
                UcEtablissementDevis ctr = new UcEtablissementDevis(laDetailDemande);
                ctr.Closed += ctr_Closed;
                ctr.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ctr_Closed(object sender, EventArgs e)
        {
            this.Parent.DialogResult = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidationDevis(laDetailDemande, false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void ValidationDevis(CsDemande laDetailDemande, bool IsTransmetre)
        {

            try
            {

                if (laDetailDemande.Branchement == null)
                {
                    laDetailDemande.Branchement = new CsBrt();
                    laDetailDemande.Branchement.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                    laDetailDemande.Branchement.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                    laDetailDemande.Branchement.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                    laDetailDemande.Branchement.NUMDEM = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                    laDetailDemande.Branchement.PRODUIT = string.IsNullOrEmpty(laDetailDemande.LaDemande.PRODUIT) ? string.Empty : laDetailDemande.LaDemande.PRODUIT;
                    laDetailDemande.Branchement.FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT == null ? 0 : laDetailDemande.LaDemande.FK_IDPRODUIT.Value;
                  
                    laDetailDemande.Branchement.DATECREATION = DateTime.Now;
                    laDetailDemande.Branchement.USERCREATION = UserConnecte.matricule;
                }
                laDetailDemande.LaDemande.ISEXTENSION = Chk_BranchementAvecExtension.IsChecked.Value;
                laDetailDemande.LaDemande.REGLAGECOMPTEUR = (Cbo_diametre.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsReglageCompteur).CODE;
                laDetailDemande.LaDemande.FK_IDREGLAGECOMPTEUR  = (Cbo_diametre.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsReglageCompteur).PK_ID ;
                laDetailDemande.LaDemande.PUISSANCESOUSCRITE = (Cbo_Puissance.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsPuissance).VALEUR;
                laDetailDemande.LaDemande.FK_IDPUISSANCESOUSCRITE = (Cbo_Puissance.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsPuissance).FK_IDPUISSANCE;
                laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                laDetailDemande.ObjetScanne.AddRange(ObjetScanne);

                laDetailDemande.LstFraixParticipation = new List<CsFraixParticipation>();
                laDetailDemande.LstFraixParticipation.AddRange(LstFraixParticipation);

                if (!string.IsNullOrEmpty(this.Txt_Distance_Extension.Text))
                    this.laDetailDemande.Branchement.LONGEXTENSION = decimal.Parse(this.Txt_Distance_Extension.Text);

                if (!string.IsNullOrEmpty(this.Txt_Distance.Text))
                    this.laDetailDemande.Branchement.LONGBRT = decimal.Parse(this.Txt_Distance.Text);

                if (!string.IsNullOrEmpty(this.Txt_BranchementProche.Text))
                    this.laDetailDemande.Branchement.NBPOINT = (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)? 6 : 1 ;

                if (!string.IsNullOrEmpty(this.Txt_PosteTransformateur.Text))
                    this.laDetailDemande.Branchement.CODEPOSTE = this.Txt_PosteTransformateur.Text;

                if (!string.IsNullOrEmpty(this.Txt_AdresseElectrique.Text))
                    this.laDetailDemande.Branchement.ADRESSERESEAU = this.Txt_AdresseElectrique.Text;

                if (Cbo_Zone.SelectedItem != null)
                {
                    this.laDetailDemande.Ag.TOURNEE = (Cbo_Zone.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsTournee).CODE;
                    this.laDetailDemande.Ag.FK_IDTOURNEE = (Cbo_Zone.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsTournee).PK_ID;
                }
                this.laDetailDemande.Ag.ORDTOUR = string.IsNullOrEmpty(this.TxtOrdreTournee.Text) ? string.Empty : this.TxtOrdreTournee.Text;

                this.laDetailDemande.Branchement.LATITUDE = TxtLatitude.Text;
                this.laDetailDemande.Branchement.LONGITUDE = TxtLongitude.Text;

                this.laDetailDemande.Branchement.ADRESSERESEAU = string.IsNullOrEmpty(this.Txt_AdresseElectrique.Text) ? null : this.Txt_AdresseElectrique.Text;
                this.laDetailDemande.Branchement.LONGITUDE = string.IsNullOrEmpty(this.TxtLongitude.Text) ? null : this.TxtLongitude.Text;
                this.laDetailDemande.Branchement.LATITUDE = string.IsNullOrEmpty(this.TxtLatitude.Text) ? null : this.TxtLatitude.Text;

                this.laDetailDemande.Branchement.FK_IDTYPEBRANCHEMENT = this.Txt_TypeBrancehment.Tag == null ? laDetailDemande.Branchement.FK_IDTYPEBRANCHEMENT : int.Parse(this.Txt_TypeBrancehment.Tag.ToString());
                this.laDetailDemande.Branchement.CODEBRT = string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text) ? null : this.Txt_TypeBrancehment.Text;

                this.laDetailDemande.Branchement.FK_IDPOSTESOURCE = this.Txt_PosteSource.Tag == null ? laDetailDemande.Branchement.FK_IDPOSTESOURCE : (int)this.Txt_PosteSource.Tag;
                this.laDetailDemande.Branchement.FK_IDPOSTETRANSFORMATION = this.Txt_PosteTransformateur.Tag == null ? laDetailDemande.Branchement.FK_IDPOSTETRANSFORMATION : (int)this.Txt_PosteTransformateur.Tag;
                this.laDetailDemande.Branchement.FK_IDDEPARTBT = this.Txt_DepartBt.Tag == null ? laDetailDemande.Branchement.FK_IDDEPARTBT : (int)this.Txt_DepartBt.Tag;
                this.laDetailDemande.Branchement.FK_IDQUARTIER = this.Txt_QuarteirPoste.Tag == null ? laDetailDemande.Branchement.FK_IDQUARTIER : (int)this.Txt_QuarteirPoste.Tag;
                this.laDetailDemande.Branchement.FK_IDDEPARTHTA = this.Txt_DepartHTA.Tag == null ? laDetailDemande.Branchement.FK_IDDEPARTHTA : (int)this.Txt_DepartHTA.Tag;
                this.laDetailDemande.Branchement.NEOUDFINAL = string.IsNullOrEmpty(this.Txt_NeoudFinal.Text) ? null : this.Txt_NeoudFinal.Text;
              
                #region Doc Scanne

                //if (laDetailDemande.ObjetScanne == null) 
                laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                laDetailDemande.ObjetScanne.AddRange(LstPiece);

                #endregion

                #region changement de compteur
                this.laDetailDemande.LaDemande.ISCHANGECOMPTEUR = Chk_ChangementDeCompteur.IsChecked.Value;
                #endregion
                if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur)
                {
                    List<CsDemandeBase> codes = new List<CsDemandeBase>();
                    codes.Add(laDetailDemande.LaDemande );
                    Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this.Parent);
                }
                else 
                OuvrireEtablissementDevis();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObjDOCUMENTSCANNE SaveFile(byte[] pStream, int pTypeDocument, ObjDOCUMENTSCANNE pDocumentScane)
        {
            try
            {
                //Récupération du contenu.
                if (pDocumentScane == null)
                {
                    pDocumentScane = new ObjDOCUMENTSCANNE { CONTENU = pStream, PK_ID = Guid.NewGuid() };
                    pDocumentScane.OriginalPK_ID = pDocumentScane.PK_ID;
                    pDocumentScane.ISNEW = true;
                    if (pTypeDocument == (int)SessionObject.TypeDocumentScanneDevis.Lettre)
                    {
                        pDocumentScane.NOMDOCUMENT = "Letter";
                        pDocumentScane.IsAutorisation = true;
                        if (laDetailDemande != null && laDetailDemande.ObjetScanne != null)
                        {
                            pDocumentScane.ISUPDATE = true;
                            pDocumentScane.DATEMODIFICATION = DateTime.Now;
                            pDocumentScane.USERMODIFICATION = UserConnecte.matricule;
                        }
                    }
                    else
                    {
                        pDocumentScane.NOMDOCUMENT = "Preuve";
                        pDocumentScane.IsAutorisation = false;
                        if (laDetailDemande != null && laDetailDemande.ObjetScanne != null)
                        {
                            pDocumentScane.ISUPDATE = true;
                            pDocumentScane.DATEMODIFICATION = DateTime.Now;
                            pDocumentScane.USERMODIFICATION = UserConnecte.matricule;
                        }
                    }
                }
                else
                    pDocumentScane.CONTENU = pStream;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }

            return pDocumentScane;
        }
        private void ActiverEnregistrer()
        {
            try
            {
                Btn_Transmettre.IsEnabled = ((this.Txt_Distance.Text != string.Empty) && (ObjetScanne != null) && (this.Cbo_Zone.SelectedItem != null));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Txt_Distance_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_Distance.Text))
                {
                    if (!DataReferenceManager.IsDecimal(Txt_Distance.Text))
                    {
                        (sender as TextBox).Background = new SolidColorBrush(Colors.Red);
                        ToolTipService.SetToolTip((sender as TextBox), Languages.msgVerificationNombre);
                        Txt_Distance.Text = string.Empty;
                    }
                    else
                    {
                        (sender as TextBox).Background = new SolidColorBrush(Colors.White);
                    }
                }
                ActiverEnregistrer();
            }
            catch (Exception ex)
            {
                if ((e.OriginalSource as TextBox) != sender)
                    Message.Show(ex.Message, Languages.txtDevis);
                else
                    Message.Show(ex.Message, Languages.msgVerificationNombre);
            }
        }

        private void Txt_Distance_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SessionObject.Enumere.IsDistanceSupplementaireFacture)
            {
                CultureInfo culture;
                decimal dis = 0;
                //bool ShowMessage = false;
                try
                {


                    if (!string.IsNullOrEmpty(Txt_Distance.Text) && (this.Parent.DialogResult != false))
                    {
                        culture = CultureInfo.CurrentCulture;
                        bool DistanceIsDecimal = decimal.TryParse(this.Txt_Distance.Text.Trim(), System.Globalization.NumberStyles.AllowDecimalPoint, culture, out dis);
                        if (!DistanceIsDecimal)
                        {
                            this.Txt_Distance.Focus();
                            this.Txt_Distance.SelectAll();
                            this.Txt_Distance.Focus();
                            return;
                        }

                        decimal additional = dis - this.seuilDistance;
                        if (dis == 0)
                        {
                            if (this.distanceMaxiSubvention > 0 && dis > this.distanceMaxiSubvention)
                            {
                                var mBoxControl = new MessageBoxControl.MessageBoxChildWindow(this.Parent.Title.ToString(), string.Format("La distance maximale autorisée pour un branchement eau subventionné est de {0} mètres.", this.distanceMaxiSubvention), MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                mBoxControl.OnMessageBoxClosed += (_, result) =>
                                {
                                    if (mBoxControl.Result == MessageBoxResult.OK)
                                    {
                                        this.Txt_Distance.Focus();
                                        this.Txt_Distance.SelectAll();
                                        return;
                                    }
                                };
                                mBoxControl.Show();
                            }
                        }
                        else
                        {
                            if ((laDetailDemande.LaDemande.FK_IDTYPEDEMANDE != (int)Galatee.Silverlight.DataReferenceManager.TypeDevis.SeparationEau) && (this.laDetailDemande.LaDemande.FK_IDTYPEDEMANDE != (int)Galatee.Silverlight.DataReferenceManager.TypeDevis.SeparationElectricity))
                            {
                                if (dis > this.distanceMaxi && laDetailDemande.LaDemande.PRODUIT == DataReferenceManager.Eau)
                                {
                                    var mBoxControl = new MessageBoxControl.MessageBoxChildWindow(this.Parent.Title.ToString(), string.Format("La distance maximale autorisée pour un branchement eau est de {0} mètres.", this.distanceMaxi), MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    mBoxControl.OnMessageBoxClosed += (_, result) =>
                                    {
                                        if (mBoxControl.Result == MessageBoxResult.OK)
                                        {
                                            this.Txt_Distance.Focus();
                                            this.Txt_Distance.SelectAll();
                                            return;
                                        }
                                    };
                                    mBoxControl.Show();
                                }
                                else if ((dis > this.seuilDistance) && (dis <= this.distanceMaxi))
                                {
                                    var Message = string.Empty;
                                    if ((bool)this.Chk_BranchementAvecExtension.IsChecked)
                                        Message = string.Format("La distance saisie est au dessus des {0} mètres. \nUne distance additionnelle de {1} mètre(s) \net les frais d'extension seront facturés.", this.seuilDistance, additional);
                                    else
                                        Message = string.Format("La distance saisie est au dessus des {0} mètres. \nUne distance additionnelle de {1} mètre(s) sera facturée.", this.seuilDistance, additional);
                                    var mBoxControl = new MessageBoxControl.MessageBoxChildWindow(this.Parent.Title.ToString(), Message, MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Question);
                                    mBoxControl.OnMessageBoxClosed += (_, result) =>
                                    {
                                        if (mBoxControl.Result == MessageBoxResult.OK)
                                        {
                                            Txt_BranchementProche.Focus();
                                            this.Txt_BranchementProche.SelectAll();
                                            return;
                                        }
                                        else
                                        {
                                            this.Txt_Distance.Focus();
                                            this.Txt_Distance.SelectAll();
                                            return;
                                        }
                                    };
                                    mBoxControl.Show();
                                }
                                //else
                                //{
                                //    Message.Show("La distance maximal à été dépassé,veuillez choisir une distance inférieur à " + this.distanceMaxi, "Information");
                                //    Txt_BranchementProche.Focus();
                                //    this.Txt_BranchementProche.SelectAll();
                                //    return;
                                //}
                            }
                            else if (dis > this.seuilDistance)
                            {
                                var mBoxControl = new MessageBoxControl.MessageBoxChildWindow(this.Parent.Title.ToString(), "La distance saisie est trop élevée pour une séparation de compteur.", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                mBoxControl.OnMessageBoxClosed += (_, result) =>
                                {
                                    if (mBoxControl.Result == MessageBoxResult.OK)
                                    {
                                        Txt_BranchementProche.Focus();
                                        this.Txt_BranchementProche.SelectAll();
                                        return;
                                    }
                                    else
                                    {
                                        this.Txt_Distance.Focus();
                                        this.Txt_Distance.SelectAll();
                                        return;
                                    }
                                };
                                mBoxControl.Show();
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    var message = string.Empty;
                    var titre = string.Empty;
                    if ((e.OriginalSource as TextBox) != sender)
                    {
                        message = ex.Message;
                        titre = this.Parent.Title.ToString();
                    }
                    else
                    {
                        message = ex.Message;
                        titre = Languages.msgVerificationNombre;
                    }
                    var mBoxControl = new MessageBoxControl.MessageBoxChildWindow(titre, message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mBoxControl.OnMessageBoxClosed += (_, result) =>
                    {
                        if (mBoxControl.Result == MessageBoxResult.OK)
                        {
                            this.Txt_Distance.Focus();
                            this.Txt_Distance.SelectAll();
                            return;
                        }
                    };
                    mBoxControl.Show();
                }
            }
        }



        private void Chk_BranchementSimplifie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (Convert.ToBoolean(this.Chk_BranchementSimplifie.IsChecked))
                //    this.Chk_BranchementComplete.IsChecked = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Chk_BranchementComplete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (Convert.ToBoolean(this.Chk_BranchementComplete.IsChecked))
                //    this.Chk_BranchementSimplifie.IsChecked = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Btn_Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Parent.DialogResult = false;
        }
        private void InitDonnee()
        {
            try
            {
                //if (this.laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance 
                //    || this.laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance 
                //    || this.laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur)
                //{
                //    Shared.ClasseMEthodeGenerique.FormStatLoopVisualTree(LayoutRoot, false);
                //    Chk_ChangementDeCompteur.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    Shared.ClasseMEthodeGenerique.FormStatLoopVisualTree(LayoutRoot, true);
                //    Chk_ChangementDeCompteur.Visibility = Visibility.Collapsed;
                //}

                ChargerDiametreBranchement(laDetailDemande);
                ChargerListeDesPostesSource();
                RetourneListeDesDepartHta();
                ChargerListeDesPostesTransformation();
                RetourneListeDesDepartBT();
                ChargeQuartier(laDetailDemande);
                RemplirTourneeExistante(laDetailDemande.LaDemande.FK_IDCENTRE);
                RemplirListeDesDiametresExistant(laDetailDemande);
                RenseignerInformationsDevis(laDetailDemande);
                string paramMaxi = string.Empty;
                string paramMaxiSubvention = string.Empty;
                string paramSeuil = string.Empty;
                if (laDetailDemande.LaDemande != null && SessionObject.Enumere.IsDistanceSupplementaireFacture )
                {
                    if ((laDetailDemande.LaDemande.PRODUIT == DataReferenceManager.Electricite) || (laDetailDemande.LaDemande.PRODUIT == DataReferenceManager.Prepaye))
                    {
                        if (SessionObject.distanceMaxiElectricite != 0 &&
                             SessionObject.seuilDistanceElectricite != 0)
                        {
                            this.distanceMaxi = SessionObject.distanceMaxiElectricite;
                            this.seuilDistance = SessionObject.seuilDistanceElectricite;
                            ActiverEnregistrer();
                            return;
                        }
                        paramMaxi = DataReferenceManager.ParametreDistanceMaxiElectricite;
                        paramSeuil = DataReferenceManager.ParametreSeuilDistanceAdditionnelleElectricite;
                    }

                    if (laDetailDemande.LaDemande.PRODUIT == DataReferenceManager.Eau)
                    {
                        if (SessionObject.distanceMaxiEau != 0 &&
                            SessionObject.seuilDistanceEau != 0 &&
                            SessionObject.distanceMaxiSubventionEau != 0)
                        {
                            this.distanceMaxi = SessionObject.distanceMaxiElectricite;
                            this.seuilDistance = SessionObject.seuilDistanceElectricite;
                            this.distanceMaxiSubvention = SessionObject.distanceMaxiSubventionEau;
                            ActiverEnregistrer();
                            return;
                        }
                        paramMaxi = DataReferenceManager.ParametreDistanceMaxiEau;
                        paramSeuil = DataReferenceManager.ParametreSeuilDistanceAdditionnelleEau;
                        paramMaxiSubvention = DataReferenceManager.ParametreDistanceMaxiEauSubvention;
                    }
                }
                if (SessionObject.Enumere.IsDistanceSupplementaireFacture)
                {
                    AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    clientDevis.GetParametresDistanceCompleted += (ssender, args) =>
                    {
                        try
                        {
                            if (args.Cancelled || args.Error != null)
                            {
                                string error = args.Error.Message;
                                Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                                return;
                            }
                            if (args.Result != null)
                            {
                                if (args.Result.Maxi == null)
                                    throw new Exception(string.Format("La distance maximale autorisée non parametrée : \nvaleur du paramètre '{0}' dans la table des paramètres généraux", paramMaxi));
                                if (args.Result.Seuil == null)
                                    throw new Exception(string.Format("Le seuil de la limite additionnelle non parametrée : \nvaleur du paramètre '{0}' dans la table des paramètres généraux", paramSeuil));

                                this.distanceMaxi = (decimal)args.Result.Maxi;
                                this.seuilDistance = (decimal)args.Result.Seuil;



                                if (args.Result.MaxiSubvention != null)
                                    this.distanceMaxiSubvention = (decimal)args.Result.MaxiSubvention;
                            }
                        }
                        catch (Exception ex)
                        {

                            Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                        }
                        ActiverEnregistrer();

                        //if (this.laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance || this.laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance || this.laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur)
                        //{
                        //    Shared.ClasseMEthodeGenerique.FormStatLoopVisualTree(this, false);
                        //    Chk_ChangementDeCompteur.Visibility = Visibility.Visible;
                        //}
                        //else
                        //{
                        //    Shared.ClasseMEthodeGenerique.FormStatLoopVisualTree(this, true);
                        //    Chk_ChangementDeCompteur.Visibility = Visibility.Collapsed;
                        //}
                    };
                    clientDevis.GetParametresDistanceAsync(paramMaxi, paramSeuil, paramMaxiSubvention);

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ChargeDetailDemande();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        //private void LayoutRoot_BindingValidationError(object sender, ValidationErrorEventArgs e)
        //{
        //    if (e.Action == ValidationErrorEventAction.Added)
        //    {
        //        //ToolTipService.SetToolTip((e.OriginalSource as TextBox), Languages.msgVerificationNombre);
        //        (e.OriginalSource as TextBox).Background = new SolidColorBrush(Colors.Yellow);
        //        ToolTipService.SetToolTip((e.OriginalSource as TextBox), e.Error.Exception.Message);
        //    }
        //    if (e.Action == ValidationErrorEventAction.Removed)
        //    {
        //        (e.OriginalSource as TextBox).Background = new SolidColorBrush(Colors.White);
        //        ToolTipService.SetToolTip((e.OriginalSource as TextBox), null);
        //    }
        //}

        private void Btn_Transmettre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!VerifieChampObligation()) return;
                ValidationDevis(laDetailDemande, true);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private bool VerifieChampObligation()
        {
            try
            {
                    bool ReturnValue = true;

                    if (string.IsNullOrEmpty(this.Txt_Distance.Text))
                        throw new Exception("Saisir la distance du branchement");

                    if (Cbo_diametre.SelectedItem ==null )
                        throw new Exception("Selectionnez le reglage compteur");

                    if (Cbo_Zone.SelectedItem ==null )
                        throw new Exception("Selectionnez la tournée");

                    if (string.IsNullOrEmpty(this.TxtOrdreTournee.Text))
                        throw new Exception("Saisir l'ordre sur la tourne ");

                    if (string.IsNullOrEmpty(Txt_TypeBrancehment.Text))
                        throw new Exception("Saisir le type du branchement");

                    if (ObjetScanne.Count == 0)
                        throw new Exception("Entrer le schéma");
                    else if(ObjetScanne.FirstOrDefault(t=>LstTypeDocument.Where(o=>o.CODE == SessionObject.Enumere.Schema).Select(i=>i.PK_ID).Contains(t.FK_IDTYPEDOCUMENT.Value)) ==null )
                        throw new Exception("Entrer le schéma");

                return ReturnValue;

            }
            catch (Exception ex)
            {
                this.Btn_Transmettre.IsEnabled = true;
                Message.ShowInformation(ex.Message, "Accueil");
                return false;
            }

        }
        

        private void Cbo_Zone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrer();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_Puissance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ActiverEnregistrer();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        //private void ChkFraisDeParticipation_Checked(object sender, RoutedEventArgs e)
        //{

        //}

        private void ChkFraisDeParticipation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(ChkFraisDeParticipation.IsChecked))
                    TxtFraisDeParticipation.Visibility = System.Windows.Visibility.Visible;
                else
                    TxtFraisDeParticipation.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_diametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Galatee.Silverlight.ServiceAccueil.CsReglageCompteur leReglage = (Galatee.Silverlight.ServiceAccueil.CsReglageCompteur )Cbo_diametre.SelectedItem;
            RemplirPuissanceSouscrite(leReglage);
            ActiverEnregistrer();
        }

        private void Txt_CodeMateriel_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ChkFraisDeParticipation_Checked(object sender, RoutedEventArgs e)
        {
            bool stat = true;
            AfficherInfoFraixParticipation(stat);
        }

        private void AfficherInfoFraixParticipation(bool stat)
        {
            lbl_refclient.IsEnabled = stat;
            txt_refClient.IsEnabled = stat;
            lbl_montantfraix.IsEnabled = stat;
            txt_montant.IsEnabled = stat;
            chk_exonere.IsEnabled = stat;
            btn_ajouter.IsEnabled = stat;
            btn_supprimer.IsEnabled = stat;
            dgListeFraixParicipation.IsEnabled = stat;
        }

        void form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //CalculerMonantFraix();
        }

        private void CalculerMonantFraix()
        {
            laDetailDemande.LstFraixParticipation = new List<CsFraixParticipation>();
            laDetailDemande.LstFraixParticipation = this.LstFraixParticipation.ToList();
            decimal? montantFraix = 0;
            foreach (var item in laDetailDemande.LstFraixParticipation)
            {
                if (item.ESTEXONERE!= false )
                    montantFraix = montantFraix + item.MONTANT;
            }
            TxtFraisDeParticipation.Text = montantFraix.ToString();
        }
        private UcImageScanne formScanne = null;
        public List<ObjDOCUMENTSCANNE> LstPiece = new List<ObjDOCUMENTSCANNE>();
        private byte[] image, imageFraix;
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
            ObjetScanne = this.LstPiece.ToList();
            //if (LstPiece.Count() > 0)
            //{
            //    this.isPreuveSelectionnee = true;
            //    EnabledDevisInformations(true);
            //}
            //else
            //{
            //    this.isPreuveSelectionnee = false;
            //    EnabledDevisInformations(false);
            //}
            //ActiverEnregistrerOuTransmettre();
        }

        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
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
                    this.dgListePiece.ItemsSource = null;
                    this.dgListePiece.ItemsSource = this.LstPiece;
                    ObjetScanne = this.LstPiece.ToList();

                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }

        private void btn_ajouter_Click_1(object sender, RoutedEventArgs e)
        {
            if (LstFraixParticipation != null && LstFraixParticipation.Count != 0 && LstFraixParticipation.FirstOrDefault(t => t.REF_CLIENT == this.txt_refClient.Text) != null)
            {
                Message.ShowInformation("Ce client est déja saisi ", "Participation");
                return;
            }
            bool IsExonneration = chk_exonere.IsChecked == true ? true : false;
            CsClient leClient = new CsClient();
            leClient.FK_IDCENTRE = laDemandeSelect.FK_IDCENTRE;
            leClient.CENTRE = laDemandeSelect.CENTRE;
            leClient.REFCLIENT = this.txt_refClient.Text;
            leClient.PRODUIT = laDemandeSelect.PRODUIT;
            RetourneOrdre(leClient, IsExonneration);
        }


        private void GetInformationFromChildWindowImagePreuveFraixP(object sender, EventArgs e)
        {
            int montant = 0;
            if (int.TryParse(txt_montant.Text, out montant))
            {
                this.LstFraixParticipation.Add(new CsFraixParticipation {FK_IDCLIENT = laDemandeSelect.FK_IDCENTRE , CENTRE = laDemandeSelect.CENTRE , REF_CLIENT = this.txt_refClient.Text, ORDRE = laDemandeSelect.ORDRE , MONTANT = montant, ESTEXONERE = chk_exonere.IsChecked.Value, PREUVE = imageFraix });
                this.dgListeFraixParicipation.ItemsSource = null ;
                this.dgListeFraixParicipation.ItemsSource = LstFraixParticipation;

                this.TxtFraisDeParticipation.Text = LstFraixParticipation.Sum(t => t.MONTANT).Value.ToString(SessionObject.FormatMontant);
                this.txt_refClient.Text = string.Empty;
                this.Txt_Ordre.Text = string.Empty;
                this.txt_montant.Text = string.Empty;
                this.chk_exonere.IsChecked = false;
                CalculerMonantFraix();
            }
            else
            {
                Message.ShowError("Veuillez saisir une valeur numerique pour le montant des fraix de participation", "Erreur");
            }
        }

        private void btn_supprimer_Click_1(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Ête-vous sure de vouloire supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    CsFraixParticipation Fraix = (CsFraixParticipation)dgListeFraixParicipation.SelectedItem;
                    this.LstFraixParticipation.Remove(Fraix);
                    this.dgListeFraixParicipation.ItemsSource = null;
                    this.dgListeFraixParicipation.ItemsSource = this.LstFraixParticipation;
                    CalculerMonantFraix();
                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }

        private void ChkFraisDeParticipation_Unchecked(object sender, RoutedEventArgs e)
        {
            AfficherInfoFraixParticipation(false);
        }

        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        private void txt_refClient_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void txt_montant_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void hyperlinkButtonPropScannee___Click(object sender, RoutedEventArgs e)
        {
            if (((HyperlinkButton)sender).Tag != null)
            {
                MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
                var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                ucImageScanne.Show();
            }
            else
            {
                Message.ShowInformation("Aucune image associer à cette ligne de fraix", "Information");
            }

        }

        private void Chk_BranchementAvecExtension_Checked(object sender, RoutedEventArgs e)
        {
            if (Chk_BranchementAvecExtension.IsChecked.Value)
            {
                ChkFraisDeParticipation.IsChecked = true;
                this.Txt_Distance_Extension.Visibility = System.Windows.Visibility.Visible;
                this.labelDistanceExtension.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.Txt_Distance_Extension.Visibility = System.Windows.Visibility.Collapsed ;
                this.labelDistanceExtension.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void btn_PosteSource_Click(object sender, RoutedEventArgs e)
        {
            this.btn_PosteSource.IsEnabled = false;
            if (SessionObject.LsDesPosteElectriquesSource.Count != 0)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesPosteElectriquesSource);
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE","Liste des poste sources");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_PosteSource);
                ctr.Show();
            }
        }

        void galatee_OkClickedbtn_PosteSource(object sender, EventArgs e)
        {
            this.btn_PosteSource.IsEnabled = true;
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsPosteSource _LePoste = (Galatee.Silverlight.ServiceAccueil.CsPosteSource)ctrs.MyObject;
                if (_LePoste != null)
                {
                    this.Txt_PosteSource.Text = _LePoste.CODE;
                    this.Txt_LibellePosteSource.Text = _LePoste.LIBELLE;
                    this.Txt_PosteSource.Tag = _LePoste.PK_ID;
                }
            }
        }

        private void btn_depart_Click(object sender, RoutedEventArgs e)
        {
            this.btn_departHta.IsEnabled = false;
            if (SessionObject.LsDesDepartHTA.Count != 0 && this.Txt_PosteSource.Tag != null )
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesDepartHTA.Where(t=>t.FK_IDPOSTESOURCE == (int)this.Txt_PosteSource.Tag ).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des departs HTA");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_depart);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_depart(object sender, EventArgs e)
        {
            this.btn_departHta.IsEnabled = true;
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsDepart _LeDepart = (Galatee.Silverlight.ServiceAccueil.CsDepart)ctrs.MyObject;
                if (_LeDepart != null)
                {
                    this.Txt_DepartHTA.Text = _LeDepart.CODE;
                    this.Txt_LibelleDepartHTA.Text = _LeDepart.LIBELLE;
                    this.Txt_DepartHTA.Tag  = _LeDepart.PK_ID ;

                }
            }
        }

        private void btn_PosteTransformateur_Click_1(object sender, RoutedEventArgs e)
        {
            this.btn_PosteTransformateur.IsEnabled = true;
            if (SessionObject.LsDesPosteElectriquesTransformateur.Count != 0 && this.Txt_DepartHTA.Tag != null )
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesPosteElectriquesTransformateur.Where(t=>t.FK_IDDEPARTHTA == (int)this.Txt_DepartHTA.Tag).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des postes transformateur");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_PosteTransformateur);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_PosteTransformateur(object sender, EventArgs e)
        {
            this.btn_PosteSource.IsEnabled = true;
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsPosteSource _LsPoste = (Galatee.Silverlight.ServiceAccueil.CsPosteSource)ctrs.MyObject;
                if (_LsPoste != null)
                {
                    this.Txt_PosteTransformateur.Text = _LsPoste.CODE;
                    this.Txt_LibellePosteTransformateur.Text = _LsPoste.LIBELLE;
                    this.Txt_PosteTransformateur.Tag  = _LsPoste.PK_ID ;

                }
            }
        }

        private void btn_DepartBT_Click(object sender, RoutedEventArgs e)
        {
            this.btn_DepartBT.IsEnabled = false;
            if (SessionObject.LsDesDepartBT.Count != 0 && this.Txt_PosteTransformateur.Tag != null )
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesDepartBT.Where(t=>t.FK_IDPOSTETRANSFORMATION == (int)this.Txt_PosteTransformateur.Tag).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des depart bt");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_DepartBT);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_DepartBT(object sender, EventArgs e)
        {
            this.btn_DepartBT.IsEnabled = true;
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsDepart _LsDepart = (Galatee.Silverlight.ServiceAccueil.CsDepart)ctrs.MyObject;
                if (_LsDepart != null)
                {
                    this.Txt_DepartBt .Text = _LsDepart.CODE;
                    this.Txt_LibelleDepartBt.Text = _LsDepart.LIBELLE;
                    this.Txt_DepartBt.Tag  = _LsDepart.PK_ID ;
                }
            }
        }

        private void RejeterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Shared.ClasseMEthodeGenerique.RejeterDemande(this.laDetailDemande);
                this.Parent.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_Imprimer_Click(object sender, RoutedEventArgs e)
        {

            List<CsDemandeBase> leDemandeAEditer = new List<CsDemandeBase>();
            laDetailDemande.LaDemande.NOMCLIENT = laDetailDemande.LeClient.NOMABON;
            laDetailDemande.LaDemande.COMMUNE  = laDetailDemande.Ag.LIBELLECOMMUNE ;
            laDetailDemande.LaDemande.QUARTIER = laDetailDemande.Ag.LIBELLEQUARTIER;
            laDetailDemande.LaDemande.LIBELLETACHE = laDetailDemande.Ag.TELEPHONE;
            laDetailDemande.LaDemande.CODEELECTRIQUE = laDetailDemande.Ag.RUE;
            laDetailDemande.LaDemande.CTAXEG  = laDetailDemande.Ag.PORTE ;
            laDetailDemande.LaDemande.LIBELLETACHE = laDetailDemande.Ag.TELEPHONE;
            laDetailDemande.LaDemande.REFEM  = laDetailDemande.LaDemande.PUISSANCESOUSCRITE.ToString() ;
            leDemandeAEditer.Add(laDetailDemande.LaDemande);

            Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(leDemandeAEditer, null, SessionObject.CheminImpression, "FicheIntervention", "Accueil", true);

        }

        private void Chk_BranchementAvecExtension_Unchecked(object sender, RoutedEventArgs e)
        {
            this.labelDistanceExtension.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_Distance_Extension.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btn_MotifRejet_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Accueil.FrAnotation ctrl = new Accueil.FrAnotation(laDetailDemande.LstCommentaire.FirstOrDefault().COMMENTAIRE );
            ctrl.Show();
        }
    }
}


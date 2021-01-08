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

namespace Galatee.Silverlight.Devis
{
    public partial class UcMetrersMT : UserControl
    {
        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = new CsDemande();
  
        decimal distanceMaxiSubvention;
        decimal distanceMaxi;
        decimal seuilDistance;
        int? CommuneDuPoste = null;

        private List<ObjDOCUMENTSCANNE> ObjetScanne = new List<ObjDOCUMENTSCANNE>();
        private List<ObjDOCUMENTSCANNE> ObjetScanneFraix = new List<ObjDOCUMENTSCANNE>();
        List<Galatee.Silverlight.ServiceAccueil.CsTypeBranchement > LstTypeBrt;
        

        List<CsFraixParticipation> LstFraixParticipation = new List<CsFraixParticipation>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        ChildWindow Parent = new ChildWindow();
        public UcMetrersMT()
        {
            InitializeComponent();
            TxtNombreTransformateur.MaxLength = 1;
            ChargerTypeDocument();
        }


        public UcMetrersMT(CsDemande laDetailDemande, ChildWindow Parent)
        {
            try
            {
                if (LayoutRoot != null)
                    LayoutRoot.Cursor = Cursors.Wait;
                InitializeComponent();
                this.Parent = Parent;

                this.Txt_PosteSource.MaxLength = 3;
                this.Txt_DepartHTA.MaxLength = 3;
                this.Txt_QuarteirPoste.MaxLength = 4;
                this.Txt_PosteTransformateur.MaxLength = 4;
                this.Txt_DepartBt.MaxLength =2;
                this.Txt_NeoudFinal.MaxLength =4;
                this.TxtNombreTransformateur.MaxLength = 1;

                this.laDetailDemande = laDetailDemande;
                Chk_ChangementDeCompteur.Visibility = Visibility.Collapsed;

                this.Txt_Distance_Extension.Visibility   = System.Windows.Visibility.Collapsed;
                labelDistanceExtension.Visibility = System.Windows.Visibility.Collapsed;

                ChargerTypeDocument();
                ChargerPuissance();
                ChargerPuissanceSouscrtie();
            }
            catch (Exception ex)
            {
                if (LayoutRoot != null)
                    LayoutRoot.Cursor = Cursors.Arrow;
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

        private void ChargeDetailDEvis()
        {
            laDemandeSelect = laDetailDemande.LaDemande;
            LayoutRoot.Cursor = Cursors.Arrow;
            InitDonnee();
        }
       private void RemplirListeDesTypeComptageExistant(CsDemande laDemande)
        {

            try
            {
                if (SessionObject.LstTypeComptage != null && SessionObject.LstTypeComptage.Count != 0)
                {
                    List<Galatee.Silverlight.ServiceAccueil.CsTypeComptage> listeTypeDeComptageExistant = SessionObject.LstTypeComptage;
                    Cbo_TypeComptage.SelectedValuePath = "PK_ID";
                    Cbo_TypeComptage.DisplayMemberPath = "LIBELLE";
                    Cbo_TypeComptage.ItemsSource = listeTypeDeComptageExistant;

                    if (listeTypeDeComptageExistant != null && listeTypeDeComptageExistant.Count == 1)
                    {
                        Cbo_TypeComptage.SelectedItem = listeTypeDeComptageExistant.First();
                        Cbo_TypeComptage.Tag = listeTypeDeComptageExistant.First();
                    }
                    if (laDemande != null && laDemande.Branchement != null && laDemande.Branchement.FK_IDTYPECOMPTAGE != null && laDemande.Branchement.FK_IDTYPECOMPTAGE != 0)
                    {
                        Cbo_TypeComptage.SelectedItem = listeTypeDeComptageExistant.FirstOrDefault(p => p.PK_ID  == laDemande.Branchement.FK_IDTYPECOMPTAGE);
                        Cbo_TypeComptage.Tag = listeTypeDeComptageExistant.FirstOrDefault(p => p.PK_ID  == laDemande.Branchement.FK_IDTYPECOMPTAGE );
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeComptageCompleted   += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    SessionObject.LstTypeComptage = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsTypeComptage> listeTypeDeComptageExistant = SessionObject.LstTypeComptage;
                    Cbo_TypeComptage.SelectedValuePath = "PK_ID";
                    Cbo_TypeComptage.DisplayMemberPath = "LIBELLE";
                    Cbo_TypeComptage.ItemsSource = listeTypeDeComptageExistant;
                    if (listeTypeDeComptageExistant != null && listeTypeDeComptageExistant.Count == 1)
                    {
                        Cbo_TypeComptage.SelectedItem = listeTypeDeComptageExistant.First();
                        Cbo_TypeComptage.Tag = listeTypeDeComptageExistant.First();
                    }
                    if (laDemande != null && laDemande.Branchement != null && laDemande.Branchement.FK_IDTYPECOMPTAGE != null && laDemande.Branchement.FK_IDTYPECOMPTAGE != 0)
                    {
                        Cbo_TypeComptage.SelectedItem = listeTypeDeComptageExistant.FirstOrDefault(p => p.PK_ID == laDemande.Branchement.FK_IDTYPECOMPTAGE);
                        Cbo_TypeComptage.Tag = listeTypeDeComptageExistant.FirstOrDefault(p => p.PK_ID == laDemande.Branchement.FK_IDTYPECOMPTAGE);
                    }
                    return;
                };
                service.ChargerTypeComptageAsync();
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

                    if (lstTournee.Where(t => t.FK_IDCENTRE == pCentreId) != null && lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).ToList().Count == 1)
                    {
                        Cbo_Zone.SelectedItem = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).First();
                        Cbo_Zone.Tag = lstTournee.Where(t => t.FK_IDCENTRE == pCentreId).First();
                    }
                    if (laDetailDemande.Branchement != null && laDetailDemande.Ag.FK_IDTOURNEE != null && laDetailDemande.Ag.FK_IDTOURNEE != 0)
                    {
                        Cbo_Zone.SelectedItem = lstTournee.FirstOrDefault(p => p.PK_ID == laDetailDemande.Ag.FK_IDTOURNEE);
                        Cbo_Zone.Tag = lstTournee.FirstOrDefault(p => p.PK_ID == laDetailDemande.Ag.FK_IDTOURNEE);

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
                    if (laDetailDemande.Branchement != null && laDetailDemande.Ag.FK_IDTOURNEE != null && laDetailDemande.Ag.FK_IDTOURNEE != 0)
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
        private void ChargerPuissanceSouscrtie()
        {
            try
            {
                if (SessionObject.LstPuissance != null && SessionObject.LstPuissance.Count != 0)
                {

                    List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissance;
                    if (_lstPuissance != null && _lstPuissance.Count != 0)
                    {
                        List<ServiceAccueil.CsPuissance> lesPuissance = _lstPuissance.Where(t => t.PRODUIT == SessionObject.Enumere.ElectriciteMT).ToList();
                        Cbo_PuissanceSouscrite.SelectedValuePath = "FK_IDPUISSANCE";
                        Cbo_PuissanceSouscrite.DisplayMemberPath = "VALEUR";
                        Cbo_PuissanceSouscrite.ItemsSource = lesPuissance;
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_PuissanceSouscrite.SelectedItem = lesPuissance.First();
                            Cbo_PuissanceSouscrite.Tag = lesPuissance.First();
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
                        Cbo_PuissanceSouscrite.SelectedValuePath = "FK_IDPUISSANCE";
                        Cbo_PuissanceSouscrite.DisplayMemberPath = "VALEUR";
                        Cbo_PuissanceSouscrite.ItemsSource = lesPuissance;
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_PuissanceSouscrite.SelectedItem = lesPuissance.First();
                            Cbo_PuissanceSouscrite.Tag = lesPuissance.First();
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
                        if (laDetailDemande.Branchement != null && laDetailDemande.Branchement.PUISSANCEINSTALLEE != null && laDetailDemande.Branchement.PUISSANCEINSTALLEE != 0)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.FirstOrDefault(p => p.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE);
                            Cbo_Puissance.Tag = lesPuissance.FirstOrDefault(p => p.PK_ID == laDetailDemande.Branchement.PUISSANCEINSTALLEE);
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
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                        if (laDetailDemande.Branchement != null && laDetailDemande.Branchement.PUISSANCEINSTALLEE != null && laDetailDemande.Branchement.PUISSANCEINSTALLEE != 0)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.FirstOrDefault(p => p.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE);
                            Cbo_Puissance.Tag = lesPuissance.FirstOrDefault(p => p.PK_ID == laDetailDemande.Branchement.PUISSANCEINSTALLEE);
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
        private void RemplirPuissance(Galatee.Silverlight.ServiceAccueil.CsTypeComptage   leDiametre)
        {
            try
            {
                if (SessionObject.LstPuissanceInstalle != null && SessionObject.LstPuissanceInstalle.Count != 0)
                {

                    List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceInstalle;
                    if (_lstPuissance != null && _lstPuissance.Count != 0)
                    {
                        List<ServiceAccueil.CsPuissance> lesPuissance = _lstPuissance.Where(t => t.PRODUIT  ==SessionObject.Enumere.ElectriciteMT  ).ToList();
                        Cbo_Puissance.SelectedValuePath = "FK_IDPUISSANCE";
                        Cbo_Puissance.DisplayMemberPath = "VALEUR";
                        Cbo_Puissance.ItemsSource = lesPuissance;
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                        if (laDetailDemande.Branchement  != null && laDetailDemande.Branchement.PUISSANCEINSTALLEE  != null && laDetailDemande.Branchement.PUISSANCEINSTALLEE  != 0)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.FirstOrDefault(p => p.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE);
                            Cbo_Puissance.Tag = lesPuissance.FirstOrDefault(p => p.PK_ID == laDetailDemande.Branchement.PUISSANCEINSTALLEE);
                        }
                        return;
                    }
                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceInstalleCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissanceInstalle  = args.Result;
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
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                        if (laDetailDemande.Branchement != null && laDetailDemande.Branchement.PUISSANCEINSTALLEE != null && laDetailDemande.Branchement.PUISSANCEINSTALLEE != 0)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.FirstOrDefault(p => p.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE);
                            Cbo_Puissance.Tag = lesPuissance.FirstOrDefault(p => p.PK_ID == laDetailDemande.Branchement.PUISSANCEINSTALLEE);
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

        private void RemplirTypeComptage(Galatee.Silverlight.ServiceAccueil.CsPuissance  lePuissance)
        {
            try
            {
                if (SessionObject.LstTypeComptage != null && SessionObject.LstTypeComptage.Count != 0)
                {

                    List<Galatee.Silverlight.ServiceAccueil.CsTypeComptage> _lstTcompte = SessionObject.LstTypeComptage ;
                    if (_lstTcompte != null && _lstTcompte.Count != 0)
                    {
                        List<ServiceAccueil.CsTypeComptage> lesTypeCompt = _lstTcompte.Where(t => t.PUISSANCEINSTALLEE_MINI <= lePuissance.VALEUR && t.PUISSANCEINSTALLEE_MAXI>= lePuissance.VALEUR ).ToList();
                        Cbo_TypeComptage.DisplayMemberPath = "LIBELLE";
                        Cbo_TypeComptage.ItemsSource = lesTypeCompt;
                        if (lesTypeCompt != null && lesTypeCompt.Count == 1)
                        {
                            Cbo_TypeComptage.SelectedItem = lesTypeCompt.First();
                            Cbo_TypeComptage.Tag = lesTypeCompt.First();
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
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                        if (laDetailDemande.Branchement != null && laDetailDemande.Branchement.PUISSANCEINSTALLEE != null && laDetailDemande.Branchement.PUISSANCEINSTALLEE != 0)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.FirstOrDefault(p => p.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE);
                            Cbo_Puissance.Tag = lesPuissance.FirstOrDefault(p => p.PK_ID == laDetailDemande.Branchement.PUISSANCEINSTALLEE);
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

        private void ChargeTypeComptage(int? nbrtransfo, int puissanceSouscrit, int puissanceInstalle)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneTypeComptageCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    List<CsTypeComptage> lesType = args.Result;
                    if (lesType != null && lesType.Count != 0)
                    {
                        Cbo_TypeComptage.SelectedValuePath = "PK_ID";
                        Cbo_TypeComptage.DisplayMemberPath = "LIBELLE";
                        Cbo_TypeComptage.ItemsSource = lesType;


                        if (lesType.Count == 1)
                        {
                            Cbo_TypeComptage.SelectedItem = lesType.First();
                            Cbo_TypeComptage.Tag  = lesType.First();
                        }
                        return;
                    }
                };
                service.RetourneTypeComptageAsync(nbrtransfo, puissanceSouscrit, puissanceInstalle);
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
        private void RetourneListeDesDepartHta()
        {
            if (SessionObject.LsDesDepartHTA != null && SessionObject.LsDesDepartHTA.Count != 0)
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
        private void ChargerListeDesPostesTransformation()
        {
            if (SessionObject.LsDesPosteElectriquesTransformateur != null && SessionObject.LsDesPosteElectriquesTransformateur.Count != 0)
            {
                return;
            }
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
        //private void RetourneListeDesDepartBT()
        //{
        //    if (SessionObject.LsDesDepartBT.Count != 0)
        //    {
        //        return;
        //    }
        //    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //    service.ChargerDepartBTCompleted += (s, args) =>
        //    {
        //        if (args != null && args.Cancelled)
        //            return;
        //        SessionObject.LsDesDepartBT = args.Result;
        //    };
        //    service.ChargerDepartBTAsync();
        //    service.CloseAsync();
        //}
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
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeDiametre);
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
            //if (LstQuartierSite != null && LstQuartierSite.Count != 0)
            //{   
            if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
            {
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
            this.btn_QuartierPoste.IsEnabled = true;
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsQuartier _LeQuartier = (ServiceAccueil.CsQuartier)ctrs.MyObject;
                if (_LeQuartier != null)
                {
                    this.Txt_LibelleQuartier.Text = _LeQuartier.LIBELLE;
                    Txt_QuarteirPoste.Text = _LeQuartier.CODE;
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
            }
            else
            {
                Message.ShowError("Les informations de la demande ne sont pas renseigné ", Languages.txtDevis);
            }


            // Charger données du devis
            if (laDetailDemande.Branchement != null)
            {
                Txt_Distance.Text = laDetailDemande.Branchement.LONGBRT.ToString();

                if (laDetailDemande.Branchement != null && !string.IsNullOrEmpty(laDetailDemande.Branchement.DIAMBRT))
                {
                    foreach (Galatee.Silverlight.ServiceAccueil .CsReglageCompteur diametre in Cbo_TypeComptage.Items)
                    {
                        if (diametre.CODE == laDetailDemande.Branchement.DIAMBRT && diametre.CODEPRODUIT == laDetailDemande.Branchement.PRODUIT)
                        {
                            Cbo_TypeComptage.SelectedItem = diametre;
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
                if (laDetailDemande.LaDemande.PUISSANCESOUSCRITE != null && laDetailDemande.LaDemande.PUISSANCESOUSCRITE != 0)
                {
                    foreach (Galatee.Silverlight.ServiceAccueil.CsPuissance puissance in Cbo_PuissanceSouscrite .Items)
                    {
                        if (puissance.VALEUR == laDetailDemande.LaDemande.PUISSANCESOUSCRITE)
                        {
                            Cbo_PuissanceSouscrite.SelectedItem = puissance;
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
                TxtNombreTransformateur.Text = this.laDetailDemande.Branchement != null  ? this.laDetailDemande.Branchement.NOMBRETRANSFORMATEUR.ToString() : string.Empty;
            }




            #region DocumentScanne
            if (laDetailDemande.ObjetScanne != null && laDetailDemande.ObjetScanne.Count != 0)
            {
                //isPreuveSelectionnee = true;
                foreach (var item in laDetailDemande.ObjetScanne)
                {
                    LstPiece.Add(item);
                    ObjetScanne.Add(item);
                }
                dgListePiece.ItemsSource = ObjetScanne;

            }
            else
            {
                //isPreuveSelectionnee = false;
            }
            #endregion

            //else
            //{
            //    isPreuveSelectionnee = false;
            //}






            //if (this.laDetailDemande.ObjetScanne!= null && laDetailDemande.ObjetScanne.FirstOrDefault(t=>t.NOMDOCUMENT =="Schema")!= null )
            //{
            //    this.Lien_Schema.Content = "Schema scanné";
            //    this.Lien_Schema.Tag = laDetailDemande.ObjetScanne.FirstOrDefault(t => t.NOMDOCUMENT == "Schema").CONTENU;
            //    this.isFicheSelectionnee = true;
            //    doc = laDetailDemande.ObjetScanne.FirstOrDefault(t => t.NOMDOCUMENT == "Schema");
            //    ActiverEnregistrer();
            //}
            //else
            //{
            //    this.Lien_Schema.Content = "Insérer le schema scanné";
            //    this.Lien_Schema.Tag = null;
            //    this.isFicheSelectionnee = false;
            //}
            //if (this.laDetailDemande.ObjetScanne != null && laDetailDemande.ObjetScanne.FirstOrDefault(t => t.NOMDOCUMENT == "Manuscrit") != null)
            //{
            //    this.Lien_Manuscrit.Content = "Manuscrit scanné";
            //    this.Lien_Manuscrit.Tag = laDetailDemande.ObjetScanne.FirstOrDefault(t => t.NOMDOCUMENT == "Manuscrit").CONTENU;
            //    this.isManuscritSelectionne = true;
            //    manuscrit = laDetailDemande.ObjetScanne.FirstOrDefault(t => t.NOMDOCUMENT == "Manuscrit");
            //}
            //else
            //{
            //    this.Lien_Manuscrit.Content = "Insérer le manuscrit scanné";
            //    this.Lien_Manuscrit.Tag = null;
            //    this.isManuscritSelectionne = false;
            //}

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
            LayoutRoot.Cursor = Cursors.Arrow;
        }
        private void OuvrireEtablissementDevis()
        {
            try
            {
                UcEtablissementDevisMt ctr = new UcEtablissementDevisMt(laDetailDemande);
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
                this.Parent.DialogResult = false;
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
                laDetailDemande.LaDemande.PUISSANCESOUSCRITE = ((CsPuissance)this.Cbo_PuissanceSouscrite.SelectedItem).VALEUR;
                laDetailDemande.LaDemande.FK_IDPUISSANCESOUSCRITE  = ((CsPuissance)this.Cbo_PuissanceSouscrite.SelectedItem).PK_ID ;

                laDetailDemande.LaDemande.TYPECOMPTAGE = ((CsTypeComptage)this.Cbo_TypeComptage.SelectedItem).CODE;
                laDetailDemande.LaDemande.FK_IDTYPECOMPTAGE  = ((CsTypeComptage)this.Cbo_TypeComptage.SelectedItem).PK_ID ;
                laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                laDetailDemande.ObjetScanne.AddRange(ObjetScanne);

                laDetailDemande.LstFraixParticipation = new List<CsFraixParticipation>();
                laDetailDemande.LstFraixParticipation.AddRange(LstFraixParticipation);

                if (!string.IsNullOrEmpty(this.Txt_Distance.Text))
                    this.laDetailDemande.Branchement.LONGBRT = decimal.Parse(this.Txt_Distance.Text);

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
                if (Cbo_Puissance.SelectedItem != null && this.Chk_IsBornePoste.IsChecked == false )
                    this.laDetailDemande.Branchement.PUISSANCEINSTALLEE = Convert.ToDecimal((Cbo_Puissance.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsPuissance).VALEUR);
                if (this.Chk_IsBornePoste.IsChecked == true)
                    this.laDetailDemande.Branchement.PUISSANCEINSTALLEE = string.IsNullOrEmpty( this.TxtPuissanceInstalle.Text)? 0: Convert.ToDecimal( this.TxtPuissanceInstalle.Text);

                this.laDetailDemande.Branchement.LATITUDE = TxtLatitude.Text;
                this.laDetailDemande.Branchement.LONGITUDE = TxtLongitude.Text;

                this.laDetailDemande.Branchement.ADRESSERESEAU = string.IsNullOrEmpty(this.Txt_AdresseElectrique.Text) ? null : this.Txt_AdresseElectrique.Text;
                this.laDetailDemande.Branchement.LONGITUDE = string.IsNullOrEmpty(this.TxtLongitude.Text) ? null : this.TxtLongitude.Text;
                this.laDetailDemande.Branchement.LATITUDE = string.IsNullOrEmpty(this.TxtLatitude.Text) ? null : this.TxtLatitude.Text;

                this.laDetailDemande.Branchement.FK_IDTYPEBRANCHEMENT = this.Txt_TypeBrancehment.Tag == null ? laDetailDemande.Branchement.FK_IDTYPEBRANCHEMENT : int.Parse(this.Txt_TypeBrancehment.Tag.ToString());
                this.laDetailDemande.Branchement.CODEBRT = string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text) ? null : this.Txt_TypeBrancehment.Text;

                this.laDetailDemande.Branchement.FK_IDTYPECOMPTAGE  = this.Txt_TypeBrancehment.Tag == null ? laDetailDemande.Branchement.FK_IDTYPEBRANCHEMENT : int.Parse(this.Txt_TypeBrancehment.Tag.ToString());
                if (Cbo_TypeComptage.SelectedItem != null)
                    this.laDetailDemande.Branchement.FK_IDTYPECOMPTAGE = (Cbo_TypeComptage.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsTypeComptage ).PK_ID ;

                this.laDetailDemande.Branchement.NOMBRETRANSFORMATEUR = string.IsNullOrEmpty(this.TxtNombreTransformateur.Text) ? 0 : int.Parse(this.TxtNombreTransformateur.Text);
                this.laDetailDemande.Branchement.FK_IDPOSTESOURCE = this.Txt_PosteSource.Tag == null ? laDetailDemande.Branchement.FK_IDPOSTESOURCE : (int)this.Txt_PosteSource.Tag;
                this.laDetailDemande.Branchement.FK_IDPOSTETRANSFORMATION = this.Txt_PosteTransformateur.Tag == null ? laDetailDemande.Branchement.FK_IDPOSTETRANSFORMATION : (int)this.Txt_PosteTransformateur.Tag;
                this.laDetailDemande.Branchement.DEPARTBT = this.Txt_DepartBt.Tag == null ? laDetailDemande.Branchement.DEPARTBT : this.Txt_DepartBt.Tag.ToString();
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
                OuvrireEtablissementDevis();
                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.ValiderDemandeCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                };
                clientDevis.ValiderDemandeAsync(laDetailDemande);

            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
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
                                    //if ((bool)this.Chk_BranchementAvecExtension.IsChecked)
                                    //    Message = string.Format("La distance saisie est au dessus des {0} mètres. \nUne distance additionnelle de {1} mètre(s) \net les frais d'extension seront facturés.", this.seuilDistance, additional);
                                    //else
                                    //    Message = string.Format("La distance saisie est au dessus des {0} mètres. \nUne distance additionnelle de {1} mètre(s) sera facturée.", this.seuilDistance, additional);
                                    var mBoxControl = new MessageBoxControl.MessageBoxChildWindow(this.Parent.Title.ToString(), Message, MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Question);
                                    mBoxControl.OnMessageBoxClosed += (_, result) =>
                                    {
                                        if (mBoxControl.Result == MessageBoxResult.OK)
                                        {
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


                if (this.laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance || this.laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance || this.laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur)
                {
                    Shared.ClasseMEthodeGenerique.FormStatLoopVisualTree(LayoutRoot, false);
                    Chk_ChangementDeCompteur.Visibility = Visibility.Visible;
                }
                else
                {
                    Shared.ClasseMEthodeGenerique.FormStatLoopVisualTree(LayoutRoot, true);
                    Chk_ChangementDeCompteur.Visibility = Visibility.Collapsed;
                }

                ChargerDiametreBranchement(laDetailDemande);
                ChargerListeDesPostesSource();
                RetourneListeDesDepartHta();
                ChargerListeDesPostesTransformation();
                //RetourneListeDesDepartBT();
                ChargeQuartier(laDetailDemande);
                RemplirTourneeExistante(laDetailDemande.LaDemande.FK_IDCENTRE);
                RemplirListeDesTypeComptageExistant(laDetailDemande);
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
                                LayoutRoot.Cursor = Cursors.Arrow;

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
                                LayoutRoot.Cursor = Cursors.Arrow;
                            }
                        }
                        catch (Exception ex)
                        {

                            LayoutRoot.Cursor = Cursors.Arrow;
                            Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                        }
                        ActiverEnregistrer();

                        if (this.laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance || this.laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance || this.laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur)
                        {
                            Shared.ClasseMEthodeGenerique.FormStatLoopVisualTree(this, false);
                            Chk_ChangementDeCompteur.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            Shared.ClasseMEthodeGenerique.FormStatLoopVisualTree(this, true);
                            Chk_ChangementDeCompteur.Visibility = Visibility.Collapsed;
                        }
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
                ChargeDetailDEvis();
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void LayoutRoot_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                //ToolTipService.SetToolTip((e.OriginalSource as TextBox), Languages.msgVerificationNombre);
                (e.OriginalSource as TextBox).Background = new SolidColorBrush(Colors.Yellow);
                ToolTipService.SetToolTip((e.OriginalSource as TextBox), e.Error.Exception.Message);
            }
            if (e.Action == ValidationErrorEventAction.Removed)
            {
                (e.OriginalSource as TextBox).Background = new SolidColorBrush(Colors.White);
                ToolTipService.SetToolTip((e.OriginalSource as TextBox), null);
            }
        }

        private void Btn_Transmettre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidationDevis(laDetailDemande, true);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        //private void Txt_Distance_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        if ((((e.Key > Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back || e.Key == Key.Tab)) /*&& (DataReferenceManager.IsDecimal(Txt_Distance.Text))*/)
        //            e.Handled = false;
        //        else
        //        {
        //            e.Handled = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.Show(ex.Message, Languages.txtDevis);
        //    } 
        //}

        private void Cbo_Zone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //if (Cbo_Zone.SelectedItem != null)
                //{
                //    CsTournee tournee = (CsTournee)Cbo_Zone.SelectedItem;
                //    TxtTournee.Text = tournee != null ? tournee.CODE : string.Empty;
                //}
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

                Galatee.Silverlight.ServiceAccueil.CsPuissance leDiametre = (Galatee.Silverlight.ServiceAccueil.CsPuissance)Cbo_Puissance.SelectedItem;
                decimal valeur = leDiametre.VALEUR * (decimal )(0.8);
                List<CsPuissance> lesPuissance = SessionObject.LstPuissance.Where(t => t.PRODUIT == SessionObject.Enumere.ElectriciteMT && t.VALEUR <= valeur).ToList();
                Cbo_PuissanceSouscrite.SelectedValuePath = "FK_IDPUISSANCE";
                Cbo_PuissanceSouscrite.DisplayMemberPath = "VALEUR";
                Cbo_PuissanceSouscrite.ItemsSource = lesPuissance;
                if (lesPuissance != null && lesPuissance.Count == 1)
                {
                    Cbo_PuissanceSouscrite.SelectedItem = lesPuissance.First();
                    Cbo_PuissanceSouscrite.Tag = lesPuissance.First();
                };
                if (!string.IsNullOrEmpty(this.TxtNombreTransformateur.Text) &&
                    this.Cbo_PuissanceSouscrite.SelectedItem != null)
                { 
                int? nbrtransfo =int.Parse(this.TxtNombreTransformateur.Text);
                int puissansSouscrit =int.Parse(((CsPuissance)this.Cbo_PuissanceSouscrite.SelectedItem ).VALEUR.ToString());
                int puissansInstalle =int.Parse(((CsPuissance )this.Cbo_Puissance.SelectedItem ).VALEUR.ToString());
                ChargeTypeComptage(nbrtransfo, puissansSouscrit, puissansInstalle);
                }
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

        private void Cbo_TypeComptage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Galatee.Silverlight.ServiceAccueil.CsTypeComptage leDiametre = (Galatee.Silverlight.ServiceAccueil.CsTypeComptage)Cbo_TypeComptage.SelectedItem;
            //RemplirPuissance(leDiametre);
            ActiverEnregistrer();
        }

        private void Txt_CodeMateriel_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ChkFraisDeParticipation_Checked(object sender, RoutedEventArgs e)
        {
            ////UcFraixParticipation form = new UcFraixParticipation();
            ////form.CallBack += form_CallBack;
            ////form.Show();
            //UcFraixParticipation form = new UcFraixParticipation();
            //form.Closing += form_Closing;
            //form.Show();
            bool stat = true;
            AfficherInfoFraixParticipation(stat);
        }

        private void AfficherInfoFraixParticipation(bool stat)
        {
            //groupBox1_Fraix.IsEnabled = stat;
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
        private Shared.UcImageScanne formScanne = null;
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
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
                        /*18/02/2019 */
                        CheminInitialDeCopyDeFichier = SessionObject.CheminDocumentScanne;
                        NomDeCopyDeFichier = openDialog.Files.First().Name;

                        FileStream stream = openDialog.File.OpenRead();
                        var memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        image = memoryStream.GetBuffer();
                      

                        formScanne = new Shared.UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                        formScanne.Closed += new EventHandler(GetInformationFromChildWindowImagePreuve);
                        formScanne.Show();
                    }
                }
            }
        }

        private void GetInformationFromChildWindowImagePreuve(object sender, EventArgs e)
        {
            this.LstPiece.Add(new ObjDOCUMENTSCANNE
            {
                PK_ID = Guid.NewGuid(),
                NOMDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).LIBELLE,
                FK_IDTYPEDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).PK_ID,
                CONTENU = image,
                CODETYPEDOC = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).CODE,
                DATECREATION = DateTime.Now,
                DATEMODIFICATION = DateTime.Now,
                USERCREATION = UserConnecte.matricule,
                USERMODIFICATION = UserConnecte.matricule,
                ISNEW = true,
                NOMDUFICHIER = NomDeCopyDeFichier,
                CHEMININIT = CheminInitialDeCopyDeFichier + "\\" + NomDeCopyDeFichier 
            });
            this.dgListePiece.ItemsSource = this.LstPiece;
            ObjetScanne = this.LstPiece.ToList();
             
        }

        FileStream fs;
        private void OuvrirPieceJointe_Click(object sender, RoutedEventArgs e)
        {
            if (dgListePiece.SelectedItem != null)
            {
                ObjDOCUMENTSCANNE selectObj = (ObjDOCUMENTSCANNE)this.dgListePiece.SelectedItem;
                if (selectObj.CONTENU != null)
                {
                    MemoryStream memoryStream = new MemoryStream(selectObj.CONTENU);
                    var ucImageScanne = new Galatee.Silverlight.Shared.UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                    ucImageScanne.Show();
                }
                else if (selectObj.CONTENU == null && string.IsNullOrEmpty(selectObj.CHEMINCOPY))
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.DocumentScanneContenuCompleted += (s, args) =>
                    {
                        if ((args != null && args.Cancelled) || (args.Error != null))
                            return;

                        MemoryStream memoryStream = new MemoryStream(args.Result.CONTENU);
                        var ucImageScanne = new Galatee.Silverlight.Shared.UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                        ucImageScanne.Show();
                    };
                    service.DocumentScanneContenuAsync(selectObj);
                    service.CloseAsync();
                }
                else if (selectObj.CHEMINCOPY != null)
                {
                    string Chemin = selectObj.CHEMINCOPY;
                    FileStream fs;
                    fs = File.Open(Chemin, FileMode.Open, FileAccess.Read, FileShare.None);
                    var memoryStream = new MemoryStream();
                    fs.CopyTo(memoryStream);
                    fs.Dispose();

                    image = memoryStream.GetBuffer();
                    var ucImageScanne = new Galatee.Silverlight.Shared.UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                    ucImageScanne.Show();
                }
            }
        }


        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new Shared.UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
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
                    ObjetScanne = this.LstPiece.ToList();

                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }
        /*18/02/2019 */
        string CheminInitialDeCopyDeFichier = string.Empty;
        string NomDeCopyDeFichier = string.Empty;
        /**/
        private void btn_ajouter_Click_1(object sender, RoutedEventArgs e)
        {
            if (chk_exonere.IsChecked.Value)
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
                        formScanne = new Shared.UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                        formScanne.Closed += new EventHandler(GetInformationFromChildWindowImagePreuveFraixP);
                        formScanne.Show();
                    }
                }
            }
            else
            {
                GetInformationFromChildWindowImagePreuveFraixP(null, null);
            }
        }

        private void GetInformationFromChildWindowImagePreuveFraixP(object sender, EventArgs e)
        {
            int montant = 0;
            if (int.TryParse(txt_montant.Text, out montant))
            {
                List<CsFraixParticipation> FraisParticipation = new List<CsFraixParticipation>();
                this.LstFraixParticipation.Add(new CsFraixParticipation { REF_CLIENT = this.txt_refClient.Text, MONTANT = montant, ESTEXONERE = chk_exonere.IsChecked.Value, PREUVE = imageFraix });
                FraisParticipation = LstFraixParticipation;
                this.dgListeFraixParicipation.ItemsSource = FraisParticipation;
                CalculerMonantFraix();
            }
            else
            {
                Message.ShowError("Veuillez saisir une valeur numerique pour le montant des fraix de participation", "Erreur");
            }
        }

        private void btn_supprimer_Click_1(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    CsFraixParticipation Fraix = (CsFraixParticipation)dgListeFraixParicipation.SelectedItem;
                    this.LstFraixParticipation.Remove(Fraix);
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
                var ucImageScanne = new Shared.UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                ucImageScanne.Show();
            }
            else
            {
                Message.ShowInformation("Aucune image associer à cette ligne de fraix", "Information");
            }

        }


        private void btn_PosteSource_Click(object sender, RoutedEventArgs e)
        {
            this.btn_PosteSource.IsEnabled = false;
            if (SessionObject.LsDesPosteElectriquesSource.Count != 0)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesPosteElectriquesSource);
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE","Liste des postes sources");
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
                    this.CommuneDuPoste = _LePoste.FK_IDCOMMUNE;

                }
            }
        }

        private void btn_depart_Click(object sender, RoutedEventArgs e)
        {
            this.btn_departHta.IsEnabled = false;
            if (SessionObject.LsDesDepartHTA.Count != 0 && this.Txt_PosteSource.Tag != null )
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesDepartHTA.Where(t=>t.FK_IDPOSTESOURCE == (int)this.Txt_PosteSource.Tag ).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des départs HTA");
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
                Galatee.Silverlight.ServiceAccueil.CsPosteTransformation _LsPoste = (Galatee.Silverlight.ServiceAccueil.CsPosteTransformation)ctrs.MyObject;
                if (_LsPoste != null)
                {
                    this.Txt_PosteTransformateur.Text = _LsPoste.CODE;
                    this.Txt_LibellePosteTransformateur.Text = _LsPoste.LIBELLE;
                    this.Txt_PosteTransformateur.Tag  = _LsPoste.PK_ID ;

                }
            }
        }


        private void RejeterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Parent.Close();
                Shared.ClasseMEthodeGenerique.RejeterDemande(this.laDetailDemande);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Cbo_PuissanceSouscrite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TxtNombreTransformateur.Text) &&
                 this.Cbo_Puissance.SelectedItem != null && this.Chk_IsBornePoste.IsChecked == false  )
            {
                int nbrtransfo = int.Parse(this.TxtNombreTransformateur.Text);
                int puissansSouscrit = int.Parse(((CsPuissance)this.Cbo_PuissanceSouscrite.SelectedItem).VALEUR.ToString());
                int puissansInstalle = int.Parse(((CsPuissance)this.Cbo_Puissance.SelectedItem).VALEUR.ToString());
                ChargeTypeComptage(nbrtransfo, puissansSouscrit, puissansInstalle);
            }
            if (!string.IsNullOrEmpty(this.TxtNombreTransformateur.Text) &&
                 !string.IsNullOrEmpty(this.TxtPuissanceInstalle.Text) &&
                 this.Chk_IsBornePoste.IsChecked == true)
            {
                int nbrtransfo = int.Parse(this.TxtNombreTransformateur.Text);
                int puissansSouscrit = int.Parse(((CsPuissance)this.Cbo_PuissanceSouscrite.SelectedItem).VALEUR.ToString());
                int puissansInstalle = int.Parse((puissansSouscrit / (decimal)(0.8)).ToString());

                this.Cbo_Puissance.Visibility = System.Windows.Visibility.Collapsed;
                this.TxtPuissanceInstalle.Visibility = System.Windows.Visibility.Visible;
                TxtPuissanceInstalle.Text = (((CsPuissance)this.Cbo_PuissanceSouscrite.SelectedItem).VALEUR / (decimal)(0.8)).ToString(SessionObject.FormatMontant);
                TxtPuissanceInstalle.IsReadOnly = true;
                ChargeTypeComptage(nbrtransfo, puissansSouscrit, puissansInstalle);
            }
        }

        private void TxtNombreTransformateur_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Cbo_PuissanceSouscrite.SelectedItem != null  &&
              this.Cbo_Puissance.SelectedItem != null && !string.IsNullOrEmpty(this.TxtNombreTransformateur.Text))
            {
                int nbrtransfo = int.Parse(this.TxtNombreTransformateur.Text);
                int puissansSouscrit = int.Parse(((CsPuissance)this.Cbo_PuissanceSouscrite.SelectedItem).VALEUR.ToString());
                int puissansInstalle = int.Parse(((CsPuissance)this.Cbo_Puissance.SelectedItem).VALEUR.ToString());
                ChargeTypeComptage(nbrtransfo, puissansSouscrit, puissansInstalle);
            }
            if (!string.IsNullOrEmpty(this.TxtNombreTransformateur.Text) &&
                !string.IsNullOrEmpty(this.TxtPuissanceInstalle.Text) &&
                this.Cbo_PuissanceSouscrite.SelectedItem != null &&
                 this.Chk_IsBornePoste.IsChecked == true)
            {
                int nbrtransfo = int.Parse(this.TxtNombreTransformateur.Text);
                int puissansSouscrit = int.Parse(((CsPuissance)this.Cbo_PuissanceSouscrite.SelectedItem).VALEUR.ToString());
                int puissansInstalle = int.Parse((puissansSouscrit / (decimal)(0.8)).ToString());

                this.Cbo_Puissance.Visibility = System.Windows.Visibility.Collapsed;
                this.TxtPuissanceInstalle.Visibility = System.Windows.Visibility.Visible;
                TxtPuissanceInstalle.Text = (((CsPuissance)this.Cbo_PuissanceSouscrite.SelectedItem).VALEUR / (decimal)(0.8)).ToString(SessionObject.FormatMontant);
                TxtPuissanceInstalle.IsReadOnly = true;
                ChargeTypeComptage(nbrtransfo, puissansSouscrit, puissansInstalle);
            }

        }

        private void Chk_ChangementDeCompteur_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Chk_IsBornePoste_Checked(object sender, RoutedEventArgs e)
        {
            if (this.Cbo_PuissanceSouscrite.SelectedItem != null)
            {
                this.Cbo_Puissance.Visibility = System.Windows.Visibility.Collapsed;
                this.TxtPuissanceInstalle.Visibility = System.Windows.Visibility.Visible ;
                TxtPuissanceInstalle.Text = (((CsPuissance)this.Cbo_PuissanceSouscrite.SelectedItem).VALEUR / (decimal)(0.8)).ToString(SessionObject.FormatMontant);
                TxtPuissanceInstalle.IsReadOnly = true;
            }
        }

        private void Chk_IsBornePoste_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Cbo_Puissance.Visibility = System.Windows.Visibility.Visible ;
            this.TxtPuissanceInstalle.Visibility = System.Windows.Visibility.Collapsed ;
        }

        private void btn_Imprimer_Click(object sender, RoutedEventArgs e)
        {
            List<CsDemandeBase> leDemandeAEditer = new List<CsDemandeBase>();
            laDetailDemande.LaDemande.NOMCLIENT = laDetailDemande.LeClient.NOMABON;
            laDetailDemande.LaDemande.COMMUNE = laDetailDemande.Ag.LIBELLECOMMUNE;
            laDetailDemande.LaDemande.QUARTIER = laDetailDemande.Ag.LIBELLEQUARTIER;
            laDetailDemande.LaDemande.LIBELLETACHE = laDetailDemande.Ag.TELEPHONE;
            laDetailDemande.LaDemande.CODEELECTRIQUE = laDetailDemande.Ag.RUE;
            laDetailDemande.LaDemande.CTAXEG = laDetailDemande.Ag.PORTE;
            laDetailDemande.LaDemande.LIBELLETACHE = laDetailDemande.Ag.TELEPHONE;
            leDemandeAEditer.Add(laDetailDemande.LaDemande);
            Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(leDemandeAEditer, null, SessionObject.CheminImpression, "FicheIntervention", "Accueil", true);
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


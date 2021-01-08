using Galatee.Silverlight.ServiceAdministration;
using Galatee.Silverlight.ServiceAuthenInitialize;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Administration
{
    public partial class FrmParametragePoste : ChildWindow
    {
        public FrmParametragePoste()
        {
            InitializeComponent();
            GetData();
        }
        public string leAgence = string.Empty;
        bool IsOkClik = false;
        public void GetData()
        {
            this.Cbo_Centre.IsEnabled = false;
            this.Cbo_Caisse.IsEnabled = false;
            isChargement = true;
            retourneCentre();
            retourneCaisseDispo();

        }
        CsPoste lePosteModif;
        public FrmParametragePoste(CsPoste lePoste)
        {
            lePosteModif = new CsPoste();
            InitializeComponent();
            lePosteModif = lePoste;
            retourneCaisseDispo();
            this.txtMomMachine.Text = string.IsNullOrEmpty(lePoste.NOMPOSTE) ? string.Empty : lePoste.NOMPOSTE;

        }
        bool isChargement = false;
        public void retourneCentre()
        {
            try
            {
                    if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                    {
                        List<ServiceAccueil.CsSite> lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(SessionObject.LstCentre);
                        Cbo_Site.ItemsSource = lstSite;
                        Cbo_Site.DisplayMemberPath = "LIBELLE";

                        if (lePosteModif != null)
                        {
                            isChargement = true;
                            ServiceAccueil.CsCentre leC = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == lePosteModif.FK_IDCENTRE);
                            if (leC != null && !string.IsNullOrEmpty(leC.CODE))
                                Cbo_Site.SelectedItem = lstSite.FirstOrDefault(t => t.PK_ID == leC.FK_IDCODESITE);
                        }
                        return;
                    }
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;

                        SessionObject.LstCentre = args.Result;
                       List<ServiceAccueil.CsSite> lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(SessionObject.LstCentre);
                       Cbo_Site.ItemsSource = null ;
                       Cbo_Site.ItemsSource = lstSite;
                        Cbo_Site.DisplayMemberPath = "LIBELLE";
                        if (lePosteModif != null)
                        {
                            isChargement = true;
                            ServiceAccueil.CsCentre leC = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == lePosteModif.FK_IDCENTRE);
                            if (leC != null && !string.IsNullOrEmpty(leC.CODE))
                                Cbo_Site.SelectedItem = lstSite.FirstOrDefault(t => t.PK_ID == leC.FK_IDCODESITE);
                        }
                    };
                    service.ListeDesDonneesDesSiteAsync(false );
                    service.CloseAsync();
              

            }
            catch (Exception)
            {

                throw;
            }
        
        }

        public void retourneSite()
        {
            try
            {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneTousSiteCompleted  += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                    };
                    service.RetourneTousSiteAsync ();
                    service.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }

        }

        List<ServiceCaisse.CsCaisse> listedesCaisse = new List<ServiceCaisse.CsCaisse>();
        public void retourneCaisseDispo()
        {
            try
            {
              
                Galatee.Silverlight.ServiceCaisse.CaisseServiceClient service = new Galatee.Silverlight.ServiceCaisse.CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                service.ChargerCaisseDisponibleCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                    listedesCaisse = args.Result;
                    retourneCentre();
                };
                service.ChargerCaisseDisponibleAsync();
                service.CloseAsync();


            }
            catch (Exception)
            {
                throw;
            }

        }

        public void MiseAjourPoste()
        {
            try
            {
                string machine = this.txtMomMachine.Text;
                if(this.Cbo_Centre .SelectedItem != null && !string.IsNullOrEmpty(this.txtMomMachine.Text))
                {
                    ServiceCaisse.CsCaisse laCaisseSelect = new ServiceCaisse.CsCaisse();
                    if (this.Cbo_Caisse.SelectedItem != null)
                        laCaisseSelect = (ServiceCaisse.CsCaisse )Cbo_Caisse.SelectedItem ;
                    Galatee.Silverlight.ServiceAccueil.CsCentre leCentre = Cbo_Centre.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsCentre;
                    CsPoste lePoste = new CsPoste()
                    {
                      CODECENTRE  = leCentre.CODE,
                      FK_IDCENTRE = leCentre.PK_ID ,
                      NOMPOSTE = this.txtMomMachine.Text.ToUpper().Trim(),
                      NUMCAISSE = (laCaisseSelect == null ? string.Empty : laCaisseSelect.NUMCAISSE),
                    };
                    lePoste.FK_IDCAISSE = null;


                    if (laCaisseSelect != null && !string.IsNullOrEmpty(laCaisseSelect.NUMCAISSE))
                    {
                        lePoste.FK_IDCAISSE = laCaisseSelect.PK_ID;

                        /** ZEG 29/08/2017 **/
                        if (SessionObject.iDCaisseDeclaree == null && lePoste.FK_IDCAISSE != null)
                            SessionObject.iDCaisseDeclaree = lePoste.FK_IDCAISSE;


                    }

                    Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient service = new Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                    service.InsertPosteCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        Classes.IsolatedStorage.storeMachine(lePoste.NOMPOSTE);
                        Classes.IsolatedStorage.storeCentre(lePoste.CODECENTRE);


                        /** ZEG 29/08/2017 **/
                        if (SessionObject.iDCaisseDeclaree == null && lePoste.FK_IDCAISSE != null)
                            SessionObject.iDCaisseDeclaree = lePoste.FK_IDCAISSE;

                    };
                    service.InsertPosteAsync(lePoste);
                    service.CloseAsync();
                }
                
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void UpdatePoste()
        {
            try
            {


                if (this.Cbo_Centre.SelectedItem != null && !string.IsNullOrEmpty(lePosteModif.NOMPOSTE ))
                {
                    ServiceCaisse.CsCaisse laCaisseSelect = new ServiceCaisse.CsCaisse();
                    if (this.Cbo_Caisse.SelectedItem != null)
                        laCaisseSelect = (ServiceCaisse.CsCaisse)Cbo_Caisse.SelectedItem;
                    Galatee.Silverlight.ServiceAccueil.CsCentre leCentre = Cbo_Centre.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsCentre;
                  
                    
                         lePosteModif.CODECENTRE = leCentre.CODE;
                         lePosteModif.FK_IDCENTRE = leCentre.PK_ID;
                         lePosteModif.NOMPOSTE = lePosteModif.NOMPOSTE ;
                         lePosteModif.NUMCAISSE = (laCaisseSelect == null ? string.Empty : laCaisseSelect.NUMCAISSE);
                         lePosteModif.FK_IDCAISSE = null;
                    if (laCaisseSelect != null && !string.IsNullOrEmpty(laCaisseSelect.NUMCAISSE))
                        lePosteModif.FK_IDCAISSE = laCaisseSelect.PK_ID;


                    /** ZEG 29/08/2017 **/
                    if (SessionObject.iDCaisseDeclaree == null && lePosteModif.FK_IDCAISSE != null)
                        SessionObject.iDCaisseDeclaree = lePosteModif.FK_IDCAISSE;


                    Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient service = new Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                    service.UpdatePosteCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.ListePoste  = args.Result;
                    };
                    service.UpdatePosteAsync(lePosteModif);
                    service.CloseAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }

        } 

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            IsOkClik = true;
            if (string.IsNullOrEmpty(this.txtMomMachine.Text))
            {
                IsOkClik = false;
                Message.ShowInformation("Veuillez saisir le nom de la machine", this.Title.ToString());
                return;
            }
            
            this.DialogResult = true;
            if (this.Cbo_Centre.SelectedItem != null && lePosteModif == null)
                MiseAjourPoste();
            else
                UpdatePoste();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true ;

        }

        private void Cbo_Site_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cbo_Site.SelectedItem != null)
            {
                string leSite = ((Galatee.Silverlight.ServiceAccueil.CsSite)Cbo_Site.SelectedItem).CODE;

                this.Cbo_Centre.IsEnabled = true;
                Cbo_Centre.ItemsSource = SessionObject.LstCentre.Where(p=>p.CODESITE == leSite).ToList();
                Cbo_Centre.SelectedValuePath = "CODE";
                Cbo_Centre.DisplayMemberPath = "LIBELLE";

                if (lePosteModif != null && !string.IsNullOrEmpty( lePosteModif.CODECENTRE) )
                    Cbo_Centre.SelectedItem = SessionObject.LstCentre.FirstOrDefault (k => lePosteModif.FK_IDCENTRE == k.PK_ID);


            }
        }

        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cbo_Centre.SelectedItem != null )
            {
                Cbo_Caisse.SelectedItem = null;
                this.Cbo_Caisse.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsCentre lecentreSelect = (Galatee.Silverlight.ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem;
                leAgence = lecentreSelect.CODE;
                if (lePosteModif != null && !string.IsNullOrEmpty(lePosteModif.CODECENTRE) && isChargement && lePosteModif.FK_IDCAISSE != null )
                {
                    listedesCaisse.Add(SessionObject.ListeCaisse.FirstOrDefault(t => t.PK_ID == lePosteModif.FK_IDCAISSE));
                    Cbo_Caisse.ItemsSource = listedesCaisse;
                    Cbo_Caisse.DisplayMemberPath = "NUMCAISSE";
                    Cbo_Caisse.SelectedItem = listedesCaisse.FirstOrDefault(t => t.PK_ID == lePosteModif.FK_IDCAISSE);

                }
                //if (listedesCaisse != null && listedesCaisse.Count != 0 && !isChargement)
               if (listedesCaisse != null && listedesCaisse.Count != 0 )
                {
                    List<Galatee.Silverlight.ServiceCaisse.CsCaisse> lstCaisseCentre  = listedesCaisse.Where(t => t.FK_IDCENTRE == lecentreSelect.PK_ID).ToList();
                    Cbo_Caisse.ItemsSource = lstCaisseCentre;
                    Cbo_Caisse.DisplayMemberPath = "NUMCAISSE";
                }
                isChargement = false;    
            }
        }

        private void Cbo_Caisse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cbo_Caisse.SelectedItem != null)
            {
                this.Cbo_Caisse.IsEnabled = true;
                Galatee.Silverlight.ServiceCaisse.CsCaisse laCaisseSelect = (Galatee.Silverlight.ServiceCaisse.CsCaisse)Cbo_Caisse.SelectedItem;
              
            }
        }
    }
}


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

namespace Galatee.Silverlight.InterfaceComptable
{
    public partial class FrmMiseAJourGC : ChildWindow
    {
        public FrmMiseAJourGC()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }
        string leEtatExecuter = string.Empty;

        public FrmMiseAJourGC(string typeEtat)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            leEtatExecuter = typeEtat;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            List<int> lstCentre = new List<int>();
            DateTime dateDebut = System.DateTime.Today ;
            DateTime dateFin = dateDebut.AddYears(3);
            string Typedemande = string.Empty;
            string Produit = string.Empty;

            dateDebut = string.IsNullOrEmpty(this.dtp_DateDebut.Text) ? dateDebut : Convert.ToDateTime(this.dtp_DateDebut.Text);
            dateFin = string.IsNullOrEmpty(this.dtp_DateFin.Text) ? dateFin : Convert.ToDateTime(this.dtp_DateFin.Text);
            EtatDesMiseAjour(dateDebut, dateFin);
            
        }
        private void EtatDesMiseAjour( DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient service1 = new ServiceInterfaceComptable.InterfaceComptableServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("InterfaceComptable"));
            service1.RetourneEtatMiseAJourGrandCompteCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    Utility.ActionDirectOrientation<ServicePrintings.CsComptabilisation, ServiceInterfaceComptable.CsComptabilisation>(res.Result, null, SessionObject.CheminImpression, "Operation", "InterfaceComptable", true);
                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.RetourneEtatMiseAJourGrandCompteAsync(dateDebut, dateFin);
            service1.CloseAsync();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        List<int> lesCentreCaisse = new List<int>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        Galatee.Silverlight.ServiceAccueil.CsSite lSiteSelect = new Galatee.Silverlight.ServiceAccueil.CsSite();
        List<Galatee.Silverlight.ServiceAccueil.CsProduit> lProduitSelect = new List<Galatee.Silverlight.ServiceAccueil.CsProduit>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void EtatAvanceConsomation(List<string> LstCateg, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceInterfaceComptable.InterfaceComptableServiceClient service1 = new ServiceInterfaceComptable.InterfaceComptableServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("InterfaceComptable"));
            service1.RetourneEtatMiseAJourGrandCompteCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    Utility.ActionDirectOrientation<ServicePrintings.CsComptabilisation, ServiceInterfaceComptable.CsComptabilisation>(res.Result, null, SessionObject.CheminImpression, "Operation", "InterfaceComptable", true);
                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.RetourneEtatMiseAJourGrandCompteAsync(dateDebut, dateFin);
            service1.CloseAsync();
        }
    }
}


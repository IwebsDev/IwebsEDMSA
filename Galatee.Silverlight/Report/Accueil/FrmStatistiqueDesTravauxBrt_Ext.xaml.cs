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

namespace Galatee.Silverlight.Report
{
    public partial class FrmStatistiqueDesTravauxBrt_Ext : ChildWindow
    {
        public FrmStatistiqueDesTravauxBrt_Ext()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            //ChargerTypeDemande();
            ChargerListeDeProduit();
        }
        string leEtatExecuter = string.Empty;

        public FrmStatistiqueDesTravauxBrt_Ext(string typeEtat)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            //ChargerTypeDemande();
            ChargerListeDeProduit();
            leEtatExecuter = typeEtat;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string codesite = string.Empty;
            string codeperiode = string.Empty;
            //DateTime dateDebut = System.DateTime.Today ;
            //DateTime dateFin = dateDebut.AddYears(3);
            string Typedemande = string.Empty;
            List<string> Produit = new List<string>();

            if (lSiteSelect != null)
                codesite = lSiteSelect.CODE;
            else
            {
                Message.ShowWarning("Veuillez sélectionnez le site", "Validation");
                return;
            }
            if (!string.IsNullOrWhiteSpace(Txt_Periode.Text))
                codeperiode = Txt_Periode.Text;
            else
            {
                Message.ShowWarning("Veuillez saisir la période", "Validation");
                return;
            }
            if(this.Txt_Produit.Tag!=null)
            Produit = this.Txt_Produit.Tag != null ?(List<string>) this.Txt_Produit.Tag:new List<string>() ;
            else
            {
                Message.ShowWarning("Veuillez sélectionnez le produit", "Validation");
                return;
            }
            //if (this.Txt_TypeDemande.Tag!=null)
            //Typedemande =this.Txt_TypeDemande.Tag != null ?this.Txt_TypeDemande.Tag.ToString() :string.Empty;
            //else
            //{
            //    Message.ShowWarning("Veuillez sélectionnez le type de demande", "Validation");
            //    return;
            //}
            //dateDebut = string.IsNullOrEmpty(this.dtp_DateDebut.Text) ? dateDebut : Convert.ToDateTime(this.dtp_DateDebut.Text);
            //dateFin = string.IsNullOrEmpty(this.dtp_DateFin.Text) ? dateFin : Convert.ToDateTime(this.dtp_DateFin.Text);

            prgBar.Visibility = System.Windows.Visibility.Visible ;

            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service1.RetourneStatistiqueTravaux_Brt_ExtCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("pPeriode", Txt_Periode.Text);
                    param.Add("pParametre", "TRAVAUX DE BRANCHEMENTS ET EXTENSION- "+Txt_Periode.Text.Split('/')[1]);
                    Utility.ActionExportation<ServicePrintings.CsStatistiqueTravaux_Brt_Ext, ServiceAccueil.CsStatistiqueTravaux_Brt_Ext>(res.Result, param, string.Empty, SessionObject.CheminImpression, "Travaux_Branchement_Extension", "Report", true, "xlsx");
                    //Utility.ActionDirectOrientation<ServicePrintings.CsDonnesStatistiqueDemande, ServiceAccueil.CsDonnesStatistiqueDemande>(res.Result, param, SessionObject.CheminImpression, "DonnesStatistiqueDemande", "Report", true);
                    //Utility.ActionDirectOrientation<ServicePrintings.CsDonnesStatistiqueDemande, ServiceAccueil.CsDonnesStatistiqueDemande>(res.Result, param, SessionObject.CheminImpression, "DonnesStatistiqueDemande", "Report", true);

                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.RetourneStatistiqueTravaux_Brt_ExtAsync(Produit,Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(codeperiode));
            service1.CloseAsync();
            //if (leEtatExecuter == SessionObject.DevisValiderHorsDelais)
            //    DevisValiderHorsLesDelais(lstCentre, dateDebut, dateFin, Typedemande, Produit);

            //if (leEtatExecuter == SessionObject.DevisValiderDelais)
            //    DevisValiderDansLesDelais (lstCentre, dateDebut, dateFin, Typedemande, Produit);

            //if (leEtatExecuter == SessionObject.DemandeEnAttenteLiaison)
            //    DemandeEnAttenteDeLiaison(lstCentre, dateDebut, dateFin, Typedemande, Produit);

            //if (leEtatExecuter == SessionObject.TravauxValiderDelais)
            //    TravauxRealierDansLesDelais(lstCentre, dateDebut, dateFin, Typedemande, Produit);

            //if (leEtatExecuter == SessionObject.TravauxValiderHorsDelais)
            //    TravauxRealierhorsDelais(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            
            //if (leEtatExecuter == SessionObject.TravauxRealiser )
            //    TravauxRealier(lstCentre, dateDebut, dateFin, Typedemande, Produit);

            //if (leEtatExecuter == SessionObject.TravauxNonRealiser  )
            //    TravauxNonRealier(lstCentre, dateDebut, dateFin, Typedemande, Produit);

            //if (leEtatExecuter == SessionObject.RegistreDemande)
            //    RegistreDemande(lstCentre, dateDebut, dateFin, Typedemande, Produit);

            //if (leEtatExecuter == SessionObject.DemandeEnAttenteDeRealisation)
                //DemandeEnAttenteDeRealisation(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            
        }
        private void DevisValiderDansLesDelais(List<int> lstCentre,DateTime  dateDebut,DateTime dateFin,List<string>  Typedemande,string  Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneDevisTerminerDsLesDelaisCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("pParametre", "LISTE DE DEMANDE VALIDEES DANS LES DELAIS");
                    Utility.ActionExportation<ServicePrintings.CsDemandeBase, ServiceReport.CsDemandeBase>(res.Result, param, string.Empty, SessionObject.CheminImpression, "Demande", "Report", true, "xlsx");
                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneDevisTerminerDsLesDelaisAsync(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        
        }

        private void TravauxRealierDansLesDelais(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, string Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneTravauxRealiserDsDelaisCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("pParametre", "LISTE DE DEMANDE REALISES DANS DELAIS");
                    Utility.ActionExportation<ServicePrintings.CsDemandeBase, ServiceReport.CsDemandeBase>(res.Result, param, string.Empty, SessionObject.CheminImpression, "Demande", "Report", true, "xlsx");

                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneTravauxRealiserDsDelaisAsync(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        }

        private void TravauxRealierhorsDelais(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, string Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneTravauxRealiserHorsDelaisCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("pParametre", "LISTE DE DEMANDE REALISES DANS DELAIS");
                    Utility.ActionExportation<ServicePrintings.CsDemandeBase, ServiceReport.CsDemandeBase>(res.Result, param, string.Empty, SessionObject.CheminImpression, "Demande", "Report", true, "xlsx");

                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneTravauxRealiserHorsDelaisAsync(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        }
        private void TravauxNonRealier(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, string Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneDemandeEnAttenteDeRealisationCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("pParametre", "LISTE DE DEMANDE EN ATTENTE DE REALISATION");
                    Utility.ActionExportation<ServicePrintings.CsDemandeBase, ServiceReport.CsDemandeBase>(res.Result, param, string.Empty, SessionObject.CheminImpression, "Demande", "Report", true, "xlsx");

                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneDemandeEnAttenteDeRealisationAsync(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        }
        private void TravauxRealier(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, string Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneTravauxRealiserCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("pParametre", "LISTE DE DEMANDE REALISES");
                    Utility.ActionExportation<ServicePrintings.CsDemandeBase, ServiceReport.CsDemandeBase>(res.Result, param, string.Empty, SessionObject.CheminImpression, "Demande", "Report", true, "xlsx");

                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneTravauxRealiserAsync(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        }
        private void DevisValiderHorsLesDelais(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, string Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Report"));
            service1.ReturneDevisTerminerHorsDelaisCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed ;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("pParametre", "LISTE DE DEMANDE VALIDEES HORS DELAIS");
                    Utility.ActionExportation<ServicePrintings.CsDemandeBase, ServiceReport.CsDemandeBase>(res.Result, param, string.Empty, SessionObject.CheminImpression, "Demande", "Report", true, "xlsx");

                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneDevisTerminerHorsDelaisAsync(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        }

        private void DemandeParType(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, string Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneDemandeParTypeCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("pParametre", "LISTE DE DEMANDE PAR TYPE");
                    Utility.ActionExportation<ServicePrintings.CsDemandeBase, ServiceReport.CsDemandeBase>(res.Result, param, string.Empty, SessionObject.CheminImpression, "Demande", "Report", true, "xlsx");

                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneDemandeParTypeAsync(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        }

        private void DemandeEnAttenteDeLiaison(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, string Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneDevisPayeEnInstanceDeLiaisonCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("pParametre", "LISTE DE DEMANDE EN ATTENTE DE LIAISON");
                    Utility.ActionExportation<ServicePrintings.CsDemandeBase, ServiceReport.CsDemandeBase>(res.Result, param, string.Empty, SessionObject.CheminImpression, "Demande", "Report", true, "xlsx");

                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneDevisPayeEnInstanceDeLiaisonAsync(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        }

        private void DemandeEnAttenteDeRealisation(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, string Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneDemandeEnAttenteDeRealisationCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("pParametre", "LISTE DE DEMANDE EN ATTENTE DE REALISATION");
                    Utility.ActionExportation<ServicePrintings.CsDemandeBase, ServiceReport.CsDemandeBase>(res.Result, param, string.Empty, SessionObject.CheminImpression, "Demande", "Report", true, "xlsx");

                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneDemandeEnAttenteDeRealisationAsync(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        }

        private void RegistreDemande(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, string Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneRegistreDemandeCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    Utility.ActionExportation<ServicePrintings.CsDemandeBase, ServiceReport.CsDemandeBase>(res.Result, null, string.Empty, SessionObject.CheminImpression, "RegistreDemande", "Report", true, "xlsx");
                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneRegistreDemandeAsync(lstCentre, dateDebut, dateFin, Typedemande, Produit);
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

                    if (lstSite != null && lstSite.Count != 0)
                    {
                        if (lstSite.Count == 1)
                        {
                            lSiteSelect = lstSite.First();
                            //this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        }
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().CODE;
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.Txt_LibelleCentre.Tag = LstCentrePerimetre.First();
                        lProduitSelect = LstCentrePerimetre.First().LESPRODUITSDUSITE;

                        if (lProduitSelect != null && lProduitSelect.Count != 0)
                        {
                            if (lProduitSelect.Count == 1)
                            {
                                this.Txt_Produit.Text = lProduitSelect.First().LIBELLE;
                                this.Txt_Produit.Tag = lProduitSelect.First().CODE;
                            }
                        }
                    }
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
                    if (lstSite != null && lstSite.Count != 0)
                    {
                        if (lstSite.Count == 1)
                        {
                            lSiteSelect = lstSite.First();
                            //this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        }
                    }
                    if (LstCentrePerimetre != null && LstCentrePerimetre.Count != 0)
                    {
                        if (LstCentrePerimetre.Count == 1)
                        {
                            this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                            this.Txt_LibelleCentre.Tag = LstCentrePerimetre.First().PK_ID;
                            this.btn_Centre.IsEnabled = false;
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChargerTypeDemande()
        {
            try
            {
                if (SessionObject.LstTypeDemande != null && SessionObject.LstTypeDemande.Count != 0)
                    return;

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneOptionDemandeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.LstTypeDemande = res.Result;
                };
                service1.RetourneOptionDemandeAsync();
                service1.CloseAsync();
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
                if (SessionObject.ListeDesProduit != null && SessionObject.ListeDesProduit.Count != 0)
                    return;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = res.Result;
                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite.Count > 0)
                {
                    //this.btn_Site.IsEnabled = false;
                    List<object> _Listgen = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedSite);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message,"Report");
            }

        }
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                //this.Txt_LibelleSite.Text = leSite.LIBELLE;
                //this.Txt_LibelleSite.Tag = leSite.PK_ID;
                lSiteSelect = leSite;
                List<ServiceAccueil.CsCentre> lsiteCentre = new List<ServiceAccueil.CsCentre>();
                    //LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).ToList();
                if (lsiteCentre.Count == 1)
                {
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_LibelleCentre.Tag = lsiteCentre.First();
                    lProduitSelect = lsiteCentre.First().LESPRODUITSDUSITE;
                    this.btn_Centre.IsEnabled = true;
                }
                else
                {
                    this.Txt_LibelleCentre.Text = string.Empty;
                    this.Txt_LibelleCentre.Tag = null;
                    this.btn_Centre.IsEnabled = true;
                }
            }
            //this.btn_Site.IsEnabled = true;

        }
        private void btn_Produit_Click(object sender, RoutedEventArgs e)
        {
            if (SessionObject.ListeDesProduit != null && SessionObject.ListeDesProduit.Count > 0)
            {

                this.btn_Produit.IsEnabled = false;
                List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsProduit>(SessionObject.ListeDesProduit);
                Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, true, "Typde demande");
                //List<object> _Listgen = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lProduitSelect);
                //Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedProduit);
                ctr.Show();
            }
        }
        void galatee_OkClickedProduit(object sender, EventArgs e)
        {
            //Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
            
            if (ctrs.isOkClick)
            {
                List<ServiceAccueil.CParametre> _LesTypeDemandeSelect = ctrs.MyObjectList as List<ServiceAccueil.CParametre>;
                //ServiceAccueil.CsProduit leProduit = (ServiceAccueil.CsProduit)ctrs.MyObject;
                List<string> lstCentre = _LesTypeDemandeSelect.Select(t => t.CODE).ToList();
                foreach (string item in lstCentre)
                    this.Txt_Produit.Text = this.Txt_Produit.Text + " "+item ;
                this.Txt_Produit.Tag = _LesTypeDemandeSelect.Select(t => t.CODE).ToList();

                //this.Txt_Produit.Text = leProduit.LIBELLE;
                //this.Txt_Produit.Tag = leProduit.CODE;
            }
            this.btn_Produit.IsEnabled = true;

        }

        private void btn_TypeDemande_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SessionObject.LstTypeDemande.Count > 0)
                {
                    //btn_TypeDemande.IsEnabled = false;
                    List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsTdem >(SessionObject.LstTypeDemande);
                    Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, true, "Typde demande");
                    ctr.Closed += new EventHandler(galatee_OkClickedTypeDemande);
                    ctr.Show();

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Report");
            }

        }
        void galatee_OkClickedTypeDemande(object sender, EventArgs e)
        {
            //btn_TypeDemande.IsEnabled = true;
            Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
            if (ctrs.isOkClick)
            {
                List<ServiceAccueil.CParametre> _LesTypeDemandeSelect = ctrs.MyObjectList as List<ServiceAccueil.CParametre>;
                List<string> lstCentre = _LesTypeDemandeSelect.Select(t => t.CODE).ToList();
                //foreach (string item in lstCentre)
                //    this.Txt_TypeDemande.Text = item + " ";
                //this.Txt_TypeDemande.Tag = _LesTypeDemandeSelect.Select(t => t.CODE ).ToList().First();
            }

        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentrePerimetre.Count != 0)
                {
                    List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsCentre>(LstCentrePerimetre);
                    Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, true, "Centre");
                    ctr.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            try
            {

                Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
                if (ctrs.isOkClick)
                {
                    List<ServiceAccueil.CParametre> _LesCentreSelect = ctrs.MyObjectList as List<ServiceAccueil.CParametre>;
                    List<string> lstCentre = _LesCentreSelect.Select(t => t.CODE).ToList();
                    foreach (string item in lstCentre)
                        this.Txt_LibelleCentre.Text = item + " ";
                    this.Txt_LibelleCentre.Tag = _LesCentreSelect.Select(t => t.PK_ID).ToList();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
    }
}


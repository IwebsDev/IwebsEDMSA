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
    public partial class FrmDevisValide : ChildWindow
    {
        public FrmDevisValide()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            ChargerTypeDemande();
            ChargerListeDeProduit();
        }
        string leEtatExecuter = string.Empty;

        public FrmDevisValide(string typeEtat)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            ChargerTypeDemande();
            ChargerListeDeProduit();
            leEtatExecuter = typeEtat;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            List<int> lstCentre = new List<int>();
            List<int> lstIdCende = new List<int>();
            DateTime dateFin =System.DateTime.Today;
            DateTime dateDebut = dateFin.AddYears(-100);

            List<string> Typedemande = new List<string>();
            //string Produit = string.Empty;
            List<string> Produit = new List<string>();

            //omael 08/05/2020
            if (this.Txt_LibelleCentre.Text != null)
            {
                //var items = this.Txt_LibelleCentre.Text.Split(' ').Where(s => s != String.Empty);

                //List<int> nbItems = items.Select(int.Parse).ToList();

                //for (int i = 0; i < nbItems.Count; i++)
                //{
                //    lstCentre.Add(nbItems[i]);
                //}


                foreach (ServiceAccueil.CParametre st in this._LesCentreSelect)
                {
                    lstIdCende.Add(st.PK_ID);
                }


            }
                //lstCentre.Add(((Galatee.Silverlight.ServiceAccueil.CsCentre)this.Txt_LibelleCentre.Tag).PK_ID);

            if (this.Txt_Produit.Tag != null)
                Produit = this.Txt_Produit.Tag != null ? (List<string>)this.Txt_Produit.Tag : new List<string>();
            else
            {
                Message.ShowWarning("Veuillez sélectionnez le produit", "Validation");
                return;
            }
            Typedemande = this.Txt_TypeDemande.Tag != null ?(List<string>) this.Txt_TypeDemande.Tag :SessionObject.LstTypeDemande.Select(p=>p.CODE ).ToList()  ;
            dateDebut = string.IsNullOrEmpty(this.dtp_DateDebut.Text) ? dateDebut : Convert.ToDateTime(this.dtp_DateDebut.Text);
            dateFin = string.IsNullOrEmpty(this.dtp_DateFin.Text) ? dateFin : Convert.ToDateTime(this.dtp_DateFin.Text);

            prgBar.Visibility = System.Windows.Visibility.Visible ;


            if (leEtatExecuter == SessionObject.DevisValiderHorsDelais)
                DevisValiderHorsLesDelais(lstCentre, dateDebut, dateFin, Typedemande, Produit);

            if (leEtatExecuter == SessionObject.DevisValiderDelais)
                DevisValiderDansLesDelais (lstCentre, dateDebut, dateFin, Typedemande, Produit);

            if (leEtatExecuter == SessionObject.DemandeEnAttenteLiaison)
                DemandeEnAttenteDeLiaison(lstCentre, dateDebut, dateFin, Typedemande, Produit);

            if (leEtatExecuter == SessionObject.TravauxValiderDelais)
                TravauxRealierDansLesDelais(lstCentre, dateDebut, dateFin, Typedemande, Produit);

            if (leEtatExecuter == SessionObject.TravauxValiderHorsDelais)
                TravauxRealierhorsDelais(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            
            if (leEtatExecuter == SessionObject.TravauxRealiser )
                TravauxRealier(lstCentre, dateDebut, dateFin, Typedemande, Produit);

            if (leEtatExecuter == SessionObject.TravauxNonRealiser  )
                TravauxNonRealier(lstCentre, dateDebut, dateFin, Typedemande, Produit);

            if (leEtatExecuter == SessionObject.RegistreDemande)
                RegistreDemande(lstCentre, dateDebut, dateFin, Typedemande, Produit);

            if (leEtatExecuter == SessionObject.DemandeEnAttenteDeRealisation)
                DemandeEnAttenteDeRealisation(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            
        }
        private void DevisValiderDansLesDelais(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, List<string> Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneDevisTerminerDsLesDelais_Completed += (sr, res) =>
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
            service1.ReturneDevisTerminerDsLesDelais_Async(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        
        }

        private void TravauxRealierDansLesDelais(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, List<string> Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneTravauxRealiserDsDelais_Completed += (sr, res) =>
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
            service1.ReturneTravauxRealiserDsDelais_Async(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        }

        private void TravauxRealierhorsDelais(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, List<string> Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneTravauxRealiserHorsDelais_Completed += (sr, res) =>
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
            service1.ReturneTravauxRealiserHorsDelais_Async(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        }
        private void TravauxNonRealier(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, List<string> Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneDemandeEnAttenteDeRealisation_Completed += (sr, res) =>
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
            service1.ReturneDemandeEnAttenteDeRealisation_Async(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        }
        private void TravauxRealier(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, List<string> Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneTravauxRealiser_Completed += (sr, res) =>
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
            service1.ReturneTravauxRealiser_Async(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        }
        private void DevisValiderHorsLesDelais(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, List<string> Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Report"));
            service1.ReturneDevisTerminerHorsDelais_Completed += (sr, res) =>
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
            service1.ReturneDevisTerminerHorsDelais_Async(lstCentre, dateDebut, dateFin, Typedemande, Produit);
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

        private void DemandeEnAttenteDeLiaison(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, List<string> Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneDevisPayeEnInstanceDeLiaison_Completed += (sr, res) =>
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
            service1.ReturneDevisPayeEnInstanceDeLiaison_Async(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        }

        private void DemandeEnAttenteDeRealisation(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, List<string> Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneDemandeEnAttenteDeRealisation_Completed += (sr, res) =>
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
            service1.ReturneDemandeEnAttenteDeRealisation_Async(lstCentre, dateDebut, dateFin, Typedemande, Produit);
            service1.CloseAsync();
        }

        private void RegistreDemande(List<int> lstCentre, DateTime dateDebut, DateTime dateFin, List<string> Typedemande, List<string> Produit)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneRegistreDemande_Completed += (sr, res) =>
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
            service1.ReturneRegistreDemande_Async(lstCentre, dateDebut, dateFin, Typedemande, Produit);
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
                            this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        }
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().CODE;
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.Txt_LibelleCentre.Tag = LstCentrePerimetre.First();
                      
                    }

                    lProduitSelect = LstCentrePerimetre.First().LESPRODUITSDUSITE;

                    //if (lProduitSelect != null && lProduitSelect.Count != 0)
                    //{
                    //    if (lProduitSelect.Count == 1)
                    //    {
                    //        this.Txt_Produit.Text = lProduitSelect.First().LIBELLE;
                    //        this.Txt_Produit.Tag = lProduitSelect.First().CODE;
                    //    }
                    //}
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
                            this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
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
                    this.btn_Site.IsEnabled = false;
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
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
                this.Txt_LibelleSite.Tag = leSite.PK_ID;
                lSiteSelect = leSite;
                List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).ToList();
                lProduitSelect = lsiteCentre.First().LESPRODUITSDUSITE;
               
                if (lsiteCentre.Count == 1)
                {
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_LibelleCentre.Tag = lsiteCentre.First();
                    this.btn_Centre.IsEnabled = true;
                }
                else
                {
                    this.Txt_LibelleCentre.Text = string.Empty;
                    this.Txt_LibelleCentre.Tag = null;
                    this.btn_Centre.IsEnabled = true;
                }
            }
            this.btn_Site.IsEnabled = true;

        }
        private void btn_Produit_Click(object sender, RoutedEventArgs e)
        {
            if (lProduitSelect != null && lProduitSelect.Count > 0)
            {
                //this.btn_Produit.IsEnabled = false;
                //List<object> _Listgen = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lProduitSelect);
                //Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                //ctr.Closed += new EventHandler(galatee_OkClickedProduit);
                //ctr.Show();



                this.btn_Produit.IsEnabled = false;
                List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsProduit>(lProduitSelect);
                Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, true, "Produit");
                ctr.Closed += new EventHandler(galatee_OkClickedProduit);
                ctr.Show();
            }
        }
        void galatee_OkClickedProduit(object sender, EventArgs e)
        {
            //Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            //if (ctrs.isOkClick)
            //{
            //    ServiceAccueil.CsProduit leProduit = (ServiceAccueil.CsProduit)ctrs.MyObject;
            //    this.Txt_Produit.Text = leProduit.LIBELLE;
            //    this.Txt_Produit.Tag = leProduit.CODE;
            //}
            //this.btn_Produit.IsEnabled = true;


            this.btn_Produit.IsEnabled = true;
            Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
            if (ctrs.isOkClick)
            {
                List<ServiceAccueil.CParametre> _LesTypeDemandeSelect = ctrs.MyObjectList as List<ServiceAccueil.CParametre>;
                List<string> lstCentre = _LesTypeDemandeSelect.Select(t => t.CODE).ToList();
                this.Txt_Produit.Text = string.Empty;
                foreach (string item in lstCentre)
                    this.Txt_Produit.Text = item + " " + this.Txt_Produit.Text;
                this.Txt_Produit.Tag = _LesTypeDemandeSelect.Select(t => t.CODE).ToList();
            }

        }

        private void btn_TypeDemande_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SessionObject.LstTypeDemande.Count > 0)
                {
                    btn_TypeDemande.IsEnabled = false;
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
            btn_TypeDemande.IsEnabled = true;
            Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
            if (ctrs.isOkClick)
            {
                List<ServiceAccueil.CParametre> _LesTypeDemandeSelect = ctrs.MyObjectList as List<ServiceAccueil.CParametre>;
                List<string> lstCentre = _LesTypeDemandeSelect.Select(t => t.CODE).ToList();
                this.Txt_TypeDemande.Text = string.Empty;
                foreach (string item in lstCentre)
                    this.Txt_TypeDemande.Text = item + " " + this.Txt_TypeDemande.Text;
                this.Txt_TypeDemande.Tag = _LesTypeDemandeSelect.Select(t => t.CODE ).ToList();
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
        //private void galatee_OkClickedCentre(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
        //        if (ctrs.isOkClick)
        //        {
        //            List<ServiceAccueil.CParametre> _LesCentreSelect = ctrs.MyObjectList as List<ServiceAccueil.CParametre>;
        //            List<string> lstCentre = _LesCentreSelect.Select(t => t.CODE).ToList();
        //            this.Txt_LibelleCentre.Text = string.Empty;
        //            foreach (string item in lstCentre)
        //            this.Txt_LibelleCentre.Text = item + " " + this.Txt_LibelleCentre.Text;

        //            this.Txt_LibelleCentre.Tag = _LesCentreSelect.Select(t => t.PK_ID).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex, "Erreur");
        //    }
        //}
        List<ServiceAccueil.CParametre> _LesCentreSelect;
        private void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            try
            {

                Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
                if (ctrs.isOkClick)
                {
                    _LesCentreSelect = ctrs.MyObjectList as List<ServiceAccueil.CParametre>;
                    List<string> lstCentre = _LesCentreSelect.Select(t => t.CODE).ToList();
                    //Txt_LibelleCentre.Text = lstCentre;
                    this.Txt_LibelleCentre.Text = string.Empty;
                    foreach (string item in lstCentre)
                        this.Txt_LibelleCentre.Text = item + " " + this.Txt_LibelleCentre.Text;

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


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
    public partial class FrmAvisEmisParPIA : ChildWindow
    {
        public FrmAvisEmisParPIA()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
        }
        string leEtatExecuter = string.Empty;

        public FrmAvisEmisParPIA(string typeEtat)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            leEtatExecuter = typeEtat;
            chk_Detail.Visibility = System.Windows.Visibility.Collapsed;
            chk_Recap.Visibility = System.Windows.Visibility.Collapsed;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            if (leEtatExecuter == SessionObject.TauxRecouvrement)
            {
                this.dtp_DateDebut.Visibility = Visibility.Collapsed;
                this.dtp_DateFin.Visibility = Visibility.Collapsed;
                lbl_Centre_Copy3.Visibility = System.Windows.Visibility.Collapsed;
                lbl_Centre_Copy4.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (leEtatExecuter == SessionObject.AvanceSurConsomation ||
                leEtatExecuter == SessionObject.EncaissementModeRegement ||
                leEtatExecuter == SessionObject.EncaissementReversement )
            {
                this.lbl_Centre_Copy1.Visibility = System.Windows.Visibility.Collapsed;
                this.btn_AgentPia.Visibility = System.Windows.Visibility.Collapsed;
                this.Cbo_AgentPIA.Visibility = System.Windows.Visibility.Collapsed;

                this.lbl_Centre_Copy1.Visibility = System.Windows.Visibility.Collapsed;
                this.btn_AgentPia.Visibility = System.Windows.Visibility.Collapsed;
                this.Cbo_AgentPIA.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (leEtatExecuter == SessionObject.TourneePIA)
            {
                lbl_Centre_Copy3.Visibility = System.Windows.Visibility.Collapsed;
                lbl_Centre_Copy4.Visibility = System.Windows.Visibility.Collapsed;
                this.dtp_DateDebut.Visibility = System.Windows.Visibility.Collapsed;
                this.dtp_DateFin.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (leEtatExecuter == SessionObject.AvisEmis ||
                leEtatExecuter == SessionObject.AvisCoupe ||
                leEtatExecuter == SessionObject.AvisRepose)
            {
                chk_Detail.Visibility = System.Windows.Visibility.Visible ;
                chk_Recap.Visibility = System.Windows.Visibility.Visible;
            }
            //if (leEtatExecuter == SessionObject.Vente ||
            //    leEtatExecuter == SessionObject.PrepaidSansAchatPeriode  ||
            //    leEtatExecuter == SessionObject.PrepaidSansJamaisAchat )
            //{
            //    lbl_Centre_Copy2.Visibility = System.Windows.Visibility.Collapsed;
            //    lbl_Centre_Copy5.Visibility = System.Windows.Visibility.Collapsed;
            //    this.txt_PeriodeDebut.Visibility = System.Windows.Visibility.Collapsed;
            //    this.txt_PeriodeFin.Visibility = System.Windows.Visibility.Collapsed;


            //}
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            int IdCentre = 0;
            List<string>  MatriculAgent = new List<string>() ;
            DateTime dateDebut = System.DateTime.Today ;
            DateTime dateFin = dateDebut.AddYears(3);
            string Typedemande = string.Empty;
            string Produit = string.Empty;
            List<int> LsIdCentre =new List<int>();
            //string PeriodeDebut = string.IsNullOrEmpty(this.txt_PeriodeDebut.Text) ? string.Empty : this.txt_PeriodeDebut.Text;
            //string PeriodeFin = string.IsNullOrEmpty(this.txt_PeriodeFin.Text) ? string.Empty : this.txt_PeriodeFin.Text; 

            if (this.Txt_LibelleCentre.Tag != null )
            {
                IdCentre =(int) this.Txt_LibelleCentre.Tag;
                LsIdCentre.Add(IdCentre);
            }

            if (this.Cbo_AgentPIA.Tag != null)
                MatriculAgent = ((List<ServiceAccueil.CParametre>)this.Cbo_AgentPIA.Tag).Select(o => o.CODE).ToList();
            else
                MatriculAgent = lstPia.Select(o => o.MATRICULE ).ToList();

            dateDebut = string.IsNullOrEmpty(this.dtp_DateDebut.Text) ? dateDebut : Convert.ToDateTime(this.dtp_DateDebut.Text);
            dateFin = string.IsNullOrEmpty(this.dtp_DateFin.Text) ? dateFin : Convert.ToDateTime(this.dtp_DateFin.Text);

            prgBar.Visibility = System.Windows.Visibility.Visible;

            if (leEtatExecuter == SessionObject.AvisEmis)
            {
                List<int> isAgent = new List<int>();
                if (Cbo_AgentPIA.Tag == null)
                    isAgent = lstPia.Where(p=>p.PK_ID != 0).Select(o => o.PK_ID).ToList();
                else
                    isAgent = ((List<ServiceAccueil.CParametre>)this.Cbo_AgentPIA.Tag).Where(p => p.PK_ID != 0).Select(y => y.PK_ID).ToList();
                AvisEmis(this.btn_Site.Tag.ToString(), isAgent, dateDebut, dateFin);
            }
            else if (leEtatExecuter == SessionObject.AvisCoupe)
            {
                List<int> isAgent = new List<int>();
                if (Cbo_AgentPIA.Tag == null)
                    isAgent = lstPia.Where(p => p.PK_ID != 0).Select(o => o.PK_ID).ToList();
                else
                    isAgent = ((List<ServiceAccueil.CParametre>)this.Cbo_AgentPIA.Tag).Where(p => p.PK_ID != 0).Select(y => y.PK_ID).ToList();

                AvisCoupe(this.btn_Site.Tag.ToString(), isAgent, dateDebut, dateFin);
            }
            else if (leEtatExecuter == SessionObject.AvisRepose)
            {
                List<int> isAgent = new List<int>();
                if (Cbo_AgentPIA.Tag == null)
                    isAgent = lstPia.Where(p => p.PK_ID != 0).Select(o => o.PK_ID).ToList();
                else
                    isAgent = ((List<ServiceAccueil.CParametre>)this.Cbo_AgentPIA.Tag).Where(p => p.PK_ID != 0).Select(y => y.PK_ID).ToList();
                ClientRemis(this.btn_Site.Tag.ToString(), isAgent, dateDebut, dateFin);
            }
            else if (leEtatExecuter == SessionObject.TauxRecouvrement)
                TauxRecouvrement(LsIdCentre);
            else if (leEtatExecuter == SessionObject.TauxEncaissement)
                TauxEncaissement(LsIdCentre);
            //else if (leEtatExecuter == SessionObject.MontantEncaisseControleur)
            //    MontantRegleAgent(IdCentre, MatriculAgent, dateDebut, dateFin);
            else if (leEtatExecuter == SessionObject.AvanceSurConsomation)
                AvanceSurConsomation(LsIdCentre, dateDebut, dateFin);
            else if (leEtatExecuter == SessionObject.EncaissementReversement)
                EncaissementReversement(LsIdCentre, dateDebut, dateFin);
            else if (leEtatExecuter == SessionObject.EncaissementModeRegement)
                ReturneEncaissementModePaiement(LsIdCentre, dateDebut, dateFin);


            else if (leEtatExecuter == SessionObject.PrepaidSansJamaisAchat)
                ReturneClientAucunAchat(LsIdCentre, dateDebut, dateFin);
            else if (leEtatExecuter == SessionObject.PrepaidSansAchatPeriode)
                ReturneClientSansAchat(LsIdCentre, dateDebut, dateFin);
            else if (leEtatExecuter == SessionObject.TourneePIA)
                ReturneTourneeParPIA(this.btn_Site.Tag.ToString());

        }
        private void AvisEmis(string CodeSite,List<int> IdAgent, DateTime  dateDebut,DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneAvisEmisCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed ;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    if (chk_Recap.IsChecked == true)
                        Utility.ActionExportation<ServicePrintings.CsDetailCampagne, ServiceReport.CsDetailCampagne>(res.Result, null, string.Empty, SessionObject.CheminImpression, "Recouv_AvisEmisRecap", "Report", true, "xlsx");
                    if (chk_Detail.IsChecked == true)
                        Utility.ActionExportation<ServicePrintings.CsDetailCampagne, ServiceReport.CsDetailCampagne>(res.Result, null, string.Empty, SessionObject.CheminImpression, "Recouv_AvisEmisDetail", "Report", true, "xlsx");
                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneAvisEmisAsync(CodeSite, IdAgent, dateDebut, dateFin);
            service1.CloseAsync();
        
        }
        private void AvisCoupe(string CodeSite, List<int> IdAgent, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneAvisCoupeTypeCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    if (chk_Recap.IsChecked == true)
                        Utility.ActionExportation<ServicePrintings.CsDetailCampagne, ServiceReport.CsDetailCampagne>(res.Result, null, string.Empty, SessionObject.CheminImpression, "Recouv_AvisCoupeRecap", "Report", true, "xlsx");
                    if (chk_Detail.IsChecked == true)
                        Utility.ActionExportation<ServicePrintings.CsDetailCampagne, ServiceReport.CsDetailCampagne>(res.Result, null, string.Empty, SessionObject.CheminImpression, "Recouv_AvisCoupeDetail", "Report", true, "xlsx");
                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneAvisCoupeTypeAsync(CodeSite, IdAgent, dateDebut, dateFin);
            service1.CloseAsync();

        }
        private void MontantReglePia(int IdCentre, List<string> Matricule, DateTime dateDebut, DateTime dateFin)
        {
            //Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            //service1.ReturneMontantCompleted += (sr, res) =>
            //{
            //    prgBar.Visibility = System.Windows.Visibility.Collapsed;

            //    if (res != null && res.Cancelled)
            //        return;

            //    if (res.Result != null && res.Result.Count != 0)
            //        Utility.ActionDirectOrientation<ServicePrintings.CsDetailCampagne, ServiceReport.CsDetailCampagne>(res.Result, null, SessionObject.CheminImpression, "MontantRecouvrementPia", "Report", true);
            //    else
            //    {
            //        Message.ShowInformation("Aucune information trouvée", "Report");
            //        return;
            //    }
            //};
            //service1.ReturneMontantAsync(IdCentre, Matricule, dateDebut, dateFin);
            //service1.CloseAsync();

        }
        //private void MontantRegleAgent(int IdCentre, List<string> Matricule, DateTime dateDebut, DateTime dateFin)
        //{
        //    Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
        //    service1.MontantPaiementPreavisCompleted += (sr, res) =>
        //    {
        //        prgBar.Visibility = System.Windows.Visibility.Collapsed;

        //        if (res != null && res.Cancelled)
        //            return;

        //        if (res.Result != null && res.Result.Count != 0)
        //            Utility.ActionDirectOrientation<ServicePrintings.CsDetailCampagne, ServiceReport.CsDetailCampagne>(res.Result, null, SessionObject.CheminImpression, "MontantRecouvrementPia", "Report", true);
        //        else
        //        {
        //            Message.ShowInformation("Aucune information trouvée", "Report");
        //            return;
        //        }
        //    };
        //    service1.MontantPaiementPreavisAsync( Matricule, dateDebut, dateFin);
        //    service1.CloseAsync();

        //}
        private void ClientRemis(string CodeSite, List<int> IdAgent, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneAvisReposeCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    //Utility.ActionDirectOrientation<ServicePrintings.CsDetailCampagne, ServiceReport.CsDetailCampagne>(res.Result, null, SessionObject.CheminImpression, "ClientReposePia", "Report", true);

                    if (chk_Recap.IsChecked == true)
                        Utility.ActionExportation<ServicePrintings.CsDetailCampagne, ServiceReport.CsDetailCampagne>(res.Result, null, string.Empty, SessionObject.CheminImpression, "Recouv_AvisRemisRecap", "Report", true, "xlsx");
                    if (chk_Detail.IsChecked == true)
                        Utility.ActionExportation<ServicePrintings.CsDetailCampagne, ServiceReport.CsDetailCampagne>(res.Result, null, string.Empty, SessionObject.CheminImpression, "Recouv_AvisRemisDetail", "Report", true, "xlsx");
                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneAvisReposeAsync(CodeSite, IdAgent, dateDebut, dateFin);
            service1.CloseAsync();

        }
        private void TauxRecouvrement(List<int> IdCentre)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.RetourneTauxDeRecouvrementCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceReport.CsLclient>(res.Result, null, SessionObject.CheminImpression, "TauxRecouvrement", "Report", true);
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.RetourneTauxDeRecouvrementAsync(IdCentre);
            service1.CloseAsync();

        }
        private void TauxEncaissement(List<int> IdCentre)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.RetourneTauxDeEncaissementCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceReport.CsLclient>(res.Result, null, SessionObject.CheminImpression, "TauxEncaissement", "Report", true);
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.RetourneTauxDeEncaissementAsync(IdCentre);
            service1.CloseAsync();

        }
        private void AvanceSurConsomation(List<int> IdCentre, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneAvanceSurConsoCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceReport.CsLclient>(res.Result, null, SessionObject.CheminImpression, "AvanceSurConsomation", "Report", true);
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneAvanceSurConsoAsync(IdCentre, dateDebut, dateFin);
            service1.CloseAsync();

        }
        private void EncaissementReversement(List<int> IdCentre, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneEncaissementReversementCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceReport.CsLclient>(res.Result, null, SessionObject.CheminImpression, "EncaissementReversement", "Report", true);
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneEncaissementReversementAsync(IdCentre, dateDebut, dateFin);
            service1.CloseAsync();

        }
        private void ReturneEncaissementModePaiement(List<int> IdCentre, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneEncaissementModePaiementCompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceReport.CsLclient>(res.Result, null, SessionObject.CheminImpression, "EncaissementModePaiement", "Report", true);
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneEncaissementModePaiementAsync(IdCentre, dateDebut, dateFin);
            service1.CloseAsync();

        }
        private void ReturneVente(List<int> IdCentre, string  periodeDebut, string  PeriodeFin)
        {
            //Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            //service1.ReturneVenteCompleted += (sr, res) =>
            //{
            //    if (res != null && res.Cancelled)
            //        return;

            //    if (res.Result != null && res.Result.Count != 0)
            //        Utility.ActionDirectOrientation<ServicePrintings.CsRedevanceFacture, ServiceReport.CsRedevanceFacture>(res.Result, null, SessionObject.CheminImpression, "Ventre", "Report", true);
            //    else
            //    {
            //        Message.ShowInformation("Aucune information trouvée", "Report");
            //        return;
            //    }
            //};
            //service1.ReturneVenteAsync(IdCentre, periodeDebut, PeriodeFin);
            //service1.CloseAsync();

        }
        private void ReturneClientSansAchat(List<int> IdCentre, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneClientPrepayeSansAchatPeriodeCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceReport.CsLclient>(res.Result, null, SessionObject.CheminImpression, "ClientSansAchat", "Report", true);
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneClientPrepayeSansAchatPeriodeAsync(IdCentre,dateDebut, dateFin);
            service1.CloseAsync();

        }
        private void ReturneClientAucunAchat(List<int> IdCentre, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneClientPrepayeJamaisAchatCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceReport.CsLclient>(res.Result, null, SessionObject.CheminImpression, "ClientSansAchat", "Report", true);
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneClientPrepayeJamaisAchatAsync(IdCentre,dateDebut,dateFin);
            service1.CloseAsync();

        }
        private void ReturneTourneeParPIA(string CodeSite)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneTourneePIACompleted += (sr, res) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                        Utility.ActionExportation<ServicePrintings.CsTournee, ServiceReport.CsTournee>(res.Result.Where(p=>!string.IsNullOrEmpty( p.NOMRELEVEUR)).ToList(), null, string.Empty, SessionObject.CheminImpression, "Recouv_ListeDestourneePIA", "Report", true, "xlsx");
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneTourneePIAAsync(CodeSite);
            service1.CloseAsync();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        List<Galatee.Silverlight.ServiceRecouvrement.CsUtilisateur> lstPia = new List<Galatee.Silverlight.ServiceRecouvrement.CsUtilisateur>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        Galatee.Silverlight.ServiceAccueil.CsSite lSiteSelect = new Galatee.Silverlight.ServiceAccueil.CsSite();
        List<Galatee.Silverlight.ServiceAccueil.CsProduit> lProduitSelect = new List<Galatee.Silverlight.ServiceAccueil.CsProduit>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                List<int> lstCentreSelect = new List<int>();
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.btn_Site.Tag = lstSite.First().CODE;
                        ChargerPiaAgence(lstSite.First().CODE);
                        this.btn_Site.IsEnabled = false;
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.btn_Centre.IsEnabled = false;
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
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        ChargerPiaAgence(lstSite.First().CODE);
                        this.btn_Site.Tag = lstSite.First().CODE;
                        this.btn_Site.IsEnabled = false;
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.btn_Centre.IsEnabled = false;
                    }
                    return;
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerPiaAgence(string CodeSite)
        {
            try
            {
                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.RetournePIAAgenceCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    lstPia.AddRange(args.Result);
                    return;
                };
                service.RetournePIAAgenceAsync(CodeSite);
                service.CloseAsync();
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
                this.btn_Site .Tag = leSite.CODE ;
                lSiteSelect = leSite;
                ChargerPiaAgence(lSiteSelect.CODE);
                List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).ToList();
                if (lsiteCentre.Count == 1)
                {
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_LibelleCentre.Tag = lsiteCentre.First().PK_ID ;
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
            this.btn_Site.IsEnabled = true;

        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            if (this.Txt_LibelleSite.Tag != null)
            {
                List<ServiceAccueil.CsCentre> lstCentreSite = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).ToList();
                if (lstCentreSite.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> _Listgen = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstCentreSite);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedClient);
                    ctr.Show();
                }

            }
        }
        void galatee_OkClickedClient(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsCentre leCentre = (ServiceAccueil.CsCentre)ctrs.MyObject;
                this.Txt_LibelleCentre.Text = leCentre.LIBELLE;
                this.Txt_LibelleCentre.Tag = leCentre.PK_ID ;
            }
            this.btn_Centre.IsEnabled = true;
        }

        private void btnController_Click(object sender, RoutedEventArgs e)
        {

            if (lstPia != null && lstPia.Count != 0)
            {
                    List<Galatee.Silverlight.ServiceRecouvrement.CsUtilisateur> lstPiaCentre = lstPia;
                    lstPiaCentre.ForEach(t => t.CODE = string.IsNullOrEmpty( t.MATRICULE)? "00000": t.MATRICULE );
                    List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceRecouvrement.CsUtilisateur>(lstPiaCentre);
                    Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, true, "Agent de recouvrement");
                    ctr.Closed += new EventHandler(controller_OkClicked);
                    ctr.Show();
            }
        }
        //private void controller_OkClicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Shared.UcListeParametre generiq = sender as Shared.UcListeParametre;
        //        if (generiq.isOkClick)
        //        {
        //            List<ServiceAccueil.CParametre> ListeCategorie = new List<ServiceAccueil.CParametre>();

        //            if (generiq.MyObjectList.Count != 0)
        //            {
        //                int passage = 1;
        //                foreach (var p in generiq.MyObjectList)
        //                {
        //                    ListeCategorie.Add((ServiceAccueil.CParametre)p);
        //                    if (passage == 1)
        //                        this.Txt_LibelleAgentRecouvrement.Text = p.CODE;
        //                    else
        //                        this.Txt_LibelleAgentRecouvrement.Text = this.Txt_LibelleAgentRecouvrement.Text + "  " + p.CODE;
        //                    passage++;
        //                }
        //                this.Txt_LibelleAgentRecouvrement.Tag = ListeCategorie.Select(t => t.PK_ID).ToList();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex, "Erreur");
        //    }
        //}

        ServiceAccueil.CParametre leParametreSelect = new ServiceAccueil.CParametre();
        private void controller_OkClicked(object sender, EventArgs e)
        {
            try
            {

                Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
                if (ctrs.isOkClick)
                {
                    List<ServiceAccueil.CParametre> _LesControleurSelect =(List<ServiceAccueil.CParametre>) ctrs.MyObjectList ;
                    this.Cbo_AgentPIA.ItemsSource = null ;
                    this.Cbo_AgentPIA.ItemsSource  = _LesControleurSelect ;
                    this.Cbo_AgentPIA.DisplayMemberPath   = "LIBELLE" ;
                    this.Cbo_AgentPIA.Tag = _LesControleurSelect;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        List<Galatee.Silverlight.ServiceRecouvrement.CsTournee> RetoureAgentPia(List<Galatee.Silverlight.ServiceRecouvrement.CsTournee> _lstinit)
        {
            try
            {
                List<Galatee.Silverlight.ServiceRecouvrement.CsTournee> lstTournee = new List<Galatee.Silverlight.ServiceRecouvrement.CsTournee>();
                if (_lstinit.Count > 0)
                {
                    var lstClientFactureDistnct = _lstinit.Select(t => new { t.CENTRE, t.NOMAGENTPIA, t.MATRICULEPIA, t.FK_IDADMUTILISATEUR }).Distinct().ToList();
                    foreach (var item in lstClientFactureDistnct)
                        lstTournee.Add(new Galatee.Silverlight.ServiceRecouvrement.CsTournee { CENTRE = item.CENTRE, NOMAGENTPIA = item.NOMAGENTPIA, MATRICULEPIA = item.MATRICULEPIA, FK_IDADMUTILISATEUR = item.FK_IDADMUTILISATEUR });

                }
                return lstTournee;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}


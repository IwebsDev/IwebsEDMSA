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
    public partial class FrmAvisEmisParControleur : ChildWindow
    {
        public FrmAvisEmisParControleur()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            ChargerListeGestionnaire();
        }
        string leEtatExecuter = string.Empty;

        public FrmAvisEmisParControleur(string typeEtat)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            ChargerListeGestionnaire();
            leEtatExecuter = typeEtat;
            if (leEtatExecuter == SessionObject.TauxPaiementMandat ||
                leEtatExecuter == SessionObject.TauxMandatemant)
            {
                this.dtp_DateDebut.Visibility = System.Windows.Visibility.Collapsed;
                this.dtp_DateFin.Visibility = System.Windows.Visibility.Collapsed;
                lbl_Centre_Copy3.Visibility = System.Windows.Visibility.Collapsed;
                lbl_Centre_Copy4.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (leEtatExecuter == SessionObject.EmissionRegroupement)
            {
                lbl_Centre_Copy1.Visibility = System.Windows.Visibility.Collapsed;
                btn_AgentPia.Visibility = System.Windows.Visibility.Collapsed;
                Txt_LibelleAgentRecouvrement.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                lbl_Centre_Copy.Visibility = System.Windows.Visibility.Collapsed;
                btn_Regroupement.Visibility = System.Windows.Visibility.Collapsed;
                Txt_LibelleRegroupement.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            int IdCentre = 0;
            string  MatriculAgent = string .Empty ;
            DateTime dateDebut = System.DateTime.Today ;
            DateTime dateFin = dateDebut.AddYears(3);
            string Typedemande = string.Empty;
            string Produit = string.Empty;
            List<int> LsIdCentre =new List<int>();

            dateDebut = string.IsNullOrEmpty(this.dtp_DateDebut.Text) ? dateDebut : Convert.ToDateTime(this.dtp_DateDebut.Text);
            dateFin = string.IsNullOrEmpty(this.dtp_DateFin.Text) ? dateFin : Convert.ToDateTime(this.dtp_DateFin.Text);

            if (leEtatExecuter == SessionObject.MontantEncaisseControleur )
              MontantReglePia( MatriculAgent, dateDebut, dateFin);
            if (leEtatExecuter == SessionObject.ListePreavis)
                ListeDePreavis(MatriculAgent, dateDebut, dateFin);
            if (leEtatExecuter == SessionObject.ListeMandatement )
                ListeDesMandatement(MatriculAgent, dateDebut, dateFin);
            if (leEtatExecuter == SessionObject.ListePaiementMandat)
                ListeDesPaiementMandatement(MatriculAgent, dateDebut, dateFin);
            if (leEtatExecuter == SessionObject.TauxMandatemant )
                ListeDesTauxMandatement(MatriculAgent);
            if (leEtatExecuter == SessionObject.TauxPaiementMandat )
                ListeDesTauxPaiementMandatement(MatriculAgent);
        }
        private void MontantReglePia( string Matricule, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.MontantPaiementPreavisCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    if (leEtatExecuter == SessionObject.MontantEncaisseControleur)
                    Utility.ActionDirectOrientation<ServicePrintings.CsDetailCampagne, ServiceReport.CsDetailCampagne>(res.Result, null, SessionObject.CheminImpression, "MontantPaiementGestionnaire", "Report", true);

                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.MontantPaiementPreavisAsync(Matricule, dateDebut, dateFin);
            service1.CloseAsync();

        }
        private void ListeDePreavis( string Matricule, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ListePreavisPreavisCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                {
                    if (leEtatExecuter == SessionObject.ListePreavis)
                        Utility.ActionDirectOrientation<ServicePrintings.CsDetailCampagne, ServiceReport.CsDetailCampagne>(res.Result, null, SessionObject.CheminImpression, "ListeDesPreavisGestionnaire", "Report", true);
                }
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ListePreavisPreavisAsync(Matricule, dateDebut, dateFin);
            service1.CloseAsync();

        }
        private void ListeDesMandatement(string Matricule, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ListeMandatementCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionDirectOrientation<ServicePrintings.CsMandatementGc, ServiceReport.CsMandatementGc>(res.Result, null, SessionObject.CheminImpression, "ListeDesMandatementGestionnaire", "Report", true);
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ListeMandatementAsync(Matricule, dateDebut, dateFin);
            service1.CloseAsync();

        }
        private void ListeDesPaiementMandatement(string Matricule, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ListePaiementMandatementCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionDirectOrientation<ServicePrintings.CsDetailCampagne, ServiceReport.CsDetailCampagne>(res.Result, null, SessionObject.CheminImpression, "ListeDesPaiementMandatGestionnaire", "Report", true);
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ListePaiementMandatementAsync(Matricule, dateDebut, dateFin);
            service1.CloseAsync();

        }
        private void ListeDesTauxMandatement(string Matricule)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.RetourneTauxDeMandatementCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionDirectOrientation<ServicePrintings.CsMandatementGc, ServiceReport.CsMandatementGc>(res.Result, null, SessionObject.CheminImpression, "TauxMandatement", "Report", true);
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.RetourneTauxDeMandatementAsync(Matricule);
            service1.CloseAsync();

        }
        private void ListeDesTauxPaiementMandatement(string Matricule)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.RetourneTauxPaiementCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    Utility.ActionDirectOrientation<ServicePrintings.CsMandatementGc, ServiceReport.CsMandatementGc>(res.Result, null, SessionObject.CheminImpression, "TauxPaiement", "Report", true);
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.RetourneTauxPaiementAsync(Matricule);
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
                        //this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().CODE;
                        //this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        //this.Txt_LibelleCentre.Tag = LstCentrePerimetre.First().PK_ID ;
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
                            //this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                            //this.Txt_LibelleCentre.Tag = LstCentrePerimetre.First().PK_ID;
                            //this.btn_Centre.IsEnabled = false;
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

        List<Galatee.Silverlight.ServiceReport.CsUtilisateur > ListeGestionnaire;
        private void ChargerListeGestionnaire()
        {
            ListeGestionnaire = new List<Galatee.Silverlight.ServiceReport.CsUtilisateur>();
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.RetourneGestionnaireCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                    ListeGestionnaire = res.Result;
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.RetourneGestionnaireAsync();
            service1.CloseAsync();
        }
        private void RemplirCodeRegroupement()
        {
            try
            {
                if (SessionObject.LstCodeRegroupement.Count != 0)
                {
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneCodeRegroupementCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeRegroupement = args.Result;
                        return;
                    };
                    service.RetourneCodeRegroupementAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void btnController_Click(object sender, RoutedEventArgs e)
        {

            if (ListeGestionnaire != null && ListeGestionnaire.Count != 0)
            {
                List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceReport.CsUtilisateur>(ListeGestionnaire);
                Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, false, "Agent de recouvrement");
                ctr.Closed += new EventHandler(controller_OkClicked);
                ctr.Show();
            }
        }
        private void controller_OkClicked(object sender, EventArgs e)
        {
            try
            {

                Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
                if (ctrs.isOkClick)
                {
                    ServiceAccueil.CParametre _LeControleurSelect = ctrs.MyObject as ServiceAccueil.CParametre;
                    this.Txt_LibelleAgentRecouvrement.Text = string.IsNullOrEmpty(_LeControleurSelect.LIBELLE) ? string.Empty : _LeControleurSelect.LIBELLE;
                    this.Txt_LibelleAgentRecouvrement.Tag = _LeControleurSelect.CODE ;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btn_Regroupement_Click(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LstCodeRegroupement != null && SessionObject.LstCodeRegroupement.Count != 0)
            {
                List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsRegCli>(SessionObject.LstCodeRegroupement);
                Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, false, "Agent de recouvrement");
                ctr.Closed += new EventHandler(controller_OkClicked);
                ctr.Show();
            }
        }
        private void Regroupement_OkClicked(object sender, EventArgs e)
        {
            try
            {

                Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
                if (ctrs.isOkClick)
                {
                    ServiceAccueil.CParametre _LeControleurSelect = ctrs.MyObject as ServiceAccueil.CParametre;
                    this.Txt_LibelleRegroupement .Text = string.IsNullOrEmpty(_LeControleurSelect.LIBELLE) ? string.Empty : _LeControleurSelect.LIBELLE;
                    this.Txt_LibelleRegroupement.Tag = _LeControleurSelect.CODE;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
    }
}


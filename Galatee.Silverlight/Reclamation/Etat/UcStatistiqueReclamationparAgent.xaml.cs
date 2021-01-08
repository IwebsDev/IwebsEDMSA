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
    public partial class UcStatistiqueReclamationparAgent : ChildWindow
    {
        public UcStatistiqueReclamationparAgent()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            btn_TypeReclamation.IsEnabled = false;
            ChargerTypeReclamation();
        }
        string LeEtatExecuter = string.Empty;
        public UcStatistiqueReclamationparAgent(string EtatExecuter)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            this.btn_TypeReclamation.IsEnabled = false;
            ChargerTypeReclamation();
            LeEtatExecuter = EtatExecuter;
            if (LeEtatExecuter == SessionObject.StatReclamation ||
                LeEtatExecuter == SessionObject.ReclamationAgent ||
                LeEtatExecuter == SessionObject.TauxReclamation )
            {
                lbl_Centre_Copy1.Visibility = System.Windows.Visibility.Collapsed;
                btn_TypeReclamation.Visibility = System.Windows.Visibility.Collapsed;
                Txt_LibelleTypeDemande.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void ChargerTypeReclamation()
        {
            try
            {
                if (SessionObject.ListeDesReclamation != null && SessionObject.ListeDesReclamation.Count() > 0)
                {
                    btn_TypeReclamation.IsEnabled = true;
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceReclamation.ReclamationsServiceClient client = new Galatee.Silverlight.ServiceReclamation.ReclamationsServiceClient(Utility.Protocole(), Utility.EndPoint("Reclamation"));
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
                            SessionObject.ListeDesReclamation = args.Result;
                            btn_TypeReclamation.IsEnabled = true;

                        }
                    };
                    client.SelectAllTypeReclamationRclAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
  
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            List<int> lstCentre = new List<int>();
            DateTime dateDebut = System.DateTime.Today ;
            DateTime dateFin = dateDebut.AddYears(3);
            string Produit = string.Empty;

            List<int> TypeReclamation = new List<int>();

            if (LeEtatExecuter == SessionObject.ReclamationListe)
            {
                TypeReclamation = Txt_LibelleTypeDemande.Tag != null ? (List<int>)Txt_LibelleTypeDemande.Tag : SessionObject.ListeDesReclamation.Select(c=>c.PK_ID).ToList();
            }
            List<int> lstCategorie = new List<int>();
            List<int> lstTournee = new List<int>();
            if (this.Txt_LibelleCentre.Tag != null)
            {
                int centre;
                 if(int.TryParse( this.Txt_LibelleCentre.Tag.ToString() ,out centre)!=true)
                    lstCentre.AddRange((List<int>)this.Txt_LibelleCentre.Tag);
                 else
                     lstCentre.Add(centre);
                    
            }
            else
                lstCentre.AddRange(LstCentrePerimetre.Where(o => o.CODESITE == this.btn_Site.Tag.ToString()).Select(y => y.PK_ID).ToList());

            dateDebut = string.IsNullOrEmpty(this.dtp_DateDebut.Text) ? dateDebut : Convert.ToDateTime(this.dtp_DateDebut.Text);
            dateFin = string.IsNullOrEmpty(this.dtp_DateFin.Text) ? dateFin : Convert.ToDateTime(this.dtp_DateFin.Text);
            if (LeEtatExecuter == SessionObject.StatReclamation )
            StatistiqueReclamation(lstCentre, dateDebut, dateFin);
            if (LeEtatExecuter == SessionObject.ReclamationAgent )
                ReclamationParAgent(lstCentre, dateDebut, dateFin);
            if (LeEtatExecuter == SessionObject.ReclamationListe )
                ListeDesReclamation(lstCentre, TypeReclamation, dateDebut, dateFin);
            if (LeEtatExecuter == SessionObject.TauxReclamation )
                TauxDetraitement(lstCentre, dateDebut, dateFin);
        }

        private void StatistiqueReclamation(List<int> lstCentre, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReclamation.ReclamationsServiceClient client = new Galatee.Silverlight.ServiceReclamation.ReclamationsServiceClient(Utility.Protocole(), Utility.EndPoint("Reclamation"));
            client.RetourStatistiqueReclamationCompleted += (ssender, args) =>
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
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
                Dictionary<string, string> parame = new Dictionary<string, string>();
                parame.Add("DateDebut", dateDebut.Date.ToShortDateString());
                parame.Add("DateFin", dateFin.Date.ToShortDateString());
                if (args.Result != null)
                    Utility.ActionDirectOrientation<ServicePrintings.cStatistiqueReclamation, Galatee.Silverlight.ServiceReclamation.cStatistiqueReclamation>(args.Result, parame, SessionObject.CheminImpression, "StatistiqueReclamations", "Reclamation", true);

            };
            client.RetourStatistiqueReclamationAsync(dateDebut, dateFin, lstCentre);

        }
        private void  ReclamationParAgent(List<int> lstCentre, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReclamation.ReclamationsServiceClient client = new Galatee.Silverlight.ServiceReclamation.ReclamationsServiceClient(Utility.Protocole(), Utility.EndPoint("Reclamation"));
            client.ReclamationParAgentCompleted += (ssender, args) =>
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
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
                if (args.Result != null)
                    Utility.ActionDirectOrientation<ServicePrintings.CsReclamationRcl, Galatee.Silverlight.ServiceReclamation.CsReclamationRcl>(args.Result, null, SessionObject.CheminImpression, "ReclamationParAgent", "Reclamation", true);

            };
            client.ReclamationParAgentAsync(dateDebut, dateFin, lstCentre);

        }
        private void ListeDesReclamation(List<int> lstCentre,List<int> TypeReclamation, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReclamation.ReclamationsServiceClient client = new Galatee.Silverlight.ServiceReclamation.ReclamationsServiceClient(Utility.Protocole(), Utility.EndPoint("Reclamation"));
            client.ListDesReclamationCompleted += (ssender, args) =>
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
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
                Dictionary<string, string> parame = new Dictionary<string, string>();
                parame.Add("DateDebut", dateDebut.Date.ToShortDateString());
                parame.Add("DateFin", dateFin.Date.ToShortDateString());
                if (args.Result != null)
                    Utility.ActionDirectOrientation<ServicePrintings.CsReclamationRcl, Galatee.Silverlight.ServiceReclamation.CsReclamationRcl>(args.Result,parame , SessionObject.CheminImpression, "ListeReclamations", "Reclamation", true);

            };
            client.ListDesReclamationAsync(dateDebut, dateFin, lstCentre, TypeReclamation);

        }
        private void TauxDetraitement(List<int> lstCentre, DateTime dateDebut, DateTime dateFin)
        {
            Galatee.Silverlight.ServiceReclamation.ReclamationsServiceClient client = new Galatee.Silverlight.ServiceReclamation.ReclamationsServiceClient(Utility.Protocole(), Utility.EndPoint("Reclamation"));
            client.SuiviTauxTraitementCompleted += (ssender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    string error = args.Error.Message;
                    Message.ShowError(error, "");
                    return;
                }
                if (args.Result == null)
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
                Dictionary<string, string> parame = new Dictionary<string, string>();
                parame.Add("DateDebut", dateDebut.Date.ToShortDateString());
                parame.Add("DateFin", dateFin.Date.ToShortDateString());
                if (args.Result != null)
                    Utility.ActionDirectOrientation<ServicePrintings.cStatistiqueReclamation, Galatee.Silverlight.ServiceReclamation.cStatistiqueReclamation>(args.Result, parame, SessionObject.CheminImpression, "SuiviTauxTraitement", "Reclamation", true);

            };
            client.SuiviTauxTraitementAsync(dateDebut, dateFin, lstCentre);

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        List<int> lesCentreCaisse = new List<int>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        Galatee.Silverlight.ServiceAccueil.CsSite lSiteSelect = new Galatee.Silverlight.ServiceAccueil.CsSite();
        Galatee.Silverlight.ServiceReclamation.CsTypeReclamationRcl lReclamationRclSelect = new Galatee.Silverlight.ServiceReclamation.CsTypeReclamationRcl();
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
                        this.Txt_LibelleCentre.Tag = LstCentrePerimetre.First().PK_ID ;
                       
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                Message.ShowError(ex.Message, "Report");
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
                btn_Site.Tag = leSite.CODE;
                lSiteSelect = leSite;
                List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).ToList();
                if (lsiteCentre.Count == 1)
                {
                    List<int> lstIdCentreSelect = new List<int>();
                    lstIdCentreSelect.Add(lsiteCentre.First().PK_ID);
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_LibelleCentre.Tag = lstIdCentreSelect;
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
            try
            {
                if (LstCentrePerimetre.Count != 0 && btn_Site.Tag!=null)
                {
                    List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsCentre>(LstCentrePerimetre.Where(c=>c.CODESITE==btn_Site.Tag.ToString()).ToList());
                    Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, true, "Centre");
                    ctr.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctr.Show();
                }
                else
                {
                    Message.ShowError("Veillez vous ssurer que qu'un site est sélectionné ou que vous ete habilité sur des centres", "Erreur");
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
                        this.Txt_LibelleCentre.Text = this.Txt_LibelleCentre.Text+" "+item + " ";
                    this.Txt_LibelleCentre.Tag = _LesCentreSelect.Select(t => t.PK_ID).ToList();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btn_TypeReclamation_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SessionObject.ListeDesReclamation.Count > 0)
                {
                    this.btn_TypeReclamation.IsEnabled = false;
                    List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<Galatee.Silverlight.ServiceReclamation.CsTypeReclamationRcl>(SessionObject.ListeDesReclamation);
                    Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, true, "Type Reclamation");
                    ctr.Closed += new EventHandler(galatee_OkClickedTypeReclamation);
                    ctr.Show();



                    //this.btn_TypeReclamation.IsEnabled = false;
                    //List<object> _Listgen = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.ListeDesReclamation);
                    //Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                    //ctr.Closed += new EventHandler(galatee_OkClickedTypeReclamation);
                    //ctr.Show();
                }
                else
                {
                    Message.ShowError("La liste est vide", "Report");
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Report");
            }
        }

        private void galatee_OkClickedTypeReclamation(object sender, EventArgs e)
        {
            this.btn_TypeReclamation.IsEnabled = true;
            Galatee.Silverlight.Shared.UcListeParametre ctrs = sender as Galatee.Silverlight.Shared.UcListeParametre;
            if (ctrs.isOkClick)
            {
                List<ServiceAccueil.CParametre> _LesCentreSelect = ctrs.MyObjectList as List<ServiceAccueil.CParametre>;
                List<string> lstCentre = _LesCentreSelect.Select(t => t.CODE).ToList();
                foreach (string item in lstCentre)
                    this.Txt_LibelleTypeDemande.Text =this.Txt_LibelleTypeDemande.Text+" "+ item + " ";
                this.Txt_LibelleTypeDemande.Tag = _LesCentreSelect.Select(t => t.PK_ID).ToList();



                //ServiceAccueil.CParametre leSite = (ServiceAccueil.CParametre)ctrs.MyObjectList;
                //this.Txt_LibelleTypeDemande.Text = leSite.LIBELLE;
                //this.Txt_LibelleTypeDemande.Tag = leSite.PK_ID;
                //btn_TypeReclamation.Tag = leSite.CODE;
                //lReclamationRclSelect =SessionObject.ListeDesReclamation.FirstOrDefault(c=>c.Code== leSite.CODE);
                //List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).ToList();
                //if (leSite !=null)
                //{
                //    List<int> lstIdCentreSelect = new List<int>();
                //    //lstIdCentreSelect.Add(lsiteCentre.First().PK_ID);
                //    this.Txt_LibelleTypeDemande.Text = leSite.LIBELLE;
                //    this.Txt_LibelleTypeDemande.Tag = leSite;
                //    this.btn_TypeReclamation.IsEnabled = true;
                //}
                //else
                //{
                //    this.Txt_LibelleCentre.Text = string.Empty;
                //    this.Txt_LibelleCentre.Tag = null;
                //    this.btn_Centre.IsEnabled = true;
                //}
            }
            //this.btn_Site.IsEnabled = true;
        }
    }
}


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
using Galatee.Silverlight.ServiceRecouvrement;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Caisse;
using Galatee.Silverlight.MainView;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmAvisDeCoupure : ChildWindow
    {
        public FrmAvisDeCoupure()
        {
            InitializeComponent();
            InitialiseCtrl();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_Periode.IsReadOnly = true;
            this.Txt_MontantPeriode.IsReadOnly = true;
            this.Txt_MontantGlobal.IsReadOnly = true;
            this.Txt_NombreFactureMontant .IsReadOnly = true;
            this.Txt_MontantFacture.IsReadOnly = true;
            this.rdbtDetail.IsChecked = true;
            this.Chk_ExclusResil.IsChecked = true;
        }

        List<Galatee.Silverlight.ServiceAccueil.CsCategorieClient> ListeCategorie = new List<Galatee.Silverlight.ServiceAccueil.CsCategorieClient>();
        List<Galatee.Silverlight.ServiceAccueil.CsCodeConsomateur> ListeCodeConsomateur = new List<Galatee.Silverlight.ServiceAccueil.CsCodeConsomateur>();
        List<CsAvisDeCoupureClient> ligne = new List<CsAvisDeCoupureClient>();
        private void   OKButton_Click(object sender, RoutedEventArgs e)
        {
            string IdCoupure = string.Empty;
             try
             {
                 CsAvisCoupureEdition avisCoupure = new CsAvisCoupureEdition();
               bntPrinting.IsEnabled = false;
               aCampagne Campagne = new aCampagne();
               aDisconnection dis = new aDisconnection();

               avisCoupure.ClientGroupe = this.Chk_ExclutRegr.IsChecked;
               avisCoupure.ClientResilie = this.Chk_ExclusResil.IsChecked;
               avisCoupure.Matricule = UserConnecte.matricule;
               string DueDate = (string.IsNullOrEmpty(this.dtpDate.Text)) ? null : dtpDate.Text;

               avisCoupure.Exigible = this.dtpDate.SelectedDate;
               avisCoupure.idCentre = (this.Txt_Centre.Tag == null ? 0 : (int)this.Txt_Centre.Tag);
               avisCoupure.Centre = (this.Txt_Centre.Tag == null ? string.Empty : this.Txt_Centre.Text);
               avisCoupure.Site  = this.Txt_Site .Text ;
             
               if (this.Rdb_facturePeriode.IsChecked == true)
               {
                   if (string.IsNullOrEmpty(this.Txt_MontantPeriode.Text))
                   {
                       Message.ShowWarning("Remplisez le montant èxigible de la période", "Avis de coupure");
                       LayoutRoot.Cursor = Cursors.Arrow ;
                       this.bntPrinting.IsEnabled = true;
                       return;
                   }
                   if (string.IsNullOrEmpty(this.Txt_Periode.Text))
                   {
                       Message.ShowWarning("Remplisez la période èxigible", "Avis de coupure");
                       this.bntPrinting.IsEnabled = true;
                       return;
                   }
                   avisCoupure.Periode  = this.Txt_Periode.Text;
                   avisCoupure.SoldeMinimum   = string.IsNullOrEmpty(this.Txt_MontantPeriode.Text) ? 0 : int.Parse(this.Txt_MontantPeriode.Text);
               }
               else if (this.Rdb_MontantGlobal.IsChecked == true)
               {
                   if (string.IsNullOrEmpty(this.Txt_MontantGlobal.Text))
                   {
                       Message.ShowWarning("Remplisez le montant èxigible", "Avis de coupure");
                       return;
                   }
                   avisCoupure.SoldeMinimum = System.Convert.ToDecimal(string.IsNullOrEmpty(this.Txt_MontantGlobal.Text) ? "0" : this.Txt_MontantGlobal.Text);
               }
               else if (this.Rdb_NombreFactureMontant.IsChecked == true)
               {
                   if (string.IsNullOrEmpty(this.Txt_MontantFacture.Text))
                   {
                       Message.ShowWarning("Remplisez le montant èxigible", "Avis de coupure");
                       return;
                   }
                   if (string.IsNullOrEmpty(this.Txt_NombreFactureMontant.Text))
                   {
                       Message.ShowWarning("Remplisez le nombre de facture èxigible", "Avis de coupure");
                       return;
                   }
                   avisCoupure.SoldeMinimum = string.IsNullOrEmpty(this.Txt_MontantFacture.Text) ? 0 : System.Convert.ToDecimal(this.Txt_MontantFacture.Text);
                   avisCoupure.NombreFactureTotalClient    = string.IsNullOrEmpty(this.Txt_NombreFactureMontant.Text) ? 0 : int.Parse(this.Txt_NombreFactureMontant.Text);
               }
               avisCoupure.NombreTotalDeClient = TryToParse(txtnbreSubscr.Text);
               avisCoupure.AgentPia = Txt_AgentRecourvrement.Text;
               avisCoupure.NomAgentPia = Txt_LibelleAgentRecouvrement .Text;
                if (string.IsNullOrEmpty(avisCoupure.MatriculeDebut)  && !string.IsNullOrEmpty(avisCoupure.MatriculeFin))
                {
                    Message.ShowInformation("Entrez la première référence !", Galatee.Silverlight.Resources.Langue.errorTitle);
                    return ;
                }
                if (!string.IsNullOrEmpty(avisCoupure.MatriculeDebut) && string.IsNullOrEmpty(avisCoupure.MatriculeFin))
                {
                    Message.ShowInformation("Entrez la deuxième référence!", Galatee.Silverlight.Resources.Langue.errorTitle);
                    return ;
                }

                Campagne.FirstRouteNumber = avisCoupure.MatriculeDebut;
                Campagne.LastRouteNumber = avisCoupure.MatriculeFin;
                avisCoupure.Categories = new List<int>();
                avisCoupure.Tournees = new List<int>();
                avisCoupure.Consomateur = new List<int>();
                if (this.txt_CodeCategorie .Tag != null)
                {
                    avisCoupure.CategorieDebut = ((List<ServiceAccueil.CParametre>)this.txt_CodeCategorie.Tag).First().CODE;
                    avisCoupure.CategorieFin = ((List<ServiceAccueil.CParametre>)this.txt_CodeCategorie.Tag).Last().CODE;
                    foreach (Galatee.Silverlight.ServiceAccueil.CParametre item in (List<Galatee.Silverlight.ServiceAccueil.CParametre>)this.txt_CodeCategorie.Tag)
                        avisCoupure.Categories.Add(item.PK_ID);
                }
                if (this.txt_codetourne  .Tag != null)
                {
                    avisCoupure.TourneeDebut = ((List<ServiceAccueil.CParametre>)this.txt_codetourne.Tag).First().CODE;
                    avisCoupure.TourneeFin = ((List<ServiceAccueil.CParametre>)this.txt_codetourne.Tag).Last().CODE;
                    foreach (ServiceAccueil.CParametre item in (List<ServiceAccueil.CParametre>)this.txt_codetourne.Tag)
                        avisCoupure.Tournees.Add(item.PK_ID);
                }
                if (this.txt_CodeConsomateur .Tag != null)
                {
                    foreach (Galatee.Silverlight.ServiceAccueil.CParametre item in (List<Galatee.Silverlight.ServiceAccueil.CParametre>)this.txt_CodeConsomateur.Tag)
                        avisCoupure.Consomateur.Add(item.PK_ID);
                }
                bool IsListe = false;
                ligne.Clear();
                if (this.rdbtnList.IsChecked == true)
                    IsListe = true;
                else
                    IsListe = false;
                        prgBar.Visibility = System.Windows.Visibility.Visible ;
                        RecouvrementServiceClient proxy = new RecouvrementServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Recouvrement"));
                        proxy.TraitementAvisCoupureAsync(avisCoupure, dis, IsListe);
                        proxy.TraitementAvisCoupureCompleted += (ssn, results) =>
                            {
                                try
                                {
                                    if (results.Cancelled || results.Error != null)
                                    {
                                        string error = results.Error.Message;
                                        Message.ShowError("Erreur d'invocation du service. !", Galatee.Silverlight.Resources.Langue.errorTitle);
                                        bntPrinting.IsEnabled = true;
                                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                        return;
                                    }

                                    if (results.Result == null || results.Result.Count == 0)
                                    {
                                        Message.ShowError("Aucune donnée de campagne retournée!", Galatee.Silverlight.Resources.Langue.errorTitle);
                                        bntPrinting.IsEnabled = true;
                                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                        return;
                                    }

                                    ligne.Clear();
                                    ligne.AddRange(results.Result);

                                    Dictionary<string, string> param = new Dictionary<string, string>();
                                    param.Add("pfirstzone", ((List<ServiceAccueil.CParametre>)this.txt_codetourne .Tag).First().CODE);
                                    param.Add("plastzone", ((List<ServiceAccueil.CParametre>)this.txt_codetourne.Tag).Last().CODE);
                                    param.Add("pminiAmount", avisCoupure.MontantPeriode.ToString());
                                    param.Add("pduedate", avisCoupure.Exigible.Value.ToShortDateString());
                                    param.Add("pnombreCustomer", avisCoupure.NombreTotalDeClient .ToString());
                                    //param.Add("pcampainNumber", results.Result.First().i);
                                    param.Add("pperioddue", avisCoupure.Periode);
                                    param.Add("pnumberbill", avisCoupure.NombreFactureTotalClient .ToString());
                                    param.Add("pfirstroute", avisCoupure.MatriculeDebut);
                                    param.Add("plastroute", avisCoupure.MatriculeFin);
                                    param.Add("pfirstcategorie", ((List<Galatee.Silverlight.ServiceAccueil.CParametre>)this.txt_CodeCategorie .Tag).First().CODE);
                                    param.Add("plastcategorie", ((List<Galatee.Silverlight.ServiceAccueil.CParametre>)this.txt_CodeCategorie.Tag).Last().CODE);
                                    param.Add("pnamecontroller", avisCoupure.NomAgentPia );
                                    string key = Utility.getKey();

                                    if (IsListe == false)
                                        Utility.ActionDirectOrientation<ServicePrintings.CsAvisDeCoupureClient, ServiceRecouvrement.CsAvisDeCoupureClient>(ligne, null, SessionObject.CheminImpression, "AvisDeCoupureDetail", "Recouvrement", true);
                                    else
                                    {
                                        Utility.ActionDirectOrientation<ServicePrintings.CsAvisDeCoupureClient, ServiceRecouvrement.CsAvisDeCoupureClient>(ligne, param, SessionObject.CheminImpression, "AvisDeCoupureListe", "Recouvrement", true);
                                    }
                                    
                                    prgBar.Visibility = System.Windows.Visibility.Collapsed  ;
                                    bntPrinting.IsEnabled = true;
                                }
                                catch (Exception ex)
                                {
                                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                    bntPrinting.IsEnabled = true;
                                    Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                                }
                            };
            }
            catch (Exception ex)
            {
                bntPrinting.IsEnabled = true;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);

            }
        }
        private static int? TryToParse(string value)
        {
            int number;
            bool result = Int32.TryParse(value, out number);
            if (result)
            {
                return number;
            }
            else
            {
                return null;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnController_Click(object sender, RoutedEventArgs e)
        {

            if (ListeTourneepIA != null && ListeTourneepIA.Count != 0)
            { 
                if(this.Txt_Centre .Tag != null )
                {
                  
                    List< CsTournee> lstPiaCentre = ListeTourneepIA.Where(t => t.FK_IDCENTRE == (int)this.Txt_Centre.Tag && !string.IsNullOrEmpty(t.MATRICULEPIA)).ToList();
                    List<CsTournee> lstAgentPIA = RetoureAgentPia(lstPiaCentre);

                    lstAgentPIA.ForEach(t => t.LIBELLE = t.NOMAGENTPIA);
                    lstAgentPIA.ForEach(t => t.CODE = t.MATRICULEPIA);
                    List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceRecouvrement.CsTournee>(lstAgentPIA);
                    Shared.UcListeParametre ctr = new UcListeParametre(lstParametre, false, "Agent de recouvrement");
                    ctr.Closed += new EventHandler(controller_OkClicked);
                    ctr.Show();
                }
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
                    this.Txt_AgentRecourvrement.Text = string.IsNullOrEmpty(_LeControleurSelect.CODE) ? string.Empty : _LeControleurSelect.CODE;
                    this.Txt_LibelleAgentRecouvrement.Text = string.IsNullOrEmpty(_LeControleurSelect.LIBELLE) ? string.Empty : _LeControleurSelect.LIBELLE;
                    this.Txt_AgentRecourvrement.Tag = ListeTourneepIA.FirstOrDefault(t => t.MATRICULEPIA == _LeControleurSelect.CODE); ;
                    ListeTourneepIASelect = ListeTourneepIA.Where(t => t.FK_IDCENTRE == (int)this.Txt_Centre.Tag && t.NOMAGENTPIA == _LeControleurSelect.LIBELLE).ToList();
                    if (ListeTourneepIASelect != null && ListeTourneepIASelect.Count != 0)
                        this.btnzone.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        List<CsTournee> RetoureAgentPia(List<CsTournee> _lstinit)
        {
            try
            {
                List<CsTournee> lstTournee = new List<CsTournee>();
                if (_lstinit.Count > 0)
                {
                    var lstClientFactureDistnct = _lstinit.Select(t => new { t.CENTRE, t.NOMAGENTPIA, t.MATRICULEPIA, t.FK_IDADMUTILISATEUR }).Distinct().ToList();
                    foreach (var item in lstClientFactureDistnct)
                        lstTournee.Add(new CsTournee { CENTRE = item.CENTRE, NOMAGENTPIA = item.NOMAGENTPIA, MATRICULEPIA = item.MATRICULEPIA, FK_IDADMUTILISATEUR = item.FK_IDADMUTILISATEUR });

                }
                return lstTournee;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        void InitialiseCtrl()
        {
            ChargerCentre ();
            LoadCategorie();
            LoadCodeConsomateur();
            this.dtpDate.SelectedDate = System.DateTime.Today;
        }
        private void ChargerCategorie()
        {
            try
            {
                    int handler___ = LoadingManager.BeginLoading("Recuperation des Catégories ...");
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneCategorieCompleted += (s, args) =>
                    {
                        try
                        {
                            if (args != null && args.Cancelled)
                                return;
                            SessionObject.LstCategorie = args.Result;
                            ListeCategorie = SessionObject.LstCategorie;
                            //Utility.ConvertListType<CsCategorieClient, Galatee.Silverlight.ServiceAccueil.CsCategorieClient>(SessionObject.LstCategorie); 
                            if (args.Cancelled || args.Error != null)
                            {
                                Message.ShowError("Erreur d'invocation du service. Réessayer svp! ", Galatee.Silverlight.Resources.Langue.wcf_error);
                                LoadingManager.EndLoading(handler___);
                                return;
                            }

                            if (args.Result == null | args.Result.Count == 0)
                            {
                                Message.ShowInformation("Aucune donnée retournée du service.Réessayer svp! ", Galatee.Silverlight.Resources.Langue.errorTitle);
                                LoadingManager.EndLoading(handler___);
                                return;
                            }

                            btnCategorie.IsEnabled = true;
                            LoadingManager.EndLoading(handler___);
                        }
                        catch (Exception e)
                        {
                            LoadingManager.EndLoading(handler___);
                            throw e;
                        }
                    };
                    service.RetourneCategorieAsync();
                    service.CloseAsync();
            }
            catch (Exception ex)
            {
               

                throw ex;
            }
        }
        //private void ChargerTournee()
        //{
        //    int handler = LoadingManager.BeginLoading("Recuperation des tournées ...");
        //    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //    service.ChargerLesTourneesCompleted  += (s, args) =>
        //    {
        //        if (args != null && args.Cancelled)
        //            return;
        //        SessionObject.LstZone = args.Result;
        //        if (args.Cancelled || args.Error != null)
        //        {
        //            Message.ShowInformation("Erreur d'invocation du service !", Galatee.Silverlight.Resources.Langue.wcf_error);
        //            LoadingManager.EndLoading(handler);
        //            return;
        //        }
        //        if (args.Result == null | args.Result.Count == 0)
        //        {
        //            Message.ShowInformation("Aucune donnée retournée du système ! ", Galatee.Silverlight.Resources.Langue.errorTitle);
        //            LoadingManager.EndLoading(handler);
        //            return;
        //        }
        //        btnzone.IsEnabled = true ;
        //        btnController .IsEnabled = true ;
        //        LoadingManager.EndLoading(handler);
        //    };
        //    service.ChargerLesTourneesAsync ();
        //    service.CloseAsync();
        //}
        private void ChargerCodeConso()
        {
            try
            {
                int handler___ = LoadingManager.BeginLoading("Récupération des codes consommateur...");
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCodeConsomateurCompleted  += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeConsomateur  = args.Result;
                        //ListeCodeConsomateur= Utility.ConvertListType<CsCodeConsomateur , Galatee.Silverlight.ServiceAccueil.CsCodeConsomateur >(SessionObject.LstCodeConsomateur);
                        ListeCodeConsomateur = SessionObject.LstCodeConsomateur;
                        btn_CodeConso.IsEnabled = true;

                        if (args.Cancelled || args.Error != null)
                        {
                            Message.ShowError("Erreur d'invocation du service. Réessayer svp! ", Galatee.Silverlight.Resources.Langue.wcf_error);
                            LoadingManager.EndLoading(handler___);
                            return;
                        }

                        if (args.Result == null | args.Result.Count == 0)
                        {
                            Message.ShowInformation("Aucune donnée retournée du service. Réessayer svp! ", Galatee.Silverlight.Resources.Langue.errorTitle);
                            LoadingManager.EndLoading(handler___);
                            return;
                        }

                        LoadingManager.EndLoading(handler___);
                    }
                    catch (Exception e)
                    {
                        LoadingManager.EndLoading(handler___);
                        throw e;
                    }
                };
                service.RetourneCodeConsomateurAsync ();
                service.CloseAsync();
            }
            catch (Exception ex)
            {


                throw ex;
            }
        }

        private void LoadZone(List<int> lstidCentre)
        {
            //if (SessionObject.LstZone.Count > 0)
            //{
            //    ListeDesZone = SessionObject.LstZone;
            //    btnzone.IsEnabled = true  ;
            //    btnController.IsEnabled = true  ;
            //}
            //else
            ChargerListeTourneePIA(lstidCentre);
        }

        private void LoadCategorie()
        {
            if (SessionObject.LstCategorie.Count > 0)
            {
                ListeCategorie = SessionObject.LstCategorie;
                btnCategorie.IsEnabled = true;
            }
            else
                ChargerCategorie();
        }
        private void LoadCodeConsomateur()
        {
            if (SessionObject.LstCodeConsomateur .Count > 0)
            {
                //ListeCodeConsomateur = Utility.ConvertListType<CsCodeConsomateur, Galatee.Silverlight.ServiceAccueil.CsCodeConsomateur>(SessionObject.LstCodeConsomateur);
                ListeCodeConsomateur = SessionObject.LstCodeConsomateur;
                btn_CodeConso.IsEnabled = true;
            }
            else
                ChargerCodeConso();
        }
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<Galatee.Silverlight.ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        private void ChargerCentre()
        {
            try
            {
                List<int> lstIdCentre = new List<int>();

                if (SessionObject.LstCentre.Count != 0)
                {
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lstIdCentre.Add(item.PK_ID);
                    LoadZone(lstIdCentre);

                    if (lstSite != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_Site.Text = _LstSite[0].CODE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.Txt_Site.IsReadOnly = true;
                        }
                        this.btnSite.IsEnabled = true ;
                        this.btnCentre.IsEnabled = true ;
                    }
                    if (LstCentre != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsCentre> _LstCentre = LstCentre.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstCentre.Count == 1)
                        {
                            this.Txt_Centre.Text = _LstCentre[0].CODE;
                            this.Txt_LibelleCentre .Text = _LstCentre[0].LIBELLE;
                            this.Txt_Centre.Tag = _LstCentre[0].PK_ID;
                            this.Txt_Centre.IsReadOnly = true;
                        }
                        this.btnSite.IsEnabled = true;
                        this.btnCentre.IsEnabled = true;
                    }
                    return;
                }
                //Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new AccesServiceWCF().GetAcceuilClient();
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lstIdCentre.Add(item.PK_ID);
                    LoadZone(lstIdCentre);
                    if (lstSite != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_Site.Text = _LstSite[0].CODE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.Txt_Site.IsReadOnly = true;
                        }
                        this.btnSite.IsEnabled = true;
                        this.btnCentre.IsEnabled = true;
                    }
                    if (LstCentre != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsCentre> _LstCentre = LstCentre.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstCentre.Count == 1)
                        {
                            this.Txt_Centre.Text = _LstCentre[0].CODE;
                            this.Txt_LibelleCentre.Text = _LstCentre[0].LIBELLE;
                            this.Txt_Centre.IsReadOnly = true;
                            this.Txt_Centre.Tag = _LstCentre[0].PK_ID;
                        }
                        this.btnSite.IsEnabled = true;
                        this.btnCentre.IsEnabled = true;
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false );
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "LoadCentre");

            }
        }
        private void btnCentre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentre != null)
                {
                        Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                        _LstColonneAffich.Add("CODE", "CENTRE");
                        _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                        List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstCentre);
                        MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Liste de centre");
                        ctrl.Closed += new EventHandler(centres_OkClicked);
                        ctrl.Show();

                    }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void centres_OkClicked(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsCentre _LeCentreSelect = ctrs.MyObject as Galatee.Silverlight.ServiceAccueil.CsCentre;
                    this.Txt_Centre .Text = string.IsNullOrEmpty(_LeCentreSelect.CODE) ? string.Empty : _LeCentreSelect.CODE;
                    this.Txt_LibelleCentre .Text = string.IsNullOrEmpty(_LeCentreSelect.LIBELLE ) ? string.Empty : _LeCentreSelect.LIBELLE ;
                    this.Txt_Centre.Tag = _LeCentreSelect.PK_ID;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        List<CsTournee> ListeTourneepIA;
        List<CsTournee> ListeTourneepIASelect = new List<CsTournee>();
        private void ChargerListeTourneePIA(List<int> lstidCentre)
        {
            ListeTourneepIA = new List<CsTournee>();
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RetourneTourneePIAAsync(lstidCentre);
            service.RetourneTourneePIACompleted += (s, args) =>
            {
                try
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        Message.ShowError("Erreur survenue à l'appel du service.", "Erreur");
                        return;
                    }

                    if (args.Result == null || args.Result.Count == 0)
                    {
                        Message.ShowError("Aucune tournée retournée par le système.", "Info");
                        return;
                    }
                    ListeTourneepIA = args.Result;
                    btnController.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }
            };
        }

        private void btnSite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite != null)
                {
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "SITE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite.Where(t => t.CODE != "000").ToList());
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Liste de site");
                    ctrl.Closed += new EventHandler(Site_OkClicked);
                    ctrl.Show();


                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void Site_OkClicked(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsSite _LeSiteSelect = ctrs.MyObject as Galatee.Silverlight.ServiceAccueil.CsSite;
                    this.Txt_Site.Text = string.IsNullOrEmpty(_LeSiteSelect.CODE) ? string.Empty : _LeSiteSelect.CODE;
                    this.Txt_LibelleSite .Text = string.IsNullOrEmpty(_LeSiteSelect.LIBELLE) ? string.Empty : _LeSiteSelect.LIBELLE;
                    LstCentre = SessionObject.LstCentre.Where(t => t.FK_IDCODESITE == _LeSiteSelect.PK_ID).ToList();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btnzone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListeTourneepIASelect == null || ListeTourneepIASelect.Count == 0)
                    return;
                List<CsTournee> lstTournePia = ListeTourneepIASelect.Where(t => t.FK_IDCENTRE == (int)this.Txt_Centre.Tag && t.FK_IDADMUTILISATEUR == ((CsTournee)this.Txt_AgentRecourvrement.Tag).FK_IDADMUTILISATEUR).ToList();
                List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceRecouvrement.CsTournee>(lstTournePia);
                Shared.UcListeParametre ctr = new UcListeParametre(lstParametre, true, "Tournée");
                ctr.Closed += new EventHandler(zones_OkClicked);
                ctr.Show();

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }

        private void zones_OkClicked(object sender, EventArgs e)
        {
            try
            {
                Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
                if (ctrs.isOkClick)
                {
                    List<ServiceAccueil.CParametre> ListeDesZones = new List<ServiceAccueil.CParametre>();
                    if (ctrs.IsMultiselect)
                    {
                        int passage = 1 ;
                        foreach (var p in ctrs.MyObjectList)
                        {
                            ListeDesZones.Add((ServiceAccueil.CParametre)p);
                            if (passage == 1 )
                                this.txt_codetourne.Text =  p.CODE;
                            else
                                this.txt_codetourne.Text = this.txt_codetourne.Text + "  " + p.CODE;
                            passage++;
                        }
                        this.txt_codetourne.Tag = ListeDesZones;
                        if (ListeDesZones.Count != 0 && ListeDesZones.Count >1)
                        {
                            this.txt_OrdreTourneeDebut.IsReadOnly = true;
                            this.txt_OrdreTourneeFin.IsReadOnly = true;
                        }
                        else if (ListeDesZones.Count != 0 && ListeDesZones.Count == 1)
                        {
                            this.txt_OrdreTourneeDebut.IsReadOnly = false ;
                            this.txt_OrdreTourneeFin.IsReadOnly = false ;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btnCategorie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsCategorieClient>(ListeCategorie);
                Shared.UcListeParametre ctr = new UcListeParametre(lstParametre, true, "Liste de catégorie");
                ctr.Closed += new EventHandler(categorie_OkClicked);
                ctr.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }

        private void categorie_OkClicked(object sender, EventArgs e)
        {
            try
            {
                Shared.UcListeParametre generiq = sender as Shared.UcListeParametre;
                if (generiq.isOkClick)
                {
                    List<ServiceAccueil.CParametre> ListeCategorie = new List<ServiceAccueil.CParametre>();
                  
                        if (generiq.MyObjectList.Count != 0)
                        {
                            int passage = 1;
                            foreach (var p in generiq.MyObjectList)
                            {
                                ListeCategorie.Add((ServiceAccueil.CParametre)p);
                                if (passage == 1 )
                                this.txt_CodeCategorie.Text = p.CODE;
                                else
                                    this.txt_CodeCategorie.Text = this.txt_CodeCategorie.Text + "  " + p.CODE;
                                passage++;
                            }
                             this.txt_CodeCategorie.Tag = ListeCategorie;
                        }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }


        private void btnCodeConso_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsCodeConsomateur >(ListeCodeConsomateur);
                Shared.UcListeParametre ctr = new UcListeParametre(lstParametre, true, "Liste de code consommateur");
                ctr.Closed += new EventHandler(CodeConso_OkClicked);
                ctr.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }

        private void CodeConso_OkClicked(object sender, EventArgs e)
        {
            try
            {
                Shared.UcListeParametre generiq = sender as Shared.UcListeParametre;
                if (generiq.isOkClick)
                {
                  
                    if (generiq.IsMultiselect)
                    {
                        if (generiq.MyObjectList.Count != 0)
                        {
                            int passage = 1;
                            List<ServiceAccueil.CParametre> lstCodeConsomateur = new List<ServiceAccueil.CParametre>();
                            foreach (var p in generiq.MyObjectList)
                            {
                                lstCodeConsomateur.Add((Galatee.Silverlight.ServiceAccueil.CParametre)p);
                                if (passage == 1 )
                                    this.txt_CodeConsomateur.Text =  p.CODE;
                                else 
                                this.txt_CodeConsomateur.Text = this.txt_CodeConsomateur.Text + "  " + p.CODE;
                                passage++;
                            }
                            this.txt_CodeConsomateur.Tag = lstCodeConsomateur;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void rdbtnList_Checked_1(object sender, RoutedEventArgs e)
        {
            if (this.rdbtDetail !=  null && this.rdbtDetail.IsChecked == true )
            this.rdbtDetail.IsChecked = false;
        }

        private void rdbtDetail_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rdbtnList != null &&   this.rdbtnList.IsChecked == true )
            this.rdbtnList.IsChecked = false;

        }

        private void Rdb_facturePeriode_Checked_1(object sender, RoutedEventArgs e)
        {
            this.Txt_MontantPeriode.IsReadOnly = false;
            this.Txt_Periode.IsReadOnly = false;

            this.Rdb_MontantGlobal.IsChecked = false;
            this.Rdb_NombreFactureMontant.IsChecked = false;
        }

        private void Rdb_facturePeriode_Unchecked_1(object sender, RoutedEventArgs e)
        {
            this.Txt_MontantPeriode.IsReadOnly = true;
            this.Txt_MontantPeriode.Text = string.Empty;
            this.Txt_Periode.Text = string.Empty;
            this.Txt_Periode.IsReadOnly = true;
        }

        private void Rdb_MontantGlobal_Checked_1(object sender, RoutedEventArgs e)
        {
            this.Txt_MontantGlobal.IsReadOnly = false;
            this.Rdb_facturePeriode.IsChecked = false;
            this.Rdb_NombreFactureMontant.IsChecked = false;
            this.Txt_MontantGlobal.Focus();
        }

        private void Rdb_NombreFactureMontant_Checked_1(object sender, RoutedEventArgs e)
        {
            this.Txt_NombreFactureMontant.IsReadOnly = false;
            this.Txt_MontantFacture.IsReadOnly = false;
            this.Txt_NombreFactureMontant.Focus();


            this.Rdb_facturePeriode.IsChecked = false;
            this.Rdb_MontantGlobal.IsChecked = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }

        private void Rdb_MontantGlobal_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Txt_MontantGlobal.Text = string.Empty;
        }

        private void Rdb_NombreFactureMontant_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Txt_MontantFacture.Text = string.Empty;
            this.Txt_NombreFactureMontant.Text = string.Empty;
        }
    }
}


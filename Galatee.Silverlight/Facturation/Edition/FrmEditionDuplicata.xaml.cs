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
using Galatee.Silverlight.ServiceFacturation;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Resources.Facturation;
//using Galatee.Silverlight.ServiceEservice;

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmEditionDuplicata : ChildWindow
    {
        private bool facturesCharges = false;
        private string centre = string.Empty;
        private string client = string.Empty;
        private string ordre = string.Empty;

        private List<CsEnteteFacture> Entetefactures = new List<CsEnteteFacture>();


        public FrmEditionDuplicata()
        {
            InitializeComponent();
            this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
            this.txtClient.MaxLength = SessionObject.Enumere.TailleClient ;
            this.txtOrdre .MaxLength = SessionObject.Enumere.TailleOrdre ;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            this.dtg_DetailFacture.IsReadOnly = true;
            ChargerDonneeDuSite();
        }
        List<int> lesCentreCaisse = new List<int>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);

                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID ;
                        lsiteCentre = LstCentrePerimetre.Where(t => t.CODESITE == lstSite.First().CODE).ToList();
                        this.btn_Site.IsEnabled = false;
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_CodeCentre.Text = LstCentrePerimetre.First().CODE;
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.Txt_CodeCentre.Tag = LstCentrePerimetre.First().PK_ID;
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
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
        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite.Count > 0)
                {
                    this.btn_Site.IsEnabled = false;
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "SITE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                    ctrl.Closed += new EventHandler(galatee_OkClickedSite);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.LibelleModule);
            }
        }
        List<ServiceAccueil.CsCentre> lsiteCentre = new List<ServiceAccueil.CsCentre>();
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODE;
                this.Txt_CodeSite.Tag = leSite.PK_ID ;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
                lsiteCentre = LstCentrePerimetre.Where(t => t.CODESITE == leSite.CODE).ToList();
                if (lsiteCentre.Count == 1)
                {
                    this.Txt_CodeCentre.Text = lsiteCentre.First().CODE;
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_CodeCentre.Tag = lsiteCentre.First().PK_ID;
                }
            }
           this.btn_Site.IsEnabled = true;
        }
        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentrePerimetre.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lsiteCentre);
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "CENTRE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false , "Liste");
                    ctrl.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.LibelleModule);
            }
        }
        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Centre.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsCentre leCentre = (Galatee.Silverlight.ServiceAccueil.CsCentre)ctrs.MyObject;
                this.Txt_CodeCentre.Text = leCentre.CODE;
                this.Txt_CodeCentre.Tag = leCentre.PK_ID;
                this.Txt_LibelleCentre.Text  = leCentre.LIBELLE ;
            }
            else
                this.btn_Centre.IsEnabled = true;

        }
        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeSite.Text) && Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    Galatee.Silverlight.ServiceAccueil.CsSite _LeSiteClient = ClasseMEthodeGenerique.RetourneObjectFromList(lstSite, this.Txt_CodeSite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeSiteClient.LIBELLE))
                    {
                        this.Txt_LibelleSite.Text = _LeSiteClient.LIBELLE;
                        this.Txt_CodeSite.Text = _LeSiteClient.CODE;
                        this.Txt_CodeSite.Tag = _LeSiteClient.PK_ID;
                        List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList();
                        if (lsiteCentre.Count == 1)
                        {
                            this.Txt_CodeCentre.Text = lsiteCentre.First().CODE;
                            this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                            this.Txt_CodeCentre.Tag = lsiteCentre.First().PK_ID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

            }
        }
        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeCentre.Text) && Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    ServiceAccueil.CsCentre _LeCentreClient = ClasseMEthodeGenerique.RetourneObjectFromList(LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList(), this.Txt_CodeCentre.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
                    {
                        this.Txt_CodeCentre.Text = _LeCentreClient.CODE;
                        this.Txt_LibelleCentre.Text = _LeCentreClient.LIBELLE;
                        this.Txt_CodeCentre.Tag = _LeCentreClient.PK_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (this.cmbFacture.SelectedItem != null)
                {
                       prgBar.Visibility = System.Windows.Visibility.Visible;
                    CsEnteteFacture laFactureSelect = (CsEnteteFacture)this.cmbFacture.SelectedItem;
                          string RDlc = "FactureSimple";

                    List<CsEnteteFacture> lstClientSelect = new List<CsEnteteFacture>();
                    lstClientSelect.Add(laFactureSelect);
                    if (laFactureSelect.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                        RDlc = "FactureSimpleMT";

                    if (laFactureSelect.PRODUIT == SessionObject.Enumere.Eau)
                        RDlc = "FactureSimpleO";

                // Envoi des factures au service
                Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Facturation"));
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                string key = Utility.getKey();
                service.EnvoyerFacturesAsync(lstClientSelect, RDlc);
                service.EnvoyerFacturesCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                        {
                            //LoadingManager.EndLoading(loaderHandler);
                            throw new Exception("Cannot display report");
                        }
                        if (res.Result != null)
                        {
                            List<CsFactureClient> lstGenerale = res.Result;
                            List<CsFactureClient> lesClient = ClasseMethodeGenerique.RetourneDistinctClientFacture(res.Result);
                            string print = "Imprimé le " + DateTime.Now + " sur le poste " + SessionObject.LePosteCourant.NOMPOSTE + " par " + UserConnecte.nomUtilisateur + "(" + UserConnecte.matricule + ") du centre " + UserConnecte.LibelleCentre;

                            foreach (CsFactureClient item in lesClient)
                            {
                                Dictionary<string, string>  param= new Dictionary<string, string>();
                                param.Add("TypeEdition", "Duplicata");
                                param.Add("Print", print);
                                List<CsFactureClient> lstDetailClient = lstGenerale.Where(t => t.Centre == item.Centre && t.Client == item.Client ).ToList();
                                Utility.ActionDirectOrientation<ServicePrintings.CsFactureClient, ServiceFacturation.CsFactureClient>(lstDetailClient, param, SessionObject.CheminImpression, RDlc, "Facturation", false);
                                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            }
                            this.DialogResult = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                    }

                };

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }



        private void RecherCherFacture()
        {
            try
            {
                if (Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre &&
                    txtClient.Text.Length == SessionObject.Enumere.TailleClient &&
                    txtOrdre.Text.Length == SessionObject.Enumere.TailleOrdre)
                {

                    if (!facturesCharges)
                        prgBar.Visibility = System.Windows.Visibility.Visible ;


                    centre = Txt_CodeCentre.Text;
                    client = txtClient.Text;
                    ordre = txtOrdre.Text;
                    this.cmbFacture.ItemsSource = null;

                    FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                    service.retourneFactureAnnulationAsync((int)this.Txt_CodeCentre.Tag , centre, client, ordre);
                    service.retourneFactureAnnulationCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des factures : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null && res.Result.Count != 0)
                                {
                                    if (Entetefactures != res.Result)
                                    {
                                        Entetefactures = res.Result;
                                        Entetefactures.ForEach(t => t.REFERENCEATM = t.FACTURE + " " +Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA ( t.PERIODE));

                                        this.cmbFacture.ItemsSource = Entetefactures.OrderBy(u=>u.PERIODE ).ToList();
                                        this.cmbFacture.DisplayMemberPath = "REFERENCEATM";
                                        if (Entetefactures.Count == 1)
                                            this.cmbFacture.SelectedItem  = Entetefactures[0];

                                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                        if (Entetefactures != null)
                                            if (Entetefactures.Count > 0)
                                                lblNom.Text = Entetefactures.First().NOMABON;
                                    }
                                }
                                else
                                    Message.Show("Aucune facture trouve pour ce client",
                                        "Erreur");
                        }
                        catch (Exception ex)
                        {
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            Message.Show("Erreur inconnue :" + ex.Message, "Erreur inconnue");
                        }
                        finally
                        {
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        private void cmbFacture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.cmbFacture.SelectedItem != null)
                {
                    CsEnteteFacture leEntfacSelect = (CsEnteteFacture)this.cmbFacture.SelectedItem;
                    cmbFacture.Tag = leEntfacSelect;
                    List<CsEnteteFacture> _lstDetail = new List<CsEnteteFacture>();
                    _lstDetail.Add(leEntfacSelect);

                    this.dtg_DetailFacture.ItemsSource = null;
                    this.dtg_DetailFacture.ItemsSource = _lstDetail;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void initCtrl()
        {
            this.lblNom.Text = string.Empty;
        }

        private void btn_Rechercher_Click(object sender, RoutedEventArgs e)
        {
            RecherCherFacture();
        }
    }
}


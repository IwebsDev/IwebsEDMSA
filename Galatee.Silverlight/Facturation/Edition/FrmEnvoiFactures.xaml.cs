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
using Galatee.Silverlight.ServiceFacturation ;
using Galatee.Silverlight.Shared;
using System.Windows.Browser;
using Galatee.Silverlight.Resources.Facturation;
//using Galatee.Silverlight.ServicePrintings;

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmEnvoiFactures : ChildWindow
    {
        string centre;
        string periode;
        string ordre;
        string client;
        List<CheckBox> selectedRows = new List<CheckBox>();
        Dictionary<string, string> parametres = new Dictionary<string, string>();

        public FrmEnvoiFactures()
        {
            InitializeComponent();
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
            this.Txt_ordre.MaxLength = SessionObject.Enumere.TailleOrdre;
            this.chk_email.IsChecked = true;
            this.chk_email.IsEnabled = false;
            ChargerDonneeDuSite();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Btn_reset_Click(object sender, RoutedEventArgs e)
        {
            this.Txt_Client.Text = string.Empty;
            this.Txt_ordre.Text = string.Empty;
            this.Txt_Periode.Text = string.Empty;
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
                        this.Txt_LibelleSite .Text = lstSite.First().LIBELLE ;
                        this.Txt_CodeSite.Tag = lstSite.First().CODE;
                        this.btn_Site.IsEnabled = false;

                       
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;

                        lstCentreSelect.Add(LstCentrePerimetre.First());
                        this.Cbo_Centre.ItemsSource = null;
                        this.Cbo_Centre.ItemsSource = lstCentreSelect;
                        this.Cbo_Centre.DisplayMemberPath = "LIBELLE";
                    }
                    else
                    {
                        this.Txt_LibelleCentre.Visibility = System.Windows.Visibility.Collapsed;
                        this.Cbo_Centre.Visibility = System.Windows.Visibility.Visible;
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
        private void Btn_search_Click(object sender, RoutedEventArgs e)
        {
            if (this.Txt_Client.Text == string.Empty)
                client = null;
            else
                client = this.Txt_Client.Text;
            if (this.Txt_ordre.Text == string.Empty)
                ordre = null;
            else
                ordre = this.Txt_ordre.Text;
            if (this.Txt_Periode.Text == string.Empty)
                periode = null;
            else
            {
                periode = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_Periode.Text); ;
            }
            List<int> lstCentre = new List<int>();
            List<string> lstPeriode = new List<string>();

            foreach (ServiceAccueil.CsCentre item in lstCentreSelect)
                lstCentre.Add(item.PK_ID);

            foreach (string item in LstPeriode)
                lstPeriode.Add(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(item));
  
            // Affichage de l'indicateur de chargement dans l'arbre d'elements visuels
            int loaderHandler = LoadingManager.BeginLoading("Recupération de données ... ");
            FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
            service.ListeDesClientPourEnvoieMailAsync(lstCentre, lstPeriode, chk_sms.IsChecked.Value, chk_email.IsChecked.Value);
            service.ListeDesClientPourEnvoieMailCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                        {
                            LoadingManager.EndLoading(loaderHandler);
                            throw new Exception("Cannot display report,voici l'erreur produit :" + res.Error + "(Requette annulé = " + res.Cancelled);
                        }
                        
                        if (res.Result != null)
                        {
                            if (res.Result.Count>0)
                            {
                                List<CsEnteteFacture > facturesAEnvoyer = res.Result;
                                foreach (var item in facturesAEnvoyer)
                                    item.PERIODE = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(item.PERIODE);

                                if (!string.IsNullOrEmpty(this.Txt_Client.Text) && !string.IsNullOrEmpty(this.Txt_ordre.Text))
                                {
                                    List<CsEnteteFacture> lstFactureClient = facturesAEnvoyer.Where(t => t.CLIENT == this.Txt_Client.Text && t.ORDRE == this.Txt_ordre.Text ).ToList();
                                    this.Dtg_factures.ItemsSource = lstFactureClient;
                                }
                                else 
                                    this.Dtg_factures.ItemsSource = facturesAEnvoyer;

                                return;
                            }
                        }
                        Message.Show("Aucune données trouvé",  "Information");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        LoadingManager.EndLoading(loaderHandler);
                    }
                };

        }
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsEnteteFacture>;

            if (dg.SelectedItem != null)
            {
                CsEnteteFacture SelectedObject = (CsEnteteFacture)dg.SelectedItem;

                if (SelectedObject.IsSelect  == false)
                    SelectedObject.IsSelect = true;
                else
                    SelectedObject.IsSelect  = false;
            }
        }


        private void Btn_send_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Txt_Periode.Text))
            {

                List<CsEnteteFacture> lstClientSelect = ((List<CsEnteteFacture>)Dtg_factures.ItemsSource).Where(t => t.IsSelect).ToList();

                List<string> listeDeCles = new List<string> ();
                bool? _LeRsult = null;

                string codePro = string.Empty;
                if (lstClientSelect != null)
                    codePro = lstClientSelect.FirstOrDefault().PRODUIT;

                string RDlc = "FactureSimple";

                if (codePro == SessionObject.Enumere.Eau)
                    RDlc = "FactureSimpleO";

                // Envoi des factures au service
                int loaderHandler = LoadingManager.BeginLoading("Traitement des factures selectionnées ... ");
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                string key = Utility.getKey();
                Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Printing"));
                var uri =new Uri( App.Current.Host.Source.AbsoluteUri);
                service.EnvoyerFacturesNewAsync(lstClientSelect, RDlc, UserConnecte.matricule, SessionObject.ServerEndPointName, SessionObject.ServerEndPointPort, uri.Port.ToString());
                service.EnvoyerFacturesNewCompleted += (er, res) =>
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
                            if (res.Result.Value)
                                Message.ShowInformation("Facture transmise avec succès", "Envoi facture");


                            //List<CsFactureClient> lstGenerale = res.Result;
                            //List<CsFactureClient> lesClient = ClasseMethodeGenerique.RetourneDistinctClientFacture(res.Result);
                            //foreach (CsFactureClient item in lesClient)
                            //{
                            //    Dictionary<string, string>  param= new Dictionary<string, string>();

                            //    List<CsFactureClient> lstDetailClient = lstGenerale.Where(t => t.Centre == item.Centre && t.Client == item.Client ).ToList();
                            //    string refclient= item.Centre + item.Client + item.Ordre;
                            //    string periode = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(lstDetailClient.First().Periode);
                            //    string montant = Convert.ToDecimal(lstDetailClient.First().TotFTTC).ToString(SessionObject.FormatMontant);
                            //    //string exigible = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(item.detail.First().dateExige);
                            //    string nomAbon = item.NomAbon;
                            //    string telephone=string.Empty;
                            //    param.Add("TypeEdition", "Facture");
                            //    if (lstDetailClient.First().ISFACTURE.Value == true)
                            //    {
                            //        param.Add("pismail", lstDetailClient.FirstOrDefault(c => c.EMAIL != null && c.EMAIL != "").EMAIL);
                            //        Utility.ActionMail<ServicePrintings.CsFactureClient, ServiceFacturation.CsFactureClient>(lstDetailClient, param, "FactureSimpleMail", "Facturation");
                            //    }
                            //    if (lstDetailClient.First().ISSMS.Value == true)
                            //    { 
                            //        #region Envoi de sms
                            //        telephone = lstDetailClient.FirstOrDefault(c => c.TELEPHONE != null && c.TELEPHONE != "").TELEPHONE;
                            //        if (!string.IsNullOrWhiteSpace(telephone))
                            //        {
                            //            string message_sms1 = "Chere:" + nomAbon + "(" + refclient + "), nous vous informons de la disponibilité de votre facture du :" + periode + " de " + montant + "FCFA.";
                            //            string message_sms2 = "Vous pouvez vous présenter à nos guichets ou utiliser le service Orange Money pour réglement .Merci de votre fidélité";
                            //            Utility.SendToSmsHandler(message_sms1, telephone);
                            //            Utility.SendToSmsHandler(message_sms2, telephone);

                            //        }
                            //        #endregion
                            //    }  
                            //}

                          
                        }
                    }
                    catch (Exception ex)
                    {
                        listeDeCles.Clear();
                        throw ex;
                    }
                    finally
                    {
                        LoadingManager.EndLoading(loaderHandler);
                        listeDeCles.Clear();
                        parametres.Clear();
                    }

                };
            }
        }

   
    
   

        private void Btn_edit_Fac_Click(object sender, RoutedEventArgs e)
        {

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
                Message.ShowError(ex.Message, Langue.LibelleModule );
            }
        }

        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            lstCentreSelect.Clear();
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODE;
                this.Txt_CodeSite.Tag = leSite.CODE;
                this.Txt_LibelleSite.Text = leSite.LIBELLE ;
                List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.CODESITE == this.Txt_CodeSite.Tag.ToString()).ToList();
                if (lsiteCentre.Count == 1)
                {
                    this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                    this.Txt_LibelleCentre.Visibility = System.Windows.Visibility.Visible;
                    this.btn_Centre.Visibility = System.Windows.Visibility.Visible;
                    this.Txt_LibelleCentre.Visibility = System.Windows.Visibility.Visible;


                    lstCentreSelect.AddRange(lsiteCentre);
                    this.Cbo_Centre.ItemsSource = null;
                    this.Cbo_Centre.DisplayMemberPath = "LIBELLE";
                    this.Cbo_Centre.ItemsSource = lstCentreSelect;
                    this.Cbo_Centre.SelectedItem = lsiteCentre;
                    this.Cbo_Centre.Visibility = System.Windows.Visibility.Collapsed ;
                    
                }
                else
                {
                    this.btn_Centre.Visibility = System.Windows.Visibility.Visible;
                    this.Txt_LibelleCentre.Visibility = System.Windows.Visibility.Collapsed;
                    this.Cbo_Centre.Visibility = System.Windows.Visibility.Visible;
                }
            }
            else
                this.btn_Site.IsEnabled = true;
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentrePerimetre.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstCentrePerimetre.Where(t=>t.CODESITE == this.Txt_CodeSite.Tag.ToString()).ToList());
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "CENTRE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, true , "Liste");
                    ctrl.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.LibelleModule );
            }
        }
        List<ServiceAccueil.CsCentre> lstCentreSelect = new List<ServiceAccueil.CsCentre>();
        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Centre.IsEnabled = true;
                lstCentreSelect.Clear();
                foreach (var p in ctrs.MyObjectList)
                    lstCentreSelect.Add((Galatee.Silverlight.ServiceAccueil.CsCentre)p);

                this.Cbo_Centre.ItemsSource = null;
                this.Cbo_Centre.ItemsSource = lstCentreSelect;
                if (lstCentreSelect.Count != 0)
                this.Cbo_Centre.SelectedItem  = lstCentreSelect.First();
                this.Cbo_Centre.DisplayMemberPath  ="LIBELLE";
            }
            else
                this.btn_Centre.IsEnabled = true;

        }

        List<string> LstPeriode = new List<string>();
        private void btn_Periode_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Periode.Text))
            {
                if (LstPeriode.FirstOrDefault(t => t == this.Txt_Periode.Text) == null)
                {
                    LstPeriode.Add(this.Txt_Periode.Text);
                    this.cbo_Periode.ItemsSource = null;
                    this.cbo_Periode.ItemsSource = LstPeriode;
                    this.cbo_Periode.SelectedIndex = 0;
                }
                else
                    Message.ShowInformation("Période déja saisie", "Edition");
            }
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {

        }
   
    }
}


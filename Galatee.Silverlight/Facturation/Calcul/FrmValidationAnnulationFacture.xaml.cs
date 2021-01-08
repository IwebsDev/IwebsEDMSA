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
    public partial class FrmValidationAnnulationFacture : ChildWindow
    {
        private bool facturesCharges = false;
        private string centre = string.Empty;
        private string client = string.Empty;
        private string ordre = string.Empty;

        private List<CsEnteteFacture> Entetefactures = new List<CsEnteteFacture>();
        ServiceAccueil.CsDemande laDetailDemande = null;


        public FrmValidationAnnulationFacture(int IdDemande)
        {
            InitializeComponent();
            this.txtClient.IsReadOnly = true;
            this.txtOrdre.IsReadOnly = true;

            this.txtClient.MaxLength = SessionObject.Enumere.TailleClient;
            this.txtOrdre.MaxLength = SessionObject.Enumere.TailleOrdre;
            ChargerDonneeDuSite();
            RemplirTypeDemande();
            ChargeDetailDEvis(IdDemande);
        }

        
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            ServiceAccueil.AcceuilServiceClient client = new ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeCompleted += (ssender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    string error = args.Error.Message;
                    Message.ShowError(error, this.Title.ToString());
                    return;
                }
                if (args.Result == null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, this.Title.ToString());
                    return;
                }
                else
                {
                    laDetailDemande = new ServiceAccueil.CsDemande();
                    laDetailDemande = args.Result;
                    this.Txt_CodeSite.Text = laDetailDemande.LaDemande.NUMDEM.Substring(3, 3);
                    this.Txt_CodeCentre.Text = laDetailDemande.LaDemande.CENTRE;
                    this.txtClient.Text = laDetailDemande.LaDemande.CLIENT;
                    this.txtOrdre.Text = laDetailDemande.LaDemande.ORDRE;
                    this.txtNom.Text = laDetailDemande.LeClient.NOMABON;

                    this.Txt_CodeSite.Tag = LstCentrePerimetre.First(r => r.PK_ID == laDetailDemande.LaDemande.FK_IDCENTRE).FK_IDCODESITE;
                    this.Txt_CodeCentre.Tag = laDetailDemande.LaDemande.FK_IDCENTRE;

                    this.TxtDemande.Text = laDetailDemande.LaDemande.NUMDEM;
                    this.txtMotifDemande.Text = laDetailDemande.LaDemande.MOTIF;

                    RecherCherFacture();
                }
            };
            client.ChargerDetailDemandeAsync (IdDemandeDevis,string.Empty );
        }








        private void RemplirTypeDemande()
        {
            try
            {
                if (SessionObject.LstTypeDemande == null || SessionObject.LstTypeDemande.Count == 0)
                {
                    ServiceAccueil.AcceuilServiceClient service1 = new ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service1.RetourneOptionDemandeCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        SessionObject.LstTypeDemande = res.Result;
                    };
                    service1.RetourneOptionDemandeAsync();
                    service1.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }









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

                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID ;
                        //this.btn_Site.IsEnabled = false;
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
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                   
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
                    //this.btn_Site.IsEnabled = false;
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
                List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE  ==(int) this.Txt_CodeSite.Tag).ToList();
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
                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstCentrePerimetre.Where(t => t.FK_IDCODESITE  ==(int) this.Txt_CodeSite.Tag).ToList());
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
                this.Txt_LibelleCentre.Text = leCentre.LIBELLE ;
                this.Txt_CodeCentre.Tag = leCentre.PK_ID;
            }
            else
                this.btn_Centre.IsEnabled = true;

        }
        private void ValiderAnnulation()
        {

            CsEnteteFacture laFactureSelect = (CsEnteteFacture)this.cmbFacture.SelectedItem;
            laDetailDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;

            ServiceAccueil.AcceuilServiceClient services = new ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            services.ValiderAnnulationFactureCompleted += (ss, argss) =>
            {

                if (argss.Cancelled || argss.Error != null)
                {
                    string error = argss.Error.Message;
                    Message.ShowError(error, this.Title.ToString());
                    return;
                }
                if (string.IsNullOrEmpty(argss.Result))
                {
                    Message.ShowInformation("Demande validée avec succès", this.Title.ToString());

                    if (this.chk_Quitance.IsChecked == true)
                    {
                        List<CsEnteteFacture> lstFacture = (List<CsEnteteFacture>)this.dtg_DetailFacture.ItemsSource;
                        Utility.ActionDirectOrientation<ServicePrintings.CsEnteteFacture, ServiceFacturation.CsEnteteFacture>(lstFacture, null, SessionObject.CheminImpression, "QuittanceAnnulation", "Facturation", false);

                    }

                    this.DialogResult = true;
                }
                else
                    Message.ShowError(argss.Result, this.Title.ToString());

            };
            services.ValiderAnnulationFactureAsync(laDetailDemande, laFactureSelect.PK_ID);
            services.CloseAsync();

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Confirmez-vous cette demande ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                        ValiderAnnulation();
                    else
                        return;
                };
                messageBox.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private ServiceAccueil.CsDemande GetDemandeDevisFromScreen(ServiceAccueil.CsDemande pDemandeDevis)
        {
            try
            {
                if (pDemandeDevis == null)
                {
                    pDemandeDevis = new ServiceAccueil.CsDemande();
                    pDemandeDevis.LaDemande = new ServiceAccueil.CsDemandeBase();
                    pDemandeDevis.Abonne = new ServiceAccueil.CsAbon();
                    pDemandeDevis.Ag = new ServiceAccueil.CsAg();
                    pDemandeDevis.Branchement = new ServiceAccueil.CsBrt();
                    pDemandeDevis.LeClient = new ServiceAccueil.CsClient();
                    pDemandeDevis.ObjetScanne = new List<ServiceAccueil.ObjDOCUMENTSCANNE>();
                    pDemandeDevis.AppareilDevis = new List<ServiceAccueil.ObjAPPAREILSDEVIS>();
                    pDemandeDevis.LstEvenement = new List<ServiceAccueil.CsEvenement>();
                    pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                    pDemandeDevis.LaDemande.USERCREATION = UserConnecte.matricule;
                    pDemandeDevis.LaDemande.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                }
                #region Demande

                if (this.cmbFacture.SelectedItem != null && !string.IsNullOrEmpty(txtMotifDemande.Text))
                {
                    ServiceAccueil.CsEvenement ev = new ServiceAccueil.CsEvenement();
                    CsEnteteFacture ent = (CsEnteteFacture)this.cmbFacture.SelectedItem;

                    ev.LOTRI = ent.LOTRI;
                    ev.FK_IDABON = ent.FK_IDABON;
                    ev.FACTURE = ent.FACTURE;
                    ev.PERIODE = ent.PERIODE;
                    ev.USERCREATION = UserConnecte.matricule;

                    pDemandeDevis.LaDemande.ISNEW = true;
                    pDemandeDevis.LstEvenement.Add(ev);
                    pDemandeDevis.LaDemande.ORDRE = txtOrdre.Text;
                    pDemandeDevis.LaDemande.CLIENT = ent.CLIENT;
                    pDemandeDevis.LaDemande.SITE = Txt_CodeSite.Text;
                    pDemandeDevis.LaDemande.CENTRE = ent.CENTRE;
                    pDemandeDevis.LaDemande.FK_IDCENTRE = ent.FK_IDCENTRE;
                    pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                    pDemandeDevis.LaDemande.USERCREATION = UserConnecte.matricule;
                    pDemandeDevis.LaDemande.MOTIF = txtMotifDemande.Text;
                    pDemandeDevis.LaDemande.ISMETREAFAIRE = false;
                    pDemandeDevis.LaDemande.TYPEDEMANDE = SessionObject.LstTypeDemande.First(a => a.CODE == "15").CODE; //AnnulationFacture
                    pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.First(a => a.CODE == "15").PK_ID; //AnnulationFacture
                }

                #endregion


                return pDemandeDevis;
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

                    centre = Txt_CodeCentre.Text;
                    client = txtClient.Text;
                    ordre = txtOrdre.Text;
                    this.cmbFacture.ItemsSource = null;


                    if (this.Txt_CodeCentre.Tag == null && this.Txt_CodeSite.Tag != null)
                    {
                        ServiceAccueil.CsCentre st = ClasseMEthodeGenerique.RetourneObjectFromList(LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList(), this.Txt_CodeCentre.Text, "CODE");
                        if (!string.IsNullOrEmpty(st.LIBELLE))
                            this.Txt_CodeCentre.Tag = st.PK_ID;
                    }



                    FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                    service.retourneFactureAnnulationAsync((int)this.Txt_CodeCentre.Tag , centre, client, ordre);
                    service.retourneFactureAnnulationCompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des factures : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result != null || res.Result.Count != 0)
                                {
                                    if (Entetefactures != res.Result)
                                    {
                                        if (this.laDetailDemande != null && this.laDetailDemande.LstEvenement != null)
                                        {
                                            ServiceAccueil.CsEvenement even = this.laDetailDemande.LstEvenement.First();
                                            Entetefactures = res.Result.Where(a => a.FACTURE == even.FACTURE && a.PERIODE == even.PERIODE).OrderByDescending(t => t.PERIODE).ToList();
                                        }
                                        else
                                            Entetefactures = res.Result.OrderByDescending(t=>t.PERIODE ).ToList();
                                        Entetefactures.ForEach(t => t.REFERENCEATM = t.FACTURE + " " + Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(t.PERIODE));

                                        this.cmbFacture.ItemsSource = Entetefactures;
                                        this.cmbFacture.DisplayMemberPath = "REFERENCEATM";
                                        if (Entetefactures.Count == 1)
                                            this.cmbFacture.SelectedItem  = Entetefactures[0];

                                        prgBar.Visibility = System.Windows.Visibility.Collapsed;


                                        this.txtNom.Text = Entetefactures.First().NOMABON;
                                    }
                                }
                                else
                                    Message.Show("Aucune facture trouvée pour ce client",
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
                this.OKButton.IsEnabled = (this.cmbFacture.SelectedItem != null) && !string.IsNullOrEmpty(this.txtMotifDemande.Text);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_Rechercher_Click(object sender, RoutedEventArgs e)
        {
            RecherCherFacture();
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
                Message.ShowError(ex.Message, this.Title.ToString());

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
                Message.ShowError(ex.Message, this.Title.ToString());

            }
        }

        private void txtMotifDemande_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.OKButton.IsEnabled = (this.cmbFacture.SelectedItem != null) && !string.IsNullOrEmpty(this.txtMotifDemande.Text);
        }

        private void btnRejeter_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
            this.DialogResult = false;

        }
         
    }
}


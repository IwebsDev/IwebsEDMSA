using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.Shared;
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

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmFactureAjustementAnc : ChildWindow
    {
        public FrmFactureAjustementAnc()
        {
            InitializeComponent();
            //this.txtClient.IsReadOnly = true;
            //this.txtOrdre.IsReadOnly = true;

            this.txtClient.MaxLength = SessionObject.Enumere.TailleClient;
            this.txtOrdre.MaxLength = SessionObject.Enumere.TailleOrdre;
            ChargerDonneeDuSite();
            RetourneListeDesCas();
        }
        List<Galatee.Silverlight.ServiceAccueil.CsCasind> LsDesCas = new List<Galatee.Silverlight.ServiceAccueil.CsCasind>();
        Galatee.Silverlight.ServiceAccueil.CsClient leClient = new Galatee.Silverlight.ServiceAccueil.CsClient();
        Galatee.Silverlight.ServiceAccueil.CsEvenement LeEvenementSelect;
        Galatee.Silverlight.ServiceAccueil.CsCanalisation LeCompteurSelect;
        List<CsEvenement> LstEvenement = new List<CsEvenement>();
        DateTime? DateDernierEvt = null;

        List<CsEvenement> LstEvenementCree = new List<CsEvenement>();

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstEvenementCree != null && LstEvenementCree.Count > 0)
                {
                    if (LstEvenementCree.First().PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    {
                        if (LstEvenementCree.Count != 6)
                            Message.ShowInformation(Langue.MsgSaisirCompteurMT, "Facturation");
                    }
                    ValiderCreation(LstEvenementCree, LstPagisolCree);
                }
            }
            catch (Exception es)
            {
                throw es;
            }
        }
        void ValiderCreation(List<CsEvenement> LstEvenementCree, List<CsPagisol> lstPagisol)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ValiderCreationFactureIsoleCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null && args.Result == true)
                {
                    string Messages = Langue.msgLotIsole + "  " + LstEvenementCree.First().LOTRI;
                    Message.ShowInformation(Messages, "Facturation");
                }

            };
            service.ValiderCreationFactureIsoleAsync(LstEvenementCree, lstPagisol);
            service.CloseAsync();

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {

        }
  

        private void CreeEvenement(Galatee.Silverlight.ServiceAccueil.CsEvenement _LeEvt)
        {
            try
            {
                _LeEvt.CONSO = string.IsNullOrEmpty(this.Txt_ConsoEnCours.Text) ? 0 : int.Parse(this.Txt_ConsoEnCours.Text);
                _LeEvt.INDEXEVT = _LeEvt.INDEXEVT + _LeEvt.CONSO;
                _LeEvt.DATEEVT = null;
                _LeEvt.FACTURE = null;
                _LeEvt.ENQUETE = string.Empty;
                _LeEvt.FACPER = string.Empty;
                _LeEvt.DERPERF = string.Empty;
                _LeEvt.DERPERFN = string.Empty;
                _LeEvt.REGCONSO = null;
                _LeEvt.REGIMPUTE = null;
                _LeEvt.CONSOFAC = 0;
                _LeEvt.MATRICULE = UserConnecte.matricule;
                _LeEvt.COMPTEUR = _LeEvt.COMPTEUR ;
                _LeEvt.TYPECOMPTEUR = _LeEvt.TYPECOMPTEUR;
                _LeEvt.COEFLECT = _LeEvt.COEFLECT;
                _LeEvt.COEFCOMPTAGE = _LeEvt.COEFCOMPTAGE;
                _LeEvt.COEFLECT = _LeEvt.COEFLECT;
                _LeEvt.CODEEVT = SessionObject.Enumere.EvenementCodeFactureIsole;
                _LeEvt.LOTRI = SessionObject.LePosteCourant.CODECENTRE + SessionObject.Enumere.LotriAjustement ;
                _LeEvt.STATUS = SessionObject.Enumere.EvenementReleve;
                _LeEvt.NUMEVENEMENT = MaxNumEvt + 1;
                _LeEvt.USERCREATION = UserConnecte.matricule;
                _LeEvt.USERMODIFICATION = UserConnecte.matricule;
                _LeEvt.DATECREATION = System.DateTime.Now.Date;
                _LeEvt.DATEMODIFICATION = System.DateTime.Now.Date;
                if (_LeEvt != null && LstEvenementCree.Count > 0)
                {
                    if (LstEvenementCree.First().PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    {
                        if (LstEvenementCree.Count != 6)
                            Message.ShowInformation(Langue.MsgSaisirCompteurMT, "Facturation");
                    }
                    ValiderCreation(LstEvenementCree, LstPagisolCree);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
  

        private void RetourneListeDesCas()
        {
            if (SessionObject.LstDesCas.Count != 0)
            {
                LsDesCas = SessionObject.LstDesCas;
                return;
            }
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneListeDesCasCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                LsDesCas = args.Result;
                SessionObject.LstDesCas = LsDesCas;

            };
            service.RetourneListeDesCasAsync();
            service.CloseAsync();
        }
        int MaxNumEvt = 0;
        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LeCompteurSelect = new Galatee.Silverlight.ServiceAccueil.CsCanalisation();
            //indexSelect = this.dataGrid1.SelectedIndex;

            if (this.dataGrid1.SelectedIndex >= 0)
            {
                LeCompteurSelect = (CsCanalisation)this.dataGrid1.SelectedItem;
                LeEvenementSelect = LstEvenement.FirstOrDefault(t => t.POINT == LeCompteurSelect.POINT);
                DateDernierEvt = LeEvenementSelect.DATEEVT;
                MaxNumEvt = LeEvenementSelect.NUMEVENEMENT;
                RemplireOngletEvenement(LeCompteurSelect, LeEvenementSelect);

            }
        }
        private void RemplireOngletEvenement(CsCanalisation _LeEvtSelect, CsEvenement _leDernierEvt)
        {
            if (!string.IsNullOrEmpty(_leDernierEvt.DERPERF))
                this.Txt_periodeFactureF.Text = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(_leDernierEvt.DERPERF);

            if (!string.IsNullOrEmpty(_leDernierEvt.DERPERFN))
                this.Txt_periodeFactureN.Text = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(_leDernierEvt.DERPERFN);

            this.Txt_IndexFacture.Text = string.IsNullOrEmpty(_leDernierEvt.INDEXEVT.ToString()) ? string.Empty : _leDernierEvt.INDEXEVT.ToString();
            this.Txt_ConsoFacture.Text = string.IsNullOrEmpty(_leDernierEvt.CONSO.ToString()) ? string.Empty : _leDernierEvt.CONSO.ToString();
            this.Txt_CasFacture.Text = string.IsNullOrEmpty(_leDernierEvt.CAS) ? string.Empty : _leDernierEvt.CAS;
            this.Txt_Quantite.Text = string.IsNullOrEmpty(_leDernierEvt.CONSOFAC.ToString()) ? string.Empty : _leDernierEvt.CONSOFAC.ToString();
            this.Txt_Quantite.IsReadOnly = true;

        }
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        List<int> lstIdCentre = new List<int>();
        private List<CsCentre> _listeDesCentreExistant = null;

        void ChargerListDesSite()
        {
            try
            {

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    _listeDesCentreExistant = lesCentre;

                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        lstIdCentre.Add(item.PK_ID);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteAsync(false);
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        if (SessionObject.LstCentre.Count != 0)
                        {
                            lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                            lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                            _listeDesCentreExistant = lesCentre;

                            foreach (ServiceAccueil.CsCentre item in lesCentre)
                                lstIdCentre.Add(item.PK_ID);

                        }
                        else
                        {
                            Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.Msg_ModificationFacture, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            w.OnMessageBoxClosed += (_, result) =>
            {
                if (w.Result == MessageBoxResult.No)
                    this.chk_ConsoSeul.IsChecked = true;
            };
            w.Show();
        }


        private void Btn_Recherche_Click(object sender, RoutedEventArgs e)
        {
            if (this.Txt_LibelleCentre.Tag != null)
            {
                CsCentre leCentreselect = (CsCentre)this.Txt_LibelleCentre.Tag;
                CsClient leClient = new CsClient();
                leClient.CENTRE = leCentreselect.CODE;
                leClient.REFCLIENT = this.txtClient.Text;
                leClient.PRODUIT = this.Txt_Produit.Tag.ToString();

                leClient.FK_IDCENTRE = leCentreselect.PK_ID;
                string OrdreMax = string.Empty;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneOrdreMaxCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    OrdreMax = args.Result;
                    if (OrdreMax != null)
                    {
                        leClient.ORDRE = OrdreMax;
                        this.txtOrdre.Text = OrdreMax;
                        RetourneInfoClient(leClient);
                    }
                    else
                    {
                        Message.ShowInformation("Abonnement non trouvé", "Facturation");
                        return;
                    }
                };
                service.RetourneOrdreMaxAsync(leClient.FK_IDCENTRE.Value, leClient.CENTRE, leClient.REFCLIENT, leClient.PRODUIT);
                service.CloseAsync();

            }
        }
        CsAbon leAbonnement = new CsAbon();
        private void RetourneInfoClient(CsClient leClientRech)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneAbonCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                {
                    Message.ShowError(Langue.Msg_AbonnemtInexistant, Galatee.Silverlight.Resources.Facturation.Langue.LibelleModule);
                    return;
                }
                if (args.Result != null && args.Result.Count == 1)
                    leAbonnement = args.Result.First();

                if (leAbonnement != null && !string.IsNullOrEmpty(leAbonnement.CENTRE))
                    RetourneEvenementCanalisation(leAbonnement);
            };
            service.RetourneAbonAsync(leClientRech.FK_IDCENTRE.Value, leClientRech.CENTRE, leClientRech.REFCLIENT, leClientRech.ORDRE);
            service.CloseAsync();

        }
        private void RetourneEvenementCanalisation(CsAbon LeAbonnement)
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneDernierEvenementDeLaCanalisationAsync(LeAbonnement);
                service.RetourneDernierEvenementDeLaCanalisationCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstEvenement = args.Result;
                        foreach (CsEvenement item in LstEvenement)
                        {
                            if (item.ETATCOMPTEUR == SessionObject.Enumere.CompteurActifValeur) item.LIBELLEETATCOMPTEUR = SessionObject.Enumere.CompteurActif;
                            else item.LIBELLEETATCOMPTEUR = SessionObject.Enumere.CompteurInactifValeur;
                        }
                        this.dataGrid1.ItemsSource = null;
                        this.dataGrid1.ItemsSource = LstEvenement;
                        if (LstEvenement != null && LstEvenement.Count != 0)
                            this.dataGrid1.SelectedItem = LstEvenement.First();
                    }
                    catch (Exception ex)
                    {
                        Message.ShowWarning(ex.Message, Langue.Msg_LeClientEstEnFacturation);
                    }

                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void btnValiderSaisie_Click(object sender, RoutedEventArgs e)
        {
            //CreeEvenement(LeEvenementSelect, LeCompteurSelect);
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
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);

                    if (lstSite != null && lstSite.Count != 0)
                    {
                        if (lstSite.Count == 1)
                        {
                            lSiteSelect = lstSite.First();
                            this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                            this.btn_Site.IsEnabled = false;
                            this.txtClient.IsReadOnly = false;
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    if (lstSite != null && lstSite.Count != 0)
                    {
                        if (lstSite.Count == 1)
                        {
                            lSiteSelect = lstSite.First();
                            this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                            this.btn_Site.IsEnabled = false;
                            this.txtClient.IsReadOnly = false;
                        }
                    }
                    if (LstCentrePerimetre != null && LstCentrePerimetre.Count != 0)
                    {
                        if (LstCentrePerimetre.Count == 1)
                        {
                            this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                            this.Txt_LibelleCentre.Tag = LstCentrePerimetre.First().PK_ID;
                            this.btn_Centre.IsEnabled = false;
                            this.txtClient.IsReadOnly = false;
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
                    List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedSite);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsSite leSite = (CsSite)ctrs.MyObject;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
                this.Txt_LibelleSite.Tag = leSite.PK_ID;
                lSiteSelect = leSite;
                this.txtClient.IsReadOnly = false;
                List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).ToList();
                if (lsiteCentre.Count == 1)
                {
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_LibelleCentre.Tag = lsiteCentre.First();
                    this.btn_Centre.IsEnabled = false;
                    lProduitSelect = lsiteCentre.First().LESPRODUITSDUSITE;

                }
                else
                {
                    this.Txt_LibelleCentre.Text = string.Empty;
                    this.Txt_LibelleCentre.Tag = null;
                    this.btn_Centre.IsEnabled = true;
                }
            }

        }
        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            if (this.Txt_LibelleSite.Tag != null)
            {
                List<CsCentre> lstCentreSite = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).ToList();
                if (lstCentreSite.Count > 0)
                {
                    this.btn_Site.IsEnabled = false;
                    List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(lstCentreSite);
                    UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedClient);
                    ctr.Show();
                }

            }
        }
        void galatee_OkClickedClient(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Centre.IsEnabled = true;
                CsCentre leCentre = (CsCentre)ctrs.MyObject;
                this.Txt_LibelleCentre.Text = leCentre.LIBELLE;
                this.Txt_LibelleCentre.Tag = leCentre;
                lProduitSelect = leCentre.LESPRODUITSDUSITE;

            }

        }

        private void btn_Produit_Click(object sender, RoutedEventArgs e)
        {
            if (lProduitSelect != null && lProduitSelect.Count > 0)
            {
                this.btn_Site.IsEnabled = false;
                List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(lProduitSelect);
                UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedProduit);
                ctr.Show();
            }
        }
        void galatee_OkClickedProduit(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Produit.IsEnabled = true;
                CsProduit leProduit = (CsProduit)ctrs.MyObject;
                this.Txt_Produit.Text = leProduit.LIBELLE;
                this.Txt_Produit.Tag = leProduit.CODE;
            }
        }
    }
}


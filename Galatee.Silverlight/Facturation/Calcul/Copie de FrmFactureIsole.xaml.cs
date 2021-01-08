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
    public partial class FrmFactureIsole : ChildWindow
    {
        public FrmFactureIsole()
        {
            InitializeComponent();
            this.txtClient.IsReadOnly = true;
            this.txtOrdre.IsReadOnly = true;

            this.txtClient.MaxLength = SessionObject.Enumere.TailleClient;
            this.txtOrdre.MaxLength = SessionObject.Enumere.TailleOrdre;
            this.Txt_CasEnCour.MaxLength = SessionObject.Enumere.TailleCas;
            this.Txt_DateRelEncour .MaxLength = SessionObject.Enumere.TailleDate ;
            ChargerDonneeDuSite();
            RetourneListeDesCas();
        }
        string SuffixeLot = SessionObject.Enumere.LotriManuel;
        public FrmFactureIsole(int idDemande)
        {
            InitializeComponent();
            this.txtClient.IsReadOnly = true;
            this.txtOrdre.IsReadOnly = true;

            this.txtClient.MaxLength = SessionObject.Enumere.TailleClient;
            this.txtOrdre.MaxLength = SessionObject.Enumere.TailleOrdre;
            this.Txt_CasEnCour.MaxLength = SessionObject.Enumere.TailleCas;
            this.Txt_DateRelEncour.MaxLength = SessionObject.Enumere.TailleDate;
            this.Btn_Recherche.Visibility = System.Windows.Visibility.Collapsed;
            this.txtClient.IsReadOnly = true;
            ChargerDemande(idDemande);
            RetourneListeDesCas();
        }
        List<Galatee.Silverlight.ServiceAccueil.CsCasind> LsDesCas = new List<Galatee.Silverlight.ServiceAccueil.CsCasind>();
        Galatee.Silverlight.ServiceAccueil.CsClient leClient =new Galatee.Silverlight.ServiceAccueil.CsClient();
        Galatee.Silverlight.ServiceAccueil.CsEvenement LeEvenementSelect;
        Galatee.Silverlight.ServiceAccueil.CsCanalisation LeCompteurSelect;
        List<CsEvenement> LstEvenement = new List<CsEvenement>();
        DateTime? DateDernierEvt = null;

        List<CsEvenement> LstEvenementCree = new List<CsEvenement>();
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Txt_CasEnCour.Text)) this.Txt_CasEnCour.Text = "00";
                {
                    VerifieCas(true);
                    if (!EstCasValider)
                        return;
                    if (!string.IsNullOrEmpty(this.Txt_IndexSaisi.Text))
                    {
                        Txt_IndexNouv.Text = this.Txt_IndexSaisi.Text;

                        if (LeEvenementSelect.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageMaximetre)
                            LeEvenementSelect.CONSO = int.Parse(this.Txt_IndexSaisi.Text) - LeEvenementSelect.INDEXEVT;
                        else
                            LeEvenementSelect.CONSO = int.Parse(this.Txt_IndexSaisi.Text);

                        LeEvenementSelect.INDEXEVT = int.Parse(this.Txt_IndexSaisi.Text);
                    }
                    if (!string.IsNullOrEmpty(this.Txt_ConsomationSaisi.Text))
                    {
                        LeEvenementSelect.CONSO = int.Parse(this.Txt_ConsomationSaisi.Text);
                        LeEvenementSelect.INDEXEVT  = LeEvenementSelect.INDEXPRECEDENTEFACTURE ;
                    }
                    if (LeCasRecherche.SAISIEINDEX == SessionObject.Enumere.CodeInterdit )
                    {
                        CreeLstEvenement(LstEvenement);
                        return;
                    }
                }
                CreeEvenement(LeEvenementSelect);
            }
            catch (Exception es)
            {
                throw es;
            }
        }

        void ValiderSaisie()
        {

            if (LstEvenementCree != null && LstEvenementCree.Count > 0)
            {
                if (LeEvenementSelect.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                {
                    if (LstEvenementCree.Count == 6)
                    {
                        LstEvenementCree.ForEach(y => y.ISCONSOSEULE = this.chk_FacturationTotal.IsChecked == true ? false : true);
                        LstEvenementCree.ForEach(y => y.ISEVENEMENTNONFACURE = (IsEvtNonFactEnCour == "1" ? true : false));
                        LstEvenementCree.ForEach(y => y.LOTASUPPRIMER = lotSupprimer);
                   
                        OKButton.IsEnabled = false;
                        ValiderCreation(LstEvenementCree);
                    }
                }
                else
                {
                    LstEvenementCree.ForEach(y => y.ISCONSOSEULE = this.chk_FacturationTotal.IsChecked == true ? false : true);
                    LstEvenementCree.ForEach(y => y.ISEVENEMENTNONFACURE = (IsEvtNonFactEnCour == "1" ? true : false));
                    LstEvenementCree.ForEach(y => y.LOTASUPPRIMER = lotSupprimer);

                    OKButton.IsEnabled = false;
                    ValiderCreation(LstEvenementCree);
                }

            }
            CsEvenement lev1 = LstEvenement.FirstOrDefault(t => t.POINT == LeEvenementSelect.POINT);
            if (lev1 != null)
                lev1.IsSaisi = true;

            this.dataGrid1.SelectedItem = LstEvenement.FirstOrDefault(t => t.IsSaisi != true);
            this.Txt_IndexSaisi.Text = string.Empty;
            this.Txt_Consomation.Text = string.Empty;
            this.Txt_CasEnCour.Text = string.Empty;
            this.Txt_IndexNouv.Text = string.Empty;
        
        }


        void ValiderCreation(List<CsEvenement> LstEvenementCree )
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Accueil"));
            service.ValiderCreationFactureIsoleCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null && args.Result == true)
                {
                    this.DialogResult = true;
                    string Messages = Langue.msgLotIsole + "  " + LstEvenementCree.First().LOTRI;
                    Message.ShowInformation(Messages, "Facturation");
                }
                else
                {
                    Message.ShowInformation("Erreur a l'insertion de l", "Facturation");
                }
    
            };
            service.ValiderCreationFactureIsoleAsync(LstEvenementCree);
            service.CloseAsync();
        
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Txt_PeriodeEnCour_TextChanged(object sender, TextChangedEventArgs e)
        {
           
                if (this.Txt_PeriodeEnCour.Text.Length == 7)
                    if (Galatee.Silverlight.Shared.ClasseMEthodeGenerique.IsFormatPeriodeValide(Txt_PeriodeEnCour.Text))
                    {
                        this.Txt_FinPeriode.Text = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.DernierJourDuMois(int.Parse(Txt_PeriodeEnCour.Text.Substring(0, 2)), int.Parse(Txt_PeriodeEnCour.Text.Substring(3, 4)));
                        this.Txt_DebutPeriode.Text = "01" + "/" + Txt_PeriodeEnCour.Text.Substring(0, 2).PadLeft(2, '0') + "/" + Txt_PeriodeEnCour.Text.Substring(3, 4);
                    }
        }
        private void Txt_IndexSaisi_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
 
        
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CreeLstEvenement(List<Galatee.Silverlight.ServiceAccueil.CsEvenement> _LeEvt)
        {
            try
            {
                var w = new MessageBoxControl.MessageBoxChildWindow("Facturation", "La saisie d'index est interdite pour le cas " + this.Txt_CasEnCour.Text + "   les index precedents vont etre reconduit" + "\n\r" + "  Voulez vous continuer?"
                    , MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            if (w.Result == MessageBoxResult.OK)
                            {
                                foreach (CsEvenement item in _LeEvt)
                                {
                                    item.CAS = this.Txt_CasEnCour.Text;

                                    if (!string.IsNullOrEmpty(this.Txt_DateRelEncour.Text))
                                        item.DATEEVT = DateTime.Parse(this.Txt_DateRelEncour.Text);

                                    item.STATUS = SessionObject.Enumere.EvenementReleve;
                                    item.MATRICULE = UserConnecte.matricule;
                                    //item.CONSO = 0; ZEG 15/09/2017
                                    item.CODEEVT = SessionObject.Enumere.EvenementCodeFactureIsole;

                                    item.LOTRI = item.CENTRE + SuffixeLot;
                                    item.PERIODE = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_PeriodeEnCour.Text);
                                    item.FACTURE = null;
                                    item.USERCREATION = UserConnecte.matricule;
                                    item.DATECREATION = System.DateTime.Now;
                                    if (laDemande != null)
                                    {
                                        item.NUMDEM = laDemande.LaDemande.NUMDEM;
                                        item.FK_IDDEMANDE = laDemande.LaDemande.PK_ID;
                                    }
                                }
                                _LeEvt.ForEach(y => y.ISCONSOSEULE = this.chk_FacturationTotal.IsChecked == true ? false : true);
                                _LeEvt.ForEach(y => y.ISEVENEMENTNONFACURE = (IsEvtNonFactEnCour == "1" ? true : false));
                                OKButton.IsEnabled = false;
                                ValiderCreation(_LeEvt);
                            }
                        };
                        w.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur a la validation", "Index");
            }

        }
        private void CreeEvenement(Galatee.Silverlight.ServiceAccueil.CsEvenement _LeEvt)
        {
            try
            {
                if ((!string.IsNullOrEmpty(this.Txt_IndexSaisi.Text) || !string.IsNullOrEmpty(this.Txt_ConsomationSaisi.Text )) &&
                    !string.IsNullOrEmpty(this.Txt_PeriodeEnCour.Text) &&
                    !string.IsNullOrEmpty(this.Txt_DateRelEncour.Text) &&
                    !string.IsNullOrEmpty(this.Txt_CasEnCour.Text))
                {
                    _LeEvt.INDEXEVT = string.IsNullOrEmpty(this.Txt_IndexSaisi.Text) ? int.Parse(this.Txt_IndexAnc.Text) : int.Parse(this.Txt_IndexSaisi.Text);
                    _LeEvt.CONSO = _LeEvt.CONSO;
                    _LeEvt.PERIODE = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_PeriodeEnCour.Text);
                    _LeEvt.CAS = this.Txt_CasEnCour.Text;
                    if (!string.IsNullOrEmpty(this.Txt_DateRelEncour.Text))
                        _LeEvt.DATEEVT = DateTime.Parse(this.Txt_DateRelEncour.Text);
                    
                        _LeEvt.CODEEVT = SessionObject.Enumere.EvenementCodeFactureIsole;
                        _LeEvt.LOTRI = _LeEvt.CENTRE  + SuffixeLot;

                    _LeEvt.STATUS = SessionObject.Enumere.EvenementReleve;

                    _LeEvt.NUMEVENEMENT = MaxNumEvt + 1;
                    _LeEvt.USERCREATION = UserConnecte.matricule;
                    _LeEvt.USERMODIFICATION = UserConnecte.matricule;
                    _LeEvt.DATECREATION = System.DateTime.Now.Date;
                    _LeEvt.DATEMODIFICATION = System.DateTime.Now.Date;
                    if (laDemande != null)
                    {
                        _LeEvt.NUMDEM = laDemande.LaDemande.NUMDEM;
                        _LeEvt.FK_IDDEMANDE  = laDemande.LaDemande.PK_ID ;
                        _LeEvt.IDETAPE = laDemande.InfoDemande.FK_IDETAPEACTUELLE;
                    }
                    IsSaisieValider(_LeEvt, LsDesCas);
                    
                }

            }
            catch (Exception ex)
            {
                Message.ShowError ("Erreur a la validation", "Index");
            }

        }
        private void btn_cas_Click(object sender, RoutedEventArgs e)
        {

            btn_cas.IsEnabled = false;
            List<object> _LstCas = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(LsDesCas);
            Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstCas, "CODE", "LIBELLE", Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeCommune);
            ctr.Closed += new EventHandler(galatee_OkClicked);
            ctr.Show();
        }
        void galatee_OkClicked(object sender, EventArgs e)
        {
            try
            {
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsCasind _lstCas = (Galatee.Silverlight.ServiceAccueil.CsCasind)ctrs.MyObject;
                    if (_lstCas != null)
                        this.Txt_CasEnCour.Text = _lstCas.CODE;
                }
                this.btn_cas.IsEnabled = true;
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
            if (this.dataGrid1.SelectedIndex >= 0)
            {
                LeEvenementSelect = (CsEvenement )this.dataGrid1.SelectedItem;
                DateDernierEvt = LeEvenementSelect.DATERELEVEPRECEDENTEFACTURE ;
                Txt_IndexAnc.Text  = LeEvenementSelect.INDEXEVT.ToString();
                MaxNumEvt = LeEvenementSelect.NUMEVENEMENT ;
                RemplireOngletEvenement( LeEvenementSelect);

            }
        }
        private void RemplireOngletEvenement(CsEvenement _leDernierEvt)
        {
            if (!string.IsNullOrEmpty(_leDernierEvt.PERIODEPRECEDENTEFACTURE))
                this.Txt_periodeFactureF.Text = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(_leDernierEvt.PERIODEPRECEDENTEFACTURE  );

            this.Txt_IndexFacture.Text = string.IsNullOrEmpty(_leDernierEvt.INDEXPRECEDENTEFACTURE.ToString()) ? string.Empty : _leDernierEvt.INDEXPRECEDENTEFACTURE.ToString();
            this.Txt_ConsoFacture.Text = string.IsNullOrEmpty(_leDernierEvt.CASPRECEDENTEFACTURE.ToString()) ? string.Empty : _leDernierEvt.CASPRECEDENTEFACTURE.ToString();
            this.Txt_CasFacture.Text = string.IsNullOrEmpty(_leDernierEvt.CASPRECEDENTEFACTURE) ? string.Empty : _leDernierEvt.CASPRECEDENTEFACTURE;
            Txt_DateFacture.Text = _leDernierEvt.DATEEVT == null  ? string.Empty : _leDernierEvt.DATEEVT.Value.ToShortDateString();
        }
        private void Txt_CasEnCour_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CasEnCour.Text.Length == SessionObject.Enumere.TailleCas)
            {
                /** ZEG 15/09/2017 **/
                if (string.IsNullOrEmpty(this.Txt_IndexSaisi.Text))
                    VerifieCas(false);
                else
                    VerifieCas(true);
                /****/

            }
        }
        private void Txt_DateRelEncour_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_DateRelEncour.Text.Length == SessionObject.Enumere.TailleDate)
                {
                    DateTime? pDateFin = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.IsDateValider(this.Txt_DateRelEncour.Text);
                    if (Galatee.Silverlight.Shared.ClasseMEthodeGenerique.IsDateSaisieValide(pDateFin, DateDernierEvt))
                    { }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.Msg_DateFinInferieurDateDebut, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            if (w.Result == MessageBoxResult.OK)
                            { }
                            else
                            {
                                this.Txt_DateRelEncour.Text = string.Empty;
                                this.Txt_DateRelEncour.Focus();
                            }
                        };
                        w.Show();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<int> lesCentreCaisse = new List<int>();

        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        Galatee.Silverlight.ServiceAccueil.CsSite lSiteSelect = new Galatee.Silverlight.ServiceAccueil.CsSite();
        List< Galatee.Silverlight.ServiceAccueil.CsProduit> lProduitSelect = new List< Galatee.Silverlight.ServiceAccueil.CsProduit>();
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
                            this.Txt_LibelleSite.Text = lstSite.First().LIBELLE ;
                            this.Txt_LibelleSite.Tag = lstSite.First().PK_ID;
                            this.btn_Site.IsEnabled = false;
                            this.txtClient.IsReadOnly = false;
                        }
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.Txt_LibelleCentre.Tag = LstCentrePerimetre.First();
                        lProduitSelect = LstCentrePerimetre.First().LESPRODUITSDUSITE;

                        if (lProduitSelect != null && lProduitSelect.Count != 0 )
                        {
                            //if (lProduitSelect.Count == 1)
                            //{
                                this.Txt_Produit.Text = lProduitSelect.First().LIBELLE;
                                this.Txt_Produit.Tag = lProduitSelect.First().CODE;
                                this.btn_Produit .Tag = lProduitSelect.First().PK_ID ;
                            //}
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
                            this.Txt_LibelleCentre.Tag  = LstCentrePerimetre.First()  ;
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
        ServiceAccueil.CsDemande laDemande = new CsDemande();
        private void ChargerDemande(int iddemande)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerDetailDemandeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                    laDemande = args.Result;

                    this.Txt_LibelleCentre.Text = laDemande.LaDemande.LIBELLECENTRE;
                    this.Txt_LibelleSite.Text = laDemande.LaDemande.LIBELLESITE;
                    this.txtClient.Text = laDemande.LaDemande.CLIENT;
                    this.txtOrdre.Text = laDemande.LaDemande.ORDRE;
                    this.Txt_LibelleCentre.Tag = laDemande.LaDemande.FK_IDCENTRE;

                    if (laDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation)
                        SuffixeLot = SessionObject.Enumere.LotriTermination;

                    CsClient leClient = new CsClient();
                    leClient.FK_IDCENTRE  = laDemande.LaDemande.FK_IDCENTRE  ;
                    leClient.CENTRE = laDemande.LaDemande.CENTRE  ;
                    leClient.REFCLIENT = laDemande.LaDemande.CLIENT   ;
                    leClient.PRODUIT = laDemande.LaDemande.PRODUIT;
                    leClient.ORDRE = laDemande.LaDemande.ORDRE;
                    leClient.PERIODE =string.Empty ;
                    leClient.FK_IDPRODUIT =laDemande.LaDemande.FK_IDPRODUIT ;

                    RetourneEvenementClient(leClient);
                };
                service.ChargerDetailDemandeAsync(iddemande,string .Empty );
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

                    this.Txt_Produit.Text = lProduitSelect.First().LIBELLE;
                    this.Txt_Produit.Tag = lProduitSelect.First().CODE;

                }
                else
                {
                    this.Txt_LibelleCentre.Text = string.Empty;
                    this.Txt_LibelleCentre.Tag = null;
                    this.btn_Centre.IsEnabled = true;

                    this.Txt_Produit.Text = string.Empty;
                    this.Txt_Produit.Tag = null;
                }
            }
            this.btn_Site.IsEnabled = true;
        }
        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            if (this.Txt_LibelleSite.Tag != null)
            {
                List<CsCentre> lstCentreSite = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_LibelleSite.Tag).ToList();
                if (lstCentreSite.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
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
                CsCentre leCentre = (CsCentre)ctrs.MyObject;
                this.Txt_LibelleCentre.Text = leCentre.LIBELLE;
                this.Txt_LibelleCentre.Tag = leCentre;
                lProduitSelect = leCentre.LESPRODUITSDUSITE;

                this.Txt_Produit.Text = lProduitSelect.First().LIBELLE;
                this.Txt_Produit.Tag = lProduitSelect.First().CODE;
                this.btn_Produit.Tag = lProduitSelect.First().PK_ID ;
            }
            this.btn_Centre.IsEnabled = true ;
        }

        private void btn_Produit_Click(object sender, RoutedEventArgs e)
        {
            if (lProduitSelect != null && lProduitSelect.Count > 0)
            {
                this.btn_Produit.IsEnabled = false;
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
                this.btn_Produit .Tag = leProduit.PK_ID ;
            }
            this.btn_Produit.IsEnabled = true ;

        }


        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            //var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.Msg_ModificationFacture, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //w.OnMessageBoxClosed += (_, result) =>
            //{
            //    if (w.Result == MessageBoxResult.No)
            //        this.chk_ConsoSeul.IsChecked = true;
            //};
            //w.Show(); 
        }


        private void Btn_Recherche_Click(object sender, RoutedEventArgs e)
        {
           
                if (this.Txt_LibelleCentre.Tag == null)
                {
                  Message.ShowInformation("Sélectionner le centre","Facture Isole");
                  return ;
                }
                if (string.IsNullOrEmpty(this.txtClient.Text ))
                {
                  Message.ShowInformation("Saisir la reférence client","Facture Isole");
                  return ;
                }
                if (string.IsNullOrEmpty(this.Txt_PeriodeEnCour.Text))
                {
                    Message.ShowInformation("Saisir la période", "Facture Isole");
                    return;
                }

                CsCentre leCentreselect = (CsCentre)this.Txt_LibelleCentre.Tag;
                CsClient leClient = new CsClient();
                leClient.FK_IDCENTRE  = leCentreselect.PK_ID ;
                leClient.CENTRE = leCentreselect.CODE;
                leClient.REFCLIENT = this.txtClient.Text;
                leClient.PRODUIT = this.Txt_Produit.Tag.ToString();
                leClient.PERIODE = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_PeriodeEnCour.Text);
                leClient.FK_IDPRODUIT =(int)this.btn_Produit.Tag;
                
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
                        RetourneEvenementClient(leClient);
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
       private void RetourneEvenementClient(CsClient leClientRech)
       {
           AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
           service.RetourneEvenementClientCompleted += (s, args) =>
           {
               if (args != null && args.Cancelled)
                   return;
               if (args.Result == null)
               {
                   Message.ShowError(Langue.Msg_AbonnemtInexistant, Galatee.Silverlight.Resources.Facturation.Langue.LibelleModule);
                   return;
               }
               LstEvenement = args.Result;
               if (LstEvenement != null && LstEvenement.Count != 0)
               {
                   LstEvenement.ForEach(y => y.INDEXEVT = y.INDEXPRECEDENTEFACTURE);
                   LstEvenement.ForEach(y => y.CONSO = y.CONSOFACPRECEDENT);
                   LstEvenement.ForEach(y => y.PERIODE  = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM( this.Txt_PeriodeEnCour.Text) );
                   this.dataGrid1.ItemsSource = null;
                   this.dataGrid1.ItemsSource = LstEvenement;
                   if (LstEvenement != null && LstEvenement.Count != 0)
                       this.dataGrid1.SelectedItem = LstEvenement.First();
                   this.Txt_PeriodeEnCour.Focus();
                   VerifierEvtNonFacture(LstEvenement.First());
               }
               else
                   Message.ShowInformation("Aucun evenement trouvé pour ce client", "Erreur");
           };
           service.RetourneEvenementClientAsync(leClientRech);
           service.CloseAsync();
       
       }
       string IsEvtNonFactEnCour = "0";
       string  lotSupprimer = null ;
       private void VerifierEvtNonFacture(CsEvenement  leEvt)
       {
           AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
           service.VerifieEvenementNonFacturerAsync(leEvt);
           service.VerifieEvenementNonFacturerCompleted += (s, args) =>
           {
               if (args != null && args.Cancelled)
                   return;
               if (args.Result == null)
               {
                   Message.ShowError(Langue.Msg_AbonnemtInexistant, Galatee.Silverlight.Resources.Facturation.Langue.LibelleModule);
                   return;
               }

               CsEvenement lev = args.Result;
               if (leEvt.LOTRI != null &&  Facturation.ClasseMethodeGenerique.IsLotIsole(lev.LOTRI))
                   Message.ShowInformation("Un événement isolé non facturé existe sur ce client . " + "\n\r " + "Il sera mis a jour", "Erreur");

               if (leEvt.LOTRI != null && !Facturation.ClasseMethodeGenerique.IsLotIsole(lev.LOTRI))
               {
                   lotSupprimer = lev.LOTRI ;
                   Message.ShowInformation("Un événement non facturé existe sur ce client . " + "\n\r " + "Il sera supprimé par la facture isolé", "Erreur");
               }
           };
           service.CloseAsync();

       }

        void IsSaisieValider(CsEvenement LeEvenementSelect, List<CsCasind> LstCas)
        {
            CsCasind LeCasRecherche = LstCas.FirstOrDefault(p => p.CODE == LeEvenementSelect.CAS );
            if (LeCasRecherche == null)
            {
                Message.ShowInformation(Galatee.Silverlight.Resources.Index.Langue.msg_CasInexistant, "Erreur");
                //dataGrid1.IsEnabled = false;
                return;
            }
            else
            {
                // Saisie d'index
                if (LeCasRecherche.SAISIEINDEX == SessionObject.Enumere.CodeObligatoire)
                {
                    if (string.IsNullOrEmpty(this.Txt_IndexSaisi.Text) && string.IsNullOrEmpty(this.Txt_ConsomationSaisi.Text ))
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Index.Langue.msg_SaisiConsoInterdite, "Alert");
                        //dataGrid1.IsEnabled = false;
                        return;

                    }
                }
                if (LeCasRecherche.SAISIEINDEX == SessionObject.Enumere.CodeInterdit)
                {
                    if (!string.IsNullOrEmpty(this.Txt_IndexSaisi.Text))
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Index.Langue.msg_SaisiIndexInterdite, "Alert");
                        //dataGrid1.IsEnabled = false;
                        return;

                    }
                }
            }
            IsCasValider(LeEvenementSelect);
        }
        void BackToZero(int? indexSaisi, int? indexPrecedent)
        {
            try
            {
                if (indexSaisi < indexPrecedent)
                {
                    var ws = new MessageBoxControl.MessageBoxChildWindow("Information", Galatee.Silverlight.Resources.Index.Langue.msg_RetourAZero, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                    ws.OnMessageBoxClosed += (l, results) =>
                    {
                        if (ws.Result == MessageBoxResult.No)
                        {
                            this.Txt_CasEnCour.Text = "82";
                            this.Txt_CasEnCour.IsReadOnly = true;
                            this.Txt_IndexSaisi.Text = this.Txt_IndexSaisi.Text;
                            //this.Txt_ConsoEnCours.Text = IndexInit.ToString();
                        }
                        else if (ws.Result == MessageBoxResult.OK)
                        {
                            this.Txt_CasEnCour.Text = "10";
                            this.Txt_CasEnCour.IsReadOnly = true;
                            if (LeCompteurSelect.CADRAN == 0)
                            {
                                Message.ShowWarning(Langue.msgCadranIndeterminer, Langue.lbl_Menu);
                                this.Txt_IndexSaisi.Text = string.Empty;
                                this.Txt_Consomation.Text = string.Empty;
                                return;
                            }
                            else
                            {
                                int Roue = int.Parse(LeCompteurSelect.CADRAN.Value.ToString());
                                int initval = 9;
                                int? Indexmax = int.Parse(initval.ToString().PadLeft(Roue, '9'));
                                this.Txt_Consomation.Text = ((Indexmax - indexPrecedent) + (int.Parse(this.Txt_IndexSaisi.Text) + 1)).ToString();
                                this.Txt_IndexSaisi.Text = this.Txt_IndexSaisi.Text;
                            }
                        }
                    };
                    ws.Show();
                    return;
                }
                else
                {
                    this.Txt_CasEnCour.IsReadOnly = false;
                    this.Txt_CasEnCour.Text = string.Empty;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        void IsCasValider(CsEvenement LeEvenementSelect)
        {
            if (!string.IsNullOrEmpty(this.Txt_IndexSaisi.Text))
            {
                int? MoyenneComparaisonSup = (LeEvenementSelect.CONSOMOYENNEPRECEDENTEFACTURE == null ? 1 : LeEvenementSelect.CONSOMOYENNEPRECEDENTEFACTURE) * 2;
                int? MoyenneComparaisonInf = (int.Parse(this.Txt_IndexSaisi.Text) - (int.Parse(this.Txt_IndexFacture.Text))) * 2;
                if (LeEvenementSelect.CAS == "00" && MoyenneComparaisonInf < (LeEvenementSelect.CONSOMOYENNEPRECEDENTEFACTURE == null ? 1 : LeEvenementSelect.CONSOMOYENNEPRECEDENTEFACTURE)
                    && LeEvenementSelect.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageMaximetre
                    && LeEvenementSelect.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageHoraire)
                {
                    var ws = new MessageBoxControl.MessageBoxChildWindow("Information", Galatee.Silverlight.Resources.Index.Langue.msg_ConsoFaible, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                    ws.OnMessageBoxClosed += (l, results) =>
                    {
                        if (ws.Result == MessageBoxResult.No)
                        {
                            LeEvenementSelect.CAS = string.Empty;
                            LeEvenementSelect.INDEXEVT = null;
                            LeEvenementSelect.CONSO = null;
                            LeEvenementSelect.IsSaisi = false;
                            dataGrid1.IsEnabled = false;
                        }
                        else if (ws.Result == MessageBoxResult.OK)
                        {
                            LeEvenementSelect.CAS = "84";
                            LeEvenementSelect.ENQUETE = "E";

                            Galatee.Silverlight.ServiceAccueil.CsEvenement _LeEvenement = LstEvenementCree.FirstOrDefault(p => p.COMPTEUR == LeEvenementSelect.COMPTEUR);
                            if (_LeEvenement != null)
                                LstEvenementCree.Remove(LeEvenementSelect);
                            LstEvenementCree.Add(LeEvenementSelect);
                            LeEvenementSelect.IsSaisi = true;
                            CsEvenement lev = LstEvenement.FirstOrDefault(t => t.POINT == LeEvenementSelect.POINT);
                            if (lev != null)
                                lev.IsSaisi = true;
                            ValiderSaisie();
                        }

                    };
                    ws.Show();
                    return;
                }
                if (LeEvenementSelect.CAS == "00" && (int.Parse(this.Txt_IndexSaisi.Text) - (int.Parse(this.Txt_IndexFacture.Text))) > MoyenneComparaisonSup &&
                          LeEvenementSelect.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageMaximetre &&
                          LeEvenementSelect.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageHoraire)
                {
                    var ws = new MessageBoxControl.MessageBoxChildWindow("Information", Galatee.Silverlight.Resources.Index.Langue.msg_ConsoForte, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                    ws.OnMessageBoxClosed += (l, results) =>
                    {
                        if (ws.Result == MessageBoxResult.No)
                        {
                            LeEvenementSelect.CAS = string.Empty;
                            LeEvenementSelect.INDEXEVT = null;
                            LeEvenementSelect.CONSO = null;
                            LeEvenementSelect.IsSaisi = false;
                            dataGrid1.IsEnabled = false;
                        }
                        else if (ws.Result == MessageBoxResult.OK)
                        {
                            LeEvenementSelect.CAS = "83";
                            Galatee.Silverlight.ServiceAccueil.CsEvenement _LeEvenement = LstEvenementCree.FirstOrDefault(p => p.COMPTEUR == LeEvenementSelect.COMPTEUR);
                            if (_LeEvenement != null)
                                LstEvenementCree.Remove(LeEvenementSelect);
                            LstEvenementCree.Add(LeEvenementSelect);
                            LeEvenementSelect.IsSaisi = true;
                            //LeCompteurSelect.is = true;
                            CsEvenement lev = LstEvenement.FirstOrDefault(t => t.POINT == LeEvenementSelect.POINT);
                            if (lev != null)
                                lev.IsSaisi = true;
                            ValiderSaisie();

                        }
                    };
                    ws.Show();
                    return;
                }
            }
            if (LeCasRecherche.SAISIEINDEX == "I")
                LeEvenementSelect.INDEXEVT  = LeEvenementSelect.INDEXPRECEDENTEFACTURE ;
            Galatee.Silverlight.ServiceAccueil.CsEvenement _LEvenement = LstEvenementCree.FirstOrDefault(p => p.COMPTEUR == LeEvenementSelect.COMPTEUR );
            if (_LEvenement != null)
                LstEvenementCree.Remove(LeEvenementSelect);
            LstEvenementCree.Add(LeEvenementSelect);
            ValiderSaisie();
            //LeCompteurSelect.is = true;
            CsEvenement lev1 = LstEvenement.FirstOrDefault(t => t.POINT == LeEvenementSelect.POINT);
            if (lev1 != null)
                lev1.IsSaisi = true;

            this.dataGrid1.SelectedItem = LstEvenement.FirstOrDefault(t => t.IsSaisi  != true);
            this.Txt_IndexSaisi.Text = string.Empty;
            this.Txt_Consomation.Text = string.Empty;
            this.Txt_CasEnCour.Text = string.Empty;
            this.Txt_IndexNouv.Text = string.Empty;
        }
        private void Txt_IndexSaisi_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_IndexSaisi.Text))
            {
                Txt_IndexNouv.Text = this.Txt_IndexSaisi.Text;

                if (LeEvenementSelect.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageMaximetre)
                    Txt_Consomation.Text  = (int.Parse(this.Txt_IndexSaisi.Text) - LeEvenementSelect.INDEXEVT).ToString();
                else
                    Txt_Consomation.Text = this.Txt_IndexSaisi.Text;
            }
        }

        private void btnValiderSaisie_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Txt_CasEnCour.Text)) this.Txt_CasEnCour.Text = "00";
            {
                VerifieCas(true);
                if (!EstCasValider)
                    return;
                if (!string.IsNullOrEmpty(this.Txt_IndexSaisi.Text))
                {
                    Txt_IndexNouv.Text = this.Txt_IndexSaisi.Text;

                    if (LeEvenementSelect.TYPECOMPTEUR != SessionObject.Enumere.TypeComptageMaximetre)
                        LeEvenementSelect.CONSO = int.Parse(this.Txt_IndexSaisi.Text) - LeEvenementSelect.INDEXEVT;
                    else
                        LeEvenementSelect.CONSO = int.Parse(this.Txt_IndexSaisi.Text);

                    LeEvenementSelect.INDEXEVT = int.Parse(this.Txt_IndexSaisi.Text);
                }
            }
            CreeEvenement(LeEvenementSelect);
        }


        bool EstCasValider = true;
        CsCasind LeCasRecherche = new CsCasind();
        private void VerifieCas(bool isvalidation)
        {
            EstCasValider = true;
            // Saisie d'index
            if (string.IsNullOrEmpty(this.Txt_CasEnCour.Text)) this.Txt_CasEnCour.Text = "00";
            {
                LeCasRecherche = LsDesCas.FirstOrDefault(t => t.CODE == this.Txt_CasEnCour.Text);
                if (LeCasRecherche == null || string.IsNullOrEmpty(LeCasRecherche.CODE))
                {
                    Message.ShowInformation("Cas inexistant", "Attention");
                    EstCasValider = false;
                    return;
                }
            }
            if (LeCasRecherche.SAISIEINDEX == SessionObject.Enumere.CodeObligatoire)
            {
                if (string.IsNullOrEmpty(this.Txt_IndexSaisi.Text) && string.IsNullOrEmpty(this.Txt_ConsomationSaisi.Text ))
                {
                    if (isvalidation)
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Index.Langue.msg_SaisiIndexObligatoire, "Attention");
                        dataGrid1.IsEnabled = false;
                    }
                    EstCasValider = false;
                    return;

                }

            }

            if (LeCasRecherche.SAISIEINDEX == SessionObject.Enumere.CodeInterdit)
            {
                if (!string.IsNullOrEmpty(this.Txt_IndexSaisi.Text))
                {
                    if (int.Parse(this.Txt_IndexSaisi.Text) != LeEvenementSelect.INDEXPRECEDENTEFACTURE)
                    {
                        if (isvalidation)
                        {
                            Message.ShowInformation(Galatee.Silverlight.Resources.Index.Langue.msg_SaisiIndexInterdite, "Attention");
                            dataGrid1.IsEnabled = false;
                            LeEvenementSelect.INDEXEVT = null;
                            this.Txt_IndexSaisi.Text = string.Empty;
                            this.Txt_IndexSaisi.IsReadOnly = true;

                        }
                        EstCasValider = false;
                        return;
                    }
                }
                else
                    this.Txt_IndexSaisi.IsReadOnly = true;
            }
            







            /** ZEG 15/09/2017 **/

            // Saisie de la consomation
            if (LeCasRecherche.SAISIECONSO == SessionObject.Enumere.CodeObligatoire)
            {
                this.Txt_ConsomationSaisi.IsEnabled = true;
                this.Txt_ConsomationSaisi.IsReadOnly = false;

                if (string.IsNullOrEmpty(this.Txt_ConsomationSaisi.Text))
                {

                    if (isvalidation)
                    {
                        Message.ShowInformation("La saisie de la consommation est obligatoire", "Attention");
                        dataGrid1.IsEnabled = false;
                    }
                    EstCasValider = false;
                    return;

                }
                else
                    LeEvenementSelect.CONSO = int.Parse(this.Txt_ConsomationSaisi.Text); //ZEG 15/09/2017
            }
            else if (LeCasRecherche.SAISIECONSO == SessionObject.Enumere.CodeInterdit)
            {
                if (LeEvenementSelect.TYPECOMPTEUR == SessionObject.Enumere.TypeComptageHoraire &&
                    !string.IsNullOrEmpty(this.Txt_CasEnCour.Text) &&
                    this.Txt_CasEnCour.Text == "10")
                {
                    if (string.IsNullOrEmpty(this.Txt_ConsomationSaisi.Text.ToString()))
                        this.Txt_ConsomationSaisi.IsReadOnly = false;
                    return;
                }
                if (!string.IsNullOrEmpty(this.Txt_ConsomationSaisi.Text.ToString()))
                {
                    if (isvalidation)
                    {
                        Message.ShowInformation("La saisie de la consommation est interdite", "Attention");
                        dataGrid1.IsEnabled = false;
                        this.Txt_ConsomationSaisi.Text = string.Empty;
                        this.Txt_ConsomationSaisi.IsReadOnly = true;
                    }
                    EstCasValider = false;

                    return;
                }
                else
                    this.Txt_ConsomationSaisi.IsReadOnly = true;
            }

            /** **/





            if (LeCasRecherche.CODE != "05" && LeCasRecherche.CODE != "10" &&
                LeCasRecherche.CODE != "13" && LeCasRecherche.CODE != "00" &&
                (!string.IsNullOrEmpty(this.Txt_IndexSaisi.Text) && int.Parse(this.Txt_IndexSaisi.Text) < LeEvenementSelect.INDEXPRECEDENTEFACTURE))
            {
                if (isvalidation)
                {
                    Message.ShowInformation("Ce cas ne correspond pas à l'index saisi", "Attention");
                    dataGrid1.IsEnabled = false;
                }
                EstCasValider = false;
                return;
            }
            if (LeCasRecherche.CODE == "05" &&
               (!string.IsNullOrEmpty(this.Txt_IndexSaisi.Text) && int.Parse(this.Txt_IndexSaisi.Text) > LeEvenementSelect.INDEXPRECEDENTEFACTURE))
            {
                if (isvalidation)
                {
                    Message.ShowInformation("Ce cas ne correspond pas à l'index saisi", "Attention");
                    dataGrid1.IsEnabled = false;
                }
                EstCasValider = false;
                return;
            }
            if (LeCasRecherche.CODE == "05" && !string.IsNullOrEmpty(Txt_IndexSaisi.Text))
            {
                if (int.Parse(Txt_IndexSaisi.Text) >= LeEvenementSelect.INDEXPRECEDENTEFACTURE)
                {
                    if (isvalidation)
                    {
                        Message.ShowInformation("Ce cas ne correspond pas à l'index saisi", "Attention");
                        dataGrid1.IsEnabled = false;
                    }
                    EstCasValider = false;
                    return;
                }
            }

          
        }
        private void RetourneEvenement(CsEvenement leCompteur)
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneEvenementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result.Count != 0)
                    {
                        UcHistoriqueEvenement ctrl = new UcHistoriqueEvenement(args.Result.OrderBy(t=>t.DATEEVT).ToList());
                        ctrl.Show();
                    }
                };
                service.RetourneEvenementAsync(leCompteur.FK_IDCENTRE, leCompteur.CENTRE, leCompteur.CLIENT, leCompteur.ORDRE, leCompteur.PRODUIT, leCompteur.POINT);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnHistoriqueEvenement_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.ItemsSource != null )
            {
                if (dataGrid1.SelectedItem != null )
                {
                    CsEvenement leEvt = (CsEvenement)dataGrid1.SelectedItem;
                    RetourneEvenement(leEvt);
                }
            }
        }

        private void ChildWindow_KeyDown_1(object sender, KeyEventArgs e)
        {
           if (e.Key.Equals(Key.Enter))
            {

                OKButton_Click(null, null);
            }
        }

        private void Txt_ConsomationSaisi_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_ConsomationSaisi.Text))
            {
                this.Txt_IndexNouv.Text = string.Empty;
            }
        }
    }
}


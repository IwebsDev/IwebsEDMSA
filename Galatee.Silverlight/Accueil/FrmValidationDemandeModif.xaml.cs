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
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmValidationDemandeModif : ChildWindow
    {

        CsCentre LeCentreSelect = new CsCentre();
        CsProduit LeProduitSelect = new CsProduit();

        List<CsCentre> LstCentre = new List<CsCentre>();
        List<CsProduit> ListeDesProduitDuSite = new List<CsProduit>();

        public CsDemande LaDemande = new CsDemande();
        List<CsTdem > lstTypeDemandeAfficher = new List<CsTdem >();
        List<CsDemande> lstDemande = new List<CsDemande>();
        List<CsDemandeBase> lesDemande = new List<CsDemandeBase >();

        public FrmValidationDemandeModif()
        {
            InitializeComponent();
            ChargerOptionDemande();
            ChargerDonneeDuSite();
            ChargerListeDeProduit();

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChargerOptionDemande()
        {
            try
            {
                if (SessionObject.LstTypeDemande != null && SessionObject.LstTypeDemande.Count != 0)
                {
                    CsTdem leType = new CsTdem();
                    leType = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ModificationAbonnement);
                    lstTypeDemandeAfficher.Add(leType);
                    leType = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ModificationAdresse);
                    lstTypeDemandeAfficher.Add(leType);
                    leType = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ModificationBranchement);
                    lstTypeDemandeAfficher.Add(leType);
                    leType = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ModificationClient);
                    lstTypeDemandeAfficher.Add(leType);
                    leType = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ModificationCompteur);
                    lstTypeDemandeAfficher.Add(leType);
                    cbo_TypeDemande.ItemsSource = null;
                    cbo_TypeDemande.ItemsSource = lstTypeDemandeAfficher;
                    cbo_TypeDemande.DisplayMemberPath = "LIBELLE";
                
                }
                else
                {
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service1.RetourneOptionDemandeCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        SessionObject.LstTypeDemande = res.Result;
                        if (SessionObject.LstTypeDemande != null)
                        {
                            CsTdem leType = new CsTdem();
                            leType = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ModificationAbonnement);
                            lstTypeDemandeAfficher.Add(leType);
                            leType = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ModificationAdresse);
                            lstTypeDemandeAfficher.Add(leType);
                            leType = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ModificationBranchement);
                            lstTypeDemandeAfficher.Add(leType);
                            leType = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ModificationClient);
                            lstTypeDemandeAfficher.Add(leType);
                            leType = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ModificationCompteur);
                            lstTypeDemandeAfficher.Add(leType);
                            cbo_TypeDemande.ItemsSource = null;
                            cbo_TypeDemande.ItemsSource = lstTypeDemandeAfficher;
                            cbo_TypeDemande.DisplayMemberPath = "LIBELLE";

                        }
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
        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
            {
                CsCentre _LeCentre = ClasseMEthodeGenerique.RetourneObjectFromList<CsCentre>(LstCentre, this.Txt_CodeCentre.Text, "CODE");
                if (_LeCentre != null)
                {
                    this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_CodeCentre.Focus();
                    };
                    w.Show();
                }
            }
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentre.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(LstCentre);
                    UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", Langue.lbl_ListeCentre);
                    ctr.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }
        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            this.btn_Centre.IsEnabled = true;
            LeCentreSelect = new CsCentre();
            UcListeGenerique ctrs = sender as UcListeGenerique;
            CsCentre leCentre = (CsCentre)ctrs.MyObject;
            LeCentreSelect = leCentre;
            this.Txt_CodeCentre.Text = leCentre.CODE;
            //this.Txt_NumDemande.Text = leCentre.CODE + leCentre.NUMDEM;
        }

        private void btn_Produit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<object> _LstProduit = ClasseMEthodeGenerique.RetourneListeObjet(ListeDesProduitDuSite);
                UcListeGenerique ctr = new UcListeGenerique(_LstProduit, "CODE", "LIBELLE", Langue.lbl_ListeProduit);
                ctr.Closed += new EventHandler(galatee_OkClickedProduit);
                ctr.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        void galatee_OkClickedProduit(object sender, EventArgs e)
        {
            try
            {

                UcListeGenerique ctrs = sender as UcListeGenerique;
                LeProduitSelect = (CsProduit)ctrs.MyObject;
                this.Txt_CodeProduit.Text = LeProduitSelect.CODE;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void Txt_CodeProduit_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
                {
                    CsProduit _LeProduitSelect = ClasseMEthodeGenerique.RetourneObjectFromList(ListeDesProduitDuSite, this.Txt_CodeProduit.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeProduitSelect.LIBELLE))
                        this.Txt_LibelleProduit.Text = _LeProduitSelect.LIBELLE;
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeProduit.Focus();
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }
        private void btn_Rechercher_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string _Centre = string.IsNullOrEmpty(this.Txt_CodeCentre.Text) ? string.Empty : this.Txt_CodeCentre.Text;
                string _Client = string.IsNullOrEmpty(this.Txt_Client.Text) ? string.Empty : this.Txt_Client.Text;
                string _Ordre = string.IsNullOrEmpty(this.Txt_Ordre.Text) ? string.Empty : this.Txt_Ordre.Text;
                string _NumDemande = string.IsNullOrEmpty(this.Txt_NumDemande.Text) ? string.Empty : this.Txt_NumDemande.Text;
                string _Produit = string.IsNullOrEmpty(this.Txt_CodeProduit.Text) ? string.Empty : this.Txt_CodeProduit.Text;
                string _typeDemande = this.cbo_TypeDemande.SelectedItem == null ? string.Empty : ((CsTdem)this.cbo_TypeDemande.SelectedItem).CODE;
                DateTime? _DateDemande = null;
                if (!string.IsNullOrEmpty(this.dtp_DateDemande.Text)) 
                   _DateDemande = Convert.ToDateTime(this.dtp_DateDemande.Text);

                RetourneDemande(_Centre, _NumDemande, _typeDemande, _Produit,  _DateDemande);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        private void ChargerDonneeDuSite()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
   

                    if (LstCentre != null)
                    {
                        List<CsCentre> _LstCentre = LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstCentre.Count == 1)
                        {
                            this.Txt_CodeCentre.Text = _LstCentre[0].CODE;
                            this.Txt_LibelleCentre.Text = _LstCentre[0].LIBELLE;
                        }
                        else
                        {
                            //CsCentre _LeCentre = LstCentre.FirstOrDefault(p => p.CODE == LaDemande.LaDemande.CENTRE);
                            //if (_LeCentre != null)
                            //{
                            //    this.Txt_CodeCentre.Text = LaDemande.LaDemande.CENTRE;
                            //    this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                            //    this.btn_Centre.IsEnabled = false;
                            //    this.Txt_CodeCentre.IsReadOnly = true;
                            //}
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false );
                service.CloseAsync();
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
                if (SessionObject.ListeDesProduit.Count != 0)
                {
                    ListeDesProduitDuSite = SessionObject.ListeDesProduit;
                    if (ListeDesProduitDuSite != null)
                    {
                        if (ListeDesProduitDuSite.Count == 1)
                        {
                            this.Txt_CodeProduit.Text = ListeDesProduitDuSite[0].CODE;
                            this.Txt_LibelleProduit.Text = ListeDesProduitDuSite[0].LIBELLE;
                            this.btn_Produit.IsEnabled = false;
                        }
                        else
                        {
                            //CsProduit _LeProduit = ListeDesProduitDuSite.FirstOrDefault(p => p.CODE == LaDemande.LaDemande.PRODUIT);
                            //if (_LeProduit != null)
                            //{
                            //    this.Txt_CodeProduit.Text = LaDemande.LaDemande.PRODUIT;
                            //    this.Txt_LibelleProduit.Text = _LeProduit.LIBELLE;
                            //    this.btn_Produit.IsEnabled = false;
                            //    this.Txt_CodeProduit.IsReadOnly = true;
                            //}
                        }
                    }

                }
                else
                {
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service1.ListeDesProduitCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        SessionObject.ListeDesProduit = res.Result;
                        ListeDesProduitDuSite = SessionObject.ListeDesProduit;
                        if (ListeDesProduitDuSite != null)
                        {
                            if (ListeDesProduitDuSite.Count == 1)
                            {
                                this.Txt_CodeProduit.Text = ListeDesProduitDuSite[0].CODE;
                                this.Txt_LibelleProduit.Text = ListeDesProduitDuSite[0].LIBELLE;
                                this.btn_Produit.IsEnabled = false;
                            }
                            else
                            {
                                //CsProduit _LeProduit = ListeDesProduitDuSite.FirstOrDefault(p => p.CODE == LaDemande.LaDemande.PRODUIT);
                                //if (_LeProduit != null)
                                //{
                                //    this.Txt_CodeProduit.Text = LaDemande.LaDemande.PRODUIT;
                                //    this.Txt_LibelleProduit.Text = _LeProduit.LIBELLE;
                                //    this.btn_Produit.IsEnabled = false;
                                //    this.Txt_CodeProduit.IsReadOnly = true;
                                //}
                            }
                        }
                    };
                    service1.ListeDesProduitAsync();
                    service1.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void RetourneDemande(string centre, string numdem, string LstTdem, string produit, DateTime? datedebut)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);

            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneListeDemandeModificationCompleted  += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    lesDemande = args.Result;
                    if (lesDemande != null)
                    {
                        foreach (CsDemandeBase  item in lesDemande)
                            item.LIBELLE = SessionObject.LstTypeDemande.FirstOrDefault(p => p.CODE == item.TYPEDEMANDE).LIBELLE;


                        List<Galatee.Silverlight.ServiceAccueil.CsCentre> lstCentreProfil = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);

                        List<int> lstCentreHabil = new List<int>();
                        foreach (var item in lstCentreProfil)
                            lstCentreHabil.Add(item.PK_ID);
                        List<CsDemandeBase> _lstDemandeAffiche = lesDemande.Where(t => lstCentreHabil.Contains(t.FK_IDCENTRE)).ToList();

                        this.dtg_Demande.ItemsSource = null;
                        this.dtg_Demande.ItemsSource = _lstDemandeAffiche;
                        datapager.Source = _lstDemandeAffiche;
                    }
                    LoadingManager.EndLoading(res);

                };
                service.RetourneListeDemandeModificationAsync(centre,numdem,LstTdem ,produit ,datedebut,UserConnecte.matricule  );
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }
        }

        private void RetourneDetailDemande(CsDemandeBase laDemandeSelect)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);

            try
            {
                CsDemande leDetailDemande = new CsDemande();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneDetailDemandeCompleted  += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    leDetailDemande = args.Result;
                    if (leDetailDemande != null)
                    {
                        if (leDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ModificationAbonnement)
                        {
                            //FrmValidationModificationAbonnement frm = new FrmValidationModificationAbonnement(leDetailDemande);
                            //frm.Closed += new EventHandler(galatee_RefraicheListe);
                            //frm.Show();
                        }
                        if (leDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ModificationAdresse)
                        {
                            FrmValidationModificationAdresse frm = new FrmValidationModificationAdresse(leDetailDemande);
                            frm.Closed += new EventHandler(galatee_RefraicheListe);

                            frm.Show();
                        }
                        if (leDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ModificationBranchement)
                        {
                            //FrmValidationModificationBranchement frm = new FrmValidationModificationBranchement(leDetailDemande);
                            //frm.Closed += new EventHandler(galatee_RefraicheListe);

                            //frm.Show();
                        }
                        if (leDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ModificationClient)
                        {
                            //FrmValidationModificationClient frm = new FrmValidationModificationClient(leDetailDemande);
                            //frm.Closed += new EventHandler(galatee_RefraicheListe);

                            //frm.Show();
                        }
                        if (leDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ModificationCompteur )
                        {
                            //FrmValidationModificationCompteur frm = new FrmValidationModificationCompteur(leDetailDemande);
                            //frm.Closed += new EventHandler(galatee_RefraicheListe);

                            //frm.Show();
                        }
                    }
                    LoadingManager.EndLoading(res);

                };
                service.RetourneDetailDemandeAsync (laDemandeSelect );
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }
        }

        private void dtg_Demande_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.dtg_Demande.SelectedItem != null)
                RetourneDetailDemande((CsDemandeBase)this.dtg_Demande.SelectedItem);
        }

        private void ChildWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            RetourneDemande(string.Empty, string.Empty, string.Empty, string.Empty,null );
        }
        private void galatee_RefraicheListe(object sender, EventArgs e)
        {
            RetourneDemande(string.Empty, string.Empty, string.Empty, string.Empty, null);
        }
    }
}


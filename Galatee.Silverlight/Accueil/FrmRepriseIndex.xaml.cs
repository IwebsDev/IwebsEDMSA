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
using System.Collections.ObjectModel;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Classes;
using Galatee.Silverlight.ServiceAccueil;
//using Galatee.Silverlight.serviceWeb;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmRepriseIndex : ChildWindow
    {
        public FrmRepriseIndex()
        {
            InitializeComponent();
            if (LaDemande == null) LaDemande = new CsDemande();
            if (LaDemande.LaDemande == null) LaDemande.LaDemande = new CsDemandeBase();
            if (LaDemande.LstEvenement == null) LaDemande.LstEvenement = new List<CsEvenement>();

            this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
            this.Txt_CodeProduit.MaxLength = SessionObject.Enumere.TailleCodeProduit;
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
           
            this.btn_Produit.IsEnabled = false ;
            LaDemande.LaDemande.TYPEDEMANDE  = SessionObject.Enumere.ReprisIndex ;
            if (SessionObject.ListeDesProduit.Count != 0)
                this.btn_Produit.IsEnabled = true;

            ChargerDonneeDuSite();
            ChargerListeDeProduit();
        }
       public FrmRepriseIndex(string _TypeDemande)
        {
            InitializeComponent();
            TypeDemande = _TypeDemande;
            this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
            this.Txt_CodeProduit .MaxLength = SessionObject.Enumere.TailleCodeProduit ;
            this.Txt_Client .MaxLength = SessionObject.Enumere.TailleClient ;

            
            if (SessionObject.ListeDesProduit .Count != 0)
                this.btn_Produit .IsEnabled = true;

            if (LaDemande == null)
            {
                LaDemande = new CsDemande();
                LaDemande.LaDemande = new CsDemandeBase();
            }
            ChargerDonneeDuSite();
            ChargerListeDeProduit();
        }

       List<CsCentre> LstCentre = new List<CsCentre>();
        CsDemande LaDemande = new CsDemande();
        CsCanalisation CanalisationClientSelectionner = new CsCanalisation();

        List<CParametre> LstBranche = new List<CParametre>();
        List<CParametre> LstQuartier = new List<CParametre>();
        CsEvenement LsDernierEvenement = new CsEvenement();
        CsClient LeClientRecherche = new CsClient();
        List<CsProduit> ListeDesProduitDuSite;
        string OrdreMax;
        string TypeDemande;
        //private void chargerDonneeDuSite()
        //{
        //    int res = LoadingManager.BeginLoading(Langue.En_Cours);
        //    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
        //    service.ListeDesDonneesDesSiteCompleted += (s, args) =>
        //    {
        //        if (args != null && args.Cancelled)
        //            return;
        //        LstCentre.AddRange(args.Result);
        //        this.btn_Centre.IsEnabled = true ;
        //        if (LstCentre.Count == 1)
        //        {
        //            this.Txt_CodeCentre.Text = LstCentre[0].CODE ;
        //            this.Txt_LibelleCentre.Text = LstCentre[0].LIBELLE  ;
        //            this.Txt_NumDemande.Text = LstCentre[0].CODE  + LstCentre[0].NUMDEM.ToString().PadLeft(10, '0');
        //            //LstDeProduit = RetourneListeDeProduitDuSite(LstCentre[0]);

        //            this.btn_Centre.IsEnabled = false;
        //        }
        //        LoadingManager.EndLoading(res);

        //    };
        //    service.ListeDesDonneesDesSiteAsync(false );
        //    service.CloseAsync();

        //}
        List<CsSite> lstSite = new List<CsSite>();
        void ChargerDonneeDuSite()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<CsSite> _LstSite = lstSite.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = _LstSite[0].CODESITE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.IsReadOnly = true;
                        }
                    }
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
                            CsCentre _LeCentre = new CsCentre();
                            if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text))
                                _LeCentre = LstCentre.FirstOrDefault(p => p.CODE == this.Txt_CodeCentre.Text);
                            if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                            {
                                this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                                this.Txt_CodeCentre.IsReadOnly = true;
                            }
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(true);
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
                    }
                }
                else
                {
                    int res1 = LoadingManager.BeginLoading(Langue.En_Cours);
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service1.ListeDesProduitCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        SessionObject.ListeDesProduit = res.Result;
                        ListeDesProduitDuSite = SessionObject.ListeDesProduit;
                        this.btn_Produit.IsEnabled = true ;
                        if (ListeDesProduitDuSite != null)
                        {
                            if (ListeDesProduitDuSite.Count == 1 )
                            {
                                this.Txt_CodeProduit.Text = ListeDesProduitDuSite[0].CODE;
                                this.Txt_LibelleProduit.Text = ListeDesProduitDuSite[0].LIBELLE;
                                this.btn_Produit.IsEnabled = false;
                            }
                       
                        }
                        LoadingManager.EndLoading(res1);
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
                CsProduit _LeProduitSelect = new CsProduit();
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    _LeProduitSelect = (CsProduit)ctrs.MyObject;
                    this.Txt_CodeProduit.Text = _LeProduitSelect.CODE;
                }
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
                    LaDemande.LaDemande.PRODUIT = this.Txt_CodeProduit.Text;
                    CsProduit _LeProduitSelect = ClasseMEthodeGenerique.RetourneObjectFromList(ListeDesProduitDuSite, this.Txt_CodeProduit.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeProduitSelect.LIBELLE))
                    {
                        this.Txt_LibelleProduit.Text = _LeProduitSelect.LIBELLE;
                        LaDemande.LaDemande.FK_IDPRODUIT = _LeProduitSelect.PK_ID;
                    }
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

        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite.Count > 0)
                {
                    this.btn_Site.IsEnabled = false;
                    List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODESITE", "LIBELLE", "Liste");
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
                this.Txt_CodeSite.Text = leSite.CODESITE;
                this.LstCentre = LstCentre.Where(t => t.FK_IDCODESITE == leSite.PK_ID).ToList();
               
            }


        }

        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
            {
                CsSite _LeSite = ClasseMEthodeGenerique.RetourneObjectFromList<CsSite>(lstSite, this.Txt_CodeSite.Text, "CODESITE");
                if (_LeSite != null && !string.IsNullOrEmpty(_LeSite.CODESITE))
                    this.Txt_LibelleSite.Text = _LeSite.LIBELLE;
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
        CsCentre LeCentreSelect = new CsCentre();
        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            this.btn_Centre.IsEnabled = true;
            LeCentreSelect = new CsCentre();
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsCentre leCentre = (CsCentre)ctrs.MyObject;
                LeCentreSelect = leCentre;
                this.Txt_CodeCentre.Text = leCentre.CODE;

                string numIncrementiel = LeCentreSelect.NUMDEM.ToString();
                if (LeCentreSelect.NUMDEM.ToString().Length >= 10)
                    numIncrementiel = LeCentreSelect.NUMDEM.ToString().Substring(numIncrementiel.Length - 9, 9);
                this.Txt_NumDemande.Text = LeCentreSelect.CODE + numIncrementiel.PadLeft(10, '0');
                LaDemande.LaDemande.FK_IDCENTRE = LeCentreSelect.PK_ID;

            }
        }
        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre && LstCentre != null && LstCentre.Count != 0)
            {
                CsCentre _LeCentre = ClasseMEthodeGenerique.RetourneObjectFromList<CsCentre>(LstCentre, this.Txt_CodeCentre.Text, "CODE");
                if (_LeCentre != null)
                {
                    this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                    this.Txt_NumDemande.Text = _LeCentre.CODE + _LeCentre.NUMDEM.ToString().PadLeft(10, '0');
                    LaDemande.LaDemande.CENTRE = _LeCentre.CODE; ;
                    LaDemande.LaDemande.FK_IDCENTRE = _LeCentre.PK_ID;
                    LaDemande.LaDemande.NUMDEM = this.Txt_NumDemande.Text;
                    LaDemande.LaDemande.MATRICULE = UserConnecte.matricule;
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

        private void ValidationDemande(CsDemande _lademande)
        {
            LaDemande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusPriseEnCompte;
            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service1.ValiderDemandeCompleted += (sr, res) =>
            {
                string Messages = string.Empty;
                Messages = Langue.MsgOperationTerminee;
                Message.ShowInformation(Messages, Langue.lbl_Menu);
                this.DialogResult = false;

            };
            service1.ValiderDemandeAsync(_lademande);
            service1.CloseAsync();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EnregisterDemande();
            if (!string.IsNullOrEmpty(Txt_NouvIndex.Text ))
            ValidationDemande(LaDemande);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Txt_Client_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_Client.Text.Length == SessionObject.Enumere.TailleClient)
            {
                LaDemande.LaDemande.CLIENT = this.Txt_Client.Text;
                retourneOrdreMax(LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT , LaDemande.LaDemande.PRODUIT );
            }
        }
        private void retourneOrdreMax(string centre, string client,string produit)
        {
            int res1 = LoadingManager.BeginLoading(Langue.En_Cours);
            try
            {

                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service1.RetourneOrdreMaxCompleted += (se, argss) =>
                {
                    if (argss != null && argss.Cancelled)
                        return;
                    OrdreMax = argss.Result;
                    if (OrdreMax != string.Empty)
                    {
                        LaDemande.LaDemande.ORDRE = OrdreMax;
                        RetourneInfoCanalisation(LaDemande.LaDemande.FK_IDCENTRE ,LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.PRODUIT, null);
                        RetourneInfoClient(LaDemande.LaDemande.FK_IDCENTRE,LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE);
                    }

                };
                service1.RetourneOrdreMaxAsync(centre, client, produit);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                LoadingManager.EndLoading(res1);
            }

        }

        private void RetourneInfoClient(int fk_idcentre,string Centre,string Client,string Ordre)
        {
            int res1 = LoadingManager.BeginLoading(Langue.En_Cours);
            try
            {
                LeClientRecherche = new CsClient();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneClientCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    LeClientRecherche = args.Result;
                    if (LeClientRecherche != null)
                        this.Txt_NomAbon.Text = string.IsNullOrEmpty(LeClientRecherche.NOMABON) ? string.Empty : LeClientRecherche.NOMABON;
                };
                service.RetourneClientAsync(fk_idcentre,Centre, Client, Ordre);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                LoadingManager.EndLoading(res1);
            }
        }
        private void RetourneInfoDernierEvenemt(int fk_idcentre,string centre, string client, string ordre, string produit,int ? point)
        {
            try
            {
                LsDernierEvenement = new CsEvenement();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneDernierEvenementFacturerCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result != null)
                    {
                        LsDernierEvenement = args.Result.FirstOrDefault(p => p.PRODUIT == produit);
                        if (LsDernierEvenement != null)
                            this.Txt_AncIndex.Text = LsDernierEvenement.INDEXEVT.ToString();
                    }
                };
                service.RetourneDernierEvenementFacturerAsync(fk_idcentre,centre, client, ordre, produit, point);
                service.CloseAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RetourneInfoCanalisation(int fk_idcentre, string centre, string client,string produit,int? point)
        {
            int res1 = LoadingManager.BeginLoading(Langue.En_Cours);
            try
            {
                List<CsCanalisation> CanalisationClientRecherche = new List<CsCanalisation>();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneCanalisationCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    CanalisationClientRecherche = args.Result;
                    RemplireComboBox(CanalisationClientRecherche);
                };
                service.RetourneCanalisationAsync(fk_idcentre,centre, client, produit, point);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                LoadingManager.EndLoading(res1);
            }
        }
        private void RemplireComboBox(List<CsCanalisation> ListCompteurInit)
        {
            List<CsCanalisation> ListCompteurFinal = new List<CsCanalisation>();
            foreach (CsCanalisation item in ListCompteurInit)
            {
                CsProduit LeProduit = ListeDesProduitDuSite.FirstOrDefault(p => p.CODE == item.PRODUIT);
                if (LeProduit != null)
                    item.LIBELLEPRODUIT = LeProduit.LIBELLE + "   " + item.NUMERO;

                if (item.ETATCOMPT == SessionObject.Enumere.CompteurActifValeur)
                    ListCompteurFinal.Add(item);

            }
            Cbo_Compteur.ItemsSource = null;
            Cbo_Compteur.ItemsSource = ListCompteurFinal;
            Cbo_Compteur.DisplayMemberPath = "LIBELLEPRODUIT";
            if (ListCompteurFinal != null && ListCompteurFinal.Count != 0)
                Cbo_Compteur.SelectedItem = ListCompteurFinal[0];
            CanalisationClientSelectionner = (CsCanalisation)Cbo_Compteur.SelectedItem;

        }
        void Translate()
        {
            this.lbl_NumDemande.Content = Langue.lbl_NumeroDemande;
            this.lbl_Centre.Content = Langue.lbl_center;
            this.lbl_Adresse.Content = Langue.lbl_adresse;
            this.lbl_Compteur.Content = Langue.lbl_Compteur;
            this.lbl_Nom.Content = Langue.lbl_Nom;
            this.lbl_AncienneConsult.Content = Langue.lbl_AncienneConsult;
            this.lbl_NouvelleConsult.Content = Langue.lbl_NumeroDemande;
        }

        private void Cbo_Compteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CanalisationClientSelectionner = (CsCanalisation)Cbo_Compteur.SelectedItem;
            RetourneInfoDernierEvenemt(LaDemande.LaDemande.FK_IDCENTRE ,LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE, LaDemande.LaDemande.PRODUIT, CanalisationClientSelectionner.POINT);
        }

        private void Txt_NouvIndex_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void EnregisterDemande()
        {
            if (!string.IsNullOrEmpty(Txt_NouvIndex.Text))
            {
                LsDernierEvenement.INDEXEVT = int.Parse(Txt_NouvIndex.Text);
                LsDernierEvenement.NUMDEM = this.Txt_NumDemande.Text;
                LsDernierEvenement.USERMODIFICATION = UserConnecte.matricule;
                LsDernierEvenement.DATEMODIFICATION = System.DateTime.Now;

                LaDemande.LaDemande.DATECREATION = System.DateTime.Now;
                LaDemande.LaDemande.DATEMODIFICATION  = System.DateTime.Now;
                LaDemande.LaDemande.USERCREATION = UserConnecte.matricule;
                LaDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                LaDemande.LstEvenement.Add(LsDernierEvenement);
             
            }
        }
    }
}


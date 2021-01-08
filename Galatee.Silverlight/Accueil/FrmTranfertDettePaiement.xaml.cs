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
using System.Collections.ObjectModel;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.Shared;


namespace Galatee.Silverlight.Accueil
{
    public partial class FrmTranfertDettePaiement : ChildWindow
    {
        #region Variable
        private string Orientation;
        CsDemande Lademande = new CsDemande();
        bool IsEnregistre = false;
        string TypeDemande = string.Empty;
        string motifRejet = string.Empty;

        #region Listes
        List<CsFermable> LstFermable;
        List<CsDenomination> LstCivilite;
        List<CsRegCli> LstCodeRegroupement;
        List<CsCategorieClient> LstCategorie;
        List<CsNatureClient> LstNatureClient;
        List<CsNationalite> LstDesNationalites;
        List<CsCodeConsomateur> LstCodeConsomateur;
        List<CsModepaiement> LstDesModePaiement = new List<CsModepaiement>();
        List<CsCentre> LstCentre = new List<CsCentre>();


        #endregion

        #region Objets Globaux

        CsClient LeClient = new CsClient();
        CsClient LeClientRecherche = new CsClient();
        CsDenomination lacivilite = new CsDenomination();
        public CsDemande LaDemande = new CsDemande();
        CsCentre LeCentreSelect = new CsCentre();
        bool IsUpate = false;

        #endregion
        #endregion

        public FrmTranfertDettePaiement()
        {
            InitializeComponent();
            Decharger.Content = "<";
            DechargerTout.Content = "<<";
            ChargerDonneeDuSite();
            Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
            Txt_Ordre.MaxLength = SessionObject.Enumere.TailleOrdre;
            Txt_Client2.MaxLength = SessionObject.Enumere.TailleClient;
            Txt_Ordre2.MaxLength = SessionObject.Enumere.TailleOrdre;
        }

        public FrmTranfertDettePaiement(string Type)
        {
            InitializeComponent();
            Decharger.Content = "<";
            DechargerTout.Content = "<<";
            this.Orientation = Type;
            ChargerDonneeDuSite();
            Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
            Txt_Ordre.MaxLength = SessionObject.Enumere.TailleOrdre;
            Txt_Client2.MaxLength = SessionObject.Enumere.TailleClient;
            Txt_Ordre2.MaxLength = SessionObject.Enumere.TailleOrdre;
        }

        public FrmTranfertDettePaiement(CsDemande UneDemande, string title)
        {
            InitializeComponent();

            IsEnregistre = true;
            Decharger.Content = "<";
            DechargerTout.Content = "<<";
            this.Lademande = UneDemande;
            this.Orientation = (Lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Debit) ? "D" : "C";
            Btn_Rejeter.Visibility = System.Windows.Visibility.Visible;
            OKButton.Content = "Valider";
            this.Title= title;
            Verrou();
            Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
            Txt_Ordre.MaxLength = SessionObject.Enumere.TailleOrdre;
            Txt_Client2.MaxLength = SessionObject.Enumere.TailleClient;
            Txt_Ordre2.MaxLength = SessionObject.Enumere.TailleOrdre;

            this.txt_anot.Text = string.IsNullOrEmpty(Lademande.LaDemande.ANNOTATION) ? string.Empty : Lademande.LaDemande.ANNOTATION;
            this.Txt_CodeCentre.Text = string.IsNullOrEmpty(Lademande.LstCoutDemande.FirstOrDefault().CENTRE) ? string.Empty : Lademande.LstCoutDemande.FirstOrDefault().CENTRE;
            this.Txt_Client.Text = string.IsNullOrEmpty(Lademande.LstCoutDemande.FirstOrDefault().CLIENT) ? string.Empty : Lademande.LstCoutDemande.FirstOrDefault().CLIENT;
            this.Txt_Ordre.Text = string.IsNullOrEmpty(Lademande.LstCoutDemande.FirstOrDefault().ORDRE) ? string.Empty : Lademande.LstCoutDemande.FirstOrDefault().ORDRE;
            this.Txt_CodeCentre2.Text = string.IsNullOrEmpty(Lademande.LaDemande.CENTRE) ? string.Empty : Lademande.LaDemande.CENTRE;
            this.Txt_LibelleCentre2.Text = string.IsNullOrEmpty(Lademande.LaDemande.CENTRE) ? string.Empty : SessionObject.LstCentre.FirstOrDefault(c => c.CODE == Lademande.LaDemande.CENTRE).LIBELLE;
            this.Txt_Client2.Text = string.IsNullOrEmpty(Lademande.LaDemande.CLIENT) ? string.Empty : Lademande.LaDemande.CLIENT;
            this.Txt_Ordre2.Text = string.IsNullOrEmpty(Lademande.LaDemande.ORDRE) ? string.Empty : Lademande.LaDemande.ORDRE;
            TypeDemande = !string.IsNullOrEmpty(Lademande.LaDemande.TYPEDEMANDE) ? Lademande.LaDemande.TYPEDEMANDE : string.Empty;

            ChargerDonneeDuSite();
            ChargerDonneeDuGrid1(Lademande.LstCoutDemande);
        }

        private void ChargerDonneeDuGrid1(List<CsDemandeDetailCout> LstDetail)
        {

            if ( !string.IsNullOrWhiteSpace(Txt_CodeCentre.Text) && !string.IsNullOrWhiteSpace(Txt_Client.Text) && !string.IsNullOrWhiteSpace(Txt_Ordre.Text))
            {
                AfficheFacture(Txt_CodeCentre.Text, Txt_Client.Text, Txt_Ordre.Text, this.Orientation, LstDetail);
            }
        }

        //private void ChargerDonneeDuGrid2(List<CsDemandeDetailCout> LstDetail)
        //{

        //    dgClientEligible.ItemsSource = null;
        //    dgClientEligible.ItemsSource = LstDetail;


        //}

        private void Verrou()
        {

            Txt_CodeSite.IsReadOnly = true;
            Txt_CodeCentre.IsReadOnly = true;
            txt_anot.IsReadOnly = true;
            Txt_Client.IsReadOnly = true;
            Txt_Ordre.IsReadOnly = true;
            Txt_CodeSite2.IsReadOnly = true;
            Txt_CodeCentre2.IsReadOnly = true;
            Txt_Client2.IsReadOnly = true;
            Txt_Ordre2.IsReadOnly = true;
            btn_Rechercher.IsEnabled = false;
            btn_Site2.IsEnabled = false;
            btn_Site.IsEnabled = false;
            btn_Centre.IsEnabled = false;
            btn_Centre2.IsEnabled = false;
            Charger.IsEnabled = false;
            ChargerTout.IsEnabled = false;
            Decharger.IsEnabled = false;
            DechargerTout.IsEnabled = false;
            dgclientselectionne.IsReadOnly = false;
            dgClientEligible.IsReadOnly = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                //if (this.IsEnregistre == true)
                //{
                //    Lademande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusPasseeEncaisse;
                //    ValidationDemande(Lademande);
                //}
                //else
                //{

                //    if (!string.IsNullOrWhiteSpace(Txt_Client2.Text) && !string.IsNullOrWhiteSpace(Txt_Client2.Text) && !string.IsNullOrWhiteSpace(Txt_Client2.Text))
                //    {
                //        Galatee.Silverlight.ServiceCaisse.CaisseServiceClient serviceRecherche = new Galatee.Silverlight.ServiceCaisse.CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));

                //        Galatee.Silverlight.ServiceCaisse.CsClient Element2 = null;
                //        serviceRecherche.TestClientExistCompleted += (s, args) =>
                //        {
                //            if (args != null && args.Cancelled)
                //                return;
                //            Element2 = args.Result;

                //            if (Element2 != null)
                //            {
                //                CsLclient unClient = new CsLclient();
                //                unClient.CENTRE = Element2.CENTRE;
                //                unClient.FK_IDCENTRE = Element2.FK_IDCENTRE;
                //                unClient.FK_IDCLIENT = Element2.PK_ID;
                //                unClient.CLIENT = Txt_Client2.Text;
                //                unClient.ORDRE = Txt_Ordre2.Text;

                //                List<CsDemandeDetailCout> LstDetailDemande = new List<CsDemandeDetailCout>();
                //                List<CsLclient> ligne = new List<CsLclient>();
                //                if (dgClientEligible.ItemsSource != null)
                //                {
                //                    foreach (var item in dgClientEligible.ItemsSource)
                //                    {
                //                        ligne.Add((CsLclient)item);
                //                    }
                //                }

                //                if (ligne.Count > 0)
                //                {
                //                    List<CsLclient> LstResultat = new List<CsLclient>();
                //                    CsLclient resultat = new CsLclient();

                //                    CsDemande Lademande = new CsDemande();
                //                    Lademande.LaDemande = new CsDemandeBase();
                //                    Lademande.LstCoutDemande = new List<CsDemandeDetailCout>();
                //                    Lademande.LeCentre = new CsCentre();
                //                    Lademande.LeTypeDemande = new CsTdem();

                //                    Lademande.LaDemande.NUMDEM = lblDemande.Text;
                //                    Lademande.LaDemande.CENTRE = unClient.CENTRE;
                //                    Lademande.LaDemande.CLIENT = unClient.CLIENT;
                //                    Lademande.LaDemande.ORDRE = unClient.ORDRE;
                //                    Lademande.LaDemande.MATRICULE = UserConnecte.matricule;
                //                    Lademande.LaDemande.TYPEDEMANDE = (this.Orientation == SessionObject.Enumere.Debit) ? SessionObject.Enumere.RechercheDemandeDebit : SessionObject.Enumere.RechercheDemandeCredit;
                //                    Lademande.LaDemande.ANNOTATION = txt_anot.Text;
                //                    Lademande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusEnAttente;

                //                    Lademande.LaDemande.PK_ID = unClient.FK_IDCLIENT;
                //                    Lademande.LaDemande.USERCREATION = UserConnecte.matricule;
                //                    Lademande.LaDemande.DATECREATION = DateTime.Now;
                //                    Lademande.LaDemande.FK_IDCENTRE = unClient.FK_IDCENTRE;

                //                    Lademande.LaDemande.PRODUIT = "03";



                //                    string coper = string.Empty;
                //                    if (this.Orientation == SessionObject.Enumere.Debit)
                //                        coper = SessionObject.Enumere.CoperTransfertDebit;
                //                    else
                //                        coper = SessionObject.Enumere.CoperTransfertDette;


                //                    foreach (CsLclient client in ligne)
                //                    {
                //                        CsDemandeDetailCout detail = new CsDemandeDetailCout();

                //                        detail.CENTRE = client.CENTRE;
                //                        detail.CLIENT = client.CLIENT;
                //                        detail.NDOC = client.NDOC;
                //                        detail.COPER = coper;
                //                        detail.DATECREATION = DateTime.Now;
                //                        detail.FK_IDCENTRE = client.FK_IDCENTRE;
                //                        detail.MONTANTHT = client.MONTANT;
                //                        detail.MONTANTTAXE = client.MONTANTTVA;
                //                        detail.NUMDEM = Lademande.LaDemande.NUMDEM;
                //                        detail.ORDRE = unClient.ORDRE;
                //                        detail.PK_ID = unClient.PK_IDLCLIENT;
                //                        detail.REFEM = client.REFEM;
                //                        detail.USERCREATION = UserConnecte.matricule;
                //                        detail.DATECREATION = DateTime.Now;
                //                        detail.NATURE = client.NATURE;

                //                        detail.TAXE = "00";

                //                        LstDetailDemande.Add(detail);

                //                    }

                //                    Lademande.LstCoutDemande.AddRange(LstDetailDemande);

                //                    ValidationDemande(Lademande);

                                    


                //                }
                //                else
                //                {
                //                    MessageBox.Show("Aucune facture dans le tableau");
                //                }


                //            }
                //            else
                //                MessageBox.Show("le client 2 n'existe pas");

                //        };
                //        serviceRecherche.TestClientExistAsync(Txt_CodeCentre2.Text, Txt_Client2.Text, Txt_Ordre2.Text);
                //        serviceRecherche.CloseAsync();

                //    }
                //    else
                //    {
                //        MessageBox.Show("Veuillez remplir les champs des clients");
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValidationDemande(CsDemande _LaDemande)
        {
            try
            {
                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ValiderDemandeCompleted += (sr, res) =>
                {
                    string Messages = string.Empty;
                    if (res != null && res.Cancelled)
                        return;
                    if (res.Result != true)
                    {
                        Messages = Langue.msg_error_Maj;
                    }
                    //Si la date de d'encaissement n'est pas renseigné c-a-d que la demende est en attente
                    if (_LaDemande.LaDemande.STATUT == SessionObject.Enumere.DemandeStatusEnAttente)
                    {
                        //Msg de confirmation de l'enregistremet
                        Messages = Langue.MsgRequestSaved;

                    }
                    //Si la date d'encaissement est renseigné 
                    else
                    {
                        ////
                        //if (LaDemande.LeTypeDemande.DEMOPTION6 == SessionObject.Enumere.CodeObligatoire && LaDemande.LaDemande.DCAISSE == null)
                        //    Messages = Langue.Msg_PassageEnCaisse;
                        //else
                        //{
                        //    if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation ||
                        //        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FactureManuelle)
                        //        Messages = Langue.MsgOperationTerminee + "/r" + Langue.Msg_etapeProchaine + LaDemande.LstEvenement[0].LOTRI;
                        //    else
                                Messages = Langue.MsgOperationTerminee;
                                if (Closed != null)
                                    Closed(this, new EventArgs());
                                this.DialogResult = true;
                        //}
                    }

                    Message.ShowInformation(Messages, Langue.lbl_Menu);
                    //ImprimerAccuseReception(LaDemande);
                    //if (Closed != null)
                    //    Closed(this, new EventArgs());
                    this.DialogResult = false;
                };
                service1.ValiderDemandeAsync(_LaDemande);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

       

        private void CreationDemandeTransfert(List<CsLclient> ligne, CsLclient unClient, string p)
        {
            //try
            //{
            //    List<CsLclient> LstResultat = new List<CsLclient>();

            //    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            //    service.TransfertCompteClientCompleted += (s, args) =>
            //    {
            //        if (args != null && args.Cancelled)
            //            return;
            //        LstResultat = args.Result;

            //        if (LstResultat.Count == 0)
            //        {
            //            Message.ShowInformation("Transfert réussi", "Transfert");
            //            dgClientEligible.ItemsSource = null;
            //        }
            //        else
            //        {
            //            dgClientEligible.ItemsSource = null;
            //            dgClientEligible.ItemsSource = LstResultat;
            //            Message.ShowInformation("Certains éléments n'ont pas étés transférés", "Transfert");

            //        }
            //    };
            //    service.TransfertCompteClientAsync(Facture, Client2, orientation);
            //    service.CloseAsync();


            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CommonMethode.TransfertDataGrid<CsLclient>(dgclientselectionne, dgClientEligible);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            CommonMethode.TransfertDataGrid<CsLclient>(dgClientEligible, dgclientselectionne);
        }

        List<CsSite> lstSite = new List<CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                //AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                //service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                //{
                //    if (args != null && args.Cancelled)
                //        return;
                //    SessionObject.LstCentre = args.Result;
                //    LstCentre = new ClasseMEthodeGenerique().RetourCentreByPerimetre();
                //    lstSite = new ClasseMEthodeGenerique().RetourneSiteByCentre(LstCentre);
                //    if (lstSite != null)
                //    {
                //        List<CsSite> _LstSite = lstSite.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                //        if (_LstSite.Count == 1)
                //        {
                //            this.Txt_CodeSite.Text = _LstSite[0].CODESITE;
                //            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                //            this.btn_Site.IsEnabled = false;
                //            this.Txt_CodeSite.IsReadOnly = true;
                //            this.Txt_CodeSite2.Text = _LstSite[0].CODESITE;
                //            this.Txt_LibelleSite2.Text = _LstSite[0].LIBELLE;
                //            this.btn_Site2.IsEnabled = false;
                //            this.Txt_CodeSite2.IsReadOnly = true;
                //        }
                //    }
                //    if (LstCentre != null)
                //    {
                //        List<CsCentre> _LstCentre = LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                //        if (_LstCentre.Count == 1)
                //        {
                //            this.Txt_CodeCentre.Text = _LstCentre[0].CODE;
                //            this.Txt_LibelleCentre.Text = _LstCentre[0].LIBELLE;
                //        }
                //        else
                //        {
                //            CsCentre _LeCentre = new CsCentre();
                //            if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text))
                //                _LeCentre = LstCentre.FirstOrDefault(p => p.CODE == this.Txt_CodeCentre.Text);
                //            if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                //            {
                //                this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                //                this.btn_Centre.IsEnabled = false;
                //                this.Txt_CodeCentre.IsReadOnly = true;
                //            }
                //        }
                //    }
                //};
                //service.ListeDesDonneesDesSiteAsync();
                //service.CloseAsync();
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
                    //List<object> _Listgen = new ClasseMEthodeGenerique().RetourneListeObjet(lstSite);
                    //UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                    //ctr.Closed += new EventHandler(galatee_OkClickedSite);
                    //ctr.Show();
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
                this.btn_Site.IsEnabled = true;
                CsSite leSite = (CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODE;
            }
            else
                this.btn_Centre.IsEnabled = true;


        }

        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
            {
                //CsSite _LeSite = new ClasseMEthodeGenerique().RetourneObjectFromList<CsSite>(lstSite, this.Txt_CodeSite.Text, "CODE");
                //if (_LeSite != null && !string.IsNullOrEmpty(_LeSite.CODESITE))
                //    this.Txt_LibelleSite.Text = _LeSite.LIBELLE;
                //else
                //{
                //    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    w.OnMessageBoxClosed += (_, result) =>
                //    {
                //        this.Txt_CodeCentre.Focus();
                //    };
                //    w.Show();
                //}
            }


        }

        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
            {
                //CsCentre _LeCentre = new ClasseMEthodeGenerique().RetourneObjectFromList<CsCentre>(LstCentre, this.Txt_CodeCentre.Text, "CODE");
                //if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                //{
                //    this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                //    //LaDemande.LaDemande.CENTRE = _LeCentre.CODE; ;
                //    //LaDemande.LaDemande.FK_IDCENTRE = _LeCentre.PK_ID;
                //    //LaDemande.LeCentre = LeCentreSelect;
                //    //LaDemande.LaDemande.MATRICULE = UserConnecte.matricule;
                //}
                //else
                //{
                //    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    w.OnMessageBoxClosed += (_, result) =>
                //    {
                //        this.Txt_CodeCentre.Focus();
                //    };
                //    w.Show();
                //}
            }
        }

        private void Txt_CodeCentre2_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (this.Txt_CodeCentre2.Text.Length == SessionObject.Enumere.TailleCentre)
            //{
            //    CsCentre _LeCentre = new ClasseMEthodeGenerique().RetourneObjectFromList<CsCentre>(LstCentre, this.Txt_CodeCentre2.Text, "CODE");
            //    if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
            //    {
            //        this.Txt_LibelleCentre2.Text = _LeCentre.LIBELLE;
            //        //LaDemande.LaDemande.CENTRE = _LeCentre.CODE; ;
            //        //LaDemande.LaDemande.FK_IDCENTRE = _LeCentre.PK_ID;
            //        //LaDemande.LeCentre = LeCentreSelect;
            //        //LaDemande.LaDemande.MATRICULE = UserConnecte.matricule;
            //    }
            //    else
            //    {
            //        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        w.OnMessageBoxClosed += (_, result) =>
            //        {
            //            this.Txt_CodeCentre.Focus();
            //        };
            //        w.Show();
            //    }
            //}
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentre.Count > 0)
                {
                    //this.btn_Centre.IsEnabled = false;
                    //List<object> _Listgen = new ClasseMEthodeGenerique().RetourneListeObjet(LstCentre);
                    //UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", Langue.lbl_ListeCentre);
                    //ctr.Closed += new EventHandler(galatee_OkClickedCentre);
                    //ctr.Show();
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
            lblDemande.Text = leCentre.NUMDEM.ToString();
            this.Txt_CodeCentre.Text = leCentre.CODE;
        }


        private void btn_Site2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (lstSite.Count > 0)
                //{
                //    this.btn_Site.IsEnabled = false;
                //    List<object> _Listgen = new ClasseMEthodeGenerique().RetourneListeObjet(lstSite);
                //    UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                //    ctr.Closed += new EventHandler(galatee_OkClickedSite2);
                //    ctr.Show();
                //}
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }
        void galatee_OkClickedSite2(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                CsSite leSite = (CsSite)ctrs.MyObject;
                this.Txt_CodeSite2.Text = leSite.CODE;
            }
            else
                this.btn_Centre.IsEnabled = true;


        }

        private void Txt_CodeSite2_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (this.Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
            //{
            //    //CsSite _LeSite = new ClasseMEthodeGenerique().RetourneObjectFromList<CsSite>(lstSite, this.Txt_CodeSite.Text, "CODE");
            //    //if (_LeSite != null && !string.IsNullOrEmpty(_LeSite.CODESITE))
            //    //    this.Txt_LibelleSite2.Text = _LeSite.LIBELLE;
            //    //else
            //    //{
            //    //    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    //    w.OnMessageBoxClosed += (_, result) =>
            //    //    {
            //    //        this.Txt_CodeCentre2.Focus();
            //    //    };
            //    //    w.Show();
            //    //}
            //}


        }

       

        private void btn_Centre2_Click(object sender, RoutedEventArgs e)
         {
            try
            {
                //if (LstCentre.Count > 0)
                //{
                //    this.btn_Centre2.IsEnabled = false;
                //    List<object> _Listgen = new ClasseMEthodeGenerique().RetourneListeObjet(LstCentre);
                //    UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", Langue.lbl_ListeCentre);
                //    ctr.Closed += new EventHandler(galatee_OkClickedCentre2);
                //    ctr.Show();
                //}
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }

        void galatee_OkClickedCentre2(object sender, EventArgs e)
        {
            this.btn_Centre2.IsEnabled = true;
            LeCentreSelect = new CsCentre();
            UcListeGenerique ctrs = sender as UcListeGenerique;
            CsCentre leCentre = (CsCentre)ctrs.MyObject;
            LeCentreSelect = leCentre;
            this.Txt_CodeCentre2.Text = leCentre.CODE;
        }

        private void Txt_Ordre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Ordre.Text))
                this.Txt_Ordre.Text = this.Txt_Ordre.Text.PadLeft(SessionObject.Enumere.TailleOrdre, '0');
        }
        private void Txt_CodeCentre_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text))
                this.Txt_CodeCentre.Text = this.Txt_CodeCentre.Text.PadLeft(SessionObject.Enumere.TailleOrdre, '0');
        }
        private void Txt_Client_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Client.Text))
                this.Txt_Client.Text = this.Txt_Client.Text.PadLeft(SessionObject.Enumere.TailleClient, '0');
        }

        private void Txt_Ordre2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Ordre2.Text))
                this.Txt_Ordre2.Text = this.Txt_Ordre2.Text.PadLeft(SessionObject.Enumere.TailleOrdre, '0');
        }
        private void Txt_CodeCentre2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text))
                this.Txt_CodeCentre2.Text = this.Txt_CodeCentre2.Text.PadLeft(SessionObject.Enumere.TailleOrdre, '0');
        }
        private void Txt_Client2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Client2.Text))
                this.Txt_Client2.Text = this.Txt_Client2.Text.PadLeft(SessionObject.Enumere.TailleClient, '0');
        }

        private void btn_Rechercher_Click_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Txt_CodeSite.Text) && !string.IsNullOrWhiteSpace(Txt_CodeCentre.Text) && !string.IsNullOrWhiteSpace(Txt_Client.Text) && !string.IsNullOrWhiteSpace(Txt_Ordre.Text))
            {
                AfficheFacture(Txt_CodeCentre.Text, Txt_Client.Text, Txt_Ordre.Text, this.Orientation, null);
            }
        }

        private void AfficheFacture(string centre, string client, string ordre, string orientation, List<CsDemandeDetailCout> LstDetail)
        {
            try
            {
                List<CsLclient> ListeElement = null;
                List<CsLclient> ListeElementDg2 = new List<CsLclient>();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCompteClientTransfertCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    ListeElement = args.Result;
                    dgclientselectionne.ItemsSource = ListeElement;


                    if (this.IsEnregistre == true)
                    {
                        foreach (CsDemandeDetailCout detail in LstDetail)
                        {
                            CsLclient Unclient = new CsLclient();
                            Unclient = ListeElement.Where(c => c.CLIENT == detail.CLIENT && c.CENTRE == detail.CENTRE && c.ORDRE == detail.ORDRE && c.NDOC == detail.NDOC && c.REFEM == detail.REFEM).FirstOrDefault();
                            ListeElementDg2.Add(Unclient);
                        }
                        dgClientEligible.ItemsSource = ListeElementDg2;
                    }


                   
                };
                CsClientRechercher Leclient = new CsClientRechercher();
                Leclient.CENTRE=centre;
                Leclient.CLIENT = client;
                Leclient.ORDRE = ordre;
                service.RetourneCompteClientTransfertAsync(Leclient, orientation);
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //new CommonMethode().TransfertclientAll<CsLclient>(dgclientselectionne, dgClientEligible);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //new CommonMethode().TransfertclientAll<CsLclient>(dgClientEligible, dgclientselectionne);
        }

        private void Rejet_Click(object sender, RoutedEventArgs e)
        {
            FrAnotation ctr = new FrAnotation();
            ctr.Closed += new EventHandler(galatee_OkClickedMotif);
            ctr.Show();
        }

        public event EventHandler Closed;
        private void galatee_OkClickedMotif(object sender, EventArgs e)
        {
            FrAnotation leMotifRejet = sender as FrAnotation;
            if (!string.IsNullOrEmpty(leMotifRejet.txtAnnotation.Text))
            {
                motifRejet = leMotifRejet.txtAnnotation.Text;
                ValidationRejetDemande(Lademande);

            }
        }

        private void ValidationRejetDemande(CsDemande _LaDemande)
        {
            try
            {
                _LaDemande.LaDemande.ANNOTATION = this.motifRejet;
                _LaDemande.LaDemande.STATUTDEMANDE = "1";
                _LaDemande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusEnAttente;
                _LaDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                _LaDemande.LaDemande.DATEMODIFICATION = System.DateTime.Now;
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ValiderRejetDemandeCompleted += (sr, res) =>
                {
                    if (res.Error == null)
                    {
                        Message.ShowInformation(Langue.MsgOperationTerminee, Langue.lbl_Menu);
                        this.DialogResult = false;
                    }
                    else
                        Message.ShowInformation("Une erreur s'est produite lors du rejet", Langue.lbl_Menu);
                    if (Closed != null)
                        Closed(this, new EventArgs());

                };
                service1.ValiderRejetDemandeAsync(_LaDemande.LaDemande);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}


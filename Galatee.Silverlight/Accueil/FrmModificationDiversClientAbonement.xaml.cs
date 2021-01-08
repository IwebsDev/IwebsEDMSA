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
using Galatee.Silverlight.ServicePrintings;
using Galatee.Silverlight.Resources.Accueil ;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.MainView;
//using Galatee.Silverlight.serviceWeb;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmModificationDiversClientAbonement : ChildWindow
    {
        public FrmModificationDiversClientAbonement()
        {
            InitializeComponent();
            ChargerDonneeDuSite();

            //LaDemande = _LaDemande;

        }
        void translate()
        {
            this.lbl_centre.Content = Langue.lbl_center;
            this.lbl_adresse.Content = Langue.lbl_adresse;
            this.lbl_NumDemande.Content = Langue.lbl_Numdemande;
            this.lbl_Ordre .Content = Langue.lbl_Ordre ;
            dataGrid1.Columns[0].Header = Langue.lbl_center;
            dataGrid1.Columns[1].Header = Langue.lbl_Ordre;
            dataGrid1.Columns[2].Header = Langue.lbl_produit ;
        }
        public FrmModificationDiversClientAbonement(string _TypeDemande)
        {
            InitializeComponent();
            TypeDemande = _TypeDemande;
            ChargerDonneeDuSite();
            ChargerListeDeProduit();
            
        }
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<Galatee.Silverlight.ServiceAccueil.CsCentre>();
        CsDemande LaDemande = new CsDemande();
        List<Galatee.Silverlight.ServiceAccueil.CsProduit> LstDeProduit;
        List<Galatee.Silverlight.ServiceAccueil.CsAbon> AbonementRecherche;
        Galatee.Silverlight.ServiceAccueil.CsAbon AbonneSelect = new Galatee.Silverlight.ServiceAccueil.CsAbon();
        Galatee.Silverlight.ServiceAccueil.CsClient LeClientRecherche = new Galatee.Silverlight.ServiceAccueil.CsClient();
        string TypeDemande;
        private void ChargerDonneeDuSite()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    LstCentre.AddRange(args.Result);
                    if (LstCentre != null)
                    {
                        if (LstCentre.Count == 1 )
                        {
                            this.Txt_CodeCentre.Text = LstCentre[0].CODECENTRE ;
                            this.Txt_LibelleCentre.Text = LstCentre[0].LIBELLE  ;
                            this.btn_Centre.IsEnabled = false;
                            ShowInfoCentre_NumDem();
                        }
                        else
                        {
                            Galatee.Silverlight.ServiceAccueil.CsCentre _LeCentre = LstCentre.FirstOrDefault(p => p.CODECENTRE == LaDemande.LaDemande.CENTRE );
                            if (_LeCentre != null)
                            {
                                this.Txt_CodeCentre.Text = LaDemande.LaDemande.CENTRE ;
                                this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE ;
                                this.btn_Centre.IsEnabled = false;
                                this.Txt_CodeCentre.IsReadOnly = true;
                                ShowInfoCentre_NumDem();
                            }
                        }
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

                };
                service.ListeDesDonneesDesSiteAsync();
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
                    LstDeProduit = SessionObject.ListeDesProduit;
                    if (LstDeProduit != null)
                    {
                       
                    }

                }
                else
                {
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service1.ListeDesProduitCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        SessionObject.ListeDesProduit = res.Result;
                        LstDeProduit = SessionObject.ListeDesProduit;
                        if (LstDeProduit != null)
                        {
                            
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

        List<Galatee.Silverlight.ServiceAccueil.CsProduit> RetourneListeDeProduitDuSite(Galatee.Silverlight.ServiceAccueil.CsSite LeSite)
        {

            List<Galatee.Silverlight.ServiceAccueil.CsProduit> ListeDesProduitDuSite = new List<Galatee.Silverlight.ServiceAccueil.CsProduit>();
            if (!string.IsNullOrEmpty(LeSite.PRODUIT1))
            {
                Galatee.Silverlight.ServiceAccueil.CsProduit LeProduit = new Galatee.Silverlight.ServiceAccueil.CsProduit();
                LeProduit.CODEPRODUIT  = LeSite.PRODUIT1;
                LeProduit.LIBELLE = LeSite.LIBELLEPRODUIT1;
                ListeDesProduitDuSite.Add(LeProduit);
            }
            if (!string.IsNullOrEmpty(LeSite.PRODUIT2))
            {
                Galatee.Silverlight.ServiceAccueil.CsProduit LeProduit = new Galatee.Silverlight.ServiceAccueil.CsProduit();
                LeProduit.CODEPRODUIT  = LeSite.PRODUIT2;
                LeProduit.LIBELLE = LeSite.LIBELLEPRODUIT2;

                ListeDesProduitDuSite.Add(LeProduit);
            }
            if (!string.IsNullOrEmpty(LeSite.PRODUIT3))
            {
                Galatee.Silverlight.ServiceAccueil.CsProduit LeProduit = new Galatee.Silverlight.ServiceAccueil.CsProduit();
                LeProduit.CODEPRODUIT = LeSite.PRODUIT3;
                LeProduit.LIBELLE = LeSite.LIBELLEPRODUIT3;

                ListeDesProduitDuSite.Add(LeProduit);
            }
            if (!string.IsNullOrEmpty(LeSite.PRODUIT4))
            {
                Galatee.Silverlight.ServiceAccueil.CsProduit LeProduit = new Galatee.Silverlight.ServiceAccueil.CsProduit();
                LeProduit.CODEPRODUIT = LeSite.PRODUIT4;
                LeProduit.LIBELLE = LeSite.LIBELLEPRODUIT4;

                ListeDesProduitDuSite.Add(LeProduit);
            }
            if (!string.IsNullOrEmpty(LeSite.PRODUIT5))
            {
                Galatee.Silverlight.ServiceAccueil.CsProduit LeProduit = new Galatee.Silverlight.ServiceAccueil.CsProduit();
                LeProduit.CODEPRODUIT = LeSite.PRODUIT5;
                LeProduit.LIBELLE = LeSite.LIBELLEPRODUIT5;

                ListeDesProduitDuSite.Add(LeProduit);
            }
            return ListeDesProduitDuSite;
        }
        private void RetourneInfoAbon(CsDemande  LaDemande)
        {
            AbonementRecherche = new List<Galatee.Silverlight.ServiceAccueil.CsAbon>();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint(this ));
            service.RetourneAbonCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                AbonementRecherche = args.Result;
                if (AbonementRecherche != null)
                {
                    RemplireDataGridAbonnement(AbonementRecherche);
                }
            };
            service.RetourneAbonAsync(LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE);
            service.CloseAsync();
        }
        private void RemplireDataGridAbonnement(List<Galatee.Silverlight.ServiceAccueil.CsAbon > ListAbon)
        {
            dataGrid1.ItemsSource = null;
            dataGrid1.ItemsSource = ListAbon;
            if (ListAbon != null && ListAbon.Count != 0)
            {
                this.dataGrid1.SelectedItem = ListAbon[0];
            }
            AbonneSelect = (Galatee.Silverlight.ServiceAccueil.CsAbon)this.dataGrid1.SelectedItem;
            if (AbonneSelect!= null )
            LaDemande.Abonne = AbonneSelect;
            LaDemande.Abonne.NUMDEM = Txt_NumDemande.Text;
            LaDemande.LaDemande.PRODUIT = LaDemande.Abonne.PRODUIT;
            RetourneInfoClient(LaDemande );
        }


        private void RetourneInfoClient(CsDemande _LeDemande)
        {
            LeClientRecherche = new Galatee.Silverlight.ServiceAccueil.CsClient();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint(this ));
            service.RetourneClientCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                LeClientRecherche = args.Result;
                if (LeClientRecherche != null )
                {
                    LaDemande.LeClient  = LeClientRecherche;
                    this.Txt_NomAbon.Text = string.IsNullOrEmpty(LeClientRecherche.NOMABON) ? string.Empty : LeClientRecherche.NOMABON ;
                }
            };
            service.RetourneClientAsync(_LeDemande.LaDemande.CENTRE, _LeDemande.LaDemande.CLIENT  , _LeDemande.LaDemande.ORDRE  );
            service.CloseAsync();

        }
        private void ValidationDemande(CsDemande _lademande)
        {
            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint(this ));
            service1.ValiderDemandeCompleted += (sr, res) =>
            {
                this.DialogResult = false;
            };
            service1.ValiderDemandeAsync(_lademande);
            service1.CloseAsync();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            //AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            //service.RetourneCatCliCompleted += (s, args) =>
            //{
            //    if (args != null && args.Cancelled)
            //        return;
            //    Dictionary<string, string> param = null;
            //    Utility.ActionPreview<ServicePrintings.CsCategorieClient, ServiceAccueil.CsCategorieClient>(args.Result, param, "Test", "Acceuil");
            //};
            //string key = Utility.getKey();
            //service.RetourneCatCliAsync(key);
            //service.CloseAsync();


            //if (LaDemande.LeClient.IsModifClient || LaDemande.Abonne.IsModifAbon)
            //{
            LaDemande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusPriseEnCompte;
            LaDemande.LaDemande.DATECREATION = DateTime.Now;
            LaDemande.LaDemande.DATEMODIFICATION = DateTime.Now;
            ValidationDemande(LaDemande);
            this.DialogResult = true;
            //}
            //else
            //{
            //    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgAucuneModif, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    w.Show();
            //}
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (LaDemande.LaDemande == null)
                LaDemande.LaDemande = new CsDemandeBase();
            if (LaDemande.LeClient == null)
                LaDemande.LeClient = new Galatee.Silverlight.ServiceAccueil.CsClient();
            if (LaDemande.Abonne == null)
                LaDemande.Abonne = new Galatee.Silverlight.ServiceAccueil.CsAbon();
        }

    
       

        private void ShowInfoCentre_NumDem()
        {
            if(LstCentre!=null)
            if (this.Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre && LstCentre.Count()>0 )
            {
                //Galatee.Silverlight.ServiceAccueil.CsCentre _LeCentre = new ClasseMEthodeGenerique().RetourneObjectFromList<Galatee.Silverlight.ServiceAccueil.CsCentre>(LstCentre, this.Txt_CodeCentre.Text, "CODECENTRE");
                Galatee.Silverlight.ServiceAccueil.CsCentre _LeCentre = LstCentre.FirstOrDefault(c => c.CODECENTRE == this.Txt_CodeCentre.Text);
                if (_LeCentre != null)
                {
                    this.Txt_LibelleCentre.Text = _LeCentre.NOM;
                    this.Txt_NumDemande.Text = _LeCentre.CODECENTRE + _LeCentre.NUMDEM.ToString().PadLeft(10, '0');
                    LaDemande.LaDemande.CENTRE = _LeCentre.CODECENTRE;
                    LaDemande.LaDemande.FK_IDCENTRE = _LeCentre.PK_ID;
                    LaDemande.LaDemande.NUMDEM = this.Txt_NumDemande.Text;
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
        private void SetDemandeFromScream()
        {
            LaDemande.LaDemande.CENTRE  = string.IsNullOrEmpty(this.Txt_CodeCentre.Text) ? string.Empty : this.Txt_CodeCentre.Text;
            LaDemande.LaDemande.CLIENT  = string.IsNullOrEmpty(this.Txt_Client.Text) ? string.Empty : this.Txt_Client.Text;
            LaDemande.LaDemande.ORDRE  = string.IsNullOrEmpty(this.Txt_OrdreClient.Text) ? string.Empty : this.Txt_OrdreClient.Text;
            LaDemande.LaDemande.NUMDEM  = string.IsNullOrEmpty(this.Txt_NumDemande.Text) ? string.Empty : this.Txt_NumDemande.Text;
            LaDemande.LaDemande.TDEM = TypeDemande;
        
        }
        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Txt_OrdreClient.Text = ((Galatee.Silverlight.ServiceAccueil.CsAbon)dataGrid1.SelectedItem)!=null?((Galatee.Silverlight.ServiceAccueil.CsAbon)dataGrid1.SelectedItem).ORDRE:string.Empty;
            LaDemande.Abonne = (Galatee.Silverlight.ServiceAccueil.CsAbon)dataGrid1.SelectedItem;
        }
        private void Txt_OrdreClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text))
            {
                SetDemandeFromScream();
                RetourneInfoAbon(LaDemande);
            }
        }
        private void btn_DetailAbon_Click(object sender, RoutedEventArgs e)
        {
            UcDetailAbonnement ctr = new UcDetailAbonnement(LaDemande);
            ctr.Closed += new EventHandler(galatee_OkClicked);
            ctr.Show();
        }
        void galatee_OkClicked(object sender, EventArgs e)
        {
            UcDetailAbonnement ctrs = sender as UcDetailAbonnement;
            LaDemande = ctrs.LaDemande ;
        }

        private void btn_DetailClient_Click(object sender, RoutedEventArgs e)
        {
            UcDetailClient ctr = new UcDetailClient(LaDemande,false );
            ctr.Closed += new EventHandler(galatee_OkClickedClient);
            ctr.Show();
        }
        void galatee_OkClickedClient(object sender, EventArgs e)
        {
            UcDetailClient ctrs = sender as UcDetailClient;
            LaDemande = ctrs.LaDemande;
        }

        private void Txt_Client_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text))
            {

                if (!string.IsNullOrEmpty(this.Txt_Client.Text) &&
                this.Txt_Client.Text.Length == SessionObject.Enumere.TailleClient)
                {
                   SetDemandeFromScream();
                    RetourneInfoAbon(LaDemande);
                }
                         
            }
        }


        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowInfoCentre_NumDem();
            //ChargerDonneeDuSite();
        }
       


        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentre.Count != 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> _LstObjet = new ClasseMEthodeGenerique().RetourneListeObjet(LstCentre);
                    UcListeGenerique ctr = new UcListeGenerique(_LstObjet, "CODECENTRE", "LIBELLE", Langue.lbl_Menu);
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnTCentre);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void galatee_OkClickedBtnTCentre(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsCentre _LeCentre = (Galatee.Silverlight.ServiceAccueil.CsCentre)ctrs.MyObject;
                    this.Txt_CodeCentre.Text = _LeCentre.CODECENTRE;
                    this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                    this.btn_Centre.IsEnabled = true;
                }
                else
                    this.btn_Centre.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }


        }

        private void Txt_CodeCentre_LostFocus_2(object sender, RoutedEventArgs e)
        {
            try
            {
                HandleLostFocus<Galatee.Silverlight.ServiceAccueil.CsCentre>((TextBox)sender, this.Txt_LibelleCentre, LstCentre, SessionObject.Enumere.TailleCentre);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }       
        
        private void HandleLostFocus<T>(TextBox Code, TextBox Libelle, List<T> listItems, int Taille)
        {
            if (!string.IsNullOrEmpty(Code.Text) &&
                Code.Text.Length == Taille &&
                listItems.Count != 0)
            {
                Code.Text = Code.Text.PadLeft(Taille, '0');
            }
            else
            {
                Code.Text = string.Empty;
                Libelle.Text = string.Empty;
            }
        }

    }
}


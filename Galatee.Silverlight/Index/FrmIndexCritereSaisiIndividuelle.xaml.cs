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
using Galatee.Silverlight.Resources.Index;
using Galatee.Silverlight.Resources.Accueil;


namespace Galatee.Silverlight.Facturation
{
    public partial class FrmIndexCritereSaisiIndividuelle : ChildWindow
    {
        public FrmIndexCritereSaisiIndividuelle()
        {
            InitializeComponent();

            try
            {
                this.Txt_Centre.MaxLength = SessionObject.Enumere.TailleCentre;
                this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
                this.Txt_produit.MaxLength = SessionObject.Enumere.TailleCodeProduit;
                this.Txt_Centre.IsReadOnly = true;
                this.Txt_Client .IsReadOnly = true;
                this.Txt_produit .IsReadOnly = true;
                this.Cbo_Compteur.IsEnabled = false;

                ChargerDonneeDuSite();
                Initctrl();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }


        }
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre.Count != 0)
                {
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre, SessionObject.LstCentre);
                    if (lstSite.Count == 1)
                    {
                        this.txt_Site.Text = lstSite.First().LIBELLE;
                        this.txt_Site.Tag = lstSite.First().PK_ID;
                        this.Txt_Centre.IsReadOnly = false;
                        this.Txt_Client.IsReadOnly = false;
                        this.Txt_produit.IsReadOnly = false;
                        this.Cbo_Compteur.IsEnabled = true;
                    }
                    if (LstCentre.Count == 1)
                    {
                        this.Txt_Centre.Text = LstCentre.First().CODE;
                        this.Txt_Centre.IsReadOnly = true;
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre, SessionObject.LstCentre);
                    if (lstSite.Count == 1)
                    {
                        this.txt_Site.Text = lstSite.First().LIBELLE ;
                        this.txt_Site.Tag = lstSite.First().PK_ID;

                        this.Txt_Centre.IsReadOnly = false;
                        this.Txt_Client.IsReadOnly = false;
                        this.Txt_produit.IsReadOnly = false;
                        this.Cbo_Compteur.IsEnabled = true;
                    }

                    return;
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerDonneeDuSite");

            }
        }

        public void Initctrl()
        {
            try
            {
                this.lbl_batch.Content = Galatee.Silverlight.Resources.Index.Langue.btn_batch;
                this.lbl_centre.Content = Galatee.Silverlight.Resources.Accueil.Langue.lbl_center;
                this.lbl_Compteur.Content = Galatee.Silverlight.Resources.Accueil.Langue.lbl_NumeroCompteur;
                this.lbl_reference.Content = Galatee.Silverlight.Resources.Index.Langue.lbl_reference;
                this.lbl_Compteur.Content = Galatee.Silverlight.Resources.Accueil.Langue.lbl_NumeroCompteur;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<CsCanalisation> LstCanalisationProduit;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SessionObject.Enumere.ElectriciteMT == this.Txt_produit.Text)
                    ChargerLstEvenementNonFacture(LstCanalisationProduit);
                else
                {
                    CsCanalisation _LaCanalisationSelect = (CsCanalisation)this.Cbo_Compteur.SelectedItem;
                    ChargerLstEvenementNonFacture(LstCanalisationProduit);
                }
            }
            catch (Exception ex)
            {
              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Txt_produit_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                RechercheCompteur();
            }
            catch (Exception ex)
            {
              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void Txt_Centre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_Centre.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    Galatee.Silverlight.ServiceAccueil.CsCentre leCentreSelect = LstCentre.FirstOrDefault (t => t.CODE == this.Txt_Centre.Text);
                    if (leCentreSelect != null && !string.IsNullOrEmpty(leCentreSelect.CODE))
                    {
                        this.Txt_Centre.Tag = leCentreSelect.PK_ID;
                        RechercheCompteur();
                    }
                }
            }
            catch (Exception ex)
            {
            Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void Txt_Client_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                RechercheCompteur();
            }
            catch (Exception ex)
            {
             Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void RechercheCompteur()
        {
            try
            {
                if (this.Txt_Centre.Text.Length == SessionObject.Enumere.TailleCentre &&
                        this.Txt_Client.Text.Length == SessionObject.Enumere.TailleClient &&
                        this.Txt_produit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
                {
                   ServiceAccueil.CsCentre leCentre = LstCentre.FirstOrDefault(t => t.CODE == this.Txt_Centre.Text && t.FK_IDCODESITE == (int)this.txt_Site.Tag) ;
                   if (leCentre != null)
                   {
                       int idcentre = LstCentre.FirstOrDefault(t => t.CODE == this.Txt_Centre.Text && t.FK_IDCODESITE == (int)this.txt_Site.Tag).PK_ID;
                       RetourneCanalisation(idcentre, this.Txt_Centre.Text, this.Txt_Client.Text, this.Txt_produit.Text);
                   }
                   else
                   {
                       Message.ShowInformation("Centre non trouvée", "Index");
                       return;
                   }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void RetourneCanalisation(int fk_idcentre, string Centre, string Client, string Produit)
        {
            List<CsCanalisation> LstCanalisation = new List<CsCanalisation>();
            LstCanalisationProduit = new List<CsCanalisation>();

            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.RetourneCanalisationCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur suite appel service", "Index");
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            Message.ShowError("Evenement non trouvé", "Index");
                            return;
                        }

                        LstCanalisation.AddRange(args.Result);
                        if (LstCanalisation.Count != 0)
                        {
                            LstCanalisationProduit = LstCanalisation.Where(p => p.PRODUIT == Produit).ToList();
                            if (LstCanalisationProduit.Count != 0)
                            {
                                foreach (CsCanalisation item in LstCanalisationProduit)
                                    item.INFOCOMPTEUR = item.NUMERO   + " " + item.LIBELLETYPECOMPTEUR;
                                this.Cbo_Compteur.ItemsSource = null;
                                this.Cbo_Compteur.ItemsSource = LstCanalisationProduit;
                                this.Cbo_Compteur.DisplayMemberPath = "INFOCOMPTEUR";
                                if (LstCanalisationProduit.Count == 1)
                                    this.Cbo_Compteur.SelectedItem = LstCanalisationProduit.First();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                     Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                service.RetourneCanalisationAsync(fk_idcentre,Centre, Client, Produit, null); // PARAMETRE MANQUANT
            }
            catch (Exception ex)
            {
              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        
        }

        private void ChargerEvenementNonFacture(CsCanalisation LaCanalisationSelect)
        {
            List<CsEvenement> LstEvenement =new  List<CsEvenement>();
            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.RetourneEvenementNonFactCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel serveur. Veuillez reessayer svp ", "");
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("Aucune donnée retournée par le système.", "Information");
                            return;
                        }

                        LstEvenement.Clear();
                        LstEvenement.AddRange(args.Result.EventLotriNull);
                        LstEvenement.AddRange(args.Result.EventPageri);
                        LstEvenement.AddRange(args.Result.EventPagisol);

                        if (LstEvenement.Count != 0)
                        {
                            LstEvenement = AffecterInfoPrecedente(LstEvenement.OrderBy(p => p.NUMEVENEMENT).ToList());
                            UcSaisieIndividuelle Ctrl = new UcSaisieIndividuelle(LstEvenement, args.Result.ConsoPrecedent, args.Result.CONSOMOYENNE);
                            Ctrl.Show();
                        }
                    }
                    catch (Exception ex)
                    {
                      Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                service.RetourneEvenementNonFactAsync(LaCanalisationSelect);
            }
            catch (Exception ex)
            {
              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
          
        
        }

        private void ChargerLstEvenementNonFacture(List<CsCanalisation> LstCanalisationSelect)
        {
            List<CsEvenement> LstEvenement = new List<CsEvenement>();
            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.RetourneListEvenementNonFactCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel serveur. Veuillez reessayer svp ", "");
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("Aucune donnée retournée par le système.", "Information");
                            return;
                        }
                        List<CsSaisiIndexIndividuel> lstEvtCompteur = new List<CsSaisiIndexIndividuel>();
                        lstEvtCompteur = args.Result;

                        if (lstEvtCompteur.Count != 0)
                        {
                            LstEvenement = AffecterInfoPrecedente(LstEvenement.OrderBy(p => p.NUMEVENEMENT).ToList());
                            UcSaisieIndividuelle Ctrl = new UcSaisieIndividuelle(lstEvtCompteur);
                            Ctrl.Show();
                        }
                        else
                        {
                            Message.ShowError("Aucun evenement non saisi", "Information");
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                service.RetourneListEvenementNonFactAsync(LstCanalisationSelect);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private List<CsEvenement> AffecterInfoPrecedente(List<CsEvenement> LstEvenement)
        {
            int i = 0;
            try
            {
                foreach (CsEvenement item in LstEvenement)
                {
                    item.INDEXPRECEDENTEFACTURE = 0;
                    item.CASPRECEDENTEFACTURE = item.CAS;
                    item.PERIODEPRECEDENT = item.PERIODE;

                    //int Indice = 0;
                    //if (i != 0)
                    //    Indice = i - 1;
                    //else Indice = i;
                    //item.INDEXEVTPRECEDENT = LstEvenement[Indice].INDEXEVT;
                    //item.CASPRECEDENT = LstEvenement[Indice].CAS;
                    //item.PERIODEPRECEDENT = LstEvenement[Indice].PERIODE ;

                    if (i != 0)
                    {
                        if (item.COMPTEUR == LstEvenement[i - 1].COMPTEUR)
                        {
                            item.INDEXPRECEDENTEFACTURE = LstEvenement[i - 1].INDEXEVT;
                            item.CASPRECEDENTEFACTURE = LstEvenement[i - 1].CAS;
                            item.PERIODEPRECEDENT = LstEvenement[i - 1].PERIODE;
                        }
                    }

                    i++;
                }
                return LstEvenement;
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }
        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> _Listgen = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    List<string> _LstColonneAffich = new List<string>();
                    _LstColonneAffich.Add("CODE");
                    _LstColonneAffich.Add("LIBELLE");
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(_Listgen, _LstColonneAffich, false, "Site");
                    ctrl.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Index");
            }

        }
        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Centre.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.txt_Site.Text = leSite.LIBELLE ;
                this.txt_Site.Tag = leSite.PK_ID;

                this.Txt_Centre.IsReadOnly = false;
                this.Txt_Client.IsReadOnly = false;
                this.Txt_produit.IsReadOnly = false;
                this.Cbo_Compteur.IsEnabled = true;
            }
            else
                this.btn_Centre.IsEnabled = true;


        }


    }
}


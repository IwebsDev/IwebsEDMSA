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
using Galatee.Silverlight.ServiceAccueil ;
using Galatee.Silverlight.Resources.Accueil  ;
using System.Collections.ObjectModel;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Classes;
using Galatee.Silverlight.Resources.Devis;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmRechercherDemande : ChildWindow
    {

        private List<CsCommune> _listeDesCommuneExistant = null;
        private List<CsCommune> _listeDesCommuneExistantCentre = null;
        public FrmRechercherDemande()
        {
            InitializeComponent();
            ChargerTypeDemande();
            ChargerDonneeDuSite();
            RemplirCommune();
            RemplirListeDesQuartierExistant();
        }

        private void txt_NumRue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.txt_NumRue.Text.Length == SessionObject.Enumere.TailleRue)
                {
                    if (this.Cbo_Secteur.SelectedItem != null)
                    {
                        CsRues laRue = _listeDesRuesExistant.FirstOrDefault(t => t.CODE == this.txt_NumRue.Text && (t.FK_IDSECTEUR == (int)this.Cbo_Secteur.Tag || t.CODE == DataReferenceManager.RueInconnue));
                        if (laRue != null)
                        {
                            //if ((this.Cbo_Rue.SelectedItem != null && (CsRues)this.Cbo_Rue.SelectedItem != laRue) || this.Cbo_Rue.SelectedItem == null)
                            //    this.Cbo_Rue.SelectedItem = laRue;
                        }
                        else
                        {
                            Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        //private void RemplirRues(int pIdSecteur)
        //{
        //    List<CsRues> ListeRuesFiltrees = new List<CsRues>();
        //    List<CsRues> RueParDefaut = null;
        //    this.txt_NumRue.Text = string.Empty;
        //    try
        //    {
        //        RueParDefaut = _listeDesRuesExistant.Where(q => q.CODE == DataReferenceManager.RueInconnue).ToList();
        //        if (RueParDefaut != null && RueParDefaut.Count > 0)
        //            ListeRuesFiltrees.AddRange(RueParDefaut);
        //        ListeRuesFiltrees.AddRange(_listeDesRuesExistant.Where(q => q.FK_IDSECTEUR == pIdSecteur && q.CODE != DataReferenceManager.RueInconnue).ToList());

        //        //Cbo_Rue.ItemsSource = null;
        //        //Cbo_Rue.ItemsSource = ListeRuesFiltrees;
        //        //Cbo_Rue.SelectedValuePath = "PK_ID";
        //        //Cbo_Rue.DisplayMemberPath = "LIBELLE";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void RemplirListeDesRuesExistant()
        {
            try
            {

                if (SessionObject.LstRues != null && SessionObject.LstRues.Count != 0)
                {
                    _listeDesRuesExistant = SessionObject.LstRues;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesRueDesSecteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstRues = args.Result;
                    _listeDesRuesExistant = SessionObject.LstRues;
                };
                service.ChargerLesRueDesSecteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void txt_NumSecteur_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.txt_NumSecteur.Text.Length == SessionObject.Enumere.TailleSecteur)
                {
                    if (this.Cbo_Quartier.SelectedItem != null)
                    {
                        List<ServiceAccueil.CsSecteur> lstSecteur = SessionObject.LstSecteur.Where(t => t.FK_IDQUARTIER == (int)this.Cbo_Quartier.Tag).ToList();
                        CsSecteur leSecteur = lstSecteur.FirstOrDefault(t => t.CODE == this.txt_NumSecteur.Text);
                        if (leSecteur != null)
                        {
                            if ((this.Cbo_Secteur.SelectedItem != null && (CsSecteur)this.Cbo_Secteur.SelectedItem != leSecteur) || this.Cbo_Secteur.SelectedItem == null)
                                this.Cbo_Secteur.SelectedItem = leSecteur;
                        }
                        else
                        {
                            Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_Secteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Secteur.SelectedItem != null)
                {
                    var Secteur = Cbo_Secteur.SelectedItem as ServiceAccueil.CsSecteur;
                    if (Secteur != null)
                    {
                        txt_NumSecteur.Text = Secteur.CODE ?? string.Empty;
                        this.Cbo_Secteur.Tag = Secteur.PK_ID;
                        //RemplirRues(Secteur.PK_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void RemplirSecteur()
        {
            try
            {
                Cbo_Secteur.SelectedValuePath = "PK_ID";
                Cbo_Secteur.DisplayMemberPath = "LIBELLE";
                if (SessionObject.LstSecteur != null && SessionObject.LstSecteur.Count != 0)
                {
                    Cbo_Secteur.Items.Clear();
                    Cbo_Secteur.ItemsSource = SessionObject.LstSecteur;
                  
                    return;
                }
                else
                {

                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerLesSecteursCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstSecteur = args.Result;
                        Cbo_Secteur.Items.Clear();
                        Cbo_Secteur.ItemsSource = SessionObject.LstSecteur;
                      
                        return;
                    };
                    service.ChargerLesSecteursAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txt_Quartier_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.txt_Quartier.Text.Length == SessionObject.Enumere.TailleQuartier)
                {
                    CsQuartier leQuartier = ListeQuartierFiltres.FirstOrDefault(t => t.CODE == this.txt_Quartier.Text);
                    if (leQuartier != null)
                    {
                        if ((this.Cbo_Quartier.SelectedItem != null && (CsQuartier)this.Cbo_Quartier.SelectedItem != leQuartier) || this.Cbo_Quartier.SelectedItem == null)
                            this.Cbo_Quartier.SelectedItem = leQuartier;
                    }
                    else
                    {
                        Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_Quartier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Quartier.SelectedItem != null)
                {
                    var quartier = Cbo_Quartier.SelectedItem as CsQuartier;
                    if (quartier != null)
                    {
                        txt_Quartier.Text = quartier.CODE ?? string.Empty;
                        this.Cbo_Quartier.Tag = quartier.PK_ID;
                        List<ServiceAccueil.CsSecteur> lstSecteur = SessionObject.LstSecteur.Where(t => t.FK_IDQUARTIER == quartier.PK_ID).ToList();

                        this.Cbo_Secteur.ItemsSource = lstSecteur;
                        Cbo_Secteur.SelectedValuePath = "PK_ID";
                        Cbo_Secteur.DisplayMemberPath = "LIBELLE";
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void RemplirListeDesQuartierExistant()
        {
            try
            {

                if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
                {
                    _listeDesQuartierExistant = SessionObject.LstQuartier;
                    return;
                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesQartiersAsync();
                service.ChargerLesQartiersCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstQuartier = args.Result;
                    _listeDesQuartierExistant = SessionObject.LstQuartier;
                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                        this.Txt_CodeSite.Tag = lstSite.First().CODE;
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
                Message.ShowError(ex.Message,"btn_Site_Click");
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
                this.Txt_CodeSite.Tag = leSite.CODE;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
                List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.CODESITE == this.Txt_CodeSite.Tag.ToString()).ToList();
                if (lsiteCentre.Count == 1)
                {
                    this.Txt_CodeCentre.Text = lsiteCentre.First().CODE;
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_CodeCentre.Tag = lsiteCentre.First().PK_ID;
                    RemplirCommuneParCentre(lsiteCentre.First());

                }
                else
                    this.Txt_LibelleCentre.Visibility = System.Windows.Visibility.Collapsed;
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
                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstCentrePerimetre.Where(t => t.CODESITE == this.Txt_CodeSite.Tag.ToString()).ToList());
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "CENTRE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, true, "Liste");
                    ctrl.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "btn_Centre_Click");
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
                RemplirCommuneParCentre(leCentre);
            }
            else
                this.btn_Centre.IsEnabled = true;

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //RechercheDemande(this.txt_Centre.Text, this.txt_NumDemande.Text, RecupereCriterDemande(), this.txt_dateDebut.Text, this.txt_datefin.Text, this.txt_dateDemande.Text, this.txt_demandeDebut.Text, this.txt_demandeFin.Text, RecupereStatusDemande());
            this.DialogResult = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        List<CsTdem> lstTypedemande = new List<CsTdem>();
        private void ChargerTypeDemande()
        {
            try
            {
                if (SessionObject.LstTypeDemande != null && SessionObject.LstTypeDemande.Count != 0)
                {
                    SessionObject.LstTypeDemande.ForEach(t => t.ISSELECT = false);
                    lstTypedemande = SessionObject.LstTypeDemande ;
                    dgt_Typedemande.ItemsSource = lstTypedemande.OrderBy(t=>t.LIBELLE ).ToList();
                    return;
                }
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneOptionDemandeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.LstTypeDemande = res.Result;
                    SessionObject.LstTypeDemande.ForEach(t => t.ISSELECT = false);
                    lstTypedemande = SessionObject.LstTypeDemande;
                    dgt_Typedemande.ItemsSource = lstTypedemande;

                };
                service1.RetourneOptionDemandeAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }
        private void RechercheDemande(int? idcentre, string numdem, List<string> LstTdem, string  datedebut,  string dateFin,
                                              string datedemande, string numerodebut, string numerofin, string status, string Commune, string Quatier, string Secteur, string Rue, string Porte, string Etage, string NumeroLot, string compteur, string nom)
        {
            try
            {
                List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
                DateTime? pDateDebut = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.IsDateValider(datedebut);
                DateTime? pDateFin = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.IsDateValider(dateFin);
                DateTime? pDatedemande = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.IsDateValider(datedemande);

                if ((pDateDebut == null && !string.IsNullOrEmpty(datedebut)) ||
                    (pDateFin == null && !string.IsNullOrEmpty(dateFin)) ||
                    (pDatedemande == null && !string.IsNullOrEmpty(datedemande)))
                    Message.ShowError(Langue.MsgDateInvalide, Langue.lbl_Menu);

                if (pDateDebut != null) pDateDebut = pDateDebut;
                if (pDateFin != null) pDateFin = pDateFin.Value.AddDays(1);

                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeDemandeCritereCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    LstDemande = res.Result;
                    if (LstDemande != null && LstDemande.Count != 0)
                    {
                        UcListInitialisation ctrl = new UcListInitialisation(LstDemande);
                        ctrl.Show();
                    }
                    else
                        Message.ShowInformation("Aucune demande trouvée", "Info");

                };
                service1.RetourneListeDemandeCritereAsync(idcentre, numdem, LstTdem, pDateDebut, pDateFin, pDatedemande, numerodebut, numerofin, status, Commune, Quatier, Secteur, Rue, Porte, Etage, NumeroLot, compteur,nom);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void DialogueClosed(object sender, EventArgs e)
        {
            DialogResult ctrs = sender as DialogResult;
            if (ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
            {
                ctrs.DialogResult = false;
                return;
            }
        }
        void Translate()
        {
            this.chk_Terminer.Content = Langue.chk_Terminer;
            this.chk_EnCaisse.Content = Langue.chk_EnCaisse;
            this.lbl_Demande.Content = Langue.lbl_Demande;
        }

        private string RecupereStatusDemande()
        {
            string critere = string.Empty;
            if (this.chk_Terminer.IsChecked == true)
                critere = SessionObject.Enumere.DemandeStatusPriseEnCompte;
            if (this.chk_EnAttente .IsChecked == true)
                critere = SessionObject.Enumere.DemandeStatusEnAttente ;
            if (this.chk_EnCaisse .IsChecked == true)
                critere = SessionObject.Enumere.DemandeStatusPasseeEncaisse  ;
            return critere;
        }
        private void btn_rechercher_Click(object sender, RoutedEventArgs e)
        {
            int? IdCentre = null;
            if (this.Txt_CodeCentre.Tag != null )
               IdCentre = (int)this.Txt_CodeCentre.Tag;

            RechercheDemande(IdCentre, this.txt_NumDemande.Text, RecupereCriterDemande(), this.Dtp_DateDebut.Text, this.Dtp_DateFin.Text, this.Dtp_Date.Text, this.txt_NumDemandeDebut.Text, txt_NumDemandeFin.Text, RecupereStatusDemande(), txt_Commune.Text, txt_Quartier.Text, txt_NumSecteur.Text, txt_NumRue.Text, Txt_Porte.Text, Txt_Etage.Text, txtPropriete.Text, txtCompteur.Text, txtNomClient.Text);
        }
        private List<string> RecupereCriterDemande()
        { 
            List<string> lstTypeDemande = null ;
          if (this.dgt_Typedemande.ItemsSource != null )
          {
              lstTypeDemande = new List<string>();
              lstTypeDemande = ((List<CsTdem>)this.dgt_Typedemande.ItemsSource).Where(t=>t.ISSELECT).Select(y=>y.CODE).ToList(); 
          }
          return lstTypeDemande;
        }
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsTdem>;
            if (dg.SelectedItem != null)
            {
                CsTdem SelectedObject = (CsTdem)dg.SelectedItem;
                if (SelectedObject.ISSELECT  == false)
                    SelectedObject.ISSELECT  = true;
                else
                    SelectedObject.ISSELECT  = false;
            }
        }

        private void RemplirCommuneParCentre(CsCentre centre)
        {
            try
            {

                if (_listeDesCommuneExistant != null && _listeDesCommuneExistant.Count > 0)
                    _listeDesCommuneExistantCentre = _listeDesCommuneExistant.Where(c => c.FK_IDCENTRE == centre.PK_ID) != null ? _listeDesCommuneExistant.Where(c => c.FK_IDCENTRE == centre.PK_ID).ToList() : new List<CsCommune>();
                txt_Commune.Text = string.Empty;
                Cbo_Commune.ItemsSource = _listeDesCommuneExistantCentre;
                Cbo_Commune.IsEnabled = true;
                Cbo_Commune.SelectedValuePath = "PK_ID";
                Cbo_Commune.DisplayMemberPath = "LIBELLE";

                Cbo_Commune.ItemsSource = _listeDesCommuneExistantCentre;
                if (_listeDesCommuneExistantCentre.Count > 0)
                {
                    if (_listeDesCommuneExistantCentre.Count == 1)
                        Cbo_Commune.SelectedItem = _listeDesCommuneExistantCentre[0];
                }
                else
                {
                    Message.ShowError("Aucune commune associé à ce centre", "Info");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirCommune()
        {
            try
            {
                if (SessionObject.LstCommune != null && SessionObject.LstCommune.Count != 0)
                {
                    _listeDesCommuneExistant = SessionObject.LstCommune;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCommuneAsync();
                service.ChargerCommuneCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    SessionObject.LstCommune = args.Result;
                    _listeDesCommuneExistant = SessionObject.LstCommune;

                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void Cbo_Commune_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Commune.SelectedItem != null)
                {
                    CsCommune commune = Cbo_Commune.SelectedItem as CsCommune;
                    if (commune != null)
                    {
                        Cbo_Commune.SelectedItem = commune;
                        Cbo_Commune.Tag = commune.PK_ID;
                        txt_Commune.Text = commune.CODE ?? string.Empty;
                        RemplirQuartier(commune.PK_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void txt_Commune_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.txt_Commune.Text.Length == SessionObject.Enumere.TailleCommune)
                {
                    CsCommune laCommune = _listeDesCommuneExistantCentre.FirstOrDefault(t => t.CODE == this.txt_Commune.Text);
                    if (laCommune != null)
                    {
                        if ((this.Cbo_Commune.SelectedItem != null && (CsCommune)this.Cbo_Commune.SelectedItem != laCommune) || this.Cbo_Commune.SelectedItem == null)
                            this.Cbo_Commune.SelectedItem = laCommune;
                    }
                    else
                    {
                        Message.ShowInformation("Le code saisie n'existe pas", "Accueil");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private List<CsRues> _listeDesRuesExistant = null;
        private List<CsQuartier> _listeDesQuartierExistant = null;
        List<CsQuartier> ListeQuartierFiltres = new List<CsQuartier>();


        private void RemplirQuartier(int pCommuneId)
        {
            List<CsQuartier> QuartierParDefaut = null;

            this.txt_Quartier.Text = string.Empty;
            try
            {
                QuartierParDefaut = _listeDesQuartierExistant.Where(q => q.FK_IDCOMMUNE == pCommuneId).ToList();
                if (QuartierParDefaut != null && QuartierParDefaut.Count > 0)
                    ListeQuartierFiltres.AddRange(QuartierParDefaut);
                ListeQuartierFiltres.AddRange(_listeDesQuartierExistant.Where(q => q.FK_IDCOMMUNE == pCommuneId && q.CODE != DataReferenceManager.QuartierInconnu).ToList());

                if (ListeQuartierFiltres.Count > 0)
                    //foreach (var item in ListeQuartierFiltres)
                    //{
                    //    Cbo_Quartier.Items.Add(item);
                    //}
                    Cbo_Quartier.ItemsSource = null;
                Cbo_Quartier.ItemsSource = ListeQuartierFiltres;
                Cbo_Quartier.SelectedValuePath = "PK_ID";
                Cbo_Quartier.DisplayMemberPath = "LIBELLE";
                Cbo_Quartier.ItemsSource = ListeQuartierFiltres;

                //Cbo_Quartier.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}


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
using Galatee.Silverlight.ServiceRecouvrement;
//using Galatee.Silverlight.ServicePrintings;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmSaisiFraisCoupure : ChildWindow
    {
        List<CsCentre> lstCentreCampagne = new List<CsCentre>();
        List<CsCAMPAGNE> lesCampagne = new List<CsCAMPAGNE>();
        ObservableCollection<CsDetailCampagne> lesClientCampagne = new ObservableCollection<CsDetailCampagne>();
        List<CsSite> lstSiteCampagne = new List<CsSite>();
        List<CsSite> lstAgentCampagne = new List<CsSite>();
        public FrmSaisiFraisCoupure()
        {
           try 
	        {	        
		         InitializeComponent();
                 ChargerDonneeDuSite();
                 ChargeTypeCoupure();
                 this.txtReferenceClient.MaxLength = SessionObject.Enumere.TailleClient;
                 this.txtOrdeClient.MaxLength = SessionObject.Enumere.TailleOrdre ;
            }
	        catch (Exception ex)
	        {
	          Message.ShowError(ex,Galatee.Silverlight.Resources.Langue.errorTitle);
	        }
        }
        List<int> lesCentreCaisse = new List<int>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                List<int> lstCentreSelect = new List<int>();

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                        ChargerPiaAgence(lstSite.First().CODE);

                        this.btn_Site.IsEnabled = false;
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
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                        ChargerPiaAgence(lstSite.First().CODE);

                        this.btn_Site.IsEnabled = false;
                    }
                    return;
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
                Message.ShowError(ex.Message, "Recouvrement");
            }
        }
        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeSite.Text) && Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    Galatee.Silverlight.ServiceAccueil.CsSite _LeSiteClient = Shared.ClasseMEthodeGenerique.RetourneObjectFromList(lstSite, this.Txt_CodeSite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeSiteClient.LIBELLE))
                    {
                        this.Txt_LibelleSite.Text = _LeSiteClient.LIBELLE;
                        this.Txt_CodeSite.Text = _LeSiteClient.CODE;
                        this.Txt_CodeSite.Tag = _LeSiteClient.PK_ID;
                        ChargerPiaAgence(_LeSiteClient.CODE);

                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

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
                this.Txt_CodeSite.Tag = leSite.PK_ID;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
            }
            this.btn_Site.IsEnabled = true;
        }
        List<Galatee.Silverlight.ServiceAccueil.CsTypeCoupure> lstTypeCoupure = new List<Galatee.Silverlight.ServiceAccueil.CsTypeCoupure>();
        private void ChargeTypeCoupure()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.RetourneTypeCoupureAsync();
                client.RetourneTypeCoupureCompleted += (es, result) =>
                {
                    try
                    {
                        if (result.Cancelled || result.Error != null)
                        {
                            string error = result.Error.Message;
                            Message.ShowError("Erreur à l'exécution du service", "SelectCentreCampagne");
                            return;
                        }

                        if (result.Result == null)
                        {
                            Message.ShowInformation("Aucune donnée trouvée", "SelectCentreCampagne");
                            return;
                        }
                        lstTypeCoupure = result.Result.Where(t => t.CODE != "BT" && t.CODE != "MT").ToList(); ;

                        lstTypeCoupure.ForEach(t => t.LIBELLE = t.LIBELLE + " " + t.COUT);
                        cbo_TypeCoupure.ItemsSource = null;
                        cbo_TypeCoupure.ItemsSource = lstTypeCoupure;
                        cbo_TypeCoupure.DisplayMemberPath = "LIBELLE";
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CsCAMPAGNE CampagneSelect;
        public CsDetailCampagne  ClientSelect;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<CsDetailCampagne> lesFacture = ((ObservableCollection<CsDetailCampagne>)this.lvwResultat.ItemsSource).ToList();
                List<CsLclient> leFrais = GetElementDeFrais(lesFacture.Where(t=>t.MONTANTFRAIS > 0 && !string.IsNullOrEmpty( t.TYPECOUPURE)).ToList() );
                if (leFrais != null && leFrais.Count != 0)
                   InsererCompteClient(leFrais);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
            //this.DialogResult = true;
        }
        private void InsererCompteClient(List<CsLclient> leFrais)
        {
            try
            {
                RecouvrementServiceClient clients = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                clients.InsererFraisPoseAsync (leFrais);
                clients.InsererFraisPoseCompleted += (es, result) =>
                {
                    try
                    {
                        if (result.Cancelled || result.Error != null)
                        {
                            string error = result.Error.Message;
                            Message.ShowError("Erreur à l'exécution du service", "InsererCompteClient");
                            return;
                        }
                        if (!string.IsNullOrEmpty( result.Result) )
                        {
                            Message.ShowInformation(result.Result, "Recouvrement");
                            return;
                        }
                        if (string.IsNullOrEmpty( result.Result)) 
                            Message.ShowInformation("Mise a jour effectuée", "Recouvrement");
                        //btnsearch_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private  List<CsLclient> GetElementDeFrais(List<CsDetailCampagne> LesCampagne)
        {
            List<CsLclient> LesFrais = new List<CsLclient>();
          
            try
            {
                foreach (var Campagne in LesCampagne)
                {
                    CsLclient Frais = new CsLclient();
                    Frais.CENTRE = Campagne.CENTRE;
                    Frais.CLIENT = Campagne.CLIENT;
                    Frais.ORDRE = Campagne.ORDRE;
                    Frais.REFEM = DateTime.Today.Date.Year.ToString("0000") + DateTime.Today.Date.Month.ToString("00");
                    Frais.IDCOUPURE = Campagne.IDCOUPURE;
                    Frais.COPER = SessionObject.Enumere.CoperFRP;
                    Frais.DENR = DateTime.Today.Date;
                    Frais.EXIGIBILITE = DateTime.Today.Date;
                    Frais.DATECREATION = DateTime.Today.Date;
                    //Frais.DATEMODIFICATION = DateTime.Today.Date;
                    Frais.DC = SessionObject.Enumere.Debit;
                    Frais.FK_IDCENTRE = Campagne.FK_IDCENTRE;
                    Frais.FK_IDCLIENT = Campagne.FK_IDCLIENT;
                    Frais.MATRICULE = UserConnecte.matricule;
                    Frais.MOISCOMPT = DateTime.Today.Date.Year.ToString("0000") + DateTime.Today.Date.Month.ToString("00");
                    Frais.MONTANT = Campagne.MONTANTFRAIS;
                    Frais.ISNONENCAISSABLE  = null ;
                    Frais.TOP1 = "8";
                    Frais.FK_IDTYPEDEVIS = Campagne.FK_IDTYPECOUPURE;
                    Frais.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                    Frais.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(u => u.CODE == SessionObject.Enumere.CoperFRP).PK_ID;
                    Frais.FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.FirstOrDefault(u => u.CODE == Frais.TOP1).PK_ID;
                    LesFrais.Add(Frais);
                }
                return LesFrais;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void ChargerPiaAgence(string CodeSite)
        {
            try
            {

                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.RetournePIAAgenceCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    List<CsUtilisateur> lstPia = new List<CsUtilisateur>();
                    lstPia.Add(new CsUtilisateur() { LIBELLE = "Aucun" });
                    lstPia.AddRange(args.Result);

                    this.cmbAgent.ItemsSource = null;
                    this.cmbAgent.ItemsSource = lstPia;
                    this.cmbAgent.DisplayMemberPath = "LIBELLE";
                    return;
                };
                service.RetournePIAAgenceAsync(CodeSite);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnReinitialiser_Click(object sender, EventArgs e)
        {
            try
            {
                this.lvwResultat.ItemsSource = null;
                this.dtpDate.Text = null;
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

         void Recherche(string CodeSite, string IdCampagne, int IdPia, DateTime? DateDebut, DateTime? DateFin, string Centre, string Client, string Ordre)
         {
             try
             {
                 RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                 client.RechercheClientCampagneCompleted += (ss, args) =>
                 {
                     try
                     {
                         if (args.Cancelled || args.Error != null)
                         {
                             string error = args.Error.Message;
                             Message.ShowError("Erreur à l'exécution du service", "SearchCampagne");
                             return;
                         }

                         if (args.Result == null || args.Result.Count == 0)
                         {
                             Message.ShowInformation("Aucune donnée trouvée", "SearchCampagne");
                             return;
                         }

                         List<CsDetailCampagne> detailcampagnes = new List<CsDetailCampagne>();
                         detailcampagnes = args.Result;
                         lesClientCampagne.Clear();
                         foreach (CsDetailCampagne item in detailcampagnes)
                             lesClientCampagne.Add(item);

                         this.lvwResultat.ItemsSource = null;
                         this.lvwResultat.ItemsSource = lesClientCampagne;


                     }
                     catch (Exception ex)
                     {
                         Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                     }
                 };
                 client.RechercheClientCampagneAsync(CodeSite, IdCampagne, IdPia, DateDebut, DateFin, Centre, Client, Ordre,2);


             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
    
         private void btnsearch_Click(object sender, RoutedEventArgs e)
         {
             try
             {
                 string CodeSite = string.Empty; string IdCampagne = string.Empty; int IdPia = 0; DateTime? DateDebut = null; DateTime? DateFin = null; string Centre = null; string Client = null; string Ordre = null;
                 if (!string.IsNullOrEmpty(this.Txt_CodeSite.Text)) CodeSite = this.Txt_CodeSite.Text;
                 if (!string.IsNullOrEmpty(this.Txt_NumCampagne.Text)) IdCampagne = this.Txt_NumCampagne.Text;
                 if (cmbAgent.SelectedItem != null) IdPia = ((CsUtilisateur)cmbAgent.SelectedItem).PK_ID;
                 if (this.dtpDate.SelectedDate != null) DateDebut = this.dtpDate.SelectedDate.Value;
                 if (this.dtpDateFin.SelectedDate != null) DateFin = this.dtpDateFin.SelectedDate.Value;
                 if (!string.IsNullOrEmpty(this.txtReferenceClient.Text)) Client = this.txtReferenceClient.Text;
                 if (!string.IsNullOrEmpty(this.txtOrdeClient.Text)) Ordre = this.txtOrdeClient.Text;
                 Recherche(CodeSite, IdCampagne, IdPia, DateDebut, DateFin, Centre, Client, Ordre);
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void cmbCampagne_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             CampagneSelect = (CsCAMPAGNE)this.cmbCampagne.SelectedItem;
         }

         private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
         {
             DataGrid dg = (sender as DataGrid);
             var allObjects = dg.ItemsSource as List<CsLclient>;
             if (dg.SelectedItem != null)
             {
                 CsDetailCampagne SelectedObject = (CsDetailCampagne)dg.SelectedItem;

                 if (SelectedObject.IsSelect  == false)
                     SelectedObject.IsSelect = true;
                 else
                     SelectedObject.IsSelect = false;
             }
         }

         private void cbo_TypeCoupure_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             if (this.lvwResultat.SelectedItem != null)
             {
                Galatee.Silverlight.ServiceAccueil.CsTypeCoupure typeCoupure = (Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem;
                ((CsDetailCampagne)this.lvwResultat.SelectedItem).TYPECOUPURE = typeCoupure.CODE;
                ((CsDetailCampagne)this.lvwResultat.SelectedItem).FK_TYPECOUPURE  = typeCoupure.PK_ID ;
                ((CsDetailCampagne)this.lvwResultat.SelectedItem).MONTANTFRAIS = typeCoupure.COUT;
                ReloadDatagraid();

             }
         }
         private void ReloadDatagraid()
         {
             try
             {
                 lvwResultat.IsReadOnly = false;
                 ObservableCollection<CsDetailCampagne> listCouranteDansLaGridTemp = new ObservableCollection<CsDetailCampagne>();

                 List<CsDetailCampagne> lstNonSaisie = lesClientCampagne.Where(f => f.DATECOUPURE == null).ToList();
                 foreach (CsDetailCampagne item in lstNonSaisie)
                     listCouranteDansLaGridTemp.Add(item);
                 if (listCouranteDansLaGridTemp.Count != 0)
                     lvwResultat.SelectedItem = listCouranteDansLaGridTemp.First();
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         private void Txt_NumCampagne_TextChanged(object sender, TextChangedEventArgs e)
         {
             if (this.Txt_NumCampagne.Text.Length ==16)
             {
                 if (lesCampagne.Where(u => u.IDCOUPURE == this.Txt_NumCampagne.Text) == null)
                 {
                     Message.ShowInformation("Numéro de campagne erroné", "Campagne");
                     return ;
                 }
             }
         }

         private void cmbAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             try
             {
                 if (this.cmbAgent.SelectedItem != null)
                     this.txtAgent.Text = ((CsUtilisateur)this.cmbAgent.SelectedItem).MATRICULE;
             }
             catch (Exception ex)
             {
                 Message.ShowInformation(ex.Message, "cmbAgent_SelectionChanged");
             }
         }
    }
}


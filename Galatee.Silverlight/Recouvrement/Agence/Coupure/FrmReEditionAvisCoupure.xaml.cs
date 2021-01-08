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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmReEditionAvisCoupure : ChildWindow
    {
        List<CsCentre> lstCentreCampagne = new List<CsCentre>();
        List<CsCAMPAGNE> lesCampagne = new List<CsCAMPAGNE>();
        List<CsSite> lstSiteCampagne = new List<CsSite>();
        List<CsSite> lstAgentCampagne = new List<CsSite>();
        public FrmReEditionAvisCoupure()
        {
           try 
	        {	        
		         InitializeComponent();
                 InitialiserControle();
                 this.Rdb_Detail.IsChecked = true;
                 prgBar.Visibility = System.Windows.Visibility.Collapsed;
	        }
	        catch (Exception ex)
	        {
	        Message.ShowError(ex,Galatee.Silverlight.Resources.Langue.errorTitle);
	        }
        }
        public FrmReEditionAvisCoupure(bool returnbackvalue)
        {
            try
            {
                InitializeComponent();
                btnOk.Visibility = Visibility.Visible;
                OKButton.Visibility = Visibility.Collapsed;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
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
        private void InitialiserControle()
        {
            try
            {
                ChargerDonneeDuSite();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     
        public event EventHandler Closed;
        public void ClosedEnventHandler()
        {
            if (this.Closed!=null)
	        {
                this.Closed(this, null);
	        }
        }
        public CsCAMPAGNE CampagneSelect;
 
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool IsListe = false;
                if (this.Rdb_Liste .IsChecked == true)
                    IsListe = true;
                else
                    IsListe = false;
               
                if (this.lvwResultat.SelectedItem != null )
                {
                    prgBar.Visibility = System.Windows.Visibility.Visible;
                    LayoutRoot.Cursor = Cursors.Wait;
                    List<ServiceRecouvrement.CsAvisDeCoupureClient> ligne = new List<ServiceRecouvrement.CsAvisDeCoupureClient>();
                    RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    client.returnAvisReedtionCoupureAsync(CampagneSelect, IsListe);
                    client.returnAvisReedtionCoupureCompleted += (ss, ee) =>
                        {
                            try
                            {
                                if (ee.Cancelled || ee.Error != null)
                                {
                                    string error = ee.Error.Message;
                                    Message.ShowError("error occurs while invoking remote procedure", "returnAvisReedtionCoupure");
                                    prgBar.Visibility = System.Windows.Visibility.Collapsed ;
                                    LayoutRoot.Cursor = Cursors.Arrow ;
                                    return;
                                }

                                if (ee.Result == null || ee.Result.Count == 0)
                                {
                                    Message.ShowInformation("no data found", Galatee.Silverlight.Resources.Langue.wcf_error);
                                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                    LayoutRoot.Cursor = Cursors.Arrow;
                                    return;
                                }

                                List<ServiceRecouvrement.CsAvisDeCoupureClient> dataTable = new List<ServiceRecouvrement.CsAvisDeCoupureClient>();
                                dataTable = ee.Result;
                                ligne.AddRange(dataTable);


                                //impression du recu de la liste of cut-off

                                Dictionary<string, string> param = new Dictionary<string, string>();
                                param.Add("pfirstzone", CampagneSelect.PREMIERE_TOURNEE );
                                param.Add("plastzone", CampagneSelect.DERNIERE_TOURNEE );
                                param.Add("pminiAmount", CampagneSelect.MONTANT_RELANCABLE  .ToString());
                                param.Add("pduedate", CampagneSelect.DATE_EXIGIBILITE.ToString() );
                                param.Add("pnombreCustomer", CampagneSelect.NOMBRE_CLIENT );
                                param.Add("pcampainNumber", CampagneSelect.IDCOUPURE );
                                param.Add("pperioddue", CampagneSelect.PERIODE_RELANCABLE );
                                param.Add("pnumberbill", CampagneSelect.NOMBRE_FACTURE   );
                                param.Add("pfirstroute", string.Empty );
                                param.Add("plastroute", string.Empty );
                                param.Add("pfirstcategorie", CampagneSelect.DEBUT_CATEGORIE );
                                param.Add("plastcategorie", CampagneSelect.FIN_CATEGORIE );
                                param.Add("pnamecontroller", CampagneSelect.AGENTPIA );
                                string key = Utility.getKey();

                                if (IsListe == false)
                                    Utility.ActionDirectOrientation<ServicePrintings.CsAvisDeCoupureClient, ServiceRecouvrement.CsAvisDeCoupureClient>(ligne, null, SessionObject.CheminImpression, "AvisDeCoupureDetail", "Recouvrement", true);
                                else
                                {
                                    List<ServiceRecouvrement.CsAvisDeCoupureClient> lstAvisEditer = new List<CsAvisDeCoupureClient>();
                                    var lstAvis = ligne.Select(h => new { h.CENTRE, h.CLIENT,h.ADRESSE , h.ORDRE, h.ORDTOUR, h.SOLDEDUE,h.NOMBREFACTURE , h.COMPTEUR, h.NOMABON }).Distinct().ToList();
                                    foreach (var item in lstAvis)
                                    {
                                        lstAvisEditer.Add(new CsAvisDeCoupureClient()
                                        {
                                            CENTRE = item.CENTRE,
                                            CLIENT = item.CLIENT,
                                            ORDRE = item.ORDRE,
                                            NOMABON = item.NOMABON,
                                            SOLDEDUE = item.SOLDEDUE,
                                            ADRESSE = item.ADRESSE,
                                            NOMBREFACTURE = item.NOMBREFACTURE,
                                            COMPTEUR = item.COMPTEUR,
                                            ORDTOUR = item.ORDTOUR
                                        });
                                    }
                                    if (Chk_Excel.IsChecked == true )
                                        Utility.ActionExportation<ServicePrintings.CsAvisDeCoupureClient, ServiceRecouvrement.CsAvisDeCoupureClient>(lstAvisEditer, param, string.Empty, SessionObject.CheminImpression, "AvisDeCoupureListe", "Recouvrement", true, "xlsx");
                                    else
                                        Utility.ActionDirectOrientation<ServicePrintings.CsAvisDeCoupureClient, ServiceRecouvrement.CsAvisDeCoupureClient>(lstAvisEditer, param, SessionObject.CheminImpression, "AvisDeCoupureListe", "Recouvrement", true);

                                }
                                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                LayoutRoot.Cursor = Cursors.Arrow;
                            }
                            catch (Exception ex)
                            {
                                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                            }

                        };
                }
                else
                {
                    ClosedEnventHandler();
                    this.DialogResult = false;
                }
                   
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                LayoutRoot.Cursor = Cursors.Arrow;
            }
        }

 
       private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }

       void Recherche(string CodeSite, string IdCampagne, int IdPia, DateTime? DateDebut, DateTime? DateFin )
       {
           try
           {
               RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
               client.RechercheCampagneAsync(CodeSite, IdCampagne, IdPia, DateDebut, DateFin, 1);
               client.RechercheCampagneCompleted += (ss, args) =>
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

                       List<CsCAMPAGNE> campagnes = new List<CsCAMPAGNE>();
                       campagnes = args.Result;
                       this.lvwResultat.ItemsSource = null;
                       this.lvwResultat.ItemsSource = campagnes;

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


         private void cmbAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             try
             {
                 if (cmbAgent.SelectedItem != null)
                 {
                     this.txtAgent.Tag = null;
                     CsUtilisateur leUser = ((CsUtilisateur)cmbAgent.SelectedItem);
                     if (leUser.LIBELLE != "Aucun")
                     {
                         this.txtAgent.Text = leUser.MATRICULE;
                         this.txtAgent.Tag = leUser.PK_ID;
                     }
                 }
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void btnsearch_Click(object sender, RoutedEventArgs e)
         {
             try
             {
                 try
                 {
                     string CodeSite = string.Empty; string IdCampagne = string.Empty; int IdPia = 0; DateTime? DateDebut = null; DateTime? DateFin = null; string Centre = null; string Client = null; string Ordre = null;
                     if (!string.IsNullOrEmpty(this.Txt_CodeSite.Text)) CodeSite = this.Txt_CodeSite.Text;
                     if (!string.IsNullOrEmpty(this.Txt_NumCampagne.Text)) IdCampagne = this.Txt_NumCampagne.Text;
                     if (cmbAgent.SelectedItem != null) IdPia = ((CsUtilisateur)cmbAgent.SelectedItem).PK_ID;
                     if (this.dtpDate.SelectedDate != null) DateDebut = this.dtpDate.SelectedDate.Value;
                     if (this.dtpDateFin.SelectedDate != null) DateFin = this.dtpDateFin.SelectedDate.Value;
                     Recherche(CodeSite, IdCampagne, IdPia, DateDebut, DateFin);
                 }
                 catch (Exception ex)
                 {
                     Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                 }
             }
             catch (Exception ex)
             {
              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             try
             {
                 OKButton.IsEnabled = true;
             
                 CampagneSelect = lvwResultat.SelectedItem as CsCAMPAGNE;
                 if (CampagneSelect != null )
                 {
                 }
                 //Campaign = (lvwResultat.SelectedItem as CsCAMPAGNE).IdCoupure;
             }
             catch (Exception ex)
             {
              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void btnOk_Click(object sender, RoutedEventArgs e)
         {
             try
             {
                 if (Closed != null)
                 {
                     //_campagnes = this.RecupererLesElementsSelectionnes();
                     Closed(this, new EventArgs());
                     this.DialogResult = true;
                 }
             }
             catch (Exception ex)
             {
              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
             }
         }

         private void btnreset_Click(object sender, RoutedEventArgs e)
         {

         }


         private void btnReinitialiser_Click(object sender, RoutedEventArgs e)
         {
             this.cmbAgent.SelectedItem = null;
             this.lvwResultat.ItemsSource = null;
             this.txtAgent.Text = string.Empty;
         }

         private void Txt_NumCampagne_TextChanged(object sender, TextChangedEventArgs e)
         {

         }
    }
}


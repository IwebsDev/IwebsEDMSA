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
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.ServiceFacturation;
using Galatee.Silverlight.Resources.Facturation ;
using Galatee.Silverlight;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmRejeterClient : ChildWindow
    {
        public FrmRejeterClient()
        {
            InitializeComponent();
            txtClient.MaxLength = SessionObject.Enumere.TailleClient;
            txtCentre.MaxLength = SessionObject.Enumere.TailleCentre ;
            txtOrdre.MaxLength = SessionObject.Enumere.TailleOrdre ;
            ChargerDonneeDuSite(string.Empty);
        }
        List<CsLotri> ListeLotri;
        List<CsEvenement> ListeEvenementAExtraire = new List<CsEvenement>();
        List<CsEvenement> ListeEvenementAAjouter = new List<CsEvenement>();
        private void InitCtrl()
        {
          
        }
        private void MiseAJourEvent(List<CsEvenement> lstEvenement)
        {

   

            FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
            service.RetirerClientLotFactCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == true)
                {
                    Message.ShowInformation("Client retiré du lot", Langue.LibelleModule);
                    chk_Tournee.IsChecked = true;
                    btnRechercher_Click(null, null);
                }
                else
                {
                    Message.ShowInformation("Une erreur est survenue lors de la mise a jour", Langue.LibelleModule);
                    return;
                }
            };
            service.RetirerClientLotFactAsync(lstEvenement);
            service.CloseAsync();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.OKButton.IsEnabled = false;
                List<CsEvenement> lesEvtDeLagrille = ((List<CsEvenement>)this.dataGrid1.ItemsSource).Where(t=>t.IsSaisi==true).ToList();
                List<CsEvenement> lstEvtRetirer = new List<CsEvenement>();
                foreach (CsEvenement item in lesEvtDeLagrille)
                   lstEvtRetirer.AddRange(ListeEvenementAExtraire.Where(t => t.FK_IDABON == item.FK_IDABON ).ToList());

                var ws = new MessageBoxControl.MessageBoxChildWindow(Langue.LibelleModule, "Voulez vous faire un rejet definitif", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                ws.OnMessageBoxClosed += (l, results) =>
                {
                    if (ws.Result == MessageBoxResult.OK)
                    {
                        lstEvtRetirer.ForEach(t => t.STATUS = SessionObject.Enumere.EvenementSupprimer);
                        MiseAJourEvent(lstEvtRetirer);
                        this.OKButton.IsEnabled = true ;

                    }
                    else
                    {
                        lstEvtRetirer.ForEach(t => t.STATUS = SessionObject.Enumere.EvenementRejeter);
                        MiseAJourEvent(lstEvtRetirer);
                        this.OKButton.IsEnabled = true;

                    }
                };
                ws.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.LibelleModule);
            }
        }

        bool IsToutSelect = false;
        private void IsSelectAll(bool IsSelection,int Grid)
        {
            try
            {
                if (IsSelection)
                {
                    this.SelectButton.Content = Langue.btn_DéselectionTout;
                    IsToutSelect = true;
                }
                else
                {
                    this.SelectButton.Content = Langue.btn_SélectionTout;
                    IsToutSelect = false;
                }
                if (Grid == 1)
                {
                    List<CsEvenement> lesEvtDeLagrille = (List<CsEvenement>)this.dataGrid1.ItemsSource;
                    lesEvtDeLagrille.ForEach(t => t.IsSaisi = IsToutSelect);
                }
                else
                {
                    ListeEvenementAAjouter.ForEach(t => t.IsSaisi = IsToutSelect);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void btnzone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<object> _LstObj = new List<object>();
                _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(DistinctTournee(ListeLotri.Where(t => lesCentre.Select(i=>i.PK_ID).Contains(t.FK_IDCENTRE) && t.NUMLOTRI == this.Txt_Lotri.Text).ToList()));

                 Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                 _LstColonneAffich.Add("CENTRE", "CENTRE");
                 _LstColonneAffich.Add("TOURNEE", "TOURNEE");

                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, true , "Tournée");
                ctrl.Closed += new EventHandler(btnTournee_OkClicked);
                ctrl.Show();

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }

        private void zones_OkClicked(object sender, EventArgs e)
        {
            try
            {
                Galatee.Silverlight.MainView.UcListeGenerique generiq = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (generiq.isOkClick)
                {
                    List<Galatee.Silverlight.ServiceAccueil.CsTournee> zones = new List<Galatee.Silverlight.ServiceAccueil.CsTournee>();
                    List<Galatee.Silverlight.ServiceAccueil.CsTournee> ListeDesZone = new List<Galatee.Silverlight.ServiceAccueil.CsTournee>();
                    if (generiq.IsMultiselect)
                    {
                        if (generiq.MyObjectList.Count != 0)
                        {
                            foreach (var p in generiq.MyObjectList)
                            {
                                ListeDesZone.Add((Galatee.Silverlight.ServiceAccueil.CsTournee)p);
                            }
                            this.cbo_tournee.ItemsSource = null;
                            this.cbo_tournee.ItemsSource = ListeDesZone;
                            this.cbo_tournee.Tag = ListeDesZone;
                            this.cbo_tournee.DisplayMemberPath = "CODE";
                            this.cbo_tournee.SelectedItem = ListeDesZone[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
       
        private void btn_Batch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<object> _LstObj = new List<object>();
                _LstObj = ClasseMEthodeGenerique.RetourneListeObjet( ClasseMethodeGenerique.DistinctLotri(ListeLotri));

                Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                _LstColonneAffich.Add("NUMLOTRI", "LOT");
                _LstColonneAffich.Add("PERIODE", "PERIODE");

                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                ctrl.Closed += new EventHandler(galatee_OkClickedBatch);
                ctrl.Show();
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, "Error");
            }
        }
       
        void galatee_OkClickedBatch(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsLotri _LeLotSelect = ctrs.MyObject as CsLotri;
                this.Txt_Lotri.Text = string.IsNullOrEmpty(_LeLotSelect.NUMLOTRI) ? string.Empty : _LeLotSelect.NUMLOTRI;
                this.Txt_periode.Text = string.IsNullOrEmpty(_LeLotSelect.PERIODE) ? string.Empty : Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA (  _LeLotSelect.PERIODE);
                this.Txt_Lotri.Tag = _LeLotSelect;
            }
        }

    
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitCtrl();
        }
 
        List<Galatee.Silverlight.ServiceAccueil.CsTournee> ListeDesZone = new List<ServiceAccueil.CsTournee>();
        private void Txt_Lotri_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        List<ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        private void ChargerDonneeDuSite(string Periode)
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
                    foreach (ServiceAccueil.CsSite item in lstSite)
                        lesDeCentre.Add(item.CODE, lesCentre.Where(i => i.CODESITE == item.CODE).Select(p => p.PK_ID).ToList());
                    ChargerPourRejet(lesDeCentre, true, Periode);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
                    foreach (ServiceAccueil.CsSite item in lstSite)
                        lesDeCentre.Add(item.CODE, lesCentre.Where(i => i.CODESITE == item.CODE).Select(p => p.PK_ID).ToList());
                    ChargerPourRejet(lesDeCentre, true, Periode);

                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChargerPourRejet(Dictionary<string, List<int>> lesDeCentre, bool IsLotCourant,string Periode)
        {
            ListeLotri = new List<CsLotri>();
            FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
            service.ChargerLotriPourRejetClientCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                ListeLotri = args.Result;
                this.btn_Batch.IsEnabled = true;
                //ListeLotri = ListeLotri.Where(t => lesCentreCaisse.Contains(t.FK_IDCENTRE)).OrderBy(p => p.NUMLOTRI).ToList();
            };
            service.ChargerLotriPourRejetClientAsync(lesDeCentre, UserConnecte.matricule, IsLotCourant, Periode);
            service.CloseAsync();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CsEvenement _LeEvtSelect = (CsEvenement)this.dataGrid1.SelectedItem;
                if (_LeEvtSelect != null)
                {
                    List<CsEvenement> lesEvtDeLagrille = (List<CsEvenement>)this.dataGrid1.ItemsSource ;

                    CsEvenement _LeRechercher = lesEvtDeLagrille.FirstOrDefault(p =>
                                                                                   p.FK_IDCENTRE == _LeEvtSelect.FK_IDCENTRE &&
                                                                                   p.CENTRE == _LeEvtSelect.CENTRE &&
                                                                                   p.CLIENT == _LeEvtSelect.CLIENT &&
                                                                                   p.ORDRE == _LeEvtSelect.ORDRE && 
                                                                                   p.PRODUIT == _LeEvtSelect.PRODUIT &&
                                                                                   p.PERIODE == _LeEvtSelect.PERIODE && 
                                                                                   p.TOURNEE == _LeEvtSelect.TOURNEE);
                    if (_LeRechercher != null)
                    {
                        if (_LeRechercher.IsSaisi  != true)
                            _LeRechercher.IsSaisi = true;
                        else _LeRechercher.IsSaisi = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex ;
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsToutSelect)
                    IsSelectAll(false,1);
                else
                    IsSelectAll(true,1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btnRechercher_Click(object sender, RoutedEventArgs e)
        {
            if (chk_Tournee.IsChecked == true )
            {
                List<CsLotri> lstLotr = new List<CsLotri>();

                if (ListeTourneeSelect != null && ListeTourneeSelect.Count != 0)
                    lstLotr = ListeTourneeSelect;

                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ChargerDesEvenementPourRejetCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    ListeEvenementAExtraire = args.Result.Where(t => t.STATUS != SessionObject.Enumere.EvenementRejeter).ToList();
                    dataGrid1.ItemsSource = RetourneDistinctClientFromEvenement(ListeEvenementAExtraire);
                };
                service.ChargerDesEvenementPourRejetAsync(lstLotr);
                service.CloseAsync();
            }
            else
            {
                CsClient leClient = new CsClient();
                leClient.CENTRE = this.txtCentre.Text;
                leClient.REFCLIENT = this.txtClient.Text;
                leClient.ORDRE = this.txtOrdre.Text;

                List<CsCentre> lstDesDistinctLotri = new List<CsCentre>();
                var lstRegroupementDistnct = ListeLotri.Select(t => new { t.CENTRE, t.FK_IDCENTRE }).Distinct().ToList();
                foreach (var item in lstRegroupementDistnct)
                    lstDesDistinctLotri.Add(new CsCentre { CODE  = item.CENTRE, PK_ID  = item.FK_IDCENTRE });

                 CsCentre leCentreClient = lstDesDistinctLotri.FirstOrDefault(t => t.CODE == leClient.CENTRE);
                if (leCentreClient != null)
                    leClient.FK_IDCENTRE = leCentreClient.PK_ID;
                else
                {
                    Message.ShowInformation("Centre inexistant", Langue.LibelleModule);
                    return;
                }
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ChargerDesEvenementClientLotCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result != null && args.Result.Count == 0)
                    {
                        Message.ShowInformation("Client non trouvé", Langue.LibelleModule);
                        return;
                    }
                    ListeEvenementAExtraire = args.Result;
                    dataGrid1.ItemsSource = RetourneDistinctClientFromEvenement(ListeEvenementAExtraire);
                };
                service.ChargerDesEvenementClientLotAsync(leClient,this.Txt_Lotri.Text );
                service.CloseAsync();
            
            }
        }
        private List<CsEvenement> RetourneDistinctClientFromEvenement(List<CsEvenement> lstEvenemnt)
        {
            List<CsEvenement> lstEvenementResult = new List<CsEvenement>();
            var lstLotDistinct = lstEvenemnt.Select(t => new {t.FK_IDCENTRE, t.CENTRE,t.CLIENT ,t.ORDRE,t.PERIODE ,t.TOURNEE ,t.FK_IDABON,t.FACTURE , t.MONTANT   }).Distinct().ToList();
            foreach (var item in lstLotDistinct)
                lstEvenementResult.Add(new CsEvenement { FK_IDCENTRE = item.FK_IDCENTRE, CENTRE = item.CENTRE, CLIENT = item.CLIENT, ORDRE = item.ORDRE, PERIODE = item.PERIODE, TOURNEE = item.TOURNEE, FK_IDABON = item.FK_IDABON, FACTURE = item.FACTURE , MONTANT = item.MONTANT });
            return lstEvenementResult;

        }

        public static List<CsLotri> DistinctTournee(List<CsLotri> lstLotInit)
        {
            List<CsLotri> lstLotResult = new List<CsLotri>();
            var lstLotDistinct = lstLotInit.Select(t => new {t.NUMLOTRI , t.CENTRE , t.FK_IDCENTRE,t.LIBELLECENTRE , t.TOURNEE , t.FK_IDTOURNEE  }).Distinct().ToList();
            foreach (var item in lstLotDistinct)
                lstLotResult.Add(new CsLotri {NUMLOTRI = item.NUMLOTRI ,  CENTRE =item.CENTRE, FK_IDCENTRE = item.FK_IDCENTRE , TOURNEE = item.TOURNEE, FK_IDTOURNEE = item.FK_IDTOURNEE });

            return lstLotResult;
        }

        public static List<CsLotri> DistinctCentre(List<CsLotri> lstLotInit)
        {
            List<CsLotri> lstLotResult = new List<CsLotri>();
            var lstLotDistinct = lstLotInit.Select(t => new { t.NUMLOTRI , t.CENTRE , t.FK_IDCENTRE,t.LIBELLECENTRE   }).Distinct().ToList();
            foreach (var item in lstLotDistinct)
                lstLotResult.Add(new CsLotri {NUMLOTRI=item.NUMLOTRI , CENTRE = item.CENTRE, FK_IDCENTRE = item.FK_IDCENTRE, LIBELLECENTRE = item.LIBELLECENTRE });

            return lstLotResult;
        }

        List<int> ListeIdTournee = new List<int>();
        List<CsLotri> ListeTourneeSelect = new List<CsLotri>();
        private void btnTournee_OkClicked(object sender, EventArgs e)
        {
            try
            {
                Galatee.Silverlight.MainView.UcListeGenerique generiq = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (generiq.isOkClick)
                {
                    if (generiq.MyObjectList != null && generiq.MyObjectList.Count != 0)
                    {
                        foreach (CsLotri item in generiq.MyObjectList)
                        {
                            ListeTourneeSelect.Add(item);
                            ListeIdTournee.Add(item.FK_IDTOURNEE.Value );
                            this.cbo_tournee.ItemsSource = null;
                            this.cbo_tournee.ItemsSource = ListeTourneeSelect;
                            this.cbo_tournee.DisplayMemberPath = "TOURNEE";
                            this.cbo_tournee.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void chk_Tournee_Checked(object sender, RoutedEventArgs e)
        {
            this.txtCentre.IsReadOnly = true;
            this.txtClient.IsReadOnly = true;
            this.txtOrdre.IsReadOnly = true;
            this.txtCentre.Text = string.Empty;
            this.txtClient.Text = string.Empty;
            this.txtOrdre.Text = string.Empty;
            this.chk_Client.IsChecked = false;

            this.btnzone.IsEnabled = true ;
        }

        private void chk_Tournee_Unchecked(object sender, RoutedEventArgs e)
        {
            this.btnzone.IsEnabled = false;
        }

        private void chk_Client_Unchecked(object sender, RoutedEventArgs e)
        {
            this.btnzone.IsEnabled = false;
            this.cbo_tournee.ItemsSource = null;

            this.txtCentre.IsReadOnly = true;
            this.txtClient.IsReadOnly = true;
            this.txtOrdre.IsReadOnly = true;
        }

        private void chk_Client_Checked(object sender, RoutedEventArgs e)
        {
            this.txtCentre.IsReadOnly = false;
            this.txtClient.IsReadOnly = false;
            this.txtOrdre.IsReadOnly = false;
            chk_Tournee.IsChecked = false;
        }

        private void txtCentre_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_Ajouter_Click(object sender, RoutedEventArgs e)
        {
            List<CsEvenement> lstEvtRetirer = ListeEvenementAAjouter.Where(t => t.IsSaisi == true).ToList();
            lstEvtRetirer.ForEach(t => t.STATUS = SessionObject.Enumere.EvenementReleve );
            MiseAJourEvent(lstEvtRetirer);
        }
    }
}


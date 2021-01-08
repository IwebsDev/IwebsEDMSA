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
    public partial class FrmDemandeDeFacturation : ChildWindow
    {
        public FrmDemandeDeFacturation()
        {
            InitializeComponent();
            IsDefacturation = true;
            this.Txt_utilisateur.Visibility = System.Windows.Visibility.Collapsed;
            ChargerDonneeDuSite();
            this.btn_Batch.IsEnabled = false;

        }
        string Action = string.Empty;
        public static string FactureIsoleIndex = "00002";
        public static string FactureResiliationIndex = "00001";
        public static string FactureAnnulatinIndex = "00003";
        public FrmDemandeDeFacturation(string _Action)
        {
            InitializeComponent();
            this.btn_Batch.IsEnabled = false;
            IsSimulation = (_Action == SessionObject.Enumere.Simulation ) ? true : false;
            IsDefacturation = (_Action == SessionObject.Enumere.Defacturation) ? true : false;
            IsNormal = (_Action == SessionObject.Enumere.Normal) ? true : false;
            IsDestructionSimulation = (_Action == SessionObject.Enumere.DestructionSimulation) ? true : false;
            Action = _Action;
            this.btn_Batch.IsEnabled = false;
            ChargerDonneeDuSite();

        }
        private void InitCtrl()
        {
        }

        List<CsLotri> ListeLotri;
        List<CsLotri> ListSelectionnee;
        ObservableCollection<CsLotri> _ListeLotriObs = new ObservableCollection<CsLotri>();
        bool IsSimulation = false;
        bool IsDefacturation = false;
        bool IsDestructionSimulation = false;
        bool IsNormal = false  ;
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.OKButton.IsEnabled = false;
            ListSelectionnee = new List<CsLotri>();
            ListSelectionnee = RetourneLotriSelectionner(_ListeLotriObs);
            if (ListSelectionnee != null && ListSelectionnee.Count != 0)
            {
                if (!IsDefacturation && !IsDestructionSimulation)
                {
                    UcTypeFacturation _ctrlDateExige = new UcTypeFacturation();
                    _ctrlDateExige.Closed += new EventHandler(galatee_OkClickedClient);
                    _ctrlDateExige.Show();
                }
                else
                {
                    string leMsg = string .Empty ;
                    if (IsDefacturation)
                          leMsg = Langue.msgDefacturation;
                    else
                        leMsg = Langue.msgSuppressionSimulation ;

                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.LibelleModule, leMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                            DefacturationLot(ListSelectionnee);
                    };
                    w.Show();
                }
            }
        }
        void galatee_OkClickedClient(object sender, EventArgs e)
        {
            UcTypeFacturation ctrs = sender as UcTypeFacturation;
            foreach (CsLotri item in _ListeLotriObs)
                if (item.IsSelect)
                {
                    if (IsSimulation)
                        item.ETATFAC4 = "S";
                    item.DATEEXIG = ctrs.MyDateExige;
                    item.EXIG = (Convert.ToDateTime( item.DATEEXIG) - System.DateTime.Today.Date).Days ;
                }
        }

        private void DefacturationLot(List<CsLotri> ListLotSelect)
        {
            int res = LoadingManager.BeginLoading(Galatee.Silverlight.Resources.Accueil.Langue.En_Cours);
            try
            {
                CsStatFacturation _laStat = new CsStatFacturation();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.DemandeDefacturerLotAsync (ListLotSelect);

                service.DemandeDefacturerLotCompleted   += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == true)
                        Message.Show("Demande de défacturation effectuée avec succès", "facturation");
                    else
                        Message.Show("Une erreur s'est produite lors de la demande ", "facturation");
                    LoadingManager.EndLoading(res);
                    this.DialogResult = true;
                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }
        }

        private List<CsLotri> RetourneLotriSelectionner(ObservableCollection<CsLotri> _ListLotri)
        {
            List<CsLotri> _ListeLotriSelect = new List<CsLotri>();
            foreach (CsLotri item in _ListeLotriObs)
                if (item.IsSelect)
                {
                    item.ETATFAC6 = item.JET;
                    _ListeLotriSelect.Add(item);
                }
            return _ListeLotriSelect;
        }
        private void IsSelectAll(bool IsSelection)
        {
            if (IsSelection)
            {
                this.SelectBoutou.Content = Langue.btn_DéselectionTout;
                IsToutSelect = true;
            }
            else
            {
                this.SelectBoutou.Content = Langue.btn_SélectionTout;
                IsToutSelect = false;
            }
            List<CsLotri> _ListeLotriSelect = new List<CsLotri>();
            foreach (CsLotri item in _ListeLotriObs)
                item.IsSelect = IsSelection;

            dataGrid1.ItemsSource = null;
            dataGrid1.ItemsSource = _ListeLotriObs;
        }
       

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void btn_Batch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<object> _LstObj = new List<object>();
                _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(ClasseMethodeGenerique.DistinctLotriJet(ListeLotri));
                Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                _LstColonneAffich.Add("NUMLOTRI", "LOT");
                _LstColonneAffich.Add("PERIODE", "PERIODE");
                _LstColonneAffich.Add("JET", "JET");

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
        CsLotri _LeLotSelect = new CsLotri();
        void galatee_OkClickedBatch(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                _LeLotSelect = ctrs.MyObject as CsLotri;
                this.Txt_Lotri.Text = string.IsNullOrEmpty(_LeLotSelect.NUMLOTRI) ? string.Empty : _LeLotSelect.NUMLOTRI;
                this.Txt_periode.Text = string.IsNullOrEmpty(_LeLotSelect.PERIODE) ? string.Empty : _LeLotSelect.PERIODE;
                this.txt_jetEnCours.Text = string.IsNullOrEmpty(_LeLotSelect.JET) ? string.Empty : _LeLotSelect.JET;
            }
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitCtrl();
        }

        private void  ChargerDetailLotri(List<CsLotri> _LstLot,string _LeLotSelectionne,string JetSelectionne)
        {
            try
            {
                //dataGrid1.ItemsSource = _ListeLotriObs.Where(t=>t.ETATFAC5 ==  SessionObject.Enumere.NonMiseAbonne).ToList().OrderBy(p=>p.PRODUIT);

                List<CsLotri> _LstDetailParLot =new List<CsLotri>();
                _LstDetailParLot = _LstLot.Where(p => p.NUMLOTRI == _LeLotSelectionne && p.JET == JetSelectionne ).ToList();

              
               _ListeLotriObs = new ObservableCollection<CsLotri>();
               if (_LstDetailParLot != null && _LstDetailParLot.Count != 0)
               {
                   this.txt_jetEnCours.Text = string.IsNullOrEmpty(_LstDetailParLot[0].JET) ? "**" : JetSelectionne;
                
                    foreach (var item in _LstDetailParLot)
                    {
                        item.IsDejaFacturer = true;
                        if (string.IsNullOrEmpty(item.JET))
                            item.JET = "**";

                        if (string.IsNullOrEmpty(item.STATUS))
                            item.STATUS = item.ETATFAC1 + " " + item.ETATFAC2 + " " + item.ETATFAC5;

                        if (item.DFAC != null)
                            item.DFAC = System.DateTime.Now.Date;
                        if (item.DATEEXIG == null)
                            item.DATEEXIG = System.DateTime.Now.Date;
                        int EtatLotri = EtatDeLaLigneDeLotri(item);
                        if (EtatLotri == 0)
                            item.IsDejaFacturer = false;
                        _ListeLotriObs.Add(item);
                    }
               }
               dataGrid1.ItemsSource = null;
               dataGrid1.ItemsSource = _ListeLotriObs.OrderBy(p => p.CENTRE).ThenBy(t=>t.PRODUIT ).ThenBy(u=>u.CATEGORIECLIENT ).ThenBy(f=>f.PERIODICITE ).ThenBy(b=>b.TOURNEE);
               dataGrid1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, "");
              
            }
        }
        private void Txt_Lotri_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.dataGrid1.ItemsSource = null;
            if (!string.IsNullOrEmpty(this.Txt_Lotri.Text))
                ChargerDetailLotri(ListeLotri, this.Txt_Lotri.Text,this.txt_jetEnCours.Text );
        }

        private int EtatDeLaLigneDeLotri(CsLotri _leLotri)
        {
            int EtatLotri = 0;

            if (_leLotri.ETATFAC5== "S")
                return EtatLotri;

            if (_leLotri.ETATFAC1== "P"  || _leLotri.ETATFAC1== "T")
                EtatLotri = 1;

            if (_leLotri.ETATFAC1== "D" || _leLotri.ETATFAC1== "E")
                EtatLotri |= 2;
          
            if (string.IsNullOrEmpty(_leLotri.ETATFAC2))
                EtatLotri |= 4;

            // ctrl
            if (string.IsNullOrEmpty(_leLotri.ETATFAC3))
                EtatLotri |= 8;

            // fac. imprimes
            if (string.IsNullOrEmpty(_leLotri.ETATFAC4))
                EtatLotri |= 16;

            // maj abonnes
            if (string.IsNullOrEmpty(_leLotri.ETATFAC5))
                EtatLotri |= 32;

            // majcomptes terminee
            if (_leLotri.ETATFAC5== "M")
                EtatLotri |= 64;

            return EtatLotri;
        }
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
                    foreach (ServiceAccueil.CsSite item in lstSite)
                        lesDeCentre.Add(item.CODE, LstCentre.Where(i => i.CODESITE == item.CODE).Select(p => p.PK_ID).ToList());
                    ChargerLotriPourDefacturation(lesDeCentre);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
                    foreach (ServiceAccueil.CsSite item in lstSite)
                        lesDeCentre.Add(item.CODE, LstCentre.Where(i => i.CODESITE == item.CODE).Select(p => p.PK_ID).ToList());
                    ChargerLotriPourDefacturation(lesDeCentre);
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
        private void ChargerLotriPourDefacturation(Dictionary<string, List<int>> lstSiteCentre)
        {
            ListeLotri = new List<CsLotri>();
            FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
            service.ChargerLotriPourDefacturationCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null || args.Result.Count == 0)
                    return;
                ListeLotri = args.Result;
                this.btn_Batch.IsEnabled = true;
            };
            service.ChargerLotriPourDefacturationAsync(lstSiteCentre, UserConnecte.matricule, false );
            service.CloseAsync();
        }
        

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CsLotri _LeLotSelect = (CsLotri)this.dataGrid1.SelectedItem;
                if (_LeLotSelect != null)
                {
                    CsLotri _LeRechercher = _ListeLotriObs.FirstOrDefault(p => p.CENTRE  == _LeLotSelect.CENTRE  && p.PRODUIT  == _LeLotSelect.PRODUIT  &&
                                                   p.PERIODE == _LeLotSelect.PERIODE && p.TOURNEE  == _LeLotSelect.TOURNEE  &&
                                                                                     p.CATEGORIECLIENT  == _LeLotSelect.CATEGORIECLIENT );
                    if (_LeRechercher != null)
                    {
                        if (_LeRechercher.IsSelect != true)
                        {
                            _LeRechercher.IsSelect = true;
                            this.txt_jetEnCours.Text = string.IsNullOrEmpty(_LeRechercher.JET) ? "**" : _LeRechercher.JET;
                        }
                        else _LeRechercher.IsSelect = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex ;
            }
        }
        bool IsToutSelect = false;
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsToutSelect)
                IsSelectAll(false);
            else
                IsSelectAll(true);
        }

        private void DeseletButon_Click(object sender, RoutedEventArgs e)
        {
            IsSelectAll(false );
        }
    }
}


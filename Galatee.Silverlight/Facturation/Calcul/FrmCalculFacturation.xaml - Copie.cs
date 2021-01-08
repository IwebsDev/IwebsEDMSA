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
    public partial class FrmCalculFacturation : ChildWindow
    {

        public FrmCalculFacturation()
        {
            InitializeComponent();
        }
        string Action = string.Empty;

        public FrmCalculFacturation(string _Action)
        {
            InitializeComponent();
            this.btn_Batch.IsEnabled = false;
            IsSimulation = (_Action == SessionObject.Enumere.Simulation ) ? true : false;
            IsDefacturation = (_Action == SessionObject.Enumere.Defacturation) ? true : false;
            IsNormal = (_Action == SessionObject.Enumere.Normal) ? true : false;
            IsDestructionSimulation = (_Action == SessionObject.Enumere.DestructionSimulation) ? true : false;
            Action = _Action;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            ChargerDonneeDuSite();

        }
        private void InitCtrl()
        {
        }

        List<CsLotri> ListeLotri;
        List<CsLotri> ListeLotriEntfac;
        List<CsLotri> ListSelectionnee;
        ObservableCollection<CsLotri> _ListeLotriObs = new ObservableCollection<CsLotri>();
        bool IsSimulation = false;
        bool IsDefacturation = false;
        bool IsDestructionSimulation = false;
        bool IsNormal = false  ;
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.OKButton.IsEnabled = false;
                ListSelectionnee = new List<CsLotri>();
                ListSelectionnee = RetourneLotriSelectionner(_ListeLotriObs, ListeLotri);
                if (ListSelectionnee != null && ListSelectionnee.Count != 0)
                {
                    if (!IsDefacturation && !IsDestructionSimulation)
                    {
                       if (ListSelectionnee.FirstOrDefault(y => (y.FACTURE != null && y.FACTURE.Contains("*"))) != null )
                        {
                            var w = new MessageBoxControl.MessageBoxChildWindow(Langue.LibelleModule, Langue.msgDestructionSimulation , MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                            w.OnMessageBoxClosed += (_, result) =>
                            {
                                if (w.Result == MessageBoxResult.OK)
                                {
                                    Action = SessionObject.Enumere.DestructionSimulation;
                                    DefacturationLot(ListSelectionnee, true);
                                }
                                else
                                    this.OKButton.IsEnabled = true;
                            };
                            w.Show();
                        }
                        else
                        {
                            UcDateExigible _ctrlDateExige = new UcDateExigible();
                            _ctrlDateExige.Closed += new EventHandler(galatee_OkClickedClient);
                            _ctrlDateExige.Show();
                        }
                    }
                    else
                    {
                        string leMsg = string.Empty;
                        if (IsDefacturation)
                            leMsg = Langue.msgDefacturation;
                        else
                            leMsg = Langue.msgSuppressionSimulation;

                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.LibelleModule, leMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            if (w.Result == MessageBoxResult.OK)
                                DefacturationLot(ListSelectionnee,false );
                        };
                        w.Show();
                    }
                }
                else
                    this.OKButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.LibelleModule);
            }
        }
        void galatee_OkClickedClient(object sender, EventArgs e)
        {
            try
            {
                UcDateExigible ctrs = sender as UcDateExigible;
                if (!ctrs.OkClick)
                {
                    this.OKButton.IsEnabled = true;
                    return;
                }
                foreach (CsLotri item in ListSelectionnee)
                    if (item.IsSelect)
                    {
                        if (IsSimulation)
                            item.ETATFAC4 = "S";
                        item.DATEEXIG = ctrs.MyDateExige;
                        item.EXIG = (Convert.ToDateTime(item.DATEEXIG) - System.DateTime.Today.Date).Days;
                        item.JET = string.IsNullOrEmpty(item.JET) ? this.txt_jetEnCours.Text : (int.Parse(item.JET) + 1).ToString("00");
                        item.USERMODIFICATION = UserConnecte.matricule;
                        item.MATRICULE  = UserConnecte.matricule;
                    }
                if (ctrs.IsFaturationTotal != null )
                {
                    bool EtatFact = (ctrs.IsFaturationTotal == true) ? true : false;
                    CalculerFacture(ListSelectionnee, IsSimulation, EtatFact);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur au calcul du lot", "Erreur");
            }
        }
        private void  CalculerFacture(List<CsLotri> ListLotSelect,bool IsSumulation,bool IsFacturationTotal)
        {
            prgBar.Visibility = System.Windows.Visibility.Visible ;
            try
            {
                List<CsFactureBrut> _laStat = new List<CsFactureBrut>();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.CalculeDuLotGeneralCompleted += (s, args) =>
                {
                    this.DialogResult = false;
                    if (args != null && args.Cancelled)
                    {
                        Message.Show(Langue.msgErrorFact, Langue.LibelleModule);
                        prgBar.Visibility = System.Windows.Visibility.Collapsed ;

                        return;
                    }
                    if (args == null && args.Cancelled)
                    {
                        Message.Show(Langue.msgErrorFact, Langue.LibelleModule);
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;

                        return;
                    }
                    _laStat = args.Result;
                    UcResultatFacturation ctrl = new UcResultatFacturation(_laStat, IsSimulation);
                    ctrl.Show();
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                };
                service.CalculeDuLotGeneralAsync(ListLotSelect, IsSumulation);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                throw ex;
            }
        }

        private void DefacturationLot(List<CsLotri> ListLotSelect,bool isDestructionLot)
        {
            prgBar.Visibility = System.Windows.Visibility.Visible ;
            try
            {
                CsStatFacturation _laStat = new CsStatFacturation();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.DefacturerLotCompleted  += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    _laStat = args.Result;
                    
                    if (_laStat != null)
                        if (!isDestructionLot)
                        {
                            Message.Show("Nombre de client :" + _laStat.NombreCalcule + "\r\n  Montant défacturé : " + decimal.Parse(_laStat.Montant.ToString()).ToString("N2"), "Statistique");
                            this.OKButton.IsEnabled = true;
                        }
                        else
                        {
                            Action = SessionObject.Enumere.Normal;
                            //UcTypeFacturation _ctrlDateExige = new UcTypeFacturation();
                            //_ctrlDateExige.Closed += new EventHandler(galatee_OkClickedClient);
                            //_ctrlDateExige.Show();

                            UcDateExigible _ctrlDateExige = new UcDateExigible();
                            _ctrlDateExige.Closed += new EventHandler(galatee_OkClickedClient);
                            _ctrlDateExige.Show();
                        }
                    prgBar.Visibility = System.Windows.Visibility.Collapsed  ;

                };
                service.DefacturerLotAsync(ListLotSelect, Action);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                throw ex;
            }
        }

        private List<CsLotri> RetourneLotriSelectionner(ObservableCollection<CsLotri> _ListLotri)
        {
            List<CsLotri> _ListeLotriSelect = new List<CsLotri>();
            foreach (CsLotri item in _ListeLotriObs)
                if (item.IsSelect)                
                    _ListeLotriSelect.Add(item);
                
            return _ListeLotriSelect;
        }
        private List<CsLotri> RetourneLotriSelectionner(ObservableCollection<CsLotri> _ListLotri, List<CsLotri> _LesLotri)
        {
            List<CsLotri> _ListeLotriSelect = new List<CsLotri>();
            foreach (CsLotri items in _ListeLotriObs)
                if (items.IsSelect)
                {
                    List<CsLotri> lstLotResult = new List<CsLotri>();
                    var lstLotDistinct = _LesLotri.Where(y=>y.NUMLOTRI == items.NUMLOTRI && y.CENTRE  == items.CENTRE  && y.PRODUIT==items.PRODUIT && 
                                                            y.CATEGORIECLIENT ==items.CATEGORIECLIENT && y.PERIODICITE == items.PERIODICITE && y.TOURNEE == items.TOURNEE )
                                                            .Select(t => new { t.NUMLOTRI, t.CENTRE, t.CATEGORIECLIENT, t.PRODUIT, t.PERIODICITE, t.DATEEXIG, t.TOURNEE, t.PERIODE, t.DFAC, t.FK_IDCATEGORIECLIENT, t.FK_IDCENTRE, t.FK_IDPRODUIT, t.FK_IDRELEVEUR, t.FK_IDTOURNEE,t.USERCREATION  }).Distinct().ToList();
                    foreach (var item in lstLotDistinct)
                        lstLotResult.Add(new CsLotri
                        {
                            NUMLOTRI = item.NUMLOTRI,
                            CENTRE = item.CENTRE,
                            CATEGORIECLIENT = item.CATEGORIECLIENT,
                            PRODUIT = item.PRODUIT,
                            PERIODICITE = item.PERIODICITE,
                            DATEEXIG = item.DATEEXIG,
                            TOURNEE = item.TOURNEE,
                            PERIODE = item.PERIODE,
                            DFAC = item.DFAC,
                            FK_IDCATEGORIECLIENT = item.FK_IDCATEGORIECLIENT,
                            FK_IDCENTRE = item.FK_IDCENTRE,
                            FK_IDPRODUIT = item.FK_IDPRODUIT,
                            FK_IDRELEVEUR = item.FK_IDRELEVEUR,
                            FK_IDTOURNEE = item.FK_IDTOURNEE,
                            MATRICULE = UserConnecte.matricule ,
                            EXIG = items.EXIG ,
                            USERCREATION = item.USERCREATION 
                        });

                    _ListeLotriSelect.AddRange(lstLotResult);
                }

            return _ListeLotriSelect;
        }

        bool IsToutSelect = false;
        private void IsSelectAll(bool IsSelection)
        {
            if (IsSelection)
            {
                this.SelectButton.Content = Langue.btn_DéselectionTout;
                IsToutSelect = true;
            }
          else 
            {
                this.SelectButton.Content = Langue.btn_SélectionTout;
                IsToutSelect = false ;
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
                _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(ClasseMethodeGenerique.DistinctLotri(ListeLotri));
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
                this.Txt_DateFacture.Text = System.DateTime.Now.ToShortDateString();
            }
        }
    
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitCtrl();
        }

        private List<CsLotri> ChargerDistinctLotriCalcule(List<CsLotri> _LstLot)
        {
            List<CsLotri> _LstDistinct = new List<CsLotri>();
            try
            {
                List<CsLotri> _ListTemp = new List<CsLotri>();

                if (_LstLot != null && _LstLot.Count != 0)
                {
                    _ListTemp = _LstLot.Where(p => p.ETATFAC5 == SessionObject.Enumere.NonMiseAJours).ToList();
                    foreach (CsLotri item in _ListTemp)
                    {
                        if (_LstDistinct.FirstOrDefault(p => p.NUMLOTRI == item.NUMLOTRI) != null)
                            continue;
                        else
                            _LstDistinct.Add(item);
                    }
                }
                return _LstDistinct;
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, "");
                return null;
            }
        }

        private List<CsLotri> ChargerDistinctProduit(List<CsLotri> _LstLot)
        {
            List<CsLotri> _LstDistinct = new List<CsLotri>();
            try
            {
                List<CsLotri> lstProduitLot = new List<CsLotri>();
                if (_LstLot.Count > 0)
                {
                    var lstProduitDistnct = _LstLot.Select(t => new { t.PRODUIT , t.FK_IDPRODUIT  }).Distinct().ToList();
                    foreach (var item in lstProduitDistnct)
                        lstProduitLot.Add(new CsLotri { PRODUIT = item.PRODUIT, FK_IDPRODUIT = item.FK_IDPRODUIT });
                }
                return lstProduitLot;
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, "");
                return null;
            }
        }
        private List<CsLotri> ChargerDistinctCentre(List<CsLotri> _LstLot)
        {
            try
            {
                List<CsLotri> lstCentreLot = new List<CsLotri>();
                if (_LstLot.Count > 0)
                {
                    var lstCentreDistnct = _LstLot.Select(t => new { t.CENTRE , t.FK_IDCENTRE,t.LIBELLECENTRE   }).Distinct().ToList();
                    foreach (var item in lstCentreDistnct)
                        lstCentreLot.Add(new CsLotri { CENTRE = item.CENTRE, FK_IDCENTRE = item.FK_IDCENTRE, LIBELLECENTRE = item.LIBELLECENTRE });
                }
                return lstCentreLot;
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, "");
                return null;
            }
        }
        private List<CsLotri> ChargerDistinctCategorie(List<CsLotri> _LstLot)
        {
            try
            {
                List<CsLotri> lstCategorieLot = new List<CsLotri>();
                if (_LstLot.Count > 0)
                {
                    var lstCategorieDistnct = _LstLot.Select(t => new { t.CATEGORIECLIENT , t.FK_IDCATEGORIECLIENT  }).Distinct().ToList();
                    foreach (var item in lstCategorieDistnct)
                        lstCategorieLot.Add(new CsLotri { CATEGORIECLIENT = item.CATEGORIECLIENT, FK_IDCATEGORIECLIENT = item.FK_IDCATEGORIECLIENT });
                }
                return lstCategorieLot;
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, "");
                return null;
            }
        }
        private List<CsLotri> ChargerDistinctTournee(List<CsLotri> _LstLot)
        {
            try
            {
                List<CsLotri> lstTourneeLot = new List<CsLotri>();
                if (_LstLot.Count > 0)
                {
                    var lstTourneeDistnct = _LstLot.Select(t => new { t.FK_IDTOURNEE , t.TOURNEE  }).Distinct().ToList();
                    foreach (var item in lstTourneeDistnct)
                        lstTourneeLot.Add(new CsLotri { FK_IDTOURNEE = item.FK_IDTOURNEE, TOURNEE = item.TOURNEE });
                }
                return lstTourneeLot;
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, "");
                return null;
            }
        }
        List<CsLotri> _LstDetailParLot = new List<CsLotri>();
        private void  ChargerDetailLotri(List<CsLotri> _LstLot,string _LeLotSelectionne)
        {
            try
            {
                //dataGrid1.ItemsSource = _ListeLotriObs.Where(t=>t.ETATFAC5 ==  SessionObject.Enumere.NonMiseAbonne).ToList().OrderBy(p=>p.PRODUIT);
                if (!IsDefacturation && !IsDestructionSimulation)
                    _LstDetailParLot = _LstLot.Where(p => p.NUMLOTRI == _LeLotSelectionne).ToList();
                else
                {
                    if(IsDefacturation)
                         _LstDetailParLot = _LstLot.Where(t => t.NUMLOTRI == _LeLotSelectionne && t.ETATFAC5 == SessionObject.Enumere.NonMiseAJours).ToList();
                    if(IsDestructionSimulation )
                        _LstDetailParLot = _LstLot.Where(t => t.NUMLOTRI == _LeLotSelectionne && t.ETATFAC5 == SessionObject.Enumere.NonMiseAJours && t.FACTURE.Contains("*")).ToList();
                }
               _ListeLotriObs = new ObservableCollection<CsLotri>();
               if (_LstDetailParLot != null && _LstDetailParLot.Count != 0)
               {
                   this.Txt_DateFacture.Text = System.DateTime.Now.ToShortDateString();
                   CsLotri leLotSimul = _LstDetailParLot.FirstOrDefault(y => (y.FACTURE != null && y.FACTURE.Contains("*")));
                   if (leLotSimul != null)
                   {
                       this.txt_jetEnCours.Text = leLotSimul.JET;
                       this.txt_JetSuivant.Text = (int.Parse(this.txt_jetEnCours.Text) + 1).ToString("00");
                   }
                   else
                   {
                       this.txt_jetEnCours.Text = string.IsNullOrEmpty(_LstDetailParLot[0].JET) ? "01" : (int.Parse(_LstDetailParLot[0].JET) + 1).ToString("00");
                       this.txt_JetSuivant.Text = (int.Parse(this.txt_jetEnCours.Text) + 1).ToString("00");
                   }
                   List<CsLotri> lstDetailAfficher = Facturation.ClasseMethodeGenerique.DistinctLigneLotri(_LstDetailParLot);
                    foreach (var item in lstDetailAfficher)
                    {
                        item.IsDejaFacturer = true;

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
               dataGrid1.ItemsSource = _ListeLotriObs.OrderBy(p => p.CENTRE).ThenBy(t => t.PRODUIT).ThenBy(u => u.CATEGORIECLIENT).ThenBy(f => f.PERIODICITE).ThenBy(b => b.TOURNEE);
               dataGrid1.Tag = _LstDetailParLot;

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
                ChargerDetailLotri(ListeLotri, this.Txt_Lotri.Text);
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
        List<int > lesCentrePerimetre = new List<int>();

        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                   List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite  = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                   foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                       lesCentrePerimetre.Add(item.PK_ID );
                   ChargerLotriAll(lesCentrePerimetre);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentrePerimetre.Add(item.PK_ID);

                    ChargerLotriAll(lesCentrePerimetre);
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerLotriAll(List<int> lstCentreHabilitation)
        {
            ListeLotri = new List<CsLotri>();
            FacturationServiceClient service = new FacturationServiceClient(Utility.Protocole(), Utility.EndPoint("Facturation"));
            service.ChargerLotriPourCalculCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null || args.Result .Count == 0)
                    return;
               
                ListeLotri = args.Result;
                this.btn_Batch.IsEnabled = true;
                ListeLotri = args.Result.Where(t => !ClasseMethodeGenerique.IsLotIsole(t)).OrderBy(p => p.NUMLOTRI).ToList();
                ListeLotri.AddRange(ClasseMethodeGenerique.LissLotIsoleUer(args.Result));
            };
            service.ChargerLotriPourCalculAsync(lstCentreHabilitation);
            service.CloseAsync();
        }
        private void ChargerLotriAllFromEntfac(List<int> lstCentreHabilitation)
        {
            ListeLotriEntfac = new List<CsLotri>();
            FacturationServiceClient service = new FacturationServiceClient(Utility.Protocole(), Utility.EndPoint("Facturation"));
            service.ChargerLotriFromEntfacCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null || args.Result.Count == 0)
                    return;

                ListeLotriEntfac = args.Result;
                //ChargerLotriAll(lstCentreHabilitation, ListeLotriEntfac);
            };
            service.ChargerLotriFromEntfacAsync(lstCentreHabilitation);
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
                            this.Txt_DateFacture.Text = System.DateTime.Now.ToShortDateString();
                            this.txt_jetEnCours.Text = string.IsNullOrEmpty(_LeRechercher.JET) ? "01" : (int.Parse(_LeRechercher.JET)+1).ToString("00");
                            this.txt_JetSuivant.Text = (int.Parse(this.txt_jetEnCours.Text) + 1).ToString("00");
                            this.Txt_periode.Text = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(_LeRechercher.PERIODE);
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

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsToutSelect)
                IsSelectAll(false);
            else
                IsSelectAll(true);
        }
        private void chk_Centre_Checked(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid1.ItemsSource != null)
            {
                Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                _LstColonneAffich.Add("CENTRE", "CENTRE");
                _LstColonneAffich.Add("LIBELECENTRE", "LIBELLE");

                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(ChargerDistinctCentre(((ObservableCollection<CsLotri>)this.dataGrid1.ItemsSource ).ToList()));
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, true , "Centre");
                ctrl.Closed += new EventHandler(galatee_OkClickedCentreDistinct);
                ctrl.Show();
            }
                
        }

        void galatee_OkClickedCentreDistinct(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                if (ctrs.MyObjectList != null && ctrs.MyObjectList.Count != 0)
                {
                    ObservableCollection<CsLotri> lstLot = (ObservableCollection<CsLotri>)this.dataGrid1.ItemsSource;
                    ObservableCollection<CsLotri> lstLotCentre = new ObservableCollection<CsLotri>();
                    foreach (CsLotri  item in ctrs.MyObjectList)
                    {
                        List<CsLotri> lstLotCentreObs = lstLot.Where(t => t.CENTRE == item.CENTRE && t.FK_IDCENTRE == item.FK_IDCENTRE).ToList();
                        foreach (CsLotri items in lstLotCentreObs)
                            lstLotCentre.Add(items);
                    }
                    dataGrid1.ItemsSource = lstLotCentre;
                    dataGrid1.Tag = lstLotCentre;
                }
            }
        }

        private void chk_Centre_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_ListeLotriObs != null && _ListeLotriObs.Count != 0)
            {
                dataGrid1.ItemsSource = null;
                dataGrid1.ItemsSource = _ListeLotriObs;
            }
        }
    }
}


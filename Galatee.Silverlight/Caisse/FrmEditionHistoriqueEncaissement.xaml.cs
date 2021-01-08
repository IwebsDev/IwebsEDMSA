using Galatee.Silverlight.Devis;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.ServiceCaisse;
using Galatee.Silverlight.ServiceRecouvrement;
using Galatee.Silverlight.Shared;
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
using Galatee.Silverlight.Resources.Caisse;

namespace Galatee.Silverlight.Caisse
{
    public partial class FrmEditionHistoriqueEncaissement : ChildWindow
    {
        public FrmEditionHistoriqueEncaissement()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            LoadEtatCaisse();
        }

        private void LoadEtatCaisse()
        {
            string centre = string.Empty ;
            int fkidcentre = 0 ;
            string matricule = string.Empty  ;
            List<CsHabilitationCaisse> lstCaisseAEditer = new List<CsHabilitationCaisse>();
            Galatee.Silverlight.ServiceCaisse.CsHabilitationCaisse laCaisseSelect = this.TxtCaissier.Tag != null ? (Galatee.Silverlight.ServiceCaisse.CsHabilitationCaisse)TxtCaissier.Tag : null;
            Galatee.Silverlight.ServiceAccueil.CsCentre leCentreSelect = Txt_CodeCentre.Tag != null ? (Galatee.Silverlight.ServiceAccueil.CsCentre)Txt_CodeCentre.Tag : null;
            Galatee.Silverlight.ServiceAccueil.CsSite  leSiteSelect = Txt_CodeSite .Tag != null ? (Galatee.Silverlight.ServiceAccueil.CsSite )Txt_CodeSite.Tag : null;
            DateTime datedebut = dtp_debut.SelectedDate != null ? dtp_debut.SelectedDate.Value : new DateTime();
            DateTime datefin = dtp_fin.SelectedDate != null ? dtp_fin.SelectedDate.Value : new DateTime();

            /*
            if (leSiteSelect != null && !string.IsNullOrEmpty(leSiteSelect.CODE) && leCentreSelect == null)
            {
                List<int> LstidCentre = new List<int>();
                foreach (var item in _lesCentre.Where(t => t.FK_IDCODESITE == leSiteSelect.PK_ID).ToList())
                    LstidCentre.Add(item.PK_ID);

                if (datedebut == new DateTime() && datefin == new DateTime())
                    lstCaisseAEditer = _listDesCaisseOuverte.Where(t => LstidCentre.Contains(t.FK_IDCENTRE)).ToList();

                if (datedebut != new DateTime() && datefin == new DateTime())
                    lstCaisseAEditer = _listDesCaisseOuverte.Where(t => LstidCentre.Contains(t.FK_IDCENTRE) && t.DATE_DEBUT >= datedebut).ToList();

                if (datedebut != new DateTime() && datefin != new DateTime())
                    lstCaisseAEditer = _listDesCaisseOuverte.Where(t => LstidCentre.Contains(t.FK_IDCENTRE) && t.DATE_DEBUT >= datedebut && t.DATE_DEBUT <= datefin).ToList();

                if (datedebut == new DateTime() && datefin != new DateTime())
                    lstCaisseAEditer = _listDesCaisseOuverte.Where(t => LstidCentre.Contains(t.FK_IDCENTRE) && t.DATE_DEBUT <= datefin).ToList();
            }

            if (leCentreSelect != null && !string.IsNullOrEmpty(leCentreSelect.CODE) && laCaisseSelect == null )
            {
                centre= leCentreSelect.CODE ;
                fkidcentre =leCentreSelect.PK_ID ;

                if (datedebut == new DateTime() && datefin == new DateTime())
                    lstCaisseAEditer = _listDesCaisseOuverte.Where(t => t.FK_IDCENTRE == fkidcentre).ToList();

                if (datedebut != new DateTime() && datefin == new DateTime())
                    lstCaisseAEditer = _listDesCaisseOuverte.Where(t => t.FK_IDCENTRE == fkidcentre && t.DATE_DEBUT >= datedebut).ToList();

                if (datedebut != new DateTime() && datefin != new DateTime())
                    lstCaisseAEditer = _listDesCaisseOuverte.Where(t => t.FK_IDCENTRE == fkidcentre && t.DATE_DEBUT >= datedebut && t.DATE_DEBUT <= datefin).ToList();

                if (datedebut == new DateTime() && datefin != new DateTime())
                    lstCaisseAEditer = _listDesCaisseOuverte.Where(t => t.FK_IDCENTRE == fkidcentre && t.DATE_DEBUT <= datefin).ToList();
            }

            if (laCaisseSelect != null && !string.IsNullOrEmpty(laCaisseSelect.MATRICULE))
            {
                centre = laCaisseSelect.CENTRE;
                fkidcentre = leCentreSelect.PK_ID ;
                matricule = laCaisseSelect.MATRICULE;

                if (datedebut == new DateTime() && datefin == new DateTime())
                    lstCaisseAEditer = _listDesCaisseOuverte.Where(t => t.FK_IDCENTRE == fkidcentre && t.MATRICULE == laCaisseSelect.MATRICULE).ToList();

                if (datedebut != new DateTime() && datefin == new DateTime())
                    lstCaisseAEditer = _listDesCaisseOuverte.Where(t => t.FK_IDCENTRE == fkidcentre && t.MATRICULE == laCaisseSelect.MATRICULE && t.DATE_DEBUT >= datedebut).ToList();

                if (datedebut != new DateTime() && datefin != new DateTime())
                    lstCaisseAEditer = _listDesCaisseOuverte.Where(t => t.FK_IDCENTRE == fkidcentre && t.MATRICULE == laCaisseSelect.MATRICULE && t.DATE_DEBUT >= datedebut && t.DATE_DEBUT <= datefin).ToList();

                if (datedebut == new DateTime() && datefin != new DateTime())
                    lstCaisseAEditer = _listDesCaisseOuverte.Where(t => t.FK_IDCENTRE == fkidcentre && t.MATRICULE == laCaisseSelect.MATRICULE && t.DATE_DEBUT <= datefin).ToList();
            }
            if (lstCaisseAEditer.Count == 0)
            {
                Message.ShowInformation("Aucune caisse trouvée pour les critères saisis", Langue.LibelleModule);
                return;
            }
            prgBar.Visibility = System.Windows.Visibility.Visible ;
            CaisseServiceClient proxy = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            proxy.HistoriqueListeEncaissementsAsync(lstCaisseAEditer);
            proxy.HistoriqueListeEncaissementsCompleted += (senders, results) =>
             * */
            
            prgBar.Visibility = System.Windows.Visibility.Visible;
            CaisseServiceClient proxy = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            proxy.HistoriqueDesEncaissementsAsync(laCaisseSelect.MATRICULE, leCentreSelect.PK_ID, datedebut, datefin);
            proxy.HistoriqueDesEncaissementsCompleted += (senders, results) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed ;
                if (results.Cancelled || results.Error != null)
                {
                    string error = results.Error.Message;
                    MessageBox.Show("Erreur d'exécution.", "Caisse", MessageBoxButton.OK);
                    return;
                }
                if (results.Result == null || results.Result.Count == 0)
                {
                    Message.ShowInformation("Aucune donnée trouvée", "Caisse");

                    return;
                }

                List<ServiceCaisse.CsLclient> dataTable = new List<ServiceCaisse.CsLclient>();
                dataTable.AddRange(results.Result);
                
                //impression du recu de la liste of cut-off

                Dictionary<string, string> param = new Dictionary<string, string>();
                //param.Add("pUser", !string.IsNullOrWhiteSpace(SessionObject.LaCaisseCourante.MATRICULE) ? "Matricule : " + SessionObject.LaCaisseCourante.NOMCAISSE : "Matricule : Aucun");
                param.Add("pUser", !string.IsNullOrWhiteSpace(UserConnecte.matricule) ? "Matricule : " + SessionObject.LaCaisseCourante.NOMCAISSE : "Matricule : Aucun");
                param.Add("pDateDebut", dtp_debut.SelectedDate != null ? "Date de début : " + dtp_debut.SelectedDate.Value.ToShortDateString() : "Date de début : Aucune");
                param.Add("pDateFin", dtp_fin.SelectedDate != null ? "Date de fin : " + dtp_fin.SelectedDate.Value.ToShortDateString() : "Date de fin : Aucune");
                Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceCaisse.CsLclient>(dataTable, param,SessionObject.CheminImpression, "ListeDesTransactions".Trim(), "Caisse".Trim(),true );
            };
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> _lesCentre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> _lesSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                List<int> lesCentreCaisse = new List<int>();
                List<int> lesSite = new List<int>();
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    _lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    _lesSite = ClasseMEthodeGenerique.RetourneSiteByCentre(_lesCentre);

                    if (_lesSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = _lesSite.First().CODE;
                        Txt_LibelleSite.Text = _lesSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = _lesSite.First();
                    }
                    if (_lesCentre .Count == 1)
                    {
                        this.Txt_CodeCentre.Text = _lesCentre.First().CODE;
                        Txt_LibelleCentre.Text = _lesCentre.First().LIBELLE;
                        this.Txt_CodeCentre.Tag = _lesCentre.First();
                    }

                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in _lesCentre)
                        lesCentreCaisse.Add(item.PK_ID);
                    RetourneCaisseHabille(lesCentreCaisse);
                    return;

                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    _lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    _lesSite = ClasseMEthodeGenerique.RetourneSiteByCentre(_lesCentre);

                    if (_lesSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = _lesSite.First().CODE;
                        Txt_LibelleSite.Text = _lesSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = _lesSite.First();
                    }
                    if (_lesCentre.Count == 1)
                    {
                        this.Txt_CodeCentre.Text = _lesCentre.First().CODE;
                        Txt_LibelleCentre.Text = _lesCentre.First().LIBELLE;
                        this.Txt_CodeCentre.Tag = _lesCentre.First();
                    }

                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in _lesCentre)
                        lesCentreCaisse.Add(item.PK_ID);
                    RetourneCaisseHabille(lesCentreCaisse);
                };
                service.ListeDesDonneesDesSiteAsync(false );
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        List<CsHabilitationCaisse> _listDesCaisseOuverte = new List<CsHabilitationCaisse>();
        private void RetourneCaisseHabille(List<int> LstCentreCaisse)
        {
            try
            {
                CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint(this));
                service.RetourneListeCaisseHabiliteAsync(LstCentreCaisse);
                service.RetourneListeCaisseHabiliteCompleted += (sender, es) =>
                {
                    try
                    {
                        if (es.Cancelled || es.Error != null || es.Result == null)
                        {
                            Message.ShowError("Aucune donnée trouvée", Langue.errorTitle);
                            return;
                        }
                        _listDesCaisseOuverte = es.Result;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Langue.errorTitle);
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void galatee_OkClickedCaissiere(object sender, EventArgs e)
        {
            try
            {
                CsHabilitationCaisse _LeCaisseSelect = new CsHabilitationCaisse();
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.MyObject != null)
                {
                    _LeCaisseSelect = (CsHabilitationCaisse)ctrs.MyObject;
                    this.TxtCaissier.Text = _LeCaisseSelect.NOMCAISSE ;
                    this.TxtCaissier.Tag = _LeCaisseSelect;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.CsSite _LeSite = new Galatee.Silverlight.ServiceAccueil.CsSite();
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.MyObject != null)
                {
                    _LeSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                    this.Txt_LibelleSite.Text = _LeSite.LIBELLE;
                    this.Txt_CodeSite.Text = _LeSite.CODE;
                    this.Txt_CodeSite.Tag = _LeSite;
                    _lesCentre = SessionObject.LstCentre.Where(t => t.FK_IDCODESITE == _LeSite.PK_ID).ToList();
                    if (_lesCentre != null && _lesCentre.Count == 1)
                    {
                        this.Txt_LibelleCentre.Text = _lesCentre.First().LIBELLE;
                        this.Txt_CodeCentre.Text = _lesCentre.First().CODE;
                        this.Txt_CodeCentre.Tag = _lesCentre.First();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            try
            {
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.MyObject != null)
                {
                   Galatee.Silverlight.ServiceAccueil.CsCentre _LeCentre  = (Galatee.Silverlight.ServiceAccueil.CsCentre )ctrs.MyObject;
                    this.Txt_LibelleCentre .Text = _LeCentre.LIBELLE;
                    this.Txt_CodeCentre.Text = _LeCentre.CODE ;
                    this.Txt_CodeCentre.Tag = _LeCentre;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            List<object> _LstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(_lesSite.Where(t => t.CODE != "000").ToList());
            Dictionary<string, string> lstcolonne = new Dictionary<string, string>();
            lstcolonne.Add("CODE", "SITE");
            lstcolonne.Add("LIBELLE", "LIBELLE");
            Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstSite, lstcolonne, false, "Liste des sites");
            ctr.Show();
            ctr.Closed += new EventHandler(galatee_OkClickedSite);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<object> _Lstcentre = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(_lesCentre);
            Dictionary<string, string> lstcolonne = new Dictionary<string, string>();
            lstcolonne.Add("CODE", "CENTRE");
            lstcolonne.Add("LIBELLE", "LIBELLE");
            Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Lstcentre, lstcolonne, false, "Liste des centres");
            ctr.Show();
            ctr.Closed += new EventHandler(galatee_OkClickedCentre);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            List<object> _LstCaissiere = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(MethodeGenerics .RetourneDistincCaissier(_listDesCaisseOuverte));
            Dictionary<string, string> lstcolonne = new Dictionary<string, string>();
            lstcolonne.Add("NOMCAISSE", "NOM");
            lstcolonne.Add("MATRICULE", "MATRICULE");
            Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstCaissiere, lstcolonne, false, "Liste des caissiers");
            ctr.Show();
            ctr.Closed += new EventHandler(galatee_OkClickedCaissiere);
        }

   

    }
}


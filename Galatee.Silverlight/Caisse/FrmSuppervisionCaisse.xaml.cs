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
    public partial class FrmSuppervisionCaisse : ChildWindow
    {

        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentreDuSite = new List<ServiceAccueil.CsCentre>();
        public FrmSuppervisionCaisse()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            LoadEtatCaisse();
        }

        private void LoadEtatCaisse()
        {
            List<int> lstCentre = new List<int>();
            DateTime? DateCaisse = this.dtp_debut.Text == null ? null : this.dtp_debut.SelectedDate;
            bool IsEncour = false;
            if (this.Chk_EnCours.IsChecked == true)
                IsEncour = true;

            if (this.Cbo_Centre.Tag != null)
            {
                List<Galatee.Silverlight.ServiceAccueil.CsCentre> lstCentreSelect = Cbo_Centre.Tag as List<Galatee.Silverlight.ServiceAccueil.CsCentre>;
                foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in lstCentreSelect)
                    lstCentre.Add(item.PK_ID);
            }
            else if (this.Cbo_Site.Tag != null)
            {
                List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSiteSelect = Cbo_Site.Tag as List<Galatee.Silverlight.ServiceAccueil.CsSite>;
                foreach (Galatee.Silverlight.ServiceAccueil.CsSite item in lstSiteSelect)
                {
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> stCentreS = SessionObject.LstCentre.Where(t => t.FK_IDCODESITE == item.PK_ID).ToList();
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre items in stCentreS)
                        lstCentre.Add(items.PK_ID);
                }

            }
            else
            {
                if (Cbo_Centre.Tag == null && Cbo_Site.Tag == null)
                    lstCentre = SessionObject.LstIdCentreDuPerimetreAction;
            }
            //List<int> lstCentreCaisse = string.IsNullOrEmpty( this.Txt_CodeCentre .Text) ? SessionObject.LstIdCentreDuPerimetreAction:  ((CsCentre)Txt_CodeCentre.Tag).PK_ID  
            CaisseServiceClient service1 = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service1.RetourneSuppervisionCaisseCompleted += (es, argss) =>
            {
                List<CsHabilitationCaisse > LstHabilCaisse = new List<CsHabilitationCaisse>();
                if (argss != null && argss.Cancelled)
                    return;
                LstHabilCaisse = argss.Result ;
                dtgEtatCaisse.ItemsSource = null;
                dtgEtatCaisse.ItemsSource = LstHabilCaisse;

            };
            service1.RetourneSuppervisionCaisseAsync(lstCentre, DateCaisse, IsEncour);
            service1.CloseAsync();

        }
          

           
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        //private void ChargerDonneeDuSite()
        //{
        //    try
        //    {
        //        if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
        //        {
        //            SessionObject.LstCentreDuPerimetreAction = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
        //            SessionObject.LstSiteDuPerimetreAction = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(SessionObject.LstCentreDuPerimetreAction);
        //            foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in SessionObject.LstCentreDuPerimetreAction)
        //                SessionObject.LstIdCentreDuPerimetreAction.Add(item.PK_ID);
        //            return;
        //        } 
        //        AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //        service.ListeDesDonneesDesSiteCompleted += (s, args) =>
        //        {
        //            if (args != null && args.Cancelled)
        //                return;
        //            SessionObject.LstCentre = args.Result;
        //            SessionObject.LstCentreDuPerimetreAction = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
        //            SessionObject.LstSiteDuPerimetreAction = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(SessionObject.LstCentreDuPerimetreAction);
        //            foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in SessionObject.LstCentreDuPerimetreAction)
        //                SessionObject.LstIdCentreDuPerimetreAction.Add(item.PK_ID);
        //        };
        //        service.ListeDesDonneesDesSiteAsync(false);
        //        service.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in _lesCentre)
                        lesCentreCaisse.Add(item.PK_ID);
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
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in _lesCentre)
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

        List<CsHabilitationCaisse> _listDesCaisseOuverte = new List<CsHabilitationCaisse>();
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            try
            {
                List<Galatee.Silverlight.ServiceAccueil.CsSite> _LeSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.MyObject != null)
                {
                    _LeSite.Add((Galatee.Silverlight.ServiceAccueil.CsSite )ctrs.MyObject);
                    Cbo_Site.ItemsSource = null;
                    Cbo_Site.ItemsSource = _LeSite;
                    Cbo_Site.DisplayMemberPath = "LIBELLE";
                    Cbo_Site.SelectedItem = _LeSite[0];
                    this.Cbo_Site.Tag = _LeSite;
                    LstCentreDuSite = _lesCentre .Where(t => t.FK_IDCODESITE == _LeSite[0].PK_ID).ToList();
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
                List<Galatee.Silverlight.ServiceAccueil.CsCentre> _LeCentre = new List< Galatee.Silverlight.ServiceAccueil.CsCentre>();
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.MyObject != null)
                {
                    _LeCentre.Add((Galatee.Silverlight.ServiceAccueil.CsCentre)ctrs.MyObject);
                    Cbo_Centre.ItemsSource = null;
                    Cbo_Centre.ItemsSource = _LeCentre;
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";
                    Cbo_Centre.SelectedItem = _LeCentre[0];
                    this.Cbo_Centre.Tag = _LeCentre;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            List<object> _LstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(_lesSite );
            Dictionary<string, string> lstcolonne = new Dictionary<string, string>();
            lstcolonne.Add("CODE", "SITE");
            lstcolonne.Add("LIBELLE", "LIBELLE");
            Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstSite, lstcolonne, false, "Liste des sites");
            ctr.Show();
            ctr.Closed += new EventHandler(galatee_OkClickedSite);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            //LstCentreDuSite = (_lesCentre.Count == 0) ? _lesCentre : _lesCentre ;
            List<object> _Lstcentre = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstCentreDuSite);
            Dictionary<string, string> lstcolonne = new Dictionary<string, string>();
            lstcolonne.Add("CODE", "CENTRE");
            lstcolonne.Add("LIBELLE", "LIBELLE");
            Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Lstcentre, lstcolonne, false, "Liste des centres");
            ctr.Show();
            ctr.Closed += new EventHandler(galatee_OkClickedCentre);
        }

    }
}


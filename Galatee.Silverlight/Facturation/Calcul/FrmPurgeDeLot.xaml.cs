using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.ServiceFacturation;
using Galatee.Silverlight.Resources.Facturation;
using Galatee.Silverlight.MainView;

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmPurgeDeLot : ChildWindow
    {
        List<CsLotri> ListeLotri = new List<CsLotri>();
        public FrmPurgeDeLot()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }
        List<int> lesCentrePerimetre = new List<int>();

        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentrePerimetre.Add(item.PK_ID);
                    ChargerLotriAll(lesCentrePerimetre);
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
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentrePerimetre.Add(item.PK_ID);

                    ChargerLotriAll(lesCentrePerimetre);
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur au chargement des données", "Facturation");
            }
        }
  

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }
        private void ChargerLotriAll(List<int> idCentre)
        {
            try
            {
                ListeLotri = new List<CsLotri>();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ChargerLotriDejaMiseAJourCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    ListeLotri = args.Result;
                    if (ListeLotri == null || ListeLotri.Count == 0)
                        return;
                    ListeLotri = ListeLotri.ToList();
                    foreach (CsLotri item in ListeLotri)
                        if (item.EXIG != null )
                        item.DATEEXIG = Convert.ToDateTime(Convert.ToDateTime(item.DFAC) + TimeSpan.FromDays((int)item.EXIG));


                };
                service.ChargerLotriDejaMiseAJourAsync(idCentre,UserConnecte.matricule );
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        private void PurgerLot(List<CsLotri > lesLot)
        {
            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.PurgeDeLotCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == true)
                    {
                        Message.ShowInformation("Lot purger avec succès", "Facturation");
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                        this.DialogResult = true;
                    }

                };
                service.PurgeDeLotAsync(lesLot);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        private void btn_Batch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<object> _LstObj = new List<object>();
                _LstObj =Shared.ClasseMEthodeGenerique.RetourneListeObjet(ClasseMethodeGenerique.DistinctLotri(ListeLotri));
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
            }
        }

        private void Txt_Lotri_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Txt_Lotri.Text ))
            {
                dataGrid1.ItemsSource = null;
                dataGrid1.ItemsSource = ListeLotri.Where(t => t.NUMLOTRI == Txt_Lotri.Text).ToList();
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGrid1.ItemsSource != null)
                {
                    prgBar.Visibility = System.Windows.Visibility.Visible;

                    List<CsLotri> ListeLotPurger = new List<CsLotri>();

                    List<CsLotri> lstLotResult = new List<CsLotri>();
                    var lstLotDistinct = ((List<CsLotri>)dataGrid1.ItemsSource).ToList().Select(t => new { t.NUMLOTRI, t.PERIODE,t.CENTRE , t.FK_IDCENTRE,t.USERCREATION   }).Distinct().ToList();
                    foreach (var item in lstLotDistinct)
                        ListeLotPurger.Add(new CsLotri { NUMLOTRI = item.NUMLOTRI, PERIODE = item.PERIODE,CENTRE = item.CENTRE ,  FK_IDCENTRE = item.FK_IDCENTRE,USERCREATION=item.USERCREATION  });

                    PurgerLot(ListeLotPurger);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur a la purge du lot", "Facturation");
            }
        }
    }
}


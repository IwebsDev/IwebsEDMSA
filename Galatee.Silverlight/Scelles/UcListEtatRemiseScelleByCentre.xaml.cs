using Galatee.Silverlight.Resources.Scelles;
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.ServiceScelles;
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

namespace Galatee.Silverlight.Scelles
{

    public partial class UcListEtatRemiseScelleByCentre : ChildWindow
    {
        ObservableCollection<CsRemiseScelleByAg> DonnesDatagridLot = new ObservableCollection<CsRemiseScelleByAg>();
        Galatee.Silverlight.ServiceAccueil.CsDemandeBase laDemandeSelect = null;
        private List<ServiceAccueil.CsCentre> _listeDesCentreExistant = null;


        public UcListEtatRemiseScelleByCentre()
        {
            InitializeComponent();
            ChargerListDesSite();
        }
        public static List<CsRemiseScelleByAg> RetourneDistinctLot(List<CsRemiseScelleByAg> leLot)
        {
            try
            {
                List<CsRemiseScelleByAg> _lstLotriDistinct = new List<CsRemiseScelleByAg>();
                var ListLotriTemp = (from p in leLot
                                     group new { p } by new { p.Couleur_libelle, p.Date_Remise, p.lot_ID } into pResult
                                     select new
                                     {
                                         pResult.Key.Couleur_libelle,
                                         pResult.Key.Date_Remise,
                                         pResult.Key.lot_ID ,
                                         Nbre_Scelles = pResult.Count()
                                     });
                foreach (var item in ListLotriTemp)
                {
                    CsRemiseScelleByAg leLots = new CsRemiseScelleByAg()
                    {
                        Couleur_libelle = item.Couleur_libelle,
                        Date_Remise = item.Date_Remise,
                        lot_ID = item.lot_ID,
                        Nbre_Scelles = item.Nbre_Scelles 
                    };
                    _lstLotriDistinct.Add(leLots);
                }
                return _lstLotriDistinct;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void GetDataLot(int fk_Centre)
        {
            try
            {
                //UcReceptionLotScellesMagasinGeneral ctrl = new UcReceptionLotScellesMagasinGeneral();
                //ctrl.Closed += new EventHandler(RafraichirList);
                //ctrl.Show();
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneListScelleByCentreCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.LibelleReceptionScelle);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    DonnesDatagridLot.Clear();
                    if (args.Result != null)
                    {

                        foreach (var item in RetourneDistinctLot(args.Result))
                            DonnesDatagridLot.Add(item);
                    }
                    dgLotScelle.ItemsSource = DonnesDatagridLot;
                };
                client.RetourneListScelleByCentreAsync(fk_Centre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirCentreDuSite(int pIdSite, int pIdcentre)
        {
            try
            {
                Cbo_Centre.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    List<ServiceAccueil.CsCentre> lesCentreDuPerimetreAction = _listeDesCentreExistant.Where(p => p.FK_IDCODESITE == pIdSite).ToList();
                    if (lesCentreDuPerimetreAction != null)
                        foreach (var item in lesCentreDuPerimetreAction)
                        {
                            Cbo_Centre.Items.Add(item);
                        }
                    //Cbo_Centre.ItemsSource = lesCentreDuPerimetreAction;
                    Cbo_Centre.SelectedValuePath = "PK_ID";
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";

                    if (pIdcentre != 0)
                        this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First(t => t.PK_ID == pIdcentre);
                    if (_listeDesCentreExistant.Count == 1)
                        this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirCentrePerimetre(List<ServiceAccueil.CsCentre> lstCentre, List<CsSite> lstSite)
        {
            try
            {
                Cbo_Centre.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    if (lstCentre != null)
                        foreach (var item in lstCentre)
                        {
                            Cbo_Centre.Items.Add(item);
                        }
                    Cbo_Centre.SelectedValuePath = "PK_ID";
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";

                    if (lstSite != null)
                        foreach (var item in lstSite)
                        {
                            Cbo_Site.Items.Add(item);
                        }
                    Cbo_Site.SelectedValuePath = "PK_ID";
                    Cbo_Site.DisplayMemberPath = "LIBELLE";

                    if (lstSite != null && lstSite.Count == 1)
                        Cbo_Site.SelectedItem = lstSite.First();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        void ChargerListDesSite()
        {
            try
            {

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    _listeDesCentreExistant = lesCentre;
                    RemplirCentrePerimetre(lesCentre, lesSite);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteAsync(false);
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        if (SessionObject.LstCentre.Count != 0)
                        {
                            lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                            lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                            _listeDesCentreExistant = lesCentre;
                            RemplirCentrePerimetre(lesCentre, lesSite);
                        }
                        else
                        {
                            Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Centre.SelectedItem != null)
                {
                    ServiceAccueil.CsCentre centre = Cbo_Centre.SelectedItem as ServiceAccueil.CsCentre;
                    if (centre != null)
                    {
                        this.txtCentre.Text = centre.CODE ?? string.Empty;
                        this.txtCentre.Tag = centre.PK_ID;
                        //RemplirCommuneParCentre(centre);
                        //RemplirProduitCentre(centre);
                    }
                    //VerifierDonneesSaisiesInformationsDevis();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Caisse);
            }
        }

        private void Cbo_Site_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Site.SelectedItem != null)
                {
                    var csSite = Cbo_Site.SelectedItem as CsSite;
                    if (csSite != null)
                    {
                        this.txtSite.Text = csSite.CODE  ?? string.Empty;
                        if (laDemandeSelect != null)
                        {
                            if (laDemandeSelect.FK_IDCENTRE != 0)
                                RemplirCentreDuSite(csSite.PK_ID, laDemandeSelect.FK_IDCENTRE);
                        }
                        else
                            RemplirCentreDuSite(csSite.PK_ID, 0);

                    }
                }
                //VerifierDonneesSaisiesInformationsDevis();
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Languages.txt);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            int FK_Centre=((ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).PK_ID;
            GetDataLot(FK_Centre);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


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
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Caisse;
using Galatee.Silverlight.MainView;

namespace Galatee.Silverlight.Precontentieux
{
    public partial class FrmCampagnePrecontentieux : ChildWindow
    {
        public FrmCampagnePrecontentieux()
        {
            InitializeComponent();
            InitialiseCtrl();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        void InitialiseCtrl()
        {
            ChargerCentre ();
        }
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<Galatee.Silverlight.ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        private void ChargerCentre()
        {
            try
            {
                List<int> lstIdCentre = new List<int>();

                if (SessionObject.LstCentre.Count != 0)
                {
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lstIdCentre.Add(item.PK_ID);
                    if (lstSite != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_Site.Text = _LstSite[0].CODE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.Txt_Site.IsReadOnly = true;
                        }
                        this.btnSite.IsEnabled = true ;
                        this.btnCentre.IsEnabled = true ;
                    }
                    if (LstCentre != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsCentre> _LstCentre = LstCentre.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstCentre.Count == 1)
                        {
                            this.Txt_Centre.Text = _LstCentre[0].CODESITE;
                            this.Txt_LibelleCentre .Text = _LstCentre[0].LIBELLE;
                            this.Txt_Centre.Tag = _LstCentre[0].PK_ID;
                            this.Txt_Centre.IsReadOnly = true;
                        }
                        this.btnSite.IsEnabled = true;
                        this.btnCentre.IsEnabled = true;
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lstIdCentre.Add(item.PK_ID);
                    if (lstSite != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_Site.Text = _LstSite[0].CODE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.Txt_Site.IsReadOnly = true;
                        }
                        this.btnSite.IsEnabled = true;
                        this.btnCentre.IsEnabled = true;
                    }
                    if (LstCentre != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsCentre> _LstCentre = LstCentre.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstCentre.Count == 1)
                        {
                            this.Txt_Centre.Text = _LstCentre[0].CODESITE;
                            this.Txt_LibelleCentre.Text = _LstCentre[0].LIBELLE;
                            this.Txt_Centre.IsReadOnly = true;
                        }
                        this.btnSite.IsEnabled = true;
                        this.btnCentre.IsEnabled = true;
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false );
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "LoadCentre");

            }
        }
        private void btnCentre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentre != null)
                {
                        Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                        _LstColonneAffich.Add("CODE", "CENTRE");
                        _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                        List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstCentre);
                        MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Liste de centre");
                        ctrl.Closed += new EventHandler(centres_OkClicked);
                        ctrl.Show();

                    }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void centres_OkClicked(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsCentre _LeCentreSelect = ctrs.MyObject as Galatee.Silverlight.ServiceAccueil.CsCentre;
                    this.Txt_Centre .Text = string.IsNullOrEmpty(_LeCentreSelect.CODE) ? string.Empty : _LeCentreSelect.CODE;
                    this.Txt_LibelleCentre .Text = string.IsNullOrEmpty(_LeCentreSelect.LIBELLE ) ? string.Empty : _LeCentreSelect.LIBELLE ;
                    this.Txt_Centre.Tag = _LeCentreSelect.PK_ID;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btnSite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite != null)
                {
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "SITE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite.Where(t => t.CODE != "000").ToList());
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Liste de site");
                    ctrl.Closed += new EventHandler(Site_OkClicked);
                    ctrl.Show();


                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void Site_OkClicked(object sender, EventArgs e)
        {
            try
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsSite _LeSiteSelect = ctrs.MyObject as Galatee.Silverlight.ServiceAccueil.CsSite;
                    this.Txt_Site.Text = string.IsNullOrEmpty(_LeSiteSelect.CODE) ? string.Empty : _LeSiteSelect.CODE;
                    this.Txt_LibelleSite .Text = string.IsNullOrEmpty(_LeSiteSelect.LIBELLE) ? string.Empty : _LeSiteSelect.LIBELLE;
                    LstCentre = SessionObject.LstCentre.Where(t => t.FK_IDCODESITE == _LeSiteSelect.PK_ID).ToList();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            int Centre = 0;
            DateTime?  DateDebut =System.DateTime.Today ;
            DateTime? DateFin =System.DateTime.Today ;
            decimal SoldeDu = 1;
            if (this.Txt_Centre.Tag != null)
                Centre = (int)this.Txt_Centre.Tag;
            if (this.dtpDebut_Resil.SelectedDate != null)
                DateDebut = this.dtpDebut_Resil.SelectedDate;

            if (this.dtpFin_Resil.SelectedDate != null)
                DateFin = this.dtpFin_Resil.SelectedDate;

            if (!string.IsNullOrEmpty(this.Txt_SoldeDue.Text))
                SoldeDu = Convert.ToDecimal(this.Txt_SoldeDue.Text);

            CreeCampagnePrecontentieux(Centre, DateDebut, DateFin, SoldeDu);

        }
        private void CreeCampagnePrecontentieux(int idcentre,DateTime? DateDebut,DateTime? DateFin,decimal SoldeDu)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                client.CREE_CAMPAGNE_PRECONTENTIEUXCompleted += (es, result) =>
                {
                    try
                    {
                        prgBar.Visibility = System.Windows.Visibility.Collapsed ;
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
                      List<CsDetailCampagnePrecontentieux>  lesCampagne = result.Result;
                      Dictionary<string, string> param = new Dictionary<string, string>();
                      param.Add("pUser",UserConnecte.nomUtilisateur) ;
                      Utility.ActionDirectOrientation<ServicePrintings.CsDetailCampagnePrecontentieux, ServiceRecouvrement.CsDetailCampagnePrecontentieux>(lesCampagne, param, SessionObject.CheminImpression, "CampagnePrecontentieux", "Precontentieux", true);
                      this.DialogResult = true;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                client.CREE_CAMPAGNE_PRECONTENTIEUXAsync(idcentre ,DateDebut ,DateFin,SoldeDu ,UserConnecte.PK_ID ,UserConnecte.matricule  );
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        
    }
}


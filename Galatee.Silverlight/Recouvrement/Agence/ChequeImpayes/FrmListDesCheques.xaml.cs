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
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.ServicePrintings;
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceRecouvrement;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmReturnedChecList : ChildWindow
    {
        public FrmReturnedChecList()
        {
            InitializeComponent();
            txtParametre.Text = "1";
            this.btnAfficher.IsEnabled = (this.txtParametre.Text != string.Empty);
            ChargerDonneeDuSite();
        }

        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        Galatee.Silverlight.ServiceAccueil.CsSite lSiteSelect = new Galatee.Silverlight.ServiceAccueil.CsSite();
        List<Galatee.Silverlight.ServiceAccueil.CsProduit> lProduitSelect = new List<Galatee.Silverlight.ServiceAccueil.CsProduit>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    if (lstSite != null && lstSite.Count != 0)
                    {
                        if (lstSite.Count == 1)
                        {
                            lSiteSelect = lstSite.First();
                            this.txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        }
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.txt_libellecentre .Text = LstCentrePerimetre.First().CODE;
                        this.txt_libellecentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.txt_libellecentre.Tag = LstCentrePerimetre.First();
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
                    lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    if (lstSite != null && lstSite.Count != 0)
                    {
                        if (lstSite.Count == 1)
                        {
                            lSiteSelect = lstSite.First();
                            this.txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        }
                    }
                    if (LstCentrePerimetre != null && LstCentrePerimetre.Count != 0)
                    {
                        if (LstCentrePerimetre.Count == 1)
                        {
                            this.txt_libellecentre.Text = LstCentrePerimetre.First().LIBELLE;
                            this.txt_libellecentre.Tag = LstCentrePerimetre.First().PK_ID;
                            this.btn_Centre.IsEnabled = false;
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnprint_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                List<int> lstCentre = new List<int>();
                List<int> lst = new List<int>();
                if (this.txt_libellecentre.Tag != null)
                    //lstCentre.AddRange(((List<ServiceAccueil.CsCentre>)this.txt_libellecentre.Tag).Select(p => p.PK_ID).ToList());
                    lstCentre.AddRange(((List<int>)this.txt_libellecentre.Tag).ToList());
                else
                    lstCentre.AddRange(LstCentrePerimetre.Select(u => u.PK_ID).ToList());

                DateTime? dateDebut = string.IsNullOrEmpty(this.dtp_debut.Text) ? null : this.dtp_debut.SelectedDate ;
                DateTime? dateFin = string.IsNullOrEmpty(this.dtp_fin.Text) ? null : this.dtp_fin.SelectedDate;
                int Nombre = int.Parse(this.txtParametre.Text);

                RecouvrementServiceClient proxy = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                proxy.ListeDesChequesImpayesCompleted += (senders, results) =>
                    {
                        if (results.Cancelled || results.Error != null)
                        {
                            Message.Show("errror occurs while calling remote method", "Recouvrement");
                            return;
                        }

                        if (results.Result == null || results.Result.Count == 0)
                        {
                            Message.Show("Aucune donnée trouvée","Recouvrement");
                            return;
                        }

                        List<ServiceRecouvrement.CsDetailCampagne> dataTable = new List<ServiceRecouvrement.CsDetailCampagne>();
                        dataTable.AddRange(results.Result);

                        lvwResultat.ItemsSource = null;
                        lvwResultat.ItemsSource = dataTable;
                    };
                proxy.ListeDesChequesImpayesAsync(lstCentre ,dateDebut ,dateFin ,Nombre );
               

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                Message.Show(ex.Message, "Recouvrement");
                return;

            }
        }

        private void txtParametre_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.btnAfficher.IsEnabled = (this.txtParametre.Text != string.Empty);
        }

        private void Txt_Centre_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CODE");
                _LstColonneAffich.Add("LIBELLE");
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Site");
                ctrl.Closed += new EventHandler(galatee_OkClickedSite);
                ctrl.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    ServiceAccueil.CsSite param = (ServiceAccueil.CsSite)ctrs.MyObject;//.VALEUR;
                    this.Txt_Site.Text = param.CODE;
                    txt_LibelleSite.Text = param.LIBELLE;
                    this.Txt_Site.Tag = param.PK_ID;
                    this.btn_Centre.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ServiceAccueil.CsCentre> lCentre = new List<ServiceAccueil.CsCentre>(); 
                if (LstCentrePerimetre.Count != 0)
                {
                    if (this.Txt_Site.Tag != null)
                        lCentre = LstCentrePerimetre.Where(d => d.FK_IDCODESITE == (int)this.Txt_Site.Tag).ToList();
                    else
                        lCentre = LstCentrePerimetre;

                    //List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsCentre>(LstCentrePerimetre);
                    List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsCentre>(lCentre);
                    Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, true, "Centre");
                    ctr.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            try
            {

                Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
                if (ctrs.isOkClick)
                {
                    List<ServiceAccueil.CParametre> _LesCentreSelect = ctrs.MyObjectList as List<ServiceAccueil.CParametre>;
                    List<string> lstCentre = _LesCentreSelect.Select(t => t.CODE).ToList();
                    foreach (string item in lstCentre)
                        this.txt_libellecentre.Text = item + " ";
                    this.txt_libellecentre.Tag = _LesCentreSelect.Select(t => t.PK_ID).ToList();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btnAfficher_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (lvwResultat.ItemsSource != null)
            {
                List<ServiceRecouvrement.CsDetailCampagne> dataTable = (List<ServiceRecouvrement.CsDetailCampagne>)lvwResultat.ItemsSource;
                Utility.ActionDirectOrientation<ServicePrintings.CsDetailCampagne, ServiceRecouvrement.CsDetailCampagne>(dataTable, null, SessionObject.CheminImpression, "ChequeImpaye", "Recouvrement", true);
            }
        }

        private void btnAfficher_Copy1_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

        }

        private void btnAfficher_Copy2_Click_1(object sender, RoutedEventArgs e)
        {
            if (lvwResultat.ItemsSource != null)
            {
                List<ServiceRecouvrement.CsDetailCampagne> dataTable = (List<ServiceRecouvrement.CsDetailCampagne>)lvwResultat.ItemsSource;
                Utility.ActionExportation<ServicePrintings.CsDetailCampagne, ServiceRecouvrement.CsDetailCampagne>(dataTable, null, string.Empty, SessionObject.CheminImpression, "ChequeImpaye", "Recouvrement", true, "xlsx");
            }
        }
    }
}


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
    public partial class FrmListeDesCaisse : ChildWindow
    {
        public FrmListeDesCaisse()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            LoadEtatCaisse();
            //this.DialogResult = true;
        }

        private void LoadEtatCaisse()
        {
            string centre = !string.IsNullOrWhiteSpace(Txt_CodeCentre.Text) ? Txt_CodeCentre.Text : "";
            int idCentre = this.Txt_CodeCentre.Tag != null ? ((ServiceAccueil.CsCentre) this.Txt_CodeCentre.Tag).PK_ID  : 0;
            DateTime datedebut = dtp_debut.SelectedDate != null ? dtp_debut.SelectedDate.Value : new DateTime();
            DateTime datefin = dtp_fin.SelectedDate != null ? dtp_fin.SelectedDate.Value : new DateTime();
            bool IsCaisseFerme = (rdb_ferme.IsChecked== true)?true : false ;
            CaisseServiceClient proxy = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            proxy.ListeDesCaisseCompleted += (senders, results) =>
            {
                if (results.Cancelled || results.Error != null)
                {
                    string error = results.Error.Message;
                    MessageBox.Show("errror occurs while calling remote method", "EtatCaisse", MessageBoxButton.OK);
                    return;
                }
                if (results.Result == null || results.Result.Count == 0)
                {
                    MessageBox.Show("no data found");
                    Message.ShowInformation("Aucune donnée trouvée", "Caisse");

                    return;
                }

                List<ServiceCaisse.CsHabilitationCaisse> dataTable = new List<ServiceCaisse.CsHabilitationCaisse>();
                dataTable.AddRange(results.Result);
                //impression du recu de la liste of cut-off

                Dictionary<string, string> param = new Dictionary<string, string>();

                param.Add("pcentre", !string.IsNullOrWhiteSpace(this.Txt_LibelleCentre.Text) ? "Centre : " + this.Txt_LibelleCentre.Text : "Centre : Aucun");
                param.Add("pmatricule", "Matricule :Aucun");
                param.Add("pdatedebut", dtp_debut.SelectedDate != null ? "Date de début : " + dtp_debut.SelectedDate.ToString() : "Date de début : Aucune");
                param.Add("pdatefin", dtp_fin.SelectedDate != null ? "Date de fin : " + dtp_fin.SelectedDate.ToString() : "Date de fin : Aucune");
                Utility.ActionDirectOrientation<ServicePrintings.CsHabilitationCaisse, ServiceCaisse.CsHabilitationCaisse>(dataTable, param, SessionObject.CheminImpression, "ListeCaisse", "Caisse",true );
            };
            proxy.ListeDesCaisseAsync(idCentre, centre, datedebut, datefin, IsCaisseFerme);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCentre.OrderBy(p => p.CODE).ToList());
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClicked);
                ctr.Show();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        private void galatee_OkClicked(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsCentre _LaCateg = (Galatee.Silverlight.ServiceAccueil.CsCentre)ctrs.MyObject;
                this.Txt_CodeCentre.Text = _LaCateg.CODE;
                this.Txt_LibelleCentre.Text = _LaCateg.LIBELLE;
                this.Txt_CodeCentre.Tag = _LaCateg.PK_ID;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {

                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(lstSite.OrderBy(p => p.CODE).ToList());
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkSiteClicked);
                ctr.Show();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        private void galatee_OkSiteClicked(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsSite _LeSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = _LeSite.CODE;
                this.Txt_LibelleSite.Text = _LeSite.LIBELLE;
                LstCentre = SessionObject.LstCentre.Where(t => t.FK_IDCODESITE == _LeSite.PK_ID).ToList();
                if (LstCentre != null && LstCentre.Count == 1)
                {
                    this.Txt_LibelleCentre.Text = LstCentre.First().LIBELLE;
                    this.Txt_CodeCentre.Text = LstCentre.First().CODE;
                    this.Txt_CodeCentre.Tag = LstCentre.First();
                }
            }
        }

        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<Galatee.Silverlight.ServiceAccueil.CsCentre>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                List<int> lesCentreCaisse = new List<int>();
                if (SessionObject.LstCentre.Count != 0)
                {
                    LstCentre = SessionObject.LstCentre;
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = _LstSite[0].CODE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.IsReadOnly = true;
                        }
                    }
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentreCaisse.Add(item.PK_ID );
                    RetourneCaisseHabille(lesCentreCaisse);
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = SessionObject.LstCentre;
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = _LstSite[0].CODE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.IsReadOnly = true;
                        }
                    }
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentreCaisse.Add(item.PK_ID );
                    RetourneCaisseHabille(lesCentreCaisse);
                };
                service.ListeDesDonneesDesSiteAsync(false );
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerDonneeDuSite");

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
                            Message.ShowError("Erreur d'annulation du reçu. Veuillez réessayer svp !", Langue.errorTitle);
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

    }
}


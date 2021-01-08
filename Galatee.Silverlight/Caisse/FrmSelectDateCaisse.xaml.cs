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
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.ServiceCaisse;
using Galatee.Silverlight.Resources.Caisse ;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Caisse
{
    public partial class FrmSelectDateCaisse : ChildWindow
    {
        CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
        List<CsHabilitationCaisse> lstCaisseDate = new List<CsHabilitationCaisse>();
        string NumCaisseSelect = string.Empty;
        public FrmSelectDateCaisse()
        {
            InitializeComponent();

            this.txtMatriculeCaissier.MaxLength = SessionObject.Enumere.TailleMatricule;
            this.Title = Langue.lbl_controle;
            btn_Caissier.IsEnabled = false;
            Btn_Update.IsEnabled = false;
            ChargerDonneeDuSite();
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

                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in _lesCentre)
                        lesCentreCaisse.Add(item.PK_ID);
                    RetourneCaisseHabille(lesCentreCaisse);
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

        private void btn_Caissier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstCaisseDate == null || lstCaisseDate.Count == 0)
                {
                    lstCaisseDate = _listDesCaisseOuverte.Where(t => t.DATE_DEBUT.Value.ToShortDateString() == this.dtpDate.Text).ToList(); 
                }

                List<object> _LstCaissiere = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstCaisseDate);
                Dictionary<string, string> lstcolonne = new Dictionary<string, string>();
                lstcolonne.Add("CENTRE", "CENTRE");
                lstcolonne.Add("NUMCAISSE", "CAISSE");
                lstcolonne.Add("NOMCAISSE", "NOM");
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstCaissiere, lstcolonne, false, "Liste des caissiers");
                ctr.Show();
                ctr.Closed += new EventHandler(galatee_OkClickedCaissiere);

            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Langue.LibelleModule);
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
                    this.txtMatriculeCaissier.Text = _LeCaisseSelect.MATRICULE;
                    this.txt_CashierName .Text = _LeCaisseSelect.NOMCAISSE ;
                    this.txtMatriculeCaissier.Tag = _LeCaisseSelect;
                    Btn_Update.IsEnabled = true ;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtMatriculeCaissier.Tag != null)
            {
                frmEtatCaisse frm = new frmEtatCaisse((CsHabilitationCaisse)this.txtMatriculeCaissier.Tag);
                frm.Show();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void txtMatriculeCaissier_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void dtpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_listDesCaisseOuverte != null && _listDesCaisseOuverte.Count != 0)
            {
               lstCaisseDate = _listDesCaisseOuverte.Where(t => t.DATE_DEBUT.Value.ToShortDateString() == dtpDate.Text).ToList();
               btn_Caissier.IsEnabled = true;
                
            }
        }
    }
}


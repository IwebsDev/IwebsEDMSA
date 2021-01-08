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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmAvisDeCoupureSGC : ChildWindow
    {
        public FrmAvisDeCoupureSGC()
        {
            InitializeComponent();
            InitialiseCtrl();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_Periode.IsReadOnly = true;
            this.Txt_MontantPeriode.IsReadOnly = true;
            this.Txt_MontantGlobal.IsReadOnly = true;
            this.Txt_NombreFactureMontant .IsReadOnly = true;
            this.Txt_MontantFacture.IsReadOnly = true;

        }


        List<ServiceRecouvrement.CsAffectationGestionnaire> LstAffectation = new List<ServiceRecouvrement.CsAffectationGestionnaire>();
        List<ServiceRecouvrement.CsRegCli> LstCodeRegroupement = new List<ServiceRecouvrement.CsRegCli>();
        List<ServiceRecouvrement.CsRegCli> LstCodeRegroupement_User = new List<ServiceRecouvrement.CsRegCli>();

        List<aDisconnection> ligne = new List<aDisconnection>();
        private void   OKButton_Click(object sender, RoutedEventArgs e)
        {
            string IdCoupure = string.Empty;
             try
             {
                 CsAvisCoupureEdition avisCoupure = new CsAvisCoupureEdition();
               bntPrinting.IsEnabled = false;
               aCampagne Campagne = new aCampagne();
               aDisconnection dis = new aDisconnection();

               avisCoupure.ClientResilie = this.Chk_ExclusResil.IsChecked;
               avisCoupure.Matricule = UserConnecte.matricule;
               avisCoupure.AgentPia = UserConnecte.matricule;
               string DueDate = (string.IsNullOrEmpty(this.dtpDate.Text)) ? null : dtpDate.Text;
               if (this.Cbo_Regcli.Tag != null)
                  avisCoupure.ListeRegroupement =(List<ServiceRecouvrement.CsRegCli>)this.Cbo_Regcli.Tag  ;

               avisCoupure.Exigible = this.dtpDate.SelectedDate;
             
               if (this.Rdb_facturePeriode.IsChecked == true)
               {
                   if (string.IsNullOrEmpty(this.Txt_MontantPeriode.Text))
                   {
                       Message.ShowWarning("Remplisez le montant èxigible de la période", "Avis de coupure");
                       LayoutRoot.Cursor = Cursors.Arrow ;

                       return;
                   }
                   if (string.IsNullOrEmpty(this.Txt_Periode.Text))
                   {
                       Message.ShowWarning("Remplisez la période èxigible", "Avis de coupure");
                       return;
                   }
                   avisCoupure.Periode  = this.Txt_Periode.Text;
                   avisCoupure.SoldeClient  = string.IsNullOrEmpty(this.Txt_MontantPeriode.Text) ? 0 : int.Parse(this.Txt_MontantPeriode.Text);
               }
               else if (this.Rdb_MontantGlobal.IsChecked == true)
               {
                   if (string.IsNullOrEmpty(this.Txt_MontantGlobal.Text))
                   {
                       Message.ShowWarning("Remplisez le montant èxigible", "Avis de coupure");
                       return;
                   }
                   avisCoupure.SoldeClient = System.Convert.ToDecimal(string.IsNullOrEmpty(this.Txt_MontantGlobal.Text) ? "0" : this.Txt_MontantGlobal.Text);
               }
               else if (this.Rdb_NombreFactureMontant.IsChecked == true)
               {
                   if (string.IsNullOrEmpty(this.Txt_MontantFacture.Text))
                   {
                       Message.ShowWarning("Remplisez le montant èxigible", "Avis de coupure");
                       return;
                   }
                   if (string.IsNullOrEmpty(this.Txt_NombreFactureMontant.Text))
                   {
                       Message.ShowWarning("Remplisez le nombre de facture èxigible", "Avis de coupure");
                       return;
                   }
                   avisCoupure.MontantPeriode = string.IsNullOrEmpty(this.Txt_MontantFacture.Text) ? 0 : System.Convert.ToDecimal(this.Txt_MontantFacture.Text);
                   avisCoupure.TotalClient  = string.IsNullOrEmpty(this.Txt_NombreFactureMontant.Text) ? 0 : int.Parse(this.Txt_NombreFactureMontant.Text);
               }
                if (string.IsNullOrEmpty(avisCoupure.MatriculeDebut)  && !string.IsNullOrEmpty(avisCoupure.MatriculeFin))
                {
                    Message.ShowInformation("Entrez la première référence !", Galatee.Silverlight.Resources.Langue.errorTitle);
                    return ;
                }
                if (!string.IsNullOrEmpty(avisCoupure.MatriculeDebut) && string.IsNullOrEmpty(avisCoupure.MatriculeFin))
                {
                    Message.ShowInformation("Entrez la deuxième référence!", Galatee.Silverlight.Resources.Langue.errorTitle);
                    return ;
                }
           
                bool IsListe = false;
                ligne.Clear();
                if (this.rdbtnList.IsChecked == true)
                    IsListe = true;
                else
                    IsListe = false;
                        prgBar.Visibility = System.Windows.Visibility.Visible ;
                        LayoutRoot.Cursor = Cursors.Wait;
                        RecouvrementServiceClient proxy = new RecouvrementServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Recouvrement"));
                        proxy.TraitementAvisCoupureGCAsync(avisCoupure, dis, IsListe);
                        proxy.TraitementAvisCoupureGCCompleted += (ssn, results) =>
                            {
                                try
                                {
                                    if (results.Cancelled || results.Error != null)
                                    {
                                        string error = results.Error.Message;
                                        Message.ShowError("Erreur d'invocation du service. Réessayer svp !", Galatee.Silverlight.Resources.Langue.errorTitle);
                                        bntPrinting.IsEnabled = true;
                                        LayoutRoot.Cursor = Cursors.Arrow;
                                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                        return;
                                    }

                                    if (results.Result == null || results.Result.Count == 0)
                                    {
                                        Message.ShowError("Aucune donnée de campagne retournée!", Galatee.Silverlight.Resources.Langue.errorTitle);
                                        bntPrinting.IsEnabled = true;
                                        LayoutRoot.Cursor = Cursors.Arrow;
                                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                        return;
                                    }

                                    ligne.Clear();
                                    ligne.AddRange(results.Result);
                                    ServiceRecouvrement.CsRegCli lereg = ((List<ServiceRecouvrement.CsRegCli>)this.Cbo_Regcli.Tag).First();
                                    Dictionary<string, string> param = new Dictionary<string, string>();
                                    string key = Utility.getKey();
                                    if (IsListe == false)
                                       Utility.Action<ServicePrintings.aDisconnection , ServiceRecouvrement.aDisconnection>(ligne,key,null,SessionObject.CheminImpression,"AvisDeCoupureDetailSGC", "Recouvrement");
                                    else
                                        Utility.Action<ServicePrintings.aDisconnection, ServiceRecouvrement.aDisconnection>(ligne, key, null, SessionObject.CheminImpression, "AvisDeCoupureListeSGC", "Recouvrement");

                                    LayoutRoot.Cursor = Cursors.Arrow;
                                    prgBar.Visibility = System.Windows.Visibility.Collapsed  ;
                                    bntPrinting.IsEnabled = true;
                                }
                                catch (Exception ex)
                                {
                                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                    LayoutRoot.Cursor = Cursors.Arrow;
                                    bntPrinting.IsEnabled = true;
                                    Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                                }
                            };
            }
            catch (Exception ex)
            {
                bntPrinting.IsEnabled = true;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                LayoutRoot.Cursor = Cursors.Arrow;
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);

            }
        }
        private static int? TryToParse(string value)
        {
            int number;
            bool result = Int32.TryParse(value, out number);
            if (result)
            {
                return number;
            }
            else
            {
                return null;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void InitialiseCtrl()
        {
            ChargerCentre ();
            RemplirCodeRegroupement();
            RemplirAffectation();
            this.dtpDate.SelectedDate = System.DateTime.Today;
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
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                        }
                    }
                    if (LstCentre != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsCentre> _LstCentre = LstCentre.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstCentre.Count == 1)
                        {
                        }
                    }
                    return;
                }
                //Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new AccesServiceWCF().GetAcceuilClient();
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                        }
                    }
                    if (LstCentre != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsCentre> _LstCentre = LstCentre.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstCentre.Count == 1)
                        {
                        }
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
        private void rdbtnList_Checked_1(object sender, RoutedEventArgs e)
        {
            if (this.rdbtDetail !=  null && this.rdbtDetail.IsChecked == true )
            this.rdbtDetail.IsChecked = false;
        }

        private void rdbtDetail_Checked(object sender, RoutedEventArgs e)
        {
            if (this.rdbtnList != null &&   this.rdbtnList.IsChecked == true )
            this.rdbtnList.IsChecked = false;

        }

        private void Rdb_facturePeriode_Checked_1(object sender, RoutedEventArgs e)
        {
            this.Txt_MontantPeriode.IsReadOnly = false;
            this.Txt_Periode.IsReadOnly = false;

            this.Rdb_MontantGlobal.IsChecked = false;
            this.Rdb_NombreFactureMontant.IsChecked = false;
        }

        private void Rdb_facturePeriode_Unchecked_1(object sender, RoutedEventArgs e)
        {
            this.Txt_MontantPeriode.IsReadOnly = true;
            this.Txt_Periode.IsReadOnly = true;
        }

        private void Rdb_MontantGlobal_Checked_1(object sender, RoutedEventArgs e)
        {
            this.Txt_MontantGlobal.IsReadOnly = false;
            this.Rdb_facturePeriode.IsChecked = false;
            this.Rdb_NombreFactureMontant.IsChecked = false;
            this.Txt_MontantGlobal.Focus();
        }

        private void Rdb_NombreFactureMontant_Checked_1(object sender, RoutedEventArgs e)
        {
            this.Txt_NombreFactureMontant.IsReadOnly = false;
            this.Txt_MontantFacture.IsReadOnly = false;
            this.Txt_NombreFactureMontant.Focus();


            this.Rdb_facturePeriode.IsChecked = false;
            this.Rdb_MontantGlobal.IsChecked = false;
        }
        private void RemplirCodeRegroupement()
        {
            try
            {

                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.RetourneCodeRegroupementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    LstCodeRegroupement = args.Result;
                    if (LstAffectation != null)
                    {
                        ReLoadingGrid();
                    }
                    return;
                };
                service.RetourneCodeRegroupementAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private void RemplirAffectation()
        {
            try
            {
                if (LstAffectation.Count != 0)
                {
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    service.RemplirAffectationCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstAffectation = args.Result;
                        if (LstCodeRegroupement != null)
                        {
                            ReLoadingGrid();
                        }
                        return;
                    };
                    service.RemplirAffectationAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ReLoadingGrid()
        {
            try
            {
                var UtilisateurSelect = UserConnecte.PK_ID;
                var Affectation = LstAffectation.Where(a => a.FK_IDADMUTILISATEUR == UtilisateurSelect && a.ISACTIVE == true);
                if (Affectation != null)
                {
                    var ListIdRegcliCorrespondant = Affectation.Select(a => a.FK_IDREGLI);
                    LstCodeRegroupement_User = LstCodeRegroupement.Where(r => ListIdRegcliCorrespondant.Contains(r.PK_ID)).ToList();
                    if (LstCodeRegroupement_User.Count == 1)
                    {
                        this.Cbo_Regcli.ItemsSource = null;
                        this.Cbo_Regcli.DisplayMemberPath  = "NOM";
                        this.Cbo_Regcli.ItemsSource = LstCodeRegroupement_User;
                        this.Cbo_Regcli.SelectedItem = LstCodeRegroupement_User.First();
                        this.Cbo_Regcli.Tag = LstCodeRegroupement_User;
                    }
                    this.btn_Regroupement.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void btn_Regroupement_Click(object sender, RoutedEventArgs e)
        {
            if (LstCodeRegroupement_User != null && LstCodeRegroupement_User.Count != 0)
            {
                 Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                _LstColonneAffich.Add("CODE", "CODE");
                _LstColonneAffich.Add("NOM", "REGROUPEMENT");

                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstCodeRegroupement_User);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, true, "Regroupement");
                ctrl.Closed += new EventHandler(galatee_OkClicked);
                ctrl.Show();
            }
        }
        List<ServiceRecouvrement.CsRegCli> lstRegrSelect = new List<ServiceRecouvrement.CsRegCli>();

        void galatee_OkClicked(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                lstRegrSelect.Clear();
                foreach (var p in ctrs.MyObjectList)
                {
                    lstRegrSelect.Add((ServiceRecouvrement.CsRegCli)p);
                }

                this.Cbo_Regcli.ItemsSource = null;
                this.Cbo_Regcli.DisplayMemberPath = "NOM";
                this.Cbo_Regcli.ItemsSource = lstRegrSelect;
                if (lstRegrSelect.Count != 0)
                    this.Cbo_Regcli.SelectedItem = lstRegrSelect.First();

                this.Cbo_Regcli.Tag = lstRegrSelect;
            }
            else
                this.Cbo_Regcli.IsEnabled = true;
        }
    }
}


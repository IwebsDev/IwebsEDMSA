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
            this.Txt_MontantExigible.IsReadOnly = true;
            this.Txt_Ordre.MaxLength = SessionObject.Enumere.TailleOrdre;
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
            RetourneParametreGC();
            //this.chk_ParAbonnement.Visibility = System.Windows.Visibility.Collapsed;
        }
        bool IsPreavis = false;
        public FrmAvisDeCoupureSGC(string IsPreavie)
        {
            InitializeComponent();
            InitialiseCtrl();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_MontantExigible.IsReadOnly = true;
            IsPreavis = IsPreavie == "OUI" ? true : false;
            this.Txt_Ordre.MaxLength = SessionObject.Enumere.TailleOrdre;
            this.Txt_Client .MaxLength = SessionObject.Enumere.TailleClient ;
            RetourneParametreGC();
            //this.chk_ParAbonnement.Visibility = System.Windows.Visibility.Collapsed;
        }

        List<ServiceRecouvrement.CsAffectationGestionnaire> LstAffectation = new List<ServiceRecouvrement.CsAffectationGestionnaire>();
        List<ServiceRecouvrement.CsRegCli> LstCodeRegroupement = new List<ServiceRecouvrement.CsRegCli>();
        List<ServiceRecouvrement.CsRegCli> LstCodeRegroupement_User = new List<ServiceRecouvrement.CsRegCli>();

        List<CsLclient> ligne = new List<CsLclient>();
        private void   OKButton_Click(object sender, RoutedEventArgs e)
        {
            string IdCoupure = string.Empty;
             try
             {
                 CsAvisCoupureEdition avisCoupure = new CsAvisCoupureEdition();
               bntPrinting.IsEnabled = false;
               aCampagne Campagne = new aCampagne();
               aDisconnection dis = new aDisconnection();

               avisCoupure.Matricule = UserConnecte.matricule;
               avisCoupure.AgentPia = UserConnecte.matricule;
               avisCoupure.referenceClientDebut = string.IsNullOrEmpty(this.Txt_Client.Text) ? string.Empty : this.Txt_Client.Text;
               avisCoupure.OrdreTourneDebut = string.IsNullOrEmpty(this.Txt_Ordre.Text) ? string.Empty : this.Txt_Ordre.Text;
               string DueDate = (string.IsNullOrEmpty(this.dtpDate.Text)) ? null : dtpDate.Text;
               if (this.Cbo_Regcli.Tag != null)
                  avisCoupure.ListeRegroupement =(List<ServiceRecouvrement.CsRegCli>)this.Cbo_Regcli.Tag  ;

               avisCoupure.MontantRelancable = string.IsNullOrEmpty(this.Txt_MontantExigible.Text) ? 0 : Convert.ToDecimal(this.Txt_MontantExigible.Text);
               avisCoupure.Exigible = this.dtpDate.SelectedDate;
               if (this.Txt_LibelleCentre.Tag != null)
               {
                   List<CsCentre> lstCentreCampage = new List<CsCentre>();
                   List<string> lstCodeCentre = (List<string>)this.Txt_LibelleCentre.Tag;
                   foreach (var item in lstCodeCentre)
                       lstCentreCampage.Add(new CsCentre { CODE = item  });
                   avisCoupure.Centre_Campagne = lstCentreCampage;
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
                if (this.chk_ParAbonnement.IsChecked == true) avisCoupure.IsParAbonnement = true;
                else avisCoupure.IsParAbonnement = false;

                if (chk_ResilierExclus.IsChecked == true)
                    avisCoupure.ClientResilie = true;
                 else
                    avisCoupure.ClientResilie = false;

                if (!string.IsNullOrEmpty(this.Txt_DateDebut.Text) && ClasseMEthodeGenerique.IsFormatPeriodeValide (this.Txt_DateDebut.Text))
                    avisCoupure.PeriodeDebut = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_DateDebut.Text);


                if (!string.IsNullOrEmpty(this.Txt_DateFin.Text) && ClasseMEthodeGenerique.IsFormatPeriodeValide(this.Txt_DateFin.Text))
                    avisCoupure.PeriodeFin  = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_DateFin.Text);  

                ligne.Clear();
                prgBar.Visibility = System.Windows.Visibility.Visible ;
                LayoutRoot.Cursor = Cursors.Wait;
                RecouvrementServiceClient proxy = new RecouvrementServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Recouvrement"));
                proxy.TraitementAvisCoupureGCAsync(avisCoupure, dis, IsPreavis);
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
                            string key = Utility.getKey();
                            Dictionary<string, string> param = new Dictionary<string, string>();
                            if (param.Count != 0) param.Clear();
                            if (this.chk_ParAbonnement.IsChecked == true)
                            {
                                if (IsPreavis)
                                {
                                    param.Add("PpChefService", leParam != null && !string.IsNullOrEmpty(leParam.NOMCHEFSERVICE) ? leParam.NOMCHEFSERVICE : string.Empty);
                                    Utility.ActionExportation<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(ligne, param, string.Empty, SessionObject.CheminImpression, "PreavisDeCoupurParAbon", "Recouvrement", true, "doc");
                                }
                                else
                                {

                                    param.Add("PpNomDonneurOrdre",leParam != null && !string.IsNullOrEmpty(leParam.NOMCHEFSERVICE )? leParam.NOMCHEFSERVICE : string.Empty );
                                    param.Add("PpTitreDonneurOrdre",leParam != null && !string.IsNullOrEmpty(leParam.TITRE_DONNEURORDRE )? leParam.TITRE_DONNEURORDRE:string.Empty  );
                                    param.Add("PpContactDonneurOrdre",leParam != null && !string.IsNullOrEmpty(leParam.CONTACT_DONNEURORDRE )? leParam.CONTACT_DONNEURORDRE :string.Empty  );
                                    param.Add("PpStructureExecution",leParam != null && !string.IsNullOrEmpty(leParam.STRUCTURE_EXECUTION )? leParam.STRUCTURE_EXECUTION :string.Empty  );
                                    param.Add("PpNomAgentExecution",leParam != null && !string.IsNullOrEmpty(leParam.AGENT_EXECUTION )? leParam.AGENT_EXECUTION :string.Empty );
                                    param.Add("PpMatriculeAgent", leParam != null && !string.IsNullOrEmpty(leParam.MATRICULE_EXECUTION) ? leParam.MATRICULE_EXECUTION : string.Empty);

                                    Utility.ActionExportation<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(ligne, param, string.Empty, SessionObject.CheminImpression, "OrdreDeCoupurParAbon", "Recouvrement", true, "doc");
                                }
                            }
                            else
                            {
                                if (IsPreavis)
                                {
                                    param.Add("PpChefService", leParam != null && !string.IsNullOrEmpty(leParam.NOMCHEFSERVICE) ? leParam.NOMCHEFSERVICE : string.Empty);
                                    Utility.ActionExportation<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(ligne, param, string.Empty, SessionObject.CheminImpression, "PreavisDeCoupur", "Recouvrement", true, "doc");
                                }
                                else
                                {
                                    param.Add("PpNomDonneurOrdre", leParam != null && !string.IsNullOrEmpty(leParam.NOMCHEFSERVICE) ? leParam.NOMCHEFSERVICE : string.Empty);
                                    param.Add("PpTitreDonneurOrdre", leParam != null && !string.IsNullOrEmpty(leParam.TITRE_DONNEURORDRE) ? leParam.TITRE_DONNEURORDRE : string.Empty);
                                    param.Add("PpContactDonneurOrdre", leParam != null && !string.IsNullOrEmpty(leParam.CONTACT_DONNEURORDRE) ? leParam.CONTACT_DONNEURORDRE : string.Empty);
                                    param.Add("PpStructureExecution", leParam != null && !string.IsNullOrEmpty(leParam.STRUCTURE_EXECUTION) ? leParam.STRUCTURE_EXECUTION : string.Empty);
                                    param.Add("PpNomAgentExecution", leParam != null && !string.IsNullOrEmpty(leParam.AGENT_EXECUTION) ? leParam.AGENT_EXECUTION : string.Empty);
                                    param.Add("PpMatriculeAgent", leParam != null && !string.IsNullOrEmpty(leParam.MATRICULE_EXECUTION) ? leParam.MATRICULE_EXECUTION : string.Empty);

                                    Utility.ActionExportation<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(ligne, param, string.Empty, SessionObject.CheminImpression, "OrdreDeCoupur", "Recouvrement", true, "doc");
                                }

                            }
                                    

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
                    return;
                }
                //Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new AccesServiceWCF().GetAcceuilClient();
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
        private void Rdb_facturePeriode_Checked_1(object sender, RoutedEventArgs e)
        {
            this.Txt_MontantExigible.IsReadOnly = false;
        }

        private void Rdb_facturePeriode_Unchecked_1(object sender, RoutedEventArgs e)
        {
            this.Txt_MontantExigible.IsReadOnly = true;
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
        Galatee.Silverlight.ServiceParametrage.CsParametreCoupureSGC leParam = new ServiceParametrage.CsParametreCoupureSGC();
        private void RetourneParametreGC()
        {
            try
            {
                Galatee.Silverlight.ServiceParametrage.ParametrageClient client = new Galatee.Silverlight.ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllParamatreSCGCCompleted += (ssender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError(error, "Recouvrement");
                            return;
                        }
                         if (args.Result != null && args.Result.Count != 0)
                            leParam = args.Result.FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, "Recouvrement");
                    }
                };
                client.SelectAllParamatreSCGCAsync();
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
                    var ListIdRegcliCorrespondant = Affectation.Select(a => a.FK_IDREGROUPEMENT);
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
                    ServiceRecouvrement.CsRegCli leRegroupement = (ServiceRecouvrement.CsRegCli)p;
                    leRegroupement.CODPOS ="(" + leRegroupement.CODE + ")  " + leRegroupement.NOM;
                    lstRegrSelect.Add(leRegroupement);
                }

                this.Cbo_Regcli.ItemsSource = null;
                this.Cbo_Regcli.DisplayMemberPath = "CODPOS";
                this.Cbo_Regcli.ItemsSource = lstRegrSelect;
                if (lstRegrSelect.Count != 0)
                    this.Cbo_Regcli.SelectedItem = lstRegrSelect.First();

                this.Cbo_Regcli.Tag = lstRegrSelect;
            }
            else
                this.Cbo_Regcli.IsEnabled = true;
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ServiceAccueil.CsCentre> lstCentreDistinct = new List<ServiceAccueil.CsCentre>();
                if (SessionObject.LstCentre.Count != 0)
                {
                    var lesDistinct = SessionObject.LstCentre.Where(ip => ip.CODE != "001" && ip.CODE != "002"
                                                                     && ip.CODE != "003" && ip.CODE != "004").
                                                                     Select(u => new { u.CODE}).Distinct();
                    foreach (var item in lesDistinct)
                    {
                        ServiceAccueil.CsCentre leCentr = new ServiceAccueil.CsCentre();
                        leCentr.CODE = item.CODE;
                        lstCentreDistinct.Add(leCentr);
                    }
                }
                if (lstCentreDistinct != null && lstCentreDistinct.Count != 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<Galatee.Silverlight.ServiceFacturation.CsLotri> leLotSelect = new List<Galatee.Silverlight.ServiceFacturation.CsLotri>();
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "CODE");
                    foreach (ServiceAccueil.CsCentre item in lstCentreDistinct)
                    {
                        leLotSelect.Add(new ServiceFacturation.CsLotri()
                        {
                            CODE = item.CODE,
                        });
                    }
                    Galatee.Silverlight.Facturation.UcGenerique ctrl = new Galatee.Silverlight.Facturation.UcGenerique(leLotSelect, true, "Liste des centres");
                    ctrl.Closed += new EventHandler(ucgCentre);
                    ctrl.Show();
                    this.btn_Centre.IsEnabled = true;
                }


            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void ucgCentre(object sender, EventArgs e)
        {

            Galatee.Silverlight.Facturation.UcGenerique ctrs = sender as Galatee.Silverlight.Facturation.UcGenerique;
            if (ctrs.isOkClick)
            {
                List<Galatee.Silverlight.ServiceFacturation.CsLotri> LesCentreeDuLot = (List<Galatee.Silverlight.ServiceFacturation.CsLotri>)ctrs.MyObjectList;
                if (LesCentreeDuLot != null && LesCentreeDuLot.Count > 0)
                {
                    int passage = 1;
                    foreach (Galatee.Silverlight.ServiceFacturation.CsLotri item in LesCentreeDuLot)
                    {
                        if (passage == 1)
                            this.Txt_LibelleCentre.Text = item.CODE;
                        else
                            this.Txt_LibelleCentre.Text = this.Txt_LibelleCentre.Text + "  " + item.CODE;
                        passage++;

                    }
                    this.Txt_LibelleCentre.Tag = LesCentreeDuLot.Select(o => o.CODE).ToList();
                }
            }
        }
    }
}


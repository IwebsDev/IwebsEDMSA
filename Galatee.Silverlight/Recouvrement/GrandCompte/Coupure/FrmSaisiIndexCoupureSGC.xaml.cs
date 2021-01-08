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
    public partial class FrmSaisiIndexCoupureSGC : ChildWindow
    {
        public FrmSaisiIndexCoupureSGC()
        {
            InitializeComponent();
            InitialiseCtrl();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;

        }


        List<ServiceRecouvrement.CsAffectationGestionnaire> LstAffectation = new List<ServiceRecouvrement.CsAffectationGestionnaire>();
        List<ServiceRecouvrement.CsRegCli> LstCodeRegroupement = new List<ServiceRecouvrement.CsRegCli>();
        List<ServiceRecouvrement.CsRegCli> LstCodeRegroupement_User = new List<ServiceRecouvrement.CsRegCli>();

        List<CsLclient> ligne = new List<CsLclient>();
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string IdCoupure = string.Empty;
            try
            {
            }
            catch (Exception ex)
            {
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
            ChargerCentre();
            RemplirCodeRegroupement();
            RemplirAffectation();
            ChargeTypeCoupure();
            ChargeObservation();
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
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "LoadCentre");

            }
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
                    var ListIdRegcliCorrespondant = Affectation.Select(a => a.FK_IDREGROUPEMENT);
                    LstCodeRegroupement_User = LstCodeRegroupement.Where(r => ListIdRegcliCorrespondant.Contains(r.PK_ID)).ToList();
                    if (LstCodeRegroupement_User.Count == 1)
                    {
                        this.Cbo_Regcli.ItemsSource = null;
                        this.Cbo_Regcli.DisplayMemberPath = "NOM";
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

        private void Cbo_Regcli_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Cbo_Regcli.SelectedItem != null)
                ChargerCampagne(((CsRegCli)this.Cbo_Regcli.SelectedItem).PK_ID); 
        }
        void ChargerCampagne(int Regroupement)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.ChargerCampagneCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                List<CsCampagneGc> lstCampagne = args.Result;
                this.Cbo_Campagne.ItemsSource = null;
                this.Cbo_Campagne.DisplayMemberPath  = "NUMEROCAMPAGNE" ;
                this.Cbo_Campagne.ItemsSource = lstCampagne;


                return;
            };
            service.ChargerCampagneAsync(Regroupement);
        }
        List<CsObservation> lstObservation = new List<CsObservation>();
        private void ChargeObservation()
        {
            try
            {
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                client.RetourneObservationAsync();
                client.RetourneObservationCompleted += (es, result) =>
                {
                    try
                    {
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
                        lstObservation = result.Result;
                        this.cbo_Observation.ItemsSource = null;
                        this.cbo_Observation.ItemsSource = lstObservation;
                        this.cbo_Observation.DisplayMemberPath = "LIBELLE";
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<Galatee.Silverlight.ServiceAccueil.CsTypeCoupure> lstTypeCoupure = new List<Galatee.Silverlight.ServiceAccueil.CsTypeCoupure>();
        private void ChargeTypeCoupure()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.RetourneTypeCoupureAsync();
                client.RetourneTypeCoupureCompleted += (es, result) =>
                {
                    try
                    {
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
                        lstTypeCoupure = result.Result;
                        this.cbo_TypeCoupure.ItemsSource = null;
                        this.cbo_TypeCoupure.ItemsSource = lstTypeCoupure;
                        this.cbo_TypeCoupure.DisplayMemberPath = "LIBELLE";
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ChargerIndexCampagne(int Regroupement)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.ChargerClientPourSaisiIndexCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                List<CsDetailCampagne > lstCampagne = args.Result;
                lvwResultat.ItemsSource = null ;
                lvwResultat.ItemsSource = lstCampagne;
                return;
            };
            service.ChargerClientPourSaisiIndexAsync(Regroupement);
        }
        private void Cbo_Campagne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Cbo_Campagne.SelectedItem != null )
                ChargerIndexCampagne(((CsCampagneGc)this.Cbo_Campagne.SelectedItem).PK_ID );
        }

        private void txt_index_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbo_TypeCoupure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbo_Observation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void btn_Valider_Click(object sender, RoutedEventArgs e)
        {
            if (lvwResultat.SelectedItem != null &&
                this.cbo_TypeCoupure.SelectedItem != null &&
                !string.IsNullOrEmpty(this.txt_DateReleve.Text))
            {
                CsDetailCampagne  LeEvtSelect = (CsDetailCampagne )this.lvwResultat.SelectedItem;
                if (LeEvtSelect.FRAIS != null && LeEvtSelect.FRAIS != 0 &&
                    LeEvtSelect.FRAIS != ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).COUT)
                {
                    string Message = "Les frais de " + LeEvtSelect.FRAIS + " ont déja été injectés dans le compte de ce client" + "\n\r" +
                                                                          "Voulez vous le remplacer par " + ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).COUT;

                    var ws = new MessageBoxControl.MessageBoxChildWindow("Index ", Message, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                    ws.OnMessageBoxClosed += (l, results) =>
                    {
                        if (ws.Result == MessageBoxResult.OK)
                        {
                            LeEvtSelect.INDEX = string.IsNullOrEmpty(this.txt_index.Text) ? 0 : int.Parse(this.txt_index.Text);
                            LeEvtSelect.DATECOUPURE = Convert.ToDateTime(this.txt_DateReleve.Text);
                            LeEvtSelect.DATERDVCLIENT = this.txt_DateReleve.Text;
                            LeEvtSelect.TYPECOUPURE = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).CODE;
                            LeEvtSelect.FRAIS = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).COUT;
                            LeEvtSelect.FK_TYPECOUPURE = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).PK_ID;
                            LeEvtSelect.MONTANTFRAIS = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).COUT;
                            LeEvtSelect.MATRICULE = UserConnecte.matricule;
                            LeEvtSelect.FK_IDOBSERVATION = null;
                            if (this.cbo_Observation.SelectedItem != null)
                                LeEvtSelect.FK_IDOBSERVATION = ((CsObservation)this.cbo_Observation.SelectedItem).PK_ID;

                            LeEvtSelect.FK_IDCOPER = SessionObject.LstDesCopers.First(t => t.CODE == SessionObject.Enumere.CoperFRP).PK_ID;
                            LeEvtSelect.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                            LeEvtSelect.FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.First(t => t.CODE == SessionObject.Enumere.TopMoratoire).PK_ID;
                            LeEvtSelect.ISNONENCAISSABLE = true;
                            InsertionIndexCoupure(LeEvtSelect);
                            return;
                        }
                        else
                            return;
                    };
                    ws.Show();
                }
                LeEvtSelect.INDEX = string.IsNullOrEmpty(this.txt_index.Text) ? 0 : int.Parse(this.txt_index.Text);
                LeEvtSelect.DATECOUPURE = Convert.ToDateTime(this.txt_DateReleve.Text);
                LeEvtSelect.DATERDVCLIENT = this.txt_DateReleve.Text;
                LeEvtSelect.TYPECOUPURE = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).CODE;
                LeEvtSelect.FRAIS = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).COUT;
                LeEvtSelect.FK_TYPECOUPURE = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).PK_ID;
                LeEvtSelect.MONTANTFRAIS = ((Galatee.Silverlight.ServiceAccueil.CsTypeCoupure)cbo_TypeCoupure.SelectedItem).COUT;
                LeEvtSelect.MATRICULE = UserConnecte.matricule;
                LeEvtSelect.FK_IDOBSERVATION = null;
                if (this.cbo_Observation.SelectedItem != null)
                    LeEvtSelect.FK_IDOBSERVATION = ((CsObservation)this.cbo_Observation.SelectedItem).PK_ID;

                LeEvtSelect.FK_IDCOPER = SessionObject.LstDesCopers.First(t => t.CODE == SessionObject.Enumere.CoperFRP).PK_ID;
                LeEvtSelect.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                LeEvtSelect.FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.First(t => t.CODE == SessionObject.Enumere.TopMoratoire).PK_ID;
                LeEvtSelect.FK_IDCAMPAGNE = ((CsCampagneGc)this.Cbo_Campagne.SelectedItem).PK_ID; 
                LeEvtSelect.ISNONENCAISSABLE = true;
                InsertionIndexCoupure(LeEvtSelect);
            }
            else
            {
                if (this.cbo_TypeCoupure.SelectedItem == null)
                    Message.ShowInformation("Saisir le type de coupure", "Recouvrement");
                if (string.IsNullOrEmpty(this.txt_DateReleve.Text))
                    Message.ShowInformation("Saisir la date de relève", "Recouvrement");

                return;
            }


        }
        private void InsertionIndexCoupure(CsDetailCampagne Lst)
        {
            try
            {
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("Recouvrement"));
                client.InsertIndexSGCCompleted += (ss, ress) =>
                {
                    try
                    {
                        if (ress.Cancelled || ress.Error != null)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel service", "Informations");
                            return;
                        }

                        if (ress.Result == null)
                        {
                            Message.ShowInformation("Erreur lors de l'insertion des index de campange! Veuillez réessayer svp ", "Informations");
                            return;
                        }
                        this.lvwResultat.ItemsSource = null;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                client.InsertIndexSGCAsync(Lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


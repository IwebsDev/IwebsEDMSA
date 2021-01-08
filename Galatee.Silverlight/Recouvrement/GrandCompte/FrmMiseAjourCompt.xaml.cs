using Galatee.Silverlight.ServiceRecouvrement;
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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmMiseAjourCompt : ChildWindow
    {    
        #region Variables

        List<CsCampagneGc> ListCapagneGc = new List<CsCampagneGc>();
        CsCampagneGc LaCampagneGc = new CsCampagneGc();

        public bool CanShowWindows = true;
        List<CsDetailCampagneGc> DetailCampagneValider = new List<CsDetailCampagneGc>();
        List<CsDetailCampagneGc> DetailCampagneNonValider = new List<CsDetailCampagneGc>();
        List<CsPaiementGc> ListMandatementGc = new List<CsPaiementGc>();
        int IdCampagne = 0;
        List<int> ListIdCampagne = new List<int>();
        int EtapeActuel = 0;
        #endregion

        #region Constructeur

        public FrmMiseAjourCompt()
        {
            InitializeComponent();
            RemplirCampagne(UserConnecte.matricule);
        }

        public FrmMiseAjourCompt(int IdCampagne)
        {
            InitializeComponent();
            this.IdCampagne=IdCampagne;

            RemplirCampagne(UserConnecte.matricule);

        }

        public FrmMiseAjourCompt(List<int> ListIdCampagne, int EtapeActuel)
        {
            InitializeComponent();
            if (ListIdCampagne != null && ListIdCampagne.Count != 0)
            {
                this.IdCampagne = ListIdCampagne[0];
                this.EtapeActuel = EtapeActuel;
                RemplirCampagne(UserConnecte.matricule);
            }

        }
        #endregion

        #region Event Handler

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            OKButton.IsEnabled = false;
            List<CsDetailPaiementGc> lstDetailPaiement = new List<CsDetailPaiementGc>();
            lstDetailPaiement = (List<CsDetailPaiementGc>)this.dg_facture.ItemsSource;
            MiseAjourCompt(lstDetailPaiement, this.IdCampagne);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrmFactureHorRegroupement frm = new FrmFactureHorRegroupement();
            frm.CallBack += frm_CallBack1;
            frm.Show();
        }
        void frm_CallBack1(object sender, Tarification.Helper.CustumEventArgs e)
        {
            //Implementer le callback
            if (e.Bag != null)
            {
                var ListFacture = (List<CsLclient>)e.Bag;
                List<CsDetailCampagneGc> datasource = (List<CsDetailCampagneGc>)dg_facture.ItemsSource;
                if (datasource == null)
                {
                    datasource = new List<CsDetailCampagneGc>();
                }
                foreach (var item in ListFacture)
                {
                    CsDetailCampagneGc facture = new CsDetailCampagneGc();
                    facture.CENTRE = item.CENTRE;
                    facture.CLIENT = item.CLIENT;
                    facture.ORDRE = item.ORDRE;
                    facture.NOM = item.NOM;
                    facture.PERIODE = item.REFEM;
                    facture.MONTANT = item.MONTANT;
                    facture.NDOC = item.NDOC;
                    datasource.Add(facture);
                }
                dg_facture.ItemsSource = datasource.OrderBy(d => d.NOM).ToList();
            }
        }

        private void dg_facture_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }
        private void dg_facture_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            var DetailCampagne = (CsDetailCampagneGc)dg_facture.SelectedItem;
            if (DetailCampagne != null)
            {
                if (DetailCampagne.MONTANT_VERSER > DetailCampagne.MONTANT_RESTANT)
                {
                    Message.ShowWarning("Le montant du mandatement doit être inférieur ou egale au montant restant à payer", "Avertissement");

                    var DataSource = (List<CsDetailCampagneGc>)dg_facture.ItemsSource;
                    int index = DataSource.IndexOf(DetailCampagne);
                    DetailCampagne.MONTANT_VERSER = 0;
                    DataSource[index] = DetailCampagne;

                    dg_facture.ItemsSource = DataSource.OrderBy(d => d.NOM).ToList();
                }
                else
                {
                    CsMandatementGc CampAnSelectioner = new CsMandatementGc();
                    CampAnSelectioner = (CsMandatementGc)dg_Campagne.SelectedItem;

                    var datasource = (List<CsDetailMandatementGc>)dg_facture.ItemsSource;
                    txt_Montant_Mandatement.Text = datasource.Sum(dm => dm.MONTANT_VERSER).ToString();

                    var MandatementGc = ListMandatementGc.FirstOrDefault(m => m.FK_IDMANDATEMANT == CampAnSelectioner.PK_ID);
                    if (MandatementGc == null)
                    {
                        var Mandatement = new CsPaiementGc { FK_IDMANDATEMANT = CampAnSelectioner.PK_ID, MONTANT = datasource.Sum(dm => dm.MONTANT_VERSER), DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule, NUMEROMANDATEMENT = txt_Numdeataire.Text };
                        List<CsDetailPaiementGc> DETAILMANDATEMENTGC_ = new List<CsDetailPaiementGc>();
                        foreach (var item in datasource)
                        {
                            if (item.MONTANT_VERSER != null && item.MONTANT_VERSER > 0)
                            {
                                CsDetailPaiementGc DetailMandatement = new CsDetailPaiementGc();

                                DetailMandatement.CENTRE = item.CENTRE;
                                DetailMandatement.CLIENT = item.CLIENT;
                                DetailMandatement.ORDRE = item.ORDRE;
                                DetailMandatement.DATECREATION = DateTime.Now;
                                DetailMandatement.DATEMODIFICATION = DateTime.Now;
                                DetailMandatement.FK_IDPAIEMENTCAMPAGNEGC = Mandatement.PK_ID;
                                DetailMandatement.MONTANT = item.MONTANT_VERSER;
                                DetailMandatement.NDOC = item.NDOC;
                                DetailMandatement.PERIODE = item.PERIODE;
                                DetailMandatement.STATUS = item.STATUS;
                                DetailMandatement.USERCREATION = UserConnecte.matricule;
                                DetailMandatement.USERMODIFICATION = UserConnecte.matricule;

                                DETAILMANDATEMENTGC_.Add(DetailMandatement);

                            }

                        }
                        Mandatement.DETAILCAMPAGNEGC_ = DETAILMANDATEMENTGC_;
                        ListMandatementGc.Add(Mandatement);
                    }
                    else
                    {

                        MandatementGc.MONTANT = datasource.Sum(dm => dm.MONTANT_VERSER);
                        MandatementGc.NUMEROMANDATEMENT = txt_Numdeataire.Text;
                        List<CsDetailPaiementGc> DETAILMANDATEMENTGC_ = new List<CsDetailPaiementGc>();
                        foreach (var item in datasource)
                        {
                            if (item.MONTANT_VERSER != null && item.MONTANT_VERSER > 0)
                            {
                                CsDetailPaiementGc DetailMandatement = new CsDetailPaiementGc();

                                DetailMandatement.CENTRE = item.CENTRE;
                                DetailMandatement.CLIENT = item.CLIENT;
                                DetailMandatement.ORDRE = item.ORDRE;
                                DetailMandatement.DATECREATION = DateTime.Now;
                                DetailMandatement.DATEMODIFICATION = DateTime.Now;
                                DetailMandatement.FK_IDPAIEMENTCAMPAGNEGC = MandatementGc.PK_ID;
                                DetailMandatement.MONTANT = item.MONTANT_VERSER;
                                DetailMandatement.NDOC = item.NDOC;
                                DetailMandatement.PERIODE = item.PERIODE;
                                DetailMandatement.STATUS = item.STATUS;
                                DetailMandatement.USERCREATION = UserConnecte.matricule;
                                DetailMandatement.USERMODIFICATION = UserConnecte.matricule;

                                DETAILMANDATEMENTGC_.Add(DetailMandatement);

                            }

                        }
                        MandatementGc.DETAILCAMPAGNEGC_ = DETAILMANDATEMENTGC_;

                        var mand = ListMandatementGc.FirstOrDefault(m => m.PK_ID == MandatementGc.PK_ID);
                        var index = ListMandatementGc.IndexOf(mand);
                        ListMandatementGc[index] = MandatementGc;
                    }
                }
            }
        }

        private void txt_MontantMandatement_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal MontaMandatement = 0;
            if (decimal.TryParse(txt_MontantMandatement.Text, out MontaMandatement))
            {
                if (MontaMandatement == ListCapagneGc[0].MONTANT)
                {
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow("Information", "Voulez vous lancer la validation automatique de toute les factures", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            chbx_ToutValider.IsChecked = true;
                        }
                        else
                        {
                            return;
                        }
                    };
                    messageBox.Show();
                }
            }
        }

        private void chbx_ToutValider_Checked(object sender, RoutedEventArgs e)
        {
            List<CsDetailCampagneGc> DetailCampagneGc = (List<CsDetailCampagneGc>)dg_facture.ItemsSource;

            for (int index = 0; index < DetailCampagneGc.Count; index++)
            {
                DetailCampagneGc[index].IsMontantValide = true;
                DetailCampagneGc[index].MONTANT_VERSER = DetailCampagneGc[index].MONTANT_RESTANT;
            }
            dg_facture.ItemsSource = DetailCampagneGc.OrderBy(d => d.NOM).ToList();
        }
        private void chbx_ToutValider_Unchecked(object sender, RoutedEventArgs e)
        {
            List<CsDetailCampagneGc> DetailCampagneGc = (List<CsDetailCampagneGc>)dg_facture.ItemsSource;
            foreach (var item in DetailCampagneGc)
            {
                item.IsMontantValide = false;
            }
            dg_facture.ItemsSource = DetailCampagneGc.OrderBy(d => d.NOM).ToList();
        }

        private void CheckBox_Unchecked_(object sender, RoutedEventArgs e)
        {
            var DetailCampagneSelectionner = (CsDetailCampagneGc)dg_facture.SelectedItem;
            if (DetailCampagneSelectionner != null)
            {
                DetailCampagneSelectionner.IsMontantNonValide = false;
                DetailCampagneNonValider.Remove(DetailCampagneSelectionner);
            }
        }
        private void CheckBox_Checked_(object sender, RoutedEventArgs e)
        {
            var DetailCampagneSelectionner = (CsDetailCampagneGc)dg_facture.SelectedItem;

            if (DetailCampagneSelectionner != null)
                CanShowWindows = true;
        }

        void Frm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MiseAJourMandatement(true);
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MiseAJourMandatement(false);
        }
        private void dg_Paiement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn_Actualiser_Click(object sender, RoutedEventArgs e)
        {
       
            dg_Paiement.SelectedItem = null;
            if (dg_Campagne.SelectedItem != null)
            {
                var mand = ((CsMandatementGc)dg_Campagne.SelectedItem);
                List<string> LstfacturMand = new List<string>();
                //LstfacturMand = mand.DETAILMANDATEMENTGC_;
                foreach (var FACTUREMANDATEMENT in mand.DETAILMANDATEMENTGC_)
                {
                    foreach (var PAIEMENTGC in mand.PAIEMENTGC_)
                    {
                        foreach (var DETAILPAIEMENT in PAIEMENTGC.DETAILCAMPAGNEGC_)
                        {
                            if (FACTUREMANDATEMENT.NDOC == DETAILPAIEMENT.NDOC)
                            {
                                LstfacturMand.Add(DETAILPAIEMENT.NDOC);
                            }
                        }
                    }
                }
                if (mand.DETAILMANDATEMENTGC_.Where(m => !LstfacturMand.Contains(m.NDOC)) != null && mand.DETAILMANDATEMENTGC_.Where(m => !LstfacturMand.Contains(m.NDOC)).Count() > 0)
                {
                    dg_facture.ItemsSource = mand.DETAILMANDATEMENTGC_.Where(m => !LstfacturMand.Contains(m.NDOC)).ToList();
                    if (mand.DETAILMANDATEMENTGC_.Where(m => !LstfacturMand.Contains(m.NDOC)) != null && mand.DETAILMANDATEMENTGC_.Where(m => !LstfacturMand.Contains(m.NDOC)).ToList().Count != 0)
                    dg_facture.SelectedItem  =mand.DETAILMANDATEMENTGC_.Where(m => !LstfacturMand.Contains(m.NDOC)).ToList().First();
 
                }
                else
                {
                    dg_facture.ItemsSource = new List<CsDetailMandatementGc>();
                }


            }
        }

        private void btn_trasmettre_Click(object sender, RoutedEventArgs e)
        {
            List<CsPaiementGc> LisPaieemntToSaveGc = new List<CsPaiementGc>();

            foreach (var item in ListMandatementGc)
            {
                if (item.DETAILCAMPAGNEGC_.Count > 0)
                {
                    LisPaieemntToSaveGc.Add(item);
                }
            }
            if (LisPaieemntToSaveGc.Count > 0)
            {
                SaveMandatement(LisPaieemntToSaveGc, true);
            }
            else
            {
                TransmettreCampagne();
            }
        }

        private void TransmettreCampagne()
        {
            var datasourcepaiement = (List<CsPaiementGc>)dg_Paiement.ItemsSource;
            decimal? montantpaiement = 0;
            datasourcepaiement.ForEach(p => montantpaiement += p.DETAILCAMPAGNEGC_.Sum(d => d.MONTANT));

            var datasourcemandatement = (List<CsMandatementGc>)dg_Campagne.ItemsSource;
            decimal? montantmandatement = 0;
            datasourcemandatement.ForEach(p => montantmandatement += p.DETAILMANDATEMENTGC_.Sum(d => d.MONTANT));
            if (montantpaiement == montantmandatement)
            {
                //Transmission a l'étape suivante
                List<int> lstId = new List<int>();
                lstId.Add(this.IdCampagne);
                EnvoyerDemandeEtapeSuivante(lstId);
            }
            else
            {
                Message.ShowWarning("La campagne ne peut pas etre transmise , paiement imcomplé", "Information");
            }
        }


        private void EnvoyerDemandeEtapeSuivante(List<int> Listid)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
            clientWkf.ExecuterActionSurDemandeParPkIDLigneCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec de sortie materiel", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    return;
                }
                if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
                {
                    Message.ShowError("Echec de sortie materiel", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    return;
                }
                else
                {
                    //Message.ShowInformation("Sortie materiel effectuée", Langue.lbl_Menu);

                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, this.EtapeActuel, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }


        private void dg_facture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void dg_facture_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dmdRow = e.Row.DataContext as CsDetailPaiementGc;

            var CampAnSelectioner = (CsMandatementGc)dg_Campagne.SelectedItem;
            var factureMand = CampAnSelectioner.DETAILMANDATEMENTGC_.Select(dm => dm.NDOC);
            if (!factureMand.Contains(dmdRow.NDOC))
            {
                SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Red);
                e.Row.Foreground = SolidColorBrush;
            }
        }
        #endregion

        #region  Service

        private void RemplirCampagne(string Matricule, bool ATransmettre = false)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RemplirCampagneByIdCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                    return;
                LaCampagneGc = args.Result;

                List<CsMandatementGc> Datasource = new List<CsMandatementGc>();

                List<CsDetailMandatementGc> ListeDetailCampagneAsuprimmer = new List<CsDetailMandatementGc>();
                //foreach (var item in ListCapagneGc)
                //{

                    foreach (var LeMand in LaCampagneGc.MANDATEMENTGC_)
                    {

                        foreach (var item_ in LeMand.DETAILMANDATEMENTGC_)
                        {
                            item_.MONTANT_RESTANT = 0;
                            item_.MONTANT_REGLER = 0;
                            foreach (var paiement in LeMand.PAIEMENTGC_)
                            {
                                var detailmand = paiement.DETAILCAMPAGNEGC_.Where(dm => dm.CENTRE == item_.CENTRE && dm.CLIENT == item_.CLIENT && dm.ORDRE == item_.ORDRE && dm.NDOC == item_.NDOC);
                                if (detailmand != null)
                                {
                                    item_.MONTANT_REGLER = item_.MONTANT_REGLER + detailmand.Sum(c => c.MONTANT);
                                }
                            }

                            item_.MONTANT_RESTANT = item_.MONTANT - item_.MONTANT_REGLER;

                            if (item_.MONTANT_RESTANT <= 0)
                            {
                                ListeDetailCampagneAsuprimmer.Add(item_);
                            }
                            item_.MONTANT_VERSER = 0;
                        }
                    }

                    if (LaCampagneGc.PK_ID == this.IdCampagne)
                        Datasource.AddRange(LaCampagneGc.MANDATEMENTGC_);
                //}

                txt_Campagne.Text = LaCampagneGc.NUMEROCAMPAGNE != null ? LaCampagneGc.NUMEROCAMPAGNE : string.Empty;
                txt_regroupement.Text = LaCampagneGc.LIBELLEREGROUPEMENT != null ? LaCampagneGc.LIBELLEREGROUPEMENT : string.Empty;
                txt_periode.Text = LaCampagneGc.PERIODE != null ? LaCampagneGc.PERIODE : string.Empty;

                dg_Campagne.ItemsSource = Datasource;
                dg_Campagne.SelectedItem = Datasource[0];

                if (ATransmettre == true)
                {
                    TransmettreCampagne();
                }
                return;
            };
            service.RemplirCampagneByIdAsync(this.IdCampagne, "1");
        }
        private void SaveMandatement(List<CsPaiementGc> ListMandatementGc, bool ATransmettre = false)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.SavePaiementCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                    return;
                if (args.Result == true)
                {
                    Message.Show("Paiement enregistré avec succès", "Information");
                    if (ATransmettre == true)
                    {
                        RemplirCampagne(UserConnecte.matricule, true);
                    }
                }
                else
                {
                    Message.Show("Le Paiement n'a pas été enregistré avec succès,veuillez refaire l'opration ", "Information");
                }

                return;
            };
            service.SavePaiementAsync(ListMandatementGc,null);
        }
        private void MiseAjourCompt(List<CsDetailPaiementGc> lstDetailPaiement, int Id)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.MiseAjourComptCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                    return;
                if (args.Result.Count  == 0)
                    return;
                Message.Show("Mise à jour effectuée avec succès", "Resultat");
                Dictionary<string, string> param = new Dictionary<string, string>();
                var lesCompteMaj = args.Result.Select(u => new { u.CENTRE, u.CLIENT, u.ORDRE,u.NOM ,u.ACQUIT ,u.REFEM ,u.NDOC,u.NUMDEVIS,u.NUMDEM   }).Distinct();
                List<CsLclient> lesCompteAEditer = new List<CsLclient>();
                int i = 0;
                foreach (var item in lesCompteMaj)
                {
                    CsLclient leC = new CsLclient();
                    leC.CENTRE = item.CENTRE ;
                    leC.CLIENT  = item.CLIENT ;
                    leC.ORDRE  = item.ORDRE ;
                    leC.REFEM  = item.REFEM ;
                    leC.NDOC = item.NDOC;
                    leC.NOM = item.NOM;
                    leC.NUMDEM = item.NUMDEM;
                    leC.NUMDEVIS = item.NUMDEVIS;
                    leC.NUMETAPE  = i + 1;
                    leC.ADRESSE = txt_Campagne.Text;
                    leC.CAPUR = System.DateTime.Today.ToShortDateString();
                    leC.MONTANT  = args.Result.Where(t=>t.CENTRE == item.CENTRE && t.CLIENT ==item.CLIENT && t.ORDRE == item.ORDRE && t.REFEM ==item.REFEM && t.NDOC == item.NDOC  ).Sum(t=>t.MONTANT);

                    lesCompteAEditer.Add(leC);
                }
                param.Add("pUserMiseAJour", UserConnecte.nomUtilisateur);
                Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(lesCompteAEditer, param, SessionObject.CheminImpression, "MiseAJourGrandCompte", "Recouvrement", true);
                this.DialogResult = true;
                //List<int> id = new List<int>();
                //id.Add(Id);
                //EnvoyerDemandeEtapeSuivante(id);
                return;
            };
            service.MiseAjourComptAsync(lstDetailPaiement, Id);
        }
        #endregion

        #region Methodes

        private void MiseAJourMandatement(bool Isvalide)
        {
            CsMandatementGc CampAnSelectioner = new CsMandatementGc();
            CampAnSelectioner = (CsMandatementGc)dg_Campagne.SelectedItem;

            var datasource = (List<CsDetailMandatementGc>)dg_facture.ItemsSource;

            UpdateDataSource(Isvalide, datasource);

            var MandatementGc = ListMandatementGc.FirstOrDefault(m => m.FK_IDMANDATEMANT == CampAnSelectioner.PK_ID);
            if (MandatementGc == null)
            {
                var Mandatement = new CsPaiementGc { FK_IDMANDATEMANT = CampAnSelectioner.PK_ID, MONTANT = datasource.Where(f => f.IsMontantValide == true).Sum(dm => dm.MONTANT), DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule, NUMEROMANDATEMENT = txt_Numdeataire.Text };
                List<CsDetailPaiementGc> DETAILMANDATEMENTGC_ = new List<CsDetailPaiementGc>();
                foreach (var item in datasource)
                {
                    if (item.IsMontantValide == true)
                    {
                        CsDetailPaiementGc DetailMandatement = new CsDetailPaiementGc();

                        DetailMandatement.CENTRE = item.CENTRE;
                        DetailMandatement.CLIENT = item.CLIENT;
                        DetailMandatement.ORDRE = item.ORDRE;
                        DetailMandatement.DATECREATION = DateTime.Now;
                        DetailMandatement.DATEMODIFICATION = DateTime.Now;
                        DetailMandatement.FK_IDPAIEMENTCAMPAGNEGC = Mandatement.PK_ID;
                        DetailMandatement.MONTANT = item.MONTANT;
                        //DetailMandatement.MONTANT_REGLER = DetailMandatement.MONTANT;
                        //DetailMandatement.MONTANT_VERSER = DetailMandatement.MONTANT;
                        //DetailMandatement.MONTANT_RESTANT = 0;
                        DetailMandatement.NDOC = item.NDOC;
                        DetailMandatement.PERIODE = item.PERIODE;
                        DetailMandatement.STATUS = item.STATUS;
                        DetailMandatement.USERCREATION = UserConnecte.matricule;
                        DetailMandatement.USERMODIFICATION = UserConnecte.matricule;

                        DETAILMANDATEMENTGC_.Add(DetailMandatement);

                    }

                }
                Mandatement.DETAILCAMPAGNEGC_ = DETAILMANDATEMENTGC_;
                ListMandatementGc.Add(Mandatement);
            }
            else
            {

                MandatementGc.MONTANT = datasource.Where(f => f.IsMontantValide == true).Sum(dm => dm.MONTANT);
                MandatementGc.NUMEROMANDATEMENT = txt_Numdeataire.Text;
                List<CsDetailPaiementGc> DETAILMANDATEMENTGC_ = new List<CsDetailPaiementGc>();
                foreach (var item in datasource)
                {
                    if (item.IsMontantValide == true)
                    {
                        CsDetailPaiementGc DetailMandatement = new CsDetailPaiementGc();

                        DetailMandatement.CENTRE = item.CENTRE;
                        DetailMandatement.CLIENT = item.CLIENT;
                        DetailMandatement.ORDRE = item.ORDRE;
                        DetailMandatement.DATECREATION = DateTime.Now;
                        DetailMandatement.DATEMODIFICATION = DateTime.Now;
                        DetailMandatement.FK_IDPAIEMENTCAMPAGNEGC = MandatementGc.PK_ID;
                        DetailMandatement.MONTANT = item.MONTANT;
                        //DetailMandatement.MONTANT_REGLER = DetailMandatement.MONTANT;
                        //DetailMandatement.MONTANT_VERSER = DetailMandatement.MONTANT;
                        //DetailMandatement.MONTANT_RESTANT = 0;
                        DetailMandatement.NDOC = item.NDOC;
                        DetailMandatement.PERIODE = item.PERIODE;
                        DetailMandatement.STATUS = item.STATUS;
                        DetailMandatement.USERCREATION = UserConnecte.matricule;
                        DetailMandatement.USERMODIFICATION = UserConnecte.matricule;

                        DETAILMANDATEMENTGC_.Add(DetailMandatement);

                    }

                }
                MandatementGc.DETAILCAMPAGNEGC_ = DETAILMANDATEMENTGC_;

                var mand = ListMandatementGc.FirstOrDefault(m => m.PK_ID == MandatementGc.PK_ID);
                var index = ListMandatementGc.IndexOf(mand);
                ListMandatementGc[index] = MandatementGc;

            }

            txt_Montant_Mandatement.Text = datasource.Where(f => f.IsMontantValide == true).Sum(dm => dm.MONTANT).ToString();

        }

        private void UpdateDataSource(bool Isvalide, List<CsDetailMandatementGc> datasource)
        {
            var FactureSelectionne = (CsDetailMandatementGc)dg_facture.SelectedItem;

            var Index = datasource.IndexOf(FactureSelectionne);

            var FactureCorrespondante = datasource.ElementAt(Index);
            FactureCorrespondante.IsMontantValide = Isvalide;
            datasource[Index] = FactureCorrespondante;
        }

        #endregion
    }
}


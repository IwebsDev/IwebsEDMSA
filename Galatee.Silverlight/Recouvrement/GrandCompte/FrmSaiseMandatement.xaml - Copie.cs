using Galatee.Silverlight.Recouvrement.GrandCompte;
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
    public partial class FrmSaiseMandatement : ChildWindow
    {

        #region Variables

        List<CsCampagneGc> ListCapagneGc = new List<CsCampagneGc>();
        CsCampagneGc LaCampagneGc = new CsCampagneGc();

        public bool CanShowWindows = true;
        List<CsDetailCampagneGc> DetailCampagneValider = new List<CsDetailCampagneGc>();
        List<CsDetailCampagneGc> DetailCampagneNonValider = new List<CsDetailCampagneGc>();
        List<CsMandatementGc> ListMandatementGc = new List<CsMandatementGc>();
        int IdCampagne = 0;
        int EtapeActuel;

        #endregion

        #region Constructeur

        public FrmSaiseMandatement()
        {
            InitializeComponent();
            this.txt_CF.MaxLength = 2;
            this.txt_BE.MaxLength = 3;
            this.txt_Numdeataire.MaxLength = 4;

            RemplirCampagne(UserConnecte.matricule);
        }

        //public FrmSaiseMandatement(int IdCampagne)
        //{
        //    InitializeComponent();
        //    this.IdCampagne=IdCampagne;

        //    RemplirCampagne(UserConnecte.matricule);

        //}

        public FrmSaiseMandatement(List<int> ListIdCampagne, int EtapeActuel)
        {
            InitializeComponent();

            this.txt_CF.MaxLength = 3;
            this.txt_BE.MaxLength = 5;
            this.txt_Numdeataire.MaxLength = 5;

            this.EtapeActuel = EtapeActuel;
            this.IdCampagne = ListIdCampagne.First();

            RemplirCampagne(UserConnecte.matricule);

        }
        #endregion

        #region Event Handler

        private void txt_CF_TextChanged(object sender, TextChangedEventArgs e)
        {
            txt_Numdeataire_Finale.Text = txt_CF.Text + txt_BE.Text + txt_Numdeataire.Text;
        }
        private void txt_BE_TextChanged(object sender, TextChangedEventArgs e)
        {
            txt_Numdeataire_Finale.Text = txt_CF.Text + txt_BE.Text + txt_Numdeataire.Text;

        }
        private void txt_Numdeataire_TextChanged(object sender, TextChangedEventArgs e)
        {
            txt_Numdeataire_Finale.Text = txt_CF.Text + txt_BE.Text + txt_Numdeataire.Text;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_Numdeataire.Text) && !string.IsNullOrWhiteSpace(txt_CF.Text) && !string.IsNullOrWhiteSpace(txt_BE.Text))
            {
                 //ListMandatementGc.ForEach(m=>m.NUMEROMANDATEMENT=txt_Numdeataire.Text);

                 List<CsDetailCampagneGc> lstDetail = ((List<CsDetailCampagneGc>)this.dg_facture.ItemsSource).ToList();
                 string NUMEROMANDATEMENT = txt_Numdeataire_Finale.Text;
                 lstDetail.ForEach(m => m.NUMEROMANDATEMENT = NUMEROMANDATEMENT);
                 SaveMandatement(lstDetail);
                 this.DialogResult = true;
            }
            else
            {
                Message.ShowWarning("Veuillez saisir le numero du mandatement", "Information");
            }
           
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<int> Listid = new List<int>();
            Listid.Add(ListCapagneGc.First().PK_ID);
            RejeterDemandeEtapePrecedente(Listid);
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
                    facture.MONTANT = item.SOLDEFACTURE ;
                    facture.NDOC = item.NDOC;
                    facture.USERCREATION = UserConnecte.matricule;
                    facture.DATECREATION  = System.DateTime.Now ;
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
            MiseAJourFacture();
        }

        private void MiseAJourFacture()
        {
            var DetailCampagne = (CsDetailCampagneGc)dg_facture.SelectedItem;
            if (DetailCampagne != null)
            {
                //if (DetailCampagne.IsMontantValide)
                //{
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
                            CsCampagneGc CampAnSelectioner = new CsCampagneGc();
                            CampAnSelectioner = (CsCampagneGc)dg_Campagne.SelectedItem;

                            var datasource = (List<CsDetailCampagneGc>)dg_facture.ItemsSource;
                            //txt_Montant_Mandatement.Text = datasource.Where(f=>f.IsMontantValide==true).Sum(dm => dm.MONTANT).ToString();
                            txt_Montant_Mandatement.Text = datasource.Sum(dm => dm.MONTANT).ToString();

                            var MandatementGc = ListMandatementGc.FirstOrDefault(m => m.FK_IDCAMPAGNA == CampAnSelectioner.PK_ID);
                            if (MandatementGc == null)
                            {
                                var Mandatement = new CsMandatementGc { FK_IDCAMPAGNA = CampAnSelectioner.PK_ID, MONTANT = datasource.Sum(dm => dm.MONTANT_VERSER), DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule, NUMEROMANDATEMENT = txt_Numdeataire.Text };
                                List<CsDetailMandatementGc> DETAILMANDATEMENTGC_ = new List<CsDetailMandatementGc>();
                                foreach (var item in datasource)
                                {
                                    if (item.MONTANT_VERSER != null && item.MONTANT_VERSER > 0)
                                    {
                                        CsDetailMandatementGc DetailMandatement = new CsDetailMandatementGc();

                                        DetailMandatement.CENTRE = item.CENTRE;
                                        DetailMandatement.CLIENT = item.CLIENT;
                                        DetailMandatement.ORDRE = item.ORDRE;
                                        DetailMandatement.DATECREATION = DateTime.Now;
                                        DetailMandatement.DATEMODIFICATION = DateTime.Now;
                                        DetailMandatement.FK_IDMANDATEMENT = Mandatement.PK_ID;
                                        DetailMandatement.MONTANT = item.MONTANT_VERSER;
                                        DetailMandatement.NDOC = item.NDOC;
                                        DetailMandatement.PERIODE = item.PERIODE;
                                        DetailMandatement.STATUS = item.STATUS;
                                        DetailMandatement.USERCREATION = UserConnecte.matricule;
                                        DetailMandatement.USERMODIFICATION = UserConnecte.matricule;

                                        DETAILMANDATEMENTGC_.Add(DetailMandatement);

                                    }

                                }
                                Mandatement.DETAILMANDATEMENTGC_ = DETAILMANDATEMENTGC_;
                                ListMandatementGc.Add(Mandatement);
                            }
                            else
                            {

                                MandatementGc.MONTANT = datasource.Where(f => f.IsMontantValide == true).Sum(dm => dm.MONTANT_VERSER);
                                MandatementGc.NUMEROMANDATEMENT = txt_Numdeataire.Text;
                                List<CsDetailMandatementGc> DETAILMANDATEMENTGC_ = new List<CsDetailMandatementGc>();
                                foreach (var item in datasource)
                                {
                                    if (item.MONTANT_VERSER != null && item.MONTANT_VERSER > 0 && item.IsMontantValide==true)
                                    {
                                        CsDetailMandatementGc DetailMandatement = new CsDetailMandatementGc();

                                        DetailMandatement.CENTRE = item.CENTRE;
                                        DetailMandatement.CLIENT = item.CLIENT;
                                        DetailMandatement.ORDRE = item.ORDRE;
                                        DetailMandatement.DATECREATION = DateTime.Now;
                                        DetailMandatement.DATEMODIFICATION = DateTime.Now;
                                        DetailMandatement.FK_IDMANDATEMENT = MandatementGc.PK_ID;
                                        DetailMandatement.MONTANT = item.MONTANT_VERSER;
                                        DetailMandatement.NDOC = item.NDOC;
                                        DetailMandatement.PERIODE = item.PERIODE;
                                        DetailMandatement.STATUS = item.STATUS;
                                        DetailMandatement.USERCREATION = UserConnecte.matricule;
                                        DetailMandatement.USERMODIFICATION = UserConnecte.matricule;

                                        DETAILMANDATEMENTGC_.Add(DetailMandatement);

                                    }

                                }
                                MandatementGc.DETAILMANDATEMENTGC_ = DETAILMANDATEMENTGC_;

                                var mand = ListMandatementGc.FirstOrDefault(m => m.PK_ID == MandatementGc.PK_ID);
                                var index = ListMandatementGc.IndexOf(mand);
                                ListMandatementGc[index] = MandatementGc;
                            }
                        }
                    }
                //}
            }
        }

        private void txt_MontantMandatement_TextChanged(object sender, TextChangedEventArgs e)
        {
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
            for (int index = 0; index < DetailCampagneGc.Count; index++)
            {
                DetailCampagneGc[index].IsMontantValide = false;
                DetailCampagneGc[index].MONTANT_VERSER = 0;
            }
            dg_facture.ItemsSource = DetailCampagneGc.OrderBy(d => d.NOM).ToList();
        }

        private void CheckBox_Unchecked_(object sender, RoutedEventArgs e)
        {
            var DetailCampagneSelectionner = (CsDetailCampagneGc)dg_facture.SelectedItem;
            if (DetailCampagneSelectionner!=null)
            {
                DetailCampagneSelectionner.IsMontantNonValide = false;
                DetailCampagneNonValider.Remove(DetailCampagneSelectionner); 
            }
        }
        private void CheckBox_Checked_(object sender, RoutedEventArgs e)
        {
            var DetailCampagneSelectionner = (CsDetailCampagneGc)dg_facture.SelectedItem;

            if (DetailCampagneSelectionner!=null)
            {
                if (CanShowWindows)
                {
                    FrmMontantMandatementVerser Frm = new FrmMontantMandatementVerser(DetailCampagneSelectionner.PK_ID);
                    Frm.Closing += Frm_Closing;
                    Frm.Show(); 
                }
                CanShowWindows = true;
               
            }
        }

        void Frm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var DataSource=(List<CsDetailCampagneGc>)dg_facture.ItemsSource;
            var Frm = ((FrmMontantMandatementVerser)sender);
            if (Frm.montant>0)
            {
                var DetailCampagneSelectionner = DataSource.FirstOrDefault(d => d.PK_ID == Frm.Pk_Id);
                int index = DataSource.IndexOf(DetailCampagneSelectionner);

                DetailCampagneSelectionner.MONTANT_VERSER = Frm.montant;

                DetailCampagneSelectionner.IsMontantValide = false;
                DetailCampagneSelectionner.IsMontantNonValide = true;

                DataSource[index] = DetailCampagneSelectionner;
                dg_facture.ItemsSource = DataSource.OrderBy(d => d.NOM).ToList();
                dg_facture.SelectedItem = DetailCampagneSelectionner;
            }
            else
            {
                var DetailCampagneSelectionner = (CsDetailCampagneGc)dg_facture.SelectedItem;

                DetailCampagneSelectionner.IsMontantValide = false;
                DetailCampagneSelectionner.IsMontantNonValide = true;

                DetailCampagneNonValider.Add(DetailCampagneSelectionner);
            }
            CanShowWindows = false;
            //dg_facture_CellEditEnded(null, null);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var DetailCampagneSelectionner = (CsDetailCampagneGc)dg_facture.SelectedItem;
                if (DetailCampagneSelectionner == null)
                    DetailCampagneSelectionner = (CsDetailCampagneGc)((CheckBox)sender).Tag;

                if (DetailCampagneSelectionner != null)
                {
                    CheckBox chb=(CheckBox)sender;
                    chb.Tag = DetailCampagneSelectionner;
                    //if (DetailCampagneSelectionner.IsMontantValide == false)
                    //{
                    DetailCampagneSelectionner.IsMontantValide = true;
                        //DetailCampagneSelectionner.IsMontantNonValide = false;

                        List<CsDetailCampagneGc> DataSource = (List<CsDetailCampagneGc>)dg_facture.ItemsSource;
                        int index = DataSource.IndexOf(DetailCampagneSelectionner);
                        DetailCampagneSelectionner.MONTANT_VERSER = DetailCampagneSelectionner.MONTANT_RESTANT;

                        #region Facture hore Campagne
                        if (DetailCampagneSelectionner.MONTANT_RESTANT == null && DetailCampagneSelectionner.IDCAMPAGNEGC == 0)
                            DetailCampagneSelectionner.MONTANT_VERSER = DetailCampagneSelectionner.MONTANT; 
                        #endregion

                        DataSource[index] = DetailCampagneSelectionner;

                        dg_facture.ItemsSource = DataSource.OrderBy(d => d.NOM).ToList();
                        dg_facture.SelectedItem = DetailCampagneSelectionner;

                        DetailCampagneValider.Add(DetailCampagneSelectionner);
                    //}
                }

                //dg_facture_CellEditEnded(null, null);
                MiseAJourFacture();
            }
            catch (Exception ex)
            {
                //CheckBox_Checked(null, null);
            }
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var DetailCampagneSelectionner = (CsDetailCampagneGc)((CheckBox)sender).Tag;
            if (DetailCampagneSelectionner == null)
                DetailCampagneSelectionner = (CsDetailCampagneGc)dg_facture.SelectedItem;

            if (DetailCampagneSelectionner!=null)
            {

                List<CsDetailCampagneGc> DataSource = (List<CsDetailCampagneGc>)dg_facture.ItemsSource;
                int index = DataSource.IndexOf(DetailCampagneSelectionner);
                DetailCampagneSelectionner.MONTANT_VERSER =0;
                DetailCampagneSelectionner.IsMontantValide = false;
                DataSource[index] = DetailCampagneSelectionner;

                dg_facture.ItemsSource = DataSource.OrderBy(d => d.NOM).ToList();
                dg_facture.SelectedItem = DetailCampagneSelectionner;

                DetailCampagneValider.Remove(DetailCampagneSelectionner); 
            }

            //dg_facture_CellEditEnded(null, null);
            MiseAJourFacture();
        }

        private void dg_facture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //dg_facture_CellEditEnded(null, null);
            MiseAJourFacture();
        }

        #endregion

        #region  Service
        private void RemplirCampagne(string Matricule)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RemplirCampagneByIdCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                    return;
                LaCampagneGc = args.Result;

                List<CsDetailCampagneGc> ListeDetailCampagneAsuprimmer = new List<CsDetailCampagneGc>();
                
                foreach (var item_ in LaCampagneGc.DETAILCAMPAGNEGC_)
                {
                    item_.MONTANT_RESTANT = 0;
                    item_.MONTANT_REGLER = 0;
                    foreach (var mand in LaCampagneGc.MANDATEMENTGC_)
                    {
                        var detailmand = mand.DETAILMANDATEMENTGC_.Where(dm => dm.CENTRE == item_.CENTRE && dm.CLIENT == item_.CLIENT && dm.ORDRE == item_.ORDRE && dm.NDOC == item_.NDOC);
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
                foreach (var item in ListeDetailCampagneAsuprimmer)
                {
                    LaCampagneGc.DETAILCAMPAGNEGC_.Remove(item);
                }
                if (LaCampagneGc.DETAILCAMPAGNEGC_.Count <= 0)
                {
                    Message.Show("Cette campagne à été totalement réglé par mandatement,prette à etre transmise à l'étape suivante", "Information");
                    LaCampagneGc.DETAILCAMPAGNEGC_ = ListeDetailCampagneAsuprimmer;
                
                }
                ListCapagneGc.Add(LaCampagneGc);
                dg_Campagne.ItemsSource = ListCapagneGc;
                if (ListCapagneGc != null && ListCapagneGc.Count != 0)
                   dg_Campagne.SelectedItem = ListCapagneGc.First();

                this.dg_facture.ItemsSource = LaCampagneGc.DETAILCAMPAGNEGC_;
                return;
            };
            service.RemplirCampagneByIdAsync(this.IdCampagne);
        }
        //private void RemplirCampagne(string Matricule )
        //{
        //    Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
        //    service.RemplirCampagneCompleted += (s, args) =>
        //    {
        //        if (args != null && args.Cancelled)
        //            return;
        //        if (args.Result == null)
        //            return;
        //        ListCapagneGc = args.Result;

        //        List<CsDetailCampagneGc> ListeDetailCampagneAsuprimmer = new List<CsDetailCampagneGc>();
        //        foreach (var item in ListCapagneGc)
        //        {
        //            foreach (var item_ in item.DETAILCAMPAGNEGC_)
        //            {
        //                item_.MONTANT_RESTANT = 0;
        //                item_.MONTANT_REGLER = 0;
        //                foreach (var mand in item.MANDATEMENTGC_)
        //                {
        //                    var detailmand = mand.DETAILMANDATEMENTGC_.Where(dm => dm.CENTRE == item_.CENTRE && dm.CLIENT == item_.CLIENT && dm.ORDRE == item_.ORDRE && dm.NDOC == item_.NDOC);
        //                    if (detailmand!=null)
        //                    {
        //                        item_.MONTANT_REGLER = item_.MONTANT_REGLER  + detailmand.Sum(c => c.MONTANT); 
        //                    }
        //                }
                        
        //                item_.MONTANT_RESTANT = item_.MONTANT - item_.MONTANT_REGLER;

        //                if (item_.MONTANT_RESTANT<=0)
        //                {
        //                    ListeDetailCampagneAsuprimmer.Add(item_);
        //                }
        //                item_.MONTANT_VERSER = 0;
        //            }
        //        }
        //        foreach (var item in ListeDetailCampagneAsuprimmer)
        //        {
        //            ListCapagneGc[0].DETAILCAMPAGNEGC_.Remove(item);
        //        }
        //        if (ListCapagneGc[0].DETAILCAMPAGNEGC_.Count<=0)
        //        {
        //            Message.Show("Cette campagne à été totalement réglé par mandatement,prette à etre transmise à l'étape suivante", "Information");

        //            ListCapagneGc[0].DETAILCAMPAGNEGC_ = ListeDetailCampagneAsuprimmer;
        //            //this.Close();
        //            //this.DialogResult = true;

        //            //txt_MontantMandatement.IsEnabled = false;
        //            //txt_Numdeataire.IsEnabled = false;
        //            //chbx_ToutValider.IsEnabled = false;
        //            //dg_facture.IsReadOnly = true;
        //        }
        //        dg_Campagne.ItemsSource = ListCapagneGc.Where(c => c.PK_ID == this.IdCampagne);
        //        dg_Campagne.SelectedItem = ListCapagneGc[0];
        //        return;
        //    };
        //    service.RemplirCampagneAsync(Matricule);
        //}
        private void SaveMandatement(List<CsDetailCampagneGc > ListMandatementGc)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.SaveMandatementCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                    return;
                if (args.Result==true)
                {
                    Message.Show("Mandatment enregistré avec succes", "Information");
                    List<int> Listid = new List<int>();
                    Listid.Add(ListCapagneGc.First().PK_ID);
                    EnvoyerDemandeEtapeSuivante(Listid);
                }
                else
                {
                    Message.Show("Le Mandatment n'a pas été enregistré ,veuillez saisir un numéro de mandatement different ", "Information");
                }

                return;
            };
            service.SaveMandatementAsync(ListMandatementGc,false);
        }

        private void EnvoyerDemandeEtapeSuivante(List<int> Listid)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.Protocole(), Utility.EndPoint("Workflow"));

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
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuel, ServiceWorkflow.CODEACTION.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }

        private void RejeterDemandeEtapePrecedente(List<int> Listid)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.Protocole(), Utility.EndPoint("Workflow"));

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
                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuel, ServiceWorkflow.CODEACTION.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }
        #endregion

    }
}


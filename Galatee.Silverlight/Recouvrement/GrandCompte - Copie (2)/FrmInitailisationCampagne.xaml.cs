using Galatee.Silverlight.Classes;
using Galatee.Silverlight.ServiceRecouvrement;
using Galatee.Silverlight.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmInitailisationCampagne : ChildWindow
    {
        #region Variables
        List<ServiceRecouvrement.CsAffectationGestionnaire> LstAffectation = new List<ServiceRecouvrement.CsAffectationGestionnaire>();
        List<ServiceRecouvrement.CsRegCli> LstCodeRegroupement = new List<ServiceRecouvrement.CsRegCli>();
        List<ServiceRecouvrement.CsRegCli> LstCodeRegroupement_User = new List<ServiceRecouvrement.CsRegCli>();
        List<CsLclient> Lstfacture=new List<CsLclient>();
        List<CsLclient> ListFacture_Selectionner=new List<CsLclient>();
        List<CsLclient> ListFacture_NonSelectionner = new List<CsLclient>();
        private List<CsCampagneGc> Anciennecamp=new List<CsCampagneGc>();

        #endregion

        #region Constructeurs

        public FrmInitailisationCampagne()
                {
                    InitializeComponent();
                    Decharger.Content = "<";
                    Charger.Content = ">";
                    RemplirCodeRegroupement();
                    RemplirAffectation();
                }

        #endregion

        #region Event Handler

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {                 
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btn_Rech_Click(object sender, RoutedEventArgs e)
        {
            if (cbo_regroupement.SelectedItem!=null && lbx_Periode.Items.Count>0)
            {
                List<string> ListRefem = new List<string>();
                foreach (var item in lbx_Periode.Items)
                {
                   ListRefem.Add(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(item.ToString()));
                }
                btn_Rech.IsEnabled = false;
                Remplirfacture((CsRegCli)cbo_regroupement.SelectedItem, ListRefem);
            }
            else
            {
                Message.Show("Veuillez vous assurer que vous avez selectionner un regroupement et saisis moin une periode", "Information");
            }
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AjouterPeriodeAListe();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ListFacture_Selectionner.Add((CsLclient)dg_facture.SelectedItem);
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ListFacture_Selectionner.Remove((CsLclient)dg_facture.SelectedItem);
        }

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            ListFacture_NonSelectionner.Add((CsLclient)dg_facture_Copy.SelectedItem);
        }
        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            ListFacture_NonSelectionner.Remove((CsLclient)dg_facture_Copy.SelectedItem);
        }
        
        private void Charger_Click(object sender, RoutedEventArgs e)
        {
            //var facture = ((List<ServiceRecouvrement.CsLclient>)dg_facture.ItemsSource);
            dg_facture.SelectedItems.Clear();

            foreach (var item in ListFacture_Selectionner)
            {
                dg_facture.SelectedItems.Add(item);
            }
            ListFacture_Selectionner.Clear();
            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsLclient>(dg_facture, dg_facture_Copy);

            LoadDataPager<CsLclient>(dg_facture.ItemsSource, datapager, dg_facture);

            LoadDataPager<CsLclient>(dg_facture_Copy.ItemsSource, datapager_Copy, dg_facture_Copy);

           
        }
        
        private void Decharger_Click(object sender, RoutedEventArgs e)
        {
            //var facture = ((List<ServiceRecouvrement.CsLclient>)dg_facture.ItemsSource);
            dg_facture_Copy.SelectedItems.Clear();

            foreach (var item in ListFacture_NonSelectionner)
            {
                dg_facture_Copy.SelectedItems.Add(item);
            }
            ListFacture_NonSelectionner.Clear();
            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsLclient>(dg_facture, dg_facture_Copy);

            LoadDataPager<CsLclient>(dg_facture.ItemsSource, datapager, dg_facture);

            LoadDataPager<CsLclient>(dg_facture_Copy.ItemsSource, datapager_Copy, dg_facture_Copy);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FrmFactureHorRegroupement frm = new FrmFactureHorRegroupement();
            frm.CallBack += frm_CallBack1;
            frm.Show();
        }

        private void OKButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (dg_facture_Copy.ItemsSource != null && cbo_regroupement.SelectedItem != null)
            {

                PagedCollectionView pcv = (PagedCollectionView)dg_facture_Copy.ItemsSource;
                List<CsLclient> CollectionName = new List<CsLclient>();
                CollectionName = (List<CsLclient>)pcv.SourceCollection;

                var ListeFacture = CollectionName;
                var regcli = (CsRegCli)cbo_regroupement.SelectedItem;
                SaveCampane(ListeFacture, regcli, UserConnecte.PK_ID);

                this.DialogResult = true;

            }
            else
            {
                Message.Show("Vous ne pouvez pas creer  de campagne sans au moin une facture selectionne", "Information");
            }
        }

        private void btn_supp_Click(object sender, RoutedEventArgs e)
        {
            if (lbx_Periode.SelectedItem != null && cbo_regroupement.SelectedItem!=null)
            {
                var periode = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(lbx_Periode.SelectedItem.ToString());
                List<CsDetailCampagneGc> factureAsupprimer = new List<CsDetailCampagneGc>();
                List<CsCampagneGc> CampagneAsupprimer = new List<CsCampagneGc>();
                foreach (var item in this.Anciennecamp)
                {
                    if (item.FK_IDREGCLI == ((CsRegCli)cbo_regroupement.SelectedItem).PK_ID)
                    {
                        foreach (var item_ in item.DETAILCAMPAGNEGC_)
                        {
                            if (item_.PERIODE == periode)
                            {
                                factureAsupprimer.Add(item_);
                            }
                        }
                    }
                }

                foreach (var item in factureAsupprimer)
                {
                    foreach (var item_ in this.Anciennecamp)
                    {
                        if (item_.DETAILCAMPAGNEGC_.Contains(item))
                        {
                            item_.DETAILCAMPAGNEGC_.Remove(item) ;
                        }

                        if (!(item_.DETAILCAMPAGNEGC_.Count > 0))
                        {
                            CampagneAsupprimer.Add(item_);
                        }
                    }
                    
                }
                foreach (var item in CampagneAsupprimer)
                {
                    if (this.Anciennecamp.Contains(item))
                    {
                        this.Anciennecamp.Remove(item);
                    }
                }


                //PagedCollectionView pcv = (PagedCollectionView)dg_facture_Copy.ItemsSource;
                //List<CsLclient> CollectionName = new List<CsLclient>();
                //CollectionName = (List<CsLclient>)pcv.SourceCollection;


                var ListFactureSupprimer = (PagedCollectionView)dg_facture_Copy.ItemsSource;
                List<CsLclient> Datasource = new List<CsLclient>();
                if (ListFactureSupprimer!=null)
                {
                    foreach (var item in (List<CsLclient>)ListFactureSupprimer.SourceCollection)
                    {
                        if (item.REFEM != periode)
                        {
                            Datasource.Add(item);
                        }
                    }
                }

                var ListFactureSupprimer_ = (PagedCollectionView)dg_facture.ItemsSource;
                List<CsLclient> Datasource_ = new List<CsLclient>();
                if (ListFactureSupprimer_ != null)
                {
                    foreach (var item in (List<CsLclient>)ListFactureSupprimer_.SourceCollection)
                    {
                        if (item.REFEM != periode)
                        {
                            Datasource_.Add(item);
                        }
                    }
                }

                //dg_facture_Copy.ItemsSource = Datasource;
                LoadDataPager<CsLclient>(Datasource, datapager_Copy, dg_facture_Copy);
                LoadDataPager<CsLclient>(Datasource_, datapager, dg_facture);
                
                lbx_Periode.Items.Remove(lbx_Periode.SelectedItem);
                //lbx_Periode.Items.Add(txt_periode.Text);

                

                //txt_periode.Text = string.Empty;
            }
            else
            {
                Message.Show("Veuillez selectionner la periode que vous souhaitez surpprimer", "Information");
            }
        }

        private void txt_periode_TextChanged(object sender, TextChangedEventArgs e)
        {
            var periode = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(txt_periode.Text);
            if (periode.Length == 6)
            {
                if (cbo_regroupement.SelectedItem != null && !string.IsNullOrWhiteSpace(periode))
                {
                    if (!lbx_Periode.Items.Contains(txt_periode.Text))
                    {
                        VerifierCampagneExiste((CsRegCli)cbo_regroupement.SelectedItem, periode);
                    }
                    else
                    {
                        Message.Show("Vous avez déja saisi cette période", "Information");
                    }
                }

            }

        }

        private void cbo_regroupement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActiverElement(true);
        }

        void frm_CallBack(object sender, Tarification.Helper.CustumEventArgs e)
        {
            AjouterPeriodeAListe();
    
            FrmCampagne Frm = (FrmCampagne)sender;
            //Implementer le callback
            if (e.Bag!=null)
            {
                if (!Frm.Arefaire)
                {
                    var camp = (CsCampagneGc)e.Bag;
                    if (Anciennecamp.FirstOrDefault(c => c.PK_ID == camp.PK_ID) != null)
                    {
                        this.Anciennecamp.Remove(camp);
                    }
                    //List<string> listperiode=new List<string>();

                    List<CsLclient> dataSource = new List<CsLclient>();
                    foreach (var item in camp.DETAILCAMPAGNEGC_)
                    {
                        CsLclient facture = new CsLclient();
                        facture.CENTRE = item.CENTRE;
                        facture.CLIENT = item.CLIENT;
                        facture.ORDRE = item.ORDRE;
                        facture.NOM = item.NOM;
                        facture.REFEM = item.PERIODE;
                        facture.MONTANT = item.MONTANT;
                        facture.NDOC = item.NDOC;
                        dataSource.Add(facture);
                    }
                    //dg_facture_Copy.ItemsSource = dataSource;
                    LoadDataPager<CsLclient>(dataSource, datapager_Copy, dg_facture_Copy);
                }
                else
                {
                    this.Anciennecamp.Add((CsCampagneGc)e.Bag);
                }
            }
            else
            {
                txt_periode.Text = string.Empty;
            }
        }

        void frm_CallBack1(object sender, Tarification.Helper.CustumEventArgs e)
        {
            //Implementer le callback
            if (e.Bag!=null)
            {
                var ListFacture = (List<CsLclient>)e.Bag;

                PagedCollectionView pvc = (PagedCollectionView)dg_facture_Copy.ItemsSource;

                List<CsLclient> datasource = (List<CsLclient>)pvc.SourceCollection;
                if (datasource==null)
                {
                    datasource = new List<CsLclient>();
                }
                foreach (var item in ListFacture)
                {
                    if (datasource.FirstOrDefault(f=>f.CENTRE==item.CENTRE && f.CLIENT==item.CLIENT && f.ORDRE== item.ORDRE && f.NDOC==item.NDOC)==null)
                    {
                        datasource.Add(item);
                    }
                }
                //dg_facture_Copy.ItemsSource = datasource;
                LoadDataPager<CsLclient>(datasource, datapager_Copy, dg_facture_Copy);
            }
        }

        void frm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            txt_periode.Text = string.Empty;
        }

        private void lbx_Periode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void datapager_PageIndexChanged(object sender, EventArgs e)
        {


        }

        #endregion

        #region Service

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
                //}
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
                        if (LstCodeRegroupement!=null)
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
        private void Remplirfacture(CsRegCli csRegCli, List<string> listperiode)
        {
            allowProgressBar();
            ActiverElement(false);
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RemplirfactureCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                Lstfacture = args.Result;
                if (this.Anciennecamp!=null)
                {
                    foreach (var item_ in this.Anciennecamp)
                    {
                        foreach (var item in item_.DETAILCAMPAGNEGC_)
                        {
                            CsLclient facture = new CsLclient();
                            facture.CENTRE = item.CENTRE;
                            facture.CLIENT = item.CLIENT;
                            facture.ORDRE = item.ORDRE;
                            facture.NOM = item.NOM;
                            facture.REFEM = item.PERIODE;
                            facture.MONTANT = item.MONTANT;
                            facture.NDOC = item.NDOC;
                            Lstfacture.Add(facture);
                        }
                    }
                    
                }

                System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(Lstfacture);

                LoadDataPager<CsLclient>(Lstfacture, datapager, dg_facture);
                //LoadDataPager<CsLclient>(dg_facture_Copy.ItemsSource, datapager_Copy, dg_facture_Copy);
                
                btn_Rech.IsEnabled = true;
                desableProgressBar();
                ActiverElement(true);

                return;
            };
            service.RemplirfactureAsync(csRegCli,listperiode);
        }

        private void SaveCampane(List<CsLclient> ListFacturation,CsRegCli csRegCli, int? ID_USER)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.SaveCampaneCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                {
                    Message.ShowInformation("L'enregistrement ne c'est pas correctement effecaion veuillez refaire l'opération", "Recouvremen");
                }
                Shared.ClasseMEthodeGenerique.InitWOrkflow(args.Result.Split('.')[0], UserConnecte.FK_IDCENTRE, "Galatee.Silverlight.Recouvrement.FrmInitailisationCampagne", args.Result.Split('.')[1]);

                Message.ShowInformation("L'enregistrement effecaion avec succes", "Recouvremen");

                return;
            };
            service.SaveCampaneAsync(ListFacturation, csRegCli, ID_USER);
        }

        private void VerifierCampagneExiste(CsRegCli csRegCli, string periode)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.VerifierCampagneExisteCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                    return;
                FrmCampagne frm = new FrmCampagne(args.Result);
                frm.CallBack += frm_CallBack;
                frm.Closing += frm_Closing;
                frm.Show();
                return;
            };
            service.VerifierCampagneExisteAsync(csRegCli, periode);
        }

       

       

        #endregion

        #region Methode

        private void LoadDataPager<T>(object ItemsSource, DataPager datapager_, DataGrid dg)
        {
            if (ItemsSource!=null)
            {
                if ((ItemsSource as List<T>) != null)
                {
                    System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ItemsSource as List<T>);
                    dg.ItemsSource = view;
                    datapager_.Source = view;
                }
            }
           
        }
        private void LoadAllFactureNonImpayer(List<string> listperiode)
        {
            lbx_Periode.Items.Add(txt_periode.Text);
            foreach (var item in lbx_Periode.Items)
            {
                listperiode.Add(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(item.ToString()));
            }
            Remplirfacture((CsRegCli)cbo_regroupement.SelectedItem, listperiode);
        }

        private void AjouterPeriodeAListe()
        {
            if (!string.IsNullOrWhiteSpace(txt_periode.Text))
            {
                lbx_Periode.Items.Add(txt_periode.Text);
                txt_periode.Text = string.Empty;
            }
            else
            {
                Message.Show("Veuillez saisir la periode que vous souhaitez ajouter", "Information");
            }
        }

        private void ReLoadingGrid()
        {
            var UtilisateurSelect = UserConnecte.PK_ID;
            var Affectation = LstAffectation.Where(a => a.FK_IDADMUTILISATEUR == UtilisateurSelect && a.ISACTIVE == true);

            if (Affectation != null)
            {
                var ListIdRegcliCorrespondant = Affectation.Select(a => a.FK_IDREGLI);
                LstCodeRegroupement_User = LstCodeRegroupement.Where(r => ListIdRegcliCorrespondant.Contains(r.PK_ID)).ToList();
                
                cbo_regroupement.DisplayMemberPath = "NOM";
                cbo_regroupement.SelectedValuePath = "CODE";
                cbo_regroupement.ItemsSource = LstCodeRegroupement_User;

            }
        }

        private void ActiverElement(bool stat)
        {
            txt_periode.IsEnabled = stat;

            lbx_Periode.IsEnabled = stat;
            cbo_regroupement.IsEnabled = stat;

            btn_ajouterFactureHorReg.IsEnabled = stat;
            btn_Rech.IsEnabled = stat;
            btn_ajouterPeriod.IsEnabled = stat;            
            btn_supp.IsEnabled = stat;
            OKButton.IsEnabled = stat;
            Charger.IsEnabled = stat;
            Decharger.IsEnabled = stat;
            
        }

        void allowProgressBar()
        {
            progressBar1.IsEnabled = true;
            progressBar1.Visibility = Visibility.Visible;
            progressBar1.IsIndeterminate = true;
        }
        void desableProgressBar()
        {
            progressBar1.IsIndeterminate = false;
            progressBar1.Visibility = Visibility.Collapsed;
        }

        #endregion 

    }
}


﻿using Galatee.Silverlight.Classes;
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
        List<CsLclient> Lstfacture=new List<CsLclient>();
        List<CsLclient> ListFacture_Selectionner=new List<CsLclient>();
        List<CsLclient> ListFacture_NonSelectionner = new List<CsLclient>();
        private List<CsCampagneGc> Anciennecamp=new List<CsCampagneGc>();
        public List<CsDetailCampagneGc> DetailCampagneGc = new List<CsDetailCampagneGc>();
        List<int> Listid = new List<int>();

        // Declare a delegate.
        public delegate void MethodeDeCallBack<T>(T param);
        #endregion

        #region Constructeurs

        public FrmInitailisationCampagne()
                {
                    InitializeComponent();
                    Decharger.Content = "<";
                    Charger.Content = ">";
                    RemplirCodeRegroupement(UserConnecte.PK_ID );
                    btn_DeChargerTout.Content = ">>";
                    btn_ChargerTout.Content = "<<";
                    this.txt_Regroupement.MaxLength = SessionObject.Enumere.TailleCodeRegroupement;
                    MethodeDeCallBack<List<CsDetailCampagneGc>> handler = SaveMandatement;
                    chb_PlagePeriode.IsChecked = false;

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
            //On s 'assure qu'il à sélèctionné un regroupement et qu'il à précisé une ou des période "annéé+mois"
            if (cbo_regroupement.SelectedItem!=null && lbx_Periode.Items.Count>0)
            {
                #region Recupération de la liste des période au format attendu
                    List<string> ListRefem = new List<string>();
                    foreach (var item in lbx_Periode.Items)
                    {
                       ListRefem.Add(ClasseMEthodeGenerique.FormatPeriodeAAAAMM(item.ToString()));
                    }
                #endregion

                //On grise le bouton de recherche pour empêcher une autre action
                btn_Rech.IsEnabled = false;

                #region On récupere le produit selectionné
                    List<int> lstIdProduit = new List<int>();
                    if (this.Chk_BT.IsChecked== true)
                        lstIdProduit.Add(SessionObject.ListeDesProduit.FirstOrDefault(t => t.CODE == SessionObject.Enumere.Electricite).PK_ID);
                    if (this.Chk_MT.IsChecked == true )
                        lstIdProduit.Add(SessionObject.ListeDesProduit.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ElectriciteMT).PK_ID);
                #endregion

                #region On prend tous les produit si aucun  produit n'est selectionné si non on prend le ou les produis cochés en compte
                    if (lstIdProduit.Count == 0)
                    {
                        lstIdProduit.Add(SessionObject.ListeDesProduit.FirstOrDefault(t => t.CODE == SessionObject.Enumere.Electricite).PK_ID);
                        lstIdProduit.Add(SessionObject.ListeDesProduit.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ElectriciteMT).PK_ID);
                        lstIdProduit.Add(SessionObject.ListeDesProduit.FirstOrDefault(t => t.CODE == SessionObject.Enumere.Eau).PK_ID);
                        RemplirfactureProduit((CsRegCli)cbo_regroupement.SelectedItem, ListRefem, lstIdProduit);
                        //Remplirfacture((CsRegCli)cbo_regroupement.SelectedItem, ListRefem);
                    }
                    else
                        RemplirfactureProduit((CsRegCli)cbo_regroupement.SelectedItem, ListRefem, lstIdProduit);
                #endregion
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
        private void btn_ChargerTout_Click_1(object sender, RoutedEventArgs e)
        {

            //var facture = ((List<ServiceRecouvrement.CsLclient>)dg_facture.ItemsSource);
            dg_facture_Copy.SelectedItems.Clear();
            System.Windows.Data.PagedCollectionView view = (System.Windows.Data.PagedCollectionView)datapager_Copy.Source;

            ListFacture_NonSelectionner = ((List<CsLclient>)view.SourceCollection).ToList();
            foreach (var item in ListFacture_NonSelectionner)
            {
                dg_facture_Copy.SelectedItems.Add(item);
                this.txt_TotalFactureEnvoie.Text = ListFacture_NonSelectionner.Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant);

            }
            ListFacture_NonSelectionner.Clear();
            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsLclient>(dg_facture_Copy,dg_facture);

            LoadDataPager<CsLclient>(dg_facture.ItemsSource, datapager, dg_facture);
            
            LoadDataPager<CsLclient>(dg_facture_Copy.ItemsSource, datapager_Copy, dg_facture_Copy);


            txt_TotalFacture.Text = ((List<CsLclient>)dg_facture.ItemsSource).Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant);
            txt_TotalFactureEnvoie.Text = ((List<CsLclient>)dg_facture_Copy.ItemsSource).Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant);



        }
        private void Charger_Click(object sender, RoutedEventArgs e)
        {
            //var facture = ((List<ServiceRecouvrement.CsLclient>)dg_facture.ItemsSource);
            dg_facture.SelectedItems.Clear();

            foreach (var item in ListFacture_Selectionner)
            {
                dg_facture.SelectedItems.Add(item);
                this.txt_TotalFacture.Text = ListFacture_Selectionner.Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant);
            }
            ListFacture_Selectionner.Clear();
            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsLclient>(dg_facture, dg_facture_Copy);

            LoadDataPager<CsLclient>(dg_facture.ItemsSource, datapager, dg_facture);

            LoadDataPager<CsLclient>(dg_facture_Copy.ItemsSource, datapager_Copy, dg_facture_Copy);

            if (dg_facture.ItemsSource != null )
                txt_TotalFacture.Text = ((List<CsLclient>)dg_facture.ItemsSource).Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant);

            if (dg_facture_Copy.ItemsSource != null )
                txt_TotalFactureEnvoie.Text = ((List<CsLclient>)dg_facture_Copy.ItemsSource).Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant);
        }



        private void Decharger_Click(object sender, RoutedEventArgs e)
        {
            //var facture = ((List<ServiceRecouvrement.CsLclient>)dg_facture.ItemsSource);
            dg_facture_Copy.SelectedItems.Clear();

            foreach (var item in ListFacture_NonSelectionner)
            {
                dg_facture_Copy.SelectedItems.Add(item);
                this.txt_TotalFactureEnvoie.Text = ListFacture_NonSelectionner.Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant);

            }
            ListFacture_NonSelectionner.Clear();
            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsLclient>(dg_facture_Copy,dg_facture);

            LoadDataPager<CsLclient>(dg_facture.ItemsSource, datapager, dg_facture);

            LoadDataPager<CsLclient>(dg_facture_Copy.ItemsSource, datapager_Copy, dg_facture_Copy);

            txt_TotalFacture.Text = ((List<CsLclient>)dg_facture.ItemsSource).Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant);
            txt_TotalFactureEnvoie.Text = ((List<CsLclient>)dg_facture_Copy.ItemsSource).Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant);
        }
        private void btn_DeChargerTout_Click(object sender, RoutedEventArgs e)
        {
            //var facture = ((List<ServiceRecouvrement.CsLclient>)dg_facture.ItemsSource);
            dg_facture.SelectedItems.Clear();
            System.Windows.Data.PagedCollectionView view = (System.Windows.Data.PagedCollectionView)datapager.Source;

            ListFacture_Selectionner = ((List<CsLclient>)view.SourceCollection).ToList();
            foreach (var item in ListFacture_Selectionner)
            {
                dg_facture.SelectedItems.Add(item);
                this.txt_TotalFacture.Text = ListFacture_Selectionner.Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant);
            }
            ListFacture_Selectionner.Clear();
            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsLclient>(dg_facture, dg_facture_Copy);

            LoadDataPager<CsLclient>(dg_facture.ItemsSource, datapager, dg_facture);

            LoadDataPager<CsLclient>(dg_facture_Copy.ItemsSource, datapager_Copy, dg_facture_Copy);

            txt_TotalFacture.Text = ((List<CsLclient>)dg_facture.ItemsSource).Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant);
            txt_TotalFactureEnvoie.Text = ((List<CsLclient>)dg_facture_Copy.ItemsSource).Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant);
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

                //PagedCollectionView pcv = (PagedCollectionView)dg_facture_Copy.ItemsSource;
                //List<CsLclient> CollectionName = new List<CsLclient>();

                System.Windows.Data.PagedCollectionView view = (System.Windows.Data.PagedCollectionView)datapager_Copy.Source;
                var ListeFacture = ((List<CsLclient>)view.SourceCollection).ToList();

                //CollectionName = (List<CsLclient>)pcv.SourceCollection;

                //var ListeFacture = CollectionName;
                var regcli = (CsRegCli)cbo_regroupement.SelectedItem;
                if (ListeFacture.Count>0)
                {
                    SaveCampane(ListeFacture, regcli, UserConnecte.PK_ID);
                }
                else
                {
                    Message.Show("Vous ne pouvez pas creer  de campagne sans au moins une facture selectionne", "Information");

                }

                this.DialogResult = true;

            }
            else
            {
                Message.Show("Vous ne pouvez pas creer  de campagne sans au moins une facture selectionne", "Information");
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
                    if (item.FK_IDREGROUPEMENT == ((CsRegCli)cbo_regroupement.SelectedItem).PK_ID)
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


                var ListFactureSupprimer = (List<CsLclient>)dg_facture_Copy.ItemsSource;
                List<CsLclient> Datasource = new List<CsLclient>();
                if (ListFactureSupprimer != null)
                {
                    foreach (var item in (List<CsLclient>)ListFactureSupprimer)
                    {
                        if (item.REFEM != periode)
                        {
                            Datasource.Add(item);
                        }
                    }
                }
                 
                if (dg_facture.ItemsSource != null )
                {
                    var ListFactureSupprimer_ = (List<CsLclient>)dg_facture.ItemsSource;
                    List<CsLclient> Datasource_ = new List<CsLclient>();
                    if (ListFactureSupprimer_ != null)
                    {
                        foreach (var item in (List<CsLclient>)ListFactureSupprimer_)
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
                }
                

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
            if (this.cbo_regroupement.SelectedItem != null)
            {
                this.txt_Regroupement.Text = ((CsRegCli)this.cbo_regroupement.SelectedItem).CODE;
                ActiverElement(true);
            }
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
                        facture.FK_IDCLIENT = item.FK_IDCLIENT;
                        facture.FK_IDLCLIENT = item.FK_IDLCLIENT;
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

                //PagedCollectionView pvc = (PagedCollectionView)dg_facture_Copy.ItemsSource;
                //List<CsLclient> datasource = (List<CsLclient>)pvc.SourceCollection;
                List<CsLclient> datasource = (List<CsLclient>)dg_facture_Copy.ItemsSource;
                if (datasource==null)
                {
                    datasource = new List<CsLclient>();
                }
                foreach (var item in ListFacture)
                {
                    if (datasource.FirstOrDefault(f=>f.CENTRE==item.CENTRE && f.CLIENT==item.CLIENT && f.ORDRE== item.ORDRE && f.NDOC==item.NDOC)==null)
                    {
                        item.SOLDEFACTURE = item.SOLDEFACTURE ;
                        datasource.Add(item);
                    }
                }
                //dg_facture_Copy.ItemsSource = datasource;

                LoadDataPager<CsLclient>(datasource, datapager_Copy, dg_facture_Copy);

                txt_TotalFactureEnvoie.Text = datasource.Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant);
                txt_TotalFacture.Text = dg_facture.ItemsSource != null ? ((List<CsLclient>)dg_facture.ItemsSource).Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant) : 0.ToString(SessionObject.FormatMontant);
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



        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            lbl_Periode_debut.Visibility = Visibility.Visible;
            lbl_Periode_Fin.Visibility = Visibility.Visible;
            txt_periode_Debut.Visibility = Visibility.Visible;
            txt_periode_Fin.Visibility = Visibility.Visible;

        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            lbl_Periode_debut.Visibility = Visibility.Collapsed;
            lbl_Periode_Fin.Visibility = Visibility.Collapsed;
            txt_periode_Debut.Visibility = Visibility.Collapsed;
            txt_periode_Fin.Visibility = Visibility.Collapsed;
        }

        private void txt_Regroupement_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txt_Regroupement.Text.Length == SessionObject.Enumere.TailleCodeRegroupement)
            {
                ServiceRecouvrement.CsRegCli leRegcli = LstCodeRegroupement.FirstOrDefault(y => y.CODE == this.txt_Regroupement.Text);
                if (leRegcli != null && !string.IsNullOrEmpty(leRegcli.CODE))
                    this.cbo_regroupement.SelectedItem = leRegcli;
                else
                    Message.ShowInformation("Le regroupement saisi est inexistant", "Recouvrement");

            }
        }

   
        #endregion

        #region Service

        private void RemplirCodeRegroupement(int iduserconnect)
        {
            try
            {

                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.RetourneRegroupementGestionnairesCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    LstCodeRegroupement = args.Result;
                    if (LstCodeRegroupement != null)
                    {
                        cbo_regroupement.DisplayMemberPath = "NOM";
                        cbo_regroupement.SelectedValuePath = "CODE";
                        cbo_regroupement.ItemsSource = LstCodeRegroupement;

                    }
                    return;
                };
                service.RetourneRegroupementGestionnairesAsync(iduserconnect);
                //}
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
                            facture.SOLDEFACTURE = item.MONTANT;
                            facture.NDOC = item.NDOC;
                            Lstfacture.Add(facture);
                        }
                    }
                    
                }

                System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(Lstfacture);
                LoadDataPager<CsLclient>(Lstfacture, datapager, dg_facture);
                if (dg_facture.ItemsSource != null)
                    this.txt_TotalFacture.Text = Lstfacture.Sum(c => (c.MONTANT != null ? c.MONTANT : 0)).Value.ToString(SessionObject.FormatMontant);

                btn_Rech.IsEnabled = true;
                desableProgressBar();
                ActiverElement(true);

                return;
            };
            service.RemplirfactureAsync(csRegCli,listperiode);
        }


        //Recherche des factures sen fonction des critère entré
        private void RemplirfactureProduit(CsRegCli csRegCli, List<string> listperiode,List<int> lstIdProduit)
        {
            //Activation du busy indicator
            allowProgressBar();

            //Grisé certain éléments du formulaire pour bloqué certainnes actions
            ActiverElement(false);

            //Exécution du service web recupération des factures
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RemplirfactureAvecProduitCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                Lstfacture = args.Result;
                if (this.Anciennecamp != null)
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
                            facture.SOLDEFACTURE = item.MONTANT;
                            facture.NDOC = item.NDOC;
                            Lstfacture.Add(facture);
                        }
                    }
                }

                System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(Lstfacture);
                LoadDataPager<CsLclient>(Lstfacture, datapager, dg_facture);
                if (dg_facture.ItemsSource != null)
                    this.txt_TotalFacture.Text = Lstfacture.Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant);

                btn_Rech.IsEnabled = true;
                desableProgressBar();
                ActiverElement(true);

                return;
            };
            service.RemplirfactureAvecProduitAsync(csRegCli, listperiode,lstIdProduit );
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
                    Message.ShowInformation("L'enregistrement ne s'est pas correctement effectué, veuillez reprendre l'opération", "Recouvrement");
                }
                if (chb_AvisDeCredit.IsChecked==true)
                {
                    DetailCampagneGc = (List<CsDetailCampagneGc>)args.Result.First().Valeur;
                    Listid.Add(int.Parse(args.Result.First().Key.ToString().Split('.')[0]));
                    InitWOrkflow((args.Result.First().Key.ToString().Split('.')[0]), UserConnecte.FK_IDCENTRE, "Galatee.Silverlight.Recouvrement.FrmInitailisationCampagne",args.Result.First().Key.ToString().Split('.')[1], SaveMandatement, DetailCampagneGc);

                }
                else
                {
                    Shared.ClasseMEthodeGenerique.InitWOrkflow(args.Result.First().Key.ToString().Split('.')[0], UserConnecte.FK_IDCENTRE, "Galatee.Silverlight.Recouvrement.FrmInitailisationCampagne", args.Result.First().Key.ToString().Split('.')[1]);

                }
                Message.ShowInformation("L'enregistrement effectué avec succès", "Recouvremen");

                return;
            };
            service.SaveCampaneAsync(ListFacturation, csRegCli, ID_USER);
        }
        //BSY - WOrkflow le 24/09/2016 a 00h12
        public  void InitWOrkflow(string IdDemandeCree, int centreDemandeCree, string FrmInitialisation, string NumeroDemandeTableTravail, MethodeDeCallBack<dynamic> handler, dynamic Pamatre)
        {
            //Ajouté par WCO le 04/08/2015
            //Pour l'insertion d'une demande directement dans ma table
            Workflow.WorkflowDmdManager managerWkf = new Workflow.WorkflowDmdManager();
            managerWkf.InsertionDemandeWorkflowComplete += (str_rsl) =>
            {
                if (str_rsl.StartsWith("ERR"))
                {
                    Message.ShowError(new Exception(str_rsl), "Création d'une demande");
                }
                else
                {
                    handler(Pamatre);
                    Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + NumeroDemandeTableTravail,
                        Silverlight.Resources.Devis.Languages.txtDevis);
              
                }
            };
            managerWkf.InsererMaDemande(IdDemandeCree, FrmInitialisation, centreDemandeCree, NumeroDemandeTableTravail);
        }
        public void SaveMandatement(dynamic param)
        {
            List<CsDetailCampagneGc> ListMandatementGc = (List<CsDetailCampagneGc>) param;
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.SaveMandatementCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                    return;
                if (args.Result == true)
                {
                    EnvoyerDemandeEtapeSuivante(Listid);
                }
                else
                {
                    Message.Show("Le Mandatment n'a pas été enregistré ", "Information");
                }

                return;
            };
            service.SaveMandatementAsync(ListMandatementGc, true);
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
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, 176, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
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

        private void RecupererPeriodeDePlage(string DebutPeriode,string FinPeriode)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RecupererPeriodeDePlageCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                    return;
                var DataSource = args.Result;
                DataSource.ForEach(a => ClasseMEthodeGenerique.FormatPeriodeMMAAAA(a));
                foreach (var item in lbx_Periode.Items)
                {
                    DataSource.Add(item.ToString());
                }
                lbx_Periode.Items.Clear();
                foreach (var p in DataSource.OrderBy(p=>p))
                {
                    var periode = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(p);
                    lbx_Periode.Items.Add(periode);
                }
                return;
            };
            service.RecupererPeriodeDePlageAsync(DebutPeriode, FinPeriode);
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
                    dg.ItemsSource = null;
                    dg.ItemsSource = view.SourceCollection;
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

            if (chb_PlagePeriode.IsChecked!=true)
            {
                AjouterPeriodeSaisie();
            }
            else
            {
                AjouterPeriodeDePlage();

            }
        }

        //private void AjouterPeriodeDePlage()
        //{
        //    List<string> Plageperiode = new List<string>();

        //    string Debut=ClasseMEthodeGenerique.FormatPeriodeAAAAMM(txt_periode_Debut.Text);
        //    string Fin=ClasseMEthodeGenerique.FormatPeriodeAAAAMM(txt_periode_Fin.Text);

        //    RecupererPeriodeDePlage(Debut, Fin);
        //}
        private void AjouterPeriodeDePlage()
        {
            List<string> Plageperiode = new List<string>();
            //string Debut = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(txt_periode_Debut.Text);
            //string Fin = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(txt_periode_Fin.Text);
            Char delimiter = '/';
            int AnneDepart = int.Parse(txt_periode_Debut.Text.Split(delimiter).Last());
            int MoisDepart = int.Parse(txt_periode_Debut.Text.Split(delimiter).First());
            int AnneFin = int.Parse(txt_periode_Fin.Text.Split(delimiter).Last());
            int MoisFin = int.Parse(txt_periode_Fin.Text.Split(delimiter).First());
            //var PeriodeDebut = int.Parse(Debut);
            //var Periodefin = int.Parse(Fin);
            for (int i = MoisDepart; (AnneDepart <= AnneFin); i++)
            {
                if (i <= 12)
                {
                    Plageperiode.Add(AnneDepart.ToString("0000") + i.ToString("00"));
                }
                else
                {
                    i = 0;
                    AnneDepart++;
                }
                if (i == MoisFin && AnneDepart == AnneFin)
                {
                    break;
                }
            }
            lbx_Periode.Items.Clear();
            foreach (var p in Plageperiode.OrderBy(p => p))
            {
                var periode = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(p);
                lbx_Periode.Items.Add(periode);
            }
            //RecupererPeriodeDePlage(Debut, Fin);
        }




        private void AjouterPeriodeSaisie()
        {
            if (!string.IsNullOrWhiteSpace(txt_periode.Text))
            {
                if (!lbx_Periode.Items.Contains(txt_periode.Text))
                {
                    var DataSource = new List<string>{txt_periode.Text};
                    foreach (var item in lbx_Periode.Items)
                    {
                        DataSource.Add(item.ToString());
                    }
                    lbx_Periode.Items.Clear();
                    DataSource=DataSource.OrderBy(p => p).ToList();
                    foreach (var p in DataSource)
                    {
                        lbx_Periode.Items.Add(p);
                    }
                }
                txt_periode.Text = string.Empty;
            }
            else
            {
                Message.Show("Veuillez saisir la periode que vous souhaitez ajouter", "Information");
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

            txt_periode_Debut.IsEnabled = stat;
            txt_periode_Fin.IsEnabled = stat;

            
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


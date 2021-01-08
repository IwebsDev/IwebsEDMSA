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
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceParametrage;

namespace Galatee.Silverlight.Parametrage
{
    public partial class UcDEMCOUT : ChildWindow, INotifyPropertyChanged
    {
        public UcDEMCOUT()
        {
            InitializeComponent();
        }

        public UcDEMCOUT(int numero, string text, string TableNom)
        {
            InitializeComponent();
            num = numero;
            this.Title = text;
            //this.TableName = TableNom;

            //InitialiserValue(datagrid, services,zones);
             this.GetData(numero);
        }

        int num;
        //List<CsProduit> produits = new List<CsProduit>();
        List<CsTdemCout> majData = new List<CsTdemCout>();
        ObservableCollection<CsTdemCout> donnesDatagrid = new ObservableCollection<CsTdemCout>();
        List<CsProduit> Lservices = new List<CsProduit>();

        private string combobaseTaxname = "cbo_Tax";
        private string combobaseProduitname = "cbo_Produit";
        private string combobaseOperationname = "cbo_Operation";
        private string combobaseRequestname = "cbo_Requests";

        List<CsProduit> rowcomboselectedProduit = new List<CsProduit>();
        List<CsLibelle> rowcomboselectedRequest = new List<CsLibelle>();

        List<CsLibelle> rowcomboselectedCoper1 = new List<CsLibelle>();
        List<CsLibelle> rowcomboselectedCoper2 = new List<CsLibelle>();
        List<CsLibelle> rowcomboselectedCoper3 = new List<CsLibelle>();
        List<CsLibelle> rowcomboselectedCoper4 = new List<CsLibelle>();
        List<CsLibelle> rowcomboselectedCoper5 = new List<CsLibelle>();

        List<CsLibelle> rowcomboselectedCtax1 = new List<CsLibelle>();
        List<CsLibelle> rowcomboselectedCtax2 = new List<CsLibelle>();
        List<CsLibelle> rowcomboselectedCtax3 = new List<CsLibelle>();
        List<CsLibelle> rowcomboselectedCtax4 = new List<CsLibelle>();
        List<CsLibelle> rowcomboselectedCtax5 = new List<CsLibelle>();

        List<CsLibelle> LTaxes = new List<CsLibelle>();
        List<CsProduit> LProduits = new List<CsProduit>();
        List<CsLibelle> LOperations = new List<CsLibelle>();
        List<CsLibelle> LRequests = new List<CsLibelle>();

        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion


        public ObservableCollection<CsTdemCout> DonnesDatagrid
        {
            get { return donnesDatagrid; }
            set
            {
                if (value == donnesDatagrid)
                    return;
                donnesDatagrid = value;
                NotifyPropertyChanged("DonnesDatagrid");
            }
        }
        
        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void GetData(int numtable)
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                client.SelectAllProductsCompleted += (ssender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            MessageBox.Show(error, ".SelectAllProducts", MessageBoxButton.OK);
                            desableProgressBar();
                            this.DialogResult = true;
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            MessageBox.Show("No data found ", ".SelectAllProducts", MessageBoxButton.OK);
                            desableProgressBar();
                            this.DialogResult = true;
                            return;
                        }

                        CsInit emptyplace = new CsInit();
                        LProduits.Clear();
                        LProduits.AddRange(args.Result);

                        ParametrageClient proxy = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                        proxy.SelectAll_CTAXCompleted += (s1, args1) =>
                            {
                                if (args1.Cancelled || args1.Error != null)
                                {
                                    string error = args1.Error.Message;
                                    MessageBox.Show(error, ".SelectAll_CTAX", MessageBoxButton.OK);
                                    desableProgressBar();
                                    this.DialogResult = true;
                                    return;
                                }

                                if (args1.Result == null || args1.Result.Count == 0)
                                {
                                    MessageBox.Show("No data found ", ".SelectAll_CTAX", MessageBoxButton.OK);
                                    desableProgressBar();
                                    return;
                                }

                                LTaxes.Clear();
                                foreach (CsCtax tax in args1.Result)
                                    LTaxes.Add(new CsLibelle() { CODE = tax.CODE, LIBELLE = tax.LIBELLE });

                                ParametrageClient proxy2 = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                                proxy2.SelectCoperLibelle100Completed += (sdi, resultdia) =>
                                    {
                                        if (resultdia.Cancelled || resultdia.Error != null)
                                        {
                                            string error = resultdia.Error.Message;
                                            MessageBox.Show(error, ".SelectCoperLibelle100", MessageBoxButton.OK);
                                            desableProgressBar();
                                            this.DialogResult = true;
                                            return;
                                        }

                                        if (resultdia.Result == null || resultdia.Result.Count == 0)
                                        {
                                            MessageBox.Show("No data found ", ".SelectCoperLibelle100", MessageBoxButton.OK);
                                            desableProgressBar();
                                            //this.DialogResult = true;
                                            return;
                                        }

                                        DonnesDatagrid.Clear();
                                        LOperations.Clear();
                                        LOperations.AddRange(resultdia.Result);

                                        ParametrageClient param = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                                        param.SelectAllTypeDemCoutCompleted += (sen, paramR) =>
                                            {
                                                if (paramR.Cancelled || paramR.Error != null)
                                                {
                                                    string error = paramR.Error.Message;
                                                    MessageBox.Show(error, ".SelectAllTypeDemCout", MessageBoxButton.OK);
                                                    desableProgressBar();
                                                    this.DialogResult = true;
                                                    return;
                                                }

                                                if (paramR.Result == null || paramR.Result.Count == 0)
                                                {
                                                    MessageBox.Show("No data found ", ".SelectAllTypeDemCout", MessageBoxButton.OK);
                                                    desableProgressBar();
                                                    return;
                                                }

                                                ParametrageClient tdemande = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                                                tdemande.SelectAllTypeDemandeCompleted += (send, tdemanR) =>
                                                    {

                                                        if (tdemanR.Cancelled || tdemanR.Error != null)
                                                        {
                                                            string error = tdemanR.Error.Message;
                                                            MessageBox.Show(error, ".SelectAllTypeDemCout", MessageBoxButton.OK);
                                                            desableProgressBar();
                                                            this.DialogResult = true;
                                                            return;
                                                        }

                                                        if (tdemanR.Result == null || tdemanR.Result.Count == 0)
                                                        {
                                                            MessageBox.Show("No data found ", ".SelectAllTypeDemCout", MessageBoxButton.OK);
                                                            desableProgressBar();
                                                            return;
                                                        }

                                                        List<CsTypeDemande> types = new List<CsTypeDemande>();
                                                        types.AddRange(tdemanR.Result);
                                                        foreach (CsTypeDemande demande in types)
                                                            LRequests.Add(new CsLibelle() { CODE = demande.TDEM, LIBELLE = demande.LIBELLE });

                                                        List<CsTdemCout> liste = new List<CsTdemCout>();
                                                        liste.AddRange(paramR.Result);
                                                        foreach (CsTdemCout tdout in liste)
                                                            DonnesDatagrid.Add(tdout);


                                                        AddNewDataInDataGrid();
                                                        DataContext = this;
                                                        // peupler la liste des nom de tables 

                                                        try
                                                        {
                                                            // Initializer les element du tableau rowcomboselectedObject
                                                            //Initializer les element du tableau columAmount
                                                            foreach (CsTdemCout tag in donnesDatagrid)
                                                            {
                                                                rowcomboselectedProduit.Add(LProduits.FirstOrDefault(p => p.CODE  == tag.PRODUIT));
                                                                rowcomboselectedRequest.Add(LRequests.FirstOrDefault(p => p.CODE == tag.TDEM));

                                                                rowcomboselectedCoper1.Add(LOperations.FirstOrDefault(p => p.CODE == tag.COPER1));
                                                                rowcomboselectedCoper2.Add(LOperations.FirstOrDefault(p => p.CODE == tag.COPER2));
                                                                rowcomboselectedCoper3.Add(LOperations.FirstOrDefault(p => p.CODE == tag.COPER3));
                                                                rowcomboselectedCoper4.Add(LOperations.FirstOrDefault(p => p.CODE == tag.COPER4));
                                                                rowcomboselectedCoper5.Add(LOperations.FirstOrDefault(p => p.CODE == tag.COPER5));

                                                                rowcomboselectedCtax1.Add(LTaxes.FirstOrDefault(p => p.CODE == tag.TAXE1));
                                                                rowcomboselectedCtax2.Add(LTaxes.FirstOrDefault(p => p.CODE == tag.TAXE2));
                                                                rowcomboselectedCtax3.Add(LTaxes.FirstOrDefault(p => p.CODE == tag.TAXE3));
                                                                rowcomboselectedCtax4.Add(LTaxes.FirstOrDefault(p => p.CODE == tag.TAXE4));
                                                                rowcomboselectedCtax5.Add(LTaxes.FirstOrDefault(p => p.CODE == tag.TAXE5));
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            string error = ex.Message;
                                                        }

                                                        dtgrdDemCout.ItemsSource = donnesDatagrid;
                                                    };
                                        tdemande.SelectAllTypeDemandeAsync();
                                            };
                                        param.SelectAllTypeDemCoutAsync();
                                    };
                                proxy2.SelectCoperLibelle100Async();
                            };
                        proxy.SelectAll_CTAXAsync();
                    };
                client.SelectAllProductsAsync();

            }
            catch (Exception ex)
            {
                string error= ex.Message;
            }

        }

        void desableProgressBar()
        {
            progressBar1.IsIndeterminate = false;
            progressBar1.Visibility = Visibility.Collapsed;
        }

        void AddNewDataInDataGrid()
        {
            DonnesDatagrid.Add(new CsTdemCout() { DMAJ = DateTime.Now });
        }

        void allowProgressBar()
        {
            progressBar1.IsEnabled = true;
            progressBar1.Visibility = Visibility.Visible;
            progressBar1.IsIndeterminate = true;
        }

        void lvwResultat_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

        }

        bool DonneeEstIntegre()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }
        }

        void ValiderSaisieDatagrid()
        {
            dtgrdDemCout.UpdateLayout();

        }


        void button1_Click(object sender, RoutedEventArgs e)
        {
            ValiderSaisieDatagrid();
            List<CsTdemCout> liste = new List<CsTdemCout>();

            foreach (CsTdemCout cas in DonnesDatagrid)
                liste.Add(cas);

            ParametrageClient insert = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
            insert.insertOrUpdateTypeDemandeCoutCompleted += (snder, insertR) =>
            {
                if (insertR.Cancelled || insertR.Error != null)
                {
                    string error = insertR.Error.Message;
                    MessageBox.Show(error, "insertOrUpdateTypeDemandeCout", MessageBoxButton.OK);
                    desableProgressBar();
                    return;
                }

                if (insertR.Result == null || insertR.Result.Count == 0)
                {
                    MessageBox.Show("Error on insert ", "insertOrUpdateTypeDemandeCout", MessageBoxButton.OK);
                    desableProgressBar();
                    return;
                }

                InitialiserDonneeDataGrid(insertR.Result);

            };
            insert.insertOrUpdateTypeDemandeCoutAsync(liste, majData);
        }

        void InitialiserDonneeDataGrid(List<CsTdemCout> listecas)
        {
            try
            {
                while (DonnesDatagrid.Count > 1)
                {
                    if (!string.IsNullOrEmpty(DonnesDatagrid[DonnesDatagrid.Count - 1].TDEM) &&
                       !string.IsNullOrEmpty(DonnesDatagrid[DonnesDatagrid.Count - 1].CENTRE))
                        DonnesDatagrid.RemoveAt(DonnesDatagrid.Count - 1);
                    else
                        DonnesDatagrid.RemoveAt(DonnesDatagrid.Count - 2);
                }

                foreach (CsTdemCout cas in listecas)
                    DonnesDatagrid.Add(cas);

                CsTdemCout casnull = DonnesDatagrid.FirstOrDefault(p => string.IsNullOrEmpty(p.CENTRE) && string.IsNullOrEmpty(p.TDEM));
                DonnesDatagrid.Remove(casnull);
                DonnesDatagrid.Add(new CsTdemCout() { DMAJ = DateTime.Now });

                majData.Clear();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        void dgINIT_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {

            CsDiacomp u = e.Row.DataContext as CsDiacomp; //fetch the row data
            //u.DMAJ = DateTime.Now;
            //donnesDatagrid[e.Row.GetIndex()].ROWID = u.ROWID;

            btnOk.IsEnabled = true;
            btnOk.Visibility = System.Windows.Visibility.Visible;

            try
            {
                //if (majData.Count > 0 && majData != null)
                //{
                //    if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                //    {
                //        foreach (CsDiacomp t in majData)
                //        {
                //            if (t.ROWID == u.ROWID)
                //            {
                //                t.CENTRE = u.CENTRE;
                //                t.PRODUIT = u.PRODUIT;
                //                t.DMAJ = DateTime.Now;
                //                t.LIBELLE = u.LIBELLE;
                //                t.CTAXAV = u.CTAXAV;
                //                t.CTAXDOS = u.CTAXDOS;
                //                t.CTAXPOL = u.CTAXPOL;
                //                t.DIAMETRE = u.DIAMETRE;
                //            }
                //        }
                //    }
                //    else
                //        majData.Add(u);
                //}
                //else
                //{
                //    majData.Add(u);
                //}
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Voulez vous vraiment supprimer cet element", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                CsTdemCout selected = dtgrdDemCout.SelectedItem as CsTdemCout;
                if (selected != null)
                {
                    ParametrageClient delete = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                    delete.DeleteDemCouTCompleted += (del, argDel) =>
                    {
                        if (argDel.Cancelled || argDel.Error != null)
                        {
                            string error = argDel.Error.Message;
                            MessageBox.Show(error, "DeleteDemCouT", MessageBoxButton.OK);
                            desableProgressBar();
                            return;
                        }

                        if (argDel.Result == false)
                        {
                            MessageBox.Show("Error on insert/update ", "DeleteDemCouT", MessageBoxButton.OK);
                            desableProgressBar();
                            return;
                        }

                        DonnesDatagrid.Remove(selected);
                    };
                    delete.DeleteDemCouTAsync(selected.CENTRE, selected.PRODUIT, selected.TDEM);
                }
                else
                    MessageBox.Show("vous devez selectionner un item dans la liste ");
            }
        }
        /// <summary>
        /// Permet de changeer la valeur de la propriete produit dans la liste des elements 
        /// a inserer .
        /// PROBLMENE : a l'ajout de l'object dans la newData list ,la propriete PRODUIT est a 1 ou 3,mais apres cett valeur prend le libelle du produit
        /// (water ou electricitt) ??????????????????
        /// </summary>
        /// <param name="listes"></param>
        void bntCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void cbo_Produit_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                var src = sender as ComboBox;
                string comboxselected = string.Empty;
                CsProduit comboObject = src.SelectedItem as CsProduit;

                CsProduit selectedObject = new CsProduit();
                if (src != null)
                {
                    comboxselected = comboObject.CODE;
                    selectedObject = comboObject;

                    string rank = src.Name.Substring(combobaseProduitname.Length);
                    rowcomboselectedProduit[int.Parse(rank)] = selectedObject;

                    //donnesDatagrid[int.Parse(rank)].PRODUIT = comboxselected;

                    //CsDiacomp u = donnesDatagrid[int.Parse(rank)];
                    //// ajout des element modifies dans la liste des elements modifies

                    //if (majData.Count > 0 && majData != null)
                    //{
                    //    if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                    //    {
                    //        foreach (CsDiacomp t in majData)
                    //        {
                    //            if (t.ROWID == u.ROWID)
                    //                t.PRODUIT = u.PRODUIT;
                    //        }
                    //    }
                    //    else
                    //        majData.Add(u);
                    //}
                    //else
                    //{
                    //    majData.Add(u);
                    //}
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void cbo_Tax_DropDownClosed(object sender, EventArgs e)
        {
            //try
            //{
            //    var src = sender as ComboBox;
            //    string comboxselected = string.Empty;
            //    CsCtax comboObject = src.SelectedItem as CsCtax;

            //    CsCtax selectedObject = new CsCtax();
            //    if (src != null)
            //    {
            //        comboxselected = comboObject.LIBELLE;
            //        selectedObject = comboObject;

            //        string rank = src.Name.Substring(combobaseTaxname.Length);
            //        rowcomboselectedTax[int.Parse(rank)] = selectedObject;

            //        //donnesDatagrid[int.Parse(rank)].CTAXAV = comboxselected;

            //        //CsDiacomp u = donnesDatagrid[int.Parse(rank)];
            //        //// ajout des element modifies dans la liste des elements modifies

            //        //if (majData.Count > 0 && majData != null)
            //        //{
            //        //    if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
            //        //    {
            //        //        foreach (CsDiacomp t in majData)
            //        //        {
            //        //            if (t.ROWID == u.ROWID)
            //        //                t.CTAXAV = u.CTAXAV;
            //        //        }
            //        //    }
            //        //    else
            //        //        majData.Add(u);
            //        //}
            //        //else
            //        //{
            //        //    majData.Add(u);
            //        //}
            //    }
            //}

            //catch (Exception ex)
            //{
            //    string error = ex.Message;
            //}
        }

        private void cbo_Services_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                var src = sender as ComboBox;
                string comboxselected = string.Empty;
                CsProduit comboObject = src.SelectedItem as CsProduit;

                CsProduit selectedObject = new CsProduit();
                if (src != null)
                {
                    comboxselected = comboObject.CODE;
                    selectedObject = comboObject;

                    string rank = src.Name.Substring(combobaseProduitname.Length);
                    rowcomboselectedProduit[int.Parse(rank)] = selectedObject;

                    DonnesDatagrid[int.Parse(rank)].PRODUIT = comboxselected;

                    CsTdemCout u = DonnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.FirstOrDefault(p => p.CENTRE == u.CENTRE && p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsTdemCout t in majData)
                            {
                                if (t.ROWID == u.ROWID)
                                    t.PRODUIT = u.PRODUIT;
                            }
                        }
                        else
                            majData.Add(u);
                    }
                    else
                    {
                        majData.Add(u);
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void cbo_Requests_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                var src = sender as ComboBox;
                string comboxselected = string.Empty;
                CsLibelle comboObject = src.SelectedItem as CsLibelle;

                CsLibelle selectedObject = new CsLibelle();
                if (src != null)
                {
                    comboxselected = comboObject.CODE;
                    selectedObject = comboObject;

                    string rank = src.Name.Substring(combobaseRequestname.Length);
                    rowcomboselectedRequest[int.Parse(rank)] = selectedObject;

                    DonnesDatagrid[int.Parse(rank)].TDEM = comboxselected;

                    CsTdemCout u = DonnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsTdemCout t in majData)
                            {
                                if (t.ROWID == u.ROWID)
                                    t.TDEM = u.TDEM;
                            }
                        }
                        else
                            majData.Add(u);
                    }
                    else
                    {
                        majData.Add(u);
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void cbo_Operation1_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                var src = sender as ComboBox;
                string comboxselected = string.Empty;
                CsLibelle comboObject = src.SelectedItem as CsLibelle;

                CsLibelle selectedObject = new CsLibelle();
                if (src != null)
                {
                    comboxselected = comboObject.CODE;
                    selectedObject = comboObject;

                    string rank = src.Name.Substring(combobaseOperationname.Length);
                    rowcomboselectedCoper1[int.Parse(rank)] = selectedObject;

                    DonnesDatagrid[int.Parse(rank)].COPER1 = comboxselected;

                    CsTdemCout u = DonnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.FirstOrDefault(p =>  p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsTdemCout t in majData)
                            {
                                if (t.ROWID == u.ROWID)
                                    t.COPER1 = u.COPER1;
                            }
                        }
                        else
                            majData.Add(u);
                    }
                    else
                    {
                        majData.Add(u);
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void cbo_Operation2_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                var src = sender as ComboBox;
                string comboxselected = string.Empty;
                CsLibelle comboObject = src.SelectedItem as CsLibelle;

                CsLibelle selectedObject = new CsLibelle();
                if (src != null)
                {
                    comboxselected = comboObject.CODE;
                    selectedObject = comboObject;

                    string rank = src.Name.Substring(combobaseOperationname.Length);
                    rowcomboselectedCoper2[int.Parse(rank)] = selectedObject;

                    DonnesDatagrid[int.Parse(rank)].COPER2 = comboxselected;

                    CsTdemCout u = DonnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsTdemCout t in majData)
                            {
                                if (t.ROWID == u.ROWID)
                                    t.COPER2 = u.COPER2;
                            }
                        }
                        else
                            majData.Add(u);
                    }
                    else
                    {
                        majData.Add(u);
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void cbo_Operation3_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                var src = sender as ComboBox;
                string comboxselected = string.Empty;
                CsLibelle comboObject = src.SelectedItem as CsLibelle;

                CsLibelle selectedObject = new CsLibelle();
                if (src != null)
                {
                    comboxselected = comboObject.CODE;
                    selectedObject = comboObject;

                    string rank = src.Name.Substring(combobaseOperationname.Length);
                    rowcomboselectedCoper3[int.Parse(rank)] = selectedObject;

                    DonnesDatagrid[int.Parse(rank)].COPER3 = comboxselected;

                    CsTdemCout u = DonnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsTdemCout t in majData)
                            {
                                if (t.ROWID == u.ROWID)
                                    t.COPER3 = u.COPER3;
                            }
                        }
                        else
                            majData.Add(u);
                    }
                    else
                    {
                        majData.Add(u);
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void cbo_Operation4_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                var src = sender as ComboBox;
                string comboxselected = string.Empty;
                CsLibelle comboObject = src.SelectedItem as CsLibelle;

                CsLibelle selectedObject = new CsLibelle();
                if (src != null)
                {
                    comboxselected = comboObject.CODE;
                    selectedObject = comboObject;

                    string rank = src.Name.Substring(combobaseOperationname.Length);
                    rowcomboselectedCoper4[int.Parse(rank)] = selectedObject;

                    DonnesDatagrid[int.Parse(rank)].COPER4 = comboxselected;

                    CsTdemCout u = DonnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsTdemCout t in majData)
                            {
                                if (t.ROWID == u.ROWID)
                                    t.COPER4 = u.COPER4;
                            }
                        }
                        else
                            majData.Add(u);
                    }
                    else
                    {
                        majData.Add(u);
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void cbo_Operation5_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                var src = sender as ComboBox;
                string comboxselected = string.Empty;
                CsLibelle comboObject = src.SelectedItem as CsLibelle;

                CsLibelle selectedObject = new CsLibelle();
                if (src != null)
                {
                    comboxselected = comboObject.CODE;
                    selectedObject = comboObject;

                    string rank = src.Name.Substring(combobaseOperationname.Length);
                    rowcomboselectedCoper5[int.Parse(rank)] = selectedObject;

                    DonnesDatagrid[int.Parse(rank)].COPER5 = comboxselected;

                    CsTdemCout u = DonnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsTdemCout t in majData)
                            {
                                if (t.ROWID == u.ROWID)
                                    t.COPER5 = u.COPER5;
                            }
                        }
                        else
                            majData.Add(u);
                    }
                    else
                    {
                        majData.Add(u);
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void cbo_Tax1_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                var src = sender as ComboBox;
                string comboxselected = string.Empty;
                CsLibelle comboObject = src.SelectedItem as CsLibelle;

                CsLibelle selectedObject = new CsLibelle();
                if (src != null)
                {
                    comboxselected = comboObject.CODE;
                    selectedObject = comboObject;

                    string rank = src.Name.Substring(combobaseTaxname.Length);
                    rowcomboselectedCtax1[int.Parse(rank)] = selectedObject;

                    DonnesDatagrid[int.Parse(rank)].TAXE1 = comboxselected;

                    CsTdemCout u = DonnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsTdemCout t in majData)
                            {
                                if (t.ROWID == u.ROWID)
                                    t.TAXE1 = u.TAXE1;
                            }
                        }
                        else
                            majData.Add(u);
                    }
                    else
                    {
                        majData.Add(u);
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void cbo_Tax2_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                var src = sender as ComboBox;
                string comboxselected = string.Empty;
                CsLibelle comboObject = src.SelectedItem as CsLibelle;

                CsLibelle selectedObject = new CsLibelle();
                if (src != null)
                {
                    comboxselected = comboObject.CODE;
                    selectedObject = comboObject;

                    string rank = src.Name.Substring(combobaseTaxname.Length);
                    rowcomboselectedCtax2[int.Parse(rank)] = selectedObject;

                    DonnesDatagrid[int.Parse(rank)].TAXE2 = comboxselected;

                    CsTdemCout u = DonnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsTdemCout t in majData)
                            {
                                if (t.ROWID == u.ROWID)
                                    t.TAXE2 = u.TAXE2;
                            }
                        }
                        else
                            majData.Add(u);
                    }
                    else
                    {
                        majData.Add(u);
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void cbo_Tax3_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                var src = sender as ComboBox;
                string comboxselected = string.Empty;
                CsLibelle comboObject = src.SelectedItem as CsLibelle;

                CsLibelle selectedObject = new CsLibelle();
                if (src != null)
                {
                    comboxselected = comboObject.CODE;
                    selectedObject = comboObject;

                    string rank = src.Name.Substring(combobaseTaxname.Length);
                    rowcomboselectedCtax3[int.Parse(rank)] = selectedObject;

                    DonnesDatagrid[int.Parse(rank)].TAXE3 = comboxselected;

                    CsTdemCout u = DonnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsTdemCout t in majData)
                            {
                                if (t.ROWID == u.ROWID)
                                    t.TAXE3 = u.TAXE3;
                            }
                        }
                        else
                            majData.Add(u);
                    }
                    else
                    {
                        majData.Add(u);
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void cbo_Tax4_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                var src = sender as ComboBox;
                string comboxselected = string.Empty;
                CsLibelle comboObject = src.SelectedItem as CsLibelle;

                CsLibelle selectedObject = new CsLibelle();
                if (src != null)
                {
                    comboxselected = comboObject.CODE;
                    selectedObject = comboObject;

                    string rank = src.Name.Substring(combobaseTaxname.Length);
                    rowcomboselectedCtax4[int.Parse(rank)] = selectedObject;

                    DonnesDatagrid[int.Parse(rank)].TAXE4 = comboxselected;

                    CsTdemCout u = DonnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsTdemCout t in majData)
                            {
                                if (t.ROWID == u.ROWID)
                                    t.TAXE4 = u.TAXE4;
                            }
                        }
                        else
                            majData.Add(u);
                    }
                    else
                    {
                        majData.Add(u);
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void cbo_Tax5_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                var src = sender as ComboBox;
                string comboxselected = string.Empty;
                CsLibelle comboObject = src.SelectedItem as CsLibelle;

                CsLibelle selectedObject = new CsLibelle();
                if (src != null)
                {
                    comboxselected = comboObject.CODE;
                    selectedObject = comboObject;

                    string rank = src.Name.Substring(combobaseTaxname.Length);
                    rowcomboselectedCtax5[int.Parse(rank)] = selectedObject;

                    DonnesDatagrid[int.Parse(rank)].TAXE5 = comboxselected;

                    CsTdemCout u = DonnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsTdemCout t in majData)
                            {
                                if (t.ROWID == u.ROWID)
                                    t.TAXE5 = u.TAXE5;
                            }
                        }
                        else
                            majData.Add(u);
                    }
                    else
                    {
                        majData.Add(u);
                    }
                }
            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void dtgrdDemCout_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                int currentIndex = e.Row.GetIndex();

                // initialiser les property and data des diff combobox des lignes
                ComboBox comboservice = this.dtgrdDemCout.Columns[1].GetCellContent(e.Row) as ComboBox;
                ComboBox comborequest = this.dtgrdDemCout.Columns[2].GetCellContent(e.Row) as ComboBox;

                ComboBox comboOp1= this.dtgrdDemCout.Columns[3].GetCellContent(e.Row) as ComboBox;
                ComboBox comboOp2 = this.dtgrdDemCout.Columns[4].GetCellContent(e.Row) as ComboBox;
                ComboBox comboOp3 = this.dtgrdDemCout.Columns[5].GetCellContent(e.Row) as ComboBox;
                ComboBox comboOp4 = this.dtgrdDemCout.Columns[6].GetCellContent(e.Row) as ComboBox;
                ComboBox comboOp5 = this.dtgrdDemCout.Columns[7].GetCellContent(e.Row) as ComboBox;

                ComboBox comboTax1 = this.dtgrdDemCout.Columns[8].GetCellContent(e.Row) as ComboBox;
                ComboBox comboTax2 = this.dtgrdDemCout.Columns[9].GetCellContent(e.Row) as ComboBox;
                ComboBox comboTax3 = this.dtgrdDemCout.Columns[10].GetCellContent(e.Row) as ComboBox;
                ComboBox comboTax4 = this.dtgrdDemCout.Columns[11].GetCellContent(e.Row) as ComboBox;
                ComboBox comboTax5 = this.dtgrdDemCout.Columns[12].GetCellContent(e.Row) as ComboBox;


                // definir le nom des different combobox 
                comboservice.Name = combobaseProduitname + e.Row.GetIndex().ToString();
                comboservice.ItemsSource = LProduits;
                comboservice.SelectedItem = rowcomboselectedProduit[currentIndex];

                comborequest.Name = combobaseRequestname + e.Row.GetIndex().ToString();

                comboOp1.Name = combobaseOperationname + e.Row.GetIndex().ToString();
                comboOp2.Name = combobaseOperationname + e.Row.GetIndex().ToString();
                comboOp3.Name = combobaseOperationname + e.Row.GetIndex().ToString();
                comboOp4.Name = combobaseOperationname + e.Row.GetIndex().ToString();
                comboOp1.Name = combobaseOperationname + e.Row.GetIndex().ToString();

                comboTax1.Name = combobaseTaxname + e.Row.GetIndex().ToString();
                comboTax2.Name = combobaseTaxname + e.Row.GetIndex().ToString();
                comboTax3.Name = combobaseTaxname + e.Row.GetIndex().ToString();
                comboTax4.Name = combobaseTaxname + e.Row.GetIndex().ToString();
                comboTax5.Name = combobaseTaxname + e.Row.GetIndex().ToString();



                // Initialiser les combox contenus dans la datagrid
                comborequest.ItemsSource = LRequests ;

                comboOp1.ItemsSource = LOperations;
                comboOp2.ItemsSource = LOperations;
                comboOp3.ItemsSource = LOperations;
                comboOp4.ItemsSource = LOperations;
                comboOp5.ItemsSource = LOperations;

                comboTax1.ItemsSource = LTaxes;
                comboTax2.ItemsSource = LTaxes;
                comboTax3.ItemsSource = LTaxes;
                comboTax4.ItemsSource = LTaxes;
                comboTax5.ItemsSource = LTaxes;

                //determiner les elements selectionnés


                comborequest.SelectedItem = rowcomboselectedRequest[currentIndex];

                comboOp1.SelectedItem = rowcomboselectedCoper1[currentIndex];
                comboOp2.SelectedItem = rowcomboselectedCoper2[currentIndex];
                comboOp3.SelectedItem = rowcomboselectedCoper3[currentIndex];
                comboOp4.SelectedItem = rowcomboselectedCoper4[currentIndex];
                comboOp5.SelectedItem = rowcomboselectedCoper5[currentIndex];

                comboTax1.SelectedItem = rowcomboselectedCtax1[currentIndex];
                comboTax2.SelectedItem = rowcomboselectedCtax2[currentIndex];
                comboTax3.SelectedItem = rowcomboselectedCtax3[currentIndex];
                comboTax4.SelectedItem = rowcomboselectedCtax4[currentIndex];
                comboTax5.SelectedItem = rowcomboselectedCtax5[currentIndex];



                 // desactiver les colonnes pour eviter une eventuelle initialisation

                DataGridColumn serviceColumn = this.dtgrdDemCout.Columns[1];
                DataGridColumn requestColumn = this.dtgrdDemCout.Columns[2];

                DataGridColumn operation1Column = this.dtgrdDemCout.Columns[3];
                DataGridColumn operation2Column = this.dtgrdDemCout.Columns[4];
                DataGridColumn operation3Column = this.dtgrdDemCout.Columns[5];
                DataGridColumn operation4Column = this.dtgrdDemCout.Columns[6];
                DataGridColumn operation5Column = this.dtgrdDemCout.Columns[7];

                DataGridColumn tax1Column = this.dtgrdDemCout.Columns[8];
                DataGridColumn tax2Column = this.dtgrdDemCout.Columns[9];
                DataGridColumn tax3Column = this.dtgrdDemCout.Columns[10];
                DataGridColumn tax4Column = this.dtgrdDemCout.Columns[11];
                DataGridColumn tax5Column = this.dtgrdDemCout.Columns[12];

                serviceColumn.IsReadOnly = true; // Works
                requestColumn.IsReadOnly = true; // Works

                operation1Column.IsReadOnly = true; // Works
                operation2Column.IsReadOnly = true; // Works
                operation3Column.IsReadOnly = true; // Works
                operation4Column.IsReadOnly = true; // Works
                operation5Column.IsReadOnly = true; // Works

                tax1Column.IsReadOnly = true; // Works
                tax2Column.IsReadOnly = true; // Works
                tax3Column.IsReadOnly = true; // Works
                tax4Column.IsReadOnly = true; // Works
                tax5Column.IsReadOnly = true; // Works



            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void dtgrdDemCout_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CsTdemCout selected = dtgrdDemCout.SelectedItem as CsTdemCout;

            txtAmount1.Text = selected.MONTANT1.ToString();
            txtAmount2.Text = selected.MONTANT2.ToString();
            txtAmount3.Text = selected.MONTANT3.ToString();
            txtAmount4.Text = selected.MONTANT4.ToString();
            txtAmount5.Text = selected.MONTANT5.ToString();
        }

        private void txtAmount1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CsTdemCout u = dtgrdDemCout.SelectedItem as CsTdemCout;
                u.MONTANT1 = Convert.ToDecimal(txtAmount1.Text);

                DonnesDatagrid[dtgrdDemCout.SelectedIndex].MONTANT1 = u.MONTANT1;

                if (majData.Count > 0 && majData != null)
                {
                    if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                    {
                        foreach (CsTdemCout t in majData)
                        {
                            if (t.ROWID == u.ROWID)
                                t.MONTANT1 = u.MONTANT1;
                        }
                    }
                    else
                        majData.Add(u);
                }
                else
                {
                    majData.Add(u);
                }
            }
            catch (Exception ex )
            {

                string error = ex.Message;
            }
            
        }

        private void txtAmount2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CsTdemCout u = dtgrdDemCout.SelectedItem as CsTdemCout;
                u.MONTANT2 = Convert.ToDecimal(txtAmount2.Text);

                DonnesDatagrid[dtgrdDemCout.SelectedIndex].MONTANT2 = u.MONTANT2;

                if (majData.Count > 0 && majData != null)
                {
                    if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                    {
                        foreach (CsTdemCout t in majData)
                        {
                            if (t.ROWID == u.ROWID)
                                t.MONTANT2 = u.MONTANT2;
                        }
                    }
                    else
                        majData.Add(u);
                }
                else
                {
                    majData.Add(u);
                }
            }
            catch (Exception ex)
            {

                string error = ex.Message;
            }
          
        }

        private void txtAmount3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CsTdemCout u = dtgrdDemCout.SelectedItem as CsTdemCout;
                u.MONTANT3 = Convert.ToDecimal(txtAmount3.Text);


                DonnesDatagrid[dtgrdDemCout.SelectedIndex].MONTANT3 = u.MONTANT3;

                if (majData.Count > 0 && majData != null)
                {
                    if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                    {
                        foreach (CsTdemCout t in majData)
                        {
                            if (t.ROWID == u.ROWID)
                                t.MONTANT3 = u.MONTANT3;
                        }
                    }
                    else
                        majData.Add(u);
                }
                else
                {
                    majData.Add(u);
                }
            }
            catch (Exception ex)
            {

                string error = ex.Message;
            }
           
        }

        private void txtAmount4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CsTdemCout u = dtgrdDemCout.SelectedItem as CsTdemCout;
                u.MONTANT4 = Convert.ToDecimal(txtAmount4.Text);

                DonnesDatagrid[dtgrdDemCout.SelectedIndex].MONTANT4 = u.MONTANT4;

                if (majData.Count > 0 && majData != null)
                {
                    if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                    {
                        foreach (CsTdemCout t in majData)
                        {
                            if (t.ROWID == u.ROWID)
                                t.MONTANT4 = u.MONTANT4;
                        }
                    }
                    else
                        majData.Add(u);
                }
                else
                {
                    majData.Add(u);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            
        }

        private void txtAmount5_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CsTdemCout u = dtgrdDemCout.SelectedItem as CsTdemCout;
                u.MONTANT5 = Convert.ToDecimal(txtAmount5.Text);

                DonnesDatagrid[dtgrdDemCout.SelectedIndex].MONTANT5 = u.MONTANT5;

                if (majData.Count > 0 && majData != null)
                {
                    if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                    {
                        foreach (CsTdemCout t in majData)
                        {
                            if (t.ROWID == u.ROWID)
                                t.MONTANT5 = u.MONTANT5;
                        }
                    }
                    else
                        majData.Add(u);
                }
                else
                {
                    majData.Add(u);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

        }

    }
}



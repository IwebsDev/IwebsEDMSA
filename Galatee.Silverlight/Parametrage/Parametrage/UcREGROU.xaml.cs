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
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceParametrage;

namespace Galatee.Silverlight.Parametrage
{
    public partial class UcREGROU : ChildWindow
    {
        public UcREGROU()
        {
            InitializeComponent();
        }

        public UcREGROU(int numero, string text, string TableNom)
        {
            InitializeComponent();
            num = numero;
            this.Title = text;
            this.TableName = TableNom;

            //InitialiserValue(datagrid, services,zones);
             this.GetData(numero);
        }

        int num;
        List<CsProduit> produits = new List<CsProduit>();
        List<CsRegrou> donnesDatagrid = new List<CsRegrou>();
        List<CsRegrou> newData = new List<CsRegrou>();
        List<CsRegrou> majData = new List<CsRegrou>();

        private string TableName;
        private string LibelleEtat = string.Empty;
        private string combobasename = "cbo_Produit";

        List<CsProduit> rowcomboselectedObject = new List<CsProduit>();
        List<DateTime?> rowselectDate = new List<DateTime?>();
        List<bool> columnAmount = new List<bool>();

          /// <summary>
          /// PERMET DE VALORISER LES DATAGRID ET   
          /// LES COMBOBOX AU CHARGEMENT DE LA PAGE
          /// </summary>
          /// <param name="datagrid"></param>
          /// <param name="services"></param>

       void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void Charger(int nummtab)
        {
            // TODO : cette ligne de code charge les onnées dans la table 'taGeneDataSet.TA'. Vous pouvez la déplacer ou la supprimer selon vos besoins.
            try
            {
                this.GetData(nummtab);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                //MessageBox.Show(ex.Message, FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void GetData(int numtable)
        {
            try
            {
                // Initialisation variables globales 

                produits.Clear(); 
                donnesDatagrid.Clear(); 
                newData.Clear(); 
                majData.Clear(); 
                rowcomboselectedObject.Clear(); 
                rowselectDate.Clear(); 
                columnAmount.Clear(); 

                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
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
                        produits.Clear();
                        produits.AddRange(args.Result);

                        cbo_refproducts.ItemsSource = produits;

                        ParametrageClient proxy = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                        proxy.SelectAll_REGROUCompleted += (s1, args1) =>
                            {
                                if (args1.Cancelled || args1.Error != null)
                                {
                                    string error = args1.Error.Message;
                                    MessageBox.Show(error, ".SelectInitTableData", MessageBoxButton.OK);
                                    desableProgressBar();
                                    this.DialogResult = true;
                                    return;
                                }

                                if (args1.Result == null || args1.Result.Count == 0)
                                {
                                    MessageBox.Show("No data found ", ".SelectInitTableData", MessageBoxButton.OK);
                                    desableProgressBar();
                                    //this.DialogResult = true;
                                    return;
                                }

                                donnesDatagrid.Clear();
                                dgINIT.ItemsSource = null;
                                donnesDatagrid.AddRange(args1.Result);
                                dgINIT.ItemsSource = donnesDatagrid;


                                // peupler la liste des nom de tables 

                                try
                                {
                                    //Initializer les element du tableau rowcomboselectedObject
                                    //Initializer les element du tableau columAmount
                                    foreach (CsRegrou tag in donnesDatagrid)
                                    {
                                        CsProduit prod = produits.FirstOrDefault(p=> p.CODE == tag.PRODUIT );
                                        rowcomboselectedObject.Add(prod);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string error = ex.Message;
                                }
                            };
                        proxy.SelectAll_REGROUAsync();

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
                //aTa u = e.Row.DataContext as aTa; //fetch the row data

                //if (majData.Count > 0 && majData != null)
                //{
                //    if (majData.First(p => p.CENTRE == u.CENTRE && p.ROWID == u.ROWID && u.CODE == p.CODE) != null)
                //    {
                //        foreach (aTa t in majData)
                //        {
                //            if (t.CODE == u.CODE && t.ROWID == u.ROWID)
                //            {
                //                t.CENTRE = u.CENTRE;
                //                t.CODE = u.CODE;
                //                t.LIBELLE = u.LIBELLE;
                //                t.NUM = u.NUM;
                //                t.DMAJ = DateTime.Now;
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
           

            //try
            //{
            //    e.Row.Background = new SolidColorBrush(Colors.Green);
            //}
            //catch (Exception ex)
            //{
            //    string error = ex.Message;
            //}

          //aCampagne u = e.Row.DataContext as aCampagne; //fetch the row data
          //dataTableDatagrid[e.Row.GetIndex()].IndexE = u.IndexE;
          //dataTableDatagrid[e.Row.GetIndex()].IndexO = u.IndexO;
          //dataTableDatagrid[e.Row.GetIndex()].DateCoupure = u.DateCoupure;
          //btnOk.IsEnabled = true;

        }

        void btnAddRow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtLibelle.Text) || string.IsNullOrEmpty(txtcentre.Text) ||
                    string.IsNullOrEmpty(txtCode.Text) || cbo_refproducts.SelectedItem == null)
                {
                    MessageBox.Show("All fiedls are required before inserting");
                    return;
                }

                CsRegrou add = new CsRegrou()
                {
                    CENTRE = txtcentre.Text,
                    PRODUIT = (cbo_refproducts.SelectedItem as CsProduit).CODE ,
                    REGROU = txtCode.Text,
                    DMAJ = DateTime.Now,
                    NOM = txtLibelle.Text
                };

                newData.Add(add);
                dgINIT.ItemsSource = null;
                List<CsRegrou> oldCsRues = new List<CsRegrou>();
                oldCsRues.AddRange(donnesDatagrid);
                donnesDatagrid.Clear();
                donnesDatagrid.Add(add);
                donnesDatagrid.AddRange(oldCsRues);

                rowcomboselectedObject.Clear();

                foreach (CsRegrou tag in donnesDatagrid)
                {
                    CsProduit produit = produits.FirstOrDefault(p => p.CODE == tag.PRODUIT);
                    rowcomboselectedObject.Add(produit);
                }

                dgINIT.ItemsSource = donnesDatagrid;

                resetInsertData();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        void resetInsertData()
        {
            txtcentre.Text = txtCode.Text = txtLibelle.Text = string.Empty ;
            cbo_refproducts.ItemsSource = null;
            cbo_refproducts.ItemsSource = produits;

        }

        void button1_Click(object sender, RoutedEventArgs e)
        {
            dgINIT.UpdateLayout();
            ParametrageClient cls = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            cls.insertOrUpdateREGROUCompleted += (ss, resut) =>
                {
                    if (resut.Cancelled || resut.Error != null)
                    {
                        string error = resut.Error.Message;
                        MessageBox.Show(error, "insertOrUpdateREGROU", MessageBoxButton.OK);
                        desableProgressBar();
                        return;
                    }

                    if (resut.Result == false)

                    {
                        MessageBox.Show("Error on insert ", "insertOrUpdateREGROU", MessageBoxButton.OK);
                        desableProgressBar();
                        return;
                    }

                    GetData(num);
                };
            cls.insertOrUpdateREGROUAsync(newData, majData);
        }

        void dgINIT_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                // initialiser les property and data des diff combobox des lignes
                CsProduit init = null;
                ComboBox combo = this.dgINIT.Columns[1].GetCellContent(e.Row) as ComboBox;
                combo.ItemsSource = produits;
                combo.Name = "cbo_Produit" + e.Row.GetIndex().ToString();
                if(e.Row.GetIndex() ==0)
                    init = rowcomboselectedObject[e.Row.GetIndex()];
                combo.SelectedItem = rowcomboselectedObject[e.Row.GetIndex()];
                //this.dgINIT.SelectedIndex = e.Row.GetIndex();

                //mettre en lecture seule la colonne conteneur des template combobox pour eviter 
                // leur reinitialisation a chaque scrolling

                //DataGridColumn column1 = this.dgINIT.Columns[1];
                //column1.IsReadOnly = true; // Works


                DatePicker dataColumn = this.dgINIT.Columns[4].GetCellContent(e.Row) as DatePicker;
                dataColumn.Name = "dtpMaj" + e.Row.GetIndex().ToString();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        void dgINIT_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {

            CsRegrou u = e.Row.DataContext as CsRegrou; //fetch the row data
            u.DMAJ = DateTime.Now;
            donnesDatagrid[e.Row.GetIndex()] = u;
            donnesDatagrid[e.Row.GetIndex()].ROWID = u.ROWID;

            btnOk.IsEnabled = true;
            btnOk.Visibility = System.Windows.Visibility.Visible;

            try
            {
                if (majData.Count > 0 && majData != null)
                {
                    if (majData.First(p => p.ROWID == u.ROWID) != null)
                    {
                        foreach (CsRegrou t in majData)
                        {
                            if (t.ROWID == u.ROWID)
                            {
                                t.CENTRE = u.CENTRE;
                                t.REGROU = u.REGROU;
                                t.DMAJ = DateTime.Now;
                                t.NOM = u.NOM;
                            }
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

        void dtpMaj_CalendarClosed(object sender, RoutedEventArgs e)
        {
            try
            {
                var src = sender as DatePicker;
                string comboxselected = string.Empty;
                CsRegrou selectedObject = new CsRegrou();
                if (src != null)
                {
                    string rank = src.Name.Substring(src.Name.Length - 1);
                    rowselectDate[int.Parse(rank)] = src.SelectedDate;

                    donnesDatagrid[int.Parse(rank)].DMAJ = src.SelectedDate.Value;
                   // btnOk.Visibility = System.Windows.Visibility.Visible;

                    CsRegrou u = donnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.First(p => p.CENTRE == u.CENTRE && p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsRegrou t in majData)
                            {
                                if (t.ROWID == u.ROWID)
                                    t.DMAJ = u.DMAJ;
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

        void btnReset_Click(object sender, RoutedEventArgs e)
        {
            resetInsertData();
        }

        void DataGridDoubleClickBehavior_Click(object sender, MouseButtonEventArgs e)
        {
            if (dgINIT.SelectedItem != null)
            {
                CsInit initselected = dgINIT.SelectedItem as CsInit;
            }
        }

        //SCROLL DATAGRID TO CERTAIN ITEM
        void DataGridScrollToItem(DataGrid theGrid, Object item)
        {
            try
            {
                //This will force the datagrid to load all data on the grid now
                theGrid.UpdateLayout();
                //Scroll the datagrid to the item postion
                theGrid.ScrollIntoView(item, null);
            }
            catch (Exception ex)
            {

                string error = ex.Message;
            }
           
        }

        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Voulez vous vraiment supprimer cet element","Confirmation",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                CsRegrou selected = dgINIT.SelectedItem as CsRegrou;
                if (selected  != null)
                {
                    ParametrageClient delete = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                    delete.Delete_REGROUCompleted += (del, argDel) =>
                        {
                            if (argDel.Cancelled || argDel.Error != null)
                            {
                                string error = argDel.Error.Message;
                                MessageBox.Show(error, "Delete_REGROU", MessageBoxButton.OK);
                                desableProgressBar();
                                return;
                            }

                            if (argDel.Result == false)
                            {
                                MessageBox.Show("Error on insert/update ", "Delete_REGROU", MessageBoxButton.OK);
                                desableProgressBar();
                                return;
                            }

                            int rank = dgINIT.SelectedIndex;
                            donnesDatagrid.RemoveAt(rank);

                            dgINIT.ItemsSource = null;
                            columnAmount.Clear();
                            rowcomboselectedObject.Clear();


                            foreach (CsRegrou tag in donnesDatagrid)
                            {
                                CsProduit prod = produits.FirstOrDefault(p => p.CODE == tag.PRODUIT);
                                rowcomboselectedObject.Add(prod);
                            }

                            dgINIT.ItemsSource = donnesDatagrid;

                            // verifier si l'element supprime est in item des liste NewAdded ou UpdatedList
                            // checkInlistItem(selected);
                        };
                    delete.Delete_REGROUAsync(selected.CENTRE,selected.REGROU, selected.PRODUIT);
                }
            }
        }

        void checkInlistItem(CsRegrou selected)
        {
            try
            {
                if(newData.Contains(selected))
                   newData.Remove(selected);
            
                 if(majData.Contains(selected))
                   majData.Remove(selected);
             }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
         

        }


        /// <summary>
        /// Permet de changeer la valeur de la propriete produit dans la liste des elements 
        /// a inserer .
        /// PROBLMENE : a l'ajout de l'object dans la newData list ,la propriete PRODUIT est a 1 ou 3,mais apres cett valeur prend le libelle du produit
        /// (water ou electricitt) ??????????????????
        /// </summary>
        /// <param name="listes"></param>
        void ChangenProduitInCsinitList(List<CsInit> listes)
        {
            foreach (CsInit csinit in listes)
            {
                CsProduit prod = produits.FirstOrDefault(p => p.LIBELLE == csinit.PRODUIT);
                csinit.PRODUIT = prod.CODE ;
            }
        }

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

                    string rank = src.Name.Substring(combobasename.Length);
                    rowcomboselectedObject[int.Parse(rank)] = selectedObject;

                    donnesDatagrid[int.Parse(rank)].PRODUIT = comboxselected;

                    CsRegrou u = donnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.First(p => p.CENTRE == u.CENTRE && p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsRegrou t in majData)
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
        
    }
}



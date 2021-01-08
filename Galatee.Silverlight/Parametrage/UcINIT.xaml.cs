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
    public partial class UcINIT : ChildWindow
    {
        public UcINIT()
        {
            InitializeComponent();
        }

        public UcINIT(int numero, string text, string TableNom, List<CsInit> datagrid, List<CsProduit> services, List<CsZone> zones)
        {
            InitializeComponent();
            num = numero;
            this.Title = text;
            this.TableName = TableNom;

            zonesFromDB.AddRange(zones);
            InitialiserValue(datagrid, services,zones);
           // this.GetData(numero);
        }

        int num;
        int rowIndex = -1;
        int rankSelected;
        int atteintchange = 0;


        List<CsProduit> produits = new List<CsProduit>();
        List<CsZone> zonesFromDB = new List<CsZone>();
        List<CsInit> donnesDatagrid = new List<CsInit>();
        List<CsInit> newData = new List<CsInit>();
        List<CsInit> majData = new List<CsInit>();
        List<CsInit> tableNames = new List<CsInit>();
        List<string> tableColumName = new List<string>();

        private string TableName;
        private string LibelleEtat = string.Empty;
        private string checkbasename = "chkAmount";
        private string combobasename = "cbo_Produit";

        private bool EstNumerique = false;
        private bool CentreEstNumerique = false;
        private bool iscrollinng = false;

        List<CsProduit> rowcomboselectedObject = new List<CsProduit>();
        List<DateTime?> rowselectDate = new List<DateTime?>();
        List<bool> columnAmount = new List<bool>();

          /// <summary>
          /// PERMET DE VALORISER LES DATAGRID ET   
          /// LES COMBOBOX AU CHARGEMENT DE LA PAGE
          /// </summary>
          /// <param name="datagrid"></param>
          /// <param name="services"></param>

        void InitialiserValue(List<CsInit> datagrid, List<CsProduit> services, List<CsZone> zones)
                 {
                     produits.Clear();
                     rowcomboselectedObject.Clear();
                     zonesFromDB.Clear();
                     donnesDatagrid.Clear();
                     newData.Clear();
                     majData.Clear();
                     tableNames.Clear();
                     tableColumName.Clear();
                     columnAmount.Clear();         

                     produits.AddRange(services);
                     cbo_products.ItemsSource = produits;

                     cbo_refZones.ItemsSource = zones;
                     //CsInit emptyplace2 = new CsInit() { 
                     // LIBELLE= string.Empty,
                     // NTABLE = string.Empty
                     //};

                     donnesDatagrid.Clear();
                     dgINIT.ItemsSource = null;
                     donnesDatagrid.AddRange(datagrid);
                     dgINIT.ItemsSource = donnesDatagrid;

                     DataGridScrollToItem(dgINIT, donnesDatagrid[donnesDatagrid.Count-1]);
                     // peupler la liste des nom de tables 
                     IEnumerable<CsInit> distinclistes = donnesDatagrid.GroupBy(cust => cust.NTABLE).Select(grp => grp.First());

                     foreach (CsInit init in distinclistes)
                         tableNames.Add(init);

                     // peupler la combobox des nom de tables 

                     cbo_refTable.ItemsSource = tableNames;
                     cbo_refTables.ItemsSource = tableNames;

                     try
                     {
                         //rowcomboselectedObject = new CsProduit[donnesDatagrid.Count];
                         //rowselectDate = new DateTime?[donnesDatagrid.Count];
                         //columnAmount = new bool[donnesDatagrid.Count];
                         int inc = 0;

                         //Initializer les element du tableau rowcomboselectedObject
                         //Initializer les element du tableau columAmount

                         foreach (CsInit tag in donnesDatagrid)
                         {
                             bool bools= tag.OBLIG == "O" ? true : false;
                             columnAmount.Add(bools);
                             CsProduit prod = produits.FirstOrDefault(p => p.CODE == tag.PRODUIT);
                             rowcomboselectedObject.Add(prod);
                             inc++;
                         }
                     }
                     catch (Exception ex)
                     {
                         string error = ex.Message;
                     }
                 }

        void cbo_Observation_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (loadOnlyOne <= dataTableCombo.Count)
                //{
                //    var src = sender as ComboBox;
                //    if (src != null)
                //    {
                //        var data = src.DataContext as aCampagne;
                //        switch (src.Name)
                //        {
                //            case "cbo_Observation":
                //                src.ItemsSource = dataTableCombo;
                //                src.SelectedValue = "Code";
                //                src.DisplayMemberPath = "Libelle";
                //                loadOnlyOne++;
                //                break;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

        }

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
                        produits.Clear();
                        produits.AddRange(args.Result);

                        cbo_products.ItemsSource = produits;

                        ParametrageClient proxy = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                        proxy.SelectInitTableDataCompleted += (s1, args1) =>
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
                                    this.DialogResult = true;
                                    return;
                                }

                                CsInit emptyplace2 = new CsInit();
                                donnesDatagrid.Clear();
                                dgINIT.ItemsSource = null;
                                donnesDatagrid.AddRange(args1.Result);
                                donnesDatagrid.Add(emptyplace2);
                                dgINIT.ItemsSource = donnesDatagrid;


                                // peupler la liste des nom de tables 
                                IEnumerable<CsInit> distinclistes = donnesDatagrid.GroupBy(cust => cust.NTABLE).Select(grp => grp.First());

                                foreach (CsInit init in distinclistes)
                                    tableNames.Add(init);

                                // peupler la combobox des nom de tables 

                                cbo_refTable.ItemsSource = tableNames;

                                try
                                {
                                    //rowcomboselectedObject = new CsProduit[donnesDatagrid.Count];
                                    //rowselectDate = new DateTime?[donnesDatagrid.Count];
                                    //columnAmount = new bool[donnesDatagrid.Count];
                                    int inc = 0;

                                    //Initializer les element du tableau rowcomboselectedObject
                                    //Initializer les element du tableau columAmount
                                    foreach (CsInit tag in donnesDatagrid)
                                    {
                                        columnAmount[inc]= tag.OBLIG == "O" ? true : false;
                                        CsProduit prod = produits.FirstOrDefault(p => p.CODE == tag.PRODUIT);
                                        rowcomboselectedObject[inc] = prod;
                                        inc++;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string error = ex.Message;
                                }
                            };
                        proxy.SelectInitTableDataAsync();

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

        void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void btnAddRow_Click(object sender, RoutedEventArgs e)
        {
              try
                {
                    if (string.IsNullOrEmpty(txtAmount.Text) || string.IsNullOrEmpty(txtcentre.Text) || string.IsNullOrEmpty(txtDefValeusss.Text) ||
                     (cbo_refTables.SelectedItem as CsInit) == null || cbo_refZones.SelectedItem == null)
                     {
                        MessageBox.Show("All fiedls are required before inserting");
                        return;
                     }

                    CsInit add = new CsInit()
                    {
                        CONTENU = txtDefValeusss.Text,
                        OBLIG= txtAmount.Text,
                        PRODUIT = (cbo_products.SelectedItem as CsProduit).CODE,
                        DMAJ = DateTime.Now,
                        CENTRE = txtcentre.Text,
                        NTABLE= cbo_refTables.SelectedValue.ToString().Trim(),
                        ZONE = (cbo_refZones.SelectedItem as CsZone).Code
                    };

                    newData.Add(add);
                    dgINIT.ItemsSource = null;
                    List<CsInit> oldCsinit = new List<CsInit>();
                    oldCsinit.AddRange(donnesDatagrid);
                    donnesDatagrid.Clear();
                    donnesDatagrid.Add(add);
                    donnesDatagrid.AddRange(oldCsinit);

                    int inc = 0;
                    rowcomboselectedObject.Clear();
                    columnAmount.Clear();

                    foreach (CsInit tag in donnesDatagrid)
                    {
                        columnAmount.Add(tag.OBLIG == "O" ? true : false);
                        CsProduit prod = produits.FirstOrDefault(p => p.LIBELLE == tag.PRODUIT || p.CODE == tag.PRODUIT);
                        rowcomboselectedObject.Add(prod);
                        inc++;
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
            txtcentre.Text = txtAmount.Text = txtDefValeusss.Text = string.Empty ;
            cbo_products.ItemsSource = null;
            cbo_products.ItemsSource = produits;

            cbo_refTables.ItemsSource = null;
            cbo_refZones.ItemsSource = null;
            cbo_refTables.ItemsSource = tableNames;
         

        }

        void cbo_Observation_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
               

        }

        void button1_Click(object sender, RoutedEventArgs e)
        {
            ChangenProduitInCsinitList(newData);
            ChangenProduitInCsinitList(majData);
            ParametrageClient cls = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
            cls.insertOrUpdateInitCompleted += (ss, resut) =>
                {
                    if (resut.Cancelled || resut.Error != null)
                    {
                        string error = resut.Error.Message;
                        MessageBox.Show(error, "insertTA0", MessageBoxButton.OK);
                        desableProgressBar();
                        return;
                    }

                    if (resut.Result == false)
                    {
                        MessageBox.Show("Error on insert ", "insertTA0", MessageBoxButton.OK);
                        desableProgressBar();
                        return;
                    }

                    GetData(num);
                };
            cls.insertOrUpdateInitAsync(newData, majData);
        }

        void dgINIT_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                //if (atteintchange >= donnesDatagrid.Count )
                //      if(iscrollinng) 
                // initialiser les property and data des diff combobox des lignes
                CsProduit init = null;
                iscrollinng = true;
                ComboBox combo = this.dgINIT.Columns[1].GetCellContent(e.Row) as ComboBox;
                combo.ItemsSource = produits;
                combo.Name = "cbo_Produit" + e.Row.GetIndex().ToString();
                if(e.Row.GetIndex() ==0)
                    init = rowcomboselectedObject[e.Row.GetIndex()];
                combo.SelectedItem = rowcomboselectedObject[e.Row.GetIndex()];
                //this.dgINIT.SelectedIndex = e.Row.GetIndex();

                // cocher les checkbox au chargement des lignes

                //CsInit tag = new CsInit();
                //tag = e.Row.DataContext as CsInit;
                CheckBox chk = this.dgINIT.Columns[2].GetCellContent(e.Row) as CheckBox;
                chk.Name = "chkAmount" + e.Row.GetIndex().ToString();
                chk.IsChecked = columnAmount[e.Row.GetIndex()];

                //chk.IsChecked = tag.OBLIG == "O" ? true : false;

                //mettre en lecture seule la colonne conteneur des template combobox pour eviter 
                // leur reinitialisation a chaque scrolling

                DataGridColumn column1 = this.dgINIT.Columns[1];
                column1.IsReadOnly = true; // Works


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
            CsInit u = e.Row.DataContext as CsInit; //fetch the row data
            u.DMAJ = DateTime.Now;
            donnesDatagrid[e.Row.GetIndex()].CONTENU = u.CONTENU;
            btnOk.IsEnabled = true;
            btnOk.Visibility = System.Windows.Visibility.Visible;

            try
            {
                if (majData.Count > 0 && majData != null)
                {
                    if (majData.First(p => p.ROWID == u.ROWID) != null)
                    {
                        foreach (CsInit t in majData)
                        {
                            if (t.ROWID == u.ROWID)
                            {
                                t.CENTRE = u.CENTRE;
                                t.CONTENU = u.CONTENU;
                                t.LIBELLE = u.LIBELLE;
                                t.PRODUIT = u.PRODUIT;
                                t.OBLIG = u.OBLIG;
                                t.DMAJ = DateTime.Now;
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
                CTab300 selectedObject = new CTab300();
                if (src != null)
                {
                    string rank = src.Name.Substring(src.Name.Length - 1);
                    rowselectDate[int.Parse(rank)] = src.SelectedDate;

                    donnesDatagrid[int.Parse(rank)].DMAJ = src.SelectedDate;
                   // btnOk.Visibility = System.Windows.Visibility.Visible;

                    CsInit u = donnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.First(p => p.CENTRE == u.CENTRE && p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsInit t in majData)
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
                cbo_refTable.SelectedItem = initselected;
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
                CsInit selected = dgINIT.SelectedItem as CsInit;
                if (selected  != null)
                {
                    ParametrageClient delete = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                    delete.DeleteINITCompleted += (del, argDel) =>
                        {
                            if (argDel.Cancelled || argDel.Error != null)
                            {
                                string error = argDel.Error.Message;
                                MessageBox.Show(error, "DeleteINIT", MessageBoxButton.OK);
                                desableProgressBar();
                                return;
                            }

                            if (argDel.Result == false)
                            {
                                MessageBox.Show("Error on insert/update ", "DeleteINIT", MessageBoxButton.OK);
                                desableProgressBar();
                                return;
                            }

                            int rank = dgINIT.SelectedIndex;
                            int inc = 0;
                            donnesDatagrid.RemoveAt(rank);

                            dgINIT.ItemsSource = null;
                            columnAmount.Clear();
                            rowcomboselectedObject.Clear();

                            //rowcomboselectedObject = new CsProduit[donnesDatagrid.Count];
                            //rowselectDate = new DateTime?[donnesDatagrid.Count];
                            //columnAmount = new bool[donnesDatagrid.Count];

                            foreach (CsInit tag in donnesDatagrid)
                            {
                                columnAmount.Add(tag.OBLIG == "O" ? true : false);
                                CsProduit prod = produits.FirstOrDefault(p => p.LIBELLE == tag.PRODUIT);
                                rowcomboselectedObject.Add(prod);
                            }

                            dgINIT.ItemsSource = donnesDatagrid;

                            // verifier si l'element supprime est in item des liste NewAdded ou UpdatedList
                            // checkInlistItem(selected);
                        };
                    delete.DeleteINITAsync(selected.CENTRE, selected.PRODUIT, selected.NTABLE, selected.ZONE);
                }
            }
        }

        void checkInlistItem(CsInit selected)
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

        void dgINIT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CsInit itemselected= dgINIT.SelectedItem as CsInit;
            cbo_refTable.SelectedItem= itemselected;
        }

        void cbo_refTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ParametrageClient clients = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                clients.SELECT_INIT_SELECT_COLUMNS_BY_NTABLECompleted += (send, es) =>
                {
                    List<CsZone> zoones = new List<CsZone>();
                    zoones.AddRange(es.Result);
                    cbo_zone.ItemsSource = null;
                    cbo_zone.ItemsSource = zoones;
                    cbo_zone.SelectedItem = zoones[2];
                };
                clients.SELECT_INIT_SELECT_COLUMNS_BY_NTABLEAsync(int.Parse((cbo_refTable.SelectedItem as CsInit).NTABLE));
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
          
        }

        void cbo_refTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //CsInit tableselected = (cbo_refTables.SelectedItem as CsInit);
                //List<cZone> zoones = (from p in zonesFromDB
                //                      select p).Where(p => p.Table == tableselected.LIBELLE).ToList();
                ParametrageClient clients = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                clients.SELECT_INIT_SELECT_COLUMNS_BY_NTABLECompleted += (send, es) =>
                    {
                        List<CsZone> zoones = new List<CsZone>();
                        zoones.AddRange(es.Result);
                        cbo_refZones.ItemsSource = null;
                        cbo_refZones.SelectedValuePath = "Table";
                        cbo_refZones.DisplayMemberPath = "Libelle";
                        cbo_refZones.ItemsSource = zoones;
                        cbo_refZones.SelectedItem = zoones[2];
                    };
                clients.SELECT_INIT_SELECT_COLUMNS_BY_NTABLEAsync(int.Parse((cbo_refTables.SelectedItem as CsInit).NTABLE));
                
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
                try
                {
                    CsProduit prod = produits.FirstOrDefault(p => p.CODE == csinit.PRODUIT );
                    csinit.PRODUIT = prod.CODE;
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
               
            }
        }

        void bntCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void cbo_Observation_DragLeave(object sender, DragEventArgs e)
         {

        }

        private void cbo_products_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void cbo_Observation_DropDownClosed(object sender, EventArgs e)
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

                    CsInit u = donnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.FirstOrDefault(p => p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsInit t in majData)
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

        private void chkAmount_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void chkAmount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var src = sender as CheckBox;
                string obligation = src.IsChecked==true ? "O" : "N";
                string comboxselected = string.Empty;
                if (src != null)
                {
                    string rank = src.Name.Substring(checkbasename.Length);
                    columnAmount[int.Parse(rank)] = false;
                    donnesDatagrid[int.Parse(rank)].OBLIG = obligation;


                    CsInit u = donnesDatagrid[int.Parse(rank)];

                    // ajout des element modifies dans la liste des elements modifies

                    if (majData.Count > 0 && majData != null)
                    {
                        if (majData.First(p => p.CENTRE == u.CENTRE && p.ROWID == u.ROWID) != null)
                        {
                            foreach (CsInit t in majData)
                            {
                                t.OBLIG = u.OBLIG;
                                t.DMAJ = DateTime.Now;
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



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
using Galatee.Silverlight.Resources.Fraude;
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceFraude;
using System.Windows.Data;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;


namespace Galatee.Silverlight.Fraude
{
    public partial class UcAnnalyseDeConsommation : ChildWindow
    {
        CsDemandeFraude LaDemande = new CsDemandeFraude();
        int EtapeActuelle;
        public ObservableCollection<DefaultValue1> defaultValues1;
        public ObservableCollection<DefaultValue2> defaultValues2;
        public ObservableCollection<DefaultValue3> defaultValues3;
        public ObservableCollection<DefaultValue4> defaultValues4;
        public ObservableCollection<DefaultValue5> defaultValues5;
        DefaultValue1 tables1 = new DefaultValue1();
        List<DefaultValue1> listMoisGrid1 = new List<DefaultValue1>();
        List<DefaultValue2> listMoisGrid2 = new List<DefaultValue2>();
        List<DefaultValue3> listMoisGrid3 = new List<DefaultValue3>();
        List<DefaultValue4> listMoisGrid4 = new List<DefaultValue4>();
        List<DefaultValue5> listMoisGrid5 = new List<DefaultValue5>();
        public ObservableCollection<DefaultValue1> table = new ObservableCollection<DefaultValue1>();
        CsDemandeFraude listForInsertOrUpdate = new CsDemandeFraude();
        public UcAnnalyseDeConsommation()
        {
            InitializeComponent();
        }
        public UcAnnalyseDeConsommation(List<int> demande, int etape)
        {
            InitializeComponent();
            EtapeActuelle = etape;
            ChargeDonneDemande(demande.First());
            this.defaultValues1 = new ObservableCollection<DefaultValue1>();
            this.defaultValues1.Add(new DefaultValue1());

            this.defaultValues2 = new ObservableCollection<DefaultValue2>();
            this.defaultValues2.Add(new DefaultValue2());

            this.defaultValues3 = new ObservableCollection<DefaultValue3>();
            this.defaultValues3.Add(new DefaultValue3());
            this.defaultValues4 = new ObservableCollection<DefaultValue4>();
            this.defaultValues4.Add(new DefaultValue4());
            this.defaultValues5 = new ObservableCollection<DefaultValue5>();
            this.defaultValues5.Add(new DefaultValue5());
            this.dgvDerniereConsoPart1.ItemsSource = this.defaultValues1;
            this.dgvDerniereConsoPart2.ItemsSource = this.defaultValues2;
            //this.dgvDerniereConsoPart3.ItemsSource = this.defaultValues3;
            //this.dgvDerniereConsoPart4.ItemsSource = this.defaultValues4;
            //this.dgvDerniereConsoPart5.ItemsSource = this.defaultValues5;
            //txtConsommationDejaFacturee.Text = "0";
            if (this.nudNombreMois.Value == 0)
            {
                //dgvDerniereConsoPart3.Visibility = Visibility.Collapsed;
                //dgvDerniereConsoPart4.Visibility = Visibility.Collapsed;
                //dgvDerniereConsoPart5.Visibility = Visibility.Collapsed;
               this.nudNombreMois.Value = 12;
            }
           
          
        }

        public class DefaultValue1
        {
            public int Mois1 { get; set; }
            public int Mois2 { get; set; }
            public int Mois3 { get; set; }
            public int Mois4 { get; set; }
            public int Mois5 { get; set; }
            public int Mois6 { get; set; }
            public int Mois7 { get; set; }
            public int Mois8 { get; set; }
            public int Mois9 { get; set; }

           
        }
        public class DefaultValue2
        {
            public int Mois1 { get; set; }
            public int Mois2 { get; set; }
            public int Mois3 { get; set; }
            public int Mois4 { get; set; }
            public int Mois5 { get; set; }
            public int Mois6 { get; set; }
            public int Mois7 { get; set; }
            public int Mois8 { get; set; }
            public int Mois9 { get; set; }
        }
        public class DefaultValue3
        {
            public int Mois1 { get; set; }
            public int Mois2 { get; set; }
            public int Mois3 { get; set; }
            public int Mois4 { get; set; }
            public int Mois5 { get; set; }
            public int Mois6 { get; set; }
            public int Mois7 { get; set; }
            public int Mois8 { get; set; }
            public int Mois9 { get; set; }
        }
        public class DefaultValue4
        {
            public int Mois1 { get; set; }
            public int Mois2 { get; set; }
            public int Mois3 { get; set; }
            public int Mois4 { get; set; }
            public int Mois5 { get; set; }
            public int Mois6 { get; set; }
            public int Mois7 { get; set; }
            public int Mois8 { get; set; }
            public int Mois9 { get; set; }
        }
        public class DefaultValue5
        {
            public int Mois1 { get; set; }
            public int Mois2 { get; set; }
            public int Mois3 { get; set; }
            public int Mois4 { get; set; }
            public int Mois5 { get; set; }
            public int Mois6 { get; set; }
            public int Mois7 { get; set; }
            public int Mois8 { get; set; }
            public int Mois9 { get; set; }
        }
       private void Recupere(CsDemandeFraude LaDemande)
        {
            LaDemande.MoisDejaFactures = new List<CsMoisDejaFactures>();
            LaDemande.MoisDejaFactures.Clear();
            LaDemande.ConsommationFrd = new CsConsommationFrd();
            //listMoisGrid1.Clear();
            listMoisGrid1 = (List<DefaultValue1>) dgvDerniereConsoPart1.ItemsSource ;

            if (listMoisGrid1 != null && listMoisGrid1.Count > 0)
            {
                foreach (DefaultValue1 Item in listMoisGrid1)
                {
                    int i = 0;
                    if (Item != null)
                    {
                        foreach (var item_ in Item.GetType().GetProperties())
                        {
                            i++;
                            var ValeurObject = GetPropValue(Item, item_.Name);
                            string ValeurString = string.Empty;

                            if (ValeurObject != null)
                            {
                                ValeurString = ValeurObject.ToString();
                            }
                            if (!string.IsNullOrWhiteSpace(ValeurString))
                            {
                                int Conso = 0;
                                int.TryParse(ValeurString, out Conso);
                                if (Conso != 0)
                                {
                                    var mois = new CsMoisDejaFactures
                                               {
                                                   ConsoDejaFacturee = Conso,
                                                   OrdreMois = i,
                                                   FK_IDPRODUIT = (int)LaDemande.CompteurFraude.FK_IDPRODUIT
                                               };
                                    LaDemande.MoisDejaFactures.Add(mois);
                                }
                            }
                        }
                    }
                }
            }

          /****/
           // listMoisGrid2.Clear();
            listMoisGrid2 = (List<DefaultValue2>)dgvDerniereConsoPart2.ItemsSource;
            if (listMoisGrid2 != null && listMoisGrid2.Count > 0)
            {
                foreach (DefaultValue2 Item in listMoisGrid2)
                {
                    int i = 0;
                    if (Item != null)
                    {
                        foreach (var item_ in Item.GetType().GetProperties())
                        {
                            i++;
                            var ValeurObject = GetPropValue(Item, item_.Name);
                            string ValeurString = string.Empty;

                            if (ValeurObject != null)
                            {
                                ValeurString = ValeurObject.ToString();
                            }
                            if (!string.IsNullOrWhiteSpace(ValeurString))
                            {
                                int Conso = 0;
                                int.TryParse(ValeurString, out Conso);
                                if (Conso != 0)
                                {
                                    var mois = new CsMoisDejaFactures
                                    {
                                        ConsoDejaFacturee = Conso,
                                        OrdreMois = i,
                                        FK_IDPRODUIT = (int)LaDemande.CompteurFraude.FK_IDPRODUIT
                                    };
                                    LaDemande.MoisDejaFactures.Add(mois);
                                }
                            }
                        }
                    }
                }
            }
           /*****/
          //  listMoisGrid3.Clear();
            listMoisGrid3 = (List<DefaultValue3>)dgvDerniereConsoPart3.ItemsSource;
            if (listMoisGrid3 != null && listMoisGrid3.Count > 0)
            {
                foreach (DefaultValue3 Item in listMoisGrid3)
                {
                    int i = 0;
                    if (Item != null)
                    {
                        foreach (var item_ in Item.GetType().GetProperties())
                        {
                            i++;
                            var ValeurObject = GetPropValue(Item, item_.Name);
                            string ValeurString = string.Empty;

                            if (ValeurObject != null)
                            {
                                ValeurString = ValeurObject.ToString();
                            }
                            if (!string.IsNullOrWhiteSpace(ValeurString))
                            {
                                int Conso = 0;
                                int.TryParse(ValeurString, out Conso);
                                if (Conso != 0)
                                {
                                    var mois = new CsMoisDejaFactures
                                    {
                                        ConsoDejaFacturee = Conso,
                                        OrdreMois = i,
                                        FK_IDPRODUIT = (int)LaDemande.CompteurFraude.FK_IDPRODUIT
                                    };
                                    LaDemande.MoisDejaFactures.Add(mois);
                                }
                            }
                        }
                    }
                }
            }
            /*****/
          //  listMoisGrid4.Clear();
          listMoisGrid4.Add(dgvDerniereConsoPart4.SelectedItem as DefaultValue4);
            if (listMoisGrid4 != null && listMoisGrid4.Count > 0)
            {
                foreach (DefaultValue4 Item in listMoisGrid4)
                {
                    if (Item != null)
                    {
                        int i = 0;
                        foreach (var item_ in Item.GetType().GetProperties())
                        {
                            i++;
                            var ValeurObject = GetPropValue(Item, item_.Name);
                            string ValeurString = string.Empty;

                            if (ValeurObject != null)
                            {
                                ValeurString = ValeurObject.ToString();
                            }
                            if (!string.IsNullOrWhiteSpace(ValeurString))
                            {
                                int Conso = 0;
                                int.TryParse(ValeurString, out Conso);
                                if (Conso != 0)
                                {
                                    var mois = new CsMoisDejaFactures
                                    {
                                        ConsoDejaFacturee = Conso,
                                        OrdreMois = i,
                                        FK_IDPRODUIT = (int)LaDemande.CompteurFraude.FK_IDPRODUIT
                                    };
                                    LaDemande.MoisDejaFactures.Add(mois);
                                }
                            }
                        }
                    }
                }
            }
            /*****/
            listMoisGrid5.Clear();
           // listMoisGrid5.Add(dgvDerniereConsoPart5.SelectedItem as DefaultValue5);
            if (listMoisGrid5 != null && listMoisGrid5.Count > 0)
            {
                foreach (DefaultValue5 Item in listMoisGrid5)
                {
                    int i = 0;
                    if (Item != null)
                    {
                        foreach (var item_ in Item.GetType().GetProperties())
                        {
                            i++;
                            var ValeurObject = GetPropValue(Item, item_.Name);
                            string ValeurString = string.Empty;

                            if (ValeurObject != null)
                            {
                                ValeurString = ValeurObject.ToString();
                            }
                            if (!string.IsNullOrWhiteSpace(ValeurString))
                            {
                                int Conso = 0;
                                int.TryParse(ValeurString, out Conso);
                                if (Conso != 0)
                                {
                                    var mois = new CsMoisDejaFactures
                                    {
                                        ConsoDejaFacturee = Conso,
                                        OrdreMois = i,
                                        FK_IDPRODUIT = (int)LaDemande.CompteurFraude.FK_IDPRODUIT
                                    };
                                    LaDemande.MoisDejaFactures.Add(mois);
                                }
                            }
                        }
                    }
                }
            }
           LaDemande.ConsommationFrd.FK_IDFRAUDE=LaDemande.Fraude.Pk_ID;
           LaDemande.ConsommationFrd.FK_IDPRODUIT = (int)LaDemande.CompteurFraude.FK_IDPRODUIT;
           LaDemande.ConsommationFrd.OrdreTraitement = (byte)(10);
           LaDemande.ConsommationFrd.MontantFactureTTC = 0;
           LaDemande.ConsommationFrd.MontantHTConsommation = 0;
           LaDemande.ConsommationFrd.MontantTVAConsommation = 0;
           LaDemande.ConsommationFrd.MontantHTPrestationEDM = 0;
           LaDemande.ConsommationFrd.MontantHTPrestationRemboursable = 0;
           LaDemande.ConsommationFrd.MontantHTRegularisationDevis = 0;





        }
    
       public static object GetPropValue(object src, string propName)
       {
           return src.GetType().GetProperty(propName).GetValue(src, null);
       }


        void OKButton_Click(object sender, RoutedEventArgs e)
            {
                Recupere(LaDemande);
                UcAnalyse Newfrm = new UcAnalyse(LaDemande, EtapeActuelle);
                Newfrm.Show();
                DialogResult = true;
            }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void NumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)     
        {

            int nbreMois;
            //DataGridViewCellStyle st = new DataGridViewCellStyle();
            //st.BackColor = System.Drawing.SystemColors.Window;
            //st.NullValue = "0";
            if (int.TryParse(this.nudNombreMois.Value.ToString(), out nbreMois))
            {
                if (nbreMois == 0)
                {
                    foreach (DataGridColumn item in dgvDerniereConsoPart1.Columns)
                    {
                        //if(column name condition of column id)
                        item.IsReadOnly = true;
                    }
                    return;
                }
            }

            if ((nbreMois > 0) && (nbreMois <= 9))
            {
                //Mettre les cellules concernées de la ligne 1 en modification
                for (int i = 0; i < nbreMois; i++)
                {
                    ////this.dgvDerniereConsoPart1.Ro[i].( "0");
                    ////  this.dgvDerniereConsoPart1.RowEdit[0].Cells[i].Value = 0;
                    this.dgvDerniereConsoPart1.Columns[i].IsReadOnly = false;

                }

                //Mettre les autres cellules non concernées de la ligne 1 en consultation
                for (int i = nbreMois; i < this.dgvDerniereConsoPart1.Columns.Count; i++)
                {
                    
                    this.dgvDerniereConsoPart1.Columns[i].IsReadOnly = false;
                }


                ////Mettre les cellules de la ligne 2 en consultation
                for (int i = 0; i < this.dgvDerniereConsoPart2.Columns.Count; i++)
                {
                    //this.dgvDerniereConsoPart2.Columns[i].DefaultCellStyle.BackColor = Color.;
                    //  this.dgvDerniereConsoPart2.RowEditEnded[0].Cells[i].Value = 0;
                    this.dgvDerniereConsoPart2.Columns[i].IsReadOnly = true;
                }
                for (int i = 0; i < this.dgvDerniereConsoPart3.Columns.Count; i++)
                {
                    //this.dgvDerniereConsoPart2.Columns[i].DefaultCellStyle.BackColor = Color.;
                    //  this.dgvDerniereConsoPart2.RowEditEnded[0].Cells[i].Value = 0;
                    this.dgvDerniereConsoPart3.Columns[i].IsReadOnly = true;
                }
                for (int i = 0; i < this.dgvDerniereConsoPart4.Columns.Count; i++)
                {
                    //this.dgvDerniereConsoPart2.Columns[i].DefaultCellStyle.BackColor = Color.;
                    //  this.dgvDerniereConsoPart2.RowEditEnded[0].Cells[i].Value = 0;
                    this.dgvDerniereConsoPart4.Columns[i].IsReadOnly = true;
                }
                //for (int i = 0; i < this.dgvDerniereConsoPart5.Columns.Count; i++)
                //{
                //    //this.dgvDerniereConsoPart2.Columns[i].DefaultCellStyle.BackColor = Color.;
                //    //  this.dgvDerniereConsoPart2.RowEditEnded[0].Cells[i].Value = 0;
                //    this.dgvDerniereConsoPart5.Columns[i].IsReadOnly = true;
                //}
                dgvDerniereConsoPart2.Visibility = Visibility.Collapsed;
                dgvDerniereConsoPart3.Visibility = Visibility.Collapsed;
                dgvDerniereConsoPart4.Visibility = Visibility.Collapsed;
                //dgvDerniereConsoPart5.Visibility = Visibility.Collapsed;

            }
            else if ((nbreMois > 9) && (nbreMois <= 18))
            {
                //Mettre les cellules de la ligne 1 en modification
                for (int i = 0; i < this.dgvDerniereConsoPart1.Columns.Count; i++)
                {
                    //this.dgvDerniereConsoPart1.Columns[i].DefaultCellStyle = st;
                    this.dgvDerniereConsoPart1.Columns[i].IsReadOnly = false;
                }

                //Mettre les cellules concernées de la ligne 2 en modification
                for (int i = 0; i < nbreMois - 9; i++)
                {
                    //this.dgvDerniereConsoPart2.Columns[i].DefaultCellStyle = st;
                    this.dgvDerniereConsoPart2.Columns[i].IsReadOnly = false;
                }

                //Mettre les autres cellules non concernées de la ligne 2 en consultation
                for (int i = nbreMois - 9; i < this.dgvDerniereConsoPart2.Columns.Count; i++)
                {
                    //this.dgvDerniereConsoPart2.Columns[i].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    //this.dgvDerniereConsoPart2.B[0].Cells[i].Value = 0;
                    this.dgvDerniereConsoPart2.Columns[i].IsReadOnly = true;
                }

                //Mettre les cellules de la ligne 3 en consultation
                for (int i = 0; i < this.dgvDerniereConsoPart3.Columns.Count; i++)
                {

                    this.dgvDerniereConsoPart3.Columns[i].IsReadOnly = true;
                }

                //Mettre les cellules de la ligne 4 en consultation
                for (int i = 0; i < this.dgvDerniereConsoPart4.Columns.Count; i++)
                {

                    this.dgvDerniereConsoPart4.Columns[i].IsReadOnly = true;
                }

                ////Mettre les cellules de la ligne 5 en consultation
                //for (int i = 0; i < this.dgvDerniereConsoPart5.Columns.Count; i++)
                //{

                //    this.dgvDerniereConsoPart5.Columns[i].IsReadOnly = true;
                //}
                dgvDerniereConsoPart2.Visibility = Visibility.Visible;
                dgvDerniereConsoPart3.Visibility = Visibility.Collapsed;
                dgvDerniereConsoPart4.Visibility = Visibility.Collapsed;
                //dgvDerniereConsoPart5.Visibility = Visibility.Collapsed;
             
            }
            else if ((nbreMois > 18) && (nbreMois <= 27))
            {
                //Mettre les cellules de la ligne 1 en modification
                for (int i = 0; i < this.dgvDerniereConsoPart1.Columns.Count; i++)
                {
                    this.dgvDerniereConsoPart1.Columns[i].IsReadOnly = false;
                }
                //Mettre les cellules de la ligne 2 en modification
                for (int i = 0; i < this.dgvDerniereConsoPart2.Columns.Count; i++)
                {
                    this.dgvDerniereConsoPart2.Columns[i].IsReadOnly = false;
                }

                //Mettre les cellules concernées de la ligne 3 en modification
                for (int i = 0; i < nbreMois - 18; i++)
                {
                    this.dgvDerniereConsoPart3.Columns[i].IsReadOnly = false;
                }
                //Mettre les autres cellules non concernées de la ligne 3 en consultation
                for (int i = nbreMois - 18; i < this.dgvDerniereConsoPart3.Columns.Count; i++)
                {
                    this.dgvDerniereConsoPart3.Columns[i].IsReadOnly = true;
                }


                //Mettre les cellules de la ligne 4 en consultation
                for (int i = 0; i < this.dgvDerniereConsoPart4.Columns.Count; i++)
                {
                    this.dgvDerniereConsoPart4.Columns[i].IsReadOnly = true;
                }

                ////Mettre les cellules de la ligne 5 en consultation
                //for (int i = 0; i < this.dgvDerniereConsoPart5.Columns.Count; i++)
                //{
                //    this.dgvDerniereConsoPart5.Columns[i].IsReadOnly = true;
                //}

                dgvDerniereConsoPart2.Visibility = Visibility.Visible;
                dgvDerniereConsoPart3.Visibility = Visibility.Visible;
                dgvDerniereConsoPart4.Visibility = Visibility.Collapsed;
                //dgvDerniereConsoPart5.Visibility = Visibility.Collapsed;
               

            }
            else if ((nbreMois > 27) && (nbreMois <= 36))
            {
                //Mettre les cellules de la ligne 1 en modification
                for (int i = 0; i < this.dgvDerniereConsoPart1.Columns.Count; i++)
                {
                    this.dgvDerniereConsoPart1.Columns[i].IsReadOnly = false;
                }

                //Mettre les cellules de la ligne 2 en modification
                for (int i = 0; i < this.dgvDerniereConsoPart2.Columns.Count; i++)
                {
                    this.dgvDerniereConsoPart2.Columns[i].IsReadOnly = false;
                }

                //Mettre les cellules de la ligne 3 en modification
                for (int i = 0; i < this.dgvDerniereConsoPart3.Columns.Count; i++)
                {
                    this.dgvDerniereConsoPart3.Columns[i].IsReadOnly = false;
                }

                //Mettre les cellules concernées de la ligne 4 en modification
                for (int i = 0; i < nbreMois - 27; i++)
                {
                    this.dgvDerniereConsoPart4.Columns[i].IsReadOnly = false;
                }


                //Mettre les autres cellules non concernées de la ligne 4 en consultation
                for (int i = nbreMois - 27; i < this.dgvDerniereConsoPart4.Columns.Count; i++)
                {

                    this.dgvDerniereConsoPart4.Columns[i].IsReadOnly = true;
                }

                ////Mettre les cellules de la ligne 5 en consultation
                //for (int i = 0; i < this.dgvDerniereConsoPart5.Columns.Count; i++)
                //{
                   
                //    this.dgvDerniereConsoPart5.Columns[i].IsReadOnly = true;
                //}

                dgvDerniereConsoPart2.Visibility = Visibility.Visible;
                dgvDerniereConsoPart3.Visibility = Visibility.Visible;
                dgvDerniereConsoPart4.Visibility = Visibility.Visible;
                //dgvDerniereConsoPart5.Visibility = Visibility.Collapsed;
               
            }
            else if ((nbreMois > 36) && (nbreMois <= 45))
            {
                //Mettre les cellules de la ligne 1 en modification
                for (int i = 0; i < this.dgvDerniereConsoPart1.Columns.Count; i++)
                {
                    this.dgvDerniereConsoPart1.Columns[i].IsReadOnly = false;
                }

                //Mettre les cellules de la ligne 2 en modification
                for (int i = 0; i < this.dgvDerniereConsoPart2.Columns.Count; i++)
                {
                    this.dgvDerniereConsoPart2.Columns[i].IsReadOnly = false;
                }

                ////Mettre les cellules de la ligne 3 en modification
                //for (int i = 0; i < this.dgvDerniereConsoPart3.Columns.Count; i++)
                //{
                //    this.dgvDerniereConsoPart3.Columns[i].IsReadOnly = false;
                //}

                ////Mettre les cellules de la ligne 4 en modification
                //for (int i = 0; i < this.dgvDerniereConsoPart4.Columns.Count; i++)
                //{
                //    this.dgvDerniereConsoPart4.Columns[i].IsReadOnly = false;
                //}

                ////Mettre les cellules concernées de la ligne 5 en modification
                //for (int i = 0; i < nbreMois - 36; i++)
                //{
                //    this.dgvDerniereConsoPart5.Columns[i].IsReadOnly = false;
                //}


                //Mettre les autres cellules non concernées de la ligne 5 en consultation
                //for (int i = nbreMois - 36; i < this.dgvDerniereConsoPart5.Columns.Count; i++)
                //{
                   
                //    this.dgvDerniereConsoPart5.Columns[i].IsReadOnly = true;
                //}

                dgvDerniereConsoPart2.Visibility = Visibility.Visible;
                //dgvDerniereConsoPart3.Visibility = Visibility.Visible;
                //dgvDerniereConsoPart4.Visibility = Visibility.Visible;
                //dgvDerniereConsoPart5.Visibility = Visibility.Visible;
            }
        }

      

        private void ChargeDonneDemande(int pk_id)
        {

            FraudeServiceClient service = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
            service.RetourDemandeFraudeCompleted += (s, args) =>
            {
                try
                {
                    if (args != null && args.Cancelled)
                        return;
                    LaDemande = args.Result;
                    //if (LaDemande != null)
                    //{
                    //    txt_Nom.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Nomabon) ? string.Empty : LaDemande.ClientFraude.Nomabon;
                    //    txt_Nom.Tag = string.IsNullOrEmpty(LaDemande.ClientFraude.PK_ID.ToString()) ? string.Empty : LaDemande.ClientFraude.PK_ID.ToString();
                    //    txt_refclient.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Client) ? string.Empty : LaDemande.ClientFraude.Client; ;
                    //    txt_email.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Email) ? string.Empty : LaDemande.ClientFraude.Email; ;
                    //    txt_telephone.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Telephone) ? string.Empty : LaDemande.ClientFraude.Telephone; ;
                    //    txt_porte.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Porte) ? string.Empty : LaDemande.ClientFraude.Porte; ;
                    //    txt_ContactAbonne.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.ContratAbonnement) ? string.Empty : LaDemande.ClientFraude.ContratAbonnement;
                    //    txt_contarBrachement.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.ContratBranchement) ? string.Empty : LaDemande.ClientFraude.ContratBranchement;
                    //    txt_Numerotraitement.Text = string.IsNullOrEmpty(LaDemande.LaDemande.NUMDEM) ? string.Empty : LaDemande.LaDemande.NUMDEM;
                    //}
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }

            };
            service.RetourDemandeFraudeAsync(pk_id);

        }

        private void Validationdemande(CsDemandeFraude LaDemande)
        {
            try
            {
                FraudeServiceClient Client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude")); ;
                Client.ValiderDemandeConsommationCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (b.Result != null)
                    {
                        Message.Show("Affectation effectuée avec succès", "Information");
                        List<int> Listid = new List<int>();
                        Listid.Add(LaDemande.LaDemande.PK_ID);
                        EnvoyerDemandeEtapeSuivante(Listid);
                        this.DialogResult = true;



                    }
                    else
                        Message.ShowError("Erreur a la cloture de la demande", "Cloturedemande");
                };
                Client.ValiderDemandeConsommationAsync(LaDemande);

            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, "Transmit");
            }
        }

        private void EnvoyerDemandeEtapeSuivante(List<int> Listid)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));

            clientWkf.ExecuterActionSurDemandeParPkIDLigneCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec de sortie materiel", Langue.lbl_Menu);
                    return;
                }
                if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
                {
                    Message.ShowError("Echec de sortie materiel", Langue.lbl_Menu);
                    return;
                }
                else
                {
                    //Message.ShowInformation("Sortie materiel éffectuée", Langue.lbl_Menu);

                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }

        //private void btnVAlidation_Click(object sender, RoutedEventArgs e)
        //{
        //    Recupere(LaDemande);
        //    Validationdemande(LaDemande);
        //}

        private void Enregistrer(CsDemandeFraude LaDemande)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Langue.Fraude, Langue.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        listForInsertOrUpdate = LaDemande;
                        var service = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                        if (listForInsertOrUpdate != null)
                        {
                            ///appel de l ecran UcAnnalyse
                            service.InsertionFraudeConsommationCompleted += (snder, insertR) =>
                            {
                                if (insertR.Cancelled ||
                                    insertR.Error != null)
                                {
                                    Message.ShowError(insertR.Error.Message, Langue.Fraude);
                                    return;
                                }
                                if (insertR.Result == false)
                                {
                                    Message.ShowError(Langue.ErreurInsertionDonnees, Langue.Fraude);
                                    return;
                                }
                                //OnEvent(null);
                                DialogResult = true;
                            };
                            service.InsertionFraudeConsommationAsync(listForInsertOrUpdate);
                        }

                        else
                        {
                            return;
                        }
                    }

                };
                messageBox.Show();
            }

            catch (Exception ex)
            {
                //Message.Show(ex.Message, Language.);
            }



        }


        private void dgvDerniereConsoPart1_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            CalculConsommationDerniereConso();
        }

        private int consomationFactureBymois( List<DefaultValue1> ListDataGrid )
        { 
            int ConsoSaise = 0;
            if (ListDataGrid != null && ListDataGrid.Count > 0)
            {
                foreach (DefaultValue1 Item in ListDataGrid)
                {
                    if (Item != null)
                    {
                        foreach (var item_ in Item.GetType().GetProperties())
                        {
                            var ValeurObject = GetPropValue(Item, item_.Name);
                            string ValeurString = string.Empty;

                            if (ValeurObject != null)
                            {
                                ValeurString = ValeurObject.ToString();
                            }
                            if (!string.IsNullOrWhiteSpace(ValeurString))
                            {
                                int Conso = 0;
                                int.TryParse(ValeurString, out Conso);
                                if (Conso != 0)
                                {
                                    ConsoSaise += Conso;

                                }
                            }
                        }
                    }
                }
            }

            return ConsoSaise;
        }

        private int consomationFactureBymois2(List<DefaultValue2> ListDataGrid)
        {
            int ConsoSaise = 0;
            if (ListDataGrid != null && ListDataGrid.Count > 0)
            {
                foreach (DefaultValue2 Item in ListDataGrid)
                {
                    if (Item != null)
                    {
                        foreach (var item_ in Item.GetType().GetProperties())
                        {
                            var ValeurObject = GetPropValue(Item, item_.Name);
                            string ValeurString = string.Empty;

                            if (ValeurObject != null)
                            {
                                ValeurString = ValeurObject.ToString();
                            }
                            if (!string.IsNullOrWhiteSpace(ValeurString))
                            {
                                int Conso = 0;
                                int.TryParse(ValeurString, out Conso);
                                if (Conso != 0)
                                {
                                    ConsoSaise += Conso;

                                }
                            }
                        }
                    }
                }
            }

            return ConsoSaise;
        }

        private int consomationFactureBymois3(List<DefaultValue3> ListDataGrid)
        {
            int ConsoSaise = 0;
            if (ListDataGrid != null && ListDataGrid.Count > 0)
            {
                foreach (DefaultValue3 Item in ListDataGrid)
                {
                    if (Item != null)
                    {
                        foreach (var item_ in Item.GetType().GetProperties())
                        {
                            var ValeurObject = GetPropValue(Item, item_.Name);
                            string ValeurString = string.Empty;

                            if (ValeurObject != null)
                            {
                                ValeurString = ValeurObject.ToString();
                            }
                            if (!string.IsNullOrWhiteSpace(ValeurString))
                            {
                                int Conso = 0;
                                int.TryParse(ValeurString, out Conso);
                                if (Conso != 0)
                                {
                                    ConsoSaise += Conso;

                                }
                            }
                        }
                    }
                }
            }

            return ConsoSaise;
        }

        private int consomationFactureBymois4(List<DefaultValue4> ListDataGrid)
        {
            int ConsoSaise = 0;
            if (ListDataGrid != null && ListDataGrid.Count > 0)
            {
                foreach (DefaultValue4 Item in ListDataGrid)
                {
                    if (Item != null)
                    {
                        foreach (var item_ in Item.GetType().GetProperties())
                        {
                            var ValeurObject = GetPropValue(Item, item_.Name);
                            string ValeurString = string.Empty;

                            if (ValeurObject != null)
                            {
                                ValeurString = ValeurObject.ToString();
                            }
                            if (!string.IsNullOrWhiteSpace(ValeurString))
                            {
                                int Conso = 0;
                                int.TryParse(ValeurString, out Conso);
                                if (Conso != 0)
                                {
                                    ConsoSaise += Conso;

                                }
                            }
                        }
                    }
                }
            }

            return ConsoSaise;
        }

        private int consomationFactureBymois5(List<DefaultValue5> ListDataGrid)
        {
            int ConsoSaise = 0;
            if (ListDataGrid != null && ListDataGrid.Count > 0)
            {
                foreach (DefaultValue5 Item in ListDataGrid)
                {
                    if (Item != null)
                    {
                        foreach (var item_ in Item.GetType().GetProperties())
                        {
                            var ValeurObject = GetPropValue(Item, item_.Name);
                            string ValeurString = string.Empty;

                            if (ValeurObject != null)
                            {
                                ValeurString = ValeurObject.ToString();
                            }
                            if (!string.IsNullOrWhiteSpace(ValeurString))
                            {
                                int Conso = 0;
                                int.TryParse(ValeurString, out Conso);
                                if (Conso != 0)
                                {
                                    ConsoSaise += Conso;

                                }
                            }
                        }
                    }
                }
            }

            return ConsoSaise;
        }

        private void CalculConsommationDerniereConso()
        {
            int facturee = 0;
            listMoisGrid1.Clear();
            listMoisGrid1 = ((ObservableCollection<DefaultValue1>)dgvDerniereConsoPart1.ItemsSource).ToList();
            listMoisGrid2.Clear();
            listMoisGrid2 = ((ObservableCollection<DefaultValue2>)dgvDerniereConsoPart2.ItemsSource).ToList();
            listMoisGrid3.Clear();
            //listMoisGrid3 = ((ObservableCollection<DefaultValue3>)dgvDerniereConsoPart3.ItemsSource).ToList();
            //listMoisGrid4.Clear();
            //listMoisGrid4 = ((ObservableCollection<DefaultValue4>)dgvDerniereConsoPart4.ItemsSource).ToList();
            //listMoisGrid5.Clear();
            //listMoisGrid5 = ((ObservableCollection<DefaultValue5>)dgvDerniereConsoPart5.ItemsSource).ToList();
            if (listMoisGrid1 != null && listMoisGrid1.Count > 0)
            {
                facturee += consomationFactureBymois(listMoisGrid1);
                if (listMoisGrid2 != null && listMoisGrid2.Count > 0)
                    facturee += consomationFactureBymois2(listMoisGrid2);
                if (listMoisGrid3 != null && listMoisGrid3.Count > 0)
                    facturee += consomationFactureBymois3(listMoisGrid3);
                if (listMoisGrid4 != null && listMoisGrid4.Count > 0)
                    facturee += consomationFactureBymois4(listMoisGrid4);
                if (listMoisGrid5 != null && listMoisGrid5.Count > 0)
                    facturee += consomationFactureBymois5(listMoisGrid5);
            }
            this.txtConsommationDejaFacturee.Text = facturee.ToString();
        }

        private void CalculConsommationDerniereConsoAd()
        {
            int facturee = 0;
            //listMoisGrid1.Clear();
            //listMoisGrid1 = ((ObservableCollection<DefaultValue1>)dgvDerniereConsoPart1.ItemsSource).ToList();
            //listMoisGrid2.Clear();
            //listMoisGrid2 = ((ObservableCollection<DefaultValue2>)dgvDerniereConsoPart2.ItemsSource).ToList();
            //listMoisGrid3.Clear();
            //listMoisGrid3 = ((ObservableCollection<DefaultValue3>)dgvDerniereConsoPart3.ItemsSource).ToList();
            //listMoisGrid4.Clear();
            //listMoisGrid4 = ((ObservableCollection<DefaultValue4>)dgvDerniereConsoPart4.ItemsSource).ToList();
            //listMoisGrid5.Clear();
            //listMoisGrid5 = ((ObservableCollection<DefaultValue5>)dgvDerniereConsoPart5.ItemsSource).ToList();
            if (listMoisGrid1 != null && listMoisGrid1.Count > 0)
            {
                facturee += consomationFactureBymois(listMoisGrid1);
                if (listMoisGrid2 != null && listMoisGrid2.Count > 0)
                    facturee += consomationFactureBymois2(listMoisGrid2);
                if (listMoisGrid3 != null && listMoisGrid3.Count > 0)
                    facturee += consomationFactureBymois3(listMoisGrid3);
                if (listMoisGrid4 != null && listMoisGrid4.Count > 0)
                    facturee += consomationFactureBymois4(listMoisGrid4);
                if (listMoisGrid5 != null && listMoisGrid5.Count > 0)
                    facturee += consomationFactureBymois5(listMoisGrid5);
            }
            this.txtConsommationDejaFacturee.Text = facturee.ToString();
        }

        private void btn_Evenement_Click(object sender, RoutedEventArgs e)
        {
            RetourneEvenement(LaDemande);

        }

        private void RetourneEvenement(CsDemandeFraude LaDemande)
        {
            try
            {
                 FraudeServiceClient service = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                service.RetourneEvenementCompleted += (s, args) =>
                  {  
                      if (args != null && args.Cancelled)
                        return;
                    if (args.Result.Count != 0)
                    {
                       UcHistoriqueEvenement ctrl = new UcHistoriqueEvenement(args.Result.OrderBy(t => t.DATEEVT).ToList());
                        ctrl.Show();
                    }
                };
                service.RetourneEvenementAsync((int)LaDemande.ClientFraude.FK_IDCENTRE, LaDemande.ClientFraude.Centre, LaDemande.ClientFraude.Client, LaDemande.ClientFraude.Ordre);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private void txt_Periode_TextChanged(object sender, TextChangedEventArgs e)
        {
         
        }
        private void RecupereConso(List<CsEvenement> Listresult )
        {
            int Competeur = Listresult.Count();
            DefaultValue1 Def1 = new DefaultValue1();
            DefaultValue2 Def2 = new DefaultValue2();
            DefaultValue3 Def3 = new DefaultValue3();
            listMoisGrid1.Clear();
            dgvDerniereConsoPart1.ItemsSource = null;
            listMoisGrid2.Clear();
            dgvDerniereConsoPart2.ItemsSource = null;
            listMoisGrid3.Clear();
           dgvDerniereConsoPart3.ItemsSource = null;
            int i = 1;
            int j = 1;
            int t = 1;
                while (i <= Competeur)
                {
               foreach (var item in Listresult)
                 {
                    if (i <= 9)
                    {
                        

                        PropertyInfo propertyInfo = Def1.GetType().GetProperty("Mois" + i);
                        propertyInfo.SetValue(Def1, item.CONSO, null);
                       
                    }
                    if (i>=10 && i <= 18)
                    {


                        PropertyInfo propertyInfo = Def2.GetType().GetProperty("Mois" + j);
                        propertyInfo.SetValue(Def2, item.CONSO, null);
                        j++;
                       
                    }
                    if ( i >= 20 && i <=27 )
                    {


                        PropertyInfo propertyInfo = Def3.GetType().GetProperty("Mois" + t);
                        propertyInfo.SetValue(Def3, item.CONSO, null);
                        t++;
                        
                    }
                    i++;
                   
                  }
                }
            //  for (int i = 1; i <= Competeur; i++)
            //    {
            //    if (i <= 9)
            //    {
                   

            //            string value = "5.5";
            //            PropertyInfo propertyInfo = Def1.GetType().GetProperty("Mois" + i);
            //            propertyInfo.SetValue(Def1, item.CONSO, null);
                       
                        
            //        }

            //    }
               
              listMoisGrid1.Add(Def1 as DefaultValue1);
              dgvDerniereConsoPart1.ItemsSource = listMoisGrid1;
              listMoisGrid2.Add(Def2 as DefaultValue2);
              dgvDerniereConsoPart2.ItemsSource = listMoisGrid2;
              listMoisGrid3.Add(Def3 as DefaultValue3);
              dgvDerniereConsoPart3.ItemsSource = listMoisGrid3;
              CalculConsommationDerniereConsoAd();
        }

       

        private void txt_Periode_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void dgvDerniereConsoPart1_Loaded(object sender, RoutedEventArgs e)
        {
            CalculConsommationDerniereConso();
        }

        private void txt_Recherche_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txt_Periode.Text != string.Empty)
                {
                    FraudeServiceClient service = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                    service.ConsommationByPeriodeMoisCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result.Count != 0)
                        {
                            RecupereConso(args.Result);
                        }
                    };
                    service.ConsommationByPeriodeMoisAsync(LaDemande, Int32.Parse(this.nudNombreMois.Value.ToString()), txt_Periode.Text.Trim());
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}


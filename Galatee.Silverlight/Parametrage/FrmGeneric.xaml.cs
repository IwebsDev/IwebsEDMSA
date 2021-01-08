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
    public partial class FrmGeneric : ChildWindow
    {
        public FrmGeneric()
        {
            InitializeComponent();
        }

        public FrmGeneric(int numero, string text, string TableNom)
        {
            InitializeComponent();

            num = numero;
            this.Title = text;
            LibelleEtat = text;
            this.TableName = TableNom;

            this.Charger(numero);
        }

        int num;
        int rowIndex = -1;

        List<aTa> donnesDatagrid = new List<aTa>();
        List<aTa> newData = new List<aTa>();
        List<aTa> majData = new List<aTa>();

        private string TableName;
        private string LibelleEtat = string.Empty;

        private bool EstNumerique = false;
        private bool CentreEstNumerique = false;

        List<CTab300> rowcomboselectedObject = new List<CTab300>();
        List<DateTime?> rowselectDate = new List<DateTime?>();

        List<aParam> RecupererLesElementsSelectionnes()
        {
            List<aParam> cls = new List<aParam>();
            foreach (aParam lvi in this.lvwResultat.SelectedItems)
            {
                aParam cl = lvi;
                cls.Add(cl);
            }
            return cls;
        }

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Charger(int nummtab)
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

                donnesDatagrid.Clear();
                newData.Clear();
                majData.Clear();

                rowcomboselectedObject.Clear();
                rowselectDate.Clear();

                ParametrageClient client = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                client.SELECTALLByNUMTABLECompleted += (ssender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            MessageBox.Show(error, "SELECTALLByNUMTABLE", MessageBoxButton.OK);
                            desableProgressBar();
                            this.DialogResult = true;
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            MessageBox.Show("No data found ", "SELECTALLByNUMTABLE", MessageBoxButton.OK);
                            desableProgressBar();
                            this.DialogResult = true;
                            return;
                        }

                        aTa emptyplace = new aTa();
                        donnesDatagrid.Clear();
                        lvwResultat.ItemsSource = null;

                        
                        donnesDatagrid.AddRange(args.Result);
                        donnesDatagrid.Add(emptyplace);
                        lvwResultat.ItemsSource = donnesDatagrid;

                    };
                client.SELECTALLByNUMTABLEAsync(numtable);

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

        private void lvwResultat_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {
            try
            {
                aTa u = e.Row.DataContext as aTa; //fetch the row data

                if (majData.Count > 0 && majData != null)
                {
                    if (majData.First(p => p.CENTRE == u.CENTRE && p.ROWID == u.ROWID && u.CODE == p.CODE) != null)
                    {
                        foreach (aTa t in majData)
                        {
                            if (t.CODE == u.CODE && t.ROWID == u.ROWID)
                            {
                                t.CENTRE = u.CENTRE;
                                t.CODE = u.CODE;
                                t.LIBELLE = u.LIBELLE;
                                t.NUM = u.NUM;
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

        private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddRow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Txt_NumeroTable.Text) || string.IsNullOrEmpty(txtcode.Text) || 
                    string.IsNullOrEmpty(txtref.Text) || string.IsNullOrEmpty(txtcentre.Text))
                     {
                    MessageBox.Show("All fiedls are required before inserting");
                    return;
                   }

                aTa add = new aTa()
                {
                    NUM = Convert.ToInt16(Txt_NumeroTable.Text),
                    LIBELLE = txtref.Text,
                    CODE = txtcode.Text,
                    DMAJ = DateTime.Now,
                    CENTRE = txtcentre.Text
                };

                newData.Add(add);
                lvwResultat.ItemsSource = null;
                donnesDatagrid.Add(add);

                lvwResultat.ItemsSource = donnesDatagrid;

                resetInsertData();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            
        }

        void resetInsertData()
        {
            txtcentre.Text =Txt_NumeroTable.Text = txtcode.Text =  string.Empty;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ParametrageClient cls = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
            cls.insertOrUpdateTA0Completed += (ss, resut) =>
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
            cls.insertOrUpdateTA0Async(newData,majData);
        }

    }
}



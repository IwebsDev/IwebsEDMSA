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
using System.Reflection;

namespace Galatee.Silverlight.Parametrage
{
    public partial class FrmListOfTables : ChildWindow
    {
        public FrmListOfTables()
        {
            InitializeComponent();
            lvwResultat.SelectionChanged += new SelectionChangedEventHandler(lvwResultat_SelectionChanged);
            GetData();
        }

        int code;

        bool passageFirst = false;
        List<CsProduit> produits = new List<CsProduit>();
        List<CsInit> donnesDatagrid = new List<CsInit>();

        List<aTa0> ta0list = new List<aTa0>();

        private string text = string.Empty;
        void GetData()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                client.SelectAllTa0Completed += (ss, res) =>
                    {
                        if (res.Cancelled || res.Error != null)
                        {
                            string error = res.Error.Message;
                            MessageBox.Show(error, "SelectAllTa0", MessageBoxButton.OK);
                            desableProgressBar();
                            return;
                        }

                        if (res.Result == null || res.Result.Count == 0)
                        {
                            MessageBox.Show("No data found ", "SelectAllTa0", MessageBoxButton.OK);
                            desableProgressBar();
                            return;
                        }

                        ta0list.AddRange(res.Result);
                        lvwResultat.ItemsSource = res.Result;
                        lvwResultat.SelectedItem = res.Result[0];
                        this.DataContext = ta0list;
                            Txt_NumeroTable.Text = res.Result[0].CODE;
                    };
                client.SelectAllTa0Async();
                
            }
            catch (Exception ex)
            {
                throw ex;
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
           
        }

        private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvwResultat.SelectedItem == null)
                return;
            
            aTa0 selected = lvwResultat.SelectedItem as aTa0;
            Txt_NumeroTable.Text = selected.CODE;
        }

        private void myDataGrid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                aTa0 o = lvwResultat.SelectedItem as aTa0;
                string Ucname = string.Empty;
                //    if(int.Parse(o.CODE) > 0 && int.Parse(o.CODE)  < 1000)
                this.AfficherControleUtilisateurCorrespondant(o);
                //this.AfficherControleUtilisateurCorrespondant(int.Parse(o.CODE), o.LIBELLE);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                //MessageBox.Show(ex.Message, FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void DoBeForeUcInitShow()
        {
            try
            {


                ParametrageClient client = new ParametrageClient(Utility.Protocole(), Utility.EndPoint(this));
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
                    produits.AddRange(args.Result);

                    ParametrageClient proxy = new ParametrageClient(Utility.Protocole(), Utility.EndPoint(this));
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
                        donnesDatagrid.AddRange(args1.Result);
                        donnesDatagrid.Add(emptyplace2);

                        ParametrageClient clss = new ParametrageClient(Utility.Protocole(), Utility.EndPoint(this));
                        clss.SELECT_INIT_SELECT_COLUMNSCompleted += (senders, ress) =>
                            {
                                if (ress.Cancelled || ress.Error != null)
                                {
                                    string error = ress.Error.Message;
                                    MessageBox.Show(error, ".SELECT_INIT_SELECT_COLUMNS", MessageBoxButton.OK);
                                    desableProgressBar();
                                    this.DialogResult = true;
                                    return;
                                }

                                if (ress.Result == null || ress.Result.Count == 0)
                                {
                                    MessageBox.Show("No data found ", ".SELECT_INIT_SELECT_COLUMNS", MessageBoxButton.OK);
                                    desableProgressBar();
                                    this.DialogResult = true;
                                    return;
                                }
                                // raise show method for ucInit

                                List<CsZone> zones = new List<CsZone>();
                                zones.AddRange(ress.Result);
                                //UcINIT Ecran = new UcINIT(code, text, SessionObject.EnumereProcedureStockee.INIT, donnesDatagrid, produits, zones);
                                //UcINIT Ecran = new UcINIT(code, text, SessionObject.EnumereProcedureStockee.INIT);
                                //Ecran.Show(); //Modifier par HGB le 03/01/2013 pour l'affichage modal de la fenêtre EN silverlight
                            };
                        clss.SELECT_INIT_SELECT_COLUMNSAsync();
                    };
                    proxy.SelectInitTableDataAsync();

                };
                client.SelectAllProductsAsync();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

        }

        private void AfficherControleUtilisateurCorrespondant(aTa0 ta0)
        {
            try
            {
                code = int.Parse(ta0.CODE);
                text = ta0.LIBELLE;

                if ((code > 0) && (code < 1000))
                {
                    FrmGeneric Fgeneric = new FrmGeneric(code, text, SessionObject.EnumereProcedureStockee.TA);
                    Fgeneric.Show();
                }
                else
                {
                    CreationFormulaire(ta0);
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
               // MessageBox.Show(ex.Message, FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreationFormulaire(aTa0 ta0)
        {
            object obj = new object();
            string classFullName = string.Empty;
            try
            {
                Type ControlType = null;
                //string classFullName = "Galatee.Silverlight.Parametrage." + ta0.FORMENAME;
                //string Assemblys = Assembly.GetExecutingAssembly().GetName().Name;
                classFullName = "Galatee.Silverlight.Parametrage."  + ta0.FORMENAME;
                ControlType = Assembly.GetExecutingAssembly().GetType(classFullName);
                obj = Activator.CreateInstance(ControlType);

                //Type[] mytypes = a.GetTypes();
                //foreach (Type ct in mytypes)
                //{
                //    var tab = new string[] { };
                //    if (ct.FullName != null) tab = ct.FullName.Split(new[] { '.' });
                //    string[] tabInfo = classFullName.Split(new[] { '.' });

                //    if (tab.Length == tabInfo.Length)
                //    {
                //        if (ct.FullName == classFullName)
                //            obj = Activator.CreateInstance(ct);

                //    }
                //}
                        ChildWindow form = (ChildWindow)obj;
                        form.Title = ta0.LIBELLE;
                        form.Tag = ta0;
                        form.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }

              
        }


        //private void AfficherControleUtilisateurCorrespondant(int _code, string libelle)
        //{
        //    try
        //    {
        //        code = _code;
        //        text = libelle;

        //        if ((code > 0) && (code < 1000))
        //        {
        //            FrmGeneric Fgeneric = new FrmGeneric(code, text, SessionObject.EnumereProcedureStockee.TA);
        //            Fgeneric.Show();
        //        }
        //        else
        //        {
        //            CreationFormulaire("");
        //            //switch (code)
        //            //{
        //            //    case 0:
        //            //        {
        //            //            this.GetData();
        //            //            break;
        //            //        }
        //            //    case 1001:
        //            //        {
        //            //            // traitement a effectue avant l'affichage du UcInit

        //            //            DoBeForeUcInitShow();

        //            //            //UcINIT Ecran = new UcINIT(code, text, SessionObject.EnumereProcedureStockee.INIT);
        //            //            ////UcINIT Ecran = new UcINIT(code, text, SessionObject.EnumereProcedureStockee.INIT);
        //            //            //Ecran.Show(); //Modifier par HGB le 03/01/2013 pour l'affichage modal de la fenêtre EN silverlight
        //            //            break;
        //            //        }
        //            //    case 1002:
        //            //        {
        //            //            UcRue Ecran = new UcRue(code, text, SessionObject.EnumereProcedureStockee.RUES);
        //            //            Ecran.Show(); //Modifier par HGB 09/01/2013 pour l'affichage modal de la fenêtre 
        //            //            break;
        //            //        }
        //            //    case 1003:
        //            //        {
        //            //            UcREGROU Ecran = new UcREGROU(code, text, SessionObject.EnumereProcedureStockee.REGROU);
        //            //            Ecran.Show(); //Modifier par HGB le 14/01/2013 pour l'affichage modal de la fenêtre 
        //            //            break;
        //            //        }

        //            //    case 1005:
        //            //        {
        //            //            UcDIACOMP Ecran = new UcDIACOMP(code, text, SessionObject.EnumereProcedureStockee.DIACOMP);
        //            //            Ecran.Show(); //Modifier par HGB le 14/01/2013 pour l'affichage modal de la fenêtre
        //            //            break;
        //            //        }
        //            //    case 1006:
        //            //        {
        //            //            UcTCOMPT Ecran = new UcTCOMPT(code, text, SessionObject.EnumereProcedureStockee.TCOMPT);
        //            //            Ecran.Show(); //Modifier par HGB le 17/01/2013 pour l'affichage modal de la fenêtre 
        //            //            break;
        //            //        }
        //            //    case 1007:
        //            //        {
        //            //            UcPuissance Ecran = new UcPuissance(code, text, SessionObject.EnumereProcedureStockee.PUISSANCE);
        //            //            Ecran.Show(); //Modifier par HGB le 17/01/2013 pour l'affichage modal de la fenêtre 
        //            //            break;
        //            //        }
        //            //    case 1010:
        //            //        {

        //            //            UcTARIF Ecran = new UcTARIF(code, text, SessionObject.EnumereProcedureStockee.TARIF);
        //            //            Ecran.Show(); //Modifier par HGB le 18/01/2013 pour l'affichage modal de la fenêtre 
        //            //            break;
        //            //        }
        //            //    case 1011:
        //            //        {
        //            //            UcFORFAIT Ecran = new UcFORFAIT(code, text, SessionObject.EnumereProcedureStockee.FORFAIT);
        //            //            Ecran.Show(); //Modifier par HGB le 18/01/2013 pour l'affichage modal de la fenêtre 
        //            //            break;
        //            //        }
        //            //    case 1014:
        //            //        {
        //            //            //Mis en commentaire par ATO pour prendre en compte l'ajout de la colonne
        //            //            //if (Galate.Datacess.EnumProcedureStockee.ActiveconnectionStringName == Galate.Datacess.EnumProcedureStockee.conectionstring.GALADBConnectionString.ToString())
        //            //            //{
        //            //            //    code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //            //    text = (String)dgTable.SelectedCells[1].Value;
        //            //            //    UcBANQUES Ecran = new UcBANQUES(code, text, EnumNomTable.BANQUE);
        //            //            //    Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //            //}
        //            //            //elsews
        //            //            //{
        //            //            UcBANQUES Ecran = new UcBANQUES(code, text, SessionObject.EnumereProcedureStockee.BANQUE);
        //            //            Ecran.Show(); //Modifier par HGB le 18/01/2013 pour l'affichage modal de la fenêtre 
        //            //            //}
        //            //            break;
        //            //        }
        //            //    case 1016:
        //            //        {
        //            //            UcCASIND Ecran = new UcCASIND(code, text, SessionObject.EnumereProcedureStockee.CASIND);
        //            //            Ecran.Show(); //Modifier par HGB le 11/02/2013  pour l'affichage modal de la fenêtre 
        //            //            break;
        //            //        }
        //            //    //case 1023:
        //            //    //    {
        //            //    //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //    //        text = (String)dgTable.SelectedCells[1].Value;
        //            //    //        UcCTAX Ecran = new UcCTAX(code, text, EnumNomTable.CTAX);
        //            //    //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //    //        break;
        //            //    //    }
        //            //    //case 1025:
        //            //    //    {
        //            //    //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //    //        text = (String)dgTable.SelectedCells[1].Value;
        //            //    //        UcMATRICULE Ecran = new UcMATRICULE(code, text, EnumNomTable.MATRICULE);
        //            //    //        Ecran.ShowDialog(this.text, Inova.Windows.FormsTAB300.ExecMode.Consultation); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //    //        break;
        //            //    //    }
        //            //    case 1034:
        //            //        {
        //            //            UcTDEM Ecran = new UcTDEM(code, text, SessionObject.EnumereProcedureStockee.TYPEDEMANDE);
        //            //            Ecran.Show(); //Modifier  par HGB le 12/02/2013 pour l'affichage modal de la fenêtre 
        //            //            break;
        //            //        }
        //            //    //case 1038:
        //            //    //    {
        //            //    //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //    //        text = (String)dgTable.SelectedCells[1].Value;
        //            //    //        UcREGCLI Ecran = new UcREGCLI(code, text, EnumNomTable.REGCLI);
        //            //    //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //    //        break;
        //            //    //    }
        //            //    //case 1039:
        //            //    //    {
        //            //    //        //FrmClientRegrp Cclientregroup = new FrmClientRegrp();
        //            //    //        //Cclientregroup.ShowDialog(); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //    //        //break;
        //            //    //        MessageBox.Show(Inova.Common.TAB300.ManageLanguage.RM.GetString("msgCetteNestPasTraite"), this.FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //    //        break;
        //            //    //    }
        //            //    //case 1041:
        //            //    //    {
        //            //    //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //    //        text = (String)dgTable.SelectedCells[1].Value;
        //            //    //        UcGEOGES Ecran = new UcGEOGES(code, text, EnumNomTable.GEOGES);
        //            //    //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //    //        break;
        //            //    //    }
        //            //    //case 1044:
        //            //    //    {
        //            //    //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //    //        text = (String)dgTable.SelectedCells[1].Value;
        //            //    //        UcQUARTIER Ecran = new UcQUARTIER(code, text, EnumNomTable.QUARTIER);
        //            //    //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //    //        break;
        //            //    //    }
        //            //    //case 1046:
        //            //    //    {
        //            //    //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //    //        text = (String)dgTable.SelectedCells[1].Value;
        //            //    //        UcMESSAGE Ecran = new UcMESSAGE(code, text, EnumNomTable.MESSAGE);
        //            //    //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //    //        break;
        //            //    //    }
        //            //    //case 1048:
        //            //    //    {
        //            //    //        MessageBox.Show(Inova.Common.TAB300.ManageLanguage.RM.GetString("msgCetteNestPasTraite"), this.FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //    //        break;
        //            //    //    }
        //            //    //case 1049:
        //            //    //    {
        //            //    //        MessageBox.Show(Inova.Common.TAB300.ManageLanguage.RM.GetString("msgCetteNestPasTraite"), this.FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //    //        break;
        //            //    //    }
        //            //    //case 1053:
        //            //    //    {
        //            //    //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //    //        text = (String)dgTable.SelectedCells[1].Value;
        //            //    //        UcSPESITE Ecran = new UcSPESITE(code, text, EnumNomTable.SPESITE);
        //            //    //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //    //        break;
        //            //    //    }
        //            //    //case 1054:
        //            //    //    {
        //            //    //        MessageBox.Show(Inova.Common.TAB300.ManageLanguage.RM.GetString("msgCetteNestPasTraite"), this.FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //    //        break;
        //            //    //    }
        //            //    //case 1055:
        //            //    //    {
        //            //    //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //    //        text = (String)dgTable.SelectedCells[1].Value;
        //            //    //        UcREGEXO Ecran = new UcREGEXO(code, text, EnumNomTable.REGEXO);
        //            //    //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //    //        break;
        //            //    //    }
        //            //    //case 1058: //table en attente de finission
        //            //    //    {
        //            //    //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //    //        text = (String)dgTable.SelectedCells[1].Value;
        //            //    //        UcDIRECTEUR Cdirecteur = new UcDIRECTEUR(code, text, EnumNomTable.DIRECTEUR);
        //            //    //        Cdirecteur.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //    //        break;
        //            //    //    }
        //            //    case 1059:  //table en attente de finission
        //            //        {

        //            //            UcDEMCOUT Cdirecteur = new UcDEMCOUT(code, text, SessionObject.EnumereProcedureStockee.DEMCOUT);
        //            //            Cdirecteur.Show(); //Modifier  par HGB le 13/02/2013 pour l'affichage modal de la fenêtre
        //            //            break;
        //            //        }
        //            //case 1060:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcMODEREG Ecran = new UcMODEREG(code, text, EnumNomTable.MODEREG);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1061:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcFRAISTIMB Ecran = new UcFRAISTIMB(code, text, EnumNomTable.FRAISTIMB);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1062:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcFRAISHP Ecran = new UcFRAISHP(code, text, EnumNomTable.FRAISHP);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1063:
        //            //    {
        //            //        MessageBox.Show(Inova.Common.TAB300.ManageLanguage.RM.GetString("msgCetteNestPasTraite"), this.FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //        break;
        //            //    }
        //            //case 1064:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcCOPER Ecran = new UcCOPER(code, text, EnumNomTable.COPER);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1065:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcNATURE Ecran = new UcNATURE(code, text, EnumNomTable.NATURE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1066:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcDOMBANC Ecran = new UcDOMBANC(code, text, EnumNomTable.DOMBANC);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }

        //            //case 1067:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcCOPEROD Ecran = new UcCOPEROD(code, text, EnumNomTable.COPEROD);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1068:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcARRETE Ecran = new UcARRETE(code, text, EnumNomTable.ARRETE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1069:
        //            //    {
        //            //        MessageBox.Show(Inova.Common.TAB300.ManageLanguage.RM.GetString("msgCetteNestPasTraite"), this.FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //        break;
        //            //    }
        //            //case 1070:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcTAXCOMP Ecran = new UcTAXCOMP(code, text, EnumNomTable.TAXCOMP);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1071:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcIMPRIM Ecran = new UcIMPRIM(code, text, EnumNomTable.IMPRIM);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1072:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcREDEVANCE Ecran = new UcREDEVANCE(code, text, EnumNomTable.REDEVANCE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1073:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcNATGEN Ecran = new UcNATGEN(code, text, EnumNomTable.NATGEN);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1074:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcSCHEMAS Ecran = new UcSCHEMAS(code, text, EnumNomTable.SCHEMAS);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1075:
        //            //    {
        //            //        DB_TA db = new DB_TA();
        //            //        if (db.SELECT_Libelle_Verifie_UtilisationAjufin() == 1)
        //            //        {
        //            //            code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //            text = (String)dgTable.SelectedCells[1].Value;
        //            //            UcAJUFIN Ecran = new UcAJUFIN(code, text, EnumNomTable.AJUFIN);
        //            //            Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        }
        //            //        else throw new Exception(Inova.Common.TAB300.ManageLanguage.RM.GetString("MsgAjufin"));
        //            //        break;
        //            //    }
        //            //case 1076:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcFRAISCONTENTIEUX Ecran = new UcFRAISCONTENTIEUX(code, text, EnumNomTable.FRAISCONTENTIEUX);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1077:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcLIBRUBACTION Ecran = new UcLIBRUBACTION(code, text, EnumNomTable.LIBRUBACTION);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1078:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcSECURITEMATRICULE Ecran = new UcSECURITEMATRICULE(code, text, EnumNomTable.SECURITEMATRICULE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1079:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcMONNAIE Ecran = new UcMONNAIE(code, text, EnumNomTable.MONNAIE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1080:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcDEFPARAMABON Ecran = new UcDEFPARAMABON(code, text, EnumNomTable.DEFPARAMABON);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1081:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcPARAMABONUTILISE Ecran = new UcPARAMABONUTILISE(code, text, EnumNomTable.PARAMABONULILISE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1082:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcCENTREENCAISSABLE Ecran = new UcCENTREENCAISSABLE(code, text, EnumNomTable.CENTREENCAISSABLE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1083:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcTYPELOT Ecran = new UcTYPELOT(code, text, EnumNomTable.TYPELOT);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1084:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcCONTROLELIGNE Ecran = new UcCONTROLELIGNE(code, text, EnumNomTable.CONTROLELIGNE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1085:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcCONTROLESECONDAIRE Ecran = new UcCONTROLESECONDAIRE(code, text, EnumNomTable.CONTROLESECONDAIRE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1086:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcCONTROLEPIECE Ecran = new UcCONTROLEPIECE(code, text, EnumNomTable.CONTROLEPIECE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre 
        //            //        break;
        //            //    }
        //            //case 1087:
        //            //    {
        //            //        MessageBox.Show(Inova.Common.TAB300.ManageLanguage.RM.GetString("msgCetteNestPasTraite"), this.FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //        break;
        //            //    }
        //            //case 1088:
        //            //    {
        //            //        MessageBox.Show(Inova.Common.TAB300.ManageLanguage.RM.GetString("msgCetteNestPasTraite"), this.FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //        break;
        //            //    }
        //            //case 1089:
        //            //    {
        //            //        MessageBox.Show(Inova.Common.TAB300.ManageLanguage.RM.GetString("msgCetteNestPasTraite"), this.FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //        break;
        //            //    }
        //            //case 1090:
        //            //    {
        //            //        MessageBox.Show(Inova.Common.TAB300.ManageLanguage.RM.GetString("msgCetteNestPasTraite"), this.FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //        break;
        //            //    }
        //            //case 1091:
        //            //    {
        //            //        MessageBox.Show(Inova.Common.TAB300.ManageLanguage.RM.GetString("msgCetteNestPasTraite"), this.FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //        break;
        //            //    }
        //            //case 1094:
        //            //    {
        //            //        MessageBox.Show(Inova.Common.TAB300.ManageLanguage.RM.GetString("msgCetteNestPasTraite"), this.FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //        break;
        //            //    }
        //            //case 1100:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcMULTIMONNAIES Ecran = new UcMULTIMONNAIES(code, text, EnumNomTable.MULTIMONNAIES);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 1101:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcCodeControle Ecran = new UcCodeControle(code, text, EnumNomTable.CODECONTROLE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 1102:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcOrigineLot Ecran = new UcOrigineLot(code, text, EnumNomTable.ORIGINELOT);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 1107:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcSECTEUR Ecran = new UcSECTEUR(code, text, EnumNomTable.SECTEUR);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 1113:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcCOUTPUISSANCE Ecran = new UcCOUTPUISSANCE(code, text, EnumNomTable.COUTPUISSANCE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }

        //            //case 1116:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcTYPEBRANCHEMENT Ecran = new UcTYPEBRANCHEMENT(code, text, EnumNomTable.TYPEBRANCHEMENT);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 1122:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcPUISPERTES Ecran = new UcPUISPERTES(code, text, EnumNomTable.PUISPERTES);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 1123:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcCOMPTAGE Ecran = new UcCOMPTAGE(code, text, EnumNomTable.COMPTAGE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 1124:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcTRANSFOCOMPTAGE Ecran = new UcTRANSFOCOMPTAGE(code, text, EnumNomTable.TRANSFOCOMPTAGE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 2000:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        FrmGenericDevis Ecran = new FrmGenericDevis(code, text, EnumNomTable.TACHEDEVIS);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 2001:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcTypeDevis Ecran = new UcTypeDevis(code, text, EnumNomTable.TYPEDEVIS);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2002:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcAppareils Ecran = new UcAppareils(code, text, EnumNomTable.APPAREILS);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 2003:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcFourniture Ecran = new UcFourniture(code, text, EnumNomTable.FOURNITURE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 2004:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcCaracteristiques Ecran = new UcCaracteristiques(code, text, EnumNomTable.CARACTERISTIQUE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 2005:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcEtapeDevis Ecran = new UcEtapeDevis(code, text, EnumNomTable.ETAPEDEVIS);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 2006:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcETAPERECLAMATION Ecran = new UcETAPERECLAMATION(code, text, EnumNomTable.ETAPERECLAMATION);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 2007:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcTYPERECLAMATION Ecran = new UcTYPERECLAMATION(code, text, EnumNomTable.TYPERECLAMATION);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 2008:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcETAPEFONCTION Ecran = new UcETAPEFONCTION(code, text, EnumNomTable.ETAPEFONCTION);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 2009:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcPRESTATAIRE Ecran = new UcPRESTATAIRE(code, text, EnumNomTable.PRESTATAIRE);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 2010:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcStatutReclamation Ecran = new UcStatutReclamation(code, text, EnumNomTable.STATUTRECLAMATION);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }
        //            //case 2011:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcGroupeReclamation Ecran = new UcGroupeReclamation(code, text, EnumNomTable.GROUPERECLAMATION);
        //            //        Ecran.ShowDialog(this.text); //Modifier par ATO le 07/04/2009 pour l'affichage modal de la fenêtre
        //            //        break;
        //            //    }

        //            //case 2012:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcGroupProgram Ecran = new UcGroupProgram(code, text, EnumNomTable.GROUPPROGRAM);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }


        //            //case 2013:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcProgram Ecran = new UcProgram(code, text, EnumNomTable.PROGRAM);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }

        //            //case 2014:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcHabilitationProgram Ecran = new UcHabilitationProgram(code, text, EnumNomTable.HABILITATIONPROGRAM);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2015:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcProduit Ecran = new UcProduit(code, text, EnumNomTable.PRODUIT);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2016:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcMotifRejet Ecran = new UcMotifRejet(code, text, EnumNomTable.FILIERE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2017:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcFonction Ecran = new UcFonction(code, text, EnumNomTable.FONCTION);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2018:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcCentre Ecran = new UcCentre(code, text, EnumNomTable.CENTRE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2019:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcSite Ecran = new UcSite(code, text, EnumNomTable.SITE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2020:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcMotifRejet Ecran = new UcMotifRejet(code, text, EnumNomTable.MOTIFREJET);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2021:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcEtapeFonctionCheque Ecran = new UcEtapeFonctionCheque(code, text, EnumNomTable.ETAPEFONCTIONCHEQUE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }

        //            ////Rajouter par ATO le 30/03/2010 pour prendre en compte le paramètrage de l'application de gestion de la fraude
        //            //#region Paramètrage de l'application de gestion de la fraude

        //            //case 2022:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcTache Ecran = new UcTache(code, text, EnumLibelleTable.LibelleTACHE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2023:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcActionSurCompteur Ecran = new UcActionSurCompteur(code, text, EnumLibelleTable.LibelleACTIONSURCOMPTEUR);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2024:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcAgence Ecran = new UcAgence(code, text, EnumLibelleTable.LibelleAGENCE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2025:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcAnomalieReleve Ecran = new UcAnomalieReleve(code, text, EnumLibelleTable.LibelleANOMALIEDERELEVE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2026:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcCalibres Ecran = new UcCalibres(code, text, EnumLibelleTable.LibelleCALIBRE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2027:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcDecisionFraude Ecran = new UcDecisionFraude(code, text, EnumLibelleTable.LibelleDECISIONFRAUDE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2028:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcDiametre Ecran = new UcDiametre(code, text, EnumLibelleTable.LibelleDIAMETRE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2029:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcEquipements Ecran = new UcEquipements(code, text, EnumLibelleTable.LibelleEQUIPEMENT);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2030:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcEtapeFraude Ecran = new UcEtapeFraude(code, text, EnumLibelleTable.LibelleETAPEFRAUDE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2031:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcMarquesCompteur Ecran = new UcMarquesCompteur(code, text, EnumLibelleTable.LibelleMARQUECOMPTEUR);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2032:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcTypeCompteur Ecran = new UcTypeCompteur(code, text, EnumLibelleTable.LibelleTYPECOMPTEUR);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2033:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcNombreFils Ecran = new UcNombreFils(code, text, EnumLibelleTable.LibelleNOMBREFILS);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2034:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcMarquesDisjoncteur Ecran = new UcMarquesDisjoncteur(code, text, EnumLibelleTable.LibelleMARQUEDISJONCTEUR);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2035:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcReglageDisjoncteur Ecran = new UcReglageDisjoncteur(code, text, EnumLibelleTable.LibelleREGLAGEDISJONCTEUR);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2036:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcUsageProduit Ecran = new UcUsageProduit(code, text, EnumLibelleTable.LibelleUSAGEPRODUIT);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2037:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcTypeFraude Ecran = new UcTypeFraude(code, text, EnumLibelleTable.LibelleTYPEFRAUDE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2038:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcSourceControle Ecran = new UcSourceControle(code, text, EnumLibelleTable.LibelleSOURCECONTROLE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2039:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcQualiteExpert Ecran = new UcQualiteExpert(code, text, EnumLibelleTable.LibelleQUALITEEXPERT);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2040:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcFonctionFraude Ecran = new UcFonctionFraude(code, text, EnumLibelleTable.LibelleFONCTION);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2041:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcModeReglement Ecran = new UcModeReglement(code, text, EnumLibelleTable.LibelleMODEREGLEMENT);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2042:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcMoyenDenonciation Ecran = new UcMoyenDenonciation(code, text, EnumLibelleTable.LibelleMOYENDENONCIATION);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2043:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcTranche Ecran = new UcTranche(code, text, EnumLibelleTable.LibelleTRANCHE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2044:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcPrestationedm Ecran = new UcPrestationedm(code, text, EnumLibelleTable.LibellePRESTATIONEDM);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2045:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcPrestationRemboursable Ecran = new UcPrestationRemboursable(code, text, EnumLibelleTable.LibellePRESTATIONREMBOURSABLE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2046:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcRegularisation Ecran = new UcRegularisation(code, text, EnumNomTable.TYPECENTRE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }

        //            //#endregion
        //            ////Fin ATO le 30/03/2010

        //            //case 2047:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcTypeCentre Ecran = new UcTypeCentre(code, text, EnumNomTable.TYPECENTRE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            //    }
        //            //case 2048:
        //            //    {
        //            //        code = Int32.Parse(dgTable.SelectedCells[0].Value.ToString());
        //            //        text = (String)dgTable.SelectedCells[1].Value;
        //            //        UcHabilitationCheque Ecran = new UcHabilitationCheque(code, text, EnumNomTable.HABILITATIONCHEQUE);
        //            //        Ecran.ShowDialog(this.text);
        //            //        break;
        //            ////    }
        //            //default:
        //            //    {
        //            //        //throw new Exception("Inova.Common.TAB300.ManageLanguage.RM.GetString(msgCetteNestPasTraite)");
        //            //        break;
        //            //    }
        //            //}
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //        // MessageBox.Show(ex.Message, FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
          //}
//}

        private void Txt_NumeroTable_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (passageFirst)
            {
                lvwResultat.SelectedItem = ta0list.FirstOrDefault(p => p.CODE == Txt_NumeroTable.Text) ;
                lvwResultat.ScrollIntoView(lvwResultat.SelectedItem, this.lvwResultat.Columns[1]);
                lvwResultat.UpdateLayout();
                
                //foreach (aTa0 item in ta0list)
                //{
                //    if (item.CODE.ToString() == Txt_NumeroTable.Text)
                //    {
                //        //lvwResultat.SelectedItem = item;
                //        lvwResultat.SelectedIndex = 81;
                //        break;
                //    }
                //}
            }
            else
                passageFirst = true;
        }

        private void lvwResultat_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }
      }
}



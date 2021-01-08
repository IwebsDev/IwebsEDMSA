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
//using Galatee.Silverlight.serviceWeb ;
using Galatee.Silverlight.ServiceCaisse;
using Galatee.Silverlight.Resources.Caisse ;

namespace Galatee.Silverlight.Caisse
{
    public partial class FrmAutrePayement : ChildWindow
    {
        List<CsCoper> ListeCoperOD = new List<CsCoper>();
        string MontantDeposit = string.Empty;

        void translate()
        {
            try
            {
                this.lbl_Caisse.Content = Langue.Caisse;
                this.lbl_montant.Content = Langue.Montant;
                this.lbl_refClient.Content = Langue.Reference_client;
                this.lbl_Nom.Content = Langue.Nom;
                this.OKButton.Content = Langue.Btn_Payement;
                this.CancelButton.Content = Langue.Btn_annuler;
                this.rdb_In.Content = Langue.rd_Entre;
                this.rdb_Out.Content = Langue.rd_Sortie;
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }
        public FrmAutrePayement()
        {
            InitializeComponent();

            try
            {
                translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }

            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.RetourneListeDeCoperODCompleted += new EventHandler<RetourneListeDeCoperODCompletedEventArgs>(ChargerListCoperParDc);
            service.RetourneListeDeCoperODAsync();
            this.txt_Matricule.IsEnabled = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                List<CsLclient> _Lstfacture = GetFactureOD();

                UcValideEncaissement UcValider = new UcValideEncaissement(_Lstfacture, SessionObject.Enumere.ActionRecuEditionNormale);
                UcValider.Closed += new EventHandler(UcValideEncaissementClosed);
                UcValider.Show(); // UcValider.ShowDialog("Payment validation", INOVA.ISF.WINDOWS.FORMS.ExecMode.Default);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.DialogResult = true;
        }
        void UcValideEncaissementClosed(object sender, EventArgs e)
        {
            UcValideEncaissement ctrs = sender as UcValideEncaissement;

            //if (!ctrs.Yes)
                //InitControle();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.rdb_In.IsChecked = true;
        }

        private void ChargerListCoperParDc(object sender, RetourneListeDeCoperODCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled || e.Error != null || e.Result == null)
                {
                    Message.ShowError("Erreur d'invocation du service. Veuillez réessayer svp !", Langue.errorTitle);
                    return;
                }

                ListeCoperOD = new List<CsCoper>();
                ListeCoperOD.AddRange(e.Result.ToList());
                this.rdb_In.IsChecked = true;
                SelectOption();

                this.txt_Matricule.Text = UserConnecte.matricule;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }

        }

        private void rdb_In_Checked(object sender, RoutedEventArgs e)
        {
            SelectOption();
        }

        private void rdb_Out_Checked(object sender, RoutedEventArgs e)
        {
            SelectOption();
        }

        private void SelectOption()
        {
            try
            {
                if (this.rdb_In.IsChecked == true)
                {
                    List<CsCoper> _LstCoperOd = new List<CsCoper>();
                    _LstCoperOd = ListeCoperOD.Where(p => p.DC == SessionObject.Enumere.Credit && p.CODE   != SessionObject.Enumere.CoperOdQPA).ToList();
                    if (_LstCoperOd.Count > 0)
                    {
                        this.cbo_Operation.ItemsSource = _LstCoperOd;
                        this.cbo_Operation.DisplayMemberPath = "LIBELLE";
                        this.cbo_Operation.SelectedValuePath = "CODE";
                        this.cbo_Operation.SelectedItem = _LstCoperOd.First();
                    }
                }
                else
                {
                    List<CsCoper> _LstCoperOd = new List<CsCoper>();
                    _LstCoperOd = ListeCoperOD.Where(p => p.DC == SessionObject.Enumere.Debit).ToList();
                    if (_LstCoperOd.Count > 0)
                    {
                        this.cbo_Operation.ItemsSource = _LstCoperOd;
                        this.cbo_Operation.DisplayMemberPath = "LIBELLE";
                        this.cbo_Operation.SelectedValuePath = "PK_COPEROD";
                        this.cbo_Operation.SelectedItem = _LstCoperOd[0];
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cbo_Operation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CsCoper _OperationSelectionner = new CsCoper();
                _OperationSelectionner = (CsCoper)this.cbo_Operation.SelectedItem;
                if (_OperationSelectionner == null) return;

                this.txt_RefClient.Text = string.Empty;
                this.txt_montant.Text = string.Empty;
                this.txt_NomClient.Text = string.Empty;
                this.txt_IdCard.Text = string.Empty;
                if (_OperationSelectionner.CODE  == SessionObject.Enumere.CoperOdDFA)
                {
                    this.txt_RefClient.IsEnabled = false;
                    this.txt_montant.IsEnabled = false;

                    this.txt_RefClient.Text = "00100000000000000001";
                    if (string.IsNullOrEmpty(MontantDeposit))
                    {
                        CaisseServiceClient service2 = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint(this));
                        service2.RetourneListeTa58Completed += (send, es) =>
                            {
                                if (es.Cancelled || es.Error != null || es.Result == null)
                                {
                                    Message.ShowError("Erreur d'invocation du service. Veuillez réessayer svp !", Langue.errorTitle);
                                    return;
                                }

                                try
                                {
                                    MontantDeposit = es.Result.LIBELLE;
                                    this.txt_montant.Text = MontantDeposit;
                                    this.txt_NomClient.Focus();
                                }
                                catch (Exception ex)
                                {
                                    Message.ShowError(ex, Langue.errorTitle);
                                }
                            };
                        service2.RetourneListeTa58Async(SessionObject.Enumere.MontantFraisTravaux);
                    }
                    else
                    {
                        this.txt_montant.Text = MontantDeposit;
                        this.txt_NomClient.Focus();
                    }
                }
                else
                {
                    this.txt_RefClient.IsEnabled = true;
                    this.txt_montant.IsEnabled = true;

                    this.txt_RefClient.Text = string.Empty ;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }

        }
        List<CsLclient > GetFactureOD()
        {
            try
            {
                List<CsLclient> _Lstfacture = new List<CsLclient>();

                CsLclient _Lafacture = new CsLclient();
                _Lafacture.CENTRE = UserConnecte.Centre ;
                _Lafacture.CLIENT = "000000000000000";
                _Lafacture.ORDRE = "01";
                _Lafacture.NDOC = "//////";
                _Lafacture.NATURE = "01";
                _Lafacture.SOLDEFACTURE = Convert.ToDecimal(this.txt_montant.Text);
                _Lafacture.MONTANT  = Convert.ToDecimal(this.txt_montant.Text);
                _Lafacture.Selectionner = true;
                _Lafacture.REFEM = "//////";
                _Lafacture.DC = "D";
                _Lafacture.COPER = (this.cbo_Operation.SelectedItem as CsCoper).CODE ;
                _Lafacture.NOM = this.txt_NomClient.Text;
                _Lafacture.SOLDECLIENT = 0;
                _Lafacture.MATRICULE = UserConnecte.matricule;
                _Lafacture.CAISSE = UserConnecte.numcaisse;
                _Lafacture.USERCREATION = UserConnecte.matricule;
                _Lafacture.DATECREATION = DateTime.Now.Date;
                //_Lafacture.IDENTITY = txt_IdCard.Text;

                _Lstfacture.Add(_Lafacture);
                return _Lstfacture;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txt_NomClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txt_NomClient.Text.Length > 0 && txt_IdCard.Text.Length > 0 && txt_RefClient.Text.Length>0
                    && txt_montant.Text.Length >0 && txt_Matricule.Text.Length >0)
                    OKButton.IsEnabled = true;
                else
                    OKButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }
    }
}


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
using Galatee.Silverlight.ServiceCaisse;
using Galatee.Silverlight.Resources.Caisse;
//using Galatee.Silverlight.serviceWeb;

namespace Galatee.Silverlight.Caisse
{
    public partial class FrmPayementDevis : ChildWindow
    {
        CsLclient LeDevis = new CsLclient();
        private bool checkButtonStatus = false;

        public bool CheckButtonStatus
        {
            get { return checkButtonStatus; }
            set { checkButtonStatus = value; }
        }

        public FrmPayementDevis()
        {
            InitializeComponent();
            InitControl();
        }
        void translate()
        {
            lblAvance.Content = Langue.LibelleAvance;
            lblMontantPaye.Content = Langue.Montant_due;
            lblRefclient.Content = Langue.Reference_client;
            lblNumerodevis.Content = Langue.Devis_numb;
            lblMontantDevis.Content = Langue.Montant_Devis;
            Btn_RechercheDevis.Content = Langue.btn_verifier;
            OKButton.Content = Langue.Btn_ok;
            CancelButton.Content = Langue.Btn_annuler;
        }
        private void InitControl()
        {
            //RecuperationListBanque();
            this.Txt_NumDevis.MaxLength = SessionObject.Enumere.TailleNumDevis;
            this.Txt_Avance.Text = string.Empty;
            this.Txt_MontantDevis.Text = string.Empty;
            this.Txt_MontantTotal.Text = string.Empty;
            this.Txt_NomClient.Text = string.Empty;
            this.Txt_NumClient.Text = string.Empty;
            this.Txt_NumDevis.Text = string.Empty;
            this.Txt_Caissier.Text = UserConnecte.matricule;
            this.Txt_Caissier.IsEnabled = false;
            this.OKButton.IsEnabled = false;
            //translate();

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            CsLclient _Lafacture = new CsLclient();

            _Lafacture.CENTRE = LeDevis.CENTRE;
            _Lafacture.CLIENT = this.Txt_NumClient.Text;
            //_Lafacture.ORDRE = "//";
            _Lafacture.ORDRE = LeDevis.ORDRE;
            _Lafacture.MATRICULE = UserConnecte.matricule;
            _Lafacture.CAISSE = UserConnecte.numcaisse;
            _Lafacture.NDOC = "//////";
            _Lafacture.REFEM = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00"); 
            _Lafacture.NUMDEVIS = LeDevis.NUMDEVIS;
            _Lafacture.COPER = SessionObject.Enumere.CoperOdQPA;
            _Lafacture.Selectionner = true;
            _Lafacture.DC = SessionObject.Enumere.Debit;
            _Lafacture.SOLDEFACTURE = Convert.ToDecimal(this.Txt_MontantTotal.Text);
            _Lafacture.NOM = this.Txt_NomClient.Text;
            //_Lafacture.FK_IDETAPEDEVIS = LeDevis.FK_IDETAPEDEVIS;
            //_Lafacture.FK_IDPRODUIT = LeDevis.FK_IDPRODUIT;
            //_Lafacture.FK_IDTYPEDEVIS = LeDevis.FK_IDTYPEDEVIS;
            _Lafacture.FK_IDCENTRE  = LeDevis.FK_IDCENTRE;
            //_Lafacture.EXIGIBILITE = LeDevis.EXIGIBILITE;
            List<CsLclient> _ListeDeFacture = new List<CsLclient>();
            _ListeDeFacture.Add(_Lafacture);


            UcValideEncaissement UcValider = new UcValideEncaissement(_ListeDeFacture, SessionObject.Enumere.OperationDeCaisseEncaissementDevis);
            UcValider.Closed += new EventHandler(UcValideEncaissementClosed);
            UcValider.Show();
            //if (UcValider.ValidationEditionfacture == true)
            //{
            //    CaisseServiceClient service1 = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint(this));
            //    service1.MiseAJourDevisCompleted += (send, es) =>
            //    {

            //    };
            //    service1.MiseAJourDevisAsync(this.Txt_NumDevis.Text, this.Txt_Caissier.Text);
            //    service1.CloseAsync();

            //}
            this.DialogResult = true;
        }

        void UcValideEncaissementClosed(object sender, EventArgs e)
        {
            try
            {
                // prevoit initialiser le controle de devis
                InitControl();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }

        }

        private void RecuperationListBanque()
        {

            if (SessionObject.ListeBanques != null || SessionObject.ListeBanques.Count > 0)
                return;

            CaisseServiceClient srv = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            srv.RetourneListeDesBanquesCompleted += (ss, ee) =>
            {
                try
                {
                    if (ee.Cancelled || ee.Error != null || ee.Result == null)
                    {
                        string error = ee.Error.InnerException.ToString();
                        return;
                    }

                    //Assignation de la variable de session contenant la liste des banques
                    SessionObject.ListeBanques = ee.Result;
                    if (SessionObject.ListeBanques == null || SessionObject.ListeBanques.Count == 0)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
            };
            srv.RetourneListeDesBanquesAsync();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        private void AfficheDevis()
        {

            if (!string.IsNullOrEmpty(this.Txt_NumDevis.Text))
            {
                int devis = 0;
                if (int.TryParse(Txt_NumDevis.Text, out devis))
                {
                    string format = "00000000";
                    this.Txt_NumDevis.Text = (devis).ToString(format);
                }
                else
                {
                    Message.ShowInformation("Le format du numéro dévis n'est pas valide.Veuillez réessayer svp", "Information");
                    return;
                }

                CaisseServiceClient service2 = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                service2.RetourneLeDevisCompleted += (send, es) =>
                {
                    try
                    {
                        if (es.Cancelled || es.Error != null)
                        {
                            Message.ShowError(Langue.msg_error_remote, Langue.errorTitle);
                            return;
                        }

                        if (es.Result == null)
                        {
                            Message.ShowInformation("Ce numéro de devis n'est pas connu du système. Veuillez resaisir svp !", Langue.errorTitle);
                            return;
                        }


                        LeDevis = new CsLclient ();
                        LeDevis = es.Result;
                        if (!string.IsNullOrEmpty(LeDevis.CLIENT))
                        {
                            //if( LeDevis.EXIGIBLE == null)
                            //{
                            //if (LeDevis.NUMETAPE == SessionObject.Enumere.EtapeEncaissementDevis)
                            //{
                            //    this.Txt_NomClient.Text = LeDevis.NOM;
                            //    this.Txt_NumClient.Text = LeDevis.CLIENT;
                            //    this.Txt_MontantDevis.Text =Convert.ToDecimal( LeDevis.MONTANT).ToString(SessionObject.FormatMontant );
                            //    this.Txt_Avance.Text = Convert.ToDecimal(LeDevis.AVANCE).ToString(SessionObject.FormatMontant);
                            //    this.Txt_MontantTotal.Text = Convert.ToDecimal(LeDevis.MONTANT).ToString(SessionObject.FormatMontant);

                            //}
                            //else
                            //    Message.ShowInformation(Langue.MsgEtapeDevis, "Info");

                            //}
                            //else
                            //    Message.ShowInformation(SessionObject.Enumere.MessageDevisRegler,"Info");
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service2.RetourneLeDevisAsync(this.Txt_NumDevis.Text);
            }
        }

        private void Btn_RechercheDevis_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AfficheDevis();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void Txt_NomClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_NomClient.Text))
                this.OKButton.IsEnabled = true;
        }

        private void Txt_NumDevis_TextChanged(object sender, TextChangedEventArgs e)
        {
            //int devis = 0;
            //Txt_NumDevis.Text.PadLeft(SessionObject.Enumere.TailleNumDevis, '0');
            //if (int.TryParse(Txt_NumDevis.Text, out devis))
            //    if (devis > 0)
            //        //if(Txt_NumDevis.Text.Length == SessionObject.Enumere.TailleNumDevis)
            //        Btn_RechercheDevis.IsEnabled = true;
            //    else
            //        Btn_RechercheDevis.IsEnabled = false;
            //try
            //{
            //    if (Txt_NumDevis.Text.Length == SessionObject.Enumere.TailleNumDevis)
            //        Btn_RechercheDevis.IsEnabled = true;
            //    else
            //        Btn_RechercheDevis.IsEnabled = false;
            //}
            //catch (Exception ex)
            //{
            //    Message.ShowError(ex, "Erreur");
            //}
        }

        private void Txt_MontantTotal_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Txt_MontantTotal.Text.Length > 0)
                OKButton.IsEnabled = true;
            else
                OKButton.IsEnabled = false;
        }

        private void Txt_NumDevis_KeyUp_1(object sender, KeyEventArgs e)
        {
            try
            {
                //int devis = 0;
                
                ////Txt_NumDevis.Text.PadLeft(SessionObject.Enumere.TailleNumDevis, '0');
                //if (int.TryParse(Txt_NumDevis.Text, out devis))
                //{
                //    string format = "00000000";
                //    this.Txt_NumDevis.Text = (devis).ToString(format);

                //    if (devis > 0)
                //        //if(Txt_NumDevis.Text.Length == SessionObject.Enumere.TailleNumDevis)
                //        Btn_RechercheDevis.IsEnabled = true;
                //    else
                //        Btn_RechercheDevis.IsEnabled = false;
                //}
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
    }
}


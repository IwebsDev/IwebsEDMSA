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
using Galatee.Silverlight.ServiceRecouvrement;
using System.ComponentModel;
using Galatee.Silverlight.Caisse;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmSaisiDesChequesImpaye : ChildWindow
    {
        string NumCheq = string.Empty;
        string banque = string.Empty;
        string guichet = string.Empty;
        Galatee.Silverlight.ServiceCaisse.CsBanque Labanque = new ServiceCaisse.CsBanque();
        public FrmSaisiDesChequesImpaye()
        {
            InitializeComponent();
            try
            {
                RecuperationListBanque();
                InitialiserControle();
                ChargerMotifRejetCheque();
                //Cbo_MotifRejet.IsEnabled = false;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                chkrchecFee.IsChecked = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

            private void InitialiserControle()
            {
 	            try 
	            {
                    Txt_ChequeNumber.IsEnabled = false;
                    Btn_Search.IsEnabled = false;
                    OKButton.IsEnabled = false;

	            }
	            catch (Exception ex)
	            {
                    Message.ShowError(ex, "Erreur");
	            }
            }

        bool result = false;
        bool ApplyFees = false;

        string ObservationCheque = string.Empty;

        CsLclient LClient = null;
        CsLclient LClientFrais;

        List<CsLclient> ClientsImpSansFrais = null;
        List<CsLclient> clientChecqImp = new List<CsLclient>();
        List<CsLclient> ltrie = null;

        private void ChargerMotifRejetCheque()
        {
            try
            {
                if (SessionObject.LstMotifRejetsCheque.Count != 0)
                {
                    Cbo_MotifRejet.ItemsSource = null;
                    Cbo_MotifRejet.ItemsSource = SessionObject.LstMotifRejetsCheque;
                    Cbo_MotifRejet.DisplayMemberPath = "LIBELLE";

                    return;
                }
                RecouvrementServiceClient service = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.RetourneMotifChequeImpayeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstMotifRejetsCheque = args.Result;

                    Cbo_MotifRejet.ItemsSource = null;
                    Cbo_MotifRejet.ItemsSource = SessionObject.LstMotifRejetsCheque;
                    Cbo_MotifRejet.DisplayMemberPath = "LIBELLE";

                    return;
                };
                service.RetourneMotifChequeImpayeAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RecuperationListBanque()
        {
            try
            {
                //int loaderHandler = LoadingManager.BeginLoading(Langue.Data_Loading);
                if (SessionObject.ListeBanques != null && SessionObject.ListeBanques.Count != 0)
                {

                    List<Galatee.Silverlight.ServiceCaisse.CsBanque> lbanque = new List<Galatee.Silverlight.ServiceCaisse.CsBanque>();
                    lbanque.AddRange(SessionObject.ListeBanques);
                    if (lbanque != null && lbanque.Count > 0)
                    {
                        this.Cbo_Bank.ItemsSource = lbanque;
                        this.Cbo_Bank.DisplayMemberPath = "LIBELLE";
                        return;
                    }
                }
                Galatee.Silverlight.ServiceCaisse.CaisseServiceClient srv = new Galatee.Silverlight.ServiceCaisse.CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
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
                        if (SessionObject.ListeBanques != null || SessionObject.ListeBanques.Count != 0)
                        {

                            List<Galatee.Silverlight.ServiceCaisse.CsBanque> lbanque = new List<Galatee.Silverlight.ServiceCaisse.CsBanque>();
                            lbanque.AddRange(SessionObject.ListeBanques);
                            if (lbanque != null && lbanque.Count > 0)
                            {
                                this.Cbo_Bank.ItemsSource = lbanque;
                                this.Cbo_Bank.DisplayMemberPath = "LIBELLE";
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                    }
                    finally
                    {
                        //LoadingManager.EndLoading(loaderHandler);
                    }
                };
                srv.RetourneListeDesBanquesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyFees = false;
            LClient = null;
            LClientFrais = new CsLclient();
            try
            {

                LClient = new CsLclient();
                if (this.Cbo_MotifRejet.SelectedItem == null)
                {
                    Message.Show("Veuillez saisir le motif", "Recouvrement");
                    return;
                }
                if (Labanque.FRAISDERETOUR == null)
                {
                    Message.Show("La banque sélectionnée n'a pas de frais paramétré", "Recouvrement");
                    return;
                }



                var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Confirmez-vous la mise à jour de la demande ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                messageBox.OnMessageBoxClosed += (at, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {



                        List<CsLclient> ClientDuCheck = new List<CsLclient>();
                        ClientDuCheck = Lsv_DetailCheque.ItemsSource as List<CsLclient>;
                        List<CsLclient> lstFactureCheque = ObtenirChequeImpaye(ClientDuCheck);
                        if (chkrchecFee.IsChecked == true && Labanque.FRAISDERETOUR > 0)
                            lstFactureCheque.Add(ObtenirFaisChequeImpaye(Shared.ClasseMEthodeGenerique.RetourneCopyObjet<CsLclient>(ClientDuCheck.First())));

                        OKButton.IsEnabled = false;
                        Btn_Search.IsEnabled = false;
                        prgBar.Visibility = System.Windows.Visibility.Visible;
                        RecouvrementServiceClient proxy = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                        proxy.InsertChecqueImpayesCompleted += (_, ssr) =>
                        {
                            try
                            {
                                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                if (ssr.Cancelled || ssr.Error != null)
                                {
                                    Message.ShowError("Erreur survenue lors de l'appel service.", "Erreur");
                                    OKButton.IsEnabled = true;
                                    Btn_Search.IsEnabled = true;
                                    return;
                                }
                                if (ssr.Result == null)
                                {
                                    Message.ShowError("Erreur d'insertion des impayés du client. Réessayer svp!", "Info");
                                    OKButton.IsEnabled = true;
                                    Btn_Search.IsEnabled = true;
                                }

                                Message.Show("Mise à jour du compte client effectuée avec succès. ", "Recouvrement");
                                InitialiserLeControle();
                            }
                            catch (Exception ex)
                            {
                                OKButton.IsEnabled = true;
                                Btn_Search.IsEnabled = true;
                                Message.ShowError(ex, "Erreur");
                            }
                        };
                        proxy.InsertChecqueImpayesAsync(ClientDuCheck);


                    }
                    else
                    {
                        return;
                    }
                };
                messageBox.Show();


            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                Message.ShowError(ex, "Erreur");
            }
        }



        List<CsLclient> ObtenirChequeImpaye(List<CsLclient> _LeChequeImpaye)
        {
            foreach (var item in _LeChequeImpaye)
            {
                item.USERCREATION = UserConnecte.matricule;
                item.DATECREATION = System.DateTime.Now;
                item.DENR  = System.DateTime.Now.Date;
                item.TOP1 = SessionObject.Enumere.TopCheqImpaye;
                item.DC = SessionObject.Enumere.Debit;
                item.COPER = SessionObject.Enumere.CoperChqImp;
                item.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperChqImp).PK_ID;
                item.FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TopCheqImpaye).PK_ID;
                item.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                item.FK_IDMOTIFCHEQUEINPAYE  =(int)this.Cbo_MotifRejet.Tag;  
            }
             return _LeChequeImpaye;
        }
        CsLclient ObtenirFaisChequeImpaye(CsLclient item)
        {
            item.MONTANT = Labanque.FRAISDERETOUR;
            item.USERCREATION = UserConnecte.matricule;
            item.DATECREATION = System.DateTime.Now;
            item.DENR = System.DateTime.Now.Date;
            item.TOP1 = SessionObject.Enumere.TopCheqImpaye;
            item.DC = SessionObject.Enumere.Debit;
            item.COPER = SessionObject.Enumere.CoperFraisChqImp;
            item.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperFraisChqImp).PK_ID;
            item.FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TopCheqImpaye).PK_ID;
            item.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
            item.FK_IDMOTIFCHEQUEINPAYE = (int)this.Cbo_MotifRejet.Tag;
            return item;
        }
        void InitialiserLeControle()
        {
            try
            {
                Cbo_Bank.SelectedItem = null;
                Txt_ChequeNumber.Text = string.Empty;
                Lsv_DetailCheque.ItemsSource = null;
                LClientFrais = null;

                   ClientsImpSansFrais = null;
                   clientChecqImp = null;
                   ltrie = null;
                   chkrchecFee.IsChecked = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

 

     

        void VerifSaisieInformation()
        {
            try
            {
                if (string.IsNullOrEmpty(Txt_ChequeNumber.Text) == false && Cbo_Bank.SelectedItem != null)
                    Btn_Search.IsEnabled = true;
                else
                    Btn_Search.IsEnabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void btnReinitialiser_Click(object sender, EventArgs e)
        {
            //this.lvwResultat.ItemsSource=null;
            //this.cmbCentre.SelectedValue = string.Empty;
            //this.txtCampagne.Text = string.Empty;
            //this.cmbAgent.SelectedValue = string.Empty;
            //this.dtpDate.Text = null;
        }



        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        void Recherche()
        {
            List<CsLclient> l = null;
            ltrie = new List<CsLclient>();
            NumCheq = string.Empty;
            banque = string.Empty;
            guichet = string.Empty;
            try
            {

                if (!string.IsNullOrEmpty(Txt_ChequeNumber.Text))
                    NumCheq = Txt_ChequeNumber.Text;

                if (Cbo_Bank.SelectedItem != null)
                {
                    Labanque = (Galatee.Silverlight.ServiceCaisse.CsBanque)Cbo_Bank.SelectedItem;
                    banque = Labanque.CODE;
                    //guichet = _banque.CODEGUICHET;
                }
                OKButton.IsEnabled = false;
                Btn_Search.IsEnabled = false;
                prgBar.Visibility = System.Windows.Visibility.Visible;

                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                client.RetourneClientsDuChecqheCompleted += (ssen, ars) =>
                    {
                        try
                        {
                            if (ars.Cancelled || ars.Error != null)
                            {
                                Message.ShowError("Erreur survenue lors de l'appel service", "Erreur");
                                Lsv_DetailCheque.ItemsSource = null;
                                OKButton.IsEnabled = true;
                                Btn_Search.IsEnabled = true;
                                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                return;
                            }

                            if (ars.Result == null || ars.Result.Count == 0)
                            {
                                Message.ShowInformation("Aucun encaissement n'a été effectué sur ce chèque ", "Information");
                                Lsv_DetailCheque.ItemsSource = null;
                                OKButton.IsEnabled = true;
                                Btn_Search.IsEnabled = true;
                                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                return;
                            }

                            /*if (ars.Result != null &&  && ars.Result.FirstOrDefault().REFEMNDOC =="OUI")
                            {
                                Message.ShowInformation("Ce chèque a déjà été saisi. ", "Information");
                                Lsv_DetailCheque.ItemsSource = null;
                                OKButton.IsEnabled = true;
                                Btn_Search.IsEnabled = true;
                                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                                return;
                            }

                            l = new List<CsLclient>();
                            l.AddRange(ars.Result);
                            Cbo_MotifRejet.IsEnabled = true;

                            Lsv_DetailCheque.ItemsSource = null;
                            Lsv_DetailCheque.ItemsSource = l;
                            Lsv_DetailCheque.SelectedItem = l.First();
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            */

                            if (ars.Result != null && ars.Result.Count > 0)
                            {
                                l = new List<CsLclient>();
                                l.AddRange(ars.Result.Where(d => d.FK_IDMOTIFCHEQUEINPAYE == 0).ToList());

                                if (l != null && l.Count > 0)
                                {
                                    if (l.Count < ars.Result.Count)
                                        Message.ShowInformation("Une saisie partielle a déjà été faite pour ce chèque. ", "Information");

                                    Cbo_MotifRejet.IsEnabled = true;

                                    Lsv_DetailCheque.ItemsSource = null;
                                    Lsv_DetailCheque.ItemsSource = l;
                                    Lsv_DetailCheque.SelectedItem = l.First();

                                }
                                else
                                    Message.ShowInformation("Ce chèque a déjà été saisi. ", "Information");

                                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                            }


                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex, "Erreur");
                            OKButton.IsEnabled = true;
                            Btn_Search.IsEnabled = true;
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;

                        }

                    };
                client.RetourneClientsDuChecqheAsync(NumCheq, banque, guichet);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }



        private void btnsearch_Click(object sender, RoutedEventArgs e)
         {
             try
             {
                 OKButton.IsEnabled = false;
                 Recherche();
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, "Erreur");
             }
         }

        private void btnreset_Click(object sender, RoutedEventArgs e)
         {

         }

        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             try
             {
                 this.VerifSaisieInformation();
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, "Erreur");
             }
         }

        private void Txt_Client_TextChanged(object sender, TextChangedEventArgs e)
         {
             try
             {
                 this.VerifSaisieInformation();
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, "Erreur");
             }
         }

        private void Txt_Ordre_TextChanged(object sender, TextChangedEventArgs e)
         {
             try
             {
                 this.VerifSaisieInformation();
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, "Erreur");
             }
         }

        private void Cbo_Bank_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             try
             {
                 Txt_ChequeNumber.IsEnabled = true  ;
                 this.VerifSaisieInformation();
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, "Erreur");
             }
         }

        private void Txt_ChequeNumber_TextChanged(object sender, TextChangedEventArgs e)
         {
             try
             {
                 this.VerifSaisieInformation();
             }
             catch (Exception ex)
             {
                 Message.ShowError(ex, "Erreur");
             }
         }

        private void Lsv_DetailCheque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OKButton.IsEnabled = true;
        }

        private void Cbo_MotifRejet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Cbo_MotifRejet.Tag = ((CsMotifChequeImpaye)this.Cbo_MotifRejet.SelectedItem).PK_ID;
        }
    }
}


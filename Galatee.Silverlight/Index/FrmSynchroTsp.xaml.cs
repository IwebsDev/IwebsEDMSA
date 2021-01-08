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
using Galatee.Silverlight.ServiceFacturation  ;
using Galatee.Silverlight.Resources.Index;
using Galatee.Silverlight.Resources.Accueil ;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmSynchroTsp : ChildWindow
    {
        List<CsLotri> listeBatch = new List<CsLotri>();
        CsLotri leBatchSelect = new CsLotri();
        List<CsLotri> ListeDesTourneeLot = new List<CsLotri>();
        List<CsLotri> ListeDesLotriAfficher = new List<CsLotri>();
        List<CsLotri> _ListeLotri = new List<CsLotri>();

        public FrmSynchroTsp()
        {
            InitializeComponent();
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                this.Txt_NumBatch.MaxLength = SessionObject.Enumere.TailleNumeroBatch;
                this.Btn_Batch.IsEnabled = false;
                translate();
                ChargerDonneeDuSite();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void translate()
        {
            this.Title = Galatee.Silverlight.Resources.Index.Langue.trt_critereSaisiePage;
            this.Btn_Batch.Content = Galatee.Silverlight.Resources.Index.Langue.btn_batch;

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<CsLotri> _lstLotSelect = new List<CsLotri>();
                CsLotri _leCritere = new CsLotri();
                if (this.Txt_NumBatch.Tag != null)
                    ChargerEvenementTransfertListeLotri((CsLotri)this.Txt_NumBatch.Tag);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void DialogInfoClosed(object sender, EventArgs e)
        {
            try
            {
                DialogResult ctrs = sender as DialogResult;
                if (ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
                {
                    ctrs.DialogResult = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ucgReturn(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique uc = sender as MainView.UcListeGenerique;
                if (uc.GetisOkClick)
                {
                    CsLotri lot = (CsLotri)uc.MyObject;
                    Txt_NumBatch.Text = lot.NUMLOTRI;
                    this.Txt_NumBatch.Tag = lot;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }


        private void btn_batch_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listeBatch != null)
                {
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("NUMLOTRI", "LOT");
                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(listeBatch);
                    MainView.UcListeGenerique ucg = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                    ucg.Closed += new EventHandler(ucgReturn);
                    ucg.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void Txt_NumBatch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Txt_NumBatch.Text.Length == SessionObject.Enumere.TailleNumeroBatch)
            {
                if (listeBatch.FirstOrDefault(t => t.NUMLOTRI == this.Txt_NumBatch.Text) == null)
                {
                    Message.ShowInformation("Lot inexistant ", "Lot");
                    return;
                }
                else
                    Txt_NumBatch.Tag = listeBatch.FirstOrDefault(t => t.NUMLOTRI == this.Txt_NumBatch.Text);
            }

        }

        List<int> IdDesCentre = new List<int>();
        List<ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);

                    ChargerDistinctListeLotri(IdDesCentre);
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void MiseAJourEvenement(List<CsEvenement > lstEvenementCenetre)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.MisAJourEvenementSynchoTSPCompleted += (s, args) =>
                {
                    try
                    {
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;

                        if (args.Error != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur d'invocation du service.", "Erreur");
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("Aucune donnée retournée du système.", "Erreur");
                            return;
                        }
                        if (args.Result == true)
                        {
                            Message.ShowInformation ("Synchronisation terminé.", "Erreur");
                            return;
                        }
                        if (args.Result == false )
                        {
                            Message.ShowError("Erreur à la synchronisation .", "Erreur");
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.MisAJourEvenementSynchoTSPAsync(lstEvenementCenetre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChargerDistinctListeLotri(List<int> lstIdCenetre)
        {
            _ListeLotri = new List<CsLotri>();
            try
            {

                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ChargerDistinctLotriTransfertspCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Error != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur d'invocation du service.", "Erreur");
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("Aucune donnée retournée du système.", "Erreur");
                            return;
                        }
                        Btn_Batch.IsEnabled = true;
                        listeBatch = args.Result;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.ChargerDistinctLotriTransfertspAsync(lstIdCenetre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<CsEvenement> lstEvtTransf;
        private void ChargerEvenementTransfertListeLotri(CsLotri leLot)
        {
            try
            {

                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ChargerListeDesTransfertspCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Error != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur d'invocation du service.", "Erreur");
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("Aucune donnée retournée du système.", "Erreur");
                            return;
                        }
                        lstEvtTransf = new List<CsEvenement>();
                        Btn_Batch.IsEnabled = true;
                        lstEvtTransf = args.Result;
                        if (lstEvtTransf != null && lstEvtTransf.Count != 0)
                            dataGrid1.ItemsSource = null;
                        dataGrid1.ItemsSource = lstEvtTransf;

                        this.txt_TotalLot.Text = lstEvtTransf.Count != 0 ? lstEvtTransf.Count().ToString() : "0";
                        this.txt_TotalSaisie.Text = lstEvtTransf.Count != 0 ? lstEvtTransf.Where(t => t.CAS  != "##").Count().ToString() : "0";

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.ChargerListeDesTransfertspAsync(leLot);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Btn_Batch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("NUMLOTRI");
                _LstColonneAffich.Add("PERIODE");
                List<CsLotri> ListeDesLotriCentre = ListeDesLotriAfficher.ToList();
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(ListeDesLotriCentre);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                ctrl.Closed += new EventHandler(galatee_OkClickedBatch);
                ctrl.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        void galatee_OkClickedBatch(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
                CsLotri param = (CsLotri)ctrs.MyObject;//.VALEUR;
                this.Txt_NumBatch.Text = param.NUMLOTRI;
                this.Txt_NumBatch.Tag  = param;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btn_Synchro_Click(object sender, RoutedEventArgs e)
        {
            if (int.Parse (this.txt_TotalLot.Text) == int.Parse (this.txt_TotalSaisie.Text))
            MiseAJourEvenement(lstEvtTransf);
            else
              Message.ShowInformation ("Terminer la saisie du lot avant de synchroniser","Facturation");

        }
    }
}


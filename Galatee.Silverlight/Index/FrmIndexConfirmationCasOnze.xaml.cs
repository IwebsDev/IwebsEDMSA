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
using Galatee.Silverlight.ServiceFacturation ;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Resources.Index;


namespace Galatee.Silverlight.Facturation
{
    public partial class FrmIndexConfirmationCasOnze : ChildWindow
    {

        List<CsLotri> listeBatch = new List<CsLotri>();
        CsLotri leBatchSelect = new CsLotri();
        List<CsLotri> ListeDesTourneeLot = new List<CsLotri>();

        List<CsLotri> ListeDesSelectTourneeLot = new List<CsLotri>();
        List<CsLotri> ListeDesSelectCentreLot = new List<CsLotri>();

        public FrmIndexConfirmationCasOnze()
        {
            InitializeComponent();
            Translate();
            ChargerDonneeDuSite();

            this.btn_batch.IsEnabled = false;
            this.Txt_NumBatch.IsReadOnly = true;

            this.btn_Centre.IsEnabled = false;
            this.Txt_Centre.IsReadOnly = true;

            this.btn_tournee .IsEnabled = false;
            this.Txt_zone.IsReadOnly = true;
        }

        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    List<int> IdDesCentre = new List<int>();
                    List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    List<ServiceAccueil.CsSite> lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);
                    Chargelotri(IdDesCentre);
                    return;

                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<int> IdDesCentre = new List<int>();
                    List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    List<ServiceAccueil.CsSite> lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);
                    Chargelotri(IdDesCentre);

                };
                service.ListeDesDonneesDesSiteAsync(true);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            List<CsEvenement> lstEvtAqValider = new List<CsEvenement>();
            List<CsEvenement> EvtValider = (List<CsEvenement>)dataGrid2.ItemsSource;
            foreach (CsEvenement item in EvtValider)
                lstEvtAqValider.AddRange(lesEquete.Where(t => t.CENTRE == item.CENTRE && t.CLIENT == item.CLIENT && t.ORDRE == item.ORDRE && t.PRODUIT == item.PRODUIT));

            if (Chk_FacturationNormal.IsChecked == true) lstEvtAqValider.ForEach(t => t.CAS = "87");
            lstEvtAqValider.ForEach(t => t.ENQUETE = "C");
            ValiderEnqute(lstEvtAqValider);
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btn_batch.IsEnabled = false;
                btn_tournee .IsEnabled = false;
                OKButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void Translate()
        {
            //this.btn_Batch.Content = Langue.btn_batch;
            this.btn_tournee .Content = Langue.btn_tournee;
        }

        private void btn_Recherche_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListeDesEnquete();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        List<CsEvenement> lesEquete = new List<CsEvenement>();
        private void ListeDesEnquete()
        {
            try
            {
                this.dataGrid1.ItemsSource = null;
                this.dataGrid2.ItemsSource = null;
                List<CsLotri> ListeTourneeSelectDuLot = new List<CsLotri>();

                if (this.Txt_NumBatch.Tag != null)
                    ListeTourneeSelectDuLot = ListeDesTourneeLot.Where(t => t.NUMLOTRI == ((CsLotri)this.Txt_NumBatch.Tag).NUMLOTRI).ToList();
                else
                {
                    Message.ShowInformation("Selectionnez un lot", "Facturation");
                    return;
                }
                if (ListeDesSelectCentreLot.Count != 0)
                {
                    List<int> LstIdCentre = new List<int>();
                    foreach (CsLotri item in ListeDesSelectCentreLot)
                        LstIdCentre.Add(item.FK_IDCENTRE);
                    ListeTourneeSelectDuLot = ListeTourneeSelectDuLot.Where(t =>LstIdCentre.Contains(t.FK_IDCENTRE)).ToList();

                }
                if (ListeDesSelectTourneeLot.Count != 0)
                {
                    List<int?> LstIdTournee = new List<int?>();
                    foreach (CsLotri item in ListeDesSelectTourneeLot)
                        LstIdTournee.Add(item.FK_IDTOURNEE );
                    ListeTourneeSelectDuLot = ListeTourneeSelectDuLot.Where(t => LstIdTournee.Contains(t.FK_IDTOURNEE)).ToList();

                }
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ListeDesEnqueteCasOnzeCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Error != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel serveur", "Erreur");
                            return;
                        }
                        if (args.Result != null && args.Result.Count  != 0)
                        {
                            List<CsEvenement> lesEqueteAffich = new List<CsEvenement>();
                            lesEquete = args.Result.OrderBy(t => t.CENTRE).ThenBy(y => y.TOURNEE).ThenBy(o => o.ORDTOUR).ToList();
                            var Equt = lesEquete.Select(j => new { j.CENTRE, j.CLIENT, j.ORDRE, j.PRODUIT,j.CAS  }).Distinct();
                            foreach (var item in Equt)
                                lesEqueteAffich.Add(new CsEvenement { CENTRE = item.CENTRE ,CLIENT =item.CLIENT ,ORDRE = item.ORDRE ,PRODUIT = item.PRODUIT ,CAS =item.CAS  });
                            dataGrid1.ItemsSource = null;
                            dataGrid1.ItemsSource = lesEqueteAffich;
                        }
                        else
                        {
                            Message.ShowError("Aucune enquète trouvée", "Facturation");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.ListeDesEnqueteCasOnzeAsync(ListeTourneeSelectDuLot);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValiderEnqute(List<CsEvenement> lstEnqueteConfirme)
        {
            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ValiderEnqueteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Error != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors du traitement", "Erreur");
                            return;
                        }
                        Message.ShowInformation(Langue.MsgEnqueteValider, Langue.libelleModule);
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.ValiderEnqueteAsync(lstEnqueteConfirme);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<CsEvenement> ListeSelect = ((List<CsEvenement>)this.dataGrid1.ItemsSource).Where(t => t.IsSaisi == true).ToList();
            foreach (CsEvenement item in ListeSelect)
            {
                item.IsSaisi = false;
                dataGrid1.SelectedItems.Add(item);
            }
            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsEvenement>(dataGrid1, dataGrid2);
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            List<CsEvenement> ListeSelect = ((List<CsEvenement>)this.dataGrid2.ItemsSource).Where(t => t.IsSaisi == true).ToList();
            foreach (CsEvenement item in ListeSelect)
            {
                item.IsSaisi = false;
                dataGrid2.SelectedItems.Add(item);
            }
            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsEvenement>(dataGrid2,dataGrid1);
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listeBatch != null)
                {
                    CsLotri leLotSelect = new CsLotri();
                    if (this.Txt_NumBatch.Tag != null)
                        leLotSelect = (CsLotri)this.Txt_NumBatch.Tag;
                    List<CsLotri> _lstLotAfficher = ClasseMethodeGenerique.RetourneDistinctCentre(listeBatch.Where(t => t.NUMLOTRI == leLotSelect.NUMLOTRI && t.PERIODE == leLotSelect.PERIODE).ToList());
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CENTRE", "CENTRE");
                    _LstColonneAffich.Add("LIBELLECENTRE", "CENTRE");

                    foreach (CsLotri item in _lstLotAfficher)
                    {
                        item.CODE = item.CENTRE;
                        item.LIBELLE = item.LIBELLECENTRE;
                    }
                    UcGenerique ctrl = new UcGenerique(_lstLotAfficher.OrderBy(t => t.CENTRE).ToList(), true, "Liste des centres");
                    ctrl.Closed += new EventHandler(ucgCentre);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void Chargelotri(List<int> lstCentreHAbilite)
        {
            FacturationServiceClient client = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
            client.RetourneListeLotNonTraiteCompleted += (_, es) =>
            {
                try
                {
                    if (es != null && es.Cancelled)
                    {
                        Message.ShowError("Lot non trouvé", "Facturation");
                        return;
                    }

                    if (es.Result == null || es.Result.Count == 0)
                    {
                        Message.ShowError("Lot non trouvé", "Facturation");
                        return;
                    }

                    if (es.Result != null && es.Result.Count != 0)
                        listeBatch =  ClasseMethodeGenerique.RetourneDistinctNumLot(es.Result);

                    ListeDesTourneeLot = new List<CsLotri>();
                    if (listeBatch.Count != 0)
                        ListeDesTourneeLot =ClasseMethodeGenerique.RetourneDistinctTournee(listeBatch);

                    this.btn_batch.IsEnabled = true;
                    this.Txt_Centre.IsReadOnly = false;
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }
            };
            client.RetourneListeLotNonTraiteAsync(lstCentreHAbilite);

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
                    if (listeBatch.Where(u=>u.NUMLOTRI ==Txt_NumBatch.Text).Select(t => new { t.FK_IDCENTRE}).Distinct().ToList().Count() == 1)
                    {
                        this.Txt_Centre.Text = listeBatch.First().CENTRE;
                        this.Txt_Centre.Tag = listeBatch.First();
                      
                    }
                    this.btn_Centre.IsEnabled = true;
                    this.Txt_Centre.IsReadOnly = false;
                    this.Txt_Centre.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void ucgCentre(object sender, EventArgs e)
        {
            ListeDesSelectCentreLot.Clear();
            UcGenerique ctrs = sender as UcGenerique;
            if (ctrs.isOkClick)
            {
                List<CsLotri> LesCentreeDuLot = (List<CsLotri>)ctrs.MyObjectList;
                if (LesCentreeDuLot != null && LesCentreeDuLot.Count > 0)
                {
                    int passage = 1;
                    foreach (CsLotri item in LesCentreeDuLot)
                    {
                        if (passage == 1)
                            this.Txt_Centre.Text = item.CENTRE;
                        else
                            this.Txt_Centre.Text = this.Txt_Centre.Text + "  " + item.CENTRE;
                        passage++;

                    }
                    this.Txt_Centre.IsReadOnly = true;
                    this.Txt_Centre.Tag = LesCentreeDuLot;
                    ListeDesSelectCentreLot = LesCentreeDuLot;
                    this.btn_tournee.IsEnabled = true;
                    this.Txt_zone.IsReadOnly = false;
                    this.Txt_zone.Text = string.Empty;
                }
            }
        }

        //private void ucgCentre(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        MainView.UcListeGenerique uc = sender as MainView.UcListeGenerique;
        //        if (uc.GetisOkClick)
        //        {
        //            CsLotri lot = (CsLotri)uc.MyObject;
        //            this.Txt_Centre.Text = lot.CENTRE;
        //            this.Txt_Centre.Tag = lot;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex, "Erreur");
        //    }
        //}
        private void btn_tournee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CENTRE");
                _LstColonneAffich.Add("TOURNEE");
                if (this.Txt_NumBatch.Tag != null)
                {
                    List<int> lsidCentre = new List<int>();
                    foreach (CsLotri item in ListeDesSelectCentreLot)
                        lsidCentre.Add(item.FK_IDCENTRE);

                    List<CsLotri> lstLotSelect = ListeDesTourneeLot.Where(p => p.NUMLOTRI == this.Txt_NumBatch.Text && lsidCentre.Contains(p.FK_IDCENTRE)).OrderBy(t => t.CENTRE).ThenBy(y => y.TOURNEE).ToList();
                    foreach (CsLotri item in lstLotSelect)
                    {
                        item.CODE = item.CENTRE;
                        item.LIBELLE = item.TOURNEE;
                    }
                    UcGenerique ctrl = new UcGenerique(lstLotSelect, true, "Liste des tournées");
                    ctrl.Closed += new EventHandler(galatee_OkClickedBtnZone1);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        void galatee_OkClickedBtnZone1(object sender, EventArgs e)
        {
            ListeDesSelectTourneeLot.Clear();
            UcGenerique ctrs = sender as UcGenerique;
            if (ctrs.isOkClick)
            {
                List<CsLotri> LestourneeDuLot = (List<CsLotri>)ctrs.MyObjectList;
                if (LestourneeDuLot != null && LestourneeDuLot.Count > 0)
                {
                    int passage = 1;
                    foreach (CsLotri item in LestourneeDuLot)
                    {
                        if (passage == 1)
                            this.Txt_zone.Text = item.TOURNEE;
                        else
                            this.Txt_zone.Text = this.Txt_zone.Text + "  " + item.TOURNEE;
                        passage++;

                    }
                    this.Txt_zone.Tag = LestourneeDuLot;
                    this.Txt_zone.IsReadOnly = true;
                    ListeDesSelectTourneeLot = LestourneeDuLot;
                }
            }
        }
        private void btn_batch_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listeBatch != null)
                {
                    List<CsLotri> _lstLotAfficher = ClasseMethodeGenerique.RetourneDistinctLotri(listeBatch);
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("NUMLOTRI", "LOT");
                    _LstColonneAffich.Add("PERIODE", "PERIODE");
                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_lstLotAfficher);
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

        private void Txt_Centre_TextChanged(object sender, TextChangedEventArgs e)
        {


            if (Txt_Centre.Text.Length == SessionObject.Enumere.TailleCentre)
            {
                CsLotri leLotSelect = new CsLotri();
                if (this.Txt_NumBatch.Tag != null)
                    leLotSelect = (CsLotri)this.Txt_NumBatch.Tag;
                List<CsLotri> _lstLotAfficher =ClasseMethodeGenerique.RetourneDistinctCentre(listeBatch.Where(t => t.NUMLOTRI == leLotSelect.NUMLOTRI && t.PERIODE == leLotSelect.PERIODE).ToList());

                if (_lstLotAfficher.FirstOrDefault(t => t.CENTRE == this.Txt_Centre.Text) == null)
                {
                    Message.ShowInformation("Centre inexistant ", "Lot");
                    return;
                }
                else
                    Txt_Centre.Tag = _lstLotAfficher.FirstOrDefault(t => t.CENTRE == this.Txt_Centre.Text);
            }
        }
        //private void Txt_zone_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (Txt_zone.Text.Length == SessionObject.Enumere.TailleCodeTournee)
        //    {
        //        CsLotri leLotSelect = new CsLotri();
        //        if (this.Txt_Centre.Tag != null)
        //            leLotSelect = (CsLotri)this.Txt_Centre.Tag;
        //        List<CsLotri> _lstLotAfficher = ListeDesTourneeLot.Where(p => p.CENTRE == leLotSelect.CENTRE && p.FK_IDCENTRE == leLotSelect.FK_IDCENTRE).OrderBy(t => t.TOURNEE).ToList();

        //        if (_lstLotAfficher.FirstOrDefault(t => t.TOURNEE == this.Txt_zone.Text) == null)
        //        {
        //            Message.ShowInformation("Centre inexistant ", "Lot");
        //            return;
        //        }
        //        else
        //            Txt_zone.Tag = _lstLotAfficher.FirstOrDefault(t => t.TOURNEE == this.Txt_zone.Text);
        //    }
        //}
        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.dataGrid1.SelectedItem != null)
            {
                if (((CsEvenement)this.dataGrid1.SelectedItem).IsSaisi == false)
                    ((CsEvenement)this.dataGrid1.SelectedItem).IsSaisi = true;
                else
                    ((CsEvenement)this.dataGrid1.SelectedItem).IsSaisi = false;
            }
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.dataGrid2.SelectedItem != null)
            {
                if (((CsEvenement)this.dataGrid2.SelectedItem).IsSaisi == false)
                    ((CsEvenement)this.dataGrid2.SelectedItem).IsSaisi = true;
                else
                    ((CsEvenement)this.dataGrid2.SelectedItem).IsSaisi = false;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            List<CsEvenement> ListeSelect = ((List<CsEvenement>)this.dataGrid1.ItemsSource).ToList();
            foreach (CsEvenement item in ListeSelect)
            {
                item.IsSaisi = false;
                dataGrid1.SelectedItems.Add(item);
            }
            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsEvenement>(dataGrid1, dataGrid2);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            List<CsEvenement> ListeSelect = ((List<CsEvenement>)this.dataGrid2.ItemsSource).ToList();
            foreach (CsEvenement item in ListeSelect)
            {
                item.IsSaisi = false;
                dataGrid2.SelectedItems.Add(item);
            }
            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsEvenement>(dataGrid2, dataGrid1);
        }

       
    }
}


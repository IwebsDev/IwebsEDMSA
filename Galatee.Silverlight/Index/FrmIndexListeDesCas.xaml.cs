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
using Galatee.Silverlight.MainView ;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Resources.Index ;


namespace Galatee.Silverlight.Facturation
{
    public partial class FrmIndexListeDesCas : ChildWindow
    {
        List<CsLotri> listeBatch = new List<CsLotri>();
        List<CsLotri> listeBatchInit = new List<CsLotri>();
        CsLotri leBatchSelect = new CsLotri();
        List<CsLotri> ListeDesTourneeLot = new List<CsLotri>();
        List<string> ListeStatus = new List<string>();

        bool isAllClick = false;
        bool isNothingClick = false;

        public FrmIndexListeDesCas()
        {
            InitializeComponent();
            Translate();
            ChargerDonneeDuSite(true , string.Empty );
            this.Txt_zone.IsReadOnly = true ;
            this.btn_Centre.IsEnabled = false;
            this.btn_Tournee.IsEnabled = false;
            this.Txt_Centre.IsReadOnly  = true ;

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            List<int> lstCentre = new List<int>();
            List<int?> lstTourne = new List<int?>();
            if (this.Txt_Centre.Tag != null)
                lstCentre = (List<int>)this.Txt_Centre.Tag;
            else
                lstCentre.AddRange(_lstLotCEntreAfficher.Select(y => y.PK_ID));


            if (this.Txt_zone.Tag != null)
            {
                var lstTrne = (List<int>)this.Txt_zone.Tag;
                lstTrne.ForEach(c => lstTourne.Add(c));
            }
            else
            {
                List<CsLotri> lesTourneeLot = new List<CsLotri>();
                if (this.Txt_Centre.Tag != null)
                {
                    List<int> lstidCentre = (List<int>)this.Txt_Centre.Tag;
                    lesTourneeLot = ListeDesTourneeLot.Where(p => p.NUMLOTRI == this.Txt_NumBatch.Text && lstidCentre.Contains(p.FK_IDCENTRE)).OrderBy(t => t.TOURNEE).ToList();
                }
                else
                    lesTourneeLot = ListeDesTourneeLot.Where(p => p.NUMLOTRI == this.Txt_NumBatch.Text).OrderBy(t => t.TOURNEE).ToList();
                foreach (var item in lesTourneeLot)
                    lstTourne.Add(item.FK_IDTOURNEE);
            }

            List<CsCasind> lesCasSelect = new List<CsCasind>();
            if (dataGrid1.ItemsSource != null)
                lesCasSelect = ((List<CsCasind>)dataGrid1.ItemsSource).Where(t=>t.IsSelect == true).ToList();

            if (this.chk_NonEnquetable.IsChecked == true || this.Chk_Enquete .IsChecked == true )
                EditerListeCasDuLot(this.Txt_NumBatch.Text, lstCentre, lstTourne, lesCasSelect);
            else
                EditerListeCasConfirmerDuLot(this.Txt_NumBatch.Text, lstCentre, lstTourne, lesCasSelect);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Chk_Enquete.IsChecked = true;
                //this.btn_Batch.IsEnabled = false;
                //btn_Tournee.IsEnabled = false;

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
            this.btn_Tournee .Content = Langue.btn_tournee ;
            this.btn_Recherche.Content = Langue.btn_Rechercher;
            this.Chk_Enquete.Content = Langue.chk_Enquetable;
            this.chk_NonEnquetable.Content = Langue.Chk_Nonenqetable;
        
        }

        private void ChargerDonneeDuSite(bool IsFacturationEnCours, string Periode)
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
                    foreach (ServiceAccueil.CsSite item in lstSite)
                        lesDeCentre.Add(item.CODE, LstCentre.Where(i => i.CODESITE == item.CODE).Select(p => p.PK_ID).ToList());
                    ChargerLotriPourEnquete(lesDeCentre, IsFacturationEnCours, Periode);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
                    foreach (ServiceAccueil.CsSite item in lstSite)
                        lesDeCentre.Add(item.CODE, LstCentre.Where(i => i.CODESITE == item.CODE).Select(p => p.PK_ID).ToList());
                    ChargerLotriPourEnquete(lesDeCentre, IsFacturationEnCours, Periode);
                    return;
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Btn_Recherche_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Periode.Text))
            {
                string Periode = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_Periode.Text);
                ChargerDonneeDuSite(false, Periode);
            }
            else
                Message.ShowInformation("Saisir la periode à afficher", "Facturation");
        }
        private void ChargerLotriPourEnquete(Dictionary<string, List<int>> lesDeCentre,bool IsFacturationCourante,string Periode)
        {
            FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
            service.ChargerLotriPourEnqueteCompleted += (_, es) =>
            {
                try
                {
                    if (es != null && es.Cancelled)
                    {
                        Message.ShowError("", "");
                        return;
                    }

                    if (es.Result == null || es.Result.Count == 0)
                    {
                        Message.ShowError("", "");
                        return;
                    }

                    if (es.Result != null && es.Result.Count != 0)
                    {
                        listeBatchInit = es.Result;
                        listeBatch = ClasseMethodeGenerique.RetourneDistinctNumLot(es.Result);
                    }

                    ListeDesTourneeLot = new List<CsLotri>();
                    if (listeBatch.Count != 0)
                        ListeDesTourneeLot = ClasseMethodeGenerique.RetourneDistinctTournee(listeBatch);

                    this.btn_batch.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }
            };
            service.ChargerLotriPourEnqueteAsync(lesDeCentre, IsFacturationCourante, Periode);

        }

        void checkSelectedItem(CheckBox check)
        {
            try
            {
                CheckBox chk = check;
                if (chk.IsChecked.Value || isNothingClick)
                    chk.IsChecked = false;
                else
                {
                    chk.IsChecked = true;
                    if (isAllClick)
                        chk.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        void UncheckAllItem(CheckBox check, bool value)
        {
            try
            {
                CheckBox chk = check;
                chk.IsChecked = value;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                //throw;
            }

        }

        //void traitementListViewItems(DataGrid d,CsCasind LeCasSelectionner)
        //{
        //    try
        //    {
        //        //isUnChecking = false;
        //        CsCasind item = d.SelectedItem as CsCasind;
        //        CheckBox _item = (CheckBox)this.dataGrid1.Columns[0].GetCellContent(d.SelectedItem as CsCasind) as CheckBox;

        //        if (_item.IsChecked.Value)
        //        {
        //            if (ListeCasSelect.FirstOrDefault(p => p.CODE  == LeCasSelectionner.CODE ) == null)
        //                ListeCasSelect.Add(LeCasSelectionner);
        //            _item.IsChecked = true;

        //        }
        //        else
        //        {
        //            if (ListeCasSelect.FirstOrDefault(p => p.CODE  == LeCasSelectionner.CODE ) != null)
        //                ListeCasSelect.Remove (LeCasSelectionner);
        //            _item.IsChecked = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void btn_RechercheCas_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                List<int> lstCentre = new List<int>();
                List<int?> lstTourne = new List<int?>();
                if (this.Txt_Centre.Tag != null)
                    lstCentre = (List<int>)this.Txt_Centre.Tag;
                else
                    lstCentre.AddRange(_lstLotCEntreAfficher.Select(y => y.FK_IDCENTRE ));


                if (this.Txt_zone.Tag != null)
                {
                    var zone = (List<int>)this.Txt_zone.Tag;

                    foreach (var item in zone)
                    {
                        lstTourne.Add(item);
                    }
                }
                else
                {
                    List<CsLotri> lesTourneeLot = new List<CsLotri>();
                    if (this.Txt_Centre.Tag != null)
                    {
                        List<int> lstidCentre = (List<int>)this.Txt_Centre.Tag;
                        lesTourneeLot = ListeDesTourneeLot.Where(p => p.NUMLOTRI == this.Txt_NumBatch.Text && lstidCentre.Contains(p.FK_IDCENTRE)).OrderBy(t => t.TOURNEE).ToList();
                    }
                    else
                        lesTourneeLot = ListeDesTourneeLot.Where(p => p.NUMLOTRI == this.Txt_NumBatch.Text).OrderBy(t => t.TOURNEE).ToList();
                    foreach (var item in lesTourneeLot)
                        lstTourne.Add(item.FK_IDTOURNEE);

                }
                ChargeListeDesCasDuLot(this.Txt_NumBatch.Text, lstCentre, lstTourne);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void ChargeListeDesCasDuLot(string Lotri,List<int>  LesCentre,List<int?> Latournee)
        {
            try
            {
                string TypeCas = string.Empty;
                List<CsCasind> lstCaAfficher = new List<CsCasind>();
                if (Chk_Enquete.IsChecked == true) TypeCas = "1";
                else if (chk_NonEnquetable.IsChecked == true ) TypeCas = "2";
                else if (chk_Confirmer.IsChecked == true) TypeCas = "3";

                List<CsCasind> ListDesCasDuLot = new List<CsCasind>();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ListeDesCasCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Error != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel serveur", "Erreur");
                            return;
                        }

                        if (args.Result != null && args.Result.Count == 0)
                        {
                            Message.ShowInformation ("Aucune données trouvées!", "Info");
                            return;
                        }

                        ListDesCasDuLot=args.Result;
                        this.dataGrid1.ItemsSource = null;
                        this.dataGrid1.ItemsSource = ListDesCasDuLot;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.ListeDesCasAsync(Lotri, LesCentre, Latournee, TypeCas);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void EditerListeCasDuLot(string Lotri, List<int> LesCentre, List<int?> Lestournee, List<CsCasind> ListCas)
        {
            try
            {
                List<CsCasind> ListDesCasDuLot = new List<CsCasind>();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.EditerListeDesCasAsync(Lotri, LesCentre, Lestournee, ListCas);
                service.EditerListeDesCasCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Error != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel serveur", "Erreur");
                            return;
                        }
                        if (produit == SessionObject.Enumere.ElectriciteMT)
                            Utility.ActionDirectOrientation<ServicePrintings.CsEvenement, ServiceFacturation.CsEvenement>(args.Result, null, SessionObject.CheminImpression, "EnqueteMt", "Index", true);
                        else
                            Utility.ActionDirectOrientation<ServicePrintings.CsEvenement, ServiceFacturation.CsEvenement>(args.Result, null, SessionObject.CheminImpression, "Enquete", "Index", true);
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void EditerListeCasConfirmerDuLot(string Lotri, List<int> LesCentre, List<int?> Latournee, List<CsCasind> ListCas)
        {
            try
            {
                List<CsCasind> ListDesCasDuLot = new List<CsCasind>();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.EditerListeDesCasConfirmerAsync(Lotri, LesCentre, Latournee, ListCas);
                service.EditerListeDesCasConfirmerCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Error != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel serveur", "Erreur");
                            return;
                        }
                        if (produit == SessionObject.Enumere.ElectriciteMT)
                        Utility.ActionDirectOrientation<ServicePrintings.CsEvenement , ServiceFacturation.CsEvenement>(args.Result, null, SessionObject.CheminImpression, "EnqueteMt", "Index", true);
                        else
                        Utility.ActionDirectOrientation<ServicePrintings.CsEvenement , ServiceFacturation.CsEvenement>(args.Result, null, SessionObject.CheminImpression, "Enquete", "Index", true);

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dataGrid1.SelectedItem != null)
                {
                    //checkSelectedItem((CheckBox)this.dataGrid1.Columns[0].GetCellContent(dataGrid1.SelectedItem as CsCasind) as CheckBox);
                    //traitementListViewItems(this.dataGrid1, (CsCasind)dataGrid1.SelectedItem);
                    //this.OKButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        string produit = string.Empty;
        List<CsLotri> _lstLotCEntreAfficher = new List<CsLotri>();
        private void ucgReturn(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique uc = sender as MainView.UcListeGenerique;
                if (uc.GetisOkClick)
                {
                    this.Txt_Centre.Text = string.Empty;
                    CsLotri lot = (CsLotri)uc.MyObject;
                    Txt_NumBatch.Text = lot.NUMLOTRI;
                    this.Txt_NumBatch.Tag = lot;
                    produit = listeBatchInit.FirstOrDefault(t=>t.NUMLOTRI ==Txt_NumBatch.Text).PRODUIT  ;
                    _lstLotCEntreAfficher = ClasseMethodeGenerique.RetourneDistinctCentre(listeBatch.Where(t => t.NUMLOTRI == lot.NUMLOTRI).ToList());
                    if (_lstLotCEntreAfficher.Count == 1)
                    {
                        List<int> lstCentre = new List<int>();
                        lstCentre.Add(_lstLotCEntreAfficher.First().FK_IDCENTRE );
                        this.Txt_Centre.Text = _lstLotCEntreAfficher.First().CENTRE;
                        this.Txt_Centre.Tag = lstCentre;
                        //this.Txt_LibelleCentre.Text = _lstLotCEntreAfficher.First().LIBELLECENTRE;
                    }

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void ucgCentre(object sender, EventArgs e)
        {
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
                    this.Txt_Centre.Tag = LesCentreeDuLot.Select(o=>o.FK_IDCENTRE ).ToList();
                    this.btn_Tournee.IsEnabled = true;
                    this.Txt_zone.IsEnabled = true;
                }
            }
        }
 
        private void btn_tournee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CENTRE");
                _LstColonneAffich.Add("TOURNEE");
                if (this.Txt_NumBatch.Tag != null)
                {
                    List<CsLotri> lesTourneeLot  = new List<CsLotri>();
                    if (this.Txt_Centre.Tag != null)
                    {
                        List<int> lstidCentre = (List<int>)this.Txt_Centre.Tag;
                        lesTourneeLot = ListeDesTourneeLot.Where(p => p.NUMLOTRI == this.Txt_NumBatch.Text && lstidCentre.Contains(p.FK_IDCENTRE)).OrderBy(t => t.TOURNEE).ToList();
                    }
                    else
                        lesTourneeLot = ListeDesTourneeLot.Where(p => p.NUMLOTRI == this.Txt_NumBatch.Text).OrderBy(t => t.TOURNEE).ToList();

                    foreach (CsLotri item in lesTourneeLot)
                    {
                        item.CODE = item.CENTRE;
                        item.LIBELLE = item.TOURNEE;
                        if(item.FK_IDTOURNEE!=null)
                            item.PK_ID = item.FK_IDTOURNEE.Value;
                    }

                    UcGenerique ctrl = new UcGenerique(lesTourneeLot, true, "Liste des tournées");
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
                    this.Txt_zone.Tag = LestourneeDuLot.Select(y=>y.PK_ID ).ToList();
                }
            }
        }
        //void galatee_OkClickedBtnZone1(object sender, EventArgs e)
        //{
        //    MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
        //    CsLotri tourneeDuLot = (CsLotri)ctrs.MyObject;
        //    if (tourneeDuLot != null)
        //    {
        //        this.Txt_zone.Text = tourneeDuLot.TOURNEE;
        //        this.Txt_zone.Tag = tourneeDuLot;
        //    }
        //}

        private void btn_batch_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listeBatch != null)
                {
                    List<CsLotri> _lstLotAfficher =ClasseMethodeGenerique.RetourneDistinctLotri(listeBatch);
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
                {
                    produit = listeBatchInit.FirstOrDefault(t => t.NUMLOTRI == Txt_NumBatch.Text).PRODUIT;
                    Txt_NumBatch.Tag = listeBatch.FirstOrDefault(t => t.NUMLOTRI == this.Txt_NumBatch.Text);

                    this.btn_Centre.IsEnabled = true;
                    this.Txt_Centre.IsReadOnly = false;
                    
                }
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
                {
                    List<int> lstidcentre = new List<int>();
                    lstidcentre.Add(_lstLotAfficher.FirstOrDefault(t => t.CENTRE == this.Txt_Centre.Text).FK_IDCENTRE);
                    Txt_Centre.Tag = lstidcentre;
                }
            }
        }
        private void Txt_zone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Txt_zone.Text.Length == SessionObject.Enumere.TailleCodeTournee)
            {
                List<CsLotri> _lstLotAfficher= new List<CsLotri>();
                if (this.Txt_Centre.Tag != null)
               _lstLotAfficher  = ListeDesTourneeLot.Where(p =>((List<int>)this.Txt_Centre.Tag).Contains( p.FK_IDCENTRE)).OrderBy(t => t.TOURNEE).ToList();

                if (_lstLotAfficher.FirstOrDefault(t => t.TOURNEE == this.Txt_zone.Text) == null)
                {
                    Message.ShowInformation("Centre inexistant ", "Lot");
                    return;
                }
                //else
                //    Txt_zone.Tag = _lstLotAfficher.FirstOrDefault(t => t.TOURNEE == this.Txt_zone.Text);
            }
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
                    UcGenerique ctrl = new UcGenerique(_lstLotAfficher, true, "Liste des centres");
                    ctrl.Closed += new EventHandler(ucgCentre);
                    ctrl.Show();



                    //List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_lstLotAfficher);
                    //MainView.UcListeGenerique ucg = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                    //ucg.Closed += new EventHandler(ucgCentre);
                    //ucg.Show();
                }


            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.ItemsSource != null)
            {
              List<CsCasind>  lesCasSelect = ((List<CsCasind>)dataGrid1.ItemsSource);
              lesCasSelect.ForEach(t => t.IsSelect = true);
              this.OKButton.IsEnabled = true;

            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.ItemsSource != null)
            {
                List<CsCasind> lesCasSelect = ((List<CsCasind>)dataGrid1.ItemsSource);
                lesCasSelect.ForEach(t => t.IsSelect = false );

                this.OKButton.IsEnabled = true;
            }
        }
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsCasind >;

            if (dg.SelectedItem != null)
            {
                CsCasind SelectedObject = (CsCasind)dg.SelectedItem;

                if (SelectedObject.IsSelect  == false)
                    SelectedObject.IsSelect = true;

                else
                    SelectedObject.IsSelect = false;
                this.OKButton.IsEnabled = true;

            }
        }

        private void chk_Autre_Checked(object sender, RoutedEventArgs e)
        {
            this.Txt_Periode.IsReadOnly = false;
            this.Btn_Recherche.IsEnabled = true;
        }
        private void chk_Autre_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Txt_Periode.IsReadOnly = true;
            this.Txt_Periode.Text = string.Empty;
        }
    }
}


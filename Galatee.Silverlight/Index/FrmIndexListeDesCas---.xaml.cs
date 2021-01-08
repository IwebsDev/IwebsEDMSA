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
        CsLotri leBatchSelect = new CsLotri();
        List<CsLotri> ListeDesTourneeLot = new List<CsLotri>();
        List<CsCasind> ListeCasSelect = new List<CsCasind>();
        List<string> ListeStatus = new List<string>();


        bool isAllClick = false;
        bool isNothingClick = false;

        public FrmIndexListeDesCas()
        {
            InitializeComponent();
            Translate();
            ChargerDonneeDuSite();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EditerListeCasDuLot(this.Txt_NumBatch.Text, this.Txt_Centre.Text, this.Txt_zone.Text, ListeCasSelect);
            //this.DialogResult = true;
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
            this.Chk_Confirme.Content = Langue.Chk_Confirmer;
            this.Chk_Enquete.Content = Langue.chk_Enquetable;
            this.chk_NonEnquetable.Content = Langue.Chk_Nonenqetable;
        
        }

        List<int> IdDesCentre = new List<int>();
        List<ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_Site.Text = lstSite.First().CODE;
                        txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_Site.Tag = lstSite.First().PK_ID;
                        this.btn_Site.IsEnabled = false;

                    }
                    else
                        this.btn_Site.IsEnabled = true;

                    if (lesCentre.Count == 1)
                    {
                        this.Txt_Centre.Text = lesCentre.First().CODE;
                        txt_libellecentre.Text = lesCentre.First().LIBELLE;
                        this.Txt_Centre.Tag = lesCentre.First().PK_ID;
                        this.btn_Centre.IsEnabled = false;
                    }
                    else
                        this.btn_Centre.IsEnabled = true;

                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);

                    Chargelotri(IdDesCentre);
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Chargelotri(List<int> lstCentreHAbilite)
        {
            FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
            service.RetourneListeLotNonTraiteCompleted += (_, es) =>
            {
                try
                {
                    if (es != null && es.Cancelled)
                    {
                        Message.ShowError("", "");
                        //LoadingManager.EndLoading(result);
                        return;
                    }

                    if (es.Result == null || es.Result.Count == 0)
                    {
                        Message.ShowError("", "");
                        //LoadingManager.EndLoading(result);
                        return;
                    }

                    if (es.Result != null && es.Result.Count != 0)
                        listeBatch = ClasseMethodeGenerique.RetourneDistinctNumLot(es.Result);

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
            service.RetourneListeLotNonTraiteAsync(lstCentreHAbilite);

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

        void traitementListViewItems(DataGrid d,CsCasind LeCasSelectionner)
        {
            try
            {
                //isUnChecking = false;
                CsCasind item = d.SelectedItem as CsCasind;
                CheckBox _item = (CheckBox)this.dataGrid1.Columns[0].GetCellContent(d.SelectedItem as CsCasind) as CheckBox;

                if (_item.IsChecked.Value)
                {
                    if (ListeCasSelect.FirstOrDefault(p => p.CODE  == LeCasSelectionner.CODE ) == null)
                        ListeCasSelect.Add(LeCasSelectionner);
                    _item.IsChecked = true;

                }
                else
                {
                    if (ListeCasSelect.FirstOrDefault(p => p.CODE  == LeCasSelectionner.CODE ) != null)
                        ListeCasSelect.Remove (LeCasSelectionner);
                    _item.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_Recherche_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                ListeStatus.Clear();

                if ((this.chk_NonEnquetable.IsChecked == true && this.Chk_Enquete.IsChecked == true && this.Chk_Confirme.IsChecked == true)
                      || (this.chk_NonEnquetable.IsChecked != true && this.Chk_Enquete.IsChecked != true && this.Chk_Confirme.IsChecked != true))
                    ListeStatus.Add(string.Empty);
                else
                {
                    if (this.Chk_Confirme.IsChecked == true)
                        ListeStatus.Add(SessionObject.Enumere.PagerieConfirme);

                    if (this.Chk_Enquete.IsChecked == true)
                        ListeStatus.Add(SessionObject.Enumere.PagerieEnquetable);

                    if (this.chk_NonEnquetable.IsChecked == true)
                        ListeStatus.Add(SessionObject.Enumere.PagerieNonEnquetable);
                }

                ChargeListeDesCasDuLot(this.Txt_NumBatch.Text, this.Txt_Centre.Text, (string.IsNullOrEmpty(this.Txt_zone.Text) ? null : this.Txt_zone.Text), ListeStatus);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void ChargeListeDesCasDuLot(string Lotri,string  LeCentre,string Latournee,List<string> Status )
        {
            try
            {
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

                        ListDesCasDuLot.AddRange(args.Result);
                        this.dataGrid1.ItemsSource = null;
                        this.dataGrid1.ItemsSource = ListDesCasDuLot;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.ListeDesCasAsync(Lotri, LeCentre, Latournee, Status);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void EditerListeCasDuLot(string Lotri, string LeCentre, string LaTournee, List<CsCasind> ListCas)
        {
            try
            {
                string key = Utility.getKey();
                List<CsCasind> ListDesCasDuLot = new List<CsCasind>();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.EditerListeDesCasCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Error != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel serveur", "Erreur");
                            return;
                        }
                        Utility.ActionImpressionDirect(SessionObject.CheminImpression, key, "EtatListeDesCas", "Index");
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.EditerListeDesCasAsync(Lotri, LeCentre, LaTournee, ListCas, key);
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
                    checkSelectedItem((CheckBox)this.dataGrid1.Columns[0].GetCellContent(dataGrid1.SelectedItem as CsCasind) as CheckBox);
                    traitementListViewItems(this.dataGrid1, (CsCasind)dataGrid1.SelectedItem);
                    this.OKButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
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


        private void btn_tournee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CENTRE");
                _LstColonneAffich.Add("TOURNEE");
                if (this.Txt_NumBatch.Tag != null)
                {
                    int idcentreSelect = (int)this.Txt_Centre.Tag;
                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(ListeDesTourneeLot.Where(p => p.FK_IDCENTRE == idcentreSelect).OrderBy(t => t.TOURNEE).ToList());
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Liste des tournées");
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
            MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
            CsLotri tourneeDuLot = (CsLotri)ctrs.MyObject;
            if (tourneeDuLot != null)
            {
                this.Txt_zone.Text = tourneeDuLot.TOURNEE;
                this.Txt_zone.Tag = tourneeDuLot;
            }
        }

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
                    _LstColonneAffich.Add("CENTRE", "CENTRE");
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
        private void Txt_zone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Txt_zone.Text.Length == SessionObject.Enumere.TailleCodeTournee)
            {
                int idcentreSelect = 0;
                if (this.Txt_Centre.Tag != null)
                    idcentreSelect = (int)this.Txt_Centre.Tag;
                List<CsLotri> _lstLotAfficher = ListeDesTourneeLot.Where(p => p.FK_IDCENTRE == idcentreSelect).OrderBy(t => t.TOURNEE).ToList();

                if (_lstLotAfficher.FirstOrDefault(t => t.TOURNEE == this.Txt_zone.Text) == null)
                {
                    Message.ShowInformation("Centre inexistant ", "Lot");
                    return;
                }
                else
                    Txt_zone.Tag = _lstLotAfficher.FirstOrDefault(t => t.TOURNEE == this.Txt_zone.Text);
            }
        }
        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CODE");
                _LstColonneAffich.Add("LIBELLE");
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Site");
                ctrl.Closed += new EventHandler(galatee_OkClickedSite);
                ctrl.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    this.Txt_Centre.Text = string.Empty;
                    this.Txt_Centre.Tag = null;
                    this.txt_libellecentre.Text = string.Empty;

                    ServiceAccueil.CsSite param = (ServiceAccueil.CsSite)ctrs.MyObject;//.VALEUR;
                    this.Txt_Site.Text = param.CODE;
                    txt_LibelleSite.Text = param.LIBELLE;
                    this.Txt_Site.Tag = param.PK_ID;
                    this.btn_Centre.IsEnabled = true;
                    List<ServiceAccueil.CsCentre> lstCEnt = lesCentre.Where(t => t.FK_IDCODESITE == (int)this.Txt_Site.Tag).ToList();
                    if (lstCEnt != null && lstCEnt.Count != 0)
                    {
                        if (lstCEnt.Count == 1)
                        {
                            this.Txt_Centre.Text = lstCEnt.First().CODE;
                            txt_libellecentre.Text = lstCEnt.First().LIBELLE;
                            this.Txt_Centre.Tag = lstCEnt.First().PK_ID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CODE");
                _LstColonneAffich.Add("LIBELLE");
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lesCentre.Where(t => t.FK_IDCODESITE == (int)this.Txt_Site.Tag).ToList());
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                ctrl.Closed += new EventHandler(galatee_OkClickedCentre);
                ctrl.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    ServiceAccueil.CsCentre param = (ServiceAccueil.CsCentre)ctrs.MyObject;//.VALEUR;
                    this.Txt_Centre.Text = param.CODE;
                    txt_libellecentre.Text = param.LIBELLE;
                    this.Txt_Centre.Tag = param.PK_ID;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }


    }
}


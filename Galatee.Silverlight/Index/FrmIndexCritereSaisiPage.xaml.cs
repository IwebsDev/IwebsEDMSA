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
    public partial class FrmIndexCritereSaisiPage : ChildWindow
    {
        List<CsLotri> listeBatch = new List<CsLotri>();
        List<CsLotri> listeBatchInit = new List<CsLotri>();
        CsLotri leBatchSelect = new CsLotri();
        List<CsLotri> ListeDesTourneeLot = new List<CsLotri>();
        List<CsLotri> _lstCentreDuLot = new List<CsLotri>();

        List<CsLotri> ListeDesSelectTourneeLot = new List<CsLotri>();
        List<CsLotri> ListeDesSelectCentreLot = new List<CsLotri>();

        public FrmIndexCritereSaisiPage()
        {
            InitializeComponent();
            try
            {
                this.Txt_Centre.MaxLength = SessionObject.Enumere.TailleCentre;
                this.Txt_NumBatch.MaxLength = SessionObject.Enumere.TailleNumeroBatch;
                this.Txt_zone.MaxLength = SessionObject.Enumere.TailleCodeTournee;

                this.btn_batch.IsEnabled = false;
                this.Txt_NumBatch.IsReadOnly = true;

                this.btn_Centre.IsEnabled = false;
                this.Txt_Centre.IsReadOnly = true;

                this.btn_tournee.IsEnabled = false;
                this.Txt_zone.IsReadOnly = true;
                translate();
                ChargerDonneeDuSite(true,string.Empty );

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerDonneeDuSite(bool IsFacturationCourant,string Periode)
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    List<int> IdDesCentre = new List<int>();
                    List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    List<ServiceAccueil.CsSite> lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
                    foreach (ServiceAccueil.CsSite item in lstSite)
                        lesDeCentre.Add(item.CODE, lesCentre.Where(i => i.CODESITE == item.CODE).Select(p => p.PK_ID).ToList());
                    Chargelotri(lesDeCentre, IsFacturationCourant, Periode);
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
                    Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
                    foreach (ServiceAccueil.CsSite item in lstSite)
                        lesDeCentre.Add(item.CODE, lesCentre.Where(i => i.CODESITE == item.CODE).Select(p => p.PK_ID).ToList());
                    Chargelotri(lesDeCentre, IsFacturationCourant, Periode);
                    return;

                };
                service.ListeDesDonneesDesSiteAsync(true);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void translate()
        {
            this.Title = Galatee.Silverlight.Resources.Index.Langue.trt_critereSaisiePage;
            this.btn_Centre .Content = Galatee.Silverlight.Resources.Accueil.Langue.lbl_center;
            this.btn_tournee.Content = Galatee.Silverlight.Resources.Accueil.Langue.lbl_Tournee;
            this.lbl_Sequence.Content = Galatee.Silverlight.Resources.Accueil.Langue.lbl_sequence;
            this.btn_batch.Content = Galatee.Silverlight.Resources.Index.Langue.btn_batch;

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.OKButton.IsEnabled = false;
                List<CsLotri> _lstLotSelect = new List<CsLotri>();
                _lstLotSelect = listeBatchInit.Where(p => p.NUMLOTRI == this.Txt_NumBatch.Text ).ToList();

                if (ListeDesSelectCentreLot.Count  != 0)
                {
                    List<int> lstIdCentre = new List<int>();
                    foreach (var item in ListeDesSelectCentreLot)
	                lstIdCentre.Add(item.FK_IDCENTRE);
                    _lstLotSelect = _lstLotSelect.Where(p => lstIdCentre.Contains(p.FK_IDCENTRE)).ToList();
                }
                if (ListeDesSelectTourneeLot != null && ListeDesSelectTourneeLot.Count != 0)
                {
                    List<int?> idTourne = new List<int?>();
                    foreach (CsLotri item in ListeDesSelectTourneeLot)
                        idTourne.Add(item.FK_IDTOURNEE);
                    _lstLotSelect = _lstLotSelect.Where(t => idTourne.Contains(t.FK_IDTOURNEE)).ToList();
                }

                ChargerEvenement(_lstLotSelect, this.Txt_Sequence.Text);
                //this.DialogResult = true;
            }
            catch (Exception ex)
            {
                this.OKButton.IsEnabled = true;

                Message.ShowError(ex, "Erreur");
            }
        }
        private void ChargerEvenement(List<CsLotri> _lstLotri, string ordtour)
        {
            int result = LoadingManager.BeginLoading("Chargement en cours...");

            try
            {
                List<CsEvenement> LstEvenement = new List<CsEvenement>();
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ChargerListeDesEvenementsCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                        {
                            this.OKButton.IsEnabled = true;

                            Message.ShowError(args.Error , Galatee.Silverlight.Resources.Index.Langue.libelleModule );
                            LoadingManager.EndLoading(result);
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            this.OKButton.IsEnabled = true;

                            Message.ShowError(Galatee.Silverlight.Resources.Index.Langue.Msg_IndexNonTrouvé, Galatee.Silverlight.Resources.Index.Langue.libelleModule);
                            LoadingManager.EndLoading(result);
                            return;
                        }

                        LstEvenement.AddRange(args.Result);
                        if (LstEvenement.Count != 0)
                        {
                            foreach (CsEvenement item in LstEvenement)
                            {
                                if (item.CAS =="##")
                                 item.CAS= string.Empty;
                                item.IsSaisi = string.IsNullOrEmpty(item.CAS) ? false : true;
                                item.REFERENCE = item.CENTRE + " " + item.CLIENT + " " + item.ORDRE;
                            }
                            LoadingManager.EndLoading(result);
                            UcSaisieParPage Ctrl = new UcSaisieParPage(_lstLotri,LstEvenement.OrderBy(t => t.CENTRE).ThenBy(u => u.TOURNEE).ThenBy(u => u.ORDTOUR).ThenBy(o => o.CLIENT).ThenBy(p => p.ORDRE).ThenBy(i => i.ORDREAFFICHAGE).ToList());
                            Ctrl.Show();
                            this.OKButton.IsEnabled = true;
                        }
                        else
                        {
                            this.OKButton.IsEnabled = true;
                            LoadingManager.EndLoading(result);
                            Message.ShowError(Galatee.Silverlight.Resources.Index.Langue.Msg_IndexNonTrouvé, "Information");
                        }
                    }
                    catch (Exception ex)
                    {
                        this.OKButton.IsEnabled = true;
                        LoadingManager.EndLoading(result);
                        Message.ShowError(ex, "Erreur");
                    }

                };
                service.ChargerListeDesEvenementsAsync(_lstLotri, ordtour);
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(result);
                throw ex;
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

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listeBatch != null)
                {
                    this.btn_Centre.IsEnabled = false;
                    CsLotri leLotSelect  = new CsLotri();
                    if (this.Txt_NumBatch.Tag != null)
                        leLotSelect = (CsLotri)this.Txt_NumBatch.Tag;
                    List<CsLotri> _lstLotAfficher = ClasseMethodeGenerique.RetourneDistinctCentre(listeBatch.Where(t => t.NUMLOTRI == leLotSelect.NUMLOTRI && t.PERIODE == leLotSelect.PERIODE).ToList());
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CENTRE", "CENTRE");
                    _LstColonneAffich.Add("LIBELLECENTRE", "LIBELLE");


                    foreach (CsLotri item in _lstLotAfficher)
                    {
                        item.CODE = item.CENTRE;
                        item.LIBELLE = item.LIBELLECENTRE;
                    }
                    UcGenerique ctrl = new UcGenerique(_lstLotAfficher.OrderBy(t=>t.CENTRE).ToList(), true, "Liste des centres");
                    ctrl.Closed += new EventHandler(ucgCentre);
                    ctrl.Show();
                }


            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void Chargelotri( Dictionary<string, List<int>> lesDeCentre,bool IsLotCourant,string Periode)
        {
            FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
            service.ChargerLotriPourSaisieCompleted += (_, es) =>
            {
                try
                {
                    if (es != null && es.Cancelled)
                    {
                        Message.ShowError("Aucun lot trouvé", "Facturation");
                        return;
                    }

                    if (es.Result == null || es.Result.Count == 0)
                    {
                        Message.ShowError("Aucun lot trouvé", "Facturation");
                        return;
                    }
                    if (es.Result != null && es.Result.Count != 0)
                    {
                        listeBatchInit = es.Result;
                        listeBatch = ClasseMethodeGenerique.RetourneDistinctNumLot(es.Result);
                        if (listeBatch != null && listeBatch.Count == 1)
                        {
                            Txt_NumBatch.Text = listeBatch.First().NUMLOTRI;
                            this.Txt_NumBatch.Tag = listeBatch.First();

                            _lstCentreDuLot = ClasseMethodeGenerique.RetourneDistinctCentre(listeBatch.Where(t => t.NUMLOTRI == listeBatch.First().NUMLOTRI && t.PERIODE == listeBatch.First().PERIODE).ToList());
                            if (_lstCentreDuLot != null && _lstCentreDuLot.Count == 1)
                            {
                                this.Txt_Centre.Text = _lstCentreDuLot.First().CENTRE;
                                this.Txt_Centre.Tag = _lstCentreDuLot.First();
                                ListeDesSelectTourneeLot = listeBatchInit.Where(t => t.FK_IDCENTRE == _lstCentreDuLot.First().FK_IDCENTRE && t.NUMLOTRI == Txt_NumBatch.Text).ToList();
                                if (ListeDesSelectTourneeLot != null && ListeDesSelectTourneeLot.Count == 1)
                                    this.Txt_zone.Text = ListeDesSelectTourneeLot.First().TOURNEE;
                            }
                        }
                    }
                    ListeDesTourneeLot = new List<CsLotri>();
                    if (listeBatch.Count != 0)
                        ListeDesTourneeLot =ClasseMethodeGenerique.RetourneDistinctTournee(listeBatch);
                    this.btn_batch.IsEnabled = true;
                    this.Txt_NumBatch.IsReadOnly = false;

                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }
            };
            service.ChargerLotriPourSaisieAsync(lesDeCentre,UserConnecte.matricule , IsLotCourant, Periode);

        }
        private void ucgReturn(object sender, EventArgs e)
        {
            try
            {
                this.btn_batch.IsEnabled = true ;
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
                this.btn_batch.IsEnabled = true;
                Message.ShowError(ex, "Erreur");
            }
        }

        private void ucgCentre(object sender, EventArgs e)
        {
            this.btn_Centre.IsEnabled = true ;
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
                    this.Txt_Centre.Tag = LesCentreeDuLot;
                    ListeDesSelectCentreLot = LesCentreeDuLot;
                    this.btn_tournee.IsEnabled = true;
                    this.Txt_zone.IsReadOnly = false;
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
                    List<int> lsidCentre = new List<int>();
                    foreach (CsLotri item in ListeDesSelectCentreLot)
                        lsidCentre.Add(item.FK_IDCENTRE);

                    List<CsLotri> lstLotSelect = ListeDesTourneeLot.Where(p => p.NUMLOTRI == this.Txt_NumBatch.Text && lsidCentre.Contains(p.FK_IDCENTRE)).OrderBy(t => t.CENTRE ).ThenBy(y=>y.TOURNEE).ToList();
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
               List< CsLotri> LestourneeDuLot = (List<CsLotri>)ctrs.MyObjectList ;
               if (LestourneeDuLot != null && LestourneeDuLot.Count > 0)
                {
                    int passage = 1;
                    foreach (CsLotri item in LestourneeDuLot)
                    {
                        if (passage == 1 )
                        this.Txt_zone.Text = item.TOURNEE;
                        else
                            this.Txt_zone.Text = this.Txt_zone.Text + "  " + item.TOURNEE;
                        passage++;

                    }
                    this.Txt_zone.Tag = LestourneeDuLot;
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
                    List<CsLotri> LstLot = new List<CsLotri>();
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    if (chk_LotIsole.IsChecked == true)
                    {
                        _LstColonneAffich.Add("NUMLOTRI", "LOT");
                        _LstColonneAffich.Add("PERIODE", "PERIODE");
                        LstLot = Facturation.ClasseMethodeGenerique.DistinctLotriPeriodeProduit(listeBatch.Where(y => y.ETATFAC10 == "O").ToList());
                    }
                    else
                    {
                          _LstColonneAffich.Add("NUMLOTRI", "LOT");
                    _LstColonneAffich.Add("PERIODE", "PERIODE");
                    LstLot = Facturation.ClasseMethodeGenerique.DistinctLotriPeriodeProduit(listeBatch.Where(y => y.ETATFAC10 != "O").ToList());
                    }
                    this.btn_batch.IsEnabled = false;
                    List<CsLotri> _lstLotAfficher = ClasseMethodeGenerique.RetourneDistinctLotri(LstLot);
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
                List<CsLotri> _lstLotAfficher = ClasseMethodeGenerique.RetourneDistinctCentre(listeBatch.Where(t => t.NUMLOTRI == leLotSelect.NUMLOTRI && t.PERIODE == leLotSelect.PERIODE).ToList());

                if (_lstLotAfficher.FirstOrDefault(t => t.CENTRE == this.Txt_Centre.Text) == null)
                {
                    Message.ShowInformation("Centre inexistant ", "Lot");
                    return;
                }
                else
                    Txt_Centre.Tag = _lstLotAfficher.FirstOrDefault(t => t.CENTRE == this.Txt_Centre.Text);
            }
            if (string.IsNullOrEmpty(this.Txt_Centre.Text)) Txt_Centre.Tag = null;
        }
        private void Txt_zone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Txt_zone.Text.Length == SessionObject.Enumere.TailleCodeTournee )
            {
                //CsLotri leLotSelect = new CsLotri();
                //if (this.Txt_Centre .Tag != null)
                //    leLotSelect = (CsLotri)this.Txt_Centre.Tag;
                //List<CsLotri> _lstLotAfficher = ListeDesTourneeLot.Where(p => p.CENTRE == leLotSelect.CENTRE && p.FK_IDCENTRE == leLotSelect.FK_IDCENTRE).OrderBy(t => t.TOURNEE).ToList();

                //if (_lstLotAfficher.FirstOrDefault(t => t.TOURNEE == this.Txt_zone.Text) == null)
                //{
                //    Message.ShowInformation("Centre inexistant ", "Lot");
                //    return;
                //}
                //else
                //    Txt_zone.Tag = _lstLotAfficher.FirstOrDefault(t => t.TOURNEE == this.Txt_zone.Text);
            }
        }

        private void Txt_NumBatch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_NumBatch.Text))
            { 
            
            }
        }

        private void Btn_Recherche_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Periode.Text))
            {
                if (Shared.ClasseMEthodeGenerique.IsFormatPeriodeValide(this.Txt_Periode.Text))
                {
                    string Periode = Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_Periode.Text);
                    ChargerDonneeDuSite(false, Periode);
                }
                else
                    Message.ShowInformation("Le format de la periode n'est pas valide", "Facturation");
            }
            else
                Message.ShowInformation("Saisir la période", "Facturation");
        }

        private void chk_Autre_Checked(object sender, RoutedEventArgs e)
        {
            this.Txt_Periode.IsReadOnly = false;
            this.Btn_Recherche.IsEnabled = true;
            chk_LotIsole.IsChecked = false;
        }

        private void chk_Autre_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Txt_Periode.IsReadOnly = true;
            this.Txt_Periode.Text = string.Empty;
            this.Btn_Recherche.IsEnabled = false;
        }
    }
}


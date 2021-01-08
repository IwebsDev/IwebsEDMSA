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
    public partial class FrmIndexEditionGeneral : ChildWindow
    {
 
        List<CsLotri> listeBatchInit = new List<CsLotri>();
        List<CsLotri> ListeDesLotriAfficher = new List<CsLotri>();
        List<CsLotri> ListeTourneeSelectDuLot = new List<CsLotri>();

        List<CsLotri> ListeDesTourneeLot = new List<CsLotri>();
        List<CsLotri> ListeDesSelectTourneeLot = new List<CsLotri>();
        List<CsLotri> ListeDesSelectCentreLot = new List<CsLotri>();
        List<CsLotri> _ListeLotri = new List<CsLotri>();
        List<CsLotri> _lstCentreDuLot = new List<CsLotri>(); 
        public FrmIndexEditionGeneral()
        {
            InitializeComponent();
            try
            {
                this.Txt_Centre.MaxLength = SessionObject.Enumere.TailleCentre;
                this.Txt_NumBatch.MaxLength = SessionObject.Enumere.TailleNumeroBatch;
                this.btn_batch.IsEnabled = false;
                this.prgBar.Visibility = System.Windows.Visibility.Collapsed;
                this.btn_Centre.IsEnabled = false;
                this.btn_Tournee1.IsEnabled = false;
                this.btn_Tournee2.IsEnabled = false;
                translate();
                ChargerDonneeDuSite(true,string.Empty );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerDonneeDuSite(bool IsFacturationCourant, string Periode)
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
                    ChargerListeLotri(lesDeCentre, IsFacturationCourant, Periode);
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
                    ChargerListeLotri(lesDeCentre, IsFacturationCourant, Periode);
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
            this.btn_batch.Content = Galatee.Silverlight.Resources.Index.Langue.btn_batch;

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.OKButton.IsEnabled = false;
                ValiderEdition();
            }
            catch (Exception ex)
            {
                this.OKButton.IsEnabled = true;

                Message.ShowError(ex, "Erreur");
            }
        }

        void ValiderEdition()
        {
            try
            {
                int TypeRequete = 0;
                string NomRapport = string.Empty;
                ListeTourneeSelectDuLot = new List<CsLotri>();

                prgBar.Visibility = System.Windows.Visibility.Visible;
                 List<int> lsidCentre = new List<int>();
                foreach (CsLotri item in ListeDesSelectCentreLot)
                    lsidCentre.Add(item.FK_IDCENTRE);
                
                ListeTourneeSelectDuLot = _ListeLotri.Where(t => t.NUMLOTRI == this.Txt_NumBatch.Text).ToList();
                if (!string.IsNullOrEmpty(this.Txt_Centre.Text))
                    ListeTourneeSelectDuLot = ListeTourneeSelectDuLot.Where(t => lsidCentre.Contains(t.FK_IDCENTRE)).ToList();


                if (!string.IsNullOrEmpty(this.Txt_Tournee1.Text) && !string.IsNullOrEmpty(this.Txt_Tournee2.Text))
                    ListeTourneeSelectDuLot = ListeTourneeSelectDuLot.Where(p => int.Parse(p.TOURNEE) >= int.Parse(this.Txt_Tournee1.Text) &&
                                                                                   int.Parse(p.TOURNEE) <= int.Parse(this.Txt_Tournee2.Text)).ToList();

                else if (!string.IsNullOrEmpty(this.Txt_Tournee1.Text) && string.IsNullOrEmpty(this.Txt_Tournee2.Text))
                    ListeTourneeSelectDuLot = ListeTourneeSelectDuLot.Where(p => int.Parse(p.TOURNEE) >= int.Parse(this.Txt_Tournee1.Text)).ToList();

              
                    TypeRequete = 1;
                    if (ListeTourneeSelectDuLot.First().PRODUIT == SessionObject.Enumere.ElectriciteMT)
                        NomRapport = "IndexMt";
                    else
                        NomRapport = "Index";
               
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.EditerLotGeneraleCompleted += (s, args) =>
                {
                    try
                    {
                        this.OKButton.IsEnabled = true;
                        if (args.Error != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel serveur", "Erreur");
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("La récupération des données du lot a retourné des erreurs! Réessayer svp!", "Erreur");
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            return;
                        }
                        if (Chk_ExportExcel.IsChecked != true)
                            Utility.ActionDirectOrientation<ServicePrintings.CsEvenement , ServiceFacturation.CsEvenement>(args.Result, null, SessionObject.CheminImpression, NomRapport, "Index", true);
                        else
                            Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceFacturation.CsEvenement>(args.Result, null, string.Empty, SessionObject.CheminImpression, NomRapport, "Index", true, "xlsx");

                        prgBar.Visibility = System.Windows.Visibility.Collapsed;

                    }
                    catch (Exception ex)
                    {
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.EditerLotGeneraleAsync(ListeTourneeSelectDuLot, TypeRequete);
            }
            catch (Exception ex)
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
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

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_ListeLotri != null)
                {
                    this.btn_Centre.IsEnabled = false;
                    CsLotri leLotSelect  = new CsLotri();
                    if (this.Txt_NumBatch.Tag != null)
                        leLotSelect = (CsLotri)this.Txt_NumBatch.Tag;
                    List<CsLotri> _lstLotAfficher = ClasseMethodeGenerique.RetourneDistinctCentre(_ListeLotri.Where(t => t.NUMLOTRI == leLotSelect.NUMLOTRI && t.PERIODE == leLotSelect.PERIODE).ToList());
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CENTRE", "CENTRE");
                    _LstColonneAffich.Add("LIBELLECENTRE", "LIBELLE");
                    foreach (CsLotri item in _lstLotAfficher)
                    {
                        item.CODE = item.CENTRE ;  
                        item.LIBELLE  = item.LIBELLECENTRE   ;  
                    }
                    UcGenerique ctrl = new UcGenerique(_lstLotAfficher, true, "Liste des centres");
                    ctrl.Closed += new EventHandler(ucgCentre);
                    ctrl.Show();
                    this.btn_Centre.IsEnabled = true ;
                }


            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void ChargerListeLotri(Dictionary<string, List<int>> lesDeCentre, bool IsLotCourant, string Periode)
        {
            _ListeLotri = new List<CsLotri>();
            try
            {

                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ChargerLotriPourEditionIndexCompleted += (s, args) =>
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
                        _ListeLotri = args.Result;
                        ListeDesLotriAfficher = new List<CsLotri>();
                        ListeDesTourneeLot = new List<CsLotri>();
                        this.btn_batch.IsEnabled = true;
                        if (_ListeLotri.Count != 0)
                        {
                            ListeDesLotriAfficher = ClasseMethodeGenerique.RetourneDistinctLotri(_ListeLotri);
                            ListeDesTourneeLot = ClasseMethodeGenerique.RetourneDistinctTournee(_ListeLotri);
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.ChargerLotriPourEditionIndexAsync(lesDeCentre, IsLotCourant, Periode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ucgReturn(object sender, EventArgs e)
        {
            try
            {
                this.btn_batch.IsEnabled = true ;
                MainView.UcListeGenerique uc = sender as MainView.UcListeGenerique;
                if (uc.GetisOkClick)
                {
                    this.btn_Centre.IsEnabled = true;
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
                    if (ListeDesSelectCentreLot != null )
                    {
                        if (ListeDesSelectCentreLot.Count != 1)
                        {
                            this.btn_Tournee1.IsEnabled = false ;
                            this.btn_Tournee2.IsEnabled = false ;
                        }
                        else
                        {
                            this.btn_Tournee1.IsEnabled = true  ;
                            this.btn_Tournee2.IsEnabled = true  ;
                        }
                    this.Txt_Tournee1.Text = string.Empty;
                    this.Txt_Tournee2.Text = string.Empty;
                    }
                }
            }
        }

        private void btn_batch_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_ListeLotri != null)
                {
                    this.btn_batch.IsEnabled = false;
                    List<CsLotri> _lstLotAfficher = ClasseMethodeGenerique.RetourneDistinctLotri(_ListeLotri);
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
                if (_ListeLotri.FirstOrDefault(t => t.NUMLOTRI == this.Txt_NumBatch.Text) == null)
                {
                    Message.ShowInformation("Lot inexistant ", "Lot");
                    return;
                }
                else
                {
                    Txt_NumBatch.Tag = _ListeLotri.FirstOrDefault(t => t.NUMLOTRI == this.Txt_NumBatch.Text);
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
                List<CsLotri> _lstLotAfficher = ClasseMethodeGenerique.RetourneDistinctCentre(_ListeLotri.Where(t => t.NUMLOTRI == leLotSelect.NUMLOTRI && t.PERIODE == leLotSelect.PERIODE).ToList());

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
 

        private void Txt_NumBatch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_NumBatch.Text))
            { 
            
            }
        }
        private void btn_Tournee1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btn_Tournee1.IsEnabled = false;
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CENTRE");
                _LstColonneAffich.Add("TOURNEE");

                List<int> lsidCentre = new List<int>();
                foreach (CsLotri item in ListeDesSelectCentreLot)
                    lsidCentre.Add(item.FK_IDCENTRE);

                List<CsLotri> lstLotSelect = ListeDesTourneeLot.Where(p => p.NUMLOTRI == this.Txt_NumBatch.Text && lsidCentre.Contains(p.FK_IDCENTRE)).OrderBy(t => t.TOURNEE).ToList();
                foreach (CsLotri item in lstLotSelect)
                {
                    item.CODE = item.CENTRE;
                    item.LIBELLE = item.TOURNEE;
                }

                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(ListeDesTourneeLot.Where(p => p.NUMLOTRI == this.Txt_NumBatch.Text && lsidCentre.Contains(p.FK_IDCENTRE)).OrderBy(t => t.TOURNEE).ToList());
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Liste des tournées");
                ctrl.Closed += new EventHandler(galatee_OkClickedBtnZone1);
                ctrl.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }


        void galatee_OkClickedBtnZone1(object sender, EventArgs e)
        {
            this.btn_Tournee1.IsEnabled = true ;
            MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsLotri tourneeDuLot = (CsLotri)ctrs.MyObject;
                if (tourneeDuLot != null)
                    this.Txt_Tournee1.Text = tourneeDuLot.TOURNEE;

                if (string.IsNullOrEmpty(this.Txt_Tournee1.Text) && string.IsNullOrEmpty(this.Txt_Tournee2.Text))
                {
                    if (int.Parse(this.Txt_Tournee1.Text) > int.Parse(this.Txt_Tournee2.Text))
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Index.Langue.msgTourneDebSupTouneeFin, Galatee.Silverlight.Resources.Index.Langue.libelleModule);
                        return;
                    }
                }
            }
        }

        void galatee_OkClickedBtnZone2(object sender, EventArgs e)
        {
            this.btn_Tournee2.IsEnabled = true ;
            MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsLotri tourneeDuLot = (CsLotri)ctrs.MyObject;
                if (tourneeDuLot != null)
                    this.Txt_Tournee2.Text = tourneeDuLot.TOURNEE;

                if (string.IsNullOrEmpty(this.Txt_Tournee1.Text) && string.IsNullOrEmpty(this.Txt_Tournee2.Text))
                {
                    if (int.Parse(this.Txt_Tournee1.Text) > int.Parse(this.Txt_Tournee2.Text))
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Index.Langue.msgTourneDebSupTouneeFin, Galatee.Silverlight.Resources.Index.Langue.libelleModule);
                        return;
                    }
                }
            }
        }

        private void btn_Tournee2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btn_Tournee2.IsEnabled = false;
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CENTRE");
                _LstColonneAffich.Add("TOURNEE");

                List<int> lsidCentre = new List<int>();
                foreach (CsLotri item in ListeDesSelectCentreLot)
                    lsidCentre.Add(item.FK_IDCENTRE);

                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(ListeDesTourneeLot.Where(p => p.NUMLOTRI == this.Txt_NumBatch.Text && lsidCentre.Contains(p.FK_IDCENTRE)).OrderBy(t => t.TOURNEE).ToList());
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Liste des tournées");
                ctrl.Closed += new EventHandler(galatee_OkClickedBtnZone2);
                ctrl.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
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
        }

        private void chk_Autre_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Txt_Periode.IsReadOnly = true;
            this.Txt_Periode.Text = string.Empty;
            this.Btn_Recherche.IsEnabled = false;
        }
    }
}


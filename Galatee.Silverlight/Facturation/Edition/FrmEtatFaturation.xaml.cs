﻿using System;
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
using Galatee.Silverlight.ServiceFacturation;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Resources.Facturation;
//using Galatee.Silverlight.ServiceEservice;

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmEtatFaturation : ChildWindow
    {
        private bool facturesCharges = false;
        private string centre = string.Empty;
        private string client = string.Empty;
        private string ordre = string.Empty;

        private List<CsEnteteFacture> Entetefactures = new List<CsEnteteFacture>();


        public FrmEtatFaturation()
        {
            InitializeComponent();
            this.Txt_CodeSite.MaxLength = SessionObject.Enumere.TailleCentre ;
            this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            ChargerDonneeDuSite();
        }
        string leEtatExecuter = string.Empty;
        public FrmEtatFaturation(string typeEtat)
        {
            InitializeComponent();
            this.Txt_CodeSite.MaxLength = SessionObject.Enumere.TailleCentre;
            this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
            leEtatExecuter = typeEtat;
            if (leEtatExecuter == SessionObject.AbonneNonConstituer)
            {
                Txt_lotri.Visibility = System.Windows.Visibility.Collapsed;
                lbl_Lotri.Visibility = System.Windows.Visibility.Collapsed;
            }
            ChargerDonneeDuSite();
        }
        List<int> lesCentreCaisse = new List<int>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                List<int> lstCentreSelect = new List<int>();

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID ;
                        this.btn_Site.IsEnabled = false;
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        lstCentreSelect.Add(LstCentrePerimetre.First().PK_ID);
                        this.Txt_CodeCentre.Text = LstCentrePerimetre.First().CODE;
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.Txt_CodeCentre.Tag = lstCentreSelect;
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_CodeSite.Text = lstSite.First().CODE;
                        this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                        this.btn_Site.IsEnabled = false;
                    }
                    if (LstCentrePerimetre.Count == 1)
                    {
                        lstCentreSelect.Add(LstCentrePerimetre.First().PK_ID);
                        this.Txt_CodeCentre.Text = LstCentrePerimetre.First().CODE;
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.Txt_CodeCentre.Tag = lstCentreSelect;
                    }
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
        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite.Count > 0)
                {
                    this.btn_Site.IsEnabled = false;
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "SITE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                    ctrl.Closed += new EventHandler(galatee_OkClickedSite);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.LibelleModule);
            }
        }

        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODE;
                this.Txt_CodeSite.Tag = leSite.PK_ID ;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
                List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE  ==(int) this.Txt_CodeSite.Tag).ToList();
                if (lsiteCentre.Count == 1)
                {
                    List<int> lstCentreSelect = new List<int>();
                    lstCentreSelect.Add(lsiteCentre.First().PK_ID);
                    this.Txt_CodeCentre.Text = lsiteCentre.First().CODE;
                    this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                    this.Txt_CodeCentre.Tag = lstCentreSelect;
                }
            }
            this.btn_Site.IsEnabled = true;
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentrePerimetre.Count  != 0)
                {
                    List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsCentre>(LstCentrePerimetre);
                    Shared.UcListeParametre ctr = new UcListeParametre(lstParametre, true , "Centre");
                    ctr.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            try
            {

                Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
                if (ctrs.isOkClick)
                {
                    List<ServiceAccueil.CParametre> _LesCentreSelect = ctrs.MyObjectList as List<ServiceAccueil.CParametre>;
                    List<string> lstCentre =  _LesCentreSelect.Select(t=>t.CODE ).ToList();
                    foreach (string item in lstCentre)
                        this.Txt_CodeCentre.Text = item + " ";
                    this.Txt_CodeCentre.Tag = _LesCentreSelect.Select(t => t.PK_ID).ToList(); 
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        Dictionary<string, string> param = null;
        List<CsEvenement> lstDonnee = new List<CsEvenement>();
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (dataGrid1.ItemsSource == null)
                    throw new Exception(Galatee.Silverlight.Resources.Parametrage.Languages.AucuneDonneeAImprimer);
                lstDonnee = ((List<CsEvenement>)dataGrid1.ItemsSource).ToList();
                Galatee.Silverlight.Shared.FrmOptionEditon ctrl = new Shared.FrmOptionEditon();
                ctrl.Closed += ctrl_Closed;
                this.IsEnabled = false;
                ctrl.Show(); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void ctrl_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            string EtatRdrl = "" ;
            Galatee.Silverlight.Shared.FrmOptionEditon ctrs = sender as Galatee.Silverlight.Shared.FrmOptionEditon;
            if (ctrs.IsOptionChoisi)
            {
                if (leEtatExecuter == SessionObject.AbonneNonConstituer) EtatRdrl = "AbonneNonConstituer";
                if (leEtatExecuter == SessionObject.AbonneNonSaisie) EtatRdrl = "AbonneNonSaisi";
                if (leEtatExecuter == SessionObject.AbonneSaisieNonFact) EtatRdrl = "AbonneNonCalcules";
                if (leEtatExecuter == SessionObject.AbonneFactureNonMaj) EtatRdrl = "AbonneNonMaj";

                if (ctrs.OptionSelect == SessionObject.EnvoiPrinter)
                    Utility.ActionDirectOrientation<ServicePrintings.CsEvenement, ServiceFacturation.CsEvenement>(lstDonnee, param, SessionObject.CheminImpression, EtatRdrl, "Report", true);
                else if (ctrs.OptionSelect == SessionObject.EnvoiExecl)
                    Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceFacturation.CsEvenement>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, EtatRdrl, "Report", true, "xlsx");

                else if (ctrs.OptionSelect == SessionObject.EnvoiWord)
                    Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceFacturation.CsEvenement>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, EtatRdrl, "Report", true, "doc");

                else if (ctrs.OptionSelect == SessionObject.EnvoiPdf)
                    Utility.ActionExportation<ServicePrintings.CsEvenement, ServiceFacturation.CsEvenement>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, EtatRdrl, "Report", true, "pdf");

            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void initCtrl()
        {
        }
        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeSite.Text) && Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    Galatee.Silverlight.ServiceAccueil.CsSite _LeSiteClient = ClasseMEthodeGenerique.RetourneObjectFromList(lstSite, this.Txt_CodeSite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeSiteClient.LIBELLE))
                    {
                        this.Txt_LibelleSite.Text = _LeSiteClient.LIBELLE;
                        this.Txt_CodeSite.Text = _LeSiteClient.CODE;
                        this.Txt_CodeSite.Tag = _LeSiteClient.PK_ID;
                        List<int> lstCentreSelect = new List<int>();

                        List<ServiceAccueil.CsCentre> lsiteCentre = LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList();
                        if (lsiteCentre.Count == 1)
                        {
                            lstCentreSelect.Add(lsiteCentre.First().PK_ID);
                            this.Txt_CodeCentre.Text = lsiteCentre.First().CODE;
                            this.Txt_LibelleCentre.Text = lsiteCentre.First().LIBELLE;
                            this.Txt_CodeCentre.Tag = lstCentreSelect;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

            }
        }

        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeCentre.Text) && Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    ServiceAccueil.CsCentre _LeCentreClient = ClasseMEthodeGenerique.RetourneObjectFromList(LstCentrePerimetre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList(), this.Txt_CodeCentre.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
                    {
                        List<int> lstCentreSelect = new List<int>();
                        lstCentreSelect.Add(_LeCentreClient.PK_ID);
                        this.Txt_CodeCentre.Text = _LeCentreClient.CODE;
                        this.Txt_LibelleCentre.Text = _LeCentreClient.LIBELLE;
                        this.Txt_CodeCentre.Tag = lstCentreSelect;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

            }
        }

    
        private void RechercheClientNonConstituer(Dictionary<string, List<int>> lesDeCentre,string periode)
        {
            try
            {
                    FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                    service.ChargerClientNonConstituerAsync(lesDeCentre,periode);
                    service.ChargerClientNonConstituerCompleted += (er, res) =>
                    {
                        try
                        {
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des factures : " + res.Error.InnerException.ToString(), "Erreur");
                            if (res.Result.Count != 0)
                            {
                                dataGrid1.ItemsSource = null;
                                dataGrid1.ItemsSource = res.Result;
                            }
                            else
                            {
                                Message.ShowInformation("Aucune données trouvée", "Etat");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.Show("Erreur inconnue :" + ex.Message, "Erreur inconnue");
                        }
                        finally
                        {
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    };
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void EtatStatistique(Dictionary<string, List<int>> lesDeCentre, string periode,string Lotri,string TypeEtat)
        {
            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.EtatStatistiqueAsync(lesDeCentre, periode, Lotri,TypeEtat );
                service.EtatStatistiqueCompleted += (er, res) =>
                {
                    try
                    {
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement des factures : " + res.Error.InnerException.ToString(), "Erreur");
                        else if (res.Result != null )
                        {
                            if (res.Result.Count != 0)
                            {
                                dataGrid1.ItemsSource = null;
                                dataGrid1.ItemsSource = res.Result;
                            }
                            else
                            {
                             Message.ShowInformation("Aucune données trouvée","Etat");
                                return ;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                        Message.Show("Erreur inconnue :" + ex.Message, "Erreur inconnue");
                    }
                    finally
                    {
                        prgBar.Visibility = System.Windows.Visibility.Collapsed;
                    }
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        Dictionary<string, List<int>> lesDeCentre = new Dictionary<string, List<int>>();
        private void btn_Recherche_Click_1(object sender, RoutedEventArgs e)
        {
            string TypeEtat = string.Empty;
            if (lesDeCentre.Count != 0) lesDeCentre.Clear();
            if (this.Txt_CodeSite.Tag == null)
            {
                Message.ShowInformation("Selectionner le site ", "Message");
                return;
            }
            prgBar.Visibility = System.Windows.Visibility.Visible ;
            if (this.Txt_CodeCentre .Tag == null)
                lesDeCentre.Add(this.Txt_CodeSite.Text, LstCentrePerimetre.Where(i => i.CODESITE == this.Txt_CodeSite.Text).Select(p => p.PK_ID).ToList());

            else
                lesDeCentre.Add(this.Txt_CodeSite.Text,(List<int>)this.Txt_CodeCentre.Tag);
            if (leEtatExecuter == SessionObject.AbonneNonConstituer)
            {
                RechercheClientNonConstituer(lesDeCentre, string.IsNullOrEmpty(this.Txt_Periode.Text) ? string.Empty : Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_Periode.Text));
                return;
            }
            else if (leEtatExecuter == SessionObject.AbonneNonSaisie) TypeEtat = "1";
            else if (leEtatExecuter == SessionObject.AbonneSaisieNonFact) TypeEtat = "2";
            else if (leEtatExecuter == SessionObject.AbonneFactureNonMaj) TypeEtat = "3";
            EtatStatistique(lesDeCentre, string.IsNullOrEmpty(this.Txt_Periode.Text) ? string.Empty : Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_Periode.Text), this.Txt_lotri.Text, TypeEtat);
        }
   

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.ItemsSource != null)
            {
                List<CsEvenement> lesCasSelect = ((List<CsEvenement>)dataGrid1.ItemsSource);
                lesCasSelect.ForEach(t => t.ISAJOUTLOT = true);
                this.OKButton.IsEnabled = true;

            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.ItemsSource != null)
            {
                List<CsEvenement> lesCasSelect = ((List<CsEvenement>)dataGrid1.ItemsSource);
                lesCasSelect.ForEach(t => t.ISAJOUTLOT = false);

                this.OKButton.IsEnabled = true;
            }
        }

 
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsEvenement>;

            if (dg.SelectedItem != null)
            {
                CsEvenement SelectedObject = (CsEvenement)dg.SelectedItem;

                if (SelectedObject.ISAJOUTLOT  == false)
                    SelectedObject.ISAJOUTLOT = true;

                else
                    SelectedObject.ISAJOUTLOT = false;
                this.OKButton.IsEnabled = true;

            }
        }

      
    }
}


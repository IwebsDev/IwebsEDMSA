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

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmEditionFacturesRegrouper : ChildWindow
    {
        public FrmEditionFacturesRegrouper()
        {
            InitializeComponent();
            ChargerListeDeProduit();
            ChargerDonneeDuSite();
            this.Rdb_facture.IsChecked = true;
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Txt_RegDebut.Tag != null && this.cbo_Periode.Tag != null && this.Cbo_Produit.Tag != null )
                EditerRegroupement();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }
  

        private void EditerRegroupement()
        {
            try
            {
                string rdlcName = string.Empty;
                // Fichier rdlc
                int regroupDebut = 0;
                if (this.Txt_RegDebut.Tag != null)
                    regroupDebut = ((ServiceAccueil.CsRegCli)this.Txt_RegDebut.Tag).PK_ID;

                int regroupFin = 0;
                if (this.Txt_RegFin.Tag != null)
                    regroupFin = ((ServiceAccueil.CsRegCli)this.Txt_RegFin.Tag).PK_ID;

                if (int.Parse(((ServiceAccueil.CsRegCli)this.Txt_RegFin.Tag).CODE) < int.Parse(((ServiceAccueil.CsRegCli)this.Txt_RegDebut.Tag).CODE))
                {
                    Message.ShowInformation("Regroupement debut inferieur au regroupement fin", "Edition");
                    return;
                }

                if (regroupDebut != 0 && regroupFin != 0)
                {
                    List<string> lesPeriode = new List<string>();
                    foreach (var per in LstPeriode)
                        lesPeriode.Add(Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(per));


                    Dictionary<string, string> param = new Dictionary<string, string>();
                    string key = Utility.getKey();

                    string print = "Imprimé le " + DateTime.Now + " sur le poste " + SessionObject.LePosteCourant.NOMPOSTE + " par " + UserConnecte.nomUtilisateur + "(" + UserConnecte.matricule + ") du centre " + UserConnecte.LibelleCentre;
                    param.Add("Print", print);


                    List<string> Produit = ((List<ServiceAccueil.CsProduit>)Cbo_Produit.ItemsSource).Select(t => t.CODE).ToList();
                    if (Rdb_facture.IsChecked == true)
                    {
                        param.Add("TypeEdition", "Originale");
                        rdlcName = "FactureSimple";
                    }
                    if (Rdb_bordereau_detail.IsChecked == true)
                        rdlcName = "FactureDetail";
                    if (Rdb_bordereau_simple.IsChecked == true)
                        rdlcName = "BordereauSimple";
                    if (Rdb_edtion_anomalie.IsChecked == true)
                        rdlcName = "FactureAnomalie";
                    if (Rdb_FactureRegroupe.IsChecked == true)
                        rdlcName = "FactureRegroupe";
                    int? leCentre = null;
                    if (this.Txt_CodeCentre.Tag != null) leCentre = (int)this.Txt_CodeCentre.Tag;
                    prgBar.Visibility = System.Windows.Visibility.Visible;
                    this.OKButton.IsEnabled = false;



                    Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Facturation"));
                    Galatee.Silverlight.ServiceFacturation.FacturationServiceClient services = new Galatee.Silverlight.ServiceFacturation.FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                    services.RetourneFacturesRegroupementAsync(this.Txt_RegDebut.Text, this.Txt_RegFin.Text, lesPeriode, Produit, leCentre, rdlcName);
                    services.RetourneFacturesRegroupementCompleted += (er, res) =>
                    {
                        try
                        {
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            this.OKButton.IsEnabled = true;
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des factures : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                                if (res.Result.Count != 0)
                                {

                                    //foreach (string leRegroupement in lesregroupement)
                                    //{
                                    if (rdlcName == "FactureRegroupe")
                                    {
                                        //foreach (string leRegroupement in lesregroupement)
                                        //{
                                        //    List<ServiceFacturation.CsFactureClient> lstRegroupement = res.Result.Where(i => i.Regcli == leRegroupement).ToList();
                                        //    if (lstRegroupement.Count == 0) continue;
                                        //Utility.ActionDirectOrientation<ServicePrintings.CsFactureClient, ServiceFacturation.CsFactureClient>(res.Result, param, SessionObject.CheminImpression, rdlcName, "Facturation", true);
                                        Utility.ActionExportation<ServicePrintings.CsFactureClient, ServiceFacturation.CsFactureClient>(res.Result, param,string.Empty , SessionObject.CheminImpression, rdlcName, "Facturation", true, "pdf");

                                        //}
                                    }
                                    else
                                    {
                                        //foreach (string leProduit in Produit)
                                        //{
                                        List<ServiceFacturation.CsFactureClient> lesFactureProduitRegpement = res.Result;
                                        if (Produit.First() == SessionObject.Enumere.ElectriciteMT)
                                        {
                                            if (rdlcName == "FactureSimple") rdlcName = "FactureSimpleMT";
                                            if (rdlcName == "FactureDetail") rdlcName = "FactureDetailMTSGC";
                                            if (rdlcName == "BordereauSimple") rdlcName = "BordereauSimpleSGC";
                                           

                                        }
                                        else
                                        {
                                            if (rdlcName == "FactureSimple")
                                            {
                                                if (Produit.First() == SessionObject.Enumere.Eau)
                                                    rdlcName = "FactureSimpleO";
                                            }
                                            if (rdlcName == "FactureDetail") rdlcName = "FactureDetailSCG";
                                            if (rdlcName == "BordereauSimple") rdlcName = "BordereauSimpleSGC";
                                        }
                                        if (Chk_ExportExcel.IsChecked != true)
                                        {
                                            if (rdlcName == "FactureSimple" || rdlcName == "FactureSimpleMT" || rdlcName == "FactureSimpleO")
                                            {
                                                List<ServiceFacturation.CsFactureClient> lstClient = new List<ServiceFacturation.CsFactureClient>();
                                                var lesClient = res.Result.Select(y => new { y.Centre, y.Client, y.Ordre }).Distinct();
                                                foreach (var item in lesClient)
                                                    lstClient.Add(new ServiceFacturation.CsFactureClient() { Centre = item.Centre, Client = item.Client, Ordre = item.Ordre });
                                                int Passage = 0;
                                                string[] tableau = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V" };
                                                while (lstClient.Where(o => o.ISFACTURE != true).Count() != 0)
                                                {
                                                    string NomFichier = rdlcName + tableau[Passage];
                                                    List<string> clientSelectionne = lstClient.Where(m => m.ISFACTURE != true).Take(100).Select(o => o.Client).ToList();
                                                    List<ServiceFacturation.CsFactureClient> factureAEditer = res.Result.Where(p => clientSelectionne.Contains(p.Client)).ToList();
                                                    Utility.ActionExportation<ServicePrintings.CsFactureClient, ServiceFacturation.CsFactureClient>(factureAEditer, param, NomFichier, SessionObject.CheminImpression, rdlcName, "Facturation", true, "pdf");
                                                    lstClient.Where(p => clientSelectionne.Contains(p.Client)).ToList().ForEach(p => p.ISFACTURE = true);
                                                    Passage++;
                                                }
                                            }
                                            else
                                            {
                                                if (rdlcName == "BordereauSimple" || rdlcName == "BordereauSimpleSGC" ||
                                                    rdlcName == "BordereauSimple" || rdlcName == "BordereauSimpleSGC")
                                                {
                                                    List<ServiceFacturation.CsFactureClient> lstClient = new List<ServiceFacturation.CsFactureClient>();
                                                    var lesClient = res.Result.Select(y => new { y.Centre, y.Client, y.Ordre }).Distinct();
                                                    foreach (var item in lesClient)
                                                        lstClient.Add(new ServiceFacturation.CsFactureClient() { Centre = item.Centre, Client = item.Client, Ordre = item.Ordre });
                                                    int Passage = 0;
                                                    string[] tableau = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V" };
                                                    while (lstClient.Where(o => o.ISFACTURE != true).Count() != 0)
                                                    {
                                                        string NomFichier = rdlcName + tableau[Passage];
                                                        List<string> clientSelectionne = lstClient.Where(m => m.ISFACTURE != true).Take(100).Select(o => o.Client).ToList();
                                                        List<ServiceFacturation.CsFactureClient> factureAEditer = res.Result.Where(p => clientSelectionne.Contains(p.Client)).ToList();
                                                        Utility.ActionExportation<ServicePrintings.CsFactureClient, ServiceFacturation.CsFactureClient>(factureAEditer, param, NomFichier, SessionObject.CheminImpression, rdlcName, "Facturation", true, "pdf");
                                                        lstClient.Where(p => clientSelectionne.Contains(p.Client)).ToList().ForEach(p => p.ISFACTURE = true);
                                                        Passage++;
                                                    }
                                                }
                                                else
                                                   Utility.ActionDirectOrientation<ServicePrintings.CsFactureClient, ServiceFacturation.CsFactureClient>(lesFactureProduitRegpement, param, SessionObject.CheminImpression, rdlcName, "Facturation", true);
                                            }
                                        }
                                        else
                                            if (rdlcName == "FactureSimple" || rdlcName == "FactureSimpleMT" || rdlcName == "FactureSimpleO")
                                            {
                                                List<ServiceFacturation.CsFactureClient> lstClient = new List<ServiceFacturation.CsFactureClient>();
                                                var lesClient = res.Result.Select(y => new { y.Centre, y.Client, y.Ordre }).Distinct();
                                                foreach (var item in lesClient)
                                                    lstClient.Add(new ServiceFacturation.CsFactureClient() { Centre = item.Centre, Client = item.Client, Ordre = item.Ordre });

                                                int Passage = 1;
                                                while (lstClient.Where(o => o.ISFACTURE != true).Count() != 0)
                                                {
                                                    string NomFichier = rdlcName + Passage.ToString();
                                                    List<string> clientSelectionne = lstClient.Where(m => m.ISFACTURE != true).Take(100).Select(o => o.Client).ToList();
                                                    List<ServiceFacturation.CsFactureClient> factureAEditer = res.Result.Where(p => clientSelectionne.Contains(p.Client)).ToList();
                                                    Utility.ActionExportation<ServicePrintings.CsFactureClient, ServiceFacturation.CsFactureClient>(factureAEditer,param,NomFichier,SessionObject.CheminImpression, rdlcName, "Facturation", true, "xlsx");
                                                    lstClient.Where(p => clientSelectionne.Contains(p.Client)).ToList().ForEach(p => p.ISFACTURE = true);
                                                    Passage++;
                                                }
                                            }
                                            else 
                                               Utility.ActionExportation<ServicePrintings.CsFactureClient, ServiceFacturation.CsFactureClient>(lesFactureProduitRegpement,param,string.Empty,SessionObject.CheminImpression, rdlcName, "Facturation", true, "xlsx");
                                    }
                                }
                                else
                                    Message.Show("Aucune factures trouvées, veuillez consultez le journal des erreurs",  "Erreur");
                                    //Message.Show("Une erreur s'est produite lors de la génération des factures, veuillez consultez le journal des erreurs",
                                    //    "Erreur");

                        }
                        catch (Exception ex)
                        {
                            Message.Show("Erreur inconnue :" + ex.Message, "Erreur inconnue");
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void EditerRegroupement()
        //{
        //    try
        //    {
        //        string rdlcName = string.Empty;
        //        // Fichier rdlc
        //        int regroupDebut = 0;
        //        if (this.Txt_RegDebut.Tag != null)
        //            regroupDebut = ((ServiceAccueil.CsRegCli)this.Txt_RegDebut.Tag).PK_ID;

        //        int regroupFin = 0;
        //        if (this.Txt_RegFin.Tag != null)
        //            regroupFin = ((ServiceAccueil.CsRegCli)this.Txt_RegFin.Tag).PK_ID;

        //        if (regroupFin < regroupDebut)
        //        {
        //            Message.ShowInformation("Regroupement debut inferieur au regroupement fin", "Edition");
        //            return;
        //        }

        //        if (regroupDebut != 0 && regroupFin != 0)
        //        {
        //            List<string> lesPeriode = new List<string>();
        //            foreach (var per in LstPeriode)
        //                lesPeriode.Add(Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(per));


        //            Dictionary<string, string> param = new Dictionary<string, string>();
        //            string key = Utility.getKey();


        //            List<ServiceAccueil.CsRegCli> lesRegroupement = SessionObject.LstCodeRegroupement.Where(t => t.PK_ID >= regroupDebut && t.PK_ID <= regroupFin).ToList();
        //            List<string> Produit = ((List<ServiceAccueil.CsProduit>)Cbo_Produit.ItemsSource).Select(t => t.CODE).ToList();
        //            List<string> lesregroupement = lesRegroupement.Select(t => t.CODE).ToList();
        //            if (Rdb_facture.IsChecked == true)
        //            {
        //                param.Add("TypeEdition", "Originale");
        //                rdlcName = "FactureSimple";
        //            }
        //            if (Rdb_bordereau_detail.IsChecked == true)
        //                rdlcName = "FactureDetail";
        //            if (Rdb_bordereau_simple.IsChecked == true)
        //                rdlcName = "BordereauSimple";
        //            if (Rdb_edtion_anomalie.IsChecked == true)
        //                rdlcName = "FactureAnomalie";
        //            if (Rdb_FactureRegroupe.IsChecked == true)
        //                rdlcName = "FactureRegroupe";
        //            int? leCentre = null;
        //            if (this.Txt_CodeCentre.Tag != null) leCentre = (int)this.Txt_CodeCentre.Tag;
        //            prgBar.Visibility = System.Windows.Visibility.Visible;
        //            this.OKButton.IsEnabled = false;



        //            Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Impression"));
        //            Galatee.Silverlight.ServiceImpression.ImpressionServiceClient services = new Galatee.Silverlight.ServiceImpression.ImpressionServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Impression"));
        //            services.RetourneFacturesRegroupementAsync(this.Txt_RegDebut.Text, this.Txt_RegFin.Text, lesPeriode, Produit, leCentre, rdlcName, param, key);
        //            services.RetourneFacturesRegroupementCompleted += (er, res) =>
        //            {
        //                try
        //                {
        //                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
        //                    this.OKButton.IsEnabled = true;
        //                    if (res.Error != null || res.Cancelled)
        //                        Message.Show("Erreur dans le traitement des factures : " + res.Error.InnerException.ToString(), "Erreur");
        //                    else
        //                        if (res.Result==true )
        //                        {

                                   
        //                            if (rdlcName == "FactureRegroupe")
        //                            {
        //                                Utility.ActionImpressionDirectOrientation(SessionObject.CheminImpression, key, rdlcName, "Facturation", true);
        //                                this.prgBar.Visibility = System.Windows.Visibility.Collapsed;
        //                            }
        //                            else
        //                            {
        //                                //foreach (string leProduit in Produit)
        //                                //{
                                       
        //                                if (Produit.First() == SessionObject.Enumere.ElectriciteMT)
        //                                {
        //                                    if (rdlcName == "FactureSimple") rdlcName = "FactureSimpleMT";
        //                                    if (rdlcName == "FactureDetail") rdlcName = "FactureDetailMTSCG";
        //                                    if (rdlcName == "BordereauSimple") rdlcName = "BordereauSimpleSGC";
        //                                }
        //                                else
        //                                {
        //                                    if (rdlcName == "FactureSimple") rdlcName = "FactureSimple";
        //                                    if (rdlcName == "FactureDetail") rdlcName = "FactureDetailSCG";
        //                                    if (rdlcName == "BordereauSimple") rdlcName = "BordereauSimpleSGC";
        //                                }
        //                                if (Chk_ExportExcel.IsChecked != true)
        //                                    Utility.ActionImpressionDirectOrientation(SessionObject.CheminImpression, key, rdlcName, "Facturation", true);
        //                                //else
        //                                //    Utility.ActionImpressionDirectOrientation(SessionObject.CheminImpression, key, rdlcName, "Facturation", true);

        //                                //    Utility.ActionExportation<ServicePrintings.CsFactureClient, ServiceFacturation.CsFactureClient>(lesFactureProduitRegpement, param, SessionObject.CheminImpression, rdlcName, "Facturation", true, "xlsx");
        //                            }
        //                        }
        //                        else
        //                            Message.Show("Une erreur s'est produite lors de la génération des factures, veuillez consultez le journal des erreurs",
        //                                "Erreur");

        //                }
        //                catch (Exception ex)
        //                {
        //                    Message.Show("Erreur inconnue :" + ex.Message, "Erreur inconnue");
        //                }
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        void ChargerListDesSite()
        {

            List<ServiceAccueil.CsCentre> LstCentre = new List<ServiceAccueil.CsCentre>();
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstCentre = args.Result;
                        SessionObject.LstCentre = LstCentre;
                        if (LstCentre.Count != 0)
                        {
                           
                        }
                        else
                        {
                            Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }

                };
                service.ListeDesDonneesDesSiteAsync(false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        List<ServiceAccueil.CsProduit> LstDeProduit = new List<ServiceAccueil.CsProduit>();

        private void ChargerListeDeProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit.Count != 0)
                {
                    LstDeProduit = SessionObject.ListeDesProduit;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = res.Result;
                    LstDeProduit = SessionObject.ListeDesProduit;

                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerListeDeProduit");
            }
        }

        private void btn_Produit_Click_1(object sender, RoutedEventArgs e)
        {
           try
            {
                if (LstDeProduit != null && LstDeProduit.Count != 0)
                {

                    List<string> _LstColonneAffich = new List<string>();
                    _LstColonneAffich.Add("CODE");
                    _LstColonneAffich.Add("LIBELLE");
                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstDeProduit.Where(t => t.CODE != "000" && (t.CODE == SessionObject.Enumere.Electricite || t.CODE == SessionObject.Enumere.ElectriciteMT) ).ToList());
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, true, "Produit");
                    ctrl.Closed += new EventHandler(galatee_OkClickedProduit);
                    ctrl.Show();
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        void galatee_OkClickedProduit(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.GetisOkClick)
            {
                if (ctrs.MyObjectList != null && ctrs.MyObjectList.Count != 0)
                {
                    List<ServiceAccueil.CsProduit> lstProduit = new List<ServiceAccueil.CsProduit>();
                    foreach (var p in ctrs.MyObjectList)
                    {
                        ServiceAccueil.CsProduit leProduit = (ServiceAccueil.CsProduit)p;
                        lstProduit.Add(leProduit);
                    }
                    this.Cbo_Produit.ItemsSource = null;
                    this.Cbo_Produit.ItemsSource = lstProduit;
                    this.Cbo_Produit.DisplayMemberPath  = "LIBELLE";
                    cbo_Periode.Tag = lstProduit;
                    Cbo_Produit.SelectedIndex = 0;
                }
            }
        }
        List<string> LstPeriode = new List<string>();
        private void btn_Periode_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Periode.Text ))
            {
                if (LstPeriode.FirstOrDefault(t => t == this.Txt_Periode.Text) == null)
                {
                    LstPeriode.Add(this.Txt_Periode.Text);
                    this.cbo_Periode.ItemsSource = null ;
                    this.cbo_Periode.ItemsSource = LstPeriode;
                    this.cbo_Periode.SelectedIndex = 0;
                }
                else
                    Message.ShowInformation("Période déja saisie", "Edition");
            }
        }

        private void btn_RegroupDeb_Click_1(object sender, RoutedEventArgs e)
        {
            List<object> _LstObj = new List<object>();
            _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstCodeRegroupement);

            Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
            _LstColonneAffich.Add("CODE", "CODE");
            _LstColonneAffich.Add("NOM", "LIBELLE");
            List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
            MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Regroupement");
            ctrl.Closed += new EventHandler(galatee_OkClickedRegroup);
            ctrl.Show();
        }
        void galatee_OkClickedRegroup(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsRegCli lotRegroupement = ctrs.MyObject as Galatee.Silverlight.ServiceAccueil.CsRegCli;
                this.Txt_RegDebut.Text = lotRegroupement.CODE;
                this.Txt_RegDebut.Tag = lotRegroupement;
            }
        }

        private void btn_RegroupFin_Click(object sender, RoutedEventArgs e)
        {
            List<object> _LstObj = new List<object>();
            _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstCodeRegroupement);

            Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
            _LstColonneAffich.Add("CODE", "CODE");
            _LstColonneAffich.Add("NOM", "LIBELLE");
            List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
            MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Regroupement");
            ctrl.Closed += new EventHandler(galatee_OkClickedRegroupFin);
            ctrl.Show();
        }
        void galatee_OkClickedRegroupFin(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsRegCli lotRegroupementFin = ctrs.MyObject as Galatee.Silverlight.ServiceAccueil.CsRegCli;
                this.Txt_RegFin.Text = lotRegroupementFin.CODE;
                this.Txt_RegFin.Tag = lotRegroupementFin;
            }
        }

        private void Txt_RegDebut_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_RegDebut.Text) && this.Txt_RegDebut.Text.Length == SessionObject.Enumere.TailleCodeRegroupement )
            {
                ServiceAccueil.CsRegCli leReg = SessionObject.LstCodeRegroupement.FirstOrDefault(t => t.CODE == this.Txt_RegDebut.Text);
                if (leReg != null)
                    this.Txt_RegDebut.Tag = leReg;

            }
        }

        private void Txt_RegFin_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_RegFin.Text) && this.Txt_RegFin.Text.Length == SessionObject.Enumere.TailleCodeRegroupement)
            {
                ServiceAccueil.CsRegCli leReg = SessionObject.LstCodeRegroupement.FirstOrDefault(t => t.CODE == this.Txt_RegFin.Text);
                if (leReg != null)
                    this.Txt_RegFin.Tag = leReg;
            }
        }

        private void Cbo_Produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Cbo_Produit.SelectedItem != null)
            {
                this.Cbo_Produit.Tag = Cbo_Produit.SelectedItem;
            }
        }

        private void btn_Supprimer_Click(object sender, RoutedEventArgs e)
        {
            if (this.cbo_Periode.SelectedItem != null )
            {
                if (LstPeriode.FirstOrDefault(t => t == (string)this.cbo_Periode.SelectedItem) != null)
                {
                    LstPeriode.Remove((string)this.cbo_Periode.SelectedItem);
                    this.cbo_Periode.ItemsSource = null;
                    this.cbo_Periode.ItemsSource = LstPeriode;
                    if (LstPeriode.Count != 0)
                      this.cbo_Periode.SelectedIndex = 0;
                }
            }
        }
        List<int> lesCentreCaisse = new List<int>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                     
                    if (LstCentrePerimetre.Count == 1)
                    {
                        this.Txt_CodeCentre.Text = LstCentrePerimetre.First().CODE;
                        this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                        this.Txt_CodeCentre.Tag = LstCentrePerimetre.First().PK_ID;
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentreCaisse.Add(item.PK_ID);
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentrePerimetre.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstCentrePerimetre);
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "CENTRE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Liste");
                    ctrl.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");
            }
        }
        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Centre.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsCentre leCentre = (Galatee.Silverlight.ServiceAccueil.CsCentre)ctrs.MyObject;
                this.Txt_CodeCentre.Text = leCentre.CODE;
                this.Txt_LibelleCentre.Text = leCentre.LIBELLE;
                this.Txt_CodeCentre.Tag = leCentre.PK_ID;
            }
            else
                this.btn_Centre.IsEnabled = true;

        }
        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeCentre.Text) && Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    ServiceAccueil.CsCentre _LeCentreClient = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstCentrePerimetre, this.Txt_CodeCentre.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
                    {
                        this.Txt_CodeCentre.Text = _LeCentreClient.CODE;
                        this.Txt_LibelleCentre.Text = _LeCentreClient.LIBELLE;
                        this.Txt_CodeCentre.Tag = _LeCentreClient.PK_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Facturation");

            }
        }
    }
}


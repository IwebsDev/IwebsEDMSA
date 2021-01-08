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
using Galatee.Silverlight;
using Galatee.Silverlight.ServiceInterfaceComptable;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.ServiceAccueil;
using System.Reflection;


namespace Galatee.Silverlight.InterfaceComptable
{
    public partial class FrmGeneration : ChildWindow
    {
        #region Constructeurs

            public FrmGeneration()
            {
                InitializeComponent();
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                this.rdbEncaisse.IsChecked = true;
            }

        #endregion

        #region Variables

            CsSite LeSiteSelect = new CsSite();
            List<CsComptabilisation> LstEcriture = new List<CsComptabilisation>();

            public List<CsCaisse> ListeCaisse { get; set; }
            public List<CsTypeFactureComptable> ListeTypeFactureComptable { get; set; }
            public List<CsCompteSpecifique> ListeCompteSpecifique { get; set; }

            public List<ServiceInterfaceComptable.CsCentreCompte> ListeCentreParametrage { get; set; }
            public List<ServiceInterfaceComptable.CsBanqueCompte > ListeBanque { get; set; }
            public List<ServiceInterfaceComptable.CsCoper> ListeOperation { get; set; }
            public List<ServiceInterfaceComptable.CsOperationComptable> ListeOperationComptable = new List<ServiceInterfaceComptable.CsOperationComptable>();
            public List<ServiceInterfaceComptable.CsTypeCompte> ListeTypeCompte = new List<CsTypeCompte>();

        #endregion


            private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
            {
                ChargerDonneeDuSite();
                RetourneCodeOperation();
                RetourneCaisse();
                RetourneCompteSpecifique();
                RetourneTypeCompte();
                RetourneCentreCompte();
                RetourneBanqueCompte();
                RetourneOperationCompte();
                this.Chk_Mt.Visibility = System.Windows.Visibility.Collapsed;
                this.Chk_Bt.Visibility = System.Windows.Visibility.Collapsed;
            }

            private void OKButton_Click(object sender, RoutedEventArgs e)
            {
                retourneDetailOperation(); 
            }
            private void CancelButton_Click(object sender, RoutedEventArgs e)
            {
                Shared.ClasseMEthodeGenerique.FermetureEcran(this);
            }
            
            private void SearchButton_Click(object sender, RoutedEventArgs e)
            {
                this.DialogResult = false;
            }
            private void EditButton_Click(object sender, RoutedEventArgs e)
            {
                if (this.TBOperationClient.IsSelected)
                {
                    if (DTOperationClientele.ItemsSource != null)
                    {
                        List<CsComptabilisation> lstComptabilisation = (List<CsComptabilisation>)this.DTOperationClientele.ItemsSource;
                        Utility.ActionExportation<ServicePrintings.CsComptabilisation, ServiceInterfaceComptable.CsComptabilisation>(lstComptabilisation, null, string.Empty, SessionObject.CheminImpression, "Operation", "InterfaceComptable", true, "xlsx");

                    }
                }
                else
                {
                    if (DTEcritureComptableFacture.ItemsSource != null)
                    {
                        List<Galatee.Silverlight.ServiceInterfaceComptable.CsEcritureComptable> lstComptabilisation = (List<ServiceInterfaceComptable.CsEcritureComptable>)this.DTEcritureComptableFacture.ItemsSource;
                        Utility.ActionExportation<ServicePrintings.CsEcritureComptable, ServiceInterfaceComptable.CsEcritureComptable>(lstComptabilisation, null, string.Empty, SessionObject.CheminImpression, "Comptabilisation", "InterfaceComptable", true, "xlsx");

                    }
                }
            }
            private void GenererButton_Click(object sender, RoutedEventArgs e)
            {
                VerifieComptabilisation ();
            }

            private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
            {

            }

            private void CmbSite_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (CmbSite.SelectedItem!=null)
                {
                    var liste = ListeCentre.Where(c => c.CODESITE == ((ServiceAccueil.CsSite)this.CmbSite.SelectedItem).CODE);
                    this.cmbCentre.ItemsSource = liste != null ? liste : new List<ServiceAccueil.CsCentre>();
                    this.cmbCentre.DisplayMemberPath = "LIBELLE";
                    this.cmbCentre.SelectedValuePath = "CODE";
                    if (liste != null && liste.Count() == 1)
                        this.cmbCentre.SelectedItem = liste.First();
                    cmbCentre.IsEnabled = true;
                }
               
            }


            private void ChkEncaissement_Checked(object sender, RoutedEventArgs e)
            {

            }

            private void RdbIntervalle_Checked(object sender, RoutedEventArgs e)
            {
                this.dtpDateCaisse.SelectedDate = null;
                this.dtpDateCaisse.IsEnabled = false;

                this.dtpDateDebut.IsEnabled = true;
                this.dtpDateFin.IsEnabled = true;
            }

            private void RdbDate_Checked(object sender, RoutedEventArgs e)
            {
                this.dtpDateDebut.SelectedDate = null;
                this.dtpDateFin.SelectedDate = null;

                this.dtpDateDebut.IsEnabled = false;
                this.dtpDateFin.IsEnabled = false;

                this.dtpDateCaisse.IsEnabled = true;

            }

            private void RdbTous_Checked(object sender, RoutedEventArgs e)
            {
                this.dtpDateDebut.SelectedDate = null;
                this.dtpDateFin.SelectedDate = null;

                this.dtpDateDebut.IsEnabled = false;
                this.dtpDateFin.IsEnabled = false;

                this.dtpDateCaisse.SelectedDate = null;
                this.dtpDateCaisse.IsEnabled = false;
            }

            private void rdbFacture_Checked_1(object sender, RoutedEventArgs e)
            {
                CmbCaisse.IsEnabled = false;
                Chk_TouteCaisse.IsEnabled = false;
            }
            private void cmbCentre_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (this.cmbCentre.SelectedItem != null)
                {
                    this.cmbCentre.Tag =((ServiceAccueil.CsCentre)this.cmbCentre.SelectedItem) ;
                    if (this.rdbFacture.IsChecked == false)
                    {
                        var datasource = ListeCaisse != null ? ListeCaisse.Where(c => c.CENTRE == ((ServiceAccueil.CsCentre)this.cmbCentre.SelectedItem).CODE).ToList() : null;
                        this.CmbCaisse.ItemsSource = datasource.OrderBy(o=>o.NUMCAISSE );
                        this.CmbCaisse.DisplayMemberPath = "NUMCAISSE";
                        CmbCaisse.IsEnabled = true;
                        Chk_TouteCaisse.IsEnabled = true;
                    }
                    else
                    {
                        CmbCaisse.IsEnabled = false ;
                        Chk_TouteCaisse.IsEnabled = false;
                    }
                }

            }

            private void CmbCaisse_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (this.CmbCaisse.SelectedItem != null)
                {
                    this.cmbCentre.Tag = this.cmbCentre.SelectedItem;
                }

            }

            private void btnOpertaion_Click(object sender, RoutedEventArgs e)
            {
                List<Galatee.Silverlight.ServiceInterfaceComptable.CsOperationComptable> CoperOrientation = new List<ServiceInterfaceComptable.CsOperationComptable>();
                if (this.rdbDecaissement.IsChecked == true)
                    CoperOrientation = ListeOperationComptable.Where(t => t.LIBELLECOMPTABLE == "DECAISSEMENT").ToList();
                else if (this.rdbEncaisse.IsChecked == true)
                    CoperOrientation = ListeOperationComptable.Where(t => t.LIBELLECOMPTABLE == "ENCAISSEMENT").ToList();
                else if (this.rdbFacture.IsChecked == true)
                    CoperOrientation = ListeOperationComptable.Where(t => t.LIBELLECOMPTABLE == "VENTE").ToList();

                Galatee.Silverlight.InterfaceComptable.UcOperationCaisse ctrl = new Galatee.Silverlight.InterfaceComptable.UcOperationCaisse(CoperOrientation.OrderBy(t => t.CODE).ToList());
                ctrl.Closed += new EventHandler(galatee_OkClicked);
                ctrl.Show();
            }
            void galatee_OkClicked(object sender, EventArgs e)
            {
                Galatee.Silverlight.InterfaceComptable.UcOperationCaisse ctrs = sender as Galatee.Silverlight.InterfaceComptable.UcOperationCaisse;
                if (ctrs.isOkClick)
                {
                    List<Galatee.Silverlight.ServiceInterfaceComptable.CsOperationComptable> _LeCoper = (List<Galatee.Silverlight.ServiceInterfaceComptable.CsOperationComptable>)ctrs.MyObjectList;
                    if (_LeCoper != null && _LeCoper.Count != 0)
                    {
                        CmbOperation.ItemsSource = null;
                        CmbOperation.ItemsSource = _LeCoper;
                        CmbOperation.DisplayMemberPath = "LIBELLE";
                        CmbOperation.SelectedItem = _LeCoper.First();
                        CmbOperation.Tag = _LeCoper;
                        if (_LeCoper.FirstOrDefault(o => o.CODE == "03" && CmbSite.SelectedItem != null && 
                            ((CsSite)CmbSite.SelectedItem ).CODE  == SessionObject.Enumere.Generale) != null)
                        {
                            this.Chk_Mt.Visibility = System.Windows.Visibility.Visible;
                            this.Chk_Bt.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            this.Chk_Mt.Visibility = System.Windows.Visibility.Collapsed;
                            this.Chk_Bt.Visibility = System.Windows.Visibility.Collapsed;
                        }

                    }
                }
            }

            private void rdbEncaisse_Checked(object sender, RoutedEventArgs e)
            {
                this.btnOpertaion.IsEnabled = true;
                this.CmbSite.IsEnabled = true;
                this.cmbCentre.IsEnabled = true;
                
                //Activation de la selection de caisee
                this.CmbCaisse.IsEnabled = true;
            }

            private void rdbDecaisse_Checked(object sender, RoutedEventArgs e)
            {
                this.btnOpertaion.IsEnabled = true;
                this.CmbSite.IsEnabled = true;
                this.cmbCentre.IsEnabled = true;

                //Desactivation de la selection de caisee
                this.CmbCaisse.IsEnabled = false;
            }
            private void rdbDeCaissement_Checked(object sender, RoutedEventArgs e)
            {
                this.btnOpertaion.IsEnabled = true;
                this.CmbSite.IsEnabled = true;
                this.cmbCentre.IsEnabled = true;

                //Activation de la selection de caisee
                this.CmbCaisse.IsEnabled = true;
            }
            //private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
            //{
            //    RdbTous.IsChecked = true;
            //}


            public void VerifieComptabilisation()
            {
                try
                {
                    if (DTEcritureComptableFacture.ItemsSource  != null )
                    {
                           List<ServiceInterfaceComptable.CsEcritureComptable> ListeLigneComptable = new List<ServiceInterfaceComptable.CsEcritureComptable>() ;
                           ListeLigneComptable = ((List<ServiceInterfaceComptable.CsEcritureComptable>)DTEcritureComptableFacture.ItemsSource).ToList();
                           if (ListeLigneComptable.Count != 0)
                           {
                               var distinctOpeDate = ListeLigneComptable.Select(u => new { u.SITE, u.CENTRE, u.CAISSE, u.DATEOPERATION, u.FK_IDOPERATION }).Distinct();
                               List<ServiceAccueil.CsEcritureComptable> lesLigne = new List<ServiceAccueil.CsEcritureComptable>();
                               foreach (var item in distinctOpeDate)
                               {
                                   ServiceAccueil.CsEcritureComptable leLigne = new ServiceAccueil.CsEcritureComptable();
                                   leLigne.CENTRE = item.CENTRE;
                                   leLigne.SITE = item.SITE;
                                   leLigne.CAISSE = item.CAISSE;
                                   leLigne.DATEOPERATION = item.DATEOPERATION;
                                   leLigne.FK_IDOPERATION = item.FK_IDOPERATION;
                                   lesLigne.Add(leLigne);
                               }
                               AcceuilServiceClient serviceFichier = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                               serviceFichier.IsOperationExisteAsync(lesLigne);
                               serviceFichier.IsOperationExisteCompleted += (s, args) =>
                               {
                                   try
                                   {
                                       if (args.Cancelled || args.Error != null)
                                       {
                                           string error = args.Error.InnerException.ToString();
                                           return;
                                       }
                                       else
                                       {
                                           List<ServiceAccueil.CsEcritureComptable> lstOperatioGene = args.Result.Where(t => t.IsGenere).ToList();
                                           if (lstOperatioGene.Count == 0)
                                               RetourneFichier(ListeLigneComptable);
                                           else
                                           {
                                               string message = string.Empty;
                                               List<string> lstDateCompta = new List<string>();
                                               foreach (var item in lstOperatioGene)
                                               {
                                                   if (!lstDateCompta.Contains(item.DATEOPERATION))
                                                   {
                                                       lstDateCompta.Add(item.DATEOPERATION);

                                                       if (message == string.Empty)
                                                           message = item.DATEOPERATION + " ";
                                                       else
                                                           message = message + "; " + item.DATEOPERATION + " ";
                                                   }
                                               }

                                               var ws = new MessageBoxControl.MessageBoxChildWindow("Comptabilisation", "les journées du " + message + "ont déjà été intégrées", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                                               ws.OnMessageBoxClosed += (l, results) =>
                                               {
                                                   if (ws.Result == MessageBoxResult.OK)
                                                       RetourneFichier(ListeLigneComptable.Where(t => !lstOperatioGene.Any(o => o.SITE == t.SITE && o.CENTRE == t.CENTRE && o.DATEOPERATION == t.DATEOPERATION && o.CAISSE == t.CAISSE && o.FK_IDOPERATION == t.FK_IDOPERATION)).ToList());
                                               };
                                               ws.Show();
                                           }
                                       }
                                   }
                                   catch (Exception ex)
                                   {
                                       Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                                   }
                               };
                           }
                    }
                    else
                        Message.ShowError("Aucune écriture comptable à intégrer", Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);

                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                }
            }




            public void RetourneFichier(List<Galatee.Silverlight.ServiceInterfaceComptable.CsEcritureComptable> lstEcriComptat)
            {
                try
                {
                    if (lstEcriComptat != null && lstEcriComptat.Count > 0)
                    {
                        List<ServiceAccueil.CsEcritureComptable> lstEc = Utility.ConvertListType<ServiceAccueil.CsEcritureComptable, ServiceInterfaceComptable.CsEcritureComptable>(lstEcriComptat);
                        AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                        service.InsertionLigneComptableGenererAsync(lstEc);
                        service.InsertionLigneComptableGenererCompleted += (s, args) =>
                        {
                            try
                            {
                                if (args.Cancelled || args.Error != null)
                                {
                                    string error = args.Error.InnerException.ToString();
                                    return;
                                }
                                else
                                {
                                    if (args.Result == true)
                                    {
                                        Message.Show("Votre fichier a été intégré avec succès", "Information");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                            }
                        };
                    }
                    else
                    {
                        Message.ShowError("Aucune écriture comptable à generer", Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                    }

                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                }
            }






            List<int> lesCentrePerimetre = new List<int>();
            public List<ServiceAccueil.CsCentre> ListeCentre = new List<ServiceAccueil.CsCentre>();
            public List<ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
            private void ChargerDonneeDuSite()
            {
                try
                {
                    if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                    {
                        ListeCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODESITE != SessionObject.Enumere.CodeSiteScaBT && p.CODESITE != SessionObject.Enumere.CodeSiteScaMT).ToList(), UserConnecte.listeProfilUser);
                        //ListeCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                        lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(ListeCentre);
                        ServiceAccueil.CsSite AgenceCentral = lstSite.FirstOrDefault(t => t.CODE == SessionObject.Enumere.Generale);
                        //ServiceAccueil.CsSite AgenceCentral = lstSite.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSiteScaBT || t.CODE ==SessionObject.Enumere.CodeSiteScaMT );
                        if (AgenceCentral != null)
                            AgenceCentral.LIBELLE = "AGENCE CENTRALE";

                        foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in ListeCentre.Where(p => p.CODESITE != SessionObject.Enumere.CodeSiteScaMT && p.CODESITE != SessionObject.Enumere.CodeSiteScaBT).ToList())
                            lesCentrePerimetre.Add(item.PK_ID);

                        if (AgenceCentral != null)
                        {
                            ServiceAccueil.CsCentre leCentreAgenceGeneral = ListeCentre.FirstOrDefault(o => o.CODE == SessionObject.Enumere.Generale);
                            leCentreAgenceGeneral.LIBELLE = "AGENCE CENTRALE";
                            List<string> lstCodeAgGnral = ListeCentre.Where(t => t.CODESITE == SessionObject.Enumere.Generale && t.CODE != SessionObject.Enumere.Generale).Select(u => u.CODE).ToList();
                            ListeCentre = ListeCentre.Where(t => !lstCodeAgGnral.Contains(t.CODE)).ToList();
                        }

                        this.CmbSite.ItemsSource = lstSite;
                        this.CmbSite.DisplayMemberPath = "LIBELLE";
                        this.CmbSite.SelectedValuePath = "CODE";
                        CmbSite.IsEnabled = true;


                        this.cmbCentre.ItemsSource = ListeCentre;
                        this.cmbCentre.DisplayMemberPath = "LIBELLE";
                        this.cmbCentre.SelectedValuePath = "CODE";
                        cmbCentre.IsEnabled = true;

                        return;
                    }
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        ListeCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                        lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(ListeCentre);
                        foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in ListeCentre)
                            lesCentrePerimetre.Add(item.PK_ID);

                        ServiceAccueil.CsSite AgenceCentral = lstSite.FirstOrDefault(t => t.CODE == SessionObject.Enumere.Generale);
                        if (AgenceCentral != null)
                            AgenceCentral.LIBELLE = "AGENCE CENTRAL";

                        this.CmbSite.ItemsSource = lstSite;
                        this.CmbSite.DisplayMemberPath = "LIBELLE";
                        this.CmbSite.SelectedValuePath = "CODE";
                        CmbSite.IsEnabled = true;


                        this.cmbCentre.ItemsSource = ListeCentre;
                        this.cmbCentre.DisplayMemberPath = "LIBELLE";
                        this.cmbCentre.SelectedValuePath = "CODE";
                        cmbCentre.IsEnabled = true;
                    };
                    service.ListeDesDonneesDesSiteAsync(false);
                    service.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }





            public void RetourneCaisse()
            {
                try
                {
                    InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                    service.RetourneCaisseCompleted += (s, args) =>
                    {
                        try
                        {
                            if (args.Cancelled || args.Error != null || args.Result == null)
                            {
                                string error = args.Error.InnerException.ToString();
                                return;
                            }
                            else
                            {
                                this.CmbCaisse.ItemsSource = null;
                                if (args.Result != null && args.Result.Count != 0)
                                {
                                    ListeCaisse = args.Result;

                                    this.CmbCaisse.ItemsSource = this.cmbCentre.SelectedItem != null ? ListeCaisse.Where(c => c.CENTRE == ((ServiceAccueil.CsCentre)this.cmbCentre.SelectedItem).CODE).OrderBy(y => y.NUMCAISSE).ToList() : ListeCaisse.OrderBy(o => o.NUMCAISSE).ToList();
                                    this.CmbCaisse.DisplayMemberPath = "NUMCAISSE";
                                    CmbCaisse.IsEnabled = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                        }
                    };
                    service.RetourneCaisseAsync();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }






            public void RetourneAction_A_Impacte_Financier()
            {
                try
                {
                    InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                    service.RetourneTypeFactureCompleted += (s, args) =>
                    {
                        try
                        {
                            if (args.Cancelled || args.Error != null || args.Result == null)
                            {
                                string error = args.Error.InnerException.ToString();
                                return;
                            }
                            else
                            {
                                if (args.Result != null && args.Result.Count != 0)
                                {
                                    ListeTypeFactureComptable = new List<CsTypeFactureComptable>();
                                    ListeTypeFactureComptable = args.Result;
                                }
                                else
                                {
                                    Message.ShowInformation("Le système n'a trouvé aucun type de facture ", "Avertissement");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                        }
                    };
                    service.RetourneTypeFactureAsync();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public void RetourneCompteSpecifique()
            {
                try
                {
                    InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                    service.RetourneCompteSpecifiqueCompleted += (s, args) =>
                    {
                        try
                        {
                            if (args.Cancelled || args.Error != null || args.Result == null)
                            {
                                string error = args.Error.InnerException.ToString();
                                return;
                            }
                            else
                            {
                                if (args.Result != null && args.Result.Count != 0)
                                {
                                    ListeCompteSpecifique = new List<CsCompteSpecifique>();
                                    ListeCompteSpecifique = args.Result;

                                }
                                else
                                {
                                    Message.ShowInformation("Le système n'a trouvé aucun Compte spécifique dans la base de données ", "Avertissement");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                        }
                    };
                    service.RetourneCompteSpecifiqueAsync();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            
            public void RetourneTypeCompte()
            {
                try
                {
                    InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                    service.RetourneTypeCompteCompleted += (s, args) =>
                    {
                        try
                        {
                            if (args.Cancelled || args.Error != null || args.Result == null)
                            {
                                string error = args.Error.InnerException.ToString();
                                return;
                            }
                            else
                            {
                                if (args.Result != null && args.Result.Count != 0)
                                {
                                    ListeTypeCompte = new List<CsTypeCompte>();
                                    ListeTypeCompte = args.Result;
                                }
                                else
                                {
                                    Message.ShowInformation("Le système n'a trouvé aucun Compte spécifique dans la base de données ", "Avertissement");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                        }
                    };
                    service.RetourneTypeCompteAsync();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public void RetourneCentreCompte()
            {
                try
                {
                    InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                    service.RetourneParamCentreAsync();
                    service.RetourneParamCentreCompleted += (s, args) =>
                    {
                        try
                        {
                            if (args.Cancelled || args.Error != null || args.Result == null)
                            {
                                string error = args.Error.InnerException.ToString();
                                return;
                            }
                            else
                            {
                                if (args.Result != null && args.Result.Count != 0)
                                {
                                    ListeCentreParametrage = new List<CsCentreCompte>();
                                    ListeCentreParametrage = args.Result;
                                }
                                else
                                {
                                    Message.ShowInformation("Le système n'a trouvé aucun Compte spécifique dans la base de données ", "Avertissement");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                        }
                    };

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public void RetourneBanqueCompte()
            {
                try
                {
                    InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                    service.RetourneBanqueCentreAsync();
                    service.RetourneBanqueCentreCompleted += (s, args) =>
                    {
                        try
                        {
                            if (args.Cancelled || args.Error != null || args.Result == null)
                            {
                                string error = args.Error.InnerException.ToString();
                                return;
                            }
                            else
                            {
                                if (args.Result != null && args.Result.Count != 0)
                                {
                                    ListeBanque = new List<CsBanqueCompte >();
                                    ListeBanque = args.Result;
                                }
                                else
                                {
                                    Message.ShowInformation("Le système n'a trouvé aucun Compte spécifique dans la base de données ", "Avertissement");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                        }
                    };

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public void RetourneOperationCompte()
            {
                try
                {
                    InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                    service.RetourneOperationComptableCompleted += (s, args) =>
                    {
                        try
                        {
                            if (args.Cancelled || args.Error != null || args.Result == null)
                            {
                                string error = args.Error.InnerException.ToString();
                                return;
                            }
                            else
                            {
                                if (args.Result != null && args.Result.Count != 0)
                                {
                                    ListeOperationComptable = new List<ServiceInterfaceComptable.CsOperationComptable>();
                                    ListeOperationComptable = args.Result;
                                }
                                else
                                {
                                    Message.ShowInformation("Le système n'a trouvé aucun Compte spécifique dans la base de données ", "Avertissement");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                        }
                    };
                    service.RetourneOperationComptableAsync();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public void RetourneCodeOperation()
            {
                try
                {
                    InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                    service.RetourneCodeOperationCompleted += (s, args) =>
                    {
                        try
                        {
                            if (args.Cancelled || args.Error != null
                                )
                            {
                                string error = args.Error.InnerException.ToString();
                                return;
                            }
                            else
                            {
                                if (args.Result != null && args.Result.Count != 0)
                                {
                                    ListeOperation = args.Result;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                        }
                    };
                    service.RetourneCodeOperationAsync();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public List<CsComptabilisation> RetourneEcritureComptableByList(List<CsComptabilisation> LstEcritureCompta)
            {
                try
                {
                    List<CsComptabilisation> EcritureComptable = new List<CsComptabilisation>();
                    if (LstEcritureCompta != null || LstEcritureCompta.Count != 0)
                    {
                        #region Affecter les montants des par regroupement de coper aux éléments des la listes des ecriture comptables qui ont le même coper a la  même date
                        foreach (CsComptabilisation r in LstEcritureCompta.OrderByDescending(p => p.DATECREATION))
                        {
                            CsComptabilisation Ecriture = EcritureComptable.FirstOrDefault(t => t.COPER == r.COPER && t.DATECREATION == r.DATECREATION);
                            if (Ecriture == null)
                               EcritureComptable.Add(r);
                            else
                            {
                              Ecriture.MONTANTCREDIT =Ecriture.MONTANTHT  + r.MONTANTCREDIT ;
                              Ecriture.MONTANTDEBIT = Ecriture.MONTANTDEBIT + r.MONTANTDEBIT ;
                              Ecriture.MONTANT  =Ecriture.MONTANT + r.MONTANT ;
                            }
                        }
                        #endregion
                    }
                    return EcritureComptable;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            List<Galatee.Silverlight.ServiceInterfaceComptable.CsOperationComptable> LstOperationSelect = new List<CsOperationComptable>();
            public void retourneDetailOperation()
            {
                try
                {
                    this.DTEcritureComptableFacture.ItemsSource = null;
                    this.DTOperationClientele.ItemsSource = null;
                    this.txt_Credit.Text = string.Empty;
                    this.txt_Debit.Text = string.Empty;
                    this.txt_total.Text = string.Empty;
                    prgBar.Visibility = System.Windows.Visibility.Visible;

                    #region Recupération des criteres de selection
                    List<Galatee.Silverlight.ServiceInterfaceComptable.CsOperationComptable> lstCopeOper = new List<Galatee.Silverlight.ServiceInterfaceComptable.CsOperationComptable>();
                    //Recuperation du centre selectionné
                    ServiceAccueil.CsCentre SelectCentre = this.cmbCentre.SelectedItem as ServiceAccueil.CsCentre;

                    //Recuperation de la liste des type opérations à prendre en compte
                    if (this.CmbOperation.Tag != null)
                    {
                        LstOperationSelect = (List<Galatee.Silverlight.ServiceInterfaceComptable.CsOperationComptable>)CmbOperation.Tag;
                        if (LstOperationSelect != null && LstOperationSelect.Count != 0)
                                lstCopeOper.AddRange(LstOperationSelect);
                    }
                    else
                        lstCopeOper.AddRange(ListeOperationComptable.ToList());

                    DateTime? DateDebut = null;
                    if (!string.IsNullOrEmpty(this.dtpDateDebut.Text))
                        DateDebut = Convert.ToDateTime(this.dtpDateDebut.Text);

                    DateTime? DateFin = null;
                    if (!string.IsNullOrEmpty(this.dtpDateFin.Text))
                        DateFin = Convert.ToDateTime(this.dtpDateFin.Text);

                     
                    if (!string.IsNullOrEmpty(this.dtpDateCaisse.Text))
                        DateDebut = Convert.ToDateTime(this.dtpDateCaisse.Text);

                    if (DateDebut == null && DateFin == null)
                    {
                        Message.ShowInformation("Veuillez sélectionner une date", "Info");
                        return;
                    }
                    List<CsCaisse> LstCaisse = new List<CsCaisse>();
                    List<ServiceAccueil.CsCentre> LstCentreSelect = new List<ServiceAccueil.CsCentre>();
                    if (this.rdbEncaisse.IsChecked == true)
                    {
                        //Recupération de la caisse selectionné
                        CsCaisse SelectCaisse = this.CmbCaisse.IsEnabled ? this.CmbCaisse.SelectedItem as CsCaisse : null;
                        string laCaisse = string.Empty;
                        if (SelectCaisse != null)
                        {
                            laCaisse = SelectCaisse.NUMCAISSE;
                            LstCaisse.Add(SelectCaisse);
                        }
                        else
                        {
                            if (cmbCentre.SelectedItem != null)
                            {
                                List<CsCaisse> liste = ListeCaisse.Where(c => c.FK_IDCENTRE  == ((ServiceAccueil.CsCentre )this.cmbCentre.SelectedItem).PK_ID ).ToList();
                                LstCaisse.AddRange(liste);
                            }
                        }
                        retourneEncaissement(LstCaisse, DateDebut, DateFin, LstOperationSelect);
                    }
                    else if (this.rdbFacture.IsChecked == true)
                    {
                        if (cmbCentre.SelectedItem != null)
                        {
                            string SiteSelect = ((ServiceAccueil.CsSite)this.CmbSite.SelectedItem).CODE;
                            if (SiteSelect == SessionObject.Enumere.Generale)
                            {
                                if (this.Chk_Bt.IsChecked == true)
                                    LstCentreSelect.AddRange(SessionObject.LstCentre.Where(t => t.CODESITE == SessionObject.Enumere.CodeSiteScaBT).ToList());
                                else if (this.Chk_Mt.IsChecked == true)
                                    LstCentreSelect.AddRange(SessionObject.LstCentre.Where(t => t.CODESITE == SessionObject.Enumere.CodeSiteScaMT).ToList());
                                else
                                    LstCentreSelect.AddRange(SessionObject.LstCentre.Where(t => t.CODESITE == SessionObject.Enumere.CodeSiteScaBT ||
                                    t.CODESITE == SessionObject.Enumere.CodeSiteScaMT).ToList());
                            }
                            else
                                LstCentreSelect.Add((ServiceAccueil.CsCentre)cmbCentre.Tag);
                        }
                        else
                        {
                            if (CmbSite.SelectedItem != null)
                            {
                                List<ServiceAccueil.CsCentre> liste = ListeCentre.Where(c => c.CODESITE == ((ServiceAccueil.CsSite)this.CmbSite.SelectedItem).CODE).ToList();
                                LstCentreSelect.AddRange(liste);
                            }
                        }
                        retourneFacture(LstCentreSelect, DateDebut, DateFin, LstOperationSelect);
                    }
                    else if (this.rdbDecaissement.IsChecked == true)
                    {
                        //Recupération de la caisse selectionné
                        CsCaisse SelectCaisse = this.CmbCaisse.IsEnabled ? this.CmbCaisse.SelectedItem as CsCaisse : null;
                        string laCaisse = string.Empty;
                        if (SelectCaisse != null)
                        {
                            laCaisse = SelectCaisse.NUMCAISSE;
                            LstCaisse.Add(SelectCaisse);
                        }
                        else
                        {
                            if (cmbCentre.SelectedItem != null)
                            {
                                List<CsCaisse> liste = ListeCaisse.Where(c => c.FK_IDCENTRE == ((ServiceAccueil.CsCentre)this.cmbCentre.SelectedItem).PK_ID).ToList();
                                LstCaisse.AddRange(liste);
                            }
                        }
                        retourneDEncaissement(LstCaisse, DateDebut, DateFin, LstOperationSelect);
                    }
                    #endregion
                } 
                catch (Exception)
                {
                    throw;
                }

            }
            public void retourneDEncaissement(List<CsCaisse> lstCaisses, DateTime? DateDebut, DateTime? DateFin, List<CsOperationComptable> LstOperationCompableSelect)
            {
                try
                {
                    string compte = string.Empty;
                    InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                    service.RetourneDEncaissementAsync(LstOperationCompableSelect, lstCaisses, DateDebut, DateFin, UserConnecte.matricule, ((ServiceAccueil.CsSite)CmbSite.SelectedItem).CODE);
                    service.RetourneDEncaissementCompleted += (s, args) =>
                    {
                        try
                        {
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            if (args.Cancelled || args.Error != null)
                            {
                                string error = args.Error.InnerException.ToString();
                                return;
                            }
                            if (args.Result != null && args.Result.Count == 0)
                            {
                                Message.ShowInformation("Aucune données trouvées", "Comptabilisation");
                                return;
                            }
                            else
                            {
                                Dictionary<List<CsComptabilisation>, List<ServiceInterfaceComptable.CsEcritureComptable>> lstEncaissement = new Dictionary<List<CsComptabilisation>, List<ServiceInterfaceComptable.CsEcritureComptable>>();
                                this.DTOperationClientele.ItemsSource = null;
                                if (args.Result != null && args.Result.Count != 0)
                                {
                                    lstEncaissement = args.Result;
                                    foreach (var item in lstEncaissement)
                                    {
                                        #region Alimentation des grides opérations clientelles
                                        this.DTOperationClientele.ItemsSource = null;
                                        this.DTOperationClientele.ItemsSource = item.Key;
                                        txt_total.Text = item.Key.Sum(t => t.MONTANT).ToString(SessionObject.FormatMontant);
                                        #endregion

                                        #region Affectation des montants des operations grouper par coper et par date d'encaissement
                                        this.DTEcritureComptableFacture.ItemsSource = null;
                                        this.DTEcritureComptableFacture.ItemsSource = item.Value.OrderBy(c => c.DATECREATION).ThenBy(o => o.CAISSE).ThenByDescending(u => u.DC).ToList();
                                        #endregion
                                        this.txt_Credit.Text = item.Value.Where(t => t.DC == SessionObject.Enumere.Credit).Sum(u => u.MONTANT).Value.ToString(SessionObject.FormatMontant);
                                        this.txt_Debit.Text = item.Value.Where(t => t.DC == SessionObject.Enumere.Debit).Sum(u => u.MONTANT).Value.ToString(SessionObject.FormatMontant);
                                    }
                                }
                                else
                                    Message.ShowInformation("Aucun élément trouvé pour les critères choisi", "Info");
                            }
                        }
                        catch (Exception ex)
                        {
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                        }
                    };

                }
                catch (Exception)
                {

                    throw;
                }

            }

            public void retourneFacture(List<ServiceAccueil.CsCentre> lstIdCentre, DateTime? DateDebut, DateTime? DateFin, List<CsOperationComptable> LstOperationCompableSelect)
            {
                try
                {
                    string Compte = string.Empty;
                    InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                    service.RetournefactureAsync(LstOperationCompableSelect, lstIdCentre.Select(t=>t.PK_ID).ToList(),DateDebut, DateFin,UserConnecte.matricule ,((CsSite )this.CmbSite.SelectedItem).CODE  );
                    service.RetournefactureCompleted += (s, args) =>
                    {
                        try
                        {
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            if (args.Cancelled || args.Error != null)
                            {
                                string error = args.Error.InnerException.ToString();
                                return;
                            }
                            if (args.Result != null && args.Result.Count == 0)
                            {
                                Message.ShowInformation("Aucune données trouvées", "Comptabilisation");
                                return;
                            }
                            else
                            {
                                Dictionary<List<CsComptabilisation>, List<ServiceInterfaceComptable.CsEcritureComptable>> lstFacture= new Dictionary<List<CsComptabilisation>, List<ServiceInterfaceComptable.CsEcritureComptable>>();
                                this.DTOperationClientele.ItemsSource = null;
                                if (args.Result != null && args.Result.Count != 0)
                                {
                                    lstFacture = args.Result;
                                    foreach (var item in lstFacture)
                                    {
                                        #region Alimentation des grides opérations clientelles
                                        this.DTOperationClientele.ItemsSource = null;
                                        this.DTOperationClientele.ItemsSource = item.Key;
                                        txt_total.Text = item.Key.Sum(t => t.MONTANT).ToString(SessionObject.FormatMontant);
                                        #endregion
                                        #region Affectation des montants des operations grouper par coper et par date d'encaissement
                                        this.DTEcritureComptableFacture.ItemsSource = null;
                                        this.DTEcritureComptableFacture.ItemsSource = item.Value.OrderBy(c => c.DESCRIPSTIONOPERATION ).ThenBy(o => o.DATEOPERATION).ThenByDescending(u => u.DEBIT).ToList();
                                        #endregion
                                        this.txt_Credit.Text = item.Value.Where(t => t.DC == SessionObject.Enumere.Credit).Sum(u => u.MONTANT).Value.ToString(SessionObject.FormatMontant);
                                        this.txt_Debit.Text = item.Value.Where(t => t.DC == SessionObject.Enumere.Debit).Sum(u => u.MONTANT).Value.ToString(SessionObject.FormatMontant);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string rt = Compte;
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                        }
                    };
                }
                catch (Exception)
                {

                    prgBar.Visibility = System.Windows.Visibility.Collapsed;
                    throw;
                }

            }
            public void retourneEncaissement(List<CsCaisse> lstCaisses, DateTime? DateDebut, DateTime? DateFin, List<CsOperationComptable> LstOperationCompableSelect)
            {
                try
                {
                    string compte = string.Empty;
                    InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                    service.RetourneEncaissementAsync(LstOperationCompableSelect, lstCaisses, DateDebut, DateFin,UserConnecte.matricule ,((ServiceAccueil.CsSite)CmbSite.SelectedItem).CODE);
                    service.RetourneEncaissementCompleted += (s, args) =>
                    {
                        try
                        {
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            if (args.Cancelled || args.Error != null)
                            {
                                string error = args.Error.InnerException.ToString();
                                return;
                            }
                            if (args.Result != null && args.Result.Count == 0)
                            {
                                Message.ShowInformation("Aucune données trouvées", "Comptabilisation");
                                return;
                            }
                            else
                            {
                                Dictionary<List<CsComptabilisation>, List<ServiceInterfaceComptable.CsEcritureComptable>> lstEncaissement = new Dictionary<List<CsComptabilisation>, List<ServiceInterfaceComptable.CsEcritureComptable>>();
                                this.DTOperationClientele.ItemsSource = null;
                                if (args.Result != null && args.Result.Count != 0)
                                {
                                    lstEncaissement = args.Result;
                                    foreach (var item in lstEncaissement)
                                    {
                                        #region Alimentation des grides opérations clientelles
                                        this.DTOperationClientele.ItemsSource = null;
                                        this.DTOperationClientele.ItemsSource = item.Key ;
                                        txt_total.Text = item.Key.Sum(t => t.MONTANT).ToString(SessionObject.FormatMontant);
                                        #endregion

                                        #region Affectation des montants des operations grouper par coper et par date d'encaissement
                                        this.DTEcritureComptableFacture.ItemsSource = null;
                                        this.DTEcritureComptableFacture.ItemsSource = item.Value.OrderBy(c => c.DATECREATION).ThenBy(o => o.CAISSE).ThenByDescending(u => u.DC).ToList();
                                        #endregion
                                        this.txt_Credit.Text = item.Value.Where(t => t.DC == SessionObject.Enumere.Credit).Sum(u => u.MONTANT).Value.ToString(SessionObject.FormatMontant);
                                        this.txt_Debit.Text = item.Value.Where(t => t.DC == SessionObject.Enumere.Debit).Sum(u => u.MONTANT).Value.ToString(SessionObject.FormatMontant);
                                    }
                                }
                                else
                                    Message.ShowInformation("Aucun élément trouvé pour les critères choisi", "Info");
                            }
                        }
                        catch (Exception ex)
                        {
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                        }
                    };

                }
                catch (Exception)
                {
                    throw;
                }

            }

            private void Chk_Mt_Checked(object sender, RoutedEventArgs e)
            {

            }
    }
}


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
//using Galatee.Silverlight.ServiceAccueil;
using System.Reflection;


namespace Galatee.Silverlight.InterfaceComptable
{
    public partial class FrmGenerationSCGC : ChildWindow
    {
        #region Constructeurs

        public FrmGenerationSCGC()
            {
                InitializeComponent();
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
            }

        #endregion

        #region Variables

            Galatee.Silverlight.ServiceAccueil.CsSite LeSiteSelect = new Galatee.Silverlight.ServiceAccueil.CsSite();
            List<CsComptabilisation> LstEcriture = new List<CsComptabilisation>();

            public List<CsCaisse> ListeCaisse { get; set; }
            public List<CsTypeFactureComptable> ListeTypeFactureComptable { get; set; }
            public List<CsCompteSpecifique> ListeCompteSpecifique { get; set; }
            public List<CsEcritureComptable> ListeLigneComptable  ;

            public List<ServiceInterfaceComptable.CsCentreCompte> ListeCentreParametrage { get; set; }
            public List<ServiceInterfaceComptable.CsBanqueCompte > ListeBanque { get; set; }
            public List<ServiceInterfaceComptable.CsCoper> ListeOperation { get; set; }
            public List<ServiceInterfaceComptable.CsOperationComptable> ListeOperationComptable = new List<ServiceInterfaceComptable.CsOperationComptable>();
            public List<ServiceInterfaceComptable.CsTypeCompte> ListeTypeCompte = new List<CsTypeCompte>();

        #endregion


            private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
            {
                ChargerDonneeDuSite();
                RetourneOperationCompte();
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
                        Utility.ActionDirectOrientation<ServicePrintings.CsComptabilisation, ServiceInterfaceComptable.CsComptabilisation>(lstComptabilisation, null, SessionObject.CheminImpression, "Operation", "InterfaceComptable", true);
                    }
                }
                else
                {
                    if (DTEcritureComptableFacture.ItemsSource != null)
                    {
                        List<CsEcritureComptable> lstComptabilisation = (List<CsEcritureComptable>)this.DTEcritureComptableFacture.ItemsSource;
                        Utility.ActionDirectOrientation<ServicePrintings.CsEcritureComptable, ServiceInterfaceComptable.CsEcritureComptable>(lstComptabilisation, null, SessionObject.CheminImpression, "Comptabilisation", "InterfaceComptable", true);
                        Utility.ActionExportation<ServicePrintings.CsEcritureComptable, ServiceInterfaceComptable.CsEcritureComptable>(lstComptabilisation, null, string.Empty, SessionObject.CheminImpression, "Comptabilisation", "InterfaceComptable", true, "xlsx");
                    }
                }
            }
            private void GenererButton_Click(object sender, RoutedEventArgs e)
            {
                VerifieComptabilisation ();
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


            public void VerifieComptabilisation()
            {
                try
                {
                    if (DTEcritureComptableFacture.ItemsSource != null)
                    {
                        List<ServiceInterfaceComptable.CsEcritureComptable> ListeLigneComptable = new List<ServiceInterfaceComptable.CsEcritureComptable>();
                        ListeLigneComptable = ((List<ServiceInterfaceComptable.CsEcritureComptable>)DTEcritureComptableFacture.ItemsSource).ToList();
                        if (ListeLigneComptable.Count != 0)
                        {
                            var distinctOpeDate = ListeLigneComptable.Select(u => new { u.SITE, u.CENTRE, u.CAISSE, u.DATEOPERATION, u.FK_IDOPERATION }).Distinct();
                            List<ServiceInterfaceComptable.CsEcritureComptable> lesLigne = new List<ServiceInterfaceComptable.CsEcritureComptable>();
                            foreach (var item in distinctOpeDate)
                            {
                                ServiceInterfaceComptable.CsEcritureComptable leLigne = new ServiceInterfaceComptable.CsEcritureComptable();
                                leLigne.CENTRE = item.CENTRE;
                                leLigne.SITE = item.SITE;
                                leLigne.CAISSE = item.CAISSE;
                                leLigne.DATEOPERATION = item.DATEOPERATION;
                                leLigne.FK_IDOPERATION = item.FK_IDOPERATION;
                                lesLigne.Add(leLigne);
                            }
                            InterfaceComptableServiceClient serviceFichier = new InterfaceComptableServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("InterfaceComptable"));
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
                                        List<ServiceInterfaceComptable.CsEcritureComptable> lstOperatioGene = args.Result.Where(t => t.IsGenere).ToList();
                                        if (lstOperatioGene.Count == 0)
                                            RetourneFichier(ListeLigneComptable);
                                        else
                                        {
                                            string message = string.Empty;
                                            foreach (var item in lstOperatioGene)
                                            {

                                                if (message == string.Empty)
                                                    message = item.DATEOPERATION + " ";
                                                else
                                                    message = message + "; " + item.DATEOPERATION + " ";
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
                        Message.ShowError("Aucune criture comptable à intégrer", Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);

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
                        Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                        lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(ListeCentre);
                        ServiceAccueil.CsSite AgenceCentral = lstSite.FirstOrDefault(t => t.CODE == SessionObject.Enumere.Generale);
                        if (AgenceCentral != null)
                            AgenceCentral.LIBELLE = "AGENCE CENTRALE";

                        foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in ListeCentre.Where(p=>p.CODESITE != SessionObject.Enumere.CodeSiteScaMT && p.CODESITE != SessionObject.Enumere.CodeSiteScaBT ).ToList())
                            lesCentrePerimetre.Add(item.PK_ID);

                        if (AgenceCentral != null)
                        {
                            ServiceAccueil.CsCentre leCentreAgenceGeneral = ListeCentre.FirstOrDefault(o => o.CODE == SessionObject.Enumere.Generale);
                            leCentreAgenceGeneral.LIBELLE = "AGENCE CENTRALE";
                            List<string> lstCodeAgGnral = ListeCentre.Where(t => t.CODESITE == SessionObject.Enumere.Generale && t.CODE != SessionObject.Enumere.Generale).Select(u => u.CODE).ToList();
                            ListeCentre = ListeCentre.Where(t => !lstCodeAgGnral.Contains(t.CODE)).ToList();
                        }
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
                            AgenceCentral.LIBELLE = "AGENCE CENTRALE";
                    };
                    service.ListeDesDonneesDesSiteAsync(false);
                    service.CloseAsync();
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
                    CsOperationComptable OperationCompableSelect = ListeOperationComptable.FirstOrDefault(t => t.CODE == "09");
                    retourneMiseAjourGC(DateDebut, DateFin, OperationCompableSelect);
                    #endregion
                } 
                catch (Exception)
                {
                    throw;
                }

            }
            public void retourneMiseAjourGC(DateTime? DateDebut, DateTime? DateFin, CsOperationComptable OperationCompableSelect)
            {
                try
                {
                    string compte = string.Empty;
                    InterfaceComptableServiceClient service = new InterfaceComptableServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("InterfaceComptable"));
                    service.RetourneMiseAJourGrandCompteAsync(OperationCompableSelect, DateDebut, DateFin, UserConnecte.matricule,string.Empty );
                    service.RetourneMiseAJourGrandCompteCompleted += (s, args) =>
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
                                Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> lstEncaissement = new Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>>();
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


  

         

         
    }
}


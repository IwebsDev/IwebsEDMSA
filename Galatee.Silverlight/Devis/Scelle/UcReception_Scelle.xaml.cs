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
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.ServiceAccueil;
 

namespace Galatee.Silverlight.Accueil
{
    public partial class UcReception_Scelle : ChildWindow
    {
        List<Galatee.Silverlight.ServiceAccueil.CsUtilisateur> lstAllUser = new List<Galatee.Silverlight.ServiceAccueil.CsUtilisateur>();
        List<CsActivite> lstAllActivite = new List<CsActivite>();
        List<CsDscelle> lademande = new List<CsDscelle>();
        List<CsLotScelle> ListLotAffecter_Selectionner = new List<CsLotScelle>();
        int EtapeActuelle;
        int Nbr_ScelleDemandeRestant = 0;
        public UcReception_Scelle(int pk_id)
        {
            InitializeComponent();
            ChargerService();
            ChargerCentre();
            ChargeListeUser();
            ChargeDonneDemande(pk_id);
            //ChargeLot();

            ChargeScelleAffecter(pk_id);
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }
        public UcReception_Scelle(List<int> demande, int etape)
        {
            InitializeComponent();

            this.EtapeActuelle = etape;

            ChargerService();
            ChargerCentre();
            ChargeListeUser();
            ChargeDonneDemande(demande.First());
            //ChargeLot();

            ChargeScelleAffecter(demande.First());
            prgBar.Visibility = System.Windows.Visibility.Collapsed;

        }

        private void ChargeScelleAffecter(int pk_id)
        {
            List<CsDscelle> lademande = new List<CsDscelle>();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneListeDetailAffectationScelleCompleted += (s, args) =>
            {
                try
                {
                    if (args != null && args.Cancelled)
                        return;
                    var MaListeScelle=args.Result.Where(sc=>sc.EstLivre==false);
                    if (MaListeScelle!=null && MaListeScelle.Count()>0)
                    {
                        var MaListeScelle_= MaListeScelle.ToList();
                         MaListeScelle_.ForEach(sc => sc.EstLivre = true);
                         DgLotMag.ItemsSource = MaListeScelle_.OrderBy(t=>t.Nuemro_Scelle).ToList();
                    }
                    else
                    {
                        Message.ShowWarning("Aucun scelle à disponible pour réception", "Information");
                        OKButton.Visibility = Visibility.Collapsed;
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }

            };
            service.RetourneListeDetailAffectationScelleAsync(pk_id);
        }

        private void ChargerCentre()
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
        private void ChargeListeUser()
        {
            try
            {

                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeAllUserCompleted += (sr, res) =>
                {

                    if (res != null && res.Cancelled)
                        return;
                    lstAllUser = res.Result;


                };
                service1.RetourneListeAllUserAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        private void ChargerService()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneListeActiviteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        lstAllActivite = args.Result;

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }

                };
                service.RetourneListeActiviteAsync(false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }
        //private void ChargeLot()
        //{
        //    Galatee.Silverlight.ServiceDevis.DevisServiceClient service = new Galatee.Silverlight.ServiceDevis.DevisServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Devis"));
        //    service.RetourneListeScelleCompleted += (s, args) =>
        //    {
        //        try
        //        {
        //            if (args != null && args.Cancelled)
        //                return;
        //            DgLotMag.ItemsSource = args.Result;
        //        }
        //        catch (Exception ex)
        //        {
        //            Message.ShowError(ex, "Erreur");
        //        }

        //    };
        //    service.RetourneListeScelleAsync();
        //}
        private void ChargeDonneDemande(int pk_id)
        {

            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneListeDemandeScelleCompleted += (s, args) =>
            {
                try
                {
                    if (args != null && args.Cancelled)
                        return;
                    lademande = args.Result;

                    if (lademande != null)
                    {
                        txtcentre.Text = lademande.First().LIBELLECENTREDESTINATAIRE;
                        txtcentre.Tag = lademande.First().FK_IDCENTRE;

                        txtAgent.Text = lademande.First().LIBELLESITEAGENT ;
                        txtAgent.Tag = lademande.First().FK_IDAGENT ;

                        txtService.Text = lademande.First().LIBELLEACTIVITE ;
                        txtService.Tag = lademande.First().FK_IDACTIVITE ;

                        string NombreScelle = lademande.FirstOrDefault().NOMBRE_DEM != null ? lademande.FirstOrDefault().NOMBRE_DEM.ToString() : string.Empty;
                        string Couleur = lademande.FirstOrDefault().LIBELLECOULEUR != null ? lademande.FirstOrDefault().LIBELLECOULEUR : string.Empty;
                        txtnombreDem.Text = NombreScelle;
                        txtCouleur.Text = Couleur;
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }

            };
            service.RetourneListeDemandeScelleAsync(pk_id);

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            var ListeScelle = (List<CsDetailAffectationScelle>)DgLotMag.ItemsSource;
            List<CsDetailAffectationScelle> ListeScelleAValider = ListeScelle.Where(sc => sc.EstLivre == true).ToList();
            ListeScelleAValider.ForEach(ds => ds.Date_Reception = DateTime.Now);
            if(ListeScelleAValider!=null)
                ValidationReception(ListeScelleAValider.ToList());
            this.DialogResult = true;
        }

        private void ValidationReception(List<CsDetailAffectationScelle> ListeScelle)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                OKButton.IsEnabled = false;
                //List<CsDscelle> lademande = new List<CsDscelle>();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ValidationReceptionCompleted += (s, args) =>
                {
                    try
                    {
                        OKButton.IsEnabled = true;

                        if (args != null && args.Cancelled)
                            return;

                        if (string.IsNullOrEmpty( args.Result) )
                            Message.Show("Validation effectuée avec succès", "Information");
                        else
                            Message.ShowError(args.Result, "Information");
                        this.DialogResult = true;

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service.ValidationReceptionAsync(ListeScelle, UserConnecte.matricule, EtapeActuelle, lademande.First().NUMDEM);
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }


        private void EnvoyerDemandeEtapeSuivante(List<int> Listid)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));

            clientWkf.ExecuterActionSurDemandeParPkIDLigneCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec de sortie materiel", Langue.lbl_Menu);
                    return;
                }
                if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
                {
                    Message.ShowError("Echec de sortie materiel", Langue.lbl_Menu);
                    return;
                }
                else
                {
                    //Message.ShowInformation("Sortie materiel effectuée", Langue.lbl_Menu);

                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (DgLotMag.SelectedItem != null)
                TxtNbScelle.Text = (((CsLotScelle)DgLotMag.SelectedItem).Nbre_Scelles != null) ? ((CsLotScelle)DgLotMag.SelectedItem).Nbre_Scelles.ToString() : string.Empty;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }


        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void MiseAjourDataGrille(CsLotScelle leLot)
        {
            var DataSource = (List<CsLotScelle>)DgLotMag.ItemsSource;
            int index = DataSource.IndexOf((CsLotScelle)DgLotMag.SelectedItem);
            DataSource[index] = leLot;
            DgLotMag.ItemsSource = DataSource;
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
           

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

    }
}


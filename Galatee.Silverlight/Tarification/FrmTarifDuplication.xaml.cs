using Galatee.Silverlight.ServiceTarification;
using Galatee.Silverlight.Shared;
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

namespace Galatee.Silverlight.Tarification
{
    public partial class FrmTarifDuplication : ChildWindow
    {

        public static List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<Galatee.Silverlight.ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        public List<ServiceAccueil.CsProduit> LstDeProduit = new List<ServiceAccueil.CsProduit>();

        private int AncienCentre;



        public FrmTarifDuplication()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
        }

        public FrmTarifDuplication(int AncienCentre)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            // TODO: Complete member initialization
            this.AncienCentre = AncienCentre;

        }

        //private void OKButton_Click(object sender, RoutedEventArgs e)
        //{
        //    this.DialogResult = true;
        //}

        //private void CancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    this.DialogResult = false;
        //}

        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre.Count != 0)
                {
                    LstCentre = SessionObject.LstCentre;
                    cbo_centreCible.ItemsSource = LstCentre;
                    cbo_centreCible.DisplayMemberPath = "LIBELLE";
                    cbo_centreCible.SelectedValuePath = "PK_ID";

                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count > 0)
                        {
                            cbo_Site.ItemsSource = _LstSite;
                            cbo_Site.DisplayMemberPath = "LIBELLE";
                            cbo_Site.SelectedValuePath = "CODE";
                        }
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = SessionObject.LstCentre;
                    cbo_centreCible.ItemsSource = LstCentre;
                    cbo_centreCible.DisplayMemberPath = "CODECENTRE";
                    cbo_centreCible.SelectedValuePath = "PK_ID";



                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count > 0)
                        {
                            cbo_Site.ItemsSource = _LstSite;
                            cbo_Site.DisplayMemberPath = "LIBELLE";
                            cbo_Site.SelectedValuePath = "CODE";
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerDonneeDuSite");

            }
        }

        private void btn_Dupli_Tarif_Centre_Click(object sender, RoutedEventArgs e)
        {
            if (this.AncienCentre >0)
            {
                int idcentre = this.AncienCentre;
                int ? produit = null ;
                if (cbo_produit.SelectedItem!=null)
                {
                    produit=((Galatee.Silverlight.ServiceAccueil.CsProduit)cbo_produit.SelectedItem).PK_ID ;
                }
                if (cbo_centreCible.SelectedItem != null)
                {
                    DuplicationTarifVersCentre(idcentre, ((Galatee.Silverlight.ServiceAccueil.CsCentre)cbo_centreCible.SelectedItem).PK_ID,produit);
                
                }
                else
                {
                    Message.Show("Veuillez selectionné un centre cible", "Info");

                }
            }
        }
        private void cbo_Site_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LstCentre = SessionObject.LstCentre;
            var DataSource = LstCentre.Where(c => c.CODESITE == ((Galatee.Silverlight.ServiceAccueil.CsSite)cbo_Site.SelectedItem).CODE);
            if (DataSource != null)
            {
                cbo_centreCible.ItemsSource = DataSource;
                cbo_centreCible.DisplayMemberPath = "LIBELLE";
                cbo_centreCible.SelectedValuePath = "PK_ID";
            }

        }
        public void DuplicationTarifVersCentre(int AncienIdCentre, int NouveauIdCentre,int? produit)
        {
            try
            {
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                int handler = LoadingManager.BeginLoading("Mise à jour des données ...");
                service.DuplicationTarifVersCentreAsync(AncienIdCentre, NouveauIdCentre, produit);
                service.DuplicationTarifVersCentreCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                if (res.Result)
                                {
                                    Message.Show("Duplication effectuée avec succès",
                                    "Info");
                                }
                                else
                                {
                                    Message.Show("Sauvegarde non effectuée avec succès, il se peut que vos modifications n'aient pas été prises en compte",
                                    "Info");
                                }
                            }
                            else
                                Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                    "Erreur");
                        LoadingManager.EndLoading(handler);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cbo_centreCible_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cbo_centreCible.SelectedItem != null)
            {
                ServiceAccueil.CsCentre leCentreSelect = (ServiceAccueil.CsCentre)this.cbo_centreCible.SelectedItem;
                cbo_produit.ItemsSource = leCentreSelect.LESPRODUITSDUSITE ;
                cbo_produit.DisplayMemberPath = "LIBELLE";
                cbo_produit.SelectedValuePath = "CODE";
            }
        }

        private void cbo_produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}


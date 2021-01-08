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
using Galatee.Silverlight.Resources.Fraude;
using Galatee.Silverlight.ServiceFraude;

namespace Galatee.Silverlight.Fraude
{
    public partial class UcTaxe : ChildWindow
    {
        CsDemandeFraude LaDemande = new CsDemandeFraude();
        int EtapeActuelle;
        public UcTaxe()
        {
            InitializeComponent();
        }
        public UcTaxe(List<int> demande, int etape)
        {
            InitializeComponent();
            EtapeActuelle = etape;
            ChargeDonneDemande(demande.First());
            ChargerTaxe();

        }
        
        private void ChargerTaxe()
        {
            try
            {
                if (SessionObject.LstDesTaxe != null && SessionObject.LstDesTaxe.Count != 0)
                {
                    Cbo_Taxe.ItemsSource = null;
                    Cbo_Taxe.SelectedValuePath = "PK_ID";
                    Cbo_Taxe.DisplayMemberPath = "LIBELLE";
                    Cbo_Taxe.ItemsSource = SessionObject.LstDesTaxe.Where(c=>c.DEBUTAPPLICATION<=DateTime.Now || c.FINAPPLICATION >= DateTime.Now );

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeTaxeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.LstDesTaxe = res.Result;
                    Cbo_Taxe.ItemsSource = null;
                    Cbo_Taxe.SelectedValuePath = "PK_ID";
                    Cbo_Taxe.DisplayMemberPath = "LIBELLE";
                    Cbo_Taxe.ItemsSource = SessionObject.LstDesTaxe.Where(c => c.DEBUTAPPLICATION <= DateTime.Now || c.FINAPPLICATION >= DateTime.Now);
                };
                service1.RetourneListeTaxeAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void ChargeDonneDemande(int pk_id)
        {

            FraudeServiceClient service = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
            service.RetourDemandeFraudeCompleted += (s, args) =>
            {
                try
                {
                    if (args != null && args.Cancelled)
                        return;
                    LaDemande = args.Result;

                    if (LaDemande != null)
                    {
                        txt_Produit.Text = LaDemande.CompteurFraude.libelle_Produit;
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }

            };
            service.RetourDemandeFraudeAsync(pk_id);

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Recuperer(LaDemande);
            UcFacturation Newfrm = new UcFacturation(LaDemande, EtapeActuelle);
            Newfrm.Show();
            DialogResult = true;
        }

        private void Recuperer(CsDemandeFraude LaDemande)
        {
            try
            {
                LaDemande.Ctax = new CsCtax();

                LaDemande.Ctax.TAUX = ((Galatee.Silverlight.ServiceAccueil.CsCtax)Cbo_Taxe.SelectedItem).TAUX;
                LaDemande.Ctax.CODE = ((Galatee.Silverlight.ServiceAccueil.CsCtax)Cbo_Taxe.SelectedItem).CODE;
                LaDemande.ConsommationFrd.TauxTVA = ((Galatee.Silverlight.ServiceAccueil.CsCtax)Cbo_Taxe.SelectedItem).TAUX;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


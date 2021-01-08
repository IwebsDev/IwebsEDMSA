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

namespace Galatee.Silverlight.Facturation
{
    public partial class UcSaisiePlus : ChildWindow
    {
        string centre = string.Empty;
        string tournee = string.Empty;
        string periode = string.Empty;
        string lotri = string.Empty;

        public UcSaisiePlus(string pcentre,string ptournee,string pperiode,string plotri)
        {
            InitializeComponent();

            try
            {
                centre = pcentre;
                tournee = ptournee;
                periode = pperiode;
                lotri = plotri;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        public UcSaisiePlus()
        {
            InitializeComponent();
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Txt_Centre.Text = centre;
                txt_tournee.Text = tournee;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btn_ok_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ObtenirDonneeIndexClient(txt_adresse.Text, centre, tournee,txt_produit.Text);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        //private void ObtenirDonneeIndexClient(string adresse, string centre, string tournee,string produit)
        //{
        //    int result = LoadingManager.BeginLoading("Chargement en cours...");

        //    try
        //    {
        //        btn_ok.IsEnabled = false;

        //        FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
        //        service.RetourneInfoClientHorsLotCompleted += (s, args) =>
        //        {
        //            try
        //            {
        //                if (args != null && args.Cancelled)
        //                {
        //                    Message.ShowError("", "");
        //                    LoadingManager.EndLoading(result);
        //                    btn_ok.IsEnabled = true;
        //                    return;
        //                }

        //                if (args.Result == null )
        //                {
        //                    Message.ShowError("", "");
        //                    LoadingManager.EndLoading(result);
        //                    btn_ok.IsEnabled = true;
        //                    return;
        //                }
        //                LoadingManager.EndLoading(result);
        //                btn_ok.IsEnabled = true;
        //                // ouverture du controle de saisi des index hors lot
        //                UcSaisieIndividuellePlus plus = new UcSaisieIndividuellePlus(args.Result,centre,adresse,tournee,produit,periode,lotri);
        //                plus.Show();
        //                this.Close();

        //            }
        //            catch (Exception ex)
        //            {
        //             Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
        //             LoadingManager.EndLoading(result);
        //             btn_ok.IsEnabled = true;
        //            }
        //        };
        //        service.RetourneInfoClientHorsLotAsync(0,centre,adresse,produit,tournee); // PARAMETRE MANQUANT
        //    }
        //    catch (Exception ex)
        //    {
        //        LoadingManager.EndLoading(result);
        //        throw ex;
        //    }
        //}

    }
}


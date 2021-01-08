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
using Galatee.Silverlight.ServiceAccueil ;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using System.Globalization;

namespace Galatee.Silverlight.Devis
{
    public partial class UcMetrer : ChildWindow
    {
        private int IdDemandeDevis;
        public UcMetrer()
        {
            InitializeComponent();
        }


        public UcMetrer(int _iddemande)
        {
            InitializeComponent();
            this.IdDemandeDevis = _iddemande;
        }

        private void LayoutRoot_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {

        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ChargeDetailDEvis(IdDemandeDevis);
        }

        private void ChargeDetailDEvis(int IdDemandeDevis)
        {
            allowProgressBar();
            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeAsync(IdDemandeDevis,string.Empty );
            client.ChargerDetailDemandeCompleted += (ssender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    string error = args.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                if (args.Result == null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                else
                {

                    var laDetailDemande = args.Result;
                    var laDemandeSelect = laDetailDemande.LaDemande;
                        if (laDemandeSelect.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        {
                            if (laDemandeSelect.TYPEDEMANDE != SessionObject.Enumere.BranchementAbonnementEp)
                            {
                                UcMetrers uc = new UcMetrers(args.Result, this);
                                Grid.SetRow(uc, 0);
                                Grid.SetColumn(uc, 0);
                                LayoutRoot.Children.Add(uc);
                            }
                            else
                            {
                                UcMetrersEp uc = new UcMetrersEp(args.Result, this);
                                Grid.SetRow(uc, 0);
                                Grid.SetColumn(uc, 0);
                                LayoutRoot.Children.Add(uc);
                            }
                        }
                        else
                        {
                            UcMetrersMT uc = new UcMetrersMT(args.Result, this);
                            Grid.SetRow(uc, 0);
                            Grid.SetColumn(uc, 0);
                            LayoutRoot.Children.Add(uc);
                        }
                    desableProgressBar();
                }
                LayoutRoot.Cursor = Cursors.Arrow;
                //InitDonnee();
            };
        }

        void allowProgressBar()
        {
            progressBar1.IsEnabled = true;
            progressBar1.Visibility = Visibility.Visible;
            progressBar1.IsIndeterminate = true;
        }

        void desableProgressBar()
        {
            progressBar1.IsIndeterminate = false;
            progressBar1.Visibility = Visibility.Collapsed;
        }
    }
}


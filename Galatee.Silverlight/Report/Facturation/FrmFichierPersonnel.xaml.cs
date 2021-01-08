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

namespace Galatee.Silverlight.Report
{
    public partial class FrmFichierPersonnel : ChildWindow
    {
        public FrmFichierPersonnel()
        {
            InitializeComponent();
        }
        string leEtatExecuter = string.Empty;
        public FrmFichierPersonnel(string typeEtat)
        {
            InitializeComponent();
            leEtatExecuter = typeEtat;
        }
        List<int> lesCentreCaisse = new List<int>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        Galatee.Silverlight.ServiceAccueil.CsSite lSiteSelect = new Galatee.Silverlight.ServiceAccueil.CsSite();
        List<Galatee.Silverlight.ServiceAccueil.CsProduit> lProduitSelect = new List<Galatee.Silverlight.ServiceAccueil.CsProduit>();

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service.ReturneFichierPersonnelCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                Message.ShowInformation("Traitement ok", "Fichier personnel");
            };
            service.ReturneFichierPersonnelAsync(Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.txt_Periode.Text),SessionObject.CheminImpression );
            service.CloseAsync();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}


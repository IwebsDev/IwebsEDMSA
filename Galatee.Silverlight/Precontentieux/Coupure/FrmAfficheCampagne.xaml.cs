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
using Galatee.Silverlight.ServiceRecouvrement ;
namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmAfficheCampagne : ChildWindow
    {
        public FrmAfficheCampagne()
        {
            InitializeComponent();
        }
        List<CsDetailCampagne> dataTable = new List<CsDetailCampagne>();
        public FrmAfficheCampagne(List<CsDetailCampagne > lstCampagne)
        {
            InitializeComponent();
            dataTable = Shared.ClasseMEthodeGenerique.RetourneListCopy<CsDetailCampagne>(lstCampagne);
            List<CsDetailCampagne> lstSource = new List<CsDetailCampagne>();
            foreach (var item in lstCampagne)
            {
                CsDetailCampagne le = new CsDetailCampagne();
                le = item;
                if (lstSource.FirstOrDefault(t => t.IDCOUPURE == le.IDCOUPURE  ) != null)
                    le.IDCOUPURE  = string.Empty;
                lstSource.Add(le);
            }
            this.dataGrid1.ItemsSource = null;
            this.dataGrid1.ItemsSource = lstSource;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Utility.ActionDirectOrientation<ServicePrintings.CsDetailCampagne, ServiceRecouvrement.CsDetailCampagne>(dataTable, null, SessionObject.CheminImpression, "Campagnes", "Recouvrement", true);
        }
        private void ExportFile_Click_1(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            string key = Utility.getKey();
            Utility.ActionExportation<ServicePrintings.CsDetailCampagne, ServiceRecouvrement.CsDetailCampagne>(dataTable, null, SessionObject.CheminImpression, "Campagnes", "Recouvrement", true, "xlsx");
        }
    }
}


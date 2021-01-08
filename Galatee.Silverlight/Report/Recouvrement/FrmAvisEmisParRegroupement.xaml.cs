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
    public partial class FrmAvisEmisParRegroupement : ChildWindow
    {
        public FrmAvisEmisParRegroupement()
        {
            InitializeComponent();
            RemplirCodeRegroupement();
            prgBar.Visibility = Visibility.Collapsed;
        }
        string leEtatExecuter = string.Empty;

        public FrmAvisEmisParRegroupement(string typeEtat)
        {
            InitializeComponent();
            RemplirCodeRegroupement();
            leEtatExecuter = typeEtat;
            if (leEtatExecuter == SessionObject.EmissionRegroupement)
            {
            }
            else
            {
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            prgBar.Visibility = Visibility.Visible ;

            List<int> lstIdREgroupement =new List<int>();
            string PeriodeDebut = string.IsNullOrEmpty( this.txt_PeriodeDebut.Text)?string.Empty : this.txt_PeriodeDebut.Text; 
            string PeriodeFin = string.IsNullOrEmpty( this.txt_PeriodeFin.Text)?string.Empty : this.txt_PeriodeFin.Text; 
            if (this.Txt_LibelleRegroupement.Tag == null )
            {
             Message.ShowInformation("Selectionner le regroupement","Report");
                return ;
            }
            lstIdREgroupement.Add((int)this.Txt_LibelleRegroupement.Tag);

            EmissionParRegroupement(lstIdREgroupement, PeriodeDebut, PeriodeFin);
            
        }
        private void EmissionParRegroupement( List<int>LstIdRegroupemt, string  PeriodeDebut, string PeriodeFin)
        {
            Galatee.Silverlight.ServiceReport.ReportServiceClient service1 = new Galatee.Silverlight.ServiceReport.ReportServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Report"));
            service1.ReturneEmissionProduitRegroupementCompleted += (sr, res) =>
            {
                prgBar.Visibility = Visibility.Collapsed;
                if (res != null && res.Cancelled)
                    return;

                if (res.Result != null && res.Result.Count != 0)
                        Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceReport.CsLclient>(res.Result, null, SessionObject.CheminImpression, "EmissionProduitRegroupement", "Report", true);
                else
                {
                    Message.ShowInformation("Aucune information trouvée", "Report");
                    return;
                }
            };
            service1.ReturneEmissionProduitRegroupementAsync(LstIdRegroupemt, PeriodeDebut, PeriodeFin);
            service1.CloseAsync();

        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void RemplirCodeRegroupement()
        {
            try
            {
                if (SessionObject.LstCodeRegroupement.Count != 0)
                {
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneCodeRegroupementCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeRegroupement = args.Result;
                        return;
                    };
                    service.RetourneCodeRegroupementAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void btn_Regroupement_Click(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LstCodeRegroupement != null && SessionObject.LstCodeRegroupement.Count != 0)
            {
                List<ServiceAccueil.CParametre> lstParametre = Shared.ClasseMEthodeGenerique.RetourneValueFromClasse<ServiceAccueil.CsRegCli>(SessionObject.LstCodeRegroupement);
                Shared.UcListeParametre ctr = new Galatee.Silverlight.Shared.UcListeParametre(lstParametre, false, "Agent de recouvrement");
                ctr.Closed += new EventHandler(Regroupement_OkClicked);
                ctr.Show();
            }
        }
        private void Regroupement_OkClicked(object sender, EventArgs e)
        {
            try
            {

                Shared.UcListeParametre ctrs = sender as Shared.UcListeParametre;
                if (ctrs.isOkClick)
                {
                    ServiceAccueil.CParametre _LeControleurSelect = ctrs.MyObject as ServiceAccueil.CParametre;
                    this.Txt_LibelleRegroupement .Text = string.IsNullOrEmpty(_LeControleurSelect.LIBELLE) ? string.Empty : _LeControleurSelect.LIBELLE;
                    this.Txt_LibelleRegroupement.Tag = _LeControleurSelect.PK_ID ;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
    }
}


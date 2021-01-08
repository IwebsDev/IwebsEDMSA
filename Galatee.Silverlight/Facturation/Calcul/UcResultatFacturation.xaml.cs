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
using Galatee.Silverlight.Resources.Facturation;

namespace Galatee.Silverlight.Facturation
{
    public partial class UcResultatFacturation : ChildWindow
    {
        public UcResultatFacturation()
        {
            InitializeComponent();
        }
        CsStatFacturation leResultatMiseAJour;
        //CsFacturation leResultatFacturation;
        List<CsFactureBrut> leResultatFacturationbrut;
        bool IsMisajour = false;
        int NombreDeRejet = 0;
        public UcResultatFacturation(CsStatFacturation leResultat)
        {
            try
            {
                InitializeComponent();
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                if (leResultat != null)
                {
                    IsMisajour = true;
                    leResultatMiseAJour = new CsStatFacturation();
                    leResultatMiseAJour = leResultat;
                     
                    txtMontantFacture.Text = leResultat.Montant.Value.ToString(SessionObject.FormatMontant);
                    txtNbreClientFacture.Text = leResultat.NombreCalcule.ToString(SessionObject.FormatMontant);
                    txtNbreClientRejeter.Text = leResultat.NombreRejete.ToString(SessionObject.FormatMontant);
                    txtQteFacture.Text = leResultat.VolumeCalcule.ToString(SessionObject.FormatMontant);
                    this.BtnAnnomalie.Content = "Anomalie(0)";
                    this.CancelButton.Content = "Ok";
                    this.OKButton.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    txtMontantFacture.Text = "0";
                    txtQteFacture.Text = "0";
                    txtNbreClientRejeter.Text = "0";
                    txtNbreClientFacture.Text = "0";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //public UcResultatFacturation(CsFacturation leResultat)
        //{
        //    try
        //    {
        //        InitializeComponent();
        //        if (leResultat != null)
        //        {
        //            leResultatFacturation = new CsFacturation();
        //            leResultatFacturation = leResultat;

        //            int? ConsoFact = 0;
        //            decimal? MontantFacture = 0;
        //            int? NombreClientFact = 0;
        //            int? NombreClientRejeter = 0;
        //            if (leResultat.laFacture != null && leResultat.laFacture.Count != 0)
        //            {
        //                NombreClientFact = leResultat.laFacture.Count();
        //                List<CsEntfac> lesFacture = leResultat.laFacture;
                       
        //                foreach (CsEntfac item in lesFacture)
        //                {
        //                    ConsoFact = ConsoFact + item.LstProfac.Sum(t => t.CONSOFAC);
        //                    MontantFacture =MontantFacture +  item.leEntfac.TOTFTTC;
        //                }
        //            }
        //            if (leResultat.lesAnnomalie != null)
        //            {
        //                this.BtnAnnomalie.Content = "Anomalie(" + leResultat.lesAnnomalie.Count().ToString() + ")";
        //                NombreClientRejeter = leResultat.lesAnnomalie.Count();
        //            }
        //            else
        //            {
        //                this.BtnAnnomalie.Content = "Anomalie(0)";
        //                NombreClientRejeter = 0;
        //            }
        //            txtMontantFacture.Text = MontantFacture.Value.ToString(SessionObject.FormatMontant);
        //            txtNbreClientFacture.Text = NombreClientFact.Value.ToString(SessionObject.FormatMontant);
        //            txtQteFacture.Text = ConsoFact.Value.ToString(SessionObject.FormatMontant);
        //            txtNbreClientRejeter.Text = NombreClientRejeter.Value .ToString(SessionObject.FormatMontant);
                   
        //        }
        //        else
        //        {
        //            txtMontantFacture.Text = "0";
        //            txtQteFacture.Text = "0";
        //            txtNbreClientRejeter.Text = "0";
        //            txtNbreClientFacture.Text = "0";

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        bool IsSimulation = false;
        public UcResultatFacturation(List<CsFactureBrut> leResultat,bool _IsSimulation)
        {
            try
            {
                InitializeComponent();
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                if (leResultat != null)
                {
                    IsSimulation = _IsSimulation;
                    leResultatFacturationbrut = leResultat;
                    if (leResultatFacturationbrut != null && leResultatFacturationbrut.Count != 0)
                    {
                        txtMontantFacture.Text = leResultatFacturationbrut.First().MONTANT.ToString(SessionObject.FormatMontant);
                        txtNbreClientFacture.Text = leResultatFacturationbrut.First().NOMBRECALCULE.ToString(SessionObject.FormatMontant);
                        txtQteFacture.Text = leResultatFacturationbrut.First().CONSOTOTAL.ToString(SessionObject.FormatMontant);
                        txtNbreClientRejeter.Text = leResultatFacturationbrut.First().REJETE.ToString(SessionObject.FormatMontant);
                        NombreDeRejet = leResultatFacturationbrut.First().REJETE;
                        this.BtnAnnomalie.Content = "Anomalie (" + leResultatFacturationbrut.First().REJETE.ToString(SessionObject.FormatMontant) + ")";
                    }
                }
                else
                {
                    txtMontantFacture.Text = "0";
                    txtQteFacture.Text = "0";
                    txtNbreClientRejeter.Text = "0";
                    txtNbreClientFacture.Text = "0";

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public UcResultatFacturation(List<CsFactureBrut> leResultat)
        {
            try
            {
                InitializeComponent();
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                if (leResultat != null)
                {
                    leResultatFacturationbrut = leResultat;
                    if (leResultatFacturationbrut != null && leResultatFacturationbrut.Count != 0)
                    {
                        txtMontantFacture.Text = leResultatFacturationbrut.First().MONTANT .ToString(SessionObject.FormatMontant);
                        txtNbreClientFacture.Text = leResultatFacturationbrut.First().NOMBRECALCULE.ToString(SessionObject.FormatMontant);
                        txtQteFacture.Text = leResultatFacturationbrut.First().CONSOTOTAL.ToString(SessionObject.FormatMontant);
                        txtNbreClientRejeter.Text = leResultatFacturationbrut.First().REJETE .ToString(SessionObject.FormatMontant);
                        NombreDeRejet = leResultatFacturationbrut.First().REJETE;
                        this.BtnAnnomalie.Content = "Anomalie (" + leResultatFacturationbrut.First().REJETE.ToString(SessionObject.FormatMontant) + ")";
                    }
                }
                else
                {
                    txtMontantFacture.Text = "0";
                    txtQteFacture.Text = "0";
                    txtNbreClientRejeter.Text = "0";
                    txtNbreClientFacture.Text = "0";

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.OKButton.IsEnabled = false;

            if (!IsMisajour)
            {
                if (leResultatFacturationbrut != null && leResultatFacturationbrut.Count != 0)
                    ConfirmerFacturation(leResultatFacturationbrut);
            }
            else
                this.DialogResult = false;

        }
        private void ConfirmerFacturation(List<CsFactureBrut> laFacturation)
        {
            this.OKButton.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            prgBar.Visibility = System.Windows.Visibility.Visible ;
            FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
            service.ValiderFacturationCompleted += (s, args) =>
            {
                prgBar.Visibility = System.Windows.Visibility.Collapsed ;
                if (args != null && args.Cancelled)
                {
                    Message.Show(Langue.msgErrorFact, Langue.LibelleModule);
                    return;
                }
                if (args == null && args.Cancelled)
                {
                    Message.Show(Langue.msgErrorFact, Langue.LibelleModule);
                    return;
                }
                if (args.Result == false)
                {
                    Message.Show(Langue.msgErreurMaj, Langue.LibelleModule);
                    this.DialogResult = false;
                    return; 
                }
                else 
                {
                    if (!IsSimulation)
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Index.Langue.MsgCalculTerminer, Galatee.Silverlight.Resources.Index.Langue.libelleModule);
                        this.DialogResult = false;
                        return;
                    }
                    else
                        this.DialogResult = false;
                }
            };
            service.ValiderFacturationAsync(laFacturation,false );
            service.CloseAsync();
        
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (leResultatFacturationbrut != null && NombreDeRejet != 0)
            {
                SupprimerAnnomalie(leResultatFacturationbrut);
            }
        }

        private void BtnAnnomalie_Click(object sender, RoutedEventArgs e)
        {
            if (leResultatFacturationbrut != null && NombreDeRejet != 0)
            {
                RetourneAnnomalie(leResultatFacturationbrut);
                //if (leResultatFacturation.lesAnnomalie != null)
                //{
                //    Dictionary<string, string> param = new Dictionary<string, string>();
                //    Utility.ActionDirectOrientation<ServicePrintings.CsAnnomalie, ServiceFacturation.CsAnnomalie>(leResultatFacturation.lesAnnomalie, new Dictionary<string, string>(), SessionObject.CheminImpression, "AnomalieFacturation", "Facturation", true);
                //}
            }
        }

        private void RetourneAnnomalie(List<CsFactureBrut> leResultatFacturationbrut)
        {
            FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
            service.RetourneLesteAnomalieCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                {
                    Message.Show(Langue.msgErrorFact, Langue.LibelleModule);
                    return;
                }
                if (args == null && args.Cancelled)
                {
                    Message.Show(Langue.msgErrorFact, Langue.LibelleModule);
                    return;
                }
                if (args.Result.Count  != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    Utility.ActionDirectOrientation<ServicePrintings.CsAnnomalie, ServiceFacturation.CsAnnomalie>(args.Result, new Dictionary<string, string>(), SessionObject.CheminImpression, "AnomalieFacturation", "Facturation", true);
                }
            };
            service.RetourneLesteAnomalieAsync(leResultatFacturationbrut.First().LOTRI, leResultatFacturationbrut.First().FK_IDCENTRE );
            service.CloseAsync();

        }


        private void SupprimerAnnomalie(List<CsFactureBrut> leResultatFacturationbrut)
        {
            FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
            service.SupprimeAnnomalieCompleted += (s, args) =>
            {
            };
            service.SupprimeAnnomalieAsync(leResultatFacturationbrut.First().LOTRI, leResultatFacturationbrut.First().FK_IDCENTRE);
            service.CloseAsync();

        }
    }
}


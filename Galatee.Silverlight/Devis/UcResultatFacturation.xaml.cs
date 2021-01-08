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
using Galatee.Silverlight.Devis ;
using Galatee.Silverlight.Resources.Facturation;
using Galatee.Silverlight.ServiceAccueil ;

namespace Galatee.Silverlight.Devis
{
    public partial class UcResultatFacturation : ChildWindow
    {
        public UcResultatFacturation()
        {
            InitializeComponent();
        }
        List<CsFactureBrut> leResultatFacturationbrut;
        CsDemande laDetailDemande = new CsDemande();
        bool IsMisajour = false;
        int NombreDeRejet = 0;
        public UcResultatFacturation(List<CsFactureBrut> leResultat,CsDemande laDemande)
        {
            try
            {
                InitializeComponent();
                laDetailDemande = laDemande;
                if (leResultat != null)
                {
                    leResultatFacturationbrut = leResultat;
                    if (leResultatFacturationbrut != null && leResultatFacturationbrut.Count != 0)
                    {


                        txtMontantFacture.Text = leResultatFacturationbrut.First().TOTFTTC .ToString(SessionObject.FormatMontant);
                        txtNbreClientFacture.Text = leResultatFacturationbrut.First().NOMBRECALCULE.ToString(SessionObject.FormatMontant);
                        txtQteFacture.Text = leResultatFacturationbrut.First().CONSOFAC .ToString(SessionObject.FormatMontant);
                        txtNbreClientRejeter.Text = leResultatFacturationbrut.First().REJETE.ToString(SessionObject.FormatMontant);
                        //NombreDeRejet = leResultatFacturationbrut.First().REJETE;
                        this.BtnAnnomalie.Content = "Annomalie (" + leResultatFacturationbrut.First().REJETE.ToString(SessionObject.FormatMontant) + ")";
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
                else
                {
                    List<string> codes = new List<string>();
                    codes.Add(laDetailDemande.InfoDemande.CODE);
                    Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
                    this.DialogResult = false;
                    return;
                }
            }
            else
                this.DialogResult = false;

        }
        private void ConfirmerFacturation(List<CsFactureBrut> laFacturation)
        {
            this.OKButton.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Accueil"));
            service.ValiderFacturationCompleted += (s, args) =>
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
                if (args.Result == false)
                {
                    Message.Show(Langue.msgErreurMaj, Langue.LibelleModule);
                    this.DialogResult = false;
                    return;
                }
                else
                {
                    List<string> codes = new List<string>();
                    codes.Add(laDetailDemande.InfoDemande.CODE);
                    Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
                    this.DialogResult = false;
                    return;
                }
            };
            service.ValiderFacturationAsync(laFacturation, true );
            service.CloseAsync();

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            AnnulerFacturation(leResultatFacturationbrut);
            this.DialogResult = false;
        }

        private void BtnAnnomalie_Click(object sender, RoutedEventArgs e)
        {
            if (leResultatFacturationbrut != null && NombreDeRejet != 0)
            {
                RetourneAnnomalie(leResultatFacturationbrut);
            }
        }

        private void RetourneAnnomalie(List<CsFactureBrut> leResultatFacturationbrut)
        {
            Galatee.Silverlight.ServiceFacturation.FacturationServiceClient service = new Galatee.Silverlight.ServiceFacturation.FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
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
                if (args.Result.Count != 0)
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    Utility.ActionDirectOrientation<ServicePrintings.CsAnnomalie, ServiceFacturation.CsAnnomalie>(args.Result, new Dictionary<string, string>(), SessionObject.CheminImpression, "AnomalieFacturation", "Facturation", true);
                }
            };
            service.RetourneLesteAnomalieAsync(leResultatFacturationbrut.First().LOTRI, leResultatFacturationbrut.First().FK_IDCENTRE);
            service.CloseAsync();

        }

        private void AnnulerFacturation(List<CsFactureBrut> leResultatFacturationbrut)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.AnnulerFactureResiliationCompleted += (s, args) =>
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
            };
            service.AnnulerFactureResiliationAsync(leResultatFacturationbrut);
            service.CloseAsync();

        }
        private void EditerFacture(List<CsFactureBrut> leResultatFacturationbrut)
        {

            string rdlc = string.Empty;
            Dictionary<string, string> param = new Dictionary<string, string>();

            if (laDetailDemande.LstEvenement.First().PRODUIT ==SessionObject.Enumere.ElectriciteMT)
                rdlc = "FactureSimpleMT";
            else if (laDetailDemande.LstEvenement.First().PRODUIT == SessionObject.Enumere.Eau)
                rdlc = "FactureSimpleO";
            else
                rdlc = "FactureSimple";

            param.Add("TypeEdition", "Originale");


            string print = "Imprimé le " + DateTime.Now + " sur le poste " + SessionObject.LePosteCourant.NOMPOSTE + " par " + UserConnecte.nomUtilisateur + "(" + UserConnecte.matricule + ") du centre " + UserConnecte.LibelleCentre;
            param.Add("Print", print);

            AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            clientDevis.EditionFactureResiliationCompleted += (ss, b) =>
            {
                if (b.Cancelled || b.Error != null)
                {
                    string error = b.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                List<CsFactureClient > _laStat = b.Result;
                Utility.ActionDirectOrientation<ServicePrintings.CsFactureClient,CsFactureClient>(_laStat, param, SessionObject.CheminImpression, rdlc, "Facturation", true);
            };
            clientDevis.EditionFactureResiliationAsync(leResultatFacturationbrut);
        }

        private void Btn_EditerFacture_Click(object sender, RoutedEventArgs e)
        {
            EditerFacture(leResultatFacturationbrut);
        }
    }
}


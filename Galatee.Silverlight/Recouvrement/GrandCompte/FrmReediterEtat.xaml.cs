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
using Galatee.Silverlight.ServiceRecouvrement;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmReediterEtat : ChildWindow
    {
        string leEtatExecuter = string.Empty;
        public FrmReediterEtat(string typeEtat)
        {
            InitializeComponent();
            leEtatExecuter = typeEtat;
        }
        public FrmReediterEtat()
        {
            InitializeComponent();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.dg_campagne.ItemsSource != null)
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                List<CsLclient> lstFacture = (List<CsLclient>)this.dg_campagne.ItemsSource;

                param.Add("pUserMiseAJour", lstFacture.First().USERCREATION);
                if (leEtatExecuter == SessionObject.ReeditionCampagne)
                {
                    param.Add("pTitreEtat", "LISTE DES CLIENT DE LA CAMPAGNE");
                    param.Add("pParametre", "N° Campagne :" + this.txt_NumCamp.Text );
                }
                if (leEtatExecuter == SessionObject.ReeditionMandatement)
                {
                    param.Add("pTitreEtat", "LISTE DES CLIENT DU MANDATEMENT");
                    param.Add("pParametre", "N° Campagne :" + this.txt_NumCamp.Text);
                }
                if (leEtatExecuter == SessionObject.ReeditionPaiement)
                {
                    param.Add("pTitreEtat", "LISTE DES CLIENT DU PAIEMENT");
                    param.Add("pParametre", "N° Avis de crédit :" + this.txt_NumCamp.Text);
                }
                if (leEtatExecuter == SessionObject.ReeditionMiseAJour)
                    Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(lstFacture, param, SessionObject.CheminImpression, "MiseAJourGrandCompte", "Recouvrement", true);
                else 
                   Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(lstFacture, param, SessionObject.CheminImpression, "EtatcampagneGC", "Recouvrement", true);

            }
        }
  

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btn_Rech_Click(object sender, RoutedEventArgs e)
        {
            if (leEtatExecuter == SessionObject.ReeditionCampagne )
                RechercheCampane(this.txt_NumCamp.Text );
            if (leEtatExecuter == SessionObject.ReeditionMandatement)
                RechercheMandatement(this.txt_NumCamp.Text );
            if (leEtatExecuter == SessionObject.ReeditionPaiement)
                RecherchePaiement(this.txt_NumCamp.Text );
            if (leEtatExecuter == SessionObject.ReeditionMiseAJour)
                RechercheMiseAJour(this.txt_NumCamp.Text);
        }

        private void RechercheCampane(string NumCamp)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RechercheDetailCampaneCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                {
                    Message.ShowInformation("L'opération ne c'est pas correctement effecaion ,veuillez la refaire ", "Recouvrement");
                }
                if (args.Result.Count == 0)
                {
                    Message.ShowInformation("Aucune données trouvées ", "Recouvrement");
                }
                this.dg_campagne.ItemsSource = args.Result;
                this.Txt_Montant.Text  = args.Result.Sum(o => o.MONTANT).Value.ToString(SessionObject.FormatMontant);
                return;
            };
            service.RechercheDetailCampaneAsync(NumCamp);
        }
        private void RechercheMandatement(string NumMandat)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RechercheMandatemantCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                {
                    Message.ShowInformation("L'opération ne c'est pas correctement effecaion ,veuillez la refaire ", "Recouvrement");
                }
                if (args.Result.Count == 0)
                {
                    Message.ShowInformation("Aucune données trouvées ", "Recouvrement");
                }
                this.dg_campagne.ItemsSource = args.Result;
                this.Txt_Montant.Text  = args.Result.Sum(o => o.MONTANT).Value.ToString(SessionObject.FormatMontant);

                return;
            };
            service.RechercheMandatemantAsync(NumMandat);
        }
        private void RecherchePaiement(string AvisDeCredit)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RecherchePaiementCompleted  += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                {
                    Message.ShowInformation("L'opération ne c'est pas correctement effecaion ,veuillez la refaire ", "Recouvrement");
                }
                if (args.Result.Count == 0)
                {
                    Message.ShowInformation("Aucune données trouvées ", "Recouvrement");
                }
                this.dg_campagne.ItemsSource = args.Result;
                this.Txt_Montant.Text  = args.Result.Sum(o => o.MONTANT).Value.ToString(SessionObject.FormatMontant);
                return;
            };
            service.RecherchePaiementAsync (AvisDeCredit);
        }
        private void RechercheMiseAJour(string AvisDeCredit)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RechercheMiseAJourCompleted  += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                {
                    Message.ShowInformation("L'opération ne c'est pas correctement effecaion ,veuillez la refaire ", "Recouvrement");
                }
                if (args.Result.Count==0)
                {
                    Message.ShowInformation("Aucune données trouvées ", "Recouvrement");
                }
                this.dg_campagne.ItemsSource = args.Result;
                this.Txt_Montant.Text  = args.Result.Sum(o => o.MONTANT).Value.ToString(SessionObject.FormatMontant);

                return;
            };
            service.RechercheMiseAJourAsync (AvisDeCredit);
        }
    }
}


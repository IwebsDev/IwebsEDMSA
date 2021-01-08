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
using Galatee.Silverlight.Resources.Accueil  ;
using System.Collections.ObjectModel;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Classes;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmRechercherDemandeAnc : ChildWindow
    {
        public FrmRechercherDemandeAnc()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //RechercheDemande(this.txt_Centre.Text, this.txt_NumDemande.Text, RecupereCriterDemande(), DateTime.Parse(this.txt_dateDebut.Text), DateTime.Parse(this.txt_datefin.Text), DateTime.Parse(this.txt_dateDemande.Text), this.txt_demandeDebut.Text, this.txt_demandeFin.Text, RecupereStatusDemande());
            //RechercheDemande(this.txt_Centre.Text, this.txt_NumDemande.Text, RecupereCriterDemande(), null , null , null , this.txt_demandeDebut.Text, this.txt_demandeFin.Text, RecupereStatusDemande());
            RechercheDemande(this.txt_Centre.Text, this.txt_NumDemande.Text, RecupereCriterDemande(), this.txt_dateDebut.Text, this.txt_datefin.Text, this.txt_dateDemande.Text, this.txt_demandeDebut.Text, this.txt_demandeFin.Text, RecupereStatusDemande());
            this.DialogResult = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void RechercheDemande(string centre, string numdem, List<string> LstTdem, string  datedebut,  string dateFin,
                                              string datedemande, string numerodebut, string numerofin, string status)
        {
            try
            {
                List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
                DateTime? pDateDebut = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.IsDateValider(datedebut);
                DateTime? pDateFin = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.IsDateValider(dateFin);
                DateTime? pDatedemande = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.IsDateValider(datedemande);

                if ((pDateDebut == null && !string.IsNullOrEmpty(datedebut)) ||
                    (pDateFin == null && !string.IsNullOrEmpty(dateFin)) ||
                    (pDatedemande == null && !string.IsNullOrEmpty(datedemande)))
                    Message.ShowError(Langue.MsgDateInvalide, Langue.lbl_Menu);

                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service1.RetourneListeDemandeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    LstDemande = res.Result;
                    if (LstDemande != null && LstDemande.Count != 0)
                    {
                        this.DialogResult = true;
                        UcListInitialisation ctrl = new UcListInitialisation(LstDemande);
                        ctrl.Show();
                    }
                    //else
                    //    Message.ShowError(Langue.MsgDemandeNonTrouve, Langue.lbl_Menu);

                };
                service1.RetourneListeDemandeAsync(centre, numdem, LstTdem, pDateDebut, pDateFin, pDatedemande, numerodebut, numerofin, status);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void DialogueClosed(object sender, EventArgs e)
        {
            DialogResult ctrs = sender as DialogResult;
            if (ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
            {
                ctrs.DialogResult = false;
                return;
            }
        }
        void Translate()
        {
            this.chk_Branchement.Content = Langue.chk_Branchement;
            this.chk_branchementAbonnement.Content = Langue.chk_branchementAbonnement;
            this.Chk_Depannage.Content = Langue.Chk_Depannage;
            this.Chk_AbonnementSeul.Content = Langue.Chk_AbonnementSeul;
            this.chk_resiliation.Content = Langue.chk_resiliation;
            this.Chk_Reabonnement.Content = Langue.Chk_Reabonnement;
            this.chk_frtbrt.Content = Langue.chk_frtbrt;
            this.chk_Ouverturebrt.Content = Langue.chk_Ouverturebrt;
            this.Chk_chtcpt.Content = Langue.Chk_chtcpt;
            this.chk_PoseCpt.Content = Langue.chk_PoseCpt;
            this.chk_FactureIsole.Content = Langue.chk_FactureIsole;
            this.chk_depose.Content = Langue.chk_depose;
            this.chk_AvoirConsommation.Content = Langue.chk_depose;
            this.chk_Terminer.Content = Langue.chk_Terminer;
            this.chk_EnCaisse.Content = Langue.chk_EnCaisse;
            this.lbl_Demande.Content = Langue.lbl_Demande;
            this.lbl_DemandeEntree.Content = Langue.lbl_DemandeEntree;
            this.lbl_EtDemandeEntree.Content = Langue.lbl_EtDemandeEntree;
            this.lbl_Centre.Content = Langue.lbl_center;
            this.lbl_DateDemande.Content = Langue.lbl_DateDemande;
            this.lbl_DateDemandeEntree.Content = Langue.lbl_DateDemandeEntree;
            this.lbl_DateRDV.Content = Langue.lbl_DateRDV;
            this.lbl_DateExecution.Content = Langue.lbl_DateExecution;
            this.lbl_DateDevis.Content = Langue.lbl_DateDevis;
            this.lbl_EtDateDemandeEntree.Content = Langue.lbl_EtDemandeEntree;
            this.lbl_EtDemandeEntree.Content = Langue.lbl_EtDemandeEntree;
            this.lbl_EtDateRDV.Content = Langue.lbl_EtDemandeEntree;
            this.lbl_EtDateExecution.Content = Langue.lbl_EtDemandeEntree;
            this.lbl_EtDateDevis.Content = Langue.lbl_EtDemandeEntree;
        
        }

        private List<string > RecupereCriterDemande()
        {
            List<string> critere = new List<string>();
          if (this.chk_Branchement.IsChecked == true)
              critere.Add(SessionObject.Enumere.BranchementSimple);
          if (this.chk_branchementAbonnement .IsChecked == true)
              critere.Add(SessionObject.Enumere.BranchementAbonement) ;
          if (this.Chk_AbonnementSeul .IsChecked == true)
              critere.Add(SessionObject.Enumere.AbonnementSeul) ;
          if (this.Chk_chtcpt .IsChecked == true)
              critere.Add(SessionObject.Enumere.ChangementCompteur) ;

          //if (this.Chk_Depannage .IsChecked == true)
          //    critere = critere + SessionObject.Enumere.d;
          if (this.chk_frtbrt .IsChecked == true)
              critere.Add(SessionObject.Enumere.FermetureBrt) ;
          if (this.chk_Ouverturebrt.IsChecked == true)
              critere.Add(SessionObject.Enumere.ReouvertureBrt);
          if (this.Chk_Reabonnement .IsChecked == true)
              critere.Add(SessionObject.Enumere.Reabonnement) ;
          if (this.chk_resiliation .IsChecked == true)
              critere.Add( SessionObject.Enumere.Resiliation) ;

          return critere;

        }
        private string RecupereStatusDemande()
        {
            string critere = string.Empty;
            if (this.chk_Terminer.IsChecked == true)
                critere = SessionObject.Enumere.DemandeStatusPriseEnCompte;
            if (this.chk_EnAttente .IsChecked == true)
                critere = SessionObject.Enumere.DemandeStatusEnAttente ;
            if (this.chk_EnCaisse .IsChecked == true)
                critere = SessionObject.Enumere.DemandeStatusPasseeEncaisse  ;
            return critere;
        }
        private void btn_rechercher_Click(object sender, RoutedEventArgs e)
        {
            //RechercheDemande(this.txt_Centre.Text, this.txt_NumDemande.Text, RecupereCriterDemande(), null, null, null, this.txt_demandeDebut.Text, this.txt_demandeFin.Text, RecupereStatusDemande());
            RechercheDemande(this.txt_Centre.Text, this.txt_NumDemande.Text, RecupereCriterDemande(),this.txt_dateDebut.Text,this.txt_datefin.Text,this.txt_dateDemande .Text , this.txt_demandeDebut.Text, this.txt_demandeFin.Text ,RecupereStatusDemande());
            //RechercheDemande(this.txt_Centre.Text, this.txt_NumDemande.Text, RecupereCriterDemande(),DateTime.Parse(this.txt_dateDebut.Text), DateTime.Parse(this.txt_datefin.Text),DateTime.Parse(this.txt_dateDemande .Text) , this.txt_demandeDebut.Text, this.txt_demandeFin.Text ,RecupereStatusDemande());
            this.DialogResult = false;
        }



    }
}


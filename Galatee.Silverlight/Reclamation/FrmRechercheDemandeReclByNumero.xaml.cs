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
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.Resources.Accueil;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmRechercheDemandeReclByNumero : ChildWindow
    {
        public FrmRechercheDemandeReclByNumero()
        {
            InitializeComponent();
            this.Txt_NumDemande.MaxLength = SessionObject.Enumere.TailleNumeroDemande;
        }
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            RechercheDemande(this.Txt_NumDemande.Text); 
            this.DialogResult = true;
        }
        private void RechercheDemande( string numdem)
        {
            try
            {
                List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service1.RetourneListeDemandeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    LstDemande = res.Result;
                    if (LstDemande != null && LstDemande.Count != 0)
                    {
                        this.DialogResult = true;
                        UcControleValidation ctrl = new UcControleValidation(LstDemande.First().PK_ID, true);
                        ctrl.Show();
                    }
                };
                service1.RetourneListeDemandeAsync(null , numdem, new List<string>(), null, null, null, null, string.Empty, string.Empty);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void RechercheDemande(string centre, string numdem, string typedemande, string datedebut, string dateFin,
        //                                   string datedemande, string numerodebut, string numerofin, string status)
        //{
        //    List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
        //    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
        //    service1.RetourneListeDemandeCompleted += (sr, res) =>
        //    {
        //        if (res != null && res.Cancelled)
        //            return;
        //        LstDemande.AddRange(res.Result);
        //        if (LstDemande.Count != 0)
        //        {
        //            this.DialogResult = true;
        //            UcDemandeAfficheRecherche ctrl = new UcDemandeAfficheRecherche(LstDemande);
        //            ctrl.Show();
        //        }
        //        else
        //        {
        //            DialogResult resultat = new DialogResult(Langue.lbl_demandeTerminer, false);//, "Warming", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //            resultat.Closed += new EventHandler(DialogueClosed);
        //            resultat.Show();
        //        }

        //    };
        //    service1.RetourneListeDemandeAsync(centre, numdem, typedemande, datedebut, dateFin, datedemande, numerodebut, numerofin, status);
        //    service1.CloseAsync();
        //}
        void DialogueClosed(object sender, EventArgs e)
        {
            DialogResult ctrs = sender as DialogResult;
            if (ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
            {
                ctrs.DialogResult = false;
                return;
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


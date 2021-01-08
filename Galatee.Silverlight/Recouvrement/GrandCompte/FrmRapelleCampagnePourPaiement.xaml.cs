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
    public partial class FrmRapelleCampagnePourPaiement : ChildWindow
    {
        public FrmRapelleCampagnePourPaiement()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            if (dg_campagne.SelectedItem!=null)
            {
                CsCampagneGc Camp =(CsCampagneGc) dg_campagne.SelectedItem;
                List<int> Listid = new List<int>();
                Listid.Add(Camp.PK_ID);
                EnvoyerDemandeEtapeSuivante(Listid);
                FrmRapelSaisePaiementCampagneGC frm = new FrmRapelSaisePaiementCampagneGC(Listid, 176);
                frm.Show();
            }
            else
            {
                Message.ShowWarning("Veuillez selectio la campagne à rapeller", "Alert");

            }


            this.DialogResult = true;
        }
        private void EnvoyerDemandeEtapeSuivante(List<int> Listid)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));

            clientWkf.ExecuterActionSurDemandeParPkIDLigneCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec de sortie materiel", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    return;
                }
                if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
                {
                    Message.ShowError("Echec de sortie materiel", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    return;
                }
                else
                {
                    //Message.ShowInformation("Sortie materiel effectuée", Langue.lbl_Menu);

                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, 176, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btn_Rech_Click(object sender, RoutedEventArgs e)
        {
            RechercheCampane(txt_NumCamp.Text);
        }

        private void RechercheCampane(string NumCamp)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RechercheCampaneCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                {
                    Message.ShowInformation("L'opération ne c'est pas correctement effecaion ,veuillez la refaire ", "Recouvrement");
                }
                dg_campagne.ItemsSource = args.Result;

                return;
            };
            service.RechercheCampaneAsync(NumCamp);
        }

    }
}


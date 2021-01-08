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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmAnnulationCampagne : ChildWindow
    {
        public FrmAnnulationCampagne()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_numeroCampagne.Text))
            {
                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.AnnulerCampagneCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == false)
                    {
                        Message.ShowInformation("L'enregistrement ne s'est pas correctement , veuillez vous assurer que la campagne existe", "Recouvrement");
                    }
                    else
                    {
                        Message.ShowInformation("L'enregistrement effectué avec succès", "Recouvrement");
                    }
                    //Shared.ClasseMEthodeGenerique.InitWOrkflow(args.Result.Split('.')[0], UserConnecte.FK_IDCENTRE, "Galatee.Silverlight.Recouvrement.FrmInitailisationCampagne", args.Result.Split('.')[1]);


                    return;
                };
                service.AnnulerCampagneAsync(txt_numeroCampagne.Text);
            }
            else
            {
                Message.ShowWarning("Veuillez saisir le numéro de la campagne à annuler", "Recouvrement");

            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


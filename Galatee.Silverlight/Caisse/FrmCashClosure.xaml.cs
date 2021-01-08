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
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.ServiceCaisse;
using Galatee.Silverlight.Resources.Caisse ;
using System.Windows.Browser;


namespace Galatee.Silverlight.Caisse
{
    public partial class FrmCashClosure : ChildWindow
    {
        public FrmCashClosure()
        {
            InitializeComponent();
            RetourneCaisseCourante();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              RetourneReglementEncaisse();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string warning = Langue.MsgFermetureCaisse1;
                Txt_Messsage.Text = warning;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
                
            }
        }
        void RetourneCaisseCourante()
        {
            try
            {
                CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                service.RetouneLaCaisseCouranteAsync(UserConnecte.matricule);
                service.RetouneLaCaisseCouranteCompleted += (s, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        Message.ShowError("Erreur de chargement des reçus de la caisse. Réessayez svp !", "Erreur");
                        this.DialogResult = true;

                    }

                    if (args.Result != null)
                        SessionObject.LaCaisseCourante = args.Result;
                };
                service.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        void RetourneReglementEncaisse()
        {
            try
            {
                this.OKButton.IsEnabled = false;
                CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                service.RetourneRecuDeCaissePourAnnulationAsync(SessionObject.LaCaisseCourante.PK_ID);
                service.RetourneRecuDeCaissePourAnnulationCompleted += (s, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        Message.ShowError("Erreur de chargement des reçus de la caisse. Réessayez svp !", "Erreur");
                        this.DialogResult = true;

                    }

                    if (args.Result == null || args.Result.Count == 0)
                    {
                        ValidationFermetureCaisse(SessionObject.LaCaisseCourante);
                        this.DialogResult = true;
                        return;
                    }

                    List<CsLclient> _LstReglement = new List<CsLclient>();
                    _LstReglement = args.Result;
                    if (_LstReglement != null && _LstReglement.Count != 0)
                    {
                        List<CsLclient> lstReglementAnnulationEncour = _LstReglement.Where(t => (!string.IsNullOrEmpty(t.STATUS) && t.STATUS == SessionObject.Enumere.StatusDemandeInitier)).ToList();
                        if (lstReglementAnnulationEncour != null && lstReglementAnnulationEncour.Count != 0)
                        {
                            Message.ShowError(Langue.MsgImposibleArretCaisse, Langue.errorTitle);
                            this.DialogResult = true;
                            return;
                        }
                        else
                        {
                            ValidationFermetureCaisse(SessionObject.LaCaisseCourante);
                            this.DialogResult = true;
                        }

                    }
                };
                service.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ValidationFermetureCaisse(CsHabilitationCaisse laCaisse)
        {
            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("Caisse"));
            service.FermetureCaisseCompleted += (s, args) =>
            {
                if (args.Cancelled || args.Error != null || args.Result == null)
                    Message.ShowError("Un problème est survenu lors de la fermeture de caisse", "Information");
                else
                {
                    // log out current user 
                    if (args.Result.Value ==true)
                    {
                        //Message.ShowInformation("La caisse est fermée, vous allez devoir vous reconnecter","Information");// "Information", MessageBoxButton.OK) == MessageBoxResult.OK)
                        HtmlPage.Document.Submit();
                    }
                    else
                        Message.ShowError("Impossible de fermer la caisse. Veuillez réessayer", "Information");
                }
                
            };
            service.FermetureCaisseAsync(laCaisse);
        }
    }
}


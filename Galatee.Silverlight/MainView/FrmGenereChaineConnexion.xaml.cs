using Galatee.Silverlight;
using Galatee.Silverlight.Security;
using Galatee.Silverlight.ServiceAuthenInitialize;
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

namespace Galatee.Silverlight.MainView
{
    public partial class FrmGenereChaineConnexion : ChildWindow
    {
        List<string> lstCatalogue = new List<string>();
        public FrmGenereChaineConnexion(List<string> lstCatalogue)
        {
            InitializeComponent();
            this.cbo_BaseDeDonne.ItemsSource = lstCatalogue;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //if (EtatConnction == false)
            //{
            //    Message.ShowInformation("Veuillez tester la connexion", "Message");
            //    return;
            //}
            GenereChaineDeConnexion();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void GenereDeConnexionGaladb(string baseDonnee, string Machine, string userId, string Password)
        {
            AuthentInitializeServiceClient client = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
            client.EcrireFichierDeGaladbConnexionCompleted += (ss, res) =>
            {

                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                    return;
                }
                EtatConnction = res.Result;
                if (res.Result != false)
                {
                    Message.ShowInformation("Connection reussi", Galatee.Silverlight.Resources.Langue.informationTitle);
                    OKButton.IsEnabled = true;
                    return;
                }
                else
                {
                    Message.ShowInformation("Connection  echouer,veuillez contacter le partenaire,puis verifiez les paramettres de connection", Galatee.Silverlight.Resources.Langue.informationTitle);
                    return;
                }

            };
            client.EcrireFichierDeGaladbConnexionAsync(Machine, baseDonnee, userId, Password);
        }

        private void GenereDeConnexionAbo07(string baseDonnee, string Machine, string userId, string Password)
        {
            AuthentInitializeServiceClient client = new AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
            client.EcrireFichierDeAbo07ConnexionCompleted += (ss, res) =>
            {

                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                    return;
                }
                EtatConnction = res.Result;
                if (res.Result != false)
                {
                    Message.ShowInformation("Connection reussi", Galatee.Silverlight.Resources.Langue.informationTitle);
                    OKButton.IsEnabled = true;
                    return;
                }
                else
                {
                    Message.ShowInformation("Connection  echouer,veuillez contacter le partenaire,puis verifiez les paramettres de connection", Galatee.Silverlight.Resources.Langue.informationTitle);
                    return;
                }
            };
            client.EcrireFichierDeAbo07ConnexionAsync(Machine, baseDonnee, userId, Password);
        }


        private void GenereChaineDeConnexion()
        {
            string baseDeDonnee = this.cbo_BaseDeDonne.SelectedValue.ToString();
            string Machine = this.TxtMachine.Text;
            string userid = this.TxtUser.Text;
            string Password = this.TxtPassword .Text;

            if (baseDeDonnee.ToUpper() == SessionObject.ConnexionGaladb.ToUpper())
                GenereDeConnexionGaladb(baseDeDonnee, Machine, userid, Password);
            else if (baseDeDonnee.ToUpper() == SessionObject.ConnexionAbo07.ToUpper())
                GenereDeConnexionAbo07(baseDeDonnee, Machine, userid, Password);
        }
        bool EtatConnction = false ;
        private void VerificationDeConnexion( string baseDonnee,string Machine,string userId,string Password)
        {
            string providerName = "SqlClient Data Provider";
            Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient client = new Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            client.TestBaseDistanteCompleted += (ss, res) =>
            {

                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                    return;
                }
                EtatConnction = res.Result;
                if (res.Result != false)
                {
                    Message.ShowInformation("Connection reussi", Galatee.Silverlight.Resources.Langue.informationTitle);
                    OKButton.IsEnabled = true;
                    return;
                }
                else
                {
                    Message.ShowInformation("Connection  echouer,veuillez contacter le partenaire,puis verifiez les paramettres de connection", Galatee.Silverlight.Resources.Langue.informationTitle);
                    return;
                }

            };
            client.TestBaseDistanteAsync("",providerName, Machine, baseDonnee, userId, Password);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string baseDeDonnee = this.cbo_BaseDeDonne.SelectedValue.ToString();
            string Machine = this.TxtMachine.Text;
            string userid = this.TxtUser.Text;
            string Password = this.TxtPassword .Text;
            VerificationDeConnexion(baseDeDonnee, Machine, userid, Password);
        }
    }
}


using Galatee.Silverlight.Security;
using Licence.ServiceAuthenInitialize;
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

namespace Licence
{
    public partial class FrmGenereChaineConnexion : ChildWindow
    {
        public FrmGenereChaineConnexion()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            GenererChaineConnexion(GetChaineDeConnexion());
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private string  GetChaineDeConnexion()
        {

            string baseDeDonnee = this.cbo_BaseDeDonne.SelectedValue.ToString();
            string Machine = this.TxtMachine.Text;
            string userid = this.TxtUser.Text;
            string Password = this.TxtUser.Text;
          

            return string.Empty ;
        }



        public void GenererChaineConnexion(string laChaine)
        {
            string _Licence;
            _Licence = Cryptage.Encrypt(laChaine);
            AuthentInitializeServiceClient client = new AuthentInitializeServiceClient();
            client.GenereLicenceCompleted += (ss, res) =>
            {
                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    return;
                }

                if (res.Result == null)
                {
                    return;
                }
            };
            client.GenereLicenceAsync(_Licence);
        }
    }
}


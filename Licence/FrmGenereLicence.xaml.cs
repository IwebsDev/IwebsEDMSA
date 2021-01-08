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
using Licence.ServiceAuthenInitialize;
using Galatee.Silverlight.Security;
using System.ServiceModel;

namespace Licence
{
    public partial class FrmGenereLicence : ChildWindow
    {
        public FrmGenereLicence()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            string dateFin = this.DateFin .Text;
            GenererLicence(dateFin, this.txtNbreJours.Text);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public BasicHttpBinding Protocole()
        {
            try
            {
                //BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                //binding.MaxReceivedMessageSize = Int32.MaxValue;
                //binding.MaxBufferSize = Int32.MaxValue;

                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                binding.OpenTimeout = new TimeSpan(0, 1, 0);
                binding.CloseTimeout = new TimeSpan(1, 0, 0);
                binding.SendTimeout = new TimeSpan(2, 0, 0);
                binding.MaxReceivedMessageSize = Int32.MaxValue;
                binding.MaxBufferSize = Int32.MaxValue;

                return binding;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }


        public void  GenererLicence(string Datedebut,string NbreJour)
        {
            string _Licence;
            _Licence = Cryptage.Encrypt(Datedebut + "," + NbreJour) ;
            AuthentInitializeServiceClient client = new AuthentInitializeServiceClient();
            client.GenereLicenceCompleted  += (ss, res) =>
            {
                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    return;
                }

                if (res.Result == null )
                {
                    return;
                }
            };
            client.GenereLicenceAsync(_Licence );
        }
    }
}


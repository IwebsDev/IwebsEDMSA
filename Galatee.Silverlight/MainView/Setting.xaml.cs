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
using Galatee.Silverlight.ServiceAdministration;
using Galatee.Silverlight.Resources;

namespace Galatee.Silverlight.MainView
{
    public partial class Setting : ChildWindow
    {

        public event EventHandler Closed;
        List<CsDataBase> dbs = new List<CsDataBase>();
        public Setting()
        {
               try 
	            {	        
		         InitializeComponent();
                 GetConnexionInfos();
	            }
	            catch (Exception ex)
	            {
		          Message.Show(ex.Message,Galatee.Silverlight.Resources.Langue.errorTitle);
	            }
        }

        private void GetConnexionInfos()
        {
            try
            {
                AdministrationServiceClient connex = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                connex.RetourneBdConfigCompleted += (_, rest) =>
                    {
                        if (rest.Cancelled || rest != null)
                        {
                            Message.Show(Langue.wcf_error, Langue.errorTitle);
                            return ;
                        }

                        if (rest.Result == null && rest.Result.Count == 0)
                        {
                            Message.Show(Langue.msgNodata, Langue.errorTitle);
                            return;
                        }

                        dbs.AddRange(rest.Result);
                    };
                connex.RetourneBdConfigAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void InitialiserComposants(List<CsDataBase> dbs)
        {
            try
            {
                // données relatives à la BD
               CsDataBase db = dbs.First();
               txtdbServer.Text = db.ServerName;
               txtCatalogDb.Text = db.Catalog;
               txtpwd.Password = db.Password;

                // Données relatives au serveur applicatif( wcf)
               var address = ((App)Application.Current).ModuleServiceConfig["Caisse"].address.Split('/');

               txtApplServer.Text = address[2].Split(':')[0];
               txtportAppl.Text = address[2].Split(':')[1];

               txtAdrApplServer.Text = address[0] + address[1] + address[2];

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}


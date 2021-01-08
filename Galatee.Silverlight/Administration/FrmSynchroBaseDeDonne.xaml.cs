using Galatee.Silverlight.ServiceAdministration;
using Galatee.Silverlight.Tarification.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Administration
{
    public partial class FrmSynchroBaseDeDonne : ChildWindow
    {
        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();
        bool IsConsultation;
        protected virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion


        public FrmSynchroBaseDeDonne()
        {
            InitializeComponent();
            LoadRequest();
            GetProviderList();

            EnableConnectionScreen(false);
            
        }
        public FrmSynchroBaseDeDonne(string Requette, string Provider, string DataSource, string IniialeCatalog, string UserId, string Password,bool IsConsultation)
        {
            InitializeComponent();
            LoadRequest();
            GetProviderList();

            this.csimport.BASEDEDONNE =string.IsNullOrWhiteSpace( IniialeCatalog)?string.Empty:IniialeCatalog;
            this.csimport.MOTDEPASSE = string.IsNullOrWhiteSpace(Password) ? string.Empty : Password;
            this.csimport.PROVIDER =string.IsNullOrWhiteSpace(  Provider)?string.Empty:Provider;
            this.csimport.REQUTETTEBASEDISTANTE =string.IsNullOrWhiteSpace(  Requette)?string.Empty:Requette;
            this.csimport.SERVER = string.IsNullOrWhiteSpace(DataSource) ? string.Empty : DataSource;
            this.csimport.UTILISATEUR = string.IsNullOrWhiteSpace(UserId) ? string.Empty : UserId;

            this.IsConsultation = IsConsultation;
            EnableConnectionScreen(false);
            FillScreen(this.csimport.REQUTETTEBASEDISTANTE, this.csimport.PROVIDER, this.csimport.SERVER, this.csimport.BASEDEDONNE, this.csimport.UTILISATEUR, this.csimport.MOTDEPASSE);
            EnableScreen(IsConsultation);
        }

       
        #region Events

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.csimport.BASEDEDONNE=txtDataBase.Text;
            this.csimport.MOTDEPASSE=txtPassword.Text;
            this.csimport.PROVIDER=cmbProvider.SelectedItem.ToString();
            this.csimport.REQUTETTEBASEDISTANTE=txt_requette.Text;
            this.csimport.SERVER=txtSereverName.Text;
            this.csimport.UTILISATEUR = txtUserName.Text;

            MyEventArg.Bag = this.csimport;
            OnEvent(MyEventArg);
            //SaveAgent(donnesDatagrid);
            this.DialogResult = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Recuperartion de la requette dans le texte boxe
            string Requette = txt_requette.Text;

            //Execution du service qui prend en paramettre la requette et qui retourne une liste d'agent a inserer dans iWEBS
            LoadAgentBaseDistante(Requette, ConnectionString);

            //Chargement de la grid
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        //private void cmbServers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (chkUseWindowsSecurity.IsChecked.Value)
        //    {
        //        if (cmbServers.SelectedItem != null)
        //        {
        //            GetSQLDatabaseList(cmbServers.SelectedItem.ToString(), true, txtUserName.Text, txtPassword.Text);
        //        }
        //        else
        //        {

        //        }

        //    }
        //    else
        //    {

        //    }
        //}

        //private void chkUseWindowsSecurity_Checked(object sender, RoutedEventArgs e)
        //{
        //    //by default the button is checked if this is fire then the user intends using user authentification 
        //    //firstly reinstate the enabled state of the browse for databases button if its disabled 
        //    //if (btnFindDatabases.IsEnabled == false)
        //    //{
        //    //    btnFindDatabases.IsEnabled = true;
        //    //}

        //    if (chkUseWindowsSecurity.IsChecked.Value)
        //    {
        //        txtUserName.IsEnabled = false;
        //        txtPassword.IsEnabled = false;
        //        chkBlankPassAllowed.IsEnabled = false;
        //    }
        //    else
        //    {
        //        txtUserName.IsEnabled = true;
        //        txtPassword.IsEnabled = true;
        //        chkBlankPassAllowed.IsEnabled = true;
        //    }
        //}

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //it's possible to duplicate the list so we need to ensure that there isn't one already
            if (cmbProvider.Items.Count != 0)
            {
            }
            else
            {
                 GetProviderList();
            }
        }

        //private void btnFindServers_Click(object sender, RoutedEventArgs e)
        //{
        //    //it's possible to duplicate the list so we need to ensure that there isn't one already
        //    if (cmbServers.Items.Count != 0)
        //    {
        //    }
        //    else
        //    {
        //        //cmbServers.ItemsSource= GetSQLServerList();
        //    }
        //}

        //private void btnFindDatabases_Click(object sender, RoutedEventArgs e)
        //{
        //    //it's possible to duplicate the list so we need to ensure that there isn't one already 
        //    if (cmbDatabases.Items.Count != 0)
        //    {
        //    }
        //    else
        //    {


        //        if (chkUseWindowsSecurity.IsChecked.Value)
        //        {

        //            //we just need to do a check with no details 

        //            GetSQLDatabaseList(cmbServers.SelectedItem.ToString(), true, txtUserName.Text, txtPassword.Text);
        //        }
        //        else
        //        {
        //            //we need to do a check with details 
        //            GetSQLDatabaseList(cmbServers.SelectedItem.ToString(), false, txtUserName.Text, txtPassword.Text);
        //        }
        //    }
        //}

        #endregion

        #region Methodes

        //private void GetSQLDatabaseList(string serverInstanceName, bool useWindowsAuthentication, string username, string password)
        //{
        //    //return new List<string>().ToArray() ;
        //     AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
        //     client.GetSQLDatabaseListCompleted += (ss, res) =>
        //    {
        //        if (res.Cancelled || res.Error != null)
        //        {
        //            string error = res.Error.Message;
        //            Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
        //            return;
        //        }

        //        cmbDatabases.ItemsSource = res.Result;
        //        //now disable the find databses button if we have a list 
        //        if (cmbDatabases.Items.Count != 0)
        //        {
        //            btnFindDatabases.IsEnabled = false;
        //        }
        //        else
        //        {
        //            btnFindDatabases.IsEnabled = true;
        //        }
        //    };
        //     client.GetSQLDatabaseListAsync(serverInstanceName, useWindowsAuthentication, username, password);
        //}
        /*Select providers*/
        private void GetProviderList()
        {
            AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            client.GetProviderListCompleted += (ss, res) =>
            {
                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                    return;
                }

                cmbProvider.ItemsSource = res.Result;
                FillScreen(this.csimport.REQUTETTEBASEDISTANTE, this.csimport.PROVIDER, this.csimport.SERVER, this.csimport.BASEDEDONNE, this.csimport.UTILISATEUR, this.csimport.MOTDEPASSE);
            };
            client.GetProviderListAsync();
            
            //return new List<string>().ToArray();
        }

        private void GetSQLServerList()
        {
            ////return new List<string>().ToArray() ;
            //AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            //client.GetSQLServerListCompleted += (ss, res) =>
            //{
            //    if (res.Cancelled || res.Error != null)
            //    {
            //        string error = res.Error.Message;
            //        Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
            //        return;
            //    }

            //    cmbServers.ItemsSource = res.Result;
            //};
            //client.GetSQLServerListAsync(serverInstanceName, useWindowsAuthentication, username, password);
        }

        private void EnableConnectionScreen(bool state)
        {
            //cmbProvider.IsEnabled = state;
            //cmbServers.IsEnabled = state;
            //cmbDatabases.IsEnabled = state;

            //btnFindServers.IsEnabled = state;
            //btnFindDatabases.IsEnabled = state;
            btnTestConnection.IsEnabled = state;
            OKButton.IsEnabled = false;

            //chkUseWindowsSecurity.IsEnabled = state;
            //chkBlankPassAllowed.IsEnabled = state;
            txt_requette.IsEnabled = state;
            txtDataBase.IsEnabled = state;
            txtSereverName.IsEnabled = state;
            txtUserName.IsEnabled = state;
            txtPassword.IsEnabled = state;
        }
        private void EnableScreen(bool state)
        {
            cmbProvider.IsEnabled  = state;
            txt_requette.IsEnabled = state;
            txtDataBase.IsEnabled  = state;
            txtSereverName.IsEnabled = state;
            txtUserName.IsEnabled = state;
            txtPassword.IsEnabled = state;
        }

        private void FillScreen(string Requette, string Provider, string DataSource, string IniialeCatalog, string UserId, string Password)
        {
            if (cmbProvider.ItemsSource!=null)
	        {
                string ProviderToSelect = ((List<string>)cmbProvider.ItemsSource).FirstOrDefault(p => p == Provider);
                if (!string.IsNullOrWhiteSpace( ProviderToSelect))
                {
                    cmbProvider.SelectedItem = ProviderToSelect;
                }
            }
                txt_requette.Text = Requette!=null?Requette:string.Empty;
                txtDataBase.Text = IniialeCatalog != null ? IniialeCatalog : string.Empty; ;
                txtSereverName.Text = DataSource != null ? DataSource : string.Empty;
                txtUserName.Text = UserId != null ? UserId : string.Empty;
                txtPassword.Text = Password != null ? Password : string.Empty; 
	       



            
        }
        private void  LoadRequest()
        {
            AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            client.LoadRequestCompleted += (ss, res) =>
            {
                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                    return;
                }

                //if (res.Result == null || res.Result.Count == 0)
                //{
                //    Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
                //    return;
                //}
                //txt_requette.Text = res.Result;

            };
            client.LoadRequestAsync();
        }
        //service qui prend en paramettre la requette et qui retourne une liste d'utilisateur a inserer en base
        private void LoadAgentBaseDistante(string Requette, string ConnectionString)
        {
            AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            client.LoadAgentBaseDistanteCompleted += (ss, res) =>
            {
                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                    return;
                }

                if (res.Result == null || res.Result.Count == 0)
                {
                    Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
                    //lvwResultat.ItemsSource = new List<CsAgent>();
                    OKButton.IsEnabled = false;
                    return;
                }
                donnesDatagrid = res.Result;
                //lvwResultat.ItemsSource = donnesDatagrid;
                OKButton.IsEnabled = true;

            };
            //client.LoadAgentBaseDistanteAsync(Requette, ConnectionString);
            //client.LoadAgentBaseDistanteAsync(Requette, "Server=SYLLAPC;Database=GALADB_DEMO;User Id=sa;Password=P@ssw0rd;");
            client.LoadAgentBaseDistanteAsync(Requette, cmbProvider.SelectedItem.ToString(),txtSereverName.Text ,txtDataBase.Text ,txtUserName.Text ,txtPassword.Text );
        }

        private void TestBaseDistante(string Requette)
        {
            AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            client.TestBaseDistanteCompleted += (ss, res) =>
            {
                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                    return;
                }

                if (res.Result !=false)
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
            //client.LoadAgentBaseDistanteAsync(Requette, ConnectionString);
            //client.LoadAgentBaseDistanteAsync(Requette, "Server=SYLLAPC;Database=GALADB_DEMO;User Id=sa;Password=P@ssw0rd;");
            client.TestBaseDistanteAsync(Requette, cmbProvider.SelectedItem.ToString(), txtSereverName.Text, txtDataBase.Text, txtUserName.Text, txtPassword.Text);
        }

        private void SaveAgent(List<CsAgent> donnesDatagrid)
        {
            AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            client.SaveAgentCompleted += (ss, res) =>
            {
                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                    return;
                }

                if (res.Result!=false)
                {
                    Message.ShowInformation("Synchronisation effectuer avec suscces", Galatee.Silverlight.Resources.Langue.informationTitle);

                    OKButton.IsEnabled = false;
                }
                else
                {
                    Message.ShowInformation("Synchronisation ne c'est pas bien passer,veuillez verifiez la coherance des données en base,et que le parametre generale de code 000406 exite", Galatee.Silverlight.Resources.Langue.informationTitle);
                    OKButton.IsEnabled = true;
                }
            };
            client.SaveAgentAsync(donnesDatagrid, txt_requette.Text);
        }
        #endregion

        #region Variable
        List<CsAgent> donnesDatagrid = new List<CsAgent>();
        ObservableCollection<CsAgent> donnesSelected = new ObservableCollection<CsAgent>();
        aImportFichier csimport = new aImportFichier();
        string ConnectionString="Provider=OraOLEDB.Oracle;Data Source=MyOracleDB;User Id=myUsername;Password=myPassword";
        #endregion

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            ConnectionString = "";
        }

        private void cmbProvider_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbProvider.SelectedItem != null)
            {
                string provider = cmbProvider.SelectedItem.ToString();
                if (provider == "SqlClient Data Provider")
                {
                    if (this.IsConsultation)
                    {
                        EnableConnectionScreen(true);
                    }
                    
                }
                else
                {
                    EnableConnectionScreen(false);
                    Message.ShowInformation("Cet fournisseur n'est pour le moment pas pris en charge,veuillez contacter le fournisseur du logiciel", Galatee.Silverlight.Resources.Langue.informationTitle);
                }
            }
        }

        private void cmbDatabases_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            TestBaseDistante("");
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txt_requette_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void lvwResultat_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSereverName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //EnableConnectionScreen(false);
            OKButton.IsEnabled = false;
        }

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //EnableConnectionScreen(false);
            OKButton.IsEnabled = false;

        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            //EnableConnectionScreen(false);
            OKButton.IsEnabled = false;

        }

        private void txtDataBase_TextChanged(object sender, TextChangedEventArgs e)
        {
            //EnableConnectionScreen(false);
            OKButton.IsEnabled = false;

        }

        


       

    }
}


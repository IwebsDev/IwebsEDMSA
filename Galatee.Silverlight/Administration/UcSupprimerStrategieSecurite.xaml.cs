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
using Galatee.Silverlight.Resources;
using Galatee.Silverlight.ServiceAdministration;

namespace Galatee.Silverlight.Administration
{
    public partial class UcSupprimerStrategieSecurite : ChildWindow
    {
        DataGrid datagrid = null;
        CsUtilisateur userselected = null;
        CsStrategieSecurite security = null;
        public UcSupprimerStrategieSecurite(object[] _user,SessionObject.ExecMode[] pExecMode,DataGrid[] pGrid)
        {
            InitializeComponent();
            try
            {
                datagrid = pGrid[0];
                    security = _user[0] as CsStrategieSecurite;
                    MessageShow.Text = Langue.demandeSuppressionSecurity;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Langue.errorTitle);
            }
        }

        public UcSupprimerStrategieSecurite()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               SupprimerSecurite();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Langue.errorTitle);
            }
        }

        void SupprimerSecurite()
        {
            try
            {
                AdministrationServiceClient delete = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                delete.DeleteStrategieSecuriteCompleted += (senderdel, resultDel) =>
                {
                    try
                    {
                        if (resultDel.Cancelled || resultDel.Error != null)
                        {
                            string error = resultDel.Error.Message;
                            Message.Show(Galatee.Silverlight.Resources.Administration.Langue.MsgErrorInsertUser, Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }

                        if (resultDel.Result == false)
                        {
                            Message.Show(Galatee.Silverlight.Resources.Administration.Langue.MsgErrorInsertUser, Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }
                        else
                        {
                            Message.Show(Galatee.Silverlight.Resources.Langue.suppressionSuccess, Galatee.Silverlight.Resources.Langue.informationTitle);
                            List<CsStrategieSecurite> _securite = datagrid.ItemsSource as List<CsStrategieSecurite>;
                            _securite.Remove(security);
                            datagrid.ItemsSource = null;
                            datagrid.ItemsSource = _securite;
                            this.DialogResult = true;
                        }
                    }
                    catch (Exception ew)
                    {
                        Message.Show(ew.Message, Langue.errorTitle);
                    }

                };
                delete.DeleteStrategieSecuriteAsync(security.PK_IDSTRATEGIESECURITE);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Langue.errorTitle);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}


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
    public partial class UcSupprimerUser : ChildWindow
    {
        DataGrid datagrid = null;
        CsUtilisateur userselected = null;
        CsStrategieSecurite security = null;
        public UcSupprimerUser(object[] _user,SessionObject.ExecMode[] pExecMode,DataGrid[] pGrid)
        {
            InitializeComponent();
            try
            {
                datagrid = pGrid[0];
                if (pExecMode[0] == SessionObject.ExecMode.Active)
                {
                    security = _user[0] as CsStrategieSecurite;
                    MessageShow.Text = Langue.demandedActivation;
                }
                else
                {
                    userselected = _user[0] as CsUtilisateur;
                    MessageShow.Text = Langue.demandeDeSuppression;
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Langue.errorTitle);
            }
        }

        public UcSupprimerUser(CsUtilisateur _user, DataGrid  pGrid)
        {
            InitializeComponent();
            try
            {
                    datagrid = pGrid ;
                    userselected = _user;
                    MessageShow.Text = Langue.demandeDeSuppression;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Langue.errorTitle);
            }
        }

       public UcSupprimerUser()
        {
            MessageShow.Text = Langue.demandeDeSuppression;
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (security == null)
                {
                    userselected.DATEMODIFICATION = DateTime.Now;
                    userselected.USERMODIFICATION = UserConnecte.matricule;
                    SupprimerUtilisateur();
                }
                else
                    if (security != null)
                        ActivateSecurite();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Langue.errorTitle);
            }
        }

        void ActivateSecurite()
        {
            try
            {
                security.ACTIF = true;
                AdministrationServiceClient active = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                active.SetActifStrategieSecuriteCompleted += (senderupdate, resultupdate) =>
                {
                    try
                    {
                        if (resultupdate.Cancelled || resultupdate.Error != null)
                        {
                            string error = resultupdate.Error.Message;
                            Message.Show(Galatee.Silverlight.Resources.Administration.Langue.MsgErrorOnActiveSecurity, Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }

                        if (resultupdate.Result == null || resultupdate.Result.Count == 0)
                        {
                            Message.Show(Galatee.Silverlight.Resources.Administration.Langue.MsgErrorOnActiveSecurity, Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }
                        else
                        {
                            Message.Show(Galatee.Silverlight.Resources.Langue.activateeSuccess, Galatee.Silverlight.Resources.Langue.informationTitle);
                            datagrid.ItemsSource = null;
                            datagrid.ItemsSource = resultupdate.Result;
                            datagrid.UpdateLayout();
                            this.DialogResult = true;
                        }
                    }
                    catch (Exception rx )
                    {

                        Message.Show(rx.Message, Langue.errorTitle);
                    }

                };
                active.SetActifStrategieSecuriteAsync(security.PK_IDSTRATEGIESECURITE);
            }
            catch (Exception rx)
            {
                
                throw rx;
            }
        }

        void SupprimerUtilisateur()
        {
            try
            {
                AdministrationServiceClient delete = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                delete.DeleteUserCompleted += (senderdel, resultDel) =>
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
                            List<CsUtilisateur> listeuser = datagrid.ItemsSource as List<CsUtilisateur>;
                            listeuser.Remove(userselected);
                            datagrid.UpdateLayout();
                            this.DialogResult = true;
                        }
                    }
                    catch (Exception ew)
                    {
                        Message.Show(ew.Message, Langue.errorTitle);
                    }

                };
                delete.DeleteUserAsync(userselected);
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


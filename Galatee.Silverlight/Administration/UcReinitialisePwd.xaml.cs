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
//using Galatee.Silverlight.Security;
//using Galatee.Silverlight.Resources.Administration;
using Galatee.Silverlight.Resources;

namespace Galatee.Silverlight.Administration
{
    public partial class UcReinitialisePwd : ChildWindow
    {
        public UcReinitialisePwd()
        {
            InitializeComponent();
        }

        DataGrid gridView = null;
        CsUtilisateur userSelected;
        public UcReinitialisePwd(object[] _user, SessionObject.ExecMode[] pExecMode, DataGrid[] grid)
        {
            try
            {
                InitializeComponent();
                userSelected = _user[0] as CsUtilisateur;
                Translate();
                InitialiseComposants(userSelected);
                gridView = grid[0];
                Tag = pExecMode[0];
                this.ckc_Changepwd.IsChecked = true;
                this.ckc_Changepwd.IsEnabled = false;
                this.txt_Login.IsReadOnly = true;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
              
            }
        }
        public UcReinitialisePwd(Galatee.Silverlight.ServiceAdministration.CsUtilisateur _user, SessionObject.ExecMode  pExecMode )
        {
            try
            {
                InitializeComponent();
                userSelected = _user ;
                Translate();
                InitialiseComposants(userSelected);
                this.ckc_Changepwd.IsChecked = true;
                this.ckc_Changepwd.IsEnabled = false;
                this.txt_Login.IsReadOnly = true;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
              
            }
        }
        
        void InitialiseComposants(CsUtilisateur user)
        {
            txt_Login.Text = string.Format("{0} ({1})", user.LIBELLE  , user.MATRICULE );
        }

         private void Translate()
        {
            try
            {
                lbl_login.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_login;
                lbl_new_pwd.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_new_pwd;
                //lbl_old_pwd.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_old_pwd;
                ckc_Changepwd.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_changement_pwd;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
               
            }
        }

         private void ValiderModifPassWord()
         {
             try
             {
                 Security.CsStrategieSecurite security = new Security.CsStrategieSecurite();
                 Utility.ParseObject<Security.CsStrategieSecurite, ServiceAuthenInitialize.CsStrategieSecurite>(security, SessionObject.securiteActive);
                 //security = Utility.ParseObject<CsStrategieSecurite, ServiceAdministration.CsStrategieSecurite>(security, SessionObject.securite);
                 CsUtilisateur user = new CsUtilisateur();
                 user = Utility.ParseObject(user, userSelected);
                 user.INITUSERPASSWORD = ckc_Changepwd.IsChecked;
                 //Galatee.Silverlight.Security.Securities.CheckConfirmPassword(txt_newPwd.Text, txt_confPwd.Text);
                 user.PASSE = Galatee.Silverlight.Security.Cryptage.GetPasswordToBeSaved(security, userSelected.LOGINNAME, txt_newPwd.Text);
                 //user.FONCTION = "Galatee";
                 user.DATEDERNIEREMODIFICATIONPASSWORD = DateTime.Now;
                 user.USERCREATION = UserConnecte.matricule;
                 AdministrationServiceClient adm = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));

                 adm.UpdateUserCompleted += (updates, resultupt) =>
                 {
                     if (resultupt.Cancelled || resultupt.Error != null)
                     {
                         string error = resultupt.Error.Message;
                         Message.Show(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                         return;
                     }

                     if (resultupt.Result == false)
                     {
                         Message.Show(Galatee.Silverlight.Resources.Langue.updateError, Galatee.Silverlight.Resources.Langue.errorTitle);
                         return;
                     }
                     else
                     {
                         Message.Show(Galatee.Silverlight.Resources.Langue.updateSuccess, Galatee.Silverlight.Resources.Langue.ConfirmationTitle);
                         //UpdateGridview(user);
                         this.DialogResult = true;

                     }
                 };
                 adm.UpdateUserAsync(user, true);
             }
             catch (Exception ex)
             {
                 Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);

             }

         }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ValiderModifPassWord();
        }

        private void UpdateGridview(CsUtilisateur newUser)
        {
            try
            {
              List<CsUtilisateur> users =  gridView.ItemsSource as List<CsUtilisateur>;
              users.Remove(userSelected);
              users.Add(newUser);
              gridView.ItemsSource = null;
              gridView.ItemsSource = users;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);

        }

        private void ChildWindow_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                if (!string.IsNullOrEmpty(txt_Login.Text) && !string.IsNullOrEmpty(this.txt_newPwd.Text))
                    OKButton_Click(null, null);
            }
        }
    }
}


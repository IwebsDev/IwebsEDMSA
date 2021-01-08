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
using Galatee.Silverlight.Security;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.ServiceAdministration;

namespace Galatee.Silverlight.Administration
{
    public partial class FrmImportFichier : ChildWindow
    {
        private Object ModeExecution = null;
        static int code_import=0;
        public FrmImportFichier()
        {
            InitializeComponent();
            ModeExecution = SessionObject.ExecMode.Creation;
        }
        public FrmImportFichier(aImportFichier codeimport, SessionObject.ExecMode pExecMode)
        {
            InitializeComponent();
            ChargeInformation(codeimport.CODE);
            ModeExecution = pExecMode;
            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
            {
                AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
            }
        }
        
        aImportFichier SelectImport = new aImportFichier();

        private void ChargeInformation(int codeimport)
        {
            AdministrationServiceClient service1 = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            service1.GetImportFichierCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;
                SelectImport = res.Result;
                if (SelectImport != null)
                {
                    code_import = codeimport;
                    txtTitre.Text = SelectImport.LIBELLE;
                    txtDesc.Text = SelectImport.DESCRIPTION;
                    txtProc.Text = SelectImport.COMMANDE;
                    txtNbCol.Text = SelectImport.NBPARAMETRE.ToString();
                    ChkProcedure.IsChecked = SelectImport.ISPROCEDURE;
                             
                }
                if (!string.IsNullOrWhiteSpace(SelectImport.REPERTOIRE) && !string.IsNullOrWhiteSpace(SelectImport.FICHIER))
                {
                    txtRepert.Text = SelectImport.REPERTOIRE;
                    txtFichier.Text = SelectImport.FICHIER;
                    chkb_Fichier.IsChecked = true;
                }
                else if (!string.IsNullOrEmpty(SelectImport.PROVIDER) && !string.IsNullOrEmpty(SelectImport.SERVER) && !string.IsNullOrEmpty(SelectImport.BASEDEDONNE) && !string.IsNullOrEmpty(SelectImport.UTILISATEUR) && !string.IsNullOrEmpty(SelectImport.MOTDEPASSE))
                {
                    chkb_BaseDeDonnee.IsChecked = true;

	            }
                else
                {
                    chkb_Fichier.IsChecked = true;
                    chkb_BaseDeDonnee.IsChecked = false;
                }
                btn_Config_Bd.IsEnabled = true;
              
            };
            service1.GetImportFichierAsync(codeimport);
            service1.CloseAsync();
        }
        private void GetColonneImport(int codeImport)
        {
            List<aImportFichierColonne> listColonne = new List<aImportFichierColonne>();
            AdministrationServiceClient service1 = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            service1.GetAllImportFichierColonneCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;
                listColonne = res.Result;
                if (listColonne != null && listColonne.Count > 0)
                {
                    txtNbCol.Text = listColonne.Count.ToString();
                }

            };
            service1.GetAllImportFichierColonneAsync(codeImport);
            service1.CloseAsync();

        }
     
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            
            //if (txtTitre.Text != null && !string.IsNullOrEmpty(txtProc.Text) && !string.IsNullOrEmpty(txtRepert.Text) && !string.IsNullOrEmpty(txtFichier.Text))
            //{
                if (!string.IsNullOrEmpty(txtNbCol.Text))
                {
                    if (txtTitre.Text != null && !string.IsNullOrEmpty(txtProc.Text) )
                    {
                        aImportFichier importation = new aImportFichier()
                        {
                            CODE = code_import,
                            LIBELLE = txtTitre.Text,
                            DESCRIPTION = txtDesc.Text,
                            COMMANDE = txtProc.Text,
                            //REPERTOIRE = txtRepert.Text,
                            //FICHIER = txtFichier.Text,
                            NBPARAMETRE = (string.IsNullOrEmpty(txtNbCol.Text)) ? 0 : int.Parse(txtNbCol.Text),
                            ISPROCEDURE= (ChkProcedure.IsChecked == true)? true:false,
                           
                        };
                        if (!string.IsNullOrEmpty(txtRepert.Text) && !string.IsNullOrEmpty(txtFichier.Text))
                        {
                            //importation.COMMANDE = txtProc.Text;
                            importation.FICHIER = txtFichier.Text;
                            importation.REPERTOIRE = txtRepert.Text;
                        }
                         else if (!string.IsNullOrEmpty(SelectImport.PROVIDER) && !string.IsNullOrEmpty(SelectImport.SERVER)&& !string.IsNullOrEmpty(SelectImport.BASEDEDONNE)&& !string.IsNullOrEmpty(SelectImport.UTILISATEUR)&& !string.IsNullOrEmpty(SelectImport.MOTDEPASSE))
                        {
                            importation.BASEDEDONNE             = SelectImport.BASEDEDONNE;
                            importation.MOTDEPASSE              = SelectImport.MOTDEPASSE;
                            importation.PROVIDER                = SelectImport.PROVIDER;
                            importation.REQUTETTEBASEDISTANTE   = SelectImport.REQUTETTEBASEDISTANTE;
                            importation.SERVER                  = SelectImport.SERVER;
                            importation.UTILISATEUR             =SelectImport.UTILISATEUR;
                        }else
	                    {
                             Message.ShowInformation("Veuillez saisir les parametres ", "Paramétrage");
	                    }
                       

                        int UpdateImport = 0;
                        AdministrationServiceClient service1 = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                        service1.MisaAjourImportFichierCompleted += (sr, res) =>
                        {
                            if (res != null && res.Cancelled)
                                return;
                            UpdateImport = res.Result;
                            if (UpdateImport == 0)
                            {
                                code_import = UpdateImport;
                            }


                        };
                        service1.MisaAjourImportFichierAsync(importation);
                        service1.CloseAsync();
                    }
                    else
                    {
                        Message.ShowInformation("Veuillez saisir les parametres ", "Paramétrage");

                    }
                }
                else
                {
                    Message.ShowInformation("Ajouter au moins une colonne", "Paramétrage");
                    this.DialogResult = true;
                }
            //}
            //else
            //{
            //    Message.ShowInformation("Veuillez saisir les parametres ", "Paramétrage");
                
            //}
            this.DialogResult = true;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);

        }


        private void btnStructure_Click_1(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(txtNbCol.Text))
            {
                if (txtTitre.Text != null && !string.IsNullOrEmpty(txtProc.Text) )
                {
                        aImportFichier importation = new aImportFichier();

                    if (!string.IsNullOrEmpty(txtRepert.Text) && !string.IsNullOrEmpty(txtFichier.Text))
                    {
                        //{
                            importation.CODE = code_import;
                            importation.LIBELLE = txtTitre.Text;
                            importation.DESCRIPTION = txtDesc.Text;
                            importation.COMMANDE = txtProc.Text;
                            importation.REPERTOIRE = txtRepert.Text;
                            importation.FICHIER = txtFichier.Text;
                            importation.NBPARAMETRE = (string.IsNullOrEmpty(txtNbCol.Text)) ? 0 : int.Parse(txtNbCol.Text);
                            importation.ISPROCEDURE = (ChkProcedure.IsChecked == true) ? true : false;

                        //};
                    }
                    else if (!string.IsNullOrEmpty(SelectImport.PROVIDER) && !string.IsNullOrEmpty(SelectImport.SERVER)&& !string.IsNullOrEmpty(SelectImport.BASEDEDONNE)&& !string.IsNullOrEmpty(SelectImport.UTILISATEUR)&& !string.IsNullOrEmpty(SelectImport.MOTDEPASSE))
                    {
                            importation.CODE = code_import;
                            importation.LIBELLE = txtTitre.Text;
                            importation.DESCRIPTION = txtDesc.Text;
                            importation.COMMANDE = txtProc.Text;
                            importation.PROVIDER = SelectImport.PROVIDER;
                            importation.SERVER = SelectImport.SERVER;
                            importation.BASEDEDONNE = SelectImport.BASEDEDONNE;
                            importation.UTILISATEUR = SelectImport.UTILISATEUR;
                            importation.MOTDEPASSE = SelectImport.MOTDEPASSE;
                            importation.REQUTETTEBASEDISTANTE = SelectImport.REQUTETTEBASEDISTANTE;
                            importation.NBPARAMETRE = (string.IsNullOrEmpty(txtNbCol.Text)) ? 0 : int.Parse(txtNbCol.Text);
                            importation.ISPROCEDURE = (ChkProcedure.IsChecked == true) ? true : false;
                    }
                    else
                    {
                        Message.ShowInformation("Veuillez saisir les parametres ", "Paramétrage");

                    }
                    

                    int UpdateImport = 0;
                    AdministrationServiceClient service1 = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                    service1.MisaAjourImportFichierCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        UpdateImport = res.Result;
                        if (UpdateImport != 0)
                        {
                            code_import = UpdateImport;
                            FrmImportFichierColonne ctr = new FrmImportFichierColonne(code_import);
                            ctr.Closed += form_Closed;
                            ctr.Show();
                            
                        }


                    };
                    service1.MisaAjourImportFichierAsync(importation);
                    service1.CloseAsync();
                }
                else
                {
                    Message.ShowInformation("Veuillez saisir les parametres ", "Paramétrage");

                }

            }
            else
            {
                FrmImportFichierColonne ctr = new FrmImportFichierColonne(code_import);
                ctr.Closed += form_Closed;
                     
                ctr.Show();
            }
           
         
        }
        void form_Closed(object sender, EventArgs e)
        {
            try
            {
                GetColonneImport(code_import);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Le 08-07-2015 Sylla :Ajout de la fonction de synchro à partir de base de donnée

        #region Events
            private void chkb_Fichier_Checked(object sender, RoutedEventArgs e)
            {
                btn_Config_Bd.Visibility = Visibility.Collapsed;

                lblFichier.Visibility = Visibility.Visible;
                lblRepert.Visibility = Visibility.Visible;

                txtFichier.Visibility = Visibility.Visible;
                txtRepert.Visibility = Visibility.Visible;

            }
            private void chkb_BaseDeDonnee_Checked(object sender, RoutedEventArgs e)
            {
                btn_Config_Bd.Visibility = Visibility.Visible ;

                lblFichier.Visibility = Visibility.Collapsed;
                lblRepert.Visibility = Visibility.Collapsed;

                txtFichier.Visibility = Visibility.Collapsed;
                txtRepert.Visibility = Visibility.Collapsed;
            }

            private void btn_Config_Bd_Click(object sender, RoutedEventArgs e)
            {
                FrmSynchroBaseDeDonne frm = new FrmSynchroBaseDeDonne(SelectImport.REQUTETTEBASEDISTANTE, SelectImport.PROVIDER, SelectImport.SERVER, SelectImport.BASEDEDONNE, SelectImport.UTILISATEUR, SelectImport.MOTDEPASSE, (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation?false:true);
                 frm.CallBack += frm_CallBack;
            frm.Show();
            }

            private void frm_CallBack(object sender, Tarification.Helper.CustumEventArgs e)
            {
                SelectImport = (aImportFichier)e.Bag;
            }
        #endregion

            

        #endregion


    }
}


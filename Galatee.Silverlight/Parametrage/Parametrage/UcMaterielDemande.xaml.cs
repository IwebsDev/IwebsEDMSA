using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.ServiceParametrage;
using Galatee.Silverlight.Resources.Parametrage;
namespace Galatee.Silverlight.Parametrage
{
    public partial class UcMaterielDemande : ChildWindow
    {
        List<CsMaterielDemande > listForInsertOrUpdate = null;
        ObservableCollection<CsMaterielDemande> donnesDatagrid = new ObservableCollection<CsMaterielDemande>();
        private CsMaterielDemande ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcMaterielDemande()
        {
            try
            {
                InitializeComponent();
                Translate();
                this.Txt_Code.MaxLength = 1;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleCodePoste);
            }
        }
        public UcMaterielDemande(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                this.Txt_Code.MaxLength = 1;
                var categorieClient = new CsMaterielDemande();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsMaterielDemande);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsMaterielDemande>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_Code.Text = ObjetSelectionnee.CODE;
                        Txt_Libelle.Text = ObjetSelectionnee.LIBELLE;
                        Txt_CoutFourniture.Text = ObjetSelectionnee.COUTUNITAIRE_FOURNITURE.Value.ToString(SessionObject.FormatMontant);
                        Txt_CoutPose.Text = ObjetSelectionnee.COUTUNITAIRE_POSE.Value.ToString(SessionObject.FormatMontant); ;
                        btnOk.IsEnabled = false;
                    }
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot,false);
                }
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleCodePoste);
            }
        }

        private void GetDataNew()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllMaterielCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.ListeCodePoste);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var pMateriel in args.Result)
                            donnesDatagrid.Add(pMateriel);
                };
                client.SelectAllMaterielAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(CsMaterielDemande  pMateriel)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                    GetDataNew();
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                    GetDataNew();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Translate()
        {
            try
            {
                Title = Languages.LibelleCodePoste;
                btnOk.Content = Languages.OK;
                Btn_Reinitialiser.Content = Languages.Annuler;
                //GboCodeDepart.Header = Languages.InformationsCodePoste;
                lab_Code.Content = Languages.Code;
                lab_Libelle.Content = Languages.Libelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsMaterielDemande > GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsMaterielDemande>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var Materiel = new CsMaterielDemande
                    {
                        CODE = Txt_Code.Text,
                        LIBELLE = Txt_Libelle.Text,
                        COUTUNITAIRE_FOURNITURE = Convert.ToDecimal(Txt_CoutFourniture.Text),
                        COUTUNITAIRE_POSE = Convert.ToDecimal(Txt_CoutPose.Text),
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule
                    };
                    listObjetForInsertOrUpdate.Add(Materiel);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.CODE = Txt_Code.Text;
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text;
                    ObjetSelectionnee.COUTUNITAIRE_FOURNITURE = Convert.ToDecimal(Txt_CoutFourniture.Text);
                    ObjetSelectionnee.COUTUNITAIRE_POSE = Convert.ToDecimal(Txt_CoutPose.Text);
                    ObjetSelectionnee.DATEMODIFICATION = DateTime.Now;
                    ObjetSelectionnee.USERMODIFICATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleCodePoste);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.QuestionEnregistrerDonnees, "Parametrage", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        listForInsertOrUpdate = GetInformationsFromScreen();
                        var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));

                        if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        {
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                            {
                                service.InsertMaterielCompleted  += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message,"Parametrage");
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, "Parametrage");
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertMaterielAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateMaterielCompleted  += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message,"Parametrage");
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, "Parametrage");
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateMaterielAsync(listForInsertOrUpdate);
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                };
                messageBox.Show();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.LibelleCodePoste);
            }
        }
         
        private void VerifierSaisie()
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_Code.Text) && !string.IsNullOrEmpty(Txt_Libelle.Text) && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation)
                    btnOk.IsEnabled = true;
                else
                {
                    btnOk.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btn_Annuler_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Reinitialiser();
                DialogResult = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleCodePoste);
            }
        }

        private void Reinitialiser()
        {
            try
            {
                Txt_Code.Text = string.Empty;
                Txt_Libelle.Text = string.Empty;
                btnOk.IsEnabled = false;
                Txt_Code.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void On_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.LibelleCodePoste);
            }
        }
    }
}



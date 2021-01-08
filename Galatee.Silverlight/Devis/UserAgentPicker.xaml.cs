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
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
//using Galatee.Silverlight.ServiceAdministration;
using Galatee.Silverlight.ServiceAccueil ;

namespace Galatee.Silverlight.Devis
{
    public partial class UserAgentPicker : ChildWindow
    {
        public CsUtilisateur AgentSelectionne { get; set; }
        string CodeUser = string.Empty;
        public UserAgentPicker()
        {
            try
            {
                InitializeComponent();
                Initialisation();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        public UserAgentPicker(string _CodeUser)
        {
            try
            {
                InitializeComponent();
                Initialisation();
                CodeUser = _CodeUser;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void RemplirFonction()
        {
            try
            {
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.SelectAllFonctionCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.txtDevis);
                    }

                    if (args.Result != null)
                    {
                        List<ServiceAccueil. CsFonction> _lstFonction = new List<ServiceAccueil.CsFonction >();
                        _lstFonction = args.Result;
                        if (!string.IsNullOrEmpty(CodeUser))
                            _lstFonction = args.Result.Where(t => t.CODE == CodeUser).ToList();
                        Cbo_Fonction.Items.Clear();
                        Cbo_Fonction.SelectedValuePath = "CODE";
                        Cbo_Fonction.DisplayMemberPath = "ROLEDISPLAYNAME";
                        Cbo_Fonction.ItemsSource = _lstFonction;
                    }
                };
                client.SelectAllFonctionAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Initialisation()
        {
            try
            {
                RemplirFonction();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Dtg_agent.SelectedItem != null)
                {
                    this.BtnOK.IsEnabled = false;
                    this.BtnCancel.IsEnabled = false;
                    AgentSelectionne = Dtg_agent.SelectedItem as ServiceAccueil.CsUtilisateur;
                    this.DialogResult = true;
                }
                else
                {
                    throw new Exception(Languages.msgEmptyUser);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_Fonction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Fonction.SelectedItem != null)
                {
                    var objFonction = Cbo_Fonction.SelectedItem as Galatee.Silverlight.ServiceAccueil .CsFonction;
                    if (objFonction != null)
                        Txt_codeFonction.Text = objFonction.CODE;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Btn_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Recherche();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void Recherche()
        {
            ObjMATRICULE critere = new ObjMATRICULE();
            try
            {
                busyIndicator.IsBusy = true;
                var admClient =  new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                critere.MATRICULE = (string.IsNullOrEmpty(this.Txt_matricule.Text)) ? null : this.Txt_matricule.Text;
                critere.LIBELLE = (string.IsNullOrEmpty(this.Txt_name.Text)) ? null : this.Txt_name.Text;
                critere.FONCTION = (string.IsNullOrEmpty(this.Txt_codeFonction.Text))
                                       ? null
                                       : this.Txt_codeFonction.Text;

                LayoutRoot.Cursor = Cursors.Wait;
                admClient.GetUserByIdFonctionMatriculeNomCompleted += (sen, result) =>
                {
                    if (result.Cancelled || result.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = result.Error.Message;
                        Message.ShowError(error, Languages.txtDevis);
                        return;
                    }
                    if (result.Result == null || result.Result.Count == 0)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(
                                "Aucun agent ne correspond aux critères saisis !",
                                Languages.txtDevis);
                        return;
                    }
                    if (result.Result != null && result.Result.Count > 0)
                    {
                        List<CsUtilisateur> lstUtulisateur = new List<CsUtilisateur>();
                        var lstCentreDistnct = result.Result.Select(t => new { t.MATRICULE ,t.LIBELLE  }).Distinct().ToList();
                        foreach (var item in lstCentreDistnct)
                        {
                            CsUtilisateur leUSer = new CsUtilisateur();
                            leUSer.MATRICULE = item.MATRICULE ;
                            leUSer.LIBELLE  = item.LIBELLE;
                            leUSer.PK_ID = result.Result.FirstOrDefault(t => t.MATRICULE == item.MATRICULE).PK_ID;
                            lstUtulisateur.Add(leUSer);
                        }
                        this.Dtg_agent.ItemsSource = lstUtulisateur;
                    }
                    busyIndicator.IsBusy = false;
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                admClient.GetUserByIdFonctionMatriculeNomAsync(critere.FONCTION, critere.MATRICULE, critere.LIBELLE);
            }
            catch (Exception ex)
            {
                busyIndicator.IsBusy = false;
                throw ex;
            }
        }

        private void Btn_reset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Dtg_agent.ItemsSource = null;
                this.Cbo_Fonction.SelectedItem = null;
                this.Txt_matricule.Text = string.Empty;
                this.Txt_name.Text = string.Empty;
                this.Txt_codeFonction.Text = string.Empty;
                AgentSelectionne = null;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
    }
}


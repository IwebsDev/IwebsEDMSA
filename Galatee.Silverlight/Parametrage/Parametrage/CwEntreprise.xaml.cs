using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceParametrage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Parametrage
{
    public partial class CwEntreprise : ChildWindow
    {
        private bool IsLogoSelectionnee = false;
        private bool IsForInsert = false;
        private CsEntreprise VgEntreprise = null;
        public CwEntreprise()
        {
            try
            {
                InitializeComponent();
                GetDataFromDataBase();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Silverlight.Resources.Parametrage.Languages.Parametrage);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ent = GetInformationsFromScreen();
                EnregistrerDonnes(ent);
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Silverlight.Resources.Parametrage.Languages.Parametrage);
            }
        }

        private void EnregistrerDonnes(CsEntreprise pEntreprise)
        {
            try
            {
                if (IsForInsert)
                {
                    ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                    client.InsertEntrepriseCompleted += (ssender, args) =>
                    {
                        try
                        {
                            if (args.Cancelled || args.Error != null)
                            {
                                string error = args.Error.Message;
                                Message.ShowError(error, Languages.Parametrage);
                                return;
                            }
                            if (!args.Result)
                            {
                                Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Parametrage);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex.Message, Languages.Parametrage);
                        }
                    };
                    client.InsertEntrepriseAsync(pEntreprise);
                }
                else
                {
                    ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                    client.UpdateEntrepriseCompleted += (ssender, args) =>
                    {
                        try
                        {
                            if (args.Cancelled || args.Error != null)
                            {
                                string error = args.Error.Message;
                                Message.ShowError(error, Languages.Parametrage);
                                return;
                            }
                            if (!args.Result)
                            {
                                Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Parametrage);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex.Message, Languages.Parametrage);
                        }
                    };
                    client.UpdateEntrepriseAsync(pEntreprise);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDataFromDataBase()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.GetAllEntrepriseCompleted += (ssender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError(error, Languages.Parametrage);
                            return;
                        }
                        if (args.Result == null)
                        {
                            //Message.Show(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            //return;
                        }
                        else
                        {
                            IsForInsert = false;
                            VgEntreprise = args.Result;
                            RenseignerInformationsEntreprise(VgEntreprise);
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Languages.Parametrage);
                    }
                };
                client.GetAllEntrepriseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsEntreprise(CsEntreprise pEntreprise)
        {
            try
            {
                TxtActivite.Text = pEntreprise.ACTIVITE ?? string.Empty;
                TxtAdressePrincipale.Text = pEntreprise.ADRESSEPRINCIPALE ?? string.Empty;
                TxtAdresseSecondaire.Text = pEntreprise.ADRESSESECONDAIRE ?? string.Empty;
                TxtEmailPrincipale.Text = pEntreprise.EMAILPRINCIPALE ?? string.Empty;
                TxtEmailSecondaire.Text = pEntreprise.EMAILSECONDAIRE ?? string.Empty;
                TxtFaxPrincipale.Text = pEntreprise.FAXPRINCIPALE ?? string.Empty;
                TxtFaxSecondaire.Text = pEntreprise.FAXSECONDAIRE ?? string.Empty;
                TxtNom.Text = pEntreprise.NOM ?? string.Empty;
                TxtPays.Text = pEntreprise.PAYS ?? string.Empty;
                TxtSigle.Text = pEntreprise.SIGLE ?? string.Empty;
                TxtSiteWeb.Text = pEntreprise.SITEINTERNET ?? string.Empty;
                TxtSlogan.Text = pEntreprise.SLOGAN ?? string.Empty;
                TxtTelPrincipale.Text = pEntreprise.TELEPHONEPRINCIPAL ?? string.Empty;
                TxtTelSecondaire.Text = pEntreprise.TELEPHONESECONDAIRE ?? string.Empty;
                if (pEntreprise.LOGO != null)
                {
                    HypLinkLogo.Tag = pEntreprise.LOGO;
                    this.HypLinkLogo.Content = "Logo inséré";
                    IsLogoSelectionnee = true;
                }
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

        private void VerifierSaisie()
        {
            try
            {
                if (!string.IsNullOrEmpty(TxtNom.Text) && !string.IsNullOrEmpty(TxtSigle.Text) && !string.IsNullOrEmpty(TxtAdressePrincipale.Text)
                    && !string.IsNullOrEmpty(TxtTelPrincipale.Text) && !string.IsNullOrEmpty(TxtFaxPrincipale.Text) && !string.IsNullOrEmpty(TxtPays.Text)
                    && HypLinkLogo.Tag != null)
                    OKButton.IsEnabled = true;
                else
                    OKButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private CsEntreprise GetInformationsFromScreen()
        {
            CsEntreprise Entreprise = null;
            try
            {
                if (VgEntreprise == null)
                {
                    Entreprise = new CsEntreprise();
                    IsForInsert = true;
                }
                else
                {
                    Entreprise = VgEntreprise;
                    IsForInsert = false;
                }
                if (!string.IsNullOrEmpty(TxtActivite.Text))
                    Entreprise.ACTIVITE = TxtActivite.Text;
                if (!string.IsNullOrEmpty(TxtAdressePrincipale.Text))
                    Entreprise.ADRESSEPRINCIPALE = TxtAdressePrincipale.Text;
                if (!string.IsNullOrEmpty(TxtAdresseSecondaire.Text))
                    Entreprise.ADRESSESECONDAIRE = TxtAdresseSecondaire.Text;
                if (!string.IsNullOrEmpty(TxtEmailPrincipale.Text))
                    Entreprise.EMAILPRINCIPALE = TxtEmailPrincipale.Text;
                if (!string.IsNullOrEmpty(TxtEmailSecondaire.Text))
                    Entreprise.EMAILSECONDAIRE = TxtEmailSecondaire.Text;
                if (!string.IsNullOrEmpty(TxtFaxPrincipale.Text))
                    Entreprise.FAXPRINCIPALE = TxtFaxPrincipale.Text;
                if (!string.IsNullOrEmpty(TxtFaxSecondaire.Text))
                    Entreprise.FAXSECONDAIRE = TxtFaxSecondaire.Text;
                if (!string.IsNullOrEmpty(TxtNom.Text))
                    Entreprise.NOM = TxtNom.Text;
                if (!string.IsNullOrEmpty(TxtNom.Text))
                    Entreprise.NOM = TxtNom.Text;
                if (!string.IsNullOrEmpty(TxtPays.Text))
                    Entreprise.PAYS = TxtPays.Text;
                if (!string.IsNullOrEmpty(TxtSigle.Text))
                    Entreprise.SIGLE = TxtSigle.Text;
                if (!string.IsNullOrEmpty(TxtSiteWeb.Text))
                    Entreprise.SITEINTERNET = TxtSiteWeb.Text;
                if (!string.IsNullOrEmpty(TxtSlogan.Text))
                    Entreprise.SLOGAN = TxtSlogan.Text;
                if (!string.IsNullOrEmpty(TxtTelPrincipale.Text))
                    Entreprise.TELEPHONEPRINCIPAL = TxtTelPrincipale.Text;
                if (!string.IsNullOrEmpty(TxtTelSecondaire.Text))
                    Entreprise.TELEPHONESECONDAIRE = TxtTelSecondaire.Text;
                if (HypLinkLogo.Tag != null)
                    Entreprise.LOGO = (byte[])HypLinkLogo.Tag;
                Entreprise.DATECREATION = DateTime.Now;
                Entreprise.USERCREATION = UserConnecte.matricule;

                return Entreprise;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void HypLinkLogo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsLogoSelectionnee)
                {
                    var openDialog = new OpenFileDialog();
                    openDialog.Filter =
                        "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                    openDialog.FilterIndex = 1;
                    openDialog.Multiselect = false;
                    bool? userClickedOk = openDialog.ShowDialog();
                    if (userClickedOk == true)
                    {
                        if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                        {
                            FileStream stream = openDialog.File.OpenRead();
                            var memoryStream = new MemoryStream();
                            stream.CopyTo(memoryStream);
                            HypLinkLogo.Tag = memoryStream.GetBuffer();
                            //var formScanne = new Galatee.Silverlight.Shared.UcLogo(memoryStream, SessionObject.ExecMode.Creation);
                            //formScanne.Closed += new EventHandler(GetInformationFromChildWindowImage);
                            //formScanne.Show();
                        }
                    }
                }
                else
                {
                    var messBoxControl = new MessageBoxControl.MessageBoxChildWindow("Logo entreprise", "Voulez vous supprimer le logo ? ", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    messBoxControl.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messBoxControl.Result == MessageBoxResult.OK)
                        {
                            this.IsLogoSelectionnee = false;
                            this.HypLinkLogo.Content = "Insérer le logo";
                            this.HypLinkLogo.Tag = null;
                        }
                        else
                        {
                            if (HypLinkLogo.Tag != null)
                            {
                                MemoryStream memoryStream = new MemoryStream(HypLinkLogo.Tag as byte[]);
                                //var uclogo = new Galatee.Silverlight.Shared.UcLogo(memoryStream, SessionObject.ExecMode.Modification);
                                //uclogo.Show();
                            }
                        }
                    };
                    messBoxControl.Show();
                }
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetInformationFromChildWindowImage(object sender, EventArgs e)
        {
            try
            {
                //var form = (Galatee.Silverlight.Shared.UcLogo)sender;
                //if (form != null)
                //{
                //    if (form.DialogResult == true)
                //    {
                //        this.HypLinkLogo.Content = "Logo inséré";
                //        this.IsLogoSelectionnee = true;
                //    }
                //    else
                //        this.IsLogoSelectionnee = false;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


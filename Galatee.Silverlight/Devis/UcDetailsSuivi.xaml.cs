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
using Galatee.Silverlight.ServiceAccueil ;

namespace Galatee.Silverlight.Devis
{
    public partial class UcDetailsSuivi : ChildWindow
    {
        private ObjSUIVIDEVIS SuiviDevisSelectionne = null;
        private ObjDEVIS DevisSelectionne = null;
        private SessionObject.ExecMode ModeExecution;
        public UcDetailsSuivi()
        {
            InitializeComponent();
        }
        public UcDetailsSuivi(ObjSUIVIDEVIS pSuiviDevis,ObjDEVIS pDevis, SessionObject.ExecMode pExecMode)
        {
            InitializeComponent();
            DevisSelectionne = pDevis;
            ModeExecution = pExecMode;
            SuiviDevisSelectionne = pSuiviDevis;
        }
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ModeExecution != SessionObject.ExecMode.Consultation)
                {

                    LayoutRoot.Cursor = Cursors.Wait;
                    DateTime date, currentDate;
                    currentDate = DateTime.Now.Date;
                    TimeSpan difference;
                    double delai;

                    date = (DateTime)this.DevisSelectionne.DATEETAPE;
                    difference = (currentDate.Date - date.Date);
                    delai = difference.TotalDays;

                    this.SuiviDevisSelectionne.MATRICULEAGENT = UserConnecte.matricule;
                    this.SuiviDevisSelectionne.FK_IDETAPE = (int)this.DevisSelectionne.FK_IDETAPEDEVIS;
                    if (this.Txt_Commentaire.Text.StartsWith("*"))
                        this.SuiviDevisSelectionne.COMMENTAIRE = this.Txt_Commentaire.Text;
                    else
                        this.SuiviDevisSelectionne.COMMENTAIRE = "* " + this.Txt_Commentaire.Text;
                    this.SuiviDevisSelectionne.DUREE = Convert.ToInt32(delai);

                    //DevisServiceClient client = new DevisServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Devis"));
                    //client.UpdateSuiviDevisCompleted += (ssender, args) =>
                    //{
                    //    if (args.Cancelled || args.Error != null)
                    //    {
                    //        LayoutRoot.Cursor = Cursors.Arrow;
                    //        string error = args.Error.Message;
                    //        Message.Show(error, Silverlight.Resources.Parametrage.Languages.Centre);
                    //        return;
                    //    }
                    //    if (!args.Result)
                    //    {
                    //        LayoutRoot.Cursor = Cursors.Arrow;
                    //        Message.Show("Erreur lors de la mise à jour", Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                    //        return;
                    //    }
                    //    LayoutRoot.Cursor = Cursors.Arrow;
                    //    this.DialogResult = true;
                    //};
                    //client.UpdateSuiviDevisAsync(SuiviDevisSelectionne);
                }
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Txt_Commentaire_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                this.OKButton.IsEnabled = ((ModeExecution == SessionObject.ExecMode.Consultation) ||
                  (this.Txt_Commentaire.Text != string.Empty));
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                this.OKButton.IsEnabled = ((ModeExecution == SessionObject.ExecMode.Consultation) ||
                      (this.Txt_Commentaire.Text != string.Empty));

                this.Txt_NumeroDevis.Text = !string.IsNullOrEmpty(SuiviDevisSelectionne.NUMDEM) ? SuiviDevisSelectionne.NUMDEM : string.Empty;
                this.Txt_Commentaire.Text = !string.IsNullOrEmpty(SuiviDevisSelectionne.COMMENTAIRE) ? SuiviDevisSelectionne.COMMENTAIRE : string.Empty;

                if (!string.IsNullOrEmpty(this.SuiviDevisSelectionne.MATRICULEAGENT))
                    this.Txt_Agent.Text = this.SuiviDevisSelectionne.MATRICULEAGENT;
                this.Txt_TypeDevis.Text = !string.IsNullOrEmpty(DevisSelectionne.LIBELLETYPEDEVIS) ? DevisSelectionne.LIBELLETYPEDEVIS : string.Empty;
                if (DevisSelectionne.ISFOURNITURE !=null && (bool)this.DevisSelectionne.ISFOURNITURE)
                    this.Txt_TypeDevis.Text += " simplifié";
                if (DevisSelectionne.ISPOSE != null && (bool)this.DevisSelectionne.ISPOSE)
                    this.Txt_TypeDevis.Text += " complété";
                if (ModeExecution != SessionObject.ExecMode.Consultation)
                    this.Txt_Commentaire.Focus();
                else
                    this.OKButton.Focus();
                Title = "Etape : " + DevisSelectionne.LIBELLETACHE;
                LayoutRoot.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
    }
}


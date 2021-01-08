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
    public partial class UcControle : ChildWindow
    {
        ObjMATRICULE agentChef = new ObjMATRICULE();
        private ObjDEVIS myDevis = new ObjDEVIS();
        //private CsControleTravaux NewControl = new CsControleTravaux();
        private ObjTRAVAUXDEVIS travail = new ObjTRAVAUXDEVIS();
        public bool Monchoix { get; set; }
        public List<ObjELEMENTDEVIS> Elements { get; set; }
        public ObjTRAVAUXDEVIS Travaux { get; set; }

 
        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = new CsDemande();

        public UcControle()
        {
            InitializeComponent();
        }

        public UcControle(ObjDEVIS pDevis)
        {
            InitializeComponent();
            myDevis = pDevis;
        }
        public UcControle(int idDevis)
        {
            InitializeComponent();
            ChargeDetailDEvis(idDevis);
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.GetDevisByNumIdDevisCompleted += (ssender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    string error = args.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                if (args.Result == null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                else
                {
                    laDetailDemande = args.Result;
                    laDemandeSelect = laDetailDemande.LaDemande;
                    InitControle(laDetailDemande);
                }
            };
            client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
        }
        void InitControle(CsDemande laDemande)
        {
            if (laDemande.TravauxDevis != null)
            {
                this.TxtDatePrevisionnelleTravaux.Text = laDemande.TravauxDevis.DATEPREVISIONNELLE.ToShortDateString();
                this.TxtDateDebutTravaux.Text = ((DateTime)(laDemande.TravauxDevis.DATEDEBUTTRVX)).ToShortDateString();
                this.TxtDateFinTravaux.Text = laDemande.TravauxDevis.DATEFINTRVX.Value.ToShortDateString();
                this.Txt_NumeroDevis.Text = laDemande.LaDemande.NUMDEM;
                this.TxtOrdre .Text = laDemande.LaDemande.ORDRE ;
            }
            TxtMethodeDeControle.Focus();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EnregisterDemande(false);
        }
        private void EnregisterDemande( bool isTransmettre)
        {
            try
            {
                if (ChkMaterielConsomme.IsChecked == false)
                    throw new Exception("Veuillez renseigner le matériel consommé");
                if (NumUpDownNoteEvaluation.Value == 0)
                    throw new Exception("Veuillez attribuer une note allant de 0 à 5");
                if (NumUpDownNoteEvaluation.Value > 5)
                    throw new Exception("Vous ne pouvez pas attribuer une note supérieur à 5");

                CsControleTravaux NewControl = new CsControleTravaux();
                NewControl.FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID;
                NewControl.FK_IDMATRICULE = UserConnecte.PK_ID;
                NewControl.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
                NewControl.ORDRE = int.Parse(laDetailDemande.LaDemande.ORDRE);
                NewControl.METMOYCONTROLE = TxtMethodeDeControle.Text;
                NewControl.DEGRADATIONVOIE = TxtDegradationVoiePublic.Text;
                NewControl.VOLUMETERTRVX = TxtVolumeTravauxTerassement.Text;
                NewControl.DATECONTROLE = Convert.ToDateTime(DtpDateControle.SelectedDate.Value);
                NewControl.NOTE = (int)NumUpDownNoteEvaluation.Value;
                NewControl.USERMODIFICATION = UserConnecte.matricule;
                NewControl.MATRICULECHEFEQUIPE = UserConnecte.matricule;
                NewControl.NOMCHEFEQUIPE = UserConnecte.nomUtilisateur;
                NewControl.DATEMODIFICATION = DateTime.Now;
                NewControl.USERCREATION = UserConnecte.matricule;
                NewControl.DATECREATION = DateTime.Now;
                NewControl.FK_IDTYPECONTROLE = 1;


                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.InsertControlCompleted += (ss, back) =>
                {
                    if (back.Cancelled || back.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = back.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (!back.Result)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(Silverlight.Resources.Devis.Languages.msgErreurMiseAJourDevis, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    else
                    {
                        if (isTransmettre)
                        {
                            List<string> codes = new List<string>();
                            codes.Add(laDetailDemande.InfoDemande.CODE);
                            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, false,this );

                          
                            if (laDetailDemande.InfoDemande != null && laDetailDemande.InfoDemande.CODE != null)
                                Shared.ClasseMEthodeGenerique.NotifierMailDemande(laDetailDemande.InfoDemande.UtilisateurEtapeSuivante , "0001", laDetailDemande.LaDemande.NUMDEM, laDetailDemande.LaDemande.LIBELLETYPEDEMANDE);
                        }
                        LayoutRoot.Cursor = Cursors.Arrow;
                        this.DialogResult = true;
                    }
                };
                service.InsertControlAsync(NewControl);

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    this.Txt_NumeroDevis.Text = this.myDevis.NUMDEVIS;
            //    this.TxtOrdre.Text = this.myDevis.ORDRE.ToString();
            //    this.DtpDateControle.SelectedDate = System.DateTime.Today;
            //    DevisServiceClient client = new DevisServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Devis"));
            //    client.SelectTravauxCompleted += (ssender, args) =>
            //    {
            //        if (args.Cancelled || args.Error != null)
            //        {
            //            string error = args.Error.Message;
            //            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
            //            return;
            //        }
            //        if (args.Result == null)
            //        {
            //            Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
            //            return;
            //        }
            //        else
            //        {
            //            try
            //            {
            //                travail = args.Result;
            //                if (travail != null)
            //                {
            //                    this.TxtDatePrevisionnelleTravaux.Text = travail.DATEPREVISIONNELLE.ToShortDateString();
            //                    this.TxtDateDebutTravaux.Text = ((DateTime)(travail.DATEDEBUTTRVX)).ToShortDateString();
            //                    this.TxtDateFinTravaux.Text = travail.DATEFINTRVX.Value.ToShortDateString();
            //                }
            //                TxtMethodeDeControle.Focus();
            //            }
            //            catch (Exception ex)
            //            {
            //                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            //            }
            //        }
            //    };
            //    client.SelectTravauxAsync(myDevis.PK_ID, (byte)myDevis.ORDRE, false);
            //    this.OKButton.IsEnabled = ((this.NumUpDownNoteEvaluation.Value.ToString() != string.Empty) && (this.DtpDateControle.SelectedDate != null));
            //}
            //catch (Exception ex)
            //{
            //    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            //}
        }

        private void ChkMaterielConsomme_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(ChkMaterielConsomme.IsChecked))
                {
                    UcMaterielsConsomme frmMaterielsConsomme = new UcMaterielsConsomme(laDetailDemande);
                    frmMaterielsConsomme.Closed += new EventHandler(frmMaterielsConsomme_Closed);
                    frmMaterielsConsomme.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void frmMaterielsConsomme_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (UcMaterielsConsomme)sender;
                if (form != null)
                {
                    if (form.DialogResult == true)
                        this.Elements = form.Elements;
                    else
                        this.ChkMaterielConsomme.IsChecked = false;
                    Monchoix = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void VerifierSaisie()
        {
            this.OKButton.IsEnabled = ((this.NumUpDownNoteEvaluation.Value != 0) && (DtpDateControle.DisplayDate!= null));
        }

        private void DtpDateControle_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void NumUpDownNoteEvaluation_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void RejeterButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
        }

        private void btn_transmetre_Click(object sender, RoutedEventArgs e)
        {
            EnregisterDemande(true);
        }
    }
}


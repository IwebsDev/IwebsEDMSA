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
    public partial class UcDecisionControle : ChildWindow
    {
        ObjMATRICULE agentChef = new ObjMATRICULE();
        private ObjDEVIS myDevis = new ObjDEVIS();
        private CsControleTravaux NewControl = new CsControleTravaux();
        private ObjTRAVAUXDEVIS travail = new ObjTRAVAUXDEVIS();
        public bool Monchoix { get; set; }
        public ObjTRAVAUXDEVIS Travaux { get; set; }
        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = new CsDemande();
        private List<CsTdem> _listeDesTypeDevisExistant = new List<CsTdem>();

        public UcDecisionControle()
        {
            InitializeComponent();
        }

        public UcDecisionControle(ObjDEVIS pDevis)
        {
            InitializeComponent();
            myDevis = pDevis;
        }
        public UcDecisionControle(int pIdDevis)
        {
            InitializeComponent();
            RemplirListeDesTypeDevisExistant();
            ChargeDetailDEvis(pIdDevis);
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
                    LoadInfoScreen(laDetailDemande);
                }
            };
            client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
        }
        private void RemplirListeDesTypeDevisExistant()
        {
            try
            {
                LayoutRoot.Cursor = Cursors.Wait;
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GetAllTypeDemandeCompleted += (ssender, args) =>
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

                        Message.ShowError("Aucun type de demande retourné,veuillez vous assurer que la base est correctement renseigné", Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    _listeDesTypeDevisExistant = args.Result.OrderBy(t => t.LIBELLE).ToList();
                    if (laDemandeSelect != null)
                    {
                        TxtTypeDevis.Text = _listeDesTypeDevisExistant.FirstOrDefault(t => t.PK_ID == laDemandeSelect.FK_IDTYPEDEMANDE).LIBELLE;
                        TxtTypeDevis.Tag = _listeDesTypeDevisExistant.FirstOrDefault(t => t.PK_ID == laDemandeSelect.FK_IDTYPEDEMANDE);
                    }
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.GetAllTypeDemandeAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadInfoScreen(CsDemande laDetailDemande)
        {
            try
            {
                Txt_NumDevis.Text = laDetailDemande.LaDemande.NUMDEM;
                TxtOrdreDevis.Text = laDetailDemande.LaDemande.ORDRE;
                if (_listeDesTypeDevisExistant != null && _listeDesTypeDevisExistant.Count>0)
                {
                    TxtTypeDevis.Text = _listeDesTypeDevisExistant.FirstOrDefault(t => t.PK_ID == laDetailDemande.LaDemande.FK_IDTYPEDEMANDE).LIBELLE;
                    TxtTypeDevis.Tag = _listeDesTypeDevisExistant.FirstOrDefault(t => t.PK_ID == laDetailDemande.LaDemande.FK_IDTYPEDEMANDE);
                }



                LayoutRoot.Cursor = Cursors.Wait;
                //this.Txt_NumDevis.Text = this.laDemandeSelect.NUMDEVIS;
                // this.TxtTypeDevis.Text = laDemandeSelect.LIBELLETYPEDEVIS;
                // this.TxtOrdreDevis.Text = this.laDemandeSelect.ORDRE.ToString();
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.SelectTravauxCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.Show(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (args.Result == null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        Message.Show(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    else
                    {
                        try
                        {
                            travail = args.Result;
                            if (travail != null)
                                this.TxtProcesVerbal.Text =!string.IsNullOrWhiteSpace(this.travail.PROCESVERBAL)?this.travail.PROCESVERBAL:string.Empty;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        LayoutRoot.Cursor = Cursors.Arrow;
                    }
                };
                client.SelectTravauxAsync(laDemandeSelect.PK_ID, int.Parse(laDemandeSelect.ORDRE));




            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private void Search()
        {
            try
            {
                UserAgentPicker FormUserAgentPicker = new UserAgentPicker();
                FormUserAgentPicker.Closed += new EventHandler(FormUserAgentPicker_Closed);
                FormUserAgentPicker.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FormUserAgentPicker_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (UserAgentPicker)sender;
                if (form != null)
                {
                    if (form.DialogResult == true && form.AgentSelectionne != null)
                    {
                        var agent = form.AgentSelectionne;
                        if (agent != null)
                        {
                            this.TxtMatricule.Text = agent.MATRICULE ;
                            this.TxtNomAgent.Text = agent.LIBELLE;
                            this.laDetailDemande.LaDemande.ISCONTROLE = true;
                        }
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            EnregisterDemande(false);
        }
        private void RejeterButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);

        }
        private void btn_transmetre_Click(object sender, RoutedEventArgs e)
        {
            EnregisterDemande(true);
        }
        private void EnregisterDemande(bool Istransmettre)
        {

            try
            {
                if (Convert.ToBoolean(rdb_Controle.IsChecked))
                {
                    if (string.IsNullOrEmpty(TxtMatricule.Text))
                    {
                        var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Vous devez renseigner le chef d'equipe controle", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        messageBox.OnMessageBoxClosed += (_, result) =>
                        {
                            if (messageBox.Result == MessageBoxResult.OK)
                                this.Search();
                            else
                            {
                                return;
                            }
                        };
                        messageBox.Show();
                    }
                    else
                    {
                        LayoutRoot.Cursor = Cursors.Wait;
                        NewControl.NUMDEM = Txt_NumDevis.Text;
                        NewControl.ORDRE = int.Parse(laDemandeSelect.ORDRE);
                        NewControl.MATRICULECHEFEQUIPE = TxtMatricule.Text;
                        NewControl.NOMCHEFEQUIPE = TxtNomAgent.Text;
                        NewControl.USERCREATION = UserConnecte.matricule;
                        NewControl.DATECREATION = DateTime.Now;
                        NewControl.FK_IDDEMANDE = laDemandeSelect.PK_ID;
                        NewControl.FK_IDMATRICULE = UserConnecte.PK_ID;
                        NewControl.FK_IDTYPECONTROLE  = 1;

                        Monchoix = true;

                        AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                        client.InsertControlCompleted += (ssender, args) =>
                        {
                            if (args.Cancelled || args.Error != null)
                            {
                                LayoutRoot.Cursor = Cursors.Arrow;
                                string error = args.Error.Message;
                                Message.Show(error, Silverlight.Resources.Devis.Languages.txtDevis);
                                return;
                            }
                            if (!args.Result)
                            {
                                LayoutRoot.Cursor = Cursors.Arrow;
                                Message.Show(Silverlight.Resources.Devis.Languages.msgErreurMiseAJourDevis, Silverlight.Resources.Devis.Languages.txtDevis);
                                return;
                            }
                            else
                            {
                                try
                                {
                                    if (Istransmettre)
                                    {
                                        List<string> codes = new List<string>();
                                        codes.Add(laDetailDemande.InfoDemande.CODE);
                                        Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, false, this);

                                        List<CsUtilisateur> leUser = new List<CsUtilisateur>();
                                        if (laDetailDemande.InfoDemande != null && laDetailDemande.InfoDemande.CODE != null)
                                        {
                                            foreach (CsUtilisateur item in laDetailDemande.InfoDemande.UtilisateurEtapeSuivante)
                                                leUser.Add(item);
                                            Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", laDetailDemande.LaDemande.NUMDEM, laDetailDemande.LaDemande.LIBELLETYPEDEMANDE);
                                        }
                                    }
                                    LayoutRoot.Cursor = Cursors.Arrow;
                                    DialogResult = true;
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        };
                        client.InsertControlAsync(NewControl);
                    }
                }
                else
                {
                    List<string> codes = new List<string>();
                    codes.Add(laDetailDemande.InfoDemande.CODE);
                    Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, false, this);

                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur a la transmission de la demande", "Demande");
            }

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        //private void ChkControle_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (Convert.ToBoolean(ChkControle.IsChecked))
        //        {
        //            this.labAgentDeControle.Visibility = this.TxtMatricule.Visibility = this.TxtNomAgent.Visibility = System.Windows.Visibility.Visible;
        //            Search();
        //            if (string.IsNullOrEmpty(this.agentChef.MATRICULE))
        //                return;

        //            this.TxtMatricule.Text = this.agentChef.MATRICULE;
        //            this.TxtNomAgent.Text = this.agentChef.LIBELLE;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
        //    }
        //}

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void rdb_Controle_Checked(object sender, RoutedEventArgs e)
        {
            this.labAgentDeControle.Visibility = this.TxtMatricule.Visibility = this.TxtNomAgent.Visibility = System.Windows.Visibility.Visible;
            Search();
            if (string.IsNullOrEmpty(this.agentChef.MATRICULE))
                return;

            this.TxtMatricule.Text = this.agentChef.MATRICULE;
            this.TxtNomAgent.Text = this.agentChef.LIBELLE;
        }

        private void rdb_PasControle_Checked_1(object sender, RoutedEventArgs e)
        {
            laDetailDemande.LaDemande.ISCONTROLE = false;
        }



   
    }
}


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
using Galatee.Silverlight.ServiceScelles;
using Galatee.Silverlight.Resources.Scelles;
using Galatee.Silverlight.Tarification.Helper;


namespace Galatee.Silverlight.Scelles
{
    public partial class UcRetourScelle : ChildWindow
    {
        private CsRemiseScelles ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        public CsRemiseScelles ObjetSelectionne { get; set; }
        public CsTbLot ObjetSelectionneLot { get; set; }
        public CsRemiseScelleByAg ObjetSelectionneScelle { get; set; }
        List<CsRemiseScelleByAg> ListdesScelles = new List<CsRemiseScelleByAg>();
        //ObservableCollection<CsTbLot> DonnesDatagridLot = new ObservableCollection<CsTbLot>();
        //ObservableCollection<CsScelle> donnesDatagridScelle = new ObservableCollection<CsScelle>();
        ObservableCollection<CsRemiseScelleByAg> donnesDatagridRemise = new ObservableCollection<CsRemiseScelleByAg>();
        List<CsRemiseScelleByAg> ListSaisie = new List<CsRemiseScelleByAg>();
        List<CsTbLot> Listlot = new List<CsTbLot>();
        List<CsRetourScelles> listForInsertOrUpdate = new List<CsRetourScelles>();
        List<ServiceAccueil.CsUtilisateur> lstAllUser = new List<ServiceAccueil.CsUtilisateur>();
        List<ServiceAccueil.CsUtilisateur> lstCentreUser = new List<ServiceAccueil.CsUtilisateur>();


        public UcRetourScelle()
        {
            InitializeComponent();
            ChargeListeUser();
            ModeExecution = SessionObject.ExecMode.Creation;
        }
        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();

        protected virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion

        private void ChargeListeUser()
        {
            try
            {

                //Lancer la transaction de mise a jour en base
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeAllUserCompleted += (sr, res) =>
                {

                    if (res != null && res.Cancelled)
                        return;
                    lstAllUser = res.Result;
                    //if (lstAllUser != null && lstAllUser.Count != 0)
                    //{
                    //    lstCentreUser = lstAllUser.Where(c => c.CODEHIER == UserConnecte.Centre).ToList();

                    //    var Query = from l in lstCentreUser group new { l.LIBELLE } by new { l.PK_ID, l.LIBELLE } into groupConcours select new { groupConcours.Key.PK_ID, groupConcours };

                    //    List<ServiceAccueil.CsUtilisateur> lst = new List<ServiceAccueil.CsUtilisateur>();
                    //    foreach (var uneligne in Query)
                    //    {
                    //        ServiceAccueil.CsUtilisateur laligne = new ServiceAccueil.CsUtilisateur();
                    //        laligne.PK_ID = uneligne.groupConcours.Key.PK_ID;
                    //        laligne.LIBELLE = uneligne.groupConcours.Key.LIBELLE;
                    //        lst.Add(laligne);
                    //    }

                    //    cboLivreur.ItemsSource = lst;
                    //    cboLivreur.DisplayMemberPath = "LIBELLE";
                    //    cboLivreur.SelectedValuePath = "PK_ID";



                    //}
                    //else
                    //    Message.ShowError("Aucune données trouvées", Langue.lbl_Menu);


                };
                service1.RetourneListeAllUserAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        private void ListeScelleExistant(int PK_Agt)
        {
            try
            {
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneListScelleCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Quartier);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }

                    if (args.Result != null)
                    {
                        foreach (var item in args.Result.Where(x => x.Status_ID == SessionObject.Enumere.StatusScelleRemis))
                        {

                            donnesDatagridRemise.Add(item);
                        }
                        dgScelleNnUtilise.ItemsSource = donnesDatagridRemise;
                    }
                };
                client.RetourneListScelleAsync(PK_Agt);
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
        private int LastSelectedTab = 1;

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (donnesDatagridRemise != null && donnesDatagridRemise.Count > 0)
            {
                TabControl tab = sender as TabControl;
                if (this.LastSelectedTab == tab.SelectedIndex)
                {
                    dgScelleAbimer.ItemsSource = donnesDatagridRemise;
                }
                else
                    dgScelleNnUtilise.ItemsSource = donnesDatagridRemise;

            }
            //if (TbItScelleAbimer.IsSelected == true)
            //    dgScelleAbimer.ItemsSource = donnesDatagridRemise;
            //else
            //  dgScelleNnUtilise.ItemsSource = donnesDatagridRemise;
        }

        private void Charger_Click(object sender, RoutedEventArgs e)
        {
            if (TbItScelleNnUtls.IsSelected == true)
            {
                ObjetSelectionneScelle = dgScelleNnUtilise.SelectedItem as CsRemiseScelleByAg;
                if (TxtScelleNnutiliser.Text != "")
                {
                    int lot = int.Parse(TxtScelleNnutiliser.Text.Trim());
                    lot = lot + 1;
                    TxtScelleNnutiliser.Text = lot.ToString();

                }
                else TxtScelleNnutiliser.Text = "1";
                Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsRemiseScelleByAg>(dgScelleNnUtilise, dgScelle);

            }
            else
            {
                ObjetSelectionneScelle = dgScelleNnUtilise.SelectedItem as CsRemiseScelleByAg;
                if (ObjetSelectionneLot != null)
                {
                    if (txtNbrScelleAbimer.Text != "")
                    {
                        int lot = int.Parse(txtNbrScelleAbimer.Text.Trim());
                        lot = lot + 1;
                        txtNbrScelleAbimer.Text = lot.ToString();

                    }
                    else txtNbrScelleAbimer.Text = ObjetSelectionneLot.Nombre_scelles_reçu.ToString();
                }
                Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsRemiseScelleByAg>(dgScelleAbimer, dgScelleAbimerLst);
            }
        }

        private void Decharger_Click(object sender, RoutedEventArgs e)
        {
            if (TbItScelleNnUtls.IsSelected == true)
            {
                ObjetSelectionneScelle = dgScelle.SelectedItem as CsRemiseScelleByAg;
                if (TxtScelleNnutiliser.Text != "" && ObjetSelectionneScelle != null)
                {
                    int lot = int.Parse(TxtScelleNnutiliser.Text.Trim());
                    lot = lot - 1;
                    TxtScelleNnutiliser.Text = lot.ToString();
                }
                Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsRemiseScelleByAg>(dgScelle, dgScelleNnUtilise);

            }
            else
            {
                ObjetSelectionneScelle = dgScelleAbimerLst.SelectedItem as CsRemiseScelleByAg;
                if (txtNbrScelleAbimer.Text != "" && ObjetSelectionneLot != null)
                {
                    int lot = int.Parse(TxtScelleNnutiliser.Text.Trim());
                    lot = lot - 1;
                    TxtScelleNnutiliser.Text = lot.ToString();
                }
                Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsRemiseScelleByAg>(dgScelleAbimerLst, dgScelleAbimer);

            }

        }

        private void chargerTout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TbItScelleNnUtls.IsSelected == true)
                {
                    dgScelle.ItemsSource = null;
                    dgScelle.ItemsSource = donnesDatagridRemise; ;
                    dgScelleNnUtilise.ItemsSource = null;
                    //ObjetSelectionneScelle = dgScelleNnUtilise.SelectedItem as CsRemiseScelleByAg;
                    foreach (CsRemiseScelleByAg item in donnesDatagridRemise)
                    {
                        if (TxtScelleNnutiliser.Text != "")
                        {
                            int lot = int.Parse(TxtScelleNnutiliser.Text.Trim());
                            lot = lot + 1;
                            TxtScelleNnutiliser.Text = lot.ToString();

                        }
                        else TxtScelleNnutiliser.Text = "1";
                    }
                }
                else
                {
                    dgScelleAbimerLst.ItemsSource = null;
                    dgScelleAbimerLst.ItemsSource = donnesDatagridRemise;
                    dgScelleAbimer.ItemsSource = null;
                    foreach (CsRemiseScelleByAg item in donnesDatagridRemise)
                    {
                        if (txtNbrScelleAbimer.Text != "")
                        {
                            int lot = int.Parse(txtNbrScelleAbimer.Text.Trim());
                            lot = lot + 1;
                            txtNbrScelleAbimer.Text = lot.ToString();


                        }
                        else txtNbrScelleAbimer.Text = "0";
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void DechargerTout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListeScelle.SelectedIndex == 0)
                {
                    dgScelleNnUtilise.ItemsSource = null;
                    dgScelle.ItemsSource = null;
                    dgScelleNnUtilise.ItemsSource = donnesDatagridRemise;
                    TxtScelleNnutiliser.Text = "";

                }
                else
                {

                    dgScelleAbimer.ItemsSource = null;
                    dgScelleAbimerLst.ItemsSource = null;
                    dgScelleAbimer.ItemsSource = donnesDatagridRemise;
                    txtNbrScelleAbimer.Text = "";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void galatee_OkClickedbtn_SearchScelleAg(object sender, EventArgs e)
        {

            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsUtilisateur utilisateur = (ServiceAccueil.CsUtilisateur)ctrs.MyObject;
                this.txtAgt_M.Text = utilisateur.MATRICULE ;
                this.txtAgt_M.Tag = utilisateur.PK_ID;
                ListeScelleExistant(utilisateur.PK_ID);
            }

        }


        private void btn_SearchAgt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstAllUser != null && lstAllUser.Count() > 0)
                {
                    List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstAllUser.Where(c => c.CENTRE == UserConnecte.Centre).ToList());
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "MATRICULE", "LIBELLE", "");
                    ctr.Closed += new EventHandler(galatee_OkClickedbtn_SearchScelleAg);
                    ctr.Show();
                }
                else
                {
                    Message.ShowInformation("Plus de scellés disponible en stock veuillez vous approvisionner", "Information");
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




        private List<CsRetourScelles> GetInformationsFromScreen()
        {

            try
            {
                var listObjetForInsertOrUpdate = new List<CsRetourScelles>();

                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    if (TbItScelleNnUtls.IsSelected == true)
                    {
                        ListSaisie.Clear();
                        ListSaisie.AddRange((List<CsRemiseScelleByAg>)dgScelle.ItemsSource);
                        foreach (CsRemiseScelleByAg element in ListSaisie)
                        {

                            var Retours = new CsRetourScelles
                            {
                                CodeCentre = UserConnecte.FK_IDCENTRE,
                                Date_Retour = DateRetour.SelectedDate != null ? DateRetour.SelectedDate.Value : DateTime.Now,
                                Nbre_Scelles = (int?)int.Parse(TxtScelleNnutiliser.Text),
                                Id_Retour = Guid.NewGuid(),
                                Donneur_Mat = txtAgt_M.Tag.ToString(),
                                Receveur_Mat = UserConnecte.PK_ID.ToString(),
                                Id_Scelle = element.Id_Scelle,
                                Id_DetailRetour = Guid.NewGuid(),
                                Motif_Retour = 1,
                                Status_ID = 3

                            };
                            listObjetForInsertOrUpdate.Add(Retours);
                        }
                    }
                    if (TbItScelleAbimer.IsSelected == true)
                    {
                        ListSaisie.Clear();
                        ListSaisie.AddRange((List<CsRemiseScelleByAg>)dgScelleAbimerLst.ItemsSource);
                        foreach (CsRemiseScelleByAg element in ListSaisie)
                        {

                            var Retours = new CsRetourScelles
                            {
                                CodeCentre = UserConnecte.FK_IDCENTRE,
                                Date_Retour = DateTime.Now,
                                //  Date_Retour = DateRetour.SelectedDate != null ? DateRetour.SelectedDate.Value : DateTime.Now,
                                //Nbre_Scelles = (int?)int.Parse(TxtScelleNnutiliser.Text),
                                Id_Retour = Guid.NewGuid(),
                                Donneur_Mat = txtAgt_M.Tag.ToString(),
                                Receveur_Mat = UserConnecte.PK_ID.ToString(),
                                Id_Scelle = element.Id_Scelle,
                                Id_DetailRetour = Guid.NewGuid(),
                                Motif_Retour = 0,
                                Status_ID = 0
                            };
                            listObjetForInsertOrUpdate.Add(Retours);
                        }
                    }

                }

                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    //foreach (CsScelle element in ListSaisie)
                    //{

                    //    //ObjetSelectionnee.Id_LotMagasinGeneral = NumeroDebut + "_" + NumeroFin + "_" + Origine_ID.ToString();
                    //    ObjetSelectionnee.CodeCentre = UserConnecte.FK_IDCENTRE;
                    //    ObjetSelectionnee.Date_Remise = Dateremise.SelectedDate != null ? Dateremise.SelectedDate.Value : DateTime.Now;
                    //    ObjetSelectionnee.Motif_ID = ((CsMotifsScelle)CboMotifs.SelectedItem).Motif_ID;
                    //    ObjetSelectionnee.Nbre_Scelles = (int?)int.Parse(txtNombredeScelle.Text);
                    //    ObjetSelectionnee.Matricule_Receiver = (int)txtAgt_M.Tag;
                    //    ObjetSelectionnee.Matricule_User = UserConnecte.PK_ID;
                    //    ObjetSelectionnee.Lot_Id = element.lot_ID;
                    //    ObjetSelectionnee.Id_Scelle = element.Id_Scelle;
                    //    ObjetSelectionnee.Id_DetailRemise = Guid.NewGuid();

                    //    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);

                }


                return listObjetForInsertOrUpdate;

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
                return null;
            }



        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Commune, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {

                        listForInsertOrUpdate = GetInformationsFromScreen();
                        var service = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));

                        if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        {
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                            {

                                if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                                {
                                    service.InsertRetoursCompleted += (snder, insertR) =>
                                    {
                                        if (insertR.Cancelled ||
                                            insertR.Error != null)
                                        {
                                            Message.ShowError(insertR.Error.Message, Languages.Commune);
                                            return;
                                        }
                                        if (insertR.Result == 0)
                                        {
                                            Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Commune);
                                            return;
                                        }
                                        //UpdateParentList(listForInsertOrUpdate[0]);
                                        OnEvent(null);
                                        DialogResult = true;
                                    };
                                    service.InsertRetoursAsync(listForInsertOrUpdate);
                                }
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateRetourseCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.Commune);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Commune);
                                        return;
                                    }
                                    // UpdateRemise(listForInsertOrUpdate[0]);
                                    OnEvent(null);
                                    DialogResult = true;
                                };
                                service.UpdateRetourseAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.Commune);
            }
        }

        private void txtAgt_M_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.txtAgt_M.Text.Length == SessionObject.Enumere.TailleMatricule)
            {
                if (lstAllUser != null && lstAllUser.Count() > 0)
                {
                    ServiceAccueil.CsUtilisateur leuser = lstAllUser.FirstOrDefault(t => t.MATRICULE == this.txtAgt_M.Text);
                    if (leuser != null)
                    {
                        this.txt_LibelleAgentScelle.Text = leuser.LIBELLE;
                        txtAgt_M.Tag = leuser.PK_ID;
                        ListeScelleExistant(leuser.PK_ID);

                    }
                    else
                    {
                        Message.ShowInformation("Aucun utilisateur n'existe ", "Information");
                        this.txtAgt_M.Focus();
                    }
                }
            }
        }


    }
}


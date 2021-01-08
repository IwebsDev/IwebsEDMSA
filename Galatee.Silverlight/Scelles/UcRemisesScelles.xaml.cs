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
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.Resources.Scelles;
using Galatee.Silverlight.Tarification.Helper;

namespace Galatee.Silverlight.Scelles
{
    public partial class UcRemisesScelles : ChildWindow
    {
        private CsRemiseScelles ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        public CsRemiseScelles ObjetSelectionne { get; set; }
        public CsTbLot ObjetSelectionneLot { get; set; }
        public CsScelle ObjetSelectionneScelle { get; set; }
        List<CsScelle> ListdesScelles = new List<CsScelle>();
        List<CsTbLot> DonnesDatagridLot = new List<CsTbLot>();
        List<CsScelle> donnesDatagridScelle = new List<CsScelle>();
        List<CsRemiseScelles> donnesDatagridRemise = new List<CsRemiseScelles>();
        List<CsScelle> ListSaisie = new List<CsScelle>();
        List<CsTbLot> Listlot = new List<CsTbLot>();
        List<CsRemiseScelles> listForInsertOrUpdate = new List<CsRemiseScelles>();
        List<ServiceAccueil.CsUtilisateur> lstAllUser = new List<ServiceAccueil.CsUtilisateur>();
        List<ServiceAccueil.CsUtilisateur> lstCentreUser = new List<ServiceAccueil.CsUtilisateur>();
       

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

        public UcRemisesScelles()
        {
            InitializeComponent();
            RemplirListeCmbDeMotifsExistant();
            ChargeListeUser();
            //Rdb_RmScelle.IsChecked = true;
            ModeExecution = SessionObject.ExecMode.Creation;
            chb_SaisiNombreScelleSouhaite.Visibility = System.Windows.Visibility.Collapsed;
            txt_NombreScellesSouhaite.Visibility = System.Windows.Visibility.Collapsed;
          ///  Dateremise.PI = DateTime.Now;
              
        }

        private void InitControl()
        {
            InitializeComponent();
            RemplirListeCmbDeMotifsExistant();
            ChargeListeUser();
            //Rdb_RmScelle.IsChecked = true;
            ModeExecution = SessionObject.ExecMode.Creation;
            chb_SaisiNombreScelleSouhaite.Visibility = System.Windows.Visibility.Collapsed;
            txt_NombreScellesSouhaite.Visibility = System.Windows.Visibility.Collapsed;
            this.dgLotScelle.ItemsSource = null;
            this.dgScelle.ItemsSource = null;
            dgRemiselot.ItemsSource = null;
            dgRemis.ItemsSource = null;
            GetDatascelle();
            this.Rdb_RmScelle.IsChecked = true;
        }


        public UcRemisesScelles(CsRemiseScelles pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                //Translate();
                var Remise = new CsRemiseScelles();
                if (pObject != null)
                    ObjetSelectionnee = Utility.ParseObject(Remise, pObject as CsRemiseScelles);
                ModeExecution = pExecMode;
                dataGrid = pGrid;
                RemplirListeCmbDeMotifsExistant();
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                   // btn_ajout.IsEnabled = true;
                }
                chb_SaisiNombreScelleSouhaite.Visibility = System.Windows.Visibility.Collapsed;
                txt_NombreScellesSouhaite.Visibility = System.Windows.Visibility.Collapsed;
                //VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
            }
        }
        private void GetDataLot()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.RetourneLotDeScelleAffectationCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.LibelleReceptionScelle);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    DonnesDatagridLot.Clear();
                    if (args.Result != null)
                        foreach (var item in args.Result.Where(x => x.Status_lot_ID == 1))
                        {
                            //if(item.DateReception==DateTime.Now)
                            DonnesDatagridLot.Add(item);
                        }
                    dgLotScelle.ItemsSource = DonnesDatagridLot.OrderBy(t => t.lot_ID).OrderBy(x => x.Numero_depart).ToList();
                };
                client.RetourneLotDeScelleAffectationAsync(UserConnecte.FK_IDCENTRE );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDatascelle()
        {
            try
            {
                 
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.RetourneScellesListeByAgenceCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.LibelleReceptionScelle);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    donnesDatagridScelle.Clear();
                    var myresult=args.Result.Where(x => x.Status_ID == 3).Distinct().ToList();
                    //if (args.Result != null)
                    foreach (var item in myresult)
                            donnesDatagridScelle.Add(item);
                    dgScelle.ItemsSource = donnesDatagridScelle.OrderBy(t=>t.Numero_Scelle ).ToList();
                };
                client.RetourneScellesListeByAgenceAsync(UserConnecte.FK_IDCENTRE );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsRemiseScelles> GetInformationsFromScreen()
        {

            try
            {
                var listObjetForInsertOrUpdate = new List<CsRemiseScelles>();

                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation )
                {
                    if (Rdb_RmScelle.IsChecked == true)
                    {
                        ListSaisie.Clear();
                        ListSaisie.AddRange((List<CsScelle>)dgRemis.ItemsSource);
                        foreach (CsScelle element in ListSaisie)
                        {

                            var Remise = new CsRemiseScelles
                            {
                                CodeCentre = UserConnecte.FK_IDCENTRE,
                                Date_Remise = Dateremise.SelectedDate != null ?  Dateremise.SelectedDate.Value : DateTime.Now ,
                                Motif_ID = ((Galatee.Silverlight.ServiceScelles.CsMotifsScelle)CboMotifs.SelectedItem).Motif_ID,
                                Nbre_Scelles = (int?)int.Parse(txtNombredeScelle.Text),
                                Id_Remise = Guid.NewGuid(),
                                Matricule_Receiver = (int)txtAgt_M.Tag,
                                Matricule_User = UserConnecte.PK_ID,
                                Lot_Id = element.lot_ID.ToString(),
                                Id_Scelle = element.Id_Scelle,
                                Id_DetailRemise = Guid.NewGuid(),
                                TypeRemise = 0,

                            };
                            listObjetForInsertOrUpdate.Add(Remise);
                        }
                    }
                    if (Rdb_RmLotScelle.IsChecked == true)
                    {
                        Listlot.Clear();
                        Listlot.AddRange((List<CsTbLot>)dgRemiselot.ItemsSource);
                        foreach (CsTbLot element in Listlot)
                        {

                            var Remise = new CsRemiseScelles
                            {
                                CodeCentre = UserConnecte.FK_IDCENTRE,
                                Date_Remise = Dateremise.SelectedDate != null ? Dateremise.SelectedDate.Value : DateTime.Now,
                                Motif_ID = ((Galatee.Silverlight.ServiceScelles.CsMotifsScelle)CboMotifs.SelectedItem).Motif_ID,
                                Nbre_Scelles = (int?)int.Parse(txtNombredeScelle.Text),
                                Id_Remise = Guid.NewGuid(),
                                Matricule_Receiver = (int)txtAgt_M.Tag,
                                Matricule_User = UserConnecte.PK_ID,
                                Lot_Id = element.lot_ID,
                                Id_Scelle = Guid.Empty,
                                
                                TypeRemise = 0,

                            };
                            listObjetForInsertOrUpdate.Add(Remise);
                        }
                    }

                }
                
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                       foreach (CsScelle element in ListSaisie)
                        {
                         
                            //ObjetSelectionnee.Id_LotMagasinGeneral = NumeroDebut + "_" + NumeroFin + "_" + Origine_ID.ToString();
                        ObjetSelectionnee.CodeCentre=UserConnecte.FK_IDCENTRE;
                        ObjetSelectionnee.Date_Remise = Dateremise.SelectedDate != null ? Dateremise.SelectedDate.Value : DateTime.Now;
                        ObjetSelectionnee. Motif_ID = ((Galatee.Silverlight.ServiceScelles.CsMotifsScelle)CboMotifs.SelectedItem).Motif_ID;
                        ObjetSelectionnee. Nbre_Scelles= (int ?)int.Parse(txtNombredeScelle.Text);
                        ObjetSelectionnee. Matricule_Receiver = (int)txtAgt_M.Tag;
                        ObjetSelectionnee.Matricule_User = UserConnecte.PK_ID;
                        ObjetSelectionnee.Lot_Id=element.lot_ID;
                        ObjetSelectionnee.  Id_Scelle = element.Id_Scelle;
                        ObjetSelectionnee.Id_DetailRemise = Guid.NewGuid();
                        
                            listObjetForInsertOrUpdate.Add(ObjetSelectionnee);

                        }
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
                if (this.txtAgt_M.Tag == null)
                {
                    Message.ShowInformation("Sélèctionner l'agent récepteur", "Information");
                    return;
                }

                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Commune, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {

                        listForInsertOrUpdate = GetInformationsFromScreen();
                        Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                        if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        {
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                            {

                                if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                                {
                                    service.InsertRemiseCompleted += (snder, insertR) =>
                                    {
                                        if (insertR.Cancelled ||
                                            insertR.Error != null)
                                        {
                                            Message.ShowError(insertR.Error.Message, Languages.Commune);
                                            return;
                                        }
                                        if (insertR.Result==0)
                                        {
                                            Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Commune);
                                            return;
                                        }
                                        InitControl();
                                    };
                                    service.InsertRemiseAsync(listForInsertOrUpdate);
                                }
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

        private void RemplirListeCmbDeMotifsExistant()
        {
            try
            {
                Galatee.Silverlight.ServiceScelles.IScelleServiceClient client = new Galatee.Silverlight.ServiceScelles.IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.SelectAllMotisScelleCompleted += (ssender, args) =>
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
                    else
                    {

                        this.CboMotifs.ItemsSource = args.Result;
                        this.CboMotifs.DisplayMemberPath = "Motif_libelle";
                        this.CboMotifs.SelectedValuePath = "Motif_ID";
                        CboMotifs.SelectedItem = args.Result.First();

                        if (ObjetSelectionnee != null)
                        {
                            foreach (Galatee.Silverlight.ServiceScelles.CsMotifsScelle marqModl in CboMotifs.ItemsSource)
                            {
                                if (marqModl.Motif_ID == ObjetSelectionnee.Motif_ID)
                                {
                                    CboMotifs.SelectedItem = marqModl;
                                    break;
                                }
                            }
                        }
                    }
                };
                client.SelectAllMotisScelleAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

                };
                service1.RetourneListeAllUserAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }

        private void Rdb_RmScelle_Checked(object sender, RoutedEventArgs e)
        {
            if (Rdb_RmScelle.IsChecked == true)
            {
                dgLotScelle.Visibility = Visibility.Collapsed;
                dgRemiselot.Visibility = Visibility.Collapsed;
                dgRemis.Visibility = Visibility.Visible;
                dgScelle.Visibility = Visibility.Visible;
                GetDatascelle();
                txtNombredeScelle.IsEnabled = false;
                txtNombredeScelle.Text ="0";

                chb_SaisiNombreScelleSouhaite.Visibility = Visibility.Visible;
            }
        }

        private void Charger_Click(object sender, RoutedEventArgs e)
        {
            if (Rdb_RmScelle.IsChecked == true)
            {
                ObjetSelectionneScelle = dgScelle.SelectedItem as CsScelle;
                if (dgRemiselot.Visibility == System.Windows.Visibility.Visible )
                {
                    if (dgRemiselot.ItemsSource != null)
                        txtNombredeScelle.Text = ((List<CsTbLot> )dgRemiselot.ItemsSource).Count.ToString();
                    
                }
                else if (dgRemis.Visibility == System.Windows.Visibility.Visible)
                {
                    if (dgRemis.ItemsSource != null)
                        txtNombredeScelle.Text = ((List<CsScelle>)dgRemis.ItemsSource).Count.ToString();
                }
                if (chb_SaisiNombreScelleSouhaite.IsChecked==true)
                {
                    if (!string.IsNullOrWhiteSpace(  txt_NombreScellesSouhaite.Text))
	                {
		                //on recupere le nmbre selectionné ,soit x
                        int NombreDeScelleSouhaiter=0;
                        if (int.TryParse(txt_NombreScellesSouhaite.Text, out NombreDeScelleSouhaiter))
                        {
                            if (NombreDeScelleSouhaiter > 0)
                            {
                                var DataSourcedgScelle = ((List<CsScelle>)dgScelle.ItemsSource);
                                var DataSourcedgRemis = ((List<CsScelle>)dgRemis.ItemsSource);

                                var ElementAAffecter = DataSourcedgScelle.Take(NombreDeScelleSouhaiter);
                                var ListElementAAffecter = ElementAAffecter != null ? ElementAAffecter.ToList() : null;
                                ListElementAAffecter.AddRange(DataSourcedgRemis!=null?DataSourcedgRemis.ToList():new List<CsScelle>());

                                if (ListElementAAffecter!=null)
                                {

                                    dgRemis.ItemsSource = null;
                                    dgRemis.ItemsSource = ListElementAAffecter.OrderBy(c => c.NUM_SCELLE).ToList();

                                    var ElementRestantAAffecter = DataSourcedgScelle.Where(s => !ListElementAAffecter.Contains(s));
                                    dgScelle.ItemsSource = null;
                                    dgScelle.ItemsSource = ElementRestantAAffecter.OrderBy(c => c.NUM_SCELLE).ToList();
                                    return;
                                }
                                else
                                {
                                    Message.ShowWarning("Aucun éléments affecté", "Information");
                                    return;
                                }
                            }
                            else
                            {
                                Message.ShowWarning("Veuillez saisir une valeur supperieur à 0", "Information");
                                return;
                            }
                        }
                        else
                        {
                            Message.ShowWarning("Veuillez saisir une valeur numérique", "Information");
                            return;
                        }
	                }
                     else
	                {
                        Message.ShowWarning("Veuillez saisir le nombre de scelles souhaité","Information");
                        return;
	                }
                }
                Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsScelle>(dgScelle, dgRemis);
               
            }
            else
            {
                ObjetSelectionneLot = dgLotScelle.SelectedItem as CsTbLot;
                if (ObjetSelectionneLot != null)
                {
                    if (txtNombredeScelle.Text !="")
                    {
                        int lot = int.Parse(txtNombredeScelle.Text);
                        lot = (int)ObjetSelectionneLot.Nombre_scelles_reçu + lot;
                        txtNombredeScelle.Text = lot.ToString();

                    }
                    else txtNombredeScelle.Text = ObjetSelectionneLot.Nombre_scelles_reçu.ToString();
                }
                Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsTbLot>(dgLotScelle, dgRemiselot);
            }
        }

        private void Decharger_Click(object sender, RoutedEventArgs e)
        {
            if (Rdb_RmScelle.IsChecked == true)
            {
                ObjetSelectionneScelle = dgRemis.SelectedItem as CsScelle;
                if (txtNombredeScelle.Text != "" && ObjetSelectionneScelle!= null)
                {
                    int lot = int.Parse(txtNombredeScelle.Text.Trim());
                    lot = lot - 1;
                    txtNombredeScelle.Text = lot.ToString();

                }
                Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsScelle>(dgRemis, dgScelle);

            }
            else
            {
                ObjetSelectionneLot = dgRemiselot.SelectedItem as CsTbLot;
                if (txtNombredeScelle.Text != "" && ObjetSelectionneLot != null)
                {
                    int lot = int.Parse(txtNombredeScelle.Text);
                    int nomscelle = (int)ObjetSelectionneLot.Nombre_scelles_reçu;
                    lot = nomscelle - lot;
                    txtNombredeScelle.Text = lot.ToString();
                }
                Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<CsTbLot>(dgRemiselot, dgLotScelle);
             
            }

        }

        private void chargerTout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Rdb_RmScelle.IsChecked == true)
                {
                    dgRemis.ItemsSource = null;
                    dgRemis.ItemsSource = donnesDatagridScelle.OrderBy(c => c.NUM_SCELLE).ToList();
                    dgScelle.ItemsSource = null;
                    ObjetSelectionneScelle = dgLotScelle.SelectedItem as CsScelle;
                    foreach (CsScelle item in donnesDatagridScelle)
                    {
                        if (txtNombredeScelle.Text != "")
                        {
                            int lot = int.Parse(txtNombredeScelle.Text.Trim());
                            lot = lot + 1;
                            txtNombredeScelle.Text = lot.ToString();

                        }
                        else txtNombredeScelle.Text = "1";
                    }
                }
                else
                {
                    dgRemis.ItemsSource = null;
                    dgRemiselot.ItemsSource = DonnesDatagridLot.OrderBy(c => c.Numero_depart).ToList();
                    dgLotScelle.ItemsSource = null;
                    foreach (CsTbLot item in DonnesDatagridLot)
                    {
                        if (txtNombredeScelle.Text != "")
                        {
                            int lot = int.Parse(txtNombredeScelle.Text.Trim())!=0?int.Parse(txtNombredeScelle.Text.Trim()):0;
                            lot = (int)item.Nombre_scelles_reçu + lot;
                            txtNombredeScelle.Text = lot.ToString();

                        }
                        else txtNombredeScelle.Text ="0";
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
                if (Rdb_RmScelle.IsChecked == true)
                {
                    dgScelle.ItemsSource = null;
                    dgRemis.ItemsSource = null;
                    dgScelle.ItemsSource = donnesDatagridScelle.OrderBy(c => c.NUM_SCELLE).ToList();
                    txtNombredeScelle.Text = "";

                }
                else
                {

                    dgLotScelle.ItemsSource = null;
                    dgRemiselot.ItemsSource = null;
                    dgLotScelle.ItemsSource = DonnesDatagridLot.OrderBy(c => c.Numero_depart).ToList();
                    txtNombredeScelle.Text = "";
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

            }

        }


        private void btn_SearchAgt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                    if (lstAllUser != null && lstAllUser.Count() > 0)
                    {
                        List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstAllUser.Where(c=>c.CENTRE==UserConnecte.Centre).ToList());
                        Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "MATRICULE", "LIBELLE","");
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

        private void Rdb_RmLotScelle_Checked(object sender, RoutedEventArgs e)
        {
            if (Rdb_RmLotScelle.IsChecked == true)
            {
               
                dgLotScelle.Visibility = Visibility.Visible;
                dgScelle.Visibility = Visibility.Collapsed;
                dgRemis.Visibility = Visibility.Collapsed;
                dgRemiselot.Visibility = Visibility.Visible;
                GetDataLot();
                txtNombredeScelle.Text = string.Empty;


                chb_SaisiNombreScelleSouhaite.Visibility = Visibility.Collapsed;
                dgScelle.IsEnabled = true;
                
            }
        }


        //private void dgLotScelle_MouseRightButtonDown(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        ObjetSelectionneLot = dgLotScelle.SelectedItem as CsTbLot;
        //        if (txtNombredeScelle.Text != null)
        //        {
        //            int lot = int.Parse(txtNombredeScelle.Text.Trim());
        //            lot = (int)ObjetSelectionneLot.Nombre_scelles_reçu + lot;
        //            txtNombredeScelle.Text = lot.ToString();
                
        //        }
        //        else txtNombredeScelle.Text = ObjetSelectionneLot.Nombre_scelles_reçu.ToString();
        //        //SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsCentre;
        //        //SessionObject.gridUtilisateur = dtgrdParametre;
        //    }
        //    catch (Exception ex)
        //    {
        //        //var dialogResult = new DialogResult(ex.Message, Languages.LibelleNationalite, false, true, false);
        //        //dialogResult.Closed += new EventHandler(DialogResultOk);
        //        //dialogResult.Show();
        //        throw ex;

        //    }
        //}


        public bool verification()
        {
            if (txtAgt_M.Text != null && CboMotifs.SelectedItem != null)
                return true;
            else
                return false;
              
        
        
        }

        private void txtAgt_M_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.txtAgt_M.Text.Length == SessionObject.Enumere.TailleMatricule)
            {
                if (lstAllUser != null && lstAllUser.Count() > 0)
                {
                   ServiceAccueil.CsUtilisateur   leuser = lstAllUser.FirstOrDefault(t => t.MATRICULE == this.txtAgt_M.Text) ;
                   if (leuser != null)
                   {
                       this.txt_LibelleAgentScelle.Text = leuser.LIBELLE;
                       txtAgt_M.Tag = leuser.PK_ID;
                   }
                   else
                   {
                       Message.ShowInformation("Aucun utilisateur n'existe ", "Information");
                       this.txtAgt_M.Focus();
                   }
                }
            }
        }

        private void chb_SaisiNombreScelleSouhaite_Checked(object sender, RoutedEventArgs e)
        {
            txt_NombreScellesSouhaite.Visibility = Visibility.Visible;
            dgScelle.IsEnabled = false;
        }

        private void chb_SaisiNombreScelleSouhaite_Unchecked(object sender, RoutedEventArgs e)
        {
            txt_NombreScellesSouhaite.Visibility = Visibility.Collapsed;
            dgScelle.IsEnabled = true;

        }
    }
}


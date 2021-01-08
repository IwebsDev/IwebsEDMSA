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
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Galatee.Silverlight.Devis
{
    public partial class UcValidationEtudeMt : ChildWindow
    {
        List<ObjELEMENTDEVIS> lElements = new List<ObjELEMENTDEVIS>();
        ObjELEMENTDEVIS selectedElement = new ObjELEMENTDEVIS();
        ObjELEMENTDEVIS eltAdditional = new ObjELEMENTDEVIS();
        private SessionObject.ExecMode ModeExecution;

        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = null;
        decimal taux = (decimal)0;
        ObjDOCUMENTSCANNE doc = new ObjDOCUMENTSCANNE();
        ObjTYPEDEVIS typeDevis = new ObjTYPEDEVIS();
        public List<ObjELEMENTDEVIS> MyElements { get; set; }
        public decimal Frais { get; set; }
        public List<ObjFOURNITURE> MyFournitures { get; set; }
        public CsCtax Taxe { get; set; }

        List<Galatee.Silverlight.ServiceAccueil.CsCoutDemande> LstDesCoutsDemande = new List<Galatee.Silverlight.ServiceAccueil.CsCoutDemande>();


        private List<ObjELEMENTDEVIS > ListeFournitureExistante = null;
        private List<ObjELEMENTDEVIS > ListeAutreCoutFixe = null;
        public event PropertyChangedEventHandler PropertyChanged;
    
        List<ObjELEMENTDEVIS> LesElementInit = new List<ObjELEMENTDEVIS>();
        public UcValidationEtudeMt(int iddemande)
        {
            try
            {
                InitializeComponent();
                if (LayoutRoot != null)
                    LayoutRoot.Cursor = Cursors.Wait;
                ChargeDetailDEvis(iddemande);
            }
            catch (Exception ex)
            {
                if (LayoutRoot != null)
                    LayoutRoot.Cursor = Cursors.Arrow;
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        public UcValidationEtudeMt()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeAsync(IdDemandeDevis, string.Empty);
            client.ChargerDetailDemandeCompleted += (ssender, args) =>
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
                    LesElementInit = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(laDetailDemande.EltDevis);
                    RemplirListeDevis(laDetailDemande);
                    RenseignerInformationsDevis(laDetailDemande);
                    RenseignerInformationsDemandeDevis(laDetailDemande);
                    this.tabControl_Consultation.SelectedItem = tabItemFournitures;
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Enregistrer(laDetailDemande,true );
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Enregistrer(CsDemande laDemande,bool IsTransmetre)
        {
            try
            {
                this.MyElements = this.LireElements();
                if (this.MyElements.Count == 0)
                    throw new Exception(Languages.msgAddFournitures);

                laDemande.EltDevis = this.MyElements;
                laDemande.LstCanalistion = null;
                laDemande.Abonne = null;
                laDemande.LaDemande.ISPRESTATION = (this.Rdb_Prestation.IsChecked == true ? true : false);
                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.ValiderDemandeCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (IsTransmetre)
                    {
                        List<string> codes = new List<string>();
                        codes.Add(laDemande.InfoDemande.CODE);
                        Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
                        List<CsUtilisateur> leUser = new List<CsUtilisateur>();
                        if (laDemande.InfoDemande != null && laDemande.InfoDemande.CODE != null)
                        {
                            foreach (CsUtilisateur item in laDemande.InfoDemande.UtilisateurEtapeSuivante)
                                leUser.Add(item);
                            Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", laDemande.LaDemande.NUMDEM, laDemande.LaDemande.LIBELLETYPEDEMANDE);
                        }

                    }
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                clientDevis.ValiderDemandeAsync(laDemande);
                //montantTotal = !string.IsNullOrEmpty(Txt_MontantTotal.Text) ? decimal.Parse(Txt_MontantTotal.Text) : 0;
                //UcBilanEtablissementDevis frmBilanEtablissementDevis = new UcBilanEtablissementDevis(this.laDetailDemande, this.MyElements, montantTotal, true );
                //frmBilanEtablissementDevis.ExecMode = ModeExecution;
                //frmBilanEtablissementDevis.Taxe = this.Taxe;
                //frmBilanEtablissementDevis.Distance = (decimal)this.laDetailDemande.Branchement.LONGBRT;
                //frmBilanEtablissementDevis.Schema = doc;
                //frmBilanEtablissementDevis.Closed += new EventHandler(frmBilanEtablissementDevis_Closed);
                //frmBilanEtablissementDevis.Show();

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
        void frmBilanEtablissementDevis_Closed(object sender, EventArgs e)
        {
            try
            {
                DialogResult = true;
                return;
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

        private void Btn_Ajouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Ajouter();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Ajouter()
        {
            try
            {
                if (laDetailDemande.LaDemande.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                {
                    var MyLstFourniture = this.ListeFournitureExistante;
                    if (MyLstFourniture != null)
                    {
                        UcListeDesignationMT frm = new UcListeDesignationMT(this.ListeFournitureExistante, MyElements, laDetailDemande);
                        if (frm != null)
                        {
                            frm.Closed += new EventHandler(frmMt_Closed);
                            frm.Show();
                        }
                    }
                    else
                    {
                        Message.ShowInformation("Aucun élément de fourniture coreespondant", "Information");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void frmMt_Closed(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    var form = ((UcListeDesignationMT)sender);
                    if (form != null && form.DialogResult.Value)
                    {
                        if (MyElements == null)
                            MyElements = new List<ObjELEMENTDEVIS>();
                        RemplirListeMaterielMT(MyElements, SessionObject.LstRubriqueDevis);
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
        void frm_Closed(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    var form = ((UcListeDesignation)sender);
                    if (form != null && form.DialogResult.Value)
                    {
                        this.MyElements = form.MyElements;
                        lElements = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(MyElements);
                        this.dataGridElementDevis.ItemsSource = null;
                        this.dataGridElementDevis.ItemsSource = MyElements.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null && t.ISEXTENSION == true).ToList();
                        CalculerCout();

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

        //private List<ObjELEMENTDEVIS> LireElements()
        //{
        //    try
        //    {
        //        List<ObjELEMENTDEVIS> ListElementDevis = new List<ObjELEMENTDEVIS>();
        //        if (dataGridElementDevis.ItemsSource != null)
        //        {
        //            foreach (ObjELEMENTDEVIS elementDevis in dataGridElementDevis.ItemsSource)
        //            {
        //                elementDevis.NUMDEM  = laDetailDemande.LaDemande.NUMDEM ;
        //                elementDevis.USERMODIFICATION  = UserConnecte.matricule;
        //                elementDevis.DATECREATION = System.DateTime.Today;
        //                elementDevis.ORDRE = int.Parse(laDemandeSelect.ORDRE);
        //                elementDevis.ISPOSE = checkSelectedItem((CheckBox)this.dataGridElementDevis.Columns[7].GetCellContent(elementDevis) as CheckBox);
        //                elementDevis.ISPM  = checkSelectedItem((CheckBox)this.dataGridElementDevis.Columns[8].GetCellContent(elementDevis) as CheckBox);
        //                elementDevis.ISFOURNITURE = elementDevis.ISPOSE ? false : true ;
        //                ListElementDevis.Add(Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneCopyObjet<ObjELEMENTDEVIS>(elementDevis));
        //            }
        //            if (ListeAutreCoutFixe != null && ListeAutreCoutFixe.Count != 0)
        //                ListElementDevis.AddRange(ListeAutreCoutFixe);
        //        }
            
        //        return ListElementDevis;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private List<ObjELEMENTDEVIS> LireElements()
        {
            try
            {
                List<ObjELEMENTDEVIS> ListElementDevis = new List<ObjELEMENTDEVIS>();
                if (dataGridElementDevis.ItemsSource != null)
                {
                    List<ObjELEMENTDEVIS> lstObjetDevisValider =((List<ObjELEMENTDEVIS>)dataGridElementDevis.ItemsSource).Where(t => t.QUANTITE != null && t.QUANTITE != 0).ToList();
                    foreach (ObjELEMENTDEVIS elementDevis in lstObjetDevisValider)
                    {
                        elementDevis.USERCREATION = UserConnecte.matricule;
                        elementDevis.DATECREATION = System.DateTime.Today.Date;
                        elementDevis.ORDRE = int.Parse(laDemandeSelect.ORDRE);
                        elementDevis.FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID;
                        elementDevis.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
                        if (elementDevis.ISDEFAULT == false  )
                        {
                            elementDevis.ISPOSE = checkSelectedItem((CheckBox)this.dataGridElementDevis.Columns[7].GetCellContent(elementDevis) as CheckBox);
                            elementDevis.ISPM = checkSelectedItem((CheckBox)this.dataGridElementDevis.Columns[8].GetCellContent(elementDevis) as CheckBox);
                        }
                        elementDevis.ISFOURNITURE = elementDevis.ISPOSE ? false : true;
                        ListElementDevis.Add(elementDevis);
                    }
                }
                return ListElementDevis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Btn_Supprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Supprimer();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Supprimer()
        {
            try
            {
                List<ObjELEMENTDEVIS> ListeSelected = new List<ObjELEMENTDEVIS>();
                if (this.dataGridElementDevis.SelectedItems.Count > 0)
                {
                    var mBoxControl = new MessageBoxControl.MessageBoxChildWindow(this.Title.ToString(), Languages.msgConfirmSuppression, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    mBoxControl.OnMessageBoxClosed += (_, result) =>
                    {
                        if (mBoxControl.Result == MessageBoxResult.OK)
                        {
                            foreach (var item in dataGridElementDevis.ItemsSource)
                            {
                                var selected = item as ObjELEMENTDEVIS;
                                ListeSelected.Add(selected);
                            }
                            ObjELEMENTDEVIS select = dataGridElementDevis.SelectedItem as ObjELEMENTDEVIS;
                            ObjELEMENTDEVIS ObjSelect = ListeSelected.FirstOrDefault(t => t.FK_IDFOURNITURE == select.FK_IDFOURNITURE);
                            if (ObjSelect != null)
                                ListeSelected.Remove(ObjSelect);
                            this.dataGridElementDevis.ItemsSource = null;
                            this.dataGridElementDevis.ItemsSource = ListeSelected;

                            this.Txt_PrixUnitaire.Text = string.Empty;
                            this.Txt_Quantite.Text = string.Empty;
                        }
                        else
                        {
                            return;
                        }
                    };
                    mBoxControl.Show();
                }
                else
                    throw new Exception("Veuillez sélectionner un élément sil vous plaît !");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region "Gestion MenuContextuel"

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    Supprimer();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    Ajouter();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void MenuContextuel_Opened(object sender, RoutedEventArgs e)
        {
            try
            {
                //MenuContextuel.IsEnabled = dataGridElementDevis.SelectedItem != null;
                //MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        #endregion

        private void ChildWindow_Closed(object sender, EventArgs e)
        {

        }
        private void dataGridElementDevis_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                var dmdRow = e.Row.DataContext as ObjELEMENTDEVIS;
                if (dmdRow != null)
                {
                    if (dmdRow.QUANTITE == 0 || dmdRow.QUANTITE == null)
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Red);
                        e.Row.Foreground = SolidColorBrush;
                    }
                    else
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Black );
                        e.Row.Foreground = SolidColorBrush;
                        //DataGridCheckBoxColumn checkBoxColumn = new DataGridCheckBoxColumn();
                        //checkBoxColumn.Header = "Pose";
                        //checkBoxColumn.Binding = new Binding("ISPOSE");
                        //dataGridElementDevis.Columns.Add(checkBoxColumn);

                        ////DataGridCheckBoxColumn checkBoxColumn1 = new DataGridCheckBoxColumn();
                        ////checkBoxColumn1.Header = "PM";
                        ////checkBoxColumn1.Binding = new Binding("ISPM");
                        ////dataGridElementDevis.Columns.Add(checkBoxColumn1);
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        private void dataGridElementDevis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.ModeExecution != SessionObject.ExecMode.Consultation)
                {
                    //if (this.dataGridElementDevis.SelectedItems.Count == 1)
                    //{
                    //    ObjELEMENTDEVIS elt = (ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem;
                    //    this.selectedElement = elt;
                    //    this.Btn_Supprimer.IsEnabled = (selectedElement.ISDEFAULT != true) ? true : false;
                    //    this.Txt_Quantite.IsReadOnly = false;
                    //    this.Txt_PrixUnitaire.Text = selectedElement.PRIX.ToString(DataReferenceManager.FormatMontant);
                    //    this.Txt_Quantite.Text = selectedElement.QUANTITE.ToString();
                    //    this.Txt_Quantite.SelectAll();
                    //    this.Txt_Quantite.Focus();
                    //}
                    //else
                    //{
                    //    this.Txt_PrixUnitaire.IsReadOnly = true;
                    //    this.Txt_Quantite.IsReadOnly = true;
                    //    Txt_PrixUnitaire.Text = string.Empty;
                    //    Txt_Quantite.Text = string.Empty;
                    //}
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private int DecToInt(decimal montant)
        {
            char[] separateur = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray();
            string[] partie = montant.ToString().Split(separateur);
            return int.Parse(partie[0]);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        ObjELEMENTDEVIS _element = new ObjELEMENTDEVIS();
        private void RemplirListeDevis(CsDemande laDemandedevis)
        {
            try
            {
                if (laDemandedevis.Branchement.LONGBRT != null && laDemandedevis.Branchement.LONGBRT > 0)
                    this.Txt_Distance.Text = DecToInt((decimal)laDemandedevis.Branchement.LONGBRT).ToString();

                //Txt_NumDevis.Text = laDetailDemande.LaDemande.NUMDEM;
                //Txt_TypeDevis.Text = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                AcceuilServiceClient Serviceclient = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                Serviceclient.SelectAllMaterielCompleted += (ss, bc) =>
                {
                    try
                    {
                        if (bc.Cancelled || bc.Error != null)
                        {
                            string error = bc.Error.Message;
                            if (LayoutRoot != null)
                                LayoutRoot.Cursor = Cursors.Arrow;
                            Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (bc.Result != null)
                        {
                            ListeFournitureExistante = bc.Result;
                            if (laDemandedevis.EltDevis != null && laDetailDemande.EltDevis.Count != 0)
                            {
                                if (laDetailDemande.EltDevis.FirstOrDefault(t => t.ISFOURNITURE == true && t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 0) != null)
                                    this.Rdb_Fourniture.IsChecked = true;
                                else if (laDetailDemande.EltDevis.FirstOrDefault(t => t.ISPRESTATION == true && t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 0) != null)
                                    this.Rdb_Prestation.IsChecked = true;
                                
                                lElements = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(laDetailDemande.EltDevis);
                                RemplirListeMaterielMT(laDetailDemande.EltDevis, SessionObject.LstRubriqueDevis);
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (LayoutRoot != null)
                            LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(ex.Message, Languages.txtDevis);
                    }
                };
                Serviceclient.SelectAllMaterielAsync();
            }
            catch (Exception ex)
            {
                if (LayoutRoot != null)
                    LayoutRoot.Cursor = Cursors.Arrow;
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
        private void RemplirListeMaterielMT(List<ObjELEMENTDEVIS> lstEltDevis, List<CsRubriqueDevis> leRubriques)
        {
            ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
            leSeparateur.LIBELLE = "----------------------------------";
            leSeparateur.ISDEFAULT = true;
            List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();
            List<ObjELEMENTDEVIS> lstFourTVA = new List<ObjELEMENTDEVIS>();
            decimal CoutAvance =0;
            int CoperTrv = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
            if (SessionObject.LstDesCoutDemande.FirstOrDefault(t => t.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement && t.COPER == SessionObject.Enumere.CoperCAU && t.PRODUIT == laDetailDemande.LaDemande.PRODUIT ) != null)
                CoutAvance = SessionObject.LstDesCoutDemande.FirstOrDefault(t => t.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement && t.COPER == SessionObject.Enumere.CoperCAU && t.PRODUIT == laDetailDemande.LaDemande.PRODUIT).MONTANT.Value;
            bool ligneIsInf = false;
            foreach (CsRubriqueDevis item in leRubriques.Where(t => t.CODE != "004").ToList())
            {
                List<ObjELEMENTDEVIS> lstFourRubrique = lstEltDevis.Where(t => t.FK_IDRUBRIQUEDEVIS == item.PK_ID).ToList();
                if (lstFourRubrique != null && lstFourRubrique.Count != 0)
                {
                    lstFourRubrique.ForEach(t => t.FK_IDCOPER = CoperTrv);
                    if (item.CODE == SessionObject.Enumere.LIGNEHTA && laDetailDemande.Branchement.CODEBRT == "0001")
                    {


                        ObjELEMENTDEVIS leIncidence = ListeFournitureExistante.FirstOrDefault(t => t.ISGENERE == true);
                        leIncidence.FK_IDRUBRIQUEDEVIS = item.PK_ID;
                        leIncidence.QUANTITE = 1;
                        leIncidence.FK_IDCOPER = CoperTrv;
                        leIncidence.MONTANTTAXE = 0;
                        leIncidence.MONTANTHT = 0;
                        leIncidence.ISGENERE = true;
                        leIncidence.FK_IDMATERIELDEVIS = leIncidence.FK_IDMATERIELDEVIS;
                        leIncidence.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;
                        leIncidence.MONTANTHT = leIncidence.QUANTITE * (leIncidence.COUTUNITAIRE_FOURNITURE + leIncidence.COUTUNITAIRE_POSE);
                        leIncidence.MONTANTTTC = leIncidence.MONTANTHT;
                        decimal? MontantLigne = 0;
                        if (lstFourRubrique.FirstOrDefault(t => t.ISGENERE) == null)
                            if (lstEltDevis.FirstOrDefault(t => t.MONTANTHT < 0) == null)
                                lstFourRubrique.Add(leIncidence);
                        MontantLigne = lstFourRubrique.Sum(t => t.MONTANTHT);
                    }
                    decimal? MontantTotRubriqueHt = lstFourRubrique.Sum(t => t.MONTANTHT);
                    decimal? MontantTotRubriqueTaxe = lstFourRubrique.Sum(t => t.MONTANTTAXE);
                    decimal? MontantTotRubrique = lstFourRubrique.Sum(t => t.MONTANTTTC);
                    if (MontantTotRubriqueHt < 0)
                    { MontantTotRubriqueHt = 0; MontantTotRubriqueTaxe = 0; MontantTotRubrique = 0; ligneIsInf = true; }
                    ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                    leResultatBranchanchement.DESIGNATION  = "SOUS TOTAL  " + item.LIBELLE;
                    leResultatBranchanchement.ISGENERE = true;
                    //leResultatBranchanchement.IsCOLORIE = true;
                    leResultatBranchanchement.ISDEFAULT = true;
                    leResultatBranchanchement.MONTANTHT = MontantTotRubriqueHt;
                    leResultatBranchanchement.MONTANTTAXE = MontantTotRubriqueTaxe;
                    leResultatBranchanchement.MONTANTTTC = MontantTotRubrique;
                    lstFourTVA.Add(leResultatBranchanchement);

                    lstFourgenerale.AddRange(lstFourRubrique);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatBranchanchement);
                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        LIBELLE = "    ",
                        ISGENERE = true

                    });
                }
            }
            ObjELEMENTDEVIS leTHT = new ObjELEMENTDEVIS();
            ObjELEMENTDEVIS leTVA = new ObjELEMENTDEVIS();

            if (lstFourgenerale.Count != 0)
            {
                if (ligneIsInf) 
                {
                   ObjELEMENTDEVIS l = lstFourgenerale.FirstOrDefault(y=>y.MONTANTHT <0);
                    if (l != null )
                    {
                        l.MONTANTHT = 0;
                        l.MONTANTTTC  = 0;
                        l.MONTANT = 0;
                    }
                }
                decimal? MontantTotRubrique = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTTTC);
                decimal? MontantTotRubriqueHt = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTHT);
                decimal? MontantTotRubriqueTaxe = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTTAXE);


                //  decimal? MontantTotRubrique = !ligneIsInf?lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTTTC):lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0 && t.MONTANTTTC >0).Sum(t => t.MONTANTTTC);
                //decimal? MontantTotRubriqueHt = !ligneIsInf?lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTHT):lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0 && t.MONTANTHT >0).Sum(t => t.MONTANTHT);
                //decimal? MontantTotRubriqueTaxe = !ligneIsInf?lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTTAXE):lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0 && t.MONTANTTTC >0 ).Sum(t => t.MONTANTTTC);
                if (MontantTotRubriqueHt < 0)
                { MontantTotRubriqueHt = 0; MontantTotRubriqueTaxe = 0; }
                ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
                leResultatGeneral.DESIGNATION = "TOTAL FACTURE TRAVAUX ";
                leResultatGeneral.ISDEFAULT = true;
                leResultatGeneral.ISGENERE = true;
                leResultatGeneral.MONTANTHT = MontantTotRubriqueHt;
                lstFourgenerale.Add(leSeparateur);
                lstFourgenerale.Add(leResultatGeneral);


                ObjELEMENTDEVIS leSurveillance = new ObjELEMENTDEVIS();
                if (lstFourTVA != null && lstFourTVA.Count != 0)
                {
                    leSurveillance.DESIGNATION = "ETUDE ET SURVEILLANCE 10 %";
                    leSurveillance.ISFORTRENCH = true;
                    leSurveillance.QUANTITE = 1;
                    leSurveillance.ISGENERE = true;

                    leSurveillance.MONTANTHT = MontantTotRubriqueHt * (decimal)(0.10); ;
                    leSurveillance.MONTANTTAXE = MontantTotRubriqueTaxe * (decimal)(0.10); ;
                    leSurveillance.MONTANTTTC = MontantTotRubrique * (decimal)(0.10);

                    leSurveillance.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == "093").PK_ID;
                    leSurveillance.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;
                    lstFourgenerale.Add(leSurveillance);
                    lstFourTVA.Add(leSurveillance);

                }

                ObjELEMENTDEVIS lstFourEnsembleCmpt = lstEltDevis.FirstOrDefault(t => t.FK_IDRUBRIQUEDEVIS == 5);
                if (lstFourEnsembleCmpt != null && lstFourEnsembleCmpt.MONTANTHT != 0)
                {
                    ObjELEMENTDEVIS leResultatComptage = new ObjELEMENTDEVIS();
                    leResultatComptage.DESIGNATION = SessionObject.LstRubriqueDevis.FirstOrDefault(k => k.PK_ID == 5).LIBELLE;
                    leResultatComptage.ISDEFAULT = true;
                    //leResultatComptage.IsCOLORIE = true;
                    leResultatComptage.QUANTITE = lstEltDevis.Where(t => t.FK_IDRUBRIQUEDEVIS == 5).Count();
                    leResultatComptage.MONTANTTAXE = lstFourEnsembleCmpt.MONTANTTAXE != null ? lstFourEnsembleCmpt.MONTANTTAXE : 0;
                    leResultatComptage.MONTANTHT = lstFourEnsembleCmpt.MONTANTHT != null ? lstFourEnsembleCmpt.MONTANTHT : 0;
                    leResultatComptage.MONTANTTTC = leResultatComptage.MONTANTTAXE + leResultatComptage.MONTANTHT;
                    leResultatComptage.FK_IDRUBRIQUEDEVIS = 5;
                    leResultatComptage.FK_IDCOPER = CoperTrv;
                    leResultatComptage.FK_IDMATERIELDEVIS = lstFourEnsembleCmpt.FK_IDMATERIELDEVIS != null ? lstFourEnsembleCmpt.FK_IDMATERIELDEVIS : null;
                    leResultatComptage.FK_IDTAXE = lstFourEnsembleCmpt.FK_IDTAXE;

                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatComptage);
                    lstFourTVA.Add(leResultatComptage);

                }
                if (lstFourTVA != null && lstFourTVA.Count != 0)
                {
                    leTHT.DESIGNATION = "TOTAL HT ";
                    leTHT.ISFORTRENCH = true;
                    leTHT.ISGENERE = true;
                    leTHT.MONTANTHT = lstFourTVA.Sum(y => y.MONTANTHT);
                    lstFourgenerale.Add(leTHT);

                }
                if (lstFourTVA != null && lstFourTVA.Count != 0)
                {
                    leTVA.DESIGNATION = "TVA 18 % ";
                    leTVA.ISFORTRENCH = true;
                    leTVA.ISGENERE = true;
                    leTVA.MONTANTHT = lstFourTVA.Sum(y => y.MONTANTHT) * (decimal)(0.18); ;
                    lstFourgenerale.Add(leTVA);

                }
            }
            ObjELEMENTDEVIS leResultatGeneralaVANCE = new ObjELEMENTDEVIS();
            leResultatGeneralaVANCE.DESIGNATION = "Avance sur consommation ";
            //leResultatGeneralaVANCE.IsCOLORIE = true;
            leResultatGeneralaVANCE.ISDEFAULT = true;
            leResultatGeneralaVANCE.ISGENERE = true;
            leResultatGeneralaVANCE.QUANTITE = int.Parse(laDetailDemande.LaDemande.PUISSANCESOUSCRITE.ToString());
            leResultatGeneralaVANCE.MONTANTHT = laDetailDemande.LaDemande.PUISSANCESOUSCRITE * CoutAvance;
            leResultatGeneralaVANCE.MONTANTTTC = leResultatGeneralaVANCE.MONTANTHT;
            leResultatGeneralaVANCE.COUTUNITAIRE_FOURNITURE = CoutAvance;
            leResultatGeneralaVANCE.PRIX_UNITAIRE = CoutAvance;
            leResultatGeneralaVANCE.MONTANTTAXE = 0;
            CsCoutDemande leCoutAvance = LstDesCoutsDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperCAU);
            if (leCoutAvance != null)
                leResultatGeneralaVANCE.COUTFOURNITURE = leCoutAvance.MONTANT.Value;

            leResultatGeneralaVANCE.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperCAU).PK_ID;
            leResultatGeneralaVANCE.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;

            lstFourgenerale.Add(leSeparateur);
            lstFourgenerale.Add(leResultatGeneralaVANCE);

            ObjELEMENTDEVIS leResultatGeneralttc = new ObjELEMENTDEVIS();
            leResultatGeneralttc.DESIGNATION = "TOTAL GENERAL TTC ";
            leResultatGeneralttc.MONTANTHT = (leTHT.MONTANTHT == null ? 0 : leTHT.MONTANTHT) + (leTVA.MONTANTHT == null ? 0 : leTVA.MONTANTHT) + (leResultatGeneralaVANCE.MONTANTHT == null ? 0 : leResultatGeneralaVANCE.MONTANTHT);
            //leResultatGeneralttc.IsCOLORIE = true;
            leResultatGeneralttc.ISDEFAULT = true;
            leResultatGeneralttc.ISGENERE = true;
            lstFourgenerale.Add(leResultatGeneralttc);

            if (MyElements == null) MyElements = new List<ObjELEMENTDEVIS>();
            if (MyElements.Count != 0) MyElements.Clear();

            this.MyElements.AddRange(lstFourgenerale.Where(t => t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 198).ToList());
            this.dataGridElementDevis.ItemsSource = null;
            this.dataGridElementDevis.ItemsSource = lstFourgenerale.ToList();

            this.Txt_MontantTotalTTC.Text = leResultatGeneralttc.MONTANTHT.Value.ToString(SessionObject.FormatMontant);
            this.Txt_MontantTotalHT.Text = leTHT.MONTANTHT.Value.ToString(SessionObject.FormatMontant);
            this.Txt_MontantTotalTaxe.Text = leTVA.MONTANTHT.Value.ToString(SessionObject.FormatMontant);
            
        }
        private void RenseignerInformationsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                    ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == laDemande.LaDemande.FK_IDCENTRE);
                    Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE.ToString()) ? laDemande.LaDemande.ORDRE.ToString() : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLECENTRE) ? laDemande.LaDemande.LIBELLECENTRE : string.Empty;
                    Txt_LibelleSite.Text = !string.IsNullOrEmpty(leCentre.LIBELLESITE) ? leCentre.LIBELLESITE : string.Empty;
                    Txt_LibelleProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLEPRODUIT) ? laDemande.LaDemande.LIBELLEPRODUIT : string.Empty;
                    Txt_Client.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CLIENT) ? laDemande.LaDemande.CLIENT : string.Empty;
                    Txt_LibelleTypeDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLETYPEDEMANDE) ? laDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;
                    txtPropriete.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;
                    this.Txt_EtapeCourante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Title = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Txt_EtapeSuivante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_SUIVANTE) ? laDemande.InfoDemande.ETAPE_SUIVANTE : string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsDemandeDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.LeClient != null && laDemande.Ag != null)
                {
                    Txt_NomClient.Text = !string.IsNullOrEmpty(laDemande.LeClient.NOMABON) ? laDemande.LeClient.NOMABON : string.Empty;
                    txt_Quartier.Text = !string.IsNullOrEmpty(laDemande.Ag.QUARTIER) ? laDemande.Ag.QUARTIER : string.Empty;
                    txt_NumRue.Text = !string.IsNullOrEmpty(laDemande.Ag.RUE) ? laDemande.Ag.RUE : string.Empty;
                    txtAdresse.Text = !string.IsNullOrEmpty(laDemande.Branchement.ADRESSERESEAU) ? laDemande.Branchement.ADRESSERESEAU : string.Empty;
                    txtNumeroPiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMEROPIECEIDENTITE) ? laDemande.LeClient.NUMEROPIECEIDENTITE : string.Empty;
                    txt_Telephone.Text = !string.IsNullOrEmpty(laDemande.LeClient.TELEPHONE) ? laDemande.LeClient.TELEPHONE : string.Empty;
                    txt_NumLot.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;
                    Txt_LibelleCommune.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLECOMMUNE) ? laDemande.Ag.LIBELLECOMMUNE : string.Empty;
                    Txt_LibelleQuartier.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLEQUARTIER) ? laDemande.Ag.LIBELLEQUARTIER : string.Empty;
                    Txt_LibelleRue.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLERUE) ? laDemande.Ag.LIBELLERUE : string.Empty;
                    Txt_LibelleCategorie.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLECATEGORIE) ? laDemande.LeClient.LIBELLECATEGORIE : string.Empty;
                    Txt_TypePiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLETYPEPIECE) ? laDemande.LeClient.LIBELLETYPEPIECE : string.Empty;
                    Txt_LibelleTournee.Text = !string.IsNullOrEmpty(laDemande.Ag.TOURNEE) ? laDemande.Ag.TOURNEE : string.Empty;


                    AfficherOuMasquer(tabItemDemandeur, true);
                }
                else
                    AfficherOuMasquer(tabItemDemandeur, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AfficherOuMasquer(TabItem pTabItem, bool pValue)
        {
            try
            {
                pTabItem.Visibility = pValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private decimal CalculerCoutTotalExtension()
        {
            decimal MontantTotal = 0;
            try
            {
                foreach (ObjELEMENTDEVIS item in dataGridElementDevis.ItemsSource)
                {
                    MontantTotal = MontantTotal + item.MONTANTTTC.Value;
                }
                return MontantTotal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private decimal CalculerCoutTotalGeneral()
        {
            decimal MontantTotal = 0;
            try
            {
                foreach (ObjELEMENTDEVIS item in dataGridElementDevis.ItemsSource)
                {
                    MontantTotal = MontantTotal + item.MONTANTTTC.Value;
                }
                return MontantTotal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Txt_Quantite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if ((!string.IsNullOrEmpty(this.Txt_Quantite.Text)) && (int.Parse(this.Txt_Quantite.Text) > 0))
                {
                    var selectedElement = (ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem;
                    this.selectedElement = selectedElement;
                    if (selectedElement != null)
                    {
                        this.selectedElement.QUANTITE = int.Parse(this.Txt_Quantite.Text);
                        this.selectedElement.COUT = (decimal)this.selectedElement.QUANTITE * this.selectedElement.PRIX;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Txt_Quantite_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((!string.IsNullOrEmpty(this.Txt_Quantite.Text)) && (int.Parse(this.Txt_Quantite.Text) > 0))
                {
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
        private void IsEnPM(ObjELEMENTDEVIS leElt, bool IsChecked)
        {
            try
            {
                List<ObjELEMENTDEVIS> lstEltDevis = new List<ObjELEMENTDEVIS>();
                lstEltDevis = laDetailDemande.EltDevis;
                ObjELEMENTDEVIS leElts = lstEltDevis.FirstOrDefault(t => t.FK_IDMATERIELDEVIS == leElt.FK_IDMATERIELDEVIS);
                ObjELEMENTDEVIS leEltss = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(ListeFournitureExistante).FirstOrDefault(t => t.PK_ID == leElt.FK_IDMATERIELDEVIS);
                if (leElts != null)
                {
                    if (IsChecked)
                    {
                        leElts.PRIX_UNITAIRE = 0;
                        leElts.COUTUNITAIRE_FOURNITURE = 0;
                        leElts.COUTUNITAIRE_POSE = 0;
                    }
                    else
                    {
                        leElts.PRIX_UNITAIRE = leEltss.COUTUNITAIRE_FOURNITURE + leEltss.COUTUNITAIRE_POSE;
                        leElts.COUTUNITAIRE_FOURNITURE = leEltss.COUTUNITAIRE_FOURNITURE;
                        leElts.COUTUNITAIRE_POSE = leEltss.COUTUNITAIRE_POSE;
                    }
                    Galatee.Silverlight.ServiceAccueil.CsCtax tax = SessionObject.LstDesTaxe.FirstOrDefault(t => t.PK_ID == leElts.FK_IDTAXE);
                    if (tax != null)
                        taux = tax.TAUX;

                    leElts.MONTANTHT = (decimal)((leElts.QUANTITE * leElts.PRIX_UNITAIRE));
                    leElts.MONTANTTAXE = leElts.MONTANTHT * taux;
                    leElts.MONTANTTTC = leElts.MONTANTHT + leElts.MONTANTTAXE;

                    leElts.MONTANT = leElts.MONTANTHT.Value;
                    leElts.TAXE = leElts.MONTANTTAXE;
                    leElts.COUT = leElts.MONTANTTTC.Value;
                    RemplirListeMaterielMT(((List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).ToList(), SessionObject.LstRubriqueDevis);
                    if (IsChecked)
                        leElts.ISPM  = true;
                    else
                        leElts.ISPM = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void  CalculerCout()
        {
            try
            {
                if (dataGridElementDevis.ItemsSource != null)
                {
                    List<ObjELEMENTDEVIS> lstEltDevis = ((List<ObjELEMENTDEVIS>)dataGridElementDevis.ItemsSource).Where(i=>i.QUANTITE != 0 && i.QUANTITE != null).ToList();
                    //Txt_MontantTotalHT.Text = lstEltDevis.Sum(t => t.MONTANTHT.Value).ToString(SessionObject.FormatMontant);
                    //Txt_MontantTotalTaxe.Text = lstEltDevis.Sum(t => t.MONTANTTAXE.Value).ToString(SessionObject.FormatMontant);
                    //Txt_MontantTotalTTC.Text = lstEltDevis.Sum(t => t.MONTANTTTC.Value).ToString(SessionObject.FormatMontant);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void IsEnPose(ObjELEMENTDEVIS leElt,bool IsChecked )
        {
            try
            {
                List<ObjELEMENTDEVIS> lstEltDevis = new List<ObjELEMENTDEVIS>();
                lstEltDevis = laDetailDemande.EltDevis;
                ObjELEMENTDEVIS leElts = lstEltDevis.FirstOrDefault(t => t.FK_IDMATERIELDEVIS == leElt.FK_IDMATERIELDEVIS);
                ObjELEMENTDEVIS leEltss = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(ListeFournitureExistante).FirstOrDefault(t => t.PK_ID == leElt.FK_IDMATERIELDEVIS);
                if (leElts != null)
                {
                    if (IsChecked)
                    {
                        leElts.PRIX_UNITAIRE = leEltss.COUTUNITAIRE_POSE;
                        leElts.COUTUNITAIRE_FOURNITURE  = 0;
                        leElts.ISPOSE = true;
                    }
                    else
                    {
                        leElts.PRIX_UNITAIRE = leEltss.COUTUNITAIRE_FOURNITURE + leEltss.COUTUNITAIRE_POSE;
                        leElts.COUTUNITAIRE_FOURNITURE = leEltss.COUTUNITAIRE_FOURNITURE;
                        leElts.ISPOSE = false;
                    }
                    Galatee.Silverlight.ServiceAccueil.CsCtax tax = SessionObject.LstDesTaxe.FirstOrDefault(t => t.PK_ID == leElts.FK_IDTAXE);
                    if (tax != null)
                        taux = tax.TAUX;

                    leElts.MONTANTHT = (decimal)((leElts.QUANTITE * leElts.PRIX_UNITAIRE));
                    leElts.MONTANTTAXE = leElts.MONTANTHT * taux;
                    leElts.MONTANTTTC = leElts.MONTANTHT + leElts.MONTANTTAXE;

                    leElts.MONTANT = leElts.MONTANTHT.Value;
                    leElts.TAXE = leElts.MONTANTTAXE;
                    leElts.COUT = leElts.MONTANTTTC.Value;
                    RemplirListeMaterielMT(((List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).ToList(), SessionObject.LstRubriqueDevis);
                    if (IsChecked)
                        leElts.ISPOSE = true;
                    else
                        leElts.ISPOSE = false;
                    //CalculerCout();

                    //this.Txt_MontantTotalG.Text = CalculerCoutTotal().ToString(DataReferenceManager.FormatMontant);
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        private void IsEnFourniture(ObjELEMENTDEVIS leElt, bool IsChecked)
        {
            List<ObjELEMENTDEVIS> lstEltDevis = new List<ObjELEMENTDEVIS>();
            lstEltDevis = laDetailDemande.EltDevis;
            ObjELEMENTDEVIS leElts = lstEltDevis.FirstOrDefault(t => t.FK_IDMATERIELDEVIS  == leElt.FK_IDMATERIELDEVIS );
            ObjELEMENTDEVIS leEltss = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(ListeFournitureExistante).FirstOrDefault(t => t.PK_ID == leElt.FK_IDMATERIELDEVIS);
            if (leElts != null)
            {
                bool IsPoseCocher = checkSelectedItem((CheckBox)this.dataGridElementDevis.Columns[6].GetCellContent(dataGridElementDevis.SelectedItem as ObjELEMENTDEVIS) as CheckBox);
                if (IsChecked)
                {
                    if (IsPoseCocher)
                        leElts.PRIX_UNITAIRE = leEltss.COUTUNITAIRE_FOURNITURE + leEltss.COUTUNITAIRE_POSE;
                    else
                        leElts.PRIX_UNITAIRE = leEltss.COUTUNITAIRE_FOURNITURE;
                }
                else
                {
                    leElts.PRIX_UNITAIRE = leEltss.COUTUNITAIRE_POSE;
                    if (!IsPoseCocher)
                        checkerSelectedItem((CheckBox)this.dataGridElementDevis.Columns[6].GetCellContent(dataGridElementDevis.SelectedItem as ObjELEMENTDEVIS) as CheckBox);
                }

                Galatee.Silverlight.ServiceAccueil.CsCtax tax = SessionObject.LstDesTaxe.FirstOrDefault(t => t.PK_ID == leElts.FK_IDTAXE);
                if (tax != null)
                    taux = tax.TAUX;
                leElts.MONTANTHT = (decimal)((leElts.QUANTITE * leElts.PRIX_UNITAIRE));
                leElts.MONTANTTAXE = leElts.MONTANTHT * taux;
                leElts.MONTANTTTC = leElts.MONTANTHT + leElts.MONTANTTAXE;

                leElts.MONTANT = leElts.MONTANTHT.Value ;
                leElts.TAXE = leElts.MONTANTTAXE;
                leElts.COUT = leElts.MONTANTTTC.Value ;

                //this.Txt_MontantTotalG.Text = CalculerCoutTotalGeneral().ToString(SessionObject.FormatMontant);
            }
        }
        bool checkSelectedItem(CheckBox check)
        {
            CheckBox chk = check;
            return chk.IsChecked.Value;
        }

        void checkerSelectedItem(CheckBox check)
        {
            try
            {
                CheckBox chk = check;
                if (chk.IsChecked.Value)
                    chk.IsChecked = false;
                else
                    chk.IsChecked = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        private void chk_enPose_Checked(object sender, RoutedEventArgs e)
        {
            List<ObjELEMENTDEVIS> lstEltDevis = (List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource;
            if (lstEltDevis != null && this.dataGridElementDevis.SelectedItem != null )
            {
                ObjELEMENTDEVIS SelectedObj =(ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem;
                if (SelectedObj.QUANTITE != null && SelectedObj.QUANTITE != 0)
                {
                    ObjELEMENTDEVIS leEltsSelect = lstEltDevis.FirstOrDefault(t => t.PK_ID == ((ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem).PK_ID);
                    IsEnPose(leEltsSelect, true);
                }
            }

        }

        private void chk_enPose_Unchecked(object sender, RoutedEventArgs e)
        {
           List<ObjELEMENTDEVIS> lstEltDevis = (List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource;
            if (lstEltDevis != null && this.dataGridElementDevis.SelectedItem != null)
            {
                 ObjELEMENTDEVIS SelectedObj =(ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem;
                 if (SelectedObj.QUANTITE != null && SelectedObj.QUANTITE != 0)
                 {
                     ObjELEMENTDEVIS leEltsSelect = lstEltDevis.FirstOrDefault(t => t.PK_ID == ((ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem).PK_ID);
                     IsEnPose(leEltsSelect, false);
                 }
            }
        }

        private void chk_enFourniture_Checked(object sender, RoutedEventArgs e)
        {
            List<ObjELEMENTDEVIS> lstEltDevis = (List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource;
            if (lstEltDevis != null && this.dataGridElementDevis.SelectedItem != null)
            {
                 ObjELEMENTDEVIS SelectedObj =(ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem;
                 if (SelectedObj.QUANTITE != null && SelectedObj.QUANTITE != 0)
                 {
                     ObjELEMENTDEVIS leEltsSelect = lstEltDevis.FirstOrDefault(t => t.PK_ID == ((ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem).PK_ID);
                     IsEnFourniture(leEltsSelect, true);
                 }
            }
        }

        private void chk_enFourniture_Unchecked(object sender, RoutedEventArgs e)
        {
            List<ObjELEMENTDEVIS> lstEltDevis = (List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource;
            if (lstEltDevis != null && this.dataGridElementDevis.SelectedItem != null)
            {
                   ObjELEMENTDEVIS SelectedObj =(ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem;
                   if (SelectedObj.QUANTITE != null && SelectedObj.QUANTITE != 0)
                   {
                       ObjELEMENTDEVIS leEltsSelect = lstEltDevis.FirstOrDefault(t => t.PK_ID == ((ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem).PK_ID);
                       IsEnFourniture(leEltsSelect, false);
                   }
            }
        }

        private void Rdb_Pose_Checked(object sender, RoutedEventArgs e)
        {
            if (this.Rdb_Prestation.IsChecked == true)
                this.dataGridElementDevis.IsEnabled = true;

            List<ObjELEMENTDEVIS> lstEltDevis = ((List<ObjELEMENTDEVIS>)dataGridElementDevis.ItemsSource).ToList();
            foreach (ObjELEMENTDEVIS item in lstEltDevis)
            {
                dataGridElementDevis.SelectedItem = item;
                if (item.QUANTITE != null && item.QUANTITE != 0)
                {
                    if (item.ISPOSE == true)
                    {
                        checkerSelectedItem((CheckBox)this.dataGridElementDevis.Columns[6].GetCellContent(dataGridElementDevis.SelectedItem as ObjELEMENTDEVIS) as CheckBox);
                        IsEnPose(item, true);
                    }
                    else if (item.ISFOURNITURE == true)
                    {
                        checkerSelectedItem((CheckBox)this.dataGridElementDevis.Columns[6].GetCellContent(dataGridElementDevis.SelectedItem as ObjELEMENTDEVIS) as CheckBox);
                        IsEnFourniture(item, true);
                    }
                }
            }
        }


       

        private void Rdb_Prestation_Checked(object sender, RoutedEventArgs e)
        {
            List<ObjELEMENTDEVIS> lstEltDevis = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(lElements.Where(t => t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 0).ToList());
            foreach (ObjELEMENTDEVIS item in lstEltDevis)
            {
                item.PRIX_UNITAIRE = (item.PRIX_UNITAIRE * 10) / 100;
                item.MONTANTHT = (item.MONTANTHT * 10) / 100;
                item.MONTANTTAXE = (item.MONTANTTAXE * 10) / 100;
                item.MONTANTTTC = (item.MONTANTTTC * 10) / 100;

                item.COUT = item.MONTANTTTC.Value;
            }

            this.dataGridElementDevis.ItemsSource = null;
            this.dataGridElementDevis.ItemsSource = lstEltDevis;
            this.dataGridElementDevis.IsEnabled = false;
            CalculerCout();

        }

        private void Rdb_Fourniture_Checked(object sender, RoutedEventArgs e)
        {
            if (this.dataGridElementDevis.ItemsSource != null)
            {
                this.dataGridElementDevis.IsEnabled = true;
                RemplirListeMaterielMT(((List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).ToList(), SessionObject.LstRubriqueDevis);
            }
        }

        private void Rdb_Prestation_Unchecked(object sender, RoutedEventArgs e)
        {
            //RemplirListeDevis(laDetailDemande);
        }

        private void btn_Rejeter_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande,true );
        }

        private void chk_enPose_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void chk_enPM(object sender, RoutedEventArgs e)
        {
            List<ObjELEMENTDEVIS> lstEltDevis = (List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource;
            if (lstEltDevis != null && this.dataGridElementDevis.SelectedItem != null)
            {
                 ObjELEMENTDEVIS SelectedObj =(ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem;
                 if (SelectedObj.QUANTITE != null && SelectedObj.QUANTITE != 0)
                 {
                     ObjELEMENTDEVIS leEltsSelect = lstEltDevis.FirstOrDefault(t => t.PK_ID == ((ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem).PK_ID);
                     IsEnPM(leEltsSelect, true);
                 }
            }
        }

        private void chk_enPM_Unchecked(object sender, RoutedEventArgs e)
        {
            List<ObjELEMENTDEVIS> lstEltDevis = (List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource;
            if (lstEltDevis != null && this.dataGridElementDevis.SelectedItem != null)
            {
                 ObjELEMENTDEVIS SelectedObj =(ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem;
                 if (SelectedObj.QUANTITE != null && SelectedObj.QUANTITE != 0)
                 {
                     ObjELEMENTDEVIS leEltsSelect = lstEltDevis.FirstOrDefault(t => t.PK_ID == ((ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem).PK_ID);
                     IsEnPM(leEltsSelect, false);
                 }
            }
        }
 
        private void dgMyDataGrid_MouseLeftButtonUp1(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsRubriqueDevis>;

            if (dg.SelectedItem != null)
            {
                foreach (CsRubriqueDevis item in allObjects)
                    item.IsSelect = false;

                CsRubriqueDevis SelectedObject = (CsRubriqueDevis)dg.SelectedItem;
                if (SelectedObject.IsSelect == false)
                {
                    SelectedObject.IsSelect = true;
                    if (lElements != null && lElements.Count != 0)
                    {
                        List<ObjELEMENTDEVIS> lstEltDevis = lElements.Where(t => t.FK_IDRUBRIQUEDEVIS == SelectedObject.PK_ID).ToList();
                        this.dataGridElementDevis.ItemsSource = null;
                        this.dataGridElementDevis.ItemsSource = lstEltDevis;
                    }
                }
                else
                    SelectedObject.IsSelect = false;
            }
        }
    }
}


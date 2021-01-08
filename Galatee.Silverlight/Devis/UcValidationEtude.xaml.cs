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

namespace Galatee.Silverlight.Devis
{
    public partial class UcValidationEtude : ChildWindow
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
        public UcValidationEtude(int iddemande)
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
        public UcValidationEtude()
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
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OKButton.IsEnabled = false;
                CsDemande LaDemandeValide = new CsDemande();
                LaDemandeValide.LaDemande = laDetailDemande.LaDemande;
                LaDemandeValide.ObjetScanne = laDetailDemande.ObjetScanne;
                LaDemandeValide.InfoDemande = laDetailDemande.InfoDemande;
                Enregistrer(LaDemandeValide, true);
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
             
                laDemande.LaDemande.ISPRESTATION = (this.Rdb_Prestation.IsChecked == true ? true : false);
                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.ValiderMetreCompleted += (ss, b) =>
                {
                    OKButton.IsEnabled = true;
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (string.IsNullOrEmpty(b.Result))
                    {
                        if (IsTransmetre)
                            Message.ShowInformation("Demande transmise avec succès", "Demande");
                        else
                            Message.ShowInformation("Mise à jour effectuée avec succès", "Demande");
                        this.DialogResult = false;

                    }
                    else
                        Message.ShowError(b.Result, "Demande");
                };
                clientDevis.ValiderMetreAsync(laDemande, IsTransmetre);
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
                var MyLstFourniture = this.ListeFournitureExistante;
                if (MyLstFourniture != null)
                {
                    if (MyElements == null)
                        MyElements = new List<ObjELEMENTDEVIS>();
                    UcListeDesignation frm = new UcListeDesignation(this.ListeFournitureExistante, MyElements, laDetailDemande);
                    if (frm != null)
                    {
                        frm.Closed += new EventHandler(frm_Closed);
                        frm.Show();
                    }
                }
                else
                {
                    Message.ShowInformation("Aucun élément de fourniture coreespondant", "Information");
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
                        this.MyElements.ForEach(p => p.ISEXTENSION = true);
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

        private List<ObjELEMENTDEVIS> LireElements()
        {
            try
            {
                List<ObjELEMENTDEVIS> ListElementDevis = new List<ObjELEMENTDEVIS>();
                if (dataGridElementDevis.ItemsSource != null)
                {
                    foreach (ObjELEMENTDEVIS elementDevis in dataGridElementDevis.ItemsSource)
                    {
                        elementDevis.NUMDEM  = laDetailDemande.LaDemande.NUMDEM ;
                        elementDevis.USERMODIFICATION  = UserConnecte.matricule;
                        elementDevis.DATECREATION = System.DateTime.Today;
                        elementDevis.ORDRE = int.Parse(laDemandeSelect.ORDRE);
                        elementDevis.FK_IDRUBRIQUEDEVIS = SessionObject.LstRubriqueDevis.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DEVISDEXTENSION).PK_ID;
                        elementDevis.RUBRIQUE = SessionObject.Enumere.DEVISDEXTENSION;
                        if (elementDevis.ISDEFAULT == false)
                        {
                            elementDevis.ISPOSE = checkSelectedItem((CheckBox)this.dataGridElementDevis.Columns[7].GetCellContent(elementDevis) as CheckBox);
                            elementDevis.ISPM = checkSelectedItem((CheckBox)this.dataGridElementDevis.Columns[8].GetCellContent(elementDevis) as CheckBox);
                        }
                        elementDevis.ISFOURNITURE = elementDevis.ISPOSE ? false : true;
                        elementDevis.ISPRESTATION = (this.Rdb_Prestation.IsChecked == true ? true : false);
                        ListElementDevis.Add(Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneCopyObjet<ObjELEMENTDEVIS>(elementDevis));
                    }
                }
                if (dataGridElementDevisBranchement.ItemsSource != null)
                {
                    foreach (ObjELEMENTDEVIS elementDevis in dataGridElementDevisBranchement.ItemsSource)
                    {
                        if (elementDevis.FK_IDMATERIELDEVIS == null) continue;
                        elementDevis.FK_IDRUBRIQUEDEVIS = SessionObject.LstRubriqueDevis.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DEVISBRANCHEMENT).PK_ID;
                        elementDevis.RUBRIQUE = SessionObject.Enumere.DEVISBRANCHEMENT;

                        elementDevis.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
                        elementDevis.USERMODIFICATION = UserConnecte.matricule;
                        elementDevis.DATECREATION = System.DateTime.Today;
                        elementDevis.ORDRE = int.Parse(laDemandeSelect.ORDRE);
                        elementDevis.ISPOSE = true;
                        elementDevis.ISFOURNITURE = true;
                        ListElementDevis.Add(Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneCopyObjet<ObjELEMENTDEVIS>(elementDevis));
                    }
                    if (ListeAutreCoutFixe != null && ListeAutreCoutFixe.Count != 0)
                        ListElementDevis.AddRange(ListeAutreCoutFixe);
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
                            ObjELEMENTDEVIS ObjSelect = ListeSelected.FirstOrDefault(t =>t.FK_IDTYPEMATERIEL == select.FK_IDTYPEMATERIEL &&
                                t.FK_IDFOURNITURE == select.FK_IDFOURNITURE);
                            if (ObjSelect != null)
                                ListeSelected.Remove(ObjSelect);
                            this.dataGridElementDevis.ItemsSource = null;
                            this.dataGridElementDevis.ItemsSource = ListeSelected;
                            //this.Txt_MontantTotalG.Text = CalculerCoutTotal().ToString(DataReferenceManager.FormatMontant);

                            this.OKButton.IsEnabled = BtnTransmettre.IsEnabled = (this.Txt_Distance.Text != string.Empty) && (lElements.Count > 0);
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
                MenuContextuel.IsEnabled = dataGridElementDevis.SelectedItem != null;
                MenuContextuel.UpdateLayout();
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

        private void dataGridElementDevis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.ModeExecution != SessionObject.ExecMode.Consultation)
                {
                    if (this.dataGridElementDevis.SelectedItems.Count == 1)
                    {
                        ObjELEMENTDEVIS elt = (ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem;
                        this.selectedElement = elt;
                        this.Btn_Supprimer.IsEnabled = (selectedElement.ISDEFAULT != true) ? true : false;
                    }
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


                Txt_NumDevis.Text = laDetailDemande.LaDemande.NUMDEM;
                Txt_TypeDevis.Text = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
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
                                if (laDetailDemande.EltDevis.FirstOrDefault(t =>t.ISFOURNITURE == true && t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 0) != null)
                                    this.Rdb_Fourniture .IsChecked = true;
                                else if (laDetailDemande.EltDevis.FirstOrDefault(t => t.ISPRESTATION == true && t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 0) != null)
                                    this.Rdb_Prestation .IsChecked = true;

                                if (laDetailDemande.EltDevis.FirstOrDefault(t => (t.ISPRESTATION == true || t.ISPRESTATION == true) && t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 0) == null)
                                {
                                    foreach (var item in laDetailDemande.EltDevis.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null).ToList())
                                        item.ISFOURNITURE = true;
                                }
                                lElements = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(laDetailDemande.EltDevis);
                                ListeAutreCoutFixe = new List<ObjELEMENTDEVIS>();
                                ListeAutreCoutFixe = laDetailDemande.EltDevis.Where(t => t.FK_IDMATERIELDEVIS == 0 || t.FK_IDMATERIELDEVIS == null).ToList();
                                ListeAutreCoutFixe.ForEach(p=>p.FK_IDRUBRIQUEDEVIS = SessionObject.LstRubriqueDevis.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DEVISDEXTENSION).PK_ID);
                                ListeAutreCoutFixe.ForEach(p=>p.RUBRIQUE = SessionObject.Enumere.DEVISDEXTENSION);
                                this.dataGridElementDevis.ItemsSource = null;
                                this.dataGridElementDevis.ItemsSource = laDetailDemande.EltDevis.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null && t.ISEXTENSION == true ).ToList();
                                Txt_MontantTotalExtensionHT.Text = laDetailDemande.EltDevis.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null && t.ISEXTENSION == true).Sum(p => p.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
                                Txt_MontantTotalExtensionTTC.Text = laDetailDemande.EltDevis.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null && t.ISEXTENSION == true).Sum(p => p.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                                Txt_MontantTotalExtensionTaxe.Text = laDetailDemande.EltDevis.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null && t.ISEXTENSION == true).Sum(p => p.MONTANTTAXE ).Value.ToString(SessionObject.FormatMontant);


                                this.dataGridElementDevisBranchement.ItemsSource = null;
                                this.dataGridElementDevisBranchement.ItemsSource = laDetailDemande.EltDevis.Where(t => t.ISEXTENSION == false ).ToList();


                                this.Txt_MontantTotalBranchement.Text = laDetailDemande.EltDevis.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.ISEXTENSION == false ).Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);

                                this.Txt_MontantTotalG.Text = CalculerCoutTotalGeneral().ToString(SessionObject.FormatMontant);

                                if (MyElements == null)
                                    MyElements = new List<ObjELEMENTDEVIS>();
                                this.MyElements = laDetailDemande.EltDevis.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null && t.ISEXTENSION == true).ToList();

                                //ZEG 28/09/2017
                                if (laDetailDemande.AnnotationDemande != null && laDetailDemande.AnnotationDemande.Count != 0) 
                                {
                                    this.txtMotif.Text = laDetailDemande.AnnotationDemande.OrderByDescending(t => t.DATECREATION).First().COMMENTAIRE;
                                    this.lblMotif.Visibility = System.Windows.Visibility.Visible;
                                    this.txtMotif.Visibility = System.Windows.Visibility.Visible;
                                }
                                /****/

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
        private void RemplirListeMateriel(List<ObjELEMENTDEVIS> lstEltDevis)
        {
            List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();
            if (lstEltDevis.Count != 0)
            {
                List<ObjELEMENTDEVIS> lstFourExtension = new List<ObjELEMENTDEVIS>();
                List<ObjELEMENTDEVIS> lstFourBranchement = new List<ObjELEMENTDEVIS>();

                lstFourExtension = lstEltDevis.Where(t => t.ISEXTENSION == true).ToList();
                lstFourBranchement = lstEltDevis.Where(t => t.ISEXTENSION == false).ToList();

                ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
                leSeparateur.DESIGNATION = "----------------------------------";


                if (lstFourBranchement.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                    leResultatBranchanchement.DESIGNATION = "TOTAL BRANCHEMENT ";
                    leResultatBranchanchement.IsCOLORIE = true;
                    leResultatBranchanchement.MONTANTHT = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTHT);
                    leResultatBranchanchement.MONTANTTAXE = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTTAXE);
                    leResultatBranchanchement.MONTANTTTC = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTTTC);

                    lstFourgenerale.AddRange(lstFourBranchement);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatBranchanchement);
                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        DESIGNATION = "    "
                    });

                }
                if (lstFourExtension.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatExtension = new ObjELEMENTDEVIS();
                    leResultatExtension.DESIGNATION = "TOTAL EXTENSION ";
                    leResultatExtension.IsCOLORIE = true;
                    leResultatExtension.MONTANTHT = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTHT);
                    leResultatExtension.MONTANTTAXE = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTTAXE);
                    leResultatExtension.MONTANTTTC = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTTTC);

                    lstFourgenerale.AddRange(lstFourExtension);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatExtension);

                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        DESIGNATION = "    "
                    });
                }
                if (lstFourBranchement.Count != 0 || lstFourExtension.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
                    leResultatGeneral.DESIGNATION = "TOTAL GENERAL ";
                    leResultatGeneral.IsCOLORIE = true;
                    leResultatGeneral.MONTANTHT = lstEltDevis.Sum(t => t.MONTANTHT);
                    leResultatGeneral.MONTANTTAXE = lstEltDevis.Sum(t => t.MONTANTTAXE);
                    leResultatGeneral.MONTANTTTC = lstEltDevis.Sum(t => t.MONTANTTTC);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatGeneral);
                }
            }
            this.dataGridElementDevis.ItemsSource = null;
            this.dataGridElementDevis.ItemsSource = lstFourgenerale;
        }

        private decimal CalculerCoutTotalBranchement()
        {
            decimal MontantTotal = 0;
            try
            {
                foreach (ObjELEMENTDEVIS item in dataGridElementDevisBranchement.ItemsSource)
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
            try
            {
                return ((string.IsNullOrEmpty(this.Txt_MontantTotalBranchement.Text) ? 0 : Convert.ToDecimal(this.Txt_MontantTotalBranchement.Text)) +
                        (string.IsNullOrEmpty(this.Txt_MontantTotalExtensionTTC.Text) ? 0 : Convert.ToDecimal(this.Txt_MontantTotalExtensionTTC.Text)));
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
                        leElts.PRIX_UNITAIRE = leEltss.COUTUNITAIRE_POSE  ;
                        leElts.COUTUNITAIRE_FOURNITURE = 0;
                    }
                    else
                    {
                        leElts.PRIX_UNITAIRE = leEltss.COUTUNITAIRE_POSE + leEltss.COUTUNITAIRE_FOURNITURE;
                        leElts.COUTUNITAIRE_FOURNITURE = leEltss.COUTUNITAIRE_FOURNITURE;
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
                    CalculerCout();
                    this.Txt_MontantTotalG.Text = CalculerCoutTotalGeneral().ToString(SessionObject.FormatMontant);
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        private void CalculerCout()
        {
            try
            {
                if (dataGridElementDevis.ItemsSource != null)
                {
                    List<ObjELEMENTDEVIS> lstEltDevis = ((List<ObjELEMENTDEVIS>)dataGridElementDevis.ItemsSource).Where(i => i.QUANTITE != 0 && i.QUANTITE != null).ToList();
                    Txt_MontantTotalExtensionHT.Text = lstEltDevis.Sum(t => t.MONTANTHT.Value).ToString(SessionObject.FormatMontant);
                    Txt_MontantTotalExtensionTaxe.Text = lstEltDevis.Sum(t => t.MONTANTTAXE.Value).ToString(SessionObject.FormatMontant);
                    Txt_MontantTotalExtensionTTC.Text = lstEltDevis.Sum(t => t.MONTANTTTC.Value).ToString(SessionObject.FormatMontant);
                    this.Txt_MontantTotalG.Text = CalculerCoutTotalGeneral().ToString(SessionObject.FormatMontant); 
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    CalculerCout();
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

                //this.Txt_MontantTotalExtension.Text = CalculerCoutTotalExtension().ToString(SessionObject.FormatMontant);
                this.Txt_MontantTotalG.Text = CalculerCoutTotalGeneral().ToString(SessionObject.FormatMontant);
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
            if (lstEltDevis != null && this.dataGridElementDevis.SelectedItem != null)
            {
                ObjELEMENTDEVIS leEltsSelect = lstEltDevis.FirstOrDefault(t => t.PK_ID == ((ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem).PK_ID);
                IsEnPose(leEltsSelect, true);
            }

        }

        private void chk_enPose_Unchecked(object sender, RoutedEventArgs e)
        {
           List<ObjELEMENTDEVIS> lstEltDevis = (List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource;
            if (lstEltDevis != null && this.dataGridElementDevis.SelectedItem != null)
            {
                ObjELEMENTDEVIS leEltsSelect = lstEltDevis.FirstOrDefault(t => t.PK_ID == ((ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem).PK_ID);
                IsEnPose(leEltsSelect, false );
            }
        }

        private void chk_enFourniture_Checked(object sender, RoutedEventArgs e)
        {
            List<ObjELEMENTDEVIS> lstEltDevis = (List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource;
            if (lstEltDevis != null && this.dataGridElementDevis.SelectedItem != null)
            {
                ObjELEMENTDEVIS leEltsSelect = lstEltDevis.FirstOrDefault(t => t.PK_ID == ((ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem).PK_ID);
                IsEnFourniture(leEltsSelect, true);
            }
        }

        private void chk_enFourniture_Unchecked(object sender, RoutedEventArgs e)
        {
            List<ObjELEMENTDEVIS> lstEltDevis = (List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource;
            if (lstEltDevis != null && this.dataGridElementDevis.SelectedItem != null)
            {
                ObjELEMENTDEVIS leEltsSelect = lstEltDevis.FirstOrDefault(t => t.PK_ID == ((ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem).PK_ID);
                IsEnFourniture(leEltsSelect, false);
            }
        }

        private void chk_enPM_checked(object sender, RoutedEventArgs e)
        {
            List<ObjELEMENTDEVIS> lstEltDevis = (List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource;
            if (lstEltDevis != null && this.dataGridElementDevis.SelectedItem != null)
            {
                ObjELEMENTDEVIS SelectedObj = (ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem;
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
                ObjELEMENTDEVIS SelectedObj = (ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem;
                if (SelectedObj.QUANTITE != null && SelectedObj.QUANTITE != 0)
                {
                    ObjELEMENTDEVIS leEltsSelect = lstEltDevis.FirstOrDefault(t => t.PK_ID == ((ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem).PK_ID);
                    IsEnPM(leEltsSelect, false);
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


       

        private void Rdb_Prestation_Checked(object sender, RoutedEventArgs e)
        {
            List<ObjELEMENTDEVIS> lstEltDevis = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(lElements.Where(t => t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 0 && t.ISEXTENSION == true).ToList());
            foreach (ObjELEMENTDEVIS item in lstEltDevis)
            {
                item.PRIX_UNITAIRE = (item.PRIX_UNITAIRE * 10) / 100;
                item.MONTANTHT = (item.MONTANTHT * 10) / 100;
                item.MONTANTTAXE = (item.MONTANTTAXE * 10) / 100;
                item.MONTANTTTC = (item.MONTANTTTC * 10) / 100;

                item.COUT = item.MONTANTTTC.Value;
            }

            this.dataGridElementDevis.ItemsSource  = null ;
            this.dataGridElementDevis.ItemsSource = lstEltDevis;
            this.dataGridElementDevis.IsEnabled = false;
            //this.Txt_MontantTotalExtension.Text = lstEltDevis.Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
            Txt_MontantTotalExtensionHT.Text = lstEltDevis.Sum(t => t.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
            Txt_MontantTotalExtensionTaxe.Text = lstEltDevis.Sum(t => t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
            Txt_MontantTotalExtensionTTC.Text = lstEltDevis.Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);

            this.Txt_MontantTotalG.Text = CalculerCoutTotalGeneral().ToString(SessionObject.FormatMontant);
        }
        private void FournitureEtPose()
        {
            this.dataGridElementDevis.ItemsSource = null;
            this.dataGridElementDevis.ItemsSource = lElements.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null && t.ISEXTENSION == true).ToList();
            this.dataGridElementDevis.IsEnabled = true;

            Txt_MontantTotalExtensionHT.Text = lElements.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null && t.ISEXTENSION == true).Sum(p => p.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
            Txt_MontantTotalExtensionTTC.Text = lElements.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null && t.ISEXTENSION == true).Sum(p => p.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
            Txt_MontantTotalExtensionTaxe.Text = lElements.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null && t.ISEXTENSION == true).Sum(p => p.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
            this.Txt_MontantTotalG.Text = CalculerCoutTotalGeneral().ToString(SessionObject.FormatMontant);
        }
        private void Rdb_Fourniture_Checked(object sender, RoutedEventArgs e)
        {
            FournitureEtPose();
        }

        private void Rdb_Prestation_Unchecked(object sender, RoutedEventArgs e)
        {
            FournitureEtPose();
        }
        private void EdiderDevisMt(  List<CsRubriqueDevis> leRubriques)
        {
            List<ObjELEMENTDEVIS> lstEl = this.LireElements();
            decimal montantTotal = lstEl.Where(u => u.MONTANTTTC != null).Sum(t => (decimal)(t.MONTANTTTC));
            List<CsEditionDevis> LstDesRubriqueDevis = new List<CsEditionDevis>();
            foreach (ObjELEMENTDEVIS item in lstEl.Where(t => t.CODECOPER != SessionObject.Enumere.CoperCAU).ToList())
            {
                CsEditionDevis LaRubriqueDevis = new CsEditionDevis();
                LaRubriqueDevis.CENTRE = laDetailDemande.LaDemande.CENTRE;
                LaRubriqueDevis.PRODUIT = laDetailDemande.LaDemande.LIBELLEPRODUIT;
                LaRubriqueDevis.TYPEDEMANDE = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                LaRubriqueDevis.COMMUNUE = laDetailDemande.Ag.LIBELLECOMMUNE;
                LaRubriqueDevis.QUARTIER = laDetailDemande.Ag.LIBELLEQUARTIER;
                LaRubriqueDevis.NOM = laDetailDemande.LeClient.NOMABON;
                LaRubriqueDevis.NUMDEMANDE = laDetailDemande.LaDemande.NUMDEM;
                LaRubriqueDevis.LATITUDE = laDetailDemande.Branchement.LATITUDE;
                LaRubriqueDevis.LONGITUDE = laDetailDemande.Branchement.LONGITUDE;
                LaRubriqueDevis.DISTANCEBRT = laDetailDemande.Branchement.LONGBRT.ToString();
                LaRubriqueDevis.DISTANCEEXT = laDetailDemande.Branchement.LONGEXTENSION.ToString();
                LaRubriqueDevis.SITE  = laDetailDemande.LaDemande.PUISSANCESOUSCRITE.ToString();
                LaRubriqueDevis.DESIGNATION = item.DESIGNATION;

                LaRubriqueDevis.QUANTITE = Convert.ToDecimal(item.QUANTITE);
                LaRubriqueDevis.PRIXUNITAIRE = item.PRIX_UNITAIRE.Value;
                LaRubriqueDevis.MONTANTHT = (decimal)(item.MONTANTTTC);
                LaRubriqueDevis.PRIXTVA = (montantTotal * 18) / 100;
                LaRubriqueDevis.TOTALDEVIS = montantTotal;
                if (item.FK_IDRUBRIQUEDEVIS != null)
                    LaRubriqueDevis.SECTION = leRubriques.FirstOrDefault(t => t.PK_ID == item.FK_IDRUBRIQUEDEVIS).LIBELLE;
                else
                    LaRubriqueDevis.SECTION = "";

                LstDesRubriqueDevis.Add(LaRubriqueDevis);
            }
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("pDate", "1");
            param.Add("pNumeroFacture", "2");
            param.Add("pClient", laDetailDemande.LeClient.NOMABON);
            param.Add("pObjet1", "2");
            param.Add("pObjet2", "3");
            Utility.ActionDirectOrientation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevis, param, SessionObject.CheminImpression, "DevisExtension", "Accueil", true);
        }
        private void btn_Imprimer_Click(object sender, RoutedEventArgs e)
        {
            EdiderDevisMt(SessionObject.LstRubriqueDevis );
        }
    }
}


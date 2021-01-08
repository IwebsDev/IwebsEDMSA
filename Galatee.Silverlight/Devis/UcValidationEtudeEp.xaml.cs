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
using Galatee.Silverlight.ServiceAccueil;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using System.ComponentModel;
using System.Globalization;

namespace Galatee.Silverlight.Devis
{
    public partial class UcValidationEtudeEp : ChildWindow
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


        private List<ObjELEMENTDEVIS> ListeFournitureExistante = null;
        private List<ObjELEMENTDEVIS> ListeAutreCoutFixe = null;
        public event PropertyChangedEventHandler PropertyChanged;

        List<ObjELEMENTDEVIS> LesElementInit = new List<ObjELEMENTDEVIS>();
        public UcValidationEtudeEp(int iddemande)
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
        public UcValidationEtudeEp()
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
                    LesElementInit = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(laDetailDemande.EltDevis);
                    RemplirListeDevis(laDetailDemande);
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
            client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Enregistrer(laDetailDemande, true);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Enregistrer(CsDemande laDemande, bool IsTransmetre)
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

                if (laDetailDemande.EltDevis != null && laDetailDemande.EltDevis.Count != 0)
                {
                    int idTrv = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;

                    if (laDetailDemande.LstCoutDemande != null && laDetailDemande.LstCoutDemande.Count != 0)
                        laDetailDemande.LstCoutDemande = new List<CsDemandeDetailCout>();
                    CsDemandeDetailCout leCoutduDevis = new CsDemandeDetailCout();
                    leCoutduDevis.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                    leCoutduDevis.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                    leCoutduDevis.ORDRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.ORDRE) ? null : laDetailDemande.LaDemande.ORDRE;
                    leCoutduDevis.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                    leCoutduDevis.NUMDEM = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                    leCoutduDevis.COPER = SessionObject.Enumere.CoperTRV;
                    leCoutduDevis.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
                    leCoutduDevis.FK_IDTAXE = laDetailDemande.EltDevis.First().FK_IDTAXE.Value;
                    leCoutduDevis.FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID;

                    leCoutduDevis.REFEM = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00");
                    leCoutduDevis.DATECREATION = DateTime.Now;
                    leCoutduDevis.USERCREATION = UserConnecte.matricule;

                    if (laDetailDemande.LaDemande.ISPRESTATION)
                    {
                        if (laDetailDemande.LstCoutDemande != null)
                        {
                            leCoutduDevis.MONTANTHT = (decimal?)Math.Ceiling((double)laDetailDemande.EltDevis.Where(t => t.FK_IDCOPER == idTrv && t.ISEXTENSION == false).Sum(h => h.MONTANTHT));
                            leCoutduDevis.MONTANTTAXE = (decimal?)Math.Ceiling((double)laDetailDemande.EltDevis.Where(t => t.FK_IDCOPER == idTrv && t.ISEXTENSION == false).Sum(h => h.MONTANTTAXE));
                            leCoutduDevis.MONTANTTTC = leCoutduDevis.MONTANTHT + leCoutduDevis.MONTANTTAXE;
                            if (leCoutduDevis.MONTANTTTC > 0)
                                laDetailDemande.LstCoutDemande.Add(leCoutduDevis);
                        }
                        if (laDetailDemande.EltDevis.Where(t => t.ISEXTENSION == true) != null)
                        {
                            CsDemandeDetailCout leCout = Shared.ClasseMEthodeGenerique.RetourneCopyObjet<CsDemandeDetailCout>(leCoutduDevis);
                            leCout.MONTANTHT = (decimal?)Math.Ceiling((double)laDetailDemande.EltDevis.Where(t => t.FK_IDCOPER == idTrv && t.ISEXTENSION == true).Sum(h => h.MONTANTHT));
                            leCout.MONTANTTAXE = (decimal?)Math.Ceiling((double)laDetailDemande.EltDevis.Where(t => t.FK_IDCOPER == idTrv && t.ISEXTENSION == true).Sum(h => h.MONTANTTAXE));
                            leCout.MONTANTTTC = leCout.MONTANTHT + leCout.MONTANTTAXE;
                            leCout.ISEXTENSION = true;
                            if (leCout.MONTANTTTC > 0)
                                laDetailDemande.LstCoutDemande.Add(leCout);
                        }
                    }
                    else
                    {
                        leCoutduDevis.MONTANTHT = (decimal?)Math.Ceiling((double)laDetailDemande.EltDevis.Where(t => t.FK_IDCOPER == idTrv).Sum(h => h.MONTANTHT));
                        leCoutduDevis.MONTANTTAXE = (decimal?)Math.Ceiling((double)laDetailDemande.EltDevis.Where(t => t.FK_IDCOPER == idTrv).Sum(h => h.MONTANTTAXE));
                        leCoutduDevis.MONTANTTTC = leCoutduDevis.MONTANTHT + leCoutduDevis.MONTANTTAXE;
                        if (leCoutduDevis.MONTANTTTC > 0)
                            laDetailDemande.LstCoutDemande.Add(leCoutduDevis);
                    }

                    foreach (ObjELEMENTDEVIS item in laDetailDemande.EltDevis.Where(t => t.FK_IDCOPER != idTrv))
                    {
                        leCoutduDevis = new CsDemandeDetailCout();
                        leCoutduDevis.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                        leCoutduDevis.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                        leCoutduDevis.ORDRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.ORDRE) ? null : laDetailDemande.LaDemande.ORDRE;
                        leCoutduDevis.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                        leCoutduDevis.NUMDEM = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                        leCoutduDevis.COPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.PK_ID == item.FK_IDCOPER).CODE;
                        leCoutduDevis.FK_IDCOPER = item.FK_IDCOPER.Value;
                        leCoutduDevis.FK_IDTAXE = item.FK_IDTAXE.Value;
                        leCoutduDevis.FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID;
                        leCoutduDevis.MONTANTHT = (decimal?)Math.Ceiling((double)item.MONTANTHT);
                        leCoutduDevis.MONTANTTAXE = (decimal?)Math.Ceiling((double)item.MONTANTTAXE);
                        leCoutduDevis.REFEM = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00");
                        leCoutduDevis.DATECREATION = DateTime.Now;
                        leCoutduDevis.USERCREATION = UserConnecte.matricule;
                        if (laDetailDemande.LstCoutDemande == null)
                        {
                            laDetailDemande.LstCoutDemande = new List<CsDemandeDetailCout>();
                            laDetailDemande.LstCoutDemande.Add(leCoutduDevis);
                        }
                        else
                            laDetailDemande.LstCoutDemande.Add(leCoutduDevis);
                    }
                }


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



                var MyLstFourniture = this.ListeFournitureExistante;
                if (MyLstFourniture != null)
                {
                    UcListeDesignation frm = new UcListeDesignation(this.ListeFournitureExistante, MyElements);
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
                        lElements = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(MyElements);
                        //this.MyFournitures = form.MyFournitures;
                        //this.dataGridElementDevis.ItemsSource = null;
                        //this.dataGridElementDevis.ItemsSource = this.MyElements;
                        //this.Txt_MontantTotalG.Text = this.MyElements.Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);


                        this.dataGridElementDevis.ItemsSource = null;
                        this.dataGridElementDevis.ItemsSource = MyElements.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null && t.ISEXTENSION == true).ToList();

                        this.Txt_MontantTotalG.Text = CalculerCoutDevis().ToString(SessionObject.FormatMontant);

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
                        elementDevis.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
                        elementDevis.USERMODIFICATION = UserConnecte.matricule;
                        elementDevis.DATECREATION = System.DateTime.Today;
                        elementDevis.ORDRE = int.Parse(laDemandeSelect.ORDRE);
                        elementDevis.ISPOSE = checkSelectedItem((CheckBox)this.dataGridElementDevis.Columns[6].GetCellContent(elementDevis) as CheckBox);
                        elementDevis.ISFOURNITURE = checkSelectedItem((CheckBox)this.dataGridElementDevis.Columns[7].GetCellContent(elementDevis) as CheckBox);
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
                            ObjELEMENTDEVIS ObjSelect = ListeSelected.FirstOrDefault(t => t.FK_IDFOURNITURE == select.FK_IDFOURNITURE);
                            if (ObjSelect != null)
                                ListeSelected.Remove(ObjSelect);
                            this.dataGridElementDevis.ItemsSource = null;
                            this.dataGridElementDevis.ItemsSource = ListeSelected;
                            //this.Txt_MontantTotalG.Text = CalculerCoutTotal().ToString(DataReferenceManager.FormatMontant);

                            this.Txt_PrixUnitaire.Text = string.Empty;
                            this.Txt_Quantite.Text = string.Empty;
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
                        this.Txt_Quantite.IsReadOnly = false;
                        this.Txt_PrixUnitaire.Text = selectedElement.PRIX.ToString(DataReferenceManager.FormatMontant);
                        this.Txt_Quantite.Text = selectedElement.QUANTITE.ToString();
                        this.Txt_Quantite.SelectAll();
                        this.Txt_Quantite.Focus();
                    }
                    else
                    {
                        this.Txt_PrixUnitaire.IsReadOnly = true;
                        this.Txt_Quantite.IsReadOnly = true;
                        Txt_PrixUnitaire.Text = string.Empty;
                        Txt_Quantite.Text = string.Empty;
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
                                if (laDetailDemande.EltDevis.FirstOrDefault(t => t.ISFOURNITURE == true && t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 0) != null)
                                    this.Rdb_Fourniture.IsChecked = true;
                                else if (laDetailDemande.EltDevis.FirstOrDefault(t => t.ISPRESTATION == true && t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 0) != null)
                                    this.Rdb_Prestation.IsChecked = true;



                                lElements = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(laDetailDemande.EltDevis);
                                ListeAutreCoutFixe = new List<ObjELEMENTDEVIS>();
                                ListeAutreCoutFixe = laDetailDemande.EltDevis.Where(t => t.FK_IDMATERIELDEVIS == 0 || t.FK_IDMATERIELDEVIS == null || t.MONTANTTAXE == 0).ToList();

                                //RemplirListeMateriel(laDetailDemande.EltDevis.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null).ToList());
                                this.dataGridElementDevis.ItemsSource = null;
                                this.dataGridElementDevis.ItemsSource = laDetailDemande.EltDevis.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null && t.MONTANTTAXE != 0).ToList();




                                this.Txt_MontantTotalG.Text = CalculerCoutDevis().ToString(SessionObject.FormatMontant);

                                if (MyElements == null)
                                    MyElements = new List<ObjELEMENTDEVIS>();
                                this.MyElements = laDetailDemande.EltDevis.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null).ToList();
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

                //Txt_NumDevis.Text = laDetailDemande.LaDemande.NUMDEM;
                //Txt_TypeDevis.Text = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                //AcceuilServiceClient Serviceclient = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                //Serviceclient.GetAllFournitureCompleted += (ss, bc) =>
                //{
                //    try
                //    {
                //        if (bc.Cancelled || bc.Error != null)
                //        {
                //            string error = bc.Error.Message;
                //            if (LayoutRoot != null)
                //                LayoutRoot.Cursor = Cursors.Arrow;
                //            Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                //            return;
                //        }
                //        if (bc.Result != null)
                //        {
                //            ListeFournitureExistante = bc.Result;
                //            if (laDemandedevis.EltDevis != null && laDetailDemande.EltDevis.Count != 0)
                //            {
                //                if (laDetailDemande.EltDevis.FirstOrDefault(t => t.ISFOURNITURE == true) != null)
                //                    this.Chk_EnFourniture.IsChecked = true;
                //                else if (laDetailDemande.EltDevis.FirstOrDefault(t => t.ISPRESTATION == true) != null)
                //                    this.Chk_EnPrestation.IsChecked = true;
                //                else this.Chk_EnPose.IsChecked = true;

                //                this.dataGridElementDevis.ItemsSource = null;
                //                this.dataGridElementDevis.ItemsSource = laDetailDemande.EltDevis.Where(t => t.FK_IDFOURNITURE != 0).ToList();
                //                this.Txt_MontantTotal.Text = laDetailDemande.EltDevis.Sum(t => t.MONTANTTTC ).Value .ToString(SessionObject.FormatMontant);


                //                if (MyElements == null)
                //                    MyElements = new List<ObjELEMENTDEVIS>();
                //                this.MyElements = laDetailDemande.EltDevis.Where(t=>t.FK_IDFOURNITURE != 0).ToList();
                //                return;
                //            }
                //            #region Cout globale
                //            if (SessionObject.Enumere.IsGestionGlobaleCoutFournitureDevis)
                //            {
                //                List<ObjFOURNITURE> four = new List<ObjFOURNITURE>();
                //                if (ListeFournitureExistante != null && ListeFournitureExistante != null)
                //                {
                //                    if (string.IsNullOrEmpty(laDemandedevis.Branchement.DIAMBRT))
                //                        four = ListeFournitureExistante.Where(t => t.FK_IDPRODUIT == laDemandedevis.LaDemande.FK_IDPRODUIT &&
                //                                                                  t.FK_IDTYPEDEMANDE == laDemandedevis.LaDemande.FK_IDTYPEDEMANDE &&
                //                                                                  (t.DIAMETRE == laDemandedevis.Branchement.DIAMBRT || string.IsNullOrEmpty(t.DIAMETRE)) &&
                //                                                                  t.ISSUMMARY == true && t.ISADDITIONAL == false).ToList();

                //                    foreach (ObjFOURNITURE f in four)
                //                    {
                //                        _element = new ObjELEMENTDEVIS();
                //                        _element.NUMDEVIS = laDemandedevis.LaDemande.NUMDEM;
                //                        _element.NUMFOURNITURE = f.CODE;
                //                        _element.DESIGNATION = f.LIBELLE ;
                //                        _element.PRIX = (decimal)(f.COUTUNITAIRE_FOURNITURE + f.COUTUNITAIRE_POSE);
                //                        _element.QUANTITE = f.QUANTITY;
                //                        _element.COUT = (decimal)(_element.PRIX * f.QUANTITY);
                //                        _element.ISSUMMARY = f.ISSUMMARY;
                //                        _element.ISADDITIONAL = f.ISADDITIONAL;
                //                        _element.ISDEFAULT = f.ISDEFAULT;
                //                        _element.FK_IDDEMANDE = laDemandedevis.LaDemande.PK_ID;
                //                        _element.FK_IDFOURNITURE = f.PK_ID;
                //                        if (MyElements == null)
                //                            MyElements = new List<ObjELEMENTDEVIS>();
                //                        this.MyElements.Add(_element);
                //                        donnesDatagrid.Add(_element);
                //                    }
                //                    List<ObjFOURNITURE> lstMaterielDefault = ListeFournitureExistante.Where(t => t.FK_IDPRODUIT == laDemandedevis.LaDemande.FK_IDPRODUIT &&
                //                                       t.FK_IDTYPEDEMANDE == laDemandedevis.LaDemande.FK_IDTYPEDEMANDE &&
                //                                       t.DIAMETRE == laDemandedevis.Branchement.DIAMBRT &&
                //                                       t.ISDEFAULT == true).ToList();
                //                    foreach (ObjFOURNITURE f in lstMaterielDefault)
                //                    {
                //                        _element = new ObjELEMENTDEVIS();
                //                        _element.NUMDEVIS = laDemandedevis.LaDemande.NUMDEM;
                //                        _element.NUMFOURNITURE = f.CODE;
                //                        _element.DESIGNATION = f.LIBELLE ;
                //                        _element.PRIX = 0;
                //                        _element.QUANTITE = f.QUANTITY;
                //                        _element.COUT =(decimal) (f.COUTUNITAIRE_FOURNITURE + f.COUTUNITAIRE_POSE);
                //                        _element.ISSUMMARY = f.ISSUMMARY;
                //                        _element.ISADDITIONAL = f.ISADDITIONAL;
                //                        _element.ISDEFAULT = f.ISDEFAULT;
                //                        _element.FK_IDDEMANDE = laDemandedevis.LaDemande.PK_ID;
                //                        _element.FK_IDFOURNITURE = f.PK_ID;
                //                        if (MyElements == null)
                //                            MyElements = new List<ObjELEMENTDEVIS>();
                //                        this.MyElements.Add(_element);
                //                        donnesDatagrid.Add(_element);
                //                    }

                //                    dataGridElementDevis.ItemsSource = null;
                //                    dataGridElementDevis.ItemsSource = donnesDatagrid;
                //                    Txt_Quantite.IsEnabled = false;
                //                    this.Btn_Supprimer.IsEnabled = false;
                //                    this.Btn_Ajouter.IsEnabled = false;
                //                    this.OKButton.IsEnabled = true;

                //                    if (donnesDatagrid.Where(c => c.ISSUMMARY == true).Count() > 0)
                //                    {
                //                        Txt_Quantite.Text = "1";
                //                        dataGridElementDevis.SelectedIndex = 0;
                //                        Txt_Quantite_TextChanged(null, null);
                //                    }
                //                    else
                //                    {
                //                        Message.ShowError("Aucun élements associé à ce type de devis pour le produit concerné", Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                //                        this.OKButton.IsEnabled = false;
                //                    }
                //                    this.Txt_MontantTotal.Text = donnesDatagrid.Sum(t => t.COUT).ToString(SessionObject.FormatMontant);
                //                }
                //            }
                //            #endregion
                //            #region Cout detail
                //            else
                //            {
                //                Galatee.Silverlight.ServiceAccueil.CsCtax tax = new ServiceAccueil.CsCtax();
                //                int idtaxe = 0;
                //                Galatee.Silverlight.ServiceAccueil.CsCoutDemande Devis = SessionObject.LstDesCoutDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperTRV);
                //                if (Devis != null)
                //                {
                //                    idtaxe = Devis.FK_IDTAXE;
                //                    tax = SessionObject.LstDesTaxe.FirstOrDefault(t => t.PK_ID == idtaxe);
                //                    if (tax != null)
                //                        taux = tax.TAUX;
                //                }
                //                List<ObjFOURNITURE> lstMaterielParTypeDevis = ListeFournitureExistante.Where(t => t.FK_IDPRODUIT == laDemandedevis.LaDemande.FK_IDPRODUIT &&
                //                                                                                                  t.FK_IDTYPEDEMANDE == laDemandedevis.LaDemande.FK_IDTYPEDEMANDE && t.ISDEFAULT == true   
                //                                                                                                  //&& 
                //                                                                                                  //(t.DIAMETRE == laDemandedevis.Branchement.DIAMBRT || string.IsNullOrEmpty(t.DIAMETRE))
                //                    ).ToList();
                //                /** Longueur de cable**/
                //                ObjFOURNITURE leFourniture = lstMaterielParTypeDevis.FirstOrDefault(t => t.ISDISTANCE );
                //                if (leFourniture != null)
                //                {
                //                    _element = new ObjELEMENTDEVIS();
                //                    _element.NUMDEVIS = laDemandedevis.LaDemande.NUMDEM;
                //                    _element.NUMFOURNITURE = leFourniture.CODE;
                //                    _element.DESIGNATION = leFourniture.LIBELLE;
                //                    _element.PRIX =(decimal)(leFourniture.COUTUNITAIRE_POSE + leFourniture.COUTUNITAIRE_FOURNITURE );
                //                    _element.QUANTITE = laDemandedevis.Branchement.LONGBRT != null ? (int?)laDemandedevis.Branchement.LONGBRT : 0; ;
                //                    _element.COUT = (_element.PRIX != null && laDemandedevis.Branchement.LONGBRT != null) ? (int)laDemandedevis.Branchement.LONGBRT * (decimal)_element.PRIX : 0;
                //                    _element.MONTANT = (decimal)Math.Ceiling((double)_element.COUT);
                //                    _element.CODECOPER = SessionObject.Enumere.CoperTRV;
                //                    _element.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
                //                    _element.TAXE = (decimal)Math.Ceiling((double)(_element.COUT * taux));
                //                    _element.FK_IDTAXE = tax.PK_ID;
                //                    _element.TVARECAP = _element.TAXE.Value.ToString(SessionObject.FormatMontant);
                //                    _element.ISSUMMARY = leFourniture.ISSUMMARY;
                //                    _element.ISADDITIONAL = leFourniture.ISADDITIONAL;
                //                    _element.ISDEFAULT = leFourniture.ISDEFAULT;
                //                    _element.FK_IDFOURNITURE = leFourniture.PK_ID;
                //                    _element.FK_IDDEMANDE = laDemandedevis.LaDemande.PK_ID;
                //                    if (MyElements == null)
                //                        MyElements = new List<ObjELEMENTDEVIS>();
                //                    this.MyElements.Add(_element);
                //                    donnesDatagrid.Add(_element);
                //                }
                //                /**Autre fourniture**/
                //                foreach (ObjFOURNITURE f in lstMaterielParTypeDevis.Where(t=>!t.ISDISTANCE ).ToList())
                //                {
                //                    _element = new ObjELEMENTDEVIS();
                //                    _element.NUMDEVIS = laDemandedevis.LaDemande.NUMDEM;
                //                    _element.NUMFOURNITURE = f.CODE;
                //                    _element.DESIGNATION = f.LIBELLE ;
                //                    _element.PRIX =(decimal)(f.COUTUNITAIRE_FOURNITURE + f.COUTUNITAIRE_POSE );
                //                    _element.QUANTITE = f.QUANTITY;
                //                    _element.COUT = (_element.PRIX != null && f.QUANTITY != null) ? (int)f.QUANTITY * (decimal)_element.PRIX : 0;
                //                    _element.MONTANT =(decimal)Math.Ceiling((double) _element.COUT);
                //                    _element.CODECOPER = SessionObject.Enumere.CoperTRV;
                //                    _element.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
                //                    _element.TAXE =(decimal)Math.Ceiling((double)(_element.COUT * taux));
                //                    _element.FK_IDTAXE = tax.PK_ID;
                //                    _element.TVARECAP = _element.TAXE.Value.ToString(SessionObject.FormatMontant);
                //                    _element.ISSUMMARY = f.ISSUMMARY;
                //                    _element.ISADDITIONAL = f.ISADDITIONAL;
                //                    _element.ISDEFAULT = f.ISDEFAULT;
                //                    _element.FK_IDFOURNITURE = f.PK_ID;
                //                    _element.FK_IDDEMANDE = laDemandedevis.LaDemande.PK_ID;
                //                    if (MyElements == null)
                //                        MyElements = new List<ObjELEMENTDEVIS>();
                //                    this.MyElements.Add(_element);
                //                    donnesDatagrid.Add(_element);
                //                }
                //                Supplement = Convert.ToDecimal(laDemandedevis.Branchement.LONGBRT - seuilDistance);
                //                if (Supplement > 0)
                //                {
                //                    ObjFOURNITURE MetreAdditionnel = ListeFournitureExistante.FirstOrDefault(t => t.FK_IDPRODUIT == laDemandedevis.LaDemande.FK_IDPRODUIT &&
                //                                                                                                   t.ISADDITIONAL == true );
                //                    if (MetreAdditionnel != null)
                //                    {
                //                        _element = new ObjELEMENTDEVIS();
                //                        _element.NUMDEVIS = laDemandedevis.LaDemande.NUMDEM;
                //                        _element.NUMFOURNITURE = MetreAdditionnel.CODE;
                //                        _element.DESIGNATION = MetreAdditionnel.LIBELLE ;
                //                        _element.PRIX =(decimal) (MetreAdditionnel.COUTUNITAIRE_FOURNITURE + MetreAdditionnel.COUTUNITAIRE_POSE);
                //                        _element.QUANTITE = (int)Supplement;
                //                        _element.COUT = (_element.PRIX != null) ? (decimal)Supplement * (decimal)_element.PRIX : 0;
                //                        _element.MONTANT = _element.COUT;
                //                        _element.CODECOPER = SessionObject.Enumere.CoperTRV;
                //                        _element.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
                //                        _element.TAXE = _element.COUT * taux;
                //                        _element.FK_IDTAXE = tax.PK_ID;
                //                        _element.TVARECAP = _element.TAXE.Value.ToString(SessionObject.FormatMontant);
                //                        _element.ISSUMMARY = MetreAdditionnel.ISSUMMARY;
                //                        _element.ISADDITIONAL = MetreAdditionnel.ISADDITIONAL;
                //                        _element.ISDEFAULT = MetreAdditionnel.ISDEFAULT;
                //                        _element.FK_IDDEMANDE = laDemandedevis.LaDemande.PK_ID;
                //                        _element.FK_IDFOURNITURE = MetreAdditionnel.PK_ID;
                //                        if (MyElements == null)
                //                            MyElements = new List<ObjELEMENTDEVIS>();
                //                        this.MyElements.Add(_element);
                //                        donnesDatagrid.Add(_element);
                //                    }
                //                }
                //                //ChargerCoutDemande(laDemandedevis);
                //                dataGridElementDevis.ItemsSource = null;
                //                ItemsSource = donnesDatagrid;
                //                OKButton.IsEnabled = true;
                //                this.Txt_MontantTotal.Text = donnesDatagrid.Sum(t =>(decimal)( t.COUT + t.TAXE )).ToString(SessionObject.FormatMontant);
                //            }
                //            #endregion


                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        if (LayoutRoot != null)
                //            LayoutRoot.Cursor = Cursors.Arrow;
                //        Message.ShowError(ex.Message, Languages.txtDevis);
                //    }
                //};
                //Serviceclient.GetAllFournitureAsync();

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


        private decimal CalculerCoutDevis()
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
                        //Txt_MontantTotalG.Text = CalculerCoutTotal().ToString(DataReferenceManager.FormatMontant);
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

        private void IsEnPose(ObjELEMENTDEVIS leElt, bool IsChecked)
        {
            try
            {
                List<ObjELEMENTDEVIS> lstEltDevis = new List<ObjELEMENTDEVIS>();
                lstEltDevis = laDetailDemande.EltDevis;
                ObjELEMENTDEVIS leElts = lstEltDevis.FirstOrDefault(t => t.FK_IDMATERIELDEVIS == leElt.FK_IDMATERIELDEVIS);
                ObjELEMENTDEVIS leEltss = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(ListeFournitureExistante).FirstOrDefault(t => t.PK_ID == leElt.FK_IDMATERIELDEVIS);
                if (leElts != null)
                {
                    bool IsFournitureCocher = checkSelectedItem((CheckBox)this.dataGridElementDevis.Columns[7].GetCellContent(dataGridElementDevis.SelectedItem as ObjELEMENTDEVIS) as CheckBox);
                    if (IsChecked)
                    {
                        if (IsFournitureCocher)
                            leElts.PRIX_UNITAIRE = leEltss.COUTUNITAIRE_POSE + leEltss.COUTUNITAIRE_FOURNITURE;
                        else
                            leElts.PRIX_UNITAIRE = leEltss.COUTUNITAIRE_POSE;
                    }
                    else
                    {
                        leElts.PRIX_UNITAIRE = leEltss.COUTUNITAIRE_FOURNITURE;
                        if (!IsFournitureCocher)
                            checkerSelectedItem((CheckBox)this.dataGridElementDevis.Columns[7].GetCellContent(dataGridElementDevis.SelectedItem as ObjELEMENTDEVIS) as CheckBox);
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
            ObjELEMENTDEVIS leElts = lstEltDevis.FirstOrDefault(t => t.FK_IDMATERIELDEVIS == leElt.FK_IDMATERIELDEVIS);
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

                leElts.MONTANT = leElts.MONTANTHT.Value;
                leElts.TAXE = leElts.MONTANTTAXE;
                leElts.COUT = leElts.MONTANTTTC.Value;

                this.Txt_MontantTotalG.Text = CalculerCoutDevis().ToString(SessionObject.FormatMontant);
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
                IsEnPose(leEltsSelect, false);
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
            List<ObjELEMENTDEVIS> lstEltDevis = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(lElements.Where(t => t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 0 ).ToList());
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
            this.Txt_MontantTotalG.Text = CalculerCoutDevis().ToString(SessionObject.FormatMontant);
        }

        private void Rdb_Fourniture_Checked(object sender, RoutedEventArgs e)
        {
            this.dataGridElementDevis.ItemsSource = null;
            this.dataGridElementDevis.ItemsSource = lElements.Where(t => t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null).ToList();
            this.dataGridElementDevis.IsEnabled = true;
            this.Txt_MontantTotalG.Text = CalculerCoutDevis().ToString(SessionObject.FormatMontant);
        }

        private void Rdb_Prestation_Unchecked(object sender, RoutedEventArgs e)
        {
            //RemplirListeDevis(laDetailDemande);
        }

        private void btn_Rejeter_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande, true);
        }
    }
}


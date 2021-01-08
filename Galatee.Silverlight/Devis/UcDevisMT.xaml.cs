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
    public partial class UcDevisMT : ChildWindow
    {
        private ObjDEVIS myDevis = new ObjDEVIS();

        public List<ObjELEMENTDEVIS> MyFournitures { get; set; }
        public List<ObjELEMENTDEVIS> MyElements { get; set; }
        private ObjELEMENTDEVIS _elementDevis = null;
        List<ObjELEMENTDEVIS> ListeFourniture = new List<ObjELEMENTDEVIS>();
        Galatee.Silverlight.ServiceAccueil.CsCoutDemande Devis = new CsCoutDemande();
        Galatee.Silverlight.ServiceAccueil.CsCtax taxe = new CsCtax();
        public UcDevisMT()
        {
            InitializeComponent();
        }
        CsDemande MaDemande = new CsDemande();
        public UcDevisMT(List<ObjELEMENTDEVIS> _ListeFourniture, List<ObjELEMENTDEVIS> lstEltsSelect, CsDemande laDemande)
        {
            InitializeComponent();
            MaDemande = laDemande;
            ListeFourniture = _ListeFourniture;
            Devis = SessionObject.LstDesCoutDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperTRV);
            if (Devis != null)
                taxe = SessionObject.LstDesTaxe.FirstOrDefault(t => t.PK_ID == Devis.FK_IDTAXE);


            List<int> lstIdFournDevis = new List<int>();
            if (lstEltsSelect == null) lstEltsSelect = new List<ObjELEMENTDEVIS>();

            MyElements = lstEltsSelect;
            foreach (ObjELEMENTDEVIS item in lstEltsSelect.Where(t => t.FK_IDMATERIELDEVIS != null))
                lstIdFournDevis.Add(item.FK_IDMATERIELDEVIS.Value);

            RemplirListeMaterielMT(_ListeFourniture, SessionObject.LstRubriqueDevis);
            RemplirListeRubrique();
        }
 

        private void RemplirListeRubrique()
        {
            try
            {
                dataGridRubriqueDevis.ItemsSource = SessionObject.LstRubriqueDevis.Where(t=>t.FK_IDPRODUIT == MaDemande.LaDemande.FK_IDPRODUIT).ToList() ;
                var rowCount = dataGridRubriqueDevis.ItemsSource != null ? dataGridRubriqueDevis.ItemsSource.OfType<object>().Count() : 0;
                //this.OKButton.IsEnabled = (rowCount > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void dataGridElementDevis_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                var fournitureRow = e.Row.DataContext as ObjELEMENTDEVIS ;
                if(fournitureRow != null && !string.IsNullOrEmpty(fournitureRow.UTILISE) && fournitureRow.UTILISE.Contains("*"))
                    e.Row.FontWeight = FontWeights.Bold;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MyFournitures = new List<ObjELEMENTDEVIS>();
                MyFournitures = ((List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource).Where(t => t.QUANTITE != null && t.QUANTITE != 0).ToList();
                MyFournitures.ForEach(t => t.FK_IDTAXE = taxe.PK_ID);
                MyFournitures.ForEach(t => t.ISFOURNITURE = true);
                MyFournitures.ForEach(t => t.ISPOSE  = true);
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                this.DialogResult = null;
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private float Arrondir(float montant)
        {
            try
            {
                string[] partie = montant.ToString().Split(new char[] { ',' });
                if (partie.Length == 1)
                    return float.Parse(partie[0]);
                if (int.Parse(partie[1].Substring(0, 1)) >= 5)
                    return float.Parse(partie[0]) + (float)1;
                else
                    return float.Parse(partie[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {
            try
            {

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
                
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void dataGridElementDevis_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.Key == Key.Enter)
                //Ajouter();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

   

        private void dataGridElementDevis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //this.OKButton.IsEnabled = (this.dataGridElementDevis.SelectedItems.Count > 0);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Btn_Ajouter_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridRubriqueDevis.SelectedItem != null)
            {
                CsRubriqueDevis laRubriqueSelect = (CsRubriqueDevis)dataGridRubriqueDevis.SelectedItem ;
                List<ObjELEMENTDEVIS> _LstEltDevis = Shared.ClasseMEthodeGenerique.RetourneListCopy<ObjELEMENTDEVIS>(ListeFourniture.Where(t=>t.ISGENERE == false).ToList());
                UcListeMaterielDevis ctrt = new UcListeMaterielDevis(_LstEltDevis, laRubriqueSelect);
                ctrt.Closed += ctrt_Closed;
                ctrt.Show();
            }
            else
            {
                Message.ShowInformation("Sélectionnez une rubrique", "");
            }
        }

        void ctrt_Closed(object sender, EventArgs e)
        {
            var form = ((UcListeMaterielDevis)sender);
            if (form != null && form.DialogResult.Value)
            {
                this.MyElements.AddRange(form.MyElements);
                RemplirListeMaterielMT(MyElements, SessionObject.LstRubriqueDevis);
            }
            else
                return;

        }
        private void dgMyDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dmdRow = e.Row.DataContext as ObjELEMENTDEVIS;
            if (dmdRow != null)
            {
                if (dmdRow.IsCOLORIE)
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Green);
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Bold;
                }
            }
        }
        private void RemplirListeMaterielMT(List<ObjELEMENTDEVIS> lstEltDevis,List<CsRubriqueDevis> leRubriques)
        {
            ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
            leSeparateur.LIBELLE  = "----------------------------------";
            leSeparateur.ISDEFAULT = true;
            List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();

            foreach (CsRubriqueDevis item in leRubriques)
            {
                bool MiseAZereLigne = false ;
                List<ObjELEMENTDEVIS> lstFourRubrique = lstEltDevis.Where(t => t.FK_IDRUBRIQUEDEVIS == item.PK_ID).ToList();
                if (lstFourRubrique != null && lstFourRubrique.Count != 0)
                {
                    if (item.PK_ID == 1 && MaDemande.Branchement.CODEBRT == "0001")
                    {
                       decimal? MontantLigne = 0;

                        ObjELEMENTDEVIS leIncidence = ListeFourniture.FirstOrDefault(t=>t.ISGENERE == true );
                        leIncidence.FK_IDRUBRIQUEDEVIS = item.PK_ID;
                        leIncidence.QUANTITE = 1;
                        leIncidence.MONTANTTTC = leIncidence.QUANTITE * (leIncidence.COUTUNITAIRE_FOURNITURE  + leIncidence.COUTUNITAIRE_POSE) * (-1);
                        leIncidence.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;

                        if (lstFourRubrique.FirstOrDefault(t=>t.ISGENERE)== null )
                        lstFourRubrique.Add(leIncidence);
                        MontantLigne = lstFourRubrique.Sum(t => t.MONTANTTTC);
                        if (MontantLigne < 0)
                            MiseAZereLigne = true;

                    }
                    decimal? MontantTotRubrique = lstFourRubrique.Sum(t => t.MONTANTTTC);
                    decimal? MontantTotRubriqueHt = lstFourRubrique.Sum(t => t.MONTANTHT);
                    decimal? MontantTotRubriqueTaxe = lstFourRubrique.Sum(t => t.MONTANTTAXE);
                    if (MiseAZereLigne == true)
                    { MontantTotRubrique = 0; MontantTotRubriqueHt = 0; MontantTotRubriqueTaxe = 0; }
                    ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                    leResultatBranchanchement.LIBELLE = "Sous Total " + item.LIBELLE;
                    leResultatBranchanchement.IsCOLORIE = true;
                    leResultatBranchanchement.FK_IDRUBRIQUEDEVIS = item.PK_ID ;
                    leResultatBranchanchement.ISDEFAULT = true;
                    leResultatBranchanchement.MONTANTHT = MontantTotRubriqueHt;
                    leResultatBranchanchement.MONTANTTAXE = MontantTotRubriqueTaxe;
                    leResultatBranchanchement.MONTANTTTC = MontantTotRubrique;

                    lstFourgenerale.AddRange(lstFourRubrique);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatBranchanchement);
                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        LIBELLE = "    "
                    });
                }
                
            }
            if (lstFourgenerale.Count != 0 )
            {
                decimal? MontantTotRubrique = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTTTC);
                decimal? MontantTotRubriqueHt = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTHT);
                decimal? MontantTotRubriqueTaxe = lstFourgenerale.Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(t => t.MONTANTTAXE);
                if (MontantTotRubrique < 0)
                { MontantTotRubrique = 0; MontantTotRubriqueHt = 0; MontantTotRubriqueTaxe = 0; }


                ObjELEMENTDEVIS leSurveillance = new ObjELEMENTDEVIS();
                leSurveillance.LIBELLE = "Etude et surveillance ";
                leSurveillance.ISFORTRENCH  = true;
                leSurveillance.MONTANTHT = MontantTotRubriqueHt * (decimal)(0.10); ;
                leSurveillance.MONTANTTAXE = MontantTotRubriqueTaxe * (decimal)(0.10); ;
                leSurveillance.MONTANTTTC = MontantTotRubrique * (decimal)(0.10);
                leSurveillance.FK_IDTAXE = SessionObject.LstDesTaxe.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CodeSansTaxe).PK_ID;
                lstFourgenerale.Add(leSurveillance);


                ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
                leResultatGeneral.LIBELLE = "TOTAL GENERAL ";
                leResultatGeneral.IsCOLORIE = true;
                leResultatGeneral.ISDEFAULT = true;
                leResultatGeneral.MONTANTHT = MontantTotRubrique;
                leResultatGeneral.MONTANTTAXE = MontantTotRubriqueHt;
                leResultatGeneral.MONTANTTTC = MontantTotRubriqueTaxe;
                lstFourgenerale.Add(leSeparateur);
                lstFourgenerale.Add(leResultatGeneral);
            }
            MyElements.Clear();
            this.MyElements.AddRange(lstFourgenerale.Where(t=>t.QUANTITE != null && t.QUANTITE != 0).ToList());
            this.dataGridElementDevis.ItemsSource = null;
            this.dataGridElementDevis.ItemsSource = lstFourgenerale.ToList();
        }

        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<ObjELEMENTDEVIS >;

            if (dg.SelectedItem != null)
            {
                ObjELEMENTDEVIS SelectedObject = (ObjELEMENTDEVIS)dg.SelectedItem;
                if (SelectedObject.ISFORTRENCH  == true )
                    this.Btn_Modifier.Visibility = System.Windows.Visibility.Visible;
                else
                    this.Btn_Modifier.Visibility = System.Windows.Visibility.Collapsed ;

            }
        }

        private void Btn_Modifier_Click(object sender, RoutedEventArgs e)
        {
            //UcTauxDeSurveillance ctr = new UcTauxDeSurveillance();
            //ctr.Closed += ctrModif_Closed;
            //ctr.Show();
        }

        void ctrModif_Closed(object sender, EventArgs e)
        {
            //var form = ((UcTauxDeSurveillance)sender);
            //if (form != null && form.IsOkClick )
            //{
            //    decimal  taux =decimal.Parse( form.txt_Taux.Text);
            //    decimal Taux = taux / 100;
            //    ObjELEMENTDEVIS SelectedObject = (ObjELEMENTDEVIS)dataGridElementDevis .SelectedItem;
            //    List<ObjELEMENTDEVIS> lesCoutgnral = ((List<ObjELEMENTDEVIS>)dataGridElementDevis.ItemsSource).Where(y=>y.QUANTITE!= null && y.QUANTITE != 0).ToList();
            //    ObjELEMENTDEVIS leCoutgnral = ((List<ObjELEMENTDEVIS>)dataGridElementDevis.ItemsSource).FirstOrDefault(t => t.LIBELLE == "TOTAL GENERAL ");
            //    if (leCoutgnral != null)
            //    {
            //        SelectedObject.MONTANTHT = lesCoutgnral.Sum(y=>y.MONTANTHT) * Taux;
            //        SelectedObject.MONTANTTAXE = lesCoutgnral.Sum(y => y.MONTANTTAXE ) * Taux;
            //        SelectedObject.MONTANTTTC = lesCoutgnral.Sum(y => y.MONTANTTTC ) * Taux;
            //        lesCoutgnral.Remove(SelectedObject);
            //        lesCoutgnral.Add (SelectedObject);

            //        leCoutgnral.MONTANTHT = lesCoutgnral.Sum(y => y.MONTANTHT);
            //        leCoutgnral.MONTANTTAXE = lesCoutgnral.Sum(y => y.MONTANTTAXE);
            //        leCoutgnral.MONTANTTTC = lesCoutgnral.Sum(y => y.MONTANTTTC);
            //    }
            //}
            //else
            //    return;
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
                if (SelectedObject.IsSelect  == false)
                    SelectedObject.IsSelect = true;
                else
                    SelectedObject.IsSelect = false;
            }
        }

        private void chk_Critere_Checked_1(object sender, RoutedEventArgs e)
        {

        }
    }
}


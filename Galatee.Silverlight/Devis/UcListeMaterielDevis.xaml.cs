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
    public partial class UcListeMaterielDevis : ChildWindow
    {
        private ObjDEVIS myDevis = new ObjDEVIS();

        public List<ObjELEMENTDEVIS> MyElements = new List<ObjELEMENTDEVIS>();
        private ObjELEMENTDEVIS _elementDevis = null;

        List<ObjELEMENTDEVIS> lstElementDevis = new List<ObjELEMENTDEVIS>();
        CsRubriqueDevis laRubrique = null;
        Galatee.Silverlight.ServiceAccueil.CsCoutDemande Devis = new CsCoutDemande();
        Galatee.Silverlight.ServiceAccueil.CsCtax taxe = new CsCtax();
        public UcListeMaterielDevis()
        {
            InitializeComponent();
        }
        public UcListeMaterielDevis(List<ObjELEMENTDEVIS> lstEltsSelect,CsRubriqueDevis laRubriqueSelect)
        {
            InitializeComponent();
            laRubrique = new CsRubriqueDevis();
            laRubrique = laRubriqueSelect;
             Devis = SessionObject.LstDesCoutDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperTRV);
            if (Devis != null)
                taxe = SessionObject.LstDesTaxe.FirstOrDefault(t => t.PK_ID ==  Devis.FK_IDTAXE);
            lstElementDevis = lstEltsSelect;
            lstEltsSelect.ForEach(t => t.FK_IDMATERIELDEVIS = t.PK_ID);
            RemplirListe(lstEltsSelect );
        }
        private void Contruire(List<ObjELEMENTDEVIS> ListeAAfficher)
        {
            try
            {
                dataGridElementDevis.ItemsSource = ListeAAfficher;
                var rowCount = dataGridElementDevis.ItemsSource != null ? dataGridElementDevis.ItemsSource.OfType<object>().Count() : 0;
                this.OKButton.IsEnabled = (rowCount > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListe(List<ObjELEMENTDEVIS> ListeAAfficher)
        {
            try
            {
                dataGridElementDevis.ItemsSource = ListeAAfficher;
                var rowCount = dataGridElementDevis.ItemsSource != null ? dataGridElementDevis.ItemsSource.OfType<object>().Count() : 0;
                this.OKButton.IsEnabled = (rowCount > 0);
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
                var fournitureRow = e.Row.DataContext as ObjELEMENTDEVIS;
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
                Ajouter();
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

      
        private void Ajouter()
        {
            MyElements.AddRange(((List<ObjELEMENTDEVIS>)dataGridElementDevis.ItemsSource).Where(t => t.QUANTITE != null && t.QUANTITE != 0).ToList());
            MyElements.ForEach(t => t.FK_IDRUBRIQUEDEVIS = laRubrique.PK_ID);
        }

        private void dataGridElementDevis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.OKButton.IsEnabled = (this.dataGridElementDevis.SelectedItems.Count > 0);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void txt_LibelleMateriel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.dataGridElementDevis.ItemsSource != null)
            {
                //if (!string.IsNullOrEmpty(this.txt_LibelleMateriel.Text))
                //{
                //    List<ObjELEMENTDEVIS> lstObjetRecherche = lstElementDevis.Where(t => t.LIBELLE.ToUpper().Contains(this.txt_LibelleMateriel.Text.ToUpper())).ToList();
                //    this.dataGridElementDevis.ItemsSource = null;
                //    this.dataGridElementDevis.ItemsSource = lstObjetRecherche;
                //}
                //else
                //{
                //    this.dataGridElementDevis.ItemsSource = null;
                //    this.dataGridElementDevis.ItemsSource = lstElementDevis;
                //}
            }
        }

        private void txt_CodeMateriel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.dataGridElementDevis.ItemsSource != null)
            {
                //if (!string.IsNullOrEmpty(this.txt_CodeMateriel.Text))
                //{
                //    List<ObjELEMENTDEVIS > lstObjetRecherche = lstElementDevis.Where(t => t.CODE.Contains(this.txt_CodeMateriel.Text)).ToList();
                //    i
                //}
                //else
                //{
                //    this.dataGridElementDevis.ItemsSource = null;
                //    this.dataGridElementDevis.ItemsSource = ListeFourniture;
                //}
            }
        }

        private void Txt_Nombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Txt_Nombre.Text))
            {
                if (dataGridElementDevis.SelectedItem != null)
                {
                    var Materiel = dataGridElementDevis.SelectedItem as ObjELEMENTDEVIS;
                    if (Materiel != null)
                    {
                        Materiel.QUANTITE  = int.Parse(Txt_Nombre.Text);
                        Materiel.MONTANTHT = Materiel.COUTUNITAIRE * Materiel.QUANTITE ;
                        Materiel.MONTANTTAXE = Materiel.MONTANTHT * taxe.TAUX;
                        Materiel.MONTANTTTC  = Materiel.MONTANTHT + Materiel.MONTANTTAXE;
                    }
                }
            }
        }
   
    }
}


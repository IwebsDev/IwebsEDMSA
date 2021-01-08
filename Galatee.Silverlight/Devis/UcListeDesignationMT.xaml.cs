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
    public partial class UcListeDesignationMT : ChildWindow
    {
        private ObjDEVIS myDevis = new ObjDEVIS();
   
        public List<ObjELEMENTDEVIS> MyFournitures { get; set; }
        public List<ObjELEMENTDEVIS> MyElements { get; set; }
        private ObjELEMENTDEVIS _elementDevis = null;
        List<ObjELEMENTDEVIS> ListeFourniture = new List<ObjELEMENTDEVIS>();
        Galatee.Silverlight.ServiceAccueil.CsCoutDemande Devis = new CsCoutDemande();
        Galatee.Silverlight.ServiceAccueil.CsCtax taxe = new CsCtax();
        public UcListeDesignationMT()
        {
            InitializeComponent();
            ChargerTypeMateriel();
            RemplirListeRubrique();
        }
        public UcListeDesignationMT(List<ObjELEMENTDEVIS> _ListeFourniture, List<ObjELEMENTDEVIS> lstEltsSelect)
        {
            InitializeComponent();
            ListeFourniture = _ListeFourniture;
             Devis = SessionObject.LstDesCoutDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperTRV);
            if (Devis != null)
                taxe = SessionObject.LstDesTaxe.FirstOrDefault(t => t.PK_ID ==  Devis.FK_IDTAXE);
           

            List<int> lstIdFournDevis = new List<int>();
            if(lstEltsSelect==null)lstEltsSelect=new List<ObjELEMENTDEVIS>();

            MyElements = lstEltsSelect;
            foreach (ObjELEMENTDEVIS item in lstEltsSelect.Where(t=>t.FK_IDMATERIELDEVIS  != null ))
                lstIdFournDevis.Add(item.FK_IDMATERIELDEVIS.Value);

            RemplirListe(ListeFourniture.Where(t => !lstIdFournDevis.Contains(t.PK_ID)).ToList());
            ChargerTypeMateriel();
        }
        CsDemande MaDemande = new CsDemande();
        public UcListeDesignationMT(List<ObjELEMENTDEVIS> _ListeFourniture, List<ObjELEMENTDEVIS> lstEltsSelect, CsDemande laDemande)
        {
            InitializeComponent();
            MaDemande = laDemande;

            RemplirListeRubrique();
            if (MaDemande.LaDemande.ISEXTENSION ==true )
                Chk_Extension.Visibility = System.Windows.Visibility.Visible ;
           
            ListeFourniture = _ListeFourniture;
            Devis = SessionObject.LstDesCoutDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperTRV);
            if (Devis != null)
                taxe = SessionObject.LstDesTaxe.FirstOrDefault(t => t.PK_ID == Devis.FK_IDTAXE);


            List<int> lstIdFournDevis = new List<int>();
            if (lstEltsSelect == null) lstEltsSelect = new List<ObjELEMENTDEVIS>();

            MyElements = lstEltsSelect;
            foreach (ObjELEMENTDEVIS item in lstEltsSelect.Where(t => t.FK_IDMATERIELDEVIS != null))
                lstIdFournDevis.Add(item.FK_IDMATERIELDEVIS.Value);

            RemplirListe(ListeFourniture.Where(t => !lstIdFournDevis.Contains(t.PK_ID)).ToList());
            ChargerTypeMateriel();

        }

        private void RemplirListeRubrique()
        {
            try
            {
                List<CsRubriqueDevis> lesRubr = Shared.ClasseMEthodeGenerique.RetourneListCopy<CsRubriqueDevis>(SessionObject.LstRubriqueDevis);
                dataGridRubriqueDevis.ItemsSource = lesRubr.Where(t => t.FK_IDPRODUIT == MaDemande.LaDemande.FK_IDPRODUIT).ToList();
                var rowCount = dataGridRubriqueDevis.ItemsSource != null ? dataGridRubriqueDevis.ItemsSource.OfType<object>().Count() : 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Contruire(List<ObjELEMENTDEVIS> ListeAAfficher)
        {
            try
            {
                dataGridElementDevis.ItemsSource = ListeAAfficher;
                var rowCount = dataGridElementDevis.ItemsSource != null ? dataGridElementDevis.ItemsSource.OfType<object>().Count() : 0;
                //this.OKButton.IsEnabled = (rowCount > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListe(List<ObjELEMENTDEVIS > ListeAAfficher)
        {
            try
            {
                dataGridElementDevis.ItemsSource = ListeAAfficher;
                var rowCount = dataGridElementDevis.ItemsSource != null ? dataGridElementDevis.ItemsSource.OfType<object>().Count() : 0;
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
                //var fournitureRow = e.Row.DataContext as ObjELEMENTDEVIS;
                //if(fournitureRow != null && !string.IsNullOrEmpty(fournitureRow.UTILISE) && fournitureRow.UTILISE.Contains("*"))
                //    e.Row.FontWeight = FontWeights.Bold;
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
                //if (e.Key == Key.NumPad1 ||
                //    e.Key == Key.NumPad2 ||
                //    e.Key == Key.NumPad3 ||
                //    e.Key == Key.NumPad4 ||
                //    e.Key == Key.NumPad5 ||
                //    e.Key == Key.NumPad6 ||
                //    e.Key == Key.NumPad7 ||
                //    e.Key == Key.NumPad8 ||
                //    e.Key == Key.NumPad9)
                //{
                //    ObjELEMENTDEVIS leElmtSelect = (ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem;
                //    if (leElmtSelect != null)
                //    {
                //        leElmtSelect.QUANTITE = leElmtSelect.QUANTITE ;
                //        leElmtSelect.MONTANTHT = leElmtSelect.COUTUNITAIRE * leElmtSelect.QUANTITE;
                //        leElmtSelect.MONTANTTAXE = leElmtSelect.MONTANTHT * taxe .TAUX;
                //        leElmtSelect.MONTANTTTC = leElmtSelect.MONTANTHT + leElmtSelect.MONTANTTAXE;
                //    }
                //}
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        //private void Ajouter()
        //{
        //    /* Completer la listes des elements du devis avec les
        //     * nouvelles fournitures selectionnées et non marquées
        //     * comme deja presentes sur le devis */

        //    ObjELEMENTDEVIS _element;
        //    foreach (ObjELEMENTDEVIS _fourniture in dataGridElementDevisFinal.SelectedItems)
        //    {
        //        int QuantiteUtilise = 0;
        //        int.TryParse(_fourniture.UTILISE, out QuantiteUtilise);
        //        if (_fourniture.UTILISE != "***")
        //        {

        //            if (QuantiteUtilise != 0)
        //            {
        //                _element = new ObjELEMENTDEVIS();
        //                _element.NUMDEM = this.myDevis.NUMDEVIS;
        //                _element.NUMDEVIS = this.myDevis.NUMDEVIS;
        //                _element.NUMFOURNITURE = _fourniture.CODE;
        //                _element.DESIGNATION = _fourniture.LIBELLE;
        //                _element.PRIX = (decimal)(_fourniture.COUTUNITAIRE_POSE + _fourniture.COUTUNITAIRE_FOURNITURE);
        //                _element.QUANTITE = QuantiteUtilise;
        //                _element.COUT = (decimal)(_element.PRIX * _fourniture.QUANTITE);
        //                _element.ISADDITIONAL = _fourniture.ISADDITIONAL;
        //                _element.ISSUMMARY = _fourniture.ISSUMMARY;
        //                _element.ISDEFAULT = _fourniture.ISDEFAULT;
        //                _element.FK_IDFOURNITURE = _fourniture.PK_ID;
        //                if (MyElements != null && MyElements.Count() > 0)
        //                {
        //                    _element.FK_IDDEMANDE = MyElements.First().PK_ID;
        //                    _element.ORDRE = (int)MyElements.First().ORDRE;
        //                }

        //                this.MyElements.Add(_element);
        //            }
        //            else
        //            {
        //                Message.ShowWarning("Veuillez saisir une quantité au format numerique", "Information");
        //            }
        //        }
        //    }

        //}
        //private void Ajouter()
        //{
        //    /* Completer la listes des elements du devis avec les
        //     * nouvelles fournitures selectionnées et non marquées
        //     * comme deja presentes sur le devis */

        //    ObjELEMENTDEVIS _element;
        //    foreach (ObjELEMENTDEVIS _fourniture in dataGridElementDevisFinal.SelectedItems)
        //    {
        //        int QuantiteUtilise = 0;
        //        int.TryParse(_fourniture.UTILISE, out QuantiteUtilise);
        //        if (_fourniture.UTILISE != "***")
        //        {

        //            if (QuantiteUtilise != 0)
        //            {
        //                _element = new ObjELEMENTDEVIS();
        //                _element.NUMDEM = this.myDevis.NUMDEVIS;
        //                _element.NUMDEVIS = this.myDevis.NUMDEVIS;
        //                _element.NUMFOURNITURE = _fourniture.CODE;
        //                _element.DESIGNATION = _fourniture.LIBELLE;
        //                _element.PRIX = (decimal)(_fourniture.COUTUNITAIRE_POSE + _fourniture.COUTUNITAIRE_FOURNITURE);
        //                _element.QUANTITE = QuantiteUtilise;
        //                _element.COUT = (decimal)(_element.PRIX * _fourniture.QUANTITE);
        //                _element.ISADDITIONAL = _fourniture.ISADDITIONAL;
        //                _element.ISSUMMARY = _fourniture.ISSUMMARY;
        //                _element.ISDEFAULT = _fourniture.ISDEFAULT;
        //                _element.FK_IDFOURNITURE = _fourniture.PK_ID;
        //                if (MyElements != null && MyElements.Count() > 0)
        //                {
        //                    _element.FK_IDDEMANDE = MyElements.First().PK_ID;
        //                    _element.ORDRE = (int)MyElements.First().ORDRE;
        //                }

        //                this.MyElements.Add(_element);
        //            }
        //            else
        //            {
        //                Message.ShowWarning("Veuillez saisir une quantité au format numerique", "Information");
        //            }
        //        }
        //    }

        //}
        private void Ajouter()
        {
            /* Completer la listes des elements du devis avec les
             * nouvelles fournitures selectionnées et non marquées
             * comme deja presentes sur le devis */

            ObjELEMENTDEVIS _element;
            foreach (ObjELEMENTDEVIS _fourniture in dataGridElementDevisFinal.ItemsSource )
            {
                        _element = new ObjELEMENTDEVIS();
                        _element.NUMDEM = this.myDevis.NUMDEVIS;
                        _element.DESIGNATION =_element.LIBELLE  = _fourniture.LIBELLE;
                        _element.PRIX =(decimal )(_fourniture.COUTUNITAIRE_POSE + _fourniture.COUTUNITAIRE_FOURNITURE ) ;
                        _element.PRIX_UNITAIRE = (decimal)(_fourniture.COUTUNITAIRE_POSE + _fourniture.COUTUNITAIRE_FOURNITURE);
                        _element.QUANTITE = _fourniture.QUANTITE ;
                        _element.COUT = (decimal)(_element.PRIX * _fourniture.QUANTITE); 
                        _element.ISADDITIONAL = _fourniture.ISADDITIONAL;
                        _element.ISSUMMARY = _fourniture.ISSUMMARY;
                        _element.ISDEFAULT = _fourniture.ISDEFAULT;
                        _element.ISEXTENSION = _fourniture.ISEXTENSION ;
                        _element.FK_IDMATERIELDEVIS = _fourniture.PK_ID;
                        _element.MONTANTTAXE = _fourniture.MONTANTTAXE ;
                        _element.MONTANTHT = _fourniture.MONTANTHT ;
                        _element.MONTANTTTC = _fourniture.MONTANTTTC ;
                        _element.FK_IDCOPER = Devis.FK_IDCOPER;
                        _element.FK_IDRUBRIQUEDEVIS = _fourniture.FK_IDRUBRIQUEDEVIS;
                        _element.FK_IDTAXE = taxe.PK_ID;
                        _element.COUTUNITAIRE_FOURNITURE = _fourniture.COUTUNITAIRE_FOURNITURE;
                        _element.COUTUNITAIRE_POSE = _fourniture.COUTUNITAIRE_POSE;
                        _element.ISFOURNITURE = true;
                        _element.RUBRIQUE  = _fourniture.RUBRIQUE ;
                        _element.USERCREATION = UserConnecte.matricule;
                        _element.DATECREATION  = System.DateTime.Now ;

                
                        if (MyElements != null && MyElements.Count() > 0)
                        {
                            _element.FK_IDDEMANDE = MyElements.First().PK_ID;
                            _element.ORDRE = (int)MyElements.First().ORDRE;
                        }
                        this.MyElements.Add(_element);
            }
        
        }
        private void txt_LibelleMateriel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.dataGridElementDevis.ItemsSource != null)
            {
                if (!string.IsNullOrEmpty(this.txt_LibelleMateriel.Text))
                {
                    List<ObjELEMENTDEVIS > lstObjetRecherche = ListeFourniture.Where(t => t.LIBELLE.ToUpper().Contains(this.txt_LibelleMateriel.Text.ToUpper())).ToList();
                    this.dataGridElementDevis.ItemsSource = null;
                    this.dataGridElementDevis.ItemsSource = lstObjetRecherche;
                }
                else
                {
                    this.dataGridElementDevis.ItemsSource = null;
                    this.dataGridElementDevis.ItemsSource = ListeFourniture;
                }
            }
        }

        //private void Txt_Nombre_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(Txt_Nombre.Text) )
        //    {
        //        if (dataGridElementDevis.SelectedItem != null)
        //        {
        //            var Materiel = dataGridElementDevis.SelectedItem as ObjELEMENTDEVIS;
        //            if (Materiel != null)
        //            {
        //                Materiel.QUANTITE  = int.Parse(Txt_Nombre.Text);
        //                Materiel.MONTANTHT = Materiel.COUTUNITAIRE * Materiel.QUANTITE ;
        //                Materiel.MONTANTTAXE = Materiel.MONTANTHT * Cbo_Nationnalite.TAUX;
        //                Materiel.MONTANTTTC = Materiel.MONTANTHT + Materiel.MONTANTTAXE;
        //            }
        //        }
        //    }
        //}
        private void btn_Ajouter_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGridElementDevis.SelectedItem != null)
            {
                List<ObjELEMENTDEVIS> lstElementDevisFinal = new List<ObjELEMENTDEVIS>();
                List<ObjELEMENTDEVIS> lstElementDevis = ((List<ObjELEMENTDEVIS>)this.dataGridElementDevis.ItemsSource).ToList(); 
                ObjELEMENTDEVIS leEltSelect = Shared.ClasseMEthodeGenerique.RetourneCopyObjet<ObjELEMENTDEVIS>((ObjELEMENTDEVIS)this.dataGridElementDevis.SelectedItem);
                if (lstElementDevis != null && lstElementDevis.Count != 0)
                {
                    if (lstElementDevis.FirstOrDefault(t => t.PK_ID == leEltSelect.PK_ID && t.ISEXTENSION == leEltSelect.ISEXTENSION) != null)
                    {
                        Message.ShowInformation("Ce materiel est deja utilisé", "Devis");
                        return;
                    }
                }
                if (leEltSelect.QUANTITE != 0 && leEltSelect.QUANTITE != null)
                {
                    leEltSelect.ISEXTENSION = this.Chk_Extension.IsChecked == true ? true : false;
                    if (dataGridElementDevisFinal.ItemsSource != null )
                        lstElementDevisFinal = ((List<ObjELEMENTDEVIS>)this.dataGridElementDevisFinal.ItemsSource).ToList(); 
                    else
                        lstElementDevisFinal.Add(leEltSelect);
                }
                else
                {
                    Message.ShowInformation("Saisir la quantité", "Devis");
                    return;
                }
                this.dataGridElementDevisFinal.ItemsSource = null;
                this.dataGridElementDevisFinal.ItemsSource = lstElementDevisFinal;
            }
        }
        
        //private void RemplirListeMateriel()
        //{
        //    List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();
        //    if (lstEltDevis.Count != 0)
        //    {
        //        List<ObjELEMENTDEVIS> lstFourExtension = new List<ObjELEMENTDEVIS>();
        //        List<ObjELEMENTDEVIS> lstFourBranchement = new List<ObjELEMENTDEVIS>();
               
                 
        //        lstFourExtension = lstEltDevis.Where(t => t.ISEXTENSION == true).ToList();
        //        lstFourBranchement = lstEltDevis.Where(t => t.ISEXTENSION == false).ToList();

        //        ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
        //        leSeparateur.LIBELLE = "----------------------------------";

        //        if (lstFourExtension.Count != 0)
        //        {
        //            ObjELEMENTDEVIS leResultatExtension = new ObjELEMENTDEVIS();
        //            leResultatExtension.LIBELLE = "TOTAL " + lstEltDevis.First(t => t.ISEXTENSION == true).LIBELLE ;
        //            leResultatExtension.MONTANTHT = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTHT);
        //            leResultatExtension.MONTANTTAXE = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTTAXE);
        //            leResultatExtension.MONTANTTTC = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTTTC);

        //            lstFourgenerale.AddRange(lstFourExtension);
        //            lstFourgenerale.Add(leSeparateur);
        //            lstFourgenerale.Add(leResultatExtension);
        //        }
        //        if (lstFourBranchement.Count != 0)
        //        {
        //            ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
        //            leResultatBranchanchement.LIBELLE = "TOTAL " + lstEltDevis.First(t => t.ISEXTENSION == false).LIBELLE ;
        //            leResultatBranchanchement.MONTANTHT = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTHT);
        //            leResultatBranchanchement.MONTANTTAXE = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTTAXE);
        //            leResultatBranchanchement.MONTANTTTC = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTTTC);

        //            lstFourgenerale.AddRange(lstFourBranchement);
        //            lstFourgenerale.Add(leSeparateur);
        //            lstFourgenerale.Add(leResultatBranchanchement);
        //        }
        //        if (lstFourBranchement.Count != 0 || lstFourExtension.Count != 0)
        //        {
        //            ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
        //            leResultatGeneral.LIBELLE = "TOTAL ";
        //            leResultatGeneral.MONTANTHT = lstEltDevis.Sum(t => t.MONTANTHT);
        //            leResultatGeneral.MONTANTTAXE = lstEltDevis.Sum(t => t.MONTANTTAXE);
        //            leResultatGeneral.MONTANTTTC = lstEltDevis.Sum(t => t.MONTANTTTC);
        //            lstFourgenerale.Add (leSeparateur);
        //            lstFourgenerale.Add(leResultatGeneral);
        //        }
        //    }
        //    this.dataGridElementDevisFinal.ItemsSource = null;
        //    this.dataGridElementDevisFinal.ItemsSource = lstFourgenerale;
        //}
        private void Btn_Retirer_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGridElementDevisFinal.SelectedItem != null)
            {
               List< ObjELEMENTDEVIS> listeEltSelect = ((List<ObjELEMENTDEVIS>)this.dataGridElementDevisFinal.ItemsSource).ToList() ;
                ObjELEMENTDEVIS leEltSelect = (ObjELEMENTDEVIS)this.dataGridElementDevisFinal.SelectedItem;
                listeEltSelect.Remove(leEltSelect);
                this.dataGridElementDevisFinal.ItemsSource = null;
                this.dataGridElementDevisFinal.ItemsSource = listeEltSelect;
               
            }
        }
        //private void Txt_NombreFinal_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(Txt_Nombre.Text))
        //    {
        //        if (dataGridElementDevisFinal.SelectedItem != null)
        //        {
        //            var Materiel = dataGridElementDevisFinal.SelectedItem as ObjELEMENTDEVIS;
        //            if (Materiel != null)
        //            {
        //                Materiel.QUANTITE = int.Parse(Txt_Nombre.Text);
        //                Materiel.MONTANTHT = Materiel.COUTUNITAIRE * Materiel.QUANTITE;
        //                Materiel.MONTANTTAXE = Materiel.MONTANTHT * taxe.TAUX;
        //                Materiel.MONTANTTTC = Materiel.MONTANTHT + Materiel.MONTANTTAXE;
        //            }
        //        }
        //    }
        //}

        private void ChargerTypeMateriel()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneTypeMaterielCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    Cbo_TypeMateriel.ItemsSource = args.Result.ToList();
                    Cbo_TypeMateriel.DisplayMemberPath = "LIBELLE";
                    Cbo_TypeMateriel.SelectedValuePath = "PK_ID";
                };
                service.RetourneTypeMaterielAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Cbo_TypeMateriel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Cbo_TypeMateriel.SelectedItem != null)
            {
                if (!string.IsNullOrEmpty(((CsRubriqueDevis)this.Cbo_TypeMateriel.SelectedItem).LIBELLE))
                {
                    this.dataGridElementDevis.ItemsSource = null;
                    this.dataGridElementDevis.ItemsSource = ListeFourniture.Where(t => t.FK_IDTYPEMATERIEL == ((CsRubriqueDevis)this.Cbo_TypeMateriel.SelectedItem).PK_ID).ToList();
                }
                else
                {
                    this.dataGridElementDevis.ItemsSource = null;
                    this.dataGridElementDevis.ItemsSource = ListeFourniture;
                }
            }
        }

        DateTime lastClick = DateTime.Now;
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<ObjELEMENTDEVIS>;
            if (dg.SelectedItem != null && this.dataGridRubriqueDevis.SelectedItem != null)
            {
                ObjELEMENTDEVIS SelectedObject = Shared.ClasseMEthodeGenerique.RetourneCopyObjet<ObjELEMENTDEVIS>((ObjELEMENTDEVIS)dg.SelectedItem);
                if ((DateTime.Now - lastClick).Ticks < 2500000)
                {
                    if (SelectedObject.QUANTITE == null || SelectedObject.QUANTITE == 0)
                    {
                        if (SelectedObject.ISCOMPTEUR && MaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance)
                        {
                            UcSaisiQuantite ctrl = new UcSaisiQuantite(SelectedObject, true, true, taxe);
                            this.IsEnabled = false;
                            ctrl.Closed += ctrl_Closed;
                            ctrl.Show();
                        }
                        else
                        {
                            UcSaisiQuantite ctrl = new UcSaisiQuantite(SelectedObject, true, taxe);
                            ctrl.Closed += ctrl_Closed;
                            ctrl.Show();
                        }
                    }
                    else
                        btn_Ajouter_Click(null, null);
                }
                lastClick = DateTime.Now;
            }
            else
            {
                this.IsEnabled = false;
                Message.ShowInformation("Sélectionner la rubrique de matériel", "Devis");
                this.IsEnabled = true;
            }
        }
        void ctrl_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true ;
            UcSaisiQuantite ctrs = sender as UcSaisiQuantite;
            if (ctrs.isOkClick)
            {
                List<ObjELEMENTDEVIS> lesElements = new List<ObjELEMENTDEVIS>();
                if (this.dataGridElementDevisFinal.ItemsSource != null)
                {
                    lesElements = ((List<ObjELEMENTDEVIS>)this.dataGridElementDevisFinal.ItemsSource).ToList();
                    ObjELEMENTDEVIS leElemt = lesElements.FirstOrDefault(t => t.FK_IDMATERIELDEVIS == ctrs.SelectedObject.FK_IDMATERIELDEVIS && t.FK_IDRUBRIQUEDEVIS == ctrs.SelectedObject.FK_IDRUBRIQUEDEVIS);
                    if (leElemt == null)
                    {
                        if (MaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementMt)
                        {
                            if (this.dataGridRubriqueDevis.SelectedItem != null)
                            {
                                ctrs.SelectedObject.FK_IDRUBRIQUEDEVIS = ((CsRubriqueDevis)this.dataGridRubriqueDevis.SelectedItem ).PK_ID ;
                                ctrs.SelectedObject.RUBRIQUE = ((CsRubriqueDevis)this.dataGridRubriqueDevis.SelectedItem ).CODE ;
                                ctrs.SelectedObject.ISEXTENSION = true;
                            }
                        }
                        lesElements.Add(ctrs.SelectedObject);
                    }
                    else
                    {
                        lesElements.Remove(leElemt);
                        lesElements.Add(leElemt);
                    }
                }
                else
                {
                    if (this.dataGridRubriqueDevis.SelectedItem != null)
                    {
                        ctrs.SelectedObject.FK_IDRUBRIQUEDEVIS = ((CsRubriqueDevis)this.dataGridRubriqueDevis.SelectedItem).PK_ID;
                        ctrs.SelectedObject.RUBRIQUE = ((CsRubriqueDevis)this.dataGridRubriqueDevis.SelectedItem).CODE;
                        ctrs.SelectedObject.ISEXTENSION = true;
                    }
                    lesElements.Add(ctrs.SelectedObject);
                }
                this.dataGridElementDevisFinal.ItemsSource = null;
                this.dataGridElementDevisFinal.ItemsSource = lesElements;
                if (lesElements != null && lesElements.Count != 0)
                this.txt_MontantTotalDevis.Text = lesElements.Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
            }
        }

        private void dgMyDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //DataGrid dg = (sender as DataGrid);
            //this.dataGridElementDevis.Columns[1].IsReadOnly = true;
            //this.dataGridElementDevis.Columns[2].IsReadOnly = true;
            //this.dataGridElementDevis.Columns[3].IsReadOnly = true;
            //this.dataGridElementDevis.Columns[4].IsReadOnly = true;
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
                    SelectedObject.IsSelect = true;
                else
                    SelectedObject.IsSelect = false;
            }
        }
    }
}


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
    public partial class UcListeDesignation : ChildWindow
    {
        private ObjDEVIS myDevis = new ObjDEVIS();
   
        public List<ObjELEMENTDEVIS> MyFournitures { get; set; }
        public List<ObjELEMENTDEVIS> MyElements { get; set; }
        private ObjELEMENTDEVIS _elementDevis = null;
        List<ObjELEMENTDEVIS> ListeFourniture = new List<ObjELEMENTDEVIS>();
        Galatee.Silverlight.ServiceAccueil.CsCoutDemande Devis = new CsCoutDemande();
        Galatee.Silverlight.ServiceAccueil.CsCtax taxe = new CsCtax();
        public UcListeDesignation()
        {
            InitializeComponent();
            ChargerTypeMateriel();
        }
        public UcListeDesignation(List<ObjELEMENTDEVIS> _ListeFourniture, List<ObjELEMENTDEVIS> lstEltsSelect)
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
        public UcListeDesignation(List<ObjELEMENTDEVIS> _ListeFourniture, List<ObjELEMENTDEVIS> lstEltsSelect, CsDemande laDemande)
        {
            InitializeComponent();
            Chk_Extension.Visibility = System.Windows.Visibility.Collapsed;
           
            MaDemande = laDemande;
            if (MaDemande.LaDemande.ISEXTENSION ==true )
                Chk_Extension.Visibility = System.Windows.Visibility.Visible ;
           
            ListeFourniture = _ListeFourniture;
            foreach (ObjELEMENTDEVIS item in ListeFourniture)
            {
                if (item.COUTUNITAIRE_FOURNITURE == null)
                    item.COUTUNITAIRE_FOURNITURE = 0;
                if (item.COUTUNITAIRE_POSE  == null)
                    item.COUTUNITAIRE_POSE = 0;
            }
            foreach (ObjELEMENTDEVIS item in lstEltsSelect)
            {
                if (item.COUTUNITAIRE_FOURNITURE == null)
                    item.COUTUNITAIRE_FOURNITURE = 0;
                if (item.COUTUNITAIRE_POSE == null)
                    item.COUTUNITAIRE_POSE = 0;
            }
            Devis = SessionObject.LstDesCoutDemande.FirstOrDefault(t => t.COPER == SessionObject.Enumere.CoperTRV);
            if (Devis != null)
                taxe = SessionObject.LstDesTaxe.FirstOrDefault(t => t.PK_ID == Devis.FK_IDTAXE);


            List<int> lstIdFournDevis = new List<int>();
            if (lstEltsSelect == null) lstEltsSelect = new List<ObjELEMENTDEVIS>();

            MyElements = lstEltsSelect;
            foreach (ObjELEMENTDEVIS item in lstEltsSelect.Where(t => t.FK_IDMATERIELDEVIS != null))
                lstIdFournDevis.Add(item.FK_IDMATERIELDEVIS.Value);

            dataGridElementDevisFinal.ItemsSource = null;
            dataGridElementDevisFinal.ItemsSource = lstEltsSelect;

            this.Txt_TotalHt.Text = lstEltsSelect.Where(p => p.QUANTITE != null && p.QUANTITE != 0).ToList().Sum(t => t.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
            this.Txt_TotalTtc.Text = lstEltsSelect.Where(p => p.QUANTITE != null && p.QUANTITE != 0).ToList().Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
            this.Txt_TotalTva.Text = lstEltsSelect.Where(p => p.QUANTITE != null && p.QUANTITE != 0).ToList().Sum(t => t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);

            RemplirListe(ListeFourniture.Where(t => !lstIdFournDevis.Contains(t.PK_ID)).ToList());
            ChargerTypeMateriel();


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

        private void RemplirListe(List<ObjELEMENTDEVIS > ListeAAfficher)
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
 
        private void Ajouter()
        {
            if (this.MyElements != null && this.MyElements.Count != 0)
                this.MyElements.Clear();

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
                        _element.FK_IDMATERIELDEVIS = _fourniture.FK_IDMATERIELDEVIS ;
                        if (_fourniture.ISDEFAULT == true)
                        {
                            _element.FK_IDMATERIELDEVIS = null;
                            _element.FK_IDFOURNITURE  = null;
                            _element.FK_IDCOPER = _fourniture.FK_IDCOPER;
                            _element.FK_IDCOUTCOPER = _fourniture.FK_IDCOUTCOPER;
                        }
                        else
                            _element.FK_IDCOPER = Devis.FK_IDCOPER;

                        _element.MONTANTTAXE = _fourniture.MONTANTTAXE ;
                        _element.MONTANTHT = _fourniture.MONTANTHT ;
                        _element.MONTANTTTC = _fourniture.MONTANTTTC ;
                        _element.FK_IDRUBRIQUEDEVIS = _fourniture.FK_IDRUBRIQUEDEVIS;
                        _element.FK_IDTAXE = taxe.PK_ID;
                        _element.COUTUNITAIRE_FOURNITURE = _fourniture.COUTUNITAIRE_FOURNITURE;
                        _element.COUTUNITAIRE_POSE = _fourniture.COUTUNITAIRE_POSE;
                        _element.ISFOURNITURE = true;
                        _element.RUBRIQUE  = _fourniture.RUBRIQUE ;

                        _element.ISPOSE = _fourniture.ISPOSE;
                        _element.ISFOURNITURE = _fourniture.ISFOURNITURE;
                        _element.ISPM = _fourniture.ISPM;
                
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
        private void Btn_Retirer_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGridElementDevisFinal.SelectedItem != null)
            {
               List< ObjELEMENTDEVIS> listeEltSelect = ((List<ObjELEMENTDEVIS>)this.dataGridElementDevisFinal.ItemsSource).ToList() ;
                ObjELEMENTDEVIS leEltSelect = (ObjELEMENTDEVIS)this.dataGridElementDevisFinal.SelectedItem;
                listeEltSelect.Remove(leEltSelect);
                this.dataGridElementDevisFinal.ItemsSource = null;
                this.dataGridElementDevisFinal.ItemsSource = listeEltSelect;
                this.Txt_TotalHt.Text = listeEltSelect.Where(p => p.QUANTITE != null && p.QUANTITE != 0).ToList().Sum(t => t.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalTtc.Text = listeEltSelect.Where(p => p.QUANTITE != null && p.QUANTITE != 0).ToList().Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalTva.Text = listeEltSelect.Where(p => p.QUANTITE != null && p.QUANTITE != 0).ToList().Sum(t => t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
            }
        }
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
            if (dg.SelectedItem != null)
            {
                ObjELEMENTDEVIS SelectedObject =Shared.ClasseMEthodeGenerique.RetourneCopyObjet<ObjELEMENTDEVIS>((ObjELEMENTDEVIS)dg.SelectedItem);
                if ((DateTime.Now - lastClick).Ticks < 2500000)
                {
                    if (SelectedObject.QUANTITE == null || SelectedObject.QUANTITE == 0)
                    {
                        if (SelectedObject.ISCOMPTEUR && MaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance)
                        {

                            UcSaisiQuantite ctrl = new UcSaisiQuantite(SelectedObject, true,true , taxe);
                            this.IsEnabled = false;
                            ctrl.Closed += ctrl_Closed;
                            ctrl.Show();
                        }
                        else
                        {
                            UcSaisiQuantite ctrl = new UcSaisiQuantite(SelectedObject, true, taxe);
                            this.IsEnabled = false;
                            ctrl.Closed += ctrl_Closed;
                            ctrl.Show();
                        }
                    }
                    else
                        btn_Ajouter_Click(null, null);
                }
                lastClick = DateTime.Now;
            }
        }
        void ctrl_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
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
                        if (MaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement)
                            if (this.Chk_Extension.IsChecked == true)
                            {
                                ctrs.SelectedObject.FK_IDRUBRIQUEDEVIS = SessionObject.LstRubriqueDevis.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DEVISBRANCHEMENT).PK_ID;
                                ctrs.SelectedObject.RUBRIQUE  =SessionObject.Enumere.DEVISBRANCHEMENT;
                                ctrs.SelectedObject.ISEXTENSION = true;
                            }
                        else 
                            {
                                ctrs.SelectedObject.FK_IDRUBRIQUEDEVIS = SessionObject.LstRubriqueDevis.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DEVISDEXTENSION).PK_ID;
                                ctrs.SelectedObject.RUBRIQUE = SessionObject.Enumere.DEVISDEXTENSION;
                                ctrs.SelectedObject.ISEXTENSION = false ;
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
                    ctrs.SelectedObject.FK_IDRUBRIQUEDEVIS = this.Chk_Extension.IsChecked == false ?
                                                SessionObject.LstRubriqueDevis.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DEVISBRANCHEMENT).PK_ID :
                                                SessionObject.LstRubriqueDevis.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DEVISDEXTENSION).PK_ID;
                    lesElements.Add(ctrs.SelectedObject);
                }
                this.dataGridElementDevisFinal.ItemsSource = null;
                this.dataGridElementDevisFinal.ItemsSource = lesElements;

                this.Txt_TotalHt.Text = lesElements.Where(p => p.QUANTITE != null && p.QUANTITE != 0).ToList().Sum(t => t.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalTtc.Text = lesElements.Where(p => p.QUANTITE != null && p.QUANTITE != 0).ToList().Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalTva.Text = lesElements.Where(p => p.QUANTITE != null && p.QUANTITE != 0).ToList().Sum(t => t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
            }
        }

        private void dgMyDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
        }

        private void dataGridElementDevisFinal_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridElementDevisFinal.SelectedItem != null)
            {
                ObjELEMENTDEVIS leEltSelect = (ObjELEMENTDEVIS)dataGridElementDevisFinal.SelectedItem;
                if (leEltSelect.ISDEFAULT == true)
                    this.Btn_Retirer.Visibility = System.Windows.Visibility.Collapsed;
                else
                    this.Btn_Retirer.Visibility = System.Windows.Visibility.Visible ;
            }
        }
    }
}


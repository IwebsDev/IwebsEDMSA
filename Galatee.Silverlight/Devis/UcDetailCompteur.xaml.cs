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
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Devis
{
    public partial class UcDetailCompteur : ChildWindow
    {




        List<CsReglageCompteur> LstReglageCompteur;
        List<CsMarqueCompteur> LstMarque;
        List<CsCadran> LstCadran;
        int FK_IDCENTRE;

        CsCanalisation     CanalisationClientRecherche = new  CsCanalisation ();
        CsEvenement LsDernierEvenement = new CsEvenement();
        List<CsEvenement> LstEvenement = new List<CsEvenement>();
        List<CsCanalisation> LstCanalisation = new List<CsCanalisation>();
        List<CsTcompteur> LstType;

       List< CsTcompteur>  LeCompteurSelect  ;
        public event EventHandler Closed;
        List<CsCompteur> ObjectSelect = new List<CsCompteur>();
        public List<CsCompteur> MyObject { get; set; }

        public bool isOkClick;
        public bool GetisOkClick
        {
            set { isOkClick = value; }
            get { return isOkClick; }
        }

        CsDemande LaDemande = new CsDemande();
        public CsDemande MaDemande
        {
            get { return LaDemande; }
            set { LaDemande = value; }
        }    
        public UcDetailCompteur()
        {

            InitializeComponent();
        }
        public UcDetailCompteur(CsDemandeBase _LaDemande, List<CsCompteur> LstCompteur)
        {
            InitializeComponent();
            ChargerCompteur(LstCompteur);
        }
        public UcDetailCompteur( List<CsCompteur> LstCompteur)
        {
            InitializeComponent();
            ChargerCompteur(LstCompteur);
        }
        public void getElementCloseAfterSelection()
        {
            try
            {
                if (Closed != null)
                {
                    foreach (CsCompteur obj in dataGrid1.SelectedItems)
                    {
                        ObjectSelect.Add(obj);
                        
                    }
                    MyObject = ObjectSelect ;
                    Closed(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ChargerCompteur(List<CsCompteur> LstCompteur)
        {
            dataGrid1.ItemsSource = LstCompteur;
            if (LstCompteur == null || LstCompteur.Count == 0)
                Message.ShowInformation("Aucun compteur disponible pour le calibre sélectionné", "Liaison");
        }
     
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LeCompteurSelect = new List<CsTcompteur>();
            LeCompteurSelect = this.dataGrid1.SelectedItem as List<CsTcompteur>;
        }
       
        void _ctrl_Closed(object sender, EventArgs e)
        {
            UcLiasonCompteur ctr = sender as UcLiasonCompteur;
            List<CsCompteur> _LeCompteurSelectDatagrid = new List<CsCompteur>();
            List<CsTcompteur> _LeCompteurSelect = new List<CsTcompteur>();

            

            _LeCompteurSelectDatagrid = (List<CsCompteur>)ctr.dgDemande.SelectedItem;
            foreach (CsCompteur cpt in _LeCompteurSelectDatagrid)
            {
                CsTcompteur cp= new CsTcompteur()
                {
                    PK_ID = cpt.PK_ID,
                    COMPTEUR = cpt.NUMERO,
                    MCOMPT = cpt.MARQUE,
                    SAISIE = "Oui"

            };
                _LeCompteurSelect.Add(cp);
            }

        }
        

        void dialogue_Closed(object sender, EventArgs e)
        {
            DialogResult ctrs = sender as DialogResult;
            if (ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
            {
                ctrs.DialogResult = false;
                return;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.DialogResult = true;
                isOkClick = true;

                    getElementCloseAfterSelection();
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            isOkClick = false;
            Closed(this, new EventArgs());
            this.DialogResult = false;
        }

    }
}


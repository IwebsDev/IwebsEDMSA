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
using Galatee.Silverlight.ServiceCaisse;
//using Galatee.Silverlight.serviceWeb;

namespace Galatee.Silverlight.Caisse
{
    public partial class FrmPayementAnticipe : ChildWindow
    {
     
        string LibelleTypeFacture = string.Empty;
        public CsClient  LeClientNaf { get; set; }
        public CsLclient leNaf = new CsLclient();
       
        string nomAbon = string.Empty;
        decimal? solde = null;
        int debutClient = SessionObject.Enumere.TailleCentre;
        int debutOrdre = SessionObject.Enumere.TailleCentre + SessionObject.Enumere.TailleClient;
        string referenceClient = string.Empty;
        public event EventHandler Closed;

        public FrmPayementAnticipe()
        {
            InitializeComponent();
        }
 
        public FrmPayementAnticipe(CsClient  LeClient,string _LibelleTypeFacture)
        {
            InitializeComponent();
            try
            {
                LeClientNaf = LeClient;
                LibelleTypeFacture = _LibelleTypeFacture;
                nomAbon = LeClient.NOMABON ;
                solde = LeClient.SOLDE ;
                referenceClient =LeClient.CENTRE + LeClient.REFCLIENT + LeClient.ORDRE ;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        private void InitCtr( )
        {
            try
            {
                this.Txt_TypeFacture.Text = LibelleTypeFacture;
                this.Txt_Periode.Text = DateTime.Now.Year.ToString("0000")+DateTime.Now.Month.ToString("00");
                this.Txt_Periode.IsEnabled = false;
                //this.btn_Ajouter.IsEnabled = false;
            }
            catch (Exception ex)
            {
               Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreationPayementAnticipe(LeClientNaf);
                Closed(this, new EventArgs());
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void CreationPayementAnticipe(CsClient  LeClient)
        {
            try
            {
                
                    CsLclient FactureSelect = new CsLclient();

                    FactureSelect.CENTRE = LeClient.CENTRE ;
                    FactureSelect.CLIENT = LeClient.REFCLIENT   ;
                    FactureSelect.ORDRE = LeClient.ORDRE  ;

                    FactureSelect.REFEM = DateTime.Today.Year.ToString("0000") + DateTime.Today.Month.ToString("00");
                    FactureSelect.LIBELLENATURE = "FPA";
                    FactureSelect.LIBELLECOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperNAF).LIBELLE;
                    FactureSelect.COPER = SessionObject.Enumere.CoperNAF;
                    FactureSelect.DC = SessionObject.Enumere.Debit;
                    FactureSelect.MATRICULE  = UserConnecte.matricule;
                    FactureSelect.CAISSE = SessionObject.LePosteCourant.NUMCAISSE ;
                    FactureSelect.Selectionner = true;
                    FactureSelect.SOLDEFACTURE = Convert.ToDecimal(this.Txt_Montant.Text); ;
                    FactureSelect.NOM = nomAbon;
                    FactureSelect.SOLDECLIENT = solde.Value;
                    FactureSelect.USERCREATION = UserConnecte.matricule;
                    FactureSelect.DATECREATION = DateTime.Now;
                    FactureSelect.MONTANT = Convert.ToDecimal(this.Txt_Montant.Text);
                    FactureSelect.SOLDEFACTURE = Convert.ToDecimal(this.Txt_Montant.Text);
                    FactureSelect.MONTANTPAYE = Convert.ToDecimal(this.Txt_Montant.Text);
                    FactureSelect.IsPAIEMENTANTICIPE  = true ;

                    FactureSelect.FK_IDCENTRE = LeClient.FK_IDCENTRE.Value;
                    FactureSelect.FK_IDCLIENT = LeClient.PK_ID;
                    FactureSelect.FK_IDCOPER = SessionObject.LstDesCopers.First(t => t.CODE == FactureSelect.COPER).PK_ID;
                    FactureSelect.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                    FactureSelect.FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.First(t => t.CODE == SessionObject.Enumere.TopCaisse).PK_ID;
                    FactureSelect.FK_IDHABILITATIONCAISSE = SessionObject.LaCaisseCourante.PK_ID;
                    FactureSelect.FK_IDCAISSIERE = SessionObject.LaCaisseCourante.FK_IDCAISSIERE;
                    FactureSelect.FK_IDAGENTSAISIE = null;
                    FactureSelect.FK_IDPOSTECLIENT = null;
                    leNaf  = FactureSelect;
                    //FactureClient.Add(FactureSelect);
              
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

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitCtr();
        }



        private void Txt_Montant_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal montantSaisi = 0;
                if (decimal.TryParse(Txt_Montant.Text, out montantSaisi))
                    OKButton.IsEnabled = true;
                else
                    OKButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

    }
}


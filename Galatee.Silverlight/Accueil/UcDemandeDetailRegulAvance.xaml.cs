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
using Galatee.Silverlight.Resources.Accueil ;
using Galatee.Silverlight.Shared;


namespace Galatee.Silverlight.Accueil
{
    public partial class UcDemandeDetailRegulAvance : UserControl
    {

        List<CsTcompteur> LstTypeCompteur = new List<CsTcompteur>();
        List<CsDiacomp> LstDiametre = new List<CsDiacomp>();
        List<CsMarqueCompteur > LstMarque = new List<CsMarqueCompteur>();
        List<CsCadran > LstCadran = new List<CsCadran>() ;

        CsTcompteur LeTypeCompteurSelect= new CsTcompteur();
        CsDiacomp LeDiametreSelect = new CsDiacomp();
        CsMarqueCompteur LeMarqueSelect = new CsMarqueCompteur();
        CsCadran LeCadranSelect = new CsCadran();


        CsCanalisation CanalisationAfficher= new CsCanalisation();
        CsEvenement EvenementAfficher = new CsEvenement();
        List<CsEvenement> LstEvenement;

        CsEvenement LsDernierEvenement = new CsEvenement();
        int NumCompteur = 0;
        decimal initValue = 0;
        bool IsUpdate = false;
        public  CsDemande LaDemande = new CsDemande();
        public UcDemandeDetailRegulAvance()
        {
            InitializeComponent();
        }

        public UcDemandeDetailRegulAvance(CsDemande _LaDemande,bool _IsUpdate)
        {
            InitializeComponent();
            Translate();
            LaDemande = _LaDemande;
            if (LaDemande.Abonne == null)
                LaDemande.Abonne = new CsAbon();
            IsUpdate = _IsUpdate;
            AfficherInfoAbonnement(LaDemande.Abonne);

            
        }

        private void AfficherInfoAbonnement(CsAbon _LeAbonnementdemande)
        {
            try
            {
                this.Txt_MontantAncAvance.Text = string.IsNullOrEmpty(_LeAbonnementdemande.AVANCE.ToString()) ? string.Empty : Convert.ToDecimal(_LeAbonnementdemande.AVANCE).ToString(SessionObject.FormatMontant);
                this.Txt_DateAncAvance .Text = _LeAbonnementdemande.DAVANCE == null   ? string.Empty : Convert.ToDateTime( _LeAbonnementdemande.DAVANCE).ToShortDateString() ;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void Translate()
        {

        }
        public void EnregisterDemande(CsDemande _Lademande)
        {
            try
            {
                _Lademande.Abonne.NUMDEM = string.IsNullOrEmpty(_Lademande.LaDemande.NUMDEM) ? string.Empty : _Lademande.LaDemande.NUMDEM;
                _Lademande.Abonne.AVANCE  =Convert.ToDecimal( this.Txt_MontantNouvAvance .Text);
                _Lademande.Abonne.DAVANCE = null;
                if (!string.IsNullOrEmpty(this.Txt_DateNouvAvance.Text)) 
                _Lademande.Abonne.DAVANCE = Convert.ToDateTime(Txt_DateNouvAvance.Text);
               
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }
        void InitialiseCtrl()
        {
            try
            {
               

                this.Txt_Client.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
                this.Txt_Addresse.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
                this.Txt_Ordre.Text = string.IsNullOrEmpty(LaDemande.LaDemande.ORDRE) ? string.Empty : LaDemande.LaDemande.ORDRE;
       
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitialiseCtrl();
        }
    }
}

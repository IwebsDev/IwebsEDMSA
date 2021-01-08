using Galatee.Silverlight.MainView;
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.ServiceInterfaceComptable;
using Galatee.Silverlight.ServiceTarification;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Tarification.Helper;
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

namespace Galatee.Silverlight.Parametrage
{
    public partial class FrmCompteSpecifique : ChildWindow
    {

        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();

        protected virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion

        #region Variables
        public CsCompteSpecifique CsCompteSpecifique = new CsCompteSpecifique();
        public bool p = false;
         
        #endregion

        #region Constructeurs

        public FrmCompteSpecifique()
        {
            InitializeComponent();

            this.CsCompteSpecifique = new CsCompteSpecifique();
            //this.CsCompteSpecifique.TRANCHETypeCompte = new List<ServiceTarification.CsTrancheRedevence>();
            this.txt_code.MaxLength = 10;
            LayoutRoot.DataContext = this.CsCompteSpecifique;
            
        
        }

        public FrmCompteSpecifique(CsCompteSpecifique CsCompteSpecifique)
        {
            InitializeComponent();

               
            // TODO: Complete member initialization
            this.CsCompteSpecifique = CsCompteSpecifique;

            //DataBinding de la redevence au context de la fenetre
            LayoutRoot.DataContext = this.CsCompteSpecifique;

            //Mise de la fenetre en lecture 
            InitializeScreenConsultation();

        }

        public FrmCompteSpecifique(CsCompteSpecifique CsCompteSpecifique, bool p)
        {
            InitializeComponent();


              
            // TODO: Complete member initialization
            this.CsCompteSpecifique = CsCompteSpecifique;
            this.p = p;

            //DataBinding de la redevence au context de la fenetre
            LayoutRoot.DataContext = this.CsCompteSpecifique;

            //Mise de la fenetre en lecture 
            InitializeScreenModification();

        }

        #endregion

        #region Methodes d'interface
        private void InitializeScreenConsultation()
        {
            txt_code.IsReadOnly = true;
            txt_champ_Filtre.IsReadOnly = true;
            txt_libelle.IsReadOnly = true;
            chbx_Direction.IsEnabled = false;
            OKButton.IsEnabled = false;


        }

        private void InitializeScreenModification()
        {
            txt_code.IsReadOnly = true;
        }

        #endregion

        #region Events Handlers

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.CsCompteSpecifique.CODE) && !string.IsNullOrWhiteSpace(this.CsCompteSpecifique.LIBELLE.ToString()) && !string.IsNullOrWhiteSpace(this.CsCompteSpecifique.VALEURFILTRE.ToString()))
            {
                this.CsCompteSpecifique.LIBELLE = this.txt_libelle.Text;
                this.CsCompteSpecifique.VALEURFILTRE = this.txt_champ_Filtre.Text;
                //this.CsCompteSpecifique.AVECFILTRE = chbx_EstFacture.IsEnabled;
                this.CsCompteSpecifique.USERCREATION = UserConnecte.matricule;
                this.CsCompteSpecifique.USERMODIFICATION = UserConnecte.matricule;
                this.CsCompteSpecifique.DATECREATION = DateTime.Now;
                this.CsCompteSpecifique.DATEMODIFICATION = DateTime.Now;;

                MyEventArg.Bag = this.CsCompteSpecifique;
                OnEvent(MyEventArg);


            }
            else
            {
                Message.ShowInformation("Tous les champs sont obligatoire   ", "Information");
            }
            this.DialogResult = true;
        }
            
            private void CancelButton_Click(object sender, RoutedEventArgs e)
                {
                    this.DialogResult = false;
                }

            private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
            {
                this.CsCompteSpecifique.CODE = txt_code.Text;
                if (this.p==false)
                {
                    if (txt_code.Text.Length <= 10)
                    {
                        CheickCodeCompteSepecifiqueExist();
                    }
                    else
                    {
                        Message.ShowInformation("Veuillez vous assurer que le code est sur deux position", "Information");
                    }
                }
            }

            private void txt_libelle_TextChanged(object sender, TextChangedEventArgs e)
            {
                this.CsCompteSpecifique.LIBELLE = txt_libelle.Text;
            }

        #endregion

        #region Servies

            private void CheickCodeCompteSepecifiqueExist()
            {
                try
                {
                    InterfaceComptableServiceClient service1 = new InterfaceComptableServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("InterfaceComptable"));
                    service1.CheickCodeCompteSepecifiqueExistCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        if (res.Result == false)
                        {
                            Message.ShowError("Ce code exite deja,Veuillez le modifier", "Avertissement");
                            txt_code.Text = string.Empty;
                            txt_code.Focus();
                        }

                    };
                    service1.CheickCodeCompteSepecifiqueExistAsync(txt_code.Text.Trim());
                    service1.CloseAsync();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "ChargerListeDeProduit");
                }
            }

        #endregion






    }
}


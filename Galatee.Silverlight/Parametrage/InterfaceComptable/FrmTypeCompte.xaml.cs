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
    public partial class FrmTypeCompteCompte : ChildWindow
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
        public CsTypeCompte csTypeCompte = new CsTypeCompte();
        public bool p = false;
         
        #endregion

        #region Constructeurs

        public FrmTypeCompteCompte()
        {
            InitializeComponent();

            this.csTypeCompte = new CsTypeCompte();
            //this.csTypeCompte.TRANCHETypeCompte = new List<ServiceTarification.CsTrancheRedevence>();
            this.txt_code.MaxLength = 10;
            LayoutRoot.DataContext = this.csTypeCompte;
        
        }

        public FrmTypeCompteCompte(CsTypeCompte csTypeCompte)
        {
            InitializeComponent();

               
            // TODO: Complete member initialization
            this.csTypeCompte = csTypeCompte;

            //DataBinding de la redevence au context de la fenetre
            LayoutRoot.DataContext = this.csTypeCompte;

            //Mise de la fenetre en lecture 
            InitializeScreenConsultation();

        }

        public FrmTypeCompteCompte(CsTypeCompte csTypeCompte, bool p)
        {
            InitializeComponent();


              
            // TODO: Complete member initialization
            this.csTypeCompte = csTypeCompte;
            this.p = p;

            //DataBinding de la redevence au context de la fenetre
            LayoutRoot.DataContext = this.csTypeCompte;

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
            chbx_EstFacture.IsEnabled = false;
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
            if (!string.IsNullOrWhiteSpace(this.csTypeCompte.CODE) && !string.IsNullOrWhiteSpace(this.csTypeCompte.LIBELLE.ToString()) && !string.IsNullOrWhiteSpace(this.csTypeCompte.TABLEFILTRE.ToString()))
            {
                this.csTypeCompte.LIBELLE = this.txt_libelle.Text;
                this.csTypeCompte.TABLEFILTRE = this.txt_champ_Filtre.Text;
                this.csTypeCompte.AVECFILTRE = chbx_EstFacture.IsEnabled;
                this.csTypeCompte.USERCREATION = UserConnecte.matricule;
                this.csTypeCompte.USERMODIFICATION = UserConnecte.matricule;
                this.csTypeCompte.DATECREATION = DateTime.Now;
                this.csTypeCompte.DATEMODIFICATION = DateTime.Now;;

                MyEventArg.Bag = this.csTypeCompte;
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
                this.csTypeCompte.CODE = txt_code.Text;
                if (this.p==false)
                {
                    if (txt_code.Text.Length <= 10)
                    {
                        CheickCodeTypeComteExist();
                    }
                    else
                    {
                        Message.ShowInformation("Veuillez vous assurer que le code est sur deux position", "Information");
                    }
                }
            }

            private void txt_libelle_TextChanged(object sender, TextChangedEventArgs e)
            {
                this.csTypeCompte.LIBELLE = txt_libelle.Text;
            }

        #endregion

        #region Servies

            private void CheickCodeTypeComteExist()
            {
                try
                {
                    InterfaceComptableServiceClient service1 = new InterfaceComptableServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("InterfaceComptable"));
                    service1.CheickCodeTypeComteExistCompleted += (sr, res) =>
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
                    service1.CheickCodeTypeComteExistAsync(txt_code.Text.Trim());
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


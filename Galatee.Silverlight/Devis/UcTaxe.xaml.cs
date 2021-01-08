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
using Galatee.Silverlight.Resources.Devis;

namespace Galatee.Silverlight.Devis
{
    public partial class UcTaxe : ChildWindow
    {
        private CsDemandeBase ObjetDevisSelectionne = null;
        public CsCtax Taxe { get; set; }
        private CsCtax tauxSubvention = new CsCtax();

        public UcTaxe()
        {
            InitializeComponent();
        }

        public UcTaxe(CsDemandeBase pDemande)
        {
            InitializeComponent();
            ObjetDevisSelectionne = pDemande;
            Taxe = new CsCtax();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if(Cbo_Taxe.SelectedItem != null)
                this.Taxe = (this.Cbo_Taxe.SelectedItem as CsCtax);
            this.DialogResult = true;
            //this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void RemplirComboTaxe()
        {
            try
            {
                try
                {
                    RetouneListeDesTaxes();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void LoadComboTaxe()
        //{
        //    int loaderHandler = LoadingManager.BeginLoading(Galatee.Silverlight.Resources.Devis.Languages.Data_Loading);
        //    DevisServiceClient client = new DevisServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Devis"));
        //    client.GetAllCtaxCompleted += (ssender, args) =>
        //    {
        //        if (args.Cancelled || args.Error != null)
        //        {
        //            string error = args.Error.Message;
        //            Message.Show(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
        //            return;
        //        }
        //        SessionObject.LstDesTaxeDevis = args.Result;
        //        FillComboxTaxe(SessionObject.LstDesTaxeDevis);
        //        LoadingManager.EndLoading(loaderHandler);
        //    };
        //    client.GetAllCtaxAsync();
        //}
        private void RetouneListeDesTaxes()
        {
            if (SessionObject.LstDesTaxe != null && SessionObject.LstDesTaxe.Count  != 0)
            {
                List<CsCtax> lstTournee = SessionObject.LstDesTaxe;
                FillComboxTaxe(lstTournee);
                return;
            }
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneListeTaxeCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstDesTaxe = args.Result;
                List<CsCtax> lstTournee =  SessionObject.LstDesTaxe;
                FillComboxTaxe(lstTournee.Where(t=>t.CENTRE ==ObjetDevisSelectionne.CENTRE && t.FK_IDCENTRE == ObjetDevisSelectionne.FK_IDCENTRE ).ToList());
                return;
            };
            service.RetourneListeTaxeAsync();
            service.CloseAsync();
        }
        private void FillComboxTaxe(List<CsCtax> args)
        {
            
            Cbo_Taxe.Items.Clear();
            if (args != null && args.Count > 0)
                Cbo_Taxe.ItemsSource = args.Where(t => t.CENTRE == ObjetDevisSelectionne.CENTRE && t.FK_IDCENTRE == ObjetDevisSelectionne.FK_IDCENTRE).ToList();
            Cbo_Taxe.SelectedValuePath = "PK_CTAX";
            Cbo_Taxe.DisplayMemberPath = "LIBELLE";
            foreach (CsCtax st in args.Where(t => t.CENTRE == ObjetDevisSelectionne.CENTRE && t.FK_IDCENTRE == ObjetDevisSelectionne.FK_IDCENTRE).ToList())
            {
                if ((this.Taxe.TAUX != (decimal)0) && (this.Taxe.TAUX == st.TAUX))
                {
                    this.Cbo_Taxe.SelectedItem = st;
                    break;
                }
                if (st.TAUX == (decimal)0)
                    tauxSubvention = st;
            }
            //if (this.ObjetDevisSelectionne.ISSUBVENTION)
            //{
            //    this.Cbo_Taxe.SelectedItem = this.tauxSubvention;
            //    this.Cbo_Taxe.IsEnabled = true;
            //}

       
        }

        private void Cbo_Taxe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.OKButton.IsEnabled = (this.Cbo_Taxe.SelectedIndex != -1);
                if (Cbo_Taxe.SelectedItem != null)
                {
                    var ctax = this.Cbo_Taxe.SelectedItem as CsCtax;
                    if(ctax.TAUX != null)
                    this.Txt_TauxTaxe.Text = ((decimal)ctax.TAUX).ToString("N2");
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RemplirComboTaxe();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
    }
}


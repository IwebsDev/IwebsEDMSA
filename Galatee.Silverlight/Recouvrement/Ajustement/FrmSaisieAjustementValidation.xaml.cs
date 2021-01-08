using Galatee.Silverlight.ServiceRecouvrement;
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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmSaisieAjustementValidation : ChildWindow
    {

        #region Service

        private void MiseAjourCompt(List<CsDetailLot> lstDetailPaiement, int Id)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.MiseAjourComptAjustementCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                    return;
                if (args.Result.Count == 0)
                    return;
                Message.Show("Mise à jour effectuée avec succès", "Resultat");
                Dictionary<string, string> param = new Dictionary<string, string>();
                var lesCompteMaj = args.Result.Select(u => new { u.CENTRE, u.CLIENT, u.ORDRE, u.NOM, u.REFEM, u.NDOC }).Distinct();
                List<CsLclient> lesCompteAEditer = new List<CsLclient>();
                int i = 0;
                foreach (var item in lesCompteMaj)
                {
                    CsLclient leC = new CsLclient();
                    leC.CENTRE = item.CENTRE;
                    leC.CLIENT = item.CLIENT;
                    leC.ORDRE = item.ORDRE;
                    leC.REFEM = item.REFEM;
                    leC.NDOC = item.NDOC;
                    leC.NOM = item.NOM;
                    //leC.NUMDEM = item.NUMDEM;
                    //leC.NUMDEVIS = item.NUMDEVIS;
                    leC.NUMETAPE = i + 1;
                    //leC.ADRESSE = txt_Campagne.Text;
                    leC.CAPUR = System.DateTime.Today.ToShortDateString();
                    leC.MONTANT = args.Result.Where(t => t.CENTRE == item.CENTRE && t.CLIENT == item.CLIENT && t.ORDRE == item.ORDRE && t.REFEM == item.REFEM && t.NDOC == item.NDOC).Sum(t => t.MONTANT);

                    lesCompteAEditer.Add(leC);
                }
                param.Add("pUserMiseAJour", UserConnecte.nomUtilisateur);
                Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(lesCompteAEditer, param, SessionObject.CheminImpression, "MiseAJourGrandCompte", "Recouvrement", true);
                this.DialogResult = true;
                //List<int> id = new List<int>();
                //id.Add(Id);
                //EnvoyerDemandeEtapeSuivante(id);
                return;
            };
            service.MiseAjourComptAjustementAsync(lstDetailPaiement.Where(d => d.STATUT == true).ToList(), Id);
        }

        #endregion

        #region Methodes
        private void LoadDataPager<T>(object ItemsSource, DataPager datapager_, DataGrid dg)
        {
            if (ItemsSource != null)
            {
                if ((ItemsSource as List<T>) != null)
                {

                    System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ItemsSource as List<T>);
                    dg.ItemsSource = null;
                    dg.ItemsSource = view.SourceCollection;
                    datapager_.Source = view;
                }
            }

        }
        private void InitializeScreen(bool IsConsultation)
        {
            if (this.csLotComptClient.DC == "D")
            {
                rb_Debit.IsChecked = true;
            }
            else
            {
                rb_Credit.IsChecked = true;
            }
           
            dg_facture_Copy.IsReadOnly = IsConsultation;
            dg_facture_Copy.IsEnabled = !IsConsultation;
            
            btn_ajouterFactureHorReg.IsEnabled = !IsConsultation;
            btn_ajouterFactureHorReg_Copy.IsEnabled = !IsConsultation;
            btn_ajouterFactureHorReg_Copy1.IsEnabled = !IsConsultation;
            btn_ajouterFactureHorReg_Copy2.IsEnabled = !IsConsultation;

            List<CsDetailLot> datasource = ListFactureInitiale.Where(d => d.STATUT == true) != null ? ListFactureInitiale.Where(d => d.STATUT == true).ToList() : new List<CsDetailLot>();
            LoadDataPager<CsDetailLot>(datasource, datapager_Copy, dg_facture_Copy);
            txt_TotalFactureEnvoie.Text = datasource.Sum(c => (c.MONTANT_AJUSTEMENT != null ? c.MONTANT_AJUSTEMENT : 0)).Value.ToString(SessionObject.FormatMontant);

        }

        #endregion

        #region Variables
        private CsLotComptClient csLotComptClient;
        private bool? p=null;
        List<CsDetailLot> ListFacture_Selectionner = new List<CsDetailLot>();
        List<CsDetailLot> ListFactureInitiale = new List<CsDetailLot>();
        bool Direction = false;
        #endregion

        #region Constructeur

        public FrmSaisieAjustementValidation()
        {
            InitializeComponent();
            this.csLotComptClient = new CsLotComptClient();
            this.csLotComptClient.DetaiLot = new List<CsDetailLot>();

            rb_Debit.IsChecked = true;
        }
        public FrmSaisieAjustementValidation(CsLotComptClient csLotComptClient)
        {
            InitializeComponent();

            // TODO: Complete member initialization
            this.csLotComptClient = csLotComptClient;
            this.ListFactureInitiale = csLotComptClient.DetaiLot;

            LayoutRoot.DataContext = this.csLotComptClient;
            this.p = true;
            txt_TotalFactureEnvoie_Copy1.IsReadOnly = true;
            txt_TotalFactureEnvoie.IsReadOnly = true;
            rb_Credit.IsEnabled = false;
            rb_Debit.IsEnabled = false;
            //Mise de la fenetre en lecture 
            InitializeScreen(this.p.Value);
        }
        public FrmSaisieAjustementValidation(CsLotComptClient csLotComptClient, bool p)
        {
            InitializeComponent();

            // TODO: Complete member initialization
            this.csLotComptClient = csLotComptClient;
            this.p = p;
            this.ListFactureInitiale = csLotComptClient.DetaiLot;
            LayoutRoot.DataContext = this.csLotComptClient;

            //ListFacture_Selectionner = csLotComptClient.DetaiLot;
            txt_TotalFactureEnvoie_Copy1.IsReadOnly = true;
            txt_TotalFactureEnvoie.IsReadOnly = true;
            rb_Credit.IsEnabled = false;
            rb_Debit.IsEnabled = false;
            //Mise de la fenetre en lecture 
            InitializeScreen(this.p.Value);
        }

        #endregion

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

        #region Events Handler

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.csLotComptClient.NUMEROLOT) )
            {
                this.csLotComptClient.DATECREATION = DateTime.Now;
                this.csLotComptClient.DATEMODIFICATION = DateTime.Now;
                this.csLotComptClient.DC = rb_Debit.IsChecked.Value?"D":"C";
                this.csLotComptClient.DetaiLot = ListFactureInitiale;
                this.csLotComptClient.MOISCOMPTABLE = DateTime.Now.Month.ToString("D2");
                this.csLotComptClient.MONTANT = !string.IsNullOrEmpty(txt_TotalFactureEnvoie.Text) ? decimal.Parse(txt_TotalFactureEnvoie.Text) : 0;
                this.csLotComptClient.NUMEROLOT = this.p == null ? txt_TotalFactureEnvoie_Copy1.Text + "_" + DateTime.Now : this.csLotComptClient.NUMEROLOT;
                this.csLotComptClient.STATUS = "1";
                this.csLotComptClient.USERCREATION = UserConnecte.matricule;
                this.csLotComptClient.USERMODIFICATION = UserConnecte.matricule;
                
                
                MyEventArg.Bag = this.csLotComptClient;
                OnEvent(MyEventArg);
                this.DialogResult = true;

            }
            else
            {
                Message.ShowInformation("Veuillez vous assurer que tous les champs obligatoire sont renseignés", "Information");
            }

            
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            ListFacture_Selectionner.Add((CsDetailLot)dg_facture_Copy.SelectedItem);
        }
        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            ListFacture_Selectionner.Remove((CsDetailLot)dg_facture_Copy.SelectedItem);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FrmFactureHorRegroupement frm = new FrmFactureHorRegroupement();
            frm.CallBack += frm_CallBack1;
            frm.Show();
        }
        void frm_CallBack1(object sender, Tarification.Helper.CustumEventArgs e)
        {
            //Implementer le callback
            if (e.Bag != null)
            {
                var ListFacture = Utility.ConvertListTypeByMaping<CsDetailLot, ServiceRecouvrement.CsLclient>((List<ServiceRecouvrement.CsLclient>)e.Bag);


                //List<CsDetailLot> datasource = (List<CsDetailLot>)dg_facture_Copy.ItemsSource;
                //List<CsDetailLot> datasource = ListFactureInitiale;
                //if (datasource == null)
                //{
                //    datasource = new List<CsDetailLot>();
                //}
                foreach (var item in ListFacture)
                {
                    if (ListFactureInitiale.FirstOrDefault(f => f.CENTRE == item.CENTRE && f.CLIENT == item.CLIENT && f.ORDRE == item.ORDRE && f.NDOC == item.NDOC) == null)
                    {
                        item.STATUT = true;
                        item.PK_ID = 0;
                        //datasource.Add(item);
                        ListFactureInitiale.Add(item);
                    }
                    else
                    {
                        var d = ListFactureInitiale.FirstOrDefault(f => f.CENTRE == item.CENTRE && f.CLIENT == item.CLIENT && f.ORDRE == item.ORDRE && f.NDOC == item.NDOC);
                        int index = ListFactureInitiale.IndexOf(d);
                        d.STATUT = true;
                        ListFactureInitiale[index] = d;
                    }
                }
                //dg_facture_Copy.ItemsSource = datasource;
                List<CsDetailLot> datasource = ListFactureInitiale.Where(d => d.STATUT == true)!=null?ListFactureInitiale.Where(d => d.STATUT == true).ToList():new List<CsDetailLot>();
                LoadDataPager<CsDetailLot>(datasource, datapager_Copy, dg_facture_Copy);

                txt_TotalFactureEnvoie.Text = datasource.Sum(c => (c.MONTANT_AJUSTEMENT != null ? c.MONTANT_AJUSTEMENT : 0)).Value.ToString(SessionObject.FormatMontant);
                //txt_TotalFacture.Text = dg_facture.ItemsSource != null ? ((List<CsLotComptClient>)dg_facture.ItemsSource).Sum(c => (c.SOLDEFACTURE != null ? c.SOLDEFACTURE : 0)).Value.ToString(SessionObject.FormatMontant) : 0.ToString(SessionObject.FormatMontant);
            }
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            Direction = true;
        }
        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            Direction = false;

        }

        private void btn_ajouterFactureHorReg_Copy_Click_1(object sender, RoutedEventArgs e)
        {
            //List<CsDetailLot> datasource =((List<CsDetailLot>)dg_facture_Copy.ItemsSource) ;

            foreach (var p in ListFacture_Selectionner)
            {
                p.STATUT = false;
                int index = ListFactureInitiale.IndexOf(p);
                ListFactureInitiale[index] = p;
            }
            List<CsDetailLot> datasource = ListFactureInitiale.Where(d => d.STATUT == true) != null ? ListFactureInitiale.Where(d => d.STATUT == true).ToList() : new List<CsDetailLot>();
            LoadDataPager<CsDetailLot>(datasource, datapager_Copy, dg_facture_Copy);

            txt_TotalFactureEnvoie.Text = datasource.Sum(c => (c.MONTANT_AJUSTEMENT != null ? c.MONTANT_AJUSTEMENT : 0)).Value.ToString(SessionObject.FormatMontant);
            ListFacture_Selectionner = new List<CsDetailLot>();
        }
        private void txt_TotalFactureEnvoie_Copy1_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.csLotComptClient.NUMEROLOT = txt_TotalFactureEnvoie_Copy1.Text;
        }

        private void bnt_maj_compt_Click(object sender, RoutedEventArgs e)
        {
            MiseAjourCompt(ListFactureInitiale, this.csLotComptClient.PK_ID);
        }

        private void dg_facture_Copy_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {
            List<CsDetailLot> datasource = ListFactureInitiale.Where(d => d.STATUT == true) != null ? ListFactureInitiale.Where(d => d.STATUT == true).ToList() : new List<CsDetailLot>();
            LoadDataPager<CsDetailLot>(datasource, datapager_Copy, dg_facture_Copy);

            txt_TotalFactureEnvoie.Text = datasource.Sum(c => (c.MONTANT_AJUSTEMENT != null ? c.MONTANT_AJUSTEMENT : 0)).Value.ToString(SessionObject.FormatMontant);
        }

        #endregion
       
    }
}


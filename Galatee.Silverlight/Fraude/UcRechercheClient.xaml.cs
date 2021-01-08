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
using Galatee.Silverlight.Fraude;
using Galatee.Silverlight.Resources.Accueil ;
using Galatee.Silverlight.Resources.Devis;
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Galatee.Silverlight.ServiceFraude;
using Galatee.Silverlight.Tarification.Helper;
using Galatee.Silverlight.Resources.Fraude;
using Galatee.Silverlight.Resources.Parametrage;
namespace Galatee.Silverlight.Fraude
{
    public partial class UcRechercheClient : ChildWindow, INotifyPropertyChanged
    {
        #region Variables

        #region Listes
        private List<Galatee.Silverlight.ServiceAccueil.CsCentre> _listeDesCentreExistant = null;
        ObservableCollection<Galatee.Silverlight.ServiceAccueil.CsClient> donnesDatagrid = new ObservableCollection<Galatee.Silverlight.ServiceAccueil.CsClient>();
       List< Galatee.Silverlight.ServiceAccueil.CsClient>LeClientRecherche = new  List< Galatee.Silverlight.ServiceAccueil.CsClient >();
       public Galatee.Silverlight.ServiceAccueil.CsClient ObjetSelectionneClient { get; set; }
        #endregion

        #region Objets Globaux

        Galatee.Silverlight.ServiceAccueil.CsClient LeClient = new Galatee.Silverlight.ServiceAccueil.CsClient();
        Galatee.Silverlight.ServiceAccueil.CsCentre LeCentreSelect = new Galatee.Silverlight.ServiceAccueil.CsCentre();

        #endregion

        #endregion

        #region Constructeurs


        public UcRechercheClient()
        {
            InitializeComponent();
            this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
            ChargerDonneeDuSite();
            //GetData();
        }

        private void Txt_Client_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Client.Text))
                this.Txt_Client.Text = this.Txt_Client.Text.PadLeft(SessionObject.Enumere.TailleClient, '0');
        }
        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
            {
               Galatee.Silverlight.ServiceAccueil. CsCentre _LeCentre = ClasseMEthodeGenerique.RetourneObjectFromList<Galatee.Silverlight.ServiceAccueil.CsCentre>(LstCentrePerimetre, this.Txt_CodeCentre.Text, "CODE");
                if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                {
                    this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                    this.Txt_CodeCentre.Tag = _LeCentre.PK_ID;
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Recouvrement.Langue.lbltoMatricule, Galatee.Silverlight.Resources.Recouvrement.Langue.lblDueDate, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_CodeCentre.Focus();
                    };
                    w.Show();
                }
            }
        }
        string TypeDemande = string.Empty;


        #endregion
        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();

        public virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion
   
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<Galatee.Silverlight.ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre = new List<ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        List<int> lesCentreCaisse = new List<int>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);

                    if (lstSite != null && lstSite.Count != 0)
                    {
                        if (lstSite.Count == 1)
                        {
                            this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;
                            this.Txt_CodeSite.Text = lstSite.First().CODE;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                        }
                        if (LstCentrePerimetre != null && LstCentrePerimetre.Count != 0)
                        {
                            if (LstCentrePerimetre.Count == 1)
                            {
                                this.Txt_CodeCentre.Text = LstCentrePerimetre.First().CODE;
                                this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                                this.Txt_CodeCentre.Tag = LstCentrePerimetre.First().PK_ID;
                                this.btn_Centre.IsEnabled = false;
                            }
                        }
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentrePerimetre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentrePerimetre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentrePerimetre)
                        lesCentreCaisse.Add(item.PK_ID);
                    if (lstSite != null && lstSite.Count != 0)
                    {
                        if (lstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Tag = lstSite.First().PK_ID;
                            this.Txt_LibelleSite.Text = lstSite.First().LIBELLE;

                            this.btn_Site.IsEnabled = false;
                        }
                    }
                    if (LstCentrePerimetre != null && LstCentrePerimetre.Count != 0)
                    {
                        if (LstCentrePerimetre.Count == 1)
                        {
                            this.Txt_LibelleCentre.Text = LstCentrePerimetre.First().LIBELLE;
                            this.Txt_CodeCentre.Tag = LstCentrePerimetre.First().PK_ID;

                            this.btn_Centre.IsEnabled = false;
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentre.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(LstCentre);
                    UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeCentre);
                    ctr.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }
        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            this.btn_Centre.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsCentre leCentre = (Galatee.Silverlight.ServiceAccueil.CsCentre)ctrs.MyObject;
                this.Txt_CodeCentre.Text = leCentre.CODE;
                this.Txt_CodeCentre.Tag = leCentre.PK_ID;
            }
        }
   
        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite.Count > 0)
                {
                    this.btn_Site.IsEnabled = false;
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("CODE", "SITE");
                    _LstColonneAffich.Add("LIBELLE", "LIBELLE");

                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                    ctrl.Closed += new EventHandler(galatee_OkClickedSite);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "btn_Site_Click");
            }
        }
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODE;
                this.Txt_CodeSite.Tag = leSite.CODE;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
                LstCentre = LstCentrePerimetre.Where(t => t.CODESITE == this.Txt_CodeSite.Tag.ToString()).ToList();
                if (LstCentre.Count == 1)
                {
                    this.Txt_CodeCentre.Text = LstCentre.First().CODE;
                    this.Txt_LibelleCentre.Text = LstCentre.First().LIBELLE;
                    this.Txt_CodeCentre.Tag = LstCentre.First().PK_ID;
                }
            }
            else
                this.btn_Site.IsEnabled = true;
        }

   
       

        private void AfficherInformationClient(Galatee.Silverlight.ServiceAccueil.CsClient _LeClient)
        {
            DonnesDatagrid.Clear();
            DonnesDatagrid.Add(_LeClient);
            dtgrdClient.ItemsSource = DonnesDatagrid;
        }

        //private void GetData()
        //{
        //    try
        //    {
        //        FraudeServiceClient client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
        //        client.SelectAllClientCompleted += (ssender, args) =>
        //        {
        //            if (args.Cancelled || args.Error != null)
        //            {
        //                string error = args.Error.Message;
        //                throw new Exception(error);
        //            }
        //            if (args.Result == null)
        //            {
        //                Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, Galatee.Silverlight.Resources.Devis.Languages.lbl_Client);
        //                return;
        //            }
        //            DonnesDatagrid.Clear();
        //            foreach (var item in args.Result)
        //            {
        //                DonnesDatagrid.Add(item);
        //                LeClientRecherche.Add(item);

        //            }
        //            dtgrdClient.ItemsSource = DonnesDatagrid;
        //        };
        //        client.SelectAllClientAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void btn_Rechercher_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.Txt_CodeCentre.Tag != null && !string.IsNullOrEmpty(this.Txt_CodeCentre.Text) && !string.IsNullOrEmpty(this.Txt_Client.Text))
            {
                Galatee.Silverlight.ServiceAccueil.CsClient leClient = new Galatee.Silverlight.ServiceAccueil.CsClient();
                leClient.FK_IDCENTRE = (int)this.Txt_CodeCentre.Tag;
                leClient.CENTRE = this.Txt_CodeCentre.Text;
                leClient.REFCLIENT = this.Txt_Client.Text;
                leClient.ORDRE = this.Txt_Ordre.Text;
                RechercheClient(leClient);
            }
        }
        private void RechercheClient(Galatee.Silverlight.ServiceAccueil.CsClient leClient)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));

                //FraudeServiceClient service = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                service.RetournelstClientCompleted += (s, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    else if(args.Result.Count()<=0)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }

                    DonnesDatagrid.Clear();
                    foreach (var item in args.Result)
                    {
                        DonnesDatagrid.Add(item);
                    }
                    dtgrdClient.ItemsSource = DonnesDatagrid;
                };
                service.RetournelstClientAsync(leClient.FK_IDCENTRE.Value, leClient.CENTRE, leClient.REFCLIENT, leClient.ORDRE);
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

  
        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion


        public ObservableCollection<Galatee.Silverlight.ServiceAccueil.CsClient> DonnesDatagrid
        {
            get { return donnesDatagrid; }
            set
            {
                if (value == donnesDatagrid)
                    return;
                donnesDatagrid = value;
                NotifyPropertyChanged("DonnesDatagrid");
            }
        }

        private void Txt_CodeCentre_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            MyEventArg.Data = ObjetSelectionneClient;
            OnEvent(MyEventArg);
            this.DialogResult = false;
        }

        private void dtgrdClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObjetSelectionneClient = dtgrdClient.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsClient;
            OKButton.IsEnabled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
   
  
    }
}
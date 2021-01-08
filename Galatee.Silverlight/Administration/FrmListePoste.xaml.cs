using Galatee.Silverlight.Shared;
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

namespace Galatee.Silverlight.Administration
{
    public partial class FrmListePoste : ChildWindow
    {
        public FrmListePoste()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            this.Txt_CodeCentre.IsReadOnly = true;
            this.btn_Centre.IsEnabled = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<Galatee.Silverlight.ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        List<int> lstIdCentre = new List<int>();

        private void ChargerDonneeDuSite()
        {
            try
            {

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    this.btn_Centre.IsEnabled = true;
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lstIdCentre.Add(item.PK_ID);

                    if (lstSite != null)
                    {
                        if (lstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = lstSite[0].CODE;
                            this.Txt_LibelleSite.Text = lstSite[0].LIBELLE;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.Tag  = lstSite.First().PK_ID ;

                            this.Txt_CodeSite.IsReadOnly = true;
                        }
                    }
                    if (LstCentre != null)
                    {
                        if (LstCentre.Count == 1)
                        {
                            this.Txt_CodeCentre.Text = LstCentre[0].CODESITE;
                            this.Txt_CodeCentre.Tag  = LstCentre[0].PK_ID ;
                            this.Txt_LibelleCentre.Text = LstCentre[0].LIBELLE;
                            this.btn_Centre.IsEnabled = false;
                            this.Txt_CodeCentre.IsReadOnly = true;
                        }
                    }
                    RetournePoste(lstIdCentre);
                    RetourneListeCaisse(lstIdCentre);
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RetournePoste(List<int> lstIdCentrePerimetre)
        {
            //if (SessionObject.ListePoste != null && SessionObject.ListePoste.Count != 0)
            //{
            //    SessionObject.ListePoste.ForEach(t => t.IsSelect = false);
            //    List<ServiceAdministration.CsPoste> lstPoste =SessionObject.ListePoste.Where(t => lstIdCentrePerimetre.Contains(t.FK_IDCENTRE.Value)).ToList();
            //    dtg_Poste.ItemsSource = null;
            //    dtg_Poste.ItemsSource = lstPoste;
            //    return;
            //}
            Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient prgram = new Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            prgram.RetourneListePosteCompleted += (sprog, resprog) =>
            {
                try
                {
                    if (resprog.Cancelled || resprog.Error != null)
                    {
                        string error = resprog.Error.Message;
                    }
                    if (resprog.Result == null || resprog.Result.Count == 0)
                    {
                    }
                    else
                    {
                        dtg_Poste.ItemsSource = null;
                        SessionObject.ListePoste = resprog.Result;
                        List<ServiceAdministration.CsPoste> lstPoste = SessionObject.ListePoste.Where(t => lstIdCentrePerimetre.Contains(t.FK_IDCENTRE.Value)).ToList();
                        dtg_Poste.ItemsSource = lstPoste;
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "RetournePoste");
                }

            };
            prgram.RetourneListePosteAsync();
        }

        private void RetourneListeCaisse(List<int> lstIdCentrePerimetre)
        {
            ServiceCaisse.CaisseServiceClient service = new ServiceCaisse.CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.RetourneListeCaisseAsync();
            service.RetourneListeCaisseCompleted += (sprog, resprog) =>
            {
                try
                {
                    if (resprog.Cancelled || resprog.Error != null)
                    {
                        string error = resprog.Error.Message;
                    }
                    if (resprog.Result == null || resprog.Result.Count == 0)
                    {
                    }
                    else
                        SessionObject.ListeCaisse = resprog.Result.Where(t=>lstIdCentrePerimetre.Contains(t.FK_IDCENTRE)).ToList();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, "RetournePoste");
                }

            };
        }

        DateTime lastClick = DateTime.Now;
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGrid dg = (sender as DataGrid);
                var allObjects = (List<ServiceAdministration.CsPoste>)dg.ItemsSource;
                foreach (var o in allObjects)
                    o.IsSelect = false;

                if (dg.SelectedItem != null)
                {
                    ServiceAdministration.CsPoste SelectedObject = (ServiceAdministration.CsPoste)dg.SelectedItem;

                    if (SelectedObject.IsSelect == false)
                        SelectedObject.IsSelect = true;
                    else
                        SelectedObject.IsSelect = false;
                }
            }
            catch (Exception es)
            {

                throw es;
            }
        }

        private void btn_Modifier_Click_1(object sender, RoutedEventArgs e)
        {
            if (dtg_Poste.SelectedItem != null)
            {
                ServiceAdministration.CsPoste lePoste = ((List<ServiceAdministration.CsPoste>)dtg_Poste.ItemsSource).FirstOrDefault(t=>t.IsSelect ==true );
                FrmParametragePoste ctrl = new FrmParametragePoste(lePoste);
                ctrl.Closed += ctrl_Closed;
                ctrl.Show();

            }
        }
        void ctrl_Closed(object sender, EventArgs e)
        {
            //RetournePoste(lstIdCentre);

            btn_Rechercher_Click(null, null);
        }

        private void btn_Site_Click_1(object sender, RoutedEventArgs e)
        {
            if (lstSite.Count > 0)
            {
                this.btn_Site.IsEnabled = false;
                List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedSite);
                ctr.Show();
            }
        }

        private void galatee_OkClickedSite(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                Galatee.Silverlight.ServiceAccueil.CsSite leSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODE;
                this.Txt_CodeSite.Tag  = leSite.PK_ID ;
                this.Txt_LibelleSite .Text = leSite.LIBELLE ;

                this.Txt_CodeCentre.IsReadOnly = true  ;
                this.btn_Centre.IsEnabled = true ;
            }
            
        }

       

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            if (LstCentre.Count > 0)
            {
                this.btn_Centre.IsEnabled = false;
                List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(LstCentre.Where(t=>t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag ).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CODE", "LIBELLE", Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeCentre);
                ctr.Closed += new EventHandler(galatee_OkClickedCentre);
                ctr.Show();
            }
        }

        private void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Centre.IsEnabled = true;
               ServiceAccueil. CsCentre leCentre = (ServiceAccueil.CsCentre )ctrs.MyObject;
               this.Txt_CodeCentre.Text = leCentre.CODE;
               this.Txt_LibelleCentre .Text = leCentre.LIBELLE ;
                this.Txt_CodeCentre.Tag = leCentre.PK_ID;

            }
        }

        private void btn_Rechercher_Click(object sender, RoutedEventArgs e)
        {
            List<int> lstCentre = new List<int>();
            if (this.Txt_CodeSite.Tag != null && this.Txt_CodeCentre.Tag == null )
            {
                List<ServiceAccueil.CsCentre> lesCentre = LstCentre.Where(t => t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag).ToList();
                foreach (ServiceAccueil.CsCentre item in lesCentre)
                    lstCentre.Add(item.PK_ID);
                RetournePoste(lstCentre);
            }
            if (this.Txt_CodeCentre.Tag !=null )
            {
            lstCentre.Add((int)this.Txt_CodeCentre.Tag);
            RetournePoste(lstCentre);
            }

        }
    }
}


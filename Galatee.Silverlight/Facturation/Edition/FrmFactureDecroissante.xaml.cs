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
using Galatee.Silverlight.ServiceFacturation;

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmFactureDecroissante : ChildWindow
    {
        public FrmFactureDecroissante()
        {
            InitializeComponent();
            ChargerDonneeDuSite();
        }
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    List<int> IdDesCentre = new List<int>();
                    List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    List<ServiceAccueil.CsSite> lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);
                    Chargelotri(IdDesCentre);
                    return;

                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<int> IdDesCentre = new List<int>();
                    List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    List<ServiceAccueil.CsSite> lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);
                    Chargelotri(IdDesCentre);

                };
                service.ListeDesDonneesDesSiteAsync(true);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List< CsLotri> listeBatch = new List< CsLotri>();

        private void Chargelotri(List<int> lstCentreHAbilite)
        {
            FacturationServiceClient client = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
            client.RetourneListeLotNonTraiteCompleted += (_, es) =>
            {
                try
                {
                    if (es != null && es.Cancelled)
                    {
                        Message.ShowError("", "");
                        //LoadingManager.EndLoading(result);
                        return;
                    }

                    if (es.Result == null || es.Result.Count == 0)
                    {
                        Message.ShowError("", "");
                        //LoadingManager.EndLoading(result);
                        return;
                    }

                    if (es.Result != null && es.Result.Count != 0)
                        listeBatch = ClasseMethodeGenerique.RetourneDistinctNumLot(es.Result);

                    this.btn_batch.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }
            };
            client.RetourneListeLotNonTraiteAsync(lstCentreHAbilite);

        }
        private void btn_batch_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listeBatch != null)
                {
                    List<CsLotri> _lstLotAfficher = ClasseMethodeGenerique.RetourneDistinctLotri(listeBatch);
                    List<string> _LstColonneAffich = new List<string>();
                    _LstColonneAffich.Add("NUMLOTRI");
                    _LstColonneAffich.Add("PERIODE");

                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_lstLotAfficher);
                    MainView.UcListeGenerique ucg = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                    ucg.Closed += new EventHandler(ucgReturn);
                    ucg.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void ucgReturn(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique uc = sender as MainView.UcListeGenerique;
                if (uc.GetisOkClick)
                {
                    CsLotri lot = (CsLotri)uc.MyObject;
                    Txt_NumBatch.Text = lot.NUMLOTRI;
                    this.Txt_NumBatch.Tag = lot;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void Txt_NumBatch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Txt_NumBatch.Text.Length == SessionObject.Enumere.TailleNumeroBatch)
            {
                if (listeBatch.FirstOrDefault(t => t.NUMLOTRI == this.Txt_NumBatch.Text) == null)
                {
                    Message.ShowInformation("Lot inexistant ", "Lot");
                    return;
                }
                else
                    Txt_NumBatch.Tag = listeBatch.FirstOrDefault(t => t.NUMLOTRI == this.Txt_NumBatch.Text);
            }

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.dtg_Facture.ItemsSource != null)
            {
                List<CsEnteteFacture> lstFacture = (List<CsEnteteFacture>)this.dtg_Facture.ItemsSource;
                Utility.ActionDirectOrientation<ServicePrintings.CsEnteteFacture, ServiceFacturation.CsEnteteFacture>(lstFacture, null, SessionObject.CheminImpression, "MontantFacture", "Facturation", false);
                this.DialogResult = false;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btn_Rechercher_Click(object sender, RoutedEventArgs e)
        {
            List<Galatee.Silverlight.ServiceFacturation.CsLotri> _lstLotSelect = new List<Galatee.Silverlight.ServiceFacturation.CsLotri>();
            Galatee.Silverlight.ServiceFacturation.CsLotri _leCritere = new Galatee.Silverlight.ServiceFacturation.CsLotri();
            if (this.Txt_NumBatch.Tag != null)
                _leCritere.NUMLOTRI = ((CsLotri)this.Txt_NumBatch.Tag).NUMLOTRI;
            List<int> lstIdCentre = listeBatch.Where(t => t.NUMLOTRI == _leCritere.NUMLOTRI).Select(t => t.FK_IDCENTRE).ToList();
            ChargerAction(_leCritere, lstIdCentre);
        }

        private void ChargerAction(Galatee.Silverlight.ServiceFacturation.CsLotri _leLotri, List<int> lstIdCentre)
        {
            try
            {
                Galatee.Silverlight.ServiceFacturation.FacturationServiceClient service = new Galatee.Silverlight.ServiceFacturation.FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.retourneFactureDecroissanceAsync(lstIdCentre,_leLotri);
                service.retourneFactureDecroissanceCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                        {
                            Message.ShowError(args.Error, Galatee.Silverlight.Resources.Index.Langue.libelleModule);
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            Message.ShowError(Galatee.Silverlight.Resources.Index.Langue.Msg_IndexNonTrouvé, Galatee.Silverlight.Resources.Index.Langue.libelleModule);
                            return;
                        }
                        this.dtg_Facture.ItemsSource = null;
                        this.dtg_Facture.ItemsSource = args.Result;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


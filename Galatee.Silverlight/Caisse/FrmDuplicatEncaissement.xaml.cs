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
using System.Globalization;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Resources.Caisse ;

namespace Galatee.Silverlight.Caisse
{
    public partial class FrmDuplicatEncaissement : ChildWindow
    {


        public FrmDuplicatEncaissement()
        {
            InitializeComponent();
            try
            {
                translate();
                Chk_BorderoSeulement.Visibility = System.Windows.Visibility.Collapsed;
                RetourneListeDesModReglement();
            }
            catch (Exception x)
            {
                Message.ShowError(x, Langue.errorTitle);
            }
        }
        void translate()
        {
            try
            {
                this.lbl_Recu.Content = Langue.Num_Recu;
                //this.dtg_FactureAnnule.Columns[0].Header = Langue.Period;
                this.dtg_FactureAnnule.Columns[0].Header = Langue.Mode_paiement;
                this.dtg_FactureAnnule.Columns[1].Header = Langue.Montant_recu;
                this.OKButton.Content = Langue.Btn_ok;
                this.CancelButton.Content = Langue.Btn_annuler;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        string MatriculeCaisse = UserConnecte.matricule;
        string NumCaisse = UserConnecte.numcaisse ;
        List<CsLclient> ListeDesReglementADupliquer = new List<CsLclient>();
        List<CsModereglement> ListeDesModesReglementADupliquer = new List<CsModereglement>();

        private void RetourneListeDesModReglement()
        {
            if (SessionObject.ListeModesReglement != null && SessionObject.ListeModesReglement.Count != 0)
                return;
            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.RetourneModesReglementCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.ListeModesReglement = args.Result;
            };
            service.RetourneModesReglementAsync();
            service.CloseAsync();
        }
        public FrmDuplicatEncaissement(string _Caisse)
        {
            InitializeComponent();
            Chk_BorderoSeulement.Visibility = System.Windows.Visibility.Collapsed;
            NumCaisse = _Caisse;
            translate();
        }

        public FrmDuplicatEncaissement(string _Caisse, string title)
        {
            InitializeComponent();
            try
            {

                NumCaisse = _Caisse;
                this.Title = title;
                Chk_BorderoSeulement.Visibility = System.Windows.Visibility.Collapsed;
                translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EditionDeRecu();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void FrmDuplicatEncaissement_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                if (SessionObject.LaCaisseCourante == null || SessionObject.LaCaisseCourante.FK_IDCAISSE == 0)
                    ChargerHabilitationCaisse();
                else
                    RetourneReglementEncaisse();
            }
            catch (Exception EX)
            {
                Message.ShowError(EX, Langue.errorTitle);
            }
        }






        private void ChargerHabilitationCaisse()
        {
            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.RetouneLaCaisseCouranteAsync(UserConnecte.matricule);
            service.RetouneLaCaisseCouranteCompleted += (sender, es) =>
            {
                try
                {
                    if (es.Cancelled || es.Error != null)
                    {
                        Message.ShowError("Erreur d'invocation du service . Veuillez réssayer svp!", Galatee.Silverlight.Resources.Langue.errorTitle);
                        this.DialogResult = true;
                    }

                    if (es.Result == null)
                    {
                        Message.ShowError("Aucune données trouvées", Langue.errorTitle);
                        return;
                    }

                    SessionObject.LaCaisseCourante = es.Result;
                    RetourneReglementEncaisse();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
            };

        }





        void RetourneReglementEncaisse()
        {
            try
            {
                CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                service.RetourneRecuDeCaissePourAnnulationAsync(SessionObject.LaCaisseCourante.PK_ID);
                service.RetourneRecuDeCaissePourAnnulationCompleted += (s, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        Message.ShowError("Erreur de chargement des reçus de la caisse. Réessayez svp !", "Erreur");
                        this.DialogResult = true;

                    }

                    if (args.Result == null || args.Result.Count == 0)
                    {
                        Message.ShowError("Aucun encaissement n'a été effectué.", Langue.errorTitle);
                        return;
                    }
                    List<CsLclient> _LstReglement = new List<CsLclient>();
                    _LstReglement = args.Result;
                    _LstrefReglement = _LstReglement;
                    if (_LstReglement != null && _LstReglement.Count != 0)
                        RemplireCombo(_LstReglement.Where(t=> string.IsNullOrEmpty(t.TOPANNUL)).OrderBy(a=>a.ACQUIT).ToList());
                };
                service.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void EditionDeRecu()
        {
            try
            {
                bool IsBordero = false ;
                if (this.Chk_BorderoSeulement.IsChecked == true) IsBordero = true;
                List<CsReglementRecu> _ListeDesRecu = new List<CsReglementRecu>();
                ListeDesReglementADupliquer = _LstrefReglement.Where(t => t.ACQUIT == leReglementSelect.ACQUIT).ToList();
                List<CsClient> _lstClientRecu = Galatee.Silverlight.Caisse.MethodeGenerics.RetourneClientFromFacture(ListeDesReglementADupliquer);
                foreach (var item in _lstClientRecu)
                {
                    List<CsLclient> lstFactureDuClient = ListeDesReglementADupliquer.Where(t => t.CENTRE == item.CENTRE &&
                                                                                                t.CLIENT == item.REFCLIENT  &&
                                                                                                t.ORDRE == item.ORDRE).ToList();
                    if (_lstClientRecu.Count > 1)
                    {
                        lstFactureDuClient.ForEach(t => t.PERCU = lstFactureDuClient.Sum(p => p.MONTANT));
                        lstFactureDuClient.ForEach(t => t.RENDU = 0);
                    }
                    lstFactureDuClient.ForEach(t => t.MONTANTEXIGIBLE = t.MONTANTEXIGIBLE - lstFactureDuClient.Where(h => h.NDOC != "TIMBRE" && h.EXIGIBILITE < System.DateTime.Today.Date ).Sum(y => y.MONTANT));
                    lstFactureDuClient.ForEach(t => t.MONTANTEXIGIBLE = t.MONTANTEXIGIBLE - lstFactureDuClient.Where(h => h.NDOC != "TIMBRE" && h.EXIGIBILITE > System.DateTime.Today.Date ).Sum(h => h.MONTANT));
                    lstFactureDuClient.ForEach(t => t.SOLDECLIENT = t.SOLDECLIENT - lstFactureDuClient.Where(l => l.NDOC != "TIMBRE").Sum(h => h.MONTANT));
                    _ListeDesRecu.AddRange(EditionRecu.ReorganiserReglement(lstFactureDuClient, "Duplicata"));

                }
                string key = Utility.getKey();
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("pTypeRecu", "DUPLICATA D'ENCAISSEMENT");
                LanceImpression(key, param, _ListeDesRecu, IsBordero);

            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        private void LanceImpression(string key, Dictionary<string, string> parametresRDLC, List<Galatee.Silverlight.ServiceCaisse.CsReglementRecu> res, bool IsBordero)
        {
            try
            {
                if (IsBordero) parametresRDLC = null;
                if (!IsBordero)
                    Utility.ActionDirectOrientation<ServicePrintings.CsReglementRecu, ServiceCaisse.CsReglementRecu>(res, parametresRDLC, SessionObject.CheminImpression, "Duplicat", "Caisse", true);
                else
                    Utility.ActionDirectOrientation<ServicePrintings.CsReglementRecu, ServiceCaisse.CsReglementRecu>(res, parametresRDLC, SessionObject.CheminImpression, "BorderoRegroupe", "Caisse", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<CsLclient> _LstrefReglement = new List<CsLclient>();
        private void RemplireCombo(List<CsLclient > _LstReglement)
        {
            try
            {
                // La grid doit afficher le detail d un recu par mode de reglement
                var reglemntParModereg = (from p in _LstReglement
                                          group new { p } by new { p.ACQUIT, p.DTRANS   } into pResult
                                          select new
                                          {
                                              pResult.Key.ACQUIT,
                                              pResult.Key.DTRANS,
                                              MONTANT = (decimal?)pResult.Where(t => t.p.ACQUIT == pResult.Key.ACQUIT).Sum(o => o.p.MONTANT)
                                          });

                List<CsLclient> ReglementAfficherParModeReg = new List<CsLclient>();

                foreach (var r in reglemntParModereg.OrderByDescending(p => p.ACQUIT))
                {
                    CsLclient rglt = new CsLclient();
                    rglt.REFFERENCECLIENT = r.ACQUIT.PadRight(10, ' ') + r.DTRANS.Value.ToShortDateString().ToString().PadRight(10, ' ') + Convert.ToDecimal(r.MONTANT).ToString(SessionObject.FormatMontant).PadLeft(10, ' ');
                    rglt.ACQUIT = r.ACQUIT;
                    ReglementAfficherParModeReg.Add(rglt);
                }

                this.Cbo_ListeRecu.ItemsSource = null;
                this.Cbo_ListeRecu.ItemsSource = ReglementAfficherParModeReg;
                this.Cbo_ListeRecu.DisplayMemberPath = "REFFERENCECLIENT";
                this.Cbo_ListeRecu.SelectedItem = ReglementAfficherParModeReg[0];
            }
            catch (Exception ex)
            {

            }
        }
        CsLclient  leReglementSelect = new CsLclient ();
        private void Cbo_ListeRecu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                leReglementSelect = (CsLclient )this.Cbo_ListeRecu.SelectedItem;
                if (leReglementSelect != null)
                {
                    var lesDistinctClient = _LstrefReglement.Where(o => o.ACQUIT == leReglementSelect.ACQUIT).Select(u => new { u.CENTRE, u.CLIENT, u.ORDRE }).Distinct();
                    int nbr =0;
                     foreach (var item in lesDistinctClient)
	                    {
		                    nbr++;
                            if (nbr > 1)
                            {
                                Chk_BorderoSeulement.Visibility = System.Windows.Visibility.Visible;
                                break;
                            }
                            else
                                Chk_BorderoSeulement.Visibility = System.Windows.Visibility.Collapsed;
	                    }
                    var _lstfacture = (from p in _LstrefReglement
                                       where p.ACQUIT == leReglementSelect.ACQUIT
                                       group new { p } by new {  p.ACQUIT,p.MODEREG  } into pResult
                                       select new
                                       {
                                           pResult.Key.ACQUIT,
                                           MONTANT = (decimal?)pResult.Where(t => t.p.ACQUIT == pResult.Key.ACQUIT).Sum(o => o.p.MONTANT),
                                           LIBELLEMODREG = SessionObject.ListeModesReglement.FirstOrDefault(t => t.CODE  == pResult.Key.MODEREG).LIBELLE 
                                       }).ToList();
                    List<CsLclient> RegltAfficher = new List<CsLclient>();
                    foreach (var item in _lstfacture)
                    {
                        CsLclient Reglt = new CsLclient();
                       Reglt.LIBELLEMODREG = item.LIBELLEMODREG;
                       Reglt.MONTANT = item.MONTANT;
                       RegltAfficher.Add(Reglt);
                    }
                    dtg_FactureAnnule.ItemsSource = null;
                    dtg_FactureAnnule.ItemsSource = RegltAfficher;
                    this.txtMontantRecuTot.Text = Convert.ToDecimal( RegltAfficher[0].MONTANT.Value ).ToString(SessionObject.FormatMontant );
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }

    }
}


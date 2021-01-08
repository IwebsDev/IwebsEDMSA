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
using System.Globalization;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Resources.Caisse ;
using Galatee.Silverlight.ServiceAdministration;


namespace Galatee.Silverlight.Caisse
{
    public partial class FrmDemandeAnnulation : ChildWindow
    {
        //List<CsLclient> _ListeDesReglementDuRecu = new List<CsLclient>();
        List<CsLclient> _LstrefReglement = new List<CsLclient>();

        List<CsUtilisateur> _ListeDesUtilisateurs = new List<CsUtilisateur>();
        int TailleRefcli = SessionObject.Enumere.TailleCentre +
                           SessionObject.Enumere.TailleClient  +
                           SessionObject.Enumere.TailleOrdre ;

        public FrmDemandeAnnulation()
        {
            InitializeComponent();
            try
            {
                //translate();
                NumCaisse = UserConnecte.numcaisse;
                MatriculeCaisse = UserConnecte.matricule;
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
                this.dtg_FactureAAnnule.Columns[0].Header = Langue.Mode_paiement;
                this.dtg_FactureAAnnule.Columns[1].Header = Langue.Montant_recu;
                this.CancelButton.Content = Langue.Btn_annuler;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        string MatriculeCaisse = UserConnecte.matricule;
        string NumCaisse = UserConnecte.numcaisse ;
        List<CsLclient> ListeDesReglementAnnuler = new List<CsLclient>();
        List<CsLclient> ListeDesReglementARetirerAnnulation = new List<CsLclient>();

     
        private void MiseAjourRetraitDemandeAnnulation(List<CsLclient> leReglement)
        {
            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint(this));
            service.RetirerDemandeAnnulationEncaissementAsync(leReglement);
            service.RetirerDemandeAnnulationEncaissementCompleted += (sender, es) =>
            {
                try
                {
                    if (es.Cancelled || es.Error != null || es.Result == null)
                    {
                        Message.ShowError("Erreur d'annulation du reçu. Veuillez réessayer svp !", Langue.errorTitle);
                        return;
                    }
                  
                    List<CsLclient> lstInitData = ((List<CsLclient>)this.dtg_FactureAjouter.ItemsSource).Where(t => t.Selectionner == true).ToList();
                    List<CsLclient> lstInitData2 = (List<CsLclient>)this.dtg_FactureAjouter.ItemsSource;
                    List<string> lstAcquit = new List<string>();
                    foreach (var item in lstInitData)
                    {
                        if (lstAcquit.FirstOrDefault(t => t == item.ACQUIT) == null)
                            lstAcquit.Add(item.ACQUIT);
                    }
                    List<CsLclient> lstAAfficher = new List<CsLclient>();
                    lstInitData2.RemoveAll(t => lstAcquit.Contains(t.ACQUIT));
                    dtg_FactureAjouter.ItemsSource = null;
                    dtg_FactureAjouter.ItemsSource = lstInitData2;

                    List<CsLclient> lstInitData1 = (List<CsLclient>)this.dtg_FactureAAnnule.ItemsSource;
                    List<CsLclient> lstInitAjouter = _LstrefReglement.Where(t => lstAcquit.Contains(t.ACQUIT)).ToList();
                    List<CsLclient> lstReglementAjouter = RetourneEltsDataGrid1(lstInitAjouter);
                    lstInitData1.AddRange(lstReglementAjouter);
                    dtg_FactureAAnnule.ItemsSource = null;
                    dtg_FactureAAnnule.ItemsSource = lstInitData1;
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
            };
        }
        private void MiseAjourDemandeAnnulation(List<CsLclient> leReglement)
        {
            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint(this));
            service.DemandeAnnulationEncaissementAsync(leReglement);
            service.DemandeAnnulationEncaissementCompleted  += (sender, es) =>
            {
                try
                {
                    if (es.Cancelled || es.Error != null || es.Result == null)
                    {
                        Message.ShowError("Erreur d'annulation du reçu. Veuillez réessayer svp !", Langue.errorTitle);
                        return;
                    }
                    
                    List<CsLclient> lstInitData1 = (List<CsLclient>)this.dtg_FactureAjouter.ItemsSource;
                    lstInitData1.AddRange(_LstrefReglement.Where(t=>t.ACQUIT ==leReglement.First().ACQUIT ));
                    List<CsLclient> lstReglementAjouter = RetourneEltsDataGrid2(lstInitData1);
                    dtg_FactureAjouter.ItemsSource = null;
                    dtg_FactureAjouter.ItemsSource = lstReglementAjouter;

                    List<CsLclient> lstInitData2 = (List<CsLclient>)this.dtg_FactureAAnnule.ItemsSource;
                    List<CsLclient> lstAAfficher = new List<CsLclient>();
                    lstInitData2.RemoveAll(t => t.ACQUIT == leReglement.First().ACQUIT);
                    dtg_FactureAAnnule.ItemsSource = null;
                    dtg_FactureAAnnule.ItemsSource = lstInitData2;
                   
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
            };
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void FrmDuplicatEncaissement_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RetourneReglementEncaisse();
            }
            catch (Exception EX)
            {
                Message.ShowError(EX, Langue.errorTitle);
            }
        }
        private List<CsLclient> RetourneEltsDataGrid1(List<CsLclient> _LstReglement)
        {
            try
            {
                List<CsLclient> lstClientReglement = new List<CsLclient>();
                if (_LstReglement!= null && _LstReglement.Count > 0)
                {
                    var lstClientReglementDistnct = (from p in _LstReglement
                                                     group new { p } by new { p.ACQUIT, p.DTRANS } into pResult
                                                      select new
                                                      {
                                                          pResult.Key.ACQUIT,
                                                          pResult.Key.DTRANS,
                                                          MONTANT = (decimal?)pResult.Where(t => t.p.ACQUIT == pResult.Key.ACQUIT).Sum(o => o.p.MONTANT)
                                                      });

                    foreach (var item in lstClientReglementDistnct)
                        lstClientReglement.Add(new CsLclient { ACQUIT = item.ACQUIT, DTRANS = item.DTRANS, MONTANT = item.MONTANT });
                }
                return lstClientReglement;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        
        }
        private List<CsLclient> RetourneEltsDataGrid2(List<CsLclient> _LstReglementClient)
        {
            try
            {
                foreach (CsLclient item in _LstReglementClient)
                {
                    if (item.STATUS == SessionObject.Enumere.StatusDemandeInitier)
                        item.LIBELLENATURE = "En cours";
                    else if (item.STATUS == SessionObject.Enumere.StatusDemandeRetirer)
                        item.LIBELLENATURE = "Retirée";
                    else if (item.STATUS == SessionObject.Enumere.StatusDemandeRejeter)
                        item.LIBELLENATURE = "Rejétée";
                    else if (item.STATUS == SessionObject.Enumere.StatusDemandeValider)
                        item.LIBELLENATURE = "Validée";
                }



                List<CsLclient> lstClientReglement = new List<CsLclient>();
                if (_LstReglementClient != null && _LstReglementClient.Count > 0)
                {
                    var lstClientReglementDistnct = (from p in _LstReglementClient
                                                     group new { p } by new { p.FK_IDCLIENT , p.CENTRE ,p.CLIENT ,p.ORDRE ,p.ACQUIT ,p.NOM,p.DTRANS,p.MOTIFANNULATION ,p.STATUS ,p.LIBELLENATURE    } into pResult
                                                     select new
                                                     {
                                                         pResult.Key.FK_IDCLIENT ,
                                                         pResult.Key.CENTRE,
                                                         pResult.Key.CLIENT,
                                                         pResult.Key.ORDRE,
                                                         pResult.Key.ACQUIT,
                                                         pResult.Key.NOM,
                                                         pResult.Key.DTRANS ,
                                                         pResult.Key.MOTIFANNULATION ,
                                                         pResult.Key.STATUS ,
                                                         pResult.Key.LIBELLENATURE ,
                                                         MONTANT = (decimal?)pResult.Where(t => t.p.ACQUIT == pResult.Key.ACQUIT).Sum(o => o.p.MONTANT)
                                                     });
                    foreach (var item in lstClientReglementDistnct)
                        lstClientReglement.Add(new CsLclient {FK_IDCLIENT = item.FK_IDCLIENT,CENTRE = item.CENTRE ,CLIENT =item.CLIENT ,ORDRE = item.ORDRE , 
                                                   ACQUIT = item.ACQUIT, MONTANT = item.MONTANT ,NOM = item.NOM,DTRANS = item.DTRANS,MOTIFANNULATION = item.MOTIFANNULATION ,STATUS = item.STATUS ,LIBELLENATURE = item.LIBELLENATURE });
                    lstClientReglement.ForEach(t => t.REFFERENCECLIENT = t.CENTRE + " " + t.CLIENT + " " + t.ORDRE);
                }
                return lstClientReglement;
            }
            catch (Exception ex)
            {

                throw;
            }

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
                    _LstReglement=args.Result;
                    _LstrefReglement = _LstReglement;
              


                    if (_LstReglement != null && _LstReglement.Count != 0)
                    {
                        List<CsLclient> lstReglementClient = _LstReglement.Where(t => (string.IsNullOrEmpty(t.STATUS) || t.STATUS == SessionObject.Enumere.StatusDemandeRetirer)).ToList();
                        this.dtg_FactureAAnnule.ItemsSource = null;
                        this.dtg_FactureAAnnule.ItemsSource = RetourneEltsDataGrid1(lstReglementClient);

                        List<CsLclient> lstReglementClientEncour = _LstReglement.Where(t => (!string.IsNullOrEmpty(t.STATUS)&& t.STATUS != SessionObject.Enumere.StatusDemandeRetirer)).ToList();
                        this.dtg_FactureAjouter.ItemsSource = null;
                        this.dtg_FactureAjouter.ItemsSource = RetourneEltsDataGrid2(lstReglementClientEncour);
                    }
                };
                service.CloseAsync();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private void btn_Retirer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.dtg_FactureAjouter.SelectedItem != null)
                    MiseAjourRetraitDemandeAnnulation(MethodeGenerics.RetourneDistincAcquitement(_LstrefReglement.Where(t => t.ACQUIT == ((CsLclient)this.dtg_FactureAjouter.SelectedItem).ACQUIT).ToList()));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void btn_Ajouter_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                CsLclient leReglementSelect = (CsLclient)dtg_FactureAAnnule.SelectedItem;
                if (leReglementSelect != null)
                {
                    FrmMotifAction ctrl = new FrmMotifAction(_LstrefReglement.Where(t => t.ACQUIT == leReglementSelect.ACQUIT).ToList());
                    ctrl.Closed += new EventHandler(galatee_OkClickedRaisonAnnulation);
                    ctrl.Show();
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Langue.LibelleModule);
            }
        }
        private void galatee_OkClickedRaisonAnnulation(object sender, EventArgs e)
        {
            try
            {
                FrmMotifAction ctrs = sender as FrmMotifAction;
                if (ctrs.IsValide)
                    MiseAjourDemandeAnnulation(MethodeGenerics.RetourneDistincAcquitement(ctrs.lstReglementAnnuler));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

   

        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsLclient>;
            foreach (var o in allObjects)
                o.Selectionner = false;

            if (dg.SelectedItem != null)
            {
                CsLclient SelectedObject = (CsLclient)dg.SelectedItem;
                //if (SelectedObject.Selectionner == false)
                    SelectedObject.Selectionner = true;
                //else
                //    SelectedObject.Selectionner = false;
            }
        }

        private void dgMyDataGrid2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsLclient>;
            //foreach (var o in allObjects)
            //    o.Selectionner = false;

            if (dg.SelectedItem != null)
            {
                CsLclient SelectedObject = (CsLclient)dg.SelectedItem;
                if (SelectedObject.Selectionner == false)
                {
                    SelectedObject.Selectionner = true;
                    if (SelectedObject.STATUS == SessionObject.Enumere.StatusDemandeValider)
                        this.btn_Retirer.Visibility = System.Windows.Visibility.Collapsed;
                    else
                        this.btn_Retirer.Visibility = System.Windows.Visibility.Visible ;
                }
                else
                    SelectedObject.Selectionner = false;
            }
        }
    }
}


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
using System.Threading;
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceCaisse;
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.Shared;
using System.ServiceModel.Description;
using Galatee.Silverlight.Resources.Caisse;
using System.Globalization;
using System.ComponentModel;
using Galatee.Silverlight.Classes;

namespace Galatee.Silverlight.Caisse
{
    public partial class FrmEncaissement : ChildWindow, INotifyPropertyChanged
    {
       
        int debutClient = SessionObject.Enumere.TailleCentre;
        int debutOrdre = SessionObject.Enumere.TailleCentre + SessionObject.Enumere.TailleClient;
        int TailleReferenceClient = SessionObject.Enumere.TailleCentre +
                            SessionObject.Enumere.TailleClient +
                            SessionObject.Enumere.TailleOrdre;

        List<CsLclient> ListeFactureAregle = new List<CsLclient>();
        List<CsClient> lstClient = new List<CsClient>();
        CsClient _UnClient;
        int initvalue = 0;
        List<CsLclient> lesEltInitial = new List<CsLclient>();
        public FrmEncaissement()
        {
            InitializeComponent();
            try
            {
                translateControls();
                InitialiseControle();
                timer.Interval = new TimeSpan(3, 0, 0);
                timer.Tick += timer_Tick;
                timer.Start();

                this.Txt_DateEncaissement.Text = System.DateTime.Today.Date.ToShortDateString();


                if (SessionObject.LePosteCourant == null || SessionObject.LePosteCourant.FK_IDCAISSE == null || SessionObject.LePosteCourant.FK_IDCAISSE == 0 || SessionObject.LePosteCourant.FK_IDCENTRE == null)
                {
                    Message.ShowError("Problème d'identification du poste", Langue.errorTitle);
                    return;
                }



                if (SessionObject.LaCaisseCourante == null || SessionObject.LaCaisseCourante.FK_IDCAISSE == 0)
                    ChargerHabilitationCaisse();
                else
                    RecuperationNumRecu();

                if (SessionObject.LstFraisTimbre == null || SessionObject.LstFraisTimbre.Count == 0)
                    RetourneLstFraixTimbre();

                if (SessionObject.ListeModesReglement == null || SessionObject.ListeModesReglement.Count == 0)
                    RetourneListeDesModReglement();


                if (SessionObject.ListeBanques == null || SessionObject.ListeBanques.Count == 0)
                    RecuperationListBanque();


                if (SessionObject.LstDesLibelleTop == null || SessionObject.LstDesLibelleTop.Count == 0)
                    RetourneListeDesLibelleTop();

                if (SessionObject.LstDesCopers == null || SessionObject.LstDesCopers.Count == 0)
                    ChargerCoper();


                if (SessionObject.LstTypeDemande == null || SessionObject.LstTypeDemande.Count == 0)
                    ChargerTypeDemande();

                if (SessionObject.ListeDesProduit == null || SessionObject.ListeDesProduit.Count == 0)
                    ChargerListeDeProduit();


            }
            catch (Exception ex)
            {
              Message.ShowError(ex, Langue.errorTitle);
            }        
        }





        private void EtatCaisse()
        {
            string EtatCaisse = string.Empty;

            try
            {
                CaisseServiceClient clt = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                clt.VerifieEtatCaisseCompleted += (xx, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        Message.Show("Error occurs while processing request !", Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                        // 
                    }
                    EtatCaisse = args.Result;

                    if (EtatCaisse == SessionObject.Enumere.EtatDeCaisseDejaCloture)
                    {
                        Message.Show(Galatee.Silverlight.Resources.Caisse.Langue.MsgCaisseDejaFerme, Galatee.Silverlight.Resources.Langue.errorTitle);
                        this.Close();
                        return;
                    }

                    else if (EtatCaisse == SessionObject.Enumere.EtatDeCaisseNonCloture)
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Caisse.Langue.MsgCaisseNonCloture, Galatee.Silverlight.Resources.Caisse.Langue.LibelleModule);
                        this.Close();
                        return;
                    }

                    else if (EtatCaisse == SessionObject.Enumere.EtatDeCaisseAutreSessionOuvert)
                    {
                        Message.ShowWarning("Vous avez ouvert une caisse sur un autre poste " + "\n\r" + "Veuillez la clôturer", "Caisse");
                        this.Close();
                        return;
                    }

                    ChargerSituationCaisse();


                };
                clt.VerifieEtatCaisseAsync(UserConnecte.matricule, SessionObject.LePosteCourant.FK_IDCAISSE.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




        private void ChargerListeDeProduit()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = res.Result;
                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void ChargerTypeDemande()
        {
            try
            {
                if (SessionObject.LstTypeDemande != null && SessionObject.LstTypeDemande.Count != 0)
                    return;

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneOptionDemandeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.LstTypeDemande = res.Result;
                };
                service1.RetourneOptionDemandeAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }


        private void ChargerCoper()
        {
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneTousCoperCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstDesCopers = args.Result;
            };
            service.RetourneTousCoperAsync();
            service.CloseAsync();
        }


        private void RecuperationListBanque()
        {
            if (SessionObject.ListeBanques == null)
            {
                int loaderHandler = LoadingManager.BeginLoading(Langue.Data_Loading);
                CaisseServiceClient srv = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                srv.RetourneListeDesBanquesCompleted += (ss, ee) =>
                {
                    try
                    {
                        if (ee.Cancelled || ee.Error != null || ee.Result == null)
                        {
                            string error = ee.Error.InnerException.ToString();
                            return;
                        }

                        //Assignation de la variable de session contenant la liste des banques
                        SessionObject.ListeBanques = ee.Result;
                        if (SessionObject.ListeBanques == null || SessionObject.ListeBanques.Count == 0)
                        {
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Caisse.Langue.errorTitle);
                    }
                    finally
                    {
                        LoadingManager.EndLoading(loaderHandler);
                    }
                };
                srv.RetourneListeDesBanquesAsync();
            }
        }


        private void RetourneListeDesModReglement()
        {
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
        private void RetourneListeDesLibelleTop()
        {
            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.RetourneTousLibelleTopCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstDesLibelleTop = args.Result;
            };
            service.RetourneTousLibelleTopAsync();
            service.CloseAsync();
        }




        private void ChargerHabilitationCaisse()
        {
            CsPoste lePoste = new CsPoste();
            lePoste.CODECENTRE = SessionObject.LePosteCourant.CODECENTRE;
            lePoste.FK_IDCAISSE = SessionObject.LePosteCourant.FK_IDCAISSE;
            lePoste.FK_IDCENTRE = SessionObject.LePosteCourant.FK_IDCENTRE;
            lePoste.IsSelect = SessionObject.LePosteCourant.IsSelect;
            lePoste.MATRICULE = SessionObject.LePosteCourant.MATRICULE;
            lePoste.NOMPOSTE = SessionObject.LePosteCourant.NOMPOSTE;
            lePoste.NUMCAISSE = SessionObject.LePosteCourant.NUMCAISSE;
            lePoste.PK_ID = SessionObject.LePosteCourant.PK_ID;

            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            //service.RetouneLaCaisseCouranteAsync(UserConnecte.matricule);
            service.RetouneLaCaisseCouranteInsereeAsync(UserConnecte.matricule, lePoste);
            service.RetouneLaCaisseCouranteInsereeCompleted += (sender, es) =>
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
                        Message.ShowError("Aucune donnée trouvée", Langue.errorTitle);
                        return;
                    }

                    SessionObject.LaCaisseCourante = es.Result;
                    RecuperationNumRecu();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
            };

        }



        private void timer_Tick(object sender, EventArgs e)
        {
            Message.ShowInformation("Session de caisse fermée", "Session");
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
            this.Close();
        }
        private void RecuperationNumRecu()
        {
            this.IsEnabled = false;


            CaisseServiceClient srv;
            int loadingHandler = LoadingManager.BeginLoading(Langue.msg_test_connexion);
            srv = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            srv.RetourneNumeroRecuCompleted += (s, es) =>
            {
                try
                {
                    if (es.Error != null || es.Cancelled || es.Result == "" || es.Result == null)
                        if (SessionObject.DernierNumeroDeRecu <= -1) // Dans ce cas on n'a pas encore recuperé de numero de la journée
                        {
                            if (string.IsNullOrEmpty(es.Result)) // Si aucun numero n'est retourné, il ne s'agit surement pas d'une caissiere
                                Message.ShowError(Langue.msg_encaissement_interdit, Langue.informationTitle);
                            else
                                Message.ShowError(Langue.msg_error_dernier_numero_recu, Langue.informationTitle);

                            SessionObject.IsServerDown = true;
                            this.DialogResult = false;
                        }
                        else
                        {
                            SessionObject.IsServerDown = true;
                            this.IsEnabled = true;
                        }

                    else
                    {
                        // On s'assure qu'il n'y a pas d'encaissement hors ligne en attente de synchronisation
                        //SessionObject.TrySynchronisation();
                        SessionObject.DernierNumeroDeRecu = Decimal.Parse(es.Result);
                        this.IsEnabled = true;
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
                finally
                {
                    LoadingManager.EndLoading(loadingHandler);
                }
            };
            srv.RetourneNumeroRecuAsync( SessionObject.LaCaisseCourante.FK_IDCAISSE,UserConnecte.matricule );
        }

        private void translateControls()
        {
            try
            {
                this.Btn_Payement.Content = Langue.Btn_Payement;
                this.CancelButton.Content = Langue.Btn_annuler;
                this.button3.Content = Langue.button3;
                this.Chk_MulitPayement .Content = Langue.checkBox1;
                this.Chb_Resil.Content = Langue.Chb_Resil;
                this.button4.Content = Langue.button4;
                this.label1.Content = Langue.label1;
                this.label3.Content = Langue.label3;
                this.label4.Content = Langue.label4;
                this.label5.Content = Langue.label5;
                this.label6.Content = Langue.label6;
                this.hyperlinkButton1.Content = Langue.hyperlinkButtonhyperlinkButton1;
                this.Lsv_ListFacture.Columns[1].Header = Langue.Period;
                this.Lsv_ListFacture.Columns[2].Header = Langue.Bill_Number;
                this.Lsv_ListFacture.Columns[3].Header = Langue.Type;
                this.Lsv_ListFacture.Columns[4].Header = Langue.Due_on;
                this.Lsv_ListFacture.Columns[5].Header = Langue.Montant_due ;
                this.Lsv_ListFacture.Columns[6].Header = Langue.Transit_Payment;
                this.Lsv_ListFacture.Columns[7 ].Header = Langue.Amount_to_pay;
                this.BntNth.Content = Langue.Btn_Nth;
                this.Bntall.Content = Langue.Btn_all;
                this.lbl_message.Content = Langue.lbl_NbreLigneFacture;
                this.lbl_message2.Content = Langue.lbl_dont;
                this.lbl_messageexigible.Content = Langue.lbl_nombreExigible;
            }
            catch (Exception ex)
            {
              throw ex;
            }
        }            

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }

        private void dataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            TextBox tb = Lsv_ListFacture .Columns[9].GetCellContent(e.Row) as TextBox;
            if (tb != null)
                tb.LostFocus += tb_LostFocus;
        }

        void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Lsv_ListFacture.SelectedItem != null)
            {
                TextBox tb = Lsv_ListFacture.Columns[9].GetCellContent(e) as TextBox;
                CsLclient leClientSelect = (CsLclient)Lsv_ListFacture.SelectedItem;
                CsLclient leClient = ListeFactureAregle.FirstOrDefault(t => t.FK_IDCLIENT == leClientSelect.FK_IDCLIENT && t.NDOC == leClientSelect.NDOC && t.REFEM == leClientSelect.REFEM);
                if (leClient != null)
                    leClient.MONTANTPAYE = Convert.ToDecimal(tb.Text);
                    
                this.Txt_CurrentBalance.Text = (leClientSelect.SOLDECLIENT - ListeFactureAregle.Where(t => t.FK_IDCLIENT == leClientSelect.FK_IDCLIENT && t.Selectionner).Sum(p => p.MONTANTPAYE)).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t => t.Selectionner).Sum(p => p.MONTANTPAYE).Value.ToString(SessionObject.FormatMontant);
            }
        }
        void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }
        private void BntNth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Lsv_ListFacture.ItemsSource != null)
                {
                    var lesFactureSelect = ((List<CsLclient>)Lsv_ListFacture.ItemsSource).ToList();
                    foreach (CsLclient item in lesFactureSelect)
                        item.Selectionner = true;

                    ListeFactureAregle.Where(y => y.FK_IDCLIENT == lesFactureSelect.First().FK_IDCLIENT).ToList().ForEach(t => t.Selectionner = false );
                    ListeFactureAregle.Where(y => y.FK_IDCLIENT == lesFactureSelect.First().FK_IDCLIENT).ToList().ForEach(t => t.MONTANTPAYE = 0);
                    this.Txt_CurrentBalance.Text = lesFactureSelect.First().SOLDECLIENT.Value.ToString(SessionObject.FormatMontant);
                    this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t => t.Selectionner== true).Sum(p => p.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant); 
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Langue.errorTitle);
            }
        }

        private void Bntall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Lsv_ListFacture.ItemsSource != null)
                {
                    List<CsLclient> lesFactureSelect = ((List <CsLclient>)Lsv_ListFacture.ItemsSource).ToList();
                    foreach (CsLclient item in lesFactureSelect)
                        item.Selectionner = true;

                    ListeFactureAregle.Where(y => y.FK_IDCLIENT == lesFactureSelect.First().FK_IDCLIENT).ToList().ForEach(t => t.Selectionner = true);
                    ListeFactureAregle.Where(y => y.FK_IDCLIENT == lesFactureSelect.First().FK_IDCLIENT).ToList().ForEach(t => t.MONTANTPAYE  = t.SOLDEFACTURE );
                    this.Txt_CurrentBalance.Text = (lesFactureSelect.First().SOLDECLIENT - ListeFactureAregle.Where(t => t.FK_IDCLIENT == lesFactureSelect.First().FK_IDCLIENT && t.Selectionner == true).Sum(p => p.SOLDEFACTURE)).Value.ToString(SessionObject.FormatMontant);
                    this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t =>t.Selectionner).Sum(p => p.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
                    this.BntNth.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Langue.errorTitle);
            }
        }

        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_reference.Text) &&
                      txt_reference.Text.Length == TailleReferenceClient &&
                      txt_reference.Visibility == System.Windows.Visibility.Visible)
                {
                    this.txt_reference.Visibility = System.Windows.Visibility.Collapsed;
                    this.Cbo_ListeClient.Visibility = System.Windows.Visibility.Visible;
                    this.hyperlinkButton1.Visibility = System.Windows.Visibility.Collapsed;
                    this.hyperlinkButton2.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }
        DateTime tempsdeb = DateTime.Now;
        TimeSpan diffTemps = new TimeSpan();
        TimeSpan diffTemps1 = new TimeSpan();
        bool IsSaisiClavier = false;
        int tailleClient = 0;
        bool textboxChanged = false;
        string _ClientSaisieReference = string.Empty;
        public System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        public System.Windows.Threading.DispatcherTimer timer1 = new System.Windows.Threading.DispatcherTimer();
        
        private void txt_reference_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (SessionObject.EtatCaisse == SessionObject.Enumere.EtatDeCaisseNonCloture)
                {
                    Message.ShowInformation(Galatee.Silverlight.Resources.Caisse.Langue.MsgCaisseNonCloture, Galatee.Silverlight.Resources.Caisse.Langue.LibelleModule);
                    return;
                }
                if (txt_reference.Text.Length == 1)
                {
                    tempsdeb = DateTime.Now;
                    IsSaisiClavier = false;
                }
                decimal dummy;
                if (Chk_MulitPayement.IsChecked == false  && textboxChanged && string.IsNullOrEmpty(txt_reference.Text))
                    InitialiseControle();

                if (txt_reference.Text.Length == (SessionObject.Enumere.TailleCentre +
                                        SessionObject.Enumere.TailleClient +
                                        SessionObject.Enumere.TailleOrdre)
                                        && decimal.TryParse(txt_reference.Text, out dummy))
                {
                    diffTemps = DateTime.Now - tempsdeb;
                    if (diffTemps.Seconds > 1)
                        IsSaisiClavier = true;
                }

                if (!IsSaisiClavier )
                {
                    if (txt_reference.Text.Length == 18)
                    {
                        debutClient = SessionObject.Enumere.TailleCentre + 2;
                        debutOrdre = debutClient + SessionObject.Enumere.TailleClient;
                        string refClient = txt_reference.Text.Substring(1, SessionObject.Enumere.TailleCentre) +
                                          txt_reference.Text.Substring(debutClient, SessionObject.Enumere.TailleClient) +
                                           txt_reference.Text.Substring(debutOrdre, SessionObject.Enumere.TailleOrdre);
                        _ClientSaisieReference = refClient;
                    }
                }

                if (IsSaisiClavier && txt_reference.Text.Length == 16)
                    _ClientSaisieReference = txt_reference.Text;

                if (_ClientSaisieReference.Length == 16)
                    AfficherImpayes(_ClientSaisieReference);

                _ClientSaisieReference = string.Empty;
                if (string.IsNullOrEmpty(txt_reference.Text))
                    InitialiseControle();
            }
            catch (Exception ex)
            {
                
                throw ex;
            } 
        }
      private void AfficherImpayes(string LeClientSaisie)
        {
            try
            {
                 _UnClient = new CsClient();
                List<CsLclient> _ListeFactureClient = new List<CsLclient>();

                this.Lsv_ListFacture.ItemsSource = null;
                int debutClient = SessionObject.Enumere.TailleCentre;
                int debutOrdre = SessionObject.Enumere.TailleCentre + SessionObject.Enumere.TailleClient;

                if (this.Chk_MulitPayement.IsChecked == false)
                {
                  if (ListeFactureAregle != null && ListeFactureAregle.Count != 0)
                      ListeFactureAregle.Clear();
                  if (lstClient != null && lstClient.Count != 0)
                      lstClient.Clear();
                }
                _UnClient.CENTRE = LeClientSaisie.Substring(0, SessionObject.Enumere.TailleCentre);
                _UnClient.REFCLIENT = LeClientSaisie.Substring(debutClient, SessionObject.Enumere.TailleClient);
                _UnClient.ORDRE = LeClientSaisie.Substring(debutOrdre, SessionObject.Enumere.TailleOrdre);
           
                
                Galatee.Silverlight.ServiceCaisse.CaisseServiceClient service = new Galatee.Silverlight.ServiceCaisse.CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                service.RetourneListeFactureNonSoldeCaisseCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == null || args.Result.Count == 0)
                    {
                        Message.ShowInformation("Ce client n'existe pas", Langue.LibelleModule);
                        return;
                    }
                    List<CsLclient> lstFactureDuClient = args.Result;
                    lstFactureDuClient.ForEach(t => t.REFEMNDOC = t.REFEM);
                    lesEltInitial.AddRange(Shared.ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(lstFactureDuClient));
                    List<CsClient> LstClientDeLaReference = MethodeGenerics.RetourneClientFromFacture(lstFactureDuClient);
                    if (LstClientDeLaReference.Count > 1)
                    {
                        ListeFactureAregle.AddRange(lstFactureDuClient.OrderBy(t=>t.REFEMNDOC));

                        List<object> _LstObj = new List<object>();
                        _LstObj = ClasseMEthodeGenerique.RetourneListeObjet(LstClientDeLaReference);
                        Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                        _LstColonneAffich.Add("CENTRE", "CENTRE");
                        _LstColonneAffich.Add("LIBELLESITE", "SITE");
                        _LstColonneAffich.Add("NOMABON", "NOM CLIENT");

                        List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                        MainView.UcListeClientMultiple ctrl = new MainView.UcListeClientMultiple(obj, _LstColonneAffich, false, "Lots");
                        ctrl.Closed += new EventHandler(galatee_OkClickedChoixClient);
                        this.IsEnabled = false;
                        ctrl.Show();
                        return;

                    }

                    _UnClient.SOLDE = lstFactureDuClient.First().SOLDECLIENT;
                    _UnClient.PK_ID = lstFactureDuClient.First().FK_IDCLIENT;
                    _UnClient.FK_IDCENTRE  = lstFactureDuClient.First().FK_IDCENTRE ;
                    _UnClient.REFERENCEATM = _UnClient.CENTRE + _UnClient.REFCLIENT + _UnClient.ORDRE;
                   
                    lstClient.Add(_UnClient);
                    Cbo_ListeClient.ItemsSource = null;
                    Cbo_ListeClient.ItemsSource = lstClient;
                    Cbo_ListeClient.DisplayMemberPath = "REFERENCEATM";
                   

                    this.TxtNomClient.Text = string.IsNullOrEmpty(lstFactureDuClient.First().NOM) ? string.Empty : lstFactureDuClient.First().NOM;
                    this.TxtAddress.Text = string.IsNullOrEmpty(lstFactureDuClient.First().ADRESSE) ? string.Empty : lstFactureDuClient.First().ADRESSE;
                    this.Txt_CurrentBalance.Text = lstFactureDuClient.Sum(p => p.SOLDEFACTURE ).Value.ToString(SessionObject.FormatMontant);
                    this.lbl_MontantTotFacture.Content = lstFactureDuClient.Sum(p => p.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant); 
                    this.lbl_NbreFacture.Content = lstFactureDuClient.Count;
                    this.lbl_NombreExigible.Content = lstFactureDuClient.Where(p => p.EXIGIBILITE < DateTime.Today.Date).Count();
                    this.lbl_MontantTotExigible.Content = lstFactureDuClient.Where(p => p.EXIGIBILITE < DateTime.Today.Date).Sum(p => p.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
                    this.txt_totaldue.Text = lstFactureDuClient.First().SOLDECLIENT.Value.ToString(SessionObject.FormatMontant);
                    decimal? totNonExigible = lstFactureDuClient.Where(p => p.EXIGIBILITE > DateTime.Today.Date).Sum(p => p.SOLDEFACTURE);
                    foreach (CsLclient item in lstFactureDuClient)
                    {
                        item.MONTANTEXIGIBLE = Convert.ToDecimal(this.lbl_MontantTotExigible.Content);
                        item.MONTANTNONEXIGIBLE = totNonExigible;
                    }
                    if (lstFactureDuClient.Count == 1 && lstFactureDuClient.First().IsPAIEMENTANTICIPE)
                    {
                        string referenceclient = _UnClient.CENTRE + _UnClient.REFCLIENT + _UnClient.ORDRE;
                        _UnClient.FK_IDCENTRE = lstFactureDuClient.First().FK_IDCENTRE;
                        _UnClient.PK_ID  = lstFactureDuClient.First().FK_IDCLIENT  ; 
                        FrmPayementAnticipe frmPA = new FrmPayementAnticipe(_UnClient, Langue.TypePaiementAnticipe);
                        frmPA.Closed += new EventHandler(GetPayementAnticipe_OkClicked);
                        frmPA.Show();

                    }
                    else
                    {
                        
                        ListeFactureAregle.AddRange(lstFactureDuClient.OrderBy(t=>t.REFEMNDOC).ToList());
                        List<CsClient> LstClient = MethodeGenerics.RetourneClientFromFacture(ListeFactureAregle);
                        Txt_NombreClient.Text = LstClient.Count.ToString();
                        lstFactureDuClient.ForEach(t => t.REFEM = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(t.REFEM));
                        foreach (var item in lstFactureDuClient)
                        {
                            item.MONTANTPAYPARTIEL = item.MONTANT - item.SOLDEFACTURE;
                            if (item.MONTANTPAYPARTIEL < 0) item.MONTANTPAYPARTIEL = 0;
                        }
                        lstFactureDuClient.ForEach(t => t.MONTANTPAYPARTIEL  = t.MONTANT - t.SOLDEFACTURE );


                        Lsv_ListFacture.ItemsSource = null;
                        Lsv_ListFacture.ItemsSource = ListeFactureAregle.Where(t => t.FK_IDCLIENT == lstFactureDuClient.First().FK_IDCLIENT).ToList();
                    }
                    Btn_Payement.Focus();
                };
                service.RetourneListeFactureNonSoldeCaisseAsync(_UnClient);
                service.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
          }

      private void galatee_OkClickedChoixClient(object sender, EventArgs e)
      {
          this.IsEnabled = true ;
          Galatee.Silverlight.MainView.UcListeClientMultiple ctrs = sender as Galatee.Silverlight.MainView.UcListeClientMultiple;
          if (ctrs.isOkClick)
          {
              _UnClient = (CsClient)ctrs.MyObject;
              lstClient.Add(_UnClient);
              lstClient.ForEach(c => c.REFERENCEATM = c.CENTRE + c.REFCLIENT + c.ORDRE);
              Cbo_ListeClient.ItemsSource = null;
              Cbo_ListeClient.ItemsSource = lstClient;
              Cbo_ListeClient.DisplayMemberPath = "REFERENCEATM";


              List<CsLclient> lstFactureDuClient = ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(ListeFactureAregle.Where(t => t.FK_IDCLIENT == _UnClient.PK_ID).ToList());
              ListeFactureAregle.Clear();
              this.TxtNomClient.Text = string.IsNullOrEmpty(lstFactureDuClient.First().NOM) ? string.Empty : lstFactureDuClient.First().NOM;
              this.TxtAddress.Text = string.IsNullOrEmpty(lstFactureDuClient.First().ADRESSE) ? string.Empty : lstFactureDuClient.First().ADRESSE;
              this.Txt_CurrentBalance.Text = lstFactureDuClient.Sum(p => p.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
              this.lbl_MontantTotFacture.Content = lstFactureDuClient.Sum(p => p.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
              this.lbl_NbreFacture.Content = lstFactureDuClient.Count;
              this.lbl_NombreExigible.Content = lstFactureDuClient.Where(p => p.EXIGIBILITE < DateTime.Today.Date).Count();
              this.lbl_MontantTotExigible.Content = lstFactureDuClient.Where(p => p.EXIGIBILITE < DateTime.Today.Date).Sum(p => p.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
              this.txt_totaldue.Text = lstFactureDuClient.First().SOLDECLIENT.Value.ToString(SessionObject.FormatMontant);
              decimal? totNonExigible = lstFactureDuClient.Where(p => p.EXIGIBILITE > DateTime.Today.Date).Sum(p => p.SOLDEFACTURE);
              _UnClient.SOLDE = lstFactureDuClient.First().SOLDECLIENT;
              foreach (CsLclient item in lstFactureDuClient)
              {
                  item.MONTANTEXIGIBLE = Convert.ToDecimal(this.lbl_MontantTotExigible.Content);
                  item.MONTANTNONEXIGIBLE = totNonExigible;
              }
              Txt_NombreClient.Text = "1";
              ListeFactureAregle = lstFactureDuClient;
              if (!ListeFactureAregle.FirstOrDefault().IsPAIEMENTANTICIPE)
              {
                  lstFactureDuClient.ForEach(t => t.REFEM = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(t.REFEM));
                  Lsv_ListFacture.ItemsSource = null;
                  Lsv_ListFacture.ItemsSource = ListeFactureAregle.Where(t => t.FK_IDCLIENT == lstFactureDuClient.First().FK_IDCLIENT).OrderBy(t => t.REFEMNDOC).ToList();
              }
              else
              {
                  string referenceclient = _UnClient.CENTRE + _UnClient.REFCLIENT + _UnClient.ORDRE;
                  _UnClient.FK_IDCENTRE = lstFactureDuClient.First().FK_IDCENTRE;
                  _UnClient.PK_ID = lstFactureDuClient.First().FK_IDCLIENT ;
                  ListeFactureAregle.Clear();
                  FrmPayementAnticipe frmPA = new FrmPayementAnticipe(_UnClient, Langue.TypePaiementAnticipe);
                  frmPA.Closed += new EventHandler(GetPayementAnticipe_OkClicked);
                  frmPA.Show();
              }
          }
      }


      void GetPayementAnticipe_OkClicked(object sender, EventArgs e)
      {
          try
          {
              FrmPayementAnticipe ctrs = sender as FrmPayementAnticipe;

              ListeFactureAregle.Add(ctrs.leNaf);
              List<CsLclient> lstFactureDuClient = ListeFactureAregle.Where(f => f.FK_IDCLIENT == _UnClient.PK_ID).ToList();
              List<CsClient> LstClient = MethodeGenerics.RetourneClientFromFacture(ListeFactureAregle);
              Txt_NombreClient.Text = "1";
              lstFactureDuClient.ForEach(t => t.REFEM = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(t.REFEM));
              Lsv_ListFacture.ItemsSource = null;
              Lsv_ListFacture.ItemsSource = ListeFactureAregle.Where(t => t.FK_IDCLIENT == lstFactureDuClient.First().FK_IDCLIENT).OrderBy(t => t.REFEM).ThenBy(y => y.NDOC).ToList();
              this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t => t.Selectionner).Sum(p => p.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);

          }
          catch (Exception ex)
          {
              Message.ShowError(ex, Langue.errorTitle);
          }


      }
        private void hyperlinkButton2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.txt_reference.Visibility = System.Windows.Visibility.Visible;
                this.Cbo_ListeClient.Visibility = System.Windows.Visibility.Collapsed;
                this.hyperlinkButton2.Visibility = System.Windows.Visibility.Collapsed;
                this.hyperlinkButton1.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
              Message.ShowError(ex, Langue.errorTitle);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FrmRechercheClient recherche = new FrmRechercheClient();
                recherche.Closed += new EventHandler(FrmRechercheClientClosed);
                recherche.Show();
            }
            catch (Exception ex)
            {
             Message.ShowError(ex, Langue.errorTitle);
            }
        }

        void FrmRechercheClientClosed(object sender, EventArgs e)
        {
            txt_reference.Focus();
            FrmRechercheClient recherche = sender as FrmRechercheClient;
            try
            {
                if(recherche.CustomerRef!=null)
                txt_reference.Text = recherche.CustomerRef;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
     

        private void Lsv_ListFacture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (this.Lsv_ListFacture.SelectedItem != null)
            //{
            //    CsLclient leClientSelect = (CsLclient)Lsv_ListFacture.SelectedItem;
            //    CsLclient leClient = ListeFactureAregle.FirstOrDefault(t => t.FK_IDCLIENT == leClientSelect.FK_IDCLIENT && t.NDOC == leClientSelect.NDOC && t.REFEM == leClientSelect.REFEM);
            //    if (leClient != null)
            //    {
            //        if (leClient.Selectionner == false)
            //        {
            //            leClient.Selectionner = true;
            //            leClient.MONTANTPAYE = leClient.SOLDEFACTURE;
            //        }
            //        else
            //        {
            //            leClient.Selectionner = false;
            //            leClient.MONTANTPAYE = null;
            //        }
            //    }
            //    this.Txt_CurrentBalance.Text = (leClientSelect.SOLDECLIENT - ListeFactureAregle.Where(t => t.FK_IDCLIENT == leClientSelect.FK_IDCLIENT && t.Selectionner).Sum(p => p.MONTANTPAYE)).Value.ToString(SessionObject.FormatMontant);
            //    this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t => t.Selectionner).Sum(p => p.MONTANTPAYE).Value.ToString(SessionObject.FormatMontant);
            //}
        }

        private void Cbo_ListeClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Cbo_ListeClient.SelectedItem != null)
            {
                CsClient leClientSelect = (CsClient)Cbo_ListeClient.SelectedItem;
                Lsv_ListFacture.ItemsSource = null;
                Lsv_ListFacture.ItemsSource = ListeFactureAregle.Where(t => t.FK_IDCLIENT == leClientSelect.PK_ID ).ToList();
                this.Txt_CurrentBalance.Text = (leClientSelect.SOLDE - ListeFactureAregle.Where(t => t.FK_IDCLIENT == leClientSelect.PK_ID  && t.Selectionner == true).Sum(p => p.SOLDEFACTURE)).Value.ToString(SessionObject.FormatMontant);
            }
        }

        private void Chk_MulitPayement_Checked(object sender, RoutedEventArgs e)
        {
            this.hyperlinkButton1.Visibility = System.Windows.Visibility.Visible;
        }

        private void Chk_MulitPayement_Unchecked(object sender, RoutedEventArgs e)
        {
            this.hyperlinkButton1.Visibility = System.Windows.Visibility.Collapsed;
        }
        public void InitialiseControle()
        {

            txt_reference.Text = string.Empty;
            _ClientSaisieReference = string.Empty;
            Cbo_ListeClient.Visibility = System.Windows.Visibility.Collapsed;
            txt_reference.Visibility = System.Windows.Visibility.Visible;
            Lsv_ListFacture.ItemsSource = null;
            BntNth.IsEnabled = false;
            button3.IsEnabled = true;
            ListeFactureAregle = new List<CsLclient>();
            this.txt_reference.Focus();
            this.Txt_CurrentBalance.Text = string.Empty;
            this.Txt_NombreClient.Text = string.Empty;
            this.Txt_CurrentBalance.Text = string.Empty;
            this.txt_totaldue.Text = string.Empty;
            this.TxtAddress.Text = string.Empty;
            this.TxtNomClient.Text = string.Empty;
            this.Txt_TotalAPayer.Text = string.Empty;
            this.lbl_NbreFacture.Content = string.Empty;
            this.lbl_MontantTotFacture.Content = string.Empty;
            this.lbl_NombreExigible.Content = string.Empty;
            this.lbl_MontantTotExigible.Content = string.Empty;
            this.Lsv_ListFacture.ItemsSource = null;
            this.Lsv_ListFacture.UpdateLayout();
            ListeFactureAregle.Clear();
            lesEltInitial.Clear();
            this.Cbo_ListeClient.ItemsSource = null;
            lstClient.Clear();
            this.txt_reference.Focus();
            this.txt_reference.MaxLength = 18;
            this.hyperlinkButton1.Visibility = System.Windows.Visibility.Collapsed;
            this.Chk_MulitPayement.IsChecked = false;

            if (SessionObject.LePosteCourant == null || SessionObject.LePosteCourant.FK_IDCAISSE == null || SessionObject.LePosteCourant.FK_IDCAISSE == 0 || SessionObject.LePosteCourant.FK_IDCENTRE == null)
            {
                Message.ShowError("Problème d'identification du poste", Langue.errorTitle);
                return;
            }

            if (SessionObject.LaCaisseCourante == null || SessionObject.LaCaisseCourante.FK_IDCAISSE == 0)
                ChargerMonHabilitationCaisse();
            else
                //ChargerSituationCaisse();
                EtatCaisse();
        }




        private void ChargerMonHabilitationCaisse()
        {

            CsPoste lePoste = new CsPoste();
            lePoste.CODECENTRE = SessionObject.LePosteCourant.CODECENTRE;
            lePoste.FK_IDCAISSE = SessionObject.LePosteCourant.FK_IDCAISSE;
            lePoste.FK_IDCENTRE = SessionObject.LePosteCourant.FK_IDCENTRE;
            lePoste.IsSelect = SessionObject.LePosteCourant.IsSelect;
            lePoste.MATRICULE = SessionObject.LePosteCourant.MATRICULE;
            lePoste.NOMPOSTE = SessionObject.LePosteCourant.NOMPOSTE;
            lePoste.NUMCAISSE = SessionObject.LePosteCourant.NUMCAISSE;
            lePoste.PK_ID = SessionObject.LePosteCourant.PK_ID;

            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            //service.RetouneLaCaisseCouranteAsync(UserConnecte.matricule);
            service.RetouneLaCaisseCouranteInsereeAsync(UserConnecte.matricule, lePoste);
            service.RetouneLaCaisseCouranteInsereeCompleted += (sender, es) =>
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
                    //ChargerSituationCaisse();
                    EtatCaisse();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
            };

        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            InitialiseControle();
        }
        private void Chk_TvaExo_Checked(object sender, RoutedEventArgs e)
        {
            List<CsLclient> lstFactureAExonerer = ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(ListeFactureAregle.Where(t=>!t.Selectionner || (t.Selectionner && t.IsExonerationTaxe )).ToList());
            UcExonerationFacture form = new UcExonerationFacture(lstFactureAExonerer);
            form.Closed += new EventHandler(formClosed);
            form.Show();
        }
        void formClosed(object sender, EventArgs e)
        {
            try
            {
                UcExonerationFacture ctrs = sender as UcExonerationFacture;
                if (ctrs.lesFacturesExonere != null && ctrs.lesFacturesExonere.Count != 0)
                {
                    var lstFactureExonerer = from x in ListeFactureAregle
                                             join y in ctrs.lesFacturesExonere on x.PK_ID equals y.PK_ID
                                             select x;

                    foreach (CsLclient item in lstFactureExonerer)
                    {
                        decimal montantfact = (item.SOLDEFACTURE  - (item.MONTANTTVA == null ? 0 : item.MONTANTTVA)).Value;
                        item.SOLDEFACTURE = (montantfact - item.MONTANTPAYPARTIEL);
                        item.MONTANTPAYE = item.SOLDEFACTURE;
                        item.Selectionner = true;
                        item.IsExonerationTaxe = true;
                    }
                    var lstFactureNonExonerer = from x in ListeFactureAregle
                                                where !ctrs.lesFacturesExonere.Any(t => t.PK_ID == x.PK_ID)
                                                select x;

                    foreach (CsLclient item in lstFactureNonExonerer)
                    {
                        decimal montantfact = (item.SOLDEFACTURE + (item.MONTANTTVA == null ? 0 : item.MONTANTTVA)).Value;
                        item.SOLDEFACTURE = montantfact - item.MONTANTPAYPARTIEL;
                        if (item.Selectionner)
                            item.MONTANTPAYE = item.SOLDEFACTURE;
                        else
                            item.MONTANTPAYE = null;
                        item.IsExonerationTaxe = false;
                    }
                    //Chk_TvaExo.IsChecked = false;
                }
                else
                {
                    foreach (CsLclient item in ListeFactureAregle )
                    {
                        decimal montantfact = (item.SOLDEFACTURE + (item.MONTANTTVA == null ? 0 : item.MONTANTTVA)).Value;
                        item.SOLDEFACTURE = montantfact - item.MONTANTPAYPARTIEL;
                        if (item.Selectionner)
                            item.MONTANTPAYE = item.SOLDEFACTURE;
                        else
                            item.MONTANTPAYE = null;

                        item.IsExonerationTaxe = false;
                    }
                    //Chk_TvaExo.IsChecked = false;
                }
                this.Txt_CurrentBalance.Text = (_UnClient.SOLDE - ListeFactureAregle.Where(t => t.FK_IDCLIENT == _UnClient.PK_ID && t.Selectionner == true).Sum(p => p.SOLDEFACTURE)).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t => t.Selectionner).Sum(p => p.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }

        }
        bool IsExoneration = false;
        private void Chk_TvaExo_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Lsv_ListFacture.ItemsSource  != null && IsExoneration == true)
            {
                CsLclient laFactureSelect = ((List<CsLclient>)Lsv_ListFacture.ItemsSource).First();
                foreach (CsLclient item in ListeFactureAregle.Where(t=>t.FK_IDCLIENT == laFactureSelect.FK_IDCLIENT ).ToList())
                {

                    decimal montantfact = (item.MONTANT + (item.MONTANTTVA == null ? 0 : item.MONTANTTVA)).Value;
                    item.MONTANT = montantfact;
                    item.SOLDEFACTURE = (item.MONTANT + item.MONTANTPAYPARTIEL);
                }
                IsExoneration = false;
                this.Txt_CurrentBalance.Text = (laFactureSelect.SOLDECLIENT - ListeFactureAregle.Where(t => t.FK_IDCLIENT == laFactureSelect.FK_IDCLIENT && t.Selectionner == true).Sum(p => p.SOLDEFACTURE)).Value.ToString(SessionObject.FormatMontant);
                this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t => t.Selectionner).Sum(p => p.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant); 
            }
        }

        private void Btn_Payement_Click(object sender, RoutedEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
            timer1 = new System.Windows.Threading.DispatcherTimer();
            timer1.Interval = new TimeSpan(3, 0, 0);
            timer1.Tick += timer_Tick;
            timer1.Start();

            List<CsLclient> lstFacture = ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(ListeFactureAregle);
            lstFacture.ForEach(t => t.COPER = SessionObject.Enumere.CoperRGT);
            lstFacture.ForEach(t => t.SOLDEFACTURE = t.MONTANTPAYE != null ? t.MONTANTPAYE : t.SOLDEFACTURE);
            //lstFacture.ForEach(t => t.SOLDEFACTURE = t.MONTANTPAYE );
            UcValideEncaissement UcValider = new UcValideEncaissement(lstFacture, SessionObject.Enumere.ActionRecuEditionNormale);
            this.IsEnabled = false;
            UcValider.Closed += new EventHandler(UcValideEncaissementClosed);
            UcValider.Show();
           

        }

        private void PayementNormal(List<CsLclient> ListeDeFacture)
        {
            try
            {
                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void UcValideEncaissementClosed(object sender, EventArgs e)
        {
            try
            {
                this.IsEnabled =true ;
                UcValideEncaissement ctrs = sender as UcValideEncaissement;
                if (ctrs.Yes)
                    InitialiseControle();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }

        }
        DateTime lastClick = DateTime.Now;
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource  as List<CsLclient >;
            if (dg.SelectedItem != null)
            {
                CsLclient SelectedObject = (CsLclient)dg.SelectedItem;
                if (SelectedObject.Selectionner == false)
                {
                    SelectedObject.Selectionner = true;
                    SelectedObject.MONTANTPAYE = SelectedObject.SOLDEFACTURE;
                }
                else 
                {
                    SelectedObject.Selectionner = false;
                    SelectedObject.MONTANTPAYE = null;
                }
                if ((DateTime.Now - lastClick).Ticks < 2500000)
                {
                    UcSaisiMontant ctrl = new UcSaisiMontant(SelectedObject);
                    ctrl.Closed += ctrl_Closed;
                    this.IsEnabled = false;
                    ctrl.Show();
                }
                lastClick = DateTime.Now;
                    this.Txt_CurrentBalance.Text = (SelectedObject.SOLDECLIENT - ListeFactureAregle.Where(t => t.FK_IDCLIENT == SelectedObject.FK_IDCLIENT && t.Selectionner).Sum(p => p.MONTANTPAYE)).Value.ToString(SessionObject.FormatMontant);
                    this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t => t.Selectionner).Sum(p => p.MONTANTPAYE).Value.ToString(SessionObject.FormatMontant);

            }
        }

        void ctrl_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            UcSaisiMontant ctrs = sender as UcSaisiMontant;
            if (ctrs.isOkClick )
            {
                CsLclient leClientMofi = ListeFactureAregle.First(t => t.PK_ID == ctrs.SelectedObject.PK_ID);
                if (leClientMofi != null)
                {
                    if (leClientMofi.SOLDEFACTURE < ctrs.SelectedObject.MONTANTPAYE)
                    {
                        Message.ShowInformation("Le montant partiel doit etre inferieur au montant de la facture", "Caisse");
                        leClientMofi.MONTANTPAYE = null ;
                        leClientMofi.Selectionner = false ;
                        return;
                    }
                    else
                    {
                        leClientMofi.MONTANTPAYE = ctrs.SelectedObject.MONTANTPAYE;
                        leClientMofi.Selectionner = true;
                    }
                }
                  btn_Rafraichir_Click(null, null);
            }
        }

        private void btn_Rafraichir_Click(object sender, RoutedEventArgs e)
        {
            this.Txt_CurrentBalance.Text = (_UnClient.SOLDE - ListeFactureAregle.Where(t => t.FK_IDCLIENT == _UnClient.PK_ID  && t.Selectionner).Sum(p => p.MONTANTPAYE)).Value.ToString(SessionObject.FormatMontant);
            this.Txt_TotalAPayer.Text = ListeFactureAregle.Where(t => t.Selectionner).Sum(p => p.MONTANTPAYE).Value.ToString(SessionObject.FormatMontant);
        }

        private void Lsv_LigneNaf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void chk_Exonere_Checked(object sender, RoutedEventArgs e)
        {
            List<CsLclient> lstFactureExoneres = (List<CsLclient>)this.Lsv_ListFacture.ItemsSource;
            if (lstFactureExoneres != null && this.Lsv_ListFacture.SelectedItem != null)
            {
                CsLclient leEltsSelect = lstFactureExoneres.FirstOrDefault(t => t.PK_ID == ((CsLclient)this.Lsv_ListFacture.SelectedItem).PK_ID);
                IsExonnere(leEltsSelect, true);
            }
        }

        private void chk_Exonere_Unchecked(object sender, RoutedEventArgs e)
        {
            List<CsLclient> lstFactureExoneres = (List<CsLclient>)this.Lsv_ListFacture.ItemsSource;
            if (lstFactureExoneres != null && this.Lsv_ListFacture.SelectedItem != null)
            {
                CsLclient leEltsSelect = lstFactureExoneres.FirstOrDefault(t => t.PK_ID == ((CsLclient)this.Lsv_ListFacture.SelectedItem).PK_ID);
                IsExonnere(leEltsSelect, false);
            }
        }

        private void IsExonnere(CsLclient leElt, bool IsChecked)
        {
            try
            {
                List<CsLclient> lstEltfacture = new List<CsLclient>();
                lstEltfacture = ((List<CsLclient>)Lsv_ListFacture.ItemsSource).ToList();
                CsLclient leElts = lstEltfacture.FirstOrDefault(t => t.PK_ID == leElt.PK_ID);
                CsLclient leEltss = lesEltInitial.FirstOrDefault(t => t.PK_ID == leElt.PK_ID);
                if (leElts != null)
                {
                    if (IsChecked)
                    {
                        decimal montantfact = (leEltss.SOLDEFACTURE - (leEltss.MONTANTTVA == null ? 0 : leEltss.MONTANTTVA)).Value;
                        leElts.MONTANTPAYE = montantfact;
                        leElts.SOLDEFACTURE = montantfact;
                        leElts.IsExonerationTaxe = true;
                        
                    }
                    else
                    {
                        decimal montantfact = leEltss.SOLDEFACTURE.Value;
                        leElts.MONTANTPAYE = montantfact;
                        leElts.SOLDEFACTURE = montantfact;
                        leElts.IsExonerationTaxe = false;
                    }
                    if (Lsv_ListFacture.ItemsSource != null)
                        this.Txt_TotalAPayer.Text = ((List<CsLclient>)Lsv_ListFacture.ItemsSource).Sum(y => y.MONTANTPAYE).Value.ToString(SessionObject.FormatMontant);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        bool checkSelectedItem(CheckBox check)
        {
            CheckBox chk = check;
            return chk.IsChecked.Value;
        }

        void checkerSelectedItem(CheckBox check)
        {
            try
            {
                CheckBox chk = check;
                if (chk.IsChecked.Value)
                    chk.IsChecked = false;
                else
                    chk.IsChecked = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerSituationCaisse()
        {
            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.RetourneEtatDeCaisseAsync(SessionObject.LaCaisseCourante);
            service.RetourneEtatDeCaisseCompleted += (sender, es) =>
            {
                try
                {
                    if (es.Cancelled || es.Error != null)
                    {
                        Message.ShowError("Erreur d'invocation du service . Veuillez réssayer svp!", Galatee.Silverlight.Resources.Langue.errorTitle);
                        this.DialogResult = true;
                    }
                    if (es.Result == null) return;
                    List<CsLclient> _LstReglement = new List<CsLclient>();
                    _LstReglement.AddRange(es.Result);

                    if (_LstReglement.Count > 0)
                    {
                        this.dtg_EtatCaisse.ItemsSource = null;
                        this.dtg_EtatCaisse.ItemsSource = _LstReglement;
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
            };

        }


        private void RetourneLstFraixTimbre()
        {
            string ServerMode = "Online";

            CaisseServiceClient cais = new CaisseServiceClient(Utility.Protocole(), Utility.EndPoint("Caisse"));
            cais.RetourneListeTimbreCompleted += (send, aa) =>
            {
                try
                {
                    if (aa.Cancelled || aa.Error != null)
                    {
                        Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }
                    if (aa.Result == null || aa.Result.Count == 0)
                    {
                        Message.Show(Galatee.Silverlight.Resources.Langue.msgNodata, "RetourneListeTimbre =>" + Galatee.Silverlight.Resources.Langue.informationTitle);
                        return;
                    }
                    SessionObject.LstFraisTimbre = aa.Result;

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            };
            cais.RetourneListeTimbreAsync(ServerMode);

        }



    }
}


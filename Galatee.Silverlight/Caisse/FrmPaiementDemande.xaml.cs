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
using Galatee.Silverlight.Resources.Caisse ;
using System.Globalization;
//using Galatee.Silverlight.serviceWeb;

namespace Galatee.Silverlight.Caisse
{
    public partial class FrmPaiementDemande : ChildWindow
    {
        List<ServiceCaisse.CsLclient> LstFactureDeLaDemande;
        List<ServiceCaisse.CsCtax> LstDesTaxe = new List<ServiceCaisse.CsCtax>();
        decimal _LetauxMinimal = 0;
        decimal _LeMonantMinimal = 0;
        decimal? TotalHt = 0;
        decimal? TotalTaxe = 0;
        decimal? TotalTTc = 0;


        public FrmPaiementDemande()
        {
            InitializeComponent();

            try
            {
                //ListeDesTaxes(null, null);
                Txt_NumeroDemande.IsEnabled = false;
                translate();
                this.Txt_NumeroDemande.MaxLength = SessionObject.Enumere.TailleNumeroDemande;
                ChargerTauxMini("000231");
                this.Chk_DevisPrestation.Visibility = System.Windows.Visibility.Collapsed ;



                if (SessionObject.LePosteCourant == null || SessionObject.LePosteCourant.FK_IDCAISSE == null || SessionObject.LePosteCourant.FK_IDCAISSE == 0 || SessionObject.LePosteCourant.FK_IDCENTRE == null)
                {
                    Message.ShowError("Problème d'identification du poste", Langue.errorTitle);
                    return;
                }



                if (SessionObject.LaCaisseCourante == null || SessionObject.LaCaisseCourante.FK_IDCAISSE == 0)
                    ChargerHabilitationCaisse();
                else
                    RecuperationNumCaisse();


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





        bool IsExtension = false;
        public FrmPaiementDemande(string _IsExtention)
        {
            InitializeComponent();

            try
            {
                //ListeDesTaxes(null, null);
                Txt_NumeroDemande.IsEnabled = false;
                translate();
                this.Txt_NumeroDemande.MaxLength = SessionObject.Enumere.TailleNumeroDemande;
                ChargerTauxMini("000231");
                //RecuperationNumCaisse();
                IsExtension = true;

                if (SessionObject.LePosteCourant == null || SessionObject.LePosteCourant.FK_IDCAISSE == null || SessionObject.LePosteCourant.FK_IDCAISSE == 0 || SessionObject.LePosteCourant.FK_IDCENTRE == null)
                {
                    Message.ShowError("Problème d'identification du poste", Langue.errorTitle);
                    return;
                }



                if (SessionObject.LaCaisseCourante == null || SessionObject.LaCaisseCourante.FK_IDCAISSE == 0)
                    ChargerHabilitationCaisse();
                else
                    RecuperationNumCaisse();

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




        void translate()
        {
            try
            {
                //this.lbl_site.Content = Langue.Site;
                this.lbl_Nom.Content = Langue.Nom;
                this.lbl_RefClient.Content = Langue.Reference_client;
                this.lbl_NumDemande.Content = Langue.Numero_requete;
                this.lbl_total.Content = Langue.lbl_TotalDu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                    RecuperationNumCaisse();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
            };

        }


        private void RecuperationNumCaisse()
        {
           


            if (SessionObject.DernierNumeroDeRecu <= 0 )
            {
            CaisseServiceClient srv;
            srv = new CaisseServiceClient(Utility.Protocole(), Utility.EndPoint("Caisse"));
            srv.RetourneNumeroRecuAsync(SessionObject.LaCaisseCourante.FK_IDCAISSE ,UserConnecte.matricule );
            srv.RetourneNumeroRecuCompleted += (s, es) =>
            {
                try
                {
                    if (es.Error != null || es.Cancelled)
                    {
                        Message.ShowError("Erreur! :" + es.Error.Message, "Erreur");
                        return;
                    }
                    if (es.Result == null)
                    {
                        Message.ShowError("Erreur", "Erreur");
                        return;
                    }
                    else
                        SessionObject.DernierNumeroDeRecu = Decimal.Parse(es.Result);
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
                finally
                {
                }
            };
        }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txt_MontantPayes.Text) &&
                !string.IsNullOrEmpty(this.Txt_TotalTTC.Text) &&
                decimal.Parse(this.txt_MontantPayes.Text) > decimal.Parse(this.Txt_TotalTTC.Text))
            {
                var w = new MessageBoxControl.MessageBoxChildWindow(Langue.LibelleModule, Langue.Msg_MontantPayeSuperieur + "  " + this.Txt_TotalTTC.Text, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                w.OnMessageBoxClosed += (_, result) =>
                {
                };
                w.Show();
            }
            else 
            {
                if (!string.IsNullOrEmpty(this.txt_MontantPayes.Text) && VerifieMontantPaye())
                {
                    List<CsLclient> _LesFactureFactureInit =(List<CsLclient>) this.LsvFacture.ItemsSource ;
                    List<CsLclient> _LesFactureASolde = SoldeFacture(_LesFactureFactureInit, decimal.Parse(this.txt_MontantPayes.Text));
                    if (!_LesFactureASolde.First().ISDECAISSEMENT)
                    {
                        UcValideEncaissement UcValider = new UcValideEncaissement(_LesFactureASolde, SessionObject.Enumere.OperationDeCaisseEncaissementDevis);
                        UcValider.Closed += new EventHandler(UcValideEncaissementClosed);
                        UcValider.Show();
                    }
                    if (_LesFactureASolde.First().ISDECAISSEMENT)
                    {
                        UcValideEncaissement UcValider = new UcValideEncaissement(_LesFactureASolde, SessionObject.Enumere.OperationDeCaisseEncaissementDevis,"OUI");
                        UcValider.Closed += new EventHandler(UcValideEncaissementClosed);
                        UcValider.Show();
                    }
                }
            }
        }

        void UcValideEncaissementClosed(object sender, EventArgs e)
        {
            try
            {
                UcValideEncaissement ctrs = sender as UcValideEncaissement;

                if (ctrs.Yes)
                    InitControle();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void InitControle()
        {
            try
            {
                txt_MontantPayes.Text = string.Empty;
                Txt_Client.Text = string.Empty;
                Txt_NomClient.Text = string.Empty;
                Txt_TotalHt.Text = string.Empty;
                Txt_totalTaxe.Text = string.Empty;
                Txt_TotalTTC.Text = string.Empty;
                Txt_NumeroDemande.Text = string.Empty;
                LsvFacture.ItemsSource = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ListeDesTaxes(string Centre, string ctax)
        {

            LstDesTaxe = new List<ServiceCaisse.CsCtax>();
            CaisseServiceClient service = new CaisseServiceClient(Utility.Protocole(), Utility.EndPoint("Caisse"));
            service.RetourneListeTaxeCompleted += (s, args) =>
            {
                if (args.Error != null || args.Cancelled)
                {
                    Message.ShowError("Erreur du serveur. Réessayer svp!", Langue.errorTitle);
                    return;
                }
                try
                {
                    LstDesTaxe = args.Result;
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
            };
            service.RetourneListeTaxeAsync();
        }

        void ChargerTauxMini(string code)
        {
            CaisseServiceClient service = new CaisseServiceClient(Utility.Protocole(), Utility.EndPoint("Caisse"));
            service.RetourneListeTa58Completed += (s, args) =>
            {
                    if (args.Error != null && args.Cancelled)
                    {
                        Message.ShowError("Erreur du serveur. Réessayer svp!", Langue.errorTitle);
                        return;
                    }

                    if (args.Result == null)
                    {
                        Message.ShowError("Echec lors de la récupération du taux minimal.Réessayer svp!", Langue.errorTitle);
                        return;
                    }

                    try
                    {
                        Txt_NumeroDemande.IsEnabled = true;
                            CsParametresGeneraux  _LeParametre = args.Result;
                            _LetauxMinimal = int.Parse(_LeParametre.LIBELLE);
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Langue.errorTitle);
                    }
            };
            service.RetourneListeTa58Async(code);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {

        }
    
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Chargercontrole();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }

        private List<CsLclient> SoldeFacture(List<CsLclient> _LstFacture, decimal _MontantPayes)
        {
            List<CsLclient> LesFactureSolde = new List<CsLclient>();
            decimal? _montantRest = _MontantPayes ;
            foreach (CsLclient _laFact in _LstFacture)
            {
                if (_laFact.COPER == SessionObject.Enumere.CoperACT ||
                    _laFact.COPER == SessionObject.Enumere.CoperRembTrvx ||
                    _laFact.COPER == SessionObject.Enumere.CoperRAC  ||
                    _laFact.COPER == SessionObject.Enumere.CoperRembAvance) //Decaissement
                {
                    _laFact.DC = SessionObject.Enumere.Credit ;
                    _laFact.Selectionner = true;
                    _laFact.ISDECAISSEMENT  = true;

                }
                else  //Encaissement
                {
                    //_laFact.COPER = SessionObject.Enumere.CoperRGT;
                    _laFact.DC = SessionObject.Enumere.Debit;
                    _laFact.Selectionner = true;

                }
                if (_montantRest >= _laFact.SOLDEFACTURE )
                {
                    _laFact.MONTANTPAYE = _laFact.SOLDEFACTURE;
                    _montantRest = _montantRest - _laFact.SOLDEFACTURE ;
                    LesFactureSolde.Add(_laFact);

                    continue;
                }
                if (_montantRest < _laFact.SOLDEFACTURE )
                {
                    _laFact.MONTANTPAYE = _montantRest;
                    _laFact.SOLDEFACTURE  = _montantRest.Value;
                    LesFactureSolde.Add(_laFact);
                    _montantRest = 0;
                    break;
                }
            }
            return LesFactureSolde;
        }

        private void Txt_NumeroDemande_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!string.IsNullOrEmpty(this.Txt_NumeroDemande.Text) && 
                this.Txt_NumeroDemande.Text.Length == SessionObject.Enumere.TailleNumeroDemande)
                RetourneDemande();
        }
        private void RetourneDemande()
        {
            try
            {
                CaisseServiceClient service2 = new CaisseServiceClient(Utility.Protocole(), Utility.EndPoint("Caisse"));
                List<CsLclient> _demandeaEncaisser = new List<CsLclient>();
                service2.RetourneDemandeCompleted += (send, es) =>
                {
                    try
                    {
                        if (es != null && es.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel du service.", "Erreur");
                            return;
                        }

                        if (es.Result == null)
                        {
                            Message.ShowError("La récupération des éléments de la demande a retourné une erreur.Réessayer svp!", "Information");
                            return;
                        }
                        if (es.Result != null && es.Result.Count != 0)
                        {
                            _demandeaEncaisser = es.Result;
                            LstFactureDeLaDemande = new List<CsLclient>();
                            LstFactureDeLaDemande = _demandeaEncaisser.Where(t => t.MONTANT != 0).ToList();

                            if (LstFactureDeLaDemande != null && LstFactureDeLaDemande.Count != 0)
                            {
                                if (LstFactureDeLaDemande.FirstOrDefault().TYPEDEMANDE == SessionObject.Enumere.AchatTimbre)
                                {
                                    LstFactureDeLaDemande.ForEach(o => o.CLIENT = "00000000000");
                                    LstFactureDeLaDemande.ForEach(o => o.ORDRE = "00");
                                }

                                if (LstFactureDeLaDemande.FirstOrDefault(t => t.NUMEROLOT == "ENCA1") != null) /* AKO 23/08/2017 tester si paiement facture de prestation */
                                {
                                    LstFactureDeLaDemande.Clear();
                                    LstFactureDeLaDemande = _demandeaEncaisser.Where(t => t.MONTANT != 0 && t.ISPRESTATIONSEULEMENT == true).ToList();
                                }
                                // valoriser les champs relatifs à la caissiere en cours 
                                foreach (CsLclient _EltsFacture in LstFactureDeLaDemande)
                                {
                                    _EltsFacture.SOLDEFACTURE = _EltsFacture.MONTANT + (_EltsFacture.MONTANTTVA == null ? 0 : _EltsFacture.MONTANTTVA);
                                    _EltsFacture.MATRICULE = UserConnecte.matricule;
                                    _EltsFacture.CAISSE = UserConnecte.numcaisse;
                                    _EltsFacture.REFEM = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(_EltsFacture.REFEM);
                                }
                                this.LsvFacture.ItemsSource = LstFactureDeLaDemande;
                                this.Txt_TotalHt.Text = LstFactureDeLaDemande.Sum(t => t.MONTANT).Value.ToString(SessionObject.FormatMontant);
                                this.Txt_totalTaxe.Text = LstFactureDeLaDemande.Sum(t => t.MONTANTTVA).Value.ToString(SessionObject.FormatMontant);
                                this.Txt_TotalTTC.Text = txt_MontantPayes.Text = LstFactureDeLaDemande.Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
                                _LeMonantMinimal = (TotalTTc.Value * _LetauxMinimal) / 100;
                                Txt_NomClient.Text = string.IsNullOrWhiteSpace(LstFactureDeLaDemande.First().NOM) ? string.Empty : LstFactureDeLaDemande.First().NOM;
                                Txt_Client.Text = (string.IsNullOrWhiteSpace(LstFactureDeLaDemande.First().CENTRE) ? string.Empty : LstFactureDeLaDemande.First().CENTRE) + (string.IsNullOrWhiteSpace(LstFactureDeLaDemande.First().CLIENT) ? string.Empty : LstFactureDeLaDemande.First().CLIENT) + (string.IsNullOrWhiteSpace(LstFactureDeLaDemande.First().ORDRE) ? string.Empty : LstFactureDeLaDemande.First().ORDRE);
                            }
                            else
                            {
                                Message.ShowInformation("Aucune facture trouvée.", Langue.MsgPasDemande);
                                //Txt_NumeroDemande.Text = string.Empty;
                                this.LsvFacture.ItemsSource = null;
                                this.Txt_TotalHt.Text = string.Empty;
                                this.Txt_totalTaxe.Text = string.Empty;
                                this.Txt_TotalTTC.Text = string.Empty;
                                Txt_NomClient.Text = string.Empty;
                                Txt_Client.Text = string.Empty;

                                return;
                            }
                        }
                        else
                        {
                            Message.ShowInformation("Demande non trouvée.", Langue.MsgPasDemande);
                            this.LsvFacture.ItemsSource = null;
                            this.Txt_TotalHt.Text = string.Empty;
                            this.Txt_totalTaxe.Text = string.Empty;
                            this.Txt_TotalTTC.Text = string.Empty;
                            Txt_NomClient.Text = string.Empty;
                            Txt_Client.Text = string.Empty;
                        }

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                service2.RetourneDemandeAsync(this.Txt_NumeroDemande.Text, IsExtension);
            }
            catch (Exception ex)
            {

                Message.ShowInformation("Erreur au chargement des données", Langue.MsgPasDemande);

            }
        }
        private void txt_MontantPayes_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private bool  VerifieMontantPaye()
        {

            if (decimal.Parse(this.txt_MontantPayes.Text) < _LeMonantMinimal)
            {
                 //
                Message.ShowInformation(Langue.Msg_MontantPayeIncorecte + "  " + _LeMonantMinimal.ToString(), "Info");
                //Message.ShowInformation("Le montant entré ne doit pas être inférieur au montant minimal. Veuillez entrer un montant valide !", "Info");
                return false;
            }
            return true;
            //else
            //    if (decimal.Parse(this.txt_MontantPayes.Text) > TotalTTc.Value)
            //        //if (decimal.Parse(this.txt_MontantPayes.Text) > decimal.Parse(this.Txt_TotalTTC.Text))
            //         {
            //        Message.ShowInformation("Le montant entré ne doit pas être supérieur au montant TTC. Veuillez entrer un montant valide !", "Info");
            //        return false;
            //     }
            //    else return true;
            
        }

        private void Chk_DevisPrestation_Checked(object sender, RoutedEventArgs e)
        {
            if (LstFactureDeLaDemande != null && LstFactureDeLaDemande.Count != 0)
            {
                if (LstFactureDeLaDemande.FirstOrDefault(t => t.ISPRESTATIONSEULEMENT) != null)
                {
                    List<CsLclient> lstFactureRegle = Shared.ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(LstFactureDeLaDemande);
                    lstFactureRegle = lstFactureRegle.Where(t => t.COPER != SessionObject.Enumere.CoperCAU ).ToList();

                    this.LsvFacture.ItemsSource = null;
                    this.LsvFacture.ItemsSource = lstFactureRegle;

                    this.Txt_TotalHt.Text = lstFactureRegle.Sum(t => t.MONTANT).Value.ToString(SessionObject.FormatMontant);
                    this.Txt_totalTaxe.Text = lstFactureRegle.Sum(t => t.MONTANTTVA).Value.ToString(SessionObject.FormatMontant);
                    this.Txt_TotalTTC.Text = txt_MontantPayes.Text = lstFactureRegle.Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
                    _LeMonantMinimal = (TotalTTc.Value * _LetauxMinimal) / 100;
                    Txt_NomClient.Text = lstFactureRegle.First().NOM;
                    Txt_Client.Text = lstFactureRegle.First().CENTRE + lstFactureRegle.First().CLIENT + lstFactureRegle.First().ORDRE;
                }
            
            }
        }

        private void Chk_DevisPrestation_Unchecked(object sender, RoutedEventArgs e)
        {
            if (LstFactureDeLaDemande != null && LstFactureDeLaDemande.Count != 0)
            {
                if (LstFactureDeLaDemande.FirstOrDefault(t =>t.ISPRESTATIONSEULEMENT) != null)
                {
                    List<CsLclient> lstFactureRegle = Shared.ClasseMEthodeGenerique.RetourneListCopy<CsLclient>(LstFactureDeLaDemande);
                    this.LsvFacture.ItemsSource = null;
                    this.LsvFacture.ItemsSource = lstFactureRegle;
                    this.Txt_TotalHt.Text = LstFactureDeLaDemande.Sum(t => t.MONTANT).Value.ToString(SessionObject.FormatMontant);
                    this.Txt_totalTaxe.Text = LstFactureDeLaDemande.Sum(t => t.MONTANTTVA).Value.ToString(SessionObject.FormatMontant);
                    this.Txt_TotalTTC.Text = txt_MontantPayes.Text = LstFactureDeLaDemande.Sum(t => t.SOLDEFACTURE).Value.ToString(SessionObject.FormatMontant);
                    _LeMonantMinimal = (TotalTTc.Value * _LetauxMinimal) / 100;
                    Txt_NomClient.Text = lstFactureRegle.First().NOM;
                    Txt_Client.Text = lstFactureRegle.First().CENTRE + lstFactureRegle.First().CLIENT + lstFactureRegle.First().ORDRE;
                }

            }
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


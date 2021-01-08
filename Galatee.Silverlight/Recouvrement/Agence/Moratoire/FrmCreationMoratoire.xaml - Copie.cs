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
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceRecouvrement;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Caisse;
using Galatee.Silverlight;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmCreationMoratoire : ChildWindow
    {
        public FrmCreationMoratoire()
        {
            InitializeComponent();
            ChargerListDesSite();
            this.prgBar.Visibility = System.Windows.Visibility.Collapsed;

        }
        string MoisComptable = string.Empty;
        int frequenceMoratoire;
        int selectedoccurMoratoire = 0;
        long montantEcheance = 0;
        int refClient = SessionObject.Enumere.TailleCentre +
                            SessionObject.Enumere.TailleClient +
                            SessionObject.Enumere.TailleOrdre;

        int debutClient = SessionObject.Enumere.TailleCentre;
        int debutOrdre = SessionObject.Enumere.TailleCentre + SessionObject.Enumere.TailleClient;
        int TailleReferenceClient = SessionObject.Enumere.TailleCentre +
                            SessionObject.Enumere.TailleClient +
                            SessionObject.Enumere.TailleOrdre;

        decimal soldeFactureSelect = 0;

        List<string> listeAmountMoratoire = new List<string>();
        List<string> listeDateMoratoire = new List<string>();
        
        CsClient _UnClient;
        List<CsLclient> listFacture = new List<CsLclient>();
        List<CsLclient> lstFactureDeMoratoire = new List<CsLclient>();

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<int?> lesIdCentre = new List<int?>();
        void ChargerListDesSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        lesIdCentre.Add(item.PK_ID);
                    return;                
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        if (SessionObject.LstCentre.Count != 0)
                        {
                            lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                            foreach (ServiceAccueil.CsCentre item in lesCentre)
                                lesIdCentre.Add(item.PK_ID);
                        }
                        else
                        {
                            Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }

                };
                service.ListeDesDonneesDesSiteAsync(false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void txtReference_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtReference.Text.Length == refClient)
                    AfficherImpayes(txtReference.Text);
                else 
                {
                  btnOk.IsEnabled = btnCalcul.IsEnabled = btnPrintQuote.IsEnabled=false;
                  if(string.IsNullOrEmpty(txtReference.Text))
                    CleanIHM();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        /// <summary>
        /// Affiche les impayes d'un client donné
        /// </summary>
        /// <param name="refClient">determine la reference du client </param>
        //private void AfficherImpayes(string refClient)
        //{
        //    try
        //    {
        //        int debutClient = SessionObject.Enumere.TailleCentre;
        //        int debutOrdre = SessionObject.Enumere.TailleCentre + SessionObject.Enumere.TailleClient;

        //        _UnClient = new CsClient();
        //        _UnClient.CENTRE = refClient.Substring(0, SessionObject.Enumere.TailleCentre);
        //        _UnClient.REFCLIENT = refClient.Substring(debutClient, SessionObject.Enumere.TailleClient);
        //        _UnClient.ORDRE = refClient.Substring(debutOrdre, SessionObject.Enumere.TailleOrdre);
        //        RecouvrementServiceClient service = new RecouvrementServiceClient(Utility.Protocole(), Utility.EndPoint("Recouvrement"));
        //        service.RetourneListeFactureNonSoldePourMoratoireAsync(_UnClient);
        //        service.RetourneListeFactureNonSoldePourMoratoireCompleted += (s, args) =>
        //        {

        //            if (args.Cancelled || args.Error != null)
        //            {
        //                Message.ShowError("Erreur survenue lors de l'appel serveur !", Galatee.Silverlight.Resources.Langue.wcf_error);
        //                return;
        //            }
        //            if (args.Result == null || args.Result.Count == 0)
        //            {
        //                Message.ShowError("Ce client n'a aucun impayé dans son compte client.", Galatee.Silverlight.Resources.Langue.errorTitle);
        //                CleanIHM();
        //                return;
        //            }
        //            listFacture = args.Result;
        //            List<CsClient> LstClientDeLaReference = MethodeGenerics.RetourneClientFromFacture(listFacture).Where(t => lesIdCentre.Contains(t.FK_IDCENTRE)).ToList();
        //            if (LstClientDeLaReference == null || LstClientDeLaReference.Count == 0)
        //            {
        //                Message.ShowInformation("Ce client n'est pas de votre périmetre d'action", Galatee.Silverlight.Resources.Langue.errorTitle);
        //                return;
        //            }
        //            else if (LstClientDeLaReference.Count > 1)
        //            {
        //                Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
        //                _LstColonneAffich.Add("REFCLIENT", "CLIENT");
        //                _LstColonneAffich.Add("NOMABON", "NOM");

        //                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstClientDeLaReference);
        //                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Liste de client");
        //                ctrl.Closed += new EventHandler(galatee_OkClickedChoixClient);
        //                ctrl.Show();
        //            }
        //            else
        //            {
        //                if (listFacture.Where(t=>string.IsNullOrEmpty(t.CRET)).Count() == 0)
        //                {
        //                    Message.ShowError("Ce client n'a aucun impayé dans son compte client.", Galatee.Silverlight.Resources.Langue.errorTitle);
        //                    CleanIHM();

        //                    return;
        //                }
        //                if (listFacture.First().IsPAIEMENTANTICIPE)
        //                {
        //                    Message.ShowError("Ce client n'a aucun impayé dans son compte client.", Galatee.Silverlight.Resources.Langue.errorTitle);
        //                    CleanIHM();

        //                    return;
        //                }
        //                lstFactureDeMoratoire = listFacture;
        //                soldeFactureSelect = listFacture.First().SOLDECLIENT.Value;
        //                SessionObject.moisComptable = listFacture.FirstOrDefault().MOISCOMPT;
        //                listFacture.ForEach(t => t.REFERENCE = t.NDOC + "           " + t.REFEM + "             " + t.SOLDEFACTURE.Value.ToString(SessionObject.FormatMontant));
        //                txtClientName.Text = listFacture.FirstOrDefault().NOM;
        //                txtClientAdresse.Text = string.IsNullOrEmpty(listFacture.FirstOrDefault().ADRESSE) ? string.Empty : listFacture.FirstOrDefault().ADRESSE;
        //                txtSoldeDue.Text = listFacture.FirstOrDefault().SOLDECLIENT.Value.ToString(SessionObject.FormatMontant);
        //                txtNbreFactureGlobal.Text = listFacture.Count().ToString(SessionObject.FormatMontant);
        //                rdbMoratoireEntier.IsChecked = true;
        //                btnCalcul.IsEnabled = true;

        //            }
        //        };
        //        service.CloseAsync();
     
        //    }
        //    catch (Exception ex)
        //    {
        //        CleanIHM();
        //        btnOk.IsEnabled = btnCalcul.IsEnabled = btnPrintQuote.IsEnabled = false;
        //        throw ex;
        //    }
        //}

        private void AfficherImpayes(string LeClientSaisie)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                _UnClient = new CsClient();
                List<CsLclient> _ListeFactureClient = new List<CsLclient>();

                int debutClient = SessionObject.Enumere.TailleCentre;
                int debutOrdre = SessionObject.Enumere.TailleCentre + SessionObject.Enumere.TailleClient;


                _UnClient.CENTRE = LeClientSaisie.Substring(0, SessionObject.Enumere.TailleCentre);
                _UnClient.REFCLIENT = LeClientSaisie.Substring(debutClient, SessionObject.Enumere.TailleClient);
                _UnClient.ORDRE = LeClientSaisie.Substring(debutOrdre, SessionObject.Enumere.TailleOrdre);
                RecouvrementServiceClient service = new RecouvrementServiceClient(Utility.Protocole(), Utility.EndPoint("Recouvrement"));
                service.RetourneListeFactureNonSoldePourMoratoireAsync(_UnClient);
                service.RetourneListeFactureNonSoldePourMoratoireCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == null || args.Result.Count == 0)
                    {
                        Message.ShowInformation("Ce client n'existe pas", "Recouvrement");
                        CleanIHM();
                        return;
                    }

                    List<CsLclient> lstFactureDuClient = args.Result;
                    lstFactureDuClient.ForEach(t => t.REFEMNDOC = t.REFEM);
                    List<CsClient> LstClientDeLaReference = MethodeGenerics.RetourneClientFromFacture(lstFactureDuClient);
                    if (LstClientDeLaReference.Count > 1)
                    {
                        List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(LstClientDeLaReference);
                        Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CENTRE", "LIBELLESITE", "Liste des site");
                        ctr.Show();
                        ctr.Closed += new EventHandler(galatee_OkClickedChoixClient);
                        return;
                    }

                    _UnClient.SOLDE = lstFactureDuClient.First().SOLDECLIENT;
                    _UnClient.PK_ID = lstFactureDuClient.First().FK_IDCLIENT;
                    _UnClient.FK_IDCENTRE = lstFactureDuClient.First().FK_IDCENTRE;
                    _UnClient.REFERENCEATM = _UnClient.CENTRE + _UnClient.REFCLIENT + _UnClient.ORDRE;
                    listFacture = lstFactureDuClient;
                    lstFactureDeMoratoire = listFacture;
                    soldeFactureSelect = listFacture.First().SOLDECLIENT.Value;
                    if (soldeFactureSelect == 0)
                    {
                        Message.ShowInformation("Ce client ne doit aucune facture", "Recouvrement");
                        CleanIHM();
                        return;
                    }
                    SessionObject.moisComptable = listFacture.FirstOrDefault().MOISCOMPT;
                    listFacture.ForEach(t => t.REFERENCE = t.NDOC + "           " + t.REFEM + "             " + t.SOLDEFACTURE.Value.ToString(SessionObject.FormatMontant));
                    txtClientName.Text = listFacture.FirstOrDefault().NOM;
                    txtClientAdresse.Text = string.IsNullOrEmpty(listFacture.FirstOrDefault().ADRESSE) ? string.Empty : listFacture.FirstOrDefault().ADRESSE;
                    txtSoldeDue.Text = listFacture.FirstOrDefault().SOLDECLIENT.Value.ToString(SessionObject.FormatMontant);
                    txtNbreFactureGlobal.Text = listFacture.Count().ToString(SessionObject.FormatMontant);
                    rdbMoratoireEntier.IsChecked = true;
                    btnCalcul.IsEnabled = true;
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                };
                service.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void galatee_OkClickedChoixClient(object sender, EventArgs e)
        {
               Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
               if (ctrs.isOkClick)
               {
                   _UnClient = (CsClient)ctrs.MyObject;
                   List<CsLclient> lstFactureSelect = listFacture.Where(t => t.FK_IDCLIENT  == _UnClient.PK_ID).ToList();
                   listFacture = lstFactureSelect;

                   if (listFacture.First().IsPAIEMENTANTICIPE)
                   {
                       Message.ShowError("Ce client n'a aucun impayé dans son compte client.", Galatee.Silverlight.Resources.Langue.errorTitle);
                       CleanIHM();
                       return;
                   }
                   listFacture.ForEach(t => t.REFERENCE = t.NDOC + "           " + t.REFEM + "             " + t.SOLDEFACTURE.Value.ToString(SessionObject.FormatMontant));
                   soldeFactureSelect = lstFactureSelect.First().SOLDECLIENT.Value;
                   SessionObject.moisComptable = lstFactureSelect.FirstOrDefault().MOISCOMPT;
                   txtClientName.Text = listFacture.FirstOrDefault().NOM;
                   btnCalcul.IsEnabled = true;
                   lstFactureDeMoratoire = lstFactureSelect;
               }
        }

        private void Cbo_ListeClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_ListeClient.SelectedItem != null)
                {
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        /// <summary>
        /// Permet l'initialisation des controles de IHM
        /// 
        /// </summary>
        void Initialisation()
        { 
           /// Peupler la combobox des nombre d'installation
           
               try 
	            {	        
		            List<string> listeValue =new List<string>();
                        for (int i = 2; i < 13; i++)
                            listeValue.Add(i.ToString());
                        cboNoInstall.ItemsSource = listeValue;
                        cboNoInstall.SelectedItem = listeValue[0];

                        rdbMonth.IsChecked = true;
                        rdbMoratoireEntier .IsChecked = true;
                        btnCalcul.IsEnabled = false;
	            }
	            catch (Exception ex)
	            {
		            throw ex;
	            }
        }
        /// <summary>
        /// Inititalise tous les controles de l'IHM pour une nouvelle
        /// saisie de données
        /// </summary>
        /// 

        void CleanIHM()
        {
            try
            {
                for (int i = 1; i < 13; i++)
                {

                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    TextBox date = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);
                    amounts.Text = string.Empty;
                    date.Text = string.Empty;

                }
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                txtAmount.Text = string.Empty;
                txtAmount.Text = string.Empty;
                txtClientName.Text = string.Empty;
                txtReference.Text = string.Empty;
                txtClientAdresse.Text = string.Empty;
                //txtDue.Text = string.Empty;

                if (listFacture != null && listFacture.Count > 0)
                    listFacture.Clear();
                if (listeDateMoratoire != null && listeDateMoratoire.Count > 0)
                    listeDateMoratoire.Clear();
                if (listeAmountMoratoire != null && listeAmountMoratoire.Count > 0)
                    listeAmountMoratoire.Clear();

             

                rdbMonth.IsChecked = true;
                rdbMoratoireFacture.IsChecked = true;
                frequenceMoratoire = 1;

                Cbo_ListeClient.ItemsSource = null;

                this.txtAmount.Text = string.Empty;
                txtNbreFactureSelect.Text = string.Empty;

                txtNbreFactureGlobal.Text = string.Empty;
                txtSoldeDue.Text = string.Empty;

                Cbo_ListeClient.Visibility = System.Windows.Visibility.Collapsed;
                btn_factureClient.Visibility = System.Windows.Visibility.Collapsed;
                label3.Visibility = System.Windows.Visibility.Collapsed;
                label4.Visibility = System.Windows.Visibility.Collapsed;
                label5.Visibility = System.Windows.Visibility.Collapsed;
                txtAmount.Visibility = System.Windows.Visibility.Collapsed;
                label8_Copy.Visibility = System.Windows.Visibility.Collapsed;
                label8.Visibility = System.Windows.Visibility.Collapsed;
                txtNbreFactureSelect.Visibility = System.Windows.Visibility.Collapsed;

                btnCalcul.IsEnabled = false;
               

                if (listFacture != null && listFacture.Count > 0)
                    listFacture.Clear();

                if (lstFactureDeMoratoire != null && lstFactureDeMoratoire.Count > 0)
                    lstFactureDeMoratoire.Clear();
       
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void ClearAmountTextBox()
        {
            try
            {
                for (int i = 1; i < 13; i++)
                {

                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    TextBox date = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);
                    amounts.Text = string.Empty;
                    date.Text = string.Empty;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnCalcul_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                IsProposition = false;

                CalculerMoratoire(frequence, soldeFactureSelect, 0, false);
                this.btnOk.IsEnabled = true;

                //if (rdbMoratoireFacture.IsChecked == true)
                //{
                //    InitialiseMontantMoratoires(frequence, Convert.ToDecimal(soldeFactureSelect));
                //    InitialiserDateMoratoiresParFacture(frequenceMoratoire, frequence);
                //}
                //else
                //{
                //    InitialiseMontantMoratoires(frequence,soldeFactureSelect);
                //    InitialiserDateMoratoiresParFacture(frequenceMoratoire, frequence);
                //}
                //if (listeAmountMoratoire.Count > 0 && listeAmountMoratoire != null)
                //{
                //    int i = 0;
                //    foreach (string amount in listeAmountMoratoire)
                //    {
                //        i++;
                //        TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                //        amounts.Text = amount;
                //        amounts.IsReadOnly = true;
                //    }
                //}

                //if (listeDateMoratoire.Count > 0 && listeDateMoratoire != null)
                //{
                //    int i = 0;
                //    foreach (string date in listeDateMoratoire)
                //    {
                //        i++;
                //        TextBox dateBox = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);
                //        dateBox.Text = date;
                //        btnPrintQuote.IsEnabled = true;
                //        dateBox.IsReadOnly = true;
                //    }
                //}
                //if (this.rdbMoratoireEntier.IsChecked != true)
                //    return;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex,Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        /// <summary>
        /// Permet de peupler la liste des montants des occurrences du moratoires d'une facture donnée
        /// </summary>
        /// <param name="frequence"></param>
        void InitialiseMontantMoratoires(int frequence,decimal montantSolde)
        {
            try
            {
                listeAmountMoratoire.Clear();

                double montantFacture = Convert.ToDouble(montantSolde);
                double reste = Math.Round(montantFacture % frequence, 2);
                double partieEntiere = montantFacture - reste;
                double diviseur = partieEntiere / frequence;
                for (int i = 0; i < frequence; i++)
                {
                    if (i == int.Parse(cboNoInstall.SelectedItem.ToString()) - 1)
                    {
                        double montant = reste + diviseur;
                        montantEcheance = Convert.ToInt64(diviseur);
                        listeAmountMoratoire.Add(montant.ToString(SessionObject.FormatMontant ));
                    }
                    else
                        listeAmountMoratoire.Add(diviseur.ToString(SessionObject.FormatMontant));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void InitialiseMontantMoratoiresTouteFactures(int frequence,decimal soldeFacture)
        {
            try
            {
                listeAmountMoratoire.Clear();

                double montantdesFacture = Convert.ToDouble(soldeFacture);
                double reste = Math.Round(montantdesFacture % frequence, 2);
                double partieEntiere = montantdesFacture - reste;
                double diviseur = partieEntiere / frequence;
                for (int i = 0; i < frequence; i++)
                {
                    if (i == int.Parse(cboNoInstall.SelectedItem.ToString()) - 1)
                    {
                        double montant = reste + diviseur;
                        listeAmountMoratoire.Add(montant.ToString(SessionObject.FormatMontant));
                    }
                    else
                        listeAmountMoratoire.Add(diviseur.ToString(SessionObject.FormatMontant));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Permet de peupler la liste des dates des occurrences du moratoires
        /// </summary>
        /// <param name="frequenceMoratoire"></param>
        /// <param name="NombreOccurMor"></param>
        void InitialiserDateMoratoiresParFacture(int frequenceMoratoire, int NombreOccurMor,DateTime DateDebut )
        {
            try
            {
                DateTime now = new DateTime();
                if (DateDebut== System.DateTime.Today )
                 now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                else
                    now = new DateTime(DateDebut.Year, DateDebut.Month, DateDebut.Day);

                listeDateMoratoire.Clear();

                for (int i = 0; i < NombreOccurMor; i++)
                {
                    if (i == 0)
                        listeDateMoratoire.Add(now.ToString("d"));
                    else
                    {
                        string datte = string.Empty;

                        if (frequenceMoratoire == 1)
                            now = now.AddMonths(1);
                        else
                            if (frequenceMoratoire == 2)
                                now = now.AddDays(14);
                            else
                                now = now.AddDays(7);

                        datte = now.ToString("d");
                        listeDateMoratoire.Add(datte);

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        private void rdbMoratoireFacture_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Initialisation();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void rdbMoratoireEntier_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (string.IsNullOrEmpty(txtReference.Text))
                //    return;

                if (rdbMoratoireEntier.IsChecked == true)
                {
                    Cbo_ListeClient.Visibility = System.Windows.Visibility.Collapsed;
                    btn_factureClient.Visibility = System.Windows.Visibility.Collapsed;
                    label3.Visibility = System.Windows.Visibility.Collapsed;
                    label4.Visibility = System.Windows.Visibility.Collapsed;
                    label5.Visibility = System.Windows.Visibility.Collapsed;
                    txtAmount.Visibility = System.Windows.Visibility.Collapsed;
                    label8_Copy.Visibility = System.Windows.Visibility.Collapsed;
                    label8.Visibility = System.Windows.Visibility.Collapsed;
                    txtNbreFactureSelect.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void rdbMoratoireFacture_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtReference.Text))
                    return;

                if (Cbo_ListeClient.SelectedItem != null)
                    if (rdbMoratoireFacture.IsChecked == true)
                    {
                        Cbo_ListeClient.IsEnabled = true;
                        Cbo_ListeClient_SelectionChanged(null, null);
                    }
                Cbo_ListeClient.Visibility = System.Windows.Visibility.Visible ;
                btn_factureClient.Visibility = System.Windows.Visibility.Visible;
                label3.Visibility = System.Windows.Visibility.Visible;
                label4.Visibility = System.Windows.Visibility.Visible;
                label5.Visibility = System.Windows.Visibility.Visible;

                txtAmount.Visibility = System.Windows.Visibility.Visible;
                label8_Copy.Visibility = System.Windows.Visibility.Visible;
                label8.Visibility = System.Windows.Visibility.Visible;
                txtNbreFactureSelect.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void rdbMonth_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                frequenceMoratoire = (int)SessionObject.frequenceMoratoire.Month;
                if (listeAmountMoratoire != null && listeAmountMoratoire.Count > 0)
                {
                    ClearAmountTextBox();
                    btnCalcul_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erruer");
            }
        }

        private void rdbFornight_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                frequenceMoratoire = (int)SessionObject.frequenceMoratoire.ForNight;
                if (listeAmountMoratoire != null && listeAmountMoratoire.Count > 0)
                {
                    ClearAmountTextBox();
                    btnCalcul_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void rdbWeek_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                frequenceMoratoire = (int)SessionObject.frequenceMoratoire.Week;
                if (listeAmountMoratoire != null && listeAmountMoratoire.Count > 0)
                {
                    ClearAmountTextBox();
                    btnCalcul_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran( this);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtReference.Text))
                    return;
                if (IsProposition)
                {
                    decimal montantTotal = 0;
                    if (listeAmountMoratoire.Count > 0 && listeAmountMoratoire != null)
                    {
                        int i = 0;
                        foreach (string amount in listeAmountMoratoire)
                        {
                            i++;
                            TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                            if (!string.IsNullOrEmpty(amounts.Text ))
                                montantTotal = montantTotal + Convert.ToDecimal(amounts.Text);
                        }
                    }
                    if (montantTotal > Convert.ToDecimal(soldeFactureSelect))
                    {
                        Message.ShowInformation("Montant des moratoires supperieur au montant de facture selectionnée", "Moratoire");
                        return;
                    }
                }
               
                btnOk.IsEnabled = false;
                List<CsLclient> listeMORA = new List<CsLclient>();
                listeMORA.AddRange(CreateListeFromDetailMoratoire(lstFactureDeMoratoire));
                this.prgBar.Visibility = System.Windows.Visibility.Visible;

                RecouvrementServiceClient cl = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                cl.InsertDetailMoratoireCompleted += (s, results) =>
                 {
                     try
                     {
                         this.prgBar.Visibility = System.Windows.Visibility.Collapsed ;

                         if (results.Cancelled || results.Result == false)
                         {
                             Message.Show(results.Error.Message, Galatee.Silverlight.Resources.Langue.wcf_error);
                             return;
                         }
                         btnOk.IsEnabled = true;
                         string reffact = string.Empty;
                         int i = 0;
                         foreach (var item in lstFactureDeMoratoire)
                         {
                             if (i == 6) break;
                             reffact = reffact + item.REFEM + "-" + item.NDOC + "    ";
                             i++;
                         }
                         //impression du recu du moratoire
                         Dictionary<string, string> param = new Dictionary<string, string>();
                         param.Add("pCentre", lstFactureDeMoratoire.First().CENTRE);
                         param.Add("pClient", lstFactureDeMoratoire.First().CLIENT);
                         param.Add("pOrdre", lstFactureDeMoratoire.First().ORDRE);
                         param.Add("pNombreEcheance", (listeMORA.Count - 1).ToString());
                         param.Add("pMontantGlobal", soldeFactureSelect.ToString(SessionObject.FormatMontant));
                         param.Add("pName", txtClientName.Text);
                         param.Add("pNombreFacture", lstFactureDeMoratoire.Count().ToString());
                         param.Add("pReferencefacture", reffact);
                         param.Add("pOperationMor", "MORATOIRE");

                         foreach (CsLclient item in listeMORA.Where(t => t.EXIGIBILITE != null).ToList())
                         {
                             if (item.EXIGIBILITE != null)
                                 item.DC = item.EXIGIBILITE.Value.ToShortDateString();
                             else
                                 item.DC = string.Empty;
                         }
                         param.Add("pExigibilite", listeMORA.First(t => t.EXIGIBILITE != null).DC.ToString());

                         //Utility.ActionPreview <ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(listeMORA, param, "moratoire", "Recouvrement");

                         Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(listeMORA.Where(t=>t.EXIGIBILITE != null ).ToList(), param, SessionObject.CheminImpression, "moratoire", "Recouvrement", true);


                         //string CheminImpression = "55" + SessionObject.LePosteCourant.NOMPOSTE + "5"  + "Impression";
                         //Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(listeMORA, param, CheminImpression, "moratoire", "Recouvrement", false);
                         CleanIHM();
                     }
                     catch (Exception ex)
                     {
                         this.prgBar.Visibility = System.Windows.Visibility.Collapsed ;
                         Message.ShowError(ex, "Erreur");
                     }
                 };
                cl.InsertDetailMoratoireAsync(listeMORA, lstFactureDeMoratoire);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
            
        }


        List<CsLclient> CreateListeFromDetailMoratoire(List<CsLclient> lesfactureMoratoire )
        {
            try
            {
                string refem = MoisComptable;
                if (lesfactureMoratoire.Count == 1)
                    refem = lesfactureMoratoire.First().REFEM;

                    List<CsLclient> moratoires = new List<CsLclient>();
                    selectedoccurMoratoire = int.Parse(cboNoInstall.SelectedItem.ToString());
                    for (int i = 0; i < selectedoccurMoratoire + 1; i++)
                    {
                        int inc = i + 1;
                        TextBox amounts = null;
                        TextBox dates = null;
                        if (i != 0)
                        {
                            amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                            dates = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);
                        }

                        if (i == 0)
                        {
                            //kan on est en moratoire par facture
                            CsLclient echeanceMoratoire = new CsLclient();
                            echeanceMoratoire.CENTRE = lesfactureMoratoire.First().CENTRE ;
                            echeanceMoratoire.CLIENT = lesfactureMoratoire.First().CLIENT ; ;
                            echeanceMoratoire.ORDRE = lesfactureMoratoire.First().ORDRE ; ;
                            echeanceMoratoire.MOISCOMPT =  MoisComptable;
                            echeanceMoratoire.IsGlobal  = (rdbMoratoireEntier.IsChecked.Value == true ? true  : false );
                            echeanceMoratoire.COPER = SessionObject.Enumere.CoperMorSolde;
                            echeanceMoratoire.REFEM = lesfactureMoratoire.First().REFEM ;
                            echeanceMoratoire.NDOC = lesfactureMoratoire.First().NDOC ;
                            echeanceMoratoire.CRET = string.Empty;
                            echeanceMoratoire.NATURE = SessionObject.Enumere.NatureMorSolde;
                            echeanceMoratoire.DATEVALEUR = DateTime.Parse(txtDate1.Text);
                            echeanceMoratoire.DC = SessionObject.Enumere.Debit ;
                            echeanceMoratoire.MONTANT = lesfactureMoratoire.Sum(t=>t.SOLDEFACTURE);
                            echeanceMoratoire.EXIGIBILITE = null;
                            echeanceMoratoire.TOP1 = SessionObject.Enumere.TopMoratoire;
                            echeanceMoratoire.MATRICULE = UserConnecte.matricule;
                            echeanceMoratoire.DATECREATION = DateTime.Now.Date;
                            echeanceMoratoire.USERCREATION = UserConnecte.matricule;
                            echeanceMoratoire.FK_IDCLIENT = lesfactureMoratoire.First().FK_IDCLIENT;
                            echeanceMoratoire.FK_IDLCLIENT = lesfactureMoratoire.First().PK_ID ;
                            echeanceMoratoire.FK_IDCENTRE = lesfactureMoratoire.First().FK_IDCENTRE ;
                            echeanceMoratoire.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                            echeanceMoratoire.DATEENCAISSEMENT = DateTime.Now.Date;
                            echeanceMoratoire.DATEFLAG = null;
                            echeanceMoratoire.DENR = DateTime.Now.Date;
                            echeanceMoratoire.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperMOR).PK_ID;
                            echeanceMoratoire.FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TopMoratoire).PK_ID;
                            moratoires.Add(echeanceMoratoire);

                        }
                        else
                        {
                            CsLclient echeanceMoratoire = new CsLclient()
                            {
                                COPER =SessionObject.Enumere.CoperMOR ,
                                REFEM = MoisComptable,
                                CENTRE = lesfactureMoratoire.First().CENTRE  ,
                                CLIENT = lesfactureMoratoire.First().CLIENT ,
                                ORDRE = lesfactureMoratoire.First().ORDRE ,
                                MOISCOMPT = (rdbMoratoireEntier.IsChecked.Value == true ? string.Empty : MoisComptable),
                                CRET = SessionObject.Cret()[i - 1],
                                NATURE = SessionObject.Enumere.NatureMor,
                                DATEVALEUR = null,
                                DC = SessionObject.Enumere.Debit ,
                                MONTANT = Convert.ToDecimal(amounts.Text),
                                EXIGIBILITE = DateTime.Parse(dates.Text),
                                TOP1 = SessionObject.Enumere.TopMoratoire,
                                MATRICULE = UserConnecte.matricule,
                                DATECREATION = DateTime.Now.Date,
                                USERCREATION = UserConnecte.matricule,
                                FK_IDCLIENT = lesfactureMoratoire.First().FK_IDCLIENT,
                                FK_IDCENTRE = lesfactureMoratoire.First().FK_IDCENTRE,
                                FK_IDLCLIENT = lesfactureMoratoire.First().PK_ID  ,
                                FK_IDADMUTILISATEUR = UserConnecte.PK_ID,
                                FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperMOR).PK_ID,
                                FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TopMoratoire).PK_ID,
                            };
                            moratoires.Add(echeanceMoratoire);
                        }
                    }
                
                return moratoires;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
     
        private void cboNoInstall_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (listeAmountMoratoire != null && listeAmountMoratoire.Count > 0)
                {
                    ClearAmountTextBox();
                    btnCalcul_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void txtAmount1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtAmount1.Text))
                {
                    
                    btnOk.IsEnabled = true;
                }
                else
                    btnOk.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void txtReference_TextChanged_1(object sender, TextChangedEventArgs e)
        {
           
        }

        private void btncancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult=true;
        }

  

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FrmRechercheClient recherche = new FrmRechercheClient();
                recherche.Closed += (s, es) =>
                {
                    try
                    {
                        txtReference.Text = string.IsNullOrEmpty(recherche.CustomerRef) ? string.Empty : recherche.CustomerRef;
                        txtClientName.Text = string.IsNullOrEmpty(recherche.CustomerName) ? string.Empty : recherche.CustomerName;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }

                };
                recherche.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        bool IsProposition = false;

        private void CalculerMoratoire(int frequence, decimal montant, int position, bool isProposition)
        {
            try
            {
                if (rdbMoratoireFacture.IsChecked == true)
                {
                    InitialiseMontantMoratoires(frequence, montant);
                    InitialiserDateMoratoiresParFacture(frequenceMoratoire, frequence,System.DateTime.Today );
                }
                else
                {
                    InitialiseMontantMoratoires(frequence, montant);
                    InitialiserDateMoratoiresParFacture(frequenceMoratoire, frequence, System.DateTime.Today);
                }
                if (listeAmountMoratoire.Count > 0 && listeAmountMoratoire != null)
                {
                    int i = position;
                    foreach (string amount in listeAmountMoratoire)
                    {
                        i++;
                        TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                        amounts.Text = amount ;
                        amounts.IsReadOnly = !isProposition;
                    }
                }

                if (listeDateMoratoire.Count > 0 && listeDateMoratoire != null)
                {
                    int i = 0;
                    foreach (string date in listeDateMoratoire)
                    {
                        i++;
                        TextBox dateBox = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);
                        dateBox.Text = date;
                        btnPrintQuote.IsEnabled = !isProposition;
                        dateBox.IsReadOnly = !isProposition;
                    }
                }
                if (this.rdbMoratoireEntier.IsChecked != true)
                    return;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        private void CalculerMoratoire(int frequence, decimal montant, int position, bool isProposition,DateTime DateDebut)
        {
            try
            {
                if (rdbMoratoireFacture.IsChecked == true)
                {
                    InitialiseMontantMoratoires(frequence, montant);
                    InitialiserDateMoratoiresParFacture(frequenceMoratoire, frequence, DateDebut);
                }
                else
                {
                    InitialiseMontantMoratoires(frequence, montant);
                    InitialiserDateMoratoiresParFacture(frequenceMoratoire, frequence, DateDebut);
                }
                if (listeAmountMoratoire.Count > 0 && listeAmountMoratoire != null)
                {
                    int i = position;
                    foreach (string amount in listeAmountMoratoire)
                    {
                        i++;
                        TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                        amounts.Text = amount;
                        amounts.IsReadOnly = !isProposition;
                    }
                }

                if (listeDateMoratoire.Count > 0 && listeDateMoratoire != null)
                {
                    int i = 0;
                    foreach (string date in listeDateMoratoire)
                    {
                        i++;
                        TextBox dateBox = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);
                        dateBox.Text = date;
                        btnPrintQuote.IsEnabled = !isProposition;
                        dateBox.IsReadOnly = !isProposition;
                    }
                }
                if (this.rdbMoratoireEntier.IsChecked != true)
                    return;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btnPrintQuote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                IsProposition = true;
                CalculerMoratoire(frequence, soldeFactureSelect, 0, IsProposition);
                
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void txtAmount2_TextChanged(object sender, TextChangedEventArgs e)
        {
       
        }

      

        private void btn_factureClient_Click(object sender, RoutedEventArgs e)
        {

            FrmdetailleFacture ctrl = new FrmdetailleFacture(listFacture );
            ctrl.Closed += new EventHandler(galatee_OkClickedFavtureClient);
            ctrl.Show();
        }

        private void galatee_OkClickedFavtureClient(object sender, EventArgs e)
        {
            this.Cbo_ListeClient.ItemsSource = null;
            this.Cbo_ListeClient.ItemsSource  = listFacture.Where(t => t.Selectionner == true);
            this.Cbo_ListeClient.DisplayMemberPath = "REFERENCE";
            if (listFacture.Where(t => t.Selectionner == true).Count() != 0)
                this.Cbo_ListeClient.SelectedItem = listFacture.First();
            soldeFactureSelect = listFacture.Where(t => t.Selectionner == true).Sum(y => y.SOLDEFACTURE).Value;
            this.txtAmount.Text = soldeFactureSelect.ToString(SessionObject.FormatMontant);
            this.txtNbreFactureSelect.Text = listFacture.Where(t => t.Selectionner == true).Count().ToString();
            lstFactureDeMoratoire = listFacture.Where(t => t.Selectionner == true).ToList();

        }
        private void txtAmount1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount1.Text))
            {
                txtAmount1.Text = Convert.ToDecimal(txtAmount1.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                IsProposition = true;
                for (int i = 1; i <= 1; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(frequence - 1, (soldeFactureSelect - MontantRestant), 1, true);
            }
        }
        private void txtAmount2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount2.Text))
            {
                txtAmount2.Text = Convert.ToDecimal(txtAmount2.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                IsProposition = true;
                for (int i = 1; i <= 2; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(frequence - 2, (soldeFactureSelect - MontantRestant), 2, true);
            }
        }
        private void txtAmount3_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount3.Text))
            {
                txtAmount3.Text = Convert.ToDecimal(txtAmount3.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                IsProposition = true;
                for (int i = 1; i <= 3; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(frequence - 3, (soldeFactureSelect - MontantRestant),3, true);
            }
        }
        private void txtAmount4_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount4.Text))
            {
                txtAmount4.Text = Convert.ToDecimal(txtAmount4.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                IsProposition = true;
                for (int i = 1; i <= 4; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(frequence - 4, (soldeFactureSelect - MontantRestant), 4, true);
            }
        }
        private void txtAmount5_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount5.Text))
            {
                txtAmount5.Text = Convert.ToDecimal(txtAmount5.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                IsProposition = true;
                for (int i = 1; i <= 5; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(frequence - 5, (soldeFactureSelect - MontantRestant), 5, true);
            }
        }
        private void txtAmount6_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount6.Text))
            {
                txtAmount6.Text = Convert.ToDecimal(txtAmount6.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                IsProposition = true;
                for (int i = 1; i <= 6; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(frequence -6, (soldeFactureSelect - MontantRestant), 6, true);
            }
        }
        private void txtAmount7_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount7.Text))
            {
                txtAmount7.Text = Convert.ToDecimal(txtAmount7.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                IsProposition = true;
                for (int i = 1; i <= 7; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(frequence - 7, (soldeFactureSelect - MontantRestant), 7, true);
            }
        }
        private void txtAmount8_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount8.Text))
            {
                txtAmount8.Text = Convert.ToDecimal(txtAmount8.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                IsProposition = true;
                for (int i = 1; i <= 8; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(frequence - 8, (soldeFactureSelect - MontantRestant), 8, true);
            }
        }
        private void txtAmount9_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount9.Text))
            {
                txtAmount9.Text = Convert.ToDecimal(txtAmount9.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                IsProposition = true;
                for (int i = 1; i <= 9; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(frequence -9, (soldeFactureSelect - MontantRestant),9, true);
            }
        }
        private void txtAmount10_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount10.Text))
            {
                txtAmount10.Text = Convert.ToDecimal(txtAmount10.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                IsProposition = true;
                for (int i = 1; i <=10; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(frequence - 10, (soldeFactureSelect - MontantRestant), 10, true);
            }
        }
        private void txtAmount11_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount11.Text))
            {
                txtAmount10.Text = Convert.ToDecimal(txtAmount11.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                IsProposition = true;
                for (int i = 1; i <= 11; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(frequence - 11, (soldeFactureSelect - MontantRestant), 11, true);
            }
        }
        private void txtAmount12_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmount12.Text))
            {
                txtAmount10.Text = Convert.ToDecimal(txtAmount12.Text).ToString(SessionObject.FormatMontant);
                decimal MontantRestant = 0;
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                IsProposition = true;
                for (int i = 1; i <= 12; i++)
                {
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    if (!string.IsNullOrEmpty(amounts.Text))
                        MontantRestant = MontantRestant + Convert.ToDecimal(amounts.Text);
                }
                CalculerMoratoire(frequence - 12, (soldeFactureSelect - MontantRestant), 12, true);
            }
        }

        private void txtDate1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDate1.Text))
            {
                int frequence = int.Parse(cboNoInstall.SelectedItem.ToString());
                InitialiserDateMoratoiresParFacture(frequenceMoratoire, frequence, Convert.ToDateTime(txtDate1.Text));
                int i = 0;
                foreach (string date in listeDateMoratoire)
                {
                    i++;
                    TextBox dateBox = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);
                    dateBox.Text = date;
                }
                

            }
        }
    }
}



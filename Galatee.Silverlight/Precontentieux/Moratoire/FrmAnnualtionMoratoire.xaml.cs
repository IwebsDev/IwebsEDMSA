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
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Precontentieux
{
    public partial class FrmAnnualtionMoratoire : ChildWindow
    {
        public FrmAnnualtionMoratoire()
        {
            InitializeComponent();
            prgBar.Visibility = System.Windows.Visibility.Collapsed;
        }

        string centre = string.Empty;
        string client = string.Empty;
        string ordre = string.Empty;
        string amountselected = string.Empty;
        string refermSelected = string.Empty;
        string ndocSelected = string.Empty;
        string numerofacture = string.Empty;
        string idmoratoireselected = string.Empty;
        string nombreEchaeance = string.Empty;
        int TaillerefClient = SessionObject.Enumere.TailleCentre +
                          SessionObject.Enumere.TailleClient +
                          SessionObject.Enumere.TailleOrdre;

        List<string> lientFactureDUcLIENT = new List<string>();
        List<string> lientFactureDUcLIENTSplit = new List<string>();
        List<string> listeAmountMoratoire = new List<string>();
        List<string> listeDateMoratoire = new List<string>();
        int pkId;
        int fk_Idcentre;

        List<ServiceRecouvrement.CsDetailMoratoire> listeDetailsMoratoires = new List<ServiceRecouvrement.CsDetailMoratoire>();
        List<ServiceRecouvrement.CsDetailMoratoire> detailSelectionne = new List<ServiceRecouvrement.CsDetailMoratoire>();

  
 

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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
                    //amounts.Text = string.Empty;
                    //date.Text = string.Empty;
                }
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                txtClientName.Text = string.Empty;
                txtReference.Text = string.Empty;
                txtDue.Text = string.Empty;
                txtNoInstalmnt.Text = string.Empty;
                txtClientAdresse.Text = string.Empty;
        
                if (listeDateMoratoire != null)
                    listeDateMoratoire.Clear();
                if (listeAmountMoratoire != null)
                    listeAmountMoratoire.Clear();

                amountselected = string.Empty;
                Cbo_ListeClient.ItemsSource = null;

                if (lientFactureDUcLIENT != null)
                    lientFactureDUcLIENT.Clear();
                if (lientFactureDUcLIENTSplit != null)
                    lientFactureDUcLIENTSplit.Clear();
            }
            catch (Exception ex)
            {
              throw ex;
            }

        }
        int refClient = SessionObject.Enumere.TailleCentre +
                        SessionObject.Enumere.TailleClient +
                        SessionObject.Enumere.TailleOrdre;
        private void txtReference_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtReference.Text.Length == refClient)
                {
                    AfficherImpayes(txtReference.Text);
                    
                }
                else
                {
                    if (string.IsNullOrEmpty(txtReference.Text))
                        CleanIHM();
                }
                    
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        
        
        
        
        
        
        
        
        
        }

        private void AfficherImpayes(string refClient)
        {
            try
            {
                int debutClient = SessionObject.Enumere.TailleCentre;
                int debutOrdre = SessionObject.Enumere.TailleCentre + SessionObject.Enumere.TailleClient;

                centre = refClient.Substring(0, SessionObject.Enumere.TailleCentre);
                client = refClient.Substring(debutClient, SessionObject.Enumere.TailleClient);
                ordre = refClient.Substring(debutOrdre, SessionObject.Enumere.TailleOrdre);

                RecouvrementServiceClient srv = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                srv.TestClientExistCompleted += new EventHandler<TestClientExistCompletedEventArgs>(TestClientExist);// Call back sur la methode TestClientTxi
                srv.TestClientExistAsync(centre, client, ordre);
            }
            catch (Exception ex)
            {
                CleanIHM();
                throw ex;
            }
        }

        /// <summary>
        /// Fonction ramenne les resultats d l'appel asynchrone du serci'e wcv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void TestClientExist(object sender, TestClientExistCompletedEventArgs e)
        {
            try
            {
                prgBar.Visibility = System.Windows.Visibility.Visible;
                if (e.Cancelled || e.Error != null)
                    return;
                if (e.Result == null || e.Result.Count == 0)
                {
                    prgBar.Visibility = System.Windows.Visibility.Collapsed ;

                    Message.ShowError("La référence client ne correspond à aucun abonné ", Galatee.Silverlight.Resources.Langue.errorTitle);
                    return;

                }
                else
                {

                    txtClientName.Text = e.Result[0].NOMABON;
                    txtClientAdresse.Text = e.Result[0].ADRMAND1 ;
                    pkId = e.Result[0].PK_ID;
                    fk_Idcentre = e.Result[0].FK_IDCENTRE.Value;


                    RecouvrementServiceClient srv = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    srv.RetourneMoratoireDuClientCompleted += (es, res) =>
                    {
                        try
                        {
                            if (res.Cancelled || res.Error != null)
                            {
                                Message.ShowError("Erreur survenue lors de l'appel serveur.", "Erreur");
                                CleanIHM();
                                return;
                            }

                            if (res.Result == null || res.Result.Count == 0)
                            {
                                Message.ShowError("Ce client n'a aucun impayé dans son compte client.", Galatee.Silverlight.Resources.Langue.errorTitle);
                                CleanIHM();
                                return;
                            }

                            listeDetailsMoratoires = res.Result;
                            if (listeDetailsMoratoires != null)
                                FillCombobox(listeDetailsMoratoires);
                            btnOk.IsEnabled = true ;
                            prgBar.Visibility = System.Windows.Visibility.Collapsed;

                        }
                        catch (Exception ex)
                        {
                            CleanIHM();
                            Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                        }
                    };
                    srv.RetourneMoratoireDuClientAsync(centre, client, ordre);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        void InitializationControle(List<ServiceRecouvrement.CsDetailMoratoire> pMoratoires)
        {
            try
            {
                FillCombobox(pMoratoires);
            }
            catch (Exception ex)
            {
              throw ex;
            }
        }

        void FillDetailData(List<ServiceRecouvrement.CsDetailMoratoire> l)
        {

            try
            {

                for (int j = 0; j <= 10; j++)
                {
                    j++;
                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + j);
                    TextBox dateBox = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + j);
                    amounts.Text = string.Empty;
                    dateBox.Text = string.Empty;
                }

                int i = 0;
                List<ServiceRecouvrement.CsDetailMoratoire> listeIsCret = new List<ServiceRecouvrement.CsDetailMoratoire>();
                foreach (ServiceRecouvrement.CsDetailMoratoire moratoires in l)
                {

                    // fill les données des échéances 
                    i++;

                    TextBox amounts = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtAmount" + i);
                    TextBox dateBox = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtDate" + i);

                    if (!string.IsNullOrEmpty(moratoires.CRET))
                    {
                        listeIsCret.Add(moratoires);
                        amounts.Text = moratoires.MONTANT == null ? string.Empty : moratoires.MONTANT.Value.ToString(SessionObject.FormatMontant );
                        dateBox.Text = (moratoires.EXIGIBILITE == null ? string.Empty : moratoires.EXIGIBILITE.Value.ToString("d"));
                    }
                    else
                    {
                       // listeIsCret.Add(moratoires);
                        amounts.Text = moratoires.MONTANT == null ? string.Empty : moratoires.MONTANT.Value.ToString(SessionObject.FormatMontant);
                        dateBox.Text = (moratoires.EXIGIBILITE == null ? string.Empty : moratoires.EXIGIBILITE.Value.ToString("d"));
                    }
                }

                Decimal fraisFacture = listeIsCret.Sum(p => p.FRAISDERETARD == null ? 0 : p.FRAISDERETARD.Value);
                Decimal montantFacture = listeIsCret.Sum(p => p.MONTANT == null ? 0 : p.MONTANT.Value);
                amountselected = txtDue.Text = (montantFacture - fraisFacture).ToString(SessionObject.FormatMontant );
                nombreEchaeance = listeIsCret.Count.ToString();
                txtNoInstalmnt.Text = listeIsCret.Count.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
          

        }
        void FillCombobox(List<ServiceRecouvrement.CsDetailMoratoire> l)
        {
            try
            {
                List<ServiceRecouvrement.CsDetailMoratoire> detailParNdocReferm = new List<ServiceRecouvrement.CsDetailMoratoire>();
                List<string> referNdoc = new List<string>();
                referNdoc.Add(string.Empty);

                lientFactureDUcLIENTSplit.Clear();
                lientFactureDUcLIENT.Clear();
                int i = 0;

                foreach (ServiceRecouvrement.CsDetailMoratoire moratoires in l)
                {
                    string filter = moratoires.IDMORATOIRE .ToString();
                    if (!referNdoc.Contains(filter))
                    {
                        referNdoc.Add(filter);

                        string MemberPath = moratoires.REFEM + "              " + moratoires.NDOC + "                " + moratoires.IDMORATOIRE ;
                        string valuePath = moratoires.REFEM + "." + moratoires.NDOC + "." + moratoires.IDMORATOIRE ;

                        lientFactureDUcLIENTSplit.Add(valuePath);
                        lientFactureDUcLIENT.Add(MemberPath);
                    }

                }

                if (lientFactureDUcLIENT == null || lientFactureDUcLIENT.Count == 0)
                {
                    CleanIHM();
                    return;
                }


                Cbo_ListeClient.ItemsSource = null;
                Cbo_ListeClient.ItemsSource = lientFactureDUcLIENT.ToArray();
                this.Cbo_ListeClient.SelectedItem = lientFactureDUcLIENT[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
                     
      private void button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FrmRechercheClient recherche = new FrmRechercheClient();
                recherche.Closed += (s, es) =>
                    {
                        txtReference.Text = string.IsNullOrEmpty(recherche.CustomerRef) ? string.Empty : recherche.CustomerRef;
                    };
                recherche.Show();
            }
            catch (Exception ex)
            {
               Message.Show(ex,Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

      private void btncancel_Click(object sender, RoutedEventArgs e)
      {
          this.DialogResult = true; 
      }
      private void ValiderSuppression()
      {
          List<CsDetailMoratoire> lstDistinctFactureDuMoratoire = listeDetailsMoratoires.Where(t => t.IDMORATOIRE == int.Parse(idmoratoireselected)).ToList();

          RecouvrementServiceClient clients = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
          clients.VerifiePaiementMoratoireCompleted += (send, res) =>
          {
              //impression du recu du moratoire
              try
              {
                  prgBar.Visibility = System.Windows.Visibility.Visible;
                  if (res.Cancelled || res.Error != null)
                  {

                      Message.ShowError("Erreur survenue lors de l'appel serveur.", "Erreur");
                      CleanIHM();
                      return;
                  }
                  if (res.Result == null)
                  {
                      Message.ShowError("Erreur survenue lors de la suppcression du moratoire.Réessayer svp!", Galatee.Silverlight.Resources.Langue.errorTitle);
                      CleanIHM();
                      return;
                  }
                  List<CsDetailMoratoire> lstMoratoirePayer = new List<CsDetailMoratoire>();

                  if (res.Result.Count == 0)
                  {
                      lstMoratoirePayer = res.Result;
                      MiseAJoureMoratoire(lstDistinctFactureDuMoratoire, lstMoratoirePayer);
                  }
                  else
                  {
                      var ws = new MessageBoxControl.MessageBoxChildWindow("Recouvrement", Galatee.Silverlight.Resources.Langue.MsgImpossibleDeSupprimerMor, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                      ws.OnMessageBoxClosed += (l, results) =>
                      {
                          if (ws.Result == MessageBoxResult.OK)
                              MiseAJoureMoratoire(lstDistinctFactureDuMoratoire, lstMoratoirePayer);
                      };
                      ws.Show();
                  }
              }
              catch (Exception ex)
              {
                  Message.ShowError(ex, "Erreur");
              }
          };
          clients.VerifiePaiementMoratoireAsync(int.Parse(idmoratoireselected));
      }
      private void SupprimerMoratoire()
      {
          try
          {
              RecouvrementServiceClient clients = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
              clients.SuppressionMoratoireDuClientAsync(int.Parse(idmoratoireselected));
              clients.SuppressionMoratoireDuClientCompleted += (send, res) =>
              {
                  //impression du recu du moratoire
                  try
                  {

                      if (res.Cancelled || res.Error != null)
                      {
                          Message.ShowError("Erreur survenue lors de l'appel serveur.", "Erreur");
                          CleanIHM();

                          return;
                      }

                      if (res.Result == null)
                      {
                          Message.ShowError("Erreur survenue lors de la suppression du moratoire.Réessayer svp!", Galatee.Silverlight.Resources.Langue.errorTitle);
                          CleanIHM();

                          return;
                      }

                      List<CsDetailMoratoire> lstDistinctFactureDuMoratoire = listeDetailsMoratoires.Where(t => t.IDMORATOIRE == int.Parse(idmoratoireselected)).ToList();
                      nombreEchaeance = (lstDistinctFactureDuMoratoire.Count() - 1).ToString();
                      amountselected = (lstDistinctFactureDuMoratoire[0].MONTANT).ToString();

                      Dictionary<string, string> param = new Dictionary<string, string>();
                      param.Add("pCentre", centre);
                      param.Add("pClient", client);
                      param.Add("pOrdre", ordre);
                      param.Add("pNombreEcheance", (lstDistinctFactureDuMoratoire.Count() - 1).ToString());
                      param.Add("pMontantGlobal", (lstDistinctFactureDuMoratoire[0].MONTANT).Value.ToString(SessionObject.FormatMontant));
                      param.Add("pName", txtClientName.Text);
                      param.Add("pNombreFacture", lstDistinctFactureDuMoratoire.First().REFERENCE.ToString());
                      param.Add("pReferencefacture", lstDistinctFactureDuMoratoire.First().REFEMNDOC.ToString());
                      param.Add("pOperationMor", "ANNULATION MORATOIRE");
                      foreach (CsDetailMoratoire item in lstDistinctFactureDuMoratoire)
                      {
                          if (item.EXIGIBILITE != null)
                              item.DC = item.EXIGIBILITE.Value.ToShortDateString ();
                      }
                      param.Add("pExigibilite", lstDistinctFactureDuMoratoire.Where(t=>t.EXIGIBILITE!=null).First(t => t.EXIGIBILITE != null).DC.ToString());
                      Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(RetourneCsLclientFromCsMoratoire(lstDistinctFactureDuMoratoire.Where(t => t.EXIGIBILITE != null).ToList()), param, SessionObject.CheminImpression, "moratoire", "Recouvrement", false);

                      CleanIHM();
                  }
                  catch (Exception ex)
                  {
                      prgBar.Visibility = System.Windows.Visibility.Visible ;
                      Message.ShowError(ex, "Erreur");
                  }
              };
          }
          catch (Exception ex)
          {
              
              throw ex;
          }

      }
      private void MiseAJoureMoratoire(List<CsDetailMoratoire> lstDetailMoratoire, List<CsDetailMoratoire> lstMoratoirePayer)
      {
          try
          {
              List<CsDetailMoratoire> lstNonRegle = lstDetailMoratoire.Where(p => !lstMoratoirePayer.Any(o => o.NDOC == p.NDOC && o.REFEM == p.REFEM)).ToList();
              lstDetailMoratoire.ForEach(t => t.EXIGIBILITE = System.DateTime.Today);
              RecouvrementServiceClient clients = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
              clients.MiseAJourMoratoireAsync(int.Parse(idmoratoireselected));
              clients.MiseAJourMoratoireCompleted += (send, res) =>
              {
                  try
                  {

                      if (res.Cancelled || res.Error != null)
                      {
                          Message.ShowError("Erreur survenue lors de l'appel serveur.", "Erreur");
                          CleanIHM();
                          return;
                      }

                      if (res.Result == null)
                      {
                          Message.ShowError("Erreur survenue lors de la suppression du moratoire.Réessayer svp!", Galatee.Silverlight.Resources.Langue.errorTitle);
                          CleanIHM();
                          return;
                      }
                      List<CsDetailMoratoire> lstDistinctFactureDuMoratoire = listeDetailsMoratoires.Where(t => t.IDMORATOIRE == int.Parse(idmoratoireselected)).ToList();
                      nombreEchaeance = (lstDistinctFactureDuMoratoire.Count() - 1).ToString();
                      amountselected = (lstDistinctFactureDuMoratoire[0].MONTANT).ToString();

                      Dictionary<string, string> param = new Dictionary<string, string>();
                      param.Add("pCentre", centre);
                      param.Add("pClient", client);
                      param.Add("pOrdre", ordre);
                      param.Add("pNombreEcheance", (lstDistinctFactureDuMoratoire.Count() - 1).ToString());
                      param.Add("pMontantGlobal", (lstDistinctFactureDuMoratoire[0].MONTANT).Value.ToString(SessionObject.FormatMontant));
                      param.Add("pName", txtClientName.Text);
                      param.Add("pNombreFacture", lstDistinctFactureDuMoratoire.First().REFERENCE.ToString());
                      param.Add("pReferencefacture", lstDistinctFactureDuMoratoire.First().REFEMNDOC.ToString());
                      param.Add("pOperationMor", "ANNULATION MORATOIRE");

                      foreach (CsDetailMoratoire item in lstDistinctFactureDuMoratoire)
                      {
                          if (item.EXIGIBILITE != null)
                              item.DC =System.DateTime.Today.Date.ToShortDateString();
                      }
                      param.Add("pExigibilite", lstDistinctFactureDuMoratoire.First().DC.ToString());

                      Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(RetourneCsLclientFromCsMoratoire(lstDistinctFactureDuMoratoire), param, SessionObject.CheminImpression, "moratoire", "Recouvrement", true);
                      //Utility.ActionPreview<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(RetourneCsLclientFromCsMoratoire(listeDetailsMoratoires), param, "moratoire", "Recouvrement");
                      CleanIHM();
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
      private void btnOk_Click(object sender, RoutedEventArgs e)
      {
          try
          {
              if (string.IsNullOrEmpty(txtReference.Text) || listeDetailsMoratoires == null || listeDetailsMoratoires.Count == 0)
                  return;
              ValiderSuppression();
          }
          catch (Exception ex)
          {
           Message.ShowError(ex,Galatee.Silverlight.Resources.Langue.errorTitle);
          }

       //   CleanIHM();
      }
      private List<CsLclient> RetourneCsLclientFromCsMoratoire(List<CsDetailMoratoire> DetailMor)
      {
          List<CsLclient> _lstAEditer = new List<CsLclient>();
          foreach (CsDetailMoratoire m in DetailMor)
          {
              CsLclient cmpte = new CsLclient();

              cmpte.CRET = m.CRET;
              cmpte.DATEVALEUR = m.DATEVALEUR;
              cmpte.DC = m.DC;
              cmpte.EXIGIBILITE = m.EXIGIBILITE;
              cmpte.CENTRE = m.CENTRE;
              cmpte.CLIENT = m.CLIENT;
              cmpte.COPER = m.COPER;
              cmpte.NATURE = m.NATURE;
              cmpte.ORDRE = m.ORDRE;
              cmpte.NDOC = m.NDOC;
              cmpte.TOP1 = m.TOP1;
              cmpte.FRAISDERETARD = m.FRAISDERETARD;
              cmpte.MOISCOMPT = m.MOISCOMPT;
              cmpte.MONTANT = m.MONTANT;

              cmpte.REFEM = m.REFEM;
              cmpte.MATRICULE = m.MATRICULE;

              cmpte.USERCREATION = m.USERCREATION;
              cmpte.DATECREATION = m.DATECREATION;
              cmpte.CAISSE = m.CAISSE;
              cmpte.MODEREG = m.MODEREG;
              cmpte.USERCREATION = m.MATRICULE;
              cmpte.DATECREATION = m.DATECREATION;
              cmpte.FK_IDCLIENT = m.FK_IDCLIENT;

              // valorisation des propriétés foreign key

              cmpte.FK_IDCENTRE = m.FK_IDCENTRE;
              cmpte.FK_IDCOPER = m.FK_IDCOPER;
              cmpte.FK_IDLIBELLETOP = m.FK_IDLIBELLETOP;
              cmpte.FK_IDNATURE = m.FK_IDNATURE;
              cmpte.FK_IDADMUTILISATEUR = m.FK_IDADMUTILISATEUR;

              _lstAEditer.Add(cmpte);
          }
          return _lstAEditer;

      }
    

      private void Cbo_ListeClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
          try
          {
              if (Cbo_ListeClient.SelectedItem != null)
              {
                  if (lientFactureDUcLIENTSplit != null && lientFactureDUcLIENTSplit.Count > 0)
                  {
                      idmoratoireselected = lientFactureDUcLIENTSplit[Cbo_ListeClient.SelectedIndex].Split('.')[2];
                      refermSelected = lientFactureDUcLIENTSplit[Cbo_ListeClient.SelectedIndex].Split('.')[0];
                      ndocSelected = lientFactureDUcLIENTSplit[Cbo_ListeClient.SelectedIndex].Split('.')[1];

                      List<ServiceRecouvrement.CsDetailMoratoire> listemoratDetil = new List<ServiceRecouvrement.CsDetailMoratoire>();
                      if (detailSelectionne != null && detailSelectionne.Count > 0)
                          detailSelectionne.Clear();

                      listemoratDetil = listeDetailsMoratoires.Where(t => t.IDMORATOIRE  == int.Parse(idmoratoireselected)).ToList();
                      detailSelectionne.AddRange(listemoratDetil);
                      FillDetailData(listemoratDetil.Where(t=>t.EXIGIBILITE != null ).ToList());

                      ServiceRecouvrement.CsDetailMoratoire selectionneMoratoire = listeDetailsMoratoires[Cbo_ListeClient.SelectedIndex];
                      txtDue.Text = selectionneMoratoire.MONTANT == null ? "0" : selectionneMoratoire.MONTANT.Value.ToString(SessionObject.FormatMontant); ;
                  }

              }
          }
          catch (Exception ex)
          {
              Message.ShowError(ex,Galatee.Silverlight.Resources.Langue.errorTitle);
          }
      }
    }
}



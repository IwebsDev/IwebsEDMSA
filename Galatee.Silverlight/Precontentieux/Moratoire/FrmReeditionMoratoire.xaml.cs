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
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Caisse;

namespace Galatee.Silverlight.Precontentieux
{
    public partial class FrmReeditionMoratoire : ChildWindow
    {
        public FrmReeditionMoratoire()
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
        int refClient = SessionObject.Enumere.TailleCentre +
                            SessionObject.Enumere.TailleClient +
                            SessionObject.Enumere.TailleOrdre;

        int debutClient = SessionObject.Enumere.TailleCentre;
        int debutOrdre = SessionObject.Enumere.TailleCentre + SessionObject.Enumere.TailleClient;
        int TailleReferenceClient = SessionObject.Enumere.TailleCentre +
                            SessionObject.Enumere.TailleClient +
                            SessionObject.Enumere.TailleOrdre;

        List<string> lientFactureDUcLIENT = new List<string>();
        List<string> lientFactureDUcLIENTSplit = new List<string>();
        List<string> listeAmountMoratoire = new List<string>();
        List<string> listeDateMoratoire = new List<string>();


        List<CsDetailMoratoire> listeDetailsMoratoires = new List<CsDetailMoratoire>();
        List<CsDetailMoratoire> detailSelectionne = new List<CsDetailMoratoire>();

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
                    TextBox charges = AllInOne.FindControl<TextBox>((UIElement)LayoutRoot, typeof(TextBox), "txtCharg" + i);
                    //amounts.Text = string.Empty;
                    //date.Text = string.Empty;
                    //charges.Text = string.Empty;

                }
                prgBar.Visibility = System.Windows.Visibility.Collapsed;

                txtClientName.Text = string.Empty;
                txtClientAdresse.Text = string.Empty;
                //txtReference.Text = string.Empty;
                txtDue.Text = string.Empty;
                txtNoInstalmnt.Text = string.Empty;

                Cbo_ListeClient.ItemsSource = null;

                if (listeDateMoratoire != null)
                    listeDateMoratoire.Clear();
                if (listeAmountMoratoire != null)
                    listeAmountMoratoire.Clear();

                amountselected = string.Empty;

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

        private void txtReference_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtReference.Text.Length != refClient)
                {
                    btnOk.IsEnabled  = false;
                    return;
                }
                if (string.IsNullOrEmpty(txtReference.Text))
                    CleanIHM();
                else
                    AfficherImpayes(txtReference.Text);
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
        private void AfficherImpayes(string refClient)
        {
            try
            {
                int debutClient = SessionObject.Enumere.TailleCentre;
                int debutOrdre = SessionObject.Enumere.TailleCentre + SessionObject.Enumere.TailleClient;

                centre = refClient.Substring(0, SessionObject.Enumere.TailleCentre);
                client = refClient.Substring(debutClient, SessionObject.Enumere.TailleClient);
                ordre = refClient.Substring(debutOrdre, SessionObject.Enumere.TailleOrdre);
                prgBar.Visibility = System.Windows.Visibility.Visible ;

                RecouvrementServiceClient srv = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                srv.TestClientExistCompleted += new EventHandler<TestClientExistCompletedEventArgs>(TestClientExist);// Call back sur la methode TestClientTxi
                srv.TestClientExistAsync(centre, client, ordre);
            }
            catch (Exception ex)
            {
                CleanIHM();
                btnOk.IsEnabled = false;
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
                if (e.Cancelled || e.Error != null )
                {
                    Message.ShowError("Erreur d'invocation du serveur.",Silverlight.Resources.Langue.wcf_error);
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                    return;
                }
                if (e.Result == null )
                {
                    Message.Show("Aucun client ne correspond à cette référence ", Silverlight.Resources.Langue.wcf_error);
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                    return;
                }
                if ( e.Result.Count ==0)
                {
                    Message.Show("Aucun client ne correspond à cette référence ", Silverlight.Resources.Langue.wcf_error);
                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                    return;
                }
                else
                {
                    txtClientName.Text = e.Result[0].NOMABON;
                    txtClientAdresse.Text = e.Result[0].ADRMAND1;
                    RecouvrementServiceClient srv = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    srv.RetourneMoratoireDuClientCompleted += (es, res) =>
                        {
                            try
                            {
                                if (res.Cancelled || res.Error != null)
                                {
                                    Message.ShowError(res.Error.Message, Galatee.Silverlight.Resources.Langue.wcf_error);
                                    prgBar.Visibility = System.Windows.Visibility.Collapsed;

                                    return;
                                }
                                if (res.Result == null || res.Result.Count == 0)
                                {
                                    Message.ShowError("Aucun moratoire retourné pour ce client. ", Galatee.Silverlight.Resources.Langue.errorTitle);

                                    CleanIHM();
                                    return;
                                }
                                btnOk.IsEnabled = true ;
                                listeDetailsMoratoires = res.Result;
                                if (listeDetailsMoratoires != null)
                                    FillCombobox(listeDetailsMoratoires);
                                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                            }
                            catch (Exception ex)
                            {
                              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                              prgBar.Visibility = System.Windows.Visibility.Collapsed;

                            }
                        };
                    srv.RetourneMoratoireDuClientAsync(centre, client, ordre);
                }
            }
            catch (Exception ex)
            {
              Message.ShowError(ex,Galatee.Silverlight.Resources.Langue.errorTitle);
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
                        amounts.Text = moratoires.MONTANT == null ? "0" : moratoires.MONTANT.Value .ToString(SessionObject.FormatMontant );
                        dateBox.Text = (moratoires.EXIGIBILITE == null ? string.Empty : moratoires.EXIGIBILITE.Value.ToString("d"));
                    }
                    else
                    {
                        // listeIsCret.Add(moratoires);
                        amounts.Text = moratoires.MONTANT.Value.ToString(SessionObject.FormatMontant);
                        dateBox.Text = (moratoires.EXIGIBILITE == null ? string.Empty : moratoires.EXIGIBILITE.Value.ToString("d"));
                    }
                }

                Decimal fraisFacture = listeIsCret.Sum(p => p.FRAISDERETARD == null ? 0 : p.FRAISDERETARD.Value);
                Decimal montantFacture = listeIsCret.Sum(p => p.MONTANT == null ? 0 : p.MONTANT.Value);
                amountselected = txtDue.Text = (montantFacture - fraisFacture).ToString(SessionObject.FormatMontant);
                nombreEchaeance = listeIsCret.Count.ToString();
                txtNoInstalmnt.Text = listeIsCret.Count.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void FillCombobox(List<CsDetailMoratoire> l)
        {
            try
            {
                List<CsDetailMoratoire> detailParNdocReferm = new List<CsDetailMoratoire>();
                List<string> referNdoc = new List<string>();
                referNdoc.Add(string.Empty);

                lientFactureDUcLIENTSplit.Clear();
                lientFactureDUcLIENT.Clear();
                int i = 0;

                foreach (CsDetailMoratoire moratoires in l)
                {
                    string filter = moratoires.IDMORATOIRE.ToString();
                    if (!referNdoc.Contains(filter))
                    {
                        referNdoc.Add(moratoires.IDMORATOIRE.ToString());

                        string MemberPath = moratoires.REFEM + "              " + moratoires.NDOC + "                " + moratoires.IDMORATOIRE;
                        string valuePath = moratoires.REFEM + "." + moratoires.NDOC + "." + moratoires.IDMORATOIRE;

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
            catch (Exception ex )
            {
              throw ex;
            }
          
      }
                     
      private void button3_Click(object sender, RoutedEventArgs e)
        {
            FrmRechercheClient recherche = new FrmRechercheClient();
            recherche.Closed += (s, es) =>
                {
                    try
                    {
                        txtReference.Text = string.IsNullOrEmpty(recherche.CustomerRef) ? string.Empty : recherche.CustomerRef;
                    }
                    catch (Exception ex )
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
            recherche.Show();
        }

      private void btncancel_Click(object sender, RoutedEventArgs e)
      {
          this.DialogResult = true; 
      }

      private void btnOk_Click(object sender, RoutedEventArgs e)
      {
          try
          {
              if (string.IsNullOrEmpty(txtReference.Text) || listeDetailsMoratoires == null || listeDetailsMoratoires.Count == 0)
                  return;

              List<CsDetailMoratoire> lstDistinctFactureDuMoratoire = listeDetailsMoratoires.Where(t => t.IDMORATOIRE == int.Parse(idmoratoireselected) && t.EXIGIBILITE != null ).ToList();
              nombreEchaeance = (lstDistinctFactureDuMoratoire.Count()).ToString();
              amountselected = (lstDistinctFactureDuMoratoire.Sum(t => t.MONTANT)).ToString();

              Dictionary<string, string> param = new Dictionary<string, string>();
              param.Add("pCentre", centre);
              param.Add("pClient", client);
              param.Add("pOrdre", ordre);
              param.Add("pNombreEcheance", (lstDistinctFactureDuMoratoire.Count()).ToString());
              param.Add("pMontantGlobal", ( lstDistinctFactureDuMoratoire.Sum(t=>t.MONTANT)).Value.ToString(SessionObject.FormatMontant));
              param.Add("pName", txtClientName.Text);
              param.Add("pNombreFacture", lstDistinctFactureDuMoratoire.Count().ToString());
              param.Add("pReferencefacture", lstDistinctFactureDuMoratoire.First().REFEMNDOC.ToString());
              param.Add("pOperationMor", "DUPLICATA MORATOIRE");

              foreach (CsDetailMoratoire item in lstDistinctFactureDuMoratoire)
              {
                  if (item.EXIGIBILITE != null)
                      item.DC = item.EXIGIBILITE.Value.ToShortDateString ();
              }
              param.Add("pExigibilite", lstDistinctFactureDuMoratoire.First().DC .ToString());

              Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceRecouvrement.CsLclient>(RetourneCsLclientFromCsMoratoire(lstDistinctFactureDuMoratoire), param, SessionObject.CheminImpression, "moratoire", "Recouvrement", true);
          }
          catch (Exception EX)
          {
              Message.ShowError(EX,Galatee.Silverlight.Resources.Langue.errorTitle);
          }
      }
      private List<CsLclient> RetourneCsLclientFromCsMoratoire(List<CsDetailMoratoire> DetailMor)
      {
          List<CsLclient> _lstAEditer = new List<CsLclient>();
          foreach (CsDetailMoratoire m in DetailMor)
                {
                    CsLclient  cmpte = new CsLclient();
                  
                    cmpte.CRET = m.CRET;
                    cmpte.DATEVALEUR = m.DATEVALEUR;
                    cmpte.DC = m.DC;
                    cmpte.EXIGIBILITE = m.EXIGIBILITE;
                    cmpte.CENTRE = m.CENTRE;
                    cmpte.CLIENT = m.CLIENT;
                    cmpte.NDOC = m.NDOC;
                    cmpte.COPER = m.COPER;
                    cmpte.NATURE = m.NATURE;
                    cmpte.ORDRE = m.ORDRE;
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
               

                      listemoratDetil = listeDetailsMoratoires.Where(t => t.IDMORATOIRE == int.Parse(idmoratoireselected) && t.EXIGIBILITE != null ).ToList();

                      detailSelectionne.AddRange(listemoratDetil);
                      FillDetailData(listemoratDetil);

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

      private void txtClientName_TextChanged(object sender, TextChangedEventArgs e)
      {
          try
          {
          }
          catch (Exception ex)
          {
              Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
          }
      }
    }
}



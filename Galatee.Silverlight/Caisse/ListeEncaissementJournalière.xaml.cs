using Galatee.Silverlight.Devis;
using Galatee.Silverlight.ServiceCaisse;
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

namespace Galatee.Silverlight.Caisse
{
    public partial class ListeEncaissementJournalière : ChildWindow
    {
        public ListeEncaissementJournalière()
        {
            InitializeComponent();
            CustumeInit();
            this.DialogResult = false;

        }

        private void CustumeInit()
        {
            TxtDateCaisse.IsReadOnly = true;
            TxtDateCaisse.Text = SessionObject.LaCaisseCourante.DATE_DEBUT.Value.ToShortDateString();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtDateCaisse.Text))
            {
                ListeDesTransactions(SessionObject.LaCaisseCourante);
            }
            this.DialogResult = true;
        }

        private void ListeDesTransactions(CsHabilitationCaisse laCaisse)
        {
            int handler = LoadingManager.BeginLoading("Traitement en cours ...");

            try
            {

                if (!string.IsNullOrWhiteSpace(TxtDateCaisse.Text))
                {
                    DateTime debut = DateTime.Today;
                    DateTime fin = (DateTime.Today.AddDays(1));

                    CaisseServiceClient proxy = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                    proxy.LitseDesTransactionAsync(laCaisse);
                    proxy.LitseDesTransactionCompleted  += (senders, results) =>
                    {
                        if (results.Cancelled || results.Error != null)
                        {
                            string error = results.Error.Message;
                            MessageBox.Show("errror occurs while calling remote method", "ReportListeEncaissements", MessageBoxButton.OK);
                            return;
                        }
                        if (results.Result == null || results.Result.Count == 0)
                        {
                            Message.ShowInformation("Aucune donnée trouvée", "Caisse");
                            return;
                        }

                        List<ServiceCaisse.CsLclient> dataTable = new List<ServiceCaisse.CsLclient>();
                        List<ServiceCaisse.CsLclient> tri = new List<ServiceCaisse.CsLclient>();
                        dataTable.AddRange(results.Result);
                        
                        tri = results.Result.OrderBy(t => t.DTRANS).ToList();
                        debut = tri[0].DTRANS.Value;
                        fin = tri[tri.Count-1].DTRANS.Value;


                        Dictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("pUser", !string.IsNullOrWhiteSpace(SessionObject.LaCaisseCourante.MATRICULE) ? "Matricule : " + SessionObject.LaCaisseCourante.NOMCAISSE  : "Matricule :Aucun");
                        param.Add("pDateDebut", "Date debut : " + debut);
                        param.Add("pDateFin", "Date fin : " + fin);

                        string key = Utility.getKey();
                        Utility.ActionDirectOrientation<ServicePrintings.CsLclient, ServiceCaisse.CsLclient>(dataTable, param, SessionObject.CheminImpression, "ListeDesTransactions".Trim(), "Caisse".Trim(), true);
                        LoadingManager.EndLoading(handler);
                    };
                }
                else
                {
                    Message.Show("Veuillez choisir un utilisateur et réessayer svp! ", "Information");
                    LoadingManager.EndLoading(handler);
                }
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(handler);
            
            }
            finally
            {

            }
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
  
    }
}


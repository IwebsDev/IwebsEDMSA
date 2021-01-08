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
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.Resources.Accueil;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmRechercheTransfert : ChildWindow
    {
        public FrmRechercheTransfert()
        {
            InitializeComponent();
            ChargerCombo();
        }

        private void ChargerCombo()
        {

            List<CsCoper> LstCoper = new List<CsCoper>();
            LstCoper=SessionObject.LstDesCopers;
            //LstCoper = LstCoper.Where(c => c.CODEOPERATION == SessionObject.Enumere.CoperTransfertDebit || c.CODEOPERATION == SessionObject.Enumere.CoperTransfertDette || c.CODEOPERATION == SessionObject.Enumere.CoperTransfertSolde || c.CODEOPERATION == SessionObject.Enumere.CoperTransfertRemboursement).ToList();
            cboCoper.ItemsSource = LstCoper;
            cboCoper.DisplayMemberPath = "LIBELLE";
            cboCoper.SelectedValuePath = "CODEOPERATION";
            

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChargerGrid(true);
        }

        private void ChargerGrid( bool depart)
        {
            dgDemande.ItemsSource = null;
            List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service1.RetourneListeDemandeCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;
                LstDemande = res.Result;
                LstDemande = LstDemande.Where(c => !c.STATUTDEMANDE.Equals("1")).ToList();
                if (LstDemande != null && LstDemande.Count != 0)
                { 
                    //foreach (CsDemandeBase item in LstDemande)
                    //{
                    //    item.LIBELLE = SessionObject.LstTypeDemande.FirstOrDefault(p => p.IDTDEM == item.TYPEDEMANDE).LIBELLE;
                    //    item.LIBELLESTATUT = RetourneLibelleStatutDemande(item);
                    //}

                    //UcDemandeAfficheRecherche ctrl = new UcDemandeAfficheRecherche(LstDemande, Langue.lbl_Recherche);
                    
                    dgDemande.ItemsSource = LstDemande;
                    //ctrl.Show();
                }
                else
                {
                    if(depart== true)
                    Message.ShowError(Langue.MsgDemandeNonTrouve, Langue.lbl_Menu);
                }

            };
            service1.RetourneListeDemandeAsync(string.Empty, string.Empty, RecupereCriterDemande(cboCoper.SelectedValue.ToString()), null, null, null, null, string.Empty, SessionObject.Enumere.DemandeStatusEnAttente);
            service1.CloseAsync();
        }

        private string RetourneLibelleStatutDemande(CsDemandeBase _leDemande)
        {
            string Libelle = string.Empty;
            if (_leDemande.STATUT == "1")
                if (string.IsNullOrEmpty(_leDemande.STATUTDEMANDE))
                    Libelle = Langue.lib_Statut_EnAttente;
                else
                    Libelle = Langue.lib_Statut_Rejeter;

            else if (_leDemande.STATUT == "3")
                Libelle = Langue.lib_Statut_terminer;

            else if (_leDemande.STATUT == "2")
                Libelle = Langue.lib_Statut_EnCaisse;

            return Libelle;

        }

        private List<string> RecupereCriterDemande(string coper)
        {
            List<string> critere = new List<string>();
            if (coper == SessionObject.Enumere.CoperTransfertDebit)
                critere.Add(SessionObject.Enumere.RechercheDemandeDebit);
            if (coper == SessionObject.Enumere.CoperTransfertDette)
                critere.Add(SessionObject.Enumere.RechercheDemandeCredit);
            //if (coper == SessionObject.Enumere.CoperTransfertSolde)
            //    critere.Add(SessionObject.Enumere.RechercheDemandeSolde);
            //if (coper == SessionObject.Enumere.CoperTransfertRemboursement)
            //    critere.Add(SessionObject.Enumere.RechercheDemandeRemboursement);

            return critere;

        }

        private void dgDemande_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgDemande.SelectedItem != null)
            {
                RetourneDetailDemande((CsDemandeBase)dgDemande.SelectedItem);
            }
        }

        private void RetourneDetailDemande(CsDemandeBase laDemandeSelect)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);

            try
            {
                CsDemande leDetailDemande = new CsDemande();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneDetailDemandeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    leDetailDemande = args.Result;
                    LoadingManager.EndLoading(res);

                    if (laDemandeSelect.LIBELLESTATUT == Langue.lib_Statut_EnAttente ||
                        laDemandeSelect.LIBELLESTATUT == Langue.lib_Statut_Rejeter)
                    {
                        if (leDetailDemande.LstCoutDemande.FirstOrDefault(c => c.COPER == SessionObject.Enumere.CoperTransfertDebit) != null || leDetailDemande.LstCoutDemande.FirstOrDefault(c => c.COPER == SessionObject.Enumere.CoperTransfertDette) != null)
                        {
                            FrmTranfertDettePaiement ctrl = new FrmTranfertDettePaiement(leDetailDemande, Langue.lbl_Recherche);
                            ctrl.Closed += new EventHandler(RafraichirList);
                            ctrl.Show();
                        }
                        //if (leDetailDemande.LstCoutDemande.FirstOrDefault(c => c.COPER == SessionObject.Enumere.CoperTransfertSolde) != null)
                        //{
                        //    FrmTranfertSole ctrl = new FrmTranfertSole(leDetailDemande, Langue.lbl_Recherche);
                        //    ctrl.Closed += new EventHandler(RafraichirList);
                        //    ctrl.Show();
                        //}
                        //if (leDetailDemande.LstCoutDemande.FirstOrDefault(c => c.COPER == SessionObject.Enumere.CoperTransfertRemboursement) != null)
                        //{
                        //    FrmTranfertRemboursement ctrl = new FrmTranfertRemboursement(leDetailDemande, Langue.lbl_Recherche);
                        //    ctrl.Closed += new EventHandler(RafraichirList);
                        //    ctrl.Show();
                        //}

                        
                    }
                };
                service.RetourneDetailDemandeAsync(laDemandeSelect);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }
        }

        private void RafraichirList(object sender, EventArgs e)
        {
            ChargerGrid(false);
        }

    }
}


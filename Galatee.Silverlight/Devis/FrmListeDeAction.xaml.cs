using Galatee.Silverlight.ServiceAccueil;
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

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmListeDeAction : ChildWindow
    {
        public FrmListeDeAction()
        {
            InitializeComponent();
        }

        string leEtatExecuter = string.Empty;
        public FrmListeDeAction(string typeEtat)
        {
            InitializeComponent();
            leEtatExecuter = typeEtat;
            ChargeEquipe(UserConnecte.Centre);
        }
        List<CsGroupe> lstequipe = new List<CsGroupe>();
        private void ChargeEquipe(string p)
        {
            try
            {
                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeGroupeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    lstequipe = res.Result;
                    cboEquipe.ItemsSource = lstequipe;
                    cboEquipe.SelectedValuePath = "ID";
                    cboEquipe.DisplayMemberPath = "LIBELLE";

                };
                service1.RetourneListeGroupeAsync(UserConnecte.Centre);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        public List<CsProgarmmation> lstprogrammation = new List<CsProgarmmation>();

        private void ChargeProgrammation()
        {
            try
            {
                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneProgrammationCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    lstprogrammation = res.Result;
                    if (leEtatExecuter == SessionObject.EditionProgrammation)
                    {
                        if (lstprogrammation != null && lstprogrammation.Count != 0)
                        {
                            if (this.dtp_DateFin.SelectedDate != null && this.dtp_DateFin.SelectedDate != null)
                            {
                                List<CsProgarmmation> lstPgr = lstprogrammation.Where(t => t.DATEPROGRAMME >= this.dtp_DateDebut .SelectedDate && t.DATEPROGRAMME <= this.dtp_DateFin.SelectedDate).ToList();
                                if (lstPgr != null && lstPgr.Count != 0)
                                {
                                    lstPgr.ForEach(t => t.EQUIPE = lstequipe.FirstOrDefault(y => y.ID == t.FK_IDEQUIPE).LIBELLE);
                                    Utility.ActionDirectOrientation<ServicePrintings.CsProgarmmation, CsProgarmmation>(lstPgr, null, SessionObject.CheminImpression, "Programmation", "Devis", true);
                                }
                            }
                        }
                    }
                };
                service1.RetourneProgrammationAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }


        private void ChargeSortieMateriel()
        {
            try
            {
                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneSortieMaterielCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    List<CsSortieMateriel> lstSortiMat = res.Result;
                    if (leEtatExecuter == SessionObject.EditionSortieMateriel )
                    {
                        if (lstSortiMat != null && lstSortiMat.Count != 0)
                        {
                            if (this.dtp_DateFin.SelectedDate != null && this.dtp_DateFin.SelectedDate != null)
                            {
                                List<CsSortieMateriel> lstPgr = lstSortiMat.Where(t => t.DATELIVRAISON >= this.dtp_DateDebut.SelectedDate && t.DATELIVRAISON <= this.dtp_DateFin.SelectedDate).ToList();
                                if (lstPgr != null && lstPgr.Count != 0)
                                    RetourneDetailDemande(lstPgr.Select(y => y.FK_IDDEMANDE).ToList(), lstPgr);
                            }
                        }
                    }
                };
                service1.RetourneSortieMaterielAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        private void EditerProgrammation(List<int> demandes)
        {
            try
            {

                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneElementDEvisFromIdDemandeCompleted += (sr, res) =>
                {

                    if (res != null && res.Cancelled)
                        return;
                    //res.Result.ForEach(t => t.NUMDEVIS = this.dtProgram.SelectedDate.Value.ToShortDateString());
                    res.Result.ForEach(t => t.NUMFOURNITURE = ((CsGroupe)this.cboEquipe.SelectedItem).LIBELLE);
                    Utility.ActionDirectOrientation<ServicePrintings.ObjELEMENTDEVIS, ObjELEMENTDEVIS>(res.Result, null, SessionObject.CheminImpression, "FicheProgrammation", "Devis", true);

                };
                service1.RetourneElementDEvisFromIdDemandeAsync(demandes);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (leEtatExecuter == SessionObject.EditionProgrammation)
            {
                ChargeProgrammation();
            }
            else if (leEtatExecuter == SessionObject.EditionSortieMateriel)
            {
                ChargeSortieMateriel();
            }
            this.DialogResult = false;
        }

        private void RetourneDetailDemande(List<int> idDemande,List<CsSortieMateriel> lstSortiMat)
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.GetDemandeByListeNumIdDemandeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                   List<CsDemande>  ListDemande = args.Result;

                    if (ListDemande != null && ListDemande.Count > 0)
                    {
                        List<ObjELEMENTDEVIS> lesElemtEditer = new List<ObjELEMENTDEVIS>();

                        List<CsCanalisation> ListeDeCanalisation = new List<CsCanalisation>();
                        foreach (var item in ListDemande)
                        {
                            if (item.LstCanalistion != null && item.LstCanalistion.Count != 0)
                            {
                                item.LstCanalistion.ForEach(t => t.NOMCLIENT = item.LeClient.NOMABON);
                                item.EltDevis.ForEach(e => e.QUANTITEALIVRET = e.QUANTITE);
                                ListeDeCanalisation.Add(item.LstCanalistion.First());
                                lesElemtEditer.Add(new ObjELEMENTDEVIS {
                                 NUMDEM = item.LaDemande.NUMDEM ,
                                 DESIGNATION = "COMTEUR NUMERO  " + item.LstCanalistion.First().NUMERO ,
                                 QUANTITE = 1,

                                 });

                                lesElemtEditer.AddRange(item.EltDevis);
                                lesElemtEditer.ForEach(t => t.NUMFOURNITURE = ((CsGroupe)this.cboEquipe.SelectedItem).LIBELLE);
                                lesElemtEditer.ForEach(t => t.QUANTITERECAP = lstSortiMat.FirstOrDefault(y => y.NUMDEM == t.NUMDEM).LIVREUR);
                                lesElemtEditer.ForEach(t => t.MontantRecap = lstSortiMat.FirstOrDefault(y => y.NUMDEM == t.NUMDEM).RECEPTEUR);
                                Utility.ActionDirectOrientation<ServicePrintings.ObjELEMENTDEVIS, ObjELEMENTDEVIS>(lesElemtEditer, null, SessionObject.CheminImpression, "SortieMateriel", "Devis", true);
                            }
                        }

                    }
                    else
                    {
                        Message.Show("Aucune demande trouvé", "Information");
                    }

                };
                service.GetDemandeByListeNumIdDemandeAsync(idDemande);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


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
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.ServiceAccueil;

namespace Galatee.Silverlight.Devis
{
    public partial class FrmReeditionSortieMateriel : ChildWindow
    {

        List<CsUtilisateur> lstAllUser = new List<CsUtilisateur>();
        List<CsUtilisateur> lstCentreUser = new List<CsUtilisateur>();
        List<CsSortieMateriel> lstSortie = new List<CsSortieMateriel>();
        List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
        List<CsDemandeBase> listeDemandeSelectionees = new List<CsDemandeBase>();
        List<CsCanalisation> LstDemandeValide = new List<CsCanalisation>();
        List<CsSortieAutreMateriel> lstAutreMateriel = new List<CsSortieAutreMateriel>();
        int? EtapeActuelle;
        int fkiddemande = 0;
        int i = 0;
        CsDemande LaDemande = new CsDemande();
        List<CsDemande> ListDemande = new List<CsDemande>();

       
        List<int> lesCentrePerimetre = new List<int>();
        ServiceAccueil.CsProgarmmation LeProgramme = new CsProgarmmation();
        public FrmReeditionSortieMateriel(ServiceAccueil.CsProgarmmation _leProgramme)
        {
            this.EtapeActuelle = null;
            InitializeComponent();

            LstDemandeValide = new List<CsCanalisation>();
            lstAllUser = new List<CsUtilisateur>();
            lstSortie = new List<CsSortieMateriel>();
            listeDemandeSelectionees = new List<CsDemandeBase>();
            LstDemandeValide = new List<CsCanalisation>();
            lstAutreMateriel = new List<CsSortieAutreMateriel>();
            LeProgramme = _leProgramme;
            ChargerDetailSortieMateriel(LeProgramme);
        }
        CsCanalisation leDetailDemande = new CsCanalisation();
        CsCanalisation demandecheck = new CsCanalisation();
        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            List<CsCanalisation> lesElementsDesDevisSelect = new List<CsCanalisation>();
            demandecheck = ((CheckBox)sender).Tag as CsCanalisation;
            CsCanalisation _LaDemandeSelect = new CsCanalisation();
            if (this.dgDemande.SelectedItem != null)
                _LaDemandeSelect = (CsCanalisation)this.dgDemande.SelectedItem;
            if (_LaDemandeSelect.PK_ID != 0)
            {
                List<CsCanalisation> lesDemandeSelect = ((List<CsCanalisation>)this.dgDemande.ItemsSource).Where(t => t.IsSelect == true).ToList();
                foreach (CsCanalisation item in lesDemandeSelect)
                    lesElementsDesDevisSelect.AddRange( LstCannalisation.Where(t => t.FK_IDDEMANDE == item.FK_IDDEMANDE).ToList());

            }
           var reglemntParModereg = (from p in lesElementsDesDevisSelect
                                          group new { p } by new { p.LIBELLEMATERIEL, p.FK_IDMATERIELDEVIS   } into pResult
                                          select new
                                          {
                                              pResult.Key.LIBELLEMATERIEL ,
                                              pResult.Key.FK_IDMATERIELDEVIS,
                                              QUANTITE = (int)pResult.Where(t => t.p.FK_IDMATERIELDEVIS  == pResult.Key.FK_IDMATERIELDEVIS).Sum(o => o.p.QUANTITE )
                                          });
           List<CsCanalisation> lstEltDevisAffiche = new List<CsCanalisation>();
           foreach (var item in reglemntParModereg)
               lstEltDevisAffiche.Add(new CsCanalisation() { LIBELLEMATERIEL = item.LIBELLEMATERIEL ,QUANTITE =item.QUANTITE   });

           dgAutreMateriel.ItemsSource = null;
           dgAutreMateriel.ItemsSource = lstEltDevisAffiche;

        }
        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            CsCanalisation demandecheck = ((CheckBox)sender).Tag as CsCanalisation;


            CsCanalisation _LaDemandeSelect = new CsCanalisation();
            if (this.dgDemande.SelectedItem != null)
                _LaDemandeSelect = (CsCanalisation)this.dgDemande.SelectedItem;
            if (_LaDemandeSelect.PK_ID != 0)
            {
                LstDemandeValide = LstDemandeValide.Where(c => c.PK_ID != demandecheck.PK_ID).ToList();
            }

            MiseAjourElementDevis();
        }

        private void MiseAjourElementDevis()
        {
            var ListeDemande = ListDemande.Where(d =>  LstDemandeValide.Select(c => c.FK_IDDEMANDE).Contains(d.LaDemande.PK_ID));
            List<ObjELEMENTDEVIS> ElementPrisEnConte = new List<ObjELEMENTDEVIS>();
            foreach (var item in ListeDemande)
                 ElementPrisEnConte.AddRange(item.EltDevis.Where(t =>t.ISFOURNITURE == true && t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null && t.MONTANTTAXE != 0).ToList());

          dgAutreMateriel .ItemsSource=  RetournListeElementAggreger(ElementPrisEnConte);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgAutreMateriel.ItemsSource != null)
                EditionSortieMateriel(LstCannalisation);
        }




        private void EditionSortieMateriel(List<CsCanalisation> lstCanaisation)
        {
            try
            {
                List<ObjELEMENTDEVIS> lstMateriel = new List<ObjELEMENTDEVIS>();
                foreach (CsCanalisation item in lstCanaisation)
                {
                    ObjELEMENTDEVIS lstMat = new ObjELEMENTDEVIS();
                    lstMat.NUMDEM = item.NUMDEM;
                    lstMat.DESIGNATION = item.LIBELLEMATERIEL;
                    lstMat.QUANTITE = item.QUANTITE;
                    lstMat.QUANTITERECAP = this.txt_LibelleAgentLivreur.Text;
                    lstMat.MontantRecap = this.txt_LibelleAgentRecepteur.Text;

                    lstMat.NUMFOURNITURE = LeProgramme.LIBELLEEQUIPE;
                    lstMat.NUMDEVIS = LeProgramme.DATEPROGRAMME.Value.ToShortDateString();
                    lstMat.USERMODIFICATION   = LeProgramme.NUMPROGRAMME ;
                    
                    lstMateriel.Add(lstMat);
                }
                Utility.ActionDirectOrientation<ServicePrintings.ObjELEMENTDEVIS, ObjELEMENTDEVIS>(lstMateriel, null, SessionObject.CheminImpression, "SortieMateriel", "Devis", true);

                 
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

        private List<ObjELEMENTDEVIS> RetournListeElementAggreger(List<ObjELEMENTDEVIS> ElementDeDevis)
        {
            List<ObjELEMENTDEVIS> ElementDeDevisAggrege = (from e in ElementDeDevis
                                                           group new
                                                           {
                                                               e.NUMDEVIS,
                                                               e.NUMDEM,
                                                               e.ORDRE,
                                                               e.QUANTITE,
                                                               e.QUANTITEREMISENSTOCK,
                                                               e.QUANTITECONSOMMEE,
                                                               e.QUANTITELIVRET,
                                                               e.QUANTITEALIVRET,
                                                               e.TAXE,
                                                               e.COUT,
                                                               e.DESIGNATION,
                                                               e.PRIX,
                                                               e.REMBOURSEMENT,
                                                               e.MONTANT,
                                                               e.MONTANTCONSOMME,
                                                               e.MONTANTVALIDE,
                                                               e.PRIX_UNITAIRE,
                                                               e.ISSUMMARY,
                                                               e.ISADDITIONAL,
                                                               e.ISFORTRENCH,
                                                               e.ISDEFAULT,
                                                               e.UTILISE,
                                                               e.COUTRECAP,
                                                               e.TVARECAP,
                                                               e.QUANTITERECAP,
                                                               e.MontantRecap,
                                                               e.REMISE,
                                                               e.CONSOMME,
                                                               e.COUTTOTAL,
                                                               e.DATECREATION,
                                                               e.DATEMODIFICATION,
                                                               e.USERCREATION,
                                                               e.USERMODIFICATION,
                                                               e.CODECOPER,
                                                               e.FK_IDCOPER,
                                                               e.FK_IDTAXE,
                                                               e.FK_IDTDEM,
                                                               e.FK_IDMATERIELDEVIS ,
                                                               e.FK_IDCOUTCOPER,
                                                               e.FK_IDDEMANDE
                                                           } by new
                                                           {
                                                               e.DESIGNATION,
                                                               e.NUMFOURNITURE
                                                           }
                                                               into groupe
                                                               select new ObjELEMENTDEVIS
                                                               {
                                                                   NUMFOURNITURE=groupe.Key.NUMFOURNITURE,
                                                                   DESIGNATION = groupe.Key.DESIGNATION,
                                                                   QUANTITE = groupe.Sum(g => g.QUANTITE),
                                                                   QUANTITEALIVRET = groupe.Sum(g => g.QUANTITEALIVRET)

                                                               }).ToList();

            return ElementDeDevisAggrege;
        }
      
        private void ListeSortieAutreMateriel(CsDemande demande)
        {
            dgAutreMateriel.ItemsSource = demande.EltDevis;
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgAutreMateriel.SelectedItem != null)
                {
                    CsSortieAutreMateriel lAutreSelect = (CsSortieAutreMateriel)dgAutreMateriel.SelectedItem;

                    dgAutreMateriel.ItemsSource = null;
                    lstAutreMateriel = lstAutreMateriel.Where(c => c.LIBELLE != lAutreSelect.LIBELLE && c.NOMBRE != lAutreSelect.NOMBRE).ToList();
                    dgAutreMateriel.ItemsSource = lstAutreMateriel;
                }
            }
            catch
            {
            }
        }
        List<CsCanalisation> LstCannalisation = new List<CsCanalisation>();
        private void ChargerDetailSortieMateriel(ServiceAccueil.CsProgarmmation leProgramme)
        {
            try
            {
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.GetDemandeByListeNumIdDemandeSortieMaterielCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    if (res.Result != null && res.Result.Count != 0)
                    {
                        LstCannalisation = res.Result;
                        List<CsCanalisation> lstElementDatagrid = new List<CsCanalisation>();
                        var DistinctCannalisation = res.Result.Select(u => new { u.FK_IDDEMANDE, u.NUMDEM, u.NUMERO, u.NOMCLIENT, u.CENTRE, u.CLIENT, u.ORDRE }).Distinct();
                        foreach (var item in DistinctCannalisation)
                            lstElementDatagrid.Add(new CsCanalisation() { FK_IDDEMANDE = item.FK_IDDEMANDE, NUMDEM = item.NUMDEM, NUMERO = item.NUMERO, NOMCLIENT = item.NOMCLIENT, CENTRE = item.CENTRE, CLIENT = item.CLIENT, ORDRE = item.ORDRE });

                        dgDemande.ItemsSource = null;
                        dgDemande.ItemsSource = lstElementDatagrid;

                        AgregerMaterielDevis();


                        this.txt_Equipe.Text = leProgramme.LIBELLEEQUIPE;
                        this.txt_DateProgramme.Text = leProgramme.DATEPROGRAMME.Value.ToShortDateString();
                    }
                    else
                        Message.Show("Aucune demande trouvé", "Information");
                };
                service1.GetDemandeByListeNumIdDemandeSortieMaterielAsync(leProgramme.NUMPROGRAMME , EtapeActuelle);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
             
        }
        private void AgregerMaterielDevis()
        {
            List<CsCanalisation> lesElementsDesDevisSelect = new List<CsCanalisation>();
            List<CsCanalisation> lesDemandeSelect = ((List<CsCanalisation>)this.dgDemande.ItemsSource).ToList();
            foreach (CsCanalisation item in lesDemandeSelect)
                lesElementsDesDevisSelect.AddRange(LstCannalisation.Where(t => t.FK_IDDEMANDE == item.FK_IDDEMANDE).ToList());

            var reglemntParModereg = (from p in lesElementsDesDevisSelect
                                      group new { p } by new { p.LIBELLEMATERIEL, p.FK_IDMATERIELDEVIS } into pResult
                                      select new
                                      {
                                          pResult.Key.LIBELLEMATERIEL,
                                          pResult.Key.FK_IDMATERIELDEVIS,
                                          QUANTITE = (int)pResult.Where(t => t.p.FK_IDMATERIELDEVIS == pResult.Key.FK_IDMATERIELDEVIS).Sum(o => o.p.QUANTITE)
                                      });
            List<CsCanalisation> lstEltDevisAffiche = new List<CsCanalisation>();
            foreach (var item in reglemntParModereg)
                lstEltDevisAffiche.Add(new CsCanalisation() { LIBELLEMATERIEL = item.LIBELLEMATERIEL, QUANTITE = item.QUANTITE });

            dgAutreMateriel.ItemsSource = null;
            dgAutreMateriel.ItemsSource = lstEltDevisAffiche;
        
        }
    }
}


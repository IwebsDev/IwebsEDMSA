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
    public partial class UcReeditionPgrammeCompteur : ChildWindow
    {

        List<CsUtilisateur> lstAllUser = new List<CsUtilisateur>();
        List<CsUtilisateur> lstCentreUser = new List<CsUtilisateur>();
        int? EtapeActuelle;
        int fkiddemande = 0;
        int i = 0;
        int TypeEdition = 1;
        ServiceAccueil.CsProgarmmation LeProgramme = new CsProgarmmation();
        public UcReeditionPgrammeCompteur(ServiceAccueil.CsProgarmmation _leProgramme,int typeEdition)
        {
            InitializeComponent();
            LeProgramme = _leProgramme;
            EtapeActuelle = null;
            TypeEdition = typeEdition;
            if (typeEdition == 1)
            {
                this.txt_LibelleAgentLivreur.Visibility = System.Windows.Visibility.Collapsed;
                this.txt_LibelleAgentRecepteur.Visibility = System.Windows.Visibility.Collapsed;
                this.lbl_Livreur.Visibility = System.Windows.Visibility.Collapsed;
                this.lbl_Recepteur.Visibility = System.Windows.Visibility.Collapsed;
                ChargerDetailProgramme(_leProgramme);
            }
            else 
            ChargerDetailSortieCompteur(_leProgramme);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (TypeEdition == 1)
            {
                if (Programme != null && Programme.Count != 0)
                    EditionProgramme(Programme);
            }
            else if (TypeEdition == 2 && dgDemande.ItemsSource != null)
                EditionSortieCompteur((List<CsCanalisation>)dgDemande.ItemsSource);
        }
        private void EditionSortieCompteur(List<CsCanalisation> lesCompteurs)
        {
            lesCompteurs.ForEach(t => t.COMMENTAIRE = this.txt_LibelleAgentLivreur.Text);
            lesCompteurs.ForEach(t => t.ETATDUCOMPTEUR = this.txt_LibelleAgentRecepteur.Text);
            lesCompteurs.ForEach(t => t.CODECALIBRECOMPTEUR = LeProgramme.LIBELLEEQUIPE);
            lesCompteurs.ForEach(t => t.BRANCHEMENT = LeProgramme.DATEPROGRAMME.Value.ToShortDateString());
            lesCompteurs.ForEach(t => t.FONCTIONNEMENT = LeProgramme.NUMPROGRAMME);
            
            Utility.ActionDirectOrientation<ServicePrintings.CsCanalisation, CsCanalisation>(lesCompteurs, null, SessionObject.CheminImpression, "SortieCompteur1", "Devis", true);
        }
        private void EditionProgramme(List<ObjELEMENTDEVIS > leDevis)
        {

            List<ObjELEMENTDEVIS> list = new List<ObjELEMENTDEVIS>();
            list.AddRange(leDevis.Where(t => !t.ISEXTENSION).ToList());

            list.ForEach(t => t.USERCREATION = LeProgramme.NUMPROGRAMME);
            Utility.ActionDirectOrientation<ServicePrintings.ObjELEMENTDEVIS, ObjELEMENTDEVIS>(list, null, SessionObject.CheminImpression, "FicheProgrammation", "Devis", true);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChargerDetailSortieCompteur(ServiceAccueil.CsProgarmmation leProgramme)
        {
            try
            {
                List<CsCanalisation> LstCannalisation = new List<CsCanalisation>();
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.GetDemandeByListeNumIdDemandeSortieCompteurCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    if (res.Result != null && res.Result.Count != 0)
                    {
                        LstCannalisation = res.Result;
                        dgDemande.ItemsSource = null;
                        dgDemande.ItemsSource = LstCannalisation;

                        this.txt_Equipe.Text = leProgramme.LIBELLEEQUIPE;
                        this.txt_DateProgramme.Text = leProgramme.DATEPROGRAMME.Value.ToShortDateString();
                    }
                    else
                        Message.Show("Aucune demande trouvé", "Information");
                };
                service1.GetDemandeByListeNumIdDemandeSortieCompteurAsync(leProgramme.NUMPROGRAMME, EtapeActuelle);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        List<ObjELEMENTDEVIS> Programme = new List<ObjELEMENTDEVIS>();
        private void ChargerDetailProgramme(ServiceAccueil.CsProgarmmation leProgramme)
        {
            try
            {
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ChargerListeDonneeProgramReeditionCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    if (res.Result != null && res.Result.Count != 0)
                    {
                        Programme = res.Result;
                        var lstElmtDevis = Programme.Select(p => new { p.NUMDEM, p.CLIENT, p.CODEMATERIELDEVIS, p.NOM, p.COMPTEUR }).Distinct().ToList();
                        List<CsCanalisation> lstProg = new List<CsCanalisation>();
                        foreach (var item in lstElmtDevis)
                        {
                            CsCanalisation Prog = new CsCanalisation();
                            Prog.NUMDEM = item.NUMDEM;
                            Prog.CLIENT = item.CLIENT;
                            Prog.NOMCLIENT = item.NOM;
                            Prog.LIBELLEMARQUE = item.CODEMATERIELDEVIS;
                            Prog.NUMERO = item.COMPTEUR;
                            lstProg.Add(Prog);
                        }
                        dgDemande.ItemsSource = null;
                        dgDemande.ItemsSource = lstProg;
                        this.txt_Equipe.Text = leProgramme.LIBELLEEQUIPE;
                        this.txt_DateProgramme.Text = leProgramme.DATEPROGRAMME.Value.ToShortDateString();
                        Programme.ForEach(t => t.NUMFOURNITURE = leProgramme.LIBELLEEQUIPE);
                        Programme.ForEach(t => t.NUMDEVIS = leProgramme.DATEPROGRAMME.Value.ToShortDateString());
                    }
                    else
                        Message.Show("Aucune demande trouvé", "Information");
                };
                service1.ChargerListeDonneeProgramReeditionAsync(leProgramme.NUMPROGRAMME, leProgramme.FK_IDEQUIPE.Value);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsCanalisation>;
            if (dg.SelectedItem != null)
            {
                CsCanalisation SelectedObject = (CsCanalisation)dg.SelectedItem;
                if (SelectedObject.IsSelect == false)
                {
                    SelectedObject.IsSelect = true;
                    SelectedObject.ETATDUCOMPTEUR = "Livré";
                }
                else
                {
                    SelectedObject.IsSelect = false;
                    SelectedObject.ETATDUCOMPTEUR = "";
                }
            }
        }

        private void btn_tout_Click(object sender, RoutedEventArgs e)
        {
            List<CsCanalisation> allObjects = this.dgDemande.ItemsSource as List<CsCanalisation>;
            if (allObjects != null && allObjects.Count != 0)
            {
                allObjects.ForEach(y => y.IsSelect = true);
                allObjects.ForEach(y => y.ETATDUCOMPTEUR = "Livré");
            }
        }

        private void btn_Rien_Click(object sender, RoutedEventArgs e)
        {
            List<CsCanalisation> allObjects = this.dgDemande.ItemsSource as List<CsCanalisation>;
            if (allObjects != null && allObjects.Count != 0)
            {
                allObjects.ForEach(y => y.IsSelect = false);
                allObjects.ForEach(y => y.ETATDUCOMPTEUR = "");
            }
        }

    }
}


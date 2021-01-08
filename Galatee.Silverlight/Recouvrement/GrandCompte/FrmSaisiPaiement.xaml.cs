﻿using Galatee.Silverlight.ServiceRecouvrement;
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

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmSaisiPaiement : ChildWindow
    {
        #region Variable

        List<ServiceRecouvrement.CsRegCli> LstCodeRegroupement = new List<ServiceRecouvrement.CsRegCli>(); 
        List<ServiceRecouvrement.CsAffectationGestionnaire> LstAffectation = new List<ServiceRecouvrement.CsAffectationGestionnaire>();
        List<ServiceRecouvrement.CsCampagneGc> LstCampagneGc = new List<ServiceRecouvrement.CsCampagneGc>();
        ServiceRecouvrement.CsCampagneGc CampagneGc = new ServiceRecouvrement.CsCampagneGc();
        List<ServiceRecouvrement.CsRegCli> LstCodeRegroupement_User=new List<ServiceRecouvrement.CsRegCli>();
        int IdCampagne = 0;
        int EtapeActuel;

        decimal MontantPayer = 0;
        #endregion

        #region Constructeur

        public FrmSaisiPaiement()
        {
            InitializeComponent();
            RemplirCodeRegroupement();
            RemplirAffectation();
        }
        public FrmSaisiPaiement(int IdCampagne)
        {
            InitializeComponent();
            this.IdCampagne=IdCampagne;
            RemplirCodeRegroupement();
            RemplirAffectation();
        }
        public FrmSaisiPaiement(List<int> ListIdCampagne, int EtapeActuel)
        {
            InitializeComponent();
            this.EtapeActuel = EtapeActuel;
            this.IdCampagne = ListIdCampagne.First();
            RemplirCodeRegroupement();
            RemplirAffectation();
        }
        #endregion

        #region Event Handler

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (dg_campagne.SelectedItem!=null)
	        {
		        int id=((CsCampagneGc)dg_campagne.SelectedItem).PK_ID;
                decimal  montant;
                decimal.TryParse(txt_montant.Text.Split('.')[0],out montant);

                SaisiPaiement(montant, id);
	        }
            
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        
        private void btn_rech_Click(object sender, RoutedEventArgs e)
        {
            if (cbo_regroupement.SelectedItem!=null)
            {
              var Regroupement=  ((ServiceRecouvrement.CsRegCli)cbo_regroupement.SelectedItem);
              //RetournCampagneByRegcli(Regroupement, txt_periode.Text);
            }
        }

        

        #endregion

        #region Services

        private void RemplirCodeRegroupement()
        {
            try
            {

                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.RetourneCodeRegroupementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    LstCodeRegroupement = args.Result;
                    RemplirCampagneById(this.IdCampagne);

                    return;
                };
                service.RetourneCodeRegroupementAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void RemplirAffectation()
        {
            try
            {
                if (LstAffectation.Count != 0)
                {
                    return;
                }
                else
                {
                    Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    service.RemplirAffectationCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstAffectation = args.Result;
                        RemplirCampagneById(this.IdCampagne);
                        return;
                    };
                    service.RemplirAffectationAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void RetournCampagneByRegcli(ServiceRecouvrement.CsRegCli Regroupement, string periode)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RetournCampagneByRegcliCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                    return;
                LstCampagneGc = args.Result;

                dg_campagne.ItemsSource = LstCampagneGc;
                return;
            };
            service.RetournCampagneByRegcliAsync(Regroupement, periode);
        }
        private void SaisiPaiement(decimal? Monant,int Id)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.SaisiPaiementCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                    return;
                Message.Show("Mise a jour efectué avec succes", "Resultat");
                List<int> Listid = new List<int>();
                Listid.Add(LstCampagneGc.First().PK_ID);
                EnvoyerDemandeEtapeSuivante(Listid);
                return;
            };
            service.SaisiPaiementAsync(Monant, Id);
        }
        private void RemplirCampagneById(int IdCampagne)
        {
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            service.RemplirCampagneByIdCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == null)
                    return;
                CampagneGc = args.Result;
               
                //txt_periode.Text = CampagneGc.PERIODE;
                ReLoadingGrid();
                LstCampagneGc.Clear();
                LstCampagneGc.Add(CampagneGc);
                List<CsDetailCampagneGc> ListeDetailCampagneAsuprimmer = new List<CsDetailCampagneGc>();
                MontantPayer = 0;
                foreach (var item in LstCampagneGc)
                {
                    foreach (var item_ in item.DETAILCAMPAGNEGC_)
                    {
                        item_.MONTANT_RESTANT = 0;
                        item_.MONTANT_REGLER = 0;
                        foreach (var mand in item.MANDATEMENTGC_)
                        {
                            var detailmand = mand.DETAILMANDATEMENTGC_.Where(dm => dm.CENTRE == item_.CENTRE && dm.CLIENT == item_.CLIENT && dm.ORDRE == item_.ORDRE && dm.NDOC == item_.NDOC);
                            if (detailmand != null)
                            {
                                item_.MONTANT_REGLER = item_.MONTANT_REGLER + detailmand.Sum(c => c.MONTANT);
                            }
                        }
                        MontantPayer = MontantPayer + item_.MONTANT_REGLER.Value;
                        item_.MONTANT_RESTANT = item_.MONTANT - item_.MONTANT_REGLER;
                        item_.MONTANT_VERSER = 0;
                    }
                }

                dg_campagne.ItemsSource = LstCampagneGc.Where(c => c.PK_ID == this.IdCampagne);
                dg_campagne.SelectedItem = LstCampagneGc[0];
                return;
            };
            service.RemplirCampagneByIdAsync(IdCampagne);
        }

        private void EnvoyerDemandeEtapeSuivante(List<int> Listid)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.Protocole(), Utility.EndPoint("Workflow"));

            clientWkf.ExecuterActionSurDemandeParPkIDLigneCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec de sortie materiel", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    return;
                }
                if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
                {
                    Message.ShowError("Echec de sortie materiel", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    return;
                }
                else
                {
                    //Message.ShowInformation("Sortie materiel éffectuée", Langue.lbl_Menu);

                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuel, ServiceWorkflow.CODEACTION.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }

        #endregion

        #region Methode

        private void ReLoadingGrid()
        {
            var UtilisateurSelect = UserConnecte.PK_ID;
            var Affectation = LstAffectation.Where(a => a.FK_IDADMUTILISATEUR == UtilisateurSelect && a.ISACTIVE == true);

            if (Affectation != null)
            {
                var ListIdRegcliCorrespondant = Affectation.Select(a => a.FK_IDREGLI);
                LstCodeRegroupement_User = LstCodeRegroupement.Where(r => ListIdRegcliCorrespondant.Contains(r.PK_ID)).ToList();

                cbo_regroupement.DisplayMemberPath = "NOM";
                cbo_regroupement.SelectedValuePath = "CODE";
                cbo_regroupement.ItemsSource = LstCodeRegroupement_User.Where(r=>r.PK_ID==CampagneGc.FK_IDREGCLI);
                cbo_regroupement.SelectedIndex = 0;
            }
        }

        #endregion

        private void dg_campagne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txt_montant.Text = MontantPayer.ToString();
        }
    }
}


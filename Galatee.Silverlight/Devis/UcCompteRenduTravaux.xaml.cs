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
using Galatee.Silverlight.ServiceAccueil ;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Devis
{
    public partial class UcCompteRenduTravaux : ChildWindow
    {
        private ObjDEVIS ObjetDevisSelectionne = null;
        private ObjDEVIS MyDevis = new ObjDEVIS();
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = new CsDemande();
        List<Galatee.Silverlight.ServiceAccueil.CsTypeBranchement> LstTypeBrt;
        int? CommuneDuPoste = null;

        List<Galatee.Silverlight.ServiceAccueil.CsQuartier> LstQuartierSite = new List<Galatee.Silverlight.ServiceAccueil.CsQuartier>();
        List<Galatee.Silverlight.ServiceAccueil.CsDepart> LstDepart = new List<Galatee.Silverlight.ServiceAccueil.CsDepart>();


        private void RemplirListeDesDiametresExistant(CsDemande laDemande)
        {

            try
            {
                if (SessionObject.LstReglageCompteur != null && SessionObject.LstReglageCompteur.Count != 0)
                {

                    if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceParReglageCompteur;
                        if (_lstPuissance != null && _lstPuissance.Count != 0)
                        {
                            List<ServiceAccueil.CsPuissance> lesPuissanceRegalage = _lstPuissance.Where(t => t.FK_IDPRODUIT == laDetailDemande.LaDemande.FK_IDPRODUIT && t.VALEUR == laDetailDemande.LaDemande.PUISSANCESOUSCRITE).ToList();
                            List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> lstReglageCompteur = SessionObject.LstReglageCompteur.Where(p => p.FK_IDPRODUIT == laDemande.LaDemande.FK_IDPRODUIT && lesPuissanceRegalage.Select(y => y.FK_IDREGLAGECOMPTEUR).Contains(p.PK_ID)).ToList();
                            Cbo_diametre.SelectedValuePath = "PK_ID";
                            Cbo_diametre.DisplayMemberPath = "LIBELLE";
                            Cbo_diametre.ItemsSource = null;
                            Cbo_diametre.ItemsSource = lstReglageCompteur;
                            if (lstReglageCompteur != null && lstReglageCompteur.Count == 1)
                            {
                                Cbo_diametre.SelectedItem = lstReglageCompteur.First();
                                Cbo_diametre.Tag = lstReglageCompteur.First();
                            }
                        }
                        return;
                    }
                    List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> listeReglageCompteurExistant = SessionObject.LstReglageCompteur;
                    List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> lstDiam = listeReglageCompteurExistant.Where(p => p.FK_IDPRODUIT == laDemande.LaDemande.FK_IDPRODUIT).ToList();
                    Cbo_diametre.SelectedValuePath = "PK_ID";
                    Cbo_diametre.DisplayMemberPath = "LIBELLE";
                    Cbo_diametre.ItemsSource = lstDiam;
                    if (lstDiam != null && lstDiam.Count == 1)
                    {
                        Cbo_diametre.SelectedItem = lstDiam.First();
                        Cbo_diametre.Tag = lstDiam.First();
                    }
                    if (laDemande != null && laDemande.LaDemande != null && !string.IsNullOrEmpty(laDemande.LaDemande.REGLAGECOMPTEUR))
                    {
                        Cbo_diametre.SelectedItem = lstDiam.FirstOrDefault(p => p.CODE == laDemande.LaDemande.REGLAGECOMPTEUR);
                        Cbo_diametre.Tag = lstDiam.FirstOrDefault(p => p.CODE == laDemande.LaDemande.REGLAGECOMPTEUR);
                    }
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerReglageCompteurCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    SessionObject.LstReglageCompteur = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> listeReglageCompteurExistant = SessionObject.LstReglageCompteur;
                    List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> lstDiam = listeReglageCompteurExistant.Where(p => p.FK_IDPRODUIT == laDemande.LaDemande.FK_IDPRODUIT).ToList();
                    Cbo_diametre.SelectedValuePath = "PK_ID";
                    Cbo_diametre.DisplayMemberPath = "LIBELLE";
                    Cbo_diametre.ItemsSource = lstDiam;
                    if (lstDiam != null && lstDiam.Count == 1)
                    {
                        Cbo_diametre.SelectedItem = lstDiam.First();
                        Cbo_diametre.Tag = lstDiam.First();
                    }
                    if (laDemande != null && laDemande.LaDemande != null && !string.IsNullOrEmpty(laDemande.LaDemande.REGLAGECOMPTEUR))
                    {
                        Cbo_diametre.SelectedItem = listeReglageCompteurExistant.FirstOrDefault(p => p.CODE == laDemande.LaDemande.REGLAGECOMPTEUR);
                        Cbo_diametre.Tag = listeReglageCompteurExistant.FirstOrDefault(p => p.CODE == laDemande.LaDemande.REGLAGECOMPTEUR);
                    }
                    return;
                };
                service.ChargerReglageCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception es)
            {

                MessageBox.Show(es.Message);
            }
        }
        private void RemplirPuissanceSouscrite(Galatee.Silverlight.ServiceAccueil.CsReglageCompteur leReglageCompteur)
        {
            try
            {
                if (SessionObject.LstPuissance != null && SessionObject.LstPuissanceParReglageCompteur.Count != 0)
                {

                    List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceParReglageCompteur;
                    if (_lstPuissance != null && _lstPuissance.Count != 0)
                    {
                        List<ServiceAccueil.CsPuissance> lesPuissance = _lstPuissance.Where(t => t.FK_IDPRODUIT == leReglageCompteur.FK_IDPRODUIT && t.FK_IDREGLAGECOMPTEUR == leReglageCompteur.PK_ID).ToList();
                        Cbo_Puissance.ItemsSource = null;
                        Cbo_Puissance.ItemsSource = lesPuissance;
                        Cbo_Puissance.DisplayMemberPath = "VALEUR";
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                    }
                    return;
                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerPuissanceReglageCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstPuissanceParReglageCompteur = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsPuissance> _lstPuissance = SessionObject.LstPuissanceParReglageCompteur;
                    if (_lstPuissance != null && _lstPuissance.Count != 0)
                    {

                        List<ServiceAccueil.CsPuissance> lesPuissance = _lstPuissance.Where(t => t.FK_IDPRODUIT == leReglageCompteur.FK_IDPRODUIT && t.FK_IDREGLAGECOMPTEUR == leReglageCompteur.PK_ID).ToList();
                        Cbo_Puissance.ItemsSource = null;
                        Cbo_Puissance.ItemsSource = lesPuissance;
                        Cbo_Puissance.DisplayMemberPath = "VALEUR";
                        if (lesPuissance != null && lesPuissance.Count == 1)
                        {
                            Cbo_Puissance.SelectedItem = lesPuissance.First();
                            Cbo_Puissance.Tag = lesPuissance.First();
                        }
                        return;
                    }
                };
                service.ChargerPuissanceReglageCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerDiametreBranchement(CsDemande laDemande)
        {
            try
            {
                if (SessionObject.LstTypeBranchement != null && SessionObject.LstTypeBranchement.Count != 0)
                {
                    LstTypeBrt = SessionObject.LstTypeBranchement.Where(p => p.PRODUIT == laDemande.LaDemande.PRODUIT).ToList();
                    if (LstTypeBrt != null && LstTypeBrt.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text))
                        {
                            Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _LeDiametre = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeBrt, this.Txt_TypeBrancehment.Text, "CODE");
                            if (_LeDiametre != null && !string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                                this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                        }
                    }

                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeBranchementCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    SessionObject.LstTypeBranchement = args.Result;
                    LstTypeBrt = SessionObject.LstTypeBranchement.Where(p => p.PRODUIT == laDemande.LaDemande.PRODUIT).ToList();
                    if (LstTypeBrt != null && LstTypeBrt.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text))
                        {
                            Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _LeDiametre = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneObjectFromList(LstTypeBrt, this.Txt_TypeBrancehment.Text, "CODE");
                            if (_LeDiametre != null && !string.IsNullOrEmpty(_LeDiametre.LIBELLE))
                                this.Txt_LibelleDiametre.Text = _LeDiametre.LIBELLE;
                        }
                    }
                };
                service.ChargerTypeBranchementAsync();
                service.CloseAsync();
            }
            catch (Exception es)
            {

                MessageBox.Show(es.Message);
            }

        }
        private void ChargerListeDesPostesSource()
        {
            if (SessionObject.LsDesPosteElectriquesSource != null && SessionObject.LsDesPosteElectriquesSource.Count != 0)
            {
                return;
            }
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerPosteSourceCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    SessionObject.LsDesPosteElectriquesSource = args.Result;
                }
            };
            service.ChargerPosteSourceAsync();
            service.CloseAsync();
        }
        private void RetourneListeDesDepartHta()
        {
            if (SessionObject.LsDesDepartHTA != null && SessionObject.LsDesDepartHTA.Count != 0)
            {
                return;
            }
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerDepartHTACompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    SessionObject.LsDesDepartHTA = args.Result;
                }
            };
            service.ChargerDepartHTAAsync();
            service.CloseAsync();
        }
        private void ChargerListeDesPostesTransformation()
        {
            if (SessionObject.LsDesPosteElectriquesTransformateur != null && SessionObject.LsDesPosteElectriquesTransformateur.Count != 0)
            {
                return;
            }
            Galatee.Silverlight.ServiceParametrage.ParametrageClient service = new ServiceParametrage.ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            service.SelectAllPosteTransformationCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    CsPosteTransformation poste;
                    SessionObject.LsDesPosteElectriquesTransformateur.Clear();
                    foreach (var item in args.Result)
                    {
                        poste = new CsPosteTransformation();
                        poste.CODE = item.CODE;
                        poste.CODEDEPARTHTA = item.CODEDEPARTHTA;
                        poste.DATECREATION = item.DATECREATION;
                        poste.DATEMODIFICATION = item.DATEMODIFICATION;
                        poste.FK_IDDEPARTHTA = item.FK_IDDEPARTHTA;
                        poste.LIBELLE = item.LIBELLE;
                        poste.LIBELLEDEPARTHTA = item.LIBELLEDEPARTHTA;
                        poste.OriginalCODE = item.OriginalCODE;
                        poste.PK_ID = item.PK_ID;
                        poste.USERCREATION = item.USERCREATION;
                        poste.USERMODIFICATION = item.USERMODIFICATION;
                        SessionObject.LsDesPosteElectriquesTransformateur.Add(poste);
                    }
                }
            };
            service.SelectAllPosteTransformationAsync();

            service.CloseAsync();
        }
        //private void RetourneListeDesDepartBT()
        //{
        //    if (SessionObject.LsDesDepartBT.Count != 0)
        //    {
        //        return;
        //    }
        //    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //    service.ChargerDepartBTCompleted += (s, args) =>
        //    {
        //        if (args != null && args.Cancelled)
        //            return;
        //        SessionObject.LsDesDepartBT = args.Result;
        //    };
        //    service.ChargerDepartBTAsync();
        //    service.CloseAsync();
        //}
        private void ChargeQuartier(CsDemande laDemande)
        {
            try
            {
                if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
                {
                    LstQuartierSite = SessionObject.LstQuartier.Where(t => t.COMMUNE == laDemande.Ag.COMMUNE).ToList();
                }
                else
                {
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerLesQartiersCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstQuartier = args.Result;
                        LstQuartierSite = SessionObject.LstQuartier;
                    };
                    service.ChargerLesQartiersAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void Txt_CodeDiametre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_TypeBrancehment.Text.Length == SessionObject.Enumere.TailleDiametreBranchement && (LstTypeBrt != null && LstTypeBrt.Count != 0))
                {
                    Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _leDiametre = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneObjectFromList<Galatee.Silverlight.ServiceAccueil.CsTypeBranchement>(LstTypeBrt, this.Txt_TypeBrancehment.Text, "CODE");
                    if (_leDiametre != null && !string.IsNullOrEmpty(_leDiametre.LIBELLE))
                    {
                        this.Txt_LibelleDiametre.Text = _leDiametre.LIBELLE;
                        this.Txt_TypeBrancehment.Tag = _leDiametre.PK_ID;

                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu, Galatee.Silverlight.Resources.Accueil.Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_TypeBrancehment.Focus();
                            this.Txt_TypeBrancehment.Text = string.Empty;
                            this.Txt_LibelleDiametre.Text = string.Empty;
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }

        }
        private void btn_diametre_Click(object sender, RoutedEventArgs e)
        {
            this.btn_diametre.IsEnabled = false;
            if (LstTypeBrt.Count != 0)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstTypeBrt);
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des type de branchement");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnDiametre);
                ctr.Show();
            }

        }
        void galatee_OkClickedBtnDiametre(object sender, EventArgs e)
        {

            try
            {
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.GetisOkClick)
                {
                    Galatee.Silverlight.ServiceAccueil.CsTypeBranchement _LeDiametre = (Galatee.Silverlight.ServiceAccueil.CsTypeBranchement)ctrs.MyObject;
                    this.Txt_TypeBrancehment.Text = _LeDiametre.CODE;
                    this.Txt_TypeBrancehment.Tag = _LeDiametre.PK_ID;
                }
                this.btn_diametre.IsEnabled = true;

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
            }
        }
        private void btn_QuartierPoste_Click_1(object sender, RoutedEventArgs e)
        {
            this.btn_QuartierPoste.IsEnabled = false;
            //if (LstQuartierSite != null && LstQuartierSite.Count != 0)
            //{
            if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
            {
                if (this.CommuneDuPoste != null)
                    LstQuartierSite = SessionObject.LstQuartier.Where(t => t.FK_IDCOMMUNE == this.CommuneDuPoste.Value).ToList();
                else
                    LstQuartierSite = SessionObject.LstQuartier;

                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstQuartierSite);
                Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                _LstColonneAffich.Add("CODE", "CODE");
                _LstColonneAffich.Add("LIBELLE", "QUARTIER");
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(_LstObj);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, Galatee.Silverlight.Resources.Accueil.Langue.lbl_ListeDiametre);
                ctrl.Closed += new EventHandler(galatee_OkClickedBtnQuartier);
                ctrl.Show();
            }
        }
        void galatee_OkClickedBtnQuartier(object sender, EventArgs e)
        {
            this.btn_QuartierPoste.IsEnabled = true;
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsQuartier _LeQuartier = (ServiceAccueil.CsQuartier)ctrs.MyObject;
                if (_LeQuartier != null)
                {
                    this.Txt_LibelleQuartier.Text = _LeQuartier.LIBELLE;
                    Txt_QuarteirPoste.Text = _LeQuartier.CODE;
                }
            }
        }

        private void Txt_QuartierPoste_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenereCodification();
        }
        private void Txt_SequenceNumPoste_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenereCodification();
        }
        private void Txt_Depart_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Txt_PosteTransformateur.Text = this.Txt_LibellePosteTransformateur.Text = string.Empty;
            GenereCodification();
        }
        private void Txt_NeoudFinal_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenereCodification();
        }

        private void btn_PosteSource_Click(object sender, RoutedEventArgs e)
        {
            this.btn_PosteSource.IsEnabled = false;
            if (SessionObject.LsDesPosteElectriquesSource.Count != 0)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesPosteElectriquesSource);
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des poste sources");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_PosteSource);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_PosteSource(object sender, EventArgs e)
        {
            this.btn_PosteSource.IsEnabled = true;
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsPosteSource _LePoste = (Galatee.Silverlight.ServiceAccueil.CsPosteSource)ctrs.MyObject;
                if (_LePoste != null)
                {
                    this.Txt_PosteSource.Text = _LePoste.CODE;
                    this.Txt_LibellePosteSource.Text = _LePoste.LIBELLE;
                    this.Txt_PosteSource.Tag = _LePoste.PK_ID;
                    this.CommuneDuPoste = _LePoste.FK_IDCOMMUNE;

                }
            }
        }

        private void btn_depart_Click(object sender, RoutedEventArgs e)
        {
            this.btn_departHta.IsEnabled = false;
            if (SessionObject.LsDesDepartHTA.Count != 0 && this.Txt_PosteSource.Tag != null)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesDepartHTA.Where(t => t.FK_IDPOSTESOURCE == (int)this.Txt_PosteSource.Tag).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des departs HTA");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_depart);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_depart(object sender, EventArgs e)
        {
            this.btn_departHta.IsEnabled = true;
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsDepart _LeDepart = (Galatee.Silverlight.ServiceAccueil.CsDepart)ctrs.MyObject;
                if (_LeDepart != null)
                {
                    this.Txt_DepartHTA.Text = _LeDepart.CODE;
                    this.Txt_LibelleDepartHTA.Text = _LeDepart.LIBELLE;
                    this.Txt_DepartHTA.Tag = _LeDepart.PK_ID;

                }
            }
        }

        private void btn_PosteTransformateur_Click_1(object sender, RoutedEventArgs e)
        {
            this.btn_PosteTransformateur.IsEnabled = true;
            if (SessionObject.LsDesPosteElectriquesTransformateur.Count != 0 && this.Txt_DepartHTA.Tag != null)
            {
                List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LsDesPosteElectriquesTransformateur.Where(t => t.FK_IDDEPARTHTA == (int)this.Txt_DepartHTA.Tag).ToList());
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "CODE", "LIBELLE", "Liste des postes transformateurs");
                ctr.Closed += new EventHandler(galatee_OkClickedbtn_PosteTransformateur);
                ctr.Show();
            }
        }
        void galatee_OkClickedbtn_PosteTransformateur(object sender, EventArgs e)
        {
            this.btn_PosteSource.IsEnabled = true;
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsPosteTransformation _LsPoste = (Galatee.Silverlight.ServiceAccueil.CsPosteTransformation)ctrs.MyObject;
                if (_LsPoste != null)
                {
                    this.Txt_PosteTransformateur.Text = _LsPoste.CODE;
                    this.Txt_LibellePosteTransformateur.Text = _LsPoste.LIBELLE;
                    this.Txt_PosteTransformateur.Tag = _LsPoste.PK_ID;

                }
            }
        }

        private void Cbo_Puissance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Chk_BranchementAvecExtension_Unchecked(object sender, RoutedEventArgs e)
        {
            this.labelDistanceExtension.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_Distance_Extension.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void Chk_BranchementAvecExtension_Checked(object sender, RoutedEventArgs e)
        {
            if (Chk_BranchementAvecExtension.IsChecked.Value)
            {
                this.Txt_Distance_Extension.Visibility = System.Windows.Visibility.Visible;
                this.labelDistanceExtension.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.Txt_Distance_Extension.Visibility = System.Windows.Visibility.Collapsed;
                this.labelDistanceExtension.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        private void Cbo_diametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Galatee.Silverlight.ServiceAccueil.CsReglageCompteur leReglage = (Galatee.Silverlight.ServiceAccueil.CsReglageCompteur)Cbo_diametre.SelectedItem;
            RemplirPuissanceSouscrite(leReglage);
            
        }

        void GenereCodification()
        {
            if (!string.IsNullOrEmpty(this.Txt_PosteSource.Text) &&
                !string.IsNullOrEmpty(this.Txt_DepartHTA.Text) &&
                !string.IsNullOrEmpty(this.Txt_QuarteirPoste.Text) &&
                !string.IsNullOrEmpty(this.Txt_PosteTransformateur.Text) &&
                !string.IsNullOrEmpty(this.Txt_DepartBt.Text) &&
                !string.IsNullOrEmpty(this.Txt_NeoudFinal.Text))
                this.Txt_AdresseElectrique.Text =
                    (this.Txt_PosteSource.Text + this.Txt_DepartHTA.Text + this.Txt_QuarteirPoste.Text +
                     this.Txt_PosteTransformateur.Text + this.Txt_DepartBt.Text +
                     this.Txt_NeoudFinal.Text);
            else
                this.Txt_AdresseElectrique.Text = string.Empty;
        }

        public UcCompteRenduTravaux()
        {
            InitializeComponent();
        }
        public UcCompteRenduTravaux(int idDevis)
        {
            InitializeComponent();
            ChargerTypeDocument();
            AfficherOuMasquer(tabItemDemandeur, false);
            AfficherOuMasquer(tabItemAppareils, false);
            AfficherOuMasquer(tabItemFournitures, false);
            AfficherOuMasquer(tabItemAbonnement, false);
            AfficherOuMasquer(tabItemMetre, false);
            AfficherOuMasquer(tabItemSuivie, false);
            AfficherCacherMetre(System.Windows.Visibility.Collapsed);
            ChargerListeDesPostesSource();
            RetourneListeDesDepartHta();
            ChargerListeDesPostesTransformation();
            //RetourneListeDesDepartBT();
            ChargeDetailDEvis(idDevis);

        }
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.GetDevisByNumIdDevisCompleted += (ssender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    string error = args.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                if (args.Result == null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                else
                {
                    laDetailDemande = args.Result;
                    #region DocumentScanne
                    if (laDetailDemande.ObjetScanne != null && laDetailDemande.ObjetScanne.Count != 0)
                    {
                        foreach (var item in laDetailDemande.ObjetScanne)
                        {
                            LstPiece.Add(item);
                            ObjetScanne.Add(item);
                        }
                        dgListePiece.ItemsSource = null ;
                        dgListePiece.ItemsSource = ObjetScanne;
                    }
                    #endregion

                    laDemandeSelect = laDetailDemande.LaDemande;
                    RenseignerInformationsDevis(laDetailDemande);
                    RenseignerInformationsDemandeDevis(laDetailDemande);
                    RenseignerInformationsAppareilsDevis(laDetailDemande);
                    RenseignerInformationsFournitureDevis(laDetailDemande);
                    RenseignerInformationsAbonnement(laDetailDemande);
                    RenseignerInformationsBrt(laDetailDemande);
                    AfficherOuMasquer(tabItemSuivie, true);

                    this.DtpDebutTravauxTrx.SelectedDate = System.DateTime.Today.Date ;
                    LayoutRoot.Cursor = Cursors.Arrow;
                    if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementEp && laDetailDemande.LaDemande.ISBONNEINITIATIVE )
                    {
                        ChargeQuartier(laDetailDemande);
                        RemplirListeDesDiametresExistant(laDetailDemande);
                        RenseignerInformationsDevis(laDetailDemande);
                        ChargerDiametreBranchement(laDetailDemande);
                        AfficherCacherMetre(System.Windows.Visibility.Visible );
                    }
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
            client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
        }
        private void AfficherCacherMetre(Visibility Visible)
        {
            Chk_BranchementAvecExtension.Visibility = Visible;

            Cbo_diametre.Visibility = Visible;
            label_PuissanceSous.Visibility = Visible;

            Cbo_Puissance.Visibility = Visible;
            labelReglage.Visibility = Visible;

            Txt_NbreDeFoyer.Visibility = Visible;
            label1_Foyer.Visibility = Visible;

            Txt_Distance_Extension.Visibility = Visible;
            labelDistanceExtension.Visibility = Visible;      

            Txt_Distance.Visibility = Visible;
            label1A.Visibility = Visible;

            Txt_PosteTransformateur.Visibility = Visible;
            btn_PosteSource.Visibility = Visible;
            Txt_LibellePosteSource.Visibility = Visible;
            lbl_Depart_Copy.Visibility = Visible;

            Txt_DepartHTA.Visibility = Visible;
            btn_departHta.Visibility = Visible;
            lbl_Depart.Visibility = Visible;
            Txt_LibelleDepartHTA.Visibility = Visible;

            Txt_QuarteirPoste.Visibility = Visible;
            lbl_QuartierDuPoste.Visibility = Visible;
            btn_QuartierPoste.Visibility = Visible;
            Txt_LibelleQuartierPoste.Visibility = Visible;

            Txt_DepartBt.Visibility = Visible;
            lbl_QuartierDuPoste_Copy1.Visibility = Visible ;

            Txt_AdresseElectrique.Visibility = Visible;

            TxtLatitude.Visibility = Visible;
            lbl_longitude.Visibility = Visible;

            lbl_diametre.Visibility = Visible;
            btn_diametre.Visibility = Visible;
            Txt_LibelleDiametre.Visibility = Visible;

            TxtLongitude.Visibility = Visible;
            lbl_latitude.Visibility = Visible;

            Txt_TypeBrancehment.Visibility = Visible;
            Txt_PosteSource.Visibility = Visible;
            btn_PosteTransformateur.Visibility = Visible;
            Txt_LibellePosteTransformateur.Visibility = Visible;

            Txt_PosteTransformateur.Visibility = Visible;
            lbl_QuartierDuPoste_Copy.Visibility = Visible;

            Txt_AdresseElectrique.Visibility = Visible;
            lbl_Codification.Visibility = Visible;

            Gbo_Branchement.Visibility = Visible;
            lbl_NoeudFinal.Visibility = Visible;
            Txt_NeoudFinal.Visibility = Visible;
        }
        private void GetBranchement()
        {
            try
            {
                if (laDetailDemande.Branchement == null)
                {
                    laDetailDemande.Branchement = new CsBrt();
                    laDetailDemande.Branchement.CENTRE = string.IsNullOrEmpty(laDetailDemande.LaDemande.CENTRE) ? null : laDetailDemande.LaDemande.CENTRE;
                    laDetailDemande.Branchement.CLIENT = string.IsNullOrEmpty(laDetailDemande.LaDemande.CLIENT) ? null : laDetailDemande.LaDemande.CLIENT;
                    laDetailDemande.Branchement.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                    laDetailDemande.Branchement.NUMDEM = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                    laDetailDemande.Branchement.PRODUIT = string.IsNullOrEmpty(laDetailDemande.LaDemande.PRODUIT) ? string.Empty : laDetailDemande.LaDemande.PRODUIT;
                    laDetailDemande.Branchement.FK_IDPRODUIT = laDetailDemande.LaDemande.FK_IDPRODUIT == null ? 0 : laDetailDemande.LaDemande.FK_IDPRODUIT.Value;
                    laDetailDemande.Branchement.DATECREATION = DateTime.Now;
                    laDetailDemande.Branchement.USERCREATION = UserConnecte.matricule;
                }
                laDetailDemande.LaDemande.ISEXTENSION = Chk_BranchementAvecExtension.IsChecked.Value;
                laDetailDemande.LaDemande.REGLAGECOMPTEUR = (Cbo_diametre.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsReglageCompteur).CODE;
                laDetailDemande.LaDemande.FK_IDREGLAGECOMPTEUR  = (Cbo_diametre.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsReglageCompteur).PK_ID ;
                laDetailDemande.LaDemande.PUISSANCESOUSCRITE = (Cbo_Puissance.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsPuissance).VALEUR;
                laDetailDemande.LaDemande.FK_IDPUISSANCESOUSCRITE = (Cbo_Puissance.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsPuissance).FK_IDPUISSANCE;

                if (!string.IsNullOrEmpty(this.Txt_NbreDeFoyer.Text))
                    this.laDetailDemande.LaDemande.NOMBREDEFOYER = int.Parse(this.Txt_NbreDeFoyer.Text);

                if (!string.IsNullOrEmpty(this.Txt_Distance_Extension.Text))
                    this.laDetailDemande.Branchement.LONGEXTENSION = decimal.Parse(this.Txt_Distance_Extension.Text);

                if (!string.IsNullOrEmpty(this.Txt_Distance.Text))
                    this.laDetailDemande.Branchement.LONGBRT = decimal.Parse(this.Txt_Distance.Text);

                if (!string.IsNullOrEmpty(this.Txt_PosteTransformateur.Text))
                    this.laDetailDemande.Branchement.CODEPOSTE = this.Txt_PosteTransformateur.Text;

                if (!string.IsNullOrEmpty(this.Txt_AdresseElectrique.Text))
                    this.laDetailDemande.Branchement.ADRESSERESEAU = this.Txt_AdresseElectrique.Text;


                this.laDetailDemande.Branchement.LATITUDE = TxtLatitude.Text;
                this.laDetailDemande.Branchement.LONGITUDE = TxtLongitude.Text;

                this.laDetailDemande.Branchement.ADRESSERESEAU = string.IsNullOrEmpty(this.Txt_AdresseElectrique.Text) ? null : this.Txt_AdresseElectrique.Text;
                this.laDetailDemande.Branchement.LONGITUDE = string.IsNullOrEmpty(this.TxtLongitude.Text) ? null : this.TxtLongitude.Text;
                this.laDetailDemande.Branchement.LATITUDE = string.IsNullOrEmpty(this.TxtLatitude.Text) ? null : this.TxtLatitude.Text;

                this.laDetailDemande.Branchement.FK_IDTYPEBRANCHEMENT = this.Txt_TypeBrancehment.Tag == null ? laDetailDemande.Branchement.FK_IDTYPEBRANCHEMENT : int.Parse(this.Txt_TypeBrancehment.Tag.ToString());
                this.laDetailDemande.Branchement.CODEBRT = string.IsNullOrEmpty(this.Txt_TypeBrancehment.Text) ? null : this.Txt_TypeBrancehment.Text;

                this.laDetailDemande.Branchement.FK_IDPOSTESOURCE = this.Txt_PosteSource.Tag == null ? laDetailDemande.Branchement.FK_IDPOSTESOURCE : (int)this.Txt_PosteSource.Tag;
                this.laDetailDemande.Branchement.FK_IDPOSTETRANSFORMATION = this.Txt_PosteTransformateur.Tag == null ? laDetailDemande.Branchement.FK_IDPOSTETRANSFORMATION : (int)this.Txt_PosteTransformateur.Tag;
                this.laDetailDemande.Branchement.DEPARTBT = this.Txt_DepartBt.Tag == null ? laDetailDemande.Branchement.DEPARTBT : this.Txt_DepartBt.Tag.ToString();
                this.laDetailDemande.Branchement.FK_IDQUARTIER = this.Txt_QuarteirPoste.Tag == null ? laDetailDemande.Branchement.FK_IDQUARTIER : (int)this.Txt_QuarteirPoste.Tag;
                this.laDetailDemande.Branchement.FK_IDDEPARTHTA = this.Txt_DepartHTA.Tag == null ? laDetailDemande.Branchement.FK_IDDEPARTHTA : (int)this.Txt_DepartHTA.Tag;
                this.laDetailDemande.Branchement.NEOUDFINAL = string.IsNullOrEmpty(this.Txt_NeoudFinal.Text) ? null : this.Txt_NeoudFinal.Text;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btn_transmetre.IsEnabled = false;
                EnregisterOuTransmetre(false);
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
        private void EnregisterOuTransmetre(bool isTransmetre)
        {
            if (laDetailDemande.TravauxDevis == null)
                laDetailDemande.TravauxDevis = new ObjTRAVAUXDEVIS();
            if (laDetailDemande.LaDemande.TYPEDEMANDE != SessionObject.Enumere.TransfertSiteNonMigre)
            {
                if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementMt)
                {
                    if (string.IsNullOrEmpty(this.Txt_NbrCableSection.Text))
                    {
                        Message.ShowWarning("Saisir le nombre de section de cable", "Demande");
                        return;
                    }
                    laDetailDemande.TravauxDevis.NBRCABLESECTION = this.Txt_NbrCableSection.Text;
                }
                laDetailDemande.TravauxDevis.FK_IDDEVIS = laDetailDemande.LaDemande.PK_ID;
                laDetailDemande.TravauxDevis.NUMDEVIS = laDetailDemande.LaDemande.NUMDEM;
                laDetailDemande.TravauxDevis.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
                laDetailDemande.TravauxDevis.ORDRE = int.Parse(laDetailDemande.LaDemande.ORDRE);
                laDetailDemande.TravauxDevis.PROCESVERBAL = this.Txt_CommentaireTrx.Text;
                laDetailDemande.TravauxDevis.DATEDEBUTTRVX = Convert.ToDateTime(this.DtpDebutTravauxTrx.SelectedDate.Value);
                laDetailDemande.TravauxDevis.DATEFINTRVX = Convert.ToDateTime(this.DtpDebutTravauxTrx.SelectedDate.Value);
                laDetailDemande.TravauxDevis.DATECREATION = System.DateTime.Today.Date;
                laDetailDemande.TravauxDevis.USERCREATION = UserConnecte.matricule;
                laDetailDemande.TravauxDevis.MATRICULECHEFEQUIPE = UserConnecte.matricule;
                laDetailDemande.TravauxDevis.MATRICULEREGLEMENT = UserConnecte.matricule;
                laDetailDemande.TravauxDevis.NOMCHEFEQUIPE = UserConnecte.nomUtilisateur;
                laDetailDemande.TravauxDevis.DATEREGLEMENT = System.DateTime.Today.Date;
                laDetailDemande.TravauxDevis.DATEPREVISIONNELLE = System.DateTime.Today.Date;

                if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementEp && laDetailDemande.LaDemande.ISBONNEINITIATIVE)
                    GetBranchement();

                //if (chk_TravauxValide.IsChecked == true)
                //    laDetailDemande.LaDemande.ISDEVISCOMPLEMENTAIRE = true;

                laDetailDemande.EltDevis = null;
                laDetailDemande.LstEvenement = null;
                laDetailDemande.LstCanalistion = null;
                laDetailDemande.LstCoutDemande = null;
                laDetailDemande.Ag = null;
                laDetailDemande.LeClient = null;
                laDetailDemande.Abonne = null;
                laDetailDemande.Branchement = null;

                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.ValiderDemandeCompleted += (ss, b) =>
                {
                    this.btn_transmetre.IsEnabled = true;
                    this.RejeterButton.IsEnabled = true;
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (isTransmetre == true)
                    {
                        List<string> codes = new List<string>();
                        codes.Add(laDetailDemande.InfoDemande.CODE);

                        //ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
                        //List<CsUtilisateur> leUser = new List<CsUtilisateur>();
                        //if (laDetailDemande.InfoDemande != null && laDetailDemande.InfoDemande.CODE != null)
                        //{
                        //    foreach (CsUtilisateur item in laDetailDemande.InfoDemande.UtilisateurEtapeSuivante)
                        //        leUser.Add(item);
                        //    Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", laDetailDemande.LaDemande.NUMDEM, laDetailDemande.LaDemande.LIBELLETYPEDEMANDE);
                        //}
                    }
                };
                clientDevis.ValiderDemandeAsync(laDetailDemande);
            }
            else
            {

                AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clientDevis.ClotureValiderDemandeCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (isTransmetre == true)
                    {
                        List<string> codes = new List<string>();
                        codes.Add(laDetailDemande.InfoDemande.CODE);

                        ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);
                        //List<CsUtilisateur> leUser = new List<CsUtilisateur>();
                        //if (laDetailDemande.InfoDemande != null && laDetailDemande.InfoDemande.CODE != null)
                        //{
                        //    foreach (CsUtilisateur item in laDetailDemande.InfoDemande.UtilisateurEtapeSuivante)
                        //        leUser.Add(item);
                        //    Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", laDetailDemande.LaDemande.NUMDEM, laDetailDemande.LaDemande.LIBELLETYPEDEMANDE);
                        //    EditerContrat(laDetailDemande);
                        //}
                    }
                };
                clientDevis.ClotureValiderDemandeAsync(laDetailDemande);
            }
        }
        private void EditerContrat(CsDemande laDemande)
        {
            CsContrat leContrat = new CsContrat();
            leContrat.AGENCE = laDemande.LaDemande.LIBELLECENTRE;
            leContrat.BOITEPOSTALE = laDemande.LeClient.BOITEPOSTAL;
            leContrat.CATEGORIE = laDemande.LeClient.CATEGORIE;
            leContrat.CENTRE = laDemande.LaDemande.CENTRE;
            leContrat.CLIENT = laDemande.LaDemande.CLIENT;
            leContrat.CODCONSOMATEUR = laDemande.LeClient.CODECONSO;
            leContrat.CODETARIF = laDemande.Abonne == null ? string.Empty : laDemande.Abonne.TYPETARIF;
            leContrat.COMMUNE = laDemande.Ag  == null ? string.Empty : laDemande.Ag.LIBELLECOMMUNE ;  
            leContrat.LIBELLEPRODUIT = laDemande.LaDemande.LIBELLEPRODUIT;
            leContrat.LONGUEURBRANCHEMENT = laDemande.Branchement.LONGBRT.ToString();

            leContrat.NATUREBRANCHEMENT = laDemande.Branchement  == null ? string.Empty : laDemande.Branchement.LIBELLETYPEBRANCHEMENT; 
            leContrat.NOMCLIENT = laDemande.LeClient.NOMABON;
            leContrat.NOMPROPRIETAIRE = laDemande.Ag.NOMP;
            leContrat.NUMDEMANDE = laDemande.LaDemande.NUMDEM;
            leContrat.ORDRE = laDemande.LaDemande.ORDRE;
            leContrat.PORTE = laDemande.Ag.PORTE;
            leContrat.PUISSANCESOUSCRITE = laDemande.Abonne.PUISSANCE.Value.ToString(SessionObject.FormatMontant);
            leContrat.QUARTIER = laDemande.Ag == null ? string.Empty : laDemande.Ag.LIBELLEQUARTIER ;
            leContrat.QUARTIERCLIENT = laDemande.Ag == null ? string.Empty : laDemande.Ag.LIBELLEQUARTIER;

            leContrat.FRAISTIMBRE = System.DateTime.Today.ToShortDateString();
            leContrat.TOTAL1 = laDemande.Ag.TOURNEE + "   " + laDemande.Ag.ORDTOUR;
            leContrat.NUMEROPIECE = laDemande.LeClient.LIBELLETYPEPIECE + "  N° " + laDemande.LeClient.NUMEROPIECEIDENTITE;
            leContrat.USAGE = laDemande.LeClient.LIBELLECODECONSO;


            leContrat.RUE = laDemande.Ag.LIBELLERUE;
            leContrat.RUECLIENT = laDemande.Ag.LIBELLERUE;
            leContrat.TELEPHONE = laDemande.LeClient.TELEPHONE;
            List<CsContrat> lstContrat = new List<CsContrat>();
            lstContrat.Add(leContrat);

            Utility.ActionDirectOrientation<ServicePrintings.CsContrat, CsContrat>(lstContrat, null, SessionObject.CheminImpression, "CreationClient", "Accueil", true);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        void Translate()
        {

        }


        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int resultLoding = 0;
        }

        private void AfficherOuMasquer(TabItem pTabItem, bool pValue)
        {
            try
            {
                pTabItem.Visibility = pValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RenseignerInformationsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                    ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID  == laDemande.LaDemande.FK_IDCENTRE );
                    Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE.ToString()) ? laDemande.LaDemande.ORDRE.ToString() : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLECENTRE) ? laDemande.LaDemande.LIBELLECENTRE : string.Empty;
                    Txt_LibelleSite.Text = !string.IsNullOrEmpty(leCentre.LIBELLESITE) ? leCentre.LIBELLESITE : string.Empty;
                    Txt_LibelleProduit.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLEPRODUIT) ? laDemande.LaDemande.LIBELLEPRODUIT : string.Empty;
                    Txt_Client.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CLIENT) ? laDemande.LaDemande.CLIENT : string.Empty;
                    Txt_LibelleTypeDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLETYPEDEMANDE) ? laDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;
                    txtPropriete.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;
                    Txt_Motif.Text = !string.IsNullOrEmpty(laDemande.LaDemande.MOTIF) ? laDemande.LaDemande.MOTIF : string.Empty;
                    this.Txt_EtapeCourante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Title = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Txt_EtapeSuivante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_SUIVANTE) ? laDemande.InfoDemande.ETAPE_SUIVANTE : string.Empty; 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsDemandeDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.LeClient != null && laDemande.Ag != null)
                {
                    Txt_NomClient.Text = !string.IsNullOrEmpty(laDemande.LeClient.NOMABON) ? laDemande.LeClient.NOMABON : string.Empty; 
                    //txt_Commune.Text = !string.IsNullOrEmpty(laDemande.Ag.COMMUNE) ? laDemande.Ag.COMMUNE : string.Empty;
                    txt_Quartier.Text = !string.IsNullOrEmpty(laDemande.Ag.QUARTIER) ? laDemande.Ag.QUARTIER : string.Empty;
                    txt_NumRue.Text = !string.IsNullOrEmpty(laDemande.Ag.RUE) ? laDemande.Ag.RUE : string.Empty;
                    txtAdresse.Text =laDemande.Branchement != null && !string.IsNullOrEmpty(laDemande.Branchement.ADRESSERESEAU) ? laDemande.Branchement.ADRESSERESEAU : string.Empty;
                    //this.TxtPoteau.Text = laDemande.Branchement.NBPOINT != null ? laDemande.Branchement.NBPOINT.ToString() : string.Empty;
                    txtNumeroPiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMEROPIECEIDENTITE) ? laDemande.LeClient.NUMEROPIECEIDENTITE : string.Empty;
                    txt_Telephone.Text = !string.IsNullOrEmpty(laDemande.LeClient.TELEPHONE) ? laDemande.LeClient.TELEPHONE : string.Empty;
                    txt_NumLot.Text = !string.IsNullOrEmpty(laDemande.LeClient.NUMPROPRIETE) ? laDemande.LeClient.NUMPROPRIETE : string.Empty;
                    Txt_LibelleCommune.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLECOMMUNE) ? laDemande.Ag.LIBELLECOMMUNE : string.Empty;
                    Txt_LibelleQuartier.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLEQUARTIER) ? laDemande.Ag.LIBELLEQUARTIER : string.Empty;
                    Txt_LibelleRue.Text = !string.IsNullOrEmpty(laDemande.Ag.LIBELLERUE) ? laDemande.Ag.LIBELLERUE : string.Empty;
                    //Txt_LibelleDiametre.Text = !string.IsNullOrEmpty(laDemande.Branchement.LIBELLETYPEBRANCHEMENT) ? laDemande.Branchement.LIBELLETYPEBRANCHEMENT : string.Empty;
                    Txt_LibelleCategorie.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLECATEGORIE) ? laDemande.LeClient.LIBELLECATEGORIE : string.Empty;
                    Txt_TypePiece.Text = !string.IsNullOrEmpty(laDemande.LeClient.LIBELLETYPEPIECE) ? laDemande.LeClient.LIBELLETYPEPIECE : string.Empty;
                    Txt_LibelleTournee.Text = !string.IsNullOrEmpty(laDemande.Ag.TOURNEE) ? laDemande.Ag.TOURNEE : string.Empty;

                    //TxtLongitude.Text = !string.IsNullOrEmpty(laDemande.Branchement.LONGITUDE) ? laDemande.Branchement.LONGITUDE : string.Empty;
                    //TxtLatitude.Text = !string.IsNullOrEmpty(laDemande.Branchement.LATITUDE) ? laDemande.Branchement.LATITUDE : string.Empty;

            
                    AfficherOuMasquer(tabItemDemandeur, true);
                }
                else
                    AfficherOuMasquer(tabItemDemandeur, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsAppareilsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.AppareilDevis != null && laDemande.AppareilDevis.Count > 0)
                {
                    dtgAppareils.ItemsSource = laDemande.AppareilDevis;
                    AfficherOuMasquer(tabItemAppareils, true);
                }
                else
                    AfficherOuMasquer(tabItemAppareils, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void RemplirListeMateriel(List<ObjELEMENTDEVIS> lstEltDevis)
        {
            List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();
            if (lstEltDevis.Count != 0)
            {
                List<ObjELEMENTDEVIS> lstFourExtension = new List<ObjELEMENTDEVIS>();
                List<ObjELEMENTDEVIS> lstFourBranchement = new List<ObjELEMENTDEVIS>();

                lstFourExtension = lstEltDevis.Where(t => t.ISEXTENSION == true).ToList();
                lstFourBranchement = lstEltDevis.Where(t => t.ISEXTENSION == false).ToList();

                ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
                leSeparateur.DESIGNATION = "----------------------------------";


                if (lstFourBranchement.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                    leResultatBranchanchement.DESIGNATION = "TOTAL BRANCHEMENT ";
                    leResultatBranchanchement.IsCOLORIE = true;
                    leResultatBranchanchement.MONTANTHT = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTHT);
                    leResultatBranchanchement.MONTANTTAXE = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTTAXE);
                    leResultatBranchanchement.MONTANTTTC = lstEltDevis.Where(t => t.ISEXTENSION == false).Sum(t => t.MONTANTTTC);

                    lstFourgenerale.AddRange(lstFourBranchement);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatBranchanchement);
                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        DESIGNATION = "    "
                    });

                }
                if (lstFourExtension.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatExtension = new ObjELEMENTDEVIS();
                    leResultatExtension.DESIGNATION = "TOTAL EXTENSION ";
                    leResultatExtension.IsCOLORIE = true;
                    leResultatExtension.MONTANTHT = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTHT);
                    leResultatExtension.MONTANTTAXE = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTTAXE);
                    leResultatExtension.MONTANTTTC = lstEltDevis.Where(t => t.ISEXTENSION == true).Sum(t => t.MONTANTTTC);

                    lstFourgenerale.AddRange(lstFourExtension);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatExtension);

                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        DESIGNATION = "    "
                    });
                }
                if (lstFourBranchement.Count != 0 || lstFourExtension.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
                    leResultatGeneral.DESIGNATION = "TOTAL GENERAL ";
                    leResultatGeneral.IsCOLORIE = true;
                    leResultatGeneral.MONTANTHT = lstEltDevis.Sum(t => t.MONTANTHT);
                    leResultatGeneral.MONTANTTAXE = lstEltDevis.Sum(t => t.MONTANTTAXE);
                    leResultatGeneral.MONTANTTTC = lstEltDevis.Sum(t => t.MONTANTTTC);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatGeneral);
                }
            }
            this.dataGridForniture.ItemsSource = null;
            this.dataGridForniture.ItemsSource = lstFourgenerale;

            this.Txt_TotalHt.Text = lstEltDevis.Where(p => p.QUANTITE != null && p.QUANTITE != 0).ToList().Sum(t => t.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
            this.Txt_TotalTtc.Text = lstEltDevis.Where(p => p.QUANTITE != null && p.QUANTITE != 0).ToList().Sum(t => t.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
            this.Txt_TotalTva.Text = lstEltDevis.Where(p => p.QUANTITE != null && p.QUANTITE != 0).ToList().Sum(t => t.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
        }
        private void RemplirListeMaterielMT(List<ObjELEMENTDEVIS> lstEltDevis)
        {
            List<ObjELEMENTDEVIS> lstFourgenerale = new List<ObjELEMENTDEVIS>();
            if (lstEltDevis.Count != 0)
            {
                List<ObjELEMENTDEVIS> lstFourHTA = new List<ObjELEMENTDEVIS>();
                List<ObjELEMENTDEVIS> lstFourBTA = new List<ObjELEMENTDEVIS>();
                List<ObjELEMENTDEVIS> lstComptage = new List<ObjELEMENTDEVIS>();

                lstFourHTA = lstEltDevis.Where(t => t.RUBRIQUE == "HTA").ToList();
                lstFourBTA = lstEltDevis.Where(t => t.RUBRIQUE == "BTA").ToList();
                lstComptage = lstEltDevis.Where(t => t.RUBRIQUE == "COM").ToList();

                ObjELEMENTDEVIS leSeparateur = new ObjELEMENTDEVIS();
                leSeparateur.DESIGNATION = "----------------------------------";
                leSeparateur.ISDEFAULT = true;


                if (lstFourHTA.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatBranchanchement = new ObjELEMENTDEVIS();
                    leResultatBranchanchement.DESIGNATION = "Sous Total Ligne HTA";
                    leResultatBranchanchement.IsCOLORIE = true;
                    leResultatBranchanchement.ISDEFAULT = true;
                    leResultatBranchanchement.MONTANTHT = lstEltDevis.Where(t => t.RUBRIQUE == "HTA").Sum(t => t.MONTANTHT);
                    leResultatBranchanchement.MONTANTTAXE = lstEltDevis.Where(t => t.RUBRIQUE == "HTA").Sum(t => t.MONTANTTAXE);
                    leResultatBranchanchement.MONTANTTTC = lstEltDevis.Where(t => t.RUBRIQUE == "HTA").Sum(t => t.MONTANTTTC);

                    lstFourgenerale.AddRange(lstFourHTA);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatBranchanchement);
                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        DESIGNATION = "    "
                    });

                }
                if (lstFourBTA.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatExtension = new ObjELEMENTDEVIS();
                    leResultatExtension.DESIGNATION = "Sous Total Ligne HTA/BT";
                    leResultatExtension.IsCOLORIE = true;
                    leResultatExtension.ISDEFAULT = true;
                    leResultatExtension.MONTANTHT = lstEltDevis.Where(t => t.RUBRIQUE == "BTA").Sum(t => t.MONTANTHT);
                    leResultatExtension.MONTANTTAXE = lstEltDevis.Where(t => t.RUBRIQUE == "BTA").Sum(t => t.MONTANTTAXE);
                    leResultatExtension.MONTANTTTC = lstEltDevis.Where(t => t.RUBRIQUE == "BTA").Sum(t => t.MONTANTTTC);

                    lstFourgenerale.AddRange(lstFourBTA);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatExtension);

                    lstFourgenerale.Add(new ObjELEMENTDEVIS()
                    {
                        DESIGNATION = "    "
                    });
                }

                if (lstFourBTA.Count != 0 || lstFourHTA.Count != 0)
                {
                    ObjELEMENTDEVIS leResultatGeneral = new ObjELEMENTDEVIS();
                    leResultatGeneral.DESIGNATION = "TOTAL GENERAL ";
                    leResultatGeneral.IsCOLORIE = true;
                    leResultatGeneral.ISDEFAULT = true;
                    leResultatGeneral.MONTANTHT = lstEltDevis.Sum(t => t.MONTANTHT);
                    leResultatGeneral.MONTANTTAXE = lstEltDevis.Sum(t => t.MONTANTTAXE);
                    leResultatGeneral.MONTANTTTC = lstEltDevis.Sum(t => t.MONTANTTTC);
                    lstFourgenerale.Add(leSeparateur);
                    lstFourgenerale.Add(leResultatGeneral);
                }
            }
            this.dataGridForniture.ItemsSource = null;
            this.dataGridForniture.ItemsSource = lstFourgenerale.ToList();
            if (dataGridForniture.ItemsSource != null)
            {
                Txt_TotalTtc.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTTC).Value.ToString(SessionObject.FormatMontant);
                Txt_TotalTva.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTTAXE).Value.ToString(SessionObject.FormatMontant);
                Txt_TotalHt.Text = ((List<ObjELEMENTDEVIS>)dataGridForniture.ItemsSource).ToList().Where(t => t.QUANTITE != null && t.QUANTITE != 0).Sum(y => y.MONTANTHT).Value.ToString(SessionObject.FormatMontant);
            }
        }
        private void dgMyDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dmdRow = e.Row.DataContext as ObjELEMENTDEVIS;
            if (dmdRow != null)
            {
                if (dmdRow.IsCOLORIE)
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Green);
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Bold;
                }
            }
        }
        private void RenseignerInformationsFournitureDevis( CsDemande lademande)
        {
            try
            {
                if (lademande != null && (lademande.EltDevis != null && lademande.EltDevis.Count > 0))
                {
                    if (lademande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                        RemplirListeMateriel(lademande.EltDevis);
                    else
                    {
                        if (lademande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement)
                            RemplirListeMaterielMT(lademande.EltDevis);
                        else
                            RemplirListeMateriel(lademande.EltDevis);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur au chargement des fournitures", "Validation devis");
            }
        }

     
        private void RenseignerInformationsAbonnement(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.Abonne != null && laDemande.Abonne != null)
                {
                        this.Txt_CodePuissanceUtilise.Text = laDetailDemande.Branchement.PUISSANCEINSTALLEE.ToString();
                        this.Txt_CodeTarif.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.TYPETARIF) ? laDetailDemande.Abonne.TYPETARIF : string.Empty;
                        this.Txt_CodePussanceSoucrite.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.PUISSANCE.Value.ToString()) ? laDetailDemande.Abonne.PUISSANCE.Value.ToString() : string.Empty;

                        if (laDetailDemande.Abonne.PUISSANCE != null)
                            this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(laDetailDemande.Abonne.PUISSANCE.ToString()).ToString("N2");
                        if (laDetailDemande.Abonne.PUISSANCEUTILISEE != null)
                            this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(laDetailDemande.Abonne.PUISSANCEUTILISEE.Value).ToString("N2");
                        this.Txt_CodeRistoune.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.RISTOURNE.ToString()) ? string.Empty : Convert.ToDecimal(laDetailDemande.Abonne.RISTOURNE.Value).ToString("N2");

                        this.Txt_CodeForfait.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.FORFAIT) ? string.Empty : laDetailDemande.Abonne.FORFAIT;
                        this.Txt_LibelleForfait.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEFORFAIT) ? string.Empty : laDetailDemande.Abonne.LIBELLEFORFAIT;

                        this.Txt_CodeTarif.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.TYPETARIF) ? string.Empty : laDetailDemande.Abonne.TYPETARIF;
                        this.Txt_LibelleTarif.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLETARIF) ? laDetailDemande.Abonne.LIBELLETARIF : string.Empty;

                        this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.PERFAC) ? string.Empty : laDetailDemande.Abonne.PERFAC;
                        this.Txt_LibelleFrequence.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEFREQUENCE) ? laDetailDemande.Abonne.LIBELLEFREQUENCE : string.Empty;

                        this.Txt_CodeMoisIndex.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.MOISREL) ? string.Empty : laDetailDemande.Abonne.MOISREL;
                        this.Txt_LibelleMoisIndex.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEMOISIND) ? laDetailDemande.Abonne.LIBELLEMOISIND : string.Empty;

                        this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.MOISFAC) ? string.Empty : laDetailDemande.Abonne.MOISFAC;
                        this.Txt_LibMoisFact.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEMOISFACT) ? laDetailDemande.Abonne.LIBELLEMOISFACT : string.Empty;

                        this.Txt_DateAbonnement.Text = (laDetailDemande.Abonne.DABONNEMENT == null) ? DateTime.Now.ToShortDateString() : Convert.ToDateTime(laDetailDemande.Abonne.DABONNEMENT.Value).ToShortDateString();
                    AfficherOuMasquer(tabItemAbonnement, true);
                }
                else
                    AfficherOuMasquer(tabItemAbonnement, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenseignerInformationsBrt(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null && laDemande.Branchement  != null  )
                {
                    if (laDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                    {
                        if (SessionObject.LstReglageCompteur != null && SessionObject.LstReglageCompteur.Count != 0 && laDemande.LaDemande.REGLAGECOMPTEUR != null )
                            this.Txt_TypedecompteurA.Text = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.CODE == laDemande.LaDemande.REGLAGECOMPTEUR).LIBELLE;
                    }

                    this.Txt_DistanceA.Text = laDetailDemande.Branchement.LONGBRT == null  ? string.Empty : laDetailDemande.Branchement.LONGBRT.ToString();
                    this.Txt_TourneeA.Text = string.IsNullOrEmpty(laDetailDemande.Ag.TOURNEE) ? string.Empty : laDetailDemande.Ag.TOURNEE;
                    this.TxtOrdreTourneeA.Text = string.IsNullOrEmpty(laDetailDemande.Ag.ORDTOUR) ? string.Empty : laDetailDemande.Ag.ORDTOUR;
                    this.TxtPuissanceA.Text = laDetailDemande.Branchement.PUISSANCEINSTALLEE == null ? string.Empty : laDetailDemande.Branchement.PUISSANCEINSTALLEE.Value.ToString(SessionObject.FormatMontant);
                    this.TxtLongitudeA.Text =string.IsNullOrEmpty(laDetailDemande.Branchement.LONGITUDE)? string.Empty : laDetailDemande.Branchement.LONGITUDE;
                    this.TxtLatitudeA.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LATITUDE) ? string.Empty : laDetailDemande.Branchement.LATITUDE;

                    dgListeFraixParicipation.ItemsSource = null;
                    dgListeFraixParicipation.ItemsSource = laDemande.LstFraixParticipation;

                    Txt_LibelleTypeBrtA.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLETYPEBRANCHEMENT) ? string.Empty : laDetailDemande.Branchement.LIBELLETYPEBRANCHEMENT;
                    Txt_LibellePosteSourceA.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEPOSTESOURCE) ? string.Empty : laDetailDemande.Branchement.LIBELLEPOSTESOURCE;
                    Txt_LibelleDepartHTAA.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEDEPARTHTA) ? string.Empty : laDetailDemande.Branchement.LIBELLEDEPARTHTA;
                    Txt_LibelleQuartierPosteA.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLEQUARTIER) ? string.Empty : laDetailDemande.Branchement.LIBELLEQUARTIER;
                    Txt_LibellePosteTransformateurA.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.LIBELLETRANSFORMATEUR) ? string.Empty : laDetailDemande.Branchement.LIBELLETRANSFORMATEUR;
                    Txt_LibelleDepartBtA.Text = string.IsNullOrEmpty(laDetailDemande.Branchement.DEPARTBT) ? string.Empty : laDetailDemande.Branchement.DEPARTBT;

                    AfficherOuMasquer(tabItemMetre, true);
                }
                else
                    AfficherOuMasquer(tabItemMetre, false);

            }
            catch (Exception ex)
            {
                Message.ShowInformation("Erreur au chargement du metré", "Demande");
            }
        }

        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        private UcImageScanne formScanne = null;
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        private byte[] image, imageFraix;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (cbo_typedoc.SelectedItem != null)
            {
                // Create an instance of the open file dialog box.
                var openDialog = new OpenFileDialog();
                // Set filter options and filter index.
                openDialog.Filter =
                    "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                openDialog.FilterIndex = 1;
                openDialog.Multiselect = false;
                // Call the ShowDialog method to show the dialog box.
                bool? userClickedOk = openDialog.ShowDialog();
                // Process input if the user clicked OK.
                if (userClickedOk == true)
                {
                    if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                    {
                        FileStream stream = openDialog.File.OpenRead();
                        var memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        image = memoryStream.GetBuffer();
                        formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                        formScanne.Closed += new EventHandler(GetInformationFromChildWindowImagePreuve);
                        formScanne.Show();
                    }
                }
            }
        }
        private List<ObjDOCUMENTSCANNE> ObjetScanne = new List<ObjDOCUMENTSCANNE>();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();

        private void GetInformationFromChildWindowImagePreuve(object sender, EventArgs e)
        {
            this.LstPiece.Add(new ObjDOCUMENTSCANNE { PK_ID = Guid.NewGuid(), NOMDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).LIBELLE, FK_IDTYPEDOCUMENT = ((CsTypeDOCUMENTSCANNE)cbo_typedoc.SelectedItem).PK_ID, CONTENU = image, DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule });
            this.dgListePiece.ItemsSource = this.LstPiece;
            ObjetScanne = this.LstPiece.ToList();
        }
        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
        }
        private void hyperlinkButtonPropScannee___Click(object sender, RoutedEventArgs e)
        {
            if (((HyperlinkButton)sender).Tag != null)
            {
                MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
                var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                ucImageScanne.Show();
            }
            else
            {
                Message.ShowInformation("Aucune image associer à cette ligne de fraix", "Information");
            }

        }
        private void ChargerTypeDocument()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeDocumentCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstTypeDocument.Add(item);
                    }
                    cbo_typedoc.ItemsSource = LstTypeDocument;
                    cbo_typedoc.DisplayMemberPath = "LIBELLE";
                    cbo_typedoc.SelectedValuePath = "PK_ID";
                };
                service.ChargerTypeDocumentAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    ObjDOCUMENTSCANNE Fraix = (ObjDOCUMENTSCANNE)dgListePiece.SelectedItem;
                    this.LstPiece.Remove(Fraix);
                    this.dgListePiece.ItemsSource = this.LstPiece;
                    ObjetScanne = this.LstPiece.ToList();

                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }

        private void btn_transmetre_Click(object sender, RoutedEventArgs e)
        {
            this.btn_transmetre.IsEnabled = false;
            this.RejeterButton.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            EnregisterOuTransmetre(true);
        }

        private void RejeterButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
        }

        private void Txt_Distance_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Txt_Distance_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_PosteSource_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Txt_QuarteirPoste.Text = this.Txt_LibelleQuartier.Text = string.Empty;
            this.Txt_DepartHTA.Text = this.Txt_LibelleDepartHTA.Text = string.Empty;
            this.Txt_PosteTransformateur.Text = this.Txt_LibellePosteTransformateur.Text = string.Empty;
            GenereCodification();

        }

        private void Txt_PosteTransformateur_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenereCodification();
        }

        private void Txt_DepartBT_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenereCodification();
        }

    
    }
}


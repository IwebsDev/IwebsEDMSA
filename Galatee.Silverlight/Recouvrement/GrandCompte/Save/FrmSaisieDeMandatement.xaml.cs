using Galatee.Silverlight.ServiceRecouvrement;
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
    public partial class FrmSaisieDeMandatement : ChildWindow
    {
        #region Variables

            int IdCampagne = 0;
            List<CsCampagneGc> ListCapagneGc = new List<CsCampagneGc>();
            List<CsMandatementGc> ListMandatementGc = new List<CsMandatementGc>();
 
        #endregion

        #region Constructeur

        public FrmSaisieDeMandatement(int IdCampagne)
            {
                InitializeComponent();
                this.IdCampagne = IdCampagne;

                RemplirCampagne(UserConnecte.matricule);
            }

        #endregion

        #region  Service

            private void RemplirCampagne(string Matricule)
            {
                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.RemplirCampagneCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == null)
                        return;
                    ListCapagneGc = args.Result;

                    List<CsDetailCampagneGc> ListeDetailCampagneAsuprimmer = new List<CsDetailCampagneGc>();
                    foreach (var item in ListCapagneGc)
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

                            item_.MONTANT_RESTANT = item_.MONTANT - item_.MONTANT_REGLER;

                            if (item_.MONTANT_RESTANT <= 0)
                            {
                                ListeDetailCampagneAsuprimmer.Add(item_);
                            }
                            item_.MONTANT_VERSER = 0;
                        }
                    }
                    foreach (var item in ListeDetailCampagneAsuprimmer)
                    {
                        ListCapagneGc[0].DETAILCAMPAGNEGC_.Remove(item);
                    }
                    if (ListCapagneGc[0].DETAILCAMPAGNEGC_.Count <= 0)
                    {
                        Message.Show("Cette campagne à été totalement réglé par mandatement,prette à etre transmise à l'étape suivante", "Information");

                        ListCapagneGc[0].DETAILCAMPAGNEGC_ = ListeDetailCampagneAsuprimmer;
                    }
                    dg_Campagne.ItemsSource = ListCapagneGc.Where(c => c.PK_ID == this.IdCampagne);
                    dg_Campagne.SelectedItem = ListCapagneGc[0];
                    return;
                };
                service.RemplirCampagneAsync(Matricule);
            }

        #endregion

        #region Event Handler

            private void OKButton_Click(object sender, RoutedEventArgs e)
            {
                List<CsMandatementGc> ListMandatementTosaveGc = new List<CsMandatementGc>();

                if (!string.IsNullOrWhiteSpace(txt_Numdeataire.Text))
                {
                    foreach (var item in ListMandatementGc)
                    {
                        if (string.IsNullOrWhiteSpace(item.NUMEROMANDATEMENT))
                        {
                            item.NUMEROMANDATEMENT = txt_Numdeataire.Text;
                        }
                        if (item.DETAILMANDATEMENTGC_.Count > 0)
                        {
                            ListMandatementTosaveGc.Add(item);
                        }
                    }
                    if (ListMandatementTosaveGc.Count > 0)
                    {
                        SaveMandatement(ListMandatementTosaveGc);
                    }
                    else
                    {
                        Message.ShowWarning("Veuillez selectionner au moin une facture pour valider le mandatement", "Information");
                    }
                    this.DialogResult = true;
                }
                else
                {
                    Message.ShowWarning("Veuillez saisir le numero du mandatement", "Information");
                }
            }
            private void CancelButton_Click(object sender, RoutedEventArgs e)
            {
                this.DialogResult = false;
            }

            private void CheckBox_Checked(object sender, RoutedEventArgs e)
            {
                MiseAJourMandatement(true);
            }

            private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
            {
                MiseAJourMandatement(false);
            }

            private void dg_facture_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
            {
                //CsDetailCampagneGc facture = (CsDetailCampagneGc)e.Row.DataContext;
                //MiseAJourMandatement();
            }
            private void Button_Click(object sender, RoutedEventArgs e)
            {

            }

            private void chbx_ToutValider_Unchecked(object sender, RoutedEventArgs e)
            {

            }
            private void chbx_ToutValider_Checked(object sender, RoutedEventArgs e)
            {

            }

            private void txt_MontantMandatement_TextChanged(object sender, TextChangedEventArgs e)
            {

            }

        #endregion

        #region Methodes 

            private void MiseAJourMandatement(bool Isvalide)
            {
                CsCampagneGc CampAnSelectioner = new CsCampagneGc();
                CampAnSelectioner = (CsCampagneGc)dg_Campagne.SelectedItem;
               
                var datasource = (List<CsDetailCampagneGc>)dg_facture.ItemsSource;

                UpdateDataSource(Isvalide, datasource);

                var MandatementGc = ListMandatementGc.FirstOrDefault(m => m.FK_IDCAMPAGNA == CampAnSelectioner.PK_ID);
                if (MandatementGc == null)
                {
                    var Mandatement = new CsMandatementGc { FK_IDCAMPAGNA = CampAnSelectioner.PK_ID, MONTANT = datasource.Where(f => f.IsMontantValide == true).Sum(dm => dm.MONTANT), DATECREATION = DateTime.Now, DATEMODIFICATION = DateTime.Now, USERCREATION = UserConnecte.matricule, USERMODIFICATION = UserConnecte.matricule, NUMEROMANDATEMENT = txt_Numdeataire.Text };
                    List<CsDetailMandatementGc> DETAILMANDATEMENTGC_ = new List<CsDetailMandatementGc>();
                    foreach (var item in datasource)
                    {
                        if (item.IsMontantValide == true)
                        {
                            CsDetailMandatementGc DetailMandatement = new CsDetailMandatementGc();

                            DetailMandatement.CENTRE = item.CENTRE;
                            DetailMandatement.CLIENT = item.CLIENT;
                            DetailMandatement.ORDRE = item.ORDRE;
                            DetailMandatement.DATECREATION = DateTime.Now;
                            DetailMandatement.DATEMODIFICATION = DateTime.Now;
                            DetailMandatement.FK_IDMANDATEMENT = Mandatement.PK_ID;
                            DetailMandatement.MONTANT = item.MONTANT;
                            DetailMandatement.MONTANT_REGLER = DetailMandatement.MONTANT;
                            DetailMandatement.MONTANT_VERSER = DetailMandatement.MONTANT;
                            DetailMandatement.MONTANT_RESTANT = 0;
                            DetailMandatement.NDOC = item.NDOC;
                            DetailMandatement.PERIODE = item.PERIODE;
                            DetailMandatement.STATUS = item.STATUS;
                            DetailMandatement.USERCREATION = UserConnecte.matricule;
                            DetailMandatement.USERMODIFICATION = UserConnecte.matricule;

                            DETAILMANDATEMENTGC_.Add(DetailMandatement);

                        }

                    }
                    Mandatement.DETAILMANDATEMENTGC_ = DETAILMANDATEMENTGC_;
                    ListMandatementGc.Add(Mandatement);
                }
                else
                {

                    MandatementGc.MONTANT = datasource.Where(f => f.IsMontantValide == true).Sum(dm => dm.MONTANT);
                    MandatementGc.NUMEROMANDATEMENT = txt_Numdeataire.Text;
                    List<CsDetailMandatementGc> DETAILMANDATEMENTGC_ = new List<CsDetailMandatementGc>();
                    foreach (var item in datasource)
                    {
                        if (item.IsMontantValide == true)
                        {
                            CsDetailMandatementGc DetailMandatement = new CsDetailMandatementGc();

                            DetailMandatement.CENTRE = item.CENTRE;
                            DetailMandatement.CLIENT = item.CLIENT;
                            DetailMandatement.ORDRE = item.ORDRE;
                            DetailMandatement.DATECREATION = DateTime.Now;
                            DetailMandatement.DATEMODIFICATION = DateTime.Now;
                            DetailMandatement.FK_IDMANDATEMENT = MandatementGc.PK_ID;
                            DetailMandatement.MONTANT = item.MONTANT; 
                            DetailMandatement.MONTANT_REGLER = DetailMandatement.MONTANT;
                            DetailMandatement.MONTANT_VERSER = DetailMandatement.MONTANT;
                            DetailMandatement.MONTANT_RESTANT = 0;
                            DetailMandatement.NDOC = item.NDOC;
                            DetailMandatement.PERIODE = item.PERIODE;
                            DetailMandatement.STATUS = item.STATUS;
                            DetailMandatement.USERCREATION = UserConnecte.matricule;
                            DetailMandatement.USERMODIFICATION = UserConnecte.matricule;

                            DETAILMANDATEMENTGC_.Add(DetailMandatement);

                        }

                    }
                    MandatementGc.DETAILMANDATEMENTGC_ = DETAILMANDATEMENTGC_;

                    var mand = ListMandatementGc.FirstOrDefault(m => m.PK_ID == MandatementGc.PK_ID);
                    var index = ListMandatementGc.IndexOf(mand);
                    ListMandatementGc[index] = MandatementGc;

                }

                txt_Montant_Mandatement.Text = datasource.Where(f => f.IsMontantValide == true).Sum(dm => dm.MONTANT).ToString();

            }

            private void UpdateDataSource(bool Isvalide, List<CsDetailCampagneGc> datasource)
            {
                var FactureSelectionne = (CsDetailCampagneGc)dg_facture.SelectedItem;

                var Index = datasource.IndexOf(FactureSelectionne);

                var FactureCorrespondante = datasource.ElementAt(Index);
                FactureCorrespondante.IsMontantValide = Isvalide;
                datasource[Index] = FactureCorrespondante;
            }



            private void SaveMandatement(List<CsMandatementGc> ListMandatementGc)
            {
                Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                service.SaveMandatementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == null)
                        return;
                    if (args.Result == true)
                    {
                        Message.Show("Mandatment enregistré avec succes", "Information");
                    }
                    else
                    {
                        Message.Show("Le Mandatment n'a pas été enregistré avec succes,veuillez refaire l'opration ", "Information");
                    }

                    return;
                };
                service.SaveMandatementAsync(ListMandatementGc);
            }

        #endregion

            private void txt_Numdeataire_TextChanged(object sender, TextChangedEventArgs e)
            {
                
            }

           




    }
}


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
using Galatee.Silverlight.ServiceCaisse;
using Galatee.Silverlight.Resources.Caisse ;
//using Galatee.Silverlight.serviceWeb;



namespace Galatee.Silverlight.Caisse
{
    public partial class frmEtatCaisse : ChildWindow
    {
        public frmEtatCaisse()
        {
            InitializeComponent();
        }

        string MatriculeCaisse = UserConnecte.matricule;
        string NumCaisse = string.Empty ;
        public frmEtatCaisse(string _Matricule)
        {
            InitializeComponent();
            try
            {
                MatriculeCaisse = _Matricule;
                translate();
            }
            catch (Exception EX)
            {
                Message.ShowError(EX, Langue.errorTitle);
            }
        }
        void translate()
        {
            try
            {
                this.lbl_numCaisse.Content = Langue.Caisse;
                dtg_EtatCaisse.Columns[0].Header = Langue.Mode_paiement;
                dtg_EtatCaisse.Columns[1].Header = Langue.Montant_Encaissé;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public frmEtatCaisse(CsHabilitationCaisse LaCaisse)
        {
            InitializeComponent();

            try
            {
                laCaisse = LaCaisse;
                translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }

        }
        public frmEtatCaisse(string _Matricule, string _NumCaisse)
        {
            InitializeComponent();

            try
            {
                MatriculeCaisse = _Matricule;
                NumCaisse = _NumCaisse;
                translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        CsHabilitationCaisse laCaisse = new CsHabilitationCaisse();

        private void frmEtatCaisse_Loaded(object sender, RoutedEventArgs e)
        {
            if (laCaisse == null || laCaisse.PK_ID == 0)
                ChargerHabilitationCaisse();
            else
                ChargerControl();
        }


        private void ChargerHabilitationCaisse()
        {
            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.RetouneLaCaisseCouranteAsync(UserConnecte.matricule);
            service.RetouneLaCaisseCouranteCompleted += (sender, es) =>
            {
                try
                {
                    if (es.Cancelled || es.Error != null)
                    {
                        Message.ShowError("Erreur d'invocation du service . Veuillez réssayer svp!", Galatee.Silverlight.Resources.Langue.errorTitle);
                        this.DialogResult = true;
                    }

                    if (es.Result == null)
                    {
                        Message.ShowError("Aucune données trouvées", Langue.errorTitle);
                        return;
                    }

                    laCaisse = es.Result;
                    SessionObject.LaCaisseCourante = laCaisse;
                    ChargerControl();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
            };

        }


        private void ChargerControl()
        {
            if (string.IsNullOrEmpty(laCaisse.CENTRE)) laCaisse = SessionObject.LaCaisseCourante;

            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
            service.RetourneEtatDeCaisseAsync(laCaisse);
            service.RetourneEtatDeCaisseCompleted += (sender, es) =>
            {
                try
                {
                    if (es.Cancelled || es.Error != null)
                    {
                        Message.ShowError("Erreur d'invocation du service . Veuillez réssayer svp!", Galatee.Silverlight.Resources.Langue.errorTitle);
                        this.DialogResult = true;
                    }

                    if (es.Result == null || es.Result.Count == 0)
                    {
                        Message.ShowError("Aucune données trouvées", Langue.errorTitle);
                        return;
                    }
                    this.Txt_NumCaissiere.Text = laCaisse.NUMCAISSE;
                    List<CsLclient> _LstReglement = new List<CsLclient>();
                    _LstReglement.AddRange(es.Result);

                    if (_LstReglement.Count > 0)
                    {
                        this.dtg_EtatCaisse.ItemsSource = null;
                        this.dtg_EtatCaisse.ItemsSource = _LstReglement;

                        decimal? TotalEncaisse = _LstReglement.FirstOrDefault(t => t.MODEREG  == "1") ==null  ? 0 : _LstReglement.FirstOrDefault(t => t.MODEREG  == "1").MONTANT;
                        decimal? TotalDecaisse = _LstReglement.FirstOrDefault(t => t.MODEREG == "A") == null ? 0 : _LstReglement.FirstOrDefault(t => t.MODEREG == "A").MONTANT;
                        this.Txt_TotalEnCaise.Text = (TotalEncaisse - TotalDecaisse).Value.ToString(SessionObject.FormatMontant ); 

                        //List<CsLclient> _LstDeReglementNonAnnule =_LstReglement.Where(t => string.IsNullOrEmpty(t.TOPANNUL) && t.DC == SessionObject.Enumere.Debit ).ToList();
                        //RetourneGridModePaiement(_LstDeReglementNonAnnule);
                        //this.Txt_MontantEncaisse.Text = _LstDeReglementNonAnnule.Sum(p => p.MONTANT).Value.ToString(SessionObject.FormatMontant);

                        //var NbreRecu = _LstDeReglementNonAnnule.Select(l => new { l.ACQUIT }).Distinct().ToList();
                        //var NbreRecuDecaisse = _LstReglement.Where(t => string.IsNullOrEmpty(t.TOPANNUL) && t.DC == SessionObject.Enumere.Credit).Select(l => new { l.ACQUIT }).Distinct().ToList();
                        //var NbreRecuAnnuler = _LstReglement.Where(t => !string.IsNullOrEmpty(t.TOPANNUL)).Select(l => new { l.ACQUIT }).Distinct().ToList();


                        //this.Txt_MontantDecaisse.Text = _LstReglement.Where(t => string.IsNullOrEmpty(t.TOPANNUL) && t.DC == SessionObject.Enumere.Credit ).Sum(t => t.MONTANT).Value.ToString(SessionObject.FormatMontant);
                        //this.Txt_FondDeCaisse.Text = (laCaisse.FONDCAISSE == null) ? "0" : laCaisse.FONDCAISSE.Value.ToString(SessionObject.FormatMontant);
                        //this.Txt_Annulation.Text = _LstReglement.Where(t => !string.IsNullOrEmpty(t.TOPANNUL)).Sum(p => p.MONTANT).Value.ToString(SessionObject.FormatMontant);
                        //this.Txt_nbreRecu.Text = (NbreRecu != null && NbreRecu.Count != 0) ? NbreRecu.Count().ToString() : "0";
                        //this.Txt_nbreRecuDecaisse.Text = (NbreRecuDecaisse != null && NbreRecuDecaisse.Count != 0) ? NbreRecuDecaisse.Count().ToString() : "0";
                        //this.Txt_nbreRecuAnnule.Text = (NbreRecuAnnuler != null && NbreRecuAnnuler.Count != 0) ? NbreRecuAnnuler.Count().ToString() : "0";

                        //decimal TotalEncaisse = string.IsNullOrEmpty(this.Txt_MontantEncaisse.Text)? 0 :Convert.ToDecimal(this.Txt_MontantEncaisse.Text);
                        //decimal TotalDecaisse = string.IsNullOrEmpty(this.Txt_MontantDecaisse.Text)? 0 :Convert.ToDecimal(this.Txt_MontantDecaisse.Text);
                        //this.txt_totalGeneral.Text = (TotalEncaisse - TotalDecaisse).ToString(); 
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, Langue.errorTitle);
                }
            };
        
        }

        private void RetourneGridModePaiement(List<CsLclient> _LstReglementClient)
        {
            try
            {

                List<CsLclient> lstClientReglement = new List<CsLclient>();
                if (_LstReglementClient != null && _LstReglementClient.Count > 0)
                {
                    var lstClientReglementDistnct = (from p in _LstReglementClient
                                                     group new { p } by new { p.MODEREG ,p.LIBELLEMODREG  } into pResult
                                                     select new
                                                     {
                                                         pResult.Key.MODEREG,
                                                         pResult.Key.LIBELLEMODREG,
                                                         MONTANT = (decimal?)pResult.Where(t => t.p.MODEREG == pResult.Key.MODEREG).Sum(o => o.p.MONTANT)
                                                     });
                    this.dtg_EtatCaisse.ItemsSource = null;
                    this.dtg_EtatCaisse.ItemsSource = lstClientReglementDistnct;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }



    }
}


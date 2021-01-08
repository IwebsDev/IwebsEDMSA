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
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.ServiceCaisse;
using Galatee.Silverlight.Resources.Caisse ;
using System.Windows.Browser;


namespace Galatee.Silverlight.Caisse
{
    public partial class FrmReversement : ChildWindow
    {
        public FrmReversement()
        {
            InitializeComponent();
        }
        CsHabilitationCaisse laCaisseARevereser;
        public FrmReversement(CsHabilitationCaisse _laCaisseARevereser)
        {
            InitializeComponent();
            laCaisseARevereser  = new CsHabilitationCaisse();
            laCaisseARevereser =_laCaisseARevereser;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<CsHabilitationCaisse> listeCaisseAReverser = new List<CsHabilitationCaisse>();
                CsHabilitationCaisse laCaisse = new CsHabilitationCaisse();
                if (laCaisseARevereser == null)
                {
                    SessionObject.LaCaisseCourante.IsCAISSECOURANTE = true;
                    listeCaisseAReverser.Add(SessionObject.LaCaisseCourante);
                }
                else
                    listeCaisseAReverser.Add(laCaisseARevereser);


                if (leCaisseHabilNonReverse != null && leCaisseHabilNonReverse.Count != 0)
                    listeCaisseAReverser.AddRange(leCaisseHabilNonReverse);

                ValidationReversementCaisse(listeCaisseAReverser);
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btn_Detail.IsEnabled = false;
                string warning = Langue.MsgFermetureCaisse1;
                if (laCaisseARevereser != null)
                {
                    this.Txt_NumeroCaisse.Text = string.Format("{0} ({1})", laCaisseARevereser.NUMCAISSE , laCaisseARevereser.NOMCAISSE) ;
                    this.Txt_NumeroCaisse.IsReadOnly = true;
                    this.dtp_DateCaisse.SelectedDate = laCaisseARevereser.DATE_DEBUT.Value;
                    this.dtp_DateCaisse.IsEnabled = false;
                    RetourneCaisseNonReverser(laCaisseARevereser);
                }
                else
                {
                this.Txt_NumeroCaisse.Text = UserConnecte.numcaisse + "  " + UserConnecte.nomUtilisateur;
                this.Txt_NumeroCaisse.IsReadOnly = true;
                this.dtp_DateCaisse.SelectedDate = SessionObject.LaCaisseCourante.DATE_DEBUT.Value;
                this.dtp_DateCaisse.IsEnabled = false;
                RetourneCaisseNonReverser(SessionObject.LaCaisseCourante);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
                
            }
        }

        private void ValidationReversementCaisse(List<CsHabilitationCaisse> leIdCaisse)
        {
            List<CsReversementCaisse> lstReversemnt = new List<CsReversementCaisse>();
            foreach (CsHabilitationCaisse item in leIdCaisse)
            {
                lstReversemnt.Add(new CsReversementCaisse()
                    {
                     FK_IDHABILITATIONCAISSE = item.PK_ID  ,
                     MONTANT = item.MONTANTENCAISSE ,
                     DATE = DateTime.Now,
                      IsCAISSECOURANTE = item.IsCAISSECOURANTE 
                    });
                
            }
            CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint(this));
            service.ReverserCaisseCompleted += (s, args) =>
            {
                if (args.Cancelled || args.Error != null || args.Result == null)
                    Message.ShowError("Un problème est survenu lors de la fermeture de caisse", "Information");
                else
                {
                    if (args.Result==true)
                    {
                        Message.ShowInformation(Langue.msgReversement,Langue.LibelleModule);
                    }
                }
                
            };
            service.ReverserCaisseAsync(lstReversemnt);
        }
        decimal initValue = 0;
        private void RetourneReversement(CsHabilitationCaisse laCaisse)
        {
            try
            {
                CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("Caisse"));
                service.RetourneHabileCaisseReversementCompleted += (s, args) =>
                {
                    CsHabilitationCaisse leCaisseHabil = new CsHabilitationCaisse();
                    if (args.Cancelled || args.Error != null )
                        Message.ShowError("Un problème est survenu lors de la fermeture de caisse", "Information");
                    leCaisseHabil = args.Result;


                    if (leCaisseHabil.MONTANTENCAISSE != null)
                        this.txt_MontantEncaisse.Text = leCaisseHabil.MONTANTENCAISSE.Value.ToString(SessionObject.FormatMontant);
                    else
                        this.txt_MontantEncaisse.Text = initValue.ToString(SessionObject.FormatMontant);

                    if (leCaisseHabil.MONTANTREVERSER != null)
                        this.Txt_MontantReverse.Text = leCaisseHabil.MONTANTREVERSER.Value.ToString(SessionObject.FormatMontant);
                    else
                        this.Txt_MontantReverse.Text = initValue.ToString(SessionObject.FormatMontant);


                    if (leCaisseHabil.MONTANTREVERSER != null)
                    {
                        this.Txt_MontantAReverser.Text = ((Convert.ToDecimal(this.txt_MontantEncaisse.Text) - Convert.ToDecimal(this.Txt_MontantReverse.Text)) + Convert.ToDecimal(this.Txt_MontantNonReverse.Text)).ToString(SessionObject.FormatMontant);
                         SessionObject.LaCaisseCourante.MONTANTENCAISSE =(Convert.ToDecimal(this.txt_MontantEncaisse.Text) - Convert.ToDecimal(this.Txt_MontantReverse.Text));
                    }
                    else
                    {
                        this.Txt_MontantAReverser.Text = (Convert.ToDecimal(this.txt_MontantEncaisse.Text) + Convert.ToDecimal(this.Txt_MontantNonReverse.Text)).ToString(SessionObject.FormatMontant);
                        SessionObject.LaCaisseCourante.MONTANTENCAISSE = Convert.ToDecimal(this.txt_MontantEncaisse.Text);
                    }
                };
                service.RetourneHabileCaisseReversementAsync(laCaisse);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        List<CsHabilitationCaisse> leCaisseHabilNonReverse = new List<CsHabilitationCaisse>();

        private void RetourneCaisseNonReverser(CsHabilitationCaisse laCaisse)
        {
            try
            {
                CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("Caisse"));
                service.RetourneHabileCaisseNonReversementAsync(laCaisse);
                service.RetourneHabileCaisseNonReversementCompleted += (s, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                        Message.ShowError("Un problème est survenu lors de la fermeture de caisse", "Information");
                    leCaisseHabilNonReverse = args.Result;
                    if (leCaisseHabilNonReverse != null && leCaisseHabilNonReverse.Count != 0)
                    {
                        this.btn_Detail.IsEnabled = true;
                        Txt_MontantNonReverse.Text = leCaisseHabilNonReverse.Sum(t => t.ECART).Value.ToString(SessionObject.FormatMontant);
                    }
                    else
                        this.Txt_MontantNonReverse.Text = initValue.ToString(SessionObject.FormatMontant);
                    RetourneReversement(laCaisse);
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btn_Detail_Click(object sender, RoutedEventArgs e)
        {
            FrmDetailCaisseNonReverser ctrl = new FrmDetailCaisseNonReverser(leCaisseHabilNonReverse);
            ctrl.Show();
        }
    }
}


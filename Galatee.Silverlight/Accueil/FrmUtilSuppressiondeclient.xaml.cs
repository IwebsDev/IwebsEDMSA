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
using Galatee.Silverlight.Resources.Accueil ;
//using Galatee.Silverlight.serviceWeb;


namespace Galatee.Silverlight.Accueil
{
    public partial class FrmUtilSuppressiondeclient : ChildWindow
    {

        List<CsPagisol> LstPagisol ;
        CsPagisol LePagisol;
        public FrmUtilSuppressiondeclient()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            LePagisol = new CsPagisol();
            LePagisol = (CsPagisol)this.dataGrid1.SelectedItem;
            try
            {
                string leMessage = "Voulez vous supprimer le client " + LePagisol.CLIENT   + 
                                    "\n\r  du batch " + LePagisol.LOTRI + "?";

                var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu , leMessage, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Warning );
                w.OnMessageBoxClosed += (_, result) =>
                {
                    if (w.Result == MessageBoxResult.OK)
                    {
                        ValiderSuppression(LePagisol);
                    }
                };
                w.Show();
            }
            catch (Exception ex)
            {
                Message.ShowWarning(ex.Message , Langue.lbl_Menu);

            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        public void RetournePagisol(string Centre, string lotri,string produit)
        {

            LstPagisol = new List<CsPagisol >();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil" ));
            service.RetournePagisolCompleted  += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result!= null && args.Result.Count != 0)
                LstPagisol = args.Result.Where(p => p.STATUT == SessionObject.Enumere.EvenementCree.ToString()).ToList();
                if (LstPagisol != null && LstPagisol.Count != 0)
                {
                    dataGrid1.ItemsSource = LstPagisol.OrderBy(p => p.PRODUIT);
                    dataGrid1.SelectedItem = LstPagisol[0];
                }
            };
            service.RetournePagisolAsync (Centre, lotri ,produit );
            service.CloseAsync();

        }
      
        public void ValiderSuppression(CsPagisol LaPagisol)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint(this));
            service.ValiderSuppressionCompleted   += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
            };
            service.ValiderSuppressionAsync(LaPagisol);
            service.CloseAsync();
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RetournePagisol(null, null, null);
        }
    }
}


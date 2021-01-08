using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Classes;
using Galatee.Silverlight.ServiceParametrage;
using Galatee.Silverlight.Resources.Parametrage;
namespace Galatee.Silverlight.Parametrage
{
    public partial class UcListMonnaie : ChildWindow, INotifyPropertyChanged
    {
        public CsMonnaie ObjetSelectionne { get; set; }

        ObservableCollection<CsMonnaie> donnesDatagrid = new ObservableCollection<CsMonnaie>();
        List<CsMonnaie> ListeMonnaie = null;
        public UcListMonnaie()
        {
            try
            {
                InitializeComponent();
                GetData();
                Translate();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Monnaie);
            }
        }

        private void Translate()
        {
            try
            {
                //dtgrdParametre.Columns[0].Header = Languages.Id;
                dtgrdParametre.Columns[1].Header = Languages.Libelle;
                Title = Languages.ColonneMonnaie;
                groupBox1.Header = Languages.ElementDansTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;
        
        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion

        public ObservableCollection<CsMonnaie> DonnesDatagrid
        {
            get { return donnesDatagrid; }
            set
            {
                if (value == donnesDatagrid)
                    return;
                donnesDatagrid = value;
                NotifyPropertyChanged("DonnesDatagrid");
            }
        }

        private List<CsLibelle> ConstruireListeSupport()
        {
            List<CsLibelle> ListeSupport = new List<CsLibelle>();

            CsLibelle Piece = new CsLibelle();
            Piece.CODE = "0";
            Piece.LIBELLE = Languages.Piece;
            ListeSupport.Add(Piece);

            CsLibelle Billet = new CsLibelle();
            Billet.CODE = "1";
            Billet.LIBELLE = Languages.Billet;
            ListeSupport.Add(Billet);

            return ListeSupport;
        }

        private void GetData()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllMonnaieCompleted += (ssender, args) =>
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError(error, Languages.TacheDevis);
                            return;
                        }
                        if (args.Result == null)
                        {
                            Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, Languages.ColonneMonnaie);
                            return;
                        }
                        DonnesDatagrid.Clear();
                        if (args.Result != null)
                        {
                            ListeMonnaie = args.Result;
                            foreach (var item in args.Result)
                            {
                                if (!string.IsNullOrEmpty(item.SUPPORT))
                                {
                                    var libelle = ConstruireListeSupport().FirstOrDefault(p => p.CODE == item.SUPPORT);
                                    if (libelle != null && !string.IsNullOrEmpty(libelle.LIBELLE))
                                        item.LIBELLESUPPORT = libelle.LIBELLE;
                                }
                                item.DISPLAYVALUE = item.VALEUR.Value.ToString("N2");
                                DonnesDatagrid.Add(item);
                            }
                        }
                        dtgrdParametre.ItemsSource = DonnesDatagrid;
                    };
                client.SelectAllMonnaieAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Supprimer()
        {
            try
            {
                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeASupprimer);
                if (dtgrdParametre.SelectedItem != null && dtgrdParametre.SelectedItems.Count > 0)
                {

                    var w = new MessageBoxControl.MessageBoxChildWindow(Languages.Monnaie, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                        {
                            var selected = dtgrdParametre.SelectedItem as CsMonnaie;
                            if (selected != null)
                            {
                                ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                 Utility.EndPoint("Parametrage"));
                                delete.DeleteMonnaieCompleted += (del, argDel) =>
                                {
                                    if (argDel.Cancelled || argDel.Error != null)
                                    {
                                        Message.ShowError(argDel.Error.Message, Languages.Monnaie);
                                        return;
                                    }
                                    if (argDel.Result == false)
                                    {
                                        Message.ShowError(Languages.ErreurSuppressionDonnees, Languages.Monnaie);
                                        return;
                                    }
                                    DonnesDatagrid.Remove(selected);
                                };
                                delete.DeleteMonnaieAsync(selected);
                            }
                        }
                        else
                        {
                            return;
                        }
                    };
                    w.Show();
                }
                else
                {
                    throw new Exception(Languages.SelectionnerUnElement);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Monnaie);
            }
        }
        
        private void Imprimer()
        {
            var dictionaryParam = new Dictionary<string, string>();
            try
            {
                if (ListeMonnaie.Count == 0)
                    throw new Exception(Languages.AucuneDonneeAImprimer);
                dictionaryParam.Add("RptParam_Libelle", Languages.Libelle.ToUpper());
                dictionaryParam.Add("RptParam_DateCreation", Languages.DateCreation);
                dictionaryParam.Add("RptParam_DateModification", Languages.DateModification);
                dictionaryParam.Add("RptParam_UserCreation", Languages.UserCreation);
                dictionaryParam.Add("RptParam_UserModification", Languages.UserModification);
                dictionaryParam.Add("Rpt_Support", Languages.ColonneSupport.ToUpper());
                dictionaryParam.Add("Rpt_Centre", Languages.Centre.ToUpper());
                dictionaryParam.Add("Rpt_Valeur", Languages.ColonneValeur.ToUpper());
                dictionaryParam.Add("RptParam_Title", Languages.ListeMonnaie.ToUpper());
                var w = new MessageBoxControl.MessageBoxChildWindow(Languages.Monnaie, Languages.QuestionImpressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                w.OnMessageBoxClosed += (_, result) =>
                {
                    if (w.Result == MessageBoxResult.OK)
                    {
                        var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                        string key = Utility.getKey();
                        service.EditerListeMonnaieCompleted += (snder, print) =>
                        {
                            if (print.Cancelled || print.Error != null)
                            {
                                Message.ShowError(print.Error.Message, Languages.Monnaie);
                                return;
                            }
                            if (!print.Result)
                            {
                                Message.ShowError(Languages.ErreurImpressionDonnees, Languages.Monnaie);
                                return;
                            }
                            Utility.ActionImpressionDirect(null, key, "Monnaie", "Parametrage");
                        };
                        service.EditerListeMonnaieAsync(key, dictionaryParam, ListeMonnaie);
                    }
                    else
                    {
                       return;
                    }
                };
                w.Show();
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItems != null)
                {
                    MenuContextuel.IsEnabled = (this.dtgrdParametre.SelectedItems.Count == 1);
                }
                MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Monnaie);
            }
        }

        #region "Gestion MenuContextuel"

        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UcMonnaie form = new UcMonnaie(null, SessionObject.ExecMode.Creation, dtgrdParametre);
                form.Closed += form_Closed;
                form.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Monnaie);
            }
        }

        void form_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (UcMonnaie)sender;
                if (form != null && form.DialogResult == true)
                {
                    GetData();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CsMonnaie)dtgrdParametre.SelectedItem;
                    UcMonnaie form = new UcMonnaie(objetselectionne, SessionObject.ExecMode.Modification, dtgrdParametre);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Monnaie);
            }
        }

        private void Editer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Imprimer();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Monnaie);
            }
        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    Supprimer();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Monnaie);
            }
        }

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CsMonnaie)dtgrdParametre.SelectedItem;
                    UcMonnaie form = new UcMonnaie(objetselectionne, SessionObject.ExecMode.Consultation, dtgrdParametre);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Monnaie);
            }
        }

        private void MenuContextuel_Opened(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuContextuelModifier.IsEnabled = MenuContextuelConsulter.IsEnabled = MenuContextuelSupprimer.IsEnabled = dtgrdParametre.SelectedItems.Count > 0;
                MenuContextuelModifier.UpdateLayout();
                MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Monnaie);
            }
        }
        #endregion
    }
}



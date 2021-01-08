using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Parametrage
{
    public partial class UcWKFSetUtilisateurRang : ChildWindow
    {

        ObservableCollection<CustomHabilitation> __donnesDatagrid = new ObservableCollection<CustomHabilitation>();
        private DataGrid dataGrid = null;
        CustomHabilitation _customHab;

        public UcWKFSetUtilisateurRang(CustomHabilitation customHab, DataGrid dtGrid)
        {
            try
            {
                if (dataGrid != null) __donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CustomHabilitation>;
                InitializeComponent();
                _customHab = customHab;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void AfficherDetail()
        {
            if (null != _customHab)
            {
                txtLogin.Text = _customHab.LOGINNAME;
                txtNomPrenoms.Text = _customHab.LIBELLE;
                txtLogin.IsEnabled = false;
                txtNomPrenoms.IsEnabled = false;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //On récupère le rang
            int rang = (int)NUPRang.Value;
            _customHab.RANG = rang;
            //on modifie xa dans le datagrid
            var Hab = __donnesDatagrid.Where(c => c.LOGINNAME == _customHab.LOGINNAME && c.LIBELLE == _customHab.LIBELLE)
                .FirstOrDefault();
            Hab.RANG = rang;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


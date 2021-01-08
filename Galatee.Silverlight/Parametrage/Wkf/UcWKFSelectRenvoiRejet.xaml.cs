using Galatee.Silverlight.ServiceParametrage;
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

namespace Galatee.Silverlight.Parametrage
{
    public partial class UcWKFSelectRenvoiRejet : ChildWindow
    {

        CsEtape OrigineEtape;
        List<CsEtape> _lsEtapes;

        public CsRenvoiRejet LeRenvoi { get; set; }

        public UcWKFSelectRenvoiRejet(CsEtape _etape, List<CsEtape> _lsEtape)
        {
            InitializeComponent();

            OrigineEtape = _etape;
            _lsEtapes = _lsEtape;

            cmbEtapeConditionVrai.DisplayMemberPath = "NOM";
            cmbEtapeConditionVrai.SelectedValuePath = "PK_ID";
            cmbEtapeConditionVrai.ItemsSource = _lsEtapes;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (null != cmbEtapeConditionVrai.SelectedValue)
            {
                LeRenvoi = new CsRenvoiRejet()
                {
                    PK_ID = Guid.NewGuid(),
                    FK_IDETAPE = int.Parse(cmbEtapeConditionVrai.SelectedValue.ToString()),
                    FK_IDETAPEACTUELLE = OrigineEtape.PK_ID
                };
            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


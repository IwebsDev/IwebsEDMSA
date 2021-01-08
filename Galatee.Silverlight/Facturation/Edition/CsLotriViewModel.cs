using Galatee.Silverlight.ServiceFacturation;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Facturation.Edition
{
    public class CsLotriViewModel
    {
        public CsLotri CsLotri { get; set; }
        public ObservableCollection<LstMoisCpt> moisComptable { get; set; }
    }

    public   class LstMoisCpt
    {
        public string LeMois { get; set; }
    }
}

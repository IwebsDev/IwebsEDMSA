using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;
using Galatee.Silverlight.ServiceCaisse;
using System.Collections.Generic;

namespace Galatee.Silverlight.Caisse
{
    public class TimeInformationViewModel : ViewModelBase
    {
        public TimeInformationViewModel()
        {
        }

        private List<CsRegCli > code;
        public List<CsRegCli> CurrentCode
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
                RaisePropertyChangedEvent("CurrentCode");
            }
        }
    }
}

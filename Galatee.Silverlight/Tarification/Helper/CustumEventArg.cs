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

namespace Galatee.Silverlight.Tarification.Helper
{
    public class CustumEventArg
    {
        
    }
        public class BaseEventArgs : EventArgs
        {
            public object Data { get; set; }
        }
        public class CustumEventArgs : BaseEventArgs
        {
            public object Bag { get; set; }
        }
}

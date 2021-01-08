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

namespace Galatee.WorkflowManager
{
    public delegate void ExecutionOkButtonEventHandler(object sender, ExecutionOkButtonEventArgs e);

    public class ExecutionOkButtonEventArgs: EventArgs
    {
        bool _isOk;
        string _pKIDLigne = string.Empty;

        public bool ExecutionOk { get { return _isOk; } }
        public string IDLigne { get { return _pKIDLigne; } }

        public ExecutionOkButtonEventArgs(bool isOk, string pkIDligne)
        {
            _isOk = isOk;
            _pKIDLigne = pkIDligne;
        }

    }
}

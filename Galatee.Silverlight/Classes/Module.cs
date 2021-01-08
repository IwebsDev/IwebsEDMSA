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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Galatee.Silverlight
{
    public class Module : INotifyPropertyChanged
    {
        public Module()
        {
            Programs = new ObservableCollection<Program>();
            CheckIHM = true;
        }
        private bool _Check;
        private bool _CheckIHM;
        private bool _CheckProfil;

        public bool CheckProfil
        {
            get { return _CheckProfil; }
            set { _CheckProfil = value; }
        }

        public bool CheckIHM
        {
            get { return _CheckIHM; }
            set { _CheckIHM = value; }
        }

        public bool Check
        {
            get { return _Check; }
             set
            {
                if (value == _Check)
                    if (!CheckIHM)
                    return;
                _Check = value;
                NotifyPropertyChanged("Check");
            }
        }

        public string Name { get; set; }
        public int? IdModule { get; set; }
        public ObservableCollection<Program> Programs { get; set; }

        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
                if(CheckIHM)
                foreach (Program program in Programs)
                    program.Check = this._Check;
            }
        }
        #endregion
    }
}

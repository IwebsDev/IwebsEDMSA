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
using System.ComponentModel;

namespace Galatee.Silverlight.Resources.Recouvrement
{
    public class ResourceLocaliser :INotifyPropertyChanged
    {
        private static Langue RecouvrementappStrings = new Langue();

        public Langue RecouvrementAppLangue
        {
            get { return RecouvrementappStrings; }
        }

        public void ChangeCulture(string culture)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);

            NotifyPropertyChanged("RecouvrementAppLangue");
        }

        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(name));
        }
        #endregion
    }
}

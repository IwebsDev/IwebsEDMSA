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

namespace Galatee.Silverlight.Resources.Accueil
{

 public class AcceuilResourceLocaliser :INotifyPropertyChanged
    {
     public  static Langue AccueilappStrings = new Langue();

     public Langue AccueilAppLangue
        {
            get { return AccueilappStrings; }
        }

        public void ChangeCulture(string culture)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);

            NotifyPropertyChanged("AccueilAppLangue");
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

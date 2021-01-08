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

namespace Galatee.Silverlight.Resources.Rpnt
{
    public class ResourceLocaliser
    {
        public ResourceLocaliser()
        { }

        private static Langue langueRpnt = new Langue();
        internal Langue LangueRpnt
        {
            get { return langueRpnt; }
        }
    }
}

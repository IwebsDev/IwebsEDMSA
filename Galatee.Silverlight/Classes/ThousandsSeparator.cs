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

namespace Galatee.Silverlight.Classes
{
    public static class ThousandsSeparator
    {
        //public ThousandsSeparator()
        //{
            
        //}
        public static string formate(decimal value)
        {
            var valuetoreturn = String.Format("{0:n}", value);
            return valuetoreturn;
        }
        public static string formate(decimal? value)
        {
            var valuetoreturn=String.Format("{0:n}", value);
            return valuetoreturn;
        }
    }
}

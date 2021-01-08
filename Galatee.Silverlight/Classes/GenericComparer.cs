using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Ecrit par WCO le 28/07/2015
    /// Cette classe va aider à faire un distinct sur des objets, selon la classe, grâce à la méthode
    /// Equals. 
    /// Elle va être utiliser dans la fonction Distinct() de LINQ
    /// </summary>
    public class GenericComparer<T> : IEqualityComparer<T> where T : class
    {
        private Func<T, object> _expr { get; set; }
        public GenericComparer(Func<T, object> expr)
        {
            this._expr = expr;
        }
        public bool Equals(T x, T y)
        {
            var first = _expr.Invoke(x);
            var sec = _expr.Invoke(y);
            if (first != null && first.Equals(sec))
                return true;
            else
                return false;
        }
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}

using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Microsoft.Maps.MapControl;

namespace Galatee.Silverlight.SIG
{
    public abstract class CsMapObject
    {
        public Location Coordinate {get; set;}
        //public StackPanel infosPanel;

        //public virtual StackPanel ShowPanel()
        //{ return null; }            
    }

    public class Abonne : CsMapObject
    {
        public string numeroClient { get; set; }
        public string nomAbonne { get; set; }
        public string centre { get; set; }
        public string telephone { get; set; }        
    }
}

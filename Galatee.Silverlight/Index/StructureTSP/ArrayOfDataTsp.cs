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
using System.Xml.Serialization;

namespace Galatee.Silverlight.Index.StructureTSP
{
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRoot("ArrayOfDataTsp")]
    public class ArrayOfDataTsp
    {
        [XmlElement("CsEvenementTsp")]
        public List<CsEvenementTsp> CsEvenementTsps { get; set; }
        [XmlElement("CsCasindTsp")]
        public List<CsCasindTsp> CsCasindTsps { get; set; }
        public ArrayOfDataTsp()
        {
            CsEvenementTsps = new List<CsEvenementTsp>();
            CsCasindTsps = new List<CsCasindTsp>();
        }

    }
}

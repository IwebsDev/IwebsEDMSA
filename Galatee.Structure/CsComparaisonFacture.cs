using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{


    [DataContract]
    public class CsComparaisonFacture : CsPrint
    {
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string CLIENT { get; set; }
        [DataMember] public string ORDRE { get; set; }
        [DataMember] public string COMPTEUR { get; set; }
        [DataMember] public string NOMABON { get; set; }
        [DataMember] public int POINT { get; set; }
        [DataMember] public string  PERIODE1 { get; set; }
        [DataMember] public DateTime  DATEEVT1 { get; set; }

        [DataMember] public string PERIODE2 { get; set; }
        [DataMember] public DateTime  DATEEVT2 { get; set; }

        [DataMember] public int CONSOFAC1 { get; set; }
        [DataMember] public int CONSOFAC2 { get; set; }
        [DataMember] public int ECARTCONSOFAC { get; set; }

        [DataMember] public decimal MONTANT1 { get; set; }
        [DataMember] public decimal MONTANT2 { get; set; }
        [DataMember] public decimal ECARTMONTANT { get; set; }

   



        




    }

}

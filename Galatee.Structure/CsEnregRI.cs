using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsEnregRI : CsPrint
    {
        [DataMember] public string   CENTRE { get; set; }
        [DataMember] public string   CLIENT { get; set; }
        [DataMember] public string ORDRE { get; set; }
        [DataMember] public int POINT { get; set; }
        [DataMember] public int? STATUS { get; set; }
        [DataMember] public string PRODUIT { get; set; }
        [DataMember] public string NOMABON { get; set; }
        [DataMember] public string ADRESSE { get; set; }
        [DataMember] public string LIBPRODUIT { get; set; }
        [DataMember] public string LIBRELEVEUR { get; set; }

        [DataMember] public string SEQUENCE { get; set; }
        [DataMember] public string COMPTEUR { get; set; }
        [DataMember] public string DIAMETRE { get; set; }

        [DataMember] public int? DANCIENINDEX { get; set; }
        [DataMember] public int? DNOUVELINDEX { get; set; }
        [DataMember] public string ANCIENCAS { get; set; }
        [DataMember] public DateTime? DATERELEVE { get; set; }
        [DataMember] public string REPCOMPT { get; set; }
        [DataMember] public string SOLDE { get; set; }
        [DataMember] public string PERIODE { get; set; }
        [DataMember] public string READER { get; set; }
        [DataMember] public string LOTRI { get; set; }
        [DataMember] public string TOURNEE { get; set; }
        [DataMember] public int? CONSO { get; set; }
        [DataMember] public string SOLDEFORMATE { get; set; }        

        [DataMember] public int? NUMEVENEMENT { get; set; }
        [DataMember] public int? FK_IDCANALISATION { get; set; }
        [DataMember] public int? FK_IDCENTRE { get; set; }
        [DataMember] public int? INDEXEVT { get; set; }

    }

}










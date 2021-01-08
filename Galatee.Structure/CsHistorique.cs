using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsHistorique
    {
        # region Generale
             
            [DataMember] public int PK_ID { get; set; }
            [DataMember] public string CENTRE { get; set; }
            [DataMember] public string CLIENT { get; set; }
            [DataMember] public string PRODUIT { get; set; }
            [DataMember] public int POINT { get; set; }
            [DataMember] public string ORDRE { get; set; }
            [DataMember] public string PERIODE { get; set; }
            [DataMember] public string CAS { get; set; }
            [DataMember] public Nullable<int> CONSO { get; set; }
            [DataMember] public Nullable<int> CONSOFAC { get; set; }
            [DataMember] public Nullable<int> NBREJOURFACTURE { get; set; }
            [DataMember] public Nullable<bool> ESTCONSORELEVEE { get; set; }
            [DataMember] public System.DateTime DATECREATION { get; set; }
            [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
            [DataMember] public string USERCREATION { get; set; }
            [DataMember] public string USERMODIFICATION { get; set; }
            [DataMember] public int FK_IDCENTRE { get; set; }
            [DataMember] public int FK_IDPRODUIT { get; set; }
            [DataMember] public int FK_IDABON { get; set; }
            [DataMember] public int FK_IDEVENEMENT { get; set; }
         #endregion
            [DataMember] public int NbreDePeriode { get; set; }

    }

}










using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsReglageCompteur : CsPrint
    {
       [DataMember] public string CODE { get; set; }
       [DataMember] public string LIBELLE { get; set; }
       [DataMember] public Nullable<int> REGLAGEMINI { get; set; }
       [DataMember] public Nullable<int> REGLAGEMAXI { get; set; }
       [DataMember] public Nullable<int> REGLAGE { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public int FK_IDPRODUIT { get; set; }
       [DataMember] public Nullable<int> FK_IDPHASE { get; set; }
       [DataMember] public Nullable<int> FK_IDCALIBRECOMPTEUR { get; set; }
       [DataMember] public bool  IsRecommender { get; set; }
    

       [DataMember] public string LIBELLEPRODUIT { get; set; }
       [DataMember] public string CODEPUISSANCE { get; set; }
       [DataMember] public string CODEPRODUIT { get; set; }

    

    }
}










using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsCompteur
    {

       [DataMember] public string CODESITE { get; set; }
       [DataMember] public string CODECENTRE { get; set; }
       [DataMember] public string CODEPRODUIT { get; set; }
       [DataMember] public string NUMERO { get; set; }
       [DataMember] public string TYPECOMPTEUR { get; set; }
       [DataMember] public string TYPECOMPTAGE { get; set; }
       [DataMember] public string CALIBRECOMPTEUR { get; set; }
       [DataMember] public string MARQUE { get; set; }
       [DataMember] public Nullable<decimal> COEFLECT { get; set; }
       [DataMember] public Nullable<int> COEFCOMPTAGE { get; set; }
       [DataMember] public Nullable<byte> CADRAN { get; set; }
       [DataMember] public string ANNEEFAB { get; set; }
       [DataMember] public string ETAT { get; set; }
       [DataMember] public string FONCTIONNEMENT { get; set; }
       [DataMember] public string PLOMBAGE { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public Nullable<int> FK_IDTYPECOMPTAGE { get; set; }
       [DataMember] public int FK_IDTYPECOMPTEUR { get; set; }
       [DataMember] public int FK_IDPRODUIT { get; set; }
       [DataMember] public int FK_IDMARQUECOMPTEUR { get; set; }
       [DataMember] public Nullable<int> FK_IDCALIBRECOMPTEUR { get; set; }
       [DataMember] public int FK_IDETATCOMPTEUR { get; set; }
       [DataMember] public Nullable<int> FK_IDCOMPTEUR { get; set; }
       [DataMember] public int FK_IDDEMANDE { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public int FK_IDSTATUTCOMPTEUR { get; set; }
       [DataMember] public string STATUT { get; set; }
       [DataMember] public DateTime  MISEENSERVICE { get; set; }

        
  
       [DataMember] public string LIBELLETYPE { get; set; }
       [DataMember] public string LIBELLECALIBRE { get; set; }
       [DataMember] public string LIBELLEMARQUE { get; set; }
    
    }

}










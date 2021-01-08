using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsLotCompteClient : CsPrint
    {

       [DataMember]public string ORIGINE { get; set; }
       [DataMember]public string NUMEROLOT { get; set; }
       [DataMember]public string MOISCOMPTABLE { get; set; }
       [DataMember]public string TYPELOT { get; set; }
       [DataMember]public string PARAMETRE { get; set; }
       [DataMember]public int IDLOT { get; set; }
       [DataMember]public string CAISSE { get; set; }
       [DataMember]public Nullable<System.DateTime> DATEDECREATION { get; set; }
       [DataMember]public string STATUS { get; set; }
       [DataMember]public Nullable<decimal> MONTANT { get; set; }
       [DataMember]public string REFERENCE { get; set; }
       [DataMember]public System.DateTime DATECREATION { get; set; }
       [DataMember]public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember]public string MATRICULE { get; set; }
       [DataMember]public string USERCREATION { get; set; }
       [DataMember]public string USERMODIFICATION { get; set; }

       [DataMember]public string TOP1 { get; set; }
       [DataMember]public int FK_IDLIBELLETOP { get; set; }
       [DataMember]public string DC { get; set; }

       [DataMember]public int PK_ID { get; set; }
       [DataMember]public int FK_IDMATRICULE { get; set; }
        
    }
}










using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsEcritureComptable:CsPrint
    {
      [DataMember] public int PK_ID { get; set; }
      [DataMember] public string SITE { get; set; }
      [DataMember] public string CENTRE { get; set; }
      [DataMember] public string CAISSE { get; set; }
      [DataMember] public string DC { get; set; }
      [DataMember] public int FK_IDOPERATION { get; set; }
        
      [DataMember] public string INITIAL { get; set; }
      [DataMember] public string CODEINTERFACE { get; set; }
      [DataMember] public string NEGATIVE { get; set; }
      [DataMember] public string PROVENACE { get; set; }
      [DataMember] public string DESCRIPSTIONOPERATION { get; set; }
      [DataMember] public string DATEOPERATION { get; set; }
      [DataMember] public string DEVISE { get; set; }
      [DataMember] public string DATEGENERATION { get; set; }
      [DataMember] public string ZERO { get; set; }
      [DataMember] public string ALPHA { get; set; }
      [DataMember] public string SOCIETE { get; set; }
      [DataMember] public string ACTIVITE { get; set; }
      [DataMember] public string COMPTE { get; set; }
      [DataMember] public string CENTREIMPUTATION { get; set; }
      [DataMember] public string FILIERE { get; set; }
      [DataMember] public string SOUSCOMPTE { get; set; }
      [DataMember] public string LOCALISATION { get; set; }
      [DataMember] public string NATUREIMMO { get; set; }
      [DataMember] public string LIBRE { get; set; }
      [DataMember] public Nullable<decimal> MONTANT { get; set; }
      [DataMember] public Nullable<decimal> DEBIT { get; set; }
      [DataMember] public Nullable<decimal> CREDIT { get; set; }
      [DataMember] public Nullable<decimal> DEBIT1 { get; set; }
      [DataMember] public Nullable<decimal> CREDIT1 { get; set; }
      [DataMember] public string LIBELLEOPERATION { get; set; }
      [DataMember] public string NUMPIECE { get; set; }
      [DataMember] public string NO { get; set; }
      [DataMember] public string DESCRIPTIONLIGNE { get; set; }
      [DataMember] public string USERCREATION { get; set; }
      [DataMember] public System.DateTime DATECREATION { get; set; }
      [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
      [DataMember] public string USERMODIFICATION { get; set; }
      [DataMember] public bool IsGenere { get; set; }

      [DataMember] public string NUMEROMANDATEMENT { get; set; }
    }
}

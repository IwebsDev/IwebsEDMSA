using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
   public class CsCompteSpecifique
    {
       [DataMember] public string SOCIETE { get; set; }
       [DataMember] public string ACTIVITE { get; set; }
       [DataMember] public string OPERATION { get; set; }
       [DataMember] public string COMPTE { get; set; }
       [DataMember] public string CENTREIMPUTATION { get; set; }
       [DataMember] public string FILIERE { get; set; }
       [DataMember] public string SOUSCOMPTE { get; set; }
       [DataMember] public string LOC { get; set; }
       [DataMember] public string NATIMMO { get; set; }
       [DataMember] public string LIBRE { get; set; }
       [DataMember] public string LIBELLE { get; set; }
       [DataMember] public Nullable<int> FK_IDOPERATIONCOMPTA { get; set; }
       [DataMember] public Nullable<int> FK_IDTYPE_COMPTE { get; set; }
       [DataMember] public string DC { get; set; }
       [DataMember] public string COPERASSOCIE { get; set; }
       [DataMember] public string VALEURFILTRE { get; set; }
       [DataMember] public string VALEURFILTRE1 { get; set; }
       [DataMember] public string VALEURFILTRE2 { get; set; }
       [DataMember] public string VALEURMONTANT { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }

       [DataMember] public List<string> LSTVALEURFILTRE { get; set; }
       [DataMember] public List<string> LSTVALEURFILTRE1 { get; set; }
       [DataMember] public List<string> LSTVALEURFILTRE2 { get; set; }
        

    }
}

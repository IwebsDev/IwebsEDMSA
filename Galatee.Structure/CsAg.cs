using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsAg
    {
       [DataMember] public string NUMDEM { get; set; }
      [DataMember] public string CENTRE { get; set; }
      [DataMember] public string CLIENT { get; set; }
      [DataMember] public string NOMP { get; set; }
      [DataMember] public string COMMUNE { get; set; }
      [DataMember] public string QUARTIER { get; set; }
      [DataMember] public string RUE { get; set; }
      [DataMember] public string ETAGE { get; set; }
      [DataMember] public string PORTE { get; set; }
      [DataMember] public string CADR { get; set; }
      [DataMember] public string REGROU { get; set; }
      [DataMember] public string CPARC { get; set; }
      [DataMember] public Nullable<System.DateTime> DMAJ { get; set; }
      [DataMember] public string TOURNEE { get; set; }
      [DataMember] public string ORDTOUR { get; set; }
      [DataMember] public string SECTEUR { get; set; }
      [DataMember] public string CPOS { get; set; }
      [DataMember] public string TELEPHONE { get; set; }
      [DataMember] public string FAX { get; set; }
      [DataMember] public string EMAIL { get; set; }
      [DataMember] public string USERCREATION { get; set; }
      [DataMember] public System.DateTime DATECREATION { get; set; }
      [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
      [DataMember] public string USERMODIFICATION { get; set; }
      [DataMember] public int PK_ID { get; set; }
      [DataMember] public Nullable<int> FK_IDTOURNEE { get; set; }
      [DataMember] public Nullable<int> FK_IDQUARTIER { get; set; }
      [DataMember] public Nullable<int> FK_IDCOMMUNE { get; set; }
      [DataMember] public Nullable<int> FK_IDRUE { get; set; }
      [DataMember] public Nullable<int> FK_IDSECTEUR { get; set; }
      [DataMember] public int FK_IDCENTRE { get; set; }
      [DataMember] public int FK_IDNUMDEM { get; set; }
      [DataMember] public bool ISACTIF { get; set; }

        //Autre
       [DataMember] public string  CODERUE { get; set; }
       [DataMember] public string  LIBELLECENTRE { get; set; }
       [DataMember] public string  LIBELLECOMMUNE { get; set; }
       [DataMember] public string  LIBELLEQUARTIER { get; set; }
       [DataMember] public string  LIBELLESECTEUR { get; set; }
       [DataMember] public string  LIBELLERUE { get; set; }
       [DataMember] public string  LIBELLETOURNEE { get; set; }
       [DataMember] public bool ISMODIFIER { get; set; }
        

        //
    }

   
}










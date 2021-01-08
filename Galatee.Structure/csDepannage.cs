using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDepannage : CsPrint
    {

        
           [DataMember]public string LECENTRE { get; set; }
           [DataMember]public string LACOMMUNE { get; set; }
           [DataMember]public string LEQUARTIER { get; set; }
           [DataMember]public string MODERECUEIL { get; set; }
           [DataMember]public string TYPEDEPANNE { get; set; }
           [DataMember] public string LARUE { get; set; }
           [DataMember] public string ETAGE { get; set; }
           [DataMember] public string PORTE { get; set; }
           [DataMember] public string LESECTEUR { get; set; }
           [DataMember] public string TELEPHONE { get; set; }
           [DataMember] public string DESCRIPTIONPANNE { get; set; }
           [DataMember] public int PK_ID { get; set; }
           [DataMember] public Nullable<int> FK_IDQUARTIER { get; set; }
           [DataMember] public Nullable<int> FK_IDCOMMUNE { get; set; }
           [DataMember] public Nullable<int> FK_IDRUE { get; set; }
           [DataMember] public Nullable<int> FK_IDSECTEUR { get; set; }
           [DataMember] public int FK_IDCENTRE { get; set; }
           [DataMember] public Nullable<int> FK_IDDEMANDE { get; set; }
           [DataMember] public string LIEUINTERVANTION { get; set; }
           [DataMember] public Nullable<int> FK_IDMODERECEUIL { get; set; }
           [DataMember] public Nullable<int> FK_IDTYPEDEPANNE { get; set; }
           [DataMember] public Nullable<int> FK_IDDETAILPANNE { get; set; }
           [DataMember] public Nullable<bool > ISPERSONNEEXTERIEUR { get; set; }
           [DataMember] public Nullable<bool> ISEDM { get; set; }
           [DataMember] public Nullable<bool> ISCOMMUNE { get; set; }

           [DataMember] public Nullable<DateTime> HEUREDEBUT { get; set; }
           [DataMember]
           public Nullable<DateTime> HEUREFIN { get; set; }
           [DataMember] public Nullable<bool> ISRESEAU { get; set; }
           [DataMember] public Nullable<bool> ISBRANCHEMENT { get; set; }
           [DataMember] public Nullable<bool> ISPROVISOIR { get; set; }
           [DataMember] public Nullable<bool> ISDEFINITIF { get; set; }

        
           [DataMember] public string PROCESVERBAL { get; set; }
           [DataMember] public string PANNETRAITE { get; set; }
           
           [DataMember]
           public string NOM_DECLARANT { get; set; }
           [DataMember]
           public string NOM_CLIENT_DEPANE { get; set; }
           [DataMember]
           public string SIEGE_DEFAUT { get; set; }
           [DataMember]
           public string CAUSE_DEFAUT { get; set; }
           [DataMember]
           public string POSTE { get; set; }
           [DataMember]
           public string IMMATRICULATION { get; set; }
           [DataMember]
           public Nullable<int> FK_IDTYPEDEPANNE_TRAITE { get; set; }
        
    }
}

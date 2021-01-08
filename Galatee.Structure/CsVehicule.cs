using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsVehicule : CsPrint
    {
       [DataMember] public int ID { get; set; }
       [DataMember] public string LIBELLE { get; set; }
       [DataMember] public string IMMATRICULATION { get; set; }
       [DataMember] public string MARQUE { get; set; }
       [DataMember] public Nullable<int> ID_TYPE_VEHICULE { get; set; }
       [DataMember] public bool EST_SUPRIME { get; set; }
       [DataMember] public string ID_ENTITE { get; set; }
       [DataMember] public Nullable<System.DateTime> DATE_MISE_EN_SERVICE { get; set; }
       [DataMember] public Nullable<System.DateTime> DATE_FIN_UTILISATION { get; set; }
       [DataMember] public int ID_STATUT { get; set; }
       [DataMember] public string EXPLOITATION { get; set; }
       [DataMember] public string IMMOBILISATION { get; set; }
       [DataMember] public string NUMERORADIO { get; set; }
       [DataMember] public string CREER_PAR { get; set; }
       [DataMember] public System.DateTime DATE_CREATION { get; set; }
       [DataMember] public string DERNIER_UTILISATEUR { get; set; }
       [DataMember] public System.DateTime DATE_MODIFICATION { get; set; }
    }
}

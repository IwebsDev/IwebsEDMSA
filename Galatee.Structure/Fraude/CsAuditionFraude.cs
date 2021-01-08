using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;
namespace Galatee.Structure
{
     [DataContract]
   public class CsAuditionFraude
    {
         [DataMember]
         public int PK_ID { get; set; }
         [DataMember]
         public System.DateTime DateRendezVous { get; set; }
         [DataMember]
         public Nullable<System.DateTime> DateAudition { get; set; }
         [DataMember]
         public string NomRepondant { get; set; }
         [DataMember]
         public string QualiteRepondant { get; set; }
         [DataMember]
         public Nullable<bool> IsProprietaire { get; set; }
         [DataMember]
         public Nullable<bool> IsMaisonHabitee { get; set; }
         [DataMember]
         public Nullable<int> NombreHabitant { get; set; }
         [DataMember]
         public Nullable<bool> IsDejaDepanne { get; set; }
         [DataMember]
         public string MotifDepannage { get; set; }
         [DataMember]
         public Nullable<bool> IsDejaPenaliseSurCompteur { get; set; }
         [DataMember]
         public Nullable<bool> IsFacturePenaliteDejaRecue { get; set; }
         [DataMember]
         public Nullable<bool> IsDemandeVerificationDejaEmise { get; set; }
         [DataMember]
         public Nullable<bool> IsAccuseReceptionDemande { get; set; }
         [DataMember]
         public Nullable<bool> IsCertificatPlombageRecu { get; set; }
         [DataMember]
         public Nullable<bool> IsNewAppareilAcquis { get; set; }
         [DataMember]
         public Nullable<System.Guid> FK_IDPROCESVERBALSCANNE { get; set; }
         [DataMember]
         public int FK_IDFRAUDE { get; set; }
    }
}

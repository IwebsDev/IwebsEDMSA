using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
namespace Galatee.Structure
{
    [DataContract] 
   public class CsFraude:CsPrint 
    {
       [DataMember] public int Pk_ID { get; set; }
       [DataMember] public Nullable<bool> IsConvocationRespectee { get; set; } 
       [DataMember] public Nullable<bool> IsFraudeConfirmee { get; set; }
       [DataMember] public Nullable<decimal> MontantCaution { get; set; }
       [DataMember] public DateTime DateReclamation { get; set; }
       [DataMember] public string MotifReclamation { get; set; }
       [DataMember] public DateTime DateCreation { get; set; }
       [DataMember] public DateTime DateEtape { get; set; }
       [DataMember] public string FicheTraitement { get; set; }
       [DataMember] public string LIBELLESOURCECONTROLE { get; set; }
       [DataMember] public Nullable<int> Ordre { get; set; }
       [DataMember] public string BordereauTransmission { get; set; }
       [DataMember]  public Nullable<int> FK_IDCLIENTFRAUDE { get; set; }
       [DataMember] public Nullable<int> FK_IDDENONCIATEUR { get; set; }
       [DataMember] public Nullable<int> FK_IDSOURCECONTROLE { get; set; }
       [DataMember] public Nullable<int> FK_IDDECISIONFRAUDE { get; set; }
       [DataMember]  public Nullable<int> FK_IDDEMANDE { get; set; }

      
    }
}

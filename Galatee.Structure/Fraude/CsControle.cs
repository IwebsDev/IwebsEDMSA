using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

 
namespace Galatee.Structure
{
    [DataContract]
  public  class CsControle : CsPrint 
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public string FicheControle { get; set; }
        [DataMember] public string Ordonnateur { get; set; }
        [DataMember] public string NomExpert { get; set; }
        [DataMember] public System.DateTime DateControle { get; set; }
        [DataMember] public string HeureControle { get; set; }
        [DataMember] public string DescriptionIrregulariteEtObservations { get; set; }
        [DataMember] public bool IsAbonneOuRepresentantPresent { get; set; }
        [DataMember] public Nullable<int> CourantAdmissibleParCable { get; set; }
        [DataMember] public Nullable<bool> IsFraudeAveree { get; set; }
        [DataMember] public bool IsConvocationRemise { get; set; }
        [DataMember] public string CommissariatPolicePresent { get; set; }
        [DataMember] public Nullable<bool> IsAnomalieReconnue { get; set; }
        [DataMember] public byte OrdreTraitement { get; set; }
        [DataMember] public int FK_IDCLIENTFRAUDE { get; set; }
        [DataMember] public int FK_IDFRAUDE { get; set; }
        [DataMember] public Nullable<int> FK_IDQUALITEEXPERT { get; set; }
        [DataMember] public Nullable<System.Guid> FK_IDPROCESVERBALSCANNE { get; set; }
    
    }
}

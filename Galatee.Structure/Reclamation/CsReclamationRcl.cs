using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
  public class CsReclamationRcl : CsPrint 
  {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public string Ordre { get; set; }
        [DataMember] public string CENTRE { get; set; }
        [DataMember]public string NumeroReclamation { get; set; }
        [DataMember]public Nullable<int> Fk_IdTypeReclamation { get; set; }
        [DataMember] public string NomClient { get; set; }
        [DataMember]public Nullable<int> Fk_IdClient { get; set; }
        [DataMember]  public Nullable<System.DateTime> DateOuverture { get; set; }
        [DataMember] public Nullable<System.DateTime> DateTransmission { get; set; }
        [DataMember] public string AgentEmetteur { get; set; }
        [DataMember] public string AgentRecepteur { get; set; }
        [DataMember]public string AgentValidation { get; set; }
        [DataMember]public Nullable<System.DateTime> DateRdv { get; set; }
        [DataMember] public Nullable<System.DateTime> DateRetourSouhaite { get; set; }
        [DataMember] public Nullable<System.DateTime> DateRetour { get; set; }
        [DataMember] public string Observation { get; set; }
        [DataMember] public Nullable<int> Fk_IdModeReception { get; set; }
        [DataMember]public string Adresse { get; set; }
        [DataMember] public string Email { get; set; }
        [DataMember] public string NumeroTelephonePortable { get; set; }
        [DataMember] public string NumeroTelephoneFixe { get; set; }
        [DataMember] public string ObjetReclamation { get; set; }
        [DataMember] public string ActionMenees { get; set; }
        [DataMember] public Nullable<byte> Fk_IdStatutReclamation { get; set; }
        [DataMember] public string MotifReprise { get; set; }
        [DataMember] public Nullable<bool> NonConformite { get; set; }
        [DataMember]public string LettreReponse { get; set; }
        [DataMember] public Nullable<System.DateTime> DateValidation { get; set; }
        [DataMember] public string MotifRejet { get; set; }
        [DataMember] public Nullable<int> Fk_IdCentre { get; set; }
        [DataMember]  public string Client { get; set; }
        [DataMember]  public Nullable<int> FK_IDDEMANDE { get; set; }
        [DataMember] public Nullable<int> FK_IDCOMMUNE { get; set; }
        [DataMember] public Nullable<int> FK_IDQUARTIER { get; set; }
        [DataMember] public Nullable<int> FK_IDRUE { get; set; }
        [DataMember] public string COMMUNE { get; set; }
        [DataMember] public Nullable<int> FK_IDSECTEUR { get; set; }
        [DataMember] public string SECTEUR { get; set; }
        [DataMember] public string QUARTIER { get; set; }
        [DataMember] public byte? Fk_IdValidation { get; set; }
        [DataMember] public string Prenoms { get; set; }
        [DataMember] public Nullable<System.Guid> Fk_Id_GroupValidation { get; set; }
        
        //Autre 
        [DataMember] public string LIBELLESITE { get; set; }
        [DataMember] public string LIBELLECENTRE { get; set; }
        [DataMember] public string LIBELLETYPERECLAMATION { get; set; }
        [DataMember] public string NOMAGENTCREATION{ get; set; }
        [DataMember] public string NOMAGENTRECEPTEUR{ get; set; }
        [DataMember] public string NOMAGENTVALIDATEUR{ get; set; }
        [DataMember] public int NOMBRE { get; set; }
        [DataMember] public int TAUX { get; set; }

    }
}

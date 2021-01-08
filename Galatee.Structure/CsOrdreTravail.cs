using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsOrdreTravail : CsPrint
    {
       [DataMember]  public string PRESTATAIRE { get; set; }
       [DataMember]  public string USERCREATION { get; set; }
       [DataMember]  public System.DateTime DATECREATION { get; set; }
       [DataMember]  public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember]  public string USERMODIFICATION { get; set; }
       [DataMember]  public int PK_ID { get; set; }
       [DataMember]  public string MATRICULE { get; set; }
       [DataMember]  public int FK_IDDEMANDE { get; set; }
       [DataMember]  public int FK_IDADMUTILISATEUR { get; set; }
       [DataMember]  public string LIBELLEAGENT { get; set; }
       [DataMember]  public string COMMENTAIRE { get; set; }
       [DataMember]  public Nullable<System.DateTime> DATEDEBUTTRAVAUX { get; set; }
       [DataMember]  public Nullable<System.DateTime> DATEFINTRAVAUX { get; set; }

       [DataMember]  public string LIBELLEPRODUIT { get; set; }
       [DataMember]  public string LIBELLETYPEDEMANDE { get; set; }
       [DataMember]  public string NOMCLIENT { get; set; }
       [DataMember]  public string TELEPHONNE { get; set; }
       [DataMember]  public string COMMUNE { get; set; }
       [DataMember]  public string QUARTIER { get; set; }
       [DataMember]  public string LONGITUDE { get; set; }
       [DataMember]  public string LATITUDE { get; set; }


        
    }
 }










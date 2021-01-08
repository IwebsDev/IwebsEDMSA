using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public  class CsUtilisateur
    {

        public enum StatusCompte { Actif = 1, Inactif, Verrouille ,VerrouilleOuvertureSession };
          [DataMember] public string CENTRE { get; set; }
          [DataMember] public string MATRICULE { get; set; }
          [DataMember] public string LIBELLE { get; set; }
          [DataMember] public string PASSE { get; set; }
          [DataMember] public string FONCTION { get; set; }
          [DataMember] public string CFONCT { get; set; }
          [DataMember] public string CODEHIER { get; set; }
          [DataMember] public Nullable<System.DateTime> DMAJ { get; set; }
          [DataMember] public string TRANS { get; set; }
          [DataMember] public string LOGINNAME { get; set; }
          [DataMember] public string E_MAIL { get; set; }
          [DataMember] public Nullable<System.DateTime> DATEDERNIEREMODIFICATION { get; set; }
          [DataMember] public Nullable<System.DateTime> DATEDEBUTVALIDITE { get; set; }
          [DataMember] public Nullable<System.DateTime> DATEFINVALIDITE { get; set; }
          [DataMember] public Nullable<int> STATUSCOMPTE { get; set; }
          [DataMember] public Nullable<System.DateTime> DATEDERNIEREMODIFICATIONPASSWORD { get; set; }
          [DataMember] public Nullable<bool> INITUSERPASSWORD { get; set; }
          [DataMember] public Nullable<int> NOMBREECHECSOUVERTURESESSION { get; set; }
          [DataMember] public Nullable<System.DateTime> DATEDERNIERECONNEXION { get; set; }
          [DataMember] public Nullable<bool> DERNIERECONNEXIONREUSSIE { get; set; }
          [DataMember] public Nullable<System.DateTime> DATEDERNIERVERROUILLAGE { get; set; }
          [DataMember] public string BRANCHE { get; set; }
          [DataMember] public byte  PERIMETREACTION { get; set; }
          [DataMember] public Nullable<bool> ESTSUPPRIMER { get; set; }
          [DataMember] public string USERCREATION { get; set; }
          [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
          [DataMember] public string USERMODIFICATION { get; set; }
          [DataMember] public System.DateTime DATECREATION { get; set; }
          [DataMember] public int FK_IDFONCTION { get; set; }
          [DataMember] public int FK_IDCENTRE { get; set; }
          [DataMember] public int PK_ID { get; set; }
          [DataMember] public byte FK_IDSTATUS { get; set; }
          [DataMember] public string COMPTEWINDOW { get; set; }
          [DataMember] public string TELEPHONE { get; set; }

        
           [DataMember] public int FK_IDANCIENCENTRE { get; set; }

           [DataMember] public string LIBELLESTATUSCOMPTE { get; set; }
           [DataMember] public string LIBELLEFONCTION { get; set; }
           [DataMember] public string LIBELLECENTRE { get; set; }
           [DataMember] public string LIBELLEPERIMETREACTION { get; set; }
           [DataMember] public bool ESTCONSULTATION { get; set; }
           [DataMember] public bool EsADMIN { get; set; }
           [DataMember] public string CENTREAFFICHER { get; set; }
           [DataMember] public int FK_CENTREAFFECTATION { get; set; }
           [DataMember] public string NOM { get; set; }
           [DataMember] public string CODE { get; set; }
           [DataMember] public List<CsProfil> LESPROFILSUTILISATEUR { get; set; }
           [DataMember] public List<CsCentreDuProfil> LESCENTRESDESPROFILSUSER { get; set; }
           [DataMember] public bool ISSELECT { get; set; }

        
    }
}

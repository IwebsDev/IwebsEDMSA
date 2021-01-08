using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Fichier  généré par InovaDTOcreator , - utilitaire WPF  créé par JAB pour la generation des Data  Transfert Object http://beableworld.wordpress.com
namespace Galatee.Structure

{
    [DataContract]
    public class ObjMATRICULE
    {


        [DataMember]
        public String CENTRE { get; set; }
        [DataMember]
        public String MATRICULE { get; set; }
        [DataMember]
        public String LIBELLE { get; set; }
        [DataMember]
        public String PASSE { get; set; }
        [DataMember]
        public String FONCTION { get; set; }
        [DataMember]
        public String CFONCT { get; set; }
        [DataMember]
        public String CODEHIER { get; set; }
        [DataMember]
        public DateTime? DMAJ { get; set; }
        [DataMember]
        public String TRANS { get; set; }
        [DataMember]
        public Binary ROWID { get; set; }
        [DataMember]
        public String LoginName { get; set; }
        [DataMember]
        public String E_MAIL { get; set; }
        [DataMember]
        public DateTime? DateCreation { get; set; }
        [DataMember]
        public DateTime? DateDerniereModification { get; set; }
        [DataMember]
        public DateTime? DateDebutValidite { get; set; }
        [DataMember]
        public DateTime? DateFinValidite { get; set; }
        [DataMember]
        public int? IdStatusCompte { get; set; }
        [DataMember]
        public Guid? RoleID { get; set; }
        [DataMember]
        public DateTime? DateDerniereModificationPassword { get; set; }
        [DataMember]
        public Boolean? InitUserPassword { get; set; }
        [DataMember]
        public int? NombreEchecsOuvertureSession { get; set; }
        [DataMember]
        public DateTime? DateDerniereConnexion { get; set; }
        [DataMember]
        public Boolean? DerniereConnexionReussie { get; set; }
        [DataMember]
        public DateTime? DateDernierVerrouillage { get; set; }
        [DataMember]
        public String NumCaisse { get; set; }
        [DataMember]
        public String Branche { get; set; }
        [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
    }
}

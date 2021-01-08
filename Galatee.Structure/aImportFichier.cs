using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class aImportFichier
    {
        [DataMember] 
        public int CODE { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public string DESCRIPTION { get; set; }
        [DataMember]
        public string COMMANDE { get; set; }
        [DataMember]
        public string REPERTOIRE { get; set; }
        [DataMember]
        public string FICHIER { get; set; }
        [DataMember]
        public int? NBPARAMETRE { get; set; }
        [DataMember]
        public bool ISPROCEDURE { get; set; }
        [DataMember]
        public string SERVER { get; set; }
        [DataMember]
        public string BASEDEDONNE { get; set; }
        [DataMember]
        public string UTILISATEUR { get; set; }
        [DataMember]
        public string MOTDEPASSE { get; set; }
        [DataMember]
        public string PROVIDER { get; set; }
        [DataMember]
        public string REQUTETTEBASEDISTANTE { get; set; }
        
    }

    [DataContract]
    public class aImportFichierColonne
    {
        [DataMember]
        public int ID_COLONNE { get; set; }
        [DataMember]
        public string NOM { get; set; }
        [DataMember]
        public string TYPE { get; set; }
        [DataMember]
        public int? LONGUEUR { get; set; }
        [DataMember]
        public string DESCRIPTION { get; set; }
        [DataMember]
        public int? ID_PARAMETRAGE { get; set; }
    }
}

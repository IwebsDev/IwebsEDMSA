using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Fichier  généré par InovaDTOcreator , - utilitaire WPF  créé par JAB pour la generation des Data  Transfert Object http://beableworld.wordpress.com
namespace Galatee.Structure

{
    [DataContract]
    public class ObjSITE
    {


        [DataMember]
        public String CODESITE { get; set; }
        [DataMember]
        public String LIBELLE { get; set; }
        [DataMember]
        public String SERVEUR { get; set; }
        [DataMember]
        public String USERID { get; set; }
        [DataMember]
        public String PWD { get; set; }
        [DataMember]
        public String CATALOGUE { get; set; }


    }
}

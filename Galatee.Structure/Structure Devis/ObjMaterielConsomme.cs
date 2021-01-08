using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Fichier  généré par InovaDTOcreator , - utilitaire WPF  créé par JAB pour la generation des Data  Transfert Object http://beableworld.wordpress.com
namespace Galatee.Structure

{
    [DataContract]
    public class ObjMaterielConsomme
    {


        [DataMember]
        public String NumDevis { get; set; }
        [DataMember]
        public Byte Ordre { get; set; }
        [DataMember]
        public Int32 NumFourniture { get; set; }
        [DataMember]
        public Int32 Quantite { get; set; }


    }
}

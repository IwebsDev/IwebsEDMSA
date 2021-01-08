using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
   public class ObjFonction
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Libelle { get; set; }
        [DataMember]
        public int IdFiliere { get; set; }

       ////public ObjFonction(Fonction fonction)
       ////{
       ////    this.Id = fonction.Id;
       ////    this.Code = fonction.Code;
       ////    this.Libelle = fonction.Libelle;
       ////    this.IdFiliere = fonction.IdFiliere;
       ////}
    }
}

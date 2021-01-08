using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class objProduit
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Libelle { get; set; }

        //public objProduit(Produit obj )
        //{

        //    this.Id = obj.Id;
        //    this.Libelle = obj.Libelle; 

        //}

    }
}

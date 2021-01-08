using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCriteresDevis
    {
        [DataMember]
        public string NumeroDevis { get; set; }

        [DataMember]
        public int IdDevis { get; set; }

        [DataMember]
        public Guid IdDocumentAutorisation { get; set; }

        [DataMember]
        public Guid IdDocumentPreuvePropriete { get; set; }

        [DataMember]
        public Guid IdDocumentSchema { get; set; }

        [DataMember]
        public Guid IdDocumentManuscrit { get; set; }

        [DataMember]
        public string CodeProduit { get; set; }

        [DataMember]
        public DateTime DateEtape { get; set; }

        [DataMember]
        public int IdEtapeDevis { get; set; }

        [DataMember]
        public decimal? Maxi { get; set; }

        [DataMember]
        public decimal? Seuil { get; set; }

        [DataMember]
        public decimal? MaxiSubvention { get; set; }

    }
}

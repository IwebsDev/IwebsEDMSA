using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsEncaissementHorsLigne : CsPrint
    {
        [DataMember]
        public string Centre;
        [DataMember]
        public string Client;
        [DataMember]
        public string Ordre;
        [DataMember]
        public decimal Montant;
        [DataMember]
        public string NumeroCheque;
        [DataMember]
        public string ReferenceClient;
        [DataMember]
        public string CollectorName;
        [DataMember]
        public string Caisse;
        [DataMember]
        public string Acquit;
        [DataMember]
        public string Matricule;
        [DataMember]
        public string NumeroRecu;
        [DataMember]
        public DateTime DateEnregistrement;
        [DataMember]
        public string Banque;
        [DataMember]
        public string Guichet;
        [DataMember]
        public string Origine;
        [DataMember]
        public bool IsCanceled;
        [DataMember]
        public string modeReglement;
        [DataMember]
        public string modeReglementLibelle;
    }

    public class XmlRoot : CsPrint
    {
        [DataMember]
        public string root;
        [DataMember]
        public List<CsEncaissementHorsLigne> Encaissements;
    }
}

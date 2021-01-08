using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure 
{
    [DataContract]
    public class CCaracteristique
    {
        private string id;
        private string typeTenstion;
        private string amperageDiametre;
        private string codeProduit;
        private string libelle;

        public CCaracteristique()
        {
        }

        [DataMember]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        [DataMember]
        public string Libelle
        {
            get { return libelle; }
            set { libelle = value; }
        }

        [DataMember]
        public string CodeProduit
        {
            get { return codeProduit; }
            set { codeProduit = value; }
        }
        [DataMember]
        public string TypeTension
        {
            get { return typeTenstion; }
            set { typeTenstion = value; }
        }

        [DataMember]
        public string AmperageDiametre
        {
            get { return amperageDiametre; }
            set { amperageDiametre = value; }
        }

        public override string ToString()
        {
            return this.AmperageDiametre;
        }
    }
}

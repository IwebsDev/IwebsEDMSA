using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsTableReference 
    {
        [DataMember]
        public List<CParametre> Produits
        {
            get;
            set;
        }
        [DataMember]
        public List<CParametre> Frequences
        {
            get;
            set;
        }
        [DataMember]
        public List<CParametre> CategorieClient
        {
            get;
            set;
        }

        public CsTableReference()
        {
            Frequences = new List<CParametre>();
            CategorieClient = new List<CParametre>();
            Produits = new List<CParametre>();
        }
    }
}

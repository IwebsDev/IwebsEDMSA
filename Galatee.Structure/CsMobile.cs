using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Galatee.Structure
{
    [DataContract]
    public class CsMobile
    {
        [DataMember]
        public string AdresseMac { get; set; }

        [DataMember]
        public bool? IsToBeConsumed { get; set; }

        [DataMember]
        public List<CASIND> CasIndex { get; set; }

        [DataMember]
        public List<MODULE> Modules { get; set; }

        [DataMember]
        public List<RELEVEUR> Releveurs { get; set; }

        [DataMember]
        public List<PAGERI> Pageri { get; set; }

        [DataMember]
        public List<EVENEMENT> Evenement { get; set; }

        [DataMember]
        public bool Allow { get; set; }

        [DataMember]
        public List<string> Actions { get; set; }

        public CsMobile()
        {
            try
            {
                CasIndex = new List<CASIND>();
                Modules = new List<MODULE>();
                Releveurs = new List<RELEVEUR>();
                Pageri = new List<PAGERI>();
                Evenement = new List<EVENEMENT>();
                Actions = new List<string>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

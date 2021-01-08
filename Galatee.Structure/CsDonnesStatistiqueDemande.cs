using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDonnesStatistiqueDemande:CsPrint
    {
                                                      [DataMember]
        public string   PK_ID    { get; set; }        [DataMember]
        public string   LIBELLE  { get; set; }        [DataMember]
        public string   SITE     { get; set; }        [DataMember]
        public string   PRODUIT  { get; set; }        [DataMember]
        public string   INDIC    { get; set; }        [DataMember]
        public string   INSTANCE { get; set; }        [DataMember]
        public string   janvier  { get; set; }        [DataMember]
        public string   fevrier  { get; set; }        [DataMember]
        public string   mars     { get; set; }        [DataMember]
        public string   avril    { get; set; }        [DataMember]
        public string   mai      { get; set; }        [DataMember]
        public string   juin     { get; set; }        [DataMember]
        public string   juillet  { get; set; }        [DataMember]
        public string   aout     { get; set; }        [DataMember]
        public string   sepembre { get; set; }        [DataMember]
        public string   octobre  { get; set; }        [DataMember]
        public string   nonvembre{ get; set; }        [DataMember]
        public string   decembre { get; set; }        
	        
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsEncaissement : CsPrint
    {
        [DataMember] public List<CsClient> Clients { get; set; }
        [DataMember] public List<CsLclient > FactureARegler { get; set; }
        [DataMember] public List<CsCtax> Taxes { get; set; }
        [DataMember] public CsTa ParametreGeneraux { get; set; }
        [DataMember] public List<CsCentre> Sites { get; set; }
        public CsEncaissement()
        {
            try
            {
                Clients = new List<CsClient>();
                FactureARegler = new List<CsLclient >();
                Taxes = new List<CsCtax>();
                ParametreGeneraux = new CsTa();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }

}










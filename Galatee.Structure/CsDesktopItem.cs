using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
    public class CsDesktopItem
    {
       
        [DataMember]
        public int? ID
        { get; set; }

   
          [DataMember]
        public int? IdGroupProgram
        { get; set; }

      
          [DataMember]
        public string LIBELLE_FONCTION
        { get; set; }

     
          [DataMember]
        public string NOM
          { get; set; }

             [DataMember]
        public string Process
        { get; set; }

             [DataMember]
             public CsProfil ProfilDesktopItem
             { get; set; }

             [DataMember]
             public List<string> LstModuleIdProfil 
             { get; set; }
    }
}

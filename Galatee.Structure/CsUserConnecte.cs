using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsUserConnecte
    {
        [DataMember] public string   matricule { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public int FK_IDFONCTION { get; set; }
        [DataMember] public string   numcaisse { get; set; }
        [DataMember] public string   nomUtilisateur { get; set; }
        [DataMember] public string   codefontion { get; set; }
        [DataMember] public string LibelleFontion { get; set; }
        [DataMember] public bool isAdmin { get; set; }
        [DataMember] public  string Centre { get; set; }
        [DataMember] public string LibelleCentre { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public int   PerimetreAction { get; set; }
        [DataMember] public string PK_ID_CENTRE { get; set; }
        [DataMember] public string PK_ID_SITE { get; set; }
        [DataMember] public string LIBELLESITE { get; set; }
        [DataMember] public string CODESITE { get; set; }
        [DataMember] public List<CsProfil> listeProfilUser { get; set; }
        [DataMember] public List<string > ListeDesCentreProfil { get; set; }
        [DataMember] public int FK_IDCENTRE { get; set; }
        
      
        }
   
}











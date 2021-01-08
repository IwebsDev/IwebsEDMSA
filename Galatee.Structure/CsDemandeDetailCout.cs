using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsDemandeDetailCout
    {
          [DataMember]  public string NUMDEM { get; set; }
          [DataMember]  public string CENTRE { get; set; }
          [DataMember]  public string NDOC { get; set; }
          [DataMember]  public string REFEM { get; set; }
          [DataMember]  public string CLIENT { get; set; }
          [DataMember]  public string ORDRE { get; set; }
          [DataMember]  public string NATURE { get; set; }
          [DataMember]  public string COPER { get; set; }
          [DataMember]  public Nullable<decimal> MONTANTHT { get; set; }
          [DataMember]  public Nullable<decimal> MONTANTTAXE { get; set; }
          [DataMember]  public Nullable<decimal> MONTANTTTC { get; set; }
          [DataMember]  public string TAXE { get; set; }
          [DataMember]  public Nullable<System.DateTime> DATECREATION { get; set; }
          [DataMember]  public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
          [DataMember]  public string USERCREATION { get; set; }
          [DataMember]  public string USERMODIFICATION { get; set; }
          [DataMember]  public int PK_ID { get; set; }
          [DataMember]  public int FK_IDCENTRE { get; set; }
          [DataMember]  public int FK_IDTAXE { get; set; }
          [DataMember]  public int FK_IDCOPER { get; set; }
          [DataMember]  public int FK_IDDEMANDE { get; set; }
          [DataMember] public string DIRECTION { get; set; }
          [DataMember] public bool  ISVALIDER { get; set; }
          [DataMember] public bool  ISEXTENSION { get; set; }
          [DataMember]  public Nullable<System.DateTime> DATECAISSE { get; set; }
          [DataMember] public string NOMCLIENT { get; set; }

        
        /*Autres*/
          [DataMember] public string LIBELLE { get; set; }
          [DataMember] public string LIBELLETAXE { get; set; }
        
    
    }

}










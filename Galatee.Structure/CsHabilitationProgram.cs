using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsHabilitationProgram : CsPrint
    {

       [DataMember]  public int? Id { get; set; }
       [DataMember]  public int? FK_IDGROUPPROGRAM { get; set; }
       [DataMember]  public int FK_IDPROFIL { get; set; }
       [DataMember]  public string LIBELLEFONCTION { get; set; }
       [DataMember]  public string CODEFONCTION { get; set; }
       [DataMember]  public int FK_IDFONCTION { get; set; }
       [DataMember]  public int? FK_IDMODULE { get; set; }
       [DataMember]  public string Code { get; set; }
       [DataMember]  public string Libelle { get; set; }
       [DataMember]  public string ModuleName { get; set; }
       [DataMember]  public int? FK_IDMENU { get; set; }
       [DataMember]  public string ProgramName { get; set; }
       [DataMember]  public string CodeProgram { get; set; }
       [DataMember]  public DateTime? DATECREATION { get; set; }
       [DataMember]  public string USERCREATION { get; set; }
       [DataMember]  public DateTime? DATEMODIFICATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
        
    }

}










﻿using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsSite : CsPrint
    {
      [DataMember]  public string CODE  { get; set; }
      [DataMember]  public string LIBELLE { get; set; }
      [DataMember]  public string SERVEUR { get; set; }
      [DataMember]  public string USERID { get; set; }
      [DataMember]  public string PWD { get; set; }
      [DataMember]  public string CATALOGUE { get; set; }
      [DataMember]  public string  NUMERODEMANDE { get; set; }
      [DataMember]  public string NUMEROFACTURE { get; set; }
      [DataMember]  public System.DateTime DATECREATION { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
      [DataMember]  public string USERCREATION { get; set; }
      [DataMember]  public string USERMODIFICATION { get; set; }
      [DataMember]  public int PK_ID { get; set; }
      [DataMember] public bool IsSelect { get; set; }
    }

}










﻿using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsDepart
    {
      [DataMember] public string CODE { get; set; }
      [DataMember] public string LIBELLE { get; set; }
      [DataMember] public System.DateTime DATECREATION { get; set; }
      [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
      [DataMember] public string USERCREATION { get; set; }
      [DataMember] public string USERMODIFICATION { get; set; }
      [DataMember] public int PK_ID { get; set; }


      //[DataMember] public int FK_IDPOSTETRANSFORMATION { get; set; }
      [DataMember] public int FK_IDPOSTESOURCE { get; set; }

      //[DataMember] public string CODETRANSFORMATEUR { get; set; }
      //[DataMember] public string LIBELLETRANSFORMATEUR { get; set; }

      [DataMember] public string CODEPOSTESOURCE { get; set; }
      [DataMember] public string LIBELLEPOSTESOURCE { get; set; }

    }
}










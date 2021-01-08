using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsCasind : CsPrint
    {
        
      [DataMember]  public string CODE { get; set; }
      [DataMember]  public string LIBELLE { get; set; }
      [DataMember]  public string APRESENQUETE { get; set; }
      [DataMember]  public string SANSENQUETE { get; set; }
      [DataMember]  public string CASGEN1 { get; set; }
      [DataMember]  public string CASGEN2 { get; set; }
      [DataMember]  public string CASGEN3 { get; set; }
      [DataMember]  public string CASGEN4 { get; set; }
      [DataMember]  public string CASGEN5 { get; set; }
      [DataMember]  public string CASGEN6 { get; set; }
      [DataMember]  public string CASGEN7 { get; set; }
      [DataMember]  public string CASGEN8 { get; set; }
      [DataMember]  public string CASGEN9 { get; set; }
      [DataMember]  public string CASGEN10 { get; set; }
      [DataMember]  public string SAISIEINDEX { get; set; }
      [DataMember]  public string SAISIECOMPTEUR { get; set; }
      [DataMember]  public string SAISIECONSO { get; set; }
      [DataMember]  public bool  ENQUETABLE { get; set; }
      [DataMember]  public System.DateTime DATECREATION { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
      [DataMember]  public string USERCREATION { get; set; }
      [DataMember]  public string USERMODIFICATION { get; set; }
      [DataMember]  public int PK_ID { get; set; }
      [DataMember]  public int? FK_IDTYPEFACTURATIONAPRESENQUETE { get; set; }
      [DataMember]  public int? FK_IDTYPEFACTURATIONSANSENQUETE { get; set; }
      [DataMember]  public bool  ESSUPPRIMER { get; set; }
      [DataMember]  public bool  IsSelect { get; set; }

        public override string ToString()
        {
            return LIBELLE;
        }
    }
}










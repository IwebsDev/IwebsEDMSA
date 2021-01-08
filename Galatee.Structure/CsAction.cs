using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsAction : CsPrint 
    {
      [DataMember]  public string LOT { get; set; }
      [DataMember]  public string PERIODE { get; set; }
      [DataMember]  public string ACTION1 { get; set; }
      [DataMember]  public string JET { get; set; }
      [DataMember]  public string SSACTION { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATE1 { get; set; }
      [DataMember]  public Nullable<int> NOMBRE1 { get; set; }
      [DataMember]  public Nullable<decimal> MONTANT1 { get; set; }
      [DataMember]  public Nullable<int> NOMBRE2 { get; set; }
      [DataMember]  public Nullable<decimal> MONTANT2 { get; set; }
      [DataMember]  public Nullable<int> NOMBRE3 { get; set; }
      [DataMember]  public Nullable<decimal> MONTANT3 { get; set; }
      [DataMember]  public string MATRICULE { get; set; }
      [DataMember]  public string PRODUIT { get; set; }
      [DataMember]  public string STATUT { get; set; }
      [DataMember]  public string LIBELLE { get; set; }
      [DataMember]  public byte[] ROWID { get; set; }
      [DataMember]  public int PK_ID { get; set; }
    }
}










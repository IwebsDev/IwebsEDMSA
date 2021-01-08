using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBSYNCHRONISATION
    {
		
		
[DataMember]
		public Guid IDSYNCHRONISATION { get; set; }
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public DateTime DATEDEBUTSYNCHRONISATION { get; set; }
[DataMember]
		public DateTime DATEFINSYNCHRONISATION { get; set; }
[DataMember]
		public Int32 IDTYPESYNCHRONISATION { get; set; }
[DataMember]
		public Int32? NBRECOMPTEURSBTATRAITES { get; set; }
[DataMember]
		public Int32? NBRECOMPTEURSBTAAJOUTES { get; set; }
[DataMember]
		public Int32? NBRECOMPTEURSBTAMAJ { get; set; }
[DataMember]
		public Int32? NBRECOMPTEURSBTAREJETES { get; set; }
[DataMember]
		public Int32? NBREBRANCHEMENTSTRAITES { get; set; }
[DataMember]
		public Int32? NBREBRANCHEMENTSAJOUTES { get; set; }
[DataMember]
		public Int32? NBREBRANCHEMENTSMAJ { get; set; }
[DataMember]
		public Int32? NBREBRANCHEMENTSREJETES { get; set; }
[DataMember]
		public Int32? NBRERACCORDEMENTSTRAITES { get; set; }
[DataMember]
		public Int32? NBRERACCORDEMENTSAJOUTES { get; set; }
[DataMember]
		public Int32? NBRERACCORDEMENTSMAJ { get; set; }
[DataMember]
		public Int32? NBRERACCORDEMENTSREJETES { get; set; }
[DataMember]
		public Int32? NBRECONSOMMATIONSBTATRAITES { get; set; }
[DataMember]
		public Int32? NBRECONSOMMATIONSBTAAJOUTES { get; set; }
[DataMember]
		public Int32? NBRECONSOMMATIONSBTAMAJ { get; set; }
[DataMember]
		public Int32? NBRECONSOMMATIONSBTAREJETEES { get; set; }
[DataMember]
		public Int32? NBREABONNESBTATRAITES { get; set; }
[DataMember]
		public Int32? NBREABONNESBTAAJOUTES { get; set; }
[DataMember]
		public Int32? NBREABONNESBTAMAJ { get; set; }
[DataMember]
		public Int32? NBREABONNESBTAREJETES { get; set; }
[DataMember]
		public Int32? NBREBETTRAITES { get; set; }
[DataMember]
		public Int32? NBREBETAJOUTES { get; set; }
[DataMember]
		public Int32? NBREBETMAJ { get; set; }
[DataMember]
		public Int32? NBREBETENERREUR { get; set; }
[DataMember]
		public Int32? NBREAGENTSTRAITES { get; set; }
[DataMember]
		public Int32? NBREAGENTSAJOUTES { get; set; }
[DataMember]
		public Int32? NBREAGENTSMAJ { get; set; }
[DataMember]
		public Int32? NBREAGENTSREJETES { get; set; }
[DataMember]
		public Int32? NBRECONSOMMATIONSHTATRAITEES { get; set; }
[DataMember]
		public Int32? NBRECONSOMMATIONSHTAAJOUTEES { get; set; }
[DataMember]
		public Int32? NBRECONSOMMATIONSHTAMAJ { get; set; }
[DataMember]
		public Int32? NBRECONSOMMATIONSHTAREJETEES { get; set; }
[DataMember]
		public Int32? NBREABONNESHTATRAITES { get; set; }
[DataMember]
		public Int32? NBREABONNESHTAAJOUTES { get; set; }
[DataMember]
		public Int32? NBREABONNESHTAMAJ { get; set; }
[DataMember]
		public Int32? NBREABONNESHTAREJETES { get; set; }
[DataMember]
		public Boolean? SYNCHROMANUELLE { get; set; }
[DataMember]
		public Int32? CODEFINSYNCHRONISATION { get; set; }
[DataMember]
		public DateTime? DATEDEBUTPRECHARGEMENT { get; set; }
[DataMember]
		public DateTime? DATEFINPRECHARGEMENT { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsQtFacMt
    {
       [DataMember] public decimal dPuissanceSouscrite { get; set; }
       [DataMember] public decimal dConsoActive { get; set; }
       [DataMember] public decimal dConsoActHPointes { get; set; }
       [DataMember] public decimal dConsoActHPleines { get; set; }
       [DataMember] public decimal dConsoActHCreuses { get; set; }
       [DataMember] public decimal dConsoReactive { get; set; }
       [DataMember] public decimal dConsoHoraire { get; set; }
       [DataMember] public decimal dConsoMaximetre { get; set; }
       [DataMember] public decimal dConsoTotaleActive { get; set; }
       [DataMember] public decimal dConsoTotActiveAvecPertes { get; set; }
       [DataMember] public decimal dConsoTotReactiveAvecPertes { get; set; }
       [DataMember] public decimal dKa1 { get; set; }
       [DataMember] public decimal dKa2 { get; set; }
       [DataMember] public decimal dKr1 { get; set; }
       [DataMember] public decimal dKr2 { get; set; }
       [DataMember] public decimal dKimp { get; set; }
       [DataMember] public decimal dPertesActives { get; set; }
       [DataMember] public decimal dPertesReactives { get; set; }
       [DataMember] public decimal dTanPhi { get; set; }
       [DataMember] public decimal dCoefMinoMajo { get; set; }
       [DataMember] public decimal dConsoActiveAfterPertes { get; set; }
       [DataMember] public decimal dConsoActiveHPointesAfterPertes { get; set; }
       [DataMember] public decimal dConsoActiveHPleinesAfterPertes { get; set; }
       [DataMember] public decimal dConsoActiveHCreusesAfterPertes { get; set; }
       [DataMember] public decimal dMontantHNormal { get; set; }
       [DataMember] public decimal dMontantHPointes { get; set; }
       [DataMember] public decimal dMontantHPleines { get; set; }
       [DataMember] public decimal dMontantHCreuses { get; set; }
       [DataMember] public decimal dMontantPenaliteDepassement { get; set; }
       [DataMember] public decimal dMontantPrimeFixeMensuelle { get; set; }

       [DataMember] public decimal dCumulRegFacDeb { get; set; }
       [DataMember] public decimal dCumulRegConsoDeb { get; set; }

       [DataMember]  public string  PertesActives { get; set; }
       [DataMember]  public string PertesReactives { get; set; }

       [DataMember]  public string TotActiveAvecPertes  { get; set; }
       [DataMember]  public string TotReactiveAvecPertes  { get; set; }


       [DataMember]  public string TanPhi  { get; set; }
       [DataMember]  public string WrMr  { get; set; }
       [DataMember]  public string WaMa  { get; set; }



        
        


        
        
    }

}










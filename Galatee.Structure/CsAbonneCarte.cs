using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
//using Microsoft.Maps.MapControl;

namespace Galatee.Structure
{
    [DataContract]
    public class CsAbonneCarte 
    {
        [DataMember]
        public string Longitude
        { get; set; }

        //[DataMember]
        //public string SetLongitude
        //{
        //    //get { return longitude; }
        //    set { longitude = double.Parse(value); }
        //}

        //[DataMember]
        //public string SetLatitude
        //{
        //    //get { return longitude; }
        //    set { longitude = double.Parse(value); }
        //}
        //[DataMember]
        //private double latitude;
        [DataMember]
        public string Latitude
        { get; set; }

        //[DataMember]
        //private string numeroClient;
        [DataMember]
        public string NumeroClient
        { get; set; }

       //[DataMember]
        //private string nomAbonne;
        [DataMember]
        public string NomAbonne
        { get; set; }

        //[DataMember]
        //private string centre;
        [DataMember]
        public string Centre
        { get; set; }

        //[DataMember]
        //private string telephone;
        [DataMember]
        public string Telephone
        { get; set; }
    }
}

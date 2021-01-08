using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Galatee.Structure
{
     [DataContract]
   public class CsImageFile:CsPrint
    {
        [DataMember]
        public string ImageName { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
        [DataMember]
        public byte[] Imagestream { get; set; }
       
    }


     [DataContract]
     public class CsByte : CsPrint
     {
         [DataMember]
         public byte[] Imagestream { get; set; }

     }
}

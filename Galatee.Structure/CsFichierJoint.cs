using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsFichierJoint
    {
        int id;
        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        string nomDoc;
        [DataMember]
        public string NomDoc
        {
            get { return nomDoc; }
            set { nomDoc = value; }
        }

        Byte[] contenu;
        [DataMember]
        public Byte[] Contenu
        {
            get { return contenu; }
            set { contenu = value; }
        }

        string idEtape;
        [DataMember]
        public string IdEtape
        {
            get { return idEtape; }
            set { idEtape = value; }
        }

        string idOrdreReclamation;
        [DataMember]
        public string IdOrdreReclamation
        {
            get { return idOrdreReclamation; }
            set { idOrdreReclamation = value; }
        }

        Guid idReclamation;
        [DataMember]
        public Guid IdReclamation
        {
            get { return idReclamation; }
            set { idReclamation = value; }
        }
    }
}

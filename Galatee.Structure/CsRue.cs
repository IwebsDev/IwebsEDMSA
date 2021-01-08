using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
    public class CsRue
    {
        private string m_CENTRE;
         [DataMember]
        public string CENTRE
        {
            get { return m_CENTRE; }
            set { m_CENTRE = value; }
        }
        private string m_COMMUNE;
         [DataMember]
        public string COMMUNE
        {
            get { return m_COMMUNE; }
            set { m_COMMUNE = value; }
        }
        private string m_RUE;
         [DataMember]
        public string RUE
        {
            get { return m_RUE; }
            set { m_RUE = value; }
        }
        private string m_NRUE;
         [DataMember]
        public string NRUE
        {
            get { return m_NRUE; }
            set { m_NRUE = value; }
        }
        private DateTime m_DMAJ;
         [DataMember]
        public DateTime DMAJ
        {
            get { return m_DMAJ; }
            set { m_DMAJ = value; }
        }
        private string m_TRANS;
         [DataMember]
        public string TRANS
        {
            get { return m_TRANS; }
            set { m_TRANS = value; }
        }
        private byte[] m_ROWID;
         [DataMember]
        public byte[] ROWID
        {
            get { return m_ROWID; }
            set { m_ROWID = value; }
        }
    }
}

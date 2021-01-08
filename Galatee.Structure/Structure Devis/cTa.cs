using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure 
{
    [DataContract]
    public class cTa
    {
		private string m_NUM;
        [DataMember]
		public string NUM
		{
			get { return m_NUM; }
			set { m_NUM = value; }
		}
		private string m_TRANS;
        [DataMember]
		public string TRANS
		{
			get { return m_TRANS; }
			set { m_TRANS = value; }
		}
		private string m_DMAJ;
        [DataMember]
		public string DMAJ
		{
			get { return m_DMAJ; }
			set { m_DMAJ = value; }
		}
		private string m_CENTRE;
        [DataMember]
		public string CENTRE
		{
			get { return m_CENTRE; }
			set { m_CENTRE = value; }
		}
		private string m_CODE;
        [DataMember]
		public string CODE
		{
			get { return m_CODE; }
			set { m_CODE = value; }
		}
		private string m_LIBELLE;
        [DataMember]
		public string LIBELLE
		{
			get { return m_LIBELLE; }
			set { m_LIBELLE = value; }
		}
		private string m_ROWID;
        [DataMember]
		public string ROWID
		{
			get { return m_ROWID; }
			set { m_ROWID = value; }
		}

        //public override string ToString()
        //{
        //    return LIBELLE;
        //}
    }
}

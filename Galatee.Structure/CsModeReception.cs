using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galatee.Structure
{
    public class CsModeReception
    {
        private byte _Id;
        private string _Libelle;
        private int _Pk_id;

        public byte Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        public string Libelle
        {
            get { return _Libelle; }
            set { _Libelle = value; }
        }
        public int pk_id
        {
            get { return _Pk_id; }
            set { _Pk_id = value; }
        }
        public override string ToString()
        {
            return this._Libelle;
        }
    }
}

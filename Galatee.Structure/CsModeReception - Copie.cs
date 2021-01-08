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
        public override string ToString()
        {
            return this._Libelle;
        }
    }
}

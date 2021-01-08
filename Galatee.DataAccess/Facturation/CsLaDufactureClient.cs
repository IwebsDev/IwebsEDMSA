using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class CsLaDufactureClient
    {
        public ENTFAC ENTETEFACTURE { get; set; }
        public List<PROFAC > PRODUITFACTURE { get; set; }
        public List<REDFAC > REDEVENCEFACTURE { get; set; }
    }
}

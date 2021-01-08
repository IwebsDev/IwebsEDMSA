using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure.Rpnt
{
    public class CstbParametres
    {

        public int Parametre_ID { get; set; }
        public int TypeValeur { get; set; }
        public string LibelleParametre { get; set; }
        public string Description { get; set; }
        public Nullable<int> ValeurGlobaleInt { get; set; }
        public string ValeurGlobaleChaine { get; set; }
        public Nullable<decimal> ValeurGlobaleDecimal { get; set; }
        public Nullable<int> Methode_ID { get; set; }

    }
}

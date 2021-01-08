using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Tools
{
    public class ADException:Exception
    {
        public ADException(string message) : base(message)
        { }
    }
}

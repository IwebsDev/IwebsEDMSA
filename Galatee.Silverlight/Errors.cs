using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Silverlight
{
    public class Errors
    {
        public int ErrorNumber { get; set; }

        public string ErrorLabel { get; set; }
    }

    public static class ErrorManager
    {
        public static string ExceptionSource(Exception ex)
        {
            try
            {
                //if (ex.InnerException != null)
                    return GetCurrentMethod();
                //else
                  //  return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(4); // 3 correspond au niveau de la méthode dans le StackTrace

            return sf.GetMethod().Name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Galatee.Structure;

namespace Galatee.DataAccess
{
    public class cContext
    {
        public class AppContext
        {   //  
            private static object _token = new object();
            private static AppContext _appContext;

            private AppContext()
            {
            }

            public static AppContext Instance
            {
                get
                {
                    if (_appContext == null)
                    {
                        lock (_token)
                        {
                            if (_appContext == null)
                            {
                                _appContext = new AppContext();
                            }
                        }
                    }
                    return _appContext;
                }
            }
        }
    }
}

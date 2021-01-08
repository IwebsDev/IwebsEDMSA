using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.ServiceFacturation;
using Galatee.Silverlight.ServiceAdministration ;
using Galatee.Silverlight.ServiceAccueil  ;
using Galatee.Silverlight.ServiceRecouvrement;

namespace Galatee.Silverlight
{
    public class AccesServiceWCF
    {
        private ChildWindow DefautWindow = new ChildWindow();
       static FacturationServiceClient _ClientFac = null;
       public static FacturationServiceClient  ClientFac
        {
            get
            {
                try
                {
                    if (_ClientFac == null)
                    {
                        //_Service = new ServiceDME();
                        _ClientFac = new FacturationServiceClient();
                    }

                    return _ClientFac;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
       public FacturationServiceClient GetFacturationClient()
         {
             try
             {
                 return new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

       static AcceuilServiceClient _ClientAcc = null;
       public static AcceuilServiceClient ClientAcc
       {
           get
           {
               try
               {
                   if (_ClientAcc == null)
                   {
                       //_Service = new ServiceDME();
                       _ClientAcc = new AcceuilServiceClient();
                   }

                   return _ClientAcc;
               }
               catch (Exception ex)
               {
                   throw ex;
               }
           }
       }
       public AcceuilServiceClient GetAcceuilClient()
       {
           try
           {
               return new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Acceuil"));
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }


       static AdministrationServiceClient  _ClientFact = null;
       public static AdministrationServiceClient ClientFact
       {
           get
           {
               try
               {
                   if (_ClientFact == null)
                   {
                       //_Service = new ServiceDME();
                       _ClientFact = new AdministrationServiceClient();
                   }

                   return _ClientFact;
               }
               catch (Exception ex)
               {
                   throw ex;
               }
           }
       }
       public AdministrationServiceClient GetAdministrationClient()
       {
           try
           {
               return new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }


       static RecouvrementServiceClient _ClientRecouv = null;
       public static RecouvrementServiceClient ClientRecouv
       {
           get
           {
               try
               {
                   if (_ClientRecouv == null)
                   {
                       //_Service = new ServiceDME();
                       _ClientRecouv = new RecouvrementServiceClient();
                   }

                   return _ClientRecouv;
               }
               catch (Exception ex)
               {
                   throw ex;
               }
           }
       }
       public RecouvrementServiceClient GetRecouvrementClient()
       {
           try
           {
               return new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }


    }
}

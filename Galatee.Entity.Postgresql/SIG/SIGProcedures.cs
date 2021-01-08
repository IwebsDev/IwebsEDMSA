using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Galatee.Structure;

namespace Galatee.Entity.Model.Reports
{
    public static class SIGProcedures
    {
        public static int ParseInt(string number)
        {
            return Int32.Parse(number);
        }

        /* En stand by */
        public static DataTable RetournerCoodonnesAbonne(CsClientRechercher client)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var Ag = context.AG ;


                    IEnumerable<object > localisations =
                        from a in Ag
                        from c in a.CLIENT1 
                        from b in a.BRT
                        where
                            a.CLIENT == client.CLIENT && a.CENTRE == client.CENTRE

                        select new 
                        {
                            Centre = b.CENTRE,
                            Latitude =b.LATITUDE,
                            Longitude = b.LONGITUDE,
                            NomAbonne = c.NOMABON,
                            NumeroClient = b.CLIENT,
                            Telephone =c.TELEPHONE                            
                        };
                    return Galatee.Tools.Utility.ListToDataTable(localisations);
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable RetournerCoodonnesAbonne(string  IDCOUPURE)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var AG = context.AG ;
                    var camp = context.DETAILCAMPAGNE ;
                    IEnumerable<object> localisations =
                        from a in AG
                        from b in a.BRT
                        from cl in a.CLIENT1
                        join d in camp on cl.PK_ID  equals d.FK_IDCLIENT 
                        where
                            d.IDCOUPURE == IDCOUPURE
                        select new
                        {
                            Centre = b.CENTRE,
                            Latitude = b.LATITUDE,
                            Longitude = b.LONGITUDE,
                            NomAbonne = cl.NOMABON,
                            NumeroClient = b.CLIENT,
                            Telephone = cl.TELEPHONE
                        };
                    return Galatee.Tools.Utility.ListToDataTable(localisations);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

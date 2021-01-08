using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Entity.Model
{
    public class EclairagePublicProcedures
    {
        
        public List<CsCredfac> GetCRedFacByRedevance(string centre, string periode)
        {
            try
            {
                using (ABO07Entities context = new ABO07Entities())
                {
                    List<CsCredfac> Credfac = Entities.ConvertObject<CsCredfac, CREDFAC>(context.CREDFAC.Where(r => r.REDEVANCE ==Enumere.Redevance).ToList());
                    if (!string.IsNullOrWhiteSpace( centre))
                    {
                        Credfac = Credfac.Where(r => r.CENTRE == centre).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace( periode))
                    {
                        Credfac = Credfac.Where(r => r.PERIODE == periode).ToList();
                    }
                    return Credfac;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galatee.Structure;
using Galatee.DataAccess;
using System.Data.SqlClient;
namespace WindowsFormsApplication1
{
    class ChargementDeDonne
    {
        private string ConnectionString;
        public ChargementDeDonne()
        {
            try
            {
                ConnectionString = Session.GetSqlConnexionString();
            }
            catch (Exception)
            {
                //Ckecking in test
                throw;
            }
        }

        public void ImprimerImage()
        {
         
            DBAccueil dt = new DBAccueil();
            List<ObjDOCUMENTSCANNE> leDocument = new List<ObjDOCUMENTSCANNE>();
            int nbr = 0;
            while (nbr == 0)
            {
                leDocument = new DBAccueil().Select_DocumentNonMigre();
                SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);

                foreach (ObjDOCUMENTSCANNE image in leDocument)
                {
                    dt.CreateFiles(image.CONTENU, image.CHEMINCOPY, image.NOMDUFICHIER);
                    image.CONTENU = null;
                    image.ismigre  = true;
                    dt.InsertOrUpdateDocumentScane(image, laCommande);
                }
                nbr =leDocument.Where(p=>p.CONTENU !=null ).Count();
                laCommande.Transaction.Commit();
            }
           
        }
    }
}

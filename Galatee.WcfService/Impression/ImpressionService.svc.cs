using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using Galatee.DataAccess;
using System.ServiceModel.Activation;
//using Microsoft.Reporting.WebForms;
using System.IO;
using System.Web.Hosting;
using System.Data.SqlClient;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "FacturationService" à la fois dans le code, le fichier svc et le fichier de configuration.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ImpressionService : IImpressionService
    {

        public bool? RetourneFacturesRegroupement(string regroupementDebut, string regroupementFin, List<string> LstPeriode, List<string> Produit, int? idcentre, string rdlc, Dictionary<string, string> parameters, string key)
        {

            try
            {
                DbFacturation db = new DbFacturation();
                List<CsFactureClient> lstFacture = new List<CsFactureClient>();
                lstFacture = db.RetourneFacturesRegroupement(regroupementDebut, regroupementFin, LstPeriode, Produit, idcentre, rdlc);
                List<CsPrint> listeDeFactures = new List<CsPrint>();
                foreach (CsFactureClient facture in lstFacture)
                    listeDeFactures.Add(facture);

                Printings.PrintingsService printService = new Printings.PrintingsService();
                return printService.setFromWebPart(listeDeFactures, key, parameters);
            }
            catch (Exception ex)
            {

                ErrorManager.LogException(this, ex);
                return null;
            }


        }
    }
}

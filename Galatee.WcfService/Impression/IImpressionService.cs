using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using Galatee.DataAccess;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IFacturationService" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IImpressionService
    {
        [OperationContract]
        [FaultContract(typeof(Errors))]
        bool? RetourneFacturesRegroupement(string regroupementDebut, string regroupementFin, List<string> LstPeriode, List<string> Produit, int? idcentre, string rdlc, Dictionary<string, string> parameters, string key);

    }
}

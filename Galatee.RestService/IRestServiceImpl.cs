using Galatee.RestService.ServiceIndex;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace RestService
{
    
    [ServiceContract]
    public interface IRestServiceImpl
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "xml/{id}")]
        string XMLData(string id);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "ListedesLot/json/{NumPortable}")]
        List<CsLotri> JSONData(string NumPortable);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "Evenements/json/{NumLot}/{Centre}/{Tournee}")]
        List<CsEvenement> JSON_EvenementsData(string NumLot, string Centre, string Tournee);



        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "Casind/json")]
        List<CsCasind> JSON_CasindData();


       [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "saveEvent/json/{Event}")]
        string JSON_saveEventData(string Event);

    }
}
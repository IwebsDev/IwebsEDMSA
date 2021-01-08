using Galatee.RestService.ServiceIndex;
using System;
using System.Collections.Generic;
namespace RestService
{
    public class RestServiceImpl : IRestServiceImpl
    {
        #region IRestServiceImpl Members

        public string XMLData(string id)
        {
            return "You requested product " + id;
        }

        public List<CsLotri> JSONData(string NumPortable)
        {
            IndexServiceClient service = new IndexServiceClient();
           return service.RetourneListeLotNonTraiteParReleveur(NumPortable);
        }
        public List<CsEvenement> JSON_EvenementsData(string NumLot, string Centre, string Tournee)
        {
            IndexServiceClient service = new IndexServiceClient();
            return service.RetourneListeLotSelonCritere(NumLot, Centre, Tournee);
            //return new List<CsEvenement>();
        }
        public List<CsCasind> JSON_CasindData()
        {
            IndexServiceClient service = new IndexServiceClient();
            return service.RetourneListeDesCas(string.Empty,string.Empty);
            //return new List<CsEvenement>();
        }
        public string JSON_saveEventData(string Event)
        {
            try
            {
                IndexServiceClient service = new IndexServiceClient();
                string[] Param=Event.Split(',');
                return service.MiseAJourEvenementTSP(Param[0], Param[1], Param[2], Param[3], Param[4], Param[5], Param[6], Param[7], Param[8], Param[9], Param[10]);
                //return Event;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }


        public List<CsCasind> JSON_teseData()
        {
            IndexServiceClient service = new IndexServiceClient();
            return service.RetourneListeDesCas(string.Empty, string.Empty);
            //return new List<CsEvenement>();
        }
        #endregion
    }
}
using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.DTO;
using NLog;

namespace WebApplication1.Controllers
{
    public class mapController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        igroup199_prodEntities db = new igroup199_prodEntities();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/map/{country}")]
        public HttpResponseMessage Map(string country)
        {

            try
            {
                List<Country_Dto> locations = db.Countries.Where(x => x.CountryName == country).Select(l => new Country_Dto
            {
                CountryName = l.CountryName,
                CountryLat = (float)l.CountryLat,
                CountryLon = (float)l.CountryLon,
                AttractionList = db.Attractions.Where(a => a.AttractionsLocationCountry == country).Select(at => new Attraction_Dto
                {
                    AttractionCountry = at.AttractionsLocationCountry,
                    AttractionsLatitude = (float)at.AttractionsLatitude,
                    AttractionsLongitude = (float)at.AttractionsLongitude
                }).ToList(),
                AidCompList = db.AidComplexes.Where(a => a.AidComplexesLocationCountry == country).Select(at => new AidCompleX_Dto
                {
                    AidCompCountry = at.AidComplexesLocationCountry,
                    AidCompLat = (float)at.AidComplexesLatitude,
                    AidCompLon = (float)at.AidComplexesLongitude
                }).ToList(),
                SleepingCompList = db.SleepingComplexes.Where(a => a.SleepingComplexesLocationCountry == country).Select(at => new SleepingComp_Dto
                {
                    SleepingCompCountry = at.SleepingComplexesLocationCountry,
                    SleepingCompLat = (float)at.SleepingComplexesLongitude,
                    SleepingCompLon = (float)at.SleepingComplexesLongitude
                }).ToList(),
                tripList = db.Trips.Where(z => z.TripsLocationCountry == country).Select(at => new Trips_Dto
                {
                    TripsName = at.TripsName,
                    TripsLocationCountry = at.TripsLocationCountry,
                    TripsLatitude = (float)at.TripsLatitude,
                    TripsLongitude = (float)at.TripsLongitude
                }).ToList()
            }).ToList();

            if (locations != null)
            {
                    logger.Info($"country list was shown \n statusCode:{HttpStatusCode.OK}");
                    return Request.CreateResponse(HttpStatusCode.OK, locations);
            }
                logger.Error($" country list was not found \n statusCode:{HttpStatusCode.BadRequest}");
                return Request.CreateResponse(HttpStatusCode.NotFound, $"{country} was not found in the database.");
            }
            catch (Exception)
            {
                logger.Error($"something went worng! \n statusCode:{HttpStatusCode.NotFound}");
                return Request.CreateResponse(HttpStatusCode.NotFound);//יחזור סטטוס קוד 404 ללא שליחת אובייקט
            }
        }

    }
    
}
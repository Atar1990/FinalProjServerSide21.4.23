using Antlr.Runtime.Misc;
using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using WebApplication1.DTO;
using NLog;

namespace WebApplication1.Controllers
{
    public class FeedbackController : ApiController
    {
        igroup199_prodEntities db = new igroup199_prodEntities();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        [HttpGet]
        [Route("api/Feedback")] // הדפסת כל הפידבקים
        public IHttpActionResult Getall()
        {
            try
            {
                List<Feedback> feedbacks = db.Feedbacks.ToList();
                List<FeedbackDTO> feedbackDTOs = new List<FeedbackDTO>();
                foreach (Feedback item in feedbacks)
                {
                    feedbackDTOs.Add(new FeedbackDTO
                    {
                        FeedbackKey = item.FeedbackKey,
                        KindOfFeedback = item.KindOfFeedback,
                        FeedbackDescription = item.FeedbackDescription
                    });
                }
                logger.Info($"all feedbacks was shown \n statusCode:{HttpStatusCode.OK}");
                return Ok(feedbackDTOs);
            }
            catch (Exception)
            {
                logger.Error($"something went wrong! cant be found feedbackes \n statusCode:{HttpStatusCode.NotFound}");
                return NotFound(); 
            }
        }

        [HttpGet]
        [Route("api/Feedback/{FeedbackKey}/GetByName")] // הדפסת פידבק ספציפי
        public IHttpActionResult GetByName(int FeedbackKey)
        {
            try
            {
                List<Feedback> feedbacks = db.Feedbacks.ToList();
                List<FeedbackDTO> feedbackDTOs = new List<FeedbackDTO>();
                foreach (var item in feedbacks)
                {
                    if (item.FeedbackKey == FeedbackKey)
                    {
                        feedbackDTOs.Add(new FeedbackDTO
                        {
                            FeedbackKey = item.FeedbackKey,
                            KindOfFeedback = item.KindOfFeedback,
                            FeedbackDescription = item.FeedbackDescription
                        });
                    }
                }
                logger.Info($"feedback was found \n statusCode:{HttpStatusCode.OK}");
                return Ok(feedbackDTOs);
            }
            catch (Exception)
            {
                logger.Error($"something went wrong! cant be found feedbacke \n statusCode:{HttpStatusCode.NotFound}");
                return NotFound();
            }
        }

        [HttpPost]
        [Route("api/Feedback/{feedbackINTindb}/PostNew")] // הוספת הפידבק לטבלה הרצויה
        public IHttpActionResult PostNew(int feedbackINTindb)
        {
            var feedbacksepc = db.Feedbacks.Where(x => x.FeedbackKey == feedbackINTindb).SingleOrDefault();
            logger.Info($"feedback was found \n statusCode:{HttpStatusCode.OK}");
            int maxFevKey = db.UserFavourites.Max(x => x.FavouritesKey);
            int maxOptKey = db.Options.Max(x => x.OptionsKey);
            int maxAtrrKey = db.Attractions.Max(x => x.AttractionsKey);
            int maxTripKey = db.Trips.Max(x => x.TripsKey);
            int maxAidCom = db.AidComplexes.Max(x => x.AidComplexesKey);
            int maxSleeAidKey = db.SleepingComplexes.Max(x => x.SleepingComplexesKey);

            try
            {
                var userfevoritenew = new UserFavourite
                {
                    FavouritesKey = maxFevKey + 1,
                    UserEmail = "noycn27@gmail.com",
                    CountryName = feedbacksepc.FeedbackCountry,
                    UserFavouritesRegionOfTheCountry = feedbacksepc.FeedbackRegionOfTheCountry,
                    Titel = feedbacksepc.FeedbackTitle,
                    Description = feedbacksepc.KindOfFeedback
                };
                db.UserFavourites.Add(userfevoritenew);
                var newoption = new Option
                {
                    OptionsKey = maxOptKey + 1,
                    FavouritesKey = userfevoritenew.FavouritesKey,
                    OptionsLikeDislike = "L",
                    OptionName = feedbacksepc.FeedbackTitle,
                    OptionLocationCountry = feedbacksepc.FeedbackCountry,
                    OptionsRegionOfTheCountry = feedbacksepc.FeedbackCountry,
                };
                db.Options.Add(newoption);
                if (feedbacksepc.KindOfFeedback == "Attractions")
                {
                    var newattraction = new Attraction
                    {
                        AttractionsKey = maxAtrrKey + 1,
                        OptionsKey = newoption.OptionsKey,
                        KindOfAttraction = "omega",
                        AttractionsTimeInMinutes = 0,
                        AttractionsCost = 0,
                        NumberOfRepeatAttractions = 1,
                        AttractionsName = feedbacksepc.FeedbackTitle,
                        AttractionsLocationCountry = newoption.OptionLocationCountry,
                        AttractionsRegionOfTheCountry = newoption.OptionsRegionOfTheCountry,
                        AttractionsLongitude = feedbacksepc.FeedbackLongitude,
                        AttractionsLatitude = feedbacksepc.FeedbackLatitude,
                        AttractionsFinallandmark = (float)1.1,
                    };
                    db.Attractions.Add(newattraction);
                }
                if (feedbacksepc.KindOfFeedback == "Trips")
                {
                    var newattrip = new Trip
                    {
                        TripsKey = maxTripKey + 1,
                        OptionsKey = newoption.OptionsKey,
                        KindOfTrips = "climbing",
                        TripsTimeInMinutes = 0,
                        TripsCost = 0,
                        NumberOfRepeatTrips = 1,
                        TripsName = feedbacksepc.FeedbackTitle,
                        TripsLocationCountry = newoption.OptionLocationCountry,
                        TripsRegionOfTheCountry = newoption.OptionsRegionOfTheCountry,
                        TripsLongitude = feedbacksepc.FeedbackLongitude,
                        TripsLatitude = feedbacksepc.FeedbackLatitude,
                        TripsFinallandmark = (float)1.1,
                    };
                    db.Trips.Add(newattrip);
                }
                if (feedbacksepc.KindOfFeedback == "AidComplexes")
                {
                    var newAidComplexes = new AidComplex
                    {
                        AidComplexesKey = maxAidCom + 1,
                        OptionsKey = newoption.OptionsKey,
                        AidComplexesOpenHours = "24/7",
                        AidComplexesPhone = 0,
                        AidComplexesName = feedbacksepc.FeedbackTitle,
                        AidComplexesLocationCountry = feedbacksepc.FeedbackRegionOfTheCountry,
                        AidComplexesLongitude = feedbacksepc.FeedbackLongitude,
                        AidComplexesLatitude = feedbacksepc.FeedbackLatitude,
                        AidComplexesFinallandmark = (float)1.1,
                    };
                    db.AidComplexes.Add(newAidComplexes);
                }
                if (feedbacksepc.KindOfFeedback == "SleepingComplexes")
                {
                    var newSleepingComplexes = new SleepingComplex
                    {
                        SleepingComplexesKey = maxSleeAidKey + 1,
                        OptionsKey = newoption.OptionsKey,
                        SleepingComplexesCost = 0,
                        NumberOfNightsSleepingComplexes = 1,
                        SleepingComplexesName = "admin",
                        SleepingComplexesLocationCountry = newoption.OptionLocationCountry,
                        SleepingComplexesRegionOfTheCountry = newoption.OptionsRegionOfTheCountry,
                        SleepingComplexesLongitude = feedbacksepc.FeedbackLongitude,
                        SleepingComplexesLatitude = feedbacksepc.FeedbackLatitude,
                        SleepingComplexesFinallandmark = (float)1.1,
                    };
                    db.SleepingComplexes.Add(newSleepingComplexes);
                }
                db.SaveChanges();
                logger.Info($"feedback was found \n statusCode:{HttpStatusCode.OK}");
                return Ok();
            }
            catch (Exception e)
            {
                logger.Error($"something went wrong! cant be found feedbacke to add to DB \n statusCode:{HttpStatusCode.NotFound}");
                return NotFound();
            }
        }

        public void Put(int id, [FromBody] string value)
        {

        }

        [HttpDelete]
        [Route("api/Feedback/DeleteFeedback/{feedbackIDfromdb}")] // מחיקת פידבק ספציפי
        public IHttpActionResult DeleteFeedback(int feedbackIDfromdb)
        {
            try
            {
                Feedback feedbackToDelete = db.Feedbacks.Where(x => x.FeedbackKey == feedbackIDfromdb).FirstOrDefault();
                if (feedbackIDfromdb == 0)
                {
                    logger.Error($" cant be found feedback \n statusCode:{HttpStatusCode.NotFound}");
                    return BadRequest("Invalid feedback name value.");
                }
                logger.Info($"feedback to delete was found \n statusCode:{HttpStatusCode.OK}");
                db.Feedbacks.Remove(feedbackToDelete);
                db.SaveChanges();
                logger.Info($"feedback  was deleted from DB \n statusCode:{HttpStatusCode.OK}");
                return Ok();
            }
            catch (Exception)
            {
                logger.Error($"something went wrong! cant be found feedbacke to delete \n statusCode:{HttpStatusCode.NotFound}");
                return NotFound();
            }
        }
    }
}
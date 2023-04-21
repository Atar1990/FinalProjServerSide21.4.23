using ClassLibrary1;
using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApplication1.DTO;
using NLog;

namespace WebApplication1.Controllers
{
    public class TravelDiaryChapterController : ApiController
    {
        igroup199_prodEntities db = new igroup199_prodEntities();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        // GET: api/TravelDiaryChapter
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet] //האימייל של המשתמש אני אקבל מההתחברות של היוזר עצמו
        [Route("api/traveldiary/{useremailinput}/chapters")] //הדפסת פרק לפי מייל ספציפי
        public IHttpActionResult GetTravelDiaryChapters(string useremailinput)
        {
            try
            {
                var userkey = db.TravelDiaries.Where(x => x.UserEmail == useremailinput).FirstOrDefault();
                logger.Info($"user can be found in DB \n statusCode:{HttpStatusCode.OK}");
                if (userkey.TravelDiaryKey <= 0)
                {
                    logger.Error($"user cant be found in DB \n statusCode:{HttpStatusCode.NotFound}");
                    return BadRequest("Invalid TravelDiaryKey value.");
                }
                List<Chapter> travelDiaries = db.Chapters.Where(td => td.TravelDiaryKey == userkey.TravelDiaryKey).ToList();
                if (travelDiaries == null || travelDiaries.Count == 0)
                {
                    logger.Error($"travelDiaries cant be found in DB \n statusCode:{HttpStatusCode.NotFound}");
                    return NotFound();
                }
                List<TravelDiaryChapterDTO> result = new List<TravelDiaryChapterDTO>();
                foreach (Chapter td in travelDiaries)
                {
                    result.Add(new TravelDiaryChapterDTO
                    {
                        NameOfChapter = td.NameOfChapter,
                        ChapterDescription = td.ChapterDescription,
                        ChapterPictures = td.ChapterPictures,
                        ChapterDate = (DateTime)td.ChapterDate,
                        ChapterTime = (TimeSpan)td.ChapterTime,
                    }); ;
                }
                logger.Info($"chapters from DB has been shows \n statusCode:{HttpStatusCode.OK}");
                return Ok(result);
            }
            catch (Exception)
            {
                logger.Error($"somthing went worng!\n statusCode:{HttpStatusCode.BadRequest}");
                return NotFound();
            }
        }

        [HttpGet] //האימייל של המשתמש אני אקבל מההתחברות של היוזר עצמו
        [Route("api/traveldiary/{useremailinput}/GetTravelDiaryChaptersById/{chaptername}")] //  ושם הפרק הדפסת פרק לפי מייל ספציפי
        public IHttpActionResult GetTravelDiaryChaptersById(string useremailinput, string chaptername)
        {
            try
            {
                var userkey = db.TravelDiaries.Where(x => x.UserEmail == useremailinput).FirstOrDefault();
                if (userkey.TravelDiaryKey <= 0)
                {
                    logger.Error($"invalid travel diary key from DB \n statusCode:{HttpStatusCode.NotFound}");
                    return BadRequest("Invalid TravelDiaryKey value.");
                }
                logger.Info($"travel diary key from DB has been found \n statusCode:{HttpStatusCode.OK}");
                Chapter c = db.Chapters.Where(x => x.NameOfChapter == chaptername && x.TravelDiaryKey == userkey.TravelDiaryKey).FirstOrDefault();
                List<TravelDiaryChapterDTO> dt = new List<TravelDiaryChapterDTO>();
                dt.Add(new TravelDiaryChapterDTO
                {
                    NameOfChapter = c.NameOfChapter,
                    ChapterDescription = c.ChapterDescription,
                    ChapterPictures = c.ChapterPictures,
                    ChapterDate = (DateTime)c.ChapterDate,
                    ChapterTime = (TimeSpan)c.ChapterTime,
                    TravelDiaryKey = c.TravelDiaryKey
                });
                logger.Info($"chapter from DB has been shows \n statusCode:{HttpStatusCode.OK}");
                return Ok(dt);
            }
            catch (Exception)
            {
                logger.Error($"somthing went worng!\n statusCode:{HttpStatusCode.BadRequest}");
                return NotFound();
            }

        }

        [HttpPut] //שינוי תאור פרק
        [Route("api/traveldiary/PutUpdate/{nameofchapterfromdb}/")]
        public HttpResponseMessage PutUpdate(string nameofchapterfromdb, [FromBody] TravelDiaryChapterDTO newchap)
        {
            try
            {
                string CapterNameInput = nameofchapterfromdb;
                var CapterParameters = db.Chapters.Where(x => x.NameOfChapter == CapterNameInput).FirstOrDefault();
                logger.Info($"chapter has been found \n statusCode:{HttpStatusCode.OK}");
                Chapter newchapter = new Chapter();
                if (CapterParameters.ChapterDescription != newchap.ChapterDescription)
                {
                    CapterParameters.ChapterDescription = newchap.ChapterDescription;
                }
                if (CapterParameters.ChapterPictures != newchap.ChapterPictures)
                {
                    CapterParameters.ChapterPictures = newchap.ChapterPictures;
                }
                if (CapterParameters.ChapterDate != newchap.ChapterDate)
                {
                    CapterParameters.ChapterDate = newchap.ChapterDate;
                }
                if (CapterParameters.ChapterTime != newchap.ChapterTime)
                {
                    CapterParameters.ChapterTime = newchap.ChapterTime;
                }
                if (CapterParameters.NameOfChapter != newchap.NameOfChapter)
                {
                    newchapter = new Chapter
                    {
                        NameOfChapter = newchap.NameOfChapter,
                        ChapterDescription = CapterParameters.ChapterDescription,
                        ChapterPictures = CapterParameters.ChapterPictures,
                        TravelDiaryKey = CapterParameters.TravelDiaryKey,
                        ChapterDate = CapterParameters.ChapterDate,
                        ChapterTime = CapterParameters.ChapterTime,
                    };
                    db.Chapters.Add(newchapter);
                    db.Chapters.Remove(CapterParameters);
                }
                db.SaveChanges();
                logger.Info($"chapter has been update \n statusCode:{HttpStatusCode.OK}");
                return Request.CreateResponse(HttpStatusCode.OK, newchapter.NameOfChapter);// שליחת סטטוס קוד 200 + את שם פרק החדש 
            }
            catch (Exception e)
            {
                logger.Error($"somthing went worng!\n statusCode:{HttpStatusCode.BadRequest}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, "somting wrong!\n" + e.Message.ToString());// נפילה - שליחת שגיאה
            }
        }

        [HttpDelete] // מחיקת פרק
        [Route("api/traveldiary/{useremailinput}/deletechapter/{NameOfChapter}")]
        public IHttpActionResult DeleteChapter(string useremailinput, string NameOfChapter)
        {
            try
            {
                var userkey = db.TravelDiaries.Where(x => x.UserEmail == useremailinput).FirstOrDefault();
                if (userkey.TravelDiaryKey <= 0)
                {
                    logger.Error($"invalid travel diary key from DB \n statusCode:{HttpStatusCode.NotFound}");
                    return BadRequest("Invalid TravelDiaryKey value.");
                }
                if (string.IsNullOrEmpty(NameOfChapter))
                {
                    logger.Error($"invalid Name Of Chapter from DB \n statusCode:{HttpStatusCode.NotFound}");
                    return BadRequest("Invalid NameOfChapter value.");
                }
                logger.Info($"chapter to delete has been found \n statusCode:{HttpStatusCode.OK}");
                Chapter chapter = db.Chapters.FirstOrDefault(td => td.TravelDiaryKey == userkey.TravelDiaryKey && td.NameOfChapter == NameOfChapter);
                if (chapter == null)
                {
                    logger.Error($"chapter is not exsist in DB \n statusCode:{HttpStatusCode.NotFound}");
                    return NotFound();
                }

                db.Chapters.Remove(chapter);
                List<TravelDiary> tr = db.TravelDiaries.ToList();
                foreach (var item in tr)
                {
                    if (item.TravelDiaryKey == userkey.TravelDiaryKey)
                    {
                        item.NumberOfChapters--;
                    }
                }
                db.SaveChanges();
                logger.Info($"chapter has been removed \n statusCode:{HttpStatusCode.OK}");
                return Ok(chapter);
            }
            catch (Exception)
            {
                logger.Error($"somthing went worng!\n statusCode:{HttpStatusCode.BadRequest}");
                return NotFound();
            }
        }

        [HttpPost] // הוספת פרק
        [Route("api/traveldiary/PostCAP/{userImput}/")]
        public IHttpActionResult PostCAP(string userImput, [FromBody] Chapter newchap)
        {
            var useremailinput = db.TravelDiaries.Where(x => x.UserEmail == userImput).FirstOrDefault();
            logger.Info($"user has been found \n statusCode:{HttpStatusCode.OK}");
            if (useremailinput.TravelDiaryKey <= 0)
            {
                logger.Error($"Invalid Travel Diary Key in DB \n statusCode:{HttpStatusCode.NotFound}");
                return BadRequest("Invalid TravelDiaryKey value.");
            }
            var chapterNew = new Chapter();
            try
            {
                chapterNew.TravelDiaryKey = useremailinput.TravelDiaryKey;
                chapterNew.NameOfChapter = newchap.NameOfChapter;
                chapterNew.ChapterDescription = newchap.ChapterDescription;
                chapterNew.ChapterPictures = newchap.ChapterPictures;
                chapterNew.ChapterDate = newchap.ChapterDate;
                chapterNew.ChapterTime = newchap.ChapterTime;

                if (chapterNew != null)
                {
                    db.Chapters.Add(chapterNew);
                    List<TravelDiary> tr = db.TravelDiaries.ToList();
                    foreach (var item in tr)
                    {
                        if (item.TravelDiaryKey == useremailinput.TravelDiaryKey)
                        {
                            item.NumberOfChapters++;
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                logger.Error($"somthing went worng!\n statusCode:{HttpStatusCode.BadRequest}");
                return NotFound();
            }
            logger.Info($"chapter has been added \n statusCode:{HttpStatusCode.OK}");
            return Ok();
        }
    }
}
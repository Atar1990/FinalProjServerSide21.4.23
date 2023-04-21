using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NLog;
using WebApplication1.DTO;

namespace WebApplication1.Controllers
{
    public class ExpensesController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        igroup199_prodEntities db = new igroup199_prodEntities();

        [HttpGet]
        [Route("api/expenses/{ExpensesKey}")]
        public HttpResponseMessage Getall(int ExpensesKey)
        {
            //ExpensesDTO expens = new ExpensesDTO(); // הכנת אובקייט מסוג הוצאה שטוח
            ExpensesDTO expense = new ExpensesDTO();// הכנת רשימת אובייקטים מסוג משתמש שטוח
            try
            {
                int numOfExpenses2 = db.Expenses.Max(x => x.ExpensesKey);//// מה הקוד הגדול ביותר 


                if (numOfExpenses2 < ExpensesKey)// אם מספר ההוצאה שהתקבל למחיקה גדול מהמספר של הפתח הגדול ביותר שנשמר עד כה - זרוק שגיאה מתאימה
                {
                    logger.Error($"user try to get expose from DB\n ExpensesKey is {ExpensesKey}- too big \n statusCode:{HttpStatusCode.BadRequest}");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "the number of expens is undifinde");// נפילה - שליחת שגיאה
                }

                expense = db.Expenses.Where(e => e.ExpensesKey == ExpensesKey).Select(e => new ExpensesDTO
                {
                    ExpensesKey = e.ExpensesKey,
                    UserEmail = e.UserEmail,
                    KindOfExpenses = e.KindOfExpenses,
                    ExpensesTitle = e.ExpensesTitle,
                    PricePerOne = e.PricePerOne,
                    NumberOfRepeatExpenses = e.NumberOfRepeatExpenses,
                    TotalPriceToPay = e.TotalPriceToPay

                }).SingleOrDefault();// אם נמצא נחזיר, אם לא נמצא אנדיפיינד

            }
            catch (Exception e)
            {
                logger.Warn(e, $"user try get expose from DB \n statusCode:{HttpStatusCode.BadRequest}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, "somting wrong!" + e.Message.ToString());// נפילה - שליחת שגיאה

            }

            if (expense != null)
            {
                logger.Info($"user get expose by number from DB\n ExpensesKey is {ExpensesKey} \n statusCode:{HttpStatusCode.OK}");
                return Request.CreateResponse(HttpStatusCode.OK, expense);// שליחת סטטוס קוד 200 + את כל רשימת הההוצאות של אותו משתמש
            }
            logger.Error($"user get expose by number from DB\n ExpensesKey is {ExpensesKey}- expose num was deleted \n statusCode:{HttpStatusCode.NotFound}");
            return Request.CreateResponse(HttpStatusCode.NotFound, "the number of expens is deleted");// אם כתובת המייל לא נמצא במשתמשי המערכת שלנו, יחזור סטטוס קוד 404 ללא שליחת אובייקט

        }




        // GET api/<controller>/5
        //'http://localhost:65095/api/expenses/?email=Benda669@gmail.com'
        [HttpGet]
        [Route("api/expenses/")]
        public HttpResponseMessage GetE(string email)
        {
            //ExpensesDTO expens = new ExpensesDTO(); // הכנת אובקייט מסוג הוצאה שטוח
            List<ExpensesDTO> expenses = new List<ExpensesDTO>();// הכנת רשימת אובייקטים מסוג משתמש שטוח
            try
            {
                expenses = db.Expenses.Where(e => e.UserEmail == email).Select(e => new ExpensesDTO
                {
                    ExpensesKey = e.ExpensesKey,
                    UserEmail = e.UserEmail,
                    KindOfExpenses = e.KindOfExpenses,
                    ExpensesTitle = e.ExpensesTitle,
                    PricePerOne = e.PricePerOne,
                    NumberOfRepeatExpenses = e.NumberOfRepeatExpenses,
                    TotalPriceToPay = e.TotalPriceToPay

                }).ToList();// הבאת כל המשתמשים מהדאטה בייס שלנו והשמה ברשימה משתמשים שטוחים

            }
            catch (Exception e)
            {
                logger.Warn(e, $"user try get expose from DB \n statusCode:{HttpStatusCode.BadRequest}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, "somting wrong!\n" + e.Message.ToString());// נפילה - שליחת שגיאה
            }

            if (expenses != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, expenses);// שליחת סטטוס קוד 200 + את כל רשימת הההוצאות של אותו משתמש
            }
            logger.Error($"user try to get all exposes from DB\n statusCode:{HttpStatusCode.NotFound}");
            return Request.CreateResponse(HttpStatusCode.NotFound, " ");// אם כתובת המייל לא נמצא במשתמשי המערכת שלנו, יחזור סטטוס קוד 404 ללא שליחת אובייקט

        }

        [HttpGet]
        [Route("api/expenses/getsumof/")]
        public HttpResponseMessage GetSumOfExpenses(string email)
        {
            List<ExpensesDTO> expenses = new List<ExpensesDTO>();// הכנת רשימת אובייקטים מסוג משתמש שטוח
            int sumOfExpense = 0;
            try
            {
                expenses = db.Expenses.Where(e => e.UserEmail == email).Select(e => new ExpensesDTO
                {
                    ExpensesKey = e.ExpensesKey,
                    UserEmail = e.UserEmail,
                    KindOfExpenses = e.KindOfExpenses,
                    ExpensesTitle = e.ExpensesTitle,
                    PricePerOne = e.PricePerOne,
                    NumberOfRepeatExpenses = e.NumberOfRepeatExpenses,
                    TotalPriceToPay = e.TotalPriceToPay

                }).ToList();

                if (expenses != null)
                {
                    sumOfExpense = expenses.Sum(e => e.TotalPriceToPay);
                }
                else
                {
                    logger.Error($"sum of exposes faild- user email is undifind\n statusCode:{HttpStatusCode.NotFound}");
                    return Request.CreateResponse(HttpStatusCode.NotFound, "email is undifind");// אם כתובת המייל לא נמצא במשתמשי המערכת שלנו, יחזור סטטוס קוד 404 ללא שליחת אובייקט
                }
            }
            catch (Exception e)
            {
                logger.Warn(e, $"sum of exposes faild \n statusCode:{HttpStatusCode.BadRequest}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, "somting wrong!\n" + e.Message.ToString());// נפילה - שליחת שגיאה
            }

            if (0 < sumOfExpense)
            {
                return Request.CreateResponse(HttpStatusCode.OK, sumOfExpense);// שליחת סטטוס קוד 200 + את כל רשימת הההוצאות של אותו משתמש
            }
            logger.Error($"sum of exposes under zere\n statusCode:{HttpStatusCode.BadRequest}");
            return Request.CreateResponse(HttpStatusCode.BadRequest, "plese check you expense list in the application");// שגיאה בחישוב הסכום שמביאה לצואה שלילת

        }



        // POST api/<controller>
        [HttpPost]
        [Route("api/expenses/Postexpence")]
        public HttpResponseMessage Postexpence([FromBody] ExpensesDTO expense)
        {
            //List<ExpensesDTO> expenses = new List<ExpensesDTO>();// הכנת רשימת אובייקטים מסוג משתמש שטוח
            var NewExpenses = new Expens();// יצירת משתנה הוצאה - החזקה באובייקט כללי 
            try
            {

                // int lengthOf = db.Expenses.Count();// כמה שורות יש בטבלה
                int lengthOf2 = db.Expenses.Max(x => x.ExpensesKey);//// מה הקוד הגדול ביותר 

                NewExpenses.UserEmail = expense.UserEmail;
                NewExpenses.PricePerOne = expense.PricePerOne;
                NewExpenses.NumberOfRepeatExpenses = expense.NumberOfRepeatExpenses;
                NewExpenses.ExpensesTitle = expense.ExpensesTitle;
                NewExpenses.KindOfExpenses = expense.KindOfExpenses;
                //NewExpenses.ExpensesKey = lengthOf + 1;/// לפי כמות השורות שיש בטבלה פלוס אחד
                NewExpenses.ExpensesKey = lengthOf2 + 1;/// השיטה הקודמת לא מספיק טובה מכיוון שלאחר שתימחק הוצאה מאמצע הרשימה המיקום לא יהיה רלוונטי, בגלל שהמספר רץ השיטה הזו עדיפה
                NewExpenses.TotalPriceToPay = expense.TotalPriceToPay;

                if (NewExpenses.UserEmail != null)
                {
                    db.Expenses.Add(NewExpenses);// הוספה לדאטה בייס את האובייקט החדש שהתקבל
                    db.SaveChanges();
                    logger.Info($"add expose to DB by user \n statusCode:{HttpStatusCode.OK}");

                    return Request.CreateResponse(HttpStatusCode.OK, NewExpenses);// שליחת סטטוס קוד 200 + את כל רשימת הההוצאות של אותו משתמש
                }

                logger.Error($"user try to add expose to DB \n statusCode:{HttpStatusCode.NotFound}");
                return Request.CreateResponse(HttpStatusCode.NotFound, "");// אם כתובת המייל לא נמצא במשתמשי המערכת שלנו או שלא נשלחה בכלל, יחזור סטטוס קוד 404 ללא שליחת אובייקט

            }
            catch (Exception e)
            {
                logger.Warn(e, $"user try toadd expose to DB \n statusCode:{HttpStatusCode.BadRequest}");

                return Request.CreateResponse(HttpStatusCode.BadRequest, "somting wrong!\n" + e.Message.ToString());// נפילה - שליחת שגיאה
            }
        }

        // PUT api/<controller>/5
        //public void Put(int id, string value)
        //{
        //}
        [HttpPut]
        [Route("api/expenses/PutExpence/{ExpensesKey}")]
        public HttpResponseMessage PutExpence(int ExpensesKey, [FromBody] ExpensesDTO expense)
        {
            var NewExpenses = new Expens();// יצירת משתנה הוצאה - החזקה באובייקט כללי 
            var ExpenseToUpdate = db.Expenses.Where(x => x.ExpensesKey == ExpensesKey).SingleOrDefault();

            try
            {

                ExpenseToUpdate.UserEmail = expense.UserEmail;
                ExpenseToUpdate.PricePerOne = expense.PricePerOne;
                ExpenseToUpdate.NumberOfRepeatExpenses = expense.NumberOfRepeatExpenses;
                ExpenseToUpdate.ExpensesTitle = expense.ExpensesTitle;
                ExpenseToUpdate.KindOfExpenses = expense.KindOfExpenses;
                //NewExpenses.ExpensesKey = lengthOf + 1;/// לפי כמות השורות שיש בטבלה פלוס אחד
                //ExpenseToUpdate.ExpensesKey = ExpensesKey;/// מעודכן על ידי 
                ExpenseToUpdate.TotalPriceToPay = expense.TotalPriceToPay;

                if (ExpenseToUpdate != null)/// מספר ההוצאה לשינוי
                {
                    db.SaveChanges();
                    logger.Info($"expose changed by user \n expensNumber:{ExpensesKey}\n statusCode:{HttpStatusCode.OK}");
                    return Request.CreateResponse(HttpStatusCode.OK, ExpenseToUpdate.ExpensesKey);// שליחת סטטוס קוד 200 + את ההוצאה שעודכנה
                }
                logger.Error($"user try to change is expose \n expensNumber:{ExpensesKey}\n statusCode:{HttpStatusCode.NotFound}");
                return Request.CreateResponse(HttpStatusCode.NotFound, " ");// אם כתובת המייל לא נמצא במשתמשי המערכת שלנו, יחזור סטטוס קוד 404 ללא שליחת אובייקט

            }
            catch (Exception e)
            {
                logger.Warn(e, $"user try to change is expose \n expensNumber:{ExpensesKey}\n statusCode:{HttpStatusCode.BadRequest}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, "somting wrong!\n" + e.Message.ToString());// נפילה - שליחת שגיאה
            }

        }


        // DELETE api/<controller>/5
        [HttpDelete]
        [Route("api/expenses/Deleteexpence/{ExpensesKey}")]
        public HttpResponseMessage Deleteexpence(int ExpensesKey)
        {
            Expens expens = new Expens();// הכנת אובייקט למחיקה
            ExpensesDTO expensDTO = new ExpensesDTO();// הכנת רשימת אובייקטים מסוג משתמש שטוח
            try
            {
                int numOfExpenses2 = db.Expenses.Max(x => x.ExpensesKey);//// מה הקוד הגדול ביותר 

                //int numOfExpenses = db.Expenses.Count();/// כמה שורות של הוצאות יש בטבלה

                if (numOfExpenses2 < ExpensesKey)// אם מספר ההוצאה שהתקבל למחיקה גדול מהמספר של הפתח הגדול ביותר שנשמר עד כה - זרוק שגיאה מתאימה
                {
                    logger.Error($"user try to delete expose \n expensNumber:{ExpensesKey} -the number of expens is undifinde\n statusCode:{HttpStatusCode.NotFound}");

                    return Request.CreateResponse(HttpStatusCode.BadRequest, "the number of expens is undifinde");// נפילה - שליחת שגיאה
                }
                expensDTO = db.Expenses.Where(e => e.ExpensesKey == ExpensesKey).Select(e => new ExpensesDTO
                {
                    ExpensesKey = e.ExpensesKey,
                    UserEmail = e.UserEmail,
                    KindOfExpenses = e.KindOfExpenses,
                    ExpensesTitle = e.ExpensesTitle,
                    PricePerOne = e.PricePerOne,
                    NumberOfRepeatExpenses = e.NumberOfRepeatExpenses,
                    TotalPriceToPay = e.TotalPriceToPay

                }).SingleOrDefault();

                expens = db.Expenses.Where(e => e.ExpensesKey == ExpensesKey).SingleOrDefault();
                if (expens != null)
                {
                    db.Expenses.Remove(expens);
                    db.SaveChanges();
                    logger.Info($"expose delete by user \n expensNumber:{ExpensesKey}\n statusCode:{HttpStatusCode.OK}");

                    return Request.CreateResponse(HttpStatusCode.OK, expensDTO);// שליחת סטטוס קוד 200 + את כל רשימת הההוצאות של אותו משתמש
                }
                logger.Error($"user try to delete expose \n expensNumber:{ExpensesKey} -the number of expens is undifinde\n statusCode:{HttpStatusCode.NotFound}");
                return Request.CreateResponse(HttpStatusCode.NotFound, " ");// אם בסינגל או דיפולט קיבלנו NULL משמה אין ערך למחיקה במפתח שהתקבל

            }
            catch (Exception e)
            {
                logger.Warn(e, $"user try to deleted  expose \n expensNumber:{ExpensesKey}\n statusCode:{HttpStatusCode.BadRequest}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, "somting wrong!" + e.Message.ToString());// נפילה - שליחת שגיאה
            }
        }
    }
}
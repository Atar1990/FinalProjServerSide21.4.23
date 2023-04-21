using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.DTO
{
    public class FeedbackDTO
    {
        public int FeedbackKey { get; set; }
        public string KindOfFeedback { get; set; }
        public string FeedbackDescription { get; set; }
        public float FeedbackLongitude { get; set; }
        public float FeedbackLatitude { get; set; }
        public string FeedbackCountry { get; set; }
        public string FeedbackRegionOfTheCountry { get; set; }
        public string FeedbackTitle { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.DTO
{
    public class Country_Dto
    {
        public string CountryName { get; set; }
        public string CountryMainland { get; set; }
        public float CountryLat { get; set; }
        public float CountryLon { get; set; }
        public List<Attraction_Dto> AttractionList { get; set; }
        public List<SleepingComp_Dto> SleepingCompList { get; set; }
        public List<AidCompleX_Dto> AidCompList { get; set; }
        public List<Trips_Dto> tripList { get; set; }
    }

}
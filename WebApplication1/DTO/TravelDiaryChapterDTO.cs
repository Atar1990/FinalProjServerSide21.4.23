using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.DTO
{
    public class TravelDiaryChapterDTO
    {
        public string NameOfChapter { get; set; }
        public int TravelDiaryKey { get; set; }
        public string ChapterDescription { get; set; }
        public string ChapterPictures { get; set; }
        public DateTime ChapterDate { get; set; }
        public TimeSpan ChapterTime { get; set; }
    }
}
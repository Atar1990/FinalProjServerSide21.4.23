//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Chapter
    {
        public string NameOfChapter { get; set; }
        public int TravelDiaryKey { get; set; }
        public string ChapterDescription { get; set; }
        public string ChapterPictures { get; set; }
        public Nullable<System.DateTime> ChapterDate { get; set; }
        public Nullable<System.TimeSpan> ChapterTime { get; set; }
    
        public virtual TravelDiary TravelDiary { get; set; }
    }
}
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
    
    public partial class RegionOfTheCountry
    {
        public int RegionOfTheCountryKey { get; set; }
        public string CountryName { get; set; }
        public string RegionOfTheCountry1 { get; set; }
        public Nullable<float> RegionOfTheCountryLat { get; set; }
        public Nullable<float> RegionOfTheCountryLon { get; set; }
    
        public virtual Country Country { get; set; }
    }
}

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
    
    public partial class UserFavourite
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserFavourite()
        {
            this.Options = new HashSet<Option>();
        }
    
        public int FavouritesKey { get; set; }
        public string UserEmail { get; set; }
        public string CountryName { get; set; }
        public string UserFavouritesRegionOfTheCountry { get; set; }
        public string Titel { get; set; }
        public string Description { get; set; }
    
        public virtual Country Country { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Option> Options { get; set; }
        public virtual User User { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LTW4.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SOTIETKIEM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SOTIETKIEM()
        {
            this.CHITIETs = new HashSet<CHITIET>();
        }
    
        public string sosotk { get; set; }
        public DateTime ngaylap { get; set; }
        public string tenkh { get; set; }
        public string socmnd { get; set; }
        public string diachi { get; set; }
        public string ghichu { get; set; }
        public string anh { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIET> CHITIETs { get; set; }
    }
}
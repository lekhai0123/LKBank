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
    
    public partial class TYGIA
    {       
        public System.DateTime ngaybatdauapdung { get; set; }
        public string mangoaite { get; set; }
        public Nullable<double> quyravnd { get; set; }
        public string ghichu { get; set; }
    
        public virtual NGOAITE NGOAITE { get; set; }
    }
}
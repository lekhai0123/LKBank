﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class sotietkiemEntities : DbContext
    {
        public sotietkiemEntities()
            : base("name=sotietkiemEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CHITIET> CHITIETs { get; set; }
        public virtual DbSet<LAISUAT> LAISUATs { get; set; }
        public virtual DbSet<NGOAITE> NGOAITEs { get; set; }
        public virtual DbSet<SOTIETKIEM> SOTIETKIEMs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TYGIA> TYGIAs { get; set; }
        public virtual DbSet<USER> USERs { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gumruk.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class app_base_modules
    {
        public app_base_modules()
        {
            this.app_sub_modules = new HashSet<app_sub_modules>();
        }
    
        public int id { get; set; }
        public int mid { get; set; }
        public string name { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public Nullable<bool> isempty { get; set; }
    
        public virtual ICollection<app_sub_modules> app_sub_modules { get; set; }
    }
}

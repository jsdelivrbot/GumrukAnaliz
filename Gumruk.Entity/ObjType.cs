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
    
    public partial class ObjType
    {
        public ObjType()
        {
            this.DBObjects = new HashSet<DBObjects>();
        }
    
        public int ID { get; set; }
        public string objType1 { get; set; }
    
        public virtual ICollection<DBObjects> DBObjects { get; set; }
    }
}

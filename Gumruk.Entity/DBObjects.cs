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
    
    public partial class DBObjects
    {
        public int ID { get; set; }
        public string objName { get; set; }
        public Nullable<int> objType { get; set; }
        public string schemaName { get; set; }
        public Nullable<int> schemaID { get; set; }
    
        public virtual ObjType ObjType1 { get; set; }
    }
}

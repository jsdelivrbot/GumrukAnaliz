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
    
    public partial class d_b_columns
    {
        public d_b_columns()
        {
            this.d_b_column_explanations = new HashSet<d_b_column_explanations>();
            this.d_b_column_links = new HashSet<d_b_column_links>();
            this.d_b_column_lookups = new HashSet<d_b_column_lookups>();
        }
    
        public int id { get; set; }
        public int mid { get; set; }
        public string name { get; set; }
        public int db_column_type_id { get; set; }
        public int length { get; set; }
        public int scale { get; set; }
        public bool isnull { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public int d_b_tablesId { get; set; }
        public Nullable<bool> isunique { get; set; }
        public Nullable<int> precision { get; set; }
        public Nullable<bool> isempty { get; set; }
        public Nullable<int> distinctnum { get; set; }
        public Nullable<bool> isprimary { get; set; }
        public Nullable<bool> tobe { get; set; }
        public Nullable<int> samenameexist { get; set; }
    
        public virtual d_b_tables d_b_tables { get; set; }
        public virtual d_b_column_types d_b_column_types { get; set; }
        public virtual ICollection<d_b_column_explanations> d_b_column_explanations { get; set; }
        public virtual ICollection<d_b_column_links> d_b_column_links { get; set; }
        public virtual ICollection<d_b_column_lookups> d_b_column_lookups { get; set; }
    }
}

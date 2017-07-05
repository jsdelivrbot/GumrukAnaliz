using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumruk.Entity.Model
{
    public class ColumnToExcel
    {
        //public int tableID { get; set; }
        public string Module { get; set; }

        public string Schema { get; set; }
        public string Table { get; set; }
        public string Field { get; set; }
        public string Primary { get; set; }
        public string AllowNull { get; set; }
        public string Type { get; set; }
        public string Explanation1 { get; set; }
        public string Explanation2 { get; set; }

        public string LookUp { get; set; }

        public string Link { get; set; }

        public string FormAlani { get; set; }

        //public DateTime CreatedDate { get; set; }
        public string SystemType { get; set; }

        public int UserID { get; set; }

    }
}
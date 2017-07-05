using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumruk.Web.Models
{
    public class TableProperties
    {
        public string TableName { get; set; }
        public string SchemaName { get; set; }
        public int ColumnCount { get; set; }

        public string RelatedTables { get; set; }
        public int SchemaID { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumruk.Web.Models
{
    public class DataColumnForJson
    {
        public string Name { get; set; }
        public string Length { get; set; }
        public string Scale { get; set; }
        public string Precision { get; set; }
        public string isNull { get; set; }
        public string DataType { get; set; }
        public string Unique { get; set; }

        public int schemaID { get; set; }

    }
}
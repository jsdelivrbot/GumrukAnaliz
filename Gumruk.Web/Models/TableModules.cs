using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumruk.Web.Models
{
    public class TableModules
    {
        public string  ModuleName{ get; set; }
        public string SubModuleName { get; set; }
        public string Operation { get; set; }
        public string Query { get; set; }
    }
}
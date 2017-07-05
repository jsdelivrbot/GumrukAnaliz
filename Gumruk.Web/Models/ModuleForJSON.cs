using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumruk.Web.Models
{
    public class ModuleForJSON
    {
        public ModuleForJSON(string _value)
        {
            value = _value;
            data = new List<SubModuleForJSON>();
        }
        public string value { get; set; }
        public List<SubModuleForJSON> data { get; set; }

    }
}
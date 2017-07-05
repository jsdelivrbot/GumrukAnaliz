using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumruk.Web.Models
{
    public class SubModuleForJSON
    {

        public SubModuleForJSON(int _id,string _value)
        {
            id = _id;
            value = _value;
            operations = new List<ModuleOperation>();
        }

        public int id { get; set; }
        public string value { get; set; }
        public List<ModuleOperation> operations { get; set; }
    }
}
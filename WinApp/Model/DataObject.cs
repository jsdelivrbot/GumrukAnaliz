using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Model
{
    public class DataObject
    {
        public string schemaName { get; set; }
        public string objName { get; set; }
        public string objType { get; set; }
       // public SubDataObject childList { get; set; }

    }
    public class SubDataObject
    {
        public string schemaName { get; set; }
        public string objName { get; set; }
        public string objType { get; set; }
    }
}

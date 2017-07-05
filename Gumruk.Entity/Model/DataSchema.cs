using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gumruk.Entity.Model
{
    public class DataSchema
    {
        public int id { get; set; }
        public string value { get; set; }

        public List<DataTable> data { get; set; }
    }
}

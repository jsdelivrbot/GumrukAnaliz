using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gumruk.Entity.Model
{
    public class DataTable
    {
        public string value { set; get; }

        public List<DataColumn> data { get; set; }

        public double id { get; set; }
    }
}

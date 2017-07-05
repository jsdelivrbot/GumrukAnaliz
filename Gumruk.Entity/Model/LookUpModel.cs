using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gumruk.Entity.Model
{
    public class LookUpModel
    {
        public string SchemaNameFrom { get; set; }

        public string TableNameFrom { get; set; }

        public string ColumnNameFrom { get; set; }

        public string SchemaNameTo { get; set; }

        public string TableNameTo { get; set; }

        public string ColumnNameTo { get; set; }

    }
}

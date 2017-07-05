using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gumruk.Entity.Model
{
    public class ProceduresSearchResult
    {
        public List<d_b_schemas> _schemas { get; set; }

        public List<d_b_tables> _tables { get; set; }

        public List<d_b_columns> _columns { get; set; }

        public List<SQLProcedures> _procedures { get; set; }

        public List<DBErrorMessages> _errorMessages { get; set; }

        public bool fromLayout { get; set; }
    }
}

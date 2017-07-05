using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumruk.Web.Models
{
    public class ModuleOperation
    {

        public ModuleOperation(string _query, string _operation, int _table_id, string _table_name,string _schemaName)
        {
            query = _query;
            operation = _operation;
            table_id = _table_id;
            table_name = _table_name;
            schema_name = _schemaName;
        }

        public string query { get; set; }
        public string operation { get; set; }

        public int table_id { get; set; }

        public string table_name { get; set; }

        public string schema_name { get; set; }

    }
}
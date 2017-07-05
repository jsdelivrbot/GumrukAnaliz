using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumruk.Entity.Model
{
    public class SearchResult
    {
        public SearchResult(string _schema_name, int _schema_id, string _table_name, int _table_id, string _column_name, int _column_id)
        {
            schema_name = _schema_name;
            schema_id = _schema_id;

            table_name = _table_name;
            table_id = _table_id;

            column_name = _column_name;
            column_id = _column_id;

            id = _column_id;
            value = schema_name + "." + table_name + "." + column_name;
        }

        public int id { get; set; }
        public string value { get; set; }


        public string schema_name { get; set; }
        public int schema_id { get; set; }

        public string table_name { get; set; }
        public int table_id { get; set; }

        public string column_name { get; set; }

        public int column_id { get; set; }


        
    }
}
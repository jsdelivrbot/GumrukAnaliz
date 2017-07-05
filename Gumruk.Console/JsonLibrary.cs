using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using OfficeOpenXml;

namespace Gumruk.Console
{
    public class JsonLibrary
    {
        public List<JsonItem> CreateList()
        {
            using (StreamReader r = new StreamReader("D:\\4S Projeler\\docs\\results.json"))
            {
                string json = r.ReadToEnd();
                List<JsonItem> items = JsonConvert.DeserializeObject<List<JsonItem>>(json);

                //var data = JsonConvert.DeserializeObject(json);

                //var name = ((Newtonsoft.Json.Linq.JArray)(data))[0]["name"];
               
                return items;
            }
        }

        public void ExcelExport(List<JsonItem> list)
        {
            System.Data.DataTable dt = ToDataTable<JsonItem>(list);

            var newFile = new FileInfo("d:\\excel.xlsx");
            using (ExcelPackage pck = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Columns");
                ws.Cells["A1"].LoadFromDataTable(dt, true);
                pck.Save();
            }

        }

        //gönderilen list verisini datatable'a dönüştürür.
        public static System.Data.DataTable ToDataTable<T>(List<T> items)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }

    public class JsonItem
    {
        public string name { get; set; }
        public string modules { get; set; }

        public string type { get; set; }
        public string text { get; set; }
    }

    }

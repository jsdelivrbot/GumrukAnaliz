using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gumruk.UnitOfWork.Contract;
using Gumruk.UnitOfWork;
using Gumruk.Entity;
using Newtonsoft.Json;
using System.Web.Security;
using System.Text;
using System.Web.Script.Serialization;
using Gumruk.Web.Models;
using Gumruk.Entity.Model;
using System.IO;
using System.Web.UI;
using OfficeOpenXml;
using System.Reflection;


namespace Gumruk.Web.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        public JsonResult GetSchemas(int schema_id)
        {
            IGumruk _iGumruk = new BSGumruk();
            List<DataTable> schemas = new List<DataTable>();

            schemas = _iGumruk.GetAllSchemas(true, true, schema_id);

            //string json = JsonConvert.SerializeObject(schemas);

            return Json(schemas, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSchema(int schemaID)
        {
            IGumruk _iGumruk = new BSGumruk();

            d_b_schemas schemas = _iGumruk.GetSchema(schemaID);
            // var jsonResult = JsonConvert.SerializeObject(schemas);

            return Json(schemas, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSchemaNames(bool getTableCount = true)
        {
            IGumruk _iGumruk = new BSGumruk();

            List<DataSchema> schemas = _iGumruk.GetSchemaNames(getTableCount);

            //string json = JsonConvert.SerializeObject(schemas);

            return Json(schemas, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTableByName(string name)
        {
            IGumruk _iGumruk = new BSGumruk();

            DataTable table = _iGumruk.GetTableByName(name);

            //string json = JsonConvert.SerializeObject(schemas);

            return Json(table, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetColumnById(int columnID)
        {
            IGumruk _iGumruk = new BSGumruk();

            d_b_columns dbColumns = _iGumruk.GetColumnById(columnID);

            //string json = JsonConvert.SerializeObject(schemas);

            DataColumnForJson dt = new Models.DataColumnForJson()
            {
                Name = dbColumns.name,
                DataType = dbColumns.d_b_column_types.name,
                isNull = dbColumns.isnull == true ? "Evet" : "Hayır",
                Length = dbColumns.length.ToString(),
                Precision = dbColumns.precision.ToString(),
                Scale = dbColumns.scale.ToString(),
                Unique = dbColumns.isunique == true ? "Evet" : "Hayır",
                schemaID = dbColumns.d_b_tables.d_b_schemasId,
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTableById(double tableID)
        {
            IGumruk _iGumruk = new BSGumruk();
            int tblID = (int)(tableID / 0.123123);
            d_b_tables dbTable = _iGumruk.GetTableById(tblID);

            //string json = JsonConvert.SerializeObject(schemas);

            TableProperties dt = new Models.TableProperties()
            {
                TableName = dbTable.name,
                SchemaName = dbTable.d_b_schemas.name,
                ColumnCount = dbTable.d_b_columns.Count,
                SchemaID = dbTable.d_b_schemasId,
            };

            foreach (var item2 in dbTable.d_b_columns)
            {
                foreach (var item3 in item2.d_b_column_lookups)
                {
                    item3.d_b_Column_to = _iGumruk.GetColumnById(item3.column_to_id);
                    dt.RelatedTables = dt.RelatedTables + "  " + item3.d_b_Column_to.d_b_tables.d_b_schemas.name + "." +
                        item3.d_b_Column_to.d_b_tables.name + "." + item3.d_b_Column_to.name;
                }
            }

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTablePropertiesById(int tableID)
        {
            IGumruk _iGumruk = new BSGumruk();

            d_b_tables dbTable = _iGumruk.GetTableById(tableID);

            return PartialView("Table", dbTable);
        }

        public JsonResult GetDetails(int ColumnID)
        {
            IGumruk _iGumruk = new BSGumruk();

            List<d_b_column_explanations> listExps = _iGumruk.GetDetailsByColumnID(ColumnID);

            List<ColumnDetails> response = new List<ColumnDetails>();
            foreach (var item in listExps)
            {
                ColumnDetails clmDetails = new ColumnDetails()
                {
                    Date = item.created_at.ToString(),
                    explanation = item.details,
                    User = item.users.name,
                    ExpID = item.id
                };

                response.Add(clmDetails);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AuthenticationAction]
        public ActionResult Index()
        {
            return RedirectToAction("GetSchemasPage", "Home");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            //return Redirect("../login.html");
            return View("Login");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string loginname, string password)
        {
            IUser iUser = new BSUser();
            users oUser = iUser.Login(loginname, password);

            if (oUser != null)
            {
                //login successed
                SetCurrentUser(oUser);
                FormsAuthentication.SetAuthCookie(loginname, false);
                return Content("");
            }
            else //login işlemi başarısız                
                throw new Exception("Kullanıcı adı ve/veya şifre hatalı");
        }

        [HttpPost]
        public int AddSchemas(string details, string name)
        {
            IGumruk _iGumruk = new BSGumruk();

            d_b_schemas sema = new d_b_schemas();

            sema.id = 0;
            sema.mid = 0;
            sema.name = name;
            sema.details = details;
            sema.created_at = DateTime.Now;
            sema.updated_at = DateTime.Now;

            return _iGumruk.NewSchema(sema);
        }

        [HttpPost]
        public int AddTables(string tablename, string details, int schema_id, int isempty, bool isView)
        {
            IGumruk _iGumruk = new BSGumruk();

            d_b_tables tbl = new d_b_tables();

            tbl.id = 0;
            tbl.mid = 0;
            tbl.name = tablename;
            tbl.d_b_schemasId = schema_id;
            tbl.details = details;
            tbl.updated_at = DateTime.Now;
            tbl.created_at = DateTime.Now;
            tbl.isempty = Convert.ToBoolean(isempty);
            tbl.isView = isView;

            return _iGumruk.NewTable(tbl);
        }

        [HttpPost]
        public int AddColumn(string name, string datatype, int length, int scale, int isnull, int tableID, int precision, int isunique)
        {
            IGumruk _iGumruk = new BSGumruk();

            d_b_columns col = new d_b_columns();

            col.name = name;
            col.length = length;
            col.scale = scale;
            col.isnull = Convert.ToBoolean(isnull);
            col.precision = precision;
            col.isunique = Convert.ToBoolean(isunique);
            col.DataType = datatype;
            col.created_at = DateTime.Now;
            col.updated_at = DateTime.Now;
            col.d_b_tablesId = tableID;

            return _iGumruk.NewColumn(col);
        }

        [AuthenticationAction]
        public ActionResult GetSchemasPage()
        {
            return View("GetShemasPage");
        }

        [AuthenticationAction]
        public ActionResult DataObjects()
        {
            return View("DataBaseObjects");
        }

        [HttpGet]
        [AllowAnonymous]
        public void AddColumnExplanation(int column_id, string Exp)
        {
            IGumruk iGumruk = new BSGumruk();

            int userID = 0;

            if (GetCurrentUser() != null)
                userID = GetCurrentUser().id;

            iGumruk.AddColumnExplanation(column_id, Exp, userID);

        }

        [HttpPost]
        [AllowAnonymous]
        public void AddColumnExplanationPost(int column_id, string Exp)
        {
            IGumruk iGumruk = new BSGumruk();

            iGumruk.AddColumnExplanation(column_id, Exp, GetCurrentUser().id);
        }

        [HttpPost]
        [AuthenticationAction]
        public void AddExplanation(int objectID, string type, string exp)
        {
            IGumruk iGumruk = new BSGumruk();

            if (type == "column")
                iGumruk.AddColumnExplanation(objectID, exp, GetCurrentUser().id);
            else if (type == "table")
                iGumruk.AddTableExplanation(objectID, exp, GetCurrentUser().id);

        }


        public void AddTableExplanation(double table_id, string Exp)
        {
            IGumruk iGumruk = new BSGumruk();

            int tblID = (int)(table_id / 0.123123);

            iGumruk.AddTableExplanation(tblID, Exp, GetCurrentUser().id);

        }

        public JsonResult GetTableExplanations(double tableID)
        {
            IGumruk _iGumruk = new BSGumruk();

            int tblID = (int)(tableID / 0.123123);

            List<d_b_table_explanation> listExps = _iGumruk.GetTableExplanationsByTableID(tblID);

            List<ColumnDetails> response = new List<ColumnDetails>();
            foreach (var item in listExps)
            {
                ColumnDetails clmDetails = new ColumnDetails()
                {
                    Date = item.created_at.ToString(),
                    explanation = item.details,
                    User = item.users.name,
                    ExpID = item.id
                };

                response.Add(clmDetails);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTableExplanationsByTableID(int ID)
        {
            IGumruk _iGumruk = new BSGumruk();

            List<d_b_table_explanation> listExps = _iGumruk.GetTableExplanationsByTableID(ID);

            List<ColumnDetails> response = new List<ColumnDetails>();
            foreach (var item in listExps)
            {
                ColumnDetails clmDetails = new ColumnDetails()
                {
                    Date = item.created_at.ToString(),
                    explanation = item.details,
                    User = item.users.name,
                    ExpID = item.id
                };

                response.Add(clmDetails);
            }

            return PartialView("TableExplanations", response);
        }

        [HttpPost]
        public ActionResult GetColumnExplanationsByColumnsID(int ID)
        {
            IGumruk _iGumruk = new BSGumruk();

            List<d_b_column_explanations> listExps = _iGumruk.GetDetailsByColumnID(ID);

            List<ColumnDetails> response = new List<ColumnDetails>();
            foreach (var item in listExps)
            {
                ColumnDetails clmDetails = new ColumnDetails()
                {
                    Date = item.created_at.ToString(),
                    explanation = item.details,
                    User = item.users.name,
                    ExpID = item.id
                };

                response.Add(clmDetails);
            }

            return PartialView("TableExplanations", response);
        }

        [HttpPost]
        public void AddLookup(int column_id_from, int column_id_to)
        {
            IGumruk iGumruk = new BSGumruk();

            int userID = 0;

            if (GetCurrentUser() != null)
                userID = GetCurrentUser().id;

            iGumruk.AddLookup(column_id_from, column_id_to);

        }

        public JsonResult GetLookUps(int ColumnID)
        {
            IGumruk iGumruk = new BSGumruk();

            List<LookUpModel> lookups = iGumruk.GetLookupModels(ColumnID);

            return Json(lookups, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTableModules(double tableID)
        {
            IGumruk iGumruk = new BSGumruk();
            int tblID = (int)(tableID / 0.123123);
            d_b_tables dbTable = iGumruk.GetTableById(tblID);

            List<TableModules> modules = new List<TableModules>();

            foreach (var item in dbTable.app_module_operations)
            {
                TableModules tblMod = new TableModules()
                {
                    ModuleName = item.app_sub_modules.app_base_modules.name,
                    Operation = item.operation,
                    Query = item.query,
                    SubModuleName = item.app_sub_modules.name,
                };
                modules.Add(tblMod);
            }

            return Json(modules, JsonRequestBehavior.AllowGet);
        }

        public void DeleteExp(int ExpID, int ColumnID)
        {
            IGumruk iGumruk = new BSGumruk();

            iGumruk.DeleteExp(ExpID);

            GetDetails(ColumnID);
        }

        public void DeleteTableExp(int ExpID, double tableID)
        {
            IGumruk iGumruk = new BSGumruk();

            iGumruk.DeleteTableExp(ExpID);

            GetTableExplanations(tableID);
        }

        [HttpPost]
        public ActionResult DeleteTableColumnExp(int ExpID, string type)
        {
            IGumruk iGumruk = new BSGumruk();
            if (type == "table")
                iGumruk.DeleteTableExp(ExpID);
            else if (type == "column")
                iGumruk.DeleteExp(ExpID);

            return Content("");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");

        }

        public int AddBaseModule(string name)
        {
            IGumruk iGumruk = new BSGumruk();

            app_base_modules module = new app_base_modules()
            {
                created_at = DateTime.Now,
                isempty = false,
                mid = 0,
                name = name,
                updated_at = DateTime.Now,
            };

            return iGumruk.NewBaseModule(module);
        }

        public int AddSubModule(string name, int base_module_id)
        {
            IGumruk iGumruk = new BSGumruk();

            app_sub_modules module = new app_sub_modules()
            {
                created_at = DateTime.Now,
                isempty = false,
                mid = 0,
                name = name,
                updated_at = DateTime.Now,
                app_base_module_id = base_module_id,
            };

            return iGumruk.NewSubModule(module);
        }

        public int AddModuleOperations(int table_id, string operation, string query, int sub_module_id, string explanation)
        {
            IGumruk iGumruk = new BSGumruk();

            app_module_operations moduleOps = new app_module_operations()
            {
                created_at = DateTime.Now,
                d_b_tables_id = table_id,
                operation = operation,
                query = query,
                updated_at = DateTime.Now,
                app_sub_module_id = sub_module_id,
                Explanation = explanation,
            };

            return iGumruk.NewModuleOperations(moduleOps);
        }

        [HttpGet]
        public ActionResult TableExcelExport(double tableID, int isSchema)
        {
            IGumruk iGumruk = new BSGumruk();

            List<d_b_columns> columns1;
            //List<d_b_tables> tables;
            int tblID = (int)(tableID / 0.123123);

            if (!Convert.ToBoolean(isSchema))
                columns1 = iGumruk.GetAllColumns(tblID);
            else
                columns1 = iGumruk.GetAllColumns(tblID);

            string filename = columns1[0].d_b_tables.name + "_Columns.xlsx";

            List<ColumnToExcel> excelList = new List<ColumnToExcel>();

            foreach (var item in columns1)
            {
                ColumnToExcel cl = new ColumnToExcel()
                {
                    Module = "",
                    Schema = item.d_b_tables.d_b_schemas.name,
                    Table = item.d_b_tables.name,
                    Field = item.name,
                    Primary = "False", // bu kolon yok 
                    AllowNull = item.isnull.ToString(),
                    Type = item.d_b_column_types.name,
                    LookUp = "",//şimdilik boş gönderiyorum. Doldurulacak.
                    FormAlani = "",
                    SystemType = "",
                };

                foreach (var item2 in item.d_b_column_explanations)
                {
                    cl.Explanation1 = item2.details;
                    break;
                }

                if (item.d_b_column_explanations.Count > 1)
                {
                    for (int i = 1; i < item.d_b_column_explanations.Count; i++)
                    {
                        if (i != item.d_b_column_explanations.Count - 1)
                            cl.Explanation2 += item.d_b_column_explanations.ToList()[i].details + " & ";
                        else
                            cl.Explanation2 += item.d_b_column_explanations.ToList()[i].details;
                    }
                }


                foreach (var item3 in item.d_b_column_lookups)
                {
                    cl.LookUp = item3.d_b_Column_to.d_b_tables.d_b_schemas.name + "." + item3.d_b_Column_to.d_b_tables.name + "." + item3.d_b_Column_to.name;
                }

                excelList.Add(cl);
            }

            //excel e export kısmını yapan burası .
            System.Data.DataTable dt = ToDataTable<ColumnToExcel>(excelList);

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename, System.Text.Encoding.UTF8));

            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Columns");
                ws.Cells["A1"].LoadFromDataTable(dt, true);
                var ms = new System.IO.MemoryStream();
                pck.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);
            }

            HttpContext.Response.Flush();
            HttpContext.Response.End();

            return Content("");
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

        public ActionResult ExcelImport(byte[] files)
        {
            //if (Request != null)
            //{
            //    HttpPostedFileBase file = Request.Files[0];

            //    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            //    {
            //        string fileName = file.FileName;
            //        string fileContentType = file.ContentType;
            //        byte[] fileBytes = new byte[file.ContentLength];
            //        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
            //    }
            //}

            IGumruk iGumruk = new BSGumruk();

            if (Request != null)
            {
                List<ColumnToExcel> listCltoExcel = new List<ColumnToExcel>();
                HttpPostedFileBase file = Request.Files[0];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    var usersList = new List<d_b_columns>();
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        string tableName;
                        string schemaName;
                        string columnName;
                        bool allowNull;
                        string type;
                        string exp1;
                        string exp2;
                        bool primary;
                        string lookup;
                        string link;
                        string formAlani;
                        string systemType;



                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            schemaName = (workSheet.Cells[rowIterator, 2].Value == null) ? "" : workSheet.Cells[rowIterator, 2].Value.ToString();
                            tableName = (workSheet.Cells[rowIterator, 3].Value == null) ? "" : workSheet.Cells[rowIterator, 3].Value.ToString();
                            columnName = (workSheet.Cells[rowIterator, 4].Value == null) ? "" : workSheet.Cells[rowIterator, 4].Value.ToString();
                            primary = (workSheet.Cells[rowIterator, 5].Value == null) ? false : bool.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                            allowNull = (workSheet.Cells[rowIterator, 6].Value == null) ? false : bool.Parse(workSheet.Cells[rowIterator, 6].Value.ToString());
                            type = (workSheet.Cells[rowIterator, 7].Value == null) ? "" : workSheet.Cells[rowIterator, 7].Value.ToString();
                            exp1 = (workSheet.Cells[rowIterator, 8].Value == null) ? "" : workSheet.Cells[rowIterator, 8].Value.ToString();
                            exp2 = (workSheet.Cells[rowIterator, 9].Value == null) ? "" : workSheet.Cells[rowIterator, 9].Value.ToString();
                            lookup = (workSheet.Cells[rowIterator, 10].Value == null) ? "" : workSheet.Cells[rowIterator, 10].Value.ToString();
                            link = (workSheet.Cells[rowIterator, 11].Value == null) ? "" : workSheet.Cells[rowIterator, 11].Value.ToString();
                            formAlani = (workSheet.Cells[rowIterator, 12].Value == null) ? "" : workSheet.Cells[rowIterator, 12].Value.ToString();
                            systemType = (workSheet.Cells[rowIterator, 13].Value == null) ? "" : workSheet.Cells[rowIterator, 13].Value.ToString();

                            //veriler excel'den alındı. İlgili tablolara atılması gerekiyor. 

                            ColumnToExcel cl = new ColumnToExcel();
                            cl.AllowNull = allowNull.ToString();
                            cl.Explanation1 = exp1;
                            cl.Explanation2 = exp2;
                            cl.Field = columnName;
                            cl.FormAlani = formAlani;
                            cl.Link = link;
                            cl.LookUp = lookup;
                            cl.Module = "";
                            cl.Primary = primary.ToString();
                            cl.Schema = schemaName;
                            cl.SystemType = systemType;
                            cl.Table = tableName;
                            cl.Type = type;
                            cl.UserID = GetCurrentUser().id;

                            listCltoExcel.Add(cl);
                        }
                    }

                    iGumruk.ExcelImport(listCltoExcel);
                }
            }

            return View("Index");
        }

        public JsonResult SearchColumnExplanations(string filter)
        {
            IGumruk iGumruk = new BSGumruk();

            string searchValue = HttpContext.Request.QueryString["filter[value]"];

            if (searchValue == null || searchValue.Length < 4)
                return Json(null, JsonRequestBehavior.AllowGet);

            List<SearchResult> searchResults = iGumruk.ColumnExplanationSearch(searchValue);

            return Json(searchResults, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MyJsonForm()
        {
            return View();
        }

        public ActionResult Formatter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetTablesModules(int ID)
        {
            IGumruk _iGumruk = new BSGumruk();

            d_b_tables dbTable = _iGumruk.GetTableById(ID);

            return PartialView("TableModules",dbTable.Modules);

        }
       

        [AuthenticationAction]
        public ActionResult NewUser()
        {
            return View();
        }

        public JsonResult SaveUser(users _user)
        {
            IGumruk igumruk = new BSGumruk();
            users user = igumruk.SaveUser(_user);

            return Json(user, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePassword()
        {
            return View(GetCurrentUser());
        }

        [HttpPost]
        public void ChangePass(string pass)
        {
            IGumruk igumruk = new BSGumruk();

            users user = new users();
            user.id = GetCurrentUser().id;
            user.password = pass;

            igumruk.ChangePassword(user);
        }
    }
}
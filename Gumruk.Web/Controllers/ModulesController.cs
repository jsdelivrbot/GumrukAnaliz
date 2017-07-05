using Gumruk.Entity;
using Gumruk.UnitOfWork;
using Gumruk.UnitOfWork.Contract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Gumruk.Web.Models;
using System.Web.Script.Serialization;

namespace Gumruk.Web.Controllers
{
    [AuthenticationAction]
    public class ModulesController : BaseController
    {
        //
        // GET: /Modules/

        public ActionResult Index()
        {
            IGumruk igumruk = new BSGumruk();

            List<Modules> modules = igumruk.GetModules();

            return View("ModuleMainPage", modules);
        }

        public JsonResult GetModulesList()
        {
            IGumruk iGumruk = new BSGumruk();

            List<app_base_modules> all_modules = iGumruk.GetAllModules();

            List<ModuleForJSON> modulesResponse = new List<ModuleForJSON>();


            foreach (app_base_modules module in all_modules)
            {

                ModuleForJSON baseModule = new ModuleForJSON(module.name);
                modulesResponse.Add(baseModule);
                foreach (app_sub_modules subModule in module.app_sub_modules)
                {
                    baseModule.data.Add(new SubModuleForJSON(subModule.id, subModule.name));
                }

            }
            return Json(modulesResponse, JsonRequestBehavior.AllowGet);


            //return View();

        }

        public JsonResult GetOperationsForModule(int subModuleID)
        {
            IGumruk iGumruk = new BSGumruk();

            List<app_module_operations> dbOperations = iGumruk.GetAllOperationsForModule(subModuleID);

            List<ModuleOperation> operationsResponse = new List<ModuleOperation>();

            foreach (app_module_operations op in dbOperations)
            {
                string tableName = "";
                if (op.d_b_tables != null)
                {
                    tableName = op.d_b_tables.name;
                }
                operationsResponse.Add(new ModuleOperation(op.query, op.operation, op.d_b_tables_id, tableName, op.d_b_tables.d_b_schemas.name));
            }


            return Json(operationsResponse, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetTablesByModuleID(int subModuleID)
        {
            IGumruk iGumruk = new BSGumruk();
            List<ModuleOperation> operationsResponse = new List<ModuleOperation>();
            List<d_b_tables> listTables = iGumruk.GetTablesBySubModuleID(subModuleID);

            foreach (var op in listTables)
            {
                if (operationsResponse.Where(p => p.schema_name == op.d_b_schemas.name && p.table_name == op.name).FirstOrDefault() == null)
                    operationsResponse.Add(new ModuleOperation("", "", op.id, op.name, op.d_b_schemas.name));
            }

            return Json(operationsResponse.OrderBy(p => p.schema_name), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubModulesFiles(int subModuleID)
        {
            IGumruk igumruk = new BSGumruk();

            List<ModulesFiles> model = igumruk.GetSubModulesFiles(subModuleID).OrderBy(p => p.Name).ToList();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ModulesFiles(int ModuleID)
        {
            IGumruk igumruk = new BSGumruk();

            List<ModulesFiles> model = igumruk.GetModulesFiles(ModuleID);
            return PartialView("SubModulesFiles", model);
        }

        [HttpPost]
        public ActionResult GetSubModulesSPs(int subModuleID)
        {
            IGumruk igumruk = new BSGumruk();

            List<SQLProcedureModules> sqlModules = igumruk.GetSQLProcedureModulesBySubModuleID(subModuleID);

            return PartialView(sqlModules);
        }

        [HttpPost]
        public ActionResult GetSubModulesSPsByFileID(int FileID)
        {
            IGumruk igumruk = new BSGumruk();

            List<SQLProcedureModules> sqlModules = igumruk.GetSQLProcedureModulesByFileID(FileID);

            return PartialView("GetSubModulesSPs", sqlModules);
        }

        [HttpPost]
        public ActionResult GetModulesSPs(int ModuleID)
        {
            IGumruk igumruk = new BSGumruk();

            List<SQLProcedureModules> sqlModules = igumruk.GetSQLProcedureModulesByModuleID(ModuleID);

            return PartialView("GetSubModulesSPs", sqlModules);
        }

        [HttpPost]
        public ActionResult GetSubModulesTables(int subModuleID)
        {
            IGumruk igumruk = new BSGumruk();

            List<ModulesTables> sqlModules = igumruk.GetModulesTablesBySubModuleID(subModuleID);

            return PartialView(sqlModules);
        }

        [HttpPost]
        public ActionResult GetModulesTables(int ModuleID)
        {
            IGumruk igumruk = new BSGumruk();

            List<ModulesTables> sqlModules = igumruk.GetModulesTablesByModuleID(ModuleID);

            return PartialView("GetSubModulesTables", sqlModules);
        }

        [HttpPost]
        public ActionResult GetFileTables(int FileID)
        {
            IGumruk igumruk = new BSGumruk();

            List<ModulesTables> sqlModules = igumruk.GetModulesTablesByFileD(FileID);

            return PartialView("GetSubModulesTables", sqlModules);
        }

        [HttpPost]
        public JsonResult GetModuleFileByID(int ID)
        {
            IGumruk iGumruk = new BSGumruk();

            ModulesFiles modFile = new ModulesFiles();

            modFile = iGumruk.GetModuleFileByID(ID);

            var jsonResult = Json(modFile, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        [HttpPost]
        public ActionResult GetInnerModulesByModID(int ID)
        {
            IGumruk igumruk = new BSGumruk();

            List<InnerModule> innMods = igumruk.GetInnerModule(ID);
            return PartialView("InnerModules", innMods);
        }

    }
}
using Gumruk.Entity;
using Gumruk.Entity.Model;
using Gumruk.UnitOfWork;
using Gumruk.UnitOfWork.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gumruk.Web.Controllers
{
    public class StoredProceduresController : BaseController
    {
        // GET: StoredProcedures
        public ActionResult Index()
        {
            return GetProcedures();
        }

        [HttpPost]
        public JsonResult GetProcedures()
        {
            IGumruk iGumruk = new BSGumruk();

            List<SQLProcedures> pros = iGumruk.GetAllProcedures("A", 1);
            return Json(pros, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthenticationAction]
        public ActionResult GetAllProcedures()
        {
            IGumruk iGumruk = new BSGumruk();

            return View(iGumruk.GetAllProcedures("a", 1));
        }

        [HttpPost]
        public ActionResult GetAllProceduresByLetter(string letter, int schemaID)
        {
            IGumruk iGumruk = new BSGumruk();

            return PartialView(iGumruk.GetAllProcedures(letter, schemaID));
        }

        [HttpPost]
        public ActionResult SearchProcedures(string searchText, bool fromLayout)
        {
            IGumruk iGumruk = new BSGumruk();
            ProceduresSearchResult result = new ProceduresSearchResult();
            if (searchText != "")
                result = iGumruk.SearchProcedures(searchText, fromLayout);

            result.fromLayout = fromLayout;

            return PartialView(result);
        }

        [HttpPost]
        public ActionResult GetProceduresCommentsByID(int procedureID)
        {
            IGumruk iGumruk = new BSGumruk();

            return PartialView("ProceduresCommentList", iGumruk.GetSQLProcedureComments(procedureID));
        }

        [HttpPost]
        public ActionResult ProceduresAddComment(string comment, int procedureID, string position, string sqlText)
        {
            IGumruk iGumruk = new BSGumruk();
            int? pos = null;
            if (position != "")
                pos = int.Parse(position);

            SQLProceduresComments prosComment = new SQLProceduresComments()
            {
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
                Details = comment,
                userID = GetCurrentUser().id,
                SQLProceduresID = procedureID,
                user = GetCurrentUser(),
                position = pos,
            };



            iGumruk.AddSQLProcedureComment(prosComment, pos, sqlText);

            return PartialView("ProceduresCommentList", iGumruk.GetSQLProcedureComments(procedureID));
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateSQLText(int ID, string sqlText)
        {
            IGumruk iGumruk = new BSGumruk();
            SQLProcedures pros = new SQLProcedures()
            {
                ID = ID,
                sqlText = sqlText
            };

            pros = iGumruk.UpdateSQLText(pros);

            return Json(pros, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeleteComment(int ID, int procedureID)
        {
            IGumruk iGumruk = new BSGumruk();

            iGumruk.deleteSQlProcedureComment(ID);

            return PartialView("ProceduresCommentList", iGumruk.GetSQLProcedureComments(procedureID));
        }


        [HttpPost]
        public ActionResult GetSQLProcedureModules(int procedureID)
        {
            IGumruk igumruk = new BSGumruk();

            List<SQLProcedureModules> model = igumruk.GetSQLProcedureModules(procedureID);
            return PartialView("GetSQLProcedureProperties", model);
        }

        public JsonResult GetProcedureID(string name, string schemaName)
        {
            IGumruk iGumruk = new BSGumruk();

            SQLProcedures proc = iGumruk.GetProcedureByNameSchemaName(name, schemaName);

            return Json(proc, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetProceduresChildsByID(string procedureName)
        {
            IGumruk iGumruk = new BSGumruk();

            DBObjects obj = iGumruk.GetDBObjectByID(procedureName);
            return PartialView("DBObject", obj);
        }

        public ActionResult GetTablePropertiesByNameSchemaName(string tableName, string schemaName)
        {
            IGumruk _iGumruk = new BSGumruk();

            d_b_tables dbTable = _iGumruk.GetTableByNameSchemaName(tableName, schemaName);

            return PartialView("../Home/Table", dbTable);
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveFormattedSQL(int ID, string sql, string schemaID)
        {
            IGumruk iGumruk = new BSGumruk();
            SQLProcedures sqlPro = new SQLProcedures()
            {
                formattedBody = sql,
                ID = ID
            };
            sqlPro = iGumruk.SaveSQlProcedures(sqlPro, schemaID);

            return Json(sqlPro, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetProcedureByID(int id, string TXT)
        {
            IGumruk iGumruk = new BSGumruk();

            SQLProcedures pros = iGumruk.GetSQLProcedure(id);

            return Json(pros, JsonRequestBehavior.AllowGet);
        }
    }
}
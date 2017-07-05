using Gumruk.Entity;
using Gumruk.Entity.Model;
using Gumruk.UnitOfWork;
using Gumruk.UnitOfWork.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gumruk.Web.Controllers
{
    public class ERController : BaseController
    {
        //
        // GET: /ER/
        [AuthenticationAction]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSchemas(int schema_id)
        {
            IGumruk _iGumruk = new BSGumruk();
            List<DataTable> schemas = new List<DataTable>();

            schemas = _iGumruk.GetAllSchemasWitERs(true, true, schema_id);

            //string json = JsonConvert.SerializeObject(schemas);

            return Json(schemas, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult XMLSave(string XMLData, string diagramName, int diagramID)
        {
            IGumruk iGumruk = new BSGumruk();

            //session'dan table ları al.
            string tables = string.Empty;
            if (Session["ERtables"] != null)
                tables = Session["ERtables"].ToString();

            iGumruk.saveXML(XMLData, tables, diagramName, diagramID);

            return Content("");
        }

        [HttpPost]
        public ActionResult XMLCreate(string tables, int type)
        {
            IGumruk iGumruk = new BSGumruk();

            string[] tableIDs = tables.Split(',');

            List<int> listTableID = new List<int>();
            for (int i = 1; i < tableIDs.Count(); i++)
            {
                listTableID.Add(int.Parse(tableIDs[i]));
            }

            Session["ERtables"] = tables;
            string strXML = iGumruk.prepareSQLViewerData(listTableID, type);

            return Content(strXML);
        }

        [HttpPost]
        public ActionResult DeleteErDiagram(int ID)
        {
            IGumruk iGumruk = new BSGumruk();

            iGumruk.deleteErDiagrams(ID);

            return Content("");
        }

        [HttpPost]
        public JsonResult GetEr(int erid)
        {
            IGumruk _iGumruk = new BSGumruk();

            ERxml er = _iGumruk.GetErxmlByID(erid);

            return Json(er, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewErDesigner()
        {
            return View("View1");
        }

        public ActionResult ErDesigner()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult GetSchemasTables(int schemaID, bool getCheckBox)
        {
            IGumruk iGumruk = new BSGumruk();

            d_b_schemas schema = iGumruk.GetSchema(schemaID);

            //bool silinecek = true;
            for (int i = 0; i < schema.d_b_tables.Count; i++)
            {
                if (schema.d_b_tables.ToList()[i].name.Contains("ESKI"))
                {
                    schema.d_b_tables.Remove(schema.d_b_tables.ToList()[i]);
                    continue;
                }
                if (schema.d_b_tables.ToList()[i].isempty == true || schema.d_b_tables.ToList()[i].isView == true)
                {
                    schema.d_b_tables.Remove(schema.d_b_tables.ToList()[i]);
                    continue;
                }

                //foreach (var item in schema.d_b_tables.ToList()[i].d_b_columns)
                //{
                //    if (item.d_b_column_lookups.Count > 0)
                //    {
                //        foreach (var lookup in item.d_b_column_lookups)
                //        {
                //            d_b_columns column = iGumruk.GetColumnById(lookup.column_to_id);

                //            if (column != null)
                //            {
                //                if (column.d_b_tables.d_b_schemasId == item.d_b_tables.d_b_schemasId)
                //                { silinecek = false;continue; }

                //            }
                //        }
                //    }


                //}

                //if (silinecek)
                //    schema.d_b_tables.Remove(schema.d_b_tables.ToList()[i]);



            }
            ViewData["checkBox"] = getCheckBox;
            return PartialView("TablesView", schema.d_b_tables);
        }

        [HttpPost]
        public ActionResult GetSavedDiagramsBySchemaID(int schemaID)
        {
            IGumruk iGumruk = new BSGumruk();

            List<ERxml> diagrams = iGumruk.GetSavedDiagramsBySchemaID(schemaID);
            return PartialView("Diagrams", diagrams);
        }
    }
}
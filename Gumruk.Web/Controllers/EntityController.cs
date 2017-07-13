using Gumruk.Web.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gumruk.Web.Controllers
{
    public class EntityController : BaseController
    {
        // GET: Entity
        [AuthenticationAction]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetNodeData()
        {
            EntityNodeData nodeDatas = new EntityNodeData();

            List<NodeData> nodes = new List<NodeData>();

            #region products 

            NodeData newNoteData = new NodeData();
            newNoteData.key = "Products";
            newNoteData.items = new List<NodeDataSubitems>();
            NodeDataSubitems newNodeDataSubItem;

            newNodeDataSubItem = new NodeDataSubitems("ProductID", true, "Decision", NodeData.yellowgrad);
            newNoteData.items.Add(newNodeDataSubItem);

            newNodeDataSubItem = new NodeDataSubitems("ProductName", false, "Cube1", "bluegrad");
            newNoteData.items.Add(newNodeDataSubItem);

            newNodeDataSubItem = new NodeDataSubitems("SupplierID", false, "Decision", "gray");
            newNoteData.items.Add(newNodeDataSubItem);

            newNodeDataSubItem = new NodeDataSubitems("CategoryID", false, "Decision", "gray");
            newNoteData.items.Add(newNodeDataSubItem);

            nodes.Add(newNoteData);
            #endregion

            #region Suppliers
            newNoteData = new NodeData();
            newNoteData.key = "Suppliers";
            newNoteData.items = new List<NodeDataSubitems>();
            newNodeDataSubItem = new NodeDataSubitems("SupplierID", true, "Decision", "yellowgrad");
            newNoteData.items.Add(newNodeDataSubItem);

            newNodeDataSubItem = new NodeDataSubitems("CompanyName", true, "Cube1", "bluegrad");
            newNoteData.items.Add(newNodeDataSubItem);

            newNodeDataSubItem = new NodeDataSubitems("ContactName", true, "Cube1", "bluegrad");
            newNoteData.items.Add(newNodeDataSubItem);

            newNodeDataSubItem = new NodeDataSubitems("Address", true, "Cube1", "bluegrad");
            newNoteData.items.Add(newNodeDataSubItem);

            nodes.Add(newNoteData);
            #endregion

            newNoteData = new NodeData();
            newNoteData.key = "Categories";
            newNoteData.items = new List<NodeDataSubitems>();
            newNodeDataSubItem = new NodeDataSubitems("CategoryID", true, "Decision", "yellowgrad");
            newNoteData.items.Add(newNodeDataSubItem);

            newNodeDataSubItem = new NodeDataSubitems("CategoryName", false, "Cube1", "bluegrad");
            newNoteData.items.Add(newNodeDataSubItem);
            nodes.Add(newNoteData);

            nodeDatas.NodeDatas = nodes;


            List<NodeLink> linkNodes = new List<NodeLink>();
            NodeLink linkNode = new NodeLink("Products", "Suppliers", "0..N SupplierID", "1");
            linkNodes.Add(linkNode);

            linkNode = new NodeLink("Products", "Categories", "1 CotegoryID", "1");
            linkNodes.Add(linkNode);

            nodeDatas.NodeLinks = linkNodes;

            return Json(nodeDatas, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            EntityNodeData nodeDatas = new EntityNodeData();
            List<NodeData> nodes = new List<NodeData>();
            List<NodeLink> linkNodes = new List<NodeLink>();
            if (Request.Files.Count > 0)
            {
                HttpFileCollectionBase files = Request.Files;

                string filename = Path.GetFileName(Request.Files[0].FileName);

                HttpPostedFileBase file = files[0];
                string fname;
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                }

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {

                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        string sheetName = Session["sheetName"].ToString();

                        ExcelWorksheet workSheet;
                        if (sheetName == string.Empty)
                            workSheet = currentSheet.First();
                        else
                            workSheet = currentSheet.Where(p => p.Name == sheetName).FirstOrDefault();

                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        string tableName;
                        string columnName;
                        string lookupTable;
                        string LinkColumn;
                        string LinkType;
                        string pk;
                        string toColumn;
                        string datatype;
                        string tableExp;
                        string columnExp;

                        List<string> tables = new List<string>();
                        NodeData newNoteData = null;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            tableExp= (workSheet.Cells[rowIterator, 1].Value == null) ? "" : workSheet.Cells[rowIterator, 1].Value.ToString();
                            tableName = (workSheet.Cells[rowIterator, 2].Value == null) ? "" : workSheet.Cells[rowIterator, 2].Value.ToString();
                            columnName = (workSheet.Cells[rowIterator, 3].Value == null) ? "" : workSheet.Cells[rowIterator, 3].Value.ToString();
                            columnExp = (workSheet.Cells[rowIterator, 4].Value == null) ? "" : workSheet.Cells[rowIterator, 4].Value.ToString();
                            datatype = (workSheet.Cells[rowIterator, 5].Value == null) ? "" : " (" +workSheet.Cells[rowIterator, 5].Value.ToString()+ ") ";
                            lookupTable = (workSheet.Cells[rowIterator, 6].Value == null) ? "" : workSheet.Cells[rowIterator, 6].Value.ToString();
                            LinkColumn = (workSheet.Cells[rowIterator, 7].Value == null) ? "" : workSheet.Cells[rowIterator, 7].Value.ToString();
                            toColumn = (workSheet.Cells[rowIterator, 8].Value == null) ? "" : workSheet.Cells[rowIterator, 8].Value.ToString();
                            LinkType = (workSheet.Cells[rowIterator, 9].Value == null) ? "" : workSheet.Cells[rowIterator, 9].Value.ToString();
                            pk = (workSheet.Cells[rowIterator, 10].Value == null) ? "" : workSheet.Cells[rowIterator, 10].Value.ToString();
                            

                            //veriler excel'den alındı. İlgili tablolara atılması gerekiyor. 
                            if (tableName != string.Empty && !tables.Contains(tableName))
                            {
                                newNoteData = new NodeData();
                                newNoteData.key = tableName;
                                newNoteData.items = new List<NodeDataSubitems>();
                                tables.Add(tableName);
                                nodes.Add(newNoteData);
                            }

                            if (tableName != string.Empty && tableExp != string.Empty)
                            {
                                NodeDataSubitems newNodeDataSubItem;
                                newNodeDataSubItem = new NodeDataSubitems(tableExp, false, "Cube2", "Gray");
                                newNoteData.items.Add(newNodeDataSubItem);
                            }

                            if (tableName != string.Empty)
                            {
                                NodeDataSubitems newNodeDataSubItem;
                                newNodeDataSubItem = new NodeDataSubitems((pk == "true") ? columnName +datatype+ " PK" : columnName+datatype, (pk == "true") ? true : false, (pk == "true") ? "Decision" : "Cylinder1", (pk == "true") ? "yellow" : "white");
                                newNoteData.items.Add(newNodeDataSubItem);
                            }

                            if (lookupTable != string.Empty && LinkColumn != string.Empty)
                            {
                                string[] tmpType = null;
                                if (LinkType != string.Empty)
                                    tmpType = LinkType.Split('-');

                                string toText = "";
                                string fromText = "";
                                if (tmpType != null && tmpType.Count() > 0)
                                {
                                    toText = tmpType[1];
                                    fromText = tmpType[0];
                                }

                                NodeLink linkNode = new NodeLink(tableName, lookupTable, fromText + "    " + columnName, toColumn  + "  " + toText);
                                linkNodes.Add(linkNode);
                            }
                        } // for end
                    }//using end

                    nodeDatas.NodeDatas = nodes;
                    nodeDatas.NodeLinks = linkNodes;
                }//if end



            }
            Session["UploadedEntity"] = nodeDatas;

            return Content("");
        }

        [HttpPost]
        public JsonResult GetSessionNodeData()
        {
            EntityNodeData nodeDatas = (EntityNodeData)Session["UploadedEntity"];

            return Json(nodeDatas, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetSheetName(string sheetName)
        {
            Session["sheetName"] = sheetName;
            return Content("");
        }
    }
}
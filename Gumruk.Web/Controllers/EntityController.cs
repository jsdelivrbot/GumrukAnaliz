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
                        string color;

                        List<string> tables = new List<string>();
                        NodeData newNoteData = null;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            tableExp = (workSheet.Cells[rowIterator, 1].Value == null) ? "" : workSheet.Cells[rowIterator, 1].Value.ToString();
                            tableName = ((workSheet.Cells[rowIterator, 2].Value == null) ? "" : workSheet.Cells[rowIterator, 2].Value.ToString()).ToUpper();
                            columnName = (workSheet.Cells[rowIterator, 3].Value == null) ? "" : workSheet.Cells[rowIterator, 3].Value.ToString();
                            columnExp = (workSheet.Cells[rowIterator, 4].Value == null) ? "" : workSheet.Cells[rowIterator, 4].Value.ToString();
                            datatype = (workSheet.Cells[rowIterator, 5].Value == null) ? "" : " (" + workSheet.Cells[rowIterator, 5].Value.ToString() + ") ";
                            lookupTable = ((workSheet.Cells[rowIterator, 6].Value == null) ? "" : workSheet.Cells[rowIterator, 6].Value.ToString()).ToUpper();
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

                                newNodeDataSubItem = new NodeDataSubitems(" ", false, "Cube2", "White");
                                newNoteData.items.Add(newNodeDataSubItem);

                            }

                            if (tableName != string.Empty)
                            {
                                if (pk == "true")
                                    color = "yellow";
                                else if (LinkColumn != string.Empty)
                                    color = "blue";
                                else
                                    color = "white";

                                NodeDataSubitems newNodeDataSubItem;
                                newNodeDataSubItem = new NodeDataSubitems((pk == "true") ? columnName + datatype + " PK" + " " + columnExp : columnName + datatype + "  " + columnExp, (pk == "true") ? true : false, (pk == "true" || color == "blue") ? "Cube1" : "Cylinder1", color);
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

                                NodeLink linkNode = new NodeLink(tableName, lookupTable, fromText + "    " + columnName, toColumn + "  " + toText);
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

        public EntityNodeData PrepareList()
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
                        string uzunluk;

                        bool boolPK = false;

                        List<string> tables = new List<string>();
                        NodeData newNoteData = null;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            tableExp = (workSheet.Cells[rowIterator, 1].Value == null) ? "" : workSheet.Cells[rowIterator, 1].Value.ToString();
                            tableName = ((workSheet.Cells[rowIterator, 2].Value == null) ? "" : workSheet.Cells[rowIterator, 2].Value.ToString()).ToUpperInvariant();
                            columnName = (workSheet.Cells[rowIterator, 3].Value == null) ? "" : workSheet.Cells[rowIterator, 3].Value.ToString();
                            columnExp = (workSheet.Cells[rowIterator, 4].Value == null) ? "" : workSheet.Cells[rowIterator, 4].Value.ToString();
                            datatype = (workSheet.Cells[rowIterator, 5].Value == null) ? "" : workSheet.Cells[rowIterator, 5].Value.ToString();
                            uzunluk = (workSheet.Cells[rowIterator, 6].Value == null) ? "" : workSheet.Cells[rowIterator, 6].Value.ToString();
                            lookupTable = ((workSheet.Cells[rowIterator, 7].Value == null) ? "" : workSheet.Cells[rowIterator, 7].Value.ToString()).ToUpperInvariant();
                            LinkColumn = (workSheet.Cells[rowIterator, 8].Value == null) ? "" : workSheet.Cells[rowIterator, 8].Value.ToString();
                            toColumn = (workSheet.Cells[rowIterator, 9].Value == null) ? "" : workSheet.Cells[rowIterator, 9].Value.ToString();
                            LinkType = (workSheet.Cells[rowIterator, 10].Value == null) ? "" : workSheet.Cells[rowIterator, 10].Value.ToString();
                            pk = (workSheet.Cells[rowIterator, 11].Value == null) ? "" : workSheet.Cells[rowIterator, 11].Value.ToString();

                            if (toColumn == string.Empty)
                                toColumn = columnName;

                            if (pk != string.Empty)
                                boolPK = bool.Parse(pk);
                            else
                                boolPK = false;
                            //veriler excel'den alındı. İlgili tablolara atılması gerekiyor. 
                            if (tableName != string.Empty && !tables.Contains(tableName))
                            {
                                newNoteData = new NodeData();
                                newNoteData.key = tableName;
                                newNoteData.description = tableExp;
                                newNoteData.tables = new List<NodeDataTable>();
                                tables.Add(tableName);
                                nodes.Add(newNoteData);
                            }

                            if (tableName != string.Empty)
                            {
                                NodeDataTable newNodeDataSubItem;
                                newNodeDataSubItem = new NodeDataTable(columnName, boolPK, datatype, uzunluk, columnExp);
                                newNoteData.tables.Add(newNodeDataSubItem);
                            }

                            if (lookupTable != string.Empty && LinkColumn != string.Empty)
                            {
                                NodeLink linkNode = new NodeLink(tableName, lookupTable, columnName, toColumn);
                                linkNodes.Add(linkNode);
                            }
                        } // for end
                    }//using end

                    nodeDatas.NodeDatas = nodes;
                    nodeDatas.NodeLinks = linkNodes;
                }//if end
            }
            return nodeDatas;
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

        public ActionResult CreateSQLScript()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFilesForScript()
        {
            EntityNodeData nodeDatas = new EntityNodeData();
            string sql = string.Empty;
            nodeDatas = PrepareList();

            //table script fonksiyonları yazıldı.
            foreach (var node in nodeDatas.NodeDatas)
            {
                sql += "CREATE TABLE " + node.key + " (" + "\n";
                foreach (var column in node.tables)
                {
                    if (column.datatype == "varchar")
                        sql += column.name + " " + column.datatype + "(" + column.uzunluk + ")" + (column.iskey == true ? " primary key not null" : " ") + ",\n";
                    else
                        sql += column.name + " " + column.datatype + " " + (column.iskey == true ? " primary key not null" : " ") + ",\n";
                }
                sql += ") \n";
            }
            sql += "GO\n";

            if (nodeDatas.NodeLinks.Count > 0)
            {
                //foreign key tanımları yapılıyor.
                foreach (var node in nodeDatas.NodeLinks)
                {
                    sql += "ALTER TABLE [" + node.from+ "]\n";
                    sql += "ADD CONSTRAINT FK_" +  node.from+"_"+node.to+node.text + "\n";
                    sql += "FOREIGN KEY ("+node.text+")"+" REFERENCES ["+node.to.ToUpperInvariant()+"]("+node.toText+")\n" ;
                    sql += "\n";
                }
            }
            sql += "GO\n";

            foreach (var node in nodeDatas.NodeDatas)
            {
                int firstColumn = 0;
                foreach (var column in node.tables)
                {
                    if(firstColumn==0 && node.description!=string.Empty)// ilk kolonsa tablonun açıklamasını koy.
                    {
                        sql += string.Format("EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'{0}' , @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE',@level1name = N'{1}', @level2type = N'COLUMN',@level2name = N'{2}'", node.description.Replace("'", " "), node.key, column.name) + "\n";
                        sql += "GO\n";
                    }
                    else if(firstColumn>0 && column.description!=string.Empty) // kolon için açıklama varsa.
                    {
                        sql += string.Format("EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'{0}' , @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE',@level1name = N'{1}', @level2type = N'COLUMN',@level2name = N'{2}'",column.description.Replace("'"," "), node.key,column.name)+"\n";
                        sql += "GO\n";
                    }
                    firstColumn++;
                }
            }

            return Content(sql);
        }
    }
}
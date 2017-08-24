using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
//using Excel;
using Gumruk.UnitOfWork.Contract;
using Gumruk.UnitOfWork;
using Gumruk.Entity;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using Gumruk.Entity.Model;
using OfficeOpenXml;
using WinApp.Model;

namespace WinApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IGumruk iGumruk = new BSGumruk();
            DirectoryInfo di = new DirectoryInfo("D:\\TFS\\BILGEV2\\BILGE-TEST\\OzetBeyanTC");

            DirectoryInfo[] dirs = di.GetDirectories();

            foreach (var item in dirs)
            {

                FileInfo[] files = item.GetFiles("*.cs");

                string contents;
                for (int i = 0; i < files.Count(); i++)
                {
                    contents = File.ReadAllText(files[i].FullName);

                    SQLProcedures sql = new SQLProcedures()
                    {
                        body = contents,
                        name = files[i].Name,
                        formattedBody = null,
                        isFormatted = false,
                        mid = 0,
                        sqlText = null
                    };

                    //sql = iGumruk.SaveSQlProcedures(sql, item.Name);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IGumruk iGumruk = new BSGumruk();

            string filePath = "D:\\PLSQL\\ErrorLists.xls";

            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

            //1. Reading from a binary Excel file ('97-2003 format; *.xls)
            //IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            //...
            //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
            //IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            //...
            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            //DataSet result = excelReader.AsDataSet();
            //...
            ////4. DataSet - Create column names from first row
            //excelReader.IsFirstRowAsColumnNames = true;
            //DataSet result = excelReader.AsDataSet();

            //foreach (DataRow item in result.Tables[0].Rows)
            //{
            //    DBErrorMessages message = new DBErrorMessages()
            //    {
            //        Code = item["Code"].ToString(),
            //        Descrip = item["Descr"].ToString(),
            //        Descren = item["Descren"].ToString()
            //    };

            //    message = iGumruk.SaveErrorMessages(message);
            //}
            ////5. Data Reader methods
            ////while (excelReader.Read())
            ////{
            ////    //excelReader.GetInt32(0);
            ////}

            ////6. Free resources (IExcelDataReader is IDisposable)
            //excelReader.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IGumruk iGumruk = new BSGumruk();

            List<SQLProcedures> procedures = iGumruk.GetAllProcedures("a", 1);

            foreach (var item in procedures)
            {
                if (item.formattedBody == null)
                    continue;

                item.formattedBody = item.formattedBody.Replace("DROP", "<font color='#0000ff'>DROP</font>");
                item.formattedBody = item.formattedBody.Replace("CREATE", "<font color=#0000ff>CREATE</font>");
                item.formattedBody = item.formattedBody.Replace("SELECT", "<font color=#0000ff>SELECT</font>");
                item.formattedBody = item.formattedBody.Replace("PROCEDURE", "<font color='#0000ff'>PROCEDURE</font>");
                item.formattedBody = item.formattedBody.Replace("BEGIN", "<font color='#0000ff'>BEGIN</font>");
                item.formattedBody = item.formattedBody.Replace("END", "<font color='#0000ff'>END</font>");
                item.formattedBody = item.formattedBody.Replace("INSERT", "<font color='#0000ff'>INSERT</font>");
                item.formattedBody = item.formattedBody.Replace("VALUES", "<font color='#0000ff'>VALUES</font>");
                item.formattedBody = item.formattedBody.Replace("UPDATE", "<font color='#0000ff'>UPDATE</font>");
                item.formattedBody = item.formattedBody.Replace("DELETE", "<font color='#0000ff'>DELETE</font>");
                item.formattedBody = item.formattedBody.Replace("INTO", "<font color='#0000ff'>INTO</font>");
                item.formattedBody = item.formattedBody.Replace("FROM", "<font color='#0000ff'>FROM</font>");


                iGumruk.SaveSQlProcedures(item, "");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            IGumruk igumruk = new BSGumruk();

            List<SQLProcedures> listSP = igumruk.GetAllSP(true);

            List<Modules> modules = igumruk.GetModules();

            List<d_b_schemas> listSchemas = igumruk.GetAllSchemas();

            foreach (var modItem in modules)
            {
                foreach (var modSubitem in modItem.SubModules)
                {
                    foreach (var modFile in modSubitem.ModulesFiles)
                    {
                        //SPCalistir_ geçen line'ı bul. 
                        string[] lines = modFile.Body.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                        int lineCount = 0;
                        foreach (var line in lines)
                        {
                            lineCount++;
                            if (line.Contains("SPCalistir"))
                            {
                                string pureLine = line.Trim();
                                string[] str = pureLine.Split('"');

                                //str nesnesindeki tüm öğeleri sp isimleri ile karşılaştır. varsa sqlproceduresmodules tablosuna kayıt atmak gerekecek.
                                for (int i = 0; i < str.Count(); i++)
                                {
                                    string prosName = str[i] + ".prc";
                                    string funcName = str[i] + ".fnc";
                                    List<SQLProcedures> procedure = listSP.Where(p => p.name == prosName || p.name == funcName).ToList();

                                    foreach (var item in procedure)
                                    {
                                        SQLProcedureModules sqlMod = new SQLProcedureModules()
                                        {
                                            SQLProcedureID = item.ID,
                                            ModuleID = modItem.ID,
                                            SubModuleID = modSubitem.ID,
                                        };
                                        igumruk.SaveSqlProcedureModules(sqlMod);
                                    }

                                    //str[i] yi slipt edelim . karakterine göre
                                    string[] newStr = str[i].Split('.');

                                    if (newStr.Count() > 1)
                                    {
                                        d_b_schemas schema = listSchemas.Where(p => p.name == newStr[0]).SingleOrDefault();
                                        prosName = newStr[1] + ".prc";
                                        funcName = newStr[1] + ".fnc";
                                        SQLProcedures pros = null;
                                        if (schema != null)
                                            pros = listSP.Where(p => (p.name == prosName || p.name == funcName) && p.schema_id == schema.id).SingleOrDefault();

                                        if (pros != null)
                                        {
                                            SQLProcedureModules sqlMod = new SQLProcedureModules()
                                            {
                                                SQLProcedureID = pros.ID,
                                                ModuleID = modItem.ID,
                                                SubModuleID = modSubitem.ID,
                                            };
                                            igumruk.SaveSqlProcedureModules(sqlMod);
                                        }
                                    }
                                }
                            }
                        }


                        //UpdateModuleFile(modFile.ID, modFile.Body);
                    }
                }
            }
        }

        private void UpdateModuleFile(int ID, string newBody)
        {
            IGumruk igumruk = new BSGumruk();
            ModulesFiles modFile = new ModulesFiles();
            modFile = igumruk.GetModuleFile(ID);

            modFile.Body = newBody;

            modFile = igumruk.UpdateModulesFiles(modFile);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            IGumruk igumruk = new BSGumruk();

            List<SQLProcedures> listSP = igumruk.GetAllSP();
            SQLProcedures pros = new SQLProcedures();



            foreach (var item in listSP)
            {
                string[] lines = item.body.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                pros = igumruk.GetProcedure(item.ID);
                pros.IfCount = IfCounter(item.body.ToUpperInvariant(), "IF ");
                pros.IfCount += IfCounter(item.body, "ELSE ", (int)pros.IfCount);
                pros.IfCount += IfCounter(item.body.ToUpperInvariant(), "ELSIF ", (int)pros.IfCount);
                pros.IfCount = IfCounter(item.body.ToUpperInvariant(), " IF ");
                pros.IfCount += IfCounter(item.body, " ELSE ", (int)pros.IfCount);
                pros.IfCount += IfCounter(item.body.ToUpperInvariant(), " ELSIF ", (int)pros.IfCount);
                pros.LineCount = lines.Count();
                igumruk.SaveSQlProcedures(pros, "");
            }
        }

        private int IfCounter(string body, string word, int count = 0)
        {
            string newBody = "";
            int ifIndex = body.IndexOf(word);
            if (ifIndex > -1)
            {
                ifIndex += word.Length;
                count++;
                newBody = body.Substring(ifIndex);
            }


            if (newBody.Length > 0)
                return IfCounter(newBody, word, count);
            else
                return count;
        }

        private void button6_Click(object sender, EventArgs e)
        {

            List<DBErrorMessages> listMessages = new List<DBErrorMessages>();
            List<SQLProcedures> sqlProcedures = new List<SQLProcedures>();

            IGumruk igumruk = new BSGumruk();

            listMessages = igumruk.GetAllErrorMessages();
            sqlProcedures = igumruk.GetAllSP();

            foreach (var itemMessage in listMessages)
            {
                foreach (var itemSP in sqlProcedures)
                {
                    if (itemSP.body.Contains(itemMessage.Code))
                    {

                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            IGumruk iGumruk = new BSGumruk();
            string path = textBox1.Text;
            DirectoryInfo di = new DirectoryInfo(path);

            DirectoryInfo[] dirs = di.GetDirectories();

            foreach (var itemdir in dirs)
            {
                Modules mod = new Modules()
                {
                    created_at = DateTime.Now,
                    isempty = false,
                    Name = itemdir.Name,
                    modType = 1,
                };

                mod = iGumruk.SaveModule(mod);

                // varsa modulun altındaki dosyaları ekliyoruz. 
                FileInfo[] modfiles = itemdir.GetFiles("*.cs");

                string modcontents;
                for (int i = 0; i < modfiles.Count(); i++)
                {
                    modcontents = File.ReadAllText(modfiles[i].FullName);

                    ModulesFiles modFile = new ModulesFiles()
                    {
                        Body = modcontents,
                        ModuleID = mod.ID,
                        Name = modfiles[i].Name,
                    };

                    modFile = iGumruk.SaveModulesFiles(modFile);
                }

                DirectoryInfo[] subDirs = itemdir.GetDirectories();

                foreach (var item in subDirs)
                {
                    SubModules subMod = new SubModules()
                    {
                        created_at = DateTime.Now,
                        isempty = false,
                        ModuleID = mod.ID,
                        Name = item.Name,
                    };

                    subMod = iGumruk.SaveSubModule(subMod);

                    FileInfo[] files = item.GetFiles("*.cs");

                    string contents;
                    for (int i = 0; i < files.Count(); i++)
                    {
                        contents = File.ReadAllText(files[i].FullName);

                        ModulesFiles modFile = new ModulesFiles()
                        {
                            Body = contents,
                            ModuleID = mod.ID,
                            Name = files[i].Name,
                            SubModuleID = subMod.ID,
                        };

                        modFile = iGumruk.SaveModulesFiles(modFile);
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            IGumruk igumruk = new BSGumruk();

            List<d_b_tables> listTables = igumruk.GetAllTables();

            List<Modules> modules = igumruk.GetModules();

            foreach (var modItem in modules)
            {
                foreach (var modSubitem in modItem.SubModules)
                {
                    foreach (var modFile in modSubitem.ModulesFiles)
                    {
                        foreach (var item in listTables)
                        {
                            if (item.name.Length >= 3)
                            {
                                string strFullName = item.d_b_schemas.name + "." + item.name;
                                string strName = " " + item.name + " ";

                                if (modFile.Body.Contains(strFullName) || modFile.Body.Contains(strName) || modFile.Body.Contains(strFullName.ToUpperInvariant()) || modFile.Body.Contains(strName.ToUpperInvariant()))
                                {
                                    modFile.Body = modFile.Body.Replace(item.name, "<span class='tableCSS'>" + item.name + " </span>");

                                    ModulesTables sqlMod = new ModulesTables()
                                    {
                                        d_b_tablesID = item.id,
                                        moduleID = modItem.ID,
                                        subModuleID = modSubitem.ID,
                                    };
                                    igumruk.SaveModuleTable(sqlMod);
                                }
                            }
                        }
                        UpdateModuleFile(modFile.ID, modFile.Body);
                    }
                }
            }
        }

        public static List<DBObjects> listObj;
        public static List<DBObjectsChilds> listChilds;
        public static IEnumerable<DBObjects> allDbObj;
        public static Hashtable types;
        public static int publicObjID = 0;
        public static int publicSubObjID = 0;
        private void button9_Click(object sender, EventArgs e)
        {
            string filePath = textBox1.Text;
            DirectoryInfo di = new DirectoryInfo(filePath);
            IGumruk igumruk = new BSGumruk();


            FileInfo[] files = di.GetFiles("*.json");

            listObj = new List<DBObjects>();
            int index = 0;
            dynamic dynJson;
            for (int i = 0; i < files.Count(); i++)
            {
                index++;
                using (StreamReader sr = new StreamReader(files[i].FullName))
                {
                    string json = sr.ReadToEnd();
                    dynJson = JObject.Parse(json);
                    //dynJson = JsonConvert.DeserializeObject(json);
                    publicObjID++;
                    DBObjects dbObj = new DBObjects()
                    {
                        objName = dynJson.objName,
                        schemaID = 0,
                        schemaName = dynJson.schemaName,
                        strObjType = dynJson.objType,
                        ID = publicObjID,
                        parentID = 0,
                    };

                    ////daha once eklenmemişse ekle 
                    //IEnumerable<DBObjects> o = listObj.Where(p => p.schemaName == dbObj.schemaName && p.objName == dbObj.objName && p.strObjType == dbObj.strObjType);
                    //if (o == null)
                    listObj.Add(dbObj);
                    //igumruk.AddDbOject(dbObj);

                    if (dynJson.childList != null)
                        adddbobject(dynJson.childList, dbObj.ID);
                }
            }

            igumruk.AddDbOject(listObj);
        }

        public void adddbobject(dynamic childList, int parentID)
        {
            IGumruk igumruk = new BSGumruk();
            if (childList.Type != Newtonsoft.Json.Linq.JTokenType.Array)
            {
                publicSubObjID++;
                DBObjects dbObj = new DBObjects()
                {
                    objName = childList["objName"],
                    schemaID = 0,
                    schemaName = childList["schemaName"],
                    strObjType = childList["objType"],
                    ID = publicSubObjID,
                    parentID = parentID,
                };

                // igumruk.AddDbOject(dbObj);
                listObj.Add(dbObj);
                if (childList.childList != null)
                {
                    foreach (var obj in childList.childList)
                    {
                        if (obj.objName != null)
                        {
                            publicSubObjID++;
                            DBObjects dbobj2 = new DBObjects()
                            {
                                objName = obj.objName,
                                schemaID = 0,
                                schemaName = obj.schemaName,
                                strObjType = obj.objType,
                                ID = publicSubObjID,
                            };

                            //igumruk.AddDbOject(dbobj2);
                            if (obj.childList != null)
                                parentID = publicSubObjID;

                            parentID = dbobj2.ID;
                            listObj.Add(dbobj2);
                        }

                        if (obj.childList != null)
                            adddbobject(obj.childList, parentID);
                    }
                }
            }
            if (childList.Type == Newtonsoft.Json.Linq.JTokenType.Array)
                foreach (var obj in childList)
                {
                    if (obj.objName != null)
                    {
                        publicSubObjID++;
                        DBObjects dbobj2 = new DBObjects()
                        {
                            objName = obj.objName,
                            schemaID = 0,
                            schemaName = obj.schemaName,
                            strObjType = obj.objType,
                            parentID = parentID,
                            ID = publicSubObjID,
                        };

                        if (obj.childList != null)
                            parentID = publicSubObjID;
                        //igumruk.AddDbOject(dbobj2);
                        listObj.Add(dbobj2);
                    }

                    if (obj.childList != null)
                        adddbobject(obj.childList, parentID);
                }
        }

        public void markChilds(dynamic childList, int parentID)
        {
            if (listChilds == null)
                listChilds = new List<DBObjectsChilds>();

            IGumruk igumruk = new BSGumruk();
            if (childList.Type != Newtonsoft.Json.Linq.JTokenType.Array)
            {
                // igumruk.AddDbOject(dbObj);
                DBObjects _obj = allDbObj.Where(p => p.objName == Convert.ToString(childList.objName) && p.schemaName == Convert.ToString(childList.schemaName) && p.objType == types[Convert.ToString(childList.objType)]).FirstOrDefault();

                DBObjectsChilds child = new DBObjectsChilds()
                {
                    ChildID = _obj.ID,
                    ParentID = parentID,
                };

                listChilds.Add(child);

                if (childList.childList != null)
                {
                    foreach (var obj in childList.childList)
                    {
                        if (obj.objName != null)
                        {
                            DBObjects _obj2 = allDbObj.Where(p => p.objName == Convert.ToString(obj.objName) && p.schemaName == Convert.ToString(obj.schemaName) && p.objType == types[Convert.ToString(obj.objType)]).FirstOrDefault();
                            DBObjectsChilds child2 = new DBObjectsChilds()
                            {
                                ChildID = _obj2.ID,
                                ParentID = parentID,
                            };

                            listChilds.Add(child2);
                        }

                        if (obj.childList != null)
                            markChilds(obj.childList, parentID);
                    }
                }
            }
            if (childList.Type == Newtonsoft.Json.Linq.JTokenType.Array)
                foreach (var obj in childList)
                {
                    if (obj.objName != null)
                    {
                        DBObjects _obj2 = allDbObj.Where(p => p.objName == Convert.ToString(obj.objName) && p.schemaName == Convert.ToString(obj.schemaName) && p.objType == types[Convert.ToString(obj.objType)]).FirstOrDefault();
                        DBObjectsChilds child3 = new DBObjectsChilds()
                        {
                            ChildID = _obj2.ID,
                            ParentID = parentID,
                        };

                        listChilds.Add(child3);
                    }

                    if (obj.childList != null)
                        markChilds(obj.childList, parentID);
                }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            string filePath = textBox1.Text;
            DirectoryInfo di = new DirectoryInfo(filePath);

            FileInfo[] files = di.GetFiles("*.json");
            IGumruk igumruk = new BSGumruk();
            listObj = new List<DBObjects>();
            int index = 0;
            dynamic dynJson;
            List<ObjType> objTypes = igumruk.GetTypes();
            types = new Hashtable();
            foreach (var item in objTypes)
            {
                types.Add(item.objType1, item.ID);
            }

            allDbObj = igumruk.GetAllDBObjects();

            for (int i = 0; i < files.Count(); i++)
            {
                index++;
                using (StreamReader sr = new StreamReader(files[i].FullName))
                {
                    string json = sr.ReadToEnd();
                    dynJson = JObject.Parse(json);
                    //dynJson = JsonConvert.DeserializeObject(json);

                    DBObjects obj = allDbObj.Where(p => p.objName == Convert.ToString(dynJson.objName) && p.schemaName == Convert.ToString(dynJson.schemaName) && p.objType == types[Convert.ToString(dynJson.objType)]).FirstOrDefault();

                    if (dynJson.childList != null && obj != null)
                        markChilds(dynJson.childList, obj.ID);
                }
            }

            igumruk.AddChildDbOject(listChilds);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            IGumruk iGumruk = new BSGumruk();
            List<Modules> Modules = new List<Gumruk.Entity.Modules>();

            Modules = iGumruk.GetModules();

            List<ModulesFiles> allFiles = new List<ModulesFiles>();
            allFiles = iGumruk.GetAllModulesFiles();

            List<InnerModule> innerModules = new List<InnerModule>();
            foreach (var itemModule in Modules)
            {
                foreach (var itemSubModule in itemModule.SubModules)
                {
                    foreach (var itemFile in itemSubModule.ModulesFiles)
                    {
                        string[] lines = itemFile.Body.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                        foreach (var line in lines)
                        {
                            if (line.Contains("Araclar.IsKuraliYarat"))
                            {
                                int startIndex = line.IndexOf("<");
                                int finishIndex = line.IndexOf(">");

                                string fileNameFull = line.Substring(startIndex + 1, finishIndex - startIndex - 1) + ".cs";
                                string filename = line.Substring(startIndex + 1, finishIndex - startIndex - 1);
                                List<ModulesFiles> files = allFiles.Where(p => p.Name == fileNameFull).ToList();

                                foreach (var item in files)
                                {
                                    if (item.Name == fileNameFull)
                                    {
                                        if (item.SubModules.Modules.Name != itemModule.Name)
                                        {
                                            //bu kısımda tabloya kayıt atacaz. 
                                            InnerModule innerMod = new InnerModule()
                                            {
                                                ContainModuleID = itemModule.ID,
                                                ModuleID = item.ModuleID,
                                                ModFileName = item.Name,
                                                Method = line,
                                                ContainFileName = itemFile.Name,
                                                SubContainsModuleID = itemSubModule.ID,
                                            };


                                            //daha önceden insert edildi ise tekrar etme.
                                            if (innerModules.Where(p => p.ModuleID == innerMod.ModuleID && p.ContainModuleID == innerMod.ContainModuleID).FirstOrDefault() == null)
                                                innerMod = iGumruk.AddInnerModule(innerMod);

                                            innerModules.Add(innerMod);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            IGumruk igumruk = new BSGumruk();

            List<ModulesFiles> files = igumruk.GetAllModulesFiles();

            int lineCount = 0;
            foreach (var item in files)
            {
                string[] lines = item.Body.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                lineCount += lines.Count();
            }

            label1.Text = lineCount.ToString();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            IGumruk igumruk = new BSGumruk();

            List<SQLProcedures> sps = igumruk.GetAllSP();

            int lineCount = 0;
            foreach (var item in sps.Where(p => p.name.Contains(".prc")))
            {
                string[] lines = item.body.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                lineCount += lines.Count();
            }

            label1.Text = lineCount.ToString();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            IGumruk igumruk = new BSGumruk();

            List<SQLProcedures> sps = igumruk.GetAllSP();

            int lineCount = 0;
            foreach (var item in sps.Where(p => p.name.Contains(".fnc")))
            {
                string[] lines = item.body.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                lineCount += lines.Count();
            }

            label1.Text = lineCount.ToString();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //Get all module files. Split into line. Add line number to each line.
            IGumruk igumruk = new BSGumruk();

            List<ModulesFiles> fileList = igumruk.GetAllModulesFiles(false);

            int lineCount = 0;
            foreach (var file in fileList)
            {
                file.Body = file.Body.Replace("<", "&#60;");
                lineCount = 0;
                string[] lines = file.Body.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                string newLine = "";
                for (int i = 0; i < lines.Count(); i++)
                {
                    lineCount++;
                    //    // line içerisinde "//" karakteri aranıyor. Bulunduğu yere color belirteci koyulacak.
                    int commentIndex = lines[i].IndexOf("//");
                    if (commentIndex > -1)
                        lines[i] = lines[i].Substring(0, commentIndex) + "<span class='green'>" + lines[i].Substring(commentIndex) + "</span>";


                    newLine += "<span class='line'>" + lineCount.ToString() + "</span> " + lines[i] + "\n";

                }

                file.formattedBody = newLine;
                file.LineCount = lineCount;

                igumruk.UpdateModulesFiles(file);
            }


        }

        private void button16_Click(object sender, EventArgs e)
        {
            //Get all stored procedures with schemanames.
            IGumruk igumruk = new BSGumruk();

            List<SQLProcedures> SPlist = igumruk.GetAllSP(true);

            List<ModulesFiles> modulesFiles = igumruk.GetAllModulesFiles(false);
            string SPname = "";
            foreach (var item in SPlist)
            {
                SPname = item.schemas.name.ToUpperInvariant() + "." + item.name.ToUpperInvariant().Substring(0, item.name.Length - 4);

                foreach (var itemFile in modulesFiles)
                {
                    string body = itemFile.Body.ToUpperInvariant();
                    string[] lines = itemFile.Body.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                    int lineCount = 0;
                    for (int i = 0; i < lines.Count(); i++)
                    {
                        lineCount++;
                        if (lines[i].Contains(SPname))
                        {
                            SQLProcedureModules sqlProceduresNew = new SQLProcedureModules()
                            {
                                FileID = itemFile.ID,
                                lineNumber = lineCount,
                                ModuleID = (int)itemFile.ModuleID,
                                SQLProcedureID = item.ID,
                                SubModuleID = (int)itemFile.SubModuleID,
                            };

                            igumruk.SaveSqlProcedureModules(sqlProceduresNew);
                        }
                    }
                }
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            IGumruk igumruk = new BSGumruk();

            List<d_b_tables> SPlist = igumruk.GetAllTables();

            List<ModulesFiles> modulesFiles = igumruk.GetAllModulesFiles(false);
            string tablename = "";
            foreach (var item in SPlist)
            {
                tablename = item.d_b_schemas.name + "." + item.name;

                foreach (var itemFile in modulesFiles)
                {
                    string[] lines = itemFile.Body.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                    int lineCount = 0;
                    for (int i = 0; i < lines.Count(); i++)
                    {
                        lineCount++;
                        if (lines[i].Contains(tablename))
                        {
                            ModulesTables sqlProceduresNew = new ModulesTables()
                            {
                                d_b_tablesID = item.id,
                                fileID = itemFile.ID,
                                lineNumber = lineCount,
                                moduleID = itemFile.ModuleID,
                                subModuleID = itemFile.SubModuleID,
                            };

                            igumruk.SaveModuleTable(sqlProceduresNew);
                        }
                    }
                }
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            IGumruk igumruk = new BSGumruk();
            List<ModulesFiles> modFiles = igumruk.GetAllModulesFiles(false);

            foreach (var file in modFiles)
            {
                file.formattedBody = file.Body.Replace("<", "&#60;");
                int lineCount = 0;
                string[] lines = file.formattedBody.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                string newLine = "";
                for (int i = 0; i < lines.Count(); i++)
                {
                    lineCount++;
                    //lines[i] = lines[i].Replace("/*", "<span class='green'>/*");
                    //lines[i] = lines[i].Replace("*/", "*/</span>");

                    // line içerisinde "//" karakteri aranıyor. Bulunduğu yere color belirteci koyulacak.
                    int commentIndex = lines[i].IndexOf("//");
                    if (commentIndex > -1)
                        lines[i] = lines[i].Substring(0, commentIndex) + "<span class='green'>" + lines[i].Substring(commentIndex) + "</span>";

                    newLine += "<span style='color:#4bc6d1;'>" + lineCount.ToString() + "</span> " + lines[i] + "\n";
                }

                file.formattedBody = newLine;

                igumruk.UpdateModulesFiles(file);
            }

        }

        private void button19_Click(object sender, EventArgs e)
        {
            IGumruk igumruk = new BSGumruk();
            string file = File.ReadAllText("D:\\degisentables.txt");

            string[] lines = file.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            List<d_b_columns> columns;
            foreach (var line in lines)
            {
                string[] col = line.Split(',');

                columns = igumruk.GetColumnByName(col[2]);

            }
        }

        private void button20_Click(object sender, EventArgs e)
        {


            createtable();


        }

        private void createtable()
        {
            Word._Application objApp;
            Word._Document objDoc;
            try
            {
                object objMiss = System.Reflection.Missing.Value;
                object objEndOfDocFlag = "\\endofdoc"; /* \endofdoc is a predefined bookmark */

                //Start Word and create a new document.
                objApp = new Word.Application();
                objApp.Visible = true;
                objDoc = objApp.Documents.Add(ref objMiss, ref objMiss,
                    ref objMiss, ref objMiss);

                //Insert a paragraph at the end of the document.
                Word.Paragraph objPara2; //define paragraph object
                object oRng = objDoc.Bookmarks.get_Item(ref objEndOfDocFlag).Range; //go to end of the page
                objPara2 = objDoc.Content.Paragraphs.Add(ref oRng); //add paragraph at end of document
                objPara2.Range.Text = "Test Table Caption"; //add some text in paragraph
                objPara2.Format.SpaceAfter = 10; //defind some style
                objPara2.Range.InsertParagraphAfter(); //insert paragraph

                int basSchemaID = int.Parse(txtBasSchemaID.Text);
                int sonSchemaID = int.Parse(txtSonSchemaID.Text);

                IGumruk igumruk = new BSGumruk();
                List<d_b_schemas> schemas = igumruk.GetAllSchemas(true).Where(p => p.id >= basSchemaID && p.id <= sonSchemaID).ToList();

                //foreach (var schema in schemas)
                //{
                //    foreach (var table in schema.d_b_tables)
                //    {
                //        Word.Table objTab1; //create table object
                //        Word.Range objWordRng = objDoc.Bookmarks.get_Item(ref objEndOfDocFlag).Range; //go to end of document
                //                                                                                      //Insert a 2 x 2 table, (table with 2 row and 2 column)

                //        objTab1 = objDoc.Tables.Add(objWordRng, 7, 2, ref objMiss, ref objMiss); //add table object in word document
                //        objTab1.Range.ParagraphFormat.SpaceAfter = 6;

                //        objTab1.Cell(1, 1).Range.Text = "Metaveri Adı"; 
                //        objTab1.Cell(1, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(1, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(1, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(1, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;


                //        objTab1.Cell(2, 1).Range.Text = "Metaveri Türü"; 
                //        objTab1.Cell(2, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(2, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(2, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(2, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                //        objTab1.Cell(3, 1).Range.Text = "Veri Elemanı"; 
                //        objTab1.Cell(3, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(3, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(3, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(3, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                //        objTab1.Cell(4, 1).Range.Text = "Veri Seti"; 
                //        objTab1.Cell(4, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(4, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(4, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(4, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                //        objTab1.Cell(5, 1).Range.Text = "Tanımı"; 
                //        objTab1.Cell(5, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(5, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(5, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(5, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                //        objTab1.Cell(6, 1).Range.Text = "Açıklama"; 
                //        objTab1.Cell(6, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(6, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(6, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(6, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;


                //        objTab1.Cell(7, 1).Range.Text = "Güncelleme Zamanı/Periyodu"; 
                //        objTab1.Cell(7, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(7, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(7, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(7, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                //        //columnlar şekilleniyor
                //        objTab1.Cell(1, 2).Range.Text = schema.name; 
                //        objTab1.Cell(1, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(1, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(1, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(1, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                //        objTab1.Cell(2, 2).Range.Text = table.name; 
                //        objTab1.Cell(2, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(2, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(2, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(2, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                //        if (table.details != null)
                //            objTab1.Cell(3, 2).Range.Text = table.details; 
                //        else if (table.d_b_table_explanation.Count > 0)
                //            objTab1.Cell(3, 2).Range.Text = table.d_b_table_explanation.ToList()[0].details; 
                //        else
                //            objTab1.Cell(3, 2).Range.Text = ""; 
                //        objTab1.Cell(3, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(3, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(3, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(3, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                //        objTab1.Cell(4, 2).Range.Text = "Uygulama"; 
                //        objTab1.Cell(4, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(4, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(4, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(4, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                //        objTab1.Cell(5, 2).Range.Text = "Anlık"; 
                //        objTab1.Cell(5, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(5, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(5, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(5, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                //        objTab1.Cell(6, 2).Range.Text = ""; 
                //        objTab1.Cell(6, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(6, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(6, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(6, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                //        //table.d_b_columns.ToList()[0].d_b_column_lookups.ToList()[0].d_b_Column_to.d_b_tables.name

                //        objTab1.Cell(7, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(7, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(7, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //        objTab1.Cell(7, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                //        objWordRng = objDoc.Bookmarks.get_Item(ref objEndOfDocFlag).Range;
                //        objWordRng.InsertParagraphAfter(); //put enter in document
                //        if (table.d_b_columns.Count > 0)
                //            objWordRng.InsertAfter("Değişkenler");
                //        else
                //            objWordRng.InsertAfter("");

                //    }
                //}


                foreach (var schema in schemas)
                {
                    foreach (var table in schema.d_b_tables)
                    {
                        if (table.id == 6390 || table.id == 6391 || table.id == 6392 || table.id == 6393 || table.id == 6394 || table.id == 6395 || table.id == 6401 || table.id == 6400 || table.isempty == true || table.isView == true || table.name.Contains("ESKI") || table.name.Contains("YEDEK"))
                            continue;

                        Word.Table objTab1; //create table object
                        Word.Range objWordRng = objDoc.Bookmarks.get_Item(ref objEndOfDocFlag).Range; //go to end of document
                                                                                                      //Insert a 2 x 2 table, (table with 2 row and 2 column)

                        objTab1 = objDoc.Tables.Add(objWordRng, 6, 2, ref objMiss, ref objMiss); //add table object in word document
                        objTab1.Range.ParagraphFormat.SpaceAfter = 6;

                        objTab1.Cell(1, 1).Range.Text = "Schema Adı";
                        objTab1.Cell(1, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(1, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(1, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(1, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;


                        objTab1.Cell(2, 1).Range.Text = "Tablo Adı";
                        objTab1.Cell(2, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(2, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(2, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(2, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        objTab1.Cell(3, 1).Range.Text = "Tablo Tanımı";
                        objTab1.Cell(3, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(3, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(3, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(3, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        objTab1.Cell(4, 1).Range.Text = "Veri Kaynağı";
                        objTab1.Cell(4, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(4, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(4, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(4, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        objTab1.Cell(5, 1).Range.Text = "Güncelleme Periyodu";
                        objTab1.Cell(5, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(5, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(5, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(5, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        objTab1.Cell(6, 1).Range.Text = "Tekil Tanımı";
                        objTab1.Cell(6, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(6, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(6, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(6, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;


                        objTab1.Cell(7, 1).Range.Text = "Bağlı Tablolar";
                        objTab1.Cell(7, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(7, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(7, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(7, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        //columnlar şekilleniyor
                        objTab1.Cell(1, 2).Range.Text = schema.name;
                        objTab1.Cell(1, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(1, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(1, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(1, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        objTab1.Cell(2, 2).Range.Text = table.name;
                        objTab1.Cell(2, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(2, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(2, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(2, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        if (table.details != null)
                            objTab1.Cell(3, 2).Range.Text = table.details;
                        else if (table.d_b_table_explanation.Count > 0)
                            objTab1.Cell(3, 2).Range.Text = table.d_b_table_explanation.ToList()[0].details;
                        else
                            objTab1.Cell(3, 2).Range.Text = "";
                        objTab1.Cell(3, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(3, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(3, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(3, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        objTab1.Cell(4, 2).Range.Text = "Uygulama";
                        objTab1.Cell(4, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(4, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(4, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(4, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        objTab1.Cell(5, 2).Range.Text = "Anlık";
                        objTab1.Cell(5, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(5, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(5, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(5, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        objTab1.Cell(6, 2).Range.Text = "";
                        objTab1.Cell(6, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(6, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(6, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(6, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        //table.d_b_columns.ToList()[0].d_b_column_lookups.ToList()[0].d_b_Column_to.d_b_tables.name

                        objTab1.Cell(7, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(7, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(7, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        objTab1.Cell(7, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        objWordRng = objDoc.Bookmarks.get_Item(ref objEndOfDocFlag).Range;
                        objWordRng.InsertParagraphAfter(); //put enter in document
                        if (table.d_b_columns.Count > 0)
                            objWordRng.InsertAfter("Değişkenler");
                        else
                            objWordRng.InsertAfter("");

                        string baglantiliTablolar = "";

                        int a = 1;
                        if (table.d_b_columns.Count > 0)
                        {
                            Word.Table objTab2; //create table object
                            Word.Range objWordRng2 = objDoc.Bookmarks.get_Item(ref objEndOfDocFlag).Range; //go to end of document

                            objTab2 = objDoc.Tables.Add(objWordRng2, table.d_b_columns.Count + 1, 2, ref objMiss, ref objMiss); //add table object in word document
                            objTab2.Range.ParagraphFormat.SpaceAfter = 6;

                            objTab2.Cell(1, 1).Range.Text = "Değişken Adı";
                            objTab2.Cell(1, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                            objTab2.Cell(1, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                            objTab2.Cell(1, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                            objTab2.Cell(1, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;


                            objTab2.Cell(1, 2).Range.Text = "Değişken Tanımı";
                            objTab2.Cell(1, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                            objTab2.Cell(1, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                            objTab2.Cell(1, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                            objTab2.Cell(1, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                            foreach (var column in table.d_b_columns)
                            {
                                a++;
                                objTab2.Cell(a, 1).Range.Text = column.name;
                                objTab2.Cell(a, 1).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                                objTab2.Cell(a, 1).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                                objTab2.Cell(a, 1).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                                objTab2.Cell(a, 1).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                                if (column.d_b_column_explanations.Count > 0)
                                    objTab2.Cell(a, 2).Range.Text = column.d_b_column_explanations.ToList()[0].details;
                                else
                                    objTab2.Cell(a, 2).Range.Text = "";

                                objTab2.Cell(a, 2).Range.Borders[Word.WdBorderType.wdBorderLeft].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                                objTab2.Cell(a, 2).Range.Borders[Word.WdBorderType.wdBorderRight].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                                objTab2.Cell(a, 2).Range.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                                objTab2.Cell(a, 2).Range.Borders[Word.WdBorderType.wdBorderTop].LineStyle = Word.WdLineStyle.wdLineStyleSingle;

                                if (column.d_b_column_lookups.Count > 0)
                                {
                                    foreach (var lookup in column.d_b_column_lookups)
                                    {
                                        d_b_columns dbCol = igumruk.GetColumnById(lookup.column_to_id);
                                        baglantiliTablolar = baglantiliTablolar + " " + dbCol.d_b_tables.name;
                                        objTab1.Cell(7, 2).Range.Text = baglantiliTablolar;
                                    }
                                }
                            }
                            objWordRng2 = objDoc.Bookmarks.get_Item(ref objEndOfDocFlag).Range;
                            objWordRng2.InsertParagraphAfter(); //put enter in document
                            objWordRng2.InsertAfter("");

                        }
                    }
                }


                //Add some text after table


                object szPath = "test.docx"; //your file gets saved with name 'test.docx'
                objDoc.SaveAs(ref szPath);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error occurred while executing code : " + ex.Message);
            }
            finally
            {
                //you can dispose object here
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            IGumruk iGumruk = new BSGumruk();

            List<d_b_columns> columns1 = iGumruk.GetAllColumns();
            //List<d_b_tables> tables;


            string filename = "D:\\Columns.xlsx";

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

            int a = 0;

            dt.WriteXml("D:\\columns.xlsx");
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

        private void button22_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"D:\";
            openFileDialog1.Title = "Browse Excel Files";

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowReadOnly = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
            else
                return;

            FileStream file = File.OpenRead(textBox1.Text);
            string filename = file.Name;


            string[] newfilename = filename.Split('\\');

            filename = newfilename[newfilename.Count() - 1];


            newfilename = filename.Split('.');

            filename = newfilename[0];

            using (var package = new ExcelPackage(file))
            {
                var currentSheet = package.Workbook.Worksheets;
                string sheetName = string.Empty;

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

                string frekans;
                string mevzuat;
                string referans;
                string referansKaynak;
                string referansVeri;
                string referansVeriListesi;

                List<MetaTable> listMetaTable = new List<MetaTable>();
                MetaTable metaTbl = null;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tableExp = (workSheet.Cells[rowIterator, 1].Value == null) ? "" : workSheet.Cells[rowIterator, 1].Value.ToString();
                    tableName = ((workSheet.Cells[rowIterator, 2].Value == null) ? "" : workSheet.Cells[rowIterator, 2].Value.ToString()).ToUpperInvariant();
                    columnName = (workSheet.Cells[rowIterator, 3].Value == null) ? "" : workSheet.Cells[rowIterator, 3].Value.ToString();
                    columnExp = (workSheet.Cells[rowIterator, 4].Value == null) ? "" : workSheet.Cells[rowIterator, 4].Value.ToString();
                    datatype = (workSheet.Cells[rowIterator, 5].Value == null) ? "" : workSheet.Cells[rowIterator, 5].Value.ToString().ToUpperInvariant();
                    uzunluk = (workSheet.Cells[rowIterator, 6].Value == null) ? "" : workSheet.Cells[rowIterator, 6].Value.ToString();
                    lookupTable = ((workSheet.Cells[rowIterator, 7].Value == null) ? "" : workSheet.Cells[rowIterator, 7].Value.ToString()).ToUpperInvariant();
                    LinkColumn = (workSheet.Cells[rowIterator, 8].Value == null) ? "" : workSheet.Cells[rowIterator, 8].Value.ToString();
                    toColumn = (workSheet.Cells[rowIterator, 9].Value == null) ? "" : workSheet.Cells[rowIterator, 9].Value.ToString();
                    LinkType = (workSheet.Cells[rowIterator, 10].Value == null) ? "" : workSheet.Cells[rowIterator, 10].Value.ToString();
                    pk = (workSheet.Cells[rowIterator, 11].Value == null) ? "" : workSheet.Cells[rowIterator, 11].Value.ToString();

                    frekans = (workSheet.Cells[rowIterator, 12].Value == null) ? "" : workSheet.Cells[rowIterator, 12].Value.ToString(); ;
                    mevzuat = (workSheet.Cells[rowIterator, 13].Value == null) ? "" : workSheet.Cells[rowIterator, 13].Value.ToString();
                    referans = (workSheet.Cells[rowIterator, 14].Value == null) ? "" : workSheet.Cells[rowIterator, 14].Value.ToString();
                    referansKaynak = (workSheet.Cells[rowIterator, 15].Value == null) ? "" : workSheet.Cells[rowIterator, 15].Value.ToString();
                    referansVeri = (workSheet.Cells[rowIterator, 16].Value == null) ? "" : workSheet.Cells[rowIterator, 16].Value.ToString();
                    referansVeriListesi = (workSheet.Cells[rowIterator, 17].Value == null) ? "" : workSheet.Cells[rowIterator, 17].Value.ToString();

                    if (tableExp != string.Empty && tableName != string.Empty)
                    {
                        metaTbl = new MetaTable();
                        metaTbl.Name = tableName;
                        metaTbl.Aciklama = tableExp;

                        listMetaTable.Add(metaTbl);
                    }

                    if (metaTbl != null)
                    {
                        if (columnName != string.Empty)
                        {
                            MetaColumn metaColumn = new MetaColumn();
                            metaColumn.name = columnName;
                            metaColumn.frekans = frekans;
                            metaColumn.mevzuat = mevzuat;
                            metaColumn.referans = referans;
                            metaColumn.referansKaynak = referansKaynak;
                            metaColumn.referansVeriListesi = referansVeriListesi;
                            metaColumn.tanimi = columnExp;
                            metaColumn.PK = (pk == "true" ? true : false);
                            metaColumn.VeriTipi = datatype;
                            metaColumn.Uzunluk = uzunluk;
                            if (metaTbl.MetaColumns == null)
                                metaTbl.MetaColumns = new List<MetaColumn>();

                            metaTbl.MetaColumns.Add(metaColumn);
                        }
                    }
                } // for end

                PrepareWordDocument(listMetaTable, filename);

                MessageBox.Show("Tablo Sayısı:" + listMetaTable.Count().ToString());
            }
        }

        public void PrepareWordDocument(List<MetaTable> tables, string filename)
        {
            Word._Application objApp;
            Word._Document objDoc;

            object objMiss = System.Reflection.Missing.Value;
            object objEndOfDocFlag = "\\endofdoc"; /* \endofdoc is a predefined bookmark */

            filename = filename.ToUpperInvariant();
            //Start Word and create a new document.
            objApp = new Word.Application();
            objApp.Visible = true;
            objDoc = objApp.Documents.Add(ref objMiss, ref objMiss,
                ref objMiss, ref objMiss);

            //Insert a paragraph at the end of the document.
            Word.Paragraph objPara2; //define paragraph object
            object oRng = objDoc.Bookmarks.get_Item(ref objEndOfDocFlag).Range; //go to end of the page
            objPara2 = objDoc.Content.Paragraphs.Add(ref oRng); //add paragraph at end of document
            objPara2.Range.Text = filename + " METAVERİ DÖKÜMANI";
            objPara2.Format.SpaceAfter = 10; //defind some style
            objPara2.Range.InsertParagraphAfter(); //insert paragraph

            int index = 0;
            foreach (var table in tables)
            {
                index++;
                Word.Table objTab1; //create table object
                Word.Range objWordRng = objDoc.Bookmarks.get_Item(ref objEndOfDocFlag).Range; //go to end of document
                //Insert a 2 x 2 table, (table with 2 row and 2 column)

                objTab1 = objDoc.Tables.Add(objWordRng, 7, 2, ref objMiss, ref objMiss); //add table object in word document
                objTab1.Range.ParagraphFormat.SpaceAfter = 6;

                objTab1.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                objTab1.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                objTab1.Cell(1, 1).Range.Shading.BackgroundPatternColor = (Word.WdColor)Color.FromArgb(0, 240, 240, 240).ToArgb();
                objTab1.Cell(2, 1).Range.Shading.BackgroundPatternColor = (Word.WdColor)Color.FromArgb(0, 240, 240, 240).ToArgb();
                objTab1.Cell(3, 1).Range.Shading.BackgroundPatternColor = (Word.WdColor)Color.FromArgb(0, 240, 240, 240).ToArgb();
                objTab1.Cell(4, 1).Range.Shading.BackgroundPatternColor = (Word.WdColor)Color.FromArgb(0, 240, 240, 240).ToArgb();
                objTab1.Cell(5, 1).Range.Shading.BackgroundPatternColor = (Word.WdColor)Color.FromArgb(0, 240, 240, 240).ToArgb();
                objTab1.Cell(6, 1).Range.Shading.BackgroundPatternColor = (Word.WdColor)Color.FromArgb(0, 240, 240, 240).ToArgb();
                objTab1.Cell(7, 1).Range.Shading.BackgroundPatternColor = (Word.WdColor)Color.FromArgb(0, 240, 240, 240).ToArgb();

                objTab1.Cell(1, 1).SetWidth(objApp.Application.CentimetersToPoints(3f), Word.WdRulerStyle.wdAdjustNone);
                objTab1.Cell(2, 1).SetWidth(objApp.Application.CentimetersToPoints(3f), Word.WdRulerStyle.wdAdjustNone);
                objTab1.Cell(3, 1).SetWidth(objApp.Application.CentimetersToPoints(3f), Word.WdRulerStyle.wdAdjustNone);
                objTab1.Cell(4, 1).SetWidth(objApp.Application.CentimetersToPoints(3f), Word.WdRulerStyle.wdAdjustNone);
                objTab1.Cell(5, 1).SetWidth(objApp.Application.CentimetersToPoints(3f), Word.WdRulerStyle.wdAdjustNone);
                objTab1.Cell(6, 1).SetWidth(objApp.Application.CentimetersToPoints(3f), Word.WdRulerStyle.wdAdjustNone);
                objTab1.Cell(7, 1).SetWidth(objApp.Application.CentimetersToPoints(3f), Word.WdRulerStyle.wdAdjustNone);

                objTab1.Cell(1, 1).Range.Text = "Tablo Adı";
                objTab1.Cell(2, 1).Range.Text = "Tablo Tanımı";
                objTab1.Cell(3, 1).Range.Text = "Tablo Tipi";
                objTab1.Cell(4, 1).Range.Text = "Referans";
                objTab1.Cell(5, 1).Range.Text = "Referans Kaynak";
                objTab1.Cell(6, 1).Range.Text = "Frekans";
                objTab1.Cell(7, 1).Range.Text = "Mevzuat";

                objTab1.Cell(1, 2).SetWidth(objApp.Application.CentimetersToPoints(14f), Word.WdRulerStyle.wdAdjustNone);
                objTab1.Cell(2, 2).SetWidth(objApp.Application.CentimetersToPoints(14f), Word.WdRulerStyle.wdAdjustNone);
                objTab1.Cell(3, 2).SetWidth(objApp.Application.CentimetersToPoints(14f), Word.WdRulerStyle.wdAdjustNone);
                objTab1.Cell(4, 2).SetWidth(objApp.Application.CentimetersToPoints(14f), Word.WdRulerStyle.wdAdjustNone);
                objTab1.Cell(5, 2).SetWidth(objApp.Application.CentimetersToPoints(14f), Word.WdRulerStyle.wdAdjustNone);
                objTab1.Cell(6, 2).SetWidth(objApp.Application.CentimetersToPoints(14f), Word.WdRulerStyle.wdAdjustNone);
                objTab1.Cell(7, 2).SetWidth(objApp.Application.CentimetersToPoints(14f), Word.WdRulerStyle.wdAdjustNone);

                //columnlar şekilleniyor
                objTab1.Cell(1, 2).Range.Text = table.Name;
                objTab1.Cell(2, 2).Range.Text = table.Aciklama;
                if (table.MetaColumns[0].referans == "Referans" && table.MetaColumns[0].referansKaynak == "BİLGE GÜMRÜK Referans Verileri")
                    objTab1.Cell(3, 2).Range.Text = "View";
                else
                    objTab1.Cell(3, 2).Range.Text = "Tablo";

                //referans bilgisi tablo bilgileri arasında yer alacak. Tablonun columnları arasında yapılacak olan bir sorgu ile referans bilgisi boş olmayan ilk column'ın değeri alınıp yazılacak.
                objTab1.Cell(4, 2).Range.Text = table.MetaColumns[0].referans;
                objTab1.Cell(5, 2).Range.Text = table.MetaColumns[0].referansKaynak;
                objTab1.Cell(6, 2).Range.Text = table.MetaColumns[0].frekans;
                objTab1.Cell(7, 2).Range.Text = table.MetaColumns[0].mevzuat;

                objWordRng = objDoc.Bookmarks.get_Item(ref objEndOfDocFlag).Range;
                objWordRng.InsertParagraphAfter(); //put enter in document
                if (table.MetaColumns.Count > 0)
                    objWordRng.InsertAfter("Tablo Değişkenleri");
                else
                    objWordRng.InsertAfter("");

                int a = 1;
                if (table.MetaColumns.Count > 0)
                {
                    Word.Table objTab2; //create table object
                    Word.Range objWordRng2 = objDoc.Bookmarks.get_Item(ref objEndOfDocFlag).Range; //go to end of document

                    objTab2 = objDoc.Tables.Add(objWordRng2, table.MetaColumns.Count + 1, 5, ref objMiss, ref objMiss); //add table object in word document
                    objTab2.Range.ParagraphFormat.SpaceAfter = 6;

                    objTab2.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    objTab2.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

                    objTab2.Cell(1, 1).Range.Shading.BackgroundPatternColor = (Word.WdColor)Color.FromArgb(0, 240, 240, 240).ToArgb();
                    objTab2.Cell(1, 2).Range.Shading.BackgroundPatternColor = (Word.WdColor)Color.FromArgb(0, 240, 240, 240).ToArgb();
                    objTab2.Cell(1, 3).Range.Shading.BackgroundPatternColor = (Word.WdColor)Color.FromArgb(0, 240, 240, 240).ToArgb();
                    objTab2.Cell(1, 4).Range.Shading.BackgroundPatternColor = (Word.WdColor)Color.FromArgb(0, 240, 240, 240).ToArgb();
                    objTab2.Cell(1, 5).Range.Shading.BackgroundPatternColor = (Word.WdColor)Color.FromArgb(0, 240, 240, 240).ToArgb();

                    objTab2.Cell(1, 1).SetWidth(objApp.Application.CentimetersToPoints(4f), Word.WdRulerStyle.wdAdjustNone);
                    objTab2.Cell(1, 2).SetWidth(objApp.Application.CentimetersToPoints(6f), Word.WdRulerStyle.wdAdjustNone);
                    objTab2.Cell(1, 3).SetWidth(objApp.Application.CentimetersToPoints(2f), Word.WdRulerStyle.wdAdjustNone);
                    objTab2.Cell(1, 4).SetWidth(objApp.Application.CentimetersToPoints(1f), Word.WdRulerStyle.wdAdjustNone);
                    objTab2.Cell(1, 5).SetWidth(objApp.Application.CentimetersToPoints(4f), Word.WdRulerStyle.wdAdjustNone);


                    objTab2.Cell(1, 1).Range.Text = "Değişken Adı";
                    objTab2.Cell(1, 2).Range.Text = "Değişken Tanımı";
                    objTab2.Cell(1, 3).Range.Text = "Değişken Tipi";
                    objTab2.Cell(1, 4).Range.Text = "Uzunluk";
                    objTab2.Cell(1, 5).Range.Text = "Referans Veri Listesi";

                    foreach (var column in table.MetaColumns)
                    {
                        a++;

                        objTab2.Cell(a, 1).SetWidth(objApp.Application.CentimetersToPoints(4f), Word.WdRulerStyle.wdAdjustNone);
                        objTab2.Cell(a, 2).SetWidth(objApp.Application.CentimetersToPoints(6f), Word.WdRulerStyle.wdAdjustNone);
                        objTab2.Cell(a, 3).SetWidth(objApp.Application.CentimetersToPoints(2f), Word.WdRulerStyle.wdAdjustNone);
                        objTab2.Cell(a, 4).SetWidth(objApp.Application.CentimetersToPoints(1f), Word.WdRulerStyle.wdAdjustNone);
                        objTab2.Cell(a, 5).SetWidth(objApp.Application.CentimetersToPoints(4f), Word.WdRulerStyle.wdAdjustNone);

                        objTab2.Cell(a, 1).Range.Text = column.name + (column.PK == true ? " (Primary Key)" : "");
                        objTab2.Cell(a, 2).Range.Text = column.tanimi;
                        objTab2.Cell(a, 3).Range.Text = column.VeriTipi;
                        objTab2.Cell(a, 4).Range.Text = column.Uzunluk;
                        objTab2.Cell(a, 5).Range.Text = column.referansVeriListesi;


                    }

                    objWordRng2 = objDoc.Bookmarks.get_Item(ref objEndOfDocFlag).Range;
                    objWordRng2.InsertParagraphAfter(); //put enter in document
                    objWordRng2.InsertAfter("");
                }

            }

            object szPath = "D:\\Metaveriler\\" + filename+" Metaveri"; //your file gets saved with name 'test.docx'
            objDoc.SaveAs(ref szPath);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gumruk.Entity;
using Gumruk.Repository;
using Gumruk.UnitOfWork.Contract;
using Gumruk.Entity.Model;
using System.Xml;
using System.Text.RegularExpressions;

namespace Gumruk.UnitOfWork
{
    public class BSGumruk : IGumruk, IDisposable
    {
        #region Get Methods
        public List<d_b_tables> GetAllSchemasTables(int schemaId, bool getColumns)
        {
            IRepositoryBase<d_b_tables> _rep = new RepositoryBase<d_b_tables>();

            List<d_b_tables> response = new List<d_b_tables>();

            response = _rep.GetList(p => p.d_b_schemasId == schemaId).ToList();

            //response = _rep.AllIncluding<d_b_tables>(p => p.d_b_columns).Where(x => x.d_b_schemasId == schemaId).ToList();

            if (!getColumns)  // if doesn't wants to get columns , do not get columns
                return response;

            IRepositoryBase<d_b_columns> _repColumn = new RepositoryBase<d_b_columns>();

            foreach (var item in response)
            {
                item.d_b_columns = _repColumn.GetList(p => p.d_b_tablesId == item.id).ToList();
            }

            return response;
        }

        public List<d_b_columns> GetAllColumns(int TableID)
        {
            IRepositoryBase<d_b_columns> _rep = new RepositoryBase<d_b_columns>();

            List<d_b_columns> response = new List<d_b_columns>();

            response = _rep.GetList(p => p.d_b_tablesId == TableID).ToList();

            foreach (var item in response)
            {
                foreach (var item2 in item.d_b_column_lookups)
                {
                    item2.d_b_Column_to = _rep.Get(p => p.id == item2.column_to_id);
                }
            }
            return response;
        }

        public List<d_b_columns> GetAllColumns()
        {
            IRepositoryBase<d_b_columns> _rep = new RepositoryBase<d_b_columns>();

            List<d_b_columns> response = new List<d_b_columns>();

            response = _rep.GetList().ToList();

            foreach (var item in response)
            {
                foreach (var item2 in item.d_b_column_lookups)
                {
                    item2.d_b_Column_to = _rep.Get(p => p.id == item2.column_to_id);
                }
            }
            return response;
        }

        public List<DataTable> GetAllSchemas(bool getTables, bool getTableColumns, int schemaID)
        {
            IRepositoryBase<d_b_schemas> _rep = new RepositoryBase<d_b_schemas>();

            d_b_schemas response = new d_b_schemas();

            //if (getTables)
            //    response = _rep.AllIncluding<d_b_schemas>(p => p.d_b_tables).ToList();
            //else
            response = _rep.Get(p => p.mid == schemaID); ;

            //response.d_b_tables=new List<d_b_tables>();

            //response.d_b_tables = GetAllSchemasTables(response.id, false);

            List<DataSchema> semas = new List<DataSchema>();

            //DataSchema dschema = new DataSchema();
            //dschema.value = response.name;

            //dschema.data = new List<DataTable>();

            List<DataTable> listdt = new List<DataTable>();

            foreach (var xx in response.d_b_tables)
            {
                DataTable dt = new DataTable();
                if (!(bool)xx.isView)
                    dt.value = (xx.name + " (Column Count : " + xx.d_b_columns.Count() + ")");
                else
                    dt.value = ("<span style='background-color:#db0050;color:#fff;'> " + xx.name + "(Column Count : " + xx.d_b_columns.Count() + ")</span>");

                if (xx.isempty != null && (bool)xx.isempty)
                    dt.value = ("<span style='background-color:#ddd;color:#444;'> " + xx.name + "(Column Count : " + xx.d_b_columns.Count() + ")</span>");

                dt.id = xx.id * 0.123123;

                dt.data = new List<DataColumn>();
                if (xx.d_b_columns != null && xx.d_b_columns.Count > 0)
                    foreach (var xxx in xx.d_b_columns) //for columns 
                    {
                        DataColumn dc = new DataColumn();
                        if (xxx.isempty != null && (bool)xxx.isempty)
                            dc.value = "<span style='background-color:#aaa;color:#444;'> " + xxx.name + "</span>";
                        else
                            dc.value = xxx.name;

                        dc.id = xxx.id;

                        dt.data.Add(dc);
                    }
                listdt.Add(dt);
            }
            return listdt;
        }

        public List<DataTable> GetAllSchemasWitERs(bool getTables, bool getTableColumns, int schemaID)
        {
            IRepositoryBase<d_b_schemas> _rep = new RepositoryBase<d_b_schemas>();

            d_b_schemas response = new d_b_schemas();

            //if (getTables)
            //    response = _rep.AllIncluding<d_b_schemas>(p => p.d_b_tables).ToList();
            //else
            response = _rep.Get(p => p.mid == schemaID); ;

            //response.d_b_tables=new List<d_b_tables>();

            //response.d_b_tables = GetAllSchemasTables(response.id, false);

            List<DataSchema> semas = new List<DataSchema>();

            //DataSchema dschema = new DataSchema();
            //dschema.value = response.name;

            //dschema.data = new List<DataTable>();

            List<DataTable> listdt = new List<DataTable>();

            foreach (var xx in response.d_b_tables)
            {
                DataTable dt = new DataTable();
                if (!(bool)xx.isView)
                    dt.value = (xx.name + " (Column Count : " + xx.d_b_columns.Count() + ")");
                else
                    dt.value = ("<span style='background-color:#db0050;color:#fff;'> " + xx.name + "(Column Count : " + xx.d_b_columns.Count() + ")</span>");

                if (xx.isempty != null && (bool)xx.isempty)
                    dt.value = ("<span style='background-color:#ddd;color:#444;'> " + xx.name + "(Column Count : " + xx.d_b_columns.Count() + ")</span>");

                dt.id = xx.id * 0.123123;

                dt.data = new List<DataColumn>();
                if (xx.d_b_columns != null && xx.d_b_columns.Count > 0)
                    foreach (var xxx in xx.d_b_columns) //for columns 
                    {
                        DataColumn dc = new DataColumn();
                        if (xxx.isempty != null && (bool)xxx.isempty)
                            dc.value = "<span style='background-color:#aaa;color:#444;'> " + xxx.name + "</span>";
                        else
                            dc.value = xxx.name;

                        dc.id = xxx.id;

                        dt.data.Add(dc);
                    }

                listdt.Add(dt);
            }



            return listdt;
        }

        public List<ERxml> GetSavedDiagramsBySchemaID(int schemaID)
        {
            //şemanın kayıtlı er diagramlarını al.
            //id'lerini -1 ile çarparak datatable olarak ekle.

            IRepositoryBase<ERxml> _repXml = new RepositoryBase<ERxml>();

            List<ERxml> ers = _repXml.GetList(p => p.schemaid == schemaID);

            return ers;
        }
        public DataTable GetTableByName(string TableName)
        {
            string[] str = TableName.Split(' ');

            IRepositoryBase<d_b_tables> _rep = new RepositoryBase<d_b_tables>();

            string tableName = str[0].ToString();

            d_b_tables dt = _rep.Get(x => x.name == tableName);

            DataTable dtreturn = new DataTable()
            {
                value = dt.name,
                id = dt.id
            };

            return dtreturn;
        }

        public d_b_columns GetColumnById(int columnID)
        {
            IRepositoryBase<d_b_columns> _rep = new RepositoryBase<d_b_columns>();

            d_b_columns dbColumns = _rep.Get(x => x.id == columnID);

            return dbColumns;
        }

        public d_b_tables GetTableById(int tableID)
        {
            IRepositoryBase<d_b_tables> _rep = new RepositoryBase<d_b_tables>();
            IRepositoryBase<Modules> _repMod = new RepositoryBase<Modules>();

            d_b_tables dbTables = _rep.Get(x => x.id == tableID);
            dbTables.Modules = new List<Modules>();
            foreach (var item in dbTables.ModulesTables)
            {
                Modules mod = _repMod.Get(p => p.ID == item.moduleID);

                if (!dbTables.Modules.Contains(mod))
                    dbTables.Modules.Add(mod);
            }
            return dbTables;
        }

        public List<d_b_column_explanations> GetDetailsByColumnID(int columnID)
        {
            IRepositoryBase<d_b_column_explanations> _rep = new RepositoryBase<d_b_column_explanations>();

            List<d_b_column_explanations> response = _rep.GetList(p => p.db_column_id == columnID).OrderBy(p => p.details).ToList();

            return response;
        }

        public List<DataSchema> GetSchemaNames(bool getTableCount = true)
        {
            IRepositoryBase<d_b_schemas> _rep = new RepositoryBase<d_b_schemas>();

            List<d_b_schemas> response = new List<d_b_schemas>();

            //if (getTables)
            //    response = _rep.AllIncluding<d_b_schemas>(p => p.d_b_tables).ToList();
            //else
            response = _rep.All<d_b_schemas>().OrderBy(p => p.name).ToList();
            List<DataSchema> semas = new List<DataSchema>();

            foreach (var x in response)
            {
                DataSchema dschema = new DataSchema();
                if (getTableCount)
                    dschema.value = (x.name + " (Table Count :" + x.d_b_tables.Count().ToString() + ")");
                else
                    dschema.value = x.name;

                dschema.id = x.id;
                semas.Add(dschema);
            }

            return semas;
        }

        public d_b_schemas GetSchema(int schemaID)
        {
            IRepositoryBase<d_b_schemas> _rep = new RepositoryBase<d_b_schemas>();

            d_b_schemas response = new d_b_schemas();

            response = _rep.Get(p => p.mid == schemaID);

            return response;
        }

        public List<LookUpModel> GetLookupModels(int columnIDfrom)
        {
            List<LookUpModel> response = new List<LookUpModel>();

            RepositoryBase<d_b_column_lookups> _rep = new RepositoryBase<d_b_column_lookups>();

            List<d_b_column_lookups> lookUps = _rep.GetList(p => p.column_from_id == columnIDfrom).ToList();

            RepositoryBase<d_b_columns> _repColumn = new RepositoryBase<d_b_columns>();

            LookUpModel upModel;
            d_b_columns dbColumn;

            foreach (var item in lookUps)
            {
                upModel = new LookUpModel();

                dbColumn = _repColumn.Get(p => p.id == item.column_from_id);// from db column 
                upModel.ColumnNameFrom = dbColumn.name;
                upModel.TableNameFrom = dbColumn.d_b_tables.name;
                upModel.SchemaNameFrom = dbColumn.d_b_tables.d_b_schemas.name;

                dbColumn = _repColumn.Get(p => p.id == item.column_to_id);
                upModel.ColumnNameTo = dbColumn.name;
                upModel.TableNameTo = dbColumn.d_b_tables.name;
                upModel.SchemaNameTo = dbColumn.d_b_tables.d_b_schemas.name;

                response.Add(upModel);

                dbColumn = null;
            }

            return response;
        }

        public List<d_b_table_explanation> GetTableExplanationsByTableID(int tableID)
        {
            IRepositoryBase<d_b_table_explanation> _rep = new RepositoryBase<d_b_table_explanation>();

            List<d_b_table_explanation> response = _rep.GetList(p => p.db_table_id == tableID).OrderBy(p => p.details).ToList();

            return response;
        }

        public List<app_base_modules> GetAllModules()
        {
            IRepositoryBase<app_base_modules> _rep = new RepositoryBase<app_base_modules>();

            List<app_base_modules> response = _rep.GetList(p => true);

            return response;
        }


        public List<app_module_operations> GetAllOperationsForModule(int subModuleID)
        {
            IRepositoryBase<app_sub_modules> _rep = new RepositoryBase<app_sub_modules>();

            app_sub_modules subModule = _rep.Get(p => p.id == subModuleID);

            return subModule.app_module_operations.ToList<app_module_operations>();

        }

        #endregion


        #region inserts
        public int NewTable(d_b_tables tbl)
        {
            IRepositoryBase<d_b_tables> _rep = new RepositoryBase<d_b_tables>();

            tbl = _rep.Add(tbl);

            tbl.mid = tbl.id;

            tbl = _rep.Update(tbl);

            return tbl.id;
        }

        public int NewColumn(d_b_columns clm)
        {
            IRepositoryBase<d_b_columns> _rep = new RepositoryBase<d_b_columns>();

            //get data type from db with sent type name , if it exists then get ID , if is not create one.

            IRepositoryBase<d_b_column_types> _repType = new RepositoryBase<d_b_column_types>();

            d_b_column_types columnType = _repType.Get(p => p.name == clm.DataType);

            if (columnType != null)
                clm.db_column_type_id = columnType.id;
            else
            {
                columnType = new d_b_column_types();
                columnType.id = 0;
                columnType.mid = 0;
                columnType.name = clm.DataType;
                columnType.created_at = DateTime.Now;
                columnType.updated_at = DateTime.Now;

                columnType = _repType.Add(columnType);

                clm.db_column_type_id = columnType.id;
            }


            clm = _rep.Add(clm);

            clm.mid = clm.id;

            clm = _rep.Update(clm);

            return clm.id;
        }

        public int NewSchema(d_b_schemas sch)
        {
            IRepositoryBase<d_b_schemas> _rep = new RepositoryBase<d_b_schemas>();

            sch = _rep.Add(sch);

            sch.mid = sch.id;

            sch = _rep.Update(sch);

            return sch.id;
        }

        public void AddColumnExplanation(int columnID, string Exp, int userID)
        {
            IRepositoryBase<d_b_column_explanations> _rep = new RepositoryBase<d_b_column_explanations>();
            IRepositoryBase<d_b_schemas> _repSchema = new RepositoryBase<d_b_schemas>();
            IRepositoryBase<d_b_tables> _repTable = new RepositoryBase<d_b_tables>();
            IRepositoryBase<d_b_columns> _repColumn = new RepositoryBase<d_b_columns>();

            string[] str = Exp.Split('$');

            if (str.Count() > 3)
            {
                IRepositoryBase<d_b_column_lookups> _replookup = new RepositoryBase<d_b_column_lookups>();

                d_b_schemas schema;
                d_b_tables table;
                d_b_columns column;

                string schemaName;
                string tableName;
                string columnName;

                schemaName = str[1];

                if (schemaName != string.Empty)
                    schema = _repSchema.Get(p => p.name == schemaName);
                else
                {
                    column = _repColumn.Get(p => p.id == columnID);
                    schema = column.d_b_tables.d_b_schemas;
                }

                tableName = str[2];
                table = _repTable.Get(p => p.name == tableName && p.d_b_schemasId == schema.id);

                columnName = str[3];
                int tableID = table.id;
                column = _repColumn.Get(p => p.name == columnName && p.d_b_tablesId == tableID);

                d_b_column_lookups lookup = new d_b_column_lookups()
                {
                    column_from_id = columnID,
                    column_to_id = column.id,
                    created_at = DateTime.Now,
                    explanation = "",
                    isempty = false,
                    mid = 0,
                    updated_at = DateTime.Now,

                };

                _replookup.Add(lookup);

            }
            else if (Exp == "!unused" || Exp == "!notused")
            {
                d_b_columns column = _repColumn.Get(p => p.id == columnID);

                column.isempty = true;

                column = _repColumn.Update(column);
            }
            else
            {
                d_b_column_explanations dbExp = new d_b_column_explanations()
                {
                    created_at = DateTime.Now,
                    db_column_id = columnID,
                    details = Exp,
                    isempty = false,
                    mid = 0,
                    updated_at = DateTime.Now,
                    userId = userID,
                };

                dbExp = _rep.Add(dbExp);

                dbExp.mid = dbExp.id;

                dbExp = _rep.Update(dbExp);
            }
        }
        public void AddLookup(int column_id_from, int column_id_to)
        {
            IRepositoryBase<d_b_column_lookups> _rep = new RepositoryBase<d_b_column_lookups>();

            d_b_column_lookups dbLookup = new d_b_column_lookups()
            {
                column_from_id = column_id_from,
                column_to_id = column_id_to,
                created_at = DateTime.Now,
                isempty = false,
                mid = 0,
                updated_at = DateTime.Now,
                explanation = string.Empty,
            };

            dbLookup = _rep.Add(dbLookup);

            dbLookup.mid = dbLookup.id;

            dbLookup = _rep.Update(dbLookup);

        }

        public void AddTableExplanation(int tableID, string Exp, int userID)
        {
            IRepositoryBase<d_b_table_explanation> _rep = new RepositoryBase<d_b_table_explanation>();

            IRepositoryBase<d_b_tables> _repTable = new RepositoryBase<d_b_tables>();
            if (Exp == "!unused" || Exp == "!notused")
            {
                d_b_tables table = _repTable.Get(p => p.id == tableID);

                table.isempty = true;

                table = _repTable.Update(table);

                return;
            }

            d_b_table_explanation dbExp = new d_b_table_explanation()
            {
                created_at = DateTime.Now,
                db_table_id = tableID,
                details = Exp,
                isempty = false,
                mid = 0,
                updated_at = DateTime.Now,
                userId = userID,
            };

            dbExp = _rep.Add(dbExp);

            dbExp.mid = dbExp.id;

            _rep.Update(dbExp);

        }

        public int NewBaseModule(app_base_modules module)
        {
            IRepositoryBase<app_base_modules> _rep = new RepositoryBase<app_base_modules>();

            module = _rep.Add(module);

            module.mid = module.id;

            module = _rep.Update(module);

            return module.id;
        }

        public int NewSubModule(app_sub_modules module)
        {
            IRepositoryBase<app_sub_modules> _rep = new RepositoryBase<app_sub_modules>();

            module = _rep.Add(module);

            module.mid = module.id;

            module = _rep.Update(module);

            return module.id;
        }

        public int NewModuleOperations(app_module_operations moduleOperations)
        {
            IRepositoryBase<app_module_operations> _rep = new RepositoryBase<app_module_operations>();

            moduleOperations = _rep.Add(moduleOperations);

            return moduleOperations.id;
        }

        public void ExcelImport(List<ColumnToExcel> listClExcel)
        {
            IRepositoryBase<d_b_schemas> _repSchema = new RepositoryBase<d_b_schemas>();
            IRepositoryBase<d_b_tables> _repTable = new RepositoryBase<d_b_tables>();
            IRepositoryBase<d_b_columns> _repColumn = new RepositoryBase<d_b_columns>();
            IRepositoryBase<d_b_column_explanations> _repExp = new RepositoryBase<d_b_column_explanations>();
            IRepositoryBase<d_b_column_lookups> _repLookUp = new RepositoryBase<d_b_column_lookups>();
            IRepositoryBase<d_b_column_types> _repTypes = new RepositoryBase<d_b_column_types>();

            string lookupSchema;
            string lookupTable;
            string lookupColumn;

            int i = 0;
            foreach (var item in listClExcel)
            {
                i++;
                if (item.Schema == string.Empty)
                    continue;

                d_b_schemas schema = _repSchema.Get(p => p.name == item.Schema);
                if (schema == null) continue;

                d_b_tables table = _repTable.Get(p => p.name == item.Table && p.d_b_schemasId == schema.id);
                if (table == null) continue;

                d_b_columns column = _repColumn.Get(p => p.name == item.Field && p.d_b_tablesId == table.id);
                if (column == null) continue;


                List<d_b_column_explanations> exps = _repExp.GetList(p => p.db_column_id == column.id).ToList();

                foreach (var exp in exps)
                {
                    _repExp.Remove(exp);
                }

                d_b_column_explanations exp1 = new d_b_column_explanations()
                {
                    created_at = DateTime.Now,
                    db_column_id = column.id,
                    details = item.Explanation1,
                    isempty = false,
                    mid = 0,
                    updated_at = DateTime.Now,
                    userId = item.UserID,
                };

                exp1 = _repExp.Add(exp1);

                string[] explanations = item.Explanation2.Split('&');

                foreach (string expla in explanations)
                {
                    d_b_column_explanations exp2 = new d_b_column_explanations()
                    {
                        created_at = DateTime.Now,
                        db_column_id = column.id,
                        details = expla,
                        isempty = false,
                        mid = 0,
                        updated_at = DateTime.Now,
                        userId = item.UserID,
                    };

                    exp2 = _repExp.Add(exp2);
                }

                //column lookup processes

                lookupSchema = string.Empty;
                lookupTable = string.Empty;
                lookupColumn = string.Empty;
                string[] lookups = item.LookUp.Split('.');

                if (lookups.Count() > 0 && lookups[0] != string.Empty) // gelen kayıtta lookup varsa eskilerini sil. yoksa elleşme.
                {
                    //delete exists lookup records.
                    List<d_b_column_lookups> columnLookUps = _repLookUp.GetList(p => p.column_from_id == column.id);

                    foreach (var lookup in columnLookUps)
                    {
                        _repLookUp.Remove(lookup);

                    }

                    int schemaID = schema.id;
                    if (lookups.Count() > 2)
                    {
                        lookupSchema = lookups[0];
                        d_b_schemas schemaForLookup = _repSchema.Get(p => p.name == lookupSchema);
                        if (schemaForLookup == null)
                            continue;
                        schemaID = schemaForLookup.id;

                        lookupTable = lookups[1];
                        lookupColumn = lookups[2];
                    }
                    else
                    {
                        lookupTable = lookups[0];
                        if (lookups.Count() > 1)
                            lookupColumn = lookups[1];
                    }

                    d_b_tables tableForLookUp = _repTable.Get(p => p.name == lookupTable && p.d_b_schemasId == schemaID);

                    if (tableForLookUp == null)
                        continue;

                    d_b_columns col = _repColumn.Get(p => p.name == lookupColumn && p.d_b_tablesId == tableForLookUp.id);

                    int columnIdTo = 0;
                    if (col != null)
                    {
                        columnIdTo = _repColumn.Get(p => p.name == lookupColumn && p.d_b_tablesId == tableForLookUp.id).id;

                        d_b_column_lookups newLookUp = new d_b_column_lookups()
                        {
                            column_from_id = column.id,
                            column_to_id = columnIdTo,
                            created_at = DateTime.Now,
                            explanation = string.Empty,
                            isempty = null,
                            mid = 0,
                            updated_at = DateTime.Now
                        };

                        _repLookUp.Add(newLookUp);
                    }
                }
            }
        }
        #endregion

        public void DeleteExp(int ExpID)
        {
            RepositoryBase<d_b_column_explanations> _rep = new RepositoryBase<d_b_column_explanations>();

            d_b_column_explanations exp = _rep.Get(p => p.id == ExpID);

            _rep.Remove(exp);

        }

        public void DeleteTableExp(int ExpID)
        {
            RepositoryBase<d_b_table_explanation> _rep = new RepositoryBase<d_b_table_explanation>();

            d_b_table_explanation exp = _rep.Get(p => p.id == ExpID);

            _rep.Remove(exp);
        }
        public void Dispose()
        {
            this.Dispose();
        }

        public string prepareSQLViewerData(List<int> tableIds, int type)
        {
            //type 1 ise seçilen tabloları göster.
            //tpye 2 ise seçilen tabloların tüm relationslarını dahil et.

            RepositoryBase<d_b_schemas> _schemarep = new RepositoryBase<d_b_schemas>();

            RepositoryBase<d_b_tables> _tablerep = new RepositoryBase<d_b_tables>();
            RepositoryBase<d_b_column_lookups> _lookuprep = new RepositoryBase<d_b_column_lookups>();
            RepositoryBase<d_b_columns> _colrep = new RepositoryBase<d_b_columns>();
            RepositoryBase<d_b_column_explanations> _repExp = new RepositoryBase<d_b_column_explanations>();

            List<d_b_tables> dbTablesToInclude = new List<d_b_tables>();
            foreach (int tableId in tableIds)
            {
                dbTablesToInclude.Add(_tablerep.Get(p => p.id == tableId));
            }

            List<int> fromLookUp = new List<int>();
            if (type == 2)
            {
                foreach (var itemTable in dbTablesToInclude)
                {
                    foreach (var itemColumn in itemTable.d_b_columns)
                    {
                        foreach (var itemLookup in itemColumn.d_b_column_lookups)
                        {
                            foreach (var itemID in GetLookUpTables(itemLookup.column_to_id))
                            {
                                if (!fromLookUp.Contains(itemID))
                                    fromLookUp.Add(itemID);
                            }
                        }
                    }
                }

                foreach (var item in fromLookUp)
                {
                    if (!tableIds.Contains(item))
                        dbTablesToInclude.Add(_tablerep.Get(p => p.id == item));
                }
            }


            //Console.WriteLine("Getting all tables");

            //d_b_schemas osofix = _schemarep.Get(p => p.id == dbTablesToInclude[0].d_b_schemasId);
            //dbTablesToInclude = osofix.d_b_tables.ToList();

            //Console.WriteLine("Got all tables");

            XmlDocument document = new XmlDocument();
            XmlElement sqlElement = (XmlElement)document.AppendChild(document.CreateElement("sql"));

            XmlElement dataTypesElement = (XmlElement)sqlElement.AppendChild(document.CreateElement("datatypes"));
            dataTypesElement.SetAttribute("db", "oracle");
            dataTypesElement.InnerXml = "<group label=\"Number\" color=\"rgb(238,238,170)\"> <type label=\"NUMERIC\" length=\"0\" sql=\"DECIMAL\" re=\"INT\" quote=\"\"/> <type label=\"NUMBER\" length=\"0\" sql=\"DECIMAL\" re=\"INT\" quote=\"\"/> <type label=\"Decimal\" length=\"1\" sql=\"DECIMAL\" re=\"DEC\" quote=\"\"/> <type label=\"Single precision\" length=\"0\" sql=\"FLOAT\" quote=\"\"/> <type label=\"Double precision\" length=\"0\" sql=\"DOUBLE\" re=\"DOUBLE\" quote=\"\"/> </group> <group label=\"Character\" color=\"rgb(226,226,226)\"> <type label=\"Char\" length=\"1\" sql=\"CHAR\" quote=\"\'\"/> <type label=\"NCHAR\" length=\"1\" sql=\"NCHAR\" quote=\"\'\"/> <type label=\"Varchar2\" length=\"1\" sql=\"VARCHAR2\" quote=\"\'\"/> <type label=\"CLOB\" length=\"0\" sql=\"MEDIUMTEXT\" re=\"TEXT\" quote=\"\'\"/> <type label=\"Binary\" length=\"1\" sql=\"BINARY\" quote=\"\'\"/> <type label=\"Varbinary\" length=\"1\" sql=\"VARBINARY\" quote=\"\'\"/> <type label=\"BLOB\" length=\"0\" sql=\"BLOB\" re=\"BLOB\" quote=\"\'\"/> </group> <group label=\"Date &amp; Time\" color=\"rgb(200,255,200)\"> <type label=\"Date\" length=\"0\" sql=\"DATE\" quote=\"\'\"/> <type label=\"Timestamp\" length=\"0\" sql=\"TIMESTAMP\" quote=\"\'\"/> </group> <group label=\"Miscellaneous\" color=\"rgb(200,200,255)\"> <type label=\"ENUM\" length=\"1\" sql=\"ENUM\" quote=\"\"/> <type label=\"SET\" length=\"1\" sql=\"SET\" quote=\"\"/> </group>";

            Console.WriteLine("Total tables count = " + dbTablesToInclude.Count.ToString());
            int currentTable = 0;
            foreach (d_b_tables table in dbTablesToInclude)
            {
                currentTable += 1;
                //Console.WriteLine("Progress = " + currentTable.ToString() + " / " + dbTablesToInclude.Count.ToString());

                XmlElement tableElement = (XmlElement)sqlElement.AppendChild(document.CreateElement("table"));
                //tableElement.SetAttribute("x", table.posX.toString());
                //tableElement.SetAttribute("y", table.posY.toString());
                tableElement.SetAttribute("name", (table.name + "(" + table.d_b_schemas.name + ")"));
                XmlElement dataTableCommentElement = (XmlElement)tableElement.AppendChild(document.CreateElement("comment"));

                //dataTableCommentElement.InnerText = table.d_b_schemas.name;
                foreach (var item in table.d_b_table_explanation)
                {
                    dataTableCommentElement.InnerText += " " + item.details;
                }

                foreach (d_b_columns col in table.d_b_columns)
                {
                    List<d_b_column_lookups> lookupres = _lookuprep.GetList(p => p.column_from_id == col.id);

                    XmlElement columnElement = (XmlElement)tableElement.AppendChild(document.CreateElement("row"));
                    columnElement.SetAttribute("name", col.name);
                    XmlElement dataTypeElement = (XmlElement)columnElement.AppendChild(document.CreateElement("datatype"));
                    dataTypeElement.InnerText = col.d_b_column_types.name;

                    //get comment for columns 
                    List<d_b_column_explanations> columnExps = _repExp.GetList(p => p.db_column_id == col.id);

                    XmlElement dataCommentElement = (XmlElement)columnElement.AppendChild(document.CreateElement("comment"));
                    foreach (var colExp in columnExps)
                    {
                        dataCommentElement.InnerText += colExp.details;
                    }

                    if (lookupres.Count < 1)
                    {
                        continue;
                    }

                    foreach (d_b_column_lookups lookup in lookupres)
                    {
                        d_b_columns targetColumn = _colrep.Get(p => p.id == lookup.column_to_id);
                        XmlElement relationElement = (XmlElement)columnElement.AppendChild(document.CreateElement("relation"));
                        relationElement.SetAttribute("table", targetColumn.d_b_tables.name + "(" + targetColumn.d_b_tables.d_b_schemas.name + ")");
                        relationElement.SetAttribute("row", targetColumn.name);
                    }
                }

            }

            return document.OuterXml;
        }

        List<int> GetLookUpTables(int ColumnId) //columnId lookup tablosundan gelen "to column id" 
        {
            List<int> tableIDs = new List<int>();
            RepositoryBase<d_b_columns> _colrep = new RepositoryBase<d_b_columns>();

            //id si gönderilen kolonun tablosunu çek. 
            d_b_columns col = _colrep.Get(p => p.id == ColumnId);

            //liste ekle
            tableIDs.Add(col.d_b_tablesId);

            //bu kolonun ilgili olduğu lookupları kontrol et.
            foreach (var item in col.d_b_column_lookups)
            {
                col = _colrep.Get(p => p.id == item.column_to_id);

                if (!tableIDs.Contains(col.d_b_tablesId))
                    tableIDs.Add(col.d_b_tablesId);
            }

            //kolonun tablosunun tüm kolonlarının lookuplarını kontrol et.

            foreach (var itemColumn in col.d_b_tables.d_b_columns)
            {
                foreach (var itemLookup in itemColumn.d_b_column_lookups)
                {
                    col = _colrep.Get(p => p.id == itemLookup.column_to_id);
                    if (!tableIDs.Contains(col.d_b_tablesId))
                        tableIDs.Add(col.d_b_tablesId);
                }
            }

            return tableIDs;

        }
        public void deleteErDiagrams(int ID)
        {
            RepositoryBase<ERxml> _rep = new RepositoryBase<ERxml>();

            ERxml erXml = _rep.Get(p => p.id == ID);
            if (erXml != null)
                _rep.Remove(erXml);

        }

        public void ExplanationFix()
        {
            RepositoryBase<d_b_column_explanations> _rep = new RepositoryBase<d_b_column_explanations>();
            RepositoryBase<d_b_tables> _repTable = new RepositoryBase<d_b_tables>();
            RepositoryBase<d_b_schemas> _repSchema = new RepositoryBase<d_b_schemas>();
            RepositoryBase<d_b_columns> _repColumn = new RepositoryBase<d_b_columns>();
            RepositoryBase<d_b_column_lookups> _replookup = new RepositoryBase<d_b_column_lookups>();

            List<d_b_column_explanations> exps = _rep.GetList(p => p.details.Contains("$"));

            string schemaName;
            string tablename;
            string columndName;

            foreach (var item in exps)
            {
                d_b_schemas schema;
                d_b_tables table;
                d_b_columns columnd_to;

                schemaName = string.Empty;
                tablename = string.Empty;
                columndName = string.Empty;


                string[] str = item.details.Split('$');

                if (str.Count() > 2)
                {
                    schemaName = str[0];
                    schema = _repSchema.Get(p => p.name == schemaName);

                    tablename = str[1];
                    columndName = str[2];
                }
                else
                {
                    schema = item.d_b_columns.d_b_tables.d_b_schemas;
                    tablename = str[0];
                    columndName = str[1];
                }


                table = _repTable.Get(p => p.name == tablename && p.d_b_schemasId == schema.id);
                int tableID = table.id;
                columnd_to = _repColumn.Get(p => p.name == columndName && p.d_b_tablesId == tableID);

                if (columnd_to == null)
                    continue;

                d_b_column_lookups lookup = new d_b_column_lookups()
                {
                    column_from_id = item.d_b_columns.id,
                    column_to_id = columnd_to.id,
                    created_at = item.created_at,
                    explanation = "",
                    isempty = null,
                    mid = 0,
                    updated_at = item.updated_at,

                };

                _replookup.Add(lookup);

                _rep.Remove(item);
            }
        }

        public List<SearchResult> ColumnExplanationSearch(string searchParam)
        {
            List<SearchResult> searchResults = new List<SearchResult>();

            if (searchParam == null || searchParam.Length < 4)
                return searchResults;

            RepositoryBase<d_b_column_explanations> _rep = new RepositoryBase<d_b_column_explanations>();

            List<d_b_column_explanations> exps = _rep.GetList(p => p.details.Contains(searchParam)).Take(10).ToList();

            foreach (d_b_column_explanations exp in exps)
            {
                searchResults.Add(new SearchResult(exp.d_b_columns.d_b_tables.d_b_schemas.name, exp.d_b_columns.d_b_tables.d_b_schemas.id, exp.d_b_columns.d_b_tables.name, exp.d_b_columns.d_b_tables.id, exp.d_b_columns.name, exp.d_b_columns.id));
            }


            return searchResults;

        }

        public void saveXML(string XMLData, string ERtables, string diagramName, int diagramID)
        {
            RepositoryBase<ERxml> _rep = new RepositoryBase<ERxml>();
            RepositoryBase<d_b_tables> _repTable = new RepositoryBase<d_b_tables>();

            if (diagramID < 0) //update yapılacak.
            {
                ERxml erXML = _rep.Get(p => p.id == (diagramID * -1));

                erXML.xmlData = XMLData;

                erXML = _rep.Update(erXML);
            }
            else // insert edilecek.
            {
                string[] tblIds = ERtables.Split(',');

                int tblId = int.Parse(tblIds[1]);

                d_b_tables t = _repTable.Get(p => p.id == tblId);
                ERxml erXML = new ERxml()
                {
                    schemaid = t.d_b_schemasId,
                    tablesid = ERtables,
                    xmlData = XMLData,
                    ErName = diagramName,
                };

                _rep.Add(erXML);
            }
        }

        public ERxml GetErxmlByID(int erID)
        {
            RepositoryBase<ERxml> _repXml = new RepositoryBase<ERxml>();

            return _repXml.Get(p => p.id == erID);
        }

        public List<TasimaSekilleri> GetTasimaSekilleri()
        {
            RepositoryBase<TasimaSekilleri> _rep = new RepositoryBase<TasimaSekilleri>();

            return _rep.GetList();
        }

        public List<BeyanTurleri> GetBeyanTurleri()
        {
            RepositoryBase<BeyanTurleri> _rep = new RepositoryBase<BeyanTurleri>();

            return _rep.GetList().OrderBy(p => p.BeyanTurKodu).ToList();
        }

        public List<GumrukIdareleri> GetGumrukIdareleri()
        {
            RepositoryBase<GumrukIdareleri> _rep = new RepositoryBase<GumrukIdareleri>();

            return _rep.GetList();
        }

        public List<Ulkeler> GetUlkeler()
        {
            RepositoryBase<Ulkeler> _rep = new RepositoryBase<Ulkeler>();

            return _rep.GetList();
        }

        public List<Limanlar> GetLimanlarByUlkeId(int ulkeID)
        {
            RepositoryBase<Limanlar> _rep = new RepositoryBase<Limanlar>();

            return _rep.GetList(p => p.UlkeID == ulkeID);
        }

        public OzelTuzelSahis GetOzelTuzelSahisByVergiNo(string vergiNo)
        {
            RepositoryBase<OzelTuzelSahis> _rep = new RepositoryBase<OzelTuzelSahis>();

            return _rep.Get(p => p.KimlikNo == vergiNo);
        }

        public List<KimlikTurleri> GetKimlikTurleri()
        {
            RepositoryBase<KimlikTurleri> _rep = new RepositoryBase<KimlikTurleri>();

            return _rep.GetList();
        }

        public OzelTuzelSahis SahisKaydet(OzelTuzelSahis sahis)
        {
            RepositoryBase<OzelTuzelSahis> _rep = new RepositoryBase<OzelTuzelSahis>();

            if (sahis.ID == 0)
                return _rep.Add(sahis);
            else
                return _rep.Update(sahis);
        }

        public OzetBeyan OzetBeyanKaydet(OzetBeyan oztBeyan)
        {
            RepositoryBase<OzetBeyan> _rep = new RepositoryBase<OzetBeyan>();

            return _rep.Add(oztBeyan);
        }

        public TasitBilgileri SearchTasit(string searchValue)
        {
            RepositoryBase<TasitBilgileri> _rep = new RepositoryBase<TasitBilgileri>();

            return _rep.Get(p => p.Ad.Contains(searchValue) || p.Numarasi == searchValue);
        }

        public TasitBilgileri TasitKaydet(TasitBilgileri tst)
        {
            RepositoryBase<TasitBilgileri> _rep = new RepositoryBase<TasitBilgileri>();

            tst = _rep.Add(tst);

            return tst;
        }

        public List<d_b_tables> GetTablesBySubModuleID(int subModuleID)
        {
            RepositoryBase<app_module_operations> _rep = new RepositoryBase<app_module_operations>();

            List<app_module_operations> appModuleOps = _rep.GetList(p => p.app_sub_module_id == subModuleID);

            List<d_b_tables> returnList = new List<d_b_tables>();

            foreach (var item in appModuleOps)
            {
                returnList.Add(item.d_b_tables);
            }

            return returnList;
        }

        public TasitBilgileri GetTasitBilgileriByID(int ID)
        {
            RepositoryBase<TasitBilgileri> _rep = new RepositoryBase<TasitBilgileri>();
            return _rep.Get(p => p.ID == ID);
        }

        public OzelTuzelSahis GetTasiyiciFirma(int ID)
        {
            RepositoryBase<OzelTuzelSahis> _rep = new RepositoryBase<OzelTuzelSahis>();

            return _rep.Get(p => p.ID == ID);
        }

        public DBErrorMessages SaveErrorMessages(DBErrorMessages message)
        {
            RepositoryBase<DBErrorMessages> _rep = new RepositoryBase<DBErrorMessages>();

            return _rep.Add(message);
        }

        public SQLProcedures SaveSQlProcedures(SQLProcedures procedure, string dirName)
        {
            RepositoryBase<SQLProcedures> _rep = new RepositoryBase<SQLProcedures>(false);
            RepositoryBase<d_b_schemas> _repSchema = new RepositoryBase<d_b_schemas>();

            d_b_schemas schema = _repSchema.Get(p => p.name == dirName);

            if (schema != null)
            {
                SQLProcedures prc = new SQLProcedures();
                if (procedure.ID != 0)
                {
                    prc = _rep.Get(p => p.ID == procedure.ID);
                    prc.formattedBody = procedure.formattedBody;
                    prc.schema_id = schema.id;
                    prc.isFormatted = true;
                    prc.sqlText = procedure.sqlText;

                    if (procedure.sqlText == null)
                        prc.sqlText = procedure.formattedBody;

                    prc.mid = 0;

                    _rep.Update(prc);

                    return formatter(prc);
                }
                else
                {
                    procedure.schema_id = schema.id;
                    return _rep.Add(procedure);
                }
            }
            else
            {
                if (procedure.ID != 0)
                {
                    procedure.IfCount = procedure.IfCount;

                    return _rep.Update(procedure);
                }

            }

            return null;
        }

        public List<SQLProcedures> GetAllProcedures(string letter, int schemaID)
        {
            RepositoryBase<SQLProcedures> _rep = new RepositoryBase<SQLProcedures>();

            if (letter != "all")
                return _rep.GetList().Where(p => (p.name.StartsWith(letter.ToLower()) || p.name.StartsWith(letter.ToUpper())) && p.schema_id == schemaID).OrderBy(p => p.name).ToList();
            else
                return _rep.GetList().Where(p => p.schema_id == schemaID).OrderBy(p => p.name).ToList();
        }

        SQLProcedures formatter(SQLProcedures procedure)
        {
            RepositoryBase<SQLProcedures> _rep = new RepositoryBase<SQLProcedures>();

            RepositoryBase<SQLKeys> _repSQlKey = new RepositoryBase<SQLKeys>();
            RepositoryBase<DBErrorMessages> _repERR = new RepositoryBase<DBErrorMessages>();
            RepositoryBase<d_b_tables> _reptable = new RepositoryBase<d_b_tables>();
            RepositoryBase<d_b_columns> _repColumn = new RepositoryBase<d_b_columns>();
            List<SQLKeys> keyLists = _repSQlKey.GetList();

            List<string> colorlist = new List<string>();

            List<DBErrorMessages> listError = new List<DBErrorMessages>();
            listError = _repERR.GetList();
            if (procedure.formattedBody != null)
                procedure.formattedBody = procedure.formattedBody.ToUpperInvariant();
            else
                procedure.formattedBody = procedure.body;

            //hata kodları renklendiriliyor. 
            foreach (var item in listError)
            {
                if (procedure.formattedBody.Contains(item.Code))
                {
                    procedure.formattedBody = procedure.formattedBody.Replace("'" + item.Code + "'", "<strong><font color='#ff6600'>'" + item.Code + "'</font></strong>");
                }
            }

            foreach (var item in keyLists)
            {
                procedure.formattedBody = procedure.formattedBody.Replace(item.SqlKey, "<font color='#" + item.Color + "'>" + item.SqlKey + "</font>");
                //procedure.formattedBody = procedure.formattedBody.Replace(item.SqlKey.ToLowerInvariant(), "<font color='#" + item.Color + "'>" + item.SqlKey + "</font>");
                if (colorlist != null && !colorlist.Contains(item.Color))
                    colorlist.Add(item.Color);
            }

            procedure.formattedBody = procedure.formattedBody.Replace("COLLAPSE", "collapse");

            string[] lines = procedure.formattedBody.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            int startIndex = 0;
            int lastIndex = 0;
            string oldline = "";
            string newline = "";
            string str = "";
            string str2 = "";

            foreach (var item in lines)
            {
                if (item.Contains("--"))
                {
                    oldline = item;
                    startIndex = item.IndexOf("--");

                    str = item.Substring(startIndex);
                    str2 = str;
                    foreach (var color in colorlist)
                    {
                        str2 = str2.Replace(color, "00b200");
                    }

                    oldline = oldline.Replace(str, str2);

                    newline = oldline.Insert(startIndex, "<font color='#00b200'>");

                    lastIndex = newline.Length - 1;

                    newline = newline.Insert(lastIndex + 1, "</font>");

                    procedure.formattedBody = procedure.formattedBody.Replace(item, newline);
                }
            }

            // /* */ multiline comment alanları renklendiriliyor.
            procedure.formattedBody = procedure.formattedBody.Replace("/*", "<span class='green'>/*");
            procedure.formattedBody = procedure.formattedBody.Replace("*/", "*/</span>");

            return procedure;
        }

        string ColorfulString(string plainText, string startWord, string finishWord)
        {
            string colorFullText = plainText;

            int startIndex = plainText.IndexOf(startWord);
            if (startIndex == -1)
                return plainText;

            string restWord = plainText.Substring(0, startIndex + 2);

            startIndex = restWord.IndexOf(finishWord);

            string afterWord = restWord.Substring(startIndex);

            return colorFullText;
        }
        /// <summary>
        /// Arama yapılacak text verinin eşleştiği stored procedure,table,schema,column,error messageları döner.
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public ProceduresSearchResult SearchProcedures(string searchText, bool fromLayout)
        {
            ProceduresSearchResult result = new ProceduresSearchResult();
            IRepositoryBase<d_b_schemas> _repSchema = new RepositoryBase<d_b_schemas>();
            IRepositoryBase<d_b_tables> _repTable = new RepositoryBase<d_b_tables>();
            IRepositoryBase<d_b_columns> _repColumns = new RepositoryBase<d_b_columns>();
            IRepositoryBase<SQLProcedures> _repProcedures = new RepositoryBase<SQLProcedures>();
            IRepositoryBase<DBErrorMessages> _repDBErrors = new RepositoryBase<DBErrorMessages>();

            string[] _str = null;

            if (searchText.Contains("."))
                _str = searchText.Split('.');
            int tableid = 0;

            result._schemas = _repSchema.GetList(p => p.name.Contains(searchText));
            if (_str != null && _str.Count() > 0)
            {
                string table = _str[0].Trim();
                result._tables = _repTable.GetList(p => p.name == table);
                if (result._tables != null && result._tables.Count > 0)
                    tableid = result._tables[0].id;
            }
            else
                result._tables = _repTable.GetList(p => p.name == searchText);


            if (_str != null && _str.Count() > 1)
            {
                string column = _str[1].Trim();
                result._columns = _repColumns.GetList(p => p.name == column && p.d_b_tablesId == tableid);
            }
            else
            {
                result._columns = _repColumns.GetList(p => p.name == searchText.Trim());
            }

            string procedureName = "";
            string schemaAdi = "";
            if (_str != null && _str.Count() > 1)
            {
                schemaAdi = _str[0].Trim();
                procedureName = _str[1].Trim();
                d_b_schemas schema = _repSchema.Get(p => p.name == schemaAdi);
                if (schema != null)
                    result._procedures = _repProcedures.GetList(p => (p.name == (procedureName + ".prc") || p.name == (procedureName + ".fnc")) && p.schema_id == schema.id);

                if (result._procedures != null && result._procedures.Count > 0)
                    result._procedures[0].schemas = schema;

                if (result._procedures != null && result._procedures.Count > 0 && result._procedures[0].formattedBody == null)
                    result._procedures[0].formattedBody = result._procedures[0].body;

                if (result._procedures != null && result._procedures.Count > 0 && result._procedures[0] != null)
                    result._procedures[0] = formatter(result._procedures[0]);
            }
            else
            {
                procedureName = searchText;
                if (!fromLayout)
                    result._procedures = _repProcedures.GetList(p => p.name == (procedureName + ".prc") || p.name == (procedureName + ".fnc"));
                else
                    result._procedures = _repProcedures.GetList(p => p.name.Contains(procedureName));

                foreach (var item in result._procedures)
                {
                    item.schemas = _repSchema.Get(p => p.id == item.schema_id);
                }

                if (result._procedures != null && result._procedures.Count > 0 && result._procedures[0].formattedBody == null)
                    result._procedures[0].formattedBody = result._procedures[0].body;

                if (result._procedures != null && result._procedures.Count > 0 && result._procedures[0] != null)
                    result._procedures[0] = formatter(result._procedures[0]);
            }

            result._errorMessages = _repDBErrors.GetList(p => p.Code == searchText);

            return result;

        }

        public List<SQLProceduresComments> GetSQLProcedureComments(int SQLProcedureID)
        {
            RepositoryBase<SQLProceduresComments> _rep = new RepositoryBase<SQLProceduresComments>();
            RepositoryBase<users> _repUser = new RepositoryBase<users>();

            List<SQLProceduresComments> result = _rep.GetList(p => p.SQLProceduresID == SQLProcedureID);

            foreach (var item in result)
            {
                item.user = _repUser.Get(p => p.id == item.userID);
            }
            return _rep.GetList(p => p.SQLProceduresID == SQLProcedureID);
        }

        public SQLProceduresComments AddSQLProcedureComment(SQLProceduresComments comment, int? position, string sqlText)
        {
            RepositoryBase<SQLProceduresComments> _rep = new RepositoryBase<SQLProceduresComments>();
            IRepositoryBase<SQLProcedures> _repProcedure = new RepositoryBase<SQLProcedures>();
            IRepositoryBase<users> _repUser = new RepositoryBase<users>();

            SQLProcedures pros = _repProcedure.Get(p => p.ID == comment.SQLProceduresID);

            if (comment.position == null)
                comment.position = pros.sqlText.Length;

            SQLProceduresComments sqlComment = _rep.Add(comment);

            List<SQLProceduresComments> listComm = _rep.GetList(p => p.position > sqlComment.position && p.SQLProceduresID == comment.SQLProceduresID);

            foreach (var item in listComm)
            {
                item.user = _repUser.Get(p => p.id == item.userID);
                item.position += item.user.name.Length + item.Details.Length + item.created_at.ToString().Length + 1;
                _rep.Update(item);
            }


            //pros.sqlText = sqlText;
            //_repProcedure.Update(pros);

            return sqlComment;
        }

        public void deleteSQlProcedureComment(int ID)
        {
            IRepositoryBase<SQLProceduresComments> _rep = new RepositoryBase<SQLProceduresComments>();
            IRepositoryBase<users> _repUser = new RepositoryBase<users>();
            SQLProceduresComments com = _rep.Get(p => p.ID == ID);

            List<SQLProceduresComments> listComm = _rep.GetList(p => p.position > com.position && p.SQLProceduresID == com.SQLProceduresID);

            foreach (var item in listComm)
            {
                item.user = _repUser.Get(p => p.id == item.userID);
                item.position -= item.user.name.Length + item.Details.Length + item.created_at.ToString().Length + 1;
                _rep.Update(item);
            }

            _rep.Remove(com);
        }

        public SQLProcedures UpdateSQLText(SQLProcedures procedure)
        {
            RepositoryBase<SQLProcedures> _rep = new RepositoryBase<SQLProcedures>();

            SQLProcedures prc = new SQLProcedures();
            if (procedure.ID != 0)
            {
                prc = _rep.Get(p => p.ID == procedure.ID);
                prc.sqlText = procedure.sqlText;
                return _rep.Update(prc);
            }

            return null;

        }


        public List<SQLProcedures> GetAllSP(bool getSchemas = false)
        {
            RepositoryBase<SQLProcedures> _rep = new RepositoryBase<SQLProcedures>();
            RepositoryBase<d_b_schemas> _repSchema = new RepositoryBase<d_b_schemas>();

            List<SQLProcedures> result = _rep.GetList();
            if (getSchemas)
                foreach (var item in result)
                {
                    item.schemas = _repSchema.Get(p => p.id == item.schema_id);
                }
            return result;
        }

        public SQLProcedures GetProcedure(int ID)
        {
            RepositoryBase<SQLProcedures> _rep = new RepositoryBase<SQLProcedures>();
            SQLProcedures pros = _rep.Get(p => p.ID == ID);

            return formatter(pros);
        }

        public List<SQLProcedureModules> GetSQLProcedureModules(int ID)
        {
            List<app_base_modules> returnvalue = new List<app_base_modules>();

            RepositoryBase<SQLProcedureModules> _repProcMod = new RepositoryBase<SQLProcedureModules>();
            
            List<SQLProcedureModules> listMod = _repProcMod.GetList(p => p.SQLProcedureID == ID);

            
            return listMod;
        }

        public List<DBErrorMessages> GetAllErrorMessages()
        {
            IRepositoryBase<DBErrorMessages> _rep = new RepositoryBase<DBErrorMessages>();

            return _rep.GetList();
        }

        /// <summary>
        /// ID'si girilen procedure'nin bodysini fotmatlanıdırıp dönderir.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public SQLProcedures GetSQLProcedure(int ID)
        {
            RepositoryBase<SQLProcedures> _rep = new RepositoryBase<SQLProcedures>(false);
            RepositoryBase<SQLProceduresComments> _repCommentID = new RepositoryBase<SQLProceduresComments>();
            RepositoryBase<SQLProcedureModules> _repProModul = new RepositoryBase<SQLProcedureModules>();
            RepositoryBase<app_base_modules> _repModul = new RepositoryBase<app_base_modules>();

            SQLProcedures procedure = _rep.Get(p => p.ID == ID);

            //get all comments 
            List<SQLProceduresComments> allComments = _repCommentID.GetList(p => p.SQLProceduresID == ID).OrderBy(p => p.position).ToList();
            string TXT = procedure.formattedBody;

            if (procedure.formattedBody == null)
                procedure.sqlText = procedure.body;

            if (TXT != "" && allComments != null && allComments.Count > 0)
            {
                RepositoryBase<users> _repUser = new RepositoryBase<users>();
                int toplamLenght = 0;

                foreach (var item in allComments)
                {
                    item.position += toplamLenght;
                    item.user = _repUser.Get(p => p.id == item.userID);
                    TXT = TXT.Substring(0, (int)item.position) + "<button class='btn btn-default btn-xs' data-toggle='collapse' data-target='#comment_" + item.ID + "'>" + item.user.name + " " + item.created_at.ToString() + "</button><div class='collapse'  id='comment_" + item.ID + "'><span style='color:#ff6600;font-weight:bold;'>" + item.Details + "</span></div>" + TXT.Substring((int)item.position);
                    toplamLenght += ("<button class='btn btn-default btn-xs' data-toggle='collapse' data-target='#comment_" + item.ID + "'></button><div class='collapse' id='comment_" + item.ID + "'></div>").Length;
                }
            }

            procedure.formattedBody = TXT;

            procedure = formatter(procedure);

            return procedure;
        }

        public Modules SaveModule(Modules module)
        {
            RepositoryBase<Modules> _rep = new RepositoryBase<Modules>();

            return _rep.Add(module);
        }

        public SubModules SaveSubModule(SubModules subModule)
        {
            RepositoryBase<SubModules> _rep = new RepositoryBase<SubModules>();

            return _rep.Add(subModule);
        }

        public ModulesFiles SaveModulesFiles(ModulesFiles moduleFile)
        {
            RepositoryBase<ModulesFiles> _rep = new RepositoryBase<ModulesFiles>();

            return _rep.Add(moduleFile);
        }

        public List<Modules> GetModules()
        {
            RepositoryBase<Modules> _rep = new RepositoryBase<Modules>();

            return _rep.GetList();
        }

        public List<ModulesFiles> GetSubModulesFiles(int subModuleID)
        {
            RepositoryBase<ModulesFiles> _rep = new RepositoryBase<ModulesFiles>();

            List<ModulesFiles> files = _rep.GetList(p => p.SubModuleID == subModuleID);

            return files;
        }

        public List<ModulesFiles> GetModulesFiles(int ModuleID)
        {
            RepositoryBase<ModulesFiles> _rep = new RepositoryBase<ModulesFiles>();

            List<ModulesFiles> files = _rep.GetList(p => p.ModuleID == ModuleID);

            return files;
        }

        public ModulesFiles GetModuleFile(int modFileID)
        {
            RepositoryBase<ModulesFiles> _rep = new RepositoryBase<ModulesFiles>();

            return _rep.Get(p => p.ID == modFileID, false);
        }

        public ModulesFiles UpdateModulesFiles(ModulesFiles moduleFile)
        {
            RepositoryBase<ModulesFiles> _rep = new RepositoryBase<ModulesFiles>();

            return _rep.Update(moduleFile);
        }

        public SQLProcedureModules SaveSqlProcedureModules(SQLProcedureModules sqlMod)
        {
            RepositoryBase<SQLProcedureModules> _rep = new RepositoryBase<SQLProcedureModules>();

            return _rep.Add(sqlMod);
        }

        public List<SQLProcedureModules> GetSQLProcedureModulesBySubModuleID(int subModuleID)
        {
            RepositoryBase<SQLProcedureModules> _rep = new RepositoryBase<SQLProcedureModules>();
            RepositoryBase<d_b_schemas> _repSchema = new RepositoryBase<d_b_schemas>();
            List<SQLProcedureModules> result = _rep.GetList(p => p.SubModuleID == subModuleID);

            foreach (var item in result)
            {
                item.SQLProcedures.schemas = _repSchema.Get(p => p.id == item.SQLProcedures.schema_id);
            }
            return result;
        }

        public List<SQLProcedureModules> GetSQLProcedureModulesByFileID(int FileID)
        {
            RepositoryBase<SQLProcedureModules> _rep = new RepositoryBase<SQLProcedureModules>();
            RepositoryBase<d_b_schemas> _repSchema = new RepositoryBase<d_b_schemas>();
            List<SQLProcedureModules> result = _rep.GetList(p => p.FileID == FileID);

            foreach (var item in result)
            {
                item.SQLProcedures.schemas = _repSchema.Get(p => p.id == item.SQLProcedures.schema_id);
            }
            return result;
        }

        public List<SQLProcedureModules> GetSQLProcedureModulesByModuleID(int ModuleID)
        {
            RepositoryBase<SQLProcedureModules> _rep = new RepositoryBase<SQLProcedureModules>();
            RepositoryBase<d_b_schemas> _repSchema = new RepositoryBase<d_b_schemas>();
            List<SQLProcedureModules> result = _rep.GetList(p => p.ModuleID == ModuleID);

            foreach (var item in result)
            {
                item.SQLProcedures.schemas = _repSchema.Get(p => p.id == item.SQLProcedures.schema_id);
            }
            return result;
        }

        public List<d_b_tables> GetAllTables()
        {
            RepositoryBase<d_b_tables> _rep = new RepositoryBase<d_b_tables>();

            List<d_b_tables> tables = _rep.GetList();

            return tables;
        }

        public ModulesTables SaveModuleTable(ModulesTables moduleTable)
        {
            RepositoryBase<ModulesTables> _rep = new RepositoryBase<ModulesTables>();

            return _rep.Add(moduleTable);
        }

        public List<ModulesTables> GetModulesTablesBySubModuleID(int subModuleID)
        {
            RepositoryBase<ModulesTables> _rep = new RepositoryBase<ModulesTables>();

            List<ModulesTables> result = _rep.GetList(p => p.subModuleID == subModuleID);

            return result;
        }

        public List<ModulesTables> GetModulesTablesByModuleID(int ModuleID)
        {
            RepositoryBase<ModulesTables> _rep = new RepositoryBase<ModulesTables>();

            List<ModulesTables> result = _rep.GetList(p => p.moduleID == ModuleID);

            return result;
        }

        public List<ModulesTables> GetModulesTablesByFileD(int FileID)
        {
            RepositoryBase<ModulesTables> _rep = new RepositoryBase<ModulesTables>();

            List<ModulesTables> result = _rep.GetList(p => p.fileID == FileID);

            return result;
        }
        public ModulesFiles GetModuleFileByID(int ID)
        {
            RepositoryBase<ModulesFiles> _rep = new RepositoryBase<ModulesFiles>(false);

            ModulesFiles file = _rep.Get(p => p.ID == ID);
            //file.Body = file.Body.Replace("<", "&#60;");
            //int lineCount = 0;
            //string[] lines = file.Body.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            //string newLine = "";
            //for (int i = 0; i < lines.Count(); i++)
            //{
            //    lineCount++;
            //    //lines[i] = lines[i].Replace("/*", "<span class='green'>/*");
            //    //lines[i] = lines[i].Replace("*/", "*/</span>");

            //    // line içerisinde "//" karakteri aranıyor. Bulunduğu yere color belirteci koyulacak.
            //    int commentIndex = lines[i].IndexOf("//");
            //    if (commentIndex > -1)
            //        lines[i] = lines[i].Substring(0, commentIndex) + "<span class='green'>" + lines[i].Substring(commentIndex) + "</span>";

            //    newLine += "<span style='color:#4bc6d1;'>" + lineCount.ToString() + "</span> " + lines[i] + "\n";
            //}

            //file.Body = newLine;


            return file;
        }

        public DBObjects AddDbOject(DBObjects dbObj)
        {
            RepositoryBase<DBObjects> _rep = new RepositoryBase<DBObjects>();
            RepositoryBase<d_b_schemas> _repSchema = new RepositoryBase<d_b_schemas>();
            RepositoryBase<d_b_tables> _repTable = new RepositoryBase<d_b_tables>();
            RepositoryBase<SQLProcedures> _repSqlProcedure = new RepositoryBase<SQLProcedures>();
            RepositoryBase<ObjType> _repobjType = new RepositoryBase<ObjType>();

            d_b_schemas schema = _repSchema.Get(p => p.name == dbObj.schemaName);

            //obje tipini al 
            ObjType objType = _repobjType.Get(p => p.objType1 == dbObj.strObjType);

            if (objType != null)
            {
                dbObj.objType = objType.ID;
            }
            else// yoksa type ekle
            {
                ObjType typ = new ObjType()
                {
                    objType1 = dbObj.strObjType,
                };

                typ = _repobjType.Add(typ);
                dbObj.objType = typ.ID;
            }

            if (schema != null)
            {
                dbObj.schemaID = schema.id;
            }

            //eklemeden önce var mı kontrol et 
            DBObjects obj = _rep.Get(p => p.objName == dbObj.objName && p.objType == dbObj.objType && p.schemaID == dbObj.schemaID);
            if (obj != null)
                return dbObj;
            else
                return _rep.Add(dbObj);
        }

        public void AddDbOject(List<DBObjects> dbObjList)
        {
            RepositoryBase<DBObjectsChilds> _rep = new RepositoryBase<DBObjectsChilds>();
            RepositoryBase<DBObjects> _repDBobj = new RepositoryBase<DBObjects>();
            int i = 0;
            foreach (var item in dbObjList)
            {
                DBObjects obj = AddDbOject(item);
                List<DBObjects> listChild = dbObjList.Where(p => p.parentID == item.ID).ToList();
                item.ID = obj.ID;

                foreach (var child in listChild)
                {
                    child.parentID = obj.ID;
                }
                i++;
            }

            //child kayıt işlemi yapılıyor.
            foreach (var item in dbObjList)
            {
                //database'den kaydı çek yeni ID'sini set et.
                DBObjects dbOBJ = _repDBobj.Get(p => p.objName == item.objName && p.schemaName == item.schemaName && p.objType == item.objType);
                if (dbOBJ != null)
                    item.ID = dbOBJ.ID;

                DBObjectsChilds child = new DBObjectsChilds()
                {
                    ChildID = item.ID,
                    ParentID = item.parentID,
                };

                _rep.Add(child);
            }

        }

        public DBObjects GetDBObject(string schemaName, string objName, string objType)
        {
            RepositoryBase<DBObjects> _rep = new RepositoryBase<DBObjects>();
            RepositoryBase<ObjType> _repType = new RepositoryBase<ObjType>();

            ObjType type = _repType.Get(p => p.objType1 == objType);

            return _rep.Get(p => p.schemaName == schemaName && p.objName == objName && p.objType == type.ID);
        }

        public void AddChildDbOject(List<DBObjectsChilds> dbObjChildList)
        {
            RepositoryBase<DBObjectsChilds> _rep = new RepositoryBase<DBObjectsChilds>();
            foreach (var item in dbObjChildList)
            {
                _rep.Add(item);
            }
        }

        public List<DBObjects> GetAllDBObjects()
        {
            RepositoryBase<DBObjects> _rep = new RepositoryBase<DBObjects>();

            return _rep.GetList();
        }

        public List<ObjType> GetTypes()
        {
            RepositoryBase<ObjType> _rep = new RepositoryBase<ObjType>();

            return _rep.GetList();

        }

        public DBObjects GetDBObjectByID(string name)
        {
            RepositoryBase<DBObjects> _rep = new RepositoryBase<DBObjects>();
            RepositoryBase<DBObjectsChilds> _repChild = new RepositoryBase<DBObjectsChilds>();

            DBObjects obj = _rep.Get(p => p.objName == name);

            obj.ChildObjects = _repChild.GetList(p => p.ParentID == obj.ID);

            obj.ChildDBObject = new List<DBObjects>();

            foreach (var item in obj.ChildObjects)
            {
                obj.ChildDBObject.Add(_rep.Get(p => p.ID == item.ChildID));
            }
            return obj;
        }

        public d_b_tables GetTableByNameSchemaName(string tableName, string schemaName)
        {
            RepositoryBase<d_b_tables> _rep = new RepositoryBase<d_b_tables>();
            RepositoryBase<d_b_schemas> _repSchema = new RepositoryBase<d_b_schemas>();
            int schemaID = 0;
            d_b_schemas schema = _repSchema.Get(p => p.name == schemaName);
            if (schema != null)
                schemaID = schema.id;
            if (schemaID != 0)
                return _rep.Get(p => p.name == tableName && p.d_b_schemasId == schemaID);
            else
                return _rep.GetList(p => p.name == tableName).FirstOrDefault();
        }

        public SQLProcedures GetProcedureByNameSchemaName(string name, string SchemaName)
        {
            RepositoryBase<SQLProcedures> _rep = new RepositoryBase<SQLProcedures>(false);

            RepositoryBase<d_b_schemas> _repSchema = new RepositoryBase<d_b_schemas>();
            int schemaID = 0;
            d_b_schemas schema = _repSchema.Get(p => p.name == SchemaName);
            if (schema != null)
                schemaID = schema.id;

            name = name + ".prc";

            SQLProcedures proc = _rep.Get(p => p.name == name && p.schema_id == schemaID);

            if (proc != null)
                return proc;

            name = name + ".fnc";
            proc = _rep.Get(p => p.name == name && p.schema_id == schemaID);

            if (proc != null)
                return proc;

            return null;
        }

        public users SaveUser(users user)
        {
            RepositoryBase<users> _rep = new RepositoryBase<users>();

            user.created_at = DateTime.Now;
            user.updated_at = DateTime.Now;

            return _rep.Add(user);
        }

        public void ChangePassword(users user)
        {
            RepositoryBase<users> _rep = new RepositoryBase<users>();

            users u = _rep.Get(p => p.id == user.id);

            u.password = user.password;

            _rep.Update(u);
        }

        public List<ModulesFiles> GetAllModulesFiles(bool getRelations = true)
        {
            RepositoryBase<ModulesFiles> _rep = new RepositoryBase<ModulesFiles>(getRelations);

            return _rep.GetList();
        }

        public InnerModule AddInnerModule(InnerModule inMod)
        {
            RepositoryBase<InnerModule> _rep = new RepositoryBase<InnerModule>();

            return _rep.Add(inMod);
        }

        public List<InnerModule> GetInnerModule(int moduleID)
        {
            //ID sub module ID 'dir .

            RepositoryBase<InnerModule> _rep = new RepositoryBase<InnerModule>();
            RepositoryBase<Modules> _repMod = new RepositoryBase<Modules>();

            List<InnerModule> result = new List<InnerModule>();

            result = _rep.GetList(p => p.SubContainsModuleID == moduleID);

            foreach (var item in result)
            {
                item.ContainModule = _repMod.Get(p => p.ID == item.ContainModuleID);
                item.Module = _repMod.Get(p => p.ID == item.ModuleID);
            }
            return result;
        }

        public List<d_b_schemas> GetAllSchemas(bool getTables=false)
        {
            RepositoryBase<d_b_schemas> _rep = new RepositoryBase<d_b_schemas>(getTables);

            return _rep.GetList();
        }

     
    }
}

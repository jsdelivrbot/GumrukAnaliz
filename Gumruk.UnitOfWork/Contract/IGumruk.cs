using Gumruk.Entity;
using Gumruk.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gumruk.UnitOfWork.Contract
{
    public interface IGumruk
    {
        List<d_b_tables> GetAllSchemasTables(int schemaId, bool getColumns);

        List<d_b_columns> GetAllColumns(int TableID);

        List<d_b_columns> GetAllColumns();

        List<DataTable> GetAllSchemas(bool getTables, bool getTableColumns,int schemaID);

        List<d_b_schemas> GetAllSchemas(bool getTables=false);

        List<DataSchema> GetSchemaNames(bool getTableCount = true);
        

        DataTable GetTableByName(string TableName);

        d_b_schemas GetSchema(int schemaID);

        d_b_columns GetColumnById(int columnID);

        d_b_tables GetTableById(int tableID);

        List<d_b_column_explanations> GetDetailsByColumnID(int columnID);

        void AddColumnExplanation(int columnID,string Exp,int userID);

        void AddLookup(int column_id_from, int column_id_to);

        int NewTable(d_b_tables tbl);
        int NewColumn(d_b_columns tbl);
        int NewSchema(d_b_schemas tbl);

        List<LookUpModel> GetLookupModels(int columnIDfrom);

        void DeleteExp(int ExpID);

        void DeleteTableExp(int ExpID);

        void AddTableExplanation(int tableID, string Exp, int userID);

        List<d_b_table_explanation> GetTableExplanationsByTableID(int tableID);

        int NewBaseModule(app_base_modules module);

        int NewSubModule(app_sub_modules module);

        int NewModuleOperations(app_module_operations moduleOperations);

        void ExcelImport(List<ColumnToExcel> listClExcel);

        List<app_base_modules> GetAllModules();

        List<app_module_operations> GetAllOperationsForModule(int subModuleId);

        void ExplanationFix();

        List<SearchResult> ColumnExplanationSearch(string searchParam);

        string prepareSQLViewerData(List<int> tableIds,int type);

        void saveXML(string XMLData, string ERtables, string diagramName, int diagramID);

        List<DataTable> GetAllSchemasWitERs(bool getTables, bool getTableColumns, int schemaID);

        ERxml GetErxmlByID(int erID);

        void deleteErDiagrams(int tableIds);

        List<TasimaSekilleri> GetTasimaSekilleri();

        List<BeyanTurleri> GetBeyanTurleri();

        List<GumrukIdareleri> GetGumrukIdareleri();

        List<Ulkeler> GetUlkeler();

        List<Limanlar> GetLimanlarByUlkeId(int ulkeID);

        OzelTuzelSahis GetOzelTuzelSahisByVergiNo(string vergiNo);

        List<KimlikTurleri> GetKimlikTurleri();

        OzelTuzelSahis SahisKaydet(OzelTuzelSahis sahis);

        OzetBeyan OzetBeyanKaydet(OzetBeyan oztBeyan);

        TasitBilgileri SearchTasit(string searchValue);

        TasitBilgileri TasitKaydet(TasitBilgileri tst);

        List<d_b_tables> GetTablesBySubModuleID(int subModuleID);

        TasitBilgileri GetTasitBilgileriByID(int ID);

        OzelTuzelSahis GetTasiyiciFirma(int ID);

        DBErrorMessages SaveErrorMessages(DBErrorMessages message);

        SQLProcedures SaveSQlProcedures(SQLProcedures procedure,string dirName);

        List<SQLProcedures> GetAllProcedures(string letter,int schemaID);

        /// <summary>
        /// ID'si girilen procedure'nin bodysini fotmatlanıdırıp dönderir.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        SQLProcedures GetSQLProcedure(int ID);

        ProceduresSearchResult SearchProcedures(string searchText,bool fromLayout);

        List<SQLProceduresComments> GetSQLProcedureComments(int SQLProcedureID);

        SQLProceduresComments AddSQLProcedureComment(SQLProceduresComments comment,int? position,string sqlText);

        void deleteSQlProcedureComment(int ID);

        SQLProcedures UpdateSQLText(SQLProcedures procedure);

        ///<summary>Tüm Stored Procedure'leri getirir.</summary>
        List<SQLProcedures> GetAllSP(bool getSchemas = false);

        /// <summary>
        /// ID'si girilen procedure'yi hiç bir işleme tabi tutmadan direk olarak döndürür.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        SQLProcedures GetProcedure(int ID);

        List<SQLProcedureModules> GetSQLProcedureModules(int ID);

        /// <summary>
        /// Tümr error mesajlarını getirir.
        /// </summary>
        /// <returns>DBErrorMessages tipinde değer döndürür</returns>
        List<DBErrorMessages> GetAllErrorMessages();

        /// <summary>
        /// Schema'nın kayıtlı olan ER diagramlarını getirir.
        /// </summary>
        /// <param name="schemaID"></param>
        /// <returns></returns>
        List<ERxml> GetSavedDiagramsBySchemaID(int schemaID);

        Modules SaveModule(Modules module);

        SubModules SaveSubModule(SubModules subModule);

        ModulesFiles SaveModulesFiles(ModulesFiles moduleFile);

        List<Modules> GetModules();

        List<ModulesFiles> GetSubModulesFiles(int subModuleID);
        List<ModulesFiles> GetModulesFiles(int ModuleID);

        ModulesFiles GetModuleFile(int modFileID);

        ModulesFiles UpdateModulesFiles(ModulesFiles moduleFile);

        SQLProcedureModules SaveSqlProcedureModules(SQLProcedureModules sqlMod);

        List<SQLProcedureModules> GetSQLProcedureModulesBySubModuleID(int subModuleID);

        List<SQLProcedureModules> GetSQLProcedureModulesByFileID(int FileID);

        List<SQLProcedureModules> GetSQLProcedureModulesByModuleID(int ModuleID);

        List<d_b_tables> GetAllTables();

        ModulesTables SaveModuleTable(ModulesTables moduleTable);

        List<ModulesTables> GetModulesTablesBySubModuleID(int subModuleID);
        List<ModulesTables> GetModulesTablesByModuleID(int ModuleID);

        List<ModulesTables> GetModulesTablesByFileD(int FileID);

        ModulesFiles GetModuleFileByID(int ID);

        DBObjects AddDbOject(DBObjects dbObj);

        void AddDbOject(List<DBObjects> dbObjList);

        DBObjects GetDBObject(string schemaName,string objName,string objType);

        void AddChildDbOject(List<DBObjectsChilds> dbObjChildList);

        List<DBObjects> GetAllDBObjects();

        List<ObjType> GetTypes();

        DBObjects GetDBObjectByID(string name);

        d_b_tables GetTableByNameSchemaName(string tableName,string schemaName);
        SQLProcedures GetProcedureByNameSchemaName(string name,string SchemaName);

        users SaveUser(users user);

        void ChangePassword(users user);

        List<ModulesFiles> GetAllModulesFiles(bool getRelations=true);

        InnerModule AddInnerModule(InnerModule inMod);

        List<InnerModule> GetInnerModule(int moduleID);
    }
}

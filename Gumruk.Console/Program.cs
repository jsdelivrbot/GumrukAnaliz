using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gumruk.Entity;
using Gumruk.UnitOfWork;
using Gumruk.UnitOfWork.Contract;
using System.IO;
using Gumruk.Console;

namespace Gumruk.ConsoleApp
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    IGumruk iGumruk = new BSGumruk();

        //    app_base_modules module = new app_base_modules()
        //    {
        //        created_at=DateTime.Now,
        //        isempty=false,
        //        mid=0,
        //        name="base modul 1",
        //        updated_at=DateTime.Now,
        //    };

        //    int baseModuleID = iGumruk.NewBaseModule(module);
        //    System.Console.WriteLine(baseModuleID.ToString());

        //    app_sub_modules submodule = new app_sub_modules()
        //    {
        //        app_base_module_id=baseModuleID,
        //        created_at=DateTime.Now,
        //        isempty=false,
        //        mid=0,
        //        name="sub modul 1",
        //        updated_at=DateTime.Now,
        //    };

        //    int subModuleID= iGumruk.NewSubModule(submodule);

        //    Console.WriteLine(subModuleID.ToString());

        //    app_module_operations moduleOps = new app_module_operations() 
        //    {
        //        app_sub_module_id=subModuleID,
        //        created_at=DateTime.Now,
        //        operation="Update",
        //        query="update tablo set kolon=3",
        //        d_b_tables_id=1,
        //        updated_at=DateTime.Now,
        //    };

        //    int moduleOperationsID=iGumruk.NewModuleOperations(moduleOps);
        //    Console.WriteLine(moduleOperationsID.ToString());

        //    System.Console.ReadKey();
        //}


        static void Main(string[] args)
        {
            //IGumruk iGumruk = new BSGumruk();

            //iGumruk.ExplanationFix();

            JsonLibrary lib = new JsonLibrary();

            List<JsonItem> items = lib.CreateList();

            lib.ExcelExport(items);

        }


    }
}

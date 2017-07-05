using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //getting list of directory, then reading file names and files counts

            // DirSearch("D:\\Gümrük");

            //string[] files=Directory.GetFiles("D:\\Gümrük","*.*" ,SearchOption.AllDirectories);

            //Console.WriteLine(files.Count().ToString());

            //foreach (string item in files)
            //{

            //}

            using (var streamReader = new StreamReader(@"D:\Gümrük\Gümrük Kanunu .html", Encoding.UTF8))
            {
                 string text = streamReader.ReadToEnd();
            }

            Console.ReadLine();
        }

        static void DirSearch(string sDir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {

                    foreach (string f in Directory.GetFiles(d))
                    {
                        Console.WriteLine(f);
                    }
                    Console.WriteLine("Bu klasördeki dosyalar tamamlandı...");
                    Console.Read();
                    DirSearch(d);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


    }
}

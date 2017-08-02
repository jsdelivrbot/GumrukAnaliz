using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Model
{
    public class MetaColumn
    {
        public string name { get; set; }
        public string tanimi { get; set; }
        public string frekans { get; set; }
        public string mevzuat { get; set; }
        public string referans { get; set; }
        public string referansKaynak { get; set; }
        public string referansVeriListesi { get; set; }
        public string VeriTipi { get; set; }
        public string Uzunluk { get; set; }
        public bool PK { get; set; }
    }
}

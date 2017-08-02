using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Model
{
    public class MetaTable
    {
        public string Name { get; set; }
        public string Aciklama { get; set; }

        public List<MetaColumn> MetaColumns { get; set; }
    }
}

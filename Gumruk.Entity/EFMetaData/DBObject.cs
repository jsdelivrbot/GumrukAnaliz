using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gumruk.Entity
{
    public partial class DBObjects
    {
        public string strObjType { get; set; }

        public List<DBObjectsChilds> ChildObjects { get; set; }

        public List<DBObjects> ChildDBObject { get; set; }

        public int parentID { get; set; }
    }
}

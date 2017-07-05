using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gumruk.Entity
{
    public partial class InnerModule
    {
        public Modules ContainModule { get; set; }
        public Modules Module { get; set; }

    }
}

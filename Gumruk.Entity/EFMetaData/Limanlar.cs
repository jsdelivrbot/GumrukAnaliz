using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gumruk.Entity
{
    partial class Limanlar
    {
        public string LimanTamAdi {
            get {
                return LimanKodu + " " + LimanAdi;
            }

            set
            {
                LimanTamAdi = value;
            }
        }
    }
}

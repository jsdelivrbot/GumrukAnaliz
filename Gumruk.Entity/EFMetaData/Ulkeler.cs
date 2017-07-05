using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gumruk.Entity
{
    public partial class Ulkeler
    {
        public string UlkeTamAdi
        {
            get {  return UlkeKodu + " " + UlkeAdi; }
        }
    }
}

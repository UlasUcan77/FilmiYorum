using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace filmiyorum
{
    public class premium:kullanici
    {
        public string uyelik { get; set; }
        public premium(string a, string b, string c, string d, string e, string f, string g,string h)
            : base(a, b, c, d, e, f, g)
        {
            this.uyelik = h;
        }
        public override int fiyat()
        {
            return base.fiyat() * 25 / 100 + base.fiyat();
        }
     }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace filmiyorum
{
    public class premium:kullanici
    {

        public premium(string a, string b, string c, string d, string e, string f, string g)
            : base(a, b, c, d, e, f, g,"Premium")
        {

        }
        public override int fiyat()
        {
            return base.fiyat() * 25 / 100 + base.fiyat();
        }
     }
}

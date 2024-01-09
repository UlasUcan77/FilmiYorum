using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace filmiyorum
{
    class standart:kullanici
    {
        public string uyelik { get; set; }
        public standart(string a, string b, string c, string d, string e, string f, string g)
            : base(a, b, c, d, e, f, g,"standart")
        {

        }
        public override int fiyat()
        {
            return base.fiyat();
        }
    }
}

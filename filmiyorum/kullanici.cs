using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace filmiyorum
{
    public class kullanici
    {
        public string ad { get; set; }
        public string soyad { get; set; }

        public string tckimlikno { get; set; }

        public string dogumtarihi { get; set; }

        public string cinsiyet { get; set; }

        public string kullaniciadi { get; set; }

        public string sifre { get; set; }

        public string uyelik { get; set; }



        public kullanici(string a,string b,string c, string d, string e, string f , string g, string h)
        {
            this.ad = a;
            this.soyad = b;
            this.tckimlikno = c;
            this.dogumtarihi = d;
            this.cinsiyet = e;
            this.kullaniciadi = f;
            this.sifre = g;
            this.uyelik = h;
        }

        public virtual int fiyat()
        {
            return 100;
        }
    }
}

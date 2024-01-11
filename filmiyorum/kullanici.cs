using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace filmiyorum
{
    public abstract class kullanici
    {
        public string ad { get; set; }
        public string soyad { get; set; }

        public string tckimlikno { get; set; }

        public string dogumtarihi { get; set; }

        public string cinsiyet { get; set; }

        public string kullaniciadi { get; set; }

        public string sifre { get; set; }

        public string uyelik { get; set; }

        public List<string> Watchlist;

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
        public void WatchListSil(string filmAdi)
        {
            Watchlist.Remove(filmAdi);
        }
        public void WatchListEkle(string filmAdi)
        {
            // Eğer Watchlist null ise, yeni bir liste oluştur
            if (Watchlist == null)
            {
                Watchlist = new List<string>();
            }

            // Watchlist'e film adını ekle
            Watchlist.Add(filmAdi);
        
        }
        
        public virtual int fiyat()
        {
            return 100;
        }
    }
}

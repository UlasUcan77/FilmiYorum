using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;


namespace filmiyorum
{
    public partial class profilim : Form
    {
        public profilim()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglan = new NpgsqlConnection("server=localHost; port=5432;Database=Filmiyorum;user ID=postgres; password=1234");

        private void button3_Click(object sender, EventArgs e)
        {
            Anasayfa anasayfa = new Anasayfa();
            anasayfa.Show();
            this.Hide();
        }

        private void profilim_Load(object sender, EventArgs e)
        {
            txtad.Text = Log.User.ad;
            txtsoyad.Text = Log.User.soyad;
            txtdt.Text = Log.User.dogumtarihi;
            txttc.Text = Log.User.tckimlikno;
            cinsiyetbox.SelectedItem = Log.User.cinsiyet;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string kullaniciadi = Log.User.kullaniciadi;
            baglan.Open();
            NpgsqlCommand degis = new NpgsqlCommand("UPDATE \"kullanicilar\" SET ad=@ad, soyad=@soyad, tckimlikno=@tckimlikno, dogumtarihi=@dogumtarihi, cinsiyet=@cinsiyet WHERE kullaniciadi=@kullaniciadi", baglan);
            degis.Parameters.AddWithValue("@ad", txtad.Text);
            degis.Parameters.AddWithValue("@soyad", txtsoyad.Text);
            degis.Parameters.AddWithValue("@tckimlikno", txttc.Text);
            degis.Parameters.AddWithValue("@dogumtarihi", txtdt.Text);
            degis.Parameters.AddWithValue("@cinsiyet", cinsiyetbox.SelectedItem.ToString());
            degis.Parameters.AddWithValue("@kullaniciadi", kullaniciadi);
            degis.ExecuteNonQuery();
            MessageBox.Show("Değiştirme işlemi Başarı ile gerçekleşti...");
            baglan.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciadi = Log.User.kullaniciadi;
            baglan.Open();
            NpgsqlCommand sil = new NpgsqlCommand("DELETE FROM \"kullanicilar\" WHERE kullaniciadi=@kullaniciadi", baglan);
            sil.Parameters.AddWithValue("@kullaniciadi", kullaniciadi);
            sil.ExecuteNonQuery();
            MessageBox.Show("Hesap başarıyla silindi...");
            baglan.Close();
            //BURADAN SONRA LOG'DAN CIKIS YAPILACAK VE GİRİŞ EKRANINA YÖNLENDİRİLECEK
        }
    }
}

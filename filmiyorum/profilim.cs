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
        NpgsqlConnection baglan = new NpgsqlConnection("server=localHost; port=5432;Database=Filmiyorum;user ID=postgres; password=dntf78523sql");

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
            txtdt.Text = Log.User.dogumtarihi.ToString();
            txttc.Text = Log.User.tckimlikno;
            cinsiyetbox.SelectedItem = Log.User.cinsiyet;
            abonelikbox.SelectedItem = Log.User.uyelik;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string kullaniciadi = Log.User.kullaniciadi;
                baglan.Open();
                NpgsqlCommand degis = new NpgsqlCommand("UPDATE \"kullanicilar\" SET ad=:ad, soyad=:soyad, tckimlikno=:tckimlikno, dogumtarihi=:dogumtarihi, cinsiyet=:cinsiyet, uyelik=:uyelik WHERE kullaniciadi=:kullaniciadi", baglan);
                degis.Parameters.AddWithValue(":ad", txtad.Text);
                degis.Parameters.AddWithValue(":soyad", txtsoyad.Text);
                degis.Parameters.AddWithValue(":tckimlikno", txttc.Text);
                degis.Parameters.AddWithValue(":dogumtarihi", Convert.ToDateTime(txtdt.Text));
                degis.Parameters.AddWithValue(":cinsiyet", cinsiyetbox.SelectedItem.ToString());
                degis.Parameters.AddWithValue(":kullaniciadi", kullaniciadi);
                degis.Parameters.AddWithValue(":uyelik", abonelikbox.SelectedItem.ToString());
                degis.ExecuteNonQuery();
                MessageBox.Show("Değiştirme işlemi Başarı ile gerçekleşti...");
                baglan.Close();
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();

                string oncekiAbonelik = Log.User.uyelik;
                if (abonelikbox.SelectedItem != null && abonelikbox.SelectedItem.ToString() != oncekiAbonelik)
                {
                    if (abonelikbox.Text == "Standart")
                    {
                        standart standart = new standart(Log.User.ad, Log.User.soyad, Log.User.tckimlikno, Log.User.dogumtarihi, Log.User.cinsiyet, Log.User.kullaniciadi, Log.User.sifre);
                        MessageBox.Show("Üyelik Ücretiniz '" + standart.fiyat() + "' TL olarak Güncellenmiştir");

                    }
                    else
                    {

                        premium premium = new premium(Log.User.ad, Log.User.soyad, Log.User.tckimlikno, Log.User.dogumtarihi, Log.User.cinsiyet, Log.User.kullaniciadi, Log.User.sifre);
                        MessageBox.Show("Üyelik Ücretiniz '" + premium.fiyat() + "' TL olarak Güncellenmiştir");
                    }
                }
                Log.User = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bilgieri kontrol edin. " + ex.Message);
            }
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
            Log.User = null;
            Form1 giris = new Form1();
            giris.Show();
            this.Hide();
        }

        private void btnWatchlist_Click(object sender, EventArgs e)
        {
            panelWatchlist.Visible = true;

            // Watchlist içindeki tekrar eden filmleri 1e düşürmek için 
            HashSet<string> uniqueFilms = new HashSet<string>();

            // GroupBox içindeki önceki etiketleri temizle
            groupBoxWatchlist.Controls.Clear();
           
            if (Log.User.Watchlist == null || Log.User.Watchlist.Count == 0)
            {
                MessageBox.Show("Watchlist boş.");
            }
            else { 

            for (int i = 0; i < Log.User.Watchlist.Count; i++)
            {
                string filmAdi = Log.User.Watchlist[i];
                int labelwidth = 200;

                // Eğer daha önce eklenmemişse, etiketi oluştur ve GroupBox'a ekle
                if (uniqueFilms.Add(filmAdi))
                {
                    Label label = new Label();
                    label.Width = labelwidth;
                    label.Text = filmAdi;
                    label.Top = 10 + uniqueFilms.Count * 30;  // Y ekseninde aralıklı konumlandırma
                    label.Left = 20;
                    groupBoxWatchlist.Controls.Add(label);
                }

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panelWatchlist.Visible = false;
        }
    }
    }


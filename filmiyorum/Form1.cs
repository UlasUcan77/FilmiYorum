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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            txtKullaniciSifre.UseSystemPasswordChar = true;
            checkSifreGoster.CheckedChanged += checkSifreGoster_CheckedChanged;

        }
        NpgsqlConnection baglan = new NpgsqlConnection("server=localHost; port=5432;Database=Filmiyorum;user ID=postgres; password=dntf78523sql");

        //bağlanma işlemi ve atama işlemleri yapılır

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {


            baglan.Open();
            NpgsqlCommand aktar = new NpgsqlCommand("insert into \"kullanicilar\"(ad, soyad, tckimlikno, dogumtarihi,cinsiyet ,uyelik, kullaniciadi,sifre) values (@ad, @soyad, @tckimlikno, @dogumtarihi,@cinsiyet ,@uyelik, @kullaniciadi,@sifre)", baglan);

            aktar.Parameters.AddWithValue("@ad", adtxt.Text);
            aktar.Parameters.AddWithValue("@soyad", soyadtxt.Text);
            aktar.Parameters.AddWithValue("@tckimlikno", tcnotxt.Text);
            aktar.Parameters.AddWithValue("@dogumtarihi", Convert.ToDateTime(dogumtarihitxt.Text));
            aktar.Parameters.AddWithValue("@cinsiyet", cinsiyetbox.Text);
            aktar.Parameters.AddWithValue("@uyelik", uyelikbox.Text);
            aktar.Parameters.AddWithValue("@kullaniciadi", kullaniciaditxt.Text);
            aktar.Parameters.AddWithValue("@sifre", sifretxt.Text);

            aktar.ExecuteNonQuery();
            baglan.Close();
            MessageBox.Show("Aramıza Hosgeldiniz");

            panel2.Visible = true;
            panel1.Visible = false;

        }

        private void girisyap_Click(object sender, EventArgs e)
        {
            if (txtKullaniciAdi.Text == "admin" && txtKullaniciSifre.Text == "1234")
            {
                Admin admin = new Admin();
                admin.Show();

            }
            else
            {
                NpgsqlCommand komut = new NpgsqlCommand("SELECT * FROM kullanicilar WHERE kullaniciadi = @kullaniciadi", baglan);
                komut.Parameters.AddWithValue("@kullaniciadi", txtKullaniciAdi.Text);

                baglan.Open();
                NpgsqlDataReader reader = komut.ExecuteReader();

                if (reader.Read())
                {
                    string sifre = reader["sifre"].ToString();
                    string ad = reader["ad"].ToString();
                    string soyad = reader["soyad"].ToString();
                    string tckimlikno = reader["tckimlikno"].ToString();
                    string dogumtarihi = reader["dogumtarihi"].ToString();
                    string cinsiyet = reader["cinsiyet"].ToString();
                    string uyelik = reader["uyelik"].ToString();
                    string kullaniciadi = txtKullaniciAdi.Text;

                    int filmCount = 17;
                    int filmSayisi = GetFilmSayisi();

                    NotifyIcon notifyIcon = new NotifyIcon();
                    notifyIcon.Icon = SystemIcons.Information;
                    notifyIcon.Visible = false;

                    if (txtKullaniciSifre.Text == sifre)
                    {
                        if (uyelik == "Premium") { 
                            Log.User = new premium(ad, soyad, tckimlikno, dogumtarihi, cinsiyet, kullaniciadi, sifre);
                            // Kullanıcı adı ve şifre doğrulandı
                            Anasayfa form6 = new Anasayfa();
                            form6.Show();
                            this.Hide();

                            if (filmSayisi > filmCount)
                            {
                                notifyIcon.Visible = true;
                                notifyIcon.BalloonTipTitle = "FilmiYoruma Hoşgeldiniz";
                                notifyIcon.BalloonTipText = filmSayisi - filmCount + " tane yeni film eklenmiştir.";
                                notifyIcon.ShowBalloonTip(3000);
                            }
                            else
                            {
                                notifyIcon.Visible = true;
                                notifyIcon.BalloonTipTitle = "FilmiYoruma Hoşgeldiniz";
                                notifyIcon.BalloonTipText = "Giriş Başarılı";
                                notifyIcon.ShowBalloonTip(3000);
                            }
                        }
                        else
                        {
                            Log.User = new standart(ad, soyad, tckimlikno, dogumtarihi, cinsiyet, kullaniciadi, sifre);
                            // Kullanıcı adı ve şifre doğrulandı
                            Anasayfa form6 = new Anasayfa();
                            form6.Show();
                            this.Hide();

                            if (filmSayisi > filmCount)
                            {
                                notifyIcon.Visible = true;
                                notifyIcon.BalloonTipTitle = "FilmiYoruma Hoşgeldiniz";
                                notifyIcon.BalloonTipText = filmSayisi - filmCount + " tane yeni film eklenmiştir.";
                                notifyIcon.ShowBalloonTip(3000);
                            }
                            else
                            {
                                notifyIcon.Visible = true;
                                notifyIcon.BalloonTipTitle = "FilmiYoruma Hoşgeldiniz";
                                notifyIcon.BalloonTipText = "Giriş Başarılı";
                                notifyIcon.ShowBalloonTip(3000);
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Yanlış şifre. Lütfen tekrar deneyin.");
                    }
                }
                else
                {
                    MessageBox.Show("Kullanıcı bulunamadı.");
                }

                reader.Close();
                baglan.Close();
            }
        }

        private int GetFilmSayisi()
        {
            int filmSayisi = 0;
            using (NpgsqlConnection connection = new NpgsqlConnection("server=localHost; port=5432;Database=Filmiyorum;user ID=postgres; password=dntf78523sql"))
            {
                connection.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(*) FROM filmler", connection))
                {
                    filmSayisi = Convert.ToInt32(cmd.ExecuteScalar());
                }

                connection.Close();
            }
            return filmSayisi;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel1.Visible = false;
        }

        private void uyelikbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uyelikbox.Text == "Premium")
            {
                button4.Text = "125TL ile Uye Olun";
                       
                    }
            else
            {
                button4.Text = "100TL ile Uye Olun";
            }
        }

        private void checkSifreGoster_CheckedChanged(object sender, EventArgs e)
        {
            txtKullaniciSifre.UseSystemPasswordChar = !checkSifreGoster.Checked; //butona basıldığına şifreyi gösterme tekrar basıldıgında tekrar gizleme
        }
    }
}

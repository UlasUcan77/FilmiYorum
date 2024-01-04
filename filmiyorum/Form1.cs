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
        }
        NpgsqlConnection baglan = new NpgsqlConnection("server=localHost; port=5432;Database=Filmiyorum;user ID=postgres; password=1234");

        //bağlanma işlemi ve atama işlemleri yapılır

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string adminkullaniciadi = "admin";
            string adminsifre = "123";

            baglan.Open();
            NpgsqlCommand komut = new NpgsqlCommand("Select * FROM kullanicilar", baglan); //Hastane database'inden giris table'ına erişim
            NpgsqlDataReader reader = komut.ExecuteReader();


            if (reader.Read())
            {
                string kullaniciadi = reader["kullaniciadi"].ToString();    //database'den sifre ve sistemid bilgilerini alma
                string sifre = reader["sifre"].ToString();

                if (txtKullaniciAdi.Text == kullaniciadi && txtKullaniciSifre.Text == sifre)  //kullanıcıdan alınanlarla sistemdeki bilgileri karşılaştırma
                {
                    Anasayfa form2 = new Anasayfa();
                    form2.ShowDialog();
                    this.Hide();
                }
                else if(txtKullaniciAdi.Text== adminkullaniciadi && txtKullaniciSifre.Text==adminsifre) 
                {
                    Admin form3 = new Admin();
                    form3.ShowDialog();
                    this.Hide();

                    MessageBox.Show("Yanlış sistem id veya şifre. \nLütfen tekrar deneyiniz.\nŞifrenizi unuttuysanız 'Şifremi Unuttum'a tıklayınız.");
                }
            }

            reader.Close();
            baglan.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
      

            baglan.Open();
            NpgsqlCommand aktar = new NpgsqlCommand("insert into \"kullanicilar\"(ad, soyad, tckimlikno, dogumtarihi,cinsiyet ,uyelik, kullaniciadi,sifre) values (@ad, @soyad, @tckimlikno, @dogumtarihi,@cinsiyet ,@uyelik, @kullaniciadi,@sifre)", baglan);

            aktar.Parameters.AddWithValue("@ad", adtxt.Text);
            aktar.Parameters.AddWithValue("@soyad", soyadtxt.Text);
            aktar.Parameters.AddWithValue("@tckimlikno", tcnotxt.Text);
            aktar.Parameters.AddWithValue("@dogumtarihi", dogumtarihitxt.Text);
            aktar.Parameters.AddWithValue("@cinsiyet", cinsiyetbox.Text);
            aktar.Parameters.AddWithValue("@uyelik", uyelikbox.Text);
            aktar.Parameters.AddWithValue("@kullaniciadi", kullaniciaditxt.Text);
            aktar.Parameters.AddWithValue("@sifre", sifretxt.Text);

            aktar.ExecuteNonQuery();
            baglan.Close();
            MessageBox.Show("Aramıza Hosgeldiniz");
            Anasayfa form2 = new Anasayfa();
            form2.Show();
            Form1 formuye = new Form1();
            formuye.Hide();

        }

        private void girisyap_Click(object sender, EventArgs e)
        {
            string adminkullaniciadi = "admin";
            string adminsifre = "123";
            baglan.Open();
            NpgsqlCommand komut = new NpgsqlCommand("Select * FROM kullanicilar", baglan); //Hastane database'inden giris table'ına erişim
            NpgsqlDataReader reader = komut.ExecuteReader();


            if (reader.Read())
            {
                string kullaniciadi = reader["kullaniciadi"].ToString();    //database'den sifre ve sistemid bilgilerini alma
                string sifre = reader["sifre"].ToString();

                if (txtKullaniciAdi.Text == kullaniciadi && txtKullaniciSifre.Text == sifre)  //kullanıcıdan alınanlarla sistemdeki bilgileri karşılaştırma
                {
                    Anasayfa form2 = new Anasayfa();
                    form2.ShowDialog();
                    this.Hide();
                }
                else if (txtKullaniciAdi.Text == adminkullaniciadi && txtKullaniciSifre.Text == adminsifre)
                {
                    Admin form3 = new Admin();
                    form3.ShowDialog();
                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Yanlış sistem id veya şifre. \nLütfen tekrar deneyiniz.\nŞifrenizi unuttuysanız 'Şifremi Unuttum'a tıklayınız.");
                }
            }

            reader.Close();
            baglan.Close();
        }
    }
}

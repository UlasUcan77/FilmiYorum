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

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Anasayfa form2 = new Anasayfa();
            form2.Show();
            Admin form3 = new Admin();
            form3.Show();
            profil form4 = new profil();
            form4.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

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
    }
}

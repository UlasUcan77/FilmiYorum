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
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglan = new NpgsqlConnection("server=localHost; port=5432;Database=Filmiyorum;user ID=postgres; password=1234");

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Visible = true;
            button3.Visible = true;
            button1.Visible = false;
            baglan.Open();
            NpgsqlCommand cagir = new NpgsqlCommand("SELECT yonetmen, tur, yayinyili,oyuncular FROM  \"filmler\" WHERE filmadi=@filmadi", baglan);
            string filmadi = textBox1.Text;
            cagir.Parameters.AddWithValue("@filmadi", filmadi);
            NpgsqlDataReader reader = cagir.ExecuteReader();
            if (reader.Read())
            {
                textBox2.Text = reader["yonetmen"].ToString();
                textBox3.Text = reader["yayinyili"].ToString();
                textBox4.Text = reader["tur"].ToString();
                richTextBox1.Text = reader["oyuncular"].ToString();
            }
            reader.Close();
            baglan.Close();




        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filmadi = textBox1.Text;
            baglan.Open();
            NpgsqlCommand sil = new NpgsqlCommand("delete from \"filmler\" where filmadi=@filmadi ", baglan);
            sil.Parameters.AddWithValue("@filmadi", filmadi);
            sil.ExecuteNonQuery();
            baglan.Close();
            MessageBox.Show("Başarı ile silindi");
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            richTextBox1.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string filmadi = textBox8.Text;
            string yonetmen = textBox7.Text;
            DateTime yayinyili =Convert.ToDateTime( textBox6.Text);
            string tur = textBox5.Text;
            string oyuncular = richTextBox2.Text;
            baglan.Open();

            //kayıt işlemi için komut tanımlanır
            NpgsqlCommand aktar = new NpgsqlCommand("insert into \"filmler\"(filmadi, yonetmen, yayinyili, tur,oyuncular) values (@filmadi, @yonetmen, @yayinyili, @tur,@oyuncular)", baglan);
            aktar.Parameters.AddWithValue("@filmadi", filmadi);
            aktar.Parameters.AddWithValue("@yonetmen", yonetmen);
            aktar.Parameters.AddWithValue("@yayinyili", yayinyili);
            aktar.Parameters.AddWithValue("@tur", tur);
            aktar.Parameters.AddWithValue("@oyuncular", oyuncular);

            aktar.ExecuteNonQuery();


            baglan.Close();


            MessageBox.Show("Kayıt İşlemi Başarı ile Tamamlandı...");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filmadi = textBox1.Text;
            baglan.Open();
            NpgsqlCommand degis = new NpgsqlCommand("update \"filmler\" set yonetmen=@yonetmen, tur=@tur, yayinyili=@yayinyili,oyuncular=@oyuncular WHERE filmadi=@filmadi", baglan);
            degis.Parameters.AddWithValue("@yonetmen", textBox2.Text);
            degis.Parameters.AddWithValue("@tur", textBox4.Text);
            degis.Parameters.AddWithValue("@yayinyili",Convert.ToDateTime( textBox3.Text));
            degis.Parameters.AddWithValue("@oyuncular", richTextBox1.Text);
            degis.Parameters.AddWithValue("@filmadi", filmadi);
            degis.ExecuteNonQuery();
            MessageBox.Show("Değiştirme işlemi Başarı ile gerçekleşti...");
            baglan.Close();
        }
    }
}

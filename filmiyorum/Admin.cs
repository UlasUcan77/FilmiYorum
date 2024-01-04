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
    }
}

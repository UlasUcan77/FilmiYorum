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
    public partial class Anasayfa : Form
    {
        public Anasayfa()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglan = new NpgsqlConnection("server=localHost; port=5432;Database=Filmiyorum;user ID=postgres; password=1234");
        private void button7_Click(object sender, EventArgs e)
        {
            if (degerlendirme.Visible)
            {
                degerlendirme.Visible = false; // Eğer panel görünürse, gizle
            }
            else
            {
                degerlendirme.Visible = true; // Eğer panel gizliyse, göster
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {

            string puan = trackBar1.Value.ToString();
            
            if (degerlendirme.Visible)
            {
                degerlendirme.Visible = false; // Eğer panel görünürse, gizle
            }
            else
            {
                degerlendirme.Visible = true; // Eğer panel gizliyse, göster
            }
        }

        private void Anasayfa_Load(object sender, EventArgs e)
        {
            // Npgsql kütüphanesini kullanarak PostgreSQL bağlantısı oluşturulduğunu varsayalım
            // NpgsqlConnection baglan = new NpgsqlConnection(connectionString);

            // Veritabanından veriyi almak için NpgsqlCommand oluşturun
            NpgsqlCommand komut = new NpgsqlCommand("SELECT afis FROM filmler WHERE filmadi = 'Cars'", baglan);

            // Veritabanı bağlantısını açın
            baglan.Open();

            // Veriyi okumak için NpgsqlDataReader kullanın
            NpgsqlDataReader reader = komut.ExecuteReader();

            if (reader.Read())
            {
                byte[] binaryData = (byte[])reader["afis"]; // Veritabanından alınan binary veri

                // Byte dizisinden görüntü oluşturma
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(binaryData))
                {
                    Image image = Image.FromStream(ms);

                    // PictureBox üzerinde görüntüyü gösterme
                    pictureBox1.Image = image;
                }
            }

            // Veritabanı bağlantısını kapatın
            baglan.Close();

        }


    }
}

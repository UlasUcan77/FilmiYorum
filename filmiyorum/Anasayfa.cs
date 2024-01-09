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
            degerlendirmePanel.Visible = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {

            string puan = trackBar1.Value.ToString();
            string degerlendirme = txtyorum.Text;
            string kullaniciadi = Log.User.kullaniciadi;
            string filmAdiLabel = lblFilmadi.Text;
            string[] splitFilmAdi = filmAdiLabel.Split(':');
            string filmAdi = splitFilmAdi[1].Trim();

            baglan.Open();
            NpgsqlCommand ekle = new NpgsqlCommand("INSERT INTO degerlendirme (kullaniciadi, puan, degerlendirme,filmadi) VALUES (@kullaniciadi, @puan, @degerlendirme,@filmadi)", baglan);
            ekle.Parameters.AddWithValue("@kullaniciadi", kullaniciadi);
            ekle.Parameters.AddWithValue("@puan", puan);
            ekle.Parameters.AddWithValue("@degerlendirme", degerlendirme);
            ekle.Parameters.AddWithValue("@filmadi", filmAdi);
            ekle.ExecuteNonQuery();
            baglan.Close();

            panel2.Visible = true;
        }

        private void Anasayfa_Load(object sender, EventArgs e)
        {

            if (Log.User != null)
            {
                if (Log.User.ad != null)
                {
                    label2.Text = Log.User.ad;
                }
                else
                {
                    label2.Text = "Ad bilgisi bulunamadı";
                }
            }
            else
            {
                label2.Text = "Kullanıcı bilgisi bulunamadı";
            }
            baglan.Open();
            NpgsqlCommand komut = new NpgsqlCommand("SELECT filmadi,afis FROM filmler", baglan);
            NpgsqlDataReader reader = komut.ExecuteReader();

            int pictureBoxWidth = 160; // PictureBox'ın genişliği
            int pictureBoxHeight = 180; // PictureBox'ın yüksekliği
            int spacing = 20; // PictureBox'lar arasındaki boşluk

            int panelWidth = 925; // Panel genişliği
            int currentX = 20; // Şu anki PictureBox'ın X konumu
            int currentY = 80; // Şu anki PictureBox'ın Y konumu




            while (reader.Read())
            {

                if (reader["afis"] != DBNull.Value)
                {
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Height = pictureBoxHeight;
                    pictureBox.Width = pictureBoxWidth;
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    byte[] binaryData = (byte[])reader["afis"];
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(binaryData))
                    {

                        Image image = Image.FromStream(ms);
                        pictureBox.Image = image;
                    }
                    pictureBox.Click += PictureBox_Click;
                    pictureBox.Tag = reader["filmadi"];

                    pictureBox.Location = new Point(currentX, currentY);

                    // Yatayda sığabilecek kadar yer varsa
                    if (currentX + pictureBoxWidth + spacing < panelWidth - spacing)
                    {
                        currentX += pictureBoxWidth + spacing;
                    }
                    else
                    {
                        // Yatayda sığamıyorsa, bir alt satıra geç
                        currentX = 20;
                        currentY += pictureBoxHeight + spacing;
                    }

                    panel2.Controls.Add(pictureBox);

                }
            }


            panel2.Width = panelWidth;
            panel2.AutoScroll = true;
            baglan.Close();
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {

            PictureBox pictureBox = (PictureBox)sender;
            string filmadi = pictureBox.Tag.ToString();

            baglan.Open();
            NpgsqlCommand filmBilgiKomut = new NpgsqlCommand("SELECT * FROM filmler WHERE filmadi = @filmadi", baglan);
            filmBilgiKomut.Parameters.AddWithValue("@filmadi", filmadi);

            NpgsqlDataReader filmReader = filmBilgiKomut.ExecuteReader();

            if (filmReader.Read())
            {
                // Film bilgilerini labellara yazdır
                lblFilmadi.Text = "Filmind adı: " + filmReader["filmadi"].ToString();
                lblYonetmen.Text = "Yönetmen: " + filmReader["yonetmen"].ToString();
                // lblTur.Text = "Türü: " + filmReader["tur"]; 
                lblTarih.Text = "Çıkış Tarihi: " + filmReader["yayinyili"].ToString();
                lblPuan.Text = "Puan: " + filmReader["puan"].ToString();
                lblOyuncular.Text = "Oyuncular: " + filmReader["oyuncular"].ToString();
                byte[] binaryData = (byte[])filmReader["afis"];
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(binaryData))
                {

                    Image image = Image.FromStream(ms);
                    picFilm.Image = image;
                }
            }

            baglan.Close();

            panel2.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            profilim profilim = new profilim();
            profilim.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }
    }
}

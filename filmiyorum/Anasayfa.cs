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
        NpgsqlConnection baglan = new NpgsqlConnection("server=localHost; port=5432;Database=Filmiyorum;user ID=postgres; password=dntf78523sql");
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
            baglan.Open();
            NpgsqlCommand komut = new NpgsqlCommand("SELECT filmadi,afis FROM filmler", baglan);
            NpgsqlDataReader reader = komut.ExecuteReader();

            int pictureBoxWidth = 100; // PictureBox'ın genişliği
            int pictureBoxHeight = 150; // PictureBox'ın yüksekliği
            int spacing = 14; // PictureBox'lar arasındaki boşluk

            int panelWidth = panel2.ClientSize.Width; // Panel genişliği
            int currentX = 50; // Şu anki PictureBox'ın X konumu
            int currentY = 50; // Şu anki PictureBox'ın Y konumu

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
                    if (currentX + pictureBoxWidth + spacing < panelWidth)
                    {
                        currentX += pictureBoxWidth + spacing;
                    }
                    else
                    {
                        // Yatayda sığamıyorsa, bir alt satıra geç
                        currentX = 0;
                        currentY += pictureBoxHeight + spacing;
                    }

                    panel2.Controls.Add(pictureBox);

                }
            }
            baglan.Close();
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {

            PictureBox pictureBox = (PictureBox)sender;
            string filmadi = pictureBox.Tag.ToString();
        }
    }
}

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
            int filmCount = 17;
            int filmSayisi = GetFilmSayisi();

            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Information;
            if (filmSayisi > filmCount)
            {
                notifyIcon.Visible = true;
                notifyIcon.BalloonTipTitle = "FilmiYoruma Hoşgeldiniz";
                notifyIcon.BalloonTipText = filmSayisi - filmCount + " tane yeni film eklenmiştir.";
                notifyIcon.ShowBalloonTip(3000);
            }
            else
            {
                notifyIcon.BalloonTipTitle = "FilmiYoruma Hoşgeldiniz";
                notifyIcon.BalloonTipText = "Giriş Başarılı";
                notifyIcon.ShowBalloonTip(3000);
            }


        }
        NpgsqlConnection baglan = new NpgsqlConnection("server=localHost; port=5432;Database=Filmiyorum;user ID=postgres; password=1234");


        private int GetFilmSayisi()
        {
            // Film sayısını veritabanından al
            int filmSayisi = 0;

            baglan.Open();

            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(*) FROM filmler", baglan))
            {
                filmSayisi = Convert.ToInt32(cmd.ExecuteScalar());
            }

            baglan.Close();


            return filmSayisi;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            degerlendirmePanel.Visible = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {

            double degerlendirmepuani = trackBar1.Value;
            string degerlendirme = txtyorum.Text;
            string kullaniciadi = Log.User.kullaniciadi;
            string filmAdi = lblFilmAdi.Text;
            

            baglan.Open();
            NpgsqlCommand ekle = new NpgsqlCommand("INSERT INTO degerlendirme (kullaniciadi, degerlendirmepuani, degerlendirme,filmadi) VALUES (@kullaniciadi, @degerlendirmepuani, @degerlendirme,@filmadi)", baglan);
            ekle.Parameters.AddWithValue("@kullaniciadi", kullaniciadi);
            ekle.Parameters.AddWithValue("@degerlendirmepuani", degerlendirmepuani);
            ekle.Parameters.AddWithValue("@degerlendirme", degerlendirme);
            ekle.Parameters.AddWithValue("@filmadi", filmAdi);
            ekle.ExecuteNonQuery();
            baglan.Close();

            panel2.Visible = true;
        }

        private void Anasayfa_Load(object sender, EventArgs e)
        {
            label4.Text = Log.User.uyelik;
            if (Log.User.uyelik == "Premium")
            {
                button7.Visible = true;
            }
            else if(Log.User.uyelik == "Standart")
            {
                button7.Visible = false;
            }
            else
            {
                MessageBox.Show("HATA");
            }
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
            NpgsqlCommand filmBilgiKomut = new NpgsqlCommand("SELECT f.filmadi, f.yonetmen, f.tur, f.yayinyili, f.puan, f.oyuncular, f.afis, d.degerlendirme, d.degerlendirmepuani, d.kullaniciadi FROM filmler f INNER JOIN degerlendirme d ON f.filmadi = d.filmadi WHERE f.filmadi = @filmadi;", baglan);
            filmBilgiKomut.Parameters.AddWithValue("@filmadi", filmadi);

            NpgsqlDataReader filmReader = filmBilgiKomut.ExecuteReader();

            int YorumlarWidth = 400;
            int YorumlarHeight = 300;


            RichTextBox richTextBox = new RichTextBox();
            richTextBox.Height = YorumlarHeight;
            richTextBox.Width = YorumlarWidth;

            int currentX = 670;
            int currentY = 50;

            richTextBox.Location = new Point(currentX, currentY);
            if (filmReader.Read())
            {
                lblFilmAdi.Text = filmReader["filmadi"].ToString();
                lblYonetmen.Text = filmReader["yonetmen"].ToString();
                lblTur.Text = filmReader["tur"].ToString();
                lblTarih.Text = filmReader["yayinyili"].ToString();
                lblPuan.Text = filmReader["puan"].ToString();
                lblOyuncular.Text = filmReader["oyuncular"].ToString();
                byte[] binaryData = (byte[])filmReader["afis"];
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(binaryData))
                {

                    Image image = Image.FromStream(ms);
                    picFilm.Image = image;
                }

                do
                {
                    string kullaniciAdi = filmReader["kullaniciadi"].ToString();
                    string degerlendirme = filmReader["degerlendirme"].ToString();
                    string puan = filmReader["degerlendirmepuani"].ToString();

                    if (string.IsNullOrEmpty(degerlendirme) || string.IsNullOrEmpty(puan))
                    {
                        richTextBox.Text += "\n Henüz yorum yapılmamış.\n";
                    }
                    else
                    {
                        richTextBox.Text += "\n   Kullanıcı Adı: " + kullaniciAdi + " Puan: " + puan + "\n " + degerlendirme + "\n";
                    }

                } while (filmReader.Read());

                panel3.Controls.Add(richTextBox);
            }

            baglan.Close();
            richTextBox.SelectionStart = richTextBox.Text.Length;
            richTextBox.ScrollToCaret();
            panel2.Visible = false;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            profilim profilim = new profilim();
            profilim.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e) //ARAMA BUTONU
        {

            foreach (Control control in panel2.Controls.OfType<PictureBox>().ToList())
            {
                panel2.Controls.Remove(control);
                control.Dispose(); // Dispose, PictureBox nesnesini bellekten temizler
            }
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Lütfen bir filtre seçin...");
            }
            else
            {
                string filtre = comboBox1.SelectedItem.ToString(); // Seçilen filtre
            string arama = textBox1.Text; // Arama metni

            string sorgu = string.Empty;
 

            // Filtreye göre sorgu oluşturma
            switch (filtre)
            {
                case "Film Adi":
                    sorgu = "SELECT filmadi,afis FROM filmler WHERE filmadi LIKE @arama";
                    break;
                case "Yönetmen":
                    sorgu = "SELECT filmadi,afis FROM filmler WHERE yonetmen LIKE @arama";
                    break;
                case "Tür":
                    sorgu = "SELECT filmadi,afis FROM filmler WHERE tur LIKE @arama";
                    break;
                default:
                    MessageBox.Show("Lütfen bir filtre seçin.");
                    break;
            }

                if (!string.IsNullOrEmpty(sorgu))
                {
                    baglan.Open();
                    NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglan);
                    komut.Parameters.AddWithValue("@arama", "%" + arama + "%");

                    NpgsqlDataReader reader = komut.ExecuteReader();

                    int pictureBoxWidth = 160; // PictureBox'ın genişliği
                    int pictureBoxHeight = 180; // PictureBox'ın yüksekliği
                    int spacing = 20; // PictureBox'lar arasındaki boşluk

                    int panelWidth = 925; // Panel genişliği
                    int currentX = 20; // Şu anki PictureBox'ın X konumu
                    int currentY = 80; // Şu anki PictureBox'ın Y konumu


                    while (reader.Read())
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

                    // ... Diğer işlemler buraya eklenir.

                    panel2.Width = panelWidth;
                    panel2.AutoScroll = true;
                    baglan.Close();
                }
                }
        }

        private void btnMaxPuan_Click(object sender, EventArgs e)  // PUANA GÖRE SIRALAMA BUTONU
        {
            foreach (Control control in panel2.Controls.OfType<PictureBox>().ToList())
            {
                panel2.Controls.Remove(control);
                control.Dispose(); // Dispose, PictureBox nesnesini bellekten temizler
            }

            baglan.Open();
            NpgsqlCommand komut = new NpgsqlCommand("SELECT * FROM filmler ORDER BY puan DESC LIMIT 10", baglan);
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

        private void btnLatest_Click(object sender, EventArgs e) // TARİHE GÖRE SIRALAMA
        {
            foreach (Control control in panel2.Controls.OfType<PictureBox>().ToList())
            {
                panel2.Controls.Remove(control);
                control.Dispose(); // Dispose, PictureBox nesnesini bellekten temizler
            }

            baglan.Open();
            NpgsqlCommand komut = new NpgsqlCommand("SELECT * FROM filmler ORDER BY yayinyili DESC", baglan);
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

        private void btnAll_Click(object sender, EventArgs e)
        {

            foreach (Control control in panel2.Controls.OfType<PictureBox>().ToList())
            {
                panel2.Controls.Remove(control);
                control.Dispose(); // Dispose, PictureBox nesnesini bellekten temizler
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

        private void button10_Click(object sender, EventArgs e)
        {
            Log.User = null;
            Form1 giris = new Form1();
            giris.Show();
            this.Hide();
        }

        private void btnMaxPuan_Click_1(object sender, EventArgs e)
        {
            foreach (Control control in panel2.Controls.OfType<PictureBox>().ToList())
            {
                panel2.Controls.Remove(control);
                control.Dispose(); // Dispose, PictureBox nesnesini bellekten temizler
            }

            baglan.Open();
            NpgsqlCommand komut = new NpgsqlCommand("SELECT * FROM filmler ORDER BY puan DESC LIMIT 10", baglan);
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

        private void btnLatest_Click_1(object sender, EventArgs e)
        {
            foreach (Control control in panel2.Controls.OfType<PictureBox>().ToList())
            {
                panel2.Controls.Remove(control);
                control.Dispose(); // Dispose, PictureBox nesnesini bellekten temizler
            }

            baglan.Open();
            NpgsqlCommand komut = new NpgsqlCommand("SELECT * FROM filmler ORDER BY yayinyili DESC LIMIT 10", baglan);
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
            baglan.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (Control control in panel2.Controls.OfType<PictureBox>().ToList())
            {
                panel2.Controls.Remove(control);
                control.Dispose(); // Dispose, PictureBox nesnesini bellekten temizler
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

        private void button1_Click(object sender, EventArgs e)
        {
            degerlendirmePanel.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Control control in panel3.Controls.OfType<RichTextBox>().ToList())
            {
                panel3.Controls.Remove(control);
                control.Dispose(); // Dispose, RichTextBox nesnesini bellekten temizler
            }
            panel2.Visible = true;
        }



        private void button5_Click(object sender, EventArgs e)
        {//EKLE BUTONU
            string filmadi = lblFilmAdi.Text;
            Log.User.WatchListEkle(filmadi);
            MessageBox.Show(filmadi +" Filmi Listenize Eklenmiştir");
            button5.Enabled = false;
            button6.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {//ÇIKAR BUTONU
            string filmadi = lblFilmAdi.Text;
            Log.User.WatchListSil(filmadi);
            MessageBox.Show(filmadi+" Filmi Listenizden Çıkartılmıştır");
            button5.Enabled = true;
            button6.Enabled = false;
        }
    }
}
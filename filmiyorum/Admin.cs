﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        NpgsqlConnection baglan = new NpgsqlConnection("server=localHost; port=5432;Database=Filmiyorum;user ID=postgres; password=dntf78523sql");

        private void button1_Click(object sender, EventArgs e)   // BİLGİ GÜNCELLEME
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Film Adı Girin!");
            }
            else
            {


                button5.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
                button1.Visible = false;
                label3.Visible = label4.Visible = label5.Visible = label6.Visible = true;
                textBox2.Visible = textBox3.Visible = textBox4.Visible = richTextBox1.Visible = true;
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
        }

        private void button3_Click(object sender, EventArgs e)  // FİLM SİLME
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

        private void button4_Click(object sender, EventArgs e)  //FİLM Ekleme
        {
            if(textBox8.Text != ""&& textBox7.Text != "" && textBox6.Text != "" && textBox5.Text != "" && richTextBox2.Text != "") 
            { 
                string filmadi = textBox8.Text;
                string yonetmen = textBox7.Text;
                DateTime yayinyili =Convert.ToDateTime( textBox6.Text);
                string tur = textBox5.Text;
                string oyuncular = richTextBox2.Text;
                baglan.Open();

                //kayıt işlemi için komut tanımlanır
                NpgsqlCommand aktar = new NpgsqlCommand("insert into \"filmler\"(filmadi, yonetmen, yayinyili, tur,oyuncular, afis) values (@filmadi, @yonetmen, @yayinyili, @tur,@oyuncular, @afis)", baglan);
                aktar.Parameters.AddWithValue("@filmadi", filmadi);
                aktar.Parameters.AddWithValue("@yonetmen", yonetmen);
                aktar.Parameters.AddWithValue("@yayinyili", yayinyili);
                aktar.Parameters.AddWithValue("@tur", tur);
                aktar.Parameters.AddWithValue("@oyuncular", oyuncular);
                if (pictureBox1.Image != null)
                {
                    byte[] imageBytes = ImageToByteArray(pictureBox1.Image);
                    aktar.Parameters.Add("@afis", NpgsqlTypes.NpgsqlDbType.Bytea).Value = imageBytes; // bytea olarak kaydetme
                        
                }
                else
                {
                    MessageBox.Show("Önce uygun bir afiş seçin.");
                    return;
                }

                try
                {
                    aktar.ExecuteNonQuery();
                    MessageBox.Show("Film verisi başarıyla eklendi.");


                    // Film adı değerlendirme veritabanına eklenir
                    NpgsqlCommand degerlendirmeEkle = new NpgsqlCommand("INSERT INTO \"degerlendirme\" (filmadi) VALUES (@filmadi)", baglan);
                    degerlendirmeEkle.Parameters.AddWithValue("@filmadi", filmadi);
                    degerlendirmeEkle.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Film verisi eklenirken bir hata oluştu: " + ex.Message);
                }
                finally
                {
                    baglan.Close();
                }
            }
            else
            {
                MessageBox.Show("Boş Kutu Bırakmayınız Lütfen");
            }
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);  // afişi byte array'e dönüştürme
                return ms.ToArray();
            }
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

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button1.Visible = true;
            label3.Visible = label4.Visible = label5.Visible = label6.Visible = false;
            textBox2.Visible = textBox3.Visible = textBox4.Visible = richTextBox1.Visible = false;
            textBox1.Clear();
            
        }

        private void afisEkle_Click(object sender, EventArgs e)
        {
           
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string imagePath = dialog.FileName;
                pictureBox1.Image = Image.FromFile(imagePath);
            }

        }
    }
}

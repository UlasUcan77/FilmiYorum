using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace filmiyorum
{
    public partial class Anasayfa : Form
    {
        public Anasayfa()
        {
            InitializeComponent();
        }

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
            if (degerlendirme.Visible)
            {
                degerlendirme.Visible = false; // Eğer panel görünürse, gizle
            }
            else
            {
                degerlendirme.Visible = true; // Eğer panel gizliyse, göster
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto1_Compiladores_Version1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.AutoScroll = true;
            try
            {

                PictureBox p1 = new PictureBox();
                p1.Height = 900;
                p1.Width = 900;
                p1.SizeMode = PictureBoxSizeMode.AutoSize;
                panel1.Controls.Add(p1);


                 System.IO.FileStream fs;
                 fs = new System.IO.FileStream("grafo1.png",
                  System.IO.FileMode.Open, System.IO.FileAccess.Read);                
                 //this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                 //pictureBox1.Image = System.Drawing.Image.FromStream(fs);
                 p1.Image = System.Drawing.Image.FromStream(fs);
                fs.Close();
               

            }
            catch (Exception x)
            {

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

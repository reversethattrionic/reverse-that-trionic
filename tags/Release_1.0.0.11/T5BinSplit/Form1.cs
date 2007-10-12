using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BinSplit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox1.Text))
            {
                FileInfo fi = new FileInfo(textBox1.Text);

                FileStream fs = File.Create("chip2.bin");
                BinaryWriter bw = new BinaryWriter(fs);
                FileStream fs2 = File.Create("chip1.bin");
                BinaryWriter bw2 = new BinaryWriter(fs2);
                FileStream fsi1 = File.OpenRead(textBox1.Text);
                BinaryReader br1 = new BinaryReader(fsi1);
                bool toggle = false;
                for (int tel = 0; tel < fi.Length; tel++)
                {
                    Byte ib1 = br1.ReadByte();
                    if (!toggle)
                    {
                        toggle = true;
                        bw.Write(ib1);
                    }
                    else
                    {
                        toggle = false;
                        bw2.Write(ib1);
                    }
                    
                    //Byte ib2 = br2.ReadByte();
                    //bw.Write(ib2);
                    //bw.Write(ib1);
                }

                bw.Close();
                bw2.Close();
                fs.Close();
                fs2.Close();
                fsi1.Close();
                br1.Close();
                //fsi2.Close();
                //br2.Close();

                MessageBox.Show("File split!");

            }

        }
    }
}
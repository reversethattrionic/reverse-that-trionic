using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

/* Calculates and updates Trionic 5 checksum in a binary file
 * Makes a copy of the input file with extension -calculated.bin
 */

/* checksum fields T5:
 * 1.   Entire the file upto the part where FFFF values start calculated as checksum-32 (addition)
 *      checkusmlocation: 0x0003FFFC - 0x0003FFFF MSB first
 * 2.   BRUTE FORCE OPTION!!!
 * 
 * */

namespace T5Checksum
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static int IndexOf(byte[] arrayToSearchThrough, byte[] patternToFind)
        {
            if (patternToFind.Length > arrayToSearchThrough.Length)
                return -1;
            for (int i = 0; i < arrayToSearchThrough.Length - patternToFind.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < patternToFind.Length; j++)
                {
                    if (arrayToSearchThrough[i + j] != patternToFind[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i;
                }
            }
            return -1;
        }



       /* private int IndexOf(byte[] input, byte[] pattern)
        {
            byte firstByte = pattern[0];
            int index = -1;

            if ((index = Array.IndexOf(input, firstByte)) >= 0)
            {
                for (int i = 0; i < pattern.Length; i++)
                {
                    if (index + i >= input.Length ||
                     pattern[i] != input[index + i]) return -1;
                }
            }

            return index;
        }*/


        private void AddLogItem(string item)
        {
            listBox1.Items.Add(item);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            Console.WriteLine(item);
            Application.DoEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] firstpattern = new byte[4] {0x4E, 0xFA, 0xFB ,0xCC};
            byte[] secondpattern = new byte[4] {0x13, 0xFC, 0x00 ,0xFF};

            uint checksum=0;
            AddLogItem("Checking file for validity: " + textBox1.Text);
            if (File.Exists(textBox1.Text))
            {
                AddLogItem("File exists, checking length");
                FileInfo fi = new FileInfo(textBox1.Text);
                if (fi.Length == 0x00040000)
                {
                    AddLogItem("File has correct length: 0x00040000 bytes, start parsing");
                    // lees eerst de gehele file in
                    byte[] filebytes = File.ReadAllBytes(textBox1.Text);
                    // nu nog de eindmarkering vinden!
                    // 4EFAFBCC = eindmarkering
                    // 13FC00FF = eindmarkering data
                    int indexoffirstmarking = IndexOf(filebytes, firstpattern);
                    int indexofsecondmarking = IndexOf(filebytes, secondpattern);
                    if (indexoffirstmarking > 0)
                    {
                        AddLogItem("Marker (4EFAFBCC) found on: " + indexoffirstmarking.ToString("X08"));
                        FileStream fsi1 = File.OpenRead(textBox1.Text);
                        BinaryReader br1 = new BinaryReader(fsi1);
                        AddLogItem("Start checksum (32 bit) calculation");
                        for (int tel = 0; tel < indexoffirstmarking + 4/*0x0002A590*/; tel++)
                        {
                            Byte ib1 = br1.ReadByte();
                            checksum += ib1;
                        }
                        // checksum is now complete
                        // verify against value in file
                        AddLogItem("Reading current file checksum");
                        fsi1.Position = 0x0003FFFC;
                        uint readchecksum = (uint)br1.ReadByte() * 0x01000000;
                        readchecksum += (uint)br1.ReadByte() * 0x00010000;
                        readchecksum += (uint)br1.ReadByte() * 0x00000100;
                        readchecksum += (uint)br1.ReadByte();
                        if (readchecksum == checksum)
                        {
                            AddLogItem("Checksum matches! checksum = " + readchecksum.ToString("X08"));
                        }
                        else
                        {
                            AddLogItem("Checksum failed! read = " + readchecksum.ToString("X08") + " versus calculated = " + checksum.ToString("X08"));
                            if (checkBox1.Checked)
                            {
                                AddLogItem("Writing file " + textBox1.Text + "-modified.bin with modified checksum");
                                fsi1.Position = 0;
                                FileStream fs2 = new FileStream(textBox1.Text + "-modified.bin", FileMode.Create);
                                BinaryWriter bw1 = new BinaryWriter(fs2);
                                for (int t = 0; t < 0x0003FFFC; t++)
                                {
                                    bw1.Write(br1.ReadByte());
                                }
                                bw1.Write((byte)((checksum & 0xFF000000) >> 24));
                                bw1.Write((byte)((checksum & 0x00FF0000) >> 16));
                                bw1.Write((byte)((checksum & 0x0000FF00) >> 8));
                                bw1.Write((byte)((checksum & 0x000000FF)));
                                bw1.Close();
                                fs2.Close();
                            }
                        }
                        br1.Close();
                        fsi1.Close();
                    }
                    else
                    {
                        AddLogItem("End of data marker not found in file! Is this really a Trionic 5 file?");
                    }
                }
                else
                {
                    AddLogItem("File has incorrect length (<> 0x00040000)");
                }
            }
            else
            {
                AddLogItem("File doesn't seem to exist, please select a valid filename");
            }
            AddLogItem("Finished.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // try to brute force the checksums in the file
            button4.Enabled = true;
            int minimal_length = 256; // presume there will be no checksum on fields shorter then minimal_length bytes
            // repeat for every valid field length
            if (File.Exists(textBox1.Text))
            {
                AddLogItem("File exists, checking length");
                FileInfo fi = new FileInfo(textBox1.Text);
                FileStream fsi1 = File.OpenRead(textBox1.Text);
                BinaryReader br1 = new BinaryReader(fsi1);
                byte[] filebytes = File.ReadAllBytes(textBox1.Text);

                for (int field_length = minimal_length; field_length < fi.Length - 4; field_length++)
                {
                    AddLogItem("starting field length: " + field_length.ToString());
                    // try 32 bits checksum
                    fsi1.Position = 0; // start at beginning of the file
                    int file_index = 0;
                    // read the bytes and calculate the checksum
                    while (fsi1.Position <= fi.Length - field_length)
                    {
                        if (!button4.Enabled)
                        {
                            br1.Close();
                            fsi1.Close(); 
                            return;
                        }
                        uint sum_32 = 0;
                        for (int byte_tel = 0; byte_tel < field_length; byte_tel++)
                        {
                            sum_32 += br1.ReadByte();
                            file_index++;
                        }
                        // try to find the checkum in the file
                        if (sum_32 != 0xFFFFFFFF && sum_32 != 0xFFFF0000 && sum_32 != 0x00FF0000 && sum_32 != 0x000FF00 && sum_32 != 0xFF000000 && sum_32 != 0x000000FF && sum_32 != 0x0000FFFF && sum_32 != 0x00)
                        {
                            byte[] pattern_32 = new byte[4];
                            pattern_32.SetValue((byte)((sum_32 & 0xFF000000) >> 24), 0);
                            pattern_32.SetValue((byte)((sum_32 & 0x00FF0000) >> 16), 1);
                            pattern_32.SetValue((byte)((sum_32 & 0x0000FF00) >> 8), 2);
                            pattern_32.SetValue((byte)(sum_32 & 0x000000FF), 3);
                            int offset_32 = IndexOf(filebytes, pattern_32);
                            if (offset_32>0)
                            {
                                AddLogItem("Possible checksum found on field " + file_index.ToString("X08") + " length: " + field_length.ToString() + " checksum location: :" + offset_32.ToString("X08") + " sum: " + sum_32.ToString("X08"));
                            }
                        }
                    }

                    // try 16 bits checksum
                    fsi1.Position = 0; // start at beginning of the file

                    // try 8 bits checksum
                    fsi1.Position = 0; // start at beginning of the file

                }
                br1.Close();
                fsi1.Close();
            }
                 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }
    }
}
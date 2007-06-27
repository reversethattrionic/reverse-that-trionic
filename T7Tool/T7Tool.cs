using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace T7Tool
{
    public partial class T7Tool : Form
    {
        public T7Tool()
        {
            InitializeComponent();
        }



        private void chassisIDTextBox_TextChanged(object sender, EventArgs e)
        {
            t7InfoHeader.setChassisID(chassisIDTextBox.Text);
        }

     

        private void openFileDialog_FileOk_1(object sender, CancelEventArgs e)
        {
            m_fileName = openFileDialog.FileName;
            fileNameLabel.Text = m_fileName;
            t7InfoHeader.init(m_fileName);
            chassisIDTextBox.Text = t7InfoHeader.getChassisID();
            softwareVersionTextBox.Text = t7InfoHeader.getSoftwareVersion();
            carDescriptionTextBox.Text = t7InfoHeader.getCarDescription();
            imobilizerTextBox.Text = t7InfoHeader.getImmobilizerID();
            checksumF2TextBox.Text = "0x" + t7InfoHeader.getChecksumF2().ToString("X");
            checksumFBTextBox.Text = "0x" + t7InfoHeader.getChecksumFB().ToString("X");
            firmwareLengthTextBox.Text = "0x" + t7InfoHeader.getFWLength().ToString("X");
             
        }

        

        private void getIDButton_Click(object sender, EventArgs e)
        {

        }

  

        private void openFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }

        T7FileHeader t7InfoHeader = new T7FileHeader();
        string m_fileName;

        private void imobilizerTextBox_TextChanged(object sender, EventArgs e)
        {
            t7InfoHeader.setImmobilizerID(imobilizerTextBox.Text);
        }

        private void softwareVersionTextBox_TextChanged(object sender, EventArgs e)
        {
            t7InfoHeader.setSoftwareVersion(softwareVersionTextBox.Text);
        }

        private void carDescriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            t7InfoHeader.setCarDescription(carDescriptionTextBox.Text);
        }

        private void saveFileButton_Click(object sender, EventArgs e)
        {
            t7InfoHeader.save(m_fileName);
        }
    }
}
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
            openFile();             
        }

        

        private void getIDButton_Click(object sender, EventArgs e)
        {
            getIDFileDialog.ShowDialog();
        }

  

        private void openFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }


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

        private void getIDFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            string fileName = getIDFileDialog.FileName;
            T7FileHeader fileHeader = new T7FileHeader();
            fileHeader.init(fileName);
            chassisIDTextBox.Text = fileHeader.getChassisID();
            imobilizerTextBox.Text = fileHeader.getImmobilizerID();

        }

        T7FileHeader t7InfoHeader = new T7FileHeader();
        string m_fileName;

        private void saveFileAsbButton_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            string fileName = saveFileDialog.FileName;
            File.Copy(m_fileName, fileName, true);
            t7InfoHeader.save(fileName);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void fixChecksumButton_Click(object sender, EventArgs e)
        {
            ChecksumHandler csHandler = new ChecksumHandler();
            int fwLength = t7InfoHeader.getFWLength();
            int calculatedFWChecksum = csHandler.calculateFWChecksum(m_fileName);
            csHandler.setFWChecksum(m_fileName, calculatedFWChecksum);

            uint calculatedF2Checksum = csHandler.calculateF2Checksum(m_fileName, 0, fwLength);
            int calculatedFBChecksum = csHandler.calculateFBChecksum(m_fileName, 0, fwLength);
            t7InfoHeader.setChecksumF2((int)calculatedF2Checksum);
            t7InfoHeader.setChecksumFB(calculatedFBChecksum);

            t7InfoHeader.save(m_fileName);
            

            openFile();
        }

        private void openFile()
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

            ChecksumHandler csHandler = new ChecksumHandler();
            int fwChecksum = csHandler.getFWChecksum(m_fileName);
            int fwLength = t7InfoHeader.getFWLength();
            uint f2Checksum = (uint)t7InfoHeader.getChecksumF2();
            int fbChecksum = t7InfoHeader.getChecksumFB();
            checksumFWTextBox.Text = "0x" + fwChecksum.ToString("X");

            int calculatedFWChecksum = csHandler.calculateFWChecksum(m_fileName);
            uint calculatedF2Checksum = csHandler.calculateF2Checksum(m_fileName, 0, fwLength);
            int calculatedFBChecksum = csHandler.calculateFBChecksum(m_fileName, 0, fwLength);

            if (fwChecksum == calculatedFWChecksum)
                checksumFWTextBox.BackColor = Color.LightGreen;
            else
                checksumFWTextBox.BackColor = Color.Red;

            if (calculatedF2Checksum == f2Checksum)
                checksumF2TextBox.BackColor = Color.LightGreen;
            else
                checksumF2TextBox.BackColor = Color.Red;

            if (calculatedFBChecksum == fbChecksum)
                checksumFBTextBox.BackColor = Color.LightGreen;
            else
                checksumFBTextBox.BackColor = Color.Red;
        }

        private void aboutT7ToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
    }
}
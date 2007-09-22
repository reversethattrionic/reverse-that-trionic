using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace T7Tool
{
    class T7FileHeader
    {
        string m_chassisID = "";
        string m_immobilizerID = "";
        string m_softwareVersion = "";
        string m_carDescription = "";
        long fileLength;
        int m_checksumF2;
        int m_checksumFB;
        int m_fwLength;

        class FileHeaderField
        {
            public byte m_fieldID;
            public byte m_fieldLength;
            public byte[] m_data = new byte[255];
        }

        public bool save(string a_filename)
        {
            if (!File.Exists(a_filename))
                File.Create(a_filename);
            FileStream fs = new FileStream(a_filename, FileMode.Open, FileAccess.ReadWrite);
            FileHeaderField fhf;
            fs.Seek(0, SeekOrigin.End);
            long writePos;
            fileLength = fs.Position;
            do
            {
                writePos = fs.Position;
                fhf = readField(fs);
                switch (fhf.m_fieldID)
                {
                    case 0x90:  setHeaderString(fhf, m_chassisID);
                                fs.Position = writePos;
                                writeField(fs, fhf);
                                break;
                    case 0x97:  setHeaderString(fhf, m_carDescription);
                                fs.Position = writePos;
                                writeField(fs, fhf);
                                break;
                    case 0x95:  setHeaderString(fhf, m_softwareVersion);
                                fs.Position = writePos;
                                writeField(fs, fhf);
                                break;
                    case 0x92:  setHeaderString(fhf, m_immobilizerID);
                                fs.Position = writePos;
                                writeField(fs, fhf);
                                break;
                    case 0xFB:  setHeaderIntValue(fhf, m_checksumFB);
                                fs.Position = writePos;
                                writeField(fs, fhf);
                                break;
                    case 0xF2:  setHeaderIntValue(fhf, m_checksumF2);
                                fs.Position = writePos;
                                writeField(fs, fhf);
                                break;
                    case 0xFE:  setHeaderIntValue(fhf, m_fwLength);
                                fs.Position = writePos;
                                writeField(fs, fhf);
                                break;
                    default:
                                break;
                }   
            }
            while (fhf.m_fieldID != 0xFF && fhf.m_fieldID != 0xF9 );    // Don't write past 0xF9 "End of header"
            fs.Close();
            return true;
        }

        public bool init(string a_filename)
        {
            m_checksumF2 = 0;
            m_checksumFB = 0;
            m_chassisID = "";
            m_immobilizerID = "";
            m_softwareVersion = "";
            m_carDescription = "";

            if (!File.Exists(a_filename)) 
                return false;
            FileStream fs = new FileStream(a_filename, FileMode.Open, FileAccess.Read);
            FileHeaderField fhf;
            fs.Seek(0, SeekOrigin.End);
            fileLength = fs.Position;
            do
            {
                fhf = readField(fs);
                switch (fhf.m_fieldID)
                {
                    case 0x90:  m_chassisID = getHeaderString(fhf);
                                break;
                    case 0x97: m_carDescription = getHeaderString(fhf);
                                break;
                    case 0x95: m_softwareVersion = getHeaderString(fhf);
                                break;
                    case 0x92: m_immobilizerID = getHeaderString(fhf);
                                break;
                    case 0xFB: m_checksumFB = getHeaderIntValue(fhf);
                                break;
                    case 0xF2: m_checksumF2 = getHeaderIntValue(fhf);                                
                                break;
                    case 0xFE: m_fwLength = getHeaderIntValue(fhf);
                                break;
                    default:
                        break;
                }
            }
            while (fhf.m_fieldID != 0xFF && fhf.m_fieldID != 0xF9 );    // Don't read past 0xF9 "End of header"
            fs.Close();
            return true;
        }

        private string getHeaderString(FileHeaderField a_fileHeaderField)
        {
            Encoding ascii = Encoding.ASCII;
            ascii.GetChars(a_fileHeaderField.m_data, 0, a_fileHeaderField.m_fieldLength);
            return ascii.GetString(a_fileHeaderField.m_data, 0, a_fileHeaderField.m_fieldLength);
        }

        private void setHeaderString(FileHeaderField a_fileHeaderField, string a_string)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(a_string);
            a_fileHeaderField.m_data = bytes;
        }

        private int getHeaderIntValue(FileHeaderField a_fileHeaderField)
        {
            int intValue = 0;
            intValue |= a_fileHeaderField.m_data[0];
            intValue <<= 8;
            intValue |= a_fileHeaderField.m_data[1];
            intValue <<= 8;
            intValue |= a_fileHeaderField.m_data[2];
            intValue <<= 8;
            intValue |= a_fileHeaderField.m_data[3];
            return intValue;
        }

        private void setHeaderIntValue(FileHeaderField a_fileHeaderField, int a_value)
        {
            a_fileHeaderField.m_data[3] = (byte) a_value;
            a_value >>= 8;
            a_fileHeaderField.m_data[2] = (byte) a_value;
            a_value >>= 8;
            a_fileHeaderField.m_data[1] = (byte) a_value;
            a_value >>= 8;
            a_fileHeaderField.m_data[0] = (byte) a_value;
        }

        private FileHeaderField readField(FileStream a_fileStream)
        {
            FileHeaderField fhf = new FileHeaderField();
            a_fileStream.Position -= 1;
            fhf.m_fieldLength = (byte) a_fileStream.ReadByte();
            a_fileStream.Position -= 2;
            fhf.m_fieldID = (byte) a_fileStream.ReadByte();
            if (fhf.m_fieldID == 0xFF)
                return fhf;
            for (int i = 0; i < fhf.m_fieldLength; i++)
            {
                a_fileStream.Position -= 2;
                fhf.m_data[i] = (byte)a_fileStream.ReadByte();

            }
            a_fileStream.Position -= 1;
            return fhf;
        }

        private void writeField(FileStream a_fileStream, FileHeaderField a_fhf)
        {
            a_fileStream.Position -= 3;         // Skip ID and length
            for (int i = 0; i < a_fhf.m_fieldLength; i++)
            {
                a_fileStream.WriteByte(a_fhf.m_data[i]);
                a_fileStream.Position -= 2;

            }
            a_fileStream.Position += 1;
        }

        public string getChassisID()
        {
            return m_chassisID;
        }

        public void setChassisID(string a_string)
        {
             m_chassisID = a_string;
        }

        public string getImmobilizerID()
        {
            return m_immobilizerID;
        }

        public void setImmobilizerID(string a_string)
        {
            m_immobilizerID = a_string; 
        }

        public string getSoftwareVersion()
        {
            return m_softwareVersion;
        }

        public void setSoftwareVersion(string a_string)
        {
            m_softwareVersion = a_string;
        }

        public string getCarDescription()
        {
            return m_carDescription;
        }

        public void setCarDescription(string a_string)
        {
            m_carDescription = a_string;
        }

        public int getChecksumF2()
        {
            return m_checksumF2;
        }

        public void setChecksumF2(int a_checksum)
        {
            m_checksumF2 = a_checksum;
        }

        public void setChecksumFB(int a_checksum)
        {
            m_checksumFB = a_checksum;
        }

        public int getChecksumFB()
        {
            return m_checksumFB;
        }

        public int getFWLength()
        {
            return m_fwLength;
        }
    }



}

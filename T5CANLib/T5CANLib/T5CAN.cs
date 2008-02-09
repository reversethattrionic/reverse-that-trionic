using System;
using System.Collections.Generic;
using System.Text;
using T5CANLib.CAN;

namespace T5CANLib
{
    public class T5CAN
    {
        private ICANDevice m_canDevice;
        private CANListener m_canListener;
        private string RESET = "#";
        private string CR = "\r";
        private string NL = "\n";
        private string WAKE_UP_COMMAND = "\r";
        private string SOFTWARE_VERSION_COMMAND = "s";
        private string PROMPT = ">";

        public void setCANDevice(ICANDevice a_device)
        {
            m_canDevice = a_device;
            if(m_canListener == null)
                m_canListener = new CANListener();
            m_canDevice.addListener(m_canListener);
        }

        public bool openDevice(out string r_swVersion)
        {
            r_swVersion = "";
            string str = "";
            m_canListener.setWaitMessageID(0x00C);
            string expectedResponse1 = ">AB" + CR + NL;
            string expectedResponse2 = ">A";
            string expectedResponse3 = "B>" + CR;
            if(m_canDevice.open() != OpenResult.OK)
                return false;
            str = sendCommand(CR, CR+NL);
            if (str.Length == 15)           //In synch. SW version is returned with size 15 (including >, CR and NL)
            {
                r_swVersion = trimString(str);
                return true;
            }
            if (str.Equals(expectedResponse1))
            {
                str = sendCommand(CR, expectedResponse2);
                if (str.Equals(expectedResponse2))
                {
                    str = sendCommand(SOFTWARE_VERSION_COMMAND, CR);
                    if (str.Equals(expectedResponse3))
                    {
                        str = sendCommand(CR, CR + NL);
                        r_swVersion = trimString(str);
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
            {
                findSynch();
            }

            str = sendCommand(CR, CR + NL);
            if (str.Length == 15)           //In synch. SW version is returned with size 15 (including >, CR and NL)
            {
                r_swVersion = trimString(str);
                return true;
            }
            if (str.Equals(expectedResponse1))
            {
                str = sendCommand(CR, expectedResponse2);
                if (str.Equals(expectedResponse2))
                {
                    str = sendCommand(SOFTWARE_VERSION_COMMAND, CR);
                    if (str.Equals(expectedResponse3))
                    {
                        str = sendCommand(CR, CR + NL);
                        r_swVersion = trimString(str);
                    }
                    else
                        return false;
                }
                else
                    return false;
            }

            return true;
        }

        public bool getSymbolTable(out string r_symbolTable)
        {
            System.Console.WriteLine("###### Downloading symbol table ######");
            string str = "";
            str = sendCommand(CR, 2);
            str = sendCommand("S", 3);
            r_symbolTable = sendCommand(CR, "END"+CR+NL);
            return true;
        }


        private string sendCommand(string a_command)
        {
            string retString = "";
            sendCommandByte(a_command[0]);
            retString = waitForResponse();
            return retString;
        }

        private string sendCommand(string a_command, uint a_nrOfBytes)
        {
            string retString = "";
            sendCommandByte(a_command[0]);
            for (int i = 0; i < a_nrOfBytes; i++ )
            {
                retString += waitForResponse();
            }
            return retString;
        }

        private string sendCommand(string a_command, string a_endChar)
        {
            string retString = "";
            sendCommandByte(a_command[0]);
            string recChar = "";
            do
            {
                recChar = waitForResponse();
                System.Console.Write(recChar);
                retString += recChar;
            }
            while (!retString.EndsWith(a_endChar));
            return retString;
        }

        private void sendCommandByte(char a_commandByte)
        {
            System.Console.WriteLine(a_commandByte);
            CANMessage msg = new CANMessage(0x005, 0, 8);
            ulong cmd = 0x0000000000000000;
            cmd |= (ulong)a_commandByte;
            cmd <<= 8;
            cmd |= (ulong)0xC4;
            cmd |= 0xFFFFFFFFFFFF0000;
            msg.setData(cmd);
            if (!m_canDevice.sendMessage(msg))
                throw new Exception("Couldn't send message");
            
        }

        private string waitForResponse()
        {
            string returnString = "";
            CANMessage response = new CANMessage();
            response = m_canListener.waitForMessage(0x00C, 1000);
            if ((byte)response.getData() != 0xC6)
                throw new Exception("Error receiveing data");
            if (response.getLength() < 8)
            {
                returnString = "";
                return returnString;
            }
            returnString += (char)(response.getData() >> 16);
            sendAck(response);
            return returnString;
        }

        private void sendAck(CANMessage a_msg)
        {
            CANMessage ack =  new CANMessage(0x006, 0, 2);
            ack.setData(0x00000000000000C6);
            if (!m_canDevice.sendMessage(ack))
                throw new Exception("Couldn't send message");
        }


        private void findSynch()
        {
            System.Console.WriteLine("###### Looking for synch ######");
            string str = "";
            char ch;
            CANMessage ack = new CANMessage(0x006, 0, 2);
            CANMessage response = new CANMessage();
            ack.setData(0x00000000000000C6);
            do
            {
                if (!m_canDevice.sendMessage(ack))
                    throw new Exception("Couldn't send message");
                response = m_canListener.waitForMessage(0x00C, 1000);
                if ((byte)response.getData() != 0xC6)
                    throw new Exception("Error receiveing data");
                ch = (char)(response.getData() >> 16);
                System.Console.Write(ch);
                str += ch;
            }
            while (!str.EndsWith("END" + CR + NL));
        }

        string trimString(string a_string)
        {
            a_string = a_string.TrimStart('>');
            a_string = a_string.TrimEnd('\n');
            a_string = a_string.TrimEnd('\r');
            return a_string;
        }
    }
}

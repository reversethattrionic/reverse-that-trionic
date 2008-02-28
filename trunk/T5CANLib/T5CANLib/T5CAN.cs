using System;
using System.Collections.Generic;
using System.Text;
using T5CANLib.CAN;
using System.Threading;

namespace T5CANLib
{
    public class T5CAN
    {
        private ICANDevice m_canDevice;
        private CANListener m_canListener;
        private string CR = "\r";
        private string NL = "\n";
        private string TIMEOUT = "timeout";

        public void setCANDevice(ICANDevice a_device)
        {
            m_canDevice = a_device;
            if(m_canListener == null)
                m_canListener = new CANListener();
            m_canDevice.addListener(m_canListener);
            m_canListener.setCANDevice(m_canDevice);
        }

        public bool openDevice(out string r_swVersion)
        {
            r_swVersion = "";
            string str = "";
            string symbolTable = "";
            m_canListener.setWaitMessageID(0x00C);
            if(m_canDevice.open() != OpenResult.OK)
                return false;
            initialize();


            r_swVersion = sendCRNLCommand("s");

            symbolTable = sendCRNLCommand("S", "END" + CR + NL);
            
    /*        str = sendCommand(CR, ">A");
            str = sendCommand("A", 3);
            str = sendCommand("6", 4);
            str = sendCommand("5", 5);
            str = sendCommand("D", 6);
            str = sendCommand("0", 7);
            str = sendCommand("0", 8);
            str = sendCommand("0", 9);
            str = sendCommand("0", 10);
            str = sendCommand("E", 11);
            str = sendCommand(CR, 6);
            str = sendCommand("B", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand(CR, ">");
            str = sendCommand("R", ">");
            str = sendCommand("R", ">");
            str = sendCommand(CR, CR+NL);       
            str = sendCommand(CR, 2);
            str = sendCommand("A", 3);
            str = sendCommand("6", 4);
            str = sendCommand("5", 5);
            str = sendCommand("D", 6);
            str = sendCommand("E", 7);
            str = sendCommand("0", 8);
            str = sendCommand("0", CR+NL);
            str = sendCommand("6", ">");
            str = sendCommand("4", ">");
            str = sendCommand(CR, ">");
            str = sendCommand("B", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand(CR, ">");
            str = sendCommand("R", ">");
            str = sendCommand("R", ">");
            str = sendCommand(CR, CR+NL);
            str = sendCommand(CR, 2);
            str = sendCommand("W", 3);
            str = sendCommand("6", 4);
            str = sendCommand("6", 5);
            str = sendCommand("4", 6);
            str = sendCommand("2", 7);
            str = sendCommand("5", 8);
            str = sendCommand("5", 9);
            str = sendCommand(CR, 7);
            str = sendCommand(CR, ">");
            str = sendCommand("A", ">");
            str = sendCommand("6", ">");
            str = sendCommand("6", ">");
            str = sendCommand("4", ">");
            str = sendCommand("2", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("1", ">");
            str = sendCommand(CR, ">");
            str = sendCommand("B", ">");*/
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand("0", ">");
            str = sendCommand(CR, ">");
            str = sendCommand("R", ">");
            str = sendCommand("R", ">");
            str = sendCommand(CR, "AB"+CR+NL);
            
            return true;
        }

        public bool initialize()
        {
            string str = "";
            str = sendCommand(CR, CR + NL);
            if (str != ">AB" + CR + NL)
            {
                if (str.Equals(">"))
                {
                    str = sendCommand(CR, ">");
                    if (!str.Equals(">"))
                        return false;
                }
                else
                {
                    str = trimString(str);
                    if (str.Length != 12)            //Check if received string is SW nr.
                        findSynch();
                }

            }
            Thread.Sleep(500);
            return true;
        }

        public string getSWVersion()
        {
            string str = "";
            string tmpStr = "";


            string sendString = CR + "s" + CR;
            int i = 0;
            sendCommandByte(sendString[i++]);
            tmpStr = waitNoAck();
            do
            {
                if (tmpStr.Equals(">") && (i < sendString.Length))
                {
                    sendCommandByte(sendString[i++]);
                    sendAck();
                }
                else
                    sendAck();
                str += tmpStr;
                tmpStr = waitNoAck();
                if (tmpStr.Equals("timeout"))
                {
                    sendAck();
                    tmpStr = "";
                }
            } while (!str.EndsWith(CR + NL));



        /*    str = sendStartCRNLCommand(CR);
            str = sendCRNLCommand("s", 3);     
            str = sendEndCRNLCommand(CR);
            str = trimString(str);*/
            return str;
        }


        private string sendStartCRNLCommand(string a_command)
        {
            string retString = "";
            sendCommandByte(a_command[0]);
            retString = waitForResponse();
            retString = waitNoAck();
            return retString;
        }

        private string sendCRNLCommand(string a_command)
        {
            return sendCRNLCommand(a_command, CR + NL);
        }
        private string sendCRNLCommand(string a_command, string a_endString)
        {
            string retString = "";
            string str = "";
            str += sendCommand(CR, ">");
            str += waitNoAck();
            str += waitNoAck(13);
            if (!str.EndsWith(TIMEOUT))
                return TIMEOUT;
            sendCommandByte(a_command[0]);
            sendAck();
            str += waitNoAck();
            str += waitNoAck();
            sendAck();
            str += waitNoAck();
            sendCommandByte(CR[0]);
            sendAck();
            str += waitNoAck();
            str += waitNoAck();
            retString += receiveResponse(a_endString);
            sendAck();
            sendAck();
            return retString;
        }

        private string sendEndCRNLCommand(string a_command)
        {
            string retString = "";
            sendCommandByte(a_command[0]);
            sendAck();
            do
            {
                retString += waitForResponse();
            } while (!retString.EndsWith(CR+NL));
            return retString;
        }

        public bool getSymbolTable(out string r_symbolTable)
        {
            System.Console.WriteLine("###### Downloading symbol table ######");
            r_symbolTable = sendCRNLCommand("S", "END");
            return true;
        }


        private string sendCommand(string a_command)
        {
            string retString = "";
            sendCommandByte(a_command[0]);
            retString = waitForResponse();
            //Thread.Sleep(100);
            return retString;
        }

        private void sendNoAck(string a_command)
        {
            sendCommandByte(a_command[0]);
        }

        private string sendCommand(string a_command, uint a_nrOfBytes)
        {
            string retString = "";
            string str = "";
            sendCommandByte(a_command[0]);
            for (int i = 0; i < a_nrOfBytes; i++ )
            {
                str = waitForResponse();
                if (str.Equals(TIMEOUT))
                {
                    System.Console.Write(" TIMEOUT ");
                    return retString;
                }
                retString += str;
            }
            //Thread.Sleep(100);
            return retString;
        }

        private string receiveResponse(string a_endChar)
        {
            string retString = "";
            sendAck();
            string recChar = "";
            do
            {
                recChar = waitForResponse();
                if (recChar.Equals(TIMEOUT))
                {
                    System.Console.Write(" TIMEOUT ");
                        return retString;
                }
                retString += recChar;
                if (!a_endChar.EndsWith(CR + NL))      //Not variable length.                
                {
                    if (retString.Length > a_endChar.Length)
                    {
                        return retString;
                    }
                }
            }
            while (!retString.EndsWith(a_endChar));
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
                if (recChar.Equals(TIMEOUT))                
                {                    
                    System.Console.Write(" TIMEOUT ");                    
                    return retString;                
                }                
                retString += recChar;                
                if(!a_endChar.EndsWith(CR+NL))      //Not variable length.                
                {                    
                    if (retString.Length > a_endChar.Length)                    
                    {                                               
                        return retString;                    
                    }                
                }            
            }            
            while (!retString.EndsWith(a_endChar));                        
            return retString;        
        } 

        private void sendCommandByte(char a_commandByte)
        {
            //Thread.Sleep(100);
            string str = "";
            if(a_commandByte == 0x0D)
                str = "CR";
            if(a_commandByte == 0x0A)
                str = "NL";
            System.Console.Write("\nCommand: " + str + a_commandByte);
            System.Console.Write("\nResponse:");
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
            bool timeout = false;
            CANMessage response = new CANMessage();
            response = m_canListener.waitForMessage(0x00C, 1000, out timeout);
            if (timeout)
            {
                return TIMEOUT;
            }
            if ((byte)response.getData() != 0xC6)
                throw new Exception("Error receiveing data");
            if (response.getLength() < 8)
            {
                returnString = "";
                return returnString;
            }
            returnString += (char)(response.getData() >> 16);
            if(returnString.Equals(CR))
                System.Console.Write(" CR");
            else if(returnString.Equals(NL))
                System.Console.Write(" NL\n");
            else
                System.Console.Write(returnString);
            sendAck();
            return returnString;
        }

        private string waitNoAck()
        {
            return waitNoAck(1000);
        }

        private string waitNoAck(uint a_timeout)
        {
            string returnString = "";
            bool timeout = false;
            CANMessage response = new CANMessage();
            response = m_canListener.waitForMessage(0x00C, (int)a_timeout, out timeout);
            if (timeout)
            {
                return TIMEOUT;
            }
            if ((byte)response.getData() != 0xC6)
                throw new Exception("Error receiveing data");
            returnString += (char)(response.getData() >> 16);
            if (returnString.Equals(CR))
                System.Console.Write(" CR");
            else if (returnString.Equals(NL))
                System.Console.Write(" NL\n");
            else
                System.Console.Write(returnString);
            return returnString;
        }

        private void sendAck()
        {
            if (m_canDevice == null)
                throw new Exception("CAN device not set");
            CANMessage ack = new CANMessage(0x006, 0, 2);
            ack.setData(0x00000000000000C6);
            if (!m_canDevice.sendMessage(ack))
                throw new Exception("Couldn't send message");
        }

        private void findSynch()
        {
            System.Console.WriteLine("###### Looking for synch ######");
            string str = "";
            char ch;
            bool timeout = false;
            CANMessage ack = new CANMessage(0x006, 0, 2);
            CANMessage response = new CANMessage();
            ack.setData(0x00000000000000C6);
            do
            {
                if (!m_canDevice.sendMessage(ack))
                    throw new Exception("Couldn't send message");
                response = m_canListener.waitForMessage(0x00C, 1000, out timeout);
                if (timeout)
                {
                    return;
                }
                if ((byte)response.getData() != 0xC6)
                    throw new Exception("Error receiveing data");
                ch = (char)(response.getData() >> 16);
                System.Console.Write(ch);
                str += ch;
                if (str.EndsWith(CR + NL + NL + NL))
                {
                    return;
                }
                if (str.EndsWith(NL + NL + NL + NL))
                    str = sendCommand(CR, 1);
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

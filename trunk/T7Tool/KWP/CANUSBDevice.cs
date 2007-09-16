using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace T7Tool.KWP
{

    class CANUSBDevice : IKWPDevice
    {

        static uint m_deviceHandle = 0;
        static uint timeoutPeriod = 1000;

        public OpenResult open()
        {
            LAWICEL.CANMsg msg = new LAWICEL.CANMsg();
            //Check if P bus is connected
            m_deviceHandle = LAWICEL.canusb_Open(IntPtr.Zero,
            LAWICEL.CAN_BAUD_500K,
            LAWICEL.CANUSB_ACCEPTANCE_CODE_ALL,
            LAWICEL.CANUSB_ACCEPTANCE_MASK_ALL,
            LAWICEL.CANUSB_FLAG_TIMESTAMP);
            if (waitForMessage(0x1A0, 5000, out msg) != 0)
                return OpenResult.OK;

            //P bus not connected
            //Check if I bus is connected
            m_deviceHandle = LAWICEL.canusb_Open(IntPtr.Zero,
                "0xcb:0x9a",
                LAWICEL.CANUSB_ACCEPTANCE_CODE_ALL,
                LAWICEL.CANUSB_ACCEPTANCE_MASK_ALL,
                LAWICEL.CANUSB_FLAG_TIMESTAMP);
            if(waitForMessage(0x1A0, 5000, out msg) != 0)
                return OpenResult.OK;
            int res = LAWICEL.canusb_Close(m_deviceHandle);

            return OpenResult.OpenError;
        }

        public CloseResult close()
        {
            int res = LAWICEL.canusb_Close(m_deviceHandle);
            m_deviceHandle = 0;
            if (LAWICEL.ERROR_CANUSB_OK == res)
            {
                return CloseResult.OK;
            }
            else
            {
                return CloseResult.CloseError;
            }
        }

        public bool isOpen()
        {
            if (m_deviceHandle > 0)
                return true;
            else
                return false;
        }

        public bool startSession()
        {
            LAWICEL.CANMsg msg = new LAWICEL.CANMsg();
            msg.id = 0x220;
            msg.len = 8;
            msg.flags = 0;
            msg.data = 0x000040021100813F;
            int writeResult = LAWICEL.canusb_Write(m_deviceHandle, ref msg);
            if (writeResult != LAWICEL.ERROR_CANUSB_OK)
            {
                return false;
            }
            if (waitForMessage(0x238, timeoutPeriod, out msg) == 0x238)
                return true;
            else
                return false;
        }


        public RequestResult sendRequest(KWPRequest a_request, out KWPReply r_reply)
        {
            LAWICEL.CANMsg msg = new LAWICEL.CANMsg();
            int writeResult;
            uint row;
            msg.id = 0x240;
            msg.len = 8;
            msg.flags = 0;
            for(row = 0; row < nrOfRowsToSend(a_request.getData()); row++)
            {
                msg.data = createCanMessage(a_request.getData(), row);
                writeResult = LAWICEL.canusb_Write(m_deviceHandle, ref msg);
                if (writeResult != LAWICEL.ERROR_CANUSB_OK)
                {
                    r_reply = new KWPReply();
                    return RequestResult.ErrorSending;
                }
            }

            if (waitForMessage(0x258, timeoutPeriod, out msg) == 0x258)
            {
                uint nrOfRows = nrOfRowsToRead(msg.data);
                row = 0;
                if(nrOfRows == 0)
                    throw new Exception("Wrong nr of rows");
                byte[] reply = new byte[nrOfRows * 6];
                reply = collectReply(reply, msg.data, row);
                sendAck(nrOfRows - 1);
                nrOfRows--;
                while (nrOfRows > 0)
                {
                    if (waitForMessage(0x258, timeoutPeriod, out msg) == 0x258)
                    {
                        row++;
                        reply = collectReply(reply, msg.data, row);
                        sendAck(nrOfRows - 1);
                        nrOfRows--;
                    }
                    else
                    {
                        r_reply = new KWPReply();
                        return RequestResult.Timeout;
                    }

                }
                r_reply = new KWPReply(reply, a_request.getNrOfPID());
                return RequestResult.NoError;
            }
            else
            {
                r_reply = new KWPReply();
                return RequestResult.Timeout;
            }
        }

        private void sendAck(uint a_rowNr)
        {
            LAWICEL.CANMsg msg = new LAWICEL.CANMsg();
            uint i = 0;
            msg.id = 0x266;
            msg.len = 8;
            msg.flags = 0;
            msg.data = setCanData(msg.data, (byte)0x40, i++);
            msg.data = setCanData(msg.data, (byte)0xA1, i++);
            msg.data = setCanData(msg.data, (byte)0x3F, i++);
            msg.data = setCanData(msg.data, (byte)(0x80 | (int)(a_rowNr)), i++);
            int writeResult;
            writeResult = LAWICEL.canusb_Write(m_deviceHandle, ref msg);
            if (writeResult != LAWICEL.ERROR_CANUSB_OK)
                throw new Exception("Error sending ack");

        }

        private ulong setCanData(ulong a_canData, byte a_byte, uint a_index)
        {
            if (a_index > 7)
                throw new Exception("Index out of range");
            ulong tmp = (ulong)a_byte;
            tmp = tmp << (int)(a_index * 8);
            return a_canData | tmp;
        }

        private byte getCanData(ulong a_canmsg, uint a_nr)
        {
            return (byte)(a_canmsg >> (int)(a_nr * 8));
        }

        private uint nrOfRowsToSend(byte[] a_data)
        {
            return (uint)(a_data[0] / 6) + 1;
        }

        private uint nrOfRowsToRead(ulong a_canMsg)
        {
            uint nrOfRows = 0;
            nrOfRows = (uint)a_canMsg & 0x0F;
            nrOfRows++;
            return nrOfRows;
        }

        private byte[] collectReply(byte[] a_data, ulong a_canMsg, uint a_row)
        {
            uint j;
            if (a_row == 0)
                j = 0;
            else
                j = (a_row * 6) - 1;
            for (uint i = 2; i < 8; i++, j++)
                a_data[j] = getCanData(a_canMsg, i);
            return a_data;
        }

        private ulong createCanMessage(byte[] a_data, uint a_row)
        {
            if (a_row > nrOfRowsToSend(a_data) - 1)
                throw new Exception("Message nr out of index");
            ulong result = 0;
            uint i = 0;
            result = setCanData(result, (byte)(0x40 | a_row), i++);
            result = setCanData(result, (byte)0xA1, i++);
            uint j;
            if (a_row == 0)
                j = 0;
            else
                j = (a_row * 6) - 1;
            for (; j < a_data.Length; i++, j++)
                result = setCanData(result, (byte)a_data[j], i);
            return result;
        }

        public uint waitForMessage(uint a_canID, uint timeout, out LAWICEL.CANMsg r_canMsg)
        {
            int readResult = 0;
            int nrOfWait = 0;
            while (nrOfWait < timeout)
            {
                readResult = LAWICEL.canusb_Read(m_deviceHandle, out r_canMsg);
                if (readResult == LAWICEL.ERROR_CANUSB_OK)
                {
                    if (r_canMsg.id != a_canID)
                        continue;
                    return (uint)r_canMsg.id; 
                }
                else if (readResult == LAWICEL.ERROR_CANUSB_NO_MESSAGE)
                {
                    Thread.Sleep(1);
                    nrOfWait++;
                }
            }
            r_canMsg = new LAWICEL.CANMsg(); 
            return 0;
        }

       
    }


           
}

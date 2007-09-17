using System;
using System.Collections.Generic;
using System.Text;
using T7Tool.CAN;

namespace T7Tool.KWP
{
    class KWPCANDevice : IKWPDevice
    {
        private Object m_lockObject = new Object();
        private ICANDevice m_canDevice;
        KWPCANListener m_kwpCanListener = new KWPCANListener();
        const int timeoutPeriod = 1000;

        public void setCANDevice(ICANDevice a_canDevice)
        {
            lock (m_lockObject)
            {
                m_canDevice = a_canDevice;
            }
        }

        public bool open()
        {
            bool retVal = false;
            lock (m_lockObject)
            {
                if (m_canDevice.open() == OpenResult.OK)
                {
                    m_canDevice.addListener(m_kwpCanListener);
                    retVal = true;
                }
                else
                    retVal = false;
            }
            return retVal;
        }

        public bool isOpen()
        {
            bool retVal = false;
            lock (m_lockObject)
            {
                if (m_canDevice.isOpen())
                    retVal = true;
                else
                    retVal = false;
            }
            return retVal;
        }

        public bool close()
        {
            bool retVal = false;
            lock (m_lockObject)
            {
                if (m_canDevice.close() == CloseResult.OK)
                    retVal = true;
                else
                    retVal = false;
                m_canDevice.removeListener(m_kwpCanListener);
            }
            return retVal;
        }

        public bool startSession()
        {
            CANMessage msg = new CANMessage(0x220, 0, 8);
            msg.setData(0x000040021100813F);

            if (!m_canDevice.sendMessage(msg)) 
            {
                return false;
            }
            if (m_kwpCanListener.waitForMessage(0x238, timeoutPeriod).getID() == 0x238)
                return true;
            else
                return false;
        }

        public RequestResult sendRequest(KWPRequest a_request, out KWPReply r_reply)
        {
            CANMessage msg = new CANMessage(0x240, 0, 8);
            uint row;
            for (row = 0; row < nrOfRowsToSend(a_request.getData()); row++)
            {
                msg.setData(createCanMessage(a_request.getData(), row));
                if (!m_canDevice.sendMessage(msg))
                {
                    r_reply = new KWPReply();
                    return RequestResult.ErrorSending;
                }
            }
            msg = m_kwpCanListener.waitForMessage(0x258, timeoutPeriod);
            if (msg.getID() == 0x258)
            {
                uint nrOfRows = nrOfRowsToRead(msg.getData());
                row = 0;
                if (nrOfRows == 0)
                    throw new Exception("Wrong nr of rows");
                byte[] reply = new byte[nrOfRows * 6];
                reply = collectReply(reply, msg.getData(), row);
                sendAck(nrOfRows - 1);
                nrOfRows--;
                while (nrOfRows > 0)
                {
                    msg = m_kwpCanListener.waitForMessage(0x258, timeoutPeriod);
                    if (msg.getID() == 0x258)
                    {
                        row++;
                        reply = collectReply(reply, msg.getData(), row);
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

        private void sendAck(uint a_rowNr)
        {
            CANMessage msg = new CANMessage(0x266,0,8);
            uint i = 0;
            ulong data = 0;
            data = setCanData(data, (byte)0x40, i++);
            data = setCanData(data, (byte)0xA1, i++);
            data = setCanData(data, (byte)0x3F, i++);
            data = setCanData(data, (byte)(0x80 | (int)(a_rowNr)), i++);
            msg.setData(data);
            if (!m_canDevice.sendMessage(msg))
                throw new Exception("Error sending ack");

        }
    }
}

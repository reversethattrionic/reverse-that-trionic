using System;
using System.Collections.Generic;
using System.Text;

namespace T7Tool.KWP
{
    class KWPReply
    {
        byte[] m_reply;
        uint m_nrOfPid;

        public KWPReply(byte[] a_reply, uint a_nrOfPid)
        {
            if (a_nrOfPid > 2)
                throw new Exception("Nr of PID out of range");
            m_reply = a_reply;
            m_nrOfPid = a_nrOfPid;
        }

        public KWPReply()
        {
        }

        public void setData(byte[] a_data)
        {
            m_reply = a_data;
        }

        public void setNrOfPID(uint a_nr) { m_nrOfPid = a_nr; }
        public uint getNrOfPID() { return m_nrOfPid; }

        public byte getLength()
        {
            return m_reply[0];
        }

        public byte getMode()
        {
            return m_reply[1];
        }

        public byte getPid()
        {
            return m_reply[2];
        }

        public byte getPidHigh()
        {
            return m_reply[2];
        }

        public byte getPidLow()
        {
            return m_reply[3];
        }

        public byte[] getData()
        {
            byte[] data = new byte[getLength() - m_nrOfPid - 1];
            uint i;
            if (m_nrOfPid == 1)
                i = 3;
            else
                i = 4;
            for (uint j = 0; j < data.Length; i++, j++)
                data[j] = m_reply[i];
            return data;
        }
    }
}

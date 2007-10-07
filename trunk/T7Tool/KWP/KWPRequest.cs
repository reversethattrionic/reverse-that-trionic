using System;
using System.Collections.Generic;
using System.Text;

namespace T7Tool.KWP
{
    class KWPRequest
    {

        byte[] m_request;
        uint m_nrOfPid;

        public uint getNrOfPID() { return m_nrOfPid; }
        public KWPRequest(byte a_mode, byte a_pid)
        {
            int i = 0;
            byte length = 2;
            m_request = new byte[length + 1];
            m_request[i++] = length;
            m_request[i++] = a_mode;
            m_request[i++] = a_pid;
            m_nrOfPid = 1;
        }

        public KWPRequest(byte a_mode, byte[] a_data)
        {
            int i = 0;
            byte length = (byte)(1 + a_data.Length);
            m_request = new byte[length + 1];
            //Set length of request
            m_request[i++] = length;
            m_request[i++] = a_mode;
            for (int j = 0; i < m_request.Length; i++, j++)
                m_request[i] = a_data[j];
            m_nrOfPid = 0;
        }

        public KWPRequest(byte a_mode, byte a_pid, byte[] a_data)
        {
            int i = 0;
            byte length = (byte)(2 + a_data.Length);
            m_request = new byte[length + 1];
            //Set length of request
            m_request[i++] = length;
            m_request[i++] = a_mode;
            m_request[i++] = a_pid;
            for (int j = 0; i < m_request.Length; i++, j++)
                m_request[i] = a_data[j];
            m_nrOfPid = 1;
        }

        public KWPRequest(byte a_mode, byte a_pidHigh, byte a_pidLow, byte[] a_data)
        {
            int i = 0;
            byte length = (byte)(3 + a_data.Length);
            m_request = new byte[length + 1];
            m_request[i++] = length;
            m_request[i++] = a_mode;
            m_request[i++] = a_pidHigh;
            m_request[i++] = a_pidLow;
            for (int j = 0; i < m_request.Length; i++, j++)
                m_request[i] = a_data[j];
            m_nrOfPid = 2;
        }

        public byte[] getData() { return m_request; }
    }
}

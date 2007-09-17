using System;
using System.Collections.Generic;
using System.Text;

namespace T7Tool.CAN
{
    class CANMessage
    {

            private uint m_id;			// 11/29 bit Identifier
            private uint m_timestamp;   // Hardware Timestamp (0-9999mS)
            private byte m_flags;		// Message Flags
            private byte m_length;		// Number of data bytes 0-8
            private ulong m_data;		// Data Bytes 0..7

        public CANMessage(uint a_id, uint a_timestamp, byte a_flags, byte a_length, ulong a_data)
        {
            m_id = a_id;
            m_timestamp = a_timestamp;
            m_flags = a_flags;
            m_length = a_length;
            m_data = a_data;
        }

        public CANMessage(uint a_id, byte a_flags, byte a_length)
        {
            m_id = a_id;
            m_timestamp = 0;
            m_flags = a_flags;
            m_length = a_length;
            m_data = 0;
        }

        public CANMessage()
        {
            m_id = 0;
            m_timestamp = 0;
            m_flags = 0;
            m_length = 0;
            m_data = 0;
        }

        public uint getID() { return m_id; }
        public uint getTimeStamp() { return m_timestamp; }
        public byte getFlags() { return m_flags; }
        public byte getLength() { return m_length; }
        public ulong getData() { return m_data; }
        public void setID(uint a_id) { m_id = a_id; }
        public void setTimeStamp(uint a_timeStamp) { m_timestamp = a_timeStamp; }
        public void setFlags(byte a_flags) { m_flags = a_flags; }
        public void setLength(byte a_length) { m_length = a_length; }
        public void setData(ulong a_data) { m_data = a_data; }

        public void setCanData(byte a_byte, uint a_index)
        {
            if (a_index > 7)
                throw new Exception("Index out of range");
            ulong tmp = (ulong)a_byte;
            tmp = tmp << (int)(a_index * 8);
            m_data = m_data | tmp;
        }

        public byte getCanData(uint a_index)
        {
            return (byte)(m_data >> (int)(a_index * 8));
        }

    }
}

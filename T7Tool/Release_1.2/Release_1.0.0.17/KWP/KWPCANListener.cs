using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using T7Tool.CAN;

namespace T7Tool.KWP
{
    /// <summary>
    /// KWPCANListener is used by the KWPCANDevice for listening for CAN messages.
    /// </summary>
    class KWPCANListener : ICANListener
    {
        private CANMessage m_canMessage = new CANMessage();
        private uint m_waitMsgID = 0;
        private AutoResetEvent m_resetEvent = new AutoResetEvent(false);

        public CANMessage waitForMessage(uint a_canID, int a_timeout)
        {
            CANMessage retMsg;
            lock (m_canMessage)
            {
                m_waitMsgID = a_canID;
            }
            m_resetEvent.WaitOne(a_timeout, true);
            lock (m_canMessage)
            {
                retMsg = m_canMessage;
            }

            return retMsg;
        }

        override public void handleMessage(CANMessage a_message)
        {
            lock (m_canMessage)
            {
                if (a_message.getID() == m_waitMsgID)
                {
                    m_canMessage.setData(a_message.getData());
                    m_canMessage.setFlags(a_message.getFlags());
                    m_canMessage.setID(a_message.getID());
                    m_canMessage.setLength(a_message.getLength());
                    m_canMessage.setTimeStamp(a_message.getTimeStamp());
                    m_resetEvent.Set();
                }
            }
        }
    }
}

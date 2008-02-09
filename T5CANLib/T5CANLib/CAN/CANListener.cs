using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace T5CANLib.CAN
{
    /// <summary>
    /// CANListener is used by the CANDevice for listening for CAN messages.
    /// </summary>
    class CANListener : ICANListener
    {
        private CANMessage m_canMessage = new CANMessage();
        private uint m_waitMsgID = 0;
        private AutoResetEvent m_resetEvent = new AutoResetEvent(false);

        public void setWaitMessageID(uint a_canID)
        {
            lock (m_canMessage)
            {
                m_waitMsgID = a_canID;
            }
        }


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
                else
                {
                    m_canMessage.setData(0);
                    m_canMessage.setFlags(0);
                    m_canMessage.setID(0);
                    m_canMessage.setLength(0);
                    m_canMessage.setTimeStamp(0);
                }
            }
        }
    }
}

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
        private Queue<CANMessage> m_msgQueue = new Queue<CANMessage>();
        private uint m_waitMsgID = 0;
        private AutoResetEvent m_resetEvent = new AutoResetEvent(false);
        private ICANDevice m_canDevice = null;

        public void setWaitMessageID(uint a_canID)
        {
            lock (m_msgQueue)
            {
                m_waitMsgID = a_canID;
            }
        }

        public void setCANDevice(ICANDevice a_canDevice)
        {
            lock (m_msgQueue)
            {
                m_canDevice = a_canDevice;
            }
        }

        public CANMessage waitForMessage(uint a_canID, int a_timeout, out bool r_timeout)
        {
            r_timeout = false;
            CANMessage retMsg = new CANMessage();
            if (!m_resetEvent.WaitOne(a_timeout, true))
            {
                r_timeout = true;
                return retMsg;
            }
            lock (m_msgQueue)
            {
                retMsg = m_msgQueue.Dequeue(); 
            }
            
            return retMsg;
        }

        private void sendAck()
        {
            CANMessage ack = new CANMessage(0x006, 0, 2);
            ack.setData(0x00000000000000C6);
            if (!m_canDevice.sendMessage(ack))
                throw new Exception("Couldn't send message");
        }

        override public void handleMessage(CANMessage a_message)
        {
            CANMessage canMessage = new CANMessage();
            lock (m_msgQueue)
            {
                canMessage.setData(a_message.getData());
                canMessage.setFlags(a_message.getFlags());
                canMessage.setID(a_message.getID());
                canMessage.setLength(a_message.getLength());
                canMessage.setTimeStamp(a_message.getTimeStamp());
                m_msgQueue.Enqueue(canMessage);
                m_resetEvent.Set();
              //  sendAck();
            }
        }

    }
}

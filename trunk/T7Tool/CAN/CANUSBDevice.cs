using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using T7Tool.CAN;

namespace T7Tool.KWP
{

    class CANUSBDevice : ICANDevice
    {

        static uint m_deviceHandle = 0;
        Thread m_readThread;
        Object m_synchObject = new Object();
        bool m_endThread = false;

        public CANUSBDevice()
        {
            m_readThread = new Thread(readMessages);
        }

        ~CANUSBDevice()
        {
            lock (m_synchObject)
            {
                m_endThread = true;
            }
            close();
            m_readThread.Join();
        }


        public void readMessages()
        {
            int readResult = 0;
            LAWICEL.CANMsg r_canMsg = new LAWICEL.CANMsg();
            CANMessage canMessage = new CANMessage();
            while (true)
            {
                lock (m_synchObject)
                {
                    if (m_endThread)
                        return;
                }
                readResult = LAWICEL.canusb_Read(m_deviceHandle, out r_canMsg);
                if (readResult == LAWICEL.ERROR_CANUSB_OK)
                {
                    canMessage.setID(r_canMsg.id);
                    canMessage.setLength(r_canMsg.len);
                    canMessage.setTimeStamp(r_canMsg.timestamp);
                    canMessage.setFlags(r_canMsg.flags);
                    canMessage.setData(r_canMsg.data);
                    lock (m_listeners)
                    {
                        foreach (ICANListener listener in m_listeners)
                        {
                            listener.handleMessage(canMessage);
                        }
                    }
                }
                else if (readResult == LAWICEL.ERROR_CANUSB_NO_MESSAGE)
                {
                    Thread.Sleep(1);
                }
            }
        }

        override public OpenResult open()
        {
            close();
            //Check if P bus is connected
            m_deviceHandle = LAWICEL.canusb_Open(IntPtr.Zero,
            LAWICEL.CAN_BAUD_500K,
            LAWICEL.CANUSB_ACCEPTANCE_CODE_ALL,
            LAWICEL.CANUSB_ACCEPTANCE_MASK_ALL,
            LAWICEL.CANUSB_FLAG_TIMESTAMP);
            if (boxIsThere())
            {
                m_readThread.Start();
                return OpenResult.OK;
            }
            
            //P bus not connected
            //Check if I bus is connected
            close();
            m_deviceHandle = LAWICEL.canusb_Open(IntPtr.Zero,
                "0xcb:0x9a",
                LAWICEL.CANUSB_ACCEPTANCE_CODE_ALL,
                LAWICEL.CANUSB_ACCEPTANCE_MASK_ALL,
                LAWICEL.CANUSB_FLAG_TIMESTAMP);
            if (boxIsThere())
            {
                m_readThread.Start();
                return OpenResult.OK;
            }
            close();
            return OpenResult.OpenError;
        }

        override public CloseResult close()
        {
            if(m_readThread.IsAlive)
                m_readThread.Join();
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

        override public bool isOpen()
        {
            if (m_deviceHandle > 0)
                return true;
            else
                return false;
        }


        override public bool sendMessage(CANMessage a_message)
        {
            LAWICEL.CANMsg msg = new LAWICEL.CANMsg();
            msg.id = a_message.getID();
            msg.len = a_message.getLength();
            msg.flags = a_message.getFlags();
            msg.data = a_message.getData();
            int writeResult;
            writeResult = LAWICEL.canusb_Write(m_deviceHandle, ref msg);
            if (writeResult == LAWICEL.ERROR_CANUSB_OK)
                return true;
            else
                return false;
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

        private bool boxIsThere()
        {
            LAWICEL.CANMsg msg = new LAWICEL.CANMsg();
            if (waitForMessage(0x280, 2000, out msg) != 0)
                return true;
            if (sendWriteRequest())
                return true;

            return false;
        }

        private bool sendWriteRequest()
        {
            CANMessage msg1 = new CANMessage(0x240, 0, 8);
            CANMessage msg2 = new CANMessage(0x240, 0, 8);
            LAWICEL.CANMsg msg = new LAWICEL.CANMsg();

            msg1.setData(0x000000003408A141);
            msg2.setData(0x00000000B007A100);

            if (!sendMessage(msg1))
                return false;
            if (!sendMessage(msg2))
                return false;
            if(waitForMessage(0x258, 5000, out msg) == 0x258)
                return false;
            return true;
        }

       
    }


           
}

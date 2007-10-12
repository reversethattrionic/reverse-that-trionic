using System;
using System.Collections.Generic;
using System.Text;

namespace T7Tool.CAN
{
    public enum OpenResult
    {
        OK,
        OpenError
    }

    public enum CloseResult
    {
        OK,
        CloseError
    }

    abstract class ICANDevice
    {
        abstract public OpenResult open();
        abstract public CloseResult close();
        abstract public bool isOpen();
        abstract public bool sendMessage(CANMessage a_message);
        public bool addListener(ICANListener a_listener) 
        { 
            lock(m_listeners)
            {
                m_listeners.Add(a_listener);
            }
            return true;
        }
        public bool removeListener(ICANListener a_listener) 
        {
            lock(m_listeners)
            {
                m_listeners.Remove(a_listener);
            }
            return true;
        }

        protected List<ICANListener> m_listeners = new List<ICANListener>();
    }
}

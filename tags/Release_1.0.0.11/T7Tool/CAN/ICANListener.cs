using System;
using System.Collections.Generic;
using System.Text;

namespace T7Tool.CAN
{
    abstract class ICANListener
    {
        public abstract void handleMessage(CANMessage a_canMessage);
        public void addIDFilter(uint a_canID) { m_idFilter.Add(a_canID); }
        public void removeIDFilter(uint a_canID) { m_idFilter.Remove(a_canID); }
        private List<uint> m_idFilter = new List<uint>();
    }
}

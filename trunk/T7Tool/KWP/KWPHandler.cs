using System;
using System.Collections.Generic;
using System.Text;

namespace T7Tool.KWP
{
    enum KWPResult
    {
        OK,
        Timeout,
        DeviceNotConnected
    }

    class KWPHandler
    {
        IKWPDevice m_kwpDevice;

        public KWPHandler(IKWPDevice a_kwpDevice)
        {
            m_kwpDevice = a_kwpDevice;
        }

        public KWPHandler()
        {
        }

        public void setKWPDevice(IKWPDevice a_device)
        {
            m_kwpDevice = a_device;
        }

        public bool openKWPDevice()
        {
            if (m_kwpDevice.open() == OpenResult.OK)
                return true;
            else
                return false;
        }

        public bool startSession()
        {
            return m_kwpDevice.startSession();
        }

        public bool closeKWPDevice()
        {
            if (m_kwpDevice.close() == CloseResult.OK)
                return true;
            else
                return false;
        }

        public KWPResult getVIN(out string r_vin)
        {
            r_vin = "MYSAAB";
            KWPRequest request = new KWPRequest(0x1A,0x90);
            KWPReply reply = new KWPReply();
            if (!m_kwpDevice.isOpen())
                return KWPResult.DeviceNotConnected;
            m_kwpDevice.sendRequest(request, out reply);
            r_vin = getString(reply);
            return KWPResult.OK;
        }

        private string getString(KWPReply a_reply)
        {
            Encoding ascii = Encoding.ASCII;
            ascii.GetChars(a_reply.getData(), 0, a_reply.getData().Length);
            return ascii.GetString(a_reply.getData(), 0, a_reply.getData().Length);
        }
    }
}

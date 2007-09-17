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

        public bool startSession()
        {
            return m_kwpDevice.startSession();
        }

        public bool openDevice()
        {
            return m_kwpDevice.open();
        }

        public bool closeDevice()
        {
            return m_kwpDevice.close();
        }


        public KWPResult getVIN(out string r_vin)
        {
            KWPReply reply = new KWPReply();
            KWPResult result;
            result = sendRequest(new KWPRequest(0x1A,0x90), out reply);
            r_vin = getString(reply);
            return KWPResult.OK;
        }

        public KWPResult getImmo(out string r_immo)
        {
            KWPReply reply = new KWPReply();
            KWPResult result;
            result = sendRequest(new KWPRequest(0x1A, 0x92), out reply);
            r_immo = getString(reply);
            return result;
        }

        public KWPResult getSwPartNumber(out string r_swPartNo)
        {
            KWPReply reply = new KWPReply();
            KWPResult result;
            result = sendRequest(new KWPRequest(0x1A, 0x94), out reply);
            r_swPartNo = getString(reply);
            return result;
        }

        public KWPResult getSwVersion(out string r_swVersion)
        {
            KWPReply reply = new KWPReply();
            KWPResult result;
            result = sendRequest(new KWPRequest(0x1A, 0x95), out reply);
            r_swVersion = getString(reply);
            return result;
        }

        public KWPResult getEngineType(out string r_swVersion)
        {
            KWPReply reply = new KWPReply();
            KWPResult result;
            result = sendRequest(new KWPRequest(0x1A, 0x97), out reply);
            r_swVersion = getString(reply);
            return result;
        }


        private KWPResult sendRequest(KWPRequest a_request, out KWPReply a_reply)
        {
            KWPReply reply = new KWPReply();
            RequestResult result;
            a_reply = new KWPReply();
            if (!m_kwpDevice.isOpen())
                return KWPResult.DeviceNotConnected;
            result = m_kwpDevice.sendRequest(a_request, out reply);
            a_reply = reply;
            if (result == RequestResult.NoError)
                return KWPResult.OK;
            else
                return KWPResult.Timeout;
        }

        private string getString(KWPReply a_reply)
        {
            Encoding ascii = Encoding.ASCII;
            ascii.GetChars(a_reply.getData(), 0, a_reply.getData().Length);
            return ascii.GetString(a_reply.getData(), 0, a_reply.getData().Length);
        }
    }
}

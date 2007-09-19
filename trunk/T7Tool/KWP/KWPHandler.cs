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

        /// <summary>
        /// Send a request for sequrity access.
        /// This is needed to access protected functions (flash reading and writing).
        /// </summary>
        /// <returns>True on success, otherwise false.</returns>
        public bool requestSequrityAccess()
        {
            //Try method 1
            if (requestSequrityAccessLevel(1))
                return true;
            if (requestSequrityAccessLevel(2))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Send a request for a sequrity access with one out of two methods to 
        /// calculate the key.
        /// </summary>
        /// <param name="a_method">Key calculation method [1,2]</param>
        /// <returns>true if sequrity access was granted, otherwise false</returns>
        private bool requestSequrityAccessLevel(uint a_method)
        {
            KWPReply reply = new KWPReply();
            KWPResult result;
            byte[] seed = new byte[2];
            byte[] key = new byte[2];
            // Send a seed request.
            result = sendRequest(new KWPRequest(0x27, 0x05), out reply);
            if (result != KWPResult.OK)
                return false;
            seed[0] = reply.getData()[0];
            seed[1] = reply.getData()[1];
            if (a_method == 1)
                key = calculateKey(seed, 0);
            else
                key = calculateKey(seed, 1);
            // Send key reply.
            result = sendRequest(new KWPRequest(0x27, 0x06, key), out reply);
            if (result != KWPResult.OK)
                return false;
            //Check if sequrity was granted.
            if ((reply.getData()[0] == 0x67) && (reply.getData()[2] == 0x34))
                return true;

            return false;
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

        /// <summary>
        /// Calculate key for a seed.
        /// </summary>
        /// <param name="a_seed">Byte array with two bytes representing the seed.</param>
        /// <param name="a_method">Type of method to use for calculation [0,1].</param>
        /// <returns>Byte array with two bytes representing the key.</returns>
        private byte[] calculateKey(byte[] a_seed, uint a_method)
        {
            int key;
            byte[] returnKey = new byte[2];
            int seed = (int)a_seed[0] << 8;
            seed = seed & (int)a_seed[1];
    
            key = seed << 2;
            key &= 0xFFFF;
            key ^= (a_method == 1 ? 0x4081 : 0x8142);
            key -= (a_method == 1 ? 0x1F6F : 0x2356);
            key &= 0xFFFF;

            returnKey[1] = (byte)key;
            key = key >> 8;
            returnKey[0] = (byte)key;

            return returnKey;
        }

    }
}

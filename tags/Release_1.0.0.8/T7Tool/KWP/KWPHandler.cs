using System;
using System.Collections.Generic;
using System.Text;

namespace T7Tool.KWP
{
    enum KWPResult
    {
        OK,
        NOK,
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
            if ((reply.getMode() == 0x67) && (reply.getData()[0] == 0x34))
                return true;

            return false;
        }

        public KWPResult getVIN(out string r_vin)
        {
            KWPReply reply = new KWPReply();
            KWPResult result;
            result = sendRequest(new KWPRequest(0x1A,0x90), out reply);
            if (result == KWPResult.OK)
            {
                r_vin = getString(reply);
                return KWPResult.OK;
            }
            else
            {
                r_vin = "";
                return KWPResult.Timeout;
            }
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

        /// <summary>
        /// sendEraseRequest sends an erase request to the ECU.
        /// </summary>
        /// <returns>KWPResult</returns>
        public KWPResult sendEraseRequest()
        {
            KWPReply reply = new KWPReply();
            KWPReply reply2 = new KWPReply();
            KWPResult result = KWPResult.Timeout;
            int i = 0;

            //First erase message. Up to 5 retries.
            //Mode = 0x31
            //PID = 0x52
            //Expected result is 0x71
            result = sendRequest(new KWPRequest(0x31, 0x52), out reply);
            while (reply.getMode() != 0x71) 
            {
                System.Threading.Thread.Sleep(1000);
                result = sendRequest(new KWPRequest(0x31, 0x52), out reply);
                if (i++ > 15) return KWPResult.Timeout;
            }
            if (result != KWPResult.OK) 
                return result;

            //Second erase message. Up to 10 retries.
            //Mode = 0x31
            //PID = 0x53
            //Expected result is 0x71
            i = 0;
            result = sendRequest(new KWPRequest(0x31, 0x53), out reply2);
            while (reply2.getMode() != 0x71)
            {
                System.Threading.Thread.Sleep(1000);
                result = sendRequest(new KWPRequest(0x31, 0x53), out reply2);
                if (i++ > 20) return KWPResult.Timeout;
            }

            //Erase confirm message
            //Mode = 0x3E
            //Expected result is 0x7E
            result = sendRequest(new KWPRequest(0x3E, 0x53), out reply2);

            return result;
        }

        public KWPResult sendWriteRequest(uint a_address, uint a_length)
        {
            KWPReply reply = new KWPReply();
            KWPResult result;
            byte[] addressAndLength = new byte[7];
            //set address (byte 0 to 2)
            addressAndLength[0] = (byte)(a_address >> 16);
            addressAndLength[1] = (byte)(a_address >> 8);
            addressAndLength[2] = (byte)(a_address);
            //set length (byte 3 to 6);
            addressAndLength[3] = (byte)(a_length >> 24);
            addressAndLength[4] = (byte)(a_length >> 16);
            addressAndLength[5] = (byte)(a_length >> 8);
            addressAndLength[6] = (byte)(a_length);

            //Send request
            //Mode = 0x34
            //PID = no PID used by this request
            //Data = aaallll (aaa = address, llll = length)
            //Expected result = 0x74
            result = sendRequest(new KWPRequest(0x34, addressAndLength), out reply);
            if (reply.getMode() != 0x74)
                return KWPResult.NOK;
            else
                return result;
        }

        public KWPResult sendWriteDataRequest(byte[] a_data)
        {
            KWPReply reply = new KWPReply();
            KWPResult result;

            //Send request
            //Mode = 0x36
            //PID = no PID used by this request
            //Data = data to be flashed
            //Expected result = 0x76
            result = sendRequest(new KWPRequest(0x36, a_data), out reply);
            if (reply.getMode() != 0x76)
                return KWPResult.NOK;
            else
                return result;
        }

        public bool sendReadRequest(uint a_address, uint a_length)
        {
            KWPReply reply = new KWPReply();
            KWPResult result;
            byte[] lengthAndAddress = new byte[5];
            //set length (byte 0 and 1)
            lengthAndAddress[0] = (byte)(a_length >> 8);
            lengthAndAddress[1] = (byte)(a_length);
            //set address (byte 2 to 4);
            lengthAndAddress[2] = (byte)(a_address >> 16);
            lengthAndAddress[3] = (byte)(a_address >> 8);
            lengthAndAddress[4] = (byte)(a_address);
            result = sendRequest(new KWPRequest(0x2C, 0xF0, 0x03, lengthAndAddress), out reply);
            if (result == KWPResult.OK)
                return true;
            else
                return false;
        }

        public bool sendDataTransferExitRequest()
        {
            KWPReply reply = new KWPReply();
            KWPResult result;
            result = sendRequest(new KWPRequest(0x82, 0x00), out reply);
            if (result == KWPResult.OK)
                return true;
            else 
                return false;
        }

        public bool sendDataTransferRequest(out byte[] r_data)
        {
            KWPReply reply = new KWPReply();
            KWPResult result;
            result = sendRequest(new KWPRequest(0x21, 0xF0), out reply);
            r_data = reply.getData();
            if (result == KWPResult.OK)
                return true;
            else 
                return false;
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
            int seed = a_seed[0] << 8 | a_seed[1];
    
            key = seed << 2;
            key &= 0xFFFF;
            key ^= (a_method == 1 ? 0x4081 : 0x8142);
            key -= (a_method == 1 ? 0x1F6F : 0x2356);
            key &= 0xFFFF;

            returnKey[0] = (byte)((key >> 8) & 0xFF);
            returnKey[1] = (byte)(key & 0xFF);

            return returnKey;
        }

    }
}

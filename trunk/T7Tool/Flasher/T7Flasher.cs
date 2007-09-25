using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using T7Tool.KWP;
using System.Threading;

namespace T7Tool.Flasher
{
    class T7Flasher
    {

        public enum FlashCommand
        {
            ReadCommand,
            WriteCommand,
            StopCommand, 
            NoCommand
        };

        public enum FlashStatus
        {
            Reading,
            Writing,
            NoSequrityAccess,
            DoinNuthin,
            Completed
        }


        public FlashStatus getStatus() { return m_flashStatus; }
        public int getNrOfBytesRead() { return m_nrOfBytesRead; }
        public void stopFlasher()
        {
            lock (m_synchObject)
            {
                m_command = FlashCommand.StopCommand;
            }
        }

        public T7Flasher(KWPHandler a_kwpHandler)
        {
            m_kwpHandler = a_kwpHandler;
            m_command = FlashCommand.NoCommand;
            m_flashStatus = FlashStatus.DoinNuthin;
            m_thread = new Thread(run);
            m_thread.Start();
        }

        ~T7Flasher()
        {
            lock (m_synchObject)
            {
                m_endThread = true;
            }
            m_resetEvent.Set();
            m_thread.Join();
        }

        public void readFlash(string a_fileName)
        {
            lock (m_synchObject)
            {
                m_command = FlashCommand.ReadCommand;
                m_fileName = a_fileName;
            }
            m_resetEvent.Set();
        }

        private void run()
        {
            bool gotSequrityAccess = false;
            while (true)
            {
                
                m_nrOfRetries = 0;
                m_nrOfBytesRead = 0;
                m_resetEvent.WaitOne(-1, true);
                lock (m_synchObject)
                {
                    if (m_endThread)
                        return;
                }
                m_kwpHandler.startSession();
                if (!gotSequrityAccess)
                {
                    for (int nrOfSequrityTries = 0; nrOfSequrityTries < 5; nrOfSequrityTries++)
                    {
                        if (!m_kwpHandler.requestSequrityAccess())
                            m_flashStatus = FlashStatus.NoSequrityAccess;
                        else
                        {
                            gotSequrityAccess = true;
                            break;
                        }
                    }
                }
                if (!gotSequrityAccess)
                    continue;
                if (m_command == FlashCommand.ReadCommand)
                {
                    int nrOfBytes = 64;
                    byte[] data;

                    if(File.Exists(m_fileName))
                        File.Delete(m_fileName);
                    FileStream fileStream = File.Create(m_fileName, 1024); 
                             
                    for (int i = 0; i < 512 * 1024 / nrOfBytes; i++)
                    {
                        lock (m_synchObject)
                        {
                            if (m_command == FlashCommand.StopCommand)
                                break;
                            if (m_endThread)
                                return;
                        }
                        m_flashStatus = FlashStatus.Reading;
                        while(!m_kwpHandler.sendReadRequest((uint)(nrOfBytes * i), (uint)nrOfBytes))
                        {
                            m_nrOfRetries++;
                        }

                        while (!m_kwpHandler.sendDataTransferRequest(out data))
                        {
                            m_nrOfRetries++;
                        }
                        fileStream.Write(data, 0, nrOfBytes);
                        m_nrOfBytesRead +=  nrOfBytes;
                    }
                    fileStream.Close();
                    m_kwpHandler.sendDataTransferExitRequest();
                }
                m_flashStatus = FlashStatus.DoinNuthin;
            }
        }


        private Thread m_thread;
        private AutoResetEvent m_resetEvent = new AutoResetEvent(false);
        private FlashCommand m_command;
        private string m_fileName;
        private Object m_synchObject = new Object();
        private KWPHandler m_kwpHandler;
        private int m_nrOfRetries;
        private FlashStatus m_flashStatus;
        private int m_nrOfBytesRead;
        private bool m_endThread = false;
    }
}

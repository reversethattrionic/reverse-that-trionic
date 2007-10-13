using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using T7Tool.KWP;
using System.Threading;

namespace T7Tool.Flasher
{
    /// <summary>
    /// T7Flasher handles reading and writing of flash in Trionic 7 ECUs.
    /// 
    /// To use this class a KWPHandler must be set for the communication.
    /// </summary>
    class T7Flasher
    {
        /// <summary>
        /// FlashCommand is a representation of the commands for this class.
        /// </summary>
        public enum FlashCommand
        {
            ReadCommand,
            WriteCommand,
            StopCommand, 
            NoCommand
        };


        /// <summary>
        /// FlashStatus is used for reporting the current status a flashing session.
        /// </summary>
        public enum FlashStatus
        {
            Reading,
            Writing,
            NoSequrityAccess,
            DoinNuthin,
            Completed,
            NoSuchFile,
            EraseError,
            WriteError
        }

        /// <summary>
        /// This method returns the current status of this class.
        /// </summary>
        /// <returns>FlashStatus</returns>
        public FlashStatus getStatus() { return m_flashStatus; }

        /// <summary>
        /// This method returns the number of bytes that has been read or written so far.
        /// 0 is returned if there is no read or write session ongoing.
        /// </summary>
        /// <returns>Number of bytes that has been read or written.</returns>
        public int getNrOfBytesRead() { return m_nrOfBytesRead; }

        /// <summary>
        /// This method interrupts ongoing read or write session.
        /// </summary>
        public void stopFlasher()
        {
            lock (m_synchObject)
            {
                m_command = FlashCommand.StopCommand;
            }
        }

        /// <summary>
        /// Constructor for T7Flasher.
        /// </summary>
        /// <param name="a_kwpHandler">The KWPHandler to be used for the communication.</param>
        public T7Flasher(KWPHandler a_kwpHandler)
        {
            m_kwpHandler = a_kwpHandler;
            m_command = FlashCommand.NoCommand;
            m_flashStatus = FlashStatus.DoinNuthin;
            m_thread = new Thread(run);
            m_thread.Start();
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~T7Flasher()
        {
            lock (m_synchObject)
            {
                m_endThread = true;
            }
            m_resetEvent.Set();
        }

        /// <summary>
        /// This method starts a reading session.
        /// </summary>
        /// <param name="a_fileName">Name of the file where the flash contents is saved.</param>
        public void readFlash(string a_fileName)
        {
            lock (m_synchObject)
            {
                m_command = FlashCommand.ReadCommand;
                m_fileName = a_fileName;
            }
            m_resetEvent.Set();
        }

        /// <summary>
        /// This method starts writing to flash.
        /// </summary>
        /// <param name="a_fileName">The name of the file from where to read the data from.</param>
        public void writeFlash(string a_fileName)
        {
            lock (m_synchObject)
            {
                m_command = FlashCommand.WriteCommand;
                m_fileName = a_fileName;
            }
            m_resetEvent.Set();
        }

        /// <summary>
        /// The run method handles writing and reading. It waits for a command to start read
        /// or write and handles this command until it's completed, stopped or until there is 
        /// a failure.
        /// </summary>
        private void run()
        {
            bool gotSequrityAccess = false;
            while (true)
            {
                
                m_nrOfRetries = 0;
                m_nrOfBytesRead = 0;
                m_resetEvent.WaitOne(-1, true);
                gotSequrityAccess = false;
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
                //Here it would make sense to stop if we didn't ge security acces but
                //let's try anyway. It could be that we don't get a possitive reply from the 
                //ECU if we alredy have security access (from a previous, interrupted, session).
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
                                continue;
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
                else if (m_command == FlashCommand.WriteCommand)
                {
                    int nrOfBytes = 128;
                    int i = 0;
                    byte[] data = new byte[nrOfBytes];
                    m_flashStatus = FlashStatus.Writing;
                    if (!File.Exists(m_fileName))
                    {
                        m_flashStatus = FlashStatus.NoSuchFile;
                        continue;
                    }
                    if (m_kwpHandler.sendEraseRequest() != KWPResult.OK)
                    {
                        m_flashStatus = FlashStatus.EraseError;
                       // break;
                    }
                    FileStream fs = new FileStream(m_fileName, FileMode.Open, FileAccess.Read);

                    //Write 0x0-0x7B000
                    if (m_kwpHandler.sendWriteRequest(0x0, 0x7B000) != KWPResult.OK)
                    {
                        m_flashStatus = FlashStatus.WriteError;
                        continue;
                    }
                    for (i = 0; i < 0x7B000 / nrOfBytes; i++)
                    {
                        fs.Read(data, 0, nrOfBytes);
                        m_nrOfBytesRead = i * nrOfBytes;
                        if (m_kwpHandler.sendWriteDataRequest(data) != KWPResult.OK)
                        {
                            m_flashStatus = FlashStatus.WriteError;
                            continue; 
                        }
                        lock (m_synchObject)
                        {
                            if (m_command == FlashCommand.StopCommand)
                                continue;
                            if (m_endThread)
                                return;
                        }
                    }

                    //Write 0x7FE00-0x7FFFF
                    if (m_kwpHandler.sendWriteRequest(0x7FE00, 0x200) != KWPResult.OK)
                    {
                        m_flashStatus = FlashStatus.WriteError;
                        continue;
                    }
                    fs.Seek(0x7FE00, System.IO.SeekOrigin.Begin);
                    for (i = 0x7FE00 / nrOfBytes; i < 0x80000 / nrOfBytes; i++)
                    {
                        fs.Read(data, 0, nrOfBytes);
                        m_nrOfBytesRead = i * nrOfBytes;
                        if (m_kwpHandler.sendWriteDataRequest(data) != KWPResult.OK)
                        {
                            m_flashStatus = FlashStatus.WriteError;
                            continue;
                        }
                        lock (m_synchObject)
                        {
                            if (m_command == FlashCommand.StopCommand)
                                continue;
                            if (m_endThread)
                                return;
                        }
                    }
                }
                m_flashStatus = FlashStatus.Completed;
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

using System;
using System.Collections.Generic;
using System.Text;

namespace T7Tool.KWP
{

    public enum RequestResult 
    {
        NoError,
        ErrorSending,
        Timeout,
        ErrorReceiving
    } 


    interface IKWPDevice
    {
        bool startSession();
        RequestResult sendRequest(KWPRequest a_request, out KWPReply r_reply);
        bool open();
        bool close();
        bool isOpen();
    }
}

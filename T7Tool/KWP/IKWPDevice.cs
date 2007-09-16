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

    interface IKWPDevice
    {
        OpenResult open();
        CloseResult close();
        bool isOpen();
        bool startSession();
        RequestResult sendRequest(KWPRequest a_request, out KWPReply r_reply);
    }
}

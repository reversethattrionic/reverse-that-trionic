using System;
using System.Collections.Generic;
using System.Text;
using T5CANLib.CAN;
using T5CANLib;

namespace T5CANTest
{
    class Program
    {
        static void Main(string[] args)
        {
            T5CAN t5can = new T5CAN();
            CANUSBDevice canusb = new CANUSBDevice();
            string swVersion = "";
            string symbolTable = "";
            t5can.setCANDevice(canusb);
            if (!t5can.openDevice(out swVersion))
            {
                System.Console.WriteLine("Could not open CAN device");
                return;
            }
      /*      if(!t5can.initialize())
            {
                System.Console.WriteLine("Could not initialize");
                return;
            }
            swVersion = t5can.getSWVersion();*/
            System.Console.WriteLine("SW version: " + swVersion);
            System.Console.WriteLine();
           // t5can.getSymbolTable(out symbolTable);

            Environment.Exit(0);
        }
    }
}

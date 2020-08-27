using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using Assignment_2b.Server;

namespace Assignment_2b
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            Trace.Listeners.Add(new TextWriterTraceListener(new StreamWriter("log.txt", true)));
            MyServer myServer = new MyServer();
            myServer.Start();

            Console.ReadKey();
            Trace.Close();
        }
    }
}

using System;
using System.IO;
using SimpleFramework.TCPServer;

namespace Assignment_2b.Server
{
    public class MyServer : AbstractTCPServer
    {
        protected override void TcpServerWork(StreamReader streamReader, StreamWriter streamWriter)
        {
            streamWriter.WriteLine();
            string line = streamReader.ReadLine();

            while (!string.IsNullOrEmpty(line))
            {
                Console.WriteLine("Client: " + line.ToUpper());

                string[] strings = line.Split(' ');

                line = streamReader.ReadLine();
            }
        }
    }
}
using System;
using System.IO;
using SimpleFramework.TCPServer;

namespace SimpleFramework
{
    public class Work : AbstractTCPServer
    {
        protected override void TcpServerWork(StreamReader streamReader, StreamWriter streamWriter)
        {
            string str = streamReader.ReadLine();
            streamWriter.WriteLine(str);
        }
    }
}

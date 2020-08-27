using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Xml;

namespace SimpleFramework.TCPServer
{
    public abstract class AbstractTCPServer
    {
        private int port;
        //XmlTextReader xmlTextReader = new XmlTextReader("AbstractTCPServerConfiq.xml");

        public void Start()
        {
            XmlDocument configDoc = new XmlDocument();
            configDoc.Load("AbstractTCPServerConfiq.xml");

            //while (xmlTextReader.Read())
            //{
            //    switch (xmlTextReader.NodeType)
            //    {
            //        case XmlNodeType.Element:
            //            Console.WriteLine(xmlTextReader.Name);
            //            break;

            //        case XmlNodeType.Text:
            //            Console.WriteLine(xmlTextReader.Value);
            //            break;
            //    }
            //}

            XmlNode xxNode = configDoc.DocumentElement.SelectSingleNode("serverport");
            if (xxNode != null)
            {
                port = Convert.ToInt32(xxNode.InnerText.Trim());
                Console.WriteLine(port);
            }

            TcpListener server = new TcpListener(IPAddress.Loopback, port);
            server.Start();
            Console.WriteLine("Server started");
            while (true)
            {
                TcpClient socket = server.AcceptTcpClient(); // venter på client
                Console.WriteLine("Server connected to a client");
                // starter ny tråd
                Task.Run(
                    // indsætter en metode (delegate)
                    () =>
                    {
                        TcpClient tmpsocket = socket;
                        DoClient(tmpsocket);
                    }
                );
            }
        }

        private void DoClient(TcpClient socket)
        {
            using (StreamReader streamReader = new StreamReader(socket.GetStream()))
            using (StreamWriter streamWriter = new StreamWriter(socket.GetStream()))
            {
                TcpServerWork(streamReader, streamWriter);
                streamWriter.Flush();
            }
        }

        //Template metode
        protected abstract void TcpServerWork(StreamReader streamReader, StreamWriter streamWriter);
    }
}
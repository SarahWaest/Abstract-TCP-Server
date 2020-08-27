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
        private string servername;
        private bool running = true;

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

            XmlNode portNode = configDoc.DocumentElement.SelectSingleNode("serverport");
            if (portNode != null)
            {
                port = Convert.ToInt32(portNode.InnerText.Trim());
                Console.WriteLine(port);
            }

            XmlNode serverNameNode = configDoc.DocumentElement.SelectSingleNode("servername");
            if (serverNameNode != null)
            {
                servername = serverNameNode.InnerText.Trim();
            }

            TcpListener shutdownServer = new TcpListener(IPAddress.Loopback, port + 1);
            Task.Run(() =>
            {
                shutdownServer.Start();
                while (running)
                {
                    TcpClient ShutdownSocket = shutdownServer.AcceptTcpClient();
                    using (StreamReader sr = new StreamReader(ShutdownSocket.GetStream()))
                    {
                        if (sr.ReadLine() == "Close")
                        {
                            Console.WriteLine("Server shutdown aktiveret");
                            softShutdown();
                        }
                    }
                }
            });

            TcpListener server = new TcpListener(IPAddress.Loopback, port);
            server.Start();
            Console.WriteLine(servername + " started at " + port);
            while (running)
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


        private void softShutdown()
        {
            if (running == true)
            {
                running = false;
            }
        }

        private void StopServer()
        {
            port = port + 1;

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
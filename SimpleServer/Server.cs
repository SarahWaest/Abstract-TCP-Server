using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleServer
{
    public class Server
    {
        public void DoClient(TcpClient socket)
        {
            using (socket)
            {
                NetworkStream ns = socket.GetStream();
                StreamWriter sw = new StreamWriter(ns);
                StreamReader sr = new StreamReader(ns);
                sw.AutoFlush = true;
                string line = sr.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    Console.WriteLine("Client: " + line);

                    string[] str = line.Split(' ');

                    line = sr.ReadLine();
                }
            }
        }

        public void Start()
        {
            TcpListener server = new TcpListener(IPAddress.Loopback, 7007);
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
    }
}
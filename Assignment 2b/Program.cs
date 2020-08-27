using System;
using Assignment_2b.Server;

namespace Assignment_2b
{
    class Program
    {
        static void Main(string[] args)
        {
            MyServer myServer = new MyServer();
            myServer.Start();
        }
    }
}

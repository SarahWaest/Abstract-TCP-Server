﻿using System;

namespace SimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Start();
        }
    }
}

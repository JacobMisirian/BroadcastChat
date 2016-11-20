using System;

using BroadcastChatServer.Server;

namespace BroadcastChatServer
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            new BroadcastChatServer.Server.BroadcastChatServer().Start(1337);
        }
    }
}

using System;

using BroadcastChatServer.Config;
using BroadcastChatServer.Server;

namespace BroadcastChatServer
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            BroadcastChatServer.Server.BroadcastChatServer.CreateFromConfig(BroadcastChatServerConfigParser.Parse(args[0])).Start();
        }
    }
}

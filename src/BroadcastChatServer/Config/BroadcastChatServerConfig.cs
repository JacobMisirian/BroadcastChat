using System;

namespace BroadcastChatServer.Config
{
    public class BroadcastChatServerConfig
    {
        public const string DEFAULT_MOTD = "Welcome to a BroadcastChat server. Use the NICK command to set your nick first!";
        public const string DEFAULT_SERVER_NAME = "BroadcastChat";

        public string Motd { get; set; }
        public int Port { get; set; }
        public string ServerName { get; set; }

        public BroadcastChatServerConfig()
        {
            Motd = DEFAULT_MOTD;
            Port = 1337;
            ServerName = DEFAULT_SERVER_NAME;
        }
    }
}
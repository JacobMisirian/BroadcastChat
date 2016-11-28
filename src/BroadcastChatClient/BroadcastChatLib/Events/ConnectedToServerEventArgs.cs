using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class ConnectedToServerEventArgs: EventArgs
    {
        public string Host { get; private set; }
        public int Port { get; private set; }

        public ConnectedToServerEventArgs(string host, int port)
        {
            Host = host;
            Port = port;
        }
    }
}


using System;

using BroadcastChatServer.Networking;

namespace BroadcastChatServer.Events
{
    public class ClientDisconnectedEventArgs: EventArgs
    {
        public BroadcastChatClient Client { get; private set; }

        public ClientDisconnectedEventArgs(BroadcastChatClient client)
        {
            Client = client;
        }
    }
}


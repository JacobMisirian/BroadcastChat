using System;

using BroadcastChatServer.Networking;

namespace BroadcastChatServer.Events
{
    public class ClientConnectedEventArgs: EventArgs
    {
        public BroadcastChatClient Client { get; private set; }

        public ClientConnectedEventArgs(BroadcastChatClient client)
        {
            Client = client;
        }
    }
}


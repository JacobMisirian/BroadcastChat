using System;

using BroadcastChatServer.Networking;

namespace BroadcastChatServer.Events
{
    public class ClientMessageReceivedEventArgs: EventArgs
    {
        public BroadcastChatClient Client { get; private set; }
        public string Message { get; private set; }

        public ClientMessageReceivedEventArgs(BroadcastChatClient client, string message)
        {
            Client = client;
            Message = message;
        }
    }
}


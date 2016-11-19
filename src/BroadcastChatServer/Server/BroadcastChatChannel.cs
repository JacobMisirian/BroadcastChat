using System;
using System.Collections.Generic;

using BroadcastChatServer.Networking;

namespace BroadcastChatServer.Server
{
    public class BroadcastChatChannel
    {
        public List<BroadcastChatClient> Clients { get; private set; }
        public string Name { get; private set; }

        public void SendChanMsg(BroadcastChatClient sender, string message)
        {
            foreach (var client in Clients)
                client.SendChanMsg(Name, sender.Nick, message);
        }
    }
}


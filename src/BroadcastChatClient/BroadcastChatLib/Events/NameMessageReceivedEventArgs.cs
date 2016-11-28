using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class NameMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string ServerName { get; private set; }

        public NameMessageReceivedEventArgs(string rawMessage, string serverName) : base(rawMessage)
        {
            ServerName = serverName;
        }
    }
}


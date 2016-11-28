using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class MotdMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string Motd { get; private set; }

        public MotdMessageReceivedEventArgs(string rawMessage, string motd) : base(rawMessage)
        {
            Motd = motd;
        }
    }
}


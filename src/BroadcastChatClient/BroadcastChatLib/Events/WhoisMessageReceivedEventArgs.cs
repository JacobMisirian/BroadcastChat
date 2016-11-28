using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class WhoisMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string User { get; private set; }
        public string Whois { get; private set; }

        public WhoisMessageReceivedEventArgs(string rawMessage, string user, string whois) : base(rawMessage)
        {
            User = user;
            Whois = whois;
        }
    }
}


using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class BanMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string Banner { get; private set; }
        public string Channel { get; private set; }
        public string User { get; private set; }

        public BanMessageReceivedEventArgs(string rawMessage, string channel, string user, string banner) : base(rawMessage)
        {
            Channel = channel;
            User = user;
            Banner = banner;
        }
    }
}


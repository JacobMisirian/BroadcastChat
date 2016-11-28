using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class UnbanMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string Channel { get; private set; }
        public string Unbanner { get; private set; }
        public string User { get; private set; }

        public UnbanMessageReceivedEventArgs(string rawMessage, string channel, string unbanner, string user) : base(rawMessage)
        {
            Channel = channel;
            Unbanner = unbanner;
            User = user;
        }
    }
}


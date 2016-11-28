using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class KickMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string Channel { get; private set; }
        public string Kicker { get; private set; }
        public string User { get; private set; }

        public KickMessageReceivedEventArgs(string rawMessage, string channel, string user, string kicker) : base(rawMessage)
        {
            Channel = channel;
            Kicker = kicker;
            User = user;
        }
    }
}


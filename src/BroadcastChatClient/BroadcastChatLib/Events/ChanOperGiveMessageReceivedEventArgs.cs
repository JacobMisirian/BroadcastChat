using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class ChanOperGiveMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string Channel { get; private set; }
        public string Giver { get; private set; }
        public string User { get; private set; }

        public ChanOperGiveMessageReceivedEventArgs(string rawMessage, string channel, string giver, string user) : base(rawMessage)
        {
            Channel = channel;
            Giver = giver;
            User = user;
        }
    }
}


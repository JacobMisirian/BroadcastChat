using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class ChanOperTakeMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string Channel { get; private set; }
        public string Taker { get; private set; }
        public string User { get; private set; }

        public ChanOperTakeMessageReceivedEventArgs(string rawMessage, string channel, string taker, string user) : base(rawMessage)
        {
            Channel = channel;
            Taker = taker;
            User = user;
        }
    }
}


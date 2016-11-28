using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class NickMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string Channel { get; private set; }
        public string OldNick { get; private set; }
        public string NewNick { get; private set; }

        public NickMessageReceivedEventArgs(string rawMessage, string channel, string oldNick, string newNick) : base(rawMessage)
        {
            Channel = channel;
            OldNick = oldNick;
            NewNick = newNick;
        }
    }
}


using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class JoinMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string Channel { get; private set; }
        public string Nick { get; private set; }

        public JoinMessageReceivedEventArgs(string rawMessage, string channel, string nick) : base(rawMessage)
        {
            Channel = channel;
            Nick = nick;
        }
    }
}


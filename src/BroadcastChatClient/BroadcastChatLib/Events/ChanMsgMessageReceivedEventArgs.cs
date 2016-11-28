using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class ChanMsgMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string Channel { get; private set; }
        public string Sender { get; private set; }
        public string Message { get; private set; }

        public ChanMsgMessageReceivedEventArgs(string rawMessage, string channel, string sender, string message) : base(rawMessage)
        {
            Channel = channel;
            Sender = sender;
            Message = message;
        }
    }
}
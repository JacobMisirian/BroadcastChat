using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class PrivMsgMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string Sender { get; private set; }
        public string Message { get; private set; }

        public PrivMsgMessageReceivedEventArgs(string rawMessage, string sender, string message) : base(rawMessage)
        {
            Sender = sender;
            Message = message;
        }
    }
}


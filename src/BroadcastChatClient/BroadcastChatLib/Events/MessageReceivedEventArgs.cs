using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public abstract class MessageReceivedEventArgs: EventArgs
    {
        public string RawMessage { get; private set; }

        public MessageReceivedEventArgs(string rawMessage)
        {
            RawMessage = rawMessage;
        }
    }
}


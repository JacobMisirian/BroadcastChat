using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class ErrorMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string Error { get; private set; }

        public ErrorMessageReceivedEventArgs(string rawMessage, string error) : base(rawMessage)
        {
            Error = error;
        }
    }
}


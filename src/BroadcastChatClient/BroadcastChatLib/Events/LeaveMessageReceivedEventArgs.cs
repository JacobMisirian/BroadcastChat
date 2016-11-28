using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class LeaveMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string Channel { get; private set; }
        public string Reason { get; private set; }
        public string User { get; private set; }

        public LeaveMessageReceivedEventArgs(string rawMessage, string channel, string user, string reason) : base(rawMessage)
        {
            Channel = channel;
            Reason = reason;
            User = user;
        }
    }
}


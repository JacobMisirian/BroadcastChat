using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class ChanListMessageReceivedEventargs: MessageReceivedEventArgs
    {
        public string[] Channels { get; private set; }

        public ChanListMessageReceivedEventargs(string rawMessage, string chanList) : base(rawMessage)
        {
            Channels = chanList.Split(' ');
        }
    }
}


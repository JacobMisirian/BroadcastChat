using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class BanListMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string[] BanList { get; private set; }
        public string Channel { get; private set; }

        public BanListMessageReceivedEventArgs(string rawMessage, string channel, string banList) : base(rawMessage)
        {
            BanList = banList.Split(' ');
            Channel = channel;
        }
    }
}


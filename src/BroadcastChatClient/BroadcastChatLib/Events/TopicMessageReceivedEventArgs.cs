using System;

namespace BroadcastChatClient.BroadcastChatLib.Events
{
    public class TopicMessageReceivedEventArgs: MessageReceivedEventArgs
    {
        public string Channel { get; private set; }
        public string Topic { get; private set; }

        public TopicMessageReceivedEventArgs(string rawMessage, string channel, string topic) : base(rawMessage)
        {
            Channel = channel;
            Topic = topic;
        }
    }
}


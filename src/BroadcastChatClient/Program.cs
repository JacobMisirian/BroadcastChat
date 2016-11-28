using System;
using BroadcastChatClient.TUIClient;

namespace BroadcastChatClient
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            new ClientTUI(args[0], Convert.ToInt32(args[1])).Start();
        }
    }
}
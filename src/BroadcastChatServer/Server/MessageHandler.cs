using System;
using System.Text;

using BroadcastChatServer.Networking;

namespace BroadcastChatServer.Server
{
    public class MessageHandler
    {
        private BroadcastChatServer server;

        public MessageHandler(BroadcastChatServer server)
        {
            this.server = server;
        }

        public void HandleMessage(BroadcastChatClient client, string msg)
        {
            string[] parts = msg.Split(' ');

            switch (parts[0].ToUpper())
            {
                case "CHANMSG":
                    if (parts.Length < 3)
                        client.SendErrorArgLength(parts[0].ToUpper(), 3, parts.Length);
                    else
                        handleChanMsg(client, parts[1], sliceArray(parts, 2, parts.Length));
                    break;
                case "PRIVMSG":
                    if (parts.Length < 3)
                        client.SendErrorArgLength(parts[0].ToUpper(), 3, parts.Length);
                    else
                        handlePrivMsg(client, parts[1], sliceArray(parts, 2, parts.Length));
                    break;
            }
        }

        private void handleChanMsg(BroadcastChatClient client, string channel, string message)
        {
            if (!server.Channels.ContainsKey(channel))
                client.SendErrorNoChannel(channel);
            else if (!client.Channels.Contains(channel))
                client.SendErrorNotInChannel(channel);
            else
                server.Channels[channel].SendChanMsg(client, message);
        }

        private void handlePrivMsg(BroadcastChatClient client, string receiver, string message)
        {
            if (!server.Clients.ContainsKey(receiver))
                client.SendErrorNoNick(receiver);
            else
                server.Clients[receiver].SendPrivMsg(client.Nick, message);
        }

        private string sliceArray(string[] arr, int start, int end)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = start; i < end; i++)
                sb.Append(arr[i]);

            return sb.ToString();
        }
    }
}


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
                        handleChanMsg(client, parts[1], sliceArray(parts, 2, parts.Length, " "));
                    break;
                case "JOIN":
                    if (parts.Length < 2)
                        client.SendErrorArgLength(parts[0].ToLower(), 2, parts.Length);
                    else
                        handleJoin(client, parts[1]);
                    break;
                case "LEAVE":
                    if (parts.Length < 3)
                        client.SendErrorArgLength(parts[0].ToUpper(), 3, parts.Length);
                    else
                        handleLeave(client, parts[1], sliceArray(parts, 2, parts.Length, " "));
                    break;
                case "NICK":
                    if (parts.Length < 2)
                        client.SendErrorArgLength(parts[0].ToUpper(), 2, parts.Length);
                    else
                        handleNick(client, parts[1]);
                    break;
                case "PRIVMSG":
                    if (parts.Length < 3)
                        client.SendErrorArgLength(parts[0].ToUpper(), 3, parts.Length);
                    else
                        handlePrivMsg(client, parts[1], sliceArray(parts, 2, parts.Length, " "));
                    break;
                case "QUIT":
                    if (parts.Length < 2)
                        client.SendErrorArgLength(parts[0].ToUpper(), 2, parts.Length);
                    else
                        handleQuit(client, sliceArray(parts, 1, parts.Length, " "));
                    break;
            }
        }

        private void handleChanMsg(BroadcastChatClient client, string channel, string message)
        {
            if (!server.Channels.ContainsKey(channel))
                client.SendErrorNoChannel(channel);
            else if (!client.Channels.ContainsKey(channel))
                client.SendErrorNotInChannel(channel);
            else
                server.Channels[channel].SendChanMsg(client.Nick, message);
        }
        public void handleJoin(BroadcastChatClient client, string channel)
        {
            if (!channel.StartsWith("#"))
                client.SendErrorChannelName(channel);
            else
            {
                if (!server.Channels.ContainsKey(channel))
                    server.Channels.Add(channel, new BroadcastChatChannel(channel));
                if (server.Channels[channel].Clients.ContainsKey(client.Nick))
                    client.SendErrorInChannel(channel);
                else
                    server.Channels[channel].SendJoin(client);
            }
        }
        private void handleLeave(BroadcastChatClient client, string channel, string reason)
        {
            if (!client.Channels.ContainsKey(channel))
                client.SendErrorNotInChannel(channel);
            else if (!server.Channels.ContainsKey(channel))
                client.SendErrorNoChannel(channel);
            server.Channels[channel].SendLeave(client.Nick, reason);
        }
        private void handleNick(BroadcastChatClient client, string newNick)
        {
            if (server.Clients.ContainsKey(newNick))
                client.SendErrorNickExists(newNick);
            else
                client.ChangeNick(newNick);
        }
        private void handlePrivMsg(BroadcastChatClient client, string receiver, string message)
        {
            if (!server.Clients.ContainsKey(receiver))
                client.SendErrorNoNick(receiver);
            else
                server.Clients[receiver].SendPrivMsg(client.Nick, message);
        }
        private void handleQuit(BroadcastChatClient client, string reason)
        {
            client.Quit(reason);
        }

        private string sliceArray(string[] arr, int start, int end, string sep = "")
        {
            StringBuilder sb = new StringBuilder();

            for (int i = start; i < end; i++)
                sb.AppendFormat("{0}{1}", arr[i], sep);

            return sb.ToString();
        }
    }
}


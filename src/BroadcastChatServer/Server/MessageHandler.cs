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

            // We can't have clients doing anything until we know who they are.
            if (client.Nick == null && parts[0].ToUpper() != "NICK")
            {
                client.SendErrorNickNotSet();
                return;
            }

            switch (parts[0].ToUpper())
            {
                case "CHANLIST":
                    handleChanList(client);
                    break;
                case "CHANMSG":
                    if (parts.Length < 3)
                        client.SendErrorArgLength(parts[0].ToUpper(), 3, parts.Length);
                    else
                        handleChanMsg(client, parts[1], sliceArray(parts, 2, parts.Length, " "));
                    break;
                case "CHANOPER":
                    if (parts.Length < 4)
                        client.SendErrorArgLength(parts[0].ToUpper(), 4, parts.Length);
                    else
                        handleChanOper(client, parts[1], parts[2], parts[3]);
                    break;
                case "JOIN":
                    if (parts.Length < 2)
                        client.SendErrorArgLength(parts[0].ToUpper(), 2, parts.Length);
                    else
                        handleJoin(client, parts[1]);
                    break;
                case "KICK":
                    if (parts.Length < 3)
                        client.SendErrorArgLength(parts[0].ToUpper(), 3, parts.Length);
                    else
                        handleKick(client, parts[1], parts[2]);
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
                case "TOPIC":
                    if (parts.Length < 3)
                        client.SendErrorArgLength(parts[0].ToUpper(), 3, parts.Length);
                    else
                        handleTopic(client, parts[1], sliceArray(parts, 2, parts.Length, " "));
                    break;
                case "USERLIST":
                    if (parts.Length < 2)
                        client.SendErrorArgLength(parts[0].ToUpper(), 2, parts.Length);
                    else
                        handleUserList(client, parts[1]);
                    break;
            }
        }

        private void handleChanList(BroadcastChatClient client)
        {
            StringBuilder sb = new StringBuilder();
    
            foreach (string chan in server.Channels.Keys)
                sb.AppendFormat("{0} ", chan);

            client.SendChanList(sb.ToString());
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
        private void handleChanOper(BroadcastChatClient client, string channel, string mod, string target)
        {
            if (!server.Channels.ContainsKey(channel))
                client.SendErrorNoChannel(channel);
            else if (!server.Channels[channel].Clients.ContainsKey(client.Nick))
                client.SendErrorNotInChannel(channel);
            else if (!server.Channels[channel].OperClients.ContainsKey(client.Nick))
                client.SendErrorNotChanOper(channel, client.Nick);
            else if (mod.ToUpper() == "GIVE")
            {
                if (server.Channels[channel].OperClients.ContainsKey(target))
                    client.SendErrorAlreadyChanOper(channel, target);
                else
                    server.Channels[channel].SendChanOperGive(client, target);
            }
            else if (mod.ToUpper() == "TAKE")
            {
                if (!server.Channels[channel].OperClients.ContainsKey(target))
                    client.SendErrorNotChanOper(channel, target);
                else
                    server.Channels[channel].SendChanOperTake(client, target);
            }
            else
                client.SendErrorExpected(mod.ToUpper(), "GIVE", "TAKE");
        }
        private void handleJoin(BroadcastChatClient client, string channel)
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
                {
                    var chan = server.Channels[channel];
                    chan.SendJoin(client);
                    client.SendTopic(chan.Name, chan.TopicSetter, chan.Topic);
                }
            }
        }
        private void handleKick(BroadcastChatClient client, string channel, string kicked)
        {
            if (!server.Channels.ContainsKey(channel))
                client.SendErrorNoChannel(channel);
            else if (!server.Clients.ContainsKey(kicked))
                client.SendErrorNoNick(kicked);
            else if (!server.Channels[channel].Clients.ContainsKey(kicked))
                client.SendErrorUserNotInChannel(channel, kicked);
            else if (!server.Channels[channel].OperClients.ContainsKey(client.Nick))
                client.SendErrorNotChanOper(channel, client.Nick);
            else
                server.Channels[channel].SendKick(client.Nick, server.Clients[kicked]);
        }
        private void handleLeave(BroadcastChatClient client, string channel, string reason)
        {
            if (!client.Channels.ContainsKey(channel))
                client.SendErrorNotInChannel(channel);
            else if (!server.Channels.ContainsKey(channel))
                client.SendErrorNoChannel(channel);
            else
            {
                server.Channels[channel].SendLeave(client.Nick, reason);
                client.Channels.Remove(channel);
            }
        }
        private void handleNick(BroadcastChatClient client, string newNick)
        {
            if (server.Clients.ContainsKey(newNick))
                client.SendErrorNickExists(newNick);
            else
            {
                if (client.Nick == null)
                    server.Clients.Add(newNick, client);
                else
                {
                    var temp = server.Clients[client.Nick];
                    server.Clients.Remove(client.Nick);
                    server.Clients.Add(newNick, client);
                }
                client.ChangeNick(newNick);
            }
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
        private void handleTopic(BroadcastChatClient client, string channel, string newTopic)
        {
            if (!server.Channels.ContainsKey(channel))
                client.SendErrorNoChannel(channel);
            else if (!server.Channels[channel].Clients.ContainsKey(client.Nick))
                client.SendErrorNotInChannel(channel);
            else if (!server.Channels[channel].OperClients.ContainsKey(client.Nick))
                client.SendErrorNotChanOper(channel, client.Nick);
            else
                server.Channels[channel].SendTopic(client, newTopic);
        }
        private void handleUserList(BroadcastChatClient client, string channel)
        {
            if (!server.Channels.ContainsKey(channel))
                client.SendErrorNoChannel(channel);
            else if (!client.Channels.ContainsKey(channel))
                client.SendErrorNotInChannel(channel);
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (string cl in server.Channels[channel].Clients.Keys)
                    sb.AppendFormat(server.Channels[channel].OperClients.ContainsKey(cl) ? "@{0} " : "{0} ", cl);
                client.SendUserList(channel, sb.ToString());
            }
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


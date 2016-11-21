using System;
using System.Collections.Generic;

using BroadcastChatServer.Networking;

namespace BroadcastChatServer.Server
{
    public class BroadcastChatChannel
    {
        public Dictionary<string, BroadcastChatClient> Clients { get; private set; }
        public Dictionary<string, BroadcastChatClient> BannedClients { get; private set; }
        public Dictionary<string, BroadcastChatClient> OperClients { get; private set; }

        public string Name { get; private set; }
        public string Topic { get; private set; }
        public string TopicSetter { get; private set; }

        public BroadcastChatChannel(string name)
        {
            Clients = new Dictionary<string, BroadcastChatClient>();
            BannedClients = new Dictionary<string, BroadcastChatClient>();
            OperClients = new Dictionary<string, BroadcastChatClient>();
            Name = name;
            Topic = string.Empty;
            TopicSetter = string.Empty;
        }

        public void SendBan(string banner, BroadcastChatClient banned)
        {
            foreach (var client in Clients.Values)
                client.SendBan(Name, banner, banned.Nick);
            BannedClients.Add(banned.Nick, banned);
        }
        public void SendChanMsg(string sender, string message)
        {
            foreach (var client in Clients.Values)
                client.SendChanMsg(Name, sender, message);
        }
        public void SendChanOperGive(BroadcastChatClient cl, string target)
        {
            foreach (var client in Clients.Values)
                client.SendChanOperGive(Name, cl.Nick, target);
            OperClients.Add(target, Clients[target]);
        }
        public void SendChanOperTake(BroadcastChatClient cl, string target)
        {
            foreach (var client in Clients.Values)
                client.SendChanOperTake(Name, cl.Nick, target);
            OperClients.Remove(target);
        }
        public void SendJoin(BroadcastChatClient cl)
        {
            if (Clients.Count == 0)
                OperClients.Add(cl.Nick, cl);
            Clients.Add(cl.Nick, cl);
            cl.Channels.Add(Name, this);
            foreach (var client in Clients.Values)
                client.SendJoin(Name, cl.Nick);
        }
        public void SendKick(string kicker, BroadcastChatClient kicked)
        {
            foreach (var client in Clients.Values)
                client.SendKick(Name, kicked.Nick, kicker);
            Clients.Remove(kicked.Nick);
            if (OperClients.ContainsKey(kicked.Nick))
                OperClients.Remove(kicked.Nick);
            kicked.Channels.Remove(Name);
        }
        public void SendLeave(string nick, string reason)
        {
            foreach (var client in Clients.Values)
                client.SendLeave(Name, nick, reason);
            Clients.Remove(nick);
            if (OperClients.ContainsKey(nick))
                OperClients.Remove(nick);
        }
        public void SendNickChange(string oldNick, string newNick)
        {
            var temp = Clients[oldNick];
            Clients.Remove(oldNick);
            Clients.Add(newNick, temp);

            if (OperClients.ContainsKey(oldNick))
            {
                temp = OperClients[oldNick];
                OperClients.Remove(oldNick);
                OperClients.Add(newNick, temp);
            }

            if (BannedClients.ContainsKey(oldNick))
            {
                temp = BannedClients[oldNick];
                BannedClients.Remove(oldNick);
                BannedClients.Add(newNick, temp);
            }

            foreach (var client in Clients.Values)
                client.SendNickChange(Name, oldNick, newNick);
        }
        public void SendQuit(BroadcastChatClient cl, string reason)
        {
            Clients.Remove(cl.Nick);
            if (OperClients.ContainsKey(cl.Nick))
                OperClients.Remove(cl.Nick);
            foreach (var client in Clients.Values)
                client.SendQuit(Name, cl.Nick, reason);
        }
        public void SendTopic(BroadcastChatClient cl, string newTopic)
        {
            foreach (var client in Clients.Values)
                client.SendTopic(Name, cl.Nick, newTopic);
            Topic = newTopic;
            TopicSetter = cl.Nick;
        }
        public void SendUnban(string unbanner, string unbanned)
        {
            foreach (var client in Clients.Values)
                client.SendUnban(Name, unbanner, unbanned);
            BannedClients.Remove(unbanned);
        }
    }
}


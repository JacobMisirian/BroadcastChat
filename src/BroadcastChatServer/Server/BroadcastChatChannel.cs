using System;
using System.Collections.Generic;

using BroadcastChatServer.Networking;

namespace BroadcastChatServer.Server
{
    public class BroadcastChatChannel
    {
        public Dictionary<string, BroadcastChatClient> Clients { get; private set; }
        public Dictionary<string, BroadcastChatClient> OperClients { get; private set; }

        public string Name { get; private set; }
        public string Topic { get; private set; }
        public string TopicSetter { get; private set; }

        public BroadcastChatChannel(string name)
        {
            Clients = new Dictionary<string, BroadcastChatClient>();
            OperClients = new Dictionary<string, BroadcastChatClient>();
            Name = name;
            Topic = string.Empty;
            TopicSetter = string.Empty;
        }

        public void SendChanMsg(string sender, string message)
        {
            foreach (var client in Clients.Values)
                client.SendChanMsg(Name, sender, message);
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

            foreach (var client in Clients.Values)
                client.SendNickChange(Name, oldNick, newNick);
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
        public void SendTopic(BroadcastChatClient cl, string newTopic)
        {
            foreach (var client in Clients.Values)
                client.SendTopic(Name, cl.Nick, newTopic);
            Topic = newTopic;
            TopicSetter = cl.Nick;
        }
        public void SendQuit(BroadcastChatClient cl, string reason)
        {
            Clients.Remove(cl.Nick);
            if (OperClients.ContainsKey(cl.Nick))
                OperClients.Remove(cl.Nick);
            foreach (var client in Clients.Values)
                client.SendQuit(Name, cl.Nick, reason);
        }
    }
}


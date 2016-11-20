using System;
using System.Collections.Generic;

using BroadcastChatServer.Networking;

namespace BroadcastChatServer.Server
{
    public class BroadcastChatChannel
    {
        public Dictionary<string, BroadcastChatClient> Clients { get; private set; }
        public string Name { get; private set; }

        public BroadcastChatChannel(string name)
        {
            Clients = new Dictionary<string, BroadcastChatClient>();
            Name = name;
        }

        public void SendChanMsg(string sender, string message)
        {
            foreach (var client in Clients.Values)
                client.SendChanMsg(Name, sender, message);
        }
        public void SendJoin(BroadcastChatClient cl)
        {
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
        }
        public void SendNickChange(string oldNick, string newNick)
        {
            var temp = Clients[oldNick];
            Clients.Remove(oldNick);
            Clients.Add(newNick, temp);

            foreach (var client in Clients.Values)
                client.SendNickChange(Name, oldNick, newNick);
        }
        public void SendQuit(BroadcastChatClient cl, string reason)
        {
            Clients.Remove(cl.Nick);
            foreach (var client in Clients.Values)
                client.SendQuit(Name, cl.Nick, reason);
        }
    }
}


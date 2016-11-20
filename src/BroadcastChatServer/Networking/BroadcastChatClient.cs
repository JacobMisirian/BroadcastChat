using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using BroadcastChatServer.Server;

namespace BroadcastChatServer.Networking
{
    public class BroadcastChatClient
    {
        public TcpClient TcpClient { get; private set; }

        public StreamReader StreamReader { get; private set; }
        public StreamWriter StreamWriter { get; private set; }

        public int Ping { get; set; }

        public Thread ListenThread { get; set; }
        public Thread PingThread { get; set; }

        public Dictionary<string, BroadcastChatChannel> Channels { get; private set; }

        public string Nick { get; private set; }

        public BroadcastChatClient(TcpClient client)
        {
            TcpClient = client;

            StreamReader = new StreamReader(client.GetStream());
            StreamWriter = new StreamWriter(client.GetStream());

            Ping = 0;

            Channels = new Dictionary<string, BroadcastChatChannel>();
        }

        public void ChangeNick(string newNick)
        {
            foreach (var channel in Channels.Values)
                channel.SendNickChange(Nick, newNick);
            Nick = newNick;
        }
        public void Quit(string reason)
        {
            foreach (var channel in Channels.Values)
                channel.SendQuit(this, reason);

            ListenThread.Abort();
            PingThread.Abort();
            TcpClient.Close();
        }

        public void Send(string msg, params object[] args)
        {
            StreamWriter.WriteLine(string.Format(msg, args));
            StreamWriter.Flush();
        }

        public void SendChanMsg(string channel, string sender, string message)
        {
            Send("CHANMSG {0} {1} {2}", channel, sender, message);
        }       
        public void SendJoin(string channel, string nick)
        {
            Send("JOIN {0} {1}", channel, nick);
        }
        public void SendLeave(string channel, string nick, string reason)
        {
            Send("LEAVE {0} {1} {2}", channel, nick, reason);
        }
        public void SendNickChange(string channel, string oldNick, string newNick)
        {
            Send("NICK {0} {1} {2}", channel, oldNick, newNick);
        }
        public void SendPrivMsg(string sender, string message)
        {
            Send("PRIVMSG {0} {1}", sender, message);
        }
        public void SendQuit(string channel, string nick, string message)
        {
            Send("QUIT {0} {1} {2}", channel, nick, message);
        }

        public void SendError(string msg, params object[] args)
        {
            Send("ERROR {0}", string.Format(msg, args));
        }
        public void SendErrorArgLength(string baseCmd, int expected, int given)
        {
            SendError("Command {0} expects {1} argument(s), given: {2}", baseCmd, expected, given);
        }
        public void SendErrorChannelName(string channel)
        {
            SendError("Channel names must start with #, got {0}", channel);
        }
        public void SendErrorInChannel(string channel)
        {
            SendError("Already in channel {0}", channel);
        }
        public void SendErrorNickExists(string nick)
        {
            SendError("Nick already exists {0}", nick);
        }
        public void SendErrorNoChannel(string channel)
        {
            SendError("No such channel {0}", channel);
        }
        public void SendErrorNoNick(string nick)
        {
            SendError("No such nick {0}", nick);
        }
        public void SendErrorNotInChannel(string channel)
        {
            SendError("Not in channel {0}", channel);
        }
        public void SendErrorChannelExists(string channel)
        {
            SendError("Channel already exists {0}", channel);
        }

        public string Read()
        {
            return StreamReader.ReadLine();
        }
    }
}


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

        public List<string> Channels { get; private set; }

        public string Nick { get; set; }

        public BroadcastChatClient(TcpClient client)
        {
            TcpClient = client;

            StreamReader = new StreamReader(client.GetStream());
            StreamWriter = new StreamWriter(client.GetStream());

            Ping = 0;

            Channels = new List<string>();
        }

        public void Send(string msg, params object[] args)
        {
            StreamWriter.WriteLine(string.Format(msg, args));
            StreamWriter.Flush();
        }

        public void SendChanJoin(string channel, string nick)
        {
            Send("CHANJOIN {0} {1}", channel, nick);
        }
        public void SendChanLeave(string channel, string nick)
        {
            Send("CHANLEAVE {0} {1}", channel, nick);
        }
        public void SendChanMsg(string channel, string sender, string message)
        {
            Send("CHANMSG {0} {1} {2}", channel, sender, message);
        }
        public void SendNickChange(string channel, string oldNick, string newNick)
        {
            Send("NICKCHANGE {0} {1} {2}", channel, oldNick, newNick);
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
        public void SendErrorNoChannel(string channel)
        {
            SendError("No such channel {0}", channel);
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


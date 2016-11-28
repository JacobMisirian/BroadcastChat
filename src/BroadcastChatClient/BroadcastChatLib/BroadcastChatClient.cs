using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using BroadcastChatClient.BroadcastChatLib.Events;

namespace BroadcastChatClient.BroadcastChatLib
{
    public class BroadcastChatClient
    {
        public event EventHandler<BanListMessageReceivedEventArgs> BanListMessageReceived;
        public event EventHandler<BanMessageReceivedEventArgs> BanMessageReceived;
        public event EventHandler<ChanListMessageReceivedEventargs> ChanListMessageReceived;
        public event EventHandler<ChanMsgMessageReceivedEventArgs> ChanMsgMessageReceived;
        public event EventHandler<ChanOperGiveMessageReceivedEventArgs> ChanOperGiveMessageReceived;
        public event EventHandler<ChanOperTakeMessageReceivedEventArgs> ChanOperTakeMessageReceived;
        public event EventHandler<ConnectedToServerEventArgs> ConnectedToServer;
        public event EventHandler<DisconnectedFromServerEventArgs> DisconnectedFromServer;
        public event EventHandler<ErrorMessageReceivedEventArgs> ErrorMessageReceived;
        public event EventHandler<JoinMessageReceivedEventArgs> JoinMessageReceived;
        public event EventHandler<KickMessageReceivedEventArgs> KickMessageReceived;
        public event EventHandler<LeaveMessageReceivedEventArgs> LeaveMessageReceived;
        public event EventHandler<MotdMessageReceivedEventArgs> MotdMessageReceived;
        public event EventHandler<NameMessageReceivedEventArgs> NameMessageReceived;
        public event EventHandler<NickMessageReceivedEventArgs> NickMessageReceived;
        public event EventHandler<PrivMsgMessageReceivedEventArgs> PrivMsgMessageReceived;
        public event EventHandler<QuitMessageReceivedEventArgs> QuitMessageReceived;
        public event EventHandler<TopicMessageReceivedEventArgs> TopicMessageReceived;
        public event EventHandler<UnbanMessageReceivedEventArgs> UnbanMessageReceived;
        public event EventHandler<WhoisMessageReceivedEventArgs> WhoisMessageReceived;

        private string host;
        private int port;

        private StreamReader reader;
        private StreamWriter writer;

        public BroadcastChatClient(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public bool Connect(int timeout = 20)
        {
            TcpClient client = new TcpClient(host, port);

            for (int i = 0; !client.Connected; Thread.Sleep(++i * 1000))
                if (i >= timeout)
                    return false;

            var stream = client.GetStream();

            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);

            new Thread(() => listenThread()).Start();

            return true;
        }

        public void Send(string formatStr, params string[] args)
        {
            writer.WriteLine(string.Format(formatStr, args));
            writer.Flush();
        }

        public void SendBan(string channel, string user)
        {
            Send("BAN {0} {1}", channel, user);
        }
        public void SendBanList(string channel)
        {
            Send("BANLIST {0}", channel);
        }
        public void SendChanList()
        {
            Send("CHANLIST");
        }
        public void SendChanMsg(string channel, string message)
        {
            Send("CHANMSG {0} {1}", channel, message);
        }
        public void SendChanOper(string channel, string mod, string target)
        {
            Send("CHANOPER {0} {1} {2}", channel, mod, target);
        }
        public void SendJoin(string channel)
        {
            Send("JOIN {0}", channel);
        }
        public void SendKick(string channel, string user)
        {
            Send("KICK {0} {1}", channel, user);
        }
        public void SendLeave(string channel, string reason)
        {
            Send("LEAVE {0} {1}", channel, reason);
        }
        public void SendNick(string newNick)
        {
            Send("NICK {0}", newNick);
        }
        public void SendPrivMsg(string receiver, string message)
        {
            Send("PRIVMSG {0} {1}", receiver, message);
        }
        public void SendQuit(string reason)
        {
            Send("QUIT {0}", reason);
        }
        public void SendTopic(string channel, string newTopic)
        {
            Send("TOPIC {0} {1}", channel, newTopic);
        }
        public void SendUnban(string channel, string user)
        {
            Send("UNBAN {0} {1}", channel, user);
        }
        public void SendUserList(string channel)
        {
            Send("USERLIST {0}", channel);
        }
        public void SendWhois(string user)
        {
            Send("WHOIS {0}", user);
        }

        private void listenThread()
        {
            try
            {
                while (true)
                {
                    string rawMessage = reader.ReadLine();
                    string[] parts = reader.ReadLine().Split(' ');

                    switch (parts[0].ToUpper())
                    {
                        case "PING":
                            Send("PONG");
                            break;
                        case "BAN":
                            OnBanMessageReceived(new BanMessageReceivedEventArgs(rawMessage, parts[1], parts[2], parts[3]));
                            break;
                        case "BANLIST":
                            OnBanListMessageReceived(new BanListMessageReceivedEventArgs(rawMessage, parts[1], splitArray(parts, 2)));
                            break;
                        case "CHANLIST":
                            OnChanListMessageReceived(new ChanListMessageReceivedEventargs(rawMessage, splitArray(parts, 1)));
                            break;
                        case "CHANMSG":
                            OnChanMsgMessageReceived(new ChanMsgMessageReceivedEventArgs(rawMessage, parts[1], parts[2], splitArray(parts, 3)));
                            break;
                        case "CHANOPER":
                            if (parts[2].ToUpper() == "GIVE")
                                OnChanOperGiveMessageReceived(new ChanOperGiveMessageReceivedEventArgs(rawMessage, parts[1], parts[4], parts[3]));
                            else if (parts[2].ToUpper() == "TAKE")
                                OnChanOperTakeMessageReceived(new ChanOperTakeMessageReceivedEventArgs(rawMessage, parts[1], parts[4], parts[3]));
                            break;
                        case "ERROR":
                            OnErrorMessageReceived(new ErrorMessageReceivedEventArgs(rawMessage, splitArray(parts, 1)));
                            break;
                        case "JOIN":
                            OnJoinMessageReceived(new JoinMessageReceivedEventArgs(rawMessage, parts[1], parts[2]));
                            break;
                        case "KICK":
                            OnKickMessageReceived(new KickMessageReceivedEventArgs(rawMessage, parts[1], parts[2], parts[3]));
                            break;
                        case "LEAVE":
                            OnLeaveMessageReceived(new LeaveMessageReceivedEventArgs(rawMessage, parts[1], parts[2], splitArray(parts, 3)));
                            break;
                        case "MOTD":
                            OnMotdMessageReceived(new MotdMessageReceivedEventArgs(rawMessage, splitArray(parts, 1)));
                            break;
                        case "NAME":
                            OnNameMessageReceived(new NameMessageReceivedEventArgs(rawMessage, splitArray(parts, 1)));
                            break;
                        case "NICK":
                            OnNickMessageReceived(new NickMessageReceivedEventArgs(rawMessage, parts[1], parts[2], parts[3]));
                            break;
                        case "PRIVMSG":
                            OnPrivMsgMessageReceived(new PrivMsgMessageReceivedEventArgs(rawMessage, parts[1], splitArray(parts, 2)));
                            break;
                        case "QUIT":
                            OnQuitMessageReceived(new QuitMessageReceivedEventArgs(rawMessage, parts[1], parts[2], splitArray(parts, 3)));
                            break;
                        case "TOPIC":
                            OnTopicMessageReceived(new TopicMessageReceivedEventArgs(rawMessage, parts[1], splitArray(parts, 2)));
                            break;
                        case "UNBAN":
                            OnUnbanMessageReceived(new UnbanMessageReceivedEventArgs(rawMessage, parts[1], parts[3], parts[2]));
                            break;
                        case "WHOIS":
                            OnWhoisMessageReceived(new WhoisMessageReceivedEventArgs(rawMessage, parts[1], splitArray(parts, 2)));
                            break;
                    }
                }
            }
            catch (IOException)
            {
                OnDisconnectedFromServer(new DisconnectedFromServerEventArgs());
            }
        }

        private string splitArray(string[] arr, int start, int end = -1, string sep = " ")
        {
            end = end == -1 ? arr.Length : end;

            StringBuilder sb = new StringBuilder();

            for (int i = start; i < end; i++)
                sb.AppendFormat("{0}{1}", arr[i], sep);

            return sb.ToString();
        }

        protected virtual void OnBanListMessageReceived(BanListMessageReceivedEventArgs e)
        {
            var handler = BanListMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnBanMessageReceived(BanMessageReceivedEventArgs e)
        {
            var handler = BanMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnChanListMessageReceived(ChanListMessageReceivedEventargs e)
        {
            var handler = ChanListMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnChanMsgMessageReceived(ChanMsgMessageReceivedEventArgs e)
        {
            var handler = ChanMsgMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnChanOperGiveMessageReceived(ChanOperGiveMessageReceivedEventArgs e)
        {
            var handler = ChanOperGiveMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnChanOperTakeMessageReceived(ChanOperTakeMessageReceivedEventArgs e)
        {
            var handler = ChanOperTakeMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnConnectedToServer(ConnectedToServerEventArgs e)
        {
            var handler = ConnectedToServer;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnDisconnectedFromServer(DisconnectedFromServerEventArgs e)
        {
            var handler = DisconnectedFromServer;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnErrorMessageReceived(ErrorMessageReceivedEventArgs e)
        {
            var handler = ErrorMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnJoinMessageReceived(JoinMessageReceivedEventArgs e)
        {
            var handler = JoinMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnKickMessageReceived(KickMessageReceivedEventArgs e)
        {
            var handler = KickMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnLeaveMessageReceived(LeaveMessageReceivedEventArgs e)
        {
            var handler = LeaveMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnMotdMessageReceived(MotdMessageReceivedEventArgs e)
        {
            var handler = MotdMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnNameMessageReceived(NameMessageReceivedEventArgs e)
        {
            var handler = NameMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnNickMessageReceived(NickMessageReceivedEventArgs e)
        {
            var handler = NickMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnPrivMsgMessageReceived(PrivMsgMessageReceivedEventArgs e)
        {
            var handler = PrivMsgMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnQuitMessageReceived(QuitMessageReceivedEventArgs e)
        {
            var handler = QuitMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnTopicMessageReceived(TopicMessageReceivedEventArgs e)
        {
            var handler = TopicMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnUnbanMessageReceived(UnbanMessageReceivedEventArgs e)
        {
            var handler = UnbanMessageReceived;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnWhoisMessageReceived(WhoisMessageReceivedEventArgs e)
        {
            var handler = WhoisMessageReceived;
            if (handler != null)
                handler(this, e);
        }
    }
}


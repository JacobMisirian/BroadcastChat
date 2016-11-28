using System;

using BroadcastChatClient.BroadcastChatLib;
using BroadcastChatClient.BroadcastChatLib.Events;

namespace BroadcastChatClient.TUIClient
{
    public class ClientTUI
    {
        private BroadcastChatClient.BroadcastChatLib.BroadcastChatClient client;

        private string currentChannel;
        private string serverName;

        public ClientTUI(string host, int port)
        {
            client = new BroadcastChatClient.BroadcastChatLib.BroadcastChatClient(host, port);

            client.BanListMessageReceived += client_BanListMessageReceived;
            client.BanMessageReceived += client_BanMessageReceived;
            client.ChanListMessageReceived += client_ChanListMessageReceived;
            client.ChanMsgMessageReceived += client_ChanMsgMessageReceived;
            client.ChanOperGiveMessageReceived += client_ChanOperGiveMessageReceived;
            client.ChanOperTakeMessageReceived += client_ChanOperTakeMessageReceived;
            client.ConnectedToServer += client_ConnectedToServer;
            client.DisconnectedFromServer += client_DisconnectedFromServer;
            client.ErrorMessageReceived += client_ErrorMessageReceived;
            client.JoinMessageReceived += client_JoinMessageReceived;
            client.KickMessageReceived += client_KickMessageReceived;
            client.LeaveMessageReceived += client_LeaveMessageReceived;
            client.MotdMessageReceived += client_MotdMessaageReceived;
            client.NameMessageReceived += client_NameMessageReceived;
            client.NickMessageReceived += client_NickMessageReceived;
            client.PrivMsgMessageReceived += client_PrivMsgMessageReceived;
            client.QuitMessageReceived += client_QuitMessageReceived;
            client.TopicMessageReceived += client_TopicMessageReceived;
            client.UnbanMessageReceived += client_UnbanMessageReceived;
            client.WhoisMessageReceived += client_WhoisMessageReceived;
        }

        public void Start(int timeout = 20)
        {
            client.Connect(timeout);

            while (true)
            {
                Console.Write("> ");
                handleMessage(Console.ReadLine());
            }
        }

        private void client_BanListMessageReceived(object sender, BanListMessageReceivedEventArgs e)
        {
            Console.Write("<{0}> Ban list for {1}: ", serverName, e.Channel);
            foreach (var nick in e.BanList)
                Console.Write("{0} ", nick);
            Console.WriteLine();
        }
        private void client_BanMessageReceived(object sender, BanMessageReceivedEventArgs e)
        {
            if (e.Channel == currentChannel)
                Console.WriteLine("<{0}> {1} was banned by {2}", e.Channel, e.User, e.Banner);
        }
        private void client_ChanListMessageReceived(object sender, ChanListMessageReceivedEventargs e)
        {
            Console.Write("<{0}> Channels: ", serverName);
            foreach (var channel in e.Channels)
                Console.Write("{0} ", channel);
            Console.WriteLine();
        }
        private void client_ChanMsgMessageReceived(object sender, ChanMsgMessageReceivedEventArgs e)
        {
            if (currentChannel == e.Channel)
                Console.WriteLine("<{0}> {1}", e.Sender, e.Message);
        }
        private void client_ChanOperGiveMessageReceived(object sender, ChanOperGiveMessageReceivedEventArgs e)
        {
            if (currentChannel == e.Channel)
                Console.WriteLine("<{0}> {1} was made oper by {2}", e.Channel, e.User, e.Giver);
        }
        private void client_ChanOperTakeMessageReceived(object sender, ChanOperTakeMessageReceivedEventArgs e)
        {
            if (currentChannel == e.Channel)
                Console.WriteLine("<{0}> {1} had oper taken by {1}", e.Channel, e.User, e.Taker);
        }
        private void client_ConnectedToServer(object sender, ConnectedToServerEventArgs e)
        {
            Console.WriteLine("Connected to {0}:{1}!", e.Host, e.Port);
        }
        private void client_DisconnectedFromServer(object sender, DisconnectedFromServerEventArgs e)
        {
            Console.WriteLine("Disconnected from server!");
        }
        private void client_ErrorMessageReceived(object sender, ErrorMessageReceivedEventArgs e)
        {
            Console.WriteLine("<{0}> {1}", serverName, e.Error);
        }
        private void client_JoinMessageReceived(object sender, JoinMessageReceivedEventArgs e)
        {
            if (e.Channel == currentChannel)
                Console.WriteLine("<{0}> {1} has joined!", e.Channel, e.Nick);
        }
        private void client_KickMessageReceived(object sender, KickMessageReceivedEventArgs e)
        {
            if (e.Channel == currentChannel)
                Console.WriteLine("<{0}> {1} has been kicked by {2}", e.Channel, e.User, e.Kicker);
        }
        private void client_LeaveMessageReceived(object sender, LeaveMessageReceivedEventArgs e)
        {
            if (e.Channel == currentChannel)
                Console.WriteLine("<{0}> {1} has left! Reason: {2}", e.Channel, e.User, e.Reason);
        }
        private void client_MotdMessaageReceived(object sender, MotdMessageReceivedEventArgs e)
        {
            Console.WriteLine("<{0}> MOTD: {1}", serverName, e.Motd);
        }
        private void client_NameMessageReceived(object sender, NameMessageReceivedEventArgs e)
        {
            serverName = e.ServerName;
        }
        private void client_NickMessageReceived(object sender, NickMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.Channel);
            if (e.Channel == currentChannel)
                Console.WriteLine("<{0}> {1} is now known as {2}!", e.Channel, e.OldNick, e.NewNick);
        }
        private void client_PrivMsgMessageReceived(object sender, PrivMsgMessageReceivedEventArgs e)
        {
            Console.WriteLine("<{0}> {1}", e.Sender, e.Message);
        }
        private void client_QuitMessageReceived(object sender, QuitMessageReceivedEventArgs e)
        {
            if (e.Channel == currentChannel)
                Console.WriteLine("<{0}> User {1} has quit. Reason: {2}", serverName, e.User, e.Reason);
        }
        private void client_TopicMessageReceived(object sender, TopicMessageReceivedEventArgs e)
        {
            if (e.Channel == currentChannel)
                Console.WriteLine("<{0}> Topic: ", e.Channel, e.Topic);
        }
        private void client_UnbanMessageReceived(object sender, UnbanMessageReceivedEventArgs e)
        {
            if (e.Channel == currentChannel)
                Console.WriteLine("<{0}> {1} has been unbanned by {2}", e.Channel, e.User, e.Unbanner);
        }
        private void client_WhoisMessageReceived(object sender, WhoisMessageReceivedEventArgs e)
        {
            Console.WriteLine("<{0}> WHOIS: {1}\n{2}", serverName, e.User, e.Whois);
        }

        private void handleMessage(string message)
        {
            if (message.StartsWith("/"))
            {
                string[] parts = message.Substring(1).Split(' ');

                switch (parts[0].ToUpper())
                {
                    case "BAN":
                        client.SendBan(currentChannel, parts[1]);
                        break;
                    case "BANLIST":
                        client.SendBanList(currentChannel);
                        break;
                    case "CHANLIST":
                        client.SendChanList();
                        break;
                    case "CHANOPER":
                        client.SendChanOper(parts[1], parts[2], parts[3]);
                        break;
                    case "JOIN":
                        currentChannel = parts[1];
                        client.SendJoin(parts[1]);
                        break;
                    case "KICK":
                        client.SendKick(currentChannel, parts[1]);
                        break;
                    case "LEAVE":
                        client.SendLeave(currentChannel, message.Substring(message.IndexOf(" ") + 1));
                        break;
                    case "NICK":
                        client.SendNick(parts[1]);
                        break;
                    case "QUERY":
                        currentChannel = parts[1];
                        break;
                    case "QUIT":
                        client.SendQuit(message.Substring(message.IndexOf(" ") + 1));
                        break;
                    case "RAW":
                        client.Send(message.Substring(message.IndexOf(" ") + 1));
                        break;
                    case "TOPIC":
                        client.SendTopic(currentChannel, message.Substring(message.IndexOf(" ") + 1));
                        break;
                    case "UNBAN":
                        client.SendUnban(currentChannel, parts[1]);
                        break;
                    case "WHOIS":
                        client.SendWhois(parts[1]);
                        break;
                    default:
                        Console.WriteLine("Unknown command {0}!", parts[1]);
                        break;
                }
            }
            else
                client.SendChanMsg(currentChannel, message);
        }
    }
}


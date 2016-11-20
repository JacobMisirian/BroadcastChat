using System;
using System.Collections.Generic;

using BroadcastChatServer.Events;
using BroadcastChatServer.Networking;

namespace BroadcastChatServer.Server
{
    public class BroadcastChatServer
    {
        public Dictionary<string, BroadcastChatChannel> Channels { get; private set; }
        public Dictionary<string, BroadcastChatClient> Clients { get; private set; }

        private MessageHandler messageHandler;

        public BroadcastChatServer()
        {
            Channels = new Dictionary<string, BroadcastChatChannel>();
            Clients = new Dictionary<string, BroadcastChatClient>();
            messageHandler = new MessageHandler(this);
        }

        public void Start(int port)
        {

            ConnectionListener listener = new ConnectionListener(port);
            listener.ClientConnected += listener_ClientConnected;
            listener.ClientDisconnected += listener_ClientDisconnected;
            listener.ClientMessageReceived += listener_ClientMessageReceived;

            listener.Start();
        }

        private void listener_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            if (e.Client.Nick != null)
                Clients.Add(e.Client.Nick, e.Client);
        }
        private void listener_ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            if (e.Client.Nick != null)
                Clients.Remove(e.Client.Nick);
            e.Client.ListenThread.Abort();
            e.Client.PingThread.Abort();
            foreach (var channel in e.Client.Channels.Values)
                channel.SendQuit(e.Client, "Client Disconnected");
            e.Client.TcpClient.Close();
            e.Client = null;
        }
        private void listener_ClientMessageReceived(object sender, ClientMessageReceivedEventArgs e)
        {
            if (e.Client != null && e.Message != null)
                messageHandler.HandleMessage(e.Client, e.Message);
        }
    }
}


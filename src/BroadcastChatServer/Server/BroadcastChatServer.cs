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

        public BroadcastChatServer()
        {
            Channels = new Dictionary<string, BroadcastChatChannel>();
            Clients = new Dictionary<string, BroadcastChatClient>();
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
        }
        private void listener_ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
        }
        private void listener_ClientMessageReceived(object sender, ClientMessageReceivedEventArgs e)
        {
        }
    }
}


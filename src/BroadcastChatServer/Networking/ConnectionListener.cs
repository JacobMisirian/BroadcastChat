using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using BroadcastChatServer.Events;

namespace BroadcastChatServer.Networking
{
    public class ConnectionListener
    {
        public const string PING_MESSAGE = "PING";
        public const string PONG_MESSAGE = "PONG";
        public const int PING_TIMEOUT = 10000;

        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;
        public event EventHandler<ClientMessageReceivedEventArgs> ClientMessageReceived;

        private TcpListener listener;

        public ConnectionListener(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            listener.Start();
            new Thread(() => listenForConnectionsThread()).Start();
        }

        private void listenForConnectionsThread()
        {
            while (true)
            {
                BroadcastChatClient client;
                try
                {
                    client = new BroadcastChatClient(listener.AcceptTcpClient());
                    client.ListenThread = new Thread(() => listenForMessagesThread(client));
                    client.PingThread = new Thread(() => pingThread(client));
                    client.ListenThread.Start();
                    client.PingThread.Start();
                    OnClientConnected(new ClientConnectedEventArgs(client));
                }
                catch (IOException)
                {
                }
            }
        }

        private void listenForMessagesThread(BroadcastChatClient client)
        {
            try
            {
                while (true)
                {
                    string message = client.Read();
                    if (message == PONG_MESSAGE)
                        client.Ping = 0;
                    else
                        OnClientMessageReceived(new ClientMessageReceivedEventArgs(client, message));
                }
            }
            catch (IOException)
            {
                OnClientDisconnected(new ClientDisconnectedEventArgs(client));
            }
        }
        private void pingThread(BroadcastChatClient client)
        {
            try
            {
                while (true)
                {
                    client.Send("PING");
                    Thread.Sleep(PING_TIMEOUT);
                }
            }
            catch (IOException)
            {
                OnClientDisconnected(new ClientDisconnectedEventArgs(client));
            }
        }

        protected virtual void OnClientConnected(ClientConnectedEventArgs e)
        {
            var handler = ClientConnected;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnClientDisconnected(ClientDisconnectedEventArgs e)
        {
            var handler = ClientDisconnected;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnClientMessageReceived(ClientMessageReceivedEventArgs e)
        {
            var handler = ClientMessageReceived;
            if (handler != null)
                handler(this, e);
        }
    }
}


using ChatServer2;
using ChatServer2.Net.IO;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks.Dataflow;

namespace Chat
{
    class Program
    {
        static List<Client> _users;
        static TcpListener _listener;
        static void Main(string[] args)
        {
            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
            _listener.Start();
            
            while(true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);
                /* Connection to everyone */
                BroadcastConnection();
            }
        }

        static void BroadcastConnection()
        {
            foreach(var client in _users) 
            { 
                foreach (var user in _users)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteMessage(user.Username);
                    broadcastPacket.WriteMessage(user.UID.ToString());
                    client.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }
        public static void BroadcastMessage(string message)
        {
            foreach (var user in _users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
        }
        public static void BroadcastDisconnect(string uid)
        {
            var disconnectUser = _users.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            _users.Remove(disconnectUser);
            foreach (var user in _users)
            {
                var BroadcastPacket = new PacketBuilder();
                BroadcastPacket.WriteOpCode(10);
                BroadcastPacket.WriteMessage(uid);
                user.ClientSocket.Client.Send(BroadcastPacket.GetPacketBytes());
            }

            BroadcastMessage($"[{disconnectUser.Username}] Disconnected!");
        }

    }
}
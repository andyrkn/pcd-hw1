﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server.Business;

namespace Server.Presentation
{
    public sealed class UdpSocket
    {
        private Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        public UdpSocket()
        {
            Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress,true);
            Socket.Bind(new IPEndPoint(IPAddress.Any, 15001));
        }

        public void Start()
        {
            var buff = new byte[8]; // transfer size __ chunkSize
            var endPoint = (EndPoint)(new IPEndPoint(IPAddress.Any, 0));
            Socket.ReceiveFrom(buff, ref endPoint);

            Socket.Connect(endPoint);

            var transferInfo = Encoding.ASCII.GetString(buff);
            var transferSize = int.Parse(transferInfo.Substring(0, 3));
            var chunkStream = transferInfo.Substring(3,5);
            var chunkSize = int.Parse(chunkStream.Trim('_'));

            var transfer = new Transfer(Socket);
            transfer.StartSending(transferSize, chunkSize, endPoint);
        }
    }
}
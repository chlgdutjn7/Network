using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{

    public class SocketWrapper
    {

        private readonly int MAX_BUFF = 1024;
        private Socket listenSock;
        private List<Socket> connectionSock;
        private Queue<string> messageQue = new Queue<string>();
        private readonly object lockObj = new object();


        public Queue<string> MessageQue => messageQue;


        public SocketWrapper(int port)
        {

            connectionSock = new List<Socket>();
            listenSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            listenSock.Bind(new IPEndPoint(IPAddress.Any, port));
            listenSock.Listen();

            Task.Run(AcceptSocket);
        }


        private async Task AcceptSocket()
        {


            while (true)
            {
                var acceptClient = await listenSock.AcceptAsync();
                if (acceptClient == null) continue;

                Console.WriteLine("클라이언트가 접속했습니다.");
                acceptClient.NoDelay = true;
                connectionSock.Add(acceptClient);
                Task task = AsyncRecv(acceptClient);
            }

        }


        private async Task AsyncRecv(object sock)
        {
            while (true)
            {
                var buf = new byte[MAX_BUFF];
                var Client = (Socket)sock;

                var recvBuf = await Client.ReceiveAsync(buf, SocketFlags.None);
                if (recvBuf > 0)
                {
                    var msg = Encoding.UTF8.GetString(buf, 0, recvBuf);
                    lock (lockObj) messageQue.Enqueue(msg);
                }
            }
        }

        public async void AsyncSend(object Sock, object msg)
        {
            var message = (string)msg;
            var sock = (Socket)Sock;
            var buff = Encoding.UTF8.GetBytes(message, 0, message.Length);
            await sock.SendAsync(buff, SocketFlags.None);
        }


        public void boradcast(object msg)
        {
            foreach (var sock in connectionSock)
            {
                AsyncSend(sock, msg);
            }
        }



        public string PopMessageQue()
        {
            lock (lockObj)
                return messageQue.Dequeue();

        }
    }
}

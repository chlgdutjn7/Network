using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class SocketWrapper
    {

        private readonly int MAX_BUFF = 1024;
        private Socket Sock;
        private Queue<string> MessageQue;


        public Queue<string> messageQue => MessageQue;



        public SocketWrapper(string host, int port)
        {
            MessageQue = new Queue<string>();
            Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Task Connect = AsyncConnectSock(host, port);

        }



        private async Task AsyncConnectSock(string host, int port)
        {
            await Sock.ConnectAsync(host, port);
            Console.WriteLine("서버와 연결이 되었습니다.");

            await asyncRecv();
        }




        public async void AsyncSend(object _msg)
        {
            var msg = (string)_msg;

            var message = Encoding.UTF8.GetBytes(msg, 0, msg.Length);
            await Sock.SendAsync(message, SocketFlags.None);
        }



        public async Task asyncRecv()
        {
            while (true)
            {
                var Buff = new byte[MAX_BUFF];
                try
                {
                    var recvCount = await Sock.ReceiveAsync(Buff, SocketFlags.None);
                    if (recvCount > 0)
                    {
                        var str = Encoding.UTF8.GetString(Buff, 0, recvCount);
                        MessageQue.Enqueue(str);
                    }
                }
                catch (SocketException SE)
                {
                    Console.WriteLine(SE);
                    return;
                }

            }
        }



    }
}

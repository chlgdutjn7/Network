using System;

namespace ConsoleApp1
{
    class Program
    {
        static private SocketWrapper Sock;

        static void Main(string[] args)
        {

            var str = Console.ReadLine();
            Sock = new SocketWrapper(str, 9000);

            while (true)
            {
                str = Console.ReadLine();
                Sock.AsyncSend(str);

                while (Sock.messageQue.Count != 0)
                {
                    Console.WriteLine(Sock.messageQue.Dequeue());
                }
            }

        }
    }
}

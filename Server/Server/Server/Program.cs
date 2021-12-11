using System;


namespace Server
{

    class Program
    {

        static private SocketWrapper SocketWrapper;

        static void Main(string[] args)
        {

            SocketWrapper = new SocketWrapper(9000);
            while (true)
            {
                if (SocketWrapper.MessageQue.Count != 0)
                {
                    var data = SocketWrapper.PopMessageQue();
                    SocketWrapper.boradcast(data);
                    Console.WriteLine(data);
                }
            }
        }
    }
}

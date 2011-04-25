using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ListenForNotify2
{
    class Program
    {
        static void Main(string[] args)
        {
            new UDPDemo();
        }
    }

    class UDPDemo
    {

        private UdpClient udp;

        public UDPDemo()
        {

            Console.WriteLine("Starting Udp Client...");

            IPAddress address = IPAddress.Parse("239.255.255.250");
            IPEndPoint ipep = new IPEndPoint(address, 1900);

            udp = new UdpClient();
            udp.MulticastLoopback = true;
            udp.EnableBroadcast = true;
            udp.Connect( ipep );

            while (true)
            {
               IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
               byte[] bytes = udp.Receive(ref ipep);

                String text = Encoding.UTF8.GetString(bytes);
                Console.WriteLine(text);
            }



        }



    }

}
